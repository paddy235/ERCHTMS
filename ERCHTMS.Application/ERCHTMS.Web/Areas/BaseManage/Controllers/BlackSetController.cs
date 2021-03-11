using ERCHTMS.Entity.PersonManage;
using ERCHTMS.Busines.PersonManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Linq;
using ERCHTMS.Busines.SystemManage;
using System.Collections.Generic;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
namespace ERCHTMS.Web.Areas.BaseManage.Controllers
{
    /// <summary>
    /// �� ������������׼����
    /// </summary>
    public class BlackSetController : MvcControllerBase
    {
        private BlackSetBLL scoresetbll = new BlackSetBLL();

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
            var data = scoresetbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        [HttpGet]
        public ActionResult GetItemListJson()
        {
            var data = scoresetbll.GetList(ERCHTMS.Code.OperatorProvider.Provider.Current().OrganizeCode);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAgeRange(string deptCode)
        {
            var data = scoresetbll.GetAgeRange(ERCHTMS.Code.OperatorProvider.Provider.Current().OrganizeCode);
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
            DataItemDetailBLL itemBll = new DataItemDetailBLL();
            ERCHTMS.Entity.SystemManage.DataItemDetailEntity entity = itemBll.GetEntity(keyValue);
            return ToJsonResult(entity);
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
            scoresetbll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        ///  <param name="score">��ʼ����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue,string itemJson)
        {
            List<BlackSetEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BlackSetEntity>>(itemJson);
            if (list.Count>0)
            {
                new BlackSetBLL().SaveForm(keyValue, list);
                return Success("�����ɹ���");
            }
            else
            {
                return Error("����д������ȷ�����ݣ�");
            }
          
        }
        #endregion
    }
}
