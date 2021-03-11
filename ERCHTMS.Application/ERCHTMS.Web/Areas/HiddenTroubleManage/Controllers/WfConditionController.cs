using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Web;
using ERCHTMS.Code;

namespace ERCHTMS.Web.Areas.HiddenTroubleManage.Controllers
{
    /// <summary>
    /// 描 述：流程配置条件表
    /// </summary>
    public class WfConditionController : MvcControllerBase
    {
        private WfConditionBLL wfconditionbll = new WfConditionBLL();
        private WfConditionAddtionBLL wfconditionaddtionbll = new WfConditionAddtionBLL(); //指定部门条件
        private WfConditionOfRoleBLL wfconditionofrolebll = new WfConditionOfRoleBLL(); //指定角色条件

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

        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddtionForm()
        {
            return View();
        }

        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BaseAddtiionForm()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BaseForm()
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
            var data = wfconditionbll.GetList(queryJson);
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
            var data = wfconditionbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion


        /// <summary>
        /// 获取指定部门的数据
        /// </summary>
        /// <param name="conditionId">条件id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAddtionListJson(string conditionId)
        {
            var data = wfconditionaddtionbll.GetList(conditionId);
            return ToJsonResult(data);
        }


        [HttpGet]
        public ActionResult GetAddtionRoleListJson(string addtionId)
        {
            var data = wfconditionofrolebll.GetList(addtionId);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetAddtionFormJson(string keyValue)
        {
            var data = wfconditionaddtionbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetAddtionRoleFormJson(string keyValue)
        {
            var data = wfconditionofrolebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        #region 流程配置实例列表
        /// <summary>
        /// 流程配置实例列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>   
        [HttpGet]
        public ActionResult GetWfConditionInfoPageList(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = wfconditionbll.GetWfConditionInfoPageList(pagination, queryJson);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return Content(JsonData.ToJson());
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
            var addtion = wfconditionaddtionbll.GetList(keyValue);
            foreach (WfConditionAddtionEntity aentity in addtion)
            {
                string addtionid = aentity.ID;
                wfconditionaddtionbll.RemoveForm(addtionid);
            }
            wfconditionbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, WfConditionEntity entity)
        {
            string flowcode = "bosafety";
            if (!string.IsNullOrEmpty(entity.SQLCONTENT))
            {
                entity.SQLCONTENT = HttpCommon.Decrypt(entity.SQLCONTENT.ToUpper(), flowcode, flowcode);
                entity.SQLCONTENT = HttpUtility.UrlDecode(entity.SQLCONTENT);
            }
            wfconditionbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
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
        public ActionResult RemoveAddtionForm(string keyValue)
        {
            wfconditionaddtionbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }


        /// <summary>
        /// 删除角色条件
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveAddtionRoleForm(string keyValue)
        {
            wfconditionofrolebll.RemoveForm(keyValue);
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
        public ActionResult SaveAddtionForm(string keyValue, WfConditionAddtionEntity entity)
        {
            wfconditionaddtionbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
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
        public ActionResult SaveAddtionRoleForm(string keyValue, WfConditionOfRoleEntity entity)
        {
            wfconditionofrolebll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}