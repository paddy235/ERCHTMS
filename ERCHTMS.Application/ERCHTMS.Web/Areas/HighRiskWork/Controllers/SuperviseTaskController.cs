using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Busines.HighRiskWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using System.Data;
using System.Web;
using System;
using System.Linq;
using System.Collections.Generic;
using ERCHTMS.Cache;
using ERCHTMS.Entity.SystemManage.ViewModel;
using Newtonsoft.Json;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using System.Net;
using ERCHTMS.Busines.SystemManage;
using BSFramework.Util.Offices;

namespace ERCHTMS.Web.Areas.HighRiskWork.Controllers
{
    /// <summary>
    /// �� ������վ�ල����
    /// </summary>
    public class SuperviseTaskController : MvcControllerBase
    {
        private SuperviseTaskBLL supervisetaskbll = new SuperviseTaskBLL();
        private SidePersonBLL sidepersonbll = new SidePersonBLL();

        private HighRiskCommonApplyBLL highriskcommonapplybll = new HighRiskCommonApplyBLL();
        private SafetychangeBLL safetychangebll = new SafetychangeBLL();
        private ScaffoldBLL scaffoldbll = new ScaffoldBLL();
        private DataItemCache dataItemCache = new DataItemCache();
        private SuperviseWorkInfoBLL superviseworkinfobll = new SuperviseWorkInfoBLL();

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
            ViewBag.IsTeams = new DataItemDetailBLL().GetItemValue("�Ƿ�������ʱ����");
            return View();
        }

        /// <summary>
        /// �ļ�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ShowFiles()
        {
            return View();
        }

        /// <summary>
        /// ѡ��߷���ͨ����ҵҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SelectCommon()
        {
            return View();
        }

        /// <summary>
        /// ѡ��߷�����ʩ�䶯ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SelectChange()
        {
            return View();
        }

        /// <summary>
        /// ѡ��߷��ս��ּ�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SelectScaffold()
        {
            return View();
        }

        /// <summary>
        /// ѡ����ҵ���
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SelectWorkType()
        {
            return View();
        }

