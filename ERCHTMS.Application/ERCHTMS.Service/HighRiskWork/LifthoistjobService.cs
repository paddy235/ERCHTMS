using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Code;
using System;
using BSFramework.Data;
using System.Data.Common;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Data;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Service.HighRiskWork;
using ERCHTMS.Service.BaseManage;
using BSFramework.Util.Extension;

namespace ERCHTMS.Service.HighRiskWork
{
    /// <summary>
    /// 描 述：起重吊装作业
    /// </summary>
    public class LifthoistjobService : RepositoryFactory<LifthoistjobEntity>, LifthoistjobIService
    {
        private HighRiskCommonApplyService highriskcommonapplyservice = new HighRiskCommonApplyService();
        private DepartmentService departmentservice = new DepartmentService();
        private ManyPowerCheckService manypowercheckservice = new ManyPowerCheckService();
        private UserService userservice = new UserService();
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public DataTable GetList(Pagination page, LifthoistSearchModel search)
        {
            DatabaseType dataTye = DatabaseType.Oracle;

            #region 查表
            page.p_kid = "a.id";
            page.p_fields = @"a.applyuserid,a.applyusername,a.applycompanyname,a.applydate,a.applycode,a.applycodestr,a.qualitytype,f.itemname as qualitytypename,a.toolname as toolvalue,c.itemname as toolname,a.workdepttype,a.engineeringid,a.createuserdeptcode,
                                a.constructionunitid,a.constructionunitname,a.constructionunitcode,a.constructionaddress,a.workstartdate,a.workenddate,a.auditstate,
                                a.flowid,a.flowname,a.specialtytype,a.flowroleid,a.flowrolename,a.flowdeptid,a.flowdeptname,a.flowremark,'' as approveuserid,'' as approveusername,'' as approveuseraccount,b.outtransferuseraccount,b.intransferuseraccount";
            page.p_tablename = @"bis_lifthoistjob a left join base_dataitemdetail f on a.qualitytype=f.itemvalue and f.itemid=(select itemid from base_dataitem where itemcode='LifthoistQualityType') left join base_dataitemdetail c on a.toolname=c.itemvalue and c.itemid =(select itemid from base_dataitem where itemcode='ToolName')
                                left join (select recid,flowid,outtransferuseraccount,intransferuseraccount,row_number()  over(partition by recid,flowid order by createdate desc) as num from BIS_TRANSFERRECORD where disable=0) b on a.id=b.recid and a.flowid=b.flowid and b.num=1";
            #endregion

            #region  筛选条件
            //作业状态
            if (!string.IsNullOrEmpty(search.auditstate))
            {
                page.conditionJson += " and a.auditstate in (" + search.auditstate + ")";
            }
            //作业时间
            if (!string.IsNullOrEmpty(search.workstartdate))
            {
                page.conditionJson += string.Format(" and a.WorkStartDate >= to_date('{0}','yyyy-MM-dd')", search.workstartdate);
            }
            if (!string.IsNullOrEmpty(search.workenddate))
            {
                page.conditionJson += string.Format(" and a.WorkEndDate <= to_date('{0}','yyyy-MM-dd')", Convert.ToDateTime(search.workenddate).AddDays(1).ToString("yyyy-MM-dd"));
            }
            //申请编号
            if (!string.IsNullOrEmpty(search.applycode))
            {
                page.conditionJson += " and a.applycodestr like '%" + search.applycode + "%'";
            }
            //作业单位
            if (!string.IsNullOrEmpty(search.constructionunitid))
            {
                page.conditionJson += " and a.constructionunitid = '" + search.constructionunitid + "'";
            }
            if (!string.IsNullOrEmpty(search.qualitytype))
            {
                page.conditionJson += " and a.qualitytype = " + search.qualitytype;
            }
            if (!string.IsNullOrEmpty(search.toolname))
            {
                page.conditionJson += " and a.toolname like '%" + search.toolname + "%'";
            }
            if (!string.IsNullOrEmpty(search.viewrange))
            {
                var user = OperatorProvider.Provider.Current();
                //本人
                if (search.viewrange.ToLower() == "self")
                {
                    page.conditionJson += string.Format(" and a.ApplyUserId='{0}'", user.UserId);
                }
                else if (search.viewrange == "selfaudit")
                {

                    string strCondition = " and a.AuditState in(1)";
                    DataTable dt = BaseRepository().FindTable("select " + page.p_kid + "," + page.p_fields + " from " + page.p_tablename + " where " + page.conditionJson + strCondition);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string executedept = string.Empty;
                        highriskcommonapplyservice.GetExecutedept(dt.Rows[i]["workdepttype"].ToString(), dt.Rows[i]["constructionunitid"].ToString(), dt.Rows[i]["engineeringid"].ToString(), out executedept);
                        string createdetpid = departmentservice.GetEntityByCode(dt.Rows[i]["createuserdeptcode"].ToString()).IsEmpty() ? "" : departmentservice.GetEntityByCode(dt.Rows[i]["createuserdeptcode"].ToString()).DepartmentId;
                        string outsouringengineerdept = string.Empty;
                        highriskcommonapplyservice.GetOutsouringengineerDept(dt.Rows[i]["constructionunitid"].ToString(), out outsouringengineerdept);
                        string str = manypowercheckservice.GetApproveUserAccount(dt.Rows[i]["flowid"].ToString(), dt.Rows[i]["id"].ToString(), "", dt.Rows[i]["specialtytype"].ToString(), executedept, outsouringengineerdept, createdetpid, "", "");
                        dt.Rows[i]["approveuseraccount"] = str;
                    }
                    string[] applyids = dt.Select("(outtransferuseraccount is null or outtransferuseraccount not like '%" + user.Account + ",%') and (approveuseraccount like '%" + user.Account + ",%' or intransferuseraccount like '%" + user.Account + ",%')").AsEnumerable().Select(d => d.Field<string>("id")).ToArray();

                    page.conditionJson += string.Format(" and a.id in ('{0}') {1}", string.Join("','", applyids), strCondition);
                }
                else if (search.viewrange=="selfoperator")
                {
                    page.conditionJson += string.Format(" and a.id in(select businessid from  bis_lifthoistauditrecord where audituserid='{0}')", user.UserId);
                }
            }

