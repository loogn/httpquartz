using System;

namespace HttpQuartz.Client.Models
{
    /// <summary>
    /// 日历间隔触发器
    /// </summary>
    [Serializable]
    public class CalendarIntervalTriggerInfo
    {
        /// <summary>
        /// 重复间隔
        /// </summary>
        public int RepeatInterval { get; set; }

        /// <summary>
        /// 重复间隔单位
        /// </summary>
        public IntervalUnit RepeatIntervalUnit { get; set; }

        /// <summary>
        /// 失火指令，
        /// IgnoreMisfirePolicy=-1,
        /// FireOnceNow=1,
        /// DoNothing=2
        /// </summary>
        public int MisfireInstruction { get; set; }

        public bool PreserveHourOfDayAcrossDaylightSavings { get; set; }
        public bool SkipDayIfHourDoesNotExist { get; set; }
        
        /// <summary>
        /// 时区ID,默认不填是本地时区，即PRC
        /// </summary>
        public string TimeZoneId { get; set; }
    }
}