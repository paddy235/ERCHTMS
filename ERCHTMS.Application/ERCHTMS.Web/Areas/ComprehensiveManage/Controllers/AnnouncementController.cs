using ERCHTMS.Entity.ComprehensiveManage;
using ERCHTMS.Busines.ComprehensiveManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using BSFramework.Util.Offices;
using System.Collections.Generic;

namespace ERCHTMS.Web.Areas.ComprehensiveManage.Controllers
{
    /// <summary>
    /// �� ����֪ͨ����
    /// </summary>
    public class AnnouncementController : MvcControllerBase
    {
        private AnnouncementBLL announcementbll = new AnnouncementBLL();

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
            var data = announcementbll.GetList(queryJson);
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
            pagination.p_fields = "IsRemind,Title,IssueDeptName, IssuerName,IssueTime,ReadUserIdList,IsSend,CreateUserId";
            pagination.p_tablename = "HRS_ANNOUNCEMENT";
            pagination.conditionJson = "1=1";
            //pagination.

            var watch = CommonHelper.TimerStart();
            var data = announcementbll.GetPageList(pagination, queryJson);
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
            var data = announcementbll.GetEntity(keyValue);
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
            announcementbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue,string isSend, AnnouncementsEntity entity)
        {
            entity.IsSend = isSend;//�Ƿ���
            announcementbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion

        #region ���ݵ���
        /// <summary>
        /// �����û��б�
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "����֪ͨ��������")]
        public ActionResult ExportData(string queryJson)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "Id";
            pagination.p_fields = "(case isremind when 1 then '��' else '��' end) as IsRemind,Title,IssueDeptName,IssuerName,IssueTime";
            pagination.p_tablename = "HRS_ANNOUNCEMENT";
            pagination.conditionJson = "1=1";

            var watch = CommonHelper.TimerStart();
            var data = announcementbll.GetPageList(pagination, queryJson);

            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "֪ͨ����";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "֪ͨ����.xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            ColumnEntity columnentity = new ColumnEntity();
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "r", ExcelColumn = "���", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "isremind", ExcelColumn = "��Ҫ", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "title", ExcelColumn = "����", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "issuedeptname", ExcelColumn = "��������", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "issuername", ExcelColumn = "������", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "issuetime", ExcelColumn = "����ʱ��", Alignment = "center" });
            
            //���õ�������
            ExcelHelper.ExcelDownload(data, excelconfig);

            return Success("�����ɹ���");
        }
        #endregion
    }
}
