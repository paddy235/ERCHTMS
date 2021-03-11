using ERCHTMS.Entity.AccidentEvent;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.AccidentEvent
{
    /// <summary>
    /// 描 述：WSSJBG
    /// </summary>
    public class WSSJBGMap : EntityTypeConfiguration<WSSJBGEntity>
    {
        public WSSJBGMap()
        {
            #region 表、主键
            //表
            this.ToTable("AEM_WSSJBG");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
