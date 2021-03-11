using BSFramework.Data.Repository;
using ERCHTMS.Entity.SaftyCheck;
using ERCHTMS.IService.SaftyCheck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Service.SaftyCheck
{
    /// <summary>
    /// 描 述：安全检查内容
    /// </summary>
    public class SaftyCheckContentService : RepositoryFactory<SaftyCheckContentEntity>, SaftyCheckContentIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SaftyCheckContentEntity> GetList(string queryJson)
        {
            IEnumerable<SaftyCheckContentEntity> content = this.BaseRepository().IQueryable().ToList();
            return content;
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, SaftyCheckContentEntity entity)
        {
            entity.Create();
            this.BaseRepository().Insert(entity);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                this.BaseRepository().ExecuteBySql("delete BIS_SAFTYCONTENT where recid='" + keyValue + "'");
            }
        }
        #endregion
    }
}
