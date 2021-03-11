using ERCHTMS.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.TrainPlan
{
    /// <summary>
    /// 安措计划台账
    /// </summary>
    [Table("BIS_SAFEMEASURE_SUMMARY")]
   public class SafeSummaryEntity:BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Column("ID")]
        public string Id { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        [Column("BELONGYEAR")]
        public int? BelongYear { get; set; }

        /// <summary>
        /// 季度
        /// </summary>
        [Column("QUARTER")]
        public int? Quarter { get; set; }

        /// <summary>
        /// 报告名称
        /// </summary>
        [Column("REPORTNAME")]
        public string ReportName { get; set; }

        /// <summary>
        /// 部门id
        /// </summary>
        [Column("DEPARTMENTID")]
        public string DepartmentId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [Column("DEPARTMENTNAME")]
        public string DepartmentName { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Column("CONTENT")]
        public string Content { get; set; }

        /// <summary>
        /// 编制人id
        /// </summary>
        [Column("OPERATEUSERID")]
        public string OperateUserId { get; set; }

        /// <summary>
        /// 编制人
        /// </summary>
        [Column("OPERATEUSERNAME")]
        public string OperateUserName { get; set; }

        /// <summary>
        /// 编制时间
        /// </summary>
        [Column("OPERATEDATE")]
        public DateTime? OperateDate { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 创建人id
        /// </summary>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }

        /// <summary>
        /// 创建人部门code
        /// </summary>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }

        /// <summary>
        /// 创建人组织Code
        /// </summary>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }

        /// <summary>
        /// 修改人id
        /// </summary>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 状态(0:未提交 1:已提交)
        /// </summary>
        [Column("STATE")]
        public int? State { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        [Column("SUBMITTIME")]
        public DateTime? SubmitTime { get; set; }

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
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
