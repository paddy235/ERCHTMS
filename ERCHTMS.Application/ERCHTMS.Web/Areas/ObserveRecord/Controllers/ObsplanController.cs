
using ERCHTMS.Busines.Observerecord;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Busines.SystemManage;
using System.Linq;
using ERCHTMS.Entity.Observerecord;
using System;
using System.Web;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Web.Areas.Observerecord.Controllers
{
    /// <summary>
    /// 描 述：观察计划
    /// </summary>
    public class ObsplanController : MvcControllerBase
    {
        private ObsplanBLL obsplanbll = new ObsplanBLL();
        private ObsplanworkBLL obsplanworkbll = new ObsplanworkBLL();
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
            return View();
        }
        [HttpGet]
        public ActionResult FeedBackForm()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CopyPlanIndex()
        {
            return View();
        }
        /// <summary>
        /// 台账页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult StandingIndex()
        {
            return View();
        }
        /// <summary>
        /// 台账详情页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult StandingShow()
        {
            return View();
        }
        /// <summary>
        ///导入页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ImportPlanData()
        {
            return View();
        }

        /// <summary>
        ///选择观察计划
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SelectObsPlan()
        {
            return View();
        }
        #endregion

        #region 获取数据

        /// <summary>
        /// 台账页面获取分页列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetStandingPageJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var tableClass = "bis_obsplan_tz t";
            pagination.p_kid = "t.id tid";
            pagination.p_fields = @" t.planyear,
                                       t.plandept,
                                       t.planspeciaty,
                                       t.plandeptcode,t.plandeptid,
                                       t.planspeciatycode,
                                       t.planarea,t.planareacode,
                                       t.planlevel,p.risklevel,
                                       p.workname fjname,
                                       t.workname,p.id pid,t.oldplanid,
                                       p.obsperson,p.oldworkid,
                                       p.obspersonid,
                                       p.obsnum,p.obsnumtext,
                                       p.obsmonth,
                                       t.createuserid,
                                       t.createuserdeptcode,
                                       t.createuserorgcode,
                                       t.createdate,t.iscommit,p.remark,null status";

            Operator currUser = OperatorProvider.Provider.Current();
            pagination.conditionJson = "t.iscommit='1' and t.ispublic ='1'";
            //DataItemModel ehsDepart = dataitemdetailbll.GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == currUser.OrganizeId&&p.ItemValue==currUser.DeptCode).ToList().FirstOrDefault();
            //if (ehsDepart != null) {
            //    tableClass = "bis_obsplan_commitehs t";
            //}
            //if (currUser.RoleName.Contains("厂级部门"))
            //{
            //    tableClass = "bis_obsplan_commitehs t";
            //}
            //if (currUser.RoleName.Contains("公司级") && currUser.RoleName.Contains("安全管理员"))
            //{
            //    tableClass = "bis_obsplan_fb t";
            //}
            pagination.p_tablename = string.Format(@"{0}
                                        left join bis_obsplanwork p
                                            on p.planid = t.id", tableClass);
            pagination.sidx = pagination.sidx + " " + pagination.sord + ",t.id";
            if (!currUser.IsSystem)
            {
                if (currUser.RoleName.Contains("专业级用户") || currUser.RoleName.Contains("班组级用户"))
                {
                    var d = new DepartmentBLL().GetParentDeptBySpecialArgs(currUser.ParentId, "部门");
                    if (d != null)
                    {
                        pagination.conditionJson += " and t.plandeptcode like '" + d.EnCode + "%'";
                    }
                    else
                    {
                        pagination.conditionJson += " and 0=1";
                    }
                }
                else
                {
                    string authType = new AuthorizeBLL().GetOperAuthorzeType(currUser, "e4097233-5867-4c46-bba9-f052d512ffd8", "search");
                    if (!string.IsNullOrEmpty(authType))
                    {

                        switch (authType)
                        {
                            case "1":
                                pagination.conditionJson += " and  t.createuserid='" + currUser.UserId + "'";
                                break;
                            case "2":
                                pagination.conditionJson += " and t.plandeptcode='" + currUser.DeptCode + "'";
                                break;
                            case "3":
                                pagination.conditionJson += " and t.plandeptcode like '" + currUser.DeptCode + "%'";
                                break;
                            case "4":
                                pagination.conditionJson += " and t.plandeptcode like '" + currUser.OrganizeCode + "%'";
                                break;
                            case "5":
                                pagination.conditionJson += string.Format(" and t.plandeptcode in(select encode from BASE_DEPARTMENT where deptcode like '{0}%')", currUser.NewDeptCode);
                                break;
                        }
                    }
                    else
                    {
                        pagination.conditionJson += " and 0=1";
                    }
                }
            }
            var queryParam = queryJson.ToJObject();
            var PlanYear = queryParam["PlanYear"].ToString();
            var data = obsplanbll.GetPageList(pagination, queryJson);
            for (int i = 0; i < data.Rows.Count; i++)
            {
                var dt = obsplanbll.GetObsRecordIsExist(data.Rows[i]["oldplanid"].ToString(), data.Rows[i]["oldworkid"].ToString(), PlanYear);
                var obsmonth = data.Rows[i]["obsmonth"].ToString().Split(',');
                if (dt.Rows.Count > 0)
                {
                    for (int j = 0; j < obsmonth.Length; j++)
                    {

                        if (DateTime.Now.Year.ToString() == PlanYear && DateTime.Now.Month <= Convert.ToInt32(obsmonth[j]))
                        {
                            if (Convert.ToInt32(dt.Rows[0]["m" + obsmonth[j]]) > 0)
                            {
                                data.Rows[i]["status"] += "1,";
                            }
                            else
                            {
                                if (DateTime.Now.Month == Convert.ToInt32(obsmonth[j]))
                                {
                                    var currTime = DateTime.Now;
                                    var lastTime = Convert.ToDateTime(DateTime.Now.AddMonths(1).ToString("yyyy-MM-01")).AddDays(-1);
                                    if (DateTime.Compare(lastTime, currTime) <= 5)
                                    {
                                        data.Rows[i]["status"] += "2,";
                                    }
                                    else
                                    {
                                        data.Rows[i]["status"] += "3,";
                                    }
                                }
                                else
                                {
                                    data.Rows[i]["status"] += "3,";
                                }
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[0]["m" + obsmonth[j]]) > 0)
                            {
                                data.Rows[i]["status"] += "1,";
                            }
                            else
                            {
                                data.Rows[i]["status"] += "4,";
                            }
                        }

                    }
                    if (!string.IsNullOrWhiteSpace(data.Rows[i]["status"].ToString()))
                    {
                        data.Rows[i]["status"] = data.Rows[i]["status"].ToString().Substring(0, data.Rows[i]["status"].ToString().Length - 1);
                    }
                }
                else
                {
                    for (int j = 0; j < obsmonth.Length; j++)
                    {

                        if (DateTime.Now.Year.ToString() == PlanYear && DateTime.Now.Month <= Convert.ToInt32(obsmonth[j]))
                        {
                            if (DateTime.Now.Month == Convert.ToInt32(obsmonth[j]))
                            {
                                var currTime = DateTime.Now;
                                var lastTime = Convert.ToDateTime(DateTime.Now.AddMonths(1).ToString("yyyy-MM-01")).AddDays(-1);
                                if (DateTime.Compare(lastTime, currTime) <= 5)
                                {
                                    data.Rows[i]["status"] += "2,";
                                }
                                else
                                {
                                    data.Rows[i]["status"] += "3,";
                                }
                            }
                            else
                            {
                                data.Rows[i]["status"] += "3,";
                            }
                        }
                        else
                        {

                            data.Rows[i]["status"] += "4,";
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(data.Rows[i]["status"].ToString()))
                    {
                        data.Rows[i]["status"] = data.Rows[i]["status"].ToString().Substring(0, data.Rows[i]["status"].ToString().Length - 1);
                    }
                }
            }
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
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var tableClass = "bis_obsplan t";
            pagination.p_kid = "t.id tid";
            pagination.p_fields = @" t.planyear,
                                       t.plandept,
                                       t.planspeciaty,
                                       t.plandeptcode,
                                       t.planspeciatycode,
                                       t.planarea,
                                       t.planlevel,p.risklevel,
                                       p.workname fjname,
                                       t.workname,
                                       p.obsperson,
                                       p.obspersonid,
                                       p.obsnum,p.obsnumtext,
                                       p.obsmonth,t.ispublic,
                                       t.createuserid,
                                       t.createuserdeptcode,
                                       t.createuserorgcode,
                                       t.createdate,t.iscommit,p.remark";

            Operator currUser = OperatorProvider.Provider.Current();
            pagination.conditionJson = "1=1";
            //DataItemModel ehsDepart = dataitemdetailbll.GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == currUser.OrganizeId&&p.ItemValue==currUser.DeptCode).ToList().FirstOrDefault();
            //if (ehsDepart != null) {
            //    tableClass = "bis_obsplan_commitehs t";
            //}
            if (currUser.RoleName.Contains("厂级部门"))
            {
                pagination.p_fields += ",t.oldplanid";
                tableClass = "bis_obsplan_commitehs t";
            }
            if (currUser.RoleName.Contains("公司级"))
            {
                pagination.p_fields += ",t.oldplanid";
                tableClass = "bis_obsplan_fb t";
            }
            pagination.p_tablename = string.Format(@"{0}
                                        left join bis_obsplanwork p
                                            on p.planid = t.id", tableClass);
            pagination.sidx = pagination.sidx + " " + pagination.sord + ",t.id";
            if (!currUser.IsSystem)
            {
                if (currUser.RoleName.Contains("专业级用户") || currUser.RoleName.Contains("班组级用户"))
                {
                    var d = new DepartmentBLL().GetParentDeptBySpecialArgs(currUser.ParentId, "部门");
                    if (d != null)
                    {
                        pagination.conditionJson += " and t.plandeptcode like '" + d.EnCode + "%'";
                    }
                    else
                    {
                        pagination.conditionJson += " and 0=1";
                    }
                }
                else
                {
                    string authType = new AuthorizeBLL().GetOperAuthorzeType(currUser, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                    if (!string.IsNullOrEmpty(authType))
                    {

                        switch (authType)
                        {
                            case "1":
                                pagination.conditionJson += " and  t.createuserid='" + currUser.UserId + "'";
                                break;
                            case "2":
                                pagination.conditionJson += " and t.plandeptcode='" + currUser.DeptCode + "'";
                                break;
                            case "3":
                                pagination.conditionJson += " and t.plandeptcode like '" + currUser.DeptCode + "%'";
                                break;
                            case "4":
                                pagination.conditionJson += " and t.plandeptcode like '" + currUser.OrganizeCode + "%'";
                                break;
                            case "5":
                                pagination.conditionJson += string.Format(" and t.plandeptcode in(select encode from BASE_DEPARTMENT where deptcode like '{0}%')", currUser.NewDeptCode);
                                break;
                        }
                    }
                    else
                    {
                        pagination.conditionJson += " and 0=1";
                    }
                }

            }
            var data = obsplanbll.GetPageList(pagination, queryJson);
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
        /// 获取意见列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetFeedBackList(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var tableClass = "bis_obsfeedback t";
            pagination.p_kid = "t.id";
            pagination.p_fields = @" t.acceptdept,
                                       t.acceptdeptcode,
                                       t.suggest,
                                       t.acceptdeptid,
                                       t.createuserid, t.createusername,
                                       t.createuserdeptcode,
                                       t.createuserorgcode,
                                       t.createdate";

            Operator currUser = OperatorProvider.Provider.Current();
            pagination.conditionJson = "1=1";

            if (currUser.RoleName.Contains("厂级部门"))
            {
                tableClass = "bis_obsfeedback_ehs t";
            }
            if (currUser.RoleName.Contains("公司级") && currUser.RoleName.Contains("安全管理员"))
            {
                tableClass = "bis_obsfeedback_fb t";
            }

            pagination.p_tablename = string.Format(@"{0}", tableClass);
            if (currUser.RoleName.Contains("专业级用户") || currUser.RoleName.Contains("班组级用户"))
            {
                var d = new DepartmentBLL().GetParentDeptBySpecialArgs(currUser.ParentId, "部门");
                if (d != null)
                {
                    pagination.conditionJson += " and t.acceptdeptcode like '" + d.EnCode + "%'";
                }
                else
                {
                    pagination.conditionJson += " and 0=1";
                }
            }
            var data = obsplanbll.GetPageList(pagination, queryJson);
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
            var data = obsplanbll.GetList(queryJson);
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
            Operator currUser = OperatorProvider.Provider.Current();
            //var data = obsplanbll.GetEntity(keyValue);
            if (currUser.RoleName.Contains("厂级部门"))
            {
                var data = obsplanbll.GetEHSEntity(keyValue);
                return ToJsonResult(data);
            }
            if (currUser.RoleName.Contains("公司级"))
            {
                var data = obsplanbll.GetFBEntity(keyValue);
                return ToJsonResult(data);
            }
            var data1 = obsplanbll.GetEntity(keyValue);
            return ToJsonResult(data1);
        }
        /// <summary>
        /// 台账页面详情获取
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetStandingFormJson(string keyValue)
        {
            var data = obsplanbll.GetTZEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 根据观察计划Id与任务分解Id获取相应信息
        /// </summary>
        /// <param name="PlanId">计划id </param>
        /// <param name="PlanFjId">任务分解Id</param>
        /// <param name="PlanMonth">观察月份</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPlanById(string PlanId, string PlanFjId, string PlanMonth)
        {
            var data = obsplanbll.GetPlanById(PlanId, PlanFjId, PlanMonth);
            return ToJsonResult(data);
        }


         /// <summary>
         /// 查询上一级节点
         /// </summary>
         /// <param name="parentid"></param>
         /// <param name="nature"></param>
         /// <returns></returns>
        public ActionResult GetParentDeptBySpecialArgs(string parentid, string nature)
        {

            var deptEntity= new DepartmentBLL().GetParentDeptBySpecialArgs(parentid, nature);
            return ToJsonResult(deptEntity);
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
            obsplanbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, ObsplanEntity entity)
        {
            var worklist = obsplanworkbll.GetList().Where(x => x.PlanId == keyValue).ToList();
            for (int i = 0; i < worklist.Count; i++)
            {
                if (worklist[i].RiskLevel == "IV级" || worklist[i].RiskLevel == "V级") {
                    entity.PlanLevel = "公司级";
                    break;
                }
            }
            obsplanbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveEHSForm(string keyValue, ObsplanEHSEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.PlanLevel)) {
                var worklist = obsplanworkbll.GetList().Where(x => x.PlanId == entity.ID).ToList();
                for (int i = 0; i < worklist.Count; i++)
                {
                    if (worklist[i].RiskLevel == "IV级" || worklist[i].RiskLevel == "V级")
                    {
                        entity.PlanLevel = "公司级";
                        break;
                    }
                }
            }
            obsplanbll.SaveEHSForm(keyValue, entity);
            return Success("操作成功。");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveFbForm(string keyValue, ObsplanFBEntity entity)
        {
            obsplanbll.SaveFBForm(keyValue, entity);
            return Success("操作成功。");
        }
        #region 意见保存
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveFeedBackForm(string keyValue, ObsFeedBackEntity entity)
        {
            obsplanbll.SaveFeedBackForm(keyValue, entity);
            return Success("操作成功。");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveFeedBackEHSForm(string keyValue, ObsFeedBackEHSEntity entity)
        {
            //上级添加意见向下同步
            obsplanbll.SaveFeedBackEHSForm(keyValue, entity);
            ObsFeedBackEntity feedback = new ObsFeedBackEntity();
            feedback.AcceptDept = entity.AcceptDept;
            feedback.AcceptDeptId = entity.AcceptDeptId;
            feedback.AcceptDeptCode = entity.AcceptDeptCode;
            feedback.Suggest = entity.Suggest;
            obsplanbll.SaveFeedBackForm("", feedback);
            return Success("操作成功。");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveFeedBackFbForm(string keyValue, ObsFeedBackFBEntity entity)
        {
            //上级添加意见向下同步
            obsplanbll.SaveFeedBackFBForm(keyValue, entity);
            ObsFeedBackEHSEntity ehs = new ObsFeedBackEHSEntity();
            ObsFeedBackEntity feedback = new ObsFeedBackEntity();
            ehs.AcceptDept = feedback.AcceptDept = entity.AcceptDept;
            ehs.AcceptDeptId = feedback.AcceptDeptId = entity.AcceptDeptId;
            ehs.AcceptDeptCode = feedback.AcceptDeptCode = entity.AcceptDeptCode;
            ehs.Suggest = feedback.Suggest = entity.Suggest;
            obsplanbll.SaveFeedBackEHSForm("", ehs);
            obsplanbll.SaveFeedBackForm("", feedback);
            return Success("操作成功。");
        }
        #endregion
        /// <summary>
        /// 提交到上一级
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult CommitEhsData()
        {
            Operator currUser = OperatorProvider.Provider.Current();
            if (obsplanbll.CommitEhsData(currUser))
            {
                return Success("操作成功。");
            }
            else
            {
                return Error("操作失败。");
            }
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult SetPlanLevelSave(string keyValue, string OldPlanId,string PlanLevel) {
            var ehsEntity = obsplanbll.GetEHSEntity(keyValue);
            if (ehsEntity != null)
            {
                ehsEntity.PlanLevel = PlanLevel;
                if (!string.IsNullOrWhiteSpace(OldPlanId))
                {
                    var oldEntity = obsplanbll.GetEntity(OldPlanId);
                    if (oldEntity != null)
                    {
                        oldEntity.PlanLevel = PlanLevel;
                        obsplanbll.SaveForm(oldEntity.ID, oldEntity);
                    }
                }
                obsplanbll.SaveEHSForm(ehsEntity.ID, ehsEntity);
                return Success("操作成功。");
            }
            else {
                return Error("获取数据失败。");
            }
           
            
        
        }

        /// <summary>
        /// 观察计划导出
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult ExportData(string queryJson, string fileName)
        {
            Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
            string fName = "观察计划_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            wb.Open(Server.MapPath("~/Resource/ExcelTemplate/年度安全行为观察工作计划导出模板.xlsx"));
            var queryParam = queryJson.ToJObject();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 10000000;
            pagination.sidx = "t.createdate";
            pagination.sord = "desc";

            pagination.p_kid = "t.id";
            pagination.p_fields = @"t.plandept,t.planarea,t.workname,t.planlevel,p.workname fjname,p.risklevel,
                                       p.obsnumtext,p.obsperson,null m1,null m2,null m3,null m4,null m5,null m6,
                                        null m7,null m8,null m9,null m10,null m11,null m12,p.remark,p.obsmonth,p.id wid,t.oldplanid,p.oldworkid";
            pagination.p_tablename = string.Format(@"bis_obsplan_tz t left join bis_obsplanwork p on p.planid = t.id");
            pagination.conditionJson = "1=1";
            if (!user.IsSystem)
            {
                if (user.RoleName.Contains("专业级用户") || user.RoleName.Contains("班组级用户"))
                {
                    var d = new DepartmentBLL().GetList().Where(x => x.DepartmentId == user.ParentId).FirstOrDefault();
                    if (d != null)
                    {
                        if (d.Nature == "部门")
                        {
                            pagination.conditionJson += " and t.plandeptcode like '" + d.EnCode + "%'";
                        }
                        else
                        {
                            var d1 = new DepartmentBLL().GetList().Where(x => x.DepartmentId == d.ParentId).FirstOrDefault();
                            if (d1.Nature == "部门")
                            {
                                pagination.conditionJson += " and t.plandeptcode like '" + d1.EnCode + "%'";
                            }
                            else
                            {
                                pagination.conditionJson += " and 0=1";
                            }
                        }
                    }
                    else
                    {
                        pagination.conditionJson += " and 0=1";
                    }
                }
                else
                {
                    //根据当前用户对模块的权限获取记录
                    string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "t.createuserdeptcode", "t.createuserorgcode");
                    if (!string.IsNullOrEmpty(where))
                    {
                        pagination.conditionJson += " and " + where;
                    }
                }
            }
            var data = obsplanbll.GetPageList(pagination, queryJson);
            //查询观察记录是否意见进行了记录
            for (int i = 0; i < data.Rows.Count; i++)
            {
                var dt = obsplanbll.GetObsRecordIsExist(data.Rows[i]["oldplanid"].ToString(), data.Rows[i]["oldworkid"].ToString(), queryParam["PlanYear"].ToString());
                var obsmonth = data.Rows[i]["obsmonth"].ToString().Split(',');
                if (dt.Rows.Count > 0)
                {
                    for (int j = 0; j < obsmonth.Length; j++)
                    {

                        if (DateTime.Now.Year.ToString() == queryParam["PlanYear"].ToString() && DateTime.Now.Month <= Convert.ToInt32(obsmonth[j]))
                        {
                            if (Convert.ToInt32(dt.Rows[0]["m" + obsmonth[j]]) > 0)
                            {
                                data.Rows[i]["m" + obsmonth[j]] = "√";
                            }
                            else
                            {
                                data.Rows[i]["m" + obsmonth[j]] = "□";
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[0]["m" + obsmonth[j]]) > 0)
                            {
                                data.Rows[i]["m" + obsmonth[j]] = "√";
                            }
                            else
                            {
                                data.Rows[i]["m" + obsmonth[j]] = "×";
                            }
                        }

                    }
                }
                else
                {
                    for (int j = 0; j < obsmonth.Length; j++)
                    {

                        if (DateTime.Now.Year.ToString() == queryParam["PlanYear"].ToString() && DateTime.Now.Month <= Convert.ToInt32(obsmonth[j]))
                        {
                            data.Rows[i]["m" + obsmonth[j]] = "□";
                        }
                        else
                        {

                            data.Rows[i]["m" + obsmonth[j]] = "×";
                        }
                    }
                }
            }
            var cells = wb.Worksheets[0].Cells;
            int Colnum = data.Columns.Count;
            int Rownum = data.Rows.Count;
            for (int i = 0; i < Rownum; i++)
            {
                for (int k = 0; k < Colnum - 5; k++)
                {
                    if (k == 0)
                    {
                        cells[4 + i, k].PutValue(i + 1);
                    }
                    else
                    {
                        cells[4 + i, k].PutValue(data.Rows[i][k].ToString());
                    }
                }
            }
            int q = 0;
            int RowOrder = 0;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                RowOrder = data.Select(string.Format("id='{0}'", data.Rows[i]["id"].ToString())).ToList().Count;
                cells.Merge(4 + q, 0, RowOrder, 1);
                cells.Merge(4 + q, 1, RowOrder, 1);
                cells.Merge(4 + q, 2, RowOrder, 1);
                cells.Merge(4 + q, 3, RowOrder, 1);
                cells.Merge(4 + q, 4, RowOrder, 1);
                q += RowOrder;
            }
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            System.Threading.Thread.Sleep(400);
            wb.Save(Server.MapPath("~/Resource/Temp/" + fName));
            return Success("导出成功。", fName);
        }
        /// <summary>
        /// 导入观察计划
        /// </summary>
        /// <returns></returns>
        [HandlerLogin(LoginMode.Ignore)]
        [HandlerAuthorize(PermissionMode.Ignore)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(5, "导入观察计划")]
        public string ImportPlanDataList()
        {
            int error = 0;
            string message = "请选择格式正确的文件再导入!";

            string falseMessage = "";
            int count = HttpContext.Request.Files.Count;
            if (count > 0)
            {
                count = 0;
                HttpPostedFileBase file = HttpContext.Request.Files[0];
                if (string.IsNullOrEmpty(file.FileName))
                {
                    return message;
                }
                if (!(file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xls") || file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xlsx")))
                {
                    return message;
                }
                
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName));
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                wb.Open(Server.MapPath("~/Resource/temp/" + fileName), file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xlsx") ? Aspose.Cells.FileFormatType.Excel2007Xlsx : Aspose.Cells.FileFormatType.Excel2003);
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                //判断表头是否正确,以免使用错误模板
                var sheet = wb.Worksheets[0];
                if (sheet.Cells[2, 1].StringValue != "专业" || sheet.Cells[2, 2].StringValue != "区域" || sheet.Cells[2, 3].StringValue != "作业内容" || sheet.Cells[2, 4].StringValue != "计划年度"
                    || sheet.Cells[2, 5].StringValue != "任务分解" || sheet.Cells[2, 6].StringValue != "风险等级" || sheet.Cells[2, 7].StringValue != "观察频率"
                    || sheet.Cells[2, 8].StringValue != "观察人员" || sheet.Cells[2, 9].StringValue != "计划观察月份" || sheet.Cells[2, 10].StringValue != "备注")
                {
                    return message;
                }
                
               
                var ObsEhsPlan = new List<ObsplanEHSEntity>();
                var ObsPlan = new List<ObsplanEntity>();
                var ObsPlanWork = new List<ObsplanworkEntity>();

                for (int i = 3; i <= sheet.Cells.MaxDataRow; i++)
                {
                    //区域、作业内容为必填字段
                    if (user.RoleName.Contains("厂级部门"))
                    {
                        var plan = new ObsplanEHSEntity();
                        plan.Create();
                        plan.PlanDept = user.DeptName;
                        plan.PlanDeptCode = user.DeptCode;
                        plan.PlanDeptId = user.DeptId;
                        plan.IsPublic = "0";
                        plan.PlanSpeciaty = sheet.Cells[i, 1].StringValue;
                        plan.WorkName = sheet.Cells[i, 3].StringValue;
                        plan.PlanArea = sheet.Cells[i, 2].StringValue;
                        plan.PlanYear = sheet.Cells[i, 4].StringValue;
                        plan.Iscommit = "0";
                        if (string.IsNullOrEmpty(sheet.Cells[i, 3].StringValue))
                        {
                            plan.ID = ObsEhsPlan[i - 3 - 1].ID;
                        }
                        ObsEhsPlan.Add(plan);
                    }
                    else {
                        var plan = new ObsplanEntity();
                        plan.Create();
                        plan.PlanDept = user.DeptName;
                        plan.PlanDeptCode = user.DeptCode;
                        plan.PlanDeptId = user.DeptId;
                        plan.IsEmsCommit = "0";
                        plan.IsPublic = "0";
                        plan.PlanSpeciaty = sheet.Cells[i, 1].StringValue;
                        plan.WorkName = sheet.Cells[i, 3].StringValue;
                        plan.PlanArea = sheet.Cells[i, 2].StringValue;
                        plan.PlanYear = sheet.Cells[i, 4].StringValue;
                        plan.Iscommit = "0";
                        if (string.IsNullOrEmpty(sheet.Cells[i, 3].StringValue))
                        {
                            plan.ID = ObsPlan[i - 3 - 1].ID;
                        }
                        ObsPlan.Add(plan);
                    }
                }
                for (int i = 3; i <= sheet.Cells.MaxDataRow; i++)
                {
                    //风险等级、观察人员、计划观察月份为必填字段
                    var dentity = new ObsplanworkEntity();
                    dentity.WorkName = sheet.Cells[i, 5].StringValue;
                    if (!string.IsNullOrWhiteSpace(sheet.Cells[i, 6].StringValue))
                    {
                        dentity.RiskLevel = sheet.Cells[i, 6].StringValue;
                    }
                    else
                    {
                        falseMessage += "</br>" + "第" + (i + 1) + "行风险等级不能为空,未能导入.";
                        error++;
                        continue;
                    }
                    dentity.ObsNumText = sheet.Cells[i, 7].StringValue.Replace('，', ',');
                    switch (sheet.Cells[i, 7].StringValue.Replace('，', ','))
                    {
                        case "选择性观察":
                            dentity.ObsNum = "I级";
                            break;
                        case "1次/半年,以发生作业为准":
                            dentity.ObsNum = "II级";
                            break;
                        case "1次/季度,以发生作业为准":
                            dentity.ObsNum = "III级";
                            break;
                        case "1次/月,以发生作业为准":
                            dentity.ObsNum = "IV级";
                            break;
                        case "每次观察,以发生作业为准":
                            dentity.ObsNum = "V级";
                            break;
                        default:
                            break;
                    }
                    if (!string.IsNullOrWhiteSpace(sheet.Cells[i, 8].StringValue))
                    {
                        var array = sheet.Cells[i, 8].StringValue.Split(',');
                        for (int h = 0; h < array.Length; h++)
                        {
                            var u = new UserBLL().GetList().Where(x => x.RealName == array[h] && x.OrganizeId == user.OrganizeId).FirstOrDefault();
                            if (u != null)
                            {
                                dentity.ObsPerson += u.RealName + ",";
                                dentity.ObsPersonId += u.UserId + ",";
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(dentity.ObsPerson))
                        {
                            dentity.ObsPerson = dentity.ObsPerson.Substring(0, dentity.ObsPerson.Length - 1);
                        }
                        if (!string.IsNullOrWhiteSpace(dentity.ObsPersonId))
                        {
                            dentity.ObsPersonId = dentity.ObsPersonId.Substring(0, dentity.ObsPersonId.Length - 1);
                        }
                    }
                    else
                    {
                        falseMessage += "</br>" + "第" + (i + 1) + "行观察人员不能为空,未能导入.";
                        error++;
                        continue;
                    }
                    if (!string.IsNullOrWhiteSpace(sheet.Cells[i, 9].StringValue))
                    {
                        var monthArr = sheet.Cells[i, 9].StringValue.Split(',');
                        //判断是否输入的为数字
                        for (int m = 0; m < monthArr.Length; m++)
                        {
                            int num = 0;
                            if (Int32.TryParse(monthArr[m], out num))
                            {
                                dentity.ObsMonth += num + ",";
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(dentity.ObsMonth))
                        {
                            dentity.ObsMonth = dentity.ObsMonth.Substring(0, dentity.ObsMonth.Length - 1);
                        }
                        else
                        {
                            falseMessage += "</br>" + "第" + (i + 1) + "行计划观察月份输入格式不正确,未能导入.";
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += "</br>" + "第" + (i + 1) + "行计划观察月份不能为空,未能导入.";
                        error++;
                        continue;
                    }
                    dentity.Remark = sheet.Cells[i, 10].StringValue;
                    dentity.Create();
                    if (user.RoleName.Contains("厂级部门"))
                    {
                        dentity.PlanId = ObsEhsPlan[i - 3].ID;
                    }
                    else {
                        dentity.PlanId = ObsPlan[i - 3].ID;
                    }
                    
                    ObsPlanWork.Add(dentity);
                }
                if (user.RoleName.Contains("厂级部门"))
                {
                    ObsEhsPlan = ObsEhsPlan.Where(x => x.WorkName != "").ToList();
                    count = ObsEhsPlan.Count;
                    int countNum = 0;
                    for (int i = 0; i < ObsEhsPlan.Count; i++)
                    {
                        //风险等级为IV或者V级 ,观察计划的计划等级为公司级
                        var r = ObsPlanWork.Where(x => x.PlanId == ObsEhsPlan[i].ID).Where(x => x.RiskLevel == "IV级" || x.RiskLevel == "V级").FirstOrDefault();
                        if (r != null)
                        {
                            ObsEhsPlan[i].PlanLevel = "公司级";
                        }
                        else
                        {
                            ObsEhsPlan[i].PlanLevel = "部门级";
                        }

                        if (!string.IsNullOrWhiteSpace(ObsEhsPlan[i].PlanSpeciaty))
                        {
                            var s = new DataItemDetailBLL().GetDataItemListByItemCode("'SpecialtyType'").Where(x => x.ItemName == ObsEhsPlan[i].PlanSpeciaty).FirstOrDefault();
                            if (s != null)
                            {
                                ObsEhsPlan[i].PlanSpeciatyCode = s.ItemValue;
                            }
                            else
                            {
                                falseMessage += "</br>" + "第" + (countNum + 1) + "行专业填写错误,未能导入.";
                                error++;
                                ObsEhsPlan.Remove(ObsEhsPlan[i]);
                                i--;
                                countNum++;
                                continue;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(ObsEhsPlan[i].PlanArea))
                        {
                            DistrictEntity disEntity = new DistrictBLL().GetDistrict(user.OrganizeId, ObsEhsPlan[i].PlanArea);
                            if (disEntity == null)
                            {
                                //电厂没有该区域则不赋值
                                ObsEhsPlan[i].PlanArea = "";
                            }
                            else
                            {
                                ObsEhsPlan[i].PlanAreaCode = disEntity.DistrictID;
                            }
                        }
                        else
                        {
                            falseMessage += "</br>" + "第" + (countNum + 1) + "行区域不能为空,未能导入.";
                            error++;
                            ObsEhsPlan.Remove(ObsEhsPlan[i]);
                            i--;
                            countNum++;
                            continue;
                        }
                        if (!string.IsNullOrWhiteSpace(ObsEhsPlan[i].WorkName))
                        {

                        }
                        else
                        {
                            falseMessage += "</br>" + "第" + (countNum + 1) + "行作业内容不能为空,未能导入.";
                            error++;
                            ObsEhsPlan.Remove(ObsEhsPlan[i]);
                            i--;
                            countNum++;
                            continue;
                        }
                        //年度判断-先判断是否整形,在判断是否能转换成时间格式
                        if (!string.IsNullOrWhiteSpace(ObsEhsPlan[i].PlanYear))
                        {
                            int num = 0;
                            DateTime t = new DateTime();
                            if (Int32.TryParse(ObsEhsPlan[i].PlanYear, out num))
                            {
                                t = new DateTime(num, 1, 1);
                                DateTime x = new DateTime();
                                if (DateTime.TryParse(t.ToString(), out x))
                                {

                                }
                                else
                                {
                                    falseMessage += "</br>" + "第" + (countNum + 1) + "行计划年度填写错误,未能导入.";
                                    error++;
                                    ObsEhsPlan.Remove(ObsEhsPlan[i]);
                                    i--;
                                    countNum++;
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            ObsEhsPlan[i].PlanYear = DateTime.Now.Year.ToString();
                        }
                        countNum++;
                    }
                    obsplanbll.InsertImportData(ObsEhsPlan, ObsPlanWork);
                }
                else {
                    ObsPlan = ObsPlan.Where(x => x.WorkName != "").ToList();
                    count = ObsPlan.Count;
                    int countNum = 0;
                    for (int i = 0; i < ObsPlan.Count; i++)
                    {
                        //风险等级为IV或者V级 ,观察计划的计划等级为公司级
                        var r = ObsPlanWork.Where(x => x.PlanId == ObsPlan[i].ID).Where(x => x.RiskLevel == "IV级" || x.RiskLevel == "V级").FirstOrDefault();
                        if (r != null)
                        {
                            ObsPlan[i].PlanLevel = "公司级";
                        }
                        else
                        {
                            ObsPlan[i].PlanLevel = "部门级";
                        }

                        if (!string.IsNullOrWhiteSpace(ObsPlan[i].PlanSpeciaty))
                        {
                            var s = new DataItemDetailBLL().GetDataItemListByItemCode("'SpecialtyType'").Where(x => x.ItemName == ObsPlan[i].PlanSpeciaty).FirstOrDefault();
                            if (s != null)
                            {
                                ObsPlan[i].PlanSpeciatyCode = s.ItemValue;
                            }
                            else
                            {
                                falseMessage += "</br>" + "第" + (countNum + 1) + "行专业填写错误,未能导入.";
                                error++;
                                ObsPlan.Remove(ObsPlan[i]);
                                i--;
                                countNum++;
                                continue;
                            }
                        }
                        if (!string.IsNullOrWhiteSpace(ObsPlan[i].PlanArea))
                        {
                            DistrictEntity disEntity = new DistrictBLL().GetDistrict(user.OrganizeId, ObsPlan[i].PlanArea);
                            if (disEntity == null)
                            {
                                //电厂没有该区域则不赋值
                                ObsPlan[i].PlanArea = "";
                            }
                            else
                            {
                                ObsPlan[i].PlanAreaCode = disEntity.DistrictID;
                            }
                        }
                        else
                        {
                            falseMessage += "</br>" + "第" + (countNum + 1) + "行区域不能为空,未能导入.";
                            error++;
                            ObsPlan.Remove(ObsPlan[i]);
                            i--;
                            countNum++;
                            continue;
                        }
                        if (!string.IsNullOrWhiteSpace(ObsPlan[i].WorkName))
                        {

                        }
                        else
                        {
                            falseMessage += "</br>" + "第" + (countNum + 1) + "行作业内容不能为空,未能导入.";
                            error++;
                            ObsPlan.Remove(ObsPlan[i]);
                            i--;
                            countNum++;
                            continue;
                        }
                        //年度判断-先判断是否整形,在判断是否能转换成时间格式
                        if (!string.IsNullOrWhiteSpace(ObsPlan[i].PlanYear))
                        {
                            int num = 0;
                            DateTime t = new DateTime();
                            if (Int32.TryParse(ObsPlan[i].PlanYear, out num))
                            {
                                t = new DateTime(num, 1, 1);
                                DateTime x = new DateTime();
                                if (DateTime.TryParse(t.ToString(), out x))
                                {

                                }
                                else
                                {
                                    falseMessage += "</br>" + "第" + (countNum + 1) + "行计划年度填写错误,未能导入.";
                                    error++;
                                    ObsPlan.Remove(ObsPlan[i]);
                                    i--;
                                    countNum++;
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            ObsPlan[i].PlanYear = DateTime.Now.Year.ToString();
                        }
                        countNum++;
                    }
                    obsplanbll.InsertImportData(ObsPlan, ObsPlanWork);
                }
                message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条。";
                if (error > 0)
                {
                    message += "</br>" + falseMessage;
                }
            }
            return message;
        }
        [HttpPost]
        [AjaxOnly]
        public ActionResult CopyHistoryData(string oldYear, string newYear)
        {
            Operator currUser = OperatorProvider.Provider.Current();
            if (obsplanbll.CopyHistoryData(currUser, oldYear, newYear))
            {
                return Success("操作成功。");
            }
            else
            {
                return Error("操作失败。");
            }
        }
        #endregion
    }
}
