using Bogus;
using System;
using System.Diagnostics;
using System.Linq;

namespace EFOracle
{
    class Program
    {
        static void Main(string[] args)
        {
            var gengerator = new Faker<Order>()
            //.StrictMode(true)
            .RuleFor(x => x.Id, a => a.Random.Guid().ToString("N"))
            .RuleFor(x => x.Name, a => a.Name.FullName())
            .RuleFor(x => x.Qty, a => a.Random.Int(1, 100));

            var list = gengerator.GenerateForever().Take(10000).ToList();

            var list2 = gengerator.GenerateForever().Take(10000).ToList();
            list2.AddRange(list.Take(10));

            var watch = new Stopwatch();
            watch.Start();

            var sum = 0;
            foreach (var item in list)
            {
                //var item2 = list2.FirstOrDefault(x => x.Id == item.Id && x.Qty == item.Qty);
                //if (item2 != null)
                //{
                sum += item.Qty;
                //}
            }

            sum = list.Sum(q => q.Qty);
            watch.Stop();
            Console.WriteLine("FirstOrDefault：" + watch.ElapsedMilliseconds);
            Console.WriteLine(sum);

            watch.Restart();
            var dic = list2.ToDictionary(x => new { x.Id, x.Qty }, a => a);
            sum = 0;
            foreach (var item in list)
            {
                if (dic.TryGetValue(new { item.Id, item.Qty }, out var item2))
                {
                    sum += item2.Qty;
                }
            }
            watch.Stop();
            Console.WriteLine("ToDictionary：" + watch.ElapsedMilliseconds);
            Console.WriteLine(sum);

            Console.ReadKey();
        }


    }

    public class Order
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Qty { get; set; }

    }
}
