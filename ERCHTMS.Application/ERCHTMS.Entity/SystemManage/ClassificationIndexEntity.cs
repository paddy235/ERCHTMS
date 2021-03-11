using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.SystemManage
{
    /// <summary>
    /// 描 述：分项指标项目表
    /// </summary>
    [Table("BIS_CLASSIFICATIONINDEX")]
    public class ClassificationIndexEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CLASSIFICATIONCODE")]
        public string ClassificationCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CALCULATESTANDARD")]
        public string CalculateStandard { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AFFILIATEDORGANIZENAME")]
        public string AffiliatedOrganizeName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AFFILIATEDORGANIZEID")]
        public string AffiliatedOrganizeId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("AFFILIATEDORGANIZECODE")]
        public string AffiliatedOrganizeCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("INDEXARGSVALUE")]
        public string IndexArgsValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("INDEXSTANDARDFORMAT")]
        public string IndexStandardFormat { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("INDEXSTANDARD")]
        public string IndexStandard { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("INDEXSCORE")]
        public string IndexScore { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns> 
        [Column("INDEXCODE")]
        public string IndexCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("INDEXNAME")]
        public string IndexName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CLASSIFICATIONINDEX")]
        public string ClassificationIndex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CLASSIFICATIONID")]
        public string ClassificationId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        /// <returns></returns>
        [Column("ISENABLE")]
        public string IsEnable { get; set; }  
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
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