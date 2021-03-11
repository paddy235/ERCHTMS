using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ERCHTMS.Entity.HighRiskWork
{
    /// <summary>
    /// 描 述：起吊作业审核记录
    /// </summary>
    [Table("BIS_LIFTHOISTAUDITRECORD")]
    public class LifthoistauditrecordEntity : BaseEntity
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
        [JsonIgnore]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        [JsonIgnore]
        public string CREATEUSERDEPTCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        [JsonIgnore]
        public string CREATEUSERORGCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        [JsonIgnore]
        public DateTime? CREATEDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        [JsonIgnore]
        public string CREATEUSERNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        [JsonIgnore]
        public DateTime? MODIFYDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODITYUSERID")]
        [JsonIgnore]
        public string MODITYUSERID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODITYUSERNAME")]
        [JsonIgnore]
        public string MODITYUSERNAME { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        /// <returns></returns>
        [Column("AUDITDATE")]
        public DateTime? AUDITDATE { get; set; }
        /// <summary>
        /// 审核部门ID
        /// </summary>
        /// <returns></returns>
        [Column("AUDITDEPTID")]
        public string AUDITDEPTID { get; set; }
        /// <summary>
        /// 审核部门CODE
        /// </summary>
        /// <returns></returns>
        [Column("AUDITDEPTCODE")]
        public string AUDITDEPTCODE { get; set; }
        /// <summary>
        /// 审核部门名称
        /// </summary>
        /// <returns></returns>
        [Column("AUDITDEPTNAME")]
        public string AUDITDEPTNAME { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        /// <returns></returns>
        [Column("AUDITUSERID")]
        public string AUDITUSERID { get; set; }
        /// <summary>
        /// 审核人名称
        /// </summary>
        /// <returns></returns>
        [Column("AUDITUSERNAME")]
        public string AUDITUSERNAME { get; set; }
        /// <summary>
        /// 审核意见
        /// </summary>
        /// <returns></returns>
        [Column("AUDITREMARK")]
        public string AUDITREMARK { get; set; }
        /// <summary>
        /// 审核状态（0 or null-不同意 1-同意）
        /// </summary>
        /// <returns></returns>
        [Column("AUDITSTATE")]
        public int? AUDITSTATE { get; set; }
        /// <summary>
        /// 业务ID
        /// </summary>
        /// <returns></returns>
        [Column("BUSINESSID")]
        public string BUSINESSID { get; set; }
        /// <summary>
        /// 流程ID
        /// </summary>
        /// <returns></returns>
        [Column("FLOWID")]
        public string FLOWID { get; set; }

        /// <summary>
        /// 审核签名
        /// </summary>
        [Column("AUDITSIGNIMG")]
        public string AUDITSIGNIMG { get; set; }

        /// <summary>
        /// 是否失效  0:有效 1：失效
        /// </summary>
        [Column("DISABLE")]
        public int? DISABLE { get; set; }
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
            this.MODITYUSERID = OperatorProvider.Provider.Current().UserId;
            this.MODITYUSERNAME = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}