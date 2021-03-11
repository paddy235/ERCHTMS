using ERCHTMS.Entity.DangerousJobConfig;
using ERCHTMS.Busines.DangerousJobConfig;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System;
using System.Linq;

namespace ERCHTMS.Web.Areas.DangerousJobConfig.Controllers
{
    /// <summary>
    /// �� ����Σ����ҵ�ּ���׼����
    /// </summary>
    public class ClassStandardConfigController : MvcControllerBase
    {
        private ClassStandardConfigBLL classstandardconfigbll = new ClassStandardConfigBLL();

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
            try
            {
                var watch = CommonHelper.TimerStart();
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                pagination.p_kid = "a.id";
                pagination.p_fields = @"a.createuserid,a.createuserdeptcode,a.createuserorgcode
                ,a.createdate,worktype,d.itemname as worktypename,deptid,deptcode,deptname,a.createusername";
                pagination.p_tablename = "dj_classstandardconfig a left join base_dataitemdetail d on a.worktype=d.itemvalue and d.itemid =(select itemid from base_dataitem where itemcode='DangerousJobConfig')";
                if (user.IsSystem)
                {
                    pagination.conditionJson = "1=1";
                }
                else
                {
                    pagination.conditionJson = string.Format("a.createuserid='System'  or a.createuserorgcode='{0}' or a.createuserid='{1}'", user.OrganizeCode, user.UserId);
                }
                var data = classstandardconfigbll.GetPageList(pagination, queryJson);
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

                return Error(ex.Message);
            }

        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = classstandardconfigbll.GetList(queryJson);
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
            var data = classstandardconfigbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJsonByWorkType(string WorkType)
        {
            try
            {
                Operator user = OperatorProvider.Provider.Current();
                var data = classstandardconfigbll.GetList("").Where(t => t.WorkType == WorkType && t.DeptCode == user.OrganizeCode).FirstOrDefault();
                return Success("��ȡ�ɹ�", data);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
            
        }
        /// <summary>
        /// �Ƿ������ͬ��������
        /// </summary>
        /// <param RiskType="��������"></param>
        /// <param WayType="ȡֵ����"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult IsExistDataByType(string WorkType)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var data = classstandardconfigbll.GetList("").Where(x => x.WorkType == WorkType && x.DeptCode == user.OrganizeCode).ToList();
                if (data.Count > 0)
                {
                    return ToJsonResult(false);
                }
                else
                {
                    return ToJsonResult(true);
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
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
            classstandardconfigbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, ClassStandardConfigEntity entity)
        {
            classstandardconfigbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
