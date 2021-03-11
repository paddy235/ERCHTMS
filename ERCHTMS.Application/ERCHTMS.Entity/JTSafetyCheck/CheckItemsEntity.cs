using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Code;

namespace ERCHTMS.Entity.JTSafetyCheck
{
    /// <summary>
    /// 描 述：安全事例
    /// </summary>
    [Table("jt_checkitems")]
    public class CheckItemsEntity : BaseEntity
    {
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 问题项目
        /// </summary>
        /// <returns></returns>
        [Column("ITEMNAME")]
        public string ItemName { get; set; }
        /// <summary>
        /// 整改治理措施
        /// </summary>
        /// <returns></returns>
        [Column("MEASURES")]
        public string Measures { get; set; }
        /// <summary>
        ///计划开始时间
        /// </summary>
        /// <returns></returns>
        [Column("PLANDATE")]
        public DateTime? PlanDate { get; set; }
        /// <summary>
        ///计划完成时间
        /// </summary>
        /// <returns></returns>
        [Column("REALITYDATE")]
        public DateTime? RealityDate { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSER")]
        public string DutyUser { get; set; }
        /// <summary>
        /// 负责人ID
        /// </summary>
        /// <returns></returns>
        [Column("DUTYUSERID")]
        public string DutyUserId { get; set; }
        /// <summary>
        /// 责任部门
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNAME")]
        public string DeptName { get; set; }
        /// <summary>
        ///责任部门编码
        /// </summary>
        /// <returns></returns>
        [Column("DEPTCODE")]
        public string DeptCode { get; set; }
        /// <summary>
        /// 完成情况（未完成，已完成）
        /// </summary>
        /// <returns></returns>
        [Column("RESULT")]
        public string Result { get; set; }
        /// <summary>
        /// 验收人
        /// </summary>
        /// <returns></returns>
        [Column("CHECKUSER")]
        public string CheckUser { get; set; }
        /// <summary>
        /// 验收人ID
        /// </summary>
        /// <returns></returns>
        [Column("CHECKUSERID")]
        public string CheckUserId { get; set; }
        /// <summary>
        /// 关联检查记录ID
        /// </summary>
        /// <returns></returns>
        [Column("CHECKID")]
        public string CheckId { get; set; }
        /// <summary>
        /// 记录创建人ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 记录创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATETIME")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        /// <returns></returns>
        [Column("SORTCODE")]
        public int? SortCode { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// 是否关联发送短消息(1:已发送，其他未发送)
        /// </summary>
        /// <returns></returns>
        [Column("ISSEND")]
        public int? IsSend { get; set; }

        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
            this.CreateTime = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            IsSend = 0;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.Id = keyValue;
        }
    }
}
