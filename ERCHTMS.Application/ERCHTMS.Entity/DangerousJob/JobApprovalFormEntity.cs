using ERCHTMS.Code;
using ERCHTMS.Entity.HighRiskWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERCHTMS.Entity.DangerousJob
{
    /// <summary>
    /// 描 述：危险作业审批表
    /// </summary>
    [Table("BIS_JOBAPPROVALFORM")]
    public class JobApprovalFormEntity : BaseEntity
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
        /// <returns></retworkoperateurns>
        [Column("AUTOID")]
        public int? Autoid { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建用户所属部门
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 创建用户机构
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 创建用户名称
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 操作用户
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 操作用户名称
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 申请单位id
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDEPTID")]
        public string ApplyDeptId { get; set; }
        /// <summary>
        /// 申请单位code
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDEPTCODE")]
        public string ApplyDeptCode { get; set; }
        /// <summary>
        /// 申请单位名称
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDEPTNAME")]
        public string ApplyDeptName { get; set; }
        /// <summary>
        /// 申请人id
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERID")]
        public string ApplyUserId { get; set; }
        /// <summary>
        /// 申请人名称
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERNAME")]
        public string ApplyUserName { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        /// <returns></returns>
        [Column("APPLYTIME")]
        public DateTime? ApplyTime { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        /// <returns></returns>
        [Column("APPLYNO")]
        public string ApplyNo { get; set; }

        /// <summary>
        /// 危险作业类型
        /// </summary>
        /// <returns></returns>
        [Column("JOBTYPE")]
        public string JobType { get; set; }

        /// <summary>
        /// 危险作业类型名称
        /// </summary>
        /// <returns></returns>
        [Column("JOBTYPENAME")]
        public string JobTypeName { get; set; }

        /// <summary>
        /// 危险作业级别
        /// </summary>
        /// <returns></returns>
        [Column("JOBLEVEL")]
        public string JobLevel { get; set; }
        /// <summary>
        /// 危险作业级别名称
        /// </summary>
        [NotMapped]
        public string JobLevelName { get; set; }
        /// <summary>
        /// 对应作业安全证
        /// </summary>
        /// <returns></returns>
        [Column("JOBSAFETYCARD")]
        public string JobSafetyCard { get; set; }

        /// <summary>
        /// 对应作业安全证ID
        /// </summary>
        /// <returns></returns>
        [Column("JOBSAFETYCARDID")]
        public string JobSafetyCardId { get; set; }
        /// <summary>
        /// 作业单位id
        /// </summary>
        /// <returns></returns>
        [Column("JOBDEPTID")]
        public string JobDeptId { get; set; }

        /// <summary>
        /// 作业单位code
        /// </summary>
        /// <returns></returns>
        [Column("JOBDEPTCODE")]
        public string JobDeptCode { get; set; }
        /// <summary>
        /// 作业单位名称
        /// </summary>
        /// <returns></returns>
        [Column("JOBDEPTNAME")]
        public string JobDeptName { get; set; }
        /// <summary>
        /// 工作票编号
        /// </summary>
        /// <returns></returns>
        [Column("JOBTICKETNO")]
        public string JobTicketNo { get; set; }

        /// <summary>
        /// 计划作业开始时间
        /// </summary>
        /// <returns></returns>
        [Column("JOBSTARTTIME")]
        public DateTime? JobStartTime { get; set; }
        /// <summary>
        /// 作业结束时间
        /// </summary>
        /// <returns></returns>
        [Column("JOBENDTIME")]
        public DateTime? JobEndTime { get; set; }
        /// <summary>
        /// 作业区域id
        /// </summary>
        /// <returns></returns>
        [Column("JOBAREAID")]
        public string JobAreaId { get; set; }
        /// <summary>
        /// 作业区域
        /// </summary>
        /// <returns></returns>
        [Column("JOBAREA")]
        public string JobArea { get; set; }
        /// <summary>
        /// 作业地点
        /// </summary>
        /// <returns></returns>
        [Column("JOBPLACE")]
        public string JobPlace { get; set; }
        /// <summary>
        /// 作业内容
        /// </summary>
        /// <returns></returns>
        [Column("JOBCONTENT")]
        public string JobContent { get; set; }

        ///// <summary>
        ///// 作业高度
        ///// </summary>
        ///// <returns></returns>
        //[NotMapped]
        //public string JobHeight { get; set; }

        /// <summary>
        /// 监护人id
        /// </summary>
        /// <returns></returns>
        [Column("CUSTODIANID")]
        public string CustodianId { get; set; }
        /// <summary>
        /// 监护人名称
        /// </summary>
        /// <returns></returns>
        [Column("CUSTODIAN")]
        public string Custodian { get; set; }
        /// <summary>
        /// 作业人员id
        /// </summary>
        /// <returns></returns>
        [Column("JOBPERSONID")]
        public string JobPersonId { get; set; }
        /// <summary>
        /// 作业人员名称
        /// </summary>
        /// <returns></returns>
        [Column("JOBPERSON")]
        public string JobPerson { get; set; }
        /// <summary>
        /// 危害辨识
        /// </summary>
        /// <returns></returns>
        [Column("DANGEROUSDECIPHER")]
        public string DangerousDecipher { get; set; }
        /// <summary>
        /// 安全措施
        /// </summary>
        /// <returns></returns>
        [Column("SAFETYMEASURES")]
        public string SafetyMeasures { get; set; }
        /// <summary>
        /// 签名url
        /// </summary>
        /// <returns></returns>
        [Column("SIGNURL")]
        public string SignUrl { get; set; }

        /// <summary>
        /// 作业状态，0申请中，1审批中，2审批通过，3已作废，4审批不通过
        /// </summary>
        /// <returns></returns>
        [Column("JOBSTATE")]
        public int? JobState { get; set; }

        /// <summary>
        /// 是否提交,0保存,1提交
        /// </summary>
        /// <returns></returns>
        [Column("ISSUBMIT")]
        public int? IsSubmit { get; set; }
        /// <summary>
        /// 第几次申请
        /// </summary>
        /// <returns></returns>
        [Column("APPLYNUMBER")]
        public int ApplyNumber { get; set; }
        /// <summary>
        /// 逐级审核流程模块名称
        /// </summary>
        [NotMapped]
        public string ModuleName { get; set; }
        /// <summary>
        /// 作废原因
        /// </summary>
        /// <returns></returns>
        [Column("CANCELREASON")]
        public string CancelReason { get; set; }
        /// <summary>
        /// 作废操作人id
        /// </summary>
        /// <returns></returns>
        [Column("CANCELUSERID")]
        public string CancelUserId { get; set; }
        /// <summary>
        /// 作废操作人名称
        /// </summary>
        /// <returns></returns>
        [Column("CANCELUSERNAME")]
        public string CancelUserName { get; set; }
        /// <summary>
        /// 作废操作时间
        /// </summary>
        /// <returns></returns>
        [Column("CANCELTIME")]
        public DateTime? CancelTime { get; set; }
        /// <summary>
        /// 实际作业开始时间
        /// </summary>
        [Column("REALITYJOBSTARTTIME")]
        public DateTime? RealityJobStartTime { get; set; }
        /// <summary>
        /// 实际作业结束时间
        /// </summary>
        [Column("REALITYJOBENDTIME")]
        public DateTime? RealityJobEndTime { get; set; }
        /// <summary>
        /// 作业人数
        /// </summary>
        [Column("JOBNUM")]
        public int? JobNum { get; set; }
        /// <summary>
        /// APP端删除文件的ID，用逗号隔开
        /// </summary>
        [NotMapped]
        public string DeleteFileIds { get; set; }
        /// <summary>
        /// APP端删除文件的ID，用逗号隔开
        /// </summary>
        [NotMapped]
        public IList<dynamic> File { get; set; }
        /// <summary>
        /// 作业状态 0: 正常作业  1:暂停作业
        /// </summary>
        [Column("WORKOPERATE")]
        public string WorkOperate { get; set; }
        /// <summary>
        /// 获取流程记录
        /// </summary>
        [NotMapped]
        public IList<CheckInfoEntity> CheckInfo { get; set; }
        /// <summary>
        /// 获取审核信息
        /// </summary>
        [NotMapped]
        public List<checkperson> Items { get; set; }
        /// <summary>
        /// 获取执行信息记录
        /// </summary>
        [NotMapped]
        public List<FireWaterCondition> conditionitems { get; set; }
        /// <summary>
        /// 获取流程图
        /// </summary>
        [NotMapped]
        public List<ERCHTMS.Entity.HighRiskWork.ViewModel.CheckFlowData> checkflow { get; set; }
        /// <summary>
        /// 审批操作人名称
        /// </summary>
        [NotMapped]
        public string OperatorName { get; set; }
        /// <summary>
        /// 审批操作人ID
        /// </summary>
        [NotMapped]
        public string OperatorId { get; set; }
        /// <summary>
        /// 审批操作人账号
        /// </summary>
        [NotMapped]
        public string OperatorAccount { get; set; }
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
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
    /// <summary>
    /// 审核记录
    /// </summary>
    public class CheckInfoEntity
    {
        /// <summary>
        /// 审核部门
        /// </summary>
        public string ApproveDeptName { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string ApprovePerson { get; set; }
        /// <summary>
        /// 审核结果
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 审核意见
        /// </summary>

        public string ApproveOpinion { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public string ApproveTime { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string SignUrl { get; set; }
    }
}
