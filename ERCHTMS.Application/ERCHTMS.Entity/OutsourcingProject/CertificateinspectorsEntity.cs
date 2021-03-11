using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// �� �������������Ա֤����
    /// </summary>
    [Table("EPG_CERTIFICATEINSPECTORS")]
    public class CertificateinspectorsEntity : BaseEntity
    {
        #region ʵ���Ա
        /// <summary>
        /// ��֤����
        /// </summary>
        /// <returns></returns>
        [Column("CREDENTIALSORG")]
        public string CREDENTIALSORG { get; set; }
        /// <summary>
        /// ֤����Ƭ��ַ
        /// </summary>
        /// <returns></returns>
        [Column("CREDENTIALSFILE")]
        public string CREDENTIALSFILE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// ���������ԱId
        /// </summary>
        /// <returns></returns>
        [Column("USERID")]
        public string USERID { get; set; }
        /// <summary>
        /// ��Ч��(��)
        /// </summary>
        /// <returns></returns>
        [Column("VALIDTTIME")]
        public string VALIDTTIME { get; set; }
        /// <summary>
        /// ֤������
        /// </summary>
        /// <returns></returns>
        [Column("CREDENTIALSNAME")]
        public string CREDENTIALSNAME { get; set; }
        /// <summary>
        /// ֤�����
        /// </summary>
        /// <returns></returns>
        [Column("CREDENTIALSCODE")]
        public string CREDENTIALSCODE { get; set; }
        /// <summary>
        /// ��֤����
        /// </summary>
        /// <returns></returns>
        [Column("CREDENTIALSTIME")]
        public DateTime? CREDENTIALSTIME { get; set; }
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

        /// <summary>
        /// ֤��ʧЧ����
        /// </summary>
        /// <returns></returns>
        [Column("ENDDATE")]
        public DateTime? EndDate { get; set; }
        #endregion

        #region ��չ����
        /// <summary>
        /// ��������
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID)?Guid.NewGuid().ToString():ID;

        }
        /// <summary>
        /// �༭����
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ID = keyValue;
                                            }
        #endregion
    }
}