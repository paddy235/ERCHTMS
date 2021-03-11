using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.RiskDatabase
{
    /// <summary>
    /// 描 述：预知训练作业风险措施
    /// </summary>
    [Table("BIS_TRAINMEASURES")]
    public class TrainmeasuresEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 关联风险训练Id
        /// </summary>
        /// <returns></returns>
        [Column("WORKID")]
        public string WorkId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("RISKCONTENT")]
        public string RiskContent { get; set; }
        /// <summary>
        /// 状态，0：未读，1：已读
        /// </summary>
        /// <returns></returns>
        [Column("STATUS")]
        public int? Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MEASURE")]
        public string Measure { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("LSPEOPLE")]
        public string LsPeople { get; set; }
        [Column("LSPEOPLEID")]
        public string LsPeopleId { get; set; }
        [Column("SEQ")]
        public int? Seq { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id= string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
            this.CreateDate = DateTime.Now;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.Id = keyValue;
        }
        #endregion
    }
}