            #endregion
            DataTable data= this.BaseRepository().FindTableByProcPager(page, dataTye);
            for (int i = 0; i < data.Rows.Count; i++)
            {
                string executedept = string.Empty;
                highriskcommonapplyservice.GetExecutedept(data.Rows[i]["workdepttype"].ToString(), data.Rows[i]["constructionunitid"].ToString(), data.Rows[i]["engineeringid"].ToString(), out executedept); //获取执行部门
                string createdetpid = departmentservice.GetEntityByCode(data.Rows[i]["createuserdeptcode"].ToString()).IsEmpty() ? "" : departmentservice.GetEntityByCode(data.Rows[i]["createuserdeptcode"].ToString()).DepartmentId; //获取创建部门
                string outsouringengineerdept = string.Empty;
                highriskcommonapplyservice.GetOutsouringengineerDept(data.Rows[i]["constructionunitid"].ToString(), out outsouringengineerdept);
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
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public DataTable getTempEquipentList(Pagination page, LifthoistSearchModel search)
        {
            DatabaseType dataTye = DatabaseType.Oracle;

            #region 查表
            page.p_kid = "t.id";
            page.p_fields = @"t.id as jobid,t1.id as certid,t.applycompanyid,t.applycompanycode,t.applycompanyname,t.toolname as toolvalue,c.itemname as toolname,t.workstartdate,t.workenddate";
            page.p_tablename = @"bis_lifthoistjob t left join bis_lifthoistcert t1 on t.id = t1.lifthoistjobid  left join base_dataitemdetail c on t.toolname=c.itemvalue and itemid =(select itemid from base_dataitem where itemcode='ToolName')";
            #endregion

            #region  筛选条件
            //作业状态
            if (!string.IsNullOrEmpty(search.auditstate))
            {
                page.conditionJson += " and t.auditstate in (" + search.auditstate + ")";
            }
            //作业时间
            if (!string.IsNullOrEmpty(search.workstartdate))
            {
                page.conditionJson += string.Format(" and t.WorkStartDate >= to_date('{0}','yyyy-MM-dd')", search.workstartdate);
            }
            if (!string.IsNullOrEmpty(search.workenddate))
            {
                page.conditionJson += string.Format(" and t.WorkEndDate <= to_date('{0}','yyyy-MM-dd')", Convert.ToDateTime(search.workenddate).AddDays(1).ToString("yyyy-MM-dd"));
            }
            //申请编号
            if (!string.IsNullOrEmpty(search.applycode))
            {
                page.conditionJson += " and t.applycodestr like '%" + search.applycode + "%'";
            }
            //作业单位
            if (!string.IsNullOrEmpty(search.constructionunitid))
            {
                page.conditionJson += " and t.constructionunitid = '" + search.constructionunitid + "'";
            }
            if (!string.IsNullOrEmpty(search.qualitytype))
            {
                page.conditionJson += " and t.qualitytype = " + search.qualitytype;
            }
            if (!string.IsNullOrEmpty(search.toolname))
            {
                page.conditionJson += " and t.toolname= '" + search.toolname + "'";
            }
            if (!string.IsNullOrEmpty(search.viewrange))
            {
                var user = OperatorProvider.Provider.Current();
                //本人
                if (search.viewrange.ToLower() == "self")
                {
                    page.conditionJson += string.Format(" and t.ApplyUserId='{0}'", user.UserId);
                }
                else if (search.viewrange == "selfaudit")
                {
                    string[] roles = user.RoleName.Split(',');
                    string roleWhere = "";
                    foreach (var r in roles)
                    {
                        roleWhere += string.Format("or instr(t.flowrolename,'{0}') > 0  ", r);
                    }
                    roleWhere = roleWhere.Substring(2);
                    //当前有审核权限的部门及角色，才可查看
                    page.conditionJson += string.Format("  and t.AuditState = 1 and t.flowdeptid like '%{0}%' and ({1})", user.DeptId, roleWhere);
                }
            }

            #endregion
            return this.BaseRepository().FindTableByProcPager(page, dataTye);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public LifthoistjobEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        //public string GetMaxCode()
        //{
        //    string orgCode = OperatorProvider.Provider.Current().OrganizeCode;
        //    string sql = "select max(ApplyCode) from bis_lifthoistjob where to_char(CreateDate,'MM') = @month";
        //    object o = this.BaseRepository().FindObject(sql, new DbParameter[]{
        //        DbParameters.CreateDbParameter("@month", DateTime.Now.ToString("MM"))
        //    });
        //    if (o == null || o.ToString() == "")
        //        return DateTime.Now.ToString("yyyyMM") + "001";
        //    int num = Convert.ToInt32(o.ToString().Substring(6));
        //    num++;
        //    return DateTime.Now.ToString("yyyyMM") + num.ToString().PadLeft(3, '0');
        //}
        public string GetMaxCode()
        {
            string orgCode = OperatorProvider.Provider.Current().OrganizeCode;
            string sql = string.Format("select max(ApplyCode) from bis_lifthoistjob where CreateUserOrgCode = @orgCode and ApplyCode like '%{0}%'", DateTime.Now.ToString("yyyyMMdd"));
            object o = this.BaseRepository().FindObject(sql, new DbParameter[]{
                DbParameters.CreateDbParameter("@orgCode",orgCode)
            });
            if (o == null || o.ToString() == "")
                return DateTime.Now.ToString("yyyyMMdd") + "001";
            int num = Convert.ToInt32(o.ToString().Substring(8));
            num++;
            return DateTime.Now.ToString("yyyyMMdd") + num.ToString().PadLeft(3, '0');
        }

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
            LifthoistjobEntity entity = GetEntity(keyValue);
            Flow flow = new Flow();
            flow.title = "";
            flow.initNum = 22;
            flow.activeID = entity.FLOWID;
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
                        highriskcommonapplyservice.GetExecutedept(entity.WORKDEPTTYPE.ToString(), entity.CONSTRUCTIONUNITID, entity.ENGINEERINGID, out executedept);//获取执行部门
                        string createdetpid = departmentservice.GetEntityByCode(entity.CREATEUSERDEPTCODE).IsEmpty() ? "" : departmentservice.GetEntityByCode(entity.CREATEUSERDEPTCODE).DepartmentId; //获取创建部门ID
                        string outsouringengineerdept = string.Empty;
                        highriskcommonapplyservice.GetOutsouringengineerDept(entity.CONSTRUCTIONUNITID, out outsouringengineerdept);
                        string accountstr = manypowercheckservice.GetApproveUserAccount(dr["id"].ToString(), entity.ID, "", entity.SPECIALTYTYPE, executedept, outsouringengineerdept, createdetpid, "", ""); //获取审核人账号
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
                if (entity.AUDITSTATE == 2)
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


        public List<CheckFlowData> GetAppFlowList(string keyValue, string modulename)
        {
            List<CheckFlowData> nodelist = new List<CheckFlowData>();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            LifthoistjobEntity entity = GetEntity(keyValue);
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
                        string executedept = string.Empty;
                        highriskcommonapplyservice.GetExecutedept(entity.WORKDEPTTYPE.ToString(), entity.CONSTRUCTIONUNITID, entity.ENGINEERINGID, out executedept);//获取执行部门
                        string createdetpid = departmentservice.GetEntityByCode(entity.CREATEUSERDEPTCODE).IsEmpty() ? "" : departmentservice.GetEntityByCode(entity.CREATEUSERDEPTCODE).DepartmentId; //获取创建部门ID
                        string outsouringengineerdept = string.Empty;
                        highriskcommonapplyservice.GetOutsouringengineerDept(entity.CONSTRUCTIONUNITID, out outsouringengineerdept);
                        string accountstr = manypowercheckservice.GetApproveUserAccount(dr["id"].ToString(), entity.ID, "", entity.SPECIALTYTYPE, executedept, outsouringengineerdept, createdetpid, "", ""); //获取审核人账号
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
                        if (entity.AUDITSTATE == 2)
                            checkdata.isoperate = "0";
                        else
                            checkdata.isoperate = dr["id"].ToString() == entity.FLOWID ? "1" : "0";
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

        public DataTable GetCheckInfo(string keyValue, string modulename, Operator user)
        {
            string node_sql = string.Format(@"select 
                                                    a.id,a.flowname,a.serialnum,a.autoid,a.checkdeptname,a.checkroleid,a.checkrolename,a.checkdeptid,a.remark,b.auditdeptname,b.auditusername,b.auditdate,b.auditstate,b.auditremark,e.outtransferuseraccount,e.intransferuseraccount
                                              from 
                                                    bis_manypowercheck a left join bis_lifthoistauditrecord b
                                                    on a.id = b.flowid and b.businessid = '{2}'
                                                    left join(select recid,flowid,outtransferuseraccount,intransferuseraccount,row_number() over(partition by recid,flowid order by createdate desc) as num from bis_transferrecord where disable=0 ) e on a.id=e.flowid and e.recid='{2}' and e.num=1
                                              where 
                                                    a.createuserorgcode = '{0}' and a.modulename = '{1}'
                                              order by
                                                    serialnum ", user.OrganizeCode, modulename, keyValue);
            DataTable nodeDt = this.BaseRepository().FindTable(node_sql);
            return nodeDt;
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                var res = DbFactory.Base().BeginTrans();
                this.BaseRepository().BeginTrans();
                this.BaseRepository().ExecuteBySql(string.Format("delete from bis_lifthoistjob where id = '{0}'", keyValue));
                this.BaseRepository().ExecuteBySql(string.Format("delete from bis_lifthoistauditrecord where BUSINESSID = '{0}'", keyValue));
                this.BaseRepository().ExecuteBySql(string.Format("delete from bis_lifthoistauditrecord where BUSINESSID = (select id from bis_lifthoistcert where LIFTHOISTJOBID = '{0}')", keyValue));
                this.BaseRepository().ExecuteBySql(string.Format("delete from bis_lifthoistcert where LIFTHOISTJOBID = '{0}'", keyValue));
                this.BaseRepository().Commit();
            }
            catch (Exception ex)
            {
                this.BaseRepository().Rollback();
                throw ex;
            }
        }

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, LifthoistjobEntity entity)
        {
            var res = DbFactory.Base().BeginTrans();
            List<HighRiskRecordEntity> riskList = entity.RiskRecord;
            try
            {
                Repository<LifthoistjobEntity> rep = new Repository<LifthoistjobEntity>(DbFactory.Base());
                LifthoistjobEntity jobEntity = rep.FindEntity(keyValue);
                //新增
                if (jobEntity == null)
                {
                    entity.Create();
                    entity.APPLYCODE = this.GetMaxCode();
                    entity.APPLYCODESTR = "Q/CRPHZHB 2205.10.02-JL04-" + entity.APPLYCODE;
                    res.Insert(entity);
                }
                else
                {
                    entity.RiskRecord = null;
                    entity.Modify(keyValue);
                    res.Update(entity);
                }
                //添加或更新作业安全分析 先删除再添加
                res.Delete<HighRiskRecordEntity>(t => t.WorkId == keyValue);
                if (riskList != null)
                {
                    var num = 0;
                    foreach (var risk in riskList)
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
        /// 审核更新
        /// </summary>
        /// <param name="jobEntity">起重吊装作业实体</param>
        /// <param name="auditEntity">审核实体</param>
        public void ApplyCheck(LifthoistjobEntity jobEntity, LifthoistauditrecordEntity auditEntity)
        {
            var res = DbFactory.Base().BeginTrans();
            try
            {
                //起重吊装作业直接更新
                res.Update(jobEntity);
                //审核实体不为空时，才插入
                if (auditEntity != null)
                {
                    auditEntity.Create();
                    auditEntity.BUSINESSID = jobEntity.ID;
                    if (auditEntity.AUDITSTATE == 0)
                    {
                        var list = new LifthoistauditrecordService().GetList(jobEntity.ID);
                        foreach (var item in list)
                        {
                            item.DISABLE = 1;
                            res.Update(item);
                        }
                        auditEntity.DISABLE = 1;
                    }
                    res.Insert(auditEntity);
                }
                res.Commit();

            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }
        }
        #endregion
    }
}
