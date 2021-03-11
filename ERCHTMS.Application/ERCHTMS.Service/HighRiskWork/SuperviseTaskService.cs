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
using ERCHTMS.Entity.HighRiskWork.ViewModel;

namespace ERCHTMS.Service.HighRiskWork
{
    /// <summary>
    /// 描 述：旁站监督任务
    /// </summary>
    public class SuperviseTaskService : RepositoryFactory<SuperviseTaskEntity>, SuperviseTaskIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<SuperviseTaskEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return this.BaseRepository().FindList(pagination);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SuperviseTaskEntity> GetList(string queryJson)
        {
            return this.BaseRepository().FindList(string.Format("select * from bis_supervisetask where 1=1 " + queryJson)).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SuperviseTaskEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }


        /// <summary>
        /// 获取监督任务列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageDataTable(Pagination pagination, string queryJson)
        {
            var user = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["status"].IsEmpty())//监督状态
            {
                pagination.conditionJson += string.Format(" and supervisestate='{0}'", queryParam["status"].ToString());
            }
            if (!queryParam["worktype"].IsEmpty())//作业类型
            {
                pagination.conditionJson += string.Format(" and  ','||taskworktypeid ||',' like '%,{0},%'", queryParam["worktype"].ToString());
            }
            //时间选择
            if (!queryParam["st"].IsEmpty())//作业开始时间
            {
                string from = queryParam["st"].ToString().Trim();
                pagination.conditionJson += string.Format(" and taskWorkStartTime>=to_date('{0}','yyyy-mm-dd')", from);
            }
            if (!queryParam["et"].IsEmpty())//作业结束时间
            {
                string to = Convert.ToDateTime(queryParam["et"].ToString().Trim()).AddDays(1).ToString("yyyy-MM-dd");
                pagination.conditionJson += string.Format(" and taskWorkEndTime<=to_date('{0}','yyyy-mm-dd')", to);
            }
            if (!queryParam["workdept"].IsEmpty() && !queryParam["workdeptid"].IsEmpty())//作业单位
            {
                pagination.conditionJson += string.Format(" and id in(select t.superviseid  from bis_superviseworkinfo t where  workdeptcode in(select encode from base_department  where encode like '{0}%' or senddeptid='{1}') group by superviseid)", queryParam["workdept"].ToString(), queryParam["workdeptid"].ToString());
            }
            if (!queryParam["sideuser"].IsEmpty())//监督员
            {
                pagination.conditionJson += string.Format(" and TaskUserId  like '%{0}%'", queryParam["sideuser"].ToString());
            }
            if (!queryParam["teams"].IsEmpty())//班组
            {
                pagination.conditionJson += string.Format(" and steamid='{0}'", queryParam["teams"].ToString());
            }
            if (!queryParam["parentid"].IsEmpty())//二级任务
            {
                pagination.conditionJson += string.Format(" and superparentid='{0}'", queryParam["parentid"].ToString());
            }
            else//默认只显示一级任务
            {
                if (!queryParam["mytask"].IsEmpty())//我的监督任务
                {
                    pagination.conditionJson += string.Format(" and supervisestate!=1 and TaskUserId  like '%{0}%' and  IsSubmit='0' and tasklevel='1'",user.UserId);
                }
                else
                {
                    pagination.conditionJson += " and tasklevel='0'";
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
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
        /// <param name="model">实体对象</param>
        /// <returns></returns>
        public string SaveForm(string keyValue, SuperviseTaskModel model)
        {
            var res = DbFactory.Base().BeginTrans();
            try
            {

                Repository<SuperviseTaskEntity> repScaffold = new Repository<SuperviseTaskEntity>(DbFactory.Base());
                SuperviseTaskEntity entity = repScaffold.FindEntity(keyValue);
                model.Id = keyValue;
                //新增
                if (entity == null)
                {
                    entity = new SuperviseTaskEntity();
                    entity.Id = Guid.NewGuid().ToString();
                    keyValue = entity.Id;
                    //实体赋值
                    this.copyProperties(entity, model);
                    entity.Create();
                    //添加操作
                    res.Insert(entity);
                }
                else
                {
                    //编辑 
                    entity.Modify(keyValue);
                    //实体赋值
                    this.copyProperties(entity, model);
                    //更新操作
                    res.Update(entity);
                }
                //添加或更新作业信息 先删除再添加
                res.Delete<SuperviseWorkInfoEntity>(t => t.SuperviseId == entity.Id);
                foreach (var spec in model.WorkSpecs)
                {
                    spec.SuperviseId = entity.Id;
                    spec.Create();
                    res.Insert(spec);
                }
                res.Commit();
                return keyValue;
            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }
        }


        /// <summary>
        /// 从源实体给目标实体属性赋值
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="source">源</param>
        private void copyProperties(SuperviseTaskEntity target, SuperviseTaskModel source)
        {
            target.TaskLevel = source.TaskLevel;
            target.IsSubmit = "0";//一开始未提交
            target.TimeLong = source.TimeLong;
            target.OrganizeManager = source.OrganizeManager;
            target.SuperviseCode = source.SuperviseCode;
            target.SuperviseState = source.SuperviseState;
            target.TaskWorkEndTime = source.TaskWorkEndTime;
            target.TimeLongStr = source.TimeLongStr;
            target.TaskWorkTypeId = source.TaskWorkTypeId;
            target.TaskUserName = source.TaskUserName;
            target.TaskWorkType = source.TaskWorkType;
            target.TaskWorkStartTime = source.TaskWorkStartTime;
            target.TaskUserId = source.TaskUserId;
            target.RiskAnalyse = source.RiskAnalyse;
            target.SafetyMeasure = source.SafetyMeasure;
            target.TaskBill = source.TaskBill;
            target.RiskAnalyse = source.RiskAnalyse;
            target.SuperParentId = source.SuperParentId;
            target.ConstructLayout = source.ConstructLayout;
            target.STeamId = source.STeamId;
            target.STeamCode = source.STeamCode;
            target.STeamName = source.STeamName;
            target.HandType = source.HandType;
            target.CreateUserId = source.CreateUserId;
            target.CreateUserName = source.CreateUserName;
            target.CreateUserDeptCode = source.CreateUserDeptCode;
            target.CreateUserOrgCode = source.CreateUserOrgCode;
        }


        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveOnlyTask(string keyValue, SuperviseTaskEntity entity)
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
