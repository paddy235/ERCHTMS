using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace ERCHTMS.Entity.DangerousJob
{
    /// <summary>
    /// 描 述：作业安全证申请
    /// </summary>
    [Table("BIS_JOBSAFETYCARDAPPLY")]
    public class JobSafetyCardApplyEntity : BaseEntity
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
        /// 工作票编号
        /// </summary>
        /// <returns></returns>
        [Column("JOBTICKETNO")]
        public string JobTicketNo { get; set; }
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
        /// 作业开始时间/计划断路开始时间
        /// </summary>
        /// <returns></returns>
        [Column("JOBSTARTTIME")]
        public DateTime? JobStartTime { get; set; }
        /// <summary>
        /// 作业结束时间/计划断路结束时间
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
        /// 作业地点/吊装地点
        /// </summary>
        /// <returns></returns>
        [Column("JOBPLACE")]
        public string JobPlace { get; set; }
        /// <summary>
        /// 作业内容/共用
        /// </summary>
        /// <returns></returns>
        [Column("JOBCONTENT")]
        public string JobContent { get; set; }
        /// <summary>
        /// 作业级别
        /// </summary>
        /// <returns></returns>
        [Column("JOBLEVEL")]
        public string JobLevel { get; set; }
        /// <summary>
        /// 作业高度
        /// </summary>
        /// <returns></returns>
        [Column("JOBHEIGHT")]
        public string JobHeight { get; set; }
        /// <summary>
        /// 监护人id/安全监护人id/监护人（堵）id
        /// </summary>
        /// <returns></returns>
        [Column("CUSTODIANID")]
        public string CustodianId { get; set; }
        /// <summary>
        /// 监护人名称/安全监护人名称/监护人（堵）名称
        /// </summary>
        /// <returns></returns>
        [Column("CUSTODIAN")]
        public string Custodian { get; set; }
        /// <summary>
        /// 作业人员id/吊装人员id/作业人员（堵）id
        /// </summary>
        /// <returns></returns>
        [Column("JOBPERSONID")]
        public string JobPersonId { get; set; }
        /// <summary>
        /// 作业人员名称/吊装人员名称/作业人员（堵）
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
        /// 作业类型
        /// </summary>
        /// <returns></returns>
        [Column("JOBTYPE")]
        public string JobType { get; set; }
        /// <summary>
        /// 作业类型
        /// </summary>
        /// <returns></returns>
        [Column("JOBTYPENAME")]
        public string JobTypeName { get; set; }
        /// <summary>
        /// 作业状态，申请中：0 审核中:1  审核不通过：2 措施确认中：3  停电中：4 备案中：5 验收中：6 送电中：7 开始作业：8 作业暂停：9 作业中：10 流程结束：11
        /// </summary>
        /// <returns></returns>
        [Column("JOBSTATE")]
        public int JobState { get; set; }
        /// <summary>
        /// 吊装设备设施名称
        /// </summary>
        /// <returns></returns>
        [Column("HOISTINGEQUNAME")]
        public string HoistingEquName { get; set; }
        /// <summary>
        /// 起吊重物质量
        /// </summary>
        /// <returns></returns>
        [Column("WEIGHTQUALITY")]
        public string WeightQuality { get; set; }
        /// <summary>
        /// 吊装指挥id
        /// </summary>
        /// <returns></returns>
        [Column("HOISTINGCOMMANDID")]
        public string HoistingCommandId { get; set; }
        /// <summary>
        /// 吊装指挥
        /// </summary>
        /// <returns></returns>
        [Column("HOISTINGCOMMAND")]
        public string HoistingCommand { get; set; }
        /// <summary>
        /// 特殊工种作业证号
        /// </summary>
        /// <returns></returns>
        [Column("WORKTYPECARD")]
        public string WorkTypeCard { get; set; }
        /// <summary>
        /// 动土范围内容
        /// </summary>
        /// <returns></returns>
        [Column("BREAKGROUNDCONTENT")]
        public string BreakGroundContent { get; set; }
        /// <summary>
        /// 断路原因
        /// </summary>
        /// <returns></returns>
        [Column("BREAKROADREASON")]
        public string BreakRoadReason { get; set; }
        /// <summary>
        /// 示意图附件id
        /// </summary>
        /// <returns></returns>
        [Column("SKETCHMAP")]
        public string SketchMap { get; set; }
        /// <summary>
        /// 实际作业开始时间
        /// </summary>
        /// <returns></returns>
        [Column("REALITYJOBSTARTTIME")]
        public DateTime? RealityJobStartTime { get; set; }
        /// <summary>
        /// 实际作业结束时间
        /// </summary>
        /// <returns></returns>
        [Column("REALITYJOBENDTIME")]
        public DateTime? RealityJobEndTime { get; set; }
        /// <summary>
        /// 是否提交,0保存,1提交
        /// </summary>
        /// <returns></returns>
        [Column("ISSUBMIT")]
        public int IsSubmit { get; set; }
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
        public string ModuleNo { get; set; }
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
        /// 备案人
        /// </summary>
        [Column("RECORDSPERSON")]
        public string RecordsPerson { get; set; }

        /// <summary>
        /// 备案人Id
        /// </summary>
        [Column("RECORDSPERSONID")]
        public string RecordsPersonId { get; set; }

        /// <summary>
        /// 验收人
        /// </summary>
        [Column("CHECKPERSON")]
        public string CheckPerson { get; set; }

        /// <summary>
        /// 验收人Id
        /// </summary>
        [Column("CHECKPERSONID")]
        public string CheckPersonId { get; set; }

        /// <summary>
        /// 责任人
        /// </summary>
        [Column("DUTYPERSON")]
        public string DutyPerson { get; set; }

        /// <summary>
        /// 责任人Id
        /// </summary>
        [Column("DUTYPERSONID")]
        public string DutyPersonId { get; set; }

        /// <summary>
        /// 动火部位单位
        /// </summary>
        [Column("WHENHOTDEPTNAME")]
        public string WhenHotDeptName { get; set; }

        /// <summary>
        /// 动火部门单位Id
        /// </summary>
        [Column("WHENHOTDEPTID")]
        public string WhenHotDeptId { get; set; }

        /// <summary>
        /// 动火部门单位Code
        /// </summary>
        [Column("WHENHOTDEPTCODE")]
        public string WhenHotDeptCode { get; set; }

        /// <summary>
        /// 设备管道名称
        /// </summary>
        [Column("EQUPIPELINENAME")]
        public string EquPipelineName { get; set; }

        /// <summary>
        /// 介质
        /// </summary>
        [Column("MEDIA")]
        public string Media { get; set; }

        /// <summary>
        /// 温度
        /// </summary>
        [Column("TEMPERATURE")]
        public string Temperature { get; set; }

        /// <summary>
        /// 压力
        /// </summary>
        [Column("PRESSURE")]
        public string Pressure { get; set; }

        /// <summary>
        /// 监护人（抽）名称
        /// </summary>
        /// <returns></returns>
        [Column("CUSTODIANID2")]
        public string CustodianId2 { get; set; }
        /// <summary>
        /// 监护人（抽）名称
        /// </summary>
        /// <returns></returns>
        [Column("CUSTODIAN2")]
        public string Custodian2 { get; set; }
        /// <summary>
        /// 作业人员（抽）
        /// </summary>
        /// <returns></returns>
        [Column("JOBPERSONID2")]
        public string JobPersonId2 { get; set; }
        /// <summary>
        /// 作业人员（抽）
        /// </summary>
        /// <returns></returns>
        [Column("JOBPERSON2")]
        public string JobPerson2 { get; set; }

        /// <summary>
        /// 作业中可能产生的有害物质
        /// </summary>
        [Column("HARMFULSUBSTANCE")]
        public string HarmfulSubstance { get; set; }

        /// <summary>
        /// 受限空间所在单位
        /// </summary>
        [Column("LIMITEDSPACEDEPT")]
        public string LimitedSpaceDept { get; set; }

        /// <summary>
        /// 受限空间所在单位Code
        /// </summary>
        [Column("LIMITEDSPACEDEPTCODE")]
        public string LimitedSpaceDeptCode { get; set; }

        /// <summary>
        /// 受限空间所在单位Id
        /// </summary>
        [Column("LIMITEDSPACEDEPTID")]
        public string LimitedSpaceDeptId { get; set; }

        /// <summary>
        /// 受限空间名称
        /// </summary>
        [Column("LIMITEDSPACENAME")]
        public string LimitedSpaceName { get; set; }

        /// <summary>
        /// 受限空间主要介质
        /// </summary>
        [Column("LIMITEDSPACEMEDIA")]
        public string LimitedSpaceMedia { get; set; }

        /// <summary>
        /// 措施确认人
        /// </summary>
        [Column("MEASUREPERSON")]
        public string MeasurePerson { get; set; }

        /// <summary>
        /// 措施确认人Id
        /// </summary>
        [Column("MEASUREPERSONID")]
        public string MeasurePersonId { get; set; }

        /// <summary>
        /// 确认措施
        /// </summary>
        [Column("CONFIRMMEASURES")]
        public string ConfirmMeasures { get; set; }

        /// <summary>
        /// 有毒有害介质分析标准
        /// </summary>
        [Column("DANGEROUSSTANDARD")]
        public string DangerousStandard { get; set; }

        /// <summary>
        /// 可燃气分析标准
        /// </summary>
        [Column("GASSTANDARD")]
        public string GasStandard { get; set; }

        /// <summary>
        /// 氧含量分析标准
        /// </summary>
        [Column("OXYGENCONTENTSTANDARD")]
        public string OxygenContentStandard { get; set; }

        /// <summary>
        /// 停电人员
        /// </summary>
        [Column("POWERCUTPERSON")]
        public string PowerCutPerson { get; set; }

        /// <summary>
        /// 停电人员Id
        /// </summary>
        [Column("POWERCUTPERSONID")]
        public string PowerCutPersonId { get; set; }

        /// <summary>
        /// 送电人员
        /// </summary>
        [Column("POWERGIVEPERSON")]
        public string PowerGivePerson { get; set; }

        /// <summary>
        /// 送电人员id
        /// </summary>
        [Column("POWERGIVEPERSONID")]
        public string PowerGivePersonId { get; set; }

        /// <summary>
        /// 措施确认签名
        /// </summary>
        [Column("CONFIRMSIGNURL")]
        public string ConfirmSignUrl { get; set; }

        /// <summary>
        /// 附加确认安全措施内容
        /// </summary>
        [Column("CONFIRMMEASURECONTENT")]
        public string ConfirmMeasureContent { get; set; }

        /// <summary>
        /// 附加安全措施内容
        /// </summary>
        [Column("MEASURECONTENT")]
        public string MeasureContent { get; set; }

        /// <summary>
        /// 盲板抽堵信息
        /// </summary>
        [NotMapped]
        public List<BlindPlateWallSpecEntity> spec { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        [NotMapped]
        public string approvename { get; set; }
        /// <summary>
        /// 审核人id
        /// </summary>
        [NotMapped]
        public string approveid { get; set; }
        /// <summary>
        /// 审核人账号
        /// </summary>
        [NotMapped]
        public string approveaccount { get; set; }
        /// <summary>
        /// 作业分析数据
        /// </summary>
        [NotMapped]
        public List<WhenHotAnalysisEntity> AnalysisData { get; set; }

        /// <summary>
        /// 对应作业审批单id
        /// </summary>
        [NotMapped]
        public string approveformid { get; set; }

        /// <summary>
        /// 流程基础信息
        /// </summary>
        [NotMapped]
        public List<checkperson> arr { get; set; }

        /// <summary>
        /// 作业级别
        /// </summary>
        /// <returns></returns>
        [NotMapped]
        public string JobLevelName { get; set; }
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

    public class checkperson {
        /// <summary>
        /// 逐级审核流程id
        /// </summary>
        public string id { get; set; }
        public string userid { get; set; }
        public string username { get; set; }
        public string account { get; set; }
        /// <summary>
        /// 保存到高风险作业流程id
        /// </summary>
        public string workid { get; set; }
        /// <summary>
        /// 流程名称
        /// </summary>
        public string flowname { get; set; }

        /// <summary>
        /// 选择人标题
        /// </summary>
        public string choosepersontitle { get; set; }

        /// <summary>
        /// 选择人提示语
        /// </summary>
        public string choosepersonwarn { get; set; }
    }
}
