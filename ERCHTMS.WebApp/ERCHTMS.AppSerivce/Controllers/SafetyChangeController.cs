using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.Desktop;
using ERCHTMS.Busines.HighRiskWork;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Busines.PersonManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.RiskDatabase;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Entity.PersonManage;
using ERCHTMS.Entity.PublicInfoManage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ERCHTMS.Busines.JPush;
using ERCHTMS.Cache;
using ERCHTMS.Busines.HighRiskWork;
using BSFramework.Util.Extension;

namespace ERCHTMS.AppSerivce.Controllers
{

    /// <summary>
    /// 安全设施变动(验收)
    /// </summary>
    public class SafetyChangeController : BaseApiController
    {
        private SafetychangeBLL safetychangebll = new SafetychangeBLL();
        private PeopleReviewBLL peoplereviewbll = new PeopleReviewBLL();
        private ScaffoldauditrecordBLL scaffoldauditrecordbll = new ScaffoldauditrecordBLL();
        private HighRiskRecordBLL highriskrecordbll = new HighRiskRecordBLL();
        private DataItemCache dataItemCache = new DataItemCache();
        private ScaffoldBLL scaffoldbll = new ScaffoldBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private FireWaterBLL firewaterbll = new FireWaterBLL();
        private DepartmentBLL departmentbll = new DepartmentBLL();
        private HighRiskCommonApplyBLL highriskcommonapplybll = new HighRiskCommonApplyBLL();
        private ManyPowerCheckBLL manypowercheckbll = new ManyPowerCheckBLL();
        private UserBLL userbll = new UserBLL();

