using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Busines.PersonManage;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Entity.PersonManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.PersonManage.Controllers
{
    /// <summary>
    /// 外包人员离场审核
    /// </summary>
    public class LeaveApproveController : MvcControllerBase
    {
        private LeaveApproveBLL leaveApproveBLL = new LeaveApproveBLL();
        private ManyPowerCheckBLL manypowercheckbll = new ManyPowerCheckBLL();
        private DepartmentBLL departmentbll = new DepartmentBLL();
        private AptitudeinvestigateauditBLL aptitudeinvestigateauditBLL = new AptitudeinvestigateauditBLL();
        #region [视图功能]
        // GET: PersonManage/LeaveApprove
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 离场审批表单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LeaveApproveForm()
        {
            return View();
        }


        public ActionResult Flow()
        {
            return View();
        }
        #endregion
        #region [获取数据]
        /// <summary>
        /// 离场人员审核列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetLeaveApproveList(Pagination pagination, string queryJson)
        {
            pagination.conditionJson = " 1=1 ";
            var queryParam = queryJson.ToJObject();
            //根据后台配置查看数据权限
            string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "createuserdeptcode", "createuserorgcode");
            if (!string.IsNullOrEmpty(where))
            {
                pagination.conditionJson += " and " + where;
            }
            var watch = CommonHelper.TimerStart();
            var dt = leaveApproveBLL.GetLeaveApproveList(pagination, queryJson);
            string str = string.Empty;
            foreach (DataRow row in dt.Rows)
            {
                str = manypowercheckbll.GetApproveUserAccount(row["flowid"].ToString(), row["id"].ToString(), "", "", row["LeaveDeptId"].ToString());
                //获取审核人账号
                row["approveuseraccount"] = str;
            }
            var jsonData = new
            {
                rows = dt,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return ToJsonResult(jsonData);
        }

        /// <summary>
        /// 获取表单数据
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = new
            {
                apply=leaveApproveBLL.GetEntity(keyValue),
                approve= aptitudeinvestigateauditBLL.GetAuditList(keyValue).FirstOrDefault()
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        public ActionResult GetLeaveInfo(string userid)
        {
            DataTable dt = leaveApproveBLL.GetLeaveInfo(userid);
          return ToJsonResult(dt);
        }
        #region 获取流程图对象
        /// <summary>
        /// 获取流程图对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult GetWorkActionList(string keyValue)
        {
            try
            {
                var josnData = leaveApproveBLL.GetFlow(keyValue);
                return Content(josnData.ToJson());
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }

        }
        #endregion
        #endregion

        #region [提交数据]
        /// <summary>
        /// 外包人员离场
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Leave(LeaveApproveEntity entity)
        {
            try
            {
                bool flag= leaveApproveBLL.SaveForm(entity);
                return Content(flag.ToString().ToLower());
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 离场审批
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="aentity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ApproveForm(string keyValue, AptitudeinvestigateauditEntity aentity)
        {
            try
            {
                LeaveApproveEntity entity = leaveApproveBLL.GetEntity(keyValue);
                leaveApproveBLL.LeaveApprove(keyValue, entity, aentity);
                return Content("true");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion
    }
}