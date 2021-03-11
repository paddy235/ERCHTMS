using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using ERCHTMS.Entity.Observerecord;
using ERCHTMS.IService.Observerecord;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.Observerecord
{
    public class ObsAnalysisReportService : RepositoryFactory<ObsAnalysisReportEntity>, ObsAnalysisReportIService
    {
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            if (!queryParam["deptcode"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and  t.workunitcode like'{0}%' ", queryParam["deptcode"].ToString());
            }
            if (!queryParam["quarter"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.quarter ='{0}' ", queryParam["quarter"].ToString());
            }
            if (!queryParam["year"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.year ='{0}' ", queryParam["year"].ToString());
            }
            if (!queryParam["reporttype"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.reporttype ='{0}' ", queryParam["reporttype"].ToString());
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ObsAnalysisReportEntity GetEntity(string keyValue) {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="keyValue"></param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void SaveForm(string keyValue, ObsAnalysisReportEntity entity)
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
    }
}
