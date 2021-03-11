using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.IService.HiddenTroubleManage;
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

namespace ERCHTMS.Service.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：
    /// </summary>
    public class PublicityService : RepositoryFactory<PublicityEntity>, PublicityIService
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

                //查询条件 安全检查类型
                if (!queryParam["ExamineType"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and p.ExamineType='{0}'", queryParam["ExamineType"].ToString());
                }
                //查询条件 年度
                if (!queryParam["Year"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and p.Year='{0}'", queryParam["Year"].ToString());
                }
                //查询条件 展示类别  1 按周  2 按月
                if (!queryParam["ShowType"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and p.ShowType='{0}'", queryParam["ShowType"].ToString());
                }
                //查询条件 人员类别
                if (!queryParam["StaffType"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and p.StaffType='{0}'", queryParam["StaffType"].ToString());
                }
                
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
                    pagination.conditionJson += string.Format(" and CreateDate between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", startTime, endTime);
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<PublicityEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public PublicityEntity GetEntity(string keyValue)
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
        /// 执行sql
        /// </summary>
        /// <param name="sql">sql语句</param>
        public int ExecuteBySql(string sql)
        {

            return this.BaseRepository().ExecuteBySql(sql);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, PublicityEntity entity)
        {
            entity.Id = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                PublicityEntity fe = this.BaseRepository().FindEntity(keyValue);
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
