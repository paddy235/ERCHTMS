using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.RoutineSafetyWork
{
    /// <summary>
    /// 描 述：文件管理
    /// </summary>
    [Table("BIS_FILEMANAGE")]
    public class FileManageEntity : BSEntity
    {
        #region 
        /// <summary>
        /// 文件名称
        /// </summary>
        /// <returns></returns>
        [Column("FILENAME")]
        public string FileName { get; set; }
        /// <summary>
        /// 文件编号
        /// </summary>
        /// <returns></returns>
        [Column("FILENO")]
        public string FileNo { get; set; }
        /// <summary>
        /// 文件类型ID
        /// </summary>
        /// <returns></returns>
        [Column("FILETYPEID")]
        public string FileTypeId { get; set; }
        /// <summary>
        /// 文件类型ID
        /// </summary>
        /// <returns></returns>
        [Column("FILETYPECODE")]
        public string FileTypeCode { get; set; }
        /// <summary>
        /// 文件类型名称
        /// </summary>
        /// <returns></returns>
        [Column("FILETYPENAME")]
        public string FileTypeName { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        /// <returns></returns>
        [Column("RELEASETIME")]
        public DateTime? ReleaseTime { get; set; }
        /// <summary>
        /// 发布单位ID
        /// </summary>
        /// <returns></returns>
        [Column("RELEASEDEPTID")]
        public string ReleaseDeptId { get; set; }
        /// <summary>
        /// 发布单位名称
        /// </summary>
        /// <returns></returns>
        [Column("RELEASEDEPTNAME")]
        public string ReleaseDeptName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        #endregion

        
    }
}