using ERCHTMS.Entity.SaftProductTargetManage;
using ERCHTMS.Busines.SaftProductTargetManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Code;
using System.Data;

namespace ERCHTMS.Web.Areas.SaftProductTargetManage.Controllers
{
    /// <summary>
    /// 描 述：安全生产责任书
    /// </summary>
    public class SafeProductDutyBookController : MvcControllerBase
    {
        private SafeProductDutyBookBLL safeproductdutybookbll = new SafeProductDutyBookBLL();

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
            var data = safeproductdutybookbll.GetList(queryJson);
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
            var data = safeproductdutybookbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取文件信息列表
        /// </summary>
        ///<param name="fileId">责任书id</param>
        [HttpGet]
        public ActionResult GetFiles(string fileId)
        {
            FileInfoBLL fi = new FileInfoBLL();
            var data = fi.GetFiles(fileId);
            foreach (DataRow item in data.Rows)
            {
               var path = item.Field<string>("FilePath");
               var url = Url.Content(path);
               item.SetField<string>("FilePath", url);
            }
            return ToJsonResult(data);

        }

        /// <summary>
        /// 获取责任书列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetDataListJson(string productId)
        {
            var data = safeproductdutybookbll.GetListByProductId(productId);
            return ToJsonResult(data);
        }

        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "Id";
            pagination.p_fields = "DutyBookName,PartyA,PartyB,WriteDate,ProductId,CreateDate,FileId";
            pagination.p_tablename = "bis_safeproductdutybook";
            pagination.conditionJson = "1=1";
            var watch = CommonHelper.TimerStart();
            var data = safeproductdutybookbll.GetPageList(pagination, queryJson);
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
            safeproductdutybookbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SafeProductDutyBookEntity entity)
        {
            safeproductdutybookbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
