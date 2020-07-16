using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HttpQuartz.Client.Models;

namespace HttpQuartz.Client
{
    public class HttpQuartzClient
    {
        private readonly HttpClient httpClient;

        private readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions()
        {
            IgnoreNullValues = true
        };

        public HttpQuartzClient(HttpClient httpClientWithBaseAddress)
        {
            httpClient = httpClientWithBaseAddress;
        }

        /// <summary>
        /// 新增job,成功返回success
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<string> ScheduleJob(TriggerModel model)
        {
            var json = JsonSerializer.Serialize(model, JsonSerializerOptions);
            StringContent content = new StringContent(json, Encoding.UTF8,"application/json");

            var response = await httpClient.PostAsync("/Quartz/ScheduleJob", content);
            return await response.Content.ReadAsStringAsync();
        }


        /// <summary>
        /// 移除job，成功返回success
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<string> UnscheduleJob(TriggerKeyModel model)
        {
            var json = JsonSerializer.Serialize(model, JsonSerializerOptions);
            StringContent content = new StringContent(json, Encoding.UTF8,"application/json");

            var response = await httpClient.PostAsync("/Quartz/UnscheduleJob", content);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> UnscheduleJob(string triggerName, string triggerGroup)
        {
            var key = new TriggerKeyModel(triggerName, triggerGroup);
            return await UnscheduleJob(key);
        }


        /// <summary>
        /// 暂停触发器，成功返回success
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<string> PauseTrigger(TriggerKeyModel model)
        {
            var json = JsonSerializer.Serialize(model, JsonSerializerOptions);
            StringContent content = new StringContent(json, Encoding.UTF8,"application/json");

            var response = await httpClient.PostAsync("/Quartz/PauseTrigger", content);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PauseTrigger(string triggerName, string triggerGroup)
        {
            var key = new TriggerKeyModel(triggerName, triggerGroup);
            return await PauseTrigger(key);
        }

        /// <summary>
        /// 恢复触发器，成功返回success
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<string> ResumeTrigger(TriggerKeyModel model)
        {
            var json = JsonSerializer.Serialize(model, JsonSerializerOptions);
            StringContent content = new StringContent(json, Encoding.UTF8,"application/json");

            var response = await httpClient.PostAsync("/Quartz/ResumeTrigger", content);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> ResumeTrigger(string triggerName, string triggerGroup)
        {
            var key = new TriggerKeyModel(triggerName, triggerGroup);
            return await ResumeTrigger(key);
        }
    }
}