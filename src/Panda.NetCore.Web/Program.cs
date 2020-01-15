using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Panda.NetCore.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
            var host = new HostBuilder().UseEnvironment(Microsoft.Extensions.Hosting.EnvironmentName.Development);
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        //public static async Task Main()
        //{
        //    await new WebHostBuilder().UseH
        //        .UseHttpListener()
        //        .Configure(app => app
        //            .Use(FooMiddleware)
        //            .Use(BarMiddleware)
        //            .Use(BazMiddleware))
        //        .Build()
        //        .StartAsync();
        //}

        //public static RequestDelegate FooMiddleware(RequestDelegate next)
        //=> async context => {
        //    await context.Response.WriteAsync("Foo=>");
        //    await next(context);
        //};

        //public static RequestDelegate BarMiddleware(RequestDelegate next)
        //=> async context => {
        //    await context.Response.WriteAsync("Bar=>");

        //    await next(context);
        //};

        //public static RequestDelegate BazMiddleware(RequestDelegate next)
        //=> context => context.Response.WriteAsync("Baz");
    }
}
