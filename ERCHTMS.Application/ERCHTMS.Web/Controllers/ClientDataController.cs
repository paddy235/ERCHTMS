using ERCHTMS.Cache;
using ERCHTMS.Entity.SystemManage.ViewModel;
using BSFramework.Util;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.AuthorizeManage;
using System.Data;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.BaseManage;
using System.Text;
using System;
using System.Configuration;

namespace ERCHTMS.Web.Controllers
{
    /// <summary>
    /// 描 述：客户端数据
    /// </summary>
    public class ClientDataController : MvcControllerBase
    {
        private DataItemCache dataItemCache = new DataItemCache();
        private OrganizeCache organizeCache = new OrganizeCache();
        private DepartmentCache departmentCache = new DepartmentCache();
        private PostCache postCache = new PostCache();
        private RoleCache roleCache = new RoleCache();
        private UserGroupCache userGroupCache = new UserGroupCache();
        private UserCache userCache = new UserCache();
        private AuthorizeBLL authorizeBLL = new AuthorizeBLL();
        private ModuleBLL moduleBLL = new ModuleBLL();
        #region 获取数据
        /// <summary>
        /// 批量加载数据给客户端（把常用数据全部加载到浏览器中 这样能够减少数据库交互）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult GetClientDataJson()
        {
            var jsonData = new
            {
                organize = this.GetOrganizeData(),            //公司
                                                              //department = this.GetDepartmentData(),          //部门
                                                              // post = this.GetPostData(),                      //岗位
                                                              // role = this.GetRoleData(),                      //角色
                                                              //userGroup = this.GetUserGroupData(),            //用户组
                                                              //user = this.GetUserData(),                      //用户
                                                              //dataItem = this.GetDataItem(),                  //字典
                authorizeMenu = this.GetModuleData(),           //导航菜单
                authorizeButton = this.GetModuleButtonData(),   //功能按钮
                //authorizeColumn = this.GetModuleColumnData(),   //功能视图
            };
            return ToJsonResult(jsonData);
        }
        #endregion
        /// <summary>
        /// 获取用户功能菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult GetMenuDataJson()
        {
            return Json(this.GetModuleData());
        }

