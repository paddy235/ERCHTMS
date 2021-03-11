using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Entity.TrainPlan;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.IService.TrainPlan;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Service.OutsourcingProject;
using ERCHTMS.Service.TrainPlan;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Busines.TrainPlan
{
    /// <summary>
    /// 描 述：安措计划
    /// </summary>
    public class SafeMeasureBLL
    {
        private ISafeMeasureService service = new SafeMeasureService();
        private IUserService userservice = new UserService();
        private IDepartmentService deptservice = new DepartmentService();
        private ISafeAdjustmentService adjustmentService = new SafeAdjustmentService();
        private AptitudeinvestigateauditIService aptitudeinvestigateauditIService = new AptitudeinvestigateauditService();
        private IManyPowerCheckService manyPowerCheckService = new ManyPowerCheckService();

        #region 获取数据
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public SafeMeasureEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        public DataTable GetList(Pagination pagination, string queryJson)
        {
            return service.GetList(pagination, queryJson);
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ExistSafeMeasure(SafeMeasureEntity entity)
        {
            return service.ExistSafeMeasure(entity);
        }

        /// <summary>
        /// 获取审批人
        /// </summary>
        /// <param name="flowRoleName"></param>
        /// <returns></returns>
        public string GetNextStepUser(string flowRoleName, string deptId, out string userName)
        {
            string approveuserids = string.Empty;
            string[] nextStepUser = service.GetNextStepUser(flowRoleName, deptId).Split('|');
            userName = nextStepUser[0];
            approveuserids = nextStepUser[2];
            return approveuserids;
        }

        /// <summary>
        /// 获取流程图
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="modulename"></param>
        /// <returns></returns>
        public Flow GetFlow(string modulename, string keyValue, string adjustId)
        {
            List<nodes> nlist = new List<nodes>();
            List<lines> llist = new List<lines>();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            //得到流程节点
            SafeAdjustmentEntity adjustmentEntity = adjustmentService.GetEntity(keyValue);
            if (string.IsNullOrEmpty(adjustId))
            {
                //取最新一条
                adjustmentEntity = adjustmentService.GetEntity(keyValue);
                adjustId = adjustmentEntity.ID;
            }
            DataTable nodeDt = service.GetCheckInfo(modulename, keyValue, adjustId);
            SafeMeasureEntity entity = service.GetEntity(keyValue);
            Flow flow = new Flow();
            flow.title = "";
            flow.initNum = 22;
            flow.activeID = entity.FlowId;

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
                        string executeDeptId = string.Empty;
                        if (dr["checkdeptid"].ToString() == "-3")
                        {
                            executeDeptId = service.GetExecuteDept(entity.Id);
                        }
                        else
                        {
                            executeDeptId = dr["checkdeptid"].ToString();
                        }
                        //获取执行人
                        IManyPowerCheckService powerCheck = new ManyPowerCheckService();
                        //获取流程节点配置
                        List<ManyPowerCheckEntity> powerList = powerCheck.GetListBySerialNum(user.OrganizeCode, "安措计划调整审批");
                        if (powerList.Count > 0)
                        {
                            var item = powerList.Where(t => t.FLOWNAME == dr["FLOWNAME"].ToString()).FirstOrDefault();
                            if (item.CHECKDEPTCODE == "-3" || item.CHECKDEPTID == "-3")
                            {
                                string executedept = new DepartmentService().GetEntity(executeDeptId).DepartmentId;
                                switch (item.ChooseDeptRange) //判断部门范围
                                {
                                    case "0":
                                        item.CHECKDEPTID = executedept;
                                        break;
                                    case "1":
                                        var dept = deptservice.GetEntity(executedept);
                                        while (dept.Nature != "部门" && dept.Nature != "厂级")
                                        {
                                            dept = deptservice.GetEntity(dept.ParentId);
                                        }
                                        item.CHECKDEPTID = dept.DepartmentId;
                                        break;
                                    case "2":
                                        var dept1 = deptservice.GetEntity(executedept);
                                        while (dept1.Nature != "部门" && dept1.Nature != "厂级")
                                        {
                                            dept1 = deptservice.GetEntity(dept1.ParentId);
                                        }
                                        item.CHECKDEPTID = (dept1.DepartmentId + "," + executedept).Trim(',');
                                        break;
                                    default:
                                        item.CHECKDEPTID = executedept;
                                        break;
                                }
                                executeDeptId = item.CHECKDEPTID;
                            }
                        }
                        string userNames = "";
                        string execusers = GetNextStepUser(dr["checkrolename"].ToString(), executeDeptId, out userNames);
                        DataTable dtuser = userservice.GetUserTable(execusers.Split(','));
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
                if (entity.Stauts == "审批不通过" || (entity.IsOver == 1 && entity.Stauts == "审批通过"))
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
                #endregion
            }
            return flow;
        }

        /// <summary>
        /// 获取安措计划季度数据
        /// </summary>
        /// <param name="belongYear"></param>
        /// <param name="quarter"></param>
        /// <returns></returns>
        public DataTable GetSafeMeasureData(string queryJson)
        {
            return service.GetSafeMeasureData(queryJson);
        }

        /// <summary>
        /// 获取反馈信息
        /// </summary>
        /// <param name="safeMeasureId"></param>
        /// <returns></returns>
        public DataTable GetFeedbackInfo(string safeMeasureId)
        {
            return service.GetFeedbackInfo(safeMeasureId);
        }
        #endregion
        /// <summary>
        /// 批量保存
        /// </summary>
        /// <param name="list"></param>
        public void SaveForm(List<SafeMeasureEntity> list)
        {
            service.SaveForm(list);
        }

        public void UpdateSafeMeasure(SafeMeasureEntity entity)
        {
            service.UpdateSafeMeasure(entity);
        }

        /// <summary>
        /// 删除安措计划
        /// </summary>
        /// <param name="keyValue"></param>
        public void RemoveForm(string keyValue, string iscommit)
        {
            if (iscommit != null && iscommit.Equals("1"))
            {
                //如果有审批流程，先删除调整申请和审批信息
                adjustmentService.DeleteAdjustment(keyValue);
                aptitudeinvestigateauditIService.DeleteFormByAptitudeId(keyValue);
            }
            service.RemoveForm(keyValue);
        }

        /// <summary>
        /// 判断是否存在未下发数据
        /// </summary>
        /// <returns></returns>
        public bool CheckUnPublish(string userId)
        {
            return service.CheckUnPublish(userId);
        }

        /// <summary>
        /// 下发
        /// </summary>
        /// <param name="userId"></param>
        public void IssueData(string userId)
        {
            service.IssueData(userId);
        }
    }
}
