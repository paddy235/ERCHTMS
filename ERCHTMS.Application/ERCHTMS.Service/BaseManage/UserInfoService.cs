using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using BSFramework.Util.Extension;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Linq;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.IService.AuthorizeManage;
using ERCHTMS.Service.AuthorizeManage;
using System;
using ERCHTMS.Code;
using ERCHTMS.Service.HighRiskWork;
using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Service.SystemManage;
using System.Net;
using System.Web;
using System.Dynamic;
using ERCHTMS.IService.SystemManage;


namespace ERCHTMS.Service.BaseManage
{
    /// 描 述：用户管理
    /// </summary>
    public class UserInfoService : RepositoryFactory<UserInfoEntity>, IUserInfoService
    {
        private SidePersonService spservice = new SidePersonService();
        private IDepartmentService departservice = new DepartmentService();
        private IDataItemDetailService idataitemdetailservice = new DataItemDetailService();
        #region 获取数据
        /// <summary>
        /// 用户列表
        /// </summary> 
        /// <returns></returns>
        public DataTable GetTable()
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM  V_USERINFO ");
            return this.BaseRepository().FindTable(strSql.ToString());
        }

        /// <summary>
        /// 获取用户集合
        /// </summary>
        /// <returns></returns>
        public IList<UserInfoEntity> GetAllUserInfoList()
        {
            var sql = new StringBuilder();

            sql.Append(@"select a.* from  v_userinfo a");

            var list = this.BaseRepository().FindList(sql.ToString()).ToList();

            return list;
        }


