using System.Collections.Generic;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Busines.CarManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;

namespace ERCHTMS.Web.Areas.CarManage.Controllers
{
    /// <summary>
    /// 描 述：门禁点权限表
    /// </summary>
    public class HikaccessaurhorityController : MvcControllerBase
    {
        private HikaccessaurhorityBLL hikaccessaurhoritybll = new HikaccessaurhorityBLL();

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
        public ActionResult Personindex()
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
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            string orgcode = OperatorProvider.Provider.Current().OrganizeCode;
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "ID";
            pagination.p_fields = "DEVICENAME,OUTTYPE,AREANAME,AREAID,HIKID,StartTime,EndTime";
            pagination.p_tablename = @"BIS_HIKACCESSAURHORITY";
            pagination.conditionJson = " 1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            //if (user.IsSystem)
            //{
            pagination.conditionJson = "1=1";
            //}
            //else
            //{




            //    string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
            //    pagination.conditionJson += " and " + where;




            //}

            var data = hikaccessaurhoritybll.GetPageList(pagination, queryJson);

            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch),

            };
            return Content(JsonData.ToJson());
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = hikaccessaurhoritybll.GetEntity(keyValue);
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
            DataItemDetailBLL pdata = new DataItemDetailBLL();
            var pitem = pdata.GetItemValue("Hikappkey");//海康服务器密钥
            var url = pdata.GetItemValue("HikBaseUrl");//海康服务器地址
            hikaccessaurhoritybll.RemoveForm(keyValue, pitem,url);
            return Success("删除成功。");
        }
        /// <summary>
        /// 通过用户删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveUserForm(string keyValue)
        {
            DataItemDetailBLL pdata = new DataItemDetailBLL();
            var pitem = pdata.GetItemValue("Hikappkey");//海康服务器密钥
            var url = pdata.GetItemValue("HikBaseUrl");//海康服务器地址
            hikaccessaurhoritybll.RemoveUserForm(keyValue, pitem, url);
            return Success("删除成功。");
        }

        /// <summary>
        /// 门禁点权限下发
        /// </summary>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="DeptList">部门/人员列表</param>
        /// <param name="AccessList">门禁点列表</param>
        /// <param name="Type">状态 0为部门数据 1位人员数据</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string StartTime, string EndTime, List<Access> DeptList, List<Access> AccessList, int Type)
        {
            DataItemDetailBLL pdata = new DataItemDetailBLL();
            var pitem = pdata.GetItemValue("Hikappkey");//海康服务器密钥
            var url = pdata.GetItemValue("HikBaseUrl");//海康服务器地址
            hikaccessaurhoritybll.SaveForm(StartTime, EndTime, DeptList, AccessList, Type, pitem, url);
            return Success("操作成功。");
        }
        #endregion
    }
}
