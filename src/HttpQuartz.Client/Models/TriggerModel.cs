using System;
using System.Text.Json;

namespace HttpQuartz.Client.Models
{
    [Serializable]
    public class TriggerModel
    {
        /// <summary>
        /// key
        /// </summary>
        public TriggerKeyModel Key { get; set; }

        /// <summary>
        /// 如果key相同，是否重新加入调度器
        /// </summary>
        public bool Reschedule { get; set; } = true;

        /// <summary>
        /// 权重
        /// </summary>
        public int Priority { get; set; } = 5;

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTimeOffset StartTime { get; set; } = DateTimeOffset.UtcNow;

        /// <summary>
        /// 结束时间【选填】
        /// </summary>
        public DateTimeOffset? EndTime { get; set; }

        /// <summary>
        /// 请求job数据
        /// </summary>
        public JobDataInfo JobData { get; set; }

        /// <summary>
        /// 简单触发器【选填】
        /// </summary>
        public SimpleTriggerInfo SimpleTrigger { get; set; }

        /// <summary>
        /// Cron表达式触发器【选填】
        /// </summary>
        public CronTriggerInfo CronTrigger { get; set; }

        /// <summary>
        /// 日历间隔触发器【选填】
        /// </summary>
        public CalendarIntervalTriggerInfo CalendarIntervalTrigger { get; set; }

        /// <summary>
        /// 每天时间间隔触发器【选填】
        /// </summary>
        public DailyTimeIntervalTriggerInfo DailyTimeIntervalTrigger { get; set; }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions()
            {
                WriteIndented = true
            });
        }
    }
}