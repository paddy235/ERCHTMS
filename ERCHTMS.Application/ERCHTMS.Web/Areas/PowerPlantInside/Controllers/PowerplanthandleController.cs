using ERCHTMS.Entity.PowerPlantInside;
using ERCHTMS.Busines.PowerPlantInside;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Entity.BaseManage;
using System.Collections.Generic;
using ERCHTMS.Busines.BaseManage;
using System.Linq;
using System;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using Aspose.Words;
using System.Data;
using BSFramework.Util.Extension;
using System.Web;
using System.Text;
using ERCHTMS.Busines.SystemManage;
using Aspose.Cells;
using ERCHTMS.Entity.HiddenTroubleManage;

namespace ERCHTMS.Web.Areas.PowerPlantInside.Controllers
{
    /// <summary>
    /// �� �����¹��¼�����
    /// </summary>
    public class PowerplanthandleController : MvcControllerBase
    {
        private PowerplanthandleBLL powerplanthandlebll = new PowerplanthandleBLL();
        private AptitudeinvestigateauditBLL aptitudeinvestigateauditbll = new AptitudeinvestigateauditBLL();
        private PowerplanthandledetailBLL PowerplanthandledetailBLL = new PowerplanthandledetailBLL();
        private PowerplantreformBLL powerplantreformbll = new PowerplantreformBLL();
        private PowerplantcheckBLL powerplantcheckbll = new PowerplantcheckBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private ManyPowerCheckBLL manypowercheckbll = new ManyPowerCheckBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private UserBLL userbll = new UserBLL();


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

        /// <summary>
        /// �¹��¼�������Ϣ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HandleForm()
        {
            return View();
        }

        /// <summary>
        /// ���ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AppForm()
        {
            return View();
        }

        /// <summary>
        /// ����������Ϣ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AppHandleForm()
        {
            return View();
        }

        /// <summary>
        /// ��ʷ��˼�¼
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HistoryIndex()
        {
            return View();
        }

