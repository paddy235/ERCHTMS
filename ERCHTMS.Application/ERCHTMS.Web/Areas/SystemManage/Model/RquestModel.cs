using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERCHTMS.Web.Areas.SystemManage.Model
{
    public class RquestModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        /// <returns></returns>
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string CreateUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ModifyUserName { get; set; }
        /// <summary>
        /// 电厂编码
        /// </summary>
        /// <returns></returns>
        public string DeptCode { get; set; }
        /// <summary>
        /// 电厂ID
        /// </summary>
        /// <returns></returns>
        public string DeptId { get; set; }
        /// <summary>
        /// 电厂名称
        /// </summary>
        /// <returns></returns>
        public string DeptName { get; set; }
        /// <summary>
        /// 平台类型 null-Web端 0-window终端 1-Android终端 2-手机APP 
        /// </summary>
        /// <returns></returns>
        public int? PaltformType { get; set; }
        /// <summary>
        /// 菜单ID
        /// </summary>
        /// <returns></returns>
        public string ModuleId { get; set; }
        /// <summary>
        /// 菜单编码
        /// </summary>
        /// <returns></returns>
        public string ModuleCode { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        /// <returns></returns>
        public string ModuleName { get; set; }
        /// <summary>
        /// 是否显示 0 or null-不显示 1-显示
        /// </summary>
        /// <returns></returns>
        public int? IsView { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <returns></returns>
        public string Remark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string BAK2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string BAK3 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string BAK4 { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        /// <returns></returns>
        public int? Sort { get; set; }
        /// <summary>
        /// 上级ID
        /// </summary>
        /// <returns></returns>
        public string ParentId { get; set; }
        /// <summary>
        /// 上级菜单的名称
        /// </summary>
        /// <returns></returns>
        public string ParentName { get; set; }

        /// <summary>
        /// ASSOCIATIONID 关联iD 逗号隔开
        /// </summary>
        /// <returns></returns>
        public string AssociationId { get; set; }
        /// <summary>
        /// AssociationName 关联的菜单的名称 逗号隔开
        /// </summary>
        /// <returns></returns>
        public string AssociationName { get; set; }

        /// <summary>
        /// AUTHORIZEID 授权ID 逗号隔开
        /// </summary>
        /// <returns></returns>
        public string AuthorizeId { get; set; }
        /// <summary>
        /// AUTHORIZENAME 授权对象名称  逗号隔开
        /// </summary>
        /// <returns></returns>
        public string AuthorizeName { get; set; }

        public string MenuIcon { get; set; }
        /// <summary>
        /// 配置
        /// </summary>
        public string JsonData { get; set; }
    }

    public class MenuAuthRquestModel
    {
        public List<string> ModuleIds { get; set; }
        public string departCode { get; set; }
        public string departId { get; set; }
        public string departName { get; set; }
    }

    public class MenuSettingRquestModel {
        public string deptId { get; set; }
        public int themeType { get; set; }
        public int platform { get; set; }

        public string ColumnId { get; set; }
    }

    public class CopyRequestModel {
        public string ThisName { get; set; }
        public string Id { get; set; }
        public string DeptName { get; set; }
        public string DeptCode { get; set; }
        public string DeptId { get; set; }
    }
    /// <summary>
    /// 批量保存栏目取菜单的关系
    /// </summary>
    public class BatchAssociationRequestModel
    {
        public string ColumnId { get; set; }
        public string DeptId { get; set; }
        public string ColumnName { get; set; }
        public List<string> ModuleIds { get; set; }
        public int? PaltformType { get; set; }
    }
}