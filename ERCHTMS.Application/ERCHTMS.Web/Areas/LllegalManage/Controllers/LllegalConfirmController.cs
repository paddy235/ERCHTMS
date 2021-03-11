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
using ERCHTMS.Busines.LllegalManage;
using ERCHTMS.Entity.LllegalManage;

namespace ERCHTMS.Web.Areas.LllegalManage.Controllers
{
    /// <summary>
    /// �� ����Υ������ȷ�ϣ�ʡ��˾����Ϣ��
    /// </summary>
    public class LllegalConfirmController : MvcControllerBase
    {
        private UserBLL userbll = new UserBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private LllegalRegisterBLL lllegalregisterbll = new LllegalRegisterBLL(); // Υ�»�����Ϣ
        private LllegalReformBLL lllegalreformbll = new LllegalReformBLL(); //������Ϣ����
        private LllegalApproveBLL lllegalapprovebll = new LllegalApproveBLL(); //��׼��Ϣ����
        private LllegalAcceptBLL lllegalacceptbll = new LllegalAcceptBLL(); //������Ϣ����
        private LllegalConfirmBLL lllegalconfirmbll = new LllegalConfirmBLL(); //����ȷ����Ϣ����
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL();

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

        /// <summary>
        /// �б�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DetailList()
        {
            return View();
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detail()
        {
            return View();
        }
        #endregion

        #region �ύ����ȷ����������
        /// <summary>
        /// �ύ����ȷ����������
        /// </summary>
        /// <param name="keyValue">Υ�±��</param>
        /// <param name="ConfirmEntity">����ȷ��ʵ��</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, LllegalConfirmEntity ConfirmEntity)
        {
            Operator curUser = OperatorProvider.Provider.Current();

            WfControlResult result = new WfControlResult();
            try
            {
                string wfFlag = string.Empty;  //���̱�ʶ

                string participant = string.Empty;  //��ȡ������һ�ڵ�Ĳ�����Ա (ȡ����ȷ����)

                LllegalRegisterEntity baseEntity = lllegalregisterbll.GetEntity(keyValue);

                LllegalConfirmEntity aptEntity = lllegalconfirmbll.GetEntityByBid(keyValue);
                if (aptEntity == null)
                {
                    aptEntity = ConfirmEntity;
                    aptEntity.LLLEGALID = keyValue;
                }
                aptEntity.CONFIRMRESULT = ConfirmEntity.CONFIRMRESULT;
                aptEntity.CONFIRMMIND = ConfirmEntity.CONFIRMMIND;
                aptEntity.MODIFYDATE = DateTime.Now;
                aptEntity.MODIFYUSERID = curUser.UserId;
                aptEntity.MODIFYUSERNAME = curUser.UserName;             


                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = keyValue;
                wfentity.argument1 = baseEntity.MAJORCLASSIFY; //רҵ����
                wfentity.startflow = baseEntity.FLOWSTATE;
                wfentity.rankid = null;
                wfentity.user = curUser;
                wfentity.organizeid = baseEntity.BELONGDEPARTID; //��Ӧ�糧id
                //ʡ���Ǽǵ�
                if (baseEntity.ADDTYPE == "2")
                {
                    wfentity.mark = "ʡ��Υ������";
                }
                else
                {
                    wfentity.mark = "����Υ������";
                }
                //����ͨ��
                if (ConfirmEntity.CONFIRMRESULT == "1")
                {
                    wfentity.submittype = "�ύ";
                }
                else //���ղ�ͨ��
                {
                    wfentity.submittype = "�˻�";
                }
                 //��ȡ��һ���̵Ĳ�����
                result = wfcontrolbll.GetWfControl(wfentity); 
            
                //���ز�������ɹ�
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
                            //�������ȷ����Ϣ
                            lllegalconfirmbll.SaveForm(aptEntity.ID, aptEntity);

                            //�˻��������������ȷ�ϼ�¼
                            if (wfentity.submittype == "�˻�")
                            {
                                string tagName = htworkflowbll.QueryTagNameByCurrentWF(keyValue);
                                if (tagName == "Υ������")
                                {
                                    //���ļ�¼
                                    LllegalReformEntity reformEntity = lllegalreformbll.GetEntityByBid(keyValue);
                                    LllegalReformEntity newEntity = new LllegalReformEntity();
                                    newEntity = reformEntity;
                                    newEntity.CREATEDATE = DateTime.Now;
                                    newEntity.MODIFYDATE = DateTime.Now;
                                    newEntity.MODIFYUSERID = curUser.UserId;
                                    newEntity.MODIFYUSERNAME = curUser.UserName;
                                    newEntity.REFORMPIC = string.Empty; //��������ͼƬGUID
                                    newEntity.REFORMATTACHMENT = string.Empty; //���ĸ���
                                    newEntity.REFORMSTATUS = string.Empty; //����������
                                    newEntity.REFORMMEASURE = string.Empty; //���ľ����ʩ
                                    newEntity.REFORMFINISHDATE = null; //�������ʱ��
                                    newEntity.ID = "";
                                    lllegalreformbll.SaveForm("", newEntity);
                                }
                                //���ռ�¼
                                LllegalAcceptEntity acceptEntity = lllegalacceptbll.GetEntityByBid(keyValue); 
                                LllegalAcceptEntity newacceptEntity = new LllegalAcceptEntity();
                                newacceptEntity = acceptEntity;
                                newacceptEntity.ID = null;
                                newacceptEntity.CREATEDATE = DateTime.Now;
                                newacceptEntity.MODIFYDATE = DateTime.Now;
                                newacceptEntity.ACCEPTRESULT = null;
                                newacceptEntity.ACCEPTMIND = null;
                                newacceptEntity.ACCEPTPIC = null;
                                lllegalacceptbll.SaveForm("", newacceptEntity);

                                //����ȷ�ϼ�¼
                                LllegalConfirmEntity cptEntity = new LllegalConfirmEntity();
                                cptEntity = aptEntity;
                                cptEntity.ID = null;
                                cptEntity.CREATEDATE = DateTime.Now;
                                cptEntity.MODIFYDATE = DateTime.Now;
                                cptEntity.CONFIRMRESULT = null;
                                cptEntity.CONFIRMMIND = null;
                                lllegalconfirmbll.SaveForm("", cptEntity);
                            }
                        }
                    }
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
            return Success((result.message));
        }
        #endregion

        #region ��ȡ����ȷ����ʷ��¼
        /// <summary>
        /// ��ȡ����ȷ����ʷ��¼
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetHistoryListJson(string keyValue)
        {
            var data = lllegalconfirmbll.GetHistoryList(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region ��ȡ����ȷ�������¼
        /// <summary>
        /// ��ȡ����ȷ�������¼
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetEntityJson(string keyValue)   
        {
            var data = lllegalconfirmbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion
    }
}