        /// <summary>
        /// ����ͼ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult WorkFlow()
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
            pagination.p_kid = "ID";
            pagination.p_fields = @"a.CreateUserId,a.CreateDate,a.CreateUserName,a.ModifyUserId,a.ModifyDate,a.ModifyUserName,a.CreateUserDeptCode,a.CreateUserOrgCode,a.accidenteventname,
           b.itemname as accidenteventtype,c.itemname as accidenteventproperty,to_char(a.happentime,'yyyy-MM-dd HH24:mi') happentime,a.belongdept,a.issaved,a.applystate,a.flowdeptname,a.flowdept,a.flowrolename,a.flowrole,a.flowname,a.flowid ";
            pagination.p_tablename = @"BIS_POWERPLANTHANDLE a
            left join ( select * from base_dataitemdetail  where itemid = ( select itemid from base_dataitem where  parentid = 
            (select itemid from base_dataitem where itemname = '��λ�ڲ��챨' ) and  itemcode = 'AccidentEventType') ) b on a.accidenteventtype = b.itemvalue
              left join ( select * from base_dataitemdetail  where itemid = ( select itemid from base_dataitem where  parentid = 
            (select itemid from base_dataitem where itemname = '��λ�ڲ��챨' ) and  itemcode = 'AccidentEventProperty') ) c on a.accidenteventproperty = c.itemvalue";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                //���ݵ�ǰ�û���ģ���Ȩ�޻�ȡ��¼
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "createuserdeptcode", "createuserorgcode");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }
            }


            var watch = CommonHelper.TimerStart();
            var data = powerplanthandlebll.GetPageList(pagination, queryJson);
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
            var data = powerplanthandlebll.GetList(queryJson);
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
            var data = powerplanthandlebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }


        /// <summary>
        /// ����ҵ��id��ȡ��Ӧ����˼�¼�б� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetSpecialAuditList(string keyValue)
        {
            var data = aptitudeinvestigateauditbll.GetAuditList(keyValue).Where(t => t.Disable == "0").OrderByDescending(x => x.AUDITTIME).ToList();
            return ToJsonResult(data);
        }


        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="keyValue">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetHistoryAuditList(string keyValue)
        {
            try
            {
                var data = aptitudeinvestigateauditbll.GetAuditList(keyValue).Where(t => t.Disable == "1").OrderByDescending(x => x.AUDITTIME).ToList();
                return ToJsonResult(data);
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
        [HandlerMonitor(6, "�¹��¼�����ɾ��")]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            powerplanthandlebll.RemoveForm(keyValue);
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
        [HandlerMonitor(5, "�¹��¼�������")]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, PowerplanthandleEntity entity)
        {
            entity.IsSaved = 0;
            entity.ApplyState = 0;
            powerplanthandlebll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
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
        [HandlerMonitor(8, "�¹��¼������ύ")]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, PowerplanthandleEntity entity)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            string state = string.Empty;

            string moduleName = "(�¹��¼������¼)���";

            /// <param name="currUser">��ǰ��¼��</param>
            /// <param name="state">�Ƿ���Ȩ����� 1������� 0 ���������</param>
            /// <param name="moduleName">ģ������</param>
            /// <param name="outengineerid">����Id</param>
            ManyPowerCheckEntity mpcEntity = powerplanthandlebll.CheckAuditPower(curUser, out state, moduleName, curUser.DeptId);

            string flowid = string.Empty;
            List<ManyPowerCheckEntity> powerList = new ManyPowerCheckBLL().GetListBySerialNum(curUser.OrganizeCode, moduleName);
            List<ManyPowerCheckEntity> checkPower = new List<ManyPowerCheckEntity>();
            foreach (var item in powerList)
            {
                if (item.CHECKDEPTID == "-3" || item.CHECKDEPTID == "-1")
                {
                    item.CHECKDEPTID = curUser.DeptId;
                    item.CHECKDEPTCODE = curUser.DeptCode;
                    item.CHECKDEPTNAME = curUser.DeptName;
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
            if (null != mpcEntity)
            {
                entity.FlowDept = mpcEntity.CHECKDEPTID;
                entity.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                entity.FlowRole = mpcEntity.CHECKROLEID;
                entity.FlowRoleName = mpcEntity.CHECKROLENAME;
                entity.IsSaved = 1; //����Ѿ��ӵǼǵ���˽׶�
                entity.ApplyState = 1; 
                entity.FlowId = mpcEntity.ID;
                entity.FlowName = mpcEntity.CHECKDEPTNAME + "�����";
                //�����¹��¼�������Ϣ״̬
                IList<PowerplanthandledetailEntity> HandleDetailList = PowerplanthandledetailBLL.GetHandleDetailList(keyValue);
                foreach (var item in HandleDetailList)
                {
                    item.ApplyState = 1;
                    PowerplanthandledetailBLL.SaveForm(item.Id, item);
                }
            }
            else  //Ϊ�����ʾ�Ѿ��������
            {
                entity.FlowDept = "";
                entity.FlowDeptName = "";
                entity.FlowRole = "";
                entity.FlowRoleName = "";
                entity.IsSaved = 1; //����Ѿ��ӵǼǵ���˽׶�
                entity.ApplyState = 3; 
                entity.FlowName = "";
                entity.FlowId = "";
                //�����¹��¼�������Ϣ״̬
                IList<PowerplanthandledetailEntity> HandleDetailList = PowerplanthandledetailBLL.GetHandleDetailList(keyValue);
                foreach (var item in HandleDetailList)
                {
                    //��������Ϣ��ָ��������ʱ��,�Ǽ������ɺ󽫵���������Ϣ״̬����Ϊ�����У���֮����Ϊǩ����
                    if (item.IsAssignPerson == "0")
                    {
                        item.ApplyState = 3;
                    }
                    else
                    {
                        item.ApplyState = 6;
                    }
                    PowerplanthandledetailBLL.SaveForm(item.Id, item);
                }
            }
            powerplanthandlebll.SaveForm(keyValue, entity);
            powerplanthandlebll.UpdateApplyStatus(keyValue);
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
                aidEntity.Disable = "0";
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


        /// <summary>
        /// �Ǽǵ������ύ����˻��߽������ύ����һ�����̣�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <param name="aentity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(8, "�¹��¼�����ύ")]
        [AjaxOnly]
        public ActionResult ApporveForm(string keyValue, AptitudeinvestigateauditEntity aentity)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            string state = string.Empty;

            string moduleName = "(�¹��¼������¼)���";

            PowerplanthandleEntity entity = powerplanthandlebll.GetEntity(keyValue);
            /// <param name="currUser">��ǰ��¼��</param>
            /// <param name="state">�Ƿ���Ȩ����� 1������� 0 ���������</param>
            /// <param name="moduleName">ģ������</param>
            /// <param name="createdeptid">�����˲���ID</param>
            ManyPowerCheckEntity mpcEntity = powerplanthandlebll.CheckAuditPower(curUser, out state, moduleName, new DepartmentBLL().GetEntityByCode(entity.CreateUserDeptCode).DepartmentId);


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
            aidEntity.FlowId = entity.FlowId;
            aidEntity.AUDITSIGNIMG = string.IsNullOrWhiteSpace(aentity.AUDITSIGNIMG) ? "" : aentity.AUDITSIGNIMG.ToString().Replace("../..", "");
            aidEntity.Disable = "0";
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

            #region  //�����¹��¼������¼
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
                    entity.ApplyState = 1;
                    entity.FlowId = mpcEntity.ID;
                    entity.FlowName = mpcEntity.CHECKDEPTNAME + "�����";
                }
                else
                {
                    entity.FlowDept = "";
                    entity.FlowDeptName = "";
                    entity.FlowRole = "";
                    entity.FlowRoleName = "";
                    entity.IsSaved = 1;
                    entity.ApplyState = 3;
                    entity.FlowName = "";
                    entity.FlowId = "";
                    //�����¹��¼�������Ϣ״̬
                    IList<PowerplanthandledetailEntity> HandleDetailList = PowerplanthandledetailBLL.GetHandleDetailList(keyValue);
                    foreach (var item in HandleDetailList)
                    {
                        //��������Ϣ��ָ��������ʱ��,�Ǽ������ɺ󽫵���������Ϣ״̬����Ϊ�����У���֮����Ϊǩ����
                        if (item.IsAssignPerson == "0")
                        {
                            item.ApplyState = 3;
                        }
                        else
                        {
                            item.ApplyState = 6;
                        }
                        PowerplanthandledetailBLL.SaveForm(item.Id, item);
                    }
                }
            }
            else //��˲�ͨ�� 
            {
                entity.FlowDept = "";
                entity.FlowDeptName = "";
                entity.FlowRole = "";
                entity.FlowRoleName = "";
                entity.ApplyState = 0; //���ڵǼǽ׶�
                entity.IsSaved = 0; //�Ƿ����״̬��ֵΪδ���
                entity.FlowName = "";
                entity.FlowId = "";
                //�����¹��¼�������Ϣ״̬
                IList<PowerplanthandledetailEntity> HandleDetailList = PowerplanthandledetailBLL.GetHandleDetailList(keyValue);
                foreach (var item in HandleDetailList)
                {
                    item.ApplyState = 0;
                    PowerplanthandledetailBLL.SaveForm(item.Id, item);
                }

            }
            //�����¹��¼�����״̬��Ϣ
            powerplanthandlebll.SaveForm(keyValue, entity);
            powerplanthandlebll.UpdateApplyStatus(keyValue);
            #endregion

            #region    //��˲�ͨ��
            if (aentity.AUDITRESULT == "1")
            {

                //��ȡ��ǰҵ������������˼�¼
                var shlist = aptitudeinvestigateauditbll.GetAuditList(keyValue);
                //����������˼�¼����ID
                foreach (AptitudeinvestigateauditEntity mode in shlist)
                {
                    mode.Disable = "1";
                    aptitudeinvestigateauditbll.SaveForm(mode.ID, mode);
                }
            }
            #endregion

            return Success("�����ɹ�!");
        }


        /// <summary>
        /// �����¹��¼��������ձ�
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HandlerMonitor(0, "�����¹��¼��������ձ�")]
        public ActionResult ExportPowerPlantHandleInfo(string keyValue)
        {
            try
            {
                HttpResponse resp = System.Web.HttpContext.Current.Response;
                //�������

                string fileName = "�¹��¼��������ձ�_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
                string strDocPath = Request.PhysicalApplicationPath + @"Resource\ExcelTemplate\�¹��¼��������ձ�.docx";
                Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);
                DocumentBuilder builder = new DocumentBuilder(doc);
                DataTable dt = new DataTable();
                dt.Columns.Add("AccidentEventName"); //�¹�/�¼�����
                dt.Columns.Add("AccidentEventType"); //�¹�/�¼�����
                dt.Columns.Add("AccidentEventProperty"); //�¹�/�¼�����
                dt.Columns.Add("HappenTime"); //����ʱ��
                dt.Columns.Add("BelongDept"); //��������
                dt.Columns.Add("SituationIntroduction"); //�������
                dt.Columns.Add("ReasonAndProblem"); //ԭ�򼰴�������
                DataRow row = dt.NewRow();


                //�¹��¼������¼
                PowerplanthandleEntity powerplanthandleentity = powerplanthandlebll.GetEntity(keyValue);
                row["AccidentEventName"] = powerplanthandleentity.AccidentEventName;
                row["AccidentEventType"] = dataitemdetailbll.GetItemName("AccidentEventType", powerplanthandleentity.AccidentEventType);
                row["AccidentEventProperty"] = dataitemdetailbll.GetItemName("AccidentEventProperty", powerplanthandleentity.AccidentEventProperty);
                row["HappenTime"] = powerplanthandleentity.HappenTime.IsEmpty() ? "" : Convert.ToDateTime(powerplanthandleentity.HappenTime).ToString("yyyy-MM-dd");
                row["BelongDept"] = powerplanthandleentity.BelongDept;

                row["SituationIntroduction"] = powerplanthandleentity.SituationIntroduction;
                row["ReasonAndProblem"] = powerplanthandleentity.ReasonAndProblem;
                dt.Rows.Add(row);
                doc.MailMerge.Execute(dt);
                //��˼�¼
                List<AptitudeinvestigateauditEntity> list = aptitudeinvestigateauditbll.GetAuditList(keyValue).Where(t => t.Disable == "0").OrderByDescending(x => x.AUDITTIME).ToList();
                DataTable dtAptitud = new DataTable();
                dtAptitud.TableName = "U";
                dtAptitud.Columns.Add("ApproveIdea");
                dtAptitud.Columns.Add("ApprovePerson");
                dtAptitud.Columns.Add("Person");
                dtAptitud.Columns.Add("ApproveDate");
                foreach (var item in list)
                {
                    DataRow dtrow = dtAptitud.NewRow();
                    dtrow["ApproveIdea"] = item.AUDITOPINION;
                    dtrow["ApprovePerson"] = item.AUDITPEOPLE;
                    dtrow["Person"] = item.AUDITSIGNIMG.IsEmpty() ? Server.MapPath("~/content/Images/no_1.png") : System.IO.File.Exists(Server.MapPath("~/") + item.AUDITSIGNIMG.ToString().Replace("../../", "").ToString()) ? Server.MapPath("~/") + item.AUDITSIGNIMG.ToString().Replace("../../", "").ToString() : Server.MapPath("~/content/Images/no_1.png");
                    dtrow["ApproveDate"] = item.AUDITTIME.IsEmpty() ? "" : Convert.ToDateTime(item.AUDITTIME).ToString("yyyy-MM-dd");
                    dtAptitud.Rows.Add(dtrow);
                }
                doc.MailMerge.ExecuteWithRegions(dtAptitud);

                builder.MoveToBookmark("ReformAndCheck");
                StringBuilder html = new StringBuilder();
                html.Append("<table border='1' cellspacing='0' width='600'>");
                //������Ϣ
                IList<PowerplantreformEntity> reformlist = powerplantreformbll.GetList("").Where(t => t.PowerPlantHandleId == keyValue && t.Disable == 0).ToList();
                foreach (var item in reformlist)
                {
                    html.Append("<tr><td ><b>���Ĵ�ʩ</b></td><td colspan='3'>" + item.RectificationMeasures + "</td></tr>");
                    html.Append("<tr><td><b>����������</b></td><td>" + item.RectificationPerson + "</td><td><b>��������</b></td><td>" + (item.RectificationTime.IsEmpty() ? "" : Convert.ToDateTime(item.RectificationTime).ToString("yyyy-MM-dd")) + "</td></tr>");
                    html.Append("<tr><td><b>�����������</b></td><td colspan='3'>" + item.RectificationSituation + "</td></tr>");
                    html.Append(@"<tr><td><b>����������ǩ��</b></td><td><img src='" + 
                        (item.RectificationPersonSignImg.IsEmpty() ? Server.MapPath("~/content/Images/no_1.png") : System.IO.File.Exists(Server.MapPath("~/") + item.RectificationPersonSignImg.ToString().Replace("../../", "").ToString()) ? 
                        Server.MapPath("~/") + item.RectificationPersonSignImg.ToString().Replace("../../", "").ToString() : Server.MapPath("~/content/Images/no_1.png"))
                        + "'></img></td><td ><b>�������ʱ��</b></td><td>" + (item.RectificationEndTime.IsEmpty() ? "" : Convert.ToDateTime(item.RectificationEndTime).ToString("yyyy-MM-dd")) + "</td></tr>");
                    IList<PowerplantcheckEntity> checklist = powerplantcheckbll.GetList("").Where(t => t.PowerPlantReformId == item.Id && t.Disable == 0).ToList();
                    html.Append("<tr><td><b>�������</b></td><td ><b>������ǩ��</b></td><td><b>��������������</b></td><td><b>����ʱ��</b></td></tr>");
                    foreach (var temp in checklist)
                    {
                        html.Append(@"<tr><td>" + temp.AuditOpinion + "</td><td><img src='" +
                            (temp.AuditSignImg.IsEmpty() ? Server.MapPath("~/content/Images/no_1.png") : System.IO.File.Exists(Server.MapPath("~/") + temp.AuditSignImg.ToString().Replace("../../", "").ToString()) ?
                        Server.MapPath("~/") + temp.AuditSignImg.ToString().Replace("../../", "").ToString() : Server.MapPath("~/content/Images/no_1.png"))
                            + "'></img></td><td>" + temp.AuditDept + "</td><td>" + temp.AuditTime + "</td></tr>");
                    }
                }
                html.Append("</table>");
                builder.InsertHtml(html.ToString());
                doc.MailMerge.DeleteFields();

                doc.Save(resp, Server.UrlEncode(fileName), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
                return Success("�����ɹ�!");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }

            
        }
        #endregion

        #region �����¹��¼��ջ�����������ܱ�
        /// <summary>
        /// �����¹��¼��ջ�����������ܱ�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        [HandlerMonitor(0, "�����¹��¼��ջ�����������ܱ�")]
        public ActionResult ExportPowerPlantList(string queryJson)
        {
            try
            {
                HttpResponse resp = System.Web.HttpContext.Current.Response;
                Pagination pagination = new Pagination();
                pagination.p_kid = "ID";
                pagination.p_fields = @"a.CreateUserId,a.CreateDate,a.CreateUserName,a.ModifyUserId,a.ModifyDate,a.ModifyUserName,a.CreateUserDeptCode,a.CreateUserOrgCode,a.accidenteventname,
                b.itemname as accidenteventtype,c.itemname as accidenteventproperty,to_char(a.happentime,'yyyy-MM-dd HH24:mi') happentime,a.belongdept,a.issaved,a.applystate,a.flowdeptname,a.flowdept,a.flowrolename,a.flowrole,a.flowname,a.flowid ";
                pagination.p_tablename = @"BIS_POWERPLANTHANDLE a
                left join ( select * from base_dataitemdetail  where itemid = ( select itemid from base_dataitem where  parentid = 
                (select itemid from base_dataitem where itemname = '��λ�ڲ��챨' ) and  itemcode = 'AccidentEventType') ) b on a.accidenteventtype = b.itemvalue
                  left join ( select * from base_dataitemdetail  where itemid = ( select itemid from base_dataitem where  parentid = 
                (select itemid from base_dataitem where itemname = '��λ�ڲ��챨' ) and  itemcode = 'AccidentEventProperty') ) c on a.accidenteventproperty = c.itemvalue";
                pagination.conditionJson = "1=1 ";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (!user.IsSystem)
                {
                    //���ݵ�ǰ�û���ģ���Ȩ�޻�ȡ��¼
                    string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "createuserdeptcode", "createuserorgcode");
                    if (!string.IsNullOrEmpty(where))
                    {
                        pagination.conditionJson += " and " + where;
                    }
                }
                pagination.rows = 10000;
                pagination.page = 1;
                string fileUrl = "~/Resource/ExcelTemplate/�¹��¼��ջ�����������ܱ�.xlsx";
                DataTable data = powerplanthandlebll.GetPageList(pagination, queryJson);
                Workbook wb = new Workbook();
                wb.Open(Server.MapPath(fileUrl));
                Worksheet sheet = wb.Worksheets[0];
                Aspose.Cells.Cells cells = sheet.Cells;
                string fielname = "�¹��¼��ջ�����������ܱ�" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                //��ӱ�ͷ
                sheet.Cells[0, 0].PutValue("���");
                cells.SetColumnWidth(0, 10);
                sheet.Cells[0, 1].PutValue("�¹�/�¼�����");
                cells.SetColumnWidth(1, 30);
                sheet.Cells[0, 2].PutValue("����ʱ��");
                cells.SetColumnWidth(2, 20);
                sheet.Cells[0, 3].PutValue("ԭ�򼰱�¶����");
                cells.SetColumnWidth(3, 30);
                sheet.Cells[0, 4].PutValue("����(����)��ʩ");
                cells.SetColumnWidth(4, 30);
                sheet.Cells[0, 5].PutValue("����������");
                cells.SetColumnWidth(5, 20);
                sheet.Cells[0, 6].PutValue("�������β���");
                cells.SetColumnWidth(6, 20);
                sheet.Cells[0, 7].PutValue("��������");
                cells.SetColumnWidth(7, 20);
                sheet.Cells[0, 8].PutValue("�����������");
                cells.SetColumnWidth(8, 30);
                List<ManyPowerCheckEntity> ManyPowerCheckList = manypowercheckbll.GetList(user.OrganizeCode, "�¹��¼������¼-����");
                for (int i = 0; i < ManyPowerCheckList.Count; i++)
                {
                    if (i <= 2)
                    {
                        sheet.Cells[0, 9 + i].PutValue(ManyPowerCheckList[i].FLOWNAME);
                        cells.SetColumnWidth(9 + i, 20);
                    }
                }
                int lastcol = ManyPowerCheckList.Count > 3 ? 12 : 9 + ManyPowerCheckList.Count;
                sheet.Cells[0, lastcol].PutValue("����״̬");
                cells.SetColumnWidth(lastcol, 20);

                int extentrow = 0; //��չ����,��һ���¹��¼���¼��Ӧ���������¼ʱ,��ֵ�����ۼ�
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    //����¹��¼�������Ϣ
                    sheet.Cells[1 + i + extentrow, 0].PutValue(i + 1);
                    sheet.Cells[1 + i + extentrow, 1].PutValue(data.Rows[i]["accidenteventname"].ToString());
                    sheet.Cells[1 + i + extentrow, 2].PutValue(Convert.ToDateTime(data.Rows[i]["happentime"]).ToString("yyyy-MM-dd"));
                    //����¹��¼���������������Ϣ
                    var powerhandledetaillist = PowerplanthandledetailBLL.GetList("").Where(t => t.PowerPlantHandleId == data.Rows[i]["id"].ToString()).ToList();
                    
                    for (int j = 0; j < powerhandledetaillist.Count; j++)
                    {
                        sheet.Cells[1 + i + j + extentrow, 3].PutValue(powerhandledetaillist[j].RectificationMeasures); //ԭ�򼰱�¶����
                        sheet.Cells[1 + i + j + extentrow, 4].PutValue(powerhandledetaillist[j].RectificationMeasures); //����(����)��ʩ
                        sheet.Cells[1 + i + j + extentrow, 5].PutValue(powerhandledetaillist[j].RectificationDutyPerson); //����������
                        sheet.Cells[1 + i + j + extentrow, 6].PutValue(powerhandledetaillist[j].RectificationDutyDept); //�������β���
                        sheet.Cells[1 + i + j + extentrow, 7].PutValue(Convert.ToDateTime(powerhandledetaillist[j].RectificationTime).ToString("yyyy-MM-dd")); //��������
                        var powerplantreform = powerplantreformbll.GetList("").Where(t => t.PowerPlantHandleDetailId == powerhandledetaillist[j].Id && t.Disable == 0).FirstOrDefault(); //������Ϣ
                        if (!powerplantreform.IsEmpty())
                        {
                            sheet.Cells[1 + i + j + extentrow, 8].PutValue(powerplantreform.RectificationSituation); //�����������
                        }
                        var powerplantchecklist = powerplantcheckbll.GetList("").Where(t => t.PowerPlantHandleDetailId == powerhandledetaillist[j].Id && t.Disable == 0).ToList();  //������Ϣ
                        for (int k = 0; k < powerplantchecklist.Count; k++)
                        {
                            if (k < ManyPowerCheckList.Count)
                            {
                                sheet.Cells[1 + i + j + extentrow, 9 + k].PutValue(powerplantchecklist[k].AuditPeople); //������
                            }
                        }
                        string ApplyState = "������";
                        switch (powerhandledetaillist[j].ApplyState)
                        {
                            case 0:
                                ApplyState = "������";
                                break;
                            case 1:
                                ApplyState = "�����";
                                break;
                            case 2:
                                ApplyState = "��˲�ͨ��";
                                break;
                            case 3:
                                ApplyState = "������";
                                break;
                            case 4:
                                ApplyState = "������";
                                break;
                            case 5:
                                ApplyState = "�����";
                                break;
                            case 6:
                                ApplyState = "ǩ����";
                                break;
                            default:
                                break;
                        }
                        sheet.Cells[1 + i + j + extentrow, lastcol].PutValue(ApplyState); //����״̬
                    }
                    cells.Merge(1 + i + extentrow, 0, powerhandledetaillist.Count, 1);//��źϲ�
                    cells.Merge(1 + i + extentrow, 1, powerhandledetaillist.Count, 1);//�¹��¼����ƺϲ�
                    cells.Merge(1 + i + extentrow, 2, powerhandledetaillist.Count, 1);//�����¼��ϲ�
                    extentrow += powerhandledetaillist.Count - 1;
                }

                wb.Save(Server.UrlEncode(fielname), Aspose.Cells.FileFormatType.Excel2003, Aspose.Cells.SaveType.OpenInBrowser, resp);
                return Success("�����ɹ�!");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
        }
        #endregion

        /// <summary>
        /// ��ȡ����ͼ
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetFlowList(string id)
        {
            Flow flow = new Flow();
            List<lines> lines = new List<lines>();
            List<nodes> nodes = new List<nodes>();
            PowerplanthandleEntity root = powerplanthandlebll.GetEntity(id);
            string deptname = departmentBLL.GetEntityByCode(root.CreateUserDeptCode).FullName;
            string deptid= departmentBLL.GetEntityByCode(root.CreateUserDeptCode).DepartmentId;
            nodes startnode = new nodes();
            startnode.id = root.Id;
            startnode.left = 400;
            startnode.top = 30;
            startnode.name = "��������<br />(" + deptname + ")";
            startnode.type = "startround";
            startnode.setInfo = new setInfo
            {
                Taged = 1,
                NodeDesignateData = new List<NodeDesignateData>{
                 new NodeDesignateData{
                   creatdept=deptname,
                   createuser=root.CreateUserName,
                   createdate=root.CreateDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                   status="�Ѵ���"
                 }
                }
            };
            nodes.Add(startnode);
            #region (�¹��¼������¼)��˽ڵ�
            DataTable dtNodes = powerplanthandlebll.GetAuditInfo(id, "(�¹��¼������¼)���");
            if (dtNodes != null && dtNodes.Rows.Count > 0)
            {
                for (int i = 0; i < dtNodes.Rows.Count; i++)
                {
                    DataRow dr = dtNodes.Rows[i];
                    nodes node = new nodes();
                    node.alt = true;
                    node.isclick = false;
                    node.css = "";
                    node.id = dr["id"].ToString(); //����
                    node.img = "";
                    node.name = dr["flowname"].ToString();
                    node.type = "stepnode";
                    node.width = 150;
                    node.height = 60;
                    node.left = 400;
                    node.top = ((i + 1) * 100) + 30;
                    setInfo sinfo = new setInfo();
                    sinfo.NodeName = node.name;
                    //��˼�¼
                    if (dr["auditdept"] != null && !string.IsNullOrEmpty(dr["auditdept"].ToString()))
                    {
                        sinfo.Taged = 1;
                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                        DateTime auditdate;
                        DateTime.TryParse(dr["audittime"].ToString(), out auditdate);
                        nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                        nodedesignatedata.creatdept = dr["auditdept"].ToString();
                        nodedesignatedata.createuser = dr["auditpeople"].ToString();
                        nodedesignatedata.status = dr["auditresult"].ToString() == "0" ? "ͬ��" : "��ͬ��";
                        if (i == 0)
                        {
                            nodedesignatedata.prevnode = "��";
                        }
                        else
                        {
                            nodedesignatedata.prevnode = dtNodes.Rows[i - 1]["flowname"].ToString();
                        }

                        nodelist.Add(nodedesignatedata);
                        sinfo.NodeDesignateData = nodelist;
                        node.setInfo = sinfo;
                    }
                    else
                    {
                        if (root.FlowId == dr["id"].ToString())
                        {
                            sinfo.Taged = 0;
                        }
                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                        nodedesignatedata.createdate = "��";

                        //����,��Ա
                        var checkDeptId = dr["checkdeptid"].ToString();
                        var checkremark = dr["remark"].ToString();
                        string type = checkremark != "1" ? "0" : "1";
                        if (checkDeptId == "-3")
                        {
                            checkDeptId = deptid;
                            nodedesignatedata.creatdept = deptname;
                        }
                        else
                        {
                            nodedesignatedata.creatdept = dr["checkdeptname"].ToString();
                        }
                        string userNames = powerplanthandlebll.GetUserName(checkDeptId, dr["checkrolename"].ToString()).Split('|')[0];
                        nodedesignatedata.createuser = !string.IsNullOrEmpty(userNames) ? userNames : "��";

                        nodedesignatedata.status = "��";
                        if (i == 0)
                        {
                            nodedesignatedata.prevnode = "��";
                        }
                        else
                        {
                            nodedesignatedata.prevnode = dtNodes.Rows[i - 1]["flowname"].ToString();
                        }

                        nodelist.Add(nodedesignatedata);
                        sinfo.NodeDesignateData = nodelist;
                        node.setInfo = sinfo;
                    }
                    nodes.Add(node);

                    lines line = new lines();
                    line.alt = true;
                    line.id = Guid.NewGuid().ToString();
                    line.from = i == 0 ? root.Id : dtNodes.Rows[i - 1]["id"].ToString();
                    line.to = dtNodes.Rows[i]["id"].ToString();
                    line.name = "";
                    line.type = "sl";
                    lines.Add(line);
                }
                
            }
            #endregion

            #region �¹��¼����Ľڵ�
            DataTable dtReformNodes = powerplanthandlebll.GetReformInfo(id);
            if (dtReformNodes != null && dtReformNodes.Rows.Count > 0)
            {
                for (int i = 0; i < dtReformNodes.Rows.Count; i++)
                {
                    DataRow dr = dtReformNodes.Rows[i];
                    #region ǩ�սڵ�
                    Boolean HaveSignNode = dr["isassignperson"].ToString() == "1" ? true : false;
                    if (dr["isassignperson"].ToString() == "1")
                    {
                        nodes signnode = new nodes();
                        signnode.alt = true;
                        signnode.isclick = false;
                        signnode.css = "";
                        signnode.id = dr["id"].ToString() + "-01"; //����
                        signnode.img = "";
                        signnode.name = dr["signdeptname"].ToString() + "ǩ��";
                        signnode.type = "stepnode";
                        signnode.width = 150;
                        signnode.height = 60;
                        signnode.left = i * 200 + 100;
                        signnode.top = ((dtNodes.Rows.Count + 1) * 100) + 30;
                        setInfo signsinfo = new setInfo();
                        signsinfo.NodeName = signnode.name;
                        //��˼�¼
                        if (dr["realsignpersonname"] != null && !string.IsNullOrEmpty(dr["realsignpersonname"].ToString()))
                        {
                            signsinfo.Taged = 1;
                            List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                            NodeDesignateData nodedesignatedata = new NodeDesignateData();
                            DateTime auditdate;
                            DateTime.TryParse(dr["realsigndate"].ToString(), out auditdate);
                            nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                            nodedesignatedata.creatdept = dr["realsignpersondept"].ToString();
                            nodedesignatedata.createuser = dr["realsignpersonname"].ToString();
                            nodedesignatedata.status = "��ǩ��";
                            nodedesignatedata.prevnode = dtNodes.Rows.Count > 0 ? dtNodes.Rows[dtNodes.Rows.Count - 1]["flowname"].ToString() : root.Id;
                            nodelist.Add(nodedesignatedata);
                            signsinfo.NodeDesignateData = nodelist;
                            signnode.setInfo = signsinfo;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(root.FlowId))
                            {
                                signsinfo.Taged = 0;
                            }
                            List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                            NodeDesignateData nodedesignatedata = new NodeDesignateData();
                            nodedesignatedata.createdate = "��";
                            nodedesignatedata.createuser = !dr["signpersonname"].IsEmpty() ? dr["signpersonname"].ToString() : "��";
                            nodedesignatedata.creatdept = !dr["signdeptname"].IsEmpty() ? dr["signdeptname"].ToString() : "��";
                            nodedesignatedata.status = "��";
                            nodedesignatedata.prevnode = dtNodes.Rows.Count > 0 ? dtNodes.Rows[dtNodes.Rows.Count - 1]["flowname"].ToString() : root.Id;
                            nodelist.Add(nodedesignatedata);
                            signsinfo.NodeDesignateData = nodelist;
                            signnode.setInfo = signsinfo;
                        }
                        nodes.Add(signnode);

                        lines signline = new lines();
                        signline.alt = true;
                        signline.id = Guid.NewGuid().ToString();
                        signline.from = dtNodes.Rows.Count > 0 ? dtNodes.Rows[dtNodes.Rows.Count - 1]["id"].ToString() : root.Id;
                        signline.to = signnode.id;
                        signline.name = "";
                        signline.type = "sl";
                        lines.Add(signline);
                    }
                    #endregion
                    #region ���Ľڵ�
                    if (!dr["rectificationdutyperson"].IsEmpty())
                    {
                        nodes node = new nodes();
                        node.alt = true;
                        node.isclick = false;
                        node.css = "";
                        node.id = dr["id"].ToString(); //����
                        node.img = "";
                        node.name = dr["rectificationdutydept"].ToString() + "����";
                        node.type = "stepnode";
                        node.width = 150;
                        node.height = 60;
                        node.left = i * 200 + 100;
                        node.top = HaveSignNode ? ((dtNodes.Rows.Count + 1) * 100) + 130 : ((dtNodes.Rows.Count + 1) * 100) + 30;
                        setInfo sinfo = new setInfo();
                        sinfo.NodeName = node.name;
                        //��˼�¼
                        if (dr["rectificationperson"] != null && !string.IsNullOrEmpty(dr["rectificationperson"].ToString()))
                        {
                            sinfo.Taged = 1;
                            List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                            NodeDesignateData nodedesignatedata = new NodeDesignateData();
                            DateTime auditdate;
                            DateTime.TryParse(dr["createdate"].ToString(), out auditdate);
                            nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                            nodedesignatedata.creatdept = departmentBLL.GetEntityByCode(dr["createuserdeptcode"].ToString()).FullName;
                            nodedesignatedata.createuser = dr["rectificationperson"].ToString();
                            nodedesignatedata.status = "������";
                            nodedesignatedata.prevnode = HaveSignNode ? (dr["signpersonname"].ToString().Length > 20 ? dr["signpersonname"].ToString().Substring(0, 20) + "..." + "ǩ��" : dr["signpersonname"].ToString() + "ǩ��") : (dtNodes.Rows.Count > 0 ? dtNodes.Rows[dtNodes.Rows.Count - 1]["flowname"].ToString() : root.Id);
                            nodelist.Add(nodedesignatedata);
                            sinfo.NodeDesignateData = nodelist;
                            node.setInfo = sinfo;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(root.FlowId))
                            {
                                sinfo.Taged = 0;
                            }
                            List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                            NodeDesignateData nodedesignatedata = new NodeDesignateData();
                            nodedesignatedata.createdate = "��";
                            //nodedesignatedata.createuser = !dr["rectificationdutyperson"].IsEmpty() ? dr["rectificationdutyperson"].ToString() : "��";
                            //nodedesignatedata.creatdept = !dr["rectificationdutydept"].IsEmpty() ? dr["rectificationdutydept"].ToString() : "��";
                            string approveuserid = dr["rectificationdutypersonid"].IsEmpty() ? "" : dr["rectificationdutypersonid"].ToString();
                            string[] accounts = userbll.GetUserTable(approveuserid.Split(',')).AsEnumerable().Select(e => e.Field<string>("ACCOUNT")).ToArray();
                            string accountstr = accounts.Length > 0 ? string.Join(",", accounts) + "," : "";
                            string outtransferuseraccount = dr["outtransferuseraccount"].IsEmpty() ? "" : dr["outtransferuseraccount"].ToString();//ת��������
                            string intransferuseraccount = dr["intransferuseraccount"].IsEmpty() ? "" : dr["intransferuseraccount"].ToString();//ת��������
                            string[] outtransferuseraccountlist = outtransferuseraccount.Split(',');
                            string[] intransferuseraccountlist = intransferuseraccount.Split(',');
                            foreach (var item in intransferuseraccountlist)
                            {
                                if (!item.IsEmpty() && !accountstr.Contains(item + ","))
                                {
                                    accountstr += (item + ",");//��ת�������˼�������˺���
                                }
                            }
                            foreach (var item in outtransferuseraccountlist)
                            {
                                if (!item.IsEmpty() && accountstr.Contains(item + ","))
                                {
                                    accountstr = accountstr.Replace(item + ",", "");//��ת�������˴�����˺����Ƴ�
                                }
                            }

                            DataTable dtuser = userbll.GetUserTable(accountstr.Split(','));
                            string[] usernames = dtuser.AsEnumerable().Select(d => d.Field<string>("realname")).ToArray();
                            string[] deptnames = dtuser.AsEnumerable().Select(d => d.Field<string>("deptname")).ToArray().GroupBy(t => t).Select(p => p.Key).ToArray();
                            nodedesignatedata.createuser = usernames.Length > 0 ? string.Join(",", usernames) : "��";
                            nodedesignatedata.creatdept = deptnames.Length > 0 ? string.Join(",", deptnames) : "��";
                            nodedesignatedata.status = "��";
                            nodedesignatedata.prevnode = HaveSignNode ? (dr["signpersonname"].ToString().Length > 20 ? dr["signpersonname"].ToString().Substring(0, 20) + "..." + "ǩ��" : dr["signpersonname"].ToString() + "ǩ��") : (dtNodes.Rows.Count > 0 ? dtNodes.Rows[dtNodes.Rows.Count - 1]["flowname"].ToString() : root.Id);
                            nodelist.Add(nodedesignatedata);
                            sinfo.NodeDesignateData = nodelist;
                            node.setInfo = sinfo;
                        }
                        nodes.Add(node);

                        lines line = new lines();
                        line.alt = true;
                        line.id = Guid.NewGuid().ToString();
                        line.from = HaveSignNode ? node.id + "-01" : (dtNodes.Rows.Count > 0 ? dtNodes.Rows[dtNodes.Rows.Count - 1]["id"].ToString() : root.Id); //����ǩ�սڵ�ʱ�� from�ڵ�Ϊǩ�սڵ��ID
                        line.to = node.id;
                        line.name = "";
                        line.type = "sl";
                        lines.Add(line);
                        #region  �¹��¼����սڵ�
                        if (dr["rectificationperson"] != null && !string.IsNullOrEmpty(dr["rectificationperson"].ToString()))
                        {
                            DataTable dtCheckNodes = powerplanthandlebll.GetCheckInfo(dr["id"].ToString(), "�¹��¼������¼-����");
                            if (dtCheckNodes != null && dtCheckNodes.Rows.Count > 0)
                            {
                                for (int j = 0; j < dtCheckNodes.Rows.Count; j++)
                                {
                                    DataRow drtemp = dtCheckNodes.Rows[j];
                                    nodes nodetemp = new nodes();
                                    nodetemp.alt = true;
                                    nodetemp.isclick = false;
                                    nodetemp.css = "";
                                    nodetemp.id = Guid.NewGuid().ToString(); //����
                                    nodetemp.img = "";
                                    nodetemp.name = drtemp["flowname"].ToString();
                                    nodetemp.type = "stepnode";
                                    nodetemp.width = 150;
                                    nodetemp.height = 60;
                                    nodetemp.left = node.left;
                                    nodetemp.top = ((j + 1) * 100) + node.top;
                                    setInfo sinfotemp = new setInfo();
                                    sinfotemp.NodeName = nodetemp.name;
                                    //��˼�¼
                                    if (drtemp["auditdept"] != null && !string.IsNullOrEmpty(drtemp["auditdept"].ToString()))
                                    {
                                        sinfotemp.Taged = 1;
                                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                        DateTime auditdate;
                                        DateTime.TryParse(drtemp["audittime"].ToString(), out auditdate);
                                        nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                                        nodedesignatedata.creatdept = drtemp["auditdept"].ToString();
                                        nodedesignatedata.createuser = drtemp["auditpeople"].ToString();
                                        nodedesignatedata.status = drtemp["auditresult"].ToString() == "0" ? "ͬ��" : "��ͬ��";
                                        if (j == 0)
                                        {
                                            nodedesignatedata.prevnode = node.name;
                                        }
                                        else
                                        {
                                            nodedesignatedata.prevnode = dtCheckNodes.Rows[j - 1]["flowname"].ToString();
                                        }

                                        nodelist.Add(nodedesignatedata);
                                        sinfotemp.NodeDesignateData = nodelist;
                                        nodetemp.setInfo = sinfotemp;
                                    }
                                    else
                                    {
                                        if (drtemp["flowid"].ToString() == dr["flowid"].ToString())
                                        {
                                            sinfotemp.Taged = 0;
                                        }
                                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                        nodedesignatedata.createdate = "��";

                                        //����,��Ա
                                        var checkDeptId = drtemp["checkdeptid"].ToString();
                                        var checkremark = drtemp["remark"].ToString();
                                        string type = checkremark != "1" ? "0" : "1";
                                        if (checkDeptId == "-3" || checkDeptId == "-1")
                                        {
                                            checkDeptId = dr["realreformdeptid"].ToString();
                                            nodedesignatedata.creatdept = dr["realreformdept"].ToString();
                                        }
                                        else
                                        {
                                            nodedesignatedata.creatdept = drtemp["checkdeptname"].ToString();
                                        }
                                        string userNames = powerplanthandlebll.GetUserName(checkDeptId, drtemp["checkrolename"].ToString()).Split('|')[0];
                                        nodedesignatedata.createuser = !string.IsNullOrEmpty(userNames) ? userNames : "��";

                                        nodedesignatedata.status = "��";
                                        if (j == 0)
                                        {
                                            nodedesignatedata.prevnode = node.name;
                                        }
                                        else
                                        {
                                            nodedesignatedata.prevnode = dtCheckNodes.Rows[j - 1]["flowname"].ToString();
                                        }

                                        nodelist.Add(nodedesignatedata);
                                        sinfotemp.NodeDesignateData = nodelist;
                                        nodetemp.setInfo = sinfotemp;
                                    }
                                    nodes.Add(nodetemp);

                                    lines linetemp = new lines();
                                    linetemp.alt = true;
                                    linetemp.id = Guid.NewGuid().ToString();
                                    linetemp.from = node.id;
                                    linetemp.to = nodetemp.id;
                                    linetemp.name = "";
                                    linetemp.type = "sl";
                                    lines.Add(linetemp);
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion
                }

            }
            #endregion
            flow.nodes = nodes;
            flow.lines = lines;
            flow.title = "�¹��¼���������ͼ";
            return Success("��ȡ���ݳɹ�", flow);
        }
    }
}
