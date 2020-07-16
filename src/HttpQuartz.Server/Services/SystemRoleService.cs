using System.Collections.Generic;
using Autowired.Core;
using CoreHelper;
using HttpQuartz.Server.dao;
using HttpQuartz.Server.dao.Models;
using Loogn.OrmLite;

namespace HttpQuartz.Server.Services
{
    [AppService]
    public class SystemRoleService
    {
        [Autowired] SystemRoleDao systemRoleDao;
        [Autowired] SystemUser_RoleDao systemUser_RoleDao;
        [Autowired] SystemRole_ResDao systemRole_ResDao;

        public SystemRoleService(AutowiredService autowiredService)
        {
            autowiredService.Autowired(this);
        }


        public ResultObject Edit(SystemRole m)
        {
            if (string.IsNullOrEmpty(m.Name))
            {
                return new ResultObject("名称不能为空");
            }

            var flag = 0L;
            if (m.Id > 0)
            {
                flag = systemRoleDao.Update(m);
            }
            else
            {
                flag = systemRoleDao.Insert(m);
            }

            return new ResultObject(flag > 0);
        }

        public SystemRole SingleById(long id)
        {
            return systemRoleDao.SingleById(id);
        }

        public List<SystemRole> SelectAll(string name)
        {
            name = SqlInjection.Filter(name);
            if (string.IsNullOrEmpty(name))
                return systemRoleDao.SelectAll();
            else
                return systemRoleDao.Select($"name like '%{name}%'", null);
        }

        public ResultObject DeleteRole(long roleId)
        {
            var flag = systemRoleDao.DeleteById(roleId);
            if (flag > 0)
            {
                systemUser_RoleDao.DeleteByRoleId(roleId);
                systemRole_ResDao.DeleteWhere("SystemRoleId",roleId);
            }

            return new ResultObject(flag);
        }
    }
}