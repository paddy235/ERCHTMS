using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Entity.DangerousJob;
using ERCHTMS.IService.DangerousJob;
using ERCHTMS.Service.HighRiskWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Service.DangerousJob
{
    /// <summary>
    /// 描 述：危险作业流程流转表
    /// </summary>
    public class DangerousJobFlowDetailService : RepositoryFactory<DangerousJobFlowDetailEntity>, DangerousJobFlowDetailIService
    {
        DangerousJobFlowService jsService = new DangerousJobFlowService();
        TransferrecordService tfService = new TransferrecordService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<DangerousJobFlowDetailEntity> GetList()
        {
            return this.BaseRepository().IQueryable().ToList();
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
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string role = user.RoleName;
            var queryParam = queryJson.ToJObject();
            #region 查表
            pagination.p_kid = "t.id";
            pagination.p_fields = @"t.CreateUserId,t.jobstate,t.applyno,t.jobtype,t.jobdeptname,
                        t.jobplace,t.jobstarttime,t.realityjobstarttime,t.applyusername,t.applytime";
            pagination.p_tablename = @"BIS_JobSafetyCardApply t";
            if (pagination.sidx == null)
            {
                pagination.sidx = "t.createdate";
            }
            if (pagination.sord == null)
            {
                pagination.sord = "desc";
            }
            #endregion
            DataTable data = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return data;
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public DangerousJobFlowDetailEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 获取实体/链表查
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="fid">反馈历史数据id</param>
        /// <returns></returns>
        public DataTable GetEntityByT(string keyValue, string fid)
        {
            string sql = string.Format(@"select t.*,t1.id as fid,t1.FinishInfo,t1.FeedbackDate,t1.SignUrl,t1.CreateDate as FCreateDate,t2.id as cid,t2.SuperviseResult,t2.SuperviseOpinion,t2.ConfirmationDate,t2.SignUrl as SignUrlT,t2.CreateDate as CCreateDate
 from BIS_SafetyWorkSupervise t left join (select * from  BIS_SafetyWorkFeedback where flag='0') t1 
on t.id=t1.superviseid left join (select * from BIS_SuperviseConfirmation where flag='0') t2 on t1.id=t2.feedbackid
where t.id='{0}'", keyValue);
            if (!string.IsNullOrEmpty(fid))
            {
                sql = string.Format(@"select t.*,t1.id as fid,t1.FinishInfo,t1.FeedbackDate,t1.SignUrl,t1.CreateDate as FCreateDate,t2.id as cid,t2.SuperviseResult,t2.SuperviseOpinion,t2.ConfirmationDate,t2.SignUrl as SignUrlT,t2.CreateDate as CCreateDate
 from BIS_SafetyWorkSupervise t left join (select * from BIS_SafetyWorkFeedback where id='{1}') t1 
on t.id=t1.superviseid left join (select * from BIS_SuperviseConfirmation  where feedbackid='{1}') t2 on t1.id=t2.feedbackid
where t.id='{0}'", keyValue, fid);
            }
            return this.BaseRepository().FindTable(sql);
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
        public void SaveForm(string keyValue, DangerousJobFlowDetailEntity entity)
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
        /// 审核保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主表主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void CheckSaveForm(string keyValue, DangerousJobFlowDetailEntity entity)
        {
            JobSafetyCardApplyService jsService = new JobSafetyCardApplyService();
            var jsEntity = jsService.GetEntity(keyValue);//获取工作证申请主表
            //先处理当前步骤，获取流转表业务数据当前步骤
            var entity1 = this.GetList().Where(x => x.BusinessId == keyValue && x.Status == 0 && x.ApplyNumber == jsEntity.ApplyNumber).ToList().FirstOrDefault();
            if (entity1 != null)
            {
                entity1.Status = 1;//标记已处理
                entity1.CheckResult = entity.CheckResult;
                entity1.ApproveTime = entity.ApproveTime;
                entity1.ApproveDeptName = entity.ApproveDeptName;
                entity1.ApproveDeptId = entity.ApproveDeptId;
                entity1.ApprovePerson = entity.ApprovePerson;
                entity1.ApprovePersonId = entity.ApprovePersonId;
                entity1.ApproveOpinion = entity.ApproveOpinion;
                entity1.SignUrl = string.IsNullOrWhiteSpace(entity.SignUrl) ? "" : entity.SignUrl.Replace("../..", "");
                entity.Modify(entity1.Id);
                this.BaseRepository().Update(entity1);//更新当前步骤审核信息
                if (entity1.CheckResult == "1")
                {
                    int ApplyNumber = jsEntity.ApplyNumber + 1;
                    ////审批不同意流程重新发起
                    //NextStep(keyValue, 1, ApplyNumber);
                    //不同意更新主表状态
                    jsEntity.JobState = 2;
                    jsEntity.ApplyNumber = ApplyNumber;//更新申请次数
                    jsService.SaveForm(keyValue, jsEntity);
                }
                else
                {
                    if (entity1.NextStep != 0)
                    {
                        //不等于0标示流程未结束，继续执行下一步
                        NextStep(keyValue, (int)entity1.NextStep, jsEntity.ApplyNumber);
                    }
                    else
                    {
                        //标示流程已走完，更新主表状态
                        if (jsEntity.JobType == "Digging")
                        {
                            jsEntity.JobState = 5; //动土作业 审核通过进入备案中状态
                        }
                        else
                        {
                            jsEntity.JobState = 8; //其他作业  审核通过进入开始作业状态
                        }
                        jsService.SaveForm(keyValue, jsEntity);
                    }
                }

            }

        }
        #endregion

        #region 流程数据
        /// <summary>
        /// 下一步流程
        /// </summary>
        /// <param name="BusinessId">业务数据id</param>
        /// <param name="FlowStep">流程步骤</param>
        /// <param name="ApplyNumber">第几次申请</param>
        public void NextStep(string BusinessId,int FlowStep,int ApplyNumber) {
            //获取业务数据对应的下一步流程步骤
            var djf = jsService.GetList().Where(x => x.BusinessId == BusinessId && x.FlowStep == FlowStep).ToList().FirstOrDefault();
            if (djf != null)
            {
                int NextStep = FlowStep + 1;
                //查询是否存在下一步流程,不存在下一步流程标示0
                var djf1 = jsService.GetList().Where(x => x.BusinessId == BusinessId && x.FlowStep == NextStep).ToList().FirstOrDefault();
                if (djf1 == null)
                {
                    NextStep = 0;
                }
                DangerousJobFlowDetailEntity entity = new DangerousJobFlowDetailEntity()
                {
                    ModuleNo = djf.ModuleNo,//逐级审核模块编号
                    FlowName = djf.FlowName,//节点名称
                    CurrentStep = FlowStep,//当前流程步骤
                    NextStep = NextStep,//下一步流程步骤,0标识流程已结束
                    BusinessId = djf.BusinessId,//关联业务数据id
                    ProcessorFlag = djf.ProcessorFlag,//当前步骤处理标示(0部门加角色，1执行脚本获取业务某个字段，2指定审核人，3业务选择审核人)
                    DeptId = djf.DeptId,
                    DeptCode = djf.DeptCode,
                    DeptName = djf.DeptName,
                    RoleName = djf.RoleName,
                    RoleId = djf.RoleId,
                    UserId = djf.UserId,//这里取危险作业流程表里的值(有页面选择审批人的情况)
                    UserAccount = djf.UserAccount,
                    UserName = djf.UserName,
                    Status = 0,
                    ApplyNumber= ApplyNumber
                };
                this.SaveForm("", entity);
            }
        }

        /// <summary>
        /// 获取当前用户是否有权限
        /// </summary>
        /// <param name="BusinessId"></param>
        /// <param name="userAccount"></param>
        /// <param name="flowdetailid">流转表id</param>
        /// <param name="approveName">审核人姓名</param>
        /// <param name="approveId">审核人id</param>
        /// <param name="approveAccount">审核人账号</param>
        /// <returns></returns>
        public int GetCurrentStepRole(string BusinessId,string userAccount,string flowdetailid,out string approveName,out string approveId,out string approveAccount) {
            int b = 1;//1表示无权限
            approveName = "";
            approveId = "";
            approveAccount = "";
            try
            {
                var entity = this.GetList().Where(x => x.BusinessId == BusinessId && x.Status == 0).ToList().FirstOrDefault();
                if (entity != null)
                {
                    //string OutTransferUserAccount = "";//移除的审批人
                    //string InTransferUserAccount = "";//新增的审批人
                    ////取转交表最新转交人信息
                    //var entityTF = tfService.GetList(t => t.FlowId == flowdetailid && t.Disable == 0).FirstOrDefault();
                    //if (entityTF != null)
                    //{
                    //    OutTransferUserAccount = entityTF.OutTransferUserAccount;//移除的审批人
                    //    InTransferUserAccount = entityTF.InTransferUserAccount;//新增的审批人
                    //}
                    //string[] OutUserList = OutTransferUserAccount.Split(',');
                    //string[] InUserList = InTransferUserAccount.Split(',');
                    //当前步骤处理标示(0部门加角色，1执行脚本获取业务某个字段，2指定审核人，3业务选择审核人)
                    string ProcessorFlag = entity.ProcessorFlag;
                    if (ProcessorFlag == "3")
                    {
                        string[] userList = entity.UserAccount.Split(',');
                        approveName = entity.UserName;
                        approveId = entity.UserId;
                        approveAccount = entity.UserAccount;
                        if (userList.Contains(userAccount))
                        {
                            b = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            
            return b;
        }

        /// <summary>
        /// 获取当前审核人id
        /// </summary>
        /// <param name="BusinessId"></param>
        /// <param name="flowdetailid">流转表id</param>
        /// <returns></returns>
        public string GetCurrentStepUser(string BusinessId, string flowdetailid)
        {
            try
            {
                var entity = this.GetList().Where(x => x.BusinessId == BusinessId && x.Status == 0).ToList().FirstOrDefault();
                if (entity != null)
                {
                    string ProcessorFlag = entity.ProcessorFlag;
                    if (ProcessorFlag == "3")
                    {
                        string[] userList = entity.UserId.Split(',');
                        return string.Join(",", userList);
                    }
                }
            }
            catch (Exception ex)
            {
                return "";
            }
            return "";
        }
        #endregion

        /// <summary>
        /// 审核保存表单（新增、修改）(危险作业审批单)
        /// </summary>
        /// <param name="keyValue">主表主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void ApprovalFormCheckSaveForm(string keyValue, DangerousJobFlowDetailEntity entity)
        {
            JobApprovalFormService jsService = new JobApprovalFormService();
            var jsEntity = jsService.GetEntity(keyValue);//获取工作证申请主表
            //先处理当前步骤，获取流转表业务数据当前步骤
            var entity1 = this.GetList().Where(x => x.BusinessId == keyValue && x.Status == 0).ToList().FirstOrDefault();
            if (entity1 != null)
            {
                entity1.Status = 1;//标记已处理
                entity1.CheckResult = entity.CheckResult;
                entity1.ApproveTime = entity.ApproveTime;
                entity1.ApproveDeptName = entity.ApproveDeptName;
                entity1.ApproveDeptId = entity.ApproveDeptId;
                entity1.ApprovePerson = entity.ApprovePerson;
                entity1.ApprovePersonId = entity.ApprovePersonId;
                entity1.ApproveOpinion = entity.ApproveOpinion;
                entity1.SignUrl = string.IsNullOrWhiteSpace(entity.SignUrl) ? "" : entity.SignUrl.Replace("../..", "");
                entity.Modify(entity1.Id);
                this.BaseRepository().Update(entity1);//更新当前步骤审核信息
                if (entity1.CheckResult == "1")
                {
                    int ApplyNumber = jsEntity.ApplyNumber + 1;
                    //审批不同意流程重新发起
                    //NextStep(keyValue, 1, ApplyNumber);
                    //不同意更新主表状态
                    jsEntity.JobState = 4;
                    jsEntity.ApplyNumber = ApplyNumber;//更新申请次数
                    jsService.SaveForm(keyValue, jsEntity);
                }
                else
                {
                    if (entity1.NextStep != 0)
                    {
                        //不等于0标示流程未结束，继续执行下一步
                        NextStep(keyValue, (int)entity1.NextStep, jsEntity.ApplyNumber);
                    }
                    else
                    {
                        //标示流程已走完，更新主表状态
                        jsEntity.JobState = 2;
                        jsService.SaveForm(keyValue, jsEntity);
                    }
                }

            }

        }
    }
}
