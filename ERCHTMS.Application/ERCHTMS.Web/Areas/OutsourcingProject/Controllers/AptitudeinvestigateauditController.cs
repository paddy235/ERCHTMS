using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// �� �������������˱�
    /// </summary>
    public class AptitudeinvestigateauditController : MvcControllerBase
    {
        private AptitudeinvestigateauditBLL aptitudeinvestigateauditbll = new AptitudeinvestigateauditBLL();

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
            var data = aptitudeinvestigateauditbll.GetList(queryJson);
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
            var data = aptitudeinvestigateauditbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetAuditListJson(string recId)
        {
            var data = aptitudeinvestigateauditbll.GetAuditRecList(recId);
            return ToJsonResult(data);
        }
        [HttpGet]
        public ActionResult GetHisAuditListJson(string recId)
        {
            var data = new HistoryAuditBLL().GetHisAuditRecList(recId);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ���ݹ���ҵ��Id��ѯ��˼�¼
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "t.ID";
                pagination.p_fields = @" t.auditresult,t.audittime,t.auditopinion,t.auditpeople,t.auditdept ";
                pagination.p_tablename = @"epg_aptitudeinvestigateaudit t";
                pagination.sidx = "t.audittime";//�����ֶ�
                pagination.sord = "desc";//����ʽ
                pagination.conditionJson = " 1=1 ";
                var data = aptitudeinvestigateauditbll.GetPageList(pagination, queryJson);


                var jsonData = new
                {
                    rows = data,
                    total = pagination.total,
                    page = pagination.page,
                    records = pagination.records,
                    costtime = CommonHelper.TimerEnd(watch)
                };
                return ToJsonResult(jsonData);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// ����ҵ��id��ȡ��Ӧ����˼�¼�б� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetAuditList(string keyValue) 
        {
            var data = aptitudeinvestigateauditbll.GetAuditRecList(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ����ҵ��id��ȡ��Ӧ����˼�¼�б� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetSpecialAuditList(string keyValue)
        {
            var data = aptitudeinvestigateauditbll.GetAuditList(keyValue);
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
            aptitudeinvestigateauditbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, AptitudeinvestigateauditEntity entity)
        {
            aptitudeinvestigateauditbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        /// <summary>
        /// ��������������޸ģ�
        /// ����������ͨ��ͬ�� ���� ��λ ��Ա��Ϣ
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveSynchrodata(string keyValue, AptitudeinvestigateauditEntity entity)
        {
            aptitudeinvestigateauditbll.SaveSynchrodata(keyValue, entity);
            return Success("�����ɹ���");
        }
        /// <summary>
        /// ��������������޸ģ�
        /// ��֤�����
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveSafetyEamestMoney(string keyValue, AptitudeinvestigateauditEntity entity)
        {
            aptitudeinvestigateauditbll.SaveSafetyEamestMoney(keyValue, entity);
            return Success("�����ɹ���");
        }

        /// <summary>
        /// ��������������޸ģ�
        /// �����������:���¹���״̬
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult AuditReturnForWork(string keyValue, AptitudeinvestigateauditEntity entity)
        {
            aptitudeinvestigateauditbll.AuditReturnForWork(keyValue, entity);
            return Success("�����ɹ���");
        }

        /// <summary>
        /// ��������������޸ģ�
        /// �����������:���¹���״̬
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult AuditStartApply(string keyValue, AptitudeinvestigateauditEntity entity, string projectId,string result="",string users="")
        {
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            StartapplyforBLL applyBll = new StartapplyforBLL();
            var sl = applyBll.GetEntity(entity.APTITUDEID);
            string status = "";
            var mp = new PeopleReviewBLL().CheckAuditPower(user, out status, "��������", projectId);
            if (mp != null)
            {
                if (sl!=null)
                {
                    sl.NodeName = mp.FLOWNAME;
                    sl.NodeId = mp.ID;
                    sl.AuditRole = mp.CHECKROLEID;
                    applyBll.SaveForm(entity.APTITUDEID, sl);
                }
               
            }
            else
            {
                if (status=="1")
                {
                    sl.NodeName = "������";
                    sl.NodeId = "";
                    sl.IsOver = 1;
                }
            }
            if (entity.AUDITRESULT == "1")
            {
                sl.ISCOMMIT = "0";
                sl.NodeName = "";
                sl.NodeId = "";
                sl.IsOver = 0;
            }
            if (!string.IsNullOrEmpty(result))
            {
                sl.CheckResult = result;
                sl.CheckUsers = users;
            }
           if( applyBll.SaveForm(entity.APTITUDEID, sl))
           {
               aptitudeinvestigateauditbll.AuditStartApply("", entity);
           }
           return Success("�����ɹ���");
        }


        /// <summary>
        /// ��Ա����������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult AuditPeopleReview(string keyValue, AptitudeinvestigateauditEntity entity)
        {
            aptitudeinvestigateauditbll.AuditPeopleReview(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
