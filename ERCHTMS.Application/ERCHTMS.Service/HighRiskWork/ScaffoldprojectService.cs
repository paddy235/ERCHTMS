using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System;
using System.Data.Common;
using BSFramework.Data;
using ERCHTMS.Entity.HighRiskWork.ViewModel;

namespace ERCHTMS.Service.HighRiskWork
{
    /// <summary>
    /// 描 述：1.脚手架验收项目
    /// </summary>
    public class ScaffoldprojectService : RepositoryFactory<ScaffoldprojectEntity>, ScaffoldprojectIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="scaffoldid">查询参数</param>
        /// <returns>返回列表</returns>
        public List<ScaffoldprojectEntity> GetList(string scaffoldid)
        {
            string sql = @"select * from bis_scaffoldproject where scaffoldid = @scaffoldid";
            return this.BaseRepository().FindList(sql, new DbParameter[]{
                    DbParameters.CreateDbParameter("@scaffoldid",scaffoldid)
            }).ToList();
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<ScaffoldprojectEntity> GetListByCondition(string queryJson)
        {
            return this.BaseRepository().FindList(string.Format("select * from bis_scaffoldproject where 1=1 " + queryJson)).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ScaffoldprojectEntity GetEntity(string keyValue)
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
        /// 跟据条件表达式删除
        /// </summary>
        /// <param name="condition"></param>
        public void RemoveForm(Expression<Func<ScaffoldprojectEntity, bool>> condition)
        {
            this.BaseRepository().Delete(condition);
        }

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, ScaffoldprojectEntity entity)
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
