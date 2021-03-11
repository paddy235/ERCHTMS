using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SystemManage
{
    /// <summary>
    /// 描 述：短消息
    /// </summary>
    public class MessageMap : EntityTypeConfiguration<MessageEntity>
    {
        public MessageMap()
        {
            #region 表、主键
            //表
            this.ToTable("BASE_MESSAGE");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
