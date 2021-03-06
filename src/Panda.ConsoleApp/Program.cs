﻿using FastMember;
using MongoDB.Driver;
using Newtonsoft.Json;
using Panda.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Panda.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            #region test

            //Test_GetRandomString2();
            //Test_GenerateGuid();

            //Test_SequentialGuidGenerator();

            //Test_TaskRun();

            //Test_GetMethodName();

            //Test_BankRound();

            //Test_GetRandomInt();

            //Test_ConvertEnum();

            //Test_Default();

            //Test_HashSet();

            //Test_EnumHasFlag();

            //Test_Child();

            //Test_Networkdays();

            //Test_LamadaChange();

            //Test_NullLf0();

            //Test_DeserializeObject();


            //decimal d = 352.5M;
            //Console.WriteLine(Math.Round(d, 0,MidpointRounding.ToEven));

            //Test_TrimEndWith();

            //var d = 1234.56789;
            //var str = d.ToString("N2");
            //Console.WriteLine(str);

            //str = d.ToString("N1");
            //Console.WriteLine(str);

            //str = d.ToString("N0");
            //Console.WriteLine(str);

            //str = d.ToString("N");
            //Console.WriteLine(str);

            //var list = new List<string>();
            //list.Insert(0, "Panda");

            //Console.WriteLine($"{string.Join("、", list)}");

            //var total = 10000;

            //Test_ConstConcat(total);
            //Test_ConstConcat2(total);
            //Test_ConstConcat3(total);

            //Test_JobNameCorrected();

            //List<string> list1 = new List<string>();
            //list1.Add("1111");
            //list1.Add("2222");
            //list1.Add("3333");
            //list1.Add("4444");

            //List<string> list2 = new List<string>();
            //list2.Add("3333");
            //list2.Add("4444");
            //list2.Add("5555");
            //list2.Add("6666");

            ////交集
            //var list11 = list1.Intersect(list2);

            ////差集
            //var list22 = list1.Except(list2);

            ////并集
            //var list33 = list1.Union(list2);

            //Console.WriteLine("交集" + string.Join(" ", list11));
            //Console.WriteLine("差集" + string.Join(" ", list22));
            //Console.WriteLine("并集" + string.Join(" ", list33));


            #endregion

            //Test_JobNameCorrected();

            //Test_Operator();

            //Test_enum();

            //Test_DecimalNullX();

            //Test_ListReturn();

            //Test_GetProperties();

            //Test_UTF8();

            //Test_Action2().Wait();



            int a = 123;
            int b = 456;
            a ^= b;
            b ^= a;
            a ^= b;

            var arr = new int[100];

            Console.WriteLine($"a = {a}");
            Console.WriteLine($"b = {b}");


            Console.ReadKey();
        }

        #region Action、Func:使用的是同一个线程

        /// <summary>
        /// //Action、Func:使用的是同一个线程
        /// </summary>
        static void Test_Action1()  //Action、Func:阻塞主线程 且 使用的是同一个线程
        {
            Console.WriteLine($"主线程 开始:{Thread.CurrentThread.ManagedThreadId}");

            MyFunc((a, b) =>
            {
                Console.WriteLine($"Action 开始:{Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine(a + b);
                Thread.Sleep(3000);
                Console.WriteLine($"Action 结束:{Thread.CurrentThread.ManagedThreadId}");
                return a + b;
            });
            Console.WriteLine($"主线程 结束:{Thread.CurrentThread.ManagedThreadId}");
        }

        /// <summary>
        /// 
        /// </summary>
        static async Task Test_Action2()
        {
            //Task.Run:非阻塞主线程 且 使用的不同一个线程
            // Task.Run().Wait():可以阻塞主线程

            var zhu = 100;
            Console.WriteLine($"主线程 开始:{Thread.CurrentThread.ManagedThreadId}");
            int i = 0;
            while (i < 10)
            {

                i++;
            }
            await Task.Run(() =>
            {
                Console.WriteLine($"Action 开始:{Thread.CurrentThread.ManagedThreadId}");
                //Console.WriteLine(a + b);
                Thread.Sleep(1000);
                Console.WriteLine($"Action 结束:{Thread.CurrentThread.ManagedThreadId}  {zhu}");
            });
            Console.WriteLine($"主线程 结束:{Thread.CurrentThread.ManagedThreadId}");

        }


        static void MyAction(Action<int, int> a)
        {

            a(1, 2);
            //a.Invoke(3, 4);
            //var r = a.BeginInvoke(4, 5, callback: null, null);
            //a.EndInvoke(r);

        }

        static void MyFunc(Func<int, int, int> a)
        {
            //a(1, 2);
            a.Invoke(3, 4);
            //var r = a.BeginInvoke(4, 5, callback: null, null);
            //a.EndInvoke(r);
        }


        static void MyTask(Task task)
        {
            Console.WriteLine($"主线程 开始:{Thread.CurrentThread.ManagedThreadId}");

            Console.WriteLine($"主线程 结束:{Thread.CurrentThread.ManagedThreadId}");
        }
        #endregion

        #region UTF8中文占3个字节,英文和数组1个字节； Unicode都是2个字节

        static void Test_UTF8()
        {
            string s = "情系";

            Console.WriteLine(s);

            var bytes1 = Encoding.UTF8.GetBytes(s);
            var bytes2 = Encoding.Unicode.GetBytes(s);

            Console.WriteLine("UTF8长度：" + Encoding.UTF8.GetBytes(s).Length);
            foreach (var b in bytes1)
            {
                Console.Write(b); Console.Write("  ");
            }
            Console.WriteLine();

            Console.WriteLine("Unicode长度：" + Encoding.Unicode.GetBytes(s).Length);
            foreach (var b in bytes2)
            {
                Console.Write(b); Console.Write("  ");
            }
            Console.WriteLine();

            Console.WriteLine(Encoding.UTF8.GetString(bytes1));
            Console.WriteLine(Encoding.Unicode.GetString(bytes2));

        }

        #endregion

        #region 反射属性速度影响

        static void Test_GetProperties()
        {
            Stopwatch st = new Stopwatch();
            st.Start();
            for (var i = 0; i < 10000; i++)
            {
                TestClass c = new TestClass() { ID = 1, Name = "Panda", Value = 10000 };
                var ty = typeof(TestClass);
                //var props = ty.GetProperties();
                //var nameAttr = props.FirstOrDefault(x => x.Name == "Name");

                //if (nameAttr != null)
                //{
                //    var name = nameAttr.GetValue(c);
                //}

                var accessor = TypeAccessor.Create(ty);

                var o = accessor[c, "Name"];

            }
            st.Stop();
            Console.WriteLine(st.ElapsedMilliseconds);
        }

        #endregion

        #region 引用类型： return 和 as 都不会产生新的地址

        static void Test_ListReturn()
        {
            var data = new List<string>() { "a", "b" };
            var data1 = ChangeList(data);

            var data2 = new List<string>() { "a", "b", "c" };

            Console.WriteLine(data1 == data); // true
            Console.WriteLine(data2 == data); // false


            var children = new List<Child>
            {
                new Child() { Id = 1, EmployeeCode = "a" }
            };
            var children1 = ChangeListObj(children);

            Console.WriteLine(children1 == children); // true

        }

        static List<T> ChangeList<T>(List<T> data)
        {
            var data1 = data as List<string>;
            data1.Add("c");
            data = data1 as List<T>;
            return data;
        }

        static List<T> ChangeListObj<T>(List<T> data)
        {
            var data1 = data as List<Child>;
            data1[0].EmployeeCode = "b";
            data = data1 as List<T>;
            return data;
        }

        #endregion


        #region 可空变量是否可以相乘

        static void Test_DecimalNullX()
        {
            decimal? a = 2;
            decimal b = 2;
            decimal? c = null;
            decimal? d = null;

            Console.WriteLine($"null * 2 null : {c * a}"); // null
            Console.WriteLine($"null * 2      : {c * b}"); // null
            Console.WriteLine($"null * null   : {c * d}"); // null
            Console.WriteLine($"2null * 2     : {a * b}"); // 4
        }

        #endregion

        #region 枚举转化

        static void Test_enum()
        {
            var code = 1;
            object obj = code;
            if (Enum.TryParse(typeof(MyEnym), obj.ToString(), true, out var c))
            {
                Console.WriteLine(c.ToString());
            }

            Console.WriteLine((MyEnym)obj);
        }

        #endregion

        #region 网页爬虫



        #endregion

        #region 运算符重载

        static void Test_Operator()
        {
            TestClass t = 10;

            //operator int(TestClass t)
            int i = t;

            TestClass2 t2 = (TestClass2)t;

            TestClass tt = (TestClass)t2;

            Console.WriteLine(tt.Value);
        }

        #endregion

        #region 去掉字符串末尾的数字

        private static void Test_JobNameCorrected()
        {
            var str = Console.ReadLine();
            while (!string.IsNullOrWhiteSpace(str) && str != "exit")
            {
                Console.WriteLine(JobNameCorrected(str));
                str = Console.ReadLine();
            }
        }

        /// <summary>
        /// 除去末尾的点和数字
        /// </summary>
        /// <returns></returns>
        private static string JobNameCorrected(string jobName)
        {
            if (string.IsNullOrWhiteSpace(jobName))
            {
                return jobName;
            }

            //jobName = jobName.Replace(".", "");

            string regularExpression = @"([0-9]|\.)$";
            Regex rg = new Regex(regularExpression);

            while (rg.IsMatch(jobName))
            {
                jobName = jobName.Substring(0, jobName.Length - 1);
            }

            return jobName.Trim();

        }

        #endregion

        #region 常量拼接

        static void Test_ConstConcat(int totalNum)
        {
            GC.Collect();

            // 定义一个秒表，执行获取执行时间
            Stopwatch st = new Stopwatch();//实例化类
            st.Start();//开始计时

            Console.Write("开始执行，通过变量 + 连接字符串：");
            string result = "a";
            // 定义一个数组
            for (int i = 0; i < totalNum; i++)
            {
                result = result + ".warning";
            }

            //需要统计时间的代码段

            st.Stop();//终止计时
            Console.WriteLine($"总耗时{st.ElapsedMilliseconds.ToString()}毫秒");
        }

        static void Test_ConstConcat2(int totalNum)
        {
            GC.Collect();

            // 定义一个秒表，执行获取执行时间
            Stopwatch st = new Stopwatch();//实例化类
            st.Start();//开始计时

            Console.Write("开始执行，通过常量 + 连接字符串：");

            string s1 = "a";
            // 定义一个数组
            for (int i = 0; i < totalNum; i++)
            {
                s1 += ".warning";
            }

            //需要统计时间的代码段

            st.Stop();//终止计时
            Console.WriteLine($"总耗时{st.ElapsedMilliseconds.ToString()}毫秒");
        }

        static void Test_ConstConcat3(int totalNum)
        {
            GC.Collect();

            // 定义一个秒表，执行获取执行时间
            Stopwatch st = new Stopwatch();//实例化类
            st.Start();//开始计时

            Console.Write("开始执行，通过变量 $ 连接字符串：");
            string result = "a";
            // 定义一个数组
            for (int i = 0; i < totalNum; i++)
            {
                result = $"{result}.warning";
            }

            //需要统计时间的代码段

            st.Stop();//终止计时
            Console.WriteLine($"总耗时{st.ElapsedMilliseconds.ToString()}毫秒");
        }

        #endregion

        #region 去掉末尾指定的字符串测试

        static void Test_TrimEndWith()
        {
            var str = "https://gitlab.fangte.com/";
            Console.WriteLine($"{str}:{TrimEndWith(str, "/")}");

            str = "www.baidu.com";
            Console.WriteLine($"{str}:{TrimEndWith(str, "/")}");

            str = "0123456";
            Console.WriteLine($"{str}:{TrimEndWith(str, "56")}");
        }

        /// <summary>
        /// 清除末尾的 特定字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string TrimEndWith(string str, string c)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            if (str.EndsWith(c))
            {
                str = str.Substring(0, str.Length - c.Length);
            }

            return str;
        }

        #endregion

        #region Json转化为实体

        static void Test_DeserializeObject()
        {
            Console.WriteLine($"3 = {GetValue<int>("3")}");
            Console.WriteLine($"false = {GetValue<int>("false")}");
            Console.WriteLine($"true = {GetValue<int>("true")}");
        }

        /// <summary>
        /// 根据属性名获取属性
        /// </summary>
        public static T GetValue<T>(string value)
        {
            if (value == null)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(value);
        }

        #endregion

        #region null值小于0？

        /// <summary>
        /// null值小于0？
        /// </summary>
        static void Test_NullLf0()
        {
            decimal? d1 = null;

            Console.Write($"null值小于等于0?：");
            Console.WriteLine(d1 <= 0);

            Console.Write($"null值大于0?：");
            Console.WriteLine(d1 > 0);
        }

        #endregion

        #region lamda获取的元素是否可以改动

        static void Test_LamadaChange()
        {
            var list = new List<TestClass>();
            list.Add(new TestClass() { ID = 1, Name = "Panda" });

            Console.WriteLine($"改动前：{list[0].Name}");

            var item = list.FirstOrDefault(x => x.ID == 1);
            if (item != null)
            {
                item.Name = "Ling";
            }
            Console.WriteLine($"改动后：{list[0].Name}");

        }

        #endregion

        #region 工作日计算

        static void Test_Networkdays()
        {
            for (int i = 1; i < 31; i++)
            {
                var d = Networkdays($"2020-01-01", $"2020-01-{i}");
                Console.WriteLine(i.ToString() + " : " + d);
            }
            //var d = Networkdays("2020-01-01", "2020-01-01");
            //Console.WriteLine(d);
        }

        static int Networkdays(string start, string end, int holidays = 0)
        {
            var firstDay = DateTime.Parse(start);
            var lastDay = DateTime.Parse(end);
            var totalDays = (lastDay - firstDay).TotalDays * 5;
            var calcBusinessDays = (int)(1 + (totalDays - (firstDay.DayOfWeek - lastDay.DayOfWeek) * 2) / 7);
            if (lastDay.DayOfWeek == DayOfWeek.Saturday) calcBusinessDays--;
            if (firstDay.DayOfWeek == DayOfWeek.Sunday) calcBusinessDays--;
            return calcBusinessDays - holidays;
        }

        #endregion

        #region 哈希表测试

        static void Test_HashSet()
        {
            var hs = new HashSet<string>();
            hs.Add("Key1");
            hs.Add("key1");
            hs.Add("Key1");


            Console.WriteLine(string.Join(",", hs));
        }

        #endregion

        #region 默认值对比

        static void Test_Default()
        {
            long? creatorId = default;
            if (creatorId == default)
            {
                Console.WriteLine(true);
            }
            else
            {
                Console.WriteLine(false);
            }
        }

        #endregion

        #region 枚举强制转换

        static void Test_ConvertEnum()
        {
            MyEnym my = (MyEnym)10;
            Console.WriteLine(my);
        }

        #endregion

        #region 随机产生数字验证码

        static void Test_GetRandomInt()
        {
            for (int i = 0; i < 100; i++)
            {
                var code = GetRandomInt(4);
                Console.WriteLine(code);
            }
        }

        private static string GetRandomInt(int len)
        {
            StringBuilder code = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                var rand = new Random(System.Guid.NewGuid().GetHashCode());
                var r = rand.Next(0, 9);
                code.Append(r);
            }
            return code.ToString();
        }

        #endregion

        #region 银行家算法验证

        //银行家算法：四舍六入五考虑，五后非零就进一，五后为零看奇偶，五前为偶应舍去，五前为奇要进一
        static void Test_BankRound()
        {
            Console.WriteLine(Convert.ToInt32(2.50));    // 2
            double a = 1;
            double b = 3;
            Console.WriteLine(a / b);
        }

        #endregion

        #region 获取方法全名

        static void Test_GetMethodName()
        {
            var methodBase = System.Reflection.MethodBase.GetCurrentMethod();
            var name = $"{methodBase.DeclaringType?.FullName}.{methodBase.Name}";
            Console.WriteLine(name);
            var name2 = $"{typeof(Program).FullName}.{methodBase.Name}";
            Console.WriteLine(name2);
        }


        #endregion

        #region Task 异步

        static void Test_TaskRun()
        {
            for (var i = 0; i < 1000; i++)
            {
                Task.Run(() =>
                {
                    try
                    {
                        Thread.Sleep(5000);
                        Console.WriteLine($"Task执行完毕{i}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"异常：{i}{ex}");
                    }
                });

                Console.WriteLine($"循环结束{i}");
            }
        }

        #endregion

        #region 短信计费条数

        /// <summary>
        /// 获取短信计费条数
        /// </summary>
        /// <param name="type">短信平台</param>
        /// <param name="smsLength">签名+正文=总长度</param>
        /// <param name="phoneAmount">手机号码数量</param>
        /// <returns></returns>
        private static int GetSmsAmount(int smsLength, int phoneAmount)
        {
            var result = 0;
            //超过70个字，每天短信按67个字计费
            if (smsLength <= 70)
            {
                result = 1;
            }
            else
            {
                result = ((smsLength + 66) / 67);
            }
            //乘以发送手机号的数量
            result = result * phoneAmount;
            return result;
        }

        #endregion

        #region 生成指定位数不重复的编码

        static void Test_SequentialGuidGenerator()
        {
            SequentialGuidGenerator.Instance.DatabaseType = SequentialGuidGenerator.SequentialGuidDatabaseType.MySql;

            for (var i = 0; i < 1000; i++)
            {
                var guid = SequentialGuidGenerator.Instance.Create();

                Console.WriteLine(guid.ToString("N"));

            }
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


        public static void Test_GenerateGuid()
        {
            for (var i = 0; i < 1000; i++)
            {
                var guid = GenerateGuid();
                Console.WriteLine($"{guid:N}");
            }

        }

        public static Guid GenerateGuid()
        {
            byte[] guidArray = Guid.NewGuid().ToByteArray();

            var baseDate = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;
            var days = new TimeSpan(now.Ticks - baseDate.Ticks);
            TimeSpan msecs = now.TimeOfDay;

            byte[] daysArray = BitConverter.GetBytes(days.Days);
            byte[] msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            Array.Copy(daysArray, daysArray.Length - 2, guidArray, guidArray.Length - 6, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, guidArray, guidArray.Length - 4, 4);

            return new Guid(guidArray);
        }

        #endregion

        #region 枚举

        static void Test_EnumHasFlag()
        {
            MyEnym e = MyEnym.E1 | MyEnym.E3;
            //e = (MyEnym)7;
            Console.WriteLine((int)e);
            Console.WriteLine(e.HasFlag(MyEnym.E1));
            Console.WriteLine(e.HasFlag(MyEnym.E2));
            Console.WriteLine(e.HasFlag(MyEnym.E3));
            Console.WriteLine(e.HasFlag(MyEnym.E4));
        }

        #endregion

        #region 子类赋值后是否覆盖了基类的虚属性

        static void Test_Child()
        {
            var child = new Child();
            child.EmployeeCode = "181613";
            child.Id = 100;
            ShowParent(child);
        }

        static void ShowParent<T>(T t) where T : Parent
        {
            Console.WriteLine($"id={t.Id}");
            Console.WriteLine($"EmployeeCode={t.EmployeeCode}");
        }

        #endregion
    }

    enum MyEnym
    {
        E1 = 1,
        E2 = 2,
        E3 = 4,
        E4 = 8,
    }

    #region 子类赋值后是否覆盖了基类的虚属性

    public class Parent
    {
        public virtual string EmployeeCode { get; set; }

        public virtual int Id { get; set; }
    }

    public class Child : Parent
    {
        public override string EmployeeCode { get; set; }

        public override int Id { get; set; }
    }

    #endregion

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

    public class TestClass
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int Value { get; set; }

        //int i = new TestClass();
        public static implicit operator int(TestClass t)
        {
            return t.Value;
        }

        //TestClass t = 10;
        public static implicit operator TestClass(int val)
        {
            return new TestClass() { Value = val };
        }

        //TestClass2 t2 = (TestClass2)t;
        public static explicit operator TestClass2(TestClass val)
        {
            return new TestClass2() { Value2 = val.Value };
        }

        //TestClass t = (TestClass)t2;
        public static explicit operator TestClass(TestClass2 val)
        {
            return new TestClass() { Value = val.Value2 };
        }
    }

    public class TestClass2
    {
        public int Value2 { get; set; }

    }

}
