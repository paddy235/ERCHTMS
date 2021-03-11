using ERCHTMS.Entity.FireManage;
using ERCHTMS.IService.FireManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System;
using System.Data.Common;

namespace ERCHTMS.Service.FireManage
{
    /// <summary>
    /// 描 述：日常巡查
    /// </summary>
    public class EverydayPatrolService : RepositoryFactory<EverydayPatrolEntity>, EverydayPatrolIService
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
                //时间范围
                if (!queryParam["sTime"].IsEmpty() || !queryParam["eTime"].IsEmpty())
                {
                    string startTime = queryParam["sTime"].ToString();
                    string endTime = queryParam["eTime"].ToString();
                    if (queryParam["sTime"].IsEmpty())
                    {
                        startTime = "1899-01-01";
                    }
                    if (queryParam["eTime"].IsEmpty())
                    {
                        endTime = "2099-01-01";
                        //endTime = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    endTime = (Convert.ToDateTime(endTime).AddDays(1)).ToString("yyyy-MM-dd");
                    pagination.conditionJson += string.Format(" and PatrolDate between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", startTime, endTime);
                }
                //查询条件
                if (!queryParam["PatrolPersonId"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and PatrolPersonId='{0}'", queryParam["PatrolPersonId"].ToString());
                }
                //部门
                if (!queryParam["PatrolDeptCode"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and PatrolDeptCode like '{0}%'", queryParam["PatrolDeptCode"].ToString());
                }
                //巡查类型
                if (!queryParam["PatrolTypeCode"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and PatrolTypeCode='{0}'", queryParam["PatrolTypeCode"].ToString());
                }
                //流程状态
                if (!queryParam["AffirmState"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and AffirmState='{0}'", queryParam["AffirmState"].ToString());
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<EverydayPatrolEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public EverydayPatrolEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 获取部门负责人账户
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetMajorUserId(string departmentid)
        {
            string sql = @"select ACCOUNT from base_user where instr(rolename,'负责人' )> 0 and departmentid =@departmentid";
            DataTable dt = this.BaseRepository().FindTable(sql, new DbParameter[] { DbParameters.CreateDbParameter("@departmentid", departmentid) });
            string approverPeopleIds = "";
            foreach (DataRow dr in dt.Rows)
            {
                approverPeopleIds += dr["account"] + ",";
            }
            return !string.IsNullOrEmpty(approverPeopleIds) ? approverPeopleIds.Substring(0, approverPeopleIds.Length - 1) : "";
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
        public void SaveForm(string keyValue, EverydayPatrolEntity entity)
        {
            entity.Id = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                EverydayPatrolEntity ee = this.BaseRepository().FindEntity(keyValue);
                if (ee == null)
                {
                    if (entity.PatrolTypeCode == "RJ" || entity.PatrolTypeCode == "ZJ")
                    {
                        entity.AffirmUserId = entity.DutyUserId;
                    }
                    else
                    {
                        entity.AffirmUserId = entity.ByUserId;
                    }
                    //entity.AffirmState = 0;
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
                if (entity.PatrolTypeCode == "RJ" || entity.PatrolTypeCode == "ZJ")
                {
                    entity.AffirmUserId = entity.DutyUserId;
                }
                else
                {
                    entity.AffirmUserId = entity.ByUserId;
                }
                //entity.AffirmState = 0;
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
