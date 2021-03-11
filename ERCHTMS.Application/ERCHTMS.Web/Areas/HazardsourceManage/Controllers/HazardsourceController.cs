using ERCHTMS.Entity.HazardsourceManage;
using ERCHTMS.Busines.HazardsourceManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Code;
using System.Text;
using System;
using BSFramework.Util.Extension;
using System.Linq;
using System.Web;
using System.Data;
using ERCHTMS.Cache;
using BSFramework.Util.Offices;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using System.Linq.Expressions;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.RiskDatabase;
using ERCHTMS.Entity.RiskDatabase;
using System.Collections.Generic;
using System.Threading;

namespace ERCHTMS.Web.Areas.HazardsourceManage.Controllers
{
    /// <summary>
    /// �� ����Σ��Դ��ʶ����
    /// </summary>
    public class HazardsourceController : MvcControllerBase
    {
        private HazardsourceBLL hazardsourcebll = new HazardsourceBLL();
        private HistoryBLL historybll = new HistoryBLL();
        private Hisrelationhd_qdBLL hisrelationhd_qdbLL = new Hisrelationhd_qdBLL();
        private UserBLL userBLL = new UserBLL();
        private DepartmentBLL departBLL = new DepartmentBLL();
        private OrganizeBLL orgBLL = new OrganizeBLL();
        private DistrictBLL bis_districtbll = new DistrictBLL();
        private DataItemDetailBLL didbll = new DataItemDetailBLL();
        private MeasuresBLL measuresBLL = new MeasuresBLL();
        #region ��ͼ����
        [HttpGet]
        public ActionResult ShowMeaSure()
        {
            return View();
        }


        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Selectq()
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
            pagination.p_fields = @"gradeval,riskassessid,DeptCode as createuserdeptcode,
                                    createuserorgcode,createuserid,districtname,(select count(id) from bis_measures where riskId=t.id) MeaSureNum, DANGERSOURCE, 
                                    ACCIDENTNAME,MEASURE,DEPTNAME,JDGLZRRFULLNAME,ISDANGER,RESULT,
                                    case WHEN  ISDANGER>0 then '��' else '��' end as ISDANGERNAME,grade";
            pagination.p_tablename = "hsd_Hazardsource t";
            pagination.conditionJson = "1=1";
            pagination.sidx = pagination.sidx + " " + pagination.sord + ",id";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string val = new DataItemDetailBLL().GetItemValue("IsOpenPassword");
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                if (user.RoleName.Contains("ʡ��"))
                {
                    pagination.conditionJson += " and (CreateUserId='" + user.UserId + "' or DeptCode in(select  encode from BASE_DEPARTMENT start with encode='" + user.NewDeptCode + "' connect by  prior  departmentid = parentid))";
                }
                else {
                    if (user.RoleName.Contains("����") || user.RoleName.Contains("��˾��"))
                    {
                        pagination.conditionJson += " and (CreateUserId='" + user.UserId + "' or DeptCode like '" + user.OrganizeCode + "%')";
                    }
                    else 
                    {
                        string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                        if (!string.IsNullOrEmpty(authType))
                        {
                            switch (authType)
                            {
                                case "1":
                                    pagination.conditionJson += " and createuserid='" + user.UserId + "'";
                                    break;
                                case "2":
                                    pagination.conditionJson += " and DeptCode='" + user.DeptCode + "'";
                                    break;
                                case "3":
                                    pagination.conditionJson += " and (CreateUserId='" + user.UserId + "' or DeptCode like '" + user.DeptCode + "%')";
                                    break;
                                case "4":
                                    pagination.conditionJson += " and (CreateUserId='" + user.UserId + "' or DeptCode like '" + user.OrganizeCode + "%')";
                                    break;
                            }
                        }
                        //if (val == "true")
                        //{
                        //    pagination.conditionJson += " and (CreateUserId='" + user.UserId + "' or DeptCode like '" + user.OrganizeCode + "%')";
                        //}
                        //else
                        //{
                        //    pagination.conditionJson += " and (CreateUserId='" + user.UserId + "' or DeptCode like '" + user.DeptCode + "%')";
                        //}
                    }
                }
               
            }

