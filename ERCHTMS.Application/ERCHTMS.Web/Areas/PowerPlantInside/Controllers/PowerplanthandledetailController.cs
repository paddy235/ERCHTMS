using ERCHTMS.Entity.PowerPlantInside;
using ERCHTMS.Busines.PowerPlantInside;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using System.Collections.Generic;
using System.Data;
using BSFramework.Util.Extension;
using ERCHTMS.Busines.OutsourcingProject;
using System.Linq;
using System;
using System.Web;
using Aspose.Words;
using ERCHTMS.Busines.HighRiskWork;
using ERCHTMS.Entity.OutsourcingProject;

namespace ERCHTMS.Web.Areas.PowerPlantInside.Controllers
{
    /// <summary>
    /// �� �����¹��¼�������Ϣ
    /// </summary>
    public class PowerplanthandledetailController : MvcControllerBase
    {
        private PowerplanthandledetailBLL powerplanthandledetailbll = new PowerplanthandledetailBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private UserBLL userbll = new UserBLL();
        private PowerplanthandleBLL powerplanthandlebll = new PowerplanthandleBLL();
        private PowerplantreformBLL powerplantreformbll = new PowerplantreformBLL();
        private PowerplantcheckBLL powerplantcheckbll = new PowerplantcheckBLL();
        private ScaffoldBLL scaffoldbll = new ScaffoldBLL();
        private AptitudeinvestigateauditBLL aptitudeinvestigateauditbll = new AptitudeinvestigateauditBLL();
            
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
            var data = powerplanthandledetailbll.GetList(queryJson);
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
            var data = powerplanthandledetailbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }


