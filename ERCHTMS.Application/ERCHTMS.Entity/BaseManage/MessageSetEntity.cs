using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ERCHTMS.Entity;
using ERCHTMS.Code;

namespace ERCHTMS.Entity.BaseManage
{
    /// <summary>
    /// 描 述：消息设置
    /// </summary>
    [Table("BASE_MESSAGESET")]
    public class MessageSetEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>		
        [Column("ID")]
        public string Id { get; set; }
         
        /// <summary>
        /// 类别
        /// </summary>		
        [Column("KIND")]
        public string Kind { get; set; }
        /// <summary>
        /// 名称
        /// </summary>		
        [Column("NAME")]
        public string Name { get; set; }
        /// <summary>
        /// 编码
        /// </summary>		
        [Column("CODE")]
        public string Code { get; set; }
        /// <summary>
        /// 触发条件
        /// </summary>		
        [Column("EVENT")]
        public string Event { get; set; }
        /// <summary>
        /// 标题
        /// </summary>		
        [Column("TITLE")]
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>		
        [Column("CONTENT")]
        public string Content { get; set; }
        /// <summary>
        /// 状态
        /// </summary>		
        [Column("STATUS")]
        public int Status { get; set; }
        /// <summary>
        /// 地址
        /// </summary>		
        [Column("URL")]
        public string Url { get; set; }
        /// <summary>
        /// 备用字段
        /// </summary>		
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// 是否采用极光推送至App
        /// </summary>		
        [Column("ISPUSH")]
        public int IsPush { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
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