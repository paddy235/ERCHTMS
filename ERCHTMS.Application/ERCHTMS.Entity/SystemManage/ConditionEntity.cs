using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.SystemManage
{
    /// <summary>
    /// 描 述：流程跳转
    /// </summary>
    [Table("SYS_WFTBCONDITION")]
    public class ConditionEntity : BaseEntity
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
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("OPERDATE")]
        public DateTime? OperDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AUTOID")]
        public int? AutoId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("OPERUSER")]
        public string OperUser { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("EXPRESSION")]
        public string Expression { get; set; }
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
        [Column("CREATEUSER")]
        public string CreateUser { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("TOACTIVITYID")]
        public string ToActivityId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ACTIVITYID")]
        public string ActivityId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("PROCESSID")]
        public string ProcessId { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
            this.CreateDate = DateTime.Now;
            this.OperDate = DateTime.Now;
            this.OperUser = OperatorProvider.Provider.Current().UserId;
            this.CreateUser = OperatorProvider.Provider.Current().UserId;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.Id = keyValue;
            this.OperDate = DateTime.Now;
            this.OperUser = OperatorProvider.Provider.Current().UserId;
        }
        #endregion
    }
}