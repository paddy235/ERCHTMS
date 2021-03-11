using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.Busines.RiskDatabase;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Collections.Generic;
using ERCHTMS.Busines.SystemManage;
using System.Text;
using System.Data;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using System.Linq;
using ERCHTMS.Busines.AuthorizeManage;
using BSFramework.Util.Offices;
using ERCHTMS.Code;
using BSFramework.Data;
using System;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Busines.OccupationalHealthManage;
using ERCHTMS.Entity.HazardsourceManage;
using BSFramework.Util.Extension;
using System.Web;
using Newtonsoft.Json;

namespace ERCHTMS.Web.Areas.RiskDatabase.Controllers
{
    /// <summary>
    /// 描 述：安全风险库
    /// </summary>
    public class RiskAssessController : MvcControllerBase
    {
        private RiskAssessBLL riskassessbll = new RiskAssessBLL();
        private MeasuresBLL measuresBLL = new MeasuresBLL();
        private RiskAssessHistoryBLL riskassesshistorybll = new RiskAssessHistoryBLL();
        private RiskHistotyBLL riskhistotybll = new RiskHistotyBLL();
        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            string gxhs = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("广西华昇版本");
            if (!string.IsNullOrWhiteSpace(gxhs))
            {
                string actionName = "";
                string[] keys = Request.QueryString.AllKeys;
                if (keys.Count() > 0)
                {
                    actionName = "GXHSAssessIndex?";
                    int num = 0;
                    for (int i = 0; i < keys.Count(); i++)
                    {
                        if (num == 0)
                        {
                            actionName += keys[i] + "=" + Request.QueryString[keys[i]];
                        }
                        else
                        {
                            actionName += "&" + keys[i] + "=" + Request.QueryString[keys[i]];
                        }
                        num++;
                    }
                }
                else
                {
                    actionName = "GXHSAssessIndex";
                }
                return Redirect(actionName);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// 历史记录列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HistoryIndex()
        {
            return View();
        }
        [HttpGet]
        public ActionResult HistoryRecord()
        {
            string gxhs = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("广西华昇版本");
            if (!string.IsNullOrWhiteSpace(gxhs))
            {
                string actionName = "";
                string[] keys = Request.QueryString.AllKeys;
                if (keys.Count() > 0)
                {
                    actionName = "GXHSHistoryRecord?";
                    int num = 0;
                    for (int i = 0; i < keys.Count(); i++)
                    {
                        if (num == 0)
                        {
                            actionName += keys[i] + "=" + Request.QueryString[keys[i]];
                        }
                        else
                        {
                            actionName += "&" + keys[i] + "=" + Request.QueryString[keys[i]];
                        }
                        num++;
                    }
                }
                else
                {
                    actionName = "GXHSHistoryRecord";
                }
                return Redirect(actionName);
            }
            else
            {
                return View();
            }
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
        public ActionResult Details()
        {
            string gxhs = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("广西华昇版本");
            if (!string.IsNullOrWhiteSpace(gxhs))
            {
                string actionName = "";
                string[] keys = Request.QueryString.AllKeys;
                if (keys.Count() > 0)
                {
                    actionName = "GXHSDetails?";
                    int num = 0;
                    for (int i = 0; i < keys.Count(); i++)
                    {
                        if (num == 0)
                        {
                            actionName += keys[i] + "=" + Request.QueryString[keys[i]];
                        }
                        else
                        {
                            actionName += "&" + keys[i] + "=" + Request.QueryString[keys[i]];
                        }
                        num++;
                    }
                }
                else
                {
                    actionName = "GXHSDetails";
                }
                return Redirect(actionName);
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult Show()
        {
            string gxhs = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("广西华昇版本");
            if (!string.IsNullOrWhiteSpace(gxhs))
            {
                string actionName = "";
                string[] keys = Request.QueryString.AllKeys;
                if (keys.Count() > 0)
                {
                    actionName = "GXHSDetails?";
                    int num = 0;
                    for (int i = 0; i < keys.Count(); i++)
                    {
                        if (num == 0)
                        {
                            actionName += keys[i] + "=" + Request.QueryString[keys[i]];
                        }
                        else
                        {
                            actionName += "&" + keys[i] + "=" + Request.QueryString[keys[i]];
                        }
                        num++;
                    }
                }
                else
                {
                    actionName = "GXHSDetails";
                }
                return Redirect(actionName);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// 重大风险清单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult List()
        {
            return View();
        }
        /// <summary>
        /// 岗位风险卡
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RiskCard()
        {
            return View();
        }
        /// <summary>
        /// 岗位风险卡详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RiskCardDetails()
        {
            return View();
        }
        /// <summary>
        /// 统计分析
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Stat()
        {
            return View();
        }
        /// <summary>
        /// 历史辨识评估记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult History()
        {
            string gxhs = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("广西华昇版本");
            if (!string.IsNullOrWhiteSpace(gxhs))
            {
                string actionName = "";
                string[] keys = Request.QueryString.AllKeys;
                if (keys.Count() > 0)
                {
                    actionName = "GXHSAssessHistory?";
                    int num = 0;
                    for (int i = 0; i < keys.Count(); i++)
                    {
                        if (num == 0)
                        {
                            actionName += keys[i] + "=" + Request.QueryString[keys[i]];
                        }
                        else
                        {
                            actionName += "&" + keys[i] + "=" + Request.QueryString[keys[i]];
                        }
                        num++;
                    }
                }
                else
                {
                    actionName = "GXHSAssessHistory";
                }
                return Redirect(actionName);
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// 广西华昇风险详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GXHSDetails()
        {
            return View();
        }

        /// <summary>
        /// 广西华昇安全风险清单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GXHSList()
        {
            return View();
        }
        /// <summary>
        /// 风险种类
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TypesOfRiskForm()
        {
            return View();
        }
        /// <summary>
        /// 广西华昇历史菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GXHSHistoryRecord()
        {
            return View();
        }
        /// <summary>
        /// 广西华昇风险管控清单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GXHSControlList()
        {
            return View();
        }
        /// <summary>
        /// 广西华昇辨识评估列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GXHSAssessIndex()
        {
            return View();
        }

        /// <summary>
        /// 广西华昇辨识评估完成数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GXHSAssessHistory()
        {
            return View();
        }
        #endregion
        #region 风险清单选择页面视图
        /// <summary>
        /// 工作任务清单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult WorkListForm()
        {
            return View();
        }
        /// <summary>
        /// 作业步骤清单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult WorkProcessForm()
        {
            return View();
        }
        /// <summary>
        /// 作业风险危害因素
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult WorkRiskDescForm()
        {
            return View();
        }
        /// <summary>
        /// 风险过后
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult WorkRiskResultForm()
        {
            return View();
        }
        /// <summary>
        /// 故障类别
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FaultClassifyForm()
        {
            return View();
        }
        /// <summary>
        /// 危险源
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DangerSourceForm()
        {
            return View();
        }
        /// <summary>
        /// 风险点类别
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MajorNameTypeForm()
        {
            return View();
        }
        /// <summary>
        /// 危险源类别
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DangerSourceTypForm()
        {
            return View();
        }
        /// <summary>
        /// 工器具危化品风险点类别
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ToolMajorNameTypeForm()
        {
            return View();
        }
        /// <summary>
        /// 工器具危化品危险源类别
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ToolDangerSourceTypForm()
        {
            return View();
        }
        #endregion
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="areaCode">区域Code</param>
        /// <param name="areaId">区域ID</param>
        /// <param name="grade">风险等级</param>
        /// <param name="accType">事故类别编码</param>
        /// <param name="deptCode">涉及部门Code</param>
        /// <param name="keyWord">查询关键字</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string areaCode, string areaId, string grade, string accType, string deptCode, string keyWord)
        {
            var data = riskassessbll.GetList(areaCode, areaId, grade, accType, deptCode, keyWord);
            return ToJsonResult(data);
        }
        [HttpGet]
        public ActionResult GetPageListJson1(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "t.id";
            pagination.p_fields = @"t.areaid,risktype,dangersource,riskdesc,itemr,grade,
                                        accidentname,result,createuserid,createuserdeptcode,
                                        deptname,postname,districtname,t.districtid,deptcode,
                                        createdate,MajorName,Description,t.worktask,process,
                                        equipmentname,parts,createuserorgcode,DutyPerson ";
            pagination.p_tablename = " bis_riskassess t inner join (select districtid,worktask,max(id)id  from bis_riskassess where worktask is not null and status=1 and deletemark=0 group by worktask,districtid)w on w.id=t.id";
            pagination.conditionJson = "status=1 and deletemark=0";

            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.conditionJson += " and deptcode like '" + user.OrganizeCode + "%'";
            //if (!user.IsSystem)
            //{

            //        string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
            //        if (!string.IsNullOrEmpty(authType))
            //        {

            //            switch (authType)
            //            {
            //                case "1":
            //                    pagination.conditionJson += " and createuserid='" + user.UserId + "'";
            //                    break;
            //                case "2":
            //                    pagination.conditionJson += " and deptcode='" + user.DeptCode + "'";
            //                    break;
            //                case "3":
            //                    pagination.conditionJson += " and deptcode like '" + user.DeptCode + "%'";
            //                    break;
            //                case "4":
            //                    pagination.conditionJson += " and deptcode like '" + user.OrganizeCode + "%'";
            //                    break;
            //                case "5":
            //                    pagination.conditionJson += string.Format(" and deptcode in(select encode from BASE_DEPARTMENT where deptcode like '{0}%')", user.NewDeptCode);
            //                    break;
            //            }
            //        }
            //        else
            //        {
            //            pagination.conditionJson += " and 0=1";
            //        }

            //}
            try
            {
                var data = riskassessbll.GetPageList(pagination, queryJson);
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
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        /// <summary>
        /// 获取历史记录
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAssessHisToryPage(Pagination pagination, string queryJson)
        {
            queryJson = queryJson ?? "";
            pagination.p_kid = "ID";
            pagination.p_fields = "HISNAME, CREATEDATE, CREATEUSERNAME";
            pagination.p_tablename = "BIS_RISKASSESSHISTORY t";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                //登陆人可查看本机构的所有数据
                pagination.conditionJson = " CreateUserOrgCode='" + user.OrganizeCode + "' and CREATEUSERDEPTCODE like '" + user.DeptCode + "%'";
            }


            var watch = CommonHelper.TimerStart();
            var data = riskassesshistorybll.GetPageList(pagination, queryJson);
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

        [HttpGet]
        public ActionResult GetAssessHistoryPageListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "id";
            pagination.p_fields = @"id as keyvalue,areaid,risktype,dangersource,riskdesc,itemr,grade,gradeval,
accidentname,result,createuserid,createuserdeptcode,
createuserorgcode,deptname,postname,districtname,
deptcode,createdate,MajorName,Description,jobname,toolordanger,dangersourcetype,hjsystem,hjequpment,
worktask,process,equipmentname,parts,levelname,1 as faultordanger,faulttype,project,dutyperson,dutypersonid,element,faultcategory,majornametype,
                                        packuntil,packnum,storagespace,postdept,postdeptid,postperson,postpersonid,postdeptcode,'' f1,'' f2,'' f3,'' f4,'' f5,name,workcontent,harmname,harmdescription,name as equname,isspecialequ,checkprojectname,checkstandard,typesofrisk,riskcategory,exposedrisk,consequences,existingmeasures,itema,itemb,itemc,advicemeasures,effectiveness,costfactor,measuresresult,isadopt,case when  HazardType is null then '' when HazardType is not null then (select wm_concat(b.itemname) from  (select * from base_dataitemdetail where itemid in (select itemid from base_dataitem where itemcode='HazardType')) b where instr(','||HazardType||',',','||b.itemvalue||',')>0) end  as hazardtype  ";
            pagination.p_tablename = "BIS_RISKHISTORY";
            pagination.conditionJson = "status=1 and deletemark=0";
            var data = riskhistotybll.GetPageList(pagination, queryJson);
            var measureBll = new MeasuresBLL();
            foreach (DataRow dr in data.Rows)
            {
                string content = measureBll.GetMeasures(dr["id"].ToString(), "工程技术");
                dr["f1"] = content;
                content = measureBll.GetMeasures(dr["id"].ToString(), "管理");
                dr["f2"] = content;
                content = measureBll.GetMeasures(dr["id"].ToString(), "培训教育");
                dr["f3"] = content;
                content = measureBll.GetMeasures(dr["id"].ToString(), "个体防护");
                dr["f4"] = content;
                content = measureBll.GetMeasures(dr["id"].ToString(), "应急处置");
                dr["f5"] = content;
            }
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
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson, string mode = "", string allList = "", string IndexState = "")
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "id";
            pagination.p_fields = @"areaid,risktype,dangersource,riskdesc,itemr,grade,gradeval,
accidentname,result,createuserid,createuserdeptcode,jobname,toolordanger,dangersourcetype,hjsystem,hjequpment,
createuserorgcode,deptname,postname,districtname,
deptcode,planid,createdate,MajorName,Description,
worktask,process,equipmentname,parts,levelname,1 as faultordanger,faulttype,project,dutyperson,dutypersonid,element,faultcategory,majornametype,
                                        packuntil,packnum,storagespace,postdept,postdeptid,postperson,postpersonid,postdeptcode,'' f1,'' f2,'' f3,'' f4,'' f5,name,workcontent,harmname,harmdescription,name as equname,isspecialequ,checkprojectname,checkstandard,typesofrisk,riskcategory,exposedrisk,consequences,existingmeasures,itema,itemb,itemc,advicemeasures,effectiveness,costfactor,measuresresult,isadopt,case when  HazardType is null then '' when HazardType is not null then (select wm_concat(b.itemname) from  (select * from base_dataitemdetail where itemid in (select itemid from base_dataitem where itemcode='HazardType')) b where instr(','||HazardType||',',','||b.itemvalue||',')>0) end  as hazardtype";
            pagination.p_tablename = "bis_riskassess";
            pagination.conditionJson = "status=1 and deletemark=0 and enabledmark=0";
            pagination.sidx = pagination.sidx + " " + pagination.sord + ",id";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {

                if (!string.IsNullOrEmpty(mode))
                {
                    pagination.conditionJson += " and deptcode like '" + user.OrganizeCode + "%'";
                    if (mode == "1")
                    {
                        if (!string.IsNullOrWhiteSpace(new DataItemDetailBLL().GetItemValue("IsGdxy")))
                        {
                            pagination.conditionJson += " and risktype in('管理','设备','区域')";
                        }

                    }
                }
                //风险清单,重大风险清单特殊需求,查看全部数据
                else if (!string.IsNullOrWhiteSpace(allList))
                {
                    if (user.RoleName.Contains("省级"))
                    {
                        string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                        if (!string.IsNullOrEmpty(authType))
                        {

                            switch (authType)
                            {
                                case "1":
                                    pagination.conditionJson += " and createuserid='" + user.UserId + "'";
                                    break;
                                case "2":
                                    pagination.conditionJson += " and deptcode='" + user.DeptCode + "'";
                                    break;
                                case "3":
                                    pagination.conditionJson += " and deptcode like '" + user.DeptCode + "%'";
                                    break;
                                case "4":
                                    pagination.conditionJson += " and deptcode like '" + user.OrganizeCode + "%'";
                                    break;
                                case "5":
                                    pagination.conditionJson += string.Format(" and deptcode in(select encode from BASE_DEPARTMENT where deptcode like '{0}%')", user.NewDeptCode);
                                    break;
                            }
                        }
                    }
                    else
                    {
                        pagination.conditionJson += " and deptcode like '" + user.OrganizeCode + "%'";
                    }

                }
                else
                {
                    string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                    if (!string.IsNullOrEmpty(authType))
                    {

                        switch (authType)
                        {
                            case "1":
                                pagination.conditionJson += " and createuserid='" + user.UserId + "'";
                                break;
                            case "2":
                                pagination.conditionJson += " and deptcode='" + user.DeptCode + "'";
                                break;
                            case "3":
                                pagination.conditionJson += " and deptcode like '" + user.DeptCode + "%'";
                                break;
                            case "4":
                                pagination.conditionJson += " and deptcode like '" + user.OrganizeCode + "%'";
                                break;
                            case "5":
                                pagination.conditionJson += string.Format(" and deptcode in(select encode from BASE_DEPARTMENT where deptcode like '{0}%')", user.NewDeptCode);
                                break;
                        }
                    }
                }

                //首页跳转
                if (!string.IsNullOrWhiteSpace(IndexState))
                {
                    string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                    if (!string.IsNullOrEmpty(authType))
                    {

                        switch (authType)
                        {
                            case "1":
                                pagination.conditionJson += " and createuserid='" + user.UserId + "'";
                                break;
                            case "2":
                                pagination.conditionJson += " and deptcode='" + user.DeptCode + "'";
                                break;
                            case "3":
                                pagination.conditionJson += " and deptcode like '" + user.DeptCode + "%'";
                                break;
                            case "4":
                                pagination.conditionJson += " and deptcode like '" + user.OrganizeCode + "%'";
                                break;
                            case "5":
                                pagination.conditionJson += string.Format(" and deptcode in(select encode from BASE_DEPARTMENT where deptcode like '{0}%')", user.NewDeptCode);
                                break;
                        }
                    }
                    else
                    {
                        pagination.conditionJson += " and 0=1";
                    }
                }
            }

            try
            {
                var data = riskassessbll.GetPageList(pagination, queryJson);
                var measureBll = new MeasuresBLL();
                foreach (DataRow dr in data.Rows)
                {
                    string content = measureBll.GetMeasures(dr["id"].ToString(), "工程技术");
                    dr["f1"] = content;
                    content = measureBll.GetMeasures(dr["id"].ToString(), "管理");
                    dr["f2"] = content;
                    content = measureBll.GetMeasures(dr["id"].ToString(), "培训教育");
                    dr["f3"] = content;
                    content = measureBll.GetMeasures(dr["id"].ToString(), "个体防护");
                    dr["f4"] = content;
                    content = measureBll.GetMeasures(dr["id"].ToString(), "应急处置");
                    dr["f5"] = content;
                }
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
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }

        /// <summary>
        /// 获取风险管控列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPageControlListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "";
            pagination.p_fields = @"*";
            pagination.p_tablename = @"(select '' centerdept,deptcode,deptname,name,risktype,wm_concat(districtname) as districtname,wm_concat(areacode) as areacode,wm_concat(areaid) as areaid,wm_concat(typesofrisk) as typesofrisk,min(gradeval) as gradeval,'' grade,'' levelname,wm_concat(remark) as remark,wm_concat(dutyperson) as dutyperson，wm_concat(dutypersonid) as dutypersonid from bis_riskassess where status=1 and deletemark=0 and enabledmark=0";
            pagination.conditionJson = "1=1";
            pagination.sidx = "deptcode";
            pagination.sord = "asc";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            try
            {
                DepartmentBLL deptbll = new DepartmentBLL();
                var data = riskassessbll.GetPageControlListJson(pagination, queryJson);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    var dept = deptbll.GetEntityByCode(data.Rows[i]["deptcode"].ToString()); //判断创建部门是不是部门级
                    while (dept.Nature == "班组" || dept.Nature == "专业")
                    {
                        dept = deptbll.GetEntity(dept.ParentId);
                    }
                    data.Rows[i]["centerdept"] = dept.FullName; //将中心部门赋值为上级部门
                    if (dept.EnCode == data.Rows[i]["deptcode"].ToString())
                    {
                        data.Rows[i]["deptname"] = ""; //如果创建部不是部门级，则将（工序）班组 赋值为空

                    }
                    data.Rows[i]["districtname"] = string.Join(",", data.Rows[i]["districtname"].ToString().Split(',').Distinct());
                    data.Rows[i]["areaid"] = string.Join(",", data.Rows[i]["areaid"].ToString().Split(',').Distinct());
                    data.Rows[i]["areacode"] = string.Join(",", data.Rows[i]["areacode"].ToString().Split(',').Distinct());
                    data.Rows[i]["typesofrisk"] = string.Join(",", data.Rows[i]["typesofrisk"].ToString().Split(',').Distinct());
                    data.Rows[i]["remark"] = string.Join(",", data.Rows[i]["remark"].ToString().Split(',').Distinct());
                    data.Rows[i]["dutyperson"] = string.Join(",", data.Rows[i]["dutyperson"].ToString().Split(',').Distinct());
                    data.Rows[i]["dutypersonid"] = string.Join(",", data.Rows[i]["dutypersonid"].ToString().Split(',').Distinct());
                    switch (data.Rows[i]["gradeval"].ToString())//根据风险等级值  给风险等级 管控层级赋值
                    {
                        case "1":
                            data.Rows[i]["levelname"] = "公司级";
                            data.Rows[i]["grade"] = "一级风险";
                            break;
                        case "2":
                            data.Rows[i]["levelname"] = "中心（部门）级";
                            data.Rows[i]["grade"] = "二级风险";
                            break;
                        case "3":
                            data.Rows[i]["levelname"] = "工序（班组）级";
                            data.Rows[i]["grade"] = "三级风险";
                            break;
                        case "4":
                            data.Rows[i]["levelname"] = "班组级";
                            data.Rows[i]["grade"] = "四级风险";
                            break;
                        default:
                            break;
                    }
                }
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
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        [HttpGet]
        public ActionResult GetTrainPageListJson(Pagination pagination, string queryJson, string mode = "")
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "id";
            pagination.p_fields = "dangersource,riskdesc,itemr,grade,accidentname,result,deptname,postname,districtname,worktask,measure";
            pagination.p_tablename = "BIS_RISKASSESS";
            pagination.conditionJson = "status=1 and deletemark=0";

            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                if (!string.IsNullOrEmpty(mode))
                {
                    pagination.conditionJson += " and deptcode like '" + user.OrganizeCode + "%'";
                }
                else
                {
                    string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                    if (!string.IsNullOrEmpty(authType))
                    {
                        string orgCode = "";
                        var dbtype = DbHelper.DbType;
                        if (dbtype == DatabaseType.SqlServer)
                        {
                            orgCode = "substring(deptcode,0,4)";
                        }
                        if (dbtype == DatabaseType.Oracle)
                        {
                            orgCode = "substr(deptcode,0,3)";
                        }
                        if (dbtype == DatabaseType.MySql)
                        {
                            orgCode = "substring(deptcode,1,3)";
                        }
                        switch (authType)
                        {
                            case "1":
                                pagination.conditionJson += " and createuserid='" + user.UserId + "'";
                                break;
                            case "2":
                                pagination.conditionJson += " and deptcode='" + user.DeptCode + "'";
                                break;
                            case "3":
                                pagination.conditionJson += " and deptcode like'" + user.DeptCode + "%'";
                                break;
                            case "4":
                                pagination.conditionJson += " and " + orgCode + "='" + user.OrganizeCode + "'";
                                break;
                        }
                    }
                    else
                    {
                        pagination.conditionJson += " and 0=1";
                    }
                }
            }
            var data = riskassessbll.GetPageList(pagination, queryJson);
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
        [HttpGet]
        public ActionResult GetAssessPageListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "id";
            pagination.p_fields = @"areaid,risktype,dangersource,description,itemr,grade,accidentname,result,
                                        createuserid,createuserdeptcode,createuserorgcode,deptname,postname,
                                        districtname,deptcode,planid,createdate,riskdesc,levelname,
                                        worktask,process,equipmentname,parts,MajorName,1 as faultordanger,faulttype,
                                        jobname,toolordanger,dangersourcetype,hjsystem,hjequpment,
                                        project,dutyperson,dutypersonid,element,faultcategory,majornametype,
                                        packuntil,packnum,storagespace,postdept,postdeptid,postperson,postpersonid,postdeptcode,'' f1,'' f2,'' f3,'' f4,'' f5,name,workcontent,harmname,harmdescription,name as equname,isspecialequ,checkprojectname,checkstandard,typesofrisk,riskcategory,exposedrisk,consequences,existingmeasures,itema,itemb,itemc,advicemeasures,effectiveness,costfactor,measuresresult,isadopt,case when  HazardType is null then '' when HazardType is not null then (select wm_concat(b.itemname) from  (select * from base_dataitemdetail where itemid in (select itemid from base_dataitem where itemcode='HazardType')) b where instr(','||HazardType||',',','||b.itemvalue||',')>0) end  as hazardtype,gradeval ";
            pagination.p_tablename = "BIS_RISKASSESS";
            pagination.conditionJson = "status>0 and deletemark=0";
            pagination.sidx = pagination.sidx + " " + pagination.sord + ",id";
            var data = riskassessbll.GetPageList(pagination, queryJson);
            var measureBll = new MeasuresBLL();
            foreach (DataRow dr in data.Rows)
            {
                string content = measureBll.GetMeasures(dr["id"].ToString(), "工程技术");
                dr["f1"] = content;
                content = measureBll.GetMeasures(dr["id"].ToString(), "管理");
                dr["f2"] = content;
                content = measureBll.GetMeasures(dr["id"].ToString(), "培训教育");
                dr["f3"] = content;
                content = measureBll.GetMeasures(dr["id"].ToString(), "个体防护");
                dr["f4"] = content;
                content = measureBll.GetMeasures(dr["id"].ToString(), "应急处置");
                dr["f5"] = content;
            }
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
        [HttpGet]
        public ActionResult GetGxhsAssessPageListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "id";
            pagination.p_fields = @"areaid,risktype,dangersource,description,itemr,grade,accidentname,result,
                                        createuserid,createuserdeptcode,createuserorgcode,deptname,postname,
                                        districtname,deptcode,planid,createdate,riskdesc,levelname,
                                        worktask,process,equipmentname,parts,MajorName,1 as faultordanger,faulttype,
                                        jobname,toolordanger,dangersourcetype,hjsystem,hjequpment,
                                        project,dutyperson,dutypersonid,element,faultcategory,majornametype,
                                        packuntil,packnum,storagespace,postdept,postdeptid,postperson,postpersonid,postdeptcode,'' f1,'' f2,'' f3,'' f4,'' f5,name,workcontent,harmname,harmdescription,name as equname,isspecialequ,checkprojectname,checkstandard,typesofrisk,riskcategory,exposedrisk,consequences,existingmeasures,itema,itemb,itemc,advicemeasures,effectiveness,costfactor,measuresresult,isadopt,case when  HazardType is null then '' when HazardType is not null then (select wm_concat(b.itemname) from  (select * from base_dataitemdetail where itemid in (select itemid from base_dataitem where itemcode='HazardType')) b where instr(','||HazardType||',',','||b.itemvalue||',')>0) end  as hazardtype,gradeval ";
            pagination.p_tablename = "BIS_RISKASSESS";
            pagination.conditionJson = "status>0 and deletemark=0";
            pagination.sidx = pagination.sidx + " " + pagination.sord + ",id";
            var data = riskassessbll.GetPageList(pagination, queryJson);
            var measureBll = new MeasuresBLL();
            foreach (DataRow dr in data.Rows)
            {
                string content = measureBll.GetMeasures(dr["id"].ToString(), "工程技术");
                dr["f1"] = content;
                content = measureBll.GetMeasures(dr["id"].ToString(), "管理");
                dr["f2"] = content;
                content = measureBll.GetMeasures(dr["id"].ToString(), "培训教育");
                dr["f3"] = content;
                content = measureBll.GetMeasures(dr["id"].ToString(), "个体防护");
                dr["f4"] = content;
                content = measureBll.GetMeasures(dr["id"].ToString(), "应急处置");
                dr["f5"] = content;
            }
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
        [HttpGet]
        public ActionResult GetHistoryListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "id";
            pagination.p_fields = @"areaid,risktype,dangersource,riskdesc,itemr,grade,gradeval,accidentname,
                                    result,createuserid,createuserdeptcode,createuserorgcode,levelname,faulttype,1 as faultordanger,
                                    deptname,postname,districtname,deptcode,planid,createdate,worktask,process,equipmentname,parts,MajorName,Description,
jobname,toolordanger,dangersourcetype,hjsystem,hjequpment,project,dutyperson,dutypersonid,element,faultcategory,majornametype,
                                        packuntil,packnum,storagespace,postdept,postdeptid,postperson,postpersonid,postdeptcode,'' f1,'' f2,'' f3,'' f4,'' f5,name,workcontent,harmname,harmdescription,name as equname,isspecialequ,checkprojectname,checkstandard,typesofrisk,riskcategory,exposedrisk,consequences,existingmeasures,itema,itemb,itemc,advicemeasures,effectiveness,costfactor,measuresresult,isadopt,case when  HazardType is null then '' when HazardType is not null then (select wm_concat(b.itemname) from  (select * from base_dataitemdetail where itemid in (select itemid from base_dataitem where itemcode='HazardType')) b where instr(','||HazardType||',',','||b.itemvalue||',')>0) end  as hazardtype  ";
            pagination.p_tablename = "BIS_RISKHISTORY";
            pagination.conditionJson = "status>0 and deletemark=0";
            pagination.sidx = pagination.sidx + " " + pagination.sord + ",id";
            var data = riskassessbll.GetPageList(pagination, queryJson);
            var measureBll = new MeasuresBLL();
            foreach (DataRow dr in data.Rows)
            {
                string content = measureBll.GetMeasures(dr["id"].ToString(), "工程技术");
                dr["f1"] = content;
                content = measureBll.GetMeasures(dr["id"].ToString(), "管理");
                dr["f2"] = content;
                content = measureBll.GetMeasures(dr["id"].ToString(), "培训教育");
                dr["f3"] = content;
                content = measureBll.GetMeasures(dr["id"].ToString(), "个体防护");
                dr["f4"] = content;
                content = measureBll.GetMeasures(dr["id"].ToString(), "应急处置");
                dr["f5"] = content;
            }
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
        /// <summary>
        /// 获取岗位风险卡
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询条件</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetCardListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "";
            pagination.p_fields = "distinct deptcode,deptname,postname,postid";
            pagination.p_tablename = "BIS_RISKASSESS";
            pagination.conditionJson = "status=1 and deletemark=0";

            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
            string orgCode = "";
            var dbtype = DbHelper.DbType;
            if (dbtype == DatabaseType.SqlServer)
            {
                orgCode = "substring(deptcode,0,4)";
            }
            if (dbtype == DatabaseType.Oracle)
            {
                orgCode = "substr(deptcode,0,3)";
            }
            if (dbtype == DatabaseType.MySql)
            {
                orgCode = "substring(deptcode,1,3)";
            }
            if (!string.IsNullOrEmpty(authType))
            {
                switch (authType)
                {
                    case "1":
                        pagination.conditionJson += " and createuserid='" + user.UserId + "'";
                        break;
                    case "2":
                        pagination.conditionJson += " and deptcode='" + user.DeptCode + "'";
                        break;
                    case "3":
                        pagination.conditionJson += " and deptcode like'" + user.DeptCode + "%'";
                        break;
                    case "4":
                        pagination.conditionJson += " and " + orgCode + "='" + user.OrganizeCode + "'";
                        break;
                }
            }
            else
            {
                pagination.conditionJson += " and 0=1";
            }

            var data = riskassessbll.GetPageList(pagination, queryJson);
            int records = riskassessbll.GetPostCardCount(pagination.conditionJson);
            decimal totalPage = 0;
            if (records % pagination.rows == 0)
            {
                totalPage = records / pagination.rows;
            }
            else
            {
                totalPage = records / pagination.rows + 1;
            }
            totalPage = Math.Round(decimal.Parse(totalPage.ToString()), 0);
            var JsonData = new
            {
                rows = data,
                total = totalPage,
                page = pagination.page,
                records = records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return Content(JsonData.ToJson());
        }
        [HttpGet]
        public ActionResult GetCardInfoJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "id";
            pagination.p_fields = "districtname,dangersource,result,'' measure";
            pagination.p_tablename = "BIS_RISKASSESS";
            pagination.conditionJson = "status=1 and deletemark=0";

            var watch = CommonHelper.TimerStart();
            DataTable data = riskassessbll.GetPageList(pagination, queryJson);
            MeasuresBLL measuresbll = new MeasuresBLL();
            foreach (DataRow dr in data.Rows)
            {
                StringBuilder sb = new StringBuilder();
                IEnumerable<MeasuresEntity> list = measuresbll.GetList("", dr[0].ToString());
                int j = 1;
                foreach (MeasuresEntity measure in list)
                {
                    sb.AppendFormat("{1}. {0}；<br />", measure.Content, j);
                    j++;
                }
                dr["measure"] = sb.ToString();
            }
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
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            try
            {
                var data = riskassessbll.GetEntity(keyValue);
                string gxhs = new DataItemDetailBLL().GetItemValue("广西华昇版本");
                if (!string.IsNullOrWhiteSpace(gxhs))
                {
                    data.Grade = string.IsNullOrWhiteSpace(data.Grade)?"": data.Grade.Replace("重大风险", "一级风险").Replace("较大风险", "二级风险").Replace("一般风险", "三级风险").Replace("低风险", "四级风险");
                }
                return ToJsonResult(data);
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }

        }
        /// <summary>
        /// 获取辨识记录实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetHistoryFormJson(string keyValue)
        {
            RiskHistotyBLL history = new RiskHistotyBLL();
            var data = history.GetEntity(keyValue);
            string gxhs = new DataItemDetailBLL().GetItemValue("广西华昇版本");
            if (!string.IsNullOrWhiteSpace(gxhs))
            {
                data.Grade = string.IsNullOrWhiteSpace(data.Grade) ? "" : data.Grade.Replace("重大风险", "一级风险").Replace("较大风险", "二级风险").Replace("一般风险", "三级风险").Replace("低风险", "四级风险");
            }
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        ///  <param name="workId">作业步骤ID</param>
        /// <param name="dangerId">危险点ID</param>
        /// <param name="areaId">区域ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetRiskJson(string workId, string dangerId, string areaId)
        {
            var data = riskassessbll.GetEntity(workId, dangerId, areaId);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取区域
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetAreas(string OrgId = "")
        {
            DistrictBLL districtBLL = new DistrictBLL();
            var data = districtBLL.GetList(OrgId);
            StringBuilder sb = new StringBuilder();
            foreach (DistrictEntity dist in data)
            {
                sb.AppendFormat("<option value='{0}'>{1}</option>", dist.DistrictID, dist.DistrictName);
            }
            return sb.ToString();
        }
        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="areaId">区域Id</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetEquipmentJson(string areaId)
        {
            var data = riskassessbll.GetEuqByAreaId(areaId);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 根据区域获取风险点
        /// </summary>
        /// <param name="parentId">区域ID</param>
        /// <returns></returns>
        [HttpPost]
        public string GetRisks(string parentId)
        {
            DangerSourceBLL dangerBLL = new DangerSourceBLL();
            List<DangerSourceEntity> listDS = dangerBLL.GetList(parentId, "").ToList();
            StringBuilder sb = new StringBuilder();
            foreach (DangerSourceEntity dist in listDS)
            {
                sb.AppendFormat("<option value='{0}'>{1}</option>", dist.Id, dist.Name);
            }
            return sb.ToString();
        }
        /// <summary>
        /// 统计风险点数量
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <returns></returns>
        [HttpGet]
        public string getRiskCount(string deptCode, string year = "")
        {
            //Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            //string roleNames=user.RoleName;
            //if (roleNames.Contains("公司级用户") || roleNames.Contains("厂级部门用户"))
            //{
            //    deptCode = user.OrganizeCode;
            //}
            return riskassessbll.GetRiskCountByDeptCode(deptCode, year);
        }
        /// <summary>
        /// 按区域统计风险点数量
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <returns></returns>
        [HttpGet]
        public string getAreaRiskCount(string deptCode, string year = "")
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            //string roleNames = user.RoleName;
            //if (roleNames.Contains("公司级用户") || roleNames.Contains("厂级部门用户"))
            //{
            //    deptCode = user.OrganizeCode;
            //}
            return riskassessbll.GetAreaRiskCountByDeptCode(deptCode, year);
        }
        [HttpGet]
        public ActionResult GetDept(string deptCode)
        {
            var DeptEntity = new DepartmentBLL().GetEntityByCode(deptCode);
            return Content(DeptEntity.ToJson());
        }
        /// <summary>
        /// 按年份统计风险点数量
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <returns></returns>
        [HttpGet]
        public string getYearRiskCount(string deptCode, string year = "", string riskGrade = "重大风险,较大风险,一般风险,低风险", string areaCode = "")
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string roleNames = user.RoleName;
            //if (roleNames.Contains("公司级用户") || roleNames.Contains("厂级部门用户"))
            //{
            //    deptCode = user.OrganizeCode;
            //}
            return riskassessbll.GetYearRiskCountByDeptCode(deptCode, year, riskGrade, areaCode);
        }
        /// <summary>
        /// 对比分析图
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <returns></returns>
        [HttpGet]
        public string getRatherRiskCount(string deptCode, string year = "", string riskGrade = "重大风险,较大风险,一般风险,低风险", string areaCode = "")
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string roleNames = user.RoleName;
            //if (roleNames.Contains("公司级用户") || roleNames.Contains("厂级部门用户"))
            //{
            //    deptCode = user.OrganizeCode;
            //}
            return riskassessbll.GetRatherRiskCountByDeptCode(deptCode, year, riskGrade, areaCode);
        }
        /// <summary>
        ///获取统计表格数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <returns></returns>
        [HttpPost]
        public string GetAreaRiskList(string deptCode, string year = "", string riskGrade = "重大风险,较大风险,一般风险,低风险")
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string roleNames = user.RoleName;
            //if (roleNames.Contains("公司级用户") || roleNames.Contains("厂级部门用户"))
            //{
            //    deptCode = user.OrganizeCode;
            //}
            return riskassessbll.GetAreaRiskList(deptCode, year, riskGrade);
        }
        /// <summary>
        ///按部门获取风险对比数据
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <returns></returns>
        [HttpPost]
        public string GetDeptRiskList(string deptCode, string year = "", string riskGrade = "重大风险,较大风险,一般风险,低风险")
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string roleNames = user.RoleName;
            //if (roleNames.Contains("公司级用户") || roleNames.Contains("厂级部门用户"))
            //{
            //    deptCode = user.OrganizeCode;
            //}
            return riskassessbll.GetDeptRiskList(deptCode, year, riskGrade);
        }
        /// <summary>
        ///按区域对比分析风险
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <returns></returns>
        [HttpGet]
        public string GetAreaRatherRiskStat(string deptCode, string year = "", string riskGrade = "重大风险,较大风险,一般风险,低风险")
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string roleNames = user.RoleName;
            //if (roleNames.Contains("公司级用户") || roleNames.Contains("厂级部门用户"))
            //{
            //    deptCode = user.OrganizeCode;
            //}
            return riskassessbll.GetAreaRatherRiskStat(deptCode, year, riskGrade);
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
        [HandlerMonitor(6, "删除风险辨识记录")]
        public ActionResult RemoveForm(string keyValue, string planId = "")
        {
            riskassessbll.RemoveForm(keyValue, planId);
            Operator user = OperatorProvider.Provider.Current();
            LogEntity logEntity = new LogEntity();
            logEntity.CategoryId = 6;
            logEntity.OperateTypeId = 6.ToString();
            logEntity.OperateType = "删除";
            logEntity.OperateAccount = user.UserName;
            logEntity.OperateUserId = user.UserId;
            logEntity.ExecuteResult = 1;
            logEntity.Module = SystemInfo.CurrentModuleName;
            logEntity.ModuleId = SystemInfo.CurrentModuleId;
            logEntity.ExecuteResultJson = "";
            LogBLL.WriteLog(logEntity);
            return Success("删除成功。");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(6, "删除风险清单")]
        public ActionResult Remove(string ids)
        {
            try
            {
                riskassessbll.Delete(ids);
                return Success("删除成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(6, "删除历史风险清单")]
        public ActionResult RemoveAssessHistory(string keyValue)
        {
            var num = riskassesshistorybll.RemoveAssessHistory(keyValue);
            if (num > 0) return Success("删除成功。");
            else return Error("删除失败");
        }

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        ///  <param name="keyValue">主键值</param>
        /// <param name="measuresJson">管控措施</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "新增或修改风险辨识信息")]
        public ActionResult SaveForm(string keyValue, string measuresJson, RiskAssessEntity entity)
        {
            try
            {
                if (entity.RiskType == "设备")
                {
                    entity.DangerSource = entity.FaultType;
                }
                int count = riskassessbll.SaveForm(keyValue, entity);
                //保存关联的管控措施
                if (count > 0)
                {
                    HazardfactorsBLL hf = new HazardfactorsBLL();
                    hf.Add(entity.DistrictId, entity.DistrictName, entity.DangerSource);
                    if (!string.IsNullOrWhiteSpace(measuresJson) && measuresJson.Length > 0)
                    {
                        if (measuresBLL.Remove(entity.Id) >= 0)
                        {
                            List<MeasuresEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MeasuresEntity>>(measuresJson);
                            measuresBLL.SaveForm("", list);
                        }
                    }
                    return Success("操作成功。");
                }
                else
                {
                    return Error("操作失败。");
                }
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }


        }
        /// <summary>
        /// 重大风险清单导出--暂停使用（2018-12-06）
        /// </summary>
        /// <param name="queryJson"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ActionResult ExportExcel(string queryJson, string fileName)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.p_kid = "id";
            pagination.p_fields = "postname,deptname,districtname,riskdesc,resulttype,risktype,result,way,itema,itemb,itemc,itemr,grade,accidentname";
            pagination.p_tablename = "BIS_RISKASSESS";
            pagination.rows = 1000000000;

            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
            pagination.conditionJson = "status=1 and deletemark=0";
            string orgCode = "";
            var dbtype = DbHelper.DbType;
            if (dbtype == DatabaseType.SqlServer)
            {
                orgCode = "substring(deptcode,0,4)";
            }
            if (dbtype == DatabaseType.Oracle)
            {
                orgCode = "substr(deptcode,0,3)";
            }
            if (dbtype == DatabaseType.MySql)
            {
                orgCode = "substring(deptcode,1,3)";
            }
            if (!string.IsNullOrEmpty(authType))
            {
                switch (authType)
                {
                    case "1":
                        pagination.conditionJson += " and createuserid='" + user.UserId + "'";
                        break;
                    case "2":
                        pagination.conditionJson += " and deptcode='" + user.DeptCode + "'";
                        break;
                    case "3":
                        pagination.conditionJson += " and deptcode like'" + user.DeptCode + "%'";
                        break;
                    case "4":
                        pagination.conditionJson += " and " + orgCode + "='" + user.OrganizeCode + "'";
                        break;
                }
            }
            else
            {
                pagination.conditionJson += " and 0=1";
            }
            //取出数据源
            DataTable exportTable = riskassessbll.GetPageList(pagination, queryJson);
            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "重大风险清单";
            excelconfig.FileName = fileName + ".xls";
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            ColumnEntity columnentity = new ColumnEntity();
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "postname", ExcelColumn = "岗位", Width = 10 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "deptname", ExcelColumn = "管控部门", Width = 10 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "districtname", ExcelColumn = "区域", Width = 10 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "riskdesc", ExcelColumn = "风险描述", Width = 20 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "resulttype", ExcelColumn = "风险后果分类", Width = 10 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "risktype", ExcelColumn = "风险类别", Width = 10 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "result", ExcelColumn = "风险后果", Width = 10 });
            // excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "manager", ExcelColumn = "风险控制措施" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "way", ExcelColumn = "评价方式", Width = 10 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "itema", ExcelColumn = "可能性", Width = 10 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "itemb", ExcelColumn = "频繁程度", Width = 10 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "itemc", ExcelColumn = "严重性", Width = 10 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "itemr", ExcelColumn = "风险分值", Width = 10 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "grade", ExcelColumn = "风险等级", Width = 10 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "accidentname", ExcelColumn = "可能导致的事故类型", Width = 20 });
            //调用导出方法
            ExcelHelper.ExcelDownload(exportTable, excelconfig);

            return Success("导出成功。");
        }
        public ActionResult ExportCard(string queryJson, string fileName, string postName, string deptName)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.p_kid = "id";
            pagination.p_fields = "id,districtname,dangersource,result,'' measure";
            pagination.p_tablename = "BIS_RISKASSESS";
            pagination.conditionJson = "status=1 and deletemark=0";
            pagination.rows = 1000000000;

            if (!string.IsNullOrEmpty(postName) && !string.IsNullOrEmpty(deptName))
            {
                pagination.conditionJson += " and deptname='" + deptName + "' and postname='" + postName + "'";
            }
            //Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            //string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
            //string orgCode = "";
            //var dbtype = DbHelper.DbType;
            //if (dbtype == DatabaseType.SqlServer)
            //{
            //    orgCode = "substring(deptcode,0,4)";
            //}
            //if (dbtype == DatabaseType.Oracle)
            //{
            //    orgCode = "substr(deptcode,0,3)";
            //}
            //if (dbtype == DatabaseType.MySql)
            //{
            //    orgCode = "substring(deptcode,1,3)";
            //}
            //if (!string.IsNullOrEmpty(authType))
            //{
            //    switch (authType)
            //    {
            //        case "1":
            //            pagination.conditionJson += " and createuserid='" + user.UserId + "'";
            //            break;
            //        case "2":
            //            pagination.conditionJson += " and deptcode='" + user.DeptCode + "'";
            //            break;
            //        case "3":
            //            pagination.conditionJson += " and deptcode like'" + user.DeptCode + "%'";
            //            break;
            //        case "4":
            //            pagination.conditionJson += " and " + orgCode + "='" + user.OrganizeCode + "'";
            //            break;
            //    }
            //}
            //else
            //{
            //    pagination.conditionJson += " and 0=1";
            //}
            DataTable data = riskassessbll.GetPageList(pagination, queryJson);
            MeasuresBLL measuresbll = new MeasuresBLL();
            foreach (DataRow dr in data.Rows)
            {
                StringBuilder sb = new StringBuilder();
                IEnumerable<MeasuresEntity> list = measuresbll.GetList("", dr[0].ToString());
                int j = 1;
                foreach (MeasuresEntity measure in list)
                {
                    sb.AppendFormat("{1}. {0}；\r\n", measure.Content, j);
                    j++;
                }
                dr["measure"] = sb.ToString();
                dr["dangersource"] = "[" + dr["districtname"].ToString() + "] " + dr["dangersource"].ToString();
            }
            data.Columns.Remove("id"); data.Columns.Remove("districtname");
            //取出数据源
            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.FileName = fileName + ".xls";
            excelconfig.Title = "岗位:" + Server.UrlDecode(postName) + "             部门:" + Server.UrlDecode(deptName);
            excelconfig.TitlePoint = 14;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            ColumnEntity columnentity = new ColumnEntity();
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "dangersource", ExcelColumn = "岗位风险", Width = 50 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "result", ExcelColumn = "风险后果", Width = 20 });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "measure", ExcelColumn = "预控措施", Width = 50 });
            //调用导出方法
            ExcelHelper.ExcelDownload(data, excelconfig);

            return Success("导出成功。");
        }