        /// <summary>
        /// ��ȡ�¹��¼�������Ϣ�б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="recid">�����¹��¼������¼ID</param>
        /// <returns></returns>
        public ActionResult GetHandleDetailListJson(Pagination pagination, string recid)
        {
            try
            {
                Operator user = OperatorProvider.Provider.Current();
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "a.ID";
                pagination.p_fields = "a.powerplanthandleid,a.rectificationmeasures,a.rectificationdutyperson,a.rectificationdutydept,a.APPLYSTATE,a.rectificationtime,b.id as PowerPlantReformId,a.reasonandproblem,'' as applystatename,a.signpersonname,e.outtransferuseraccount,e.intransferuseraccount,e.outtransferusername,e.intransferusername,a.flowdept,a.flowrole";
                pagination.p_tablename = @" bis_powerplanthandledetail a left join bis_powerplantreform b on a.id=b.powerplanthandledetailid and b.disable=0
                                            left join (select recid,flowid,outtransferuseraccount,intransferuseraccount,outtransferusername,intransferusername,row_number()  over(partition by recid,flowid order by createdate desc) as num from BIS_TRANSFERRECORD where disable=0) e on a.id=e.recid and e.num=1";
                pagination.conditionJson = string.Format("a.powerplanthandleid='{0}'", recid);
                pagination.sidx = "a.createdate";
                pagination.sord = "desc";
                var data = powerplanthandledetailbll.GetPageList(pagination, "");
                foreach (DataRow item in data.Rows)
                {
                    switch (item["applystate"].ToString())
                    {
                        case "0":
                            item["applystatename"] = "������";
                            break;
                        case "1":
                            item["applystatename"] = "�����";
                            break;
                        case "2":
                            item["applystatename"] = "��˲�ͨ��";
                            break;
                        case "3":
                            string rectificationdutyperson = item["rectificationdutyperson"].ToString(); //���ĸ�����
                            string outtransferusername = item["outtransferusername"].IsEmpty() ? "" : item["outtransferusername"].ToString();//ת��������
                            string intransferuseruser = item["intransferusername"].IsEmpty() ? "" : item["intransferusername"].ToString();//ת��������
                            string[] outtransferusernamelist = outtransferusername.Split(',');
                            string[] intransferuseruserlist = intransferuseruser.Split(',');
                            foreach (var temp in intransferuseruserlist)
                            {
                                if (!temp.IsEmpty() && !rectificationdutyperson.Contains(temp + ","))
                                {
                                    rectificationdutyperson += (temp + ",");//��ת�������˼�����������
                                }
                            }
                            foreach (var temp in outtransferusernamelist)
                            {
                                if (!temp.IsEmpty() && rectificationdutyperson.Contains(temp + ","))
                                {
                                    rectificationdutyperson = rectificationdutyperson.Replace(temp + ",", "");//��ת�������˴����������Ƴ�
                                }
                            }
                            item["applystatename"] = (rectificationdutyperson.Length > 18 ? rectificationdutyperson.Substring(0, 18) + "..." : rectificationdutyperson.ToString()) + "������";
                            break;
                        case "4":
                            string[] deptlist = item["flowdept"].ToString().Split(',');
                            string[] rolelist = item["flowrole"].ToString().Split(',');
                            IList<UserEntity> userlist = userbll.GetUserListByDeptId("'" + string.Join("','", deptlist) + "'", "'" + string.Join("','", rolelist) + "'", true, "");
                            string username = "";
                            if (userlist.Count > 0)
                            {
                                foreach (var temp in userlist)
                                {
                                    username += temp.RealName + ",";
                                }
                                username = string.IsNullOrEmpty(username) ? "" : username.Substring(0, username.Length - 1);
                            }
                            item["applystatename"] = (username.Length > 18 ? username.Substring(0, 18) + "..." : username.ToString()) + "������";
                            break;
                        case "5":
                            item["applystatename"] = "�����";
                            break;
                        case "6":
                            item["applystatename"] = (item["signpersonname"].ToString().Length > 18 ? item["signpersonname"].ToString().Substring(0, 18) + "..." : item["signpersonname"].ToString()) + "ǩ����";
                            break;
                        default:
                            break;
                    }
                    
                }
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
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// ��ȡǩ����
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSignPerson(string deptid)
        {
            try
            {
                Operator user = OperatorProvider.Provider.Current();
                string roles = dataitemdetailbll.GetItemValue(user.OrganizeId, "SignPersonRole");
                IList<UserEntity> userlist = new List<UserEntity>();
                if (string.IsNullOrEmpty(roles))
                {
                    userlist = userbll.GetUserListByRoleName("'" + deptid + "'", "'������','��ȫ����Ա','ר��'", true, "");
                }
                else
                {
                    string[] rolelist = roles.Split('|');
                    userlist = userbll.GetUserListByDeptId("'" + deptid + "'", "'" + string.Join("','", rolelist) + "'", true, "");
                }
                string username = "";
                string userid = "";
                if (userlist.Count > 0)
                {
                    foreach (var item in userlist)
                    {
                        username += item.RealName + ",";
                        userid += item.UserId + ",";
                    }
                    username = string.IsNullOrEmpty(username) ? "" : username.Substring(0, username.Length - 1);
                    userid = string.IsNullOrEmpty(userid) ? "" : userid.Substring(0, userid.Length - 1);
                }
                return Success("��ȡ���ݳɹ�", new { username = username, userid = userid });
            }
            catch (System.Exception ex)
            {
                return Error(ex.ToString());
            }
            
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
        [HandlerMonitor(6, "�¹��¼�������Ϣɾ��")]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            try
            {
                powerplanthandledetailbll.RemoveForm(keyValue);
                return Success("ɾ���ɹ���");
            }
            catch (System.Exception ex)
            {
                return Error(ex.ToString());
            }
            
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(5, "�¹��¼�������Ϣ����")]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, PowerplanthandledetailEntity entity)
        {
            try
            {
                powerplanthandledetailbll.SaveForm(keyValue, entity);
                if (entity.ApplyState == 3)
                {
                    powerplanthandlebll.UpdateApplyStatus(entity.PowerPlantHandleId);
                }
                return Success("�����ɹ���");
            }
            catch (System.Exception ex)
            {
                return Error(ex.ToString());
            }
            
        }


        /// <summary>
        /// �����¹��¼�������Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HandlerMonitor(0, "�����¹��¼�������Ϣ")]
        public ActionResult ExportSGSJCLXX(string keyValue)
        {
            try
            {
                var userInfo = OperatorProvider.Provider.Current();  //��ȡ��ǰ�û�
                var tempconfig = new TempConfigBLL().GetList("").Where(x => x.DeptCode == userInfo.OrganizeCode && x.ModuleCode == "SGSJCLJL").ToList();
                string tempPath = @"~/Resource/ExcelTemplate/�¹��¼�����ģ��-����ɽ.doc";
                var tempEntity = tempconfig.FirstOrDefault();
                string fileName = "�¹��¼�������Ϣ������_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
                if (tempconfig.Count > 0)
                {
                    if (tempEntity != null)
                    {
                        switch (tempEntity.ProessMode)
                        {
                            case "TY"://ͨ�ô���ʽ
                                tempPath = @"~/Resource/ExcelTemplate/�¹��¼�����ģ��-����ɽ.doc";
                                break;
                            case "GDXY"://��������
                                tempPath = @"~/Resource/ExcelTemplate/�¹��¼�����ģ��_��������.doc";
                                break;
                            case "HRCB"://������
                                tempPath = @"~/Resource/ExcelTemplate/�¹��¼�����ģ��-����ɽ.doc";
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    tempPath = @"~/Resource/ExcelTemplate/�¹��¼�����ģ��-����ɽ.doc";
                }
                ExportDataByCode(keyValue, tempPath, fileName);
                return Success("�����ɹ�!");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        private void ExportDataByCode(string keyValue, string tempPath, string fileName)
        {
            var userInfo = OperatorProvider.Provider.Current();  //��ȡ��ǰ�û�
            string strDocPath = Server.MapPath(tempPath);
            Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);
            DataTable dt = new DataTable("people");
            dt.Columns.Add("ApplyCode"); //���
            dt.Columns.Add("CreateDate"); //�����
            dt.Columns.Add("AccidentEventName");  //�¹��¼�����
            dt.Columns.Add("HappenTime");  //����ʱ��
            dt.Columns.Add("BelongDept");  //���ι���
            dt.Columns.Add("AccidentEventProperty");  //����
            dt.Columns.Add("SituationIntroduction");  //�������
            dt.Columns.Add("ReasonAndProblem");  //��¶ԭ������
            dt.Columns.Add("RectificationMeasures"); //������ʩ��Ҫ��
            dt.Columns.Add("SignDeptName");  //������λ
            dt.Columns.Add("RealSignPersonName");  //ǩ����
            dt.Columns.Add("ApproveDept");    //��˲���
            dt.Columns.Add("ApprovePerson");  //�����
            dt.Columns.Add("RectificationPerson");  //���ĸ�����
            dt.Columns.Add("RectificationDutyDept"); //���β���
            dt.Columns.Add("RectificationTime");  //�������
            dt.Columns.Add("RectificationSituation");  //��ʩ������
            dt.Columns.Add("RectificationPersonSignImg");  //��ʩ���������ǩ��
            dt.Columns.Add("ReformDate");  //��������
            dt.Columns.Add("CheckPerson1");  //���Ÿ�����
            dt.Columns.Add("CheckIdea1");  //�������1
            dt.Columns.Add("CheckIdea2");  //�������2
            dt.Columns.Add("CheckPerson2");   //������2
            dt.Columns.Add("CheckDate2");  //��������2
            dt.Columns.Add("CheckIdea3");  //�������3
            dt.Columns.Add("CheckPerson3");  //������3
            dt.Columns.Add("CheckDate3");  //��������3
            dt.Columns.Add("CheckDept3");  //���ղ���3
            dt.Columns.Add("CheckIdea4");    //�������4
            dt.Columns.Add("CheckPerson4");  //������4
            dt.Columns.Add("CheckDate4");  //��������4
            dt.Columns.Add("CheckDept4");  //���ղ���4

            HttpResponse resp = System.Web.HttpContext.Current.Response;
            string defaultimage = Server.MapPath("~/content/Images/no_1.png");
            DataRow row = dt.NewRow();
            PowerplanthandledetailEntity powerplanthandledetailentity = powerplanthandledetailbll.GetEntity(keyValue);
            PowerplanthandleEntity powerplanthandleentity = powerplanthandlebll.GetEntity(powerplanthandledetailentity.PowerPlantHandleId);
            AptitudeinvestigateauditEntity aptitudeinvestigateauditentity = aptitudeinvestigateauditbll.GetAuditList(powerplanthandleentity.Id).Where(t => t.Disable == "0").OrderByDescending(t => t.AUDITTIME).FirstOrDefault();
            PowerplantreformEntity powerplantreformentity = powerplantreformbll.GetList("").Where(t => t.Disable == 0 && t.PowerPlantHandleDetailId == keyValue).FirstOrDefault();
            IList<PowerplantcheckEntity> powerplantcheckentitylist = powerplantcheckbll.GetList("").Where(t => t.Disable == 0 && t.PowerPlantHandleDetailId == keyValue).OrderBy(t => t.CreateDate).ToList();

            row["ApplyCode"] = powerplanthandledetailentity.ApplyCode;
            row["CreateDate"] = Convert.ToDateTime(powerplanthandledetailentity.CreateDate).ToString("yyyy��MM��dd��");
            row["AccidentEventName"] = powerplanthandleentity.AccidentEventName;
            row["HappenTime"] = Convert.ToDateTime(powerplanthandleentity.HappenTime).ToString("yyyy��MM��dd��");
            row["BelongDept"] = powerplanthandleentity.BelongDept;
            row["AccidentEventProperty"] = scaffoldbll.getName(powerplanthandleentity.AccidentEventProperty, "AccidentEventProperty");
            row["SituationIntroduction"] = powerplanthandleentity.SituationIntroduction;
            row["ReasonAndProblem"] = powerplanthandledetailentity.ReasonAndProblem;
            row["RectificationMeasures"] = powerplanthandledetailentity.RectificationMeasures;
            row["SignDeptName"] = powerplanthandledetailentity.SignDeptName;
            row["RealSignPersonName"] = powerplanthandledetailentity.RealSignPersonName;
            row["ApproveDept"] = aptitudeinvestigateauditentity == null ? "" : aptitudeinvestigateauditentity.AUDITDEPT;
            row["ApprovePerson"] = aptitudeinvestigateauditentity == null ? "" : (System.IO.File.Exists(Server.MapPath("~/") + aptitudeinvestigateauditentity.AUDITSIGNIMG.Replace("../../", "").ToString()) ? Server.MapPath("~/") + aptitudeinvestigateauditentity.AUDITSIGNIMG.Replace("../../", "").ToString() : defaultimage);
            row["RectificationPerson"] = powerplantreformentity.RectificationPerson;
            row["RectificationDutyDept"] = powerplantreformentity.RectificationDutyDept;
            row["RectificationTime"] = Convert.ToDateTime(powerplantreformentity.RectificationTime).ToString("yyyy��MM��dd��");
            row["RectificationSituation"] = powerplantreformentity.RectificationSituation;
            row["RectificationPersonSignImg"] = System.IO.File.Exists(Server.MapPath("~/") + powerplantreformentity.RectificationPersonSignImg.Replace("../../", "").ToString()) ? Server.MapPath("~/") + powerplantreformentity.RectificationPersonSignImg.Replace("../../", "").ToString() : defaultimage;
            row["ReformDate"] = Convert.ToDateTime(powerplantreformentity.CreateDate).ToString("yyyy��MM��dd��");
            for (int i = 0; i < powerplantcheckentitylist.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        row["CheckPerson1"] = System.IO.File.Exists(Server.MapPath("~/") + powerplantcheckentitylist[i].AuditSignImg.Replace("../../", "").ToString()) ? Server.MapPath("~/") + powerplantcheckentitylist[i].AuditSignImg.Replace("../../", "").ToString() : defaultimage;
                        row["CheckIdea1"] = string.IsNullOrEmpty(powerplantcheckentitylist[i].AuditOpinion) ? "ͬ��" : powerplantcheckentitylist[i].AuditOpinion;
                        break;
                    case 1:
                        row["CheckIdea2"] = powerplantcheckentitylist[i].AuditOpinion;
                        row["CheckPerson2"] = System.IO.File.Exists(Server.MapPath("~/") + powerplantcheckentitylist[i].AuditSignImg.Replace("../../", "").ToString()) ? Server.MapPath("~/") + powerplantcheckentitylist[i].AuditSignImg.Replace("../../", "").ToString() : defaultimage;
                        row["CheckDate2"] = Convert.ToDateTime(powerplantcheckentitylist[i].AuditTime).ToString("yyyy��MM��dd��");
                        break;
                    case 2:
                        row["CheckIdea3"] = powerplantcheckentitylist[i].AuditOpinion;
                        row["CheckPerson3"] = System.IO.File.Exists(Server.MapPath("~/") + powerplantcheckentitylist[i].AuditSignImg.Replace("../../", "").ToString()) ? Server.MapPath("~/") + powerplantcheckentitylist[i].AuditSignImg.Replace("../../", "").ToString() : defaultimage;
                        row["CheckDate3"] = Convert.ToDateTime(powerplantcheckentitylist[i].AuditTime).ToString("yyyy��MM��dd��");
                        row["CheckDept3"] = powerplantcheckentitylist[i].AuditDept;
                        break;
                    case 3:
                        row["CheckIdea4"] = powerplantcheckentitylist[i].AuditOpinion;
                        row["CheckPerson4"] = System.IO.File.Exists(Server.MapPath("~/") + powerplantcheckentitylist[i].AuditSignImg.Replace("../../", "").ToString()) ? Server.MapPath("~/") + powerplantcheckentitylist[i].AuditSignImg.Replace("../../", "").ToString() : defaultimage;
                        row["CheckDate4"] = Convert.ToDateTime(powerplantcheckentitylist[i].AuditTime).ToString("yyyy��MM��dd��");
                        row["CheckDept4"] = powerplantcheckentitylist[i].AuditDept;
                        break;
                    default:
                        break;
                }
            }

            dt.Rows.Add(row);
            doc.MailMerge.Execute(dt);
            doc.MailMerge.DeleteFields();
            doc.Save(resp, Server.UrlEncode(fileName), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
        }
        #endregion
    }
}
