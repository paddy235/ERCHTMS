using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Busines.HighRiskWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using System;
using Newtonsoft.Json;
using ERCHTMS.Busines.SystemManage;
using System.Data;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using ERCHTMS.Busines.BaseManage;
using System.Linq;
using ERCHTMS.Busines.LllegalManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Web.Areas.HiddenTroubleManage.Controllers;

namespace ERCHTMS.Web.Areas.HighRiskWork.Controllers
{
    /// <summary>
    /// �� ������������
    /// </summary>
    public class TaskShareController : MvcControllerBase
    {
        private TaskShareBLL tasksharebll = new TaskShareBLL();
        private TeamsInfoBLL teamsinfobll = new TeamsInfoBLL();
        private StaffInfoBLL staffinfobll = new StaffInfoBLL();

        #region ��ͼ����
        /// <summary>
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.IsTeams = new DataItemDetailBLL().GetItemValue("�Ƿ�������ʱ����");
            return View();
        }
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FormOne()
        {
            ViewBag.IsTeams = new DataItemDetailBLL().GetItemValue("�Ƿ�������ʱ����");
            return View();
        }

        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FormTwo()
        {
            ViewBag.IsTeams = new DataItemDetailBLL().GetItemValue("�Ƿ�������ʱ����");
            return View();
        }

        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FormThree()
        {
            return View();
        }


