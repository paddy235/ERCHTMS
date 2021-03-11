using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.IService.EmergencyPlatform;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util.Extension;
using BSFramework.Util;

namespace ERCHTMS.Service.EmergencyPlatform
{
    /// <summary>
    /// 描 述：应急演练记录步骤表
    /// </summary>
    public class DrillplanrecordstepService : RepositoryFactory<DrillplanrecordstepEntity>, DrillplanrecordstepIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<DrillplanrecordstepEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public DrillplanrecordstepEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }


        /// <summary>
        /// 根据recid获取步骤列表
        /// </summary>
        /// <param name="recid"></param>
        /// <returns></returns>
        public IList<DrillplanrecordstepEntity> GetListByRecid(string recid)
        {
            var list = this.BaseRepository().IQueryable().Where(x => x.DrillStepRecordId == recid).OrderBy(t => t.SortId).ToList();
            return list;
        }

        /// <summary>
        /// 应急记录步骤列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;

            if (!queryJson.IsEmpty())
            {
                var queryParam = queryJson.ToJObject();
                if (!queryParam["recid"].IsEmpty())
                {
                    pagination.conditionJson += " and drillsteprecordid = '" + queryParam["recid"].ToString() + "'";
                }
            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        #endregion

        #region 提交数据

        /// <summary>
        /// 根据关联ID删除数据
        /// </summary>
        /// <param name="recid"></param>
        public void RemoveFormByRecid(string recid)
        {
            string sql = "delete MAE_DRILLPLANRECORDSTEP where DRILLSTEPRECORDID='" + recid + "'";
            this.BaseRepository().ExecuteBySql(sql);
        }

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
        public void SaveForm(string keyValue, DrillplanrecordstepEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                DrillplanrecordstepEntity se = this.BaseRepository().FindEntity(keyValue);
                if (se == null)
                {
                    entity.Id = keyValue;
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
