using System;
using Loogn.OrmLite;

namespace HttpQuartz.Server.dao.Models
{
    /// <summary>
    /// 简单属性触发器
    /// </summary>
    public partial class QRTZ_SIMPROP_TRIGGERS:QRTZ_TRIGGERKEY
    {
        public string SCHED_NAME { get; set; }

        public string TRIGGER_NAME { get; set; }

        public string TRIGGER_GROUP { get; set; }

        /// <summary>
        /// 间隔单位 
        /// </summary>
        public string STR_PROP_1 { get; set; }

        /// <summary>
        /// 周天  0,1,2,3,4,5,6
        /// </summary>
        public string STR_PROP_2 { get; set; }

        /// <summary>
        /// 每天时间范围 时,分,秒,时,分,秒
        /// </summary>
        public string STR_PROP_3 { get; set; }

        /// <summary>
        /// 间隔量
        /// </summary>
        public int INT_PROP_1 { get; set; }

        public int INT_PROP_2 { get; set; }

        /// <summary>
        /// 重复量，-1为永久重复
        /// </summary>
        public long LONG_PROP_1 { get; set; }

        public long LONG_PROP_2 { get; set; }

        public decimal DEC_PROP_1 { get; set; }

        public decimal DEC_PROP_2 { get; set; }

        /// <summary>
        /// PreserveHourOfDayAcrossDaylightSavings
        /// </summary>
        public bool BOOL_PROP_1 { get; set; }

        /// <summary>
        /// SkipDayIfHourDoesNotExist
        /// </summary>
        public bool BOOL_PROP_2 { get; set; }

        public string TIME_ZONE_ID { get; set; }


    }
}
