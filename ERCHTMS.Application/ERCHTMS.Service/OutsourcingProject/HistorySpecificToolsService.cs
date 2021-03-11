using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Data;

namespace ERCHTMS.Service.OutsourcingProject
{
    public class HistorySpecificToolsService : RepositoryFactory<HistorySpecificToolsEntity>, HistorySpecificToolsIService
    {
        public DataTable GetHistoryPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();

            if (!queryParam["toolsid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" t.toolsid='{0}' ", queryParam["toolsid"].ToString());
            }

            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return dt;
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HistorySpecificToolsEntity> GetList(string queryJson)
        {
            //return this.BaseRepository().IQueryable().ToList();
            return this.BaseRepository().FindList(" select * from EPG_HISTORYSPECIFICTOOLS where 1=1 " + queryJson).ToList();
        }
        public HistorySpecificToolsEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
    }
}
