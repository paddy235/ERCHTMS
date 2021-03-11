using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Cache;
using System.Web;
using System.IO;
using NPOI.HSSF.UserModel;
using ERCHTMS.Busines.SaftyCheck;
using System.Data;
using ERCHTMS.Entity.SystemManage;
using BSFramework.Util.Offices;
using Aspose.Words;
using ERCHTMS.Busines.LllegalManage;
using ERCHTMS.Entity.LllegalManage;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.HazardsourceManage;
using Newtonsoft.Json;
using System.Threading;
using ERCHTMS.Entity.PersonManage;
using ERCHTMS.Busines.PersonManage;
using System.Collections;
using Newtonsoft.Json.Linq;
using ERCHTMS.Busines.AuthorizeManage;
using System.Drawing;
using Aspose.Cells;
using BSFramework.Util.Attributes;
using BSFramework.Util.Extension;

namespace ERCHTMS.Web.Areas.LllegalManage.Controllers
{
    /// <summary>
    /// �� ����Υ�»�����Ϣ��
    /// </summary>
    public class LllegalRegisterController : MvcControllerBase
    {
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL(); //����ҵ�����
        private UserBLL userbll = new UserBLL(); //�û���������
        private LllegalRegisterBLL lllegalregisterbll = new LllegalRegisterBLL(); // Υ�»�����Ϣ
        private LllegalReformBLL lllegalreformbll = new LllegalReformBLL(); //������Ϣ����
        private LllegalApproveBLL lllegalapprovebll = new LllegalApproveBLL(); //��׼��Ϣ����
        private LllegalAcceptBLL lllegalacceptbll = new LllegalAcceptBLL(); //������Ϣ����
        private LllegalConfirmBLL lllegalconfirmbll = new LllegalConfirmBLL(); //����ȷ����Ϣ����
        private LllegalPunishBLL lllegalpunishbll = new LllegalPunishBLL(); // ������Ϣ����
        private LllegalAwardDetailBLL lllegalawarddetailbll = new LllegalAwardDetailBLL(); //Υ�½�����Ϣ
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private OrganizeCache organizeCache = new OrganizeCache();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();
        private HazardsourceBLL hazardsourcebll = new HazardsourceBLL();
        private ModuleListColumnAuthBLL modulelistcolumnauthbll = new ModuleListColumnAuthBLL();
        private HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL();

        private WfControlBLL wfcontrolbll = new WfControlBLL();//�Զ������̷���

        #region ��ͼ����
        [HttpGet]
        public ActionResult Intergral()
        {
            return View();
        }

        [HttpGet]
        public ActionResult IntergralPerson()
        {
            //��ȡ��Ϣ
            var Ids = Request["Ids"] ?? "";
            var list = new List<LllegalPunishEntity>();
            var list2 = new List<LllegalRegisterEntity>();
            if (Ids.Length > 0)
            {
                var Idsp = Ids.Split(',');

                foreach (var Id in Idsp)
                {
                    var baseInfo = lllegalregisterbll.GetEntity(Id);  //Υ�»�����Ϣ
                    var approveInfo = lllegalpunishbll.GetEntityByBid(Id);
                    list.Add(approveInfo);
                    list2.Add(baseInfo);
                }
            }
            ViewBag.listLA = list;
            ViewBag.listBaseInfo = list2;
            return View();
        }

        [HttpGet]
        public ActionResult IntergralPersonHistory()
        {
            //��ȡ��Ϣ
            return View();
        }

        /// <summary>
        /// ������������ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RemindForm()
        {
            return View();
        }

        [HttpGet]
        public ActionResult XssIntergralPerson()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ExaminIndex()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AwardForm()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AwardIndex()
        {
            return View();
        }

        #region �б�ҳ��
        /// <summary>
        /// �б�ҳ��  ������ҳ��ʹ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }



        #endregion

        #region ��ҳ��
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            string km_major_role = dataitemdetailbll.GetItemValue("KM_MAJOR_ROLE"); //��������  
            //����
            if (!string.IsNullOrEmpty(km_major_role))
            {
                string actionName = string.Empty;
                string[] allKeys = Request.QueryString.AllKeys;
                if (allKeys.Count() > 0)
                {
                    actionName = "KmForm?";
                    int num = 0;
                    foreach (string str in allKeys)
                    {
                        string strValue = Request.QueryString[str];
                        if (num == 0)
                        {
                            actionName += str + "=" + strValue;
                        }
                        else
                        {
                            actionName += "&" + str + "=" + strValue;
                        }
                        num++;
                    }
                }
                else
                {
                    actionName = "KmForm";
                }
                return Redirect(actionName);
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult KmForm()
        {
            return View();
        }
        #endregion

        #region ̨��ҳ��
        /// <summary>
        ///  ������������ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SdIndex()
        {
            return View();
        }
        #endregion

        #region ̨��ҳ��
        /// <summary>
        ///  ����ɽ����ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult XSSIndex()
        {
            return View();
        }
        #endregion

        #region ��������
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExaminForm()
        {
            return View();
        }
        #endregion

        #region ������������ҳ��
        /// <summary>
        ///  ������������ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NewForm()
        {
            return View();
        }
        #endregion

        #region ����Υ��ҳ��
        /// <summary>
        ///  ������������ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DetailList()
        {
            return View();
        }
        #endregion


        #region Ӧ�ò�Υ��ҳ��
        /// <summary>
        ///  ������������ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AppIndex()
        {
            return View();
        }
        #endregion

        #endregion

        #region �������ݲ�ѯ����



        #region ҳ�������ʼ��
        /// <summary>
        /// ��ʼ������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetInitDataJson()
        {
            Operator CreateUser = OperatorProvider.Provider.Current();
            //���˷�ʽ Υ������ Υ�µȼ� ����״̬
            string itemCode = "'ExamineWay','LllegalType','LllegalLevel','FlowState','HidMajorClassify','ChangeDeptRelevancePerson','IsUseConciseRegister','ControlPicMustUpload','LllegalAwardDetailAuth'";
            //����
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode(itemCode);

            string mark = userbll.GetSafetyAndDeviceDept(CreateUser); //���ڱ�ǿ���������Υ��

            string RelevancePersonRole = string.Empty;
            if (itemlist.Where(p => p.EnCode == "ChangeDeptRelevancePerson").Count() > 0)
            {
                RelevancePersonRole = itemlist.Where(p => p.EnCode == "ChangeDeptRelevancePerson").Where(p => p.ItemName == CreateUser.OrganizeCode).FirstOrDefault().ItemValue;
            }
            //���ͼƬ�ش���������
            string ControlPicMustUpload = string.Empty;
            var cpmu = itemlist.Where(p => p.EnCode == "ControlPicMustUpload").Where(p => p.ItemName == CreateUser.OrganizeId);
            if (cpmu.Count() > 0)
            {
                ControlPicMustUpload = cpmu.FirstOrDefault().ItemValue;
            }
            //�Ƿ��ܲ���Υ�½���
            var awardauth = itemlist.Where(p => p.EnCode == "LllegalAwardDetailAuth").Where(p => p.ItemName == CreateUser.OrganizeId);
            string LllegalAwardDetailAuth = awardauth.Count() > 0 ? "1" : "";
            //����ֵ
            var josnData = new
            {
                CreateUser = CreateUser.UserName,
                User = CreateUser,  //�û�����
                Mark = mark, // 1 Ϊ��ȫ������  2 Ϊװ�ò���
                ApplianceClass = dataitemdetailbll.GetItemValue("ApplianceClass"),  //װ������� 
                ExamineWay = itemlist.Where(p => p.EnCode == "ExamineWay"), //���˷�ʽ
                LllegalType = itemlist.Where(p => p.EnCode == "LllegalType"), //Υ������
                LllegalLevel = itemlist.Where(p => p.EnCode == "LllegalLevel"),  //Υ�¼���
                FlowState = itemlist.Where(p => p.EnCode == "FlowState"),  //Υ������״̬
                MajorClassify = itemlist.Where(p => p.EnCode == "HidMajorClassify"),  //רҵ����
                IsHrdl = dataitemdetailbll.GetItemValue("IsOpenPassword"), //�Ƿ������
                RelevancePersonRole = RelevancePersonRole,
                IsDeliver = htworkflowbll.GetCurUserWfAuth("", "Υ������", "Υ������", "����Υ������", "ת��") == "1" ? "1" : "",
                IsAcceptDeliver = htworkflowbll.GetCurUserWfAuth("", "Υ������", "Υ������", "����Υ������", "ת��") == "1" ? "1" : "",
                IsUseConciseRegister = itemlist.Where(p => p.EnCode == "IsUseConciseRegister").Where(p => p.ItemValue == CreateUser.OrganizeId).Count(), //�Ƿ���ü���Ǽ�
                ControlPicMustUpload = ControlPicMustUpload,
                LllegalAwardDetailAuth = LllegalAwardDetailAuth
            };

            return Content(josnData.ToJson());
        }
        #endregion

        #region ��ʼ����ѯ����
        /// <summary>
        /// ��ʼ����ѯ����
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetQueryConditionJson()
        {
            Operator CreateUser = OperatorProvider.Provider.Current();

            //���˷�ʽ��Υ�����ͣ�Υ�¼�������״̬
            string itemCode = "'ExamineWay','LllegalType','LllegalLevel','FlowState','LllegalDataScope','HidStandingType','LllegalStatus'";
            //����
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode(itemCode);
            string mark = userbll.GetSafetyAndDeviceDept(CreateUser); //���ڱ�ǿ���������Υ��
            //����ֵ
            var josnData = new
            {
                Mark = mark, // 1 Ϊ��ȫ������  2 Ϊװ�ò���
                ApplianceClass = dataitemdetailbll.GetItemValue("ApplianceClass"),  //װ������� 
                ExamineWay = itemlist.Where(p => p.EnCode == "ExamineWay"), //���˷�ʽ
                LllegalType = itemlist.Where(p => p.EnCode == "LllegalType"), //Υ������
                LllegalLevel = itemlist.Where(p => p.EnCode == "LllegalLevel"),  //Υ�¼��� 
                FlowState = itemlist.Where(p => p.EnCode == "FlowState"),  //Υ������״̬
                DataScope = itemlist.Where(p => p.EnCode == "LllegalDataScope"),//���ݷ�Χ 
                HidStandingType = itemlist.Where(p => p.EnCode == "HidStandingType"),//̨������  
                LllegalStatus = itemlist.Where(p => p.EnCode == "LllegalStatus") //Υ��״̬
            };

            return Content(josnData.ToJson());
        }
        #endregion


        #region ��ȡ�����µ�Ȩ�޿���
        /// <summary>
        /// ��ȡ�����µ�Ȩ�޿���
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public int GetCurOperAuthByItemCode(string itemcode)
        {
            Operator curUser = OperatorProvider.Provider.Current();

            string code = "'" + itemcode + "'";

            var itemlist = dataitemdetailbll.GetDataItemListByItemCode(code);

            var item = itemlist.Where(p => p.ItemName.Trim() == curUser.DeptId);

            int rcount = 0;

            if (item.Count() > 0)
            {
                string rolename = item.FirstOrDefault().ItemValue.Trim();

                if (!string.IsNullOrEmpty(rolename))
                {
                    foreach (string rolestr in rolename.Split(','))
                    {
                        if (!string.IsNullOrEmpty(rolestr))
                        {
                            if (curUser.RoleName.Contains(rolestr))
                            {
                                rcount += 1;
                            }
                        }
                    }
                }
            }

            return rcount;
        }
        #endregion



        /// <summary>
        /// ��ȡרҵ����
        /// </summary>
        /// <param name="reformdeptcode"></param>
        /// <param name="majorclassify"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetKmSpecialPerson(string reformdeptcode, string majorclassify)
        {
            UserInfoEntity userinfo = new UserInfoEntity();

            //רҵ��Ϊ�գ���ȡרҵ����
            if (!string.IsNullOrEmpty(reformdeptcode) && !string.IsNullOrEmpty(majorclassify))
            {
                List<UserInfoEntity> ulist = userbll.GetUserListByCodeAndRole(reformdeptcode, "").ToList();
                List<UserInfoEntity> lastlist = new List<UserInfoEntity>();
                string km_major_role = dataitemdetailbll.GetItemValue("KM_MAJOR_ROLE"); //�������� 
                var mcItem = dataitemdetailbll.GetDataItemListByItemCode("'HidMajorClassify'").Where(p => p.ItemDetailId == majorclassify).FirstOrDefault();
                var templist = ulist.Where(p => p.RoleName.Contains(km_major_role)).ToList();
                string SpecialtyType = "," + mcItem.ItemValue + ",";
                foreach (UserInfoEntity entity in templist)
                {
                    string sptype = !string.IsNullOrEmpty(entity.SpecialtyType) ? "," + entity.SpecialtyType + "," : "";
                    if (sptype.Contains(SpecialtyType))
                    {
                        lastlist.Add(entity);
                    }
                }
                if (lastlist.Count() > 0)
                {
                    userinfo = lastlist.FirstOrDefault();
                }
            }
            return Content(userinfo.ToJson());
        }

        #region ��ȡ��ǰ�û�������Ȩ��
        /// <summary>
        /// ��ȡ��ǰ�û�������Ȩ��
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCurUserWfAuth(string rankname, string workflow, string endflow, string mark, string submittype, string arg1 = "", string arg2 = "", string arg3 = "", string businessid = "")
        {
            Operator curUser = OperatorProvider.Provider.Current();
            WfControlObj wfentity = new WfControlObj();
            wfentity.businessid = businessid; //
            wfentity.startflow = workflow;
            wfentity.endflow = endflow;
            wfentity.submittype = submittype;
            wfentity.rankname = rankname;
            wfentity.user = curUser;
            wfentity.mark = mark;
            wfentity.isvaliauth = true;
            wfentity.argument1 = arg1;
            wfentity.argument2 = arg2;
            wfentity.argument3 = arg3;

            //��ȡ��һ���̵Ĳ�����
            WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
            if (result.ishave)
            {
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }
        #endregion

        #region ��ȡ��Ӧ��Ȩ��
        /// <summary>
        /// ��ȡ��Ӧ��Ȩ��
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCurUserWfAuthByCondition(string queryJson)
        {
            List<object> list = new List<object>();
            if (!string.IsNullOrEmpty(queryJson))
            {

                Operator curUser = OperatorProvider.Provider.Current();
                JArray jarray = (JArray)JsonConvert.DeserializeObject(queryJson);
                foreach (JObject obj in jarray)
                {
                    string rankname = obj["rankname"].ToString();
                    string workflow = obj["workflow"].ToString();
                    string endflow = obj["endflow"].ToString();
                    string mark = obj["mark"].ToString();
                    string submittype = obj["submittype"].ToString();
                    string arg1 = queryJson.Contains("arg1") ? obj["arg1"].ToString() : string.Empty;
                    string arg2 = queryJson.Contains("arg2") ? obj["arg2"].ToString() : string.Empty;
                    string arg4 = queryJson.Contains("arg4") ? obj["arg4"].ToString() : string.Empty;
                    string arg5 = queryJson.Contains("arg5") ? obj["arg5"].ToString() : string.Empty;
                    string businessid = obj["businessid"].ToString();
                    string action = obj["action"].ToString();

                    WfControlObj wfentity = new WfControlObj();
                    wfentity.businessid = businessid;
                    wfentity.startflow = workflow;
                    wfentity.endflow = endflow;
                    wfentity.submittype = submittype;
                    wfentity.rankname = rankname;
                    wfentity.user = curUser;
                    wfentity.mark = mark;
                    wfentity.isvaliauth = true;
                    wfentity.argument1 = arg1;
                    wfentity.argument2 = arg2;
                    wfentity.argument3 = curUser.DeptId;
                    wfentity.argument4 = arg4;
                    wfentity.argument5 = arg5;
                    //��ȡ��һ���̵Ĳ�����
                    WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
                    list.Add(new { key = action, value = result.ishave });
                }
            }
            return Content(list.ToJson());
        }
        #endregion

        #region ��ȡΥ���б�����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {

            var watch = CommonHelper.TimerStart();
            Operator opertator = new OperatorProvider().Current();
            queryJson = queryJson.Insert(1, "\"userId\":\"" + opertator.UserId + "\","); //��ӵ�ǰ�û�
            var data = lllegalregisterbll.GetLllegalBaseInfo(pagination, queryJson);
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
        #endregion

        #region ��ȡΥ�µ��������
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            //����������Ϣ
            var baseInfo = lllegalregisterbll.GetEntity(keyValue);  //Υ�»�����Ϣ

            var approveInfo = lllegalapprovebll.GetEntityByBid(baseInfo.ID); //��׼��Ϣ

            var reformInfo = lllegalreformbll.GetEntityByBid(baseInfo.ID); // ������Ϣ

            var punishInfo = lllegalpunishbll.GetEntityByBid(baseInfo.ID); //��������(Υ�¸�����)

            var relevanceInfo = lllegalpunishbll.GetListByLllegalId(baseInfo.ID, "");//�������ݣ����������ˣ�

            var acceptInfo = lllegalacceptbll.GetEntityByBid(baseInfo.ID); //������Ϣ

            var confirmInfo = lllegalconfirmbll.GetEntityByBid(baseInfo.ID); //������Ϣ

            var awardInfo = lllegalawarddetailbll.GetListByLllegalId(baseInfo.ID);//Υ�½�����Ϣ

            var userInfo = OperatorProvider.Provider.Current();  //��ȡ��ǰ�û�

            int isReformBack = 0;

            var historyacceptList = lllegalacceptbll.GetHistoryList(keyValue).ToList();
            if (historyacceptList.Count() > 0)
            {
                isReformBack = 1;
            }
            var data = new { baseInfo = baseInfo, approveInfo = approveInfo, reformInfo = reformInfo, punishInfo = punishInfo, relevanceInfo = relevanceInfo, acceptInfo = acceptInfo, confirmInfo = confirmInfo, awardInfo = awardInfo, userInfo = userInfo, isreformback = isReformBack };

            return ToJsonResult(data);
        }

        #endregion

        #endregion

        #region ��������  �����������޸ġ� ɾ��

        #region  ��������������޸ģ�
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, LllegalRegisterEntity entity, LllegalReformEntity rEntity, LllegalAcceptEntity aEntity)
        {
            CommonSaveForm(keyValue, "03", entity, rEntity, aEntity);
            return Success("�����ɹ�!");
        }
        #endregion

