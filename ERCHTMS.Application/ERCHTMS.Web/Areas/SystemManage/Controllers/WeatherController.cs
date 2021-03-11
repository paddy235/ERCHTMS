using System.Collections.Generic;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Busines.SystemManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using BSFramework.Util.Offices;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Code;

namespace ERCHTMS.Web.Areas.SystemManage.Controllers
{
    /// <summary>
    /// �� ��������Ԥ��
    /// </summary>
    public class WeatherController : MvcControllerBase
    {
        private WeatherBLL weatherbll = new WeatherBLL();

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
            var data = weatherbll.GetList(queryJson);
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
            var data = weatherbll.GetEntity(keyValue);
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
            pagination.p_kid = "ID";
            pagination.p_fields = @"a.CreateUserId,a.CreateDate,a.CreateUserName,a.ModifyUserId,a.ModifyDate,a.ModifyUserName,a.CreateUserDeptCode,a.CreateUserOrgCode,a.WEATHER,a.REQUIRE,a.INITIATEMODE";
            pagination.p_tablename = @"BIS_WEATHER a";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                //���ݵ�ǰ�û���ģ���Ȩ�޻�ȡ��¼
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "createuserdeptcode", "createuserorgcode");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }
            }


            var watch = CommonHelper.TimerStart();
            var data = weatherbll.GetPageList(pagination, queryJson);
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
        /// ����Ԥ��
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "����Ԥ��")]
        public ActionResult exportExcelData(string condition, string queryJson)
        {
            Pagination pagination = new Pagination();
            queryJson = queryJson ?? "";
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_fields = @"a.WEATHER,a.REQUIRE, case  when a.INITIATEMODE ='1' then '��' else '��' end INITIATEMODE";
            pagination.p_tablename = @"BIS_WEATHER a";
            pagination.sord = "CreateDate";
            #region Ȩ��У��
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "CREATEUSERDEPTCODE", "CREATEUSERORGCODE");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }

            }
            #endregion
            var data = weatherbll.GetPageList(pagination, queryJson);

            ////���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "����Ԥ��";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "����Ԥ��" + ".xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            excelconfig.ColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "weather".ToLower(), ExcelColumn = "����" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "require".ToLower(), ExcelColumn = "��ҵҪ��" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "initiatemode".ToLower(), ExcelColumn = "�Ƿ�����" });

            //���õ�������
            //ExcelHelper.ExcelDownload(data, excelconfig);
            //return Success("�����ɹ���");
            //���õ�����ʽ

            //���õ�������
            ExcelHelper.ExportByAspose(data, excelconfig.FileName, excelconfig.ColumnEntity);
            return Success("�����ɹ���");
        }


        /// <summary>
        /// ����������ȡԤ����Ϣ
        /// </summary>
        /// <param name="weather">����</param>
        /// <returns>Ԥ����ϢJson</returns>
        [HttpGet]
        public string GetRequire(string weather)
        {
            return weatherbll.GetRequire(weather);
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
            weatherbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, WeatherEntity entity)
        {
            weatherbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
