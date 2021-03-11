using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.RoutineSafetyWork
{
    /// <summary>
    /// 描 述：文件管理树结构
    /// </summary>
    [Table("BIS_FILEMANAGETREE")]
    public class FileManageTreeEntity : BSEntity
    {
        #region 实体成员
        /// <summary>
        /// 名称
        /// </summary>
        /// <returns></returns>
        [Column("TREENAME")]
        public string TreeName { get; set; }
        /// <summary>
        /// 上级id
        /// </summary>
        /// <returns></returns>
        [Column("PARENTID")]
        public string ParentId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        /// <returns></returns>
        [Column("TREECODE")]
        public string TreeCode { get; set; }
        #endregion
    }
}
