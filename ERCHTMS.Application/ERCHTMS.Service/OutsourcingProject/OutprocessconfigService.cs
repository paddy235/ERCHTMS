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
using System.Linq.Expressions;
using System;

namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// 描 述：外包流程配置表
    /// </summary>
    public class OutprocessconfigService : RepositoryFactory<OutprocessconfigEntity>, OutprocessconfigIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<OutprocessconfigEntity> GetList()
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        public DataTable GetPageListJson(Pagination pagination, string queryJson) {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            if (!queryParam["modulename"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.modulename like '%{0}%'", queryParam["modulename"].ToString());
            }
            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return dt;
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public OutprocessconfigEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 判断该电厂是否存在该模块的配置
        /// </summary>
        /// <param name="deptid">电厂ID</param>
        /// <param name="moduleCode">模块Code</param>
        /// <returns>0:不存在 >0 存在</returns>
        public int IsExistByModuleCode(string deptid, string moduleCode) {
            var sql = string.Format(@"select id from epg_outprocessconfig c where c.deptid='{0}' and c.modulecode='{1}'", deptid, moduleCode);
            var dt = this.BaseRepository().FindTable(sql);

            return dt.Rows.Count;
        }

        public OutprocessconfigEntity GetEntityByModuleCode(string deptid, string moduleCode) {
            string sql = string.Format(@"select t.* from wf_schemecontent a left join epg_outprocessconfig t on a.id=t.recid where a.wfschemeinfoid='{0}' and t.modulecode='{1}'  order by t.modulecode", deptid, moduleCode);
            return this.BaseRepository().FindList(sql).FirstOrDefault();
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
        public void SaveForm(string keyValue, OutprocessconfigEntity entity)
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
        /// 删除关联数据
        /// </summary>
        /// <param name="recid"></param>
        public void DeleteLinkData(string recid)
        {
            try
            {
                var expression = LinqExtensions.True<OutprocessconfigEntity>();
                expression = expression.And(t => t.RecId == recid);
                this.BaseRepository().Delete(expression);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
