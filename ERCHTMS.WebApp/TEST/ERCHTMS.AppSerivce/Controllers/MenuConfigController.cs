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

namespace ERCHTMS.AppSerivce.Controllers
{
    public class MenuConfigController : BaseApiController
    {

        [HttpPost]
        public object GetMenuConfigList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string deptId = dy.data.DeptId;//电厂ID
                long paltformType = dy.data.PaltformType;//平台类型
                                        
                var data = new MenuConfigBLL().GetList(deptId, Convert.ToInt32(paltformType));
                List<object> objlist = new List<object>();
                foreach (var item in data)
                {
                    var obj = new
                    {
                        id = item.ModuleId,
                        name = item.ModuleName,
                        key = item.ModuleCode,
                        index = item.Sort
                    };
                    objlist.Add(obj);
                }
                return new { Code = 0, objlist.Count, Info = "获取数据成功", objlist };
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
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string registcode = dy.registcode;
                //获取用户基本信息
                MenuAuthorizeBLL authorizeBLL = new MenuAuthorizeBLL();
                List<MenuAuthorizeEntity> data = authorizeBLL.GetListByRegistCode(registcode);
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
                if (user.RoleId==null)
                {
                    return new { Code = -1, Info = "获取数据失败", Message= "用户的角色为空" };
                }
                List<string> roleId = user.RoleId.Replace(" ","").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

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
                List<MenuConfigEntity> menus = menuConfigBLL.GetList("", platform, roleId).Distinct().ToList();
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
                return new { Code = -1, Info = "获取数据失败", ex.Message };
            }
    
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
                if (data==null)
                {
                    return new { Code = -1, Info = "获取数据失败", Message="没有找到对应的数据" };
                }
                return new { Code = 0, Info = "获取数据成功", data };
            }
            catch (Exception ex)
            {
                return new { Code = -1,  Info = "获取数据失败", ex.Message };
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
                List<AppMenuSettingEntity> appMenuSettingEntities = settingBLL.GetList(menuAuthorize.DepartId, themeType, 1);
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
                    };
                    var menusIds = settingAssociationEntities.Where(p => p.ColumnId == item.Id).Select(x => x.ModuleId).ToList();
                    if (menusIds != null && menusIds.Count > 0)
                    {
                        var appMenu = menus.Where(x => menusIds.Contains(x.ModuleId)).ToList();
                        menuSettingData.AddChild(appMenu);
                    }
                    menuSettingDatas.Add(menuSettingData);
                }
                var data = menuSettingDatas.OrderBy(x => x.Sort).ToList();
                return new { Code = 0, data.Count, Info = "获取数据成功", data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count=0, Info = "获取数据失败",ex.Message };
            }

        }
    }
}
