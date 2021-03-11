using ERCHTMS.Entity.EvaluateManage;
using ERCHTMS.Busines.EvaluateManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Code;
using System.Data;

namespace ERCHTMS.Web.Areas.EvaluateManage.Controllers
{
    /// <summary>
    /// �� �����Ϲ������ۼƻ�
    /// </summary>
    public class EvaluatePlanController : MvcControllerBase
    {
        private EvaluatePlanBLL evaluateplanbll = new EvaluatePlanBLL();

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
            queryJson = queryJson ?? "";
            pagination.p_kid = "Id";
            pagination.p_fields = "WorkTitle,Dept,AbortDate,IsSubmit,createuserid,createuserdeptcode,createuserorgcode,donedeptnum,deptnum,checkstate";
            pagination.p_tablename = "HRS_EVALUATEPLAN";
            pagination.conditionJson = "1=1";
            var watch = CommonHelper.TimerStart();
            var data = evaluateplanbll.GetPageList(pagination, queryJson);
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
            var data = evaluateplanbll.GetList(queryJson);
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
            var data = evaluateplanbll.GetEntity(keyValue);
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
            evaluateplanbll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="IsSubmit">�Ƿ��ύ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, int IsSubmit, int type, EvaluatePlanEntity entity)
        {
            if (type == 1)
            {
                entity.IsSubmit = IsSubmit;
            }
            if (type == 2)
            {
                entity.CheckState = IsSubmit;//0�������ύ 1���۱��汣�� 2���۱����ύ 3��˱��� 4����ύ
            }
            evaluateplanbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion

        /// <summary>
        /// //һ������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult Remind(string keyValue)
        {
            try
            {
                Operator curUser = OperatorProvider.Provider.Current();
                
                MessageEntity messageEntity = new MessageEntity();
                var data = evaluateplanbll.GetEntity(keyValue);
                messageEntity.Title = data.WorkTitle;
                messageEntity.Content = "�����" + data.WorkTitle + "�����۽�ֹʱ��Ϊ��" + data.AbortDate.Value.ToString("yyyy-MM-dd") + "��";
                messageEntity.SendUser = curUser.Account;
                messageEntity.SendUserName = curUser.UserName;
                messageEntity.SendTime = DateTime.Now;
                messageEntity.Category = "����";
                DataTable dt = evaluateplanbll.GetRemindUser(keyValue);

                string userid = "";
                string username = "";
                //��������
                if (dt != null)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        userid += item["userid"].ToString() + ",";
                        username += item["username"].ToString() + ",";
                    }
                }
                messageEntity.UserId = userid;
                messageEntity.UserName = username;

                new MessageBLL().SaveForm("", messageEntity);
                return Success("�����ɹ���");
            }
            catch (Exception ex)
            {
                return Success("����ʧ�ܡ�");
            }
        }
        
    }
}
