using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HseToolMange
{
    /// <summary>
    /// 描 述：HSE自我评估信息表
    /// </summary>
    [Table("HSE_SELFEVALUATE")]
    public class SelfEvaluateEntity : BaseEntity
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
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 填报日期
        /// </summary>
        /// <returns></returns>
        [Column("FILLDATE")]
        public DateTime? FillDate { get; set; }

        /// <summary>
        /// 提交日期
        /// </summary>
        /// <returns></returns>
        [Column("SUBMITDATE")]
        public DateTime? SubmitDate { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSER")]
        public string CreateUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DEPTID")]
        public string DeptId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNAME")]
        public string DeptName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("DEPTCODE")]
        public string DeptCode { get; set; }

        /// <summary>
        /// 是否填报
        /// </summary>
        /// <returns></returns>
        [Column("ISFILL")]
        public string IsFill { get; set; }

        /// <summary>
        /// 是否提交
        /// </summary>
        /// <returns></returns>
        [Column("ISSUBMIT")]
        public string IsSubmit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("SUMMARY")]
        public string Summary { get; set; }

        [Column("YEAR")]
        public string Year { get; set; }

        [Column("MONTH")]
        public string Month { get; set; }
        [NotMapped]
        public EvaluateAEntity A { get; set; }
        [NotMapped]
        public EvaluateBEntity B { get; set; }
        [NotMapped]
        public EvaluateCEntity C { get; set; }
        [NotMapped]
        public EvaluateDEntity D { get; set; }
        [NotMapped]
        public EvaluateEEntity E { get; set; }

        [NotMapped]
        public EvaluateGroupSummaryEntity  SummaryEntity { get; set; }
    }
}
