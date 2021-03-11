using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HighRiskWork
{
    /// <summary>
    /// 描 述：使用消防水
    /// </summary>
    [Table("BIS_FIREWATER")]
    public class FireWaterEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 申请单位
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDEPTCODE")]
        public string ApplyDeptCode { get; set; }
        /// <summary>
        /// 申请单位
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDEPTNAME")]
        public string ApplyDeptName { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERID")]
        public string ApplyUserId { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERNAME")]
        public string ApplyUserName { get; set; }
        /// <summary>
        /// 使用消防水单位类别(0:电厂内部 1:外包单位)
        /// </summary>
        /// <returns></returns>
        [Column("WORKDEPTTYPE")]
        public string WorkDeptType { get; set; }
        /// <summary>
        /// 使用消防水单位
        /// </summary>
        /// <returns></returns>
        [Column("WORKDEPTID")]
        public string WorkDeptId { get; set; }
        /// <summary>
        /// 使用消防水单位
        /// </summary>
        /// <returns></returns>
        [Column("WORKDEPTCODE")]
        public string WorkDeptCode { get; set; }
        /// <summary>
        /// 使用消防水单位
        /// </summary>
        /// <returns></returns>
        [Column("WORKDEPTNAME")]
        public string WorkDeptName { get; set; }
        /// <summary>
        /// 工程
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERINGID")]
        public string EngineeringId { get; set; }
        /// <summary>
        /// 工程名称
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERINGNAME")]
        public string EngineeringName { get; set; }
        /// <summary>
        /// 使用消防水开始时间
        /// </summary>
        /// <returns></returns>
        [Column("WORKSTARTTIME")]
        public DateTime? WorkStartTime { get; set; }
        /// <summary>
        /// 使用消防水结束时间
        /// </summary>
        /// <returns></returns>
        [Column("WORKENDTIME")]
        public DateTime? WorkEndTime { get; set; }
        /// <summary>
        /// 使用消防水区域
        /// </summary>
        /// <returns></returns>
        [Column("WORKAREACODE")]
        public string WorkAreaCode { get; set; }
        /// <summary>
        /// 使用消防水区域
        /// </summary>
        /// <returns></returns>
        [Column("WORKAREANAME")]
        public string WorkAreaName { get; set; }
        /// <summary>
        /// 使用消防水地点
        /// </summary>
        /// <returns></returns>
        [Column("WORKPLACE")]
        public string WorkPlace { get; set; }
        /// <summary>
        /// 使用消防水内容
        /// </summary>
        /// <returns></returns>
        [Column("WORKCONTENT")]
        public string WorkContent { get; set; }
        /// <summary>
        /// 申请状态(0.申请中,1.审核（批）中,2.审核（批）未通过,3.审核（批）通过）
        /// </summary>
        /// <returns></returns>
        [Column("APPLYSTATE")]
        public string ApplyState { get; set; }
        /// <summary>
        /// 申请编号
        /// </summary>
        /// <returns></returns>
        [Column("APPLYNUMBER")]
        public string ApplyNumber { get; set; }
        /// <summary>
        /// 流程节点部门
        /// </summary>
        /// <returns></returns>
        [Column("FLOWDEPTNAME")]
        public string FlowDeptName { get; set; }
        /// <summary>
        /// 流程节点部门id
        /// </summary>
        /// <returns></returns>
        [Column("FLOWDEPT")]
        public string FlowDept { get; set; }
        /// <summary>
        /// 流程节点角色
        /// </summary>
        /// <returns></returns>
        [Column("FLOWROLENAME")]
        public string FlowRoleName { get; set; }
        /// <summary>
        /// 流程节点角色id
        /// </summary>
        /// <returns></returns>
        [Column("FLOWROLE")]
        public string FlowRole { get; set; }
        /// <summary>
        /// 流程名称
        /// </summary>
        /// <returns></returns>
        [Column("FLOWNAME")]
        public string FlowName { get; set; }
        /// <summary>
        /// 流程id
        /// </summary>
        /// <returns></returns>
        [Column("FLOWID")]
        public string FlowId { get; set; }
        /// <summary>
        /// 结果（0：申请 2：审核 3：完成）
        /// </summary>
        /// <returns></returns>
        [Column("INVESTIGATESTATE")]
        public string InvestigateState { get; set; }
        /// <summary>
        /// 作业人员
        /// </summary>
        /// <returns></returns>
        [Column("WORKUSERIDS")]
        public string WorkUserIds { get; set; }
        /// <summary>
        /// 作业人员
        /// </summary>
        /// <returns></returns>
        [Column("WORKUSERNAMES")]
        public string WorkUserNames { get; set; }
        /// <summary>
        /// 使用消防水实际开始时间
        /// </summary>
        /// <returns></returns>
        [Column("REALITYWORKSTARTTIME")]
        public DateTime? RealityWorkStartTime { get; set; }
        /// <summary>
        /// 使用消防水实际结束时间
        /// </summary>
        /// <returns></returns>
        [Column("REALITYWORKENDTIME")]
        public DateTime? RealityWorkEndTime { get; set; }
        /// <summary>
        /// 专业类别
        /// </summary>
        /// <returns></returns>
        [Column("SPECIALTYTYPE")]
        public string SpecialtyType { get; set; }
        /// <summary>
        /// 审核备注
        /// </summary>
        /// <returns></returns>
        [Column("FLOWREMARK")]
        public string FlowRemark { get; set; }
        /// <summary>
        /// 抄送人
        /// </summary>
        /// <returns></returns>
        [Column("COPYUSERIDS")]
        public string CopyUserIds { get; set; }
        /// <summary>
        /// 抄送人
        /// </summary>
        /// <returns></returns>
        [Column("COPYUSERNAMES")]
        public string CopyUserNames { get; set; }
        /// <summary>
        /// 短信通知审批人(0:否 1:是)
        /// </summary>
        /// <returns></returns>
        [Column("ISMESSAGE")]
        public string IsMessage { get; set; }
        /// <summary>
        /// 使用中应采取措施
        /// </summary>
        /// <returns></returns>
        [Column("MEASURE")]
        public string Measure { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 创建用户id
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 修改用户id
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 创建用户所属部门编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 创建用户所属机构编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 申请单位
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDEPTID")]
        public string ApplyDeptId { get; set; }
        /// <summary>
        /// 执行状态 0 未执行 1 已执行
        /// </summary>
        [Column("CONDITIONSTATE")]
        public string ConditionState { get; set; }

        /// <summary>
        /// 申请用途
        /// </summary>
        [Column("WORKUSE")]
        public string WorkUse { get; set; }

        /// <summary>
        /// 作业状态 0: 正常作业  1:暂停作业
        /// </summary>
        [Column("WORKOPERATE")]
        public string WorkOperate { get; set; }

        /// <summary>
        /// 消防工具
        /// </summary>
        [Column("TOOL")]
        public string Tool { get; set; }

        /// <summary>
        /// 手动选择的消防工具
        /// </summary>
        [Column("HDTOOL")]
        public string hdTool { get; set; }

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