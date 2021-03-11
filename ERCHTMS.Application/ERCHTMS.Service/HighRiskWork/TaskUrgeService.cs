using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data;
using System;
using ERCHTMS.Code;
using System.Data.Common;
using ERCHTMS.Entity.BaseManage;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ERCHTMS.Service.HighRiskWork
{
    /// <summary>
    /// 描 述：旁站监管任务
    /// </summary>
    public class TaskUrgeService : RepositoryFactory<TaskUrgeEntity>, TaskUrgeIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TaskUrgeEntity> GetList(string queryJson)
        {
            return this.BaseRepository().FindList(string.Format("select * from bis_taskurge where 1=1 " + queryJson)).ToList();
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DatabaseType.Oracle;
            #region 数据权限
            #endregion
            #region 查表
            pagination.p_kid = "a.id";
            pagination.p_fields = "idea,files,urgetime,urgeuserid,urgeusername,deptname,deptcode,deptid,staffid,createdate,dataissubmit";
            pagination.p_tablename = " bis_taskurge a";
            pagination.conditionJson = "1=1";
            #endregion
            #region  筛选条件
            var queryParam = JObject.Parse(queryJson);
            if (!queryParam["staffid"].IsEmpty())//任务id
            {
                pagination.conditionJson += string.Format(" and staffid='{0}'", queryParam["staffid"].ToString());
            }
            #endregion
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TaskUrgeEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, TaskUrgeEntity entity)
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
