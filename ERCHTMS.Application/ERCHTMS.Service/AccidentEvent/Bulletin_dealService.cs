using ERCHTMS.Entity.AccidentEvent;
using ERCHTMS.IService.AccidentEvent;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System;

namespace ERCHTMS.Service.AccidentEvent
{
    /// <summary>
    /// 描 述：事故事件调查处理
    /// </summary>
    public class Bulletin_dealService : RepositoryFactory<Bulletin_dealEntity>, IBulletin_deaIService
    {
        #region 获取数据

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="condition">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<Bulletin_dealEntity> GetListForCon(Expression<Func<Bulletin_dealEntity, bool>> condition)
        {

            return this.BaseRepository().IQueryable(condition).ToList();
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<Bulletin_dealEntity> GetList(string queryJson)
        {
            return this.BaseRepository().FindList(" select * from aem_Bulletin_deal where 1=1 " + queryJson).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public Bulletin_dealEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, Bulletin_dealEntity entity)
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
