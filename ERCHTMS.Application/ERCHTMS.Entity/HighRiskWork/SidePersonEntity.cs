using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.HighRiskWork
{
    /// <summary>
    /// 描 述：旁站监督人员(高风险作业)
    /// </summary>
    [Table("BIS_SIDEPERSON")]
    public class SidePersonEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 创建用户id
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 修改用户id
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 创建用户所属部门编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 创建用户所属机构编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 旁站监督人员
        /// </summary>
        /// <returns></returns>
        [Column("SIDEUSERID")]
        public string SideUserId { get; set; }
        /// <summary>
        /// 旁站监督人员
        /// </summary>
        /// <returns></returns>
        [Column("SIDEUSERNAME")]
        public string SideUserName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        /// <returns></returns>
        [Column("SIDEUSERSEX")]
        public string SideUserSex { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        /// <returns></returns>
        [Column("SIDEUSERIDCARD")]
        public string SideUserIdCard { get; set; }
        /// <summary>
        /// 旁站监督级别
        /// </summary>
        /// <returns></returns>
        [Column("SIDEUSERLEVEL")]
        public string SideUserLevel { get; set; }
        /// <summary>
        /// 单位id
        /// </summary>
        /// <returns></returns>
        [Column("SIDEUSERDEPTID")]
        public string SideUserDeptId { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        /// <returns></returns>
        [Column("SIDEUSERDEPTNAME")]
        public string SideUserDeptName { get; set; }
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