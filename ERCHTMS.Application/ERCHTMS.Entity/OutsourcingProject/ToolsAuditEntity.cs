using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// 描 述：工器具审核表
    /// </summary>
    [Table("EPG_TOOLSAUDIT")]
    public class ToolsAuditEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 审核表ID
        /// </summary>
        /// <returns></returns>
        [Column("AUDITID")]
        public string AUDITID { get; set; }
        /// <summary>
        /// 业务关联表Id
        /// </summary>
        /// <returns></returns>
        [Column("TOOLSID")]
        public string TOOLSID { get; set; }
        /// <summary>
        /// 审核结果：0 通过 1 不通过 2 待审核
        /// </summary>
        /// <returns></returns>
        [Column("AUDITRESULT")]
        public string AUDITRESULT { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        /// <returns></returns>
        [Column("AUDITPEOPLE")]
        public string AUDITPEOPLE { get; set; }
        /// <summary>
        /// 审核日期
        /// </summary>
        /// <returns></returns>
        [Column("AUDITTIME")]
        public DateTime? AUDITTIME { get; set; }
        /// <summary>
        /// 审核部门
        /// </summary>
        /// <returns></returns>
        [Column("AUDITDEPT")]
        public string AUDITDEPT { get; set; }
        /// <summary>
        /// 审核意见
        /// </summary>
        /// <returns></returns>
        [Column("AUDITOPINION")]
        public string AUDITOPINION { get; set; }
        /// <summary>
        /// 备注字段
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string REMARK { get; set; }
        /// <summary>
        /// 审核部门ID
        /// </summary>
        /// <returns></returns>
        [Column("AUDITDEPTID")]
        public string AUDITDEPTID { get; set; }
        /// <summary>
        /// 审核人ID
        /// </summary>
        /// <returns></returns>
        [Column("AUDITPEOPLEID")]
        public string AUDITPEOPLEID { get; set; }

        /// <summary>
        /// 审核附件
        /// </summary>
        /// <returns></returns>
        [Column("AUDITFILE")]
        public string AUDITFILE { get; set; }
        /// <summary>
        /// 审核流程Id
        /// </summary>
        /// <returns></returns>
        [Column("FLOWID")]
        public string FlowId { get; set; }
        
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.AUDITID = string.IsNullOrEmpty(AUDITID) ? Guid.NewGuid().ToString() : AUDITID;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.AUDITID = keyValue;
        }
        #endregion
    }
}
