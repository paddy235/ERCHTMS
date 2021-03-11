using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.HighRiskWork.ViewModel
{
    /// <summary>
    /// 脚手架页面实体
    /// </summary>
    public class ScaffoldModel
    {

        #region 实体成员
        /// <summary>
        /// 主键ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// APP端删除文件的ID，用逗号隔开
        /// </summary>
        public string DeleteFileIds { get; set; }
        /// <summary>
        /// 申请单位名称
        /// </summary>
        public string ApplyCompanyName { get; set; }
        /// <summary>
        /// 申请单位ID
        /// </summary>
        /// <returns></returns>
        public string ApplyCompanyId { get; set; }

        /// <summary>
        /// 申请单位Code
        /// </summary>
        public string ApplyCompanyCode { get; set; }

        /// <summary>
        /// 申请单位名称
        /// </summary>
        public string ApplyUserName { get; set; }
        /// <summary>
        /// 申请人ID
        /// </summary>
        /// <returns></returns>
        public string ApplyUserId { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        /// <returns></returns>
        public string ApplyDate { get; set; }
        /// <summary>
        /// 申请编号
        ///类型首字母+年份+3位数（如J2018001、J2018002）
        /// </summary>
        /// <returns></returns>
        public string ApplyCode { get; set; }
        /// <summary>
        /// 单位类别(0-单位内部 1-外包单位)
        /// </summary>
        /// <returns></returns>
        public int? SetupCompanyType { get; set; }

        /// <summary>
        /// 单位类别
        /// </summary>
        public string SetupCompanyTypeStr
        {
            get
            {
                if (this.SetupCompanyType == 0)
                    return "单位内部";
                return "外包单位";
            }
        }
        /// <summary>
        /// 脚手架搭设类型
        ///0-6米以下脚手架搭设申请
        ///1-6米以上脚手架搭设申请
        /// </summary>
        /// <returns></returns>
        public int? SetupType { get; set; }

        public string SetupTypeName { get; set; }

        /// <summary>
        /// 脚手架高度最小值
        /// </summary>
        public double min { get; set; }

        /// <summary>
        /// 脚手架高度最大值
        /// </summary>
        public double max { get; set; }

        public string SetupTypeStr
        {
            get
            {
                if (this.SetupType == 0)
                {
                    return "6米以下脚手架" + this.ScaffoldTypeStr;
                }
                else
                {
                    return "6米以上脚手架" + this.ScaffoldTypeStr;
                }
            }
        }
        /// <summary>
        /// 使用单位ID
        /// </summary>
        /// <returns></returns>
        public string SetupCompanyId { get; set; }

        /// <summary>
        /// 使用单位代码 
        /// </summary>
        public string SetupCompanyCode { get; set; }
        /// <summary>
        /// 使用单位名称
        /// </summary>
        public string SetupCompanyName { get; set; }

        /// <summary>
        /// 搭设/拆除单位ID
        /// </summary>
        /// <returns></returns>
        public string SetupCompanyId1 { get; set; }

        /// <summary>
        /// 搭设/拆除单位代码 
        /// </summary>
        public string SetupCompanyCode1 { get; set; }
        /// <summary>
        /// 搭设/拆除单位名称
        /// </summary>
        public string SetupCompanyName1 { get; set; }


        /// <summary>
        /// 工程ID
        /// </summary>
        /// <returns></returns>
        public string OutProjectId { get; set; }

        /// <summary>
        /// 工程名称
        /// </summary>
        public string OutProjectName { get; set; }

        /// <summary>
        /// 搭设开始时间
        ///验收中为申请搭设时间
        /// </summary>
        /// <returns></returns>
        public DateTime? SetupStartDate { get; set; }
        /// <summary>
        /// 搭设结束时间

        ///验收中为申请搭设时间
        /// </summary>
        /// <returns></returns>
        public DateTime? SetupEndDate { get; set; }
        /// <summary>
        /// 作业区域
        /// </summary>
        /// <returns></returns>
        public string WorkArea { get; set; }

        /// <summary>
        /// 作业区域Code
        /// </summary>
        public string WorkAreaCode { get; set; }
        /// <summary>
        /// 搭设地点
        /// </summary>
        /// <returns></returns>
        public string SetupAddress { get; set; }
        /// <summary>
        /// 搭设负责人ID
        /// </summary>
        /// <returns></returns>
        public string SetupChargePersonIds { get; set; }
        /// <summary>
        /// 搭设负责人
        /// </summary>
        /// <returns></returns>
        public string SetupChargePerson { get; set; }
        /// <summary>
        /// 搭设人员ID
        ///人员多选，使用“，”号分隔
        /// </summary>
        /// <returns></returns>
        public string SetupPersonIds { get; set; }
        /// <summary>
        /// 搭设人员
        ///人员多选，使用“，”号分隔
        /// </summary>
        /// <returns></returns>
        public string SetupPersons { get; set; }
        /// <summary>
        /// 抄送人员
        /// </summary>
        public string CopyUserNames { get; set; }
        /// <summary>
        /// 抄送人员ID
        /// </summary>
        public string CopyUserIds { get; set; }
        /// <summary>
        /// 脚手架用途
        /// </summary>
        /// <returns></returns>
        public string Purpose { get; set; }
        /// <summary>
        /// 脚手架参数
        /// </summary>
        /// <returns></returns>
        public string Parameter { get; set; }
        /// <summary>
        /// 预计拆除时间
        /// </summary>
        /// <returns></returns>
        public DateTime? ExpectDismentleDate { get; set; }
        /// <summary>
        /// 要求拆除时间
        /// </summary>
        /// <returns></returns>
        public DateTime? DemandDismentleDate { get; set; }
        /// <summary>
        /// 实际搭设开始时间
        /// </summary>
        /// <returns></returns>
        public DateTime? ActSetupStartDate { get; set; }
        /// <summary>
        /// 实际搭设结束时间
        /// </summary>
        /// <returns></returns>
        public DateTime? ActSetupEndDate { get; set; }
        /// <summary>
        /// 拆除开始日期
        /// </summary>
        /// <returns></returns>
        public DateTime? DismentleStartDate { get; set; }
        /// <summary>
        /// 拆除结束日期
        /// </summary>
        /// <returns></returns>
        public DateTime? DismentleEndDate { get; set; }
        /// <summary>
        /// 拆除部位
        /// </summary>
        /// <returns></returns>
        public string DismentlePart { get; set; }
        /// <summary>
        /// 拆除原因
        /// </summary>
        /// <returns></returns>
        public string DismentleReason { get; set; }
        /// <summary>
        /// 拆除人员ID
        /// </summary>
        /// <returns></returns>
        public string DismentlePersonsIds { get; set; }
        /// <summary>
        /// 拆除人员
        /// </summary>
        /// <returns></returns>
        public string DismentlePersons { get; set; }
        /// <summary>
        /// 架体材质
        /// </summary>
        /// <returns></returns>
        public string FrameMaterial { get; set; }
        /// <summary>
        ///搭设信息ID
        ///脚手架搭设信息ID，保留字段，验收及拆除时
        ///存入用户选择的信息ID
        /// </summary>
        /// <returns></returns>
        public string SetupInfoId { get; set; }
        /// <summary>
        ///搭设信息Code
        /// </summary>
        /// <returns></returns>
        public string SetupInfoCode { get; set; }
        /// <summary>
        /// 脚手架类型
        ///0-搭设申请
        ///1-验收申请
        ///2-拆除申请
        /// </summary>
        /// <returns></returns>
        public int? ScaffoldType { get; set; }

        public string ScaffoldTypeStr
        {
            get
            {
                if (this.ScaffoldType == 0)
                    return "搭设申请";
                if (this.ScaffoldType == 1)
                    return "验收申请";
                return "拆除申请";
            }
        }

        /// <summary>
        /// 审核状态
        ///脚手架类型为“搭设申请”时
        ///0-申请中
        ///1-审核中
        ///2-审核未通过
        ///3-审核通过
        ///脚手架类型为“验收申请”时
        ///0-申请中
        ///1-审核中
        ///2-审核未通过
        ///3-审核通过
        ///4-验收中
        ///5-验收未通过
        ///6-验收通过
        ///脚手架类型为“拆除申请”时
        ///0-申请中
        ///1-审核中
        ///2-审核未通过
        ///3-审核通过
        /// </summary>
        /// <returns></returns>
        public int? AuditState { get; set; }

        /// <summary>
        /// 作业状态文本
        /// </summary>
        public string AuditStateStr
        {
            get
            {

                if (this.AuditState == 0)
                    return "申请中";
                if (this.AuditState == 1)
                    return "审核(批)中";
                if (this.AuditState == 2)
                    return "审核(批)未通过";
                if (this.AuditState == 3)
                    return "审核(批)通过";
                if (this.AuditState == 4)
                    return "验收中";
                if (this.AuditState == 5)
                    return "验收未通过";
                if (this.AuditState == 6)
                    return "验收通过";
                return "";
            }
        }
        /// <summary>
        /// 措施落实人
        /// </summary>
        /// <returns></returns>
        public string MeasureCarryout { get; set; }

        /// <summary>
        /// 措施落实人ID
        /// </summary>
        /// <returns></returns>
        public string MeasureCarryoutId { get; set; }

        /// <summary>
        /// 方案措施
        /// </summary>
        /// <returns></returns>
        public string MeasurePlan { get; set; }



        /// <summary>
        /// 流程ID
        /// </summary>
        public string FlowId { get; set; }

        /// <summary>
        /// 流程名
        /// </summary>
        public string FlowName { get; set; }

        /// <summary>
        /// 流程角色ID
        /// </summary>
        public string FlowRoleId { get; set; }

        /// <summary>
        ///流程角色名
        /// </summary>
        public string FlowRoleName { get; set; }
        /// <summary>
        /// 流程部门ID
        /// </summary>
        public string FlowDeptId { get; set; }

        /// <summary>
        /// 流程部门名
        /// </summary>
        public string FlowDeptName { get; set; }

        /// <summary>
        /// 确认结果（0：申请 1：确认 2：审核 3：完成）
        /// </summary>
        public int? InvestigateState { get; set; }

        /// <summary>
        /// 验收照片
        /// </summary>
        public string AcceptFileId { get; set; }

        /// <summary>
        /// 专业类别
        /// </summary>
        public string SpecialtyType { get; set; }

        /// <summary>
        /// 专业类别
        /// </summary>
        public string SpecialtyTypeName { get; set; }

        /// <summary>
        /// 审核备注
        /// </summary>
        public string FlowRemark { get; set; }
        /// <summary>
        /// 架体规格及形式
        /// </summary>
        public List<ScaffoldspecEntity> ScaffoldSpecs { get; set; }

        /// <summary>
        /// 作业安全分析
        /// </summary>
        public List<HighRiskRecordEntity> RiskRecord { get; set; }

        /// <summary>
        /// 审核记录
        /// </summary>
        public List<ScaffoldauditrecordEntity> ScaffoldAudits { get; set; }

        /// <summary>
        /// 验收项目
        /// </summary>
        public List<ScaffoldprojectEntity> ScaffoldProjects { get; set; }

        /// <summary>
        /// 流程步骤
        /// </summary>
        public List<CheckFlowData> CheckFlow { get; set; }

        /// <summary>
        /// 文件
        /// </summary>
        public IList<Photo> cfiles { get; set; }

        /// <summary>
        /// 验收照片
        /// </summary>
        public IList<Photo> acceptfiles { get; set; }

        public IList<FireWaterCondition> conditionlist { get; set; }
        #endregion
    }

    //文件实体
    public class Photo
    {
        public string fileid { get; set; }
        public string filename { get; set; }
        public string fileurl { get; set; }

        public string folderid { get; set; }
    }
}
