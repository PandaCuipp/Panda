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
                    string data = string.Empty;
                    foreach (var zi in collect)
                    {
                        data += zi.ToString();
                    }
                    list.Add(data.Substring(1, data.Length - 2));
                }
            }
            return list;
        }
    }
}
