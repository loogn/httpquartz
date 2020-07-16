using System;
using System.Net.Http;
using System.Threading.Tasks;
using Quartz;
using Quartz.Util;

namespace HttpQuartz.Server.BgServices.QuartzScheduler
{
    public class HttpJob : IJob
    {
        public static string JobName = "httpjob";
        public static string JobGroup = "global";
        public static JobKey JobKey = new JobKey(JobName, JobGroup);

        private readonly IHttpClientFactory httpClientFactory;

        public HttpJob(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                //未启用
                var method = context.Trigger.JobDataMap.TryGetAndReturn("method")?.ToString();
                var url = context.Trigger.JobDataMap.TryGetAndReturn("url")?.ToString();
                var timeoutStr = context.Trigger.JobDataMap.TryGetAndReturn("timeout")?.ToString();
                var expRefire = context.Trigger.JobDataMap.TryGetAndReturn("expRefire")?.ToString();
                var expRemove = context.Trigger.JobDataMap.TryGetAndReturn("expRemove")?.ToString();
                var body = context.Trigger.JobDataMap.TryGetAndReturn("body")?.ToString();
                /*
                 * Trigger.JobDataMap中的值和说明,值全部使用string表示
                 * method:请求方法,GET|POST，默认GET
                 * url:请求地址
                 * timeout:请求超时时间，默认100，单位秒
                 * expRefire:异常时是否立即再次触发，true|false，默认false
                 * expRemove: 异常时是否移除触发器，true|false，默认false
                 */

                //不符合调用条件，直接移除触发器
                if (string.IsNullOrEmpty(url))
                {
                    throw new JobExecutionException("url不能为空")
                    {
                        UnscheduleFiringTrigger = true,
                    };
                }

                if (string.IsNullOrEmpty(method))
                {
                    method = "GET";
                }

                var refireImmediately = "true".Equals(expRefire, StringComparison.Ordinal);
                var unscheduleFiringTrigger = "true".Equals(expRemove, StringComparison.Ordinal);


                var client = httpClientFactory.CreateClient("polly");
                //过期时间，如果不填，默认是100s
                if (!string.IsNullOrEmpty(timeoutStr))
                {
                    if (int.TryParse(timeoutStr, out int secnods))
                    {
                        if (secnods > 0)
                        {
                            client.Timeout = TimeSpan.FromSeconds(secnods);
                        }
                    }
                }

                try
                {
                    HttpResponseMessage responseMessage = null;
                    if ("GET".Equals(method, StringComparison.Ordinal))
                    {
                        responseMessage = await client.GetAsync(url);
                    }
                    else if ("POST".Equals(method, StringComparison.Ordinal))
                    {
                        StringContent stringContent = new StringContent(body ?? "");
                        responseMessage = await client.PostAsync(url, stringContent);
                    }
                    else
                    {
                        throw new JobExecutionException("不支持的method:" + method)
                        {
                            UnscheduleFiringTrigger = true
                        };
                    }

                    //实际情况可以去掉
                    var result = await responseMessage.Content.ReadAsStringAsync();
                    Console.WriteLine("请求结果：" + result);
                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        //客户端出错了
                        throw new JobExecutionException(
                            context.Trigger.Key + "请求发生异常,状态码：" + responseMessage.StatusCode)
                        {
                            UnscheduleFiringTrigger = unscheduleFiringTrigger,
                            RefireImmediately = refireImmediately
                        };
                    }
                }
                catch (JobExecutionException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    //调用出错了
                    throw new JobExecutionException(context.Trigger.Key + "请求发生异常", e)
                    {
                        UnscheduleFiringTrigger = unscheduleFiringTrigger,
                        RefireImmediately = refireImmediately
                    };
                }
            }
            catch (JobExecutionException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new JobExecutionException(context.Trigger.Key + "任务发生异常", e);
            }
        }
    }
}