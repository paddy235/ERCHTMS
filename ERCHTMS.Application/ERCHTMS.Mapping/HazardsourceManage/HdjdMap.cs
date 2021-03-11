using ERCHTMS.Entity.HazardsourceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HazardsourceManage
{
    /// <summary>
    /// 描 述：登记建档
    /// </summary>
    public class HdjdMap : EntityTypeConfiguration<HdjdEntity>
    {
        public HdjdMap()
        {
            #region 表、主键
            //表
            this.ToTable("HSD_HDJD");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
