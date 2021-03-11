using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.DangerousJob
{
    /// <summary>
    /// 描 述：动火作业分析表
    /// </summary>
    [Table("BIS_WHENHOTANALYSIS")]
    public class WhenHotAnalysisEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 分析数据
        /// </summary>
        /// <returns></returns>
        [Column("ANALYSISDATA")]
        public string AnalysisData { get; set; }
        /// <summary>
        /// 分析人
        /// </summary>
        /// <returns></returns>
        [Column("ANALYSISPERSON")]
        public string AnalysisPerson { get; set; }
        /// <summary>
        /// 采样地点
        /// </summary>
        /// <returns></returns>
        [Column("SAMPLINGPLACE")]
        public string SamplingPlace { get; set; }
        /// <summary>
        /// 动火分析时间
        /// </summary>
        /// <returns></returns>
        [Column("ANALYSISDATE")]
        public DateTime? AnalysisDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 分析人
        /// </summary>
        /// <returns></returns>
        [Column("ANALYSISPERSONID")]
        public string AnalysisPersonId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 关联动火作业id
        /// </summary>
        /// <returns></returns>
        [Column("RECID")]
        public string RecId { get; set; }

        /// <summary>
        /// 有毒有害介质分析数据
        /// </summary>
        [Column("DANGEROUSDATA")]
        public string DangerousData { get; set; }

        /// <summary>
        /// 可燃气分析数据
        /// </summary>
        [Column("GASDATA")]
        public string GasData { get; set; }

        /// <summary>
        /// 含氧量分析数据
        /// </summary>
        [Column("OXYGENCONTENTDATA")]
        public string OxygenContentData { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id)?Guid.NewGuid().ToString():Id;
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
            this.Id = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}