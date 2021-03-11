using ERCHTMS.Code;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ERCHTMS.Busines;
using ERCHTMS.Busines.BaseManage;
using System.Data;
using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.Busines.AuthorizeManage;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.RiskDatabase;
using ERCHTMS.Busines.Observerecord;
namespace ERCHTMS.AppSerivce.Controllers
{
    public class RiskController : BaseApiController
    {
        ERCHTMS.Busines.RiskDatabase.RiskBLL riskBll = new Busines.RiskDatabase.RiskBLL();
        RiskAssessBLL riskassessbll = new RiskAssessBLL();
        /// <summary>
        /// 11.1 获取风险清单并按区域分组
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getRiskList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                long type = dy.data.type;
                string areaId = dy.data.areaid;//区域
                string grade = dy.data.risklevel;//风险等级
                string riskKind = dy.data.riskKind;//风险类别
                string keyWord = dy.data.keyWord;//搜索关键字
                string deptcode = res.Contains("deptcode") ? dy.data.deptcode : "";
               
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                DataTable dt = riskBll.GetAreaList(int.Parse(type.ToString()), areaId, grade, user, riskKind,deptcode, keyWord);
                return new { Code = 0, Count = -1, Info = "获取数据成功", data = dt };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 11.2 获取风险列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object riskList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string areaId = dy.data.identity;//区域Id
                long type = dy.data.type; //风险类别
                string grade = dy.data.risklevel; //风险等级
                string kind = dy.data.riskKind;//风险类别
                string keyWord = dy.data.keyWord;//搜索关键字
                string deptcode = res.Contains("deptcode") ? dy.data.deptcode : ""; //电厂Code

                long page = dy.pageindex;
                long rows = dy.pagesize;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                Pagination pag = new Pagination();
                pag.p_kid = "id riskId";
                pag.p_fields = "riskdesc riskDescribe,grade riskLevel,risktype,majorname as riskpoint,worktask,process,equipmentname,parts,description,DangerSource";
                pag.p_tablename = "BIS_RISKASSESS";
                string roleNames = user.RoleName;
                string sql = "deletemark=0 and enabledmark=0 and status=1";
                if (!string.IsNullOrEmpty(areaId)) {
                    sql += string.Format(" and districtid='{0}' ", areaId);
                }
                //string sql = string.Format("districtid='{0}' and deletemark=0 and enabledmark=0 ", areaId);
                if (!string.IsNullOrEmpty(kind))
                {
                    sql += " and risktype='" + kind + "'";
                }
                if (!string.IsNullOrEmpty(deptcode)) {
                    sql += " and deptcode like '" + deptcode + "%'";
                }
                if (!string.IsNullOrEmpty(keyWord))
                {
                    if (kind == "作业")
                    {
                        sql += " and WorkTask like '%" + keyWord + "%'";
                    }
                    else if (kind == "设备")
                    {
                        sql += " and equipmentname like '%" + keyWord + "%'";
                    }
                    else if (kind == "管理" || kind == "环境")
                    {
                        sql += " and DangerSource like '%" + keyWord + "%'";
                    }
                    else if (kind == "职业病危害")
                    {
                        sql += " and MajorName like '%" + keyWord + "%'";
                    }
                    else {
                        sql += " and (WorkTask like '%" + keyWord + "%' or equipmentname like '%" + keyWord + "%' or DangerSource like '%" + keyWord + "%' or MajorName like '%" + keyWord + "%' )";
                    }

                }
                if (type == 1)
                {
                    sql += "  and createuserid ='" + user.UserId + "'";
                }
                if (type == 2)
                {

                    if (roleNames.Contains("省级"))
                    {

                    }
                    else if (!(roleNames.Contains("公司级用户") || roleNames.Contains("厂级部门用户")))
                    {
                        sql += " and deptcode like '" + user.DeptCode + "%'";
                    }
                    sql += " and  gradeval=1";
                }
                if (type == 3)
                {

                    if (roleNames.Contains("省级"))
                    {

                    }
                    else if (!(roleNames.Contains("公司级用户") || roleNames.Contains("厂级部门用户")))
                    {
                        sql += " and deptcode like '" + user.DeptCode + "%'";
                    }
                }
              
                if (!string.IsNullOrEmpty(areaId))
                {
                    sql += " and districtid='" + areaId + "'";
                }
                if (!string.IsNullOrEmpty(grade))
                {
                    sql += " and grade='" + grade + "'";
                }
                pag.conditionJson = sql;
                pag.sidx = "createdate";
                pag.sord = "desc";
                pag.page = int.Parse(page.ToString());
                pag.rows = int.Parse(rows.ToString());

