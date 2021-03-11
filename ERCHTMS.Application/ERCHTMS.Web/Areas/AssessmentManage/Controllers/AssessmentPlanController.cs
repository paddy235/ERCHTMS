using ERCHTMS.Entity.AssessmentManage;
using ERCHTMS.Busines.AssessmentManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using ERCHTMS.Code;

namespace ERCHTMS.Web.Areas.AssessmentManage.Controllers
{
    /// <summary>
    /// �� ���������ƻ�
    /// </summary>
    public class AssessmentPlanController : MvcControllerBase
    {
        private AssessmentPlanBLL assessmentplanbll = new AssessmentPlanBLL();

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
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_kid = "Id";
            pagination.p_fields = "CreateDate,PlanName,TeamLeaderName,Status,IsLock,createuserid,createuserdeptcode,createuserorgcode,TeamLeader";
            pagination.p_tablename = " bis_assessmentplan";
            pagination.conditionJson = "1=1 and createuserorgcode='" + user.OrganizeCode + "'";
            var data = assessmentplanbll.GetPageList(pagination, queryJson);
            var watch = CommonHelper.TimerStart();
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
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = assessmentplanbll.GetList(queryJson);
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
            var data = assessmentplanbll.GetEntity(keyValue);
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
            assessmentplanbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, AssessmentPlanEntity entity, [System.Web.Http.FromBody]string dataJson)
        {
            if (string.IsNullOrEmpty(keyValue))//����
            {
                entity.Status = "������";
                entity.IsLock = "����";
            }
            assessmentplanbll.SaveForm(keyValue, entity);
            //��������Ĵӱ��¼
            if (dataJson.Length > 0)
            {
                AssessmentSumBLL safeproductPbll = new AssessmentSumBLL();
                List<AssessmentSumEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AssessmentSumEntity>>(dataJson);
                foreach (AssessmentSumEntity data in list)
                {
                    if (string.IsNullOrEmpty(keyValue))
                    {
                        data.Reserve = "δɸѡ";//ɸѡ״̬
                        data.GradeStatus = "δ����";//����״̬
                        safeproductPbll.SaveForm("", data);
                    }
                    else
                    {
                        var sumentity = safeproductPbll.GetEntity(data.ChapterID);//�޸�ʱ��sumidΪ�����ܽ�����id
                        sumentity.DutyName = data.DutyName;
                        sumentity.DutyID = data.DutyID;
                        safeproductPbll.SaveForm(sumentity.Id, sumentity);
                    }

                }
            }
            return Success("�����ɹ���");
        }


        /// <summary>
        /// ���������ƻ�����״̬
        /// </summary>
        /// <param name="planid"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SavePlanInfo(string planid)
        {
            var message = "";
            AssessmentPlanEntity resultEntity = assessmentplanbll.GetEntity(planid);
            if (resultEntity != null)
            {
                if (resultEntity.IsLock == "����")
                {
                    resultEntity.IsLock = "����";
                    message = "�������üƻ���";
                }
                else
                {
                    resultEntity.IsLock = "����";
                    message = "�ѽ����üƻ���";
                }
                assessmentplanbll.SaveForm(resultEntity.Id, resultEntity);
            }
            return Success(message);
        }
        #endregion
    }
}
