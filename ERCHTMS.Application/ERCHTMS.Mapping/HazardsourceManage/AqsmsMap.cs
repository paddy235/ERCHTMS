using ERCHTMS.Entity.HazardsourceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HazardsourceManage
{
    /// <summary>
    /// 描 述：化学品安全技术说明书
    /// </summary>
    public class AqsmsMap : EntityTypeConfiguration<AqsmsEntity>
    {
        public AqsmsMap()
        {
            #region 表、主键
            //表
            this.ToTable("HSD_AQSMS");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
