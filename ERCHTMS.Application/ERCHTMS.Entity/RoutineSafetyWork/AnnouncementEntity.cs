using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using ERCHTMS.Entity.HighRiskWork.ViewModel;

namespace ERCHTMS.Entity.RoutineSafetyWork
{
    /// <summary>
    /// 描 述：通知公告
    /// </summary>
    [Table("BIS_ANNOUNCEMENT")]
    public class AnnouncementEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
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
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        /// <returns></returns>
        [Column("TITLE")]
        public string Title { get; set; }
        /// <summary>
        /// 发布人
        /// </summary>
        /// <returns></returns>
        [Column("PUBLISHER")]
        public string Publisher { get; set; }
        /// <summary>
        /// 发布人ID
        /// </summary>
        /// <returns></returns>
        [Column("PUBLISHERID")]
        public string PublisherId { get; set; }
        /// <summary>
        /// 发布部门
        /// </summary>
        /// <returns></returns>
        [Column("PUBLISHERDEPT")]
        public string PublisherDept  { get; set; }
        /// <summary>
        /// 发布部门ID
        /// </summary>
        /// <returns></returns>
        [Column("PUBLISHERDEPTID")]
        public string PublisherDeptId { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        /// <returns></returns>
        [Column("RELEASETIME")]
        public DateTime? ReleaseTime { get; set; }
        /// <summary>
        /// 是否重要
        /// </summary>
        /// <returns></returns>
        [Column("ISIMPORTANT")]
        public string IsImportant { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        /// <returns></returns>
        [Column("CONTENT")]
        public string Content { get; set; }
        /// <summary>
        /// 是否发送(0是，1否)
        /// </summary>
        /// <returns></returns>
        [Column("ISSEND")]
        public string IsSend { get; set; }
        /// <summary>
        /// 已读人员
        /// </summary>
        /// <returns></returns>
        [Column("USERID")]
        public string UserId { get; set; }
        /// <summary>
        /// 已读人员
        /// </summary>
        /// <returns></returns>
        [Column("USERNAME")]
        public string UserName { get; set; }

        /// <summary>
        /// 发布范围
        /// </summary>
        /// <returns></returns>
        [Column("ISSUERANGEDEPT")]
        public string IssueRangeDept { get; set; }

        /// <summary>
        /// 发布范围
        /// </summary>
        /// <returns></returns>
        [Column("ISSUERANGEDEPTCODE")]
        public string IssueRangeDeptCode { get; set; }

        /// <summary>
        /// 发布范围
        /// </summary>
        /// <returns></returns>
        [Column("ISSUERANGEDEPTNAME")]
        public string IssueRangeDeptName { get; set; }

        /// <summary>
        /// 通知公告类型
        /// </summary>
        /// <returns></returns>
        [Column("NOTICTYPE")]
        public string NoticType { get; set; }

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
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}