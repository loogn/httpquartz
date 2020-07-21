using System;
using System.Text;
using Autowired.Core;
using CoreHelper;
using HttpQuartz.Server.dao;
using HttpQuartz.Server.dao.Models;
using Loogn.OrmLite;

namespace HttpQuartz.Server.Services
{
    [AppService]
    public class QRTZ_TRIGGERSService
    {
        [Autowired] private QRTZ_TRIGGERSDao qrtzTriggersDao;

        public QRTZ_TRIGGERSService(AutowiredService autowiredService)
        {
            autowiredService.Autowired(this);
        }

        public OrmLitePageResult<QRTZ_TRIGGERS> SelectPage(string sched, string name, string group, string state,
            string type,
            int pageIndex, int pageSize)
        {
            StringBuilder sb = new StringBuilder("SCHED_NAME=@sched");
            var ps = DictBuilder.Assign("sched", sched);

            if (!string.IsNullOrEmpty(name))
            {
                sb.AppendFormat(" and TRIGGER_NAME=@name");
                ps.Assign("name", name);
            }

            if (!string.IsNullOrEmpty(group))
            {
                sb.AppendFormat(" and TRIGGER_GROUP=@group");
                ps.Assign("group", group);
            }

            if (!string.IsNullOrEmpty(state))
            {
                sb.AppendFormat(" and TRIGGER_STATE=@state");
                ps.Assign("state", state);
            }

            if (!string.IsNullOrEmpty(type))
            {
                sb.AppendFormat(" and TRIGGER_TYPE=@type");
                ps.Assign("type", type);
            }

            return qrtzTriggersDao.SelectPage(new OrmLitePageFactor
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Conditions = sb.ToString(),
                Params = ps,
                OrderBy = "SCHED_NAME,TRIGGER_NAME,TRIGGER_GROUP"
            });
        }


        public QRTZ_TRIGGERS SingleTrigger(string sched, string name, string group)
        {
            return qrtzTriggersDao.SingleTrigger<QRTZ_TRIGGERS>(sched, name, group);
        }

        public QRTZ_CRON_TRIGGERS SingleCronTrigger(string sched, string name, string group)
        {
            return qrtzTriggersDao.SingleTrigger<QRTZ_CRON_TRIGGERS>(sched, name, group);
        }

        public QRTZ_SIMPLE_TRIGGERS SingleSimpleTrigger(string sched, string name, string group)
        {
            return qrtzTriggersDao.SingleTrigger<QRTZ_SIMPLE_TRIGGERS>(sched, name, group);
        }

        public QRTZ_SIMPROP_TRIGGERS SinglePropTrigger(string sched, string name, string group)
        {
            return qrtzTriggersDao.SingleTrigger<QRTZ_SIMPROP_TRIGGERS>(sched, name, group);
        }
    }
}