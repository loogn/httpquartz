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
    }

}