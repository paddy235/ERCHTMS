using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.HseManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.HseManage;
using ERCHTMS.Entity.HseManage.ViewModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.HseManage.Controllers
{
    public class WarningCardController : MvcControllerBase
    {
        // GET: HseManage/WarningCard
        public ViewResult Index()
        {
            var user = OperatorProvider.Provider.Current();
            ViewBag.userid = user.UserId;
            return View();
        }

        public JsonResult GetData(int rows, int page, string key)
        {
            if (rows == -1) rows = int.MaxValue;
            var bll = new WarningCardBLL();
            var total = 0;
            var data = bll.GetData(key, rows, page, out total);
            return Json(new { rows = data, records = total, page, total = Math.Ceiling((decimal)total / rows) }, JsonRequestBehavior.AllowGet);
        }

        public ViewResult Edit(string id)
        {
            var model = default(WarningCardEntity);
            ViewBag.id = id;
            if (string.IsNullOrEmpty(id))
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                model = new WarningCardEntity() { SubmitTime = DateTime.Now, SubmitUser = user.UserName, CheckContents = new List<CheckContentEntity>() };
            }
            else
            {
                var bll = new WarningCardBLL();
                model = bll.GetDetail(id);
            }

            ViewBag.json = Newtonsoft.Json.JsonConvert.SerializeObject(model.CheckContents);
            DataItemDetailBLL dataitembll = new DataItemDetailBLL();
            var list1 = dataitembll.GetDataItemListByItemCode("预警指标卡类别");
            var data1 = list1.Select(x => new SelectListItem() { Value = x.ItemValue, Text = x.ItemName });
            ViewData["Category"] = data1;
            return View(model);
        }

        [HttpPost]
        public JsonResult Edit(string id, WarningCardEntity model)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            if (string.IsNullOrEmpty(id))
                model.CardId = Guid.NewGuid().ToString();
            else
                model.CardId = id;

            model.CreateUserId = model.ModifyUserId = user.UserId;
            model.CreateTime = model.ModifyTime = DateTime.Now;
            model.DeptId = user.DeptId;
            if (model.CheckContents == null) model.CheckContents = new List<CheckContentEntity>();
            foreach (var item in model.CheckContents)
            {
                if (string.IsNullOrEmpty(item.CheckContentId))
                    item.CheckContentId = Guid.NewGuid().ToString();
                item.CreateUserId = item.ModifyUserId = user.UserId;
                item.CreateTime = item.ModifyTime = DateTime.Now;
                item.CardId = model.CardId;
            }
            var success = true;
            var message = "保存成功！";
            var bll = new WarningCardBLL();
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
        public JsonResult Import(HttpPostedFile file)
        {
            var success = true;
            var message = "导入成功！";
            var workbook = new XSSFWorkbook(this.Request.Files[0].InputStream);
            var data = new List<string>();
            try
            {
                if (workbook.GetSheetAt(1).GetRow(0) == null)
                    throw new Exception("请使用导入文件");
                if (workbook.GetSheetAt(1).GetRow(0).GetCell(0) == null)
                    throw new Exception("请使用导入文件");
                if (workbook.GetSheetAt(1).GetRow(0).GetCell(0).NumericCellValue != 9527)
                    throw new Exception("请使用导入文件");
                var sheet = workbook.GetSheetAt(0);
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    data.Add(sheet.GetRow(i).GetCell(0).StringCellValue);
                }
            }
            catch (Exception e)
            {
                success = false;
                message = e.Message;
            }

            return Json(new { success, message, data });
        }

        public FileResult Download()
        {
            var path = Server.MapPath("~/Resource/ExcelTemplate/指标卡检查内容导入模板.xlsx");
            return File(path, "aplication/octet-stream", "指标卡检查内容导入模板.xlsx");
        }

        [HttpPost]
        public JsonResult Delete(string id)
        {
            var success = true;
            var message = "删除成功！";
            var bll = new WarningCardBLL();
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

        #region 统计分析
        /// <summary>
        /// 统计分析 视图页
        /// </summary>
        /// <returns></returns>
        public ActionResult Statistics()
        {
            var user = OperatorProvider.Provider.Current();
            string deptid = user.DeptId;
            //如果登陆的为hse部，则查整个电厂的数据
            if (user.Account.ToLower().Equals("hse")) deptid = user.OrganizeId;
            int year = DateTime.Now.Year;
            int minYear = year - 6;
            List<SelectListItem> yearList = new List<SelectListItem>();
            do
            {
                yearList.Add(new SelectListItem() { Text = year.ToString(), Value = year.ToString() });
                year--;
            }
            while (year > minYear);
            ViewBag.YearList = yearList;
            ViewBag.DpetId = deptid;
            return View();
        }

        public ActionResult TJT()
        {
            return View();
        }
        /// <summary>
        /// 安全比统计
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult GetCountData(FormCollection form)
        {
            //安全比=安全项/总数（（总项-风险-紧急风险）/总数）
            try
            {
                string year = form["year"];//年份
                string deptId = form["Dept"];//部门ID
                var bll = new WarningCardBLL();
                var user = OperatorProvider.Provider.Current();
                if (string.IsNullOrWhiteSpace(deptId))
                {
                    deptId = user.DeptId;//默认查本部门
                }
                List<HseKeyValue> data = bll.GetAQBData(year, deptId);
                return Json(new { Code = 0, Data = data });
            }
            catch (Exception ex)
            {
                return Json(new { Code = -1, ex.Message });
            }
        }
        /// <summary>
        /// 预警指标卡统计
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult GetWarningCardCount(FormCollection form)
        {
            try
            {
                var user = OperatorProvider.Provider.Current();
                List<DepartmentEntity> allDeptList = new DepartmentBLL().GetList().ToList();
                //1、获取页面上要显示的部门（当前用户所在的部门的下级部门）
                List<DepartmentEntity> showDepts = new List<DepartmentEntity>();
                List<string> searchDeptIds = new List<string>();
                List<HseKeyValueParameter> serchParameter = new List<HseKeyValueParameter>();//搜索用参数
                var userDept = allDeptList.FirstOrDefault(p => p.DepartmentId.Equals(user.DeptId));
                if (userDept == null)
                {
                    return Json(new { Code = -1, Message = "找不到当前用户的部门" });
                }
                if (userDept.Nature == "班组")
                {
                    //如果改用户是班组级用户则查上级部门下的所有班组
                    showDepts = allDeptList.Where(p => p.ParentId == userDept.ParentId).ToList();
                    showDepts.ForEach(p =>
                    {
                        serchParameter.Add(new HseKeyValueParameter()
                        {
                            RootId = p.DepartmentId,
                            RootName = p.FullName,
                            DeptIds = new List<string>() { p.DepartmentId }
                        });
                    });
                }
                else
                {
                    //如果不是班组级的话 ，则查当前用户部门的下级部门即可，预警指标卡数据根据Encode模糊查询出的部门的去做匹配
                    if (userDept.IsOrg == 1)
                    {
                        //厂级部门查全厂的数据
                        showDepts = allDeptList.Where(p => p.ParentId == userDept.OrganizeId).ToList();
                    }
                    else
                    {
                        //非厂级部门查 本子部门的数据
                        showDepts = allDeptList.Where(p => p.ParentId == userDept.DepartmentId).ToList();
                    }

                    showDepts.ForEach(p =>
                    {
                        serchParameter.Add(new HseKeyValueParameter()
                        {
                            RootId = p.DepartmentId,
                            RootName = p.FullName,
                            DeptIds = allDeptList.Where(x => x.EnCode.StartsWith(p.EnCode)).Select(m => m.DepartmentId).ToList()
                        });
                    });
                }
                var bll = new WarningCardBLL();
                List<HseKeyValue> dataCount = bll.GetWarningCardCount(serchParameter.SelectMany(p => p.DeptIds).ToList(), form["start"], form["end"]);
                List<HseKeyValue> data = new List<HseKeyValue>();
                var allUser = new UserBLL().GetList().Where(p=>p.IsPresence=="1").ToList();
                serchParameter.ForEach(p =>
                {
                    HseKeyValue keyValue = new HseKeyValue();
                    keyValue.Key = p.RootName;
                    var matchData = dataCount.Where(x => p.DeptIds.Contains(x.DeptId)).ToList();
                    keyValue.Num1 = matchData.Sum(m => m.Num1);
                    keyValue.Num3 = matchData.Sum(m => m.Num3);
                    keyValue.Num4 = matchData.Sum(m => m.Num4);
                    keyValue.Num5 = (keyValue.Num3 + keyValue.Num4) == 0 ? 0 : Math.Round(keyValue.Num3 / (keyValue.Num3 + keyValue.Num4) * 100, 2);
                    int userCount = allUser.Count(u => u.DepartmentId == p.RootId || p.DeptIds.Contains(u.DepartmentId));//本子部门的用户的数量
                    keyValue.Num2 = userCount == 0 ? 0 : Math.Round( keyValue.Num1 / userCount,2);//如果没有用户 ，默认0
                    data.Add(keyValue);
                });



                return Json(new { Code = 0, Data = data });
            }
            catch (Exception ex)
            {
                return Json(new { Code = -1, ex.Message });
            }
        }
        /// <summary>
        /// 参与度统计
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult GetCYDCount(FormCollection form)
        {
            //参与度=(已提交卡总数/总人数*周数)*(实际提交人数/总人数)*100%
            try
            {
                var user = OperatorProvider.Provider.Current();
                string year = form["year"];//年份
                string deptId = form["Dept"];//部门ID
                if (string.IsNullOrWhiteSpace(deptId))
                {
                    deptId = user.DeptId;//默认查本部门
                }
                var bll = new WarningCardBLL();
                List<HseKeyValue> data = bll.GetCYDData(year, deptId);
                return Json(new { Code = 0, Data = data });
            }
            catch (Exception ex)
            {
                return Json(new { Code = -1, ex.Message });
            }

        }
        #endregion
    }
}