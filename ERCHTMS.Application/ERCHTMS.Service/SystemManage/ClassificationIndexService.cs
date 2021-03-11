using ERCHTMS.Entity.SystemManage;
using ERCHTMS.IService.SystemManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Service.SystemManage
{
    /// <summary>
    /// 描 述：分项指标项目表
    /// </summary>
    public class ClassificationIndexService : RepositoryFactory<ClassificationIndexEntity>, IClassificationIndexService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<ClassificationIndexEntity> GetList(string classificationId)
        {
            return this.BaseRepository().IQueryable().Where(p => p.ClassificationId == classificationId).OrderBy(p => p.IndexCode).ToList();
        }

        public IEnumerable<ClassificationIndexEntity> GetListByOrganizeId(string organizeId) 
        {
            return this.BaseRepository().IQueryable().Where(p => p.AffiliatedOrganizeId == organizeId).OrderBy(p => p.IndexCode).ToList();
        }

        public ClassificationIndexEntity GetEntity(string organizeId , string classificationCode, string indexCode) 
        {
            return this.BaseRepository().IQueryable().Where(p => p.AffiliatedOrganizeId == organizeId && p.ClassificationCode == classificationCode && p.IndexCode == indexCode).ToList().FirstOrDefault();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ClassificationIndexEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, ClassificationIndexEntity entity)
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