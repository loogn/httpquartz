using System;
using HttpQuartz.Server.BgServices.QuartzScheduler;
using Polly;
using Quartz.Impl;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class QuartzSchedulerServiceCollectionExtensions
    {
        public static IServiceCollection AddQuartzScheduler(this IServiceCollection services)
        {
            //httpfactory
            services.AddHttpClient("polly",
                    (client =>
                    {
                        client.DefaultRequestHeaders.Add("x-requested-with", "XMLHttpRequest");
                    }))
                .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(3),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10)
                }));


            //任务管理服务
            // services.AddSingleton<DelayTaskManager>();
            // services.AddSingleton<TimedTaskManager>();

            services.AddSingleton<HttpJob>();
            services.AddSingleton((ServiceProvider) =>
            {
                var factory = new StdSchedulerFactory();
                var scheduler = factory.GetScheduler().GetAwaiter().GetResult();
                scheduler.JobFactory = new HttpJobFactory(ServiceProvider);
                // LogProvider.SetCurrentLogProvider(new SerilogLogProvider());
                return scheduler;
            });

            //托管服务
            services.AddHostedService<QuartzSchedulerService>();

            return services;
        }
    }
}