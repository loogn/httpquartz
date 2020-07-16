using Loogn.OrmLite;

namespace HttpQuartz.Server.dao.Models
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SystemRole
    {
        [OrmLiteField(IsPrimaryKey = true, InsertIgnore = true)]
        public long Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }


    }
}
