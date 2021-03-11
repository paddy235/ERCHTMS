using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.HighRiskWork.ViewModel
{
   public class SuperviseTaskModel
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        public string Id { get; set; }
        /// <summary>
        /// 监督级别(0:1级 1:2级)
        /// </summary>
        /// <returns></returns>
        public string TaskLevel { get; set; }
        /// <summary>
        /// 创建用户id
        /// </summary>
        /// <returns></returns>
        public string CreateUserId { get; set; }
        /// <summary>
        /// 监督时长
        /// </summary>
        /// <returns></returns>
        public string TimeLong { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 组织管理
        /// </summary>
        /// <returns></returns>
        public string OrganizeManager { get; set; }
        /// <summary>
        /// 监督code(列表树形结构所需)
        /// </summary>
        /// <returns></returns>
        public string SuperviseCode { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 监督状态(1.创建监督 2.未监督 3.已监督)
        /// </summary>
        /// <returns></returns
        public string SuperviseState { get; set; }
        /// <summary>
        /// 作业结束时间
        /// </summary>
        /// <returns></returns>
        public DateTime? TaskWorkEndTime { get; set; }
        /// <summary>
        /// 创建用户所属部门编码
        /// </summary>
        /// <returns></returns>
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <returns></returns>
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 监督时长
        /// </summary>
        /// <returns></returns
        public string TimeLongStr { get; set; }
        /// <summary>
        /// 作业类别id
        /// </summary>
        /// <returns></returns>
        public string TaskWorkTypeId { get; set; }
        /// <summary>
        /// 监督员
        /// </summary>
        /// <returns></returns>
        public string TaskUserName { get; set; }
        /// <summary>
        /// 作业类别
        /// </summary>
        /// <returns></returns>
        public string TaskWorkType { get; set; }
        /// <summary>
        /// 修改用户id
        /// </summary>
        /// <returns></returns>
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 作业开始时间
        /// </summary>
        /// <returns></returns>
        public DateTime? TaskWorkStartTime { get; set; }
        /// <summary>
        /// 监督员
        /// </summary>
        /// <returns></returns>
        public string TaskUserId { get; set; }
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
        /// 工作票号
        /// </summary>
        /// <returns></returns>
        public string TaskBill { get; set; }
        /// <summary>
        /// 创建用户所属机构编码
        /// </summary>
        /// <returns></returns>
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 监督任务id(多级任务所需)
        /// </summary>
        /// <returns></returns>
        public string SuperParentId { get; set; }
        /// <summary>
        /// 施工布置
        /// </summary>
        /// <returns></returns
        public string ConstructLayout { get; set; }

        /// <summary>
        /// 旁站监督班组
        /// </summary>
        /// <returns></returns>
        public string STeamId { get; set; }

        /// <summary>
        /// 旁站监督班组code
        /// </summary>
        public string STeamCode { get; set; }

        /// <summary>
        /// 旁站监督班组
        /// </summary>
        /// <returns></returns>
        public string STeamName { get; set; }

        /// <summary>
        /// 作业类别(手动输入)
        /// </summary>
        /// <returns></returns>
        public string HandType { get; set; }

        /// <summary>
        ///  班组终端是否提交了监督状态（0：未提交 1：已提交）
        /// </summary>
        /// <returns></returns>
        public string IsSubmit { get; set; }


        /// <summary>
        /// 作业信息
        /// </summary>
        public List<SuperviseWorkInfoEntity> WorkSpecs { get; set; }


        #endregion
    }
}
