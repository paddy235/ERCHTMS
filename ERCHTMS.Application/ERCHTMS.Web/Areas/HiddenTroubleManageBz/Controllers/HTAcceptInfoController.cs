using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
using System;
using ERCHTMS.Busines.SystemManage;

namespace ERCHTMS.Web.Areas.HiddenTroubleManageBz.Controllers 
{
    /// <summary>
    /// �� ��������������Ϣ��
    /// </summary>
    public class HTAcceptInfoController : MvcControllerBase
    {
        private HTAcceptInfoBLL htacceptinfobll = new HTAcceptInfoBLL();
        private HTApprovalBLL htapprovalbll = new HTApprovalBLL();
        private UserBLL userbll = new UserBLL();
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL();
        private HTEstimateBLL htestimatebll = new HTEstimateBLL();
        private HTChangeInfoBLL htchangeinfobll = new HTChangeInfoBLL();
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
        public ActionResult DetailList()
        {
            return View();
        }
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detail() 
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
        public ActionResult GetListJson(string hideCode) 
        {
            var data = htacceptinfobll.GetList(hideCode);
            return ToJsonResult(data);
        }


        [HttpGet]
        public ActionResult GetHistoryListJson(string keyCode)
        {
            var data = htacceptinfobll.GetHistoryList(keyCode);
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
            var data = htacceptinfobll.GetEntity(keyValue);
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
        [HandlerMonitor(6, "ɾ������������Ϣ")]
        public ActionResult RemoveForm(string keyValue)
        {
            htacceptinfobll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, HTBaseInfoEntity bentity, HTAcceptInfoEntity entity, HTChangeInfoEntity centity)
        {
            Operator curUser = OperatorProvider.Provider.Current();
            string ACCEPTID = Request.Form["ACCEPTID"] != null ? Request.Form["ACCEPTID"].ToString() : ""; //����ID
            string ACCEPTSTATUS = Request.Form["ACCEPTSTATUS"] != null ? Request.Form["ACCEPTSTATUS"].ToString() : ""; //�������
            string CHANGEID = Request.Form["CHANGEID"] != null ? Request.Form["CHANGEID"].ToString() : ""; //����ID
            string participant = string.Empty;  //��ȡ������һ�ڵ�Ĳ�����Ա
            string wfFlag = string.Empty; //���̱�ʶ

            string IIMajorRisks = dataitemdetailbll.GetItemValue("IIMajorRisks"); //II���ش�����

            string IMajorRisks = dataitemdetailbll.GetItemValue("IMajorRisks"); //I���ش�����
            //����ͨ��
            if (ACCEPTSTATUS == "1")
            {
                participant = curUser.Account;

                //�ش����������ύ��������������
                if (bentity.HIDRANK == IMajorRisks || bentity.HIDRANK == IIMajorRisks)
                {
                    wfFlag = "2";

                }
                else  //һ��������ֱ�����Ľ��� 
                {
                    wfFlag = "4";
                }
            }
            else //���ղ�ͨ��
            {
                //�˻ص�����
                UserEntity auser = userbll.GetEntity(centity.CHANGEPERSON);
                //�˻ص�����
                participant = auser.Account;

                wfFlag = "3";
            }

            if (!string.IsNullOrEmpty(ACCEPTID))
            {
                var tempEntity = htacceptinfobll.GetEntity(ACCEPTID);
                entity.AUTOID = tempEntity.AUTOID;
            }
            htacceptinfobll.SaveForm(ACCEPTID, entity);

            //�˻�������������ռ�¼
            if (wfFlag == "3") 
            {
                //���ļ�¼
                HTChangeInfoEntity chEntity = htchangeinfobll.GetEntity(CHANGEID);
                HTChangeInfoEntity newEntity = new HTChangeInfoEntity();
                newEntity = chEntity;
                newEntity.CREATEDATE = DateTime.Now;
                newEntity.MODIFYDATE = DateTime.Now;
                newEntity.ID = null;
                htchangeinfobll.SaveForm("", newEntity);

                //���ռ�¼
                HTAcceptInfoEntity htacceptinfoentity = htacceptinfobll.GetEntityByHidCode(bentity.HIDCODE);
                HTAcceptInfoEntity accptEntity = new HTAcceptInfoEntity();
                accptEntity = htacceptinfoentity;
                accptEntity.ID = null;
                accptEntity.CREATEDATE = DateTime.Now;
                accptEntity.MODIFYDATE = DateTime.Now;
                accptEntity.ACCEPTSTATUS = null;
                accptEntity.ACCEPTIDEA = null;
                accptEntity.ACCEPTPHOTO = null;
                htacceptinfobll.SaveForm("", accptEntity);
            }

            //�����ش����������ύ������ͨ��,�򴴽�����������������
            if ((bentity.HIDRANK == IMajorRisks && wfFlag == "2") || (bentity.HIDRANK == IIMajorRisks && wfFlag == "2"))
            {
               
                //�ش������´����µ���������
                HTEstimateEntity esEntity = new HTEstimateEntity();
                esEntity.HIDCODE = bentity.HIDCODE;
                esEntity.ESTIMATEPERSON = entity.ACCEPTPERSON;
                esEntity.ESTIMATEPERSONNAME = entity.ACCEPTPERSONNAME;
                esEntity.ESTIMATEDEPARTCODE = entity.ACCEPTDEPARTCODE;
                esEntity.ESTIMATEDEPARTNAME = entity.ACCEPTDEPARTNAME;
                //��������
                htestimatebll.SaveForm("", esEntity);  //���
            }
            //�ύ����
            int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);

            if (count > 0)
            {
                htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //����ҵ������״̬
            }

            return Success("�����ɹ���");
        }
        #endregion
    }
}
