using ERCHTMS.Entity.LaborProtectionManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.LaborProtectionManage
{
    /// <summary>
    /// 描 述：劳动防护用品
    /// </summary>
    public class LaborprotectionMap : EntityTypeConfiguration<LaborprotectionEntity>
    {
        public LaborprotectionMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_LABORPROTECTION");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
