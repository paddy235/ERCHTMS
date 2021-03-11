using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.SystemManage
{
    /// <summary>
    /// 描 述：分项指标表
    /// </summary>
    [Table("BIS_CLASSIFICATION")]
    public class ClassificationEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 分享指标
        /// </summary>
        /// <returns></returns>
        [Column("CLASSIFICATIONINDEX")]
        public string ClassificationIndex { get; set; } 
        /// <summary>
        /// 权重比例
        /// </summary>
        /// <returns></returns>
        [Column("WEIGHTCOEFFCIENT")]
        public string WeightCoeffcient { get; set; }
        /// <summary>
        /// 所属机构编码
        /// </summary>
        /// <returns></returns>
        [Column("AFFILIATEDORGANIZECODE")]
        public string AffiliatedOrganizeCode { get; set; } 
        /// <summary>
        /// 所属机构ID
        /// </summary>
        /// <returns></returns>
        [Column("AFFILIATEDORGANIZEID")]
        public string AffiliatedOrganizeId { get; set; } 
        /// <summary>
        /// 所属机构名称
        /// </summary>
        /// <returns></returns>
        [Column("AFFILIATEDORGANIZENAME")]
        public string AffiliatedOrganizeName { get; set; } 
        /// <summary>
        /// 所属年度
        /// </summary>
        /// <returns></returns>
        [Column("AFFILIATEDYEAR")]
        public string AffiliatedYear { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CLASSIFICATIONCODE")]
        public string ClassificationCode { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        /// <returns></returns>
        [Column("CISENABLE")]
        public string CisEnable { get; set; }   
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
