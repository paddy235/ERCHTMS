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
using ERCHTMS.Service.SystemManage;
using ERCHTMS.Service.BaseManage;

namespace ERCHTMS.Service.HighRiskWork
{
    /// <summary>
    /// 描 述：任务分配班组
    /// </summary>
    public class TeamsInfoService : RepositoryFactory<TeamsInfoEntity>, TeamsInfoIService
    {
        #region 获取数据
        /// <summary>
        /// 根据条件获取班组任务分配信息
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<TeamsInfoEntity> GetList(string queryJson)
        {
            Operator curUser = OperatorProvider.Provider.Current();
            var queryParam = JObject.Parse(queryJson);
            var parameter = new List<DbParameter>();
            string sql = "select * from bis_teamsinfo  where  taskshareid= @taskshareid";
            string taskshareid = queryParam["taskshareid"].ToString();
            parameter.Add(DbParameters.CreateDbParameter("@taskshareid", taskshareid));
            if (!queryParam["teamid"].IsEmpty())//班组id
            {
                sql += " and teamid= @teamid";
                parameter.Add(DbParameters.CreateDbParameter("@teamid", queryParam["teamid"].ToString()));
            }
            var data = this.BaseRepository().FindList(sql, parameter.ToArray()).ToList();
            var taskshare = new TaskShareService().GetEntity(taskshareid);
            string userids = "";
            List<UserEntity> users = new List<UserEntity>();
            if (taskshare != null)
            {
                if (!string.IsNullOrEmpty(taskshare.TaskType) && !string.IsNullOrEmpty(taskshare.FlowStep))
                {
                    if (taskshare.TaskType != "2" && (taskshare.FlowStep == "2" || taskshare.FlowStep == "3"))
                    {
                        if (taskshare.TaskType == "0")
                        {
                            string rolenames = new DataItemDetailService().GetItemValue(curUser.OrganizeId, "deptsuperviserole");
                            if (!string.IsNullOrEmpty(rolenames))
                            {
                                rolenames = "'" + rolenames.Replace(",", "','") + "'";
                                users = new UserService().GetUserListByRoleName("'" + taskshare.SuperviseDeptId + "'", rolenames, true, string.Empty).ToList();
                            }
                        }
                        var userEntity = new UserService().GetEntity(taskshare.CreateUserId);
                        if (userEntity != null)
                            users.Add(userEntity);
                    }
                }
            }
            if (users != null && users.Count > 0)
            {
                userids = string.Join(",", users.Select(x => x.UserId).ToArray());
            }
            data.ForEach(t =>
            {
                t.ModifyUserId = userids;//modifyuserid借用该字段
            });
            return data;
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<TeamsInfoEntity> GetAllList(string queryJson)
        {
            return this.BaseRepository().FindList(string.Format("select * from bis_teamsinfo where 1=1 " + queryJson)).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TeamsInfoEntity GetEntity(string keyValue)
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
        public string SaveForm(string keyValue, TeamsInfoEntity entity)
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
            return entity.Id;
        }
        #endregion
    }
}
