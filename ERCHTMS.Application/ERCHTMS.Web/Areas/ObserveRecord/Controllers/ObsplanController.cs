
using ERCHTMS.Busines.Observerecord;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Busines.SystemManage;
using System.Linq;
using ERCHTMS.Entity.Observerecord;
using System;
using System.Web;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Web.Areas.Observerecord.Controllers
{
    /// <summary>
    /// �� �����۲�ƻ�
    /// </summary>
    public class ObsplanController : MvcControllerBase
    {
        private ObsplanBLL obsplanbll = new ObsplanBLL();
        private ObsplanworkBLL obsplanworkbll = new ObsplanworkBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
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
        public ActionResult FeedBackForm()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CopyPlanIndex()
        {
            return View();
        }
        /// <summary>
        /// ̨��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult StandingIndex()
        {
            return View();
        }
        /// <summary>
        /// ̨������ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult StandingShow()
        {
            return View();
        }
        /// <summary>
        ///����ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ImportPlanData()
        {
            return View();
        }

        /// <summary>
        ///ѡ��۲�ƻ�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SelectObsPlan()
        {
            return View();
        }
        #endregion

        #region ��ȡ����

        /// <summary>
        /// ̨��ҳ���ȡ��ҳ�б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetStandingPageJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var tableClass = "bis_obsplan_tz t";
            pagination.p_kid = "t.id tid";
            pagination.p_fields = @" t.planyear,
                                       t.plandept,
                                       t.planspeciaty,
                                       t.plandeptcode,t.plandeptid,
                                       t.planspeciatycode,
                                       t.planarea,t.planareacode,
                                       t.planlevel,p.risklevel,
                                       p.workname fjname,
                                       t.workname,p.id pid,t.oldplanid,
                                       p.obsperson,p.oldworkid,
                                       p.obspersonid,
                                       p.obsnum,p.obsnumtext,
                                       p.obsmonth,
                                       t.createuserid,
                                       t.createuserdeptcode,
                                       t.createuserorgcode,
                                       t.createdate,t.iscommit,p.remark,null status";

            Operator currUser = OperatorProvider.Provider.Current();
            pagination.conditionJson = "t.iscommit='1' and t.ispublic ='1'";
            //DataItemModel ehsDepart = dataitemdetailbll.GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == currUser.OrganizeId&&p.ItemValue==currUser.DeptCode).ToList().FirstOrDefault();
            //if (ehsDepart != null) {
            //    tableClass = "bis_obsplan_commitehs t";
            //}
            //if (currUser.RoleName.Contains("��������"))
            //{
            //    tableClass = "bis_obsplan_commitehs t";
            //}
            //if (currUser.RoleName.Contains("��˾��") && currUser.RoleName.Contains("��ȫ����Ա"))
            //{
            //    tableClass = "bis_obsplan_fb t";
            //}
            pagination.p_tablename = string.Format(@"{0}
                                        left join bis_obsplanwork p
                                            on p.planid = t.id", tableClass);
            pagination.sidx = pagination.sidx + " " + pagination.sord + ",t.id";
            if (!currUser.IsSystem)
            {
                if (currUser.RoleName.Contains("רҵ���û�") || currUser.RoleName.Contains("���鼶�û�"))
                {
                    var d = new DepartmentBLL().GetParentDeptBySpecialArgs(currUser.ParentId, "����");
                    if (d != null)
                    {
                        pagination.conditionJson += " and t.plandeptcode like '" + d.EnCode + "%'";
                    }
                    else
                    {
                        pagination.conditionJson += " and 0=1";
                    }
                }
                else
                {
                    string authType = new AuthorizeBLL().GetOperAuthorzeType(currUser, "e4097233-5867-4c46-bba9-f052d512ffd8", "search");
                    if (!string.IsNullOrEmpty(authType))
                    {

                        switch (authType)
                        {
                            case "1":
                                pagination.conditionJson += " and  t.createuserid='" + currUser.UserId + "'";
                                break;
                            case "2":
                                pagination.conditionJson += " and t.plandeptcode='" + currUser.DeptCode + "'";
                                break;
                            case "3":
                                pagination.conditionJson += " and t.plandeptcode like '" + currUser.DeptCode + "%'";
                                break;
                            case "4":
                                pagination.conditionJson += " and t.plandeptcode like '" + currUser.OrganizeCode + "%'";
                                break;
                            case "5":
                                pagination.conditionJson += string.Format(" and t.plandeptcode in(select encode from BASE_DEPARTMENT where deptcode like '{0}%')", currUser.NewDeptCode);
                                break;
                        }
                    }
                    else
                    {
                        pagination.conditionJson += " and 0=1";
                    }
                }
            }
            var queryParam = queryJson.ToJObject();
            var PlanYear = queryParam["PlanYear"].ToString();
            var data = obsplanbll.GetPageList(pagination, queryJson);
            for (int i = 0; i < data.Rows.Count; i++)
            {
                var dt = obsplanbll.GetObsRecordIsExist(data.Rows[i]["oldplanid"].ToString(), data.Rows[i]["oldworkid"].ToString(), PlanYear);
                var obsmonth = data.Rows[i]["obsmonth"].ToString().Split(',');
                if (dt.Rows.Count > 0)
                {
                    for (int j = 0; j < obsmonth.Length; j++)
                    {

                        if (DateTime.Now.Year.ToString() == PlanYear && DateTime.Now.Month <= Convert.ToInt32(obsmonth[j]))
                        {
                            if (Convert.ToInt32(dt.Rows[0]["m" + obsmonth[j]]) > 0)
                            {
                                data.Rows[i]["status"] += "1,";
                            }
                            else
                            {
                                if (DateTime.Now.Month == Convert.ToInt32(obsmonth[j]))
                                {
                                    var currTime = DateTime.Now;
                                    var lastTime = Convert.ToDateTime(DateTime.Now.AddMonths(1).ToString("yyyy-MM-01")).AddDays(-1);
                                    if (DateTime.Compare(lastTime, currTime) <= 5)
                                    {
                                        data.Rows[i]["status"] += "2,";
                                    }
                                    else
                                    {
                                        data.Rows[i]["status"] += "3,";
                                    }
                                }
                                else
                                {
                                    data.Rows[i]["status"] += "3,";
                                }
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[0]["m" + obsmonth[j]]) > 0)
                            {
                                data.Rows[i]["status"] += "1,";
                            }
                            else
                            {
                                data.Rows[i]["status"] += "4,";
                            }
                        }

                    }
                    if (!string.IsNullOrWhiteSpace(data.Rows[i]["status"].ToString()))
                    {
                        data.Rows[i]["status"] = data.Rows[i]["status"].ToString().Substring(0, data.Rows[i]["status"].ToString().Length - 1);
                    }
                }
                else
                {
                    for (int j = 0; j < obsmonth.Length; j++)
                    {

                        if (DateTime.Now.Year.ToString() == PlanYear && DateTime.Now.Month <= Convert.ToInt32(obsmonth[j]))
                        {
                            if (DateTime.Now.Month == Convert.ToInt32(obsmonth[j]))
                            {
                                var currTime = DateTime.Now;
                                var lastTime = Convert.ToDateTime(DateTime.Now.AddMonths(1).ToString("yyyy-MM-01")).AddDays(-1);
                                if (DateTime.Compare(lastTime, currTime) <= 5)
                                {
                                    data.Rows[i]["status"] += "2,";
                                }
                                else
                                {
                                    data.Rows[i]["status"] += "3,";
                                }
                            }
                            else
                            {
                                data.Rows[i]["status"] += "3,";
                            }
                        }
                        else
                        {

                            data.Rows[i]["status"] += "4,";
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(data.Rows[i]["status"].ToString()))
                    {
                        data.Rows[i]["status"] = data.Rows[i]["status"].ToString().Substring(0, data.Rows[i]["status"].ToString().Length - 1);
                    }
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
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var tableClass = "bis_obsplan t";
            pagination.p_kid = "t.id tid";
            pagination.p_fields = @" t.planyear,
                                       t.plandept,
                                       t.planspeciaty,
                                       t.plandeptcode,
                                       t.planspeciatycode,
                                       t.planarea,
                                       t.planlevel,p.risklevel,
                                       p.workname fjname,
                                       t.workname,
                                       p.obsperson,
                                       p.obspersonid,
                                       p.obsnum,p.obsnumtext,
                                       p.obsmonth,t.ispublic,
                                       t.createuserid,
                                       t.createuserdeptcode,
                                       t.createuserorgcode,
                                       t.createdate,t.iscommit,p.remark";

            Operator currUser = OperatorProvider.Provider.Current();
            pagination.conditionJson = "1=1";
            //DataItemModel ehsDepart = dataitemdetailbll.GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == currUser.OrganizeId&&p.ItemValue==currUser.DeptCode).ToList().FirstOrDefault();
            //if (ehsDepart != null) {
            //    tableClass = "bis_obsplan_commitehs t";
            //}
            if (currUser.RoleName.Contains("��������"))
            {
                pagination.p_fields += ",t.oldplanid";
                tableClass = "bis_obsplan_commitehs t";
            }
            if (currUser.RoleName.Contains("��˾��"))
            {
                pagination.p_fields += ",t.oldplanid";
                tableClass = "bis_obsplan_fb t";
            }
            pagination.p_tablename = string.Format(@"{0}
                                        left join bis_obsplanwork p
                                            on p.planid = t.id", tableClass);
            pagination.sidx = pagination.sidx + " " + pagination.sord + ",t.id";
            if (!currUser.IsSystem)
            {
                if (currUser.RoleName.Contains("רҵ���û�") || currUser.RoleName.Contains("���鼶�û�"))
                {
                    var d = new DepartmentBLL().GetParentDeptBySpecialArgs(currUser.ParentId, "����");
                    if (d != null)
                    {
                        pagination.conditionJson += " and t.plandeptcode like '" + d.EnCode + "%'";
                    }
                    else
                    {
                        pagination.conditionJson += " and 0=1";
                    }
                }
                else
                {
                    string authType = new AuthorizeBLL().GetOperAuthorzeType(currUser, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                    if (!string.IsNullOrEmpty(authType))
                    {

                        switch (authType)
                        {
                            case "1":
                                pagination.conditionJson += " and  t.createuserid='" + currUser.UserId + "'";
                                break;
                            case "2":
                                pagination.conditionJson += " and t.plandeptcode='" + currUser.DeptCode + "'";
                                break;
                            case "3":
                                pagination.conditionJson += " and t.plandeptcode like '" + currUser.DeptCode + "%'";
                                break;
                            case "4":
                                pagination.conditionJson += " and t.plandeptcode like '" + currUser.OrganizeCode + "%'";
                                break;
                            case "5":
                                pagination.conditionJson += string.Format(" and t.plandeptcode in(select encode from BASE_DEPARTMENT where deptcode like '{0}%')", currUser.NewDeptCode);
                                break;
                        }
                    }
                    else
                    {
                        pagination.conditionJson += " and 0=1";
                    }
                }

            }
            var data = obsplanbll.GetPageList(pagination, queryJson);
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
        /// ��ȡ����б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetFeedBackList(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var tableClass = "bis_obsfeedback t";
            pagination.p_kid = "t.id";
            pagination.p_fields = @" t.acceptdept,
                                       t.acceptdeptcode,
                                       t.suggest,
                                       t.acceptdeptid,
                                       t.createuserid, t.createusername,
                                       t.createuserdeptcode,
                                       t.createuserorgcode,
                                       t.createdate";

            Operator currUser = OperatorProvider.Provider.Current();
            pagination.conditionJson = "1=1";

            if (currUser.RoleName.Contains("��������"))
            {
                tableClass = "bis_obsfeedback_ehs t";
            }
            if (currUser.RoleName.Contains("��˾��") && currUser.RoleName.Contains("��ȫ����Ա"))
            {
                tableClass = "bis_obsfeedback_fb t";
            }

            pagination.p_tablename = string.Format(@"{0}", tableClass);
            if (currUser.RoleName.Contains("רҵ���û�") || currUser.RoleName.Contains("���鼶�û�"))
            {
                var d = new DepartmentBLL().GetParentDeptBySpecialArgs(currUser.ParentId, "����");
                if (d != null)
                {
                    pagination.conditionJson += " and t.acceptdeptcode like '" + d.EnCode + "%'";
                }
                else
                {
                    pagination.conditionJson += " and 0=1";
                }
            }
            var data = obsplanbll.GetPageList(pagination, queryJson);
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
            var data = obsplanbll.GetList(queryJson);
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
            Operator currUser = OperatorProvider.Provider.Current();
            //var data = obsplanbll.GetEntity(keyValue);
            if (currUser.RoleName.Contains("��������"))
            {
                var data = obsplanbll.GetEHSEntity(keyValue);
                return ToJsonResult(data);
            }
            if (currUser.RoleName.Contains("��˾��"))
            {
                var data = obsplanbll.GetFBEntity(keyValue);
                return ToJsonResult(data);
            }
            var data1 = obsplanbll.GetEntity(keyValue);
            return ToJsonResult(data1);
        }
        /// <summary>
        /// ̨��ҳ�������ȡ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetStandingFormJson(string keyValue)
        {
            var data = obsplanbll.GetTZEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ���ݹ۲�ƻ�Id������ֽ�Id��ȡ��Ӧ��Ϣ
        /// </summary>
        /// <param name="PlanId">�ƻ�id </param>
        /// <param name="PlanFjId">����ֽ�Id</param>
        /// <param name="PlanMonth">�۲��·�</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPlanById(string PlanId, string PlanFjId, string PlanMonth)
        {
            var data = obsplanbll.GetPlanById(PlanId, PlanFjId, PlanMonth);
            return ToJsonResult(data);
        }


         /// <summary>
         /// ��ѯ��һ���ڵ�
         /// </summary>
         /// <param name="parentid"></param>
         /// <param name="nature"></param>
         /// <returns></returns>
        public ActionResult GetParentDeptBySpecialArgs(string parentid, string nature)
        {

            var deptEntity= new DepartmentBLL().GetParentDeptBySpecialArgs(parentid, nature);
            return ToJsonResult(deptEntity);
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
            obsplanbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, ObsplanEntity entity)
        {
            var worklist = obsplanworkbll.GetList().Where(x => x.PlanId == keyValue).ToList();
            for (int i = 0; i < worklist.Count; i++)
            {
                if (worklist[i].RiskLevel == "IV��" || worklist[i].RiskLevel == "V��") {
                    entity.PlanLevel = "��˾��";
                    break;
                }
            }
            obsplanbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveEHSForm(string keyValue, ObsplanEHSEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.PlanLevel)) {
                var worklist = obsplanworkbll.GetList().Where(x => x.PlanId == entity.ID).ToList();
                for (int i = 0; i < worklist.Count; i++)
                {
                    if (worklist[i].RiskLevel == "IV��" || worklist[i].RiskLevel == "V��")
                    {
                        entity.PlanLevel = "��˾��";
                        break;
                    }
                }
            }
            obsplanbll.SaveEHSForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveFbForm(string keyValue, ObsplanFBEntity entity)
        {
            obsplanbll.SaveFBForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #region �������
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveFeedBackForm(string keyValue, ObsFeedBackEntity entity)
        {
            obsplanbll.SaveFeedBackForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveFeedBackEHSForm(string keyValue, ObsFeedBackEHSEntity entity)
        {
            //�ϼ�����������ͬ��
            obsplanbll.SaveFeedBackEHSForm(keyValue, entity);
            ObsFeedBackEntity feedback = new ObsFeedBackEntity();
            feedback.AcceptDept = entity.AcceptDept;
            feedback.AcceptDeptId = entity.AcceptDeptId;
            feedback.AcceptDeptCode = entity.AcceptDeptCode;
            feedback.Suggest = entity.Suggest;
            obsplanbll.SaveFeedBackForm("", feedback);
            return Success("�����ɹ���");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveFeedBackFbForm(string keyValue, ObsFeedBackFBEntity entity)
        {
            //�ϼ�����������ͬ��
            obsplanbll.SaveFeedBackFBForm(keyValue, entity);
            ObsFeedBackEHSEntity ehs = new ObsFeedBackEHSEntity();
            ObsFeedBackEntity feedback = new ObsFeedBackEntity();
            ehs.AcceptDept = feedback.AcceptDept = entity.AcceptDept;
            ehs.AcceptDeptId = feedback.AcceptDeptId = entity.AcceptDeptId;
            ehs.AcceptDeptCode = feedback.AcceptDeptCode = entity.AcceptDeptCode;
            ehs.Suggest = feedback.Suggest = entity.Suggest;
            obsplanbll.SaveFeedBackEHSForm("", ehs);
            obsplanbll.SaveFeedBackForm("", feedback);
            return Success("�����ɹ���");
        }
        #endregion
        /// <summary>
        /// �ύ����һ��
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult CommitEhsData()
        {
            Operator currUser = OperatorProvider.Provider.Current();
            if (obsplanbll.CommitEhsData(currUser))
            {
                return Success("�����ɹ���");
            }
            else
            {
                return Error("����ʧ�ܡ�");
            }
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult SetPlanLevelSave(string keyValue, string OldPlanId,string PlanLevel) {
            var ehsEntity = obsplanbll.GetEHSEntity(keyValue);
            if (ehsEntity != null)
            {
                ehsEntity.PlanLevel = PlanLevel;
                if (!string.IsNullOrWhiteSpace(OldPlanId))
                {
                    var oldEntity = obsplanbll.GetEntity(OldPlanId);
                    if (oldEntity != null)
                    {
                        oldEntity.PlanLevel = PlanLevel;
                        obsplanbll.SaveForm(oldEntity.ID, oldEntity);
                    }
                }
                obsplanbll.SaveEHSForm(ehsEntity.ID, ehsEntity);
                return Success("�����ɹ���");
            }
            else {
                return Error("��ȡ����ʧ�ܡ�");
            }
           
            
        
        }

        /// <summary>
        /// �۲�ƻ�����
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult ExportData(string queryJson, string fileName)
        {
            Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
            string fName = "�۲�ƻ�_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            wb.Open(Server.MapPath("~/Resource/ExcelTemplate/��Ȱ�ȫ��Ϊ�۲칤���ƻ�����ģ��.xlsx"));
            var queryParam = queryJson.ToJObject();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 10000000;
            pagination.sidx = "t.createdate";
            pagination.sord = "desc";

            pagination.p_kid = "t.id";
            pagination.p_fields = @"t.plandept,t.planarea,t.workname,t.planlevel,p.workname fjname,p.risklevel,
                                       p.obsnumtext,p.obsperson,null m1,null m2,null m3,null m4,null m5,null m6,
                                        null m7,null m8,null m9,null m10,null m11,null m12,p.remark,p.obsmonth,p.id wid,t.oldplanid,p.oldworkid";
            pagination.p_tablename = string.Format(@"bis_obsplan_tz t left join bis_obsplanwork p on p.planid = t.id");
            pagination.conditionJson = "1=1";
            if (!user.IsSystem)
            {
                if (user.RoleName.Contains("רҵ���û�") || user.RoleName.Contains("���鼶�û�"))
                {
                    var d = new DepartmentBLL().GetList().Where(x => x.DepartmentId == user.ParentId).FirstOrDefault();
                    if (d != null)
                    {
                        if (d.Nature == "����")
                        {
                            pagination.conditionJson += " and t.plandeptcode like '" + d.EnCode + "%'";
                        }
                        else
                        {
                            var d1 = new DepartmentBLL().GetList().Where(x => x.DepartmentId == d.ParentId).FirstOrDefault();
                            if (d1.Nature == "����")
                            {
                                pagination.conditionJson += " and t.plandeptcode like '" + d1.EnCode + "%'";
                            }
                            else
                            {
                                pagination.conditionJson += " and 0=1";
                            }
                        }
                    }
                    else
                    {
                        pagination.conditionJson += " and 0=1";
                    }
                }
                else
                {
                    //���ݵ�ǰ�û���ģ���Ȩ�޻�ȡ��¼
                    string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "t.createuserdeptcode", "t.createuserorgcode");
                    if (!string.IsNullOrEmpty(where))
                    {
                        pagination.conditionJson += " and " + where;
                    }
                }
            }
            var data = obsplanbll.GetPageList(pagination, queryJson);
            //��ѯ�۲��¼�Ƿ���������˼�¼
            for (int i = 0; i < data.Rows.Count; i++)
            {
                var dt = obsplanbll.GetObsRecordIsExist(data.Rows[i]["oldplanid"].ToString(), data.Rows[i]["oldworkid"].ToString(), queryParam["PlanYear"].ToString());
                var obsmonth = data.Rows[i]["obsmonth"].ToString().Split(',');
                if (dt.Rows.Count > 0)
                {
                    for (int j = 0; j < obsmonth.Length; j++)
                    {

                        if (DateTime.Now.Year.ToString() == queryParam["PlanYear"].ToString() && DateTime.Now.Month <= Convert.ToInt32(obsmonth[j]))
                        {
                            if (Convert.ToInt32(dt.Rows[0]["m" + obsmonth[j]]) > 0)
                            {
                                data.Rows[i]["m" + obsmonth[j]] = "��";
                            }
                            else
                            {
                                data.Rows[i]["m" + obsmonth[j]] = "��";
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[0]["m" + obsmonth[j]]) > 0)
                            {
                                data.Rows[i]["m" + obsmonth[j]] = "��";
                            }
                            else
                            {
                                data.Rows[i]["m" + obsmonth[j]] = "��";
                            }
                        }

                    }
                }
                else
                {
                    for (int j = 0; j < obsmonth.Length; j++)
                    {

                        if (DateTime.Now.Year.ToString() == queryParam["PlanYear"].ToString() && DateTime.Now.Month <= Convert.ToInt32(obsmonth[j]))
                        {
                            data.Rows[i]["m" + obsmonth[j]] = "��";
                        }
                        else
                        {

                            data.Rows[i]["m" + obsmonth[j]] = "��";
                        }
                    }
                }
            }
            var cells = wb.Worksheets[0].Cells;
            int Colnum = data.Columns.Count;
            int Rownum = data.Rows.Count;
            for (int i = 0; i < Rownum; i++)
            {
                for (int k = 0; k < Colnum - 5; k++)
                {
                    if (k == 0)
                    {
                        cells[4 + i, k].PutValue(i + 1);
                    }
                    else
                    {
                        cells[4 + i, k].PutValue(data.Rows[i][k].ToString());
                    }
                }
            }
            int q = 0;
            int RowOrder = 0;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                RowOrder = data.Select(string.Format("id='{0}'", data.Rows[i]["id"].ToString())).ToList().Count;
                cells.Merge(4 + q, 0, RowOrder, 1);
                cells.Merge(4 + q, 1, RowOrder, 1);
                cells.Merge(4 + q, 2, RowOrder, 1);
                cells.Merge(4 + q, 3, RowOrder, 1);
                cells.Merge(4 + q, 4, RowOrder, 1);
                q += RowOrder;
            }
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            System.Threading.Thread.Sleep(400);
            wb.Save(Server.MapPath("~/Resource/Temp/" + fName));
            return Success("�����ɹ���", fName);
        }
        /// <summary>
        /// ����۲�ƻ�
        /// </summary>
        /// <returns></returns>
        [HandlerLogin(LoginMode.Ignore)]
        [HandlerAuthorize(PermissionMode.Ignore)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(5, "����۲�ƻ�")]
        public string ImportPlanDataList()
        {
            int error = 0;
            string message = "��ѡ���ʽ��ȷ���ļ��ٵ���!";

            string falseMessage = "";
            int count = HttpContext.Request.Files.Count;
            if (count > 0)
            {
                count = 0;
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
                wb.Open(Server.MapPath("~/Resource/temp/" + fileName), file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xlsx") ? Aspose.Cells.FileFormatType.Excel2007Xlsx : Aspose.Cells.FileFormatType.Excel2003);
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                //�жϱ�ͷ�Ƿ���ȷ,����ʹ�ô���ģ��
                var sheet = wb.Worksheets[0];
                if (sheet.Cells[2, 1].StringValue != "רҵ" || sheet.Cells[2, 2].StringValue != "����" || sheet.Cells[2, 3].StringValue != "��ҵ����" || sheet.Cells[2, 4].StringValue != "�ƻ����"
                    || sheet.Cells[2, 5].StringValue != "����ֽ�" || sheet.Cells[2, 6].StringValue != "���յȼ�" || sheet.Cells[2, 7].StringValue != "�۲�Ƶ��"
                    || sheet.Cells[2, 8].StringValue != "�۲���Ա" || sheet.Cells[2, 9].StringValue != "�ƻ��۲��·�" || sheet.Cells[2, 10].StringValue != "��ע")
                {
                    return message;
                }
                
               
                var ObsEhsPlan = new List<ObsplanEHSEntity>();
                var ObsPlan = new List<ObsplanEntity>();
                var ObsPlanWork = new List<ObsplanworkEntity>();

                for (int i = 3; i <= sheet.Cells.MaxDataRow; i++)
                {
                    //������ҵ����Ϊ�����ֶ�
                    if (user.RoleName.Contains("��������"))
                    {
                        var plan = new ObsplanEHSEntity();
                        plan.Create();
                        plan.PlanDept = user.DeptName;
                        plan.PlanDeptCode = user.DeptCode;
                        plan.PlanDeptId = user.DeptId;
                        plan.IsPublic = "0";
                        plan.PlanSpeciaty = sheet.Cells[i, 1].StringValue;
                        plan.WorkName = sheet.Cells[i, 3].StringValue;
                        plan.PlanArea = sheet.Cells[i, 2].StringValue;
                        plan.PlanYear = sheet.Cells[i, 4].StringValue;
                        plan.Iscommit = "0";
                        if (string.IsNullOrEmpty(sheet.Cells[i, 3].StringValue))
                        {
                            plan.ID = ObsEhsPlan[i - 3 - 1].ID;
                        }
                        ObsEhsPlan.Add(plan);
                    }
                    else {
                        var plan = new ObsplanEntity();
                        plan.Create();
                        plan.PlanDept = user.DeptName;
                        plan.PlanDeptCode = user.DeptCode;
                        plan.PlanDeptId = user.DeptId;
                        plan.IsEmsCommit = "0";
                        plan.IsPublic = "0";
                        plan.PlanSpeciaty = sheet.Cells[i, 1].StringValue;
                        plan.WorkName = sheet.Cells[i, 3].StringValue;
                        plan.PlanArea = sheet.Cells[i, 2].StringValue;
                        plan.PlanYear = sheet.Cells[i, 4].StringValue;
                        plan.Iscommit = "0";
                        if (string.IsNullOrEmpty(sheet.Cells[i, 3].StringValue))
                        {
                            plan.ID = ObsPlan[i - 3 - 1].ID;
                        }
                        ObsPlan.Add(plan);
                    }
                }
                for (int i = 3; i <= sheet.Cells.MaxDataRow; i++)
                {
                    //���յȼ����۲���Ա���ƻ��۲��·�Ϊ�����ֶ�
                    var dentity = new ObsplanworkEntity();
                    dentity.WorkName = sheet.Cells[i, 5].StringValue;
                    if (!string.IsNullOrWhiteSpace(sheet.Cells[i, 6].StringValue))
                    {
                        dentity.RiskLevel = sheet.Cells[i, 6].StringValue;
                    }
                    else
                    {
                        falseMessage += "</br>" + "��" + (i + 1) + "�з��յȼ�����Ϊ��,δ�ܵ���.";
                        error++;
                        continue;
                    }
                    dentity.ObsNumText = sheet.Cells[i, 7].StringValue.Replace('��', ',');
                    switch (sheet.Cells[i, 7].StringValue.Replace('��', ','))
                    {
                        case "ѡ���Թ۲�":
                            dentity.ObsNum = "I��";
                            break;
                        case "1��/����,�Է�����ҵΪ׼":
                            dentity.ObsNum = "II��";
                            break;
                        case "1��/����,�Է�����ҵΪ׼":
                            dentity.ObsNum = "III��";
                            break;
                        case "1��/��,�Է�����ҵΪ׼":
                            dentity.ObsNum = "IV��";
                            break;
                        case "ÿ�ι۲�,�Է�����ҵΪ׼":
                            dentity.ObsNum = "V��";
                            break;
                        default:
                            break;
                    }
                    if (!string.IsNullOrWhiteSpace(sheet.Cells[i, 8].StringValue))
                    {
                        var array = sheet.Cells[i, 8].StringValue.Split(',');
                        for (int h = 0; h < array.Length; h++)
                        {
                            var u = new UserBLL().GetList().Where(x => x.RealName == array[h] && x.OrganizeId == user.OrganizeId).FirstOrDefault();
                            if (u != null)
                            {
                                dentity.ObsPerson += u.RealName + ",";
                                dentity.ObsPersonId += u.UserId + ",";
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(dentity.ObsPerson))
                        {
                            dentity.ObsPerson = dentity.ObsPerson.Substring(0, dentity.ObsPerson.Length - 1);
                        }
                        if (!string.IsNullOrWhiteSpace(dentity.ObsPersonId))
                        {
                            dentity.ObsPersonId = dentity.ObsPersonId.Substring(0, dentity.ObsPersonId.Length - 1);
                        }
                    }
                    else
                    {
                        falseMessage += "</br>" + "��" + (i + 1) + "�й۲���Ա����Ϊ��,δ�ܵ���.";
                        error++;
                        continue;
                    }
                    if (!string.IsNullOrWhiteSpace(sheet.Cells[i, 9].StringValue))
                    {
                        var monthArr = sheet.Cells[i, 9].StringValue.Split(',');
                        //�ж��Ƿ������Ϊ����
                        for (int m = 0; m < monthArr.Length; m++)
                        {
                            int num = 0;
                            if (Int32.TryParse(monthArr[m], out num))
                            {
                                dentity.ObsMonth += num + ",";
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(dentity.ObsMonth))
                        {
                            dentity.ObsMonth = dentity.ObsMonth.Substring(0, dentity.ObsMonth.Length - 1);
                        }
                        else
                        {
                            falseMessage += "</br>" + "��" + (i + 1) + "�мƻ��۲��·������ʽ����ȷ,δ�ܵ���.";
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += "</br>" + "��" + (i + 1) + "�мƻ��۲��·ݲ���Ϊ��,δ�ܵ���.";
                        error++;
                        continue;
                    }
                    dentity.Remark = sheet.Cells[i, 10].StringValue;
                    dentity.Create();
                    if (user.RoleName.Contains("��������"))
                    {
                        dentity.PlanId = ObsEhsPlan[i - 3].ID;
                    }
                    else {
                        dentity.PlanId = ObsPlan[i - 3].ID;
                    }
                    
                    ObsPlanWork.Add(dentity);
                }
                if (user.RoleName.Contains("��������"))
                {
                    ObsEhsPlan = ObsEhsPlan.Where(x => x.WorkName != "").ToList();
                    count = ObsEhsPlan.Count;
                    int countNum = 0;
                    for (int i = 0; i < ObsEhsPlan.Count; i++)
                    {
                        //���յȼ�ΪIV����V�� ,�۲�ƻ��ļƻ��ȼ�Ϊ��˾��
                        var r = ObsPlanWork.Where(x => x.PlanId == ObsEhsPlan[i].ID).Where(x => x.RiskLevel == "IV��" || x.RiskLevel == "V��").FirstOrDefault();
                        if (r != null)
                        {
                            ObsEhsPlan[i].PlanLevel = "��˾��";
                        }
                        else
                        {
                            ObsEhsPlan[i].PlanLevel = "���ż�";
                        }

                        if (!string.IsNullOrWhiteSpace(ObsEhsPlan[i].PlanSpeciaty))
                        {
                            var s = new DataItemDetailBLL().GetDataItemListByItemCode("'SpecialtyType'").Where(x => x.ItemName == ObsEhsPlan[i].PlanSpeciaty).FirstOrDefault();
                            if (s != null)
                            {
                                ObsEhsPlan[i].PlanSpeciatyCode = s.ItemValue;
                            }
                            else
                            {
                                falseMessage += "</br>" + "��" + (countNum + 1) + "��רҵ��д����,δ�ܵ���.";
                                error++;
                                ObsEhsPlan.Remove(ObsEhsPlan[i]);
                                i--;
                                countNum++;
                                continue;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(ObsEhsPlan[i].PlanArea))
                        {
                            DistrictEntity disEntity = new DistrictBLL().GetDistrict(user.OrganizeId, ObsEhsPlan[i].PlanArea);
                            if (disEntity == null)
                            {
                                //�糧û�и������򲻸�ֵ
                                ObsEhsPlan[i].PlanArea = "";
                            }
                            else
                            {
                                ObsEhsPlan[i].PlanAreaCode = disEntity.DistrictID;
                            }
                        }
                        else
                        {
                            falseMessage += "</br>" + "��" + (countNum + 1) + "��������Ϊ��,δ�ܵ���.";
                            error++;
                            ObsEhsPlan.Remove(ObsEhsPlan[i]);
                            i--;
                            countNum++;
                            continue;
                        }
                        if (!string.IsNullOrWhiteSpace(ObsEhsPlan[i].WorkName))
                        {

                        }
                        else
                        {
                            falseMessage += "</br>" + "��" + (countNum + 1) + "����ҵ���ݲ���Ϊ��,δ�ܵ���.";
                            error++;
                            ObsEhsPlan.Remove(ObsEhsPlan[i]);
                            i--;
                            countNum++;
                            continue;
                        }
                        //����ж�-���ж��Ƿ�����,���ж��Ƿ���ת����ʱ���ʽ
                        if (!string.IsNullOrWhiteSpace(ObsEhsPlan[i].PlanYear))
                        {
                            int num = 0;
                            DateTime t = new DateTime();
                            if (Int32.TryParse(ObsEhsPlan[i].PlanYear, out num))
                            {
                                t = new DateTime(num, 1, 1);
                                DateTime x = new DateTime();
                                if (DateTime.TryParse(t.ToString(), out x))
                                {

                                }
                                else
                                {
                                    falseMessage += "</br>" + "��" + (countNum + 1) + "�мƻ������д����,δ�ܵ���.";
                                    error++;
                                    ObsEhsPlan.Remove(ObsEhsPlan[i]);
                                    i--;
                                    countNum++;
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            ObsEhsPlan[i].PlanYear = DateTime.Now.Year.ToString();
                        }
                        countNum++;
                    }
                    obsplanbll.InsertImportData(ObsEhsPlan, ObsPlanWork);
                }
                else {
                    ObsPlan = ObsPlan.Where(x => x.WorkName != "").ToList();
                    count = ObsPlan.Count;
                    int countNum = 0;
                    for (int i = 0; i < ObsPlan.Count; i++)
                    {
                        //���յȼ�ΪIV����V�� ,�۲�ƻ��ļƻ��ȼ�Ϊ��˾��
                        var r = ObsPlanWork.Where(x => x.PlanId == ObsPlan[i].ID).Where(x => x.RiskLevel == "IV��" || x.RiskLevel == "V��").FirstOrDefault();
                        if (r != null)
                        {
                            ObsPlan[i].PlanLevel = "��˾��";
                        }
                        else
                        {
                            ObsPlan[i].PlanLevel = "���ż�";
                        }

                        if (!string.IsNullOrWhiteSpace(ObsPlan[i].PlanSpeciaty))
                        {
                            var s = new DataItemDetailBLL().GetDataItemListByItemCode("'SpecialtyType'").Where(x => x.ItemName == ObsPlan[i].PlanSpeciaty).FirstOrDefault();
                            if (s != null)
                            {
                                ObsPlan[i].PlanSpeciatyCode = s.ItemValue;
                            }
                            else
                            {
                                falseMessage += "</br>" + "��" + (countNum + 1) + "��רҵ��д����,δ�ܵ���.";
                                error++;
                                ObsPlan.Remove(ObsPlan[i]);
                                i--;
                                countNum++;
                                continue;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(ObsPlan[i].PlanArea))
                        {
                            DistrictEntity disEntity = new DistrictBLL().GetDistrict(user.OrganizeId, ObsPlan[i].PlanArea);
                            if (disEntity == null)
                            {
                                //�糧û�и������򲻸�ֵ
                                ObsPlan[i].PlanArea = "";
                            }
                            else
                            {
                                ObsPlan[i].PlanAreaCode = disEntity.DistrictID;
                            }
                        }
                        else
                        {
                            falseMessage += "</br>" + "��" + (countNum + 1) + "��������Ϊ��,δ�ܵ���.";
                            error++;
                            ObsPlan.Remove(ObsPlan[i]);
                            i--;
                            countNum++;
                            continue;
                        }
                        if (!string.IsNullOrWhiteSpace(ObsPlan[i].WorkName))
                        {

                        }
                        else
                        {
                            falseMessage += "</br>" + "��" + (countNum + 1) + "����ҵ���ݲ���Ϊ��,δ�ܵ���.";
                            error++;
                            ObsPlan.Remove(ObsPlan[i]);
                            i--;
                            countNum++;
                            continue;
                        }
                        //����ж�-���ж��Ƿ�����,���ж��Ƿ���ת����ʱ���ʽ
                        if (!string.IsNullOrWhiteSpace(ObsPlan[i].PlanYear))
                        {
                            int num = 0;
                            DateTime t = new DateTime();
                            if (Int32.TryParse(ObsPlan[i].PlanYear, out num))
                            {
                                t = new DateTime(num, 1, 1);
                                DateTime x = new DateTime();
                                if (DateTime.TryParse(t.ToString(), out x))
                                {

                                }
                                else
                                {
                                    falseMessage += "</br>" + "��" + (countNum + 1) + "�мƻ������д����,δ�ܵ���.";
                                    error++;
                                    ObsPlan.Remove(ObsPlan[i]);
                                    i--;
                                    countNum++;
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            ObsPlan[i].PlanYear = DateTime.Now.Year.ToString();
                        }
                        countNum++;
                    }
                    obsplanbll.InsertImportData(ObsPlan, ObsPlanWork);
                }
                message = "����" + count + "����¼,�ɹ�����" + (count - error) + "����ʧ��" + error + "����";
                if (error > 0)
                {
                    message += "</br>" + falseMessage;
                }
            }
            return message;
        }
        [HttpPost]
        [AjaxOnly]
        public ActionResult CopyHistoryData(string oldYear, string newYear)
        {
            Operator currUser = OperatorProvider.Provider.Current();
            if (obsplanbll.CopyHistoryData(currUser, oldYear, newYear))
            {
                return Success("�����ɹ���");
            }
            else
            {
                return Error("����ʧ�ܡ�");
            }
        }
        #endregion
    }
}
