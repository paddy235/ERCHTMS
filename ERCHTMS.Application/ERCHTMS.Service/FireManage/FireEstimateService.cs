using ERCHTMS.Entity.FireManage;
using ERCHTMS.IService.FireManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using ERCHTMS.Code;
using BSFramework.Util.Extension;
using System;

namespace ERCHTMS.Service.FireManage
{
    /// <summary>
    /// 描 述：消防估算指标
    /// </summary>
    public class FireEstimateService : RepositoryFactory<FireEstimateEntity>, FireEstimateIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;
            if (queryJson != null && queryJson != "")
            {
                var queryParam = queryJson.ToJObject();

                //查询条件 名称
                if (!queryParam["FireTypeName"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and FireTypeName='{0}'", queryParam["FireTypeName"].ToString());
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<FireEstimateEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public FireEstimateEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
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
        public void SaveForm(string keyValue, FireEstimateEntity entity)
        {
            entity.Id = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                FireEstimateEntity fe = this.BaseRepository().FindEntity(keyValue);
                if (fe == null)
                {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                }
                else
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
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
