using ERCHTMS.Entity.NosaManage;
using ERCHTMS.Busines.NosaManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System;

namespace ERCHTMS.Web.Areas.NosaManage.Controllers
{
    /// <summary>
    /// �� ���������ɹ�
    /// </summary>
    public class NosaworkresultController : MvcControllerBase
    {
        private NosaworkresultBLL nosaworkresultbll = new NosaworkresultBLL();

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
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
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
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = nosaworkresultbll.GetEntity(keyValue);
            //����ֵ
            var josnData = new
            {
                data
            };

            return Content(josnData.ToJson());
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
            nosaworkresultbll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ��������������޸ģ�
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

            return "����ɹ���";
        }
        #endregion
    }
}
