using ERCHTMS.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.TwoTickets
{
    /// <summary>
    /// 描 述：两票信息
    /// </summary>
    [Table("XSS_TWOTICKETS")]
    public class TwoTicketsEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 签发人ID
        /// </summary>
        /// <returns></returns>
        [Column("SENDUSERID")]
        public string SendUserId { get; set; }
        /// <summary>
        /// 监护人ID
        /// </summary>
        /// <returns></returns>
        [Column("TUTELAGEUSERID")]
        public string TutelageUserId { get; set; }
        /// <summary>
        /// 登记人
        /// </summary>
        /// <returns></returns>
        [Column("REGISTERUSER")]
        public string RegisterUser { get; set; }
        /// <summary>
        /// 审批人
        /// </summary>
        /// <returns></returns>
        [Column("AUDITUSER1")]
        public string AuditUser1 { get; set; }
        /// <summary>
        /// 审批人
        /// </summary>
        /// <returns></returns>
        [Column("AUDITUSERID1")]
        public string AuditUserId1 { get; set; }
        /// <summary>
        /// 许可人ID
        /// </summary>
        /// <returns></returns>
        [Column("AUDITUSERID")]
        public string AuditUserId { get; set; }
        /// <summary>
        /// 票号
        /// </summary>
        /// <returns></returns>
        [Column("SNO")]
        public string Sno { get; set; }
        /// <summary>
        /// 停送电编号
        /// </summary>
        /// <returns></returns>
        [Column("TSDSNO")]
        public string TsdSno { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        /// <returns></returns>
        [Column("CONTENT")]
        public string Content { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATETIME")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 创建用户所属部门编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 许可人
        /// </summary>
        /// <returns></returns>
        [Column("AUDITUSER")]
        public string AuditUser { get; set; }
        /// <summary>
        /// 开票时间
        /// </summary>
        /// <returns></returns>
        [Column("WORKTIME")]
        public DateTime? WorkTime { get; set; }
        /// <summary>
        /// 创建人ID 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 负责人ID
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSERID")]
        public string DutyUserId { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSER")]
        public string DutyUser { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNAME")]
        public string DeptName { get; set; }
        /// <summary>
        /// 批准工作(结束)时间
        /// </summary>
        /// <returns></returns>
        [Column("REGISTERTIME")]
        public DateTime? RegisterTime { get; set; }
        /// <summary>
        /// 登记人ID
        /// </summary>
        /// <returns></returns>
        [Column("REGISTERUSERID")]
        public string RegisterUserId { get; set; }
        /// <summary>
        /// 监护人
        /// </summary>
        /// <returns></returns>
        [Column("TUTELAGEUSER")]
        public string TutelageUser { get; set; }
        /// <summary>
        /// 签发人
        /// </summary>
        /// <returns></returns>
        [Column("SENDUSER")]
        public string SendUser { get; set; }
        /// <summary>
        /// 部门编码
        /// </summary>
        /// <returns></returns>
        [Column("DEPTCODE")]
        public string DeptCode { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        /// <returns></returns>
        [Column("DEPTID")]
        public string DeptId { get; set; }
        /// <summary>
        /// 地点及设备名称
        /// </summary>
        /// <returns></returns>
        [Column("ADDRESS")]
        public string Address { get; set; }
        /// <summary>
        /// 批准时间
        /// </summary>
        /// <returns></returns>
        [Column("AUDITTIME")]
        public DateTime? AuditTime { get; set; }
        /// <summary>
        /// 票类型
        /// </summary>
        /// <returns></returns>
        [Column("TICKETTYPE")]
        public string TicketType { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        /// <returns></returns>
        [Column("STATUS")]
        public string Status { get; set; }

        /// <summary>
        /// 是否提交（0：未提交，1：已提交）
        /// </summary>
        /// <returns></returns>
        [Column("ISCOMMIT")]
        public int? IsCommit { get; set; }

        /// <summary>
        /// 票分类（1：工作票，2：操作票，3：联系票，4：动火票）
        /// </summary>
        /// <returns></returns>
        [Column("DATATYPE")]
        public string DataType { get; set; }

        /// <summary>
        /// 工作票许可时间
        /// </summary>
        [Column("WORKPERMITTIME")]
        public DateTime? WorkPermitTime { get; set; }

        /// <summary>
        /// 值长/班长
        /// </summary>
        [Column("MONITOR")]
        public string Monitor { get; set; }

        /// <summary>
        /// 值长/班长ID
        /// </summary>
        [Column("MONITORID")]
        public string MonitorId { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
             Operator user = OperatorProvider.Provider.Current();
            if (user!=null)
            {
                this.CreateUserId =string.IsNullOrEmpty(this.CreateUserId)? OperatorProvider.Provider.Current().UserId : this.CreateUserId;
                this.CreateUserName =string.IsNullOrEmpty(this.CreateUserName)? OperatorProvider.Provider.Current().UserName: this.CreateUserName;
                this.CreateUserDeptCode =string.IsNullOrEmpty(this.CreateUserDeptCode)? OperatorProvider.Provider.Current().DeptCode: this.CreateUserDeptCode;
            }
          
            CreateTime = Convert.IsDBNull(this.CreateTime)?DateTime.Now:this.CreateTime;
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
    public class TwoTicketRecordEntity
    {
        /// <summary>
        /// ID 
        /// </summary>
        public string Id { set; get; }
        /// <summary>
        /// 用户
        /// </summary>
        public string HearUser { set; get; }
        /// <summary>
        /// 用户账号
        /// </summary>
        public string HearUserId { set; get; }
        /// <summary>
        /// 时间
        /// </summary>
        public string Time { set; get; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { set; get; }
        /// <summary>
        /// 票据ID
        /// </summary>
        public string TicketId { set; get; }
        /// <summary>
        ///票状态（2：延期，3：消票，4：作废）
        /// </summary>
        public int? Status { set; get; }

        /// <summary>
        /// 是否提交（0：未提交，1：已提交）
        /// </summary>
        public int? IsCommit { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateDate { set; get; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { set; get; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        public string CreateUserId { set; get; }
        
    }
}
