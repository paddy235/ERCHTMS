using System.Web.Mvc;


namespace ERCHTMS.Web.Areas.LllegalManage.Controllers
{
    /// <summary>
    /// �� �����糧��ȫ���ܲ�������Υ����Ϣ
    /// </summary>
    public class LllegalPerfectController : MvcControllerBase
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
        #endregion          
    }         
}
