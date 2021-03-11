using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ERCHTMS.Entity;
using ERCHTMS.Code;

namespace ERCHTMS.Entity.SystemManage
{
    /// <summary>
    /// 描 述：短消息
    /// </summary>
    [Table("BASE_MESSAGE")]
    public class MessageEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>		
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        ///读取时间
        /// </summary>		
        [Column("READTIME")]
        public DateTime? ReadTime { get; set; }
        /// <summary>
        ///发送时间
        /// </summary>		
        [Column("SENDTIME")]
        public DateTime? SendTime { get; set; }
        /// <summary>
        /// 接收人账号（多个用逗号分隔）
        /// </summary>		
        [Column("USERID")]
        public string UserId { get; set; }
        /// <summary>
        /// 接收人姓名（多个用逗号分隔）
        /// </summary>		
        [Column("USERNAME")]
        public string UserName { get; set; }
        /// <summary>
        ///标题
        /// </summary>		
        [Column("TITLE")]
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>		
        [Column("CONTENT")]
        public string Content { get; set; }
        /// <summary>
        /// 地址
        /// </summary>		
        [Column("URL")]
        public string Url { get; set; }
        /// <summary>
        /// 业务记录Id
        /// </summary>		
        [Column("RECID")]
        public string RecId { get; set; }
        /// <summary>
        /// 状态
        /// </summary>		
        [Column("STATUS")]
        public string Status { get; set; }
        /// <summary>
        /// 备用字段
        /// </summary>		
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// 发送人
        /// </summary>		
        [Column("SENDUSER")]
        public string SendUser { get; set; }
        /// <summary>
        /// 发送人姓名
        /// </summary>	
        [Column("SENDUSERNAME")]
        public string SendUserName { get; set; }
        /// <summary>
        /// 消息类别
        /// </summary>	
        [Column("CATEGORY")]
        public string Category { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
            SendTime = DateTime.Now;
           
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.Id = keyValue;
        }
        #endregion
    }
}