using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Data.Repository;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Data;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.OutsourcingProject
{
    public class HistoryCertificateService : RepositoryFactory<HistoryCertificate>, HistoryCertificateIService
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HistoryCertificate> GetList(string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["UserId"].IsEmpty())
            {
                return this.BaseRepository().IQueryable().ToList().Where(x => x.HISUSERID == queryParam["UserId"].ToString());
            }
            return null;
        }

        public HistoryCertificate GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }

        public void SaveForm(string keyValue, HistoryCertificate entity)
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
    }
}
