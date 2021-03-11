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
        private LllegalPunishBLL lllegalpunishbll = new LllegalPunishBLL(); // ������Ϣ����
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private OrganizeCache organizeCache = new OrganizeCache();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();
        private HazardsourceBLL hazardsourcebll = new HazardsourceBLL();

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
            //Υ�±���
            string LllegalNumber = DateTime.Now.ToString("yyyyMMddHHmmssfff").ToString();
            //���˷�ʽ Υ������ Υ�µȼ� ����״̬
            string itemCode = "'ExamineWay','LllegalType','LllegalLevel','FlowState'";
            //����
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode(itemCode);
            //��˾���û�
            if (userbll.HaveRoleListByKey(CreateUser.UserId, dataitemdetailbll.GetItemValue("HidOrganize")).Rows.Count > 0)
            {
                CreateUser.DeptCode = CreateUser.OrganizeCode;
                CreateUser.DeptName = CreateUser.OrganizeName;
            }

            string mark = userbll.GetSafetyAndDeviceDept(CreateUser); //���ڱ�ǿ���������Υ��

            string isPrincipal = userbll.HaveRoleListByKey(CreateUser.UserId, dataitemdetailbll.GetItemValue("PrincipalUser")).Rows.Count > 0 ? "1" : "";

            string isEpiboly = userbll.HaveRoleListByKey(CreateUser.UserId, dataitemdetailbll.GetItemValue("EpibolyUser")).Rows.Count > 0 ? "1" : "";  //�а���
            //����ֵ
            var josnData = new
            {
                CreateUser = CreateUser.UserName,
                User = CreateUser,  //�û�����
                Mark = mark, // 1 Ϊ��ȫ������  2 Ϊװ�ò���
                IsPrincipal = isPrincipal,
                IsEpiboly = isEpiboly,  //�а���
                ApplianceClass = dataitemdetailbll.GetItemValue("ApplianceClass"),  //װ������� 
                LllegalNumber = LllegalNumber,   //Υ�±��
                //LllegalPic = Guid.NewGuid().ToString(), //Υ��ͼƬ 
                //ReformPic = Guid.NewGuid().ToString(), //����ͼƬ
                //AcceptPic = Guid.NewGuid().ToString(), //����ͼƬ  
                ExamineWay = itemlist.Where(p => p.EnCode == "ExamineWay"), //���˷�ʽ
                LllegalType = itemlist.Where(p => p.EnCode == "LllegalType"), //Υ������
                LllegalLevel = itemlist.Where(p => p.EnCode == "LllegalLevel"),  //Υ�¼���
                FlowState = itemlist.Where(p => p.EnCode == "FlowState")  //Υ������״̬
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
            string itemCode = "'ExamineWay','LllegalType','LllegalLevel','FlowState','LllegalDataScope'";
            //����
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode(itemCode);
            string mark = userbll.GetSafetyAndDeviceDept(CreateUser); //���ڱ�ǿ���������Υ��

            string isPrincipal = userbll.HaveRoleListByKey(CreateUser.UserId, dataitemdetailbll.GetItemValue("PrincipalUser")).Rows.Count > 0 ? "1" : ""; //������

            string isEpiboly = userbll.HaveRoleListByKey(CreateUser.UserId, dataitemdetailbll.GetItemValue("EpibolyUser")).Rows.Count > 0 ? "1" : "";  //�а���
            //����ֵ
            var josnData = new
            {
                Mark = mark, // 1 Ϊ��ȫ������  2 Ϊװ�ò���
                IsPrincipal = isPrincipal,
                IsEpiboly = isEpiboly,  //�а���
                ApplianceClass = dataitemdetailbll.GetItemValue("ApplianceClass"),  //װ������� 
                ExamineWay = itemlist.Where(p => p.EnCode == "ExamineWay"), //���˷�ʽ
                LllegalType = itemlist.Where(p => p.EnCode == "LllegalType"), //Υ������
                LllegalLevel = itemlist.Where(p => p.EnCode == "LllegalLevel"),  //Υ�¼��� 
                FlowState = itemlist.Where(p => p.EnCode == "FlowState"),  //Υ������״̬
                DataScope = itemlist.Where(p => p.EnCode == "LllegalDataScope")  //���ݷ�Χ 
            };

            return Content(josnData.ToJson());
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

            var punishInfo = lllegalpunishbll.GetEntityByBid(baseInfo.ID); //��������

            var acceptInfo = lllegalacceptbll.GetEntityByBid(baseInfo.ID); //������Ϣ

            var userInfo = OperatorProvider.Provider.Current();  //��ȡ��ǰ�û�

            //����
            if (userInfo.isPlanLevel == "1")
            {
                userInfo.DeptName = userInfo.OrganizeName;
                userInfo.DeptCode = userInfo.OrganizeCode;
            }

            var data = new { baseInfo = baseInfo, approveInfo = approveInfo, reformInfo = reformInfo, punishInfo = punishInfo, acceptInfo = acceptInfo, userInfo = userInfo };

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
        public ActionResult SaveForm(string keyValue, LllegalRegisterEntity entity, LllegalPunishEntity pbEntity, LllegalReformEntity rEntity, LllegalAcceptEntity aEntity)
        {
            CommonSaveForm(keyValue, "03", entity, pbEntity, rEntity, aEntity);
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
        public void CommonSaveForm(string keyValue, string workFlow, LllegalRegisterEntity entity, LllegalPunishEntity pbEntity, LllegalReformEntity rEntity, LllegalAcceptEntity aEntity)
        {
            //�ύͨ��
            string userId = OperatorProvider.Provider.Current().UserId;

            //����Υ�»�����Ϣ
            entity.RESEVERFOUR = "";
            entity.RESEVERFIVE = "";
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


            string PUNISHID = Request.Form["PUNISHID"].ToString();
            pbEntity.LLLEGALID = entity.ID;
            pbEntity.MARK = "0"; //�����"0"��ʾ���˼�¼��Ϣ��"1"��ʾΥ�º�׼�ĺ�׼���˼�¼
            //������ʱ�����ӿ���
            if (!string.IsNullOrEmpty(PUNISHID))
            {
                var tempEntity = lllegalpunishbll.GetEntity(PUNISHID);
                pbEntity.ID = tempEntity.ID;
                pbEntity.AUTOID = tempEntity.AUTOID;
                pbEntity.APPROVEID = null;
            }
            lllegalpunishbll.SaveForm(PUNISHID, pbEntity);

            /********������Ϣ************/
            string REFORMID = Request.Form["REFORMID"].ToString();
            rEntity.LLLEGALID = entity.ID;

            //����״̬�����
            if (!string.IsNullOrEmpty(REFORMID))
            {
                var tempEntity = lllegalreformbll.GetEntity(REFORMID);
                rEntity.AUTOID = tempEntity.AUTOID;
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
        public ActionResult SubmitForm(string keyValue, LllegalRegisterEntity entity, LllegalPunishEntity pbEntity,
            LllegalReformEntity rEntity, LllegalAcceptEntity aEntity)
        {
            //�ж��ظ���Ź���
            var curHtBaseInfor = lllegalregisterbll.GetListByNumber(entity.LLLEGALNUMBER).FirstOrDefault();

            if (null != curHtBaseInfor)
            {
                if (curHtBaseInfor.ID != keyValue && string.IsNullOrEmpty(keyValue))
                {
                    return Error("Υ�±���ظ�,����������!");
                }
            }
            string errorMsg = string.Empty;

            bool isAddScore = false; //�Ƿ���ӵ��û�����

            try
            {
                //�������̣������Ӧ��Ϣ
                CommonSaveForm(keyValue, "03", entity, pbEntity, rEntity, aEntity);
                //����������ʵ����
                if (string.IsNullOrEmpty(keyValue))
                {
                    keyValue = entity.ID;
                }
                //�˴���Ҫ�жϵ�ǰ���Ƿ�Ϊ��ȫ����Ա
                string wfFlag = string.Empty;
                //��ǰ�û�
                Operator curUser = OperatorProvider.Provider.Current();

                IList<UserEntity> ulist = new List<UserEntity>();
                //������Ա
                string participant = string.Empty;

                //�����λ��Ա�ύ��������λ
                if (userbll.HaveRoleListByKey(curUser.UserId, dataitemdetailbll.GetItemValue("EpibolyUser")).Rows.Count > 0)
                {
                    wfFlag = "1";  // �Ǽ�=>I����׼


                    errorMsg = "������λ��Ա";
                    //ȡ������λ ��׼
                    participant = userbll.GetSafetyDeviceDeptUser("3", curUser);
                }
                else  //�����㼶���û� 
                {
                    //��ȫ�������û��ύ
                    if (userbll.GetSafetyAndDeviceDept(curUser).Contains("1"))
                    {
                        //װ�ò���
                        if (userbll.GetSafetyAndDeviceDept(curUser).Contains("2"))
                        {
                            wfFlag = "2";  // �Ǽ�=>����

                            errorMsg = "������";
                            //�����װ���� ���ύ������
                            UserEntity reformUser = userbll.GetEntity(rEntity.REFORMPEOPLEID); //�����û�����
                            //ȡ������
                            participant = reformUser.Account;

                            isAddScore = true;
                        }
                        else  //��װ�ò���
                        {
                            //װ����  ���ύ��װ�ò��ź�׼
                            var lllegatypename = dataitemdetailbll.GetEntity(entity.LLLEGALTYPE).ItemName;
                            //�����ǰѡ�����װ���� ȡװ�õ�λ ���˻�
                            if (lllegatypename == dataitemdetailbll.GetItemValue("ApplianceClass"))
                            {

                                wfFlag = "3";  // �Ǽ�=>II����׼

                                errorMsg = "װ�ò����û�";
                                //ȡװ���û�
                                participant = userbll.GetSafetyDeviceDeptUser("1", curUser);
                            }
                            else  //��װ����
                            {
                                wfFlag = "2";  // �Ǽ�=>����

                                errorMsg = "������";
                                //�����װ���� ���ύ������
                                UserEntity reformUser = userbll.GetEntity(rEntity.REFORMPEOPLEID); //�����û�����
                                //ȡ������
                                participant = reformUser.Account;

                                isAddScore = true;
                            }
                        }

                    }
                    //װ�ò�����Ա
                    else if (userbll.GetSafetyAndDeviceDept(curUser).Contains("2"))
                    {
                        wfFlag = "2";  // �Ǽ�=>����

                        //�����װ���� ���ύ������
                        UserEntity reformUser = userbll.GetEntity(rEntity.REFORMPEOPLEID); //�����û�����


                        errorMsg = "������";
                        //ȡ������
                        participant = reformUser.Account;

                        isAddScore = true;
                    }
                    else  //�ǰ�ȫ�������ύ����ȫ�����ź�׼
                    {

                        //�������ύ�����û���ϱ���ֱ�����ģ���ֱ֮���ύ����ȫ�����ź�׼(���κ�׼)
                        if (userbll.HaveRoleListByKey(curUser.UserId, dataitemdetailbll.GetItemValue("PrincipalUser")).Rows.Count > 0)
                        {
                            //�ϱ�
                            if (entity.ISUPSAFETY == "1")
                            {
                                wfFlag = "3";  // �Ǽ�=>II����׼

                                errorMsg = "��ȫ�������û�";

                                //ȡ��ȫ�������û�
                                participant = userbll.GetSafetyDeviceDeptUser("0", curUser);
                            }
                            else  //���ϱ�
                            {
                                wfFlag = "2";  // �Ǽ�=>����

                                UserEntity reformUser = userbll.GetEntity(rEntity.REFORMPEOPLEID); //�����û�����

                                errorMsg = "������";
                                //ȡ������
                                participant = reformUser.Account;

                                isAddScore = true;
                            }
                        }
                        else  //�ύ�����鸺���˴���׼
                        {
                            wfFlag = "1";  // �Ǽ�=>I����׼

                            errorMsg = "�����Ÿ�����";
                            //ȡ���鸺����
                            participant = userbll.GetSafetyDeviceDeptUser("2", curUser);
                        }
                    }
                }

                ///����û����ֹ���
                if (isAddScore)
                {
                    lllegalpunishbll.SaveUserScore(pbEntity.PERSONINCHARGEID, entity.LLLEGALLEVEL);
                    lllegalpunishbll.SaveUserScore(pbEntity.FIRSTINCHARGEID, entity.LLLEGALLEVEL);
                    lllegalpunishbll.SaveUserScore(pbEntity.SECONDINCHARGEID, entity.LLLEGALLEVEL);
                }


                //�ύ���̵���һ�ڵ�
                if (!string.IsNullOrEmpty(participant))
                {
                    int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);

                    if (count > 0)
                    {
                        htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", keyValue);  //����ҵ������״̬
                    }
                }
                else
                {
                    return Error("����ϵϵͳ����Ա��ȷ��" + errorMsg + "!");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Success("�����ɹ�!");
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
            LllegalPunishEntity pbEntity, LllegalReformEntity rEntity, LllegalAcceptEntity aEntity)
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
            CommonSaveForm(keyValue, "04", entity, pbEntity, rEntity, aEntity);

            ////�����׼��Ϣ
            pEntity.LLLEGALID = entity.ID;
            lllegalapprovebll.SaveForm(ApproveID, pEntity);

            //�������˼�¼
            LllegalPunishEntity punishentity = new LllegalPunishEntity();
            punishentity = pbEntity;
            punishentity.AUTOID = null;
            punishentity.ID = null;
            punishentity.LLLEGALID = entity.ID;
            punishentity.APPROVEID = pEntity.ID;
            punishentity.MARK = "1";
            lllegalpunishbll.SaveForm("", punishentity);

            wfFlag = "1";//���Ľ���

            participant = curUser.Account;

            //�ύ����
            int count = htworkflowbll.SubmitWorkFlow(entity.ID, participant, wfFlag, curUser.UserId);

            if (count > 0)
            {
                lllegalregisterbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", entity.ID);  //����ҵ������״̬
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

            var dtHid = lllegalregisterbll.GetListByCheckId(checkid, curUser.UserId, "Υ�µǼ�");

            string keyValue = string.Empty;

            string reformpeopleid = string.Empty; //������

            string errorMsg = string.Empty;

            bool isAddScore = false; //�Ƿ���ӵ��û�����

            foreach (DataRow row in dtHid.Rows)
            {
                keyValue = row["id"].ToString();

                reformpeopleid = row["reformpeopleid"].ToString();


                LllegalRegisterEntity entity = lllegalregisterbll.GetEntity(keyValue);

                LllegalPunishEntity pbEntity = lllegalpunishbll.GetEntityByBid(keyValue);

                //�˴���Ҫ�жϵ�ǰ���Ƿ�Ϊ��ȫ����Ա
                string wfFlag = string.Empty;

                IList<UserEntity> ulist = new List<UserEntity>();
                //������Ա
                string participant = string.Empty;

                //�����λ��Ա�ύ��������λ
                if (userbll.HaveRoleListByKey(curUser.UserId, dataitemdetailbll.GetItemValue("EpibolyUser")).Rows.Count > 0)
                {
                    wfFlag = "1";  // �Ǽ�=>I����׼

                    errorMsg = "������λ��Ա";
                    //ȡ������λ ��׼
                    participant = userbll.GetSafetyDeviceDeptUser("3", curUser);
                }
                else  //�����㼶���û� 
                {
                    //��ȫ�������û��ύ
                    if (userbll.GetSafetyAndDeviceDept(curUser).Contains("1"))
                    {
                        //װ�ò���
                        if (userbll.GetSafetyAndDeviceDept(curUser).Contains("2"))
                        {
                            wfFlag = "2";  // �Ǽ�=>����

                            errorMsg = "������";
                            //�����װ���� ���ύ������
                            UserEntity reformUser = userbll.GetEntity(reformpeopleid); //�����û�����
                            //ȡ������
                            participant = reformUser.Account;

                            isAddScore = true;
                        }
                        else  //��װ�ò���
                        {
                            //װ����  ���ύ��װ�ò��ź�׼
                            var lllegatypename = dataitemdetailbll.GetEntity(entity.LLLEGALTYPE).ItemName;
                            //�����ǰѡ�����װ���� ȡװ�õ�λ ���˻�
                            if (lllegatypename == dataitemdetailbll.GetItemValue("ApplianceClass"))
                            {

                                wfFlag = "3";  // �Ǽ�=>II����׼

                                errorMsg = "װ�ò����û�";
                                //ȡװ���û�
                                participant = userbll.GetSafetyDeviceDeptUser("1", curUser);
                            }
                            else  //��װ����
                            {
                                wfFlag = "2";  // �Ǽ�=>����

                                errorMsg = "������";
                                //�����װ���� ���ύ������
                                UserEntity reformUser = userbll.GetEntity(reformpeopleid); //�����û�����
                                //ȡ������
                                participant = reformUser.Account;

                                isAddScore = true;
                            }
                        }

                    }
                    //װ�ò�����Ա
                    else if (userbll.GetSafetyAndDeviceDept(curUser).Contains("2"))
                    {
                        wfFlag = "2";  // �Ǽ�=>����

                        //�����װ���� ���ύ������
                        UserEntity reformUser = userbll.GetEntity(reformpeopleid); //�����û�����


                        errorMsg = "������";
                        //ȡ������
                        participant = reformUser.Account;

                        isAddScore = true;
                    }
                    else  //�ǰ�ȫ�������ύ����ȫ�����ź�׼
                    {

                        //�������ύ�����û���ϱ���ֱ�����ģ���ֱ֮���ύ����ȫ�����ź�׼(���κ�׼)
                        if (userbll.HaveRoleListByKey(curUser.UserId, dataitemdetailbll.GetItemValue("PrincipalUser")).Rows.Count > 0)
                        {
                            //�ϱ�
                            if (entity.ISUPSAFETY == "1")
                            {
                                wfFlag = "3";  // �Ǽ�=>II����׼

                                errorMsg = "��ȫ�������û�";

                                //ȡ��ȫ�������û�
                                participant = userbll.GetSafetyDeviceDeptUser("0", curUser);
                            }
                            else  //���ϱ�
                            {
                                wfFlag = "2";  // �Ǽ�=>����

                                UserEntity reformUser = userbll.GetEntity(reformpeopleid); //�����û�����

                                errorMsg = "������";
                                //ȡ������
                                participant = reformUser.Account;

                                isAddScore = true;
                            }
                        }
                        else  //�ύ�����鸺���˴���׼
                        {
                            wfFlag = "1";  // �Ǽ�=>I����׼

                            errorMsg = "�����Ÿ�����";
                            //ȡ���鸺����
                            participant = userbll.GetSafetyDeviceDeptUser("2", curUser);
                        }
                    }
                }

                ///����û����ֹ���
                if (isAddScore)
                {
                    lllegalpunishbll.SaveUserScore(pbEntity.PERSONINCHARGEID, entity.LLLEGALLEVEL);
                    lllegalpunishbll.SaveUserScore(pbEntity.FIRSTINCHARGEID, entity.LLLEGALLEVEL);
                    lllegalpunishbll.SaveUserScore(pbEntity.SECONDINCHARGEID, entity.LLLEGALLEVEL);
                }


                //�ύ���̵���һ�ڵ�
                if (!string.IsNullOrEmpty(participant))
                {
                    int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);

                    if (count > 0)
                    {
                        htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", keyValue);  //����ҵ������״̬
                    }
                }
            }

            return Success("�����ɹ�!");
        }
        #endregion

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

            var userInfo = OperatorProvider.Provider.Current();  //��ȡ��ǰ�û�

            string fileName = "Υ�¿���֪ͨ��_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";

            string strDocPath = Server.MapPath("~/Resource/ExcelTemplate/Υ�¿���֪ͨ������ģ��.doc");

            Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);

            DataTable dt = new DataTable();
            dt.Columns.Add("LllegalYear");
            dt.Columns.Add("LllegalMonth");
            dt.Columns.Add("LllegalDate");
            dt.Columns.Add("LllegalTime");
            dt.Columns.Add("LllegalAddress");
            dt.Columns.Add("LllegalPeople");
            dt.Columns.Add("LllegalDescribe");
            dt.Columns.Add("FirstPerson");
            dt.Columns.Add("FirstMoney");
            dt.Columns.Add("FirstPoint");
            dt.Columns.Add("SecondPerson");
            dt.Columns.Add("SecondMoney");
            dt.Columns.Add("SecondPoint");
            dt.Columns.Add("ThirdPerson");
            dt.Columns.Add("ThirdMoney");
            dt.Columns.Add("ThirdPoint");
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
                        builder.InsertImage(url, Aspose.Words.Drawing.RelativeHorizontalPosition.Margin, 1, Aspose.Words.Drawing.RelativeVerticalPosition.Margin, 320, 50, 50, Aspose.Words.Drawing.WrapType.Inline);
                    }
                }
            }
            row["LllegalYear"] = year;
            row["LllegalMonth"] = month;
            row["LllegalDate"] = date;
            row["LllegalAddress"] = lllegaldt.Rows[0]["lllegaladdress"].ToString();
            row["LllegalPeople"] = lllegaldt.Rows[0]["lllegalperson"].ToString();
            row["LllegalDescribe"] = lllegaldt.Rows[0]["lllegaldescribe"].ToString();
            row["FirstPerson"] = lllegaldt.Rows[0]["personinchargename"].ToString();
            row["FirstMoney"] = !string.IsNullOrEmpty(lllegaldt.Rows[0]["economicspunish"].ToString()) ? lllegaldt.Rows[0]["economicspunish"].ToString() : "0";
            row["FirstPoint"] = !string.IsNullOrEmpty(lllegaldt.Rows[0]["lllegalpoint"].ToString()) ? lllegaldt.Rows[0]["lllegalpoint"].ToString() : "0";
            row["SecondPerson"] = lllegaldt.Rows[0]["firstinchargename"].ToString();
            row["SecondMoney"] = !string.IsNullOrEmpty(lllegaldt.Rows[0]["firsteconomicspunish"].ToString()) ? lllegaldt.Rows[0]["firsteconomicspunish"].ToString() : "0";
            row["SecondPoint"] = !string.IsNullOrEmpty(lllegaldt.Rows[0]["firstlllegalpoint"].ToString()) ? lllegaldt.Rows[0]["firstlllegalpoint"].ToString() : "0";
            row["ThirdPerson"] = lllegaldt.Rows[0]["secondinchargename"].ToString();
            row["ThirdMoney"] = !string.IsNullOrEmpty(lllegaldt.Rows[0]["secondeconomicspunish"].ToString()) ? lllegaldt.Rows[0]["secondeconomicspunish"].ToString() : "0";
            row["ThirdPoint"] = !string.IsNullOrEmpty(lllegaldt.Rows[0]["secondlllegalpoint"].ToString()) ? lllegaldt.Rows[0]["secondlllegalpoint"].ToString() : "0";
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

        #region ��ȡ���ֵ���

        public List<OrganizeEntity> organizedata = new List<OrganizeEntity>();
        public List<DepartmentEntity> departmentdata = new List<DepartmentEntity>();
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
                organizedata = organizeCache.GetList().OrderByDescending(x => x.CreateDate).ToList();
                departmentdata = departmentBLL.GetList().OrderBy(a => a.SortCode).ToList();
            }
            else
            {
                if (user.RoleName.Contains("��˾���û�") || user.RoleName.Contains("���������û�") || user.DeptName.Contains("������"))
                {
                    organizedata = organizeCache.GetList().OrderByDescending(x => x.CreateDate).Where(e => e.EnCode == user.OrganizeCode).ToList();
                    departmentdata = departmentBLL.GetList().OrderBy(a => a.SortCode).Where(e => e.OrganizeId == user.OrganizeId).ToList();
                }
                else
                {
                    departmentdata = departmentBLL.GetList(user.OrganizeId).Where(t => t.EnCode.Contains(user.DeptCode) || t.Description == "������̳а���" || t.SendDeptID == user.DeptId).OrderBy(x => x.SortCode).ToList();

                }
            }
            //�첽��������
            //����������ί��
            MyDelegate dele = new MyDelegate(AddOrg);
            //��Ӳ���
            var result = dele.BeginInvoke(Year, null, null);
            treeList.AddRange(dele.EndInvoke(result));
            dele = new MyDelegate(AddDept);
            result = dele.BeginInvoke(Year, null, null);
            treeList.AddRange(dele.EndInvoke(result));

            var json = treeList.TreeJson(parentId);
            //��������
            return Content(json);

        }

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

        /// <summary>
        /// ��ȡ���ŵ�Υ�´���
        /// </summary>
        /// <param name="DepartmentId"></param>
        /// <param name="Year"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int GetWZNum(string DepartmentId, string Year)
        {
            string sql = "select count(1) as Num from v_LllegalAll2 where DepartmentId='" + DepartmentId + "'";

            if (Year.Length > 0)
            {
                sql += " and to_char(lllegaltime, 'yyyy')='" + Year + "'";

            }
            var dt = hazardsourcebll.FindTableBySql(sql);
            return int.Parse(dt.Rows[0]["Num"].ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DepartmentId"></param>
        /// <returns></returns>
        public decimal GetWZScore(string DepartmentId, string Year)
        {

            string sql = "select lllegallevelname,Id,lllegalpoint from v_LllegalAll where DepartmentId='" + DepartmentId + "'";
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

            var DeptId = Request["DeptId"] ?? "";
            string tWhere = " and DepartmentId='" + DeptId + "'";
            var Year = Request["Year"] ?? "";
            if (Year.Length > 0)
            {
                tWhere += "and  to_char(lllegaltime, 'yyyy')='" + Year + "'";

            }
            queryJson = queryJson ?? "";
            pagination.p_fields = "deptname,realname,wznum,lllegalpoint";
            pagination.p_tablename = @"(select min(deptname) as deptname,realname,count(1) as wznum,sum(lllegalpoint) as lllegalpoint  from v_LllegalAll2 where 1=1 " + tWhere + " group by RealName) t";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

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
                organizedata = organizeCache.GetList().OrderByDescending(x => x.CreateDate).ToList();
                departmentdata = departmentBLL.GetList().OrderBy(a => a.SortCode).ToList();
            }
            else
            {
                if (user.RoleName.Contains("��˾���û�") || user.RoleName.Contains("���������û�") || user.DeptName.Contains("������"))
                {
                    organizedata = organizeCache.GetList().OrderByDescending(x => x.CreateDate).Where(e => e.EnCode == user.OrganizeCode).ToList();
                    departmentdata = departmentBLL.GetList().OrderBy(a => a.SortCode).Where(e => e.EnCode.Substring(0, user.DeptCode.Length) == user.DeptCode).ToList();
                }
                else
                {
                    departmentdata = departmentBLL.GetList().OrderBy(a => a.SortCode).Where(e => e.EnCode.Length >= user.DeptCode.Length && e.EnCode.Substring(0, user.DeptCode.Length) == user.DeptCode).ToList();
                    parentId = user.ParentId;
                }
            }
            var treeList = new List<TreeGridEntity>();
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
                var Ids = "";
                itemJson = itemJson.Insert(1, "\"DepartWZNum\":\"" + GetWZNum(item.OrganizeId, Year) + "\",");

                itemJson = itemJson.Insert(1, "\"DepartWZScore\":\"" + GetWZScore(item.OrganizeId, Year) + "\",");
                itemJson = itemJson.Insert(1, "\"Ids\":\"" + Ids + "\",");
                tree.entityJson = itemJson;
                treeList.Add(tree);
            }
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
    }
}
