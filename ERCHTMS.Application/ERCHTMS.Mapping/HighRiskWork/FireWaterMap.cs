using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// 描 述：使用消防水
    /// </summary>
    public class FireWaterMap : EntityTypeConfiguration<FireWaterEntity>
    {
        public FireWaterMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_FIREWATER");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
