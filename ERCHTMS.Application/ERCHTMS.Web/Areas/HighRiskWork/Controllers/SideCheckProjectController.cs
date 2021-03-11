using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Busines.HighRiskWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Web;
using System;
using BSFramework.Util.Offices;
using System.Data;
using System.Collections;
using System.Collections.Generic;

namespace ERCHTMS.Web.Areas.HighRiskWork.Controllers
{
    /// <summary>
    /// �� �����ල��������Ŀ
    /// </summary>
    public class SideCheckProjectController : MvcControllerBase
    {
        private SideCheckProjectBLL sidecheckprojectbll = new SideCheckProjectBLL();

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

        /// <summary>
        /// ģ�嵼��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Import()
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
            var data = sidecheckprojectbll.GetList(queryJson);
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
            var data = sidecheckprojectbll.GetEntity(keyValue);
            return ToJsonResult(data);
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
            sidecheckprojectbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SideCheckProjectEntity entity)
        {
            sidecheckprojectbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion

        #region ����ල��������Ŀ
        /// <summary>
        /// ����ල��������Ŀ
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportCase()
        {
            int error = 0;
            string message = "��ѡ���ʽ��ȷ���ļ��ٵ���!";
            string falseMessage = "";
            int count = HttpContext.Request.Files.Count;
            if (count > 0)
            {
                HttpPostedFileBase file = HttpContext.Request.Files[0];
                if (string.IsNullOrEmpty(file.FileName))
                {
                    return message;
                }
                if (!(file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xls") || file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xlsx")))
                {
                    return message;
                }
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName));
                DataTable dt = ExcelHelper.ExcelImport(Server.MapPath("~/Resource/temp/" + fileName));
                Hashtable hast = new Hashtable();
                List<SideCheckProjectEntity> listEntity = new List<SideCheckProjectEntity>();
                SideCheckProjectEntity chapter = new SideCheckProjectEntity();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var chartnumber = dt.Rows[i][0].ToString();
                    if (!string.IsNullOrEmpty(chartnumber))
                    {
                        if (!chartnumber.Contains("."))
                        {
                            chapter = new SideCheckProjectEntity();
                            chapter.CheckContent = dt.Rows[i][1].ToString();
                            chapter.CheckNumber = chartnumber.ToString();
                            chapter.ParentId = "-1";
                            chapter.Id = Guid.NewGuid().ToString();
                            hast.Add(chapter.CheckNumber, chapter.Id);
                           
                        }
                        else
                        {
                            chapter = new SideCheckProjectEntity();
                            if (hast.Keys.Count > 0)
                            {
                                string m = dt.Rows[i][0].ToString();
                                string[] b = new string[20];
                                string key = "";
                                if (m.Contains("."))
                                {
                                    b = m.Split('.');
                                    if (b.Length > 1)
                                    {
                                        key = b[0];
                                    }
                                }
                                object obj = hast[key];
                                if (obj == null)
                                {
                                    chapter.ParentId = "-1";
                                }
                                else
                                {
                                    chapter.ParentId = obj.ToString();
                                }
                                chapter.CheckContent = dt.Rows[i][1].ToString();
                                chapter.CheckNumber = chartnumber.ToString();
                                chapter.Id = Guid.NewGuid().ToString();
                            }
                        }
                        listEntity.Add(chapter);
                    }
                }
                for (int j = 0; j < listEntity.Count; j++)
                {
                    try
                    {
                        sidecheckprojectbll.SaveForm("", listEntity[j]);
                    }
                    catch (Exception ex)
                    {
                        error++;
                    }

                }
                count = listEntity.Count;
                message = "����" + count + "����¼,�ɹ�����" + (count - error) + "����ʧ��" + error + "��";
                message += "</br>" + falseMessage;
            }
            return message;
        }
        #endregion
    }
}
