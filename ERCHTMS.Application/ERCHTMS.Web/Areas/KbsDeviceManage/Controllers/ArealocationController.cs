using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.Busines.KbsDeviceManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Collections.Generic;
using ERCHTMS.Entity.CarManage;
using System.Linq;
using Newtonsoft.Json;
using ERCHTMS.Busines.BaseManage;

namespace ERCHTMS.Web.Areas.KbsDeviceManage.Controllers
{
    /// <summary>
    /// �� ��������λ��
    /// </summary>
    public class ArealocationController : MvcControllerBase
    {
        private ArealocationBLL arealocationbll = new ArealocationBLL();

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
        public ActionResult GetListJson(Pagination pagination)
        {

            var data = arealocationbll.GetTable();

            List<TreeGridEntity> treeList = new List<TreeGridEntity>();
            foreach (KbsAreaLocation item in data)
            {
                TreeGridEntity tree = new TreeGridEntity();
                bool hasChildren = data.Count(t => t.ParentID == item.DistrictID) == 0 ? false : true;
                tree.id = item.DistrictID;
                tree.parentId = item.ParentID;
                tree.expanded = false;
                tree.hasChildren = hasChildren;
                string itemJson = item.ToJson();
                tree.entityJson = itemJson;
                treeList.Add(tree);
            }
            return Content(treeList.TreeJson("0"));
        }
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = arealocationbll.GetEntity(keyValue);
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
            arealocationbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string ID,string DistrictId,string PostList,string ModelIds)
        {
            DistrictBLL disbll = new DistrictBLL();
            var dis = disbll.GetEntity(DistrictId);
            ArealocationEntity Area = new ArealocationEntity();
            Area.AreaCode = dis.DistrictCode;
            Area.AreaId = DistrictId;
            Area.AreaName = dis.DistrictName;
            Area.AreaParentId = dis.ParentID;
            if (ID == "")
            {
                Area.Create();
            }
            else
            {
                Area.Modify(ID);
            }
            Area.AreaParentId = dis.ParentID;
            Area.PointList = PostList;
            Area.ModelIds = ModelIds;
            arealocationbll.SaveForm(ID, Area);

            KbsAreaLocation ka = new KbsAreaLocation();
            ka.DistrictCode = Area.AreaCode;
            ka.DistrictID = Area.AreaId;
            ka.DistrictName = Area.AreaName;
            ka.ID = Area.ID;
            ka.ModelIds = Area.ModelIds;
            ka.OrganizeId = dis.OrganizeId;
            ka.ParentID = Area.AreaParentId;
            ka.PointList = Area.PointList;
            ka.SortCode = dis.SortCode;
            SendData sd = new SendData();
            if (ID == "")
            {
                sd.DataName = "AddArea";
            }
            else
            {
                sd.DataName = "UpdateArea";
            }
            sd.EntityString = JsonConvert.SerializeObject(ka);

            //���°󶨵ı�ǩ��Ϣͬ������̨���������
            RabbitMQHelper rh = RabbitMQHelper.CreateInstance();
            rh.SendMessage(JsonConvert.SerializeObject(sd));
            return Success("�����ɹ���");
        }
        #endregion
    }
}
