using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.AccidentEvent
{
    /// <summary>
    /// 描 述：WSSJBG
    /// </summary>
    [Table("AEM_BULLETIN_DEAL")]
    public class WSSJBGEntity : BaseEntity
    {
        #region 实体成员


        /// <summary>
        /// 是否提交
        /// </summary>
        [Column("ISSUBMIT")]
        public int? IsSubmit { get; set; }

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
        [Column("WSSJNAME")]
        public string WSSJNAME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("WSSJTYPE")]
        public string WSSJTYPE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("HAPPENTIME")]
        public DateTime? HAPPENTIME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AREAID")]
        public string AREAID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AREANAME")]
        public string AREANAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("JYJG")]
        public string JYJG { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("HGHYX")]
        public string HGHYX { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("YYCBPD")]
        public string YYCBPD { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CQDCS")]
        public string CQDCS { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("WSSJTPSP")]
        public string WSSJTPSP { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("WSSJBGUSERID")]
        public string WSSJBGUSERID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("WSSJBGUSERNAME")]
        public string WSSJBGUSERNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("TBTIME")]
        public string TBTIME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("BGDEPARTID")]
        public string BGDEPARTID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("BGDEPARTNAME")]
        public string BGDEPARTNAME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("WSSJTYPENAME")]
        public string WSSJTYPENAME { get; set; }
   

     
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
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }

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