            var IsDanger = Request["IsDanger"] ?? "";

            if (IsDanger.Length > 0)
                pagination.conditionJson += " and IsDanger=" + IsDanger;

            var Grade = Request["Grade"] ?? "";

            if (Grade.Length > 0)
            {
                if (Grade == "All")
                    pagination.conditionJson += " and GradeVal>0";

                else
                    pagination.conditionJson += " and Grade='" + Grade + "' and GradeVal>0";
            }
            var GradeVal = Request["GradeVal"] ?? "";
            if (GradeVal.Length > 0){
                pagination.conditionJson += string.Format(" and GradeVal = '{0}'", GradeVal);
            }
            var FullName = Request["fullName"] ?? "";
            if (FullName.Length > 0 && FullName == "ȫ��")
            {

            }
            else {
                var UnitCode = Request["UnitCode"] ?? "";
                if (UnitCode.Length > 0)
                {
                    pagination.conditionJson += string.Format(" and deptcode like '{0}%'", UnitCode);
                }
            }
            var areaCode = Request["areaCode"] ?? "";
            if (!string.IsNullOrEmpty(areaCode))
            {
                pagination.conditionJson += string.Format(" and gradeval>0 and districtid in(select districtid from bis_district where districtcode like '{0}%')", areaCode);
            }
            var DistrictName = Request["DistrictName"] ?? "";

            if (DistrictName.Length > 0)
                pagination.conditionJson += " and DistrictName like '%" + DistrictName + "%'";


            var TimeYear = Request["TimeYear"] ?? "";
            if (TimeYear.Length > 0)
                pagination.conditionJson += string.Format(" and to_char(CreateDate, 'yyyy')='{0}'", TimeYear);




