﻿using System.Collections.Generic;
using Autowired.Core;
using HttpQuartz.Server.dao.Models;
using Loogn.OrmLite;

namespace HttpQuartz.Server.dao
{
    [AppService]
    public class SystemUser_RoleDao : BaseDao<SystemUser_Role>
    {

        public List<long> GetEmployeeIds(long roleId)
        {
            using (var db = Open())
            {
                return db.Column<long>("select SystemUserId from SystemUser_Role where SystemRoleId=" + roleId);
            }
        }

        public List<long> GetRoleIds(long employeeId)
        {
            using (var db = Open())
            {
                return db.Column<long>("select SystemRoleId from SystemUser_Role where SystemUserId=" + employeeId);
            }
        }

        public int DeleteByRoleId(long roleId)
        {
            return DeleteWhere("SystemRoleId", roleId);

        }

        public int DeleteByUserId(long userId)
        {
            return DeleteWhere("SystemUserId", userId);
        }

    }
}
