using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System;
using System.Linq;
using ERCHTMS.Code;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Cache;
using ERCHTMS.Busines.BaseManage;
using System.Web;

namespace ERCHTMS.Web.Areas.HiddenTroubleManage.Controllers
{
    public class WfSettingController : MvcControllerBase
    {
        private WfInstanceBLL wfinstancebll = new WfInstanceBLL();
        private WfSettingBLL wfsettingbll = new WfSettingBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private OrganizeCache organizeCache = new OrganizeCache();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private RoleBLL rolebll = new RoleBLL();
        private WfConditionBLL wfconditionbll = new WfConditionBLL();
        private WfConditionAddtionBLL wfconditionaddtionbll = new WfConditionAddtionBLL();
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
            var data = wfsettingbll.GetList(queryJson);
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
            var data = wfsettingbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult InitPageDataJson()
        {
            Operator curUser = OperatorProvider.Provider.Current();
            //集合
            var Instance = wfinstancebll.GetList(""); //获取所有流程配置实例
            //返回值
            var josnData = new
            {
                Instance = Instance
            };
            return Content(josnData.ToJson());
        }

        [HttpGet]
        public ActionResult InitPageBaseDataJson(string queryJson)
        {
            Operator curUser = OperatorProvider.Provider.Current();
            //集合
            var Instance = wfinstancebll.GetList(queryJson); //获取所有流程配置实例
            //返回值
            var josnData = new
            {
                Instance = Instance
            };
            return Content(josnData.ToJson());
        }

        #endregion

        #region 流程配置实例列表
        /// <summary>
        /// 流程配置实例列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>   
        [HttpGet]
        public ActionResult GetWfSettingInfoPageList(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = wfsettingbll.GetWfSettingInfoPageList(pagination, queryJson);
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
            var condition = wfconditionbll.GetList(keyValue);
            foreach (WfConditionEntity centity in condition)
            {
                string conditionid = centity.ID;
                var addtion = wfconditionaddtionbll.GetList(conditionid);
                foreach (WfConditionAddtionEntity aentity in addtion)
                {
                    string addtionid = aentity.ID;
                    wfconditionaddtionbll.RemoveForm(addtionid);
                }
                wfconditionbll.RemoveForm(conditionid);
            }
            wfsettingbll.RemoveForm(keyValue);

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
        public ActionResult SaveForm(string keyValue, WfSettingEntity entity)
        {
            string flowcode = "bosafety";
            if (!string.IsNullOrEmpty(entity.FLOWCODE))
            {
                if (entity.FLOWCODE.Length >= 8)
                {
                    flowcode = entity.FLOWCODE.Substring(0, 8);
                }
            }
            if (!string.IsNullOrEmpty(entity.SCRIPTCURCONTENT))
            {
                entity.SCRIPTCURCONTENT = HttpCommon.Decrypt(entity.SCRIPTCURCONTENT.ToUpper(), flowcode, flowcode);
                entity.SCRIPTCURCONTENT = HttpUtility.UrlDecode(entity.SCRIPTCURCONTENT);
            }
            if (!string.IsNullOrEmpty(entity.SCRIPTCONTENT))
            {
                entity.SCRIPTCONTENT = HttpCommon.Decrypt(entity.SCRIPTCONTENT.ToUpper(), flowcode, flowcode);
                entity.SCRIPTCONTENT = HttpUtility.UrlDecode(entity.SCRIPTCONTENT);
            }
            wfsettingbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}