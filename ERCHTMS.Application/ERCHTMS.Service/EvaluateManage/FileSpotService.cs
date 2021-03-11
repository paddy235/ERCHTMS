using ERCHTMS.Entity.EvaluateManage;
using ERCHTMS.IService.EvaluateManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Code;
using BSFramework.Data;
using BSFramework.Util.Extension;
using System;
using BSFramework.Util;

namespace ERCHTMS.Service.EvaluateManage
{
    /// <summary>
    /// 描 述：文件识别
    /// </summary>
    public class FileSpotService : RepositoryFactory<FileSpotEntity>, FilespotIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<FileSpotEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return this.BaseRepository().FindList(pagination);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<FileSpotEntity> GetList(string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;
            if (queryJson != null && queryJson != "")
            {

                var queryParam = queryJson.ToJObject();
                var expression = LinqExtensions.True<FileSpotEntity>();
                
                if (!queryParam["DeptName"].IsEmpty())
                {
                    string DeptName = queryParam["DeptName"].ToString();
                    expression = expression.And(t => t.DeptName == DeptName);
                }
                return this.BaseRepository().IQueryable(expression).ToList();
            }
            else
            {
                return this.BaseRepository().IQueryable().ToList();
            }
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public FileSpotEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, FileSpotEntity entity)
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
