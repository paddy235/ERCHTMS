using ERCHTMS.Entity.StandardSystem;
using ERCHTMS.Busines.StandardSystem;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;

namespace ERCHTMS.Web.Areas.StandardSystem.Controllers
{
    /// <summary>
    /// �� ������׼������
    /// </summary>
    public class StandardNewsController : MvcControllerBase
    {
        private StandardNewsBLL standardnewsbll = new StandardNewsBLL();

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
            pagination.p_kid = "ID";
            pagination.p_fields = "createuserid,Title,Publisher,ReleaseTime,IsSend";
            pagination.p_tablename = "BIS_StandardNews t";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                if (!string.IsNullOrEmpty(authType))
                {
                    switch (authType)
                    {
                        case "1":
                            pagination.conditionJson += " and createuserid='" + user.UserId + "'";
                            break;
                        case "2":
                            pagination.conditionJson += " and createuserdeptcode='" + user.DeptCode + "'";
                            break;
                        case "3":
                            pagination.conditionJson += " and createuserdeptcode like'" + user.DeptCode + "%'";
                            break;
                        case "4":
                            pagination.conditionJson += " and createuserorgcode='" + user.OrganizeCode + "'";
                            break;
                    }
                }
                else
                {
                    pagination.conditionJson += " and 0=1";
                }
            }
            pagination.conditionJson += string.Format(" and (IsSend='0' or createuserid='{0}')", user.UserId);
            var watch = CommonHelper.TimerStart();
            var data = standardnewsbll.GetPageList(pagination, queryJson);
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
        /// ��ȡ�б�(��ҳ)
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetPageDataTableJsonForDesk(string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 8;
            pagination.p_kid = "ID";
            pagination.p_fields = "createuserid,Title,Publisher,ReleaseTime,IsSend";
            pagination.p_tablename = "BIS_StandardNews t";
            pagination.conditionJson = string.Format(" createuserorgcode='{0}' and IsSend='0'", user.OrganizeCode);
            pagination.sidx = "createdate";//�����ֶ�
            pagination.sord = "desc";//����ʽ
            var watch = CommonHelper.TimerStart();
            var data = standardnewsbll.GetPageList(pagination, queryJson);
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
            var data = standardnewsbll.GetList(queryJson);
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
            var data = standardnewsbll.GetEntity(keyValue);
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
            standardnewsbll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, StandardNewsEntity entity)
        {
            standardnewsbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