            var watch = CommonHelper.TimerStart();
            var data = hazardsourcebll.GetPageList(pagination, queryJson);
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
        /// 
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetDangerDjjdPageListJson(Pagination pagination, string queryJson)
        {
            queryJson = queryJson ?? "";
            pagination.p_kid = "id";
            pagination.p_fields = "MEASURENum,riskassessid,gradeval,grade,DeptCode as createuserdeptcode, createuserorgcode,createuserid,districtname, DANGERSOURCE, ACCIDENTNAME,MEASURE,DEPTNAME,JDGLZRRFULLNAME,ishx,isba,isdjjd,qrcode, dealid";
            pagination.p_tablename = "v_hsd_dangerqd_djjd t";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string val = new DataItemDetailBLL().GetItemValue("IsOpenPassword");
                if (user.RoleName.Contains("ʡ��"))
                {
                    pagination.conditionJson += " and (CreateUserId='" + user.UserId + "' or DeptCode in(select  encode from BASE_DEPARTMENT start with encode='" + user.NewDeptCode + "' connect by  prior  departmentid = parentid))";
                }
                else
                {
                    string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                    if (!string.IsNullOrEmpty(authType))
                    {
                        switch (authType)
                        {
                            case "1":
                                pagination.conditionJson += " and createuserid='" + user.UserId + "'";
                                break;
                            case "2":
                                pagination.conditionJson += " and DeptCode='" + user.DeptCode + "'";
                                break;
                            case "3":
                                pagination.conditionJson += " and (CreateUserId='" + user.UserId + "' or DeptCode in(select  encode from BASE_DEPARTMENT start with encode='" + user.DeptCode + "' connect by  prior  departmentid = parentid))";
                                break;
                            case "4":
                                pagination.conditionJson += " and (CreateUserId='" + user.UserId + "' or DeptCode in(select  encode from BASE_DEPARTMENT start with encode='" + user.OrganizeCode + "' connect by  prior  departmentid = parentid))";
                                break;
                        }
                    }

                    //if (val=="true")
                    //{
                    //    pagination.conditionJson += " and (CreateUserId='" + user.UserId + "' or DeptCode in(select  encode from BASE_DEPARTMENT start with encode='" + user.OrganizeCode + "' connect by  prior  departmentid = parentid))";
                    //}
                    //else
                    //{
                    //    pagination.conditionJson += " and (CreateUserId='" + user.UserId + "' or DeptCode in(select  encode from BASE_DEPARTMENT start with encode='" + user.DeptCode + "' connect by  prior  departmentid = parentid))";
                    //}
                  
                }
            }
            var State = Request["State"] ?? "";
            if (State.Length > 0)
            {
                if (State == "3")
                {
                    pagination.conditionJson += string.Format(" and  (t.isdjjd = '0' or t.isdjjd is null) ");
                }
            }
            var TimeYear = Request["TimeYear"] ?? "";
            if (TimeYear.Length > 0)
                pagination.conditionJson += string.Format(" and to_char(CreateDate, 'yyyy')='{0}'", TimeYear);
            var watch = CommonHelper.TimerStart();
            var data = hazardsourcebll.GetPageList(pagination, queryJson);
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
        /// ���
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetJkPageListJson(Pagination pagination, string queryJson)
        {
            queryJson = queryJson ?? "";
            pagination.p_kid = "id";
            pagination.p_fields = "gradeval, jkarear,grade,DeptCode as createuserdeptcode, createuserorgcode,createuserid,districtname, DANGERSOURCE, ACCIDENTNAME,MEASURE,DEPTNAME,jktimeend,jktimestart,JkyhzgIds,jkskstatus, dealid";
            pagination.p_tablename = "V_HSD_DANGERQD_JK t";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string val = new DataItemDetailBLL().GetItemValue("IsOpenPassword");
                if (user.RoleName.Contains("ʡ��"))
                {
                    pagination.conditionJson += " and (CreateUserId='" + user.UserId + "' or DeptCode in(select  encode from BASE_DEPARTMENT start with encode='" + user.NewDeptCode + "' connect by  prior  departmentid = parentid))";
                }
                else
                {
                    string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                    if (!string.IsNullOrEmpty(authType))
                    {
                        switch (authType)
                        {
                            case "1":
                                pagination.conditionJson += " and createuserid='" + user.UserId + "'";
                                break;
                            case "2":
                                pagination.conditionJson += " and DeptCode='" + user.DeptCode + "'";
                                break;
                            case "3":
                                pagination.conditionJson += " and (CreateUserId='" + user.UserId + "' or DeptCode in(select  encode from BASE_DEPARTMENT start with encode='" + user.DeptCode + "' connect by  prior  departmentid = parentid))";
                                break;
                            case "4":
                                pagination.conditionJson += " and (CreateUserId='" + user.UserId + "' or DeptCode in(select  encode from BASE_DEPARTMENT start with encode='" + user.OrganizeCode + "' connect by  prior  departmentid = parentid))";
                                break;
                        }
                    }
                    //if (val=="true")
                    //{
                    //    pagination.conditionJson += " and (CreateUserId='" + user.UserId + "' or DeptCode in(select  encode from BASE_DEPARTMENT start with encode='" + user.OrganizeCode + "' connect by  prior  departmentid = parentid))";
                    //}
                    //else
                    //{
                    //    pagination.conditionJson += " and (CreateUserId='" + user.UserId + "' or DeptCode in(select  encode from BASE_DEPARTMENT start with encode='" + user.DeptCode + "' connect by  prior  departmentid = parentid))";
                    //}
                   
                }
            }
            var State = Request["State"] ?? "";
            if (State.Length > 0)
            {
                if (State == "1")
                {
                    pagination.conditionJson += string.Format(" and t.jkyhzgids>0");
                }
                else if (State == "2")
                {
                    pagination.conditionJson += string.Format(" and (t.jkskstatus='0' or t.jkskstatus is null)");
                }
            }
            var TimeYear = Request["TimeYear"] ?? "";
            if (TimeYear.Length > 0)
                pagination.conditionJson += string.Format(" and to_char(CreateDate, 'yyyy')='{0}'", TimeYear);
            var watch = CommonHelper.TimerStart();
            var data = hazardsourcebll.GetPageList(pagination, queryJson);
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
            var data = hazardsourcebll.GetList(queryJson);
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
            var data = hazardsourcebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region �ύ����

        /// <summary>
        /// ���뱾��
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportHazardsource()
        {
            //if (OperatorProvider.Provider.Current().IsSystem)
            //{
            //    return "��������Ա�޴˲���Ȩ��";
            //}
            string orgId = OperatorProvider.Provider.Current().OrganizeId;//������˾
            int error = 0;
            string message = "��ѡ���ʽ��ȷ���ļ��ٵ���!";
            string falseMessage = "";
            int count = HttpContext.Request.Files.Count;
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
                    // ��������
                    var districtname = dt.Rows[i][0].ToString();
                    var district = bis_districtbll.GetList().Where(e => e.DistrictName == districtname).FirstOrDefault();
                    if (district == null)
                    {
                        falseMessage += "��" + (i) + "�е���ʧ��,�������򲻴��ڣ�</br>";
                        error++;
                        continue;
                    }
                    // Σ��Դ����/����
                    var dangersource = dt.Rows[i][1].ToString();
                    //���ܵ��µ��¹�����
                    var accidentname = dt.Rows[i][2].ToString();
                    var accidenttype = "";
                    //int checkacc = 0;
                    //var listaccident = accidentname.Split(',');
                    //foreach (var item in listaccident)
                    //{
                    //    var datarow = didbll.GetListByCode("AccidentType").Select(" ItemName='" + item + "'");
                    //    if (datarow == null || datarow.Count() == 0)
                    //    {
                    //        checkacc++;
                    //    }
                    //    else
                    //    {
                    //        if (accidenttype == "")
                    //            accidenttype = datarow[0]["itemvalue"].ToString();
                    //        else
                    //            accidenttype += "," + datarow[0]["itemvalue"].ToString();

                    //    }

                    //}
                    //if (checkacc > 0)
                    //{
                    //    falseMessage += "��" + (i) + "�е���ʧ��,���ܵ��µ��¹����Ͳ����ڣ�</br>";
                    //    error++;
                    //    continue;
                    //}


                    //Σ��Դ���յȼ�
                    string grade = dt.Rows[i][3].ToString();
                    var gradeval = grade == "һ��" ? "1" : (grade == "����" ? "2" : (grade == "����" ? "3" : "4"));
                    //��ȫ���ƴ�ʩ
                    var measure = dt.Rows[i][4].ToString();
                    var listmeasure = measure.Split('&');
                    //���β���
                    string departname = dt.Rows[i][5].ToString();
                    var depart = departBLL.GetList().Where(e => e.FullName == departname && e.OrganizeId == orgId).FirstOrDefault();
                    var org = orgBLL.GetList().Where(e => e.FullName == departname).FirstOrDefault();
                    if (depart == null && org == null)
                    {
                        falseMessage += "��" + (i) + "�е���ʧ��,���β��Ų����ڣ�</br>";
                        error++;
                        continue;
                    }
                    //�ල����������
                    string fullname = dt.Rows[i][6].ToString();
                    Expression<Func<UserEntity, bool>> condition = e => e.RealName == fullname && e.OrganizeId == orgId;
                    var user = userBLL.GetListForCon(condition).FirstOrDefault();
                    if (user == null)
                    {
                        falseMessage += "��" + (i) + "�е���ʧ��,�ල���������˲����ڣ�</br>";
                        error++;
                        continue;
                    }
                    //�������� L,E,C,D
                    string L = dt.Rows[i][7].ToString();
                    string E = dt.Rows[i][8].ToString();
                    string C = dt.Rows[i][9].ToString();
                    string D = dt.Rows[i][10].ToString();
                    //�Ƿ��ش�Σ��Դ
                    string isdangername = dt.Rows[i][11].ToString();
                    int? isdanger = isdangername == "��" ? 1 : 0;
                    try
                    {
                        var item = new HazardsourceEntity
                        {
                            ID = Guid.NewGuid().ToString(),
                            MeaSureNum = listmeasure.Count(),
                            DistrictName = districtname,
                            DistrictId = district.DistrictID,
                            DangerSource = dangersource,
                            AccidentName = accidentname,
                            AccidentType = accidenttype,
                            Grade = grade,
                            GradeVal = Convert.ToInt32(gradeval),

                            DeptName = departname,
                            DeptCode = depart.EnCode,
                            JdglzrrFullName = fullname,
                            JdglzrrUserId = user.UserId,
                            ItemA = decimal.Parse(L),
                            ItemB = decimal.Parse(E),
                            ItemC = decimal.Parse(C),
                            ItemR = decimal.Parse(D),
                            IsDanger = isdanger,
                            Way = "LEC",
                            Status = 1
                        };
                        item.ID = Guid.NewGuid().ToString();
                        hazardsourcebll.SaveForm("", item);
                        var list = new List<MeasuresEntity>();
                        foreach (var item2 in listmeasure)
                        {
                            if (string.IsNullOrEmpty(item2))
                                continue;
                            list.Add(new MeasuresEntity { RiskId = item.ID, Content = item2 });

                        }

                        var mbll = new MeasuresBLL();
                        mbll.SaveForm("", list);
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

            return message;
        }



        /// <summary>
        /// ����ƽ̨����
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]