        #region ���÷�������������
        /// <summary>
        /// ���÷�������������
        /// </summary>
        /// <param name="keyValue">����ID</param>
        /// <param name="entity">Υ�»�����Ϣ</param>
        /// <param name="rEntity">������Ϣ</param>
        /// <param name="aEntity">������Ϣ</param>
        public void CommonSaveForm(string keyValue, string workFlow, LllegalRegisterEntity entity, LllegalReformEntity rEntity, LllegalAcceptEntity aEntity)
        {
            try
            {
                //�ύͨ��
                string userId = OperatorProvider.Provider.Current().UserId;
                var userInfo = OperatorProvider.Provider.Current();  //��ȡ��ǰ�û�
                //����Υ�»�����Ϣ
                entity.RESEVERFOUR = "";
                entity.RESEVERFIVE = "";
                //ͨ��Υ�±����ж��Ƿ�����ظ�
                if (string.IsNullOrEmpty(keyValue))
                {
                    string lenNum = !string.IsNullOrEmpty(dataitemdetailbll.GetItemValue("LllegalSerialNumberLen")) ? dataitemdetailbll.GetItemValue("LllegalSerialNumberLen") : "3";

                    entity.LLLEGALNUMBER = lllegalregisterbll.GenerateHidCode("bis_lllegalregister", "lllegalnumber", int.Parse(lenNum));

                    entity.CREATEDEPTID = userInfo.DeptId;
                    entity.CREATEDEPTNAME = userInfo.DeptName;
                    entity.BELONGDEPARTID = userInfo.OrganizeId;
                    entity.BELONGDEPART = userInfo.OrganizeName;
                }

                lllegalregisterbll.SaveForm(keyValue, entity);

                //��������ʵ��
                if (string.IsNullOrEmpty(keyValue))
                {
                    bool isSucess = htworkflowbll.CreateWorkFlowObj(workFlow, entity.ID, userId);
                    if (isSucess)
                    {
                        lllegalregisterbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", entity.ID);  //����ҵ������״̬
                    }
                }

                /************������Ϣ**********/
                #region ��������
                string RELEVANCEDATA = Request.Form["RELEVANCEDATA"];
                if (!string.IsNullOrEmpty(RELEVANCEDATA))
                {  //��ɾ�����������˼���
                    lllegalpunishbll.DeleteLllegalPunishList(entity.ID, "");

                    JArray jarray = (JArray)JsonConvert.DeserializeObject(RELEVANCEDATA);

                    foreach (JObject rhInfo in jarray)
                    {
                        //string relevanceId = rhInfo["ID"].ToString(); //����id
                        string assessobject = rhInfo["ASSESSOBJECT"].ToString();
                        string personinchargename = rhInfo["PERSONINCHARGENAME"].ToString(); //��������������
                        string personinchargeid = rhInfo["PERSONINCHARGEID"].ToString();//����������id
                        string performancepoint = rhInfo["PERFORMANCEPOINT"].ToString();//EHS��Ч���� 
                        string economicspunish = rhInfo["ECONOMICSPUNISH"].ToString(); // ���ô���
                        string education = rhInfo["EDUCATION"].ToString(); //������ѵ
                        string lllegalpoint = rhInfo["LLLEGALPOINT"].ToString();//Υ�¿۷�
                        string awaitjob = rhInfo["AWAITJOB"].ToString();//����
                        LllegalPunishEntity newpunishEntity = new LllegalPunishEntity();
                        newpunishEntity.LLLEGALID = entity.ID;
                        newpunishEntity.ASSESSOBJECT = assessobject; //���˶���
                        newpunishEntity.PERSONINCHARGEID = personinchargeid;
                        newpunishEntity.PERSONINCHARGENAME = personinchargename;
                        newpunishEntity.PERFORMANCEPOINT = !string.IsNullOrEmpty(performancepoint) ? Convert.ToDecimal(performancepoint) : 0;
                        newpunishEntity.ECONOMICSPUNISH = !string.IsNullOrEmpty(economicspunish) ? Convert.ToDecimal(economicspunish) : 0;
                        newpunishEntity.EDUCATION = !string.IsNullOrEmpty(education) ? Convert.ToDecimal(education) : 0;
                        newpunishEntity.LLLEGALPOINT = !string.IsNullOrEmpty(lllegalpoint) ? Convert.ToDecimal(lllegalpoint) : 0;
                        newpunishEntity.AWAITJOB = !string.IsNullOrEmpty(awaitjob) ? Convert.ToDecimal(awaitjob) : 0;
                        newpunishEntity.MARK = assessobject.Contains("����") ? "0" : "1"; //���0���ˣ�1����
                        lllegalpunishbll.SaveForm("", newpunishEntity);
                    }
                }
                #endregion

                #region Υ�½�����Ϣ
                string AWARDDATA = Request.Form["AWARDDATA"];
                if (!string.IsNullOrEmpty(AWARDDATA))
                {  //��ɾ����������
                    lllegalawarddetailbll.DeleteLllegalAwardList(entity.ID);

                    JArray jarray = (JArray)JsonConvert.DeserializeObject(AWARDDATA);

                    foreach (JObject rhInfo in jarray)
                    {
                        string userid = rhInfo["USERID"].ToString(); //�����û�
                        string username = rhInfo["USERNAME"].ToString();
                        string deptid = rhInfo["DEPTID"].ToString();//�����û�����
                        string deptname = rhInfo["DEPTNAME"].ToString();
                        string points = rhInfo["POINTS"].ToString();  //��������
                        string money = rhInfo["MONEY"].ToString(); //�������

                        LllegalAwardDetailEntity awardEntity = new LllegalAwardDetailEntity();
                        awardEntity.LLLEGALID = entity.ID;
                        awardEntity.USERID = userid; //��������
                        awardEntity.USERNAME = username;
                        awardEntity.DEPTID = deptid;
                        awardEntity.DEPTNAME = deptname;
                        awardEntity.POINTS = !string.IsNullOrEmpty(points) ? int.Parse(points) : 0;
                        awardEntity.MONEY = !string.IsNullOrEmpty(money) ? Convert.ToDecimal(money) : 0;
                        lllegalawarddetailbll.SaveForm("", awardEntity);
                    }
                }
                #endregion

                /********������Ϣ************/
                string REFORMID = Request.Form["REFORMID"].ToString();
                rEntity.LLLEGALID = entity.ID;

                //����״̬�����
                if (!string.IsNullOrEmpty(REFORMID))
                {
                    var tempEntity = lllegalreformbll.GetEntity(REFORMID);
                    rEntity.AUTOID = tempEntity.AUTOID;
                    rEntity.REFORMSTATUS = string.Empty;
                }
                lllegalreformbll.SaveForm(REFORMID, rEntity);


                /********������Ϣ************/
                string ACCEPTID = Request.Form["ACCEPTID"].ToString();
                aEntity.LLLEGALID = entity.ID;
                if (!string.IsNullOrEmpty(ACCEPTID))
                {
                    var tempEntity = lllegalacceptbll.GetEntity(ACCEPTID);
                    aEntity.AUTOID = tempEntity.AUTOID;
                }
                lllegalacceptbll.SaveForm(ACCEPTID, aEntity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region ɾ��Υ����Ϣ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(6, "ɾ��Υ�»�����Ϣ")]
        public ActionResult RemoveForm(string keyValue)
        {
            LllegalRegisterEntity entity = lllegalregisterbll.GetEntity(keyValue);
            Operator user = OperatorProvider.Provider.Current();
            //ɾ��Υ��������Ϣ
            lllegalregisterbll.RemoveForm(keyValue);


            LogEntity logEntity = new LogEntity();
            logEntity.Browser = this.Request.Browser.Browser;
            logEntity.CategoryId = 6;
            logEntity.OperateTypeId = "6";
            logEntity.OperateType = "ɾ��";
            logEntity.OperateAccount = user.UserName;
            logEntity.OperateUserId = OperatorProvider.Provider.Current().UserId;
            logEntity.ExecuteResult = 1;
            logEntity.Module = SystemInfo.CurrentModuleName;
            logEntity.ModuleId = SystemInfo.CurrentModuleId;
            logEntity.ExecuteResultJson = "������Ϣ:ɾ��Υ�±��Ϊ" + entity.LLLEGALNUMBER + ",Υ������Ϊ" + entity.LLLEGALDESCRIBE + "��Υ����Ϣ, ��������: ��, ������Ϣ:��";
            LogBLL.WriteLog(logEntity);

            return Success("ɾ���ɹ�!");
        }
        #endregion

        #endregion

        #region �ύ����  ���������ύ

        #region �ύ���̣�ͬʱ�������޸�������Ϣ��
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, LllegalRegisterEntity entity,
            LllegalReformEntity rEntity, LllegalAcceptEntity aEntity)
        {
            //�ж��ظ���Ź���
            if (!string.IsNullOrEmpty(entity.LLLEGALNUMBER))
            {
                var curHtBaseInfor = lllegalregisterbll.GetListByNumber(entity.LLLEGALNUMBER).FirstOrDefault();

                if (null != curHtBaseInfor)
                {
                    if (curHtBaseInfor.ID != keyValue && string.IsNullOrEmpty(keyValue))
                    {
                        return Error("Υ�±���ظ�,����������!");
                    }
                }
            }
            try
            {
                //�������̣������Ӧ��Ϣ
                CommonSaveForm(keyValue, "03", entity, rEntity, aEntity);

                if (!string.IsNullOrEmpty(entity.ID))
                {
                    entity = lllegalregisterbll.GetEntity(entity.ID);
                }
                //����������ʵ����
                if (string.IsNullOrEmpty(keyValue))
                {
                    keyValue = entity.ID;
                }
                //�˴���Ҫ�жϵ�ǰ���Ƿ�Ϊ��ȫ����Ա
                string wfFlag = string.Empty;
                //��ǰ�û�
                Operator curUser = OperatorProvider.Provider.Current();

                //������Ա
                string participant = string.Empty;

                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = keyValue; //
                wfentity.argument1 = entity.MAJORCLASSIFY; //רҵ����
                wfentity.argument2 = entity.LLLEGALTYPE;//Υ������
                wfentity.argument3 = curUser.DeptId;//��ǰ����id
                wfentity.argument4 = entity.LLLEGALTEAMCODE;//Υ�²���
                wfentity.argument5 = entity.LLLEGALLEVEL; //Υ�¼���
                wfentity.startflow = entity.FLOWSTATE;
                //�Ƿ��ϱ�
                if (entity.ISUPSAFETY == "1")
                {
                    wfentity.submittype = "�ϱ�";
                }
                else
                {
                    wfentity.submittype = "�ύ";
                    //��ָ������������
                    if (rEntity.ISAPPOINT == "0")
                    {
                        wfentity.submittype = "�ƶ��ύ";
                    }
                }
                wfentity.rankid = null;
                wfentity.user = curUser;
                wfentity.mark = "����Υ������";
                wfentity.organizeid = entity.BELONGDEPARTID; //��Ӧ�糧id
                //��ȡ��һ���̵Ĳ�����
                WfControlResult result = wfcontrolbll.GetWfControl(wfentity);

                //����ɹ�
                if (result.code == WfCode.Sucess)
                {
                    participant = result.actionperson;
                    wfFlag = result.wfflag;

                    //�ύ���̵���һ�ڵ�
                    if (!string.IsNullOrEmpty(participant))
                    {
                        int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);

                        if (count > 0)
                        {
                            htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", keyValue);  //����ҵ������״̬
                        }
                    }
                    return Success(result.message);
                }
                else
                {
                    return Error(result.message);
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion

        #region �ύ�ƶ����ļƻ����̣�ͬʱ�޸�������Ϣ��
        /// <summary>
        /// �ύ�ƶ����ļƻ����̣�ͬʱ�޸�������Ϣ��
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitChangePlanForm(string keyValue, LllegalRegisterEntity entity,
            LllegalReformEntity rEntity, LllegalAcceptEntity aEntity)
        {
            Operator curUser = OperatorProvider.Provider.Current();
            string participant = string.Empty;

            //�ж��ظ���Ź���
            if (!string.IsNullOrEmpty(entity.LLLEGALNUMBER))
            {
                var curHtBaseInfor = lllegalregisterbll.GetListByNumber(entity.LLLEGALNUMBER).FirstOrDefault();

                if (null != curHtBaseInfor)
                {
                    if (curHtBaseInfor.ID != keyValue && string.IsNullOrEmpty(keyValue))
                    {
                        return Error("Υ�±���ظ�,����������!");
                    }
                }
            }
            //�������̣������Ӧ��Ϣ
            CommonSaveForm(keyValue, "03", entity, rEntity, aEntity);

            //����������ʵ����
            if (string.IsNullOrEmpty(keyValue))
            {
                keyValue = entity.ID;
            }

            //�˴���Ҫ�жϵ�ǰ���Ƿ�Ϊ��ȫ����Ա
            string wfFlag = string.Empty;

            WfControlObj wfentity = new WfControlObj();
            wfentity.businessid = keyValue; //
            wfentity.startflow = "�ƶ����ļƻ�";
            wfentity.submittype = "�ύ";
            wfentity.rankid = null;
            wfentity.user = curUser;
            if (entity.ADDTYPE == "2")
            {
                wfentity.mark = "ʡ��Υ������";
            }
            else
            {
                wfentity.mark = "����Υ������";
            }
            wfentity.organizeid = entity.BELONGDEPARTID; //��Ӧ�糧id
            //��ȡ��һ���̵Ĳ�����
            WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
            //����ɹ�
            if (result.code == WfCode.Sucess)
            {
                participant = result.actionperson;
                wfFlag = result.wfflag;
                if (!string.IsNullOrEmpty(participant))
                {
                    int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);

                    if (count > 0)
                    {
                        htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", keyValue);  //����ҵ������״̬
                    }
                }
                else
                {
                    return Error("����ϵϵͳ����Ա����ӵ�ǰ�����µĲ�����!");
                }
                return Success(result.message);
            }
            else
            {
                return Error(result.message);
            }
        }
        #endregion

        #region ת���ƶ����ļƻ�/Υ������,Υ����������
        /// <summary>
        /// ת���ƶ����ļƻ�/Υ������,Υ����������
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult DeliverPlanForm(string keyValue, LllegalReformEntity rentity,LllegalAcceptEntity aentity)
        {
            Operator curUser = OperatorProvider.Provider.Current();
            string participant = string.Empty;
            try
            {
                //Υ�»�����Ϣ
                LllegalRegisterEntity entity = lllegalregisterbll.GetEntity(keyValue);

                /********������Ϣ************/
                if (!Request.Form["REFORMID"].IsEmpty())
                {
                    string REFORMID = Request.Form["REFORMID"].ToString();
                    //����������Ϣ
                    if (!string.IsNullOrEmpty(REFORMID))
                    {
                        var reformEntity = lllegalreformbll.GetEntity(REFORMID);
                        reformEntity.REFORMCHARGEDEPTID = rentity.REFORMCHARGEDEPTID;
                        reformEntity.REFORMCHARGEDEPTNAME = rentity.REFORMCHARGEDEPTNAME;
                        reformEntity.REFORMCHARGEPERSON = rentity.REFORMCHARGEPERSON;
                        reformEntity.REFORMCHARGEPERSONNAME = rentity.REFORMCHARGEPERSONNAME;
                        reformEntity.REFORMPEOPLE = rentity.REFORMPEOPLE;
                        reformEntity.REFORMPEOPLEID = rentity.REFORMPEOPLEID;
                        reformEntity.REFORMDEPTNAME = rentity.REFORMDEPTNAME;
                        reformEntity.REFORMDEPTCODE = rentity.REFORMDEPTCODE;
                        reformEntity.REFORMTEL = rentity.REFORMTEL;
                        lllegalreformbll.SaveForm(REFORMID, reformEntity);
                    }
                }
                /*******������Ϣ*****/
                if (!Request.Form["ACCEPTID"].IsEmpty())
                {
                    string ACCEPTID = Request.Form["ACCEPTID"].ToString();
                    //����������Ϣ
                    if (!string.IsNullOrEmpty(ACCEPTID))
                    {
                        var acceptEntity = lllegalacceptbll.GetEntity(ACCEPTID);
                        acceptEntity.ACCEPTDEPTCODE = aentity.ACCEPTDEPTCODE;
                        acceptEntity.ACCEPTDEPTNAME = aentity.ACCEPTDEPTNAME;
                        acceptEntity.ACCEPTPEOPLE = aentity.ACCEPTPEOPLE;
                        acceptEntity.ACCEPTPEOPLEID = aentity.ACCEPTPEOPLEID;
                        lllegalacceptbll.SaveForm(ACCEPTID, acceptEntity);
                    }
                }

                //�˴���Ҫ�жϵ�ǰ���Ƿ�Ϊ��ȫ����Ա
                string wfFlag = string.Empty;
                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = keyValue; //
                wfentity.startflow = entity.FLOWSTATE;
                wfentity.endflow = entity.FLOWSTATE;
                wfentity.submittype = "ת��";
                wfentity.rankid = null;
                wfentity.user = curUser;
                if (entity.ADDTYPE == "2")
                {
                    wfentity.mark = "ʡ��Υ������";
                }
                else
                {
                    wfentity.mark = "����Υ������";
                }
                wfentity.organizeid = entity.BELONGDEPARTID; //��Ӧ�糧id
                                                             //��ȡ��һ���̵Ĳ�����
                WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
                //����ɹ�
                if (result.code == WfCode.Sucess)
                {
                    participant = result.actionperson;
                    wfFlag = result.wfflag;
                    if (!string.IsNullOrEmpty(participant))
                    {
                        //������״̬
                        if (!result.ischangestatus)
                        {
                            htworkflowbll.SubmitWorkFlowNoChangeStatus(wfentity, result, keyValue, participant, curUser.UserId);
                            return Success(result.message);
                        }
                    }
                    else
                    {
                        return Error("����ϵϵͳ����Ա����ӵ�ǰ�����µĲ�����!");
                    }
                    return Success(result.message);
                }
                else
                {
                    return Error(result.message);
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion

        #region һ�����ύΥ�¼�������Ϣ
        /// <summary>
        /// һ�����ύΥ�¼�������Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="pEntity"></param>
        /// <param name="rEntity"></param>
        /// <param name="aEntity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult OneSubmitForm(string keyValue, LllegalRegisterEntity entity, LllegalApproveEntity pEntity,
            LllegalReformEntity rEntity, LllegalAcceptEntity aEntity)
        {
            string ApproveID = Request.Form["APPROVEID"].ToString();
            string ReformID = Request.Form["REFORMID"].ToString();
            string AcceptID = Request.Form["ACCEPTID"].ToString();
            //�˴���Ҫ�жϵ�ǰ���Ƿ�Ϊ��ȫ����Ա
            string wfFlag = string.Empty;

            Operator curUser = OperatorProvider.Provider.Current();

            //������Ա
            string participant = string.Empty;

            ////����Υ����Ϣ
            CommonSaveForm(keyValue, "04", entity, rEntity, aEntity);

            ////�����׼��Ϣ
            pEntity.LLLEGALID = entity.ID;

            lllegalapprovebll.SaveForm(ApproveID, pEntity);

            wfFlag = "1";//���Ľ���

            participant = curUser.Account;

            //�ύ����
            int count = htworkflowbll.SubmitWorkFlow(entity.ID, participant, wfFlag, curUser.UserId);

            if (count > 0)
            {
                htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", entity.ID);  //����ҵ������״̬

                //string tagName = htworkflowbll.QueryTagNameByCurrentWF(entity.ID);

                //#region Υ�����ֶ���
                //if (tagName == "Υ������" || tagName == "���̽���")
                //{
                //    //��ӷ�����
                //    lllegalregisterbll.AddLllegalScore(entity);
                //}
                //#endregion
            }
            return Success("�����ɹ�!");
        }
        #endregion

        #region һ�����ύ�����������˵ĵǼ�Υ����Ϣ
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult CheckLllegalForm(string checkid)
        {
            Operator curUser = OperatorProvider.Provider.Current();
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'FlowState'");
            string startnode = itemlist.Where(p => p.ItemName == "Υ�µǼ�").Count() > 0 ? "Υ�µǼ�" : "Υ�¾ٱ�";
            var dtHid = lllegalregisterbll.GetListByCheckId(checkid, curUser.UserId, startnode);

            string keyValue = string.Empty;

            string reformpeopleid = string.Empty; //������

            foreach (DataRow row in dtHid.Rows)
            {
                keyValue = row["id"].ToString();

                reformpeopleid = row["reformpeopleid"].ToString();

                string createuserid = row["createuserid"].ToString();

                LllegalRegisterEntity entity = lllegalregisterbll.GetEntity(keyValue);

                LllegalPunishEntity pbEntity = lllegalpunishbll.GetEntityByBid(keyValue);

                //�˴���Ҫ�жϵ�ǰ���Ƿ�Ϊ��ȫ����Ա
                string wfFlag = string.Empty;
                //������Ա
                string participant = string.Empty;

                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = keyValue; //
                wfentity.argument1 = entity.MAJORCLASSIFY; //רҵ����
                wfentity.argument2 = entity.LLLEGALTYPE;//Υ������
                wfentity.argument3 = curUser.DeptId;//��ǰ����id
                wfentity.argument4 = entity.LLLEGALTEAMCODE;//Υ�²���
                wfentity.argument5 = entity.LLLEGALLEVEL; //Υ�¼���
                wfentity.startflow = startnode;
                //�Ƿ��ϱ�
                if (entity.ISUPSAFETY == "1")
                {
                    wfentity.submittype = "�ϱ�";
                }
                else
                {
                    wfentity.submittype = "�ύ";
                    //��ָ������������
                    if (row["isappoint"].ToString() == "0")
                    {
                        wfentity.submittype = "�ƶ��ύ";
                    }
                }
                wfentity.rankid = null;
                wfentity.spuser = userbll.GetUserInfoEntity(createuserid);
                //ʡ���Ǽǵ�
                if (entity.ADDTYPE == "2")
                {
                    wfentity.mark = "ʡ��Υ������";
                }
                else
                {
                    wfentity.mark = "����Υ������";
                }
                wfentity.organizeid = entity.BELONGDEPARTID; //��Ӧ�糧id
                //��ȡ��һ���̵Ĳ�����
                WfControlResult result = wfcontrolbll.GetWfControl(wfentity);

                //����ɹ�
                if (result.code == WfCode.Sucess)
                {
                    participant = result.actionperson;
                    wfFlag = result.wfflag;

                    //�ύ���̵���һ�ڵ�
                    if (!string.IsNullOrEmpty(participant))
                    {
                        int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, createuserid);

                        if (count > 0)
                        {
                            htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", keyValue);  //����ҵ������״̬
                        }
                    }
                }
            }

            return Success("�����ɹ�!");
        }
        #endregion

        #endregion


        #region ����Υ�»�����Ϣ
        /// <summary>
        /// ����Υ�»�����Ϣ
        /// </summary>
        /// <param name="queryJson"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ActionResult ExportExcel(string queryJson, string fileName, string currentModuleId, string mode = "")
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            queryJson = queryJson.Insert(1, "\"userId\":\"" + curUser.UserId + "\","); //��ӵ�ǰ�û�
            string userId = curUser.UserId;
            string p_fields = string.Empty;
            string p_fieldsName = string.Empty;
            try
            {
                //ϵͳĬ�ϵ��б�����
                var defaultdata = modulelistcolumnauthbll.GetEntity(currentModuleId, "", 0);
                if (null != defaultdata)
                {
                    p_fields = defaultdata.DEFAULTCOLUMNFIELDS;
                    p_fieldsName = defaultdata.DEFAULTCOLUMNNAME;
                }
                //��ǰ�û����б�����
                var data = modulelistcolumnauthbll.GetEntity(currentModuleId, curUser.UserId, 1);
                //Ϊ�գ��Զ���ȡϵͳĬ��
                if (null != data)
                {
                    p_fields = data.DEFAULTCOLUMNFIELDS;
                    p_fieldsName = data.DEFAULTCOLUMNNAME;
                }
                p_fields = "flowstate," + p_fields + ",participantname";
                p_fieldsName = "����״̬," + p_fieldsName + ",���̴�����";
                pagination.p_fields = p_fields;
                //ȡ������Դ
                DataTable exportTable = lllegalregisterbll.GetLllegalBaseInfo(pagination, queryJson);
                exportTable.Columns.Remove("id");
                exportTable.Columns["r"].SetOrdinal(0);

                // ��ϸ�б�����
                string fielname = fileName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                wb.Open(Server.MapPath("~/Resource/ExcelTemplate/tmp.xls"));
                Aspose.Cells.Worksheet sheet = wb.Worksheets[0] as Aspose.Cells.Worksheet;

                Aspose.Cells.Cell cell = sheet.Cells[0, 0];
                cell.PutValue("Υ�»�����Ϣ"); //����
                cell.Style.Pattern = BackgroundType.Solid;
                cell.Style.Font.Size = 14;
                cell.Style.Font.Color = Color.Black;

                DataTable dt = new DataTable();
                dt.Columns.Add("r");
                //�������������
                if (!string.IsNullOrEmpty(p_fields))
                {
                    string[] p_fieldsArray = p_fields.Split(',');
                    for (int i = 0; i < p_fieldsArray.Length; i++)
                    {
                        dt.Columns.Add(p_fieldsArray[i].ToString(), typeof(string));//��ͷ
                    }

                    foreach (DataRow row in exportTable.Rows)
                    {
                        DataRow newrow = dt.NewRow();
                        newrow["r"] = row["r"].ToString();
                        for (int i = 0; i < p_fieldsArray.Length; i++)
                        {
                            string curColName = p_fieldsArray[i].ToString();

                            if (curColName != "lllegalfilepath" && curColName != "reformfilepath")
                            {
                                newrow[curColName] = row[curColName].ToString();
                            }
                            else
                            {
                                newrow[curColName] = "";
                            }
                        }
                        dt.Rows.Add(newrow);
                    }
                }

                //�������������
                if (!string.IsNullOrEmpty(p_fieldsName))
                {
                    //��̬����
                    string[] p_filedsNameArray = p_fieldsName.Split(',');
                    //�����
                    Aspose.Cells.Cell serialcell = sheet.Cells[1, 0];
                    serialcell.PutValue("���"); //���λ

                    for (int i = 0; i < p_filedsNameArray.Length; i++)
                    {
                        Aspose.Cells.Cell curcell = sheet.Cells[1, i + 1];
                        curcell.PutValue(p_filedsNameArray[i].ToString()); //��ͷ
                    }
                    //�ϲ���Ԫ��
                    Aspose.Cells.Cells cells = sheet.Cells;
                    cells.Merge(0, 0, 1, p_filedsNameArray.Length + 1);
                }

                //�ȵ�������
                sheet.Cells.ImportDataTable(dt, false, 2, 0);

                //����ͼƬ����
                if (!string.IsNullOrEmpty(p_fieldsName))
                {
                    int picnum = 0;
                    foreach (DataRow row in exportTable.Rows)
                    {
                        //Υ��ͼƬ
                        if (exportTable.Columns.Contains("lllegalfilepath"))
                        {
                            int colIndex = exportTable.Columns.IndexOf("lllegalfilepath");
                            string lllegalfilepath = row["lllegalfilepath"].ToString();
                            if (!string.IsNullOrEmpty(lllegalfilepath))
                            {
                                string imageUrl = System.Web.HttpContext.Current.Server.MapPath(lllegalfilepath);
                                if (System.IO.File.Exists(imageUrl))
                                {
                                    sheet.Pictures.Add(2 + picnum, colIndex, 3 + picnum, colIndex + 1, imageUrl);
                                }
                            }
                            row["lllegalfilepath"] = "";
                        }
                        //����ͼƬ
                        if (exportTable.Columns.Contains("reformfilepath"))
                        {
                            int colIndex = exportTable.Columns.IndexOf("reformfilepath");
                            string reformfilepath = row["reformfilepath"].ToString();
                            if (!string.IsNullOrEmpty(reformfilepath))
                            {
                                string imageUrl = System.Web.HttpContext.Current.Server.MapPath(reformfilepath);
                                if (System.IO.File.Exists(imageUrl))
                                {
                                    sheet.Pictures.Add(2 + picnum, colIndex, 3 + picnum, colIndex + 1, imageUrl);
                                }
                            }
                            row["reformfilepath"] = "";
                        }
                        picnum++;
                    }
                }
                if (!string.IsNullOrEmpty(mode))
                {
                    string tempSavePath = Server.MapPath("~/Resource/Temp/") + fielname;
                    wb.Save(tempSavePath);
                    string url = "../../Utility/DownloadFile?filePath=~/Resource/Temp/" + fielname + "&speed=10240000&newFileName=" + fielname;
                    return Redirect(url);
                }
                else
                {
                    HttpResponse resp = System.Web.HttpContext.Current.Response;
                    resp.Clear();
                    resp.Buffer = true;
                    wb.Save(Server.UrlEncode(fielname), Aspose.Cells.FileFormatType.Excel2003, Aspose.Cells.SaveType.OpenInBrowser, resp);
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
            return Success("�����ɹ�!");
        }
        #endregion

        #region Υ�¿���֪ͨ������ģ��
        /// <summary>
        /// Υ�¿���֪ͨ������ģ��
        /// </summary>
        /// <param name="queryJson"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ActionResult ExportExamReport(string keyValue)
        {
            //Υ�»�����Ϣ
            DataTable lllegaldt = lllegalregisterbll.GetLllegalModel(keyValue);

            List<LllegalPunishEntity> list = lllegalpunishbll.GetListByLllegalId(keyValue, "");

            var userInfo = OperatorProvider.Provider.Current();  //��ȡ��ǰ�û�

            string fileName = "Υ�¿���֪ͨ��_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";

            string strDocPath = Server.MapPath("~/Resource/ExcelTemplate/Υ�¿���֪ͨ������ģ��.doc");

            Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);

            DataTable dt = new DataTable();
            dt.Columns.Add("LllegalTime");
            dt.Columns.Add("LllegalAddress");
            dt.Columns.Add("LllegalPeople");
            dt.Columns.Add("LllegalDescribe");
            dt.Columns.Add("LllegalContent");
            dt.Columns.Add("ReformRequire");
            dt.Columns.Add("ApproveDept");
            dt.Columns.Add("CurDate");
            HttpResponse resp = System.Web.HttpContext.Current.Response;

            DataRow row = dt.NewRow();
            string lllegalpic = lllegaldt.Rows[0]["lllegalpic"].ToString();
            IEnumerable<FileInfoEntity> reformfile = fileinfobll.GetImageListByObject(lllegalpic);
            if (reformfile.Count() > 0)
            {
                DocumentBuilder builder = new DocumentBuilder(doc);
                foreach (FileInfoEntity fentity in reformfile)
                {
                    string url = AppDomain.CurrentDomain.BaseDirectory + fentity.FilePath.Substring(1).Replace("~/", "");
                    if (System.IO.File.Exists(url))
                    {
                        builder.MoveToBookmark("LllegalPic");
                        builder.InsertImage(url, Aspose.Words.Drawing.RelativeHorizontalPosition.Margin, 1, Aspose.Words.Drawing.RelativeVerticalPosition.Margin, 320, 50, 50, Aspose.Words.Drawing.WrapType.Inline);
                    }
                }
            }
            string lllegaltime = lllegaldt.Rows[0]["lllegaltime"].ToString();
            row["LllegalTime"] = !string.IsNullOrEmpty(lllegaltime) ? Convert.ToDateTime(lllegaltime).ToString("yyyy��MM��dd��") : "";
            row["LllegalAddress"] = lllegaldt.Rows[0]["lllegaladdress"].ToString();
            row["LllegalPeople"] = lllegaldt.Rows[0]["lllegalperson"].ToString();
            row["LllegalDescribe"] = lllegaldt.Rows[0]["lllegaldescribe"].ToString();//EconomicsPunish LllegalPoint
            string lllegalcontent = string.Empty;
            if (list.Count() > 0)
            {
                foreach (LllegalPunishEntity entity in list)
                {
                    if (entity.ASSESSOBJECT.Contains("��λ"))
                    {
                        lllegalcontent += entity.PERSONINCHARGENAME + "���ô���" + entity.ECONOMICSPUNISH.ToString() + "Ԫ,EHS��Ч����" + entity.PERFORMANCEPOINT.ToString() +
                            "��,Υ�¿۷�" + entity.LLLEGALPOINT.ToString() + "��;";
                    }
                    else  //��Ա 
                    {
                        lllegalcontent += entity.PERSONINCHARGENAME + "���ô���" + entity.ECONOMICSPUNISH.ToString() + "Ԫ,EHS��Ч����" + entity.PERFORMANCEPOINT.ToString() +
                            "��,Υ�¿۷�" + entity.LLLEGALPOINT.ToString() + "��,������ѵ" + entity.EDUCATION.ToString() + "ѧʱ,����" + entity.AWAITJOB + "��;";
                    }
                }
            }
            else
            {
                lllegalcontent = "��δ���ֿ�����Ϣ.";
            }
            row["LllegalContent"] = lllegalcontent;
            row["ReformRequire"] = lllegaldt.Rows[0]["reformrequire"].ToString();
            row["ApproveDept"] = lllegaldt.Rows[0]["approvedeptname"].ToString();
            row["CurDate"] = DateTime.Now.ToString("yyyy-MM-dd");
            dt.Rows.Add(row);

            doc.MailMerge.Execute(dt);
            doc.MailMerge.DeleteFields();
            doc.Save(resp, Server.UrlEncode(fileName), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));

            return Success("�����ɹ�!");
        }
        #endregion

        #region Υ������֪ͨ������ģ��
        /// <summary>
        /// Υ������֪ͨ������ģ��
        /// </summary>
        /// <param name="queryJson"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ActionResult ExportReformReport(string keyValue)
        {
            //Υ�»�����Ϣ
            DataTable lllegaldt = lllegalregisterbll.GetLllegalModel(keyValue);

            var userInfo = OperatorProvider.Provider.Current();  //��ȡ��ǰ�û�

            string fileName = "Υ������֪ͨ��_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";

            string strDocPath = Server.MapPath("~/Resource/ExcelTemplate/Υ������֪ͨ������ģ��.doc");

            Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);

            DataTable dt = new DataTable();
            dt.Columns.Add("ReformYear");
            dt.Columns.Add("ReformMonth");
            dt.Columns.Add("ReformDate");
            dt.Columns.Add("LllegalAddress");
            dt.Columns.Add("LllegalPeople");
            dt.Columns.Add("LllegalDescribe");
            dt.Columns.Add("ReformRequire");
            dt.Columns.Add("ApproveDept");
            dt.Columns.Add("CurDate");
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            DataRow row = dt.NewRow();
            string lllegaltime = lllegaldt.Rows[0]["lllegaltime"].ToString();
            string year = string.Empty;
            string month = string.Empty;
            string date = string.Empty;
            string lllegalpic = lllegaldt.Rows[0]["lllegalpic"].ToString();
            if (!string.IsNullOrEmpty(lllegaltime))
            {
                year = Convert.ToDateTime(lllegaltime).Year.ToString();
                month = Convert.ToDateTime(lllegaltime).Month.ToString();
                date = Convert.ToDateTime(lllegaltime).Day.ToString();
            }
            IEnumerable<FileInfoEntity> reformfile = fileinfobll.GetImageListByObject(lllegalpic);
            if (reformfile.Count() > 0)
            {
                DocumentBuilder builder = new DocumentBuilder(doc);
                foreach (FileInfoEntity fentity in reformfile)
                {
                    string url = AppDomain.CurrentDomain.BaseDirectory + fentity.FilePath.Substring(1).Replace("~/", "");
                    if (System.IO.File.Exists(url))
                    {
                        builder.MoveToBookmark("LllegalPic");
                        builder.InsertImage(url, Aspose.Words.Drawing.RelativeHorizontalPosition.Margin, 1, Aspose.Words.Drawing.RelativeVerticalPosition.Margin, 180, 60, 60, Aspose.Words.Drawing.WrapType.Inline);
                    }
                }
            }
            row["ReformYear"] = year;
            row["ReformMonth"] = month;
            row["ReformDate"] = date;
            row["LllegalAddress"] = lllegaldt.Rows[0]["lllegaladdress"].ToString();
            row["LllegalPeople"] = lllegaldt.Rows[0]["lllegalperson"].ToString();
            row["LllegalDescribe"] = lllegaldt.Rows[0]["lllegaldescribe"].ToString();
            row["ReformRequire"] = lllegaldt.Rows[0]["reformrequire"].ToString();
            row["ApproveDept"] = lllegaldt.Rows[0]["approvedeptname"].ToString();
            row["CurDate"] = DateTime.Now.ToString("yyyy-MM-dd");
            dt.Rows.Add(row);

            doc.MailMerge.Execute(dt);
            doc.MailMerge.DeleteFields();
            doc.Save(resp, Server.UrlEncode(fileName), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));

            return Success("�����ɹ�!");
        }
        #endregion



