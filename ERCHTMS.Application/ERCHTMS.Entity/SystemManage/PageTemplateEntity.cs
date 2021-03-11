using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.SystemManage
{
    /// <summary>
    /// 描 述：模板页面管理表
    /// </summary>
    [Table("BIS_PAGETEMPLATE")]
    public class PageTemplateEntity : BaseEntity
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
        /// <summary>
        /// 所属单位id
        /// </summary>
        /// <returns></returns>
        [Column("ORGANIZEID")]
        public string ORGANIZEID { get; set; }
        /// <summary>
        /// 所属单位名称
        /// </summary>
        /// <returns></returns>
        [Column("ORGANIZENAME")]
        public string ORGANIZENAME { get; set; }
        /// <summary>
        /// 检查人姓名
        /// </summary>
        /// <returns></returns>
        [Column("ISENABLE")]
        public string ISENABLE { get; set; }
        /// <summary>
        /// 模板内容
        /// </summary>
        /// <returns></returns>
        [Column("TEMPLATECONTENT")]
        public string TEMPLATECONTENT { get; set; }
        /// <summary>
        /// 模块名称
        /// </summary>
        /// <returns></returns>
        [Column("MODULENAME")]
        public string MODULENAME { get; set; }

        /// <summary>
        /// 相对地址
        /// </summary>
        /// <returns></returns>
        [Column("RELATIVEPATH")]
        public string RELATIVEPATH { get; set; }
        /// <summary>
        /// 模板名称
        /// </summary>
        /// <returns></returns>
        [Column("TEMPLATENAME")]
        public string TEMPLATENAME { get; set; }
        /// <summary>
                /// 文件名称
                /// </summary>
                /// <returns></returns>
        [Column("FILENAME")]
        public string FILENAME { get; set; }
        /// <summary>
                /// 模板代码
                /// </summary>
                /// <returns></returns>
        [Column("TEMPLATECODE")]
        public string TEMPLATECODE { get; set; }
        /// <summary>
                /// 模板类型
                /// </summary>
                /// <returns></returns>
        [Column("TEMPLATETYPE")]
        public string TEMPLATETYPE { get; set; }
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