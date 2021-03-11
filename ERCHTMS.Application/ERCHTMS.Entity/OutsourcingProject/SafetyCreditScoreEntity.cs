using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// 描 述：安全信用评价分数表
    /// </summary>
    [Table("EPG_SAFETYCREDITSCORE")]
    public class SafetyCreditScoreEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 安全计划评价主键ID
        /// </summary>
        /// <returns></returns>
        [Column("SAFETYCREDITEVALUATEID")]
        public string SAFETYCREDITEVALUATEID { get; set; }
        /// <summary>
        /// 分数类型 0：加 1：减
        /// </summary>
        /// <returns></returns>
        [Column("SCORETYPE")]
        public string SCORETYPE { get; set; }
        /// <summary>
        /// 评分人ID
        /// </summary>
        /// <returns></returns>
        [Column("SCOREPERSONID")]
        public string SCOREPERSONID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
        /// <summary>
        /// 评分人
        /// </summary>
        /// <returns></returns>
        [Column("SCOREPERSON")]
        public string SCOREPERSON { get; set; }
        /// <summary>
        /// 评分时间
        /// </summary>
        /// <returns></returns>
        [Column("SCORETIME")]
        public DateTime? SCORETIME { get; set; }
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
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
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
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
        /// <summary>
        /// 理由
        /// </summary>
        /// <returns></returns>
        [Column("REASON")]
        public string REASON { get; set; }
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
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// 分数
        /// </summary>
        /// <returns></returns>
        [Column("SCORE")]
        public string SCORE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CREATEUSERORGCODE { get; set; }

        /// <summary>
        /// 评价部门id
        /// </summary>
        /// <returns></returns>
        [Column("EVALUATEDEPT")]
        public string EVALUATEDEPT { get; set; }

        /// <summary>
        /// 评价部门名称
        /// </summary>
        /// <returns></returns>
        [Column("EVALUATEDEPTNAME")]
        public string EVALUATEDEPTNAME { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        /// <returns></returns>
        [Column("ISVALID")]
        public string ISVALID { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
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