using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.PowerPlantInside
{
    /// <summary>
    /// 描 述：事故事件处理信息
    /// </summary>
    [Table("BIS_POWERPLANTHANDLEDETAIL")]
    public class PowerplanthandledetailEntity : BaseEntity
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
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 申请状态(0.申请中,1.审核中,2,审核不通过,3.整改中,4.验收中,5.已完成)
        /// </summary>
        /// <returns></returns>
        [Column("APPLYSTATE")]
        public int? ApplyState { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 关联事故事件处理记录ID
        /// </summary>
        /// <returns></returns>
        [Column("POWERPLANTHANDLEID")]
        public string PowerPlantHandleId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
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
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 整改责任人ID
        /// </summary>
        /// <returns></returns>
        [Column("RECTIFICATIONDUTYPERSONID")]
        public string RectificationDutyPersonId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 整改责任部门
        /// </summary>
        /// <returns></returns>
        [Column("RECTIFICATIONDUTYDEPT")]
        public string RectificationDutyDept { get; set; }
        /// <summary>
        /// 整改责任部门ID
        /// </summary>
        /// <returns></returns>
        [Column("RECTIFICATIONDUTYDEPTID")]
        public string RectificationDutyDeptId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 整改期限
        /// </summary>
        /// <returns></returns>
        [Column("RECTIFICATIONTIME")]
        public DateTime? RectificationTime { get; set; }
        /// <summary>
        /// 整改措施
        /// </summary>
        /// <returns></returns>
        [Column("RECTIFICATIONMEASURES")]
        public string RectificationMeasures { get; set; }
        /// <summary>
        /// 整改责任人
        /// </summary>
        /// <returns></returns>
        [Column("RECTIFICATIONDUTYPERSON")]
        public string RectificationDutyPerson { get; set; }

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
        /// 流程名称
        /// </summary>
        [Column("FLOWROLE")]
        public string FlowRole { get; set; }

        /// <summary>
        /// 流程名称
        /// </summary>
        [Column("FLOWNAME")]
        public string FlowName { get; set; }
        /// <summary>
        /// 流程节点ID
        /// </summary>
        [Column("FLOWID")]
        public string FlowId { get; set; }

        /// <summary>
        /// 实际整改部门
        /// </summary>
        [Column("REALREFORMDEPT")]
        public string RealReformDept { get; set; }

        /// <summary>
        /// 实际整改部门ID
        /// </summary>
        [Column("REALREFORMDEPTID")]
        public string RealReformDeptId { get; set; }

        /// <summary>
        /// 实际整改部门Code
        /// </summary>
        [Column("REALREFORMDEPTCODE")]
        public string RealReformDeptCode { get; set; }

        /// <summary>
        /// 原因及暴露问题
        /// </summary>
        [Column("REASONANDPROBLEM")]
        public string ReasonAndProblem { get; set; }

        /// <summary>
        /// 签收部门
        /// </summary>
        [Column("SIGNDEPTNAME")]
        public string SignDeptName { get; set; }

        /// <summary>
        /// 签收部门
        /// </summary>
        [Column("SIGNDEPTID")]
        public string SignDeptId { get; set; }

        /// <summary>
        /// 签收人
        /// </summary>
        [Column("SIGNPERSONNAME")]
        public string SignPersonName { get; set; }

        /// <summary>
        /// 签收人
        /// </summary>
        [Column("SIGNPERSONID")]
        public string SignPersonId { get; set; }

        /// <summary>
        /// 是否指定责任人
        /// </summary>
        [Column("ISASSIGNPERSON")]
        public string IsAssignPerson { get; set; }

        /// <summary>
        /// 实际签收人
        /// </summary>
        [Column("REALSIGNPERSONNAME")]
        public string RealSignPersonName { get; set; }

        /// <summary>
        /// 实际签收人
        /// </summary>
        [Column("REALSIGNPERSONID")]
        public string RealSignPersonId { get; set; }

        /// <summary>
        /// 签收时间
        /// </summary>
        [Column("REALSIGNDATE")]
        public DateTime? RealSignDate { get; set; }

        /// <summary>
        /// 实际签收部门
        /// </summary>
        [Column("REALSIGNPERSONDEPT")]
        public string RealSignPersonDept { get; set; }

        /// <summary>
        /// 实际签收部门
        /// </summary>
        [Column("REALSIGNPERSONDEPTID")]
        public string RealSignPersonDeptId { get; set; }

        /// <summary>
        /// 实际签收部门
        /// </summary>
        [Column("REALSIGNPERSONDEPTCODE")]
        public string RealSignPersonDeptCode { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        [Column("APPLYCODE")]
        public string ApplyCode { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id)?Guid.NewGuid().ToString():Id;
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