using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.DangerousJob;
using ERCHTMS.Code;
using ERCHTMS.Entity.DangerousJob;
using ERCHTMS.Entity.SafePunish;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ERCHTMS.AppSerivce.Controllers
{
    public class DangerJobListController : BaseApiController
    {
        DangerjoblistBLL dangerJobListBll = new DangerjoblistBLL();
        public HttpContext ctx { get { return HttpContext.Current; } }

        // GET api/dangerjoblist
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/dangerjoblist/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/dangerjoblist
        public void Post([FromBody]string value)
        {
        }

        // PUT api/dangerjoblist/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/dangerjoblist/5
        public void Delete(int id)
        {
        }

        /// <summary>
        /// 保存危险作业清单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object SaveDangerJobList()
        {
            try
            {
                string res = ctx.Request["json"];
                var dyObj = JsonConvert.DeserializeAnonymousType(res, new
                {
                    userid = string.Empty,
                    data = new DangerjoblistEntity()
                });
                string userId = dyObj.userid; //当前用户ID 
                OperatorProvider.AppUserId = userId;  //设置当前用户
                string keyValue = !string.IsNullOrEmpty(dyObj.data.Id) ? dyObj.data.Id : "";
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                dangerJobListBll.SaveForm(keyValue, dyObj.data);

            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "保存失败" };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }


        /// <summary>
        /// 获取危险作业列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public Object GetDangerJobList([FromBody]JObject json)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;


                //模糊查询条件
                string keyword = res.Contains("keyword") ? dy.data.keyword : "";

                //危险作业级别
                string joblevel = res.Contains("joblevel") ? dy.data.joblevel : "";

                //作业人数
                string numberofpeople = res.Contains("numberofpeople") ? dy.data.numberofpeople : "";

                //组织机构查询
                string deptcode = res.Contains("deptcode") ? dy.data.deptcode : "";

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
                pagination.conditionJson = "1=1";

                string queryJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    keyword = keyword,
                    joblevel = joblevel,
                    numberofpeople = numberofpeople,
                    code = deptcode
                });
                var rewarddata = dangerJobListBll.GetPageList(pagination, queryJson);
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
        /// 获取危险作业详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetDangerJobDetail([FromBody]JObject json)
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
                object data = dangerJobListBll.GetEntity(keyValue);
                return new { Code = 0, Count = -1, Info = "获取数据成功", data = data };

            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }


        /// <summary>
        /// 删除危险作业
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object DeleteDangerJob([FromBody]JObject json)
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
                dangerJobListBll.RemoveForm(keyValue);
                return new { Code = 0, Count = -1, Info = "操作成功", data = "" };

            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
    }
}
