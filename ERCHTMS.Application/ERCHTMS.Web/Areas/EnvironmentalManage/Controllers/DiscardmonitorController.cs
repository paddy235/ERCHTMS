using System;
using System.Dynamic;
using System.Net;
using System.Text;
using ERCHTMS.Entity.EnvironmentalManage;
using ERCHTMS.Busines.EnvironmentalManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Xml;
using NetTaste;
using Newtonsoft.Json;
using ERCHTMS.Cache;

namespace ERCHTMS.Web.Areas.EnvironmentalManage.Controllers
{
    /// <summary>
    /// 描 述：环保管理
    /// </summary>
    public class DiscardmonitorController : MvcControllerBase
    {
        private DiscardmonitorBLL discardmonitorbll = new DiscardmonitorBLL();

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
            var data = discardmonitorbll.GetList(queryJson);
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
            var data = discardmonitorbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 同步Safety废气检测值
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSafetyValue(string keyValue)
        {

            try
            {
                string point = "";
                string token = new TokenCache().GetToken();
                if (!string.IsNullOrEmpty(token))
                {
                    string environmentalUrl = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("EnvironmentalUrl");
                    string url = environmentalUrl + token;
                    WebClient wc = new WebClient();
                    wc.Credentials = CredentialCache.DefaultCredentials;
                    //wc.Headers.Add("Content-Type", "text/json; charset=utf-8");
                    wc.Encoding = Encoding.GetEncoding("GB2312");
                    System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                    nc.Add("pointID", keyValue);
                    byte[] bytes =  wc.UploadValues(new Uri(url), "POST", nc);
                    var data = Encoding.UTF8.GetString(bytes);
                    XmlDocument xml = Newtonsoft.Json.JsonConvert.DeserializeXmlNode(data);
                    if (xml.ChildNodes.Count > 0)
                    {
                        foreach (dynamic xmlChildNode in xml.ChildNodes)
                        {
                            if (xmlChildNode.Name == "soap:Envelope")
                            {
                                point = xmlChildNode.InnerText;
                            }
                        }
                    }
                    return ToJsonResult(new { code = 0, message = "获取数据成功", point = point });
                }
                else
                {
                    return ToJsonResult(new { code = -1, message = "token失效", point = "" });
                }

            }
            catch (Exception e)
            {
                return ToJsonResult(new { code = -1, message = e.Message, point = "" });
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
            discardmonitorbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, DiscardmonitorEntity entity)
        {
            discardmonitorbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