        #region ��������Υ�µ���
        /// <summary>
        /// ��������Υ�µ���
        /// </summary>
        /// <param name="queryJson"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ActionResult ExportPersonWzRecord(string userId)
        {
            try
            {
                //Υ�»�����Ϣ
                DataTable lllegaldt = lllegalregisterbll.GetLllegalForPersonRecord(userId);
                var userInfo = userbll.GetUserInfoEntity(userId);  //��ȡ��ǰ�û�
                string fileName = "����(��)Υ�µ���_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
                string strDocPath = Server.MapPath("~/Resource/ExcelTemplate/����(��)Υ�µ���.doc");
                Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);
                HttpResponse resp = System.Web.HttpContext.Current.Response;
                //�û���Ϣ
                DataTable dt = new DataTable();
                dt.Columns.Add("UserName");
                dt.Columns.Add("DutyName");
                dt.Columns.Add("DeptName");
                DataRow urow = dt.NewRow();
                urow["UserName"] = !string.IsNullOrEmpty(userInfo.RealName) ? userInfo.RealName : "";
                urow["DutyName"] = !string.IsNullOrEmpty(userInfo.DutyName) ? userInfo.DutyName : "";
                urow["DeptName"] = userInfo.DeptName;
                if (userInfo.RoleName.Contains("����") || userInfo.RoleName.Contains("רҵ"))
                {
                    urow["DeptName"] = userInfo.ParentName + "/" + userInfo.DeptName;
                }
                dt.Rows.Add(urow);
                doc.MailMerge.Execute(dt);

                DataTable datadt = new DataTable("LllegalInfo");
                datadt.Columns.Add("SerialNumber");
                datadt.Columns.Add("LllegalDescribe");
                datadt.Columns.Add("LllegalTime");
                datadt.Columns.Add("EconomicsPunish");
                datadt.Columns.Add("LllegalPoint");
                datadt.Columns.Add("Money");
                datadt.Columns.Add("Points");

                int rownum = 1;
                foreach (DataRow wzrow in lllegaldt.Rows)
                {
                    DataRow row = datadt.NewRow();
                    row["SerialNumber"] = rownum;
                    row["LllegalDescribe"] = wzrow["lllegaldescribe"].ToString();
                    row["LllegalTime"] = wzrow["lllegaltime"].ToString();
                    row["EconomicsPunish"] = wzrow["economicspunish"].ToString();
                    row["LllegalPoint"] = wzrow["lllegalpoint"].ToString();
                    row["Money"] = wzrow["money"].ToString();
                    row["Points"] = wzrow["points"].ToString();
                    datadt.Rows.Add(row);
                    rownum++;
                }
                doc.MailMerge.ExecuteWithRegions(datadt);
                doc.MailMerge.DeleteFields();
                doc.Save(resp, Server.UrlEncode(fileName), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));

