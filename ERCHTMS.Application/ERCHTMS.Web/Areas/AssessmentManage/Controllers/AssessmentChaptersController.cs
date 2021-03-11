using ERCHTMS.Entity.AssessmentManage;
using ERCHTMS.Busines.AssessmentManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Web;
using System;
using BSFramework.Util.Offices;
using System.Data;
using System.Collections;
using System.Collections.Generic;

namespace ERCHTMS.Web.Areas.AssessmentManage.Controllers
{
    /// <summary>
    /// �� ����������׼
    /// </summary>
    public class AssessmentChaptersController : MvcControllerBase
    {
        private AssessmentChaptersBLL assessmentchaptersbll = new AssessmentChaptersBLL();
        private AssessmentStandardBLL assessmentstandardbll = new AssessmentStandardBLL();

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
        /// �������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }

        /// <summary>
        /// ����ҳ��
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
        public ActionResult GetListDuty(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "Id as sumid";
            pagination.p_fields = "'' as DutyID,concat(MajorNumber,ChaptersName) as SumName,Reserve as DutyName";
            pagination.p_tablename = "bis_assessmentchapters";
            pagination.conditionJson = "ChaptersParentID='-1'";
            pagination.sidx = "cast(replace(majornumber,'.','') as number)";
            pagination.sord = "asc";
            var data = assessmentchaptersbll.GetPageList(pagination, queryJson);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = assessmentchaptersbll.GetList(queryJson);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "Id";
            pagination.p_fields = "CreateDate,MajorNumber,ChaptersName,Content,ReviewWay,Score,createuserid,createuserdeptcode,createuserorgcode";
            pagination.p_tablename = "bis_assessmentchapters";
            pagination.conditionJson = "1=1";
            var data = assessmentchaptersbll.GetPageList(pagination, queryJson);
            var watch = CommonHelper.TimerStart();
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
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = assessmentchaptersbll.GetEntity(keyValue);
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
            assessmentchaptersbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, AssessmentChaptersEntity entity)
        {
            assessmentchaptersbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion


        #region ����������׼
        /// <summary>
        /// ����������׼
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
                List<AssessmentChaptersEntity> listEntity = new List<AssessmentChaptersEntity>();
                AssessmentChaptersEntity chapter = new AssessmentChaptersEntity();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i][0].ToString()))
                    {
                        chapter = new AssessmentChaptersEntity();
                        chapter.MajorNumber = dt.Rows[i][0].ToString();
                        chapter.Id = Guid.NewGuid().ToString();
                        chapter.ChaptersName = dt.Rows[i][1].ToString();
                        if (hast.Keys.Count > 0)
                        {
                            string m = dt.Rows[i][0].ToString();
                            string[] b = new string[20];
                            string key = "";
                            if (m.Contains("."))
                            {
                                b = m.Split('.');
                                if (b.Length > 2)
                                {
                                    key = b[0] + "." + b[1];
                                }
                            }
                            object obj = hast[key];
                            if (obj == null)
                            {
                                chapter.ChaptersParentID = "-1";
                            }
                            else
                            {
                                chapter.ChaptersParentID = obj.ToString();
                                if (!string.IsNullOrEmpty(dt.Rows[i][4].ToString()))
                                {
                                    //���ֱ�׼
                                    AssessmentStandardEntity aentity = new AssessmentStandardEntity();
                                    aentity.AChapters = chapter.Id;
                                    aentity.Content = dt.Rows[i][4].ToString();
                                    assessmentstandardbll.SaveForm("", aentity);
                                }
                                if (!string.IsNullOrEmpty(dt.Rows[i][5].ToString()))
                                {
                                    chapter.ReviewWay = dt.Rows[i][5].ToString();
                                }
                            }
                        }
                        else
                        {
                            chapter.ChaptersParentID = "-1";
                        }
                        chapter.Content = dt.Rows[i][2].ToString();
                        chapter.Score = Convert.ToInt32(dt.Rows[i][3].ToString());
                        hast.Add(chapter.MajorNumber, chapter.Id);
                        listEntity.Add(chapter);
                    }
                    else
                    {
                        chapter.ChaptersName += dt.Rows[i][1].ToString();
                        chapter.Content += dt.Rows[i][2].ToString();
                        if (!string.IsNullOrEmpty(dt.Rows[i][4].ToString()))
                        {
                            //���ֱ�׼
                            AssessmentStandardEntity aentity = new AssessmentStandardEntity();
                            aentity.AChapters = chapter.Id;
                            aentity.Content = dt.Rows[i][4].ToString();
                            assessmentstandardbll.SaveForm("", aentity);
                        }
                    }
                }
                for (int j = 0; j < listEntity.Count; j++)
                {
                    try
                    {
                        assessmentchaptersbll.SaveForm("", listEntity[j]);
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
