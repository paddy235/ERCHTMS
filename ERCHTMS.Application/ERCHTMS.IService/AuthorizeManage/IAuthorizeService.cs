using ERCHTMS.Code;
using ERCHTMS.Entity.AuthorizeManage;
using ERCHTMS.Entity.AuthorizeManage.ViewModel;
using ERCHTMS.Entity.BaseManage;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.AuthorizeManage
{
    /// <summary>
    /// 描 述：授权认证
    /// </summary>
    public interface IAuthorizeService
    {
        /// <summary>
        /// 获取授权功能
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        IEnumerable<ModuleEntity> GetModuleList(string userId);
        /// <summary>
        /// 获取授权功能按钮
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        IEnumerable<ModuleButtonEntity> GetModuleButtonList(string userId);
        DataTable GetModuleButtonListByUserId(string userId);
        /// <summary>
        /// 获取授权功能视图
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        IEnumerable<ModuleColumnEntity> GetModuleColumnList(string userId);
        /// <summary>
        /// 获取授权功能Url、操作Url
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        IEnumerable<AuthorizeUrlModel> GetUrlList(string userId);
        /// <summary>
        /// 获取关联用户关系
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        IEnumerable<UserRelationEntity> GetUserRelationList(string userId);
        /// <summary>
        /// 获得权限范围用户ID
        /// </summary>
        /// <param name="operators">当前登陆用户信息</param>
        /// <param name="isWrite">可写入</param>
        /// <returns></returns>
        string GetDataAuthorUserId(Operator operators, bool isWrite = false);
        /// <summary>
        /// 获得可读数据权限范围SQL
        /// </summary>
        /// <param name="operators">当前登陆用户信息</param>
        /// <param name="isWrite">可写入</param>
        /// <returns></returns>
        string GetDataAuthor(Operator operators, bool isWrite = false);
           /// <summary>
        /// 获取用户对模块的数据权限（本人,本部门，本部门及下属部门，本机构，全部）
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        string GetModuleDataAuthority(Operator user, string moduleId, string deptCode = "createuserdeptcode", string orgCode = "createuserorgcode");
         /// <summary>
        /// 判断用户对模块有无指定的操作权限
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <param name="moduleId">模块Id</param>
        ///  <param name="enCode">功能编号，如add(新增),edit(修改),del(删除),export(导出)等</param>
        /// <returns></returns>
        bool HasOperAuthority(Operator user, string moduleId, string enCode);
        bool HasOperAuthorityEncode(Operator user, string moduleEncode, string enCode);
         /// <summary>
         /// 获取用户对模块的数据的修改和删除权限（本人,本部门，本部门及下属部门，本机构，全部）
         /// </summary>
         /// <param name="user">当前用户</param>
         /// <param name="moduleId">模块Id</param>
         /// <param name="jsonData">json集合字符串，如[{UserId:'1',DeptCode:'0001',OrgCode:'00'},{UserId:'2',DeptCode:'0002',OrgCode:'00'}]</param>
         /// <returns>json集合字符串</returns>
        string GetDataAuthority(Operator user, string moduleId, string jsonData);
           /// <summary>
        /// 判断用户对模块有无指定的操作权限
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <param name="moduleId">模块Id</param>
        ///  <param name="enCode">功能编号，如add(新增),edit(修改),delete(删除),export(导出)等，多个可用英文逗号分隔</param>
        /// <returns></returns>
        string GetOperAuthority(Operator user, string moduleId, string enCode);
        /// <summary>
        /// 获取用户对模块的数据操作权限
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <param name="moduleId">模块Id</param>
        ///  <param name="enCode">功能编号，如add(新增),edit(修改),delete(删除),export(导出)等</param>
        /// <returns></returns>
        string GetOperAuthorzeType(Operator user, string moduleId, string enCode);
        string GetOperAuthorzeTypeEncode(Operator user, string moduleEncode, string enCode);
    }
}
