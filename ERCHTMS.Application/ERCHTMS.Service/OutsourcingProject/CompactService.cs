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
    /// 描 述：合同
    /// </summary>
    public class CompactService : RepositoryFactory<CompactEntity>, CompactIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = (DatabaseType)DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        public IEnumerable<CompactEntity> GetList() {
            return this.BaseRepository().IQueryable();
        }
        #region 获取工程下的合同信息
        /// <summary>
        /// 获取工程下的合同信息
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public List<CompactEntity> GetListByProjectId(string projectId)
        {
            try
            {
                List<CompactEntity> list = new List<CompactEntity>();
                list = this.BaseRepository().FindList(string.Format(@"select * from epg_compact where projectid ='{0}'  order by createdate", projectId)).ToList();
                return list;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根基工程Id获取合同期限
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public DataTable GetComoactTimeByProjectId(string projectId)
        {
            try
            {
               
                 return  this.BaseRepository().FindTable(string.Format(@"select max(compacteffectivedate) maxtime,min(compacttakeeffectdate) mintime from epg_compact where projectid ='{0}'  order by createdate", projectId));
                
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public DataTable GetEntity(string keyValue)
        {
            string sql = string.Format(@"SELECT T.FIRSTPARTY,T.SECONDPARTY,T.ID,R.ENGINEERNAME,
R.ENGINEERCODE,R.ENGINEERTYPE,R.ENGINEERAREA,R.ENGINEERLEVEL,R.ENGINEERAREANAME,
R.ENGINEERLETDEPT,R.ENGINEERCONTENT,T.COMPACTTAKEEFFECTDATE,
T.COMPACTEFFECTIVEDATE,T.COMPACTNO,T.PROJECTID,T.COMPACTMONEY,T.REMARK
 FROM EPG_COMPACT T LEFT JOIN EPG_OUTSOURINGENGINEER R ON T.PROJECTID=R.ID 
WHERE T.ID='{0}'", keyValue);
            DataTable data = this.BaseRepository().FindTable(sql);
            return data;
        }

        public object GetCompactProtocol(string keyValue)
        {
            DataTable Compact = new DataTable();//合同
            string sql = string.Format(@"select * from EPG_COMPACT t where t.ProjectID='{0}'", keyValue);
            Compact = this.BaseRepository().FindTable(sql);
            DataTable Protocol = new DataTable();//协议
            sql = string.Format(@"select * from EPG_Protocol t where t.ProjectID='{0}'", keyValue);
            Protocol = this.BaseRepository().FindTable(sql);
            DataTable SafetyEvaluate = new DataTable();//安全评价
            sql = string.Format(@"select * from EPG_SafetyEvaluate t where t.ProjectID='{0}'", keyValue);
            SafetyEvaluate = this.BaseRepository().FindTable(sql);
            var resultData = new
            {
                Compact = Compact,
                Protocol = Protocol,
                SafetyEvaluate = SafetyEvaluate
            };
            return resultData;
        }


        /// <summary>
        /// 获取最后一笔合同与协议信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public object GetLastCompactProtocol(string keyValue)
        {
            DataTable Compact = new DataTable();//合同
            string sql = string.Format(@"select * from (select * from EPG_COMPACT t where t.ProjectID='{0}' order by createdate desc) where rownum<=1", keyValue);
            Compact = this.BaseRepository().FindTable(sql);
            DataTable Protocol = new DataTable();//协议
            sql = string.Format(@"select * from (select * from EPG_Protocol t where t.ProjectID='{0}' order by createdate desc) where rownum<=1", keyValue);
            Protocol = this.BaseRepository().FindTable(sql);
            //DataTable SafetyEvaluate = new DataTable();//安全评价
            //sql = string.Format(@"select * from EPG_SafetyEvaluate t where t.ProjectID='{0}'", keyValue);
            //SafetyEvaluate = this.BaseRepository().FindTable(sql);
            var resultData = new
            {
                Compact = Compact,
                Protocol = Protocol
            };
            return resultData;
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
        public void SaveForm(string keyValue, CompactEntity entity)
        {
            entity.ID = keyValue;

            int count = 0;
            //开始事务
            var res = DbFactory.Base().BeginTrans();
            try
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    CompactEntity se = this.BaseRepository().FindEntity(keyValue);
                    if (se == null)
                    {
                        entity.Create();
                        count = res.Insert<CompactEntity>(entity);
                        //res.Commit();
                    }
                    else
                    {
                        entity.Modify(keyValue);
                        count = res.Update<CompactEntity>(entity);
                    }
                    #region 更新工程流程状态
                    Repository<StartappprocessstatusEntity> proecss = new Repository<StartappprocessstatusEntity>(DbFactory.Base());
                    StartappprocessstatusEntity startProecss = proecss.FindList(string.Format("select * from epg_startappprocessstatus t where t.outengineerid='{0}'", entity.PROJECTID)).ToList().FirstOrDefault();
                    startProecss.COMPACTSTATUS = "1";
                    res.Update<StartappprocessstatusEntity>(startProecss);
                    res.Commit();
                    #endregion
                }
                else
                {
                    entity.Create();
                    count = res.Insert<CompactEntity>(entity);
                    res.Commit();
                }

            }
            catch (System.Exception)
            {
                res.Rollback();
            }

        }
        #endregion
    }
}
