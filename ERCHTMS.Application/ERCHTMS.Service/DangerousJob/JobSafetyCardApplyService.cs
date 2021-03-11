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
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace ERCHTMS.Service.DangerousJob
{
    /// <summary>
    /// 描 述：作业安全证申请
    /// </summary>
    public class JobSafetyCardApplyService : RepositoryFactory<JobSafetyCardApplyEntity>, JobSafetyCardApplyIService
    {
        DangerousJobFlowDetailService DetailService = new DangerousJobFlowDetailService();
        DangerousJobFlowService flowservice = new DangerousJobFlowService();
        UserService userservice = new UserService();
        BlindPlateWallSpecService wallservice = new BlindPlateWallSpecService();
        DepartmentService departmentservice = new DepartmentService();
        DangerousJobOperateService dangerousjoboperateservice = new DangerousJobOperateService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<JobSafetyCardApplyEntity> GetList(string queryJson)
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
            pagination.p_fields = string.IsNullOrWhiteSpace(pagination.p_fields) ? @"t.CreateUserId,to_char(t.jobstate) as jobstate,t.applyno,t.jobtype,t.jobtypename,t.jobdeptname,
                        t.jobplace,t.jobstarttime,t.jobendtime,t.realityjobstarttime,t.realityjobendtime,t.applyusername,t.applytime,to_char(t.issubmit) as issubmit,t1.id as flowdetailid,'' as isrole,recordspersonid,recordsperson,checkpersonid,checkperson,measurepersonid,measureperson,powercutpersonid,powercutperson,powergivepersonid,powergiveperson,'' as approvename,'' as approveid,'' as approveaccount,t.createuserid as workoperateid,t.createusername as workoperatename" : pagination.p_fields;
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

            //关键字
            if (!queryParam["keyword"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and (t.applyno like '%{0}%' or t.jobcontent like '%{0}%' or t.jobplace like '%{0}%') ", queryParam["keyword"].ToString());
            }
            //作业许可状态
            if (!queryParam["jobstate"].IsEmpty())
            {
                var status = queryParam["jobstate"].ToString();
                if (status == "12")
                {
                    pagination.conditionJson += string.Format(" and t.jobstate in(5,6,7,8,9,10,11)"); //查询审核通过的数据   审核通过后的状态有备案中、验收中、送电中、开始作业、暂停作业、流程结束
                }
                else if (status == "13")
                {
                    pagination.conditionJson += string.Format(" and t.jobstate in (6,7,11)");
                }
                else
                {
                    pagination.conditionJson += string.Format(" and t.jobstate='{0}' ", status);
                }

            }
            //作业类型
            if (!queryParam["jobtype"].IsEmpty())
            {
                var jobtype = queryParam["jobtype"].ToString();
                pagination.conditionJson += string.Format(" and t.jobtype like '{0}%' ", jobtype);
            }
            //开始时间
            if (!queryParam["jobstarttime"].IsEmpty() && queryParam["jobendtime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and  (jobstarttime>=to_date('{0}','yyyy-mm-dd') or realityjobstarttime >=to_date('{0}','yyyy-mm-dd'))", queryParam["jobstarttime"].ToString());
            }
            //结束时间
            if (!queryParam["jobendtime"].IsEmpty() && queryParam["jobstarttime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and (jobstarttime<=to_date('{0}','yyyy-mm-dd') or realityjobstarttime<=to_date('{0}','yyyy-mm-dd'))",Convert.ToDateTime(queryParam["jobendtime"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            }

            if (!queryParam["jobstarttime"].IsEmpty() && !queryParam["jobendtime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and ((jobstarttime>=to_date('{0}','yyyy-mm-dd') and jobstarttime<=to_date('{1}','yyyy-mm-dd')) or (realityjobstarttime >=to_date('{0}','yyyy-mm-dd') and realityjobstarttime<=to_date('{1}','yyyy-mm-dd')))", queryParam["jobstarttime"].ToString(), Convert.ToDateTime(queryParam["jobendtime"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            }
            //查看范围
            if (!queryParam["showrange"].IsEmpty())
            {
                var showRange = queryParam["showrange"].ToString();
                if (showRange == "0")//本人申请
                {
                    pagination.conditionJson += string.Format(" and t.createuserid='{0}'", user.UserId);
                }
                else if (showRange == "1")//本人处理
                {
                    DataTable dt = BaseRepository().FindTable("select " + pagination.p_kid + "," + pagination.p_fields + " from " + pagination.p_tablename + " where " + pagination.conditionJson + " and t.jobstate=1");
                    foreach (DataRow dr in dt.Rows)
                    {
                        string BusinessId = dr["id"].ToString();
                        //获取当前用户是否有权限操作该条数据
                        string approveName = "";
                        string approveId = "";
                        string approveAccount = "";
                        dr["isrole"] = DetailService.GetCurrentStepRole(BusinessId, user.Account, dr["flowdetailid"] == null ? "" : dr["flowdetailid"].ToString(),out approveName,out approveId,out approveAccount);
                    }
                    pagination.conditionJson += string.Format(" and ((t.jobstate=1 and t.id in ('{0}')) or", string.Join("','", dt.Select("isrole ='0'").AsEnumerable().Select(d => d.Field<string>("id")).ToArray()));
                    pagination.conditionJson += " (t.jobstate=3 and t.measurepersonid like '%" + user.UserId + "%') or ";//待措施确认
                    pagination.conditionJson += " (t.jobstate=4 and t.powercutpersonid like '%" + user.UserId + "%') or ";//待停电
                    pagination.conditionJson += " (t.jobstate=5 and t.recordspersonid like '%" + user.UserId + "%') or ";//待备案
                    pagination.conditionJson += " (t.jobstate=6 and t.checkpersonid like '%" + user.UserId + "%') or ";//待验收
                    pagination.conditionJson += " (t.jobstate=7 and t.powergivepersonid like '%" + user.UserId + "%')) "; //待送电
                }
                //作业台账
                else if (showRange == "2")
                {
                    pagination.conditionJson += string.Format(" and t.jobstate in (6,7,8,9,10,11) ");
                }
            }
            //待办
            if (!queryParam["dbsx"].IsEmpty())
            {
                var dbsx = queryParam["dbsx"].ToString();
                switch (dbsx)
                {
                    case "0":
                        pagination.conditionJson += " and t.jobstate=3 and t.measurepersonid like '%"+ user.UserId + "%'"; //待措施确认
                        break;
                    case "1":
                        pagination.conditionJson += " and t.jobstate=4 and t.powercutpersonid like '%" + user.UserId + "%'";//待停电
                        break;
                    case "2":
                        pagination.conditionJson += " and t.jobstate=5 and t.recordspersonid like '%" + user.UserId + "%'";//待备案
                        break;
                    case "3":
                        pagination.conditionJson += " and t.jobstate=6 and t.checkpersonid like '%" + user.UserId + "%'";//待验收
                        break;
                    case "4":
                        pagination.conditionJson += " and t.jobstate=7 and t.powergivepersonid like '%" + user.UserId + "%'";//待送电
                        break;
                    case "5":
                        pagination.conditionJson += " and t.jobstate=1";
                        DataTable dt = BaseRepository().FindTable("select " + pagination.p_kid + "," + pagination.p_fields + " from " + pagination.p_tablename + " where " + pagination.conditionJson);
                        foreach (DataRow dr in dt.Rows)
                        {
                            string BusinessId = dr["id"].ToString();
                            //获取当前用户是否有权限操作该条数据
                            string approveName = "";
                            string approveId = "";
                            string approveAccount = "";
                            dr["isrole"] = DetailService.GetCurrentStepRole(BusinessId, user.Account, dr["flowdetailid"] == null ? "" : dr["flowdetailid"].ToString(),out approveName,out approveId,out approveAccount);
                        }
                        pagination.conditionJson += string.Format(" and t.id in ('{0}')", string.Join("','", dt.Select("isrole ='0'").AsEnumerable().Select(d => d.Field<string>("id")).ToArray()));
                        break;
                    default:
                        break;
                }
            }
            //作业单位
            if (!queryParam["code"].IsEmpty())
            {
                string deptlist = string.Join(",", departmentservice.GetList().Where(t => t.EnCode.StartsWith(queryParam["code"].ToString())).Select(t => t.DepartmentId).ToArray());
                pagination.conditionJson += string.Format(" and iscontaions(jobdeptid,'{0}')=1", deptlist);
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
                    dr["isrole"] = DetailService.GetCurrentStepRole(BusinessId, user.Account, dr["flowdetailid"] == null ? "" : dr["flowdetailid"].ToString(),out approveName,out approveId,out approveAccount);
                    dr["approvename"] = approveName;
                    dr["approveid"] = approveId;
                    dr["approveaccount"] = approveAccount;
                }
            }
            return data;
        }

        /// <summary>
        /// 获取今日高危作业列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetTodayWorkList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string role = user.RoleName;
            var queryParam = queryJson.ToJObject();
            JobApprovalFormIService JobApprovalFormbll = new JobApprovalFormService();
            var data = JobApprovalFormbll.GetList("").Where(t => t.RealityJobStartTime != null && t.RealityJobEndTime == null).Select(x => x.JobSafetyCardId).ToList();
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
                }
            }
            str = string.Join("','", ilist);
            string where = "";
            if (!string.IsNullOrWhiteSpace(str))
            {
                where += string.Format(" and a.id not in('{0}')", str);
            }
            string sql = string.Format(@"select to_char(a.id) as id,to_char(a.JobDeptName) as workdeptname, to_char(a.jobtypename) as worktypename,to_char(a.jobplace) as workplace, to_char(a.jobcontent) as workcontent, '' as risktypename,to_char(a.JobPerson) as WorkUserNames,
                                       to_char(a.CUSTODIAN) as worktutelageusername,
                                       b.APPROVEPERSON as auditusername,to_char(a.realityjobstarttime,'yyyy-mm-dd hh24:mi') as realityworkstarttime,to_char(a.realityjobendtime,'yyyy-mm-dd hh24:mi') as realityworkendtime,'-6' as worktype
                                  from BIS_JOBSAFETYCARDAPPLY a
                                  left join (select id,businessid,APPROVEPERSON,CHECKRESULT,APPROVETIME,row_number() over(partition by businessid order by APPROVETIME desc) as num
                                               from BIS_DangerousJobFlowDetail) b
                                    on a.id = b.businessid
                                   and b.num = 1
                                 where a.jobstate = 10 and a.createuserorgcode='{0}' {1}
                                union all
                                select  to_char(a.id) as id, to_char(a.JobDeptName) as workdeptname,to_char(a.JOBTYPENAME) as worktypename,to_char(a.jobplace) as workplace,to_char(a.jobcontent) as workcontent,
                                d.itemname as risktypename,to_char(a.JobPerson) as WorkUserNames ,to_char(a.CUSTODIAN) as worktutelageusername,b.APPROVEPERSON as auditusername,to_char(a.realityjobstarttime,'yyyy-mm-dd hh24:mi') as realityworkstarttime,to_char(a.realityjobendtime,'yyyy-mm-dd hh24:mi') as realityworkendtime,'-5' as worktype from BIS_JOBAPPROVALFORM a
                                    left join (select id, businessid,APPROVEPERSON,CHECKRESULT,APPROVETIME,row_number() over(partition by businessid order by APPROVETIME desc) as num
                                   from BIS_DangerousJobFlowDetail) b
                            on a.id = b.businessid
                           and b.num = 1
                           left join (select c.itemname,c.itemvalue from base_dataitemdetail c where c.itemid in (select itemid from base_dataitem where itemcode='DangerousJobCheck')) d on a.joblevel=d.itemvalue
                        where a.realityjobstarttime is not null and a.realityjobendtime is null and a.createuserorgcode='{0}'", user.OrganizeCode, where);
            pagination.p_tablename = "(" + sql + ")";
            pagination.p_fields = "*";
            pagination.conditionJson = "1=1";
            DataTable result = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            
            return result;
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public JobSafetyCardApplyEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 获取实体/链表查
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="moduleno">逐级审核模块编号</param>
        /// <returns></returns>
        public DataTable ConfigurationByWorkList(string keyValue, string moduleno)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string sql = string.Format(@"select t.id,t.flowname,t.moduleno,t.modulename,t.remark,t.serialnum,t.applytype,
t.checkdeptid,t.checkdeptcode,t.checkdeptname,t.checkrolename,t.checkroleid,t1.businessid,t1.userid,t1.useraccount account,t1.username,t.ChoosePersonTitle,t.ChoosePersonWarn
from bis_manypowercheck t left join
(select * from BIS_DangerousJobFlow t1 where t1.businessid='{0}') t1 on t.id=t1.flowid
where t.MODULENO='{1}' and t.CREATEUSERORGCODE='{2}' order by t.serialnum", keyValue, moduleno, user.OrganizeCode);

            return this.BaseRepository().FindTable(sql);

        }

        /// <summary>
        /// 作业类型统计(统计图)
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public string GetDangerousJobCount(string starttime, string endtime, string deptid, string deptcode)
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
        /// 作业类型统计(统计表)
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public string GetDangerousJobList(string starttime, string endtime, string deptid, string deptcode)
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
        /// 获取作业类型统计数据源
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
                //queryJson += string.Format(@" and instr((',' || t.jobdeptcode || ','),(',' || {0} || ','))>0", deptcode);
                queryJson += string.Format(@" and (exists(select departmentid
                       from base_department B
                      where encode like '{0}%' and instr(t.jobdeptid,B.departmentid)>0))", deptcode);
            }

            string sql = string.Format(@"select  d.itemname name,count(id) y from BIS_JOBSAFETYCARDAPPLY t
                            right join (select itemname,itemcode,itemvalue from base_dataitemdetail where itemid = ( select itemid from base_dataitem  where itemcode = 'DangerousJobConfig') and enabledmark = 1 and deletemark = 0 ) d 
                            on t.jobtype=d.itemvalue where 1=1   {0}
                            group by d.itemname, jobtype", queryJson);
            return this.BaseRepository().FindTable(sql);
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageView(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string role = user.RoleName;
            var queryParam = queryJson.ToJObject();
            #region 查表
            pagination.p_kid = "t.id";
            pagination.p_fields = @"t.CreateUserId,t.jobstate,t.applyno,t.jobtype,t.jobtypename,t.jobdeptname,t.jobcontent,
                        t.jobplace,t.jobstarttime,t.jobendtime,t.realityjobstarttime,t.realityjobendtime,t.applyusername,t.applytime,t.issubmit,t1.id as flowdetailid,'' as isrole,recordspersonid,checkpersonid,measurepersonid,powercutpersonid,powergivepersonid";
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
            //审核通过作业台账
            pagination.conditionJson += string.Format(" and t.jobstate in (6,7,8,9,10,11) ");
            //关键字
            if (!queryParam["keyword"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and (t.applyno like '%{0}%' or t.jobcontent like '%{0}%' or t.jobplace like '%{0}%') ", queryParam["keyword"].ToString());
            }
            //作业许可状态
            if (!queryParam["jobstate"].IsEmpty())
            {
                var status = queryParam["jobstate"].ToString();
                if (status == "12")
                {
                    pagination.conditionJson += string.Format(" and t.jobstate in(5,6,7,8,9,10,11)"); //查询审核通过的数据   审核通过后的状态有备案中、验收中、送电中、开始作业、暂停作业、流程结束
                }
                else
                {
                    pagination.conditionJson += string.Format(" and t.jobstate='{0}' ", status);
                }

            }
            //作业类型
            if (!queryParam["jobtype"].IsEmpty())
            {
                var jobtype = queryParam["jobtype"].ToString();
                pagination.conditionJson += string.Format(" and t.jobtype like '{0}%' ", jobtype);
            }
            //开始时间
            if (!queryParam["jobstarttime"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and  (t.jobstarttime>=to_date('{0}','yyyy-mm-dd') OR t.realityjobstarttime>=to_date('{0}','yyyy-mm-dd'))", queryParam["jobstarttime"]);

                //pagination.conditionJson += string.Format(" and  (t.jobstarttime>=to_date('{0}','yyyy-mm-dd'))", queryParam["jobstarttime"]);
            }
            //结束时间
            if (!queryParam["jobendtime"].IsEmpty())
            {
                var eTime = (Convert.ToDateTime(queryParam["jobendtime"].ToString()).AddDays(1)).ToString("yyyy-MM-dd");
                pagination.conditionJson += string.Format(" and (t.jobendtime<=to_date('{0}','yyyy-mm-dd') OR t.realityjobendtime<=to_date('{0}','yyyy-mm-dd'))", eTime);

                //pagination.conditionJson += string.Format(" and (t.jobendtime<=to_date('{0}','yyyy-mm-dd'))", queryParam["jobendtime"]);
            }

            //作业单位
            if (!queryParam["code"].IsEmpty())
            {
                string deptlist = string.Join(",", departmentservice.GetList().Where(t => t.EnCode.StartsWith(queryParam["code"].ToString())).Select(t => t.DepartmentId).ToArray());
                pagination.conditionJson += string.Format(" and iscontaions(jobdeptid,'{0}')=1", deptlist);
            }

            DataTable data = this.BaseRepository().FindTableByProcPager(pagination, dataType);


            return data;
        }

        public DataTable FindTable(string sql)
        {
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
        public void SaveForm(string keyValue, JobSafetyCardApplyEntity entity)
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
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <param name="data">业务对应的逐级审核流程</param>
        /// <param name="arr">页面手动选择的流程审批人json</param>
        /// <param name="arrData">盲板抽堵作业规格</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, JobSafetyCardApplyEntity entity, List<ManyPowerCheckEntity> data, string arr, string arrData)
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

            wallservice.DeleteRec(keyValue);
            if (!string.IsNullOrWhiteSpace(arrData))
            {
                List<BlindPlateWallSpecEntity> wall = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BlindPlateWallSpecEntity>>(arrData);
                foreach (var item in wall)
                {
                    item.RecId = string.IsNullOrWhiteSpace(item.RecId) ? keyValue : item.RecId;
                    wallservice.SaveForm("", item);
                }
            }

            entity.SignUrl = string.IsNullOrWhiteSpace(entity.SignUrl) ? "" : entity.SignUrl.Replace("../..", "");
            entity.ConfirmSignUrl = string.IsNullOrWhiteSpace(entity.ConfirmSignUrl) ? "" : entity.ConfirmSignUrl.Replace("../..", "");
            bool b = false;
            if (!string.IsNullOrWhiteSpace(keyValue))
            {
                var sl = BaseRepository().FindEntity(keyValue);
                if (sl != null)
                {
                    entity.ModuleNo = null;
                    entity.Modify(keyValue);
                    entity.spec = null;
                    entity.approvename = null;
                    entity.approveid = null;
                    entity.approveaccount = null;
                    entity.AnalysisData = null;
                    entity.approveformid = null;
                    entity.arr = null;
                    entity.JobLevelName = null;
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
                string pinyin = BSFramework.Util.Str.PinYin(entity.JobTypeName);
                pinyin = pinyin.Substring(0, pinyin.Length - 2).ToUpper();
                var temp = this.GetList("").Where(t => t.ApplyNo.StartsWith(pinyin + DateTime.Now.ToString("yyyyMMdd"))).OrderByDescending(t => t.ApplyNo).FirstOrDefault();
                if (temp == null)
                {
                    entity.ApplyNo = pinyin + DateTime.Now.ToString("yyyyMMdd") + "001";
                }
                else
                {
                    entity.ApplyNo = temp.ApplyNo.Substring(0, temp.ApplyNo.Length - 3) + (int.Parse(temp.ApplyNo.Substring(temp.ApplyNo.Length - 3, 3)) + 1).ToString().PadLeft(3, '0');
                }
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
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
                    case 3: //措施确认中状态
                        entity.MeasurePerson = TransferUserName;
                        entity.MeasurePersonId = TransferUserId;
                        break;
                    case 4: //停电中状态
                        entity.PowerCutPerson = TransferUserName;
                        entity.PowerCutPersonId = TransferUserId;
                        break;
                    case 5: //备案中状态
                        entity.RecordsPerson = TransferUserName;
                        entity.RecordsPersonId = TransferUserId;
                        break;
                    case 6: //验收中状态
                        entity.CheckPerson = TransferUserName;
                        entity.CheckPersonId = TransferUserId;
                        break;
                    case 7: //送电中
                        entity.PowerGivePerson = TransferUserName;
                        entity.PowerGivePersonId = TransferUserId;
                        break;
                    default:
                        break;
                }
                SaveForm(entity.Id, entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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
            JobSafetyCardApplyEntity entity = GetEntity(KeyValue);
            Flow flow = new Flow();
            flow.title = "";
            flow.initNum = 22;
            if (nodeDt != null && nodeDt.Rows.Count > 0)
            {
                #region 创建node对象

                #region 措施确认node
                if (entity.JobType == "LimitedSpace" || entity.JobType == "EquOverhaulClean")
                {
                    nodes nodes = new nodes();
                    nodes.alt = true;
                    nodes.isclick = false;
                    nodes.css = "";
                    nodes.id = Guid.NewGuid().ToString(); //主键
                    nodes.img = "";
                    nodes.name = "措施确认";
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
                    if (entity.JobState == 3)
                    {
                        flow.activeID = nodes.id;
                    }
                    setInfo sinfo = new setInfo();
                    sinfo.NodeName = nodes.name;
                    if (!string.IsNullOrWhiteSpace(entity.ConfirmMeasures)) //通过确认措施是否有值来判断是否经过措施确认
                    {
                        sinfo.Taged = 1;
                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                        //DateTime auditdate;
                        //DateTime.TryParse(dr["approvetime"].ToString(), out auditdate);
                        //nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                        nodedesignatedata.createdate = "";
                        try
                        {
                            nodedesignatedata.creatdept = departmentservice.GetEntity(userservice.GetEntity(entity.MeasurePersonId).DepartmentId).FullName;
                        }
                        catch (Exception ex)
                        {
                            nodedesignatedata.creatdept = "无";
                        }

                        nodedesignatedata.createuser = entity.MeasurePerson;
                        nodedesignatedata.status = "同意";
                        if (nlist.Count == 0)
                        {
                            nodedesignatedata.prevnode = "无";
                        }
                        else
                        {
                            nodedesignatedata.prevnode = nlist[nlist.Count - 1].name;
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
                        try
                        {
                            nodedesignatedata.creatdept = departmentservice.GetEntity(userservice.GetEntity(entity.MeasurePersonId).DepartmentId).FullName;
                        }
                        catch (Exception ex)
                        {
                            nodedesignatedata.creatdept = "无";
                        }
                        nodedesignatedata.createuser = entity.MeasurePerson;
                        nodedesignatedata.status = "无";
                        if (nlist.Count == 0)
                        {
                            nodedesignatedata.prevnode = "无";
                        }
                        else
                        {
                            nodedesignatedata.prevnode = nlist[nlist.Count - 1].name;
                        }
                        nodelist.Add(nodedesignatedata);
                        sinfo.NodeDesignateData = nodelist;
                        nodes.setInfo = sinfo;
                    }
                    nlist.Add(nodes);
                }

                #endregion

                #region 停电node
                if (entity.JobType == "EquOverhaulClean")
                {
                    nodes nodes = new nodes();
                    nodes.alt = true;
                    nodes.isclick = false;
                    nodes.css = "";
                    nodes.id = Guid.NewGuid().ToString(); //主键
                    nodes.img = "";
                    nodes.name = "停电";
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
                    if (entity.JobState == 4)
                    {
                        flow.activeID = nodes.id;
                    }
                    setInfo sinfo = new setInfo();
                    sinfo.NodeName = nodes.name;
                    var emptyele = dangerousjoboperateservice.GetList("").Where(t => t.RecId == KeyValue && t.OperateType == 2).OrderByDescending(t=>t.CreateDate).FirstOrDefault();
                    if (emptyele != null) //通过是否有停电记录来判断是否有停电
                    {
                        sinfo.Taged = 1;
                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                        //DateTime auditdate;
                        //DateTime.TryParse(dr["approvetime"].ToString(), out auditdate);
                        nodedesignatedata.createdate = Convert.ToDateTime(emptyele.OperateTime).ToString("yyyy-MM-dd HH:mm");
                        nodedesignatedata.creatdept = emptyele.OperatePersonDept;
                        nodedesignatedata.createuser = emptyele.OperatePerson;
                        nodedesignatedata.status = "同意";
                        if (nlist.Count == 0)
                        {
                            nodedesignatedata.prevnode = "无";
                        }
                        else
                        {
                            nodedesignatedata.prevnode = nlist[nlist.Count - 1].name;
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
                        DataTable dtuser = userservice.GetUserTable(entity.PowerCutPersonId.Split(','));
                        string[] usernames = dtuser.AsEnumerable().Select(d => d.Field<string>("realname")).ToArray();
                        string[] deptnames = dtuser.AsEnumerable().Select(d => d.Field<string>("deptname")).ToArray().GroupBy(t => t).Select(p => p.Key).ToArray();
                        nodedesignatedata.createuser = usernames.Length > 0 ? string.Join(",", usernames) : "无";
                        nodedesignatedata.creatdept = deptnames.Length > 0 ? string.Join(",", deptnames) : "无";
                        nodedesignatedata.status = "无";
                        if (nlist.Count == 0)
                        {
                            nodedesignatedata.prevnode = "无";
                        }
                        else
                        {
                            nodedesignatedata.prevnode = nlist[nlist.Count - 1].name;
                        }
                        nodelist.Add(nodedesignatedata);
                        sinfo.NodeDesignateData = nodelist;
                        nodes.setInfo = sinfo;
                    }
                    nlist.Add(nodes);
                }
                #endregion

                #region 审核node
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
                #endregion

                #region 备案node
                if (entity.JobType == "Digging")
                {
                    nodes nodes = new nodes();
                    nodes.alt = true;
                    nodes.isclick = false;
                    nodes.css = "";
                    nodes.id = Guid.NewGuid().ToString(); //主键
                    nodes.img = "";
                    nodes.name = "备案";
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
                    if (entity.JobState == 5)
                    {
                        flow.activeID = nodes.id;
                    }
                    setInfo sinfo = new setInfo();
                    sinfo.NodeName = nodes.name;
                    var emptyele = dangerousjoboperateservice.GetList("").Where(t => t.RecId == KeyValue && t.OperateType == 1).OrderByDescending(t => t.CreateDate).FirstOrDefault();
                    if (emptyele != null) //通过是否有备案记录来判断是否备案
                    {
                        sinfo.Taged = 1;
                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                        //DateTime auditdate;
                        //DateTime.TryParse(dr["approvetime"].ToString(), out auditdate);
                        nodedesignatedata.createdate = Convert.ToDateTime(emptyele.OperateTime).ToString("yyyy-MM-dd HH:mm");
                        nodedesignatedata.creatdept = emptyele.OperatePersonDept;
                        nodedesignatedata.createuser = emptyele.OperatePerson;
                        nodedesignatedata.status = "同意";
                        if (nlist.Count == 0)
                        {
                            nodedesignatedata.prevnode = "无";
                        }
                        else
                        {
                            nodedesignatedata.prevnode = nlist[nlist.Count - 1].name;
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
                        DataTable dtuser = userservice.GetUserTable(entity.RecordsPersonId.Split(','));
                        string[] usernames = dtuser.AsEnumerable().Select(d => d.Field<string>("realname")).ToArray();
                        string[] deptnames = dtuser.AsEnumerable().Select(d => d.Field<string>("deptname")).ToArray().GroupBy(t => t).Select(p => p.Key).ToArray();
                        nodedesignatedata.createuser = usernames.Length > 0 ? string.Join(",", usernames) : "无";
                        nodedesignatedata.creatdept = deptnames.Length > 0 ? string.Join(",", deptnames) : "无";
                        nodedesignatedata.status = "无";
                        if (nlist.Count == 0)
                        {
                            nodedesignatedata.prevnode = "无";
                        }
                        else
                        {
                            nodedesignatedata.prevnode = nlist[nlist.Count - 1].name;
                        }
                        nodelist.Add(nodedesignatedata);
                        sinfo.NodeDesignateData = nodelist;
                        nodes.setInfo = sinfo;
                    }
                    nlist.Add(nodes);
                }
                #endregion

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

                //判断状态值是否大于等于8即是否开始作业或者送电或者验收状态
                if (entity.JobState >= 8)
                {
                    setInfo sinfo = new setInfo();
                    sinfo.NodeName = nodes_end.name;
                    sinfo.Taged = 1;
                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                    NodeDesignateData nodedesignatedata = new NodeDesignateData();

                    //取流程结束时的节点信息
                    DataRow[] end_rows = nodeDt.Select("approveperson is not null");
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
        /// 获取手机端流程图
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public List<CheckFlowData> GetAppFlowList(string keyValue)
        {
            List<CheckFlowData> nodelist = new List<CheckFlowData>();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DataTable dt = GetCheckInfo(keyValue);
            JobSafetyCardApplyEntity entity = GetEntity(keyValue);
            if (dt != null)
            {
                #region 措施确认node
                if (entity.JobType == "LimitedSpace" || entity.JobType == "EquOverhaulClean")
                {
                    if (!string.IsNullOrWhiteSpace(entity.ConfirmMeasures)) //通过确认措施是否有值来判断是否经过措施确认
                    {
                        CheckFlowData checkdata = new CheckFlowData();
                        DateTime auditdate;
                        checkdata.auditdate = "";
                        try
                        {
                            checkdata.auditdeptname = departmentservice.GetEntity(userservice.GetEntity(entity.MeasurePersonId).DepartmentId).FullName;
                        }
                        catch (Exception ex)
                        {
                            checkdata.auditdeptname = "无";
                        }
                        
                        checkdata.auditusername = entity.MeasurePerson;
                        checkdata.auditstate = "已措施确认";
                        checkdata.auditremark = "";
                        checkdata.isapprove = "1";
                        checkdata.isoperate = "0";
                        nodelist.Add(checkdata);
                    }
                    else
                    {
                        CheckFlowData checkdata = new CheckFlowData();
                        checkdata.auditdate = "";
                        try
                        {
                            checkdata.auditdeptname = departmentservice.GetEntity(userservice.GetEntity(entity.MeasurePersonId).DepartmentId).FullName;
                        }
                        catch (Exception ex)
                        {
                            checkdata.auditdeptname = "无";
                        }
                        checkdata.auditusername = entity.MeasurePerson;
                        checkdata.auditremark = "";
                        checkdata.isapprove = "0";
                        if (entity.JobState == 3)
                        {
                            checkdata.isoperate = "1";
                        }
                        else
                        {
                            checkdata.isoperate = "0";
                        }
                        checkdata.auditstate = "措施确认中";
                        nodelist.Add(checkdata);
                    }
                }

                #endregion
                #region 停电node
                if (entity.JobType == "EquOverhaulClean")
                {
                    var emptyele = dangerousjoboperateservice.GetList("").Where(t => t.RecId == keyValue && t.OperateType == 2).OrderByDescending(t => t.CreateDate).FirstOrDefault();
                    if (emptyele != null) //通过是否有停电记录来判断是否有停电
                    {
                        CheckFlowData checkdata = new CheckFlowData();
                        checkdata.auditdate = Convert.ToDateTime(emptyele.OperateTime).ToString("yyyy-MM-dd HH:mm");
                        checkdata.auditdeptname = emptyele.OperatePersonDept;
                        checkdata.auditusername = emptyele.OperatePerson;
                        checkdata.auditstate = "已停电";
                        checkdata.auditremark = "";
                        checkdata.isapprove = "1";
                        checkdata.isoperate = "0";
                        nodelist.Add(checkdata);
                    }
                    else
                    {
                        CheckFlowData checkdata = new CheckFlowData();
                        checkdata.auditdate = "";
                        DataTable dtuser = userservice.GetUserTable(entity.PowerCutPersonId.Split(','));
                        string[] usernames = dtuser.AsEnumerable().Select(d => d.Field<string>("realname")).ToArray();
                        string[] deptnames = dtuser.AsEnumerable().Select(d => d.Field<string>("deptname")).ToArray().GroupBy(t => t).Select(p => p.Key).ToArray();
                        checkdata.auditdeptname = deptnames.Length > 0 ? string.Join(",", deptnames) : "无";
                        checkdata.auditusername = usernames.Length > 0 ? string.Join(",", usernames) : "无";
                        checkdata.auditremark = "";
                        checkdata.isapprove = "0";
                        if (entity.JobState == 4)
                        {
                            checkdata.isoperate = "1";
                        }
                        else
                        {
                            checkdata.isoperate = "0";
                        }
                        checkdata.auditstate = "停电中";
                        nodelist.Add(checkdata);
                    }
                }
                #endregion

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

                #region 备案node
                if (entity.JobType == "Digging")
                {
                    var emptyele = dangerousjoboperateservice.GetList("").Where(t => t.RecId == keyValue && t.OperateType == 1).OrderByDescending(t=>t.CreateDate).FirstOrDefault();
                    if (emptyele != null) //通过是否有备案记录来判断是否备案
                    {
                        CheckFlowData checkdata = new CheckFlowData();
                        checkdata.auditdate = Convert.ToDateTime(emptyele.OperateTime).ToString("yyyy-MM-dd HH:mm");
                        checkdata.auditdeptname = emptyele.OperatePersonDept;
                        checkdata.auditusername = emptyele.OperatePerson;
                        checkdata.auditstate = "已备案";
                        checkdata.auditremark = "";
                        checkdata.isapprove = "1";
                        checkdata.isoperate = "0";
                        nodelist.Add(checkdata);
                    }
                    else
                    {
                        CheckFlowData checkdata = new CheckFlowData();
                        checkdata.auditdate = "";
                        DataTable dtuser = userservice.GetUserTable(entity.RecordsPersonId.Split(','));
                        string[] usernames = dtuser.AsEnumerable().Select(d => d.Field<string>("realname")).ToArray();
                        string[] deptnames = dtuser.AsEnumerable().Select(d => d.Field<string>("deptname")).ToArray().GroupBy(t => t).Select(p => p.Key).ToArray();
                        checkdata.auditdeptname = deptnames.Length > 0 ? string.Join(",", deptnames) : "无";
                        checkdata.auditusername = usernames.Length > 0 ? string.Join(",", usernames) : "无";
                        checkdata.auditremark = "";
                        checkdata.isapprove = "0";
                        if (entity.JobState == 5)
                        {
                            checkdata.isoperate = "1";
                        }
                        else
                        {
                            checkdata.isoperate = "0";
                        }
                        checkdata.auditstate = "备案中";
                        nodelist.Add(checkdata);
                    }
                }
                #endregion

                #region 验收node
                if (entity.JobType == "OpenCircuit")
                {
                    var emptyele = dangerousjoboperateservice.GetList("").Where(t => t.RecId == keyValue && t.OperateType == 0).OrderByDescending(t=>t.CreateDate).FirstOrDefault();
                    if (emptyele != null) //通过是否有验收记录来判断是否验收
                    {
                        CheckFlowData checkdata = new CheckFlowData();
                        checkdata.auditdate = Convert.ToDateTime(emptyele.OperateTime).ToString("yyyy-MM-dd HH:mm");
                        checkdata.auditdeptname = emptyele.OperatePersonDept;
                        checkdata.auditusername = emptyele.OperatePerson;
                        checkdata.auditstate = "已验收";
                        checkdata.auditremark = "";
                        checkdata.isapprove = "1";
                        checkdata.isoperate = "0";
                        nodelist.Add(checkdata);
                    }
                }
                #endregion

                #region 送电node
                if (entity.JobType == "EquOverhaulClean")
                {
                    var emptyele = dangerousjoboperateservice.GetList("").Where(t => t.RecId == keyValue && t.OperateType == 3).OrderByDescending(t=>t.CreateDate).FirstOrDefault();
                    if (emptyele != null) //通过是否有送电记录来判断是否送电
                    {
                        CheckFlowData checkdata = new CheckFlowData();
                        checkdata.auditdate = Convert.ToDateTime(emptyele.OperateTime).ToString("yyyy-MM-dd HH:mm");
                        checkdata.auditdeptname = emptyele.OperatePersonDept;
                        checkdata.auditusername = emptyele.OperatePerson;
                        checkdata.auditstate = "已送电";
                        checkdata.auditremark = "";
                        checkdata.isapprove = "1";
                        checkdata.isoperate = "0";
                        nodelist.Add(checkdata);
                    }
                }
                #endregion
            }
            return nodelist;
        }

        public DataTable GetCheckInfo(string KeyValue)
        {
            DataTable dt = new DataTable();
            string sql = string.Format(@"select c.flowid,c.flowname,c.useraccount,c.userid,c.username,d.checkresult,d.approvetime,d.approveperson,d.approvedeptname,d.approveopinion,d.status from  (select a.id,b.id as flowid,b.flowname,a.applynumber,b.flowstep,b.useraccount,b.userid,b.username
                  from BIS_JOBSAFETYCARDAPPLY a
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
        #endregion

    }
}
