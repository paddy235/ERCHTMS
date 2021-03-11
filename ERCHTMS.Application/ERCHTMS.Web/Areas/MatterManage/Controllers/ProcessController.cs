using ERCHTMS.Entity.MatterManage;
using ERCHTMS.Busines.MatterManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.MatterManage.Controllers
{
    /// <summary>
    /// �� �������̼�¼����
    /// </summary>
    public class ProcessController : MvcControllerBase
    {
        private OperticketmanagerBLL operticketmanagerbll = new OperticketmanagerBLL();

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
        /// ��ӡ��ͼ
        /// </summary>
        /// <returns></returns>
        public ActionResult Stamp()
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
        public ActionResult GetPageList(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "Id";
            pagination.p_fields = "Numbers,createdate,GetData,ProductType,SupplyName,isdelete,PlateNumber,Dress,Remark,transporttype,takegoodsname,PassRemark,IsFirst,IsTrajectory,WeighingNum,DataBaseNum,OutDate,StayTime,Status,deletecontent";
            pagination.p_tablename = "WL_OPERTICKETMANAGER";
            pagination.conditionJson = "1=1 ";
            var watch = CommonHelper.TimerStart();
            var data = operticketmanagerbll.GetPageList(pagination, queryJson);
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
            var data = operticketmanagerbll.GetEntity(keyValue);
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
            operticketmanagerbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, OperticketmanagerEntity entity)
        {
            operticketmanagerbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }

        /// <summary>
        /// �޸ļ�¼״̬
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpateStatus(string keyValue, OperticketmanagerEntity entity)
        {
            var data = operticketmanagerbll.GetEntity(keyValue);
            if (data != null)
            {
                data.Isdelete = 0;
                data.DeleteContent = entity.DeleteContent;
                operticketmanagerbll.SaveForm(keyValue, data);
            }
            return Success("�����ɹ���");
        }

        /// <summary>
        /// ��ӡʱ���ü�¼���ɶ�ά��ͼƬ
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveImg(string keyValue, OperticketmanagerEntity entity)
        {
            var data = operticketmanagerbll.GetEntity(keyValue);
            if (data != null)
            {
                operticketmanagerbll.SaveForm(keyValue, data);
            }
            return Success("�����ɹ���");
        }


        #endregion
    }
}
