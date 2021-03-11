using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.TrainPlan
{
    /// <summary>
    /// 按错计划调整信息
    /// </summary>
    [Table("BIS_SAFEMEASURE_ADJUSTMENT")]
    public class SafeAdjustmentEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Column("ID")]
        public string ID { get; set; }

        /// <summary>
        /// 调整原因
        /// </summary>
        [Column("APPLYREASON")]
        public string ApplyReason { get; set; }

        /// <summary>
        /// 是否延期
        /// </summary>
        [Column("ISDELAY")]
        public int? IsDelay { get; set; } = 0;

        /// <summary>
        /// 延期天数
        /// </summary>
        [Column("DELAYDAYS")]
        public int? DelayDays { get; set; }

        /// <summary>
        /// 是否调整费用
        /// </summary>
        [Column("ISADJUSTFEE")]
        public int? IsAdjustFee { get; set; }

        /// <summary>
        /// 调整费用(万元)
        /// </summary>
        [Column("ADJUSTFEE")]
        public decimal? AdjustFee { get; set; }

        /// <summary>
        /// 申请人ID
        /// </summary>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }

        /// <summary>
        /// 申请人
        /// </summary>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }

        /// <summary>
        /// 申请人所在部门ID
        /// </summary>
        [Column("APPLYDEPTID")]
        public string ApplyDeptId { get; set; }

        /// <summary>
        /// 申请人所在部门
        /// </summary>
        [Column("APPLYDEPTNAME")]
        public string ApplyDeptName { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 创建人所在部门编号
        /// </summary>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }

        /// <summary>
        /// 创建人所在机构编号
        /// </summary>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }

        /// <summary>
        /// 按错计划表主键
        /// </summary>
        [Column("SAFEMEASUREID")]
        public string SafeMeasureId { get; set; }

        /// <summary>
        /// 调整状态
        /// </summary>
        [Column("ADJUSTSTAUTS")]
        public string AdjustStauts { get; set; }

        /// <summary>
        /// (0:无调整 1:调整申请 2:调整审批 3:结束)
        /// </summary>
        [Column("PROCESSSTATE")]
        public int? ProcessState { get; set; }
    }
}
