using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.HighRiskWork
{
    /// <summary>
    /// 描 述：监督任务检查项目
    /// </summary>
    public class SideCheckProjectService : RepositoryFactory<SideCheckProjectEntity>, SideCheckProjectIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SideCheckProjectEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SideCheckProjectEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 大项检查项目信息
        /// </summary>
        /// <param name="approveid"></param>
        /// <returns></returns>
        public IEnumerable<SideCheckProjectEntity> GetBigCheckInfo()
        {
            var expression = LinqExtensions.True<SideCheckProjectEntity>();
            expression = expression.And(t => t.ParentId == "-1");
            return this.BaseRepository().IQueryable(expression).OrderBy(t => t.CheckNumber).ToList();
        }

        /// <summary>
        /// 根据大项检查项目id获取小项检查项目
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public IEnumerable<SideCheckProjectEntity> GetAllSmallCheckInfo(string parentid)
        {
            var expression = LinqExtensions.True<SideCheckProjectEntity>();
            expression = expression.And(t => t.ParentId != "-1");
            if (!string.IsNullOrEmpty(parentid))
            {
                expression = expression.And(t => t.ParentId == parentid);
            }
            return this.BaseRepository().IQueryable(expression).OrderBy(t => t.CheckNumber).ToList();
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
        public void SaveForm(string keyValue, SideCheckProjectEntity entity)
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
