using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.SafetyWorkSupervise
{
    /// <summary>
    /// 描 述：安全重点工作督办
    /// </summary>
    [Table("BIS_SAFETYWORKSUPERVISE")]
    public class SafetyworksuperviseEntity : BaseEntity
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
        [Column("AUTOID")]
        public int? Autoid { get; set; }
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
        /// 督办时间
        /// </summary>
        /// <returns></returns>
        [Column("SUPERVISEDATE")]
        public DateTime? SuperviseDate { get; set; }
        /// <summary>
        /// 重点工作任务
        /// </summary>
        /// <returns></returns>
        [Column("WORKTASK")]
        public string WorkTask { get; set; }
        /// <summary>
        /// 责任单位名称
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPTNAME")]
        public string DutyDeptName { get; set; }
        /// <summary>
        /// 责任单位ID
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPTID")]
        public string DutyDeptId { get; set; }
        /// <summary>
        /// 责任单位Code
        /// </summary>
        /// <returns></returns>
        [Column("DUTYDEPTCODE")]
        public string DutyDeptCode { get; set; }
        /// <summary>
        /// 责任人
        /// </summary>
        /// <returns></returns>
        [Column("DUTYPERSON")]
        public string DutyPerson { get; set; }
        /// <summary>
        /// 责任人id
        /// </summary>
        /// <returns></returns>
        [Column("DUTYPERSONID")]
        public string DutyPersonId { get; set; }
        /// <summary>
        /// 督办人
        /// </summary>
        /// <returns></returns>
        [Column("SUPERVISEPERSON")]
        public string SupervisePerson { get; set; }
        /// <summary>
        /// 督办人id
        /// </summary>
        /// <returns></returns>
        [Column("SUPERVISEPERSONID")]
        public string SupervisePersonId { get; set; }
        /// <summary>
        /// 督办单位名称
        /// </summary>
        /// <returns></returns>
        [Column("SUPERVISEDEPTNAME")]
        public string SuperviseDeptName { get; set; }
        /// <summary>
        /// 督办单位ID
        /// </summary>
        /// <returns></returns>
        [Column("SUPERVISEDEPTID")]
        public string SuperviseDeptId { get; set; }
        /// <summary>
        /// 督办单位Code
        /// </summary>
        /// <returns></returns>
        [Column("SUPERVISEDEPTCODE")]
        public string SuperviseDeptCode { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        /// <returns></returns>
        [Column("FINISHDATE")]
        public DateTime? FinishDate { get; set; }
        /// <summary>
        /// 流程状态,0待下发,1办理反馈中,2督办确认中,3已结束
        /// </summary>
        /// <returns></returns>
        [Column("FLOWSTATE")]
        public string FlowState { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id)?Guid.NewGuid().ToString():Id;
            this.CreateDate = CreateDate == null ? DateTime.Now : CreateDate;
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
    /// <summary>
    /// 
    /// </summary>
    public class SuperviseEntity {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int? Autoid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string CreateUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 督办时间
        /// </summary>
        /// <returns></returns>
        public DateTime? SuperviseDate { get; set; }
        /// <summary>
        /// 重点工作任务
        /// </summary>
        /// <returns></returns>
        public string WorkTask { get; set; }
        /// <summary>
        /// 责任单位名称
        /// </summary>
        /// <returns></returns>
        public string DutyDeptName { get; set; }
        /// <summary>
        /// 责任单位ID
        /// </summary>
        /// <returns></returns>
        public string DutyDeptId { get; set; }
        /// <summary>
        /// 责任单位Code
        /// </summary>
        /// <returns></returns>
        public string DutyDeptCode { get; set; }
        /// <summary>
        /// 责任人
        /// </summary>
        /// <returns></returns>
        public string DutyPerson { get; set; }
        /// <summary>
        /// 责任人id
        /// </summary>
        /// <returns></returns>
        public string DutyPersonId { get; set; }
        /// <summary>
        /// 督办人
        /// </summary>
        /// <returns></returns>
        public string SupervisePerson { get; set; }
        /// <summary>
        /// 督办人id
        /// </summary>
        /// <returns></returns>
        public string SupervisePersonId { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        /// <returns></returns>
        public DateTime? FinishDate { get; set; }
        /// <summary>
        /// 流程状态,0待下发,1办理反馈中,2督办确认中,3已结束
        /// </summary>
        /// <returns></returns>
        public string FlowState { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        public string Remark { get; set; }
        /// <summary>
        /// 反馈id
        /// </summary>
        /// <returns></returns>
        public string Fid { get; set; }
        /// <summary>
        /// 历史数据数量
        /// </summary>
        /// <returns></returns>
        public string btgnum { get; set; }
        /// <summary>
        /// 督办确认id
        /// </summary>
        /// <returns></returns>
        public string Cid { get; set; }
        /// <summary>
        /// 完成情况
        /// </summary>
        /// <returns></returns>
        public string FinishInfo { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        /// <returns></returns>
        public DateTime? FeedbackDate { get; set; }
        /// <summary>
        /// 督办结果
        /// </summary>
        /// <returns></returns>
        public string SuperviseResult { get; set; }
        /// <summary>
        /// 督办意见
        /// </summary>
        /// <returns></returns>
        public string SuperviseOpinion { get; set; }
        /// <summary>
        /// 确认时间
        /// </summary>
        /// <returns></returns>
        public DateTime? ConfirmationDate { get; set; }
        /// <summary>
        /// 反馈签名url
        /// </summary>
        /// <returns></returns>
        public string SignUrl { get; set; }
        /// <summary>
        /// 督办确认签名
        /// </summary>
        /// <returns></returns>
        public string SignUrlT { get; set; }
        /// <summary>
        /// 督办单位名称
        /// </summary>
        public string SuperviseDeptName { get; set; }
    }
}