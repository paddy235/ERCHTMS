using ERCHTMS.Entity.EmergencyPlatform;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EmergencyPlatform
{
    /// <summary>
    /// 描 述：应急物资领用申请详细
    /// </summary>
    public class SuppliesAcceptDetailMap : EntityTypeConfiguration<SuppliesAcceptDetailEntity>
    {
        public SuppliesAcceptDetailMap()
        {
            #region 表、主键
            //表
            this.ToTable("MAE_SUPPLIESACCEPT_DETAIL");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
