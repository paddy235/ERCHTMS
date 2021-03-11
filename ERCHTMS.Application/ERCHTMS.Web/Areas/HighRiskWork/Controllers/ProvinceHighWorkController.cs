using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.AuthorizeManage;
using System.Linq;
using ERCHTMS.Busines.HighRiskWork;
using ERCHTMS.Code;
using System.Data;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Web.Areas.HighRiskWork.Controllers
{
    /// <summary>
    /// �� ����ʡ��ҳ��
    /// </summary>
    public class ProvinceHighWorkController : MvcControllerBase
    {
        private ProvinceHighWorkBLL provincehighworkbll = new ProvinceHighWorkBLL();

        #region ��ͼ����
        /// <summary>
        /// �߷����嵥ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// ͳ��ͼҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult StatisticsIndex()
        {
            return View();
        }
        #endregion


        /// <summary>
        ///��ҵ����ͳ��(ͳ��ͼ)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetProvinceHighCount(string starttime, string endtime, string deptid, string deptcode)
        {
            return provincehighworkbll.GetProvinceHighCount(starttime, endtime, deptid, deptcode);
        }

        /// <summary>
        ///��ҵ����ͳ��(ͳ�Ʊ��)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetProvinceHighList(string starttime, string endtime, string deptid, string deptcode)
        {
            return provincehighworkbll.GetProvinceHighList(starttime, endtime, deptid, deptcode);
        }

        /// <summary>
        /// ��λ�Ա�(ͳ��ͼ)
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetProvinceHighDepartCount(string starttime, string endtime)
        {
            return provincehighworkbll.GetProvinceHighDepartCount(starttime, endtime);
        }

        /// <summary>
        ///��λ�Ա�(ͳ�Ʊ��)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetProvinceHighDepartList(string starttime, string endtime)
        {
            return provincehighworkbll.GetProvinceHighDepartList(starttime, endtime);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetPageTableJson(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_kid = "a.Id";
            pagination.p_fields = "worktype,b.itemname worktypename,applynumber,workdepttypename,engineeringname,workplace,workstarttime,workendtime,workdeptname,a.createusername,a.createdate,c.fullname as createuserorgname";
            pagination.p_tablename = " v_highriskstat a left join base_dataitemdetail b on a.worktype=b.itemvalue and itemid =(select itemid from base_dataitem where itemcode='StatisticsType') left join base_department c on a.createuserorgcode=c.encode";
            pagination.conditionJson = string.Format("WorkDeptCode in (select encode from base_department start with encode='{0}' connect by  prior departmentid = parentid)", user.NewDeptCode);
            var data = provincehighworkbll.GetPageDataTable(pagination, queryJson);
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

        #region ����
        /// <summary>
        /// ����
        /// </summary>
        [HandlerMonitor(0, "��������")]
        public ActionResult ExportData(string queryJson)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.p_kid = "a.Id";
                pagination.p_fields = "b.itemname worktypename,applynumber,workdepttypename,engineeringname,workplace,to_char(workstarttime,'yyyy-mm-dd hh24:mi') || ' - '||to_char(workendtime,'yyyy-mm-dd hh24:mi') as worktime,workdeptname,a.createusername,a.createdate,c.fullname as createuserorgname";
                pagination.p_tablename = " v_highriskstat a left join base_dataitemdetail b on a.worktype=b.itemvalue and itemid =(select itemid from base_dataitem where itemcode='StatisticsType') left join base_department c on a.createuserorgcode=c.encode";
                pagination.conditionJson = string.Format("WorkDeptCode in (select encode from base_department start with encode='{0}' connect by  prior departmentid = parentid)", user.NewDeptCode);
                pagination.sidx = "a.createdate";//�����ֶ�
                pagination.sord = "desc";//����ʽ

                DataTable exportTable = provincehighworkbll.GetPageDataTable(pagination, queryJson);
                //���õ�����ʽ
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "�߷�����ҵ��Ϣ";
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "�߷�����ҵ��Ϣ.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //�������Դ��˳�򱣳�һ��
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "worktypename", ExcelColumn = "��ҵ����", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applynumber", ExcelColumn = "������", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workdepttypename", ExcelColumn = "��ҵ��λ���", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "engineeringname", ExcelColumn = "��������", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workplace", ExcelColumn = "��ҵ�ص�", Width = 60 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "worktime", ExcelColumn = "��ҵʱ��", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workdeptname", ExcelColumn = "��ҵ��λ", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createusername", ExcelColumn = "������", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createdate", ExcelColumn = "����ʱ��", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createuserorgname", ExcelColumn = "�����糧", Width = 20 });
                //���õ�������
                ExcelHelper.ExcelDownload(exportTable, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("�����ɹ���");
        }
        #endregion

    }
}
