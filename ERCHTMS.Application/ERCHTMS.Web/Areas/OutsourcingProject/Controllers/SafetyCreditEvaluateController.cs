using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using BSFramework.Util.Extension;
using System;
using System.Linq;
using ERCHTMS.Busines.BaseManage;
using System.Data;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// 安全信用评价
    /// </summary>
    public class SafetyCreditEvaluateController : MvcControllerBase
    {
        private SafetyCreditEvaluateBLL safetycreditevaluatebll = new SafetyCreditEvaluateBLL();
        private SafetyCreditScoreBLL Safetycreditscorebll = new SafetyCreditScoreBLL();
        private DepartmentBLL departmentbll = new DepartmentBLL();


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

        [HttpGet]
        public ActionResult ScoreForm()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ScoreStandardForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        ///  获取项目基本信息
        /// </summary>
        /// <param name="keyValue">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetEngineerDataById(string keyValue)
        {
            Operator currUser = OperatorProvider.Provider.Current();
            DataTable dt = safetycreditevaluatebll.GetEngineerDataById(keyValue);
            return ToJsonResult(dt);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            //var data = safetycreditevaluatebll.GetList(queryJson);
            //return ToJsonResult(data);
            pagination.p_kid = "t.ID";
            pagination.p_fields = @" e.fullname,r.ENGINEERNAME,t.EVALUATEDEPTNAME,t.ORIGINALSCORE,t.ACTUALSCORE,t.EVALUATESTATE,
t.EVALUATEDEPT,to_char(t.createevaluatetime,'yyyy-MM-dd') as createevaluatetime,t.modifyuserid,t.CREATEPERSON as createusername ,t.CREATEPERSONID ";
            pagination.p_tablename = @"epg_safetycreditevaluate t left join EPG_OutSouringEngineer r 
on t.projectid=r.id left join base_department e on r.outprojectid=e.departmentid";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string role = user.RoleName;
            //if (role.Contains("公司级用户") || role.Contains("厂级部门用户"))
            //{
            //    pagination.conditionJson = string.Format(" (t.createuserorgcode  = '{0}' and t.issend='1' or t.createuserid ='{1}')", user.OrganizeCode, user.UserId);
            //}
            //else if (role.Contains("承包商级用户"))
            //{
            //    pagination.conditionJson = string.Format(" (e.departmentid = '{0}' or r.SUPERVISORID='{0}' or t.createuserid ='{1}') ", user.DeptId, user.UserId);
            //}
            //else
            //{
            //    var deptentity = departmentbll.GetEntity(user.DeptId);
            //    while (deptentity.Nature == "班组" || deptentity.Nature == "专业")
            //    {
            //        deptentity = departmentbll.GetEntity(deptentity.ParentId);
            //    }
            //    pagination.conditionJson = string.Format(" (r.engineerletdeptid in (select departmentid from base_department where encode like '{0}%')  or t.createuserid='{1}') ", deptentity.EnCode, user.UserId);

            //    //pagination.conditionJson = string.Format(" (r.engineerletdeptid = '{0}' and t.issend='1' or t.createuserid ='{1}') ", user.DeptId, user.UserId);
            //}

            pagination.conditionJson = string.Format(" (t.CREATEUSERORGCODE  = '{0}')", user.OrganizeCode);

            var queryParam = queryJson.ToJObject();
            //时间范围
            if (!queryParam["sTime"].IsEmpty() || !queryParam["eTime"].IsEmpty())
            {
                string startTime = queryParam["sTime"].ToString();
                string endTime = queryParam["eTime"].ToString();
                if (queryParam["sTime"].IsEmpty())
                {
                    startTime = "1899-01-01";
                }
                if (queryParam["eTime"].IsEmpty())
                {
                    endTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
                // 需要修改时间
                pagination.conditionJson += string.Format(" and to_date(to_char(t.CREATEEVALUATETIME,'yyyy-MM-dd'),'yyyy-MM-dd') between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", startTime, endTime);
            }
            //查询条件
            if (!queryParam["condition"].IsEmpty() && !queryParam["txtSearch"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and {0} like '%{1}%'", queryParam["condition"].ToString(), queryParam["txtSearch"].ToString());
            }
            if (!queryParam["projectid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.projectid='{0}'", queryParam["projectid"].ToString());
            }

            var watch = CommonHelper.TimerStart();
            var data = safetycreditevaluatebll.GetList(pagination, queryJson);
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
        /// 获取安全评分列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetScoreListJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "t.ID";
            pagination.p_fields = @" t.SAFETYCREDITEVALUATEID,t.SCORETYPE,t.SCORE,to_char(t.SCORETIME,'yyyy-MM-dd') as SCORETIME,t.SCOREPERSON,t.SCOREPERSONID,t.REASON,t.EVALUATEDEPTNAME  ";
            pagination.p_tablename = @" EPG_SAFETYCREDITSCORE t ";
            
            var queryParam = queryJson.ToJObject();
            if (!queryParam["ID"].IsEmpty() && !queryParam["ID"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" t.SAFETYCREDITEVALUATEID = '{0}'", queryParam["ID"].ToString());
            }

            var watch = CommonHelper.TimerStart();
            var data = Safetycreditscorebll.GetList(pagination, queryJson);
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
        /// 获取评分标准
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetScoreStandardJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "t.ID";
            pagination.p_fields = @" t.SCOREREASON,t.SCORESTANDARD,t.STANDARDTYPE  ";
            pagination.p_tablename = @" EPG_SAFETYSCORESTANDARD t ";

            var queryParam = queryJson.ToJObject();
            if (!queryParam["STANDARDTYPE"].IsEmpty() && !queryParam["STANDARDTYPE"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" t.STANDARDTYPE = '{0}' and CREATEUSERORGCODE = '{1}' ", queryParam["STANDARDTYPE"].ToString(), OperatorProvider.Provider.Current().OrganizeCode);
            }

            var watch = CommonHelper.TimerStart();
            var data = Safetycreditscorebll.GetList(pagination, queryJson);
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
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = safetycreditevaluatebll.GetEntity(keyValue);
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
            string num = safetycreditevaluatebll.RemoveForm(keyValue);
            if (num == "0")
            {
                return Success("删除成功");
            }
            else
            {
                return Error("当前已产生评分记录，不能删除");
            }


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
        public ActionResult SaveForm(string keyValue, SafetyCreditEvaluateEntity entity)
        {
            safetycreditevaluatebll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }

        /// <summary>
        /// 保存分数表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveScoreForm(string keyValue, SafetyCreditScoreEntity entity)
        {
            SafetyCreditEvaluateEntity sceInfo = safetycreditevaluatebll.GetEntity(entity.SAFETYCREDITEVALUATEID);

            if (sceInfo.EVALUATESTATE == "1")
            {
                return Error("操作失败，已结束评价。");
            }
            else
            {
                Safetycreditscorebll.SaveForm(keyValue, entity);
                return Success("操作成功。");
            }

            
        }

        /// <summary>
        /// 提交分数算总分
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveScoreTotal(string keyValue)
        {
            Safetycreditscorebll.SaveScoreTotal(keyValue);
            return Success("操作成功。");
        }

        /// <summary>
        /// 删除分数数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveScoreForm(string keyValue)
        {
            Safetycreditscorebll.RemoveForm(keyValue);
            return Success("删除成功。");
        }

        /// <summary>
        /// 结束评价
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult FinishForm(string keyValue)
        {
            safetycreditevaluatebll.FinishForm(keyValue);
            return Success("操作成功。");
        }
        #endregion
	}
}