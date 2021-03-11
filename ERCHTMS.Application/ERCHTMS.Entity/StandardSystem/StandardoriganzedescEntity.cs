using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.StandardSystem
{
    /// <summary>
    /// 描 述：标准化组织机构描述表
    /// </summary>
    [Table("HRS_STANDARDORIGANZEDESC")]
    public class StandardoriganzedescEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("PERSON1")]
        public string PERSON1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("PERSON2")]
        public string PERSON2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("PERSON3")]
        public string PERSON3 { get; set; }
        /// <summary>
        /// 工作职责
        /// </summary>
        /// <returns></returns>
        [Column("WORKDUTY")]
        public string WORKDUTY { get; set; }
        /// <summary>
        /// 机构分类
        /// </summary>
        /// <returns></returns>
        [Column("ORIGANZETYPE")]
        public string ORIGANZETYPE { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;

        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ID = keyValue;
                                            }
        #endregion
    }
}