using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Entity.LaborProtectionManage;
using ERCHTMS.Busines.LaborProtectionManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.SystemManage.ViewModel;

namespace ERCHTMS.Web.Areas.LaborProtectionManage.Controllers
{
    /// <summary>
    /// �� �����Ͷ�����Ԥ����
    /// </summary>
    public class LaboreamyjController : MvcControllerBase
    {
        private LaboreamyjBLL laboreamyjbll = new LaboreamyjBLL();

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
            var data = laboreamyjbll.GetList(queryJson).ToList();
            //��ȡ�ֵ���ģ�������
            DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
            var datadetail = dataItemDetailBLL.GetDataItemListByItemCode("'LaborName'");
            List<LaboreamyjEntity> lylist = new List<LaboreamyjEntity>();
            foreach (DataItemModel item in datadetail)
            {
                if (data.Where(it => it.Name == item.ItemName).Count() == 0)
                {
                    LaboreamyjEntity ly = new LaboreamyjEntity();
                    ly.Name = item.ItemName;
                    string[] ec=item.ItemCode.Split('|');
                    ly.Type = ec[0];
                    ly.No = ec[1];
                    ly.Unit = item.Description;
                    data.Add(ly);
                }
               

            }
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
            var data = laboreamyjbll.GetEntity(keyValue);
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
            laboreamyjbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string json)
        {
            laboreamyjbll.SaveForm(json);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
