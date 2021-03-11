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
    /// 描 述：门禁使用用户表
    /// </summary>
    public class HikaccessuserController : MvcControllerBase
    {
        private HikaccessuserBLL hikaccessuserbll = new HikaccessuserBLL();

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
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = hikaccessuserbll.GetEntity(keyValue);
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
            hikaccessuserbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, HikaccessuserEntity entity)
        {
            hikaccessuserbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
