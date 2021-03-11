using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using ERCHTMS.Entity.PowerPlantInside;
using ERCHTMS.Busines.PowerPlantInside;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Aspose.Words.Lists;
using BSFramework.Util.Offices;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Code;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using ServiceStack.Text;
using Svg;
using Svg.Transforms;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Entity.PublicInfoManage;

namespace ERCHTMS.Web.Areas.PowerPlantInside.Controllers
{
    /// <summary>
    /// �� ������λ�ڲ��챨
    /// </summary>
    public class PowerplantinsideController : MvcControllerBase
    {
        private PowerplantinsideBLL powerplantinsidebll = new PowerplantinsideBLL();
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

        /// <summary>
        /// ͳ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PowerplantStatistics()
        {
            return View();
        }

        /// <summary>
        /// ���ҳ��
        /// </summary>
        /// <returns></returns>
        public ActionResult ApproveForm()
        {
            return View();
        }

        /// <summary>
        /// ѡ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Select()
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
            var data = powerplantinsidebll.GetList(queryJson);
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
            pagination.p_kid = "ID";
            pagination.p_fields = @"a.CreateUserId,a.CreateDate,a.CreateUserName,a.ModifyUserId,a.ModifyDate,a.ModifyUserName,a.CreateUserDeptCode,a.CreateUserOrgCode,a.accidenteventname,a.accidenteventno,f.itemname as belongsystem,a.district,
           b.itemname as accidenteventtype,a.accidenteventtype as accidenteventtypevalue,c.itemname as accidenteventproperty,a.accidenteventproperty as accidenteventpropertyvalue,a.accidenteventcausename as accidenteventcause,
            to_char(a.happentime,'yyyy-MM-dd HH24:mi') happentime,a.belongdept,a.belongdeptid,a.belongdeptcode,e.itemname as specialty,a.issaved,a.isover,a.flowdeptname,a.flowdept,a.flowrolename,a.flowrole,a.flowname,a.flowid ";
            pagination.p_tablename = @"BIS_POWERPLANTINSIDE a
            left join ( select * from base_dataitemdetail  where itemid = ( select itemid from base_dataitem where  parentid = 
            (select itemid from base_dataitem where itemname = '��λ�ڲ��챨' ) and  itemcode = 'AccidentEventType') ) b on a.accidenteventtype = b.itemvalue
              left join ( select * from base_dataitemdetail  where itemid = ( select itemid from base_dataitem where  parentid = 
            (select itemid from base_dataitem where itemname = '��λ�ڲ��챨' ) and  itemcode = 'AccidentEventProperty') ) c on a.accidenteventproperty = c.itemvalue
            left join ( select * from base_dataitemdetail  where itemid = ( select itemid from base_dataitem where   itemcode = 'SpecialtyType') ) e on a.Specialty = e.itemvalue
            left join ( select * from base_dataitemdetail  where itemid = ( select itemid from base_dataitem where   itemcode = 'BelongSystem') ) f on a.belongsystem = f.itemvalue";
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
            var data = powerplantinsidebll.GetPageList(pagination, queryJson);
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
            var data = powerplantinsidebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }


        /// <summary>
        /// ��ȱ仯ͳ���б�
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public string GetStatisticsList(int year, string mode)
        {
            return powerplantinsidebll.GetStatisticsList(year,mode);
        }
        /// <summary>
        ///�¶ȱ仯ͳ��ͼ
        /// </summary>
        /// <param name="year">���</param>
        /// <returns></returns>
        [HttpGet]
        public string GetStatisticsHighchart(string year, string mode)
        {
            return powerplantinsidebll.GetStatisticsHighchart(year, mode);
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
            powerplantinsidebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, PowerplantinsideEntity entity)
        {
            entity.IsOver = 0;
            entity.IsSaved = 0;
            powerplantinsidebll.SaveForm(keyValue, entity);
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
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, PowerplantinsideEntity entity)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            string state = string.Empty;