        /// <summary>
        /// 设为历史记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SetHistory(string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            try
            {
                //新建历史记录清单
                var entity = new RiskAssessHistoryEntity
                {
                    HisName = "安全风险清单" + DateTime.Now.ToString("yyyyMMdd"),
                    CreateDate = DateTime.Now,
                    CreateUserDeptCode = user.DeptCode,
                    CreateUserId = user.UserId,
                    CreateUserName = user.UserName,
                    CreateUserOrgCode = user.OrganizeCode
                };
                entity.ID = Guid.NewGuid().ToString();
                string newId = Guid.NewGuid().ToString();
                //向关系表中添加数据
                StringBuilder sbSql = new StringBuilder();
                StringBuilder subsbSql = new StringBuilder();
                StringBuilder sqlWhere = new StringBuilder();
                sbSql.Append(" insert into BIS_RISKHISTORY (HISTORYID,AREAID,AREACODE,WORKCONTENT,DANGERSOURCE,");
                sbSql.Append(" ID,ACCIDENTTYPE,WAY,ITEMA,ITEMB,ITEMC,ITEMR,GRADE,DEPTCODE,DEPTNAME,POSTID,POSTNAME,CREATEUSERID,");
                sbSql.Append(" CREATEDATE,CREATEUSERNAME,MODIFYUSERID,MODIFYDATE,MODIFYUSERNAME,CREATEUSERDEPTCODE,CREATEUSERORGCODE,");
                sbSql.Append(" HTDESC,ISWZ,HTGRADE,HTTYPE,HTMEASURES,MEASURE,HARMPROPERTY,FAULTTYPE,LEVELNAME,");
                sbSql.Append(" RESULT,ACCIDENTNAME,HARMTYPE,STATE,RISKTYPE,PLANID,DISTRICTID,DISTRICTNAME,PROCESS,WORKTASK,EQUIPMENTNAME,PARTS,RESULTTYPE,RISKDESC,");
                sbSql.Append(" AREANAME,DELETEMARK,ENABLEDMARK,STATUS,GRADEVAL,MAJORNAME,MAJORCODE,TEAMNAME,TEAMCODE,DESCRIPTION,");
                sbSql.Append(@"jobname,toolordanger,dangersourcetype,hjsystem,hjequpment,project,dutyperson,dutypersonid,element,faultcategory,majornametype,packuntil,packnum,storagespace,postdept,postdeptid,postperson,postpersonid,postdeptcode,name,listingid,harmname,hazardtype,harmdescription,typesofrisk,riskcategory,exposedrisk,existingmeasures,isspecialequ,checkprojectname,checkstandard,consequences,advicemeasures,effectiveness,costfactor,measuresresult,isadopt,measuresresultval,isconventional,remark)");
                sbSql.Append(" select (select '" + entity.ID + "' FROM DUAL),AREAID,AREACODE,WORKCONTENT,DANGERSOURCE,");
                sbSql.Append(" (ID||'" + newId + "'),ACCIDENTTYPE,WAY,ITEMA,ITEMB,ITEMC,ITEMR,GRADE,DEPTCODE,DEPTNAME,POSTID,POSTNAME,CREATEUSERID,");
                sbSql.Append(" CREATEDATE,CREATEUSERNAME,MODIFYUSERID,MODIFYDATE,MODIFYUSERNAME,CREATEUSERDEPTCODE,CREATEUSERORGCODE,");
                sbSql.Append(" HTDESC,ISWZ,HTGRADE,HTTYPE,HTMEASURES,MEASURE,HARMPROPERTY,FAULTTYPE,LEVELNAME,");
                sbSql.Append(" RESULT,ACCIDENTNAME,HARMTYPE,STATE,RISKTYPE,PLANID,DISTRICTID,DISTRICTNAME,PROCESS,WORKTASK,EQUIPMENTNAME,PARTS,RESULTTYPE,RISKDESC,");
                sbSql.Append(" AREANAME,DELETEMARK,ENABLEDMARK,STATUS,GRADEVAL,MAJORNAME,MAJORCODE,TEAMNAME,TEAMCODE,DESCRIPTION,");
                sbSql.Append(@"jobname,toolordanger,dangersourcetype,hjsystem,hjequpment,project,dutyperson,dutypersonid,element,faultcategory,majornametype,
                                        packuntil,packnum,storagespace,postdept,postdeptid,postperson,postpersonid,postdeptcode,name,listingid,harmname,hazardtype,harmdescription,typesofrisk,riskcategory,exposedrisk,existingmeasures,isspecialequ,checkprojectname,checkstandard,consequences,advicemeasures,effectiveness,costfactor,measuresresult,isadopt,measuresresultval,isconventional,remark");
                sbSql.Append(" from BIS_RISKASSESS where status=1 and deletemark=0  ");


                if (!user.IsSystem)
                {
                    //设置本机构的数据
                    sqlWhere.Append(" and (CreateUserId='" + user.UserId + "' or DeptCode like '" + user.OrganizeCode + "%')");
                }
                string gxhs = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("广西华昇版本");
                if (!string.IsNullOrWhiteSpace(gxhs))
                {
                    if (queryJson.Length > 0)
                    {

                        var queryParam = queryJson.ToJObject();

                        //查询条件
                        if (!queryParam["year"].IsEmpty())
                        {
                            string year = queryParam["year"].ToString();
                            sqlWhere.Append(string.Format(" and to_char(createdate,'yyyy')='{0}'", year));
                        }
                        //查询条件
                        if (!queryParam["riskType"].IsEmpty())
                        {
                            string riskType = queryParam["riskType"].ToString();
                            sqlWhere.Append(string.Format(" and risktype ='{0}'", riskType));
                        }
                        //查询条件
                        if (!queryParam["level"].IsEmpty())
                        {
                            string level = queryParam["level"].ToString();
                            sqlWhere.Append(string.Format(" and gradeval={0}", level));
                        }
                        //查询条件
                        if (!queryParam["areaCode"].IsEmpty())
                        {
                            string areaCode = queryParam["areaCode"].ToString();
                            sqlWhere.Append(string.Format(" and areaCode like '{0}%'", areaCode));
                        }
                        //风险等级
                        if (!queryParam["grade"].IsEmpty())
                        {
                            string grade = queryParam["grade"].ToString();
                            sqlWhere.Append(string.Format(" and grade = '{0}'", grade));
                        }
                        //部门Code
                        if (!queryParam["deptCode"].IsEmpty())
                        {
                            string deptCode = queryParam["deptCode"].ToString();
                            if (user.RoleName.Contains("省级"))
                            {
                                var dept = new DepartmentBLL().GetEntityByCode(deptCode);
                                if (dept != null)
                                {
                                    if (dept.Nature == "厂级")
                                    {
                                        sqlWhere.Append(string.Format(" and deptCode like '{0}%'", deptCode));

                                    }
                                }
                            }
                            else
                            {
                                sqlWhere.Append(string.Format(" and deptCode like '{0}%'", deptCode));
                            }
                        }
                        if (!queryParam["createCode"].IsEmpty())
                        {
                            sqlWhere.Append(string.Format(" and createuserdeptcode like '{0}%'", queryParam["createCode"].ToString()));
                        }
                        if (!queryParam["Name"].IsEmpty())
                        {
                            sqlWhere.Append(string.Format(" and name like '%{0}%'", queryParam["Name"].ToString()));
                        }
                    }
                }
                
                string sql = sbSql.Append(sqlWhere.ToString()).ToString();

                //保存关联数据
                var result = riskassesshistorybll.ExecuteBySql(sql);

                if (result > 0)
                {
                    subsbSql.Append("insert into BIS_MEASURES(ID,CONTENT,TYPECODE,RISKID,CREATEUSERID,CREATEDATE,CREATEUSERNAME,MODIFYUSERID,MODIFYDATE,MODIFYUSERNAME,");
                    subsbSql.Append("CREATEUSERDEPTCODE,CREATEUSERORGCODE,DELETEMARK,ENABLEDMARK,REMARK,AREACODE,TYPENAME,AREAID)");
                    subsbSql.Append("select (ID||'" + newId + "'),CONTENT,TYPECODE,(RISKID||'" + newId + "'),CREATEUSERID,CREATEDATE,CREATEUSERNAME,MODIFYUSERID,MODIFYDATE,MODIFYUSERNAME,");
                    subsbSql.Append(string.Format("CREATEUSERDEPTCODE,CREATEUSERORGCODE,DELETEMARK,ENABLEDMARK,REMARK,AREACODE,TYPENAME,AREAID from BIS_MEASURES where riskid in(select id from BIS_RISKASSESS where 1=1 {0})", sqlWhere.ToString()));
                    string subSql = subsbSql.ToString();
                    var r = riskassesshistorybll.ExecuteBySql(subSql);
                    riskassesshistorybll.SaveFrom(entity.ID, entity);
                    return Success("操作成功。");

                }
                else
                {
                    return Error("暂无数据可设为历史记录。");
                }

            }
            catch (Exception ex)
            {
                return Error("操作失败。");
                throw;
            }

        }