        #region 处理基础数据
        /// <summary>
        /// 获取公司数据
        /// </summary>
        /// <returns></returns>
        private object GetOrganizeData()
        {
            var data = organizeCache.GetList();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (OrganizeEntity item in data)
            {
                var fieldItem = new
                {
                    EnCode = item.EnCode,
                    FullName = item.FullName
                };
                dictionary.Add(item.OrganizeId, fieldItem);
            }
            return dictionary;
        }
        /// <summary>
        /// 获取部门数据
        /// </summary>
        /// <returns></returns>
        private object GetDepartmentData()
        {
            var data = departmentCache.GetList();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (DepartmentEntity item in data)
            {
                var fieldItem = new
                {
                    EnCode = item.EnCode,
                    FullName = item.FullName,
                    OrganizeId = item.OrganizeId
                };
                dictionary.Add(item.DepartmentId, fieldItem);
            }
            return dictionary;
        }
        /// <summary>
        /// 获取岗位数据
        /// </summary>
        /// <returns></returns>
        private object GetUserGroupData()
        {
            var data = userGroupCache.GetList();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (RoleEntity item in data)
            {
                var fieldItem = new
                {
                    EnCode = item.EnCode,
                    FullName = item.FullName
                };
                dictionary.Add(item.RoleId, fieldItem);
            }
            return dictionary;
        }
        /// <summary>
        /// 获取岗位数据
        /// </summary>
        /// <returns></returns>
        private object GetPostData()
        {
            var data = postCache.GetList();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (RoleEntity item in data)
            {
                var fieldItem = new
                {
                    EnCode = item.EnCode,
                    FullName = item.FullName
                };
                dictionary.Add(item.RoleId, fieldItem);
            }
            return dictionary;
        }
        /// <summary>
        /// 获取角色数据
        /// </summary>
        /// <returns></returns>
        private object GetRoleData()
        {
            var data = roleCache.GetList();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (RoleEntity item in data)
            {
                var fieldItem = new
                {
                    EnCode = item.EnCode,
                    FullName = item.FullName
                };
                dictionary.Add(item.RoleId, fieldItem);
            }
            return dictionary;
        }
        /// <summary>
        /// 获取用户数据
        /// </summary>
        /// <returns></returns>
        private object GetUserData()
        {
            var data = userCache.GetList();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (UserEntity item in data)
            {
                var fieldItem = new
                {
                    EnCode = item.EnCode,
                    Account = item.Account,
                    RealName = item.RealName,
                    OrganizeId = item.OrganizeId,
                    DepartmentId = item.DepartmentId,
                    Photo = item.HeadIcon
                };
                dictionary.Add(item.UserId, fieldItem);
            }
            return dictionary;
        }
        /// <summary>
        /// 获取数据字典
        /// </summary>
        /// <returns></returns>
        private object GetDataItem()
        {
            var dataList = dataItemCache.GetDataItemList();
            var dataSort = dataList.Distinct(new Comparint<DataItemModel>("EnCode"));
            Dictionary<string, object> dictionarySort = new Dictionary<string, object>();
            foreach (DataItemModel itemSort in dataSort)
            {
                var dataItemList = dataList.Where(t => t.EnCode.Equals(itemSort.EnCode));
                Dictionary<string, string> dictionaryItemList = new Dictionary<string, string>();
                foreach (DataItemModel itemList in dataItemList)
                {
                    dictionaryItemList.Add(itemList.ItemValue, itemList.ItemName);
                }
                foreach (DataItemModel itemList in dataItemList)
                {
                    dictionaryItemList.Add(itemList.ItemDetailId, itemList.ItemName);
                }
                dictionarySort.Add(itemSort.EnCode, dictionaryItemList);
            }
            return dictionarySort;
        }
        #endregion

