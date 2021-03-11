using ERCHTMS.Entity.AccidentEvent;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.AccidentEvent
{
    /// <summary>
    /// 描 述：事故事件调查处理
    /// </summary>
    public class Bulletin_dealMap : EntityTypeConfiguration<Bulletin_dealEntity>
    {
        public Bulletin_dealMap()
        {
            #region 表、主键
            //表
            this.ToTable("AEM_BULLETIN_DEAL");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
