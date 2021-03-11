using ERCHTMS.Entity.HazardsourceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HazardsourceManage
{
    /// <summary>
    /// 描 述：历史记录
    /// </summary>
    public class HisrelationhdMap : EntityTypeConfiguration<HisrelationhdEntity>
    {
        public HisrelationhdMap()
        {
            #region 表、主键
            //表
            this.ToTable("HSD_HISRELATIONHD");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
