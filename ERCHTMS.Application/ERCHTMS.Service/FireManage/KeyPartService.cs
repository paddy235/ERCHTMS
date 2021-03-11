using ERCHTMS.Entity.FireManage;
using ERCHTMS.IService.FireManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using System;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.FireManage
{
    /// <summary>
    /// 描 述：重点防火部位
    /// </summary>
    public class KeyPartService : RepositoryFactory<KeyPartEntity>, KeyPartIService
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
            DatabaseType dataType = DbHelper.DbType;
            if (queryJson != null && queryJson != "")
            {
                var queryParam = queryJson.ToJObject();
                //名称
                if (!queryParam["PartNo"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and PartNo='{0}'", queryParam["PartNo"].ToString());
                }
                //部门
                if (!queryParam["DutydeptCode"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and DutydeptCode like'{0}%'", queryParam["DutydeptCode"].ToString());
                }
                //使用状态
                if (!queryParam["EmployState"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and EmployState='{0}'", queryParam["EmployState"].ToString());
                }
                //动火级别
                if (!queryParam["Rank"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and Rank='{0}'", queryParam["Rank"].ToString());
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<KeyPartEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public KeyPartEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, KeyPartEntity entity)
        {
            entity.Id = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                KeyPartEntity ke = this.BaseRepository().FindEntity(keyValue);
                if (ke == null)
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
