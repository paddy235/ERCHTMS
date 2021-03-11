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
using ERCHTMS.Busines.AuthorizeManage;

namespace ERCHTMS.AppSerivce.Controllers
{
    public class DepartmentController : BaseApiController
    {
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private OrganizeBLL orgBLL = new OrganizeBLL();
        private DepartmentCache departmentCache = new DepartmentCache();
        private UserBLL userbll = new UserBLL();
        #region 获取公司机构(整改部门)
        /// <summary>
        /// 获取公司机构
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetDepartments([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string userid = dy.userid;
            //获取用户基本信息
            OperatorProvider.AppUserId = userid;  //设置当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            //获取当前部门
            string organizeId = curUser.OrganizeId;
            string parentId = "0";
            IList<DeptData> list = new List<DeptData>();
            //获取当前机构下的所有部门
            OrganizeEntity org = orgBLL.GetEntity(organizeId);
            DeptData dept = new DeptData();
            dept.deptid = org.OrganizeId;
            dept.code = org.EnCode;
            dept.oranizeid = org.OrganizeId;
            dept.parentcode = "";
            dept.parentid = parentId;
            dept.name = org.FullName;
            list.Add(dept);
            List<DepartmentEntity> data = new List<DepartmentEntity>();
            if (!curUser.IsSystem)
            {
                if (curUser.RoleName.Contains("厂级") || curUser.RoleName.Contains("公司"))
                {
                    data = departmentBLL.GetList().Where(t => t.OrganizeId == organizeId).OrderBy(t => t.SortCode).ToList();
                }
                else if (curUser.RoleName.Contains("部门"))      
                {
                    data = departmentBLL.GetList().Where(x => x.DepartmentId == curUser.DeptId || x.SendDeptID == curUser.DeptId || (x.Description == "外包工程承包商" && x.OrganizeId == organizeId)).OrderBy(t => t.SortCode).ToList();
                }
                else
                {
                    list = new List<DeptData>();
                    data = departmentBLL.GetDepts(curUser.DeptId).OrderBy(t => t.SortCode).ToList();
                }
            }
            foreach (DepartmentEntity entity in data)
            {
                DeptData depts = new DeptData();
                depts.deptid = entity.DepartmentId;
                depts.code = entity.EnCode;
                depts.oranizeid = entity.OrganizeId;
                depts.parentid = entity.ParentId;
                var parentDept = data.Where(p => p.DepartmentId == depts.parentid).FirstOrDefault();
                if (null != parentDept)
                {
                    depts.parentcode = parentDept.EnCode;
                }
                else
                {
                    depts.parentcode = "";
                }
                depts.name = entity.FullName;
                list.Add(depts);
            }
            return new { code = 0, info = "获取数据成功", count = data.Count(), data = list };
        }
        /// <summary>
        /// 获取各电厂（省公司用）
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetDepartmentsX([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string userid = dy.userid;
            //获取用户基本信息
            OperatorProvider.AppUserId = userid;  //设置当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            //获取省公司所有电厂
            List<DepartmentEntity> data = departmentBLL.GetList().Where(t => t.DeptCode.StartsWith(curUser.OrganizeCode) && t.Nature=="厂级").OrderBy(t => t.SortCode).ToList();
            var list = data.Select(x => { return new { departmentid=x.DepartmentId, encode=x.EnCode, fullname=x.FullName }; });   
            
            return new { code = 0, info = "获取数据成功", count = data.Count(), data = list };
        }
        #endregion

        #region 获取对应部门下的用户信息
        /// <summary>
        /// 获取对应部门下的用户信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetUserInfos([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string userid = dy.userid;
            //获取用户基本信息
            OperatorProvider.AppUserId = userid;  //设置当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            string deptcode = res.Contains("deptcode") ? dy.data.deptcode : "";

            List<UserInfoEntity> ulist = userbll.GetUserInfoByDeptCode(deptcode).ToList();

            return new { code = 0, info = "获取数据成功", count = ulist.Count(), data = ulist };
        }
        #endregion

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        /// <summary>
        /// 获取用户对模块的操作权限
        /// </summary>
        /// <param name="json"></param>
        /// <returns>结果True：有，False：无</returns>
        [HttpPost]
        public object HasOperAuthority([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid;
                string moduleEncode = dy.data.moduleEncode;
                string encode = dy.data.encode;
                //获取用户基本信息
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!", data = new object() };
                }

                bool hasOper = new AuthorizeBLL().HasOperAuthorityEncode(curUser, moduleEncode, encode);                
                return new
                {
                    code = 0,
                    info = "获取数据成功",
                    count = 0,
                    data = hasOper
                };                
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "获取失败，错误：" + ex.Message, data = new object() };
            }
        }
        /// <summary>
        /// 获取用户对模块的数据操作权限
        /// </summary>
        /// <param name="json">{userid:'111',data:{moduleEncode:"xxxx",encode:"修改、查看、删除等"}}</param>
        /// <returns>1:本人，2：本部门，3：本子部门，4：本机构，5：全部</returns>
        [HttpPost]
        public object GetOperAuthorityType([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid;
                string moduleEncode = dy.data.moduleEncode;
                string encode = dy.data.encode;
                //获取用户基本信息
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!", data = new object() };
                }

                string authority = new AuthorizeBLL().GetOperAuthorzeTypeEncode(curUser, moduleEncode, encode);
                return new
                {
                    code = 0,
                    info = "获取数据成功",
                    count = 0,
                    data = authority
                };
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "获取失败，错误：" + ex.Message, data = new object() };
            }
        }
    }
}