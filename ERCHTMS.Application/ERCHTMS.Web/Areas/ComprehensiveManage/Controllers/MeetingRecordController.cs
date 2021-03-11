using ERCHTMS.Entity.ComprehensiveManage;
using ERCHTMS.Busines.ComprehensiveManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Web.Areas.ComprehensiveManage.Controllers
{
    /// <summary>
    /// �� ����֪ͨ����
    /// </summary>
    public class MeetingRecordController : MvcControllerBase
    {
        private MeetingRecordBLL meetingrecordbll = new MeetingRecordBLL();

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
            var data = meetingrecordbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>   
        //[HandlerMonitor(3, "��ҳ��ѯ�û���Ϣ!")]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            queryJson = queryJson ?? "";
            pagination.p_kid = "Id";
            pagination.p_fields = "Security,Name,SettlePerson,IssueTime,ReadUserIdList,IsSend,CreateUserId";
            pagination.p_tablename = "HRS_MEETINGRECORD";
            pagination.conditionJson = "1=1";


            var watch = CommonHelper.TimerStart();
            var data = meetingrecordbll.GetPageList(pagination, queryJson);
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
            var data = meetingrecordbll.GetEntity(keyValue);
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
            meetingrecordbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue,string isSend, MeetingRecordEntity entity)
        {
            entity.IsSend = isSend;//�Ƿ���
            entity.IssueTime = DateTime.Now;//����ʱ��
            meetingrecordbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion

        #region ���ݵ���
        /// <summary>
        /// �����Ҫ����
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult ExportData(string keyValue)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 1;
            pagination.p_kid = "id";
            pagination.p_fields = "Security,Name,SettlePerson,Content,code,meetingtime,Address,direct,attendperson";//,Content,code,meetingtime,address,direct,attendperson,settleperson
            pagination.p_tablename = "HRS_MEETINGRECORD s";
            pagination.conditionJson = "id='" + keyValue + "'";
            //pagination.sidx = "realname";
            //pagination.sord = "desc";
            DataTable dtUser = meetingrecordbll.GetPageList(pagination, "{}");

            dtUser.TableName = "U";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user != null)
            {
                dtUser.Columns.Add("company");
                dtUser.Rows[0]["company"] = user.OrganizeName;
            }

            Aspose.Words.Document doc = new Aspose.Words.Document(Server.MapPath("~/Resource/ExcelTemplate/�����Ҫģ��.doc"));
            Aspose.Words.DocumentBuilder db = new Aspose.Words.DocumentBuilder(doc);
            doc.MailMerge.Execute(dtUser);

            doc.MailMerge.DeleteFields();
            string filePath = Server.MapPath("~/Resource/temp/" + dtUser.Rows[0]["name"].ToString() + ".doc");
            doc.Save(filePath);

            string url = "../../Utility/DownloadFile?filePath=" + filePath + "&speed=102400&newFileName=" + dtUser.Rows[0]["name"].ToString() + ".doc";
            return Redirect(url);
        }
        #endregion
    }
}
