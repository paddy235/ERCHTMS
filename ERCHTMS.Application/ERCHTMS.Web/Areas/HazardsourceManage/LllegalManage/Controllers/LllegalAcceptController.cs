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
            try
            {

                /*
                 �����������  
                 * �����ղ�ͨ�����������˻ص�Υ������״̬���������������� ��������Ҫע�⣬һ����ͨ��������������һ��������Ϣ��һ��������Ϣ.
                 * ������ͨ��,�����Ľ���
                 */

                /*
                   ����  ���յ����Ľ���
                */
                string wfFlag = string.Empty;  //���̱�ʶ

                string participant = string.Empty;  //��ȡ������һ�ڵ�Ĳ�����Ա (ȡ������)


                LllegalAcceptEntity aptEntity = lllegalacceptbll.GetEntityByBid(keyValue);
                aptEntity.ACCEPTRESULT = acceptEntity.ACCEPTRESULT;
                aptEntity.ACCEPTMIND = acceptEntity.ACCEPTMIND;
                aptEntity.MODIFYDATE = DateTime.Now;
                aptEntity.MODIFYUSERID = curUser.UserId;
                aptEntity.MODIFYUSERNAME = curUser.UserName;
                aptEntity.ACCEPTPIC = acceptEntity.ACCEPTPIC;
                lllegalacceptbll.SaveForm(aptEntity.ID, aptEntity);

                //��ͨ��
                if (acceptEntity.ACCEPTRESULT == "0")
                {
                    //���ļ�¼
                    LllegalReformEntity reformEntity = lllegalreformbll.GetEntityByBid(keyValue);
                    LllegalReformEntity newEntity = new LllegalReformEntity();
                    newEntity = reformEntity;
                    newEntity.CREATEDATE = DateTime.Now;
                    newEntity.MODIFYDATE = DateTime.Now;
                    newEntity.MODIFYUSERID = curUser.UserId;
                    newEntity.MODIFYUSERNAME = curUser.UserName;
                    newEntity.REFORMPIC = null; //��������ͼƬGUID
                    newEntity.REFORMSTATUS = null; //����������
                    newEntity.REFORMMEASURE = null; //���ľ����ʩ
                    newEntity.REFORMFINISHDATE = null; //�������ʱ��
                    newEntity.ID = "";
                    lllegalreformbll.SaveForm("", newEntity);

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


                    wfFlag = "2";  // Υ������

                    UserEntity reformUser = userbll.GetEntity(reformEntity.REFORMPEOPLEID); //�����û�����
                    //ȡ������
                    participant = reformUser.Account;
                }
                else  //ͨ���������
                {
                    wfFlag = "1";  // ���Ľ���

                    participant = curUser.Account;
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
                    return Error("����ϵϵͳ����Ա��ȷ���ύ����!");
                }
            }
            catch (Exception)
            {
                throw;
            }

            return Success("�����ɹ�!");
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
