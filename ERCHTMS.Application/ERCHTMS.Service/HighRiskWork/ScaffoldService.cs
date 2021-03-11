using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Util.Extension;
using System.Data.Common;
using BSFramework.Data;
using ERCHTMS.Code;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using System;
using System.Data;
using Newtonsoft.Json.Linq;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Service.OutsourcingProject;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Service.SystemManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Entity.PublicInfoManage;

namespace ERCHTMS.Service.HighRiskWork
{
    /// <summary>
    /// �� ����1.���ּܴ��衢���ա��������2.���ּܴ��衢���ա��������
    /// </summary>
    public class ScaffoldService : RepositoryFactory<ScaffoldEntity>, ScaffoldIService
    {

        ScaffoldspecService scaffoldspecservice = new ScaffoldspecService();
        ScaffoldprojectService scaffoldprojectservice = new ScaffoldprojectService();
        ScaffoldauditrecordService scaffoldauditrecordservice = new ScaffoldauditrecordService();
        HighProjectSetService highProjectSetService = new HighProjectSetService();
        ManyPowerCheckService powerCheck = new ManyPowerCheckService();

        private DataItemDetailService dataitemdetailservice = new DataItemDetailService();
        private IDepartmentService departmentIService = new DepartmentService();
        private HighRiskRecordService highriskrecordservice = new HighRiskRecordService();
        private DepartmentService departmentservice = new DepartmentService();
        private HighRiskCommonApplyService highriskcommonapplyservice = new HighRiskCommonApplyService();
        private UserService userservice = new UserService();
        #region ��ȡ����

        /// <summary>
        /// �õ���ǰ�����
        /// </summary>
        /// <returns></returns>
        public string GetMaxCode()
        {
            string orgCode = OperatorProvider.Provider.Current().OrganizeCode;
            string sql =string.Format("select max(ApplyCode) from bis_scaffold where CreateUserOrgCode = @orgCode and applyCode like '%{0}%'", DateTime.Now.ToString("yyyyMMdd"));
            object o = this.BaseRepository().FindObject(sql, new DbParameter[]{
                DbParameters.CreateDbParameter("@orgCode",orgCode)
            });
            if (o == null || o.ToString() == "")
                return "J" + DateTime.Now.ToString("yyyyMMdd") + "001";
            int num = Convert.ToInt32(o.ToString().Substring(9));
            num++;
            return "J" + DateTime.Now.ToString("yyyyMMdd") + num.ToString().PadLeft(3, '0');
        }

