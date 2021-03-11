using ERCHTMS.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.DangerousJob
{
    /// <summary>
    /// 描 述：危险作业流程流转表
    /// </summary>
    [Table("BIS_DANGEROUSJOBFLOWDETAIL")]
    public class DangerousJobFlowDetailEntity : BaseEntity
    {

        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int? Autoid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 逐级审核模块编号
        /// </summary>
        /// <returns></returns>
        [Column("MODULENO")]
        public string ModuleNo { get; set; }
        /// <summary>
        /// 节点名称
        /// </summary>
        /// <returns></returns>
        [Column("FLOWNAME")]
        public string FlowName { get; set; }
        /// <summary>
        /// 当前流程步骤
        /// </summary>
        /// <returns></returns>
        [Column("CURRENTSTEP")]
        public int? CurrentStep { get; set; }
        /// <summary>
        /// 下一步流程步骤,0标识流程已结束
        /// </summary>
        /// <returns></returns>
        [Column("NEXTSTEP")]
        public int? NextStep { get; set; }
        /// <summary>
        /// 关联业务数据id
        /// </summary>
        /// <returns></returns>
        [Column("BUSINESSID")]
        public string BusinessId { get; set; }
        /// <summary>
        /// 当前步骤处理标示(0部门加角色，1执行脚本获取业务某个字段，2指定审核人，3业务选择审核人)
        /// </summary>
        /// <returns></returns>
        [Column("PROCESSORFLAG")]
        public string ProcessorFlag { get; set; }
        /// <summary>
        /// 部门id
        /// </summary>
        /// <returns></returns>
        [Column("DEPTID")]
        public string DeptId { get; set; }
        /// <summary>
        /// 部门code
        /// </summary>
        /// <returns></returns>
        [Column("DEPTCODE")]
        public string DeptCode { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNAME")]
        public string DeptName { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        /// <returns></returns>
        [Column("ROLENAME")]
        public string RoleName { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        /// <returns></returns>
        [Column("ROLEID")]
        public string RoleId { get; set; }
        /// <summary>
        /// 审核人id
        /// </summary>
        /// <returns></returns>
        [Column("USERID")]
        public string UserId { get; set; }
        /// <summary>
        /// 审核人账号
        /// </summary>
        /// <returns></returns>
        [Column("USERACCOUNT")]
        public string UserAccount { get; set; }
        /// <summary>
        /// 审核人名称
        /// </summary>
        /// <returns></returns>
        [Column("USERNAME")]
        public string UserName { get; set; }
        /// <summary>
        /// 状态,0正在处理,1已处理
        /// </summary>
        /// <returns></returns>
        [Column("STATUS")]
        public int? Status { get; set; }
        /// <summary>
        /// 审核结果
        /// </summary>
        /// <returns></returns>
        [Column("CHECKRESULT")]
        public string CheckResult { get; set; }
        /// <summary>
        /// 审批时间
        /// </summary>
        /// <returns></returns>
        [Column("APPROVETIME")]
        public DateTime? ApproveTime { get; set; }
        /// <summary>
        /// 审批部门
        /// </summary>
        /// <returns></returns>
        [Column("APPROVEDEPTNAME")]
        public string ApproveDeptName { get; set; }
        /// <summary>
        /// 审批部门id
        /// </summary>
        /// <returns></returns>
        [Column("APPROVEDEPTID")]
        public string ApproveDeptId { get; set; }
        /// <summary>
        /// 审批人
        /// </summary>
        /// <returns></returns>
        [Column("APPROVEPERSON")]
        public string ApprovePerson { get; set; }
        /// <summary>
        /// 审批人id
        /// </summary>
        /// <returns></returns>
        [Column("APPROVEPERSONID")]
        public string ApprovePersonId { get; set; }
        /// <summary>
        /// 审批意见
        /// </summary>
        /// <returns></returns>
        [Column("APPROVEOPINION")]
        public string ApproveOpinion { get; set; }
        /// <summary>
        /// 签名url
        /// </summary>
        /// <returns></returns>
        [Column("SIGNURL")]
        public string SignUrl { get; set; }
        /// <summary>
        /// 第几次申请
        /// </summary>
        /// <returns></returns>
        [Column("APPLYNUMBER")]
        public int ApplyNumber { get; set; }
        /// <summary>
        /// 变更移除的审核人账号
        /// </summary>
        /// <returns></returns>
        [Column("REMOVEACCOUNT")]
        public string RemoveAccount { get; set; }
        /// <summary>
        /// 变更新增的审核人账号
        /// </summary>
        /// <returns></returns>
        [Column("ADDACCOUNT")]
        public string AddAccount { get; set; }
        #endregion

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
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.Id = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}
