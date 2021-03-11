using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.Busines.KbsDeviceManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Entity.SystemManage;
using Newtonsoft.Json;
using ERCHTMS.Web.Areas.KbsDeviceManage.Models;

namespace ERCHTMS.Web.Areas.KbsDeviceManage.Controllers
{
    /// <summary>
    /// 描 述：标签管理
    /// </summary>
    public class LablemanageController : MvcControllerBase
    {
        private LablemanageBLL lablemanagebll = new LablemanageBLL();
        private DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();

        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form(string id)
        {
            ViewBag.id = id;
            return View();
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            ViewBag.id = id;

            var user = OperatorProvider.Provider.Current();
            ViewBag.orgid = user.OrganizeId;

            var items = dataItemDetailBLL.GetListItems("标签类型");
            items = items.Where(x => x.Description.Contains("0"));
            ViewData["list"] = items.Select(x => new SelectListItem { Value = x.ItemValue, Text = x.ItemName });

            var model = new Models.LableModel { BindTime = DateTime.Now, Operator = user.UserName, Power = "100" };

            return View(model);
        }

        /// <summary>
        /// 表单提交
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Edit(string id, Models.LableModel model)
        {
            var user = OperatorProvider.Provider.Current();
            model.LabelId = model.LabelId.PadLeft(6, '0');
            if (lablemanagebll.GetIsBind(model.LabelId))
            {
                return Json(new AjaxResult { type = ResultType.error, message = "标签已经绑定！" });
            }
            if (!string.IsNullOrEmpty(model.UserId) && lablemanagebll.GetUserLable(model.UserId) != null)
            {
                return Json(new AjaxResult { type = ResultType.error, message = model.Name + "已经绑定！" });
            }
            var entity = new LablemanageEntity()
            {
                ID = Guid.NewGuid().ToString(),
                DeptId = model.DeptId,
                DeptCode = model.DeptCode,
                DeptName = model.DeptName,
                BindTime = model.BindTime,
                CreateDate = DateTime.Now,
                CreateUserDeptCode = user.DeptCode,
                CreateUserId = user.UserId,
                ModifyDate = DateTime.Now,
                ModifyUserId = user.DeptId,
                CreateUserOrgCode = user.OrganizeCode,
                IdCardOrDriver = model.IdCardOrDriver,
                IsBind = 1,
                LableId = model.LabelId,
                LableTypeName = model.LableTypeName,
                LableTypeId = model.LableTypeId,
                Name = model.Name,
                OperUserId = user.UserName,
                Phone = model.Phone,
                Power = "100%",
                Type = 0,
                State = "离线",
                UserId = model.UserId
            };
            lablemanagebll.SaveForm(id, entity);
            if (string.IsNullOrEmpty(id))
            {
                //将标签信息同步到后台计算服务中
                RabbitMQHelper rh = RabbitMQHelper.CreateInstance();
                SendData sd = new SendData();
                sd.DataName = "LableEntity";
                sd.EntityString = JsonConvert.SerializeObject(entity);
                rh.SendMessage(JsonConvert.SerializeObject(sd));
            }
            return Json(new AjaxResult { type = ResultType.success, message = "保存成功！" });
        }


        /// <summary>
        /// 统计页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Static()
        {
            return View();
        }

        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            // string orgcode = OperatorProvider.Provider.Current().OrganizeCode;
            var watch = CommonHelper.TimerStart();
            //Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var data = lablemanagebll.GetPageList(pagination, queryJson);
            int total = pagination.records / pagination.rows;
            if (pagination.records % pagination.rows != 0)
            {
                total += 1;
            }
            var jsonData = new
            {
                rows = data,
                total = total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };

            return ToJsonResult(jsonData);
        }

        /// <summary>
        /// 获取标签总数
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetCount()
        {
            return lablemanagebll.GetCount().ToString();
        }

        ///// <summary>
        ///// 获取列表
        ///// </summary>
        ///// <param name="queryJson">查询参数</param>
        ///// <returns>返回列表Json</returns>
        //[HttpGet]
        //public ActionResult GetListJson(string queryJson)
        //{
        //    var data = lablemanagebll.GetList(queryJson);
        //    return ToJsonResult(data);
        //}

        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = lablemanagebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 标签统计图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetLableChart()
        {
            return lablemanagebll.GetLableChart();
        }

        /// <summary>
        /// 标签统计表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetLableStatistics()
        {
            var dt = lablemanagebll.GetLableStatistics();
            List<LablemanageEntity> lblist = lablemanagebll.GetList("").Where(it => it.IsBind == 1).ToList(); ;
            DataItemBLL di = new DataItemBLL();
            //先获取到字典项
            DataItemEntity DataItems = di.GetEntityByCode("LabelType");

            DataItemDetailBLL did = new DataItemDetailBLL();
            //根据字典项获取值
            List<DataItemDetailEntity> didList = did.GetList(DataItems.ItemId).ToList();
            List<KbsEntity> klist = new List<KbsEntity>();
            int Znum = 0;
            foreach (var item in didList)
            {
                KbsEntity kbs = new KbsEntity();
                kbs.Name = item.ItemName;
                int num = 0;
                int zxnum = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["labletypeid"].ToString() == item.ItemValue)
                    {
                        num = Convert.ToInt32(dt.Rows[i]["cou"]);
                        zxnum = lblist.Where(it => it.LableTypeId == item.ItemValue && it.State == "在线").Count();
                        break;
                    }
                }
                kbs.Num = num;
                Znum += num;
                kbs.Num2 = zxnum;
                klist.Add(kbs);
            }
            for (int j = 0; j < klist.Count; j++)
            {
                double Proportion = 0;
                if (Znum != 0)
                {
                    Proportion = (double)klist[j].Num / Znum;
                    Proportion = Proportion * 100;
                }
                klist[j].Proportion = Proportion.ToString("0") + "%";
            }
            return Content(klist.ToJson());
        }

        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            lablemanagebll.RemoveForm(keyValue);
            return Success("删除成功。");
        }

        /// <summary>
        /// 解绑标签
        /// </summary>
        /// <param name="keyValue"></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult Untie(string keyValue)
        {
            lablemanagebll.Untie(keyValue);
            //将新绑定的标签信息同步到后台计算服务中
            RabbitMQHelper rh = RabbitMQHelper.CreateInstance();
            SendData sd = new SendData();
            sd.DataName = "UntieLable";
            sd.EntityString = keyValue;
            rh.SendMessage(JsonConvert.SerializeObject(sd));
            return Success("解绑成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, LablemanageEntity entity)
        {
            lablemanagebll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
