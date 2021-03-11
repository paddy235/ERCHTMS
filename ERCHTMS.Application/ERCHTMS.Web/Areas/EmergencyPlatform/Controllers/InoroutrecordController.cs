using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.Busines.EmergencyPlatform;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.AuthorizeManage;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using ERCHTMS.Code;
using ERCHTMS.Busines.BaseManage;

namespace ERCHTMS.Web.Areas.EmergencyPlatform.Controllers
{
    /// <summary>
    /// �� ����������¼
    /// </summary>
    public class InoroutrecordController : MvcControllerBase
    {
        private InoroutrecordBLL inoroutrecordbll = new InoroutrecordBLL();
        private SuppliesBLL suppliesbll = new SuppliesBLL();

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
        /// �û��б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>   
        //[HandlerMonitor(3, "��ҳ��ѯ�û���Ϣ!")]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            var SUPPLIESID = Request["SUPPLIESID"] ?? "";
            queryJson = queryJson ?? "";
            pagination.p_kid = "ID";
            pagination.p_fields = " STATUNAME,NUM,SUPPLIESUNTILNAME,USERNAME,INOROUTTIME";
            pagination.p_tablename = "MAE_INOROUTRECORD t";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }

            if (SUPPLIESID.Length > 0)
                pagination.conditionJson += string.Format(" and SUPPLIESID='{0}'", SUPPLIESID);
            var watch = CommonHelper.TimerStart();
            var data = inoroutrecordbll.GetPageList(pagination, queryJson);
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
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = inoroutrecordbll.GetList(queryJson);
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
            var data = inoroutrecordbll.GetEntity(keyValue);
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
            inoroutrecordbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, InoroutrecordEntity entity)
        {
            inoroutrecordbll.SaveForm(keyValue, entity);
            var entitySup = suppliesbll.GetEntity(entity.SUPPLIESID);
            //���
            if (entity.STATUS == 2)
                entitySup.NUM += entity.NUM;
            else
                entitySup.NUM -= entity.NUM;
            suppliesbll.SaveForm(entitySup.ID, entitySup);
            return Success("�����ɹ���");
        }
        #endregion

        #region ���ݵ���
        /// <summary>
        /// �����û��б�
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "������¼")]
        public ActionResult ExportInoroutrecordList(string condition, string queryJson)
        {
            Pagination pagination = new Pagination();
            var SUPPLIESID = Request["SUPPLIESID"] ?? "";
            queryJson = queryJson ?? "";
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "ID";
            pagination.p_fields = "CREATEUSERID, STATUNAME,NUM,SUPPLIESUNTILNAME,USERNAME,INOROUTTIME";
            pagination.p_tablename = "MAE_INOROUTRECORD t";
            pagination.conditionJson = "1=1";
            pagination.sidx = "STATUNAME";
            //string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "departmentcode", "organizecode");
            //pagination.conditionJson = string.IsNullOrEmpty(where) ? "1=1" : where;
            pagination.conditionJson += string.Format(" and SUPPLIESID='{0}'", SUPPLIESID);
            var data = inoroutrecordbll.GetPageList(pagination, queryJson);

            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "������¼";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "������¼.xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "STATUNAME".ToLower(), ExcelColumn = "��ʽ" });
            listColumnEntity.Add(new ColumnEntity() { Column = "NUM".ToLower(), ExcelColumn = "����" });
            listColumnEntity.Add(new ColumnEntity() { Column = "SUPPLIESUNTILNAME".ToLower(), ExcelColumn = "��λ" });
            listColumnEntity.Add(new ColumnEntity() { Column = "USERNAME".ToLower(), ExcelColumn = "ִ����" });
            listColumnEntity.Add(new ColumnEntity() { Column = "INOROUTTIME".ToLower(), ExcelColumn = "ʱ��" });
            excelconfig.ColumnEntity = listColumnEntity;

            //���õ�������
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
