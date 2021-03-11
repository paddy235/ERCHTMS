using ERCHTMS.Entity.CarManage;
using ERCHTMS.Busines.CarManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System.Collections.Generic;
using ERCHTMS.Busines.SystemManage;
using Newtonsoft.Json;
using System.Net.Http;
using System;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Asn1.Ocsp;

namespace ERCHTMS.Web.Areas.CarManage.Controllers
{
    /// <summary>
    /// 描 述：违章信息类
    /// </summary>
    public class CarviolationController : MvcControllerBase
    {
        private CarviolationBLL carviolationbll = new CarviolationBLL();

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
        /// 实时预警
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ViolationRecord()
        {
            return View();
        }

        /// <summary>
        /// 违规处理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HandleForm()
        {
            return View();
        }

        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            string orgcode = OperatorProvider.Provider.Current().OrganizeCode;
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "ID";
            pagination.p_fields = "createdate,violationmsg,processmeasure,isprocess";
            pagination.p_tablename = "BIS_CARVIOLATION";
            pagination.conditionJson = " 1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            pagination.conditionJson = "1=1";

            var data = carviolationbll.GetPageList(pagination, queryJson);

            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch),

            };
            return Content(JsonData.ToJson());
        }

        /// <summary>
        /// 实时预警列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetViolationdListJson(Pagination pagination, string queryJson)
        {
            string orgcode = OperatorProvider.Provider.Current().OrganizeCode;
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "ID";
            pagination.p_fields = "info.createdate,info.violationmsg,info.processmeasure,info.isprocess,info.modifydate,info.violationtype,cardno,hikpicsvr,vehiclepicurl";
            pagination.p_tablename = "bis_carviolation info ";
            pagination.conditionJson = " 1=1";
            //Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            var data = carviolationbll.GetPageList(pagination, queryJson);

            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch),

            };
            return Content(JsonData.ToJson());
        }


        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = carviolationbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取预警中心数据（未处理的数据）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerLogin(LoginMode.Ignore)]
        public ActionResult GetIndexWaring()
        {
            List<CarviolationEntity> data = carviolationbll.GetIndexWaring();
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取预警中心统计数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerLogin(LoginMode.Ignore)]
        public ActionResult GetIndexWaringCount()
        {
            object data = carviolationbll.GetIndexWaringCount();
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取海康车辆抓拍的图片
        /// </summary>
        /// <param name="hikpicsvr">海康图片服务器唯一编码</param>
        /// <param name="vehiclepicurl">车辆抓拍图片地址</param>
        /// <returns></returns>
        [HttpPost]
        [HandlerLogin(LoginMode.Ignore)]
        public ActionResult GetHikPicUrl(string hikpicsvr, string vehiclepicurl)
        {
            try
            {
                DataItemDetailBLL pdata = new DataItemDetailBLL();
                var pitem = pdata.GetItemValue("Hikappkey");//海康服务器密钥
                var hikBaseUrl = pdata.GetItemValue("HikBaseUrl");//海康服务器地址
                string url = "/artemis/api/video/v1/events/picture";
                string key = string.Empty;// "21049470";
                string sign = string.Empty;// "4gZkNoh3W92X6C66Rb6X";
                if (!string.IsNullOrEmpty(pitem))
                {
                    key = pitem.Split('|')[0];
                    sign = pitem.Split('|')[1];
                }
                var model = new
                {
                    svrIndexCode = hikpicsvr,
                    picUri = vehiclepicurl
                };
                var msg = SocketHelper.LoadCameraList(model, hikBaseUrl, url, key, sign);
                SocketHelper.SetLog("OverSpeedPicOK", "返回消息 获取车辆违章抓拍图片   参数 hikpicsvr:" + hikpicsvr + "   vehiclepicurl：" + vehiclepicurl, msg);
                return Json(msg);
            }
            catch (System.Exception ex)
            {
                SocketHelper.SetLog("OverSpeedPicEoor", "获取车辆违章抓拍图片报错   参数 hikpicsvr:" + hikpicsvr + "   vehiclepicurl：" + vehiclepicurl, JsonConvert.SerializeObject(ex));
                return Json(new { code = "-1", msg = ex.Message });
            }
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
            carviolationbll.RemoveForm(keyValue);
            return Success("删除成功。");
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
        public ActionResult SaveForm(string keyValue, CarviolationEntity entity)
        {
            carviolationbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }

        /// <summary>
        /// 违规处理
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveHandleForm(string keyValue, string Content)
        {
            var data = carviolationbll.GetEntity(keyValue);
            if (data != null)
            {
                data.ProcessMeasure = Content;
                data.IsProcess = 1;
                carviolationbll.SaveForm(keyValue, data);
            }
            return Success("操作成功。");
        }

        #endregion
    }
}
