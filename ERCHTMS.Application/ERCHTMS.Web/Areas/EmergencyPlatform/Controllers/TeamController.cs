using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Cache;
using ERCHTMS.Code;
using ERCHTMS.Entity.AuthorizeManage;
using ERCHTMS.Entity.BaseManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using BSFramework.Util.Offices;
using System.Drawing;
using System.Web;
using ERCHTMS.Entity.SystemManage.ViewModel;
using System.Text.RegularExpressions;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.SystemManage;
using BSFramework.Util.Attributes;
using ERCHTMS.Busines.EmergencyPlatform;
using ERCHTMS.Entity.EmergencyPlatform;
using System.Linq;
using System.Linq.Expressions;

namespace ERCHTMS.Web.Areas.EmergencyPlatform.Controllers
{
    /// <summary>
    /// �� ����Ӧ������
    /// </summary>
    public class TeamController : MvcControllerBase
    {
        private TeamBLL teambll = new TeamBLL();
        private UserBLL userBLL = new UserBLL();
        private PostBLL postBLL = new PostBLL();
        private DepartmentBLL departBLL = new DepartmentBLL();
        private OrganizeBLL orgBLL = new OrganizeBLL();


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
            pagination.p_kid = "TEAMID";
            pagination.p_fields = "TEAMID as Id,CREATEUSERID, Mobile, POSTID,USERID,PostName,UserFullName,DepartName,DepartId,CREATEUSERDEPTCODE as departmentcode,CREATEUSERORGCODE as  organizecode,orgname,orgcode,remark";
            pagination.p_tablename = "v_mae_team t";
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


