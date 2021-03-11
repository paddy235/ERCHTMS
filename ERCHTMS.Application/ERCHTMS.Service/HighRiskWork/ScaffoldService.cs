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
    /// 描 述：1.脚手架搭设、验收、拆除申请2.脚手架搭设、验收、拆除审批
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
        #region 获取数据

        /// <summary>
        /// 得到当前最大编号
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

        #region 获取列表
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="page">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public DataTable GetList(Pagination page, string queryJson)
        {
            DatabaseType dataTye = DatabaseType.Oracle;

            #region 查表
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

            #region  筛选条件
            var queryParam = JObject.Parse(queryJson);
            //列表类型
            if (!queryParam["ScaffoldType"].IsEmpty())
            {
                page.conditionJson += " and a.ScaffoldType = " + queryParam["ScaffoldType"].ToString();
            }
            //脚手架搭设类型
            if (!queryParam["SetupType"].IsEmpty())
            {
                page.conditionJson += " and a.SetupType = " + queryParam["SetupType"].ToString();
            }
            //搭设单位类型
            if (!queryParam["SetupCompanyType"].IsEmpty())
            {
                page.conditionJson += " and a.SetupCompanyType = " + queryParam["SetupCompanyType"].ToString();
            }
            //作业状态
            if (!queryParam["AuditState"].IsEmpty())
            {
                page.conditionJson += " and a.AuditState in (" + queryParam["AuditState"].ToString() + ")";
            }
            //搭设单位
            if (!queryParam["SetupCompanyId"].IsEmpty() && !queryParam["SetupCompanyCode"].IsEmpty())
            {
                page.conditionJson += string.Format(" and ((a.setupcompanyid in(select departmentid from base_department where encode like '{0}%'))  or (a.outprojectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", queryParam["SetupCompanyCode"].ToString(), queryParam["SetupCompanyId"].ToString());
            }
            //搭设时间
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
                //如果列表类型为验收申请，则根据日期查实际搭设时间
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
                //如果列表类型为拆除申请，则根据日期查拆除时间
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
            //申请编号
            if (!queryParam["ApplyCode"].IsEmpty())
            {
                page.conditionJson += " and a.ApplyCode = '" + queryParam["ApplyCode"].ToString() + "'";
            }

            if (!queryParam["ViewRange"].IsEmpty())
            {
                var user = OperatorProvider.Provider.Current();
                //本人
                if (queryParam["ViewRange"].ToString().ToLower() == "self")
                {
                    page.conditionJson += string.Format(" and a.ApplyUserId='{0}'", user.UserId);
                }
                else if (queryParam["ViewRange"].ToString().ToLower() == "selfaudit" || queryParam["ViewRange"].ToString().ToLower() == "selfapprove" || queryParam["ViewRange"].ToString().ToLower() == "selfconfirm")
                {
                    string strCondition = "";
                    switch (queryParam["ViewRange"].ToString().ToLower())
                    {
                        case "selfaudit":  //待操作
                            strCondition = " and a.AuditState in(1,4,6)";
                            break;
                        case "selfapprove":  //待审核(批)
                            strCondition = " and a.AuditState in(1,6)";
                            break;
                        case "selfconfirm":  //待验收确认
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
                    //全部 排除其他人申请保存的数据
                    page.conditionJson += string.Format("  and a.id not in(select id from bis_scaffold where auditstate = 0 and  applyuserid != '{0}')", user.UserId);
                }
            }

            if (!queryParam["IsNoDismentle"].IsEmpty() && !queryParam["ScaffoldType"].IsEmpty())
            {
                if (queryParam["IsNoDismentle"].ToString().ToLower() == "true")
                {
                    if (queryParam["ScaffoldType"].ToString() == "0")
                    {
                        //验收申请时，选择搭设申请信息审核通过,验收申请审核未通过且未验收申请的数据
                        page.conditionJson += @" and a.id  in (
                                select id from bis_scaffold where scaffoldtype = 0 and auditstate = 3
                                  and id not in(select nvl(setupinfoid,'-') from bis_scaffold where scaffoldtype = 1 and auditstate in(0,1,3,4,6))
                              ) ";
                    }
                    if (queryParam["ScaffoldType"].ToString() == "1")
                    {
                        //拆除申请时，选择验收申请审核通过、拆除申请审核未通过且未申请拆除的数据
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
        #endregion

        #region 台账列表
        /// <summary>
        /// 台账列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetLedgerList(Pagination page, string queryJson, string authType)
        {
            DatabaseType dataTye = DatabaseType.Oracle;
            #region 数据权限
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var curUserId = user.UserId;
            //查看范围数据权限
            /**
             * 1.作业单位及子部门（下级）
             * 2.本人创建的高风险作业
             * 3.发包部门管辖的外包单位
             * 4.外包单位只能看本单位的
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
                            case "3"://本子部门
                                page.conditionJson += string.Format("  and ((a.setupcompanyid in(select departmentid from base_department where encode like '{0}%'))  or (a.outprojectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);
                                break;
                            case "4":
                                page.conditionJson += " and a.createuserorgcode='" + user.OrganizeCode + "'";
                                break;
                            case "app":
                                if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级"))
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
              即将搭设:搭设申请通过,实际开始时间为空，且验收未完成
              搭设中:搭设申请通过,实际开始时间不为空，实际结束时间为空，且验收未完成
              已搭设:搭设申请通过,实际开始时间不为空，实际结束时间不为空，且验收未完成
              在用:验收申请通过,且拆除申请未完成
              即将拆除:拆除申请通过,实际拆除开始时间为空
              拆除中:拆除申请通过,实际拆除结束时间为空，拆除开始时间不为空
              拆除完成:拆除申请通过,实际拆除结束时间不为空，拆除开始时间不为空
             * 
             */ 
            #region 查表
            page.p_kid = "a.id";
            page.p_fields = @"a.applycode,a.scaffoldtype,a.ledgertype,b.itemname as ledgertypename,a.outprojectname,a.outprojectid,a.setupcompanyname,a.actsetupstartdate,a.actsetupenddate,a.SetupStartDate,a.SetupEndDate,a.checkdate,a.dismentlecompanyname,a.dismentlestartdate,a.dismentleenddate,a.setupaddress,a.setupchargepersonids,a.measurecarryoutid,a.realitydismentlestartdate,a.realitydismentleenddate,a.createuserid,a.setupcompanytype,a.setupcompanyid,'' as isoperate,workoperate";
            page.p_tablename = @"v_scaffoldledger a left join base_dataitemdetail b on a.ledgertype=b.itemvalue and itemid =(select itemid from base_dataitem where itemcode='LedgerType')";
            #endregion

            #region  筛选条件
            var queryParam = JObject.Parse(queryJson);

            //搭设或拆除单位
            if (!queryParam["SetupCompanyId"].IsEmpty() && !queryParam["SetupCompanyCode"].IsEmpty())
            {
                page.conditionJson += string.Format("  and ((a.setupcompanyid in(select departmentid from base_department where encode like '{0}%'))  or (a.outprojectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", queryParam["SetupCompanyCode"].ToString(), queryParam["SetupCompanyId"].ToString());
            }
            if (!queryParam["DismentleCompanyId"].IsEmpty() && !queryParam["DismentleCompanyCode"].IsEmpty())
            {
                page.conditionJson += string.Format(" and ((a.dismentlecompanyid in(select departmentid from base_department where encode like '{0}%'))  or (a.outprojectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", queryParam["DismentleCompanyCode"].ToString(), queryParam["DismentleCompanyId"].ToString());
            }
            //实际搭设时间
            if (!queryParam["ActSetupStartDate"].IsEmpty())
            {
                page.conditionJson += string.Format(" and a.ActSetupStartDate >= to_date('{0}','yyyy-MM-dd')", queryParam["ActSetupStartDate"].ToString());
            }
            if (!queryParam["ActSetupEndDate"].IsEmpty())
            {
                page.conditionJson += string.Format(" and a.ActSetupEndDate <= to_date('{0}','yyyy-MM-dd')", Convert.ToDateTime(queryParam["ActSetupEndDate"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));

            }
            //拆除时间
            if (!queryParam["DismentleStartDate"].IsEmpty())
            {
                page.conditionJson += string.Format(" and a.dismentlestartdate >= to_date('{0}','yyyy-MM-dd')", queryParam["DismentleStartDate"].ToString());
            }
            if (!queryParam["DismentleEndDate"].IsEmpty())
            {
                page.conditionJson += string.Format(" and a.dismentleenddate <= to_date('{0}','yyyy-MM-dd')", Convert.ToDateTime(queryParam["DismentleEndDate"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            }
            //验收时间
            if (!queryParam["CheckStartDate"].IsEmpty())
            {
                page.conditionJson += string.Format(" and to_date(a.checkdate,'yyyy-MM-dd hh24:mi') >= to_date('{0}','yyyy-MM-dd')", queryParam["CheckStartDate"].ToString());
            }
            if (!queryParam["CheckEndDate"].IsEmpty())
            {
                page.conditionJson += string.Format(" and to_date(a.checkdate,'yyyy-MM-dd hh24:mi') <= to_date('{0}','yyyy-MM-dd')", Convert.ToDateTime(queryParam["CheckEndDate"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            }
            //状态
            if (!queryParam["LedgerType"].IsEmpty())
            {
                page.conditionJson += " and a.LedgerType = " + queryParam["LedgerType"].ToString();
            }
            //搭设中和拆除中
            if (!queryParam["Working"].IsEmpty())
            {
                page.conditionJson += " and a.LedgerType in(1,5)";
            }
            //工程名称
            if (!queryParam["OutProjectName"].IsEmpty())
            {
                page.conditionJson += " and a.OutProjectName like '%" + queryParam["OutProjectName"].ToString() + "%'";
            }
            //申请编号
            if (!queryParam["applynumber"].IsEmpty())
            {
                page.conditionJson += " and a.applycode like '%" + queryParam["applynumber"].ToString() + "%'";
            }
            #endregion

            //return this.BaseRepository().FindTableByProcPager(page, dataTye);
            var data = this.BaseRepository().FindTableByProcPager(page, dataTye);
            #region 操作权限
            if (data != null)
            {
                string strRole = dataitemdetailservice.GetItemValue(user.OrganizeId, "LedgerSendDept");//发包部门角色
                string strManageRole = dataitemdetailservice.GetItemValue(user.OrganizeId, "LedgerManageDept");//安全主管部门监管角色
                string strWorkRole = dataitemdetailservice.GetItemValue(user.OrganizeId, "LedgerWorkDept");//作业单位
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    string str = "0";
                    string buildDutyUserId = data.Rows[i]["setupchargepersonids"].ToString();
                    string dismantleDutyUserId = data.Rows[i]["measurecarryoutid"].ToString();
                    string applyUserId = data.Rows[i]["createuserid"].ToString();
                    string outprojectid = data.Rows[i]["outprojectid"].ToString();//工程id
                    string workDeptType = data.Rows[i]["setupcompanytype"].ToString();
                    string workdeptid = data.Rows[i]["setupcompanyid"].ToString();//作业单位id
                    var dept = new OutsouringengineerService().GetEntity(outprojectid); //获取工程id对应的责任部门
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
                    if (str != "1" && (curUserId == buildDutyUserId || curUserId == applyUserId || curUserId == dismantleDutyUserId))//作业负责人或申请人
                    {
                        str = "1";
                    }
                    if (str != "1" && dept != null)
                    {
                        if (workDeptType == "1")//外包单位
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
            ScaffoldEntity entity = GetEntity(keyValue);
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
                        highriskcommonapplyservice.GetExecutedept(entity.SetupCompanyType.ToString(), entity.SetupCompanyId, entity.OutProjectId, out executedept);//获取执行部门
                        string createdetpid = departmentservice.GetEntityByCode(entity.CreateUserDeptCode).IsEmpty() ? "" : departmentservice.GetEntityByCode(entity.CreateUserDeptCode).DepartmentId; //获取创建部门ID
                        string outsouringengineerdept = string.Empty;
                        highriskcommonapplyservice.GetOutsouringengineerDept(entity.SetupCompanyId, out outsouringengineerdept);
                        string accountstr = powerCheck.GetApproveUserAccount(dr["id"].ToString(), entity.Id, "", entity.SpecialtyType, executedept, outsouringengineerdept, createdetpid, "", ""); //获取审核人账号
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
                if (entity.InvestigateState == 3)
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
                        highriskcommonapplyservice.GetExecutedept(entity.SetupCompanyType.ToString(), entity.SetupCompanyId, entity.OutProjectId, out executedept);//获取执行部门
                        string createdetpid = departmentservice.GetEntityByCode(entity.CreateUserDeptCode).IsEmpty()?"": departmentservice.GetEntityByCode(entity.CreateUserDeptCode).DepartmentId; //获取创建部门ID
                        string outsouringengineerdept = string.Empty;
                        highriskcommonapplyservice.GetOutsouringengineerDept(entity.SetupCompanyId, out outsouringengineerdept);
                        string accountstr = powerCheck.GetApproveUserAccount(dr["id"].ToString(), entity.Id, "", entity.SpecialtyType, executedept, outsouringengineerdept, createdetpid, "", ""); //获取审核人账号
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
                        if (entity.InvestigateState == 3)
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

        #region 获取脚手架搭设和拆除
        /// <summary>
        /// 获取脚手架搭设和拆除
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetSelectPageList(Pagination pagination, string queryJson)
        {
            var user = OperatorProvider.Provider.Current();
            DatabaseType dataTye = DatabaseType.Oracle;
            var queryParam = JObject.Parse(queryJson);
            //脚手架搭设类型
            if (!queryParam["checktype"].IsEmpty())
            {
                var checktype = queryParam["checktype"].ToString();
                if (checktype == "-1")//搭设
                {
                    pagination.conditionJson += "  and LedgerType in('0','1')";
                }
                else if (checktype == "-2")//拆除
                {
                    pagination.conditionJson += "  and LedgerType in('4','5')";
                }
            }
            //作业单位
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
            //工程名称
            if (!queryParam["engineeringname"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and outprojectname='{0}'", queryParam["engineeringname"].ToString());
            }
            return this.BaseRepository().FindTableByProcPager(pagination, dataTye);
        }
        #endregion

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ScaffoldEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }


        /// <summary>
        /// 获取单位
        /// </summary>
        /// <param name="deptid">部门id</param>
        /// <returns></returns>
        public DepartmentEntity GetDutyDept(string deptid, string projectid, string checkdeptid, string approvedeptid = "")
        {
            DepartmentEntity dept = new DepartmentEntity();
            dept = departmentservice.GetEntity(deptid);
            Operator currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (checkdeptid == "-2")
            {
                var cbsentity = departmentservice.GetList().Where(t => t.Description == "外包工程承包商" && t.OrganizeId == currUser.OrganizeId).FirstOrDefault();
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
                while (dept.Nature != "部门")
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
        /// 获取人员
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

                        if (item.RoleName.Contains("专工") && flowrolename.Split(',').Union(item.RoleName.Split(',')).Count() == (flowrolename.Split(',').Count() + item.RoleName.Split(',').Count() - 1)) //如果用户拥有专工角色而且还有审核角色中的其他一个就不需要判断专业
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
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
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
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="model">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, ScaffoldModel model)
        {
            var res = DbFactory.Base().BeginTrans();
            try
            {

                Repository<ScaffoldEntity> repScaffold = new Repository<ScaffoldEntity>(DbFactory.Base());
                ScaffoldEntity entity = repScaffold.FindEntity(keyValue);
                //新增
                if (entity == null)
                {
                    entity = new ScaffoldEntity();
                    entity.Create(keyValue);
                    //实体赋值
                    this.copyProperties(entity, model);
                    //生成编码
                    entity.ApplyCode = string.IsNullOrEmpty(entity.SetupInfoCode) ? this.GetMaxCode() : entity.SetupInfoCode;
                    //添加操作
                    res.Insert(entity);


                    //处理验收申请项目问题 申请保存时
                    if (entity.ScaffoldType == 1 && (model.ScaffoldProjects == null || model.ScaffoldProjects.Count == 0))
                    {
                        //检查项目表是否有值
                        List<ScaffoldprojectEntity> projects = scaffoldprojectservice.GetList(keyValue);
                        if (projects == null || projects.Count == 0)
                        {

                            //如没有则从配置表中代入
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
                    //编辑 
                    entity.Modify(keyValue);
                    //实体赋值
                    this.copyProperties(entity, model);
                    //更新操作
                    res.Update(entity);
                }

                //添加或更新架体规格 先删除再添加
                res.Delete<ScaffoldspecEntity>(t => t.ScaffoldId == entity.Id);
                foreach (var spec in model.ScaffoldSpecs)
                {
                    spec.ScaffoldId = entity.Id;
                    spec.Create();
                    res.Insert(spec);
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

                //添加或更新验收项目 验收申请时才处理
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
        /// 更新业务表、审核表、验收项目
        /// </summary>
        /// <param name="scaffoldEntity">业务主表实体</param>
        /// <param name="auditEntity">审核表实体</param>
        /// <param name="projects">验收项目 ScaffoldType=1 时才有</param>
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
        /// 从源实体给目标实体属性赋值
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="source">源</param>
        private void copyProperties(ScaffoldEntity target, ScaffoldModel source)
        {
            //搭设申请
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
            //验收存入部门
            target.ActSetupStartDate = source.ActSetupStartDate;
            target.ActSetupEndDate = source.ActSetupEndDate;
            //拆除存入部门 
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
            //信息关联ID
            target.SetupInfoId = source.SetupInfoId;
            target.SetupInfoCode = source.SetupInfoCode;
            target.SetupType = source.SetupType;
            target.ScaffoldType = source.ScaffoldType;
            target.AuditState = source.AuditState;
            //审核流程信息
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
