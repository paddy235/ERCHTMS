using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;
using BSFramework.Data;
using System.Text;
using System;

namespace ERCHTMS.Service.HighRiskWork
{
    /// <summary>
    /// 描 述：旁站监督作业信息
    /// </summary>
    public class SuperviseWorkInfoService : RepositoryFactory<SuperviseWorkInfoEntity>, SuperviseWorkInfoIService
    {
        #region 获取数据
        /// <summary>
        /// 根据分配任务id获取作业信息
        /// </summary>
        /// <param name="taskshareid"></param>
        /// <returns></returns>
        public IEnumerable<SuperviseWorkInfoEntity> GetList(string strwhere)
        {
            return this.BaseRepository().FindList(string.Format("select * from bis_superviseworkinfo where 1=1 " + strwhere)).ToList();
        }


        /// <summary>
        /// 根据分配任务id和班组id获取作业信息
        /// </summary>
        /// <param name="taskshareid"></param>
        /// <returns></returns>
        public IEnumerable<SuperviseWorkInfoEntity> GetList(string taskshareid,string teamid)
        {
            string sql = @" select * from bis_superviseworkinfo where id in(select distinct(a.wrokid) from  bis_teamswork a where a.teamtaskid in(select id from bis_teamsinfo t where t.taskshareid=@taskshareid  and t.teamid=@teamid))";
            var parameter = new List<DbParameter>();
            parameter.Add(DbParameters.CreateDbParameter("@taskshareid", taskshareid));
            parameter.Add(DbParameters.CreateDbParameter("@teamid", teamid));
            return this.BaseRepository().FindList(sql, parameter.ToArray()).ToList();
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SuperviseWorkInfoEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, SuperviseWorkInfoEntity entity)
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
        ///根据分配id删除作业信息
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveWorkByTaskShareId(string keyValue)
        {
            IRepository db = new RepositoryFactory().BaseRepository().BeginTrans();
            try
            {
                db.Delete<SuperviseWorkInfoEntity>(t => t.TaskShareId.Equals(keyValue));
                db.Commit();
            }
            catch (Exception)
            {
                db.Rollback();
                throw;
            }
        }
        #endregion
    }
}
