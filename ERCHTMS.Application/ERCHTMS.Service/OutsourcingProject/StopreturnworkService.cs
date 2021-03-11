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
using System;
using ERCHTMS.Code;

namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// 描 述：停复工管理表
    /// </summary>
    public class StopreturnworkService : RepositoryFactory<StopreturnworkEntity>, StopreturnworkIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<StopreturnworkEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public StopreturnworkEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }


        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            Operator currUser = OperatorProvider.Provider.Current();
            //查询条件
            if (!queryParam["proName"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and  b.fullname like'%{0}%' ", queryParam["proName"].ToString());
            }
            if (!queryParam["engineerName"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and  e.engineername like'%{0}%' ", queryParam["engineerName"].ToString());
            }
            if (!queryParam["SendDept"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and e.engineerletdeptid='{0}' ", queryParam["SendDept"].ToString());
            }
            if (!queryParam["orgCode"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.createuserorgcode='{0}' ", queryParam["orgCode"].ToString());
            }
            //if (!queryParam["StartTime"].IsEmpty())
            //{
            //    pagination.conditionJson += string.Format(" and s.stoptime>='{0}' ", queryParam["StartTime"].ToString());
            //}
            //if (!queryParam["EndTime"].IsEmpty())
            //{
            //    pagination.conditionJson += string.Format(" and s.stoptime<='{0}' ", queryParam["EndTime"].ToString());
            //}
            if (!queryParam["StartTime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.stoptime>=to_date('{0}','yyyy-MM-dd hh24:mi:ss') ", queryParam["StartTime"].ToString());
            }
            if (!queryParam["EndTime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.stoptime<=to_date('{0}','yyyy-MM-dd hh24:mi:ss') ", Convert.ToDateTime(queryParam["EndTime"]).AddDays(1).ToString("yyyy-MM-dd"));
            }
            //pagination.conditionJson += string.Format(" or (t.createuserid='{0}' ", currUser.UserId);

            ////查询条件
            //if (!queryParam["proName"].IsEmpty())
            //{
            //    pagination.conditionJson += string.Format(" and  b.fullname like'%{0}%' ", queryParam["proName"].ToString());
            //}
            //if (!queryParam["engineerName"].IsEmpty())
            //{
            //    pagination.conditionJson += string.Format(" and  e.engineername like'%{0}%' ", queryParam["engineerName"].ToString());
            //}
            //if (!queryParam["SendDept"].IsEmpty())
            //{
            //    pagination.conditionJson += string.Format(" and b.senddeptid='{0}' ", queryParam["SendDept"].ToString());
            //}
            //if (!queryParam["orgCode"].IsEmpty())
            //{
            //    pagination.conditionJson += string.Format(" and t.createuserorgcode='{0}' ", queryParam["orgCode"].ToString());
            //}
            ////if (!queryParam["StartTime"].IsEmpty())
            ////{
            ////    pagination.conditionJson += string.Format(" and s.stoptime>='{0}' ", queryParam["StartTime"].ToString());
            ////}
            ////if (!queryParam["EndTime"].IsEmpty())
            ////{
            ////    pagination.conditionJson += string.Format(" and s.stoptime<='{0}' ", queryParam["EndTime"].ToString());
            ////}
            //if (!queryParam["StartTime"].IsEmpty())
            //{
            //    pagination.conditionJson += string.Format(" and t.stoptime>=to_date('{0}','yyyy-MM-dd hh24:mi:ss') ", queryParam["StartTime"].ToString());
            //}
            //if (!queryParam["EndTime"].IsEmpty())
            //{
            //    pagination.conditionJson += string.Format(" and t.stoptime<=to_date('{0}','yyyy-MM-dd hh24:mi:ss') ", Convert.ToDateTime(queryParam["EndTime"]).AddDays(1).ToString("yyyy-MM-dd"));
            //}
            //pagination.conditionJson += string.Format(" ) ");
            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
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
            var res = DbFactory.Base().BeginTrans();
            try
            {
                StopreturnworkEntity se = this.BaseRepository().FindEntity(keyValue);
                Repository<OutsouringengineerEntity> ourEngineer = new Repository<OutsouringengineerEntity>(DbFactory.Base());
                OutsouringengineerEntity engineerEntity = ourEngineer.FindEntity(se.OUTENGINEERID);
                engineerEntity.STOPRETURNSTATE = "0";
                res.Update<OutsouringengineerEntity>(engineerEntity);
                res.Delete<StopreturnworkEntity>(keyValue);
                res.Commit();
            }
            catch (Exception)
            {
                res.Rollback();
            }
           
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, StopreturnworkEntity entity)
        {
            //开始事务
            var res = DbFactory.Base().BeginTrans();
            try
            {
                entity.ID = keyValue;
                StopreturnworkEntity se = this.BaseRepository().FindEntity(keyValue);
                if (se == null)
                {
                    entity.Create();
                    res.Insert<StopreturnworkEntity>(entity);
                }
                else
                {
                    entity.Modify(keyValue);
                    res.Update<StopreturnworkEntity>(entity);
                }
                Repository<OutsouringengineerEntity> ourEngineer = new Repository<OutsouringengineerEntity>(DbFactory.Base());
                OutsouringengineerEntity engineerEntity = ourEngineer.FindEntity(entity.OUTENGINEERID);
                engineerEntity.STOPRETURNSTATE = "1";
                res.Update<OutsouringengineerEntity>(engineerEntity);
                res.Commit();
            }
            catch (System.Exception)
            {

                res.Rollback();
            }

        }
        #endregion
    }
}
