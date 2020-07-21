using System;

namespace HttpQuartz.Client.Models
{
    /// <summary>
    /// Cron表达式触发器
    /// </summary>
    [Serializable]
    public class CronTriggerInfo
    {
        /// <summary>
        /// cron表达式
        /// </summary>
        public string CronExpression { get; set; }

        /// <summary>
        /// 失火指令，
        /// IgnoreMisfirePolicy=-1,
        /// FireOnceNow=1,
        /// DoNothing=2
        /// </summary>
        public int MisfireInstruction { get; set; }
        
        /// <summary>
        /// 时区ID,默认不填是本地时区，即PRC
        /// </summary>
        public string TimeZoneId { get; set; }
    }

}