using BSFramework.Util.WebControl;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Web.Areas.SystemManage.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Web.Areas.SystemManage.Controllers
{
    public class MenuAuthorizeController : MvcControllerBase
    {
        private MenuAuthorizeBLL authorizeBLL = new MenuAuthorizeBLL();
        private MenuConfigBLL MenuConfigBLL = new MenuConfigBLL();
        private DeptMenuAuthBLL deptMenuAuthBLL = new DeptMenuAuthBLL();

        #region 视图
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form()
        {
            return View();
        }
        public ActionResult AuthorizeForm(string DepartId, string DepartName, string DepartCode)
        {
            ViewBag.DepartId = DepartId;
            ViewBag.DepartName = DepartName;
            ViewBag.DepartCode = DepartCode;
            var data = MenuConfigBLL.GetAllList().OrderBy(x=>x.Sort).ToList();//所有的菜单
            List<string> menuAuthList = deptMenuAuthBLL.GetList(DepartId).Select(x=>x.ModuleId).ToList();//授权了的菜单ID
            ViewBag.MenuAuthList = menuAuthList;


            var treeModel = MenuTreeHelper.InitData();
            foreach (var firstLevel in treeModel)
            {
                MenuTreeHelper.FomateTree(firstLevel, data, menuAuthList,false,true);
            }
            ViewBag.MenuTree = treeModel;

            var authTreeModel = MenuTreeHelper.InitData();
     
            foreach (var firstLevel in authTreeModel)
            {
                MenuTreeHelper.FomateTree(firstLevel, data, false, false, true);
            }
            ViewBag.AuthTreeModel = authTreeModel;
            return View(data);
        }

        public ActionResult MenuSetting()
        {
            return View();
        }

        public ActionResult MenuSettingForm()
        {
            return View();
        }

        public ActionResult AssociationFrom(string DepartId,int platform,string ColumnId)
        {
            AppSettingAssociationBLL associationBLL = new AppSettingAssociationBLL();
            List<DeptMenuAuthEntity> deptMenuAuthEntities = deptMenuAuthBLL.GetList(DepartId);//找到所有的授权的菜单
            List<AppSettingAssociationEntity> associationEntities = associationBLL.GetListByColumnId(ColumnId, deptMenuAuthEntities.Select(x=>x.ModuleId).ToList()).ToList().OrderBy(x => x.Sort).ToList();//获取当前栏目下授权了的菜单
            return View(associationEntities.Select(p=>p.ModuleId).ToList());
        }

        public ActionResult RegistManageIndex()
        {
            return View();
        }
        public ActionResult RegistManageForm()
        {
            return View();
        }

        public ActionResult CopyForm()
        {
            return View();
        }
        public ActionResult EditAssociationFrom(string keyValue, string ColumnId, int platform)
        {
            AppSettingAssociationBLL associationBLL = new AppSettingAssociationBLL();
            AppSettingAssociationEntity entity = associationBLL.GetEntity(keyValue, ColumnId);
            MenuConfigEntity menuConfig = new MenuConfigBLL().GetEntity(keyValue);
            ViewBag.ModuleName = menuConfig.ModuleName;
            if (menuConfig.ParentId == "0" || menuConfig.ParentId == "1" || menuConfig.ParentId == "2")
            {
                ViewBag.ParentName = entity.ColumnName;
            }
            else
            {
                ViewBag.ParentName = menuConfig.ParentName;
            }
      
            return View(entity);
        }


        public ActionResult RuleInfoForm()
        {
            return View();
        }

        #endregion


        #region 后台方法

        /// <summary>
        /// 生成单位授权过的菜单的下拉列表数据
        /// </summary>
        /// <param name="deptId">单位ID</param>
        /// <param name="platform">平台类型</param>
        /// <param name="keyword">关键字</param>
        /// <param name="columnId">栏目ID</param>
        /// <returns></returns>
        public ActionResult GetMenuAuthCheckBoxData( string deptId,int? platform,string keyword,string columnId,int themeType)
        {
            AppMenuSettingBLL settingBLL = new AppMenuSettingBLL();
            AppSettingAssociationBLL associationBLL = new AppSettingAssociationBLL();
            //AppMenuSettingEntity settingEntity = settingBLL.GetEntity(columnId);  //找到当前栏目
            List<DeptMenuAuthEntity> deptMenuAuthEntities = deptMenuAuthBLL.GetList(deptId);//找到所有的授权的菜单
            List<MenuConfigEntity> menuConfigEntities = MenuConfigBLL.GetListByModuleIds(deptMenuAuthEntities.Select(p => p.ModuleId).ToList(), platform).ToList();//获取当前单位下的菜单
            List<AppSettingAssociationEntity> associationEntities = associationBLL.GetList(deptId, deptMenuAuthEntities.Select(x => x.ModuleId).ToList()).OrderBy(x => x.Sort).ToList();//再找出所有的关系(排除掉非授权的菜单)
            //剔除已经被其他栏目选择了的菜单,但不包括当前选中的栏目
            var columnIds = settingBLL.GetList(deptId, themeType, platform.Value).Where(p => p.Id != columnId).Select(x => x.Id).ToList();
            List<string> setedIds = associationEntities.Where(x =>columnIds.Contains(x.ColumnId)).Select(p => p.ModuleId).ToList();
            var columnMenu = menuConfigEntities.Where(x => !setedIds.Contains(x.ModuleId)).ToList();//当前栏目可选的菜单，即没有被其他栏目选择过的菜单
            List<AppSettingAssociationEntity> checkMenus = associationBLL.GetListByColumnId(columnId, deptMenuAuthEntities.Select(x => x.ModuleId).ToList()).ToList().OrderBy(x => x.Sort).ToList();//获取当前栏目下授权了的菜单
            //var data = MenuTreeHelper.BuildMenuTree(menuList, true);
            var firstLevelMenus = columnMenu.Where(x => x.ParentId == platform.ToString());

            List<MenuTreeModel> data = new List<MenuTreeModel>();
            foreach (var firstmenu in firstLevelMenus)
            {
                data.Add(new MenuTreeModel(firstmenu, platform.ToString(), true));
            }
            foreach (var firstLevel in data)
            {
                MenuTreeHelper.FomateTree(firstLevel, columnMenu,checkMenus.Select(p=>p.ModuleId).ToList(),true);
            }


            return Json(data, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 查询所有的已授权的菜单
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetMenuAuthPagedList(Pagination pagination, string queryJson)
        {
            var data = authorizeBLL.GetPageList(pagination, queryJson);
          //  var aa = authorizeBLL.GetListByRegistCode("11111");
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 保存表单
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ActionResult SaveForm(string keyValue, MenuAuthorizeEntity entity)
        {
            authorizeBLL.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        /// <summary>
        /// 根据ID获取单个数据
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult GetFormJson(string keyValue)
        {
            var data = authorizeBLL.GetEntity(keyValue);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public ActionResult Remove(string keyValue)
        {
            //先查询出deptId
            var entity = authorizeBLL.GetEntity(keyValue);
            authorizeBLL.RemoveForm(keyValue);
            deptMenuAuthBLL.Remove(entity.DepartId);
            new AppMenuSettingBLL().RemoveByDeptId(entity.DepartId);
           new AppSettingAssociationBLL().Remove(entity.DepartId);
            return Success("操作成功。");
        }

        public ActionResult SaveMenuAuth(MenuAuthRquestModel param)
        {
            try
            {
                //先从数据库提取出所有的该单位下的菜单。已经有的就不管，没有的就添加，多的就删除
                var oldData = deptMenuAuthBLL.GetList(param.departId);//旧的数据
                var delData = oldData.Where(x => !param.ModuleIds.Contains(x.ModuleId)).ToList();//要删除逇数据;

                var oldmoduleIdsList = oldData.Select(x => x.ModuleId).ToList();
                var newdataIds = param.ModuleIds.Where(x => !oldmoduleIdsList.Contains(x)).ToList();//要新增进去的数据Id的集合
                var newData = new List<DeptMenuAuthEntity>();
                newdataIds.ForEach(p =>
                {
                    newData.Add(new DeptMenuAuthEntity()
                    {
                        Id = Guid.NewGuid().ToString(),
                        DeptCode = param.departCode,
                        DeptId = param.departId,
                        DeptName = param.departName,
                        ModuleId = p
                    });
                });
                //执行删除操作之前，判断是否有菜单已经被使用了，被适用的菜单，不允许取消授权。防止误操作
                bool validData = new AppSettingAssociationBLL().CheckData(delData);
                if (!validData)
                {
                    return Error("操作失败：" + "存在正在被使用的菜单模块，请先删除“APP(安卓终端)界面设置”中工作栏里的菜单后再操作");
                }

                deptMenuAuthBLL.Remove(delData);
                deptMenuAuthBLL.Add(newData);

                //取消了菜单授权之后 ，要删除对应的单位的栏目菜单关联关系
                new AppSettingAssociationBLL().Remove(param.departId, delData.Select(p => p.ModuleId).ToList());
                // 如果新增的数据里面有安卓终端的数据 ，则把这些菜单添加到默认栏目里面， 没有栏目的则默认新建一个栏目
                //1、找出新增的菜单里面是安卓菜单的数据
                List<MenuConfigEntity> newMenuEntityList = MenuConfigBLL.GetListByModuleIds(newdataIds, 1);
                if (newMenuEntityList != null && newMenuEntityList.Count > 0)
                {
                    //2、先找栏目，栏目没有就新建一个
                    AppMenuSettingBLL settingBLL = new AppMenuSettingBLL();
                    AppMenuSettingEntity menusetting = settingBLL.GetList(param.departId, 0, 1).FirstOrDefault();
                    string menusettingId = Guid.NewGuid().ToString();
                    string menusettingName = string.Empty;
                    if (menusetting == null)
                    {
                        menusetting = new AppMenuSettingEntity();
                        menusetting.DeptCode = param.departCode;
                        menusetting.DeptId = param.departId;
                        menusetting.DeptName = param.departName;
                        menusetting.PlatformType = 1;
                        menusetting.Name = "总栏目";
                        menusetting.Sort = 1;
                        menusetting.ThemeCode = 0;
                        menusetting.Id = menusettingId;
                        menusettingName = menusetting.Name;
                        settingBLL.SaveForm(null, menusetting);
                    }
                    else
                    {
                        menusettingId = menusetting.Id;
                        menusettingName = menusetting.Name;
                    }
                    //建立栏目与菜单的关联关系
                    List<AppSettingAssociationEntity> associationEntities = new List<AppSettingAssociationEntity>();
                    newMenuEntityList.ForEach(x => {
                        associationEntities.Add(new AppSettingAssociationEntity()
                        {
                            ColumnId = menusettingId,
                            ColumnName = menusettingName,
                            DeptId = param.departId,
                            Id = Guid.NewGuid().ToString(),
                            ModuleId = x.ModuleId,
                            Sort = x.Sort,
                        });
                    });
                    new AppSettingAssociationBLL().SaveList(associationEntities);
                }
                MenuAuthorizeEntity menuAuthorize = authorizeBLL.GetEntityByDeptId(param.departId);
                if (menuAuthorize != null)
                {
                    authorizeBLL.SaveForm(menuAuthorize.Id, menuAuthorize);//此处单纯的改变一下版本号
                }
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Error("操作失败：" + ex.Message);
            }
       
        }
        /// <summary>
        /// 获取菜单配置信息
        /// </summary>
        /// <param name="rquestModel"></param>
        /// <returns></returns>
        public ContentResult GetAPPMenuSetting(MenuSettingRquestModel rquestModel)
        {
            try
            {
                AppMenuSettingBLL settingBLL = new AppMenuSettingBLL();
                AppSettingAssociationBLL associationBLL = new AppSettingAssociationBLL();
                List<AppMenuSettingEntity> settingEntities = settingBLL.GetList(rquestModel.deptId, rquestModel.themeType, rquestModel.platform).OrderBy(p => p.Sort).ToList(); //先找出所有的栏目
                List<DeptMenuAuthEntity> deptMenuAuthEntities = deptMenuAuthBLL.GetList(rquestModel.deptId);//找到所有的授权的菜单
                List<AppSettingAssociationEntity> associationEntities = associationBLL.GetList(rquestModel.deptId, deptMenuAuthEntities.Select(x => x.ModuleId).ToList()).OrderBy(x => x.Sort).ToList();//再找出所有的关系(排除掉非授权的菜单)
                List<MenuConfigEntity> menuConfigEntities = MenuConfigBLL.GetListByModuleIds(associationEntities.Select(x => x.ModuleId).ToList());//获取完整的菜单的
                                                                                                                                                   //构建树
                var treeList = new List<TreeGridEntity>();
                settingEntities.ForEach(setting =>
                {
                    //添加栏目
                    MenuTreeGridModel treeGridModel = new MenuTreeGridModel()
                    {
                        Id = setting.Id,
                        Name = setting.Name,
                        DeptId = setting.DeptId,
                        DataSouceType = 0,
                        Remark = setting.Remark,
                        ParentId = "0",
                        Sort = setting.Sort,
                        ColumnId = setting.Id
                    };
                    TreeGridEntity treeGridEntity = new TreeGridEntity()
                    {
                        parentId = "0",
                        entityJson = JsonConvert.SerializeObject(treeGridModel),
                        expanded = false,
                        hasChildren = true,
                        id = setting.Id,
                        text = setting.Name,
                        code = setting.Id
                    };
                    treeList.Add(treeGridEntity);
                    //添加该栏目下面的菜单
                    var menuassocia = associationEntities.Where(x => x.ColumnId == setting.Id).OrderBy(s => s.Sort).ToList();
                    var moduleIds = menuassocia.Select(p => p.ModuleId);
                    var menuList = menuConfigEntities.Where(x => moduleIds.Contains(x.ModuleId)).ToList();
                    menuassocia.ForEach(a =>
                    {
                        var aa = menuConfigEntities.FirstOrDefault(x => x.ModuleId == a.ModuleId);
                        if (aa != null)
                        {
                            MenuTreeGridModel treeGrid1 = new MenuTreeGridModel()
                            {
                                Id = aa.Id,
                                Name = aa.ModuleName,
                                DeptId = a.DeptId,
                                DataSouceType = 1,
                                Remark = aa.Remark,
                                ParentId = aa.ParentId.Equals(rquestModel.platform.ToString()) ? setting.Id : aa.ParentId,//最顶级菜单
                                ParentName = aa.ParentId.Equals(rquestModel.platform.ToString()) ? setting.Name : aa.ParentName,//最顶级菜单
                                Sort = a.Sort,
                                ColumnId = setting.Id
                            };
                            if (!menuassocia.Any(x => x.ModuleId == treeGrid1.ParentId))
                            {
                                treeGrid1.ParentId = setting.Id;
                                treeGrid1.ParentName = setting.Name;
                            }
                            TreeGridEntity treeGridEntity1 = new TreeGridEntity()
                            {
                                parentId = treeGrid1.ParentId,
                                entityJson = JsonConvert.SerializeObject(treeGrid1),
                                expanded = false,
                                hasChildren = true,
                                id = aa.Id,
                                text = aa.ModuleName,
                                code = aa.Id,
                            };
                            treeList.Add(treeGridEntity1);
                        }
                    });
                });
                return Content(treeList.TreeJson());

            }
            catch (Exception ex)
            {
                throw new Exception("请求失败", ex);
            }
           
            #region 测试
            //var data1 = new
            //{
            //    EnCode = "001",
            //    Name = "测试",
            //    ParentId = "0",

            //};
            //var data2 = new
            //{
            //    EnCode = "006",
            //    Name = "测试1",
            //    ParentId = "001",

            //};
            //var data3 = new
            //{
            //    EnCode = "006002",
            //    Name = "测试1-2",
            //    ParentId = "006",

            //};
            //var data4 = new
            //{
            //    EnCode = "007",
            //    Name = "???",
            //    ParentId = "0",
            //};
            //var data5 = new
            //{
            //    EnCode = "009",
            //    Name = "XX",
            //    ParentId = "0",
            //};
            // treeList = new List<TreeGridEntity>();
            //TreeGridEntity tree = new TreeGridEntity()
            //{
            //    parentId = data1.ParentId,
            //    entityJson = JsonConvert.SerializeObject(data1),
            //    expanded = false,
            //    hasChildren = true,
            //    id = data1.EnCode,
            //    text = data1.Name,
            //    code = data1.EnCode
            //};
            //TreeGridEntity tree1 = new TreeGridEntity()
            //{
            //    parentId = data2.ParentId,
            //    entityJson = JsonConvert.SerializeObject(data2),
            //    expanded = false,
            //    hasChildren = true,
            //    id = data2.EnCode,
            //    text = data2.Name,
            //    code = data2.EnCode
            //};
            //TreeGridEntity tree2 = new TreeGridEntity()
            //{
            //    parentId = data3.ParentId,
            //    entityJson = JsonConvert.SerializeObject(data3),
            //    expanded = false,
            //    hasChildren = true,
            //    id = data3.EnCode,
            //    text = data3.Name,
            //    code = data3.EnCode
            //};
            //TreeGridEntity tree4 = new TreeGridEntity()
            //{
            //    parentId = "0",
            //    entityJson = JsonConvert.SerializeObject(data4),
            //    expanded = false,
            //    hasChildren = true,
            //    id = data4.EnCode,
            //    text = data4.Name,
            //    code = data4.EnCode
            //};
            //TreeGridEntity tree5 = new TreeGridEntity()
            //{
            //    parentId = "0",
            //    entityJson = JsonConvert.SerializeObject(data5),
            //    expanded = false,
            //    hasChildren = false,
            //    id = data5.EnCode,
            //    text = data5.Name,
            //    code = data5.EnCode
            //};
            //treeList.Add(tree);
            //treeList.Add(tree1);
            //treeList.Add(tree2);
            //treeList.Add(tree4);

            //treeList.Add(tree5);
            #endregion
        }
        /// <summary>
        /// 保存菜单设置
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ActionResult SaveMenuSetting(string keyValue,AppMenuSettingEntity entity)
        {
            AppMenuSettingBLL settingBLL = new AppMenuSettingBLL();
            settingBLL.SaveForm(keyValue,entity);
            //判断该菜单是不是安卓终端的，是的话就更新版本
          
                MenuAuthorizeEntity menuAuthorize = authorizeBLL.GetEntityByDeptId(entity.DeptId);
                if (menuAuthorize != null)
                {
                    authorizeBLL.SaveForm(menuAuthorize.Id, menuAuthorize);//此处单纯的改变一下版本号
                }
            return Success("操作成功。");
        }
        /// <summary>
        /// 根据 ID获取单个菜单配置
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult GetMenuSettingFormJson(string keyValue)
        {
            AppMenuSettingBLL settingBLL = new AppMenuSettingBLL();
            AppMenuSettingEntity entity = settingBLL.GetEntity(keyValue);
            return Json(entity, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 保存菜单与栏目的关系
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ActionResult SaveMenuAssociationSetting(string keyValue, AppSettingAssociationEntity entity)
        {
            AppSettingAssociationBLL  associationBLL = new AppSettingAssociationBLL();
            associationBLL.SaveForm(keyValue, entity);
            //判断该菜单是不是安卓终端的，是的话就更新版本
            var module = new MenuConfigBLL().GetEntity(entity.ModuleId);
            if (module.PaltformType==1)
            {
                MenuAuthorizeEntity menuAuthorize = authorizeBLL.GetEntityByDeptId(entity.DeptId);
                if (menuAuthorize != null)
                {
                    authorizeBLL.SaveForm(menuAuthorize.Id, menuAuthorize);//此处单纯的改变一下版本号
                }
            }
            return Success("操作成功。");
        }
        /// <summary>
        /// 获取菜单关联配置信息
        /// </summary>
        /// <param name="keyValue">菜单ID</param>
        /// <param name="columnId">栏目ID</param>
        /// <returns></returns>
        public ActionResult GetAssociationSettingFormJson(string keyValue,string columnId)
        {
            AppSettingAssociationBLL settingBLL = new AppSettingAssociationBLL();
            AppSettingAssociationEntity entity = settingBLL.GetEntity(keyValue, columnId); //!! 这里的keyvalue是菜单Id
            return Json(entity, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除栏目（因为栏目Id是唯一的，所以不用根据platform平台类型去区分删除）
        /// </summary>
        /// <param name="keyValue">栏目ID</param>
        /// <returns></returns>
        public ActionResult RemoveMenuSetting(string keyValue)
        {
            AppMenuSettingBLL settingBLL = new AppMenuSettingBLL();
            AppMenuSettingEntity appMenu = settingBLL.GetEntity(keyValue);
            settingBLL.Remove(keyValue);
            AppSettingAssociationBLL associationBLL = new AppSettingAssociationBLL();
            associationBLL.RemoveByColumnId(keyValue);//删除栏目与菜单的关联关系

            //判断该菜单是不是安卓终端的，是的话就更新版本
            if (appMenu.PlatformType == 1)
            {
                MenuAuthorizeEntity menuAuthorize = authorizeBLL.GetEntityByDeptId(appMenu.DeptId);
                if (menuAuthorize != null)
                {
                    authorizeBLL.SaveForm(menuAuthorize.Id, menuAuthorize);//此处单纯的改变一下版本号
                }
            }

            return Success("操作成功。");
        }
        /// <summary>
        /// 删除栏目与菜单之间的关联关系
        /// </summary>
        /// <param name="keyValue">菜单Id</param>
        /// <param name="ColumnId">栏目ID</param>
        /// <returns></returns>
        public ActionResult RemoveAssociation(string ModuleId, string ColumnId)
        {
            try
            {
                AppSettingAssociationBLL associationBLL = new AppSettingAssociationBLL();
                AppSettingAssociationEntity association = associationBLL.GetEntity(ModuleId, ColumnId);
                //========因页面上的授权都改为了级联添加，删除也要做成级联删除===hm====//
                //1、先找查询出所有的菜单，然后递归找出所有的子级菜单
                List<MenuConfigEntity> menuConfigEntities = new MenuConfigBLL().GetAllList();
                List<MenuConfigEntity> childMenus = new List<MenuConfigEntity>();
                childMenus.Add(menuConfigEntities.FirstOrDefault(p => p.ModuleId == ModuleId));//当前项也要删除
                MenuTreeHelper.FindAllChild(menuConfigEntities, childMenus,ModuleId);//找出所有的下级菜单

                //====================END==============================//
                associationBLL.Remove(childMenus.Select(x=>x.ModuleId).ToList(), ColumnId);
                //判断该菜单是不是安卓终端的，是的话就更新版本
                var module = new MenuConfigBLL().GetEntity(ModuleId);
                if (module.PaltformType == 1)
                {
                    MenuAuthorizeEntity menuAuthorize = authorizeBLL.GetEntityByDeptId(association.DeptId);
                    if (menuAuthorize != null)
                    {
                        authorizeBLL.SaveForm(menuAuthorize.Id, menuAuthorize);//此处单纯的改变一下版本号
                    }
                }
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Error("操作失败："+ex.Message);
            }
     
        }
        public ActionResult GetRegistCodeList(string keyword)
        {
            RegistManageBLL registManageBLL = new RegistManageBLL();
            List<RegistManageEntity> entities = registManageBLL.GetList(keyword);
            return Json(entities, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveRegistCode(string keyValue, RegistManageEntity entity)
        {
            RegistManageBLL registManageBLL = new RegistManageBLL();
            registManageBLL.SaveForm(keyValue, entity);
            return Success("操作成功");
        }

        public ActionResult GetRegistManageForm(string keyValue)
        {
            RegistManageBLL registManageBLL = new RegistManageBLL();
            RegistManageEntity data = registManageBLL.GetForm(keyValue);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoveRegistManage(string keyValue)
        {
            RegistManageBLL registManageBLL = new RegistManageBLL();
            registManageBLL.RemoveRegistManage(keyValue);

            return Success("操作成功");
        }
        public ActionResult CopyInfo(CopyRequestModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.DeptId) && !string.IsNullOrWhiteSpace(model.Id))
            {
                //先查询出授权信息
                MenuAuthorizeEntity entity = authorizeBLL.GetEntity(model.Id);
                if (entity !=null)
                {                          
                    DepartmentBLL departmentBll = new DepartmentBLL();
                    var deptIdArry = model.DeptId.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var deptCodeArry = model.DeptCode.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var deptNameArry = model.DeptName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    //菜单授权信息
                    DeptMenuAuthBLL deptMenuAuthBll = new DeptMenuAuthBLL();
                    List<DeptMenuAuthEntity> deptMenuAuthList = deptMenuAuthBll.GetList(entity.DepartId);
                    //界面设置
                    List<AppMenuSettingEntity> appMenuSettingEntities =
                        new AppMenuSettingBLL().GetListByDeptId(entity.DepartId);
                    //栏目与菜单的关联关系
                    List<AppSettingAssociationEntity> appSettingAssociationList =
                        new AppSettingAssociationBLL().GetList(entity.DepartId);

                    List<DeptMenuAuthEntity> insetDeptMenuAuthList = new List<DeptMenuAuthEntity>();
                    List<MenuAuthorizeEntity> authorizeEntities = new List<MenuAuthorizeEntity>(); //复制之后要保存到数据库中的单位授权数据
                    List<AppMenuSettingEntity> insertMenuSettingEntities = new List<AppMenuSettingEntity>();          //界面设置
                    List<AppSettingAssociationEntity> insertAssociationEntities = new List<AppSettingAssociationEntity>(); //关联关系

                    for (int i = 0; i < deptIdArry.Length ; i++)
                    {
                        //1、先复制单位的授权信息
                        DepartmentEntity deptEntity = departmentBll.GetEntity(deptIdArry[i]);
                        MenuAuthorizeEntity insertEntity = new MenuAuthorizeEntity()
                        {
                            Id = Guid.NewGuid().ToString(),
                            DepartId = deptIdArry[i],
                            DepartCode = deptCodeArry[i],
                            DepartName = deptNameArry[i],
                            DisplayName = deptNameArry[i],
                            BZApiUrl = entity.BZApiUrl,
                            CreateDate = entity.CreateDate,
                            CreateUserId = entity.CreateUserId,
                            CreateUserName = entity.CreateUserName,
                            CulturalUrl = entity.CulturalUrl,
                            ModifyDate = entity.ModifyDate,
                            ModifyUserId = entity.ModifyUserId,
                            ModifyUserName = entity.ModifyUserName,
                            PXApiUrl = entity.PXApiUrl,
                            RegistCode = entity.RegistCode,
                            SKApiUrl = entity.SKApiUrl,
                            TerminalCode = entity.TerminalCode,
                            ThemeType = entity.ThemeType,
                            VersionCode = entity.VersionCode,
                            ParentId = deptEntity != null? deptEntity.ParentId : "",
                            ParentName =
                                deptEntity != null && deptEntity.ParentId == "0"
                                    ? ""
                                    : departmentBll.GetEntity(deptEntity.ParentId).FullName
                        };
                        authorizeEntities.Add(insertEntity);
                        //2、复制界面设置
                        appMenuSettingEntities.ForEach(setting =>
                        {
                            AppMenuSettingEntity appMenu = setting.Clone(deptIdArry[i],deptNameArry[i],deptCodeArry[i],appSettingAssociationList, insertAssociationEntities);
                            insertMenuSettingEntities.Add(appMenu);
                        });
                        //3、  复制关联关系
                        //appSettingAssociationList.ForEach(e =>
                        //{
                        //    AppSettingAssociationEntity association = e.Clone(deptIdArry[i], ,insetDeptMenuAuthList);
                        //    insertAssociationEntities.Add(association);
                        //});
                        //4、复制菜单授权信息
                        deptMenuAuthList.ForEach(p =>
                        {
                            DeptMenuAuthEntity deptMenu = p.Clone(deptIdArry[i],deptCodeArry[i],deptNameArry[i]);
                            insetDeptMenuAuthList.Add(deptMenu);
                        });
                    }
                    authorizeBLL.InsertEntity(authorizeEntities.ToArray());
                    new AppMenuSettingBLL().InsertList(insertMenuSettingEntities);
                    new AppSettingAssociationBLL().InsertList(insertAssociationEntities);
                    new DeptMenuAuthBLL().InsertList(insetDeptMenuAuthList);

                }
            }
            return Success("操作成功");
        }
        /// <summary>
        /// 批量保存栏目与菜单的关系
        /// </summary>
        /// <param name="postData"></param>
        /// <returns></returns>
        public ActionResult BatchSaveAssociationSetting(BatchAssociationRequestModel postData)
        {
            try
            {
                AppSettingAssociationBLL associationBLL = new AppSettingAssociationBLL();
                MenuConfigBLL menuConfigBLL = new MenuConfigBLL();
                List<MenuConfigEntity> menuConfigEntities = MenuConfigBLL.GetList(null, postData.PaltformType).ToList();//所有的菜单，获取排序用
                //先找出该栏目数据库里已存在的关联关系
                List<AppSettingAssociationEntity> associationEntities = associationBLL.GetListByColumnId(postData.ColumnId);
                List<string> associationModuleIds = associationEntities.Select(x => x.ModuleId).ToList();
                if (postData.ModuleIds != null && postData.ModuleIds.Count > 0)
                {
                    //找出要新增的Id
                    List<string> addIds = postData.ModuleIds.Where(x => !associationModuleIds.Contains(x)).ToList();//数据库里面没有的，则是要新增的
                                                                                                                    //找出要删除的Id
                    List<string> delIds = associationModuleIds.Where(p => !postData.ModuleIds.Contains(p)).ToList();//数据库有 ，但是前台传入的ID集合里没有的，则是要删除的
                                                                                                                    //构建新增的实体
                    List<AppSettingAssociationEntity> inserEntities = new List<AppSettingAssociationEntity>();
                    addIds.ForEach(moduleId =>
                    {
                        var module = menuConfigEntities.FirstOrDefault(x => x.ModuleId == moduleId);
                        inserEntities.Add(new AppSettingAssociationEntity()
                        {
                            Id = Guid.NewGuid().ToString(),
                            ColumnId = postData.ColumnId,
                            ColumnName = postData.ColumnName,
                            DeptId = postData.DeptId,
                            ModuleId = moduleId,
                            Sort = module == null ? null : module.Sort
                        });
                    });

                    associationBLL.InsertList(inserEntities);
                    associationBLL.Remove(postData.DeptId, delIds);
                }
                else
                {
                    associationBLL.Remove(postData.DeptId, associationModuleIds);
                }
           
                //判断该菜单是不是安卓终端的，是的话就更新版本
                if (postData.PaltformType == 1)
                {
                    MenuAuthorizeEntity menuAuthorize = authorizeBLL.GetEntityByDeptId(postData.DeptId);
                    if (menuAuthorize != null)
                    {
                        authorizeBLL.SaveForm(menuAuthorize.Id, menuAuthorize);//此处单纯的改变一下版本号
                    }
                }
                return Success("保存成功");
            }
            catch (Exception ex)
            {
                return Error("保存失败：" + ex.Message);
            }
        }

        #region 角色相关数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="infotype"></param>
        /// <param name="authId"></param>
        /// <returns></returns>
        public ActionResult GetTCRuleList(int infotype, string authId)
        {
            TCRuleBLL bll = new TCRuleBLL();
            var data = bll.GetList(infotype,authId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveRuleForm(string keyValue, TCRuleEntity entity)
        {
            try
            {
                TCRuleBLL tCRuleBLL = new TCRuleBLL();
                if (!string.IsNullOrWhiteSpace(keyValue))
                {
                    //修改
                    tCRuleBLL.Update(keyValue, entity);
                }
                else
                {
                    //新增
                    tCRuleBLL.Insert(entity);
                }
                return Success("成功");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public ActionResult GetRole(string keyValue)
        {
            TCRuleBLL bll = new TCRuleBLL();
            var data = bll.GetEntity(keyValue);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RemoveRule(string keyValue)
        {
            try
            {
                TCRuleBLL tCRuleBLL = new TCRuleBLL();
                tCRuleBLL.Delete(keyValue);
                return Success("成功");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        #endregion
        #endregion
    }
}