                return Success("�����ɹ�!");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion

        #region ��ȡ���ֵ���

        public List<OrganizeEntity> organizedata = new List<OrganizeEntity>();
        public List<DepartmentEntity> departmentdata = new List<DepartmentEntity>();

        #region ��ȡ���ŵ����νṹ
        /// <summary>
        /// ��ȡ���ŵ����νṹ
        /// </summary>
        /// <param name="EnCode"></param>
        /// <param name="ItemNameLike"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDataDeptWZ(string condition, string keyword)
        {
            var Year = Request["Year"] ?? "";
            Operator user = OperatorProvider.Provider.Current();
            List<TreeGridEntity> treeList = new List<TreeGridEntity>();
            var parentId = "0";
            var parentEntiy = departmentBLL.GetEntity(user.DeptId);
            if (parentEntiy != null)
                parentId = departmentBLL.GetEntity(user.DeptId).ParentId;

            if (user.IsSystem)
            {
                //organizedata = organizeCache.GetList().OrderByDescending(x => x.CreateDate).ToList();
                departmentdata = departmentBLL.GetList().OrderBy(a => a.SortCode).ToList();
            }
            else
            {
                if (user.RoleName.Contains("��˾���û�") || user.RoleName.Contains("���������û�") || user.DeptName.Contains("������"))
                {
                    //organizedata = organizeCache.GetList().OrderByDescending(x => x.CreateDate).Where(e => e.EnCode == user.OrganizeCode).ToList();
                    departmentdata = departmentBLL.GetList().OrderBy(a => a.SortCode).Where(e => e.OrganizeId == user.OrganizeId).ToList();
                }
                else
                {
                    departmentdata = departmentBLL.GetList(user.OrganizeId).Where(t => t.EnCode.Contains(user.DeptCode) || t.Description == "������̳а���" || t.SendDeptID == user.DeptId).OrderBy(x => x.SortCode).ToList();

                }
            }
            //�첽��������
            //����������ί��
            //MyDelegate dele = new MyDelegate(AddOrg);
            ////��Ӳ���
            //var result = dele.BeginInvoke(Year, null, null);
            //treeList.AddRange(dele.EndInvoke(result));
            MyDelegate dele = new MyDelegate(AddDept);
            var result = dele.BeginInvoke(Year, null, null);
            treeList.AddRange(dele.EndInvoke(result));

            var json = treeList.TreeJson(parentId);
            //��������
            return Content(json);

        }
        #endregion

