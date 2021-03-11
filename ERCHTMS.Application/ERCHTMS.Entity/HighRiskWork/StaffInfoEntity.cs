using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HighRiskWork
{
    /// <summary>
    /// �� �������������Ա
    /// </summary>
    [Table("BIS_STAFFINFO")]
    public class StaffInfoEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// �����û�id
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// �����û�
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// �޸��û�id
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// �޸�ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// �޸��û�
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// �����û��������ű���
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// �����û�������������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// ��վ�ල����name
        /// </summary>
        /// <returns></returns>
        [Column("PTEAMNAME")]
        public string PTeamName { get; set; }
        /// <summary>
        /// ��վ�ල����code
        /// </summary>
        /// <returns></returns>
        [Column("PTEAMCODE")]
        public string PTeamCode { get; set; }
        /// <summary>
        /// ��վ�ල����id
        /// </summary>
        /// <returns></returns>
        [Column("PTEAMID")]
        public string PTeamId { get; set; }
        /// <summary>
        /// ��Աname
        /// </summary>
        /// <returns></returns>
        [Column("TASKUSERNAME")]
        public string TaskUserName { get; set; }
        /// <summary>
        /// ��Աid
        /// </summary>
        /// <returns></returns>
        [Column("TASKUSERID")]
        public string TaskUserId { get; set; }
        /// <summary>
        /// ��վ�ƻ���ʼʱ��
        /// </summary>
        /// <returns></returns>
        [Column("PSTARTTIME")]
        public DateTime? PStartTime { get; set; }
        /// <summary>
        /// ��վ�ƻ�����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("PENDTIME")]
        public DateTime? PEndTime { get; set; }
        /// <summary>
        /// ������ҵ��Ϣ����
        /// </summary>
        /// <returns></returns>
        [Column("WORKINFONAME")]
        public string WorkInfoName { get; set; }
        /// <summary>
        /// ������ҵ��Ϣid
        /// </summary>
        /// <returns></returns>
        [Column("WORKINFOID")]
        public string WorkInfoId { get; set; }
        /// <summary>
        /// �������id
        /// </summary>
        /// <returns></returns>
        [Column("TASKSHAREID")]
        public string TaskShareId { get; set; }
        /// <summary>
        /// �ල����(1:1��[(����)] 1:2��[��վʱ��(����)])
        /// </summary>
        /// <returns></returns>
        [Column("TASKLEVEL")]
        public string TaskLevel { get; set; }
        /// <summary>
        /// 1��Ϊ��,2����Ա����id
        /// </summary>
        /// <returns></returns>
        [Column("STAFFID")]
        public string StaffId { get; set; }

        /// <summary>
        /// �Ƿ������ύ(0:�� 1:��)
        /// </summary>
        /// <returns></returns>
        [Column("DATAISSUBMIT")]
        public string DataIsSubmit { get; set; }
        /// <summary>
        /// ��֯����
        /// </summary>
        /// <returns></returns>
        [Column("ORGANIZEMANAGER")]
        public string OrganizeManager { get; set; }
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
        /// ʩ������
        /// </summary>
        /// <returns></returns>
        [Column("CONSTRUCTLAYOUT")]
        public string ConstructLayout { get; set; }
        /// <summary>
        /// ʩ���ֳ���ȫ����ʩ������
        /// </summary>
        /// <returns></returns>
        [Column("EVALUATE")]
        public string Evaluate { get; set; }

        /// <summary>
        /// ��ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("SUMTIMESTR")]
        public int? SumTimeStr { get; set; }

        /// <summary>
        /// �ල״̬
        /// </summary>
        /// <returns></returns>
        [Column("SUPERVISESTATE")]
        public string SuperviseState { get; set; }

        /// <summary>
        /// �Ƿ�ͬ��
        /// </summary>
        /// <returns></returns>
        [Column("ISSYNCHRONIZATION")]
        public string IsSynchronization { get; set; }


        /// <summary>
        /// רҵ���
        /// </summary>
        /// <returns></returns>
        [Column("SPECIALTYTYPE")]
        public string SpecialtyType { get; set; }


        /// <summary>
        /// 1:����� 0 or null δ��ѡ(��ҵ��ȫ����ɣ���վ�ල����ȫ������)
        /// </summary>
        /// <returns></returns>
        [Column("ISFINISH")]
        public string IsFinish { get; set; }


        public string SpecialtyTypeName { get; set; }

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