        public ActionResult ImportForPlamt()
        {
            Operator user = OperatorProvider.Provider.Current();
            if (user.IsSystem) {
                return Error("ϵͳ����Ա�������룡");
            }
            StringBuilder sbwhere = new StringBuilder();
            StringBuilder sbSql = new StringBuilder();
            //insert into  select �﷨
            var random = new Random(100);
            sbSql.Append(" insert into hsd_hazardsource ");
            sbSql.Append(" (ID,districtid,districtname,DANGERSOURCE,RESULT,ACCIDENTNAME,MEASURE,");
            sbSql.Append(" DEPTCODE,DEPTNAME,STATUS,CREATEDATE,CREATEUSERID,CREATEUSERNAME,CREATEUSERDEPTCODE,");
            sbSql.Append(" CREATEUSERORGCODE,RISKASSESSID,ISDANGER,GRADEVAL,GRADE,ITEMA,ITEMB,ITEMC,ITEMR,RiskType,Way)");
            sbSql.Append(" select");
            sbSql.Append(" (id||'-'||'" + DateTime.Now.ToString("yyyyMMddhhmmssff") + "'),districtid,districtname,DANGERSOURCE,RESULT,RESULT,MEASURE,");
            sbSql.Append(" DEPTCODE,DEPTNAME,2,(select sysdate FROM DUAL),CREATEUSERID,CREATEUSERNAME,CREATEUSERDEPTCODE,");
            sbSql.Append("  CREATEUSERORGCODE,ID,case WHEN  GRADEVal>2 then  0 else 1 end as ISDANGER,0,grade,ITEMA,ITEMB,ITEMC,ITEMR,RiskType,upper(Way)");
            //��ȫ�����嵥������
            sbSql.Append(" from BIS_RISKASSESS where status=1 and deletemark=0 and DANGERSOURCE is not null  ");

            //��ѯ����
            if (user.IsSystem)
            {
                sbwhere.Append(" and  1=1");
            }
            else
            {
                //��ѯ����(����ǰ��¼���й�ϵ�Ĳ�������)
                sbwhere.Append(" and DeptCode like '" + user.DeptCode + "%'");
            }
            string sql = sbSql.ToString() + sbwhere.ToString();
            //ִ��֮ǰ��ɾ����ǰ��������(������)
            string sqldel = "delete from hsd_hazardsource where DeptCode like '" + user.DeptCode + "%' and status=2";
            //ִ��sql
            try
            {
                var result = hazardsourcebll.ExecuteBySql(sqldel);


                result = hazardsourcebll.ExecuteBySql(sql);
                //����ɹ�,����ȥ��
                //Thread tr = new Thread(DataQC);
                //tr.Start();
                //while (!tr.IsAlive)
                //{
                //    tr.Abort();//��ֹ�߳�
                //}
                //����������ί��
                MyDelegate dele = new MyDelegate(DataQC);
                var resultDele = dele.BeginInvoke(0, null, null);
                var qci = dele.EndInvoke(resultDele);
                if (qci > 0)
                    return Success("����ɹ�����������ȥ�أ�");
                return Success("����ɹ���");

            }
            catch (Exception)
            {
                return Error("����ʧ��");
                throw;
            }
        }


