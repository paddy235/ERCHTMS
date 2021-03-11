using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Busines.HighRiskWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.FireManage;
using ERCHTMS.Entity.FireManage;
using System.Data;
using System;
using BSFramework.Data;
using BSFramework.Util.Extension;
using System.Linq;

namespace ERCHTMS.Web.Areas.FireManage.Controllers
{
    /// <summary>
    /// �� �����߷�������Ŀ����
    /// </summary>
    public class EverydayProjectSetController : MvcControllerBase
    {
        private EverydayProjectSetBLL EverydayProjectSetbll = new EverydayProjectSetBLL();

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
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form1() 
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
            pagination.p_kid = "Id";
            pagination.p_fields = "CreateDate,MeasureName,MeasureResultOne,MeasureResultTwo,OrderNumber";
            pagination.p_tablename = " hrs_EverydayProjectSet";
            pagination.conditionJson = "1=1 ";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                pagination.conditionJson += " and createuserorgcode='" + user.OrganizeCode + "'";
                //string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                //if (!string.IsNullOrEmpty(authType))
                //{
                //    switch (authType)
                //    {
                //        case "1":
                //            pagination.conditionJson += " and createuserid='" + user.UserId + "'";
                //            break;
                //        case "2":
                //            pagination.conditionJson += " and createuserdeptcode='" + user.DeptCode + "";
                //            break;
                //        case "3":
                //            pagination.conditionJson += " and createuserdeptcode like'" + user.DeptCode + "%'";
                //            break;
                //        case "4":
                //            pagination.conditionJson += " and createuserorgcode='" + user.OrganizeCode + "'";
                //            break;
                //    }
                //}
                //else
                //{
                //    pagination.conditionJson += " and 0=1";
                //}
            }
            var data = EverydayProjectSetbll.GetPageDataTable(pagination, queryJson);

            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //Ѳ���б���룬�滻ID
            if (!queryParam["type"].IsEmpty())
            {
                foreach (DataRow item in data.Rows)
                {
                    item["id"] = Guid.NewGuid().ToString();
                }
            }
            
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
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = EverydayProjectSetbll.GetList(queryJson);
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
            var data = EverydayProjectSetbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡ˳���
        /// </summary>
        /// <param name="typecode">���ͱ���</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetOrderNumber(string typecode)
        {
            var orderNumber = 1;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            try
            {
                var data = EverydayProjectSetbll.GetList("").Where(t => t.TypeNum == typecode && t.CreateUserOrgCode == user.OrganizeCode).OrderByDescending(t => t.OrderNumber).FirstOrDefault();
                if (data != null)
                {
                    orderNumber = Convert.ToInt32(data.OrderNumber) + 1;
                }
            }
            catch {

            }
            return ToJsonResult(orderNumber);
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
            EverydayProjectSetbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, EverydayProjectSetEntity entity)
        {
            EverydayProjectSetbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
