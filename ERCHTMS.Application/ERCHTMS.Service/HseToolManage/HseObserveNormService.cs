using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.HseManage.ViewModel;
using ERCHTMS.Entity.HseToolMange;
using ERCHTMS.IService.HseToolMange;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Service.HseToolManage
{
    /// <summary>
    /// 安全观察内容标准
    /// </summary>
    public class HseObserveNormService : RepositoryFactory<HseObserveNormEntity>, HseObserveNormIService
    {

        /// <summary>
        /// 获取台账分页数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public List<HseObserveNormEntity> GetList()
        {
            return this.BaseRepository().IQueryable(x=>x.CREATEUSERNAME!=null).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HseObserveNormEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="keyValue"></param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void SaveForm(string keyValue, HseObserveNormEntity entity)
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
        /// 保存数据
        /// </summary>
        /// <param name="entity"></param>
        public void SaveFormList(List<HseObserveNormEntity> entity)
        {
            var db = this.BaseRepository().BeginTrans();
            try
            {
                foreach (var item in entity)
                {
                    if (!string.IsNullOrEmpty(item.Id))
                    {
                        item.Modify(item.Id);
                        db.Update(item);
                    }
                    else
                    {
                        item.Create();
                        db.Insert(item);
                    }
                }
                db.Commit();
            }
            catch (Exception)
            {

                db.Rollback();
            }

        }
    }
}
