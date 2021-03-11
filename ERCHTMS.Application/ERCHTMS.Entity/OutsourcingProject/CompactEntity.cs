using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// 描 述：合同
    /// </summary>
    [Table("EPG_COMPACT")]
    public class CompactEntity : BaseEntity
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
        /// 合同生效时间
        /// </summary>
        /// <returns></returns>
        [Column("COMPACTTAKEEFFECTDATE")]
        public DateTime? COMPACTTAKEEFFECTDATE { get; set; }
        /// <summary>
        /// 合同有效时间
        /// </summary>
        /// <returns></returns>
        [Column("COMPACTEFFECTIVEDATE")]
        public DateTime? COMPACTEFFECTIVEDATE { get; set; }
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
        ///合同编号
        /// </summary>
        /// <returns></returns>
        [Column("COMPACTNO")]
        public string COMPACTNO { get; set; }

        /// <summary>
        ///甲方签订人/单位
        /// </summary>
        /// <returns></returns>
        [Column("FIRSTPARTY")]
        public string FIRSTPARTY { get; set; }

        /// <summary>
        ///乙方签订人/单位
        /// </summary>
        /// <returns></returns>
        [Column("SECONDPARTY")]
        public string SECONDPARTY { get; set; }

        /// <summary>
        /// 合同金额
        /// </summary>
        [Column("COMPACTMONEY")]
        public decimal? COMPACTMONEY { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("REMARK")]
        public string REMARK { get; set;}
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