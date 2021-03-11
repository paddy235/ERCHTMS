using ERCHTMS.Entity.RoutineSafetyWork;
using ERCHTMS.IService.RoutineSafetyWork;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Service.RoutineSafetyWork
{
    /// <summary>
    /// 描 述：文件管理
    /// </summary>
    public class FileManageService : RepositoryFactory<FileManageEntity>, FileManageIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<FileManageEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public FileManageEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, FileManageEntity entity)
        {
            bool b = true;
            entity.ID = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                FileManageEntity se = this.BaseRepository().FindEntity(keyValue);
                if (se != null)
                {
                    b = false;
                }
            }
            if (b)
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
            else {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
        }
        #endregion
    }
}
