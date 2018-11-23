using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region 创建注释
/*
* 作    者 ：cuipp
* 创建时间 ：2018-11-21 11:49:00
* 机器名称 ：CUIPP 
* CLR 版本 ：4.0.30319.42000
*/
#endregion
namespace Panda.Common
{
    /// <summary>
    /// 身份证号解析
    /// </summary>
    public static class IDCardHepler
    {
        /// <summary>
        /// 验证身份证号是否合法
        /// </summary>
        /// <param name="str_idcard"></param>
        /// <returns></returns>
        public static bool IsIdCard(this string str_idcard)
        {
            if (string.IsNullOrWhiteSpace(str_idcard))
                return false;
            var correct = false;
            str_idcard = str_idcard.ToUpper();
            var card15 = @"^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$";
            var card18 = @"^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}([0-9]|X)$";
            if (str_idcard.Length == 15)
            {
                correct = System.Text.RegularExpressions.Regex.IsMatch(str_idcard, card15);
            }
            else
            {
                correct = System.Text.RegularExpressions.Regex.IsMatch(str_idcard, card18);
                if (correct == true)
                {
                    correct = CheckIdentification(str_idcard);
                }
            }
            return correct;
            //return System.Text.RegularExpressions.Regex.IsMatch(str_idcard, @"(^\d{18}$)|(^\d{15}$)");
        }
        // <summary>
        /// 解析身份证
        /// </summary>
        /// <param name="pid"></param>
        public static IdentificationDto AnalysisIdentification(string pid)
        {
            IdentificationDto identification = new IdentificationDto();
            if (!string.IsNullOrEmpty(pid) && pid.Length == 18)
            {
                identification.Sex = int.Parse(pid.Substring(16, 1)) % 2 == 0 ? "女" : "男";
                identification.ProvinceCode = pid.Substring(0, 2);
                identification.CityCode = pid.Substring(0, 4);
                identification.DistrictCode = pid.Substring(0, 6);
                identification.Birthday = DateTime.Parse(pid.Substring(6, 8).Insert(6, "-").Insert(4, "-"));
            }
            return identification;
        }

        /// <summary>
        /// 获取年龄
        /// </summary>
        /// <param name="idcardNo"></param>
        /// <returns></returns>
        public static double GetAge(string idcardNo)
        {
            int age = 0;
            if (!string.IsNullOrWhiteSpace(idcardNo))
            {
                try
                {
                    string year, month, day;
                    if (idcardNo.Length == 18)
                    {
                        year = idcardNo.Substring(6, 4);
                        month = idcardNo.Substring(10, 2);
                        day = idcardNo.Substring(12, 2);
                    }
                    else if (idcardNo.Length == 15)
                    {
                        year = string.Concat("19", idcardNo.Substring(6, 2));
                        month = idcardNo.Substring(8, 2);
                        day = idcardNo.Substring(10, 2);
                    }
                    else
                    {
                        return 0;
                    }

                    if (DateTime.TryParse(string.Concat(year, "-", month, "-", day), out DateTime birthday))
                    {
                        int y = DateTime.Today.Year - birthday.Year;
                        DateTime tmp = birthday.AddYears(y);
                        age = (int)(y + ((DateTime.Today - tmp).TotalDays) / (DateTime.DaysInMonth(tmp.Year, 2) + 337));
                    }
                }
                catch { }
            }
            return age;

        }

        /// <summary>
        /// 从身份证号中获取生日
        /// </summary>
        /// <param name="idCardNo"></param>
        /// <returns></returns>
        public static DateTime? GetBirthDay(string idCardNo)
        {
            DateTime? birthday = null;
            
            if (string.IsNullOrWhiteSpace(idCardNo)) { return birthday; }
            if (!IsIdCard(idCardNo)) { return birthday; }

            string year, month, day;
            if (idCardNo.Length == 18)
            {
                year = idCardNo.Substring(6, 4);
                month = idCardNo.Substring(10, 2);
                day = idCardNo.Substring(12, 2);
            }
            else if (idCardNo.Length == 15)
            {
                year = string.Concat("19", idCardNo.Substring(6, 2));
                month = idCardNo.Substring(8, 2);
                day = idCardNo.Substring(10, 2);
            }
            else
            {
                return birthday;
            }

            try
            {
                birthday = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
            }
            catch { }

            return birthday;
        }


        /// <summary>
        /// 校验身份证最后一位字符是否是合法的
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        private static bool CheckIdentification(string pid)
        {
            /*
             * 
             *  1、将前面的身份证号码17位数分别乘以不同的系数。从第一位到第十七位的系数分别为：7－9－10－5－8－4－2－1－6－3－7－9－10－5－8－4－2。
                2、将这17位数字和系数相乘的结果相加。
                3、用加出来和除以11，看余数是多少？
                4、余数只可能有0－1－2－3－4－5－6－7－8－9－10这11个数字。其分别对应的最后一位身份证的号码为1－0－X －9－8－7－6－5－4－3－2。(即余数0对应1，余数1对应0，余数2对应X...)
                5、通过上面得知如果余数是3，就会在身份证的第18位数字上出现的是9。如果对应的数字是2，身份证的最后一位号码就是罗马数字x。
             */
            int[] wi = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1 };
            double sum = 0;
            for (int i = 0; i <= 16; i++)
            {
                var n = pid[i].ToString();
                if (n != "X")
                {
                    sum += int.Parse(n, System.Globalization.NumberStyles.HexNumber) * wi[i];
                }
            }
            string[] ai = new string[] { "1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2" };
            return ai[(int)sum % 11] == pid[17].ToString();
        }

        /// <summary>
        /// 通过17位身份证号，补全最后一位验证码
        /// </summary>
        /// <param name="IdCard"></param>
        /// <returns></returns>
        public static string TransFToS(string IdCard)
        {
            if (string.IsNullOrEmpty(IdCard))
            {
                throw new ArgumentNullException();
            }

            IdCard = IdCard.Trim();

            if (IdCard.Length != 17)
            {
                throw new Exception("位数不正确");
            }

            int[] Weight = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 }; //计算权重
            int Result = 0;

            for (int i = 0; i < Weight.Length; i++)
            {
                Result += Int32.Parse(IdCard[i].ToString()) * Weight[i];
            }

            char[] Code = new char[] { '1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2' }; //最后一位验证码

            return IdCard + Code[Result % 11];
        }
    }

    /// <summary>
    /// 身份证实体类
    /// </summary>
    public class IdentificationDto
    {
        public string ProvinceCode { get; set; }
        public string CityCode { get; set; }
        public string DistrictCode { get; set; }
        public string Sex { get; set; }
        public DateTime Birthday { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
