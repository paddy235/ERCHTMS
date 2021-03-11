using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.Busines.EmergencyPlatform;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using System.Collections.Generic;
using BSFramework.Util.Offices;
using System.Linq;
using ERCHTMS.Busines.BaseManage;
using System.Data;
using System.Web;
using System;
using ERCHTMS.Cache;
using System.Linq.Expressions;
using System.Text;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Web.Areas.EmergencyPlatform.Controllers
{
    /// <summary>
    /// �� ����Ӧ������
    /// </summary>
    public class DrillplanController : MvcControllerBase
    {
        private TeamBLL teambll = new TeamBLL();
        private UserBLL userBLL = new UserBLL();
        private PostBLL postBLL = new PostBLL();
        private DepartmentBLL departBLL = new DepartmentBLL();
        private DrillplanBLL drillplanbll = new DrillplanBLL();
        private DrillplanrecordBLL drillplanrecordbll = new DrillplanrecordBLL();
        private DataItemCache dataItemCache = new DataItemCache();

        #region ��ͼ����
        [HttpGet]
        public ActionResult Select()
        {
            return View();
        }



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
        /// ����
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Import()
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
            queryJson = queryJson ?? "";
            pagination.p_kid = "ID";
            pagination.p_fields = @"createuserid,createuserdeptcode,createuserorgcode, DEPARTID, DEPARTNAME,DRILLTYPE,DRILLMODE,to_char(PLANTIME,'yyyy-MM') as PLANTIME,
            DRILLTYPENAME,DRILLMODENAME,NAME,RPLANID,orgdeptid,orgdept,orgdeptcode,(select count(1) from MAE_DRILLPLANRECORD d where d.DRILLPLANId=t.id) recordnum,executepersonname,executepersonid,DrillCost";
            pagination.p_tablename = "mae_drillplan t";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "orgdeptcode", "CREATEUSERORGCODE");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }

            }


            var watch = CommonHelper.TimerStart();
            var data = drillplanbll.GetPageList(pagination, queryJson);
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
            var data = drillplanbll.GetList(queryJson);
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
            var data = drillplanbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region �ύ����

        /// <summary>
        /// �����û�
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportDriplan(string PostId)
        {
            var user = OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                //return "��������Ա�޴˲���Ȩ��";
            }
            List<UserEntity> ulist = userBLL.GetList().ToList();
            string orgId = user.OrganizeId;//������˾
            string deptId = PostId;//��������
            int error = 0;
            string message = "��ѡ���ʽ��ȷ���ļ��ٵ���!";
            string falseMessage = "";
            int count = HttpContext.Request.Files.Count;
            try
            {
                if (count > 0)
                {
                    HttpPostedFileBase file = HttpContext.Request.Files[0];
                    if (string.IsNullOrEmpty(file.FileName))
                    {
                        return message;
                    }
                    if (!(file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xls") || file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xlsx")))
                    {
                        return message;
                    }
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName));
                    DataTable dt = ExcelHelper.ExcelImport(Server.MapPath("~/Resource/temp/" + fileName));
                    int order = 1;
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        order = i;
                        //---****ֵ���ڿ���֤*****--
                        if (string.IsNullOrEmpty(dt.Rows[i][0].ToString()) ||
                            string.IsNullOrEmpty(dt.Rows[i][1].ToString()) ||
                            string.IsNullOrEmpty(dt.Rows[i][2].ToString()) ||
                            string.IsNullOrEmpty(dt.Rows[i][3].ToString()) ||
                            string.IsNullOrEmpty(dt.Rows[i][4].ToString()))
                        {
                            falseMessage += "</br>" + "��" + (i + 2) + "��ֵ���ڿ�,δ�ܵ���.";
                            error++;
                            continue;
                        }
                        //��֯����
                        string orgdept = dt.Rows[i][0].ToString();
                        var orgDepart = departBLL.GetList().Where(e => e.FullName == orgdept && e.OrganizeId == orgId).FirstOrDefault();
                        if (orgDepart == null)
                        {
                            falseMessage += "��" + i + "�е���ʧ��,����֯���Ų����ڣ�</br>";
                            error++;
                            continue;
                        }
                        string deptName = string.Empty;
                        string yldeptId = string.Empty;
                        string departname = dt.Rows[i][1].ToString();
                        var deptList = departname.Split(',');
                        for (int k = 0; k < deptList.Length; k++)
                        {
                            var Depart = departBLL.GetList().Where(e => e.FullName == deptList[k].ToString()).FirstOrDefault();
                            if (Depart == null)
                            {
                                continue;
                            }
                            else
                            {
                                if (string.IsNullOrWhiteSpace(deptName))
                                {
                                    deptName += Depart.FullName + ",";
                                    yldeptId += Depart.DepartmentId + ",";
                                }
                                else
                                {
                                    if (!yldeptId.Contains(Depart.DepartmentId))
                                    {
                                        deptName += Depart.FullName + ",";
                                        yldeptId += Depart.DepartmentId + ",";
                                    }
                                }
                            }
                        }

                        if (string.IsNullOrWhiteSpace(deptName))
                        {
                            falseMessage += "��" + i + "�е���ʧ��,���������Ų����ڣ�</br>";
                            error++;
                            continue;
                        }
                        else
                        {
                            deptName = deptName.Substring(0, deptName.Length - 1);
                            yldeptId = yldeptId.Substring(0, yldeptId.Length - 1);
                        }
                        //����Ԥ������
                        string name = dt.Rows[i][2].ToString();
                        //����Ԥ������
                        string yzTypeName = dt.Rows[i][3].ToString();
                        var yzType = dataItemCache.GetDataItemList("MAE_DirllPlanType").Where(e => e.ItemName == yzTypeName).FirstOrDefault();
                        if (yzType == null)
                        {
                            falseMessage += "��" + i + "�е���ʧ��,������Ԥ�����Ͳ����ڣ�</br>";
                            error++;
                            continue;
                        }
                        //������ʽ
                        string yzModeName = dt.Rows[i][4].ToString();
                        var yzMode = dataItemCache.GetDataItemList("MAE_DirllMode").Where(e => e.ItemName == yzModeName).FirstOrDefault();
                        if (yzMode == null)
                        {
                            falseMessage += "��" + i + "�е���ʧ��,��������ʽ�����ڣ�</br>";
                            error++;
                            continue;
                        }
                        //�����ƻ�����
                        string yzDrillCost = dt.Rows[i][5].ToString();
                        decimal DrillCost;
                        if (!string.IsNullOrEmpty(yzDrillCost))
                        {
                            if (!decimal.TryParse(yzDrillCost, out DrillCost))
                            {
                                falseMessage += "��" + i + "�е���ʧ��,�����ƻ����ø�ʽ����ȷ��</br>";
                                error++;
                                continue;
                            }
                        }
                        //ִ����
                        string executepersonname = string.Empty;
                        string executepersonid = string.Empty;
                        string tempusername = dt.Rows[i][6].ToString();
                        if (!string.IsNullOrEmpty(tempusername))
                        {
                            var tpuserlist = ulist.Where(p => p.RealName == tempusername).ToList();
                            if (tpuserlist.Count() > 0)
                            {
                                executepersonname = tempusername;
                                executepersonid = tpuserlist.FirstOrDefault().UserId;
                            }
                        }
                        //�ƻ�ʱ��
                        DateTime PLANTIME = new DateTime();
                        try
                        {
                            if (!string.IsNullOrEmpty(dt.Rows[i][7].ToString()))
                            {
                                PLANTIME = DateTime.Parse(DateTime.Parse(dt.Rows[i][7].ToString()).ToString("yyyy-MM"));
                            }
                        }
                        catch
                        {
                            falseMessage += "</br>" + "��" + (i + 2) + "��ʱ������,δ�ܵ���.";
                            error++;
                            continue;
                        }
                        try
                        {
                            drillplanbll.SaveForm("", new DrillplanEntity
                            {
                                NAME = name,
                                OrgDeptId = orgDepart.DepartmentId,
                                OrgDept = orgDepart.FullName,
                                OrgDeptCode=orgDepart.EnCode,
                                DEPARTID = yldeptId,
                                DEPARTNAME = deptName,
                                DRILLTYPE = yzType.ItemValue,
                                DRILLTYPENAME = yzType.ItemName,
                                DRILLMODE = yzMode.ItemValue,
                                DRILLMODENAME = yzMode.ItemName,
                                DrillCost = yzDrillCost,
                                EXECUTEPERSONNAME = executepersonname,
                                EXECUTEPERSONID = executepersonid,
                                PLANTIME = PLANTIME
                            });
                        }
                        catch
                        {
                            error++;
                        }

                    }
                    count = dt.Rows.Count - 1;
                    message = "����" + count + "����¼,�ɹ�����" + (count - error) + "����ʧ��" + error + "��";
                    message += "</br>" + falseMessage;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return message;
        }

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
            drillplanbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, DrillplanEntity entity)
        {
            drillplanbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }

        [HttpPost]
        public ActionResult SaveListForm()
        {
            string data = Request["param"];
            var list = data.ToObject<List<DrillplanEntity>>();

            foreach (var item in list)
            {
                drillplanbll.SaveForm(item.ID, item);
            }

            return Success("�����ɹ���");
        }

        #endregion

        #region ���ݵ���
        /// <summary>
        /// �����û��б�
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "����Ӧ�������ƻ�")]
        public ActionResult ExportDrillplanList(string condition, string queryJson)
        {
            Pagination pagination = new Pagination();
            queryJson = queryJson ?? "";
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "ID";
            pagination.p_fields = "Orgdept,DEPARTNAME,NAME,DRILLTYPENAME,DRILLMODENAME,DrillCost,PLANTIME,EXECUTEPERSONNAME";
            pagination.p_tablename = "V_mae_drillplan t";
            pagination.conditionJson = "1=1";
            pagination.sidx = "createdate";//�����ֶ�
            pagination.sord = "desc";//����ʽ  
            #region Ȩ��У��
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
            var data = drillplanbll.GetPageList(pagination, queryJson);
            #endregion
            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "Ӧ�������ƻ�";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "Ӧ�������ƻ�.xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "orgdept", ExcelColumn = "��֯����" });
            listColumnEntity.Add(new ColumnEntity() { Column = "departname", ExcelColumn = "��������" });
            listColumnEntity.Add(new ColumnEntity() { Column = "name", ExcelColumn = "����Ԥ������", Width = 50 });
            listColumnEntity.Add(new ColumnEntity() { Column = "drilltypename", ExcelColumn = "����Ԥ������" });
            listColumnEntity.Add(new ColumnEntity() { Column = "drillmodename", ExcelColumn = "������ʽ" });
            listColumnEntity.Add(new ColumnEntity() { Column = "drillcost", ExcelColumn = "�����ƻ����ã�Ԫ��" });
            listColumnEntity.Add(new ColumnEntity() { Column = "executepersonname", ExcelColumn = "ִ����" });
            listColumnEntity.Add(new ColumnEntity() { Column = "plantime", ExcelColumn = "�ƻ�ʱ��" });
            excelconfig.ColumnEntity = listColumnEntity;

            //���õ�������
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("�����ɹ���");
        }
        #endregion

        #region ����ͳ��
        /// <summary>
        /// Ӧ�������ƻ������
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string DrillplanFinish()
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var year = int.Parse(Request["year"] ?? "0");
            var deptId = Request["deptId"] ?? "";
            var jd = int.Parse(Request["jd"] ?? "0");
            var monthCK = int.Parse(Request["month"] ?? "0");
            var type = int.Parse(Request["type"] ?? "0");
            var starttime = Request["starttime"] ?? "";
            var endtime = Request["endtime"] ?? "";
            var returnList = new List<Object>();

            //Ȩ��
            #region Ȩ��
            string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "CREATEUSERDEPTCODE", "CREATEUSERORGCODE");
            string sqlwhere = " and 1=1 ";
            if (!string.IsNullOrEmpty(where))
            {
                sqlwhere += " and " + where;
            }
            else
            {

                sqlwhere += string.Format(" and CREATEUSERDEPTCODE like '{0}%'", user.DeptCode);
            }
            #endregion
            string cwhere = string.Empty;
            string cyear = Request["year"] ?? "";
            string cdeptid = Request["deptId"] ?? "";
            string cmonth = Request["month"] ?? "";

            if (user.RoleName.Contains("��˾���û�") || user.RoleName.Contains("���������û�"))
            {
                cwhere += string.Format("  and  a.createuserdeptcode like '{0}%'", user.OrganizeCode);
            }
            else
            {
                cwhere += string.Format("  and  a.createuserdeptcode like '{0}%'", user.DeptCode);
            }

            if (!string.IsNullOrEmpty(cyear))
            {
                cwhere += string.Format(@" and to_char(a.drilltime,'yyyy') = '{0}' ", cyear);
            }
            if (!string.IsNullOrEmpty(cdeptid))
            {
                cwhere += string.Format(@" and a.departid  like  '%{0}%' ", cdeptid);
            }
            if (!string.IsNullOrEmpty(starttime))
            {
                cwhere += string.Format(@" and a.drilltime >=  to_date('{0}','yyyy-mm-dd hh24:mi:ss') ", starttime);
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                cwhere += string.Format(@" and a.drilltime  <=  to_date('{0}','yyyy-mm-dd hh24:mi:ss') ", endtime);
            }
            if (type == 0) //Ӧ������Ԥ������ͳ��
            {
                var seriesList = new List<Object>();
                var drilldownList = new List<Object>();

                // var stable = new List<Object>();
                var dt = drillplanrecordbll.GetDrillPlanRecordTypeSta(cwhere, 0);

                foreach (DataRow row in dt.Rows)
                {
                    // stable.Add(new { id =  row["itemname"].ToString() ,name = row["itemname"].ToString(), value = int.Parse(row["num"].ToString()) }); 
                    if (row["itemname"].ToString() == "�ֳ����÷���")
                    {
                        string twhere = cwhere + string.Format("  and a.drilltypename ='{0}' ", row["itemname"].ToString());

                        var drilldt = drillplanrecordbll.GetDrillPlanRecordTypeSta(twhere, 1);

                        var drilldowndata = new List<object>();

                        foreach (DataRow drow in drilldt.Rows)
                        {
                            // stable.Add(new { id = drow["itemname"].ToString(), name = drow["itemname"].ToString(), value = int.Parse(drow["num"].ToString()), parentid = row["itemname"].ToString() }); 
                            drilldowndata.Add(new { name = drow["itemname"].ToString(), y = int.Parse(drow["num"].ToString()), p = 0 });
                        }

                        drilldownList.Add(new { name = row["itemname"].ToString(), id = row["itemname"].ToString(), data = drilldowndata });

                        seriesList.Add(new { name = row["itemname"].ToString(), y = int.Parse(row["num"].ToString()), p = 0, drilldown = row["itemname"].ToString() });
                    }
                    else
                    {
                        seriesList.Add(new { name = row["itemname"].ToString(), y = int.Parse(row["num"].ToString()), p = 0 });
                    }
                }

                #region MyRegion
                //var treeList = new List<TreeGridEntity>();
                //foreach (DataRow row in dt.Rows)
                //{
                //    //TreeListForHidden tentity = new TreeListForHidden();
                //    //tentity.createuserdeptcode = row["createuserdeptcode"].ToString();
                //    //tentity.fullname = row["fullname"].ToString();
                //    //tentity.sortcode = row["sortcode"].ToString();
                //    //tentity.departmentid = row["departmentid"].ToString();
                //    //if (row["parentid"].ToString() != "0")
                //    //{
                //    //    tentity.parent = row["parentid"].ToString();
                //    //}
                //    //tentity.importanhid = Convert.ToDecimal(row["importanhid"].ToString());
                //    //tentity.ordinaryhid = Convert.ToDecimal(row["ordinaryhid"].ToString());
                //    //tentity.total = Convert.ToDecimal(row["total"].ToString());
                //    TreeGridEntity tree = new TreeGridEntity();
                //    bool hasChildren = dt.Select(string.Format(" parentid ='{0}'", tentity.departmentid)).Count() == 0 ? false : true;
                //    tentity.haschild = hasChildren;
                //    tree.id = row["departmentid"].ToString();
                //    tree.parentId = row["parentid"].ToString();
                //    string itemJson = tentity.ToJson();
                //    tree.entityJson = itemJson;
                //    tree.expanded = false;
                //    tree.hasChildren = hasChildren;
                //    treeList.Add(tree);
                //} 
                #endregion

                returnList.Add(new { seriesdata = seriesList, drilldowndata = drilldownList });
            }
            else if (type == 1) //�μ������˴�ͳ��
            {
                var dt = drillplanrecordbll.GetDrillPlanRecordTypeSta(cwhere, 2);
                foreach (DataRow row in dt.Rows)
                {
                    returnList.Add(new { name = row["itemname"].ToString(), y = int.Parse(row["num"].ToString()), p = 0 });
                }
            }
            else if (type == 5) //��ί����λ�ڲ�����
            {
                var bll = new ReserverplanBLL();
                var list = bll.GetList(sqlwhere).GroupBy(e => e.ORGXZNAME);
                foreach (var item in list)
                {
                    returnList.Add(new { text = item.Key, value = item.Count() });
                }
            }
            else
            {
                //�ƻ�������
                var list_JH = drillplanbll.GetList(sqlwhere).Where(e => e.PLANTIME.Value.Year == year && (deptId.Length > 0 ? e.DEPARTID == deptId : 1 == 1));
                DrillplanrecordBLL dbll = new DrillplanrecordBLL();
                //ͳ��ʵ�ʼƻ�����
                var list_SJ = dbll.GetList(sqlwhere).Where(e => e.DRILLTIME.Value.Year == year && (deptId.Length > 0 ? e.DEPARTID == deptId : 1 == 1));
                if (type == 2 || type == 3)
                {
                    #region  ����ͳ��
                    //��ʼͳ��,�ƻ�������
                    decimal[] jd_JH = { 0, 0, 0, 0 };
                    foreach (var item in list_JH)
                    {
                        if (deptId.Length > 0 && item.DEPARTID != deptId)
                            continue;
                        if (year > 0 && item.PLANTIME.Value.Year != year)
                            continue;
                        var month = item.PLANTIME.Value.Month;

                        if (month >= 1 && month <= 3)
                            jd_JH[0]++;

                        if (month >= 4 && month <= 6)
                            jd_JH[1]++;

                        if (month >= 7 && month <= 9)
                            jd_JH[2]++;

                        if (month >= 10 && month <= 12)
                            jd_JH[3]++;
                    }

                    //��ʼͳ�ƣ�ʵ�ʵ�����
                    decimal[] jd_SJ = { 0, 0, 0, 0 };
                    foreach (var item in list_SJ)
                    {
                        if (deptId.Length > 0 && item.DEPARTID != deptId)
                            continue;
                        if (year > 0 && item.DRILLTIME.Value.Year != year)
                            continue;
                        var month = item.DRILLTIME.Value.Month;

                        if (month >= 1 && month <= 3)
                            jd_SJ[0]++;

                        if (month >= 4 && month <= 6)
                            jd_SJ[1]++;

                        if (month >= 7 && month <= 9)
                            jd_SJ[2]++;

                        if (month >= 10 && month <= 12)
                            jd_SJ[3]++;

                    }
                    List<string> xValues = new List<string>();

                    if (jd == 0)
                    {

                        for (int i = 0; i < 4; i++)
                        {
                            xValues.Add("��" + (i + 1) + "����");
                        }
                        for (int i = 0; i < xValues.Count; i++)
                        {

                            var jhNum = 0M;
                            var value = 0M;

                            if (jd_JH[i] == 0)
                            {
                                jhNum = 0;
                                value = 0;
                            }
                            else
                            {
                                jhNum = jd_JH[i];
                                value = (jd_SJ[i] / jd_JH[i]) * 100;
                            }
                            var finish = new { text = xValues[i], jhNum = jhNum, value = decimal.Round(value, 2), sjNum = jd_SJ[i] };
                            returnList.Add(finish);
                        }
                    }
                    else
                    {

                        var jhNum = 0M;
                        var value = 0M;

                        if (jd_JH[jd - 1] == 0)
                        {
                            jhNum = 0;
                            value = 0;
                        }
                        else
                        {
                            jhNum = jd_JH[jd - 1];
                            value = (jd_SJ[jd - 1] / jd_JH[jd - 1]) * 100;
                        }
                        var finish = new { text = "��" + jd + "����", jhNum = jhNum, value = decimal.Round(value, 2), sjNum = jd_SJ[jd - 1] };
                        returnList.Add(finish);
                    }
                    #endregion
                }
                if (type == 4)
                {
                    #region ��ʽͳ��

                    ////�ж��·�
                    var drillmode = dataItemCache.GetDataItemList("MAE_DirllMode");
                    foreach (var item in drillmode)
                    {
                        var dpf = new { text = item.ItemName, jhNum = list_JH.Where(e => (year > 0 ? e.PLANTIME.Value.Year == year : 1 == 1) && (monthCK > 0 ? e.PLANTIME.Value.Month == monthCK : 1 == 1) && e.DRILLMODENAME == item.ItemName).Count(), sjNum = list_SJ.Where(e => (year > 0 ? e.DRILLTIME.Value.Year == year : 1 == 1) && (monthCK > 0 ? e.DRILLTIME.Value.Month == monthCK : 1 == 1) && e.DRILLMODENAME == item.ItemName).Count() };
                        returnList.Add(dpf);
                    }
                    #endregion
                }
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(returnList);
        }

        [HttpPost]
        public ActionResult DrillplanStat()
        {
            var curUser = new OperatorProvider().Current(); //��ǰ�û�

            var deptCode = Request["deptCode"] ?? "";

            var starttime = Request["starttime"] ?? "";
            var endtime = Request["endtime"] ?? "";
            var isCompany = false;
            //��ǰ�û��ǳ���
            if (curUser.RoleName.Contains("����") || curUser.RoleName.Contains("��˾��"))
            {
                isCompany = true;
            }
            else
            {
                isCompany = false;
            }
            deptCode = deptCode == "" ? curUser.DeptCode : deptCode;
            //x ��Title 
            List<dseries> xdata = new List<dseries>();

            //x ��Title 
            List<dseries> sdata = new List<dseries>();

            var DirllMode = new DataItemDetailBLL().GetDataItemListByItemCode("'MAE_DirllMode'").ToList();
            for (int i = 0; i < DirllMode.Count; i++)
            {
                var dt = drillplanrecordbll.DrillplanStat(DirllMode[i].ItemName, isCompany, deptCode, starttime, endtime);

                List<dseries_child> mode = new List<dseries_child>();

                dseries s1 = new dseries();

                s1.name = DirllMode[i].ItemName;
                s1.id = DirllMode[i].ItemValue;
                //ͼ�����
                foreach (DataRow row in dt.Rows)
                {
                    dseries_child ybyh = new dseries_child();
                    ybyh.name = row["fullname"].ToString();
                    ybyh.y = Convert.ToInt32(row["recordnum"].ToString());
                    ybyh.drilldown = DirllMode[i].ItemDetailId + row["encode"].ToString();//���ű���
                    mode.Add(ybyh);
                    List<dseries_child> cyblist = new List<dseries_child>();
                    var cdeptCode = row["encode"].ToString();
                    var dept = new DepartmentBLL().GetEntityByCode(cdeptCode);
                    if (dept != null)
                    {
                        if (dept.Nature == "����")
                        {
                            continue;
                        }
                    }
                    var dtChild = drillplanrecordbll.DrillplanStatDetail(DirllMode[i].ItemName, false, cdeptCode, "", "");
                    foreach (DataRow childRow in dtChild.Rows)
                    {
                        dseries_child cybmodel = new dseries_child();
                        cybmodel.name = childRow["fullname"].ToString();
                        cybmodel.y = Convert.ToInt32(childRow["recordnum"].ToString());
                        cyblist.Add(cybmodel);
                    }
                    dseries cybdseries = new dseries();
                    cybdseries.name = DirllMode[i].ItemName;
                    cybdseries.id = DirllMode[i].ItemDetailId + row["encode"].ToString();
                    cybdseries.data = cyblist;
                    sdata.Add(cybdseries);
                }
                s1.data = mode;
                xdata.Add(s1);
            }
            var jsonData = new { xdata = xdata, sdata = sdata };

            return Content(jsonData.ToJson());
        }


        public ActionResult DrillplanStatList(string queryJson)
        {
            var curUser = new OperatorProvider().Current(); //��ǰ�û�
            var queryParam = queryJson.ToJObject();

            string startTime = queryJson.Contains("startTime") ? queryParam["startTime"].ToString() : "";  //��ʼ���� 
            string endTime = queryJson.Contains("endTime") ? queryParam["endTime"].ToString() : "";//��ֹ����
            var isCompany = false;
            var code = string.Empty;
            //��ǰ�û��ǳ���
            if (curUser.RoleName.Contains("����") || curUser.RoleName.Contains("��˾��"))
            {
                isCompany = true;
                code = curUser.OrganizeCode;
            }
            else
            {
                isCompany = false;
                code = curUser.DeptCode;
            }
            string deptCode = queryJson.Contains("deptCode") ? queryParam["deptCode"].ToString() == "" ? code : queryParam["deptCode"].ToString() : "";  //���ű���
            //string deptCode = curUser.DeptCode;
            var treeList = new List<TreeGridEntity>();
            var dt = drillplanrecordbll.DrillplanStatList("", isCompany, deptCode, startTime, endTime);

            var datamode = new DataItemDetailBLL().GetDataItemListByItemCode("'MAE_DirllMode'").ToList();

            foreach (DataRow row in dt.Rows)
            {
                DataTable newDt = new DataTable();
                newDt.Columns.Add("changedutydepartcode");
                newDt.Columns.Add("fullname");
                newDt.Columns.Add("departmentid");
                newDt.Columns.Add("parent");
                newDt.Columns.Add("total");
                newDt.Columns.Add("haschild", typeof(bool));
                if (datamode.Count > 0)
                {
                    for (int i = 0; i < datamode.Count; i++)
                    {
                        newDt.Columns.Add("recordnum" + (i + 1));
                    }
                }
                else
                {
                    newDt.Columns.Add("recordnum1");
                    newDt.Columns.Add("recordnum2");
                }
                DataRow newDr = newDt.NewRow();
                newDr["changedutydepartcode"] = row["encode"].ToString();
                newDr["fullname"] = row["fullname"].ToString();
                newDr["departmentid"] = row["departmentid"].ToString();
                if (row["parentid"].ToString() != "0")
                {
                    newDr["parent"] = row["parentid"].ToString();
                }

                newDr["total"] = row["total"].ToString();
                if (datamode.Count > 0)
                {
                    for (int i = 0; i < datamode.Count; i++)
                    {
                        newDr["recordnum" + (i + 1)] = row["recordnum" + (i + 1)].ToString();
                    }
                }
                else
                {
                    newDr["recordnum1"] = row["recordnum1"].ToString();
                    newDr["recordnum2"] = row["recordnum2"].ToString();
                }
                //if (datamode.Count > 0) {
                //    var tentity = new
                //    {
                //        changedutydepartcode = row["encode"].ToString(),
                //        fullname = row["fullname"].ToString(),
                //        departmentid = row["departmentid"].ToString(),
                //        parent = row["parentid"].ToString(),
                //        total=Convert.ToDecimal(row["total"].ToString())
                //    };
                //    for (int i = 0; i < datamode.Count; i++)
                //    {

                //    }
                //}

                //TreeListForHiddenSituation tentity = new TreeListForHiddenSituation();
                //tentity.changedutydepartcode = row["encode"].ToString();
                //tentity.fullname = row["fullname"].ToString();
                //tentity.departmentid = row["departmentid"].ToString();
                //if (row["parentid"].ToString() != "0")
                //{
                //    tentity.parent = row["parentid"].ToString();
                //}
                //tentity.total = Convert.ToDecimal(row["total"].ToString());
                TreeGridEntity tree = new TreeGridEntity();
                bool hasChildren = dt.Select(string.Format(" parentid ='{0}'", row["departmentid"].ToString())).Count() == 0 ? false : true;
                newDr["haschild"] = hasChildren;
                newDt.Rows.Add(newDr);
                //tentity.haschild = hasChildren;
                tree.id = row["departmentid"].ToString();
                tree.parentId = row["parentid"].ToString();
                string itemJson = newDt.ToJson().Replace("[", "").Replace("]", "");
                //string itemJson = tentity.ToJson();
                tree.entityJson = itemJson;
                tree.expanded = false;
                tree.hasChildren = hasChildren;
                treeList.Add(tree);
            }

            //�������
            return Content(treeList.TreeJson(curUser.ParentId));
        }
        #endregion
        public void DrillplanStatExportExcel(string queryJson)
        {
            DataTable dt = new DataTable();
            var queryParam = queryJson.ToJObject();
            var curUser = new OperatorProvider().Current(); //��ǰ�û�
            var isCompany = false;
            //��ǰ�û��ǳ���
            if (curUser.RoleName.Contains("����") || curUser.RoleName.Contains("��˾��"))
            {
                isCompany = true;
            }
            else
            {
                isCompany = false;
            }
            string deptCode = queryJson.Contains("deptCode") ? queryParam["deptCode"].ToString() == "" ? curUser.DeptCode : queryParam["deptCode"].ToString() : "";  //���ű���
            string startTime = queryJson.Contains("startTime") ? queryParam["startTime"].ToString() : "";  //��ʼ���� 
            string endTime = queryJson.Contains("endTime") ? queryParam["endTime"].ToString() : "";//��ֹ����


            ////���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            ////ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            ColumnEntity columnentity = new ColumnEntity();
            excelconfig.Title = "Ӧ����������ͳ��";
            excelconfig.FileName = "Ӧ����������ͳ��.xls";
            var datamode = new DataItemDetailBLL().GetDataItemListByItemCode("'MAE_DirllMode'").ToList();
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "fullname", ExcelColumn = "��������", Width = 20 });
            if (datamode.Count > 0)
            {
                for (int i = 0; i < datamode.Count; i++)
                {
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "recordnum" + (i + 1), ExcelColumn = datamode[i].ItemName, Width = 20 });
                }
            }
            else
            {
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "recordnum1", ExcelColumn = "��������", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "recordnum2", ExcelColumn = "ʵս����", Width = 20 });
            }
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "total", ExcelColumn = "�ϼ�", Width = 20 });
            dt = drillplanrecordbll.DrillplanStatList("", isCompany, deptCode, startTime, endTime);
            ////���õ�������
            ExcelHelper.ExcelDownload(dt, excelconfig);
        }

        [HttpGet]
        public static string GetResult(string keyword)
        {

            string[] key = keyword.Split(' ');
            StringBuilder sql = new StringBuilder();
            string returnStr = "";
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    returnStr += i + ";";
                }
            }
            catch { }
            return returnStr;
        }

        public class dseries
        {
            public string name { get; set; }
            public string id { get; set; }
            public List<dseries_child> data { get; set; }
        }

        public class dseries_child
        {
            public string name { get; set; }
            public int y { get; set; }
            public string drilldown { get; set; }
        }
    }
}
