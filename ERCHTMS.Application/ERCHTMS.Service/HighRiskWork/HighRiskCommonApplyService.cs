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
using ERCHTMS.Service.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Entity.BaseManage;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Service.SystemManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Entity.HiddenTroubleManage;
using System.Collections;

namespace ERCHTMS.Service.HighRiskWork
{
    /// <summary>
    /// 描 述：高风险通用作业申请
    /// </summary>
    public class HighRiskCommonApplyService : RepositoryFactory<HighRiskCommonApplyEntity>, HighRiskCommonApplyIService
    {
        private PeopleReviewIService peopleReview = new PeopleReviewService();
        private ManyPowerCheckService manypowercheckservice = new ManyPowerCheckService();

        private HighImportTypeIService importtypeservice = new HighImportTypeService();

        private HighProjectSetIService highprojectsetservice = new HighProjectSetService();
        private DataItemDetailService dataitemdetailservice = new DataItemDetailService();
        private IDepartmentService departmentIService = new DepartmentService();
        private DepartmentService departmentservice = new DepartmentService();
        private UserService userservice = new UserService();

        #region 获取数据

        /// <summary>
        /// 得到当前最大编号
        /// </summary>
        /// <returns></returns>
        public object GetMaxCode()
        {
            string orgCode = OperatorProvider.Provider.Current().OrganizeCode;
            string sql = "select max(ApplyNumber) from bis_highriskcommonapply where CreateUserOrgCode = @orgCode";
            object o = this.BaseRepository().FindObject(sql, new DbParameter[]{
                DbParameters.CreateDbParameter("@orgCode",orgCode)
            });
            return o;
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<HighRiskCommonApplyEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return this.BaseRepository().FindList(pagination);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HighRiskCommonApplyEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HighRiskCommonApplyEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }


