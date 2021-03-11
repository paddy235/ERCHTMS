using ERCHTMS.Entity.StandardSystem;
using ERCHTMS.IService.StandardSystem;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Code;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.StandardSystem
{
    /// <summary>
    /// 描 述：收藏标准
    /// </summary>
    public class StorestandardService : RepositoryFactory<StorestandardEntity>, StorestandardIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<StorestandardEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public StorestandardEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 根据标准ID判断是否有收藏
        /// </summary>
        /// <param name="standardID"></param>
        /// <returns></returns>
        public int GetStoreByStandardID(string standardID)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            return this.BaseRepository().FindObject(string.Format("select count(1) from hrs_storestandard where userid='{0}' and standardid='{1}'", user.UserId, standardID)).ToInt();
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
        public void SaveForm(string keyValue, StorestandardEntity entity)
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
