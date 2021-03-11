using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.EvaluateManage
{
    /// <summary>
    /// �� �����Ϲ������ۼƻ�
    /// </summary>
    [Table("HRS_EVALUATEPLAN")]
    public class EvaluatePlanEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����
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
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("WORKTITLE")]
        public string WorkTitle { get; set; }
        /// <summary>
        /// ���۲���
        /// </summary>
        /// <returns></returns>
        [Column("DEPT")]
        public string Dept { get; set; }
        /// <summary>
        /// ���۽�ֹʱ��
        /// </summary>
        /// <returns></returns>
        [Column("ABORTDATE")]
        public DateTime? AbortDate { get; set; }
        /// <summary>
        /// �Ƿ��ύ
        /// </summary>
        /// <returns></returns>
        [Column("ISSUBMIT")]
        public int? IsSubmit { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [Column("REMAKE")]
        public string Remake { get; set; }
        /// <summary>
        /// ���״̬//0�������ύ 1���۱��汣�� 2���۱����ύ 3��˱��� 4����ύ
        /// </summary>
        /// <returns></returns>
        [Column("CHECKSTATE")]
        public int? CheckState { get; set; }
        /// <summary>
        /// �ƻ�����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("ISSUEDATE")]
        public DateTime? IssueDate { get; set; }
        /// <summary>
        /// ���
        /// </summary>
        /// <returns></returns>
        [Column("YEAR")]
        public int? Year { get; set; }
        /// <summary>
        /// �����۲��Ÿ���
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNUM")]
        public int? DeptNum { get; set; }
        /// <summary>
        /// �����۲��Ÿ���
        /// </summary>
        /// <returns></returns>
        [Column("DONEDEPTNUM")]
        public int? DoneDeptNum { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("APPROVER")]
        public string Approver { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("APPROVEDEPT")]
        public string ApproveDept { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("APPROVEDATE")]
        public DateTime? ApproveDate { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [Column("APPROVERESULT")]
        public int? ApproveResult { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [Column("APPROVEOPINION")]
        public string ApproveOpinion { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("REVIEWDATE")]
        public string ReviewDate { get; set; }
        /// <summary>
        /// �μ����۵���Ҫ��Ա
        /// </summary>
        /// <returns></returns>
        [Column("INMEMBER")]
        public string InMember { get; set; }
        /// <summary>
        /// ���۷�Χ
        /// </summary>
        /// <returns></returns>
        [Column("EVALUATESCOPE")]
        public string EvaluateScope { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("EVALUATEGIST")]
        public string EvaluateGist { get; set; }
        /// <summary>
        /// ����Ҫ��ʶ�����
        /// </summary>
        /// <returns></returns>
        [Column("RECOGNIZECONDITION")]
        public string RecognizeCondition { get; set; }
        /// <summary>
        /// �Ϲ�����������
        /// </summary>
        /// <returns></returns>
        [Column("EVALUATESUMMARIZE")]
        public string EvaluateSummarize { get; set; }
        /// <summary>
        /// ���۽���
        /// </summary>
        /// <returns></returns>
        [Column("EVALUATEVERDICT")]
        public string EvaluateVerdict { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
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
        /// �༭����
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