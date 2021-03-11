using ERCHTMS.Entity.HazardsourceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HazardsourceManage
{
    /// <summary>
    /// 描 述：监控内容
    /// </summary>
    public class JkcontentMap : EntityTypeConfiguration<JkcontentEntity>
    {
        public JkcontentMap()
        {
            #region 表、主键
            //表
            this.ToTable("HSD_JKCONTENT");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
