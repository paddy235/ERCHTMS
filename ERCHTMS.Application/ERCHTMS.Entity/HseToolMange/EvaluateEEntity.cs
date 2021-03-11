using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HseToolMange
{
    /// <summary>
    /// 描 述：评估E
    /// </summary>
    [Table("HSE_EVALUATEE")]
    public class EvaluateEEntity : BaseEntity
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
        /// 交通
        /// </summary>
        [Column("TRAFFIC")]
        public string Traffic { get; set; }
        /// <summary>
        /// 交通 其他
        /// </summary>
        [Column("TRAOTHER")]
        public string TraOther { get; set; }
        /// <summary>
        /// 用电
        /// </summary>
        [Column("ELECTRICITY")]
        public string Electricity { get; set; }
        /// <summary>
        /// 用电 其他
        /// </summary>
        [Column("ELEOTHER")]
        public string EleOther { get; set; }
        /// <summary>
        /// 防火
        /// </summary>
        [Column("FIRE")]
        public string Fire { get; set; }
        /// <summary>
        /// 防火 其他
        /// </summary>
        [Column("FIREOTHER")]
        public string FireOther { get; set; }
        /// <summary>
        /// 体力操作
        /// </summary>
        [Column("POWER")]
        public string Power { get; set; }
        /// <summary>
        /// 体力操作 其他
        /// </summary>
        [Column("POWOTHER")]
        public string PowOther { get; set; }
        /// <summary>
        /// 其他
        /// </summary>
        [Column("OTHER")]
        public string Other { get; set; }
        /// <summary>
        /// 其他 其他
        /// </summary>
        [Column("OTHOTHER")]
        public string OthOther { get; set; }

        [Column("ISFILL")]
        public string IsFill { get; set; }
    }
}
