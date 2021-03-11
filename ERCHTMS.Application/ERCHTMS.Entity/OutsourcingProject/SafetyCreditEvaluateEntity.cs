using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// 安全信用评价
    /// </summary>
    [Table("EPG_SAFETYCREDITEVALUATE")]
    public class SafetyCreditEvaluateEntity : BaseEntity
    {

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
        /// 工程id
        /// </summary>
        /// <returns></returns>
        [Column("PROJECTID")]
        public string PROJECTID { get; set; }
        /// <summary>
        /// 评价部门
        /// </summary>
        /// <returns></returns>
        [Column("EVALUATEDEPTNAME")]
        public string EVALUATEDEPTNAME { get; set; }
        /// <summary>
        /// 评价部门id
        /// </summary>
        /// <returns></returns>
        [Column("EVALUATEDEPT")]
        public string EVALUATEDEPT { get; set; }
        /// <summary>
        /// 评价状态 0：评价中  1：已评价
        /// </summary>
        /// <returns></returns>
        [Column("EVALUATESTATE")]
        public string EVALUATESTATE { get; set; }
        /// <summary>
        /// 安全信用原始分值
        /// </summary>
        /// <returns></returns>
        [Column("ORIGINALSCORE")]
        public string ORIGINALSCORE { get; set; }
        /// <summary>
        /// 安全信用实际分值
        /// </summary>
        /// <returns></returns>
        [Column("ACTUALSCORE")]
        public string ACTUALSCORE { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        /// <returns></returns>
        [Column("CREATEPERSON")]
        public string CREATEPERSON { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEEVALUATETIME")]
        public DateTime? CREATEEVALUATETIME { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEPERSONID")]
        public string CREATEPERSONID { get; set; }
        

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
