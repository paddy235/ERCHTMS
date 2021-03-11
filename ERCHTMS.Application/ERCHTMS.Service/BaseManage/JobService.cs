using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.AuthorizeManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Service.AuthorizeManage;
using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Data;

namespace ERCHTMS.Service.BaseManage
{
    /// <summary>
    /// 描 述：职位管理
    /// </summary>
    public class JobService : RepositoryFactory<RoleEntity>, IJobService
    {
        private IAuthorizeService<RoleEntity> iauthorizeservice = new AuthorizeService<RoleEntity>();

        #region 获取数据
        /// <summary>
        /// 职位列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RoleEntity> GetList()
        {
            var expression = LinqExtensions.True<RoleEntity>();
            expression = expression.And(t => t.Category == 3).And(t => t.EnabledMark == 1).And(t => t.DeleteMark == 0);
            return this.BaseRepository().IQueryable(expression).OrderByDescending(t => t.CreateDate).ToList();
        }
        /// <summary>
        /// 职位列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<RoleEntity> GetPageList(Pagination pagination, string queryJson)
        {
            var expression = LinqExtensions.True<RoleEntity>();
            var queryParam = queryJson.ToJObject();
            //机构主键
            if (!queryParam["organizeId"].IsEmpty())
            {
                string organizeId = queryParam["organizeId"].ToString();
                expression = expression.And(t => t.OrganizeId.Equals(organizeId));
            }
            //查询条件
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyword = queryParam["keyword"].ToString();
                switch (condition)
                {
                    case "EnCode":            //职位编号
                        expression = expression.And(t => t.EnCode.Contains(keyword));
                        break;
                    case "FullName":          //职位名称
                        expression = expression.And(t => t.FullName.Contains(keyword));
                        break;
                    default:
                        break;
                }
            }
            expression = expression.And(t => t.Category == 3);
            return this.BaseRepository().FindList(expression, pagination);
        }
        /// <summary>
        /// 职位列表分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();
            ////公司主键
            //if (!queryParam["organizeId"].IsEmpty())
            //{
            //    string organizeId = queryParam["organizeId"].ToString();
            //    pagination.conditionJson += string.Format(" and t.organizeId = '{0}'", organizeId);
            //}
            //查询条件
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyord = queryParam["keyword"].ToString();
                switch (condition)
                {
                    case "EnCode":            //账户
                        pagination.conditionJson += string.Format(" and t.EnCode  like '%{0}%'", keyord);
                        break;
                    case "FullName":          //姓名
                        pagination.conditionJson += string.Format(" and t.FullName  like '%{0}%'", keyord);
                        break;
                    default:
                        break;
                }
            }
            if (!queryParam["deptid"].IsEmpty())
            {
                //先获取到需要查询的部门id
                Repository<DepartmentEntity> deptdb = new Repository<DepartmentEntity>(DbFactory.Base());
                string deptsql = string.Format("select * from base_Department  where departmentid ='{0}' ", queryParam["deptid"].ToString());
                DepartmentEntity dept = deptdb.FindList(deptsql).FirstOrDefault();
                if (dept != null)
                {
                    //根据该部门机构id和层级查询职务
                    pagination.conditionJson += string.Format(" and t.organizeid  = '{0}' and t.nature='{1}'",
                       dept.OrganizeId, dept.Nature);
                }
                else
                {
                    //根据该部门机构id和层级查询职务
                    pagination.conditionJson += string.Format(" and t.deptid  = '{0}'",
                        queryParam["deptid"].ToString());
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 职位实体
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
        /// 职位编号不能重复
        /// </summary>
        /// <param name="enCode">编号</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistEnCode(string enCode, string keyValue)
        {
            var expression = LinqExtensions.True<RoleEntity>();
            expression = expression.And(t => t.EnCode == enCode).And(t => t.Category == 3);
            if (!string.IsNullOrEmpty(keyValue))
            {
                expression = expression.And(t => t.RoleId != keyValue);
            }
            return this.BaseRepository().IQueryable(expression).Count() == 0 ? true : false;
        }
        /// <summary>
        /// 职位名称不能重复
        /// </summary>
        /// <param name="fullName">名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistFullName(string fullName, string keyValue)
        {
            var expression = LinqExtensions.True<RoleEntity>();
            expression = expression.And(t => t.FullName == fullName).And(t => t.Category == 3);
            if (!string.IsNullOrEmpty(keyValue))
            {
                expression = expression.And(t => t.RoleId != keyValue);
            }
            return this.BaseRepository().IQueryable(expression).Count() == 0 ? true : false;
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除职位
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存职位表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="jobEntity">职位实体</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, RoleEntity jobEntity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                jobEntity.Modify(keyValue);
                this.BaseRepository().Update(jobEntity);
            }
            else
            {
                jobEntity.Create();
                jobEntity.Category = 3;
                this.BaseRepository().Insert(jobEntity);
            }
        }
        #endregion
    }
}