        #region 处理授权数据
        /// <summary>
        /// 获取功能数据
        /// </summary>
        /// <returns></returns>
        private object GetModuleData()
        {
            var bzweb = string.Empty;
            var appSetting = ConfigurationManager.AppSettings["bzweb"];
            if (appSetting != null) bzweb = appSetting.ToString();

            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            List<ModuleEntity> list = new List<ModuleEntity>();
            DataTable dtModules = new DepartmentBLL().GetDataTable(string.Format("select moduleid from BASE_APPSETTINGASSOCIATION t where t.deptid='{0}' and PALTFORMTYPE=3", user.OrganizeId));
            if (dtModules.Rows.Count > 0)
            {
                string[] mIds = dtModules.AsEnumerable().Select(d => d.Field<string>("moduleid")).ToArray();
                list = moduleBLL.GetListBySql(string.Format("select * from base_module t where moduleid in('{0}') order by sortcode asc", string.Join(",", mIds).Replace(",", "','"))).ToList();
            }
            else
            {
                list = authorizeBLL.GetModuleList(SystemInfo.CurrentUserId).ToList();

            }
            var di = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL();
            string val = new DataItemDetailBLL().GetItemValue("TrainSyncWay");//对接方式，0：账号，1：身份证,不配置默认为账号
            string way = new DataItemDetailBLL().GetItemValue("WhatWay");//对接平台 0：.net培训平台 1:java培训平台
            DepartmentEntity org = new DepartmentBLL().GetEntity(user.OrganizeId);
            foreach (ModuleEntity entity in list)
            {
                if (!string.IsNullOrEmpty(entity.UrlAddress))
                {
                    if (!string.IsNullOrEmpty(bzweb))
                        entity.UrlAddress = entity.UrlAddress.Replace("{bzweb}", bzweb);
                    if (entity.EnCode == "SafetyTrain")
                    {

                        string url = di.GetItemValue("TrainWebUrl");
                        if (!string.IsNullOrEmpty(url))
                        {

                            if (way == "1")
                            {
                                UserEntity ue = new UserBLL().GetEntity(user.UserId);
                                string account = string.IsNullOrWhiteSpace(ue.NewAccount) ? user.Account : ue.NewAccount;
                                entity.UrlAddress = url + "?account=" + account + "&psw=" + Md5Helper.MD5(account, 32).ToLower() + "&companyId=" + org.InnerPhone;
                            }
                            else
                            {
                                if (string.IsNullOrWhiteSpace(user.IdentifyID))
                                {
                                    entity.UrlAddress = url + "?tokenId=" + BSFramework.Util.DESEncrypt.EncryptString(user.Account);
                                }
                                else
                                {
                                    if (!string.IsNullOrWhiteSpace(val))
                                    {
                                        if (val == "0")
                                        {
                                            entity.UrlAddress = url + "?tokenId=" + BSFramework.Util.DESEncrypt.EncryptString(user.Account);
                                        }
                                        else
                                        {
                                            entity.UrlAddress = url + "?tokenId=" + BSFramework.Util.DESEncrypt.EncryptString(user.IdentifyID);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        string bzUrl = di.GetItemValue("bzWebUrl");
                        if (!string.IsNullOrWhiteSpace(bzUrl))
                        {
                            if (entity.UrlAddress.ToLower().StartsWith("http://") && entity.UrlAddress.ToLower().Contains(bzUrl.ToLower()))
                            {
                                string args = args = BSFramework.Util.DESEncrypt.Encrypt(string.Concat(user.Account, "^" + entity.UrlAddress + "^", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "^DLBZ"));
                                entity.UrlAddress = bzUrl + "login/signin?args=" + args;
                            }
                        }

                        if (entity.EnCode == "TaskSchedulerManager")
                        {
                            entity.UrlAddress += "?args=" + BSFramework.Util.DESEncrypt.Encrypt("admin|1", "!2#3@1YV");
                        }
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 获取功能按钮数据
        /// </summary>
        /// <returns></returns>
        private object GetModuleButtonData()
        {
            var data = authorizeBLL.GetModuleButtonList(SystemInfo.CurrentUserId);
            var dataModule = data.Distinct(new Comparint<ModuleButtonEntity>("ModuleId"));
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (ModuleButtonEntity item in dataModule)
            {
                if (!string.IsNullOrEmpty(item.ModuleId))
                {
                    var buttonList = data.Where(t => t.ModuleId.Equals(item.ModuleId));
                    dictionary.Add(item.ModuleId, buttonList);
                }
            }
            return dictionary;

            //var data = authorizeBLL.GetModuleButtonListByUserId(SystemInfo.CurrentUserId);
            //var dataModule = moduleBLL.GetModuleIds();
            //Dictionary<string, object> dictionary = new Dictionary<string, object>();
            //foreach(DataRow item in dataModule.Rows)
            //{
            //    string moduleId = item[0].ToString();
            //    if (!string.IsNullOrEmpty(moduleId))
            //    {
            //        var buttonList = data.Select("ModuleId='" + moduleId + "'");
            //        dictionary.Add(moduleId, buttonList.ToList());
            //    }
            //}
            //return dictionary;
        }
        /// <summary>
        /// 获取功能视图数据
        /// </summary>
        /// <returns></returns>
        private object GetModuleColumnData()
        {
            var data = authorizeBLL.GetModuleColumnList(SystemInfo.CurrentUserId);
            var dataModule = data.Distinct(new Comparint<ModuleColumnEntity>("ModuleId"));
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (ModuleColumnEntity item in dataModule)
            {
                var columnList = data.Where(t => t.ModuleId.Equals(item.ModuleId));
                dictionary.Add(item.ModuleId, columnList);
            }
            return dictionary;
        }
        #endregion
    }
}
