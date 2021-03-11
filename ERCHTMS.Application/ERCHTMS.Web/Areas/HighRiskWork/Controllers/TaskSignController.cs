using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Busines.HighRiskWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.HighRiskWork.Controllers
{
    /// <summary>
    /// �� �����ල����ǩ��
    /// </summary>
    public class TaskSignController : MvcControllerBase
    {
        private TaskSignBLL tasksignbll = new TaskSignBLL();

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
            var data = tasksignbll.GetList(queryJson);
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
            var data = tasksignbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ������վ�ල����id��ȡ�ලǩ����Ϣ
        /// </summary>
        /// <param name="superviseId">�ල����id</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetTaskSignInfo(string superviseId)
        {
            var data = tasksignbll.GetTaskSignInfo(superviseId);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ������վ�ල����id��ȡ�ලǩ���б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetTaskSignTable(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            var superviseid = queryParam["superviseid"];
            pagination.p_kid = "id";
            pagination.p_fields = "supervisetime,supervisestate,signfile";
            pagination.p_tablename = "bis_tasksign";
            pagination.conditionJson = string.Format("superviseid='{0}'", superviseid);
            var data = tasksignbll.GetPageDataTable(pagination);
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
            tasksignbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, TaskSignEntity entity)
        {
            tasksignbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
