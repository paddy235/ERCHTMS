using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// 描 述：电厂模板管理
    /// </summary>
    [Table("EPG_TEMPMANAGE")]
    public class TempConfigEntity : BaseEntity
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
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
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
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
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
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
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
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 模块名称
        /// </summary>
        /// <returns></returns>
        [Column("MODULENAME")]
        public string ModuleName { get; set; }
        /// <summary>
        /// 模块Code
        /// </summary>
        /// <returns></returns>
        [Column("MODULECODE")]
        public string ModuleCode { get; set; }
        /// <summary>
        /// 模板名称
        /// </summary>
        /// <returns></returns>
        [Column("TEMPNAME")]
        public string TempName { get; set; }
        /// <summary>
        /// 处理方式
        /// </summary>
        /// <returns></returns>
        [Column("PROESSMODE")]
        public string ProessMode { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// 所属电厂
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNAME")]
        public string DeptName { get; set; }
        /// <summary>
        /// 所属电厂
        /// </summary>
        /// <returns></returns>
        [Column("DEPTCODE")]
        public string DeptCode { get; set; }
        /// <summary>
        /// 所属电厂
        /// </summary>
        /// <returns></returns>
        [Column("DEPTID")]
        public string DeptId { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;
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
            this.ID = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
        }
        #endregion
    }
}