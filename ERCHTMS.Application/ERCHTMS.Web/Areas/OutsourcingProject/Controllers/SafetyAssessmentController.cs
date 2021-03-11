using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Busines.PublicInfoManage;
using System.Linq;
using System.Web;
using System.Drawing;
using BSFramework.Data;
using BSFramework.Util.Extension;
using Aspose.Words;
using Aspose.Words.Tables;
using ERCHTMS.Busines.AuthorizeManage;
using System.Text;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// �� ������ȫ��������
    /// </summary>
    public class SafetyAssessmentController : MvcControllerBase
    {
        private SafetyAssessmentBLL safetyassessmentbll = new SafetyAssessmentBLL();
        private SafetyassessmentpersonBLL safetyassessmentpersonbll = new SafetyassessmentpersonBLL();
        private AptitudeinvestigateauditBLL aptitudeinvestigateauditbll = new AptitudeinvestigateauditBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();
        private HistorysafetyassessmentBLL historysafetyassessmentbll = new HistorysafetyassessmentBLL();
        private DepartmentBLL departmentbll = new DepartmentBLL();
        private UserBLL userbll = new UserBLL();
        private SafestandardBLL safestandardbll = new SafestandardBLL();
        private SafestandarditemBLL safestandarditembll = new SafestandarditemBLL();

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
        /// ��ʷҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HistoryIndex()
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
            ViewBag.Code = safetyassessmentbll.GetMaxCode();
            return View();
        }

        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ScoreForm()
        {
            return View();
        }

        /// <summary>
        /// ����ͼ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FlowForm()
        {
            return View();
        }

        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ApplyForm()
        {
            return View();
        }

        /// <summary>
        /// ����ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExportTotalForm()
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
            var data = safetyassessmentbll.GetList(queryJson);
            return ToJsonResult(data);
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
            Operator user = OperatorProvider.Provider.Current();
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "id";
            //string strfild = "(select to_char(wm_concat(to_char(c.SCORE))) from EPG_SAFETYASSESSMENTPERSON c  where c.SAFETYASSESSMENTID=t.ID) applyscore,";
            //strfild += "(select to_char(wm_concat(to_char(c.EVALUATESCORE))) from EPG_SAFETYASSESSMENTPERSON c  where c.SAFETYASSESSMENTID=t.ID) applyevaluatescore,";
            //strfild += "(select to_char(wm_concat(to_char(c.EVALUATECONTENT))) from EPG_SAFETYASSESSMENTPERSON c  where c.SAFETYASSESSMENTID=t.ID) applycontent,";
            // isapply ����0˵����������������0˵�����Ǹ����˲�������
            pagination.p_fields =  "(select to_char(wm_concat(to_char(c.EVALUATEDEPTNAME))) from EPG_SAFETYASSESSMENTPERSON c  where c.SAFETYASSESSMENTID=t.ID) as examinetodept,  case when((flowdept = '" + user.DeptId + "') and instr('" + user.RoleName + "',FLOWROLENAME) >0) then 1 else 0 end isapply, createuserid, createuserdeptcode, createuserorgcode, createdate, createusername, modifydate, modifyuserid, modifyusername, flowid, numcode, examinecode, examinedept, examinedeptid, examineperson, examinepersonid, examinetype,  to_char(t.examinetime,'yyyy-MM-dd') as examinetime, examinereason, examinebasis, flowdeptname, flowdept, flowrolename, flowrole, flowname, issaved, isover, evaluatetype,examinetypename";
            pagination.p_tablename = " epg_safetyassessment t ";

            //pagination.conditionJson = "1=1";
            pagination.conditionJson = "((CREATEUSERID = '" + user.UserId + "') or( ISSAVED = '1'))";
            if (!user.IsSystem)
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
                            pagination.conditionJson += " and CREATEUSERDEPTCODE='" + user.DeptCode + "'";
                            break;
                        case "3":
                            var deptentity = departmentbll.GetEntity(user.DeptId);
                            while (deptentity.Nature == "����" || deptentity.Nature == "רҵ")
                            {
                                deptentity = departmentbll.GetEntity(deptentity.ParentId);
                            }
                            pagination.conditionJson += " and (CREATEUSERDEPTCODE like '" + user.DeptCode + "%' or CREATEUSERDEPTCODE in(select dept.ENCODE from base_department dept where dept.DEPARTMENTID in ( select a.outprojectid from epg_outsouringengineer a where a.engineerletdeptid in (select departmentid from base_department where encode like '" + deptentity.EnCode + "%'))) )";
                            break;
                        case "4":
                            pagination.conditionJson += " and CREATEUSERDEPTCODE like '" + user.OrganizeCode + "%'";
                            break;
                        //case "5":
                        //    pagination.conditionJson += string.Format(" and createdeptcode in(select encode from BASE_DEPARTMENT where deptcode like '{0}%')", user.NewDeptCode);
                        //    break;
                    }
                }
                else
                {
                    //pagination.conditionJson += " and createdeptcode like '" + user.OrganizeCode + "%'";
                }

            }
            // Ĭ�ϵ�ǰ�˻��߻����ύ���˿��Կ�
            //pagination.conditionJson = "( (CREATEUSERID = '" + user.UserId + "') or (CREATEUSERORGCODE = '" + user.OrganizeCode + "' and ISSAVED = '1'))";
            //pagination.conditionJson = "()";
            //pagination.sidx = "createdate";
            //pagination.sord = "desc";
            // 
            //if (!user.IsSystem)
            //{
            //    pagination.conditionJson += " and createuserorgcode='" + user.OrganizeCode + "'";
            //}
            var data = safetyassessmentbll.GetPageList(pagination, queryJson);
            data.Columns.Add("applycontent", Type.GetType("System.String"));
            foreach (DataRow dr in data.Rows)
            {
                var personeva =  safetyassessmentpersonbll.GetList(" SAFETYASSESSMENTID = '"+ dr["id"]+ "' ");
                string applycontent = "";
                int num = 0;
                foreach (SafetyassessmentpersonEntity pe in personeva)
                {
                    applycontent += pe.EVALUATEDEPTNAME +"("+ pe.SCORE + "/"+pe.EVALUATESCORE+"/"+pe.EVALUATECONTENT + ")";
                    if (personeva.Count()-1 > num)
                    {
                        applycontent += "��";
                    }
                    num++;
                }
                dr["applycontent"] = applycontent;
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
        /// ��ȡ��ʷ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetHistoryPageListJson(Pagination pagination, string queryJson)
        {
            Operator user = OperatorProvider.Provider.Current();
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "id";
            // isapply ����0˵����������������0˵�����Ǹ����˲�������
            pagination.p_fields = " (select to_char(wm_concat(to_char(c.EVALUATEDEPTNAME))) from EPG_SAFETYASSESSMENTPERSON c  where c.SAFETYASSESSMENTID=t.ID) as examinetodept,  case when((flowdept = '" + user.DeptId + "') and instr('" + user.RoleName + "',FLOWROLENAME) >0) then 1 else 0 end isapply, createuserid, createuserdeptcode, createuserorgcode, createdate, createusername, modifydate, modifyuserid, modifyusername, flowid, numcode, examinecode, examinedept, examinedeptid, examineperson, examinepersonid, examinetype, to_char(t.examinetime,'yyyy-MM-dd') as examinetime, examinereason, examinebasis, flowdeptname, flowdept, flowrolename, flowrole, flowname, issaved, isover, evaluatetype,examinetypename";
            pagination.p_tablename = " epg_historysafetyassessment t ";

            pagination.conditionJson = "1=1";
            // Ĭ�ϵ�ǰ�˺�����˿��Կ�
            //pagination.conditionJson = "()";
            //pagination.sidx = "createdate";
            //pagination.sord = "desc";
            // 
            if (!user.IsSystem)
            {
                pagination.conditionJson += " and createuserorgcode='" + user.OrganizeCode + "'";
            }
            var data = safetyassessmentbll.GetPageList(pagination, queryJson);
            data.Columns.Add("applycontent", Type.GetType("System.String"));
            foreach (DataRow dr in data.Rows)
            {
                var personeva = safetyassessmentpersonbll.GetList(" SAFETYASSESSMENTID = '" + dr["id"] + "' ");
                string applycontent = "";
                int num = 0;
                foreach (SafetyassessmentpersonEntity pe in personeva)
                {
                    applycontent += pe.EVALUATEDEPTNAME + "(" + pe.SCORE + "/" + pe.EVALUATESCORE + "/" + pe.EVALUATECONTENT + ")";
                    if (personeva.Count() - 1 > num)
                    {
                        applycontent += "��";
                    }
                    num++;
                }
                dr["applycontent"] = applycontent;
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
        /// ��ȡ���������б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetScoreListJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "t.ID";
            pagination.p_fields = @"  createuserid,scoretype, score, evaluatetype, evaluatetypename, evaluatedept, evaluatedeptname, evaluatescore, evaluatecontent, evaluateother, safetyassessmentid  ";
            pagination.p_tablename = @" epg_safetyassessmentperson t ";

            var queryParam = queryJson.ToJObject();
            if (!queryParam["ID"].IsEmpty() && !queryParam["ID"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" t.safetyassessmentid = '{0}'", queryParam["ID"].ToString());
            }

            var watch = CommonHelper.TimerStart();
            var data = safetyassessmentpersonbll.GetList(pagination, queryJson);
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
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = safetyassessmentbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ��ʷ��¼�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJsontotal(string keyValue)
        {
            int resultNum = safetyassessmentbll.GetFormJsontotal(keyValue);
            return ToJsonResult(resultNum);
        }

        /// <summary>
        /// ��ȡ��ʷʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetHistoryFormJson(string keyValue)
        {
            var data = historysafetyassessmentbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ������Ϣʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetScoreFormJson(string keyValue)
        {
            var data = safetyassessmentpersonbll.GetEntity(keyValue);
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
            safetyassessmentbll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }

        /// <summary>
        /// ɾ����������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveScoreForm(string keyValue)
        {
            safetyassessmentpersonbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SafetyAssessmentEntity entity)
        {
            try
            {
                entity.IsOver = 0;
                entity.IsSaved = 0;
                if (entity.EXAMINECODE == "" || entity.EXAMINECODE == null)
                {
                    entity.EXAMINECODE = safetyassessmentbll.GetMaxCode();
                }
                safetyassessmentbll.SaveForm(keyValue, entity);
                return Success("�����ɹ���");
            }
            catch (Exception e)
            {
                //return Error("����ʧ�ܡ�");
                return Error(e.Message);
            }
        }

        /// <summary>
        /// ���濼����Ϣ�����������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveScoreForm(string keyValue, SafetyassessmentpersonEntity entity)
        {
            try
            {
                safetyassessmentpersonbll.SaveForm(keyValue, entity);
                return Success("�����ɹ���");
            }
            catch (Exception e)
            {

                return Success(e.Message);
            }
            
            
        }

        /// <summary>
        /// ���ݰ�������IDɾ��������Ϣ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public ActionResult DelByKeyId(string keyValue)
        {
            try
            {
                safetyassessmentpersonbll.DelByKeyId(keyValue);
                return Success("�л��ɹ���");
            }
            catch (Exception e)
            {

                return Success(e.Message);
            }


        }


        #region 
        /// <summary>
        /// �ύ���ύ����һ�����̣�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, SafetyAssessmentEntity entity)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            string state = string.Empty;

            string result = "0";
            var personeva = safetyassessmentpersonbll.GetList(" SAFETYASSESSMENTID = '" + keyValue + "' ");
            if (personeva.Count() > 0)
            {
                string moduleName = "��ȫ�������";

                /// <param name="currUser">��ǰ��¼��</param>
                /// <param name="state">�Ƿ���Ȩ����� 1������� 0 ���������</param>
                /// <param name="moduleName">ģ������</param>
                /// <param name="outengineerid">����Id</param>
                ManyPowerCheckEntity mpcEntity = safetyassessmentbll.CheckAuditPower(curUser, out state, moduleName, curUser.DeptId, "0");

                string flowid = string.Empty;

                if (null != mpcEntity)
                {
                    //���氲ȫ���˼�¼
                    entity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                    entity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                    entity.FLOWROLE = mpcEntity.CHECKROLEID;
                    entity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                    entity.IsSaved = 1; //����Ѿ��ӵǼǵ���˽׶�
                    entity.IsOver = 0; //����δ��ɣ�1��ʾ���
                    entity.FLOWID = mpcEntity.ID;
                    entity.FLOWNAME = mpcEntity.FLOWNAME + "�����";
                }
                if (entity.EXAMINECODE == "" || entity.EXAMINECODE == null)
                {
                    entity.EXAMINECODE = safetyassessmentbll.GetMaxCode();
                }
                safetyassessmentbll.SaveForm(keyValue, entity);

                return Success("�����ɹ�!");
            }
            else
            {
                return Error("����д������Ϣ!");
            }
             
        }
        #endregion






        #region ��ȫ������������
        /// <summary>
        /// ��ȫ������������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <param name="aentity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult ApporveForm(string keyValue, AptitudeinvestigateauditEntity aentity)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            string state = string.Empty;

            string moduleName = "��ȫ�������";

            SafetyAssessmentEntity entity = safetyassessmentbll.GetEntity(keyValue);
            ///// <param name="currUser">��ǰ��¼��</param>
            ///// <param name="state">�Ƿ���Ȩ����� 1������� 0 ���������</param>
            ///// <param name="moduleName">ģ������</param>
            ///// <param name="createdeptid">�����˲���ID</param>
            ManyPowerCheckEntity mpcEntity = safetyassessmentbll.CheckAuditPower(curUser, out state, moduleName, entity.CreateUserDeptId,"1");


            #region //�����Ϣ��
            AptitudeinvestigateauditEntity aidEntity = new AptitudeinvestigateauditEntity();
            aidEntity.AUDITRESULT = aentity.AUDITRESULT; //ͨ��
            aidEntity.AUDITTIME = Convert.ToDateTime(aentity.AUDITTIME.Value.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss")); //���ʱ��
            aidEntity.AUDITPEOPLE = aentity.AUDITPEOPLE;  //�����Ա����
            aidEntity.AUDITPEOPLEID = aentity.AUDITPEOPLEID;//�����Աid
            aidEntity.APTITUDEID = keyValue;  //������ҵ��ID 
            aidEntity.AUDITDEPTID = aentity.AUDITDEPTID;//��˲���id
            aidEntity.AUDITDEPT = aentity.AUDITDEPT; //��˲���
            aidEntity.AUDITOPINION = aentity.AUDITOPINION; //������
            aidEntity.FlowId = entity.FLOWID;
            aidEntity.AUDITSIGNIMG = HttpUtility.UrlDecode(aidEntity.AUDITSIGNIMG);
            aidEntity.AUDITSIGNIMG = string.IsNullOrWhiteSpace(aentity.AUDITSIGNIMG) ? "" : aentity.AUDITSIGNIMG.ToString().Replace("../..", "");
            if (null != mpcEntity)
            {
                aidEntity.REMARK = (mpcEntity.AUTOID.Value - 1).ToString(); //��ע �����̵�˳���
            }
            else
            {
                aidEntity.REMARK = "1";
            }
            aptitudeinvestigateauditbll.SaveForm(aidEntity.ID, aidEntity);
            #endregion

            #region  //���氲ȫ����
            //���ͨ��
            if (aentity.AUDITRESULT == "0")
            {
                //0��ʾ����δ��ɣ�1��ʾ���̽���
                if (null != mpcEntity)
                {
                    entity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                    entity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                    entity.FLOWROLE = mpcEntity.CHECKROLEID;
                    entity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                    entity.IsSaved = 1;
                    entity.IsOver = 0;
                    entity.FLOWID = mpcEntity.ID;
                    entity.FLOWNAME = mpcEntity.CHECKDEPTNAME + "�����";
                }
                else
                {
                    entity.FLOWDEPT = "";
                    entity.FLOWDEPTNAME = "";
                    entity.FLOWROLE = "";
                    entity.FLOWROLENAME = "";
                    entity.IsSaved = 1;
                    entity.IsOver = 1;
                    entity.FLOWNAME = "";
                }
            }
            else //��˲�ͨ�� 
            {
                entity.FLOWDEPT = "";
                entity.FLOWDEPTNAME = "";
                entity.FLOWROLE = "";
                entity.FLOWROLENAME = "";
                entity.IsOver = 0; //���ڵǼǽ׶�
                entity.IsSaved = 0; //�Ƿ����״̬��ֵΪδ���
                entity.FLOWNAME = "";
                entity.FLOWID = "";

            }
            //�����ճ����˻���״̬��Ϣ
            safetyassessmentbll.SaveForm(keyValue, entity);
            #endregion

            #region    //��˲�ͨ��
            if (aentity.AUDITRESULT == "1")
            {
                //�����ʷ��¼
                ///
                HistorysafetyassessmentEntity hsentity = new HistorysafetyassessmentEntity();
                hsentity.CREATEUSERID = entity.CREATEUSERID;
                hsentity.CREATEUSERDEPTCODE = entity.CREATEUSERDEPTCODE;
                hsentity.CREATEUSERORGCODE = entity.CREATEUSERORGCODE;
                hsentity.CREATEDATE = entity.CREATEDATE;
                hsentity.CREATEUSERNAME = entity.CREATEUSERNAME;
                //hsentity.CreateUserDeptId = entity.CreateUserDeptId;
                hsentity.MODIFYDATE = entity.MODIFYDATE;
                hsentity.MODIFYUSERID = entity.MODIFYUSERID;
                hsentity.MODIFYUSERNAME = entity.MODIFYUSERNAME;
                hsentity.FLOWID = entity.FLOWID;
                hsentity.NUMCODE = entity.NUMCODE;
                hsentity.EXAMINECODE = entity.EXAMINECODE;
                hsentity.EXAMINEDEPT = entity.EXAMINEDEPT;
                hsentity.EXAMINEDEPTID = entity.EXAMINEDEPTID;
                hsentity.EXAMINEPERSON = entity.EXAMINEPERSON; //����ID
                hsentity.EXAMINEPERSONID = entity.EXAMINEPERSONID;
                hsentity.EXAMINETYPE = entity.EXAMINETYPE;
                hsentity.EXAMINETIME = entity.EXAMINETIME; //����ID
                hsentity.EXAMINEREASON = entity.EXAMINEREASON;
                hsentity.EXAMINEBASIS = entity.EXAMINEBASIS;
                hsentity.FLOWDEPTNAME = entity.FLOWDEPTNAME;
                hsentity.FLOWDEPT = entity.FLOWDEPT;
                hsentity.CONTRACTID = entity.ID;//����ID
                hsentity.FLOWROLENAME = entity.FLOWROLENAME;
                hsentity.FLOWROLE = entity.FLOWROLE;
                hsentity.FLOWNAME = entity.FLOWNAME;
                hsentity.ISSAVED = 1;
                hsentity.ISOVER = entity.IsOver;
                hsentity.EVALUATETYPE = entity.EvaluateType;
                hsentity.EXAMINETYPENAME = entity.EXAMINETYPENAME;
                hsentity.ID = "";

                historysafetyassessmentbll.SaveForm(hsentity.ID, hsentity);

                //��ȡ��ǰҵ������������˼�¼
                var shlist = aptitudeinvestigateauditbll.GetAuditList(keyValue);
                //����������˼�¼����ID
                foreach (AptitudeinvestigateauditEntity mode in shlist)
                {
                    mode.APTITUDEID = hsentity.ID; //��Ӧ�µ�ID
                    aptitudeinvestigateauditbll.SaveForm(mode.ID, mode);
                }
                //�������¸�����¼����ID
                var flist = fileinfobll.GetImageListByObject(keyValue);
                foreach (FileInfoEntity fmode in flist)
                {
                    fmode.RecId = hsentity.ID; //��Ӧ�µ�ID
                    fileinfobll.SaveForm("", fmode);
                }

                //�������¿�����Ϣ

                var personeva = safetyassessmentpersonbll.GetList(" SAFETYASSESSMENTID = '" + keyValue + "' ");

                foreach (SafetyassessmentpersonEntity samode in personeva)
                {
                    samode.SAFETYASSESSMENTID = hsentity.ID; //��Ӧ�µ�ID
                    safetyassessmentpersonbll.SaveForm("", samode);
                }
            }
            #endregion

            return Success("�����ɹ�!");
        }
        #endregion


        /// <summary>
        /// ��ȡ�����Ϣ
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetApplyListJson(Pagination pagination, string queryJson)
        {
           

            var watch = CommonHelper.TimerStart();
            var queryParam = queryJson.ToJObject();
            var data = aptitudeinvestigateauditbll.GetAuditList(queryParam["ID"].ToString());
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
        /// ��ѯ�������ͼ
        /// </summary>
        /// <param name="keyValue">����</param>
        /// <param name="urltype">��ѯ���ͣ�0����ȫ����</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAuditFlowData(string keyValue, string urltype)
        {
            try
            {
                var data = safetyassessmentbll.GetAuditFlowData(keyValue, urltype);
                return ToJsonResult(data);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }

        /// <summary>
        /// ��ȫ���˱��ĵ���
        /// </summary>
        /// <param name="keyValue"></param>
        public void ExportDataSafeAssment(string keyValue)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            //�������
            string fileName = Server.MapPath("~/Resource/ExcelTemplate/��ȫ������.doc");
            
            DataTable dt = new DataTable();
            dt.Columns.Add("numcode"); //���
            dt.Columns.Add("deptsorpersons"); //������λ
            dt.Columns.Add("createtime");
            dt.Columns.Add("resaon"); 
            dt.Columns.Add("basis");
            dt.Columns.Add("money");
            dt.Columns.Add("auditreason");
            dt.Columns.Add("auditdept");
            dt.Columns.Add("audittime");
            dt.Columns.Add("deptname");
            dt.Columns.Add("gzpic");

            dt.Columns.Add("assmentimage");

            DataRow row = dt.NewRow();

            SafetyAssessmentEntity entity = safetyassessmentbll.GetEntity(keyValue); // ������Ϣ
            if (entity.EvaluateType == "0")
            {
                fileName = Server.MapPath("~/Resource/ExcelTemplate/��ȫ������.doc");
            }
            else
            {
                fileName = Server.MapPath("~/Resource/ExcelTemplate/��ȫ������.doc");
            }
            Aspose.Words.Document doc = new Aspose.Words.Document(fileName);
            DocumentBuilder builder = new DocumentBuilder(doc);
            var personeva = safetyassessmentpersonbll.GetList(" SAFETYASSESSMENTID = '" + entity.ID + "' "); // ������Ϣ
            var auditlist = aptitudeinvestigateauditbll.GetAuditList(entity.ID); // ������Ϣ
            string deptsorpersons = "";
            int numfor = 1;
            int isAJB = 0;
            decimal money = 0;
            foreach (SafetyassessmentpersonEntity samode in personeva)
            {
                if (numfor >= personeva.Count())
                {
                    deptsorpersons += samode.EVALUATEDEPTNAME ;
                }
                else
                {
                    deptsorpersons += samode.EVALUATEDEPTNAME + "��";
                }
                if (samode.SCORE != "" && samode.SCORE != null)
                {
                    money += Convert.ToDecimal(samode.SCORE);
                }

                numfor++;
            }
            row["numcode"] = entity.EXAMINECODE;
            row["deptsorpersons"] = deptsorpersons;
            row["createtime"] = entity.EXAMINETIME.Value.ToString("yyyy��MM��dd��");
            row["resaon"] = entity.EXAMINEREASON+","+entity.EXAMINEBASIS;
            row["basis"] = entity.EXAMINEBASIS;;
            row["money"] = CmycurD(money);
            foreach (var audit in auditlist)
            {
                string audittype = audit.AUDITRESULT == "0" ? "ͨ��;" : "��ͨ��;";
                row["auditreason"] = audittype +  audit.AUDITOPINION;
                row["auditdept"] = audit.AUDITDEPT;
                row["audittime"] = audit.AUDITTIME.Value.ToString("yyyy��MM��dd��");
                row["deptname"] = audit.AUDITDEPT;
                string userid = audit.AUDITPEOPLEID;
                UserEntity useren =  userbll.GetEntity(audit.AUDITPEOPLEID);
                // ����ǰ��ಿ�����ˣ�����ʾͼ��
                if (audit.AUDITDEPT == "���ಿ" && user.RoleName.Contains("������"))
                {
                    isAJB = 1;
                }
            }
            Aspose.Words.DocumentBuilder db = new Aspose.Words.DocumentBuilder(doc);
            db.MoveToMergeField("assmentimage");

            var data = fileinfobll.GetFiles(entity.ID);

            if (data != null)
            {
                foreach (DataRow item in data.Rows)
                {
                    var path = item.Field<string>("FilePath");
                    db.InsertImage(Server.MapPath(path), 190, 140);
                }

            }
            else
            {
                row["assmentimage"] = "";
            }

            
            db.MoveToMergeField("gzpic");
            if (isAJB == 1)
            {
                Aspose.Words.Drawing.Shape shape = db.InsertImage(Server.MapPath("~/content/Images/Page0001.png"), 120, 112);
            }
            else
            {
                row["gzpic"] = "";
            }
            
           
            dt.Rows.Add(row);
            doc.MailMerge.Execute(dt);

            if (entity.EvaluateType == "0")
            {
                doc.Save(resp, Server.UrlEncode("��ȫ������_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".doc"), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
            }
            else
            {
                doc.Save(resp, Server.UrlEncode("��ȫ������_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".doc"), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
            }
            
        }

        /// <summary>
        /// ����ת��д
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public string CmycurD(decimal num)
        {
            string str1 = "��Ҽ��������½��ƾ�";            //0-9����Ӧ�ĺ��� 
            string str2 = "��Ǫ��ʰ��Ǫ��ʰ��Ǫ��ʰԪ�Ƿ�"; //����λ����Ӧ�ĺ��� 
            string str3 = "";    //��ԭnumֵ��ȡ����ֵ 
            string str4 = "";    //���ֵ��ַ�����ʽ 
            string str5 = "";  //����Ҵ�д�����ʽ 
            int i;    //ѭ������ 
            int j;    //num��ֵ����100���ַ������� 
            string ch1 = "";    //���ֵĺ������ 
            string ch2 = "";    //����λ�ĺ��ֶ��� 
            int nzero = 0;  //����������������ֵ�Ǽ��� 
            int temp;            //��ԭnumֵ��ȡ����ֵ 

            num = Math.Round(Math.Abs(num), 2);    //��numȡ����ֵ����������ȡ2λС�� 
            str4 = ((long)(num * 100)).ToString();        //��num��100��ת�����ַ�����ʽ 
            j = str4.Length;      //�ҳ����λ 
            if (j > 15) { return "���"; }
            str2 = str2.Substring(15 - j);   //ȡ����Ӧλ����str2��ֵ���磺200.55,jΪ5����str2=��ʰԪ�Ƿ� 

            //ѭ��ȡ��ÿһλ��Ҫת����ֵ 
            for (i = 0; i < j; i++)
            {
                str3 = str4.Substring(i, 1);          //ȡ����ת����ĳһλ��ֵ 
                temp = Convert.ToInt32(str3);      //ת��Ϊ���� 
                if (i != (j - 3) && i != (j - 7) && i != (j - 11) && i != (j - 15))
                {
                    //����ȡλ����ΪԪ�����ڡ������ϵ�����ʱ 
                    if (str3 == "0")
                    {
                        ch1 = "";
                        ch2 = "";
                        nzero = nzero + 1;
                    }
                    else
                    {
                        if (str3 != "0" && nzero != 0)
                        {
                            ch1 = "��" + str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                    }
                }
                else
                {
                    //��λ�����ڣ��ڣ���Ԫλ�ȹؼ�λ 
                    if (str3 != "0" && nzero != 0)
                    {
                        ch1 = "��" + str1.Substring(temp * 1, 1);
                        ch2 = str2.Substring(i, 1);
                        nzero = 0;
                    }
                    else
                    {
                        if (str3 != "0" && nzero == 0)
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            if (str3 == "0" && nzero >= 3)
                            {
                                ch1 = "";
                                ch2 = "";
                                nzero = nzero + 1;
                            }
                            else
                            {
                                if (j >= 11)
                                {
                                    ch1 = "";
                                    nzero = nzero + 1;
                                }
                                else
                                {
                                    ch1 = "";
                                    ch2 = str2.Substring(i, 1);
                                    nzero = nzero + 1;
                                }
                            }
                        }
                    }
                }
                if (i == (j - 11) || i == (j - 3))
                {
                    //�����λ����λ��Ԫλ�������д�� 
                    ch2 = str2.Substring(i, 1);
                }
                str5 = str5 + ch1 + ch2;

                if (i == j - 1 && str3 == "0")
                {
                    //���һλ���֣�Ϊ0ʱ�����ϡ����� 
                    str5 = str5 + '��';
                }
            }
            if (num == 0)
            {
                str5 = "��Ԫ��";
            }
            return str5;
        }



        /// <summary>
        /// ��ȫ���˻��ܵ���
        /// </summary>
        /// <param name="time"></param>
        /// <param name="deptid"></param>
        public void ExportDataTotal(string time,string deptid)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            //�������
            string fileName = Server.MapPath("~/Resource/ExcelTemplate/X���X�·ݰ�ȫ�����������������������ͳ��.doc");
            Aspose.Words.Document doc = new Aspose.Words.Document(fileName);
            DocumentBuilder builder = new DocumentBuilder(doc);
            DataTable dt = new DataTable();
            dt.Columns.Add("year"); 
            dt.Columns.Add("month"); 
            DataRow row = dt.NewRow();

            row["year"] = Convert.ToDateTime(time).ToString("yyyy");
            row["month"] = Convert.ToDateTime(time).ToString("MM");


            dt.Rows.Add(row);
            doc.MailMerge.Execute(dt);

            DataTable dt1 = new DataTable("A");
            dt1.Columns.Add("sortnum");
            dt1.Columns.Add("time");
            dt1.Columns.Add("deptname");
            dt1.Columns.Add("reasonbasis");
            dt1.Columns.Add("money");
            dt1.Columns.Add("score");
            dt1.Columns.Add("remark");
            string deptcode = departmentbll.GetEntity(deptid).EnCode;
            DataTable assmentData = safetyassessmentbll.ExportDataTotal(time, deptcode);
            if (assmentData.Rows.Count > 0)
            {
                for (int i = 0; i < assmentData.Rows.Count; i++)
                {
                    DataRow row1 = dt1.NewRow();
                    row1["sortnum"] = i + 1;
                    row1["time"] = assmentData.Rows[i]["CREATEDATE"];

                    row1["deptname"] = assmentData.Rows[i]["DEPTRESULTNAME"];
                    string con = "";
                    if (assmentData.Rows[i]["SCORETYPE"].ToString() == "0")
                    {
                        con += "����" + assmentData.Rows[i]["EVALUATEDEPTNAME"].ToString();
                    }
                    else
                    {
                        con += "����" + assmentData.Rows[i]["EVALUATEDEPTNAME"].ToString();
                    }
                    if (assmentData.Rows[i]["SCORE"].ToString() != "")
                    {
                        con += assmentData.Rows[i]["SCORE"].ToString()+"Ԫ;";
                    }
                    if (assmentData.Rows[i]["EVALUATESCORE"].ToString() != "")
                    {
                        con += assmentData.Rows[i]["EVALUATESCORE"].ToString() + "��;";
                    }
                    if (assmentData.Rows[i]["EVALUATECONTENT"].ToString() != "")
                    {
                        con += "��Ч"+assmentData.Rows[i]["EVALUATECONTENT"].ToString() + ";";
                    }
                    row1["reasonbasis"] = assmentData.Rows[i]["EXAMINEREASON"] + "  " + assmentData.Rows[i]["EXAMINEBASIS"]+";"+ con;
                    row1["money"] = ((assmentData.Rows[i]["SCORETYPE"].ToString()=="0" && assmentData.Rows[i]["SCORE"].ToString() != "") ?"-":"") + assmentData.Rows[i]["SCORE"];
                    row1["score"] = assmentData.Rows[i]["EVALUATESCORE"];
                    row1["remark"] = assmentData.Rows[i]["EXAMINETYPENAME"];
                    dt1.Rows.Add(row1);
                }
            }
            //builder.StartTable
            doc.MailMerge.ExecuteWithRegions(dt1);
            doc.MailMerge.DeleteFields();
            doc.Save(resp, Server.UrlEncode("��ȫ���˻��ܱ�_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".doc"), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
        }

        /// <summary>
        /// �����ⲿ���ſ���
        /// </summary>
        /// <param name="time"></param>
        /// <param name="deptid"></param>
        public void ExportDataOutDept(string time, string deptid)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            //�������
            string fileName = Server.MapPath("~/Resource/ExcelTemplate/X��X����ίά����λ���������ϸ���ܱ�.doc");
            Aspose.Words.Document doc = new Aspose.Words.Document(fileName);
            DocumentBuilder builder = new DocumentBuilder(doc);

            //  ��ȡ�ⲿ���ſ�����Ϣ
            string deptcode = departmentbll.GetEntity(deptid).EnCode;
            DataTable assmentData = safetyassessmentbll.ExportDataOutDept(time, deptcode);

            DataTable dtyear = new DataTable();
            dtyear.Columns.Add("year");
            DataRow rowyear = dtyear.NewRow();
            //rowyear["year"] = DateTime.Now.ToString("yyyy��MM��");
            rowyear["year"] = Convert.ToDateTime(time).ToString("yyyy��MM��");


            dtyear.Rows.Add(rowyear);
            doc.MailMerge.Execute(dtyear);

            builder.MoveToBookmark("table");
            StringBuilder strexcel = new StringBuilder();
            strexcel.Append(@"<table   border=0 cellspacing=0 cellpadding=0 style='border-collapse:collapse;TABLE-LAYOUT:fixed;word-break: break-all; width:100%'>");


            strexcel.Append(@"  <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes;height:18.25pt'>
                                    <td width=691 colspan=8 style='width:518.3pt;border:none;border-bottom:solid windowtext 1.0pt;
                                    mso-border-bottom-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                    height:18.25pt'>
                                    <p class=MsoNormal style='mso-pagination:widow-orphan;layout-grid-mode:char;
                                    vertical-align:middle'><b><span lang=EN-US style='font-size:10.5pt;
                                    font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
                                    AR'><o:p>&nbsp;</o:p></span></b></p>
                                    </td>
                                    <td width=236 colspan=4 style='width:177.0pt;border:none;border-bottom:solid windowtext 1.0pt;
                                    mso-border-bottom-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                    height:18.25pt'>
                                    <p class=MsoNormal align=left style='text-align:left;text-indent:10.5pt;
                                    mso-char-indent-count:1.0;mso-pagination:widow-orphan;layout-grid-mode:char;
                                    vertical-align:middle'><span style='font-size:10.5pt;font-family:����;
                                    mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'>�������ڣ�");
            strexcel.Append(DateTime.Now.ToString("yyyy��MM��dd��"));
            strexcel.Append(@" <span lang=EN-US><o:p></o:p></span></b></span></p>
                                    </td>
                                    </tr>");
            #region ��ͷ
            strexcel.Append(@"  <tr style='mso-yfti-irow:1;height:27.5pt'>
                                      <td width=74 style='width:55.55pt;border:solid windowtext 1.0pt;border-top:
                                      none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
                                      padding:.75pt .75pt .75pt .75pt;height:27.5pt'>
                                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
                                      font-family:����;mso-bidi-font-family:����'>���ε�λ<span lang=EN-US><o:p></o:p></span></span></b></p>
                                      </td>
                                      <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
                                      solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
                                      solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
                                      solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:27.5pt'>
                                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
                                      font-family:����;mso-bidi-font-family:����'>���<span lang=EN-US><o:p></o:p></span></span></b></p>
                                      </td>
                                      <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
                                      border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
                                      mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
                                      mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                      height:27.5pt'>
                                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
                                      font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
                                      AR'>ʱ��<span lang=EN-US><o:p></o:p></span></span></b></p>
                                      </td>
                                      <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
                                      none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
                                      mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
                                      mso-border-top-alt:windowtext;mso-border-left-alt:windowtext;mso-border-bottom-alt:
                                      black;mso-border-right-alt:black;mso-border-style-alt:solid;mso-border-width-alt:
                                      .5pt;padding:.75pt .75pt .75pt .75pt;height:27.5pt'>
                                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
                                      font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
                                      AR'>��������<span lang=EN-US><o:p></o:p></span></span></b></p>
                                      </td>
                                      <td width=124 style='width:93.0pt;border-top:none;border-left:none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;mso-border-top-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:27.5pt'>
                                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'>��������<span lang=EN-US><o:p></o:p></span></span></b></p>
                                      </td>
                                      <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
                                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                                      solid windowtext .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:
                                      solid black .5pt;mso-border-top-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                      height:27.5pt'>
                                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
                                      font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
                                      AR'>���˽��<span lang=EN-US><o:p></o:p></span></span></b></p>
                                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
                                      font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
                                      AR'>��Ԫ��<span lang=EN-US><o:p></o:p></span></span></b></p>
                                      </td>
                                      <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
                                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid windowtext .5pt;
                                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                      mso-border-top-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                      height:27.5pt'>
                                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
                                      font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
                                      AR'>������˲��Ż�רҵ<span lang=EN-US><o:p></o:p></span></span></b></p>
                                      </td>
                                      <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
                                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid windowtext .5pt;
                                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                      mso-border-top-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                      height:27.5pt'>
                                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
                                      font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
                                      AR'>��ע<span lang=EN-US><o:p></o:p></span></span></b></p>
                                      </td>
                                     </tr>");
            #endregion

            #region �м�����
            decimal litterNum = 0; //С��
            decimal insertNum = 0; //���Ӻϼ�
            decimal lessNum = 0; //��Ǯ�ϼ�
            if (assmentData.Rows.Count > 0)
            {
                int rowstar = 0; //�ڵ㿪ʼ
                string deotcode = string.Empty; //����code
                int num = 0; // ��ʾ��һ����ͬ��̧ͷ������
                int xNum = 0; //���
                
                foreach (DataRow dr in assmentData.Rows)
                {
                    if (deotcode != null && deotcode != dr["BMNAMECODE"].ToString() && deotcode != "")
                    {
                        rowstar = 0;
                        strexcel.Append(@"<tr style='mso-yfti-irow:8;height:23.05pt'>
                                              <td width=74 style='width:55.55pt;border:solid windowtext 1.0pt;border-top:
                                              none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
                                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                              layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
                                              style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;color:black'><o:p>&nbsp;</o:p></span></p>
                                              </td>
                                              <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
                                              solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
                                              solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
                                              solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                              vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                              ����;mso-bidi-font-family:����;color:black'><o:p>&nbsp;</o:p></span></p>
                                              </td>
                                              <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
                                              border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
                                              mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
                                              mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                              height:23.05pt'>
                                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                              layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:
                                              normal'><span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:
                                              ����;color:blue;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
                                              </td>
                                              <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
                                              none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
                                              mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
                                              mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
                                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                              vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                                              style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:
                                              0pt;mso-bidi-language:AR'>�¶ȹ���Ч�����ۿ�����<span lang=EN-US><o:p></o:p></span></span></b></p>
                                              </td>
                                              <td width=124 style='width:93.0pt;border-top:none;border-left:none;
                                              border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                                              solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                              vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                              ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR;
                                              mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
                                              </td>
                                              <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
                                              border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                                              solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                              vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                              ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR;
                                              mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
                                              </td>
                                              <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
                                              solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                              mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                              vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                              ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR;
                                              mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
                                              </td>
                                              <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
                                              solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                              mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                              <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
                                              lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
                                              mso-font-kerning:0pt;mso-bidi-language:AR;mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
                                              </td>
                                             </tr>");
                        // ���С��
                        strexcel.Append(@"<tr style='mso-yfti-irow:9;height:23.05pt'>
                              <td width=74 style='width:55.55pt;border:solid windowtext 1.0pt;border-top:
                              none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                              layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
                              font-family:����;mso-bidi-font-family:����;color:black'>С�ƣ�</span></b><span
                              lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
                              color:black'><o:p></o:p></span></p>
                              </td>
                              <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
                              solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
                              solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
                              solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                              vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                              ����;mso-bidi-font-family:����;color:black'><o:p>&nbsp;</o:p></span></p>
                              </td>
                              <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
                              border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
                              mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
                              mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                              height:23.05pt'>
                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                              layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:
                              normal'><span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:
                              ����;color:blue;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
                              </td>
                              <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
                              none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
                              mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
                              mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                              <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
                              vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                              lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
                              mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
                              </td>
                              <td width=124 style='width:93.0pt;border-top:none;border-left:none;
                              border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                              solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                              vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                              ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR;
                              mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
                              </td>
                              <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
                              border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                              solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                              vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                              ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR;
                              mso-bidi-font-weight:bold'>");
                        strexcel.Append(litterNum);
                        strexcel.Append(@"<o:p>&nbsp;</o:p></span></p>
                              </td>
                              <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
                              solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                              mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                              vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                              ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR;
                              mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
                              </td>
                              <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
                              solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                              mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                              <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
                              lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
                              mso-font-kerning:0pt;mso-bidi-language:AR;mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
                              </td>  </tr>");
                        litterNum = 0;
                    }
                    strexcel.Append("<tr style='mso-yfti-irow:2;height:23.05pt'>");
                    
                    if (rowstar == 0)
                    {
                        rowstar = 1;
                        xNum = 1; // �µĲ�����ʾ 1
                        num = 0;
                        foreach (DataRow drnum in assmentData.Rows)
                        {
                            if (dr["BMNAMECODE"].ToString() == drnum["BMNAMECODE"].ToString())
                            {
                                num++;
                                if (drnum["SCORE"] != null && drnum["SCORE"].ToString() != "")
                                {
                                    if (drnum["SCORETYPE"].ToString() == "0")
                                    {
                                        litterNum = litterNum - Convert.ToDecimal(drnum["SCORE"].ToString());
                                        lessNum = lessNum - Convert.ToDecimal(drnum["SCORE"].ToString());
                                    }
                                    else
                                    {
                                        litterNum = litterNum + Convert.ToDecimal(drnum["SCORE"].ToString());
                                        insertNum = insertNum + Convert.ToDecimal(drnum["SCORE"].ToString());
                                    }
                                }

                            }

                        }

                        //��һ��
                        strexcel.Append("<td width=74 rowspan=" + num + " style='width:55.55pt;border:solid windowtext 1.0pt;border-top:none; mso - border - top - alt:solid windowtext .5pt; mso - border - alt:solid windowtext .5pt;padding: .75pt .75pt .75pt .75pt; height: 23.05pt'>" +
                                           "<p class=MsoNormal align = center style='text-align:center;mso-pagination:widow-orphan;layout -grid-mode:char;vertical-align:middle'>" +
                                           "<span style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;'>" + dr["BMNAME"].ToString() +
                                            "<span lang = EN - US ></span></span></p>" +
                                          "</td>");

                    }

                    #region �м�ͨ�����ݼ���
                    // ������
                    strexcel.Append("<td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'><p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;color:black'>" + xNum + "</span></p></td>");
                    //���ʱ��
                    strexcel.Append("<td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none; border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt; mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'><p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;layout-grid-mode:char;vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����; mso-font-kerning:0pt;mso-bidi-language:AR'>" + Convert.ToDateTime(dr["CREATEDATE"].ToString()).ToString("MM��dd��") + "</span><span lang=EN-US></span></span></p></td>");
                    //��������
                    //strexcel.Append("<td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt; mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt; word-break: break-all;  word-wrap: break-word; '><p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'>" + dr["EXAMINEREASON"].ToString()  + "<o:p>&nbsp;</o:p></span></p></td>");
                    string khnrhtml = "";
                    for (int i = 0; i < dr["EXAMINEREASON"].ToString().Length; i = i + 26)
                    {
                        if (dr["EXAMINEREASON"].ToString().Length > i + 26)
                        {
                            khnrhtml += "<span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'>" + dr["EXAMINEREASON"].ToString().Substring(i, 26) + "</span>";
                        }
                        else
                        {
                            khnrhtml += "<span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'>" + dr["EXAMINEREASON"].ToString().Substring(i, dr["EXAMINEREASON"].ToString().Length - i) + "</span>";
                        }


                    }
                    strexcel.Append("<td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt; mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt; word-break: break-all;  word-wrap: break-word; '><p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;vertical-align:middle'>" +
                       khnrhtml +
                        "</p></td>");

                    //��������
                    //strexcel.Append("<td width=124 style='width:93.0pt;border-top:none;border-left:none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt: solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'><p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'>" + dr["EXAMINEBASIS"].ToString() + "<o:p>&nbsp;</o:p></span></p></td>");
                    string htmlsqpn = "";
                    for (int i = 0; i < dr["EXAMINEBASIS"].ToString().Length; i = i + 8)
                    {
                        if (dr["EXAMINEBASIS"].ToString().Length > i + 8)
                        {
                            htmlsqpn += "<span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'>" + dr["EXAMINEBASIS"].ToString().Substring(i, 8) + "</span>";
                        }
                        else
                        {
                            htmlsqpn += "<span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'>" + dr["EXAMINEBASIS"].ToString().Substring(i, dr["EXAMINEBASIS"].ToString().Length - i) + "</span>";
                        }


                    }
                    strexcel.Append("<td width=124 style='width:93.0pt;border-top:none;border-left:none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt: solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt;word-break: break-all;word-wrap: break-word;'>" +
                        "  <p>" + htmlsqpn + "</p>" +
                        "</td>");
                    // ���
                    strexcel.Append(" <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none; border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'><p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan; vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'>" + ((dr["SCORETYPE"].ToString() == "0" && dr["SCORE"].ToString() !="") ? "-" : "") + dr["SCORE"].ToString() + "<o:p>&nbsp;</o:p></span></p>" +
                                      "</td>");
                    // ������˲���
                    strexcel.Append(" <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom: solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt; mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>" +
                                          "<p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'>" + dr["EXAMINEDEPT"].ToString() + "<o:p>&nbsp;</o:p></span></p>" +
                                          "</td>");
                    // ��ע
                    //strexcel.Append(@"<td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>" +
                    //                  "<p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'>" + dr["EVALUATEOTHER"].ToString()  + "</span></p>" +
                    //                  "</td>");
                    string bzhtml = "";
                    for (int i = 0; i < dr["EVALUATEOTHER"].ToString().Length; i = i + 6)
                    {
                        if (dr["EVALUATEOTHER"].ToString().Length > i + 6)
                        {
                            bzhtml += "<span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'>" + dr["EVALUATEOTHER"].ToString().Substring(i, 6) + "</span>";
                        }
                        else
                        {
                            bzhtml += "<span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'>" + dr["EVALUATEOTHER"].ToString().Substring(i, dr["EVALUATEOTHER"].ToString().Length - i) + "</span>";
                        }


                    }
                    strexcel.Append(@"<td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>" +
                                      "<p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'>" + bzhtml+
                                      "</p>" +
                                      "</td>");
                    #endregion
                    strexcel.Append("</tr>");
                    //if (deotcode != null && deotcode != dr["BMNAMECODE"].ToString() && deotcode != "")
                    //{
                    //    rowstar = 0;
                        
                       
                        

                    //}
                    xNum++; 
                    deotcode = dr["BMNAMECODE"].ToString();

                }
                strexcel.Append(@"<tr style='mso-yfti-irow:8;height:23.05pt'>
                                              <td width=74 style='width:55.55pt;border:solid windowtext 1.0pt;border-top:
                                              none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
                                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                              layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
                                              style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;color:black'><o:p>&nbsp;</o:p></span></p>
                                              </td>
                                              <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
                                              solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
                                              solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
                                              solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                              vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                              ����;mso-bidi-font-family:����;color:black'><o:p>&nbsp;</o:p></span></p>
                                              </td>
                                              <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
                                              border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
                                              mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
                                              mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                              height:23.05pt'>
                                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                              layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:
                                              normal'><span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:
                                              ����;color:blue;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
                                              </td>
                                              <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
                                              none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
                                              mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
                                              mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
                                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                              vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                                              style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:
                                              0pt;mso-bidi-language:AR'>�¶ȹ���Ч�����ۿ�����<span lang=EN-US><o:p></o:p></span></span></b></p>
                                              </td>
                                              <td width=124 style='width:93.0pt;border-top:none;border-left:none;
                                              border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                                              solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                              vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                              ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR;
                                              mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
                                              </td>
                                              <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
                                              border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                                              solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                              vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                              ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR;
                                              mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
                                              </td>
                                              <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
                                              solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                              mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                              vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                              ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR;
                                              mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
                                              </td>
                                              <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
                                              solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                              mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                              <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
                                              lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
                                              mso-font-kerning:0pt;mso-bidi-language:AR;mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
                                              </td>
                                             </tr>");
                strexcel.Append(@"<tr style='mso-yfti-irow:9;height:23.05pt'>
                              <td width=74 style='width:55.55pt;border:solid windowtext 1.0pt;border-top:
                              none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                              layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
                              font-family:����;mso-bidi-font-family:����;color:black'>С�ƣ�</span></b><span
                              lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
                              color:black'><o:p></o:p></span></p>
                              </td>
                              <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
                              solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
                              solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
                              solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                              vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                              ����;mso-bidi-font-family:����;color:black'><o:p>&nbsp;</o:p></span></p>
                              </td>
                              <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
                              border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
                              mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
                              mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                              height:23.05pt'>
                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                              layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:
                              normal'><span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:
                              ����;color:blue;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
                              </td>
                              <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
                              none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
                              mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
                              mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                              <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
                              vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                              lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
                              mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
                              </td>
                              <td width=124 style='width:93.0pt;border-top:none;border-left:none;
                              border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                              solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                              vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                              ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR;
                              mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
                              </td>
                              <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
                              border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                              solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                              vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                              ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR;
                              mso-bidi-font-weight:bold'>");
                strexcel.Append(litterNum);
                strexcel.Append(@"<o:p>&nbsp;</o:p></span></p>
                              </td>
                              <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
                              solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                              mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                              vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                              ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR;
                              mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
                              </td>
                              <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
                              solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                              mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                              <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
                              lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
                              mso-font-kerning:0pt;mso-bidi-language:AR;mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
                              </td>  </tr>");
                litterNum = 0;

            }
            #region  
            //strexcel.Append(@"<tr style='mso-yfti-irow:2;height:23.05pt'>
            //      <td width=74 rowspan=6 style='width:55.55pt;border:solid windowtext 1.0pt;
            //      border-top:none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><span style='font-size:10.5pt;
            //      font-family:����;mso-bidi-font-family:����;color:black'>��</span><span
            //      style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;color:blue'>������<span
            //      lang=EN-US><o:p></o:p></span></span></p>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><span style='font-size:10.5pt;
            //      font-family:����;mso-bidi-font-family:����;color:blue'>��λ</span><span
            //      style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;color:black'>��<span
            //      lang=EN-US><o:p></o:p></span></span></p>
            //      </td>
            //      <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
            //      solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //      solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //      solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;color:black'>1<o:p></o:p></span></p>
            //      </td>
            //      <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
            //      border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
            //      mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //      height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
            //      style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;color:blue;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'>X</span><span style='font-size:
            //      10.5pt;font-family:����;mso-bidi-font-family:����;color:blue;mso-font-kerning:
            //      0pt;mso-bidi-language:AR'>��<span lang=EN-US>X</span>��<span lang=EN-US><o:p></o:p></span></span></p>
            //      </td>
            //      <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
            //      none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
            //      mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=124 style='width:93.0pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //     </tr>
            //     <tr style='mso-yfti-irow:3;height:23.05pt'>
            //      <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
            //      solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //      solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //      solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;color:black'>2<o:p></o:p></span></p>
            //      </td>
            //      <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
            //      border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
            //      mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //      height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
            //      style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;color:blue;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
            //      none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
            //      mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=124 style='width:93.0pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //     </tr>
            //     <tr style='mso-yfti-irow:4;height:23.05pt'>
            //      <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
            //      solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //      solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //      solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;color:black'>3<o:p></o:p></span></p>
            //      </td>
            //      <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
            //      border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
            //      mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //      height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
            //      style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;color:blue;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
            //      none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
            //      mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=124 style='width:93.0pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //     </tr>
            //     <tr style='mso-yfti-irow:5;height:23.05pt'>
            //      <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
            //      solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //      solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //      solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;color:black'>4<o:p></o:p></span></p>
            //      </td>
            //      <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
            //      border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
            //      mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //      height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
            //      style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;color:blue;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
            //      none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
            //      mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=124 style='width:93.0pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //     </tr>
            //     <tr style='mso-yfti-irow:6;height:23.05pt'>
            //      <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
            //      solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //      solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //      solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;color:black'>5<o:p></o:p></span></p>
            //      </td>
            //      <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
            //      border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
            //      mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //      height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
            //      style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;color:blue;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
            //      none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
            //      mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=124 style='width:93.0pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //     </tr>
            //     <tr style='mso-yfti-irow:7;height:23.05pt'>
            //      <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
            //      solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //      solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //      solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;color:black'>6<o:p></o:p></span></p>
            //      </td>
            //      <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
            //      border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
            //      mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //      height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
            //      style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;color:blue;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
            //      none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
            //      mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=124 style='width:93.0pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //     </tr>
            //     <tr style='mso-yfti-irow:8;height:23.05pt'>
            //      <td width=74 style='width:55.55pt;border:solid windowtext 1.0pt;border-top:
            //      none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
            //      style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;color:black'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
            //      solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //      solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //      solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;color:black'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
            //      border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
            //      mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //      height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:
            //      normal'><span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:
            //      ����;color:blue;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
            //      none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
            //      mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:
            //      0pt;mso-bidi-language:AR'>�¶ȹ���Ч�����ۿ�����<span lang=EN-US><o:p></o:p></span></span></b></p>
            //      </td>
            //      <td width=124 style='width:93.0pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR;
            //      mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR;
            //      mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR;
            //      mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //      mso-font-kerning:0pt;mso-bidi-language:AR;mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //     </tr>
            //     <tr style='mso-yfti-irow:9;height:23.05pt'>
            //      <td width=74 style='width:55.55pt;border:solid windowtext 1.0pt;border-top:
            //      none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //      font-family:����;mso-bidi-font-family:����;color:black'>С�ƣ�</span></b><span
            //      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //      color:black'><o:p></o:p></span></p>
            //      </td>
            //      <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
            //      solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //      solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //      solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;color:black'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
            //      border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
            //      mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //      height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:
            //      normal'><span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:
            //      ����;color:blue;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
            //      none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
            //      mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=124 style='width:93.0pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR;
            //      mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR;
            //      mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR;
            //      mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //      mso-font-kerning:0pt;mso-bidi-language:AR;mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //     </tr>
            //     <tr style='mso-yfti-irow:10;height:23.05pt'>
            //      <td width=74 rowspan=3 style='width:55.55pt;border:solid windowtext 1.0pt;
            //      border-top:none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><span style='font-size:10.5pt;
            //      font-family:����;mso-bidi-font-family:����;color:black'>��</span><span
            //      style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;color:blue'>������<span
            //      lang=EN-US><o:p></o:p></span></span></p>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><span style='font-size:10.5pt;
            //      font-family:����;mso-bidi-font-family:����;color:blue'>��λ</span><span
            //      style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;color:black'>��<b><span
            //      lang=EN-US><o:p></o:p></span></b></span></p>
            //      </td>
            //      <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
            //      solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //      solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //      solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;color:black'>1<o:p></o:p></span></p>
            //      </td>
            //      <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
            //      border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
            //      mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //      height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:
            //      normal'><span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:
            //      ����;color:blue;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
            //      none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
            //      mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=124 style='width:93.0pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //     </tr>
            //     <tr style='mso-yfti-irow:11;height:23.05pt'>
            //      <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
            //      solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //      solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //      solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;color:black'>2<o:p></o:p></span></p>
            //      </td>
            //      <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
            //      border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
            //      mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //      height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:
            //      normal'><span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:
            //      ����;color:blue;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
            //      none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
            //      mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=124 style='width:93.0pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //     </tr>
            //     <tr style='mso-yfti-irow:12;height:23.05pt'>
            //      <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
            //      solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //      solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //      solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;color:black'>3<o:p></o:p></span></p>
            //      </td>
            //      <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
            //      border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
            //      mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //      height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:
            //      normal'><span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:
            //      ����;color:blue;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
            //      none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
            //      mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=124 style='width:93.0pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //     </tr>
            //     <tr style='mso-yfti-irow:13;height:23.05pt'>
            //      <td width=74 style='width:55.55pt;border:solid windowtext 1.0pt;border-top:
            //      none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><b><span lang=EN-US
            //      style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;color:black'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
            //      solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //      solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //      solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      ����;mso-bidi-font-family:����;color:black'>4<o:p></o:p></span></p>
            //      </td>
            //      <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
            //      border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
            //      mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //      height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:
            //      normal'><span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:
            //      ����;color:blue;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
            //      none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
            //      mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:
            //      0pt;mso-bidi-language:AR'>�¶ȹ���Ч�����ۿ�����<span lang=EN-US><o:p></o:p></span></span></b></p>
            //      </td>
            //      <td width=124 style='width:93.0pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //     </tr>");
            #endregion

            #endregion


            #region �ϼ�
            // �����ϼ�
            strexcel.Append(@"<tr style='mso-yfti-irow:15;height:23.05pt'>
                                  <td width=74 style='width:55.55pt;border:solid windowtext 1.0pt;border-top:none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt; padding:.75pt .75pt .75pt .75pt;height:23.05pt'><p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;color:black'>�����ϼƣ�<span lang=EN-US><o:p></o:p></span></span></b></p></td>
                                  <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'><p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����'><o:p>&nbsp;</o:p></span></p></td>
                                  <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt; height:23.05pt'><p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p></td>
                                  <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
                                  none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
                                  mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
                                  mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                  <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                                  lang=EN-US style='font-size:10.5pt;font-f
amily:����;mso-bidi-font-family:����;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
                                  </td>
                                  <td width=124 style='width:93.0pt;border-top:none;border-left:none;
                                  border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                                  solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
                                  </td>
                                  <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
                                  border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                                  solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'>");
            strexcel.Append(insertNum);
            strexcel.Append(@"<o:p>&nbsp;</o:p></span></b></p>
                                  </td>
                                  <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
                                  solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                  mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
                                  </td>
                                  <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
                                  solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                  mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
                                  </td>
                                 </tr>");

            // ��Ǯ�ϼ�
            strexcel.Append(@"<tr style='mso-yfti-irow:16;height:25.3pt'>
                                  <td width=74 style='width:55.55pt;border:solid windowtext 1.0pt;border-top:
                                  none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
                                  font-family:����;mso-bidi-font-family:����;color:black'>���˺ϼƣ�<span lang=EN-US><o:p></o:p></span></span></b></p>
                                  </td>
                                  <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
                                  solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
                                  solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
                                  solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  ����;mso-bidi-font-family:����'><o:p>&nbsp;</o:p></span></p>
                                  </td>
                                  <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
                                  border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
                                  mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
                                  mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                  height:25.3pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:
                                  normal'><span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:
                                  ����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
                                  </td>
                                  <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
                                  none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
                                  mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
                                  mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
                                  </td>
                                  <td width=124 style='width:93.0pt;border-top:none;border-left:none;
                                  border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                                  solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
                                  </td>
                                  <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
                                  border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                                  solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'>");
            strexcel.Append(lessNum);
            strexcel.Append(@"<o:p>&nbsp;</o:p></span></b></p>
                                  </td>
                                  <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
                                  solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                  mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
                                  </td>
                                  <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
                                  solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                  mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
                                  </td>
                                 </tr>");

            // �ϼ�
            strexcel.Append(@"<tr style='mso-yfti-irow:17;height:25.3pt'>
                                  <td width=74 style='width:55.55pt;border:solid windowtext 1.0pt;border-top:
                                  none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
                                  font-family:����;mso-bidi-font-family:����;color:black'>�ϼƣ�<span lang=EN-US><o:p></o:p></span></span></b></p>
                                  </td>
                                  <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
                                  solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
                                  solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
                                  solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  ����;mso-bidi-font-family:����'><o:p>&nbsp;</o:p></span></p>
                                  </td>
                                  <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
                                  border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
                                  mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
                                  mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                  height:25.3pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:
                                  normal'><span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:
                                  ����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
                                  </td>
                                  <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
                                  none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
                                  mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
                                  mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
                                  </td>
                                  <td width=124 style='width:93.0pt;border-top:none;border-left:none;
                                  border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                                  solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
                                  </td>
                                  <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
                                  border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                                  solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'>");
            strexcel.Append(insertNum + lessNum);
            strexcel.Append(@"<o:p>&nbsp;</o:p></span></b></p>
                                  </td>
                                  <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
                                  solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                  mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
                                  </td>
                                  <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
                                  solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                  mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
                                  </td>
                                 </tr>");

            #region html �ϼ�
            //            strexcel.Append(@" <tr style='mso-yfti-irow:15;height:23.05pt'>
            //                                  <td width=74 style='width:55.55pt;border:solid windowtext 1.0pt;border-top:none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt; padding:.75pt .75pt .75pt .75pt;height:23.05pt'><p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;color:black'>�����ϼƣ�<span lang=EN-US><o:p></o:p></span></span></b></p></td>
            //                                  <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'><p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����'><o:p>&nbsp;</o:p></span></p></td>
            //                                  <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt; height:23.05pt'><p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p></td>
            //                                  <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
            //                                  none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
            //                                  mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //                                  mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                                  <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
            //                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                                  lang=EN-US style='font-size:10.5pt;font-f
            //amily:����;mso-bidi-font-family:����;
            //                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                                  </td>
            //                                  <td width=124 style='width:93.0pt;border-top:none;border-left:none;
            //                                  border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                                  solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                                  </td>
            //                                  <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
            //                                  border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                                  solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                                  </td>
            //                                  <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
            //                                  solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                                  mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                                  </td>
            //                                  <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
            //                                  solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                                  mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                                  <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                                  </td>
            //                                 </tr>
            //                                 <tr style='mso-yfti-irow:16;height:25.3pt'>
            //                                  <td width=74 style='width:55.55pt;border:solid windowtext 1.0pt;border-top:
            //                                  none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
            //                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
            //                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                                  layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                                  font-family:����;mso-bidi-font-family:����;color:black'>���˺ϼƣ�<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                                  </td>
            //                                  <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
            //                                  solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //                                  solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //                                  solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
            //                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                                  ����;mso-bidi-font-family:����'><o:p>&nbsp;</o:p></span></p>
            //                                  </td>
            //                                  <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
            //                                  border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
            //                                  mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
            //                                  mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //                                  height:25.3pt'>
            //                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                                  layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:
            //                                  normal'><span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:
            //                                  ����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                                  </td>
            //                                  <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
            //                                  none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
            //                                  mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //                                  mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
            //                                  <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
            //                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                                  </td>
            //                                  <td width=124 style='width:93.0pt;border-top:none;border-left:none;
            //                                  border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                                  solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
            //                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                                  </td>
            //                                  <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
            //                                  border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                                  solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
            //                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                                  </td>
            //                                  <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
            //                                  solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                                  mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
            //                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                                  </td>
            //                                  <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
            //                                  solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                                  mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
            //                                  <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                                  </td>
            //                                 </tr>
            //                                 <tr style='mso-yfti-irow:17;height:25.3pt'>
            //                                  <td width=74 style='width:55.55pt;border:solid windowtext 1.0pt;border-top:
            //                                  none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
            //                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
            //                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                                  layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                                  font-family:����;mso-bidi-font-family:����;color:black'>�ϼƣ�<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                                  </td>
            //                                  <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
            //                                  solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //                                  solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //                                  solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
            //                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                                  ����;mso-bidi-font-family:����'><o:p>&nbsp;</o:p></span></p>
            //                                  </td>
            //                                  <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
            //                                  border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
            //                                  mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
            //                                  mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //                                  height:25.3pt'>
            //                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                                  layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:
            //                                  normal'><span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:
            //                                  ����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                                  </td>
            //                                  <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
            //                                  none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
            //                                  mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //                                  mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
            //                                  <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
            //                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                                  </td>
            //                                  <td width=124 style='width:93.0pt;border-top:none;border-left:none;
            //                                  border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                                  solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
            //                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                                  </td>
            //                                  <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
            //                                  border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                                  solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
            //                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                                  </td>
            //                                  <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
            //                                  solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                                  mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
            //                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                                  </td>
            //                                  <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
            //                                  solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                                  mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
            //                                  <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                                  </td>
            //                                 </tr>");
            #endregion

            #endregion

            #region  �����ڶ������
            strexcel.Append(@"<tr style='mso-yfti-irow:18;height:80.2pt'>
                                  <td width=171 colspan=3 valign=top style='width:128.3pt;border-top:none;
                                  border-left:solid windowtext 1.0pt;border-bottom:solid windowtext 1.0pt;
                                  border-right:solid black 1.0pt;mso-border-top-alt:solid windowtext .5pt;
                                  mso-border-alt:solid windowtext .5pt;mso-border-right-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:80.2pt'>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;layout-grid-mode:char;
                                  vertical-align:middle'><span style='font-size:10.5pt;font-family:����;
                                  mso-bidi-font-family:����'>���粿�����<span lang=EN-US><o:p></o:p></span></span></p>
                                  <p class=MsoNormal style='text-indent:21.0pt;mso-char-indent-count:2.0;
                                  mso-pagination:widow-orphan;layout-grid-mode:char;vertical-align:middle'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����'><o:p>&nbsp;</o:p></span></p>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;layout-grid-mode:char;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  ����;mso-bidi-font-family:����'><o:p>&nbsp;</o:p></span></p>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;layout-grid-mode:char;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  ����;mso-bidi-font-family:����'><o:p>&nbsp;</o:p></span></p>
                                  <p class=MsoNormal align=right style='text-align:right;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;mso-layout-grid-align:none;vertical-align:middle'><span
                                  style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'>���粿����ǩ�£�<span lang=EN-US><o:p></o:p></span></span></p>
                                  <p class=MsoNormal align=right style='text-align:right;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;vertical-align:middle'><span style='font-size:10.5pt;
                                  font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
                                  AR'>��<span lang=EN-US><span style='mso-spacerun:yes'>&nbsp;&nbsp; </span></span>��<span
                                  lang=EN-US><span style='mso-spacerun:yes'>&nbsp;&nbsp; </span></span>��</span><span
                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����'><o:p></o:p></span></p>
                                  </td>
                                  <td width=198 colspan=2 valign=top style='width:148.5pt;border-top:none;
                                  border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;
                                  mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
                                  mso-border-alt:solid windowtext .5pt;mso-border-right-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:80.2pt'>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;layout-grid-mode:char;
                                  vertical-align:middle'><span style='font-size:10.5pt;font-family:����;
                                  mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'>ȼ�ϴ��˲������<span
                                  lang=EN-US><o:p></o:p></span></span></p>
                                  <p class=MsoNormal style='text-indent:21.0pt;mso-char-indent-count:2.0;
                                  mso-pagination:widow-orphan;layout-grid-mode:char;vertical-align:middle'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;layout-grid-mode:char;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;layout-grid-mode:char;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
                                  <p class=MsoNormal align=right style='text-align:right;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;mso-layout-grid-align:none;vertical-align:middle'><span
                                  style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'>ȼ�ϴ��˲���ǩ�£�<span lang=EN-US><o:p></o:p></span></span></p>
                                  <p class=MsoNormal align=right style='text-align:right;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
                                  style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'><span style='mso-spacerun:yes'>&nbsp; </span></span><span
                                  style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'>��<span lang=EN-US><span
                                  style='mso-spacerun:yes'>&nbsp;&nbsp; </span></span>��<span lang=EN-US><span
                                  style='mso-spacerun:yes'>&nbsp;&nbsp; </span></span>��<span lang=EN-US><o:p></o:p></span></span></p>
                                  </td>
                                  <td width=181 valign=top style='width:135.75pt;border-top:none;border-left:
                                  none;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;
                                  mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
                                  mso-border-alt:solid windowtext .5pt;mso-border-right-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:80.2pt'>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;layout-grid-mode:char;
                                  vertical-align:middle'><span style='font-size:10.5pt;font-family:����;
                                  mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'>�豸���������<span
                                  lang=EN-US><o:p></o:p></span></span></p>
                                  <p class=MsoNormal style='text-indent:21.0pt;mso-char-indent-count:2.0;
                                  mso-pagination:widow-orphan;layout-grid-mode:char;vertical-align:middle'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;layout-grid-mode:char;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;layout-grid-mode:char;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
                                  <p class=MsoNormal align=right style='text-align:right;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;mso-layout-grid-align:none;vertical-align:middle'><span
                                  style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'>�豸������ǩ�£�<span lang=EN-US><o:p></o:p></span></span></p>
                                  <p class=MsoNormal align=right style='text-align:right;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
                                  style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'><span style='mso-spacerun:yes'>&nbsp;</span></span><span
                                  style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'>��<span lang=EN-US><span
                                  style='mso-spacerun:yes'>&nbsp;&nbsp; </span></span>��<span lang=EN-US><span
                                  style='mso-spacerun:yes'>&nbsp;&nbsp; </span></span>��<span lang=EN-US><o:p></o:p></span></span></p>
                                  </td>
                                  <td width=171 colspan=3 valign=top style='width:128.25pt;border-top:none;
                                  border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;
                                  mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
                                  mso-border-alt:solid windowtext .5pt;mso-border-right-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:80.2pt'>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;layout-grid-mode:char;
                                  vertical-align:middle'><span style='font-size:10.5pt;font-family:����;
                                  mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR;mso-bidi-font-weight:
                                  bold'>��ȫ�����������<span lang=EN-US><o:p></o:p></span></span></p>
                                  <p class=MsoNormal style='text-indent:21.0pt;mso-char-indent-count:2.0;
                                  mso-pagination:widow-orphan;layout-grid-mode:char;vertical-align:middle'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
                                  mso-font-kerning:0pt;mso-bidi-language:AR;mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;layout-grid-mode:char;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR;
                                  mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;layout-grid-mode:char;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR;
                                  mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
                                  <p class=MsoNormal align=right style='text-align:right;text-indent:31.5pt;
                                  mso-char-indent-count:3.0;mso-pagination:widow-orphan;layout-grid-mode:char;
                                  mso-layout-grid-align:none;vertical-align:middle'><span lang=EN-US
                                  style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'><span style='mso-spacerun:yes'>&nbsp;</span></span><span
                                  style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'>��ȫ��������ǩ�£�<span lang=EN-US><o:p></o:p></span></span></p>
                                  <p class=MsoNormal align=right style='text-align:right;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
                                  style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'><span style='mso-spacerun:yes'>&nbsp;</span></span><span
                                  style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'>��<span lang=EN-US><span
                                  style='mso-spacerun:yes'>&nbsp; </span></span>��<span lang=EN-US><span
                                  style='mso-spacerun:yes'>&nbsp;&nbsp; </span></span>��<span lang=EN-US
                                  style='mso-bidi-font-weight:bold'><o:p></o:p></span></span></p>
                                  </td>
                                  <td width=206 colspan=3 valign=top style='width:154.5pt;border-top:none;
                                  border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;
                                  mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
                                  mso-border-alt:solid windowtext .5pt;mso-border-right-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:80.2pt'>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;layout-grid-mode:char;
                                  vertical-align:middle'><span style='font-size:10.5pt;font-family:����;
                                  mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'>�ۺϱ��ϲ������<span
                                  lang=EN-US><o:p></o:p></span></span></p>
                                  <p class=MsoNormal style='text-indent:21.0pt;mso-char-indent-count:2.0;
                                  mso-pagination:widow-orphan;layout-grid-mode:char;vertical-align:middle'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;layout-grid-mode:char;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;layout-grid-mode:char;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
                                  <p class=MsoNormal align=right style='text-align:right;text-indent:31.5pt;
                                  mso-char-indent-count:3.0;mso-pagination:widow-orphan;layout-grid-mode:char;
                                  mso-layout-grid-align:none;vertical-align:middle'><span style='font-size:
                                  10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;
                                  mso-bidi-language:AR'>�ۺϱ��ϲ���ǩ�£�<span lang=EN-US><o:p></o:p></span></span></p>
                                  <p class=MsoNormal align=right style='text-align:right;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
                                  style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'><span style='mso-spacerun:yes'>&nbsp; </span></span><span
                                  style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'>��<span lang=EN-US><span
                                  style='mso-spacerun:yes'>&nbsp;&nbsp; </span></span>��<span lang=EN-US><span
                                  style='mso-spacerun:yes'>&nbsp;&nbsp; </span></span>��<span lang=EN-US><o:p></o:p></span></span></p>
                                  </td>
                                 </tr>");
            #endregion

            // ���һ�����
            strexcel.Append(@" <tr style='mso-yfti-irow:19;mso-yfti-lastrow:yes;height:25.3pt'>
                                  <td width=927 colspan=12 valign=top style='width:695.3pt;border-top:none;
                                  border-left:solid windowtext 1.0pt;border-bottom:solid windowtext 1.0pt;
                                  border-right:solid black 1.0pt;mso-border-top-alt:solid windowtext .5pt;
                                  mso-border-alt:solid windowtext .5pt;mso-border-right-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
                                  style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'>�������ܾ����ܹ���ʦ����ʾ��<span lang=EN-US><o:p></o:p></span></span></p>
                                  <p class=MsoNormal style='text-indent:21.0pt;mso-char-indent-count:2.0;
                                  mso-pagination:widow-orphan;layout-grid-mode:char;vertical-align:middle'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;layout-grid-mode:char;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
                                  <p class=MsoNormal align=right style='text-align:right;mso-pagination:widow-orphan;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><span
                                  style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                  </span><span
                                  style='mso-spacerun:yes'></span></span><span
                                  style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'>��<span lang=EN-US><span
                                  style='mso-spacerun:yes'>&nbsp;&nbsp; </span></span>��<span lang=EN-US><span
                                  style='mso-spacerun:yes'>&nbsp;&nbsp; </span></span>��<span lang=EN-US><o:p></o:p></span></span></p>
                                  </td>
                                 </tr>");


            strexcel.Append(@"</table>");
            builder.InsertHtml(strexcel.ToString());

            doc.Save(resp, Server.UrlEncode("��ί��λ���˱�_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".doc"), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
        }

        /// <summary>
        /// �����ڲ����ſ���
        /// </summary>
        /// <param name="time"></param>
        /// <param name="deptid"></param>
        public void ExportDataInDept(string time,string deptid)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            //�������
            string fileName = Server.MapPath("~/Resource/ExcelTemplate/X��X�°�ȫ����������������������ܱ���.doc");
            Aspose.Words.Document doc = new Aspose.Words.Document(fileName);
            DocumentBuilder builder = new DocumentBuilder(doc);


            #region  ������޸�
            DataTable dtyear = new DataTable();
            dtyear.Columns.Add("ogname");
            dtyear.Columns.Add("year");
            DataRow rowyear = dtyear.NewRow();
            rowyear["year"] = Convert.ToDateTime(time).ToString("yyyy��MM��");
            rowyear["ogname"] = user.OrganizeName;
            dtyear.Rows.Add(rowyear);
            doc.MailMerge.Execute(dtyear);
            #endregion

            builder.MoveToBookmark("table");
            StringBuilder strexcel = new StringBuilder();
            strexcel.Append(@"<table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 style='border-collapse:collapse'>");



            //  ��ȡ�ڲ����ſ�����Ϣ
            string deptcode = departmentbll.GetEntity(deptid).EnCode;
            DataTable assmentData = safetyassessmentbll.ExportDataInDept(time, deptcode);
            // ��ȡ�ڲ����ϼ����� = 
            DataTable InDeptData = safetyassessmentbll.GetInDeptData();

            int deptnum = 0; // ��������

            deptnum = InDeptData.Rows.Count;


            #region ���Ϣ
            strexcel.Append(@" <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes;height:23.05pt'>
                                  <td width=471 colspan=");
            strexcel.Append((deptnum+4)/2);
            strexcel.Append(@"  style='width:353.3pt;border:none;border-bottom:solid windowtext 1.0pt;
                                  mso-border-bottom-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                  height:23.05pt'>
                                  <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
                                  vertical-align:middle'><span style='font-size:10.5pt;font-family:����;
                                  mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'>����ţ����£���");
            strexcel.Append(departmentbll.GetEntity(deptid).FullName);
            strexcel.Append(@" <span lang=EN-US><o:p></o:p></span></span></p>
                                  </td>
                                  <td width=476 colspan=");
            strexcel.Append((deptnum + 4)-((deptnum + 4) / 2));
            strexcel.Append(@"  style='width:356.9pt;border:none;border-bottom:solid windowtext 1.0pt;
                                  mso-border-bottom-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                  height:23.05pt'>
                                  <p class=MsoNormal style='text-indent:136.5pt;mso-char-indent-count:13.0;
                                  mso-pagination:widow-orphan;layout-grid-mode:char;vertical-align:middle'><span
                                  style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'>������ˣ����ڣ�");
            strexcel.Append(DateTime.Now.ToString("yyyy��MM��dd��"));
            strexcel.Append(@" <span
                                  lang=EN-US><o:p></o:p></span></span></p>
                                  </td>
                                 </tr>");
            #endregion


            #region ��һ�й̶�
            strexcel.Append(@"<tr style='mso-yfti-irow:1;height:25.3pt'>
                                  <td width=34 rowspan=2 style='width:25.55pt;border:solid windowtext 1.0pt;
                                  border-top:none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><span style='font-size:10.5pt;font-family:����;
                                  mso-bidi-font-family:����;color:black'>���<span lang=EN-US><o:p></o:p></span></span></p>
                                  </td>
                                  <td width=98 rowspan=2 style='width:73.5pt;border-top:none;border-left:none;
                                  border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
                                  mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
                                  mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                  height:25.3pt'>
                                  <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
                                  font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
                                  AR'>��������������</span></b><span style='font-size:9.0pt;font-family:����;mso-bidi-font-family:
                                  ����;mso-font-kerning:0pt;mso-bidi-language:AR'>��ע�����ݡ����Ź���������ʵʩϸ���еڼ����������˱�׼��</span><span
                                  lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p></o:p></span></p>
                                  </td>
                                  <td width=536 colspan=");
            strexcel.Append(deptnum);
            strexcel.Append(@" style='width:402.0pt;border-top:none;border-left:
                                  none;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;
                                  mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
                                  mso-border-alt:solid windowtext .5pt;mso-border-right-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><b><span style='font-size:10.5pt;font-family:����;
                                  mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'>�¶ȿ���������ۼ�����ֵǰ�ꡰ<span
                                  lang=EN-US>-</span>��������ֱ������ֵ��<span lang=EN-US><o:p></o:p></span></span></b></p>
                                  </td>
                                  <td width=64 rowspan=2 style='width:48.0pt;border-top:none;border-left:none;
                                  border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                                  solid windowtext .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:
                                  solid black .5pt;mso-border-top-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                  height:25.3pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><b><span style='font-size:10.5pt;font-family:����;
                                  mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'>���˺ϼ�<span
                                  lang=EN-US><o:p></o:p></span></span></b></p>
                                  </td>
                                  <td width=215 rowspan=2 style='width:161.15pt;border-top:none;border-left:
                                  none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
                                  mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid black .5pt;
                                  mso-border-alt:solid black .5pt;mso-border-top-alt:solid windowtext .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
                                  font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
                                  AR'>�������˵��<span lang=EN-US><o:p></o:p></span></span></b></p>
                                  <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;vertical-align:middle'><span style='font-size:9.0pt;
                                  font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
                                  AR'>��Ҫ��һ������һ˵�������г���������Ա�������������˽�</span><span lang=EN-US style='font-size:
                                  10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;
                                  mso-bidi-language:AR'><o:p></o:p></span></p>
                                  </td>
                                 </tr>");
            #endregion

            #region �ڶ��в��Ŷ�̬
            strexcel.Append(@"<tr style='mso-yfti-irow:2;height:27.5pt'>");
            foreach (DataRow dr in InDeptData.Rows)
            {
                strexcel.Append("<td width=48 style='width:36.0pt;border-top:none;border-left:none;border-bottom: "+
                                 " solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid windowtext .5pt; "+
                                 " mso-border-left-alt:solid windowtext .5pt;mso-border-top-alt:windowtext; "+
                                 " mso-border-left-alt:windowtext;mso-border-bottom-alt:black;mso-border-right-alt: "+
                                 " black;mso-border-style-alt:solid;mso-border-width-alt:.5pt;padding:.75pt .75pt .75pt .75pt; "+
                                 " height:27.5pt'> "+
                                 " <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan; "+
                                 " layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt; "+
                                 " font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language: "+
                                 " AR'>"+ dr["FULLNAME"] + "<span lang=EN-US><o:p></o:p></span></span></b></p> "+
                                 " </td>");
            }
            strexcel.Append(@"</tr>");
            //strexcel.Append(@" <tr style='mso-yfti-irow:2;height:27.5pt'>
            //                      <td width=48 style='width:36.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid windowtext .5pt;
            //                      mso-border-left-alt:solid windowtext .5pt;mso-border-top-alt:windowtext;
            //                      mso-border-left-alt:windowtext;mso-border-bottom-alt:black;mso-border-right-alt:
            //                      black;mso-border-style-alt:solid;mso-border-width-alt:.5pt;padding:.75pt .75pt .75pt .75pt;
            //                      height:27.5pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>�ۺ�<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>����<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      </td>
            //                      <td width=55 style='width:41.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid windowtext .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:
            //                      solid black .5pt;mso-border-top-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //                      height:27.5pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>�ƻ�<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>��Ӫ��<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      </td>
            //                      <td width=60 style='width:45.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid windowtext .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      mso-border-top-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //                      height:27.5pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>����<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>�ʲ���<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      </td>
            //                      <td width=62 style='width:46.5pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid windowtext .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      mso-border-top-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //                      height:27.5pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>ȼ��<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>���˲�<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      </td>
            //                      <td width=55 style='width:41.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid windowtext .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:
            //                      solid black .5pt;mso-border-top-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //                      height:27.5pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>��ȫ<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>������<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      </td>
            //                      <td width=59 style='width:44.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid windowtext .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:
            //                      solid black .5pt;mso-border-top-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //                      height:27.5pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>�豸<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>����<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      </td>
            //                      <td width=48 style='width:36.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid windowtext .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      mso-border-top-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //                      height:27.5pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>���粿<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      </td>
            //                      <td width=47 style='width:35.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid windowtext .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:
            //                      solid black .5pt;mso-border-top-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //                      height:27.5pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>����<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>������<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      </td>
            //                      <td width=49 style='width:36.75pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid windowtext .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:
            //                      solid black .5pt;mso-border-top-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //                      height:27.5pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>��ί<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>�칫��<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      </td>
            //                      <td width=53 style='width:39.75pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid windowtext .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:
            //                      solid black .5pt;mso-border-top-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //                      height:27.5pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>�ۺ�<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>���ϲ�<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      </td>
            //                     </tr>");
            #endregion

            #region ��̬������
            int coneNum = 1;
            List<string> list = new List<string>();
            Dictionary<string, decimal> jl = new Dictionary<string, decimal>();  // ��������ͳ�Ƽ���
            Dictionary<string, decimal> cf = new Dictionary<string, decimal>();  // ��������ͳ�Ƽ���
            // ��IDȥ�ز���list����
            foreach (DataRow dr in assmentData.Rows)
            {
                if (!list.Contains(dr["ID"].ToString()))
                {
                    list.Add(dr["ID"].ToString());
                }
              
            }
            // ��������
            foreach (string strlist in list)
            {
                strexcel.Append("<tr style='mso-yfti-irow:3;height:23.05pt'>");
                // ���
                strexcel.Append(@" <td width=34 style='width:25.55pt;border:solid windowtext 1.0pt;border-top:
                                  none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  ����;mso-bidi-font-family:����;color:black'>");
                strexcel.Append(coneNum);
                strexcel.Append(@" <o:p></o:p></span></p></td>");

                //������������
                                     
                strexcel.Append(@"<td width=98 style='width:73.5pt;border-top:none;border-left:none;border-bottom:
                                  solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
                                  solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
                                  solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                   <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                      layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:
                                      normal'><span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:
                                      ����;mso-font-kerning:0pt;mso-bidi-language:AR'>");
                foreach(DataRow drkh in assmentData.Rows)
                {
                    if (strlist == drkh["ID"].ToString())
                    {
                        if (drkh["EXAMINEBASISID"].ToString() == "")
                        {
                            break;
                        }
                        string[] standearr = drkh["EXAMINEBASISID"].ToString().Split(',');
                        foreach (string stan in standearr)
                        {
                            SafestandarditemEntity ite =  safestandarditembll.GetEntity(stan);
                            SafestandardEntity staninfo = safestandardbll.GetEntity(ite.STID);
                            strexcel.Append(staninfo.NAME+" ");
                        }

                        
                        //strexcel.Append(drkh["EXAMINEBASISID"].ToString());
                        break;
                    }
                }
                strexcel.Append(@"<o:p>&nbsp;</o:p></span></b></p></td>");


                decimal testTotal = 0; // ÿ�п��˺ϼ�
                string khContent = string.Empty;
                string dxContent = string.Empty;

                #region �������ݱ���
                // �����м������
                foreach (DataRow drdept in InDeptData.Rows)
                {
                    //khContent = string.Empty;
                    //dxContent = string.Empty;
               
                    decimal deptscore = 0;
                    string inDeptid = drdept["ENCODE"].ToString();
                    
                    strexcel.Append(@" <td width=48 style='width:36.0pt;border-top:none;border-left:none;border-bottom:
                                                solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                                mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid black .5pt;
                                                mso-border-left-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                                height:23.05pt'>
                                               <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                           layout-grid-mode:char;vertical-align:middle'><span
                                          lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
                                          mso-font-kerning:0pt;mso-bidi-language:AR'>");
                    foreach (DataRow datadr in assmentData.Rows)
                    {
                        
                        if (inDeptid == datadr["BMNAMECODE"].ToString() && datadr["ID"].ToString() == strlist)
                        {
                            if (datadr["SCORE"].ToString() != "")
                            {
                                if (datadr["SCORETYPE"].ToString() == "0")
                                {

                                    testTotal = testTotal - Convert.ToDecimal(datadr["SCORE"].ToString());
                                    deptscore = deptscore - Convert.ToDecimal(datadr["SCORE"].ToString());

                                    if (cf.ContainsKey(inDeptid))
                                    {
                                        cf[inDeptid] = cf[inDeptid] - Convert.ToDecimal(datadr["SCORE"].ToString());
                                    }
                                    else
                                    {
                                        cf[inDeptid] =  - Convert.ToDecimal(datadr["SCORE"].ToString());
                                    }

                                }
                                else
                                {
                                    testTotal = testTotal + Convert.ToDecimal(datadr["SCORE"].ToString());
                                    deptscore = deptscore + Convert.ToDecimal(datadr["SCORE"].ToString());

                                    if (jl.ContainsKey(inDeptid))
                                    {
                                        jl[inDeptid] = jl[inDeptid] + Convert.ToDecimal(datadr["SCORE"].ToString());
                                    }
                                    else
                                    {
                                        jl[inDeptid] =  Convert.ToDecimal(datadr["SCORE"].ToString());
                                    }
                                }
                            }

                            if (khContent == null || khContent == "")
                            {
                                khContent = datadr["EXAMINEREASON"].ToString() + " " + datadr["EXAMINEBASIS"].ToString();
                            }
                            dxContent = dxContent + (datadr["SCORETYPE"].ToString() == "0" ? "����" : "����") + datadr["EVALUATEDEPTNAME"].ToString();
                            if (datadr["SCORE"].ToString() != "")
                            {
                                dxContent = dxContent + "���" + datadr["SCORE"].ToString()+"Ԫ;";
                            }
                            if (datadr["EVALUATESCORE"].ToString() != "")
                            {
                                dxContent = dxContent + "����" + datadr["EVALUATESCORE"].ToString() + "��;";

                            }
                            if (datadr["EVALUATECONTENT"].ToString() != "")
                            {
                                dxContent = dxContent + "��Ч" + datadr["EVALUATECONTENT"].ToString() + ";";

                            }


                        }
                       
                    }
                    strexcel.Append(deptscore);
                    strexcel.Append(@" <o:p>&nbsp;</o:p></span></p></td>");
                }
                #endregion

                // ���˺ϼ�
                strexcel.Append(@" <td width=64 style='width:48.0pt;border-top:none;border-left:none;border-bottom:
                                  solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                  mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
                                      AR'>");
                strexcel.Append(testTotal + " <o:p>&nbsp;</o:p></span></p></td>");

                // �������
                                      
                strexcel.Append(@"<td width=215 style='width:161.15pt;border-top:none;border-left:none;
                                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                      <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
                                      layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
                                      style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
                                      mso-font-kerning:0pt;mso-bidi-language:AR'>");
                //strexcel.Append(khContent + dxContent + "<o:p>&nbsp;</o:p></span></p></td>");
                
                string khnrhtml = "";
                string resulkhqk = khContent + dxContent;
                for (int i = 0; i < resulkhqk.Length; i = i + 11)
                {
                    if (resulkhqk.Length > i + 11)
                    {
                        khnrhtml += "<span lang=EN-US  style = 'font-size:10.5pt;font-family:����;mso-bidi-font-family:����; mso - font - kerning:0pt; mso - bidi - language:AR'>" + resulkhqk.Substring(i, 11) + "</span>";
                    }
                    else
                    {
                        khnrhtml += "<span lang=EN-US  style = 'font-size:10.5pt;font-family:����;mso-bidi-font-family:����; mso - font - kerning:0pt; mso - bidi - language:AR'>" + resulkhqk.Substring(i, resulkhqk.Length - i) + "</span>";
                    }


                }

                strexcel.Append(khnrhtml+ "</p></td>");


                strexcel.Append("</tr>");
                coneNum++;
            }



            //strexcel.Append(@"<tr style='mso-yfti-irow:3;height:23.05pt'>
            //                      <td width=34 style='width:25.55pt;border:solid windowtext 1.0pt;border-top:
            //                      none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                      ����;mso-bidi-font-family:����;color:black'>1<o:p></o:p></span></p>
            //                      </td>
            //                      <td width=98 style='width:73.5pt;border-top:none;border-left:none;border-bottom:
            //                      solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //                      solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //                      solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:
            //                      normal'><span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:
            //                      ����;color:blue;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                      </td>
            //                      <td width=48 style='width:36.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid black .5pt;
            //                      mso-border-left-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //                      height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                      </td>
            //                      <td width=55 style='width:41.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                      </td>
            //                      <td width=60 style='width:45.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                      </td>
            //                      <td width=62 style='width:46.5pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                      </td>
            //                      <td width=55 style='width:41.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=59 style='width:44.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=48 style='width:36.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=47 style='width:35.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=49 style='width:36.75pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                      ����;mso-bidi-font-family:����;color:blue;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=53 style='width:39.75pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                      ����;mso-bidi-font-family:����;color:blue;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=64 style='width:48.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                      ����;mso-bidi-font-family:����;color:blue;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=215 style='width:161.15pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
            //                      style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;color:blue;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                     </tr>");
            #endregion

            #region ���ƺϼ�
            // ����
            strexcel.Append(@"<tr style='mso-yfti-irow:11;height:23.05pt'>");
            // ��һ�еڶ��й̶�
            strexcel.Append(@"<td width=34 style='width:25.55pt;border:solid windowtext 1.0pt;border-top:
                                  none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  ����;mso-bidi-font-family:����'><o:p>&nbsp;</o:p></span></p>
                                  </td>
                                  <td width=98 style='width:73.5pt;border-top:none;border-left:none;border-bottom:
                                  solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
                                  solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
                                  solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                                  style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'>����<span lang=EN-US><o:p></o:p></span></span></b></p>
                                  </td>");
            // �����п�ʼ��������
            decimal allTotal = 0;
            foreach (DataRow dr in InDeptData.Rows)
            {

                strexcel.Append(@" <td width=48 style='width:36.0pt;border-top:none;border-left:none;border-bottom:
                                                solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                                mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid black .5pt;
                                                mso-border-left-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                                height:23.05pt'>
                                                <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                                vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'>");
                decimal cftotal = 0;
                if (cf.ContainsKey(dr["ENCODE"].ToString()))
                {
                    cftotal = cftotal + cf[dr["ENCODE"].ToString()];
                }
                if (jl.ContainsKey(dr["ENCODE"].ToString()))
                {
                    cftotal = cftotal + jl[dr["ENCODE"].ToString()];
                }
                allTotal += cftotal;
                strexcel.Append(cftotal);
                strexcel.Append(@" <o:p>&nbsp;</o:p></span></p></td>");
            }
            // �̶��������
            strexcel.Append(@"<td width=64 style='width:48.0pt;border-top:none;border-left:none;border-bottom:
                                  solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                  mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'>");
            strexcel.Append(allTotal);
            strexcel.Append(@"<o:p>&nbsp;</o:p></span></p>
                                  </td>
                                  <td width=215 style='width:161.15pt;border-top:none;border-left:none;
                                  border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                                  solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                  <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
                                  style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
                                  </td>");

            strexcel.Append(@"</tr>");

            // �������
            strexcel.Append(@" <tr style='mso-yfti-irow:12;height:23.05pt'>");
            // ��һ�к͵ڶ���
            strexcel.Append(@"<td width=34 rowspan=2 style='width:25.55pt;border:solid windowtext 1.0pt;
                                  border-top:none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><span style='font-size:10.5pt;font-family:����;
                                  mso-bidi-font-family:����'>����<span lang=EN-US><o:p></o:p></span></span></p>
                                  </td>
                                  <td width=98 style='width:73.5pt;border-top:none;border-left:none;border-bottom:
                                  solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
                                  solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
                                  solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                                  style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'>�������ϼ�<span lang=EN-US><o:p></o:p></span></span></b></p>
                                  </td>");
            decimal jlAlltotal = 0;
            foreach (DataRow dr in InDeptData.Rows)
            {
                strexcel.Append(@" <td width=48 style='width:36.0pt;border-top:none;border-left:none;border-bottom:
                                                solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                                mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid black .5pt;
                                                mso-border-left-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                                height:23.05pt'>
                                                <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                                vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'>");
                decimal jltotal = 0;
                if (jl.ContainsKey(dr["ENCODE"].ToString()))
                {
                    jltotal = jltotal + jl[dr["ENCODE"].ToString()];
                }
                jlAlltotal += jltotal;
                strexcel.Append(jltotal);
                strexcel.Append(@" <o:p>&nbsp;</o:p></span></p></td>");
            }
            strexcel.Append(@"<td width=64 style='width:48.0pt;border-top:none;border-left:none;border-bottom:
                                  solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                  mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'>");
            strexcel.Append(jlAlltotal);
            strexcel.Append(@"<o:p>&nbsp;</o:p></span></p>
                                  </td>
                                <td width=215 rowspan=2 style='width:161.15pt;border-top:none;border-left:
                                  none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
                                  mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid black .5pt;
                                  mso-border-alt:solid black .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                  <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
                                  font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
                                  AR'>˵��<span lang=EN-US>:</span></span></b><span lang=EN-US style='font-size:
                                  10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;
                                  mso-bidi-language:AR'>1</span><span style='font-size:10.5pt;font-family:����;
                                  mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'>������������ԪΪ��λ��С�������ֵ��ȥ��<span
                                  lang=EN-US><o:p></o:p></span></span></p>
                                  <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
                                  style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'>2</span><span style='font-size:10.5pt;font-family:
                                  ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'>���漰�������ۼ�����ʱ���������ʲ���ϵ�ṩ���������ֵ��<span
                                  lang=EN-US><o:p></o:p></span></span></p>
                                  </td>");

            strexcel.Append(@"</tr>");

            // ���˽�� 
            strexcel.Append(@" <tr style='mso-yfti-irow:12;height:23.05pt'>");
            // �ڶ���
            strexcel.Append(@" <td width=98 style='width:73.5pt;border-top:none;border-left:none;border-bottom:
                                  solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
                                  solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
                                  solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                                  style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'>���˽��ϼ�<span lang=EN-US style='color:blue'><o:p></o:p></span></span></b></p>
                                  </td>");
            decimal cfAlltotal = 0;
            foreach (DataRow dr in InDeptData.Rows)
            {
                strexcel.Append(@" <td width=48 style='width:36.0pt;border-top:none;border-left:none;border-bottom:
                                                solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                                mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid black .5pt;
                                                mso-border-left-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                                height:23.05pt'>
                                                <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                                vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'>");
                decimal cftotal = 0;
                if (cf.ContainsKey(dr["ENCODE"].ToString()))
                {
                    cftotal = cftotal + cf[dr["ENCODE"].ToString()];
                }
                cfAlltotal += cftotal;
                strexcel.Append(cftotal);
                strexcel.Append(@"<o:p>&nbsp;</o:p></span> </p></td>");
            }
            strexcel.Append(@"<td width=64 style='width:48.0pt;border-top:none;border-left:none;border-bottom:
                                  solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                  mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'>");
            strexcel.Append(cfAlltotal);
            strexcel.Append(@"<o:p>&nbsp;</o:p></span></p>
                                  </td>");
            strexcel.Append(@"</tr>");



            //strexcel.Append(@"<tr style='mso-yfti-irow:11;height:23.05pt'>
            //                      <td width=34 style='width:25.55pt;border:solid windowtext 1.0pt;border-top:
            //                      none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                      ����;mso-bidi-font-family:����'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=98 style='width:73.5pt;border-top:none;border-left:none;border-bottom:
            //                      solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //                      solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //                      solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                      style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:
            //                      0pt;mso-bidi-language:AR'>����<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      </td>
            //                      <td width=48 style='width:36.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid black .5pt;
            //                      mso-border-left-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //                      height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                      </td>
            //                      <td width=55 style='width:41.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                      </td>
            //                      <td width=60 style='width:45.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                      </td>
            //                      <td width=62 style='width:46.5pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                      </td>
            //                      <td width=55 style='width:41.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=59 style='width:44.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=48 style='width:36.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=47 style='width:35.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=49 style='width:36.75pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=53 style='width:39.75pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=64 style='width:48.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=215 style='width:161.15pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
            //                      style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:
            //                      0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                     </tr>
            //                     <tr style='mso-yfti-irow:12;height:23.05pt'>
            //                      <td width=34 rowspan=2 style='width:25.55pt;border:solid windowtext 1.0pt;
            //                      border-top:none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><span style='font-size:10.5pt;font-family:����;
            //                      mso-bidi-font-family:����'>����<span lang=EN-US><o:p></o:p></span></span></p>
            //                      </td>
            //                      <td width=98 style='width:73.5pt;border-top:none;border-left:none;border-bottom:
            //                      solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //                      solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //                      solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                      style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:
            //                      0pt;mso-bidi-language:AR'>�������ϼ�<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      </td>
            //                      <td width=48 style='width:36.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid black .5pt;
            //                      mso-border-left-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //                      height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><b
            //                      style='mso-bidi-font-weight:normal'><span lang=EN-US style='font-size:10.5pt;
            //                      font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'><o:p>&nbsp;</o:p></span></b></p>
            //                      </td>
            //                      <td width=55 style='width:41.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><b
            //                      style='mso-bidi-font-weight:normal'><span lang=EN-US style='font-size:10.5pt;
            //                      font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'><o:p>&nbsp;</o:p></span></b></p>
            //                      </td>
            //                      <td width=60 style='width:45.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><b
            //                      style='mso-bidi-font-weight:normal'><span lang=EN-US style='font-size:10.5pt;
            //                      font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'><o:p>&nbsp;</o:p></span></b></p>
            //                      </td>
            //                      <td width=62 style='width:46.5pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><b
            //                      style='mso-bidi-font-weight:normal'><span lang=EN-US style='font-size:10.5pt;
            //                      font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'><o:p>&nbsp;</o:p></span></b></p>
            //                      </td>
            //                      <td width=55 style='width:41.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=59 style='width:44.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=48 style='width:36.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=47 style='width:35.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=49 style='width:36.75pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=53 style='width:39.75pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=64 style='width:48.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=215 rowspan=2 style='width:161.15pt;border-top:none;border-left:
            //                      none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
            //                      mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid black .5pt;
            //                      mso-border-alt:solid black .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>˵��<span lang=EN-US>:</span></span></b><span lang=EN-US style='font-size:
            //                      10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;
            //                      mso-bidi-language:AR'>1</span><span style='font-size:10.5pt;font-family:����;
            //                      mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'>������������ԪΪ��λ��С�������ֵ��ȥ��<span
            //                      lang=EN-US><o:p></o:p></span></span></p>
            //                      <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
            //                      style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:
            //                      0pt;mso-bidi-language:AR'>2</span><span style='font-size:10.5pt;font-family:
            //                      ����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:AR'>���漰�������ۼ�����ʱ���������ʲ���ϵ�ṩ���������ֵ��<span
            //                      lang=EN-US><o:p></o:p></span></span></p>
            //                      </td>
            //                     </tr>
            //                     <tr style='mso-yfti-irow:13;mso-yfti-lastrow:yes;height:23.05pt'>
            //                      <td width=98 style='width:73.5pt;border-top:none;border-left:none;border-bottom:
            //                      solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //                      solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //                      solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                      style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;mso-font-kerning:
            //                      0pt;mso-bidi-language:AR'>���˽��ϼ�<span lang=EN-US style='color:blue'><o:p></o:p></span></span></b></p>
            //                      </td>
            //                      <td width=48 style='width:36.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid black .5pt;
            //                      mso-border-left-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //                      height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><b
            //                      style='mso-bidi-font-weight:normal'><span lang=EN-US style='font-size:10.5pt;
            //                      font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'><o:p>&nbsp;</o:p></span></b></p>
            //                      </td>
            //                      <td width=55 style='width:41.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><b
            //                      style='mso-bidi-font-weight:normal'><span lang=EN-US style='font-size:10.5pt;
            //                      font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'><o:p>&nbsp;</o:p></span></b></p>
            //                      </td>
            //                      <td width=60 style='width:45.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><b
            //                      style='mso-bidi-font-weight:normal'><span lang=EN-US style='font-size:10.5pt;
            //                      font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'><o:p>&nbsp;</o:p></span></b></p>
            //                      </td>
            //                      <td width=62 style='width:46.5pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><b
            //                      style='mso-bidi-font-weight:normal'><span lang=EN-US style='font-size:10.5pt;
            //                      font-family:����;mso-bidi-font-family:����;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'><o:p>&nbsp;</o:p></span></b></p>
            //                      </td>
            //                      <td width=55 style='width:41.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=59 style='width:44.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=48 style='width:36.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=47 style='width:35.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=49 style='width:36.75pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                      color:blue;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=53 style='width:39.75pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                      color:blue;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=64 style='width:48.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:����;mso-bidi-font-family:����;
            //                      color:blue;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                     </tr>");
            #endregion

            strexcel.Append(@" </table>");
            builder.InsertHtml(strexcel.ToString());

            doc.Save(resp, Server.UrlEncode("�ڲ����ſ��˱�_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".doc"), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
        }

        #endregion
    }
}
