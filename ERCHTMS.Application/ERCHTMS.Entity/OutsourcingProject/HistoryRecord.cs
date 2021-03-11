using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Code;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// 资质审查历史记录表
    /// </summary>
    [Table("EPG_HISTORYRECORD")]
    public class HistoryRecord : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 历史资质审查Id
        /// </summary>
        [Column("HISTORYZZSCID")]
        public string HistoryzzscId { get; set; }
        /// <summary>
        /// 审核历史记录Id
        /// </summary>
        [Column("HISTORYAUDITID")]
        public string HistoryauditId { get; set; }
         /// <summary>
        /// 申请人Id
        /// </summary>
        [Column("APPLYPEOPLEID")]
        public string APPLYPEOPLEID { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        [Column("APPLYPEOPLE")]
        public string APPLYPEOPLE { get; set; }
            /// <summary>
        /// 申请时间
        /// </summary>
        [Column("APPLYDATE")]
        public DateTime? APPLYDATE { get; set; }

           /// <summary>
        /// 资质审查Id
        /// </summary>
        [Column("ZZSCID")]
        public string Zzscid { get; set; }
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;
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
            this.ID = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
    }
}
