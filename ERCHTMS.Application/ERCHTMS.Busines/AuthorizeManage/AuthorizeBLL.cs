using ERCHTMS.Code;
using ERCHTMS.Entity.AuthorizeManage;
using ERCHTMS.Entity.AuthorizeManage.ViewModel;
using ERCHTMS.IService.AuthorizeManage;
using ERCHTMS.Service.AuthorizeManage;
using BSFramework.Cache.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ERCHTMS.Busines.AuthorizeManage
{
    /// <summary>
    /// 描 述：授权认证
    /// </summary>
    public class AuthorizeBLL
    {
        private IAuthorizeService service = new AuthorizeService();
        private ModuleBLL moduleBLL = new ModuleBLL();
        private ModuleButtonBLL moduleButtonBLL = new ModuleButtonBLL();
        private ModuleColumnBLL moduleColumnBLL = new ModuleColumnBLL();

        /// <summary>
        /// 获取授权功能
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public IEnumerable<ModuleEntity> GetModuleList(string userId)
        {
            if (OperatorProvider.Provider.Current().IsSystem)
            {
                return moduleBLL.GetList().FindAll(t => t.EnabledMark.Equals(1));
            }
            else
            {
                return service.GetModuleList(userId);
            }
        }
        /// <summary>
        /// 获取授权功能按钮
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public IEnumerable<ModuleButtonEntity> GetModuleButtonList(string userId)
        {
            if (OperatorProvider.Provider.Current().IsSystem)
            {
                return moduleButtonBLL.GetList();
            }
            else
            {
                return service.GetModuleButtonList(userId);
            }
        }
        public DataTable GetModuleButtonListByUserId(string userId)
        {
            if (OperatorProvider.Provider.Current().IsSystem)
            {
                return moduleButtonBLL.GetButtonList();
            }
            else
            {
                return service.GetModuleButtonListByUserId(userId);
            }
        }
        /// <summary>
        /// 获取授权功能视图
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public IEnumerable<ModuleColumnEntity> GetModuleColumnList(string userId)
        {
            if (OperatorProvider.Provider.Current().IsSystem)
            {
                return moduleColumnBLL.GetList();
            }
            else
            {
                return service.GetModuleColumnList(userId);
            }
        }
        /// <summary>
        /// 获取授权功能Url、操作Url
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public IEnumerable<AuthorizeUrlModel> GetUrlList(string userId)
        {
            return service.GetUrlList(userId);
        }
        /// <summary>
        /// Action执行权限认证
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="action">请求地址</param>
        /// <returns></returns>
        public bool ActionAuthorize(string userId, string moduleId, string action)
        {
            List<AuthorizeUrlModel> authorizeUrlList = new List<AuthorizeUrlModel>();
            var cacheList = CacheFactory.Cache().GetCache<List<AuthorizeUrlModel>>("AuthorizeUrl_" + userId);
            if (cacheList == null)
            {
                authorizeUrlList = this.GetUrlList(userId).ToList();
                CacheFactory.Cache().WriteCache(authorizeUrlList, "AuthorizeUrl_" + userId, DateTime.Now.AddMinutes(1));
            }
            else
            {
                authorizeUrlList = cacheList;
            }
            if (!string.IsNullOrWhiteSpace(moduleId))
            {
                authorizeUrlList = authorizeUrlList.FindAll(t => t.ModuleId == moduleId);
            }
            if (authorizeUrlList.Count>0)
            {
                return true;
            }
            //foreach (AuthorizeUrlModel item in authorizeUrlList)
            //{
            //    if (!string.IsNullOrEmpty(item.UrlAddress))
            //    {
            //        // string[] url = item.UrlAddress.Split('?');
            //        // if (item.ModuleId == moduleId && url[0] == action)
            //        // if (item.ModuleId == moduleId && action.Contains(url[0]))
            //        if(item.ModuleId == moduleId)
            //        {
            //            return true;
            //        }
            //    }
            //}
            return false;
        }
        /// <summary>
        /// 获得权限范围用户ID
        /// </summary>
        /// <param name="operators">当前登陆用户信息</param>
        /// <param name="isWrite">可写入</param>
        /// <returns></returns>
        public string GetDataAuthorUserId(Operator operators, bool isWrite = false)
        {
            return service.GetDataAuthorUserId(operators, isWrite);
        }
        /// <summary>
        /// 获得可读数据权限范围SQL
        /// </summary>
        /// <param name="operators">当前登陆用户信息</param>
        /// <param name="isWrite">可写入</param>
        /// <returns></returns>
        public string GetDataAuthor(Operator operators, bool isWrite = false)
        {
            return service.GetDataAuthor(operators, isWrite);
        }
        /// <summary>
        /// 获取用户对模块的数据权限（1:本人,2:本部门，3:本部门及下属部门，4:本机构，5:全部）
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public string GetModuleDataAuthority(Operator user, string moduleId, string deptCode, string orgCode)
        {
            return service.GetModuleDataAuthority(user, moduleId, deptCode, orgCode);
        }
        public string GetModuleDataAuthority(Operator user, string moduleId)
        {
            return service.GetModuleDataAuthority(user, moduleId);
        }
         /// <summary>
        /// 判断用户对模块有无指定的操作权限
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <param name="moduleId">模块Id</param>
        ///  <param name="enCode">功能编号，如add(新增),edit(修改),del(删除),export(导出)等</param>
        /// <returns></returns>
        public bool HasOperAuthority(Operator user, string moduleId, string enCode)
        {
            return service.HasOperAuthority(user, moduleId, enCode);
        }
        public bool HasOperAuthorityEncode(Operator user, string moduleEncode, string enCode)
        {
            return service.HasOperAuthorityEncode(user, moduleEncode, enCode);
        }
        /// <summary>
        /// 获取用户对模块的数据的修改和删除权限（本人,本部门，本部门及下属部门，本机构，全部）
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="jsonData">json集合字符串，如[{UserId:'1',DeptCode:'0001',OrgCode:'00'},{UserId:'2',DeptCode:'0002',OrgCode:'00'}]</param>
        /// <returns>json集合字符串</returns>
        public string GetDataAuthority(Operator user, string moduleId, string jsonData)
        {
            return service.GetDataAuthority(user, moduleId, jsonData);
        }
           /// <summary>
        /// 判断用户对模块有无指定的操作权限
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <param name="moduleId">模块Id</param>
        ///  <param name="enCode">功能编号，如add(新增),edit(修改),delete(删除),export(导出)等，多个可用英文逗号分隔</param>
        /// <returns></returns>
        public string GetOperAuthority(Operator user, string moduleId, string enCode)
        {
            return service.GetOperAuthority(user, moduleId, enCode);
        }
        /// <summary>
        /// 获取用户对模块的数据操作权限
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <param name="moduleId">模块Id</param>
        ///  <param name="enCode">功能编号，如add(新增),edit(修改),delete(删除),export(导出)等</param>
        /// <returns>1:本人,2:本部门，3:本部门及下属部门，4:本机构，5:全部</returns>
        public string GetOperAuthorzeType(Operator user, string moduleId, string enCode) 
        {
            return service.GetOperAuthorzeType(user, moduleId, enCode);
        }
        public string GetOperAuthorzeTypeEncode(Operator user, string moduleEncode, string enCode)
        {
            return service.GetOperAuthorzeTypeEncode(user, moduleEncode, enCode);
        }
    }
}
