using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ERCHTMS.Entity;

namespace ERCHTMS.Entity.SafetyLawManage
{
    /// <summary>
    /// 描 述：标准制度分类表
    /// </summary>
    [Table("HRS_STDSYSTYPE")]
    public class StdsysTypeEntity : BSEntity
    {
        #region 实体成员        
        /// <summary>
        /// 名称
        /// </summary>
        /// <returns></returns>
        [Column("NAME")]
        public string Name { get; set; }        
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
        [Column("CODE")]
        public string Code { get; set; }
        /// <summary>
        /// 数据范围
        /// </summary>
        /// <returns></returns>
        [Column("SCOPE")]
        public string Scope { get; set; }
        #endregion
    }
}