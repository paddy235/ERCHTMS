using ERCHTMS.Busines.BaseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Data;
using ERCHTMS.Entity.BaseManage;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Busines.SystemManage;
using System.Web;
namespace ERCHTMS.AppSerivce.Controllers
{
    /// <summary>
    /// 从预控获取所有用户信息并同步到管控平台，培训平台和班组
    /// </summary>
    public class DirectoryController : ApiController
    {
        UserBLL userBll = new UserBLL();
        DepartmentBLL deptBll = new DepartmentBLL();
       
        [AcceptVerbs("Get","Post")]
        /// <summary>
        /// 验证域用户
        /// </summary>
        /// <param name="account">域账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public object checkUser()
        {
            string account = HttpContext.Current.Request["account"];
            string password = HttpContext.Current.Request["password"];
            string domainIP = Config.GetValue("DomainName");      //域名
            try
            {
                using (System.DirectoryServices.DirectoryEntry deUser = new System.DirectoryServices.DirectoryEntry(@"LDAP://" + domainIP, account, password))
                {
                    if (deUser == null)
                    {
                        return new { code = 1, message = "验证失败" };
                    }
                    else
                    {
                        if (deUser.Properties.Count==0)
                        {
                            return new { code = 1, message = "验证失败" };
                        }
                        else
                        {
                            return new { code = 0, message = "验证成功" };
                        }
                      
                    }
                }
            }
            catch (Exception ex)
            {  
                return new { code = 1, message = ex.Message };
            }
        }
        [AcceptVerbs("Get", "Post")]
        /// <summary>
        /// 验证域用户
        /// </summary>
        /// <param name="account">域账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public object queryUser()
        {
            try
            {
                string accounts = HttpContext.Current.Request["accounts"];
                StringBuilder sb = new StringBuilder();
                string domainIP = Config.GetValue("DomainName");      //域名
                string userAccount = Config.GetValue("Account");    //域账号
                string Password = Config.GetValue("Pwd");      //域账号密码 　　　　　　　　　
                using (System.DirectoryServices.DirectoryEntry deUser = new System.DirectoryServices.DirectoryEntry(@"LDAP://" + domainIP, userAccount, Password))
                {

                    System.DirectoryServices.DirectorySearcher src = new System.DirectoryServices.DirectorySearcher(deUser);
                    if (!string.IsNullOrWhiteSpace(accounts))
                    {
                        StringBuilder sbAcounts = new StringBuilder();
                        string[] arr = accounts.Split(',');
                        foreach (string str in arr)
                        {
                            sbAcounts.AppendFormat("(sAMAccountName=*{0})", accounts);
                        }
                        src.Filter = string.Format("(&(objectClass=user)(company=*广西华昇新材料有限公司)(|({0})))", sbAcounts.ToString());//筛选条件
                    }
                    else
                    {
                        src.Filter = "(&(objectClass=user)(company=*广西华昇新材料有限公司))";//筛选条件
                    }
                    src.SearchRoot = deUser;
                    src.SearchScope = System.DirectoryServices.SearchScope.Subtree;
                    System.DirectoryServices.SearchResultCollection results = src.FindAll();

                    sb.AppendFormat("总共{0}条记录\n", results.Count);
                    foreach (System.DirectoryServices.SearchResult result in results)
                    {
                        System.DirectoryServices.PropertyCollection rprops = result.GetDirectoryEntry().Properties;
                        string account = "";
                        //获取账号
                        if (rprops["sAMAccountName"] != null)
                        {
                            if (rprops["sAMAccountName"].Value != null)
                            {
                                account = rprops["sAMAccountName"].Value.ToString();
                            }

                        }
                        string realName = "";
                        //获取姓名
                        if (rprops["displayName"] != null)
                        {
                            if (rprops["displayName"].Value != null)
                            {
                                realName = rprops["displayName"].Value.ToString();
                            }

                        }
                        string mobile = "";
                        //获取手机号
                        if (rprops["telephoneNumber"] != null)
                        {
                            if (rprops["telephoneNumber"].Value != null)
                            {
                                mobile = rprops["telephoneNumber"].Value.ToString();
                            }

                        }
                        string department = "";
                        //获取部门名称
                        if (rprops["department"] != null)
                        {
                            if (rprops["department"].Value != null)
                            {
                                department = rprops["department"].Value.ToString();
                            }
                        }
                        sb.AppendFormat("账号:{0},姓名:{1},手机号:{2},部门:{3}\n", account, realName, mobile, department);
                        sb.Append("\n");

                    }
                }
                return new { code = 0, message = sb.ToString() };
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(string.Format(@"D:\logs\{0}.log", DateTime.Now.ToString("yyyyMMdd")), ex.Message);
                return new { code = 1, message = ex.Message };
            }
        }
        [AcceptVerbs("Get","Post")]
        // GET api/<controller>/5
        /// <summary>
        /// 获取域用户信息并更新系统用户（广西华昇）
        /// </summary>
        /// <param name="accounts">需要同步的用户账号(多个用逗号分隔)</param>
        /// <param name="orgId">单位Id</param>
        /// <returns></returns>
        public object SyncUser(string orgId = "2b322255-c10b-a8e6-8bd1-d2fcc7e677f8")
        {
            try
            {
                string accounts = HttpContext.Current.Request["accounts"];//需要更新的账号,为空则获取更新所有匹配的用户
                StringBuilder sb = new StringBuilder();
                string domainIP = Config.GetValue("DomainName");      //域名
                string userAccount = Config.GetValue("Account");    //域账号
                string Password = Config.GetValue("Pwd");      //域账号密码 　　　　　　　　　
                using (System.DirectoryServices.DirectoryEntry deUser = new System.DirectoryServices.DirectoryEntry(@"LDAP://" + domainIP, userAccount, Password))
                {
                    
                    System.DirectoryServices.DirectorySearcher src = new System.DirectoryServices.DirectorySearcher(deUser);
                    if (!string.IsNullOrWhiteSpace(accounts))
                    {
                        StringBuilder sbAcounts = new StringBuilder();
                        string[] arr = accounts.Split(',');
                        foreach (string str in arr)
                        {
                            sbAcounts.AppendFormat("(sAMAccountName=*{0})", accounts);
                        }
                        src.Filter = string.Format("(&(objectClass=user)(company=*广西华昇新材料有限公司)(|({0})))", sbAcounts.ToString());//筛选条件
                    }
                    else
                    {
                        src.Filter = "(&(objectClass=user)(company=*广西华昇新材料有限公司))";//筛选条件
                    }
                    //src.PropertiesToLoad.Add("cn");
                    src.SearchRoot = deUser;
                    src.SearchScope = System.DirectoryServices.SearchScope.Subtree;
                    System.DirectoryServices.SearchResultCollection results = src.FindAll();
                 
                    sb.AppendFormat("总共{0}条记录\n", results.Count);
                    List<object> list = new List<object>();
                    List<UserEntity> lstUsers = new List<UserEntity>();
                    DepartmentEntity org = deptBll.GetEntity(orgId);
                    string orgCode = org.EnCode;
                    foreach (System.DirectoryServices.SearchResult result in results)
                    {
                        System.DirectoryServices.PropertyCollection rprops = result.GetDirectoryEntry().Properties;
                        string account = "";
                        //获取账号
                        if (rprops["sAMAccountName"] != null)
                        {
                            if (rprops["sAMAccountName"].Value != null)
                            {
                                account = rprops["sAMAccountName"].Value.ToString();
                            }

                        }
                        string realName = "";
                        //获取姓名
                        if (rprops["displayName"] != null)
                        {
                            if (rprops["displayName"].Value != null)
                            {
                                realName = rprops["displayName"].Value.ToString();
                            }

                        }
                        string mobile = "";
                        //获取手机号
                        if (rprops["telephoneNumber"] != null)
                        {
                            if (rprops["telephoneNumber"].Value != null)
                            {
                                mobile = rprops["telephoneNumber"].Value.ToString();
                            }

                        }
                        string department = "";
                        string deptId = ""; //部门ID
                        string deptCode = "";//部门编码
                        string pxDeptId = ""; //培训平台部门ID
                        string pxDeptCode = "";//培训平台部门编码
                        string roleId = "";//角色ID
                        string roleName = "";//角色名称
                        //获取部门名称
                        if (rprops["department"] != null)
                        {
                            if (rprops["department"].Value != null)
                            {
                                department = rprops["department"].Value.ToString();
                                System.Data.DataTable dtDept = new System.Data.DataTable();
                                System.Data.DataTable dtRole = new System.Data.DataTable();
                                if (department == "公司领导")
                                {
                                    deptId = pxDeptId = orgId;
                                    deptCode = pxDeptCode = orgCode;
                                    dtDept = deptBll.GetDataTable(string.Format("select d.departmentid,d.encode,d.deptkey from base_department d where departmentid='{0}'", orgId));

                                    //如果是公司领导则赋予普通用户和公司级用户角色
                                    dtRole = deptBll.GetDataTable(string.Format("select r.roleid,r.fullname from base_role r where r.category=1 and fullname in('普通用户','公司级用户')"));

                                }
                                else //如果是部门
                                {
                                    dtDept = deptBll.GetDataTable(string.Format("select d.departmentid,d.encode,d.deptkey from base_department d where organizeid='{1}' and d.fullname='{0}'", department, orgId));

                                    //如果是公司领导则赋予普通用户和部门级用户角色
                                    dtRole = deptBll.GetDataTable(string.Format("select r.roleid,r.fullname from base_role r where r.category=1 and fullname in('普通用户','部门级用户')"));

                                }
                                if (dtRole.Rows.Count > 0)
                                {
                                    roleId = string.Join(",", dtRole.AsEnumerable().Select(t => t.Field<string>("roleid")).ToArray());
                                    roleName = string.Join(",", dtRole.AsEnumerable().Select(t => t.Field<string>("fullname")).ToArray());
                                }

                                if (dtDept.Rows.Count > 0)
                                {
                                    deptId = pxDeptId = dtDept.Rows[0][0].ToString();
                                    deptCode = pxDeptCode = dtDept.Rows[0][1].ToString();
                                    string deptKey = dtDept.Rows[0][2].ToString();
                                    //转换成培训平台对应的部门ID
                                    if (!string.IsNullOrWhiteSpace(deptKey))
                                    {
                                        string[] arr = deptKey.Split('|');
                                        pxDeptId = arr[0];
                                        if (arr.Length > 1)
                                        {
                                            pxDeptCode = arr[1];
                                        }
                                    }
                                }
                                else  //部门名称不匹配
                                {
                                    sb.AppendFormat("用户(账号:{0},姓名:{1},部门:{2})部门与系统部门名称不匹配,无法同步!\n", account, realName, department);
                                    continue;
                                }
                            }
                        }
                        sb.AppendFormat("账号:{0},姓名:{1},手机号:{2},部门:{3}\n", account, realName, mobile, department);
                        sb.Append("\n");
                        System.Data.DataTable dtUser = deptBll.GetDataTable(string.Format("select userid from base_user where account='{0}'", account));

                        UserEntity user = new UserEntity();
                        string action = "add";
                        string userId = Guid.NewGuid().ToString();
                        string password = "Abc@gxhs123";
                        if (dtUser.Rows.Count > 0)  //修改
                        {
                            action = "edit";
                            userId = dtUser.Rows[0][0].ToString();

                            user = userBll.GetEntity(userId);
                            password = null;
                            if (user.RoleName.Contains("部门级"))
                            {
                                user.DepartmentId = deptId;
                                user.DepartmentCode = deptCode;
                            }
                        }
                        else   //新增
                        {
                            user.UserId = userId;
                            user.Account = account;
                            user.Password = password;
                            user.RoleId = roleId;
                            user.RoleName = roleName;
                            user.IsEpiboly = "0";
                            user.IsPresence = "1";
                            user.DeleteMark = 0;
                            user.EnabledMark = 1;
                            user.DepartmentId = deptId;
                            user.DepartmentCode = deptCode;
                            user.OrganizeCode = orgCode;
                            user.OrganizeId = orgId;
                        }
                        user.OpenId = 1; //此字段标记数据来源于预控用户
                        user.RealName = realName;
                        user.Mobile = mobile;
                        userId = userBll.SaveForm(userId, user);
                        if (!string.IsNullOrWhiteSpace(userId))
                        {
                            object obj = new
                            {
                                action = action,
                                time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                userId = userId,
                                userName = realName,
                                password = password,
                                account = account,
                                deptId = pxDeptId,
                                deptCode = pxDeptCode,
                                sex = user.Gender,
                                idCard = user.IdentifyID,
                                email = user.Email,
                                mobile = user.Mobile,
                                birth = user.Birthday,
                                postId = user.DutyId,
                                postName = user.DutyName,//岗位
                                age = user.Age.ToIntOrNull(),//年龄
                                native = user.Native, //籍贯
                                nation = user.Nation, //民族
                                encode = user.EnCode,//工号
                                jobTitle = user.JobTitle,
                                techLevel = user.TechnicalGrade,
                                workType = user.Craft,
                                companyId = org.InnerPhone,
                                trainRoles = user.TrainRoleId,
                                role = 0//角色（0:学员，1:培训管理员）
                            };
                            list.Add(obj);
                            user.Password = password;
                            lstUsers.Add(user);
                            sb.AppendFormat("已同步用户信息(账号:{0},姓名:{1},部门:{2},手机号:{3})!\n", account, realName, department,mobile);
                        }
                    }
                    //推送用户数据到消息队列
                    if (list.Count > 0)
                    {
                        if (list.Count > 50)
                        {
                            int page = 0;
                            int total = list.Count;
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
                                Busines.JPush.JPushApi.PushMessage(list.Skip(j * 50).Take(50), 1);
                            }
                        }
                        else
                        {
                            Busines.JPush.JPushApi.PushMessage(list, 1);
                        }
                        System.IO.File.AppendAllText(string.Format(@"D:\logs\{0}.log",DateTime.Now.ToString("yyyyMMdd")), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"：" + list.ToJson()+"\n\n");
                    }
                    //同步用户信息到班组
                    if(lstUsers.Count>0)
                    {
                        ImportUsersToBZ(lstUsers);
                    }
                }
                return new { code =0, message =sb.ToString()};
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(string.Format(@"D:\logs\{0}.log", DateTime.Now.ToString("yyyyMMdd")), ex.Message);
                return new {code=1,message=ex.Message };
            }
           
        }
        /// <summary>
        /// 同步用户到班组
        /// </summary>
        /// <param name="userList"></param>
        private void ImportUsersToBZ(List<UserEntity> userList)
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
                wc.UploadStringCompleted += wc_UploadStringCompleted;
                wc.UploadStringAsync(new Uri(new DataItemDetailBLL().GetItemValue("bzAppUrl") + "PostEmployees"), "post", userList.ToJson());

            }
            catch (Exception ex)
            {
                //将同步结果写入日志文件
                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/syncbz/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：同步用户失败，同步信息" + Newtonsoft.Json.JsonConvert.SerializeObject(userList) + ",异常信息：" + ex.Message + "\r\n");
            }
        }
        void wc_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            var error = e.Error;
            //将同步结果写入日志文件
            string fileName = "user_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath("~/logs/syncbz")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/logs/syncbz"));
            }
            try
            {

                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/syncbz/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：同步用户结果>" + e.Result + "\r\n");
            }
            catch (Exception ex)
            {
                string msg = error == null ? ex.Message : error.Message;
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/syncbz/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：同步用户结果>" + msg + "\r\n");
            }

        }
    }
}