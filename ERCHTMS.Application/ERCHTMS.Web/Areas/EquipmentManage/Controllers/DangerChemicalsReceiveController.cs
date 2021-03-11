using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.EquipmentManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity;
using ERCHTMS.Entity.SystemManage.ViewModel;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.EquipmentManage;
using ERCHTMS.Entity.OutsourcingProject;
using Newtonsoft.Json;
using BSFramework.Util.Offices;
using Aspose.Cells;
using System.Drawing;
using System.Web;

namespace ERCHTMS.Web.Areas.EquipmentManage.Controllers
{
    /// <summary>
    /// �� ����EHS�ƻ������
    /// </summary>
    public class DangerChemicalsReceiveController : MvcControllerBase
    {
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL(); //����ҵ�����
        private UserBLL userbll = new UserBLL(); //�û���������
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private DangerChemicalsReceiveBLL DangerChemicalsReceiveBll = new DangerChemicalsReceiveBLL();

        private OutsouringengineerBLL outsouringengineerbll = new OutsouringengineerBLL();
        private AptitudeinvestigateauditBLL aptitudeinvestigateauditbll = new AptitudeinvestigateauditBLL();

        private DangerChemicalsBLL DangerChemicalsBll = new DangerChemicalsBLL();
        private DailyexamineBLL dailyexaminebll = new DailyexamineBLL();
        private AptitudeinvestigateinfoBLL aptitudeinvestigateinfobll = new AptitudeinvestigateinfoBLL();


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
        /// ��ȡ�б�(����)
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_fields = @"t.createdate,t.createuserid,t.createuserdeptcode,t.createuserorgcode,t.modifydate,t.modifyuserid,
t.MainId,t.Purpose,t.ReceiveNum,t.ReceiveUnit,t.ReceiveUserId,t.ReceiveUser,t.SignImg,t.GrantState,t.FLOWNAME,t.FLOWROLE,t.FLOWROLENAME,t.FLOWDEPT,t.FLOWDEPTNAME,t.ISSAVED,t.ISOVER,t.FlowId,
t.Name,t.Specification,t.SpecificationUnit,t.Inventory,t.Unit,t.RiskType,t.Amount,t.AmountUnit,t.Site,t1.GrantPersonId,t1.GrantPerson,t.GrantUser";
            pagination.p_kid = "t.id";
            pagination.p_tablename = @"XLD_DANGEROUSCHEMICALRECEIVE t left join XLD_DANGEROUSCHEMICAL t1 on t.mainid=t1.id";
            //pagination.sidx = "createdate";//�����ֶ�
            //pagination.sord = "desc";//����ʽ  
            if (curUser.RoleName.Contains("ʡ���û�") || curUser.RoleName.Contains("�����û�"))
            {
                pagination.conditionJson = string.Format(" t.GrantState=3 ");
            }
            else
            {
                pagination.conditionJson = string.Format(" 1=1 ");
            }


