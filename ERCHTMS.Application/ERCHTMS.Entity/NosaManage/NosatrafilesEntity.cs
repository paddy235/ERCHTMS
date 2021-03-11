using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.NosaManage
{
    /// <summary>
    /// 描 述：培训文件
    /// </summary>
    [Table("HRS_NOSATRAFILES")]
    public class NosatrafilesEntity : BSEntity
    {
        #region 实体成员        
        /// <summary>
        /// 文件名称
        /// </summary>
        /// <returns></returns>
        [Column("FILENAME")]
        public string FileName { get; set; }
        /// <summary>
        /// 引用id
        /// </summary>
        /// <returns></returns>
        [Column("REFID")]
        public string RefId { get; set; }
        /// <summary>
        /// 引用名称
        /// </summary>
        /// <returns></returns>
        [Column("REFNAME")]
        public string RefName { get; set; }
        /// <summary>
        /// 发布日期
        /// </summary>
        /// <returns></returns>
        [Column("PUBDATE")]
        public DateTime? PubDate { get; set; }
        /// <summary>
        /// 发布部门id
        /// </summary>
        /// <returns></returns>
        [Column("PUBDEPARTID")]
        public string PubDepartId { get; set; }
        /// <summary>
        /// 发布部门名称
        /// </summary>
        /// <returns></returns>
        [Column("PUBDEPARTNAME")]
        public string PubDepartName { get; set; }
        /// <summary>
        /// 发布人id
        /// </summary>
        /// <returns></returns>
        [Column("PUBUSERID")]
        public string PubUserId { get; set; }
        /// <summary>
        /// 发布人名称
        /// </summary>
        /// <returns></returns>
        [Column("PUBUSERNAME")]
        public string PubUserName { get; set; }
        /// <summary>
        /// 消息接收人id
        /// </summary>
        /// <returns></returns>
        [Column("MSGUSERID")]
        public string MsgUserId { get; set; }
        /// <summary>
        /// 消息接收人姓名
        /// </summary>
        /// <returns></returns>
        [Column("MSGUSERNAME")]
        public string MsgUserName { get; set; }
        /// <summary>
        /// 查阅次数
        /// </summary>
        /// <returns></returns>
        [Column("VIEWTIMES")]
        public int? ViewTimes { get; set; }
        #endregion
    }
}

namespace ERCHTMS.Entity
{
    /// <summary>
    /// 描 述：基础类
    /// </summary>
    public class BSEntity : BaseEntity
    {
        #region 默认字段
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
        [Column("CREATEUSERORGCODE")]
        public string CREATEUSERORGCODE { get; set; }
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
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
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


