namespace HttpQuartz.Server.dao.Models
{
    /// <summary>
    /// 
    /// </summary>
    public partial class QRTZ_SIMPLE_TRIGGERS
    {
        public string SCHED_NAME { get; set; }

        public string TRIGGER_NAME { get; set; }

        public string TRIGGER_GROUP { get; set; }

        public long REPEAT_COUNT { get; set; }

        public long REPEAT_INTERVAL { get; set; }

        public long TIMES_TRIGGERED { get; set; }


    }
}
