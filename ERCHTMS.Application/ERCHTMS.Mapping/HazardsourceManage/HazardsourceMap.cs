using ERCHTMS.Entity.HazardsourceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HazardsourceManage
{
    /// <summary>
    /// 描 述：危险源辨识评估
    /// </summary>
    public class HazardsourceMap : EntityTypeConfiguration<HazardsourceEntity>
    {
        public HazardsourceMap()
        {
            #region 表、主键
            //表
            this.ToTable("HSD_HAZARDSOURCE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
