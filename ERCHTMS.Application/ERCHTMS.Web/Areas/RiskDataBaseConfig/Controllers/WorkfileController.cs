using ERCHTMS.Entity.RiskDataBaseConfig;
using ERCHTMS.Busines.RiskDataBaseConfig;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System;
using System.Linq;

namespace ERCHTMS.Web.Areas.RiskDataBaseConfig.Controllers
{
    /// <summary>
    /// �� �������������嵥˵����
    /// </summary>
    public class WorkfileController : MvcControllerBase
    {
        private WorkfileBLL workfilebll = new WorkfileBLL();

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
        public ActionResult Create()
        {
            return View();
        }
        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ѯ���</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination,string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                pagination.p_kid = "id";
                pagination.p_fields = @"createuserid,createuserdeptcode,createuserorgcode
,createdate,title,issend,sendtime,deptname,deptcode,deptid,createusername";
                pagination.p_tablename = "bis_workfile";
                if (user.IsSystem)
                {
                    pagination.conditionJson = "1=1";
                }
                else
                {
                    pagination.conditionJson = string.Format("((createuserid='System' and issend='1') or (createuserorgcode='{0}' and issend='1') or (createuserid='{1}'))", user.OrganizeCode, user.UserId);
                }

                var data = workfilebll.GetPageList(pagination, queryJson);
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
        /// ���ݻ���Code��ѯ�������Ƿ��Ѿ����
        /// </summary>
        /// <param name="orgCode">����Code</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetIsExist(string orgCode)
        {
            try
            {
                var data = workfilebll.GetIsExist(orgCode);
                return ToJsonResult(data);
            }
            catch (Exception ex)
            {

                return Error(ex.Message);
            }
            
        }
        /// <summary>
        /// ��ѯ�Ƿ����Ĭ������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetWrokFileData() {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                //��ѯ����λ�Ƿ���Ĭ������,û�����ѯϵͳĬ������,û���򷵻�null
                var data = workfilebll.GetList("").Where(x => x.Issend == 1 && x.DeptCode == user.OrganizeCode).ToList();
                if (data.Count > 0)
                {
                    return ToJsonResult(data);
                }
                else
                {
                    data = workfilebll.GetList("").Where(x => x.Issend == 1 && x.DeptCode == "0").ToList();
                    return ToJsonResult(data);
                }
            }
            catch (Exception ex)
            {

                return Error(ex.Message);
            }
          
        }
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            try
            {
                var data = workfilebll.GetEntity(keyValue);
                return ToJsonResult(data);
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
            try
            {
                workfilebll.RemoveForm(keyValue);
                return Success("ɾ���ɹ���");
            }
            catch (Exception ex)
            {

                return Error(ex.Message);
            }
          
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
        public ActionResult SaveForm(string keyValue, WorkfileEntity entity)
        {
            try
            {
                workfilebll.SaveForm(keyValue, entity);
                return Success("�����ɹ���");
            }
            catch (Exception ex)
            {

                return Error(ex.Message);
            }
          
        }
        #endregion
    }
}
