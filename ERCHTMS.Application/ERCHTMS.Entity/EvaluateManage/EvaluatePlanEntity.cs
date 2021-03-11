using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.EvaluateManage
{
    /// <summary>
    /// 描 述：合规性评价计划
    /// </summary>
    [Table("HRS_EVALUATEPLAN")]
    public class EvaluatePlanEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int? AUTOID { get; set; }
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
        /// 工作标题
        /// </summary>
        /// <returns></returns>
        [Column("WORKTITLE")]
        public string WorkTitle { get; set; }
        /// <summary>
        /// 评价部门
        /// </summary>
        /// <returns></returns>
        [Column("DEPT")]
        public string Dept { get; set; }
        /// <summary>
        /// 评价截止时间
        /// </summary>
        /// <returns></returns>
        [Column("ABORTDATE")]
        public DateTime? AbortDate { get; set; }
        /// <summary>
        /// 是否提交
        /// </summary>
        /// <returns></returns>
        [Column("ISSUBMIT")]
        public int? IsSubmit { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("REMAKE")]
        public string Remake { get; set; }
        /// <summary>
        /// 审核状态//0数据已提交 1评价报告保存 2评价报告提交 3审核保存 4审核提交
        /// </summary>
        /// <returns></returns>
        [Column("CHECKSTATE")]
        public int? CheckState { get; set; }
        /// <summary>
        /// 计划发布时间
        /// </summary>
        /// <returns></returns>
        [Column("ISSUEDATE")]
        public DateTime? IssueDate { get; set; }
        /// <summary>
        /// 年度
        /// </summary>
        /// <returns></returns>
        [Column("YEAR")]
        public int? Year { get; set; }
        /// <summary>
        /// 需评价部门个数
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNUM")]
        public int? DeptNum { get; set; }
        /// <summary>
        /// 已评价部门个数
        /// </summary>
        /// <returns></returns>
        [Column("DONEDEPTNUM")]
        public int? DoneDeptNum { get; set; }
        /// <summary>
        /// 审批人
        /// </summary>
        /// <returns></returns>
        [Column("APPROVER")]
        public string Approver { get; set; }
        /// <summary>
        /// 审批部门
        /// </summary>
        /// <returns></returns>
        [Column("APPROVEDEPT")]
        public string ApproveDept { get; set; }
        /// <summary>
        /// 审批时间
        /// </summary>
        /// <returns></returns>
        [Column("APPROVEDATE")]
        public DateTime? ApproveDate { get; set; }
        /// <summary>
        /// 审批结果
        /// </summary>
        /// <returns></returns>
        [Column("APPROVERESULT")]
        public int? ApproveResult { get; set; }
        /// <summary>
        /// 审批意见
        /// </summary>
        /// <returns></returns>
        [Column("APPROVEOPINION")]
        public string ApproveOpinion { get; set; }
        /// <summary>
        /// 评审时间
        /// </summary>
        /// <returns></returns>
        [Column("REVIEWDATE")]
        public string ReviewDate { get; set; }
        /// <summary>
        /// 参加评价的主要成员
        /// </summary>
        /// <returns></returns>
        [Column("INMEMBER")]
        public string InMember { get; set; }
        /// <summary>
        /// 评价范围
        /// </summary>
        /// <returns></returns>
        [Column("EVALUATESCOPE")]
        public string EvaluateScope { get; set; }
        /// <summary>
        /// 评价依据
        /// </summary>
        /// <returns></returns>
        [Column("EVALUATEGIST")]
        public string EvaluateGist { get; set; }
        /// <summary>
        /// 法定要求识别情况
        /// </summary>
        /// <returns></returns>
        [Column("RECOGNIZECONDITION")]
        public string RecognizeCondition { get; set; }
        /// <summary>
        /// 合规性评价综述
        /// </summary>
        /// <returns></returns>
        [Column("EVALUATESUMMARIZE")]
        public string EvaluateSummarize { get; set; }
        /// <summary>
        /// 评价结论
        /// </summary>
        /// <returns></returns>
        [Column("EVALUATEVERDICT")]
        public string EvaluateVerdict { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id)?Guid.NewGuid().ToString():Id;
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