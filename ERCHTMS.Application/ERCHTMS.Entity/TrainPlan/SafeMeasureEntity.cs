using ERCHTMS.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.TrainPlan
{
    /// <summary>
    /// 描 述：安措计划
    /// </summary>
    [Table("BIS_SAFEMEASURE")]
    public class SafeMeasureEntity: BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Column("ID")]
        public string Id { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
       [Column("PLANTYPE")]
        public string PlanType { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        [Column("PROJECTNAME")]
        public string ProjectName { get; set; }

        /// <summary>
        /// 部门id
        /// </summary>
        [Column("DEPARTMENTID")]
        public string DepartmentId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [Column("DEPARTMENTNAME")]
        public string DepartmentName { get; set; }

        /// <summary>
        /// 部门代码
        /// </summary>
        [Column("DEPTCODE")]
        public string  DeptCode { get; set; }

        /// <summary>
        /// 计划费用（万）
        /// </summary>
        [Column("COST")]
        public double? Cost { get; set; }

        /// <summary>
        /// 计划完成日期
        /// </summary>
        [Column("PLANFINISHDATE")]
        public DateTime? PlanFinishDate { get; set; }


        /// <summary>
        /// 部门验收人
        /// </summary>
        [Column("CHECKUSERNAME")]
        public string CheckUserName { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 创建人部门编号
        /// </summary>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }

        /// <summary>
        /// 创建人组织编号
        /// </summary>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        
        /// <summary>
        /// 发布状态(0:待下发、1:未完成、2:已完成)
        /// </summary>
        [Column("PUBLISHSTATE")]
        public int? PublishState { get; set; } = 0;

        /// <summary>
        /// 是否结束(0:未结束 1:已结束)
        /// </summary>
        [Column("ISOVER")]
        public int? IsOver { get; set; } = 0;

        /// <summary>
        /// 当前流程节点ID
        /// </summary>
        [Column("FLOWID")]
        public string FlowId { get; set; }

        /// <summary>
        /// 当前流程节点
        /// </summary>
        [Column("FLOWNAME")]
        public string FlowName { get; set; }

        /// <summary>
        /// 流程角色ID
        /// </summary>
        [Column("FLOWROLE")]
        public string FlowRole { get; set; }

        /// <summary>
        /// 流程角色名称
        /// </summary>
        [Column("FLOWROLENAME")]
        public string FlowRoleName { get; set; }

        /// <summary>
        /// 流程部门编码/ID
        /// </summary>
        [Column("FLOWDEPT")]
        public string FlowDept { get; set; }

        /// <summary>
        /// 流程部门名称
        /// </summary>
        [Column("FLOWDEPTNAME")]
        public string FlowDeptName { get; set; }

        /// <summary>
        /// 是否提交 0 ：未提交 1：提交
        /// </summary>
        [Column("ISCOMMIT")]
        public string IsCommit { get; set; }

        /// <summary>
        /// 调整流程
        /// </summary>
        [Column("STAUTS")]
        public string Stauts { get; set; }

        /// <summary>
        /// (0:无调整 1:调整申请 2:调整审批 3:结束)
        /// </summary>
        [Column("PROCESSSTATE")]
        public int? ProcessState { get; set; }

        /// <summary>
        /// 实际完成时间
        /// </summary>
        [Column("FINISHDATE")]
        public DateTime? FinishDate { get; set; }

        /// <summary>
        /// 实际费用
        /// </summary>
        [Column("FEE")]
        public decimal? Fee { get; set; }

        /// <summary>
        /// 暂存状态(总结报告提交后同步更新PublishState)
        /// </summary>
        [Column("TEMPSTATE")]
        public int? TempState { get; set; }

        /// <summary>
        /// 总结报告表主键
        /// </summary>
        [Column("REPORTID")]
        public string ReportID { get; set; }

        /// <summary>
        /// 安措计划报告是否结束
        /// </summary>
        [Column("STATE")]
        public int? State { get; set; } = 0;
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
