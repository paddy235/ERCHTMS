using ERCHTMS.Entity.SaftProductTargetManage;
using ERCHTMS.IService.SaftProductTargetManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data;

namespace ERCHTMS.Service.SaftProductTargetManage
{
    /// <summary>
    /// 描 述：安全生产责任书
    /// </summary>
    public class SafeProductDutyBookService : RepositoryFactory<SafeProductDutyBookEntity>, SafeProductDutyBookIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SafeProductDutyBookEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SafeProductDutyBookEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 获取安全目标责任书列表
        /// </summary>
        /// <param name="ProductId">安全目标id</param>
        /// <returns></returns>
        public IEnumerable<SafeProductDutyBookEntity> GetListByProductId(string productId)
        {
            return this.BaseRepository().IQueryable().ToList().Where(t => t.ProductId == productId);
        }

        /// <summary>
        /// 列表分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public IEnumerable<SafeProductDutyBookEntity> GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();
            if (!queryParam["productId"].IsEmpty())
            {
                string productid = queryParam["productId"].ToString();
                pagination.conditionJson += string.Format(" and ProductId='{0}'", productid);
            }
            else
            {
                pagination.conditionJson += string.Format(" and 1=2");
            }
            IEnumerable<SafeProductDutyBookEntity> list = this.BaseRepository().FindListByProcPager(pagination, dataType);
            return list;

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
        /// 根据安全生产目标ID删除数据
        /// </summary>
        /// <param name="planId">安全生产目标id</param>
        public int Remove(string productId)
        {
            return this.BaseRepository().ExecuteBySql(string.Format("delete from bis_safeproductdutybook where productid='{0}'", productId));
        }

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, SafeProductDutyBookEntity entity)
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
