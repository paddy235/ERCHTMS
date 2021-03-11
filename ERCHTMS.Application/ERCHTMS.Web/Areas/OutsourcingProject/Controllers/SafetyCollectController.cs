using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.SystemManage;
using System.Linq;
using System;
using System.Data;
using System.Web;
using Aspose.Words;
using Aspose.Words.Tables;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// �� �������-��ȫ����
    /// </summary>
    public class SafetyCollectController : MvcControllerBase
    {
        private SafetyCollectBLL SafetyCollectbll = new SafetyCollectBLL();
        private AptitudeinvestigateauditBLL aptitudeinvestigateauditbll = new AptitudeinvestigateauditBLL();
        private DailyexamineBLL dailyexaminebll = new DailyexamineBLL();
        private AptitudeinvestigateinfoBLL aptitudeinvestigateinfobll = new AptitudeinvestigateinfoBLL();
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL(); //����ҵ�����
        private UserBLL userbll = new UserBLL(); //�û���������
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private OutsouringengineerBLL outsouringengineerbll = new OutsouringengineerBLL();
        private OutsourcingprojectBLL outProjectbll = new OutsourcingprojectBLL();
        private DepartmentBLL departmentbll = new DepartmentBLL();
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
            var data = SafetyCollectbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>   
        //[HandlerMonitor(3, "��ҳ��ѯ�û���Ϣ!")]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            queryJson = queryJson ?? "";
            pagination.p_kid = "t.Id";
            pagination.p_fields = "t.EngineerId,t.CREATEUSERID,t.CREATEUSERDEPTCODE,t.CREATEUSERORGCODE,t.CREATEDATE,t.CREATEUSERNAME,o.engineerletdept,o.engineername,p.outsourcingname,t.FLOWNAME,t.FLOWROLE,t.FLOWROLENAME,t.FLOWDEPT,t.FLOWDEPTNAME,t.ISSAVED,t.ISOVER,t.FlowId";
            pagination.p_tablename = "EPG_SAFETYCOLLECT t left join EPG_OUTSOURINGENGINEER o on t.engineerid=o.id left join EPG_OUTSOURCINGPROJECT p on o.outprojectid=p.outprojectid";
            //pagination.conditionJson = "1=1";

            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string role = user.RoleName;
            string allrangedept = "";
            try
            {
                allrangedept = dataitemdetailbll.GetDataItemByDetailCode("SBDept", "SBDeptId").FirstOrDefault().ItemValue;
            }
            catch (Exception)
            {

            }

            if (role.Contains("ʡ��"))
            {
                pagination.conditionJson = string.Format(@" t.createuserorgcode  in (select encode
                from BASE_DEPARTMENT d
                        where d.deptcode like '{0}%' and d.nature = '����' and d.description is null)", user.NewDeptCode);
            }
            else if (role.Contains("��˾���û�") || role.Contains("���������û�") || allrangedept.Contains(user.DeptId))
            {
                pagination.conditionJson = string.Format(" t.createuserorgcode  = '{0}'", user.OrganizeCode);
            }
            else if (role.Contains("�а��̼��û�"))
            {
                pagination.conditionJson = string.Format(" (o.outprojectid ='{0}' or o.supervisorid='{0}' or t.createuserid = '{1}')", user.DeptId, user.UserId);
            }
            else
            {
                var deptentity = departmentbll.GetEntity(user.DeptId);
                while (deptentity.Nature == "����" || deptentity.Nature == "רҵ")
                {
                    deptentity = departmentbll.GetEntity(deptentity.ParentId);
                }
                pagination.conditionJson = string.Format(" o.engineerletdeptid  in (select departmentid from base_department where encode like '{0}%') ", deptentity.EnCode);

            }

            var watch = CommonHelper.TimerStart();
            var data = SafetyCollectbll.GetPageList(pagination, queryJson);
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
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = SafetyCollectbll.GetEntity(keyValue);
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
            SafetyCollectbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SafetyCollectEntity entity)
        {
            entity.ISSAVED = "0"; //���������
            SafetyCollectbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
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
        public ActionResult SubmitForm(string keyValue, SafetyCollectEntity entity)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            string state = string.Empty;

            string flowid = string.Empty;

            string moduleName = "������ȫ����";

            // <param name="state">�Ƿ���Ȩ����� 1������� 0 ���������</param>
            ManyPowerCheckEntity mpcEntity = dailyexaminebll.CheckAuditPower(curUser, out state, moduleName, curUser.DeptId);

            //����ʱ����ݽ�ɫ�Զ����
            List<ManyPowerCheckEntity> powerList = new ManyPowerCheckBLL().GetListBySerialNum(curUser.OrganizeCode, "������ȫ����");
            List<ManyPowerCheckEntity> checkPower = new List<ManyPowerCheckEntity>();
            var outsouringengineer = outsouringengineerbll.GetEntity(entity.EngineerId);
            //�Ȳ��ִ�в��ű���
            for (int i = 0; i < powerList.Count; i++)
            {
                if (powerList[i].CHECKDEPTCODE == "-1" || powerList[i].CHECKDEPTID == "-1")
                {
                    var createdeptentity = new DepartmentBLL().GetEntity(outsouringengineer.ENGINEERLETDEPTID);
                    var createdeptentity2 = new DepartmentEntity();
                    while (createdeptentity.Nature == "רҵ" || createdeptentity.Nature == "����")
                    {
                        createdeptentity2 = new DepartmentBLL().GetEntity(createdeptentity.ParentId);
                        if (createdeptentity2.Nature != "רҵ" || createdeptentity2.Nature != "����") {
                            break;
                        }
                    }
                    powerList[i].CHECKDEPTCODE = createdeptentity.DeptCode;
                    powerList[i].CHECKDEPTID = createdeptentity.DepartmentId;
                    if (createdeptentity2 != null) {
                        powerList[i].CHECKDEPTCODE = createdeptentity.DeptCode + "," + createdeptentity2.DeptCode;
                        powerList[i].CHECKDEPTID = createdeptentity.DepartmentId + "," + createdeptentity2.DepartmentId;
                    }
                }
                //��������
                if (powerList[i].CHECKDEPTCODE == "-3" || powerList[i].CHECKDEPTID == "-3")
                {
                    var createdeptentity = new DepartmentBLL().GetEntityByCode(curUser.DeptCode);
                    while (createdeptentity.Nature == "רҵ" || createdeptentity.Nature == "����")
                    {
                        createdeptentity = new DepartmentBLL().GetEntity(createdeptentity.ParentId);
                    }
                    powerList[i].CHECKDEPTCODE = createdeptentity.DeptCode;
                    powerList[i].CHECKDEPTID = createdeptentity.DepartmentId;
                }
            }
            //��¼���Ƿ������Ȩ��--�����Ȩ��ֱ�����ͨ��
            for (int i = 0; i < powerList.Count; i++)
            {
                if (powerList[i].CHECKDEPTID.Contains(curUser.DeptId))
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
                state = "1";
                ManyPowerCheckEntity check = checkPower.Last();//��ǰ

                for (int i = 0; i < powerList.Count; i++)
                {
                    if (check.ID == powerList[i].ID)
                    {
                        flowid = powerList[i].ID;
                    }
                }
            }
            else
            {
                state = "0";
                mpcEntity = powerList.First();
            }
            if (null != mpcEntity)
            {
                entity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                entity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                entity.FLOWROLE = mpcEntity.CHECKROLEID;
                entity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                entity.ISSAVED = "1"; //����Ѿ��ӵǼǵ���˽׶�
                entity.ISOVER = "0"; //����δ��ɣ�1��ʾ���
                //entity.FLOWNAME = entity.FLOWDEPTNAME + "�����";
                if (mpcEntity.CHECKDEPTNAME == "ִ�в���" && mpcEntity.CHECKROLENAME == "������")
                {
                    entity.FLOWNAME = outsouringengineer.ENGINEERLETDEPT + "������";
                }
                else
                {
                    entity.FLOWNAME = mpcEntity.CHECKDEPTNAME + "������";
                }
                entity.FlowId = mpcEntity.ID;
                
            }
            else  //Ϊ�����ʾ�Ѿ��������
            {
                entity.FLOWDEPT = "";
                entity.FLOWDEPTNAME = "";
                entity.FLOWROLE = "";
                entity.FLOWROLENAME = "";
                entity.ISSAVED = "1"; //����Ѿ��ӵǼǵ���˽׶�
                entity.ISOVER = "1"; //����δ��ɣ�1��ʾ���
                entity.FLOWNAME = "";
                entity.FlowId = flowid;
            }
            SafetyCollectbll.SaveForm(keyValue, entity);

            //�����˼�¼
            if (state == "1")
            {
                //�����Ϣ��
                AptitudeinvestigateauditEntity aidEntity = new AptitudeinvestigateauditEntity();
                aidEntity.AUDITRESULT = "0"; //ͨ��
                aidEntity.AUDITTIME = DateTime.Now;
                aidEntity.AUDITPEOPLE = curUser.UserName;
                aidEntity.AUDITPEOPLEID = curUser.UserId;
                aidEntity.APTITUDEID = entity.ID;  //������ҵ��ID 
                aidEntity.AUDITOPINION = ""; //������
                aidEntity.AUDITSIGNIMG = curUser.SignImg;
                if (null != mpcEntity)
                {
                    aidEntity.REMARK = (powerList[0].AUTOID.Value - 1).ToString(); //��ע �����̵�˳���

                    //aidEntity.FlowId = mpcEntity.ID;
                }
                else
                {
                    aidEntity.REMARK = "7";
                }
                aidEntity.FlowId = flowid;
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
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult ApporveForm(string keyValue, SafetyCollectEntity entity, AptitudeinvestigateauditEntity aentity)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            string state = string.Empty;

            string moduleName = "������ȫ����";


            /// <param name="currUser">��ǰ��¼��</param>
            /// <param name="state">�Ƿ���Ȩ����� 1������� 0 ���������</param>
            /// <param name="moduleName">ģ������</param>
            /// <param name="outengineerid">����Id</param>
            //ManyPowerCheckEntity mpcEntity = peoplereviewbll.CheckAuditPower(curUser, out state, moduleName, outengineerid);
            ManyPowerCheckEntity mpcEntity = dailyexaminebll.CheckAuditPower(curUser, out state, moduleName, curUser.DeptId);

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
            aidEntity.FlowId = aentity.FlowId;
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

            #region  //���濢����ȫ���ռ�¼
            var smEntity = SafetyCollectbll.GetEntity(keyValue);
            //���ͨ��
            if (aentity.AUDITRESULT == "0")
            {

                //0��ʾ����δ��ɣ�1��ʾ���̽���
                if (null != mpcEntity)
                {
                    smEntity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                    smEntity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                    smEntity.FLOWROLE = mpcEntity.CHECKROLEID;
                    smEntity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                    smEntity.ISSAVED = "1";
                    smEntity.ISOVER = "0";
                    smEntity.FlowId = mpcEntity.ID;//��ֵ����Id
                    smEntity.FLOWNAME = mpcEntity.CHECKDEPTNAME + "������";

                }
                else
                {
                    smEntity.FLOWDEPT = "";
                    smEntity.FLOWDEPTNAME = "";
                    smEntity.FLOWROLE = "";
                    smEntity.FLOWROLENAME = "";
                    smEntity.ISSAVED = "1";
                    smEntity.ISOVER = "1";
                    smEntity.FLOWNAME = "";
                }
            }
            else //��˲�ͨ�� �鵵
            {
                smEntity.FLOWDEPT = "";
                smEntity.FLOWDEPTNAME = "";
                smEntity.FLOWROLE = "";
                smEntity.FLOWROLENAME = "";
                smEntity.ISSAVED = "2"; //�����˲�ͨ��
                smEntity.ISOVER = "1"; //���̽���
                smEntity.FLOWNAME = "";
                //smEntity.FlowId = mpcEntity.ID;//���˺�����Id���
                //var applyUser = new UserBLL().GetEntity(smEntity.CREATEUSERID);
                //if (applyUser != null)
                //{
                //    JPushApi.PushMessage(applyUser.Account, smEntity.CREATEUSERNAME, "WB002", entity.ID);
                //}

            }
            //���¿�����ȫ���ջ���״̬��Ϣ
            SafetyCollectbll.SaveForm(keyValue, smEntity);
            #endregion

            #region    //��˲�ͨ��
            if (aentity.AUDITRESULT == "1")
            {
                //��ȡ��ǰҵ������������˼�¼
                var shlist = aptitudeinvestigateauditbll.GetAuditList(keyValue);
                //����������˼�¼����ID
                foreach (AptitudeinvestigateauditEntity mode in shlist)
                {
                    //mode.APTITUDEID = hsentity.ID; //��Ӧ�µ�ID
                    //mode.REMARK = "99";
                    aptitudeinvestigateauditbll.SaveForm(mode.ID, mode);
                }
            }
            #endregion

            return Success("�����ɹ�!");
        }
        #endregion
        #endregion

        #region ���ݵ���
        /// <summary>
        /// �����û��б�
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "����֪ͨ��������")]
        public ActionResult ExportData(string queryJson)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "Id";
            pagination.p_fields = "(case isremind when 1 then '��' else '��' end) as IsRemind,Title,IssueDeptName,IssuerName,IssueTime";
            pagination.p_tablename = "HRS_SafetyCollect";
            pagination.conditionJson = "1=1";

            var watch = CommonHelper.TimerStart();
            var data = SafetyCollectbll.GetPageList(pagination, queryJson);

            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "֪ͨ����";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "֪ͨ����.xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            ColumnEntity columnentity = new ColumnEntity();
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "r", ExcelColumn = "���", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "isremind", ExcelColumn = "��Ҫ", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "title", ExcelColumn = "����", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "issuedeptname", ExcelColumn = "��������", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "issuername", ExcelColumn = "������", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "issuetime", ExcelColumn = "����ʱ��", Alignment = "center" });

            //���õ�������
            ExcelHelper.ExcelDownload(data, excelconfig);

            return Success("�����ɹ���");
        }
        public ActionResult ExportSafetyCollect(string keyValue)
        {
            try
            {
                var userInfo = OperatorProvider.Provider.Current();  //��ȡ��ǰ�û�
                var tempconfig = new TempConfigBLL().GetList("").Where(x => x.DeptCode == userInfo.OrganizeCode && x.ModuleCode == "RYZZSC").ToList();
                string tempPath = @"~/Resource/ExcelTemplate/������̿�����ȫ���ձ�.doc";
                var tempEntity = tempconfig.FirstOrDefault();
                string fileName = "������̿�����ȫ���ձ�_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";

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
            DocumentBuilder builder = new DocumentBuilder(doc);
            DataSet ds = new DataSet();
            DataTable dtPro = new DataTable("project");
            dtPro.Columns.Add("EngineerName");//�����������
            dtPro.Columns.Add("OUTPROJECTNAME");//�а���λ����
            dtPro.Columns.Add("OrgName");//�糧����
            dtPro.Columns.Add("Reason");//��������
            dtPro.Columns.Add("AUDITSIGNIMG1");//�а��̵�λ������

            dtPro.Columns.Add("AUDITOPINION1");  //���Ÿ��������
            dtPro.Columns.Add("AUDITSIGNIMG2"); //���Ÿ�����ǩ��
            dtPro.Columns.Add("DATE1"); //�������ʱ��

            dtPro.Columns.Add("AUDITOPINION2");  //���������������
            dtPro.Columns.Add("AUDITSIGNIMG3"); //������������ǩ��
            dtPro.Columns.Add("DATE2"); //���������ʱ��

            dtPro.Columns.Add("AUDITOPINION3");  //���������������
            dtPro.Columns.Add("AUDITSIGNIMG4"); //������������ǩ��
            dtPro.Columns.Add("DATE3"); //���������ʱ��

            HttpResponse resp = System.Web.HttpContext.Current.Response;

            var sc = SafetyCollectbll.GetEntity(keyValue);
            

            DataRow row = dtPro.NewRow();
            if (sc != null)
            {
                OutsouringengineerEntity eng = outsouringengineerbll.GetEntity(sc.EngineerId);
                OutsourcingprojectEntity pro = outProjectbll.GetOutProjectInfo(eng.OUTPROJECTID);

                row["EngineerName"] = eng.ENGINEERNAME;
                row["OUTPROJECTNAME"] = pro.OUTSOURCINGNAME;
                row["OrgName"] = userInfo.OrganizeName;
                row["Reason"] = sc.Reason;
                row["AUDITSIGNIMG1"] = eng.UnitSuper;
                //��˼�¼
                List<AptitudeinvestigateauditEntity> list = aptitudeinvestigateauditbll.GetAuditList(keyValue);
                #region ͨ�ð汾��˼�¼
                var i = 0;
                foreach (AptitudeinvestigateauditEntity entity in list)
                {
                    i++;
                    if (i == 1)
                    {
                        if (string.IsNullOrWhiteSpace(entity.AUDITSIGNIMG))
                        {
                            row["AUDITSIGNIMG2"] = Server.MapPath("~/content/Images/no_1.png");
                        }
                        else
                        {
                            var filepath = Server.MapPath("~/") + entity.AUDITSIGNIMG.ToString().Replace("../../", "").ToString();
                            if (System.IO.File.Exists(filepath))
                            {
                                row["AUDITSIGNIMG2"] = filepath;
                            }
                            else
                            {
                                row["AUDITSIGNIMG2"] = Server.MapPath("~/content/Images/no_1.png");
                            }
                        }
                        builder.MoveToMergeField("AUDITSIGNIMG2");
                        builder.InsertImage(row["AUDITSIGNIMG2"].ToString(), 80, 35);
                        row["AUDITOPINION1"] = !string.IsNullOrEmpty(entity.AUDITOPINION) ? entity.AUDITOPINION.ToString() : "";
                        row["DATE1"] = entity.AUDITTIME.Value.ToString("yyyy��MM��dd��");
                    }
                    if (i == 2)
                    {
                        if (string.IsNullOrWhiteSpace(entity.AUDITSIGNIMG))
                        {
                            row["AUDITSIGNIMG3"] = Server.MapPath("~/content/Images/no_1.png");
                        }
                        else
                        {
                            var filepath = Server.MapPath("~/") + entity.AUDITSIGNIMG.ToString().Replace("../../", "").ToString();
                            if (System.IO.File.Exists(filepath))
                            {
                                row["AUDITSIGNIMG3"] = filepath;
                            }
                            else
                            {
                                row["AUDITSIGNIMG3"] = Server.MapPath("~/content/Images/no_1.png");
                            }
                        }
                        builder.MoveToMergeField("AUDITSIGNIMG3");
                        builder.InsertImage(row["AUDITSIGNIMG3"].ToString(), 80, 35);
                        row["AUDITOPINION2"] = !string.IsNullOrEmpty(entity.AUDITOPINION) ? entity.AUDITOPINION.ToString() : "";
                        row["DATE2"] = entity.AUDITTIME.Value.ToString("yyyy��MM��dd��");
                    }
                    if (i == 3)
                    {
                        if (string.IsNullOrWhiteSpace(entity.AUDITSIGNIMG))
                        {
                            row["AUDITSIGNIMG4"] = Server.MapPath("~/content/Images/no_1.png");
                        }
                        else
                        {
                            var filepath = Server.MapPath("~/") + entity.AUDITSIGNIMG.ToString().Replace("../../", "").ToString();
                            if (System.IO.File.Exists(filepath))
                            {
                                row["AUDITSIGNIMG4"] = filepath;
                            }
                            else
                            {
                                row["AUDITSIGNIMG4"] = Server.MapPath("~/content/Images/no_1.png");
                            }
                        }
                        builder.MoveToMergeField("AUDITSIGNIMG4");
                        builder.InsertImage(row["AUDITSIGNIMG4"].ToString(), 80, 35);
                        row["AUDITOPINION3"] = !string.IsNullOrEmpty(entity.AUDITOPINION) ? entity.AUDITOPINION.ToString() : "";
                        row["DATE3"] = entity.AUDITTIME.Value.ToString("yyyy��MM��dd��");
                    }
                }
                #endregion
            }

            dtPro.Rows.Add(row);
            doc.MailMerge.Execute(dtPro);
            doc.MailMerge.DeleteFields();
            doc.Save(resp, Server.UrlEncode(fileName), Aspose.Words.ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
        }
        #endregion
    }
}