        /// <summary>
        /// 修改安全风险管控清单
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ModifyControlList(List<RiskControlList> list)
        {
            try
            {
                string area = "";
                Boolean isequ = false; //判断风险类型中是否有设备设施的数据，如果有则不更新所在地点
                if (list.Where(t => t.RiskType == "设备设施").Count() > 0)
                {
                    isequ = true;
                }
                foreach (var item in list)
                {
                    area = isequ ? "" : ",AREAID='" + item.AreaId + "',AREACODE='" + item.AreaCode + "',DISTRICTID='" + item.AreaId + "',DISTRICTNAME='" + item.AreaName + "',AREANAME='" + item.AreaName + "'";
                    StringBuilder sbSql = new StringBuilder();
                    sbSql.Append(" update bis_riskassess set dutyperson='" + item.DutyPerson + "',dutypersonid='" + item.DutyPersonId + "',remark='" + item.Remark + "'" + area + " where deptcode='" + item.DeptCode + "' and name='" + item.Name + "' and risktype='" + item.RiskType + "'");
                    riskassesshistorybll.ExecuteBySql(sbSql.ToString());
                    if (item.RiskType == "作业活动") //更新作业活动清单 所在地点
                    {
                        StringBuilder sbsSql = new StringBuilder();
                        sbsSql.Append(" update BIS_BASELISTING set AREANAME='" + item.AreaName + "',areaid='" + item.AreaId + "',areacode='" + item.AreaCode + "' where id in (select listingid from bis_riskassess where deptcode='" + item.DeptCode + "' and name='" + item.Name + "' and risktype='" + item.RiskType + "')");
                        riskassesshistorybll.ExecuteBySql(sbsSql.ToString());
                    }
                }
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
        }

        /// <summary>
        /// 导出风险管控列表
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult ExportControlListExcel(string queryJson)
        {
            try
            {
                HttpResponse resp = System.Web.HttpContext.Current.Response;
                string sql = @"select '' num,'' centerdept,deptname,name,risktype,wm_concat(districtname) as districtname,wm_concat(typesofrisk) as typesofrisk,'' grade,'' levelname,wm_concat(dutyperson) as dutyperson，wm_concat(remark) as remark,min(gradeval) as gradeval,deptcode from bis_riskassess where status=1 and deletemark=0 and enabledmark=0";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                DepartmentBLL deptbll = new DepartmentBLL();
                if (user.RoleName.Contains("厂级") || user.RoleName.Contains("公司级"))
                {
                    sql += string.Format(" and deptCode like '{0}%'", user.OrganizeCode);
                }
                else
                {
                    sql += string.Format(" and deptCode = '{0}'", user.DeptCode);
                }
                if (!string.IsNullOrWhiteSpace(queryJson))
                {
                    var queryParam = queryJson.ToJObject();
                    //风险类别
                    if (!queryParam["riskType"].IsEmpty())
                    {
                        string riskType = queryParam["riskType"].ToString();
                        sql += string.Format(" and risktype ='{0}'", riskType);
                    }

                    //部门Code
                    if (!queryParam["deptCode"].IsEmpty())
                    {
                        string deptCode = queryParam["deptCode"].ToString();
                        if (user.RoleName.Contains("省级"))
                        {
                            var dept = deptbll.GetEntityByCode(deptCode);
                            if (dept != null)
                            {
                                if (dept.Nature == "厂级")
                                {
                                    sql += string.Format(" and deptCode like '{0}%'", deptCode);
                                }
                            }
                        }
                        else
                        {

                            sql += string.Format(" and deptCode like '{0}%'", deptCode);
                        }
                    }
                    if (!queryParam["name"].IsEmpty())
                    {
                        sql += string.Format(" and name like '%{0}%'", queryParam["name"].ToString());
                    }

                    sql += " group by deptcode,deptname,name,risktype";
                    //风险等级
                    if (!queryParam["grade"].IsEmpty())
                    {
                        string grade = queryParam["grade"].ToString();
                        sql += string.Format(" having min(gradeval) = '{0}'", grade);
                    }
                    //区域Code
                    string areaCode = "";
                    if (!queryParam["areaCode"].IsEmpty())
                    {
                        areaCode = queryParam["areaCode"].ToString();
                        sql += sql.Contains("having") ? string.Format(" and areaCode like '{0}%'", areaCode) : string.Format(" having areaCode like '{0}%'", areaCode);
                    }
                }
                var data = riskassessbll.FindTableBySql(sql);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    data.Rows[i]["num"] = i + 1;
                    var dept = deptbll.GetEntityByCode(data.Rows[i]["deptcode"].ToString()); //判断创建部门是不是部门级
                    while (dept.Nature == "班组" || dept.Nature == "专业")
                    {
                        dept = deptbll.GetEntity(dept.ParentId);
                    }
                    if (dept.EnCode != data.Rows[i]["deptcode"].ToString())
                    {
                        data.Rows[i]["centerdept"] = dept.FullName; //如果创建部门不是部门级，则将中心部门赋值上级部门
                    }
                    data.Rows[i]["districtname"] = string.Join(",", data.Rows[i]["districtname"].ToString().Split(',').Distinct());
                    data.Rows[i]["typesofrisk"] = string.Join(",", data.Rows[i]["typesofrisk"].ToString().Split(',').Distinct());
                    data.Rows[i]["remark"] = string.Join(",", data.Rows[i]["remark"].ToString().Split(',').Distinct());
                    data.Rows[i]["dutyperson"] = string.Join(",", data.Rows[i]["dutyperson"].ToString().Split(',').Distinct());
                    switch (data.Rows[i]["gradeval"].ToString())//根据风险等级值  给风险等级 管控层级赋值
                    {
                        case "1":
                            data.Rows[i]["levelname"] = "公司级";
                            data.Rows[i]["grade"] = "一级风险";
                            break;
                        case "2":
                            data.Rows[i]["levelname"] = "中心（部门）级";
                            data.Rows[i]["grade"] = "二级风险";
                            break;
                        case "3":
                            data.Rows[i]["levelname"] = "工序（班组）级";
                            data.Rows[i]["grade"] = "三级风险";
                            break;
                        case "4":
                            data.Rows[i]["levelname"] = "班组级";
                            data.Rows[i]["grade"] = "四级风险";
                            break;
                        default:
                            break;
                    }
                }
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                wb.Open(Server.MapPath("~/Resource/ExcelTemplate/风险管控清单导出模板.xls"));
                wb.Worksheets[0].Cells.ImportDataTable(data, false, 3, 0,data.Rows.Count,11);
                wb.Save(Server.UrlEncode("风险管控清单.xls"), Aspose.Cells.FileFormatType.Excel2003, Aspose.Cells.SaveType.OpenInBrowser, resp);
                return Success("导出成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
            
        }


        /// <summary>
        /// 广西华昇风险评价清单导出
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportGxhsListExcel()
        {
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            Pagination pagination = new Pagination();
            pagination.p_kid = "";
            pagination.p_fields = @"'' as num,name,workcontent,districtname,harmname,case when  HazardType is null then '' when HazardType is not null then (select wm_concat(b.itemname) from  (select * from base_dataitemdetail where itemid in (select itemid from base_dataitem where itemcode='HazardType')) b where instr(','||HazardType||',',','||b.itemvalue||',')>0) end  as hazardtype,harmdescription,to_char(isspecialequ) as isspecialequ,checkprojectname,checkstandard,riskdesc,typesofrisk,riskcategory,exposedrisk,consequences,existingmeasures,itema,itemb,itemc,itemr,advicemeasures,effectiveness,costfactor,measuresresult,to_char(isadopt) as isadopt,risktype";
            pagination.p_tablename = "bis_riskassess";
            pagination.conditionJson = "status=1 and deletemark=0 and enabledmark=0";
            pagination.page = 1;
            pagination.rows = 100000;
            pagination.sidx = "createdate";
            pagination.sord = "desc";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.RoleName.Contains("公司级") || user.RoleName.Contains("厂级"))
            {
                pagination.conditionJson += " and CREATEUSERORGCODE='" + user.OrganizeCode + "'";
            }
            else
            {
                pagination.conditionJson += " and deptcode='" + user.DeptCode + "'";
            }
            try
            {
                var data = riskassessbll.GetPageList(pagination, "");
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                wb.Open(Server.MapPath("~/Resource/ExcelTemplate/风险评价清单导出模板.xls"));
                if (data.Rows.Count>0)
                {
                    DataTable dt1 = data.Select("risktype='作业活动'").Count() > 0 ? data.Select("risktype='作业活动'").CopyToDataTable() : new DataTable();
                    
                    if (dt1.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            dt1.Rows[i]["num"] = i + 1;
                            dt1.Rows[i]["isadopt"] = string.IsNullOrWhiteSpace(dt1.Rows[i]["isadopt"].ToString()) ? "" : dt1.Rows[i]["isadopt"].ToString() == "0" ? "采纳" : "不采纳";
                        }
                        dt1.Columns.Remove("isspecialequ"); dt1.Columns.Remove("checkprojectname");
                        dt1.Columns.Remove("checkstandard"); dt1.Columns.Remove("consequences"); dt1.Columns.Remove("risktype");
                        wb.Worksheets[0].Cells.ImportDataTable(dt1, false, 2, 0, dt1.Rows.Count, 21);
                    }
                    DataTable dt2 = data.Select("risktype='设备设施'").Count() > 0 ? data.Select("risktype='设备设施'").CopyToDataTable() : new DataTable();
                    
                    if (dt2.Rows.Count > 0)
                    {
                        dt2.Columns.Remove("workcontent"); dt2.Columns.Remove("harmname"); dt2.Columns.Remove("hazardtype");
                        dt2.Columns.Remove("harmdescription"); dt2.Columns.Remove("exposedrisk"); dt2.Columns.Remove("risktype");
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            dt2.Rows[i]["num"] = i + 1;
                            dt2.Rows[i]["isspecialequ"] = dt2.Rows[i]["isspecialequ"].ToString() == "0" ? "是" : "否";
                            dt2.Rows[i]["isadopt"] = string.IsNullOrWhiteSpace(dt2.Rows[i]["isadopt"].ToString()) ? "" : dt2.Rows[i]["isadopt"].ToString() == "0" ? "采纳" : "不采纳";
                        }
                        wb.Worksheets[1].Cells.ImportDataTable(dt2, false, 2, 0, dt2.Rows.Count, 20);
                    }
                }
                wb.Save(Server.UrlEncode("风险评价清单.xls"), Aspose.Cells.FileFormatType.Excel2003, Aspose.Cells.SaveType.OpenInBrowser, resp);
                return Success("导出成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion
    }
}  