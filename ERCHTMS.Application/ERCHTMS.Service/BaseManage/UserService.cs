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
using System.Linq.Expressions;
using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;
using System.Net;
using ERCHTMS.Service.SystemManage;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace ERCHTMS.Service.BaseManage
{
    /// 描 述：用户管理
    /// </summary>
    public class UserService : RepositoryFactory<UserEntity>, IUserService
    {
        private IAuthorizeService<UserEntity> iauthorizeservice = new AuthorizeService<UserEntity>();


        private SidePersonService spservice = new SidePersonService();

        #region 获取数据

        /// <summary> 
        /// 获取列表
        /// </summary>
        /// <param name="condition">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<UserEntity> GetListForCon(Expression<Func<UserEntity, bool>> condition)
        {

            return this.BaseRepository().IQueryable(condition).ToList();
        }

        /// <summary> 
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable(string deptId = "")
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT  u.userid,u.EnCode,u.Account,u.RealName,u.departmentid,gender,
                                    d.FullName AS DepartmentName,u.OrganizeId
                            FROM    Base_User u
                                    LEFT JOIN Base_Department d ON d.DepartmentId = u.DepartmentId
                            WHERE   1=1");
            strSql.Append(" AND u.UserId <> 'System' AND u.EnabledMark = 1 AND u.DeleteMark=0");
            if (!string.IsNullOrEmpty(deptId))
            {
                strSql.Append(" AND (u.organizeid=@deptId or u.DepartmentId=@deptId) ");
            }
            return this.BaseRepository().FindTable(strSql.ToString(), new DbParameter[] { DbParameters.CreateDbParameter("@deptId", deptId) });
        }

        /// <summary> 
        /// 通过用户id获取用户列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetUserTable(string[] userids)
        {
            var strSql = new StringBuilder();
            string sql = string.Join(",", userids).Replace(",", "','");
            strSql.Append(string.Format(@"SELECT  USERID,
                            REALNAME, MOBILE, OrganizeName, ORGANIZEID, DEPTNAME, DEPTCODE, DEPARTMENTID, DEPARTMENTCODE, DUTYNAME, POSTNAME, ROLENAME, ROLEID, MANAGER, ENABLEDMARK, ENCODE, ACCOUNT, NICKNAME, HEADICON, GENDER, EMAIL, Telephone,OrganizeCode, identifyid,SignImg
                            FROM V_USERINFO u WHERE Account!='System' and ispresence='是' and (userid in('{0}') or account in('{0}'))", sql));

            //DbParameter[] dp = new DbParameter[userids.Length];
            //if (userids.Length > 1)
            //{
            //    for (int i = 0; i < userids.Length; i++)
            //    {
            //        if (i == 0)
            //        {
            //            strSql.Append(" AND (u.userid=@userid  ");
            //            dp[i] = DbParameters.CreateDbParameter("@userid", userids[i]);
            //        }
            //        else if (i == userids.Length - 1)
            //        {
            //            strSql.Append(" or u.userid=@userid) ");
            //            dp[i] = DbParameters.CreateDbParameter("@userid", userids[i]);
            //        }
            //        else
            //        {
            //            strSql.Append(" or u.userid=@userid  ");
            //            dp[i] = DbParameters.CreateDbParameter("@userid", userids[i]);
            //        }
            //    }

            //}
            //else if (userids.Length == 1)
            //{
            //    strSql.Append(" AND u.userid=@userid  ");
            //    dp[0] = DbParameters.CreateDbParameter("@userid", userids[0]);
            //}
            return this.BaseRepository().FindTable(strSql.ToString());
        }
        public DataTable GetMembers(string deptId)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT  u.userid id,u.RealName username,case when u.headicon is null then '../Content/images/on-line.png' else headicon end avatar,'签名' sign
                            FROM    Base_User u
                                    LEFT JOIN Base_Department d ON d.DepartmentId = u.DepartmentId
                            WHERE   1=1");
            strSql.Append(" AND u.UserId <> 'System' AND u.EnabledMark = 1 AND u.DeleteMark=0");
            if (!string.IsNullOrEmpty(deptId))
            {
                strSql.Append(" AND u.DepartmentId=@deptId ");
            }
            return this.BaseRepository().FindTable(strSql.ToString(), new DbParameter[] { DbParameters.CreateDbParameter("@deptId", deptId) });
        }
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserEntity> GetList()
        {
            var expression = LinqExtensions.True<UserEntity>();
            expression = expression.And(t => t.EnabledMark == 1).And(t => t.DeleteMark == 0);
            return this.BaseRepository().IQueryable(expression).OrderByDescending(t => t.CreateDate).ToList();
        }
        /// <summary>
        /// 根据用户ID和类别获取用户拥有的资源名称，多个用英文逗号分隔
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="category">数据类别，1:部门名称,2:角色名称,3:岗位名称,4:职位名称,5:工作组</param>
        /// <returns></returns>
        public string GetObjectName(string userId, int category)
        {
            StringBuilder sb = new StringBuilder();
            DbParameter[] param ={
                                    DbParameters.CreateDbParameter("@userId",userId),
                                    DbParameters.CreateDbParameter("@category",category)
                                };
            DataTable dt = this.BaseRepository().FindTable("select fullname from base_role where roleid in( select objectid from BASE_USERRELATION  where userid=@userId and category=@category)", param.ToArray());
            foreach (DataRow dr in dt.Rows)
            {
                sb.Append(dr[0].ToString() + ",");
            }
            dt.Dispose();
            return sb.ToString().TrimEnd(',');
        }
        /// <summary>
        /// 根据用户ID和类别获取用户拥有的资源Id，多个用英文逗号分隔
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="category">数据类别，1:部门名称,2:角色名称,3:岗位名称,4:职位名称,5:工作组</param>
        /// <returns></returns>
        public string GetObjectId(string userId, int category)
        {
            StringBuilder sb = new StringBuilder();
            DbParameter[] param ={
                                    DbParameters.CreateDbParameter("@userId",userId),
                                    DbParameters.CreateDbParameter("@category",category)
                                };
            DataTable dt = this.BaseRepository().FindTable("select roleid from base_role where roleid in( select objectid from BASE_USERRELATION  where userid=@userId and category=@category)", param.ToArray());
            foreach (DataRow dr in dt.Rows)
            {
                sb.Append(dr[0].ToString() + ",");
            }
            dt.Dispose();
            return sb.ToString().TrimEnd(',');
        }
        /// <summary>
        /// 根据用户ID获取角色名称，多个用英文逗号分隔
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public string GetRoleName(string userId)
        {
            return GetObjectName(userId, 2);
        }
        /// <summary>
        /// 根据用户ID获取角色Id，多个用英文逗号分隔
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public string GetRoleId(string userId)
        {
            return GetObjectId(userId, 2);
        }

        /// <summary>
        ///获取培训人员记录(来自工具箱同步过来的数据)
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetTrainUsersPageList(Pagination pagination, string queryJson)
        {
            var curuser = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            //是否三种人员
            if (!queryParam["isThree"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and ISFOURPERSON='是'", 0);
            }
            //查询条件
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyord = queryParam["keyword"].ToString().Trim();
                switch (condition)
                {
                    case "Account":            //账户
                        pagination.conditionJson += string.Format(" and ACCOUNT  like '%{0}%'", keyord);
                        break;
                    case "RealName":          //姓名
                        pagination.conditionJson += string.Format(" and REALNAME  like '%{0}%'", keyord);
                        break;
                    case "Mobile":          //手机
                        pagination.conditionJson += string.Format(" and MOBILE like '%{0}%'", keyord);
                        break;
                    case "DeptName":          //部门名称机
                        pagination.conditionJson += string.Format(" and DeptName like '%{0}%'", keyord);
                        break;
                    case "identifyid":          //身份证号
                        pagination.conditionJson += string.Format(" and identifyid like '%{0}%'", keyord);
                        break;
                    default:
                        break;
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            var curuser = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            //是否三种人员
            if (!queryParam["isThree"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and ISFOURPERSON='是'", 0);
            }
            //是否三种人员
            if (!queryParam["threetype"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and fourpersontype='{0}'", queryParam["threetype"].ToString());
            }
            //是否特种作业或特种设备操作人员
            if (!queryParam["userKind"].IsEmpty())
            {
                string userKind = queryParam["userKind"].ToString();
                if (userKind == "1")
                {
                    pagination.conditionJson += string.Format(" and (isspecial='1' or isspecial='是')", 0);
                }
                if (userKind == "2")
                {
                    pagination.conditionJson += string.Format(" and (isspecialequ='1' or isspecialequ='是')", 0);
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
                pagination.conditionJson += string.Format(" and t.UserId  not in(select sideuserid from bis_sideperson where createuserorgcode='{0}')", curuser.OrganizeCode);
            }
            if (!queryParam["side"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.UserId  in(select sideuserid from bis_sideperson where createuserorgcode='{0}')", curuser.OrganizeCode);
            }
            //姓名
            if (!queryParam["userName"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and  realname like '%{0}%'", queryParam["userName"].ToString().Trim());
            }
            //身份证号
            if (!queryParam["idCard"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and  identifyid like '%{0}%'", queryParam["idCard"].ToString().Trim());
            }
            //部门名称
            if (!queryParam["deptName"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and  DEPTNAME like '%{0}%'", queryParam["deptName"].ToString());
            }
            //黑名单人员
            if (!queryParam["isBlack"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and  isBlack=1", 0);
            }
            //排除黑名单人员
            if (!queryParam["NotBlack"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and (isBlack=0 or isblack is null)", 0);
            }
            //排除离场人员
            if (!queryParam["notPresence"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and isPresence='{0}'", "是");
            }
            //离场人员
            if (!queryParam["isPresence"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and isPresence='{0}'", "否");
            }
            //离场人员
            if (!queryParam["userType"].IsEmpty())
            {
                string userType = queryParam["userType"].ToString();
                pagination.conditionJson += string.Format(" and userType='{0}'", userType);
            }
            //公司主键
            if (!queryParam["organizeId"].IsEmpty())
            {
                string organizeId = queryParam["organizeId"].ToString();
                pagination.conditionJson += string.Format(" and ORGANIZEID = '{0}'", organizeId);
            }
            //部门主键
            if (!queryParam["departmentId"].IsEmpty())//departmentcode
            {
                string departmentId = queryParam["departmentId"].ToString();
                pagination.conditionJson += string.Format(" and DEPARTMENTID = '{0}'", departmentId);
            }
            if (!queryParam["departmentCode"].IsEmpty())
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                string departmentcode = queryParam["departmentCode"].ToString();
                if (!queryParam["datatype"].IsEmpty())
                {
                    string type = queryParam["datatype"].ToString();
                    if (type == "1")
                    {
                        pagination.conditionJson += string.Format(" and departmentcode like '{0}%' and (DEPARTMENTID in(select DEPARTMENTID from BASE_DEPARTMENT where nature not in('分包商','承包商')) or departmentcode='{0}')", departmentcode);
                    }
                    else if (type == "2")
                    {
                        if (user.RoleName.Contains("省级"))
                        {
                            pagination.conditionJson += string.Format(" and departmentcode in (select encode from BASE_DEPARTMENT where deptcode like '{0}%')", departmentcode);
                        }
                        else
                        {
                            pagination.conditionJson += string.Format(" and departmentcode like '{0}%'", departmentcode);
                        }
                    }
                    else if (type == "3")
                    {
                        pagination.conditionJson += string.Format(" and organizecode in (select encode from BASE_DEPARTMENT where nature='厂级' and deptcode like '{0}%')", departmentcode);
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(" and departmentcode like '{0}%'", departmentcode);
                    }
                }
            }
            if (!queryParam["pMode"].IsEmpty())
            {
                pagination.conditionJson += " and isepiboly='是'";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户"))
                {

                    pagination.conditionJson += string.Format("  and organizecode='{0}'", user.OrganizeCode);
                }
                else
                {
                    if (!user.RoleName.Contains("省级"))
                    {
                        pagination.conditionJson += string.Format(" and departmentcode like '{0}%' ", user.DeptCode);
                    }
                }
                if (queryParam["pMode"].ToString() == "10")
                {
                    pagination.conditionJson += string.Format("  and to_char(ENTERTIME,'yyyy-mm')='{0}'", DateTime.Now.ToString("yyyy-MM"));
                }
            }
            //查询条件
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyord = queryParam["keyword"].ToString().Trim();
                switch (condition)
                {
                    case "Account":            //账户
                        pagination.conditionJson += string.Format(" and ACCOUNT  like '%{0}%'", keyord);
                        break;
                    case "RealName":          //姓名
                        pagination.conditionJson += string.Format(" and REALNAME  like '%{0}%'", keyord);
                        break;
                    case "Mobile":          //手机
                        pagination.conditionJson += string.Format(" and MOBILE like '%{0}%'", keyord);
                        break;
                    case "DeptName":          //部门名称机
                        pagination.conditionJson += string.Format(" and DeptName like '%{0}%'", keyord);
                        break;
                    case "identifyid":          //身份证号
                        pagination.conditionJson += string.Format(" and identifyid like '%{0}%'", keyord);
                        break;
                    default:
                        break;
                }
            }
            if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
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
                    pagination.conditionJson += string.Format(" and organizecode='{0}'", deptCode);
                    //if (user.IsSystem || user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户"))
                    //{
                    //    pagination.conditionJson += string.Format(" and (departmentcode like '{0}%'", deptCode);
                    //}
                    //else
                    //{
                    //    pagination.conditionJson += string.Format(" and (departmentcode like '{0}%'", user.DeptCode);
                    //}
                    //pagination.conditionJson += string.Format(" or departmentid in(select departmentid from BASE_DEPARTMENT t where senddeptid=(select departmentid from base_department d where d.encode='{0}')))", user.DeptCode);
                }

                else if (queryParam["isOrg"].ToString() == "Department" || user.IsSystem)
                {
                    pagination.conditionJson += string.Format(" and (departmentcode like '{0}%' or nickname  in (select departmentid from base_department where encode like '{0}%'))", deptCode);
                }
                else
                {
                    pagination.conditionJson += string.Format(" and (departmentcode='{0}' or nickname in (select departmentid from base_department where encode='{0}'))", deptCode);
                    // pagination.conditionJson += string.Format(" and departmentid in(select departmentid from BASE_DEPARTMENT t where t.senddeptid=(select departmentid from base_department d where d.encode='{0}')))", user.DeptCode);
                }
                if (!string.IsNullOrWhiteSpace(deptType))
                {
                    pagination.conditionJson += string.Format("and departmentid in(select departmentid from base_department d where d.depttype='{0}' and d.encode like '{1}%')",deptType,deptCode);
                }
            }

            if (!queryParam["exportids"].IsEmpty() && queryParam["exportids"].ToString().Length > 0)
            {
                string[] ids = queryParam["exportids"].ToString().Split(',');
                string userids = "";
                for (int i = 0; i < ids.Length; i++)
                {
                    userids += "'" + ids[i] + "'" + ",";
                }

                if (userids.Length > 0)
                {
                    userids = userids.Substring(0, userids.Length - 1);
                }

                pagination.conditionJson += "  and t.USERID in (" + userids + ")";
            }


            if (!queryParam["isldap"].IsEmpty() && queryParam["isldap"].ToString() == "true")
            {
                pagination.conditionJson += "  and t.ACCOUNTTYPE =0 and t.ISAPPLICATIONLDAP= 1";
            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 用户列表（ALL）
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllTable()
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT  u.UserId ,
                                    u.EnCode ,
                                    u.Account ,
                                    u.RealName ,
                                    u.Gender ,
                                    u.Birthday ,
                                    u.Mobile ,
                                    u.Manager ,
                                    u.OrganizeId,
                                    u.DepartmentId,
                                    o.FullName AS OrganizeName ,
                                    d.FullName AS DepartmentName ,
                                    u.RoleId ,
                                    u.DutyName ,
                                    u.PostName ,
                                    u.EnabledMark ,
                                    u.CreateDate,
                                    u.Description
                            FROM    Base_User u
                                    LEFT JOIN Base_Organize o ON o.OrganizeId = u.OrganizeId
                                    LEFT JOIN Base_Department d ON d.DepartmentId = u.DepartmentId
                            WHERE   1=1");
            strSql.Append(" AND u.UserId <> 'System' AND u.EnabledMark = 1 AND u.DeleteMark=0 order by o.FullName,d.FullName,u.RealName");
            return this.BaseRepository().FindTable(strSql.ToString());
        }
        /// <summary>
        /// 根据当前用户角色获取用户所属单位信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public DepartmentEntity GetUserOrgInfo(string userId)
        {
            UserEntity userEntity = GetEntity(userId);
            DepartmentEntity dept = new DepartmentEntity();
            string roleName = GetObjectName(userEntity.UserId, 2);
            DataTable dt = null;
            string deptType = "厂级";
            if (roleName.Contains("班组") || roleName.Contains("专业") || roleName.Contains("承包商") || roleName.Contains("分包商"))
            {
                deptType = "厂级";
            }
            if (roleName.Contains("集团"))
            {
                deptType = "集团";
            }
            if (roleName.Contains("省级"))
            {
                deptType = "省级";
            }
            dt = BaseRepository().FindTable(string.Format("select t.departmentid from BASE_DEPARTMENT t where instr('{1}',t.encode)=1 and t.nature='{0}' and t.description is null order by encode desc", deptType, userEntity.DepartmentCode));
            if (dt.Rows.Count > 0)
            {
                dept = new Repository<DepartmentEntity>(DbFactory.Base()).FindEntity(dt.Rows[0][0].ToString());
            }
            else
            {
                dept = new Repository<DepartmentEntity>(DbFactory.Base()).FindEntity(userEntity.OrganizeId);

            }
            dt.Dispose();
            return dept;
        }
        public DepartmentEntity GetUserOrganizeInfo(UserInfoEntity userEntity)
        {
            DepartmentEntity dept = new DepartmentEntity();
            string roleName = GetObjectName(userEntity.UserId, 2);
            DataTable dt = null;
            string deptType = "厂级";
            if (roleName.Contains("班组") || roleName.Contains("专业") || roleName.Contains("承包商") || roleName.Contains("分包商"))
            {
                deptType = "厂级";
            }
            if (roleName.Contains("集团"))
            {
                deptType = "集团";
            }
            if (roleName.Contains("省级"))
            {
                deptType = "省级";
            }
            dt = BaseRepository().FindTable(string.Format("select t.departmentid from BASE_DEPARTMENT t where instr('{1}',t.encode)=1 and t.nature='{0}' and t.description is null order by encode desc", deptType, userEntity.DepartmentCode));
            if (dt.Rows.Count > 0)
            {
                //dept = new Repository<DepartmentEntity>(DbFactory.Base()).FindEntity(dt.Rows[0][0].ToString());
                dept = new DepartmentService().GetEntity(dt.Rows[0][0].ToString());
            }
            else
            {
                dept = new Repository<DepartmentEntity>(DbFactory.Base()).FindEntity(userEntity.OrganizeId);

            }
            dt.Dispose();
            return dept;
        }

        #region 获取所有部门
        /// <summary>
        /// 获取所有部门
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllTableByArgs(string argValue, string organizeid)
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT  u.userid  ,
                                    u.encode ,
                                    u.account ,
                                    u.realname ,
                                    u.gender ,
                                    u.birthday ,
                                    u.mobile ,
                                    u.manager ,
                                    u.organizeid,
                                    u.departmentid,
                                    o.fullname AS organizename ,
                                    d.fullname AS departmentname ,
                                    u.roleid ,
                                    u.dutyname ,
                                    u.postname ,
                                    u.enabledmark ,
                                    u.createdate,
                                    u.description
                            FROM    Base_User u
                                    LEFT JOIN Base_Organize o ON o.OrganizeId = u.OrganizeId
                                    LEFT JOIN Base_Department d ON d.DepartmentId = u.DepartmentId
                            WHERE   1=1");
            strSql.Append(" AND u.UserId <> 'System' AND u.EnabledMark = 1 AND u.DeleteMark=0 ");
            //姓名参数
            if (!argValue.IsEmpty())
            {
                strSql.AppendFormat(" u.realname like '%{0}%' ", argValue);
            }
            //当前部门
            if (!organizeid.IsEmpty())
            {
                strSql.AppendFormat(" u.organizeid ='{0}' ", organizeid);
            }
            strSql.Append(" order by o.FullName,d.FullName,u.RealName");

            return this.BaseRepository().FindTable(strSql.ToString());
        }
        #endregion

        #region 获取所有部门
        /// <summary>
        /// 获取所有部门
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllTableByArgs(string username, string deptcode, string organizeid, string sjorgid, string reqmark, string threeperson = "")
        {
            string str = "";

            //姓名参数
            if (!username.IsEmpty())
            {
                str += string.Format(" and  u.realname like '%{0}%' ", username);
            }
            string orgids = string.Empty;


            //当前部门
            if (!deptcode.IsEmpty())
            {
                if (reqmark == "children")
                {
                    str += string.Format(" and  u.departmentcode like '{0}%' ",deptcode);
                }
                else 
                {
                    string tdeptcode = "'" + deptcode.Replace(",", "','") + "'";

                    str += string.Format(" and  (u.departmentcode in ({0}) or   u.departmentid in ({0}) or u.departmentid in (select departmentid from base_department where encode in ({0})) or u.nickname in (select departmentid from base_department where encode in ({0}))) ", tdeptcode);
                }
            }
            if (!reqmark.IsEmpty())
            {
                //排查  
                if (reqmark == "1" || reqmark == "4" || reqmark == "5")
                {
                    //省级不为空，则取省级
                    if (!sjorgid.IsEmpty())
                    {
                        str += string.Format(" and  u.organizeid ='{0}' ", sjorgid);
                    }
                    else
                    {
                        str += string.Format(" and  u.organizeid ='{0}' ", organizeid);
                    }
                }
                //整改
                else if (reqmark == "2" || reqmark == "3")
                {
                    str += string.Format(" and  u.organizeid ='{0}' ", organizeid);
                }
            }
            else
            {
                //当前机构
                if (!organizeid.IsEmpty())
                {
                    orgids += "'" + organizeid + "',";
                }
                //省级机构
                if (!sjorgid.IsEmpty())
                {
                    orgids += "'" + sjorgid + "',";
                }
                if (!orgids.IsEmpty())
                {
                    orgids = orgids.Substring(0, orgids.Length - 1);

                    str += string.Format(" and  u.organizeid in  ({0}) ", orgids);
                }
            }

            if (!threeperson.IsEmpty())
            {
                str += string.Format(" and u.fourpersontype like '%{0}%'", threeperson);
            }
            string strSql = string.Format(@" select  u.userid  ,
                                    u.encode ,
                                    u.account ,
                                    u.realname ,
                                    u.gender ,
                                    u.birthday ,
                                    u.mobile ,
                                    u.manager ,
                                    u.organizeid,
                                    u.departmentid,
                                    d.encode as departmentcode,
                                    d.fullname as departmentname ,
                                    u.roleid ,
                                    u.dutyname ,
                                    u.postname ,
                                    u.enabledmark ,
                                    u.createdate,
                                    u.description,
                                    u.deletemark,
                                    u.identifyid,
                                    u.sortcode
                            from    base_user u
                                    left join base_department d on d.departmentid = u.departmentid
                            where u.ispresence='1' and u.departmentid is not null  and  u.userid <> 'system' and u.enabledmark = 1 and u.deletemark=0  {0}
                           ", str);

            strSql += " order by  d.sortcode ,u.sortcode ,userid desc";


            return this.BaseRepository().FindTable(strSql.ToString());
        }
        #endregion

        /// <summary>
        /// 用户列表（导出Excel）
        /// </summary>
        /// <returns></returns>
        public DataTable GetExportList(string condition, string keyword, string code)
        {
            var strSql = new StringBuilder();
            //strSql.Append(@"SELECT [Account]
            //              ,[RealName]
            //              ,CASE WHEN Gender=1 THEN '男' ELSE '女' END AS Gender
            //              ,[Birthday]
            //              ,[Mobile]
            //              ,[Telephone]
            //              ,u.[Email]
            //              ,[WeChat]
            //              ,[MSN]
            //              ,u.[Manager]
            //              ,o.FullName AS Organize
            //              ,d.FullName AS Department
            //              ,u.[Description]
            //              ,u.[CreateDate]
            //              ,u.[CreateUserName]
            //          FROM Base_User u
            //          INNER JOIN Base_Department d ON u.DepartmentId=d.DepartmentId
            //          INNER JOIN Base_Organize o ON u.OrganizeId=o.OrganizeId");
            var where = "";
            if (!string.IsNullOrEmpty(code))
            {
                if (code.Length <= 3)
                {
                    where += string.Format("  and o.organizeid  in (select organizeid from base_organize where encode like '{0}%')", code);
                }
                else
                {
                    where += string.Format("  and d.departmentid  in (select departmentid from base_department where encode like '{0}%')", code);
                }
            }
            if (condition == "Account")
            {
                where += string.Format("  and u.account  like '%{0}%'", keyword);
            }
            else if (condition == "RealName")
            {
                where += string.Format("  and u.realname  like '%{0}%'", keyword);
            }
            else if (condition == "Mobile")
            {
                where += string.Format("  and u.mobile  like '%{0}%'", keyword);
            }
            strSql.Append(@"SELECT u.account
                                  ,u.realname
                                  ,CASE WHEN u.gender=1 THEN '男' ELSE '女' END AS gender
                                  ,u.birthday
                                  ,u.mobile
                                  ,u.telephone
                                  ,u.email
                                  ,u.wechat
                                  ,u.msn
                                  ,u.manager
                                  ,o.FullName AS Organize
                                  ,d.FullName AS Department
                                  ,d.senddeptid
                                  ,u.description
                                  ,u.createdate
                                  ,u.createUserName
                              FROM Base_User u
                              LEFT JOIN Base_Department d ON u.DepartmentId=d.DepartmentId
                              LEFT JOIN Base_Organize o ON u.OrganizeId=o.OrganizeId where 1=1" + where);
            return this.BaseRepository().FindTable(strSql.ToString());
        }
        /// <summary>
        /// 用户实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public UserEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 通过身份证号获取用户信息
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        public UserEntity GetUserByIdCard(string idCard)
        {
            return this.BaseRepository().IQueryable(t => t.IdentifyID == idCard).FirstOrDefault();
        }
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        public UserEntity CheckLogin(string username)
        {
            //var expression = LinqExtensions.True<UserEntity>();
            //expression = expression.And(t => t.Account == username);
            //expression = expression.Or(t => t.Mobile == username);
            //expression = expression.Or(t => t.Email == username);
            // return this.BaseRepository().FindEntity(expression);
            return BaseRepository().FindList(string.Format("select *from base_user where (upper(account)='{0}' or mobile='{0}' or upper(email)='{0}' or upper(encode)='{0}')", username.ToUpper())).FirstOrDefault();
        }
        public UserInfoEntity CheckUserLogin(string username)
        {
            return DbFactory.Base().FindList<UserInfoEntity>(string.Format("select *from V_USERINFO where (upper(account)='{0}' or mobile='{0}' or upper(email)='{0}' or upper(encode)='{0}')", username.ToUpper())).FirstOrDefault();
        }
        /// <summary>
        /// 根据单位编码统计单位人员信息
        /// </summary>
        /// <param name="deptCode">单位编码</param>
        /// <returns></returns>
        public List<string> GetStatByDeptCode(string deptCode, string deptType = "0")
        {
            string orgCode = deptType == "2" || deptType == "1" ? deptCode : new OrganizeService().GetOrgCode(deptCode);
            object obj = BaseRepository().FindObject(string.Format("select encode from BASE_DEPARTMENT t where t.encode like '{0}%' and t.description='外包工程承包商'", orgCode));
            string code = "-100";
            if (obj != null)
            {
                code = BaseRepository().FindObject(string.Format("select encode from BASE_DEPARTMENT t where t.encode like '{0}%' and t.description='外包工程承包商'", orgCode)).ToString();
            }
            //string code = BaseRepository().FindObject(string.Format("select encode from BASE_DEPARTMENT t where t.encode like '{0}%' and t.description='外包工程承包商'", orgCode)).ToString();
            List<string> list = new List<string>();
            string sql = string.Format("select count(1) from base_user u where ispresence='1' and u.departmentcode like '{0}%'", deptCode);//在职人员总数
            if (deptType == "1")
            {
                sql = string.Format("select count(1) from base_user u where ispresence='1' and u.departmentcode like '{0}%' and u.departmentcode not like '{1}%'", deptCode, code);
            }
            if (deptType == "2")
            {
                sql = string.Format("select count(1) from base_user u where ispresence='1' and u.departmentcode like '{0}%'", orgCode);
            }
            decimal sum = this.BaseRepository().FindObject(sql).ToDecimal();
            list.Add(sum.ToString());

            sql = string.Format("select count(1) from base_user u where ispresence='1' and usertype='安全管理人员' and u.departmentcode like '{0}%'", deptCode);//安全管理人员人数
            if (deptType == "1")
            {
                sql = string.Format("select count(1) from base_user u where ispresence='1' and usertype='安全管理人员' and u.departmentcode like '{0}%' and u.departmentcode not like '{1}%'", deptCode, code);
            }
            if (deptType == "2")
            {
                sql = string.Format("select count(1) from base_user u where ispresence='1' and usertype='安全管理人员' and u.departmentcode like '{0}%'", orgCode);
            }
            decimal count = this.BaseRepository().FindObject(sql).ToInt();
            list.Add(count.ToString());
            decimal count1 = sum == 0 ? 0 : Math.Round(count / sum * 100, 2);
            list.Add(count1.ToString());

            sql = string.Format("select count(1) from base_user u where ispresence='1' and u.isspecial='是' and u.departmentcode like '{0}%'", deptCode);//特种作业人员人数

            if (deptType == "1")
            {
                sql = string.Format("select count(1) from base_user u where ispresence='1' and u.isspecial='是' and u.departmentcode like '{0}%' and u.departmentcode not like '{1}%'", deptCode, code);
            }
            if (deptType == "2")
            {
                sql = string.Format("select count(1) from base_user u where ispresence='1' and u.isspecial='是' and u.departmentcode like '{0}%'", orgCode);
            }

            count = this.BaseRepository().FindObject(sql).ToInt();
            list.Add(count.ToString());
            count1 = sum == 0 ? 0 : Math.Round(count / sum * 100, 2);
            list.Add(count1.ToString());

            sql = string.Format("select count(1) from base_user u where ispresence='1' and u.isspecialequ='是' and u.departmentcode like '{0}%'", deptCode);//特种设备作业人员人数

            if (deptType == "1")
            {
                sql = string.Format("select count(1) from base_user u where ispresence='1' and u.isspecialequ='是' and u.departmentcode like '{0}%' and u.departmentcode not like '{1}%'", deptCode, code);
            }
            if (deptType == "2")
            {
                sql = string.Format("select count(1) from base_user u where ispresence='1' and u.isspecialequ='是' and u.departmentcode like '{0}%'", orgCode);
            }

            count = this.BaseRepository().FindObject(sql).ToInt();
            list.Add(count.ToString());
            count1 = sum == 0 ? 0 : Math.Round(count / sum * 100, 2);
            list.Add(count1.ToString());
            //sql = string.Format("select count(1) from base_user u where u.ispresence='0' and u.departmentcode like '{0}%'", deptCode);//离场人员人数
            //count = this.BaseRepository().FindObject(sql).ToInt();
            //list.Add(count.ToString());
            //count1 = sum == 0 ? 0 : Math.Round(count / sum * 100, 2);
            //list.Add(count1.ToString());
            sql = string.Format("select count(1) from base_user u where ispresence='1' and u.isblack=1 and u.departmentcode like '{0}%'", deptCode);//黑名单人员人数
            if (deptType == "1")
            {
                sql = string.Format("select count(1) from base_user u where ispresence='1' and u.isblack=1 and u.departmentcode like '{0}%' and u.departmentcode not like '{1}%'", deptCode, code);
            }
            if (deptType == "2")
            {
                sql = string.Format("select count(1) from base_user u where ispresence='1' and u.isblack=1 and u.departmentcode like '{0}%'", orgCode);
            }

            count = this.BaseRepository().FindObject(sql).ToInt();
            list.Add(count.ToString());
            count1 = sum == 0 ? 0 : Math.Round(count / sum * 100, 2);
            list.Add(count1.ToString());
            return list;
        }
        /// <summary>
        /// 人员统计（集团）
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="newCode"></param>
        /// <param name="deptType"></param>
        /// <returns></returns>
        public List<string> GetStatByDeptCodeForGroup(string deptCode, string newCode, string deptType = "0")
        {
            string orgCode = deptCode;
            string code = "10000";
            string code1 = "10000";
            List<string> list = new List<string>();
            string sql = string.Format("select count(1) from base_user u where  ispresence='1' and account!='System' and u.organizecode='{0}'", orgCode);//在职人员总数
            if (deptType == "0")
            {
                DataTable dt = BaseRepository().FindTable(string.Format("select encode,deptcode from BASE_DEPARTMENT t where t.encode like '{0}%' and t.description='外包工程承包商'", orgCode));
                if (dt.Rows.Count > 0)
                {
                    if (!string.IsNullOrWhiteSpace(dt.Rows[0][0].ToString()))
                    {
                        code = dt.Rows[0][0].ToString();
                    }
                    if (!string.IsNullOrWhiteSpace(dt.Rows[0][1].ToString()))
                    {
                        code1 = dt.Rows[0][1].ToString();
                    }
                }

            }
            if (deptType == "1")
            {
                sql = string.Format("select count(1) from base_user u where ispresence='1' and account!='System' and u.organizecode='{0}'", deptCode);
            }
            if (deptType == "2")
            {
                sql = string.Format("select count(1) from base_user u where ispresence='1' and account!='System' and u.departmentcode in(select encode from BASE_DEPARTMENT where  deptcode like '{0}%')", newCode);
            }
            if (deptType == "3")
            {
                sql = string.Format("select count(1) from base_user u where ispresence='1' and account!='System' and u.organizecode in(select encode from BASE_DEPARTMENT where nature='厂级' and deptcode like '{0}%')", newCode);
            }
            decimal sum = this.BaseRepository().FindObject(sql).ToDecimal();
            list.Add(sum.ToString());//在厂总人数

            sql = string.Format("select count(1) from base_user u where ispresence='1' and usertype='安全管理人员' and u.organizecode ='{0}' and account!='System' and u.departmentcode not like '{1}%' and u.departmentcode not like '{2}%'", deptCode, code, code1);//安全管理人员人数
            if (deptType == "1")
            {
                sql = string.Format("select count(1) from base_user u where ispresence='1' and usertype='安全管理人员' and account!='System' and u.organizecode like '{0}%' and u.departmentcode not like '{1}%' and u.departmentcode not like '{2}%'", deptCode, code, code1);
            }
            if (deptType == "2")
            {
                sql = string.Format("select count(1) from base_user u where ispresence='1' and usertype='安全管理人员' and account!='System' and u.departmentcode in(select encode from BASE_DEPARTMENT where nature not in('承包商','分包商') and deptcode like '{0}%')", newCode);
            }
            if (deptType == "3")
            {
                sql = string.Format("select count(1) from base_user u where ispresence='1' and usertype='安全管理人员' and account!='System' and u.organizecode in(select encode from BASE_DEPARTMENT where nature='厂级' and deptcode like '{0}%') and departmentcode not in(select encode from BASE_DEPARTMENT where nature='承包商' or nature='分包商' )", newCode);
            }
            decimal count = this.BaseRepository().FindObject(sql).ToInt();
            list.Add(count.ToString());//安全管理人员人数
            decimal count1 = sum == 0 ? 0 : Math.Round(count / sum * 100, 2);
            list.Add(count1.ToString());//占百分比

            sql = string.Format("select count(1) from base_user u where ispresence='1' and u.isspecial='是' and account!='System' and u.organizecode='{0}'", deptCode);//特种作业人员人数

            if (deptType == "1")
            {
                sql = string.Format("select count(1) from base_user u where ispresence='1' and u.isspecial='是' and account!='System' and u.organizecode like '{0}%'", deptCode);
            }
            if (deptType == "2")
            {
                sql = string.Format("select count(1) from base_user u where ispresence='1' and u.isspecial='是' and account!='System' and  u.organizecode in(select encode from BASE_DEPARTMENT where  deptcode like '{0}%')", orgCode, newCode);
            }
            if (deptType == "3")
            {
                sql = string.Format("select count(1) from base_user u where ispresence='1' and u.isspecial='是' and account!='System' and u.organizecode='{0}' and u.organizecode in(select encode from BASE_DEPARTMENT where nature='厂级' and deptcode like '{0}%')", orgCode, newCode);
            }
            count = this.BaseRepository().FindObject(sql).ToInt();
            list.Add(count.ToString());
            count1 = sum == 0 ? 0 : Math.Round(count / sum * 100, 2);
            list.Add(count1.ToString());

            sql = string.Format("select count(1) from base_user u where ispresence='1' and u.isspecialequ='是' and u.organizecode='{0}' and account!='System'", deptCode);//特种设备作业人员人数

            if (deptType == "1")
            {
                sql = string.Format("select count(1) from base_user u where ispresence='1' and u.isspecialequ='是' and account!='System' and u.departmentcode like '{0}%'", deptCode);
            }
            if (deptType == "2")
            {
                sql = string.Format("select count(1) from base_user u where ispresence='1' and u.isspecialequ='是' and account!='System' and u.organizecode in(select encode from BASE_DEPARTMENT where  deptcode like '{0}%')", orgCode);
            }
            if (deptType == "3")
            {
                sql = string.Format("select count(1) from base_user u where ispresence='1' and u.isspecialequ='是' and account!='System' and u.departmentcode like '{0}%' and u.organizecode in(select encode from BASE_DEPARTMENT where nature='厂级' and deptcode like '{0}%')", orgCode);
            }
            count = this.BaseRepository().FindObject(sql).ToInt();
            list.Add(count.ToString());
            count1 = sum == 0 ? 0 : Math.Round(count / sum * 100, 2);
            list.Add(count1.ToString());
            sql = string.Format("select count(1) from base_user u where u.isblack=1 and u.organizecode='{0}'", deptCode);//黑名单人员人数
            if (deptType == "1")
            {
                sql = string.Format("select count(1) from base_user u where u.isblack=1 and u.departmentcode like '{0}%'", deptCode);
            }
            if (deptType == "2")
            {
                sql = string.Format("select count(1) from base_user u where u.isblack=1  and u.organizecode in(select encode from BASE_DEPARTMENT where  deptcode like '{0}%')", orgCode, newCode);
            }
            if (deptType == "3")
            {
                sql = string.Format("select count(1) from base_user u where u.isblack=1 and u.departmentcode like '{0}%' and u.departmentcode like '{0}%' and u.organizecode in(select encode from BASE_DEPARTMENT where nature='厂级' and deptcode like '{0}%')", orgCode, newCode);
            }
            count = this.BaseRepository().FindObject(sql).ToInt();
            list.Add(count.ToString());
            count1 = sum == 0 ? 0 : Math.Round(count / sum * 100, 2);
            list.Add(count1.ToString());
            return list;
        }

        public IEnumerable<UserEntity> GetUserList()
        {
            string sql = string.Format("select t.* from base_user t");
            return this.BaseRepository().FindList(sql);
        }

        public int ExcuteUser(string sql, DbParameter[] dbparams)
        {
            return this.BaseRepository().ExecuteBySql(sql, dbparams);
        }


        public int ExcuteBySql(string sql)
        {
            return this.BaseRepository().ExecuteBySql(sql);
        }
        /// <summary>
        /// 根据角色和部门获取用户账号和姓名
        /// </summary>
        /// <param name="orgid">厂级Id</param>
        /// <param name="deptid">部门Id</param>
        /// <param name="rolename">角色名称</param>
        /// <returns></returns>
        public DataTable GetUserAccountByRoleAndDept(string orgid, string deptid, string rolename)
        {
            var strWhere = string.Empty;
            var strWhere1 = string.Empty;
            var deptList = deptid.Split(',');
            strWhere = "(";
            if (deptList.Length > 0)
            {
                for (int i = 0; i < deptList.Length; i++)
                {
                    strWhere += string.Format(" u.departmentid='{0}' or", deptList[i].ToString());
                }
            }
            strWhere = strWhere.Substring(0, strWhere.Length - 2);
            strWhere += ")";
            var rolearr = rolename.Split(',');
            strWhere1 = "(";
            if (rolearr.Length > 0)
            {
                for (int i = 0; i < rolearr.Length; i++)
                {
                    strWhere1 += string.Format(" u.rolename like'%{0}%' or", rolearr[i].ToString());
                }
            }
            strWhere1 = strWhere1.Substring(0, strWhere1.Length - 2);
            strWhere1 += ")";
            string sql = string.Format(@"select  wm_concat(u.account) account,wm_concat(u.realname) realname from base_user u where  u.organizeid='{0}' and {1} and {2} ", orgid, strWhere, strWhere1);
            return this.BaseRepository().FindTable(sql);
        }
        #endregion

        #region 获取用户详情用于消息提醒
        /// <summary>
        /// 获取用户详情用于消息提醒
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="deptcode"></param>
        /// <param name="rolename"></param>
        /// <returns></returns>
        public DataTable GetUserByDeptCodeAndRoleName(string userid, string deptcode, string rolename)
        {
            var strWhere = " and  1=1 ";

            if (!string.IsNullOrEmpty(userid))
            {
                string tempuserid = "'" + userid.Replace(",", "','") + "'";
                strWhere += string.Format(" and a.userid in ({0})", tempuserid);
            }
            if (!string.IsNullOrEmpty(deptcode))
            {
                string tempdeptcode = "'" + deptcode.Replace(",", "','") + "'";
                strWhere += string.Format(" and b.encode in ({0})", tempdeptcode);
            }
            if (!string.IsNullOrEmpty(rolename))
            {
                var rolearr = rolename.Split(',');
                strWhere += " and (";
                if (rolearr.Length > 0)
                {
                    for (int i = 0; i < rolearr.Length; i++)
                    {
                        strWhere += string.Format(" a.rolename like'%{0}%' or", rolearr[i].ToString());
                    }
                }
                strWhere = strWhere.Substring(0, strWhere.Length - 2);
                strWhere += ")";
            }
            string sql = string.Format(@"select  wm_concat(a.account) account,wm_concat(a.realname) realname from base_user a ,base_department b 
                                      where  a.departmentid = b.departmentid {0} ", strWhere);
            return this.BaseRepository().FindTable(sql);
        }
        #endregion

        #region 验证数据
        /// <summary>
        /// 账户不能重复
        /// </summary>
        /// <param name="account">账户值</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistAccount(string account,string encode,string mobile, string keyValue)
        {
            var expression = LinqExtensions.True<UserEntity>();
            if (!string.IsNullOrWhiteSpace(account))
            {
                expression = expression.And(t => t.Account.ToUpper() == account.ToUpper() || t.EnCode.ToUpper() == account.ToUpper() || t.Mobile.ToUpper() == account.ToUpper());
            }
            if (!string.IsNullOrWhiteSpace(encode))
            {
                expression = expression.Or(t => t.Account.ToUpper() == encode.ToUpper() || t.EnCode.ToUpper() == encode.ToUpper() || t.Mobile.ToUpper() == encode.ToUpper());
            }
            if (!string.IsNullOrWhiteSpace(mobile))
            {
                expression = expression.Or(t => t.Account.ToUpper() == mobile.ToUpper() || t.EnCode.ToUpper() == mobile.ToUpper() || t.Mobile.ToUpper() == mobile.ToUpper());
            }
           
            if (!string.IsNullOrEmpty(keyValue))
            {
                expression = expression.And(t => t.UserId != keyValue);
            }
            return this.BaseRepository().IQueryable(expression).Count() == 0 ? true : false;
        }
        /// <summary>
        /// 身份证不能重复
        /// </summary>
        /// <param name="IdentifyID">身份证号</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistIdentifyID(string IdentifyID, string keyValue)
        {
            if (string.IsNullOrEmpty(IdentifyID))
            {
                return true;
            }
            var expression = LinqExtensions.True<UserEntity>();
            expression = expression.And(t => t.IdentifyID == IdentifyID);
            if (!string.IsNullOrEmpty(keyValue))
            {
                expression = expression.And(t => t.UserId != keyValue);
            }
            return this.BaseRepository().IQueryable(expression).Count() == 0 ? true : false;
        }

        /// <summary>
        /// 校验手机号是否重复，没重复返回true
        /// </summary>
        /// <param name="mobile">要校验的手机号</param>
        /// <param name="keyValue">主键</param>
        /// <returns>true‘ false</returns>
        public bool ExistMoblie(string mobile, string keyValue)
        {
            if (string.IsNullOrEmpty(mobile))
            {
                return true;
            }
            var expression = LinqExtensions.True<UserEntity>();
            expression = expression.And(t => t.Mobile == mobile);
            if (!string.IsNullOrEmpty(keyValue))
            {
                expression = expression.And(t => t.UserId != keyValue);
            }
            return this.BaseRepository().IQueryable(expression).Count() == 0 ? true : false;
        }
        #endregion
        #region sha256加密
        /// <summary>
        /// sha256
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string sha256(string data)
        {
            //string digest = SHA256Encrypt(data);
            //byte[] b = System.Text.Encoding.Default.GetBytes(digest);
            //return Convert.ToBase64String(b);


            byte[] bytes = Encoding.UTF8.GetBytes(data);
            byte[] hash = SHA256Managed.Create().ComputeHash(bytes);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("X2"));
            }
            return builder.ToString();
        }

        public string SHA256Encrypt(string strIN)
        {
            //string strIN = getstrIN(strIN);
            byte[] tmpByte;
            SHA256 sha256 = new SHA256Managed();

            tmpByte = sha256.ComputeHash(GetKeyByteArray(strIN));
            sha256.Clear();

            return GetStringValue(tmpByte);

        }

        private string GetStringValue(byte[] Byte)
        {
            string tmpString = "";
            ASCIIEncoding Asc = new ASCIIEncoding();
            tmpString = Asc.GetString(Byte);
            return tmpString;
        }

        private byte[] GetKeyByteArray(string strKey)
        {
            ASCIIEncoding Asc = new ASCIIEncoding();

            int tmpStrLen = strKey.Length;
            byte[] tmpByte = new byte[tmpStrLen - 1];

            tmpByte = Asc.GetBytes(strKey);

            return tmpByte;

        }


        #endregion
        #region 提交数据
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存用户表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="userEntity">用户实体</param>
        /// <returns></returns>
        public string SaveForm(string keyValue, UserEntity userEntity, int mode = 0)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            try
            {
                #region 基本信息
                userEntity.UserId = keyValue;
                if (!string.IsNullOrEmpty(userEntity.DepartmentId))
                {
                    DepartmentEntity depart = new DepartmentService().GetEntity(userEntity.DepartmentId);
                    if (depart != null)
                    {
                        //重新处理承包商级用户关联的部门信息
                        if (depart.Nature == "承包商")
                        {
                            string sql = string.Format("select d.departmentid,d.encode from BASE_DEPARTMENT d where d.parentid=(select t.departmentid from BASE_DEPARTMENT t where t.organizeid='{0}' and t.description='外包工程承包商') and instr('{1}',d.encode)>0", userEntity.OrganizeId, depart.EnCode);
                            DataTable dtDept = BaseRepository().FindTable(sql);
                            if (dtDept.Rows.Count > 0)
                            {
                                userEntity.NickName = depart.DepartmentId; //存储承包商或下属部门的Id
                                //以下强制复制为承包商节点部门信息
                                userEntity.DepartmentId = dtDept.Rows[0][0].ToString();
                                userEntity.DepartmentCode = dtDept.Rows[0][1].ToString();
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(keyValue))
                {
                    UserEntity user = this.BaseRepository().FindEntity(keyValue);

                    if (user == null)
                    {
                        if (ExistAccount(userEntity.Account,userEntity.EnCode,userEntity.Mobile, keyValue))
                        {
                            if (ExistIdentifyID(userEntity.IdentifyID, ""))
                            {
                                userEntity.Create();
                                userEntity.Secretkey = string.IsNullOrEmpty(userEntity.Secretkey) ? Md5Helper.MD5(CommonHelper.CreateNo(), 16).ToLower() : userEntity.Secretkey;
                                if (mode == 0)
                                {
                                    userEntity.Password = userEntity.Password.Replace("&nbsp;", "");
                                    userEntity.Password = string.IsNullOrEmpty(userEntity.Password) ? "Abc123456" : userEntity.Password;
                                    userEntity.NewPassword = DESEncrypt.Encrypt(userEntity.Password, userEntity.Secretkey);
                                    userEntity.Password = Md5Helper.MD5(DESEncrypt.Encrypt(Md5Helper.MD5(userEntity.Password, 32).ToLower(), userEntity.Secretkey).ToLower(), 32).ToLower();
                                }
                                db.Insert(userEntity);
                            }
                        }
                        else
                        {
                            return "";
                        }
                    }
                    else
                    {
                        userEntity.Modify(keyValue);
                        userEntity.Password = null;
                        db.Update(userEntity);
                    }
                }
                else
                {
                    userEntity.Create();
                    keyValue = userEntity.UserId;
                    if (ExistAccount(userEntity.Account,userEntity.EnCode,userEntity.Mobile, keyValue))
                    {
                        if (ExistIdentifyID(userEntity.IdentifyID, ""))
                        {
                            userEntity.Secretkey = string.IsNullOrEmpty(userEntity.Secretkey) ? Md5Helper.MD5(CommonHelper.CreateNo(), 16).ToLower() : userEntity.Secretkey;
                            if (mode == 0)
                            {
                                userEntity.Password = string.IsNullOrEmpty(userEntity.Password) ? "Abc123456" : userEntity.Password;
                                userEntity.Password = userEntity.Password.Replace("&nbsp;", "");
                                userEntity.NewPassword = DESEncrypt.Encrypt(userEntity.Password, userEntity.Secretkey);
                                userEntity.Password = Md5Helper.MD5(DESEncrypt.Encrypt(Md5Helper.MD5(userEntity.Password, 32).ToLower(), userEntity.Secretkey).ToLower(), 32).ToLower();
                            }
                            db.Insert(userEntity);
                        }
                    }
                    else
                    {
                        return "";
                    }

                }
                string logPath = new DataItemDetailService().GetItemValue("imgPath") + "\\logs\\";
                var task = Task.Factory.StartNew(() =>
                {
                    DepartmentEntity dept = new DepartmentService().GetEntity(userEntity.OrganizeId);
                    if (dept != null)
                    {
                        if (dept.IsTrain == 1)
                        {
                            object obj = BaseRepository().FindObject(string.Format("select px_deptid from xss_dept where deptid='{0}'", userEntity.DepartmentId));
                            if (obj != null)
                            {
                                List<object> list = new List<object>();
                                list.Add(new
                                {
                                    Id = userEntity.UserId,
                                    departid = obj.ToString(),
                                    userName = userEntity.RealName,
                                    userAccount = userEntity.Account,
                                    IdCard = userEntity.IdentifyID,
                                    role = "0",
                                    pwd = "Abc123456",
                                    userkind = "一般人员",
                                    telephone = userEntity.Mobile
                                });
                                if (userEntity.IsTrainAdmin == 1)
                                {
                                    string str = userEntity.IdentifyID.Substring(0, userEntity.IdentifyID.Length - 1);
                                    string last = userEntity.IdentifyID.Substring(userEntity.IdentifyID.Length - 1);
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
                                        departid = obj.ToString(),
                                        userName = userEntity.RealName,
                                        userAccount = userEntity.Account + "gly",
                                        IdCard = str + last,
                                        role = "1",
                                        pwd = "Abc123456",
                                        userkind = "一般人员",
                                        telephone = userEntity.Mobile
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
                                string result = Encoding.UTF8.GetString(bytes);
                                //将同步结果写入日志文件
                                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                                System.IO.File.AppendAllText(logPath + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " >同步信息：" + Newtonsoft.Json.JsonConvert.SerializeObject(userEntity) + "，执行动作：同步用户到培训平台,返回结果：" + result + "\r\n");
                            }

                        }
                    }
                });
                #endregion
                #region 默认添加 角色、岗位、职位
                db.Delete<UserRelationEntity>(t => t.IsDefault == 1 && t.UserId == userEntity.UserId && t.Category!=4);
                List<UserRelationEntity> userRelationEntitys = new List<UserRelationEntity>();
                var currUser = OperatorProvider.Provider.Current();
                string uid = currUser == null ? "" : currUser.UserId;
                string uname = currUser == null ? "" : currUser.UserName;
                //用户
                userRelationEntitys.Add(new UserRelationEntity
                {
                    Category = 6,
                    UserRelationId = Guid.NewGuid().ToString(),
                    UserId = userEntity.UserId,
                    ObjectId = userEntity.UserId,
                    CreateDate = DateTime.Now,
                    CreateUserId = uid,
                    CreateUserName = uname,
                    IsDefault = 1,
                });
                //角色
                string[] arr = userEntity.RoleId.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in arr)
                {
                    userRelationEntitys.Add(new UserRelationEntity
                    {
                        Category = 2,
                        UserRelationId = Guid.NewGuid().ToString(),
                        UserId = userEntity.UserId,
                        ObjectId = item,
                        CreateDate = DateTime.Now,
                        CreateUserId = uid,
                        CreateUserName = uname,
                        IsDefault = 1,
                    });
                }
                //岗位
                if (!string.IsNullOrEmpty(userEntity.DutyId))
                {
                    userRelationEntitys.Add(new UserRelationEntity
                    {
                        Category = 3,
                        UserRelationId = Guid.NewGuid().ToString(),
                        UserId = userEntity.UserId,
                        ObjectId = userEntity.DutyId,
                        CreateDate = DateTime.Now,
                        CreateUserId = uid,
                        CreateUserName = uname,
                        IsDefault = 1,
                    });
                }
               
                //职位
                if (!string.IsNullOrWhiteSpace(userEntity.PostId))
                {
                    db.Delete<UserRelationEntity>(t => t.IsDefault == 1 && t.UserId == userEntity.UserId && t.Category == 4);
                    userRelationEntitys.Add(new UserRelationEntity
                    {
                        Category = 4,
                        UserRelationId = Guid.NewGuid().ToString(),
                        UserId = userEntity.UserId,
                        ObjectId = userEntity.PostId,
                        CreateDate = DateTime.Now,
                        CreateUserId = uid,
                        CreateUserName = uname,
                        IsDefault = 1,
                    });
                }
                db.Insert(userRelationEntitys);
                #endregion

                db.Commit();

                return userEntity.UserId;
            }
            catch (Exception)
            {
                db.Rollback();
                return "";
            }
        }
        /// <summary>
        /// 修改用户登录密码
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="Password">新密码（MD5 小写）</param>
        public bool RevisePassword(string keyValue, string Password)
        {
            try
            {
                UserEntity userEntity = new UserEntity();
                userEntity.UserId = keyValue;
                userEntity.Secretkey = Md5Helper.MD5(CommonHelper.CreateNo(), 16).ToLower();
                userEntity.NewPassword = DESEncrypt.Encrypt(Password, userEntity.Secretkey);
                // Password = Md5Helper.MD5(Password, 32).ToLower();
                userEntity.Password = Md5Helper.MD5(DESEncrypt.Encrypt(Password, userEntity.Secretkey).ToLower(), 32).ToLower();
                userEntity.IsBz = 1;
                if (userEntity.LastVisit==null)
                {
                    userEntity.LastVisit = DateTime.Now;
                }
                userEntity.PwdErrorCount = 0;
                return this.BaseRepository().Update(userEntity) > 0 ? true : false;
            }
            catch
            {
                return false;
            }
           
        }
        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="State">状态：1-启动；0-禁用</param>
        public void UpdateState(string keyValue, int State)
        {
            UserEntity userEntity = new UserEntity();
            userEntity.Modify(keyValue);
            userEntity.PwdErrorCount = 0;
            userEntity.EnabledMark = State;
            this.BaseRepository().Update(userEntity);
        }
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="userEntity">实体对象</param>
        public void UpdateEntity(UserEntity userEntity)
        {
            this.BaseRepository().Update(userEntity);
        }
        #endregion

        #region 通过角色获取集合
        /// <summary>
        /// 通过角色获取集合
        /// </summary>
        /// <param name="category"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public IList<UserEntity> GetUserListByRole(string deptmentid, string roleCode, string orgid)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string sql = string.Format(@"select * from base_user a
                                         left join base_userrelation b on a.userid = b.userid
                                         left join base_role c on b.objectid =c.roleid
                                         where 1=1 ");

            if (string.IsNullOrEmpty(orgid))
            {
                orgid = user.OrganizeId;
                sql += " and (a.isBlack=0 or a.isblack is null) and a.isPresence='1'";
            }
            if (!orgid.IsEmpty())
            {
                sql += string.Format(" and a.organizeid ='{0}' ", orgid);
            }
            if (!deptmentid.IsEmpty())
            {
                sql += string.Format("  and  a.departmentcode = '{0}' ", deptmentid);
            }
            if (!roleCode.IsEmpty())
            {
                sql += string.Format(" and c.encode in ({0})", roleCode);
            }

            var list = this.BaseRepository().FindList(sql).ToList();

            return list;
        }

        #endregion

        #region 通过当前用户获取上级部门的安全管理员
        /// <summary>
        /// 通过当前用户获取上级部门的安全管理员
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public IList<UserEntity> GetParentUserByCurrent(string userID, string userRoleCode)
        {
            string sql = string.Format(@"
                                        select a.* from base_user a
                                        left join (
                                        select a.userid from base_userrelation a 
                                        left join base_role b on a.objectid = b.roleid  where  b.encode in ({1})
                                        ) b  on a.userid = b.userid
                                        where  b.userid is not null and a.departmentid in (select b.parentid  from base_user a 
                                        left join base_department b on a.departmentid = b.departmentid where a.userid ='{0}') ", userID, userRoleCode);

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
        public IList<UserEntity> GetUserListByDeptCode(string deptCode, string roleCode, bool isSplit, string orgid)
        {
            string sql = @"select distinct a.* from base_user a
                                    left join base_department b on a.departmentcode = b.encode
                                    left join base_userrelation c on a.userid = c.userid
                                    left join base_role d on  c.objectid = d.roleid
                                    where 1=1  ";

            string str = " ";
            if (!string.IsNullOrEmpty(orgid))
            {
                str += string.Format(" and a.organizeid ='{0}' ", orgid);
            }
            if (!string.IsNullOrEmpty(deptCode))
            {
                str += string.Format("  and  a.departmentcode in ({0})", deptCode);
            }
            if (!string.IsNullOrEmpty(roleCode))
            {
                if (isSplit)
                {
                    int count = roleCode.Split(',').Length;

                    str += string.Format(@" and a.userid in (
                                        select userid from (
                                           select userid,objectid, row_number() over(partition by userid order by createdate desc ) rn from base_userrelation  where objectid in (  
                                            select roleid from base_role where encode in ({1})  
                                            ) 
                                         ) a where  RN ={0})", count, roleCode);
                }
                else
                {
                    str += string.Format("  and  d.encode in ({0}) ", roleCode);
                }
            }

            sql += str;

            var list = this.BaseRepository().FindList(sql).ToList();

            return list;
        }
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public IList<UserEntity> GetUserListByDeptId(string deptId, string roleId, bool isSplit, string orgid)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            string sql = @"select distinct a.* from base_user a
                                    left join base_department b on a.departmentcode = b.encode
                                    left join base_userrelation c on a.userid = c.userid
                                    left join base_role d on  c.objectid = d.roleid
                                    where 1=1  ";

            string str = " ";
            if (string.IsNullOrEmpty(orgid))
            {
                orgid = user.OrganizeId;
                str += " and (a.isBlack=0 or a.isblack is null) and a.isPresence='1'";
            }
            if (!string.IsNullOrEmpty(orgid))
            {
                str += string.Format(" and a.organizeid ='{0}' ", orgid);
            }
            if (!string.IsNullOrEmpty(deptId))
            {
                str += string.Format("  and  a.departmentid in ({0})", deptId);
            }
            if (!string.IsNullOrEmpty(roleId))
            {
                str += string.Format("  and  d.roleid in ({0}) ", roleId);
            }

            sql += str;

            var list = this.BaseRepository().FindList(sql).ToList();

            return list;
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public IList<UserEntity> GetUserListByRoleName(string deptId, string roleName, bool isSplit, string orgid)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string sql = @"select a.* from base_user a
                                    where 1=1  ";
            string str = " ";
            if (string.IsNullOrEmpty(orgid))
            {
                orgid = user.OrganizeId;
                str += " and (a.isBlack=0 or a.isblack is null) and a.isPresence='1'";
            }
            if (!string.IsNullOrEmpty(orgid))
            {
                str += string.Format(" and a.organizeid ='{0}' ", orgid);
            }

            if (!string.IsNullOrEmpty(deptId))
            {
                str += string.Format("  and  a.departmentid in ({0})", deptId);
            }
            if (!string.IsNullOrEmpty(roleName))
            {
                str += string.Format("  and (");
                foreach (string roleStr in roleName.Replace("'", "").Split(','))
                {
                    str += string.Format("  a.rolename like '%{0}%'  or", roleStr);
                }
                str = str.Substring(0, str.Length - 2);
                str += string.Format(")");
                //str += string.Format("  and  a.rolename in ({0}) ", roleName); //rolename 负责人,专工  roleName '普通级用户','专工'
            }

            sql += str;

            var list = this.BaseRepository().FindList(sql).ToList();

            return list;
        }
        #endregion

        /// <summary>
        /// 设置用户黑名单状态
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="status">状态值（0:不是黑名单，1：是黑名单）</param>
        /// <returns></returns>
        public int SetBlack(string userId, int status = 0)
        {
            return this.BaseRepository().ExecuteBySql(string.Format("update base_user set isblack={0} where userid in('{1}')", status, userId.Replace(",", "','")));
        }
        /// <summary>
        /// 人员离场
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="leaveTime">离场时间</param>
        /// <returns></returns>
        public int SetLeave(string userId, string leaveTime, string DepartureReason)
        {
            leaveTime = "to_date('" + leaveTime + " 00:00:00','yyyy-mm-dd hh24:mi:ss')";
            string sql = "update base_user set IsPresence=0,DepartureTime={0},DepartureReason='{2}' where userid in('{1}')";
            return this.BaseRepository().ExecuteBySql(string.Format(sql, leaveTime, userId.Replace(",", "','"), DepartureReason));
        }

        /// <summary>
        /// 修改人员离场审批状态
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="isLeaving">是否离场审批中</param>
        /// <returns></returns>
        public int UpdateUserLeaveState(string userids,string isLeaving)
        {
            string sql = string.Format("update base_user set IsLeaving={0} where userid in ('{1}')", isLeaving, userids.Replace(",", "','"));
            return this.BaseRepository().ExecuteBySql(sql);
        }

        /// <summary>
        /// 人员转岗
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="newDeptId">新部门Id</param>
        /// <param name="newPostId">新的岗位Id</param>
        /// <param name="newPostName">新的岗位名称</param>
        /// <param name="newDutyId">新的职务Id</param>
        /// <param name="newDutyName">新的职务名称</param>
        /// <param name="time"></param>
        /// <returns></returns>
        public int LeavePost(string userId, string newDeptId, string newPostId, string newPostName, string newDutyId, string newDutyName, string time)
        {
            UserInfoEntity user = DbFactory.Base().FindEntity<UserInfoEntity>(userId);
            if (user != null)
            {
                time = "to_date('" + time + "','yyyy-mm-dd')";
                if (this.BaseRepository().ExecuteBySql(string.Format("update base_user set IsPresence=0,DepartureTime={0},departmentid='{1}',postid='{2}',postname='{3}',dutyid='{4}',dutyname='{5}' where userid in('{6}')", time, newDeptId, newDutyId, newDutyName, newPostId, newPostName, userId.Replace(",", "','"))) > 0)
                {
                    BaseRepository().ExecuteBySql(string.Format("insert into bis_workrecord(id,createdate,createuserid,deptcode,username,enterdate,deptname,leavetime,userid,deptid,postname) values('{0}',{1},'{2}','{3}','{4}',to_date('{5}','yyyy-mm-dd'),'{6}',{7},'{8}','{9}','{10}')", Guid.NewGuid().ToString(), "sysdate", "System", user.DepartmentCode, user.RealName, user.EnterTime.Value.ToString("yyyy-MM-dd"), user.DeptName, time, userId, user.DepartmentId, user.DutyName));
                }
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 同步外包工程人员到双控平台用户
        /// </summary>
        ///    
        /// <param name="projectId">工程Id</param>
        /// <param name="deptId">外包单位Id</param>
        /// <returns></returns>
        public bool SyncUsers(string projectId, string deptId)
        {
            var res = this.BaseRepository().BeginTrans();
            try
            {
                string pwd = "Abc123456";
                string key = Md5Helper.MD5(CommonHelper.CreateNo(), 16).ToLower();
                pwd = Md5Helper.MD5(DESEncrypt.Encrypt(Md5Helper.MD5(pwd, 32).ToLower(), key).ToLower(), 32).ToLower();

                string sql = string.Format(@"insert into base_user(userid,encode,realname,headicon,birthday,mobile,telephone,email,oicq,wechat,msn,organizeid,dutyid,dutyname,postid,postname,gender,isspecialequ,isspecial,projectid,nation,native,usertype,isepiboly,degreesid,degrees,identifyid,organizecode,createuserdeptcode,createuserorgcode,createdate,createuserid,departmentid,departmentcode,ISPRESENCE,account,enabledmark,password,secretkey)
 select p.id,p.encode,p.realname,p.headicon,p.birthday,p.mobile,p.telephone,p.email,p.oicq,p.wechat,p.msn,p.organizeid,p.dutyid,p.dutyname,p.postid,p.postname,p.gender,p.isspecialequ,p.isspecial,p.OUTENGINEERID,p.nation,p.native,p.usertype,p.isepiboly,p.degreesid,p.degrees,p.identifyid,p.organizecode,p.createuserdeptcode,p.createuserorgcode,p.createdate,p.createuserid,p.outprojectid,p.outprojectcode,'1',identifyid as account,1,'{2}','{3}' from EPG_APTITUDEINVESTIGATEPEOPLE p where p.outengineerid='{0}' and p.outprojectid='{1}'", projectId, deptId, pwd, key);
                res.ExecuteBySql(sql);
                sql = string.Format("select DUTYID,organizeid from EPG_APTITUDEINVESTIGATEPEOPLE p where p.outengineerid='{0}' and p.outprojectid='{1}'", projectId, deptId);
                DataTable dt = BaseRepository().FindTable(sql);
                foreach (DataRow dr in dt.Rows)
                {
                    sql = string.Format("select t.roleids from BASE_ROLE t where t.organizeid='{0}' and t.roleid='{1}'", dr[1].ToString(), dr[0].ToString());
                    object obj = BaseRepository().FindObject(sql);
                    if (obj != null && obj != DBNull.Value)
                    {
                        string[] arr = obj.ToString().Split(',');
                        foreach (string roleId in arr)
                        {

                            sql = string.Format(string.Format(" insert into BASE_USERRELATION(USERRELATIONID,userid,Objectid,category,sortcode) select '{2}' || rownum,p.id,'{3}','2',0 from EPG_APTITUDEINVESTIGATEPEOPLE p where p.outengineerid='{0}' and p.outprojectid='{1}'", projectId, deptId, Guid.NewGuid().ToString(), roleId));
                            res.ExecuteBySql(sql);
                        }
                    }
                }
                //string id = Guid.NewGuid().ToString();
                //sql = string.Format(string.Format(" insert into BASE_USERRELATION(USERRELATIONID,userid,Objectid,category) select '{2}' || rownum,p.id,'c5530ccf-e84e-4df8-8b27-fd8954a9bbe9','2' from EPG_APTITUDEINVESTIGATEPEOPLE p where p.outengineerid='{0}' and p.outprojectid='{1}'", projectId, deptId, id));
                //res.ExecuteBySql(sql);
                string id = Guid.NewGuid().ToString();
                sql = string.Format(string.Format(@"insert into bis_certificate(id,userid,years,certname,certnum,senddate,sendorgan,filepath,enddate)
select c.id,c.userid,c.validttime,c.credentialsname,c.credentialscode,c.credentialstime,c.credentialsorg,c.credentialsfile,ADD_months(credentialstime,validttime*12) from EPG_CERTIFICATEINSPECTORS c", projectId, deptId, id));
                res.ExecuteBySql(sql);

                res.Commit();
                return true;
            }
            catch
            {
                res.Rollback();
                return false;
            }
        }
        /// <summary>
        /// 上传个人签名
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="signImg">图片名称</param>
        /// <returns></returns>
        public int UploadSignImg(string userId, string signImg, int mode = 0)
        {
            if (mode == 0)
            {
                return BaseRepository().ExecuteBySql(string.Format("update base_user set signimg='{0}' where userid='{1}'", signImg, userId));
            }
            else
            {
                return BaseRepository().ExecuteBySql(string.Format("update base_user set headicon='{0}' where userid='{1}'", signImg, userId));
            }

        }
        /// <summary>
        /// 保存用户表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="userEntity">用户实体</param>
        /// <returns></returns>
        public string SaveOnlyForm(string keyValue, UserEntity userEntity)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            try
            {
                #region 基本信息
                userEntity.UserId = keyValue;
                if (!string.IsNullOrEmpty(keyValue))
                {
                    UserEntity user = this.BaseRepository().FindEntity(keyValue);
                    if (user == null)
                    {
                        userEntity.Create();
                        userEntity.Password = userEntity.Password.Replace("&nbsp;", "");
                        userEntity.Password = string.IsNullOrEmpty(userEntity.Password) ? "Abc123456" : userEntity.Password;
                        userEntity.Secretkey = string.IsNullOrEmpty(userEntity.Secretkey) ? Md5Helper.MD5(CommonHelper.CreateNo(), 16).ToLower() : userEntity.Secretkey;
                        userEntity.NewPassword = DESEncrypt.Encrypt(userEntity.Password, userEntity.Secretkey);
                        userEntity.Password = Md5Helper.MD5(DESEncrypt.Encrypt(Md5Helper.MD5(userEntity.Password, 32).ToLower(), userEntity.Secretkey).ToLower(), 32).ToLower();
                        db.Insert(userEntity);
                    }
                    else
                    {
                        userEntity.Modify(keyValue);
                        userEntity.Password = null;
                        db.Update(userEntity);
                    }
                }
                else
                {
                    userEntity.Create();
                    keyValue = userEntity.UserId;
                    userEntity.Password = string.IsNullOrEmpty(userEntity.Password) ? "Abc123456" : userEntity.Password;
                    userEntity.Secretkey = string.IsNullOrEmpty(userEntity.Secretkey) ? Md5Helper.MD5(CommonHelper.CreateNo(), 16).ToLower() : userEntity.Secretkey;
                    userEntity.NewPassword = DESEncrypt.Encrypt(userEntity.Password, userEntity.Secretkey);
                    userEntity.Password = Md5Helper.MD5(DESEncrypt.Encrypt(Md5Helper.MD5(userEntity.Password, 32).ToLower(), userEntity.Secretkey).ToLower(), 32).ToLower();
                    db.Insert(userEntity);
                }
                #endregion
                #region 默认添加 角色、岗位
                db.Delete<UserRelationEntity>(t => t.IsDefault == 1 && t.UserId == userEntity.UserId);
                List<UserRelationEntity> userRelationEntitys = new List<UserRelationEntity>();
                var currUser = OperatorProvider.Provider.Current();
                string uid = currUser == null ? "" : currUser.UserId;
                string uname = currUser == null ? "" : currUser.UserName;
                //用户
                userRelationEntitys.Add(new UserRelationEntity
                {
                    Category = 6,
                    UserRelationId = Guid.NewGuid().ToString(),
                    UserId = userEntity.UserId,
                    ObjectId = userEntity.UserId,
                    CreateDate = DateTime.Now,
                    CreateUserId = uid,
                    CreateUserName = uname,
                    IsDefault = 1,
                });
                //角色
                string[] arr = userEntity.RoleId.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in arr)
                {
                    userRelationEntitys.Add(new UserRelationEntity
                    {
                        Category = 2,
                        UserRelationId = Guid.NewGuid().ToString(),
                        UserId = userEntity.UserId,
                        ObjectId = item,
                        CreateDate = DateTime.Now,
                        CreateUserId = uid,
                        CreateUserName = uname,
                        IsDefault = 1,
                    });
                }
                db.Insert(userRelationEntitys);
                #endregion
                db.Commit();

                return keyValue;
            }
            catch (Exception)
            {
                db.Rollback();
                throw;
            }
        }

        #region 通过审核部门、审核角色、专业获取审核人账号
        /// <summary>
        /// 获取人员
        /// </summary>
        /// <param name="flowdeptid">审核部门</param>
        /// <param name="flowrolename">审核角色</param>
        /// <param name="type">是否判断专业</param>
        /// <param name="specialtytype">专业类别</param>
        public string GetUserAccount(string flowdeptid, string flowrolename, string type = "", string specialtytype = "")
        {
            string names = "";
            string useraccounts = ""; 
            string userids = "";
            string flowdeptids = "'" + flowdeptid.Replace(",", "','") + "'";
            string flowrolenames = "'" + flowrolename.Replace(",", "','") + "'";
            IList<UserEntity> users = new UserService().GetUserListByRoleName(flowdeptids, flowrolenames, true, string.Empty).OrderBy(t => t.RealName).ToList();
            if (users != null && users.Count > 0)
            {
                if (!string.IsNullOrEmpty(specialtytype) && type == "1")
                {
                    foreach (var item in users)
                    {

                        if (item.RoleName.Contains("专工") && flowrolename.Split(',').Union(item.RoleName.Split(',')).Count() == (flowrolename.Split(',').Count() + item.RoleName.Split(',').Count() - 1)) //如果用户拥有专工角色而且还有审核角色中的其他一个就不需要判断专业
                        {
                            if (!string.IsNullOrEmpty(item.SpecialtyType) && item.SpecialtyType != "null")
                            {
                                string[] str = item.SpecialtyType.Split(',');
                                for (int i = 0; i < str.Length; i++)
                                {
                                    if (("," + specialtytype + ",").Contains("," + str[i] + ","))
                                    {
                                        names += item.RealName + ",";
                                        useraccounts += item.Account + ",";
                                        userids += item.UserId + ",";
                                    }
                                }

                            }
                        }
                        else
                        {
                            names += item.RealName + ",";
                            useraccounts += item.Account + ",";
                            userids += item.UserId + ",";
                        }
                    }
                    if (!string.IsNullOrEmpty(names))
                    {
                        names = names.TrimEnd(',');
                    }
                    if (!string.IsNullOrEmpty(useraccounts))
                    {
                        useraccounts = useraccounts.TrimEnd(',');
                    }
                    if (!string.IsNullOrEmpty(userids))
                    {
                        userids = userids.TrimEnd(',');
                    }
                }
                else
                {
                    names = string.Join(",", users.Select(x => x.RealName).ToArray());
                    useraccounts = string.Join(",", users.Select(x => x.Account).ToArray());
                    userids = string.Join(",", users.Select(x => x.UserId).ToArray());
                }
            }
            string useraccountandname = names + "|" + useraccounts + "|" + userids;
            return useraccountandname;
        }

        public List<UserEntity> GetList(string[] users, int pageSize, int pageIndex, out int total)
        {
            var query = this.BaseRepository().IQueryable().Where(x => users.Contains(x.UserId));
            total = query.Count();
            return query.OrderBy(x => x.Account).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
        }

        /// <summary>
        /// 根据用户姓名获取用户信息
        /// </summary>
        /// <param name="username">姓名</param>
        /// <returns></returns>
        public UserEntity GetUserInfoByUserName(string username)
        {
            username = username.Trim();
            var expression = LinqExtensions.True<UserEntity>();
            expression = expression.And(t => t.RealName.ToLower() == username.ToLower());
            return this.BaseRepository().FindEntity(expression);
        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="dbParameter"></param>
        /// <returns></returns>
        public IEnumerable<UserEntity> FindList(string strSql, DbParameter[] dbParameter)
        {
            return this.BaseRepository().FindList(strSql, dbParameter);
        }

        public bool ExistAccount(string account, string keyValue)
        {
            throw new NotImplementedException();
        }

        public bool ExistAccount(string account, string keyValue, string mobile)
        {
            throw new NotImplementedException();
        }
    }
}
