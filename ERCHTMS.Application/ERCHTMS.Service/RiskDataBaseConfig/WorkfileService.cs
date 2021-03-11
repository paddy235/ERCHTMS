using ERCHTMS.Entity.RiskDataBaseConfig;
using ERCHTMS.IService.RiskDataBaseConfig;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Data;

namespace ERCHTMS.Service.RiskDataBaseConfig
{
    /// <summary>
    /// 描 述：工作任务清单说明表
    /// </summary>
    public class WorkfileService : RepositoryFactory<WorkfileEntity>, WorkfileIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson) {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            if (!queryParam["Keyword"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and title like'%{0}%'", queryParam["Keyword"].ToString());
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<WorkfileEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public WorkfileEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
          /// <summary>
        /// 根据机构Code查询本机构是否已经添加
        /// </summary>
        /// <param name="orgCode">机构Code</param>
        /// <returns></returns>
        public bool GetIsExist(string orgCode) {
            var count = this.BaseRepository().IQueryable().Where(x => x.CreateUserOrgCode == orgCode).ToList().Count;
            if (count == 0) return true;
            else
                return false;
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
        public void SaveForm(string keyValue, WorkfileEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
