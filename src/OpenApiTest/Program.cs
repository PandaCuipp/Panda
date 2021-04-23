using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenApiTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var http = new HttpClient();
            var client = new swaggerClient("http://localhost:5000/", http);
            var result = await client.WeatherForecastAsync(1);
            foreach (var item in result)
            {
                Console.WriteLine($"{item.Date}, {item.TemperatureF}, {item.Summary}");
            }
            Console.WriteLine("Hello World!");

            var result2 = await client.GetLenAsync(3);
            foreach (var item in result2)
            {
                Console.WriteLine($"{item.Date}, {item.TemperatureF}, {item.Summary}");
            }
            Console.WriteLine("Hello World!");
        }
    }
}
