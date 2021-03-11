using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Util.Extension;
using System.Data;
using BSFramework.Data;

namespace ERCHTMS.Service.HighRiskWork
{
    /// <summary>
    /// 描 述：已检查的检查项目
    /// </summary>
    public class TaskRelevanceProjectService : RepositoryFactory<TaskRelevanceProjectEntity>, TaskRelevanceProjectIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TaskRelevanceProjectEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TaskRelevanceProjectEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 根据监督任务获取已检查项目
        /// </summary>
        /// <param name="approveid"></param>
        /// <returns></returns>
        public IEnumerable<TaskRelevanceProjectEntity> GetEndCheckInfo(string superviseid)
        {
            var expression = LinqExtensions.True<TaskRelevanceProjectEntity>();
            if (!string.IsNullOrEmpty(superviseid))
            {
                expression = expression.And(t => t.SuperviseId == superviseid);
            }
            return this.BaseRepository().IQueryable(expression).ToList();
        }

        /// <summary>
        /// 根据检查项目id和监督任务id获取信息
        /// </summary>
        /// <returns></returns>
        public TaskRelevanceProjectEntity GetCheckResultInfo(string checkprojectid, string superviseid)
        {
            var expression = LinqExtensions.True<TaskRelevanceProjectEntity>();
            expression = expression.And(t => t.SuperviseId == superviseid).And(t => t.CheckProjectId == checkprojectid);
            return this.BaseRepository().IQueryable(expression).ToList().FirstOrDefault();
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageDataTable(Pagination pagination)
        {
            DatabaseType dataType = DbHelper.DbType;
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// 根据监督id获取隐患和违章信息
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetTaskHiddenInfo(string superviseid)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("hiddescribe");
            dt.Columns.Add("changemeasure");
            dt.Columns.Add("lllegaldescribe");
            dt.Columns.Add("reformmeasure");
            string strsql = string.Format("select hiddescribe,changemeasure from v_hiddenbasedata where relevanceId='{0}'", superviseid);
            DataTable dtstr = this.BaseRepository().FindTable(strsql);
            string breaksql = string.Format("select lllegaldescribe,reformmeasure from v_lllegalallbaseinfo where reseverone='{0}'", superviseid);
            DataTable dtbreak = this.BaseRepository().FindTable(breaksql);
            var hiddescribe = "";
            var changemeasure = "";
            var lllegaldescribe = "";
            var reformmeasure = "";
            int count = 0;
            for (int i = 0; i < dtstr.Rows.Count; i++)
            {
                hiddescribe = hiddescribe + ((i + 1) + ":" + dtstr.Rows[i]["hiddescribe"]);
                changemeasure = changemeasure + ((i + 1) + ":" + dtstr.Rows[i]["changemeasure"]);

                count++;
            }
            for (int k = 0; k < dtbreak.Rows.Count; k++)
            {
                lllegaldescribe = lllegaldescribe + ((count + k + 1) + ":" + dtbreak.Rows[k]["lllegaldescribe"]);
                reformmeasure = reformmeasure + ((count + k + 1) + ":" + dtbreak.Rows[k]["reformmeasure"]);
            }
            DataRow row = dt.NewRow();
            row["hiddescribe"] = hiddescribe;
            row["changemeasure"] = changemeasure;
            row["lllegaldescribe"] = lllegaldescribe;
            row["reformmeasure"] = reformmeasure;
            dt.Rows.Add(row);
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
        public void SaveForm(string keyValue, TaskRelevanceProjectEntity entity)
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
