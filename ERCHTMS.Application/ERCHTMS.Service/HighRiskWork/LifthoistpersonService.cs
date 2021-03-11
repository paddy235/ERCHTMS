using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using System.Data;
using System;

namespace ERCHTMS.Service.HighRiskWork
{
    /// <summary>
    /// 描 述：起重吊装作业操作人员表
    /// </summary>
    public class LifthoistpersonService : RepositoryFactory<LifthoistpersonEntity>, LifthoistpersonIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<LifthoistpersonEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }


        /// <summary>
        /// 获取起重吊装相关人员信息
        /// </summary>
        /// <param name="workid"></param>
        /// <returns></returns>
        public IEnumerable<LifthoistpersonEntity> GetRelateList(string workid)
        {
            return this.BaseRepository().IQueryable(t => t.RecId == workid).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public LifthoistpersonEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                if (queryJson.Length > 0)
                {
                    var queryParam = queryJson.ToJObject();
                }
                return this.BaseRepository().FindTableByProcPager(pagination, DbHelper.DbType);
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }


        /// <summary>
        /// 证件号不能重复
        /// </summary>
        /// <param name="CertificateNum">证件号</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistCertificateNum(string CertificateNum, string keyValue)
        {
            
            if (string.IsNullOrEmpty(CertificateNum))
            {
                return true;
            }
            string sql = string.Format("select a.id from BIS_LIFTHOISTJOB a left join BIS_LIFTHOISTPERSON b on a.id=b.recid where b.certificatenum='{0}'", CertificateNum);
            if (!string.IsNullOrEmpty(keyValue))
            {
                sql += string.Format(" and b.id !='{0}'", keyValue);
            }
            return this.BaseRepository().FindTable(sql).Rows.Count == 0 ? true : false;
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
        /// 删除起重吊装关联人员信息
        /// </summary>
        /// <param name="WorkId"></param>
        public void RemoveFormByWorkId(string WorkId)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            try
            {
                db.Delete<LifthoistpersonEntity>(t => t.RecId.Equals(WorkId));
                db.Commit();
            }
            catch (Exception ex)
            {
                db.Rollback();
                throw;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, LifthoistpersonEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                LifthoistpersonEntity se = this.BaseRepository().FindEntity(keyValue);
                if (se == null)
                {
                    entity.Id = keyValue;
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                }
                else
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
                
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
