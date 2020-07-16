using System.Data;
using Autowired.Core;
using Loogn.OrmLite;
using Loogn.OrmLite.MySql;
using MySql.Data.MySqlClient;

namespace HttpQuartz.Server.dao
{
    /// <summary>
    /// 链接配置
    /// </summary>
    [AppSetting("ConnectionStrings")]
    public class ConnectionStringsSection
    {
        public static ConnectionStringsSection Instance;

        static ConnectionStringsSection()
        {
            OrmLite.RegisterProvider(MySqlCommandDialectProvider.Instance);
            // OrmLite.RegisterProvider(SqlServerCommandDialectProvider.Instance);
        }

        public string httpquartz { get; set; }
    }

    [AppService]
    public class BaseDao<TEntity> : AbstractDao<TEntity>
    {
        protected override IDbConnection Open()
        {
            // return new SqlConnection(ConnectionStringsSection.Instance.httpquartz);
            return new MySqlConnection(ConnectionStringsSection.Instance.httpquartz);
        }
    }

    //other base class
}