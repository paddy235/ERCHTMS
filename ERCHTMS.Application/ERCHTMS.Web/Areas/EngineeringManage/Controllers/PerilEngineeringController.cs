using ERCHTMS.Entity.EngineeringManage;
using ERCHTMS.Busines.EngineeringManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Data;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Code;
using BSFramework.Data;
using ERCHTMS.Busines.PublicInfoManage;

namespace ERCHTMS.Web.Areas.EngineeringManage.Controllers
{
    /// <summary>
    /// �� ����Σ�󹤳̹���
    /// </summary>
    public class PerilEngineeringController : MvcControllerBase
    {
        private PerilEngineeringBLL perilengineeringbll = new PerilEngineeringBLL();
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();

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
        /// �ļ�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Files()
        {
            return View();
        }


        /// <summary>
        /// ���̽�չ���ͳ���б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CaseList()
        {
            return View();
        }

        /// <summary>
        /// ʡ����˾��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SIndex()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SJIndexList()
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
            pagination.p_kid = "Id";
            pagination.p_fields = "CreateDate,EvolveCase,TaskFiles,ConstructFiles,EFinishTime,EStartTime,UnitType,BelongDeptName,EngineeringType,EngineeringName,createuserid,createuserdeptcode,createuserorgcode";
            pagination.p_tablename = " bis_perilengineering";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
            if (!string.IsNullOrEmpty(authType))
            {
                switch (authType)
                {
                    case "1":
                        pagination.conditionJson += " createuserid='" + user.UserId + "'";
                        break;
                    case "2":
                        pagination.conditionJson += string.Format(" belongdeptid  in (select departmentid from base_department where encode='{0}' union select ORGANIZEID from BASE_ORGANIZE where encode='{0}')", user.OrganizeCode);
                        break;
                    case "3":
                        pagination.conditionJson += string.Format(" belongdeptid  in (select departmentid from base_department where encode like '{0}%' union select ORGANIZEID from BASE_ORGANIZE where encode like '{0}%')", user.DeptCode);
                        break;
                    case "4":
                        pagination.conditionJson +=string.Format("  belongdeptid  in (select departmentid from base_department where encode like '{0}%' union select ORGANIZEID from BASE_ORGANIZE where encode like '{0}%')",user.OrganizeCode);
                        break;
                }
            }
            else
            {
                pagination.conditionJson += " 0=1";
            }
            var data = perilengineeringbll.GetPageList(pagination, queryJson);
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
        /// ��ȡʡ���б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetPageListJsonForSJ(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "a.Id";
            pagination.p_fields = "a.CreateDate,a.EvolveCase,a.TaskFiles,a.ConstructFiles,a.EFinishTime,a.EStartTime,a.UnitType,a.BelongDeptName,a.EngineeringType,a.EngineeringName,a.createuserid,a.createuserdeptcode,a.createuserorgcode,b.fullname";
            pagination.p_tablename = " bis_perilengineering a left join base_department b on a.CREATEUSERORGCODE = b.encode";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            //string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
            //if (!string.IsNullOrEmpty(authType))
            //{
            //    switch (authType)
            //    {
            //        case "1":
            //            pagination.conditionJson += " createuserid='" + user.UserId + "'";
            //            break;
            //        case "2":
            //            pagination.conditionJson += string.Format(" belongdeptid  in (select departmentid from base_department where encode='{0}' union select ORGANIZEID from BASE_ORGANIZE where encode='{0}')", user.OrganizeCode);
            //            break;
            //        case "3":
            //            pagination.conditionJson += string.Format(" belongdeptid  in (select departmentid from base_department where encode like '{0}%' union select ORGANIZEID from BASE_ORGANIZE where encode like '{0}%')", user.DeptCode);
            //            break;
            //        case "4":
            //            pagination.conditionJson += string.Format("  belongdeptid  in (select departmentid from base_department where encode like '{0}%' union select ORGANIZEID from BASE_ORGANIZE where encode like '{0}%')", user.OrganizeCode);
            //            break;
            //    }
            //}
            //else
            //{
            //    pagination.conditionJson += " 0=1";
            //}
            pagination.conditionJson += string.Format("  a.belongdeptid  in (select departmentid from base_department where deptcode like '{0}%' union select ORGANIZEID from BASE_ORGANIZE where encode like '{0}%')", user.OrganizeCode);
            var data = perilengineeringbll.GetPageList(pagination, queryJson);
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
        /// ����
        /// </summary>
        [HandlerMonitor(0, "��������")]
        public ActionResult ExportData(string queryJson)
        {
            try
            {
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.p_kid = "a.Id";
                pagination.p_fields = "EngineeringName,ProgrammeCategory as EngineeringType,BelongDeptName,case when UnitType = 1 then '��λ�ڲ�' else '�����λ' end as UnitType,EStartTime,EFinishTime,ConstructFiles,TaskFiles,EvolveCase";
                pagination.p_tablename = " bis_perilengineering a left join bis_engineeringsetting b on a.EngineeringType = b.id";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                if (!string.IsNullOrEmpty(authType))
                {
                    switch (authType)
                    {
                        case "1":
                            pagination.conditionJson += " a.createuserid='" + user.UserId + "'";
                            break;
                        case "2":
                            pagination.conditionJson += string.Format(" belongdeptid  in (select departmentid from base_department where encode='{0}' union select ORGANIZEID from BASE_ORGANIZE where encode='{0}')", user.OrganizeCode);
                            break;
                        case "3":
                            pagination.conditionJson += string.Format(" belongdeptid  in (select departmentid from base_department where encode like '{0}%' union select ORGANIZEID from BASE_ORGANIZE where encode like '{0}%')", user.DeptCode);
                            break;
                        case "4":
                            pagination.conditionJson += string.Format("  belongdeptid  in (select departmentid from base_department where encode like '{0}%' union select ORGANIZEID from BASE_ORGANIZE where encode like '{0}%')", user.OrganizeCode);
                            break;
                    }
                }
                else
                {
                    pagination.conditionJson += " 0=1";
                }
                DataTable exportTable = perilengineeringbll.GetPageList(pagination, queryJson);
                foreach (DataRow item in exportTable.Rows)
                {
                      var dt= fileInfoBLL.GetFiles(item["ConstructFiles"].ToString());
                      if (dt.Rows.Count > 0)
                          item["ConstructFiles"] = "��";
                      else
                          item["ConstructFiles"] = "��";
                      var dt1 = fileInfoBLL.GetFiles(item["TaskFiles"].ToString());
                      if (dt1.Rows.Count > 0)
                          item["TaskFiles"] = "��";
                      else
                          item["TaskFiles"] = "��";
                }
                //���õ�����ʽ
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "Σ�󹤳���Ϣ";
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "Σ�󹤳���Ϣ����.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //�������Դ��˳�򱣳�һ��
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "engineeringname", ExcelColumn = "��������", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "engineeringtype", ExcelColumn = "�������", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "belongdeptname", ExcelColumn = "������λ", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "unittype", ExcelColumn = "��λ���", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "estarttime", ExcelColumn = "��ʼʱ��", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "efinishtime", ExcelColumn = "����ʱ��", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "constructfiles", ExcelColumn = "ʩ������", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "taskfiles", ExcelColumn = "��ȫ��������", Width = 12 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "evolvecase", ExcelColumn = "��չ���", Width = 15 });
                //���õ�������
                ExcelHelper.ExcelDownload(exportTable, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("�����ɹ���");
        }


