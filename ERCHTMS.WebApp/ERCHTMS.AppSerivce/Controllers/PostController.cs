using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ERCHTMS.AppSerivce.Model;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Cache;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BSFramework.Util.WebControl;
using BSFramework.Util;

namespace ERCHTMS.AppSerivce.Controllers
{
    public class PostController : BaseApiController
    {
        UserBLL userBll = new UserBLL();
        /// <summary>
        /// 根据部门Id获取岗位信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetPostByDeptId([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string DepartmentId = dy.data;
                //long pageIndex = dy.data.pageindex;
                //long pageSize = dy.data.pagesize;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000;
                pagination.sidx = "createdate";
                pagination.sord = "desc";
                pagination.p_kid = "roleid";
                pagination.p_fields = "t.fullname,t.deptname  DepartmentName,t.deptid DepartmentId，roleids,rolenames ";
                pagination.p_tablename = "base_role t";
                pagination.conditionJson = " category='2' ";
                var queryJson = new {
                    deptid = DepartmentId
                };
                //if (!string.IsNullOrWhiteSpace(DepartmentId)) {
                //    pagination.conditionJson += string.Format(@" and deptid='{0}'", DepartmentId);
                //}
                var data = new PostBLL().GetList(pagination, queryJson.ToJson());
                
                return new { Code = 0, Count = pagination.records, Info = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }


        /// <summary>
        /// 根据部门Id获取岗位信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetJobByDeptId([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string DepartmentId = dy.data;
                //long pageIndex = dy.data.pageindex;
                //long pageSize = dy.data.pagesize;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000;
                pagination.sidx = "createdate";
                pagination.sord = "desc";
                pagination.p_kid = "roleid";
                pagination.p_fields = "t.encode,t.fullname,t.deptname  DepartmentName,t.deptid DepartmentId ";
                pagination.p_tablename = "base_role t";
                pagination.conditionJson = " category='3' ";
                var queryJson = new
                {
                    deptid = DepartmentId
                };
                //if (!string.IsNullOrWhiteSpace(DepartmentId)) {
                //    pagination.conditionJson += string.Format(@" and deptid='{0}'", DepartmentId);
                //}
                var data = new JobBLL().GetList(pagination, queryJson.ToJson());

                return new { Code = 0, Count = pagination.records, Info = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

    }
}