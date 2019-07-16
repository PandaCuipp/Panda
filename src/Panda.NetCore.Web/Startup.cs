using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Panda.NetCore.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // 此方法由运行时调用。 使用此方法将服务添加到容器
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //此方法由运行时调用。 使用此方法配置HTTP请求管道。
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            appLifetime.ApplicationStarted.Register(() => Console.WriteLine("Started"));
            appLifetime.ApplicationStopping.Register(() => Console.WriteLine("Stopping"));
            appLifetime.ApplicationStopped.Register(() =>
                {
                    Console.WriteLine("Stopped");
                    Console.ReadKey();
                }

            );


            app.Use(next => {
                return async (context) =>
                {
                    var aa = context.Features;
                    Console.WriteLine($"{DateTime.Now}:--{context.Request.GetDisplayUrl()}");
                    await context.Response.WriteAsync("Hello Asp.Net Core!");
                    //await next(context);
                    //appLifetime.StopApplication();
                };
            });

            app.UseMvc();
        }

    }
}
