using Autowired.Core;
using HttpQuartz.Server.dao.Models;
using Loogn.OrmLite;

namespace HttpQuartz.Server.dao
{
    [AppService]
    public class QRTZ_TRIGGERSDao : BaseDao<QRTZ_TRIGGERS>
    {
      
        public QRTZ_SIMPLE_TRIGGERS SingleSimpleTrigger(string schedName, string triggerName, string triggerGroup)
        {
            using var db = Open();
            return db.SingleWhere<QRTZ_SIMPLE_TRIGGERS>(DictBuilder
                .Assign("SCHED_NAME", schedName)
                .Assign("TRIGGER_NAME", triggerName)
                .Assign("TRIGGER_GROUP", triggerGroup)
            );
        }

        public QRTZ_CRON_TRIGGERS SingleCronTrigger(string schedName, string triggerName, string triggerGroup)
        {
            using var db = Open();
            return db.SingleWhere<QRTZ_CRON_TRIGGERS>(DictBuilder
                .Assign("SCHED_NAME", schedName)
                .Assign("TRIGGER_NAME", triggerName)
                .Assign("TRIGGER_GROUP", triggerGroup)
            );
        }
    }
}