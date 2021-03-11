using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using Newtonsoft.Json.Linq;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// 描 述：审查配置表
    /// </summary>
    public class InvestigateController : MvcControllerBase
    {
        private InvestigateBLL investigatebll = new InvestigateBLL();
        private InvestigateContentBLL investigatecontentbll = new InvestigateContentBLL();

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
        [HttpGet]
        public ActionResult Create()
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
            var data = investigatebll.GetList(queryJson);
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
            //审查信息
            var data = investigatebll.GetEntity(keyValue);

            ////审查内容
            //var contentdata = investigatecontentbll.GetList(keyValue);

            //var result = new { basedata = data, contentdata = contentdata };

            return ToJsonResult(data);
        }

        public ActionResult GetAllFactory() {
            var data = investigatebll.GetAllFactory();
            return ToJsonResult(data);
        }

        #endregion


        #region 获取配置项列表
        /// <summary>
        /// 获取配置项列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetInvestigateList(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "a.id";
            pagination.p_fields = @" a.orginezeid,a.orginezename,a.settingtype,a.isuse,a.createdate,a.createusername,a.createuserid ";
            pagination.p_tablename = @" epg_investigate a ";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var queryParam = queryJson.ToJObject();
            var watch = CommonHelper.TimerStart();
            var data = investigatebll.GetInvestigatePageList(pagination, queryJson);
            var jsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return ToJsonResult(jsonData);
        }
        #endregion



        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetInvestigateContentListJson(string keyValue)
        {
            var data = investigatecontentbll.GetList(keyValue);
            return ToJsonResult(data);
        }

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
            //删除审查配置
            investigatebll.RemoveForm(keyValue);
            //删除审查内容
            investigatecontentbll.RemoveFormByRecId(keyValue);
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
        public ActionResult SaveForm(string keyValue, InvestigateEntity entity)
        {
            //审查信息
            investigatebll.SaveForm(keyValue, entity);

            //审查内容
            string investigatecontent = Request.Form["INVESTIGATECONTENT"].ToString();

            JArray arrData = JArray.Parse(investigatecontent);

            //先删除
            if (!string.IsNullOrEmpty(keyValue))
            {
                investigatecontentbll.RemoveFormByRecId(keyValue);
            }
            
            foreach (string str in arrData)
            {
                if (!string.IsNullOrEmpty(str)) 
                {
                    InvestigateContentEntity centity = new InvestigateContentEntity();
                    centity.INVESTIGATEID = entity.ID;
                    centity.INVESTIGATECONTENT = str.Split(',')[0];
                    centity.SORTID = int.Parse(str.Split(',')[1].ToString());
                    investigatecontentbll.SaveForm("", centity);
                }
            }
            return Success("操作成功。");
        }
        #endregion
    }
}