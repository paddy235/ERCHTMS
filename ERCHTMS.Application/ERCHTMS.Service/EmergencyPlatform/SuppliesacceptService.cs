using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.IService.EmergencyPlatform;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using BSFramework.Util;
using BSFramework.Data;
using ERCHTMS.Code;
using BSFramework.Util.Extension;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Entity.BaseManage;
using System;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Entity.HiddenTroubleManage;
using Newtonsoft.Json;

namespace ERCHTMS.Service.EmergencyPlatform
{
    /// <summary>
    /// 描 述：应急物资领用申请
    /// </summary>
    public class SuppliesacceptService : RepositoryFactory<SuppliesacceptEntity>, SuppliesacceptIService
    {
        private ManyPowerCheckService manypowercheckservice = new ManyPowerCheckService();
        private DepartmentService departmentservice = new DepartmentService();
        private UserService userservice = new UserService();
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<SuppliesacceptEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public SuppliesacceptEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
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
            pagination.p_kid = "a.id";
            pagination.p_fields = string.IsNullOrWhiteSpace(pagination.p_fields) ? @"a.createuserid,a.createuserdeptcode,a.createuserorgcode,a.issubmit,a.status,c.suppliesname,a.applyperson,a.applydept,to_char(a.applydate,'yyyy-MM-dd') as applydate,a.flowid,'' as approveuserids" : pagination.p_fields;
            pagination.p_tablename = @"MAE_SUPPLIESACCEPT a left join ( select wm_concat(b.suppliesname) as suppliesname,recid from MAE_SUPPLIESACCEPT_DETAIL b group by recid) c on a.id=c.recid";
            if (pagination.sidx == null)
            {
                pagination.sidx = "a.createdate";
            }
            if (pagination.sord == null)
            {
                pagination.sord = "desc";
            }
            #endregion

