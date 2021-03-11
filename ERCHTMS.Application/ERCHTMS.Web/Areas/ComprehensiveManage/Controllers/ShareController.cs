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
    /// 描 述：亮点分享
    /// </summary>
    public class ShareController : MvcControllerBase
    {
        private ShareBLL sharebll = new ShareBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();

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
            ViewBag.ehsDepartCode = "";
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            DataItemModel ehsDepart = dataitemdetailbll.GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            if (ehsDepart != null)
                ViewBag.ehsDepartCode = ehsDepart.ItemValue;
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            queryJson = queryJson ?? "";
            pagination.p_kid = "Id";
            pagination.p_fields = "Theme,DeptName,ShareName,ShareIndex,IssueTime,IsSend,ReadNum,CreateUserId";
            pagination.p_tablename = "HRS_SHARE";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.conditionJson += string.Format(" and (IsSend='1' or createuserid='{0}')", user.UserId);
            var watch = CommonHelper.TimerStart();
            var data = sharebll.GetPageList(pagination, queryJson);
            var jsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return ToJsonResult(jsonData);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = sharebll.GetList(queryJson);
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
            var data = sharebll.GetEntity(keyValue);
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
            sharebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue,string isSend, ShareEntity entity)
        {
            entity.IsSend = isSend;//是否发送
            sharebll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
