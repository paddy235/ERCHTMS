using ERCHTMS.Entity.WeChatManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.WeChatManage
{
    /// <summary>
    /// 描 述：企业号应用
    /// </summary>
    public class WeChatAppMap : EntityTypeConfiguration<WeChatAppEntity>
    {
        public WeChatAppMap()
        {
            #region 表、主键
            //表
            this.ToTable("WECHAT_APP");
            //主键
            this.HasKey(t => t.AppId);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
