using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Quartz;

namespace HttpQuartz.Server.BgServices.QuartzScheduler
{
    public class QuartzSchedulerService : IHostedService
    {
        private readonly IScheduler scheduler;

        public QuartzSchedulerService(IScheduler scheduler)
        {
            this.scheduler = scheduler;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await scheduler.Start(cancellationToken);
            
            if (!await scheduler.CheckExists(HttpJob.JobKey))
            {
                //加入httpjob            
                var jobDetail = JobBuilder.Create<HttpJob>()
                    .WithIdentity(HttpJob.JobKey)
                    .RequestRecovery()
                    .StoreDurably() //持久存储，没有trigger也不能删除
                    .Build();
                await scheduler.AddJob(jobDetail, false);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return scheduler.Shutdown(cancellationToken);
        }
    }
}