using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.AccidentEvent
{
    /// <summary>
    /// 描 述：事故事件调查处理
    /// </summary>
    [Table("AEM_BULLETIN_DEAL")]
    public class Bulletin_dealEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 是否提交
        /// </summary>
        [Column("ISSUBMIT_DEAL")]
        public int? IsSubmit_Deal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("SGNAME_DEAL")]
        public string SGNAME_DEAL { get; set; }

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
        [Column("CREATEUSERNAME")]
        public string CREATEUSERNAME { get; set; }
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
        [Column("CREATEUSERORGCODE")]
        public string CREATEUSERORGCODE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("BAQZTNAME")]
        public string BAQZTNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("SGLEVELNAME_DEAL")]
        public string SGLEVELNAME_DEAL { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("RSSHSGTYPENAME_DEAL")]
        public string RSSHSGTYPENAME_DEAL { get; set; }
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
        [Column("SZNUM_DEAL")]
        public int? SZNUM_DEAL { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("RSSHSGTYPE_DEAL")]
        public string RSSHSGTYPE_DEAL { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("SWNUM_DEAL")]
        public int? SWNUM_DEAL { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("HAPPENTIME_DEAL")]
        public DateTime? HAPPENTIME_DEAL { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("BAQXWNAME")]
        public string BAQXWNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("QSNUM_DEAL")]
        public int? QSNUM_DEAL { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ZSNUM_DEAL")]
        public int? ZSNUM_DEAL { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AREAID_DEAL")]
        public string AREAID_DEAL { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("SGLEVEL_DEAL")]
        public string SGLEVEL_DEAL { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("JJYYNAME")]
        public string JJYYNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DEPARTMENTNAME_DEAL")]
        public string DEPARTMENTNAME_DEAL { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("BAQXW")]
        public string BAQXW { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AREA_DEAL")]
        public string AREA_DEAL { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("BAQZT")]
        public string BAQZT { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("BULLETINID")]
        public string BULLETINID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DCBGFILES")]
        public string DCBGFILES { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DEPARTMENTID_DEAL")]
        public string DEPARTMENTID_DEAL { get; set; }
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
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
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
        [Column("JJYY")]
        public string JJYY { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("SGTYPENAME_DEAL")]
        public string SGTYPENAME_DEAL { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("SGTYPE_DEAL")]
        public string SGTYPE_DEAL { get; set; }
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
        [Column("FFCSYYJ")]
        public string FFCSYYJ { get; set; }
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