        /// <summary>
        /// ��վ�ලͳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Statistics()
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
            var data = tasksharebll.GetList(queryJson);
            return ToJsonResult(data);
        }


        /// <summary>
        /// ��ȡ���������б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDataTableJson(Pagination pagination, string queryJson)
        {

            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.conditionJson = " 1=1 ";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                var data = tasksharebll.GetDataTable(pagination, queryJson, authType);
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
                return Error(ex.Message);
            }
        }
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = tasksharebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }


        /// <summary>
        /// �ල���������Ա�ͼ
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QueryHidNumberComparison(string queryJson)
        {
            try
            {
                var queryParam = queryJson.ToJObject();

                string deptCode = queryParam["deptCode"].ToString();  //����
                string startDate = queryJson.Contains("startDate") ? queryParam["startDate"].ToString() : "";  //��ʼ����
                string endDate = queryJson.Contains("endDate") ? queryParam["endDate"].ToString() : "";  //��ֹ����
                var curUser = new OperatorProvider().Current(); //��ǰ�û�

                StatisticsEntity hentity = new StatisticsEntity();
                hentity.sDeptCode = string.IsNullOrEmpty(deptCode) ? curUser.DeptCode : deptCode;
                hentity.startDate = startDate;
                hentity.endDate = endDate;
                hentity.sAction = "1";   //�Ա�ͼ����
                hentity.sMark = 0;
                
                //��ǰ�û��ǳ���
                if (curUser.RoleName.Contains("����") || curUser.RoleName.Contains("��˾��"))
                {
                    hentity.isCompany = true;
                }
                else
                {
                    hentity.isCompany = false;
                }
                //�б�
                var dt = tasksharebll.QueryStatisticsByAction(hentity);

                //x ��Title 
                List<dseries> xdata = new List<dseries>();

                //x ��Title 
                List<dseries> sdata = new List<dseries>();
                //δ�ල
                List<dseries_child> yblist = new List<dseries_child>();
                //�Ѽල
                List<dseries_child> zdlist = new List<dseries_child>();

                dseries s1 = new dseries();
                s1.name = "��ල";
                s1.id = "ybyh";
                dseries s2 = new dseries();
                s2.name = "�Ѽල";
                s2.id = "zdyh";
                //ͼ�����
                foreach (DataRow row in dt.Rows)
                {
                    string dname = row["fullname"].ToString();
                    string drillId = row["createuserdeptcode"].ToString();
                    //��ල
                    dseries_child ybyh = new dseries_child();
                    ybyh.name = dname;
                    ybyh.y = Convert.ToInt32(row["total"].ToString());
                    ybyh.drilldown = "yb" + drillId;//���ű���
                    yblist.Add(ybyh);

                    //�Ѽල
                    dseries_child zdyh = new dseries_child();
                    zdyh.name = row["fullname"].ToString();
                    zdyh.y = Convert.ToInt32(row["ImportanHid"].ToString());
                    zdyh.drilldown = "zd" + drillId;//���ű���
                    zdlist.Add(zdyh);

                    //��ȡ�����Ż���������
                    List<dseries_child> cyblist = new List<dseries_child>();
                    List<dseries_child> czdlist = new List<dseries_child>();
                    hentity.sDeptCode = row["createuserdeptcode"].ToString();
                    hentity.sHidRank = "��ල,�Ѽල";
                    hentity.sMark = 1;
                    var yhdt = tasksharebll.QueryStatisticsByAction(hentity);
                    foreach (DataRow crow in yhdt.Rows)
                    {
                        //��ල
                        dseries_child cybmodel = new dseries_child();
                        cybmodel.name = crow["fullname"].ToString();
                        cybmodel.y = Convert.ToInt32(crow["total"].ToString());
                        cybmodel.drilldown = "next_yb_" + crow["pteamcode"].ToString(); ;//���ű���
                        cyblist.Add(cybmodel);

                        //�Ѽල
                        dseries_child czdmodel = new dseries_child();
                        czdmodel.name = crow["fullname"].ToString();
                        czdmodel.y = Convert.ToInt32(crow["ImportanHid"].ToString());
                        czdmodel.drilldown = "next_zd_" + crow["pteamcode"].ToString(); ;//���ű���
                        czdlist.Add(czdmodel);
                    }
                    //��ල����Ŀ
                    dseries cybdseries = new dseries();
                    cybdseries.name = "��ල";
                    cybdseries.id = "yb" + drillId;
                    cybdseries.data = cyblist;
                    sdata.Add(cybdseries);


                    //�Ѽල����Ŀ
                    dseries czddseries = new dseries();
                    czddseries.name = "�Ѽල";
                    czddseries.id = "zd" + drillId;
                    czddseries.data = czdlist;
                    sdata.Add(czddseries);
                }
                s1.data = yblist; //��ල
                xdata.Add(s1);
                s2.data = zdlist; //�Ѽල
                xdata.Add(s2);
                //�������
                var jsonData = new { tdata = dt, xdata = xdata, sdata = sdata, iscompany = hentity.isCompany ? 1 : 0 };

                return Content(jsonData.ToJson());
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        /// <summary>
        /// �ල�����������
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QueryHidNUmberComparisonList(string queryJson)
        {
            try
            {
                var queryParam = queryJson.ToJObject();

                string deptCode = queryParam["deptCode"].ToString();  //����
                string startDate = queryJson.Contains("startDate") ? queryParam["startDate"].ToString() : "";  //��ʼ����
                string endDate = queryJson.Contains("endDate") ? queryParam["endDate"].ToString() : "";  //��ֹ����
                var curUser = new OperatorProvider().Current(); //��ǰ�û�

                StatisticsEntity hentity = new StatisticsEntity();
                hentity.sDeptCode = string.IsNullOrEmpty(deptCode) ? curUser.DeptCode : deptCode;
                hentity.startDate = startDate;
                hentity.endDate = endDate;
                hentity.sAction = "1";   //�Ա�ͼ����
                hentity.sMark = 2;
                //��ǰ�û��ǳ���
                if (curUser.RoleName.Contains("����") || curUser.RoleName.Contains("��˾��"))
                {
                    hentity.isCompany = true;
                }
                else
                {
                    hentity.isCompany = false;
                }
                var treeList = new List<TreeGridEntity>();
                //�б�
                var dt = tasksharebll.QueryStatisticsByAction(hentity);

                foreach (DataRow row in dt.Rows)
                {
                    TreeListForHidden tentity = new TreeListForHidden();
                    tentity.createuserdeptcode = row["createuserdeptcode"].ToString();
                    tentity.fullname = row["fullname"].ToString();
                    tentity.sortcode = row["sortcode"].ToString();
                    tentity.departmentid = row["departmentid"].ToString();
                    if (row["parentid"].ToString() != "0")
                    {
                        tentity.parent = row["parentid"].ToString();
                    }
                    tentity.importanhid = Convert.ToDecimal(row["importanhid"].ToString());
                    tentity.ordinaryhid = Convert.ToDecimal(row["ordinaryhid"].ToString());
                    tentity.total = Convert.ToDecimal(row["total"].ToString());
                    TreeGridEntity tree = new TreeGridEntity();
                    bool hasChildren = dt.Select(string.Format(" parentid ='{0}'", tentity.departmentid)).Count() == 0 ? false : true;
                    tentity.haschild = hasChildren;
                    tree.id = row["departmentid"].ToString();
                    tree.parentId = row["parentid"].ToString();
                    string itemJson = tentity.ToJson();
                    tree.entityJson = itemJson;
                    tree.expanded = false;
                    tree.hasChildren = hasChildren;
                    treeList.Add(tree);
                }

                //�������
                return Content(treeList.TreeJson("0"));
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        /// <summary>
        /// ��վ��������Ա�ͼ
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QuerySupervisonNumberComparison(string queryJson)
        {
            try
            {
                var queryParam = queryJson.ToJObject();

                string deptCode = queryParam["deptCode"].ToString();  //����
                string startDate = queryJson.Contains("startDate") ? queryParam["startDate"].ToString() : "";  //��ʼ����
                string endDate = queryJson.Contains("endDate") ? queryParam["endDate"].ToString() : "";  //��ֹ����
                var curUser = new OperatorProvider().Current(); //��ǰ�û�

                StatisticsEntity hentity = new StatisticsEntity();
                hentity.sDeptCode = string.IsNullOrEmpty(deptCode) ? curUser.DeptCode : deptCode;
                hentity.startDate = startDate;
                hentity.endDate = endDate;
                hentity.sAction = "2";   //�Ա�ͼ����
                hentity.sMark = 0;

                //��ǰ�û��ǳ���
                if (curUser.RoleName.Contains("����") || curUser.RoleName.Contains("��˾��"))
                {
                    hentity.isCompany = true;
                }
                else
                {
                    hentity.isCompany = false;
                }
                //�б�
                var dt = tasksharebll.QueryStatisticsByAction(hentity);

                //x ��Title 
                List<dseries> xdata = new List<dseries>();

                //x ��Title 
                List<dseries> sdata = new List<dseries>();
                //δ�ල
                List<dseries_child> yblist = new List<dseries_child>();
                //�Ѽල
                List<dseries_child> zdlist = new List<dseries_child>();

                dseries s1 = new dseries();
                s1.name = "����";
                s1.id = "ybyh";
                dseries s2 = new dseries();
                s2.name = "�Ѽ��";
                s2.id = "zdyh";
                //ͼ�����
                foreach (DataRow row in dt.Rows)
                {
                    string dname = row["fullname"].ToString();
                    string drillId = row["createuserdeptcode"].ToString();
                    //����
                    dseries_child ybyh = new dseries_child();
                    ybyh.name = dname;
                    ybyh.y = Convert.ToInt32(row["ordinaryhid"].ToString());
                    ybyh.drilldown = "yb" + drillId;//���ű���
                    yblist.Add(ybyh);

                    //�Ѽ��
                    dseries_child zdyh = new dseries_child();
                    zdyh.name = row["fullname"].ToString();
                    zdyh.y = Convert.ToInt32(row["ImportanHid"].ToString());
                    zdyh.drilldown = "zd" + drillId;//���ű���
                    zdlist.Add(zdyh);
                }
                s1.data = yblist; //����
                xdata.Add(s1);
                s2.data = zdlist; //�Ѽ��
                xdata.Add(s2);
                //�������
                var jsonData = new { tdata = dt, xdata = xdata, sdata = sdata, iscompany = hentity.isCompany ? 1 : 0 };

                return Content(jsonData.ToJson());
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// ��������������
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QuerySupervisonNumberComparisonList(string queryJson)
        {
            try
            {
                var queryParam = queryJson.ToJObject();

                string deptCode = queryParam["deptCode"].ToString();  //����
                string startDate = queryJson.Contains("startDate") ? queryParam["startDate"].ToString() : "";  //��ʼ����
                string endDate = queryJson.Contains("endDate") ? queryParam["endDate"].ToString() : "";  //��ֹ����
                var curUser = new OperatorProvider().Current(); //��ǰ�û�

                StatisticsEntity hentity = new StatisticsEntity();
                hentity.sDeptCode = string.IsNullOrEmpty(deptCode) ? curUser.DeptCode : deptCode;
                hentity.startDate = startDate;
                hentity.endDate = endDate;
                hentity.sAction = "2";   //�Ա�ͼ����
                hentity.sMark = 0;
                //��ǰ�û��ǳ���
                if (curUser.RoleName.Contains("����") || curUser.RoleName.Contains("��˾��"))
                {
                    hentity.isCompany = true;
                }
                else
                {
                    hentity.isCompany = false;
                }
                var treeList = new List<TreeGridEntity>();
                //�б�
                var dt = tasksharebll.QueryStatisticsByAction(hentity);

                //�������
                return ToJsonResult(dt);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        
        /// <summary>
        /// ����ͳ���б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult ExportStatisticExcel(string queryJson)
        {
            try
            {
                var queryParam = queryJson.ToJObject();

                string deptCode = queryParam["deptCode"].ToString();  //����
                string startDate = queryJson.Contains("startDate") ? queryParam["startDate"].ToString() : "";  //��ʼ����
                string endDate = queryJson.Contains("endDate") ? queryParam["endDate"].ToString() : "";  //��ֹ����
                string stype = queryJson.Contains("stype") ? queryParam["stype"].ToString() : ""; //��������
                var curUser = new OperatorProvider().Current(); //��ǰ�û�

                StatisticsEntity hentity = new StatisticsEntity();
                hentity.sDeptCode = string.IsNullOrEmpty(deptCode) ? curUser.DeptCode : deptCode;
                hentity.startDate = startDate;
                hentity.endDate = endDate;
                if (stype == "���ͼ")
                {
                    hentity.sAction = "2";   //�Ա�ͼ����
                    hentity.sMark = 0;
                }
                else if (stype == "�ලͼ")
                {
                    hentity.sAction = "1";   //�Ա�ͼ����
                    hentity.sMark = 2;
                }
                
                //��ǰ�û��ǳ���
                if (curUser.RoleName.Contains("����") || curUser.RoleName.Contains("��˾��"))
                {
                    hentity.isCompany = true;
                }
                else
                {
                    hentity.isCompany = false;
                }
                var treeList = new List<TreeGridEntity>();
                //�б�
                var dt = tasksharebll.QueryStatisticsByAction(hentity);
                //���õ�����ʽ
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 16;
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                if (stype == "���ͼ")
                {
                    excelconfig.Title = "��վ���ͳ��";
                    excelconfig.FileName = "��վ���ͳ��.xls";
                    //�������Դ��˳�򱣳�һ��
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "fullname", ExcelColumn = "��������", Width = 40 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "ordinaryhid", ExcelColumn = "����", Width = 40 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "importanhid", ExcelColumn = "�Ѽ��", Width = 40 });
                }
                else if (stype == "�ලͼ")
                {
                    excelconfig.Title = "��վ�ලͳ��";
                    excelconfig.FileName = "��վ�ලͳ��.xls";
                    //�������Դ��˳�򱣳�һ��
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "fullname", ExcelColumn = "��������", Width = 40 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "importanhid", ExcelColumn = "�Ѽල", Width = 40 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "total", ExcelColumn = "��ල", Width = 40 });
                }
               
                //���õ�������
                ExcelHelper.ExcelDownload(dt, excelconfig);
            }
            catch (Exception ex)
            {

            }
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
            tasksharebll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult ManageDelForm(string keyValue)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var dept = new DepartmentBLL().GetEntity(keyValue);
            try
            {
                var taskshareid = "";
                var teamentity = teamsinfobll.GetEntity(keyValue);
                if (teamentity != null)
                {
                    taskshareid = teamentity.TaskShareId;
                    new TeamsInfoBLL().RemoveForm(keyValue);
                    var single = new
                    {
                        taskshareid = teamentity.TaskShareId,
                        teamid = teamentity.TeamId
                    };
                    List<StaffInfoEntity> slist = new StaffInfoBLL().GetList(JsonConvert.SerializeObject(single)).ToList();
                    foreach (var item in slist)
                    {
                        staffinfobll.RemoveForm(item.Id);
                    }
                    //д����־�ļ�
                    string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                    System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "��" + user.UserName + "ɾ��" + dept.FullName + "��������ɹ����û���Ϣ" + Newtonsoft.Json.JsonConvert.SerializeObject(user) + ",������Ϣ:" + Newtonsoft.Json.JsonConvert.SerializeObject(teamentity) + ",��Ա����:" + Newtonsoft.Json.JsonConvert.SerializeObject(slist.Where(t => t.TaskLevel == "1")) + "\r\n");
                }
            }
            catch (Exception ex)
            {
                //д����־�ļ�
                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "��" + user.UserName + "ɾ��" + dept.FullName + "��������ʧ�ܣ��û���Ϣ" + Newtonsoft.Json.JsonConvert.SerializeObject(user) + ",�쳣��Ϣ��" + ex.Message + "\r\n");
                return Success("����ʧ�ܣ�������Ϣ��" + ex.Message);
            }
            return Success("ɾ���ɹ���");
        }

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult ManageRemoveForm(string keyValue)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            try
            {
                tasksharebll.RemoveForm(keyValue);
                var single = new
                {
                    taskshareid = keyValue,
                    tasklevel = 1
                };
                List<StaffInfoEntity> slist = new StaffInfoBLL().GetList(JsonConvert.SerializeObject(single)).ToList();
                //д����־�ļ�
                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "��" + user.UserName + "��������ɾ���ɹ����û���Ϣ" + Newtonsoft.Json.JsonConvert.SerializeObject(user) + "��Ա����:" + Newtonsoft.Json.JsonConvert.SerializeObject(slist) + "\r\n");
            }
            catch (Exception ex)
            {
                //д����־�ļ�
                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "��" + user.UserName + "��������ɾ��ʧ�ܣ��û���Ϣ" + Newtonsoft.Json.JsonConvert.SerializeObject(user) + ",�쳣��Ϣ��" + ex.Message + "\r\n");
                return Success("����ʧ�ܣ�������Ϣ��" + ex.Message);
            }
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
        public ActionResult SaveForm(string keyValue, string jsonData)
        {
            TaskShareEntity model = JsonConvert.DeserializeObject<TaskShareEntity>(jsonData);
            tasksharebll.SaveForm(keyValue, model);
            return Success("�����ɹ���");
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult FinishTask(string keyValue)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            try
            {

                TaskShareEntity u = tasksharebll.GetEntity(keyValue);
                if (u != null)
                {
                    u.FlowDept = "";
                    u.FlowRoleName = "";
                    u.FlowStep = "3";
                    tasksharebll.SaveOnlyShare(keyValue, u);
                    //����˾����Ա�����������д����־�ļ�
                    string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                    System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "��" + user.UserName + "����������ɳɹ����û���Ϣ" + Newtonsoft.Json.JsonConvert.SerializeObject(user) + "\r\n");
                }
            }
            catch (Exception ex)
            {
                //����˾����Ա�����������д����־�ļ�
                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "�������������ʧ�ܣ��û���Ϣ" + Newtonsoft.Json.JsonConvert.SerializeObject(user) + ",�쳣��Ϣ��" + ex.Message + "\r\n");
                return Success("����ʧ�ܣ�������Ϣ��" + ex.Message);
            }
            return Success("�����ɹ�");
        }
        #endregion

        #region ����
        /// <summary>
        /// �����б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult ExportData(string queryJson)
        {
            try
            {
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.sidx = "a.createdate";//�����ֶ�
                pagination.sord = "desc";//����ʽ
                pagination.conditionJson = " 1=1 ";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                DataTable data = tasksharebll.GetDataTable(pagination, queryJson, authType);
                DataTable excelTable = new DataTable();
                excelTable.Columns.Add(new DataColumn("tasktype"));
                excelTable.Columns.Add(new DataColumn("fullname"));
                excelTable.Columns.Add(new DataColumn("createusername"));
                excelTable.Columns.Add(new DataColumn("createdate"));
                excelTable.Columns.Add(new DataColumn("flowdeptname"));
                excelTable.Columns.Add(new DataColumn("flowstep"));
                foreach (DataRow item in data.Rows)
                {
                    DataRow newDr = excelTable.NewRow();
                    var tasktype = item["tasktype"].ToString();
                    if (tasktype == "0")
                    {
                        tasktype = "��������";
                    }
                    else if (tasktype == "1")
                    {
                        tasktype = "��������";
                    }
                    else if (tasktype == "2")
                    {
                        tasktype = "��Ա����";
                    }
                    newDr["tasktype"] = tasktype;
                    newDr["fullname"] = item["fullname"];
                    newDr["createusername"] = item["createusername"];
                    DateTime createdate;
                    DateTime.TryParse(item["createdate"].ToString(), out createdate);
                    newDr["createdate"] = createdate.ToString("yyyy-MM-dd");
                    newDr["flowdeptname"] = item["flowdeptname"];
                    var flowstep = item["flowstep"].ToString();
                    switch (flowstep)
                    {
                        case "0":
                            flowstep = "����������";
                            break;
                        case "1":
                            flowstep = "���ŷ�����";
                            break;
                        case "2":
                            if (item["tasktype"].ToString() != "2")
                            {
                                flowstep = "���������";
                            }
                            else
                            {
                                flowstep = "������";
                            }
                            break;
                        case "3":
                            flowstep = "�������";
                            break;
                    }
                    newDr["flowstep"] = flowstep;
                    excelTable.Rows.Add(newDr);
                }
                //���õ�����ʽ
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "��վ�ල��Ϣ";
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "��վ�ල��Ϣ.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //�������Դ��˳�򱣳�һ��
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "tasktype", ExcelColumn = "��������", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "fullname", ExcelColumn = "������λ", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createusername", ExcelColumn = "������", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createdate", ExcelColumn = "����ʱ��", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "flowdeptname", ExcelColumn = "���䲿��", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "flowstep", ExcelColumn = "�������", Width = 40 });
                //���õ�������
                ExcelHelper.ExcelDownload(excelTable, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("�����ɹ���");
        }
        #endregion
    }
}
