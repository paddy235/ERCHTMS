using System.Web.Mvc;


namespace ERCHTMS.Web.Areas.LllegalManage.Controllers
{
    /// <summary>
    /// �� ����Υ���ƶ����ļƻ�
    /// </summary>
    public class LllegalPlanReformController : MvcControllerBase
    {
        #region ��ͼ
        /// <summary>
        /// �б�ҳ��  ������ҳ��ʹ��
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
        public ActionResult DeliverForm()
        {
            return View();
        }
        #endregion
    }
}
