using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using BSFramework.Util.Extension;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using BSFramework.Data;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Service.AuthorizeManage;
using ERCHTMS.IService.AuthorizeManage;

namespace ERCHTMS.Service.BaseManage
{
    /// <summary>
    /// 描 述：角色管理
    /// </summary>
    public class RoleService : RepositoryFactory<RoleEntity>, IRoleService
    {
        private IAuthorizeService<RoleEntity> iauthorizeservice = new AuthorizeService<RoleEntity>();

        #region 获取数据
        /// <summary>
        /// 角色列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoleEntity> GetList()
        {
            var expression = LinqExtensions.True<RoleEntity>();
            expression = expression.And(t => t.Category == 1).And(t => t.EnabledMark == 1).And(t => t.DeleteMark == 0);
            return this.BaseRepository().IQueryable(expression).OrderByDescending(t => t.CreateDate).ToList();
        }
        public IEnumerable<RoleEntity> GetList(int Category) {
            var expression = LinqExtensions.True<RoleEntity>();
            expression = expression.And(t => t.Category == Category).And(t => t.EnabledMark == 1).And(t => t.DeleteMark == 0);
            return this.BaseRepository().IQueryable(expression).OrderByDescending(t => t.CreateDate).ToList();
        }
        /// <summary>
        /// 角色列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<RoleEntity> GetPageList(Pagination pagination, string queryJson)
        {
            StringBuilder sb = new StringBuilder("select t.OrganizeId,t.RoleId,t.EnCode,t.FullName,o.FullName OrgName,t.CreateDate,t.IsPublic,t.EnabledMark,t.description,t.SORTCODE from BASE_ROLE t left join base_organize o on t.organizeid=o.organizeid where t.Category = 1");
            var param=new List<DbParameter>();
            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyword = queryParam["keyword"].ToString();
                switch (condition)
                {
                    case "EnCode":            //角色编号
                        sb.AppendFormat(" and t.EnCode like @EnCode");
                        param.Add(DbParameters.CreateDbParameter("@EnCode", '%' + keyword + '%'));
                        break;
                    case "FullName":          //角色名称
                        sb.AppendFormat(" and t.FullName like @FullName");
                        param.Add(DbParameters.CreateDbParameter("@FullName", '%' + keyword + '%'));

                        break;
                    default:
                        break;
                }
            }
            return this.BaseRepository().FindList(sb.ToString(), param.ToArray(),pagination);
          
        }
        /// <summary>
        /// 角色列表all
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoleEntity> GetAllList()
        {
            var strSql = new StringBuilder();
            strSql.Append(@"SELECT  r.RoleId ,
				                    o.FullName AS OrganizeId ,
				                    r.Category ,
				                    r.EnCode ,
				                    r.FullName ,
				                    r.SortCode ,
				                    r.EnabledMark ,
				                    r.Description ,
				                    r.CreateDate
                    FROM    Base_Role r
				                    LEFT JOIN Base_Organize o ON o.OrganizeId = r.OrganizeId
                    WHERE   o.FullName is not null and r.Category = 1 and r.EnabledMark =1
                    ORDER BY o.FullName, r.SortCode");
            return this.BaseRepository().FindList(strSql.ToString());
        }
        /// <summary>
        /// 角色实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public RoleEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region 验证数据
        /// <summary>
        /// 角色编号不能重复
        /// </summary>
        /// <param name="enCode">编号</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistEnCode(string enCode, string keyValue)
        {
            var expression = LinqExtensions.True<RoleEntity>();
            expression = expression.And(t => t.EnCode == enCode).And(t => t.Category == 1);
            if (!string.IsNullOrEmpty(keyValue))
            {
                expression = expression.And(t => t.RoleId != keyValue);
            }
            return this.BaseRepository().IQueryable(expression).Count() == 0 ? true : false;
        }
        /// <summary>
        /// 角色名称不能重复
        /// </summary>
        /// <param name="fullName">名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistFullName(string fullName, string keyValue)
        {
            var expression = LinqExtensions.True<RoleEntity>();
            expression = expression.And(t => t.FullName == fullName).And(t => t.Category == 1);
            if (!string.IsNullOrEmpty(keyValue))
            {
                expression = expression.And(t => t.RoleId != keyValue);
            }
            return this.BaseRepository().IQueryable(expression).Count() == 0 ? true : false;
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="keyValue">主键</param>
        public bool RemoveForm(string keyValue)
        {
            try
            {
                int count=this.BaseRepository().Delete(keyValue);
                return count > 0 ? true : false;
            }
            catch
            {
                return false;
            }
           
        }
        /// <summary>
        /// 保存角色表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="roleEntity">角色实体</param>
        /// <returns></returns>
        public bool SaveForm(string keyValue, RoleEntity roleEntity)
        {
            try
            {
                roleEntity.RoleId = keyValue;
                object roleId = BaseRepository().FindObject(string.Format("select roleid from base_role where Category=1 and FullName='{0}'", roleEntity.FullName.Trim()));
                if (roleId != null)
                {
                    keyValue = roleEntity.RoleId = roleId.ToString();
                }
                if (!string.IsNullOrEmpty(keyValue))
                {
                    RoleEntity entity = BaseRepository().FindEntity(keyValue);
                    if (entity == null)
                    {
                        roleEntity.Create();
                        roleEntity.Category = 1;
                        this.BaseRepository().Insert(roleEntity);
                    }
                    else
                    {
                        roleEntity.Modify(keyValue);
                        this.BaseRepository().Update(roleEntity);
                    }
                }
                else
                {
                    roleEntity.Create();
                    roleEntity.Category = 1;
                    this.BaseRepository().Insert(roleEntity);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
