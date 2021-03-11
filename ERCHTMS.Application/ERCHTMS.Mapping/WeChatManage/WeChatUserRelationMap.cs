using ERCHTMS.Entity.WeChatManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.WeChatManage
{
    /// <summary>
    /// 描 述：企业号成员
    /// </summary>
    public class WeChatUserRelationMap : EntityTypeConfiguration<WeChatUserRelationEntity>
    {
        public WeChatUserRelationMap()
        {
            #region 表、主键
            //表
            this.ToTable("WECHAT_USERRELATION");
            //主键
            this.HasKey(t => t.UserRelationId);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
