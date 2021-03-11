using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERCHTMS.Entity.HighRiskWork
{
    /// <summary>
    /// 描 述：旁站监督任务
    /// </summary>
    [Table("BIS_SUPERVISETASK")]
    public class SuperviseTaskEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 监督级别(0:1级 1:2级)
        /// </summary>
        /// <returns></returns>
        [Column("TASKLEVEL")]
        public string TaskLevel { get; set; }
        /// <summary>
        /// 创建用户id
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 监督时长
        /// </summary>
        /// <returns></returns>
        [Column("TIMELONG")]
        public string TimeLong { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 组织管理
        /// </summary>
        /// <returns></returns>
        [Column("ORGANIZEMANAGER")]
        public string OrganizeManager { get; set; }
        /// <summary>
        /// 监督code(列表树形结构所需)
        /// </summary>
        /// <returns></returns>
        [Column("SUPERVISECODE")]
        public string SuperviseCode { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 监督状态(1.创建监督 2.未监督 3.已监督)
        /// </summary>
        /// <returns></returns>
        [Column("SUPERVISESTATE")]
        public string SuperviseState { get; set; }
        /// <summary>
        /// 作业结束时间
        /// </summary>
        /// <returns></returns>
        [Column("TASKWORKENDTIME")]
        public DateTime? TaskWorkEndTime { get; set; }
        /// <summary>
        /// 创建用户所属部门编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 监督时长
        /// </summary>
        /// <returns></returns>
        [Column("TIMELONGSTR")]
        public string TimeLongStr { get; set; }
        /// <summary>
        /// 作业类别id
        /// </summary>
        /// <returns></returns>
        [Column("TASKWORKTYPEID")]
        public string TaskWorkTypeId { get; set; }
        /// <summary>
        /// 监督员
        /// </summary>
        /// <returns></returns>
        [Column("TASKUSERNAME")]
        public string TaskUserName { get; set; }
        /// <summary>
        /// 作业类别
        /// </summary>
        /// <returns></returns>
        [Column("TASKWORKTYPE")]
        public string TaskWorkType { get; set; }
        /// <summary>
        /// 修改用户id
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 作业开始时间
        /// </summary>
        /// <returns></returns>
        [Column("TASKWORKSTARTTIME")]
        public DateTime? TaskWorkStartTime { get; set; }
        /// <summary>
        /// 监督员
        /// </summary>
        /// <returns></returns>
        [Column("TASKUSERID")]
        public string TaskUserId { get; set; }
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
        /// 工作票号
        /// </summary>
        /// <returns></returns>
        [Column("TASKBILL")]
        public string TaskBill { get; set; }
        /// <summary>
        /// 创建用户所属机构编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 监督任务id(多级任务所需)
        /// </summary>
        /// <returns></returns>
        [Column("SUPERPARENTID")]
        public string SuperParentId { get; set; }
        /// <summary>
        /// 施工布置
        /// </summary>
        /// <returns></returns>
        [Column("CONSTRUCTLAYOUT")]
        public string ConstructLayout { get; set; }

        /// <summary>
        /// 旁站监督班组
        /// </summary>
        /// <returns></returns>
        [Column("STEAMID")]
        public string STeamId { get; set; }

        /// <summary>
        /// 旁站监督班组code
        /// </summary>
        /// <returns></returns>
        [Column("STEAMCODE")]
        public string STeamCode { get; set; }

        /// <summary>
        /// 旁站监督班组
        /// </summary>
        /// <returns></returns>
        [Column("STEAMNAME")]
        public string STeamName { get; set; }

        /// <summary>
        /// 作业类别(手动输入)
        /// </summary>
        /// <returns></returns>
        [Column("HANDTYPE")]
        public string HandType { get; set; }


        /// <summary>
        /// 班组终端是否提交了监督状态（0：未提交 1：已提交）
        /// </summary>
        /// <returns></returns>
        [Column("ISSUBMIT")]
        public string IsSubmit { get; set; }


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