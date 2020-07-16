namespace HttpQuartz.Server.dao.Models
{
    /// <summary>
    /// 
    /// </summary>
    public partial class QRTZ_CRON_TRIGGERS
    {
        public string SCHED_NAME { get; set; }

        public string TRIGGER_NAME { get; set; }

        public string TRIGGER_GROUP { get; set; }

        public string CRON_EXPRESSION { get; set; }

        public string TIME_ZONE_ID { get; set; }


    }
}
