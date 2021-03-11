using ERCHTMS.Entity.OccupationalHealthManage;
using ERCHTMS.IService.OccupationalHealthManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System;

namespace ERCHTMS.Service.OccupationalHealthManage
{
    /// <summary>
    /// 描 述：职业机构检测
    /// </summary>
    public class InspectionService : RepositoryFactory<InspectionEntity>, InspectionIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<InspectionEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        /// <summary>
        /// 存储过程分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageListByProc(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["condition"].IsEmpty() && !queryParam["keyword"].IsEmpty())
            {
                string condition = queryParam["condition"].ToString();
                string keyord = queryParam["keyword"].ToString();
                DateTime st = condition.ToDate();
                DateTime ed = keyord.ToDate();
                //自动判断日期大小
                if (ed > st)
                {
                    //根据时间范围查询
                    pagination.conditionJson += string.Format(" AND INSPECTIONTIME  >= TO_DATE('{0}','yyyy-mm-dd') AND INSPECTIONTIME<=TO_DATE('{1}','yyyy-mm-dd')", condition.Trim(), keyord.Trim());
                }
                else
                {
                    //根据时间范围查询
                    pagination.conditionJson += string.Format(" AND INSPECTIONTIME  >= TO_DATE('{0}','yyyy-mm-dd') AND INSPECTIONTIME<=TO_DATE('{1}','yyyy-mm-dd')", condition.Trim(), keyord.Trim());
                }


            }
            else if (!queryParam["condition"].IsEmpty())//只有开始时间
            {
                string condition = queryParam["condition"].ToString();
                //根据时间范围查询
                pagination.conditionJson += string.Format(" AND INSPECTIONTIME  >= TO_DATE('{0}','yyyy-mm-dd') ", condition.Trim());
            }
            else if (!queryParam["keyword"].IsEmpty())//只有开始时间
            {
                string keyord = queryParam["keyword"].ToString();
                //根据时间范围查询
                pagination.conditionJson += string.Format(" AND  INSPECTIONTIME<=TO_DATE('{0}','yyyy-mm-dd') ", keyord.Trim());
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public InspectionEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, InspectionEntity entity)
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
