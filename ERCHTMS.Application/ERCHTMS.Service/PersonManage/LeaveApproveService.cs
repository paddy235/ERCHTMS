using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Entity.PersonManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.IService.PersonManage;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Service.OutsourcingProject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Service.PersonManage
{
    /// <summary>
    /// 外包人员离场审核
    /// </summary>
    public class LeaveApproveService : RepositoryFactory<LeaveApproveEntity>, LeaveApproveIService
    {
        private IManyPowerCheckService manypowercheckservice = new ManyPowerCheckService();
        private AptitudeinvestigateauditIService aptService = new AptitudeinvestigateauditService();
        private IUserService userservice = new UserService();

        #region [获取数据]
        /// <summary>
        /// 离场审核列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetLeaveApproveList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            pagination.p_kid = "t.ID";
            pagination.p_tablename = "BIS_LEAVEAPPROVE t";
            pagination.p_fields = "t.ApplyDeptId,t.ApplyDeptName,t.LeaveTime,t.LeaveReason,t.LeaveUserNames,t.CreateUserId,t.CreateUserName,t.CreateUserDeptCode,t.CreateDate,t.CreateUserOrgCode,decode(t.ApproveState,0,'待审核',1,'通过',2,'不通过') as ApproveState,t.FlowId,'' as approveuseraccount,t.LeaveDeptId";
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            if (curUser.Account != "System")
            {
                if (curUser.RoleName.Contains("承包商"))
                {
                    pagination.conditionJson += string.Format("and t.LeaveDeptId in (select departmentid from base_department start with encode='{0}' connect by  prior departmentid = parentid)", curUser.DeptCode);
                }
                else
                {
                    pagination.conditionJson += string.Format(" and t.CREATEUSERORGCODE like '{0}%'", curUser.OrganizeCode);
                }
            }
            DataTable data = BaseRepository().FindTable("select " + pagination.p_kid + "," + pagination.p_fields + " from " + pagination.p_tablename + " where " + pagination.conditionJson);

            string str = string.Empty;
            foreach (DataRow row in data.Rows)
            {
                str = manypowercheckservice.GetApproveUserAccount(row["flowid"].ToString(), row["id"].ToString(), "", "", row["LeaveDeptId"].ToString());
                //获取审核人账号
                row["approveuseraccount"] = str;
            }
            if (queryJson.Contains("mode") && queryParam["mode"].ToString() == "dbsx")
            {
                string[] applyids = data.Select("approveuseraccount  like '%" + curUser.Account + "%'").Select(t => t.Field<string>("id")).ToArray();
                pagination.conditionJson += string.Format(" and t.id in ('{0}')", string.Join("','", applyids));
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// 获取表单数据
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public LeaveApproveEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 流程图
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public Flow GetFlow(string keyValue)
        {
            List<nodes> nlist = new List<nodes>();
            List<lines> llist = new List<lines>();
            DataTable nodeDt = GetCheckInfo(keyValue, "外包人员离厂");
            LeaveApproveEntity entity = GetEntity(keyValue);
            Flow flow = new Flow();
            flow.title = "";
            flow.initNum = 22;
            flow.activeID = entity.FlowId;
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (nodeDt != null && nodeDt.Rows.Count > 0)
            {
                #region [创建node对象]
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
                    if (dr["AUDITDEPT"] != null && !string.IsNullOrEmpty(dr["AUDITDEPT"].ToString()))
                    {
                        sinfo.Taged = 1;
                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                        DateTime auditdate;
                        DateTime.TryParse(dr["AUDITTIME"].ToString(), out auditdate);
                        nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                        nodedesignatedata.creatdept = dr["AUDITDEPT"].ToString();
                        nodedesignatedata.createuser = dr["AUDITPEOPLE"].ToString();
                        nodedesignatedata.status = dr["AUDITRESULT"].ToString() == "0" ? "同意" : "不同意";
                        if (dr["AUDITRESULT"].ToString() == "1")
                        {
                            flow.activeID = "";
                        }
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
                        //获取执行部门
                        string account = manypowercheckservice.GetApproveUserAccount(nodeDt.Rows[i]["id"].ToString(), keyValue, "", "", entity.LeaveDeptId);

                        DataTable dtuser = userservice.GetUserTable(account.Split(','));
                        string[] usernames = dtuser.AsEnumerable().Select(d => d.Field<string>("realname")).ToArray();
                        string[] deptnames = dtuser.AsEnumerable().Select(d => d.Field<string>("deptname")).ToArray().GroupBy(t => t).Select(p => p.Key).ToArray();
                        nodedesignatedata.createuser = usernames.Length > 0 ? string.Join(",", usernames) : "无";
                        nodedesignatedata.creatdept = deptnames.Length > 0 ? string.Join(",", deptnames) : "无";
                        //nodedesignatedata.createuser = "无";
                        //nodedesignatedata.creatdept = "无";
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
                if (entity.ApproveState != 0)
                {
                    setInfo sinfo = new setInfo();
                    sinfo.NodeName = nodes_end.name;
                    sinfo.Taged = 1;
                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                    NodeDesignateData nodedesignatedata = new NodeDesignateData();

                    //取流程结束时的节点信息
                    DataRow[] end_rows = nodeDt.Select("AUDITPEOPLE is not null").OrderBy(t => t.Field<DateTime>("AUDITTIME")).ToArray();
                    DataRow end_row = end_rows[end_rows.Count() - 1];
                    DateTime auditdate;
                    DateTime.TryParse(end_row["AUDITTIME"].ToString(), out auditdate);
                    nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                    nodedesignatedata.creatdept = end_row["AUDITDEPT"].ToString();
                    nodedesignatedata.createuser = end_row["AUDITPEOPLE"].ToString();
                    nodedesignatedata.status = end_row["AUDITRESULT"].ToString() == "0" ? "同意" : "不同意";
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

        public DataTable GetCheckInfo(string keyValue, string modulename)
        {
            string sql = string.Format(@"select b.APTITUDEID,a.id,a.flowname,a.serialnum,a.autoid,a.checkdeptname,a.checkroleid,a.checkrolename,a.checkdeptid,b.AUDITPEOPLE,b.AUDITOPINION,b.AUDITDEPT
,b.AUDITTIME,b.AUDITRESULT from bis_manypowercheck a left join EPG_APTITUDEINVESTIGATEAUDIT b on a.id=b.flowid and b.aptitudeid='{0}'  where a.modulename='{1}' order by SERIALNUM", keyValue, modulename);
            return this.BaseRepository().FindTable(sql);
        }

        public DataTable GetLeaveApproveData(string sql)
        {
            return this.BaseRepository().FindTable(sql);
        }

        /// <summary>
        /// 首页待办事项
        /// </summary>
        /// <param name="curUser"></param>
        /// <returns></returns>
        public int GetDBSXNum(Operator curUser)
        {
            int count = 0;
            var list = this.BaseRepository().FindList("select Id,FlowId,LeaveDeptId from BIS_LEAVEAPPROVE where ApproveState=0");

            foreach (var item in list)
            {
                string str = manypowercheckservice.GetApproveUserAccount(item.FlowId, item.Id, "", "", item.LeaveDeptId);
                if (str.IndexOf(curUser.Account) > -1)
                {
                    count++;
                }
            }
            return count;
        }

        #endregion

        #region [提交数据]
        /// <summary>
        /// 提交离场申请
        /// </summary>
        /// <param name="entity"></param>
        public bool SaveForm(LeaveApproveEntity entity)
        {
            bool flag = false;
            try
            {
                Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
                entity.ApplyDeptId = curUser.DeptId;
                entity.ApplyDeptName = curUser.DeptName;
                string state = string.Empty;
                ManyPowerCheckEntity mpcEntity = null;
                string moduleName = "外包人员离厂";
                mpcEntity = manypowercheckservice.CheckAuditForNext(curUser, moduleName, entity.FlowId);
                if (mpcEntity != null)
                {
                    entity.ApproveState = 0;
                    entity.FlowDept = mpcEntity.CHECKDEPTID;
                    entity.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                    entity.FlowRole = mpcEntity.CHECKROLEID;
                    entity.FlowRoleName = mpcEntity.CHECKROLENAME;
                    entity.FlowId = mpcEntity.ID;
                    entity.FlowName = "待审核";
                    flag = true;
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                }
                else
                {
                    flag = false;
                }
               
            }
            catch (Exception ex)
            {
                flag = false;
            }
            return flag;
        }
        /// <summary>
        /// 离场审批
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="aentity"></param>
        public PushMessageData LeaveApprove(string keyValue, LeaveApproveEntity entity, AptitudeinvestigateauditEntity aentity)
        {
            PushMessageData pushdata = new PushMessageData();
            var res = DbFactory.Base().BeginTrans();
            try
            {
                string sendCode = string.Empty;
                Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
                ManyPowerCheckEntity mpcEntity = manypowercheckservice.CheckAuditForNext(curUser, "外包人员离厂", entity.FlowId);
                aentity.FlowId = entity.FlowId;
                aentity.AUDITDEPTID = curUser.DeptId;
                aentity.AUDITPEOPLEID = curUser.UserId;
                aentity.APTITUDEID = keyValue;
                if (null != mpcEntity)
                {
                    entity.FlowDept = mpcEntity.CHECKDEPTID;
                    entity.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                    entity.FlowRole = mpcEntity.CHECKROLEID;
                    entity.FlowRoleName = mpcEntity.CHECKROLENAME;
                    entity.FlowId = mpcEntity.ID;
                }
                else
                {
                    entity.FlowDept = "";
                    entity.FlowDeptName = "";
                    entity.FlowRole = "";
                    entity.FlowRoleName = "";
                    entity.FlowId = "";
                }
                if (aentity.AUDITRESULT == "0")
                {
                    entity.FlowName = "通过";
                    entity.ApproveState = 1;
                    pushdata.SendCode = "WB001";//推送给消息申请人
                }
                else if (aentity.AUDITRESULT == "1")
                {
                    entity.FlowName = "不通过";
                    entity.ApproveState = 2;
                    pushdata.SendCode = "WB002";//推送给消息申请人
                }
                //审批结束回写状态
                res.ExecuteBySql(string.Format("update base_user set IsLeaving=0 where userid in('{0}')", entity.LeaveUserIds.Replace(",", "','")));
                aentity.Create();
                res.Insert<AptitudeinvestigateauditEntity>(aentity);
                //更改申请单
                res.Update<LeaveApproveEntity>(entity);
                res.Commit();
                //短消息
                pushdata.Success = 1;

                pushdata.EntityId = entity.Id;
            }
            catch (Exception ex)
            {
                pushdata.Success = 0;
                res.Rollback();
            }
            return pushdata;
        }
        #endregion
    }
}
