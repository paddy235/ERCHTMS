using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.HighRiskWork.ViewModel
{
    public class FireWaterModel
    {
        #region 实体成员
        /// <summary>
        /// 申请单位
        /// </summary>
        /// <returns></returns>
        public string ApplyDeptCode { get; set; }
        /// <summary>
        /// 申请单位
        /// </summary>
        /// <returns></returns>
        public string ApplyDeptName { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        /// <returns></returns>
        public string ApplyUserId { get; set; }
        /// <summary>
        /// 申请人
        /// </summary>
        /// <returns></returns>
        public string ApplyUserName { get; set; }
        /// <summary>
        /// 使用消防水单位类别(0:电厂内部 1:外包单位)
        /// </summary>
        /// <returns></returns>
        public string WorkDeptType { get; set; }

        /// <summary>
        /// 单位类别
        /// </summary>
        public string WorkDeptTypeName
        {
            get
            {
                if (this.WorkDeptType == "0")
                    return "单位内部";
                return "外包单位";
            }
        }
        /// <summary>
        /// 使用消防水单位
        /// </summary>
        /// <returns></returns>
        public string WorkDeptId { get; set; }
        /// <summary>
        /// 使用消防水单位
        /// </summary>
        /// <returns></returns>
        public string WorkDeptCode { get; set; }
        /// <summary>
        /// 使用消防水单位
        /// </summary>
        /// <returns></returns>
        public string WorkDeptName { get; set; }
        /// <summary>
        /// 工程
        /// </summary>
        /// <returns></returns>
        public string EngineeringId { get; set; }
        /// <summary>
        /// 工程名称
        /// </summary>
        /// <returns></returns>
        public string EngineeringName { get; set; }
        /// <summary>
        /// 使用消防水开始时间
        /// </summary>
        /// <returns></returns>
        public DateTime? WorkStartTime { get; set; }
        /// <summary>
        /// 使用消防水结束时间
        /// </summary>
        /// <returns></returns>
        public DateTime? WorkEndTime { get; set; }
        /// <summary>
        /// 使用消防水区域
        /// </summary>
        /// <returns></returns>
        public string WorkAreaCode { get; set; }
        /// <summary>
        /// 使用消防水区域
        /// </summary>
        /// <returns></returns>
        public string WorkAreaName { get; set; }
        /// <summary>
        /// 使用消防水地点
        /// </summary>
        /// <returns></returns>
        public string WorkPlace { get; set; }
        /// <summary>
        /// 使用消防水内容
        /// </summary>
        /// <returns></returns>
        public string WorkContent { get; set; }
        /// <summary>
        /// 申请状态(0.申请中,1.审核（批）中,2.审核（批）未通过,3.审核（批）通过）
        /// </summary>
        /// <returns></returns>
        public string ApplyState { get; set; }

        /// <summary>
        /// 许可状态文本
        /// </summary>
        public string ApplyStateStr
        {
            get
            {

                if (this.ApplyState == "0")
                    return "申请中";
                if (this.ApplyState == "1")
                    return "审核(批)中";
                if (this.ApplyState == "2")
                    return "审核(批)未通过";
                if (this.ApplyState == "3")
                    return "审核(批)通过";
                return "";
            }
        }
        /// <summary>
        /// 申请编号
        /// </summary>
        /// <returns></returns>
        public string ApplyNumber { get; set; }
        /// <summary>
        /// 流程节点部门
        /// </summary>
        /// <returns></returns>
        public string FlowDeptName { get; set; }
        /// <summary>
        /// 流程节点部门id
        /// </summary>
        /// <returns></returns>
        public string FlowDept { get; set; }
        /// <summary>
        /// 流程节点角色
        /// </summary>
        /// <returns></returns>
        public string FlowRoleName { get; set; }
        /// <summary>
        /// 流程节点角色id
        /// </summary>
        /// <returns></returns>
        public string FlowRole { get; set; }
        /// <summary>
        /// 流程名称
        /// </summary>
        /// <returns></returns>
        public string FlowName { get; set; }
        /// <summary>
        /// 流程id
        /// </summary>
        /// <returns></returns>
        public string FlowId { get; set; }
        /// <summary>
        /// 结果（0：申请 2：审核 3：完成）
        /// </summary>
        /// <returns></returns>
        public string InvestigateState { get; set; }
        /// <summary>
        /// 作业人员
        /// </summary>
        /// <returns></returns>
        public string WorkUserIds { get; set; }
        /// <summary>
        /// 作业人员
        /// </summary>
        /// <returns></returns>
        public string WorkUserNames { get; set; }
        /// <summary>
        /// 使用消防水实际开始时间
        /// </summary>
        /// <returns></returns>
        public DateTime? RealityWorkStartTime { get; set; }
        /// <summary>
        /// 使用消防水实际结束时间
        /// </summary>
        /// <returns></returns>
        public DateTime? RealityWorkEndTime { get; set; }
        /// <summary>
        /// 专业类别
        /// </summary>
        /// <returns></returns>
        public string SpecialtyType { get; set; }
        /// <summary>
        /// 审核备注
        /// </summary>
        /// <returns></returns>
        public string FlowRemark { get; set; }
        /// <summary>
        /// 抄送人
        /// </summary>
        /// <returns></returns>
        public string CopyUserIds { get; set; }
        /// <summary>
        /// 抄送人
        /// </summary>
        /// <returns></returns>
        public string CopyUserNames { get; set; }
        /// <summary>
        /// 短信通知审批人(0:否 1:是)
        /// </summary>
        /// <returns></returns>
        public string IsMessage { get; set; }
        /// <summary>
        /// 使用中应采取措施
        /// </summary>
        /// <returns></returns>
        public string Measure { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        public string Id { get; set; }
        /// <summary>
        /// 创建用户id
        /// </summary>
        /// <returns></returns>
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 修改用户id
        /// </summary>
        /// <returns></returns>
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <returns></returns>
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 创建用户所属部门编码
        /// </summary>
        /// <returns></returns>
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 创建用户所属机构编码
        /// </summary>
        /// <returns></returns>
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 申请单位
        /// </summary>
        /// <returns></returns>
        public string ApplyDeptId { get; set; }

        /// <summary>
        ///专业类别
        /// </summary>
        public string SpecialtyTypeName { get; set; }

        /// <summary>
        /// APP端删除文件的ID，用逗号隔开
        /// </summary>
        public string DeleteFileIds { get; set; }
        /// <summary>
        /// 申请用途
        /// </summary>
        public string WorkUse { get; set; }

        /// <summary>
        /// 作业安全分析
        /// </summary>
        public List<HighRiskRecordEntity> RiskRecord { get; set; }

        /// <summary>
        /// 审核记录
        /// </summary>
        public List<ScaffoldauditrecordEntity> FireWaterAudits { get; set; }

        /// <summary>
        /// 流程步骤
        /// </summary>
        public List<CheckFlowData> CheckFlow { get; set; }
        /// <summary>
        /// 文件
        /// </summary>
        public IList<Photo> cfiles { get; set; }
        /// <summary>
        /// 执行情况文件
        /// </summary>
        public IList<Photo> conditionFiles { get; set; }
        /// <summary>
        /// 执行情况实体 --华润使用
        /// </summary>
        public FireWaterCondition conditionEntity { get; set; }

        /// <summary>
        /// 执行情况
        /// </summary>
        public IList<FireWaterCondition> conditionlist { get; set; }

        /// <summary>
        /// 使用消防工具
        /// </summary>
        public string Tool { get; set; }

        /// <summary>
        /// 选择的消防工具
        /// </summary>
        public string hdTool { get; set; }
        #endregion
    }
}
