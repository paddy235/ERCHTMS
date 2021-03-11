using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HighRiskWork
{
    /// <summary>
    /// 描 述：任务分配人员
    /// </summary>
    [Table("BIS_STAFFINFO")]
    public class StaffInfoEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 创建用户id
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 修改用户id
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 创建用户所属部门编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 创建用户所属机构编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 旁站监督班组name
        /// </summary>
        /// <returns></returns>
        [Column("PTEAMNAME")]
        public string PTeamName { get; set; }
        /// <summary>
        /// 旁站监督班组code
        /// </summary>
        /// <returns></returns>
        [Column("PTEAMCODE")]
        public string PTeamCode { get; set; }
        /// <summary>
        /// 旁站监督班组id
        /// </summary>
        /// <returns></returns>
        [Column("PTEAMID")]
        public string PTeamId { get; set; }
        /// <summary>
        /// 人员name
        /// </summary>
        /// <returns></returns>
        [Column("TASKUSERNAME")]
        public string TaskUserName { get; set; }
        /// <summary>
        /// 人员id
        /// </summary>
        /// <returns></returns>
        [Column("TASKUSERID")]
        public string TaskUserId { get; set; }
        /// <summary>
        /// 旁站计划开始时间
        /// </summary>
        /// <returns></returns>
        [Column("PSTARTTIME")]
        public DateTime? PStartTime { get; set; }
        /// <summary>
        /// 旁站计划结束时间
        /// </summary>
        /// <returns></returns>
        [Column("PENDTIME")]
        public DateTime? PEndTime { get; set; }
        /// <summary>
        /// 任务作业信息名称
        /// </summary>
        /// <returns></returns>
        [Column("WORKINFONAME")]
        public string WorkInfoName { get; set; }
        /// <summary>
        /// 任务作业信息id
        /// </summary>
        /// <returns></returns>
        [Column("WORKINFOID")]
        public string WorkInfoId { get; set; }
        /// <summary>
        /// 任务分配id
        /// </summary>
        /// <returns></returns>
        [Column("TASKSHAREID")]
        public string TaskShareId { get; set; }
        /// <summary>
        /// 监督级别(1:1级[(多人)] 1:2级[旁站时间(单人)])
        /// </summary>
        /// <returns></returns>
        [Column("TASKLEVEL")]
        public string TaskLevel { get; set; }
        /// <summary>
        /// 1级为空,2级人员分配id
        /// </summary>
        /// <returns></returns>
        [Column("STAFFID")]
        public string StaffId { get; set; }

        /// <summary>
        /// 是否数据提交(0:否 1:是)
        /// </summary>
        /// <returns></returns>
        [Column("DATAISSUBMIT")]
        public string DataIsSubmit { get; set; }
        /// <summary>
        /// 组织管理
        /// </summary>
        /// <returns></returns>
        [Column("ORGANIZEMANAGER")]
        public string OrganizeManager { get; set; }
        /// <summary>
        /// 危险分析
        /// </summary>
        /// <returns></returns>
        [Column("RISKANALYSE")]
        public string RiskAnalyse { get; set; }
        /// <summary>
        /// 安全措施
        /// </summary>
        /// <returns></returns>
        [Column("SAFETYMEASURE")]
        public string SafetyMeasure { get; set; }
        /// <summary>
        /// 施工布置
        /// </summary>
        /// <returns></returns>
        [Column("CONSTRUCTLAYOUT")]
        public string ConstructLayout { get; set; }
        /// <summary>
        /// 施工现场安全文明施工评价
        /// </summary>
        /// <returns></returns>
        [Column("EVALUATE")]
        public string Evaluate { get; set; }

        /// <summary>
        /// 总时长
        /// </summary>
        /// <returns></returns>
        [Column("SUMTIMESTR")]
        public int? SumTimeStr { get; set; }

        /// <summary>
        /// 监督状态
        /// </summary>
        /// <returns></returns>
        [Column("SUPERVISESTATE")]
        public string SuperviseState { get; set; }

        /// <summary>
        /// 是否同步
        /// </summary>
        /// <returns></returns>
        [Column("ISSYNCHRONIZATION")]
        public string IsSynchronization { get; set; }


        /// <summary>
        /// 专业类别
        /// </summary>
        /// <returns></returns>
        [Column("SPECIALTYTYPE")]
        public string SpecialtyType { get; set; }


        /// <summary>
        /// 1:已完成 0 or null 未勾选(作业已全部完成，旁站监督任务全部结束)
        /// </summary>
        /// <returns></returns>
        [Column("ISFINISH")]
        public string IsFinish { get; set; }


        public string SpecialtyTypeName { get; set; }

        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = string.IsNullOrEmpty(CreateUserId) ? OperatorProvider.Provider.Current().UserId : CreateUserId;
            this.CreateUserName = string.IsNullOrEmpty(CreateUserName) ? OperatorProvider.Provider.Current().UserName : CreateUserName;
            this.CreateUserDeptCode = string.IsNullOrEmpty(CreateUserDeptCode) ? OperatorProvider.Provider.Current().DeptCode : CreateUserDeptCode;
            this.CreateUserOrgCode = string.IsNullOrEmpty(CreateUserOrgCode) ? OperatorProvider.Provider.Current().OrganizeCode : CreateUserOrgCode;
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