        #region ��ȡ�б�
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="page">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public DataTable GetList(Pagination page, string queryJson)
        {
            DatabaseType dataTye = DatabaseType.Oracle;

            #region ���
            page.p_kid = "a.id";
            page.p_fields = @"a.applyuserid,a.applyusername,a.applydate,a.applycode,a.setupcompanytype,a.setupcompanyid,a.setupcompanyname,a.setupcompanyid1,a.setupcompanyname1,
                                a.outprojectid,a.outprojectname,a.dismentlestartdate,a.dismentleenddate,a.actsetupstartdate,a.actsetupenddate,
                                a.setupstartdate,a.setupenddate,a.setuptype,a.scaffoldtype,a.auditstate,a.setupaddress,a.createuserdeptcode,
                                a.flowid,a.flowname,a.flowroleid,a.flowrolename,a.flowdeptid,a.flowdeptname,a.flowremark,a.specialtytype,'' as approveuserid,'' as approveusername,'' as approveuseraccount,case when (a.id  in (
                                select id from bis_scaffold where scaffoldtype = 0 and auditstate = 3
                                  and id not in(select nvl(setupinfoid,'-') from bis_scaffold where scaffoldtype = 1 and auditstate in(0,1,3,4,6)))) then '1'
                                  when (a.id in ( select id from bis_scaffold where scaffoldtype = 1 and auditstate= 3 and id not in(select nvl(setupinfoid,'-') from bis_scaffold where scaffoldtype = 2 and auditstate in(0,1,3)))) then '2'
                                  else '0' end status,b.outtransferuseraccount,b.intransferuseraccount";
            page.p_tablename = @"bis_scaffold a left join (select recid,flowid,outtransferuseraccount,intransferuseraccount,row_number()  over(partition by recid,flowid order by createdate desc) as num from BIS_TRANSFERRECORD where disable=0) b on a.id=b.recid and a.flowid=b.flowid and b.num=1";
            #endregion

            #region  ɸѡ����
            var queryParam = JObject.Parse(queryJson);
            //�б�����
            if (!queryParam["ScaffoldType"].IsEmpty())
            {
                page.conditionJson += " and a.ScaffoldType = " + queryParam["ScaffoldType"].ToString();
            }
            //���ּܴ�������
            if (!queryParam["SetupType"].IsEmpty())
            {
                page.conditionJson += " and a.SetupType = " + queryParam["SetupType"].ToString();
            }
            //���赥λ����
            if (!queryParam["SetupCompanyType"].IsEmpty())
            {
                page.conditionJson += " and a.SetupCompanyType = " + queryParam["SetupCompanyType"].ToString();
            }
            //��ҵ״̬
            if (!queryParam["AuditState"].IsEmpty())
            {
                page.conditionJson += " and a.AuditState in (" + queryParam["AuditState"].ToString() + ")";
            }
            //���赥λ
            if (!queryParam["SetupCompanyId"].IsEmpty() && !queryParam["SetupCompanyCode"].IsEmpty())
            {
                page.conditionJson += string.Format(" and ((a.setupcompanyid in(select departmentid from base_department where encode like '{0}%'))  or (a.outprojectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", queryParam["SetupCompanyCode"].ToString(), queryParam["SetupCompanyId"].ToString());
            }
            //����ʱ��
            if (!queryParam["ScaffoldType"].IsEmpty())
            {
                if (queryParam["ScaffoldType"].ToString() == "0")
                {
                    if (!queryParam["SetupStartDate"].IsEmpty())
                    {
                        page.conditionJson += string.Format(" and a.SetupStartDate >= to_date('{0}','yyyy-MM-dd')", queryParam["SetupStartDate"].ToString());
                    }
                    if (!queryParam["SetupEndDate"].IsEmpty())
                    {
                        page.conditionJson += string.Format(" and a.SetupEndDate <= to_date('{0}','yyyy-MM-dd')", Convert.ToDateTime(queryParam["SetupEndDate"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
                    }
                }
                //����б�����Ϊ�������룬��������ڲ�ʵ�ʴ���ʱ��
                if (queryParam["ScaffoldType"].ToString() == "1")
                {
                    if (!queryParam["SetupStartDate"].IsEmpty())
                    {
                        page.conditionJson += string.Format(" and a.ActSetupStartDate >= to_date('{0}','yyyy-MM-dd')", queryParam["SetupStartDate"].ToString());
                    }
                    if (!queryParam["SetupEndDate"].IsEmpty())
                    {
                        page.conditionJson += string.Format(" and a.ActSetupEndDate <= to_date('{0}','yyyy-MM-dd')", Convert.ToDateTime(queryParam["SetupEndDate"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
                    }
                }
                //����б�����Ϊ������룬��������ڲ���ʱ��
                if (queryParam["ScaffoldType"].ToString() == "2")
                {
                    if (!queryParam["SetupStartDate"].IsEmpty())
                    {
                        page.conditionJson += string.Format(" and a.DismentleStartDate >= to_date('{0}','yyyy-MM-dd')", queryParam["SetupStartDate"].ToString());
                    }
                    if (!queryParam["SetupEndDate"].IsEmpty())
                    {
                        page.conditionJson += string.Format(" and a.DismentleEndDate <= to_date('{0}','yyyy-MM-dd')", Convert.ToDateTime(queryParam["SetupEndDate"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
                    }
                }
            }
            //������
            if (!queryParam["ApplyCode"].IsEmpty())
            {
                page.conditionJson += " and a.ApplyCode = '" + queryParam["ApplyCode"].ToString() + "'";
            }

            if (!queryParam["ViewRange"].IsEmpty())
            {
                var user = OperatorProvider.Provider.Current();
                //����
                if (queryParam["ViewRange"].ToString().ToLower() == "self")
                {
                    page.conditionJson += string.Format(" and a.ApplyUserId='{0}'", user.UserId);
                }
                else if (queryParam["ViewRange"].ToString().ToLower() == "selfaudit" || queryParam["ViewRange"].ToString().ToLower() == "selfapprove" || queryParam["ViewRange"].ToString().ToLower() == "selfconfirm")
                {
                    string strCondition = "";
                    switch (queryParam["ViewRange"].ToString().ToLower())
                    {
                        case "selfaudit":  //������
                            strCondition = " and a.AuditState in(1,4,6)";
                            break;
                        case "selfapprove":  //�����(��)
                            strCondition = " and a.AuditState in(1,6)";
                            break;
                        case "selfconfirm":  //������ȷ��
                            strCondition = " and a.AuditState in(4)";
                            break;
                        default:
                            break;
                    }
                    DataTable dt = BaseRepository().FindTable("select " + page.p_kid + "," + page.p_fields + " from " + page.p_tablename + " where " + page.conditionJson + strCondition);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string executedept = string.Empty;
                        highriskcommonapplyservice.GetExecutedept(dt.Rows[i]["setupcompanytype"].ToString(), dt.Rows[i]["setupcompanyid"].ToString(), dt.Rows[i]["outprojectid"].ToString(), out executedept);
                        string createdetpid = departmentservice.GetEntityByCode(dt.Rows[i]["createuserdeptcode"].ToString()).IsEmpty()?"": departmentservice.GetEntityByCode(dt.Rows[i]["createuserdeptcode"].ToString()).DepartmentId;
                        string outsouringengineerdept = string.Empty;
                        highriskcommonapplyservice.GetOutsouringengineerDept(dt.Rows[i]["setupcompanyid"].ToString(), out outsouringengineerdept);
                        string str = powerCheck.GetApproveUserAccount(dt.Rows[i]["flowid"].ToString(), dt.Rows[i]["id"].ToString(), "", dt.Rows[i]["specialtytype"].ToString(), executedept, outsouringengineerdept, createdetpid, "", "");
                        dt.Rows[i]["approveuseraccount"] = str;
                    }
                    string[] applyids = dt.Select("(outtransferuseraccount is null or outtransferuseraccount not like '%" + user.Account + ",%') and (approveuseraccount like '%" + user.Account + ",%' or intransferuseraccount like '%" + user.Account + ",%')").AsEnumerable().Select(d => d.Field<string>("id")).ToArray();
                    
                    page.conditionJson += string.Format(" and a.id in ('{0}') {1}", string.Join("','", applyids), strCondition);
                }
                else
                {
                    //ȫ�� �ų����������뱣�������
                    page.conditionJson += string.Format("  and a.id not in(select id from bis_scaffold where auditstate = 0 and  applyuserid != '{0}')", user.UserId);
                }
            }

            if (!queryParam["IsNoDismentle"].IsEmpty() && !queryParam["ScaffoldType"].IsEmpty())
            {
                if (queryParam["IsNoDismentle"].ToString().ToLower() == "true")
                {
                    if (queryParam["ScaffoldType"].ToString() == "0")
                    {
                        //��������ʱ��ѡ�����������Ϣ���ͨ��,�����������δͨ����δ�������������
                        page.conditionJson += @" and a.id  in (
                                select id from bis_scaffold where scaffoldtype = 0 and auditstate = 3
                                  and id not in(select nvl(setupinfoid,'-') from bis_scaffold where scaffoldtype = 1 and auditstate in(0,1,3,4,6))
                              ) ";
                    }
                    if (queryParam["ScaffoldType"].ToString() == "1")
                    {
                        //�������ʱ��ѡ�������������ͨ��������������δͨ����δ������������
                        page.conditionJson += @" and a.id in (
                                  select id from bis_scaffold where scaffoldtype = 1 and auditstate= 3
                                  and id not in(select nvl(setupinfoid,'-') from bis_scaffold where scaffoldtype = 2 and auditstate in(0,1,3))
                        ) ";
                    }
                }
            }

            #endregion
            DataTable data = this.BaseRepository().FindTableByProcPager(page, dataTye);
            for (int i = 0; i < data.Rows.Count; i++)
            {
                string executedept = string.Empty;
                highriskcommonapplyservice.GetExecutedept(data.Rows[i]["setupcompanytype"].ToString(), data.Rows[i]["setupcompanyid"].ToString(), data.Rows[i]["outprojectid"].ToString(), out executedept);
                string createdetpid = departmentservice.GetEntityByCode(data.Rows[i]["createuserdeptcode"].ToString()).IsEmpty()?"": departmentservice.GetEntityByCode(data.Rows[i]["createuserdeptcode"].ToString()).DepartmentId;
                string outsouringengineerdept = string.Empty;
                highriskcommonapplyservice.GetOutsouringengineerDept(data.Rows[i]["setupcompanyid"].ToString(), out outsouringengineerdept);
                string str = powerCheck.GetApproveUserAccount(data.Rows[i]["flowid"].ToString(), data.Rows[i]["id"].ToString(), "", data.Rows[i]["specialtytype"].ToString(), executedept, outsouringengineerdept, createdetpid, "", "");
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
        #endregion

        #region ̨���б�
        /// <summary>
        /// ̨���б�
        /// </summary>
        /// <param name="page"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetLedgerList(Pagination page, string queryJson, string authType)
        {
            DatabaseType dataTye = DatabaseType.Oracle;
            #region ����Ȩ��
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var curUserId = user.UserId;
            //�鿴��Χ����Ȩ��
            /**
             * 1.��ҵ��λ���Ӳ��ţ��¼���
             * 2.���˴����ĸ߷�����ҵ
             * 3.�������Ź�Ͻ�������λ
             * 4.�����λֻ�ܿ�����λ��
             * */
            string isAllDataRange = dataitemdetailservice.GetEnableItemValue("HighRiskWorkDataRange");
            if (!user.IsSystem)
            {
                if (!string.IsNullOrEmpty(authType))
                {
                    if (!string.IsNullOrWhiteSpace(isAllDataRange))
                    {
                        page.conditionJson += " and a.createuserorgcode='" + user.OrganizeCode + "'";
                    }
                    else
                    {
                        switch (authType)
                        {
                            case "1":
                                page.conditionJson += " and a.applyuserid='" + user.UserId + "'";
                                break;
                            case "2":
                                page.conditionJson += " and a.setupcompanyid='" + user.DeptId + "'";
                                break;
                            case "3"://���Ӳ���
                                page.conditionJson += string.Format("  and ((a.setupcompanyid in(select departmentid from base_department where encode like '{0}%'))  or (a.outprojectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);
                                break;
                            case "4":
                                page.conditionJson += " and a.createuserorgcode='" + user.OrganizeCode + "'";
                                break;
                            case "app":
                                if (user.RoleName.Contains("��˾") || user.RoleName.Contains("����"))
                                {
                                    page.conditionJson += " and a.createuserorgcode='" + user.OrganizeCode + "'";
                                }
                                else
                                {
                                    page.conditionJson += string.Format(" and ((a.setupcompanyid in(select departmentid from base_department where encode like '{0}%'))  or (a.outprojectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);
                                }
                                break;
                        }
                    }
                    
                }
                else
                {
                    page.conditionJson = " and 0=1";
                }
            }
            #endregion

            /*
              ��������:��������ͨ��,ʵ�ʿ�ʼʱ��Ϊ�գ�������δ���
              ������:��������ͨ��,ʵ�ʿ�ʼʱ�䲻Ϊ�գ�ʵ�ʽ���ʱ��Ϊ�գ�������δ���
              �Ѵ���:��������ͨ��,ʵ�ʿ�ʼʱ�䲻Ϊ�գ�ʵ�ʽ���ʱ�䲻Ϊ�գ�������δ���
              ����:��������ͨ��,�Ҳ������δ���
              �������:�������ͨ��,ʵ�ʲ����ʼʱ��Ϊ��
              �����:�������ͨ��,ʵ�ʲ������ʱ��Ϊ�գ������ʼʱ�䲻Ϊ��
              ������:�������ͨ��,ʵ�ʲ������ʱ�䲻Ϊ�գ������ʼʱ�䲻Ϊ��
             * 
             */ 
            #region ���
            page.p_kid = "a.id";
            page.p_fields = @"a.applycode,a.scaffoldtype,a.ledgertype,b.itemname as ledgertypename,a.outprojectname,a.outprojectid,a.setupcompanyname,a.actsetupstartdate,a.actsetupenddate,a.SetupStartDate,a.SetupEndDate,a.checkdate,a.dismentlecompanyname,a.dismentlestartdate,a.dismentleenddate,a.setupaddress,a.setupchargepersonids,a.measurecarryoutid,a.realitydismentlestartdate,a.realitydismentleenddate,a.createuserid,a.setupcompanytype,a.setupcompanyid,'' as isoperate,workoperate";
            page.p_tablename = @"v_scaffoldledger a left join base_dataitemdetail b on a.ledgertype=b.itemvalue and itemid =(select itemid from base_dataitem where itemcode='LedgerType')";
            #endregion

            #region  ɸѡ����
            var queryParam = JObject.Parse(queryJson);

            //���������λ
            if (!queryParam["SetupCompanyId"].IsEmpty() && !queryParam["SetupCompanyCode"].IsEmpty())
            {
                page.conditionJson += string.Format("  and ((a.setupcompanyid in(select departmentid from base_department where encode like '{0}%'))  or (a.outprojectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", queryParam["SetupCompanyCode"].ToString(), queryParam["SetupCompanyId"].ToString());
            }
            if (!queryParam["DismentleCompanyId"].IsEmpty() && !queryParam["DismentleCompanyCode"].IsEmpty())
            {
                page.conditionJson += string.Format(" and ((a.dismentlecompanyid in(select departmentid from base_department where encode like '{0}%'))  or (a.outprojectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", queryParam["DismentleCompanyCode"].ToString(), queryParam["DismentleCompanyId"].ToString());
            }
            //ʵ�ʴ���ʱ��
            if (!queryParam["ActSetupStartDate"].IsEmpty())
            {
                page.conditionJson += string.Format(" and a.ActSetupStartDate >= to_date('{0}','yyyy-MM-dd')", queryParam["ActSetupStartDate"].ToString());
            }
            if (!queryParam["ActSetupEndDate"].IsEmpty())
            {
                page.conditionJson += string.Format(" and a.ActSetupEndDate <= to_date('{0}','yyyy-MM-dd')", Convert.ToDateTime(queryParam["ActSetupEndDate"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));

            }
            //���ʱ��
            if (!queryParam["DismentleStartDate"].IsEmpty())
            {
                page.conditionJson += string.Format(" and a.dismentlestartdate >= to_date('{0}','yyyy-MM-dd')", queryParam["DismentleStartDate"].ToString());
            }
            if (!queryParam["DismentleEndDate"].IsEmpty())
            {
                page.conditionJson += string.Format(" and a.dismentleenddate <= to_date('{0}','yyyy-MM-dd')", Convert.ToDateTime(queryParam["DismentleEndDate"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            }
            //����ʱ��
            if (!queryParam["CheckStartDate"].IsEmpty())
            {
                page.conditionJson += string.Format(" and to_date(a.checkdate,'yyyy-MM-dd hh24:mi') >= to_date('{0}','yyyy-MM-dd')", queryParam["CheckStartDate"].ToString());
            }
            if (!queryParam["CheckEndDate"].IsEmpty())
            {
                page.conditionJson += string.Format(" and to_date(a.checkdate,'yyyy-MM-dd hh24:mi') <= to_date('{0}','yyyy-MM-dd')", Convert.ToDateTime(queryParam["CheckEndDate"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            }
            //״̬
            if (!queryParam["LedgerType"].IsEmpty())
            {
                page.conditionJson += " and a.LedgerType = " + queryParam["LedgerType"].ToString();
            }
            //�����кͲ����
            if (!queryParam["Working"].IsEmpty())
            {
                page.conditionJson += " and a.LedgerType in(1,5)";
            }
            //��������
            if (!queryParam["OutProjectName"].IsEmpty())
            {
                page.conditionJson += " and a.OutProjectName like '%" + queryParam["OutProjectName"].ToString() + "%'";
            }
            //������
            if (!queryParam["applynumber"].IsEmpty())
            {
                page.conditionJson += " and a.applycode like '%" + queryParam["applynumber"].ToString() + "%'";
            }
            #endregion

            //return this.BaseRepository().FindTableByProcPager(page, dataTye);
            var data = this.BaseRepository().FindTableByProcPager(page, dataTye);
            #region ����Ȩ��
            if (data != null)
            {
                string strRole = dataitemdetailservice.GetItemValue(user.OrganizeId, "LedgerSendDept");//�������Ž�ɫ
                string strManageRole = dataitemdetailservice.GetItemValue(user.OrganizeId, "LedgerManageDept");//��ȫ���ܲ��ż�ܽ�ɫ
                string strWorkRole = dataitemdetailservice.GetItemValue(user.OrganizeId, "LedgerWorkDept");//��ҵ��λ
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    string str = "0";
                    string buildDutyUserId = data.Rows[i]["setupchargepersonids"].ToString();
                    string dismantleDutyUserId = data.Rows[i]["measurecarryoutid"].ToString();
                    string applyUserId = data.Rows[i]["createuserid"].ToString();
                    string outprojectid = data.Rows[i]["outprojectid"].ToString();//����id
                    string workDeptType = data.Rows[i]["setupcompanytype"].ToString();
                    string workdeptid = data.Rows[i]["setupcompanyid"].ToString();//��ҵ��λid
                    var dept = new OutsouringengineerService().GetEntity(outprojectid); //��ȡ����id��Ӧ�����β���
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
                    if (str != "1" && (curUserId == buildDutyUserId || curUserId == applyUserId || curUserId == dismantleDutyUserId))//��ҵ�����˻�������
                    {
                        str = "1";
                    }
                    if (str != "1" && dept != null)
                    {
                        if (workDeptType == "1")//�����λ
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
            ScaffoldEntity entity = GetEntity(keyValue);
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
                        highriskcommonapplyservice.GetExecutedept(entity.SetupCompanyType.ToString(), entity.SetupCompanyId, entity.OutProjectId, out executedept);//��ȡִ�в���
                        string createdetpid = departmentservice.GetEntityByCode(entity.CreateUserDeptCode).IsEmpty() ? "" : departmentservice.GetEntityByCode(entity.CreateUserDeptCode).DepartmentId; //��ȡ��������ID
                        string outsouringengineerdept = string.Empty;
                        highriskcommonapplyservice.GetOutsouringengineerDept(entity.SetupCompanyId, out outsouringengineerdept);
                        string accountstr = powerCheck.GetApproveUserAccount(dr["id"].ToString(), entity.Id, "", entity.SpecialtyType, executedept, outsouringengineerdept, createdetpid, "", ""); //��ȡ������˺�
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
                if (entity.InvestigateState == 3)
                {
                    setInfo sinfo = new setInfo();
                    sinfo.NodeName = nodes_end.name;
                    sinfo.Taged = 1;
                    List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                    NodeDesignateData nodedesignatedata = new NodeDesignateData();

                    //ȡ���̽���ʱ�Ľڵ���Ϣ
                    DataRow[] end_rows = nodeDt.Select("auditusername is not null").OrderBy(t => t.Field<DateTime>("auditdate")).ToArray();
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
        
        public List<CheckFlowData> GetAppFlowList(string keyValue, string modulename)
        {
            List<CheckFlowData> nodelist = new List<CheckFlowData>();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            ScaffoldEntity entity = GetEntity(keyValue);
            DataTable dt =highriskcommonapplyservice.GetCheckInfo(keyValue, modulename, user);
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
                        highriskcommonapplyservice.GetExecutedept(entity.SetupCompanyType.ToString(), entity.SetupCompanyId, entity.OutProjectId, out executedept);//��ȡִ�в���
                        string createdetpid = departmentservice.GetEntityByCode(entity.CreateUserDeptCode).IsEmpty()?"": departmentservice.GetEntityByCode(entity.CreateUserDeptCode).DepartmentId; //��ȡ��������ID
                        string outsouringengineerdept = string.Empty;
                        highriskcommonapplyservice.GetOutsouringengineerDept(entity.SetupCompanyId, out outsouringengineerdept);
                        string accountstr = powerCheck.GetApproveUserAccount(dr["id"].ToString(), entity.Id, "", entity.SpecialtyType, executedept, outsouringengineerdept, createdetpid, "", ""); //��ȡ������˺�
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
                        if (entity.InvestigateState == 3)
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

        #region ��ȡ���ּܴ���Ͳ��
        /// <summary>
        /// ��ȡ���ּܴ���Ͳ��
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetSelectPageList(Pagination pagination, string queryJson)
        {
            var user = OperatorProvider.Provider.Current();
            DatabaseType dataTye = DatabaseType.Oracle;
            var queryParam = JObject.Parse(queryJson);
            //���ּܴ�������
            if (!queryParam["checktype"].IsEmpty())
            {
                var checktype = queryParam["checktype"].ToString();
                if (checktype == "-1")//����
                {
                    pagination.conditionJson += "  and LedgerType in('0','1')";
                }
                else if (checktype == "-2")//���
                {
                    pagination.conditionJson += "  and LedgerType in('4','5')";
                }
            }
            //��ҵ��λ
            if (!queryParam["taskdeptid"].IsEmpty())
            {
                if (queryParam["tasktype"].ToString() == "0")
                {
                    pagination.conditionJson += string.Format(" and setupcompanyid='{0}'", queryParam["taskdeptid"].ToString());

                }
                else
                {
                    if (!queryParam["engineeringname"].IsEmpty())
                    {
                        pagination.conditionJson += string.Format(" and setupcompanyid='{0}'", queryParam["taskdeptid"].ToString());
                    }
                    else
                    {
                        var depart = new DepartmentService().GetEntity(queryParam["taskdeptid"].ToString());
                        pagination.conditionJson += string.Format(" and ((setupcompanyid in(select departmentid from base_department where encode like '{0}%'))  or (outprojectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", depart.EnCode, depart.DepartmentId);

                    }
                }
            }
            //��������
            if (!queryParam["engineeringname"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and outprojectname='{0}'", queryParam["engineeringname"].ToString());
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataTye);
        }
        #endregion

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public ScaffoldEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }


        /// <summary>
        /// ��ȡ��λ
        /// </summary>
        /// <param name="deptid">����id</param>
        /// <returns></returns>
        public DepartmentEntity GetDutyDept(string deptid, string projectid, string checkdeptid, string approvedeptid = "")
        {
            DepartmentEntity dept = new DepartmentEntity();
            dept = departmentservice.GetEntity(deptid);
            Operator currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (checkdeptid == "-2")
            {
                var cbsentity = departmentservice.GetList().Where(t => t.Description == "������̳а���" && t.OrganizeId == currUser.OrganizeId).FirstOrDefault();
                var wbentity = departmentservice.GetEntity(deptid);
                while (wbentity.ParentId != cbsentity.DepartmentId)
                {
                    wbentity = departmentservice.GetEntity(wbentity.ParentId);
                }
                dept = wbentity;
            }
            if (checkdeptid == "-1" && !string.IsNullOrEmpty(projectid))
            {
                dept = departmentservice.GetEntity(new OutsouringengineerService().GetEntity(projectid).ENGINEERLETDEPTID);
            }
            if (checkdeptid == "-1" && string.IsNullOrEmpty(projectid))
            {
                while (dept.Nature != "����")
                {
                    dept = departmentservice.GetEntity(dept.ParentId);
                }
                dept.DepartmentId = dept.DepartmentId + "," + departmentservice.GetEntity(deptid).DepartmentId;
                dept.FullName = dept.FullName + "," + departmentservice.GetEntity(deptid).FullName;
            }
            if (checkdeptid =="-5" && !string.IsNullOrWhiteSpace(approvedeptid))
            {
                dept = departmentservice.GetEntity(approvedeptid);
            }
            return dept;
        }


        /// <summary>
        /// ��ȡ��Ա
        /// </summary>
        /// <param name="flowdeptid"></param>
        /// <param name="flowrolename"></param>
        /// <param name="type"></param>
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

                        if (item.RoleName.Contains("ר��") && flowrolename.Split(',').Union(item.RoleName.Split(',')).Count() == (flowrolename.Split(',').Count() + item.RoleName.Split(',').Count() - 1)) //����û�ӵ��ר����ɫ���һ�����˽�ɫ�е�����һ���Ͳ���Ҫ�ж�רҵ
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

                db.Delete<ScaffoldprojectEntity>(t => t.ScaffoldId.Equals(keyValue));
                db.Delete<ScaffoldspecEntity>(t => t.ScaffoldId.Equals(keyValue));
                db.Delete<HighRiskRecordEntity>(t => t.WorkId.Equals(keyValue));
                db.Delete<FileInfoEntity>(t => t.RecId.Equals(keyValue));
                db.Delete<ScaffoldEntity>(keyValue);
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
        public void SaveForm(string keyValue, ScaffoldEntity entity)
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
        public void SaveForm(string keyValue, ScaffoldModel model)
        {
            var res = DbFactory.Base().BeginTrans();
            try
            {

                Repository<ScaffoldEntity> repScaffold = new Repository<ScaffoldEntity>(DbFactory.Base());
                ScaffoldEntity entity = repScaffold.FindEntity(keyValue);
                //����
                if (entity == null)
                {
                    entity = new ScaffoldEntity();
                    entity.Create(keyValue);
                    //ʵ�帳ֵ
                    this.copyProperties(entity, model);
                    //���ɱ���
                    entity.ApplyCode = string.IsNullOrEmpty(entity.SetupInfoCode) ? this.GetMaxCode() : entity.SetupInfoCode;
                    //��Ӳ���
                    res.Insert(entity);


                    //��������������Ŀ���� ���뱣��ʱ
                    if (entity.ScaffoldType == 1 && (model.ScaffoldProjects == null || model.ScaffoldProjects.Count == 0))
                    {
                        //�����Ŀ���Ƿ���ֵ
                        List<ScaffoldprojectEntity> projects = scaffoldprojectservice.GetList(keyValue);
                        if (projects == null || projects.Count == 0)
                        {

                            //��û��������ñ��д���
                            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                            List<HighProjectSetEntity> baseprojects = null;
                            if (!user.IsSystem)
                            {
                                baseprojects = highProjectSetService.GetList(" and typenum = -1 and  createuserorgcode = " + user.OrganizeCode).ToList();
                            }
                            else
                            {
                                baseprojects = highProjectSetService.GetList(" and typenum = -1 ").ToList();
                            }

                            if (baseprojects != null && baseprojects.Count > 0)
                            {
                                if (model.ScaffoldProjects == null)
                                {
                                    model.ScaffoldProjects = new List<ScaffoldprojectEntity>();
                                }
                                foreach (var item in baseprojects)
                                {
                                    ScaffoldprojectEntity projectEntity = new ScaffoldprojectEntity();
                                    projectEntity.ProjectId = item.Id;
                                    projectEntity.ProjectName = item.MeasureName;
                                    projectEntity.ResultYes = item.MeasureResultOne;
                                    projectEntity.ResultNo = item.MeasureResultTwo;

                                    model.ScaffoldProjects.Add(projectEntity);
                                }
                            }
                        }
                    }
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

                //��ӻ���¼����� ��ɾ�������
                res.Delete<ScaffoldspecEntity>(t => t.ScaffoldId == entity.Id);
                foreach (var spec in model.ScaffoldSpecs)
                {
                    spec.ScaffoldId = entity.Id;
                    spec.Create();
                    res.Insert(spec);
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

                //��ӻ����������Ŀ ��������ʱ�Ŵ���
                if (model.ScaffoldType == 1 && model.ScaffoldProjects != null && model.ScaffoldProjects.Count > 0)
                {
                    foreach (var pro in model.ScaffoldProjects)
                    {
                        pro.ScaffoldId = entity.Id;
                        if (!string.IsNullOrEmpty(pro.Id))
                        {
                            pro.Modify(pro.Id);
                            res.Update(pro);
                        }
                        else
                        {
                            pro.Create();
                            res.Insert(pro);
                        }
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
        /// ����ҵ�����˱�������Ŀ
        /// </summary>
        /// <param name="scaffoldEntity">ҵ������ʵ��</param>
        /// <param name="auditEntity">��˱�ʵ��</param>
        /// <param name="projects">������Ŀ ScaffoldType=1 ʱ����</param>
        public void UpdateForm(ScaffoldEntity scaffoldEntity, ScaffoldauditrecordEntity auditEntity, List<ScaffoldprojectEntity> projects)
        {
            try
            {
                this.SaveForm(scaffoldEntity.Id, scaffoldEntity);
                if (auditEntity != null)
                {
                    auditEntity.AuditDate = DateTime.Now;
                    auditEntity.AuditSignImg = string.IsNullOrWhiteSpace(auditEntity.AuditSignImg) ? "" : auditEntity.AuditSignImg.ToString().Replace("../..", "");
                    scaffoldauditrecordservice.SaveForm(auditEntity.Id, auditEntity);
                }
                if (projects != null && projects.Count > 0)
                {
                    foreach (var item in projects)
                    {
                        scaffoldprojectservice.SaveForm(item.Id, item);
                    }
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
        private void copyProperties(ScaffoldEntity target, ScaffoldModel source)
        {
            //��������
            target.ApplyCompanyId = source.ApplyCompanyId;
            target.ApplyCompanyCode = source.ApplyCompanyCode;
            target.ApplyCompanyName = source.ApplyCompanyName;
            target.ApplyUserId = source.ApplyUserId;
            target.ApplyUserName = source.ApplyUserName;
            target.ApplyDate = source.ApplyDate;
            target.ApplyCode = source.ApplyCode;
            target.SetupCompanyType = source.SetupCompanyType;
            target.SetupType = source.SetupType;
            target.SetupCompanyId = source.SetupCompanyId;
            target.SetupCompanyCode = source.SetupCompanyCode;
            target.SetupCompanyName = source.SetupCompanyName;

            target.SetupCompanyId1 = source.SetupCompanyId1;
            target.SetupCompanyCode1 = source.SetupCompanyCode1;
            target.SetupCompanyName1 = source.SetupCompanyName1;

            target.OutProjectId = source.OutProjectId;
            target.OutProjectName = source.OutProjectName;
            target.SetupStartDate = source.SetupEndDate;
            target.WorkArea = source.WorkArea;
            target.WorkAreaCode = source.WorkAreaCode;
            target.SetupAddress = source.SetupAddress;
            target.SetupChargePersonIds = source.SetupChargePersonIds;
            target.SetupChargePerson = source.SetupChargePerson;
            target.SetupPersons = source.SetupPersons;
            target.SetupPersonIds = source.SetupPersonIds;
            target.CopyUserNames = source.CopyUserNames;
            target.CopyUserIds = source.CopyUserIds;
            target.Purpose = source.Purpose;
            target.Parameter = source.Parameter;
            target.ExpectDismentleDate = source.ExpectDismentleDate;
            target.DemandDismentleDate = source.DemandDismentleDate;
            target.SetupStartDate = source.SetupStartDate;
            target.SetupEndDate = source.SetupEndDate;
            //���մ��벿��
            target.ActSetupStartDate = source.ActSetupStartDate;
            target.ActSetupEndDate = source.ActSetupEndDate;
            //������벿�� 
            target.DismentleStartDate = source.DismentleStartDate;
            target.DismentleEndDate = source.DismentleEndDate;
            target.DismentlePart = source.DismentlePart;
            target.DismentleReason = source.DismentleReason;
            target.DismentlePersonsIds = source.DismentlePersonsIds;
            target.DismentlePersons = source.DismentlePersons;
            target.FrameMaterial = source.FrameMaterial;
            target.MeasurePlan = source.MeasurePlan;
            target.MeasureCarryout = source.MeasureCarryout;
            target.MeasureCarryoutId = source.MeasureCarryoutId;
            target.SetupCompanyType = source.SetupCompanyType;
            target.SetupCompanyId = source.SetupCompanyId;
            //��Ϣ����ID
            target.SetupInfoId = source.SetupInfoId;
            target.SetupInfoCode = source.SetupInfoCode;
            target.SetupType = source.SetupType;
            target.ScaffoldType = source.ScaffoldType;
            target.AuditState = source.AuditState;
            //���������Ϣ
            target.FlowId = source.FlowId;
            target.FlowName = source.FlowName;
            target.FlowRoleId = source.FlowRoleId;
            target.FlowRoleName = source.FlowRoleName;
            target.FlowDeptId = source.FlowDeptId;
            target.FlowDeptName = source.FlowDeptName;
            target.InvestigateState = source.InvestigateState;
            target.FlowRemark = source.FlowRemark;

            target.SpecialtyType = source.SpecialtyType;
            target.SetupTypeName = source.SetupTypeName;

        }




        #endregion
    }
}