        /// <summary>
        /// 获取高风险通用台账
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <param name="GetOperate">是否获取操作权限</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetLedgerList(Pagination pagination, string queryJson,Boolean GetOperate=true)
        {
            DatabaseType dataType = DatabaseType.Oracle;
            #region 数据权限
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string curUserId = user.UserId;
            #endregion
            /*
             即将作业:审批通过且实际作业时间为空
             作业中:审批通过且实际作业开始时间不为空且实际作业结束时间为空
             已结束:审批通过且实际作业结束时间不为空
             */
            #region 查表
            pagination.p_kid = "Id as workid";
            pagination.p_fields = string.IsNullOrWhiteSpace(pagination.p_fields) ? "case when workdepttype=0 then '单位内部' when workdepttype=1 then '外包单位' end workdepttypename,workdepttype,workdeptid,workdeptname,workdeptcode,applynumber,a.CreateDate,workplace,workcontent,workstarttime,workendtime,applyusername,EngineeringName,EngineeringId,worktype as worktypecode,b.itemname as worktype,case when a.workoperate='1' then '作业暂停' when realityworkstarttime is not null and realityworkendtime is null then '作业中' when realityworkendtime is not null then '已结束'  else '即将作业' end ledgertype,a.workdutyuserid,a.RealityWorkStartTime,a.RealityWorkEndTime,'' as isoperate,a.createuserid,a.zmbdutyuserid,a.cmbdutyuserid" : pagination.p_fields;
            pagination.p_tablename = " bis_highriskcommonapply a left join base_dataitemdetail b on a.worktype=b.itemvalue and itemid =(select itemid from base_dataitem where itemcode='CommonType')";
            pagination.conditionJson = "applystate='5'";
            string isAllDataRange = dataitemdetailservice.GetEnableItemValue("HighRiskWorkDataRange");
            if (!user.IsSystem)
            {
                if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级") || !string.IsNullOrEmpty(isAllDataRange))
                {
                    pagination.conditionJson += " and a.createuserorgcode='" + user.OrganizeCode + "'";
                }
                else
                {
                    pagination.conditionJson += string.Format(" and ((workdeptcode in(select encode from base_department where encode like '{0}%'))  or (engineeringid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);
                }
            }
            #endregion

            #region  筛选条件
            var queryParam = JObject.Parse(queryJson);
            if (!queryParam["worktype"].IsEmpty())//作业类型
            {
                pagination.conditionJson += string.Format(" and WorkType='{0}'", queryParam["worktype"].ToString());
            }
            //作业单位
            if (!queryParam["workdeptcode"].IsEmpty() && !queryParam["workdeptid"].IsEmpty())
            {
                pagination.conditionJson += string.Format("  and ((workdeptcode in(select encode from base_department where encode like '{0}%'))  or (engineeringid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", queryParam["workdeptcode"].ToString(), queryParam["workdeptid"].ToString());
            }
            //时间选择
            if (!queryParam["st"].IsEmpty())//作业开始时间
            {
                string from = queryParam["st"].ToString().Trim();
                pagination.conditionJson += string.Format(" and WorkStartTime>=to_date('{0}','yyyy-mm-dd')", from);

            }
            if (!queryParam["et"].IsEmpty())//作业结束时间
            {
                string to = Convert.ToDateTime(queryParam["et"].ToString().Trim()).AddDays(1).ToString("yyyy-MM-dd");
                pagination.conditionJson += string.Format(" and WorkEndTime<=to_date('{0}','yyyy-mm-dd')", to);
            }
            if (!queryParam["ledgertype"].IsEmpty())
            {
                var ledgertype = queryParam["ledgertype"].ToString();
                if (ledgertype == "0")// 即将作业
                {
                    pagination.conditionJson += " and RealityWorkStartTime is null";
                }
                else if (ledgertype == "1")//作业中
                {
                    pagination.conditionJson += " and RealityWorkStartTime is not null and RealityWorkEndTime is null";
                }
                else if (ledgertype == "3") //作业暂停
                {
                    pagination.conditionJson += " and workoperate='1'";
                }
                else//已结束
                {
                    pagination.conditionJson += " and RealityWorkEndTime is not null and workoperate is null";
                }
            }
            if (!queryParam["keyname"].IsEmpty())
            {
                pagination.conditionJson += string.Format("  and (workdeptname like '%{0}%' or engineeringname like '%{0}%')", queryParam["keyname"].ToString());
            }
            if (!queryParam["applynumber"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and applynumber like '%{0}%'", queryParam["applynumber"].ToString());
            }
            #endregion

            //return this.BaseRepository().FindTableByProcPager(pagination, dataType);
            var data = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            #region 操作权限
            if (GetOperate)
            {
                if (data != null)
                {
                    string strRole = dataitemdetailservice.GetItemValue(user.OrganizeId, "LedgerSendDept");//责任部门角色
                    string strManageRole = dataitemdetailservice.GetItemValue(user.OrganizeId, "LedgerManageDept");//安全主管部门监管角色
                    string strWorkRole = dataitemdetailservice.GetItemValue(user.OrganizeId, "LedgerWorkDept");//作业单位角色
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        string str = "0";
                        string dutyUserId = data.Rows[i]["workdutyuserid"].ToString();
                        string applyUserId = data.Rows[i]["createuserid"].ToString();
                        string engineeringid = data.Rows[i]["engineeringid"].ToString();//工程id
                        string workDeptType = data.Rows[i]["workdepttype"].ToString();
                        string workdeptid = data.Rows[i]["workdeptid"].ToString();//作业单位id
                        string zmbdutyuserid = data.Rows[i]["zmbdutyuserid"].ToString();
                        string cmbdutyuserid = data.Rows[i]["cmbdutyuserid"].ToString();
                        string worktypeCode = data.Rows[i]["worktypecode"].ToString();

                        var dept = new OutsouringengineerService().GetEntity(engineeringid); //获取工程id对应的责任部门
                        if (user.RoleName.Contains("厂级") && !string.IsNullOrEmpty(strManageRole))//安全主管部门
                        {
                            string[] arrrole = strManageRole.Split('|');
                            string[] arrrolename = arrrole[0].Split(',');
                            if (arrrole.Length >= 2 && arrrole[1] == "1")
                            {
                                for (int j = 0; j < arrrolename.Length; j++)
                                {
                                    if (user.RoleName.Contains(arrrolename[j]))
                                    {
                                        str = "1";
                                        break;
                                    }
                                }
                            }
                        }
                        string isJdz = new DataItemDetailService().GetItemValue("景德镇版本");
                        if (!string.IsNullOrWhiteSpace(isJdz) && worktypeCode.Equals("11"))
                        {
                            if ((curUserId == zmbdutyuserid || curUserId == cmbdutyuserid || curUserId == applyUserId))//装盲板负责人/拆盲板负责人或申请人
                            {
                                str = "1";
                            }
                        }
                        else
                        {
                            if (str != "1" && !string.IsNullOrEmpty(workdeptid) && !string.IsNullOrEmpty(strWorkRole))
                            {
                                string[] arrrole = strWorkRole.Split('|');
                                string[] arrrolename = arrrole[0].Split(',');
                                if (arrrole.Length >= 2 && arrrole[1] == "1")
                                {
                                    for (int j = 0; j < arrrolename.Length; j++)
                                    {
                                        if (user.RoleName.Contains(arrrolename[j]))
                                        {
                                            str = "1";
                                            break;
                                        }
                                    }
                                }
                            }

                            if (str != "1" && (curUserId == dutyUserId || curUserId == applyUserId))//作业负责人或申请人
                            {
                                str = "1";
                            }
                            if (str != "1" && dept != null)
                            {
                                if (workDeptType == "1")//责任部门
                                {
                                    if (dept.ENGINEERLETDEPTID == user.DeptId && !string.IsNullOrEmpty(strRole))
                                    {
                                        string[] arrrole = strRole.Split('|');
                                        string[] arrrolename = arrrole[0].Split(',');
                                        if (arrrole.Length >= 2 && arrrole[1] == "1")
                                        {
                                            for (int j = 0; j < arrrolename.Length; j++)
                                            {
                                                if (user.RoleName.Contains(arrrolename[j]))
                                                {
                                                    str = "1";
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        data.Rows[i]["isoperate"] = str;
                    }
                }
            }
            
            #endregion
            return data;
        }

        /// <summary>
        /// 获取高风险通用作业申请列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetPageDataTable(Pagination pagination, string queryJson)
        {
            var user = OperatorProvider.Provider.Current();
            string role = user.RoleName;
            string deptid = user.DeptId;

            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            //查询条件
            if (!queryParam["applynumber"].IsEmpty())//申请编号
            {
                pagination.conditionJson += string.Format(" and ApplyNumber like '%{0}%'", queryParam["applynumber"].ToString());
            }
            //查询条件
            if (!queryParam["status"].IsEmpty())//作业许可状态
            {
                pagination.conditionJson += string.Format(" and ApplyState='{0}'", queryParam["status"].ToString());
            }
            if (!queryParam["worktype"].IsEmpty())//作业类型
            {
                pagination.conditionJson += string.Format(" and WorkType='{0}'", queryParam["worktype"].ToString());
            }
            //时间选择
            if (!queryParam["st"].IsEmpty())//作业开始时间
            {
                string from = queryParam["st"].ToString().Trim();
                pagination.conditionJson += string.Format(" and WorkStartTime>=to_date('{0}','yyyy-mm-dd')", from);

            }
            if (!queryParam["et"].IsEmpty())//作业结束时间
            {
                string to = Convert.ToDateTime(queryParam["et"].ToString().Trim()).AddDays(1).ToString("yyyy-MM-dd");
                pagination.conditionJson += string.Format(" and WorkEndTime<=to_date('{0}','yyyy-mm-dd')", to);
            }
            if (!queryParam["indexprocess"].IsEmpty())//正在进行的高风险作业
            {
                pagination.conditionJson += string.Format(" and WorkEndTime>to_date('{0}','yyyy-mm-dd hh24:mi:ss')", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (!queryParam["workdept"].IsEmpty() && !queryParam["workdeptid"].IsEmpty())//作业单位
            {
                pagination.conditionJson += string.Format(" and ((workdeptcode in(select encode from base_department where encode like '{0}%'))  or (engineeringid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", queryParam["workdept"].ToString(), queryParam["workdeptid"].ToString());
            }
            #region 监督任务所用
            //作业单位
            if (!queryParam["taskdeptid"].IsEmpty())
            {
                if (queryParam["tasktype"].ToString() == "0")
                {
                    pagination.conditionJson += string.Format(" and workdeptid='{0}'", queryParam["taskdeptid"].ToString());

                }
                else
                {

                    if (!queryParam["engineeringname"].IsEmpty())
                    {
                        pagination.conditionJson += string.Format(" and WorkDeptId='{0}'", queryParam["taskdeptid"].ToString());
                    }
                    else
                    {
                        var depart = new DepartmentService().GetEntity(queryParam["taskdeptid"].ToString());
                        pagination.conditionJson += string.Format(" and ((workdeptcode in(select encode from base_department where encode like '{0}%'))  or (engineeringid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", depart.EnCode, depart.DepartmentId);

                    }
                }
            }
            //工程名称
            if (!queryParam["engineeringname"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and EngineeringName='{0}'", queryParam["engineeringname"].ToString());
            }
            #endregion
            if (!queryParam["myself"].IsEmpty())
            {
                string[] arrRole = role.Split(',');
                string strWhere = string.Empty;
                string myself = queryParam["myself"].ToString();

                if (myself == "1")//本人申请(任何状态)
                {
                    pagination.conditionJson += string.Format(" and ApplyUserId='{0}'", user.UserId);
                }
                else if (myself == "2" || myself == "3" || myself == "4")//本人待确认,本人待审核(批),本人带操作
                {
                    string strCondition = "";
                    if (myself == "2")
                    {
                        strCondition = " and  a.applystate ='1'";
                    }
                    else if (myself == "3")
                    {
                        strCondition = " and  a.applystate ='3'";
                    }
                    else if (myself == "4")
                    {
                        strCondition = " and a.applystate in('1','3')";
                    }
                    DataTable data = BaseRepository().FindTable("select " + pagination.p_kid + "," + pagination.p_fields + " from " + pagination.p_tablename + " where " + pagination.conditionJson + strCondition);
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        string executedept = string.Empty;
                        //获取执行部门
                        GetExecutedept(data.Rows[i]["workdepttype"].ToString(), data.Rows[i]["workdeptid"].ToString(), data.Rows[i]["engineeringid"].ToString(), out executedept);
                        //获取创建部门
                        string createdetpid = departmentservice.GetEntityByCode(data.Rows[i]["createuserdeptcode"].ToString()).IsEmpty() ? "" : departmentservice.GetEntityByCode(data.Rows[i]["createuserdeptcode"].ToString()).DepartmentId;
                        string outsouringengineerdept = string.Empty;
                        GetOutsouringengineerDept(data.Rows[i]["workdeptid"].ToString(), out outsouringengineerdept);
                        //获取下一步审核人账号
                        string str = manypowercheckservice.GetApproveUserAccount(data.Rows[i]["flowid"].ToString(), data.Rows[i]["id"].ToString(), data.Rows[i]["nextstepapproveuseraccount"].ToString(), data.Rows[i]["specialtytype"].ToString(), executedept, outsouringengineerdept, createdetpid, "", data.Rows[i]["approvedeptid"].ToString());
                        data.Rows[i]["approveuseraccount"] = str;
                    }
                    //获取转交申请人不是本人、转交接收人是本人或者审核人是本人的数据
                    string[] applyids = data.Select(" (outtransferuseraccount is null or outtransferuseraccount not like '%" + user.Account + ",%') and (approveuseraccount like '%" + user.Account + ",%' or intransferuseraccount like '%" + user.Account + ",%')").AsEnumerable().Select(d => d.Field<string>("id")).ToArray();

                    //strCondition += string.Format(" and a.id in ('{0}')", string.Join("','", applyids));
                    pagination.conditionJson += string.Format(" and a.id in ('{0}') {1}", string.Join("','", applyids), strCondition);
                }
            }
            else
            {
                //排除其他人申请保存的数据
                pagination.conditionJson += string.Format("  and a.id  not in(select id from bis_highriskcommonapply where applystate='0' and  applyuserid!='{0}')", user.UserId);
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        public DataTable GetTable(string sql)
        {
            return this.BaseRepository().FindTable(sql);
        }

        /// <summary>
        /// 获取审核流程名称
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string GetModuleName(HighRiskCommonApplyEntity entity)
        {
            string isJdz = new DataItemDetailService().GetItemValue("景德镇版本");
            string moduleName = "(";
            string worktypename = new DataItemDetailService().GetItemName("CommonType", entity.WorkType);
            if (!string.IsNullOrWhiteSpace(isJdz) && entity.WorkType == "11")
            {
                moduleName += worktypename;
                if (entity.NonWorkingApprove == "1")
                {
                    moduleName += "-非工作时段";
                }
                //盲板抽堵作业
                if (entity.WorkDeptType == "1")
                {
                    moduleName += "-外部)审核";
                }
                else
                {
                    moduleName += "-内部)审核";
                }
            }
            else if (!string.IsNullOrWhiteSpace(isJdz) && entity.WorkType == "12")
            {
                //景德镇 高处作业
                switch (entity.RiskType)
                {
                    case "3":
                        moduleName = "一级高处作业";
                        break;
                    case "2":
                        moduleName = "二级高处作业";
                        break;
                    case "1":
                        moduleName = "三级高处作业";
                        break;
                    case "0":
                        moduleName = "特级高处作业";
                        break;
                }
                if (entity.NonWorkingApprove == "1")
                {
                    moduleName += "-非工作时段";
                }
                //外包单位
                if (entity.WorkDeptType == "1")
                {
                    moduleName += "-外部";
                }
                else
                {
                    moduleName += "-内部";
                }
                if (string.IsNullOrEmpty(entity.EffectConfimerId))
                {
                    //受影响相关方确认人为空
                    moduleName += "-无受影响相关方";
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(isJdz) && (worktypename == "有限空间作业" || worktypename == "断路作业" || worktypename == "破土作业"))
                {
                    moduleName += worktypename + "-";
                }
                if (entity.NonWorkingApprove == "1")
                {
                    switch (entity.RiskType)
                    {
                        case "0":
                        case "1":
                            moduleName += "非工作时段-一二级)通用作业";
                            break;
                        case "2":
                        case "3":
                            moduleName += "非工作时段-三四级)通用作业";
                            break;
                        default:
                            break;
                    }
                    //mpcEntity = peopleReview.CheckAuditForNextByWorkUnit(curUser, moduleName, entity.ApproveDeptId, entity.FlowId, false);
                }
                else
                {
                    if (entity.WorkDeptType == "1")
                    {
                        switch (entity.RiskType)
                        {
                            case "0":
                                moduleName += "外部-一级)通用作业";
                                break;
                            case "1":
                                moduleName += "外部-二级)通用作业";
                                break;
                            case "2":
                                moduleName += "外部-三级)通用作业";
                                break;
                            case "3":
                                moduleName += "外部-四级)通用作业";
                                break;
                            default:
                                break;
                        }
                        //mpcEntity = peopleReview.CheckAuditForNextByOutsourcing(curUser, moduleName, entity.WorkDeptId, entity.FlowId, false, true, entity.EngineeringId);
                    }
                    else
                    {
                        switch (entity.RiskType)
                        {
                            case "0":
                                moduleName += "内部-一级)通用作业";
                                break;
                            case "1":
                                moduleName += "内部-二级)通用作业";
                                break;
                            case "2":
                                moduleName += "内部-三级)通用作业";
                                break;
                            case "3":
                                moduleName += "内部-四级)通用作业";
                                break;
                            default:
                                break;
                        }
                        //mpcEntity = peopleReview.CheckAuditForNextByWorkUnit(curUser, moduleName, entity.WorkDeptId, entity.FlowId, false);
                    }
                }
            }
            return moduleName;
        }

        #region 流程图
        /// <summary>
        /// 得到流程图
        /// </summary>
        /// <param name="keyValue">业务表ID</param>
        /// <param name="modulename">逐级审核模块名</param>
        /// <returns></returns>
        public Flow GetFlow(string keyValue, string modulename)
        {
            List<nodes> nlist = new List<nodes>();
            List<lines> llist = new List<lines>();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DataTable nodeDt = GetCheckInfo(keyValue, modulename, user);
            HighRiskCommonApplyEntity entity = GetEntity(keyValue);
            Flow flow = new Flow();
            flow.title = "";
            flow.initNum = 22;
            flow.activeID = entity.FlowId;
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
                    nodes.id = dr["id"].ToString(); //主键
                    nodes.img = "";
                    nodes.name = dr["flowname"].ToString();
                    nodes.type = "stepnode";
                    nodes.width = 150;
                    nodes.height = 60;
                    //位置
                    int m = i % 4;
                    int n = i / 4;
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
                    if (dr["auditdeptname"] != null && !string.IsNullOrEmpty(dr["auditdeptname"].ToString()))
                    {
                        sinfo.Taged = 1;
                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                        DateTime auditdate;
                        DateTime.TryParse(dr["auditdate"].ToString(), out auditdate);
                        nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                        nodedesignatedata.creatdept = dr["auditdeptname"].ToString();
                        nodedesignatedata.createuser = dr["auditusername"].ToString();
                        nodedesignatedata.status = dr["auditstate"].ToString() == "1" ? "同意" : "不同意";
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
                        string executedept = string.Empty;
                        GetExecutedept(entity.WorkDeptType, entity.WorkDeptId, entity.EngineeringId, out executedept);//获取执行部门
                        string createdetpid = departmentservice.GetEntityByCode(entity.CreateUserDeptCode).IsEmpty() ? "" : departmentservice.GetEntityByCode(entity.CreateUserDeptCode).DepartmentId; //获取创建部门ID
                        string outsouringengineerdept = string.Empty;
                        GetOutsouringengineerDept(entity.WorkDeptId, out outsouringengineerdept);
                        string accountstr = manypowercheckservice.GetApproveUserAccount(dr["id"].ToString(), entity.Id, entity.NextStepApproveUserAccount, entity.SpecialtyType, executedept, outsouringengineerdept, createdetpid, "", entity.ApproveDeptId); //获取审核人账号
                        string outtransferuseraccount = dr["outtransferuseraccount"].IsEmpty() ? "" : dr["outtransferuseraccount"].ToString();//转交申请人
                        string intransferuseraccount = dr["intransferuseraccount"].IsEmpty() ? "" : dr["intransferuseraccount"].ToString();//转交接收人
                        string[] outtransferuseraccountlist = outtransferuseraccount.Split(',');
                        string[] intransferuseraccountlist = intransferuseraccount.Split(',');
                        foreach (var item in intransferuseraccountlist)
                        {
                            if (!item.IsEmpty() && !accountstr.Contains(item + ","))
                            {
                                accountstr += (item + ",");//将转交接收人加入审核账号中
                            }
                        }
                        foreach (var item in outtransferuseraccountlist)
                        {
                            if (!item.IsEmpty() && accountstr.Contains(item + ","))
                            {
                                accountstr = accountstr.Replace(item + ",", "");//将转交申请人从审核账号中移除
                            }
                        }

                        DataTable dtuser = userservice.GetUserTable(accountstr.Split(','));
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
                if (entity.InvestigateState == "3")
                {
                    setInfo sinfo = new setInfo();
                    sinfo.NodeName = nodes_end.name;
                    sinfo.Taged = 1;
                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                    NodeDesignateData nodedesignatedata = new NodeDesignateData();

                    //取流程结束时的节点信息
                    DataRow[] end_rows = nodeDt.Select("auditusername is not null").OrderBy(t => t.Field<DateTime>("auditdate")).ToArray();
                    DataRow end_row = end_rows[end_rows.Count() - 1];
                    DateTime auditdate;
                    DateTime.TryParse(end_row["auditdate"].ToString(), out auditdate);
                    nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                    nodedesignatedata.creatdept = end_row["auditdeptname"].ToString();
                    nodedesignatedata.createuser = end_row["auditusername"].ToString();
                    nodedesignatedata.status = end_row["auditstate"].ToString() == "1" ? "同意" : "不同意";
                    nodedesignatedata.prevnode = end_row["flowname"].ToString();

                    nodelist.Add(nodedesignatedata);
                    sinfo.NodeDesignateData = nodelist;
                    nodes_end.setInfo = sinfo;
                }

                #endregion

                #region 创建line对象

                for (int i = 0; i < nodeDt.Rows.Count; i++)
                {
                    lines lines = new lines();
                    lines.alt = true;
                    lines.id = Guid.NewGuid().ToString();
                    lines.from = nodeDt.Rows[i]["id"].ToString();
                    if (i < nodeDt.Rows.Count - 1)
                    {
                        lines.to = nodeDt.Rows[i + 1]["id"].ToString();
                    }
                    lines.name = "";
                    lines.type = "sl";
                    llist.Add(lines);
                }

                lines lines_end = new lines();
                lines_end.alt = true;
                lines_end.id = Guid.NewGuid().ToString();
                lines_end.from = nodeDt.Rows[nodeDt.Rows.Count - 1]["id"].ToString();
                lines_end.to = nodes_end.id;
                llist.Add(lines_end);
                #endregion

                flow.nodes = nlist;
                flow.lines = llist;
            }
            return flow;
        }

        public DataTable GetCheckInfo(string keyValue, string modulename, Operator user)
        {
            string node_sql = string.Format(@"select 
                                                    a.id,a.flowname,a.serialnum,a.autoid,a.checkdeptname,a.checkroleid,a.checkrolename,a.checkdeptid,a.remark,b.auditdeptname,b.auditusername,b.auditdate,b.auditstate,b.auditremark,e.outtransferuseraccount,e.intransferuseraccount
                                              from 
                                                    bis_manypowercheck a left join bis_scaffoldauditrecord b
                                                    on a.id = b.flowid and b.scaffoldid = '{2}'
                                                    left join(select recid,flowid,outtransferuseraccount,intransferuseraccount,row_number() over(partition by recid,flowid order by createdate desc) as num from bis_transferrecord where disable=0 ) e on a.id=e.flowid and e.recid='{2}' and e.num=1
                                              where 
                                                    a.createuserorgcode = '{0}' and a.modulename = '{1}'
                                              order by
                                                    serialnum ", user.OrganizeCode, modulename, keyValue);
            DataTable nodeDt = this.BaseRepository().FindTable(node_sql);
            return nodeDt;
        }


        public List<CheckFlowData> GetAppFlowList(string keyValue, string modulename)
        {
            List<CheckFlowData> nodelist = new List<CheckFlowData>();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            HighRiskCommonApplyEntity entity = GetEntity(keyValue);
            DataTable dt = GetCheckInfo(keyValue, modulename, user);
            if (dt != null)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    //审核记录
                    if (dr["auditdeptname"] != null && !string.IsNullOrEmpty(dr["auditdeptname"].ToString()))
                    {

                        CheckFlowData checkdata = new CheckFlowData();
                        DateTime auditdate;
                        DateTime.TryParse(dr["auditdate"].ToString(), out auditdate);
                        checkdata.auditdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                        checkdata.auditdeptname = dr["auditdeptname"].ToString();
                        checkdata.auditusername = dr["auditusername"].ToString();
                        checkdata.auditstate = dr["auditstate"].ToString() == "1" ? "同意" : "不同意";
                        checkdata.auditremark = dr["auditremark"].ToString() != "确认step" ? dr["auditremark"].ToString() : "";
                        checkdata.isapprove = "1";
                        checkdata.isoperate = "0";
                        nodelist.Add(checkdata);
                    }
                    else
                    {
                        CheckFlowData checkdata = new CheckFlowData();
                        checkdata.auditdate = "";
                        //部门,人员
                        var checkDeptId = dr["checkdeptid"].ToString();
                        var checkremark = dr["remark"].ToString();
                        string type = checkremark != "1" ? "0" : "1";
                        string executedept = string.Empty;
                        GetExecutedept(entity.WorkDeptType, entity.WorkDeptId, entity.EngineeringId, out executedept);//获取执行部门
                        string createdetpid = departmentservice.GetEntityByCode(entity.CreateUserDeptCode).IsEmpty() ? "" : departmentservice.GetEntityByCode(entity.CreateUserDeptCode).DepartmentId; //获取创建部门ID
                        string outsouringengineerdept = string.Empty;
                        GetOutsouringengineerDept(entity.WorkDeptId, out outsouringengineerdept);
                        string accountstr = manypowercheckservice.GetApproveUserAccount(dr["id"].ToString(), entity.Id, entity.NextStepApproveUserAccount, entity.SpecialtyType, executedept, outsouringengineerdept, createdetpid, "", entity.ApproveDeptId); //获取审核人账号
                        string outtransferuseraccount = dr["outtransferuseraccount"].IsEmpty() ? "" : dr["outtransferuseraccount"].ToString();//转交申请人
                        string intransferuseraccount = dr["intransferuseraccount"].IsEmpty() ? "" : dr["intransferuseraccount"].ToString();//转交接收人
                        string[] outtransferuseraccountlist = outtransferuseraccount.Split(',');
                        string[] intransferuseraccountlist = intransferuseraccount.Split(',');
                        foreach (var item in intransferuseraccountlist)
                        {
                            if (!item.IsEmpty() && !accountstr.Contains(item + ","))
                            {
                                accountstr += (item + ",");//将转交接收人加入审核账号中
                            }
                        }
                        foreach (var item in outtransferuseraccountlist)
                        {
                            if (!item.IsEmpty() && accountstr.Contains(item + ","))
                            {
                                accountstr = accountstr.Replace(item + ",", "");//将转交申请人从审核账号中移除
                            }
                        }
                        DataTable dtuser = userservice.GetUserTable(accountstr.Split(','));
                        string[] usernames = dtuser.AsEnumerable().Select(d => d.Field<string>("realname")).ToArray();
                        string[] deptnames = dtuser.AsEnumerable().Select(d => d.Field<string>("deptname")).ToArray().GroupBy(t => t).Select(p => p.Key).ToArray();
                        checkdata.auditusername = usernames.Length > 0 ? string.Join(",", usernames) : "无";
                        checkdata.auditdeptname = deptnames.Length > 0 ? string.Join(",", deptnames) : "无";
                        checkdata.auditremark = "";
                        checkdata.isapprove = "0";
                        if (entity.InvestigateState == "3")
                            checkdata.isoperate = "0";
                        else
                            checkdata.isoperate = dr["id"].ToString() == entity.FlowId ? "1" : "0";
                        if (checkdata.isoperate == "1")
                        {
                            if (dr["flowname"].ToString().Contains("确认"))
                            {
                                checkdata.auditstate = "确认中";
                            }
                            else
                            {
                                checkdata.auditstate = "审核(批)中";
                            }
                        }
                        else
                        {
                            checkdata.auditstate = "";
                        }
                        nodelist.Add(checkdata);
                    }
                }
            }
            return nodelist;
        }
        #endregion
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
                db.Delete<ScaffoldprojectEntity>(t => t.ScaffoldId.Equals(keyValue));
                db.Delete<HighRiskRecordEntity>(t => t.WorkId.Equals(keyValue));
                db.Delete<FileInfoEntity>(t => t.RecId.Equals(keyValue));
                db.Delete<HighRiskCommonApplyEntity>(keyValue);
                db.Delete<HighRiskApplyMBXXEntity>(t => t.HighRiskCommonApplyId.Equals(keyValue));
                db.Commit();
            }
            catch (Exception)
            {
                db.Rollback();
                throw;
            }
        }


        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateForm(HighRiskCommonApplyEntity entity)
        {
            var res = DbFactory.Base().BeginTrans();
            try
            {
                entity.Modify(entity.Id);
                res.Update<HighRiskCommonApplyEntity>(entity);
                res.Commit();
            }
            catch (Exception ex)
            {
                res.Rollback();
                throw;
            }

        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <param name="type">保存,提交</param>
        /// <returns></returns>
        public PushMessageData SaveForm(string keyValue, string type, HighRiskCommonApplyEntity entity, List<HighRiskRecordEntity> list, List<HighRiskApplyMBXXEntity> mbList)
        {
            PushMessageData messagedata = new PushMessageData();
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var res = DbFactory.Base().BeginTrans();
            try
            {
                entity.Id = keyValue;
                var apply = new Repository<HighRiskCommonApplyEntity>(DbFactory.Base());
                var applyc = new Repository<HighRiskCommonApplyEntity>(DbFactory.Base());
                string sql = string.Format("select  ApplyNumber from bis_highriskcommonapply t where createuserorgcode ='{0}' order by createdate desc", ERCHTMS.Code.OperatorProvider.Provider.Current().OrganizeCode);
                if (string.IsNullOrWhiteSpace(entity.ApplyNumber))
                {
                    HighRiskCommonApplyEntity Code = applyc.FindList(sql).ToList().FirstOrDefault();
                    if (Code == null)
                    {
                        var year = DateTime.Now.Year;
                        entity.ApplyNumber = "G" + year + "001";
                    }
                    else
                    {
                        var year = DateTime.Now.Year;
                        entity.ApplyNumber = "G" + year + "00" + (Convert.ToInt32(Code.ApplyNumber.Substring(7)) + 1);
                    }

                }
                #region 保存
                if (type == "0")//保存
                {
                    entity.InvestigateState = "0";//申请阶段
                    entity.ApplyState = "0";
                }
                #endregion
                #region 提交
                string isJdz = new DataItemDetailService().GetItemValue("景德镇版本");
                if (type == "1")//提交
                {
                    string state = string.Empty;
                    ManyPowerCheckEntity mpcEntity = null;
                    string moduleName = GetModuleName(entity);
                    mpcEntity = manypowercheckservice.CheckAuditForNext(curUser, moduleName, entity.FlowId);
                    if (null != mpcEntity)
                    {
                        entity.ApplyState = "1";
                        entity.FlowDept = mpcEntity.CHECKDEPTID;
                        entity.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                        entity.FlowRole = mpcEntity.CHECKROLEID;
                        entity.FlowRoleName = mpcEntity.CHECKROLENAME;
                        entity.FlowId = mpcEntity.ID;
                        entity.FlowApplyType = mpcEntity.NextStepFlowEntity.IsEmpty() ? "" : mpcEntity.NextStepFlowEntity.ApplyType;
                        entity.InvestigateState = "1";//确认阶段
                        entity.FlowName = "确认中";
                        entity.FlowRemark = !string.IsNullOrEmpty(mpcEntity.REMARK) ? mpcEntity.REMARK : "";
                        string checktype = entity.FlowRemark != "1" ? "0" : "1";

                        //推送消息到有审批权限的人
                        messagedata.UserDept = entity.FlowDept;
                        messagedata.UserRole = entity.FlowRole;
                        messagedata.SendCode = "ZY001";
                        messagedata.EntityId = entity.Id;
                        messagedata.IsSpecial = checktype;
                        messagedata.SpecialtyType = !string.IsNullOrEmpty(entity.SpecialtyType) ? entity.SpecialtyType : "";

                    }
                    else
                    {
                        entity.FlowRemark = "";
                        entity.ApplyState = "5";
                        entity.FlowDept = "";
                        entity.FlowDeptName = "";
                        entity.FlowRole = "";
                        entity.FlowRoleName = "";
                        entity.FlowId = "";
                        entity.FlowApplyType = "";
                        entity.ApproveAccount = "";
                        entity.NextStepApproveUserAccount = "";
                        entity.FlowName = "已完结";
                        entity.InvestigateState = "3"; //完结状态

                        messagedata.SendCode = "ZY018";
                        messagedata.EntityId = entity.Id;
                    }
                    var curorgcode = ERCHTMS.Code.OperatorProvider.Provider.Current().OrganizeCode;

                    string str = "1";//默认为手动输入
                    var data = importtypeservice.GetList(string.Format(" and itype='1' and CreateUserOrgCode='{0}'", curorgcode)).FirstOrDefault();

                    if (data != null)
                        str = data.IsImport;
                    if (str == "1")//手动输入
                    {
                        ScaffoldprojectEntity projectEntity = new ScaffoldprojectEntity();
                        //projectEntity.Id = Guid.NewGuid().ToString();
                        projectEntity.ScaffoldId = entity.Id;//申请信息id
                        projectEntity.ResultYes = "符合";
                        projectEntity.ResultNo = "不符合";
                        projectEntity.ProjectName = "";
                        projectEntity.ProjectId = "";
                        projectEntity.Create();
                        res.Insert<ScaffoldprojectEntity>(projectEntity);
                    }
                    else
                    {
                        var highprojectlist = highprojectsetservice.GetList(string.Format(" and typenum='{0}' and CreateUserOrgCode='{1}'", entity.WorkType, curorgcode)).OrderBy(t => Convert.ToInt32(t.OrderNumber));
                        //批量增加安全措施到确认记录表中
                        int i = 0;
                        foreach (HighProjectSetEntity setEntity in highprojectlist)
                        {
                            ScaffoldprojectEntity projectEntity = new ScaffoldprojectEntity();
                            //projectEntity.Id = Guid.NewGuid().ToString();
                            projectEntity.ScaffoldId = entity.Id;//申请信息id
                            projectEntity.ResultYes = setEntity.MeasureResultOne;
                            projectEntity.ResultNo = setEntity.MeasureResultTwo;
                            projectEntity.ProjectName = setEntity.MeasureName;
                            projectEntity.ProjectId = setEntity.Id;
                            projectEntity.Create();
                            projectEntity.CreateDate = DateTime.Now.AddSeconds(i);
                            res.Insert<ScaffoldprojectEntity>(projectEntity);
                            i++;
                        }
                    }
                }
                HighRiskCommonApplyEntity se = apply.FindEntity(keyValue);
                if (se == null)
                {
                    entity.Create();
                    res.Insert<HighRiskCommonApplyEntity>(entity);
                }
                else
                {
                    entity.Modify(keyValue);
                    res.Update<HighRiskCommonApplyEntity>(entity);
                    UpdateApplyInfo(entity);
                }
                //添加或更新作业安全分析 先删除再添加
                res.Delete<HighRiskRecordEntity>(t => t.WorkId == entity.Id);
                var num = 0;
                if (!list.IsEmpty())
                {
                    foreach (var spec in list)
                    {
                        spec.CreateDate = DateTime.Now.AddSeconds(-num);
                        spec.Create();
                        res.Insert(spec);
                        num++;
                    }
                }
                if (!string.IsNullOrWhiteSpace(isJdz) && entity.WorkType == "11")
                {
                    //添加或更新盲板信息 先删除再添加
                    res.Delete<HighRiskApplyMBXXEntity>(t => t.HighRiskCommonApplyId == entity.Id);
                    num = 0;
                    if (!mbList.IsEmpty())
                    {
                        foreach (var mb in mbList)
                        {
                            mb.Id = Guid.NewGuid().ToString();
                            mb.HighRiskCommonApplyId = entity.Id;
                            mb.CreateDate = DateTime.Now.AddSeconds(-num);
                            mb.Create();
                            res.Insert(mb);
                            num++;
                        }
                    }
                }
                res.Commit();
                #endregion


                #region 提交 更新审核人账号
                string executedept = string.Empty;
                GetExecutedept(entity.WorkDeptType, entity.WorkDeptId, entity.EngineeringId, out executedept);//获取执行部门
                string outsouringengineerdept = string.Empty;
                GetOutsouringengineerDept(entity.WorkDeptId, out outsouringengineerdept);
                string createdetpid = departmentservice.GetEntityByCode(entity.CreateUserDeptCode).IsEmpty() ? "" : departmentservice.GetEntityByCode(entity.CreateUserDeptCode).DepartmentId; //获取创建部门ID
                string accountstr = manypowercheckservice.GetApproveUserAccount(entity.FlowId, entity.Id, entity.NextStepApproveUserAccount, entity.SpecialtyType, executedept, outsouringengineerdept, createdetpid, "", entity.ApproveDeptId); //获取审核人账号
                entity.ApproveAccount = accountstr;
                entity.Modify(keyValue);
                //res.Update<HighRiskCommonApplyEntity>(entity);
                this.BaseRepository().Update(entity);

                messagedata.UserAccount = entity.ApproveAccount;
                #endregion
                messagedata.Success = 1;
            }
            catch (Exception ex)
            {
                messagedata.Success = 0;
                res.Rollback();
                throw ex;
            }
            return messagedata;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateApplyInfo(HighRiskCommonApplyEntity entity)
        {
            string sql =string.Format(@"update BIS_HIGHRISKCOMMONAPPLY set PipeLine='{0}',Media='{1}',Temperature='{2}',Pressure='{3}',ZMBDutyUserName='{4}'
,ZMBDutyUserId='{5}',CMBDutyUserName='{6}',CMBDutyUserId='{7}' where ID='{8}'", entity.PipeLine, entity.Media, entity.Temperature, entity.Pressure, entity.ZMBDutyUserName
, entity.ZMBDutyUserId, entity.CMBDutyUserName, entity.CMBDutyUserId,entity.Id);
            return this.BaseRepository().ExecuteBySql(sql) > 0 ? true : false;
        }

        /// <summary>
        /// 确认，审核
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="state"></param>
        /// <param name="recordData"></param>
        /// <param name="entity"></param>
        /// <param name="aentity"></param>
        public PushMessageData SubmitCheckForm(string keyValue, string state, string recordData, HighRiskCommonApplyEntity entity, ScaffoldauditrecordEntity aentity)
        {
            var res = DbFactory.Base().BeginTrans();
            PushMessageData messagedata = new PushMessageData();
            try
            {
                ManyPowerCheckEntity mpcEntity = null;
                entity.Id = keyValue;
                int noDoneCount = 0; //未完成个数

                string newKeyValue = string.Empty;

                Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
                string moduleName =GetModuleName(entity);
                mpcEntity = manypowercheckservice.CheckAuditForNext(curUser, moduleName, entity.FlowId);

                //更改申请信息状态

                JArray arr = new JArray();

                if (!string.IsNullOrEmpty(recordData))
                {
                    arr = (JArray)JsonConvert.DeserializeObject(recordData);
                }

                aentity.FlowId = entity.FlowId;
                //确认状态下更新安全检查项目
                if (state == "1")
                {
                    Repository<HighImportTypeEntity> imtype = new Repository<HighImportTypeEntity>(DbFactory.Base());
                    string str = "1";//默认为手动输入
                    HighImportTypeEntity projectEntity = imtype.FindList(string.Format("select * from bis_highimporttype where itype='1' and CreateUserOrgCode='{0}'", curUser.OrganizeCode)).ToList().FirstOrDefault();
                    if (projectEntity != null)
                        str = projectEntity.IsImport;

                    //只更新安全措施
                    for (int i = 0; i < arr.Count(); i++)
                    {
                        string id = arr[i]["id"].ToString();  //主键
                        string result = arr[i]["result"].ToString(); //结果
                        string people = arr[i]["people"].ToString(); //选择的人员
                        string peopleid = arr[i]["peopleid"].ToString(); //选择的人员
                        string signpic = string.IsNullOrWhiteSpace(arr[i]["signpic"].ToString()) ? "" : arr[i]["signpic"].ToString().Replace("../..", "");
                        string projectname = arr[i]["projectname"].ToString();//安全措施
                        if (!string.IsNullOrEmpty(id))
                        {
                            Repository<ScaffoldprojectEntity> sp = new Repository<ScaffoldprojectEntity>(DbFactory.Base());
                            var scEntity = sp.FindEntity(id); //安全措施项
                            scEntity.Result = result;
                            if (result != "1") { noDoneCount += 1; } //存在否定的情况的则累加
                            scEntity.CheckPersons = people;
                            scEntity.CheckPersonsId = peopleid;
                            scEntity.SignPic = signpic;//签名
                            if (str == "1")
                            {
                                scEntity.ProjectName = projectname;
                            }
                            //更新当前流程进行中的安全措施内容
                            res.Update<ScaffoldprojectEntity>(scEntity);
                        }
                    }
                    //流程结束
                    if (noDoneCount > 0)
                    {
                        aentity.AuditState = 0;//不同意
                        aentity.AuditRemark = "确认step";
                        entity.InvestigateState = "3"; //更改状态为流程结束
                        entity.ApplyState = "2";//确认未通过
                        entity.FlowDept = " ";
                        entity.FlowDeptName = " ";
                        entity.FlowRole = " ";
                        entity.FlowRoleName = " ";
                        entity.FlowName = "已完结";
                        entity.FlowRemark = "";
                        entity.FlowApplyType = "";
                        entity.ApproveAccount = "";
                        entity.NextStepApproveUserAccount = "";
                        entity.FlowId = "";

                        //推送消息到申请人
                        messagedata.SendCode = "ZY003";
                        messagedata.EntityId = entity.Id;
                    }
                    else
                    {
                        aentity.AuditState = 1;//同意
                        aentity.AuditRemark = "确认step";
                        if (null != mpcEntity)
                        {
                            entity.FlowDept = mpcEntity.CHECKDEPTID;
                            entity.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                            entity.FlowRole = mpcEntity.CHECKROLEID;
                            entity.FlowRoleName = mpcEntity.CHECKROLENAME;
                            entity.FlowId = mpcEntity.ID;
                            entity.FlowApplyType = mpcEntity.NextStepFlowEntity.IsEmpty() ? "" : mpcEntity.NextStepFlowEntity.ApplyType;
                            entity.FlowName = "审核中";
                            entity.InvestigateState = "2"; //更改状态为审核
                            entity.ApplyState = "3";//审核(批)中
                            entity.FlowRemark = !string.IsNullOrEmpty(mpcEntity.REMARK) ? mpcEntity.REMARK : "";
                            string checktype = entity.FlowRemark != "1" ? "0" : "1";
                            entity.NextStepApproveUserAccount = aentity.NextStepApproveUserAccount;
                            string executedept = string.Empty;
                            GetExecutedept(entity.WorkDeptType, entity.WorkDeptId, entity.EngineeringId, out executedept); //获取执行部门
                            string createdetpid = departmentservice.GetEntityByCode(entity.CreateUserDeptCode).IsEmpty() ? "" : departmentservice.GetEntityByCode(entity.CreateUserDeptCode).DepartmentId; //获取创建部门ID
                            string outsouringengineerdept = string.Empty;
                            GetOutsouringengineerDept(entity.WorkDeptId, out outsouringengineerdept);
                            string accountstr = manypowercheckservice.GetApproveUserAccount(entity.FlowId, entity.Id, entity.NextStepApproveUserAccount, entity.SpecialtyType, executedept, outsouringengineerdept, createdetpid, "", entity.ApproveDeptId); //获取审核人账号
                            entity.ApproveAccount = accountstr;


                            //推送消息到有审批权限的人
                            messagedata.UserAccount = entity.ApproveAccount;
                            messagedata.UserDept = entity.FlowDept;
                            messagedata.UserRole = entity.FlowRole;
                            messagedata.SendCode = "ZY002";
                            messagedata.EntityId = entity.Id;
                            messagedata.IsSpecial = checktype;
                            messagedata.SpecialtyType = !string.IsNullOrEmpty(entity.SpecialtyType) ? entity.SpecialtyType : "";
                        }
                        else
                        {
                            entity.FlowRemark = "";
                            entity.ApplyState = "5";
                            entity.FlowDept = "";
                            entity.FlowDeptName = "";
                            entity.FlowRole = "";
                            entity.FlowRoleName = "";
                            entity.FlowName = "已完结";
                            entity.InvestigateState = "3"; //完结状态
                            entity.FlowApplyType = "";
                            entity.ApproveAccount = "";
                            entity.NextStepApproveUserAccount = "";
                            entity.FlowId = "";

                            //推送消息到申请人
                            messagedata.SendCode = "ZY018";
                            messagedata.EntityId = entity.Id;
                        }
                    }
                }
                else
                {
                    //同意进行下一步
                    if (aentity.AuditState == 1)
                    {

                        //下一步流程不为空
                        if (null != mpcEntity)
                        {
                            entity.FlowDept = mpcEntity.CHECKDEPTID;
                            entity.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                            entity.FlowRole = mpcEntity.CHECKROLEID;
                            entity.FlowRoleName = mpcEntity.CHECKROLENAME;
                            entity.FlowId = mpcEntity.ID;
                            entity.FlowApplyType = mpcEntity.NextStepFlowEntity.IsEmpty() ? "" : mpcEntity.NextStepFlowEntity.ApplyType;
                            entity.FlowRemark = !string.IsNullOrEmpty(mpcEntity.REMARK) ? mpcEntity.REMARK : "";
                            string checktype = entity.FlowRemark != "1" ? "0" : "1";
                            entity.NextStepApproveUserAccount = aentity.NextStepApproveUserAccount;
                            string executedept = string.Empty;
                            GetExecutedept(entity.WorkDeptType, entity.WorkDeptId, entity.EngineeringId, out executedept); //获取执行部门
                            string createdetpid = departmentservice.GetEntityByCode(entity.CreateUserDeptCode).IsEmpty() ? "" : departmentservice.GetEntityByCode(entity.CreateUserDeptCode).DepartmentId; //获取创建部门ID
                            string outsouringengineerdept = string.Empty;
                            GetOutsouringengineerDept(entity.WorkDeptId, out outsouringengineerdept);
                            string str = manypowercheckservice.GetApproveUserAccount(entity.FlowId, entity.Id, entity.NextStepApproveUserAccount, entity.SpecialtyType, executedept, outsouringengineerdept, createdetpid, "", entity.ApproveDeptId); //获取审核人账号
                            entity.ApproveAccount = str;

                            //推送消息到有审批权限的人
                            messagedata.UserAccount = entity.ApproveAccount;
                            messagedata.UserDept = entity.FlowDept;
                            messagedata.UserRole = entity.FlowRole;
                            messagedata.SendCode = "ZY002";
                            messagedata.EntityId = entity.Id;
                            messagedata.IsSpecial = checktype;
                            messagedata.SpecialtyType = !string.IsNullOrEmpty(entity.SpecialtyType) ? entity.SpecialtyType : "";
                        }
                        else
                        {
                            entity.FlowRemark = "";
                            entity.FlowDept = " ";
                            entity.FlowDeptName = " ";
                            entity.FlowRole = " ";
                            entity.FlowRoleName = " ";
                            entity.FlowName = "已完结";
                            entity.InvestigateState = "3"; //更改状态为完结状态
                            entity.ApplyState = "5";//审核(批)通过

                            //推送消息到申请人
                            messagedata.SendCode = "ZY018";
                            messagedata.EntityId = entity.Id;
                        }
                    }
                    else
                    {
                        entity.FlowRemark = "";
                        entity.FlowDept = " ";
                        entity.FlowDeptName = " ";
                        entity.FlowRole = " ";
                        entity.FlowRoleName = " ";
                        entity.InvestigateState = "3"; //更改状态为登记状态
                        entity.ApplyState = "4";//审核(批)未通过
                        entity.FlowName = "已完结";

                        //推送消息到申请人
                        messagedata.SendCode = "ZY003";
                        messagedata.EntityId = entity.Id;
                    }
                    aentity.AuditSignImg = string.IsNullOrWhiteSpace(aentity.AuditSignImg) ? "" : aentity.AuditSignImg.ToString().Replace("../..", "");
                }
                //添加审核记录
                aentity.ScaffoldId = keyValue; //关联id 
                aentity.Create();
                aentity.AuditDate = DateTime.Now;
                res.Insert<ScaffoldauditrecordEntity>(aentity);
                //更改申请单
                res.Update<HighRiskCommonApplyEntity>(entity);
                res.Commit();
                messagedata.Success = 1;

            }
            catch (Exception)
            {
                res.Rollback();
                messagedata.Success = 0;
            }
            return messagedata;
        }

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveApplyForm(string keyValue, HighRiskCommonApplyEntity entity)
        {
            entity.Id = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                var sl = BaseRepository().FindEntity(keyValue);
                if (sl == null)
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

        #region 统计
        /// <summary>
        /// 按作业类型统计
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public string GetHighWorkCount(string starttime, string endtime, string deptid, string deptcode)
        {
            DataTable dt = GetWorkList(starttime, endtime, deptid, deptcode);
            return Newtonsoft.Json.JsonConvert.SerializeObject(dt);
        }

        private DataTable GetWorkList(string starttime, string endtime, string deptid, string deptcode)
        {
            var user = OperatorProvider.Provider.Current();
            //权限
            string strWhere = "", strScaffoldWhere = "", strBuildWhere = "", strRemoveWhere = "", strChangeWhere = "";
            if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级"))
            {
                strWhere += string.Format(" and createuserorgcode ='{0}' ", user.OrganizeCode);
                strScaffoldWhere += string.Format(" and createuserorgcode='{0}'", user.OrganizeCode);
                strChangeWhere += string.Format(" and createuserorgcode='{0}'", user.OrganizeCode);
            }
            else
            {
                strWhere += string.Format(" and ((WorkDeptCode in(select encode from base_department where encode like '{0}%'))  or (engineeringid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);

                strScaffoldWhere += string.Format(" and ((setupcompanyid in(select departmentid from base_department where encode like '{0}%'))  or (outprojectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);


                strChangeWhere += string.Format(" and ((workunitcode in(select encode from base_department where encode like '{0}%'))  or (projectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);
            }
            if (!string.IsNullOrEmpty(deptcode) && !string.IsNullOrEmpty(deptid) && deptcode != user.OrganizeCode)
            {
                strWhere += string.Format(" and ((WorkDeptCode in(select encode from base_department where encode like '{0}%'))  or (engineeringid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", deptcode, deptid);

                strScaffoldWhere += string.Format(" and ((setupcompanyid in(select departmentid from base_department where encode like '{0}%'))  or (outprojectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", deptcode, deptid);

                strChangeWhere += string.Format(" and ((workunitcode in(select encode from base_department where encode like '{0}%'))  or (projectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", deptcode, deptid);
            }
            if (!string.IsNullOrEmpty(starttime))
            {
                strWhere += string.Format(" and WorkStartTime>=to_date('{0}','yyyy-mm-dd')", starttime);
                strBuildWhere += string.Format(" and setupstartdate>=to_date('{0}','yyyy-mm-dd')", starttime);
                strRemoveWhere += string.Format(" and dismentlestartdate>=to_date('{0}','yyyy-mm-dd')", starttime);
                //申请变动时间
                strChangeWhere += string.Format(" and applychangetime>=to_date('{0}','yyyy-mm-dd')", starttime);
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                var strendtime = Convert.ToDateTime(endtime).AddDays(1).ToString("yyyy-MM-dd");
                strWhere += string.Format(" and WorkStartTime<=to_date('{0}','yyyy-mm-dd')", strendtime);
                strBuildWhere += string.Format(" and setupstartdate<=to_date('{0}','yyyy-mm-dd')", strendtime);
                strRemoveWhere += string.Format(" and dismentlestartdate<=to_date('{0}','yyyy-mm-dd')", strendtime);
                //预计恢复时间
                strChangeWhere += string.Format(" and applychangetime<=to_date('{0}','yyyy-mm-dd')", strendtime);
            }
            string sql = string.Format("select b.itemname as name,nvl(c,0) y from (select count(1) c,worktype from bis_highriskcommonapply where applystate='5' {0} group by worktype) a right join(select itemname,itemvalue from  base_dataitemdetail  where itemid =(select itemid from base_dataitem where itemcode='CommonType')  and enabledmark=1) b  on   b.itemvalue=a.worktype order by b.itemvalue", strWhere);
            DataTable dt = this.BaseRepository().FindTable(sql);


            //脚手架搭设
            string sqlscaffold = string.Format("select '脚手架搭设' as name,count(1) as y from  bis_scaffold where id not in (select id from bis_scaffold where id in(select setupinfoid from bis_scaffold where scaffoldtype = 1 and auditstate = 3)) and ScaffoldType ='0'  and AuditState in ('3') {0} {1}", strScaffoldWhere, strBuildWhere);
            DataTable dtBuild = this.BaseRepository().FindTable(sqlscaffold);
            dt.Merge(dtBuild);

            //脚手架拆除
            string sqlRemove = string.Format("select '脚手架拆除' as name,count(1) as y from  bis_scaffold where scaffoldtype='2' and auditstate='3' {0} {1}", strScaffoldWhere, strRemoveWhere);
            DataTable dtRemove = this.BaseRepository().FindTable(sqlRemove);
            dt.Merge(dtRemove);


            //安全设施变动
            string sqlChange = string.Format("select '安全设施变动' as name,count(1) as y from bis_Safetychange where iscommit=1 and isapplyover=1 and isaccpcommit in (0,1) and isaccepover=0 {0}", strChangeWhere);
            DataTable dtChange = this.BaseRepository().FindTable(sqlChange);
            dt.Merge(dtChange);

            dt.Dispose();
            return dt;
        }


        /// <summary>
        ///作业类型统计(统计表格)
        /// </summary>
        /// <returns></returns>
        public string GetHighWorkList(string starttime, string endtime, string deptid, string deptcode)
        {
            DataTable dt = GetWorkList(starttime, endtime, deptid, deptcode);
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
        /// 月度趋势(统计图)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public string GetHighWorkYearCount(string year, string deptid, string deptcode)
        {
            var user = OperatorProvider.Provider.Current();
            //权限
            string strWhere = "", strScaffoldWhere = "", strBuildWhere = "", strRemoveWhere = "", strChangeWhere = "";
            if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级"))
            {
                strWhere += string.Format(" and createuserorgcode ='{0}' ", user.OrganizeCode);
                strScaffoldWhere += string.Format(" and createuserorgcode='{0}'", user.OrganizeCode);
                strChangeWhere += string.Format(" and createuserorgcode='{0}'", user.OrganizeCode);
            }
            else
            {
                strWhere += string.Format(" and ((workdeptcode in(select encode from base_department where encode like '{0}%'))  or (engineeringid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);

                strScaffoldWhere += string.Format(" and ((setupcompanyid in(select departmentid from base_department where encode like '{0}%'))  or (outprojectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);

                strChangeWhere += string.Format(" and ((workunitcode in(select encode from base_department where encode like '{0}%'))  or (projectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);
            }
            if (!string.IsNullOrEmpty(deptcode) && !string.IsNullOrEmpty(deptid) && deptcode != user.OrganizeCode)
            {
                strWhere += string.Format(" and ((workdeptcode in(select encode from base_department where encode like '{0}%'))  or (engineeringid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", deptcode, deptid);

                strScaffoldWhere += string.Format(" and ((setupcompanyid in(select departmentid from base_department where encode like '{0}%'))  or (outprojectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", deptcode, deptid);

                strChangeWhere += string.Format(" and ((workunitcode in(select encode from base_department where encode like '{0}%'))  or (projectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", deptcode, deptid);
            }
            if (!string.IsNullOrEmpty(year))
            {
                strWhere += string.Format(" and to_char(WorkStartTime,'yyyy')='{0}'", year);
                strBuildWhere += string.Format(" and to_char(setupstartdate,'yyyy')='{0}'", year);
                strRemoveWhere += string.Format(" and to_char(dismentlestartdate,'yyyy')='{0}'", year);
                //申请变动时间
                strChangeWhere += string.Format("and to_char(applychangetime,'yyyy')='{0}'", year);
            }
            List<string> listmonths = new List<string>();
            for (int i = 1; i <= 12; i++)
            {
                listmonths.Add(i.ToString() + "月");
            }
            List<int> list = new List<int>();
            for (int i = 1; i <= 12; i++)
            {
                string whereSQL2 = " and to_char(WorkStartTime,'mm')=" + i.ToString();
                int commonnum = this.BaseRepository().FindObject(string.Format(@"select count(1) c from bis_highriskcommonapply where applystate='5' {0} {1}", strWhere, whereSQL2)).ToInt();

                //脚手架搭设(搭设申请信息审核通过，验收申请审核未通过且未拆除(拆除申请还未审核通过的)的数据)
                string buildWhereSql2 = " and to_char(setupstartdate,'mm')=" + i.ToString();
                int buildnum = this.BaseRepository().FindObject(string.Format(@"select  count(1) c from  bis_scaffold where id not in (select id from bis_scaffold where id in(select setupinfoid from bis_scaffold where scaffoldtype = 1 and auditstate = 3)) and ScaffoldType ='0'  and AuditState in ('3') {0} {1} {2}", strScaffoldWhere, strBuildWhere, buildWhereSql2)).ToInt();

                //脚手架拆除(拆除审核通过的数据)
                string removeWhereSql2 = " and to_char(dismentlestartdate,'mm')=" + i.ToString();
                int removenum = this.BaseRepository().FindObject(string.Format("select count(1) c  from  bis_scaffold  where scaffoldtype='2' and auditstate='3' {0} {1} {2}", strScaffoldWhere, strRemoveWhere, removeWhereSql2)).ToInt();

                //安全设施变动
                string changeWhereSql2 = " and to_char(applychangetime,'mm')=" + i.ToString();
                int changenum = this.BaseRepository().FindObject(string.Format("select count(1) c from bis_Safetychange where iscommit=1 and isapplyover=1 and isaccpcommit in (0,1) and isaccepover=0 {0} {1}", strChangeWhere, changeWhereSql2)).ToInt();

                int sumnum = commonnum + buildnum + removenum + changenum;
                list.Add(sumnum);
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(new { y = list, x = listmonths });

        }

        /// <summary>
        /// 月度趋势(表格)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public string GetHighWorkYearList(string year, string deptid, string deptcode)
        {
            var user = OperatorProvider.Provider.Current();
            string strWhere = "", strScaffoldWhere = "", strBuildWhere = "", strRemoveWhere = "", strChangeWhere = "";
            if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级"))
            {
                strWhere += string.Format(" and createuserorgcode ='{0}' ", user.OrganizeCode);
                strScaffoldWhere += string.Format(" and createuserorgcode='{0}'", user.OrganizeCode);
                strChangeWhere += string.Format(" and createuserorgcode='{0}'", user.OrganizeCode);
            }
            else
            {
                strWhere += string.Format(" and ((workdeptcode in(select encode from base_department where encode like '{0}%'))  or (engineeringid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);

                strScaffoldWhere += string.Format(" and ((setupcompanyid in(select departmentid from base_department where encode like '{0}%'))  or (outprojectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);

                strChangeWhere += string.Format(" and ((workunitcode in(select encode from base_department where encode like '{0}%'))  or (projectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);
            }
            if (!string.IsNullOrEmpty(deptcode) && !string.IsNullOrEmpty(deptid) && deptcode != user.OrganizeCode)
            {
                strWhere += string.Format(" and ((workdeptcode in(select encode from base_department where encode like '{0}%'))  or (engineeringid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", deptcode, deptid);

                strScaffoldWhere += string.Format(" and ((setupcompanyid in(select departmentid from base_department where encode like '{0}%'))  or (outprojectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", deptcode, deptid);

                strChangeWhere += string.Format(" and ((workunitcode in(select encode from base_department where encode like '{0}%'))  or (projectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", deptcode, deptid);
            }
            if (!string.IsNullOrEmpty(year))
            {
                strWhere += string.Format(" and to_char(WorkStartTime,'yyyy')='{0}'", year);
                strBuildWhere += string.Format(" and to_char(setupstartdate,'yyyy')='{0}'", year);
                strRemoveWhere += string.Format(" and to_char(dismentlestartdate,'yyyy')='{0}'", year);
                //申请变动时间
                strChangeWhere += string.Format("and to_char(applychangetime,'yyyy')='{0}'", year);
            }
            DataTable dtresult = new DataTable();
            dtresult.Columns.Add("name");
            for (int i = 1; i <= 12; i++)
            {
                dtresult.Columns.Add("num" + i, typeof(int));
            }
            DataRow row = dtresult.NewRow();
            row["name"] = "高风险作业数量";
            for (int i = 1; i <= 12; i++)
            {
                string whereSQL2 = " and to_char(WorkStartTime,'mm')=" + i.ToString();
                int commonnum = this.BaseRepository().FindObject(string.Format(@"select count(1) c from bis_highriskcommonapply where applystate='5' {0} {1}", strWhere, whereSQL2)).ToInt();

                //脚手架搭设
                string buildWhereSql2 = " and to_char(setupstartdate,'mm')=" + i.ToString();
                int buildnum = this.BaseRepository().FindObject(string.Format(@"select  count(1) c from  bis_scaffold where  id not in (select id from bis_scaffold where id in(select setupinfoid from bis_scaffold where scaffoldtype = 1 and auditstate = 3)) and ScaffoldType ='0'  and AuditState in ('3') {0} {1} {2}", strScaffoldWhere, strBuildWhere, buildWhereSql2)).ToInt();

                //脚手架拆除
                string removeWhereSql2 = " and to_char(dismentlestartdate,'mm')=" + i.ToString();
                int removenum = this.BaseRepository().FindObject(string.Format("select count(1) c  from  bis_scaffold  where scaffoldtype='2' and auditstate='3' {0} {1} {2}", strScaffoldWhere, strRemoveWhere, removeWhereSql2)).ToInt();

                //安全设施变动
                string changeWhereSql2 = " and to_char(applychangetime,'mm')=" + i.ToString();
                int changenum = this.BaseRepository().FindObject(string.Format("select count(1) c from bis_Safetychange where iscommit=1 and isapplyover=1 and isaccpcommit in (0,1) and isaccepover=0 {0} {1}", strChangeWhere, changeWhereSql2)).ToInt();

                int sumnum = commonnum + buildnum + removenum + changenum;
                row["num" + i] = sumnum;
            }
            dtresult.Rows.Add(row);
            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = 1,
                page = 1,
                records = dtresult.Rows.Count,
                rows = dtresult
            });
        }

        /// <summary>
        /// 单位对比(统计图)
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public string GetHighWorkDepartCount(string starttime, string endtime)
        {
            List<string> listdepts;
            List<int> list;
            GetDeptContrast(starttime, endtime, out listdepts, out list);
            return Newtonsoft.Json.JsonConvert.SerializeObject(new { x = listdepts, y = list });
        }

        private DataTable GetDeptContrast(string starttime, string endtime, out List<string> listdepts, out List<int> list)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("deptname");
            dt.Columns.Add("deptcount");

            var user = OperatorProvider.Provider.Current();
            //权限
            string strWhere = "", strScaffoldWhere = "", strBuildWhere = "", strRemoveWhere = "", strChangeWhere = "";
            if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级"))
            {
                strWhere += string.Format(" and createuserorgcode ='{0}' ", user.OrganizeCode);
                strScaffoldWhere += string.Format(" and createuserorgcode='{0}'", user.OrganizeCode);
                strChangeWhere += string.Format(" and createuserorgcode='{0}'", user.OrganizeCode);
            }
            if (!string.IsNullOrEmpty(starttime))
            {
                strWhere += string.Format(" and WorkStartTime>=to_date('{0}','yyyy-mm-dd')", starttime);
                strBuildWhere += string.Format(" and setupstartdate>=to_date('{0}','yyyy-mm-dd')", starttime);
                strRemoveWhere += string.Format(" and dismentlestartdate>=to_date('{0}','yyyy-mm-dd')", starttime);
                //申请变动时间
                strChangeWhere += string.Format(" and applychangetime>=to_date('{0}','yyyy-mm-dd')", starttime);
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                var strendtime = Convert.ToDateTime(endtime).AddDays(1).ToString("yyyy-MM-dd");
                strWhere += string.Format(" and WorkStartTime<=to_date('{0}','yyyy-mm-dd')", strendtime);
                strBuildWhere += string.Format(" and setupstartdate<=to_date('{0}','yyyy-mm-dd')", strendtime);
                strRemoveWhere += string.Format(" and dismentlestartdate<=to_date('{0}','yyyy-mm-dd')", strendtime);
                //预计恢复时间
                strChangeWhere += string.Format(" and applychangetime<=to_date('{0}','yyyy-mm-dd')", strendtime);
            }
            var dtdepts = this.BaseRepository().FindTable(string.Format("select encode ,fullname,sortcode from base_department  where nature='部门'  and  encode like '{0}%' order by sortcode", user.OrganizeCode));

            listdepts = new List<string>();
            list = new List<int>();

            foreach (DataRow item in dtdepts.Rows)
            {
                listdepts.Add(item["fullname"].ToString());
                var deptcode = item["encode"].ToString();
                string whereSQL2 = string.Format(" and WorkDeptCode like '{0}%'", deptcode);
                int commonnum = this.BaseRepository().FindObject(string.Format(@"select count(1) c from bis_highriskcommonapply where applystate='5' {0} {1}", strWhere, whereSQL2)).ToInt();

                //脚手架搭设(搭设申请信息审核通过，验收申请审核未通过且未拆除(拆除申请还未审核通过的)的数据)
                string buildWhereSql2 = string.Format(" and setupcompanyid in(select departmentid from base_department  where encode like '{0}%')", deptcode);
                int buildnum = this.BaseRepository().FindObject(string.Format(@"select  count(1) c from  bis_scaffold where id not in (select id from bis_scaffold where id in(select setupinfoid from bis_scaffold where scaffoldtype = 1 and auditstate = 3)) and ScaffoldType ='0'  and AuditState in ('3') {0} {1} {2}", strScaffoldWhere, strBuildWhere, buildWhereSql2)).ToInt();

                //脚手架拆除
                int removenum = this.BaseRepository().FindObject(string.Format("select count(1) c  from  bis_scaffold  where scaffoldtype='2' and auditstate='3' {0} {1} {2}", strScaffoldWhere, strRemoveWhere, buildWhereSql2)).ToInt();

                //安全设施变动
                string changeWhereSql2 = string.Format(" and workunitcode like '{0}%'", deptcode);
                int changenum = this.BaseRepository().FindObject(string.Format("select count(1) c from bis_Safetychange where iscommit=1 and isapplyover=1 and isaccpcommit in (0,1) and isaccepover=0 {0} {1}", strChangeWhere, changeWhereSql2)).ToInt();

                int sumnum = commonnum + buildnum + removenum + changenum;
                list.Add(sumnum);

                var row = dt.NewRow();
                row["deptname"] = item["fullname"].ToString();
                row["deptcount"] = sumnum;
                dt.Rows.Add(row);
            }
            return dt;
        }


        /// <summary>
        /// 单位对比(表格)
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public string GetHighWorkDepartList(string starttime, string endtime)
        {
            List<string> listdepart;
            List<int> list;
            var dtresult = GetDeptContrast(starttime, endtime, out listdepart, out list);

            return Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                total = 1,
                page = 1,
                records = dtresult.Rows.Count,
                rows = dtresult
            });
        }
        #endregion


        #region 获取今日高风险作业
        /// <summary>
        /// 获取今日高风险作业(作业台账中作业中的数据)
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetTodayWorkList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DatabaseType.Oracle;
            #region 数据权限
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string curUserId = user.UserId;
            #endregion
            /*
             作业中:审批通过且实际作业开始时间不为空且实际作业结束时间为空
             */
            #region 查表
            string isJdz = new DataItemDetailService().GetItemValue("景德镇版本");
            string str = string.Empty;
            if (!string.IsNullOrEmpty(isJdz))
            {
                str = "(CASE WHEN WORKTYPE='12' THEN (select itemid from base_dataitem where itemcode='CommonWorkType') ELSE (select itemid from base_dataitem where itemcode='CommonRiskType') END)";
            }
            else
            {
                str = "(select itemid from base_dataitem where itemcode='CommonRiskType')";
            }
            pagination.p_kid = "t.id";
            pagination.p_fields = "t.worktype,t.risktype,t.workdeptname,t.workareaname,t.workdeptcode,t.engineeringid,t.workplace,t.changetype,t.changename,t.realityworkstarttime,t.realityworkendtime, t1.itemname as worktypename, b.itemname as risktypename";
            pagination.p_tablename =string.Format(" v_underwaywork t left join base_dataitemdetail t1 on t.worktype = t1.itemvalue and t1.itemid = (select itemid from base_dataitem where itemcode = 'StatisticsType') left join base_dataitemdetail b on t.risktype = b.itemvalue and b.itemid ={0}", str);
            pagination.conditionJson = "1=1";
            if (!user.IsSystem)
            {
                if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级"))
                {
                    pagination.conditionJson += " and t.createuserorgcode='" + user.OrganizeCode + "'";
                }
                else
                {
                    pagination.conditionJson += string.Format(" and ((workdeptcode in(select encode from base_department where encode like '{0}%'))  or (engineeringid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);
                }
            }
            #endregion
            #region  筛选条件
            var queryParam = JObject.Parse(queryJson);
            //责任部门
            if (!queryParam["dutydeptid"].IsEmpty())
            {
                pagination.conditionJson += string.Format("  and  engineeringid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{0}')", queryParam["dutydeptid"].ToString());
            }
            #endregion
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        #endregion

        #region 手机端高风险作业统计
        public DataTable AppGetHighWork(string starttime, string endtime, string deptid, string deptcode)
        {
            DataTable dt = GetWorkList(starttime, endtime, deptid, deptcode);
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
            return dt;
        }
        #endregion

        #region 获取被引用的外包工程
        public bool GetProjectNum(string outProjectId)
        {
            bool flag = true;
            string sql = string.Format("select count(1) from v_showoutproject where outprojectid='{0}'", outProjectId);
            int count = Convert.ToInt32(this.BaseRepository().FindTable(sql).Rows[0][0].ToString());
            if (count > 0)
            {
                flag = false;
            }
            return flag;
        }
        #endregion


        #region 获取执行部门
        /// <summary>
        /// 获取执行部门
        /// </summary>
        /// <param name="workdepttype">作业单位类型</param>
        /// <param name="workdept">作业单位</param>
        /// <param name="projectid">外包工程ID</param>
        /// <param name="Executedept">执行部门</param>
        public void GetExecutedept(string workdepttype, string workdept, string projectid, out string Executedept)
        {
            try
            {
                Executedept = "";
                if (workdepttype == "1")
                {
                    Repository<OutsouringengineerEntity> ourEngineer = new Repository<OutsouringengineerEntity>(DbFactory.Base());
                    Executedept = departmentIService.GetEntity(ourEngineer.FindEntity(projectid).ENGINEERLETDEPTID).DepartmentId;
                }
                else if (workdepttype == "0")
                {
                    var deptentity = new DepartmentService().GetEntity(workdept);
                    while (deptentity.Nature == "专业" || deptentity.Nature == "班组")
                    {
                        Executedept += deptentity.DepartmentId + ",";
                        deptentity = new DepartmentService().GetEntity(deptentity.ParentId);
                    }
                    Executedept += deptentity.DepartmentId + ",";
                    Executedept = Executedept.Substring(0, Executedept.Length - 1);
                }
                else
                {
                    Executedept = "";
                }
            }
            catch (Exception)
            {
                Executedept = "";
            }


        }
        #endregion

        #region 获取外包单位
        /// <summary>
        /// 获取外包单位
        /// </summary>
        /// <param name="workdept">作业单位</param>
        /// <param name="outsouringengineerdept"></param>
        public void GetOutsouringengineerDept(string workdept, out string outsouringengineerdept)
        {
            try
            {
                Operator currUser = OperatorProvider.Provider.Current();
                var cbsentity = departmentIService.GetList().Where(t => t.Description == "外包工程承包商" && t.OrganizeId == currUser.OrganizeId).FirstOrDefault();
                var wbentity = departmentIService.GetEntity(workdept);
                while (wbentity.ParentId != cbsentity.DepartmentId)
                {
                    wbentity = departmentIService.GetEntity(wbentity.ParentId);
                }
                outsouringengineerdept = wbentity.DepartmentId;
            }
            catch (Exception)
            {
                outsouringengineerdept = "";
            }
        }

        #endregion

        /// <summary>
        /// 根据区域编码获取高风险作业的数量
        /// </summary>
        /// <param name="areaCodes"></param>
        /// <returns></returns>
        public DataTable GetCountByArea(List<string> areaCodes)
        {
            if (areaCodes != null && areaCodes.Count > 0)
            {
                string paras = "'" + string.Join("','", areaCodes) + "'";
                string sql = "select WORKAREACODE,WORKAREANAME,count(*) as COUNT from V_XSSUNDERWAYWORK where WORKAREACODE IN (" + paras + ") GROUP BY WORKAREACODE,WORKAREANAME";
                DataTable dt = BaseRepository().FindTable(sql);
                return dt;
            }
            return null;
        }
    }
}
