using ERCHTMS.Entity.EmergencyPlatform;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EmergencyPlatform
{
    /// <summary>
    /// 描 述：应急物资领用申请
    /// </summary>
    public class SuppliesacceptMap : EntityTypeConfiguration<SuppliesacceptEntity>
    {
        public SuppliesacceptMap()
        {
            #region 表、主键
            //表
            this.ToTable("MAE_SUPPLIESACCEPT");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
