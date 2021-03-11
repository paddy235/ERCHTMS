using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.SafetyLawManage
{
    /// <summary>
    /// �� ������ȫ�������ɷ���
    /// </summary>
    [Table("BIS_SAFETYLAW")]
    public class SafetyLawEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// ���ɷ������ͱ��
        /// </summary>
        /// <returns></returns>
        [Column("LAWTYPECODE")]
        public string LawTypeCode { get; set; }
        /// <summary>
        /// �����û�������������
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// �ļ����
        /// </summary>
        /// <returns></returns>
        [Column("FILECODE")]
        public string FileCode { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [Column("LAWAREA")]
        public string LawArea { get; set; }
        /// <summary>
        /// ʩ������
        /// </summary>
        /// <returns></returns>
        [Column("CARRYDATE")]
        public DateTime? CarryDate { get; set; }
        /// <summary>
        /// �����û��������ű���
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// �����û�id
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// �޸��û�
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// �޸�ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// �����û�
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// �ļ�����������
        /// </summary>
        /// <returns></returns>
        [Column("FILENAME")]
        public string FileName { get; set; }
        /// <summary>
        /// �޸��û�id
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("ISSUEDEPT")]
        public string IssueDept { get; set; }
        /// <summary>
        /// ��������CODE
        /// </summary>
        /// <returns></returns>
        [Column("ISSUEDEPTCODE")]
        public string IssueDeptCode { get; set; }
        /// <summary>
        /// ��Ч�汾��
        /// </summary>
        /// <returns></returns>
        [Column("VALIDVERSIONS")]
        public string ValidVersions { get; set; }
        /// <summary>
        /// �����ֶ�
        /// </summary>
        /// <returns></returns>
        [Column("RESERVE")]
        public string Reserve { get; set; }

        /// <summary>
        /// �ļ�id
        /// </summary>
        /// <returns></returns>
        [Column("FILESID")]
        public string FilesId { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("PROVINCE")]
        public string Province { get; set; }
        /// <summary>
        /// Ч�����𣨷������ͣ�
        /// </summary>
        /// <returns></returns>
        [Column("LAWTYPE")]
        public string LawType { get; set; }
        /// <summary>
        /// ʱЧ��
        /// </summary>
        /// <returns></returns>
        [Column("EFFETSTATE")]
        public string EffetState { get; set; }
        /// <summary>
        /// ���ݴ��
        /// </summary>
        /// <returns></returns>
        [Column("MAINCONTENT")]
        public string MainContent { get; set; }
        /// <summary>
        /// ���Ŀ���
        /// </summary>
        /// <returns></returns>
        [Column("COPYCONTENT")]
        public string CopyContent { get; set; }
        /// <summary>
        /// ������Դ(0����,1�����)
        /// </summary>
        /// <returns></returns>
        [Column("LAWSOURCE")]
        public string LawSource { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("RELEASEDATE")]
        public DateTime? ReleaseDate { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("UPDATEDATE")]
        public DateTime? UpdateDate { get; set; }
        /// <summary>
        /// ������ʽ
        /// </summary>
        /// <returns></returns>
        [Column("CHANNELTYPE")]
        public string ChannelType { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
            this.CreateDate = DateTime.Now;
            if (OperatorProvider.Provider.Current() != null)
            {
                this.CreateUserId = OperatorProvider.Provider.Current().UserId;
                this.CreateUserName = OperatorProvider.Provider.Current().UserName;
                this.CreateUserDeptCode = OperatorProvider.Provider.Current().DeptCode;
            }
            else {
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