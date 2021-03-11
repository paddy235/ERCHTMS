using System.Collections.Generic;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Busines.CarManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;

namespace ERCHTMS.Web.Areas.CarManage.Controllers
{
    /// <summary>
    /// �� ��������Σ��Ʒ�����Ŀ��
    /// </summary>
    public class CarcheckitemController : MvcControllerBase
    {
        private CarcheckitemBLL carcheckitembll = new CarcheckitemBLL();

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
            string orgcode = OperatorProvider.Provider.Current().OrganizeCode;
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "ID";
            pagination.p_fields = "CREATEUSERID,CREATEDATE,MODIFYUSERID,MODIFYDATE,CREATEUSERDEPTCODE,CREATEUSERORGCODE,CHECKITEMNAME,CREATEUSERNAME";
            pagination.p_tablename = @"BIS_CARCHECKITEM";
            pagination.conditionJson = " 1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            //if (user.IsSystem)
            //{
            //    pagination.conditionJson = "1=1";
            //}
            //else
            //{
            //    pagination.conditionJson += " and createuserorgcode like '" + user.OrganizeCode + "%'";
            //}

            var data = carcheckitembll.GetPageList(pagination, queryJson);

            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch),

            };
            return Content(JsonData.ToJson());
        }

        /// <summary>
        /// ��ȡȥ�غ��Σ��Դ�б�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetHazardousList(string KeyValue)
        {
            var data = carcheckitembll.GetHazardousList(KeyValue);
            return Content(data.ToJson());
        }
        /// <summary>
        /// ��ȡͨ���Ÿ�
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetCurrentList(string KeyValue)
        {
            var data = carcheckitembll.GetCurrentList(KeyValue);
            return Content(data.ToJson());
        }


        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = carcheckitembll.GetEntity(keyValue);
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
            carcheckitembll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, string CheckItemName, List<CarcheckitemhazardousEntity> HazardousArray, List<CarcheckitemmodelEntity> ItemArray)
        {
            carcheckitembll.SaveForm(keyValue, CheckItemName, HazardousArray, ItemArray);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
