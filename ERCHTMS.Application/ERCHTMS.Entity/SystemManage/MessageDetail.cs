using ERCHTMS.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.SystemManage
{
    /// <summary>
    /// 短消息详情表
    /// </summary>
    [Table("BASE_MESSAGEDETAIL")]
    public class MessageDetail : BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>		
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 创建人Id
        /// </summary>		
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建人部门Code
        /// </summary>		
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 创建人机构Code
        /// </summary>		
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>		
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 查看人
        /// </summary>
        [Column("USERNAME")]
        public string UserName { get; set; }
        [Column("USERACCOUNT")]
        public string UserAccount { get; set; }
        [Column("USERID")]
        public string UserId { get; set; }
        /// <summary>
        /// 查看人部门
        /// </summary>
        [Column("DEPTNAME")]
        public string DeptName { get; set; }
        [Column("DEPTCODE")]
        public string DeptCode { get; set; }
        [Column("DEPTID")]
        public string DeptId { get; set; }
        /// <summary>
        /// 查看时间
        /// </summary>
        [Column("LOOKTIME")]
        public DateTime? LookTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// 是否已经查看 1 已查看 0 未查看
        /// </summary>
        [Column("STATUS")]
        public int? Status { get; set; }

        [Column("MESSAGEID")]
        public string MessageId { get; set; }
        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
            this.CreateDate = DateTime.Now;
           
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.Id = keyValue;
            this.ModifyDate = DateTime.Now;
        }
        #endregion

    }
}
