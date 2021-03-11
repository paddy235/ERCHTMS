using System;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.Busines.KbsDeviceManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace ERCHTMS.Web.Areas.KbsDeviceManage.Controllers
{
    /// <summary>
    /// 描 述：设备离线记录
    /// </summary>
    public class OfflinedeviceController : MvcControllerBase
    {
        private OfflinedeviceBLL offlinedevicebll = new OfflinedeviceBLL();

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
        public ActionResult Form()
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
        public ActionResult GetListJson(string queryJson)
        {
            var data = offlinedevicebll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = offlinedevicebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取离线设备统计图
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetOffinedeviceImage(int type)
        {
            List<object> dic = new List<object>();
            string[] Month = { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };//先设定月份
            DataTable dt = offlinedevicebll.GetTable(type);
            List<int> Num = new List<int>();
            for (int i = 0; i < Month.Length; i++)
            {
                int count = 0;
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    if (dt.Rows[j]["offtime"].ToString() == DateTime.Now.Year + "-" + Month[i])
                    {
                        count = Convert.ToInt32(dt.Rows[j]["offcount"]);
                        break;
                    }
                }
                Num.Add(count);
            }
            return JsonConvert.SerializeObject(new { x = Num });
        }

        /// <summary>
        /// 查询离线设备前几条
        /// </summary>
        /// <param name="type">设备类型 0标签 1基站 2门禁 3摄像头</param>
        /// <param name="Time">1本年 2本周</param>
        /// <param name="topNum">前几条</param>
        /// <returns></returns>
        [HttpGet]
        public string GetOffTop(int type, int Time, int topNum)
        {
            DataTable dt = offlinedevicebll.GetOffTop(type,Time,topNum);
            return JsonConvert.SerializeObject(dt);
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
            offlinedevicebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, OfflinedeviceEntity entity)
        {
            offlinedevicebll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
