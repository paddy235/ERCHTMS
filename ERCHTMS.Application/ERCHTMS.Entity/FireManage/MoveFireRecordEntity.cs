using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.FireManage
{
    /// <summary>
    /// 描 述：动火记录（重点防火部位子表）
    /// </summary>
    [Table("HRS_MOVEFIRERECORD")]
    public class MoveFireRecordEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主表ID
        /// </summary>
        /// <returns></returns>
        [Column("MAINID")]
        public string MainId { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int? AutoId { get; set; }
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
        /// 工作票号
        /// </summary>
        /// <returns></returns>
        [Column("WORKTICKET")]
        public string WorkTicket { get; set; }
        /// <summary>
        /// 工作单位
        /// </summary>
        /// <returns></returns>
        [Column("WORKUNIT")]
        public string WorkUnit { get; set; }
        /// <summary>
        /// 工作单位Code
        /// </summary>
        /// <returns></returns>
        [Column("WORKUNITCODE")]
        public string WorkUnitCode { get; set; }
        /// <summary>
        /// 工作地点
        /// </summary>
        /// <returns></returns>
        [Column("WORKSITE")]
        public string WorkSite { get; set; }
        /// <summary>
        /// 执行开始时间
        /// </summary>
        /// <returns></returns>
        [Column("EXECUTESTARTDATE")]
        public DateTime? ExecuteStartDate { get; set; }
        /// <summary>
        /// 执行结束时间
        /// </summary>
        /// <returns></returns>
        [Column("EXECUTEENDDATE")]
        public DateTime? ExecuteEndDate { get; set; }
        /// <summary>
        /// 责任人
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSER")]
        public string DutyUser { get; set; }
        /// <summary>
        /// 责任人ID
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSERID")]
        public string DutyUserId { get; set; }
        /// <summary>
        /// 执行人
        /// </summary>
        /// <returns></returns>
        [Column("EXECUTEUSER")]
        public string ExecuteUser { get; set; }
        /// <summary>
        /// 执行人ID
        /// </summary>
        /// <returns></returns>
        [Column("EXECUTEUSERID")]
        public string ExecuteUserId { get; set; }
        /// <summary>
        /// 登记人
        /// </summary>
        /// <returns></returns>
        [Column("REGISTERUSER")]
        public string RegisterUser { get; set; }
        /// <summary>
        /// 登记人ID
        /// </summary>
        /// <returns></returns>
        [Column("REGISTERUSERID")]
        public string RegisterUserId { get; set; }
        /// <summary>
        /// 登记时间
        /// </summary>
        /// <returns></returns>
        [Column("REGISTERDATE")]
        public DateTime? RegisterDate { get; set; }
        /// <summary>
        /// 动火工作结束时间
        /// </summary>
        /// <returns></returns>
        [Column("WORKENDDATE")]
        public DateTime? WorkEndDate { get; set; }
        /// <summary>
        /// 登记人(动火工作结束信息的登记人)
        /// </summary>
        /// <returns></returns>
        [Column("WORKREGISTERUSER")]
        public string WorkRegisterUser { get; set; }
        /// <summary>
        /// 登记人ID
        /// </summary>
        /// <returns></returns>
        [Column("WORKREGISTERUSERID")]
        public string WorkRegisterUserId { get; set; }
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