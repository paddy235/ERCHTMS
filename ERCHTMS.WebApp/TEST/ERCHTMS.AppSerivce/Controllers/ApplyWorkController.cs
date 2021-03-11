using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.Desktop;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Busines.PersonManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.RiskDatabase;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
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
using System.Linq;
using ERCHTMS.Busines.JPush;

namespace ERCHTMS.AppSerivce.Controllers
{

    /// <summary>
    /// 开工申请
    /// </summary>
    public class ApplyWorkController : BaseApiController
    {
        private IntromissionBLL intromissionbll = new IntromissionBLL();
        private StartapplyforBLL startapplyforbll = new StartapplyforBLL();
        private InvestigateBLL investigatebll = new InvestigateBLL();
        private InvestigateRecordBLL investigaterecordbll = new InvestigateRecordBLL();
        private InvestigateDtRecordBLL investigatedtrecordbll = new InvestigateDtRecordBLL();
        private InvestigateContentBLL investigatecontentbll = new InvestigateContentBLL();
        private PeopleReviewBLL peoplereviewbll = new PeopleReviewBLL();
        private HistoryStartapplyBLL historystartapplybll = new HistoryStartapplyBLL();
        private AptitudeinvestigateauditBLL aptitudeinvestigateauditbll = new AptitudeinvestigateauditBLL(); //审核记录
        private SchemeMeasureBLL schememeasurebll = new SchemeMeasureBLL();//三措两案
        private TechDisclosureBLL techdisclosurebll = new TechDisclosureBLL();//安全技术交底
        private CompactBLL compactbll = new CompactBLL();//合同
        private ProtocolBLL protocolbll = new ProtocolBLL();//协议
        private FileInfoBLL filebll = new FileInfoBLL();
        /// <summary>
        /// 获取开工申请信息列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetList([FromBody]JObject json)
        {
            try
            {
                string path = new DataItemDetailBLL().GetItemValue("imgUrl");
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string startDate = dy.data.startDate; //开始时间
                string endDate = dy.data.endDate;//结束时间
                string projectId = dy.data.projectId; //外包工程Id
                string unitId = dy.data.unitId;//外包单位Id
                string senddeptId = dy.data.deptId;//发包部门Id
                string actiontype = dy.data.actiontype;//0全部 1 我的
                long pageIndex = dy.pageindex;
                long pageSize = dy.pagesize;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                Pagination pagination = new Pagination();
                pagination.page = int.Parse(pageIndex.ToString());
                pagination.rows = int.Parse(pageSize.ToString());
                pagination.p_kid = "s.id as appid";
                pagination.p_fields = @"s.iscommit,
                                           b.fullname unitname,applypeople applyuser,
                                           b.senddeptname deptname,
                                           e.engineername projectname,
                                          s.applyreturntime startdate,s.applyno,
                                            case when isover='0' then '0' else '1' end isover,
                                            case when iscommit='0' then '0'
                                                 when iscommit='1' and isinvestover=0 then '1' 
                                                 when iscommit='1' and isinvestover=1 and isover=0 then '2' 
                                                 when iscommit='1' and isinvestover=1 and isover=1 then '3' else '' end status,
                                            s.applytime";
                pagination.p_tablename = @"epg_startapplyfor s
                                              left join epg_outsouringengineer e on e.id = s.outengineerid
                                              left join base_department b on b.departmentid = s.outprojectid
                                              left join bis_manypowercheck c on s.nodeid=c.id";
                pagination.sidx = "s.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                Operator currUser = OperatorProvider.Provider.Current();
                pagination.conditionJson += "1=1 ";


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
                if (actiontype == "0")//全部
                {
                    if (currUser.IsSystem)
                    {

                    }
                    else if (currUser.RoleName.Contains("厂级部门用户") || currUser.RoleName.Contains("公司级用户"))
                    {
                        pagination.conditionJson += string.Format(" and s.createuserorgcode='{0}'", currUser.OrganizeCode);
                    }
                    else if (currUser.RoleName.Contains("承包商级用户"))
                    {
                        pagination.conditionJson += string.Format(" and b.departmentid ='{0}'", currUser.DeptId);
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(" and b.senddeptid ='{0}' ", currUser.DeptId);
                    }
                    pagination.conditionJson += string.Format(" and s.iscommit='1'");
                }
                else if (actiontype == "1")//我的
                {
                    //我的

                    string[] arrRole = role.Split(',');

                    string strWhere = string.Empty;

                    foreach (string str in arrRole)
                    {
                        strWhere += string.Format(@"  select distinct a.id from epg_startapplyfor a
                                                left join  bis_manypowercheck b on a.nodeid = b.id
                                                left join bis_manypowercheck c on  b.serialnum = c.serialnum and b.moduleno = c.moduleno
                                                left join epg_outsouringengineer d on d.id=a.outengineerid
                                        where  (c.checkdeptid = '{0}' and c.checkdeptid!='-1' and c.checkrolename like '%{1}%'  and  a.isinvestover =0 and  a.iscommit ='1' and  a.isover =0)
                                             or (c.checkdeptid ='-1' and d.engineerletdeptid='{0}' and c.checkrolename like '%{1}%' and  a.isinvestover =0 and  a.iscommit ='1' and  a.isover =0)
                                                union", deptId, str);
                        ////审查
                        //strWhere += string.Format(@"   select  distinct a.id from epg_startapplyfor a  where a.flowdept like'%{0}%' and a.flowrolename like '%{1}%' and  a.isinvestover =0 and  a.iscommit ='1' and  a.isover =0  union", deptId, str);

                        //审核
                        strWhere += string.Format(@"   select  distinct a.id from epg_startapplyfor a  where a.flowdept='{0}' and a.flowrolename like '%{1}%' and  a.isinvestover =1 and  a.iscommit ='1' and  a.isover =0  union", deptId, str);
                    }
                    if (!string.IsNullOrEmpty(strWhere))
                    {
                        strWhere += string.Format(@"  select distinct a.id from epg_startapplyfor a  where a.createuserid ='{0}' and  a.iscommit ='0'", user.UserId);
                    }
                    var conditionDt = intromissionbll.GetDataTableBySql(strWhere);

                    string ids = string.Empty;

                    foreach (DataRow row in conditionDt.Rows)
                    {
                        ids += "'" + row["id"].ToString() + "',";
                    }
                    if (!string.IsNullOrEmpty(ids))
                    {
                        ids = ids.Substring(0, ids.Length - 1);

                        //我要处理的
                        pagination.conditionJson = string.Format("  s.id in ({0})", ids);
                    }
                    else
                    {
                        pagination.conditionJson = string.Format("  1!=1 ");
                    }
                    //我要处理的
                    //pagination.conditionJson += string.Format(" and s.id in ({0})", strWhere);
                }
                else if (actiontype == "2")//待审核(批)
                {
                    string[] arrRole = role.Split(',');

                    string strWhere = string.Empty;

                    foreach (string str in arrRole)
                    {
                        strWhere += string.Format(@"  select distinct a.id from epg_startapplyfor a
                                                left join  bis_manypowercheck b on a.nodeid = b.id
                                                left join bis_manypowercheck c on  b.serialnum = c.serialnum and b.moduleno = c.moduleno
                                                left join epg_outsouringengineer d on d.id=a.outengineerid
                                        where  (c.checkdeptid = '{0}' and c.checkdeptid!='-1' and c.checkrolename like '%{1}%'  and  a.isinvestover =0 and  a.iscommit ='1' and  a.isover =0)
                                             or (c.checkdeptid ='-1' and d.engineerletdeptid='{0}' and c.checkrolename like '%{1}%' and  a.isinvestover =0 and  a.iscommit ='1' and  a.isover =0)
                                                union", deptId, str);
                        ////审查
                        //strWhere += string.Format(@"   select  distinct a.id from epg_startapplyfor a  where a.flowdept like'%{0}%' and a.flowrolename like '%{1}%' and  a.isinvestover =0 and  a.iscommit ='1' and  a.isover =0  union", deptId, str);

                        //审核
                        strWhere += string.Format(@"   select  distinct a.id from epg_startapplyfor a  where a.flowdept='{0}' and a.flowrolename like '%{1}%' and  a.isinvestover =1 and  a.iscommit ='1' and  a.isover =0  union", deptId, str);
                    }
                    if (!string.IsNullOrEmpty(strWhere))
                    {
                        strWhere = strWhere.Substring(0, strWhere.Length - 5);
                    }
                    var conditionDt = intromissionbll.GetDataTableBySql(strWhere);

                    string ids = string.Empty;

                    foreach (DataRow row in conditionDt.Rows)
                    {
                        ids += "'" + row["id"].ToString() + "',";
                    }
                    if (!string.IsNullOrEmpty(ids))
                    {
                        ids = ids.Substring(0, ids.Length - 1);

                        //我要处理的
                        pagination.conditionJson = string.Format("  s.id in ({0})", ids);
                    }
                    else
                    {
                        pagination.conditionJson = string.Format("  1!=1 ");
                    }
                }
                else//已审核(批)
                {
                    if (currUser.IsSystem)
                    {

                    }
                    else if (currUser.RoleName.Contains("厂级部门用户") || currUser.RoleName.Contains("公司级用户"))
                    {
                        pagination.conditionJson += string.Format(" and s.createuserorgcode='{0}'", currUser.OrganizeCode);
                    }
                    else if (currUser.RoleName.Contains("承包商级用户"))
                    {
                        pagination.conditionJson += string.Format(" and b.departmentid ='{0}'", currUser.DeptId);
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(" and b.senddeptid ='{0}' ", currUser.DeptId);
                    }
                    pagination.conditionJson += string.Format(" and s.isover =1 and s.iscommit='1' ");
                }

                string queryJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    StartTime = startDate,
                    EndTime = endDate,
                    ProjectId = projectId,
                    UnitId = unitId,
                    DeptId = senddeptId
                }); ;
                var data = new StartapplyforBLL().GetPageList(pagination, queryJson);
                return new { Code = 0, Count = pagination.records, Info = "获取数据成功", data = data };

            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 获取历史记录
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetHistoryList([FromBody]JObject json)
        {
            try
            {
                string path = new DataItemDetailBLL().GetItemValue("imgUrl");
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string id = dy.data.id;
                var data = new HistoryStartapplyBLL().GetApplyList(id);
                return new { Code = 0, Count = data.Rows.Count, Info = "获取数据成功", data = data };

            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        ///获取历史记录详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetHistoryInfo([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string id = dy.data.id;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                var data = new HistoryStartapplyBLL().GetApplyInfo(id);
                if (data.Rows.Count == 0)
                {
                    return new { Code = -1, Count = 0, Info = "没有数据" };
                }
                else
                {
                    var files = new FileInfoBLL().GetFiles(id);//获取相关附件

                    string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                    foreach (DataRow dr in files.Rows)
                    {
                        dr["filepath"] = dr["filepath"].ToString().Replace("~/", webUrl + "/");
                    }
                    DataTable dt = new DataTable();
                    dt.Columns.Add("num");
                    dt.Columns.Add("itemname");
                    dt.Columns.Add("status");
                    dt.Columns.Add("username");
                    dt.Columns.Add("userid");
                    dt.Columns.Add("signpic");
                    int k = 0;
                    DataTable dtItems = new IntromissionBLL().GetHistoryStartRecordList(id);
                    foreach (DataRow item in dtItems.Rows)
                    {
                        DataRow dr1 = dt.NewRow();
                        dr1[0] = 1 + k;
                        dr1[1] = item["investigatecontent"].ToString();
                        dr1[2] = item["investigateresult"].ToString();
                        dr1[3] = item["investigatepeople"].ToString();
                        dr1[4] = item["investigatepeopleid"].ToString();
                        dr1[5] =string.IsNullOrWhiteSpace(item["signpic"].ToString())? "" : webUrl + item["signpic"].ToString().Replace("../../", "/");
                        dt.Rows.Add(dr1);
                        k++;
                    }
                  
                    string projectId = data.Rows[0][2].ToString();

                    dtItems = new HistoryAuditBLL().GetAuditRecList(id);
                    foreach (DataRow item in dtItems.Rows)
                    {
                        if (string.IsNullOrWhiteSpace(item["auditsignimg"].ToString()))
                        {
                            item["auditsignimg"] = string.Empty;
                        }
                        else
                        {
                            item["auditsignimg"] = webUrl + item["auditsignimg"].ToString().Replace("../../", "/");
                        }
                    }
                    return new
                    {
                        Code = 0,
                        Count = 1,
                        Info = "获取数据成功",
                        data = new
                        {
                            appid = data.Rows[0][0],
                            deptname = data.Rows[0]["deptname"],
                            applyno = data.Rows[0]["applyno"],
                            applycause = data.Rows[0]["applycause"],
                            applytype = data.Rows[0]["applytype"],
                            unitname = data.Rows[0]["unitname"],
                            projectname = data.Rows[0]["projectname"],
                            startdate = DateTime.Parse(data.Rows[0]["startdate"].ToString()).ToString("yyyy-MM-dd"),
                            projectcode = data.Rows[0]["projectcode"],
                            projectid = data.Rows[0]["projectid"],
                            areaname = data.Rows[0]["areaname"],
                            projecttype = data.Rows[0]["projecttype"],
                            projectlevel = data.Rows[0]["projectlevel"],
                            projectcontent = data.Rows[0]["projectcontent"],
                            applyuser = data.Rows[0]["applypeople"],
                            Files = files,
                            Items = dt,
                            AuditInfo = dtItems,
                            htnum = data.Rows[0]["htnum"],
                            DutyMan = data.Rows[0]["dutyman"],
                            SafetyMan = data.Rows[0]["safetyman"],
                            applytime = DateTime.Parse(data.Rows[0]["applytime"].ToString()).ToString("yyyy-MM-dd")
                        }
                    };
                }

            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        ///获取开工申请详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetInfo([FromBody]JObject json)
        {
            try
            {
                StartapplyforBLL apply = new StartapplyforBLL();
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string id = dy.data.id;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                var data = apply.GetApplyInfo(id);
                if (data.Rows.Count == 0)
                {
                    return new { Code = -1, Count = 0, Info = "没有数据" };
                }
                else
                {
                    var files = new FileInfoBLL().GetFiles(id);//获取相关附件

                    string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                    foreach (DataRow dr in files.Rows)
                    {
                        dr["filepath"] = dr["filepath"].ToString().Replace("~/", webUrl + "/");
                    }
                    DataTable dt = new DataTable();
                    dt.Columns.Add("num");
                    dt.Columns.Add("itemid");
                    dt.Columns.Add("itemname");
                    dt.Columns.Add("status");
                    dt.Columns.Add("username");
                    dt.Columns.Add("userid");
                    dt.Columns.Add("signpic");

                    string projectId = data.Rows[0][2].ToString();
                    string nodeId = data.Rows[0]["nodeid"].ToString();

                    int k = 0;
                    DataTable dtItems = apply.GetStartForItem(id);
                    foreach (DataRow item in dtItems.Rows)
                    {
                        DataRow dr1 = dt.NewRow();
                        dr1[0] = 1 + k;
                        dr1[1] = item["itemid"].ToString();
                        dr1[2] = item["investigatecontent"].ToString();
                        dr1[3] = item["investigateresult"].ToString();
                        dr1[4] = item["investigatepeople"].ToString();
                        dr1[5] = item["investigatepeopleid"].ToString();
                        dr1[6] = string.IsNullOrWhiteSpace(item["signpic"].ToString()) ? "" : webUrl + item["signpic"].ToString().Replace("../../", "/"); //webUrl + item["signpic"].ToString().Replace("../../", "/");
                        dt.Rows.Add(dr1);
                        k++;
                    }

                    Operator user = OperatorProvider.Provider.Current();
                    DataTable dtAuditItems = new AptitudeinvestigateauditBLL().GetAuditRecList(id);
                    foreach (DataRow item in dtAuditItems.Rows)
                    {
                        if (string.IsNullOrWhiteSpace(item["auditsignimg"].ToString())) {
                            item["auditsignimg"] = string.Empty;
                        }
                        else{
                            item["auditsignimg"] = webUrl + item["auditsignimg"].ToString().Replace("../../", "/");
                        }
                        
                    }
                    return new
                    {
                        Code = 0,
                        Count = 1,
                        Info = "获取数据成功",
                        data = new
                        {
                            appid = data.Rows[0][0],
                            iscommit = data.Rows[0]["iscommit"],
                            projectid = projectId,
                            deptid = data.Rows[0]["deptid"],
                            deptname = data.Rows[0]["deptname"],
                            applyno = data.Rows[0]["applyno"],
                            applycause = data.Rows[0]["applycause"],
                            applytype = data.Rows[0]["applytype"],
                            unitid = data.Rows[0]["unitid"],
                            unitname = data.Rows[0]["unitname"],
                            projectname = data.Rows[0]["projectname"],
                            startdate = DateTime.Parse(data.Rows[0]["startdate"].ToString()).ToString("yyyy-MM-dd"),
                            isover = int.Parse(data.Rows[0]["isover"].ToString()),
                            nodename = data.Rows[0]["nodename"],
                            projectcode = data.Rows[0]["projectcode"],
                            areaname = data.Rows[0]["areaname"],
                            projecttype = data.Rows[0]["projecttype"],
                            projectlevel = data.Rows[0]["projectlevel"],
                            projectcontent = data.Rows[0]["projectcontent"],
                            applyuser = data.Rows[0]["applypeople"],
                            applytime = DateTime.Parse(data.Rows[0]["applytime"].ToString()).ToString("yyyy-MM-dd"),
                            Files = files,
                            Items = dt,
                            AuditInfo = dtAuditItems,
                            htnum = data.Rows[0]["htnum"],
                            DutyMan = data.Rows[0]["dutyman"],
                            SafetyMan = data.Rows[0]["safetyman"]
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        [HttpPost]
        public object GetCompactNo([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string projectid = dy.engineerid;
                var htnum = new CompactBLL().GetListByProjectId(projectid).OrderBy(x => x.CREATEDATE).ToList().FirstOrDefault().COMPACTNO;
                return new { Code = 0, Count = 0, Info = "操作成功", data = htnum };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 根据外包工程获取开工附件
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getFilelistByProId([FromBody]JObject json) {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string projectid = dy.data.projectid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator currUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                else {
                    string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                    List<FileInfoEntity> filelist = new List<FileInfoEntity>();
                    //三措两案--关联最近一次审核的附件
                    var three = schememeasurebll.GetSchemeMeasureListByOutengineerId(projectid);

                    //合同--全部
                    var c = compactbll.GetListByProjectId(projectid);
                    //协议--全部
                    var p = protocolbll.GetList().Where(x => x.PROJECTID == projectid).ToList();
                    //安全技术交底--全部
                    var t = techdisclosurebll.GetList().Where(x => x.PROJECTID == projectid).ToList();

                    if (three != null)
                    {
                        var file1 = filebll.GetFiles(three.ID);//三措两案附件
                        foreach (DataRow item in file1.Rows)
                        {
                            FileInfoEntity itemFile = new FileInfoEntity();
                            itemFile.FileName = item["FileName"].ToString();
                            itemFile.FilePath = item["filepath"].ToString().Replace("~/", webUrl + "/");
                            itemFile.FileSize = item["filesize"].ToString();
                            filelist.Add(itemFile);
                        }
                    }


                    if (c.Count > 0)
                    {
                        for (int i = 0; i < c.Count; i++)
                        {
                            var file2 = filebll.GetFiles(c[i].ID);//合同附件

                            foreach (DataRow item in file2.Rows)
                            {
                                FileInfoEntity itemFile = new FileInfoEntity();
                                itemFile.FileName = item["FileName"].ToString();
                                itemFile.FilePath = item["filepath"].ToString().Replace("~/", webUrl + "/");
                                itemFile.FileSize = item["filesize"].ToString();
                                filelist.Add(itemFile);
                            }

                        }
                    }
                    if (p.Count > 0)
                    {
                        for (int i = 0; i < p.Count; i++)
                        {
                            var file2 = filebll.GetFiles(p[i].ID);//协议附件

                            foreach (DataRow item in file2.Rows)
                            {
                                FileInfoEntity itemFile = new FileInfoEntity();
                                itemFile.FileName = item["FileName"].ToString();
                                itemFile.FilePath =item["filepath"].ToString().Replace("~/", webUrl + "/");
                                itemFile.FileSize = item["filesize"].ToString();
                                filelist.Add(itemFile);
                            }

                        }
                    }
                    if (t.Count > 0)
                    {
                        for (int i = 0; i < t.Count; i++)
                        {
                            var file2 = filebll.GetFiles(t[i].ID);//安全技术交底

                            foreach (DataRow item in file2.Rows)
                            {
                                FileInfoEntity itemFile = new FileInfoEntity();
                                itemFile.FileName = item["FileName"].ToString();
                                itemFile.FilePath = item["filepath"].ToString().Replace("~/", webUrl + "/");
                                itemFile.FileSize = item["filesize"].ToString();
                                filelist.Add(itemFile);
                            }
                            var file3 = filebll.GetFiles(t[i].ID + "01");//安全技术交底

                            foreach (DataRow item in file3.Rows)
                            {
                                FileInfoEntity itemFile = new FileInfoEntity();
                                itemFile.FileName = item["FileName"].ToString();
                                itemFile.FilePath = item["filepath"].ToString().Replace("~/", webUrl + "/");
                                itemFile.FileSize = item["filesize"].ToString();
                                filelist.Add(itemFile);
                            }
                        }
                    }
                    return new { Code = 0, Count = filelist.Count, Info = "操作成功", data = filelist };
                }
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        ///获取外包工程
        /// </summary>
        /// <param name="json">mode=009</param>
        /// <returns></returns>
        [HttpPost]
        public object getProjects([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string mode = dy.data.mode;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator currUser = OperatorProvider.Provider.Current();
                DataTable dt = new OutsouringengineerBLL().GetEngineerDataByCurrdeptId(currUser, mode);
                dt.Columns.Add("htnum");
                foreach (DataRow item in dt.Rows)
                {
                    var list = new CompactBLL().GetListByProjectId(item["engineerid"].ToString()).OrderBy(x => x.CREATEDATE).ToList();
                    if (list.Count > 0)
                    {
                        item["htnum"] = list.FirstOrDefault().COMPACTNO;
                    }
                    else
                    {
                        item["htnum"] = "";
                    }
                    //item["htnum"] = new CompactBLL().GetListByProjectId(item["engineerid"].ToString()).OrderBy(x => x.CREATEDATE).ToList().FirstOrDefault().COMPACTNO;
                }

                //string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                //if (dt.Rows.Count > 0)
                //{
                //    var proId = dt.Rows[0]["engineerid"].ToString();
                   
                //    var josnResult = new
                //    {
                //        engineername = dt.Rows[0]["engineername"].ToString(),
                //        engineerid = dt.Rows[0]["engineerid"].ToString(),
                //        unitid = dt.Rows[0]["unitid"].ToString(),
                //        unitname = dt.Rows[0]["unitname"].ToString(),
                //        deptname = dt.Rows[0]["deptname"].ToString(),
                //        deptid = dt.Rows[0]["deptid"].ToString(),
                //        projectcode = dt.Rows[0]["projectcode"].ToString(),
                //        areaname = dt.Rows[0]["areaname"].ToString(),
                //        projecttype = dt.Rows[0]["projecttype"].ToString(),
                //        projectlevel = dt.Rows[0]["projectlevel"].ToString(),
                //        projectcontent = dt.Rows[0]["projectcontent"].ToString(),
                //        encode = dt.Rows[0]["encode"].ToString(),
                //        htnum= dt.Rows[0]["htnum"].ToString(),
                //        filelist = filelist
                //    };
                    //return new { Code = 0, Count = 0, Info = "操作成功", data = josnResult };
                //}
                //else {
                //    var josnResult = new
                //    { };
                //    return new { Code = 0, Count = 0, Info = "操作成功", data = josnResult };
                //}
               
                return new { Code = 0, Count = 0, Info = "操作成功", data = dt };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        ///获取工程现场施工负责人和安全员
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getUsers([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                string projectId = dy.data.projectid;
                var list = new StartapplyforBLL().GetSafetyUserInfo(projectId);
                return new { Code = 0, Count = 0, Info = "操作成功", data = new { dutyManager = list[0], dutyUser = list[1] } };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        ///获取工程现场施工负责人和安全员
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getProjectUsers([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                string projectId = dy.data.projectid;
                string roletype = dy.data.roletype;
                OutsouringengineerEntity project = new OutsouringengineerBLL().GetEntity(projectId);
                List<string> str = new StartapplyforBLL().GetSafetyUserInfo(projectId, roletype, project.OUTPROJECTID).ToList();
                return new { Code = 0, Count = 0, Info = "操作成功", data = str };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        ///获取审核记录
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getAuditList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string id = dy.data.id;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator currUser = OperatorProvider.Provider.Current();
                DataTable dt = new AptitudeinvestigateauditBLL().GetAuditRecList(id);
                return new { Code = 0, Count = 0, Info = "操作成功", data = dt };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        ///新增开工申请
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object saveInfo()
        {
            try
            {
                Operator curUser = OperatorProvider.Provider.Current();
                //string res = json.Value<string>("json");
                string res = HttpContext.Current.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string id = dy.data.id;//主键（新增则为空字符串）
                string projectId = dy.data.projectId;//外包工程Id
                string unitId = dy.data.unitId;//外包单位Id
                string deptId = dy.data.deptId;//发包部门Id
                string applyTime = dy.data.startDate;//申请开工日期
                string reason = dy.data.reason; //开工原因
                string deleteids = dy.data.deleteids;//删除附件id集合
                string DutyMan = dy.data.DutyMan;
                string SafetyMan = dy.data.SafetyMan;
                string htnum = dy.data.htnum;
                string filelist = JsonConvert.SerializeObject(dy.data.filelist);
                List<FileInfoEntity> file_list = JsonConvert.DeserializeObject<List<FileInfoEntity>>(filelist);
                OperatorProvider.AppUserId = userId;  //设置当前用户
                var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                StartapplyforEntity entity = new StartapplyforEntity
                {
                    ID = id,
                    OUTPROJECTID = unitId,
                    OUTENGINEERID = projectId,
                    APPLYPEOPLEID = userId,
                    APPLYPEOPLE = user.UserName,
                    APPLYTIME = DateTime.Now,
                    APPLYTYPE = "开工申请",
                    APPLYRETURNTIME = DateTime.Parse(applyTime),
                    APPLYCAUSE = reason,
                    DutyMan = DutyMan,
                    SafetyMan = SafetyMan,
                    htnum = htnum
                };
                entity.ISCOMMIT = "1";
                bool flag = startapplyforbll.SaveForm(id, entity);
                string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                if (file_list != null) {
                    if (file_list.Count > 0)
                    {
                        for (int i = 0; i < file_list.Count; i++)
                        {
                            string fileName = file_list[i].FileName;
                            string filesize = file_list[i].FileSize;
                            string FileEextension = Path.GetExtension(fileName);
                            string fileGuid = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Guid.NewGuid().ToString();
                            string filepath = file_list[i].FilePath.ToString().Replace(webUrl, "~/");
                            //string dir = new DataItemDetailBLL().GetItemValue("imgPath") + "\\Resource\\Upfile";
                            //string newFileName = fileGuid + FileEextension;
                            //string newFilePath = dir + "\\" + newFileName;
                            //if (!Directory.Exists(dir))
                            //{
                            //    Directory.CreateDirectory(dir);
                            //}
                            FileInfoEntity fileInfoEntity = new FileInfoEntity();
                            //if (!System.IO.File.Exists(newFilePath))
                            //{
                            ////保存文件
                            //file.SaveAs(newFilePath);
                            //文件信息写入数据库
                            fileInfoEntity.Create();
                            fileInfoEntity.FileId = fileGuid;
                            fileInfoEntity.RecId = entity.ID;
                            fileInfoEntity.FileName = fileName;
                            fileInfoEntity.FilePath = filepath;
                            fileInfoEntity.FileSize = filesize;//文件大小（kb）
                            fileInfoEntity.FileExtensions = FileEextension;
                            fileInfoEntity.FileType = FileEextension.TrimStart('.');
                            FileInfoBLL fileInfoBLL = new FileInfoBLL();
                            fileInfoBLL.SaveForm("", fileInfoEntity);
                            //}
                        }
                    }
                }
              
                if (flag)
                {
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
                                fileInfoEntity.RecId = entity.ID;
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
                }
                //提交到下一个流程
                #region 提交到下一个流程
                bool isUseSetting = true;
                string moduleName = "开工申请审查";
                //判断是否需要审查(审查配置表)
                var list = investigatebll.GetList(curUser.OrganizeId).Where(p => p.SETTINGTYPE == "开工申请").ToList();
                InvestigateEntity investigateEntity = null;
                if (list.Count() > 0)
                {
                    investigateEntity = list.FirstOrDefault();
                }
                ManyPowerCheckEntity mpcEntity = null;

                if (null != investigateEntity)
                {
                    //启用审查
                    if (investigateEntity.ISUSE == "是")
                    {
                        entity.NodeName = "审查中";
                        entity.ISINVESTOVER = 0; //审查状态

                        //新增审查记录
                        InvestigateRecordEntity rcEntity = new InvestigateRecordEntity();
                        rcEntity.INTOFACTORYID = entity.ID;
                        rcEntity.INVESTIGATETYPE = "0";//当前记录标识
                        investigaterecordbll.SaveForm("", rcEntity);

                        //获取审查内容
                        var contentList = investigatecontentbll.GetList(investigateEntity.ID).ToList();

                        //批量增加审查内容到审查记录中
                        foreach (InvestigateContentEntity icEntity in contentList)
                        {
                            InvestigateDtRecordEntity dtEntity = new InvestigateDtRecordEntity();
                            dtEntity.INVESTIGATERECORDID = rcEntity.ID;
                            dtEntity.INVESTIGATECONTENT = icEntity.INVESTIGATECONTENT;
                            dtEntity.INVESTIGATECONTENTID = icEntity.ID;
                            investigatedtrecordbll.SaveForm("", dtEntity);
                        }
                    }
                    else  //未启用审查，直接跳转到审核 
                    {
                        entity.NodeName = "审核中";
                        entity.ISINVESTOVER = 1; //审核状态
                        entity.ISCOMMIT = "1";
                    }
                }
                else
                {
                    //如果没有审查配置，直接到审核
                    isUseSetting = false;
                    entity.NodeName = "审核中";
                    entity.ISCOMMIT = "1";
                    entity.ISINVESTOVER = 1; //审核状态
                }

                //更改申请信息状态
                mpcEntity = peoplereviewbll.CheckAuditForNextFlow(curUser, moduleName, entity.OUTENGINEERID, entity.NodeId, false, isUseSetting);

                if (null != mpcEntity)
                {
                    entity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                    entity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                    entity.FLOWROLE = mpcEntity.CHECKROLEID;
                    entity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                    entity.NodeId = mpcEntity.ID;
                    DataTable dt = new UserBLL().GetUserAccountByRoleAndDept(curUser.OrganizeId, mpcEntity.CHECKDEPTID, mpcEntity.CHECKROLENAME);
                    var userAccount = dt.Rows[0]["account"].ToString();
                    var userName = dt.Rows[0]["realname"].ToString();
                    JPushApi.PushMessage(userAccount, userName, "WB015", entity.ID);
                }
                else
                {
                    //未配置审核项
                    entity.FLOWDEPT = "";
                    entity.FLOWDEPTNAME = "";
                    entity.FLOWROLE = "";
                    entity.FLOWROLENAME = "";
                    entity.NodeId = "";
                    entity.NodeName = "已完结";
                    entity.IsOver = 1;
                    entity.ISCOMMIT = "1";
                    entity.ISINVESTOVER = 1;
                }
                startapplyforbll.SaveForm(entity.ID, entity);
                #endregion
                return new { Code = 0, Count = 0, Info = "操作成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #region 保存审查记录
        /// <summary>
        /// 保存开工申请审查记录
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>

        [HttpPost]
        public object SaveAppStartFor()
        {
            string res = HttpContext.Current.Request["json"];

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            string userid = dy.userid; //用户ID 

            OperatorProvider.AppUserId = userid;  //设置当前用户

            Operator curUser = OperatorProvider.Provider.Current();

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            try
            {
                string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                //审查内容 (审核阶段可不用传入)
                List<object> record = res.Contains("record") ? (List<object>)dy.data.record : null;

                foreach (dynamic rdy in record)
                {
                    string id = rdy.id.ToString();  //主键
                    string result = rdy.result.ToString(); //结果
                    string people = rdy.people.ToString(); //选择的人员
                    string peopleid = rdy.peopleid.ToString();
                    string signpic = rdy.signpic.ToString();

                    var scEntity = investigatedtrecordbll.GetEntity(id); //审查内容项
                    scEntity.INVESTIGATERESULT = result;
                    scEntity.INVESTIGATEPEOPLE = people;
                    scEntity.INVESTIGATEPEOPLEID = peopleid;
                    scEntity.SIGNPIC = string.IsNullOrWhiteSpace(signpic) ? "" : signpic.Replace(webUrl, "").ToString();
                    HttpFileCollection files = HttpContext.Current.Request.Files;
                    if (files.Count > 0)
                    {
                        for (int i = 0; i < files.AllKeys.Length; i++)
                        {
                            HttpPostedFile file = files[i];
                            string fileOverName = System.IO.Path.GetFileName(file.FileName);
                            string fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                            string FileEextension = Path.GetExtension(file.FileName);
                            if (fileName == scEntity.ID)
                            {
                                string dir = new DataItemDetailBLL().GetItemValue("imgPath") + "\\Resource\\sign";
                                string newFileName = fileName + FileEextension;
                                string newFilePath = dir + "\\" + newFileName;
                                file.SaveAs(newFilePath);
                                scEntity.SIGNPIC = "/Resource/sign/" + fileOverName;
                                break;
                            }
                        }
                    }
                    investigatedtrecordbll.SaveForm(id, scEntity);
                }
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "保存失败" };
            }
            return new { code = 0, count = 0, info = "保存成功" };
        }
        #endregion
        #region 提交审查/审核开工申请申请信息
        /// <summary>
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SubmitAppStartFor()
        {

            //string res = json.Value<string>("json");
            string res = HttpContext.Current.Request["json"];

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            string userid = dy.userid; //用户ID 

            OperatorProvider.AppUserId = userid;  //设置当前用户

            Operator curUser = OperatorProvider.Provider.Current();

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            string deptId = string.Empty;
            string deptName = string.Empty;
            string roleNames = curUser.RoleName;

            //公司级用户取机构对象
            if (roleNames.Contains("公司级用户"))
            {
                deptId = curUser.OrganizeId;  //机构ID
                deptName = curUser.OrganizeName;//机构名称
            }
            else
            {
                deptId = curUser.DeptId; //部门ID
                deptName = curUser.DeptName; //部门ID
            }

            int noDoneCount = 0; //未完成个数

            bool isUseSetting = true; //是否多个流程配置下的角色同时进行流程审核

            string newKeyValue = string.Empty;  //新的入场许可申请Id

            string state = res.Contains("state") ? dy.data.state.ToString() : "";  //判定是否是审查，还是审核阶段

            string outengineerid = res.Contains("outengineerid") ? dy.data.outengineerid.ToString() : ""; //外包工程id

            string startforId = res.Contains("startforId") ? dy.data.startforId.ToString() : ""; // 开工申请id

            //审查内容 (审核阶段可不用传入)
            List<object> record = res.Contains("record") ? (List<object>)dy.data.record : null;//{ "record":"[{"scid":"xxx","result":"未完成","people":"xxxxx"},{"scid":"yyyy","result":"已完成","people":"yyyy"}]"}

            /*******审核详情*******/
            string approveresult = res.Contains("approveresult") ? dy.data.approveresult.ToString() : ""; //审核结果  0 表示同意 1 表示不同意

            string approveopinion = res.Contains("approveopinion") ? dy.data.approveopinion.ToString() : "";  //审核意见

            //当前开工申请审查对象
            StartapplyforEntity entity = startapplyforbll.GetEntity(startforId);

            //审核对象
            AptitudeinvestigateauditEntity aentity = new AptitudeinvestigateauditEntity();

            //更改申请信息状态
            ManyPowerCheckEntity mpcEntity = peoplereviewbll.CheckAuditForNextFlow(curUser, "开工申请审查", entity.OUTENGINEERID, entity.NodeId, false, isUseSetting);

            string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
            //审查状态下更新审查内容
            if (state == "1")
            {
                //只更新审查内容
                foreach (dynamic rdy in record)
                {
                    string id = rdy.id.ToString();  //主键
                    string result = rdy.result.ToString(); //结果
                    string people = rdy.people.ToString(); //选择的人员
                    string peopleid = rdy.peopleid.ToString();
                    string signpic = rdy.signpic.ToString();

                    var scEntity = investigatedtrecordbll.GetEntity(id); //审查内容项
                    scEntity.INVESTIGATERESULT = result;
                    if (result == "未完成") { noDoneCount += 1; } //存在未完成的则累加
                    scEntity.INVESTIGATEPEOPLE = people;
                    scEntity.INVESTIGATEPEOPLEID = peopleid;
                    scEntity.SIGNPIC = string.IsNullOrWhiteSpace(signpic) ? "" : signpic.Replace(webUrl, "").ToString();
                    HttpFileCollection files = HttpContext.Current.Request.Files;
                    if (files.Count > 0)
                    {
                        for (int i = 0; i < files.AllKeys.Length; i++)
                        {
                            HttpPostedFile file = files[i];
                            string fileOverName = System.IO.Path.GetFileName(file.FileName);
                            string fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                            string FileEextension = Path.GetExtension(file.FileName);
                            if (fileName == scEntity.ID)
                            {
                                string dir = new DataItemDetailBLL().GetItemValue("imgPath") + "\\Resource\\sign";
                                string newFileName = fileName + FileEextension;
                                string newFilePath = dir + "\\" + newFileName;
                                file.SaveAs(newFilePath);
                                scEntity.SIGNPIC = "/Resource/sign/" + fileOverName;
                                break;
                            }
                        }
                    }
                    //更新当前流程进行中的审查内容
                    investigatedtrecordbll.SaveForm(id, scEntity);
                }

                //退回操作
                if (noDoneCount > 0)
                {
                    AddBackData(startforId, out newKeyValue);
                    entity.FLOWDEPT = " ";
                    entity.FLOWDEPTNAME = " ";
                    entity.FLOWROLE = " ";
                    entity.FLOWROLENAME = " ";
                    entity.NodeId = " ";
                    entity.ISCOMMIT = "0";
                    entity.ISINVESTOVER = 0; //更改状态为登记状态
                    entity.NodeName = "";
                    var applyUser = new UserBLL().GetEntity(entity.APPLYPEOPLEID);
                    if (applyUser != null)
                    {
                        JPushApi.PushMessage(applyUser.Account, entity.CREATEUSERNAME, "WB014", entity.ID);
                    }
                }
                else
                {
                    if (null != mpcEntity)
                    {
                        entity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                        entity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                        entity.FLOWROLE = mpcEntity.CHECKROLEID;
                        entity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                        entity.NodeId = mpcEntity.ID;
                        DataTable dt = new UserBLL().GetUserAccountByRoleAndDept(curUser.OrganizeId, mpcEntity.CHECKDEPTID, mpcEntity.CHECKROLENAME);
                        var userAccount = dt.Rows[0]["account"].ToString();
                        var userName = dt.Rows[0]["realname"].ToString();
                        JPushApi.PushMessage(userAccount, userName, "WB015", entity.ID);
                    }
                    entity.NodeName = "审核中";
                    entity.ISINVESTOVER = 1; //更改状态为审核
                    entity.ISCOMMIT = "1";
                    entity.IsOver = 0;
                }
            }
            else
            {
                //同意进行下一步
                if (approveresult == "0")
                {
                    //下一步流程不为空
                    if (null != mpcEntity)
                    {
                        entity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                        entity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                        entity.FLOWROLE = mpcEntity.CHECKROLEID;
                        entity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                        entity.NodeId = mpcEntity.ID;
                        entity.ISCOMMIT = "1";
                        entity.ISINVESTOVER = 1; //审核状态
                        DataTable dt = new UserBLL().GetUserAccountByRoleAndDept(curUser.OrganizeId, mpcEntity.CHECKDEPTID, mpcEntity.CHECKROLENAME);
                        var userAccount = dt.Rows[0]["account"].ToString();
                        var userName = dt.Rows[0]["realname"].ToString();
                        JPushApi.PushMessage(userAccount, userName, "WB015", entity.ID);
                    }
                    else
                    {
                        entity.FLOWDEPT = " ";
                        entity.FLOWDEPTNAME = " ";
                        entity.FLOWROLE = " ";
                        entity.FLOWROLENAME = " ";
                        entity.NodeId = " ";
                        entity.NodeName = "已完结";
                        entity.ISINVESTOVER = 1; //更改状态为完结状态
                        entity.ISCOMMIT = "1";
                        entity.IsOver = 1;
                        //开工申请审核通过更新工程状态为在建
                        OutsouringengineerEntity engineerEntity = new OutsouringengineerBLL().GetEntity(entity.OUTENGINEERID);
                        engineerEntity.ENGINEERSTATE = "002";
                        new OutsouringengineerBLL().SaveForm(engineerEntity.ID, engineerEntity);
                    }

                    //添加审核记录
                    aentity.AUDITRESULT = approveresult;
                    aentity.AUDITOPINION = approveopinion;
                    aentity.AUDITPEOPLE = curUser.UserName;
                    aentity.AUDITPEOPLEID = curUser.UserId;
                    aentity.AUDITDEPT = deptName;
                    aentity.AUDITDEPTID = deptId;
                    aentity.AUDITTIME = DateTime.Now;
                    aentity.APTITUDEID = startforId; //关联id 
                    HttpFileCollection files = HttpContext.Current.Request.Files;
                    if (files.Count > 0)
                    {
                        for (int i = 0; i < files.AllKeys.Length; i++)
                        {
                            HttpPostedFile file = files[i];
                            string fileOverName = System.IO.Path.GetFileName(file.FileName);
                            string fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                            string FileEextension = Path.GetExtension(file.FileName);
                            if (fileName == aentity.ID)
                            {
                                string dir = new DataItemDetailBLL().GetItemValue("imgPath") + "\\Resource\\sign";
                                string newFileName = fileName + FileEextension;
                                string newFilePath = dir + "\\" + newFileName;
                                file.SaveAs(newFilePath);
                                aentity.AUDITSIGNIMG = "/Resource/sign/" + fileOverName;
                                break;
                            }
                        }
                    }
                    aptitudeinvestigateauditbll.SaveForm("", aentity);

                }
                else  //退回到申请人
                {
                    AddBackData(startforId, out newKeyValue);  //添加历史记录
                    entity.FLOWDEPT = " ";
                    entity.FLOWDEPTNAME = " ";
                    entity.FLOWROLE = " ";
                    entity.FLOWROLENAME = " ";
                    entity.NodeId = " ";
                    entity.ISCOMMIT = "0";
                    entity.ISINVESTOVER = 0; //更改状态为登记状态
                    entity.IsOver = 0;
                    entity.NodeName = "";
                    var applyUser = new UserBLL().GetEntity(entity.APPLYPEOPLEID);
                    if (applyUser != null)
                    {
                        JPushApi.PushMessage(applyUser.Account, entity.CREATEUSERNAME, "WB014", entity.ID);
                    }
                    //获取当前业务对象的所有历史审核记录
                    var shlist = aptitudeinvestigateauditbll.GetAuditList(startforId);
                    //批量更新审核记录关联ID
                    foreach (AptitudeinvestigateauditEntity mode in shlist)
                    {
                        mode.APTITUDEID = newKeyValue; //对应新的关联ID
                        aptitudeinvestigateauditbll.SaveForm(mode.ID, mode);
                    }
                    //添加审核记录
                    aentity.AUDITRESULT = approveresult;
                    aentity.AUDITOPINION = approveopinion;
                    aentity.AUDITPEOPLE = curUser.UserName;
                    aentity.AUDITPEOPLEID = curUser.UserId;
                    aentity.AUDITDEPT = deptName;
                    aentity.AUDITDEPTID = deptId;
                    aentity.AUDITTIME = DateTime.Now;
                    aentity.APTITUDEID = newKeyValue; //关联id 
                    HttpFileCollection files = HttpContext.Current.Request.Files;
                    if (files.Count > 0)
                    {
                        for (int i = 0; i < files.AllKeys.Length; i++)
                        {
                            HttpPostedFile file = files[i];
                            string fileOverName = System.IO.Path.GetFileName(file.FileName);
                            string fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                            string FileEextension = Path.GetExtension(file.FileName);
                            if (fileName == aentity.ID)
                            {
                                string dir = new DataItemDetailBLL().GetItemValue("imgPath") + "\\Resource\\sign";
                                string newFileName = fileName + FileEextension;
                                string newFilePath = dir + "\\" + newFileName;
                                file.SaveAs(newFilePath);
                                aentity.AUDITSIGNIMG = "/Resource/sign/" + fileOverName;
                                break;
                            }
                        }
                    }
                    aptitudeinvestigateauditbll.SaveForm("", aentity);
                }
            }
            //更改开工申请申请单
            startapplyforbll.SaveForm(startforId, entity);

            return new { code = 0, count = 0, info = "保存成功" };
        }
        #endregion
        #region 退回添加到历史记录信息
        /// <summary>
        /// 退回添加到历史记录信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="arr"></param>
        public void AddBackData(string keyValue, out string newKeyValue)
        {
            //退回的同时保存原始的申请记录
            var dentity = startapplyforbll.GetEntity(keyValue); //原始记录
            var startfor = JsonConvert.SerializeObject(dentity);
            HistoryStartapplyEntity hentity = JsonConvert.DeserializeObject<HistoryStartapplyEntity>(startfor);
            hentity.ID = Guid.NewGuid().ToString();
            hentity.APPLYID = dentity.ID;
            var unit = new OutsourcingprojectBLL().GetOutProjectInfo(hentity.OUTPROJECTID);
            if (unit != null)
                hentity.OUTPROJECT = unit.OUTSOURCINGNAME;
            historystartapplybll.SaveForm("", hentity);
            var file1 = new FileInfoBLL().GetFiles(keyValue);
            if (file1.Rows.Count > 0)
            {
                var key = hentity.ID;
                foreach (DataRow item in file1.Rows)
                {
                    FileInfoEntity itemFile = new FileInfoEntity();
                    itemFile.FileName = item["FileName"].ToString();
                    itemFile.FilePath = item["filepath"].ToString();
                    itemFile.FileSize = item["filesize"].ToString();
                    itemFile.RecId = key;
                    new FileInfoBLL().SaveForm(itemFile.FileId, itemFile);
                }
            }
            newKeyValue = hentity.ID;
            //更新审查记录单关联ID
            InvestigateRecordEntity irEntity = investigaterecordbll.GetEntityByIntroKey(keyValue); //审查记录单
            if (null != irEntity)
            {
                irEntity.INTOFACTORYID = newKeyValue;
                irEntity.INVESTIGATETYPE = "1"; //历史记录标识
                investigaterecordbll.SaveForm(irEntity.ID, irEntity);
            }
        }
        #endregion
        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="fileInfoIds"></param>
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
    }

    public class CheckItemsEntity
    {
        public string num { get; set; }
        public string itemname { get; set; }
        public int status { get; set; }
        public string username { get; set; }

    }
}