            string moduleName = "�¹��¼��챨-�ڲ����";

            /// <param name="currUser">��ǰ��¼��</param>
            /// <param name="state">�Ƿ���Ȩ����� 1������� 0 ���������</param>
            /// <param name="moduleName">ģ������</param>
            /// <param name="outengineerid">����Id</param>
            ManyPowerCheckEntity mpcEntity = powerplantinsidebll.CheckAuditPower(curUser, out state, moduleName, curUser.DeptId);

            string flowid = string.Empty;
            List<ManyPowerCheckEntity> powerList = new ManyPowerCheckBLL().GetListBySerialNum(curUser.OrganizeCode, moduleName);
            List<ManyPowerCheckEntity> checkPower = new List<ManyPowerCheckEntity>();
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
            powerplantinsidebll.SaveForm(keyValue, entity);

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

            string moduleName = "�¹��¼��챨-�ڲ����";

            PowerplantinsideEntity entity = powerplantinsidebll.GetEntity(keyValue);
            /// <param name="currUser">��ǰ��¼��</param>
            /// <param name="state">�Ƿ���Ȩ����� 1������� 0 ���������</param>
            /// <param name="moduleName">ģ������</param>
            /// <param name="createdeptid">�����˲���ID</param>
            ManyPowerCheckEntity mpcEntity = powerplantinsidebll.CheckAuditPower(curUser, out state, moduleName, curUser.DeptId);


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
            //�����¹��¼�����״̬��Ϣ
            powerplantinsidebll.SaveForm(keyValue, entity);
            #endregion

            #region    //��˲�ͨ��
            if (aentity.AUDITRESULT == "1")
            {
                ////�����ʷ��¼
                //HistorydailyexamineEntity hsentity = new HistorydailyexamineEntity();
                //hsentity.CreateUserId = entity.CreateUserId;
                //hsentity.CreateUserDeptCode = entity.CreateUserDeptCode;
                //hsentity.CreateUserOrgCode = entity.CreateUserOrgCode;
                //hsentity.CreateDate = entity.CreateDate;
                //hsentity.CreateUserName = entity.CreateUserName;
                //hsentity.CreateUserDeptId = entity.CreateUserDeptId;
                //hsentity.ModifyDate = entity.ModifyDate;
                //hsentity.ModifyUserId = entity.ModifyUserId;
                //hsentity.ModifyUserName = entity.ModifyUserName;
                //hsentity.ExamineCode = entity.ExamineCode;
                //hsentity.ExamineDept = entity.ExamineDept;
                //hsentity.ExamineDeptId = entity.ExamineDeptId;
                //hsentity.ExamineToDeptId = entity.ExamineToDeptId;
                //hsentity.ExamineToDept = entity.ExamineToDept;
                //hsentity.ExamineType = entity.ExamineType; //����ID
                //hsentity.ExamineMoney = entity.ExamineMoney;
                //hsentity.ExaminePerson = entity.ExaminePerson;
                //hsentity.ExaminePersonId = entity.ExaminePersonId; //����ID
                //hsentity.ExamineTime = entity.ExamineTime;
                //hsentity.ExamineContent = entity.ExamineContent;
                //hsentity.ExamineBasis = entity.ExamineBasis;
                //hsentity.Remark = entity.Remark;
                //hsentity.ContractId = entity.Id;//����ID
                //hsentity.IsSaved = 2;
                //hsentity.IsOver = entity.IsOver;
                //hsentity.FlowDeptName = entity.FlowDeptName;
                //hsentity.FlowDept = entity.FlowDept;
                //hsentity.FlowRoleName = entity.FlowRoleName;
                //hsentity.FlowRole = entity.FlowRole;
                //hsentity.FlowName = entity.FlowName;
                //hsentity.Id = "";

                //historydailyexaminebll.SaveForm(hsentity.Id, hsentity);

                //��ȡ��ǰҵ������������˼�¼
                var shlist = aptitudeinvestigateauditbll.GetAuditList(keyValue);
                //����������˼�¼����ID
                foreach (AptitudeinvestigateauditEntity mode in shlist)
                {
                    mode.Disable = "1";
                    aptitudeinvestigateauditbll.SaveForm(mode.ID, mode);
                }
                ////�������¸�����¼����ID
                //var flist = fileinfobll.GetImageListByObject(keyValue);
                //foreach (FileInfoEntity fmode in flist)
                //{
                //    fmode.RecId = hsentity.Id; //��Ӧ�µ�ID
                //    fileinfobll.SaveForm("", fmode);
                //}
            }
            #endregion