            var watch = CommonHelper.TimerStart();            
            var data = DangerChemicalsReceiveBll.GetList(pagination, queryJson);
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
            var data = DangerChemicalsReceiveBll.GetEntity(keyValue);
            Operator curUser = OperatorProvider.Provider.Current();
            var dataCheck = aptitudeinvestigateinfobll.GetAppCheckFlowList(keyValue, "1", curUser);
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
            DangerChemicalsReceiveBll.RemoveForm(keyValue);//ɾ������
            htworkflowbll.DeleteWorkFlowObj(keyValue);//ɾ������

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
        public ActionResult SaveForm(string keyValue, DangerChemicalsReceiveEntity entity)
        {
            entity.ISSAVED = "0"; //���������
            DangerChemicalsReceiveBll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        /// <summary>
        /// ����ȷ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult GrantForm(string keyValue, DangerChemicalsReceiveEntity entity)
        {
            try
            {
                //����������
                var clEntity = DangerChemicalsBll.GetEntity(entity.MainId);
                if (entity.GrantUnit == clEntity.Unit)
                {
                    if (Convert.ToDecimal(entity.GrantNum) <= Convert.ToDecimal(clEntity.Inventory))
                    {
                        clEntity.Inventory = (Convert.ToDecimal(clEntity.Inventory) - Convert.ToDecimal(entity.GrantNum)).ToString();
                        clEntity.Amount= (Convert.ToDecimal(clEntity.Inventory) / Convert.ToDecimal(clEntity.Specification)).ToString("#0.00");
                        DangerChemicalsBll.SaveForm(entity.MainId, clEntity);
                        entity.PracticalNum = Convert.ToDecimal(entity.GrantNum).ToString(); //ʵ�ʷ��ſ����
                    }
                    else
                    {
                        return Error("����ʧ�ܣ���Σ��Ʒʵ�ʿ���ѱ仯�����Ϊ��" + clEntity.Inventory + " " + clEntity.Unit + "��");
                    }
                }
                if (entity.GrantUnit == clEntity.AmountUnit)
                {
                    if (Convert.ToDecimal(entity.GrantNum) <= Convert.ToDecimal(clEntity.Amount))
                    {
                        clEntity.Inventory = (Convert.ToDecimal(clEntity.Inventory) - (Convert.ToDecimal(entity.GrantNum) * Convert.ToDecimal(clEntity.Specification))).ToString();
                        clEntity.Amount = (Convert.ToDecimal(clEntity.Inventory) / Convert.ToDecimal(clEntity.Specification)).ToString("#0.00");
                        DangerChemicalsBll.SaveForm(entity.MainId, clEntity);
                        entity.PracticalNum = (Convert.ToDecimal(entity.GrantNum) * Convert.ToDecimal(clEntity.Specification)).ToString(); //ʵ�ʷ��ſ����
                    }
                    else
                    {
                        return Error("����ʧ�ܣ���Σ��Ʒʵ�ʿ���ѱ仯�����Ϊ��" + clEntity.Inventory + " " + clEntity.Unit + "��");
                    }
                }
            }
            catch { }
            entity.GrantState = 3; //��Ƿ������
            DangerChemicalsReceiveBll.SaveForm(keyValue, entity);
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
        public ActionResult SubmitForm(string keyValue, DangerChemicalsReceiveEntity entity)
        {
            var clEntity = DangerChemicalsBll.GetEntity(entity.MainId);
            if (clEntity.IsScene == "�ֳ����")
            {
                try
                {
                    if (entity.ReceiveUnit == clEntity.Unit)
                    {
                        if (Convert.ToDecimal(entity.ReceiveNum) <= Convert.ToDecimal(clEntity.Inventory))
                        {
                            clEntity.Inventory = (Convert.ToDecimal(clEntity.Inventory) - Convert.ToDecimal(entity.ReceiveNum)).ToString();
                            clEntity.Amount = (Convert.ToDecimal(clEntity.Inventory) / Convert.ToDecimal(clEntity.Specification)).ToString("#0.00");
                            DangerChemicalsBll.SaveForm(entity.MainId, clEntity);
                            entity.PracticalNum = Convert.ToDecimal(entity.ReceiveNum).ToString(); //ʵ�ʷ��ſ����
                        }
                        else
                        {
                            return Error("����ʧ�ܣ���Σ��Ʒʵ�ʿ���ѱ仯�����Ϊ��" + clEntity.Inventory + " " + clEntity.Unit + "��");
                        }
                    }
                    if (entity.ReceiveUnit == clEntity.AmountUnit)
                    {
                        if (Convert.ToDecimal(entity.ReceiveNum) <= Convert.ToDecimal(clEntity.Amount))
                        {
                            clEntity.Inventory = (Convert.ToDecimal(clEntity.Inventory) - (Convert.ToDecimal(entity.ReceiveNum) * Convert.ToDecimal(clEntity.Specification))).ToString();
                            clEntity.Amount = (Convert.ToDecimal(clEntity.Inventory) / Convert.ToDecimal(clEntity.Specification)).ToString("#0.00");
                            DangerChemicalsBll.SaveForm(entity.MainId, clEntity);
                            entity.PracticalNum = (Convert.ToDecimal(entity.ReceiveNum) * Convert.ToDecimal(clEntity.Specification)).ToString(); //ʵ�ʷ��ſ����
                        }
                        else
                        {
                            return Error("����ʧ�ܣ���Σ��Ʒʵ�ʿ���ѱ仯�����Ϊ��" + clEntity.Inventory + " " + clEntity.Unit + "��");
                        }
                    }
                }
                catch { }

                entity.FLOWDEPT = "";
                entity.FLOWDEPTNAME = "";
                entity.FLOWROLE = "";
                entity.FLOWROLENAME = "";
                entity.ISSAVED = "1"; //����Ѿ��ӵǼǵ���˽׶�
                entity.ISOVER = "1"; //����δ��ɣ�1��ʾ���
                entity.GrantState = 3; //2��ʾ������
                entity.FLOWNAME = "";
                entity.FlowId = "";
                entity.GrantDate = DateTime.Now;

                DangerChemicalsReceiveBll.SaveForm(keyValue, entity);
            }
            else
            {
                Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

                string state = string.Empty;

                string flowid = string.Empty;

                string moduleName = "Σ��Ʒ����";

                // <param name="state">�Ƿ���Ȩ����� 1������� 0 ���������</param>
                ManyPowerCheckEntity mpcEntity = dailyexaminebll.CheckAuditPower(curUser, out state, moduleName, curUser.DeptId);

                //����ʱ����ݽ�ɫ�Զ����
                List<ManyPowerCheckEntity> powerList = new ManyPowerCheckBLL().GetListBySerialNum(curUser.OrganizeCode, "Σ��Ʒ����");
                List<ManyPowerCheckEntity> checkPower = new List<ManyPowerCheckEntity>();
                //�Ȳ��ִ�в��ű���
                for (int i = 0; i < powerList.Count; i++)
                {
                    if (powerList[i].CHECKDEPTCODE == "-1" || powerList[i].CHECKDEPTID == "-1")
                    {
                        var createdeptentity = new DepartmentBLL().GetEntityByCode(curUser.DeptCode);
                        while (createdeptentity.Nature == "רҵ" || createdeptentity.Nature == "����")
                        {
                            createdeptentity = new DepartmentBLL().GetEntity(createdeptentity.ParentId);
                        }
                        powerList[i].CHECKDEPTCODE = createdeptentity.DeptCode;
                        powerList[i].CHECKDEPTID = createdeptentity.DepartmentId;
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
                else
                {
                    if (powerList.Count > 0)
                    {
                        mpcEntity = powerList.First();
                    }
                    
                }
                if (null != mpcEntity)
                {
                    //��������������¼
                    entity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                    entity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                    entity.FLOWROLE = mpcEntity.CHECKROLEID;
                    entity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                    entity.ISSAVED = "1"; //����Ѿ��ӵǼǵ���˽׶�
                    entity.ISOVER = "0"; //����δ��ɣ�1��ʾ���
                    entity.FLOWNAME = mpcEntity.CHECKDEPTNAME + "�����";
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
                    entity.GrantState = 2; //2��ʾ������
                    entity.FLOWNAME = "";
                    entity.FlowId = flowid;
                }

                DangerChemicalsReceiveBll.SaveForm(keyValue, entity);


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
        public ActionResult ApporveForm(string keyValue, DangerChemicalsReceiveEntity entity, AptitudeinvestigateauditEntity aentity)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            string state = string.Empty;

            string moduleName = "Σ��Ʒ����";


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

            #region  //����Σ��Ʒ���ü�¼
            var smEntity = DangerChemicalsReceiveBll.GetEntity(keyValue);
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
                    smEntity.FLOWNAME = mpcEntity.CHECKDEPTNAME + "�����";
                }
                else
                {
                    smEntity.FLOWDEPT = "";
                    smEntity.FLOWDEPTNAME = "";
                    smEntity.FLOWROLE = "";
                    smEntity.FLOWROLENAME = "";
                    smEntity.ISSAVED = "1";
                    smEntity.ISOVER = "1";
                    smEntity.GrantState = 2; //����δ��ɣ�2��ʾ������
                    smEntity.FLOWNAME = "";
                }
            }
            else //��˲�ͨ�� 
            {
                smEntity.FLOWDEPT = "";
                smEntity.FLOWDEPTNAME = "";
                smEntity.FLOWROLE = "";
                smEntity.FLOWROLENAME = "";
                smEntity.ISSAVED = "0"; //���ڵǼǽ׶�
                smEntity.ISOVER = "0"; //�Ƿ����״̬��ֵΪδ���
                smEntity.FLOWNAME = "";
                smEntity.FlowId = "";//���˺�����Id���
                //var applyUser = new UserBLL().GetEntity(smEntity.CREATEUSERID);
                //if (applyUser != null)
                //{
                //    JPushApi.PushMessage(applyUser.Account, smEntity.CREATEUSERNAME, "WB002", entity.ID);
                //}

            }
            //����Σ��Ʒ���û���״̬��Ϣ
            DangerChemicalsReceiveBll.SaveForm(keyValue, smEntity);
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
                    mode.REMARK = "99";
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
        [HandlerMonitor(0, "�������ü�¼")]
        public ActionResult Export(string queryJson)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "rownum idx";
            pagination.p_fields = @"t.Name,(Concat(t.Specification,t.SpecificationUnit)) as Specification,
t.risktype,t.site,
(Concat(t.receivenum,t.receiveUnit)) as receivenum,t.receiveuser,t.grantuser,t.purpose";
            pagination.p_tablename = "XLD_DANGEROUSCHEMICALRECEIVE t  left join XLD_DANGEROUSCHEMICAL t1 on t.mainid=t1.id";
            
            //pagination.sidx = "createdate";//�����ֶ�
            //pagination.sord = "desc";//����ʽ  
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                pagination.conditionJson = string.Format(" CREATEUSERORGCODE ='{0}'", user.OrganizeCode);
            }

            var watch = CommonHelper.TimerStart();
            var data = DangerChemicalsReceiveBll.GetList(pagination, queryJson);

            HttpResponse resp = System.Web.HttpContext.Current.Response;

            // ��ϸ�б�����
            string fielname = "Σ��Ʒ���ü�¼" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
            wb.Open(Server.MapPath("~/Resource/ExcelTemplate/tmp.xls"));
            Aspose.Cells.Worksheet sheet = wb.Worksheets[0] as Aspose.Cells.Worksheet;

            Aspose.Cells.Cell cell = sheet.Cells[0, 0];
            cell.PutValue("Σ��Ʒ���ü�¼"); //����
            cell.Style.Pattern = BackgroundType.Solid;
            cell.Style.Font.Size = 16;
            cell.Style.Font.Color = Color.Black;

            //Aspose.Cells.Style style = wb.Styles[wb.Styles.Add()];
            //style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;//���־���
            //style.IsTextWrapped = true;//�Զ�����
            //style.Borders[Aspose.Cells.BorderType.TopBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
            //style.Borders[Aspose.Cells.BorderType.RightBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
            //style.Borders[Aspose.Cells.BorderType.BottomBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;
            //style.Borders[Aspose.Cells.BorderType.LeftBorder].LineStyle = Aspose.Cells.CellBorderType.Thin;

            Aspose.Cells.Cells cells = sheet.Cells;
            cells.Merge(0, 0, 1, 9);

            sheet.Cells[1, 0].PutValue("���");
            sheet.Cells[1, 1].PutValue("����");
            sheet.Cells[1, 2].PutValue("���");
            sheet.Cells[1, 3].PutValue("Σ��Ʒ����");
            sheet.Cells[1, 4].PutValue("��ŵص�");
            sheet.Cells[1, 5].PutValue("��������");
            sheet.Cells[1, 6].PutValue("������");
            sheet.Cells[1, 7].PutValue("������");
            sheet.Cells[1, 8].PutValue("��;��ʹ��˵��");
            sheet.Cells.SetColumnWidthPixel(8, 250);

            int rowIndex = 2;
            foreach (DataRow row in data.Rows)
            {
                Aspose.Cells.Cell idxcell = sheet.Cells[rowIndex, 0];
                idxcell.PutValue(row["idx"]); 
                Aspose.Cells.Cell namexcell = sheet.Cells[rowIndex, 1];
                namexcell.PutValue(row["name"]); 
                Aspose.Cells.Cell specificationcell = sheet.Cells[rowIndex, 2];
                specificationcell.PutValue(row["specification"]); 
                Aspose.Cells.Cell risktypexcell = sheet.Cells[rowIndex, 3];
                risktypexcell.PutValue(row["risktype"]); 
                Aspose.Cells.Cell sitexcell = sheet.Cells[rowIndex, 4];
                sitexcell.PutValue(row["site"]); 
                Aspose.Cells.Cell receivenumxcell = sheet.Cells[rowIndex, 5];
                receivenumxcell.PutValue(row["receivenum"]); 
                Aspose.Cells.Cell receiveuserxcell = sheet.Cells[rowIndex, 6];
                receiveuserxcell.PutValue(row["receiveuser"]); 
                Aspose.Cells.Cell grantuserxcell = sheet.Cells[rowIndex, 7];
                grantuserxcell.PutValue(row["grantuser"]); 
                Aspose.Cells.Cell purposecell = sheet.Cells[rowIndex,8];
                purposecell.PutValue(row["purpose"]);

                rowIndex++;
            }
            //���õ�����ʽ
            //ExcelConfig excelconfig = new ExcelConfig();
            //excelconfig.Title = "Σ��Ʒ���ü�¼";
            //excelconfig.TitleFont = "΢���ź�";
            //excelconfig.TitlePoint = 16;
            //excelconfig.FileName = "Σ��Ʒ���ü�¼.xls";
            //excelconfig.IsAllSizeColumn = true;
            ////ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            //List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            //excelconfig.ColumnEntity = listColumnEntity;
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "idx", ExcelColumn = "���", Alignment = "center" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "name", ExcelColumn = "����", Alignment = "center" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "specification", ExcelColumn = "���", Alignment = "center" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "risktype", ExcelColumn = "Σ��Ʒ����", Alignment = "center" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "site", ExcelColumn = "��ŵص�", Alignment = "center" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "receivenum", ExcelColumn = "��������", Alignment = "center" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "receiveuser", ExcelColumn = "������", Alignment = "center" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "grantuser", ExcelColumn = "������", Alignment = "center" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "purpose", ExcelColumn = "��;��ʹ��˵��", Alignment = "center" });
            ////���õ�������
            //ExcelHelper.ExcelDownload(data, excelconfig);
            wb.Save(Server.UrlEncode(fielname), Aspose.Cells.FileFormatType.Excel2003, Aspose.Cells.SaveType.OpenInBrowser, resp);

            return Success("�����ɹ���");
        }
        #endregion
    }
}
