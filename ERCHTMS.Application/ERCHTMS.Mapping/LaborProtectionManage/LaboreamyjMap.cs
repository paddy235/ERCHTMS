using ERCHTMS.Entity.LaborProtectionManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.LaborProtectionManage
{
    /// <summary>
    /// 描 述：劳动防护预警表
    /// </summary>
    public class LaboreamyjMap : EntityTypeConfiguration<LaboreamyjEntity>
    {
        public LaboreamyjMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_LABOREAMYJ");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
