using ERCHTMS.Entity.EmergencyPlatform;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EmergencyPlatform
{
    /// <summary>
    /// 描 述：应急物资检查详情
    /// </summary>
    public class SuppliesCheckDetailMap : EntityTypeConfiguration<SuppliesCheckDetailEntity>
    {
        public SuppliesCheckDetailMap()
        {
            #region 表、主键
            //表
            this.ToTable("MAE_SUPPLIESCHECKDETAIL");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
