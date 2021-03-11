using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HseToolMange
{
    /// <summary>
    /// 描 述：评估C
    /// </summary>
    [Table("HSE_EVALUATEC")]
    public class EvaluateCEntity : BaseEntity
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
        /// 安全观察卡
        /// </summary>
        [Column("AQGCK")]
        public Int32? AQGCK { get; set; }
        /// <summary>
        /// 领先指标卡
        /// </summary>
        [Column("LXZBK")]
        public Int32? LXZBK { get; set; }
        /// <summary>
        /// 安全会议
        /// </summary>
        [Column("AQHY")]
        public Int32? AQHY { get; set; }
        /// <summary>
        /// 作业安全交底
        /// </summary>
        [Column("ZYAQJD")]
        public Int32? ZYAQJD { get; set; }
        /// <summary>
        /// 安全检查
        /// </summary>
        [Column("AQJC")]
        public Int32? AQJC { get; set; }
        /// <summary>
        /// 安全培训
        /// </summary>
        [Column("AQPX")]
        public Int32? AQPX { get; set; }

        [Column("ISFILL")]
        public string IsFill { get; set; }
    }
}
