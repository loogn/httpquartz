using System.IO;
using HttpQuartz.Server.dao;
using HttpQuartz.Server.Models;
using HttpQuartz.Server.Tools.Auth;
using HttpQuartz.Server.Tools.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HttpQuartz.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<MyActionFilterAttribute>();
                options.Filters.Add<MyExceptionFilterAttribute>();
                // options.Filters.Add<MyAuthorizeFilter>();
            });

            //添加认证和授权
            services.AddAuth();

            services.AddAppServices(Configuration);
            services.AddHttpClient();

            services.AddHttpContextAccessor();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            //任务调用服务
            services.AddQuartzScheduler();
            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(Path.GetFullPath("./logs/")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            AppSettings settings, ConnectionStringsSection conn)
        {
            ConnectionStringsSection.Instance = conn;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor
            });
            app.UseRouting();
            app.UseAuthentication(); //认证
            app.UseAuthorization(); //授权

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}");
            });
        }
    }
}