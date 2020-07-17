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

        public string GetTime(long? ticks)
        {
            if (ticks == null) return "";
            var dt = new DateTimeOffset(ticks.Value, TimeSpan.Zero);
            return dt.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
        }

        public string GetMisFireINSTR()
        {
            if (MISFIRE_INSTR == null || MISFIRE_INSTR == 0)
            {
                return "Smart";
            }

            if (MISFIRE_INSTR == -1)
            {
                return "IgnoreMisfirePolicy";
            }

            if (TRIGGER_TYPE == "SIMPLE")
            {
                switch (MISFIRE_INSTR.Value)
                {
                    case 1:
                        return "FireNow";
                    case 2:
                        return "RescheduleNowWithExistingRepeatCount";
                    case 3:
                        return "RescheduleNowWithRemainingRepeatCount";
                    case 4:
                        return "RescheduleNextWithRemainingCount";
                    case 5:
                        return "RescheduleNextWithExistingCount";
                    default:
                        return "Smart";
                }
            }
            else
            {
                switch (MISFIRE_INSTR.Value)
                {
                    case 1:
                        return "FireOnceNow";
                    case 2:
                        return "DoNothing";
                    default:
                        return "Smart";
                }
            }
        }

        //"失火指令 -1-IgnoreMisfirePolicy忽略其他指令，有资源就出发所有MisFire任务,1-FireOnceNow马上调度并继续(默认),2-DoNothing  int"
    }
}