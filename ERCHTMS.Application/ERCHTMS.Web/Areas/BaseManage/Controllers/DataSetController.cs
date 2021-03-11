using BSFramework.Util;
using BSFramework.Util.Offices;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.PersonManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.PersonManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ERCHTMS.Web.Areas.BaseManage.Controllers
{
    /// <summary>
    /// 描 述：积分设置
    /// </summary>
    public class DataSetController : MvcControllerBase
    {
        private DataSetBLL scoresetbll = new DataSetBLL();
        #region 视图功能
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
       
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "t.Id";
            pagination.p_fields = "itemcode,itemname,itemtype,itemrole,isdefault,isopen,deptcode,deptname,CreateDate,itemkind";
            pagination.p_tablename = "BASE_Dataset t";
            pagination.conditionJson = " 1=1 ";
            Operator user = OperatorProvider.Provider.Current();
            var data = scoresetbll.GetPageList(pagination, queryJson);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
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
           var entity = scoresetbll.GetEntity(keyValue);
            return ToJsonResult(entity);
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
            scoresetbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        ///  <param name="score">初始积分值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, DataSetEntity ds)
        {
            if (!string.IsNullOrWhiteSpace(ds.DeptCode))
            {
                ds.DeptCode = string.Format(",{0},",ds.DeptCode.Trim(','));
            }
            if (ds.ItemStyle.Trim().Length>0)
            {
                ds.ItemStyle = HttpUtility.UrlDecode(ds.ItemStyle);
            }
            scoresetbll.SaveForm(keyValue, ds);
            return Success("操作成功。");
        }
        #endregion
    }
}
