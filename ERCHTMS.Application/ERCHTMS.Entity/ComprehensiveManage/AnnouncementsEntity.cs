using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.ComprehensiveManage
{
    /// <summary>
    /// 描 述：通知公告
    /// </summary>
    [Table("HRS_ANNOUNCEMENT")]
    public class AnnouncementsEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        /// <returns></returns>
        [Column("TITLE")]
        public string Title { get; set; }
        /// <summary>
        /// 已阅读（ID集合）
        /// </summary>
        /// <returns></returns>
        [Column("READUSERIDLIST")]
        public string ReadUserIdList { get; set; }
        /// <summary>
        /// 发布人姓名
        /// </summary>
        /// <returns></returns>
        [Column("ISSUERNAME")]
        public string IssuerName { get; set; }
        /// <summary>
        /// 发布范围（接收人姓名集合）
        /// </summary>
        /// <returns></returns>
        [Column("ISSUERUSERNAMELIST")]
        public string IssuerUserNameList { get; set; }
        /// <summary>
        /// 是否重要
        /// </summary>
        /// <returns></returns>
        [Column("ISREMIND")]
        public int? IsRemind { get; set; }
        /// <summary>
        /// 已阅读（姓名集合）
        /// </summary>
        /// <returns></returns>
        [Column("READUSERNAMELIST")]
        public string ReadUserNameList { get; set; }
        /// <summary>
        /// 发布人部门名称
        /// </summary>
        /// <returns></returns>
        [Column("ISSUEDEPTNAME")]
        public string IssueDeptName { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        /// <returns></returns>
        [Column("ISSUETIME")]
        public DateTime? IssueTime { get; set; }
        /// <summary>
        /// 发布范围（接收人ID集合）
        /// </summary>
        /// <returns></returns>
        [Column("ISSUERUSERIDLIST")]
        public string IssuerUserIdList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        /// <returns></returns>
        [Column("CONTENT")]
        public string Content { get; set; }
        /// <summary>
        /// 是否发送
        /// </summary>
        [Column("ISSEND")]
        public string IsSend { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id)?Guid.NewGuid().ToString():Id;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserName = OperatorProvider.Provider.Current().UserName;
            this.CreateUserDeptCode = OperatorProvider.Provider.Current().DeptCode;
            this.CreateUserOrgCode = OperatorProvider.Provider.Current().OrganizeCode;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.Id = keyValue;
                                            }
        #endregion
    }
}