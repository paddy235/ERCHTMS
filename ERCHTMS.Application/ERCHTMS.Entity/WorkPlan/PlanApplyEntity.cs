using ERCHTMS.Code;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERCHTMS.Entity.WorkPlan
{
    /// <summary>
    /// 描 述：EHS计划申请表
    /// </summary>
    [Table("HRS_PLANAPPLY")]
    public class PlanApplyEntity : BaseEntity
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
        /// 申请人id
        /// </summary>
        [Column("USERID")]
        public string UserId { get; set; }
        /// <summary>
        /// 申请人姓名
        /// </summary>
        [Column("USERNAME")]
        public string UserName { get; set; }
        /// <summary>
        /// 申请部门id
        /// </summary>
        [Column("DEPARTID")]
        public string DepartId { get; set; }
        /// <summary>
        /// 申请部门名称
        /// </summary>
        [Column("DEPARTNAME")]
        public string DepartName { get; set; }
        /// <summary>
        /// 申请日期
        /// </summary>
        [Column("APPLYDATE")]
        public DateTime? ApplyDate { get; set; }
        /// <summary>
        /// 申请类型（部门计划，个人计划）
        /// </summary>
        [Column("APPLYTYPE")]
        public string ApplyType { get; set; }
        /// <summary>
        /// 审核（批）人帐号
        /// </summary>
        [Column("CHECKUSERACCOUNT")]
        public string CheckUserAccount { get; set; }
        /// <summary>
        /// 流程状态
        /// </summary>
        [Column("FLOWSTATE")]
        public string FlowState { get; set; }
        /// <summary>
        /// 引用记录id
        /// </summary>
        [Column("BASEID")]
        public string BaseId { get; set; }
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