using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.PersonManage
{
    /// <summary>
    /// �� ������Ա֤��
    /// </summary>
    [Table("BIS_CERTIFICATE")]
    public class CertificateEntity : BaseEntity
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
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
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
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// ֤������
        /// </summary>
        /// <returns></returns>
        [Column("CERTNAME")]
        public string CertName { get; set; }
        /// <summary>
        /// ֤����
        /// </summary>
        /// <returns></returns>
        [Column("CERTNUM")]
        public string CertNum { get; set; }
        /// <summary>
        /// ��֤����
        /// </summary>
        /// <returns></returns>
        [Column("SENDDATE")]
        public DateTime? SendDate { get; set; }
        /// <summary>
        /// ��Ч�ڣ��꣩
        /// </summary>
        /// <returns></returns>
        [Column("YEARS")]
        public int? Years { get; set; }
        /// <summary>
        /// ״̬(1����Ч��0����Ч)
        /// </summary>
        /// <returns></returns>
        [Column("STATUS")]
        public int? Status { get; set; }
        /// <summary>
        /// ֤��ʧЧ����
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
        /// ����·��
        /// </summary>
        /// <returns></returns>
        [Column("FILEPATH")]
        public string FilePath { get; set; }
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
        /// ֤������
        /// </summary>
        /// <returns></returns>
        [Column("CERTTYPE")]
        public string CertType { get; set; }
        /// <summary>
        /// ����/��ҵ���
        /// </summary>
        /// <returns></returns>
        [Column("WORKTYPE")]
        public string WorkType { get; set; }
        /// <summary>
        /// ��ҵ��Ŀ/׼����Ŀ
        /// </summary>
        /// <returns></returns>
        [Column("WORKITEM")]
        public string WorkItem { get; set; }
        /// <summary>
        /// ��Ŀ����
        /// </summary>
        /// <returns></returns>
        [Column("ITEMNUM")]
        public string ItemNum { get; set; }

        /// <summary>
        ///������
        /// </summary>
        /// <returns></returns>
        public string Result { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDATE")]
        public DateTime? ApplyDate { get; set; }
        /// <summary>
        /// ��ʼ����
        /// </summary>
        /// <returns></returns>
        [Column("STARTDATE")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        ///�ȼ�
        /// </summary>
        /// <returns></returns>
         [Column("GRADE")]
        public string Grade { get; set; }

        /// <summary>
        ///��ҵ
        /// </summary>
        /// <returns></returns>
         [Column("INDUSTRY")]
        public string Industry { get; set; }

        /// <summary>
        ///��Ա����
        /// </summary>
        /// <returns></returns>
         [Column("USERTYPE")]
        public string UserType { get; set; }
         /// <summary>
         ///����
         /// </summary>
         /// <returns></returns>
         [Column("CRAFT")]
         public string Craft { get; set; }
         /// <summary>
         ///�ʸ�����
         /// </summary>
         /// <returns></returns>
         [Column("ZGNAME")]
         public string ZGName { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            var user = OperatorProvider.Provider.Current();
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
            this.CreateDate = DateTime.Now;
            this.Status = 1;
            if (user!=null)
            {
                this.CreateUserId = user.UserId;
                this.CreateUserDeptCode = user.DeptCode;
                this.CreateUserOrgCode = user.OrganizeCode;
            }
          
        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.Id = keyValue;
            this.ModifyDate = DateTime.Now;
            var user = OperatorProvider.Provider.Current();
            if(user!=null)
            {
                this.ModifyUserId = user.UserId;
            }
        }
        #endregion
    }
}