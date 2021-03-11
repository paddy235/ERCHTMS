using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.PersonManage
{
    /// <summary>
    /// �� ������Ա֤�������¼
    /// </summary>
    [Table("BIS_CERTAUDIT")]
    public class CertAuditEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
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
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// ������Ŀ����
        /// </summary>
        /// <returns></returns>
        [Column("ITEMCODE")]
        public string ItemCode { get; set; }
        /// <summary>
        /// ����/��֤����
        /// </summary>
        /// <returns></returns>
        [Column("AUDITDATE")]
        public DateTime? AuditDate { get; set; }
        /// <summary>
        /// �´θ�������
        /// </summary>
        /// <returns></returns>
        [Column("NEXTDATE")]
        public DateTime? NextDate { get; set; }
        /// <summary>
        /// ֤����Ч����
        /// </summary>
        /// <returns></returns>
        [Column("ENDDATE")]
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// ��֤����
        /// </summary>
        /// <returns></returns>
        [Column("SENDORGAN")]
        public string SendOrgan { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// �����û�Id
        /// </summary>
        /// <returns></returns>
        [Column("USERID")]
        public string UserId { get; set; }
        /// <summary>
        /// ����֤��Id
        /// </summary>
        /// <returns></returns>
        [Column("CERTID")]
        public string CertId { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("AUDITTYPE")]
        public string AuditType { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        /// <returns></returns>
        [Column("RESULT")]
        public string Result { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            var user=OperatorProvider.Provider.Current();
            this.Id= string.IsNullOrEmpty(Id)?Guid.NewGuid().ToString():Id;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = user.UserId;
            this.CreateUserName = user.UserName;
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.Id = keyValue;
        }
        #endregion
    }
}