        #region 申请列表
        /// <summary>
        /// 申请列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetApplyList([FromBody]JObject json)
        {
            try
            {
                string path = new DataItemDetailBLL().GetItemValue("imgUrl");
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string starttime = dy.data.starttime; //开始时间
                string endtime = dy.data.endtime;//结束时间
                string applyno = dy.data.applyno;
                string applytype = dy.data.applytype;
                string status = dy.data.status;
                string actiontype = dy.data.actiontype;//0全部 1 我的
                long pageIndex = dy.data.pageindex;
                long pageSize = dy.data.pagesize;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator currUser = OperatorProvider.Provider.Current();
                Pagination pagination = new Pagination();
                pagination.page = int.Parse(pageIndex.ToString());
                pagination.rows = int.Parse(pageSize.ToString());
                pagination.p_kid = "t.id applyid";
                pagination.p_fields = @"t.createuserid,
                                           t.createdate,t.changetype,t.projectname,t.changename,t.workunittype,t.projectid,t.createuserdeptcode,
                                           t.applyunit,t.applychangetime,t.returntime,t.applytype,
                                           t.applyunitid,t.applypeople,t.applypeopleid,
                                           t.applytime, t.workunit,t.workunitid,t.applyno,
                                           t.iscommit,t.isaccepover,t.isapplyover,
                                           t.flowdept,t.nodename,t.nodeid,t.isaccpcommit,
                                           t.flowrole,t.flowdeptname,t.flowrolename,t.acceptime,t.workplace,'' as approveusername,'' as approveuseraccount,b.outtransferuseraccount,b.intransferuseraccount,t.flowremark,t.specialtytype,t.accspecialtytype,
                                        case 
                                                 when t.iscommit=0 then 0
                                                 when t.iscommit=1 and (t.isapplyover=0 or t.isapplyover is null)  then 1
                                                 when t.iscommit=1 and t.isapplyover=2  then 2
                                                 when t.iscommit=1 and t.isapplyover=1 and isaccpcommit=0 and (t.isaccepover=0 or t.isaccepover is null) then 3
                                                 when t.iscommit=1 and t.isapplyover=1 and isaccpcommit=1 and (t.isaccepover=0 or t.isaccepover is null) then 4
                                                 when t.iscommit=1 and t.isapplyover=1 and isaccpcommit=1 and t.isaccepover=2 then 5
                                                 when t.iscommit=1 and t.isapplyover=1 and isaccpcommit=1 and t.isaccepover=1 then 6 else 7 end applystate,
                                         case 
                                                 when t.iscommit=0 then '变动申请中'
                                                 when t.iscommit=1 and (t.isapplyover=0 or t.isapplyover is null) then '变动审核(批)中'
                                                 when t.iscommit=1 and t.isapplyover=2  then '变动审核(批)未通过'
                                                 when t.iscommit=1 and t.isapplyover=1 and isaccpcommit=0 and (t.isaccepover=0 or t.isaccepover is null) then '变动审核(批)通过待验收'
                                                 when t.iscommit=1 and t.isapplyover=1 and isaccpcommit=1 and (t.isaccepover=0 or t.isaccepover is null) then '验收审核中'
                                                 when t.iscommit=1 and t.isapplyover=1 and isaccpcommit=1 and t.isaccepover=2 then '验收审核(批)未通过'
                                                 when t.iscommit=1 and t.isapplyover=1 and isaccpcommit=1 and t.isaccepover=1 then '验收审核(批)通过' else '' end applystatename";
                pagination.p_tablename = @"   bis_safetychange t left join (select recid,flowid,outtransferuseraccount,intransferuseraccount,row_number()  over(partition by recid,flowid order by createdate desc) as num from BIS_TRANSFERRECORD where disable=0) b on t.id=b.recid and t.nodeid=b.flowid and b.num=1";
                pagination.sidx = "t.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                pagination.conditionJson = "1=1";

                string role = currUser.RoleName;
                string deptId = string.Empty;
                string deptName = string.Empty;

                //公司级用户取机构对象
                if (role.Contains("公司级用户"))
                {
                    deptId = currUser.OrganizeId;  //机构ID
                    deptName = currUser.OrganizeName;//机构名称
                }
                else
                {
                    deptId = currUser.DeptId; //部门ID
                    deptName = currUser.DeptName; //部门ID
                }
                if (actiontype == "0")
                {
                    if (currUser.IsSystem)
                    {

                    }
                    else if (currUser.RoleName.Contains("厂级部门用户") || currUser.RoleName.Contains("公司级用户"))
                    {
                        pagination.conditionJson += " and t.createuserorgcode='" + currUser.OrganizeCode + "'";
                    }
                    else if (currUser.RoleName.Contains("承包商级用户") || currUser.RoleName.Contains("班组级用户"))
                    {
                        pagination.conditionJson += " and workunitcode='" + currUser.DeptCode + "'";
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(" and ((workunitcode in(select encode from base_department where encode like '{0}%'))  or (projectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", currUser.DeptCode, currUser.DeptId);
                    }
                }
                else
                {
                    string strWhere = string.Empty;


                    strWhere += string.Format(@"  select distinct a.id from bis_safetychange a  where a.applypeopleid ='{0}' or a.acceppeopleid ='{0}' ", currUser.UserId);
                    pagination.conditionJson += string.Format(" and t.id in ({0})", strWhere);

                }

                string queryJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    st = starttime,
                    et = endtime,
                    status = status,
                    ApplyType = applytype,
                    applyno = applyno
                });
                DataTable dt = safetychangebll.GetPageList(pagination, queryJson);
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    var checkremark = dt.Rows[i]["flowremark"].ToString();
                //    string type = checkremark != "1" ? "0" : "1";
                //    string specialtytype = dt.Rows[i]["specialtytype"].ToString();
                //    if (dt.Rows[i]["iscommit"].ToString() == "1" && dt.Rows[i]["isapplyover"].ToString() == "1")
                //    {
                //        specialtytype = dt.Rows[i]["accspecialtytype"].ToString();
                //    }
                //    string str = new ScaffoldBLL().GetUserName(dt.Rows[i]["flowdept"].ToString(), dt.Rows[i]["flowrolename"].ToString(), type, specialtytype);
                //    dt.Rows[i]["approveusername"] = str.Split('|')[0];
                //}
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string executedept = string.Empty;
                    highriskcommonapplybll.GetExecutedept(dt.Rows[i]["workunittype"].ToString(), dt.Rows[i]["workunitid"].ToString(), dt.Rows[i]["projectid"].ToString(), out executedept); //获取执行部门
                    string createdetpid = departmentbll.GetEntityByCode(dt.Rows[i]["createuserdeptcode"].ToString()).IsEmpty() ? "" : departmentbll.GetEntityByCode(dt.Rows[i]["createuserdeptcode"].ToString()).DepartmentId; //获取创建部门
                    string outsouringengineerdept = string.Empty;
                    highriskcommonapplybll.GetOutsouringengineerDept(dt.Rows[i]["workunitid"].ToString(), out outsouringengineerdept);
                    string str = manypowercheckbll.GetApproveUserAccount(dt.Rows[i]["nodeid"].ToString(), dt.Rows[i]["applyid"].ToString(), "", dt.Rows[i]["specialtytype"].ToString(), executedept, outsouringengineerdept, createdetpid, "", "");
                    string outtransferuseraccount = dt.Rows[i]["outtransferuseraccount"].IsEmpty() ? "" : dt.Rows[i]["outtransferuseraccount"].ToString();//转交申请人
                    string intransferuseraccount = dt.Rows[i]["intransferuseraccount"].IsEmpty() ? "" : dt.Rows[i]["intransferuseraccount"].ToString();//转交接收人
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
                    dt.Rows[i]["approveuseraccount"] = str;
                    DataTable dtuser = userbll.GetUserTable(str.Split(','));
                    string[] usernames = dtuser.AsEnumerable().Select(d => d.Field<string>("realname")).ToArray();
                    dt.Rows[i]["approveusername"] = usernames.Length > 0 ? string.Join(",", usernames) : "";
                }
                return new { code = 0, count = pagination.records, info = "获取数据成功", data = dt };

            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }
        #endregion

        #region 审批列表
        /// <summary>
        /// 审批列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetAuditList([FromBody]JObject json)
        {
            try
            {
                string path = new DataItemDetailBLL().GetItemValue("imgUrl");
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string actiontype = dy.data.actiontype;//0全部 1 我的
                long pageIndex = dy.data.pageindex;
                long pageSize = dy.data.pagesize;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator currUser = OperatorProvider.Provider.Current();
                Pagination pagination = new Pagination();
                pagination.page = int.Parse(pageIndex.ToString());
                pagination.rows = int.Parse(pageSize.ToString());
                pagination.p_kid = "t.id applyid";
                pagination.p_fields = @"t.createuserid,
                                           t.createdate,t.changetype,t.projectname,t.changename,t.workunittype,t.projectid,t.createuserdeptcode,
                                           t.applyunit,t.applychangetime,t.returntime,
                                           t.applyunitid,t.applypeople,t.applypeopleid,t.applytype,
                                           t.applytime, t.workunit,t.workunitid,t.applyno,
                                           t.iscommit,t.isaccepover,t.isapplyover,
                                           t.flowdept,t.nodename,t.nodeid,t.isaccpcommit,
                                           t.flowrole,t.flowdeptname,t.flowrolename,t.acceptime,t.workplace,'' as approveusername,'' as approveuseraccount,b.outtransferuseraccount,b.intransferuseraccount,t.flowremark,t.specialtytype,t.accspecialtytype,
                                        case 
                                                 when t.iscommit=0 then 0
                                                 when t.iscommit=1 and t.isapplyover=0  then 1
                                                 when t.iscommit=1 and t.isapplyover=2  then 2
                                                 when t.iscommit=1 and t.isapplyover=1 and isaccpcommit=0 and t.isaccepover=0 then 3
                                                 when t.iscommit=1 and t.isapplyover=1 and isaccpcommit=1 and t.isaccepover=0 then 4
                                                 when t.iscommit=1 and t.isapplyover=1 and isaccpcommit=1 and t.isaccepover=2 then 5
                                                 when t.iscommit=1 and t.isapplyover=1 and isaccpcommit=1 and t.isaccepover=1 then 6 else 7 end applystate,
                                         case 
                                                 when t.iscommit=0 then '变动申请中'
                                                 when t.iscommit=1 and t.isapplyover=0  then '变动审核(批)中'
                                                 when t.iscommit=1 and t.isapplyover=2  then '变动审核(批)未通过'
                                                 when t.iscommit=1 and t.isapplyover=1 and isaccpcommit=0 and t.isaccepover=0 then '变动审核(批)通过待验收'
                                                 when t.iscommit=1 and t.isapplyover=1 and isaccpcommit=1 and t.isaccepover=0 then '验收审核中'
                                                 when t.iscommit=1 and t.isapplyover=1 and isaccpcommit=1 and t.isaccepover=2 then '验收审核(批)未通过'
                                                 when t.iscommit=1 and t.isapplyover=1 and isaccpcommit=1 and t.isaccepover=1 then '验收审核(批)通过' else '' end applystatename";
                pagination.p_tablename = @"   bis_safetychange t left join (select recid,flowid,outtransferuseraccount,intransferuseraccount,row_number()  over(partition by recid,flowid order by createdate desc) as num from BIS_TRANSFERRECORD where disable=0) b on t.id=b.recid and t.nodeid=b.flowid and b.num=1";
                pagination.sidx = "t.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                pagination.conditionJson = "1=1";
                string role = currUser.RoleName;
                string deptId = string.Empty;
                string deptName = string.Empty;

                //公司级用户取机构对象
                if (role.Contains("公司级用户"))
                {
                    deptId = currUser.OrganizeId;  //机构ID
                    deptName = currUser.OrganizeName;//机构名称
                }
                else
                {
                    deptId = currUser.DeptId; //部门ID
                    deptName = currUser.DeptName; //部门ID
                }
                if (actiontype == "1")
                {
                    if (currUser.IsSystem)
                    {

                    }
                    else if (currUser.RoleName.Contains("厂级部门用户") || currUser.RoleName.Contains("公司级用户"))
                    {
                        pagination.conditionJson += " and t.createuserorgcode='" + currUser.OrganizeCode + "'";
                    }
                    else if (currUser.RoleName.Contains("承包商级用户") || currUser.RoleName.Contains("班组级用户"))
                    {
                        pagination.conditionJson += " and workunitcode='" + currUser.DeptCode + "'";
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(" and ((workunitcode in(select encode from base_department where encode like '{0}%'))  or (projectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", currUser.DeptCode, currUser.DeptId);
                    }
                    pagination.conditionJson += string.Format("  and ((t.iscommit ='1' and  t.isapplyover =1) or (t.iscommit ='1' and  t.isapplyover =1  and  t.isaccpcommit =1  and  t.isaccepover =1))");
                }
                else
                {
                    string strCondition = " and ((t.isapplyover =0 and  t.iscommit =1) or (t.isapplyover =1 and  t.iscommit =1 and  t.isaccpcommit =1 and t.isaccepover=0))";
                    DataTable data = safetychangebll.FindTable("select " + pagination.p_kid + "," + pagination.p_fields + " from " + pagination.p_tablename + " where " + pagination.conditionJson + strCondition);
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        string executedept = string.Empty;
                        highriskcommonapplybll.GetExecutedept(data.Rows[i]["workunittype"].ToString(), data.Rows[i]["workunitid"].ToString(), data.Rows[i]["projectid"].ToString(), out executedept);
                        string createdetpid = departmentbll.GetEntityByCode(data.Rows[i]["createuserdeptcode"].ToString()).IsEmpty() ? "" : departmentbll.GetEntityByCode(data.Rows[i]["createuserdeptcode"].ToString()).DepartmentId;
                        string outsouringengineerdept = string.Empty;
                        highriskcommonapplybll.GetOutsouringengineerDept(data.Rows[i]["workunitid"].ToString(), out outsouringengineerdept);
                        string str = manypowercheckbll.GetApproveUserAccount(data.Rows[i]["nodeid"].ToString(), data.Rows[i]["applyid"].ToString(), "", data.Rows[i]["specialtytype"].ToString(), executedept, outsouringengineerdept, createdetpid, "", "");
                        data.Rows[i]["approveuseraccount"] = str;
                    }
                    string[] applyids = data.Select("(outtransferuseraccount is null or outtransferuseraccount not like '%" + currUser.Account + ",%') and (approveuseraccount like '%" + currUser.Account + ",%' or intransferuseraccount like '%" + currUser.Account + ",%')").AsEnumerable().Select(d => d.Field<string>("applyid")).ToArray();

                    pagination.conditionJson += string.Format(" and t.id in ('{0}') {1}", string.Join("','", applyids), strCondition);

                }
                string queryJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                });
                var dt = safetychangebll.GetPageList(pagination, queryJson);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string executedept = string.Empty;
                    highriskcommonapplybll.GetExecutedept(dt.Rows[i]["workunittype"].ToString(), dt.Rows[i]["workunitid"].ToString(), dt.Rows[i]["projectid"].ToString(), out executedept); //获取执行部门
                    string createdetpid = departmentbll.GetEntityByCode(dt.Rows[i]["createuserdeptcode"].ToString()).IsEmpty() ? "" : departmentbll.GetEntityByCode(dt.Rows[i]["createuserdeptcode"].ToString()).DepartmentId; //获取创建部门
                    string outsouringengineerdept = string.Empty;
                    highriskcommonapplybll.GetOutsouringengineerDept(dt.Rows[i]["workunitid"].ToString(), out outsouringengineerdept);
                    string str = manypowercheckbll.GetApproveUserAccount(dt.Rows[i]["nodeid"].ToString(), dt.Rows[i]["applyid"].ToString(), "", dt.Rows[i]["specialtytype"].ToString(), executedept, outsouringengineerdept, createdetpid, "", "");
                    string outtransferuseraccount = dt.Rows[i]["outtransferuseraccount"].IsEmpty() ? "" : dt.Rows[i]["outtransferuseraccount"].ToString();//转交申请人
                    string intransferuseraccount = dt.Rows[i]["intransferuseraccount"].IsEmpty() ? "" : dt.Rows[i]["intransferuseraccount"].ToString();//转交接收人
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
                    dt.Rows[i]["approveuseraccount"] = str;
                    DataTable dtuser = userbll.GetUserTable(str.Split(','));
                    string[] usernames = dtuser.AsEnumerable().Select(d => d.Field<string>("realname")).ToArray();
                    dt.Rows[i]["approveusername"] = usernames.Length > 0 ? string.Join(",", usernames) : "";
                }
                return new { code = 0, count = pagination.records, info = "获取数据成功", data = dt };

            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }
        #endregion

        #region 获取详情
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetInfo([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string applyid = dy.applyid;

                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                var change = safetychangebll.GetEntity(applyid);
                List<ScaffoldauditrecordEntity> scalist = scaffoldauditrecordbll.GetApplyAuditList(applyid, 0);
                List<ScaffoldauditrecordEntity> scaAccplist = scaffoldauditrecordbll.GetApplyAuditList(applyid, 1);
                var files = new FileInfoBLL().GetFiles(applyid);//获取相关附件
                var accfiles = new FileInfoBLL().GetFiles(applyid + "1");//获取验收相关附件

                string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                foreach (DataRow dr in files.Rows)
                {
                    dr["filepath"] = dr["filepath"].ToString().Replace("~/", webUrl + "/");
                }
                foreach (DataRow dr in accfiles.Rows)
                {
                    dr["filepath"] = dr["filepath"].ToString().Replace("~/", webUrl + "/");
                }
                foreach (var item in scalist)
                {
                    item.AuditSignImg = string.IsNullOrWhiteSpace(item.AuditSignImg) ? "" : webUrl + item.AuditSignImg.ToString().Replace("../../", "/");
                }
                foreach (var item in scaAccplist)
                {
                    item.AuditSignImg = string.IsNullOrWhiteSpace(item.AuditSignImg) ? "" : webUrl + item.AuditSignImg.ToString().Replace("../../", "/");
                }
                if (!string.IsNullOrEmpty(change.SPECIALTYTYPE))
                {
                    change.SPECIALTYTYPENAME = !string.IsNullOrEmpty(change.SPECIALTYTYPE) ? scaffoldbll.getName(change.SPECIALTYTYPE, "SpecialtyType") : "";
                }
                if (!string.IsNullOrEmpty(change.ACCSPECIALTYTYPE))
                {
                    change.ACCSPECIALTYTYPENAME = !string.IsNullOrEmpty(change.ACCSPECIALTYTYPE) ? scaffoldbll.getName(change.ACCSPECIALTYTYPE, "SpecialtyType") : "";
                }
                var riskrecord = highriskrecordbll.GetList(applyid).ToList();

                string projectid = "";
                List<string> modulename = new List<string>();
                //0：电厂内部 1外包单位
                if (change.WORKUNITTYPE == "0")
                {
                    modulename.Add("(内部)设施变动申请审核");
                    modulename.Add("(内部)设施变动验收审核");
                }
                else
                {
                    modulename.Add("(外包)设施变动申请审核");
                    modulename.Add("(外包)设施变动验收审核");
                    projectid = change.PROJECTID;
                }

                bool isendflow = false;
                if ((change.ISCOMMIT == 1 && change.ISAPPLYOVER == 2) || (change.ISCOMMIT == 1 && change.ISAPPLYOVER == 1 && change.ISACCPCOMMIT == 1 && change.ISACCEPOVER == 2)
                    || (change.ISCOMMIT == 1 && change.ISAPPLYOVER == 1 && change.ISACCPCOMMIT == 1 && change.ISACCEPOVER == 1))
                {
                    isendflow = true;
                }
                var nodelist = safetychangebll.GetAppFlowList(change.ID, modulename, change.NodeId, isendflow, change.WORKUNITID, projectid, !string.IsNullOrEmpty(change.SPECIALTYTYPE) ? change.SPECIALTYTYPE : "");

                #region 获取执行情况

                IList<FireWaterCondition> conditionlist = firewaterbll.GetConditionList(applyid).OrderBy(t => t.CreateDate).ToList();
                foreach (var item in conditionlist)
                {
                    List<FileInfoEntity> filelist = fileinfobll.GetFileList(item.Id);
                    IList<ERCHTMS.Entity.HighRiskWork.ViewModel.Photo> piclist = new List<ERCHTMS.Entity.HighRiskWork.ViewModel.Photo>();
                    foreach (var temp in filelist)
                    {
                        ERCHTMS.Entity.HighRiskWork.ViewModel.Photo pic = new ERCHTMS.Entity.HighRiskWork.ViewModel.Photo();
                        pic.filename = temp.FileName;
                        pic.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + temp.FilePath.Substring(1);
                        pic.fileid = temp.FileId;
                        piclist.Add(pic);
                    }
                    item.piclist = piclist;
                    List<FileInfoEntity> filelist1 = fileinfobll.GetFileList(item.Id + "_02");
                    IList<ERCHTMS.Entity.HighRiskWork.ViewModel.Photo> tempfilelist = new List<ERCHTMS.Entity.HighRiskWork.ViewModel.Photo>();
                    foreach (var temp in filelist1)
                    {
                        ERCHTMS.Entity.HighRiskWork.ViewModel.Photo pic = new ERCHTMS.Entity.HighRiskWork.ViewModel.Photo();
                        pic.filename = temp.FileName;
                        pic.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + temp.FilePath.Substring(1);
                        pic.fileid = temp.FileId;
                        tempfilelist.Add(pic);
                    }
                    item.filelist = tempfilelist;
                }
                #endregion

                var data = new
                {
                    applyentity = change,
                    applyauditlist = scalist,
                    accpauditlist = scaAccplist,
                    Files = files,
                    accfiles = accfiles,
                    riskrecord = riskrecord,
                    checkflow = nodelist,
                    conditionlist = conditionlist
                };
                Dictionary<string, string> dict_props = new Dictionary<string, string>();
                //Id 转换前的列名  keyvalue 转换后的列名
                //dict_props.Add("Id", "keyvalue");

                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                    DateFormatString = "yyyy-MM-dd HH:mm", //格式化日期
                    //NullValueHandling = NullValueHandling.Ignore 值为空则在JSON中体现
                };
                return new { code = 0, info = "获取数据成功", count = 1, data = JObject.Parse(JsonConvert.SerializeObject(data, Formatting.None, settings)) };
                //return new { code = 0, count = 1, info = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {

                return new { code = -1, count = 0, info = ex.Message };
            }


        }
        #endregion

        #region 保存/提交--变动/验收申请
        /// <summary>
        /// 保存/提交--变动/验收申请
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SaveCommitApply()
        {
            try
            {
                //string res = json.Value<string>("json");
                string res = HttpContext.Current.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string deleteids = dy.deleteids;//删除附件id集合
                string applyentity = JsonConvert.SerializeObject(dy.applyentity);
                SafetychangeEntity change = JsonConvert.DeserializeObject<SafetychangeEntity>(applyentity);
                string riskrecordlist = res.Contains("riskrecord") ? JsonConvert.SerializeObject(dy.riskrecord) : "";
                var riskrecord = JsonConvert.DeserializeObject<List<HighRiskRecordEntity>>(riskrecordlist);
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator currUser = OperatorProvider.Provider.Current();
                change.ISACCEPOVER = 0;
                change.ISAPPLYOVER = 0;
                ManyPowerCheckEntity mpcEntity = null;
                string Type = dy.type;
                if (string.IsNullOrEmpty(change.ID))
                {
                    change.ID = Guid.NewGuid().ToString();
                }
                switch (Type)
                {
                    case "0":
                        highriskrecordbll.RemoveFormByWorkId(change.ID);
                        if (riskrecord != null)
                        {
                            var num = 0;
                            foreach (var item in riskrecord)
                            {
                                item.CreateDate = DateTime.Now.AddSeconds(-num);
                                item.WorkId = change.ID;
                                highriskrecordbll.SaveForm("", item);
                                num++;
                            }
                        }
                        if (change.ISCOMMIT == 0)
                        {
                            safetychangebll.SaveForm(change.ID, change);
                        }
                        else
                        {
                            //电厂内部审核流程
                            if (change.WORKUNITTYPE == "0")
                            {
                                string moduleName = "(内部)设施变动申请审核";
                                mpcEntity = peoplereviewbll.CheckAuditForNextByWorkUnit(currUser, moduleName, change.WORKUNITID, change.NodeId, false);
                            }
                            else
                            {
                                //外包审核流程
                                string moduleName = "(外包)设施变动申请审核";
                                mpcEntity = peoplereviewbll.CheckAuditForNextByOutsourcing(currUser, moduleName, change.WORKUNITID, change.NodeId, false, true, change.PROJECTID);
                            }

                            if (null != mpcEntity)
                            {
                                change.FLOWDEPT = mpcEntity.CHECKDEPTID;
                                change.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                                change.FLOWROLE = mpcEntity.CHECKROLEID;
                                change.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                                change.NodeId = mpcEntity.ID;
                                change.NodeName = mpcEntity.FLOWNAME;
                                change.FLOWREMARK = !string.IsNullOrEmpty(mpcEntity.REMARK) ? mpcEntity.REMARK : "";

                                string type = change.FLOWREMARK != "1" ? "0" : "1";
                                new ScaffoldBLL().SendMessage(change.FLOWDEPT, change.FLOWROLE, "ZY011", change.ID, "", "", type, !string.IsNullOrEmpty(change.SPECIALTYTYPE) ? change.SPECIALTYTYPE : "");
                            }
                            else
                            {
                                change.FLOWREMARK = "";
                                //未配置审核项
                                change.FLOWDEPT = "";
                                change.FLOWDEPTNAME = "";
                                change.FLOWROLE = "";
                                change.FLOWROLENAME = "";
                                change.NodeId = "";
                                change.NodeName = "已完结";
                                //entity.ISACCEPOVER = 1;
                                change.ISCOMMIT = 1;
                                change.ISAPPLYOVER = 1;//申请审核完成
                            }
                            safetychangebll.SaveForm(change.ID, change);
                        }
                        break;
                    case "1":
                        if (change.ISACCPCOMMIT == 0)
                        {
                            safetychangebll.SaveForm(change.ID, change);
                        }
                        else
                        {
                            //电厂内部验收审核流程
                            if (change.WORKUNITTYPE == "0")
                            {
                                string moduleName = "(内部)设施变动验收审核";
                                mpcEntity = peoplereviewbll.CheckAuditForNextByWorkUnit(currUser, moduleName, change.WORKUNITID, change.NodeId, false);
                            }
                            else
                            {
                                string moduleName = "(外包)设施变动验收审核";
                                //外包验收审核流程
                                mpcEntity = peoplereviewbll.CheckAuditForNextByOutsourcing(currUser, moduleName, change.WORKUNITID, change.NodeId, false, true, change.PROJECTID);
                            }

                            if (null != mpcEntity)
                            {
                                change.FLOWDEPT = mpcEntity.CHECKDEPTID;
                                change.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                                change.FLOWROLE = mpcEntity.CHECKROLEID;
                                change.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                                change.NodeId = mpcEntity.ID;
                                change.NodeName = mpcEntity.FLOWNAME;
                                change.ISCOMMIT = 1;
                                change.ISAPPLYOVER = 1;//申请审核完成
                                change.FLOWREMARK = !string.IsNullOrEmpty(mpcEntity.REMARK) ? mpcEntity.REMARK : "";

                                string type = change.FLOWREMARK != "1" ? "0" : "1";
                                new ScaffoldBLL().SendMessage(change.FLOWDEPT, change.FLOWROLE, "ZY012", change.ID, "", "", type, !string.IsNullOrEmpty(change.ACCSPECIALTYTYPE) ? change.ACCSPECIALTYTYPE : "");
                            }
                            else
                            {
                                change.FLOWREMARK = "";
                                //未配置审核项
                                change.FLOWDEPT = "";
                                change.FLOWDEPTNAME = "";
                                change.FLOWROLE = "";
                                change.FLOWROLENAME = "";
                                change.NodeId = "";
                                change.NodeName = "已完结";
                                //entity.ISACCEPOVER = 1;
                                change.ISCOMMIT = 1;
                                change.ISACCPCOMMIT = 1;
                                change.ISACCEPOVER = 1;//验收审核完成
                                change.ISAPPLYOVER = 1;//申请审核完成
                            }
                            safetychangebll.SaveForm(change.ID, change);
                        }
                        break;
                    default:
                        break;
                }

                if (!string.IsNullOrEmpty(deleteids))
                {
                    DeleteFile(deleteids);
                }
                HttpFileCollection files = HttpContext.Current.Request.Files;
                if (files.Count > 0)
                {
                    for (int i = 0; i < files.AllKeys.Length; i++)
                    {
                        HttpPostedFile file = files[i];
                        //原始文件名
                        string fileName = System.IO.Path.GetFileName(file.FileName);
                        long filesize = file.ContentLength;
                        string FileEextension = Path.GetExtension(fileName);
                        string fileGuid = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Guid.NewGuid().ToString();
                        string dir = new DataItemDetailBLL().GetItemValue("imgPath") + "\\Resource\\Upfile";
                        string newFileName = fileGuid + FileEextension;
                        string newFilePath = dir + "\\" + newFileName;
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }

                        FileInfoEntity fileInfoEntity = new FileInfoEntity();
                        if (!System.IO.File.Exists(newFilePath))
                        {
                            //保存文件
                            file.SaveAs(newFilePath);
                            //文件信息写入数据库
                            fileInfoEntity.Create();
                            fileInfoEntity.FileId = fileGuid;
                            fileInfoEntity.RecId = Type == "0" ? change.ID : change.ID + "1";
                            fileInfoEntity.FileName = fileName;
                            fileInfoEntity.FilePath = "~/Resource/Upfile/" + newFileName;
                            fileInfoEntity.FileSize = (Math.Round(decimal.Parse(filesize.ToString()) / decimal.Parse("1024"), 2)).ToString();//文件大小（kb）
                            fileInfoEntity.FileExtensions = FileEextension;
                            fileInfoEntity.FileType = FileEextension.TrimStart('.');
                            FileInfoBLL fileInfoBLL = new FileInfoBLL();
                            fileInfoBLL.SaveForm("", fileInfoEntity);
                        }
                    }
                }
                #region MyRegion
                #endregion


                return new { code = 0, count = 1, info = "提交成功" };
            }
            catch (Exception ex)
            {

                return new { code = -1, count = 0, info = ex.Message };
            }
        }
        #endregion

        #region 删除文件
        public bool DeleteFile(string fileInfoIds)
        {
            FileInfoBLL fileInfoBLL = new FileInfoBLL();
            bool result = false;

            if (!string.IsNullOrEmpty(fileInfoIds))
            {
                string ids = "";

                string[] strArray = fileInfoIds.Split(',');

                foreach (string s in strArray)
                {
                    ids += "'" + s + "',";
                    var entity = fileInfoBLL.GetEntity(s);
                    if (entity != null)
                    {
                        var filePath = HttpContext.Current.Server.MapPath(entity.FilePath);
                        if (File.Exists(filePath))
                            File.Delete(filePath);
                    }
                }

                if (!string.IsNullOrEmpty(ids))
                {
                    ids = ids.Substring(0, ids.Length - 1);
                }
                int count = fileInfoBLL.DeleteFileForm(ids);

                result = count > 0 ? true : false;
            }

            return result;
        }
        #endregion

        #region 审核/审批变动申请
        /// <summary>
        /// 审核/审批变动申请
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object AppSubmitAppLyAudit()
        {
            try
            {
                string res = HttpContext.Current.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

                string userId = dy.userid;
                string applyentity = JsonConvert.SerializeObject(dy.data.applyentity);
                SafetychangeEntity change = JsonConvert.DeserializeObject<SafetychangeEntity>(applyentity);

                string auditstr = JsonConvert.SerializeObject(dy.data.aiditentity);
                ScaffoldauditrecordEntity auditentity = JsonConvert.DeserializeObject<ScaffoldauditrecordEntity>(auditstr);
                string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");

                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator currUser = OperatorProvider.Provider.Current();
                string moduleName = string.Empty;
                if (change.WORKUNITTYPE == "0")
                {
                    moduleName = "(内部)设施变动申请审核";
                }
                else
                {
                    moduleName = "(外包)设施变动申请审核";
                }
                auditentity.FlowId = change.NodeId;
                //更改申请信息状态
                ManyPowerCheckEntity mpcEntity = peoplereviewbll.CheckAuditForNextByWorkUnit(currUser, moduleName, change.WORKUNITID, change.NodeId, false);
                //同意进行下一步
                if (auditentity.AuditState == 0)
                {
                    //下一步流程不为空
                    if (null != mpcEntity)
                    {
                        change.FLOWDEPT = mpcEntity.CHECKDEPTID;
                        change.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                        change.FLOWROLE = mpcEntity.CHECKROLEID;
                        change.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                        change.NodeId = mpcEntity.ID;
                        change.NodeName = mpcEntity.FLOWNAME;
                        change.ISCOMMIT = 1;
                        change.FLOWREMARK = !string.IsNullOrEmpty(mpcEntity.REMARK) ? mpcEntity.REMARK : "";

                        string type = change.FLOWREMARK != "1" ? "0" : "1";
                        new ScaffoldBLL().SendMessage(change.FLOWDEPT, change.FLOWROLE, "ZY011", change.ID, "", "", type, !string.IsNullOrEmpty(change.SPECIALTYTYPE) ? change.SPECIALTYTYPE : "");
                    }
                    else
                    {
                        change.FLOWREMARK = "";
                        change.FLOWDEPT = " ";
                        change.FLOWDEPTNAME = " ";
                        change.FLOWROLE = " ";
                        change.FLOWROLENAME = " ";
                        //change.NodeId = " ";
                        change.NodeName = "已完结";
                        change.ISAPPLYOVER = 1;
                        change.ISCOMMIT = 1;
                    }
                }
                else
                {
                    change.FLOWREMARK = "";
                    change.FLOWDEPT = " ";
                    change.FLOWDEPTNAME = " ";
                    change.FLOWROLE = " ";
                    change.FLOWROLENAME = " ";
                    //change.NodeId = " ";
                    change.NodeName = "已完结";
                    change.ISAPPLYOVER = 2;
                    change.ISCOMMIT = 1;

                    //审批不通过,推消息到申请人
                    var high = safetychangebll.GetEntity(change.ID);
                    if (high != null)
                    {
                        UserEntity userEntity = new UserBLL().GetEntity(high.CREATEUSERID);
                        if (userEntity != null)
                        {
                            JPushApi.PushMessage(userEntity.Account, userEntity.RealName, "ZY013", change.ID);
                        }
                    }
                }
                safetychangebll.SaveForm(change.ID, change);
                auditentity.ScaffoldId = change.ID;
                auditentity.AuditDate = DateTime.Now;
                auditentity.AuditSignImg = string.IsNullOrWhiteSpace(auditentity.AuditSignImg) ? "" : auditentity.AuditSignImg.Replace(webUrl, "").ToString();
                HttpFileCollection files = HttpContext.Current.Request.Files;
                if (files.Count > 0)
                {
                    for (int i = 0; i < files.AllKeys.Length; i++)
                    {
                        HttpPostedFile file = files[i];
                        string fileOverName = System.IO.Path.GetFileName(file.FileName);
                        string fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                        string FileEextension = Path.GetExtension(file.FileName);
                        if (fileName == auditentity.Id)
                        {
                            string dir = new DataItemDetailBLL().GetItemValue("imgPath") + "\\Resource\\sign";
                            string newFileName = fileName + FileEextension;
                            string newFilePath = dir + "\\" + newFileName;
                            file.SaveAs(newFilePath);
                            auditentity.AuditSignImg = "/Resource/sign/" + fileOverName;
                            break;
                        }
                    }
                }
                auditentity.Id = null;
                scaffoldauditrecordbll.SaveForm("", auditentity);
                return new { code = 0, count = 1, info = "提交成功" };
            }
            catch (Exception ex)
            {

                return new { code = -1, count = 0, info = ex.Message };
            }

        }
        #endregion

        #region 审核/审批验收申请
        /// <summary>
        /// 审核/审批验收申请
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object AppSubmitAccpAudit()
        {
            try
            {
                string res = HttpContext.Current.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string applyentity = JsonConvert.SerializeObject(dy.data.applyentity);
                SafetychangeEntity change = JsonConvert.DeserializeObject<SafetychangeEntity>(applyentity);

                string auditstr = JsonConvert.SerializeObject(dy.data.aiditentity);
                ScaffoldauditrecordEntity auditentity = JsonConvert.DeserializeObject<ScaffoldauditrecordEntity>(auditstr);
                string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");

                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator currUser = OperatorProvider.Provider.Current();
                string moduleName = string.Empty;
                if (change.WORKUNITTYPE == "0")
                {
                    moduleName = "(内部)设施变动验收审核";
                }
                else
                {
                    moduleName = "(外包)设施变动验收审核";
                }
                auditentity.FlowId = change.NodeId;
                ManyPowerCheckEntity mpcEntity = peoplereviewbll.CheckAuditForNextByWorkUnit(currUser, moduleName, change.WORKUNITID, change.NodeId, false);
                //同意进行下一步
                if (auditentity.AuditState == 0)
                {
                    //下一步流程不为空
                    if (null != mpcEntity)
                    {
                        change.FLOWDEPT = mpcEntity.CHECKDEPTID;
                        change.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                        change.FLOWROLE = mpcEntity.CHECKROLEID;
                        change.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                        change.NodeId = mpcEntity.ID;
                        change.NodeName = mpcEntity.FLOWNAME;
                        change.ISCOMMIT = 1;
                        change.ISACCPCOMMIT = 1;
                        change.FLOWREMARK = !string.IsNullOrEmpty(mpcEntity.REMARK) ? mpcEntity.REMARK : "";

                        string type = change.FLOWREMARK != "1" ? "0" : "1";
                        new ScaffoldBLL().SendMessage(change.FLOWDEPT, change.FLOWROLE, "ZY012", change.ID, "", "", type, !string.IsNullOrEmpty(change.ACCSPECIALTYTYPE) ? change.ACCSPECIALTYTYPE : "");
                    }
                    else
                    {
                        change.FLOWREMARK = "";
                        change.FLOWDEPT = " ";
                        change.FLOWDEPTNAME = " ";
                        change.FLOWROLE = " ";
                        change.FLOWROLENAME = " ";
                        //change.NodeId = " ";
                        change.NodeName = "已完结";
                        change.ISAPPLYOVER = 1;
                        change.ISCOMMIT = 1;
                        change.ISACCPCOMMIT = 1;
                        change.ISACCEPOVER = 1;
                    }
                }
                else
                {
                    change.FLOWREMARK = "";
                    change.FLOWDEPT = " ";
                    change.FLOWDEPTNAME = " ";
                    change.FLOWROLE = " ";
                    change.FLOWROLENAME = " ";
                    //change.NodeId = " ";
                    change.NodeName = "已完结";
                    change.ISAPPLYOVER = 1;
                    change.ISCOMMIT = 1;
                    change.ISACCPCOMMIT = 1;
                    change.ISACCEPOVER = 2;

                    //审批不通过,推消息到申请人
                    var high = safetychangebll.GetEntity(change.ID);
                    if (high != null)
                    {
                        UserEntity userEntity = new UserBLL().GetEntity(high.CREATEUSERID);
                        if (userEntity != null)
                        {
                            JPushApi.PushMessage(userEntity.Account, userEntity.RealName, "ZY014", change.ID);
                        }
                    }
                }
                safetychangebll.SaveForm(change.ID, change);
                auditentity.ScaffoldId = change.ID;
                auditentity.AuditDate = DateTime.Now;
                auditentity.AuditSignImg = string.IsNullOrWhiteSpace(auditentity.AuditSignImg) ? "" : auditentity.AuditSignImg.Replace(webUrl, "").ToString();
                HttpFileCollection files = HttpContext.Current.Request.Files;
                if (files.Count > 0)
                {
                    for (int i = 0; i < files.AllKeys.Length; i++)
                    {
                        HttpPostedFile file = files[i];
                        string fileOverName = System.IO.Path.GetFileName(file.FileName);
                        string fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                        string FileEextension = Path.GetExtension(file.FileName);
                        if (fileName == auditentity.Id)
                        {
                            string dir = new DataItemDetailBLL().GetItemValue("imgPath") + "\\Resource\\sign";
                            string newFileName = fileName + FileEextension;
                            string newFilePath = dir + "\\" + newFileName;
                            file.SaveAs(newFilePath);
                            auditentity.AuditSignImg = "/Resource/sign/" + fileOverName;
                            break;
                        }
                    }
                }
                auditentity.Id = null;
                scaffoldauditrecordbll.SaveForm("", auditentity);
                return new { code = 0, count = 1, info = "提交成功" };
            }
            catch (Exception ex)
            {

                return new { code = -1, count = 0, info = ex.Message };
            }

        }
        #endregion

        #region 获取作业许可状态
        /// <summary>
        /// 获取作业许可状态 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetStateList([FromBody]JObject json)
        {
            try
            {
                string str = @"[{ ItemValue: '0', ItemName: '变动申请中' },
                                { ItemValue: '1', ItemName: '变动审核(批)中'},
                                { ItemValue: '2', ItemName: '变动审核(批)未通过' },
                                { ItemValue: '3', ItemName: '变动审核(批)通过待验收' },
                                { ItemValue: '4', ItemName: '验收审核中' },
                                { ItemValue: '5', ItemName: '验收审核(批)未通过' },
                                { ItemValue: '6', ItemName: '验收审核(批)通过' }
                            ]";
                var data = JsonConvert.DeserializeObject<List<itemClass>>(str);
                return new { code = 0, count = data.Count, info = "获取数据成功", data = data }; ;
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }
        #endregion

        #region 获取状态
        [HttpPost]
        public object GetData([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            try
            {
                string type = dy.type;
                string str = string.Empty;
                if (type == "1")
                {
                    str = @"[{ ItemValue: '防护栏杆', ItemName: '防护栏杆' },
                                { ItemValue: '临时栏杆', ItemName: '临时栏杆'},
                                { ItemValue: '楼梯', ItemName: '楼梯' },
                                { ItemValue: '爬梯', ItemName: '爬梯' },
                                { ItemValue: '平台格栅', ItemName: '平台格栅' }
                            ]";
                }
                else
                {
                    str = @"[{ ItemValue: '移动', ItemName: '移动' },
                                { ItemValue: '临时拆除', ItemName: '临时拆除'},
                                { ItemValue: '改变', ItemName: '改变' },
                                { ItemValue: '取消', ItemName: '取消' }
                            ]";
                }
                var data = JsonConvert.DeserializeObject<List<itemClass>>(str);
                return new { code = 0, count = data.Count, info = "获取数据成功", data = data }; ;
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }
        #endregion


        #region 台账
        /// <summary>
        /// 获取变动状态
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetLedgerType([FromBody]JObject json)
        {
            try
            {
                string str = @"[{ ItemValue: '0', ItemName: '即将变动' },
                                { ItemValue: '1', ItemName: '变动中'},
                                { ItemValue: '2', ItemName: '已恢复' }
                            ]";
                var data = JsonConvert.DeserializeObject<List<itemClass>>(str);
                return new { code = 0, count = data.Count, info = "获取数据成功", data = data }; ;
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }

        /// <summary>
        /// 得到作业台账列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetLedgerList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                var dy = JsonConvert.DeserializeAnonymousType(res, new
                {
                    userid = string.Empty,
                    data = new
                    {
                        pagenum = 1,
                        pagesize = 20,
                        changename = string.Empty,
                        workunitcode = string.Empty,
                        workunitid = string.Empty,
                        st = string.Empty,
                        et = string.Empty,
                        ledgertype = string.Empty,
                        applynumber = string.Empty
                    }
                });
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, info = "请求失败,请登录!" };
                }

                Pagination pagination = new Pagination();
                pagination.page = dy.data.pagenum;
                pagination.rows = dy.data.pagesize;
                pagination.sidx = "createdate";//排序字段
                pagination.sord = "desc";//排序方式
                var list = safetychangebll.GetLedgerList(pagination, JsonConvert.SerializeObject(dy.data));
                int count = pagination.records;
                pagination.rows = 100000;
                var historyCount = safetychangebll.GetLedgerList(pagination, JsonConvert.SerializeObject(dy.data)).Rows.Count;
                var query = new
                {
                    changename = dy.data.changename,
                    workunitcode = dy.data.workunitcode,
                    workunitid = dy.data.workunitid,
                    st = dy.data.st,
                    et = dy.data.et,
                    ledgertype = "0",
                    applynumber = dy.data.applynumber
                };
                var workingCount = safetychangebll.GetLedgerList(pagination, JsonConvert.SerializeObject(query)).Rows.Count;
                Dictionary<string, string> dict_props = new Dictionary<string, string>();
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                    DateFormatString = "yyyy-MM-dd HH:mm" //格式化日期
                };
                settings.Converters.Add(new DecimalToStringConverter());
                var data = new
                {
                    list = list,
                    historyCount = historyCount,
                    workingCount = workingCount
                };
                return new { code = 0, info = "获取数据成功", count = count, data = JObject.Parse(JsonConvert.SerializeObject(data, Formatting.None, settings)) };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message };
            }

        }
        #endregion

        public class itemClass
        {
            public string ItemValue { get; set; }
            public string ItemName { get; set; }
        }
    }
}