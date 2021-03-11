using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Code;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// 描 述：安全保证金历史信息
    /// </summary>
    [Table("EPG_HISSAFETYEAMESTMONEY")]
    public class HistoryMoneyEntity : BaseEntity
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
        /// 外包单位
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNAME")]
        public string DEPTNAME { get; set; }
        /// <summary>
        /// 外包单位ID
        /// </summary>
        /// <returns></returns>
        [Column("DEPTID")]
        public string DEPTID { get; set; }
        /// <summary>
        /// 缴纳人
        /// </summary>
        /// <returns></returns>
        [Column("PAYMENTPERSON")]
        public string PAYMENTPERSON { get; set; }
        /// <summary>
        /// 缴纳人ID
        /// </summary>
        /// <returns></returns>
        [Column("PAYMENTPERSONID")]
        public string PAYMENTPERSONID { get; set; }
        /// <summary>
        /// 缴纳时间
        /// </summary>
        /// <returns></returns>
        [Column("PAYMENTDATE")]
        public DateTime? PAYMENTDATE { get; set; }
        /// <summary>
        /// 缴纳金额
        /// </summary>
        /// <returns></returns>
        [Column("PAYMENTMONEY")]
        public string PAYMENTMONEY { get; set; }
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
        /// <summary>
        ///安全保证金ID
        /// </summary>
        /// <returns></returns>
        [Column("MONEYID")]
        public string MONEYID { get; set; }
        /// <summary>
        ///是否退还保证金 1 是 0 否
        /// </summary>
        /// <returns></returns>
        [Column("SENDBACK")]
        public string Sendback { get; set; }
        /// <summary>
        ///退还金额
        /// </summary>
        /// <returns></returns>
        [Column("SENDBACKMONEY")]
        public string SendbackMoney { get; set; }
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