        /// <summary>
        /// ����ȥ�ظ�
        /// </summary>
        public Operator deleUser = OperatorProvider.Provider.Current();

        //����һ��ί��
        public delegate int MyDelegate(int qci);

        public int DataQC(int qci)
        {
            //�¹�����ȥ��
            //1���ҵ�ȥ�غ��Ψһ����
            var sql = string.Format("select * from V_hsd_Hazardsource where DeptCode like '{0}%'", deleUser.DeptCode);
            var dt = hazardsourcebll.FindTableBySql(sql);
            foreach (DataRow dr in dt.Rows)
            {
                //2���ҵ��¹����ͼ���
                var AccidentInfo = GetAccidentForRiskId(dr["ID"].ToString());
                //3����ȫ��ʩȥ��
                var Count = GetCountByRiskId(dr["ID"].ToString());
                if (AccidentInfo.Length > 0)
                    hazardsourcebll.ExecuteBySql(string.Format("update hsd_Hazardsource set measurenum={0},accidentname='{1}',accidenttype='{2}' where Id='{3}'", Count, AccidentInfo, AccidentInfo, dr["ID"].ToString()));
                //3��ɾ�������е��ظ�����
                string sqldel = string.Format("delete from hsd_Hazardsource where Id!='{0}' and districtid='{1}' and dangersource='{2}' and DeptCode like '{3}%' and RiskType='{4}' and status=2", dr["ID"].ToString(), dr["districtid"].ToString(), dr["dangersource"].ToString(), deleUser.DeptCode, dr["RiskType"].ToString());
                hazardsourcebll.ExecuteBySql(sqldel);
            }

            qci++;
            return qci;
        }

