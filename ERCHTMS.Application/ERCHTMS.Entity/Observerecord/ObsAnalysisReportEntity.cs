using ERCHTMS.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.Observerecord
{
    [Table("BIS_OBSANALYSISREPORT")]
    public class ObsAnalysisReportEntity : BaseEntity
    {
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        [Column("ID")]
        public string ID { get; set; }
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        [Column("WORKUNIT")]
        public string WorkUnit { get; set; }
        [Column("WORKUNITID")]
        public string WorkUnitId { get; set; }
        [Column("WORKUNITCODE")]
        public string WorkUnitCode { get; set; }
        /// <summary>
        /// 专业
        /// </summary>
        [Column("WORKZY")]
        public string WorkZy { get; set; }
        [Column("WORKZYID")]
        public string WorkZyId { get; set; }
        [Column("WORKZYCODE")]
        public string WorkZyCode { get; set; }
        /// <summary>
        /// 报告类型
        /// </summary>
        [Column("REPORTTYPE")]
        public string ReportType { get; set; }
        /// <summary>
        /// 年度
        /// </summary>
        [Column("YEAR")]
        public string Year { get; set; }
        /// <summary>
        /// 季度
        /// </summary>
        [Column("QUARTER")]
        public string Quarter { get; set; }
        /// <summary>
        /// 分析人
        /// </summary>
        [Column("ANALYSISPEOPLE")]
        public string AnalysisPeople { get; set; }

        [Column("ANALYSISPEOPLEID")]
        public string AnalysisPeopleId { get; set; }
        ///分析时间
        [Column("ANALYSISTIME")]
        public DateTime? AnalysisTime { get; set; }
       /// <summary>
       /// 分析内容
       /// </summary>
        [Column("ANALYSISCONTENT")]
        public string AnalysisContent { get; set; }
        /// <summary>
        /// 员工的反应
        /// </summary>
        [Column("YGFY")]
        public string ygfy { get; set; }
        /// <summary>
        ///个人防护装备
        /// </summary>
        [Column("GRFHZB")]
        public string grfhzb { get; set; }
        /// <summary>
        /// 程序标准
        /// </summary>
        [Column("CXBZ")]
        public string cxbz { get; set; }
        /// <summary>
        /// 员工的位置
        /// </summary>
        [Column("YGWZ")]
        public string ygwz { get; set; }
        /// <summary>
        /// 工具设备
        /// </summary>
        [Column("GJSB")]
        public string gjsb { get; set; }
        /// <summary>
        /// 人体工效学
        /// </summary>
        [Column("RTGXX")]
        public string rtgxx { get; set; }
        /// <summary>
        /// 环境整洁
        /// </summary>
        [Column("HJZJ")]
        public string hjzj { get; set; }
         [Column("ISCOMMIT")]
        public int IsCommit { get; set; }
        
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserName = OperatorProvider.Provider.Current().UserName;
            this.CreateUserDeptCode = OperatorProvider.Provider.Current().DeptCode;
            this.CreateUserOrgCode = OperatorProvider.Provider.Current().OrganizeCode;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ID = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
    }
}
