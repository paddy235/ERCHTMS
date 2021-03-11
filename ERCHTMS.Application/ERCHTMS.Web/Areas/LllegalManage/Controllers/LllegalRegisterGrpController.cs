using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.HazardsourceManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.LllegalManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Cache;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.LllegalManage;
using ERCHTMS.Entity.SystemManage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace ERCHTMS.Web.Areas.LllegalManage.Controllers
{
    /// <summary>
    /// �� �������š�ʡ��Υ�»�����Ϣ��
    /// </summary>
    public class LllegalRegisterGrpController : MvcControllerBase
    {
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL(); //����ҵ�����
        private UserBLL userbll = new UserBLL(); //�û���������
        private LllegalRegisterBLL lllegalregisterbll = new LllegalRegisterBLL(); // Υ�»�����Ϣ
        private LllegalReformBLL lllegalreformbll = new LllegalReformBLL(); //������Ϣ����
        private LllegalApproveBLL lllegalapprovebll = new LllegalApproveBLL(); //��׼��Ϣ����
        private LllegalAcceptBLL lllegalacceptbll = new LllegalAcceptBLL(); //������Ϣ����
        private LllegalPunishBLL lllegalpunishbll = new LllegalPunishBLL(); // ������Ϣ����
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
      
        #region ��ͼ
        /// <summary>
        /// �б�ҳ��  ������ҳ��ʹ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// �б�ҳ��  ������ҳ��ʹ��(ʡ��˾)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SdIndex()
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

        #region Э������
        /// <summary>
        /// ��ȡ������λ��ʡ��˾�����糧���Ժ����ơ���
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetLllegalDepartListJson()
        {
            Operator opertator = new OperatorProvider().Current();
            var data = new DepartmentBLL().GetList().Where(t => t.DeptCode.StartsWith(opertator.NewDeptCode) && (t.Nature == "�糧" || t.Nature == "����"));
            var listDept = data.Select(x => { return new { DeptId = x.DepartmentId, DeptName = x.FullName }; });
            return Content(listDept.ToJson());
        }
        /// <summary>
        /// ��ȡ������λ��ʡ��˾�������糧���Ժ����ơ���
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetLllegalDepartListJsonGrp()
        {
            Operator opertator = new OperatorProvider().Current();
            var data = new DepartmentBLL().GetList().Where(t => t.DeptCode.StartsWith(opertator.OrganizeCode) && (t.Nature == "����" || t.Nature == "���ӹ�˾" || t.Nature == "�糧" || t.Nature == "����"));
            var listDept = data.OrderBy(x=>x.DeptCode).Select(x => { return new { DeptId = x.DepartmentId, DeptName = x.FullName }; });           
            return Content(listDept.ToJson());
        }
        #endregion

        #region  ��������������޸ģ�
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">Υ�»�����Ϣ</param>
        /// <param name="pbEntity">���˼�¼</param>
        /// <param name="rEntity">������Ϣ</param>
        /// <param name="aEntity">������Ϣ</param>
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
        /// <param name="workFlow">����������</param>
        /// <param name="entity">Υ�»�����Ϣ</param>
        /// <param name="pbEntity">���˼�¼</param>
        /// <param name="rEntity">������Ϣ</param>
        /// <param name="aEntity">������Ϣ</param>
        public void CommonSaveForm(string keyValue, string workFlow, LllegalRegisterEntity entity, LllegalPunishEntity pbEntity, LllegalReformEntity rEntity, LllegalAcceptEntity aEntity)
        {
            //�ύͨ��
            string userId = OperatorProvider.Provider.Current().UserId;

            //����Υ�»�����Ϣ
            entity.RESEVERFOUR = "";
            entity.RESEVERFIVE = "";
            if (string.IsNullOrEmpty(keyValue))
            {
                string lenNum = !string.IsNullOrEmpty(dataitemdetailbll.GetItemValue("LllegalSerialNumberLen")) ? dataitemdetailbll.GetItemValue("LllegalSerialNumberLen") : "3";

                entity.LLLEGALNUMBER = lllegalregisterbll.GenerateHidCode("bis_lllegalregister", "lllegalnumber", int.Parse(lenNum));
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

        #region �ύ���̣�ͬʱ�������޸�Υ����Ϣ�����������̡���
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>       
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">Υ�»�����Ϣ</param>
        /// <param name="pbEntity">���˼�¼</param>
        /// <param name="rEntity">������Ϣ</param>
        /// <param name="aEntity">������Ϣ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, LllegalRegisterEntity entity, LllegalPunishEntity pbEntity,
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
            string errorMsg = string.Empty;

            //bool isAddScore = false; //�Ƿ���ӵ��û�����

            string startflow = string.Empty;//��ʼ
            string endflow = string.Empty;//��ֹ

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
                var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'FlowState'");
                string startnode = itemlist.Where(p => p.ItemName == "Υ�µǼ�").Count() > 0 ? "Υ�µǼ�" : "Υ�¾ٱ�";
                //ʡ��˾��ʡ���û�
                if (userbll.HaveRoleListByKey(curUser.UserId, dataitemdetailbll.GetItemValue("GrpUser")).Rows.Count > 0)
                {
                    startflow = startnode;
                    endflow = "Υ������";
                    wfFlag = "4";  // �Ǽ�=>����
                    errorMsg = "ʡ��˾��ʡ���û�";
                    //ȡ��ȫ���ܲ����û� ����
                    participant = userbll.GetSafetyDeviceDeptUser("0", entity.BELONGDEPARTID);

                }                

                //����û����ֹ���
                //if (isAddScore)
                //{
                //    lllegalpunishbll.SaveUserScore(pbEntity.PERSONINCHARGEID, entity.LLLEGALLEVEL);
                //    //����������
                //    var relevanceList = lllegalpunishbll.GetListByLllegalId(entity.ID, "1");
                //    foreach (LllegalPunishEntity lpEntity in relevanceList)
                //    {
                //        //Υ��������
                //        lllegalpunishbll.SaveUserScore(lpEntity.PERSONINCHARGEID, entity.LLLEGALLEVEL);
                //    }
                //}
                
                if (!string.IsNullOrEmpty(participant))
                {
                    int count = htworkflowbll.SubmitWorkFlow(startflow,endflow, keyValue, participant, wfFlag, curUser.UserId);

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
    }         
}
