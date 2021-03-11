using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data;
using ERCHTMS.Code;

namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// 描 述：开收工会
    /// </summary>
    public class WorkMeetingService : RepositoryFactory<WorkMeetingEntity>, WorkMeetingIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<WorkMeetingEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public WorkMeetingEntity GetEntity(string keyValue)
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
            DatabaseType dataType = DbHelper.DbType;
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        public DataTable GetTable(string sql) {
            return this.BaseRepository().FindTable(sql);
        }
        /// <summary>
        /// 根据当前登录人获取未提交的数据
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetNotCommitData(string userid)
        {
            string sql = string.Format(@"select * from BIS_WORKMEETING t where t.createuserid='{0}' and t.iscommit='0'", userid);
            return this.BaseRepository().FindTable(sql);
        }
        /// <summary>
        /// 获取今日开工会的临时工程数
        /// </summary>
        /// <returns></returns>
        public int GetTodayTempProject(Operator curUser)
        {
            string sql = string.Format(@"select distinct b.engineerid
                                          from bis_workmeeting b
                                         where b.iscommit=1 and b.meetingtype = '开工会'
                                           and to_char(b.meetingdate, 'yyyy-MM-dd') =
                                               to_char(sysdate, 'yyyy-MM-dd')
                                           and b.engineerid in (select e.id
                                                                  from epg_outsouringengineer e
                                                                 where e.engineertype = '002')");
            string role = curUser.RoleName;
            if (role.Contains("公司级用户") || role.Contains("厂级部门用户"))
            {
                sql += string.Format(" and b.createuserorgcode  = '{0}'", curUser.OrganizeCode);
            }
            else if (role.Contains("承包商级用户"))
            {
                sql += string.Format(" and b.engineerid in (select e.id from epg_outsouringengineer e where e.engineertype = '002' and e.outprojectid='{0}')", curUser.DeptId);
            }
            else
            {
                sql += string.Format(" and  b.engineerid in (select e.id from epg_outsouringengineer e where e.engineertype = '002' and e.engineerletdeptid='{0}')", curUser.DeptId);
            }
            DataTable dt = this.BaseRepository().FindTable(sql);
            return dt.Rows.Count;
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
        public void SaveForm(string keyValue, WorkMeetingEntity entity)
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

        public void SaveWorkMeetingForm(string keyValue, WorkMeetingEntity entity, List<WorkmeetingmeasuresEntity> list, string ids)
        {
            entity.ID = keyValue;
            if (entity.MEETINGTYPE == "收工会")
            {
                var e = this.BaseRepository().FindEntity(entity.StartMeetingid);
                e.IsOver = 1;//开收工会闭环
                this.BaseRepository().Update(e);
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Workmeetingid = keyValue;
                }
            }
            if (!string.IsNullOrEmpty(keyValue))
            {
                var res = new Repository<WorkmeetingmeasuresEntity>(DbFactory.Base());
                WorkMeetingEntity re = this.BaseRepository().FindEntity(keyValue);
                if (re == null)
                {
                    entity.Create();
                    if (this.BaseRepository().Insert(entity) > 0)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            list[i].Create();
                            list[i].CreateDate = list[i].CreateDate.Value.AddSeconds(i);
                        }
                        res.Insert(list);
                    }
                }
                else
                {

                    entity.Modify(keyValue);
                    entity.ids = null ;
                    if (this.BaseRepository().Update(entity) > 0)
                    {
                        int count = res.ExecuteBySql(string.Format("delete from epg_workmeetingmeasures where workmeetingid='{0}'", entity.ID));
                        for (int i = 0; i < list.Count; i++)
                        {
                            list[i].Create();
                            list[i].CreateDate = list[i].CreateDate.Value.AddSeconds(i);
                        }
                        res.Insert(list);
                    }
                }

            }
            else
            {
                //手机端传空Id,需要处理关联Id
                entity.Create();
                var res = new Repository<WorkmeetingmeasuresEntity>(DbFactory.Base());
                if (this.BaseRepository().Insert(entity) > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(list[i].ID))
                        {
                            list[i].ID = "";
                        }
                        list[i].Workmeetingid = entity.ID;
                        list[i].Create();
                        list[i].CreateDate = list[i].CreateDate.Value.AddSeconds(i);
                    }
                    res.Insert(list);
                }
            }
            if (!string.IsNullOrWhiteSpace(ids))
            {
                var listids = ids.Split(',');
                for (int i = 0; i < listids.Length; i++)
                {
                    var e = new DangerdataService().GetEntity(listids[i]);
                    if (e != null)
                    {
                        e.UserNum += 1;
                        new DangerdataService().SaveForm(e.ID, e);
                    }
                }
            }

        }

        #endregion
    }
}
