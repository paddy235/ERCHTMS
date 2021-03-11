using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.LllegalStandard
{
    /// <summary>
    /// 描 述：违章标准表
    /// </summary>
    [Table("BIS_LLLEGALSTANDARD")]
    public class LllegalstandardEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 违章描述
        /// </summary>
        /// <returns></returns>
        [Column("DES")]
        public string DES { get; set; }
        /// <summary>
        /// 违章级别编号
        /// </summary>
        /// <returns></returns>
        [Column("LEGLEVEL")]
        public string LEGLEVEL { get; set; }
        /// <summary>
        /// 违章类型编号
        /// </summary>
        /// <returns></returns>
        [Column("LEGTYPE")]
        public string LEGTYPE { get; set; }
        /// <summary>
        /// 作业类型编号
        /// </summary>
        /// <returns></returns>
        [Column("BUSTYPE")]
        public string BUSTYPE { get; set; }
        /// <summary>
        /// 责任人扣分
        /// </summary>
        /// <returns></returns>
        [Column("DESCORE")]
        public decimal? DESCORE { get; set; }
        /// <summary>
        /// 责任人考核（元）
        /// </summary>
        /// <returns></returns>
        [Column("DEMONEY")]
        public double? DEMONEY { get; set; }
        /// <summary>
        /// 第一联责任人扣分
        /// </summary>
        /// <returns></returns>
        [Column("FIRSTDESCORE")]
        public decimal? FIRSTDESCORE { get; set; }
        /// <summary>
        /// 第一联责任人考核（元）
        /// </summary>
        /// <returns></returns>
        [Column("FIRSTDEMONEY")]
        public double? FIRSTDEMONEY { get; set; }
        /// <summary>
        /// 第二联责任人扣分
        /// </summary>
        /// <returns></returns>
        [Column("SECONDDESCORE")]
        public decimal? SECONDDESCORE { get; set; }
        /// <summary>
        /// 第二联责任人考核（元）
        /// </summary>
        /// <returns></returns>
        [Column("SECONDDEMONEY")]
        public double? SECONDDEMONEY { get; set; }
        /// <summary>
        /// 第二联责任人考核（元）
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string REMARK { get; set; }
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