using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

#region 创建注释
/*
* 作    者 ：cuipp
* 创建时间 ：2018-12-03 10:50:27
* 机器名称 ：CUIPP 
* CLR 版本 ：4.0.30319.42000
*/
#endregion
namespace Panda.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringHelper
    {
        public static Guid GetGUID()
        {
            return Guid.NewGuid();
        }

        /// <summary>
        /// 获取手机号号码归属地--通过纯真查询
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static List<string> GetPhoneAddress(string phone)
        {
            var res = HttpHelper.Post(new HttpRequestDto()
            {
                Url = "http://www.cz88.net/tools/mobile.php",
                Data = "in_id=" + phone,
            });

            Regex reg = new Regex("<div class=\"(nane|data)\">\\w+</div>");
            MatchCollection matchCollect = reg.Matches(res);

            List<string> list = new List<string>();
            foreach (var item in matchCollect)
            {
                //list.Add(item.ToString());
                Regex reg2 = new Regex(">\\w+<");
                var collect = reg2.Matches(item.ToString());
                if (collect != null)
                {
                    string data = String.Empty;
                    foreach (var zi in collect)
                    {
                        data += zi.ToString();
                    }
                    list.Add(data.Substring(1, data.Length - 2));
                }
            }
            return list;
        }

        /// <summary>
        /// 大数取模运算
        /// </summary>
        /// <param name="number">被除数</param>
        /// <param name="divisor">除数</param>
        public static int ToMod(this string number, int divisor)
        {
            var mod = 0;//余数
            foreach (var t in number)
            {
                var c = string.Concat(mod, t); //被除数
                
                if (int.TryParse(c, out var z))
                {
                    mod = (z % divisor);
                }
                else
                {
                    throw new Exception($"{number}转化整数失败");
                }
            }
            return mod;
        }
        /// <summary>
        /// 大数取商运算
        /// </summary>
        /// <param name="number">被除数</param>
        /// <param name="divisor">除数</param>
        public static string ToQuotient(this string number, long divisor)
        {
            StringBuilder quoList = new StringBuilder();
            double mod = 0;//余数
            foreach (var t in number)
            {
                var c = string.Concat(mod, t); //被除数
                if (double.TryParse(c, out var z))
                {
                    mod = (z % divisor);
                    var quo = Math.Floor(z / divisor);
                    if (quo > 0 || quoList.Length > 0)
                    {
                        quoList.Append(quo);
                    }
                }
                else
                {
                    throw new Exception("转化整数失败");
                }
            }
            return quoList.Length <= 0 ? "0" : quoList.ToString();
        }

        /// <summary>
        /// 大数运算之--进制转化，10进制转化为任一进制，余数舍弃
        /// </summary>
        /// <param name="number">原数据</param>
        /// <param name="n">n进制</param>
        /// <returns></returns>
        public static List<int> ToMultiBinary(this string number ,int n)
        {
            var list = new List<int>();

            var wei = number;
            var i = 0;
            while (wei != "0")
            {
                var index = wei.ToMod(n);
                list.Insert(0, index);
                wei = wei.ToQuotient(n);
                i++;
            }

            return list;
        }
    }
}
