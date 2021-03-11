using ERCHTMS.Entity.SaftProductTargetManage;
using ERCHTMS.Busines.SaftProductTargetManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Code;
using System.Data;

namespace ERCHTMS.Web.Areas.SaftProductTargetManage.Controllers
{
    /// <summary>
    /// �� ������ȫ����������
    /// </summary>
    public class SafeProductDutyBookController : MvcControllerBase
    {
        private SafeProductDutyBookBLL safeproductdutybookbll = new SafeProductDutyBookBLL();

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
            var data = safeproductdutybookbll.GetList(queryJson);
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
            var data = safeproductdutybookbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ�ļ���Ϣ�б�
        /// </summary>
        ///<param name="fileId">������id</param>
        [HttpGet]
        public ActionResult GetFiles(string fileId)
        {
            FileInfoBLL fi = new FileInfoBLL();
            var data = fi.GetFiles(fileId);
            foreach (DataRow item in data.Rows)
            {
               var path = item.Field<string>("FilePath");
               var url = Url.Content(path);
               item.SetField<string>("FilePath", url);
            }
            return ToJsonResult(data);

        }

        /// <summary>
        /// ��ȡ�������б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetDataListJson(string productId)
        {
            var data = safeproductdutybookbll.GetListByProductId(productId);
            return ToJsonResult(data);
        }

        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "Id";
            pagination.p_fields = "DutyBookName,PartyA,PartyB,WriteDate,ProductId,CreateDate,FileId";
            pagination.p_tablename = "bis_safeproductdutybook";
            pagination.conditionJson = "1=1";
            var watch = CommonHelper.TimerStart();
            var data = safeproductdutybookbll.GetPageList(pagination, queryJson);
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
            safeproductdutybookbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SafeProductDutyBookEntity entity)
        {
            safeproductdutybookbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
