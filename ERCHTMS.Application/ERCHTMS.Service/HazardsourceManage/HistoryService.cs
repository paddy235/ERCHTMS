using ERCHTMS.Entity.HazardsourceManage;
using ERCHTMS.IService.HazardsourceManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Service.CommonPermission;
using BSFramework.Data;
using System.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System;

namespace ERCHTMS.Service.HazardsourceManage
{
    /// <summary>
    /// 描 述：历史记录
    /// </summary>
    public class HistoryService : RepositoryFactory<HistoryEntity>, IHistoryService
    {
        #region 获取数据


        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;

            if (queryJson.Length > 0)
            {
                var queryParam = queryJson.ToJObject();
                #region 查询条件
                //查询条件
                if (!queryParam["TimeEnd"].IsEmpty())
                {
                    string TimeEnd = queryParam["TimeEnd"].ToString();
                    TimeEnd = DateTime.Parse(TimeEnd).AddDays(1).ToString();
                    pagination.conditionJson += string.Format(" and CreateDate <= (select  to_date('{0}', 'yyyy-MM-dd') from dual)", TimeEnd);

                }
                //查询条件
                if (!queryParam["TimeStart"].IsEmpty())
                {
                    string TimeStart = queryParam["TimeStart"].ToString();
                    pagination.conditionJson += string.Format(" and CreateDate >= (select  to_date('{0}', 'yyyy-MM-dd') from dual)", TimeStart);
                }
                //查询条件
                if (!queryParam["DangerSourceName"].IsEmpty())
                {
                    string DangerSourceName = queryParam["DangerSourceName"].ToString();
                    pagination.conditionJson += string.Format(" and DangerSourceName like '%{0}%'", DangerSourceName);
                }

                #endregion


                #region 权限判断
                if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
                {
                    pagination = PermissionByCurrent.GetPermissionByCurrent2(pagination, queryParam["code"].ToString(), queryParam["isOrg"].ToString());
                }
                #endregion
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HistoryEntity> GetList(string queryJson)
        {
            return this.BaseRepository().FindList(" select * from hsd_history where 1=1 " + queryJson).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HistoryEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, HistoryEntity entity)
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

        public void Save(HistoryEntity entity)
        {
            this.BaseRepository().Insert(entity);
        }
        #endregion
    }
}
