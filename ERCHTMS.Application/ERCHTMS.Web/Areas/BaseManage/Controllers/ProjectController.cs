using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using System;
using System.Data;

namespace ERCHTMS.Web.Areas.BaseManage.Controllers
{
    /// <summary>
    /// �� �������������Ŀ��Ϣ
    /// </summary>
    public class ProjectController : MvcControllerBase
    {
        private ProjectBLL projectbll = new ProjectBLL();

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

        [HttpGet]
        public ActionResult Select() 
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
            var data = projectbll.GetList(queryJson);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns> 
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "ProjectId";
            pagination.p_fields = "t.CreateDate,t.CreateUserID,ProjectName,ProjectDeptName,ProjectDeptCode,ProjectStatus,ProjectStartDate,ProjectEndDate, ProjectContent,b.SendDeptID,b.OrganizeId,OrganizeCode";
            pagination.p_tablename = "bis_project t left join base_department b on b.EnCode=t.ProjectDeptCode";
            string type = new AuthorizeBLL().GetOperAuthorzeType(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.conditionJson = " 1=1 ";
            if (type == "4")
            {
                //������
                pagination.conditionJson = string.Format(" 1=1 and b.OrganizeId='{0}' ", user.OrganizeId);
            }
            else if (type == "3")
            {
                //���Ӳ���
                if (user.RoleName.Contains("�а���") || user.RoleName.Contains("�ְ���"))
                    pagination.conditionJson = string.Format(" 1=1 and ProjectDeptCode like '{0}%' ", user.DeptCode);
                else
                    pagination.conditionJson = string.Format(" 1=1 and SendDeptID= '{0}'", user.DeptId);
            }
            else if (type == "2")
            {
                //������
                pagination.conditionJson = string.Format(" 1=1 and ProjectDeptCode = '{0}' ", user.DeptCode);
            }

            //����������ѡ����Ŀ
            if (null != Request.Params["OrgArgs"]) 
            {
                pagination.conditionJson = string.Format(" 1=1 and b.OrganizeId='{0}' ", user.OrganizeId);
            }
            var watch = CommonHelper.TimerStart();
            //var data = projectbll.GetPageList(pagination, queryJson);
            DataTable data = projectbll.GetPageDataTable(pagination, queryJson);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return Content(JsonData.ToJson());
        }

        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = projectbll.GetEntity(keyValue);
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
        [HandlerMonitor(6, "ɾ�����������Ϣ")]
        public ActionResult RemoveForm(string keyValue)
        {
            projectbll.RemoveForm(keyValue);
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
        [HandlerMonitor(5, "���������޸����������Ϣ")]
        public ActionResult SaveForm(string keyValue, ProjectEntity entity)
        {
            projectbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
