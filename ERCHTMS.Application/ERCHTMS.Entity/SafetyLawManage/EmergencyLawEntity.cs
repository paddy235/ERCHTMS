using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.SafetyLawManage
{
    /// <summary>
    /// �� ����Ӧ��Ԥ������
    /// </summary>
    [Table("BIS_EMERGENCYLAW")]
    public class EmergencyLawEntity : BaseEntity
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
        /// �ļ�����������
        /// </summary>
        /// <returns></returns>
        [Column("FILENAME")]
        public string FileName { get; set; }
        /// <summary>
        /// Ӧ����������
        /// </summary>
        /// <returns></returns>
        [Column("EMERGENCYTYPE")]
        public string EmergencyType { get; set; }
        /// <summary>
        /// ��Դ
        /// </summary>
        /// <returns></returns>
        [Column("SOURCE")]
        public string Source { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// �ļ�id
        /// </summary>
        /// <returns></returns>
        [Column("FILESID")]
        public string FilesId { get; set; }
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