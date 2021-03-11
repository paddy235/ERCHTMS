using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Busines.CarManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Web.Areas.CarManage.Controllers
{
    /// <summary>
    /// �� �����Ž�ʹ���û���
    /// </summary>
    public class HikaccessuserController : MvcControllerBase
    {
        private HikaccessuserBLL hikaccessuserbll = new HikaccessuserBLL();

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
            List<TreeEntity> treeList = new List<TreeEntity>();
            List<HikaccessuserEntity> userlist = hikaccessuserbll.GetList("").ToList();
            foreach (HikaccessuserEntity item in userlist)
            {
                int chkState = 0;
                TreeEntity tree = new TreeEntity();
                bool hasChildren = false;
                tree.id = item.UserId;
                tree.text = item.UserName;
                tree.value = item.ID;
                //tree.Attribute = "Code";
                //tree.AttributeValue = item.DistrictCode;
                //tree.AttributeA = "Dept";
                //tree.AttributeValueA = item.ChargeDept + "," + item.ChargeDeptCode + "," + item.ChargeDeptID;
                tree.parentId = "0";
                tree.isexpand = false;
                tree.complete = true;
                tree.hasChildren = false;
                tree.showcheck = true;

                tree.checkstate = chkState;
                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson("0"));
        }
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = hikaccessuserbll.GetEntity(keyValue);
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
            hikaccessuserbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, HikaccessuserEntity entity)
        {
            hikaccessuserbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
