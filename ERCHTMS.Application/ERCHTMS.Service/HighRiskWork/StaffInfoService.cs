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
using System.Data.Common;
using ERCHTMS.Entity.BaseManage;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.LllegalManage;

namespace ERCHTMS.Service.HighRiskWork
{
    /// <summary>
    /// 描 述：任务分配人员
    /// </summary>
    public class StaffInfoService : RepositoryFactory<StaffInfoEntity>, StaffInfoIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<StaffInfoEntity> GetList(string queryJson)
        {
            var queryParam = JObject.Parse(queryJson);
            var parameter = new List<DbParameter>();
            string sql = "select * from bis_staffinfo where  taskshareid= @taskshareid";
            parameter.Add(DbParameters.CreateDbParameter("@taskshareid", queryParam["taskshareid"].ToString()));
            if (!queryParam["teamid"].IsEmpty())//班组id
            {
                sql += " and pteamid= @teamid";
                parameter.Add(DbParameters.CreateDbParameter("@pteamid", queryParam["teamid"].ToString()));
            }
            if (!queryParam["tasklevel"].IsEmpty())
            {
                sql += " and tasklevel=@tasklevel";
                parameter.Add(DbParameters.CreateDbParameter("@tasklevel", queryParam["tasklevel"].ToString()));
            }
            if (!queryParam["staffid"].IsEmpty())
            {
                sql += " and staffid=@staffid";
                parameter.Add(DbParameters.CreateDbParameter("@staffid", queryParam["staffid"].ToString()));
            }
            sql += " order by createdate desc";
            return this.BaseRepository().FindList(sql, parameter.ToArray()).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public StaffInfoEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// 获取监督任务列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public DataTable GetDataTable(Pagination page, string queryJson)
        {
            DatabaseType dataTye = DatabaseType.Oracle;
            #region 查表
            page.p_kid = "a.id";
            page.p_fields = @"a.taskshareid,a.dataissubmit,a.supervisestate,a.pteamname,a.pteamid,a.pteamcode,a.taskusername,a.taskuserid,a.sumtimestr,a.createdate,a.pstarttime,a.pendtime,b.id as workid,b.workname,b.workinfotype,b.workinfotypeid,b.workdeptname,b.workdeptcode,b.workdeptid,b.workstarttime,b.workendtime,b.workareaname,b.workplace,b.handtype";
            page.p_tablename = @" bis_staffinfo a left join bis_superviseworkinfo b on a.workinfoid=b.id";
            page.conditionJson = "1=1";
            var queryParam = queryJson.ToJObject();
            //任务分配id
            if (!queryParam["taskshareid"].IsEmpty())
            {
                page.conditionJson += string.Format(" and a.taskshareid='{0}'", queryParam["taskshareid"].ToString());
            }
            //监督单位id
            if (!queryParam["teamid"].IsEmpty())
            {
                page.conditionJson += string.Format(" and pteamid='{0}'", queryParam["teamid"].ToString());
            }
            //监督级别
            if (!queryParam["tasklevel"].IsEmpty())
            {
                page.conditionJson += string.Format(" and tasklevel='{0}'", queryParam["tasklevel"].ToString());
            }
            //是否提交
            if (!queryParam["dataissubmit"].IsEmpty())
            {
                page.conditionJson += string.Format(" and dataissubmit='{0}'", queryParam["dataissubmit"].ToString());
            }
            //查询下级
            if (!queryParam["staffid"].IsEmpty())
            {
                page.conditionJson += string.Format(" and staffid='{0}'", queryParam["staffid"].ToString());
            }
            //监督状态
            if (!queryParam["supervisestate"].IsEmpty())
            {
                page.conditionJson += string.Format(" and supervisestate='{0}'", queryParam["supervisestate"].ToString());
            }
            //作业开始时间
            if (!queryParam["workstarttime"].IsEmpty())
            {
                string from = queryParam["workstarttime"].ToString().Trim();
                page.conditionJson += string.Format(" and b.workstarttime>=to_date('{0}','yyyy-mm-dd')", from);
            }
            //作业结束时间
            if (!queryParam["workendtime"].IsEmpty())
            {
                string to = Convert.ToDateTime(queryParam["workendtime"].ToString().Trim()).AddDays(1).ToString("yyyy-MM-dd");
                page.conditionJson += string.Format(" and b.workendtime<=to_date('{0}','yyyy-mm-dd')", to);
            }
            //作业单位
            if (!queryParam["workdeptid"].IsEmpty())
            {
                page.conditionJson += string.Format(" and b.workdeptid='{0}'", queryParam["workdeptid"].ToString());
            }
            //旁站监督员
            if (!queryParam["taskuserid"].IsEmpty())
            {
                page.conditionJson += string.Format(" and  ','||taskuserid||',' like '%,{0},%'", queryParam["taskuserid"].ToString());
            }
            //旁站开始时间
            if (!queryParam["pstarttime"].IsEmpty())
            {
                string from = queryParam["pstarttime"].ToString().Trim();
                page.conditionJson += string.Format(" and a.pstarttime>=to_date('{0}','yyyy-mm-dd')", from);
            }
            //旁站结束时间
            if (!queryParam["pendtime"].IsEmpty())
            {
                string to = Convert.ToDateTime(queryParam["pendtime"].ToString().Trim()).AddDays(1).ToString("yyyy-MM-dd");
                page.conditionJson += string.Format(" and a.pendtime<=to_date('{0}','yyyy-mm-dd')", to);
            }
            #endregion
            var data = this.BaseRepository().FindTableByProcPager(page, dataTye);
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
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            try
            {
                db.Delete<StaffInfoEntity>(keyValue);
                db.Delete<StaffInfoEntity>(t => t.StaffId.Equals(keyValue));
                db.Delete<HTBaseInfoEntity>(t => t.RELEVANCEID.Equals(keyValue));
                db.Delete<LllegalRegisterEntity>(t => t.RESEVERONE.Equals(keyValue));
                db.Commit();
            }
            catch (Exception)
            {
                db.Rollback();
                throw;
            }
            this.BaseRepository().Delete(keyValue);
        }

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public string SaveForm(string keyValue, StaffInfoEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                var sf = BaseRepository().FindEntity(keyValue);
                if (sf == null)
                {
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
            return entity.Id;
        }
        #endregion
    }
}
