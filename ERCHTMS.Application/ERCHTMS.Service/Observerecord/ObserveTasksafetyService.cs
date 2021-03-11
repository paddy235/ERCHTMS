using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using ERCHTMS.Entity.Observerecord;
using ERCHTMS.IService.Observerecord;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Service.Observerecord
{
   public class ObserveTasksafetyService : RepositoryFactory<ObserveTasksafetyEntity>, ObserveTasksafetyIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<ObserveTasksafetyEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        public DataTable GetSafetyPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            if (!queryParam["type"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.observetype='{0}'", queryParam["type"].ToString());
            }
            if (!queryParam["issafety"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.issafety='{0}'", queryParam["issafety"].ToString());
            }
            if (!queryParam["recordid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.recordid='{0}'", queryParam["recordid"].ToString());
            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// 根据类型和行为获取列表
        /// </summary>
        /// <param name="issafety">是否安全0-安全行为 1 -不安全行为</param>
        /// <param name="type">观察类别-7大类</param>
        /// <param name="recordid">观察记录Id</param>
        /// <returns></returns>
        public DataTable GetDataByType(string issafety, string type, string recordid)
        {
            string sql = string.Format(@"select t.issafety,t.id,
                                       t.observetype,t.observetypename,t.behaviordesc,
                                       t.recordid,t.immediatecause,t.remotecause,
                                       t.reformdeadline,t.personresponsibleid,t.personresponsible,
                                       t.preventivemeasures,t.remedialaction from bis_observetasksafety t ");
            string strWhere = " where 1=1 ";
            if (!string.IsNullOrWhiteSpace(recordid))
            {
                strWhere += string.Format(" and t.recordid='{0}'", recordid);
            }
            if (!string.IsNullOrWhiteSpace(type))
            {
                strWhere += string.Format(" and t.observetypename='{0}'", type);
            }
            if (!string.IsNullOrWhiteSpace(issafety))
            {
                strWhere += string.Format(" and t.issafety={0}", issafety);
            }
            sql = sql + strWhere;
            return this.BaseRepository().FindTable(sql);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ObserveTasksafetyEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, ObserveTasksafetyEntity entity)
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


        public void SaveForm(string recordId, List<ObserveTasksafetyEntity> entity)
        {
            var safetyNum = this.BaseRepository().ExecuteBySql(string.Format("select count(id) from bis_observetasksafety t where t.recordid='{0}'", recordId));
            if (safetyNum > 0)
            {
                this.BaseRepository().ExecuteBySql(string.Format("delete from bis_observetasksafety t where t.recordid='{0}'", recordId));
            }
            for (int i = 0; i < entity.Count; i++)
            {
                entity[i].Create();
            }
            this.BaseRepository().Insert(entity);
            //if (!string.IsNullOrEmpty(keyValue))
            //{
            //    entity.Modify(keyValue);
            //    this.BaseRepository().Update(entity);
            //}
            //else
            //{
            //    entity.Create();
            //    this.BaseRepository().Insert(entity);
            //}
        }
        #endregion
    }
}
