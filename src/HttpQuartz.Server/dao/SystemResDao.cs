using Autowired.Core;
using HttpQuartz.Server.dao.Models;
using Loogn.OrmLite;

namespace HttpQuartz.Server.dao
{
    [AppService]
    public class SystemResDao : BaseDao<SystemRes>
    {
        public OrmLitePageResult<SystemRes> SelectList(string name, int pageIndex, int pageSize)
        {
            name = SqlInjection.Filter(name);
            var condition = "";
            if (!string.IsNullOrEmpty(name))
            {
                condition = " Name LIKE '%" + name + "%'";
            }

            return SelectPage(new OrmLitePageFactor
            {
                Conditions = condition,
                OrderBy = "ID asc",
                PageIndex = pageIndex,
                PageSize = pageSize,
            });
        }

    }
}
