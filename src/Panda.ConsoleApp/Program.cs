using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Panda.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //var dt = new DateTime(2018, 1, 1);
            //var utc = dt.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            ////string result = string.Format("$lt:ISODate('{0}')", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));

            //string ParentIds = string.Empty;
            ////string result = string.Concat(ParentIds, (string.IsNullOrWhiteSpace(ParentIds) ? "" : ","), "Panda");

            //string result = string.Join(",", ParentIds, "Panda"); 

            //Console.WriteLine(result);

            //test();

            List<string> list = null;
            if (list.Any())
            {
                Console.WriteLine(true);
            }
            else
            {
                Console.WriteLine(false);
            }
            Console.ReadKey();

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
}
