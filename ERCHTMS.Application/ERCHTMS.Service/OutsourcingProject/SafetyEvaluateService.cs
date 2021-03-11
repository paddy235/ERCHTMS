using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;

namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// 描 述：安全评价
    /// </summary>
    public class SafetyEvaluateService : RepositoryFactory<SafetyEvaluateEntity>, SafetyEvaluateIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public DataTable GetEntity(string keyValue)
        {
            string sql = string.Format(@"select t.id,r.ID engid,r.ENGINEERNAME,
r.ENGINEERCODE,r.ENGINEERTYPE,r.ENGINEERAREA,r.ENGINEERLEVEL,r.ENGINEERLETDEPT,r.ENGINEERCONTENT,r.ENGINEERAREANAME as EngAreaName,
t.SiteManagementScore,t.QualityScore,t.ProjectProgressScore,t.FieldServiceScore,t.Evaluator,t.EvaluatorId,t.EvaluationTime,t.EvaluationScore
 from EPG_SafetyEvaluate t left join EPG_OutSouringEngineer r on t.projectid=r.id  where t.id='{0}'", keyValue);
            DataTable data = this.BaseRepository().FindTable(sql);
            return data;
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
        public void SaveForm(string keyValue, SafetyEvaluateEntity entity)
        {
            var res = DbFactory.Base().BeginTrans();
            try
            {
                entity.ID = keyValue;
                if (!string.IsNullOrEmpty(keyValue))
                {
                    SafetyEvaluateEntity se = this.BaseRepository().FindEntity(keyValue);
                    if (se == null)
                    {
                        entity.Create();
                        res.Insert<SafetyEvaluateEntity>(entity);
                    }
                    else
                    {
                        entity.Modify(keyValue);
                        res.Update<SafetyEvaluateEntity>(entity);
                    }
                    //Repository<OutsouringengineerEntity> ourEngineer = new Repository<OutsouringengineerEntity>(DbFactory.Base());
                    //OutsouringengineerEntity engineerEntity = ourEngineer.FindEntity(entity.PROJECTID);
                    //if (entity.ISSEND == "1")//提交时更新工程状态为'已完工'
                    //{
                    //    engineerEntity.ENGINEERSTATE = "003";
                    //    res.Update<OutsouringengineerEntity>(engineerEntity);
                    //}
                }
                else
                {
                    entity.Create();
                    res.Insert<SafetyEvaluateEntity>(entity);
                }
                
            }
            catch (System.Exception)
            {

                res.Rollback();
            }
            res.Commit();
        }
        #endregion


        public IEnumerable<SafetyEvaluateEntity> GetList()
        {
            return this.BaseRepository().IQueryable().ToList();
        }
    }
}
