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
    public class UserInfoService : RepositoryFactory<UserInfoEntity>, IUserInfoService
    {

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


        #region 获取用户
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public IList<UserInfoEntity> GetUserInfoByDeptCode(string deptCode)
        {
            string sql = string.Format(@"select * from V_USERINFO where departmentcode = '{0}' ", deptCode);

            var list = this.BaseRepository().FindList(sql.ToString()).ToList();

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
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();
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
                pagination.conditionJson += string.Format(" and t.DEPARTMENTID = '{0}'", departmentId);
            }
            //部门编码
            if (!queryParam["departmentCode"].IsEmpty())
            {
                string departmentCode = queryParam["departmentCode"].ToString();
                pagination.conditionJson += string.Format(" and t.DEPARTMENTID =(select departmentid from base_department where encode='{0}')", departmentCode);
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
                if (queryParam["isOrg"].ToString() == "Organize")
                    pagination.conditionJson += string.Format(" and t.organizeid  in (select organizeid from base_organize where encode like '{0}%')", queryParam["code"].ToString());
                else
                    pagination.conditionJson += string.Format(" and t.departmentid  in (select departmentid from base_department where encode like '{0}%')", queryParam["code"].ToString());
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
            return this.BaseRepository().FindEntity(expression);
        }
    }
}
