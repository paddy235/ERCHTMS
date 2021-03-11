using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.NosaManage
{
    /// <summary>
    /// 描 述：NOSA工作任务明细
    /// </summary>
    [Table("HRS_NOSAWORKITEM")]
    public class NosaworkitemEntity : BaseEntity
    {
        #region 默认字段
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
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
        [Column("CREATEUSERORGCODE")]
        public string CREATEUSERORGCODE { get; set; }
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
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string MODIFYUSERNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
        #endregion

        #region 实体成员
        /// <summary>
        /// 工作任务id
        /// </summary>
        /// <returns></returns>
        [Column("WORKID")]
        public string WorkId { get; set; }
        /// <summary>
        /// 负责人id
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSERID")]
        public string DutyUserId { get; set; }
        /// <summary>
        /// 负责人姓名
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSERNAME")]
        public string DutyUserName { get; set; }
        /// <summary>
        /// 责任部门id
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPARTID")]
        public string DutyDepartId { get; set; }
        /// <summary>
        /// 责任部门名称
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPARTNAME")]
        public string DutyDepartName { get; set; }
        /// <summary>
        /// 上传日期
        /// </summary>
        /// <returns></returns>
        [Column("UPLOADDATE")]
        public DateTime? UploadDate { get; set; }
        /// <summary>
        /// 是否提交（值：是，否）
        /// </summary>
        /// <returns></returns>
        [Column("ISSUBMITTED")]
        public string IsSubmitted { get; set; }
        /// <summary>
        /// 状态（待上传、待审核、通过、不通过）
        /// </summary>
        /// <returns></returns>
        [Column("STATE")]
        public string State { get; set; }
        /// <summary>
        /// 审核人id
        /// </summary>
        /// <returns></returns>
        [Column("CHECKUSERID")]
        public string CheckUserId { get; set; }
        /// <summary>
        /// 审核人姓名
        /// </summary>
        /// <returns></returns>
        [Column("CHECKUSERNAME")]
        public string CheckUserName { get; set; }
        /// <summary>
        /// 审核意见
        /// </summary>
        /// <returns></returns>
        [Column("CHECKIDEA")]
        public string CheckIdea { get; set; }
        /// <summary>
        /// 审核日期
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDATE")]
        public DateTime? CheckDate { get; set; }
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