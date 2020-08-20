using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Panda.NetCore.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMemoryCache _cache;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,IMemoryCache memoryCache)
        {
            _logger = logger;
            _cache = memoryCache;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("Test")]
        public string Test(string s)
        {
            string cacheKey = "my_string_cacheKey";
            DateTime dateTime;
            // Look for cache key.
            if (!_cache.TryGetValue(cacheKey, out dateTime))
            {
                // Key not in cache, so get data.
                dateTime = DateTime.Now;
                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(3));

                // Save data in cache.
                _cache.Set(cacheKey, dateTime, cacheEntryOptions);
            }

            var msg = $"{DateTime.Now}:{dateTime:s}";
            _logger.LogInformation(msg);
            return msg;
        }
    }
}
