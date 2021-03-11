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
using ERCHTMS.Entity.HiddenTroubleManage;

namespace ERCHTMS.Service.HighRiskWork
{
    /// <summary>
    /// 描 述：使用消防水
    /// </summary>
    public class FireWaterService : RepositoryFactory<FireWaterEntity>, FireWaterIService
    {
        ScaffoldauditrecordService scaffoldauditrecordservice = new ScaffoldauditrecordService();
        private DataItemDetailService dataitemdetailservice = new DataItemDetailService();
        private HighRiskCommonApplyService highriskcommonapplyservice = new HighRiskCommonApplyService();
        private DepartmentService departmentservice = new DepartmentService();
        private UserService userservice = new UserService();
        private ManyPowerCheckService manypowercheckservice = new ManyPowerCheckService();
        #region 获取数据
        /// <summary>
        /// 得到当前最大编号
        /// </summary>
        /// <returns></returns>
        public string GetMaxCode()
        {
            string orgCode = OperatorProvider.Provider.Current().OrganizeCode;
            string sql = string.Format("select max(ApplyNumber) from bis_firewater where CreateUserOrgCode = @orgCode and ApplyNumber like '%{0}%'", DateTime.Now.ToString("yyyyMMdd"));
            object o = this.BaseRepository().FindObject(sql, new DbParameter[]{
                DbParameters.CreateDbParameter("@orgCode",orgCode)
            });
            if (o == null || o.ToString() == "")
                return "XF" + DateTime.Now.ToString("yyyyMMdd") + "001";
            int num = Convert.ToInt32(o.ToString().Substring(10));
            num++;
            return "XF" + DateTime.Now.ToString("yyyyMMdd") + num.ToString().PadLeft(3, '0');
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public DataTable GetList(Pagination pagination, string queryJson, string authType, Operator user)
        {
            DatabaseType dataTye = DatabaseType.Oracle;
            #region 查表
            pagination.p_kid = "a.id";
            pagination.p_fields = @"a.flowdeptname,a.flowname,a.flowrolename,a.flowdept,a.investigatestate,a.workdeptid,a.engineeringid,a.flowid,b.outtransferuseraccount,b.intransferuseraccount,
                                    a.workdepttype,a.workdeptname,a.applynumber,a.createdate,a.applystate,a.workplace,a.workcontent,a.workuserids,a.workareaname,a.workareacode,
                                    a.workstarttime,a.workendtime,a.applyusername,a.applydeptname,a.createuserid,a.createuserdeptcode,nvl(a.conditionstate,0) conditionstate,
                                    a.createuserorgcode,a.flowremark,a.specialtytype,'' as approveuserid,'' as approveusername,'' as approveuseraccount,case when a.applystate='3' and (a.conditionstate='0' or a.conditionstate is null) then '1' else '0' end appconditionstate";
                                                pagination.p_tablename = " bis_firewater a left join (select recid,flowid,outtransferuseraccount,intransferuseraccount,row_number()  over(partition by recid,flowid order by createdate desc) as num from BIS_TRANSFERRECORD where disable=0) b on a.id=b.recid and a.flowid=b.flowid and b.num=1";
            pagination.conditionJson = " 1=1 ";
            if (!string.IsNullOrEmpty(authType))
            {
                //配置的部门拥有特殊查看权限
                string specialDeptId = new DataItemDetailService().GetItemValue(user.OrganizeId, "FireDept");
                var IsHrdl = new DataItemDetailService().GetItemValue("IsOpenPassword");
                string isAllDataRange = dataitemdetailservice.GetEnableItemValue("HighRiskWorkDataRange"); //特殊标记，高风险作业模块是否看全厂数据
                if (!string.IsNullOrEmpty(specialDeptId) && specialDeptId.Contains(user.DeptId) || IsHrdl== "true" || !string.IsNullOrWhiteSpace(isAllDataRange))
                {
                    pagination.conditionJson += " and a.createuserorgcode='" + user.OrganizeCode + "'";
                }
                else
                {
                    switch (authType)
                    {
                        case "1":
                            pagination.conditionJson += " and a.applyuserid='" + user.UserId + "'";
                            break;
                        case "2":
                            pagination.conditionJson += " and a.workdeptid='" + user.DeptId + "'";
                            break;
                        case "3"://本子部门
                            pagination.conditionJson += string.Format(" and ((a.workdeptid in(select departmentid from base_department where encode like '{0}%'))  or (a.engineeringid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);
                            break;
                        case "4":
                            pagination.conditionJson += " and a.createuserorgcode='" + user.OrganizeCode + "'";
                            break;
                        case "app":
                            if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级"))
                            {
                                pagination.conditionJson += " and a.createuserorgcode='" + user.OrganizeCode + "'";
                            }
                            else
                            {
                                pagination.conditionJson += string.Format(" and ((a.workdeptid in(select departmentid from base_department where encode like '{0}%'))  or (a.engineeringid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);
                            }
                            break;
                    }
                }
            }
            else
            {
                pagination.conditionJson += " and 0=1";
            }
            #endregion

            #region  筛选条件
            var queryParam = JObject.Parse(queryJson);
            //查询条件
            if (!queryParam["applynumber"].IsEmpty())//申请编号
            {
                pagination.conditionJson += string.Format(" and applynumber like '%{0}%'", queryParam["applynumber"].ToString());
            }
            //查询条件
            if (!queryParam["status"].IsEmpty())//许可状态
            {
                pagination.conditionJson += string.Format(" and applystate='{0}'", queryParam["status"].ToString());
            }
            //时间选择
            if (!queryParam["st"].IsEmpty())//开始时间
            {
                string from = queryParam["st"].ToString().Trim();
                pagination.conditionJson += string.Format(" and workstarttime>=to_date('{0}','yyyy-mm-dd')", from);

            }
            if (!queryParam["et"].IsEmpty())//结束时间
            {
                string to = Convert.ToDateTime(queryParam["et"].ToString().Trim()).AddDays(1).ToString("yyyy-MM-dd");
                pagination.conditionJson += string.Format(" and workendtime<=to_date('{0}','yyyy-mm-dd')", to);
            }
            if (!queryParam["workdept"].IsEmpty() && !queryParam["workdeptid"].IsEmpty())//使用消防水单位
            {
                pagination.conditionJson += string.Format(" and ((workdeptcode in(select encode from base_department where encode like '{0}%'))  or (engineeringid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", queryParam["workdept"].ToString(), queryParam["workdeptid"].ToString());
            }
            //查询条件
            if (!queryParam["workdeptcode"].IsEmpty())//使用消防水单位code
            {
                pagination.conditionJson += string.Format(" and workdeptcode='{0}'", queryParam["workdeptcode"].ToString());
            }
            if (!queryParam["viewrange"].IsEmpty())
            {
                //本人
                if (queryParam["viewrange"].ToString().ToLower() == "self")
                {
                    pagination.conditionJson += string.Format(" and a.applyuserid='{0}'", user.UserId);
                }
                else if (queryParam["viewrange"].ToString().ToLower() == "selfaudit")
                {
                    string strCondition = " and a.applystate ='1'";
                    DataTable dt = BaseRepository().FindTable("select " + pagination.p_kid + "," + pagination.p_fields + " from " + pagination.p_tablename + " where " + pagination.conditionJson + strCondition);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string executedept = string.Empty;
                        highriskcommonapplyservice.GetExecutedept(dt.Rows[i]["workdepttype"].ToString(), dt.Rows[i]["workdeptid"].ToString(), dt.Rows[i]["engineeringid"].ToString(), out executedept);
                        string createdetpid = departmentservice.GetEntityByCode(dt.Rows[i]["createuserdeptcode"].ToString()).IsEmpty() ? "" : departmentservice.GetEntityByCode(dt.Rows[i]["createuserdeptcode"].ToString()).DepartmentId;
                        string outsouringengineerdept = string.Empty;
                        highriskcommonapplyservice.GetOutsouringengineerDept(dt.Rows[i]["workdeptid"].ToString(), out outsouringengineerdept);
                        string str = manypowercheckservice.GetApproveUserAccount(dt.Rows[i]["flowid"].ToString(), dt.Rows[i]["id"].ToString(), "", dt.Rows[i]["specialtytype"].ToString(), executedept, outsouringengineerdept, createdetpid, "", "");
                        dt.Rows[i]["approveuseraccount"] = str;
                    }
                    string[] applyids = dt.Select(" (outtransferuseraccount is null or outtransferuseraccount not like '%" + user.Account + ",%') and (approveuseraccount like '%" + user.Account + ",%' or intransferuseraccount like '%" + user.Account + ",%')").AsEnumerable().Select(d => d.Field<string>("id")).ToArray();

                    pagination.conditionJson += string.Format(" and a.id in ('{0}') {1}", string.Join("','", applyids), strCondition);
                }
                else
                {
                    //排除其他人申请保存的数据
                    pagination.conditionJson += string.Format("  and a.id  not in(select id from bis_firewater where applystate='0' and  applyuserid!='{0}')", user.UserId);
                }
            }
            #endregion
            DataTable data = this.BaseRepository().FindTableByProcPager(pagination, dataTye);
            for (int i = 0; i < data.Rows.Count; i++)
            {
                string executedept = string.Empty;
                highriskcommonapplyservice.GetExecutedept(data.Rows[i]["workdepttype"].ToString(), data.Rows[i]["workdeptid"].ToString(), data.Rows[i]["engineeringid"].ToString(), out executedept); //获取执行部门
                string createdetpid = departmentservice.GetEntityByCode(data.Rows[i]["createuserdeptcode"].ToString()).IsEmpty() ? "" : departmentservice.GetEntityByCode(data.Rows[i]["createuserdeptcode"].ToString()).DepartmentId; //获取创建部门
                string outsouringengineerdept = string.Empty;
                highriskcommonapplyservice.GetOutsouringengineerDept(data.Rows[i]["workdeptid"].ToString(), out outsouringengineerdept);
                string str = manypowercheckservice.GetApproveUserAccount(data.Rows[i]["flowid"].ToString(), data.Rows[i]["id"].ToString(), "", data.Rows[i]["specialtytype"].ToString(), executedept, outsouringengineerdept, createdetpid, "", "");
                string outtransferuseraccount = data.Rows[i]["outtransferuseraccount"].IsEmpty() ? "" : data.Rows[i]["outtransferuseraccount"].ToString();//转交申请人
                string intransferuseraccount = data.Rows[i]["intransferuseraccount"].IsEmpty() ? "" : data.Rows[i]["intransferuseraccount"].ToString();//转交接收人
                string[] outtransferuseraccountlist = outtransferuseraccount.Split(',');
                string[] intransferuseraccountlist = intransferuseraccount.Split(',');
                foreach (var item in intransferuseraccountlist)
                {
                    if (!item.IsEmpty() && !str.Contains(item + ","))
                    {
                        str += (item + ",");//将转交接收人加入审核账号中
                    }
                }
                foreach (var item in outtransferuseraccountlist)
                {
                    if (!item.IsEmpty() && str.Contains(item + ","))
                    {
                        str = str.Replace(item + ",", "");//将转交申请人从审核账号中移除
                    }
                }
                data.Rows[i]["approveuseraccount"] = str; 
                DataTable dtuser = userservice.GetUserTable(str.Split(','));
                string[] usernames = dtuser.AsEnumerable().Select(d => d.Field<string>("realname")).ToArray();
                data.Rows[i]["approveusername"] = usernames.Length > 0 ? string.Join(",", usernames) : "";
            }
            return data;
        }
        /// <summary>
        /// 获取执行情况
        /// </summary>
        /// <param name="fireWaterId"></param>
        /// <returns></returns>
        public FireWaterCondition GetConditionEntity(string fireWaterId) {
            Repository<FireWaterCondition> condition = new Repository<FireWaterCondition>(DbFactory.Base());
            return condition.FindEntity(x => x.FireWaterId == fireWaterId);
        }

        /// <summary>
        /// 获取执行情况集合
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public IList<FireWaterCondition> GetConditionList(string keyValue)
        {
            Repository<FireWaterCondition> condition = new Repository<FireWaterCondition>(DbFactory.Base());
            return condition.IQueryable().Where(t => t.FireWaterId == keyValue).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public FireWaterEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
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
            DataTable nodeDt = highriskcommonapplyservice.GetCheckInfo(keyValue, modulename, user);
            FireWaterEntity entity = GetEntity(keyValue);
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
                        highriskcommonapplyservice.GetExecutedept(entity.WorkDeptType.ToString(), entity.WorkDeptId, entity.EngineeringId, out executedept);//获取执行部门
                        string createdetpid = departmentservice.GetEntityByCode(entity.CreateUserDeptCode).IsEmpty() ? "" : departmentservice.GetEntityByCode(entity.CreateUserDeptCode).DepartmentId; //获取创建部门ID
                        string outsouringengineerdept = string.Empty;
                        highriskcommonapplyservice.GetOutsouringengineerDept(entity.WorkDeptId, out outsouringengineerdept);
                        string accountstr = manypowercheckservice.GetApproveUserAccount(dr["id"].ToString(), entity.Id, "", entity.SpecialtyType, executedept, outsouringengineerdept, createdetpid, "", ""); //获取审核人账号
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
                    DataRow[] end_rows = nodeDt.Select("auditusername is not null");
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

        /// <summary>
        /// 获取APP流程图
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="modulename"></param>
        /// <returns></returns>
        public List<CheckFlowData> GetAppFlowList(string keyValue, string modulename)
        {
            List<CheckFlowData> nodelist = new List<CheckFlowData>();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            FireWaterEntity entity = GetEntity(keyValue);
            DataTable dt = highriskcommonapplyservice.GetCheckInfo(keyValue, modulename, user);
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
                        string executedept = string.Empty;
                        highriskcommonapplyservice.GetExecutedept(entity.WorkDeptType.ToString(), entity.WorkDeptId, entity.EngineeringId, out executedept);//获取执行部门
                        string createdetpid = departmentservice.GetEntityByCode(entity.CreateUserDeptCode).IsEmpty() ? "" : departmentservice.GetEntityByCode(entity.CreateUserDeptCode).DepartmentId; //获取创建部门ID
                        string outsouringengineerdept = string.Empty;
                        highriskcommonapplyservice.GetOutsouringengineerDept(entity.WorkDeptId, out outsouringengineerdept);
                        string accountstr = manypowercheckservice.GetApproveUserAccount(dr["id"].ToString(), entity.Id, "", entity.SpecialtyType, executedept, outsouringengineerdept, createdetpid, "", ""); //获取审核人账号
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
                        checkdata.auditdeptname = deptnames.Length > 0 ? string.Join(",", deptnames) : "无";
                        checkdata.auditusername = usernames.Length > 0 ? string.Join(",", usernames) : "无";
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

        #region 获取使用消防水台账
        /// <summary>
        /// 获取使用消防水台账
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetLedgerList(Pagination pagination, string queryJson, Operator user)
        {
            DatabaseType dataType = DatabaseType.Oracle;
            #region 数据权限
            string curUserId = user.UserId;
            #endregion
            /*
             即将作业:审批通过且实际作业时间为空
             作业中:审批通过且实际作业开始时间不为空且实际作业结束时间为空
             已结束:审批通过且实际作业结束时间不为空
             */
            #region 查表
            pagination.p_kid = "Id as workid";
            pagination.p_fields = "case when workdepttype=0 then '单位内部' when workdepttype=1 then '外包单位' end workdepttypename,workdepttype,workdeptid,workdeptname,workdeptcode,applynumber,a.createdate,workplace,workcontent,workstarttime,workendtime,applyusername,engineeringname,engineeringid,case when a.workoperate='1' then '作业暂停' when realityworkstarttime is not null and realityworkendtime is null then '作业中' when realityworkendtime is not null then '已结束'  else '即将作业' end ledgertype,a.workuserids,a.RealityWorkStartTime,a.RealityWorkEndTime,'' as isoperate,a.createuserid";
            pagination.p_tablename = " bis_firewater a";
            pagination.conditionJson = "applystate='3'";
            if (!user.IsSystem)
            {
                 //配置的部门拥有特殊查看权限
                string specialDeptId = new DataItemDetailService().GetItemValue(user.OrganizeId, "FireDept");
                if (!string.IsNullOrEmpty(specialDeptId) && specialDeptId.Contains(user.DeptId))
                {
                    pagination.conditionJson += " and a.createuserorgcode='" + user.OrganizeCode + "'";
                }
                else
                {
                    if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级"))
                    {
                        pagination.conditionJson += " and a.createuserorgcode='" + user.OrganizeCode + "'";
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(" and ((workdeptcode in(select encode from base_department where encode like '{0}%'))  or (engineeringid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);
                    }
                }
            }
            #endregion

            #region  筛选条件
            var queryParam = JObject.Parse(queryJson);
            //时间选择
            if (!queryParam["st"].IsEmpty())//使用消防水开始时间
            {
                string from = queryParam["st"].ToString().Trim();
                pagination.conditionJson += string.Format(" and WorkStartTime>=to_date('{0}','yyyy-mm-dd')", from);

            }
            if (!queryParam["et"].IsEmpty())//使用消防水结束时间
            {
                string to = Convert.ToDateTime(queryParam["et"].ToString().Trim()).AddDays(1).ToString("yyyy-MM-dd");
                pagination.conditionJson += string.Format(" and WorkEndTime<=to_date('{0}','yyyy-mm-dd')", to);
            }
            //查询条件
            if (!queryParam["workdeptcode"].IsEmpty())//使用消防水单位code
            {
                pagination.conditionJson += string.Format(" and workdeptcode='{0}'", queryParam["workdeptcode"].ToString());
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
                else if (ledgertype == "3")
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
            var data = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            #region 操作权限
            if (data != null)
            {
                string strRole = dataitemdetailservice.GetItemValue(user.OrganizeId, "LedgerSendDept");//责任部门角色
                string strManageRole = dataitemdetailservice.GetItemValue(user.OrganizeId, "LedgerManageDept");//安全主管部门监管角色
                string strWorkRole = dataitemdetailservice.GetItemValue(user.OrganizeId, "LedgerWorkDept");//作业单位
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    string str = "0";
                    string workUserIds = data.Rows[i]["workuserids"].ToString();//作业人员
                    string applyUserId = data.Rows[i]["createuserid"].ToString();
                    string engineeringid = data.Rows[i]["engineeringid"].ToString();//工程id
                    string workDeptType = data.Rows[i]["workdepttype"].ToString();
                    string workdeptid = data.Rows[i]["workdeptid"].ToString();//作业单位id
                    var dept = new OutsouringengineerService().GetEntity(engineeringid); //获取工程id对应的责任部门
                    if (user.RoleName.Contains("厂级") && !string.IsNullOrEmpty(strManageRole))//安全主管部门
                    {
                        string[] arrrolename = strManageRole.Split(',');
                        for (int j = 0; j < arrrolename.Length; j++)
                        {
                            if (user.RoleName.Contains(arrrolename[j]))
                            {
                                str = "1";
                                break;
                            }
                        }
                    }
                    if (str != "1" && !string.IsNullOrEmpty(workdeptid))
                    {
                        string[] arrrolename = strWorkRole.Split(',');
                        for (int j = 0; j < arrrolename.Length; j++)
                        {
                            if (user.RoleName.Contains(arrrolename[j]))
                            {
                                str = "1";
                                break;
                            }
                        }
                    }
                    if (str != "1" && ((!string.IsNullOrEmpty(workUserIds) && workUserIds.Contains(curUserId)) || curUserId == applyUserId))//作业人员或申请人
                    {
                        str = "1";
                    }
                    if (str != "1" && dept != null)
                    {
                        if (workDeptType == "1")//责任部门
                        {
                            if (dept.ENGINEERLETDEPTID == user.DeptId && !string.IsNullOrEmpty(strRole))
                            {
                                string[] arrrolename = strRole.Split(',');
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
                    data.Rows[i]["isoperate"] = str;
                }
            }
            #endregion
            return data;
        }
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
                db.Delete<HighRiskRecordEntity>(t => t.WorkId.Equals(keyValue));
                db.Delete<FileInfoEntity>(t => t.RecId.Equals(keyValue));
                db.Delete<FireWaterEntity>(keyValue);
                db.Commit();
            }
            catch (Exception)
            {
                db.Rollback();
                throw;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, FireWaterEntity entity)
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
        /// <param name="model">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, FireWaterModel model)
        {
            var res = DbFactory.Base().BeginTrans();
            try
            {

                Repository<FireWaterEntity> repFire = new Repository<FireWaterEntity>(DbFactory.Base());
                FireWaterEntity entity = repFire.FindEntity(keyValue);
                //新增
                if (entity == null)
                {
                    entity = new FireWaterEntity();
                    entity.Id = keyValue;
                    entity.Create();
                    //实体赋值
                    this.copyProperties(entity, model);
                    //生成编码
                    entity.ApplyNumber = this.GetMaxCode();
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
                //添加或更新作业安全分析 先删除再添加
                res.Delete<HighRiskRecordEntity>(t => t.WorkId == entity.Id);
                if (model.RiskRecord != null)
                {
                    var num = 0;
                    foreach (var risk in model.RiskRecord)
                    {
                        risk.CreateDate = DateTime.Now.AddSeconds(-num);
                        risk.Create();
                        res.Insert(risk);
                        num++;
                    }
                }

                res.Commit();
            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }
        }

        /// <summary>
        /// 更新业务表、审核表
        /// </summary>
        /// <param name="firewaterentity">业务主表实体</param>
        /// <param name="auditEntity">审核表实体</param>
        public void UpdateForm(FireWaterEntity fireWaterEntity, ScaffoldauditrecordEntity auditEntity)
        {
            try
            {
                this.SaveForm(fireWaterEntity.Id, fireWaterEntity);
                if (auditEntity != null)
                {
                    auditEntity.AuditDate = DateTime.Now;
                    auditEntity.AuditSignImg = string.IsNullOrWhiteSpace(auditEntity.AuditSignImg) ? "" : auditEntity.AuditSignImg.ToString().Replace("../..", "");
                    scaffoldauditrecordservice.SaveForm(auditEntity.Id, auditEntity);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 从源实体给目标实体属性赋值
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="source">源</param>
        private void copyProperties(FireWaterEntity target, FireWaterModel source)
        {
            target.ApplyDeptId = source.ApplyDeptId;
            target.ApplyDeptCode = source.ApplyDeptCode;
            target.ApplyDeptName = source.ApplyDeptName;
            target.ApplyUserId = source.ApplyUserId;
            target.ApplyUserName = source.ApplyUserName;
            target.ApplyNumber = source.ApplyNumber;
            target.SpecialtyType = source.SpecialtyType;
            target.WorkDeptType = source.WorkDeptType;
            target.WorkDeptId = source.WorkDeptId;
            target.WorkDeptCode = source.WorkDeptCode;
            target.WorkDeptName = source.WorkDeptName;
            target.EngineeringId = source.EngineeringId;
            target.EngineeringName = source.EngineeringName;
            target.WorkUserIds = source.WorkUserIds;
            target.WorkUserNames = source.WorkUserNames;
            target.WorkUse = source.WorkUse;
            target.WorkStartTime = source.WorkStartTime;
            target.WorkEndTime = source.WorkEndTime;
            target.WorkAreaCode = source.WorkAreaCode;
            target.WorkAreaName = source.WorkAreaName;
            target.WorkPlace = source.WorkPlace;
            target.WorkContent = source.WorkContent;
            target.Measure = source.Measure;
            target.CopyUserIds = source.CopyUserIds;
            target.CopyUserNames = source.CopyUserNames;
            target.IsMessage = source.IsMessage;
            target.ApplyState = source.ApplyState;

            //审核流程信息
            target.FlowId = source.FlowId;
            target.FlowName = source.FlowName;
            target.FlowRole = source.FlowRole;
            target.FlowRoleName = source.FlowRoleName;
            target.FlowDept = source.FlowDept;
            target.FlowDeptName = source.FlowDeptName;
            target.InvestigateState = source.InvestigateState;
            target.FlowRemark = source.FlowRemark;
            target.Tool = source.Tool;
            target.hdTool = source.hdTool;
        }
        /// <summary>
        /// 提交执行情况
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public void SubmitCondition(string keyValue, FireWaterCondition entity)
        {

            var res = new Repository<FireWaterCondition>(DbFactory.Base()).BeginTrans();
            try
            {
                entity.Id = keyValue;
                if (!string.IsNullOrEmpty(keyValue))
                {
                    var e = res.FindEntity(keyValue);
                    if (e != null)
                    {
                        entity.Modify(keyValue);
                        res.Update(entity);
                    }
                    else
                    {
                        entity.Id = keyValue;
                        entity.Create();
                        res.Insert(entity);
                    }
                }
                else
                {
                    entity.Create();
                    res.Insert(entity);
                }
                res.Commit();
            }
            catch (Exception ex)
            {

                res.Rollback();
            }
          
        }
        #endregion
    }
}
