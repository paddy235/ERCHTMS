using ERCHTMS.Entity.PersonManage;
using ERCHTMS.IService.PersonManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Data;
using System.Web.UI.WebControls;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Service.PersonManage
{
    /// <summary>
    /// 描 述：人员工作经历
    /// </summary>
    public class WorkRecordService : RepositoryFactory<WorkRecordEntity>, WorkRecordIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>返回列表</returns>
        public IEnumerable<WorkRecordEntity> GetList(string userId)
        {
            return this.BaseRepository().IQueryable(t => t.UserId == userId).OrderByDescending(t => t.CreateDate);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public WorkRecordEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, WorkRecordEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.WorkType = 1;
                entity.Create();
                if (this.BaseRepository().Insert(entity) > 0)
                {
                    string sql = string.Format("update base_user set IsPresence='{0}',entertime=to_date('{1}','yyyy-mm-dd'),departmentid='{2}',departmentcode='{3}',DepartureTime='' where userid='{4}'", "1", DateTime.Now.ToString("yyyy-MM-dd"), entity.DeptId, entity.DeptCode, entity.UserId);
                    this.BaseRepository().ExecuteBySql(sql);
                }
            }
        }


        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void NewSaveForm(string keyValue, WorkRecordEntity entity)
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
        /// 人员离场时写工作记录
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="deptId">部门Id</param>
        /// <returns></returns>
        public int WriteWorkRecord(UserInfoEntity user, ERCHTMS.Code.Operator currUser)
        {
            //找到之前没有结尾的工作记录填写结尾
            string sql = string.Format("select count(1) from BIS_WORKRECORD where userid='{0}' and deptid='{1}' and WorkType=1 and LeaveTime  =to_date('0001-01-01 00:00:00','yyyy-mm-dd hh24:mi:ss') ", user.UserId, user.DepartmentId);
            string count = this.BaseRepository().FindObject(sql).ToString();
            if (count == "0")
            {
                WorkRecordEntity workEntity = new WorkRecordEntity
                 {
                     Id = Guid.NewGuid().ToString(),
                     DeptCode = user.DepartmentCode,
                     DeptId = user.DepartmentId,
                     //EnterDate = user.EnterTime.Value,
                     UserId = user.UserId,
                     UserName = user.RealName,
                     DeptName = user.DeptName,
                     PostName = user.DutyName,
                     CreateDate = DateTime.Now,
                     CreateUserId = currUser.UserId,
                     LeaveTime = user.DepartureTime.Value,
                     OrganizeName = user.OrganizeName,
                     JobName = user.PostName,
                     WorkType = 1
                 };
                //if (user.EnterTime!=null)
                // {
                //     workEntity.EnterDate = user.EnterTime.Value;
                // }
                return new Repository<WorkRecordEntity>(DbFactory.Base()).Insert(workEntity);
            }
            else
            {
                sql = string.Format("update BIS_WORKRECORD set leavetime=to_date('{2}','yyyy-mm-dd hh24:mi:ss') where id=(select id  from (select id from BIS_WORKRECORD t where userid='{0}' and deptid='{1}'  and WorkType=1 and LeaveTime =to_date('0001-01-01 00:00:00','yyyy-mm-dd hh24:mi:ss')  order by createdate desc) a where rownum=1) ", user.UserId, user.DepartmentId, user.DepartureTime.Value);
                return this.BaseRepository().ExecuteBySql(sql);
            }


        }


        /// <summary>
        /// 人员操作换部门时写记录
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="deptId">部门Id</param>
        /// <returns></returns>
        public int WriteChangeRecord(UserInfoEntity user, ERCHTMS.Code.Operator currUser)
        {
            //找到之前没有结尾的工作记录填写结尾
            string sql = string.Format("update BIS_WORKRECORD set leavetime=to_date('{1}','yyyy-mm-dd hh24:mi:ss') where id=(select id  from (select id from BIS_WORKRECORD t where userid='{0}' and WorkType=1 and LeaveTime =to_date('0001-01-01 00:00:00','yyyy-mm-dd hh24:mi:ss')  order by createdate desc) a where rownum=1) ", user.UserId, DateTime.Now);
            this.BaseRepository().ExecuteBySql(sql);

            //在新增一条新的工作记录
            WorkRecordEntity workEntity = new WorkRecordEntity
            {
                Id = Guid.NewGuid().ToString(),
                DeptCode = user.DepartmentCode,
                DeptId = user.DepartmentId,
                EnterDate = DateTime.Now,
                UserId = user.UserId,
                UserName = user.RealName,
                DeptName = user.DeptName,
                PostName = user.DutyName,
                CreateDate = DateTime.Now,
                CreateUserId = currUser.UserId,
                //LeaveTime = user.DepartureTime.Value,
                OrganizeName = user.OrganizeName,
                JobName = user.PostName,
                WorkType = 1
            };
            return new Repository<WorkRecordEntity>(DbFactory.Base()).Insert(workEntity);



        }

        /// <summary>
        /// 修改离场原因则修改最近的工作记录结束时间
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public int EidtRecord(string userid, string time)
        {

            string sql = string.Format("update BIS_WORKRECORD set leavetime=to_date('{1}','yyyy-mm-dd hh24:mi:ss') where id=(select id  from (select id from BIS_WORKRECORD t where userid='{0}' and WorkType=1   order by createdate desc) a where rownum=1) ", userid, time.ToString());
            return this.BaseRepository().ExecuteBySql(sql);
        }

        #endregion
    }
}
