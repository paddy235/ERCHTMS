using ERCHTMS.Entity.Observerecord;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.Observerecord
{
    /// <summary>
    /// 描 述：观察记录安全行为
    /// </summary>
    public class ObservesafetyMap : EntityTypeConfiguration<ObservesafetyEntity>
    {
        public ObservesafetyMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_OBSERVESAFETY");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}