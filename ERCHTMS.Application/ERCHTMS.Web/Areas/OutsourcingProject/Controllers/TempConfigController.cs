using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using System.Linq;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// �� �����糧ģ�����
    /// </summary>
    public class TempConfigController : MvcControllerBase
    {
        private TempConfigBLL tempconfigbll = new TempConfigBLL();

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
            var data = tempconfigbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡ��ҳ�б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetTempConfigPageJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "id";
            pagination.p_fields = @" createuserid,createuserdeptcode,createuserorgcode,createdate,createusername,modulename,modulecode,tempname,proessmode,deptname,deptcode,deptid";
            pagination.p_tablename = @" epg_tempmanage t";
            Operator currUser = OperatorProvider.Provider.Current();
            pagination.conditionJson = string.Format(" 1=1 ");
            if (!currUser.IsSystem)
            {
                //���ݵ�ǰ�û���ģ���Ȩ�޻�ȡ��¼
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "createuserdeptcode", "deptcode");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }
            }
            var data = tempconfigbll.GetTempConfigPageJson(pagination, queryJson);
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
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = tempconfigbll.GetEntity(keyValue);
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
            tempconfigbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, TempConfigEntity entity, string Mode)
        {
            var currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (Mode == "add")
            {
                var e = tempconfigbll.GetList("").Where(x => x.ModuleCode == entity.ModuleCode && x.CreateUserOrgCode == currUser.OrganizeCode).ToList().Count;
                if (e > 0)
                {
                    return Error("����Ӹ�ģ���ģ��,�����ظ����!");
                }
            }
            tempconfigbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");


        }
        #endregion
    }
}
