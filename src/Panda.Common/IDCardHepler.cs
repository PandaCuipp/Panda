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
        /// 获取年龄
        /// </summary>
        /// <param name="idCardNo"></param>
        /// <returns></returns>
        public static double GetAge(string idCardNo)
        {
            double age = 0;
            DateTime? birthday = GetBirthDay(idCardNo);
            if (!birthday.HasValue) { return age; }

            int y = DateTime.Today.Year - birthday.Value.Year;
            DateTime tmp = birthday.Value.AddYears(y);
            age = y + ((DateTime.Today - tmp).TotalDays) / (DateTime.DaysInMonth(tmp.Year, 2) + 337);

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
            string year = idCardNo.Substring(6, 4);
            string month = idCardNo.Substring(10, 2);
            string day = idCardNo.Substring(12, 2);

            try
            {
                birthday = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));
            }
            catch { }
            
            return birthday;
        }
    }
}
