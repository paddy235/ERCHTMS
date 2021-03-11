using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ERCHTMS.Entity;
using ERCHTMS.Code;

namespace ERCHTMS.Entity.BaseManage
{
    /// <summary>
    /// 描 述：黑名单管理
    /// </summary>
    [Table("BIS_BLACKSET")]
    public class BlackSetEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 角色主键
        /// </summary>		
        [Column("ID")]
        public string Id { get; set; }
         
        /// <summary>
        /// 项目编码
        /// </summary>		
        [Column("ITEMCODE")]
        public string ItemCode { get; set; }
        /// <summary>
        /// 项目值
        /// </summary>		
        [Column("ITEMVALUE")]
        public string ItemValue { get; set; }
        /// <summary>
        /// 所属机构
        /// </summary>		
        [Column("DEPTCODE")]
        public string DeptCode { get; set; }
        /// <summary>
        ///状态
        /// </summary>		
        [Column("STATUS")]
        public int Status { get; set; }
        /// <summary>
        ///排序号
        /// </summary>		
        [Column("SORTCODE")]
        public int SortCode { get; set; }
        /// <summary>
        /// 备用字段
        /// </summary>		
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>		
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
            this.CreateDate = DateTime.Now;
            this.DeptCode = OperatorProvider.Provider.Current().OrganizeCode;
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