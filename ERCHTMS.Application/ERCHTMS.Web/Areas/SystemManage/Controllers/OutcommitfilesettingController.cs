using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Busines.SystemManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System.Linq;

namespace ERCHTMS.Web.Areas.SystemManage.Controllers
{
    /// <summary>
    /// �� �����������˵���û����ñ�
    /// </summary>
    public class OutcommitfilesettingController : MvcControllerBase
    {
        private OutcommitfilesettingBLL outcommitfilesettingbll = new OutcommitfilesettingBLL();

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
            var data = outcommitfilesettingbll.GetList(queryJson);
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
            var data = outcommitfilesettingbll.GetEntity(keyValue);
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
            outcommitfilesettingbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, OutcommitfilesettingEntity entity)
        {
            outcommitfilesettingbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        /// <summary>
        /// ���浱ǰ�û�������
        /// </summary>
        /// <param name="Setting"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SettingUserExplain(int Setting, string FileCommitId)
        {
           
            Operator currUser = OperatorProvider.Provider.Current();
            var s=outcommitfilesettingbll.GetList().Where(x => x.FileCommitId == FileCommitId && x.UserId == currUser.UserId).FirstOrDefault();
            //var s = new OutcommitfilesettingEntity();
            if (s == null)
            {
                s = new OutcommitfilesettingEntity();
                s.IsSetting = Setting;
                s.UserAccount = currUser.Account;
                s.UserId = currUser.UserId;
                s.UserName = currUser.UserName;
                s.FileCommitId = FileCommitId;
            }
            else {
                s.IsSetting = Setting;
            }
            outcommitfilesettingbll.SaveForm(s.ID, s);
            return Success("�����ɹ���");
        }

        #endregion
    }
}
