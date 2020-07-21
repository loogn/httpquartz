namespace HttpQuartz.Server.dao.Models
{
    public interface QRTZ_TRIGGERKEY
    {
        public string SCHED_NAME { get; set; }

        public string TRIGGER_NAME { get; set; }

        public string TRIGGER_GROUP { get; set; }
    }
}