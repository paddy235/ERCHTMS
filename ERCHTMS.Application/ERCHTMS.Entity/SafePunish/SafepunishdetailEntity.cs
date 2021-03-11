using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.SafePunish
{
    /// <summary>
    /// 描 述：安全考核详细
    /// </summary>
    [Table("BIS_SAFEPUNISHDETAIL")]
    public class SafepunishdetailEntity : BaseEntity
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
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 被考核对象ID
        /// </summary>
        /// <returns></returns>
        [Column("PUNISHNAMEID")]
        public string PunishNameId { get; set; }
        /// <summary>
        /// 考核金额
        /// </summary>
        /// <returns></returns>
        [Column("PUNISHNUM")]
        public int? PunishNum { get; set; }
        /// <summary>
        /// 被考核对象名称
        /// </summary>
        /// <returns></returns>
        [Column("PUNISHNAME")]
        public string PunishName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 安全考核主键
        /// </summary>
        /// <returns></returns>
        [Column("PUNISHID")]
        public string PunishId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 考核对象
        /// </summary>
        /// <returns></returns>
        [Column("PUNISHTYPE")]
        public string PunishType { get; set; }
        /// <summary>
        /// 是否是连带对象  0:不是连带对象  1:是连带对象
        /// </summary>
        /// <returns></returns>
        [Column("TYPE")]
        public string Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 考核绩效
        /// </summary>
        /// <returns></returns>
        [Column("PUNISHPERFORMANCE")]
        public int? PunishPerformance { get; set; }
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
        /// 对象所属部门
        /// </summary>
        [Column("BELONGDEPT")]
        public string BelongDept { get; set; }
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