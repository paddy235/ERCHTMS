using ERCHTMS.Entity.AccidentEvent;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.AccidentEvent
{
    /// <summary>
    /// 描 述：事故事件快报
    /// </summary>
    public class BulletinMap : EntityTypeConfiguration<BulletinEntity>
    {
        public BulletinMap()
        {
            #region 表、主键
            //表
            this.ToTable("AEM_BULLETIN");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
