using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Busines.HighRiskWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Collections.Generic;

namespace ERCHTMS.Web.Areas.HighRiskWork.Controllers
{
    /// <summary>
    /// �� ������Σ����ҵ���/������
    /// </summary>
    public class HighRiskCheckController : MvcControllerBase
    {
        private HighRiskCheckBLL highriskcheckbll = new HighRiskCheckBLL();
        private HighRiskApplyBLL highriskapplybll = new HighRiskApplyBLL();

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
            var data = highriskcheckbll.GetList(queryJson);
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
            var data = highriskcheckbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }


        /// <summary>
        /// ��������id��ȡ��˼�¼
        /// </summary>
        /// <param name="applyid">�����id</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetCheckListInfo(string applyid)
        {
            var data = highriskcheckbll.GetCheckListInfo(applyid);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��������id��ȡ������¼
        /// </summary>
        /// <param name="applyid">�����id</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetApproveInfo(string applyid)
        {
            var data = highriskcheckbll.GetApproveInfo(applyid);
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
        public ActionResult RemoveForm(string keyValue)
        {
            highriskcheckbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string approveid, [System.Web.Http.FromBody]string dataJson)
        {
            var nochecknum=highriskcheckbll.GetNoCheckNum(approveid);
            var applyentity = highriskapplybll.GetEntity(approveid);
            if (applyentity.ApplyState == "4")//����
            {
                if (dataJson.Length > 0)
                {
                    List<HighRiskCheckEntity> vchecklist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<HighRiskCheckEntity>>(dataJson);
                    var vcheckentity = highriskcheckbll.GetNeedCheck(approveid);
                    foreach (HighRiskCheckEntity item in vchecklist)
                    {
                        vcheckentity.ApproveReason = item.ApproveReason;
                        vcheckentity.ApproveState = item.ApproveState;
                        if (item.ApproveState == "2")
                            applyentity.ApplyState = "5";//����δͨ��
                        if (item.ApproveState == "1")
                            applyentity.ApplyState = "6";//�������
                        highriskcheckbll.SaveForm(vcheckentity.Id, vcheckentity);
                    }
                    highriskapplybll.SaveForm(approveid, applyentity);
                }
            }
            if (applyentity.ApplyState == "2")//���
            {
                if (nochecknum > 0)
                {
                    if (dataJson.Length > 0)
                    {
                        List<HighRiskCheckEntity> vchecklist = Newtonsoft.Json.JsonConvert.DeserializeObject<List<HighRiskCheckEntity>>(dataJson);
                        var vcheckentity = highriskcheckbll.GetNeedCheck(approveid);
                        foreach (HighRiskCheckEntity item in vchecklist)
                        {
                            vcheckentity.ApproveReason = item.ApproveReason;
                            vcheckentity.ApproveState = item.ApproveState;
                            if (item.ApproveState == "2")
                            {
                                applyentity.ApplyState = "3";//���δͨ��
                            }
                            if (nochecknum == 1 && item.ApproveState == "1")
                            {
                                applyentity.ApplyState = "4";//������
                            }
                            highriskcheckbll.SaveForm(vcheckentity.Id, vcheckentity);
                        }
                        highriskapplybll.SaveForm(approveid, applyentity);
                    }
                }
            }
            return Success("�����ɹ���");
        }
        #endregion
    }
}
