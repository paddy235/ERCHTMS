using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
 
namespace ERCHTMS.Entity.ComprehensiveManage
{
    /// <summary>
    /// 描 述：sdf
    /// </summary>
    [Table("HRS_MEETINGRECORD")]
    public class MeetingRecordEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 纪要整理人员
        /// </summary>
        /// <returns></returns>
        [Column("SETTLEPERSON")]
        public string SettlePerson { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        /// <returns></returns>
        [Column("ADDRESS")]
        public string Address { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        /// <returns></returns>
        [Column("CONTENT")]
        public string Content { get; set; }
        /// <summary>
        /// 是否发送 0 否 1 是
        /// </summary>
        /// <returns></returns>
        [Column("ISSEND")]
        public string IsSend { get; set; }
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
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 已阅读（ID集合）
        /// </summary>
        /// <returns></returns>
        [Column("READUSERIDLIST")]
        public string ReadUserIdList { get; set; }
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
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        /// <returns></returns>
        [Column("NAME")]
        public string Name { get; set; }
        /// <summary>
        /// 参会人员
        /// </summary>
        /// <returns></returns>
        [Column("ATTENDPERSON")]
        public string AttendPerson { get; set; }
        /// <summary>
        /// 主持
        /// </summary>
        /// <returns></returns>
        [Column("DIRECT")]
        public string Direct { get; set; }
        /// <summary>
        /// 会议时间
        /// </summary>
        /// <returns></returns>
        [Column("MEETINGTIME")]
        public DateTime? MeetingTime { get; set; }
        /// <summary>
        /// 已阅读（姓名集合）
        /// </summary>
        /// <returns></returns>
        [Column("READUSERNAMELIST")]
        public string ReadUserNameList { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        /// <returns></returns>
        [Column("ISSUETIME")]
        public DateTime? IssueTime { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        /// <returns></returns>
        [Column("CODE")]
        public string Code { get; set; }
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
        /// 参会人数
        /// </summary>
        /// <returns></returns>
        [Column("PERSONNUM")]
        public string PersonNum { get; set; }
        /// <summary>
        /// 发布范围（接收人姓名集合）
        /// </summary>
        /// <returns></returns>
        [Column("ISSUERUSERNAMELIST")]
        public string IssuerUserNameList { get; set; }
        /// <summary>
        /// 密级
        /// </summary>
        /// <returns></returns>
        [Column("SECURITY")]
        public string Security { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
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
