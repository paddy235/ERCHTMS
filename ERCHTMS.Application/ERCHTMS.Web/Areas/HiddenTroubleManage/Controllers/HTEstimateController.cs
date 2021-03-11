using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using System;

namespace ERCHTMS.Web.Areas.HiddenTroubleManage.Controllers
{
    /// <summary>
    /// �� ������������Ч��������
    /// </summary>
    public class HTEstimateController : MvcControllerBase
    {
        private HTEstimateBLL htestimatebll = new HTEstimateBLL();
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL();
        private HTChangeInfoBLL htchangeinfobll = new HTChangeInfoBLL();
        private HTAcceptInfoBLL htacceptinfobll = new HTAcceptInfoBLL();
        private UserBLL userbll = new UserBLL();
        private HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL();
        private WfControlBLL wfcontrolbll = new WfControlBLL();//�Զ������̷���

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


            var baseEntity = htbaseinfobll.GetEntity(keyValue);
            WfControlObj wfentity = new WfControlObj();
            wfentity.businessid = keyValue;
            wfentity.startflow = baseEntity.WORKSTREAM;
            wfentity.rankid = baseEntity.HIDRANK;
            wfentity.user = curUser;
            wfentity.organizeid = baseEntity.HIDDEPART; //��Ӧ�糧id
            //ʡ���Ǽǵ�
            if (baseEntity.ADDTYPE == "2")
            {
                wfentity.mark = "ʡ�������Ų�";
            }
            else //����
            {
                wfentity.mark = "���������Ų�";
            }
            //�����ϸ�
            if (ESTIMATERESULT == "1")
            {
                wfentity.submittype = "�ύ";
            }
            else //������ͨ��
            {
                wfentity.submittype = "�˻�";
            }

            //��ȡ��һ���̵Ĳ�����
            WfControlResult result = wfcontrolbll.GetWfControl(wfentity);

            //���ز�������ɹ�
            if (result.code == WfCode.Sucess)
            {
                participant = result.actionperson;

                wfFlag = result.wfflag;

                if (!string.IsNullOrEmpty(participant))
                {
                    //�ύ����
                    int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);

                    if (count > 0)
                    {
                        htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //����ҵ������״̬

                        //��������Ч������
                        if (!string.IsNullOrEmpty(ESTIMATEID))
                        {
                            var tempEntity = htestimatebll.GetEntity(ESTIMATEID);
                            entity.AUTOID = tempEntity.AUTOID;
                        }
                        htestimatebll.SaveForm(ESTIMATEID, entity);

                        //�˻غ������������ļ�¼������Ч��������¼
                        if (wfentity.submittype == "�˻�")
                        {
                            string tagName = htworkflowbll.QueryTagNameByCurrentWF(keyValue);

                            if (tagName == "��������")
                            {
                                //���ļ�¼
                                HTChangeInfoEntity cEntity = htchangeinfobll.GetEntity(CHANGEID);
                                HTChangeInfoEntity newEntity = new HTChangeInfoEntity();
                                newEntity = cEntity;
                                newEntity.CREATEDATE = DateTime.Now;
                                newEntity.MODIFYDATE = DateTime.Now;
                                newEntity.ID = null;
                                newEntity.AUTOID = cEntity.AUTOID + 1;
                                newEntity.CHANGERESUME = null;
                                newEntity.CHANGEFINISHDATE = null;
                                newEntity.REALITYMANAGECAPITAL = 0;
                                newEntity.ATTACHMENT = Guid.NewGuid().ToString(); //���ĸ���
                                newEntity.HIDCHANGEPHOTO = Guid.NewGuid().ToString(); //����ͼƬ
                                newEntity.APPSIGN = "Web";
                                htchangeinfobll.SaveForm("", newEntity);
                            }

                            HTAcceptInfoEntity htacceptinfoentity = htacceptinfobll.GetEntityByHidCode(baseEntity.HIDCODE);
                            //���ռ�¼
                            HTAcceptInfoEntity accptEntity = new HTAcceptInfoEntity();
                            accptEntity = htacceptinfoentity;
                            accptEntity.ID = null;
                            accptEntity.AUTOID = htacceptinfoentity.AUTOID + 1;
                            accptEntity.CREATEDATE = DateTime.Now;
                            accptEntity.MODIFYDATE = DateTime.Now;
                            accptEntity.ACCEPTSTATUS = null;
                            accptEntity.ACCEPTIDEA = null;
                            accptEntity.ACCEPTPHOTO = Guid.NewGuid().ToString(); //����ͼƬ
                            accptEntity.APPSIGN = "Web";
                            htacceptinfobll.SaveForm("", accptEntity);
                        }
                    }
                }
                else
                {
                    return Error("Ŀ�����̲�����δ����");
                }
                return Success(result.message);
            }
            else
            {
                return Error(result.message);
            }
        }
        #endregion
    }
}