        #region Υ�»��ֵ���(��λ) 

        /// <summary>
        /// Υ�»��ֵ���(��λ) 
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDataDeptWZY(string condition, string keyword)
        {

            Operator user = OperatorProvider.Provider.Current();
            //��ȡ���ŷ���
            decimal deptScore = 12; //Ĭ�ϳ�ʼ
            var dataitem = dataitemdetailbll.GetEntityByItemName("LllegalDeptPoint");
            if (null != dataitem)
            {
                if (!string.IsNullOrEmpty(dataitem.ItemValue))
                {
                    deptScore = Convert.ToDecimal(dataitem.ItemValue);//��ȡ�����ܷ�
                }
            }

            List<TreeGridEntity> treeList = new List<TreeGridEntity>();
            var parentId = "0";
            var parentEntiy = departmentBLL.GetEntity(user.DeptId);
            if (parentEntiy != null)
                parentId = departmentBLL.GetEntity(user.DeptId).ParentId;

            string yearStr = string.Empty;
            string Year = Request["Year"] ?? "";
            if (!string.IsNullOrEmpty(Year))
            {
                yearStr = string.Format(@" and to_char(lllegaltime, 'yyyy')='{0}'", Year);
            }
            var sql = "";
            var where = " 1=1 ";
            var order = " order by SortCode asc";
            if (!user.IsSystem)
            {
                if (user.RoleName.Contains("��˾���û�") || user.RoleName.Contains("���������û�") || user.DeptName.Contains("������"))
                {
                    where += string.Format(" and OrganizeId='{0}'", user.OrganizeId);
                }
                else
                {
                    where += string.Format(" and OrganizeId='{0}' and (encode like '{1}%' or Description='������̳а���' or SendDeptID='{2}')", user.OrganizeId, user.DeptCode, user.DeptId);
                }
            }
            sql += string.Format(@"select SortCode as Sort,DepartmentId,ParentId,OrganizeId,FullName,EnCode,HasChild ,
                                  (select count(1) as Num from v_lllegalassesforperson where DepartmentId=d.departmentid {0}) as PersonWZNum,
                                  (select nvl(sum(lllegalpoint),0) from  v_lllegalassesforperson where departmentid=d.departmentid {0}) as PersonWZScore,
                                  (select nvl(sum(lllegalpoint),0) from v_lllegalassesfordepart where DepartmentId=d.departmentid {0}) as DeptWZScore,
                                  {1} DeptScore  from ( select SendDeptID,Description,departmentid,organizeid,parentid,encode,fullname,sortcode ,haschild,1 serialnumber from base_department where nature !='�а���'  and nature !='�ְ���'  and fullname !='������̳а���' 
                                    union  select SendDeptID,Description,'cx100' departmentid, organizeid, parentid,encode,'��Э�����λ' fullname,sortcode,haschild,2 serialnumber  from base_department where fullname ='������̳а���'
                                    union  select SendDeptID,Description,departmentid,organizeid, 'cx100' parentid,encode,fullname,sortcode,haschild,3 serialnumber  from base_department where nature ='�а���' and depttype ='��Э'
                                    union  select SendDeptID,Description,'ls100' departmentid,organizeid, parentid,encode,'��ʱ�����λ' fullname,sortcode,haschild,4 serialnumber  from base_department where fullname ='������̳а���'
                                    union  select SendDeptID,Description,departmentid,organizeid, 'ls100' parentid,encode,fullname,sortcode,haschild,5 serialnumber  from base_department where nature ='�а���' and depttype ='��ʱ'  order by serialnumber asc ,sortcode asc ,encode) d where {2} {3} ", yearStr, deptScore, where, order);
            DataTable dt = hazardsourcebll.FindTableBySql(sql);
            foreach (DataRow dr in dt.Rows)
            {
                TreeGridEntity tree = new TreeGridEntity();
                bool hasChildren = dt.Select("parentid='" + dr["DepartmentId"].ToString() + "'").Count() == 0 ? false : true;
                var item = new DepartmentEntity()
                {
                    DepartmentId = dr["DepartmentId"].ToString(),
                    OrganizeId = dr["OrganizeId"].ToString(),
                    FullName = dr["FullName"].ToString(),
                    EnCode = dr["EnCode"].ToString(),
                    HasChild = hasChildren.ToString()
                };

                tree.id = dr["DepartmentId"].ToString();
                if (dr["ParentId"].ToString() == "0")
                {
                    tree.parentId = "0";
                }
                else
                {
                    tree.parentId = dr["ParentId"].ToString();
                }
                tree.expanded = true;
                tree.hasChildren = hasChildren;
                string itemJson = item.ToJson();
                itemJson = itemJson.Insert(1, "\"Sort\":\"Department\",");
                itemJson = itemJson.Insert(1, "\"PersonWZNum\":\"" + dr["PersonWZNum"].ToString() + "\",");
                itemJson = itemJson.Insert(1, "\"PersonWZScore\":\"" + dr["PersonWZScore"].ToString() + "\",");
                itemJson = itemJson.Insert(1, "\"DeptWZScore\":\"" + dr["DeptWZScore"].ToString() + "\",");
                itemJson = itemJson.Insert(1, "\"DeptScore\":\"" + dr["DeptScore"].ToString() + "\",");
                tree.entityJson = itemJson;
                treeList.Add(tree);
            }

            var json = treeList.TreeJson(parentId);
            //��������
            return Content(json);
        }
        #endregion

        #region ��ȡ��ԱΥ������
        /// <summary>
        /// ��ȡ��ԱΥ������
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetDataPersonWZY(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            //��ȡ���˷���
            decimal personScore = 12; //Ĭ�ϳ�ʼ
            var dataitem = dataitemdetailbll.GetEntityByItemName("LllegalPointInitValue"); // ����Ĭ��Υ�»���
            if (null != dataitem)
            {
                if (!string.IsNullOrEmpty(dataitem.ItemValue))
                {
                    personScore = Convert.ToDecimal(dataitem.ItemValue);//��ȡ����Ĭ��Υ�»��� 
                }
            }
            string dwhere = string.Empty;
            string cwhere = string.Empty;
            string Year = Request["Year"] ?? "";
            if (!string.IsNullOrEmpty(Year))
            {
                dwhere += string.Format(" and  to_char(b.lllegaltime, 'yyyy')='{0}'", Year);
                cwhere += string.Format(" and  to_char(a.createdate, 'yyyy')='{0}'", Year);
            }
            string twhere = string.Empty;
            string _parentId = Request["_parentId"] ?? "";
            if (!string.IsNullOrEmpty(_parentId))
            {
                twhere = string.Format(" and a.deptcode like '{0}%'", _parentId);
            }
            queryJson = queryJson ?? "";
            pagination.p_fields = "userid,realname,account,personwznum,personwzscore,awardnum,awardscore,persontotal,departmentid,deptcode,deptname,sortcode,deptsort";
           
            pagination.p_tablename = string.Format(@"��
                                                        with lllegalpointrecoverdetail as ( select count(1) pnum ,recoveruserid userid,createdate  from v_lllegalpointrecoverdetail group by recoveruserid,createdate ),
                                                        lllegalasses as ( 
                                                            select  count(a.id) PersonWZNum, sum(nvl(a.lllegalpoint,0)) PersonWZScore ,a.personinchargeid userid from bis_lllegalpunish a left join bis_lllegalregister b on a.lllegalid = b.id  left join lllegalpointrecoverdetail  c on a.personinchargeid = c.userid  where  a.personinchargeid is not null and  b.flowstate in (select itemname from v_yesqrwzstatus) and (nvl(c.pnum,0)=0  or (nvl(c.pnum,0)>0  and  b.createdate > c.createdate ))  {0}  group by a.personinchargeid
                                                         ),
                                                        lllegalawarddetail as (
                                                            select count(a.id) AwardNum, a.userid,sum(nvl(a.points,0)) points from bis_lllegalawarddetail a inner join bis_lllegalregister b on  a.lllegalid = b.id where b.flowstate in (select itemname from v_yesqrwzstatus)  {0}  group by a.userid 
                                                        ),
                                                        lllegalreward as ( 
                                                            select  a.createuserid userid, sum(b.lllegalpoint) repoints from  bis_lllegalregister a  left join  bis_lllegalreward b on a.id = b.lllegalid where  a.flowstate ='���̽���'   and b.status ='��ȷ��'  {3}  group by a.createuserid 
                                                        ) 
                                                        select a.deptcode ,a.deptname,a.realname,a.account,a.userid,a.departmentid ,a.sortcode,a.deptsort,nvl(b.PersonWZNum,0) PersonWZNum,nvl(b.PersonWZScore,0) PersonWZScore,({1} - nvl(b.PersonWZScore,0) + nvl(c.points,0) + nvl(d.repoints,0))  PersonTotal, nvl(c.AwardNum,0) AwardNum, (nvl(c.points,0) + nvl(d.repoints,0)) AwardScore  from v_userinfo a  left join lllegalasses b on a.userid = b.userid  left join lllegalawarddetail c on a.userid = c.userid left join lllegalreward d on a.userid = d.userid  where 1=1 {2} 
                                                      ) t ", dwhere, personScore, twhere, cwhere);
            pagination.conditionJson = "1=1";


            var watch = CommonHelper.TimerStart();
            var data = hazardsourcebll.GetPageList(pagination, queryJson);
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
        #endregion

        //����һ��ί��
        public delegate List<TreeGridEntity> MyDelegate(string Year);


        public List<TreeGridEntity> AddOrg(string Year)
        {
            List<TreeGridEntity> treeListO = new List<TreeGridEntity>();
            foreach (OrganizeEntity item in organizedata)
            {
                TreeGridEntity tree = new TreeGridEntity();
                bool hasChildren = organizedata.Count(t => t.ParentId == item.OrganizeId) == 0 ? false : true;
                if (hasChildren == false)
                {
                    hasChildren = departmentdata.Count(t => t.OrganizeId == item.OrganizeId) == 0 ? false : true;
                    //if (hasChildren == false)
                    //{
                    //    continue;
                    //}
                }
                tree.id = item.OrganizeId;
                tree.hasChildren = hasChildren;
                tree.parentId = item.ParentId;
                tree.expanded = true;
                string itemJson = item.ToJson();
                itemJson = itemJson.Insert(1, "\"DepartmentId\":\"" + item.OrganizeId + "\",");
                itemJson = itemJson.Insert(1, "\"Sort\":\"Organize\",");
                itemJson = itemJson.Insert(1, "\"DepartWZNum\":\"" + GetWZNum(item.OrganizeId, Year) + "\",");
                itemJson = itemJson.Insert(1, "\"DepartWZScore\":\"" + GetWZScore(item.OrganizeId, Year) + "\",");
                tree.entityJson = itemJson;
                treeListO.Add(tree);
            }
            return treeListO;
        }

        public List<TreeGridEntity> AddDept(object YearObj)
        {
            List<TreeGridEntity> treeListD = new List<TreeGridEntity>();
            var Year = YearObj.ToString();
            foreach (DepartmentEntity item in departmentdata)
            {
                TreeGridEntity tree = new TreeGridEntity();
                bool hasChildren = departmentdata.Count(t => t.ParentId == item.DepartmentId) == 0 ? false : true;
                tree.id = item.DepartmentId;
                if (item.ParentId == "0")
                {
                    tree.parentId = "0";
                }
                else
                {
                    tree.parentId = item.ParentId;
                }
                item.HasChild = hasChildren.ToString();
                tree.expanded = true;
                tree.hasChildren = hasChildren;
                string itemJson = item.ToJson();
                itemJson = itemJson.Insert(1, "\"Sort\":\"Department\",");
                itemJson = itemJson.Insert(1, "\"DepartWZNum\":\"" + GetWZNum(item.DepartmentId, Year) + "\",");
                itemJson = itemJson.Insert(1, "\"DepartWZScore\":\"" + GetWZScore(item.DepartmentId, Year) + "\",");
                tree.entityJson = itemJson;
                treeListD.Add(tree);
            }
            return treeListD;
        }

        #region ��ȡ���ŵ�Υ�´���
        /// <summary>
        /// ��ȡ���ŵ�Υ�´���
        /// </summary>
        /// <param name="DepartmentId"></param>
        /// <param name="Year"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int GetWZNum(string DepartmentId, string Year)
        {
            string sql = "select count(1) as Num from v_lllegalassesforperson where DepartmentId='" + DepartmentId + "'";

            if (Year.Length > 0)
            {
                sql += " and to_char(lllegaltime, 'yyyy')='" + Year + "'";

            }
            var dt = hazardsourcebll.FindTableBySql(sql);
            return int.Parse(dt.Rows[0]["Num"].ToString());
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DepartmentId"></param>
        /// <returns></returns>
        public decimal GetWZScore(string DepartmentId, string Year)
        {

            string sql = "select lllegallevelname,Id,lllegalpoint from v_lllegalassesforperson where DepartmentId='" + DepartmentId + "'";
            if (Year.Length > 0)
            {
                sql += " and to_char(lllegaltime, 'yyyy')='" + Year + "'";

            }
            var dt = hazardsourcebll.FindTableBySql(sql);
            decimal score = 0;
            foreach (DataRow dr in dt.Rows)
            {
                score += decimal.Parse(dr["lllegalpoint"].ToString());

            }

            return score;
        }
        private DataTable GetCount()
        {
            DataTable dt = new DataTable();
            string sql = "";

            dt = hazardsourcebll.FindTableBySql(sql);

            return dt;
        }
        #endregion

        #region ��ԱΥ����Ϣ
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetPersonWzInfo(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            //��ȡ���˷���
            decimal personScore = 12; //Ĭ�ϳ�ʼ
            var dataitem = dataitemdetailbll.GetEntityByItemName("LllegalPointInitValue"); // ����Ĭ��Υ�»���
            if (null != dataitem)
            {
                if (!string.IsNullOrEmpty(dataitem.ItemValue))
                {
                    personScore = Convert.ToDecimal(dataitem.ItemValue);//��ȡ����Ĭ��Υ�»��� 
                }
            }
            string deptid = Request["DeptId"] ?? "";

            string tWhere = string.Empty;
            string dWhere = string.Empty;
            string tempWhere = string.Empty;
            if (!string.IsNullOrEmpty(deptid))
            {
                tWhere = string.Format(" and d.departmentid='{0}'", deptid);
                dWhere = string.Format(" and a.departmentid='{0}'", deptid); 
            }
            string Year = Request["Year"] ?? "";
            if (!string.IsNullOrEmpty(Year))
            {
                tWhere += string.Format(" and  to_char(b.lllegaltime, 'yyyy')='{0}'", Year);
                tempWhere += string.Format(" and  to_char(a.createdate, 'yyyy')='{0}'", Year);
            }
            string mode = Request["mode"] ?? "";
            decimal recoverPoint = 4; //Ĭ��4��
            var recoveritem = dataitemdetailbll.GetEntityByItemName("LllegalRecoverPoint"); // Υ�»ָ����ֲ�ѯ����
            if (null != recoveritem)
            {
                if (!string.IsNullOrEmpty(recoveritem.ItemValue))
                {
                    recoverPoint = Convert.ToDecimal(recoveritem.ItemValue);//��ȡ����Ĭ��Υ�»��� 
                }
            }
            queryJson = queryJson ?? "";
            pagination.p_fields = "deptname,realname,wznum,points,lllegalpoint,personscore,userid,organizeid";

            pagination.p_tablename = string.Format(@"( with lllegalpointrecoverdetail as ( 
                                                              select count(1) pnum ,recoveruserid userid,createdate  from v_lllegalpointrecoverdetail group by recoveruserid,createdate  
                                                          ),
                                                         lllegalasses as ( 
                                                              select count(a.id) wznum,sum(nvl(a.lllegalpoint,0)) lllegalpoint,a.personinchargeid userid  from bis_lllegalpunish a left join bis_lllegalregister b on a.lllegalid = b.id left join lllegalpointrecoverdetail  c on a.personinchargeid = c.userid  left join base_user d on a.personinchargeid = d.userid where a.personinchargeid is not null and  b.flowstate in (select itemname from v_yesqrwzstatus) and (nvl(c.pnum,0)=0  or (nvl(c.pnum,0)>0  and  b.createdate > c.createdate )) {1} group by a.personinchargeid
                                                          ),
                                                          lllegalawarddetail as (
                                                              select a.userid,sum(nvl(a.points,0)) points from bis_lllegalawarddetail a inner join bis_lllegalregister b on  a.lllegalid = b.id  left join base_user d on a.userid = d.userid where b.flowstate in (select itemname from v_yesqrwzstatus) {1} group by a.userid
                                                          ),
                                                          lllegalreward as ( 
                                                              select  a.createuserid userid, sum(b.lllegalpoint) repoints from  bis_lllegalregister a  left join  bis_lllegalreward b on a.id = b.lllegalid  where  a.flowstate ='���̽���'  and b.status ='��ȷ��' {2}  group by a.createuserid 
                                                          )
                                                          select a.deptname,a.realname,nvl(b.wznum,0) wznum, nvl(b.lllegalpoint,0) lllegalpoint, ({0} - nvl(b.lllegalpoint,0) + nvl(c.points,0) + nvl(d.repoints,0))  personscore,(nvl(c.points,0) + nvl(d.repoints,0)) points,a.userid,a.organizeid  from lllegalasses b left join  v_userinfo a  on b.userid = a.userid  left join lllegalawarddetail c on a.userid = c.userid left join lllegalreward d on a.userid = d.userid  where 1=1 {3} 
                                                    ) t", personScore, tWhere, tempWhere, dWhere);

            pagination.conditionJson = "1=1";

            if (!string.IsNullOrEmpty(mode))
            {
                //���ָֻ�
                if (mode == "recover")
                {
                    pagination.conditionJson += string.Format(" and  personscore <={0}  ", recoverPoint);
                }
                else if (mode == "history")  //��ʷ��¼  �ָ���Υ����Ա����
                {
                    pagination.p_tablename = string.Format(@"( with lllegalpointrecoverdetail as ( 
                                                               select count(1) pnum ,recoveruserid userid,createdate  from v_lllegalpointrecoverdetail group by recoveruserid,createdate  
                                                              ),
                                                             lllegalasses as ( 
                                                                  select count(a.id) wznum,sum(nvl(a.lllegalpoint,0)) lllegalpoint,a.personinchargeid userid  from bis_lllegalpunish a left join bis_lllegalregister b on a.lllegalid = b.id left join lllegalpointrecoverdetail  c on a.personinchargeid = c.userid  left join base_user d on a.personinchargeid = d.userid where a.personinchargeid is not null and  b.flowstate in (select itemname from v_yesqrwzstatus) and  nvl(c.pnum,0)>0  and  b.createdate < c.createdate {1} group by a.personinchargeid
                                                              ),
                                                              lllegalawarddetail as (
                                                                  select a.userid,sum(nvl(a.points,0)) points from bis_lllegalawarddetail a inner join bis_lllegalregister b on  a.lllegalid = b.id  left join base_user d on a.userid = d.userid where b.flowstate in (select itemname from v_yesqrwzstatus) {1} group by a.userid
                                                              ),
                                                              lllegalreward as ( 
                                                                  select  a.createuserid userid, sum(b.lllegalpoint) repoints from  bis_lllegalregister a  left join  bis_lllegalreward b on a.id = b.lllegalid  where  a.flowstate ='���̽���'  and b.status ='��ȷ��' {2}  group by a.createuserid 
                                                              )
                                                              select a.deptname,a.realname,nvl(b.wznum,0) wznum, nvl(b.lllegalpoint,0) lllegalpoint,({0} - nvl(b.lllegalpoint,0) + nvl(c.points,0) + nvl(d.repoints,0))  personscore,(nvl(c.points,0) + nvl(d.repoints,0)) points,a.userid,a.organizeid  from lllegalasses b left join  v_userinfo a  on b.userid = a.userid  left join lllegalawarddetail c on a.userid = c.userid left join lllegalreward d on a.userid = d.userid  where 1=1 {3} and a.userid in  (select distinct recoveruserid from v_lllegalpointrecoverdetail)
                                                    ) t", personScore, tWhere, tempWhere, dWhere);
                }
                else if (mode == "underEight") // Υ�»��ֵ���8�ֵ���Ա
                {
                    pagination.p_tablename = string.Format(@"( with lllegalpointrecoverdetail as ( 
                                                              select count(1) pnum ,recoveruserid userid,createdate  from v_lllegalpointrecoverdetail group by recoveruserid,createdate  
                                                          ),
                                                         lllegalasses as ( 
                                                              select count(a.id) wznum,sum(nvl(a.lllegalpoint,0)) lllegalpoint,a.personinchargeid userid  from bis_lllegalpunish a left join bis_lllegalregister b on a.lllegalid = b.id left join lllegalpointrecoverdetail  c on a.personinchargeid = c.userid  left join base_user d on a.personinchargeid = d.userid where a.personinchargeid is not null and  b.flowstate in (select itemname from v_yesqrwzstatus) and (nvl(c.pnum,0)=0  or (nvl(c.pnum,0)>0  and  b.createdate > c.createdate ))  and d.organizeid='{1}' and  to_char(b.lllegaltime,'yyyy')='{2}'  group by a.personinchargeid
                                                          ),
                                                          lllegalawarddetail as (
                                                              select a.userid,sum(nvl(a.points,0)) points from bis_lllegalawarddetail a inner join bis_lllegalregister b on  a.lllegalid = b.id  where b.flowstate in (select itemname from v_yesqrwzstatus) and a.deptid in (select distinct departmentid from  base_department where organizeid= '{1}') and  to_char(b.lllegaltime,'yyyy')='{2}' group by a.userid
                                                          ),
                                                          lllegalreward as ( 
                                                              select  a.createuserid userid, sum(b.lllegalpoint) repoints from  bis_lllegalregister a  left join  bis_lllegalreward b on a.id = b.lllegalid  where  a.flowstate ='���̽���'  and b.status ='��ȷ��' and  to_char(a.createdate, 'yyyy')='{2}'  group by a.createuserid 
                                                          )
                                                          select a.deptname,a.realname,nvl(b.wznum,0) wznum, nvl(b.lllegalpoint,0) lllegalpoint, ({0} - nvl(b.lllegalpoint,0) + nvl(c.points,0) + nvl(d.repoints,0))  personscore,(nvl(c.points,0) + nvl(d.repoints,0)) points,a.userid,a.organizeid  from lllegalasses b left join  v_userinfo a  on b.userid = a.userid  left join lllegalawarddetail c on a.userid = c.userid left join lllegalreward d on a.userid = d.userid  where 1=1 
                                                    ) t", personScore, user.OrganizeId, DateTime.Now.Year.ToString());

                    pagination.conditionJson += string.Format(" and  personscore <=8  ");
                }
            }
            var watch = CommonHelper.TimerStart();
            var data = hazardsourcebll.GetPageList(pagination, queryJson);
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
        #endregion

        #region ��ԱΥ����Ϣ
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetXssWzPersonInfo(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var queryParam = queryJson.ToJObject();
            string strWhere = string.Empty;
            decimal basePoint = 12; //��������
            var lllegalPoint = dataitemdetailbll.GetDataItemListByItemCode("'LllegalTrainPointSetting'");
            if (lllegalPoint.Count() > 0)
            {
                var LllegalPointInitValue = lllegalPoint.Where(p => p.ItemName == "LllegalPointInitValue").FirstOrDefault();
                if (null != LllegalPointInitValue)
                {
                    basePoint = Convert.ToDecimal(LllegalPointInitValue.ItemValue);
                }
            }

            //����
            if (!string.IsNullOrEmpty(queryParam["Code"].ToString()))
            {
                strWhere += string.Format(@" and c.encode  like '{0}%'", queryParam["Code"].ToString());
            }
            //���
            if (!string.IsNullOrEmpty(queryParam["TimeScope"].ToString()))
            {
                strWhere += string.Format(@" and a.lllegalid in (select id from v_lllegalbaseinfo where to_char(lllegaltime, 'yyyy')='{0}' and createuserorgcode ='{1}')", queryParam["TimeScope"].ToString(), user.OrganizeCode);
            }
            //��˾��  ����
            if (user.RoleName.Contains("��˾��") || user.RoleName.Contains("����"))
            {
                strWhere += @" and  a.lllegalid  in (select distinct objectid from v_xsslllegalpointsdata where  rolename  like '%��˾��%' or  rolename like '%����%')";
            }
            //����
            if (user.RoleName.Contains("���ż�") && !user.RoleName.Contains("����"))
            {
                strWhere += string.Format(@" and  a.lllegalid  in (select distinct objectid from v_xsslllegalpointsdata where  rolename  like '%���ż�%' and   rolename  not like '%����%'  and  encode ='{0}')", user.DeptCode);
            }
            //���鼶
            if (user.RoleName.Contains("���鼶"))
            {
                strWhere += string.Format(@" and  a.lllegalid in (select distinct objectid from v_xsslllegalpointsdata where  rolename  like '%���鼶%' and  encode ='{0}')", user.DeptCode);
            }
            //����
            string tablename = string.Format(@"(select  sum(nvl(a.lllegalpoint,0)) lllegalpoint, a.personinchargeid userid ,b.realname ,c.fullname  deptname  ,c.encode,count(a.lllegalid) wznum ,({0}- sum(nvl(a.lllegalpoint,0))) lllegaljf   from bis_lllegalpunish a 
                                            left join base_user  b on a.personinchargeid = b.userid
                                            left join base_department c on b.departmentid = c.departmentid
                                            left join base_user d on a.createuserid = d.userid  where a.assessobject like '%��Ա%' and a.personinchargeid is not null and c.departmentid is not null  {1}
                                            group by a.personinchargeid ,b.realname,c.fullname,c.encode) a", basePoint, strWhere);
            pagination.p_fields = "userid,deptname,realname,wznum,lllegalpoint,lllegaljf";
            pagination.p_tablename = tablename;
            pagination.conditionJson = "1=1  and lllegalpoint>0 ";

            var watch = CommonHelper.TimerStart();
            var data = lllegalregisterbll.GetGeneralQuery(pagination);
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
        #endregion

        #region ���ݵ���
        /// <summary>
        /// δ���¼���������鴦��
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "���ݵ���")]
        public ActionResult Export()
        {
            var Year = Request["Year"] ?? "";
            Operator user = OperatorProvider.Provider.Current();
            List<OrganizeEntity> organizedata = new List<OrganizeEntity>();
            List<DepartmentEntity> departmentdata = new List<DepartmentEntity>();
            var parentId = "0";
            if (user.IsSystem)
            {
                departmentdata = departmentBLL.GetList().OrderBy(a => a.SortCode).ToList();
            }
            else
            {
                if (user.RoleName.Contains("��˾���û�") || user.RoleName.Contains("���������û�") || user.DeptName.Contains("������"))
                {
                    departmentdata = departmentBLL.GetList().OrderBy(a => a.SortCode).Where(e => e.EnCode.Substring(0, user.DeptCode.Length) == user.DeptCode).ToList();
                }
                else
                {
                    departmentdata = departmentBLL.GetList().OrderBy(a => a.SortCode).Where(e => e.EnCode.Length >= user.DeptCode.Length && e.EnCode.Substring(0, user.DeptCode.Length) == user.DeptCode).ToList();
                    parentId = user.ParentId;
                }
            }
            var treeList = new List<TreeGridEntity>();

            foreach (DepartmentEntity item in departmentdata)
            {
                TreeGridEntity tree = new TreeGridEntity();
                bool hasChildren = departmentdata.Count(t => t.ParentId == item.DepartmentId) == 0 ? false : true;
                tree.id = item.DepartmentId;
                if (item.ParentId == "0")
                {
                    tree.parentId = item.OrganizeId;
                }
                else
                {
                    tree.parentId = item.ParentId;
                }
                item.HasChild = hasChildren.ToString();
                tree.expanded = true;
                tree.hasChildren = hasChildren;
                string itemJson = item.ToJson();
                itemJson = itemJson.Insert(1, "\"Sort\":\"Department\",");
                var Ids = "";
                itemJson = itemJson.Insert(1, "\"DepartWZNum\":\"" + GetWZNum(item.DepartmentId, Year) + "\",");

                itemJson = itemJson.Insert(1, "\"DepartWZScore\":\"" + GetWZScore(item.DepartmentId, Year) + "\",");
                itemJson = itemJson.Insert(1, "\"Ids\":\"" + Ids + "\",");
                tree.entityJson = itemJson;
                treeList.Add(tree);
            }


            //���õ�����ʽ
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "���ֵ���";
            excelconfig.TitleFont = "΢���ź�";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "���ֵ���.xls";
            excelconfig.IsAllSizeColumn = true;
            //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "fullname".ToLower(), ExcelColumn = "��������" });
            listColumnEntity.Add(new ColumnEntity() { Column = "DepartWZNum".ToLower(), ExcelColumn = "Υ�´���" });
            listColumnEntity.Add(new ColumnEntity() { Column = "DepartWZScore".ToLower(), ExcelColumn = "Υ�¿۷�" });
            listColumnEntity.Add(new ColumnEntity() { Column = "DepartWZJF".ToLower(), ExcelColumn = "Υ�»���" });
            excelconfig.ColumnEntity = listColumnEntity;
            List<TemplateMode> list = new List<TemplateMode>();
            var dt = new DataTable();
            DataColumn dc = dt.Columns.Add("fullname", Type.GetType("System.String"));
            dc = dt.Columns.Add("departwznum", Type.GetType("System.String"));
            dc = dt.Columns.Add("departwzscore", Type.GetType("System.String"));
            dc = dt.Columns.Add("departwzjf", Type.GetType("System.String"));
            foreach (var item in treeList)
            {
                var entity = JsonConvert.DeserializeObject<DataEntity>(item.entityJson);
                DataRow dr = dt.NewRow();
                dr["fullname"] = entity.FullName;
                dr["departwznum"] = entity.DepartWZNum;
                dr["departwzscore"] = entity.DepartWZScore;
                dr["departwzjf"] = 12 - decimal.Parse(entity.DepartWZScore.ToString());
                dt.Rows.Add(dr);
            }
            //���õ�������
            ExcelHelper.ExcelDownload(dt, excelconfig);
            return Success("�����ɹ���");
        }
        public class DataEntity
        {
            public string FullName { get; set; }
            public string DepartWZNum { get; set; }
            public string DepartWZScore { get; set; }
            public string DepartWZJF { get; set; }
        }


