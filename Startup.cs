using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELearningPortalMSAzureV1.ConfigurationData;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ELearningPortalMSAzureV1
{
    public class Startup
    {
        //23-Jan-Added the codes for configuration, copied from the Muted Video(CRUD Operations in ASP.NET Core MVC).
        public IConfiguration Configuration;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //23-Jan-Copied the codes from the Muted Video(CRUD Operations in ASP.NET Core MVC)
            services.AddMvc();

            //23-Jan For storing in session variables
            services.AddSession();

            //23-Jan-These are Added as AppSetting.json configuration, as it was not working, copied from some portal
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //23-Jan-Commenting the code as per the Muted Video(CRUD Operations in ASP.NET Core MVC)
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});


            //23-Jan-Copying the Codes from the Muted Video(CRUD Operations in ASP.NET Core MVC), for mapping, like in Global.asax.cs
            app.UseStaticFiles();
            //23-Jan Was getting exception in setting the session.
            app.UseSession();
            app.UseMvc(routes =>
            {
                //routes.MapRoute("areaRoute", "{area:exists}/{controller=Learner}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id}");
            });
        }
    }
}
