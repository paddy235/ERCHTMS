using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.IService.EmergencyPlatform;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using System.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Code;

namespace ERCHTMS.Service.EmergencyPlatform
{
    /// <summary>
    /// 描 述：应急物资库表
    /// </summary>
    public class SuppliesfactoryService : RepositoryFactory<SuppliesfactoryEntity>, SuppliesfactoryIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SuppliesfactoryEntity> GetList(string queryJson)
        {
            Operator user = OperatorProvider.Provider.Current();
            return this.BaseRepository().IQueryable().Where(t => t.CreateUserOrgCode == user.OrganizeCode).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SuppliesfactoryEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 应急资源库列表
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
                if (!queryParam["name"].IsEmpty())
                {
                    pagination.conditionJson += " and name like '%" + queryParam["name"].ToString() + "%'";
                }
                if (!queryParam["suppliestype"].IsEmpty())
                {
                    pagination.conditionJson += " and suppliestype ='" + queryParam["suppliestype"].ToString() + "'";
                }
            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
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
        public void SaveForm(string keyValue, SuppliesfactoryEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                SuppliesfactoryEntity se = this.BaseRepository().FindEntity(keyValue);
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
