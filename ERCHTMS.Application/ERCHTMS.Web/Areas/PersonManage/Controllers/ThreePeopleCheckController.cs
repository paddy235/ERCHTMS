using ERCHTMS.Entity.PersonManage;
using ERCHTMS.Busines.PersonManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Collections.Generic;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Busines.BaseManage;
using System.Web;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Text;
using BSFramework.Cache.Factory;
using System.Linq;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.SystemManage;
using BSFramework.Util.Offices;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Busines.Desktop;
namespace ERCHTMS.Web.Areas.PersonManage.Controllers
{
    /// <summary>
    /// �� ��������������ҵ���
    /// </summary>
    public class ThreePeopleCheckController : MvcControllerBase
    {
        private ThreePeopleCheckBLL threepeoplecheckbll = new ThreePeopleCheckBLL();

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
        /// �������嵥�б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult List()
        {
            return View();
        }
        /// <summary>
        /// �����˵���
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }
        /// <summary>
        /// ������Ա
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Add()
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
        public ActionResult GetListJson(Pagination pagination, string queryJson,int mode=0)
        {
            Operator user = OperatorProvider.Provider.Current();

            try
            {
                if (mode == 1)
                {
                    
                    DesktopBLL deskBll = new DesktopBLL();
                    List<string> list = deskBll.GetThreeCount(user);
                    string ids = string.Join(",", list);
                    pagination.p_kid = "t.ID";
                    pagination.p_fields = string.Format("status,isover,issumbit,t.CreateUserId,t.useraccount,belongdept,applysno,t.applytype,t.createusername,t.createtime,flowname,t.createuserdeptid,checkroleid,'{0}' checkdeptid", user.DeptId);
                    pagination.p_tablename = string.Format("BIS_THREEPEOPLECHECK t left join bis_manypowercheck c on t.nodeid=c.id");
                    pagination.conditionJson = string.Format("t.id in('{0}')", ids.Replace(",", "','"));
                }
                else
                {
                    pagination.p_kid = "t.ID";
                    pagination.p_fields = "t.status,t.isover,issumbit,t.CreateUserId,t.useraccount,t.belongdept,t.applysno,t.applytype,t.createusername,t.createtime,c.flowname,t.createuserdeptid,c.checkdeptid,c.checkroleid";
                    pagination.p_tablename = "BIS_THREEPEOPLECHECK t left join bis_manypowercheck c on t.nodeid=c.id";
                    pagination.conditionJson = "1=1";
                    string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "t.createuserdeptcode", "t.createuserorgcode");
                    if (!string.IsNullOrEmpty(where))
                    {
                        pagination.conditionJson += " and " + where;
                    }

                }
                var watch = CommonHelper.TimerStart();
                DataTable data = threepeoplecheckbll.GetPageList(pagination, queryJson);
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
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
           
        }
        [HttpGet]
        public ActionResult GetItemListJson(string applyId)
        {
            var  data = threepeoplecheckbll.GetUserList(applyId).ToList();
            return ToJsonResult(data);
        }
        [HttpGet]
        public ActionResult GetUserCacheJson(string applyId)
        {
            var data = CacheFactory.Cache().GetCache<List<ThreePeopleInfoEntity>>(applyId);
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
            var data = threepeoplecheckbll.GetEntity(keyValue);
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
            threepeoplecheckbll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <param name="ApplyUsers">��Ա��Ϣ</param>
        ///<param name="AuditInfo">�����Ϣ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, ThreePeopleCheckEntity entity, string ApplyUsers)
        {
            List<ThreePeopleInfoEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ThreePeopleInfoEntity>>(ApplyUsers);
            if(threepeoplecheckbll.SaveForm(keyValue, entity, list))
            {
                string status = "";
                Operator curUser = OperatorProvider.Provider.Current();
                ThreePeopleCheckEntity tp = threepeoplecheckbll.GetEntity(keyValue);
                string deptId = "";
                if (tp!=null)
                {
                    if (tp.ApplyType=="�ڲ�")
                    {
                        deptId = tp.BelongDeptId;
                    }
                    else
                    {
                        string sql = string.Format("select ENGINEERLETDEPTID from EPG_OUTSOURINGENGINEER t where id='{0}'", entity.ProjectId);
                        DataTable dt =new DepartmentBLL().GetDataTable(sql);
                        if (dt.Rows.Count > 0)
                        {
                            deptId = dt.Rows[0][0].ToString();
                        }
                    }
                }
                else
                {
                    deptId = curUser.DeptId;
                }
                entity.IsSumbit = 1;
                ManyPowerCheckEntity mp = threepeoplecheckbll.CheckAuditPower(curUser, out status, "���������", deptId, entity.Id);
                if (mp != null)
                {
                        entity.NodeId = mp.ID;
                        entity.IsOver = 0;
                        entity.Status = mp.FLOWNAME;
                        threepeoplecheckbll.SaveForm(keyValue, entity);
                }
                else
                {
                    entity.IsOver = 1;
                    entity.Status = "���̽���";
                    threepeoplecheckbll.SaveForm(keyValue, entity);
                }
            }
            return Success("�����ɹ���");
        }

        /// <summary>
        /// �����Ϣ
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <param name="ApplyUsers">��Ա��Ϣ</param>
        ///<param name="AuditInfo">�����Ϣ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult Audit(string keyValue, AptitudeinvestigateauditEntity AuditInfo)
        {
            ThreePeopleCheckEntity entity=threepeoplecheckbll.GetEntity(keyValue);
            Operator curUser = OperatorProvider.Provider.Current();
            string status="0";
            ManyPowerCheckEntity mp = threepeoplecheckbll.CheckAuditPower(curUser, out status, "���������", entity.CreateUserDeptId, keyValue);
            AuditInfo.AUDITPEOPLEID = curUser.UserId;
            AuditInfo.AUDITDEPTID = curUser.DeptId;
            AuditInfo.APTITUDEID = keyValue;
            AuditInfo.FlowId = entity.NodeId;
            if (mp!=null)
            {
                if (status=="0")
                {
                    return Error("�Բ�����û����˵�Ȩ��");
                }
                else
                {
                 
                    new AptitudeinvestigateauditBLL().SaveForm("", AuditInfo);
                    entity.NodeId =AuditInfo.AUDITRESULT=="1"?"-100":mp.ID;
                    entity.IsOver =0;
                    entity.IsSumbit = AuditInfo.AUDITRESULT == "1" ? 0 : 1;
                    entity.Status = AuditInfo.AUDITRESULT=="1"?"���˻�,�������ύ":mp.FLOWNAME;
                    threepeoplecheckbll.SaveForm(keyValue, entity, new List<ThreePeopleInfoEntity>(), AuditInfo);
                    if (AuditInfo.AUDITRESULT == "1")
                    {
                        new DepartmentBLL().ExecuteSql(string.Format("update EPG_APTITUDEINVESTIGATEAUDIT set disable='1' where aptitudeid='{0}'", keyValue));
                        ERCHTMS.Busines.JPush.JPushApi.PushMessage(entity.UserAccount, entity.CreateUserName, "ThreePeople", keyValue);
                    }
                    return Success("�����ɹ���");
                }
            }
            else
            {
                if (status=="0")
                {
                    return Error("�Բ�����û����˵�Ȩ��");
                }
                else
                {
                    new AptitudeinvestigateauditBLL().SaveForm("", AuditInfo);
                    entity.IsOver = AuditInfo.AUDITRESULT == "1" ? 0:1;
                    entity.IsSumbit = AuditInfo.AUDITRESULT == "1" ? 0 : 1;
                    entity.Status = AuditInfo.AUDITRESULT == "1" ? "���˻�,�������ύ" :"���̽���";
                    entity.NodeId = "-100";
                    threepeoplecheckbll.SaveForm(keyValue, entity, new List<ThreePeopleInfoEntity>(), AuditInfo);
                    if (AuditInfo.AUDITRESULT == "1")
                    {
                        ERCHTMS.Busines.JPush.JPushApi.PushMessage(entity.UserAccount, entity.CreateUserName, "ThreePeople", keyValue);
                        new DepartmentBLL().ExecuteSql(string.Format("update EPG_APTITUDEINVESTIGATEAUDIT set disable='1' where aptitudeid='{0}'", keyValue));
                    }
                    return Success("�����ɹ�");
                }
            }
          
        }
        #endregion

        /// <summary>
        /// ����������
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportUsers(string applyId)
        {
            if (OperatorProvider.Provider.Current().IsSystem)
            {
                return "��������Ա�޷������˹���";
            }
            var currUser = OperatorProvider.Provider.Current();
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
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                wb.Open(Server.MapPath("~/Resource/temp/" + fileName));
                Aspose.Cells.Cells cells = wb.Worksheets[0].Cells;
                DataTable dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn + 1, true);
              
                StringBuilder sb = new StringBuilder("begin \r\n");
                IList<ThreePeopleInfoEntity> list = new List<ThreePeopleInfoEntity>();
                for (int i = 0; i < dt.Rows.Count;i++ )
                {
                    DataRow dr=dt.Rows[i];
                    string userName = dr[0].ToString().Trim();
                    string idCard = dr[1].ToString().Trim();
                    string userType = dr[2].ToString().Trim();
                    if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(idCard) || string.IsNullOrEmpty(userType))
                    {
                        falseMessage += "</br>" + "��" + (i + 3) + "��ֵ���ڿ�,δ�ܵ���.";
                        error++;
                        continue;
                    }
                    //---****���֤��ȷ��֤*****--
                    if (!Regex.IsMatch(idCard, @"^(^d{15}$|^\d{18}$|^\d{17}(\d|X|x))$", RegexOptions.IgnoreCase))
                    {
                        falseMessage += "</br>" + "��" + (i + 3) + "�����֤�Ÿ�ʽ����,δ�ܵ���.";
                        error++;
                        continue;
                    }
                    if (list.Count(t => t.IdCard == idCard) > 0)
                    {
                        falseMessage += "</br>" + "��" + (i + 3) + "����Ա���֤��Ϣ�Ѵ���,δ�ܵ���.";
                        error++;
                        continue;
                    }
                    else
                    {
                        list.Add(new ThreePeopleInfoEntity
                        {
                            UserName = userName,
                            IdCard = idCard,
                            ApplyId = applyId,
                            OrgCode = currUser.OrganizeCode,
                            TicketType = userType
                        });
                    }

                }
                //sb.Append("end \r\n commit;");
                if (dt.Rows.Count>0)
                {
                    CacheFactory.Cache().WriteCache(list, applyId, DateTime.Now.AddMinutes(30));
                   // new DepartmentBLL().ExecuteSql(sb.ToString());
                }
                count = dt.Rows.Count;
                message = "����" + count + "����¼,�ɹ�����" + (count - error) + "����ʧ��" + error + "��";
                message += "</br>" + falseMessage;
            }
            return message;
        }

        #region ���ݵ���
        /// <summary>
        /// �����û��б�
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "�����������嵥")]
        public ActionResult ExportData(string queryJson)
        {
            UserBLL userBLL = new UserBLL();
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.sidx = "u.createdate";
            pagination.sord = "desc";
            pagination.rows = 100000000;
            pagination.p_kid = "u.USERID";
            pagination.p_fields = "REALNAME,GENDER,identifyid,MOBILE,DEPTNAME,fourpersontype";
            pagination.p_tablename = "v_userinfo u";
            string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "departmentcode", "organizecode");
            pagination.conditionJson = string.IsNullOrEmpty(where) ? "Account!='System'" : where;
            pagination.conditionJson += " and isfourperson='��'";
            var data = userBLL.GetPageList(pagination, queryJson);

            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "�������嵥";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "�������嵥.xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            ColumnEntity columnentity = new ColumnEntity();
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "realname", ExcelColumn = "����", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "gender", ExcelColumn = "�Ա�", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "identifyid", ExcelColumn = "���֤��", Alignment = "center",Width=200 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "mobile", ExcelColumn = "�绰", Alignment = "center", Width = 150 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "deptname", ExcelColumn = "��λ/����", Alignment = "center", Width = 200 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "fourpersontype", ExcelColumn = "���������", Alignment = "center" });




            //���õ�������
            // ExcelHelper.ExcelDownload(data, excelconfig);

            ExcelHelper.ExportByAspose(data, "�������嵥_"+DateTime.Now.ToString("yyyyMMdd"), excelconfig.ColumnEntity);
            return Success("�����ɹ���");
        }
        #endregion

    }
}
