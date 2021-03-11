using ERCHTMS.Entity.ComprehensiveManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.ComprehensiveManage
{
    /// <summary>
    /// 描 述：亮点分享
    /// </summary>
    public class ShareMap : EntityTypeConfiguration<ShareEntity>
    {
        public ShareMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_SHARE");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
