using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.DangerousJob;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.IService.DangerousJob;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Service.SystemManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ERCHTMS.Service.DangerousJob
{
    /// <summary>
    /// 描 述：作业安全证申请
    /// </summary>
    public class JobApprovalFormService : RepositoryFactory<JobApprovalFormEntity>, JobApprovalFormIService
    {
        DangerousJobFlowDetailService DetailService = new DangerousJobFlowDetailService();
        JobSafetyCardApplyService JobSafetyCardApplyService = new JobSafetyCardApplyService();
        DangerousJobFlowService flowservice = new DangerousJobFlowService();
        private DataItemDetailService dataitemdetailservice = new DataItemDetailService();
        UserService userservice = new UserService();
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<JobApprovalFormEntity> GetList(string queryJson)
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
            DatabaseType dataTye = DatabaseType.Oracle;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string role = "";
            if (user != null) role = user.RoleName;
            var queryParam = queryJson.ToJObject();

            #region 查表
            pagination.p_kid = "t.id";
            pagination.p_fields = @"t.CreateUserId,t.jobdeptid,t.ApplyNo,t.JobType,t.JobDeptName,t.JobLevel,t.JobSafetyCardId,t.JobTypeName,''cardstate,
                        t.JobPlace,t.JobStartTime,t.JobEndTime,t.RealityJobStartTime,t.RealityJobendTime,t.ApplyUserName,t.ApplyTime,t.IsSubmit,t1.id as flowdetailid,'' as isrole,'' as OperatorName,'' OperatorId,
                        case when t.workoperate='1' then '作业暂停' when realityjobstarttime is not null and realityjobendtime is null then '作业中' when realityjobendtime is not null then '已结束' when jobstate =2 then '即将作业'  else '' end ledgertype,
                        case when t.workoperate='1' then 5 when realityjobstarttime is not null and realityjobendtime is null then 6 when realityjobendtime is not null then 7 else jobstate end jobstate,
                        '' as isoperate, '' as isrolestate";
            pagination.p_tablename = @"BIS_JobApprovalForm t left join BIS_DangerousJobFlowDetail t1
                                        on t.id=t1.businessid and t1.status=0";
            if (pagination.sidx == null)
            {
                pagination.sidx = "t.createdate";
            }
            if (pagination.sord == null)
            {
                pagination.sord = "desc";
            }
            #endregion

            if (!queryParam["code"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.jobdeptcode like '%{0}%' ", queryParam["code"].ToString());
            }
            //关键字
            if (!queryParam["keyword"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and (t.applyno like '%{0}%' or t.jobcontent like '%{0}%' or t.jobplace like '%{0}%') ", queryParam["keyword"].ToString());
            }
            //作业许可状态
            if (!queryParam["jobstate"].IsEmpty())
            {
                var status = queryParam["jobstate"].ToString();

                if (status == "5")
                {
                    pagination.conditionJson += string.Format(" and t.workoperate='1'");
                }
                else if (status == "6")
                {
                    pagination.conditionJson += string.Format(" and t.realityjobstarttime is not null and t.realityjobendtime is null ");
                }
                else if (status == "7")
                {
                    pagination.conditionJson += string.Format(" and t.realityjobendtime is not null and WorkOperate=0 ");
                }
                else
                {
                    pagination.conditionJson += string.Format(" and t.jobstate='{0}'", status);
                }
            }
            if (!queryParam["joblevel"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.joblevel='{0}' ", queryParam["joblevel"]);
            }
            //作业类型
            if (!queryParam["jobtype"].IsEmpty())
            {
                var jobtype = queryParam["jobtype"].ToString();
                pagination.conditionJson += string.Format(" and t.jobtype like '%{0}%' ", jobtype);
            }
            //开始时间
            if (!queryParam["jobstarttime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and  (t.jobstarttime>=to_date('{0}','yyyy-mm-dd')  or t.realityjobstarttime>=to_date('{0}','yyyy-mm-dd'))", queryParam["jobstarttime"]);
            }
            //结束时间
            if (!queryParam["jobendtime"].IsEmpty())
            {
                var eTime = (Convert.ToDateTime(queryParam["jobendtime"].ToString()).AddDays(1)).ToString("yyyy-MM-dd");
                pagination.conditionJson += string.Format(" and (t.jobendtime<=to_date('{0}','yyyy-mm-dd') or t.realityjobendtime>=to_date('{0}','yyyy-mm-dd'))", eTime);
            }
            //查看范围
            if (!queryParam["showrange"].IsEmpty() && user != null)
            {
                var showRange = queryParam["showrange"].ToString();
                if (showRange == "0")//本人申请
                {
                    pagination.conditionJson += string.Format(" and t.createuserid='{0}'", user.UserId);
                }
                else if (showRange == "1")//本人审核
                {
                    DataTable dt = BaseRepository().FindTable("select " + pagination.p_kid + "," + pagination.p_fields + " from " + pagination.p_tablename + " where " + pagination.conditionJson);

                    foreach (DataRow dr in dt.Rows)
                    {
                        string BusinessId = dr["id"].ToString();
                        //获取当前用户是否有权限操作该条数据
                        string approveName = "";
                        string approveId = "";
                        string approveAccount = "";
                        dr["isrole"] = DetailService.GetCurrentStepRole(BusinessId, user.Account, dr["flowdetailid"] == null ? "" : dr["flowdetailid"].ToString(), out approveName, out approveId, out approveAccount);
                    }
                    pagination.conditionJson += string.Format(" and t.id in ('{0}')", string.Join("','", dt.Select("isrole ='0'").AsEnumerable().Select(d => d.Field<string>("id")).ToArray()));
                }
            }

            DataTable data = this.BaseRepository().FindTableByProcPager(pagination, dataTye);

            if (data != null && data.Rows.Count > 0)
            {

                foreach (DataRow dr in data.Rows)
                {
                    string str = "0";
                    string cardstate = "1";
                    string jobdeptid = dr["jobdeptid"].ToString();//作业单位id
                    string dutyUserId = "";
                    string applyUserId = dr["createuserid"].ToString();
                    string BusinessId = dr["id"].ToString();
                    string JobsafetycardId = dr["JobSafetyCardId"].ToString();
                    //if (user != null)
                    //获取当前用户是否有权限操作该条数据
                    string approveName = "";
                    string approveId = "";
                    string approveAccount = "";
                    dr["isrole"] = DetailService.GetCurrentStepRole(BusinessId, user.Account, dr["flowdetailid"] == null ? "" : dr["flowdetailid"].ToString(), out approveName, out approveId, out approveAccount);
                    var flow = DetailService.GetList().Where(x => x.BusinessId == BusinessId && x.Status == 0).ToList().FirstOrDefault();
                    if (flow != null)
                    {
                        if (flow.ProcessorFlag == "3")
                        {
                            dr["OperatorName"] = flow.UserName;
                            dr["OperatorId"] = flow.Id;
                        }
                    }
                    if (str != "1" && (user.UserId == dutyUserId || user.UserId == applyUserId))//开始作业作业负责人或申请人
                    {
                        str = "1";
                    }
                    dr["isoperate"] = str;
                    //var list = JobSafetyCardApplyService.GetList("");
                    //var cardId = JobsafetycardId.Split(',');
                    //foreach (var item in cardId)
                    //{
                    //    var entity = list.Where(x => x.Id == item).FirstOrDefault();
                    //    if (entity != null)
                    //    {
                    //        if (entity.JobState == 1 || entity.JobState == 3 || entity.JobState == 4 || entity.JobState == 5)
                    //            cardstate = "0";
                    //    }
                    //}
                    //dr["cardstate"] = cardstate;

                }
            }
            return data;
        }
        /// <summary>
        /// 获取列表(App)
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetAppPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataTye = DatabaseType.Oracle;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string role = "";
            if (user != null) role = user.RoleName;
            var queryParam = queryJson.ToJObject();

            #region 查表
            pagination.p_kid = "t.id";
            pagination.p_fields = @"t.CreateUserId,t.jobdeptid,t.ApplyNo,t.JobType,t.JobDeptName,t.JobLevel,t.JobSafetyCardId,t.JobTypeName,''cardstate,
                        t.JobPlace,t.JobStartTime,t.JobEndTime,t.RealityJobStartTime,t.RealityJobendTime,t.ApplyUserName,t.ApplyTime,t.IsSubmit,t1.id as flowdetailid,'' as isrole,'' as OperatorName,'' OperatorId,
                        case when t.workoperate='1' then '作业暂停' when realityjobstarttime is not null and realityjobendtime is null then '作业中' when realityjobendtime is not null then '已结束' when jobstate =2 then '即将作业'  else '' end ledgertype,
                        to_char(case when t.workoperate='1' then 5 when realityjobstarttime is not null and realityjobendtime is null then 6 when realityjobendtime is not null then 7 else jobstate end) as jobstate,
                        '' as isoperate, '' as isrolestate";
            pagination.p_tablename = @"BIS_JobApprovalForm t left join BIS_DangerousJobFlowDetail t1
                                        on t.id=t1.businessid and t1.status=0";
            if (pagination.sidx == null)
            {
                pagination.sidx = "t.createdate";
            }
            if (pagination.sord == null)
            {
                pagination.sord = "desc";
            }
            #endregion

            if (!queryParam["code"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.jobdeptcode like '%{0}%' ", queryParam["code"].ToString());
            }
            //关键字
            if (!queryParam["keyword"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and (t.applyno like '%{0}%' or t.jobcontent like '%{0}%' or t.jobplace like '%{0}%') ", queryParam["keyword"].ToString());
            }
            //作业许可状态
            if (!queryParam["jobstate"].IsEmpty())
            {
                var status = queryParam["jobstate"].ToString();

                if (status == "5")
                {
                    pagination.conditionJson += string.Format(" and t.workoperate='1'");
                }
                else if (status == "6")
                {
                    pagination.conditionJson += string.Format(" and t.realityjobstarttime is not null and t.realityjobendtime is null ");
                }
                else if (status == "7")
                {
                    pagination.conditionJson += string.Format(" and t.realityjobendtime is not null and WorkOperate=0 ");
                }
                else if (status == "2")
                {
                    var showRange = queryParam["showrange"].ToString();
                    //台账 
                    if (showRange == "2")
                    {
                        pagination.conditionJson += string.Format(" and t.jobstate='{0}'  AND t.realityjobstarttime is  null and t.realityjobendtime is null", status);
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(" and t.jobstate='{0}'", status);
                    }
                }
                else
                {
                    pagination.conditionJson += string.Format(" and t.jobstate='{0}'", status);
                }
            }
            if (!queryParam["joblevel"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.joblevel='{0}' ", queryParam["joblevel"]);
            }
            //作业类型
            if (!queryParam["jobtype"].IsEmpty())
            {
                var jobtype = queryParam["jobtype"].ToString();
                pagination.conditionJson += string.Format(" and t.jobtype like '%{0}%' ", jobtype);
            }
            //开始时间
            if (!queryParam["jobstarttime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and  (t.jobstarttime>=to_date('{0}','yyyy-mm-dd')  or t.realityjobstarttime>=to_date('{0}','yyyy-mm-dd'))", queryParam["jobstarttime"]);
            }
            //结束时间
            if (!queryParam["jobendtime"].IsEmpty())
            {
                var eTime = (Convert.ToDateTime(queryParam["jobendtime"].ToString()).AddDays(1)).ToString("yyyy-MM-dd");
                pagination.conditionJson += string.Format(" and (t.jobendtime<=to_date('{0}','yyyy-mm-dd') or t.realityjobendtime>=to_date('{0}','yyyy-mm-dd'))", eTime);
            }
            //查看范围
            if (!queryParam["showrange"].IsEmpty() && user != null)
            {
                var showRange = queryParam["showrange"].ToString();
                if (showRange == "0")//本人申请
                {
                    pagination.conditionJson += string.Format(" and t.createuserid='{0}'", user.UserId);
                }
                else if (showRange == "1")//本人审核
                {
                    DataTable dt = BaseRepository().FindTable("select " + pagination.p_kid + "," + pagination.p_fields + " from " + pagination.p_tablename + " where " + pagination.conditionJson);

                    foreach (DataRow dr in dt.Rows)
                    {
                        string BusinessId = dr["id"].ToString();
                        //获取当前用户是否有权限操作该条数据
                        string approveName = "";
                        string approveId = "";
                        string approveAccount = "";
                        dr["isrole"] = DetailService.GetCurrentStepRole(BusinessId, user.Account, dr["flowdetailid"] == null ? "" : dr["flowdetailid"].ToString(), out approveName, out approveId, out approveAccount);
                    }
                    pagination.conditionJson += string.Format(" and t.id in ('{0}')", string.Join("','", dt.Select("isrole ='0'").AsEnumerable().Select(d => d.Field<string>("id")).ToArray()));
                }
                //作业台账
                else if (showRange == "2")
                {
                    pagination.conditionJson += string.Format(" and (t.jobstate >=2 AND  t.jobstate !=4) ");
                }
            }

            DataTable data = this.BaseRepository().FindTableByProcPager(pagination, dataTye);

            if (data != null && data.Rows.Count > 0)
            {

                foreach (DataRow dr in data.Rows)
                {
                    string str = "0";
                    string cardstate = "1";
                    string jobdeptid = dr["jobdeptid"].ToString();//作业单位id
                    string dutyUserId = "";
                    string applyUserId = dr["createuserid"].ToString();
                    string BusinessId = dr["id"].ToString();
                    string JobsafetycardId = dr["JobSafetyCardId"].ToString();
                    //if (user != null)
                    //获取当前用户是否有权限操作该条数据
                    string approveName = "";
                    string approveId = "";
                    string approveAccount = "";
                    dr["isrole"] = DetailService.GetCurrentStepRole(BusinessId, user.Account, dr["flowdetailid"] == null ? "" : dr["flowdetailid"].ToString(), out approveName, out approveId, out approveAccount);
                    var flow = DetailService.GetList().Where(x => x.BusinessId == BusinessId && x.Status == 0).ToList().FirstOrDefault();
                    if (flow != null)
                    {
                        if (flow.ProcessorFlag == "3")
                        {
                            dr["OperatorName"] = flow.UserName;
                            dr["OperatorId"] = flow.Id;
                        }
                    }
                    if (str != "1" && (user.UserId == dutyUserId || user.UserId == applyUserId))//开始作业作业负责人或申请人
                    {
                        str = "1";
                    }
                    dr["isoperate"] = str;
                    //var list = JobSafetyCardApplyService.GetList("");
                    //var cardId = JobsafetycardId.Split(',');
                    //foreach (var item in cardId)
                    //{
                    //    var entity = list.Where(x => x.Id == item).FirstOrDefault();
                    //    if (entity != null)
                    //    {
                    //        //t.jobstate  in ('1', '3', '4', '5', '8')"
                    //        if (entity.JobState == 1 || entity.JobState == 3 || entity.JobState == 4 || entity.JobState == 5)
                    //            cardstate = "0";
                    //    }
                    //}
                    //dr["cardstate"] = cardstate;
                    //状态
                    string jobstate = dr["jobstate"].ToString();
                    string ledgertype = dr["ledgertype"].ToString();
                    if ((jobstate == "0" || jobstate == "4") && applyUserId == user.UserId)
                        dr["isrolestate"] = "0";//申请状态
                    else if ((jobstate == "1") && dr["isrole"].ToString() == "0")
                        dr["isrolestate"] = "2";//申请中
                    else if ((ledgertype == "即将作业" || ledgertype == "作业暂停") && dr["isoperate"].ToString() == "1")
                        dr["isrolestate"] = "6";//开始作业
                    else if ((ledgertype == "作业中") && dr["isoperate"].ToString() == "1")
                        dr["isrolestate"] = "7";//结束作业

                }
            }
            return data;
        }
        /// <summary>
        /// 判断是否可以开始作业
        /// </summary>
        /// <param name="keyvalue"></param>
        /// <returns></returns>
        public string IsLedgerSetting(string keyvalue)
        {
            string cardstate = "1";
            var entity = this.GetEntity(keyvalue);
            var list = JobSafetyCardApplyService.GetList("");
            if (entity != null && !string.IsNullOrEmpty(entity.JobSafetyCardId))
            {
                var cardId = entity.JobSafetyCardId.Split(',');
                foreach (var item in cardId)
                {
                    var cardentity = list.Where(x => x.Id == item).FirstOrDefault();
                    if (cardentity != null)
                    {
                        //t.jobstate  in ('1', '3', '4', '5', '8')"
                        if (cardentity.JobState == 1 || cardentity.JobState == 3 || cardentity.JobState == 4 || cardentity.JobState == 5)
                            cardstate = "0";
                    }
                }
            }
            return cardstate;


        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageView(Pagination pagination, string queryJson)
        {
            DatabaseType dataTye = DatabaseType.Oracle;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string role = "";
            if (user != null) role = user.RoleName;
            var queryParam = queryJson.ToJObject();

            #region 查表
            pagination.p_kid = "t.id";
            pagination.p_fields = @"t.CreateUserId,t.jobdeptid,t.JobState,t.ApplyNo,t.JobType,t.JobDeptName,t.JobLevel,t.JobSafetyCardId,t.JobTypeName,''cardstate,
                        t.JobPlace,t.JobStartTime,t.JobEndTime,t.RealityJobStartTime,t.RealityJobendTime,t.ApplyUserName,t.ApplyTime,t.IsSubmit,t1.id as flowdetailid,'' as isrole,'' as OperatorName,'' OperatorId,
                        case when t.workoperate='1' then '作业暂停' when realityjobstarttime is not null and realityjobendtime is null then '作业中' when realityjobendtime is not null then '已结束' when jobstate =2 then '即将作业'  else '' end ledgertype,'' as isoperate";
            pagination.p_tablename = @"BIS_JobApprovalForm t left join BIS_DangerousJobFlowDetail t1
                                        on t.id=t1.businessid and t1.status=0";
            if (pagination.sidx == null)
            {
                pagination.sidx = "t.createdate";
            }
            if (pagination.sord == null)
            {
                pagination.sord = "desc";
            }
            #endregion
            pagination.conditionJson += string.Format(" and (t.jobstate >=2  AND  t.jobstate !=4)");
            if (!queryParam["code"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.jobdeptcode like '%{0}%' ", queryParam["code"].ToString());
            }
            if (!queryParam["applyno"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.applyno like '%{0}%' ", queryParam["applyno"].ToString());
            }
            //关键字
            if (!queryParam["keyword"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and (t.applyno like '%{0}%' or t.jobcontent like '%{0}%' or t.jobplace like '%{0}%') ", queryParam["keyword"].ToString());
            }
            //作业许可状态
            if (!queryParam["jobstate"].IsEmpty())
            {
                var status = queryParam["jobstate"].ToString();


                if (status == "5")
                {
                    pagination.conditionJson += string.Format(" and t.workoperate='1'");
                }
                else if (status == "6")
                {
                    pagination.conditionJson += string.Format(" and t.realityjobstarttime is not null and t.realityjobendtime is null ");
                }
                else if (status == "7")
                {
                    pagination.conditionJson += string.Format(" and t.realityjobendtime is not null and WorkOperate=0 ");
                }
                else
                {
                    pagination.conditionJson += string.Format(" and t.jobstate='{0}'  AND t.realityjobstarttime is  null and t.realityjobendtime is null", status);
                }
            }
            if (!queryParam["joblevel"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.joblevel = '{0}' ", queryParam["joblevel"]);
            }
            //作业类型
            if (!queryParam["jobtype"].IsEmpty())
            {
                var jobtype = queryParam["jobtype"].ToString();
                pagination.conditionJson += string.Format(" and t.jobtype like '%{0}%' ", jobtype);
            }
            //开始时间
            if (!queryParam["jobstarttime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and  (t.jobstarttime>=to_date('{0}','yyyy-mm-dd') OR t.realityjobstarttime>=to_date('{0}','yyyy-mm-dd'))", queryParam["jobstarttime"]);
            }
            //结束时间
            if (!queryParam["jobendtime"].IsEmpty())
            {
                var eTime = (Convert.ToDateTime(queryParam["jobendtime"].ToString()).AddDays(1)).ToString("yyyy-MM-dd");
                pagination.conditionJson += string.Format(" and (t.jobendtime<=to_date('{0}','yyyy-mm-dd') OR t.realityjobendtime<=to_date('{0}','yyyy-mm-dd'))", eTime);
            }
            DataTable data = this.BaseRepository().FindTableByProcPager(pagination, dataTye);

            return data;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetCardPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string role = user.RoleName;
            //var queryParam = queryJson.ToJObject();
            #region 查表
            pagination.p_kid = "t.id";
            pagination.p_fields = @"t.CreateUserId,t.jobstate,t.applyno,t.jobtype,t.jobdeptname,t.joblevel, jobtypename  ,
                        t.jobplace,t.jobstarttime,t.jobendtime,t.realityjobstarttime,t.applyusername,t.applytime,t.issubmit,t1.id as flowdetailid,'' as isrole";
            pagination.p_tablename = @"bis_JobSafetyCardApply t left join BIS_DangerousJobFlowDetail t1
                                        on t.id=t1.businessid and t1.status=0 and t.applynumber=t1.applynumber";
            if (pagination.sidx == null)
            {
                pagination.sidx = "t.createdate";
            }
            if (pagination.sord == null)
            {
                pagination.sord = "desc";
            }
            #endregion

            //关键字
            if (!queryJson.IsEmpty())
            {
                pagination.conditionJson += string.Format(" and (t.id IN ('{0}')) ", string.Join("','", queryJson.Split(',')));
            }


            DataTable data = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            if (data != null && data.Rows.Count > 0)
            {

                foreach (DataRow dr in data.Rows)
                {

                    string BusinessId = dr["id"].ToString();
                    //获取当前用户是否有权限操作该条数据
                    string approveName = "";
                    string approveId = "";
                    string approveAccount = "";
                    dr["isrole"] = DetailService.GetCurrentStepRole(BusinessId, user.Account, dr["flowdetailid"] == null ? "" : dr["flowdetailid"].ToString(), out approveName, out approveId, out approveAccount);


                }
            }
            return data;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public JobApprovalFormEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 获取实体/链表查
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="moduleno">逐级审核模块编码</param>
        /// <returns></returns>
        public DataTable ConfigurationByWorkList(string keyValue, string moduleno)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string sql = string.Format(@"select t.id,t.flowname,t.moduleno,t.modulename,t.remark,t.serialnum,t.applytype,
t.checkdeptid,t.checkdeptcode,t.checkdeptname,t.checkrolename,t.checkroleid,t1.businessid,t1.userid,t1.useraccount as account,t1.username ,t.ChoosePersonTitle,t.ChoosePersonWarn
from bis_manypowercheck t left join
(select * from BIS_DangerousJobFlow t1 where t1.businessid='{0}') t1 on t.id=t1.flowid
where t.moduleno='{1}'  ORDER by t.SERIALNUM ", keyValue, moduleno);

            return this.BaseRepository().FindTable(sql);

        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IList<JobApprovalFormEntity> GetJobSafetyCardApplyPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var queryParam = queryJson.ToJObject();
            string sql = string.Format(@"select  t.JOBSAFETYCARDID from BIS_JOBAPPROVALFORM  t ");
            if (!queryParam["keyvalue"].IsEmpty())
                sql = string.Format(@"select  t.JOBSAFETYCARDID from BIS_JOBAPPROVALFORM  t WHERE  id not in ('{0}')", queryParam["keyvalue"].ToString());
            //if (user != null)
            //{
            //    if (!queryParam["keyvalue"].IsEmpty())
            //        sql = string.Format(@"select  t.JOBSAFETYCARDID from BIS_JOBAPPROVALFORM  t  where t.CreateUserDeptCode = '{0}' AND   id not in ('{1}')", user.DeptCode, queryParam["keyvalue"].ToString());
            //    else
            //        sql = string.Format(@"select  t.JOBSAFETYCARDID from BIS_JOBAPPROVALFORM  t  where t.CreateUserDeptCode = '{0}'", user.DeptCode);
            //}
            var data = this.BaseRepository().FindList(sql).Where(x => x.JobSafetyCardId != null).Select(x => x.JobSafetyCardId).ToList();
            string str = string.Empty;
            List<string> ilist = new List<string>();
            foreach (var item in data)
            {
                if (item != null)
                {
                    var lists = item.Split(',');
                    foreach (var item1 in lists)
                    {
                        ilist.Add(item1);
                    }
                    //str +=  string.Join("','", item.Split(','))+",";
                }
            }
            str = string.Join("','", ilist).TrimEnd(',');
            string role = "";
            if (user != null) role = user.RoleName;

            #region 查表
            pagination.p_kid = "t.id";
            pagination.p_fields = @"t.CreateUserId,t.jobstate,t.applyno,t.jobtype,t.jobdeptname, jobtypename  ,
                        t.jobplace,t.jobstarttime,t.jobendtime,t.realityjobstarttime,t.realityjobendtime,t.applyusername,t.applytime,t.issubmit,t1.id as CancelUserId,'' as isrole";
            pagination.p_tablename = @"BIS_JobSafetyCardApply t left join BIS_DangerousJobFlowDetail t1
                                        on t.id=t1.businessid and t1.status=0 and t.applynumber=t1.applynumber";
            if (pagination.sidx == null)
            {
                pagination.sidx = "t.createdate";
            }
            if (pagination.sord == null)
            {
                pagination.sord = "desc";
            }
            #endregion
            if (!queryParam["code"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.jobdeptcode like '%{0}%' ", queryParam["code"].ToString());
            }
            //关键字
            if (!queryParam["keyword"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and (t.applyno like '%{0}%' or t.jobcontent like '%{0}%' or t.jobplace like '%{0}%') ", queryParam["keyword"].ToString());
            }
            //pagination.conditionJson += string.Format(" and t.jobstate   not   in ('0', '9', '10', '11')");
            //作业许可状态（审批中审批通过作业）
            pagination.conditionJson += string.Format(" and t.jobstate  in ('1', '3', '4','5','8')");
            //作业类型
            if (!queryParam["jobtype"].IsEmpty())
            {
                var jobtype = string.Join("','", queryParam["jobtype"].ToString().Split(','));
                pagination.conditionJson += string.Format(" and t.jobtype in ('{0}') ", jobtype);
            }
            //开始时间
            if (!queryParam["JobStartTime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and  jobstarttime>=to_date('{0}','yyyy-mm-dd')", queryParam["jobstarttime"].IsEmpty());
            }
            //结束时间
            if (!queryParam["jobendtime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and jobendtime<=to_date('{0}','yyyy-mm-dd')", queryParam["jobendtime"].IsEmpty());
            }
            if (!string.IsNullOrEmpty(str))
            {
                pagination.conditionJson += string.Format(" and t.ID NOT　IN ('{0}') ", str);
            }
            //pagination.conditionJson += string.Format(" and ");
            //查看范围
            if (!queryParam["showrange"].IsEmpty())
            {
                var showRange = queryParam["showrange"].ToString();
                if (showRange == "0")//本人申请
                {
                    pagination.conditionJson += string.Format(" and t.createuserid='{0}'", user.UserId);
                }
                else if (showRange == "1")//本人审核
                {
                    pagination.conditionJson += string.Format(" and t.createuserid='{0}'", user.UserId);
                }
            }


            var list = this.BaseRepository().FindListByProcPager(pagination, dataType).ToList();
            if (list != null && list.Count > 0)
            {
                foreach (var dr in list)
                {
                    string BusinessId = dr.Id.ToString();
                    //获取当前用户是否有权限操作该条数据
                    string approveName = "";
                    string approveId = "";
                    string approveAccount = "";
                    var roles = DetailService.GetCurrentStepRole(BusinessId, user.Account, dr.CancelUserId == null ? "" : dr.CancelUserId.ToString(), out approveName, out approveId, out approveAccount);
                    dr.ApplyUserName = approveName;
                    dr.ApplyUserId = approveId;
                }
            }
            return list;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetJobSafetyCardApplyList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var queryParam = queryJson.ToJObject();
            string sql = string.Format(@"select  t.JOBSAFETYCARDID from BIS_JOBAPPROVALFORM  t");
            //if (user != null)
            //{
            //    sql = string.Format(@"select  t.JOBSAFETYCARDID from BIS_JOBAPPROVALFORM  t  where t.CreateUserDeptCode = '{0}' ", user.DeptCode);
            //}
            var data = this.BaseRepository().FindList(sql).Select(x => x.JobSafetyCardId).ToList();
            string str = string.Empty;
            List<string> ilist = new List<string>();
            foreach (var item in data)
            {
                var lists = item.Split(',');
                foreach (var item1 in lists)
                {
                    ilist.Add(item1);
                }
            }
            str = string.Join("','", ilist).TrimEnd(',');
            string role = "";
            if (user != null) role = user.RoleName;

            #region 查表
            pagination.p_kid = "t.id";
            pagination.p_fields = @"t.CreateUserId,t.jobstate,t.applyno,t.jobtype,t.jobdeptname, jobtypename  ,
                        t.jobplace,t.jobstarttime,t.jobendtime,t.realityjobstarttime,t.realityjobendtime,t.applyusername,t.applytime,t.issubmit,t1.id as flowdetailid,'' as isrole";
            pagination.p_tablename = @"BIS_JobSafetyCardApply t left join BIS_DangerousJobFlowDetail t1
                                         on t.id=t1.businessid and t1.status=0 and t.applynumber=t1.applynumber";
            if (pagination.sidx == null)
            {
                pagination.sidx = "t.createdate";
            }
            if (pagination.sord == null)
            {
                pagination.sord = "desc";
            }
            #endregion
            if (!queryParam["code"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.jobdeptcode like '%{0}%' ", queryParam["code"].ToString());
            }
            //关键字
            if (!queryParam["keyword"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and (t.applyno like '%{0}%' or t.jobcontent like '%{0}%' or t.jobplace like '%{0}%') ", queryParam["keyword"].ToString());
            }
            //作业许可状态
            if (!queryParam["jobstate"].IsEmpty())
            {
                var status = queryParam["jobstate"].ToString();
                pagination.conditionJson += string.Format(" and t.jobstate in'{0}' ", status);
            }
            //作业类型
            if (!queryParam["jobtype"].IsEmpty())
            {
                var jobtype = string.Join("','", queryParam["jobtype"].ToString().Split(','));
                pagination.conditionJson += string.Format(" and t.jobtype in ('{0}') ", jobtype);
            }
            //开始时间
            if (!queryParam["JobStartTime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and  jobstarttime>=to_date('{0}','yyyy-mm-dd')", queryParam["jobstarttime"].IsEmpty());
            }
            //结束时间
            if (!queryParam["jobendtime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and jobendtime<=to_date('{0}','yyyy-mm-dd')", queryParam["jobendtime"].IsEmpty());
            }
            if (!string.IsNullOrEmpty(str))
            {
                pagination.conditionJson += string.Format(" and t.ID NOT　IN ('{0}') ", str);
            }
            //pagination.conditionJson += string.Format(" and ");
            //查看范围
            if (!queryParam["showrange"].IsEmpty())
            {
                var showRange = queryParam["showrange"].ToString();
                if (showRange == "0")//本人申请
                {
                    pagination.conditionJson += string.Format(" and t.createuserid='{0}'", user.UserId);
                }
                else if (showRange == "1")//本人审核
                {
                    pagination.conditionJson += string.Format(" and t.createuserid='{0}'", user.UserId);
                }
            }


            var dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return dt;
        }


        /// <summary>
        /// 作业级别统计(统计图)
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public string DangerousJobLevelCount(string starttime, string endtime, string deptid, string deptcode)
        {
            DataTable dt = GetDangerousJobTable(starttime, endtime, deptid, deptcode);
            List<object> items = new List<object>();
            foreach (DataRow item in dt.Rows)
            {
                items.Add(new { name = item["name"], y = item["y"] });
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(items);
        }

        /// <summary>
        /// 作业级别统计(统计表)
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public string DangerousJobLevelList(string starttime, string endtime, string deptid, string deptcode)
        {

            DataTable dt = GetDangerousJobTable(starttime, endtime, deptid, deptcode);
            dt.Columns.Add("percent", typeof(string));
            dt.Columns["name"].ColumnName = "worktype";
            dt.Columns["y"].ColumnName = "typenum";

            int allnum = dt.Rows.Count == 0 ? 0 : Convert.ToInt32(dt.Compute("sum(typenum)", "true"));
            foreach (DataRow item in dt.Rows)
            {
                var count = Convert.ToInt32(item["typenum"].ToString());
                decimal percent = allnum == 0 || count == 0 ? 0 : decimal.Parse(count.ToString()) / decimal.Parse(allnum.ToString());
                item["percent"] = Math.Round(percent * 100, 2) + "%";
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = 1,
                page = 1,
                records = dt.Rows.Count,
                rows = dt
            });


        }

        /// <summary>
        /// 获取作业级别统计数据源
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public DataTable GetDangerousJobTable(string starttime, string endtime, string deptid, string deptcode)
        {
            string queryJson = "";
            //            //根据时间进行筛选
            //            if (!string.IsNullOrEmpty(starttime) && !string.IsNullOrEmpty(endtime))
            //            {
            //                string startTime = starttime + " 00:00:00";
            //                string endTime = endtime + " 23:59:59";
            //                queryJson += string.Format(@"  and ((TO_CHAR(JobStartTime,'yyyy-MM-dd HH24:mm:ss') >= '{0}' and  TO_CHAR(JobStartTime,'yyyy-MM-dd HH24:mm:ss') <= '{1}' )
            //                OR  (TO_CHAR(RealityJobStartTime,'yyyy-MM-dd HH24:mm:ss') >= '{0}' and  TO_CHAR(RealityJobStartTime,'yyyy-MM-dd HH24:mm:ss') <= '{1}')) ", startTime, endTime);
            //            }

            //开始时间
            if (!string.IsNullOrEmpty(starttime) && string.IsNullOrEmpty(endtime))
            {
                queryJson += string.Format(" and  (jobstarttime>=to_date('{0}','yyyy-mm-dd') or realityjobstarttime >=to_date('{0}','yyyy-mm-dd'))", starttime);
            }
            //结束时间
            if (!string.IsNullOrEmpty(endtime) && string.IsNullOrEmpty(starttime))
            {
                queryJson += string.Format(" and (jobstarttime<=to_date('{0}','yyyy-mm-dd') or realityjobstarttime<=to_date('{0}','yyyy-mm-dd'))", Convert.ToDateTime(endtime).AddDays(1).ToString("yyyy-MM-dd"));
            }

            if (!string.IsNullOrEmpty(starttime) && !string.IsNullOrEmpty(endtime))
            {
                queryJson += string.Format(" and ((jobstarttime>=to_date('{0}','yyyy-mm-dd') and jobstarttime<=to_date('{1}','yyyy-mm-dd')) or (realityjobstarttime >=to_date('{0}','yyyy-mm-dd') and realityjobstarttime<=to_date('{1}','yyyy-mm-dd')))", starttime, Convert.ToDateTime(endtime).AddDays(1).ToString("yyyy-MM-dd"));
            }

            if (!string.IsNullOrEmpty(deptcode))
            {
                //queryJson += string.Format(@" and instr((',' || JOBDEPTCODE || ','),(',' || {0} || ','))>0", deptcode);
                queryJson += string.Format(@" and (exists(select departmentid
                       from base_department B
                      where encode like '{0}%' and instr(t.jobdeptid,B.departmentid)>0))", deptcode);
            }

            string sql = string.Format(@"select  d.itemname name,count(id) y from BIS_JOBAPPROVALFORM t
                            left join ( select itemname,itemvalue from base_dataitemdetail where itemid =( select itemid from base_dataitem  where itemcode = 'DangerousJobCheck')) d 
                            on t.JOBLEVEL=d.itemvalue where 1=1 {0}
                            group by d.itemname", queryJson);
            return this.BaseRepository().FindTable(sql);
        }
        /// <summary>
        /// 根据ID获取安全证数据
        /// </summary>
        /// <param name="userids"></param>
        /// <returns></returns>
        public DataTable GetSafetyCardTable(string[] userids)
        {
            var strSql = new StringBuilder();
            string sql = string.Join(",", userids).Replace(",", "','");
            strSql.Append(string.Format(@"      select  t.id,  t.CreateUserId,t.jobstate,t.applyno,t.jobtype,t.jobtypename,t.jobdeptname,
                        t.jobplace,t.jobstarttime,t.jobendtime,t.realityjobstarttime,t.realityjobendtime,t.applyusername,t.applytime,t.issubmit,t1.id as flowdetailid,'' as isrole,recordspersonid,checkpersonid,measurepersonid,powercutpersonid,powergivepersonid
           from BIS_JobSafetyCardApply t left join BIS_DangerousJobFlowDetail t1
                                        on t.id=t1.businessid and t1.status=0 and t.applynumber=t1.applynumber where t.id  in ('{0}') ", sql));



            return this.BaseRepository().FindTable(strSql.ToString());
        }
        /// <summary>
        /// 获取手机端流程图
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public List<CheckFlowData> GetAppFlowList(string keyValue)
        {
            List<CheckFlowData> nodelist = new List<CheckFlowData>();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DataTable dt = GetCheckInfo(keyValue);
            JobApprovalFormEntity entity = GetEntity(keyValue);
            if (dt != null)
            {

                #region 审核node
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    //审核记录
                    if (dr["approveperson"] != null && !string.IsNullOrEmpty(dr["approveperson"].ToString()))
                    {
                        CheckFlowData checkdata = new CheckFlowData();
                        DateTime auditdate;
                        DateTime.TryParse(dr["approvetime"].ToString(), out auditdate);
                        checkdata.auditdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                        checkdata.auditdeptname = dr["approvedeptname"].ToString();
                        checkdata.auditusername = dr["approveperson"].ToString();
                        checkdata.auditstate = dr["checkresult"].ToString() == "0" ? "同意" : "不同意";
                        checkdata.auditremark = "";
                        checkdata.isapprove = "1";
                        checkdata.isoperate = "0";
                        nodelist.Add(checkdata);
                    }
                    else
                    {
                        CheckFlowData checkdata = new CheckFlowData();
                        checkdata.auditdate = "";
                        DataTable dtuser = userservice.GetUserTable(dr["useraccount"].ToString().Split(','));
                        string[] usernames = dtuser.AsEnumerable().Select(d => d.Field<string>("realname")).ToArray();
                        string[] deptnames = dtuser.AsEnumerable().Select(d => d.Field<string>("deptname")).ToArray().GroupBy(t => t).Select(p => p.Key).ToArray();
                        checkdata.auditdeptname = deptnames.Length > 0 ? string.Join(",", deptnames) : "无";
                        checkdata.auditusername = usernames.Length > 0 ? string.Join(",", usernames) : "无";
                        checkdata.auditremark = "";
                        checkdata.isapprove = "0";
                        if (entity.JobState == 1 && dr["status"] != null && !string.IsNullOrEmpty(dr["status"].ToString()) && dr["status"].ToString() == "0")
                        {
                            checkdata.isoperate = "1";
                        }
                        else
                        {
                            checkdata.isoperate = "0";
                        }
                        checkdata.auditstate = "审批中";
                        nodelist.Add(checkdata);
                    }
                }
                #endregion

            }
            return nodelist;
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
        public void SaveForm(string keyValue, JobApprovalFormEntity entity)
        {
            try
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
            catch (Exception ex)
            {

                throw ex;
            }

        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <param name="data">业务对应的逐级审核流程</param>
        /// <param name="arr">页面手动选择的流程审批人json</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, JobApprovalFormEntity entity, List<ManyPowerCheckEntity> data, string arr)
        {
            if (arr.Length > 0)
            {
                DangerousJobFlowService djfService = new DangerousJobFlowService();

                //获取页面手动选择的流程审批人
                List<checkperson> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<checkperson>>(arr);
                if (data != null && data.Count > 0)
                {
                    //保存（提交）前先判断危险作业流程表里是否存在流程，存在就删除
                    var djfList = djfService.GetList().Where(x => x.BusinessId == keyValue).ToList();
                    if (djfList != null && djfList.Count > 0)
                    {
                        foreach (var item in djfList)
                        {
                            djfService.RemoveForm(item.Id);
                        }
                    }

                    //流程流转表
                    DangerousJobFlowDetailEntity entityDetail = new DangerousJobFlowDetailEntity();
                    //保存逐级审核流程到危险作业流程表
                    foreach (ManyPowerCheckEntity mpc in data)
                    {
                        DangerousJobFlowEntity djf = new DangerousJobFlowEntity()
                        {
                            ModuleNo = mpc.MODULENO,//逐级审核模块编号
                            FlowId = mpc.ID,//基础节点id
                            FlowName = mpc.FLOWNAME,//节点名称
                            FlowStep = mpc.SERIALNUM,//流程步骤
                            BusinessId = keyValue,//关联业务数据id
                            ProcessorFlag = mpc.ApplyType,//当前步骤处理标示(0部门加角色，1执行脚本获取业务某个字段，2指定审核人，3业务选择审核人)
                            DeptId = mpc.CHECKDEPTID,
                            DeptCode = mpc.CHECKDEPTCODE,
                            DeptName = mpc.CHECKDEPTNAME,
                            RoleName = mpc.CHECKROLENAME,
                            RoleId = mpc.CHECKROLEID,
                            UserId = mpc.CheckUserId,
                            UserAccount = mpc.CheckUserAccount,
                            UserName = mpc.CheckUserName
                        };

                        if (mpc.ApplyType == "3")
                        {
                            var entity1 = list.Where(x => x.id == mpc.ID).ToList().FirstOrDefault();
                            if (entity1 != null)
                            {
                                djf.UserId = entity1.userid;
                                djf.UserAccount = entity1.account;
                                djf.UserName = entity1.username;
                            }
                        }
                        djfService.SaveForm("", djf);

                    }
                    //如果是提交，流程流转表里新增数据(并且是第一步流程)
                    if (entity.IsSubmit == 1)
                    {
                        DetailService.NextStep(keyValue, 1, entity.ApplyNumber);//流程开始
                    }
                }
            }
            entity.SignUrl = string.IsNullOrWhiteSpace(entity.SignUrl) ? "" : entity.SignUrl.Replace("../..", "");
            bool b = false;
            if (!string.IsNullOrWhiteSpace(keyValue))
            {
                var sl = BaseRepository().FindEntity(keyValue);
                if (sl != null)
                {
                    entity.ModuleName = null;
                    var ItemName = dataitemdetailservice.GetItemName("DangerousJobCheck", entity.JobLevel);
                    string pinyin = string.Empty;
                    if (!string.IsNullOrEmpty(ItemName))
                    {
                        pinyin = BSFramework.Util.Str.PinYin(ItemName);
                        pinyin = pinyin.Substring(0, pinyin.Length - 2).ToUpper();
                        var temp = this.GetList("").Where(x => x.ApplyNo != null).Where(t => t.ApplyNo.StartsWith(pinyin + DateTime.Now.ToString("yyyyMMdd"))).OrderByDescending(t => t.ApplyNo).FirstOrDefault();
                        if (temp == null)
                        {
                            entity.ApplyNo = pinyin + DateTime.Now.ToString("yyyyMMdd") + "001";
                        }
                        else
                        {
                            entity.ApplyNo = temp.ApplyNo.Substring(0, temp.ApplyNo.Length - 3) + (int.Parse(temp.ApplyNo.Substring(temp.ApplyNo.Length - 3, 3)) + 1).ToString().PadLeft(3, '0');
                        }
                    }
                    entity.Items = null;
                    entity.JobLevelName = null;
                    entity.Items = null;
                    entity.ModuleName = null;
                    entity.DeleteFileIds = null;
                    entity.File = null;
                    entity.CheckInfo = null;
                    entity.conditionitems = null;
                    entity.checkflow = null;
                    entity.OperatorAccount = null;
                    entity.OperatorId = null;
                    entity.OperatorName = null;
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
                else
                {
                    b = true;
                }
            }
            else
            {
                b = true;
            }
            if (b)
            {
                entity.Id = keyValue;
                var ItemName = dataitemdetailservice.GetItemName("DangerousJobCheck", entity.JobLevel);
                string pinyin = string.Empty;
                if (!string.IsNullOrEmpty(ItemName))
                {
                    pinyin = BSFramework.Util.Str.PinYin(ItemName);
                    pinyin = pinyin.Substring(0, pinyin.Length - 2).ToUpper();
                    var temp = this.GetList("").Where(x => x.ApplyNo != null).Where(t => t.ApplyNo.StartsWith(pinyin + DateTime.Now.ToString("yyyyMMdd"))).OrderByDescending(t => t.ApplyNo).FirstOrDefault();
                    if (temp == null)
                    {
                        entity.ApplyNo = pinyin + DateTime.Now.ToString("yyyyMMdd") + "001";
                    }
                    else
                    {
                        entity.ApplyNo = temp.ApplyNo.Substring(0, temp.ApplyNo.Length - 3) + (int.Parse(temp.ApplyNo.Substring(temp.ApplyNo.Length - 3, 3)) + 1).ToString().PadLeft(3, '0');
                    }
                }
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }

        /// <summary>
        /// 修改sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int UpdateData(string sql)
        {
            return this.BaseRepository().ExecuteBySql(sql);
        }
        #endregion

        /// <summary>
        /// 获取流程图
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public Flow GetFlow(string KeyValue)
        {
            List<nodes> nlist = new List<nodes>();
            List<lines> llist = new List<lines>();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DataTable nodeDt = GetCheckInfo(KeyValue);
            JobApprovalFormEntity entity = GetEntity(KeyValue);
            Flow flow = new Flow();
            flow.title = "";
            flow.initNum = 22;
            if (nodeDt != null && nodeDt.Rows.Count > 0)
            {
                #region 创建node对象

                for (int i = 0; i < nodeDt.Rows.Count; i++)
                {
                    DataRow dr = nodeDt.Rows[i];
                    nodes nodes = new nodes();
                    nodes.alt = true;
                    nodes.isclick = false;
                    nodes.css = "";
                    nodes.id = dr["flowid"].ToString(); //主键
                    nodes.img = "";
                    nodes.name = dr["flowname"].ToString();
                    nodes.type = "stepnode";
                    nodes.width = 150;
                    nodes.height = 60;
                    //位置
                    int m = nlist.Count % 4;
                    int n = nlist.Count / 4;
                    if (m == 0)
                    {
                        nodes.left = 120;
                    }
                    else
                    {
                        nodes.left = 120 + ((150 + 60) * m);
                    }
                    if (n == 0)
                    {
                        nodes.top = 54;
                    }
                    else
                    {
                        nodes.top = (n * 100) + 54;
                    }
                    setInfo sinfo = new setInfo();
                    sinfo.NodeName = nodes.name;
                    //审核记录
                    if (dr["approveperson"] != null && !string.IsNullOrEmpty(dr["approveperson"].ToString()))
                    {
                        sinfo.Taged = 1;
                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                        DateTime auditdate;
                        DateTime.TryParse(dr["approvetime"].ToString(), out auditdate);
                        nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                        nodedesignatedata.creatdept = dr["approvedeptname"].ToString();
                        nodedesignatedata.createuser = dr["approveperson"].ToString();
                        nodedesignatedata.status = dr["checkresult"].ToString() == "0" ? "同意" : "不同意";
                        if (i == 0)
                        {
                            nodedesignatedata.prevnode = "无";
                        }
                        else
                        {
                            nodedesignatedata.prevnode = nodeDt.Rows[i - 1]["flowname"].ToString();
                        }
                        nodelist.Add(nodedesignatedata);
                        sinfo.NodeDesignateData = nodelist;
                        nodes.setInfo = sinfo;
                    }
                    else
                    {
                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                        nodedesignatedata.createdate = "无";
                        if (entity.JobState == 1 && dr["status"] != null && !string.IsNullOrEmpty(dr["status"].ToString()) && dr["status"].ToString() == "0")
                        {
                            flow.activeID = dr["flowid"].ToString();
                        }
                        DataTable dtuser = userservice.GetUserTable(dr["useraccount"].ToString().Split(','));
                        string[] usernames = dtuser.AsEnumerable().Select(d => d.Field<string>("realname")).ToArray();
                        string[] deptnames = dtuser.AsEnumerable().Select(d => d.Field<string>("deptname")).ToArray().GroupBy(t => t).Select(p => p.Key).ToArray();
                        nodedesignatedata.createuser = usernames.Length > 0 ? string.Join(",", usernames) : "无";
                        nodedesignatedata.creatdept = deptnames.Length > 0 ? string.Join(",", deptnames) : "无";
                        nodedesignatedata.status = "无";
                        if (i == 0)
                        {
                            nodedesignatedata.prevnode = "无";
                        }
                        else
                        {
                            nodedesignatedata.prevnode = nodeDt.Rows[i - 1]["flowname"].ToString();
                        }
                        nodelist.Add(nodedesignatedata);
                        sinfo.NodeDesignateData = nodelist;
                        nodes.setInfo = sinfo;
                    }
                    nlist.Add(nodes);
                }

                //流程结束节点
                nodes nodes_end = new nodes();
                nodes_end.alt = true;
                nodes_end.isclick = false;
                nodes_end.css = "";
                nodes_end.id = Guid.NewGuid().ToString();
                nodes_end.img = "";
                nodes_end.name = "流程结束";
                nodes_end.type = "endround";
                nodes_end.width = 150;
                nodes_end.height = 60;
                //取最后一流程的位置，相对排位
                nodes_end.left = nlist[nlist.Count - 1].left;
                nodes_end.top = nlist[nlist.Count - 1].top + 100;
                nlist.Add(nodes_end);

                //如果状态为审核通过或不通过，流程结束进行标识 
                if (entity.JobState == 3 || entity.JobState == 4 || entity.JobState == 2 || entity.JobState == 2)
                {
                    setInfo sinfo = new setInfo();
                    sinfo.NodeName = nodes_end.name;
                    sinfo.Taged = 1;
                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                    NodeDesignateData nodedesignatedata = new NodeDesignateData();

                    //取流程结束时的节点信息
                    DataRow[] end_rows = nodeDt.Select("approveperson is not null");
                    if (end_rows.Count() > 0)
                    {
                        DataRow end_row = end_rows[end_rows.Count() - 1];
                        DateTime auditdate;
                        DateTime.TryParse(end_row["approvetime"].ToString(), out auditdate);
                        nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                        nodedesignatedata.creatdept = end_row["approvedeptname"].ToString();
                        nodedesignatedata.createuser = end_row["approveperson"].ToString();
                        nodedesignatedata.status = end_row["checkresult"].ToString() == "0" ? "同意" : "不同意";
                        nodedesignatedata.prevnode = end_row["flowname"].ToString();

                        nodelist.Add(nodedesignatedata);
                        sinfo.NodeDesignateData = nodelist;
                        nodes_end.setInfo = sinfo;
                    }
                }

                #endregion

                #region 创建line对象

                for (int i = 0; i < nlist.Count; i++)
                {
                    lines lines = new lines();
                    lines.alt = true;
                    lines.id = Guid.NewGuid().ToString();
                    lines.from = nlist[i].id;
                    if (i < nlist.Count - 1)
                    {
                        lines.to = nlist[i + 1].id;
                    }
                    lines.name = "";
                    lines.type = "sl";
                    llist.Add(lines);
                }

                lines lines_end = new lines();
                lines_end.alt = true;
                lines_end.id = Guid.NewGuid().ToString();
                lines_end.from = nlist[nlist.Count - 1].id;
                lines_end.to = nodes_end.id;
                llist.Add(lines_end);
                #endregion

                flow.nodes = nlist;
                flow.lines = llist;
            }
            return flow;
        }
        /// <summary>
        /// 获取流程
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public DataTable GetCheckInfo(string KeyValue)
        {
            DataTable dt = new DataTable();
            string sql = string.Format(@"select  d.SignUrl ,c.flowid,c.flowname,c.useraccount,c.userid,c.username,d.checkresult,d.approvetime,d.approvepersonid,d.approveperson,d.approvedeptname,d.approveopinion,d.status from  (select a.id,b.id as flowid,b.flowname,a.applynumber,b.flowstep,b.useraccount,b.userid,b.username
                  from BIS_JOBAPPROVALFORM a
                  left join BIS_DANGEROUSJOBFLOW b
                    on a.id = b.businessid 
                    where a.id='{0}' order by b.flowstep) c
                  left join BIS_DANGEROUSJOBFLOWDETAIL d
                    on d.currentstep = c.flowstep
                   and c.applynumber = d.applynumber
                   and d.businessid=c.id order by c.flowstep", KeyValue);
            dt = this.BaseRepository().FindTable(sql);
            return dt;
        }


        /// <summary>
        /// 变更操作人
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="TransferUserName"></param>
        /// <param name="TransferUserAccount"></param>
        /// <param name="TransferUserId"></param>
        public void ExchangeForm(string keyValue, string TransferUserName, string TransferUserAccount, string TransferUserId)
        {
            try
            {
                var entity = GetEntity(keyValue);
                switch (entity.JobState)
                {
                    case 1: //审核中状态更改流程基础表跟流程流转表的审核人信息
                        var flow = DetailService.GetList().Where(x => x.BusinessId == keyValue && x.Status == 0).ToList().FirstOrDefault();
                        if (flow != null)
                        {
                            if (flow.ProcessorFlag == "3")
                            {
                                flow.UserAccount = TransferUserAccount;
                                flow.UserId = TransferUserId;
                                flow.UserName = TransferUserName;
                                DetailService.SaveForm(flow.Id, flow); //变更流程流转表审核人
                                var baseFlow = flowservice.GetList().Where(t => t.BusinessId == keyValue && t.FlowStep == flow.CurrentStep).FirstOrDefault();
                                if (baseFlow != null)
                                {
                                    baseFlow.UserName = TransferUserName;
                                    baseFlow.UserAccount = TransferUserAccount;
                                    baseFlow.UserId = TransferUserId;
                                    flowservice.SaveForm(baseFlow.Id, baseFlow);  //变更流程基础表审核人
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetPlanApplyGRNum(ERCHTMS.Code.Operator user)
        {
            int count = 0;
            try
            {
                //                count = BaseRepository().FindObject(string.Format(@"select count(0)
                //from hrs_planapply
                //where createuserorgcode='{0}' 
                //and (instr(checkuseraccount,'{1}')>0 or flowstate='结束') and applytype = '个人工作计划' and baseid is null
                //and checkuseraccount like '%{1}%'", user.OrganizeCode, user.Account)).ToInt();

                string sql = string.Format(@" select t.CreateUserId,t.jobdeptid,t.JobState,t.ApplyNo,t.JobType,t.JobDeptName,t.JobLevel,t.JobSafetyCardId,t.JobTypeName,
                        t.JobPlace, t.JobStartTime, t.JobEndTime, t.RealityJobStartTime, t.ApplyUserName, t.ApplyTime, t.IsSubmit, t1.id as flowdetailid, '' as isrole,
                        case when t.workoperate = '1' then '作业暂停' when realityjobstarttime is not null and realityjobendtime is null then '作业中' when realityjobendtime is not null then '已结束' when jobstate = 2 then '即将作业'  else '' end ledgertype, '' as isoperate  from BIS_JobApprovalForm t left
            join BIS_DangerousJobFlowDetail t1
       on t.id = t1.businessid and t1.status = 0");
                var list = BaseRepository().FindList(sql).Select(x => x.Id).ToList();

                string str = string.Join("','", list);
                //this.GetList().Where(x => x.BusinessId == BusinessId && x.Status == 0).ToList().FirstOrDefault();
                //DetailService.GetCurrentStepRole(BusinessId, user.Account, dr["flowdetailid"] == null ? "" : dr["flowdetailid"].ToString());
                //if (dt != null && dt.Rows.Count > 0)
                //{


                //    foreach (DataRow dr in dt.Rows)
                //    {

                //        string BusinessId = dr["Id"].ToString();
                //        if (user != null)
                //            //获取当前用户是否有权限操作该条数据
                //            dr["isrole"] = 
                //        if (dr["isrole"].ToString() == "1")
                //            count++;
                //    }
                //}
            }
            catch
            {
                return 0;
            }
            return count;
        }



    }
}