        /// <summary>
        /// ��Ϊ��ʷ��¼
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SetHistory(string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            try
            {

                //�½���ʷ��¼�嵥
                var entity = new HistoryEntity
                {
                    DangerSourceName = "Σ��Դ��ʶ�����嵥" + DateTime.Now.ToString("yyyyMMdd"),
                    CreateDate = DateTime.Now,
                    CreateUserDeptCode = user.DeptCode,
                    CreateUserId = user.UserId,
                    CreateUserName = user.UserName,
                    CreateUserOrgCode = user.OrganizeCode
                };
                entity.ID = Guid.NewGuid().ToString();
                //���ϵ�����������
                StringBuilder sbSql = new StringBuilder();
                sbSql.Append(" insert into HSD_HISRELATIONHD (HISTORYID,HAZARDSOURCEID,");
                sbSql.Append(" ID,districtid,districtname,DANGERSOURCE,RESULT,ACCIDENTNAME,MEASURE,");
                sbSql.Append(" DEPTCODE,DEPTNAME,STATUS,CREATEDATE,CREATEUSERID,CREATEUSERNAME,CREATEUSERDEPTCODE,");
                sbSql.Append(" CREATEUSERORGCODE,RISKASSESSID,ISDANGER,GRADEVAL,GRADE,ITEMA,ITEMB,ITEMC,ITEMR,RiskType,Way,");
                sbSql.Append(" ITEMDECB1,ITEMDECR,ITEMDECQ,ITEMDECQ1,ITEMDECR1,ITEMDECB,jdglzrrfullname)");
                sbSql.Append(" select (select '" + entity.ID + "' FROM DUAL),ID,");
                sbSql.Append(" (ID||'-'||'" + DateTime.Now.ToString("yyyyMMddhhmmssff") + "'),districtid,districtname,DANGERSOURCE,RESULT,ACCIDENTNAME,MEASURE,");
                sbSql.Append(" DEPTCODE,DEPTNAME,2,(select sysdate FROM DUAL),CREATEUSERID,CREATEUSERNAME,CREATEUSERDEPTCODE,");
                sbSql.Append("  CREATEUSERORGCODE,RISKASSESSID,ISDANGER,GRADEVAL,GRADE,ITEMA,ITEMB,ITEMC,ITEMR,RiskType,upper(Way),");
                sbSql.Append(" ITEMDECB1,ITEMDECR,ITEMDECQ,ITEMDECQ1,ITEMDECR1,ITEMDECB,jdglzrrfullname");
                sbSql.Append(" from hsd_hazardsource where 1=1 ");
                if (!user.IsSystem)
                {
                    //���ñ�����������
                    sbSql.Append(" and (CreateUserId='" + user.UserId + "' or DeptCode like '" + user.DeptCode + "%')");
                }

                #region ��ѯ����
                if (queryJson.Length > 0)
                {

                    var queryParam = queryJson.ToJObject();

                    //��ѯ����
                    if (!queryParam["DistrictId"].IsEmpty())
                    {
                        string DistrictId = queryParam["DistrictId"].ToString();
                        var DistrictName = queryParam["DistrictName"].ToString();
                        sbSql.Append(" and DistrictName like '%" + DistrictName + "%'");
                    }
                    //��ѯ����
                    if (!queryParam["DangerSource"].IsEmpty())
                    {
                        string DangerSource = queryParam["DangerSource"].ToString();
                        sbSql.AppendFormat(" and DangerSource like '%{0}%'", DangerSource);
                    }
                    //��ѯ����
                    if (!queryParam["IsDanger"].IsEmpty())
                    {
                        string IsDanger = queryParam["IsDanger"].ToString();
                        sbSql.AppendFormat(" and IsDanger = '{0}'", IsDanger);
                    }
                    //��ѯ����
                    if (!queryParam["isOrg"].IsEmpty())
                    {
                        string isOrg = queryParam["isOrg"].ToString();
                        if (isOrg == "Organize")
                        {
                            sbSql.AppendFormat(" and DeptCode like '{0}%'", user.DeptCode);
                        }
                    }


                }





                #endregion

                string sql = sbSql.ToString();

                //var entityCheck = historybll.GetList(" and createuserorgcode='" + user.OrganizeCode + "'  and DangerSourceName='" + entity.DangerSourceName + "'").FirstOrDefault();
                //if (entityCheck != null)
                //{
                //    historybll.RemoveForm(entityCheck.ID);
                //    //ɾ����Ӧ�Ĺ�ϵ����
                //    hazardsourcebll.ExecuteBySql(" delete from hsd_hisrelationhd where historyid='" + entityCheck.ID + "'");
                //}
                //���浱ǰ��¼
                historybll.Save(entity);
                //�����������
                var result = hazardsourcebll.ExecuteBySql(sql);

                if (result > 0)
                    return Success("�����ɹ���");

            }
            catch (Exception)
            {
                return Error("����ʧ�ܡ�");
                throw;
            }
            return Error("����ʧ�ܡ�");
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
            hazardsourcebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, HazardsourceEntity entity)
        {

            //����ɹ�,����嵥
            var json = entity.ToJson();

            var list = new List<MeasuresEntity>();
            var measuresJson = Request["measuresJson"] ?? "";
            if (measuresJson.Length > 0)
            {

                list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MeasuresEntity>>(measuresJson);
                entity.MeaSureNum = list.Count();
            }
            if (keyValue == "") entity.Status = 3;//��ʾ����
            hazardsourcebll.SaveForm(keyValue, entity);
            foreach (var item in list)
            {
                var entitySave = measuresBLL.GetEntity(item.Id);
                if (entitySave == null)
                    entitySave = new MeasuresEntity();
                entitySave.Content = item.Content;
                entitySave.RiskId = item.RiskId == "" ? entity.ID : item.RiskId;
                measuresBLL.Save(entitySave.Id, entitySave);
            }




            //var entityqd = json.ToObject<Hisrelationhd_qdEntity>();
            //entityqd.ID = "";
            //hisrelationhd_qdbLL.SaveForm("", entityqd);
            return Success("�����ɹ���");
        }
        #endregion

