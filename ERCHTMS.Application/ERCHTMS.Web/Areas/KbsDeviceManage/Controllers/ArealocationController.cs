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
    /// 描 述：区域定位表
    /// </summary>
    public class ArealocationController : MvcControllerBase
    {
        private ArealocationBLL arealocationbll = new ArealocationBLL();

        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
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
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = arealocationbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            arealocationbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
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

            //将新绑定的标签信息同步到后台计算服务中
            RabbitMQHelper rh = RabbitMQHelper.CreateInstance();
            rh.SendMessage(JsonConvert.SerializeObject(sd));
            return Success("操作成功。");
        }
        #endregion
    }
}
