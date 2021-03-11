using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Entity.TrainPlan;
using ERCHTMS.IService.TrainPlan;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Service.TrainPlan
{
   public class SafeSummaryService: RepositoryFactory<SafeSummaryEntity>, ISafeSummaryService
    {
        #region 获取数据
        /// <summary>
        /// 安措计划总结报告列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_kid = "t.ID";
            pagination.p_fields = @"State,DepartmentName,BelongYear||'年' as BelongYear,case QUARTER when 1 then '第一季度' when 2 then '第二季度' when 3 then '第三季度' when 4 then '第四季度' end QUARTER,ReportName,OperateDate,OperateUserName,CREATEUSERID,CreateUserDeptCode,SubmitTime";
            pagination.p_tablename = "BIS_SAFEMEASURE_SUMMARY t";
            var queryParam = queryJson.ToJObject();
            if (queryJson.Contains("code") && queryParam["code"].ToString() != "")
            {
                pagination.conditionJson += string.Format(" and t.createuserdeptcode like '{0}%'", queryParam["code"].ToString());
            }
            if (queryJson.Contains("belongYear") && queryParam["belongYear"].ToString()!="")
            {
                pagination.conditionJson += string.Format(" and t.BelongYear='{0}'", queryParam["belongYear"].ToString());
            }
            if (queryJson.Contains("quarter") && queryParam["quarter"].ToString() != "")
            {
                pagination.conditionJson += string.Format(" and t.QUARTER='{0}'", queryParam["quarter"].ToString());
            }
            if (pagination.sidx == null)
            {
                pagination.sidx = "t.state asc,t.submittime desc";
            }
            //if (pagination.sord == null)
            //{
            //    pagination.sord = "asc,desc";
            //}
            DataTable data = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return data;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public SafeSummaryEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 检查是否提交
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool CheckExists(string keyValue, SafeSummaryEntity entity)
        {
            string sql = "select count(1) from BIS_SAFEMEASURE_SUMMARY where BelongYear=:BelongYear and QUARTER=:QUARTER and DepartmentId=:DepartmentId and ID!=:ID";
            DbParameter[] dbParameters = {
                 DbParameters.CreateDbParameter(":BelongYear", entity.BelongYear),
                 DbParameters.CreateDbParameter(":QUARTER",entity.Quarter),
                 DbParameters.CreateDbParameter(":DepartmentId", entity.DepartmentId),
                 DbParameters.CreateDbParameter(":ID", keyValue)
            };
            return Convert.ToInt32(this.BaseRepository().FindObject(sql, dbParameters)) > 0 ? true : false;
        }


        #endregion

        #region [提交数据]
        /// <summary>
        /// 新增/修改
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void SaveForm(string keyValue, SafeSummaryEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                if (GetEntity(keyValue) != null)
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
                else {
                    entity.Create();
                    entity.Id = keyValue;
                    this.BaseRepository().Insert(entity);
                }
            }
            else
            {
                entity.Create();
                entity.Id = keyValue;
                this.BaseRepository().Insert(entity);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public bool DeleteForm(string keyValue)
        {
            return this.BaseRepository().Delete(keyValue)>0?true:false;
        }
        #endregion

    }
}
