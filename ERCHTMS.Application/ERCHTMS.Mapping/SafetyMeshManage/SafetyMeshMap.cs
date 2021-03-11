using ERCHTMS.Entity.SafetyMeshManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafetyMeshManage
{
    /// <summary>
    /// 描 述：安全网络
    /// </summary>
    public class SafetyMeshMap : EntityTypeConfiguration<SafetyMeshEntity>
    {
        public SafetyMeshMap()
        {
            #region 表、主键
            //表
            this.ToTable("HD_SAFETYMESH");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