        /// <summary>
        /// ����ʡ������
        /// </summary>
        [HandlerMonitor(0, "��������")]
        public ActionResult ExportDataForSJ(string queryJson)
        {
            try
            {
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.p_kid = "a.Id";
                pagination.p_fields = "EngineeringName,ProgrammeCategory as EngineeringType,BelongDeptName,case when UnitType = 1 then '��λ�ڲ�' else '�����λ' end as UnitType,EStartTime,EFinishTime,ConstructFiles,TaskFiles,EvolveCase,c.fullname";
                pagination.p_tablename = " bis_perilengineering a left join bis_engineeringsetting b on a.EngineeringType = b.id left join base_department c on a.createuserorgcode = c.encode";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                pagination.conditionJson += string.Format("  a.belongdeptid  in (select departmentid from base_department where deptcode like '{0}%' union select ORGANIZEID from BASE_ORGANIZE where encode like '{0}%')", user.OrganizeCode);
                DataTable exportTable = perilengineeringbll.GetPageList(pagination, queryJson);
                foreach (DataRow item in exportTable.Rows)
                {
                    var dt = fileInfoBLL.GetFiles(item["ConstructFiles"].ToString());
                    if (dt.Rows.Count > 0)
                        item["ConstructFiles"] = "��";
                    else
                        item["ConstructFiles"] = "��";
                    var dt1 = fileInfoBLL.GetFiles(item["TaskFiles"].ToString());
                    if (dt1.Rows.Count > 0)
                        item["TaskFiles"] = "��";
                    else
                        item["TaskFiles"] = "��";
                }
                //���õ�����ʽ
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "Σ�󹤳���Ϣ";
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "Σ�󹤳���Ϣ����.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //�������Դ��˳�򱣳�һ��
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "engineeringname", ExcelColumn = "��������", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "engineeringtype", ExcelColumn = "�������", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "belongdeptname", ExcelColumn = "������λ", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "unittype", ExcelColumn = "��λ���", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "estarttime", ExcelColumn = "��ʼʱ��", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "efinishtime", ExcelColumn = "����ʱ��", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "constructfiles", ExcelColumn = "ʩ������", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "taskfiles", ExcelColumn = "��ȫ��������", Width = 12 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "evolvecase", ExcelColumn = "��չ���", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "fullname", ExcelColumn = "�����糧", Width = 15 });
                //���õ�������
                ExcelHelper.ExcelDownload(exportTable, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("�����ɹ���");
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = perilengineeringbll.GetList(queryJson);
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
            var data = perilengineeringbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }


        /// <summary>
        /// ����������ȡ����
        /// </summary>
        [HttpGet]
        public ActionResult GetPeril(string code, string st, string et, string keyword)
        {
            var data = perilengineeringbll.GetPeril(code, st, et, keyword);
            return ToJsonResult(data);

        }

        /// <summary>
        /// ʡ������������ȡ���� ����GetPeril �ϲ�
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPerilForSJIndex(string queryJson)
        {
            var data = perilengineeringbll.GetPerilForSJIndex(queryJson);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ������𣨰󶨿ؼ���
        /// </summary>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetEngineeringTypeJson()
        {
            var data = perilengineeringbll.GetEngineeringType();
            return Content(data.ToJson());
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
            perilengineeringbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, PerilEngineeringEntity entity)
        {
            perilengineeringbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
