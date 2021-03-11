using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.OutsourcingProject
{
    public class HistoryPeopleService : RepositoryFactory<HistoryPeople>, HistoryPeopleIService
    {
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键Id</param>
        /// <returns></returns>
        public HistoryPeople GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="keyValue">主键Id</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }



        public void SaveForm(string keyValue, HistoryPeople entity)
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

        /// <summary>
        /// 获取人员历史记录分页显示
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetHistoryPageList(Pagination pagination, string queryJson) {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();

            if (!queryParam["hispeoplereviewid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" t.hispeoplereviewid='{0}' ", queryParam["hispeoplereviewid"].ToString());
            }

            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return dt;
        }
    }
}
