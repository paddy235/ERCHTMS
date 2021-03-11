using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Cache;
using ERCHTMS.Code;
using ERCHTMS.Entity.AuthorizeManage;
using ERCHTMS.Entity.BaseManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using BSFramework.Util.Offices;
using System.Drawing;
using System.Web;
using ERCHTMS.Entity.SystemManage.ViewModel;
using System.Text.RegularExpressions;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.SystemManage;
using BSFramework.Util.Attributes;
using BSFramework.Util.Extension;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Busines.PersonManage;
using Newtonsoft.Json;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Entity.PersonManage;
using System.Linq.Expressions;

namespace ERCHTMS.Web.Areas.BaseManage.Controllers
{
    /// <summary>
    /// 描 述：用户管理
    /// </summary>
    public class UserController : MvcControllerBase
    {
        private PostCache postCache = new PostCache();
        private PostBLL postBLL = new PostBLL();
        private UserBLL userBLL = new UserBLL();
        private UserCache userCache = new UserCache();
        private OrganizeBLL organizeBLL = new OrganizeBLL();
        private OrganizeCache organizeCache = new OrganizeCache();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private DepartmentCache departmentCache = new DepartmentCache();
        private ModuleFormInstanceBLL moduleFormInstanceBll = new ModuleFormInstanceBLL();
        private PermissionBLL permissionBLL = new PermissionBLL();
        private DataItemCache dic = new DataItemCache();
        private DataItemModel dm = new DataItemModel();
        private TemporaryGroupsBLL tempbll = new TemporaryGroupsBLL();


