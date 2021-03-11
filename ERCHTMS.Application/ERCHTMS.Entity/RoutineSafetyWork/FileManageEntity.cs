using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.RoutineSafetyWork
{
    /// <summary>
    /// �� �����ļ�����
    /// </summary>
    [Table("BIS_FILEMANAGE")]
    public class FileManageEntity : BSEntity
    {
        #region 
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
        /// �ļ�����ID
        /// </summary>
        /// <returns></returns>
        [Column("FILETYPEID")]
        public string FileTypeId { get; set; }
        /// <summary>
        /// �ļ�����ID
        /// </summary>
        /// <returns></returns>
        [Column("FILETYPECODE")]
        public string FileTypeCode { get; set; }
        /// <summary>
        /// �ļ���������
        /// </summary>
        /// <returns></returns>
        [Column("FILETYPENAME")]
        public string FileTypeName { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        /// <returns></returns>
        [Column("RELEASETIME")]
        public DateTime? ReleaseTime { get; set; }
        /// <summary>
        /// ������λID
        /// </summary>
        /// <returns></returns>
        [Column("RELEASEDEPTID")]
        public string ReleaseDeptId { get; set; }
        /// <summary>
        /// ������λ����
        /// </summary>
        /// <returns></returns>
        [Column("RELEASEDEPTNAME")]
        public string ReleaseDeptName { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        #endregion

        
    }
}