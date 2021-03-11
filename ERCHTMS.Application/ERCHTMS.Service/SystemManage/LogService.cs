using ERCHTMS.Entity.SystemManage;
using ERCHTMS.IService.SystemManage;
using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using System;
using System.Collections.Generic;

namespace ERCHTMS.Service.SystemManage
{
    /// <summary>
    /// 描 述：系统日志
    /// </summary>
    public class LogService : RepositoryFactory<LogEntity>, ILogService
    {
        #region 获取数据
        /// <summary>
        /// 日志列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<LogEntity> GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //日志分类
            if (!queryParam["Category"].IsEmpty())
            {
                int categoryId = queryParam["CategoryId"].ToInt();
                if (categoryId==3)
                {
                    pagination.conditionJson += string.Format(" and (CategoryId ={0} or CategoryId={1} or CategoryId={2} or CategoryId={3})", 5, 6, 7,8);
                }
                else
                {
                    pagination.conditionJson += string.Format(" and CategoryId ={0}", categoryId);
                }
               
            }
            //操作时间
            if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
            {
                DateTime startTime = queryParam["StartTime"].ToDate();
                DateTime endTime = queryParam["EndTime"].ToDate().AddDays(1);
                if (dataType == DatabaseType.Oracle) 
                {
                    pagination.conditionJson += string.Format(" and to_char(OperateTime,'yyyymmdd')>='{0}' and   to_char(OperateTime,'yyyymmdd')<='{1}' ", startTime.ToString("yyyyMMdd"), endTime.ToString("yyyyMMdd"));
                }
                else if (dataType == DatabaseType.SqlServer) 
                {
                    pagination.conditionJson += string.Format(" and OperateTime >= '{0}' and  OperateTime <=  '{1}'", startTime, endTime);
                }
            }
            //操作用户Id
            if (!queryParam["OperateUserId"].IsEmpty())
            {
                string OperateUserId = queryParam["OperateUserId"].ToString();
                pagination.conditionJson += string.Format(" and OperateUserId = '{0}'", OperateUserId);
            }
            //操作用户账户
            if (!queryParam["OperateAccount"].IsEmpty())
            {
                string OperateAccount = queryParam["OperateAccount"].ToString();
                pagination.conditionJson += string.Format(" and OperateAccount = '{0}'", OperateAccount);
            }
            //操作类型
            if (!queryParam["OperateType"].IsEmpty())
            {
                string operateType = queryParam["OperateType"].ToString();
                pagination.conditionJson += string.Format(" and OperateType = '{0}'", operateType); 
            }
            //功能模块
            if (!queryParam["Module"].IsEmpty())
            {
                string module = queryParam["Module"].ToString();
                pagination.conditionJson += string.Format(" and Module like '%{0}%'", module); 
            }

             return this.BaseRepository().FindListByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 日志实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public LogEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 清空日志
        /// </summary>
        /// <param name="categoryId">日志分类Id</param>
        /// <param name="keepTime">保留时间段内</param>
        public void RemoveLog(int categoryId, string keepTime)
        {
          
            DateTime operateTime = DateTime.Now;
            if (keepTime == "7")//保留近一周
            {
                operateTime = DateTime.Now.AddDays(-7);
            }
            else if (keepTime == "1")//保留近一个月
            {
                operateTime = DateTime.Now.AddMonths(-1);
            }
            else if (keepTime == "3")//保留近三个月
            {
                operateTime = DateTime.Now.AddMonths(-3);
            }
            
            string sql = string.Format("OperateTime<to_date('{0}','yyyy-mm-dd hh24:mi:ss')",operateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            if (categoryId==3)
            {
                //expression = expression.And(t => t.CategoryId == 5 || t.CategoryId==6 || t.CategoryId==7);
                sql += " and (CategoryId=5 or CategoryId=6 or CategoryId=7)";
            }
            else
            {
                sql += " and CategoryId=" + categoryId;
                //expression = expression.And(t => t.CategoryId == categoryId);
            }
            this.BaseRepository().ExecuteBySql(string.Format("delete from BASE_LOG where "+sql));
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logEntity">对象</param>
        public void WriteLog(LogEntity logEntity)
        {
            logEntity.LogId = Guid.NewGuid().ToString();
            logEntity.OperateTime = DateTime.Now;
            logEntity.DeleteMark = 0;
            logEntity.EnabledMark = 1;
            logEntity.IPAddress = Net.Ip;
            logEntity.Host = Net.Host;
            logEntity.Browser = Net.Browser;
            this.BaseRepository().Insert(logEntity);
        }
        #endregion
    }
}
