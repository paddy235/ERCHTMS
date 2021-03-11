using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Entity.BaseManage;
using System.Data;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Code;
using System.Collections.Generic;
using System;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.LllegalManage;
using ERCHTMS.Entity.LllegalManage;

namespace ERCHTMS.Web.Areas.LllegalManage.Controllers
{
    #region Υ��������Ϣ��
    /// <summary>
    /// �� ����Υ��������Ϣ��
    /// </summary>
    public class LllegalReformController : MvcControllerBase
    {

        private UserBLL userbll = new UserBLL();
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private LllegalRegisterBLL lllegalregisterbll = new LllegalRegisterBLL(); // Υ�»�����Ϣ
        private LllegalReformBLL lllegalreformbll = new LllegalReformBLL(); //������Ϣ����
        private LllegalApproveBLL lllegalapprovebll = new LllegalApproveBLL(); //��׼��Ϣ����
        private LllegalAcceptBLL lllegalacceptbll = new LllegalAcceptBLL(); //������Ϣ����

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

        #region �������Ϣ
        /// <summary>
        /// �������Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, LllegalReformEntity entity)
        {
            Operator curUser = OperatorProvider.Provider.Current();
            LllegalReformEntity rEntity = lllegalreformbll.GetEntityByBid(keyValue);
            rEntity.REFORMFINISHDATE = entity.REFORMFINISHDATE;
            rEntity.REFORMMEASURE = entity.REFORMMEASURE;
            rEntity.REFORMSTATUS = entity.REFORMSTATUS;
            rEntity.MODIFYDATE = DateTime.Now;
            rEntity.MODIFYUSERID = curUser.UserId;
            rEntity.MODIFYUSERNAME = curUser.UserName;
            rEntity.REFORMPIC = entity.REFORMPIC;
            lllegalreformbll.SaveForm(rEntity.ID, rEntity);
            return Success("�����ɹ���");
        }
        #endregion

        #region �ύ��������
        /// <summary>
        /// �����ύ
        /// </summary>
        /// <param name="isUpSubmit"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, LllegalRegisterEntity entity, LllegalReformEntity rEntity, LllegalAcceptEntity acceptEntity)
        {
            Operator curUser = OperatorProvider.Provider.Current();
            try
            {
                //�����˻ز�����Ϣ
                LllegalRegisterEntity baseentity = lllegalregisterbll.GetEntity(keyValue);
                baseentity.RESEVERFOUR = entity.RESEVERFOUR;
                baseentity.RESEVERFIVE = entity.RESEVERFIVE;
                lllegalregisterbll.SaveForm(keyValue, baseentity);

                //������Ϣ
                LllegalReformEntity rfEntity = lllegalreformbll.GetEntityByBid(keyValue);
                rfEntity.REFORMFINISHDATE = rEntity.REFORMFINISHDATE;
                rfEntity.REFORMMEASURE = rEntity.REFORMMEASURE;
                rfEntity.REFORMSTATUS = rEntity.REFORMSTATUS;
                rfEntity.MODIFYDATE = DateTime.Now;
                rfEntity.MODIFYUSERID = curUser.UserId;
                rfEntity.MODIFYUSERNAME = curUser.UserName;
                rfEntity.REFORMPIC = rEntity.REFORMPIC;
                lllegalreformbll.SaveForm(rfEntity.ID, rfEntity);

                /*
                 ����  ���ĵ�����
                 */
                string errorMsg = string.Empty;

                string wfFlag = string.Empty;

                string participant = string.Empty;

                //�˻ز���
                if (entity.RESEVERFOUR == "��")
                {
                    DataTable dt = htworkflowbll.GetBackFlowObjectByKey(keyValue);

                    if (dt.Rows.Count > 0)
                    {
                        wfFlag = dt.Rows[0]["wfflag"].ToString(); //��������

                        participant = dt.Rows[0]["participant"].ToString();  //ָ����
                    }
                }
                else
                {
                    wfFlag = "1";  //���̱�ʶ
                    //��ȡ������
                    UserEntity userEntity = userbll.GetEntity(acceptEntity.ACCEPTPEOPLEID); //������

                    if (null != userEntity)
                    {
                        participant = userEntity.Account;  //��ȡ������һ�ڵ�Ĳ�����Ա (ȡ������)
                    }
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
            var data = lllegalreformbll.GetHistoryList(keyValue);
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
            var data = lllegalreformbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion
    }
    #endregion
}