            var watch = CommonHelper.TimerStart();
            var data = teambll.GetPageList(pagination, queryJson);
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
            var data = teambll.GetList(queryJson);
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
            var data = teambll.GetEntity(keyValue);
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
        public string ImportTeam(string PostId)
        {
            //if (OperatorProvider.Provider.Current().IsSystem)
            //{
            //    return "��������Ա�޴˲���Ȩ��";
            //}
            string orgId = OperatorProvider.Provider.Current().OrganizeId;//������˾
            string deptId = PostId;//��������
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
                    //---****ֵ���ڿ���֤*****--
                    if (string.IsNullOrEmpty(dt.Rows[i][0].ToString()) || string.IsNullOrEmpty(dt.Rows[i][1].ToString()) || string.IsNullOrEmpty(dt.Rows[i][2].ToString()) || string.IsNullOrEmpty(dt.Rows[i][3].ToString()))
                    {
                        falseMessage += "</br>" + "��" + (i + 2) + "��ֵ���ڿ�,δ�ܵ���.";
                        error++;
                        continue;
                    }
                    //Ӧ����֯����
                    string orgname = dt.Rows[i][0].ToString();
                    DataItemCache dataItemCache = new DataItemCache();
                    var orgItem = dataItemCache.GetDataItemList().Where(e => e.EnCode == "MAE_ORG" && e.ItemName == orgname).FirstOrDefault();
                    string OrgCode = orgItem == null ? "" : orgItem.ItemCode;
                    //if (orgItem == null)
                    //{
                    //    falseMessage += "��" + i + "�е���ʧ��,Ӧ����֯���������ڣ�</br>";
                    //    error++;
                    //    continue;
                    //}
                    //Ӧ��ְ��
                    string postname = dt.Rows[i][1].ToString();
                    var post = dataItemCache.GetDataItemList().Where(e => e.EnCode == "MAE_TEAM_ZW" && e.ItemName == postname).FirstOrDefault();
                    //if (post == null)
                    //{
                    //    falseMessage += "��" + i + "�е���ʧ��,Ӧ��ְ�񲻴��ڣ�</br>";
                    //    error++;
                    //    continue;
                    //}
                    string postid = post == null ? "" : post.ItemValue;
                    //����
                    string usernmae = dt.Rows[i][2].ToString();
                    
                    //��������
                    string departname = dt.Rows[i][3].ToString();
                    var depart = departBLL.GetList().Where(e => e.FullName == departname && e.OrganizeId == orgId).FirstOrDefault();
                    var org = orgBLL.GetList().Where(e => e.FullName == departname && e.OrganizeId == orgId).FirstOrDefault();
                    if (depart == null && org == null)
                    {
                        falseMessage += "��" + i + "�е���ʧ��,�������Ų����ڣ�</br>";
                        error++;
                        continue;
                    }
                    //��ϵ��ʽ
                    string mobile = dt.Rows[i][4].ToString();
                    //��ע
                    string remark = dt.Rows[i][5].ToString();

                    Expression<Func<UserEntity, bool>> condition = e => e.OrganizeId == orgId && e.RealName == usernmae && e.DepartmentId == depart.DepartmentId;
                    var user = userBLL.GetListForCon(condition).FirstOrDefault();
                    if (user == null)
                    {
                        falseMessage += "��" + i + "�е���ʧ��,���û�������Ϣ�����ڣ�</br>";
                        error++;
                        continue;
                    }


                    try
                    {
                        var item = new TeamEntity { OrgName = orgname, OrgCode = OrgCode, MOBILE = mobile, USERFULLNAME = usernmae, POSTNAME = postname, POSTID = postid, DEPARTID = depart == null ? org.OrganizeId : depart.DepartmentId, DEPARTNAME = departname, USERID = user.UserId, Remark = remark };
                        var listcheck = teambll.GetList(string.Format(" and USERFULLNAME='{0}' and Mobile='{1}' and DepartId='{2}' and POSTID='{3}'", item.USERFULLNAME, item.MOBILE, item.DEPARTID, item.POSTID));
                        if (listcheck != null && listcheck.Count() > 0)
                        {
                            falseMessage += "Ӧ��ְ��:" + item.POSTNAME + ",��������" + item.DEPARTNAME + ",����:" + item.USERFULLNAME + "����Ϣ�Ѿ����ڣ�";
                            //return Error("���������Ѵ��ڡ�");
                        }
                        else
                            teambll.SaveForm("", item);

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
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            teambll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, TeamEntity entity)
        {
            teambll.SaveForm(keyValue, entity);

            return Success("�����ɹ���");
        }

        [HttpPost]
        public ActionResult SaveListForm()
        {
            string data = Request["param"];
            var list = data.ToObject<List<TeamEntity>>();
            string errormessage = "";
            foreach (var item in list)
            {
                if (string.IsNullOrEmpty(item.TEAMID))
                {
                    var listcheck = teambll.GetList(string.Format(" and USERFULLNAME='{0}' and Mobile='{1}' and DepartId='{2}' and POSTNAME='{3}'", item.USERFULLNAME, item.MOBILE, item.DEPARTID, item.POSTNAME));
                    if (listcheck != null && listcheck.Count() > 0)
                    {
                        errormessage += "Ӧ��ְ��:" + item.POSTNAME + ",����:" + item.USERFULLNAME + "����Ϣ�Ѿ�����,����ʧ�ܣ�<BR>";
                        //return Error("���������Ѵ��ڡ�");
                    }
                }
                else
                {
                    var listcheck = teambll.GetList(string.Format(" and  USERFULLNAME='{0}' and Mobile='{1}' and DepartId='{2}' and POSTNAME='{3}' and TEAMID!='{4}'", item.USERFULLNAME, item.MOBILE, item.DEPARTID, item.POSTNAME, item.TEAMID));
                    if (listcheck != null && listcheck.Count() > 0)
                    {
                        errormessage += "Ӧ��ְ��:" + item.POSTNAME + ",����:" + item.USERFULLNAME + "����Ϣ�Ѿ�����,����ʧ�ܣ�<BR>";
                        //return Error("���������Ѵ��ڡ�");
                    }

                   
                }
            }
            if (errormessage.Length > 0)
                return Error(errormessage);
            foreach (var item in list)
            {
                 teambll.SaveForm(item.TEAMID, item);
            }
  
            return Success("�����ɹ���");
        }

        #endregion

        #region ���ݵ���
        /// <summary>
        /// �����û��б�
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "�����û�����")]
        public ActionResult ExportTeamList(string condition, string queryJson)
        {
            Pagination pagination = new Pagination();
            queryJson = queryJson ?? "";
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "TEAMID";
            pagination.p_fields = " OrgName,PostName,UserFullName,Mobile,DepartName,remark";
            pagination.p_tablename = "V_MAE_TEAM t";
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
            #endregion
            var data = teambll.GetPageList(pagination, queryJson);

            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "Ӧ������";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "Ӧ������.xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();
            listColumnEntity.Add(new ColumnEntity() { Column = "orgname", ExcelColumn = "Ӧ����֯����" });
            listColumnEntity.Add(new ColumnEntity() { Column = "postname", ExcelColumn = "Ӧ��ְ��" });
            listColumnEntity.Add(new ColumnEntity() { Column = "userfullname", ExcelColumn = "����" });
            listColumnEntity.Add(new ColumnEntity() { Column = "mobile", ExcelColumn = "��ϵ��ʽ" });
            listColumnEntity.Add(new ColumnEntity() { Column = "departname", ExcelColumn = "��������" });
            listColumnEntity.Add(new ColumnEntity() { Column = "remark", ExcelColumn = "��ע" });
            excelconfig.ColumnEntity = listColumnEntity;

            //���õ�������
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("�����ɹ���");
        }
        #endregion


    }
}
