using ERCHTMS.Entity.HazardsourceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HazardsourceManage
{
    /// <summary>
    /// 描 述：危险源清单
    /// </summary>
    public class Hisrelationhd_qdMap : EntityTypeConfiguration<Hisrelationhd_qdEntity>
    {
        public Hisrelationhd_qdMap()
        {
            #region 表、主键
            //表
            this.ToTable("HSD_HISRELATIONHD_QD");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
