using Autowired.Core;
using HttpQuartz.Server.dao.Models;
using Loogn.OrmLite;

namespace HttpQuartz.Server.dao
{
    [AppService]
    public class QRTZ_TRIGGERSDao : BaseDao<QRTZ_TRIGGERS>
    {
        public T SingleTrigger<T>(string schedName, string triggerName, string triggerGroup)
            where T : QRTZ_TRIGGERKEY
        {
            using var db = Open();
            return db.SingleWhere<T>(DictBuilder
                .Assign("SCHED_NAME", schedName)
                .Assign("TRIGGER_NAME", triggerName)
                .Assign("TRIGGER_GROUP", triggerGroup)
            );
        }
    }
}