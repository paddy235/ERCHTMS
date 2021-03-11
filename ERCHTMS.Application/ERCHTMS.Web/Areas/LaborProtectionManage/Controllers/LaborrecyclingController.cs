using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERCHTMS.Entity.LaborProtectionManage;
using ERCHTMS.Busines.LaborProtectionManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.LaborProtectionManage.Controllers
{
    /// <summary>
    /// �� �����Ͷ��������ձ��ϱ�����
    /// </summary>
    public class LaborrecyclingController : MvcControllerBase
    {
        private LaborrecyclingBLL laborrecyclingbll = new LaborrecyclingBLL();

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
        public ActionResult SeeIndex()
        {
            return View();
        }

        #endregion

        #region ��ȡ����

        public ActionResult GetIssueList(string keyValue)
        {

            var data = laborrecyclingbll.GetList(keyValue);
            return ToJsonResult(data);

        }

        /// <summary>
        /// ������Ʒ��id��ȡ�����������
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetOrderLabor(string keyValue)
        {
            LaborequipmentinfoBLL laborequipmentinfobll = new LaborequipmentinfoBLL();
            LaborissuedetailBLL detail = new LaborissuedetailBLL();
            List<LaborequipmentinfoEntity> list = new List<LaborequipmentinfoEntity>();
            var data = detail.GetOrderLabor(keyValue);
            if (data != null)
            {
                list = laborequipmentinfobll.GetList(data.ID).ToList();
            }

            return ToJsonResult(list);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = laborrecyclingbll.GetList(queryJson);
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
            var data = laborrecyclingbll.GetEntity(keyValue);
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
            laborrecyclingbll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ������������������޸ģ�
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveListForm(string json)
        {
            json = HttpUtility.UrlDecode(json);
            //���û��������ֱ�ӷ���
            if (json == "")
            {
                return Error("�޶�Ӧ���ݣ�������Ч");
            }
            laborrecyclingbll.SaveListForm(json);
            return Success("�����ɹ���");
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
        public ActionResult SaveForm(string keyValue, LaborrecyclingEntity entity, string json, string InfoId)
        {
            json = HttpUtility.UrlDecode(json);
            if (json == "")
            {
                return Error("�޶�Ӧ���ݣ�������Ч");
            }
            laborrecyclingbll.SaveForm(keyValue, entity, json, InfoId);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
