using ERCHTMS.Entity.NosaManage;
using ERCHTMS.Busines.NosaManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System;

namespace ERCHTMS.Web.Areas.NosaManage.Controllers
{
    /// <summary>
    /// 描 述：工作成果
    /// </summary>
    public class NosaworkresultController : MvcControllerBase
    {
        private NosaworkresultBLL nosaworkresultbll = new NosaworkresultBLL();

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
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = nosaworkresultbll.GetList(pagination, queryJson);
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
            var data = nosaworkresultbll.GetEntity(keyValue);
            //返回值
            var josnData = new
            {
                data
            };

            return Content(josnData.ToJson());
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
            nosaworkresultbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string SaveForm()
        {
            string templatePath = "";
            string templateName = "";
            int count = HttpContext.Request.Files.Count;
            if (count > 0)
            {
                var file = Request.Files[0];
                templateName = file.FileName;
                if (!string.IsNullOrWhiteSpace(templateName))
                {
                    string sufx = System.IO.Path.GetExtension(file.FileName);
                    templatePath = string.Format("~/Resource/NosaWorkResult/{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), sufx);                    
                    string filename = Server.MapPath(templatePath);
                    var path = System.IO.Path.GetDirectoryName(filename);
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                    file.SaveAs(filename);
                }
            }
            var keyValue = Request["ID"];
            var workId = Request["WorkId"];
            NosaworkresultEntity entity = null;
            if (!string.IsNullOrWhiteSpace(keyValue))
            {
                entity = nosaworkresultbll.GetEntity(keyValue);
                if (!string.IsNullOrWhiteSpace(templateName))
                {
                    string filename = Server.MapPath(entity.TemplatePath);
                    if (System.IO.File.Exists(filename))
                    {
                        System.IO.File.Delete(filename);
                    }
                }
            }
            if (entity == null)
            {
                entity = new NosaworkresultEntity() { ID = keyValue };
            }
            entity.WorkId = workId;
            entity.Name = Request["Name"];
            entity.TemplatePath = !string.IsNullOrWhiteSpace(templatePath) ? templatePath : entity.TemplatePath;
            entity.TemplateName = !string.IsNullOrWhiteSpace(templateName) ? templateName : entity.TemplateName;
            nosaworkresultbll.SaveForm(keyValue, entity);

            return "保存成功。";
        }
        #endregion
    }
}
