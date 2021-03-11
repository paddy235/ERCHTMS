using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERCHTMS.Entity.HighRiskWork;

namespace ERCHTMS.AppSerivce.Model
{
    /// <summary>
    /// 单人记录
    /// </summary>
    public class SingleTaskModel
    {
        #region 实体成员
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
        /// <returns></returns
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
        /// 旁站监督班组name
        /// </summary>
        /// <returns></returns>
        public string PTeamName { get; set; }
        /// <summary>
        /// 旁站监督班组code
        /// </summary>
        /// <returns></returns>
        public string PTeamCode { get; set; }
        /// <summary>
        /// 旁站监督班组id
        /// </summary>
        /// <returns></returns>
        public string PTeamId { get; set; }
        /// <summary>
        /// 人员name
        /// </summary>
        /// <returns></returns>
        public string TaskUserName { get; set; }
        /// <summary>
        /// 人员id
        /// </summary>
        /// <returns></returns>
        public string TaskUserId { get; set; }
        /// <summary>
        /// 旁站计划开始时间
        /// </summary>
        /// <returns></returns>
        public DateTime? PStartTime { get; set; }
        /// <summary>
        /// 旁站计划结束时间
        /// </summary>
        /// <returns></returns>
        public DateTime? PEndTime { get; set; }
        /// <summary>
        /// 任务作业信息名称
        /// </summary>
        /// <returns></returns>
        public string WorkInfoName { get; set; }
        /// <summary>
        /// 任务作业信息id
        /// </summary>
        /// <returns></returns>
        public string WorkInfoId { get; set; }
        /// <summary>
        /// 任务分配id
        /// </summary>
        /// <returns></returns>
        public string TaskShareId { get; set; }
        /// <summary>
        /// 监督级别(1:1级[(多人)] 1:2级[旁站时间(单人)])
        /// </summary>
        /// <returns></returns>
        public string TaskLevel { get; set; }
        /// <summary>
        /// 1级为空,2级人员分配id
        /// </summary>
        /// <returns></returns>
        public string StaffId { get; set; }

        /// <summary>
        /// 是否数据提交(0:否 1:是)
        /// </summary>
        /// <returns></returns>
        public string DataIsSubmit { get; set; }
        /// <summary>
        /// 组织管理
        /// </summary>
        /// <returns></returns>
        public string OrganizeManager { get; set; }
        /// <summary>
        /// 危险分析
        /// </summary>
        /// <returns></returns>
        public string RiskAnalyse { get; set; }
        /// <summary>
        /// 安全措施
        /// </summary>
        /// <returns></returns>
        public string SafetyMeasure { get; set; }
        /// <summary>
        /// 施工布置
        /// </summary>
        /// <returns></returns>
        public string ConstructLayout { get; set; }
        /// <summary>
        /// 施工现场安全文明施工评价
        /// </summary>
        /// <returns></returns>
        public string Evaluate { get; set; }
        /// <summary>
        /// 总时长
        /// </summary>
        /// <returns></returns>
        public int? SumTimeStr { get; set; }

        /// <summary>
        /// 监督状态
        /// </summary>
        /// <returns></returns>
        public string SuperviseState { get; set; }

        /// <summary>
        /// 是否完成
        /// </summary>
        /// <returns></returns>
        public bool ischeck { get; set; }


        /// <summary>
        /// 1:已完成 0 or null 未勾选(作业已全部完成，旁站监督任务全部结束)
        /// </summary>
        /// <returns></returns>
        public string IsFinish { get; set; }

        /// <summary>
        /// 作业信息
        /// </summary>
        public List<SuperviseWorkInfoEntity> workspecs { get; set; }
        /// <summary>
        /// 签到记录
        /// </summary>
        public List<SignData> signlist;

        public List<BigCheckData> bigchecklist;
        #endregion

    }

    public class BigCheckData
    {
        public string bigcheckid { get; set; }
        public string checkcontent { get; set; }
        public string isnosuit { get; set; }
        public string ischeckaccomplish { get; set; }
    }
}