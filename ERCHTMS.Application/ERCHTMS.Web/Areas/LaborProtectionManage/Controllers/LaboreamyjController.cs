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
    /// 描 述：劳动防护预警表
    /// </summary>
    public class LaboreamyjController : MvcControllerBase
    {
        private LaboreamyjBLL laboreamyjbll = new LaboreamyjBLL();

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
            var data = laboreamyjbll.GetList(queryJson).ToList();
            //获取字典中模拟的数据
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
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = laboreamyjbll.GetEntity(keyValue);
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
            laboreamyjbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string json)
        {
            laboreamyjbll.SaveForm(json);
            return Success("操作成功。");
        }
        #endregion
    }
}
