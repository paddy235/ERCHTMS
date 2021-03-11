using System.Web;
using ERCHTMS.Entity.LaborProtectionManage;
using ERCHTMS.Busines.LaborProtectionManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.LaborProtectionManage.Controllers
{
    /// <summary>
    /// 描 述：劳动防护发放表详情
    /// </summary>
    public class LaborissuedetailController : MvcControllerBase
    {
        private LaborissuedetailBLL laborissuedetailbll = new LaborissuedetailBLL();
        private LaborinfoBLL laborinfobll = new LaborinfoBLL();
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

        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SeeIndex()
        {
            return View();
        }
        #endregion

        #region 获取数据

        public ActionResult GetIssueList(string keyValue,string InfoId)
        {
            //如果是批量发放时
            if (keyValue==null||keyValue == "")
            {
                string newinfoid = "";
                string[] ids=InfoId.Split(',');
                foreach (string id in ids)
                {
                    if (newinfoid == "")
                    {
                        newinfoid = "'" + id + "'";
                    }
                    else
                    {
                        newinfoid += ",'" + id + "'";
                    }
                }
                var data = laborinfobll.Getplff(newinfoid);
                return ToJsonResult(data);
            }
            else
            {
                var data = laborissuedetailbll.GetList(keyValue);
                return ToJsonResult(data);
            }
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = laborissuedetailbll.GetList(queryJson);
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
            var data = laborissuedetailbll.GetEntity(keyValue);
            return ToJsonResult(data);
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
            laborissuedetailbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }

        /// <summary>
        /// 批量保存表单（新增、修改）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveListForm(string json)
        {
            json = HttpUtility.UrlDecode(json);
            //如果没有数据则直接返回
            if (json == "")
            {
                return Error("无对应内容，操作无效");
            }
            laborissuedetailbll.SaveListForm( json);
            return Success("操作成功。");
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
        public ActionResult SaveForm(string keyValue, LaborissuedetailEntity entity, string json, string InfoId)
        {
            json = HttpUtility.UrlDecode(json);
            if (json == "")
            {
                return Error("无对应内容，操作无效");
            }
            laborissuedetailbll.SaveForm(keyValue, entity, json, InfoId);
            return Success("操作成功。");
        }
        #endregion
    }
}
