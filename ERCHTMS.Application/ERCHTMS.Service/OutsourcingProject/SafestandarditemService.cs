using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// 描 述：安全考核内容表
    /// </summary>
    public class SafestandarditemService : RepositoryFactory<SafestandarditemEntity>, SafestandarditemIService
    {
        #region 获取数据

        /// <summary>
        /// 列表分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //上级节点ID
            if (!queryParam["parentId"].IsEmpty())
            {
                string parentId = queryParam["parentId"].ToString();
                pagination.conditionJson += string.Format(" and parentId = '{0}'", parentId);
            }

            //标准编码
            if (!queryParam["enCode"].IsEmpty())
            {
                string enCode = queryParam["enCode"].ToString();
                pagination.conditionJson += string.Format(" and STCODE like '{0}%'", enCode);
            }
            //查询关键字
            if (!queryParam["keyWord"].IsEmpty())
            {
                string keyWord = queryParam["keyWord"].ToString();
                pagination.conditionJson += string.Format(" and (content like '%{0}%' or require like '%{0}%')", keyWord.Trim());
            }
            //如果是来自安全检查中的选择
            if (!queryParam["stIds"].IsEmpty())
            {
                string ids = queryParam["stIds"].ToString().Replace("[", "").Replace("]", "").Replace("\r\n", "").Replace("\"", "'");
                pagination.conditionJson += string.Format(" and stid in({0})", System.Text.RegularExpressions.Regex.Replace(ids, @"\s", ""));
                return this.BaseRepository().FindTable(string.Concat("select ", pagination.p_kid, ",", pagination.p_fields, " from ", pagination.p_tablename, " where ", pagination.conditionJson, "order by ", pagination.sidx, " ", pagination.sord));
            }
            else
            {
                return this.BaseRepository().FindTableByProcPager(pagination, dataType);
            }

        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SafestandarditemEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SafestandarditemEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, SafestandarditemEntity entity)
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
