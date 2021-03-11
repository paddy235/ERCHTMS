using ERCHTMS.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.PersonManage
{
    /// <summary>
    /// 
    /// </summary>
    [Table("BIS_LEAVEAPPROVE")]
    public class LeaveApproveEntity : BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Column("ID")]
        public string Id { get; set; }

        /// <summary>
        /// 申请单位Id
        /// </summary>
        [Column("APPLYDEPTID")]
        public string ApplyDeptId { get; set; }

        /// <summary>
        /// 申请单位
        /// </summary>
        [Column("APPLYDEPTNAME")]
        public string ApplyDeptName { get; set; }

        /// <summary>
        /// 离场时间
        /// </summary>
        [Column("LEAVETIME")]
        public DateTime? LeaveTime { get; set; }

        /// <summary>
        /// 离场原因
        /// </summary>
        [Column("LEAVEREASON")]
        public string LeaveReason { get; set; }

        /// <summary>
        /// 离场人员id
        /// </summary>
        [Column("LEAVEUSERIDS")]
        public string LeaveUserIds { get; set; }

        /// <summary>
        /// 离场人员
        /// </summary>
        [Column("LEAVEUSERNAMES")]
        public string LeaveUserNames { get; set; }

        /// <summary>
        /// 创建人id
        /// </summary>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }

        /// <summary>
        /// 创建用户所属部门编码
        /// </summary>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 创建用户所属机构编码
        /// </summary>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }

        /// <summary>
        /// 审核状态(0:待审核 1:通过 2:不通过)
        /// </summary>
        [Column("APPROVESTATE")]
        public int? ApproveState { get; set; } = 0;

        /// <summary>
        /// 流程节点部门
        /// </summary>
        [Column("FLOWDEPTNAME")]
        public string FlowDeptName { get; set; }

        /// <summary>
        /// 流程节点部门id
        /// </summary>
        [Column("FLOWDEPT")]
        public string FlowDept { get; set; }

        /// <summary>
        /// 流程节点角色
        /// </summary>
        [Column("FLOWROLENAME")]
        public string FlowRoleName { get; set; }

        /// <summary>
        /// 流程节点角色id
        /// </summary>
        [Column("FLOWROLE")]
        public string FlowRole { get; set; }

        /// <summary>
        /// 流程id
        /// </summary>
        [Column("FLOWID")]
        public string FlowId { get; set; }

        /// <summary>
        /// 流程名称
        /// </summary>
        [Column("FLOWNAME")]
        public string FlowName { get; set; }

        /// <summary>
        /// 离场人员所在部门id
        /// </summary>
        [Column("LEAVEDEPTID")]
        public string LeaveDeptId { get; set; }

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserName = OperatorProvider.Provider.Current().UserName;
            this.CreateUserDeptCode = OperatorProvider.Provider.Current().DeptCode;
            this.CreateUserOrgCode = OperatorProvider.Provider.Current().OrganizeCode;
        }
        #endregion
    }
}