        #endregion

        #region ��ȡ��ԱΥ�»�����������
        /// <summary>
        /// ��ȡ��ԱΥ�»�����������
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public ActionResult GetLllegalPointRemindDataJson()
        {
            Operator curUser = OperatorProvider.Provider.Current();
            //��ԱΥ�»�����������
            string itemCode = "'LllegalPointRemindSetting'";
            //����
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode(itemCode);

            decimal remindValue = 0;

            var remindList = itemlist.Where(p => p.ItemName == curUser.OrganizeCode).ToList();

            if (remindList.Count() > 0)
            {
                remindValue = decimal.Parse(remindList.FirstOrDefault().ItemValue);
            }
            //����ֵ
            var josnData = new
            {
                RemindValue = remindValue
            };

            return Content(josnData.ToJson());
        }
        #endregion

        #region  ��������������޸ģ�
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveRemindForm(string keyValue)
        {
            Operator curUser = OperatorProvider.Provider.Current();

            decimal RemindValue = decimal.Parse(Request.Form["RemindValue"].ToString());
            //��ԱΥ�»�����������
            string itemCode = "'LllegalPointRemindSetting'";

            DataItemDetailEntity entity = new DataItemDetailEntity();

            //����
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode(itemCode);

            var itemdata = new DataItemBLL().GetEntityByCode("LllegalPointRemindSetting");

            var remindList = itemlist.Where(p => p.ItemName == curUser.OrganizeCode).ToList();

            if (remindList.Count() > 0)
            {
                var model = remindList.FirstOrDefault();
                entity.ItemDetailId = model.ItemDetailId;
                entity.ItemCode = model.ItemCode;
                entity.ItemName = model.ItemName;
                entity.ItemValue = RemindValue.ToString();
                entity.ItemId = model.ItemId;
                entity.Description = model.Description;
                entity.EnabledMark = model.EnabledMark;
                entity.ParentId = model.ParentId;
                entity.SortCode = model.SortCode;
            }
            else
            {
                entity.ItemName = curUser.OrganizeCode;
                entity.ItemValue = RemindValue.ToString();
                entity.ItemCode = "WZJFLJZ";
                entity.Description = curUser.OrganizeName + "Υ�¼Ƿ��ٽ�ֵ";
                entity.ItemId = itemdata.ItemId;
                entity.ParentId = "0";
                entity.EnabledMark = 1;
            }
            dataitemdetailbll.SaveForm(entity.ItemDetailId, entity);
            return Success("�����ɹ�!");
        }
        #endregion