                DataTable dt = riskBll.GetRiskList(pag);
                return new { Code = 0, Count = pag.records, Info = "获取数据成功", data = dt };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 11.3 获取风险详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getRiskApproveDetail([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string riskId = dy.data.riskid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                object obj = riskBll.GetRisk(riskId);
                if (obj == null)
                {
                    return new { Code = -1, Count = 0, Info = "没有查询到数据！" };
                }
                else
                {
                   return new { Code = 0, Count = -1, Info = "获取数据成功", data = obj };
                }
              
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 11.4 获取区域列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getRiskArea([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string planId = dy.data.planid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                DataTable dt = riskBll.GetAreas(user, planId);
                return new { Code = 0, Count = -1, Info = "获取数据成功", data = dt };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 12.1 获取辨识评估计划列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getRiskIdentifyList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                long mode = dy.data.mode;
                string sql = "deptcode like '" + user.OrganizeCode + "%'";
                //当前用户辨识的数据
                if (mode == 0)
                {
                    sql += string.Format(" and status=0 and id in(select planid from BIS_RISKPPLANDATA where userid='{0}' and datatype=0)", user.Account);
                }
                else
                {
                    string authType = new AuthorizeBLL().GetOperAuthorzeType(user, "7b040bd0-7561-4f87-8967-49c0f969702a", "search");
                    if (!string.IsNullOrEmpty(authType))
                    {
                        switch (authType)
                        {
                            case "1":
                                sql += " and (createuserid='" + user.UserId + "'";
                                break;
                            case "2":
                                sql += " and (deptcode='" + user.DeptCode + "%'";
                                break;
                            case "3":
                                sql += " and  (deptcode  like '" + user.DeptCode + "%'";
                                break;
                            case "4":
                                sql += " and (deptcode like '" + user.OrganizeCode + "%'";
                                break;
                            case "5":
                                sql += " and (1=1 ";
                                break;
                        }
                        sql += " or (',' || userids || ',') like '%," + user.Account + ",%')";
                    }
                    else
                    {
                        sql += " (',' || userids || ',') like '%," + user.Account + ",%'";
                    }
                }
                DataTable dt = riskBll.GetPlanList(user, sql);
                return new { Code = 0, Count = -1, Info = "获取数据成功", data = dt };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 12.2 获取辨识计划中关联的区域列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getIdentifyAreaList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string planId = dy.data.riskidentifyplanid;
                long status = dy.data.status;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                DataTable dt = riskBll.GetAreaByPlanId(planId, int.Parse(status.ToString()), user);
                return new { Code = 0, Count = -1, Info = "获取数据成功", data = dt };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        [HttpPost]
        public object getRight([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string planId = dy.data.planid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                ObserverecordBLL obs = new ObserverecordBLL();
                DataTable dt = obs.GetTable(string.Format("select count(1) from BIS_RISKPPLANDATA where datatype=0 and planid='{0}' and userid='{1}'", planId, user.Account));
                int add = dt.Rows[0][0].ToString() == "0" ? 0 : 1;
                return new { Code = 0, Count = -1, Info = "获取数据成功", data = add };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 12.3 获取辨识评估计划下某区域的风险清单
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getIdentifyAreaRiskList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string planId = dy.data.riskidentifyplanid;
                string areaCode = dy.data.areacode;
                long status = dy.data.status;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                DataTable dt = riskBll.GetRiskListByPlanId(planId, areaCode, int.Parse(status.ToString()), user);
                return new { Code = 0, Count = -1, Info = "获取数据成功", data = dt };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 12.4 新增风险辨识
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object addRiskIdentify([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string areaName = dy.data.riskarea;
                string deptName = dy.data.controldutystation;
                string riskdescribe = dy.data.riskdescribe;
                string result = dy.data.riskresult;
                string riskType = dy.data.riskcategory;
                string postName = dy.data.job;
                string planId = dy.data.planid;
                string areaId = dy.data.riskareaid;
                string areaCode = dy.data.riskareacode;
                string deptCode = dy.data.deptcode;
                string postId = dy.data.jobid;
                string method = dy.data.method;
                string riskId = dy.data.riskid;
                string resulttype = dy.data.resulttype;
                string equipmentname = dy.data.equipmentname;
                string parts = dy.data.parts;
                string worktask = dy.data.worktask;
                string process = dy.data.process;
                string dangersource = dy.data.dangersource;
                string accidenttype = dy.data.accidenttype;
                string riskpoint = dy.data.riskpoint;
                string harmfactor = dy.data.harmfactor;
                string worklevel = dy.data.worklevel;
                string illname = dy.data.illname;
                string faulttype = dy.data.faulttype;
                string levelname = dy.data.levelname;
                long status = dy.data.status;
                RiskAssessEntity risk = new RiskAssessEntity();
                risk.Id = method == "add" ? "" : riskId;
                if (method != "add" && status != 2)
                {
                    risk.Status = 1;
                }
                else {
                    risk.Status = int.Parse(status.ToString());
                }
                risk.DistrictName = areaName;
                risk.DistrictId = areaId;
                risk.DangerSource = dangersource;
                risk.RiskDesc = riskdescribe;
                risk.Result = result;
                risk.PostName = postName;
                risk.RiskType = riskType.Replace("风险","");
                risk.PostId = postId;
                risk.DeptName = deptName;
                risk.DeptCode = deptCode;
                risk.PlanId = planId;
                risk.ResultType = resulttype;
                risk.EquipmentName = equipmentname;
                risk.Parts = parts;
                risk.WorkTask = worktask;
                risk.Process = process;
                risk.AccidentType = accidenttype;
                risk.AreaCode = areaCode;
               
                risk.HarmType = worklevel;
                risk.HarmProperty = illname;
                risk.MajorName = riskpoint;
                risk.Description = harmfactor;
                if (risk.RiskType == "设备") {
                    risk.DangerSource = faulttype;
                }
                risk.FaultType = faulttype;
                risk.LevelName = levelname;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;
                Operator user = OperatorProvider.Provider.Current();
                int count = riskBll.SaveRisk(risk, user);
                return new { Code = 0, Count = -1, Info = "获取数据成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 12.5 	获取管控责任单位列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getDutyStationList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;

                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                DataTable dt = riskBll.GetDeptList(user);
                return new { Code = 0, Count = -1, Info = "获取数据成功", data = dt };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 12.6 获取危害属性和风险分类
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getCodeList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string code = dy.data.itemcode;    
                DataTable dt = riskBll.GetCodeList(code);
                return new { Code = 0, Count = -1, Info = "获取数据成功", data = dt };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 12.9 获取岗位列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getPostList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string deptCode = dy.data.deptcode;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;
                Operator user = OperatorProvider.Provider.Current();
                DataTable dt = riskBll.GetPostList(deptCode, user);
                return new { Code = 0, Count = -1, Info = "获取数据成功", data = dt };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 13.1 岗位风险卡-当前本人
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getMeStationRiskList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                List<object> data = riskBll.GetPostCardList(user, 0);
                return new { Code = 0, Count = -1, Info = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 13.2 岗位风险卡-全部
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getAllStationRiskList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                var data = riskBll.GetPostCardList(user, 1);
                return new { Code = 0, Count = -1, Info = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 13.3 岗位风险卡详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getRiskCardDetails([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string postId = dy.data.jobid;
                string deptCode = dy.data.deptcode;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                List<object> list = riskBll.GetPostRiskList(user, postId, deptCode);
                return new { Code = 0, Count = -1, Info = "获取数据成功", data = list };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 14.2 风险统计
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getRiskStatisticsList([FromBody]JObject json)
        {
            try
            {
                 
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string deptcode = res.Contains("deptcode") ? dy.data.deptcode : "";

                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                object data = riskBll.GetStat(user, deptcode);
                return new { Code = 0, Count = -1, Info = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }


        [HttpPost]
        public object DelRiskAssess([FromBody]JObject json) {

            try
            {
                 string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid; //用户ID 
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;
                Operator user = OperatorProvider.Provider.Current();
                if (null == user)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string planId = dy.data.planid;
                string assessId = dy.data.assessid;
                RiskAssessEntity entity = riskassessbll.GetEntity(assessId);
                if (entity == null)
                {
                    return new { Code = -1, Count = 0, Info = "获取数据失败!" };
                }
                else {
                    riskassessbll.RemoveForm(entity.Id, planId);
                    return new { Code = 0, Count = -1, Info = "获取数据成功" };
                }
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 省级风险清单
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetDeptRiskList([FromBody]JObject json) {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                if (user == null)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                DataTable dt = riskassessbll.GetIndexRiskTarget("", user, 0);
                return new { code = 0, count = dt.Rows.Count, info = "获取成功", data = dt };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
            
        }

    }
}