        /// <summary>
        /// 获取特殊用户集合
        /// </summary>
        /// <param name="accounts"></param>
        /// <returns></returns>
        public IList<UserInfoEntity> GetCurLevelAndHigherLevelUserByArgs(string accounts,string rolename)  
        {
            List<UserInfoEntity> list = new List<UserInfoEntity>();
            //班组用户=》班组负责人、上级负责人
            string useraccounts = "'" + accounts.Replace(",", "','") + "'";

            string sql = string.Format(@"select a.* from  v_userinfo a where 1=1  and a.account in ({0}) and  a.userid <> 'System' and 
                                        a.enabledmark = 1 and a.deletemark=0 and a.ispresence ='是' ", useraccounts);

            List<UserInfoEntity> ulist = this.BaseRepository().FindList(sql).ToList();
            //var dlist = list.Distinct().Select(g => new { deptid = g.DepartmentId, orgid = g. }).ToList();
            string sdepttype = string.Empty;
            foreach (UserInfoEntity userinfo in ulist) 
            {
                //当前层级负责人
                var tempcur = GetWFUserListByDeptRoleOrg(userinfo.OrganizeId, userinfo.DepartmentId, string.Empty, string.Empty, string.Empty ,rolename);
                foreach (UserInfoEntity curfzUser in tempcur)
                {
                    if (list.Where(p => p.UserId == curfzUser.UserId).Count() == 0) 
                    {
                        list.Add(curfzUser);
                    }
                }
                //上级负责人
                string parentdeptid = userinfo.ParentId != "0" ? userinfo.ParentId : userinfo.DepartmentId;
                var temphigh = GetWFUserListByDeptRoleOrg(userinfo.OrganizeId, parentdeptid, string.Empty, string.Empty, string.Empty, rolename);

                foreach (UserInfoEntity curfzUser in temphigh)
                {
                    if (list.Where(p => p.UserId == curfzUser.UserId).Count() == 0)
                    {
                        list.Add(curfzUser);
                    }
                }
            }
            return list;
        }


        #region 流程配置条件提供程序
        /// <summary>
        /// 流程配置条件提供程序
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="deptmentid"></param>
        /// <param name="roleid"></param>
        ///<param name="roleid"></param>
        /// <param name="useraccounts"></param>
        /// <returns></returns>
        public IList<UserInfoEntity> GetWFUserListByDeptRoleOrg(string orgid, string deptmentid, string natrue, string roleid, string useraccounts, string rolename = "", string specialtytype ="")
        {
            string sql = string.Format(@"select a.* from  v_userinfo a
                                        where 1=1  and a.organizeid ='{0}' and  a.userid <> 'System' and  a.enabledmark = 1 and a.deletemark=0 and a.ispresence ='是' ", orgid);
            //用户
            if (!useraccounts.IsEmpty())
            {
                string ids = string.Empty;
                string[] accountids = useraccounts.Split(',');
                foreach (string account in accountids)
                {
                    if (!account.IsEmpty())
                    {
                        ids += "'" + account + "',";
                    }
                }
                if (!ids.IsEmpty())
                {
                    ids = ids.Substring(0, ids.Length - 1).ToString();
                }
                if (!ids.IsEmpty())
                {
                    sql += string.Format(" and a.account in ({0})", ids);
                }
            }

            //角色 判定依据是 传入的用户为空
            if (!roleid.IsEmpty() && useraccounts.IsEmpty())
            {
                string[] rolearray = roleid.Split(',');

                foreach (string rid in rolearray)
                {
                    if (!string.IsNullOrEmpty(rid))
                    {
                        sql += string.Format(" and a.roleid like '%{0}%'", rid);
                    }
                }
            }
            //角色
            if (!rolename.IsEmpty())
            {
                if (rolename.Contains("|")) 
                {
                    sql += "and (";

                    string[] rolearray = rolename.Split('|');

                    foreach (string rname in rolearray)
                    {
                        if (!string.IsNullOrEmpty(rname))
                        {
                            sql += string.Format("   a.rolename like '%{0}%' or", rname);
                        }
                    }
                    sql = sql.Substring(0, sql.Length - 2);
                    sql += ")";
                }
                else if (rolename.Contains("&"))
                {
                    string[] rolearray = rolename.Split('&');

                    foreach (string rname in rolearray)
                    {
                        if (!string.IsNullOrEmpty(rname))
                        {
                            sql += string.Format("  and  a.rolename like '%{0}%'  ", rname);
                        }
                    }
                }
                else 
                {
                    if (!string.IsNullOrEmpty(rolename))
                    {
                        sql += string.Format("  and  a.rolename like '%{0}%' ", rolename);
                    }
                }
            }
            //部门性质
            if (!natrue.IsEmpty())
            {
                sql += string.Format("  and  a.nature = '{0}' ", natrue);
            }

            //部门
            if (!deptmentid.IsEmpty())
            {
                sql += string.Format("  and  a.departmentid = '{0}' ", deptmentid);
            }

            //专业分类
            if (!specialtytype.IsEmpty())
            {
                sql += string.Format("  and  (','|| a.specialtytype || ',')  like  '%,{0},%'  and  a.specialtytype is not null ", specialtytype);
            }

            var list = this.BaseRepository().FindList(sql).ToList();

            return list;
        }
        #endregion


        #region 流程配置条件提供程序
        /// <summary>
        /// 流程配置条件提供程序
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="deptmentid"></param>
        /// <param name="roleid"></param>
        ///<param name="roleid"></param>
        /// <param name="useraccounts"></param>
        /// <returns></returns>
        public IList<UserInfoEntity> GetUserListByAnyCondition(string orgid, string deptmentcode, string rolecode,string majorclassify="")
        {
            string sql = string.Format(@"select distinct a.* from  v_userinfo a
                                        left join base_department b on a.departmentcode = b.encode
                                        left join base_userrelation c on a.userid = c.userid
                                        left join base_role d on  c.objectid = d.roleid
                                        where 1=1  and a.organizeid ='{0}' and  a.userid <> 'System' and  a.enabledmark = 1 and a.deletemark=0 and a.ispresence ='是' ", orgid);

            //角色 判定依据是 传入的用户为空
            if (!rolecode.IsEmpty())
            {
                string lastrole = "'" + rolecode.Replace(",", "','") + "'";

                sql += string.Format(" and  ( d.encode in ({0})", lastrole);

                //专业分类
                if (!majorclassify.IsEmpty())
                {
                    string specialtytype = string.Empty;
                    //判断是否启用专业分类 (整改责任单位关联人员专用)
                    var isenablemajorclassify = idataitemdetailservice.GetDataItemListByItemCode("'ChangeDeptRelevancePerson'").Where(p => p.ItemValue == orgid).Count() > 0;
                    if (isenablemajorclassify)
                    {
                        var majorEntity = idataitemdetailservice.GetEntity(majorclassify);
                        if (null != majorEntity)
                        {
                            specialtytype = majorEntity.ItemValue;
                        }
                        if (!string.IsNullOrEmpty(specialtytype))
                        {
                            sql += string.Format("  or  (a.rolename like '%专工%'  and  (','||a.specialtytype||',')  like  '%,{0},%')", specialtytype);
                        }
                    }

                }
                sql += ")";
            }
            //部门
            if (!deptmentcode.IsEmpty())
            {
                sql += string.Format("  and  (a.departmentcode = '{0}' or a.departmentid='{0}') ", deptmentcode);
            }
            sql += string.Format("  order by  a.sortcode  ");

            var list = this.BaseRepository().FindList(sql).ToList();

            return list;
        }
        #endregion

        #region 获取用户
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public IList<UserInfoEntity> GetUserInfoByDeptCode(string deptCode)
        {
            string sql = string.Format(@"select * from V_USERINFO where departmentcode = '{0}' or OrganizeCode='{0}' ", deptCode);

            var list = this.BaseRepository().FindList(sql.ToString()).ToList();

            return list;
        }
        /// <summary>
        /// 根据部门编码或角色编码获取用户信息
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public IList<UserInfoEntity> GetUserListByCodeAndRole(string deptCode, string roleCode)
        {
            string sql = @"select distinct a.* from V_USERINFO a
                                    left join base_department b on a.departmentcode = b.encode
                                    left join base_userrelation c on a.userid = c.userid
                                    left join base_role d on  c.objectid = d.roleid
                                    where 1=1  ";

            string str = " ";
            if (!string.IsNullOrEmpty(deptCode))
            {
                str += string.Format("  and  a.departmentcode='{0}'", deptCode);
            }
            if (!string.IsNullOrEmpty(roleCode))
            {

                str += string.Format("  and  d.encode in ('{0}') ", roleCode);

            }
            sql += str;
            var list = this.BaseRepository().FindList(sql).ToList();

            return list;
        }
        #endregion

        /// <summary>
        /// 判断是否存在对应的角色
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="rolename"></param>
        /// <returns></returns>
        public DataTable HaveRoleListByKey(string keyValue, string rolecode)
        {


            string sql = string.Format(@"select c.account,a.userid,c.realname, b.* from base_userrelation a 
                                        left join base_role b on a.objectid = b.roleid
                                        left join base_user c on a.userid = c.userid where a.userid='{0}'
                                        and b.encode in ({1})", keyValue, rolecode);

            var dt = this.BaseRepository().FindTable(sql.ToString());

            return dt;
        }

 

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<UserInfoEntity> GetPageList(Pagination pagination, string queryJson)
        {
            var user = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;
            if (!string.IsNullOrEmpty(queryJson))
            {
                #region 条件查询

                var queryParam = queryJson.ToJObject();
                //开工申请特殊处理
                if (!queryParam["rolename"].IsEmpty())
                {
                    string rolename = queryParam["rolename"].ToString();
                    if (rolename == "现场负责人")
                    {
                        pagination.conditionJson += string.Format(" and DUTYNAME in('现场负责人')");
                    }
                    if (rolename == "现场安全员")
                    {
                        pagination.conditionJson += string.Format(" and DUTYNAME in('安全员','专/兼职安全员')");
                    }
                    if (rolename == "负责人")
                    {
                        pagination.conditionJson += string.Format(" and instr(ROLENAME,'负责人')>0");
                    }
                    if (rolename == "分委会成员")
                    {//标准修（订）审核发布流程
                        DataItemModel president = new DataItemDetailService().GetDataItemListByItemCode("'PresidentApprove'").Where(p => p.ItemName == user.OrganizeId).ToList().FirstOrDefault();
                        if (president != null)//除总经理外的公司级用户
                            pagination.conditionJson += string.Format(" and instr(ROLENAME,'公司级用户')>0 and Account!='{0}'", president.ItemValue);
                        else
                            pagination.conditionJson += string.Format(" and instr(ROLENAME,'公司级用户')>0");
                    }
                }
                if (!queryParam["projectid"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and projectid ='{0}'", queryParam["projectid"].ToString());
                }
                //是否特种作业或特种设备操作人员
                if (!queryParam["userKind"].IsEmpty())
                {
                    string userKind = queryParam["userKind"].ToString();
                    if (userKind == "1")
                    {
                        pagination.conditionJson += string.Format(" and isspecial='是'", 0);
                    }
                    if (userKind == "2")
                    {
                        pagination.conditionJson += string.Format(" and isspecialequ='是'", 0);
                    }

                }
                //排除不需要选择的人员id
                if (!queryParam["eliminateUserIds"].IsEmpty())
                {
                    string eluserids = queryParam["eliminateUserIds"].ToString();
                    string idsarr = "";
                    if (eluserids.Contains(','))
                    {
                        string[] array = eluserids.Split(',');
                        foreach (var item in array)
                        {
                            idsarr += "'" + item + "',";
                        }
                        if (idsarr.Contains(","))
                            eluserids = idsarr.TrimEnd(',');
                    }
                    else
                    {
                        eluserids = "'" + eluserids + "'";
                    }
                    pagination.conditionJson += string.Format(" and t.UserId  not in({0})", eluserids);
                }
                if (!queryParam["special"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and t.UserId  not in(select sideuserid from bis_sideperson where createuserorgcode='{0}')", user.OrganizeCode);
                }
                if (!queryParam["side"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and t.UserId  in(select sideuserid from bis_sideperson where createuserorgcode='{0}')", user.OrganizeCode);
                }
                if (!queryParam["threeperson"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and t.fourpersontype like '%{0}%'", queryParam["threeperson"].ToString());
                }
                if (!queryParam["state"].IsEmpty() && queryParam["state"].ToString() == "1")
                {
                    //用户组id
                    string userListId = queryParam["code"].ToString();
                    pagination.conditionJson += string.Format(" and userid in(select e.userid from BIS_UserListManage e where e.moduleid='{0}')", userListId);
                }
                else if (!queryParam["state"].IsEmpty() && queryParam["state"].ToString() == "0")
                {
                    //公司主键
                    if (!queryParam["organizeId"].IsEmpty())
                    {
                        string organizeId = queryParam["organizeId"].ToString();
                        pagination.conditionJson += string.Format(" and t.ORGANIZEID = '{0}'", organizeId);
                    }
                    //部门Id 
                    if (!queryParam["departmentId"].IsEmpty())
                    {
                        string departmentId = queryParam["departmentId"].ToString();
                        if (!(!queryParam["containchild"].IsEmpty() && queryParam["containchild"].ToString() == "1"))
                        {
                            departmentId = departmentId.Replace("cx100_", "").Replace("ls100_", "");
                            if (!queryParam["pType"].IsEmpty())
                            {
                                if (queryParam["pType"].ToString() != "1")
                                {

                                    pagination.conditionJson += string.Format(" and (t.DEPARTMENTID in('{0}') or nickname in('{0}'))", departmentId.Replace(",", "','"));
                                }
                            }
                            else
                            {
                                pagination.conditionJson += string.Format(" and (t.DEPARTMENTID in('{0}') or nickname in('{0}'))", departmentId.Replace(",", "','"));
                            }

                        }
                    }
                    //部门编码
                    if (!queryParam["departmentCode"].IsEmpty())
                    {
                        string departmentCode = queryParam["departmentCode"].ToString();
                        pagination.conditionJson += string.Format(" and (t.DEPARTMENTID =(select departmentid from base_department where encode='{0}') or t.nickname =(select departmentid from base_department where encode='{0}'))", departmentCode);
                    }

                    if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
                    {
                        string deptCode = queryParam["code"].ToString();
                        string deptType = "";
                        if (deptCode.StartsWith("cx100_"))
                        {
                            deptCode = deptCode.Replace("cx100_", "");
                            deptType = "长协";
                        }
                        if (deptCode.StartsWith("ls100_"))
                        {
                            deptCode = deptCode.Replace("ls100_", "");
                            deptType = "临时";
                        }
                        if (queryParam["isOrg"].ToString() == "Organize")
                        {
                            pagination.conditionJson += string.Format(" and t.organizeid  in (select departmentid from base_department where encode like '{0}%')", deptCode);
                        }
                        else
                        {
                            pagination.conditionJson += string.Format(" and (t.departmentid  in (select departmentid from base_department where encode like '{0}%') or t.nickname  in (select departmentid from base_department where encode like '{0}%'))", deptCode);
                        }

                        if (!string.IsNullOrWhiteSpace(deptType))
                        {
                            pagination.conditionJson += string.Format("and t.departmentid in(select departmentid from base_department d where d.depttype='{0}' and d.encode like '{1}%')", deptType, deptCode);
                        }
                    }

                    if (!queryParam["code"].IsEmpty() && !queryParam["containchild"].IsEmpty())
                    {
                        string deptCode = queryParam["code"].ToString();
                        string deptType = "";
                        if (deptCode.StartsWith("cx100_"))
                        {
                            deptCode = deptCode.Replace("cx100_", "");
                            deptType = "长协";
                        }
                        if (deptCode.StartsWith("ls100_"))
                        {
                            deptCode = deptCode.Replace("ls100_", "");
                            deptType = "临时";
                        }
                        if (queryParam["containchild"].ToString() == "1")
                        {
                            pagination.conditionJson += string.Format(" and (t.departmentid  in (select departmentid from base_department where encode like '{0}%') or t.nickname  in (select departmentid from base_department where encode like '{0}%'))", deptCode);
                            if (!queryParam["mode"].IsEmpty())
                            {
                                if (queryParam["mode"].ToString() == "2")
                                {
                                    if (user.RoleName.Contains("部门级用户") && !user.RoleName.Contains("厂级"))
                                    {
                                        pagination.conditionJson += string.Format(" and t.departmentid in (select OUTPROJECTID from EPG_OUTSOURINGENGINEER where ENGINEERLETDEPTID='{0}')", user.DeptId);

                                    }
                                }

                            }

                        }
                        else
                        {
                            pagination.conditionJson += string.Format(" and (t.departmentid  in (select departmentid from base_department where encode = '{0}') or t.nickname  in (select departmentid from base_department where encode = '{0}'))", deptCode);
                        }

                        if (!string.IsNullOrWhiteSpace(deptType))
                        {
                            pagination.conditionJson += string.Format("and t.departmentid in(select departmentid from base_department d where d.depttype='{0}' and d.encode like '{1}%')", deptType, deptCode);
                        }
                    }
                }
                if (!queryParam["selDeptIds"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and t.departmentid in('{0}')", queryParam["selDeptIds"].ToString().Replace(",", "','"));
                }
                if (!queryParam["pType"].IsEmpty())
                {
                    if (!queryParam["departmentId"].IsEmpty())
                    {
                        string deptIds = queryParam["departmentId"].ToString();
                        if (queryParam["pType"].ToString() == "1")
                        {
                            if (user.RoleName.Contains("省级"))
                            {
                                StringBuilder sb = new StringBuilder();
                                foreach (string deptId in deptIds.Split(','))
                                {
                                    DataTable dtDept = BaseRepository().FindTable(string.Format("select t.departmentid from BASE_DEPARTMENT t where t.isorg=1 and t.parentid='{0}'", deptId));
                                    foreach (DataRow dr in dtDept.Rows)
                                    {
                                        sb.AppendFormat("{0},", dr[0].ToString());
                                    }
                                }
                                pagination.conditionJson += string.Format(" and (departmentid in('{1}') or departmentid in('{0}'))  and ((rolename like '%公司级用户%' and rolename like '%安全管理员%') or (rolename like '%厂级部门%' and rolename like '%负责人%') )", sb.ToString().Trim(',').Replace(",", "','"), deptIds.Replace(",", "','"));
                            }
                            else
                            {
                                pagination.conditionJson += string.Format(" and t.DEPARTMENTID in('{0}') or (departmentid in('{0}') and ((rolename like '%公司级用户%' and rolename like '%安全管理员%') or (rolename like '%厂级部门%' and rolename like '%负责人%') or (rolename like '%负责人%' or rolename like '%安全管理员%')))", deptIds.Replace(",", "','"));
                            }

                            //pagination.conditionJson += string.Format(" and (t.rolename like '%安全管理员%' or t.rolename like '%公司领导%' or t.rolename like '%负责人%')");
                        }
                        if (queryParam["pType"].ToString() == "2")
                        {
                            pagination.conditionJson += string.Format(" or (departmentid in('{0}') and (t.rolename like '%负责人%' or  t.rolename like '%安全管理员%'))", deptIds.Replace(",", "','"));
                        }
                    }
                }
                //查询条件
                if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
                {
                    string condition = queryParam["condition"].ToString();
                    string keyord = queryParam["keyword"].ToString();
                    switch (condition)
                    {
                        case "Account":            //账户
                            pagination.conditionJson += string.Format(" and t.ACCOUNT  like '%{0}%'", keyord);
                            break;
                        case "RealName":          //姓名
                            pagination.conditionJson += string.Format(" and t.REALNAME  like '%{0}%'", keyord);
                            break;
                        case "Mobile":          //手机
                            pagination.conditionJson += string.Format(" and t.MOBILE like '%{0}%'", keyord);
                            break;
                        default:
                            break;
                    }
                }
                if (!queryParam["source"].IsEmpty())
                {
                    var items = queryParam.SelectToken("source").ToObject<string[]>();
                    pagination.conditionJson += string.Format(" and t.USERID in ('{0}')", string.Join("','", items));
                }
                //自动补全查询
                if (!queryParam["autocomplete"].IsEmpty())
                {
                    if (!queryParam["username"].IsEmpty())
                    {
                        pagination.conditionJson += string.Format(" and t.REALNAME  like '%{0}%'", queryParam["username"].ToString());
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(" and 1!=1");
                    }
                }
               
                #endregion
            }
            IEnumerable<UserInfoEntity> list = this.BaseRepository().FindListByProcPager(pagination, dataType);
            return list;
        }
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllTable()
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM  V_USERINFO ");
            return this.BaseRepository().FindTable(strSql.ToString());
        }
        /// <summary>
        /// 用户列表（导出Excel）
        /// </summary>
        /// <returns></returns>
        public DataTable GetExportList()
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM  V_USERINFO");
            return this.BaseRepository().FindTable(strSql.ToString());
        }
        /// <summary>
        /// 用户实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public UserInfoEntity GetUserInfoEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        #endregion
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        public UserInfoEntity CheckLogin(string username)
        {
            var expression = LinqExtensions.True<UserInfoEntity>();
            expression = expression.And(t => t.Account == username);
            expression = expression.Or(t => t.Mobile == username);
            expression = expression.Or(t => t.Email == username);
            expression = expression.Or(t => t.EnCode == username);
            return this.BaseRepository().FindEntity(expression);
        }
        /// <summary>
        /// 根据用户账号获取用户信息
        /// </summary>
        /// <param name="account">账号</param>
        /// <returns></returns>
        public UserInfoEntity GetUserInfoByAccount(string account)
        {
            account = account.Trim();
            var expression = LinqExtensions.True<UserInfoEntity>();
            expression = expression.And(t => t.Account.ToLower() == account.ToLower() || t.Mobile == account || t.Email == account || t.EnCode == account);
            return this.BaseRepository().FindEntity(expression);
        }
        /// <summary>
        /// 根据部门、用户名称获取用户信息
        /// </summary>
        /// <param name="deptName">部门名称</param>
        /// <param name="userName">用户姓名</param>
        /// <returns></returns>
        public UserInfoEntity GetUserInfoByName(string deptName, string userName)
        {
            var expression = LinqExtensions.True<UserInfoEntity>();
            expression = expression.And(t => t.RealName == userName);
            expression = expression.And(t => t.DeptName == deptName);
            return this.BaseRepository().FindEntity(expression);
        }
        /// <summary>
        /// 根据人员持证率得分（特种作业人员和安全管理人员持证率）
        /// </summary>
        /// <param name="user"></param>
        /// <param name="time">月份，如2017-10-01</param>
        /// <returns></returns>
        public decimal GetIndexScoreByTime(ERCHTMS.Code.Operator user, string time = "")
        {
            string orgId = user.OrganizeId;
            string orgCode = user.OrganizeCode;
            string roleNames = user.RoleName;

            string sql = string.Format("select count(1) from BIS_CLASSIFICATIONINDEX where affiliatedorganizeid=@orgId");
            int count = this.BaseRepository().FindObject(sql, new DbParameter[] { DbParameters.CreateDbParameter("@orgId", orgId) }).ToInt();//查询当前电厂是否有设置考核指标
            sql = string.Format("select t.indexscore totalscore,t.indexstandardformat descr,t.indexargsvalue val,indexcode from  BIS_CLASSIFICATIONINDEX t where t.classificationcode='10'");//人员管理
            //没有的话就使用内置默认的数据
            if (count == 0)
            {
                sql += " and affiliatedorganizeid='0'";
            }
            else
            {
                sql += " and affiliatedorganizeid=@orgId";
            }
            //获取各项设置的指标值
            DataTable dt = this.BaseRepository().FindTable(sql, new DbParameter[] { DbParameters.CreateDbParameter("@orgId", orgId) });
            //总得分
            decimal totalScore = 0;
            foreach (DataRow dr in dt.Rows)
            {
                decimal sumScore = decimal.Parse(dr["totalscore"].ToString());
                decimal sum = 0; decimal num = 0; decimal score = 0;
                //特种作业人员持证率 
                if (dr["indexcode"].ToString() == "01")
                {
                    string certType = "特种作业操作证";
                    sum = decimal.Parse(this.BaseRepository().FindObject(string.Format("select count(1) from base_user u where (isspecial='是' or isspecialequ='是') and organizecode='{0}') group by userid) t", orgCode)).ToString());
                    if (sum > 0)
                    {
                        num = decimal.Parse(this.BaseRepository().FindObject(string.Format("select case when sum(num) is null then 0 else  sum(num) end from (select  count(1) as num from BIS_CERTIFICATE t where certname='{1}' and enddate<sysdate and userid in(select userid from base_user u where (isspecial='是' or isspecialequ='是') and organizecode='{0}') group by userid) t", orgCode, certType)).ToString());
                        decimal result = num / sum;
                        string[] arr = dr["val"].ToString().Split('|');
                        if (result > decimal.Parse(arr[0]))
                        {
                            result = decimal.Parse(arr[0]) - result;
                            count = int.Parse((result / decimal.Parse(arr[1])).ToString());
                            if (count > 1)
                            {
                                score = sumScore - count * decimal.Parse(arr[2]);
                                if (score > 0)
                                {
                                    totalScore += sumScore - score;
                                }
                            }

                        }
                    }
                    else
                    {
                        totalScore += sumScore;
                    }

                }
                //安全管理人员持证率
                else if (dr["indexcode"].ToString() == "02")
                {
                    sum = decimal.Parse(this.BaseRepository().FindObject(string.Format("select count(1) from base_user u where usertype='安全管理人员' and organizecode='{0}') group by userid) t", orgCode)).ToString());
                    if (sum > 0)
                    {
                        num = decimal.Parse(this.BaseRepository().FindObject(string.Format("select case when sum(num) is null then 0 else  sum(num) end from (select  count(1) as num from BIS_CERTIFICATE t where  enddate<sysdate and userid in(select userid from base_user u where usertype='安全管理人员' and organizecode='{0}') group by userid) t", orgCode)).ToString());
                        decimal result = num / sum;
                        string[] arr = dr["val"].ToString().Split('|');
                        if (result > decimal.Parse(arr[0]))
                        {
                            result = decimal.Parse(arr[0]) - result;
                            count = int.Parse((result / decimal.Parse(arr[1])).ToString());
                            if (count > 1)
                            {
                                score = sumScore - count * decimal.Parse(arr[2]);
                                if (score > 0)
                                {
                                    totalScore += sumScore - score;
                                }
                            }


                        }
                    }
                    else
                    {
                        totalScore += sumScore;
                    }
                }
            }
            dt.Dispose();
            return totalScore;
        }
        /// <summary>
        /// 获取人员来自培训平台培训档案
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public string GetTrainRecord(string userId, string userAccount, string deptId, string idCard = "")
        {
            string fileName = "Train_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            try
            {

                if (string.IsNullOrEmpty(deptId))
                {
                    object obj = BaseRepository().FindObject(string.Format("select px_deptid from XSS_USER  where userid='{0}'", userId));
                    if (obj != null)
                    {
                        deptId = obj.ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                WebClient wc = new WebClient();
                wc.Credentials = CredentialCache.DefaultCredentials;
                wc.Headers.Add("Content-Type", "text/json; charset=utf-8");
                System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                //发送请求到web api并获取返回值，默认为post方式
                string url = new DataItemDetailService().GetItemValue("TrainServiceUrl");
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(new { business = "GetTrainRecord", idcard = idCard, DeptId = deptId, userAccount = userAccount });
                nc.Add("json", json);
                string result = wc.DownloadString(new Uri(url + "?json=" + json));
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ：获取人员培训档案,远程服务器返回信息：" + result + "\r\n");
                return result;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ：获取人员培训档案异常,信息：" + ex.Message + "\r\n");
                return "";
            }
        }
        /// <summary>
        /// 获取人员来自培训平台的考试记录
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public string GetExamRecord(string userId, string userAccount, string deptId, string idCard = "")
        {
            string fileName = "Train_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            try
            {
                if (string.IsNullOrEmpty(deptId))
                {
                    var user = GetUserInfoEntity(userId);
                    deptId = BaseRepository().FindObject(string.Format("select px_deptid from xss_dept where deptid='{0}'", user.DepartmentId)).ToString();
                }
                WebClient wc = new WebClient();
                wc.Credentials = CredentialCache.DefaultCredentials;
                wc.Headers.Add("Content-Type", "text/json; charset=utf-8");
                System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                //发送请求到web api并获取返回值，默认为post方式
                string url = new DataItemDetailService().GetItemValue("TrainServiceUrl");
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(new { business = "GetExamRecord", idcard = idCard, DeptId = deptId, userAccount = userAccount });
                nc.Add("json", json);
                string result = wc.DownloadString(new Uri(url + "?json=" + json));
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ：获取人员考试记录,远程服务器返回信息：" + result + "\r\n");
                return result;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ：获取人员考试记录异常,信息：" + ex.Message + "\r\n");
                return "";
            }
        }
        /// <summary>
        /// 同步用户信息到培训平台
        /// </summary>
        /// <param name="user">用户基本信息</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public string SyncUser(UserEntity user, string pwd, ERCHTMS.Code.Operator currUser = null)
        {
            string logPath = new DataItemDetailService().GetItemValue("imgPath") + "\\logs\\";
            string fileName = "Train_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            try
            {
                string result = "";
                DataTable dt = BaseRepository().FindTable(string.Format("select px_deptid from xss_dept where deptid='{0}'", user.DepartmentId));
                if (dt.Rows.Count > 0)
                {
                    string deptId = dt.Rows[0][0].ToString();
                    pwd = pwd.Replace("*", "");
                    user = DbFactory.Base().FindEntity<UserEntity>(user.UserId);
                    if (string.IsNullOrEmpty(user.NewPassword))
                    {
                        pwd = string.IsNullOrEmpty(pwd) ? "123456" : pwd;
                    }
                    else
                    {
                        pwd = DESEncrypt.Decrypt(user.NewPassword, user.Secretkey);

                    }
                    List<object> list = new List<object>();
                    list.Add(new
                    {
                        Id = user.UserId,
                        userName = user.RealName,
                        userAccount = user.Account,
                        pwd = pwd,
                        IdCard = user.IdentifyID,
                        departid = deptId,
                        role = "0",
                        userkind = "一般人员",
                        telephone = user.Mobile
                    });
                    if (user.IsTrainAdmin == 1)
                    {
                        string str = user.IdentifyID.Substring(0, user.IdentifyID.Length - 1);
                        string last = user.IdentifyID.Substring(user.IdentifyID.Length - 1);
                        if (last.ToLower().Equals("x") || !last.Equals("0"))
                        {
                            last = "1";
                        }
                        else
                        {
                            last = "0";
                        }
                        list.Add(new
                        {
                            Id = Guid.NewGuid().ToString(),
                            userName = user.RealName,
                            userAccount = user.Account + "gly",
                            pwd = pwd,
                            IdCard = str + last,
                            departid = deptId,
                            role = "1",
                            userkind = "一般人员",
                            telephone = user.Mobile
                        });
                    }
                    WebClient wc = new WebClient();
                    wc.Credentials = CredentialCache.DefaultCredentials;
                    //wc.Headers.Add("Content-Type", "text/json; charset=utf-8");
                    wc.Encoding = Encoding.GetEncoding("GB2312");
                    System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                    byte[] bytes = null;
                    //发送请求到web api并获取返回值，默认为post方式
                    string url = new DataItemDetailService().GetItemValue("TrainServiceUrl");
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(new { business = "saverUser", UserInfo = list });
                    nc.Add("json", json);
                    bytes = wc.UploadValues(new Uri(url), "POST", nc);
                    result = Encoding.UTF8.GetString(bytes);
                    dynamic dy = Newtonsoft.Json.JsonConvert.DeserializeObject<ExpandoObject>(result);

                    if (dy.meta.success)
                    {
                        int count = BaseRepository().FindObject(string.Format("select count(1) from xss_user where useraccount='{0}'", user.Account)).ToInt();
                        if (count == 0)
                        {
                            if (BaseRepository().ExecuteBySql(string.Format("insert into xss_user(userid,useraccount,username,deptid,deptcode,px_deptid,px_uid,px_account,idcard) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')", user.UserId, user.Account, user.RealName, user.DepartmentId, user.DepartmentCode, deptId, user.UserId, user.Account, user.IdentifyID)) > 0)
                            {
                                System.IO.File.AppendAllText(logPath + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " >操作用户：" + currUser.Account + "(" + currUser.UserName + "),新增用户关联信息到双控平台成功，用户信息：" + json + "\r\n");
                            }
                            else
                            {
                                System.IO.File.AppendAllText(logPath + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " >操作用户：" + currUser.Account + "(" + currUser.UserName + "),新增用户关联信息到双控平台失败,用户信息：" + json + " \r\n");
                            }
                        }

                    }
                    System.IO.File.AppendAllText(logPath + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " >操作用户：" + currUser.Account + "(" + currUser.UserName + "),执行动作：同步用户到培训平台,返回结果：" + result + "\r\n");
                }
                else
                {
                    System.IO.File.AppendAllText(logPath + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " >操作用户：" + currUser.Account + "(" + currUser.UserName + "),执行动作：同步用户到培训平台,返回结果：\r\n");
                }
                return result;
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(logPath + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " >：" + currUser.Account + "(" + currUser.UserName + ")，同步用户到培训平台失败,该用户所属部门在培训平台中不存在或关系未配置！\r\n");
                return ex.Message;
            }

        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public IEnumerable<UserInfoEntity> GetUserListBySql(string strSql)
        {
            return this.BaseRepository().FindList(strSql);
        }
    }
}
