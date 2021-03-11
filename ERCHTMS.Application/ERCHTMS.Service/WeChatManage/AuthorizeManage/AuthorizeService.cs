﻿using ERCHTMS.Code;
using ERCHTMS.Entity.AuthorizeManage;
using ERCHTMS.Entity.AuthorizeManage.ViewModel;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.AuthorizeManage;
using BSFramework.Data;
using BSFramework.Data.Repository;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ERCHTMS.Service.AuthorizeManage
{
    /// <summary>
    /// 描 述：授权认证
    /// </summary>
    public class AuthorizeService : RepositoryFactory, IAuthorizeService
    {
        /// <summary>
        /// 获取授权功能
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public IEnumerable<ModuleEntity> GetModuleList(string userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  *
                            FROM    Base_Module
                            WHERE   ModuleId IN (
                                    SELECT  ItemId
                                    FROM    Base_Authorize
                                    WHERE   ItemType = 1
                                            AND ( ObjectId IN (
                                                  SELECT    ObjectId
                                                  FROM      Base_UserRelation
                                                  WHERE     UserId = @UserId ) )
                                            OR ObjectId = @UserId )
                            AND EnabledMark = 1  AND DeleteMark = 0 Order By SortCode");
            DbParameter[] parameter = 
            {
                DbParameters.CreateDbParameter("@UserId",userId)
            };
            return this.BaseRepository().FindList<ModuleEntity>(strSql.ToString(), parameter);
        }
        /// <summary>
        /// 获取授权功能按钮
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public IEnumerable<ModuleButtonEntity> GetModuleButtonList(string userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  *
                            FROM    Base_ModuleButton
                            WHERE   ModuleButtonId IN (
                                    SELECT  ItemId
                                    FROM    Base_Authorize
                                    WHERE   ItemType = 2
                                            AND ( ObjectId IN (
                                                  SELECT    ObjectId
                                                  FROM      Base_UserRelation
                                                  WHERE     UserId = @UserId ) )
                                            OR ObjectId = @UserId ) Order By SortCode");
            DbParameter[] parameter = 
            {
                DbParameters.CreateDbParameter("@UserId",userId)
            };
            return this.BaseRepository().FindList<ModuleButtonEntity>(strSql.ToString(), parameter);
        }
        public DataTable GetModuleButtonListByUserId(string userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT encode,moduleid
                            FROM    Base_ModuleButton
                            WHERE   ModuleButtonId IN (
                                    SELECT  ItemId
                                    FROM    Base_Authorize
                                    WHERE   ItemType = 2
                                            AND ( ObjectId IN (
                                                  SELECT    ObjectId
                                                  FROM      Base_UserRelation
                                                  WHERE     UserId = @UserId ) )
                                            OR ObjectId = @UserId ) Order By SortCode");
            DbParameter[] parameter = 
            {
                DbParameters.CreateDbParameter("@UserId",userId)
            };
            return this.BaseRepository().FindTable(strSql.ToString(), parameter);
        }
        /// <summary>
        /// 获取授权功能视图
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public IEnumerable<ModuleColumnEntity> GetModuleColumnList(string userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  *
                            FROM    Base_ModuleColumn
                            WHERE   ModuleColumnId IN (
                                    SELECT  ItemId
                                    FROM    Base_Authorize
                                    WHERE   ItemType = 3
                                            AND ( ObjectId IN (
                                                  SELECT    ObjectId
                                                  FROM      Base_UserRelation
                                                  WHERE     UserId = @UserId ) )
                                            OR ObjectId = @UserId )  Order By SortCode");
                DbParameter[] parameter = 
                {
                     DbParameters.CreateDbParameter("@UserId", userId)
                };
                return this.BaseRepository().FindList<ModuleColumnEntity>(strSql.ToString(), parameter);
        }
        /// <summary>
        /// 获取授权功能Url、操作Url
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public IEnumerable<AuthorizeUrlModel> GetUrlList(string userId)
        {
            StringBuilder strSql = new StringBuilder();
             strSql.Append(@"SELECT  ModuleId AS AuthorizeId ,
                                    ModuleId ,
                                    UrlAddress ,
                                    FullName
                            FROM    Base_Module
                            WHERE   ModuleId IN (
                                    SELECT  ItemId
                                    FROM    Base_Authorize
                                    WHERE   ItemType = 1
                                            AND ( ObjectId IN (
                                                  SELECT    ObjectId
                                                  FROM      Base_UserRelation
                                                  WHERE     UserId = @UserId ) )
                                            OR ObjectId = @UserId )
                                    AND EnabledMark = 1
                                    AND DeleteMark = 0
                                    AND IsMenu = 1
                                    AND UrlAddress IS NOT NULL
                            UNION
                            SELECT  ModuleButtonId AS AuthorizeId ,
                                    ModuleId ,
                                    ActionAddress AS UrlAddress ,
                                    FullName
                            FROM    Base_ModuleButton
                            WHERE   ModuleButtonId IN (
                                    SELECT  ItemId
                                    FROM    Base_Authorize
                                    WHERE   ItemType = 2
                                            AND ( ObjectId IN (
                                                  SELECT    ObjectId
                                                  FROM      Base_UserRelation
                                                  WHERE     UserId = @UserId ) )
                                            OR ObjectId = @UserId )
                                    AND ActionAddress IS NOT NULL");
                DbParameter[] parameter = 
                {
                     DbParameters.CreateDbParameter("@UserId", userId)
                };
               
                return this.BaseRepository().FindList<AuthorizeUrlModel>(strSql.ToString(), parameter);
        }
        /// <summary>
        /// 获取关联用户关系
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public IEnumerable<UserRelationEntity> GetUserRelationList(string userId)
        {
            return this.BaseRepository().IQueryable<UserRelationEntity>(t => t.UserId == userId);
        }
        /// <summary>
        /// 获得权限范围用户ID
        /// </summary>
        /// <param name="operators">当前登陆用户信息</param>
        /// <param name="isWrite">可写入</param>
        /// <returns></returns>
        public string GetDataAuthorUserId(Operator operators, bool isWrite = false)
        {
            string userIdList = GetDataAuthor(operators, isWrite);
            if (userIdList == "")
            {
                return "";
            }
            IRepository db = new RepositoryFactory().BaseRepository();
            string userId = operators.UserId;
            List<UserEntity> userList = db.FindList<UserEntity>(userIdList).ToList();
            StringBuilder userSb = new StringBuilder("");
            if (userList != null)
            {
                foreach (var item in userList)
                {
                    userSb.Append(item.UserId);
                    userSb.Append(",");
                }
            }
            return userSb.ToString();
        }
        /// <summary>
        /// 获得可读数据权限范围SQL
        /// </summary>
        /// <param name="operators">当前登陆用户信息</param>
        /// <param name="isWrite">可写入</param>
        /// <returns></returns>
        public string GetDataAuthor(Operator operators, bool isWrite = false)
        {
            //如果是系统管理员直接给所有数据权限
            if (operators.IsSystem)
            {
                return "";
            }
            IRepository db = new RepositoryFactory().BaseRepository();
            string userId = operators.UserId;
            StringBuilder whereSb = new StringBuilder(" select UserId from Base_user where 1=1 ");
            string strAuthorData = "";
            if (isWrite)
            {
                strAuthorData = @"   SELECT    *
                                        FROM      Base_AuthorizeData
                                        WHERE     IsRead=0 AND
                                        ObjectId IN (
                                                SELECT  ObjectId
                                                FROM    Base_UserRelation
                                                WHERE   UserId ='"+userId+"')";
            }
            else
            {
                strAuthorData = @"   SELECT    *
                                        FROM      Base_AuthorizeData
                                        WHERE     
                                        ObjectId IN (
                                                SELECT  ObjectId
                                                FROM    Base_UserRelation
                                                WHERE   UserId='" + userId + "')";
            }
            //DbParameter[] parameter = 
            //{
            //    DbParameters.CreateDbParameter("@UserId",userId),
            //};
            whereSb.Append(string.Format("AND (UserId ='{0}'", userId));
            IEnumerable<AuthorizeDataEntity> listAuthorizeData = db.FindList<AuthorizeDataEntity>(strAuthorData);
            foreach (AuthorizeDataEntity item in listAuthorizeData)
            {
                switch (item.AuthorizeType)
                {
                    //0代表最大权限
                    case 0://
                        return "";
                    //本人及下属
                    case -2://
                        whereSb.Append("  OR ManagerId ='{0}'");
                        break;
                    case -3:
                        whereSb.Append(@"  OR DepartmentId = (  SELECT  DepartmentId
                                                                    FROM    Base_User
                                                                    WHERE   UserId ='{0}'
                                                                  )");
                        break;
                    case -4:
                        whereSb.Append(@"  OR OrganizeId = (    SELECT  OrganizeId
                                                                    FROM    Base_User
                                                                    WHERE   UserId ='{0}'
                                                                  )");
                        break;
                    case -5:
                        whereSb.Append(string.Format(@"  OR DepartmentId='{1}' OR OrganizeId='{1}'", userId, item.ResourceId));
                        break;
                }
            }
            whereSb.Append(")");
            return whereSb.ToString();
        }
        /// <summary>
        /// 获取用户对模块的数据权限（本人,本部门，本部门及下属部门，本机构，全部）
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="deptCode">部门编码字段</param>
        /// <param name="orgCode">机构编码字段</param>
        /// <returns></returns>
        public string GetModuleDataAuthority(Operator user, string moduleId, string deptCode = "createuserdeptcode", string orgCode = "createuserorgcode")
        {
            if(user.IsSystem)
            {
                return "";
            }
            string userId = user.UserId;
            string result = "";
            string sql = @"select max(authorizetype) authorizetype  from BASE_AUTHORIZEDATA t where itemcode='search' and objectid in(select objectid from BASE_USERRELATION t
                where userid=@userId) and t.resourceid=@moduleId";
            DataTable dt = this.BaseRepository().FindTable(sql, new DbParameter[] { DbParameters.CreateDbParameter("@userId", userId), DbParameters.CreateDbParameter("@moduleId", moduleId) });
            if (dt.Rows.Count > 0)
            {
                string authorizeType = dt.Rows[0][0].ToString();
                switch (authorizeType)
                {
                    case "1":
                        result = "createuserid='" + userId + "'"; //只查询由本人新增的数据
                        break;
                    case "2":
                        result = deptCode+"='" + user.DeptCode + "'";//可以本部门的人员新增的数据
                        break;
                    case "3":
                        result = deptCode+" like '" + user.DeptCode + "%'";//查询部门及下属部门新增的数据
                        break;
                    case "4":
                        result = orgCode+"='" + user.OrganizeCode + "'";//查询本机构人员新增的数据
                        break;
                }
            }
            else
            {
                result = "0=1";
            }
            return result;
        }
        /// <summary>
        /// 判断用户对模块有无指定的操作权限
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <param name="moduleId">模块Id</param>
        ///  <param name="enCode">功能编号，如add(新增),edit(修改),delete(删除),export(导出)等</param>
        /// <returns></returns>
        public bool HasOperAuthority(Operator user, string moduleId,string enCode)
        {
            if (user.IsSystem)
            {
                return true;
            }
            string userId = user.UserId;
            string sql = @"select count(1) from (
                    select AUTHORIZEType,resourceid,itemcode from BASE_AUTHORIZEDATA t where t.resourceid in(select itemid from BASE_AUTHORIZE t where objectid in(select objectid from BASE_USERRELATION t
                where userid=@userId) and itemtype=1) ) where resourceid=@moduleId and itemcode=@enCode";
            string count = this.BaseRepository().FindObject(sql, new DbParameter[] { DbParameters.CreateDbParameter("@userId", userId), DbParameters.CreateDbParameter("@moduleId", moduleId), DbParameters.CreateDbParameter("@enCode", enCode) }).ToString();
            return count=="0" ? false : true;
            
        }
        /// <summary>
        /// 获取用户对模块的数据操作权限
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <param name="moduleId">模块Id</param>
        ///  <param name="enCode">功能编号，如add(新增),edit(修改),delete(删除),export(导出)等</param>
        /// <returns>1:本人，2：本部门，3：本子部门，4：本机构，5：全部</returns>
        public string GetOperAuthorzeType(Operator user, string moduleId, string enCode)
        {
            if(user.IsSystem)
            {
                return "5";
            }
            else
            {
                string userId = user.UserId;
                string sql = @"select max(AUTHORIZEType) from BASE_AUTHORIZEDATA t where t.objectid in(select objectid from BASE_USERRELATION t
                where userid=@userId) and resourceid=@moduleId and itemcode=@enCode";
                object obj = this.BaseRepository().FindObject(sql, new DbParameter[] { DbParameters.CreateDbParameter("@userId", userId), DbParameters.CreateDbParameter("@moduleId", moduleId), DbParameters.CreateDbParameter("@enCode", enCode) });
                return obj == DBNull.Value || obj == null ? "" : obj.ToString();
            }
        }
        /// <summary>
        ///获取用户对模块的操作权限
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <param name="moduleId">模块Id</param>
        ///  <param name="enCode">功能编号，如add(新增),edit(修改),delete(删除),export(导出)等，多个可用英文逗号分隔</param>
        /// <returns></returns>
        public string GetOperAuthority(Operator user, string moduleId, string enCode)
        {
            
            string userId = user.UserId;
            string sql = "";
            DataTable dt = null;
            if (string.IsNullOrEmpty(enCode))
            {
                //如果是管理员则获取所有功能权限
                if (user.IsSystem)
                {
                    sql = @"select encode,fullname,faimage,actionname from BASE_MODULEBUTTON t where t.moduleid='{0}' and buttontype=0 and encode!='search' order by sortcode asc";
                    dt = this.BaseRepository().FindTable(string.Format(sql,moduleId));
                }
                else
                {
                    sql = @"select encode,fullname,faimage,actionname from BASE_MODULEBUTTON t where t.moduleid='{1}' and buttontype=0 and encode!='search' and encode in(select itemcode from BASE_AUTHORIZEDATA t where t.objectid in(select objectid from BASE_USERRELATION t
                   where userid='{0}')) order by sortcode asc";
                    dt = this.BaseRepository().FindTable(string.Format(sql, userId, moduleId));
                }
            }
            else
            {
                //如果是管理员则获取所有功能权限
                if (user.IsSystem)
                {
                    sql = @"select encode,fullname,faimage,actionname from BASE_MODULEBUTTON t where t.moduleid='{0}' and buttontype=0 and encode!='search' order by sortcode asc";
                    dt = this.BaseRepository().FindTable(string.Format(sql, moduleId));
                }
                else
                {
                    sql = @"select  max(AUTHORIZEType) authorizetype,b.encode,faimage,actionname,fullname from BASE_AUTHORIZEDATA t inner join base_modulebutton b on t.itemcode=b.encode and t.resourceid=b.moduleid and buttontype=0 and b.encode!='search'and objectid in(select objectid from BASE_USERRELATION t
 where userid='{0}' ) and resourceid='{1}' and itemcode in('{2}') group by b.encode,faimage,actionname,fullname,b.sortcode order by b.sortcode asc";
                    dt = this.BaseRepository().FindTable(string.Format(sql, userId, moduleId, enCode.Replace(",", "','")));
                }
            }
            return dt.Rows.Count == 0 ? "" : Newtonsoft.Json.JsonConvert.SerializeObject(dt);
        }
        /// <summary>
        /// 获取用户对模块的数据行的操作权限（本人,本部门，本部门及下属部门，本机构，全部）
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="jsonData">json集合字符串，如[{UserId:'1',DeptCode:'0001',OrgCode:'00'},{UserId:'2',DeptCode:'0002',OrgCode:'00'}]</param>
        /// <returns>json集合字符串</returns>
        public string GetDataAuthority(Operator user, string moduleId,string jsonData)
        {
              string userId = user.UserId;
            DataTable dt = null;
            string sql = "";
            if (user.IsSystem)
            {
                sql = @"select 5 authorizetype,encode,faimage,actionname,fullname from BASE_MODULEBUTTON t where t.moduleid=@moduleId and buttontype=1 and encode!='search' order by sortcode asc";
                dt = this.BaseRepository().FindTable(sql, new DbParameter[] {DbParameters.CreateDbParameter("@moduleId", moduleId) });
            }
            else
            {
                sql = @"select  max(AUTHORIZEType) authorizetype,b.encode,faimage,actionname,fullname from BASE_AUTHORIZEDATA t inner join base_modulebutton b on t.itemcode=b.encode and t.resourceid=b.moduleid and buttontype=1 and b.encode!='search'and objectid in(select objectid from BASE_USERRELATION t
 where userid=@userId ) and resourceid=@moduleId group by b.encode,faimage,actionname,fullname,b.sortcode order by b.sortcode asc";
                dt = this.BaseRepository().FindTable(sql, new DbParameter[] { DbParameters.CreateDbParameter("@userId", userId), DbParameters.CreateDbParameter("@moduleId", moduleId) });
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(dt);
        }
    }
}
