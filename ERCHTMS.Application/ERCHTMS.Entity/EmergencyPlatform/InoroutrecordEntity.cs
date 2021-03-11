using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.EmergencyPlatform
{
    /// <summary>
    /// 描 述：出入库记录
    /// </summary>
    [Table("MAE_INOROUTRECORD")]
    public class InoroutrecordEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        [Column("SUPPLIESID")]
        public string SUPPLIESID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("SUPPLIESCODE")]
        public string SUPPLIESCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DEPARTID")]
        public string DEPARTID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DEPARTNAME")]
        public string DEPARTNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("SUPPLIESTYPE")]
        public string SUPPLIESTYPE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("SUPPLIESTYPENAME")]
        public string SUPPLIESTYPENAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("SUPPLIESNAME")]
        public string SUPPLIESNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("SUPPLIESUNTIL")]
        public string SUPPLIESUNTIL { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("SUPPLIESUNTILNAME")]
        public string SUPPLIESUNTILNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("STORAGEPLACE")]
        public string STORAGEPLACE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("USERID")]
        public string USERID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("USERNAME")]
        public string USERNAME { get; set; }
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
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
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
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MOBILE")]
        public string MOBILE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string REMARK { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("NUM")]
        public int? NUM { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("INOROUTTIME")]
        public DateTime? INOROUTTIME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("OUTREASON")]
        public string OUTREASON { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("OUTREASONNAME")]
        public string OUTREASONNAME { get; set; }
        /// <summary>
        /// 0新增,1出库,2入库
        /// </summary>
        /// <returns></returns>
        [Column("STATUS")]
        public int? STATUS { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("STATUNAME")]
        public string STATUNAME { get; set; }
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
            if (STATUS != 1)
            {
                this.OUTREASON = "";
                this.OUTREASONNAME = "";
            }

            if (this.STATUS == 0)
                this.STATUNAME = "新增";
            if (this.STATUS == 1)
                this.STATUNAME = "出库";
            if (this.STATUS == 2)
                this.STATUNAME = "入库";
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