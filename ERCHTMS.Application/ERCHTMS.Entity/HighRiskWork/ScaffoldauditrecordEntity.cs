using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HighRiskWork
{
    /// <summary>
    /// �� ����1.�߷�����ҵ��˼�¼��
    /// </summary>
    [Table("BIS_SCAFFOLDAUDITRECORD")]
    public class ScaffoldauditrecordEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }

        /// <summary>
        /// ��˲���ID
        /// </summary>
        [Column("AUDITDEPTID")]
        public string AuditDeptId { get; set; }
        /// <summary>
        /// ��˲���Code
        /// </summary>
        /// <returns></returns>
        [Column("AUDITDEPTCODE")]
        public string AuditDeptCode { get; set; }

        /// <summary>
        /// ��˲���
        /// </summary>
        [Column("AUDITDEPTNAME")]
        public string AuditDeptName { get; set; }
        /// <summary>
        /// ���ּ���ϢID
        /// </summary>
        /// <returns></returns>
        [Column("SCAFFOLDID")]
        public string ScaffoldId { get; set; }
        /// <summary>
        /// �����ID
        /// </summary>
        /// <returns></returns>
        [Column("AUDITUSERID")]
        public string AuditUserId { get; set; }
        /// <summary>
        /// �����ID
        /// </summary>
        /// <returns></returns>
        [Column("AUDITUSERNAME")]
        public string AuditUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// ���ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("AUDITDATE")]
        public DateTime? AuditDate { get; set; }
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
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("AUDITREMARK")]
        public string AuditRemark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// ���״̬ ��0 or null-��ͬ�� 1-ͬ�⣩
        /// </summary>
        /// <returns></returns>
        [Column("AUDITSTATE")]
        public int? AuditState { get; set; }
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
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
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
        [Column("AUDITTYPE")]
        public int AuditType { get; set; }

        [Column("FLOWID")]
        public string FlowId { get; set; }

        /// <summary>
        /// ǩ��ͼƬ
        /// </summary>
        [Column("AUDITSIGNIMG")]
        public string AuditSignImg { get; set; }

        /// <summary>
        /// ָ����һ��������˺�
        /// </summary>
        [NotMapped]
        public string NextStepApproveUserAccount { get; set; }

        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
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