        #region �ճ���鵼��
        /// <summary>
        /// �ճ���鵼��
        /// </summary>
        [HandlerMonitor(0, "��������")]
        public ActionResult DailyExportData(string queryJson, string mtype, string mode = "")
        {
            try
            {

                // ��ϸ�б�����
                string fielname = DateTime.Now.Year.ToString() + "��ȫ�ն���Υ������((ͳ����).xls";
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                wb.Open(Server.MapPath("~/Resource/ExcelTemplate/��ȫ�ն���Υ������((ͳ����).xls"));


                SaftyCheckDataRecordBLL srbll = new SaftyCheckDataRecordBLL();
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 100000000;
                pagination.p_kid = "t.ID";
                pagination.p_fields = "CheckBeginTime,checkendtime,CheckDataRecordName,CheckLevel,CheckMan,'δ��ʼ���' SolveCount,'' count,t.SolvePerson,'' count1";
                pagination.conditionJson = "1=1";
                pagination.sidx = "CreateDate desc,id";
                pagination.sord = "desc";
                var user = OperatorProvider.Provider.Current();
                string where1 = "";
                string arg = user.DeptCode;
                if (!user.IsSystem)
                {
                    if (user.RoleName.Contains("��˾") || user.RoleName.Contains("����") || user.RoleName.Contains("����") || user.RoleName.Contains("ʡ���û�"))
                    {
                        pagination.conditionJson += string.Format(" and (t1.id is not null or createuserdeptcode in (select encode from base_department start with encode='{0}' connect by  prior departmentid = parentid))", user.OrganizeCode);
                    }
                    else
                    {
                        arg = user.DeptCode;
                        where1 = string.Format(" or senddeptid='{0}'", user.DeptId);
                        pagination.conditionJson += string.Format("  and (t1.id is not null or createuserdeptcode='{0}')", arg);

                    }
                    pagination.conditionJson += string.Format(" and checkeddepartid is null and createuserdeptcode in (select encode from base_department start with encode='{0}' connect by  prior departmentid = parentid)", user.OrganizeCode);
                    pagination.p_tablename = string.Format(@"bis_saftycheckdatarecord t left join(select id from (select id,(','||checkmanid||',') as checkmanid  from  bis_saftycheckdatarecord) a 
                                                             left join (
                                                                 select (','||account||',') as account from base_user where  departmentcode in(select encode from base_department  where deptcode like '{0}%' {1} )
                                                             ) b on a.checkmanid  like '%'||b.account||'%' where account is not null group by id)t1
                                                             on t.ID=t1.id ", arg, where1);
                }
                else
                {
                    pagination.p_tablename = "bis_saftycheckdatarecord t";
                }
                //��ȡ��ȫ�����
                DataTable dt = srbll.ExportData(pagination, queryJson);

                List<string> ids = new List<string>();

                foreach (DataRow row in dt.Rows)
                {
                    ids.Add(row["id"].ToString());
                }
                //Υ�¿����ܱ�
                DataTable resultDt = lllegalregisterbll.GetLllegalBySafetyCheckIds(ids, 0);

                //��ίΥ��ͳ�� �ڶ��ű�
                Aspose.Cells.Worksheet sheet1 = wb.Worksheets[1] as Aspose.Cells.Worksheet;
                Aspose.Cells.Cell cell1 = sheet1.Cells[0, 0];
                cell1.PutValue(DateTime.Now.Year.ToString() + "�������Υ��ͳ��"); //����
                cell1.Style.Pattern = BackgroundType.Solid;
                cell1.Style.Font.Size = 14;
                cell1.Style.Font.Color = Color.Black;

                //��ίΥ��ͳ�� �����ű�
                Aspose.Cells.Worksheet sheet2 = wb.Worksheets[2] as Aspose.Cells.Worksheet;
                Aspose.Cells.Cell cell2 = sheet2.Cells[0, 0];
                cell2.PutValue(DateTime.Now.Year.ToString() + "����ί��λΥ��ͳ��"); //����
                cell2.Style.Pattern = BackgroundType.Solid;
                cell2.Style.Font.Size = 14;
                cell2.Style.Font.Color = Color.Black;

                List<string> haveDept = new List<string>();

                int wwIndex = 0;
                foreach (DataRow row in resultDt.Rows)
                {
                    if (!string.IsNullOrEmpty(row["expteam"].ToString()))
                    {
                        if (!haveDept.Contains(row["expteamcode"].ToString()))
                        {
                            haveDept.Add(row["expteamcode"].ToString());

                            Aspose.Cells.Cell wwcell = sheet2.Cells[3 + wwIndex, 1];
                            wwcell.PutValue(row["expteam"].ToString());
                            Aspose.Cells.Cell zrbmcell = sheet2.Cells[3 + wwIndex, 2];
                            zrbmcell.PutValue(row["glbmname"].ToString());

                            wwIndex++;
                        }
                    }
                }

                //��һ�ű�
                Aspose.Cells.Worksheet sheet = wb.Worksheets[0] as Aspose.Cells.Worksheet;
                Aspose.Cells.Cell cell = sheet.Cells[0, 0];
                cell.PutValue(DateTime.Now.Year.ToString() + "�갲ȫ�ն�����ܱ�"); //����
                cell.Style.Pattern = BackgroundType.Solid;
                cell.Style.Font.Size = 14;
                cell.Style.Font.Color = Color.Black;

                resultDt.Columns.Remove("lllegalteam");
                resultDt.Columns.Remove("lllegaldepart");
                resultDt.Columns.Remove("lllegalteamcode");
                resultDt.Columns.Remove("lllegaldepartcode");
                resultDt.Columns.Remove("expteamcode");
                resultDt.Columns.Remove("expdepartcode");
                resultDt.Columns.Remove("wzdwnature");
                resultDt.Columns.Remove("wzzrdwnature");
                resultDt.Columns.Remove("wwparent");
                resultDt.Columns.Remove("bmparent");
                resultDt.Columns.Remove("glbmname");
                resultDt.Columns.Remove("id");
                //�ȵ�������
                sheet.Cells.ImportDataTable(resultDt, false, 2, 0);

                if (!string.IsNullOrEmpty(mode))
                {
                    string tempSavePath = Server.MapPath("~/Resource/Temp/") + fielname;
                    wb.Save(tempSavePath);
                    string url = "../../Utility/DownloadFile?filePath=~/Resource/Temp/" + fielname + "&speed=10240000&newFileName=" + fielname;
                    return Redirect(url);
                }
                else
                {
                    HttpResponse resp = System.Web.HttpContext.Current.Response;
                    resp.Clear();
                    resp.Buffer = true;
                    wb.Save(Server.UrlEncode(fielname), Aspose.Cells.FileFormatType.Excel2003, Aspose.Cells.SaveType.OpenInBrowser, resp);
                }

            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
            return Success("�����ɹ���");
        }
        #endregion
    }
}
