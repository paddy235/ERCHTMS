using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HseToolMange
{
    [Table("HSE_EVALUATEGROUPSUMMARY")]
    public class EvaluateGroupSummaryEntity : BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Column("YEAR")]
        public string Year { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MONTH")]
        public string Month { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Column("CONTENT")]
        public string Content { get; set; }
        /// <summary>
        /// 部门Id
        /// </summary>
        [Column("DEPTID")]
        public string DeptId { get; set; }
    }
}
