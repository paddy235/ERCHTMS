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
    /// �� ������������
    /// </summary>
    public class DiscardmonitorController : MvcControllerBase
    {
        private DiscardmonitorBLL discardmonitorbll = new DiscardmonitorBLL();

        #region ��ͼ����
        /// <summary>
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }
        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = discardmonitorbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = discardmonitorbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ͬ��Safety�������ֵ
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
                    return ToJsonResult(new { code = 0, message = "��ȡ���ݳɹ�", point = point });
                }
                else
                {
                    return ToJsonResult(new { code = -1, message = "tokenʧЧ", point = "" });
                }

            }
            catch (Exception e)
            {
                return ToJsonResult(new { code = -1, message = e.Message, point = "" });
            }

        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            discardmonitorbll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, DiscardmonitorEntity entity)
        {
            discardmonitorbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
