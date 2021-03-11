using ERCHTMS.Entity.HiddenTroubleManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：五定安全检查
    /// </summary>
    public class FivesafetycheckMap : EntityTypeConfiguration<FivesafetycheckEntity>
    {
        public FivesafetycheckMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_FIVESAFETYCHECK");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
