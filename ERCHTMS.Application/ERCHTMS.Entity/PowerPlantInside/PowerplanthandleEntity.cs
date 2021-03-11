using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.PowerPlantInside
{
    /// <summary>
    /// 描 述：事故事件处理
    /// </summary>
    [Table("BIS_POWERPLANTHANDLE")]
    public class PowerplanthandleEntity : BaseEntity
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
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 所属部门
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPT")]
        public string BelongDept { get; set; }
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
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 所属部门Code
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPTCODE")]
        public string BelongDeptCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 审核步骤名称
        /// </summary>
        /// <returns></returns>
        [Column("FLOWNAME")]
        public string FlowName { get; set; }
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
        /// 审核角色ID
        /// </summary>
        /// <returns></returns>
        [Column("FLOWROLE")]
        public string FlowRole { get; set; }
        /// <summary>
        /// 是否保存成功
        /// </summary>
        /// <returns></returns>
        [Column("ISSAVED")]
        public int? IsSaved { get; set; }
        /// <summary>
        /// 发生时间
        /// </summary>
        /// <returns></returns>
        [Column("HAPPENTIME")]
        public DateTime? HappenTime { get; set; }
        /// <summary>
        /// 审核部门名称
        /// </summary>
        /// <returns></returns>
        [Column("FLOWDEPTNAME")]
        public string FlowDeptName { get; set; }
        /// <summary>
        /// 所属部门ID
        /// </summary>
        /// <returns></returns>
        [Column("BELONGDEPTID")]
        public string belongDeptId { get; set; }
        /// <summary>
        /// 事故事件类别
        /// </summary>
        /// <returns></returns>
        [Column("ACCIDENTEVENTTYPE")]
        public string AccidentEventType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ACCIDENTEVENTID")]
        public string AccidentEventId { get; set; }
        /// <summary>
        /// 事故事件性质
        /// </summary>
        /// <returns></returns>
        [Column("ACCIDENTEVENTPROPERTY")]
        public string AccidentEventProperty { get; set; }
        /// <summary>
        /// 申请状态(0.申请中,1.审核中,2.整改中,3.验收中,4.已完成)
        /// </summary>
        /// <returns></returns>
        [Column("APPLYSTATE")]
        public int? ApplyState { get; set; }
        /// <summary>
        /// 审核流程ID
        /// </summary>
        /// <returns></returns>
        [Column("FLOWID")]
        public string FlowId { get; set; }
        /// <summary>
        /// 情况简介
        /// </summary>
        /// <returns></returns>
        [Column("SITUATIONINTRODUCTION")]
        public string SituationIntroduction { get; set; }
        /// <summary>
        /// 审核部门ID
        /// </summary>
        /// <returns></returns>
        [Column("FLOWDEPT")]
        public string FlowDept { get; set; }
        /// <summary>
        /// 审核角色名称
        /// </summary>
        /// <returns></returns>
        [Column("FLOWROLENAME")]
        public string FlowRoleName { get; set; }
        /// <summary>
        /// 原因及存在问题
        /// </summary>
        /// <returns></returns>
        [Column("REASONANDPROBLEM")]
        public string ReasonAndProblem { get; set; }
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
        [Column("ACCIDENTEVENTNAME")]
        public string AccidentEventName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
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