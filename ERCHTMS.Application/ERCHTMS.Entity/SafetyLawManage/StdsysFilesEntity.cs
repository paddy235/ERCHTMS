using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.SafetyLawManage
{
    /// <summary>
    /// 描 述：标准制度文件
    /// </summary>
    [Table("HRS_STDSYSFILES")]
    public class StdsysFilesEntity : BSEntity
    {
        #region 实体成员        
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
        /// 引用id
        /// </summary>
        /// <returns></returns>
        [Column("REFID")]
        public string RefId { get; set; }
        /// <summary>
        /// 引用名称
        /// </summary>
        /// <returns></returns>
        [Column("REFNAME")]
        public string RefName { get; set; }
        /// <summary>
        /// 发布日期
        /// </summary>
        /// <returns></returns>
        [Column("PUBDATE")]
        public DateTime? PubDate { get; set; }
        /// <summary>
        /// 修订日期
        /// </summary>
        /// <returns></returns>
        [Column("REVISEDATE")]
        public DateTime? ReviseDate { get; set; }
        /// <summary>
        /// 实施日期
        /// </summary>
        /// <returns></returns>
        [Column("USEDATE")]
        public DateTime? UseDate { get; set; }
        /// <summary>
        /// 发布部门id
        /// </summary>
        /// <returns></returns>
        [Column("PUBDEPARTID")]
        public string PubDepartId { get; set; }
        /// <summary>
        /// 发布部门名称
        /// </summary>
        /// <returns></returns>
        [Column("PUBDEPARTNAME")]
        public string PubDepartName { get; set; }
        /// <summary>
        /// 发布人id
        /// </summary>
        /// <returns></returns>
        [Column("PUBUSERID")]
        public string PubUserId { get; set; }
        /// <summary>
        /// 发布人名称
        /// </summary>
        /// <returns></returns>
        [Column("PUBUSERNAME")]
        public string PubUserName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// 文件列表
        /// </summary>
        [NotMapped]
        public object Files { get; set; }
        /// <summary>
        /// 删除的附件记录编号
        /// </summary>
        [NotMapped]
        public string DeleteFileId { get; set; }
        #endregion
    }
}


