using ERCHTMS.Entity.RoutineSafetyWork;
using ERCHTMS.Busines.RoutineSafetyWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Code;
using ERCHTMS.Busines.BaseManage;

namespace ERCHTMS.Web.Areas.RoutineSafetyWork.Controllers
{
    /// <summary>
    /// 描 述：用户组管理
    /// </summary>
    public class UserGroupManageController : MvcControllerBase
    {
        private UserGroupManageBLL usergroupmanagebll = new UserGroupManageBLL();
        private UserBLL userBLL = new UserBLL();

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
        /// 选择用户组
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Select()
        {
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
            Operator user = OperatorProvider.Provider.Current();
            var queryParam = queryJson.ToJObject();
            pagination.p_kid = "ID";
            pagination.p_fields = "UserGroupName,UserId,UserName";
            pagination.p_tablename = "BIS_UserGroupManage t";
            pagination.conditionJson = string.Format(" createuserid='{0}'", user.UserId);
            var watch = CommonHelper.TimerStart();
            var data = usergroupmanagebll.GetPageList(pagination);
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
            var data = usergroupmanagebll.GetList();
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
            var data = usergroupmanagebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetTreeListJson(string queryJson)
        {
            Operator user = OperatorProvider.Provider.Current();
            List<UserGroupManageEntity> data = usergroupmanagebll.GetList().Where(a => a.CreateUserId == user.UserId).ToList();
            var treeList = new List<TreeEntity>();
            foreach (UserGroupManageEntity item in data)
            {
                TreeEntity tree = new TreeEntity();
                tree.id = item.Id;
                tree.text = item.UserGroupName;
                tree.value = item.Id;
                tree.isexpand = false;
                tree.parentId = "0";
                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson("0"));

        }
        /// <summary>
        /// 选择用户页面使用（不判断权限）
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetUserListJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "USERID";
            pagination.p_fields = "REALNAME,MOBILE,OrganizeName,ORGANIZEID,DEPTNAME,DEPARTMENTID,DEPARTMENTCODE,DUTYNAME,POSTNAME,ROLENAME,ROLEID,MANAGER,ENABLEDMARK,ENCODE,ACCOUNT,NICKNAME,HEADICON,GENDER,EMAIL,OrganizeCode";
            pagination.p_tablename = "V_USERINFO t";
            pagination.conditionJson = "Account!='System'";
            var watch = CommonHelper.TimerStart();
            var data = usergroupmanagebll.GetPageList(pagination, queryJson);
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
            usergroupmanagebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, UserGroupManageEntity entity)
        {
            usergroupmanagebll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
