using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HseToolMange
{
    /// <summary>
    /// 描 述：评估A
    /// </summary>
    [Table("HSE_EVALUATEA")]
    public class EvaluateAEntity : BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }

        /// <summary>
        /// 评估id
        /// </summary>
        [Column("EVAID")]
        public string EvaId { get; set; }
        /// <summary>
        /// 安全危害
        /// </summary>
        [Column("DANGER")]
        public string Danger { get; set; }
        /// <summary>
        /// 使用的PPE
        /// </summary>
        [Column("USEPPE")]
        public string UsePPE { get; set; }
        /// <summary>
        /// 其他防护装备
        /// </summary>
        [Column("OTHEREQUIP")]
        public string OtherEquip { get; set; }
        [Column("ISFILL")]
        public string IsFill { get; set; }


    }
}
