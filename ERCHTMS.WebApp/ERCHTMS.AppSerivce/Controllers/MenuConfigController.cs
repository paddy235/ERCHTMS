using BSFramework.Util.WebControl;
using ERCHTMS.AppSerivce.Model;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ERCHTMS.Busines.BaseManage;
using BSFramework.Util.Log;
using System.Web;
using BSFramework.Util;
using BSFramework.Util.Attributes;
using System.Linq.Expressions;
using BSFramework.Util.Extension;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.AppSerivce.Models;

namespace ERCHTMS.AppSerivce.Controllers
{
    public class MenuConfigController : BaseApiController
    {
        /// <summary>
        /// 根据获取，获取用户可获取的已授权菜单
        /// </summary>
        /// <param name="json">userid 用户ID  PaltformType平台类型 0 windows终端 </param>
        /// <returns></returns>
        [HttpPost]
        public object GetMenuConfigList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                //string deptId = dy.data.DeptId;//电厂ID
                long paltformType = dy.data.PaltformType;//平台类型
                UserEntity user = new UserBLL().GetEntity(userId);
                //OperatorProvider.AppUserId = userId;  //设置当前用户
                //Operator user = OperatorProvider.Provider.Current();
                if (user == null)
                {
                    LogEntity logEntity = new LogEntity();
                    logEntity.CategoryId = 4;
                    logEntity.OperateTypeId = ((int)OperationType.Exception).ToString();
                    logEntity.OperateType = EnumAttribute.GetDescription(OperationType.Exception);
                    if (null != OperatorProvider.Provider.Current())
                    {
                        logEntity.OperateUserId = OperatorProvider.Provider.Current().UserId;
                    }
                    logEntity.ExecuteResult = -1;
                    logEntity.ExecuteResultJson = "用户或用户的角色为空";
                    logEntity.Module = "api：GetMenuConfigList 获取用户可获取的已授权菜单";
                    logEntity.ModuleId = SystemInfo.CurrentModuleId;
                    logEntity.WriteLog();
                    return new { Code = -1, Info = "获取数据失败", Message = "用户或用户的角色为空" };
                }
                List<string> roleIds = user.RoleId.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var data = new MenuConfigBLL().GetList(user.OrganizeId, Convert.ToInt32(paltformType), roleIds);
                //List<object> objlist = new List<object>();
                //foreach (var item in data)
                //{
                //    var obj = new
                //    {
                //        id = item.ModuleId,
                //        name = item.ModuleName,
                //        key = item.ModuleCode,
                //        index = item.Sort
                //    };
                //    objlist.Add(obj);
                //}
                return new { Code = 0, Count = data.Count(), Info = "获取数据成功", data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Info = "获取数据失败", ex.Message };
            }
        }
        /// <summary>
        /// 获取授权信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>

        [HttpPost]
        public object GetAuthorizeInfo([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                MenuConfigRequestModel dy = JsonConvert.DeserializeObject<MenuConfigRequestModel>(res);
                string registcode = dy.registcode;
                string userId = dy.userId;
                //获取用户基本信息
                MenuAuthorizeBLL authorizeBLL = new MenuAuthorizeBLL();
                List<MenuAuthorizeEntity> data = authorizeBLL.GetListByRegistCode(registcode);
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    UserEntity user = new UserBLL().GetEntity(userId);
                    var userRoleIds = user.RoleId.Split(',');
                    TCRuleBLL tCRuleBLL = new TCRuleBLL();
                    List<TCRuleEntity> ruleEntities = tCRuleBLL.GetList(data.Select(p => p.Id).ToList());
                    if (data != null && data.Count > 0)
                    {
                        data.ForEach(m =>
                        {
                            List<TCRuleEntity> ts = ruleEntities.Where(x => x.AuthorizCodeId == m.Id).ToList();
                            try
                            {
                                m.ThemeType = ts.FirstOrDefault(p => p.InfoType == 1 && userRoleIds.Any(x => p.RuleIds.Contains(x))) == null ? m.ThemeType : Convert.ToInt32(ts.FirstOrDefault(p => p.InfoType == 1 && userRoleIds.Any(x => p.RuleIds.Contains(x))).InfoValue);

                            }
                            catch (Exception)
                            {
                                m.ThemeType = m.ThemeType;
                            }
                            m.CulturalUrl = ts.FirstOrDefault(p => p.InfoType == 2 && userRoleIds.Any(x => p.RuleIds.Contains(x))) == null ? m.CulturalUrl : ts.FirstOrDefault(p => p.InfoType == 2 && userRoleIds.Any(x => p.RuleIds.Contains(x))).InfoValue;
                            m.IndexUrl = ts.FirstOrDefault(p => p.InfoType == 3 && userRoleIds.Any(x => p.RuleIds.Contains(x))) == null ? m.IndexUrl : ts.FirstOrDefault(p => p.InfoType == 3 && userRoleIds.Any(x => p.RuleIds.Contains(x))).InfoValue;
                        });
                    }
                }

                return new { Code = 0, data.Count, Info = "获取数据成功", data };

            }
            catch (Exception ex)
            {
                return new { Code = -1, Info = "获取数据失败", ex.Message };
            }

        }
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetMenuList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;//用户名
                long themeTypeParam = dy.data.themetype;//0 第一套工作栏  1 第二套
                long platformParam = dy.data.platform;//2 手机APP 1 安卓终端
                int themeType = int.Parse(themeTypeParam.ToString());
                int platform = int.Parse(platformParam.ToString());
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                if (user == null || user.RoleId == null)
                {


                    //logMessage.ExceptionSource = Error.Source;
                    //logMessage.ExceptionRemark = Error.StackTrace;

                    LogEntity logEntity = new LogEntity();
                    logEntity.CategoryId = 4;
                    logEntity.OperateTypeId = ((int)OperationType.Exception).ToString();
                    logEntity.OperateType = EnumAttribute.GetDescription(OperationType.Exception);
                    if (null != OperatorProvider.Provider.Current())
                    {
                        logEntity.OperateUserId = OperatorProvider.Provider.Current().UserId;
                    }
                    logEntity.ExecuteResult = -1;
                    logEntity.ExecuteResultJson = "用户或用户的角色为空";
                    logEntity.Module = "菜单配置";
                    logEntity.ModuleId = SystemInfo.CurrentModuleId;
                    logEntity.WriteLog();
                    return new { Code = -1, Info = "获取数据失败", Message = "用户或用户的角色为空" };
                }
                List<string> roleId = user.RoleId.Replace(" ", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (user.IsTrainAdmin == 1)
                {
                    roleId.Add("15006a63-94d8-479f-8478-575e567671bc");
                }
                else
                {
                    roleId.Add("f405d85b-4f92-4e0d-8030-2f4e7a280f41");
                }
                DepartmentBLL departmentBLL = new DepartmentBLL();
                //先判断当前用户的部门是不是在厂级及以上
                var depart =
                    departmentBLL.GetList()
                        .Where(x => x.Nature.Contains("集团") || x.Nature.Contains("省级") || x.Nature.Contains("厂级"));
                bool ishave = depart.Any(entity => entity.DepartmentId == user.DeptId);
                string deptId = user.DeptId;
                if (!ishave)
                {
                    deptId = user.OrganizeId;
                }
                //1、先取出所有的栏目
                AppMenuSettingBLL settingBLL = new AppMenuSettingBLL();
                List<AppMenuSettingEntity> appMenuSettingEntities = settingBLL.GetList(deptId, themeType, platform);
                //2、取当前用户所有的授权的菜单
                MenuConfigBLL menuConfigBLL = new MenuConfigBLL();
                List<MenuConfigEntity> menusAll = menuConfigBLL.GetList("", platform, null).Distinct().ToList();
                List<MenuConfigEntity> menus = new List<MenuConfigEntity>();
                roleId.ForEach(role =>
                {
                    menus.AddRange(menusAll.Where(p => !string.IsNullOrWhiteSpace(p.AuthorizeId) && p.AuthorizeId.Contains(role)));
                });
                menus = menus.Distinct().ToList();
                DeptMenuAuthBLL deptMenuAuthBll = new DeptMenuAuthBLL();
                var deptauthList = deptMenuAuthBll.GetList(deptId).Select(x => x.ModuleId);
                menus = menus.Where(x => deptauthList.Contains(x.ModuleId)).ToList();
                //3、根据栏目与菜单的关系配置取菜单
                AppSettingAssociationBLL settingAssociationBLL = new AppSettingAssociationBLL();
                List<AppSettingAssociationEntity> settingAssociationEntities = settingAssociationBLL.GetList(deptId, menus.Select(p => p.ModuleId).ToList());
                //4、组装数据
                List<MenuSettingData> menuSettingDatas = new List<MenuSettingData>();
                foreach (var item in appMenuSettingEntities)
                {
                    MenuSettingData menuSettingData = new MenuSettingData()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Remark = item.Remark,
                        Sort = item.Sort,
                        Icon = string.IsNullOrWhiteSpace(item.Icon) ? null : new DataItemDetailBLL().GetItemValue("imgUrl") + item.Icon,
                    };
                    var menusIds = settingAssociationEntities.Where(p => p.ColumnId == item.Id).Select(x => x.ModuleId).ToList();
                    if (menusIds != null && menusIds.Count > 0)
                    {
                        var appMenu = menus.Where(x => menusIds.Contains(x.ModuleId)).ToList();
                        menuSettingData.AddChild(appMenu, settingAssociationEntities.Where(p => p.ColumnId == item.Id).ToList());
                    }
                    menuSettingDatas.Add(menuSettingData);
                }
                var data = menuSettingDatas.OrderBy(x => x.Sort).ToList();
                return new { Code = 0, data.Count, Info = "获取数据成功", data };
            }
            catch (Exception ex)
            {
                LogMessage logMessage = new LogMessage();
                logMessage.OperationTime = DateTime.Now;
                logMessage.Url = HttpContext.Current.Request.RawUrl;
                logMessage.Class = "MenuConfig";
                logMessage.Ip = Net.Ip;
                logMessage.Host = Net.Host;
                logMessage.Browser = Net.Browser;
                if (null != OperatorProvider.Provider.Current())
                {
                    logMessage.UserName = OperatorProvider.Provider.Current().Account + "（" + OperatorProvider.Provider.Current().UserName + "）";
                }

                logMessage.ExceptionInfo = Newtonsoft.Json.JsonConvert.SerializeObject(ex);
                //logMessage.ExceptionSource = Error.Source;
                //logMessage.ExceptionRemark = Error.StackTrace;
                string strMessage = new LogFormat().ExceptionFormat(logMessage);

                LogEntity logEntity = new LogEntity();
                logEntity.CategoryId = 4;
                logEntity.OperateTypeId = ((int)OperationType.Exception).ToString();
                logEntity.OperateType = EnumAttribute.GetDescription(OperationType.Exception);
                logEntity.OperateAccount = logMessage.UserName;
                if (null != OperatorProvider.Provider.Current())
                {
                    logEntity.OperateUserId = OperatorProvider.Provider.Current().UserId;
                }
                logEntity.ExecuteResult = -1;
                logEntity.ExecuteResultJson = strMessage;
                logEntity.Module = "菜单配置";
                logEntity.ModuleId = SystemInfo.CurrentModuleId;
                logEntity.WriteLog();
                return new { Code = -1, Info = "获取数据失败", ex.Message };
            }

        }

        [HttpPost]
        public ListResult<MenuSettingData> GetAnonymousMenus(ParameterModel<MenuFilterModel> parameterModel)
        {
            var baseurl = new DataItemDetailBLL().GetItemValue("imgUrl");
            AppMenuSettingBLL settingBLL = new AppMenuSettingBLL();
            AppSettingAssociationBLL settingAssociationBLL = new AppSettingAssociationBLL();
            List<AppMenuSettingEntity> appMenuSettingEntities = settingBLL.GetList(parameterModel.Data.DeptId, int.Parse(parameterModel.Data.ThemeType), int.Parse(parameterModel.Data.Platform));
            List<AppSettingAssociationEntity> settingAssociationEntities = settingAssociationBLL.GetList(parameterModel.UserId);
            MenuConfigBLL menuConfigBLL = new MenuConfigBLL();
            List<MenuConfigEntity> menusAll = menuConfigBLL.GetList("", int.Parse(parameterModel.Data.Platform), null).Distinct().ToList();
            List<MenuSettingData> menuSettingDatas = new List<MenuSettingData>();
            foreach (var item in appMenuSettingEntities)
            {
                MenuSettingData menuSettingData = new MenuSettingData()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Remark = item.Remark,
                    Sort = item.Sort,
                    Icon = string.IsNullOrWhiteSpace(item.Icon) ? null : baseurl + item.Icon,
                };
                var menusIds = settingAssociationEntities.Where(p => p.ColumnId == item.Id).Select(x => x.ModuleId).ToList();
                if (menusIds != null && menusIds.Count > 0)
                {
                    var appMenu = menusAll.Where(x => menusIds.Contains(x.ModuleId)).ToList();
                    menuSettingData.AddChild(appMenu, settingAssociationEntities.Where(p => p.ColumnId == item.Id).ToList());
                }
                menuSettingDatas.Add(menuSettingData);
            }
            var data = menuSettingDatas.OrderBy(x => x.Sort).ToList();
            return new ListResult<MenuSettingData> { Success = true, Data = data };
        }
        /// <summary>
        /// 根据注册码返回接口地址
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetApiUrl([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string registcode = dy.registcode;//注册码
                RegistManageBLL registManageBLL = new RegistManageBLL();
                RegistManageEntity data = registManageBLL.GetEntity(registcode);
                if (data == null)
                {
                    return new { Code = -1, Info = "获取数据失败", Message = "没有找到对应的数据" };
                }
                return new { Code = 0, Info = "获取数据成功", data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Info = "获取数据失败", ex.Message };
            }

        }
        /// <summary>
        /// 安卓终端专用
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetTerminalMenuList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string id = dy.id;//单位注册ID
                long themeTypeParam = dy.themetype;//0 第一套工作栏  1 第二套
                int themeType = int.Parse(themeTypeParam.ToString());
                //取出单位的注册信息
                MenuAuthorizeEntity menuAuthorize = new MenuAuthorizeBLL().GetEntity(id);

                //1、先取出所有的栏目
                AppMenuSettingBLL settingBLL = new AppMenuSettingBLL();
                List<AppMenuSettingEntity> appMenuSettingEntities = settingBLL.GetList(menuAuthorize.DepartId, themeType, 1).OrderBy(p => p.Sort).ToList();
                //2、取当前用户所有的授权的菜单
                MenuConfigBLL menuConfigBLL = new MenuConfigBLL();
                List<MenuConfigEntity> menus = menuConfigBLL.GetList(null, null, null).Distinct().ToList();
                //3、根据栏目与菜单的关系配置取菜单
                AppSettingAssociationBLL settingAssociationBLL = new AppSettingAssociationBLL();
                List<AppSettingAssociationEntity> settingAssociationEntities = settingAssociationBLL.GetList(menuAuthorize.DepartId, menus.Select(p => p.ModuleId).ToList());
                //4、组装数据
                List<MenuSettingData> menuSettingDatas = new List<MenuSettingData>();
                foreach (var item in appMenuSettingEntities)
                {
                    MenuSettingData menuSettingData = new MenuSettingData()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Remark = item.Remark,
                        Sort = item.Sort,
                        Icon = string.IsNullOrWhiteSpace(item.Icon) ? null : new DataItemDetailBLL().GetItemValue("imgUrl") + item.Icon,
                    };
                    var menusSearch = settingAssociationEntities.Where(p => p.ColumnId == item.Id).OrderBy(p => p.Sort).ToList();
                    if (menusSearch != null && menusSearch.Count > 0)
                    {
                        var menusIds = menusSearch.Select(p => p.ModuleId).ToList();
                        var appMenu = menus.Where(x => menusIds.Contains(x.ModuleId)).ToList();
                        menuSettingData.AddChild(appMenu, menusSearch);
                    }
                    menuSettingData.Child = menuSettingData.Child.OrderBy(p => p.Sort).ToList();
                    menuSettingDatas.Add(menuSettingData);
                }
                var data = menuSettingDatas.OrderBy(x => x.Sort).ToList();
                return new { Code = 0, data.Count, Info = "获取数据成功", data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = "获取数据失败", ex.Message };
            }

        }

        /// <summary>
        /// 安卓终端专用
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object ModuleShow([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string id = dy.id;//单位注册ID
                long themeTypeParam = dy.themetype;//0 第一套工作栏  1 第二套
                int themeType = int.Parse(themeTypeParam.ToString());
                //取出单位的注册信息
                MenuAuthorizeEntity menuAuthorize = new MenuAuthorizeBLL().GetEntity(id);

                //1、先取出所有的栏目
                AppMenuSettingBLL settingBLL = new AppMenuSettingBLL();
                List<AppMenuSettingEntity> appMenuSettingEntities = settingBLL.GetList(menuAuthorize.DepartId, themeType, 1);
                //2、取当前用户所有的授权的菜单
                MenuConfigBLL menuConfigBLL = new MenuConfigBLL();
                List<MenuConfigEntity> menus = menuConfigBLL.GetList(null, null, null).Distinct().ToList();
                //3、根据栏目与菜单的关系配置取菜单
                AppSettingAssociationBLL settingAssociationBLL = new AppSettingAssociationBLL();
                List<AppSettingAssociationEntity> settingAssociationEntities = settingAssociationBLL.GetList(menuAuthorize.DepartId, menus.Select(p => p.ModuleId).ToList());
                //4、组装数据
                List<MenuConfigEntity> authMenu = new List<MenuConfigEntity>();
                foreach (var item in appMenuSettingEntities)
                {

                    var menusIds = settingAssociationEntities.Where(p => p.ColumnId == item.Id).Select(x => x.ModuleId).ToList();
                    if (menusIds != null && menusIds.Count > 0)
                    {
                        var appMenu = menus.Where(x => menusIds.Contains(x.ModuleId)).ToList();
                        authMenu.AddRange(appMenu);
                    }
                }
                //根据名字判断班务公开显示的tab页
                var data = new
                {
                    kqb = authMenu.Count(p => p.ModuleName.Contains("考勤管理")) > 0 ? 1 : 0,
                    zbb = authMenu.Count(p => p.ModuleName.Contains("考勤管理")) > 0 ? 1 : 0,
                    jxkhb = authMenu.Count(p => p.ModuleName.Contains("绩效管理")) > 0 ? 1 : 0,
                };

                return new { Code = 0, Info = "获取数据成功", Data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = "获取数据失败", ex.Message };
            }

        }

        /// <summary>
        /// 返回某一个平台下所有的模块
        /// </summary>
        /// <param name="platform">0 windows 1 安卓终端 2手机app</param>
        /// <returns></returns>
        public object GetMenuList(int? platform)
        {
            try
            {
                MenuConfigBLL configBLL = new MenuConfigBLL();
                var menuList = configBLL.GetAllList().Where(p => p.PaltformType == platform).ToList();
                return new { Code = 0, Info = "请求成功", Data = menuList, Count = menuList.Count };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Info = "获取数据失败", ex.Message };
            }
        }
        /// <summary>
        /// 根据菜单的名称获取菜单的信息（此菜单是系统菜单的，不是配置的菜单）
        /// </summary>
        /// <param name="moduleIdName"></param>
        /// <returns></returns>
        public object GetModuleInfoByName(string moduleIdName)
        {

            try
            {
                ModuleBLL bLL = new ModuleBLL();
                var moduleList = bLL.GetList();
                if (moduleIdName == null)
                {
                    return new { Code = -1, Count = 1, Info = "菜单的名称不能为空" };
                }
                else
                {
                    var data = moduleList.FirstOrDefault(p => p.FullName.Equals(moduleIdName));
                    return new { Code = 0, Count = 1, Info = "获取数据成功", data };
                }
            }
            catch (Exception ex)
            {
                return new { Code = -1, Info = "获取数据失败", ex.Message };
            }
        }

        /// <summary>
        /// 获取班组文化墙地址
        /// </summary>
        /// <param name="deptId">部门Id</param>
        /// <returns></returns>
        public object GetCultureUrl()
        {
            string deptId = HttpContext.Current.Request["deptId"];
            MenuAuthorizeBLL authorizeBLL = new MenuAuthorizeBLL();
            string cultureUrl = authorizeBLL.GetCultureUrl(deptId) ?? "";
            return cultureUrl;
        }
    }

    public class MenuConfigRequestModel
    {
        public string registcode { get; set; }
        public string userId { get; set; }
    }
}
