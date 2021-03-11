using ERCHTMS.Entity.EmergencyPlatform;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EmergencyPlatform
{
    /// <summary>
    /// 描 述：应急物资
    /// </summary>
    public class SuppliesMap : EntityTypeConfiguration<SuppliesEntity>
    {
        public SuppliesMap()
        {
            #region 表、主键
            //表
            this.ToTable("MAE_SUPPLIES");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
