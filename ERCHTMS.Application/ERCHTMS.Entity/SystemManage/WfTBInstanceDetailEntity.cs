using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.SystemManage
{
    /// <summary>
    /// 描 述：流程业务明细
    /// </summary>
    [Table("SYS_WFTBINSTANCEDETAIL")]
    public class WfTBInstanceDetailEntity : BaseEntity
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
        [Column("INSTANCEID")]
        public string INSTANCEID { get; set; }
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
        [Column("BUSINESSTYPE")]
        public string BUSINESSTYPE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSER")]
        public string CREATEUSER { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTID")]
        public string CREATEUSERDEPTID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("OPERUSER")]
        public string OPERUSER { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("BUSINESSID")]
        public string BUSINESSID { get; set; }
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
        [Column("TOACTIVITYID")]
        public string TOACTIVITYID { get; set; }
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
        [Column("PREVPARTICIPANTNAMES")]
        public string PREVPARTICIPANTNAMES { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("OPERDATE")]
        public DateTime? OPERDATE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("TEMPUSERS")]
        public string TEMPUSERS { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("FROMACTIVITYID")]
        public string FROMACTIVITYID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("STATE")]
        public int? STATE { get; set; }
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
        [Column("PARTICIPANTNAME")]
        public string PARTICIPANTNAME { get; set; }
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
        [Column("PREVPARTICIPANTS")]
        public string PREVPARTICIPANTS { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("PARTICIPANT")]
        public string PARTICIPANT { get; set; }
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
            this.CREATEUSERDEPTCODE = OperatorProvider.Provider.Current().DeptCode;
            this.CREATEUSERDEPTID = OperatorProvider.Provider.Current().DeptId;
            this.CREATEUSER = OperatorProvider.Provider.Current().Account;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ID = keyValue;
            this.OPERDATE = DateTime.Now;
            this.OPERUSER = OperatorProvider.Provider.Current().Account;
        }
        #endregion
    }
}