using ERCHTMS.Entity.HazardsourceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HazardsourceManage
{
    /// <summary>
    /// 描 述：监控
    /// </summary>
    public class JkjcMap : EntityTypeConfiguration<JkjcEntity>
    {
        public JkjcMap()
        {
            #region 表、主键
            //表
            this.ToTable("HSD_JKJC");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
