using ERCHTMS.Entity.EnvironmentalManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EnvironmentalManage
{
    /// <summary>
    /// 描 述：环保管理
    /// </summary>
    public class DiscardmonitorMap : EntityTypeConfiguration<DiscardmonitorEntity>
    {
        public DiscardmonitorMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_DISCARDMONITOR");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
