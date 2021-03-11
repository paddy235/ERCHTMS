using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HighRiskWork
{
    /// <summary>
    /// 描 述：高风险通用作业申请
    /// </summary>
    [Table("BIS_HIGHRISKCOMMONAPPLY")]
    public class HighRiskCommonApplyEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 申请状态(0.申请中,1.确认中,2.确认未通过,3.审核（批）中,4.审核（批）未通过,5.审核（批）通过）
        /// </summary>
        /// <returns></returns>
        [Column("APPLYSTATE")]
        public string ApplyState { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 作业人员
        /// </summary>
        /// <returns></returns>
        [Column("WORKUSERIDS")]
        public string WorkUserIds { get; set; }
        /// <summary>
        /// 申请单位
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDEPTID")]
        public string ApplyDeptId { get; set; }
        /// <summary>
        /// 作业区域
        /// </summary>
        /// <returns></returns>
        [Column("WORKAREACODE")]
        public string WorkAreaCode { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERID")]
        public string ApplyUserId { get; set; }
        /// <summary>
        /// 作业负责人
        /// </summary>
        /// <returns></returns>
        [Column("WORKDUTYUSERNAME")]
        public string WorkDutyUserName { get; set; }
        /// <summary>
        /// 作业结束时间
        /// </summary>
        /// <returns></returns>
        [Column("WORKENDTIME")]
        public DateTime? WorkEndTime { get; set; }
        /// <summary>
        /// 作业单位类别(0:单位内部 1:外包单位)
        /// </summary>
        /// <returns></returns>
        [Column("WORKDEPTTYPE")]
        public string WorkDeptType { get; set; }
        /// <summary>
        /// 工程名称
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERINGNAME")]
        public string EngineeringName { get; set; }
        /// <summary>
        /// 风险辨识
        /// </summary>
        /// <returns></returns>
        [Column("RISKIDENTIFICATION")]
        public string RiskIdentification { get; set; }
        /// <summary>
        /// 申请单位
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDEPTNAME")]
        public string ApplyDeptName { get; set; }
        /// <summary>
        /// 监护人
        /// </summary>
        /// <returns></returns>
        [Column("WORKTUTELAGEUSERID")]
        public string WorkTutelageUserId { get; set; }
        /// <summary>
        /// 作业地点
        /// </summary>
        /// <returns></returns>
        [Column("WORKPLACE")]
        public string WorkPlace { get; set; }
        /// <summary>
        /// 作业单位
        /// </summary>
        /// <returns></returns>
        [Column("WORKDEPTID")]
        public string WorkDeptId { get; set; }
        /// <summary>
        /// 作业单位
        /// </summary>
        /// <returns></returns>
        [Column("WORKDEPTNAME")]
        public string WorkDeptName { get; set; }
        /// <summary>
        /// 作业类型
        /// </summary>
        /// <returns></returns>
        [Column("WORKTYPE")]
        public string WorkType { get; set; }
        /// <summary>
        /// 工程
        /// </summary>
        /// <returns></returns>
        [Column("ENGINEERINGID")]
        public string EngineeringId { get; set; }
        /// <summary>
        /// 作业人员
        /// </summary>
        /// <returns></returns>
        [Column("WORKUSERNAMES")]
        public string WorkUserNames { get; set; }
        /// <summary>
        /// 作业单位
        /// </summary>
        /// <returns></returns>
        [Column("WORKDEPTCODE")]
        public string WorkDeptCode { get; set; }
        /// <summary>
        /// 创建用户所属部门编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 作业开始时间
        /// </summary>
        /// <returns></returns>
        [Column("WORKSTARTTIME")]
        public DateTime? WorkStartTime { get; set; }
        /// <summary>
        /// 作业负责人
        /// </summary>
        /// <returns></returns>
        [Column("WORKDUTYUSERID")]
        public string WorkDutyUserId { get; set; }
        /// <summary>
        /// 作业区域
        /// </summary>
        /// <returns></returns>
        [Column("WORKAREANAME")]
        public string WorkAreaName { get; set; }
        /// <summary>
        /// 创建用户id
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 监护人
        /// </summary>
        /// <returns></returns>
        [Column("WORKTUTELAGEUSERNAME")]
        public string WorkTutelageUserName { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        /// <returns></returns>
        [Column("APPLYUSERNAME")]
        public string ApplyUserName { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 申请编号
        /// </summary>
        /// <returns></returns>
        [Column("APPLYNUMBER")]
        public string ApplyNumber { get; set; }
        /// <summary>
        /// 申请单位
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDEPTCODE")]
        public string ApplyDeptCode { get; set; }
        /// <summary>
        /// 创建用户所属机构编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 作业内容
        /// </summary>
        /// <returns></returns>
        [Column("WORKCONTENT")]
        public string WorkContent { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 修改用户id
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }

        /// <summary>
        /// 流程ID
        /// </summary>
        /// <returns></returns>
        [Column("FLOWID")]
        public string FlowId { get; set; }

        /// <summary>
        /// 流程名称
        /// </summary>
        /// <returns></returns>
        [Column("FLOWNAME")]
        public string FlowName { get; set; }

        /// <summary>
        /// 流程角色编码/ID
        /// </summary>
        /// <returns></returns>
        [Column("FLOWROLE")]
        public string FlowRole { get; set; }

        /// <summary>
        /// 流程角色名称
        /// </summary>
        /// <returns></returns>
        [Column("FLOWROLENAME")]
        public string FlowRoleName { get; set; }

        /// <summary>
        /// 流程部门编码/ID 
        /// </summary>
        /// <returns></returns>
        [Column("FLOWDEPT")]
        public string FlowDept { get; set; }

        /// <summary>
        /// 流程部门名称
        /// </summary>
        /// <returns></returns>
        [Column("FLOWDEPTNAME")]
        public string FlowDeptName { get; set; }

        /// <summary>
        /// 确认结果
        /// </summary>
        /// <returns></returns>
        [Column("INVESTIGATESTATE")]
        public string InvestigateState { get; set; }


        /// <summary>
        /// 删除文件
        /// </summary>
        /// <returns></returns>
        public string DeleteFileIds { get; set; }

        /// <summary>
        /// 实际作业开始时间
        /// </summary>
        /// <returns></returns>
        [Column("REALITYWORKSTARTTIME")]
        public DateTime? RealityWorkStartTime { get; set; }

        /// <summary>
        /// 实际作业结束时间
        /// </summary>
        /// <returns></returns>
        [Column("REALITYWORKENDTIME")]
        public DateTime? RealityWorkEndTime { get; set; }

        /// <summary>
        /// 风险等级
        /// </summary>
        /// <returns></returns>
        [Column("RISKTYPE")]
        public string RiskType { get; set; }

        /// <summary>
        /// 专业分类
        /// </summary>
        /// <returns></returns>
        [Column("SPECIALTYTYPE")]
        public string SpecialtyType { get; set; }

        /// <summary>
        /// 审核备注
        /// </summary>
        /// <returns></returns>
        [Column("FLOWREMARK")]
        public string FlowRemark { get; set; }

        /// <summary>
        /// 抄送人
        /// </summary>
        /// <returns></returns>
        [Column("COPYUSERIDS")]
        public string CopyUserIds { get; set; }

        /// <summary>
        /// 抄送人
        /// </summary>
        /// <returns></returns>
        [Column("COPYUSERNAMES")]
        public string CopyUserNames { get; set; }

        /// <summary>
        /// 短信通知审批人(0:否 1:是)
        /// </summary>
        /// <returns></returns>
        [Column("ISMESSAGE")]
        public string IsMessage { get; set; }

        /// <summary>
        /// 非工作时段审批
        /// </summary>
        [Column("NONWORKINGAPPROVE")]
        public string NonWorkingApprove { get; set; }

        /// <summary>
        /// 值班部门
        /// </summary>
        [Column("APPROVEDEPT")]
        public string ApproveDept { get; set; }

        /// <summary>
        /// 值班部门ID
        /// </summary>
        [Column("APPROVEDEPTID")]
        public string ApproveDeptId { get; set; }

        /// <summary>
        /// 值班部门Code
        /// </summary>
        [Column("APPROVEDEPTCODE")]
        public string ApproveDeptCode { get; set; }

        /// <summary>
        /// 工作许可人
        /// </summary>
        [Column("WORKLICENSORNAME")]
        public string WorkLicensorName { get; set; }

        /// <summary>
        /// 工作许可人ID
        /// </summary>
        [Column("WORKLICENSORID")]
        public string WorkLicensorId { get; set; }

        /// <summary>
        /// 工作许可人账号
        /// </summary>
        [Column("WORKLICENSORACCOUNT")]
        public string WorkLicensorAccount { get; set; }

        /// <summary>
        /// 指定下一步审核人账号
        /// </summary>
        [Column("NEXTSTEPAPPROVEUSERACCOUNT")]
        public string NextStepApproveUserAccount { get; set; }

        /// <summary>
        /// 工作负责人账号
        /// </summary>
        [Column("WORKDUTYUSERACCOUNT")]
        public string WorkDutyUserAccount { get; set; }

        /// <summary>
        /// 审核人账号
        /// </summary>
        [Column("APPROVEACCOUNT")]
        public string ApproveAccount { get; set; }

        /// <summary>
        /// 流程获取审核人方式
        /// </summary>
        [Column("FLOWAPPLYTYPE")]
        public string FlowApplyType { get; set; }

        /// <summary>
        /// 作业状态 0: 正常作业  1:暂停作业
        /// </summary>
        [Column("WORKOPERATE")]
        public string WorkOperate { get; set; }

        /// <summary>
        /// 电源接入点
        /// </summary>
        [Column("POWERACCESS")]
        public string PowerAccess { get; set; }

        /// <summary>
        /// 电压
        /// </summary>
        [Column("VOLTAGE")]
        public string Voltage { get; set; }

        /// <summary>
        /// 设备管道名称
        /// </summary>
        [Column("PIPELINE")]
        public string PipeLine { get; set; }

        /// <summary>
        /// 介质
        /// </summary>
        [Column("MEDIA")]
        public string Media { get; set; }

        /// <summary>
        /// 压力
        /// </summary>
        [Column("PRESSURE")]
        public string Pressure { get; set; }

        /// <summary>
        /// 装盲板负责人Id
        /// </summary>
        [Column("ZMBDUTYUSERID")]
        public string ZMBDutyUserId { get; set; }

        /// <summary>
        /// 装盲板负责人
        /// </summary>
        [Column("ZMBDUTYUSERNAME")]
        public string ZMBDutyUserName { get; set; }

        /// <summary>
        /// 拆盲板负责人Id
        /// </summary>
        [Column("CMBDUTYUSERID")]
        public string CMBDutyUserId { get; set; }

        /// <summary>
        /// 拆盲板负责人
        /// </summary>
        [Column("CMBDUTYUSERNAME")]
        public string CMBDutyUserName { get; set; }

        /// <summary>
        /// 温度
        /// </summary>
        [Column("TEMPERATURE")]
        public string Temperature { get; set; }

        /// <summary>
        /// 对应工作票编号及内容
        /// </summary>
        [Column("WORKTICKETNOCONTENT")]
        public string WorkTicketNoContent { get; set; }

        /// <summary>
        /// 工作危险分析(JHA)(0:无 1:有)
        /// </summary>
        [Column("DANGERANALYSE")]
        public int? DangerAnalyse { get; set; }

        /// <summary>
        /// 作业安全分析(JSA)(0:无 1:有)
        /// </summary>
        [Column("SAFETYANALYSE")]
        public int? SafetyAnalyse { get; set; }

        /// <summary>
        /// 运行许可人id
        /// </summary>
        [Column("YXPERMITUSERID")]
        public string YXPermitUserId { get; set; }

        /// <summary>
        /// 运行许可人
        /// </summary>
        [Column("YXPERMITUSERNAME")]
        public string YXPermitUserName { get; set; }

        /// <summary>
        /// 值长/值班负责人id
        /// </summary>
        [Column("WATCHUSERID")]
        public string WatchUserId { get; set; }

        /// <summary>
        /// 值长/值班负责人
        /// </summary>
        [Column("WATCHUSERNAME")]
        public string WatchUserName { get; set; }

        /// <summary>
        /// 受影响相关方确认人id
        /// </summary>
        [Column("EFFECTCONFIMERID")]
        public string EffectConfimerId { get; set; }

        /// <summary>
        /// 受影响相关方确认人
        /// </summary>
        [Column("EFFECTCONFIMERNAME")]
        public string EffectConfirmerName { get; set; }
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
}