using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.HseManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.HseManage;
using ERCHTMS.Entity.PublicInfoManage;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.HseManage.Controllers
{
    public class CheckController : MvcControllerBase
    {
        // GET: HseManage/Check
        public ViewResult Index()
        {
            return View();
        }

        public ViewResult Index2()
        {
            return View();
        }

        public JsonResult GetData(int rows, int page)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var bll = new CheckRecordBLL();
            var total = 0;
            var data = bll.GetData(user.UserId, rows, page, out total);
            return Json(new { rows = data, records = total, page, total = Math.Ceiling((decimal)total / rows) }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetList(string deptid, string checkuser, string from, string to, string key, int rows, int page)
        {
            DateTime? d_from = null;
            DateTime? d_to = null;
            if (!string.IsNullOrEmpty(from)) d_from = DateTime.Parse(from);
            if (!string.IsNullOrEmpty(to))
            {
                d_to = DateTime.Parse(to).AddDays(1).AddSeconds(-1);
            }

            var depts = new DepartmentBLL().GetSubDepartments(deptid, null);
            var deptis = depts.Select(x => x.DepartmentId).ToArray();

            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var bll = new CheckRecordBLL();
            var total = 0;
            var data = bll.GetList(deptis, checkuser, key, d_from, d_to, rows, page, out total);
            return Json(new { rows = data, records = total, page, total = Math.Ceiling((decimal)total / rows) }, JsonRequestBehavior.AllowGet);
        }

        public ViewResult Edit(string id, string view)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var model = default(CheckRecordEntity);
            ViewBag.id = id;
            ViewBag.deptid = user.DeptId;
            ViewBag.view = view;
            if (string.IsNullOrEmpty(id))
            {
                model = new CheckRecordEntity() { CheckTime = DateTime.Now, CheckUser = user.UserName, CheckItems = new List<CheckItemEntity>() };
            }
            else
            {
                var bll = new CheckRecordBLL();
                model = bll.GetDetail(id);
            }

            ViewBag.json = Newtonsoft.Json.JsonConvert.SerializeObject(model.CheckItems);
            DataItemDetailBLL dataitembll = new DataItemDetailBLL();
            var list1 = dataitembll.GetDataItemListByItemCode("预警指标卡类别");
            var data1 = list1.Select(x => new SelectListItem() { Value = x.ItemValue, Text = x.ItemName });
            ViewData["Category"] = data1;
            return View(model);
        }
        public ViewResult Edit2(string id)
        {
            var model = default(CheckRecordEntity);
            ViewBag.id = id;
            if (string.IsNullOrEmpty(id))
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                model = new CheckRecordEntity() { CheckTime = DateTime.Now, CheckUser = user.UserName, CheckItems = new List<CheckItemEntity>() };
            }
            else
            {
                var bll = new CheckRecordBLL();
                model = bll.GetDetail(id);
            }

            ViewBag.json = Newtonsoft.Json.JsonConvert.SerializeObject(model.CheckItems);
            DataItemDetailBLL dataitembll = new DataItemDetailBLL();
            var list1 = dataitembll.GetDataItemListByItemCode("预警指标卡类别");
            var data1 = list1.Select(x => new SelectListItem() { Value = x.ItemValue, Text = x.ItemName });
            ViewData["Category"] = data1;
            return View(model);
        }

        [HttpPost]
        public JsonResult Edit(string id, CheckRecordEntity model)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            if (string.IsNullOrEmpty(id))
                model.CheckRecordId = Guid.NewGuid().ToString();
            else
                model.CheckRecordId = id;

            model.CreateUserId = model.ModifyUserId = user.UserId;
            model.CreateTime = model.ModifyTime = DateTime.Now;
            model.DeptId = user.DeptId;
            model.DeptName = user.DeptName;
            if (model.CheckItems == null) model.CheckItems = new List<CheckItemEntity>();
            foreach (var item in model.CheckItems)
            {
                if (string.IsNullOrEmpty(item.CheckItemId))
                    item.CheckItemId = Guid.NewGuid().ToString();
                item.CreateUserId = item.ModifyUserId = user.UserId;
                item.CreateTime = item.ModifyTime = DateTime.Now;
                item.CheckRecordId = model.CheckRecordId;
            }
            var success = true;
            var message = "保存成功！";
            var bll = new CheckRecordBLL();
            try
            {
                bll.Save(model);
            }
            catch (Exception e)
            {
                success = false;
                message = e.Message;
            }

            return Json(new { success, message });
        }

        [HttpPost]
        public JsonResult Delete(string id)
        {
            var success = true;
            var message = "删除成功！";
            var bll = new CheckRecordBLL();
            try
            {
                bll.Remove(id);
            }
            catch (Exception e)
            {
                success = false;
                message = e.Message;
            }

            return Json(new { success, message });
        }

        public ViewResult Select()
        {
            return View();
        }

        public JsonResult CardDetail(string id)
        {
            var bll = new WarningCardBLL();
            var data = bll.GetDetail(id);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Import(string id)
        {
            var user = OperatorProvider.Provider.Current();
            FileInfoEntity fileentity = null;

            if (this.Request.Files.Count > 0)
            {
                fileentity = new FileInfoEntity()
                {
                    FileId = Guid.NewGuid().ToString(),
                    FileName = this.Request.Files[0].FileName,
                    FileSize = this.Request.Files[0].ContentLength.ToString(),
                    CreateDate = DateTime.Now,
                    CreateUserId = user.UserId,
                    CreateUserName = user.UserName,
                    ModifyDate = DateTime.Now,
                    ModifyUserId = user.UserId,
                    ModifyUserName = user.UserName,
                    RecId = id
                };

                var folder = "~/Resource/Upfile/Check";
                if (!Directory.Exists(Server.MapPath(folder)))
                    Directory.CreateDirectory(Server.MapPath(folder));

                fileentity.FileExtensions = Path.GetExtension(fileentity.FileName);
                fileentity.FilePath = folder + "/" + fileentity.FileId + fileentity.FileExtensions;
                fileentity.FileType = fileentity.FileExtensions.TrimStart('.');
                var path = Path.Combine(Server.MapPath(folder), fileentity.FileId + fileentity.FileExtensions);
                this.Request.Files[0].SaveAs(path);

                new FileInfoBLL().SaveForm(null, fileentity);

                fileentity.FilePath = Url.Content(fileentity.FilePath);
            }
            var success = true;
            var message = "上传成功！";
            return Json(new AjaxResult { type = success ? ResultType.success : ResultType.error, message = HttpUtility.JavaScriptStringEncode(message), resultdata = fileentity });
        }

        public JsonResult GetFiles(string id)
        {
            var files = new FileInfoBLL().GetFileList(id);
            files.ForEach(x => x.FilePath = Url.Content(x.FilePath));
            return Json(files, JsonRequestBehavior.AllowGet);
        }
    }
}