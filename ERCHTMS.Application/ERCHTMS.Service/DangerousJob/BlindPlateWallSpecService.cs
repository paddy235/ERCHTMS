using ERCHTMS.Entity.DangerousJob;
using ERCHTMS.IService.DangerousJob;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Service.DangerousJob
{
    /// <summary>
    /// 描 述：盲板作业盲板规格
    /// </summary>
    public class BlindPlateWallSpecService : RepositoryFactory<BlindPlateWallSpecEntity>, BlindPlateWallSpecIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<BlindPlateWallSpecEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public BlindPlateWallSpecEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, BlindPlateWallSpecEntity entity)
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
        /// 删除盲板抽堵作业相关规格
        /// </summary>
        /// <param name="RecId"></param>
        public void DeleteRec(string RecId)
        {
            this.BaseRepository().Delete(t => t.RecId == RecId);
        }
        #endregion
    }
}
