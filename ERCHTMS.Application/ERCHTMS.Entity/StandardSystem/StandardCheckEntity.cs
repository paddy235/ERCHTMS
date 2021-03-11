using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.StandardSystem
{
    /// <summary>
    /// 描 述：标准审批表
    /// </summary>
    [Table("HRS_STANDARDCHECK")]
    public class StandardCheckEntity : BaseEntity
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
        /// <returns></returns>
        [Column("RECID")]
        public string RecID { get; set; }
        /// <summary>
        /// 审批人ID
        /// </summary>
        /// <returns></returns>
        [Column("CHECKUSERID")]
        public string CheckUserId { get; set; }
        /// <summary>
        /// 审批人姓名
        /// </summary>
        /// <returns></returns>
        [Column("CHECKUSERNAME")]
        public string CheckUserName { get; set; }
        /// <summary>
        /// 审批部门id
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDEPTID")]
        public string CheckDeptID { get; set; }
        /// <summary>
        /// 审批部门名称
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDEPTNAME")]
        public string CheckDeptName { get; set; }
        /// <summary>
        /// 审批结果（1：通过，0：未通过）
        /// </summary>
        /// <returns></returns>
        [Column("CHECKRESULT")]
        public string CheckResult { get; set; }
        /// <summary>
        /// 审批意见
        /// </summary>
        /// <returns></returns>
        [Column("CHECKREASON")]
        public string CheckReason { get; set; }
        /// <summary>
        /// 审批日期
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDATE")]
        public DateTime? CheckDate { get; set; }
        /// <summary>
        /// 驳回类型
        /// </summary>
        /// <returns></returns>
        [Column("CHECKBACKTYPE")]
        public string CheckBackType { get; set; }
        /// <summary>
        /// 审核类型
        /// </summary>
        /// <returns></returns>
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