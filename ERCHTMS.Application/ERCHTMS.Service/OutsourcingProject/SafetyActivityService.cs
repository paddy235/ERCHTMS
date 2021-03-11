using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System;

namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// 描 述：外包安全活动
    /// </summary>
    public class SafetyActivityService : RepositoryFactory<SafetyActivityEntity>, SafetyActivityIService
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            if (queryJson != null && queryJson != "")
            {
                var queryParam = queryJson.ToJObject();
                
                //承包单位
                if (!queryParam["OutprojectName"].IsEmpty())
                {
                    string OutprojectName = queryParam["OutprojectName"].ToString();
                    pagination.conditionJson += string.Format(" and p.outsourcingname like '%{0}%'", OutprojectName);
                }
                //外包工程名称
                if (!queryParam["EngineerName"].IsEmpty())
                {
                    string EngineerName = queryParam["EngineerName"].ToString();
                    pagination.conditionJson += string.Format(" and o.engineername like '%{0}%'", EngineerName);
                }
                //开始时间
                if (!queryParam["StartTime"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(@" and t.StartTime >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["StartTime"].ToString());
                }
                //结束时间
                if (!queryParam["EndTime"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(@" and t.EndTime < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["EndTime"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SafetyActivityEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SafetyActivityEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, SafetyActivityEntity entity)
        {
            entity.ID = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                SafetyActivityEntity fe = this.BaseRepository().FindEntity(keyValue);
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