        /// <summary>
        /// �ල��������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TaskForm()
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
            pagination.p_kid = "a.Id as workid";
            pagination.p_fields = "supervisestate,'' as taskdept,'' as taskworkplace,a.CreateDate,TaskWorkStartTime,TaskWorkEndTime,TimeLong,taskusername,a.createuserid,taskworktype,handtype,b.fullname,TaskLevel";
            pagination.p_tablename = "bis_supervisetask a left join Base_Department b on a.steamid=b.departmentid";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                if (!string.IsNullOrEmpty(authType))
                {
                    switch (authType)
                    {
                        case "1":
                            pagination.conditionJson += " and a.createuserid='" + user.UserId + "'";
                            break;
                        case "2":
                            pagination.conditionJson += " and a.createuserdeptcode='" + user.DeptCode + "'";
                            break;
                        case "3"://���Ӳ���
                            pagination.conditionJson += string.Format(" and ((a.steamid in(select departmentid from base_department  where encode like '{0}%' or senddeptid='{1}') and supervisestate!='1') or a.createuserid='{2}')", user.DeptCode, user.DeptId, user.UserId);
                            break;
                        case "4":
                            pagination.conditionJson += " and ((a.createuserorgcode='" + user.OrganizeCode + "' and supervisestate!='1') or(a.createuserid='" + user.UserId + "'))";
                            break;
                    }
                }
                else
                {
                    pagination.conditionJson += " and 0=1";
                }
            }
            var data = supervisetaskbll.GetPageDataTable(pagination, queryJson);
            foreach (DataRow item in data.Rows)
            {

                var workdata = superviseworkinfobll.GetList(item["workid"].ToString());
                string place = "", deptname = "";
                foreach (var work in workdata)
                {
                    place += work.WorkPlace + ",";
                    deptname += work.WorkDeptName + ",";
                }
                if (!string.IsNullOrEmpty(place))
                    item["taskworkplace"] = place.TrimEnd(',');
                if (!string.IsNullOrEmpty(deptname))
                    item["taskdept"] = deptname.TrimEnd(',');
            }
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
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = supervisetaskbll.GetList(queryJson);
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
            var data = supervisetaskbll.GetEntity(keyValue);
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
            supervisetaskbll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="jsonData">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, string jsonData)
        {
            SuperviseTaskModel model = JsonConvert.DeserializeObject<SuperviseTaskModel>(jsonData);
            model.TaskLevel = "0";
            string objectId = supervisetaskbll.SaveForm(keyValue, model);
            if (model.SuperviseState == "2")//�ύ
            {
                var str = new DataItemDetailBLL().GetItemValue("�Ƿ�������ʱ����");
                if (str != "1")
                {
                    model.SuperParentId = objectId;
                    model.TaskLevel = "1";//����
                    model.WorkSpecs = new SuperviseWorkInfoBLL().GetList(keyValue).OrderBy(t => t.CreateDate).ToList();
                    supervisetaskbll.SaveForm("", model);
                }
            }
            return Success("�����ɹ���");
        }
        #endregion

        #region ����
        /// <summary>
        /// ������Excel
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult ExportData(string queryJson)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;

            pagination.p_kid = "a.Id as workid";
            pagination.p_fields = "supervisestate,taskworktype,handtype,to_char(TaskWorkStartTime,'yyyy-mm-dd hh24:mi') ||  ' - ' || to_char(TaskWorkStartTime,'yyyy-mm-dd hh24:mi'),'' as taskdept,'' as taskworkplace,b.fullname,taskusername,'-' as timelong";
            pagination.p_tablename = "bis_supervisetask a left join Base_Department b on a.steamid=b.departmentid";
            pagination.conditionJson = "1=1";
            pagination.sidx = "supervisestate asc,a.createdate";
            pagination.sord = "desc";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                if (!string.IsNullOrEmpty(authType))
                {
                    switch (authType)
                    {
                        case "1":
                            pagination.conditionJson += " and a.createuserid='" + user.UserId + "'";
                            break;
                        case "2":
                            pagination.conditionJson += " and a.createuserdeptcode='" + user.DeptCode + "'";
                            break;
                        case "3"://���Ӳ���
                            pagination.conditionJson += string.Format(" and ((a.steamid in(select departmentid from base_department  where encode like '{0}%' or senddeptid='{1}') and supervisestate!='1') or a.createuserid='{2}')", user.DeptCode, user.DeptId, user.UserId);
                            break;
                        case "4":
                            pagination.conditionJson += " and ((a.createuserorgcode='" + user.OrganizeCode + "' and supervisestate!='1') or(a.createuserid='" + user.UserId + "'))";
                            break;
                    }
                }
                else
                {
                    pagination.conditionJson += " and 0=1";
                }
            }
            DataTable exportTable = supervisetaskbll.GetPageDataTable(pagination, queryJson);
            foreach (DataRow item in exportTable.Rows)
            {
                if (item["supervisestate"].ToString() == "1")
                {
                    item["supervisestate"] = "�����ල";
                }
                else if (item["supervisestate"].ToString() == "2")
                {
                    item["supervisestate"] = "δ�ල";
                }
                else
                {
                    item["supervisestate"] = "�Ѽල";
                }
                var type = "";
                if (!string.IsNullOrEmpty(item["taskworktype"].ToString()) && !string.IsNullOrEmpty(item["handtype"].ToString()))
                {
                    type = item["taskworktype"].ToString() + "," + item["handtype"].ToString();
                }
                else
                {
                    if (!string.IsNullOrEmpty(item["taskworktype"].ToString()))
                    {
                        type = item["taskworktype"].ToString();
                    }
                    if (!string.IsNullOrEmpty(item["handtype"].ToString()))
                    {
                        type = item["handtype"].ToString();
                    }
                }
                item["taskworktype"] = type;
                var workdata = superviseworkinfobll.GetList(item["workid"].ToString());
                string place = "", deptname = "";
                foreach (var work in workdata)
                {
                    place += work.WorkPlace + ",";
                    deptname += work.WorkDeptName + ",";
                }
                if (!string.IsNullOrEmpty(place))
                    item["taskworkplace"] = place.TrimEnd(',');
                if (!string.IsNullOrEmpty(deptname))
                    item["taskdept"] = deptname.TrimEnd(',');
            }
            exportTable.Columns.Remove("workid");
            exportTable.Columns.Remove("handtype");
            exportTable.Columns.Remove("r");
            // ȷ�������ļ���
            string fileName = "�߷�����ҵ��վ�ල��Ϣ";
            HttpResponse resp = System.Web.HttpContext.Current.Response;

            // ��ϸ�б�����
            string fielname = fileName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
            wb.Open(Server.MapPath("~/Resource/ExcelTemplate/�߷�����ҵ��վ�ල��Ϣ.xlsx"));
            Aspose.Cells.Worksheet sheet = wb.Worksheets[0] as Aspose.Cells.Worksheet;
            Aspose.Cells.Cell cell = sheet.Cells[1, 1];
            sheet.Cells.ImportDataTable(exportTable, false, 1, 0);
            wb.Save(Server.UrlEncode(fielname), Aspose.Cells.FileFormatType.Excel2003, Aspose.Cells.SaveType.OpenInBrowser, resp);

            return Success("�����ɹ�!");
        }
        #endregion


        #region �߷�����ҵ

        #region �߷���ͨ��ѡ��ҳ��
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetSelectCommonWorkJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "Id as workid";
            pagination.p_fields = "case when workdepttype=0 then '�糧�ڲ�' when workdepttype=1 then '�����λ' end workdepttypename,workdepttype,workdeptid,workdeptname,workdeptcode,applynumber,CreateDate,workplace,workcontent,workstarttime,workendtime,applyusername,EngineeringName,EngineeringId";
            pagination.p_tablename = " bis_highriskcommonapply a";
            pagination.conditionJson = "applystate='5'";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                pagination.conditionJson += string.Format(" and WorkDeptCode in(select encode from base_department  where encode like '{0}%' or senddeptid='{1}')", user.DeptCode, user.DeptId);
            }
            var data = highriskcommonapplybll.GetPageDataTable(pagination, queryJson);
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
        #endregion

        /// <summary>
        /// ��ȡ��ʩ�䶯�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetSelectChangeWorkJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "Id as workid";
            pagination.p_fields = "workunit,workunitid,workunitcode,case when workunittype='0'  then '�糧�ڲ�'  when  workunittype='1' then '�����λ' end workunittypename,workunittype,changereason,workplace,applychangetime,returntime,projectid,projectname";
            pagination.p_tablename = " bis_Safetychange t";
            pagination.conditionJson = "iscommit=1 and isapplyover=1 and isaccpcommit in (0,1) and isaccepover=0";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                pagination.conditionJson += string.Format(" and workunitcode in(select encode from base_department  where encode like '{0}%' or senddeptid='{1}')", user.DeptCode, user.DeptId);
            }
            var data = safetychangebll.GetPageList(pagination, queryJson);
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
        /// ��ȡ���ּ��б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetSelectScaffoldWorkJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "Id as workid";
            pagination.p_fields = "purpose,dismentlereason,setupcompanyname,setupcompanyid,setupcompanycode,case when setupcompanytype='0' then '�糧�ڲ�'  when  setupcompanytype='1' then '�����λ' end setupcompanytypename,setupcompanytype,setupstartdate,setupenddate,setupaddress,dismentlestartdate,dismentleenddate,outprojectid,outprojectname";
            pagination.p_tablename = " bis_scaffold";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                pagination.conditionJson += string.Format(" and setupcompanyid in(select departmentid from base_department  where encode like '{0}%' or senddeptid='{1}')", user.DeptCode, user.DeptId);
            }
            var data = scaffoldbll.GetSelectPageList(pagination, queryJson);
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
        #endregion


        #region ��ѯ��ҵ���

        /// <summary>
        /// ��ѯ��ҵ����� 
        /// </summary>
        /// <param name="checkMode">��ѡ���ѡ��0:��ѡ��1:��ѡ</param>
        /// <param name="mode">��ѯģʽ��0:��ȡ����IDΪIds�����еĲ���(��OrganizeId=Ids)��1:��ȡ����IDΪIds�����в���(��ParentId=Ids)��2:��ȡ���ŵ�parentId��Ids�еĲ���(��ParentId in(Ids))��3:��ȡ����Id��Ids�еĲ���(��DepartmentId in(Ids))</param>
        /// <param name="typeIDs">��ɫIDs</param>
        /// <returns>��������Json</returns>
        [HttpGet]
        public ActionResult GetTypeTreeJson(string typeIDs, int checkMode = 0, int mode = 0)
        {
            var treeList = new List<TreeEntity>();
            IEnumerable<DataItemModel> data = dataItemCache.GetDataItemList("TaskWorkType");
            foreach (DataItemModel item in data)
            {
                TreeEntity tree = new TreeEntity();
                bool hasChildren = data.Count(t => t.ParentId == item.ItemDetailId) == 0 ? false : true;
                bool showcheck = data.Count(t => t.ParentId == item.ItemDetailId) == 0 ? true : false;
                tree.id = item.ItemDetailId;
                tree.text = item.ItemName;
                tree.value = item.ItemValue;
                tree.isexpand = true;
                tree.complete = true;
                if (!string.IsNullOrEmpty(typeIDs))
                {
                    var s = typeIDs.Split(',');
                    foreach (var arr in s)
                    {
                        if (arr == item.ItemValue) tree.checkstate = 1;
                    }
                }
                tree.showcheck = showcheck;
                tree.hasChildren = hasChildren;
                tree.parentId = item.ParentId;
                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson());
        }
        #endregion

        #region  �ԽӰ���(��ʱִ��)
        public void SendTaskInfo()
        {
            var tasklist = supervisetaskbll.GetList(string.Format(" and tasklevel='0' and SuperviseState='2' and TaskWorkStartTime<=to_date('{1}','yyyy-mm-dd hh24:mi:ss') and  TaskWorkEndTime>=to_date('{0}','yyyy-mm-dd hh24:mi:ss')", DateTime.Now.ToString("yyyy-MM-dd 00:00:00"), DateTime.Now.ToString("yyyy-MM-dd 23:59:59")));
            List<object> datas = new List<object>();

            List<SuperviseTaskModel> listmodel = new List<SuperviseTaskModel>();
            foreach (SuperviseTaskEntity item in tasklist)
            {
                SuperviseTaskModel model = new SuperviseTaskModel();
                model.TaskLevel = "1";//����
                model.TimeLong = item.TimeLong;
                model.OrganizeManager = item.OrganizeManager;
                model.SuperviseCode = item.SuperviseCode;
                model.SuperviseState = item.SuperviseState;
                model.TaskWorkEndTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
                model.TimeLongStr = item.TimeLongStr;
                model.TaskWorkTypeId = item.TaskWorkTypeId;
                model.TaskUserName = "";
                model.TaskWorkType = item.TaskWorkType;
                model.TaskWorkStartTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
                model.TaskUserId = "";
                model.RiskAnalyse = item.RiskAnalyse;
                model.SafetyMeasure = item.SafetyMeasure;
                model.TaskBill = item.TaskBill;
                model.RiskAnalyse = item.RiskAnalyse;
                model.SuperParentId = item.Id;
                model.ConstructLayout = item.ConstructLayout;
                model.STeamId = item.STeamId;
                model.STeamCode = item.STeamCode;
                model.STeamName = item.STeamName;
                model.HandType = item.HandType;
                model.CreateUserId = item.CreateUserId;
                model.CreateUserName = item.CreateUserName;
                model.CreateUserDeptCode = item.CreateUserDeptCode;
                model.CreateUserOrgCode = item.CreateUserOrgCode;
                model.WorkSpecs = new SuperviseWorkInfoBLL().GetList(item.Id).OrderBy(t => t.CreateDate).ToList();
                string objectId = supervisetaskbll.SaveForm("", model);
                listmodel.Add(model);

                var type = "";
                if (!string.IsNullOrEmpty(item.TaskWorkType) && !string.IsNullOrEmpty(item.HandType))
                {
                    type = item.TaskWorkType + "," + item.HandType;
                }
                else
                {
                    if (!string.IsNullOrEmpty(item.TaskWorkType))
                    {
                        type = item.TaskWorkType;
                    }
                    if (!string.IsNullOrEmpty(item.HandType))
                    {
                        type = item.HandType;
                    }
                }

                var content = "";//��ҵ��Ŀ
                var dept = "";//��ҵ��λ
                var ename = "";//��������
                var wplace = "";//�����ص�
                foreach (var ws in model.WorkSpecs)
                {
                    if (!string.IsNullOrEmpty(ws.WorkContent))
                        content += ws.WorkContent + ",";
                    if (!string.IsNullOrEmpty(ws.WorkDeptName))
                        dept += ws.WorkDeptName + ",";
                    if (!string.IsNullOrEmpty(ws.EngineeringName))
                        ename += ws.EngineeringName + ",";
                    if (!string.IsNullOrEmpty(ws.WorkPlace))
                        wplace += ws.WorkPlace + ",";
                }
                if (!string.IsNullOrEmpty(content))
                    content.TrimEnd(',');
                if (!string.IsNullOrEmpty(dept))
                    dept.TrimEnd(',');
                if (!string.IsNullOrEmpty(ename))
                    ename.TrimEnd(',');
                if (!string.IsNullOrEmpty(wplace))
                    wplace.TrimEnd(',');

                var tempdata = new
                {
                    Job = ename + "��վ�ල����",
                    //JobUsers = item.TaskUserId,
                    StartTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")),
                    EndTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59")),
                    JobProject1 = content,
                    JobDept = dept,
                    JobCategory = type,
                    JobProject2 = ename,
                    JobNo = item.TaskBill,
                    JobAddr = wplace,
                    RecId = objectId,
                    GroupId = item.STeamId
                };
                datas.Add(tempdata);
            }


            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            //��������web api����ȡ����ֵ��Ĭ��Ϊpost��ʽ
            try
            {
                System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                nc.Add("json", Newtonsoft.Json.JsonConvert.SerializeObject(datas));
                wc.UploadValuesCompleted += wc_UploadValuesCompleted1;
                wc.UploadValuesAsync(new Uri(new DataItemDetailBLL().GetItemValue("bzurl") + "PostMonitorJob"), nc);

            }
            catch (Exception ex)
            {
                //��ͬ�����д����־�ļ�
                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(new DataItemDetailBLL().GetItemValue("imgPath") + "/logs/" + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "������ʧ��" + ",�쳣��Ϣ��" + ex.Message + "\r\n");
            }
        }

        void wc_UploadValuesCompleted1(object sender, UploadValuesCompletedEventArgs e)
        {
            //��ͬ�����д����־�ļ�
            string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
            System.IO.File.AppendAllText(new DataItemDetailBLL().GetItemValue("imgPath") + "/logs/" + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "��" + System.Text.Encoding.UTF8.GetString(e.Result) + "\r\n");

        }
        #endregion
    }
}
