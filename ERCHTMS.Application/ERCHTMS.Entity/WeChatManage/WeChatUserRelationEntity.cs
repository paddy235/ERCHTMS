using ERCHTMS.Code;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.WeChatManage
{
    /// <summary>
    /// 描 述：企业号成员
    /// </summary>
    [Table("WECHAT_USERRELATION")]
    public class WeChatUserRelationEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 部门对应关系主键
        /// </summary>		
        [Column("USERRELATIONID")]
        public string UserRelationId { get; set; }
        /// <summary>
        /// 部门Id
        /// </summary>		
        [Column("DEPTID")]
        public string DeptId { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>		
        [Column("DEPTNAME")]
        public string DeptName { get; set; }
        /// <summary>
        /// 微信部门Id
        /// </summary>		
        [Column("WECHATDEPTID")]
        public int? WeChatDeptId { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>		
        [Column("USERID")]
        public string UserId { get; set; }
        /// <summary>
        /// 同步状态
        /// </summary>		
        [Column("SYNCSTATE")]
        public string SyncState { get; set; }
        /// <summary>
        /// 同步日志
        /// </summary>		
        [Column("SYNCLOG")]
        public string SyncLog { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>		
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 创建用户主键
        /// </summary>		
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>		
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserName = OperatorProvider.Provider.Current().UserName;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.UserRelationId = keyValue;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}