using ERCHTMS.Entity.PowerPlantInside;
using ERCHTMS.IService.PowerPlantInside;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Data;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Code;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;

namespace ERCHTMS.Service.PowerPlantInside
{
    /// <summary>
    /// �� �����¹��¼�����
    /// </summary>
    public class PowerplanthandleService : RepositoryFactory<PowerplanthandleEntity>, PowerplanthandleIService
    {
        private IManyPowerCheckService powerCheck = new ManyPowerCheckService();
        private PowerplanthandledetailService powerplanthandledetailservice = new PowerplanthandledetailService();
        private DepartmentService DepartmentService = new DepartmentService();
        private PowerplantreformService PowerplantreformService = new PowerplantreformService();
        private PowerplantcheckService PowerplantcheckService = new PowerplantcheckService();
        private UserService UserService = new UserService();
        private TransferrecordService TransferrecordService = new TransferrecordService();
        #region ��ȡ����

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;

            if (queryJson.Length > 0)
            {
                var queryParam = queryJson.ToJObject();
                //��ѯ����
                if (!queryParam["sgtype"].IsEmpty())
                {
                    string sgtype = queryParam["sgtype"].ToString();
                    pagination.conditionJson += string.Format(" and accidenteventtype = '{0}'", sgtype);
                }
                if (!queryParam["sgproperty"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and a.accidenteventproperty='{0}'", queryParam["sgproperty"].ToString());
                }
                if (!queryParam["keyword"].IsEmpty())
                {
                    string sgtypename = queryParam["keyword"].ToString();
                    pagination.conditionJson += string.Format(" and accidenteventname like '%{0}%'", sgtypename);
                }
                if (!queryParam["happentimestart"].IsEmpty())
                {
                    string happentimestart = queryParam["happentimestart"].ToString();
                    pagination.conditionJson += string.Format(" and happentime >= (select  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') from dual)", happentimestart);
                }
                if (!queryParam["happentimeend"].IsEmpty())
                {
                    string happentimeend = queryParam["happentimeend"].ToString();
                    if (happentimeend.Length == 10)
                        happentimeend = Convert.ToDateTime(happentimeend).AddDays(1).ToString();
                    pagination.conditionJson += string.Format(" and happentime <= (select  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') from dual)", happentimeend);
                }

                if (!queryParam["applystate"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and applystate='{0}'", queryParam["applystate"].ToString());
                }
                //��������
                if (!queryParam["mode"].IsEmpty())
                {
                    string mode = queryParam["mode"].ToString();
                    if (mode == "dbsx")
                    {
                        Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                        var roleArr = user.RoleName.Split(','); //��ǰ��Ա��ɫ
                        string strRole = "";
                        foreach (var rolename in roleArr)
                        {
                            strRole += string.Format(" flowrolename like '%{0}%' or ", rolename);
                        }

                        if (strRole.Length > 2)
                        {
                            strRole = strRole.Substring(0, strRole.Length - 3);
                        }

                        string strsql = string.Format(" and createuserorgcode = '{0}' and ISSAVED=1  and applystate= 1  and flowdept = '{1}' and ({2})", user.OrganizeCode, user.DeptId, strRole);

                        pagination.conditionJson += strsql;
                    }

                }
            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<PowerplanthandleEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public PowerplanthandleEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// �¹��¼��������
        /// </summary>
        /// <returns></returns>
        public List<string> ToAuditPowerHandle()
        {
            List<string> toAuditNum = new List<string>();
            Operator currUser = OperatorProvider.Provider.Current();
            string role = currUser.RoleName;
            string deptId = string.Empty;
            string deptName = string.Empty;

            //��˾���û�ȡ��������
            if (role.Contains("��˾���û�"))
            {
                deptId = currUser.OrganizeId;  //����ID
                deptName = currUser.OrganizeName;//��������
            }
            else
            {
                deptId = currUser.DeptId; //����ID
                deptName = currUser.DeptName; //����ID
            }
            string sql = string.Empty;
            string[] arrRole = role.Split(',');

            string strWhere = string.Empty;
            #region ������¹��¼�����
            sql = string.Empty;
            sql = string.Format("select count(id) unitnum from BIS_POWERPLANTHANDLE s ");

            strWhere = string.Empty;
            strWhere += string.Format(" WHERE createuserorgcode = '{0}' and ISSAVED=1  and applystate= 1  and flowdept = '{1}' ", currUser.OrganizeCode, currUser.DeptId);
            strWhere += " and (";
            foreach (string str in arrRole)
            {
                strWhere += string.Format(" flowrolename like '%{0}%' or", str);
            }
            strWhere = strWhere.Substring(0, strWhere.Length - 2);
            strWhere += " )";
            sql = sql + strWhere;
            var dt = this.BaseRepository().FindTable(sql);
            if (dt.Rows.Count > 0)
            {
                toAuditNum.Add(dt.Rows[0]["unitnum"].ToString());
            }
            else
            {
                toAuditNum.Add("0");
            }
            #endregion
            #region �������¹��¼�
            sql = string.Empty;
            sql = string.Format("select count(id) unitnum from bis_powerplanthandledetail s left join (select recid,flowid,outtransferuseraccount,intransferuseraccount,outtransferusername,intransferusername,row_number() over(partition by recid,flowid order by createdate desc) as num from BIS_TRANSFERRECORD where disable=0) e on s.id=e.recid and e.num=1");

            strWhere = string.Empty;
            strWhere += string.Format(" where s.applystate=3 and (s.rectificationdutypersonid like '%{0}%' or e.intransferuseraccount like '%{1}%') and (e.outtransferuseraccount is null or e.outtransferuseraccount not like '%{1}%')", currUser.UserId, currUser.Account + ",");
            sql = sql + strWhere;
            dt = this.BaseRepository().FindTable(sql);
            if (dt.Rows.Count > 0)
            {
                toAuditNum.Add(dt.Rows[0]["unitnum"].ToString());
            }
            else
            {
                toAuditNum.Add("0");
            }
            #endregion
            #region ��ǩ���¹��¼�
            sql = string.Empty;
            sql = string.Format("select count(id) unitnum from bis_powerplanthandledetail s ");

            strWhere = string.Empty;
            strWhere += string.Format(" where s.applystate=6 and s.signpersonid like '%{0}%'", currUser.UserId);
            sql = sql + strWhere;
            dt = this.BaseRepository().FindTable(sql);
            if (dt.Rows.Count > 0)
            {
                toAuditNum.Add(dt.Rows[0]["unitnum"].ToString());
            }
            else
            {
                toAuditNum.Add("0");
            }
            #endregion
            #region �������¹��¼�
            sql = string.Empty;
            sql = string.Format("select count(id) unitnum from bis_powerplanthandledetail s ");
            strWhere = string.Empty;
            strWhere += string.Format(" where createuserorgcode = '{0}'  and applystate= 4  and flowdept = '{1}' ", currUser.OrganizeCode, currUser.DeptId);
            strWhere += " and (";
            foreach (string str in arrRole)
            {
                strWhere += string.Format(" flowrolename like '%{0}%' or", str);
            }
            strWhere = strWhere.Substring(0, strWhere.Length - 2);
            strWhere += " )";
            sql = sql + strWhere;
            dt = this.BaseRepository().FindTable(sql);
            if (dt.Rows.Count > 0)
            {
                toAuditNum.Add(dt.Rows[0]["unitnum"].ToString());
            }
            else
            {
                toAuditNum.Add("0");
            }
            #endregion
            return toAuditNum;
        }

        /// <summary>
        /// ��ȡ����ͼ�����Ϣ
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

        /// <summary>
        /// ��ȡ����ͼ������Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public DataTable GetReformInfo(string keyValue)
        {
            Operator user = OperatorProvider.Provider.Current();
            string node_sql = string.Format(@"select a.id, a.rectificationdutyperson,a.rectificationdutypersonid,a.rectificationdutydept,a.realreformdept,a.realreformdeptid,a.flowid,b.rectificationperson,b.createuserdeptcode,b.createdate,a.isassignperson,a.signpersonname,
                                                e.outtransferuseraccount,e.intransferuseraccount,e.outtransferusername,e.intransferusername,a.realsignpersonname,a.signdeptname,a.realsignpersondept,a.realsigndate
                                              from bis_powerplanthandledetail a left join bis_powerplantreform b on a.id=b.powerplanthandledetailid and b.disable=0
                                              left join (select recid,flowid,outtransferuseraccount,intransferuseraccount,outtransferusername,intransferusername,row_number()  over(partition by recid,flowid order by createdate desc) as num from BIS_TRANSFERRECORD where disable=0) e on a.id=e.recid and e.num=1
            where  a.createuserorgcode = '{0}' and a.powerplanthandleid = '{1}'
                                              order by a.createdate ", user.OrganizeCode, keyValue);
            DataTable nodeDt = this.BaseRepository().FindTable(node_sql);
            return nodeDt;
        }

        /// <summary>
        /// ��ȡ����ͼ������Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="modulename"></param>
        /// <returns></returns>
        public DataTable GetCheckInfo(string keyValue, string modulename)
        {
            Operator user = OperatorProvider.Provider.Current();
            string node_sql = string.Format(@"select 
                                                   a.id as flowid, b.id,a.flowname,a.serialnum,a.autoid,a.checkdeptname,a.checkroleid,a.checkrolename,a.checkdeptid,a.remark,b.auditdept,b.auditpeople,b.audittime,b.auditresult
                                              from 
                                                    bis_manypowercheck a left join bis_powerplantcheck b
                                                    on a.id = b.flowid and b.powerplanthandledetailid = '{2}' and b.disable=0
                                              where 
                                                    a.createuserorgcode = '{0}' and a.modulename = '{1}'
                                              order by
                                                    serialnum ", user.OrganizeCode, modulename, keyValue);
            DataTable nodeDt = this.BaseRepository().FindTable(node_sql);
            return nodeDt;
        }

        /// <summary>
        /// ����sql�ű���ȡdatatable
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetTableBySql(string sql)
        {
            DataTable nodeDt = this.BaseRepository().FindTable(sql);
            return nodeDt;
        }

        /// <summary>
        /// ��ȡ�¹��¼�������Ϣ�������ͼ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public List<CheckFlowData> GetAppFlowList(string keyValue)
        {
            List<CheckFlowData> nodelist = new List<CheckFlowData>();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            PowerplanthandleEntity entity = GetEntity(keyValue);
            string deptname = DepartmentService.GetEntityByCode(entity.CreateUserDeptCode).FullName;
            string deptid = DepartmentService.GetEntityByCode(entity.CreateUserDeptCode).DepartmentId;
            DataTable dt = GetAuditInfo(keyValue, "(�¹��¼������¼)���");
            if (dt != null)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    //��˼�¼
                    if (dr["auditdept"] != null && !string.IsNullOrEmpty(dr["auditdept"].ToString()))
                    {

                        CheckFlowData checkdata = new CheckFlowData();
                        DateTime auditdate;
                        DateTime.TryParse(dr["audittime"].ToString(), out auditdate);
                        checkdata.auditdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                        checkdata.auditdeptname = dr["auditdept"].ToString();
                        checkdata.auditusername = dr["auditpeople"].ToString();
                        checkdata.auditstate = dr["auditresult"].ToString() == "0" ? "ͬ��" : "��ͬ��";
                        checkdata.isapprove = "1";
                        checkdata.isoperate = "0";
                        nodelist.Add(checkdata);
                    }
                    else
                    {
                        CheckFlowData checkdata = new CheckFlowData();
                        checkdata.auditdate = "";
                        //����,��Ա
                        var checkDeptId = dr["checkdeptid"].ToString();
                        var checkremark = dr["remark"].ToString();
                        string type = checkremark != "1" ? "0" : "1";
                        if (checkDeptId == "-3" || checkDeptId == "-1")
                        {
                            checkDeptId = deptid;
                            checkdata.auditdeptname = deptname;
                        }
                        else
                        {
                            checkdata.auditdeptname = dr["checkdeptname"].ToString();
                        }
                        string userNames = GetUserName(checkDeptId, dr["checkrolename"].ToString()).Split('|')[0];
                        
                        checkdata.auditusername = !string.IsNullOrEmpty(userNames) ? userNames : "��";
                        checkdata.auditremark = "";
                        checkdata.isapprove = "0";
                        checkdata.isoperate = entity.FlowId == dr["id"].ToString() ? "1" : "0";
                        checkdata.auditstate = "���(��)��";
                        nodelist.Add(checkdata);
                    }
                }
            }
            return nodelist;
        }

        /// <summary>
        /// ��ȡ�����¹��¼�������Ϣ��������ͼ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public List<CheckFlowData> GetAppFullFlowList(string keyValue)
        {
            List<CheckFlowData> nodelist = new List<CheckFlowData>();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            PowerplanthandledetailEntity entity = powerplanthandledetailservice.GetEntity(keyValue);
            string deptname = DepartmentService.GetEntityByCode(entity.CreateUserDeptCode).FullName;
            string deptid = DepartmentService.GetEntityByCode(entity.CreateUserDeptCode).DepartmentId;
            DataTable dt = GetAuditInfo(entity.PowerPlantHandleId, "(�¹��¼������¼)���");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    //��˼�¼
                    if (dr["auditdept"] != null && !string.IsNullOrEmpty(dr["auditdept"].ToString()))
                    {

                        CheckFlowData checkdata = new CheckFlowData();
                        DateTime auditdate;
                        DateTime.TryParse(dr["audittime"].ToString(), out auditdate);
                        checkdata.auditdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                        checkdata.auditdeptname = dr["auditdept"].ToString();
                        checkdata.auditusername = dr["auditpeople"].ToString();
                        checkdata.auditstate = dr["auditresult"].ToString() == "0" ? "ͬ��" : "��ͬ��";
                        checkdata.isapprove = "1";
                        checkdata.isoperate = "0";
                        nodelist.Add(checkdata);
                    }
                    else
                    {
                        CheckFlowData checkdata = new CheckFlowData();
                        checkdata.auditdate = "";
                        //����,��Ա
                        var checkDeptId = dr["checkdeptid"].ToString();
                        var checkremark = dr["remark"].ToString();
                        string type = checkremark != "1" ? "0" : "1";
                        if (checkDeptId == "-3" || checkDeptId == "-1")
                        {
                            checkDeptId = deptid;
                            checkdata.auditdeptname = deptname;
                        }
                        else
                        {
                            checkdata.auditdeptname = dr["checkdeptname"].ToString();
                        }
                        string userNames = GetUserName(checkDeptId, dr["checkrolename"].ToString()).Split('|')[0];

                        checkdata.auditusername = !string.IsNullOrEmpty(userNames) ? userNames : "��";
                        checkdata.auditremark = "";
                        checkdata.isapprove = "0";
                        checkdata.isoperate = entity.FlowId == dr["id"].ToString() ? "1" : "0";
                        checkdata.auditstate = "���(��)��";
                        nodelist.Add(checkdata);
                    }
                }
            }

            #region ǩ�սڵ�
            Boolean HaveSignNode = entity.IsAssignPerson == "1" ? true : false;
            if (entity.IsAssignPerson == "1")
            {
                //��˼�¼
                if (!string.IsNullOrEmpty(entity.RealSignPersonName))
                {
                    CheckFlowData checkdata = new CheckFlowData();
                    DateTime auditdate;
                    DateTime.TryParse(entity.RealSignDate.ToString(), out auditdate);
                    checkdata.auditdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                    checkdata.auditdeptname = entity.RealSignPersonDept;
                    checkdata.auditusername = entity.RealSignPersonName;
                    checkdata.auditstate = "��ǩ��";
                    checkdata.isapprove = "1";
                    checkdata.isoperate = "0";
                    nodelist.Add(checkdata);
                }
                else
                {
                    CheckFlowData checkdata = new CheckFlowData();
                    checkdata.auditdate = "";
                    checkdata.auditdeptname = entity.SignDeptName;
                    checkdata.auditusername = entity.SignPersonName;
                    checkdata.auditremark = "";
                    checkdata.isapprove = "0";
                    checkdata.isoperate = "1";
                    checkdata.auditstate = "ǩ����";
                    nodelist.Add(checkdata);
                }
            }
            #endregion
            #region ���Ľڵ�
            if (!string.IsNullOrEmpty(entity.RectificationDutyPerson))
            {
                PowerplantreformEntity reformentity = PowerplantreformService.GetList("").Where(t => t.PowerPlantHandleDetailId == keyValue && t.Disable == 0).FirstOrDefault();
                if (reformentity != null)
                {
                    CheckFlowData checkdata = new CheckFlowData();
                    DateTime auditdate;
                    DateTime.TryParse(reformentity.CreateDate.ToString(), out auditdate);
                    checkdata.auditdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                    checkdata.auditdeptname = DepartmentService.GetEntityByCode(reformentity.CreateUserDeptCode).FullName;
                    checkdata.auditusername = reformentity.RectificationPerson;
                    checkdata.auditstate = "������";
                    checkdata.isapprove = "1";
                    checkdata.isoperate = "0";
                    nodelist.Add(checkdata);
                }
                else
                {
                    CheckFlowData checkdata = new CheckFlowData();
                    checkdata.auditdate = "";

                    string approveuserid = entity.RectificationDutyPersonId;
                    string[] accounts = UserService.GetUserTable(approveuserid.Split(',')).AsEnumerable().Select(e => e.Field<string>("ACCOUNT")).ToArray();
                    string accountstr = accounts.Length > 0 ? string.Join(",", accounts) + "," : "";
                    TransferrecordEntity transfer = TransferrecordService.GetList(t => t.RecId == entity.Id && t.Disable == 0).FirstOrDefault();
                    string outtransferuseraccount = transfer == null ? "" : transfer.OutTransferUserAccount;//ת��������
                    string intransferuseraccount = transfer == null ? "" : transfer.InTransferUserAccount;//ת��������
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

                    DataTable dtuser = UserService.GetUserTable(accountstr.Split(','));
                    string[] usernames = dtuser.AsEnumerable().Select(d => d.Field<string>("realname")).ToArray();
                    string[] deptnames = dtuser.AsEnumerable().Select(d => d.Field<string>("deptname")).ToArray().GroupBy(t => t).Select(p => p.Key).ToArray();
                    checkdata.auditusername = usernames.Length > 0 ? string.Join(",", usernames) : "��";
                    checkdata.auditdeptname = deptnames.Length > 0 ? string.Join(",", deptnames) : "��";
                    checkdata.auditremark = "";
                    checkdata.isapprove = "0";
                    checkdata.isoperate = "1";
                    checkdata.auditstate = "������";
                    nodelist.Add(checkdata);
                }
                #region  �¹��¼����սڵ�
                
                if (reformentity != null)
                {
                    DataTable dtCheckNodes = GetCheckInfo(keyValue, "�¹��¼������¼-����");
                    if (dtCheckNodes != null && dtCheckNodes.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtCheckNodes.Rows.Count; j++)
                        {
                            DataRow drtemp = dtCheckNodes.Rows[j];
                            //��˼�¼
                            if (drtemp["auditdept"] != null && !string.IsNullOrEmpty(drtemp["auditdept"].ToString()))
                            {
                                CheckFlowData checkdata = new CheckFlowData();
                                DateTime auditdate;
                                DateTime.TryParse(drtemp["audittime"].ToString(), out auditdate);
                                checkdata.auditdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                                checkdata.auditdeptname = drtemp["auditdept"].ToString();
                                checkdata.auditusername = drtemp["auditpeople"].ToString();
                                checkdata.auditstate = drtemp["auditresult"].ToString() == "0" ? "ͨ��" : "��ͨ��";
                                checkdata.auditremark = "";
                                checkdata.isapprove = "1";
                                checkdata.isoperate = "0";
                                nodelist.Add(checkdata);
                            }
                            else
                            {
                                CheckFlowData checkdata = new CheckFlowData();
                                checkdata.auditdate = "";
                                //����,��Ա
                                var checkDeptId = drtemp["checkdeptid"].ToString();
                                var checkremark = drtemp["remark"].ToString();
                                string type = checkremark != "1" ? "0" : "1";
                                if (checkDeptId == "-3" || checkDeptId=="-1")
                                {
                                    checkDeptId = entity.RealReformDeptId;
                                    checkdata.auditdeptname = entity.RealReformDept;
                                }
                                else
                                {
                                    checkdata.auditdeptname = drtemp["checkdeptname"].ToString();
                                }
                                string userNames = GetUserName(checkDeptId, drtemp["checkrolename"].ToString()).Split('|')[0];

                                checkdata.auditusername = !string.IsNullOrEmpty(userNames) ? userNames : "��";
                                checkdata.auditremark = "";
                                checkdata.isapprove = "0";
                                checkdata.isoperate = entity.FlowId == drtemp["flowid"].ToString() ? "1" : "0";
                                checkdata.auditstate = "������";
                                nodelist.Add(checkdata);
                                
                            }
                        }
                    }
                }
                #endregion
            }
            #endregion
            return nodelist;
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, PowerplanthandleEntity entity)
        {
            var res = GetEntity(keyValue);
            if (res == null)
            {
                entity.Id = keyValue;
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
            else
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
        }


        /// <summary>
        /// �����¹��¼���¼״̬
        /// </summary>
        /// <param name="keyValue"></param>
        public void UpdateApplyStatus(string keyValue)
        {
            var list = powerplanthandledetailservice.GetList("").Where(t => t.PowerPlantHandleId == keyValue).ToList();
            var entity = GetEntity(keyValue);
            if (list.Where(t => t.ApplyState == 0).ToList().Count > 0) //������
            {
                entity.ApplyState = 0;
            }
            else if (list.Where(t => t.ApplyState == 1).ToList().Count > 0)  //�����
            {
                entity.ApplyState = 1;
            }
            else if (list.Where(t => t.ApplyState == 2).ToList().Count > 0)  //������
            {
                entity.ApplyState = 2;
            }
            else if (list.Where(t => t.ApplyState == 6).ToList().Count > 0)  //ǩ����
            {
                entity.ApplyState = 6;
            }
            else if (list.Where(t => t.ApplyState == 3).ToList().Count > 0)  //������
            {
                entity.ApplyState = 3;
            }
            else if (list.Where(t => t.ApplyState == 4).ToList().Count > 0)  //������
            {
                entity.ApplyState = 4;
            }
            else if (list.Where(t => t.ApplyState == 5).ToList().Count > 0) //�����
            {
                entity.ApplyState = 5;
            }
            SaveForm(keyValue, entity);
        }
        #endregion

        #region  ��ǰ��¼���Ƿ���Ȩ����˲���ȡ��һ�����Ȩ��ʵ��
        /// <summary>
        /// ��ǰ��¼���Ƿ���Ȩ����˲���ȡ��һ�����Ȩ��ʵ��
        /// </summary>
        /// <param name="currUser">��ǰ��¼��</param>
        /// <param name="state">�Ƿ���Ȩ����� 1������� 0 ���������</param>
        /// <param name="moduleName">ģ������</param>
        /// <param name="createdeptid">�����˲���ID</param>
        /// <returns>null-��ǰΪ���һ�����,ManyPowerCheckEntity����һ�����Ȩ��ʵ��</returns>
        public ManyPowerCheckEntity CheckAuditPower(Operator currUser, out string state, string moduleName, string createdeptid)
        {
            ManyPowerCheckEntity nextCheck = null;//��һ�����
            List<ManyPowerCheckEntity> powerList = powerCheck.GetListBySerialNum(currUser.OrganizeCode, moduleName);

            if (powerList.Count > 0)
            {
                var createdept= new DepartmentService().GetEntity(createdeptid);
                if (createdept != null)
                {
                    foreach (var item in powerList)
                    {
                        if (item.CHECKDEPTID == "-3" || item.CHECKDEPTID == "-1")
                        {
                            item.CHECKDEPTID = createdeptid;
                            item.CHECKDEPTCODE = createdept.DeptCode;
                            item.CHECKDEPTNAME = createdept.FullName;
                        }
                    }
                    List<ManyPowerCheckEntity> checkPower = new List<ManyPowerCheckEntity>();

                    //��¼���Ƿ������Ȩ��--�����Ȩ��ֱ�����ͨ��
                    for (int i = 0; i < powerList.Count; i++)
                    {
                        if (powerList[i].CHECKDEPTID == currUser.DeptId)
                        {
                            var rolelist = currUser.RoleName.Split(',');
                            for (int j = 0; j < rolelist.Length; j++)
                            {
                                if (powerList[i].CHECKROLENAME.Contains(rolelist[j]))
                                {
                                    checkPower.Add(powerList[i]);
                                    break;
                                }
                            }
                        }
                    }
                    powerList.GroupBy(t => t.SERIALNUM).ToList().Count();
                    if (checkPower.Count > 0)
                    {
                        state = "1";
                        ManyPowerCheckEntity check = checkPower.Last();//��ǰ

                        for (int i = 0; i < powerList.Count; i++)
                        {
                            if (check.ID == powerList[i].ID)
                            {
                                if ((i + 1) >= powerList.Count)
                                {
                                    nextCheck = null;
                                }
                                else
                                {
                                    nextCheck = powerList[i + 1];
                                }
                            }
                        }
                    }
                    else
                    {
                        state = "0";
                        nextCheck = powerList.First();
                    }

                    if (null != nextCheck)
                    {
                        //��ǰ�������µĶ�Ӧ����
                        var serialList = powerList.Where(p => p.SERIALNUM == nextCheck.SERIALNUM);
                        //���ϼ�¼����1�����ʾ���ڲ�����ˣ���飩�����
                        if (serialList.Count() > 1)
                        {
                            string flowdept = string.Empty;  // ��ȡֵ��ʽ a1,a2
                            string flowdeptname = string.Empty; // ��ȡֵ��ʽ b1,b2
                            string flowrole = string.Empty;   // ��ȡֵ��ʽ c1|c2|  (c1���ݹ��ɣ� cc1,cc2,cc3)
                            string flowrolename = string.Empty; // ��ȡֵ��ʽ d1|d2| (d1���ݹ��ɣ� dd1,dd2,dd3)

                            ManyPowerCheckEntity slastEntity = new ManyPowerCheckEntity();
                            slastEntity = serialList.LastOrDefault();
                            foreach (ManyPowerCheckEntity model in serialList)
                            {
                                flowdept += model.CHECKDEPTID + ",";
                                flowdeptname += model.CHECKDEPTNAME + ",";
                                flowrole += model.CHECKROLEID + "|";
                                flowrolename += model.CHECKROLENAME + "|";
                            }
                            if (!flowdept.IsEmpty())
                            {
                                slastEntity.CHECKDEPTID = flowdept.Substring(0, flowdept.Length - 1);
                            }
                            if (!flowdeptname.IsEmpty())
                            {
                                slastEntity.CHECKDEPTNAME = flowdeptname.Substring(0, flowdeptname.Length - 1);
                            }
                            if (!flowdept.IsEmpty())
                            {
                                slastEntity.CHECKROLEID = flowrole.Substring(0, flowrole.Length - 1);
                            }
                            if (!flowdept.IsEmpty())
                            {
                                slastEntity.CHECKROLENAME = flowrolename.Substring(0, flowrolename.Length - 1);
                            }
                            nextCheck = slastEntity;
                        }
                    }
                    return nextCheck;
                }
                else
                {
                    state = "0";
                    return nextCheck;
                }
                
            }
            else
            {
                state = "0";
                return nextCheck;
            }

        }
        #endregion

        public string GetUserName(string flowdeptid, string flowrolename, string type = "", string specialtytype = "")
        {
            string names = "";
            string userids = "";
            string flowdeptids = "'" + flowdeptid.Replace(",", "','") + "'";
            string flowrolenames = "'" + flowrolename.Replace(",", "','") + "'";
            IList<UserEntity> users = new UserService().GetUserListByRoleName(flowdeptids, flowrolenames, true, string.Empty).OrderBy(t => t.RealName).ToList();
            if (users != null && users.Count > 0)
            {
                if (!string.IsNullOrEmpty(specialtytype) && type == "1")
                {
                    foreach (var item in users)
                    {

                        if (item.RoleName.Contains("ר��") && flowrolename.Split(',').Union(item.RoleName.Split(',')).Count() == (flowrolename.Split(',').Count() + item.RoleName.Split(',').Count() - 1))
                        {
                            if (!string.IsNullOrEmpty(item.SpecialtyType) && item.SpecialtyType != "null")
                            {
                                string[] str = item.SpecialtyType.Split(',');
                                for (int i = 0; i < str.Length; i++)
                                {
                                    if (str[i] == specialtytype)
                                    {
                                        names += item.RealName + ",";
                                        userids += item.UserId + ",";
                                    }
                                }

                            }
                        }
                        else
                        {
                            names += item.RealName + ",";
                            userids += item.UserId + ",";
                        }
                    }
                    if (!string.IsNullOrEmpty(names))
                    {
                        names = names.TrimEnd(',');
                    }
                    if (!string.IsNullOrEmpty(userids))
                    {
                        userids = userids.TrimEnd(',');
                    }
                }
                else
                {
                    names = string.Join(",", users.Select(x => x.RealName).ToArray());
                    userids = string.Join(",", users.Select(x => x.UserId).ToArray());
                }
            }
            string useridandname = names + "|" + userids;
            return useridandname;
        }
    }
}
