using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.BaseManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data;
using System.Linq.Expressions;
using System;

namespace ERCHTMS.Service.BaseManage
{
    /// <summary>
    /// 描 述：外包工程项目信息
    /// </summary>
    public class ProjectService : RepositoryFactory<ProjectEntity>, IProjectService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<ProjectEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ProjectEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<ProjectEntity> GetPageList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            //选择的类型
            if (!queryParam["type"].IsEmpty())
            {
                string type = queryParam["type"].ToString();
                pagination.conditionJson += string.Format(" and t.ProjectStatus in ('{0}') and t.ProjectStatus is not null", type);
            }
            //搜索关键字
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString();
                pagination.conditionJson += string.Format(" and t.ProjectName like '%{0}%'", keyword);
            }
            IEnumerable<ProjectEntity> list = this.BaseRepository().FindListByProcPager(pagination, dataType);

            return list;

        }

        /// <summary>
        /// 列表分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageDataTable(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();

            //选择的类型
            if (!queryParam["type"].IsEmpty())
            {
                string type = queryParam["type"].ToString();
                pagination.conditionJson += string.Format(" and t.ProjectStatus in ('{0}') and t.ProjectStatus is not null", type);
            }
            //搜索关键字
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString();
                pagination.conditionJson += string.Format(" and t.ProjectName like '%{0}%'", keyword);
            }
            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return dt;

        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, ProjectEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                if (entity.ProjectStartDate == null)
                {
                    entity.ProjectStartDate = Convert.ToDateTime("1900-01-01");
                }
                if (entity.ProjectEndDate == null)
                {
                    entity.ProjectEndDate = Convert.ToDateTime("1900-01-01");
                }
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                entity.OrganizeCode = entity.ProjectDeptCode.Substring(0, 3);
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
