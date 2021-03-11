using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.IService.RiskDatabase;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Newtonsoft.Json.Linq;
using BSFramework.Util.Extension;
using System;
using System.Linq.Expressions;

namespace ERCHTMS.Service.RiskDatabase
{
    /// <summary>
    /// 描 述：作业活动及设备设施清单
    /// </summary>
    public class BaseListingService : RepositoryFactory<BaseListingEntity>, BaseListingIService
    {
        #region 获取数据

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetPageListJson(Pagination pagination, string queryJson)
        {
            DataTable dt = new DataTable();
            if (!string.IsNullOrWhiteSpace(queryJson))
            {
                var queryParam = JObject.Parse(queryJson);
                if (!queryParam["type"].IsEmpty())
                {
                    pagination.conditionJson += " and a.type=" + Convert.ToInt32(queryParam["type"].ToString());
                }
                if (!queryParam["status"].IsEmpty())
                {
                    if (queryParam["status"].ToString() == "0") //已评价
                    {
                        pagination.conditionJson += " and b.num>0";
                    }
                    else if (queryParam["status"].ToString() == "1") //未评价
                    {
                        pagination.conditionJson += " and b.num is null";
                    }
                }
                if (!queryParam["name"].IsEmpty())
                {
                    pagination.conditionJson += " and a.name ='" + queryParam["name"].ToString() + "'";
                }
            }
            dt = this.BaseRepository().FindTableByProcPager(pagination, BSFramework.Data.DatabaseType.Oracle);
            return dt;
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="condition">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<BaseListingEntity> GetList(Expression<Func<BaseListingEntity, bool>> condition)
        {
            return this.BaseRepository().IQueryable(condition).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public BaseListingEntity GetEntity(string keyValue)
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
            string[] ids = keyValue.Split(',');
            this.BaseRepository().Delete(t => ids.Contains(t.Id));
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, BaseListingEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                var data = GetEntity(keyValue);
                if (data == null)
                {
                    entity.Id = keyValue;
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
