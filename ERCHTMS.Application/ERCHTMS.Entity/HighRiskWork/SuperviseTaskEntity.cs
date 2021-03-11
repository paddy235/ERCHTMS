using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERCHTMS.Entity.HighRiskWork
{
    /// <summary>
    /// �� ������վ�ල����
    /// </summary>
    [Table("BIS_SUPERVISETASK")]
    public class SuperviseTaskEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// �ල����(0:1�� 1:2��)
        /// </summary>
        /// <returns></returns>
        [Column("TASKLEVEL")]
        public string TaskLevel { get; set; }
        /// <summary>
        /// �����û�id
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// �ලʱ��
        /// </summary>
        /// <returns></returns>
        [Column("TIMELONG")]
        public string TimeLong { get; set; }
        /// <summary>
        /// �޸�ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// ��֯����
        /// </summary>
        /// <returns></returns>
        [Column("ORGANIZEMANAGER")]
        public string OrganizeManager { get; set; }
        /// <summary>
        /// �ලcode(�б����νṹ����)
        /// </summary>
        /// <returns></returns>
        [Column("SUPERVISECODE")]
        public string SuperviseCode { get; set; }
        /// <summary>
        /// �����û�
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// �ල״̬(1.�����ල 2.δ�ල 3.�Ѽල)
        /// </summary>
        /// <returns></returns>
        [Column("SUPERVISESTATE")]
        public string SuperviseState { get; set; }
        /// <summary>
        /// ��ҵ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("TASKWORKENDTIME")]
        public DateTime? TaskWorkEndTime { get; set; }
        /// <summary>
        /// �����û��������ű���
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// �޸��û�
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// �ලʱ��
        /// </summary>
        /// <returns></returns>
        [Column("TIMELONGSTR")]
        public string TimeLongStr { get; set; }
        /// <summary>
        /// ��ҵ���id
        /// </summary>
        /// <returns></returns>
        [Column("TASKWORKTYPEID")]
        public string TaskWorkTypeId { get; set; }
        /// <summary>
        /// �ලԱ
        /// </summary>
        /// <returns></returns>
        [Column("TASKUSERNAME")]
        public string TaskUserName { get; set; }
        /// <summary>
        /// ��ҵ���
        /// </summary>
        /// <returns></returns>
        [Column("TASKWORKTYPE")]
        public string TaskWorkType { get; set; }
        /// <summary>
        /// �޸��û�id
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// ��ҵ��ʼʱ��
        /// </summary>
        /// <returns></returns>
        [Column("TASKWORKSTARTTIME")]
        public DateTime? TaskWorkStartTime { get; set; }
        /// <summary>
        /// �ලԱ
        /// </summary>
        /// <returns></returns>
        [Column("TASKUSERID")]
        public string TaskUserId { get; set; }
        /// <summary>
        /// Σ�շ���
        /// </summary>
        /// <returns></returns>
        [Column("RISKANALYSE")]
        public string RiskAnalyse { get; set; }
        /// <summary>
        /// ��ȫ��ʩ
        /// </summary>
        /// <returns></returns>
        [Column("SAFETYMEASURE")]
        public string SafetyMeasure { get; set; }
        /// <summary>
        /// ����Ʊ��
        /// </summary>
        /// <returns></returns>
        [Column("TASKBILL")]
        public string TaskBill { get; set; }
        /// <summary>
        /// �����û�������������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// �ල����id(�༶��������)
        /// </summary>
        /// <returns></returns>
        [Column("SUPERPARENTID")]
        public string SuperParentId { get; set; }
        /// <summary>
        /// ʩ������
        /// </summary>
        /// <returns></returns>
        [Column("CONSTRUCTLAYOUT")]
        public string ConstructLayout { get; set; }

        /// <summary>
        /// ��վ�ල����
        /// </summary>
        /// <returns></returns>
        [Column("STEAMID")]
        public string STeamId { get; set; }

        /// <summary>
        /// ��վ�ල����code
        /// </summary>
        /// <returns></returns>
        [Column("STEAMCODE")]
        public string STeamCode { get; set; }

        /// <summary>
        /// ��վ�ල����
        /// </summary>
        /// <returns></returns>
        [Column("STEAMNAME")]
        public string STeamName { get; set; }

        /// <summary>
        /// ��ҵ���(�ֶ�����)
        /// </summary>
        /// <returns></returns>
        [Column("HANDTYPE")]
        public string HandType { get; set; }


        /// <summary>
        /// �����ն��Ƿ��ύ�˼ල״̬��0��δ�ύ 1�����ύ��
        /// </summary>
        /// <returns></returns>
        [Column("ISSUBMIT")]
        public string IsSubmit { get; set; }


        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
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