using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HseToolMange
{
    /// <summary>
    /// 描 述：评估B
    /// </summary>
    [Table("HSE_EVALUATEB")]
    public class EvaluateBEntity : BaseEntity
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
        /// 电工培训
        /// </summary>
        [Column("DGPX")]
        public string DGPX { get; set; }
        /// <summary>
        /// 起重吊装培训
        /// </summary>
        [Column("QZDZPX")]
        public string QZDZPX { get; set; }
        /// <summary>
        /// 场内机动车培训
        /// </summary>
        [Column("CNJDCPX")]
        public string CNJDCPX { get; set; }
        /// <summary>
        /// 压力容器
        /// </summary>
        [Column("YLRQ")]
        public string YLRQ { get; set; }
        /// <summary>
        /// 锅炉水培训
        /// </summary>
        [Column("GLSPX")]
        public string GLSPX { get; set; }
        /// <summary>
        /// 锅炉作业培训
        /// </summary>
        [Column("GLZYPX")]
        public string GLZYPX { get; set; }
        /// <summary>
        /// 电焊作业培训
        /// </summary>
        [Column("DHZYPX")]
        public string DHZYPX { get; set; }
        /// <summary>
        /// 脚手架作业培训
        /// </summary>
        [Column("JSJZYPX")]
        public string JSJZYPX { get; set; }
        /// <summary>
        /// 高空作业培训
        /// </summary>
        [Column("GKZYPX")]
        public string GKZYPX { get; set; }
        /// <summary>
        /// 急救培训
        /// </summary>
        [Column("JJPX")]
        public string JJPX { get; set; }
        /// <summary>
        /// 无具体培训要求
        /// </summary>
        [Column("NONEPX")]
        public string NONEPX { get; set; }

        [Column("ISFILL")]
        public string IsFill { get; set; }
    }
}
