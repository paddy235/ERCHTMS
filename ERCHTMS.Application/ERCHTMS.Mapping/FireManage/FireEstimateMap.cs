using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FireManage
{
    /// <summary>
    /// 描 述：消防估算指标
    /// </summary>
    public class FireEstimateMap : EntityTypeConfiguration<FireEstimateEntity>
    {
        public FireEstimateMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_FIREESTIMATE");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
