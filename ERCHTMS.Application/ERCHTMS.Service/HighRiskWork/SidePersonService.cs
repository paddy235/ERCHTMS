using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data;
using System;
using ERCHTMS.Code;

namespace ERCHTMS.Service.HighRiskWork
{
    /// <summary>
    /// 描 述：旁站监督人员(高风险作业)
    /// </summary>
    public class SidePersonService : RepositoryFactory<SidePersonEntity>, SidePersonIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<SidePersonEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return this.BaseRepository().FindList(pagination);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SidePersonEntity> GetList(string queryJson)
        {
            //return this.BaseRepository().IQueryable().ToList();
            return this.BaseRepository().FindList(" select * from BIS_SIDEPERSON where 1=1 " + queryJson).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SidePersonEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageDataTable(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["keyword"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and SideUserName like '%{0}%'", queryParam["keyword"].ToString());
            }
            //查询部门树
            if (!queryParam["deptcode"].IsEmpty() && !queryParam["deptid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and sideuserdeptid in(select departmentid from base_department  where encode like '{0}%' union select b.departmentid from epg_outsouringengineer  a left join base_department b on a.outprojectid=b.departmentid  where  a.engineerletdeptid='{1}')", queryParam["deptcode"].ToString(), queryParam["deptid"].ToString());
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
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
        public void SaveForm(string keyValue, SidePersonEntity entity)
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


        #region 验证数据
        /// <summary>
        /// 旁站监督人员不能重复(当前机构)
        /// </summary>
        /// <returns></returns>
        public bool ExistSideUser(string userid)
        {
            string ownorgcode = OperatorProvider.Provider.Current().OrganizeCode;
            var expression = LinqExtensions.True<SidePersonEntity>();
            expression = expression.And(t => t.SideUserId == userid).And(t => t.CreateUserOrgCode == ownorgcode);
            return this.BaseRepository().IQueryable(expression).Count() == 0 ? true : false;
        }
        #endregion
    }
}
