using ERCHTMS.Entity.HiddenTroubleManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：五定安全检查 整改验收表
    /// </summary>
    public class FivesafetycheckauditMap : EntityTypeConfiguration<FivesafetycheckauditEntity>
    {
        public FivesafetycheckauditMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_FIVESAFETYCHECKAUDIT");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
