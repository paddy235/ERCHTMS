using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HseToolMange
{
    /// <summary>
    /// 描 述：评估D
    /// </summary>
    [Table("HSE_EVALUATED")]
    public class EvaluateDEntity : BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }


        [Column("EVAID")]
        public string EvaId { get; set; }

        /// <summary>
        /// 本月岗位主要任务
        /// </summary>
        [Column("MAINTASK")]
        public string MainTask { get; set; }
        /// <summary>
        /// 潜在安全风险
        /// </summary>
        [Column("SAFERISK")]
        public string SafeRisk { get; set; }
        /// <summary>
        /// 关键控制措施
        /// </summary>
        [Column("CONTROLMEASURE")]
        public string ControlMeasure { get; set; }

        [Column("ISFILL")]
        public string IsFill { get; set; }
    }
}
