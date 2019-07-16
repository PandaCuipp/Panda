using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Panda.Common;

namespace Panda.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {


            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1);
            //毫秒
            var timestamp = Convert.ToInt64(ts.TotalSeconds);

            var argDic = new SortedDictionary<string, string>();
            argDic.Add("Data", "asdfsafs");
            argDic.Add("TimeStamp", timestamp.ToString());

            var param = string.Join("&", argDic.Select(x => $"{x.Key}={x.Value}"));

            Console.WriteLine(param);

            Console.ReadKey();

        }

        static void Test_GetRandomString2()
        {

            //Dictionary<string, string> result = new Dictionary<string, string>();

            //Stopwatch watch = new Stopwatch();
            //watch.Start();
            //for (var i = 0; i < 1000000; i++)
            //{
            //    var res = GetRandomString2(8);

            //    Console.WriteLine(res);
            //    result.Add(res, res);
            //}
            //watch.Stop();
            //Console.WriteLine(result.Count);
            //Console.WriteLine(watch.Elapsed.TotalSeconds);
            long sp = 76927963659;
            while (sp < 99997963659)
            {
                sp = Stopwatch.GetTimestamp();
                Console.WriteLine(sp);
                Thread.Sleep(1000);

                Console.WriteLine("------");
            }
            Console.WriteLine($"{sp}结束时间{DateTime.Now}");


            for (var i = 0; i < 50; i++)
            {
                //Console.WriteLine(DateTime.Now.ToString("HH:mm:ss fffffff"));
                //Console.WriteLine(DateTime.Now.ToString("HH:mm:ss ffffff"));
                //Console.WriteLine(DateTime.Now.ToString("HH:mm:ss fffff"));
                //Console.WriteLine(DateTime.Now.ToString("HH:mm:ss ffff"));
                //Console.WriteLine(DateTime.Now.ToString("HH:mm:ss fff"));
                //TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1);
                //////毫秒
                //var timestamp = Convert.ToInt64(ts.TotalMilliseconds);
                //Console.WriteLine(timestamp);
                //Console.WriteLine(ts.Ticks);

                Console.WriteLine(Stopwatch.GetTimestamp());
                Thread.Sleep(1000 * 60 * 10);

                Console.WriteLine("------");
            }

            //string str = "1561369546555102654929988";

            //Console.WriteLine(str.ToQuotient(36));
            //Console.WriteLine(string.Join(" ", str.ToMultiBinary(36)));

        }

        static void Test_JsonConvert()
        {
            var str = "{\"response\":{\"15666538269\":{\"detail\":{\"area\":[{\"city\":\"淄博\"}],\"province\":\"山东\",\"type\":\"domestic\",\"operator\":\"联通\"},\"location\":\"山东淄博联通\"}},\"responseHeader\":{\"status\":200,\"time\":1562204538884,\"version\":\"1.1.0\"}}";
            var phone = "15666538269";
            try
            {
                var data = JsonConvert.DeserializeObject<PhoneAddressInfo>(str);
                if (data?.Response == null)
                {
                    Console.WriteLine("接口请求失败");
                    return;
                }

                if (!data.Response.Any() || !data.Response.Keys.Contains(phone))
                {
                    Console.WriteLine("未查询到数据");
                    return;
                }

                var info = data.Response[phone];
                if (info == null)
                {
                    Console.WriteLine($"未查询到{phone}归属地");
                    return;
                }

                Console.WriteLine($"{ info.Detail?.Operator}");
                Console.WriteLine($"{ info.Detail?.Province}");
                Console.WriteLine($"{ info.Detail?.Area?.FirstOrDefault()?.City}");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        //static string GetRandomString()
        //{
        //    var str = "abcdefghijklmnopqrstuvwxyz1234567890";
        //    var bt = str.Length;
        //    var len = 8; //位数
        //    var randomLen = 1;//一位 随机位

        //    System.Threading.Thread.Sleep(1);
        //    //获取时间戳
        //    TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1);
        //    //毫秒
        //    var timestamp = Convert.ToInt64(ts.TotalMilliseconds);
        //    var list = new List<int>();
        //    for (var i = 0; i < len - randomLen; i++)
        //    {
        //        var wei = timestamp / Math.Pow(bt, i);
        //        var index = wei % bt;
        //        list.Insert(0, (int)index);
        //    }

        //    for (var j = 0; j < randomLen; j++)
        //    {
        //        //加一个随机位，防止重复
        //        Random rd = new Random(System.Guid.NewGuid().ToString("N").GetHashCode());
        //        list.Add(rd.Next(0, bt));
        //    }

        //    var result = new StringBuilder();
        //    foreach (var index in list)
        //    {
        //        result.Append(str[index]);
        //    }
        //    return result.ToString();
        //}


        static string GetRandomString2(int len)
        {
            var res = GetRandomString();
            while (res.Length < len)
            {
                res = string.Concat(res, GetRandomString());
            }

            return res.Substring(res.Length - len, len);
        }

        static string GetRandomString()
        {
            var str = "0123456789abcdefghijklmnopqrstuvwxyz";
            var bt = str.Length;
            var randomLen = 1;//一位 随机位

            //System.Threading.Thread.Sleep(1);
            //获取时间戳
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1);
            //毫秒
            var datetimeTimestamp = Convert.ToInt64(ts.TotalSeconds);
            var watchTimestamp = Stopwatch.GetTimestamp();
            var timestamp = string.Concat(datetimeTimestamp, watchTimestamp);

            var list = timestamp.ToMultiBinary(bt);

            for (var j = 0; j < randomLen; j++)
            {
                //加一个随机位，防止重复
                Random rd = new Random(System.Guid.NewGuid().ToString("N").GetHashCode());
                list.Add(rd.Next(0, bt));
            }

            var result = new StringBuilder();
            foreach (var index in list)
            {
                result.Append(str[index]);
            }
            return result.ToString();
        }



        static void test()
        {
            MyEnym e = MyEnym.E1 | MyEnym.E3;
            e = (MyEnym)7;
            Console.WriteLine((int)e);
            Console.WriteLine(e.HasFlag(MyEnym.E1));
            Console.WriteLine(e.HasFlag(MyEnym.E2));
            Console.WriteLine(e.HasFlag(MyEnym.E3));
            Console.WriteLine(e.HasFlag(MyEnym.E4));
        }
    }

    enum MyEnym
    {
        E1 = 1,
        E2 = 2,
        E3 = 4,
        E4 = 8,
    }


    #region 

    /// <summary>
    /// 大汉三通查询手机号归属地 接口返回值
    /// </summary>
    public class PhoneAddressInfo
    {
        /// <summary>
        /// 返回数据
        /// </summary>
        public Dictionary<string, PhoneInfo> Response { get; set; }
    }

    /// <summary>
    /// 手机号码信息
    /// </summary>
    public class PhoneInfo
    {
        /// <summary>
        /// 归属地详情
        /// </summary>
        public PhoneInfoDetail Detail { get; set; }

        /// <summary>
        /// 例如：广东深圳移动
        /// </summary>
        public string Location { get; set; }
    }

    /// <summary>
    /// 手机号码归属地详情
    /// </summary>
    public class PhoneInfoDetail
    {
        /// <summary>
        /// 区域信息
        /// </summary>
        public List<AreaInfo> Area { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 运营商
        /// </summary>
        public string Operator { get; set; }
    }

    /// <summary>
    /// 区域信息
    /// </summary>
    public class AreaInfo
    {
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
    }

    #endregion
}
