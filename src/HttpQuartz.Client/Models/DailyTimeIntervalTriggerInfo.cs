using System;
using System.Collections.Generic;

namespace HttpQuartz.Client.Models
{
    /// <summary>
    /// 每天时间间隔触发器
    /// </summary>
    [Serializable]
    public class DailyTimeIntervalTriggerInfo
    {
        /// <summary>
        /// 重复间隔
        /// </summary>
        public int RepeatInterval { get; set; }

        /// <summary>
        /// 重复间隔单位
        /// </summary>
        public IntervalUnit RepeatIntervalUnit { get; set; } = IntervalUnit.Minute;

        /// <summary>
        /// 重复次数，-1是无限重复，实际执行次数是repeatCount+1
        /// </summary>
        public int RepeatCount { get; set; } = -1;

        /// <summary>
        /// 每天的开始时间【选填】
        /// </summary>
        public TimeOfDayInfo StartTimeOfDay { get; set; }

        /// <summary>
        /// 每天的结束时间【选填】
        /// </summary>
        public TimeOfDayInfo EndTimeOfDay { get; set; }


        /// <summary>
        /// 星期【选填】
        /// </summary>
        public HashSet<DayOfWeek> DaysOfWeek { get; set; }

        /// <summary>
        /// 失火指令【选填】
        /// IgnoreMisfirePolicy=-1,
        /// FireOnceNow=1,
        /// DoNothing=2
        /// </summary>
        public int MisfireInstruction { get; set; }
    }


}