using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using System;

namespace ERCHTMS.Web.Areas.HiddenTroubleManageBz.Controllers
{
    /// <summary>
    /// �� ��������������
    /// </summary>
    public class HTEstimateController : MvcControllerBase
    {
        private HTEstimateBLL htestimatebll = new HTEstimateBLL();
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL();
        private HTChangeInfoBLL htchangeinfobll = new HTChangeInfoBLL();
        private HTAcceptInfoBLL htacceptinfobll = new HTAcceptInfoBLL();
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
        public ActionResult GetListJson(string queryJson)
        {
            var data = htestimatebll.GetList(queryJson);
            return ToJsonResult(data);
        }


        [HttpGet]
        public ActionResult GetHistoryListJson(string keyCode)
        {
            var data = htestimatebll.GetHistoryList(keyCode);
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
            var data = htestimatebll.GetEntity(keyValue);
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
        [HandlerMonitor(6, "ɾ����������Ч��������Ϣ")]
        public ActionResult RemoveForm(string keyValue)
        {
            htestimatebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, HTAcceptInfoEntity atEntity, HTChangeInfoEntity chEntity, HTEstimateEntity entity)
        {
            Operator curUser = OperatorProvider.Provider.Current();
            string ACCEPTID = Request.Form["ACCEPTID"] != null ? Request.Form["ACCEPTID"].ToString() : ""; //����ID
            string ESTIMATEID = Request.Form["ESTIMATEID"] != null ? Request.Form["ESTIMATEID"].ToString() : ""; //����Ч������ID
            string ESTIMATERESULT = Request.Form["ESTIMATERESULT"] != null ? Request.Form["ESTIMATERESULT"].ToString() : ""; //�������
            string CHANGEID = Request.Form["CHANGEID"] != null ? Request.Form["CHANGEID"].ToString() : ""; //����ID
            string participant = string.Empty;  //��ȡ������һ�ڵ�Ĳ�����Ա
            string wfFlag = string.Empty; //���̱�ʶ
            //�����ϸ�
            if (ESTIMATERESULT == "1")
            {
                wfFlag = "3";

                //ȡ��ǰ��
                participant = curUser.Account;

            }
            else
            {
                wfFlag = "2";

                //ȡ������
                UserEntity auser = userbll.GetEntity(chEntity.CHANGEPERSON);

                participant = auser.Account;

            }

            //�˻غ������������ļ�¼������Ч��������¼
            if (wfFlag == "2")
            {
                //���ļ�¼
                HTChangeInfoEntity cEntity = htchangeinfobll.GetEntity(CHANGEID);
                HTChangeInfoEntity newEntity = new HTChangeInfoEntity(); 
                newEntity = cEntity;
                newEntity.CREATEDATE = DateTime.Now;
                newEntity.MODIFYDATE = DateTime.Now;
                newEntity.ID = null;
                newEntity.AUTOID = cEntity.AUTOID + 1;
                htchangeinfobll.SaveForm("", newEntity);

                HTAcceptInfoEntity htacceptinfoentity = htacceptinfobll.GetEntityByHidCode(cEntity.HIDCODE);
                //���ռ�¼
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

            //��������Ч������
            if (!string.IsNullOrEmpty(ESTIMATEID))
            {
                var tempEntity = htestimatebll.GetEntity(ESTIMATEID);
                entity.AUTOID = tempEntity.AUTOID;
            }
            htestimatebll.SaveForm(ESTIMATEID, entity);

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
