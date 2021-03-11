using ERCHTMS.Entity.RoutineSafetyWork;
using ERCHTMS.Busines.RoutineSafetyWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System.Data;
using System;
using Newtonsoft.Json.Linq;
using ERCHTMS.Busines.PublicInfoManage;

namespace ERCHTMS.Web.Areas.RoutineSafetyWork.Controllers
{
    /// <summary>
    /// �� ����֪ͨ����
    /// </summary>
    public class AnnouncementController : MvcControllerBase
    {
        private AnnouncementBLL announcementbll = new AnnouncementBLL();
        private AnnounDetailBLL announdetailbll = new AnnounDetailBLL();

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
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.conditionJson = " 1=1 ";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");

                var data = announcementbll.GetPageList(pagination, queryJson, authType);
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
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }  
           
        }
        /// <summary>
        /// ��ȡ֪ͨ���������б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetMessDetailListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "t.id";
                pagination.p_fields = @"t.auuounid,t.username,t.useraccount,t.userid,t.looktime,t.deptid,t.deptname,t.deptcode,t.status";
                pagination.p_tablename = @"bis_announdetail t";
                pagination.sidx = "t.looktime";//�����ֶ�
                pagination.sord = "desc";//����ʽ
                Operator currUser = OperatorProvider.Provider.Current();
                pagination.conditionJson = "  1=1 ";

                var data = announdetailbll.GetPageList(pagination, queryJson);
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
            catch (System.Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetIndexJson(Pagination pagination, string queryJson)
        {

            var watch = CommonHelper.TimerStart();
            pagination.conditionJson = " 1=1 ";
            pagination.page = 1;
            pagination.rows = 8;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string authType="";
            if (!user.IsSystem)
            {
                if (user.RoleName.Contains("��˾") || user.RoleName.Contains("����"))
                {
                    authType = "4";
                }
                else
                {
                    authType = "3";
                }
            }
            var data = announcementbll.GetPageList(pagination, queryJson, authType);
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

        /// <summary>
        /// ��ȡʵ��͸�����Ϣ 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormAndFile(string keyValue)
        {
            var data = announcementbll.GetEntity(keyValue);
            var fileList = new FileInfoBLL().GetFileList(keyValue);
            var jsonData = new
            {
                data = data,
                fileList = fileList,
            };
            return ToJsonResult(jsonData);
        }

        /// <summary>
        /// ����δ�鿴״̬
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult UpdateStatus(string keyValue)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var detail = announdetailbll.GetEntity(user.UserId, keyValue);
                if (detail != null)
                {
                    detail.Status = 1;
                    detail.LookTime = DateTime.Now;
                    announdetailbll.SaveForm(detail.Id, detail);
                }
                return Success("�����ɹ���");
            }
            catch (Exception ex)
            {

                return Error(ex.Message);
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
            try
            {
                announcementbll.RemoveForm(keyValue);
                return Success("ɾ���ɹ���");
            }
            catch (Exception)
            {
                
                throw;
            }
           
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, AnnouncementEntity entity)
        {
            try
            {
                if (entity.IsSend=="0")//����ʱ���ѷ���ʱ��Ϊ׼
                {
                    entity.ReleaseTime = DateTime.Now;
                }
                announcementbll.SaveForm(keyValue, entity);
                return Success("�����ɹ���");
            }
            catch (Exception ex)
            {

                return Error(ex.Message);
            }
           
        }
        #endregion

        #region ���ݵ���
        /// <summary>
        /// �����û��б�
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "����֪ͨ��������")]
        public ActionResult Export(string queryJson)
        {

            var watch = CommonHelper.TimerStart();
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.conditionJson = " 1=1 ";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
            var data = announcementbll.GetPageList(pagination, queryJson, authType);
            DataTable excelTable = new DataTable();
            excelTable.Columns.Add(new DataColumn("isimportant"));
            excelTable.Columns.Add(new DataColumn("notictype"));
            excelTable.Columns.Add(new DataColumn("title"));
            excelTable.Columns.Add(new DataColumn("publisherdept"));
            excelTable.Columns.Add(new DataColumn("publisher"));
            excelTable.Columns.Add(new DataColumn("releasetime"));

            foreach (DataRow item in data.Rows)
            {
                DataRow newDr = excelTable.NewRow();
                newDr["isimportant"] = item["isimportant"];
                newDr["notictype"] = item["notictype"];
                newDr["title"] = item["title"];
                newDr["publisherdept"] = item["publisherdept"];
                newDr["publisher"] = item["publisher"];

                DateTime releasetime;
                DateTime.TryParse(item["releasetime"].ToString(), out releasetime);
                newDr["releasetime"] = releasetime.ToString("yyyy-MM-dd HH:mm") != "0001-01-01 00:00" ? releasetime.ToString("yyyy-MM-dd HH:mm") : "";
                excelTable.Rows.Add(newDr);
            }
            var query = JObject.Parse(queryJson);
            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "����";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "����.xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            ColumnEntity columnentity = new ColumnEntity();
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "isimportant", ExcelColumn = "��Ҫ", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "notictype", ExcelColumn = "����", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "title", ExcelColumn = "����", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "publisherdept", ExcelColumn = "��������", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "publisher", ExcelColumn = "������", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "releasetime", ExcelColumn = "����ʱ��", Alignment = "center" });
            //���õ�������
            ExcelHelper.ExcelDownload(excelTable, excelconfig);

            return Success("�����ɹ���");
        }
        #endregion
    }
}
