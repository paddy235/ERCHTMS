using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERCHTMS.Entity.EmergencyPlatform
{
    /// <summary>
    /// 描 述：应急物资检查
    /// </summary>
    [Table("MAE_SUPPLIESCHECK")]
    public class SuppliesCheckEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 检查日期
        /// </summary>
        /// <returns></returns>
        [Column("CHECKDATE")]
        public DateTime? CheckDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 检查人ID
        /// </summary>
        /// <returns></returns>
        [Column("CHECKUSERID")]
        public string CheckUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 不合格项目数量
        /// </summary>
        /// <returns></returns>
        [Column("BADNUM")]
        public int? BadNum { get; set; }
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
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 检查项目数量
        /// </summary>
        /// <returns></returns>
        [Column("CHECKNUM")]
        public int? CheckNum { get; set; }
        /// <summary>
        /// 检查人
        /// </summary>
        /// <returns></returns>
        [Column("CHECKUSERNAME")]
        public string CheckUserName { get; set; }
        /// <summary>
        /// 检查单位
        /// </summary>
        [Column("CHECKUSERDEPT")]
        public string CheckUserDept { get; set; }
        /// <summary>
        /// 检查单位
        /// </summary>
        [Column("CHECKUSERDEPTID")]
        public string CheckUserDeptId { get; set; }
        /// <summary>
        /// 检查单位
        /// </summary>
        [Column("CHECKUSERDEPTCODE")]
        public string CheckUserDeptCode { get; set; }
        /// <summary>
        /// 应急物资检查项目
        /// </summary>
        [NotMapped]
        public List<SuppliesCheckDetailEntity> DetailData { get; set; }
        #endregion

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