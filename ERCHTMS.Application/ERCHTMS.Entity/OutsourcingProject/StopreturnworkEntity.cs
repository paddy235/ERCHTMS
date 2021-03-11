using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// 描 述：停复工管理表
    /// </summary>
    [Table("EPG_STOPRETURNWORK")]
    public class StopreturnworkEntity : BaseEntity
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
        /// 停工单位
        /// </summary>
        /// <returns></returns>
        [Column("OUTPROJECTID")]
        public string OUTPROJECTID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("OUTENGINEERID")]
        public string OUTENGINEERID { get; set; }
        /// <summary>
        /// 停工时间
        /// </summary>
        /// <returns></returns>
        [Column("STOPTIME")]
        public DateTime? STOPTIME { get; set; }
        /// <summary>
        /// 开/复工时间
        /// </summary>
        /// <returns></returns>
        [Column("STARTRETURNTIME")]
        public DateTime? STARTRETURNTIME { get; set; }
        /// <summary>
        /// 接收人
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTPEOPLE")]
        public string ACCEPTPEOPLE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTPEOPLEID")]
        public string ACCEPTPEOPLEID { get; set; }
        /// <summary>
        /// 下达通知人
        /// </summary>
        /// <returns></returns>
        [Column("TRANSMITPEOPLE")]
        public string TRANSMITPEOPLE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("TRANSMITPEOPLEID")]
        public string TRANSMITPEOPLEID { get; set; }
        /// <summary>
        /// 下达时间
        /// </summary>
        /// <returns></returns>
        [Column("TRANSMITTIME")]
        public DateTime? TRANSMITTIME { get; set; }
        /// <summary>
        /// 停工原因
        /// </summary>
        /// <returns></returns>
        [Column("STOPCAUSEREASON")]
        public string STOPCAUSEREASON { get; set; }
        /// <summary>
        /// 停工依据
        /// </summary>
        /// <returns></returns>
        [Column("STOPPURSUANT")]
        public string STOPPURSUANT { get; set; }

        /// <summary>
        /// 0:未提交 1:提交 
        /// </summary>
        /// <returns></returns>
        [Column("ISCOMMIT")]
        public string ISCOMMIT { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID)?Guid.NewGuid().ToString():ID;
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