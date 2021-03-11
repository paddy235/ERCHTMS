using ERCHTMS.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SystemManage
{
    /// <summary>
    /// 描 述：消息推送记录表表
    /// </summary>
    public class MessagePushRecordMap : EntityTypeConfiguration<MessagePushRecordEntity>
    {
        public MessagePushRecordMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_MESSAGEPUSHRECORD");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}