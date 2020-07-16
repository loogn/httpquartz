using System;
using System.Text.Json.Serialization;

namespace HttpQuartz.Client.Models
{
    
    /// <summary>
    /// 简单触发器
    /// </summary>
    [Serializable]
    public class SimpleTriggerInfo
    {
        /// <summary>
        /// 时间间隔
        /// </summary>
        [JsonConverter(typeof(JsonTimeSpanConverter))]
        public TimeSpan RepeatInterval { get; set; }
        

        /// <summary>
        /// 重复次数，-1是无限重复，实际执行次数是repeatCount+1
        /// </summary>
        public int RepeatCount { get; set; } = -1;

        /// <summary>
        /// 失火指令，
        /// IgnoreMisfirePolicy=-1,
        /// FireNow=1,
        /// RescheduleNextWithExistingCount=5
        /// RescheduleNextWithRemainingCount=4,
        /// RescheduleNowWithExistingRepeatCount=2
        /// RescheduleNowWithRemainingRepeatCount=3
        /// 
        /// </summary>
        public int misfireInstruction { get; set; }
    }
}