using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// 描 述：安全评价
    /// </summary>
    [Table("EPG_SAFETYEVALUATE")]
    public class SafetyEvaluateEntity : BaseEntity
    {
        #region 实体成员
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
        /// <summary>
        /// 安全文明生产及现场管理得分
        /// </summary>
        /// <returns></returns>
        [Column("SITEMANAGEMENTSCORE")]
        public string SITEMANAGEMENTSCORE { get; set; }
        /// <summary>
        /// 质量品质得分
        /// </summary>
        /// <returns></returns>
        [Column("QUALITYSCORE")]
        public string QUALITYSCORE { get; set; }
        /// <summary>
        /// 工程进度得分
        /// </summary>
        /// <returns></returns>
        [Column("PROJECTPROGRESSSCORE")]
        public string PROJECTPROGRESSSCORE { get; set; }
        /// <summary>
        /// 现场服务得分
        /// </summary>
        /// <returns></returns>
        [Column("FIELDSERVICESCORE")]
        public string FIELDSERVICESCORE { get; set; }
        /// <summary>
        /// 评价人
        /// </summary>
        /// <returns></returns>
        [Column("EVALUATOR")]
        public string EVALUATOR { get; set; }
        /// <summary>
        /// 评价人ID
        /// </summary>
        /// <returns></returns>
        [Column("EVALUATORID")]
        public string EVALUATORID { get; set; }
        /// <summary>
        /// 评价时间
        /// </summary>
        /// <returns></returns>
        [Column("EVALUATIONTIME")]
        public DateTime? EVALUATIONTIME { get; set; }
        /// <summary>
        /// 评价总分
        /// </summary>
        /// <returns></returns>
        [Column("EVALUATIONSCORE")]
        public string EVALUATIONSCORE { get; set; }
        /// <summary>
        /// 工程ID
        /// </summary>
        /// <returns></returns>
        [Column("PROJECTID")]
        public string PROJECTID { get; set; }
        /// <summary>
        ///是否发送0否，1是
        /// </summary>
        /// <returns></returns>
        [Column("ISSEND")]
        public string ISSEND { get; set; }
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