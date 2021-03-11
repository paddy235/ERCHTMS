using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.EmergencyPlatform
{
    /// <summary>
    /// 描 述：应急物资领用申请详细
    /// </summary>
    [Table("MAE_SUPPLIESACCEPT_DETAIL")]
    public class SuppliesAcceptDetailEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        /// <returns></returns>
        [Column("MODELS")]
        public string Models { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 物资名称
        /// </summary>
        /// <returns></returns>
        [Column("SUPPLIESNAME")]
        public string SuppliesName { get; set; }
        /// <summary>
        /// 库存数量
        /// </summary>
        /// <returns></returns>
        [Column("CORRECTNUM")]
        public int? CorrectNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        /// <returns></returns>
        [Column("SUPPLIESUNTILNAME")]
        public string SuppliesUntilName { get; set; }
        /// <summary>
        /// 责任人
        /// </summary>
        /// <returns></returns>
        [Column("USERNAME")]
        public string UserName { get; set; }
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
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 责任部门
        /// </summary>
        /// <returns></returns>
        [Column("DEPARTNAME")]
        public string DepartName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 关联物资申领id
        /// </summary>
        /// <returns></returns>
        [Column("RECID")]
        public string RecId { get; set; }
        /// <summary>
        /// 关联应急物资id
        /// </summary>
        /// <returns></returns>
        [Column("SUPPLIESID")]
        public string SuppliesId { get; set; }
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
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 领用数量
        /// </summary>
        /// <returns></returns>
        [Column("ACCEPTNUM")]
        public int? AcceptNum { get; set; }
        /// <summary>
        /// 责任部门id
        /// </summary>
        /// <returns></returns>
        [Column("DEPARTID")]
        public string DepartId { get; set; }
        /// <summary>
        /// 责任人
        /// </summary>
        /// <returns></returns>
        [Column("USERID")]
        public string UserId { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id)?Guid.NewGuid().ToString():Id;
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