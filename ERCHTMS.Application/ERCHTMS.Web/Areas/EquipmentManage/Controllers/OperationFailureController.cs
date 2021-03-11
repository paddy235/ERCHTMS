using ERCHTMS.Entity.EquipmentManage;
using ERCHTMS.Busines.EquipmentManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using System.Data;
using BSFramework.Util.Offices;
using System.Collections.Generic;

namespace ERCHTMS.Web.Areas.EquipmentManage.Controllers
{
    /// <summary>
    /// �� �������й��ϼ�¼��
    /// </summary>
    public class OperationFailureController : MvcControllerBase
    {
        private OperationFailureBLL operationfailurebll = new OperationFailureBLL();

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
            var queryParam = queryJson.ToJObject();
            //�豸ID
            string equipmentid = queryParam["equipmentid"].ToString();
            pagination.p_kid = "ID";
            pagination.p_fields = "recordname,RegisterUser,RegisterDate,FailureNature,FailureReason,TakeSteps,HandleResult";
            pagination.p_tablename = "BIS_operationFailure t";
            pagination.conditionJson = string.Format(@" equipmentid='{0}'", equipmentid);
            var watch = CommonHelper.TimerStart();
            var data = operationfailurebll.GetPageList(pagination);
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
            var data = operationfailurebll.GetList(queryJson);
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
            var data = operationfailurebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ�豸�����¹ʼ�¼�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetBulletinPageListJson(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            //�豸ID
            string equipmentid = queryParam["equipmentid"].ToString();
            pagination.p_kid = "t.id";
            pagination.p_fields = "t.sgname_deal,t.happentime_deal,t.sglevelname_deal,y.jyjg,t.dcbgfiles,t.bulletinid";
            pagination.p_tablename = "AEM_BULLETIN_DEAL t left join AEM_BULLETIN y on t.bulletinid=y.id";
            pagination.conditionJson = string.Format(@" y.equipmentid='{0}'", equipmentid);
            var watch = CommonHelper.TimerStart();
            var data = operationfailurebll.GetPageList(pagination);
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
        /// ��ȡ�豸����������¼�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GethiddenbasePageListJson(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            //�豸ID
            string equipmentid = queryParam["equipmentid"].ToString();
            pagination.p_kid = "id";
            pagination.p_fields = "CHECKMANNAME,CHECKDATE,HIDDESCRIBE,CHANGEMEASURE,case when  ACCEPTSTATUS=1 then '����ͨ��' when ACCEPTSTATUS=1 then '���ղ�ͨ��' end as ACCEPTSTATUS,addtype,workstream";
            pagination.p_tablename = "v_hiddenbasedata t";
            pagination.conditionJson = string.Format(@" deviceid='{0}'", equipmentid);
            var watch = CommonHelper.TimerStart();
            var data = operationfailurebll.GetPageList(pagination);
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
        /// ��ȡ�豸������Ա��������
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetUserPageListJson(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            //�豸ID
            string UserIds = queryParam["UserIds"].ToString();
            pagination.p_kid = "a.userid";
            pagination.p_fields = "certname,a.Gender,certnum,senddate,sendorgan,years,a.realname,b.identifyid,a.deptname,a.mobile,enddate";
            pagination.p_tablename = "v_userinfo a left join (select t.userid,certname,Gender,certnum,senddate,sendorgan,years,realname,identifyid,deptname,enddate from BIS_CERTIFICATE t left join v_userinfo u on t.userid=u.userid) b on a.userid=b.userid";
            pagination.conditionJson = string.Format(" instr('{0}',a.userid)>0",UserIds);
            var watch = CommonHelper.TimerStart();
            var data = operationfailurebll.GetPageList(pagination);
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
        /// ��ȡʡ�����й���ͳ�Ƽ�¼
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public string GetOperationFailureRecordForSJ(string queryJson)
        {
            DataTable dt= operationfailurebll.GetOperationFailureRecordForSJ(queryJson);
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = 1,
                page = 1,
                records = dt.Rows.Count,
                rows = dt
            });
        }

        /// <summary>
        /// ����ʡ�����й��ϼ�¼
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HandlerMonitor(0, "����ʡ�����й��ϼ�¼")]
        public ActionResult ExportOperationFailureRecordForSJ(string queryJson)
        {

            var watch = CommonHelper.TimerStart();
            var data = operationfailurebll.GetOperationFailureRecordForSJ(queryJson);

            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "ʡ�������豸���й���ͳ��";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "ʡ�������豸���й��ϼ�¼.xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            ColumnEntity columnentity = new ColumnEntity();
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "failurenature", ExcelColumn = "�������ʼ�����", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "failurereason", ExcelColumn = "����ԭ��", Alignment = "center", Width = 100 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "takesteps", ExcelColumn = "��ȡ�Ĵ�ʩ", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "handleresult", ExcelColumn = "������", Alignment = "center" });
            //���õ�������
            ExcelHelper.ExcelDownload(data, excelconfig);

            return Success("�����ɹ���");
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
            operationfailurebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, OperationFailureEntity entity)
        {
            operationfailurebll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
