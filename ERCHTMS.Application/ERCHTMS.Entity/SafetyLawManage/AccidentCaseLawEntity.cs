using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.SafetyLawManage
{
    /// <summary>
    /// �� �����¹ʰ�����
    /// </summary>
    [Table("BIS_ACCIDENTCASELAW")]
    public class AccidentCaseLawEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// �ļ�id
        /// </summary>
        /// <returns></returns>
        [Column("FILESID")]
        public string FilesId { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// �¹ʷ�Χ
        /// </summary>
        /// <returns></returns>
        [Column("ACCRANGE")]
        public string AccRange { get; set; }
        /// <summary>
        /// �¹�ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("ACCTIME")]
        public DateTime? AccTime { get; set; }
        /// <summary>
        /// �ļ�����������
        /// </summary>
        /// <returns></returns>
        [Column("FILENAME")]
        public string FileName { get; set; }
        /// <summary>
        /// �����û�������������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// �����û��������ű���
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// �޸��û�
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// �޸�ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// �޸��û�id
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// �����û�
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// �����û�id
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }

        /// <summary>
        /// �¹ʵ�λ
        /// </summary>
        /// <returns></returns>
        [Column("ACCIDENTCOMPANY")]
        public string AccidentCompany { get; set; }
        /// <summary>
        /// �����豸
        /// </summary>
        /// <returns></returns>
        [Column("RELATEDEQUIPMENT")]
        public string RelatedEquipment { get; set; }
        /// <summary>
        /// ���µ�λ
        /// </summary>
        /// <returns></returns>
        [Column("RELATEDCOMPANY")]
        public string RelatedCompany { get; set; }
        /// <summary>
        /// ���¹���
        /// </summary>
        /// <returns></returns>
        [Column("RELATEDJOB")]
        public string RelatedJob { get; set; }
        /// <summary>
        /// �¹ʵȼ�
        /// </summary>
        /// <returns></returns>
        [Column("ACCIDENTGRADE")]
        public string AccidentGrade { get; set; }
        /// <summary>
        /// ����Ͻ��
        /// </summary>
        /// <returns></returns>
        [Column("PROVINCE")]
        public string Province { get; set; }
        /// <summary>
        /// ��������(��)
        /// </summary>
        /// <returns></returns>
        [Column("INTDEATHS")]
        public string intDeaths { get; set; }
        /// <summary>
        /// �¹����
        /// </summary>
        /// <returns></returns>
        [Column("ACCTYPE")]
        public string AccType { get; set; }
        /// <summary>
        /// �¹����CODE
        /// </summary>
        /// <returns></returns>
        [Column("ACCTYPECODE")]
        public string AccTypeCode { get; set; }
        /// <summary>
        /// ������Դ,0����,1ͬ��������
        /// </summary>
        /// <returns></returns>
        [Column("CASESOURCE")]
        public string CaseSource { get; set; }

        /// <summary>
        /// ������Դ,0����,1ͬ��������
        /// </summary>
        /// <returns></returns>
        [Column("CASEFID")]
        public string CaseFid { get; set; }

        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id)?Guid.NewGuid().ToString():Id;
            this.CreateDate = DateTime.Now;
            if (OperatorProvider.Provider.Current() != null)
            {
                this.CreateUserId = OperatorProvider.Provider.Current().UserId;
                this.CreateUserName = OperatorProvider.Provider.Current().UserName;
                this.CreateUserDeptCode = OperatorProvider.Provider.Current().DeptCode;
            }
            else
            {
                this.CreateUserId = "System";
                this.CreateUserName = "��������Ա";
                this.CreateUserDeptCode = "00";
            }
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