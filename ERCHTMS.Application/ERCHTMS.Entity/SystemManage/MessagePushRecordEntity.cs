using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.SystemManage
{
    /// <summary>
    /// 描 述：消息推送记录表表
    /// </summary>
    [Table("BIS_MESSAGEPUSHRECORD")]
    public class MessagePushRecordEntity : BaseEntity
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
        [Column("CREATEUSERORGCODE")]
        public string CREATEUSERORGCODE { get; set; }
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
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
        /// <summary>
        /// 推送代码
        /// </summary>
        /// <returns></returns>
        [Column("PUSHCODE")]
        public string PUSHCODE { get; set; }
        /// <summary>
        /// 业务关联id
        /// </summary>
        /// <returns></returns>
        [Column("RELVANCEID")]
        public string RELVANCEID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// 标记
        /// </summary>
        /// <returns></returns>
        [Column("MARK")]
        public string MARK { get; set; }
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
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
        /// <summary>
        /// 执行次数
        /// </summary>
        /// <returns></returns>
        [Column("EXECNUM")]
        public int? EXECNUM { get; set; }
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
        [Column("SOURCEDATE")]
        public DateTime? SOURCEDATE { get; set; } 
        /// <summary>
        /// 业务流程状态
        /// </summary>
        /// <returns></returns>
        [Column("WORKFLOW")]
        public string WORKFLOW { get; set; }
        /// <summary>
        /// 业务流程状态
        /// </summary>
        /// <returns></returns>
        [Column("PUSHACCOUNT")]
        public string PUSHACCOUNT { get; set; }
        /// <summary>
        /// 推送日期
        /// </summary>
        /// <returns></returns>
        [Column("PUSHDATE")]
        public DateTime? PUSHDATE { get; set; } 
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            var curUser =  OperatorProvider.Provider.Current();
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;
            this.CREATEDATE = DateTime.Now;
            if (null != curUser)
            {
                this.CREATEUSERID = curUser.UserId;
                this.CREATEUSERNAME = curUser.UserName;
                this.CREATEUSERDEPTCODE = curUser.DeptCode;
                this.CREATEUSERORGCODE = curUser.OrganizeCode;
            }
            else 
            {
                this.CREATEUSERID = "System";
                this.CREATEUSERNAME = "系统管理员";
                this.CREATEUSERDEPTCODE = "00";
                this.CREATEUSERORGCODE = "00";
            }
  
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            var curUser = OperatorProvider.Provider.Current();
            this.ID = keyValue;
            this.MODIFYDATE = DateTime.Now;
            if (null != curUser)
            {
                this.MODIFYUSERID = curUser.UserId;
                this.MODIFYUSERNAME = curUser.UserName;
            }
        }
        #endregion
    }
}