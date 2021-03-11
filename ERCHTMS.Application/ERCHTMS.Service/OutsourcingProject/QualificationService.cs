using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Data;
using BSFramework.Util;

namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// 描 述：资质证件
    /// </summary>
    public class QualificationService : RepositoryFactory<QualificationEntity>, QualificationIService
    {
        /// <summary>
        /// 获取资质证件列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetZzzjPageJson(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            if (!string.IsNullOrWhiteSpace(queryParam["InfoId"].ToString()))
            {
                pagination.conditionJson += string.Format(" and InfoId  = '{0}'", queryParam["InfoId"].ToString());
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);

        }
        public IEnumerable<QualificationEntity> GetList()
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取资质证件实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public QualificationEntity GetZzzjFormJson(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 保存资质证件表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveZzzjForm(string keyValue, QualificationEntity entity) {
            entity.ID = keyValue;
            if (!string.IsNullOrWhiteSpace(keyValue))
            {
                QualificationEntity e = this.BaseRepository().FindEntity(keyValue);
                if (e != null)
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
            else {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
            
        }

        /// <summary>
        /// 删除资质证件信息
        /// </summary>
        /// <param name="keyValue"></param>
        public void RemoveZzzjForm(string keyValue) {
            this.BaseRepository().Delete(keyValue);
        }
    }
}