            return Success("�����ɹ�!");
        }
        #endregion

        #region ���ݵ���
        /// <summary>
        /// �¹��¼��챨
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "��λ�ڲ��¹��¼��챨")]
        public ActionResult ExportBulletinList(string condition, string queryJson)
        {
            Pagination pagination = new Pagination();
            queryJson = queryJson ?? "";
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "ID";
            pagination.p_fields = @"a.accidenteventname,a.accidenteventno,
           b.itemname as accidenteventtype,c.itemname as accidenteventproperty,f.itemname as belongsystem,a.accidenteventcausename as accidenteventcause,a.happentime,a.district,a.belongdept,e.itemname as specialty ";
            pagination.p_tablename = @"BIS_POWERPLANTINSIDE a 
            left join ( select * from base_dataitemdetail  where itemid = ( select itemid from base_dataitem where  parentid = 
            (select itemid from base_dataitem where itemname = '��λ�ڲ��챨' ) and  itemcode = 'AccidentEventType') ) b on a.accidenteventtype = b.itemvalue
              left join ( select * from base_dataitemdetail  where itemid = ( select itemid from base_dataitem where  parentid = 
            (select itemid from base_dataitem where itemname = '��λ�ڲ��챨' ) and  itemcode = 'AccidentEventProperty') ) c on a.accidenteventproperty = c.itemvalue
            left join ( select * from base_dataitemdetail  where itemid = ( select itemid from base_dataitem where   itemcode = 'SpecialtyType' ) ) e on a.Specialty = e.itemvalue
            left join ( select * from base_dataitemdetail  where itemid = ( select itemid from base_dataitem where  parentid = 
            (select itemid from base_dataitem where itemname = '��λ�ڲ��챨' ) and  itemcode = 'BelongSystem') ) f on a.belongsystem = f.itemvalue";
            pagination.sord = "CreateDate";
            #region Ȩ��У��
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
            #endregion
            var data = powerplantinsidebll.GetPageList(pagination, queryJson);

            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "��λ�ڲ��¹��¼��챨";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "��λ�ڲ��¹��¼��챨.xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "accidenteventname".ToLower(), ExcelColumn = "�¹�/�¼�����" });
            listColumnEntity.Add(new ColumnEntity() { Column = "accidenteventno".ToLower(), ExcelColumn = "���" });
            listColumnEntity.Add(new ColumnEntity() { Column = "accidenteventtype".ToLower(), ExcelColumn = "�¹ʻ��¼�����" });
            listColumnEntity.Add(new ColumnEntity() { Column = "accidenteventproperty".ToLower(), ExcelColumn = "�¹ʻ��¼�����" });
            listColumnEntity.Add(new ColumnEntity() { Column = "belongsystem".ToLower(), ExcelColumn = "����ϵͳ" });
            listColumnEntity.Add(new ColumnEntity() { Column = "accidenteventcause".ToLower(), ExcelColumn = "Ӱ���¹��¼�����" });
            listColumnEntity.Add(new ColumnEntity() { Column = "happentime".ToLower(), ExcelColumn = "����ʱ��", Alignment = "Center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "district".ToLower(), ExcelColumn = "�ص�(����)" });
            listColumnEntity.Add(new ColumnEntity() { Column = "belongdept".ToLower(), ExcelColumn = "��������/��λ" });
            listColumnEntity.Add(new ColumnEntity() { Column = "specialty".ToLower(), ExcelColumn = "���רҵ" });
            excelconfig.ColumnEntity = listColumnEntity;

            //���õ�������
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("�����ɹ���");
        }

        /// <summary>
        /// �¹��¼��챨
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "�¹��¼�ͳ��")]
        public ActionResult ExportStatisticsExcel(int year, string mode)
        {
            string jsonList = powerplantinsidebll.GetStatisticsList(year, mode); ;

            dynamic dyObj = JsonConvert.DeserializeObject(jsonList);
            ;
            DataTable tb = JsonToDataTable(dyObj.rows.ToString());

            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();

            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������

            switch (mode)
            {
                case "0":
                    excelconfig.Title = year + "-" + DateTime.Now.Year + "���¹��¼�����ͳ��";
                    excelconfig.FileName = year + "-" + DateTime.Now.Year + "���¹��¼�����ͳ��" + ".xls";
                    excelconfig.ColumnEntity = new List<ColumnEntity>()
                    {
                        new ColumnEntity() {Column = "type", ExcelColumn = "����", Alignment = "center"},
                        new ColumnEntity() {Column = "num1", ExcelColumn = "����", Alignment = "center"},
                        new ColumnEntity() {Column = "num2", ExcelColumn = "�豸", Alignment = "center"},
                        new ColumnEntity() {Column = "num3", ExcelColumn = "����", Alignment = "center"},
                        new ColumnEntity() {Column = "num4", ExcelColumn = "��ͨ", Alignment = "center"},
                        new ColumnEntity() {Column = "num5", ExcelColumn = "����", Alignment = "center"},
                        new ColumnEntity() {Column = "num6", ExcelColumn = "ְҵ����", Alignment = "center"},                     
                        new ColumnEntity() {Column = "Total", ExcelColumn = "�ܼ�", Alignment = "center"}
                    };
                    break;
                case "1":
                    excelconfig.Title = year + "-" + DateTime.Now.Year + "���¹��¼�����ͳ��";
                    excelconfig.FileName = year + "-" + DateTime.Now.Year + "���¹��¼�����ͳ��" + ".xls";
                    excelconfig.ColumnEntity = new List<ColumnEntity>()
                    {
                        new ColumnEntity() {Column = "type", ExcelColumn = "����", Alignment = "center"},
                        new ColumnEntity() {Column = "num1", ExcelColumn = "�����ϰ�", Alignment = "center"},
                        new ColumnEntity() {Column = "num2", ExcelColumn = "�쳣", Alignment = "center"},
                        new ColumnEntity() {Column = "num3", ExcelColumn = "δ��", Alignment = "center"},
                        new ColumnEntity() {Column = "num4", ExcelColumn = "С΢�¼�", Alignment = "center"},
                        new ColumnEntity() {Column = "num5", ExcelColumn = "һ���ϰ�", Alignment = "center"},
                        new ColumnEntity() {Column = "num6", ExcelColumn = "һ��", Alignment = "center"},                      
                        new ColumnEntity() {Column = "Total", ExcelColumn = "�ܼ�", Alignment = "center"}
                    };
                    break;
                case "2":
                    excelconfig.Title = year + "-" + DateTime.Now.Year + "��Ӱ���¹��¼�����ͳ��";
                    excelconfig.FileName = year + "-" + DateTime.Now.Year + "��Ӱ���¹��¼�����ͳ��" + ".xls";
                    excelconfig.ColumnEntity = new List<ColumnEntity>()
                    {
                        new ColumnEntity() {Column = "TypeName", ExcelColumn = "Ӱ������", Alignment = "center"},
                        new ColumnEntity() {Column = "num1", ExcelColumn = "1��", Alignment = "center"},
                        new ColumnEntity() {Column = "num2", ExcelColumn = "2��", Alignment = "center"},
                        new ColumnEntity() {Column = "num3", ExcelColumn = "3��", Alignment = "center"},
                        new ColumnEntity() {Column = "num4", ExcelColumn = "4��", Alignment = "center"},
                        new ColumnEntity() {Column = "num5", ExcelColumn = "5��", Alignment = "center"},
                        new ColumnEntity() {Column = "num6", ExcelColumn = "6��", Alignment = "center"},
                        new ColumnEntity() {Column = "num7", ExcelColumn = "7��", Alignment = "center"},
                        new ColumnEntity() {Column = "num8", ExcelColumn = "8��", Alignment = "center"},
                        new ColumnEntity() {Column = "num9", ExcelColumn = "9��", Alignment = "center"},
                        new ColumnEntity() {Column = "num10", ExcelColumn = "10��", Alignment = "center"},
                        new ColumnEntity() {Column = "num11", ExcelColumn = "11��", Alignment = "center"},
                        new ColumnEntity() {Column = "num12", ExcelColumn = "12��", Alignment = "center"},
                        new ColumnEntity() {Column = "Total", ExcelColumn = "�ܼ�", Alignment = "center"}
                    };
                    break;
                case "3":
                    excelconfig.Title = year + "-" + DateTime.Now.Year + "���¹��¼������Ĳ���ͳ��";
                    excelconfig.FileName = year + "-" + DateTime.Now.Year + "���¹��¼������Ĳ���ͳ��" + ".xls";
                    excelconfig.ColumnEntity = new List<ColumnEntity>()
                    {
                        new ColumnEntity() {Column = "dept", ExcelColumn = "��������", Alignment = "center"},
                        new ColumnEntity() {Column = "num1", ExcelColumn = "Ӫ����", Alignment = "center"},
                        new ColumnEntity() {Column = "num2", ExcelColumn = "����֧�ֲ�", Alignment = "center"},
                        new ColumnEntity() {Column = "num3", ExcelColumn = "���粿", Alignment = "center"},
                        new ColumnEntity() {Column = "num4", ExcelColumn = "�칫��", Alignment = "center"},
                        new ColumnEntity() {Column = "num5", ExcelColumn = "EHS��", Alignment = "center"},
                        new ColumnEntity() {Column = "num6", ExcelColumn = "��ط�", Alignment = "center"},
                        new ColumnEntity() {Column = "Total", ExcelColumn = "ȫ��", Alignment = "center"}
                    };
                    break;
                case "4":
                    excelconfig.Title = year + "-" + DateTime.Now.Year + "���¹��¼�����רҵͳ��";
                    excelconfig.FileName = year + "-" + DateTime.Now.Year + "���¹��¼�����רҵͳ��" + ".xls";
                    excelconfig.ColumnEntity = new List<ColumnEntity>()
                    {
                       new ColumnEntity() {Column = "type", ExcelColumn = "����רҵ", Alignment = "center"},
                        new ColumnEntity() {Column = "num1", ExcelColumn = "����רҵ", Alignment = "center"},
                        new ColumnEntity() {Column = "num2", ExcelColumn = "��¯רҵ", Alignment = "center"},
                        new ColumnEntity() {Column = "num3", ExcelColumn = "����רҵ", Alignment = "center"},
                        new ColumnEntity() {Column = "num4", ExcelColumn = "����רҵ", Alignment = "center"},
                        new ColumnEntity() {Column = "num5", ExcelColumn = "��ѧרҵ", Alignment = "center"},
                        new ColumnEntity() {Column = "num6", ExcelColumn = "ȼ��רҵ", Alignment = "center"},
                        new ColumnEntity() {Column = "num7", ExcelColumn = "�ȿ�רҵ", Alignment = "center"},
                        new ColumnEntity() {Column = "Total", ExcelColumn = "�ܼ�", Alignment = "center"}
                    };
                    break;
                case "5":
                    excelconfig.Title = year + "-" + DateTime.Now.Year + "���¹��¼���������ͳ��";
                    excelconfig.FileName = year + "-" + DateTime.Now.Year + "���¹��¼���������ͳ��" + ".xls";
                    excelconfig.ColumnEntity = new List<ColumnEntity>()
                    {
                        new ColumnEntity() {Column = "type", ExcelColumn = "����ϵͳ", Alignment = "center"},
                        new ColumnEntity() {Column = "num1", ExcelColumn = "#1����", Alignment = "center"},
                        new ColumnEntity() {Column = "num2", ExcelColumn = "#2����", Alignment = "center"},
                        new ColumnEntity() {Column = "num3", ExcelColumn = "#3����", Alignment = "center"},
                        new ColumnEntity() {Column = "num4", ExcelColumn = "#4����", Alignment = "center"},
                        new ColumnEntity() {Column = "num5", ExcelColumn = "����ϵͳ", Alignment = "center"},
                        new ColumnEntity() {Column = "Total", ExcelColumn = "�ܼ�", Alignment = "center"}
                    };
                    break;
                case  "6":
                    excelconfig.Title = year + "-" + DateTime.Now.Year + "���¹��¼��¶ȱ仯ͳ��";
                    excelconfig.FileName = year + "-" + DateTime.Now.Year + "���¹��¼��¶ȱ仯ͳ��" + ".xls";
                    excelconfig.ColumnEntity = new List<ColumnEntity>()
                    {
                        new ColumnEntity() {Column = "cs", ExcelColumn = "�·�", Alignment = "center"},
                        new ColumnEntity() {Column = "num1", ExcelColumn = "1��", Alignment = "center"},
                        new ColumnEntity() {Column = "num2", ExcelColumn = "2��", Alignment = "center"},
                        new ColumnEntity() {Column = "num3", ExcelColumn = "3��", Alignment = "center"},
                        new ColumnEntity() {Column = "num4", ExcelColumn = "4��", Alignment = "center"},
                        new ColumnEntity() {Column = "num5", ExcelColumn = "5��", Alignment = "center"},
                        new ColumnEntity() {Column = "num6", ExcelColumn = "6��", Alignment = "center"},
                        new ColumnEntity() {Column = "num7", ExcelColumn = "7��", Alignment = "center"},
                        new ColumnEntity() {Column = "num8", ExcelColumn = "8��", Alignment = "center"},
                        new ColumnEntity() {Column = "num9", ExcelColumn = "9��", Alignment = "center"},
                        new ColumnEntity() {Column = "num10", ExcelColumn = "10��", Alignment = "center"},
                        new ColumnEntity() {Column = "num11", ExcelColumn = "11��", Alignment = "center"},
                        new ColumnEntity() {Column = "num12", ExcelColumn = "12��", Alignment = "center"},
                        new ColumnEntity() {Column = "Total", ExcelColumn = "�ܼ�", Alignment = "center"}
                    };
                    break;
                case "7":
                    int newyear = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
                    excelconfig.Title = year + "-" + newyear + "���¹��¼���ȱ仯ͳ��";
                    excelconfig.FileName = year + "-" + DateTime.Now.Year + "���¹��¼���ȱ仯ͳ��" + ".xls";
                    excelconfig.ColumnEntity = new List<ColumnEntity>();
                    excelconfig.ColumnEntity.AddIfNotExists(new ColumnEntity() { Column = "cs", ExcelColumn = "���", Alignment = "center" });
                    for (int i = year + 1; i <= newyear; i++)
                    {
                        excelconfig.ColumnEntity.AddIfNotExists(new ColumnEntity() { Column = "num" + i, ExcelColumn = i + "��", Alignment = "center" });
                    }

                    break;
            }
           

            //���õ�������
            ExcelHelper.ExportByAspose(tb, excelconfig.FileName, excelconfig.ColumnEntity);
            return Success("�����ɹ���");
        }

        #region ����ͼƬ
        //HighCharts ����ͼƬ svg
        //filename type width scale svg
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Export(FormCollection fc)
        {
            string tType = fc["type"];
            string tSvg = fc["svg"];
            string tFileName = fc["filename"];
            string tWidth = fc["width"];
            if (string.IsNullOrEmpty(tFileName))
            {
                tFileName = "DefaultChart";
            }
            MemoryStream tData = new MemoryStream(Encoding.UTF8.GetBytes(tSvg));
            Svg.SvgDocument tSvgObj = SvgDocument.Open<SvgDocument>(tData);
            tSvgObj.Transforms = new SvgTransformCollection();
            float scalar = (float)int.Parse(tWidth) / (float)tSvgObj.Width;
            tSvgObj.Transforms.Add(new SvgScale(scalar, scalar));
            tSvgObj.Width = new SvgUnit(tSvgObj.Width.Type, tSvgObj.Width * scalar);
            tSvgObj.Height = new SvgUnit(tSvgObj.Height.Type, tSvgObj.Height * scalar);
            MemoryStream tStream = new MemoryStream();
            string tTmp = new Random().Next().ToString();
            string tExt = "";

            switch (tType)
            {
                case "image/png":
                    tExt = "png";
                    break;
                case "image/jpeg":
                    tExt = "jpg";
                    break;
                case "application/pdf":
                    tExt = "pdf";
                    break;
                case "image/svg+xml":
                    tExt = "svg";
                    break;
            }

            // Svg.SvgDocument tSvgObj = SvgDocument.Open<SvgDocument>(tData);
            switch (tExt)
            {
                case "jpg":
                    tSvgObj.Draw().Save(tStream, ImageFormat.Jpeg);
                    break;
                case "png":
                    tSvgObj.Draw().Save(tStream, ImageFormat.Png);
                    break;
                case "pdf":
                    PdfWriter tWriter = null;
                    Document tDocumentPdf = null;
                    try
                    {
                        tSvgObj.Draw().Save(tStream, ImageFormat.Png);
                        tDocumentPdf = new Document(new iTextSharp.text.Rectangle((float)tSvgObj.Width, (float)tSvgObj.Height));
                        tDocumentPdf.SetMargins(0.0f, 0.0f, 0.0f, 0.0f);
                        iTextSharp.text.Image tGraph = iTextSharp.text.Image.GetInstance(tStream.ToArray());
                        tGraph.ScaleToFit((float)tSvgObj.Width, (float)tSvgObj.Height);

                        tStream = new MemoryStream();
                        tWriter = PdfWriter.GetInstance(tDocumentPdf, tStream);
                        tDocumentPdf.Open();
                        tDocumentPdf.NewPage();
                        tDocumentPdf.Add(tGraph);
                        tDocumentPdf.Close();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        tDocumentPdf.Close();
                        tWriter.Close();
                        tData.Dispose();
                        tData.Close();

                    }
                    break;
                case "svg":
                    tStream = tData;
                    break;
            }
            tFileName = tFileName + "." + tExt;
            return File(tStream.ToArray(), tType, tFileName);
        }

        #endregion
        #endregion

        #region Json �ַ��� ת��Ϊ DataTable���ݼ���
        /// <summary>
        /// Json �ַ��� ת��Ϊ DataTable���ݼ��� ��ʽ[{"xxx":"yyy","x1":"yy2"},{"x2":"y2","x3":"y4"}]
        /// </summary>  
        /// <param name="json"></param>
        /// <returns></returns>
        public DataTable JsonToDataTable(string json)
        {
            DataTable dataTable = new DataTable();  //ʵ����
            DataTable result;
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //ȡ�������ֵ
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dictionary.Keys.Count<string>() == 0)
                        {
                            result = dataTable;
                            return result;
                        }
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                dataTable.Columns.Add(current, dictionary[current].GetType());
                            }
                        }
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            dataRow[current] = dictionary[current];
                        }

                        dataTable.Rows.Add(dataRow); //ѭ������е�DataTable��
                    }
                }
            }
            catch
            {
            }
            result = dataTable;
            return result;
        }
        #endregion
    }
}
