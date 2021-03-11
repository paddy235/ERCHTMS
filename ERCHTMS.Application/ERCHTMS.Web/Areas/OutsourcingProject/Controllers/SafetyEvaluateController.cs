using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using BSFramework.Util.Extension;
using System;
using System.Linq;
using ERCHTMS.Busines.BaseManage;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// �� ������ȫ����
    /// </summary>
    public class SafetyEvaluateController : MvcControllerBase
    {
        private SafetyEvaluateBLL safetyevaluatebll = new SafetyEvaluateBLL();
        private DepartmentBLL departmentbll = new DepartmentBLL();

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
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "t.ID";
            pagination.p_fields = @" e.fullname,r.ENGINEERNAME,t.SiteManagementScore,t.QualityScore,t.ProjectProgressScore,t.FieldServiceScore,
t.EvaluationScore,to_char(t.EvaluationTime,'yyyy-MM-dd') as EvaluationTime,t.modifyuserid,t.issend,t.createuserid ";
            pagination.p_tablename = @"EPG_SafetyEvaluate t left join EPG_OutSouringEngineer r 
on t.projectid=r.id left join base_department e on r.outprojectid=e.departmentid";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string role = user.RoleName;
            if (role.Contains("��˾���û�") || role.Contains("���������û�"))
            {
                pagination.conditionJson = string.Format(" (t.createuserorgcode  = '{0}' and t.issend='1' or t.createuserid ='{1}')", user.OrganizeCode,user.UserId);
            }
            else if (role.Contains("�а��̼��û�"))
            {
                pagination.conditionJson = string.Format(" (e.departmentid = '{0}' or r.SUPERVISORID='{0}' or t.createuserid ='{1}') ", user.DeptId, user.UserId);
            }
            else
            {
                var deptentity = departmentbll.GetEntity(user.DeptId);
                while (deptentity.Nature == "����" || deptentity.Nature == "רҵ")
                {
                    deptentity = departmentbll.GetEntity(deptentity.ParentId);
                }
                pagination.conditionJson = string.Format(" (r.engineerletdeptid in (select departmentid from base_department where encode like '{0}%') and t.issend='1'  or t.createuserid='{1}') ", deptentity.EnCode, user.UserId);

                //pagination.conditionJson = string.Format(" (r.engineerletdeptid = '{0}' and t.issend='1' or t.createuserid ='{1}') ", user.DeptId, user.UserId);
            }
            var queryParam = queryJson.ToJObject();
            //ʱ�䷶Χ
            if (!queryParam["sTime"].IsEmpty() || !queryParam["eTime"].IsEmpty())
            {
                string startTime = queryParam["sTime"].ToString();
                string endTime = queryParam["eTime"].ToString();
                if (queryParam["sTime"].IsEmpty())
                {
                    startTime = "1899-01-01";
                }
                if (queryParam["eTime"].IsEmpty())
                {
                    endTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
                pagination.conditionJson += string.Format(" and to_date(to_char(evaluationtime,'yyyy-MM-dd'),'yyyy-MM-dd') between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", startTime, endTime);
            }
            ////��ѯ����
            //if (!queryParam["txtSearch"].IsEmpty())
            //{
            //    pagination.conditionJson += string.Format(" and r.ENGINEERNAME like '%{0}%'", queryParam["txtSearch"].ToString());
            //}
            //��ѯ����
            if (!queryParam["condition"].IsEmpty() && !queryParam["txtSearch"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and {0} like '%{1}%'", queryParam["condition"].ToString(), queryParam["txtSearch"].ToString());
            }
            if (!queryParam["projectid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.projectid='{0}'", queryParam["projectid"].ToString());
            }
            var watch = CommonHelper.TimerStart();
            var data = safetyevaluatebll.GetList(pagination, queryJson);
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
            var data = safetyevaluatebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
          [HttpGet]
        public ActionResult GetSafetyByProjectId(string id)
        {
            var data = safetyevaluatebll.GetList().Where(x => x.PROJECTID == id).ToList();
            if (data.Count > 0)
            {
                return ToJsonResult(data.FirstOrDefault());
            }
            else
            {
                return ToJsonResult(null);
            }
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
            safetyevaluatebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SafetyEvaluateEntity entity)
        {
            safetyevaluatebll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
