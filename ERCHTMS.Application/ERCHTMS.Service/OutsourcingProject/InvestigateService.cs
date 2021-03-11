using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using System.Data;
using Newtonsoft.Json;

namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// 描 述：审查配置表
    /// </summary>
    public class InvestigateService : RepositoryFactory<InvestigateEntity>, InvestigateIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<InvestigateEntity> GetList(string orginezeId)
        {
            return this.BaseRepository().IQueryable(p => p.ORGINEZEID == orginezeId).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public InvestigateEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        public DataTable GetAllFactory()
        {
            return this.BaseRepository().FindTable(string.Format("select t.departmentid,t.fullname from Base_Department t where t.nature='{0}' ", "厂级"));
        }
        #endregion

        #region 获取审查数据
        /// <summary>
        /// 获取审查数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetInvestigatePageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return dt;
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
        public void SaveForm(string keyValue, InvestigateEntity entity)
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