        #region ��ȡ��ʩ��Ϣ
        /// <summary>
        /// ��ȡ��ʩ��Ϣȥ�غ������
        /// </summary>
        /// <param name="riskId"></param>
        /// <returns></returns>
        public string GetCountByRiskId(string riskId)
        {
            var entity = hazardsourcebll.GetEntity(riskId);
            string sql = string.Format("select count(1) as Num,Content from bis_measures where RiskId in (select riskassessid from hsd_Hazardsource where  status=2 and districtid='{0}' and dangersource='{1}' and DeptCode like '{2}%' and RiskType='{3}') group by Content", entity.DistrictId, entity.DangerSource, deleUser.DeptCode, entity.RiskType);
            var dt = hazardsourcebll.FindTableBySql(sql);
            if (dt == null || dt.Rows.Count == 0)
                return "0";
            var list = new List<MeasuresEntity>();
            StringBuilder sb = new StringBuilder();
            //��ѯ�����ݲ��뵽��ʩ����
            int i = 0;
            sb.Append(" insert into bis_measures(ID,Content,RiskId,CreateUserId,CreateDate,CreateUserName,CreateUserDeptCode,CreateUserOrgCode) ");
            foreach (DataRow dr in dt.Rows)
            {
                i++;
                var Content = dr["Content"].ToString();

                if (i < dt.Rows.Count)
                    sb.Append(" select '" + Guid.NewGuid().ToString() + "','" + Content + "','" + riskId + "','" + deleUser.UserId + "',sysdate,'" + deleUser.UserName + "','" + deleUser.DeptCode + "','" + deleUser.OrganizeCode + "' from dual union all");
                else
                    sb.Append(" select '" + Guid.NewGuid().ToString() + "','" + Content + "','" + riskId + "','" + deleUser.UserId + "',sysdate,'" + deleUser.UserName + "','" + deleUser.DeptCode + "','" + deleUser.OrganizeCode + "' from dual");
            }
            //�����ʩ����
            if (i > 0)
            {
                hazardsourcebll.ExecuteBySql(sb.ToString());
            }
            return i.ToString();
        }


        /// <summary>
        /// ��ȡȥ�ص��¹�����
        /// </summary>
        /// <param name="riskId"></param>
        /// <returns></returns>
        public String GetAccidentForRiskId(string riskId)
        {
            var entity = hazardsourcebll.GetEntity(riskId);
            var list = hazardsourcebll.GetList(string.Format(" and districtid='{0}' and dangersource='{1}' and status=2  and DeptCode like '{2}%' and RiskType='{3}'", entity.DistrictId, entity.DangerSource, deleUser.DeptCode, entity.RiskType));
            var AccidentName = "";
            foreach (var item in list)
            {
                if (item.AccidentName == null)
                    continue;
                var AccidentNames = item.AccidentName.Split(',');
                var AccidentNames1 = item.AccidentName.Split(';');
                if (AccidentNames.Length > 0)
                {
                    foreach (var itemAcc in AccidentNames)
                    {
                        if (AccidentName.Contains(itemAcc))
                            continue;
                        AccidentName += itemAcc + ",";
                    }
                }
                else if (AccidentNames1.Length > 0)
                {
                    foreach (var itemAcc1 in AccidentNames1)
                    {
                        if (AccidentName.Contains(itemAcc1))
                            continue;
                        AccidentName += itemAcc1 + ",";
                    }
                }
            }

            return AccidentName.TrimEnd(',');
        }


        #endregion
    }
}
