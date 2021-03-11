using ERCHTMS.Entity.EnvironmentalManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EnvironmentalManage
{
    /// <summary>
    /// 描 述：水质分析记录
    /// </summary>
    public class WaterrecordMap : EntityTypeConfiguration<WaterrecordEntity>
    {
        public WaterrecordMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_WATERRECORD");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