        #region 视图功能
        /// <summary>
        /// 用户管理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 账号时间限制设置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult TimeLimit()
        {
            return View();
        }
        /// <summary>
        /// Ldap账户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult IndexLdap()
        {
            return View();
        }
        /// <summary>
        /// 用户表单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult Form()
        {
            return View();
        }
        /// <summary>
        /// 选择用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult Select()
        {
            return View();
        }
        /// <summary>
        /// 选择用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult NoTransferSelect()
        {
            return View();
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[HandlerAuthorize(PermissionMode.Enforce)]
        public ActionResult RevisePassword()
        {
            return View();
        }
        /// <summary>
        /// 导入用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult Import()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <returns>返回机构+部门+用户树形Json</returns>
        [HttpGet]
        //[HandlerMonitor(3, "查询机构+部门+用户树形Json数据!")]
        public ActionResult GetTreeJson(string keyword)
        {
            var organizedata = organizeCache.GetList();
            var departmentdata = departmentCache.GetList();
            var userdata = userCache.GetList();
            var treeList = new List<TreeEntity>();
            foreach (OrganizeEntity item in organizedata)
            {
                #region 机构
                TreeEntity tree = new TreeEntity();
                bool hasChildren = organizedata.Count(t => t.ParentId == item.OrganizeId) == 0 ? false : true;
                if (hasChildren == false)
                {
                    hasChildren = departmentdata.Count(t => t.OrganizeId == item.OrganizeId) == 0 ? false : true;
                    //if (hasChildren == false)
                    //{
                    //    continue;
                    //}
                }
                tree.id = item.OrganizeId;
                tree.text = item.FullName;
                tree.value = item.OrganizeId;
                tree.parentId = item.ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                tree.Attribute = "Sort";
                tree.AttributeValue = "Organize";
                treeList.Add(tree);
                #endregion
            }
            foreach (DepartmentEntity item in departmentdata)
            {
                #region 部门
                TreeEntity tree = new TreeEntity();
                tree.id = item.DepartmentId;
                tree.text = item.FullName;
                tree.value = item.DepartmentId;
                if (item.ParentId == "0")
                {
                    tree.parentId = item.OrganizeId;
                }
                else
                {
                    tree.parentId = item.ParentId;
                }
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = true;
                tree.Attribute = "Sort";
                tree.AttributeValue = "Department";
                treeList.Add(tree);
                #endregion
            }
            foreach (UserEntity item in userdata)
            {
                #region 用户
                TreeEntity tree = new TreeEntity();
                tree.id = item.UserId;
                tree.text = item.RealName;
                tree.value = item.Account;
                tree.parentId = item.DepartmentId;
                tree.title = item.RealName + "（" + item.Account + "）";
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = false;
                tree.Attribute = "Sort";
                tree.AttributeValue = "User";
                tree.img = "fa fa-user";
                treeList.Add(tree);
                #endregion
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                treeList = treeList.TreeWhere(t => t.text.Contains(keyword), "id", "parentId");
            }
            return Content(treeList.TreeToJson());
        }

        [HttpGet]
        public ActionResult GetEntity(string keyValue)
        {
            var userEntity = userBLL.GetEntity(keyValue);
            if (!string.IsNullOrWhiteSpace(userEntity.SignImg))
            {
                if (!System.IO.File.Exists(Server.MapPath("~" + userEntity.SignImg.Replace("~", ""))))
                {
                    userEntity.SignImg = "";
                }
            }

            return Content(userEntity.ToJson());
        }

        /// <summary>
        /// 查询密码
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPassword(string userId)
        {
            try
            {
                var userEntity = userBLL.GetEntity(userId);
                if (userEntity != null)
                {
                    string password = DESEncrypt.Decrypt(userEntity.NewPassword, userEntity.Secretkey);
                    return Success(password);
                }
                return Error("用户信息不存在!");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 获取机构编码
        /// </summary>
        /// <param name="orgid">关键字</param>
        /// <returns>返回机构+部门+用户树形Json</returns>
        [HttpGet]
        public ActionResult GetOrganizeCode(string orgid)
        {
            var organizedata = organizeCache.GetEntity(orgid);
            return Content(organizedata.ToJson());
        }
        #region 获取机构部门组织树菜单
        /// <summary>
        /// 获取机构部门组织树菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDepartTreeJson(string orgCode = "", string orgId = "")
        {
            string parentId = "0";
            //暂时取所有，后期用户权限开发完成，数据将通过权限下来进行展示 
            Operator user = OperatorProvider.Provider.Current();
            if (!string.IsNullOrEmpty(orgCode))
            {
                user = new Operator { OrganizeCode = orgCode, OrganizeId = orgId };
            }
            List<OrganizeEntity> organizedata = new List<OrganizeEntity>();
            List<DepartmentEntity> departmentdata = new List<DepartmentEntity>();
            if (user.IsSystem)
            {
                organizedata = organizeCache.GetList().OrderByDescending(x => x.CreateDate).ToList();
                departmentdata = departmentCache.GetList().OrderBy(x => x.SortCode).ToList();
            }
            else
            {
                organizedata = organizeCache.GetList().Where(t => t.OrganizeId == user.OrganizeId).OrderByDescending(x => x.CreateDate).ToList();
                if (user.RoleName.Contains("集团公司") || user.RoleName.Contains("省级"))
                {
                    departmentdata = departmentBLL.GetList().Where(t => t.DeptCode.StartsWith(user.NewDeptCode)).ToList();
                    parentId = user.OrganizeId;
                }
                else
                {
                    if (user.RoleName.Contains("公司") || user.RoleName.Contains("电厂") || user.RoleName.Contains("厂级部门"))
                    {
                        departmentdata = departmentBLL.GetList().Where(t => t.EnCode.StartsWith(user.OrganizeCode)).ToList();
                        parentId = user.OrganizeId;
                    }
                    else
                    {
                        //含本部门及下属部门及管辖承包商
                        departmentdata = departmentBLL.GetList(user.OrganizeId).Where(t => t.EnCode.Contains(user.DeptCode) || t.Description == "外包工程承包商" || t.SendDeptID == user.DeptId).OrderBy(x => x.DeptCode).ToList();
                        //data = departmentBLL.GetList().Where(t => t.EnCode.StartsWith(user.DeptCode));
                        parentId = user.OrganizeId;
                    }
                }

            }
            var treeList = new List<TreeEntity>();
            foreach (DepartmentEntity item in departmentdata)
            {
                #region 部门
                //if (existAuthorizeData.Count(t => t.ResourceId == item.DepartmentId) == 0) continue;
                TreeEntity tree = new TreeEntity();
                tree.id = item.DepartmentId;
                tree.text = item.FullName;
                tree.value = item.DepartmentId;
                tree.parentId = item.ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = true;
                tree.Attribute = "Sort";
                tree.AttributeValue = "Department";
                tree.AttributeA = "EnCode";
                tree.AttributeValueA = item.EnCode;
                tree.AttributeB = "Nature";
                tree.AttributeValueB = item.Nature;
                treeList.Add(tree);
                #endregion
            }
            if (departmentdata.Count > 0)
            {
                parentId = departmentdata.OrderBy(t => t.DeptCode).FirstOrDefault().ParentId;
            }
            return Content(treeList.TreeToJson(parentId));
        }
        /// <summary>
        /// 获取机构部门组织树菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDeptTreeJson()
        {
            //暂时取所有，后期用户权限开发完成，数据将通过权限下来进行展示 
            Operator user = OperatorProvider.Provider.Current();
            List<OrganizeEntity> organizedata = new List<OrganizeEntity>();
            List<DepartmentEntity> departmentdata = new List<DepartmentEntity>();
            string roleNames = user.RoleName;
            if (user.IsSystem)
            {
                organizedata = organizeBLL.GetList().OrderByDescending(x => x.CreateDate).ToList();
                departmentdata = departmentBLL.GetList().OrderBy(x => x.SortCode).ToList();
            }
            else
            {
                organizedata = organizeBLL.GetList().Where(t => t.OrganizeId == user.OrganizeId).OrderByDescending(x => x.CreateDate).ToList();

                if (roleNames.Contains("公司级用户") || roleNames.Contains("厂级部门用户"))
                {
                    departmentdata = departmentBLL.GetList(user.OrganizeId).OrderBy(x => x.SortCode).ToList();
                }
                else if (roleNames.Contains("班组级用户"))
                {
                    departmentdata = departmentBLL.GetList(user.OrganizeId).Where(t => t.DepartmentId == user.DeptId).ToList();
                    departmentdata[0].ParentId = organizedata[0].OrganizeId;
                }
                else
                {
                    departmentdata = departmentBLL.GetList(user.OrganizeId).Where(t => t.EnCode.Contains(user.DeptCode) || t.Description == "外包工程承包商" || t.SendDeptID == user.DeptId).OrderBy(x => x.SortCode).ToList();
                }
            }
            var treeList = new List<TreeEntity>();
            foreach (OrganizeEntity item in organizedata)
            {
                #region 机构
                //if (existAuthorizeData.Count(t => t.ResourceId == item.OrganizeId) == 0) continue;
                TreeEntity tree = new TreeEntity();
                bool hasChildren = organizedata.Count(t => t.ParentId == item.OrganizeId) == 0 ? false : true;
                if (hasChildren == false)
                {
                    hasChildren = departmentdata.Count(t => t.OrganizeId == item.OrganizeId) == 0 ? false : true;
                }
                tree.id = item.OrganizeId;
                tree.text = item.FullName;
                tree.value = item.OrganizeId;
                tree.parentId = item.ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                tree.Attribute = "Sort";
                tree.AttributeValue = "Organize";
                tree.AttributeA = "EnCode";
                tree.AttributeValueA = item.EnCode;
                treeList.Add(tree);
                #endregion
            }
            departmentdata = departmentdata.Where(t => t.Nature == "部门" || t.Nature == "专业" || t.Nature == "班组" || t.Nature == "承包商" || t.Nature == "分包商").ToList();
            foreach (DepartmentEntity item in departmentdata)
            {
                #region 部门
                item.ParentId = item.Nature == "部门" ? "0" : item.ParentId;
                TreeEntity tree = new TreeEntity();
                tree.id = item.DepartmentId;
                tree.text = item.FullName;
                tree.value = item.DepartmentId;
                if (item.ParentId == "0")
                {
                    tree.parentId = item.OrganizeId;
                }
                else
                {
                    tree.parentId = item.ParentId;
                }
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = true;
                tree.Attribute = "Sort";
                tree.AttributeValue = (item.Nature == "分包商" || item.Nature == "承包商" || item.Description == "外包工程承包商") && !(roleNames.Contains("公司级用户") || roleNames.Contains("厂级部门用户")) ? "Contract" : "Department";
                tree.AttributeA = "EnCode";
                tree.AttributeValueA = item.EnCode;
                treeList.Add(tree);
                #endregion
            }
            return Content(treeList.TreeToJson());
        }
        #endregion


        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="departmentId">部门Id</param>
        /// <returns>返回用户列表Json</returns>
        [HttpGet]
        //[HandlerMonitor(3, "根据部门查询用户列表!")]
        public ActionResult GetListJson(string departmentId)
        {
            var data = userCache.GetList(departmentId);
            return Content(data.ToJson());
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>   
        //[HandlerMonitor(3, "分页查询用户信息!")]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "USERID";
            pagination.p_fields = "senddeptid,REALNAME,MOBILE,OrganizeName,ORGANIZEID,DEPTNAME,DEPARTMENTID,DEPARTMENTCODE,DUTYNAME,POSTNAME,ROLENAME,ROLEID,MANAGER,ENABLEDMARK,ENCODE,ACCOUNT,NICKNAME,GENDER,OrganizeCode,CreateDate,nature,istransfer,isleaving";
            pagination.p_tablename = "V_USERINFO t";
            pagination.conditionJson = "Account!='System' and ispresence='是'";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                var queryParam = queryJson.ToJObject();
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "departmentcode", "organizecode");
                if (!string.IsNullOrEmpty(where) && (queryParam["code"].IsEmpty() || !queryJson.Contains("code")))
                {
                    pagination.conditionJson += " and " + where;
                }

            }

            var watch = CommonHelper.TimerStart();
            var data = userBLL.GetPageList(pagination, queryJson);
            foreach (DataRow dr in data.Rows)
            {
                if (dr["nature"].ToString() == "专业" || dr["nature"].ToString() == "班组")
                {
                    DataTable dt = departmentBLL.GetDataTable(string.Format("select fullname from BASE_DEPARTMENT where encode=(select encode from BASE_DEPARTMENT t where instr('{0}',encode)=1 and nature='{1}' and organizeid='{2}') or encode='{0}' order by deptcode", dr["DEPARTMENTCODE"], "部门", dr["organizeid"]));
                    string name = "";
                    foreach (DataRow dr1 in dt.Rows)
                    {
                        name += dr1["fullname"].ToString() + "/";
                    }
                    dr["deptname"] = name.TrimEnd('/');
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
        /// <summary>
        /// 选择用户页面使用（不判断权限）
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetUserListJson(Pagination pagination, string queryJson)
        {
            try
            {
                pagination.p_kid = "USERID";
                pagination.p_fields = "REALNAME,MOBILE,OrganizeName,ORGANIZEID,DEPTNAME,DEPTCODE,DEPARTMENTID,DEPARTMENTCODE,DUTYNAME,POSTNAME,ROLENAME,ROLEID,MANAGER,ENABLEDMARK,ENCODE,ACCOUNT,NICKNAME,HEADICON,GENDER,EMAIL,OrganizeCode,identifyid,SIGNIMG,PARENTNAME,PARENTID,NATURE";
                pagination.p_tablename = " V_USERINFO t";
                pagination.conditionJson = "Account!='System' and ispresence='是'";
                //Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                //if (!user.IsSystem)
                //{
                //    pagination.conditionJson = "Account!='System'";
                //}
                //else
                //{
                //    pagination.conditionJson = "1=1";
                //}
                //string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                //if (!string.IsNullOrEmpty(authType))
                //{
                //    switch (authType)
                //    {
                //        case "1":
                //            pagination.conditionJson += " and createuserid='" + user.UserId + "'";
                //            break;
                //        case "2":
                //            pagination.conditionJson += " and departmentcode='" + user.DeptCode + "'";
                //            break;
                //        case "3":
                //            pagination.conditionJson += string.Format(" and (departmentcode like '{0}%' or departmentid in(select departmentid from BASE_DEPARTMENT t where t.senddeptid=(select departmentid from base_department d where d.encode='{0}')))", user.DeptCode);
                //            break;
                //        case "4":
                //            pagination.conditionJson += " and organizecode='" + user.OrganizeCode + "'";
                //            break;
                //    }
                //}
                var watch = CommonHelper.TimerStart();
                var data = userBLL.GetUserList(pagination, queryJson);
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
            catch(Exception ex)
            {
                return Error(ex.Message);
            }

        }

        /// <summary>
        /// 选择用户页面使用（不判断权限）
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetNoTransferJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "USERID";
            pagination.p_fields = "REALNAME,MOBILE,OrganizeName,ORGANIZEID,DEPTNAME,DEPTCODE,DEPARTMENTID,DEPARTMENTCODE,DUTYNAME,POSTNAME,ROLENAME,ROLEID,MANAGER,ENABLEDMARK,ENCODE,ACCOUNT,NICKNAME,HEADICON,GENDER,EMAIL,OrganizeCode,identifyid,SIGNIMG";
            pagination.p_tablename = "V_USERINFO t";
            pagination.conditionJson = "Account!='System' and ispresence='是' and istransfer=0";
            //Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            //if (!user.IsSystem)
            //{
            //    pagination.conditionJson = "Account!='System'";
            //}
            //else
            //{
            //    pagination.conditionJson = "1=1";
            //}
            //string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
            //if (!string.IsNullOrEmpty(authType))
            //{
            //    switch (authType)
            //    {
            //        case "1":
            //            pagination.conditionJson += " and createuserid='" + user.UserId + "'";
            //            break;
            //        case "2":
            //            pagination.conditionJson += " and departmentcode='" + user.DeptCode + "'";
            //            break;
            //        case "3":
            //            pagination.conditionJson += string.Format(" and (departmentcode like '{0}%' or departmentid in(select departmentid from BASE_DEPARTMENT t where t.senddeptid=(select departmentid from base_department d where d.encode='{0}')))", user.DeptCode);
            //            break;
            //        case "4":
            //            pagination.conditionJson += " and organizecode='" + user.OrganizeCode + "'";
            //            break;
            //    }
            //}
            var watch = CommonHelper.TimerStart();
            var data = userBLL.GetUserList(pagination, queryJson);
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
        /// 用户实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        //[HandlerMonitor(3, "查询用户对象信息!")]
        public ActionResult GetFormJson(string keyValue)
        {
            try
            {
                var userData = userBLL.GetEntity(keyValue);
                var dept = departmentBLL.GetEntity(userData.OrganizeId);
                string nature = "厂级";
                if (dept != null)
                {
                    nature = dept.Nature;
                }
                DataTable dt = departmentBLL.GetDataTable(string.Format("select t.px_account from XSS_USER t where t.useraccount='{0}'", userData.Account));
                if (dt.Rows.Count > 0)
                {
                    userData.Description = dt.Rows[0][0].ToString();
                }
                return Content(new { data = userData, nature = nature }.ToJson());
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
           
        }

        [HttpGet]
        public ActionResult GetFormJsonByAccount(string account)
        {
            var data = userBLL.GetList().Where(p => p.Account == account).FirstOrDefault();
            return ToJsonResult(data);
        }

        [HttpGet]
        public ActionResult GetUserInfo(string keyValue)
        {
            var data = userBLL.GetUserInfoEntity(keyValue);
            if (data == null)
            {
                data = userBLL.GetUserInfoByAccount(keyValue);
            }
            return Content(data.ToJson());
        }
        [HttpGet]
        public ActionResult GetUserInfoByAccount(string account)
        {
            var data = userBLL.GetUserInfoByAccount(account);
            return Content(data.ToJson());
        }


        /// <summary>
        /// 根据用id获取列表
        /// </summary>
        /// <param name="UserIDs"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUserJson(string UserIDs)
        {
            UserBLL ubll = new UserBLL();
            DataTable dt = ubll.GetUserTable(UserIDs.Split(','));
            //DataTable dt = new DataTable();
            return Content(dt.ToJson());
        }

       /// <summary>
       /// 获取目标条件下的用户数据
       /// </summary>
       /// <param name="queryJson"></param>
       /// <returns></returns>
        public ActionResult GetUserListByTargetCondition(string queryJson)
        {
            Pagination pagination = new Pagination();
            pagination.rows = 10000;
            pagination.page = 1;
            pagination.p_kid = "USERID";
            pagination.p_fields = "REALNAME,MOBILE,OrganizeName,ORGANIZEID,DEPTNAME,DEPTCODE,DEPARTMENTID,DEPARTMENTCODE,DUTYNAME,POSTNAME,ROLENAME,ROLEID,MANAGER,ENABLEDMARK,ENCODE,ACCOUNT,NICKNAME,HEADICON,GENDER,EMAIL,OrganizeCode,identifyid,SIGNIMG";
            pagination.p_tablename = "V_USERINFO t";
            pagination.conditionJson = "Account!='System' and ispresence='是' and istransfer=0";
            var watch = CommonHelper.TimerStart();
            var data = userBLL.GetUserList(pagination, queryJson);
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


        #region 根据用户角色，及用户机构获取用户
        /// <summary>
        /// 根据用户角色，及用户机构获取用户
        /// </summary>
        /// <param name="UserIDs"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUserJsonByOrgAndRole(string rolestr, string deptid, string orgid, string majorclassify="")
        {
            var data = userBLL.GetUserListByAnyCondition(orgid, deptid, rolestr, majorclassify);
            return Content(data.ToJson());
        }
        #endregion

        #region 初始化同步用户数据到海康平台

        /// <summary>
        /// 人员批量导入录入海康平台
        /// </summary>
        /// <param name="ulist"></param>
        public void ImportUserHik(IList<UserEntity> ulist)
        {
            DataItemDetailBLL itemBll = new DataItemDetailBLL();
            string KMIndex = itemBll.GetItemValue("KMIndexUrl");
            string HikHttps = itemBll.GetItemValue("HikHttps");
            List<UserEntity> list = new List<UserEntity>();
            if (!string.IsNullOrEmpty(KMIndex) || !string.IsNullOrEmpty(HikHttps))
            {//只允许可门电厂人员执行该操作
                List<TemporaryUserEntity> tempuserList = new TemporaryGroupsBLL().GetUserList();//所有临时人员
                List<TemporaryUserEntity> insertTemp = new List<TemporaryUserEntity>();
                List<DepartmentEntity> deptList = departmentBLL.GetList().ToList();
                foreach (var Us in ulist)
                {
                    #region 电厂人员同步到临时表中
                    var uentity = tempuserList.Where(t => t.USERID == Us.UserId).FirstOrDefault();
                    if (uentity == null)
                    {
                        //如果不存在于临时列表则新增一条数据
                        TemporaryUserEntity inserttuser = new TemporaryUserEntity();
                        inserttuser.Tel = Us.Account;
                        inserttuser.ComName = "";
                        inserttuser.CreateDate = Us.CreateDate;
                        inserttuser.CreateUserId = Us.CreateUserId;
                        inserttuser.USERID = Us.UserId;
                        inserttuser.Gender = Us.Gender;
                        inserttuser.ISDebar = 0;
                        inserttuser.Istemporary = 0;
                        inserttuser.Identifyid = Us.IdentifyID;
                        inserttuser.Postname = Us.DutyName;
                        inserttuser.UserName = Us.RealName;
                        inserttuser.Groupsid = Us.DepartmentId;
                        inserttuser.startTime = Us.CreateDate;
                        var dept1 = deptList.Where(it => it.DepartmentId == Us.DepartmentId).FirstOrDefault();
                        if (dept1 != null)
                        {
                            inserttuser.GroupsName = dept1.FullName;
                        }
                        insertTemp.Add(inserttuser);
                    }
                    #endregion
                    list.Add(Us);
                }
                new TemporaryGroupsBLL().SaveTemporaryList("", insertTemp);
                AddHikUser(list);
            }
        }

        /// <summary>
        /// 添加单个用户人员信息同步到海康平台
        /// </summary>
        public void AddSingleUser(string keyValue, UserEntity Us)
        {
            DataItemDetailBLL itemBll = new DataItemDetailBLL();
            string KMIndex = itemBll.GetItemValue("KMIndexUrl");
            string HikHttps = itemBll.GetItemValue("HikHttps");
            if (!string.IsNullOrEmpty(KMIndex) || !string.IsNullOrEmpty(HikHttps))
            {//只允许可门电厂人员执行该操作
                var uentity = new TemporaryGroupsBLL().GetEmptyUserEntity(Us.UserId);
                if (uentity == null)
                {//新增操作
                    #region 电厂人员同步到临时表中
                    List<TemporaryUserEntity> insertTemp = new List<TemporaryUserEntity>();
                    List<DepartmentEntity> deptList = departmentBLL.GetList().ToList();
                    //如果不存在于临时列表则新增一条数据
                    TemporaryUserEntity inserttuser = new TemporaryUserEntity();
                    inserttuser.Tel = Us.Account;
                    inserttuser.ComName = "";
                    inserttuser.CreateDate = Us.CreateDate;
                    inserttuser.CreateUserId = Us.CreateUserId;
                    inserttuser.USERID = Us.UserId;
                    inserttuser.Gender = Us.Gender;
                    inserttuser.ISDebar = 0;
                    inserttuser.Istemporary = 0;
                    inserttuser.Identifyid = Us.IdentifyID;
                    inserttuser.Postname = Us.DutyName;
                    inserttuser.UserName = Us.RealName;
                    inserttuser.Groupsid = Us.DepartmentId;
                    inserttuser.startTime = Us.CreateDate;
                    inserttuser.UserImg = Us.HeadIcon;
                    var dept1 = deptList.Where(it => it.DepartmentId == Us.DepartmentId).FirstOrDefault();
                    if (dept1 != null)
                    {
                        inserttuser.GroupsName = dept1.FullName;
                    }
                    insertTemp.Add(inserttuser);

                    new TemporaryGroupsBLL().SaveTemporaryList("", insertTemp);
                    #endregion

                    #region 同步到海康平台
                    AddSingleHikUser(Us);
                    #endregion
                }
                else
                {//修改操作
                    #region 同步修改操作
                    List<TemporaryUserEntity> updateTemp = new List<TemporaryUserEntity>();
                    List<DepartmentEntity> deptList = departmentBLL.GetList().ToList();
                    if (uentity != null)
                    {
                        uentity.Gender = Us.Gender;
                        uentity.Tel = Us.Mobile;
                        //uentity.UserImg = Us.HeadIcon;
                        uentity.Groupsid = Us.DepartmentId;
                        uentity.Postname = Us.DutyName;
                        uentity.UserName = Us.RealName;
                        var dept1 = deptList.Where(it => it.DepartmentId == Us.DepartmentId).FirstOrDefault();
                        if (dept1 != null)
                        {
                            uentity.GroupsName = dept1.FullName;
                        }
                        updateTemp.Add(uentity);
                    }
                    new TemporaryGroupsBLL().SaveTemporaryList(keyValue, updateTemp);
                    EditHikUser(Us);
                    #endregion
                }
            }
        }
        /// <summary>
        /// 账号时间限制
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerAuthorize(PermissionMode.Enforce)]
        [HandlerMonitor(5, "账号有效时间设置")]
        public ActionResult SaveLimit(string keyValue, DateTime? AllowStartTime, DateTime? AllowEndTime)
        {
            try
            {
                if (AllowStartTime == null || AllowEndTime == null)
                {
                    return Error("开始时间必须小于结束时间！");
                }
                if (AllowStartTime > AllowEndTime)
                {
                    return Error("开始时间必须小于结束时间！");
                }
                Expression<Func<UserEntity, bool>> condition = e => e.Account == keyValue;
                UserEntity user = userBLL.GetListForCon(condition).FirstOrDefault();
                if (user != null)
                {
                    user.AllowStartTime = AllowStartTime;
                    user.AllowEndTime = AllowEndTime;
                    string result = userBLL.SaveForm(user.UserId, user);
                    if (string.IsNullOrWhiteSpace(result))
                    {
                        return Error("操作失败！");
                    }
                    return Success("操作成功");
                }
                else
                {
                    return Error("用户信息不存在！");
                }

            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        [HttpPost]
        [AjaxOnly]
        [HandlerMonitor(5, "同步人员信息到海康安防平台")]
        public ActionResult InitHikUser()
        {
            List<UserEntity> dept = userBLL.GetList().ToList();

            string rtnMsg = AddHikUser(dept);

            #region 电厂人员同步到临时表中
            List<TemporaryUserEntity> insertTemp = new List<TemporaryUserEntity>();
            List<DepartmentEntity> deptList = departmentBLL.GetList().ToList();
            List<TemporaryUserEntity> tempuserList = new TemporaryGroupsBLL().GetUserList();//所有临时人员
            if (dept != null)
            {
                foreach (var Us in dept)
                {
                    var uentity = tempuserList.Where(t => t.USERID == Us.UserId).FirstOrDefault();
                    if (uentity == null)
                    {
                        //如果不存在于临时列表则新增一条数据
                        TemporaryUserEntity inserttuser = new TemporaryUserEntity();
                        inserttuser.Tel = Us.Account;
                        inserttuser.ComName = "";
                        inserttuser.Create(Us.UserId);
                        inserttuser.USERID = Us.UserId;
                        inserttuser.Gender = Us.Gender;
                        inserttuser.ISDebar = 0;
                        inserttuser.Istemporary = 0;
                        inserttuser.Identifyid = Us.IdentifyID;
                        inserttuser.Postname = Us.DutyName;
                        inserttuser.UserName = Us.RealName;
                        inserttuser.Groupsid = Us.DepartmentId;
                        inserttuser.startTime = Us.CreateDate;
                        var dept1 = deptList.Where(it => it.DepartmentId == Us.DepartmentId).FirstOrDefault();
                        if (dept1 != null)
                        {
                            inserttuser.GroupsName = dept1.FullName;
                        }
                        insertTemp.Add(inserttuser);
                    }
                }
            }
            new TemporaryGroupsBLL().SaveTemporaryList("", insertTemp);
            #endregion
            UserAddRtnMsg rtnList = JsonConvert.DeserializeObject<UserAddRtnMsg>(rtnMsg);

            if (rtnList.data.successes.Count > 0 && rtnList.data.failures.Count == 0)
            {
                return Success("操作成功。");
            }
            else
            {
                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                if (!System.IO.Directory.Exists(HttpContext.Server.MapPath("~/logs")))
                {
                    System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath("~/logs"));
                }
                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：同步安防平台数据失败，异常信息：" + rtnMsg + "\r\n");
                return Success("部分同步失败,具体情况请查询日志。");

            }
        }

        /// <summary>
        /// 批量同步到海康平台
        /// </summary>
        /// <param name="dept"></param>
        /// <returns></returns>
        public string AddHikUser(List<UserEntity> dept)
        {
            DataItemDetailBLL data = new DataItemDetailBLL();
            var pitem = data.GetItemValue("Hikappkey");//海康服务器密钥
            var baseurl = data.GetItemValue("HikBaseUrl");//海康服务器地址
            string HikHttps = data.GetItemValue("HikHttps");//海康1.4及以上版本https
            string Key = string.Empty;
            string Signature = string.Empty;
            if (!string.IsNullOrEmpty(pitem))
            {
                Key = pitem.Split('|')[0];
                Signature = pitem.Split('|')[1];
            }
            string Url = "/artemis/api/resource/v1/person/batch/add";//接口地址

            List<object> modelList = new List<object>();
            int i = 1;
            foreach (var item in dept)
            {
                int Gender = 0;
                if (item.Gender == "男")
                {
                    Gender = 1;
                }
                else
                {
                    Gender = 2;
                }

                var model = new
                {
                    clientId = i,
                    personId = item.UserId,
                    personName = item.RealName,
                    orgIndexCode = item.DepartmentId,
                    gender = Gender,
                    phoneNo = item.Mobile,
                    certificateType = "111",
                    certificateNo = item.IdentifyID,
                    jobNo = item.EnCode == "" ? item.Account : item.EnCode
                };
                modelList.Add(model);
            }
            string rtnMsg = string.Empty;
            if (!string.IsNullOrEmpty(HikHttps))
            {
                HttpUtillibKbs.SetPlatformInfo(Key, Signature, baseurl, 443, true);
                byte[] result = HttpUtillibKbs.HttpPost(Url, JsonConvert.SerializeObject(modelList), 20);
                if (result != null)
                {
                    rtnMsg = System.Text.Encoding.UTF8.GetString(result);
                }
            }
            else
            {
                rtnMsg = SocketHelper.LoadCameraList(modelList, baseurl, Url, Key, Signature);
            }
            return rtnMsg;
        }

        /// <summary>
        /// 同步修改海康人员信息
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string EditHikUser(UserEntity item)
        {
            DataItemDetailBLL data = new DataItemDetailBLL();
            var pitem = data.GetItemValue("Hikappkey");//海康服务器密钥
            var baseurl = data.GetItemValue("HikBaseUrl");//海康服务器地址
            string HikHttps = data.GetItemValue("HikHttps");//海康1.4及以上版本https
            string Key = string.Empty;
            string Signature = string.Empty;
            if (!string.IsNullOrEmpty(pitem))
            {
                Key = pitem.Split('|')[0];
                Signature = pitem.Split('|')[1];
            }
            string Url = "/artemis/api/resource/v1/person/single/update";
            int Gender = 0;
            if (item.Gender == "男")
            {
                Gender = 1;
            }
            else
            {
                Gender = 2;
            }

            var model = new
            {
                personId = item.UserId,
                personName = item.RealName,
                orgIndexCode = item.DepartmentId,
                gender = Gender,
                phoneNo = item.Mobile,
                certificateType = "111",
                certificateNo = item.IdentifyID,
                jobNo = item.EnCode
            };
            string rtnMsg = string.Empty;
            if (!string.IsNullOrEmpty(HikHttps))
            {
                HttpUtillibKbs.SetPlatformInfo(Key, Signature, baseurl, 443, true);
                byte[] result = HttpUtillibKbs.HttpPost(Url, JsonConvert.SerializeObject(model), 20);
                if (result != null)
                {
                    rtnMsg = System.Text.Encoding.UTF8.GetString(result);
                }
            }
            else
            {
                rtnMsg = SocketHelper.LoadCameraList(model, baseurl, Url, Key, Signature);
            }
            return rtnMsg;
        }
        /// <summary>
        /// 同步删除海康人员信息
        /// </summary>
        public ActionResult DeleteHikUser(string userid)
        {
            DataItemDetailBLL itemBll = new DataItemDetailBLL();
            string KMIndex = itemBll.GetItemValue("KMIndexUrl");
            string HikHttps = itemBll.GetItemValue("HikHttps");
            if (!string.IsNullOrEmpty(KMIndex) || !string.IsNullOrEmpty(HikHttps))
            {//只允许可门电厂人员执行该操作
                List<TemporaryUserEntity> list = new List<TemporaryUserEntity>();
                DataItemDetailBLL data = new DataItemDetailBLL();
                var pitem = data.GetItemValue("Hikappkey");//海康服务器密钥
                var baseurl = data.GetItemValue("HikBaseUrl");//海康服务器地址
                
                string Key = string.Empty;
                string Signature = string.Empty;
                if (!string.IsNullOrEmpty(pitem))
                {
                    Key = pitem.Split('|')[0];
                    Signature = pitem.Split('|')[1];
                }

                // List<TemporaryUserEntity> tempuserList = tempbll.GetUserList();//所有临时人员
                var uentity = tempbll.GetEmptyUserEntity(userid);
                if (uentity != null)
                {
                    //临时表记录
                    tempbll.DeleteTemporaryList("", uentity);

                    if (!string.IsNullOrEmpty(uentity.Postname))
                    {//已授权删除设备中对应的出入权限
                        list.Add(uentity);
                        tempbll.DeleteUserlimits(list, baseurl, Key, Signature);
                    }

                    //海康平台记录
                    string Url = "/artemis/api/resource/v1/person/batch/delete";//接口地址
                    List<string> dellist = new List<string>();
                    dellist.Add(uentity.USERID);
                    var model = new
                    {
                        personIds = dellist
                    };
                    if (!string.IsNullOrEmpty(HikHttps))
                    {
                        HttpUtillibKbs.SetPlatformInfo(Key, Signature, baseurl, 443, true);
                        byte[] result = HttpUtillibKbs.HttpPost(Url, JsonConvert.SerializeObject(model), 20);
                        //rtnMsg = System.Text.Encoding.UTF8.GetString(result);
                    }
                    else
                    {
                        SocketHelper.LoadCameraList(model, baseurl, Url, Key, Signature);
                    }
                }
            }
            return Success("删除成功。");
        }

        /// <summary>
        /// 单条数据同步
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string AddSingleHikUser(UserEntity item)
        {
            DataItemDetailBLL data = new DataItemDetailBLL();
            var pitem = data.GetItemValue("Hikappkey");//海康服务器密钥
            var baseurl = data.GetItemValue("HikBaseUrl");//海康服务器地址
            string HikHttps = data.GetItemValue("HikHttps");//海康1.4及以上版本https
            string Key = string.Empty;
            string Signature = string.Empty;
            if (!string.IsNullOrEmpty(pitem))
            {
                Key = pitem.Split('|')[0];
                Signature = pitem.Split('|')[1];
            }
            string Url = "/artemis/api/resource/v1/person/single/add";//接口地址
            int Gender = 0;
            if (item.Gender == "男")
            {
                Gender = 1;
            }
            else
            {
                Gender = 2;
            }
            var model = new
            {
                personId = item.UserId,
                personName = item.RealName,
                orgIndexCode = item.DepartmentId,
                gender = Gender,
                phoneNo = item.Mobile,
                certificateType = "111",
                certificateNo = item.IdentifyID,
                jobNo = item.EnCode == "" ? item.Account : item.EnCode
            };
            string rtnMsg = string.Empty;
            if (!string.IsNullOrEmpty(HikHttps))
            {
                HttpUtillibKbs.SetPlatformInfo(Key, Signature, baseurl, 443, true);
                byte[] result = HttpUtillibKbs.HttpPost(Url, JsonConvert.SerializeObject(model), 20);
                if (result != null)
                {
                    rtnMsg = System.Text.Encoding.UTF8.GetString(result);
                }
            }
            else
            {
                rtnMsg = SocketHelper.LoadCameraList(model, baseurl, Url, Key, Signature);
            }
            return rtnMsg;
        }

        #endregion

        #region 验证数据
        /// <summary>
        /// 账户不能重复
        /// </summary>
        /// <param name="Account">账户值</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExistAccount(string Account, string keyValue)
        {
            bool IsOk = userBLL.ExistAccount(Account, keyValue);
            return Content(IsOk.ToString());
        }
        /// <summary>
        /// 身份证不能重复
        /// </summary>
        /// <param name="IdentifyID">身份证号</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExistIdentifyID(string IdentifyID, string keyValue)
        {
            bool IsOk = userBLL.ExistIdentifyID(IdentifyID, keyValue);
            if (!IsOk)
            {
                UserEntity user = userBLL.GetUserByIdCard(IdentifyID);
                if (user != null)
                {
                    if (user.IsBlack == 1)
                    {
                        return Content("isBlack");
                    }
                }
            }
            return Content(IsOk.ToString());
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerAuthorize(PermissionMode.Ignore)]
        [HandlerMonitor(6, "删除用户信息")]
        public ActionResult RemoveForm(string keyValue)
        {
            UserInfoEntity entity = userBLL.GetUserInfoEntity(keyValue);
            Operator user = OperatorProvider.Provider.Current();
            if (keyValue == "System")
            {
                throw new Exception("当前账户不能删除");
            }
            userBLL.RemoveForm(keyValue);
            DeleteHikUser(keyValue);

            //毕节删除
            UserEntity uentity = userBLL.GetEntity(keyValue);
            DeleteHdgzUser(uentity);

            string moduleId = SystemInfo.CurrentModuleId;
            string moduleName = SystemInfo.CurrentModuleName;

            DataItemDetailBLL di = new DataItemDetailBLL();
            var task = Task.Factory.StartNew(() =>
            {
                LogEntity logEntity = new LogEntity();
                logEntity.Browser = this.Request.Browser.Browser;
                logEntity.CategoryId = 6;
                logEntity.OperateTypeId = "6";
                logEntity.OperateType = "删除";
                logEntity.OperateAccount = user.UserName;
                logEntity.OperateUserId = user.UserId;
                logEntity.ExecuteResult = 1;
                logEntity.Module = moduleName;
                logEntity.ModuleId = moduleId;
                logEntity.ExecuteResultJson = "操作信息:删除姓名为" + entity.RealName + ",账号为" + entity.Account + "的" + entity.OrganizeName + "-" + entity.DeptName + "的用户, 详细信息:" + entity.ToJson();
                LogBLL.WriteLog(logEntity);
                if (!string.IsNullOrEmpty(di.GetItemValue("bzAppUrl")))
                {
                    DeleteUser(entity, user);
                }
            });
            DepartmentEntity org = departmentBLL.GetEntity(entity.OrganizeId);
            string way = di.GetItemValue("WhatWay");
            //对接.net培训平台
            if (way == "0")
            {

            }
            //对接java培训平台
            if (way == "1" && org.IsTrain==1)
            {
                Task.Factory.StartNew(() =>
                {
                        object obj = new
                        {
                            action = "delete",
                            time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            userId = keyValue,
                            account = entity.Account,
                            companyId=org.InnerPhone
                        };
                        List<object> list = new List<object>();
                        list.Add(obj);
                        Busines.JPush.JPushApi.PushMessage(list, 1);

                        LogEntity logEntity = new LogEntity();
                        logEntity.CategoryId =5;
                        logEntity.OperateTypeId = ((int)OperationType.Delete).ToString();
                        logEntity.OperateType = "删除用户";
                        logEntity.OperateAccount = user.Account + "（" + user.UserName + "）";
                        logEntity.OperateUserId = user.UserId;

                        logEntity.ExecuteResult = -1;
                        logEntity.ExecuteResultJson = string.Format("同步用户(删除用户)到java培训平台,同步信息：{0}", list.ToJson());
                        logEntity.Module = moduleName;
                        logEntity.ModuleId = moduleId;
                        logEntity.WriteLog();
                });
            }
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存用户表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="userEntity">用户实体</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "保存(新增或修改)用户信息")]
        public ActionResult SaveForm(string keyValue, string strUserEntity, string FormInstanceId, string strModuleFormInstanceEntity)
        {
            try
            {
                string action = "add";
                UserEntity userEntity = strUserEntity.ToObject<UserEntity>();
                if (!userBLL.ExistAccount(userEntity.Account, keyValue))
                {
                    return Error("操作失败，账号已存在!");
                }
                string res = userBLL.IsBalckUser(keyValue, userEntity.IdentifyID);
                if (!string.IsNullOrWhiteSpace(res))
                {
                    return Error(string.Format("该用户于时间{0}被加入黑名单！", res));
                }
               
                ModuleFormInstanceEntity moduleFormInstanceEntity = strModuleFormInstanceEntity.ToObject<ModuleFormInstanceEntity>();
                bool isChage = false;//是否改变部门
                if (keyValue != "")
                {
                    //首先获取原来用户实体 判断是否修改部门 如果修改部门则生成工作记录
                    UserEntity OlduserEntity = userBLL.GetEntity(keyValue);
                    //如果换了部门 或者换了岗位
                    if (OlduserEntity == null || OlduserEntity.DepartmentId != userEntity.DepartmentId || OlduserEntity.DutyId != userEntity.DutyId)
                    {
                        isChage = true;

                    }
                }
                string pwd = userEntity.Password;
                UserInfoEntity curUser = userBLL.GetUserInfoByAccount(userEntity.Account);
                if (curUser != null)
                {
                    action = "edit";
                    pwd = null;
                }
                else
                {
                    pwd = string.IsNullOrWhiteSpace(userEntity.Password) ? "Abc123456" : userEntity.Password;
                }
                //获取职务Code
                if (userEntity.PostId != null && userEntity.PostId != "")
                {
                    string postcode = "";
                    IEnumerable<RoleEntity> rlist = new JobBLL().GetList();
                    string[] Postids = userEntity.PostId.Split(',');
                    for (int i = 0; i < Postids.Length; i++)
                    {
                        RoleEntity ro = rlist.Where(it => it.RoleId == Postids[i]).FirstOrDefault();
                        if (ro != null)
                        {
                            if (postcode == "")
                            {
                                postcode = ro.EnCode;
                            }
                            else
                            {
                                postcode += "," + ro.EnCode;
                            }
                        }
                    }

                    userEntity.PostCode = postcode;
                }
                DataItemDetailBLL di = new DataItemDetailBLL();

                userEntity.Account = userEntity.Account.Trim();
                //LDAP
                var isldap = di.GetItemValue("IsOpenPassword");
                if (isldap == "true")
                {
                    //if (userEntity.IsapplicationLdap == "1") {
                    UserEntity user = userBLL.GetEntity(keyValue);
                    if (user == null)
                    {
                        string ldapaccount = "CRP_";
                        int n = 0;
                        foreach (char c in userEntity.RealName)
                        {
                            n++;
                            if (n == 1)
                            {
                                ldapaccount += (ConvertToPinYin(c.ToString())).ToUpper();
                            }
                            else
                            {
                                ldapaccount += ((Str.PinYin(c.ToString())).ToUpper()).Substring(0, 1);
                            }
                        }
                        ldapaccount += (userEntity.IdentifyID).Substring((userEntity.IdentifyID).Length - 4);
                        userEntity.Account = ldapaccount;
                        userEntity.Password = "Abcd1234";
                        //userEntity.Secretkey = Md5Helper.MD5(CommonHelper.CreateNo(), 16).ToLower();
                        //userEntity.Password = DESEncrypt.Encrypt("Abcd1234", userEntity.Secretkey);
                        userEntity.EnCode = ldapaccount;

                        if (!userBLL.ExistAccount(ldapaccount))
                        {
                            return Error("操作失败，生成的帐号（" + ldapaccount + "）重复");
                        }
                    }
                    //}
                }

                if (!string.IsNullOrWhiteSpace(userEntity.IdentifyID) && action == "add")
                {
                    string error = "";
                    string birthday = "";
                    if (CheckIdCard(userEntity.IdentifyID, out error, out birthday))
                    {
                        int len = 0;
                        string[] arr1 = new string[] { };//普通人员年龄条件
                        string[] arr2 = new string[] { };//特种作业人员年龄条件
                        string[] arr3 = new string[] { };//监理人员年龄条件
                        string[] arr4 = new string[] { };//特种设备作业人员年龄条件
                        int isCL = departmentBLL.GetDataTable(string.Format("select count(1) from BIS_BLACKSET where itemcode='10' and deptcode='{0}' and status=1", userEntity.OrganizeCode)).Rows[0][0].ToInt();
                        if (isCL > 0)
                        {
                            DataTable dtItems = departmentBLL.GetDataTable(string.Format("select itemvalue from BIS_BLACKSET t where status=1 and deptcode='{0}' and (t.itemcode='01' or t.itemcode='06' or t.itemcode='07' or t.itemcode='08') order by itemcode", userEntity.OrganizeCode));
                            len = dtItems.Rows.Count;

                            StringBuilder sb = new StringBuilder();
                            if (len > 0)
                            {
                                arr1 = dtItems.Rows[0][0].ToString().Split('|');
                            }
                            if (len > 1)
                            {
                                arr2 = dtItems.Rows[1][0].ToString().Split('|');
                            }
                            if (len > 2)
                            {
                                arr3 = dtItems.Rows[2][0].ToString().Split('|');
                            }
                            if (len > 3)
                            {
                                arr4 = dtItems.Rows[3][0].ToString().Split('|');
                            }
                        }
                        userEntity.Birthday = DateTime.Parse(birthday);
                        userEntity.Age = (DateTime.Now.Year - userEntity.Birthday.Value.Year).ToString();
                        if (isCL > 0 && len > 0)
                        {
                            //判断是否超龄人员
                            if (userEntity.UserType == "监理人员" && arr3.Length > 0)
                            {
                                if (userEntity.Gender == "男" && (userEntity.Age.ToInt() < arr3[0].ToInt() || userEntity.Age.ToInt() > arr3[1].ToInt()))
                                {
                                    return Error("该人员年龄超龄，无法录入系统！");
                                }
                                if (userEntity.Gender == "女" && (userEntity.Age.ToInt() < arr3[2].ToInt() || userEntity.Age.ToInt() > arr3[3].ToInt()))
                                {
                                    return Error("该人员年龄超龄，无法录入系统！");
                                }
                            }
                            if (userEntity.IsSpecial == "是" && arr2.Length > 0)
                            {
                                if (userEntity.Gender == "男" && (userEntity.Age.ToInt() < arr2[0].ToInt() || userEntity.Age.ToInt() > arr2[1].ToInt()))
                                {
                                    return Error("该人员年龄超龄，无法录入系统！");
                                }
                                if (userEntity.Gender == "女" && (userEntity.Age.ToInt() < arr2[2].ToInt() || userEntity.Age.ToInt() > arr2[3].ToInt()))
                                {
                                    return Error("该人员年龄超龄，无法录入系统！");
                                }
                            }
                            if (userEntity.IsSpecialEqu == "是" && arr4.Length > 0)
                            {
                                if (userEntity.Gender == "男" && (userEntity.Age.ToInt() < arr4[0].ToInt() || userEntity.Age.ToInt() > arr4[1].ToInt()))
                                {
                                    return Error("该人员年龄超龄，无法录入系统！");
                                }
                                if (userEntity.Gender == "女" && (userEntity.Age.ToInt() < arr4[2].ToInt() || userEntity.Age.ToInt() > arr4[3].ToInt()))
                                {
                                    return Error("该人员年龄超龄，无法录入系统！");
                                }
                            }
                            if (userEntity.IsSpecialEqu == "否" && userEntity.IsSpecial == "否" && userEntity.UserType != "监理人员" && arr1.Length > 0)
                            {
                                if (userEntity.Gender == "男" && (userEntity.Age.ToInt() < arr1[0].ToInt() || userEntity.Age.ToInt() > arr1[1].ToInt()))
                                {
                                    return Error("该人员年龄超龄，无法录入系统！");
                                }
                                if (userEntity.Gender == "女" && (userEntity.Age.ToInt() < arr1[2].ToInt() || userEntity.Age.ToInt() > arr1[3].ToInt()))
                                {
                                    return Error("该人员年龄超龄，无法录入系统！");
                                }
                            }
                        }
                    }
                    else
                    {
                        return Error("请填写正确的身份证号！");
                    }
                }
                string objectId = userBLL.SaveForm(keyValue, userEntity);
                if (!string.IsNullOrEmpty(objectId))
                {
                    AddSingleUser(keyValue, userEntity);
                    moduleFormInstanceEntity.ObjectId = objectId;
                    moduleFormInstanceBll.SaveEntity(FormInstanceId, moduleFormInstanceEntity);
                    if (isChage)
                    {
                        UserInfoEntity uinfo = userBLL.GetUserInfoEntity(keyValue);
                        new WorkRecordBLL().WriteChangeRecord(uinfo, OperatorProvider.Provider.Current());
                    }

                    if (!string.IsNullOrWhiteSpace(di.GetItemValue("bzAppUrl")))
                    {
                        userEntity.Password = pwd;
                        var task = Task.Factory.StartNew(() =>
                        {
                            List<UserEntity> lstUsers = new List<UserEntity>();
                            lstUsers.Add(userEntity);
                            userBLL.SyncUsersToBZ(lstUsers);
                        });
                    }
                    string way = di.GetItemValue("WhatWay");
                    DepartmentEntity org = departmentBLL.GetEntity(userEntity.OrganizeId);
                    if (org.IsTrain == 1)
                    {
                        //对接.net培训平台
                        if (way == "0")
                        {

                        }
                        //对接java培训平台
                        if (way == "1" && org.IsTrain == 1)
                        {

                            if (org.IsTrain == 1)
                            {
                                var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                                userEntity = userBLL.GetEntity(userEntity.UserId);
                                DepartmentEntity dept = departmentBLL.GetEntity(userEntity.DepartmentId);
                                if (dept != null && userEntity != null)
                                {
                                    string deptId = userEntity.DepartmentId;
                                    string enCode = userEntity.DepartmentCode;
                                    if (!string.IsNullOrWhiteSpace(dept.DeptKey))
                                    {
                                        string[] arr = dept.DeptKey.Split('|');
                                        deptId = arr[0];
                                        if (arr.Length > 1)
                                        {
                                            enCode = arr[1];
                                        }
                                    }
                                    string ModuleName = SystemInfo.CurrentModuleName;
                                    string ModuleId = SystemInfo.CurrentModuleId;
                                    Task.Factory.StartNew(() =>
                                    {
                                        object obj = new
                                        {
                                            action = action,
                                            time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                            userId = userEntity.UserId,
                                            userName = userEntity.RealName,
                                            account = userEntity.Account,
                                            deptId = deptId,
                                            deptCode = enCode,
                                            password = pwd, //为null时不要修改密码!
                                            sex = userEntity.Gender,
                                            idCard = userEntity.IdentifyID,
                                            email = userEntity.Email,
                                            mobile = userEntity.Mobile,
                                            birth = userEntity.Birthday == null ? "" : userEntity.Birthday.Value.ToString("yyyy-MM-dd"),//生日
                                            postId = userEntity.DutyId,
                                            postName = userEntity.DutyName,//岗位
                                            age = userEntity.Age.ToIntOrNull(),//年龄
                                            native = userEntity.Native, //籍贯
                                            nation = userEntity.Nation, //民族
                                            encode = userEntity.EnCode,//工号
                                            jobTitle = userEntity.JobTitle,
                                            techLevel = userEntity.TechnicalGrade,
                                            workType = userEntity.Craft,
                                            companyId=org.InnerPhone,
                                            //signContent = userBLL.GetSignContent(userEntity.SignImg),//人员签名照
                                            trainRoles = userEntity.TrainRoleId,
                                            role = userEntity.IsTrainAdmin == null ? 0 : userEntity.IsTrainAdmin //角色（0:学员，1:培训管理员）
                                        };
                                        List<object> list = new List<object>();
                                        list.Add(obj);
                                        Busines.JPush.JPushApi.PushMessage(list, 1);


                                        LogEntity logEntity = new LogEntity();
                                        logEntity.CategoryId = 5;
                                        logEntity.OperateTypeId = ((int)OperationType.Update).ToString();
                                        logEntity.OperateType = "新增或修改用户";
                                        logEntity.OperateAccount = user.Account + "（" + user.UserName + "）";
                                        logEntity.OperateUserId = user.UserId;

                                        logEntity.ExecuteResult = 1;
                                        logEntity.ExecuteResultJson = string.Format("同步用户到java培训平台,同步信息:\r\n{0}", list.ToJson());
                                        logEntity.Module = ModuleName;
                                        logEntity.ModuleId = ModuleId;
                                        logEntity.WriteLog();
                                    });
                                }
                            }

                        }
                    }
                    //DepartmentEntity dept = departmentBLL.GetEntity(userEntity.OrganizeId);
                    //if (dept.IsTrain == 1)
                    //{
                    //    Operator user = OperatorProvider.Provider.Current();
                    //    var task = Task.Factory.StartNew(() =>
                    //    {
                    //        userBLL.SyncUser(userEntity, pwd, user);
                    //    });
                    //}
                    return Success("操作成功。");
                }
                else
                {
                    return Error("操作失败。账号或工号(人员编号)或手机号已存在!");
                }

            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
          
        }
        private void DeleteUser(UserInfoEntity user, Operator currUser)
        {

            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            //发送请求到web api并获取返回值，默认为post方式
            try
            {
                System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                //当前操作用户账号
                nc.Add("account", currUser.Account);
                nc.Add("json", Newtonsoft.Json.JsonConvert.SerializeObject(user));
                wc.UploadValuesCompleted += wc_UploadValuesCompleted1;
                wc.UploadValuesAsync(new Uri(new DataItemDetailBLL().GetItemValue("bzAppUrl") + "DeleteUser?keyValue=" + user.UserId), nc);

            }
            catch (Exception ex)
            {
                if (!System.IO.Directory.Exists(HttpContext.Server.MapPath("~/logs/syncbz")))
                {
                    System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath("~/logs/syncbz"));
                }
                //将同步结果写入日志文件
                string fileName = "user_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/syncbz/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：删除数据失败，用户信息" + Newtonsoft.Json.JsonConvert.SerializeObject(user) + ",异常信息：" + ex.Message + "\r\n");
            }
        }
        void wc_UploadValuesCompleted1(object sender, UploadValuesCompletedEventArgs e)
        {
            string fileName = "user_"+DateTime.Now.ToString("yyyyMMdd") + ".log";
            string logPath = new DataItemDetailBLL().GetItemValue("imgPath") + "\\logs\\syncbz\\";
            try
            {
                //将同步结果写入日志文件
                System.IO.File.AppendAllText(logPath + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：" + System.Text.Encoding.UTF8.GetString(e.Result) + "\r\n");
            }
            catch (Exception ex)
            {
                //将同步结果写入日志文件
                System.IO.File.AppendAllText(logPath + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：" + Newtonsoft.Json.JsonConvert.SerializeObject(ex) + "\r\n");
            }

        }
        private void SaveUser(UserEntity user)
        {
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            wc.Headers.Add("Content-Type", "application/json");
            DataItemDetailBLL dd = new DataItemDetailBLL();
            string imgUrl = dd.GetItemValue("imgUrl");
            string bzAppUrl = dd.GetItemValue("bzAppUrl");
            wc.Credentials = CredentialCache.DefaultCredentials;
            string logPath = new DataItemDetailBLL().GetItemValue("imgPath") + "\\logs\\syncbz\\";
            //发送请求到web api并获取返回值，默认为post方式
            try
            {
                //用户信息
                user.Gender = user.Gender == "男" ? "1" : "0";
                if (user.EnterTime == null)
                {
                    user.EnterTime = DateTime.Now;
                }
                if (!string.IsNullOrEmpty(user.SignImg))
                {
                    user.SignImg = imgUrl + user.SignImg;
                }
                if (!string.IsNullOrEmpty(user.HeadIcon))
                {
                    user.HeadIcon = imgUrl + user.HeadIcon;
                }
                if (!string.IsNullOrWhiteSpace(user.Password))
                {
                    if (user.Password.Contains("***"))
                    {
                        user.Password = null;
                    }

                }
                List<UserEntity> list=new List<UserEntity>();
                list.Add(user);
                wc.UploadStringCompleted += wc_UploadStringCompleted;
                wc.UploadStringAsync(new Uri(bzAppUrl + "PostEmployees"), "post", list.ToJson());
            }
            catch (Exception ex)
            {
                if (!System.IO.Directory.Exists(HttpContext.Server.MapPath("~/logs/syncbz")))
                {
                    System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath("~/logs/syncbz"));
                }
                //将同步结果写入日志文件
                string fileName = "user_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(logPath + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：同步数据失败，同步信息" + Newtonsoft.Json.JsonConvert.SerializeObject(user) + ",异常信息：" + ex.Message + "\r\n");
            }
        }
        void wc_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            if (!System.IO.Directory.Exists(HttpContext.Server.MapPath("~/logs/syncbz")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath("~/logs/syncbz"));
            }
            string logPath = new DataItemDetailBLL().GetItemValue("imgPath") + "\\logs\\";
            //将同步结果写入日志文件
            string fileName = "user_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            System.IO.File.AppendAllText(logPath + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：" + System.Text.Encoding.UTF8.GetString(e.Result) + "\r\n");
        }
        /// <summary>
        /// 修改用户基本信息
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="userEntity">用户实体</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(7, "修改用户信息")]
        public ActionResult UpdateForm(string keyValue, string strUserEntity)
        {
            UserEntity userEntity = strUserEntity.ToObject<UserEntity>();

            string objectId = userBLL.UpdateUserInfo(keyValue, userEntity);
            return Success("操作成功。");
        }
        /// <summary>
        /// 保存重置修改密码
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="Password">新密码</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(7, "修改用户密码信息")]
        public ActionResult SaveRevisePassword(string keyValue, string Password)
        {
            if (keyValue == "System")
            {
               return Error("当前账户不能重置密码");
            }
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            ERCHTMS.Busines.SystemManage.PasswordSetBLL psBll = new PasswordSetBLL();
            List<string> lst = psBll.IsPasswordRuleStatus(user);
            if (lst[0] == "true")
            {
                //string[] arr = lst[1].Split(';');
                //System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(lst[4]);
                //if (arr[0].Trim().Length > 0)
                //{
                //    reg1 = new System.Text.RegularExpressions.Regex(@"/^.*[A-Z]{1,}.*$/");
                //    if (!reg1.IsMatch(Password))
                //    {
                //        return Error("密码必须包含" + lst[2] + "且长度至少是" + lst[3] + "位!");
                //    }
                //}
                //if (arr[1].Trim().Length > 0)
                //{
                //    reg1 = new System.Text.RegularExpressions.Regex(@"/^.*[a-z]{1,}.*$/");
                //    if (!reg1.IsMatch(Password))
                //    {
                //        return Error("密码必须包含" + lst[2] + "且长度至少是" + lst[3] + "位!");
                //    }
                //}
                //if (arr[2].Trim().Length > 0)
                //{
                //    reg1 = new System.Text.RegularExpressions.Regex(@"/^.*[0-9]{1,}.*$/");
                //    if (!reg1.IsMatch(Password))
                //    {
                //        return Error("密码必须包含" + lst[2] + "且长度至少是" + lst[3] + "位!");
                //    }
                //}
                //if (arr[3].Trim().Length > 0)
                //{
                //    reg1 = new System.Text.RegularExpressions.Regex(@"/^.*[~_!=@#\$%^&\*\?\(\)]{1,}.*$/");
                //    if (!reg1.IsMatch(Password))
                //    {
                //        return Error("密码必须包含" + lst[2] + "且长度至少是" + lst[3] + "位!");
                //    }
                //}
                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(lst[4]);
                if (!reg.IsMatch(Password))
                {
                    return Error("密码必须包含" + lst[2] + "且长度至少是" + lst[3] + "位!");
                }
            }

            string pwd=Md5Helper.MD5(Password, 32).ToLower();
            userBLL.RevisePassword(keyValue, pwd);
            //userBLL.RecordPassword(keyValue, Password);
            DataItemDetailBLL di = new DataItemDetailBLL();
            string way = di.GetItemValue("WhatWay");
            UserEntity userEntity = userBLL.GetEntity(keyValue);
            DepartmentEntity org = departmentBLL.GetEntity(userEntity.OrganizeId);
            if(org.IsTrain==1)
            {
                //对接.net培训平台
                if (way == "0")
                {

                }
                //对接java培训平台
                if (way == "1")
                {
                   
                    string ModuleName = SystemInfo.CurrentModuleName;
                    string ModuleId = SystemInfo.CurrentModuleId;
                    Task.Factory.StartNew(() =>
                    {

                        object obj = new
                        {
                            action = "updatePwd",
                            time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            userId = keyValue,
                            account = userEntity.Account,
                            password = Password,
                            companyId=org.InnerPhone
                        };
                        List<object> list = new List<object>();
                        list.Add(obj);
                        Busines.JPush.JPushApi.PushMessage(list, 1);

                       
                        LogEntity logEntity = new LogEntity();
                        logEntity.CategoryId = 5;
                        logEntity.OperateTypeId = ((int)OperationType.Update).ToString();
                        logEntity.OperateType = "修改密码";
                        logEntity.OperateAccount = user.Account + "（" + user.UserName + "）";
                        logEntity.OperateUserId = user.UserId;

                        logEntity.ExecuteResult =1;
                        logEntity.ExecuteResultJson = string.Format("同步用户(修改密码)到java培训平台,同步信息:\r\n{0}", list.ToJson());
                        logEntity.Module = ModuleName;
                        logEntity.ModuleId = ModuleId;
                        logEntity.WriteLog();
                    });
                }
            }
            if (!string.IsNullOrEmpty(di.GetItemValue("bzAppUrl")))
            {
                UpdatePwd(keyValue, pwd);
            }
            return Success("密码修改成功，请牢记新密码。");
        }
        private void UpdatePwd(string userId, string pwd)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            //发送请求到web api并获取返回值，默认为post方式
            try
            {
                System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                wc.UploadValuesCompleted += wc_UploadValuesCompleted2;
                wc.UploadValuesAsync(new Uri(new DataItemDetailBLL().GetItemValue("bzAppUrl") + "UpdatePwd?userId=" + userId + "&pwd=" + pwd), nc);

            }
            catch (Exception ex)
            {
                //将同步结果写入日志文件
                string fileName = "user_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/syncbz/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：修改密码失败，用户信息" + userId + ",异常信息：" + ex.Message + "\r\n");
            }
        }
        void wc_UploadValuesCompleted2(object sender, UploadValuesCompletedEventArgs e)
        {
            //将同步结果写入日志文件
            string fileName = "user_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/syncbz/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：修改用户密码发生错误>" + System.Text.Encoding.UTF8.GetString(e.Result) + "\r\n");
        }
        /// <summary>
        /// 禁用账户
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        [HandlerAuthorize(PermissionMode.Enforce)]
        [HandlerMonitor(7, "设置当前用户账户不可用")]
        public ActionResult DisabledAccount(string keyValue)
        {
            if (keyValue == "System")
            {
                throw new Exception("当前账户不禁用");
            }
            userBLL.UpdateState(keyValue, 0);

            DataItemDetailBLL di = new DataItemDetailBLL();
            string way = di.GetItemValue("WhatWay");
             UserEntity userEntity = userBLL.GetEntity(keyValue);
             DepartmentEntity org = departmentBLL.GetEntity(userEntity.OrganizeId);
             if (org.IsTrain == 1)
             {
                 //对接.net培训平台
                 if (way == "0")
                 {

                 }
                 //对接java培训平台
                 if (way == "1")
                 {
                     var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                     string ModuleName = SystemInfo.CurrentModuleName;
                     string ModuleId = SystemInfo.CurrentModuleId;
                     Task.Factory.StartNew(() =>
                     {

                         object obj = new
                         {
                             action = "disabled",
                             time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                             userId = keyValue,
                             account = userEntity.Account,
                             companyId=org.InnerPhone
                         };
                         List<object> list = new List<object>();
                         list.Add(obj);
                         Busines.JPush.JPushApi.PushMessage(list, 1);

                         LogEntity logEntity = new LogEntity();
                         logEntity.CategoryId = 5;
                         logEntity.OperateTypeId = ((int)OperationType.Update).ToString();
                         logEntity.OperateType = "禁用账号";
                         logEntity.OperateAccount = user.Account + "（" + user.UserName + "）";
                         logEntity.OperateUserId = user.UserId;

                         logEntity.ExecuteResult = 1;
                         logEntity.ExecuteResultJson = string.Format("同步用户(禁用账号)到java培训平台,同步信息:\r\n{0}", list.ToJson());
                         logEntity.Module = ModuleName;
                         logEntity.ModuleId =ModuleId;
                         logEntity.WriteLog();
                     });
                 }
             }
            return Success("账户禁用成功。");
        }
        /// <summary>
        /// 启用账户
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        [HandlerAuthorize(PermissionMode.Enforce)]
        [HandlerMonitor(7, "设置当前用户账户可用")]
        public ActionResult EnabledAccount(string keyValue)
        {
            userBLL.UpdateState(keyValue, 1);
            DataItemDetailBLL di = new DataItemDetailBLL();
            string way = di.GetItemValue("WhatWay");
              UserEntity userEntity = userBLL.GetEntity(keyValue);
             DepartmentEntity org = departmentBLL.GetEntity(userEntity.OrganizeId);
             if (org.IsTrain == 1)
             {
                 //对接.net培训平台
                 if (way == "0")
                 {

                 }
                 //对接java培训平台
                 if (way == "1")
                 {
                     var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                     string ModuleName = SystemInfo.CurrentModuleName;
                     string ModuleId = SystemInfo.CurrentModuleId;
                     Task.Factory.StartNew(() =>
                     {
                         object obj = new
                         {
                             action = "enable",
                             time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                             userId = keyValue,
                             account = userEntity.Account,
                             companyId=org.InnerPhone
                         };
                         List<object> list = new List<object>();
                         list.Add(obj);
                         Busines.JPush.JPushApi.PushMessage(list, 1);

                        
                         LogEntity logEntity = new LogEntity();
                         logEntity.CategoryId = 5;
                         logEntity.OperateTypeId = ((int)OperationType.Update).ToString();
                         logEntity.OperateType = "启用账号";
                         logEntity.OperateAccount = user.Account + "（" + user.UserName + "）";
                         logEntity.OperateUserId = user.UserId;

                         logEntity.ExecuteResult = 1;
                         logEntity.ExecuteResultJson = string.Format("同步用户(启用账号)到java培训平台,同步信息:\r\n{0}", list.ToJson());
                         logEntity.Module = ModuleName;
                         logEntity.ModuleId = ModuleId;
                         logEntity.WriteLog();
                     });
                 }
             }
            return Success("账户启用成功。");
        }
        /// <summary>
        /// 导入用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public string ImportUser()
        {
            if (OperatorProvider.Provider.Current().IsSystem)
            {
                return "超级管理员无此操作权限";
            }
            RoleBLL roleBll = new RoleBLL();
            var currUser = OperatorProvider.Provider.Current();
            string orgId = OperatorProvider.Provider.Current().OrganizeId;//所属公司

            int error = 0;
            string message = "请选择格式正确的文件再导入!";
            string falseMessage = "";
            int count = HttpContext.Request.Files.Count;
            if (count > 0)
            {
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
                wb.Open(Server.MapPath("~/Resource/temp/" + fileName));
                Aspose.Cells.Cells cells = wb.Worksheets[0].Cells;
                DataTable dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn + 1, true);
                int order = 1;
                IList<UserEntity> userList = new List<UserEntity>();
                List<object> lstObjs = new List<object>();
                DataItemDetailBLL di = new DataItemDetailBLL();
                string way = di.GetItemValue("WhatWay");
                string ModuleName = SystemInfo.CurrentModuleName;
                string ModuleId = SystemInfo.CurrentModuleId;
                DepartmentEntity org = departmentBLL.GetEntity(orgId);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    order = i;
                    //用于数据验证部门
                    string deptlist = dt.Rows[i]["部门"].ToString().Trim();
                    //部门
                    string deptName = dt.Rows[i]["部门"].ToString().Trim();
                    string deptId = string.Empty;//所属部门
                    //姓名
                    string fullName = dt.Rows[i]["姓名"].ToString().Trim();
                    //性别
                    string sex = dt.Rows[i]["性别"].ToString().Trim();
                    //账号
                    string account = dt.Rows[i]["账号"].ToString().Trim();
                    //工号
                    string no = dt.Rows[i]["工号"].ToString().Trim();
                    //密码
                    string password = dt.Rows[i]["密码"].ToString().Trim();
                    password = string.IsNullOrEmpty(password) ? "Abc123456" : password;
                    //身份证号
                    string identity = dt.Rows[i]["身份证号"].ToString().Trim();


                    //岗位
                    string dutyName = dt.Rows[i]["岗位"].ToString().Trim();
                    string dutyid = "";
                    //手机号
                    string mobile = dt.Rows[i]["手机号码"].ToString().Trim();
                    //邮箱
                    string email = dt.Rows[i]["邮箱"].ToString().Trim();
                    //是否在场/在职
                    string isIn = dt.Rows[i]["是否在厂(职)"].ToString().Trim();


                    //---****值存在空验证*****--
                    if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(account) || string.IsNullOrEmpty(identity) || string.IsNullOrEmpty(dutyName) || string.IsNullOrEmpty(deptName) || string.IsNullOrEmpty(sex) || string.IsNullOrEmpty(isIn))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行值存在空,未能导入.";
                        error++;
                        continue;
                    }
                    var p1 = string.Empty;
                    var p2 = string.Empty;
                    bool isSkip = false;
                    //验证所填部门是否存在
                    var array = deptlist.Split('/');
                    for (int j = 0; j < array.Length; j++)
                    {
                        if (j == 0)
                        {
                            var entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "厂级" && x.FullName == array[j].ToString()).FirstOrDefault();
                            if (entity == null)
                            {
                                entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "部门" && x.FullName == array[j].ToString()).FirstOrDefault();
                                if (entity == null)
                                {
                                    entity = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "承包商" && x.FullName == array[j].ToString()).FirstOrDefault();
                                    if (entity == null)
                                    {
                                        falseMessage += "</br>" + "第" + (i + 2) + "行部门信息不存在,未能导入.";
                                        error++;
                                        isSkip = true;
                                        break;
                                    }
                                    else
                                    {
                                        deptId = entity.DepartmentId;
                                        p1 = entity.DepartmentId;
                                    }
                                }
                                else
                                {
                                    deptId = entity.DepartmentId;
                                    p1 = entity.DepartmentId;
                                }
                            }
                            else
                            {
                                deptId = entity.DepartmentId;
                                p1 = entity.DepartmentId;
                            }
                        }
                        else if (j == 1)
                        {
                            var entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && (x.Nature == "专业" || x.Nature == "承包商" || x.Nature == "分包商") && x.FullName == array[j].ToString() && x.ParentId == p1).FirstOrDefault();
                            if (entity1 == null)
                            {
                                entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "班组" && x.FullName == array[j].ToString() && x.ParentId == p1).FirstOrDefault();
                                if (entity1 == null)
                                {
                                    falseMessage += "</br>" + "第" + (i + 2) + "行部门信息不存在,未能导入.";
                                    error++;
                                    isSkip = true;
                                    break;
                                }
                                else
                                {
                                    deptId = entity1.DepartmentId;
                                    p2 = entity1.DepartmentId;
                                }
                            }
                            else
                            {
                                deptId = entity1.DepartmentId;
                                p2 = entity1.DepartmentId;
                            }

                        }
                        else
                        {
                            var entity1 = departmentBLL.GetList().Where(x => x.OrganizeId == currUser.OrganizeId && (x.Nature == "班组" || x.Nature == "承包商" || x.Nature == "分包商") && x.FullName == array[j].ToString() && x.ParentId == p2).FirstOrDefault();
                            if (entity1 == null)
                            {
                                falseMessage += "</br>" + "第" + (i + 2) + "行部门信息不存在,未能导入.";
                                error++;
                                isSkip = true;
                                break;
                            }
                            else
                            {
                                deptId = entity1.DepartmentId;
                            }
                        }
                    }
                    if( isSkip)
                    {
                        continue;
                    }
                    //--手机号验证
                    if (!string.IsNullOrEmpty(mobile))
                    {
                        if (!Regex.IsMatch(mobile, @"^(\+\d{2,3}\-)?\d{11}$", RegexOptions.IgnoreCase))
                        {
                            falseMessage += "</br>" + "第" + (i + 2) + "行手机号格式有误,未能导入.";
                            error++;
                            continue;
                        }
                    }
                    //--邮箱验证
                    if (!string.IsNullOrEmpty(email))
                    {
                        if (!Regex.IsMatch(email, @"^\w{3,}@\w+(\.\w+)+$", RegexOptions.IgnoreCase))
                        {
                            falseMessage += "</br>" + "第" + (i + 2) + "行邮箱格式有误,未能导入.";
                            error++;
                            continue;
                        }
                    }
                    //验证账号的唯一性
                    if (!userBLL.ExistAccount(account))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行账号已存在,未能导入.";
                        error++;
                        continue;
                    }
                    //检验所填岗位是否属于其公司或者部门
                    if (string.IsNullOrEmpty(deptId) || deptId == "undefined")
                    {
                        //所属公司
                        RoleEntity data = postCache.GetList(orgId, "true").OrderBy(x => x.SortCode).Where(a => a.FullName == dutyName).FirstOrDefault();
                        if (data == null)
                        {
                            falseMessage += "</br>" + "第" + (i + 2) + "行岗位不属于该公司,未能导入.";
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        //所属部门
                        //所属公司
                        RoleEntity data = postCache.GetList(orgId, deptId).OrderBy(x => x.SortCode).Where(a => a.FullName == dutyName).FirstOrDefault();
                        if (data == null)
                        {
                            falseMessage += "</br>" + "第" + (i + 2) + "行岗位不属于该部门,未能导入.";
                            error++;
                            continue;
                        }
                    }
                    //--**验证岗位是否存在**--


                    RoleEntity re = postBLL.GetList().Where(a => (a.FullName == dutyName && a.OrganizeId == orgId)).FirstOrDefault();
                    if (!(string.IsNullOrEmpty(deptId) || deptId == "undefined"))
                    {
                        re = postBLL.GetList().Where(a => (a.FullName == dutyName && a.OrganizeId == orgId && a.DeptId == deptId)).FirstOrDefault();
                        if (re == null)
                        {
                            re = postBLL.GetList().Where(a =>(a.FullName == dutyName && a.OrganizeId == orgId && a.Nature == departmentBLL.GetEntity(deptId).Nature)).FirstOrDefault();
                        }
                    }
                    if (re == null)
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行岗位有误,未能导入.";
                        error++;
                        continue;
                    }
                    else
                    {
                        dutyid = re.RoleId;
                    }
                    //角色
                    //--**根据选择的岗位来默认角色信息

                    string roleName = "";
                    string roleId = "";
                    //如果选择的是厂级部门的话，角色会默认追加“厂级部门用户”
                    if (!(string.IsNullOrEmpty(deptId) || deptId == "undefined"))
                    {
                        if (departmentBLL.GetEntity(deptId).IsOrg == 1)
                        {
                            roleName += "厂级部门用户,";
                            RoleEntity cj = roleBll.GetList().Where(a => a.FullName == "厂级部门用户").FirstOrDefault();
                            if (cj != null)
                                roleId += cj.RoleId + ",";
                        }
                    }
                    IEnumerable<RoleEntity> ro = postBLL.GetList().Where(a => a.RoleId == dutyid);
                    ////根据选择的岗位来追加角色（这里还要加上所选部门的层级）
                    //if (!(string.IsNullOrEmpty(DepartmentId) || DepartmentId == "undefined"))
                    //{
                    //    ro = ro.Where(a => a.Nature == departmentBLL.GetEntity(DepartmentId).Nature);
                    //}
                    RoleEntity roleentity = ro.FirstOrDefault();
                    if (roleentity != null)
                    {
                        roleName += roleentity.RoleNames;
                        roleId += roleentity.RoleIds;
                    }
                    //---****身份证正确验证*****--
                    if (!Regex.IsMatch(identity, @"^(^d{15}$|^\d{18}$|^\d{17}(\d|X|x))$", RegexOptions.IgnoreCase))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行身份证号格式有误,未能导入.";
                        error++;
                        continue;
                    }
                    string userId = "";
                    //---****身份证重复验证*****--
                    if (!userBLL.ExistIdentifyID(identity, ""))
                    {
                        string res = userBLL.IsBalckUser("", identity);
                        if (!string.IsNullOrWhiteSpace(res))
                        {
                            falseMessage += "</br>" + "第" + (i + 2) + "行,该用户于时间" + res + "被加入黑名单！";
                            error++;
                            continue;
                        }
                        else
                        {
                            falseMessage += "</br>" + "第" + (i + 2) + "行身份证号已存在,未能导入.";
                            error++;
                            continue;
                        }

                    }
                    DateTime brith = DateTime.Now;
                    string brithday;
                    int age = 0;
                    try
                    {
                        string errorStr = "";
                        if (!CheckIdCard(identity, out errorStr, out brithday))
                        {
                            falseMessage += "</br>" + "第" + (i + 2) + "行" + errorStr + ",未能导入.";
                            error++;
                            continue;
                        }

                        brith = Convert.ToDateTime(brithday);
                        //string strbirth = identity.Substring(6, 4) + "-" + identity.Substring(10, 2) + "-" +
                        //                  identity.Substring(12, 2);
                        //brith = Convert.ToDateTime(strbirth);
                        age = DateTime.Now.Year - brith.Year;
                    }
                    catch (Exception e)
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行身份证号出生年月日信息有误,未能导入.";
                        error++;
                        continue;
                    }



                    UserEntity ue = new UserEntity();
                    ue.UserId = Guid.NewGuid().ToString();
                    ue.Account = account.Trim();
                    ue.Password = password;
                    ue.RealName = fullName;
                    ue.Birthday = brith;
                    ue.EnCode = no;//工号
                    ue.Age = age.ToString();
                    ue.IsPresence = isIn == "是" ? "1" : "0";
                    ue.Gender = sex;
                    ue.Mobile = mobile;
                    ue.OrganizeId = orgId;
                    if (!(string.IsNullOrEmpty(deptId) || deptId == "undefined"))
                    {
                        ue.DepartmentId = deptId;
                        ue.DepartmentCode = departmentBLL.GetEntity(deptId).EnCode;
                    }
                    else
                    {
                        ue.DepartmentCode = organizeCache.GetEntity(orgId).EnCode;
                    }

                    ue.IsEpiboly = "0";
                    ue.RoleId = roleId;
                    ue.RoleName = roleName;
                    ue.DutyId = dutyid;
                    ue.DutyName = dutyName;
                    ue.DeleteMark = 0;
                    ue.EnabledMark = 1;
                    ue.IdentifyID = identity;
                    ue.Email = email;
                    ue.OrganizeCode = OperatorProvider.Provider.Current().OrganizeCode;

                    try
                    {
                        UserInfoEntity user = userBLL.GetUserInfoByAccount(ue.Account);
                        string action = user == null ? "add" : "edit";

                        ue.UserId = userBLL.SaveForm(userId, ue);
                        ue.Password = password;
                        if (!string.IsNullOrWhiteSpace(ue.UserId))
                        {
                            userList.Add(ue);

                          
                            if (org.IsTrain == 1)
                            {
                            //对接.net培训平台
                            if (way == "0")
                            {

                            }
                            //对接java培训平台
                            if (way == "1")
                            {
                                    DepartmentEntity dept = departmentBLL.GetEntity(ue.DepartmentId);
                                    ue = userBLL.GetEntity(ue.UserId);
                                    if (dept != null && ue!=null)
                                    {
                                       
                                        deptId = ue.DepartmentId;
                                        string enCode = ue.DepartmentCode;
                                        if (!string.IsNullOrWhiteSpace(dept.DeptKey))
                                        {
                                            string[] arr = dept.DeptKey.Split('|');
                                            deptId = arr[0];
                                            if (arr.Length > 1)
                                            {
                                                enCode = arr[1];
                                                enCode = arr[1];
                                            }
                                        }
                                        object obj = new
                                        {
                                            action = action,
                                            time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                            userId = ue.UserId,
                                            userName = ue.RealName,
                                            password = password,
                                            account = ue.Account,
                                            deptId = deptId,
                                            deptCode = enCode,
                                            sex = ue.Gender,
                                            idCard = ue.IdentifyID,
                                            email = ue.Email,
                                            mobile = ue.Mobile,
                                            birth = ue.Birthday,//生日
                                            postId=ue.DutyId,
                                            postName = ue.DutyName,//岗位
                                            age = ue.Age.ToIntOrNull(),//年龄
                                            native = ue.Native, //籍贯
                                            nation = ue.Nation, //民族
                                            encode = ue.EnCode,//工号
                                            jobTitle = ue.JobTitle,
                                            techLevel = ue.TechnicalGrade,
                                            workType=ue.Craft,
                                            companyId=org.InnerPhone,
                                            trainRoles=ue.TrainRoleId,
                                            //signContent = userBLL.GetSignContent(ue.SignImg),//人员签名照
                                            role = 0//角色（0:学员，1:培训管理员）
                                        };
                                        lstObjs.Add(obj);
                                    }
                                }
                            }
                        }
                        else
                        {
                            falseMessage += "</br>" + "第" + (i + 2) + "行账号或手机号或工号在系统中已存在,未能导入.";
                            error++;
                            continue;
                        }
                    }
                    catch
                    {
                        error++;
                        continue;
                    }
                }
                if (!string.IsNullOrEmpty(di.GetItemValue("bzAppUrl")))
                {
                    userBLL.SyncUsersToBZ(userList);
                }
                if (way == "1" && lstObjs.Count>0)
                {
                    if (lstObjs.Count > 50)
                    {
                        int page = 0;
                        int total = lstObjs.Count;
                        if (total % 50 == 0)
                        {
                            page = total / 50;
                        }
                        else
                        {
                            page = total / 50 + 1;
                        }
                        for (int j = 0; j < page; j++)
                        {
                            Busines.JPush.JPushApi.PushMessage(lstObjs.Skip(j * 50).Take(50), 1);

                            LogEntity logEntity = new LogEntity();
                            logEntity.CategoryId = 5;
                            logEntity.OperateTypeId = ((int)OperationType.Create).ToString();
                            logEntity.OperateType = "导入用户";
                            logEntity.OperateAccount = currUser.Account + "（" + currUser.UserName + "）";
                            logEntity.OperateUserId = currUser.UserId;

                            logEntity.ExecuteResult =1;
                            logEntity.ExecuteResultJson = string.Format("同步用户(导入)到java培训平台,同步信息:\r\n{0}", lstObjs.ToJson());
                            logEntity.Module = ModuleName;
                            logEntity.ModuleId = ModuleId;
                            logEntity.WriteLog();
                        }
                    }
                    else
                    {
                        Busines.JPush.JPushApi.PushMessage(lstObjs, 1);
                      
                        LogEntity logEntity = new LogEntity();
                        logEntity.CategoryId = 5;
                        logEntity.OperateTypeId = ((int)OperationType.Create).ToString();
                        logEntity.OperateType ="导入用户";
                        logEntity.OperateAccount = currUser.Account + "（" + currUser.UserName + "）";
                        logEntity.OperateUserId = currUser.UserId;

                        logEntity.ExecuteResult =1;
                        logEntity.ExecuteResultJson = string.Format("同步用户(导入)到java培训平台,同步信息:\r\n{0}", lstObjs.ToJson());
                        logEntity.Module = ModuleName;
                        logEntity.ModuleId =ModuleId;
                        logEntity.WriteLog();
                    }
                      
                }
                ImportUserHik(userList);
                //毕节新增
                UpdateHdgzUser(userList.ToList());
                count = dt.Rows.Count;
                message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条";
                message += "</br>" + falseMessage;
            }

            return message;
        }

        /// <summary>
        /// 验证身份证是否合法
        /// </summary>
        /// <param name="IdCard"></param>
        /// <param name="error"></param>
        /// <param name="sbirthday"></param>
        /// <returns></returns>
        public bool CheckIdCard(string IdCard, out string error, out string sbirthday)
        {
            //var aCity ={ 11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙江", 31: "上海", 32: "江苏", 33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南", 42: "湖北", 43: "湖南", 44: "广东", 45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南", 54: "西藏", 61: "陕西", 62: "甘肃", 63: "青海", 64: "宁夏", 65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外" };
            List<int> aCity = new List<int>() { 11, 12, 13, 14, 15, 21, 22, 23, 31, 32, 33, 34, 35, 36, 37, 41, 42, 43, 44, 45, 46, 50, 51, 52, 53, 54, 61, 62, 63, 64, 65, 71, 81, 82, 91 };
            error = "";
            sbirthday = "";
            var iSum = 0.0;
            if (!Regex.IsMatch(IdCard, @"^(^d{15}$|^\d{18}$|^\d{17}(\d|X|x))$", RegexOptions.IgnoreCase))
            {
                error = "你输入的身份证长度或格式错误";
                return false;
            }
            //IdCard = IdCard.replace(/x$/i, "a");
            if (!aCity.Contains(Convert.ToInt32(IdCard.Substring(0, 2))))
            {
                error = "你的身份证地区非法";
                return false;
            }
            var sBirthday = IdCard.Substring(6, 4) + "-" + IdCard.Substring(10, 2) + "-" + IdCard.Substring(12, 2);
            try
            {

                if (!string.IsNullOrEmpty(sBirthday))
                {
                    DateTime r = new DateTime();
                    if (DateTime.TryParse(sBirthday, out r))
                    {
                        sbirthday = r.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        error = "身份证上的出生日期非法";
                        return false;
                    }

                }
            }
            catch
            {
                error = "身份证号有误";
                return false;
            }
            //for (var i = 17; i >= 0; i--) {
            //    iSum += (Math.Pow(2, i) % 11) * Convert.ToInt32(IdCard.ToArray()[17 - i].ToInt().ToString(), 11);
            //}
            //if (iSum % 11 != 1) {
            //    error = "你输入的身份证号非法";
            //    return false;
            //}

            return true;
        }


        private void ImportUser(IList<UserEntity> userList)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            wc.Encoding = Encoding.UTF8;
            wc.Headers.Add("Content-Type", "application/json");
            //发送请求到web api并获取返回值，默认为post方式
            try
            {
                foreach (UserEntity item in userList)
                {
                    //用户信息
                    item.Gender = item.Gender == "男" ? "1" : "0";
                    if (item.RoleName.Contains("班组级用户"))
                    {
                        if (item.RoleName.Contains("负责人"))
                        {
                            item.RoleId = "a1b68f78-ec97-47e0-b433-2ec4a5368f72";
                            item.RoleName = "班组长";
                        }
                        else
                        {
                            item.RoleId = "e503d929-daa6-472d-bb03-42533a11f9c6";
                            item.RoleName = "班组成员";
                        }
                    }
                    if (item.RoleName.Contains("部门级用户"))
                    {
                        if (item.RoleName.Contains("负责人"))
                        {
                            item.RoleId = "1266af38-9c0a-4eca-a04a-9829bc2ee92d";
                            item.RoleName = "部门管理员";
                        }
                        else
                        {
                            item.RoleId = "3a4b56ac-6207-429d-ac07-28ab49dca4a6";
                            item.RoleName = "部门级用户";
                        }
                    }
                    if (item.RoleName.Contains("公司级用户"))
                    {
                        //if (user.RoleName.Contains("负责人"))
                        //{
                        item.RoleId = "97869267-e5eb-4f20-89bd-61e7202c4ecd";
                        item.RoleName = "厂级管理员";
                        // }

                    }
                    if (item.EnterTime == null)
                    {
                        item.EnterTime = DateTime.Now;
                    }
                }
                wc.UploadStringCompleted +=wc_UploadStringCompleted;
                wc.UploadStringAsync(new Uri(new DataItemDetailBLL().GetItemValue("bzAppUrl") + "PostEmployees"), "post", userList.ToJson());

            }
            catch (Exception ex)
            {
                //将同步结果写入日志文件
                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/syncbz/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：同步用户失败，同步信息" + Newtonsoft.Json.JsonConvert.SerializeObject(userList) + ",异常信息：" + ex.Message + "\r\n");
            }
        }
        void wc_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            var error = e.Error;
            //将同步结果写入日志文件
            string fileName = "user_"+DateTime.Now.ToString("yyyyMMdd") + ".log";
            if (!System.IO.Directory.Exists(HttpContext.Server.MapPath("~/logs/syncbz")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath("~/logs/syncbz"));
            }
            try
            {

                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/syncbz/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：同步用户结果>" + e.Result + "\r\n");
            }
            catch (Exception ex)
            {
                string msg = error == null ? ex.Message : error.Message;
                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/syncbz/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：同步用户结果>" + msg + "\r\n");
            }

        }
        #endregion

        #region 数据导出
        /// <summary>
        /// 导出用户列表
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出用户数据")]
        public ActionResult ExportUserList(string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            if (!queryParam["indexldap"].IsEmpty() && queryParam["indexldap"].ToString() == "true")
            {
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 100000000;
                pagination.p_kid = "USERID";
                pagination.p_fields = "account,account usercode,  'CR003' zzbh ,'100065' deptcode ,substr(TRIM(realname),0,1) zwx,substr(TRIM(realname),2) zwm , '' pyx,''pym ,realname ,email,mobile,deptname,'duxingzhan' managers,to_char(createdate,'yyyy-MM-dd') createdate,to_char(add_months(createdate,12),'yyyy-mm-dd') as sxdate ,substr(identifyid,-4) identifyid,'' password";
                pagination.p_tablename = "V_USERINFO t";
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "departmentcode", "organizecode");
                pagination.conditionJson = string.IsNullOrEmpty(where) ? "Account!='System'" : where;
                var data = userBLL.GetPageList(pagination, queryJson);
                DataTable dtResult = new DataTable();
                dtResult = data.Clone();
                foreach (DataRow row in data.Rows)
                {
                    DataRow rowNew = dtResult.NewRow();
                    rowNew["account"] = row["account"];
                    rowNew["usercode"] = row["usercode"];
                    rowNew["zzbh"] = row["zzbh"];
                    rowNew["deptcode"] = row["deptcode"];
                    rowNew["zwx"] = row["zwx"];
                    rowNew["zwm"] = row["zwm"];
                    rowNew["pyx"] = ConvertToPinYin(row["zwx"].ToString());
                    rowNew["pym"] = ConvertToPinYin(row["zwm"].ToString());
                    rowNew["realname"] = row["realname"];
                    rowNew["email"] = row["email"];
                    rowNew["mobile"] = row["mobile"];
                    rowNew["deptname"] = row["deptname"];
                    rowNew["managers"] = row["managers"];
                    rowNew["createdate"] = row["createdate"];
                    rowNew["sxdate"] = row["sxdate"];
                    rowNew["identifyid"] = row["identifyid"];
                    rowNew["password"] = row["password"];
                    dtResult.Rows.Add(rowNew);
                }
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "用户信息";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 25;
                excelconfig.FileName = "用户导出.xls";
                excelconfig.IsAllSizeColumn = true;
                //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "account", ExcelColumn = "登入名" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "usercode", ExcelColumn = "员工编号" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "zzbh", ExcelColumn = "组织编号" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "deptcode", ExcelColumn = "部门编号" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "zwx", ExcelColumn = "中文姓" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "zwm", ExcelColumn = "中文名" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "pyx", ExcelColumn = "拼音姓" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "pym", ExcelColumn = "拼音名" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "realname", ExcelColumn = "显示名称" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "email", ExcelColumn = "电子邮箱" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "mobile", ExcelColumn = "移动电话" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "deptname", ExcelColumn = "顾问公司" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "managers", ExcelColumn = "管理者" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createdate", ExcelColumn = "账号生效日期" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "sxdate", ExcelColumn = "账号失效日期" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "identifyid", ExcelColumn = "身份证后4位" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "password", ExcelColumn = "密码" });
                //调用导出方法
                ExcelHelper.ExcelDownload(dtResult, excelconfig);
                return Success("导出成功。");
            }
            else
            {
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 100000000;
                pagination.p_kid = "USERID";
                pagination.p_fields = "ACCOUNT,REALNAME,GENDER,MOBILE,EMAIL,DUTYNAME,MANAGER,OrganizeName,DEPTNAME";
                pagination.p_tablename = "V_USERINFO t";
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "departmentcode", "organizecode");
                pagination.conditionJson = string.IsNullOrEmpty(where) ? "Account!='System'" : where;
                var data = userBLL.GetPageList(pagination, queryJson);

                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "用户信息";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 25;
                excelconfig.FileName = "用户导出.xls";
                excelconfig.IsAllSizeColumn = true;
                //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "account", ExcelColumn = "账户" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "realname", ExcelColumn = "姓名" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "gender", ExcelColumn = "性别" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "mobile", ExcelColumn = "手机号码" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "email", ExcelColumn = "邮箱" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "dutyname", ExcelColumn = "岗位" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "manager", ExcelColumn = "主管" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "organizename", ExcelColumn = "公司" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "deptname", ExcelColumn = "部门" });
                //调用导出方法
                ExcelHelper.ExcelDownload(data, excelconfig);
                return Success("导出成功。");
            }


        }
        #endregion

        #region 汉字转换成全拼的拼音

        #region 定义拼音区编码数组
        //定义拼音区编码数组
        private static int[] getValue = new int[]
                    {
                        -20319, -20317, -20304, -20295, -20292, -20283, -20265, -20257, -20242, -20230, -20051, -20036,
                        -20032, -20026, -20002, -19990, -19986, -19982, -19976, -19805, -19784, -19775, -19774, -19763,
                        -19756, -19751, -19746, -19741, -19739, -19728, -19725, -19715, -19540, -19531, -19525, -19515,
                        -19500, -19484, -19479, -19467, -19289, -19288, -19281, -19275, -19270, -19263, -19261, -19249,
                        -19243, -19242, -19238, -19235, -19227, -19224, -19218, -19212, -19038, -19023, -19018, -19006,
                        -19003, -18996, -18977, -18961, -18952, -18783, -18774, -18773, -18763, -18756, -18741, -18735,
                        -18731, -18722, -18710, -18697, -18696, -18526, -18518, -18501, -18490, -18478, -18463, -18448,
                        -18447, -18446, -18239, -18237, -18231, -18220, -18211, -18201, -18184, -18183, -18181, -18012,
                        -17997, -17988, -17970, -17964, -17961, -17950, -17947, -17931, -17928, -17922, -17759, -17752,
                        -17733, -17730, -17721, -17703, -17701, -17697, -17692, -17683, -17676, -17496, -17487, -17482,
                        -17468, -17454, -17433, -17427, -17417, -17202, -17185, -16983, -16970, -16942, -16915, -16733,
                        -16708, -16706, -16689, -16664, -16657, -16647, -16474, -16470, -16465, -16459, -16452, -16448,
                        -16433, -16429, -16427, -16423, -16419, -16412, -16407, -16403, -16401, -16393, -16220, -16216,
                        -16212, -16205, -16202, -16187, -16180, -16171, -16169, -16158, -16155, -15959, -15958, -15944,
                        -15933, -15920, -15915, -15903, -15889, -15878, -15707, -15701, -15681, -15667, -15661, -15659,
                        -15652, -15640, -15631, -15625, -15454, -15448, -15436, -15435, -15419, -15416, -15408, -15394,
                        -15385, -15377, -15375, -15369, -15363, -15362, -15183, -15180, -15165, -15158, -15153, -15150,
                        -15149, -15144, -15143, -15141, -15140, -15139, -15128, -15121, -15119, -15117, -15110, -15109,
                        -14941, -14937, -14933, -14930, -14929, -14928, -14926, -14922, -14921, -14914, -14908, -14902,
                        -14894, -14889, -14882, -14873, -14871, -14857, -14678, -14674, -14670, -14668, -14663, -14654,
                        -14645, -14630, -14594, -14429, -14407, -14399, -14384, -14379, -14368, -14355, -14353, -14345,
                        -14170, -14159, -14151, -14149, -14145, -14140, -14137, -14135, -14125, -14123, -14122, -14112,
                        -14109, -14099, -14097, -14094, -14092, -14090, -14087, -14083, -13917, -13914, -13910, -13907,
                        -13906, -13905, -13896, -13894, -13878, -13870, -13859, -13847, -13831, -13658, -13611, -13601,
                        -13406, -13404, -13400, -13398, -13395, -13391, -13387, -13383, -13367, -13359, -13356, -13343,
                        -13340, -13329, -13326, -13318, -13147, -13138, -13120, -13107, -13096, -13095, -13091, -13076,
                        -13068, -13063, -13060, -12888, -12875, -12871, -12860, -12858, -12852, -12849, -12838, -12831,
                        -12829, -12812, -12802, -12607, -12597, -12594, -12585, -12556, -12359, -12346, -12320, -12300,
                        -12120, -12099, -12089, -12074, -12067, -12058, -12039, -11867, -11861, -11847, -11831, -11798,
                        -11781, -11604, -11589, -11536, -11358, -11340, -11339, -11324, -11303, -11097, -11077, -11067,
                        -11055, -11052, -11045, -11041, -11038, -11024, -11020, -11019, -11018, -11014, -10838, -10832,
                        -10815, -10800, -10790, -10780, -10764, -10587, -10544, -10533, -10519, -10331, -10329, -10328,
                        -10322, -10315, -10309, -10307, -10296, -10281, -10274, -10270, -10262, -10260, -10256, -10254
                    };

        //定义拼音数组
        private static string[] getName = new string[]
                    {
                        "A", "Ai", "An", "Ang", "Ao", "Ba", "Bai", "Ban", "Bang", "Bao", "Bei", "Ben",
                        "Beng", "Bi", "Bian", "Biao", "Bie", "Bin", "Bing", "Bo", "Bu", "Ba", "Cai", "Can",
                        "Cang", "Cao", "Ce", "Ceng", "Cha", "Chai", "Chan", "Chang", "Chao", "Che", "Chen", "Cheng",
                        "Chi", "Chong", "Chou", "Chu", "Chuai", "Chuan", "Chuang", "Chui", "Chun", "Chuo", "Ci", "Cong",
                        "Cou", "Cu", "Cuan", "Cui", "Cun", "Cuo", "Da", "Dai", "Dan", "Dang", "Dao", "De",
                        "Deng", "Di", "Dian", "Diao", "Die", "Ding", "Diu", "Dong", "Dou", "Du", "Duan", "Dui",
                        "Dun", "Duo", "E", "En", "Er", "Fa", "Fan", "Fang", "Fei", "Fen", "Feng", "Fo",
                        "Fou", "Fu", "Ga", "Gai", "Gan", "Gang", "Gao", "Ge", "Gei", "Gen", "Geng", "Gong",
                        "Gou", "Gu", "Gua", "Guai", "Guan", "Guang", "Gui", "Gun", "Guo", "Ha", "Hai", "Han",
                        "Hang", "Hao", "He", "Hei", "Hen", "Heng", "Hong", "Hou", "Hu", "Hua", "Huai", "Huan",
                        "Huang", "Hui", "Hun", "Huo", "Ji", "Jia", "Jian", "Jiang", "Jiao", "Jie", "Jin", "Jing",
                        "Jiong", "Jiu", "Ju", "Juan", "Jue", "Jun", "Ka", "Kai", "Kan", "Kang", "Kao", "Ke",
                        "Ken", "Keng", "Kong", "Kou", "Ku", "Kua", "Kuai", "Kuan", "Kuang", "Kui", "Kun", "Kuo",
                        "La", "Lai", "Lan", "Lang", "Lao", "Le", "Lei", "Leng", "Li", "Lia", "Lian", "Liang",
                        "Liao", "Lie", "Lin", "Ling", "Liu", "Long", "Lou", "Lu", "Lv", "Luan", "Lue", "Lun",
                        "Luo", "Ma", "Mai", "Man", "Mang", "Mao", "Me", "Mei", "Men", "Meng", "Mi", "Mian",
                        "Miao", "Mie", "Min", "Ming", "Miu", "Mo", "Mou", "Mu", "Na", "Nai", "Nan", "Nang",
                        "Nao", "Ne", "Nei", "Nen", "Neng", "Ni", "Nian", "Niang", "Niao", "Nie", "Nin", "Ning",
                        "Niu", "Nong", "Nu", "Nv", "Nuan", "Nue", "Nuo", "O", "Ou", "Pa", "Pai", "Pan",
                        "Pang", "Pao", "Pei", "Pen", "Peng", "Pi", "Pian", "Piao", "Pie", "Pin", "Ping", "Po",
                        "Pu", "Qi", "Qia", "Qian", "Qiang", "Qiao", "Qie", "Qin", "Qing", "Qiong", "Qiu", "Qu",
                        "Quan", "Que", "Qun", "Ran", "Rang", "Rao", "Re", "Ren", "Reng", "Ri", "Rong", "Rou",
                        "Ru", "Ruan", "Rui", "Run", "Ruo", "Sa", "Sai", "San", "Sang", "Sao", "Se", "Sen",
                        "Seng", "Sha", "Shai", "Shan", "Shang", "Shao", "She", "Shen", "Sheng", "Shi", "Shou", "Shu",
                        "Shua", "Shuai", "Shuan", "Shuang", "Shui", "Shun", "Shuo", "Si", "Song", "Sou", "Su", "Suan",
                        "Sui", "Sun", "Suo", "Ta", "Tai", "Tan", "Tang", "Tao", "Te", "Teng", "Ti", "Tian",
                        "Tiao", "Tie", "Ting", "Tong", "Tou", "Tu", "Tuan", "Tui", "Tun", "Tuo", "Wa", "Wai",
                        "Wan", "Wang", "Wei", "Wen", "Weng", "Wo", "Wu", "Xi", "Xia", "Xian", "Xiang", "Xiao",
                        "Xie", "Xin", "Xing", "Xiong", "Xiu", "Xu", "Xuan", "Xue", "Xun", "Ya", "Yan", "Yang",
                        "Yao", "Ye", "Yi", "Yin", "Ying", "Yo", "Yong", "You", "Yu", "Yuan", "Yue", "Yun",
                        "Za", "Zai", "Zan", "Zang", "Zao", "Ze", "Zei", "Zen", "Zeng", "Zha", "Zhai", "Zhan",
                        "Zhang", "Zhao", "Zhe", "Zhen", "Zheng", "Zhi", "Zhong", "Zhou", "Zhu", "Zhua", "Zhuai", "Zhuan",
                        "Zhuang", "Zhui", "Zhun", "Zhuo", "Zi", "Zong", "Zou", "Zu", "Zuan", "Zui", "Zun", "Zuo"
                    };
        #endregion

        /// <summary>
        /// 汉字转换成全拼的拼音
        /// </summary>
        /// <param name="chstr">汉字字符串</param>
        /// <returns>转换后的拼音字符串</returns>
        public static string ConvertToPinYin(string chstr)
        {
            Regex reg = new Regex("^[\u4e00-\u9fa5]$");//验证是否输入汉字
            byte[] arr = new byte[2];
            string pystr = "";
            int asc = 0, M1 = 0, M2 = 0;
            char[] mChar = string.IsNullOrEmpty(chstr) ? new char[0] : chstr.ToCharArray();//获取汉字对应的字符数组
            for (int j = 0; j < mChar.Length; j++)
            {
                //如果输入的是汉字
                if (reg.IsMatch(mChar[j].ToString()))
                {
                    arr = System.Text.Encoding.Default.GetBytes(mChar[j].ToString());
                    M1 = (short)(arr[0]);
                    M2 = (short)(arr[1]);
                    asc = M1 * 256 + M2 - 65536;
                    if (asc > 0 && asc < 160)
                    {
                        pystr += mChar[j];
                    }
                    else
                    {
                        switch (asc)
                        {
                            case -9254:
                                pystr += "Zhen";
                                break;
                            case -8985:
                                pystr += "Qian";
                                break;
                            case -5463:
                                pystr += "Jia";
                                break;
                            case -8274:
                                pystr += "Ge";
                                break;
                            case -5448:
                                pystr += "Ga";
                                break;
                            case -5447:
                                pystr += "La";
                                break;
                            case -4649:
                                pystr += "Chen";
                                break;
                            case -5436:
                                pystr += "Mao";
                                break;
                            case -5213:
                                pystr += "Mao";
                                break;
                            case -3597:
                                pystr += "Die";
                                break;
                            case -5659:
                                pystr += "Tian";
                                break;
                            default:
                                for (int i = (getValue.Length - 1); i >= 0; i--)
                                {
                                    if (getValue[i] <= asc) //判断汉字的拼音区编码是否在指定范围内
                                    {
                                        pystr += getName[i];//如果不超出范围则获取对应的拼音
                                        break;
                                    }
                                }
                                break;
                        }
                    }
                }
                else//如果不是汉字
                {
                    pystr += mChar[j].ToString();//如果不是汉字则返回
                }
            }
            return pystr;//返回获取到的汉字拼音
        }

        #endregion

        #region 毕节更新
        /// <summary>
        /// 同步更新毕节人员信息 (单条/批量)
        /// </summary>
        public void UpdateHdgzUser(List<UserEntity> uentityList)
        {
            try
            {
                string isGZBJ = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("贵州毕节版本");
                if (string.IsNullOrWhiteSpace(isGZBJ))
                {
                    return;
                }
                DataItemDetailBLL itemBll = new DataItemDetailBLL();
                DataItemDetailBLL data = new DataItemDetailBLL();
                var Key = data.GetItemValue("Hdgzappkey");//毕节URL密钥
                var baseurl = data.GetItemValue("HdgzBaseUrl");//毕节API服务器地址

                string Url = "/api/v2/employee/update/";//接口地址
                List<HdgzUserEntity> hdgzUserEntityList = new List<HdgzUserEntity>();
                foreach (var uentity in uentityList)
                {
                    //var model = new
                    //{
                    //    pin = uentity.IdentifyID,
                    //    name = uentity.RealName,
                    //    deptnumber = "1"
                    //};
                    HdgzUserEntity hdgzUserEntity = new HdgzUserEntity();
                    hdgzUserEntity.pin = uentity.IdentifyID;
                    hdgzUserEntity.name = uentity.RealName;
                    hdgzUserEntity.deptnumber = "1";
                    hdgzUserEntityList.Add(hdgzUserEntity);
                }
                SocketHelper.LoadHdgzCameraList(hdgzUserEntityList, baseurl, Url, Key);
            }
            catch { }
        }
        /// <summary>
        /// 删除毕节人员信息
        /// </summary>
        public void DeleteHdgzUser(UserEntity uentity)
        {
            try
            {
                string isGZBJ = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("贵州毕节版本");
                if (string.IsNullOrWhiteSpace(isGZBJ))
                {
                    return;
                }
                DataItemDetailBLL itemBll = new DataItemDetailBLL();
                DataItemDetailBLL data = new DataItemDetailBLL();
                var Key = data.GetItemValue("Hdgzappkey");//毕节URL密钥
                var baseurl = data.GetItemValue("HdgzBaseUrl");//毕节API服务器地址

                string Url = "/api/v2/employee/delete/";//接口地址
                var model = new
                {
                    pin = uentity.IdentifyID
                };
                SocketHelper.LoadHdgzCameraList(model, baseurl, Url, Key);
            }
            catch { }
        }
        #endregion
    }
    #region 毕节辅助类
    /// <summary>
    /// 人员信息实体
    /// </summary>
    public class HdgzUserEntity
    {
        public string pin { get; set; }
        public string name { get; set; }
        public string deptnumber { get; set; }
    }
    #endregion
}
