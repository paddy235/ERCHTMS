using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERCHTMS.Entity.SafetyLawManage
{
    /// <summary>
    /// 描 述：文件管理树结构
    /// </summary>
    [Table("BIS_SAFEINSTITUTIONTREE")]
    public class SafeInstitutionTreeEntity : BSEntity
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
        /// <summary>
        /// 0代表规章制度，1代表操作规程
        /// </summary>
        /// <returns></returns>
        [Column("DATATYPE")]
        public string DataType { get; set; }
        #endregion
    }
}
