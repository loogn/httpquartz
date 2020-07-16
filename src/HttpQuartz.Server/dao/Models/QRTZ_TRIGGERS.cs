using System;

namespace HttpQuartz.Server.dao.Models
{
    /// <summary>
    /// 
    /// </summary>
    public partial class QRTZ_TRIGGERS
    {
        public string SCHED_NAME { get; set; }

        public string TRIGGER_NAME { get; set; }

        public string TRIGGER_GROUP { get; set; }

        // public string JOB_NAME { get; set; }
        //
        // public string JOB_GROUP { get; set; }

        // public string DESCRIPTION { get; set; }

        public long? NEXT_FIRE_TIME { get; set; }

        public long? PREV_FIRE_TIME { get; set; }

        public int? PRIORITY { get; set; }

        public string TRIGGER_STATE { get; set; }

        public string TRIGGER_TYPE { get; set; }

        public long START_TIME { get; set; }

        public long? END_TIME { get; set; }

        public string CALENDAR_NAME { get; set; }

        public Int16? MISFIRE_INSTR { get; set; }

        public byte[] JOB_DATA { get; set; }
    }
}