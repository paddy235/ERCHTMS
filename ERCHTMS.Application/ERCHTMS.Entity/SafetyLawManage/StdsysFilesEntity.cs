using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.SafetyLawManage
{
    /// <summary>
    /// �� ������׼�ƶ��ļ�
    /// </summary>
    [Table("HRS_STDSYSFILES")]
    public class StdsysFilesEntity : BSEntity
    {
        #region ʵ���Ա        
        /// <summary>
        /// �ļ�����
        /// </summary>
        /// <returns></returns>
        [Column("FILENAME")]
        public string FileName { get; set; }
        /// <summary>
        /// �ļ����
        /// </summary>
        /// <returns></returns>
        [Column("FILENO")]
        public string FileNo { get; set; }
        /// <summary>
        /// ����id
        /// </summary>
        /// <returns></returns>
        [Column("REFID")]
        public string RefId { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("REFNAME")]
        public string RefName { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [Column("PUBDATE")]
        public DateTime? PubDate { get; set; }
        /// <summary>
        /// �޶�����
        /// </summary>
        /// <returns></returns>
        [Column("REVISEDATE")]
        public DateTime? ReviseDate { get; set; }
        /// <summary>
        /// ʵʩ����
        /// </summary>
        /// <returns></returns>
        [Column("USEDATE")]
        public DateTime? UseDate { get; set; }
        /// <summary>
        /// ��������id
        /// </summary>
        /// <returns></returns>
        [Column("PUBDEPARTID")]
        public string PubDepartId { get; set; }
        /// <summary>
        /// ������������
        /// </summary>
        /// <returns></returns>
        [Column("PUBDEPARTNAME")]
        public string PubDepartName { get; set; }
        /// <summary>
        /// ������id
        /// </summary>
        /// <returns></returns>
        [Column("PUBUSERID")]
        public string PubUserId { get; set; }
        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [Column("PUBUSERNAME")]
        public string PubUserName { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// �ļ��б�
        /// </summary>
        [NotMapped]
        public object Files { get; set; }
        /// <summary>
        /// ɾ���ĸ�����¼���
        /// </summary>
        [NotMapped]
        public string DeleteFileId { get; set; }
        #endregion
    }
}


