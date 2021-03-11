using ERCHTMS.Entity.RoutineSafetyWork;
using ERCHTMS.Busines.RoutineSafetyWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System.Dynamic;
using BSFramework.Data;
using BSFramework.Util.Extension;

namespace ERCHTMS.Web.Areas.RoutineSafetyWork.Controllers
{
    /// <summary>
    /// �� ������ȫ��ڰ�
    /// </summary>
    public class SecurityRedListController : MvcControllerBase
    {
        private SecurityRedListBLL securityredlistbll = new SecurityRedListBLL();

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
        /// ͳ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Stat()
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
            pagination.p_fields = "createuserid,Title,Publisher,PublisherDept,ReleaseTime,IsSend,State";
            pagination.p_tablename = "BIS_SecurityRedList t";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            
              var queryParam = queryJson.ToJObject();
              //����ҳ��ȫ������
              if (!queryParam["action"].IsEmpty())
              {
                  pagination.conditionJson += string.Format(" issend='0' and publisherdeptcode  like '{0}%'", user.OrganizeCode);
              }
              else 
              {
                  pagination.conditionJson = string.Format(" createuserorgcode = '{0}'", user.OrganizeCode);
                  pagination.conditionJson += string.Format(" and (issend='0' or createuserid='{0}')", user.UserId);
              }

            var watch = CommonHelper.TimerStart();
            var data = securityredlistbll.GetPageList(pagination, queryJson);
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
            var data = securityredlistbll.GetList(queryJson);
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
            var data = securityredlistbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡ��ȫ��ڰ�ͳ��ͼ���б�
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetSecurityRedListStat(string queryJson)
        {
            object obj = securityredlistbll.GetSecurityRedListStat(queryJson);
            return ToJsonResult(obj);
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
            securityredlistbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SecurityRedListEntity entity)
        {
            securityredlistbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
