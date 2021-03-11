using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Data;
using BSFramework.Data;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Service.EquipmentManage;
using ERCHTMS.Service.BaseManage;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data.Common;
using ERCHTMS.Service.SystemManage;
using ERCHTMS.Entity.HiddenTroubleManage;

namespace ERCHTMS.Service.OutsourcingProject
{
    /// <summary>
    /// 描 述：工器具验收
    /// </summary>
    public class ToolsService : RepositoryFactory<ToolsEntity>, IToolsService
    {
        private ToolsAuditIService ToolsAudit = new ToolsAuditService();
        private PeopleReviewIService peopleReview = new PeopleReviewService();
        private OutsouringengineerService outsouringengineerservice = new OutsouringengineerService();
        private DepartmentService departmentservice = new DepartmentService();
        private ManyPowerCheckService powerCheck = new ManyPowerCheckService();
        private ProjecttoolsService ProjecttoolsService = new ProjecttoolsService();
        private DataItemDetailService DataItemDetailService = new DataItemDetailService();
        private UserService UserService = new UserService();
        #region 获取数据

        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            DatabaseType dataType = DbHelper.DbType;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!string.IsNullOrEmpty(queryJson))
            {
                var queryParam = queryJson.ToJObject();
                //时间范围
                if (!queryParam["sTime"].IsEmpty() || !queryParam["eTime"].IsEmpty())
                {
                    string startTime = queryParam["sTime"].ToString();
                    string endTime = queryParam["eTime"].ToString();
                    if (queryParam["sTime"].IsEmpty())
                    {
                        startTime = "1899-01-01";
                    }
                    if (queryParam["eTime"].IsEmpty())
                    {
                        endTime = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    pagination.conditionJson += string.Format(" and to_date(to_char(APPLYTIME,'yyyy-MM-dd'),'yyyy-MM-dd') between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", startTime, endTime);
                }
                //查询条件
                if (!queryParam["condition"].IsEmpty() && !queryParam["txtSearch"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and {0} like '%{1}%'", queryParam["condition"].ToString(), queryParam["txtSearch"].ToString());
                }
                if (!queryParam["outengineerid"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and t.outengineerid ='{0}' ", queryParam["outengineerid"].ToString());
                }
                if (!queryParam["equiptype"].IsEmpty())
                {
                    var equiptype = queryParam["equiptype"].ToString();
                    if (equiptype == "1" || string.IsNullOrEmpty(equiptype))
                        pagination.conditionJson += string.Format(" and (t.equiptype ='{0}' or t.equiptype is null)", queryParam["equiptype"].ToString());
                    else
                        pagination.conditionJson += string.Format(" and t.equiptype ='{0}' ", queryParam["equiptype"].ToString());
                }
                if (!queryParam["orgCode"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and t.createuserorgcode ='{0}' ", queryParam["orgCode"].ToString());
                }
                if (!queryParam["indexState"].IsEmpty())//首页代办
                {
                    string strCondition = "";
                    strCondition = string.Format(" and t.createuserorgcode='{0}' and t.isover='0' and t.issaved='1'", user.OrganizeCode);
                    DataTable data = BaseRepository().FindTable("select " + pagination.p_kid + "," + pagination.p_fields + " from " + pagination.p_tablename + " where " + pagination.conditionJson + strCondition);
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        var engineerEntity = outsouringengineerservice.GetEntity(data.Rows[i]["outengineerid"].ToString());
                        var excutdept = departmentservice.GetEntity(engineerEntity.ENGINEERLETDEPTID).DepartmentId;
                        var outengineerdept = departmentservice.GetEntity(engineerEntity.OUTPROJECTID).DepartmentId;
                        var supervisordept = string.IsNullOrEmpty(engineerEntity.SupervisorId) ? "" : departmentservice.GetEntity(engineerEntity.SupervisorId).DepartmentId;
                        //获取下一步审核人
                        string str = powerCheck.GetApproveUserId(data.Rows[i]["flowid"].ToString(), data.Rows[i]["toolsid"].ToString(), "", "", excutdept, outengineerdept, "", "", "", supervisordept, data.Rows[i]["outengineerid"].ToString());
                        data.Rows[i]["approveuserids"] = str;
                    }

                    string[] applyids = data.Select(" approveuserids like '%" + user.UserId + "%'").AsEnumerable().Select(d => d.Field<string>("toolsid")).ToArray();

                    pagination.conditionJson += string.Format(" and t.toolsid in ('{0}') {1}", string.Join("','", applyids), strCondition);
                }

                if (!queryParam["projectid"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and p.id='{0}'", queryParam["projectid"].ToString());
                }
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<ToolsEntity> GetList(string queryJson)
        {
            return this.BaseRepository().FindList(" select * from EPG_TOOLS where 1=1 " + queryJson).ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ToolsEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
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
            ToolsEntity entity = GetEntity(keyValue);
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
                    int result = nlist.Where(t => t.id == nodes.id).Count();
                    if (result > 0) //判断当同一流程被审核两次时候只产生一个节点
                    {
                        continue;
                    }
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
                    if (dr["auditdept"] != null && !string.IsNullOrEmpty(dr["auditdept"].ToString()))
                    {
                        sinfo.Taged = 1;
                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        foreach (DataRow item in nodeDt.Select(" id ='" + dr["id"] + "'"))
                        {
                            NodeDesignateData nodedesignatedata = new NodeDesignateData();
                            DateTime auditdate;
                            DateTime.TryParse(item["audittime"].ToString(), out auditdate);
                            nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                            nodedesignatedata.creatdept = item["auditdept"].ToString();
                            nodedesignatedata.createuser = item["auditpeople"].ToString();
                            nodedesignatedata.status = item["auditresult"].ToString() == "0" ? "同意" : "不同意";
                            if (i == 0)
                            {
                                nodedesignatedata.prevnode = "无";
                            }
                            else
                            {
                                nodedesignatedata.prevnode = nodeDt.Rows[i - 1]["flowname"].ToString();
                            }
                            nodelist.Add(nodedesignatedata);
                        }
                        
                        sinfo.NodeDesignateData = nodelist;
                        nodes.setInfo = sinfo;
                    }
                    else
                    {
                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                        nodedesignatedata.createdate = "无";
                        var engineerEntity = outsouringengineerservice.GetEntity(entity.OUTENGINEERID);
                        var excutdept = departmentservice.GetEntity(engineerEntity.ENGINEERLETDEPTID).DepartmentId;
                        var createdept = departmentservice.GetEntityByCode(entity.CREATEUSERDEPTCODE).DepartmentId;
                        var outengineerdept = departmentservice.GetEntity(engineerEntity.OUTPROJECTID).DepartmentId;
                        var supervisordept = string.IsNullOrEmpty(engineerEntity.SupervisorId) ? "" : departmentservice.GetEntity(engineerEntity.SupervisorId).DepartmentId;
                        string useridstr = powerCheck.GetApproveUserId(dr["id"].ToString(), entity.TOOLSID, "", entity.SpecialtyType, excutdept, outengineerdept, createdept, "", "", supervisordept, entity.OUTENGINEERID); //获取审核人账号
                        DataTable dtuser = UserService.GetUserTable(useridstr.Split(','));
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
                setInfo endsinfo = new setInfo();
                if (entity.ISOVER=="1")
                {
                    endsinfo.Taged = 1;

                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                    NodeDesignateData nodedesignatedata = new NodeDesignateData();

                    if (nodeDt.Rows.Count > 0)
                    {
                        DataRow[] end_rows = nodeDt.Select("auditdept is not null").OrderBy(t => t.Field<DateTime>("audittime")).ToArray();
                        DateTime auditdate;
                        DateTime.TryParse(end_rows[end_rows.Count()-1]["audittime"].ToString(), out auditdate);
                        nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                        nodedesignatedata.creatdept = end_rows[end_rows.Count() - 1]["auditdept"].ToString();
                        nodedesignatedata.createuser = end_rows[end_rows.Count() - 1]["auditpeople"].ToString();
                        nodedesignatedata.status = end_rows[end_rows.Count() - 1]["auditresult"].ToString() == "0" ? "同意" : "不同意";
                        nodedesignatedata.prevnode = end_rows[end_rows.Count() - 1]["flowname"].ToString(); ;
                    }
                    else
                    {
                        nodedesignatedata.createdate = "";
                        nodedesignatedata.creatdept = "";
                        nodedesignatedata.createuser = "";
                        nodedesignatedata.status = "";
                        nodedesignatedata.prevnode = "";
                    }
                    nodelist.Add(nodedesignatedata);
                    endsinfo.NodeDesignateData = nodelist;
                    nodes_end.setInfo = endsinfo;
                }
                nlist.Add(nodes_end);
                
                #endregion

                #region 创建line对象

                for (int i = 0; i < nodeDt.Rows.Count; i++)
                {
                    lines lines = new lines();
                    lines.alt = true;
                    lines.id = Guid.NewGuid().ToString();
                    lines.from = nodeDt.Rows[i]["id"].ToString();
                    if (llist.Where(t => t.from == nodeDt.Rows[i]["id"].ToString()).Count() > 0)
                    {
                        llist.Remove(llist.Where(t => t.from == nodeDt.Rows[i]["id"].ToString()).FirstOrDefault());
                    }
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
                                                    a.id,a.flowname,a.serialnum,a.autoid,a.checkdeptname,a.checkroleid,a.checkrolename,a.checkdeptid,a.remark,b.AUDITDEPT,b.AUDITPEOPLE,b.AUDITTIME,b.AUDITRESULT,b.AUDITOPINION
                                              from 
                                                    bis_manypowercheck a left join epg_toolsaudit b
                                                    on a.id = b.flowid and b.toolsid = '{2}'
                                              where 
                                                    a.createuserorgcode = '{0}' and a.modulename = '{1}'
                                              order by
                                                    serialnum,autoid ", user.OrganizeCode, modulename, keyValue);
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
            this.BaseRepository().Delete(keyValue);
        }

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, string type, ToolsEntity entity)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var res = DbFactory.Base().BeginTrans();
            try
            {
                List<ProjecttoolsEntity> list = ProjecttoolsService.GetList(" and TOOLSID='" + keyValue + "'").ToList();
                List<string> zylist = new List<string>();
                string zystr = "";
                foreach (var item in list.Select(t => t.TOOLTYPE).ToArray().Distinct().ToList())
                {
                    zylist = zylist.AsEnumerable().Union(DataItemDetailService.GetDataItemByDetailValue("ToolEquipmentType", item).FirstOrDefault().Description.Split(',')).ToList();
                }
                zystr = string.Join(",", zylist.Distinct().ToList());
                entity.SpecialtyType = zystr;
                #region 保存
                if (type == "1")//保存
                {
                    entity.ISSAVED = "0"; //标记当前记录处于登记阶段
                    entity.ISOVER = "0"; //流程未完成，1表示完成
                    entity.TOOLSID = keyValue;
                    if (!string.IsNullOrEmpty(keyValue))
                    {
                        ToolsEntity se = this.BaseRepository().FindEntity(keyValue);
                        if (se == null)
                        {
                            entity.Create();
                            res.Insert<ToolsEntity>(entity);
                        }
                        else
                        {
                            entity.Modify(keyValue);
                            res.Update<ToolsEntity>(entity);
                        }
                    }
                    else
                    {
                        entity.Create();
                        res.Insert<ToolsEntity>(entity);
                    }
                }
                #endregion
                #region 提交
                if (type == "2")//提交
                {
                    string state = string.Empty;
                    string moduleName = entity.EQUIPTYPE == "2" ? "特种设备工器具" : "设备工器具";
                    string outengineerid = entity.OUTENGINEERID;
                    Repository<OutsouringengineerEntity> ourEngineer = new Repository<OutsouringengineerEntity>(DbFactory.Base());
                    OutsouringengineerEntity engineerEntity = ourEngineer.FindEntity(entity.OUTENGINEERID);
                    //currUser:当前登录人,state:是否有权限审核 1：能审核 0 ：不能审核,moduleName:模块名称,outengineerid:工程Id
                    ManyPowerCheckEntity mpcEntity = peopleReview.CheckAuditPower(curUser, out state, moduleName, outengineerid, false);
                    if (null != mpcEntity)
                    {
                        //保存设备工器具记录
                        entity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                        entity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                        entity.FLOWROLE = mpcEntity.CHECKROLEID;
                        entity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                        entity.ISSAVED = "1"; //标记已经从登记到审核阶段
                        entity.ISOVER = "0"; //流程未完成，1表示完成
                        entity.FlowId = mpcEntity.ID;
                        entity.FLOWNAME = mpcEntity.FLOWNAME;
                    }
                    else  //为空则表示已经完成流程
                    {
                        entity.FLOWDEPT = "";
                        entity.FLOWDEPTNAME = "";
                        entity.FLOWROLE = "";
                        entity.FLOWROLENAME = "";
                        entity.ISSAVED = "1"; //标记已经从登记到审核阶段
                        entity.ISOVER = "1"; //流程未完成，1表示完成
                        entity.FLOWNAME = "";
                        entity.FlowId = "";
                        #region 更新工程流程状态
                        Repository<StartappprocessstatusEntity> proecss = new Repository<StartappprocessstatusEntity>(DbFactory.Base());
                        StartappprocessstatusEntity startProecss = proecss.FindList(string.Format("select * from epg_startappprocessstatus t where t.outengineerid='{0}'", entity.OUTENGINEERID)).ToList().FirstOrDefault();
                        startProecss.EQUIPMENTTOOLSTATUS = "1";
                        res.Update<StartappprocessstatusEntity>(startProecss);
                        #endregion
                        #region 同步设备
                        if(entity.EQUIPTYPE == "2")
                            new SpecialEquipmentService().SyncSpecificTools(entity.OUTENGINEERID, entity.OUTPROJECTID, entity.TOOLSID);
                        #endregion
                    }
                    entity.TOOLSID = keyValue;
                    if (!string.IsNullOrEmpty(keyValue))
                    {
                        ToolsEntity se = this.BaseRepository().FindEntity(keyValue);
                        if (se == null)
                        {
                            entity.Create();
                            res.Insert<ToolsEntity>(entity);
                        }
                        else
                        {
                            entity.Modify(keyValue);
                            res.Update<ToolsEntity>(entity);
                        }
                    }
                    else
                    {
                        entity.Create();
                        res.Insert<ToolsEntity>(entity);
                    }
                }
                #endregion
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
