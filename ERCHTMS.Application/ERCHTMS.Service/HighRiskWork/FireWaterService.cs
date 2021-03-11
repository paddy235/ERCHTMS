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
    /// �� ����ʹ������ˮ
    /// </summary>
    public class FireWaterService : RepositoryFactory<FireWaterEntity>, FireWaterIService
    {
        ScaffoldauditrecordService scaffoldauditrecordservice = new ScaffoldauditrecordService();
        private DataItemDetailService dataitemdetailservice = new DataItemDetailService();
        private HighRiskCommonApplyService highriskcommonapplyservice = new HighRiskCommonApplyService();
        private DepartmentService departmentservice = new DepartmentService();
        private UserService userservice = new UserService();
        private ManyPowerCheckService manypowercheckservice = new ManyPowerCheckService();
        #region ��ȡ����
        /// <summary>
        /// �õ���ǰ�����
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
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public DataTable GetList(Pagination pagination, string queryJson, string authType, Operator user)
        {
            DatabaseType dataTye = DatabaseType.Oracle;
            #region ���
            pagination.p_kid = "a.id";
            pagination.p_fields = @"a.flowdeptname,a.flowname,a.flowrolename,a.flowdept,a.investigatestate,a.workdeptid,a.engineeringid,a.flowid,b.outtransferuseraccount,b.intransferuseraccount,
                                    a.workdepttype,a.workdeptname,a.applynumber,a.createdate,a.applystate,a.workplace,a.workcontent,a.workuserids,a.workareaname,a.workareacode,
                                    a.workstarttime,a.workendtime,a.applyusername,a.applydeptname,a.createuserid,a.createuserdeptcode,nvl(a.conditionstate,0) conditionstate,
                                    a.createuserorgcode,a.flowremark,a.specialtytype,'' as approveuserid,'' as approveusername,'' as approveuseraccount,case when a.applystate='3' and (a.conditionstate='0' or a.conditionstate is null) then '1' else '0' end appconditionstate";
                                                pagination.p_tablename = " bis_firewater a left join (select recid,flowid,outtransferuseraccount,intransferuseraccount,row_number()  over(partition by recid,flowid order by createdate desc) as num from BIS_TRANSFERRECORD where disable=0) b on a.id=b.recid and a.flowid=b.flowid and b.num=1";
            pagination.conditionJson = " 1=1 ";
            if (!string.IsNullOrEmpty(authType))
            {
                //���õĲ���ӵ������鿴Ȩ��
                string specialDeptId = new DataItemDetailService().GetItemValue(user.OrganizeId, "FireDept");
                var IsHrdl = new DataItemDetailService().GetItemValue("IsOpenPassword");
                string isAllDataRange = dataitemdetailservice.GetEnableItemValue("HighRiskWorkDataRange"); //�����ǣ��߷�����ҵģ���Ƿ�ȫ������
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
                        case "3"://���Ӳ���
                            pagination.conditionJson += string.Format(" and ((a.workdeptid in(select departmentid from base_department where encode like '{0}%'))  or (a.engineeringid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);
                            break;
                        case "4":
                            pagination.conditionJson += " and a.createuserorgcode='" + user.OrganizeCode + "'";
                            break;
                        case "app":
                            if (user.RoleName.Contains("��˾") || user.RoleName.Contains("����"))
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

            #region  ɸѡ����
            var queryParam = JObject.Parse(queryJson);
            //��ѯ����
            if (!queryParam["applynumber"].IsEmpty())//������
            {
                pagination.conditionJson += string.Format(" and applynumber like '%{0}%'", queryParam["applynumber"].ToString());
            }
            //��ѯ����
            if (!queryParam["status"].IsEmpty())//���״̬
            {
                pagination.conditionJson += string.Format(" and applystate='{0}'", queryParam["status"].ToString());
            }
            //ʱ��ѡ��
            if (!queryParam["st"].IsEmpty())//��ʼʱ��
            {
                string from = queryParam["st"].ToString().Trim();
                pagination.conditionJson += string.Format(" and workstarttime>=to_date('{0}','yyyy-mm-dd')", from);

            }
            if (!queryParam["et"].IsEmpty())//����ʱ��
            {
                string to = Convert.ToDateTime(queryParam["et"].ToString().Trim()).AddDays(1).ToString("yyyy-MM-dd");
                pagination.conditionJson += string.Format(" and workendtime<=to_date('{0}','yyyy-mm-dd')", to);
            }
            if (!queryParam["workdept"].IsEmpty() && !queryParam["workdeptid"].IsEmpty())//ʹ������ˮ��λ
            {
                pagination.conditionJson += string.Format(" and ((workdeptcode in(select encode from base_department where encode like '{0}%'))  or (engineeringid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", queryParam["workdept"].ToString(), queryParam["workdeptid"].ToString());
            }
            //��ѯ����
            if (!queryParam["workdeptcode"].IsEmpty())//ʹ������ˮ��λcode
            {
                pagination.conditionJson += string.Format(" and workdeptcode='{0}'", queryParam["workdeptcode"].ToString());
            }
            if (!queryParam["viewrange"].IsEmpty())
            {
                //����
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
                    //�ų����������뱣�������
                    pagination.conditionJson += string.Format("  and a.id  not in(select id from bis_firewater where applystate='0' and  applyuserid!='{0}')", user.UserId);
                }
            }
            #endregion
            DataTable data = this.BaseRepository().FindTableByProcPager(pagination, dataTye);
            for (int i = 0; i < data.Rows.Count; i++)
            {
                string executedept = string.Empty;
                highriskcommonapplyservice.GetExecutedept(data.Rows[i]["workdepttype"].ToString(), data.Rows[i]["workdeptid"].ToString(), data.Rows[i]["engineeringid"].ToString(), out executedept); //��ȡִ�в���
                string createdetpid = departmentservice.GetEntityByCode(data.Rows[i]["createuserdeptcode"].ToString()).IsEmpty() ? "" : departmentservice.GetEntityByCode(data.Rows[i]["createuserdeptcode"].ToString()).DepartmentId; //��ȡ��������
                string outsouringengineerdept = string.Empty;
                highriskcommonapplyservice.GetOutsouringengineerDept(data.Rows[i]["workdeptid"].ToString(), out outsouringengineerdept);
                string str = manypowercheckservice.GetApproveUserAccount(data.Rows[i]["flowid"].ToString(), data.Rows[i]["id"].ToString(), "", data.Rows[i]["specialtytype"].ToString(), executedept, outsouringengineerdept, createdetpid, "", "");
                string outtransferuseraccount = data.Rows[i]["outtransferuseraccount"].IsEmpty() ? "" : data.Rows[i]["outtransferuseraccount"].ToString();//ת��������
                string intransferuseraccount = data.Rows[i]["intransferuseraccount"].IsEmpty() ? "" : data.Rows[i]["intransferuseraccount"].ToString();//ת��������
                string[] outtransferuseraccountlist = outtransferuseraccount.Split(',');
                string[] intransferuseraccountlist = intransferuseraccount.Split(',');
                foreach (var item in intransferuseraccountlist)
                {
                    if (!item.IsEmpty() && !str.Contains(item + ","))
                    {
                        str += (item + ",");//��ת�������˼�������˺���
                    }
                }
                foreach (var item in outtransferuseraccountlist)
                {
                    if (!item.IsEmpty() && str.Contains(item + ","))
                    {
                        str = str.Replace(item + ",", "");//��ת�������˴�����˺����Ƴ�
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
        /// ��ȡִ�����
        /// </summary>
        /// <param name="fireWaterId"></param>
        /// <returns></returns>
        public FireWaterCondition GetConditionEntity(string fireWaterId) {
            Repository<FireWaterCondition> condition = new Repository<FireWaterCondition>(DbFactory.Base());
            return condition.FindEntity(x => x.FireWaterId == fireWaterId);
        }

        /// <summary>
        /// ��ȡִ���������
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public IList<FireWaterCondition> GetConditionList(string keyValue)
        {
            Repository<FireWaterCondition> condition = new Repository<FireWaterCondition>(DbFactory.Base());
            return condition.IQueryable().Where(t => t.FireWaterId == keyValue).ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public FireWaterEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }


        #region ����ͼ
        /// <summary>
        /// �õ�����ͼ
        /// </summary>
        /// <param name="keyValue">ҵ���ID</param>
        /// <param name="modulename">�����ģ����</param>
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
                #region ����node����

                for (int i = 0; i < nodeDt.Rows.Count; i++)
                {
                    DataRow dr = nodeDt.Rows[i];
                    nodes nodes = new nodes();
                    nodes.alt = true;
                    nodes.isclick = false;
                    nodes.css = "";
                    nodes.id = dr["id"].ToString(); //����
                    nodes.img = "";
                    nodes.name = dr["flowname"].ToString();
                    nodes.type = "stepnode";
                    nodes.width = 150;
                    nodes.height = 60;
                    //λ��
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
                    //��˼�¼
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
                        nodedesignatedata.status = dr["auditstate"].ToString() == "1" ? "ͬ��" : "��ͬ��";
                        if (i == 0)
                        {
                            nodedesignatedata.prevnode = "��";
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
                        nodedesignatedata.createdate = "��";
                        string executedept = string.Empty;
                        highriskcommonapplyservice.GetExecutedept(entity.WorkDeptType.ToString(), entity.WorkDeptId, entity.EngineeringId, out executedept);//��ȡִ�в���
                        string createdetpid = departmentservice.GetEntityByCode(entity.CreateUserDeptCode).IsEmpty() ? "" : departmentservice.GetEntityByCode(entity.CreateUserDeptCode).DepartmentId; //��ȡ��������ID
                        string outsouringengineerdept = string.Empty;
                        highriskcommonapplyservice.GetOutsouringengineerDept(entity.WorkDeptId, out outsouringengineerdept);
                        string accountstr = manypowercheckservice.GetApproveUserAccount(dr["id"].ToString(), entity.Id, "", entity.SpecialtyType, executedept, outsouringengineerdept, createdetpid, "", ""); //��ȡ������˺�
                        string outtransferuseraccount = dr["outtransferuseraccount"].IsEmpty() ? "" : dr["outtransferuseraccount"].ToString();//ת��������
                        string intransferuseraccount = dr["intransferuseraccount"].IsEmpty() ? "" : dr["intransferuseraccount"].ToString();//ת��������
                        string[] outtransferuseraccountlist = outtransferuseraccount.Split(',');
                        string[] intransferuseraccountlist = intransferuseraccount.Split(',');
                        foreach (var item in intransferuseraccountlist)
                        {
                            if (!item.IsEmpty() && !accountstr.Contains(item + ","))
                            {
                                accountstr += (item + ",");//��ת�������˼�������˺���
                            }
                        }
                        foreach (var item in outtransferuseraccountlist)
                        {
                            if (!item.IsEmpty() && accountstr.Contains(item + ","))
                            {
                                accountstr = accountstr.Replace(item + ",", "");//��ת�������˴�����˺����Ƴ�
                            }
                        }

                        DataTable dtuser = userservice.GetUserTable(accountstr.Split(','));
                        string[] usernames = dtuser.AsEnumerable().Select(d => d.Field<string>("realname")).ToArray();
                        string[] deptnames = dtuser.AsEnumerable().Select(d => d.Field<string>("deptname")).ToArray().GroupBy(t => t).Select(p => p.Key).ToArray();
                        nodedesignatedata.createuser = usernames.Length > 0 ? string.Join(",", usernames) : "��";
                        nodedesignatedata.creatdept = deptnames.Length > 0 ? string.Join(",", deptnames) : "��";

                        nodedesignatedata.status = "��";
                        if (i == 0)
                        {
                            nodedesignatedata.prevnode = "��";
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

                //���̽����ڵ�
                nodes nodes_end = new nodes();
                nodes_end.alt = true;
                nodes_end.isclick = false;
                nodes_end.css = "";
                nodes_end.id = Guid.NewGuid().ToString();
                nodes_end.img = "";
                nodes_end.name = "���̽���";
                nodes_end.type = "endround";
                nodes_end.width = 150;
                nodes_end.height = 60;
                //ȡ���һ���̵�λ�ã������λ
                nodes_end.left = nlist[nlist.Count - 1].left;
                nodes_end.top = nlist[nlist.Count - 1].top + 100;
                nlist.Add(nodes_end);

                //���״̬Ϊ���ͨ����ͨ�������̽������б�ʶ 
                if (entity.InvestigateState == "3")
                {
                    setInfo sinfo = new setInfo();
                    sinfo.NodeName = nodes_end.name;
                    sinfo.Taged = 1;
                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                    NodeDesignateData nodedesignatedata = new NodeDesignateData();

                    //ȡ���̽���ʱ�Ľڵ���Ϣ
                    DataRow[] end_rows = nodeDt.Select("auditusername is not null");
                    DataRow end_row = end_rows[end_rows.Count() - 1];
                    DateTime auditdate;
                    DateTime.TryParse(end_row["auditdate"].ToString(), out auditdate);
                    nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                    nodedesignatedata.creatdept = end_row["auditdeptname"].ToString();
                    nodedesignatedata.createuser = end_row["auditusername"].ToString();
                    nodedesignatedata.status = end_row["auditstate"].ToString() == "1" ? "ͬ��" : "��ͬ��";
                    nodedesignatedata.prevnode = end_row["flowname"].ToString();

                    nodelist.Add(nodedesignatedata);
                    sinfo.NodeDesignateData = nodelist;
                    nodes_end.setInfo = sinfo;
                }

                #endregion

                #region ����line����

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
        /// ��ȡAPP����ͼ
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
                    //��˼�¼
                    if (dr["auditdeptname"] != null && !string.IsNullOrEmpty(dr["auditdeptname"].ToString()))
                    {

                        CheckFlowData checkdata = new CheckFlowData();
                        DateTime auditdate;
                        DateTime.TryParse(dr["auditdate"].ToString(), out auditdate);
                        checkdata.auditdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                        checkdata.auditdeptname = dr["auditdeptname"].ToString();
                        checkdata.auditusername = dr["auditusername"].ToString();
                        checkdata.auditstate = dr["auditstate"].ToString() == "1" ? "ͬ��" : "��ͬ��";
                        checkdata.auditremark = dr["auditremark"].ToString() != "ȷ��step" ? dr["auditremark"].ToString() : "";
                        checkdata.isapprove = "1";
                        checkdata.isoperate = "0";
                        nodelist.Add(checkdata);
                    }
                    else
                    {
                        CheckFlowData checkdata = new CheckFlowData();
                        checkdata.auditdate = "";
                        string executedept = string.Empty;
                        highriskcommonapplyservice.GetExecutedept(entity.WorkDeptType.ToString(), entity.WorkDeptId, entity.EngineeringId, out executedept);//��ȡִ�в���
                        string createdetpid = departmentservice.GetEntityByCode(entity.CreateUserDeptCode).IsEmpty() ? "" : departmentservice.GetEntityByCode(entity.CreateUserDeptCode).DepartmentId; //��ȡ��������ID
                        string outsouringengineerdept = string.Empty;
                        highriskcommonapplyservice.GetOutsouringengineerDept(entity.WorkDeptId, out outsouringengineerdept);
                        string accountstr = manypowercheckservice.GetApproveUserAccount(dr["id"].ToString(), entity.Id, "", entity.SpecialtyType, executedept, outsouringengineerdept, createdetpid, "", ""); //��ȡ������˺�
                        string outtransferuseraccount = dr["outtransferuseraccount"].IsEmpty() ? "" : dr["outtransferuseraccount"].ToString();//ת��������
                        string intransferuseraccount = dr["intransferuseraccount"].IsEmpty() ? "" : dr["intransferuseraccount"].ToString();//ת��������
                        string[] outtransferuseraccountlist = outtransferuseraccount.Split(',');
                        string[] intransferuseraccountlist = intransferuseraccount.Split(',');
                        foreach (var item in intransferuseraccountlist)
                        {
                            if (!item.IsEmpty() && !accountstr.Contains(item + ","))
                            {
                                accountstr += (item + ",");//��ת�������˼�������˺���
                            }
                        }
                        foreach (var item in outtransferuseraccountlist)
                        {
                            if (!item.IsEmpty() && accountstr.Contains(item + ","))
                            {
                                accountstr = accountstr.Replace(item + ",", "");//��ת�������˴�����˺����Ƴ�
                            }
                        }

                        DataTable dtuser = userservice.GetUserTable(accountstr.Split(','));
                        string[] usernames = dtuser.AsEnumerable().Select(d => d.Field<string>("realname")).ToArray();
                        string[] deptnames = dtuser.AsEnumerable().Select(d => d.Field<string>("deptname")).ToArray().GroupBy(t => t).Select(p => p.Key).ToArray();
                        checkdata.auditdeptname = deptnames.Length > 0 ? string.Join(",", deptnames) : "��";
                        checkdata.auditusername = usernames.Length > 0 ? string.Join(",", usernames) : "��";
                        checkdata.auditremark = "";
                        checkdata.isapprove = "0";
                        if (entity.InvestigateState == "3")
                            checkdata.isoperate = "0";
                        else
                            checkdata.isoperate = dr["id"].ToString() == entity.FlowId ? "1" : "0";
                        if (checkdata.isoperate == "1")
                        {
                            if (dr["flowname"].ToString().Contains("ȷ��"))
                            {
                                checkdata.auditstate = "ȷ����";
                            }
                            else
                            {
                                checkdata.auditstate = "���(��)��";
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

        #region ��ȡʹ������ˮ̨��
        /// <summary>
        /// ��ȡʹ������ˮ̨��
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetLedgerList(Pagination pagination, string queryJson, Operator user)
        {
            DatabaseType dataType = DatabaseType.Oracle;
            #region ����Ȩ��
            string curUserId = user.UserId;
            #endregion
            /*
             ������ҵ:����ͨ����ʵ����ҵʱ��Ϊ��
             ��ҵ��:����ͨ����ʵ����ҵ��ʼʱ�䲻Ϊ����ʵ����ҵ����ʱ��Ϊ��
             �ѽ���:����ͨ����ʵ����ҵ����ʱ�䲻Ϊ��
             */
            #region ���
            pagination.p_kid = "Id as workid";
            pagination.p_fields = "case when workdepttype=0 then '��λ�ڲ�' when workdepttype=1 then '�����λ' end workdepttypename,workdepttype,workdeptid,workdeptname,workdeptcode,applynumber,a.createdate,workplace,workcontent,workstarttime,workendtime,applyusername,engineeringname,engineeringid,case when a.workoperate='1' then '��ҵ��ͣ' when realityworkstarttime is not null and realityworkendtime is null then '��ҵ��' when realityworkendtime is not null then '�ѽ���'  else '������ҵ' end ledgertype,a.workuserids,a.RealityWorkStartTime,a.RealityWorkEndTime,'' as isoperate,a.createuserid";
            pagination.p_tablename = " bis_firewater a";
            pagination.conditionJson = "applystate='3'";
            if (!user.IsSystem)
            {
                 //���õĲ���ӵ������鿴Ȩ��
                string specialDeptId = new DataItemDetailService().GetItemValue(user.OrganizeId, "FireDept");
                if (!string.IsNullOrEmpty(specialDeptId) && specialDeptId.Contains(user.DeptId))
                {
                    pagination.conditionJson += " and a.createuserorgcode='" + user.OrganizeCode + "'";
                }
                else
                {
                    if (user.RoleName.Contains("��˾") || user.RoleName.Contains("����"))
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

            #region  ɸѡ����
            var queryParam = JObject.Parse(queryJson);
            //ʱ��ѡ��
            if (!queryParam["st"].IsEmpty())//ʹ������ˮ��ʼʱ��
            {
                string from = queryParam["st"].ToString().Trim();
                pagination.conditionJson += string.Format(" and WorkStartTime>=to_date('{0}','yyyy-mm-dd')", from);

            }
            if (!queryParam["et"].IsEmpty())//ʹ������ˮ����ʱ��
            {
                string to = Convert.ToDateTime(queryParam["et"].ToString().Trim()).AddDays(1).ToString("yyyy-MM-dd");
                pagination.conditionJson += string.Format(" and WorkEndTime<=to_date('{0}','yyyy-mm-dd')", to);
            }
            //��ѯ����
            if (!queryParam["workdeptcode"].IsEmpty())//ʹ������ˮ��λcode
            {
                pagination.conditionJson += string.Format(" and workdeptcode='{0}'", queryParam["workdeptcode"].ToString());
            }
            if (!queryParam["ledgertype"].IsEmpty())
            {
                var ledgertype = queryParam["ledgertype"].ToString();
                if (ledgertype == "0")// ������ҵ
                {
                    pagination.conditionJson += " and RealityWorkStartTime is null";
                }
                else if (ledgertype == "1")//��ҵ��
                {
                    pagination.conditionJson += " and RealityWorkStartTime is not null and RealityWorkEndTime is null";
                }
                else if (ledgertype == "3")
                {
                    pagination.conditionJson += " and workoperate='1'";
                }
                else//�ѽ���
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
            #region ����Ȩ��
            if (data != null)
            {
                string strRole = dataitemdetailservice.GetItemValue(user.OrganizeId, "LedgerSendDept");//���β��Ž�ɫ
                string strManageRole = dataitemdetailservice.GetItemValue(user.OrganizeId, "LedgerManageDept");//��ȫ���ܲ��ż�ܽ�ɫ
                string strWorkRole = dataitemdetailservice.GetItemValue(user.OrganizeId, "LedgerWorkDept");//��ҵ��λ
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    string str = "0";
                    string workUserIds = data.Rows[i]["workuserids"].ToString();//��ҵ��Ա
                    string applyUserId = data.Rows[i]["createuserid"].ToString();
                    string engineeringid = data.Rows[i]["engineeringid"].ToString();//����id
                    string workDeptType = data.Rows[i]["workdepttype"].ToString();
                    string workdeptid = data.Rows[i]["workdeptid"].ToString();//��ҵ��λid
                    var dept = new OutsouringengineerService().GetEntity(engineeringid); //��ȡ����id��Ӧ�����β���
                    if (user.RoleName.Contains("����") && !string.IsNullOrEmpty(strManageRole))//��ȫ���ܲ���
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
                    if (str != "1" && ((!string.IsNullOrEmpty(workUserIds) && workUserIds.Contains(curUserId)) || curUserId == applyUserId))//��ҵ��Ա��������
                    {
                        str = "1";
                    }
                    if (str != "1" && dept != null)
                    {
                        if (workDeptType == "1")//���β���
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

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
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
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
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
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="model">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, FireWaterModel model)
        {
            var res = DbFactory.Base().BeginTrans();
            try
            {

                Repository<FireWaterEntity> repFire = new Repository<FireWaterEntity>(DbFactory.Base());
                FireWaterEntity entity = repFire.FindEntity(keyValue);
                //����
                if (entity == null)
                {
                    entity = new FireWaterEntity();
                    entity.Id = keyValue;
                    entity.Create();
                    //ʵ�帳ֵ
                    this.copyProperties(entity, model);
                    //���ɱ���
                    entity.ApplyNumber = this.GetMaxCode();
                    //��Ӳ���
                    res.Insert(entity);
                }
                else
                {
                    //�༭ 
                    entity.Modify(keyValue);
                    //ʵ�帳ֵ
                    this.copyProperties(entity, model);
                    //���²���
                    res.Update(entity);
                }
                //��ӻ������ҵ��ȫ���� ��ɾ�������
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
        /// ����ҵ�����˱�
        /// </summary>
        /// <param name="firewaterentity">ҵ������ʵ��</param>
        /// <param name="auditEntity">��˱�ʵ��</param>
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
        /// ��Դʵ���Ŀ��ʵ�����Ը�ֵ
        /// </summary>
        /// <param name="target">Ŀ��</param>
        /// <param name="source">Դ</param>
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

            //���������Ϣ
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
        /// �ύִ�����
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
