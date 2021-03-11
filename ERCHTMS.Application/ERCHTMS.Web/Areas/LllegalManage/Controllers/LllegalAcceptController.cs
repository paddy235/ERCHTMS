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
    /// �� ����Υ��������Ϣ��
    /// </summary>
    public class LllegalAcceptController : MvcControllerBase
    {
        private UserBLL userbll = new UserBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private LllegalRegisterBLL lllegalregisterbll = new LllegalRegisterBLL(); // Υ�»�����Ϣ
        private LllegalReformBLL lllegalreformbll = new LllegalReformBLL(); //������Ϣ����
        private LllegalApproveBLL lllegalapprovebll = new LllegalApproveBLL(); //��׼��Ϣ����
        private LllegalAcceptBLL lllegalacceptbll = new LllegalAcceptBLL(); //������Ϣ����
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
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeliverForm()
        {
            return View();
        }
        #endregion

        #region �ύ������������
        /// <summary>
        /// �ύ������������
        /// </summary>
        /// <param name="isUpSubmit"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, LllegalAcceptEntity acceptEntity)
        {
            Operator curUser = OperatorProvider.Provider.Current();
            WfControlResult result = new WfControlResult();
            try
            {

                /*
                 * �����������  
                 * �����ղ�ͨ�����������˻ص�Υ������״̬���������������� ��������Ҫע�⣬һ����ͨ��������������һ��������Ϣ��һ��������Ϣ.
                 * ������ͨ��,�����Ľ���
                 */
                string wfFlag = string.Empty;  //���̱�ʶ

                string participant = string.Empty;  //��ȡ������һ�ڵ�Ĳ�����Ա (ȡ������)

                LllegalRegisterEntity baseEntity = lllegalregisterbll.GetEntity(keyValue);

                LllegalAcceptEntity aptEntity = lllegalacceptbll.GetEntityByBid(keyValue);
                aptEntity.ACCEPTRESULT = acceptEntity.ACCEPTRESULT;
                aptEntity.ACCEPTMIND = acceptEntity.ACCEPTMIND;
                aptEntity.MODIFYDATE = DateTime.Now;
                aptEntity.MODIFYUSERID = curUser.UserId;
                aptEntity.MODIFYUSERNAME = curUser.UserName;
                aptEntity.ACCEPTPIC = acceptEntity.ACCEPTPIC;
                aptEntity.ACCEPTDEPTCODE = acceptEntity.ACCEPTDEPTCODE;
                aptEntity.ACCEPTDEPTNAME = acceptEntity.ACCEPTDEPTNAME;
                aptEntity.ACCEPTPEOPLE = acceptEntity.ACCEPTPEOPLE;
                aptEntity.ACCEPTPEOPLEID = acceptEntity.ACCEPTPEOPLEID;
                aptEntity.ACCEPTTIME = acceptEntity.ACCEPTTIME;

                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = keyValue;
                wfentity.argument1 = baseEntity.MAJORCLASSIFY; //רҵ����
                wfentity.startflow = baseEntity.FLOWSTATE;
                wfentity.rankid =null;
                wfentity.user = curUser;
                wfentity.organizeid = baseEntity.BELONGDEPARTID; //��Ӧ�糧id
                 //����
                if (baseEntity.ADDTYPE == "2")
                {
                    wfentity.mark = "ʡ��Υ������";
                }
                else
                {
                    wfentity.mark = "����Υ������";
                }
                //����ͨ��
                if (acceptEntity.ACCEPTRESULT == "1")
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
                            //���Υ�����ռ�¼
                            lllegalacceptbll.SaveForm(aptEntity.ID, aptEntity);

                            htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", keyValue);  //����ҵ������״̬

                             //�˻�������������ռ�¼
                            if (wfentity.submittype == "�˻�")
                            {
                                string tagName = htworkflowbll.QueryTagNameByCurrentWF(keyValue);
                                if (tagName == "Υ������")
                                {
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
                                LllegalAcceptEntity cptEntity = new LllegalAcceptEntity();
                                cptEntity = aptEntity;
                                cptEntity.ID = null;
                                cptEntity.CREATEDATE = DateTime.Now;
                                cptEntity.MODIFYDATE = DateTime.Now;
                                cptEntity.ACCEPTRESULT = null;
                                cptEntity.ACCEPTMIND = null;
                                cptEntity.ACCEPTPIC = null;
                                lllegalacceptbll.SaveForm("", cptEntity);
                            }
                         
                        }
                    }
                    else
                    {
                        return Error(result.message);
                    }
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
            return Success((result.message));
        }
        #endregion

        #region ��ȡ������ʷ��¼
        /// <summary>
        /// ��ȡ������ʷ��¼
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetHistoryListJson(string keyValue)
        {
            var data = lllegalacceptbll.GetHistoryList(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region ��ȡ���������¼
        /// <summary>
        /// ��ȡ���������¼
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetEntityJson(string keyValue)   
        {
            var data = lllegalacceptbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion
    }
}
