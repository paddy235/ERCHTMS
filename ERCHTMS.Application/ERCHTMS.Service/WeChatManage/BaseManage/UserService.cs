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

namespace ERCHTMS.Service.BaseManage
{
    /// 描 述：用户管理
    /// </summary>
    public class UserService : RepositoryFactory<UserEntity>, IUserService
    {
        private IAuthorizeService<UserEntity> iauthorizeservice = new AuthorizeService<UserEntity>();

        #region 获取数据
        /// <summary> 
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable(string deptId = "")
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT  u.userid,u.EnCode,u.Account,u.RealName,u.departmentid,gender,
                                    d.FullName AS DepartmentName 
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
        /// 用户列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();
            //公司主键
            if (!queryParam["organizeId"].IsEmpty())
            {
                string organizeId = queryParam["organizeId"].ToString();
                pagination.conditionJson += string.Format(" and t.ORGANIZEID = '{0}'", organizeId);
            }
            //部门主键
            if (!queryParam["departmentId"].IsEmpty())
            {
                string departmentId = queryParam["departmentId"].ToString();
                pagination.conditionJson += string.Format(" and t.DEPARTMENTID = '{0}'", departmentId);
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
            if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                string deptCode=queryParam["code"].ToString();
                if (queryParam["isOrg"].ToString() == "Organize")
                {
                    pagination.conditionJson += string.Format(" and t.organizecode  like '{0}%'", deptCode);
                    if (user.IsSystem || user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户"))
                    {
                        pagination.conditionJson += string.Format(" and (t.departmentcode like '{0}%'", deptCode);
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(" and (t.departmentcode like '{0}%'", user.DeptCode);
                    }
                    pagination.conditionJson += string.Format(" or departmentid in(select departmentid from BASE_DEPARTMENT t where t.senddeptid=(select departmentid from base_department d where d.encode='{0}')))", user.DeptCode);
                }

                else if (queryParam["isOrg"].ToString() == "Department" || user.IsSystem)
                {
                    pagination.conditionJson += string.Format(" and t.departmentcode like '{0}%'", deptCode);
                }
                else
                {
                    pagination.conditionJson += string.Format(" and (t.departmentcode like '{0}%'", deptCode);
                    pagination.conditionJson += string.Format(" and departmentid in(select departmentid from BASE_DEPARTMENT t where t.senddeptid=(select departmentid from base_department d where d.encode='{0}')))", user.DeptCode);
                }
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

        #region 获取所有部门
        /// <summary>
        /// 获取所有部门
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllTableByArgs(string argValue,string organizeid) 
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
        public DataTable GetAllTableByArgs(string username, string deptcode, string organizeid)
        {
            string str = "";

            //姓名参数
            if (!username.IsEmpty())
            {
                str += string.Format(" and  u.realname like '%{0}%' ", username);
            }
            //当前机构
            if (!organizeid.IsEmpty())
            {
                str += string.Format(" and  u.organizeid ='{0}' ", organizeid);
            }
            //当前部门
            if (!deptcode.IsEmpty())
            {
                str += string.Format(" and u.departmentcode ='{0}' ", deptcode);
            }
            string strSql = string.Format(@"select  u.userid  ,
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
                                    o.fullname as organizename ,
                                    d.fullname as departmentname ,
                                    u.roleid ,
                                    u.dutyname ,
                                    u.postname ,
                                    u.enabledmark ,
                                    u.createdate,
                                    u.description,
                                    u.deletemark
                            from    base_user u
                                    left join base_organize o on o.organizeid = u.organizeid
                                    left join base_department d on d.departmentid = u.departmentid
                            where  u.departmentid is not null  and  u.userid <> 'system' and u.enabledmark = 1 and u.deletemark=0  {0}
                            union
                            select  u.userid  ,
                                    u.encode ,
                                    u.account ,
                                    u.realname ,
                                    u.gender ,
                                    u.birthday ,
                                    u.mobile ,
                                    u.manager ,
                                    u.organizeid,
                                    u.organizeid as departmentid,
                                    o.encode as departmentcode,
                                    o.fullname as organizename ,
                                    o.fullname as departmentname ,
                                    u.roleid ,
                                    u.dutyname ,
                                    u.postname ,
                                    u.enabledmark ,
                                    u.createdate,
                                    u.description,
                                    u.deletemark
                            from    base_user u
                                    left join base_organize o on o.organizeid = u.organizeid
                            where   u.departmentid is  null  and  u.userid <> 'system' and u.enabledmark = 1 and u.deletemark=0  {0}", str);

            strSql += " order by  realname";

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
        /// 登录验证
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        public UserEntity CheckLogin(string username)
        {
            var expression = LinqExtensions.True<UserEntity>();
            expression = expression.And(t => t.Account == username);
            expression = expression.Or(t => t.Mobile == username);
            expression = expression.Or(t => t.Email == username);
            return this.BaseRepository().FindEntity(expression);
        }
        #endregion

        #region 验证数据
        /// <summary>
        /// 账户不能重复
        /// </summary>
        /// <param name="account">账户值</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistAccount(string account, string keyValue)
        {
            var expression = LinqExtensions.True<UserEntity>();
            expression = expression.And(t => t.Account == account);
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
            var expression = LinqExtensions.True<UserEntity>();
            expression = expression.And(t => t.IdentifyID == IdentifyID);
            if (!string.IsNullOrEmpty(keyValue))
            {
                expression = expression.And(t => t.UserId != keyValue);
            }
            return this.BaseRepository().IQueryable(expression).Count() == 0 ? true : false;
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
        public string SaveForm(string keyValue, UserEntity userEntity)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            try
            {
                #region 基本信息
                if (!string.IsNullOrEmpty(keyValue))
                {
                    userEntity.Modify(keyValue);
                    userEntity.Password = null;
                    db.Update(userEntity);
                }
                else
                {
                    userEntity.Create();
                    keyValue = userEntity.UserId;
                    userEntity.Secretkey = Md5Helper.MD5(CommonHelper.CreateNo(), 16).ToLower();
                    userEntity.Password = Md5Helper.MD5(DESEncrypt.Encrypt(Md5Helper.MD5(userEntity.Password, 32).ToLower(), userEntity.Secretkey).ToLower(), 32).ToLower();
                    db.Insert(userEntity);

                }
                #endregion

                #region 默认添加 角色、岗位、职位
                db.Delete<UserRelationEntity>(t => t.IsDefault == 1 && t.UserId == userEntity.UserId);
                List<UserRelationEntity> userRelationEntitys = new List<UserRelationEntity>();
                //用户
                userRelationEntitys.Add(new UserRelationEntity
                {
                    Category = 6,
                    UserRelationId = Guid.NewGuid().ToString(),
                    UserId = userEntity.UserId,
                    ObjectId = userEntity.UserId,
                    CreateDate = DateTime.Now,
                    CreateUserId = OperatorProvider.Provider.Current().UserId,
                    CreateUserName = OperatorProvider.Provider.Current().UserName,
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
                        CreateUserId = OperatorProvider.Provider.Current().UserId,
                        CreateUserName = OperatorProvider.Provider.Current().UserName,
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
                        CreateUserId = OperatorProvider.Provider.Current().UserId,
                        CreateUserName = OperatorProvider.Provider.Current().UserName,
                        IsDefault = 1,
                    });
                }
                //职位
                if (!string.IsNullOrEmpty(userEntity.PostId))
                {
                    userRelationEntitys.Add(new UserRelationEntity
                    {
                        Category = 4,
                        UserRelationId = Guid.NewGuid().ToString(),
                        UserId = userEntity.UserId,
                        ObjectId = userEntity.PostId,
                        CreateDate = DateTime.Now,
                        CreateUserId = OperatorProvider.Provider.Current().UserId,
                        CreateUserName = OperatorProvider.Provider.Current().UserName,
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
        /// <summary>
        /// 修改用户登录密码
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="Password">新密码（MD5 小写）</param>
        public void RevisePassword(string keyValue, string Password)
        {
            UserEntity userEntity = new UserEntity();
            userEntity.UserId = keyValue;
            userEntity.Secretkey = Md5Helper.MD5(CommonHelper.CreateNo(), 16).ToLower();
            userEntity.Password = Md5Helper.MD5(DESEncrypt.Encrypt(Password, userEntity.Secretkey).ToLower(), 32).ToLower();
            this.BaseRepository().Update(userEntity);
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


        /// <summary>
        /// 通过角色获取集合
        /// </summary>
        /// <param name="category"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public IList<UserEntity> GetUserListByRole(string deptmentid, string roleCode)
        {
            string sql = @"select * from base_user a
                                         left join base_userrelation b on a.userid = b.userid
                                         left join base_role c on b.objectid =c.roleid
                                         where 1=1 ";

            if (!deptmentid.IsEmpty()) 
            {
                sql += string.Format("  and  a.departmentcode = '{0}' ",deptmentid);
            }
            if (!roleCode.IsEmpty()) 
            {
                sql += string.Format(" and c.encode in ({0})", roleCode);
            }

            var list = this.BaseRepository().FindList(sql).ToList();

            return list;
        }

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

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public IList<UserEntity> GetUserListByDeptCode(string deptCode, string roleCode ,bool isSplit)
        {
            string sql = @"select distinct a.* from base_user a
                                    left join base_department b on a.departmentcode = b.encode
                                    left join base_userrelation c on a.userid = c.userid
                                    left join base_role d on  c.objectid = d.roleid
                                    where 1=1 ";

            string str = " ";

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



    }
}
