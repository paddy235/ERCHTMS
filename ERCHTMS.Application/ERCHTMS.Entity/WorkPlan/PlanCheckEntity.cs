using ERCHTMS.Code;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERCHTMS.Entity.WorkPlan
{
    /// <summary>
    /// 描 述：EHS计划申请审批（核）表
    /// </summary>
    [Table("HRS_PLANCHECK")]
    public class PlanCheckEntity : BaseEntity
    {
        #region 默认字段
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int? AUTOID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CREATEUSERORGCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CREATEUSERNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string MODIFYUSERNAME { get; set; }
        #endregion

        #region 实体成员  
        /// <summary>
        /// 申请记录id
        /// </summary>
        [Column("APPLYID")]
        public string ApplyId { get; set; }
        /// <summary>
        /// 审批人id
        /// </summary>
        [Column("CHECKUSERID")]
        public string CheckUserId { get; set; }
        /// <summary>
        /// 审批人名称
        /// </summary>
        [Column("CHECKUSERNAME")]
        public string CheckUserName { get; set; }
        /// <summary>
        /// 审批部门id
        /// </summary>
        [Column("CHECKDEPTID")]
        public string CheckDeptId { get; set; }
        /// <summary>
        /// 审批部门名称
        /// </summary>
        [Column("CHECKDEPTNAME")]
        public string CheckDeptName { get; set; }
        /// <summary>
        /// 审批结果（1：通过，0：未通过）
        /// </summary>
        [Column("CHECKRESULT")]
        public string CheckResult { get; set; }
        /// <summary>
        /// 审批意见
        /// </summary>
        [Column("CHECKREASON")]
        public string CheckReason { get; set; }
        /// <summary>
        /// 审批日期
        /// </summary>
        [Column("CHECKDATE")]
        public DateTime? CheckDate { get; set; }
        /// <summary>
        /// 流程回退类型
        /// </summary>
        [Column("CHECKBACKTYPE")]
        public string CheckBackType { get; set; }
        /// <summary>
        /// 流程类型
        /// </summary>
        [Column("CHECKTYPE")]
        public string CheckType { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;
            this.CREATEDATE = DateTime.Now;
            this.CREATEUSERID = OperatorProvider.Provider.Current().UserId;
            this.CREATEUSERNAME = OperatorProvider.Provider.Current().UserName;
            this.CREATEUSERDEPTCODE = OperatorProvider.Provider.Current().DeptCode;
            this.CREATEUSERORGCODE = OperatorProvider.Provider.Current().OrganizeCode;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ID = keyValue;
            this.MODIFYDATE = DateTime.Now;
            this.MODIFYUSERID = OperatorProvider.Provider.Current().UserId;
            this.MODIFYUSERNAME = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}