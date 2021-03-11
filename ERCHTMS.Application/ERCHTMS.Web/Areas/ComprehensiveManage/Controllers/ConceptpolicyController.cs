using ERCHTMS.Entity.ComprehensiveManage;
using ERCHTMS.Busines.ComprehensiveManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Busines.SystemManage;
using System.Linq;

namespace ERCHTMS.Web.Areas.ComprehensiveManage.Controllers
{
    /// <summary>
    /// 描 述：理念政策
    /// </summary>
    public class ConceptpolicyController : MvcControllerBase
    {
        private ConceptpolicyBLL conceptpolicybll = new ConceptpolicyBLL();

        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();

        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.ehsDepartCode = "";
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            DataItemModel ehsDepart = dataitemdetailbll.GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            if (ehsDepart != null)
                ViewBag.ehsDepartCode = ehsDepart.ItemValue;
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
            var data = conceptpolicybll.GetList(queryJson);
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
            var data = conceptpolicybll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取数据，如果库中不存在。添加一条默认，存在则取出显示
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetData(string keyValue,string HtmlStr)
        {
            //var data = conceptpolicybll.GetEntity(keyValue);
            var data = conceptpolicybll.GetList("");
            //if (data == null)
            //{
            //    //不存在则新增一条数据
            //    ConceptpolicyEntity entity = new ConceptpolicyEntity();
            //    entity.Content = HtmlStr;
            //    keyValue = null;
            //    conceptpolicybll.SaveForm(keyValue, entity);
            //}
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
            conceptpolicybll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, ConceptpolicyEntity entity)
        {
            conceptpolicybll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
