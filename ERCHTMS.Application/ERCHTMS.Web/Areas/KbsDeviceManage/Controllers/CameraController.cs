using BSFramework.Util.WebControl;
using ERCHTMS.Busines.KbsDeviceManage;
using ERCHTMS.Entity.KbsDeviceManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.KbsDeviceManage.Controllers
{
    /// <summary>
    /// 摄像头管理
    /// </summary>
    public class CameraController : MvcControllerBase
    {
        private KbscameramanageBLL camerabll;
        public CameraController()
        {
            camerabll = new KbscameramanageBLL();
        }
        // GET: KbsDeviceManage/Camera
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form()
        {
            return View();
        }


        #region 方法
        /// <summary>
        /// 获取所有的摄像头
        /// </summary>
        /// <returns></returns>
        public ActionResult GetALL()
        {
            var list = camerabll.GetList(null).OrderByDescending(x=>x.CreateDate);
          
            return ToJsonResult(list);
        }

        /// <summary>
        /// 保存表单
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(KbscameramanageEntity entity, string keyValue)
        {
            try
            {
                if (!camerabll.UniqueCheck(entity.CameraId)) throw new Exception("摄像头唯一编码已存在");
                camerabll.SaveEntity(keyValue, entity);
                return Success("操作成功");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult GetFormJson(string keyValue)
        {
            return ToJsonResult(camerabll.GetEntity(keyValue));
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult Remove(string keyValue)
        {
            try
            {
                camerabll.RemoveEntity(keyValue);
                return Success("操作成功");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion
    }
}