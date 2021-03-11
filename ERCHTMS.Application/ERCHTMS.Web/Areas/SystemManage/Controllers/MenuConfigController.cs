using BSFramework.Util.WebControl;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Web.Areas.SystemManage.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BSFramework.Util;

namespace ERCHTMS.Web.Areas.SystemManage.Controllers
{
    public class MenuConfigController : MvcControllerBase
    {
      
        // GET: SystemManage/MenuConfig
        private MenuConfigBLL menuConfigBll = new MenuConfigBLL();
        #region 视图页面

        #region 菜单配置
        public ActionResult Index()
        {
          //var list =   menuConfigBll.GetList("").Select(x=>x.ModuleId);
          //  int count = 0;
          //  List< AppSettingAssociationEntity > adds = new List<AppSettingAssociationEntity>();
          //  foreach (var moduleId in list)
          //  {
          //      AppSettingAssociationEntity association = new AppSettingAssociationEntity()
          //      {
          //          Id = Guid.NewGuid().ToString(),
          //          ColumnId = "cc18ffe9-6d48-47c6-997f-64a170af2fa8",
          //          ColumnName = "总栏目",
          //          DeptId = "95d5adae-c1da-4231-8adf-6c73840a455a",
          //          ModuleId = moduleId,
          //          Sort = count,
          //      };
          //      count++;
          //      adds.Add(association);
          //  }
          //  new AppSettingAssociationBLL().SaveList(adds);
            return View();
        }

        public ActionResult Form()
        {
            return View();
        }
        /// <summary>
        /// 当前菜单的ID，用来确定菜单是否选择
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult AssociationSetting()
        {
            //List<string> idList = new List<string>();
            //if (!string.IsNullOrWhiteSpace(Id))
            //{
            //    var idsStr=menuConfigBll.GetEntity(Id).AssociationId;
            //    idList = idsStr.Split(new char[','], StringSplitOptions.RemoveEmptyEntries).ToList();
            //}
            //var data = menuConfigBll.GetList("").ToList();     //获取所有的菜单,并剔除当前ID的菜单
            //var delModel = data.FirstOrDefault(x => x.Id == Id);
            //data.Remove(delModel);
            //#region 拼接HTML代码到前台去

            //#endregion

            //ViewBag.AssociationIdList = idList;
            return View();
        }
        #endregion

      
        #endregion
        #region 后台方法

        #region  菜单配置
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagination">分页数据</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public JsonResult GetPageListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = menuConfigBll.GetPageList(pagination, queryJson);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return Json(JsonData, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 保存（新增修改）
        /// </summary>
        /// <param name="keyValue">主键 可为空</param>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public ActionResult SaveForm(string keyValue, RquestModel request)
        {
            //实体类有私有属性，作为参数会请求失败
            MenuConfigEntity model = new MenuConfigEntity()
            {
                Id = request.Id,
                AssociationId = request.AssociationId,
                AssociationName = request.AssociationName,
                AuthorizeId = request.AuthorizeId,
                AuthorizeName = request.AuthorizeName,
                BAK3 = request.BAK3,
                BAK2 = request.BAK2,
                BAK4 = request.BAK4,
                Sort = request.Sort,
                CreateUserName = request.CreateUserName,
                CreateDate = request.CreateDate,
                DeptCode = request.DeptCode,
                DeptId = request.DeptId,
                DeptName = request.DeptName,
                ModifyDate = request.ModifyDate,
                CreateUserId = request.CreateUserId,
                IsView = request.IsView,
                ModuleCode = request.ModuleCode,
                ModifyUserId = request.ModifyUserId,
                ModifyUserName = request.ModifyUserName,
                ModuleId = request.ModuleId,
                ModuleName = request.ModuleName,
                PaltformType = request.PaltformType,
                ParentId = request.ParentId,
                ParentName = request.ParentName,
                Remark = request.Remark,
                MenuIcon = request.MenuIcon,
                JsonData = request.JsonData
            };
            ModuleBLL modulebll = new ModuleBLL();
            if (!string.IsNullOrWhiteSpace(model.ModuleId))
            {
                var module = modulebll.GetEntity(model.ModuleId);
                if (module != null) model.ModuleCode = module.EnCode;
            }
            menuConfigBll.SaveForm(keyValue, model);
            return Success("操作成功。");
        }
        /// <summary>
        /// 根据ID获取单个数据
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult GetFormJson(string keyValue)
        {
            var data = menuConfigBll.GetEntity(keyValue);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public ActionResult Remove(string keyValue)
        {

            //删除之前，检查是否授权了过了，如果当前菜单被授权了，就不允许删除
            bool validData = new DeptMenuAuthBLL().HasAuth(keyValue);
            if (validData)
            {
                return Error("操作失败：" + "该菜单已授权给电厂，不允许删除");
            }
            menuConfigBll.RemoveForm(keyValue);
            return Success("操作成功。");
        }

        /// <summary>
        /// 获取所有的数据（以树的形式）
        /// </summary>
        /// <returns></returns>
        public JsonResult GetTreeJson(bool showCheckBox=false)
        {
            var data = menuConfigBll.GetAllList();
            var  treeModel =MenuTreeHelper.InitData();
            //foreach (var item in data)
            //{
            //    MenuTreeHelper.FomateTree(treeModel, item, showCheckBox);
            //}

            foreach (var firstLevel in treeModel)
            {
                MenuTreeHelper.FomateTree(firstLevel, data, showCheckBox);
            }
            return Json(treeModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveImg()
        {
            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            //没有文件上传，直接返回
            if (files[0].ContentLength == 0 || string.IsNullOrEmpty(files[0].FileName))
            {
                return HttpNotFound();
            }
            string virtualPath = "";
            string UserId = OperatorProvider.Provider.Current().UserId;
                string FileEextension = Path.GetExtension(files[0].FileName);

                virtualPath = string.Format("/Resource/MenuIconFile/{0}{1}", Guid.NewGuid().ToString(), FileEextension);
                string fullFileName = Server.MapPath("~" + virtualPath);
                //创建文件夹，保存文件
                string path = Path.GetDirectoryName(fullFileName);
                Directory.CreateDirectory(path);
                files[0].SaveAs(fullFileName);

                return Success("上传成功。", virtualPath);
        }

        #endregion



        #endregion
    }
}