using ERCHTMS.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.SystemManage
{
    /// <summary>
    /// 菜单配置
    /// </summary>
    [Table("BASE_MENUCONFIG")]
    public  class MenuConfigEntity : BaseEntity
    {

        #region 实体成员
        /// <summary>
        /// 主键ID
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
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
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
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
        /// 电厂编码
        /// </summary>
        /// <returns></returns>
        [Column("DEPTCODE")]
        public string DeptCode { get; set; }
        /// <summary>
        /// 电厂ID
        /// </summary>
        /// <returns></returns>
        [Column("DEPTID")]
        public string DeptId { get; set; }
        /// <summary>
        /// 电厂名称
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNAME")]
        public string DeptName { get; set; }
        /// <summary>
        /// 平台类型 null-Web端 0-window终端 1-Android终端 2-手机APP  3-Web平台
        /// </summary>
        /// <returns></returns>
        [Column("PALTFORMTYPE")]
        public int? PaltformType { get; set; }
        /// <summary>
        /// 菜单ID
        /// </summary>
        /// <returns></returns>
        [Column("MODULEID")]
        public string ModuleId { get; set; }
        /// <summary>
        /// 菜单编码
        /// </summary>
        /// <returns></returns>
        [Column("MODULECODE")]
        public string ModuleCode { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        /// <returns></returns>
        [Column("MODULENAME")]
        public string ModuleName { get; set; }
        /// <summary>
        /// 是否显示 0 or null-不显示 1-显示
        /// </summary>
        /// <returns></returns>
        [Column("ISVIEW")]
        public int? IsView { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        [Column("BAK1")]
        public string Remark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("BAK2")]
        public string BAK2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("BAK3")]
        public string BAK3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("BAK4")]
        public string BAK4 { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        /// <returns></returns>
        [Column("SORT")]
        public int? Sort { get; set; }
        /// <summary>
        /// 上级ID
        /// </summary>
        /// <returns></returns>
        [Column("PARENTID")]
        public string ParentId { get; set; }
        /// <summary>
        /// 上级菜单的名称
        /// </summary>
        /// <returns></returns>
        [Column("PARENTNAME")]
        public string ParentName { get; set; }

        /// <summary>
        /// ASSOCIATIONID 关联iD 逗号隔开
        /// </summary>
        /// <returns></returns>
        [Column("ASSOCIATIONID")]
        public string AssociationId { get; set; }
        /// <summary>
        /// AssociationName 关联的菜单的名称 逗号隔开
        /// </summary>
        /// <returns></returns>
        [Column("ASSOCIATIONNAME")]
        public string AssociationName { get; set; }
        

        /// <summary>
        /// AUTHORIZEID 授权ID 逗号隔开
        /// </summary>
        /// <returns></returns>
        [Column("AUTHORIZEID")]
        public string AuthorizeId { get; set; }
        /// <summary>
        /// AUTHORIZENAME 授权对象名称  逗号隔开
        /// </summary>
        /// <returns></returns>
        [Column("AUTHORIZENAME")]
        public string AuthorizeName { get; set; }
        /// <summary>
        /// 菜单图标
        /// </summary>
        [Column("MENUICON")]
        public string MenuIcon { get; set; }

        /// <summary>
        /// 配置
        /// </summary>
        [Column("JSONDATA")]
        public string JsonData { get; set; }
        
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id) ? Guid.NewGuid().ToString() : Id;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
            this.CreateUserName = OperatorProvider.Provider.Current().UserName;
            this.ModifyUserName  = OperatorProvider.Provider.Current().UserName;
            this.ModuleId = Id;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = OperatorProvider.Provider.Current().UserId;
            this.ModifyUserName = OperatorProvider.Provider.Current().UserName;
            this.Id = keyValue;
        }
        #endregion
    }
}
