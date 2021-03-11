using ERCHTMS.Entity.AccidentEvent;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.AccidentEvent
{
    /// <summary>
    /// 描 述：未遂事件报告与调查处理
    /// </summary>
    public class Wssjbg_dealMap : EntityTypeConfiguration<Wssjbg_dealEntity>
    {
        public Wssjbg_dealMap()
        {
            #region 表、主键
            //表
            this.ToTable("AEM_WSSJBG_DEAL");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
