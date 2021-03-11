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
using Aspose.Cells;
using System.Drawing;
using BSFramework.Data;
using BSFramework.Util.Extension;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// �� �����ճ����˱�
    /// </summary>
    public class DailyexamineController : MvcControllerBase
    {
        private DailyexamineBLL dailyexaminebll = new DailyexamineBLL();
        private PeopleReviewBLL peoplereviewbll = new PeopleReviewBLL();
        private AptitudeinvestigateauditBLL aptitudeinvestigateauditbll = new AptitudeinvestigateauditBLL();
        private HistorydailyexamineBLL historydailyexaminebll = new HistorydailyexamineBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();


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
            ViewBag.Code = dailyexaminebll.GetMaxCode();
            return View();
        }

        /// <summary>
        /// ���ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ApproveForm()
        {
            return View();
        }

        /// <summary>
        /// ��ʷ�嵥ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HistoryIndex()
        {
            return View();
        }
        /// <summary>
        /// ���˻���
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExamineCollent()
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
            Operator user = OperatorProvider.Provider.Current();
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "id";
            pagination.p_fields = "examinecode,examinetype,examinetodept,examinemoney,examineperson,remark,flowdept,flowrolename,createuserid,createuserdeptcode,createuserorgcode,issaved,isover,examinecontent";
            pagination.p_tablename = " epg_dailyexamine";
            pagination.conditionJson = "1=1";
            //pagination.sidx = "createdate";
            //pagination.sord = "desc";
            if (!user.IsSystem)
            {
                pagination.conditionJson += " and createuserorgcode='" + user.OrganizeCode + "'";
            }
            var data = dailyexaminebll.GetPageList(pagination, queryJson);
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
        public ActionResult GetHistoryPageListJson(Pagination pagination, string queryJson)
        {
            Operator user = OperatorProvider.Provider.Current();
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "id";
            pagination.p_fields = "examinecode,examinetype,examinetodept,examinemoney,examineperson,remark,flowdept,flowrolename,createuserid,createuserdeptcode,createuserorgcode,issaved,isover,examinecontent";
            pagination.p_tablename = " epg_historydailyexamine";
            pagination.conditionJson = "1=1";
            //pagination.sidx = "createdate";
            //pagination.sord = "desc";
            if (!user.IsSystem)
            {
                pagination.conditionJson += " and createuserorgcode='" + user.OrganizeCode + "'";
            }
            var data = dailyexaminebll.GetPageList(pagination, queryJson);
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
            var data = dailyexaminebll.GetList(queryJson);
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
            try
            {
                var data = dailyexaminebll.GetEntity(keyValue);
                var exDept = new DepartmentBLL().GetEntity(data.ExamineToDeptId);
                if (exDept != null)
                {
                    if (exDept.Nature == "�а���")
                    {
                        var result = new
                        {
                            data = data,
                            iscbs = true
                        };
                        return ToJsonResult(result);
                    }
                    else
                    {
                        var result = new
                        {
                            data = data,
                            iscbs = false
                        };
                        return ToJsonResult(result);
                    }
                }
                else
                {
                    return ToJsonResult(data);
                }
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
        public ActionResult GetHistoryFormJson(string keyValue)
        {
            //var data = historydailyexaminebll.GetEntity(keyValue);
            //return ToJsonResult(data);
            try
            {
                var data = historydailyexaminebll.GetEntity(keyValue);
                var exDept = new DepartmentBLL().GetEntity(data.ExamineToDeptId);
                if (exDept != null)
                {
                    if (exDept.Nature == "�а���")
                    {
                        var result = new
                        {
                            data = data,
                            iscbs = true
                        };
                        return ToJsonResult(result);
                    }
                    else
                    {
                        var result = new
                        {
                            data = data,
                            iscbs = false
                        };
                        return ToJsonResult(result);
                    }
                }
                else
                {
                    return ToJsonResult(data);
                }
            }
            catch (Exception ex)
            {

                return Error(ex.Message);
            }
        }
        /// <summary>
        /// �ճ����˻���
        /// </summary>
        /// <param name="pagination">��ѯ���</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetExamineCollent(Pagination pagination, string queryJson)
        {
            Operator user = OperatorProvider.Provider.Current();
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "t.id";
            var table = @"(select 
                                        examinetodeptid,wm_concat(id) id,
                                        examinetodept,max(createdate) createdate,
                                          wm_concat(distinct(examineperson)) examineperson,
                                           to_char(min(examinetime),'yyyy-MM-dd')||'~'|| to_char(max(examinetime),'yyyy-MM-dd') examinetime,
                                        sum(examinemoney) examinemoney,
                                        wm_concat(examinetype) examinetype,createuserorgcode
                                        from epg_dailyexamine t where 1=1 {0} group by examinetodeptid,examinetodept,createuserorgcode) t";
            var strWhere = string.Empty;
            pagination.p_fields = @" t.examinetodeptid,
                                       t.examinetodept,
                                       t.examineperson,
                                       t.examinetime,
                                       t.examinemoney,
                                       t.examinetype";
          
            pagination.conditionJson = "1=1";
            pagination.sidx = "t.createdate";
            pagination.sord = "desc";
            if (!user.IsSystem)
            {
                pagination.conditionJson += " and t.createuserorgcode='" + user.OrganizeCode + "'";
            }
            if (!string.IsNullOrEmpty(queryJson))
            {
                var queryParam = queryJson.ToJObject();
                if (!queryParam["examinetodeptid"].IsEmpty())
                {
                    strWhere += " and t.examinetodeptid ='" + queryParam["examinetodeptid"].ToString() + "'";
                }
                if (!queryParam["examinetype"].IsEmpty())
                {
                    strWhere += " and t.examinetype='" + queryParam["examinetype"].ToString() + "'";
                }
                if (!queryParam["examinecontent"].IsEmpty())
                {
                    strWhere += " and t.examinecontent like '%" + queryParam["examinecontent"].ToString() + "%'";
                }
                //��ʼʱ��
                if (!queryParam["sTime"].IsEmpty())
                {
                    strWhere += string.Format(@" and t.examinetime >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["sTime"].ToString());
                }
                //����ʱ��
                if (!queryParam["eTime"].IsEmpty())
                {
                    strWhere += string.Format(@" and t.examinetime < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["eTime"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
                }
            }
            table = string.Format(table, strWhere);
            pagination.p_tablename = table;
            var data = dailyexaminebll.GetExamineCollent(pagination, queryJson);
            var jsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return ToJsonResult(jsonData);
            //var data = historydailyexaminebll.GetExamineCollent(keyValue);
            //return ToJsonResult(data);
        }

        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [HandlerMonitor(3, "ɾ������")]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            dailyexaminebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, DailyexamineEntity entity)
        {
            try
            {
                entity.IsOver = 0;
                entity.IsSaved = 0;
                dailyexaminebll.SaveForm(keyValue, entity);
                return Success("�����ɹ���");
            }
            catch (Exception)
            {
                return Error("����ʧ�ܡ�");
            }
        }


        #endregion


        #region �Ǽǵ������ύ����˻��߽���
        /// <summary>
        /// �Ǽǵ������ύ����˻��߽������ύ����һ�����̣�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, DailyexamineEntity entity)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            string state = string.Empty;

            string moduleName = "�ճ�����";

            /// <param name="currUser">��ǰ��¼��</param>
            /// <param name="state">�Ƿ���Ȩ����� 1������� 0 ���������</param>
            /// <param name="moduleName">ģ������</param>
            /// <param name="outengineerid">����Id</param>
            ManyPowerCheckEntity mpcEntity = dailyexaminebll.CheckAuditPower(curUser, out state, moduleName, curUser.DeptId);

            string flowid = string.Empty;
            List<ManyPowerCheckEntity> powerList = new ManyPowerCheckBLL().GetListBySerialNum(curUser.OrganizeCode, moduleName);
            List<ManyPowerCheckEntity> checkPower = new List<ManyPowerCheckEntity>();
            //�Ȳ��ִ�в��ű���
            for (int i = 0; i < powerList.Count; i++)
            {
                if (powerList[i].CHECKDEPTCODE == "-3" || powerList[i].CHECKDEPTID == "-3")
                {
                    if (curUser.RoleName.Contains("����") || curUser.RoleName.Contains("רҵ"))
                    {
                        var pDept = new DepartmentBLL().GetParentDeptBySpecialArgs(curUser.ParentId, "����");
                        powerList[i].CHECKDEPTCODE = pDept.EnCode;
                        powerList[i].CHECKDEPTID = pDept.DepartmentId;
                    }
                    else
                    {
                        powerList[i].CHECKDEPTCODE = new DepartmentBLL().GetEntity(curUser.DeptId).EnCode;
                        powerList[i].CHECKDEPTID = new DepartmentBLL().GetEntity(curUser.DeptId).DepartmentId;
                    }
                }
            }
            //��¼���Ƿ������Ȩ��--�����Ȩ��ֱ�����ͨ��
            for (int i = 0; i < powerList.Count; i++)
            {
                if (powerList[i].CHECKDEPTID == curUser.DeptId)
                {
                    var rolelist = curUser.RoleName.Split(',');
                    for (int j = 0; j < rolelist.Length; j++)
                    {
                        if (powerList[i].CHECKROLENAME.Contains(rolelist[j]))
                        {
                            checkPower.Add(powerList[i]);
                            break;
                        }
                    }
                }
            }
            if (checkPower.Count > 0)
            {
                ManyPowerCheckEntity check = checkPower.Last();//��ǰ

                for (int i = 0; i < powerList.Count; i++)
                {
                    if (check.ID == powerList[i].ID)
                    {
                        flowid = powerList[i].ID;
                    }
                }
            }
            //if (curUser.RoleName.Contains("��˾���û�"))
            //{
            //    mpcEntity = null;
            //}
            if (null != mpcEntity)
            {
                //��������������¼
                entity.FlowDept = mpcEntity.CHECKDEPTID;
                entity.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                entity.FlowRole = mpcEntity.CHECKROLEID;
                entity.FlowRoleName = mpcEntity.CHECKROLENAME;
                entity.IsSaved = 1; //����Ѿ��ӵǼǵ���˽׶�
                entity.IsOver = 0; //����δ��ɣ�1��ʾ���
                entity.FlowID = mpcEntity.ID;
                entity.FlowName = mpcEntity.CHECKDEPTNAME + "�����";
            }
            else  //Ϊ�����ʾ�Ѿ��������
            {
                entity.FlowDept = "";
                entity.FlowDeptName = "";
                entity.FlowRole = "";
                entity.FlowRoleName = "";
                entity.IsSaved = 1; //����Ѿ��ӵǼǵ���˽׶�
                entity.IsOver = 1; //����δ��ɣ�1��ʾ���
                entity.FlowName = "";
                entity.FlowID = flowid;
            }
            dailyexaminebll.SaveForm(keyValue, entity);

            //�����˼�¼
            if (state == "1")
            {
                //�����Ϣ��
                AptitudeinvestigateauditEntity aidEntity = new AptitudeinvestigateauditEntity();
                aidEntity.AUDITRESULT = "0"; //ͨ��
                aidEntity.AUDITTIME = DateTime.Now;
                aidEntity.AUDITPEOPLE = curUser.UserName;
                aidEntity.AUDITPEOPLEID = curUser.UserId;
                aidEntity.APTITUDEID = entity.Id;  //������ҵ��ID 
                aidEntity.AUDITOPINION = ""; //������
                aidEntity.AUDITSIGNIMG = curUser.SignImg;
                aidEntity.FlowId = flowid;
                if (null != mpcEntity)
                {
                    aidEntity.REMARK = (mpcEntity.AUTOID.Value - 1).ToString(); //��ע �����̵�˳���
                }
                else
                {
                    aidEntity.REMARK = "7";
                }
                aidEntity.AUDITDEPTID = curUser.DeptId;
                aidEntity.AUDITDEPT = curUser.DeptName;
                aptitudeinvestigateauditbll.SaveForm(aidEntity.ID, aidEntity);
            }

            return Success("�����ɹ�!");
        }
        #endregion

        #region �ύ����˻��߽���
        /// <summary>
        /// �Ǽǵ������ύ����˻��߽������ύ����һ�����̣�
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

            string moduleName = "�ճ�����";

            DailyexamineEntity entity = dailyexaminebll.GetEntity(keyValue);
            /// <param name="currUser">��ǰ��¼��</param>
            /// <param name="state">�Ƿ���Ȩ����� 1������� 0 ���������</param>
            /// <param name="moduleName">ģ������</param>
            /// <param name="createdeptid">�����˲���ID</param>
            ManyPowerCheckEntity mpcEntity = dailyexaminebll.CheckAuditPower(curUser, out state, moduleName, entity.CreateUserDeptId);


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
            aidEntity.FlowId = entity.FlowID;
            aidEntity.AUDITSIGNIMG = HttpUtility.UrlDecode(aidEntity.AUDITSIGNIMG);
            aidEntity.AUDITSIGNIMG = string.IsNullOrWhiteSpace(aentity.AUDITSIGNIMG) ? "" : aentity.AUDITSIGNIMG.ToString().Replace("../..", "");
            if (null != mpcEntity)
            {
                aidEntity.REMARK = (mpcEntity.AUTOID.Value - 1).ToString(); //��ע �����̵�˳���
            }
            else
            {
                aidEntity.REMARK = "7";
            }
            aptitudeinvestigateauditbll.SaveForm(aidEntity.ID, aidEntity);
            #endregion

            #region  //�����ճ�����
            //���ͨ��
            if (aentity.AUDITRESULT == "0")
            {
                //0��ʾ����δ��ɣ�1��ʾ���̽���
                if (null != mpcEntity)
                {
                    entity.FlowDept = mpcEntity.CHECKDEPTID;
                    entity.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                    entity.FlowRole = mpcEntity.CHECKROLEID;
                    entity.FlowRoleName = mpcEntity.CHECKROLENAME;
                    entity.IsSaved = 1;
                    entity.IsOver = 0;
                    entity.FlowID = mpcEntity.ID;
                    entity.FlowName = mpcEntity.CHECKDEPTNAME + "�����";
                }
                else
                {
                    entity.FlowDept = "";
                    entity.FlowDeptName = "";
                    entity.FlowRole = "";
                    entity.FlowRoleName = "";
                    entity.IsSaved = 1;
                    entity.IsOver = 1;
                    entity.FlowName = "";
                }
            }
            else //��˲�ͨ�� 
            {
                entity.FlowDept = "";
                entity.FlowDeptName = "";
                entity.FlowRole = "";
                entity.FlowRoleName = "";
                entity.IsOver = 0; //���ڵǼǽ׶�
                entity.IsSaved = 0; //�Ƿ����״̬��ֵΪδ���
                entity.FlowName = "";
                entity.FlowID = "";

            }
            //�����ճ����˻���״̬��Ϣ
            dailyexaminebll.SaveForm(keyValue, entity);
            #endregion

            #region    //��˲�ͨ��
            if (aentity.AUDITRESULT == "1")
            {
                //�����ʷ��¼
                HistorydailyexamineEntity hsentity = new HistorydailyexamineEntity();
                hsentity.CreateUserId = entity.CreateUserId;
                hsentity.CreateUserDeptCode = entity.CreateUserDeptCode;
                hsentity.CreateUserOrgCode = entity.CreateUserOrgCode;
                hsentity.CreateDate = entity.CreateDate;
                hsentity.CreateUserName = entity.CreateUserName;
                hsentity.CreateUserDeptId = entity.CreateUserDeptId;
                hsentity.ModifyDate = entity.ModifyDate;
                hsentity.ModifyUserId = entity.ModifyUserId;
                hsentity.ModifyUserName = entity.ModifyUserName;
                hsentity.ExamineCode = entity.ExamineCode;
                hsentity.ExamineDept = entity.ExamineDept;
                hsentity.ExamineDeptId = entity.ExamineDeptId;
                hsentity.ExamineToDeptId = entity.ExamineToDeptId;
                hsentity.ExamineToDept = entity.ExamineToDept;
                hsentity.ExamineType = entity.ExamineType; //����ID
                hsentity.ExamineMoney = entity.ExamineMoney;
                hsentity.ExaminePerson = entity.ExaminePerson;
                hsentity.ExaminePersonId = entity.ExaminePersonId; //����ID
                hsentity.ExamineTime = entity.ExamineTime;
                hsentity.ExamineContent = entity.ExamineContent;
                hsentity.ExamineBasis = entity.ExamineBasis;
                hsentity.Remark = entity.Remark;
                hsentity.ContractId = entity.Id;//����ID
                hsentity.IsSaved = 2;
                hsentity.IsOver = entity.IsOver;
                hsentity.FlowDeptName = entity.FlowDeptName;
                hsentity.FlowDept = entity.FlowDept;
                hsentity.FlowRoleName = entity.FlowRoleName;
                hsentity.FlowRole = entity.FlowRole;
                hsentity.FlowName = entity.FlowName;
                hsentity.Project = entity.Project;
                hsentity.ProjectId = entity.ProjectId;
                hsentity.Id = "";

                historydailyexaminebll.SaveForm(hsentity.Id, hsentity);

                //��ȡ��ǰҵ������������˼�¼
                var shlist = aptitudeinvestigateauditbll.GetAuditList(keyValue);
                //����������˼�¼����ID
                foreach (AptitudeinvestigateauditEntity mode in shlist)
                {
                    mode.APTITUDEID = hsentity.Id; //��Ӧ�µ�ID
                    aptitudeinvestigateauditbll.SaveForm(mode.ID, mode);
                }
                //�������¸�����¼����ID
                var flist = fileinfobll.GetImageListByObject(keyValue);
                foreach (FileInfoEntity fmode in flist)
                {
                    fmode.RecId = hsentity.Id; //��Ӧ�µ�ID
                    fileinfobll.SaveForm("", fmode);
                }
            }
            #endregion

            return Success("�����ɹ�!");
        }
        #endregion

        #region ��������
        /// <summary>
        /// ����
        /// </summary>
        [HandlerMonitor(0, "��������")]
        public ActionResult ExportData(string queryJson)
        {
            try
            {
                var queryParam = queryJson.ToJObject();
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.p_kid = "id";
                pagination.p_fields = "examinecode,examinetype,examinecontent,examinetodept,examinemoney,examineperson,remark";
                pagination.p_tablename = " epg_dailyexamine";
                pagination.conditionJson = "1=1";
                pagination.sidx = "createdate";
                pagination.sord = "desc";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (!user.IsSystem)
                {
                    pagination.conditionJson += " and createuserorgcode='" + user.OrganizeCode + "'";
                }
                DataTable exportTable = dailyexaminebll.GetPageList(pagination, queryJson);
                //���õ�����ʽ
                ExcelConfig excelconfig = new ExcelConfig();

                excelconfig.Title = "�ճ�����";
                excelconfig.FileName = "�ճ�������Ϣ����.xls";
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 16;

                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //�������Դ��˳�򱣳�һ��
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "examinecode", ExcelColumn = "���˱��", Width = 120 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "examinetype", ExcelColumn = "�������", Width = 120 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "examinecontent", ExcelColumn = "��������", Width = 400 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "examinetodept", ExcelColumn = "�����˵�λ", Width = 160 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "examinemoney", ExcelColumn = "���˽��", Width = 120 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "examineperson", ExcelColumn = "������", Width = 160 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "remark", ExcelColumn = "��ע", Width = 400 });

                //���õ�������
                //ExcelHelper.ExcelDownload(exportTable, excelconfig);
                ExcelHelper.ExportByAspose(exportTable, excelconfig.FileName, excelconfig.ColumnEntity);
            }
            catch (Exception ex)
            {

            }
            return Success("�����ɹ���");
        }

        /// <summary>
        /// �������˻���
        /// </summary>
        [HandlerMonitor(0, "��������")]
        public ActionResult ExportExamineData(string queryJson)
        {
            try
            {
                Operator user = OperatorProvider.Provider.Current();
                
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;

                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "t.id";
                var table = @"(select 
                                        examinetodeptid,wm_concat(id) id,
                                        examinetodept,max(createdate) createdate,
                                          wm_concat(distinct(examineperson)) examineperson,
                                           to_char(min(examinetime),'yyyy-MM-dd')||'~'|| to_char(max(examinetime),'yyyy-MM-dd') examinetime,
                                        sum(examinemoney) examinemoney,
                                        wm_concat(examinetype) examinetype,createuserorgcode
                                        from epg_dailyexamine t where 1=1 {0} group by examinetodeptid,examinetodept,createuserorgcode) t";
                var strWhere = string.Empty;
                pagination.p_fields = @" t.examinetodeptid,
                                       t.examinetodept,
                                       t.examineperson,
                                       t.examinetime,
                                       t.examinemoney,
                                       t.examinetype";

                pagination.conditionJson = "1=1";
                pagination.sidx = "t.createdate";
                pagination.sord = "desc";
                if (!user.IsSystem)
                {
                    pagination.conditionJson += " and t.createuserorgcode='" + user.OrganizeCode + "'";
                }
                if (!string.IsNullOrEmpty(queryJson))
                {
                    var queryParam = queryJson.ToJObject();
                    if (!queryParam["examinetodeptid"].IsEmpty())
                    {
                        strWhere += " and t.examinetodeptid ='" + queryParam["examinetodeptid"].ToString() + "'";
                    }
                    if (!queryParam["examinetype"].IsEmpty())
                    {
                        strWhere += " and t.examinetype='" + queryParam["examinetype"].ToString() + "'";
                    }
                    if (!queryParam["examinecontent"].IsEmpty())
                    {
                        strWhere += " and t.examinecontent like '%" + queryParam["examinecontent"].ToString() + "%'";
                    }
                    //��ʼʱ��
                    if (!queryParam["sTime"].IsEmpty())
                    {
                        strWhere += string.Format(@" and t.examinetime >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["sTime"].ToString());
                    }
                    //����ʱ��
                    if (!queryParam["eTime"].IsEmpty())
                    {
                        strWhere += string.Format(@" and t.examinetime < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["eTime"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
                    }
                }
                table = string.Format(table, strWhere);
                pagination.p_tablename = table;
                var data = dailyexaminebll.GetExportExamineCollent(pagination, queryJson);
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                string fName = "�ճ����˻���_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                wb.Open(Server.MapPath("~/Resource/ExcelTemplate/tmp.xls"));
                var num = wb.Worksheets[0].Cells.Columns.Count;

                Aspose.Cells.Worksheet sheet = wb.Worksheets[0] as Aspose.Cells.Worksheet;
                Aspose.Cells.Cell cell = sheet.Cells[0, 0];
                cell.PutValue("���˻��ܱ�"); //����
                cell.Style.Pattern = BackgroundType.Solid;
                cell.Style.Font.Size = 16;
                cell.Style.Font.Color = Color.Black;
                List<string> colList = new List<string>() { "�����˵�λ", "���˽��", "��������", "������", "����ʱ��" };
                List<string> colList1 = new List<string>() { "examinetodept", "examinemoney", "examinetype", "examineperson", "examinetime" };
                for (int i = 0; i < colList.Count; i++)
                {
                    //�����
                    Aspose.Cells.Cell serialcell = sheet.Cells[1, 0];
                    serialcell.PutValue(" ");

                    for (int j = 0; j < colList.Count; j++)
                    {
                        Aspose.Cells.Cell curcell = sheet.Cells[1, j + 1];
                        sheet.Cells.SetColumnWidth(j + 1, 40);
                        curcell.Style.Pattern = BackgroundType.Solid;
                        curcell.Style.Font.Size = 12;
                        curcell.Style.Font.Color = Color.Black;
                        curcell.PutValue(colList[j].ToString()); //��ͷ
                    }
                    Aspose.Cells.Cells cells = sheet.Cells;
                    cells.Merge(0, 0, 1, colList.Count + 1);
                }
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    //���к�
                    Aspose.Cells.Cell serialcell = sheet.Cells[i + 2, 0];
                    if (string.IsNullOrWhiteSpace(data.Rows[i]["parent"].ToString())) {
                        serialcell.PutValue("�ϼ�");
                    }
                    
                    //�������
                    for (int j = 0; j < colList1.Count; j++)
                    {
                        Aspose.Cells.Cell curcell = sheet.Cells[i + 2, j + 1];
                        curcell.PutValue(data.Rows[i][colList1[j]].ToString());
                    }
                   
                }
                HttpResponse resp = System.Web.HttpContext.Current.Response;
                wb.Save(Server.MapPath("~/Resource/Temp/" + fName));
                return Success("�����ɹ���", fName);

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
