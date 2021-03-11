using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.AccidentEvent;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SafePunish;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Entity.RoutineSafetyWork;
using ERCHTMS.Entity.SafePunish;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ERCHTMS.AppSerivce.Controllers
{
    public class SafePunishController : BaseApiController
    {
        SafepunishBLL SafePunish = new SafepunishBLL();
        private SafekpidataBLL safekpidatabll = new SafekpidataBLL();
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
        private AptitudeinvestigateauditBLL aptitudeinvestigateauditbll = new AptitudeinvestigateauditBLL();
        public HttpContext ctx { get { return HttpContext.Current; } }
        // GET api/SafePunish
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/SafePunish/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/SafePunish
        public void Post([FromBody]string value)
        {
        }

        // PUT api/SafePunish/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/SafePunish/5
        public void Delete(int id)
        {
        }

        /// <summary>
        /// 保存安全惩罚
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object SafePunishSave()
        {
            try
            {
                string res = ctx.Request["json"];
                var dyObj = JsonConvert.DeserializeAnonymousType(res, new
                {
                    userid = string.Empty,
                    data = new PunishModel()
                });
                string userId = dyObj.userid; //当前用户ID 
                OperatorProvider.AppUserId = userId;  //设置当前用户
                string keyValue = !string.IsNullOrEmpty(dyObj.data.keyvalue) ? dyObj.data.keyvalue : Guid.NewGuid().ToString();
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                var year = DateTime.Now.ToString("yyyy");
                var month = DateTime.Now.ToString("MM");
                var rewardCode = "Q/CRPHZHB 2208.06.01-JL02-" + year + month + SafePunish.GetPunishCode();
                dyObj.data.punentity.SafePunishCode = rewardCode;
                SafePunish.SaveForm(keyValue, dyObj.data.punentity, dyObj.data.kpientity);
            }
            catch (Exception)
            {
                return new { code = -1, count = 0, info = "保存失败" };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }


        /// <summary>
        /// 提交安全惩罚
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object SafePunishApply()
        {
            try
            {
                string res = ctx.Request["json"];
                var dyObj = JsonConvert.DeserializeAnonymousType(res, new
                {
                    userid = string.Empty,
                    data = new PunishModel()
                });
                string userId = dyObj.userid; //当前用户ID 
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string keyValue = !string.IsNullOrEmpty(dyObj.data.keyvalue) ? dyObj.data.keyvalue : Guid.NewGuid().ToString();
                if (dyObj.data.punentity != null && (string.IsNullOrEmpty(dyObj.data.punentity.ApplyState) || dyObj.data.punentity.ApplyState == "0"))
                {
                    var year = DateTime.Now.ToString("yyyy");
                    var month = DateTime.Now.ToString("MM");
                    var rewardCode = "Q/CRPHZHB 2208.06.01-JL02-" + year + month + SafePunish.GetPunishCode();
                    dyObj.data.punentity.SafePunishCode = rewardCode;
                    SafePunish.SaveForm(keyValue, dyObj.data.punentity, dyObj.data.kpientity);
                }

                if (!string.IsNullOrEmpty(keyValue) && dyObj.data.entity != null)
                {
                    SafePunish.CommitApply(keyValue, dyObj.data.entity);
                }

            }
            catch (Exception)
            {
                return new { code = -1, count = 0, info = "保存失败" };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }

        /// <summary>
        /// 获取未遂事件列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public Object GetWssjList([FromBody]JObject json)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;    
                //开始日期
                string sTime = res.Contains("stime") ? dy.data.stime : "";
                //结束日期
                string eTime = res.Contains("etime") ? dy.data.etime : "";
                //未遂事件类型
                string wssjtype = res.Contains("wssjtype") ? dy.data.wssjtype : "";
                //未遂事件名称
                string wssjtypename = res.Contains("wssjtypename") ? dy.data.wssjtypename : "";

                int pageSize = res.Contains("pagesize") ? int.Parse(dy.data.pagesize.ToString()) : 10; //每页条数

                int pageIndex = res.Contains("pagenum") ? int.Parse(dy.data.pagenum.ToString()) : 1; //请求页码

                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                Pagination pagination = new Pagination();
                pagination.p_kid = "ID";
                pagination.page = int.Parse(pageIndex.ToString());
                pagination.rows = int.Parse(pageSize.ToString());
                pagination.p_fields = "issubmit_deal,CREATEUSERID,WSSJNAME, WSSJTYPENAME,HAPPENTIME,AREANAME,WSSJBGUSERNAME,DEALID,CREATEUSERDEPTCODE as departmentcode,CREATEUSERORGCODE as  organizecode";
                pagination.p_tablename = "V_AEM_WSSJBG_deal_ORDER t";
                pagination.p_kid = "ID";
                pagination.conditionJson = "1=1";
                string queryJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    happentimestart = sTime,
                    happentimeend = eTime,
                    wssjtype = wssjtype,
                    wssjtypename = wssjtypename
                });

                var wssjbg = new WSSJBGBLL().GetPageList(pagination, queryJson);
                var data = new
                {
                    rows = wssjbg,
                    total = pagination.total,
                    page = pagination.page,
                    records = pagination.records,
                    costtime = CommonHelper.TimerEnd(watch)
                };
                return new { Code = 0, Count = pagination.records, Info = "获取数据成功", data = data };

            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }


        /// <summary>
        /// 获取安全惩罚列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public Object GetSafePunishList([FromBody]JObject json)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;

                //是否只查看我的工作
                string pager = res.Contains("pager") ? dy.data.pager : "";
                //开始日期
                string sTime = res.Contains("stime") ? dy.data.stime : "";
                //结束日期
                string eTime = res.Contains("etime") ? dy.data.etime : "";

                //流程状态
                string flowstate = res.Contains("flowstate") ? dy.data.flowstate : "";

                //模糊查询条件
                string keyword = res.Contains("keyword") ? dy.data.keyword : "";

                int pageSize = res.Contains("pagesize") ? int.Parse(dy.data.pagesize.ToString()) : 10; //每页条数

                int pageIndex = res.Contains("pagenum") ? int.Parse(dy.data.pagenum.ToString()) : 1; //请求页码

                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                Pagination pagination = new Pagination();
                pagination.page = int.Parse(pageIndex.ToString());
                pagination.rows = int.Parse(pageSize.ToString());
                pagination.p_kid = "Id";
                pagination.p_fields = @"CreateUserId,CreateDate,CreateUserName,ModifyUserId,ModifyDate,ModifyUserName,CreateUserDeptCode,CreateUserOrgCode,FlowState,ApplyUserId,ApproverPeopleIds,
                 ApplyState,Amercetype,Applydeptid,Applydeptcode,Applydeptname,Amerceamount,SafePunishCode,ApplyTime,PunishObjectNames,PunishType,PunishRemark";
                pagination.p_tablename = "BIS_SAFEPUNISH";
                pagination.sidx = "CreateDate";//排序字段
                pagination.sord = "desc";//排序方式
                pagination.conditionJson = "1=1";
                //if (curUser.IsSystem)
                //{
                //    pagination.conditionJson = "1=1";
                //}
                //else
                //{
                //    string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "CREATEUSERDEPTCODE", "CREATEUSERORGCODE");
                //    if (!string.IsNullOrEmpty(where))
                //    {
                //        pagination.conditionJson += " and " + where;
                //    }

                //}
                string queryJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    pager = pager,
                    sTime = sTime,
                    eTime = eTime,
                    flowstate =flowstate ,
                    keyword = keyword
                });
                var rewarddata = SafePunish.GetPageList(pagination, queryJson);
                var data = new
                {
                    rows = rewarddata,
                    total = pagination.total,
                    page = pagination.page,
                    records = pagination.records,
                    costtime = CommonHelper.TimerEnd(watch)
                };
                return new { Code = 0, Count = pagination.records, Info = "获取数据成功", data = data };

            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }




        /// <summary>
        /// 获取安全惩罚详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetSafePunishDetail([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string keyValue = res.Contains("id") ? dy.data.id : "";
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                object data = SafePunish.GetEntity(keyValue);
                var kpidata = safekpidatabll.GetList("").Where(p => p.SafePunishId == keyValue).FirstOrDefault();
                object obj = new
                {
                    punishdata = data,
                    kpidata = kpidata
                };

                return new { Code = 0, Count = -1, Info = "获取数据成功", data = obj };

            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }


        /// <summary>
        /// 获取历史审核记录
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetSpecialAuditList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string keyValue = res.Contains("id") ? dy.data.id : "";
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }

                object obj = aptitudeinvestigateauditbll.GetAuditList(keyValue).Where(p => p.REMARK != "0").ToList();
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
    }

    public class PunishModel
    {
        public string keyvalue { get; set; } 
        public AptitudeinvestigateauditEntity entity { get; set; }

        public SafepunishEntity punentity { get; set; }

       public SafekpidataEntity kpientity { get; set; }
    }
}