            //申请人
            if (!queryParam["applyperson"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and a.applyperson like '{0}%' ", queryParam["applyperson"].ToString());
            }
            //物质名称
            if (!queryParam["suppliesname"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and c.suppliesname like '%{0}%' ", queryParam["suppliesname"].ToString());
            }
            //待办
            if (!queryParam["dbsx"].IsEmpty())
            {
                var dbsx = queryParam["dbsx"].ToString();
                switch (dbsx)
                {
                    case "0":
                        pagination.conditionJson += " and a.status=1";
                        DataTable dt = BaseRepository().FindTable("select " + pagination.p_kid + "," + pagination.p_fields + " from " + pagination.p_tablename + " where " + pagination.conditionJson);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string createdetpid = departmentservice.GetEntityByCode(dt.Rows[i]["createuserdeptcode"].ToString()).IsEmpty() ? "" : departmentservice.GetEntityByCode(dt.Rows[i]["createuserdeptcode"].ToString()).DepartmentId;
                            //获取下一步审核人
                            string str = manypowercheckservice.GetApproveUserId(dt.Rows[i]["flowid"].ToString(), dt.Rows[i]["id"].ToString(), "", "", "", "", createdetpid, "", "", "", "");
                            dt.Rows[i]["approveuserids"] = str;
                        }
                        string[] applyids = dt.Select(" approveuserids like '%" + user.UserId + "%'").AsEnumerable().Select(d => d.Field<string>("id")).ToArray();

                        pagination.conditionJson += string.Format(" and a.id in ('{0}') ", string.Join("','", applyids));
                        break;
                    default:
                        break;
                }
            }
            //作业单位
            if (!queryParam["code"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and a.applydeptcode like '{0}%'", queryParam["code"].ToString());
            }

            DataTable data = this.BaseRepository().FindTableByProcPager(pagination, dataType);

            if (data != null && data.Rows.Count > 0)
            {
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    string createdetpid = departmentservice.GetEntityByCode(data.Rows[i]["createuserdeptcode"].ToString()).IsEmpty() ? "" : departmentservice.GetEntityByCode(data.Rows[i]["createuserdeptcode"].ToString()).DepartmentId;
                    //获取下一步审核人
                    string str = manypowercheckservice.GetApproveUserId(data.Rows[i]["flowid"].ToString(), data.Rows[i]["id"].ToString(), "", "", "", "", createdetpid, "", "", "", "");
                    data.Rows[i]["approveuserids"] = str;
                }
            }
            return data;
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
            DataTable nodeDt = GetAuditInfo(keyValue, modulename);
            SuppliesacceptEntity entity = GetEntity(keyValue);
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
                    if (dr["auditdept"] != null && !string.IsNullOrEmpty(dr["auditdept"].ToString()))
                    {
                        sinfo.Taged = 1;
                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                        DateTime auditdate;
                        DateTime.TryParse(dr["audittime"].ToString(), out auditdate);
                        nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                        nodedesignatedata.creatdept = dr["auditdept"].ToString();
                        nodedesignatedata.createuser = dr["auditpeople"].ToString();
                        nodedesignatedata.status = dr["auditresult"].ToString() == "0" ? "同意" : "不同意";
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
                        string createdetpid = departmentservice.GetEntityByCode(entity.CreateUserDeptCode).IsEmpty() ? "" : departmentservice.GetEntityByCode(entity.CreateUserDeptCode).DepartmentId;
                        string accountstr = manypowercheckservice.GetApproveUserAccount(dr["id"].ToString(), entity.Id, "", "", "", "", createdetpid, "", ""); 

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
                if (entity.Status == 2 || entity.Status == 3)
                {
                    setInfo sinfo = new setInfo();
                    sinfo.NodeName = nodes_end.name;
                    sinfo.Taged = 1;
                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                    NodeDesignateData nodedesignatedata = new NodeDesignateData();

                    //取流程结束时的节点信息
                    DataRow[] end_rows = nodeDt.Select("auditpeople is not null").OrderBy(t => t.Field<double>("serialnum")).ToArray();
                    if (end_rows.Count() > 0)
                    {
                        DataRow end_row = end_rows[end_rows.Count() - 1];
                        DateTime auditdate;
                        DateTime.TryParse(end_row["audittime"].ToString(), out auditdate);
                        nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                        nodedesignatedata.creatdept = end_row["auditdept"].ToString();
                        nodedesignatedata.createuser = end_row["auditpeople"].ToString();
                        nodedesignatedata.status = end_row["auditresult"].ToString() == "0" ? "同意" : "不同意";
                        nodedesignatedata.prevnode = end_row["flowname"].ToString();
                    }
                    else
                    {
                        nodedesignatedata.createdate = "无";
                        nodedesignatedata.creatdept = "无";
                        nodedesignatedata.createuser = "无";
                        nodedesignatedata.status = "无";
                        nodedesignatedata.prevnode = "无";
                    }
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
        /// 获取流程图审核信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="modulename"></param>
        /// <returns></returns>
        public DataTable GetAuditInfo(string keyValue, string modulename)
        {
            Operator user = OperatorProvider.Provider.Current();
            string node_sql = string.Format(@"select 
                                                    a.id,a.flowname,a.serialnum,a.autoid,a.checkdeptname,a.checkroleid,a.checkrolename,a.checkdeptid,a.remark,b.auditdept,b.auditpeople,b.audittime,b.auditresult
                                              from 
                                                    bis_manypowercheck a left join epg_aptitudeinvestigateaudit b
                                                    on a.id = b.flowid and b.aptitudeid = '{2}' and (b.disable is null or b.disable !=1)
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
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public string SaveForm(string keyValue, SuppliesacceptEntity entity)
        {
            string message = "";
            ManyPowerCheckEntity mpcEntity = new ManyPowerCheckEntity();
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            entity.Status = entity.IsSubmit;
            var res = DbFactory.Base().BeginTrans();
            if (!string.IsNullOrEmpty(keyValue))
            {
                var result = GetEntity(keyValue);
                if (result == null)
                {
                    entity.Create();
                }
                else
                {
                    entity.Modify(keyValue);
                }

            }
            else
            {
                entity.Create();
            }
            res.Delete<SuppliesAcceptDetailEntity>(t => t.RecId == entity.Id);
            var num = 0;
            if (!entity.DetailData.IsEmpty())
            {
                foreach (var spec in entity.DetailData)
                {
                    spec.CreateDate = DateTime.Now.AddSeconds(-num);
                    spec.Create();
                    spec.RecId = entity.Id;
                    res.Insert(spec);
                    num++;
                }
            }
            try
            {
                if (entity.IsSubmit == 1)
                {
                    mpcEntity = manypowercheckservice.CheckAuditForNext(curUser, "应急物资领用审批", "", curUser.DeptId, "", keyValue);
                    if (null != mpcEntity)
                    {
                        entity.FlowId = mpcEntity.ID;
                    }
                    else
                    {
                        //审核通过时更新库存，添加库存变更记录
                        Boolean yz = true; //判断是否库存数量满足申领条件
                        SuppliesService sService = new SuppliesService();
                        foreach (var item in entity.DetailData)
                        {
                            var supplies = sService.GetEntity(item.SuppliesId);
                            if (item.AcceptNum <= supplies.NUM)
                            {
                                supplies.NUM = supplies.NUM - item.AcceptNum;
                                res.Update<SuppliesEntity>(supplies);
                                var entityInorOut = new InoroutrecordEntity
                                {
                                    USERID = supplies.USERID,
                                    USERNAME = supplies.USERNAME,
                                    DEPARTID = supplies.DEPARTID,
                                    DEPARTNAME = supplies.DEPARTNAME,
                                    INOROUTTIME = DateTime.Now,
                                    SUPPLIESCODE = supplies.SUPPLIESCODE,
                                    SUPPLIESTYPE = supplies.SUPPLIESTYPE,
                                    SUPPLIESTYPENAME = supplies.SUPPLIESTYPENAME,
                                    SUPPLIESNAME = supplies.SUPPLIESNAME,
                                    SUPPLIESUNTIL = supplies.SUPPLIESUNTIL,
                                    SUPPLIESUNTILNAME = supplies.SUPPLIESUNTILNAME,
                                    NUM = item.AcceptNum,
                                    STORAGEPLACE = supplies.STORAGEPLACE,
                                    MOBILE = supplies.MOBILE,
                                    SUPPLIESID = supplies.ID,
                                    STATUS = 1
                                };
                                entityInorOut.Create();
                                res.Insert<InoroutrecordEntity>(entityInorOut);
                            }
                            else
                            {
                                message += supplies.SUPPLIESNAME + "库存不足，无法申领，实际库存为" + supplies.NUM + "；";
                                yz = false;
                            }
                        }
                        if (yz == false)
                        {
                            res.Rollback();
                            return message;
                        }
                        entity.FlowId = "";
                        entity.Status = 3;
                    }
                }
                entity.DetailData = null;
                if (!string.IsNullOrEmpty(keyValue))
                {
                    var result = GetEntity(keyValue);
                    if (result == null)
                    {
                        res.Insert(entity);
                    }
                    else
                    {
                        res.Update(entity);
                    }

                }
                else
                {
                    res.Insert(entity);
                }
                res.Commit();
                return message;
            }
            catch (System.Exception ex)
            {
                res.Rollback();
                throw ex;
            }
            
        }

        /// <summary>
        /// 审核表单
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="aentity"></param>
        /// <param name="DetailData"></param>
        public string AuditForm(string keyValue, AptitudeinvestigateauditEntity aentity,string DetailData)
        {
            string message = "";
            ManyPowerCheckEntity mpcEntity = new ManyPowerCheckEntity();
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            SuppliesacceptEntity entity = GetEntity(keyValue);
            var res = DbFactory.Base().BeginTrans();
            try
            {
                aentity.APTITUDEID = keyValue;
                aentity.ID = Guid.NewGuid().ToString();
                aentity.FlowId = entity.FlowId;
                res.Insert<AptitudeinvestigateauditEntity>(aentity);
                if (aentity.AUDITRESULT == "0")
                {
                    var createdept = departmentservice.GetEntityByCode(entity.CreateUserDeptCode);
                    mpcEntity = manypowercheckservice.CheckAuditForNext(curUser, "应急物资领用审批", "", createdept.DepartmentId, "", keyValue);
                    if (null != mpcEntity)
                    {
                        entity.FlowId = mpcEntity.ID;
                    }
                    else
                    {
                        List<SuppliesAcceptDetailEntity> detail = JsonConvert.DeserializeObject<List<SuppliesAcceptDetailEntity>>(DetailData);
                        Boolean yz = true;
                        SuppliesService sService = new SuppliesService();
                        foreach (var item in detail)
                        {
                            var supplies = sService.GetEntity(item.SuppliesId);
                            if (item.AcceptNum <= supplies.NUM)
                            {
                                res.Update<SuppliesAcceptDetailEntity>(item);
                                supplies.NUM = supplies.NUM - item.AcceptNum;
                                res.Update<SuppliesEntity>(supplies);
                                var entityInorOut = new InoroutrecordEntity
                                {
                                    USERID = supplies.USERID,
                                    USERNAME = supplies.USERNAME,
                                    DEPARTID = supplies.DEPARTID,
                                    DEPARTNAME = supplies.DEPARTNAME,
                                    INOROUTTIME = DateTime.Now,
                                    SUPPLIESCODE = supplies.SUPPLIESCODE,
                                    SUPPLIESTYPE = supplies.SUPPLIESTYPE,
                                    SUPPLIESTYPENAME = supplies.SUPPLIESTYPENAME,
                                    SUPPLIESNAME = supplies.SUPPLIESNAME,
                                    SUPPLIESUNTIL = supplies.SUPPLIESUNTIL,
                                    SUPPLIESUNTILNAME = supplies.SUPPLIESUNTILNAME,
                                    NUM = item.AcceptNum,
                                    STORAGEPLACE = supplies.STORAGEPLACE,
                                    MOBILE = supplies.MOBILE,
                                    SUPPLIESID = supplies.ID,
                                    STATUS = 1
                                };
                                entityInorOut.Create();
                                res.Insert<InoroutrecordEntity>(entityInorOut);
                            }
                            else
                            {
                                message += supplies.SUPPLIESNAME + "库存不足，无法申领，实际库存为" + supplies.NUM + "；";
                                yz = false;
                            }
                        }
                        if (yz == false)
                        {
                            res.Rollback();
                            return message;
                        }
                        entity.FlowId = "-100";
                        entity.Status = 3;
                    }
                }
                else
                {
                    entity.FlowId = "-100";
                    entity.Status = 2;
                }
                //entity.IsLastAudit = null;
                entity.DetailData = null;
                res.Update(entity);
                res.Commit();
                return message;
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
