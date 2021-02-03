using System;
using System.Threading.Tasks;

namespace Crawler
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new CrawlerClient().Run();

            Console.ReadKey();
        }
    }
}
