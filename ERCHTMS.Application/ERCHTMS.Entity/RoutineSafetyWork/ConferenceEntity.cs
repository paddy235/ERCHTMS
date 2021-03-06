using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.RoutineSafetyWork
{
    /// <summary>
    /// 描 述：安全会议
    /// </summary>
    [Table("BIS_CONFERENCE")]
    public class ConferenceEntity : BaseEntity
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
        /// 会议名称
        /// </summary>
        /// <returns></returns>
        [Column("CONFERENCENAME")]
        public string ConferenceName { get; set; }
        /// <summary>
        /// 主持人
        /// </summary>
        /// <returns></returns>
        [Column("COMPERE")]
        public string Compere { get; set; }
        /// <summary>
        /// 主持人ID
        /// </summary>
        /// <returns></returns>
        [Column("COMPEREID")]
        public string CompereId { get; set; }
        /// <summary>
        /// 召开部门
        /// </summary>
        /// <returns></returns>
        [Column("COMPEREDEPT")]
        public string CompereDept { get; set; }
        /// <summary>
        /// 召开部门
        /// </summary>
        /// <returns></returns>
        [Column("COMPEREDEPTID")]
        public string CompereDeptId { get; set; }
        /// <summary>
        /// 会议时间
        /// </summary>
        /// <returns></returns>
        [Column("CONFERENCETIME")]
        public DateTime? ConferenceTime { get; set; }
        /// <summary>
        /// 地点
        /// </summary>
        /// <returns></returns>
        [Column("LOCALE")]
        public string Locale { get; set; }
        /// <summary>
        /// 主要议题
        /// </summary>
        /// <returns></returns>
        [Column("CONTENT")]
        public string Content { get; set; }
        /// <summary>
        /// 会议应到人数
        /// </summary>
        /// <returns></returns>
        [Column("CONFERENCEPERSON")]
        public string ConferencePerson { get; set; }
        /// <summary>
        /// 会议记录附件Id
        /// </summary>
        /// <returns></returns>
        [Column("CONFERENCEREDID")]
        public string ConferenceRedId { get; set; }
        /// <summary>
        /// 是否发送(0是，1否)
        /// </summary>
        /// <returns></returns>
        [Column("ISSEND")]
        public string IsSend { get; set; }
        /// <summary>
        /// 成员Id
        /// </summary>
        /// <returns></returns>
        [Column("USERID")]
        public string UserId { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        /// <returns></returns>
        [Column("USERNAME")]
        public string UserName { get; set; }
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