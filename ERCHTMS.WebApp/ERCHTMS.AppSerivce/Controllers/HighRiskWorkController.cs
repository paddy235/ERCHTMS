using BSFramework.Util.WebControl;
using ERCHTMS.AppSerivce.Model;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
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
using ERCHTMS.Busines.HighRiskWork;
using ERCHTMS.Cache;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.LllegalManage;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.EngineeringManage;
using BSFramework.Util;
using ERCHTMS.Busines.JPush;
using ERCHTMS.Busines.RiskDatabase;
using System.Text;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using BSFramework.Util.Extension;
using ERCHTMS.Entity.HighRiskWork.ViewModel;

namespace ERCHTMS.AppSerivce.Controllers
{
    public class HighRiskWorkController : BaseApiController
    {
        #region 实例化对象
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private SidePersonBLL sidepersonbll = new SidePersonBLL();
        private DataItemCache dataItemCache = new DataItemCache();
        private TaskSignBLL tasksignbll = new TaskSignBLL();
        private SideCheckProjectBLL sidecheckprojectbll = new SideCheckProjectBLL();
        private TaskRelevanceProjectBLL taskrelevanceprojectbll = new TaskRelevanceProjectBLL();
        private HighRiskApplyBLL highriskapplybll = new HighRiskApplyBLL();
        private HighRiskCheckBLL highriskcheckbll = new HighRiskCheckBLL();
        private HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL(); //隐患基本信息
        private LllegalRegisterBLL lllegalregisterbll = new LllegalRegisterBLL(); // 违章基本信息

        private HighImportTypeBLL highimporttypebll = new HighImportTypeBLL();
        private HighProjectSetBLL highprojectsetbll = new HighProjectSetBLL();
        private HighRiskCommonApplyBLL highriskcommonapplybll = new HighRiskCommonApplyBLL();
        private HighRiskApplyMBXXBLL applymbxxbll = new HighRiskApplyMBXXBLL();
        private ScaffoldprojectBLL scaffoldprojectbll = new ScaffoldprojectBLL();
        private ScaffoldauditrecordBLL scaffoldauditrecordbll = new ScaffoldauditrecordBLL();
        private PeopleReviewBLL peoplereviewbll = new PeopleReviewBLL();

        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private OrganizeBLL orgBLL = new OrganizeBLL();
        private DepartmentCache departmentCache = new DepartmentCache();
        private UserBLL userbll = new UserBLL();
        private OutsouringengineerBLL outsouringengineerbll = new OutsouringengineerBLL();
        private DistrictBLL districtbll = new DistrictBLL();
        private ProvinceHighWorkBLL provincehighworkbll = new ProvinceHighWorkBLL();

        private TaskShareBLL tasksharebll = new TaskShareBLL();
        private StaffInfoBLL staffinfobll = new StaffInfoBLL();
        private HighRiskRecordBLL highriskrecordbll = new HighRiskRecordBLL();
        private RisktrainlibdetailBLL risktrainlibdetailbll = new RisktrainlibdetailBLL();
        private RisktrainlibBLL risktrainlibbll = new RisktrainlibBLL();
        private ScaffoldBLL scaffoldbll = new ScaffoldBLL();
        private ManyPowerCheckBLL manypowercheckbll = new ManyPowerCheckBLL();
        private FireWaterBLL firewaterbll = new FireWaterBLL();
        private TransferrecordBLL transferrecordbll = new TransferrecordBLL();
        private List<DeptData> lsData = new List<DeptData>();
        private List<DeptData> cxData = new List<DeptData>();
        #endregion

        public HttpContext ctx { get { return HttpContext.Current; } }
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {

        }

        #region 是否启动
        public string StartUp()
        {
            var str = dataitemdetailbll.GetItemValue("是否启动定时服务");
            return str;
        }
        #endregion

        #region  获取是否启动定时服务
        /// <summary>
        /// 获取是否启动定时服务
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetIsStartUp()
        {
            try
            {
                //0:不启动 1：启动
                var str = StartUp();
                return new { code = 0, info = "获取数据成功", count = 0, data = str };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 旁站监督
        #region 1.获取旁站监督员
        /// <summary>
        /// 1.获取旁站监督员
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetSidePersonList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string sidename = res.Contains("sidename") ? dy.data.sidename : "";//旁站监督人员名称
                string steamid = res.Contains("steamid") ? dy.data.steamid : "";//旁站监督单位
                //获取页数和条数
                int page = Convert.ToInt32(dy.data.pagenum), rows = Convert.ToInt32(dy.data.pagesize);
                Pagination pagination = new Pagination();
                pagination.p_kid = "id as sideid";
                pagination.p_fields = "sideuserid,sideusername";//旁站监督员id,监督员name
                pagination.p_tablename = "bis_sideperson t";
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "createdate";//排序字段
                pagination.sord = "desc";//排序方式
                string sqlwhere = string.Empty;
                pagination.conditionJson = string.Format(@"createuserorgcode='{0}'", curUser.OrganizeCode);
                if (!string.IsNullOrEmpty(sidename))
                {
                    pagination.conditionJson += string.Format(@" and sideusername like '%{0}%'", sidename);
                }
                if (!string.IsNullOrEmpty(steamid))
                {
                    pagination.conditionJson += string.Format(@" and sideuserdeptid='{0}'", steamid);
                }
                DataTable dt = sidepersonbll.GetPageDataTable(pagination, null);
                return new { code = 0, info = "获取数据成功", count = pagination.records, data = dt };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }
        #endregion

        #region 2.获取监督状态
        /// <summary>
        /// 2.获取监督状态
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetSuperviseStateList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                var type = dy.data.type;
                List<DataItemModel> itemlist = new List<DataItemModel>();
                //我的旁站监督任务
                if (type == "0")
                {
                    itemlist = dataitemdetailbll.GetDataItemListByItemCode("'SuperviseState'").Where(x => x.ItemValue != "2").ToList();
                }
                //监管
                else if (type == "1")
                {
                    itemlist = dataitemdetailbll.GetDataItemListByItemCode("'UrgeState'").ToList();
                }
                return new { code = 0, info = "获取数据成功", count = itemlist.Count(), data = itemlist.Select(x => new { statevalue = x.ItemValue, statename = x.ItemName }) };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region  3.获取作业类别
        [HttpPost]
        public object GetWorkTypeTreeJson([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string projectname = res.Contains("projectname") ? dy.data.projectname.ToString() : "";//工程名称
                string deptid = res.Contains("deptid") ? dy.data.deptid.ToString() : "";//作业单位
                string tasktype = res.Contains("tasktype") ? dy.data.tasktype.ToString() : "";//旁站任务类型
                string strwhere = " where 1=1 ";
                if (curUser.RoleName.Contains("公司") || curUser.RoleName.Contains("厂级"))
                {
                    strwhere += string.Format(" and createuserorgcode='{0}'", curUser.OrganizeCode);
                }
                else
                {
                    strwhere += string.Format(" and ((workdeptcode in(select encode from base_department where encode like '{0}%'))  or (engineeringid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", curUser.DeptCode, curUser.DeptId);
                }
                if (!string.IsNullOrEmpty(projectname))
                {
                    strwhere += string.Format(" and engineeringname='{0}'", projectname);
                }
                if (!string.IsNullOrEmpty(deptid))
                {
                    if (tasktype == "0")
                    {
                        strwhere += string.Format(" and workdeptid='{0}'", deptid);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(projectname))
                        {
                            strwhere += string.Format(" and workdeptid='{0}'", deptid);
                        }
                        else
                        {
                            var depart = new DepartmentBLL().GetEntity(deptid);
                            strwhere += string.Format(" and ((workdeptcode in(select encode from base_department where encode like '{0}%'))  or (engineeringid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", depart.EnCode, depart.DepartmentId);
                        }
                    }
                }
                var data = dataItemCache.GetDataItemList("TaskWorkType");
                var treeList = new List<WorkSortData>();
                foreach (DataItemModel item in data)
                {
                    WorkSortData tree = new WorkSortData();
                    bool hasChildren = data.Count(t => t.ParentId == item.ItemDetailId) == 0 ? false : true;
                    tree.sid = item.ItemDetailId;//作业类别id
                    tree.name = item.ItemName;//作业类别名称
                    tree.code = item.ItemValue;//作业类别值
                    tree.parentid = item.ParentId;//父节点
                    tree.isoptional = item.ItemValue == "01" ? false : true;//是否可选
                    string strwhere1 = string.Format(" and worktype='{0}'", item.ItemValue);
                    string sql = string.Format(@"select workdepttypename,workdepttype,workdeptid,workdeptname,workdeptcode,workplace,workcontent, EngineeringName,EngineeringId,worktype as workinfotype,workstarttime,workendtime,workareaname,workusernames from v_taskscaffold {0} {1} order by CreateDate", strwhere, strwhere1);
                    DataTable dt = new PerilEngineeringBLL().GetPerilEngineeringList(sql);
                    tree.superwork = dt;
                    treeList.Add(tree);
                }
                return new { code = 0, info = "获取数据成功", count = treeList.Count, data = treeList };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region  4.获取旁站监督单位
        [HttpPost]
        public object GetSuperviseDeptTree([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string showtype = res.Contains("showtype") ? dy.data.showtype.ToString() : "";//搜索:0为搜索，1.新增页面
                string deptid = res.Contains("deptid") ? dy.data.deptid.ToString() : "";//单位id
                string type = res.Contains("type") ? dy.data.type.ToString() : "";//0:单位内部 1：外包单位
                var rolenames = curUser.RoleName;
                //获取当前部门
                string organizeId = curUser.OrganizeId;
                string parentId = "0";
                IList<DeptData> result = new List<DeptData>();
                IList<DeptData> list = new List<DeptData>();
                if (showtype == "0")
                {
                    //选择部门
                    var deptcontract = departmentBLL.GetList().Where(t => t.EnCode.StartsWith(curUser.OrganizeCode) && t.EnCode != curUser.OrganizeCode && !(t.Nature == "承包商" || t.Nature == "分包商" || t.Description == "外包工程承包商" || t.Nature == "班组" || t.Nature == "专业")).OrderBy(t => t.SortCode).ToList();
                    foreach (var item in deptcontract)
                    {
                        DeptData dept = new DeptData();
                        dept.deptid = item.DepartmentId;
                        dept.code = item.EnCode;
                        dept.isorg = 0;
                        dept.oranizeid = item.OrganizeId;
                        dept.parentcode = "";
                        dept.parentid = parentId;
                        dept.name = item.FullName;
                        dept.isparent = true;
                        dept.isoptional = "";
                        dept.children = list;
                        result.Add(dept);
                    }
                }
                else if (showtype == "1")
                {
                    if (curUser.RoleName.Contains("厂级") || curUser.RoleName.Contains("公司"))
                    {
                        //OrganizeEntity org = orgBLL.GetEntity(organizeId);
                        DeptData dept = new DeptData();
                        dept.deptid = curUser.OrganizeId;
                        dept.code = curUser.OrganizeCode;
                        dept.isorg = 1;
                        dept.oranizeid = curUser.OrganizeId;
                        dept.parentcode = "";
                        dept.parentid = parentId;
                        dept.name = curUser.OrganizeName;
                        dept.isparent = true;
                        list = GetChangeDept(dept);
                        dept.children = list;
                        dept.isoptional = "1";
                        result.Add(dept);
                    }
                    else
                    {
                        DeptData dept = new DeptData();
                        dept.deptid = curUser.DeptId;
                        dept.code = curUser.DeptCode;
                        dept.isorg = 0;
                        dept.oranizeid = curUser.OrganizeId;
                        dept.parentcode = "";
                        dept.parentid = parentId;
                        dept.name = curUser.DeptName;
                        dept.isparent = true;
                        list = GetChangeDept(dept);
                        dept.children = list;
                        result.Add(dept);
                    }
                }
                else if (showtype == "2")
                {
                    if (type == "0")
                    {
                        //OrganizeEntity org = orgBLL.GetEntity(organizeId);
                        DeptData dept = new DeptData();
                        dept.deptid = curUser.OrganizeId;
                        dept.code = curUser.OrganizeCode;
                        dept.isorg = 1;
                        dept.oranizeid = curUser.OrganizeId;
                        dept.parentcode = "";
                        dept.parentid = parentId;
                        dept.name = curUser.OrganizeName;
                        dept.isparent = true;
                        list = GetChangeDept(dept);
                        dept.children = list;
                        dept.isoptional = "1";
                        result.Add(dept);
                    }
                    else if (type == "1")
                    {
                        DeptData dept = new DeptData();
                        var deptcontract = departmentBLL.GetList().Where(t => t.OrganizeId == organizeId && t.Description == "外包工程承包商").OrderBy(t => t.SortCode).ToList().FirstOrDefault();
                        if (deptcontract != null)
                        {
                            dept.deptid = deptcontract.DepartmentId;
                            dept.code = deptcontract.EnCode;
                            dept.isorg = 0;
                            dept.oranizeid = deptcontract.OrganizeId;
                            dept.parentcode = "";
                            dept.parentid = parentId;
                            dept.name = deptcontract.FullName;
                            dept.isparent = true;
                            dept.isoptional = "1";
                            list = GetChangeDept(dept);
                            dept.children = list;
                        }
                        result.Add(dept);
                    }
                }
                else if (showtype == "3")
                {
                    DepartmentEntity deptentity = departmentBLL.GetEntity(deptid);
                    //选择班组
                    var deptcontract = departmentBLL.GetList().Where(t => t.Nature == "班组" && t.DeptCode.StartsWith(deptentity.DeptCode));
                    foreach (var item in deptcontract)
                    {
                        DeptData dept = new DeptData();
                        dept.deptid = item.DepartmentId;
                        dept.code = item.EnCode;
                        dept.isorg = 0;
                        dept.oranizeid = item.OrganizeId;
                        dept.parentcode = "";
                        dept.parentid = parentId;
                        dept.name = item.FullName;
                        dept.isparent = true;
                        dept.isoptional = "";
                        dept.children = list;
                        result.Add(dept);
                    }
                }
                else if (showtype == "4")
                {
                    if (curUser.RoleName.Contains("厂级") || curUser.RoleName.Contains("公司"))
                    {
                        //OrganizeEntity org = orgBLL.GetEntity(organizeId);
                        DeptData dept = new DeptData();
                        dept.deptid = curUser.OrganizeId;
                        dept.code = curUser.OrganizeCode;
                        dept.isorg = 1;
                        dept.oranizeid = curUser.OrganizeId;
                        dept.parentcode = "";
                        dept.parentid = parentId;
                        dept.name = curUser.OrganizeName;
                        dept.isparent = true;
                        list = GetChangeDept(dept, "1");
                        dept.children = list;
                        dept.isoptional = "1";
                        result.Add(dept);
                    }
                    else
                    {
                        DeptData dept = new DeptData();
                        dept.deptid = curUser.DeptId;
                        dept.code = curUser.DeptCode;
                        dept.isorg = 0;
                        dept.oranizeid = curUser.OrganizeId;
                        dept.parentcode = "";
                        dept.parentid = parentId;
                        dept.name = curUser.DeptName;
                        dept.isparent = true;
                        list = GetChangeDept(dept);
                        dept.children = list;
                        result.Add(dept);
                        var deptcontract = departmentBLL.GetList().Where(t => t.OrganizeId == organizeId && t.Description == "外包工程承包商").OrderBy(t => t.SortCode).ToList().FirstOrDefault();
                        if (deptcontract != null)
                        {
                            DeptData dept1 = new DeptData();
                            dept1.deptid = deptcontract.DepartmentId;
                            dept1.code = deptcontract.DeptCode;
                            dept1.isorg = 0;
                            dept1.oranizeid = curUser.OrganizeId;
                            dept1.parentcode = "";
                            dept1.parentid = parentId;
                            dept1.name = deptcontract.FullName;
                            dept1.isparent = true;
                            list = GetContronDept(dept1, curUser.OrganizeId, curUser.DeptId);
                            dept1.children = list;
                            if (list.Count() > 0)
                            {
                                result.Add(dept1);
                            }
                        }
                    }
                }
                result = ExchangeData(result.ToList());
                return new { code = 0, info = "获取数据成功", count = result.Count, data = result };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 5.新增分配任务
        /// <summary>
        /// 5.新建监督任务
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object AddControlTask()
        {
            try
            {
                string res = ctx.Request["json"];
                var dy = JsonConvert.DeserializeAnonymousType(res, new
                {
                    userid = string.Empty,
                    data = new
                    {
                        id = string.Empty,
                        tasktype = string.Empty,
                        taskshare = new TaskShareEntity(),
                        workspecs = new List<SuperviseWorkInfoEntity>(),
                        teamspec = new List<TeamsInfoEntity>(),
                        staffspec = new List<StaffInfoEntity>()
                    }
                });
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (curUser == null)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                if (dy.data == null)
                {
                    throw new ArgumentException("出错，错误信息：data为null");
                }
                if (dy.data.id == null)
                {
                    throw new ArgumentException("缺少参数：id为空");
                }
                string id = dy.data.id;//申请id
                string jsondata = JsonConvert.SerializeObject(dy.data.taskshare);
                TaskShareEntity model = JsonConvert.DeserializeObject<TaskShareEntity>(jsondata);
                string json = JsonConvert.SerializeObject(dy.data.taskshare);
                if (dy.data.tasktype != "0")
                {
                    if (string.IsNullOrEmpty(id))
                    {
                        id = Guid.NewGuid().ToString();
                        model.Id = id;
                    }
                }
                if (dy.data.workspecs != null)
                {
                    foreach (var item in dy.data.workspecs)
                    {
                        item.TaskShareId = id;
                    }
                }
                if (dy.data.teamspec != null)
                {
                    foreach (var item in dy.data.teamspec)
                    {
                        item.TaskShareId = id;
                    }
                }
                if (dy.data.staffspec != null)
                {
                    foreach (var item in dy.data.staffspec)
                    {
                        item.TaskShareId = id;
                    }
                }
                model.WorkSpecs = dy.data.workspecs;
                model.TeamSpec = dy.data.teamspec;
                model.StaffSpec = dy.data.staffspec;
                tasksharebll.SaveForm(id, model);
                return new { code = 0, count = 0, info = "保存成功" };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }
        #endregion

        #region 6.获取监督任务详情
        /// <summary>
        /// 6.获取监督任务详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object ControlTaskDetail([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                var dy = JsonConvert.DeserializeAnonymousType(res, new
                {
                    userid = string.Empty,
                    data = new
                    {
                        id = string.Empty
                    }
                });
                //获取用户Id
                OperatorProvider.AppUserId = dy.userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, info = "请求失败,请登录!" };
                }
                if (dy.data == null)
                {
                    throw new ArgumentException("缺少参数：data为空");
                }
                if (string.IsNullOrEmpty(dy.data.id))
                {
                    throw new ArgumentException("缺少参数：id为空");
                }
                var share = tasksharebll.GetEntity(dy.data.id);
                if (share == null)
                {
                    throw new ArgumentException("未找到信息");
                }
                string jsondata = JsonConvert.SerializeObject(share);
                TaskShareEntity taskshare = JsonConvert.DeserializeObject<TaskShareEntity>(jsondata);
                taskshare.WorkSpecs = new SuperviseWorkInfoBLL().GetList(string.Format(" and taskshareid='{0}'", taskshare.Id)).ToList();

                var teamid = "";
                List<TeamsInfoEntity> teaminfo = new List<TeamsInfoEntity>();
                if (curUser.RoleName.Contains("班组"))
                {
                    teamid = curUser.DeptId;
                    teaminfo = new TeamsInfoBLL().GetAllList(string.Format(" and taskshareid='{0}' and  teamid='{1}'", taskshare.Id, curUser.DeptId)).ToList();
                }
                else
                {
                    teaminfo = new TeamsInfoBLL().GetAllList(string.Format(" and taskshareid='{0}'", taskshare.Id)).ToList();
                }
                string userids = "";
                List<UserEntity> users = new List<UserEntity>();
                if (share.TaskType != "2" && (share.FlowStep == "2" || share.FlowStep == "3"))
                {
                    if (taskshare.TaskType == "0")
                    {
                        string rolenames = dataitemdetailbll.GetItemValue(curUser.OrganizeId, "deptsuperviserole");
                        if (!string.IsNullOrEmpty(rolenames))
                        {
                            rolenames = "'" + rolenames.Replace(",", "','") + "'";
                            users = userbll.GetUserListByRoleName("'" + taskshare.SuperviseDeptId + "'", rolenames, true, string.Empty).ToList();
                        }
                    }
                    var userEntity = userbll.GetEntity(taskshare.CreateUserId);
                    if (userEntity != null)
                        users.Add(userEntity);
                }
                if (users != null && users.Count > 0)
                {
                    userids = string.Join(",", users.Select(x => x.UserId).ToArray());
                }
                teaminfo.ForEach(t =>
                {
                    t.ModifyUserId = userids;//modifyuserid借用该字段
                });
                taskshare.TeamSpec = teaminfo;

                var s = new
                {
                    taskshareid = share.Id,
                    teamid = teamid,
                    tasklevel = "1"
                };
                taskshare.StaffSpec = staffinfobll.GetList(JsonConvert.SerializeObject(s)).ToList();
                if (taskshare.StaffSpec != null)
                {
                    taskshare.StaffSpec.ForEach(t =>
                    {
                        t.SpecialtyTypeName = !string.IsNullOrEmpty(t.SpecialtyType) ? scaffoldbll.getName(t.SpecialtyType, "SpecialtyType") : "";
                    });
                }
                Dictionary<string, string> dict_props = new Dictionary<string, string>();
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                    DateFormatString = "yyyy-MM-dd HH:mm", //格式化日期
                };
                return new { code = 0, info = "获取数据成功", data = JObject.Parse(JsonConvert.SerializeObject(taskshare, Formatting.None, settings)) };
            }
            catch (Exception ex)
            {
                return new { code = -1, data = "", info = ex.Message };
            }
        }
        #endregion

        #region 7.获取旁站监督分配任务
        [HttpPost]
        public object GetTaskList([FromBody]JObject json)
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
                        searchtype = string.Empty //0:待分配 1:全部
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

                var watch = CommonHelper.TimerStart();

                Pagination pagination = new Pagination();
                pagination.conditionJson = " 1=1 ";
                pagination.page = dy.data.pagenum;
                pagination.rows = dy.data.pagesize;
                pagination.sidx = "a.createdate";     //排序字段
                pagination.sord = "desc";             //排序方式
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var list = tasksharebll.GetDataTable(pagination, JsonConvert.SerializeObject(dy.data), "app");
                Dictionary<string, string> dict_props = new Dictionary<string, string>();
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                    DateFormatString = "yyyy-MM-dd HH:mm", //格式化日期
                };
                return new { code = 0, info = "获取数据成功", count = pagination.records, data = JArray.Parse(JsonConvert.SerializeObject(list, Formatting.None, settings)) };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message };
            }

        }
        #endregion

        #region 8.获取旁站监督列表
        /// <summary>
        /// 8.获取旁站监督列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetSideSuperviseList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }

                string type = res.Contains("type") ? dy.data.type : "";//1:我的 2:全部 3.我的监管 4:全部监管
                string statevalue = res.Contains("statevalue") ? dy.data.statevalue : "";//监督状态值
                string starttime = res.Contains("starttime") ? dy.data.starttime : "";//旁站开始时间
                string endtime = res.Contains("endtime") ? dy.data.endtime : "";//旁站结束时间
                string deptid = res.Contains("deptid") ? dy.data.deptid : "";//旁站监督单位id
                string deptcode = res.Contains("deptcode") ? dy.data.deptcode : "";//旁站监督单位code
                string sideuserid = res.Contains("sideuserid") ? dy.data.sideuserid : "";//旁站监督人员id
                //获取页数和条数
                int page = Convert.ToInt32(dy.data.pagenum), rows = Convert.ToInt32(dy.data.pagesize);
                Pagination pagination = new Pagination();
                pagination.p_kid = "a.id";
                if (type == "1" || type == "2")
                {
                    pagination.p_fields = "a.workinfoid,a.workinfoname,a.pteamname,a.taskusername,a.pstarttime,a.pendtime,a.supervisestate";
                }
                else
                {
                    pagination.p_fields = "a.workinfoid,a.workinfoname,a.pteamname,a.taskusername,a.pstarttime,a.pendtime,nvl(b.dataissubmit,0) as dataissubmit";
                }
                if (type == "1" || type == "2")
                {
                    pagination.p_tablename = "bis_staffinfo a";
                    pagination.conditionJson = "tasklevel='2'";
                }
                else
                {
                    pagination.p_tablename = "bis_staffinfo a  left join bis_taskurge b on a.id=b.staffid";
                    pagination.conditionJson = "tasklevel='1' and a.dataissubmit='1'";
                }
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "a.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                if (type == "1")//我的旁站监督任务
                {
                    pagination.conditionJson += string.Format("  and taskuserid='{0}' and dataissubmit='0'", curUser.UserId);
                }
                else if (type == "2")
                {
                    if (!curUser.IsSystem)
                    {
                        if (curUser.RoleName.Contains("公司") || curUser.RoleName.Contains("厂级"))
                        {
                            pagination.conditionJson += string.Format("  and a.createuserorgcode='{0}'", curUser.OrganizeCode);
                        }
                        else
                        {
                            pagination.conditionJson += string.Format(@"  and a.pteamid in(select to_char(departmentid) from base_department t where t.organizeid='{0}' and t.encode like '{1}%' union  select  to_char(outprojectid) from epg_outsouringengineer a where a.engineerletdeptid = '{2}')", curUser.OrganizeId, curUser.DeptCode, curUser.DeptId);
                        }
                    }
                }
                else if (type == "3")//我的监管
                {
                    #region 我的监管
                    string roleCondition = "";
                    string strWhere = "";
                    if (curUser.RoleName.Contains("厂级"))
                    {
                        roleCondition = dataitemdetailbll.GetItemValue(curUser.OrganizeId, "urgerole");//安全主管部门监管角色
                        strWhere = string.Format("  and a.id not in(select  distinct(a.staffid) from bis_taskurge a where a.dataissubmit='1') and a.createuserorgcode='{0}'", curUser.OrganizeCode);
                    }
                    else if (curUser.RoleName.Contains("部门级用户"))
                    {
                        roleCondition = dataitemdetailbll.GetItemValue(curUser.OrganizeId, "sidepersonurgerole");//旁站监督所在部门
                        strWhere = string.Format(@"  and a.id not in(select  distinct(a.staffid) from bis_taskurge a where a.dataissubmit='1') and a.createuserorgcode='{0}'
                        and pteamid in(select to_char(departmentid) from base_department t where t.organizeid='{1}' and t.encode like '{2}%' union 
                        select to_char(outprojectid) from epg_outsouringengineer a where a.engineerletdeptid = '{3}')", curUser.OrganizeCode, curUser.OrganizeId, curUser.DeptCode, curUser.DeptId);
                        if (curUser.RoleName.Contains("专工") && !string.IsNullOrEmpty(curUser.SpecialtyType))
                        {
                            string strSpecialtyType = "'" + curUser.SpecialtyType.Replace(",", "','") + "'";
                            strWhere += " and SpecialtyType in (" + strSpecialtyType + ")";
                        }
                    }
                    else if (curUser.RoleName.Contains("承包商用户"))
                    {
                        roleCondition = dataitemdetailbll.GetItemValue(curUser.OrganizeId, "contracturgerole");//承包商
                        strWhere = string.Format("  and a.id not in(select  distinct(a.staffid) from bis_taskurge a where a.dataissubmit='1') and a.createuserorgcode='{0}' and pteamid in(select departmentid from base_department t where t.organizeid='{1}' and t.encode like '{2}%')", curUser.OrganizeCode, curUser.OrganizeId, curUser.DeptCode);
                    }
                    if (!string.IsNullOrEmpty(roleCondition))
                    {
                        var isuse = roleCondition.Split('|');
                        string[] arrrolename = isuse[0].Split(',');
                        if (isuse.Length >= 2 && isuse[1] == "1")
                        {
                            var num = 0;
                            for (int i = 0; i < arrrolename.Length; i++)
                            {
                                if (curUser.RoleName.Contains(arrrolename[i]))
                                    num++;
                                if (num != 0)
                                    break;
                            }
                            if (num != 0)
                                pagination.conditionJson += strWhere;
                            else
                                pagination.conditionJson += " and 1=2";
                        }
                        else
                        {
                            pagination.conditionJson += " and 1=2";
                        }
                    }
                    else
                    {
                        pagination.conditionJson += " and 1=2";
                    }
                    #endregion
                }
                else if (type == "4")//全部
                {
                    if (!curUser.IsSystem)
                    {
                        if (curUser.RoleName.Contains("公司") || curUser.RoleName.Contains("厂级"))
                        {
                            pagination.conditionJson += string.Format("  and a.createuserorgcode='{0}'", curUser.OrganizeCode);
                        }
                        else
                        {
                            pagination.conditionJson += string.Format(@"  and a.pteamid in(select to_char(departmentid) from base_department t where t.organizeid='{0}' and t.encode like '{1}%' union  select  to_char(outprojectid) from epg_outsouringengineer a where a.engineerletdeptid = '{2}')", curUser.OrganizeId, curUser.DeptCode, curUser.DeptId);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(statevalue))//监督状态/监管状态
                {
                    if (type == "1" || type == "2")
                    {
                        pagination.conditionJson += string.Format(" and supervisestate='{0}'", statevalue);
                    }
                    else
                    {
                        pagination.conditionJson += string.Format("  and nvl(b.dataissubmit,0)='{0}'", statevalue);
                    }
                }
                if (!string.IsNullOrEmpty(starttime))//旁站开始时间
                {
                    pagination.conditionJson += string.Format(" and pstarttime>=to_date('{0}','yyyy-mm-dd')", starttime);
                }
                if (!string.IsNullOrEmpty(endtime))//旁站结束时间
                {
                    endtime = Convert.ToDateTime(endtime).AddDays(1).ToString("yyyy-MM-dd");
                    pagination.conditionJson += string.Format(" and pendtime<=to_date('{0}','yyyy-mm-dd')", endtime);
                }
                if (!string.IsNullOrEmpty(deptid))//旁站监督单位
                {
                    pagination.conditionJson += string.Format(" and pteamid='{0}'", deptid);
                }
                if (!string.IsNullOrEmpty(sideuserid))//监督员
                {
                    pagination.conditionJson += string.Format(" and  ','||taskuserid||',' like '%,{0},%'", sideuserid);
                }
                DataTable dt = sidepersonbll.GetPageDataTable(pagination, null);
                Dictionary<string, string> dict_props = new Dictionary<string, string>();
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                    DateFormatString = "yyyy-MM-dd HH:mm", //格式化日期
                };
                return new { code = 0, info = "获取数据成功", count = pagination.records, data = JArray.Parse(JsonConvert.SerializeObject(dt, Formatting.None, settings)) };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }
        #endregion

        #region 9.获取旁站监督任务详情
        /// <summary>
        /// 9.获取旁站监督任务详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetMuchSuperviseDetail([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                var dy = JsonConvert.DeserializeAnonymousType(res, new
                {
                    userid = string.Empty,
                    data = new
                    {
                        id = string.Empty
                    }
                });
                //获取用户Id
                OperatorProvider.AppUserId = dy.userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, info = "请求失败,请登录!" };
                }
                if (dy.data == null)
                {
                    throw new ArgumentException("缺少参数：data为空");
                }
                if (string.IsNullOrEmpty(dy.data.id))
                {
                    throw new ArgumentException("缺少参数：id为空");
                }
                var taskentity = staffinfobll.GetEntity(dy.data.id);
                if (taskentity == null)
                {
                    throw new ArgumentException("未找到信息");
                }
                string webUrl = dataitemdetailbll.GetItemValue("imgUrl");
                string jsondata = JsonConvert.SerializeObject(taskentity);
                TaskModel taskmodel = JsonConvert.DeserializeObject<TaskModel>(jsondata);
                taskmodel.workspecs = new SuperviseWorkInfoBLL().GetList(string.Format(" and id='{0}'", taskentity.WorkInfoId)).ToList();
                List<TaskUrgeEntity> urge = new TaskUrgeBLL().GetList(string.Format(" and staffid='{0}'", taskentity.Id)).ToList();
                List<TaskUrgeData> datalist = new List<TaskUrgeData>();
                if (urge != null)
                {
                    foreach (var item in urge)
                    {
                        TaskUrgeData tasku = new TaskUrgeData();
                        tasku.Id = item.Id;
                        tasku.Idea = item.Idea;
                        tasku.StaffId = item.StaffId;
                        tasku.UrgeTime = item.UrgeTime;
                        tasku.UrgeUserId = item.UrgeUserId;
                        tasku.UrgeUserName = item.UrgeUserName;
                        tasku.DataIsSubmit = item.DataIsSubmit;
                        tasku.DeptName = item.DeptName;
                        tasku.DeptId = item.DeptId;
                        tasku.DeptCode = item.DeptCode;
                        tasku.SignPic = string.IsNullOrWhiteSpace(item.SignPic) ? "" : webUrl + item.SignPic.Replace("../../", "/");
                        DataTable cdt = fileInfoBLL.GetFiles(item.Files);
                        IList<ERCHTMS.AppSerivce.Model.Photo> cfiles = new List<ERCHTMS.AppSerivce.Model.Photo>(); //图片
                        foreach (DataRow itemfile in cdt.Rows)
                        {
                            ERCHTMS.AppSerivce.Model.Photo p = new ERCHTMS.AppSerivce.Model.Photo();
                            p.id = itemfile[0].ToString();
                            p.filename = itemfile[1].ToString();
                            p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + itemfile[2].ToString().Substring(1);
                            cfiles.Add(p);
                        }
                        tasku.urgefile = cfiles;
                        datalist.Add(tasku);
                    }
                }
                taskmodel.taskurge = datalist;
                var teamid = "";
                if (curUser.RoleName.Contains("班组"))
                {
                    teamid = curUser.DeptId;
                }
                var single = new
                {
                    taskshareid = taskentity.TaskShareId,
                    staffid = taskentity.Id,
                    teamid = teamid
                };
                List<StaffInfoEntity> slist = staffinfobll.GetList(JsonConvert.SerializeObject(single)).ToList();
                taskmodel.singletask = slist;
                Dictionary<string, string> dict_props = new Dictionary<string, string>();
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                    DateFormatString = "yyyy-MM-dd HH:mm", //格式化日期
                };
                return new { code = 0, info = "获取数据成功", data = JObject.Parse(JsonConvert.SerializeObject(taskmodel, Formatting.None, settings)) };
            }
            catch (Exception ex)
            {
                return new { code = -1, data = "", info = ex.Message };
            }
        }
        #endregion

        #region 10 获取单人监督任务
        /// <summary>
        /// 10.获取单人旁站监督任务详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetSingleSuperviseDetail([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                var dy = JsonConvert.DeserializeAnonymousType(res, new
                {
                    userid = string.Empty,
                    data = new
                    {
                        id = string.Empty
                    }
                });
                //获取用户Id
                OperatorProvider.AppUserId = dy.userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, info = "请求失败,请登录!" };
                }
                if (dy.data == null)
                {
                    throw new ArgumentException("缺少参数：data为空");
                }
                if (string.IsNullOrEmpty(dy.data.id))
                {
                    throw new ArgumentException("缺少参数：id为空");
                }
                var singletaskentity = staffinfobll.GetEntity(dy.data.id);
                if (singletaskentity == null)
                {
                    throw new ArgumentException("未找到信息");
                }
                string jsondata = JsonConvert.SerializeObject(singletaskentity);
                SingleTaskModel singletaskmodel = JsonConvert.DeserializeObject<SingleTaskModel>(jsondata);

                var signlist = tasksignbll.GetTaskSignInfo(singletaskentity.Id);
                List<SignData> signdatalist = new List<SignData>();
                foreach (var signitem in signlist)
                {
                    SignData signentity = new SignData();
                    signentity.signid = signitem.Id;
                    signentity.supervisetime = Convert.ToDateTime(signitem.SuperviseTime).ToString("yyyy-MM-dd HH:mm:ss");
                    signentity.supervisestate = signitem.SuperviseState;
                    DataTable cdt = fileInfoBLL.GetFiles(signitem.Signfile);
                    IList<ERCHTMS.AppSerivce.Model.Photo> cfiles = new List<ERCHTMS.AppSerivce.Model.Photo>(); //监督签到图片
                    foreach (DataRow item in cdt.Rows)
                    {
                        ERCHTMS.AppSerivce.Model.Photo p = new ERCHTMS.AppSerivce.Model.Photo();
                        p.id = item[0].ToString();
                        p.filename = item[1].ToString();
                        p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + item[2].ToString().Substring(1);
                        cfiles.Add(p);
                    }
                    signentity.signfile = cfiles;
                    signdatalist.Add(signentity);
                }
                singletaskmodel.signlist = signdatalist;
                List<SuperviseWorkInfoEntity> sworklist = new List<SuperviseWorkInfoEntity>();
                SuperviseWorkInfoEntity swentity = new SuperviseWorkInfoBLL().GetEntity(singletaskentity.WorkInfoId);
                sworklist.Add(swentity);
                singletaskmodel.workspecs = sworklist;
                var bigchecklist = sidecheckprojectbll.GetBigCheckInfo();
                foreach (var item in bigchecklist)
                {
                    var resultentity = taskrelevanceprojectbll.GetCheckResultInfo(item.Id, singletaskentity.Id);
                    if (resultentity != null)
                    {
                        item.CheckNumber = resultentity.IsCorrespond;
                    }
                    else
                    {
                        item.CheckNumber = "0";
                    }
                }
                //总的小项检查项目
                var allsmallnum = sidecheckprojectbll.GetAllSmallCheckInfo("").Count();
                //已检查项目
                var endchecknum = taskrelevanceprojectbll.GetEndCheckInfo(singletaskentity.Id).Count();
                bool flag = false;
                if (endchecknum == allsmallnum)//说明已检查完成
                {
                    flag = true;
                }
                singletaskmodel.ischeck = flag;
                string checkjson = JsonConvert.SerializeObject(bigchecklist.Select(x => new { bigcheckid = x.Id, checkcontent = x.CheckContent, isnosuit = x.CheckNumber, ischeckaccomplish = GetBigCheckFlag(x.Id, singletaskentity.Id) }));
                List<BigCheckData> checklist = JsonConvert.DeserializeObject<List<BigCheckData>>(checkjson);
                singletaskmodel.bigchecklist = checklist;

                Dictionary<string, string> dict_props = new Dictionary<string, string>();
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                    DateFormatString = "yyyy-MM-dd HH:mm", //格式化日期
                };
                return new { code = 0, info = "获取数据成功", data = JObject.Parse(JsonConvert.SerializeObject(singletaskmodel, Formatting.None, settings)) };
            }
            catch (Exception ex)
            {
                return new { code = -1, data = "", info = ex.Message };
            }
        }
        #endregion

        #region 11.获取检查项目
        /// <summary>
        /// 11.获取检查项目
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetCheckProjectList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string id = dy.data.id;//监督任务id
                string parentid = dy.data.parentid;//大项检查项目
                var checkprojectlist = sidecheckprojectbll.GetAllSmallCheckInfo(parentid);

                List<SmallCheckData> smallchecklist = new List<SmallCheckData>();
                foreach (var item in checkprojectlist)
                {
                    var resultentity = taskrelevanceprojectbll.GetCheckResultInfo(item.Id, id);
                    SmallCheckData smallcheckentity = new SmallCheckData();
                    smallcheckentity.checkid = item.Id;//检查项目id
                    smallcheckentity.checkcontent = item.CheckContent;//检查项目内容
                    smallcheckentity.checkresult = resultentity == null ? "" : resultentity.IsCorrespond;//若没检查显示为空,否则显示已检查结果
                    smallchecklist.Add(smallcheckentity);
                }
                return new
                {
                    Code = 0,
                    Count = smallchecklist.Count,
                    Info = "获取数据成功",
                    data = smallchecklist
                };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }
        #endregion

        #region 12.获取检查项目详情
        /// <summary>
        /// 12.获取检查项目详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetCheckProjectDetail([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string id = dy.data.id;//监督任务id
                string checkid = dy.data.checkid;//检查项目id
                var checkresultentity = taskrelevanceprojectbll.GetCheckResultInfo(checkid, id);
                var checkentity = sidecheckprojectbll.GetEntity(checkid);
                var checkresult = "";
                var remark = "";
                IList<ERCHTMS.AppSerivce.Model.Photo> cfiles = new List<ERCHTMS.AppSerivce.Model.Photo>(); //检查项目图片
                if (checkresultentity != null)
                {
                    checkresult = checkresultentity.IsCorrespond;//是否符合要求
                    remark = checkresultentity.Remark;//备注
                    DataTable cdt = fileInfoBLL.GetFiles(checkresultentity.CFiles);
                    foreach (DataRow item in cdt.Rows)
                    {
                        ERCHTMS.AppSerivce.Model.Photo p = new ERCHTMS.AppSerivce.Model.Photo();
                        p.id = item[0].ToString();
                        p.filename = item[1].ToString();
                        p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + item[2].ToString().Substring(1);
                        cfiles.Add(p);
                    }
                }
                int hidnum = htbaseinfobll.GetList(string.Format(" and RelevanceId='{0}' and  relevanceType ='{1}'", id, checkid)).ToList().Count;
                int breaknum = lllegalregisterbll.GetList(string.Format(" and Reseverone='{0}' and  Resevertwo='{1}'", id, checkid)).ToList().Count;
                return new
                {
                    Code = 0,
                    Count = "-1",
                    Info = "获取数据成功",
                    //hiddentnum 隐患数量 breaknum违章数量
                    data = new { checkid = checkentity.Id, checkcontent = checkentity.CheckContent, checkresult = checkresult, remark = remark, cfiles = cfiles, hiddentnum = hidnum, breaknum = breaknum }
                };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }
        #endregion

        #region 13.提交检查项目
        /// <summary>
        /// 13.提交检查项目
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object AddCheckProject()
        {
            try
            {
                string res = ctx.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                TaskRelevanceProjectEntity entity = new TaskRelevanceProjectEntity();
                entity.SuperviseId = dy.data.id;//监督任务id
                entity.CheckProjectId = dy.data.checkid;//检查项目id
                entity.Remark = dy.data.remark;//备注
                entity.IsCorrespond = dy.data.checkresult;//是否符合要求(1.是 2.否 3.无此项)
                entity.CFiles = Guid.NewGuid().ToString();
                HttpFileCollection files = ctx.Request.Files;//上传的文件 
                //上传检查项目图片
                UploadifyFile(entity.CFiles, "highriskwork", files);
                taskrelevanceprojectbll.SaveForm("", entity);
                return new { code = 0, count = 0, info = "保存成功" };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }
        #endregion

        #region 14.监督任务
        /// <summary>
        /// 14.监督任务
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object AddSuperviseTask()
        {
            try
            {
                string res = ctx.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }

                string id = dy.data.id;//监督任务id
                var supervise = staffinfobll.GetEntity(id);
                supervise.OrganizeManager = dy.data.organizemanager;//组织管理
                supervise.RiskAnalyse = dy.data.riskanalyse;//危险分析
                supervise.ConstructLayout = dy.data.constructlayout;//施工布置
                supervise.SafetyMeasure = dy.data.safetymeasure;//安全措施
                supervise.Evaluate = dy.data.evaluate;//施工评价
                supervise.SumTimeStr = string.IsNullOrEmpty(dy.data.sumtimestr) ? 0 : Convert.ToInt32(dy.data.sumtimestr);//监督时长
                supervise.IsFinish = dy.data.isfinish;//是否勾选[作业已全部完成，旁站监督任务全部结束]选项,1:已勾选
                bool flag = true;
                foreach (var item in dy.data.bigchecklist)
                {
                    var task = taskrelevanceprojectbll.GetCheckResultInfo(item.bigcheckid, id);
                    if (task != null)
                        taskrelevanceprojectbll.RemoveForm(task.Id);

                    TaskRelevanceProjectEntity entity = new TaskRelevanceProjectEntity();
                    entity.SuperviseId = dy.data.id;//监督任务id
                    entity.CheckProjectId = item.bigcheckid;//检查项目id
                    entity.IsCorrespond = item.isnosuit;//0:适宜  1:不适宜
                    taskrelevanceprojectbll.SaveForm("", entity);

                    var checkprojectlist = sidecheckprojectbll.GetAllSmallCheckInfo(item.bigcheckid);
                    if (checkprojectlist != null)
                    {
                        foreach (var checkproject in checkprojectlist)
                        {
                            var tasksamll = taskrelevanceprojectbll.GetCheckResultInfo(checkproject.Id, id);
                            if (item.isnosuit == "1")
                            {
                                if (tasksamll != null)
                                    taskrelevanceprojectbll.RemoveForm(tasksamll.Id);
                            }
                            else if (item.isnosuit == "0" && dy.data.type == "1")
                            {
                                if (tasksamll == null)
                                    flag = false;
                            }
                        }
                    }
                }
                //保存或提交验证通过
                if (dy.data.type == "0" || (dy.data.type == "1" && flag))
                {
                    #region 签到
                    var signlist = dy.data.signlist;
                    foreach (var item in signlist)
                    {
                        TaskSignEntity signentity = new TaskSignEntity();
                        signentity.SuperviseTime = string.IsNullOrEmpty(item.supervisetime) ? "" : Convert.ToDateTime(item.supervisetime);//监督时间
                        signentity.SuperviseState = item.supervisestate;//监督状态
                        signentity.SuperviseId = id;//监督任务id
                        signentity.Signfile = item.signid;
                        tasksignbll.SaveForm("", signentity);
                    }
                    HttpFileCollection files = ctx.Request.Files;//上传的文件 
                    //上传签到监督图片
                    AllUploadifyFile("highriskwork", files);
                    #endregion
                }
                if (dy.data.type == "1")
                {
                    if (flag)
                    {
                        supervise.SuperviseState = "1";// 1.提交(已监督)
                        supervise.DataIsSubmit = "1";//1.提交
                        staffinfobll.SaveForm(id, supervise);
                        var single = new { taskshareid = supervise.TaskShareId, staffid = supervise.StaffId };
                        var mushstaff = staffinfobll.GetEntity(supervise.StaffId);
                        List<StaffInfoEntity> alllist = staffinfobll.GetList(JsonConvert.SerializeObject(single)).ToList();

                        #region  提交时更改多人记录的监督状态及监督时长
                        //监督时长
                        int sumtime = 0;
                        //监督状态
                        int singnum = 0;//单人未监督数量
                        int singisfinish = 0;//单人是否完成
                        foreach (var staff in alllist)
                        {
                            if (staff.SuperviseState == "0")
                            {
                                singnum++;
                            }
                            if (staff.IsFinish == "1")
                            {
                                singisfinish++;
                            }
                            if (!string.IsNullOrEmpty(staff.SumTimeStr.ToString()))
                            {
                                sumtime += Convert.ToInt32(staff.SumTimeStr);
                            }
                        }
                        if (alllist != null && alllist.Count() >= 0)
                        {
                            mushstaff.IsFinish = singisfinish == alllist.Count() ? "1" : "0";
                        }
                        mushstaff.SumTimeStr = sumtime;//总时长
                        if (dy.data.type == "1")
                        {
                            if (singnum == 0)
                            {
                                mushstaff.SuperviseState = "1";//已监督
                            }
                            else if (singnum < alllist.Count())
                            {
                                mushstaff.SuperviseState = "2";//监督中
                            }
                            #region 发送消息给创建人和部门分配人员
                            var taskShareEntity = tasksharebll.GetEntity(supervise.TaskShareId);
                            if (taskShareEntity != null)
                            {
                                if (!string.IsNullOrEmpty(mushstaff.IsFinish))
                                {
                                    if (mushstaff.IsFinish == "1")
                                    {
                                        List<UserEntity> users = new List<UserEntity>();
                                        //0:发送创建人和部门分配人员 1:发送创建人 2:不发送
                                        if (taskShareEntity.TaskType == "0" || taskShareEntity.TaskType == "1")
                                        {
                                            if (taskShareEntity.TaskType == "0")
                                            {
                                                string rolenames = dataitemdetailbll.GetItemValue(curUser.OrganizeId, "deptsuperviserole");
                                                if (!string.IsNullOrEmpty(rolenames))
                                                {
                                                    rolenames = "'" + rolenames.Replace(",", "','") + "'";
                                                    users = userbll.GetUserListByRoleName("'" + taskShareEntity.SuperviseDeptId + "'", rolenames, true, string.Empty).ToList();
                                                }
                                            }
                                            var userEntity = userbll.GetEntity(taskShareEntity.CreateUserId);
                                            if (userEntity != null)
                                                users.Add(userEntity);
                                        }
                                        if (users != null && users.Count > 0)
                                        {
                                            var workInfo = new SuperviseWorkInfoBLL().GetList(string.Format(" and id='{0}'", supervise.WorkInfoId)).ToList();
                                            string workname = "";
                                            if (workInfo != null && workInfo.Count() > 0)
                                            {
                                                if (!string.IsNullOrEmpty(workInfo.FirstOrDefault().WorkName))
                                                {
                                                    workname = workInfo.FirstOrDefault().WorkName;
                                                }
                                            }
                                            string names = string.Join(",", users.Select(x => x.RealName).ToArray());
                                            string accounts = string.Join(",", users.Select(x => x.Account).ToArray());
                                            var senduser = userbll.GetUserInfoByAccount("System");
                                            MessageEntity msg = new MessageEntity
                                            {
                                                Title = "旁站监督",
                                                Content = workname + "已全部完成,不需再进行旁站监督任务,请结束旁站任务分配。",
                                                UserId = accounts,
                                                UserName = names,
                                                Status = "",
                                                Url = string.Empty,
                                                SendUser = senduser.Account,
                                                SendUserName = senduser.RealName,
                                                Category = "高风险作业"
                                            };
                                            if (new MessageBLL().SaveForm("", msg))
                                            {
                                                JPushApi.PublicMessage(msg);
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        staffinfobll.SaveForm(mushstaff.Id, mushstaff);
                        #endregion

                        #region 监督记录同步班组
                        if (StartUp() == "1")
                        {
                            if (dy.data.type == "1")
                            {

                                // var relist = alllist.Where(t => t.SuperviseState == "0").ToList();
                                //所有小项的监督任务全部完成，才提交班组数据
                                //if (relist.Count() <= 0)
                                //{
                                var remark = "";
                                IList<ERCHTMS.AppSerivce.Model.Photo> cfiles = new List<ERCHTMS.AppSerivce.Model.Photo>(); //检查项目图片
                                foreach (var staffitem in alllist)
                                {
                                    var result = dataitemdetailbll.GetItemValue("检查内容id");
                                    var task = taskrelevanceprojectbll.GetCheckResultInfo(result, staffitem.Id);
                                    if (task != null)
                                    {
                                        DataTable cdt = fileInfoBLL.GetFiles(task.CFiles);
                                        foreach (DataRow item in cdt.Rows)
                                        {
                                            ERCHTMS.AppSerivce.Model.Photo p = new ERCHTMS.AppSerivce.Model.Photo();
                                            p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + item[2].ToString().Substring(1);
                                            cfiles.Add(p);
                                        }
                                        if (!string.IsNullOrEmpty(task.Remark))
                                        {
                                            remark += task.Remark + ",";
                                        }
                                    }
                                }
                                var tempdata = new
                                {
                                    TimeLength = sumtime,//监督时长
                                    Result = !string.IsNullOrEmpty(remark) ? remark.TrimEnd(',') : "",//备注
                                    RecId = supervise.StaffId,
                                    ImageList = string.Join(",", cfiles.Select(x => x.fileurl)),
                                    IsFinished = dy.data.type == "0" ? "undo" : "finish"
                                };
                                WebClient wc = new WebClient();
                                wc.Credentials = CredentialCache.DefaultCredentials;
                                //发送请求到web api并获取返回值，默认为post方式
                                try
                                {
                                    System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                                    nc.Add("json", Newtonsoft.Json.JsonConvert.SerializeObject(tempdata));

                                    string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                                    System.IO.File.AppendAllText(dataitemdetailbll.GetItemValue("imgPath") + "/logs/" + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：同步成功,数据为:" + Newtonsoft.Json.JsonConvert.SerializeObject(tempdata) + "\r\n");

                                    wc.UploadValuesAsync(new Uri(dataitemdetailbll.GetItemValue("bzurl") + "UpdateMonitorJob"), nc);

                                }
                                catch (Exception ex)
                                {
                                    //将同步结果写入日志文件
                                    string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                                    System.IO.File.AppendAllText(dataitemdetailbll.GetItemValue("imgPath") + "~/logs/" + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：数据失败" + ",异常信息：" + ex.Message + "\r\n");
                                }
                                //}
                            }
                        }
                        #endregion
                        return new { code = 0, count = 0, info = "保存成功" };
                    }
                    else
                    {
                        supervise.SuperviseState = "0";//0.保存
                        supervise.DataIsSubmit = "0";//0.保存
                        staffinfobll.SaveForm(id, supervise);
                        return new { code = 1, count = 0, info = "检查项目未检查完" };
                    }
                }
                else
                {
                    supervise.SuperviseState = "0";//0.保存(未监督) 
                    supervise.DataIsSubmit = "0";//0.保存
                    staffinfobll.SaveForm(id, supervise);
                    return new { code = 0, count = 0, info = "保存成功" };
                }
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }
        #endregion

        #region 获取大项是否已操作
        /// <summary>
        /// 获取大项是否已操作
        /// </summary>
        /// <param name="bigcheckid"></param>
        /// <returns></returns>
        public bool GetBigCheckFlag(string bigcheckid, string singleid)
        {
            bool flag = true;
            var checklist = sidecheckprojectbll.GetAllSmallCheckInfo(bigcheckid);
            if (checklist != null)
            {
                foreach (var checkproject in checklist)
                {
                    var resultentity = taskrelevanceprojectbll.GetCheckResultInfo(checkproject.Id, singleid);
                    if (resultentity == null)
                    {
                        flag = false;
                        break;
                    }
                }
            }
            return flag;
        }
        #endregion

        #region 16.新增监管信息
        /// <summary>
        /// 16.新增监管信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object AddTaskUrge()
        {
            try
            {
                string res = ctx.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string webUrl = dataitemdetailbll.GetItemValue("imgUrl");


                string id = dy.data.id;//监督任务id
                var item = dy.data.taskurge;
                TaskUrgeEntity urgeentity = new TaskUrgeEntity();
                urgeentity.UrgeTime = string.IsNullOrEmpty(item.urgetime) ? "" : Convert.ToDateTime(item.urgetime);//监管时间
                urgeentity.Idea = item.idea;//监管意见
                urgeentity.UrgeUserId = curUser.UserId;
                urgeentity.UrgeUserName = curUser.UserName;
                urgeentity.DeptId = curUser.DeptId;
                urgeentity.DeptCode = curUser.DeptCode;
                urgeentity.DeptName = curUser.DeptName;
                urgeentity.DataIsSubmit = item.dataissubmit;
                urgeentity.StaffId = id;//监督任务id
                string signpic = item.signpic.ToString();
                urgeentity.Files = Guid.NewGuid().ToString();
                urgeentity.SignPic = string.IsNullOrWhiteSpace(signpic) ? "" : signpic.Replace(webUrl, "").ToString();

                HttpFileCollection files = ctx.Request.Files;//上传的文件
                if (files.Count > 0)
                {
                    for (int i = 0; i < files.AllKeys.Length; i++)
                    {
                        HttpPostedFile file = files[i];
                        string fileOverName = System.IO.Path.GetFileName(file.FileName);
                        string fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                        string FileEextension = Path.GetExtension(file.FileName);
                        if (fileName.Split('_')[0] == "sign")
                        {
                            string dir = dataitemdetailbll.GetItemValue("imgPath") + "\\Resource\\sign";
                            string newFileName = fileName + FileEextension;
                            string newFilePath = dir + "\\" + newFileName;
                            file.SaveAs(newFilePath);
                            urgeentity.SignPic = "/Resource/sign/" + fileOverName;
                            break;
                        }
                        else
                        {
                            UploadSingleFile(urgeentity.Files, "file", file);
                        }
                    }
                }
                new TaskUrgeBLL().SaveForm("", urgeentity);
                return new { code = 0, count = 0, info = "保存成功" };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }
        #endregion

        #region 17.保存作业信息
        /// <summary>
        /// 17.保存作业信息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SaveWorkInfo([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string id = dy.data.id;//作业信息id
                string workticketno = dy.data.workticketno;//工作票号
                string workplace = dy.data.workplace;//工作地点
                string workcontent = dy.data.workcontent;//工作内容
                string workareaname = dy.data.workareaname;//工作区域
                string workinfotypeid = dy.data.workinfotypeid;//作业类别id
                string workinfotype = dy.data.workinfotype;//作业类别
                string handtype = dy.data.handtype;//手动输入的作业类别
                string workstarttime = dy.data.workstarttime;//作业开始时间
                string workendtime = dy.data.workendtime;//作业结束时间
                string workusernames = dy.data.workusernames;//作业人员
                string workprojectname = dy.data.workprojectname;//项目名称
                string workname = dy.data.workname; //作业信息名称
                var workentity = new SuperviseWorkInfoBLL().GetEntity(id);
                if (workentity != null)
                {
                    DateTime? timestart = null, timeend = null;
                    workentity.WorkAreaName = workareaname;
                    workentity.WorkTicketNo = workticketno;
                    workentity.WorkPlace = workplace;
                    workentity.WorkContent = workcontent;
                    workentity.WorkInfoType = workinfotype;
                    workentity.WorkInfoTypeId = workinfotypeid;
                    workentity.HandType = handtype;
                    workentity.WorkName = workname;

                    if (!string.IsNullOrEmpty(workstarttime))
                        timestart = Convert.ToDateTime(workstarttime);

                    if (!string.IsNullOrEmpty(workendtime))
                        timeend = Convert.ToDateTime(workendtime);

                    workentity.WorkStartTime = timestart;
                    workentity.WorkEndTime = timeend;
                    workentity.WorkUserNames = workusernames;
                    workentity.WorkProjectName = workprojectname;
                    new SuperviseWorkInfoBLL().SaveForm(id, workentity);
                }
                return new { code = 0, count = 0, info = "保存成功" };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }
        #endregion

        #region 18.任务已开始旁站，请前去进行监管
        /// <summary>
        /// 18.发消息去监管
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SendUrge([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string id = dy.data.id;//旁站监督任务id
                var staffInfoEntity = staffinfobll.GetEntity(id);
                if (staffInfoEntity == null)
                {
                    throw new ArgumentException("出错，此任务不存在");
                }
                var workInfo = new SuperviseWorkInfoBLL().GetList(string.Format(" and id='{0}'", staffInfoEntity.WorkInfoId)).ToList();
                string workname = "";
                if (workInfo != null && workInfo.Count() > 0)
                {
                    if (!string.IsNullOrEmpty(workInfo.FirstOrDefault().WorkName))
                    {
                        workname = workInfo.FirstOrDefault().WorkName;
                    }
                }
                var dept = departmentBLL.GetEntity(curUser.DeptId);
                string rolenames = "";
                IList<UserEntity> users1 = null;
                IList<UserEntity> users2 = null;
                IList<UserEntity> users3 = null;
                if (dept != null)
                {
                    if (curUser.RoleName.Contains("承包商"))//发送承包商,承包商对应的责任部门,安全主管部门
                    {
                        #region 承包商
                        rolenames = dataitemdetailbll.GetItemValue(curUser.OrganizeId, "contracturgerole");//承包商
                        if (!string.IsNullOrEmpty(rolenames))
                        {
                            var isuse = rolenames.Split('|');
                            string[] arrrolename = isuse[0].Split(',');
                            if (isuse.Length >= 2 && isuse[1] == "1")
                            {
                                rolenames = "'" + rolenames.Replace(",", "','") + "'";
                                users1 = userbll.GetUserListByRoleName("'" + dept.DepartmentId + "'", rolenames, true, string.Empty);
                            }
                        }
                        #endregion

                        #region 责任部门
                        DataTable dt = departmentBLL.GetDataTable(string.Format("select distinct  engineerletdeptid  from epg_outsouringengineer where outprojectid='{0}'", dept.DepartmentId));
                        StringBuilder sb = new StringBuilder();
                        foreach (DataRow dr in dt.Rows)
                        {
                            sb.AppendFormat("{0},", dr[0].ToString());
                        }
                        string dutydeptids = sb.ToString().Trim(',');
                        rolenames = dataitemdetailbll.GetItemValue(curUser.OrganizeId, "sidepersonurgerole");//所在部门
                        if (!string.IsNullOrEmpty(rolenames) && !string.IsNullOrEmpty(dutydeptids))
                        {
                            var isuse = rolenames.Split('|');
                            string[] arrrolename = isuse[0].Split(',');
                            if (isuse.Length >= 2 && isuse[1] == "1")
                            {
                                dutydeptids = "'" + dutydeptids.Replace(",", "','") + "'";
                                rolenames = "'" + rolenames.Replace(",", "','") + "'";
                                users3 = userbll.GetUserListByRoleName("'" + dutydeptids + "'", rolenames, true, string.Empty);
                            }
                        }
                        #endregion
                    }
                    else //发送对应的部门及安全主管部门
                    {
                        if (!curUser.RoleName.Contains("厂级"))
                        {
                            var deptid = dept.DepartmentId;
                            if (curUser.RoleName.Contains("班组"))
                            {
                                deptid = dept.ParentId;
                            }
                            rolenames = dataitemdetailbll.GetItemValue(curUser.OrganizeId, "sidepersonurgerole");//所在部门
                            if (!string.IsNullOrEmpty(rolenames))
                            {
                                var isuse = rolenames.Split('|');
                                string[] arrrolename = isuse[0].Split(',');
                                if (isuse.Length >= 2 && isuse[1] == "1")
                                {
                                    rolenames = "'" + rolenames.Replace(",", "','") + "'";
                                    users1 = userbll.GetUserListByRoleName("'" + deptid + "'", rolenames, true, string.Empty);
                                }
                            }
                        }
                    }
                    #region  安全主管部门
                    IEnumerable<DepartmentEntity> departs = departmentBLL.GetDepts(curUser.OrganizeId, 1);
                    string deptids = string.Empty;
                    //配置的角色
                    rolenames = dataitemdetailbll.GetItemValue(curUser.OrganizeId, "urgerole");
                    if (departs != null && departs.Count() > 0 && !string.IsNullOrEmpty(rolenames))
                    {
                        deptids = string.Join(",", departs.Select(x => x.DepartmentId).ToArray());
                        deptids = "'" + deptids.Replace(",", "','") + "'";
                        var isuse = rolenames.Split('|');
                        string[] arrrolename = isuse[0].Split(',');
                        if (isuse.Length >= 2 && isuse[1] == "1")
                        {
                            rolenames = "'" + rolenames.Replace(",", "','") + "'";
                            //安全主管部门，角色的用户
                            users2 = userbll.GetUserListByRoleName(deptids, rolenames, true, string.Empty);
                        }
                    }
                    #endregion
                    List<UserEntity> users = new List<UserEntity>();
                    if (users1 != null && users1.Count > 0)
                    {
                        users.AddRange(users1);
                        users = users.Union(users1).ToList();
                    }
                    if (users2 != null && users2.Count > 0)
                    {
                        users.AddRange(users2);
                        users = users.Union(users2).ToList();
                    }
                    if (users3 != null && users3.Count > 0)
                    {
                        users.AddRange(users3);
                        users = users.Union(users3).ToList();
                    }
                    if (users != null && users.Count > 0)
                    {
                        string names = string.Join(",", users.Select(x => x.RealName).ToArray());
                        string accounts = string.Join(",", users.Select(x => x.Account).ToArray());
                        var senduser = new UserBLL().GetUserInfoByAccount("System");
                        MessageEntity msg = new MessageEntity
                        {
                            Title = "旁站监督",
                            Content = workname + "已开始旁站,请前去监管。",
                            UserId = accounts,
                            UserName = names,
                            Status = "",
                            Url = string.Empty,
                            SendUser = senduser.Account,
                            SendUserName = senduser.RealName,
                            Category = "高风险作业"
                        };
                        if (new MessageBLL().SaveForm("", msg))
                        {
                            JPushApi.PublicMessage(msg);
                        }
                    }
                }
                return new { code = 0, info = "发送成功", count = 0 };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 19.班组任务完成
        /// <summary>
        /// 19.班组任务完成
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SendAccomplish([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string id = dy.data.id;//班组任务id
                TeamsInfoEntity u = new TeamsInfoBLL().GetEntity(id);
                if (u != null)
                {
                    u.IsAccomplish = "1";
                    new TeamsInfoBLL().SaveForm(id, u);
                    string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                    System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：" + curUser.UserName + "分配任务完成成功，用户信息" + Newtonsoft.Json.JsonConvert.SerializeObject(curUser) + "\r\n");
                }
                return new { code = 0, info = "操作完成", count = 0 };
            }
            catch (Exception ex)
            {
                //写入日志文件
                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：分配任务完成失败，用户信息" + Newtonsoft.Json.JsonConvert.SerializeObject(curUser) + ",异常信息：" + ex.Message + "\r\n");
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 20.旁站监督监管统计
        /// <summary>
        /// 旁站监督监管统计
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetTaskShareStatistics([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            //获取用户Id
            string userId = dy.userid;
            OperatorProvider.AppUserId = userId;  //设置当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            string deptCode = res.Contains("deptcode") ? dy.data.deptcode : "";  //部门
            string startDate = res.Contains("startdate") ? dy.data.startdate : "";  //起始日期
            string endDate = res.Contains("enddate") ? dy.data.enddate : "";  //截止日期
            string stype = res.Contains("stype") ? dy.data.stype : ""; //导出类型

            StatisticsEntity hentity = new StatisticsEntity();
            hentity.sDeptCode = string.IsNullOrEmpty(deptCode) ? curUser.DeptCode : deptCode;
            hentity.startDate = startDate;
            hentity.endDate = endDate;
            if (stype == "监管图")
            {
                hentity.sAction = "3";   //对比图分析
                hentity.sMark = 0;
            }
            else if (stype == "监督图")
            {
                hentity.sAction = "3";   //对比图分析
                hentity.sMark = 1;
            }

            //当前用户是厂级
            if (curUser.RoleName.Contains("厂级") || curUser.RoleName.Contains("公司级"))
            {
                hentity.isCompany = true;
            }
            else
            {
                hentity.isCompany = false;
            }
            var treeList = new List<TreeGridEntity>();
            //列表
            var dt = tasksharebll.QueryStatisticsByAction(hentity);
            return new
            {
                code = 0,
                count = dt.Rows.Count,
                info = "获取数据成功",
                data = dt
            };
        }
        #endregion
        #endregion

        #region 作业许可
        /// <summary>
        /// 11.获取作业许可状态
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetPermitStateList()
        {
            try
            {
                var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'WorkStatus'").Where(x => x.ItemValue != "1");
                return new { code = 0, info = "获取数据成功", count = itemlist.Count(), data = itemlist.Select(x => new { statevalue = x.ItemValue, statename = x.ItemName }) };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }


        /// <summary>
        /// 12.获取高风险作业类型
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetPermitWorkTypeList()
        {
            try
            {
                var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'WorkType'");
                return new { code = 0, info = "获取数据成功", count = itemlist.Count(), data = itemlist.Select(x => new { worktypevalue = x.ItemValue, worktypename = x.ItemName }) };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 危险作业统计

        #region 15.按作业类型统计
        /// <summary>
        /// 15.按作业类型统计
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetWorkTypeCount([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            //获取用户Id
            string userId = dy.userid;
            OperatorProvider.AppUserId = userId;  //设置当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            string starttime = dy.data.starttime;//开始时间
            string endtime = dy.data.endtime;//结束时间
            string deptid = dy.data.deptid;//作业单位id
            string deptcode = dy.data.deptcode;//作业单位code
            var resulttable = highriskapplybll.GetWorkTypeInfo(starttime, endtime, deptid, deptcode);
            return new
            {
                Code = 0,
                Count = "-1",
                Info = "获取数据成功",
                data = resulttable
            };
        }
        #endregion

        #region 16.月度趋势
        /// <summary>
        /// 16.月度趋势
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetWorkYearCount([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            //获取用户Id
            string userId = dy.userid;
            OperatorProvider.AppUserId = userId;  //设置当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            string year = dy.data.year;//年度
            string deptid = dy.data.deptid;//作业单位id
            string deptcode = dy.data.deptcode;//作业单位code
            var resulttable = highriskapplybll.GetWorkYearCount(year, deptid, deptcode);
            return new
            {
                Code = 0,
                Count = "-1",
                Info = "获取数据成功",
                data = resulttable
            };
        }
        #endregion

        #endregion

        #region 图片上传
        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="Filedata"></param>
        public void UploadifyFile(string folderId, string foldername, HttpFileCollection fileList)
        {

            try
            {
                if (fileList.Count > 0)
                {
                    for (int i = 0; i < fileList.AllKeys.Length; i++)
                    {
                        HttpPostedFile file = fileList[i];
                        if (fileList.AllKeys[i].Contains(foldername))
                        {
                            //获取文件完整文件名(包含绝对路径)
                            //文件存放路径格式：/Resource/ResourceFile/{userId}{data}/{guid}.{后缀名}
                            string userId = OperatorProvider.Provider.Current().UserId;
                            string fileGuid = Guid.NewGuid().ToString();
                            long filesize = file.ContentLength;
                            string FileEextension = Path.GetExtension(file.FileName);
                            string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                            string virtualPath = string.Format("~/Resource/ht/images/{0}/{1}{2}", uploadDate, fileGuid, FileEextension);
                            string virtualPath1 = string.Format("/Resource/ht/images/{0}/{1}{2}", uploadDate, fileGuid, FileEextension);
                            string fullFileName = dataitemdetailbll.GetItemValue("imgPath") + virtualPath1;
                            //创建文件夹
                            string path = Path.GetDirectoryName(fullFileName);
                            Directory.CreateDirectory(path);
                            FileInfoEntity fileInfoEntity = new FileInfoEntity();
                            if (!System.IO.File.Exists(fullFileName))
                            {
                                //保存文件
                                file.SaveAs(fullFileName);
                                //文件信息写入数据库

                                fileInfoEntity.Create();
                                fileInfoEntity.FileId = fileGuid;
                                fileInfoEntity.RecId = folderId; //关联ID
                                fileInfoEntity.FolderId = "ht/images";
                                fileInfoEntity.FileName = file.FileName;
                                fileInfoEntity.FilePath = virtualPath;
                                fileInfoEntity.FileSize = filesize.ToString();
                                fileInfoEntity.FileExtensions = FileEextension;
                                fileInfoEntity.FileType = FileEextension.Replace(".", "");
                                fileInfoBLL.SaveForm("", fileInfoEntity);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region 多条记录的图片一次性上传
        /// <summary>
        /// 多条记录的图片一次性上传
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="Filedata"></param>
        public void AllUploadifyFile(string foldername, HttpFileCollection fileList)
        {

            try
            {
                if (fileList.Count > 0)
                {
                    for (int i = 0; i < fileList.AllKeys.Length; i++)
                    {
                        HttpPostedFile file = fileList[i];
                        //获取文件完整文件名(包含绝对路径)
                        //文件存放路径格式：/Resource/ResourceFile/{userId}{data}/{guid}.{后缀名}
                        string userId = OperatorProvider.Provider.Current().UserId;
                        string fileGuid = Guid.NewGuid().ToString();
                        long filesize = file.ContentLength;
                        string FileEextension = Path.GetExtension(file.FileName);
                        string filename = Path.GetFileNameWithoutExtension(file.FileName);
                        string sid = filename.Split('_')[0];
                        string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                        string virtualPath = string.Format("~/Resource/ht/images/{0}/{1}{2}", uploadDate, fileGuid, FileEextension);
                        string virtualPath1 = string.Format("/Resource/ht/images/{0}/{1}{2}", uploadDate, fileGuid, FileEextension);
                        string fullFileName = dataitemdetailbll.GetItemValue("imgPath") + virtualPath1;
                        //创建文件夹
                        string path = Path.GetDirectoryName(fullFileName);
                        Directory.CreateDirectory(path);
                        FileInfoEntity fileInfoEntity = new FileInfoEntity();
                        if (!System.IO.File.Exists(fullFileName))
                        {
                            //保存文件
                            file.SaveAs(fullFileName);
                            //文件信息写入数据库
                            fileInfoEntity.Create();
                            fileInfoEntity.FileId = fileGuid;
                            fileInfoEntity.RecId = sid; //关联ID
                            fileInfoEntity.FolderId = "ht/images";
                            fileInfoEntity.FileName = "Photo_" + i + FileEextension;
                            fileInfoEntity.FilePath = virtualPath;
                            fileInfoEntity.FileSize = filesize.ToString();
                            fileInfoEntity.FileExtensions = FileEextension;
                            fileInfoEntity.FileType = FileEextension.Replace(".", "");
                            fileInfoBLL.SaveForm("", fileInfoEntity);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region 图片上传
        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="Filedata"></param>
        public void UploadSingleFile(string folderId, string foldername, HttpPostedFile file)
        {

            try
            {
                //获取文件完整文件名(包含绝对路径)
                //文件存放路径格式：/Resource/ResourceFile/{userId}{data}/{guid}.{后缀名}
                string userId = OperatorProvider.Provider.Current().UserId;
                string fileGuid = Guid.NewGuid().ToString();
                long filesize = file.ContentLength;
                string FileEextension = Path.GetExtension(file.FileName);
                string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                string virtualPath = string.Format("~/Resource/ht/images/{0}/{1}{2}", uploadDate, fileGuid, FileEextension);
                string virtualPath1 = string.Format("/Resource/ht/images/{0}/{1}{2}", uploadDate, fileGuid, FileEextension);
                string fullFileName = dataitemdetailbll.GetItemValue("imgPath") + virtualPath1;
                //创建文件夹
                string path = Path.GetDirectoryName(fullFileName);
                Directory.CreateDirectory(path);
                FileInfoEntity fileInfoEntity = new FileInfoEntity();
                if (!System.IO.File.Exists(fullFileName))
                {
                    //保存文件
                    file.SaveAs(fullFileName);
                    //文件信息写入数据库
                    fileInfoEntity.Create();
                    fileInfoEntity.FileId = fileGuid;
                    fileInfoEntity.RecId = folderId; //关联ID
                    fileInfoEntity.FolderId = "ht/images";
                    fileInfoEntity.FileName = file.FileName;
                    fileInfoEntity.FilePath = virtualPath;
                    fileInfoEntity.FileSize = filesize.ToString();
                    fileInfoEntity.FileExtensions = FileEextension;
                    fileInfoEntity.FileType = FileEextension.Replace(".", "");
                    fileInfoBLL.SaveForm("", fileInfoEntity);
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region  高风险通用作业
        #region 获取通用作业许可状态
        /// <summary>
        /// 1.获取通用作业许可状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetCommonStateList()
        {
            try
            {
                var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'CommonStatus'");
                return new { code = 0, info = "获取数据成功", count = itemlist.Count(), data = itemlist.Select(x => new { statevalue = x.ItemValue, statename = x.ItemName }) };
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }
        #endregion

        #region 获取通用作业类型
        /// <summary>
        /// 2.获取通用作业类型
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetCommonWorkTypeList()
        {
            try
            {
                var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'CommonType'");
                return new { code = 0, info = "获取数据成功", count = itemlist.Count(), data = itemlist.Select(x => new { worktypevalue = x.ItemValue, worktypename = x.ItemName }) };
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }
        #endregion

        #region 获取通用作业安全措施显示形式
        /// <summary>
        /// 3.获取通用作业安全措施显示形式
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetCommonShowType([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                var str = GetShowType(curUser.OrganizeCode);

                return new { code = 0, info = "获取数据成功", count = 0, data = str };
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }
        #endregion

        #region 获取作业类型获取安全措施
        /// <summary>
        /// 4.获取作业类型获取安全措施
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetCommonMeasure([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }

                var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'CommonType'");
                var str = GetShowType(curUser.OrganizeCode);

                List<object> datas = new List<object>();
                List<HighProjectSetEntity> setlist = new List<HighProjectSetEntity>();
                foreach (var item in itemlist)
                {
                    DataTable dt = new DataTable();
                    if (str != "1")
                    {
                        setlist = (List<HighProjectSetEntity>)highprojectsetbll.GetList(string.Format(" and createuserorgcode='{0}' and typenum='{1}' order by OrderNumber asc", curUser.OrganizeCode, item.ItemValue)).ToList();
                    }
                    var tempdata = new
                    {
                        worktypevalue = item.ItemValue,
                        worktypename = item.ItemName,
                        measure = setlist.Select(x => new { measureid = x.Id, measurename = x.MeasureName, measureresultone = x.MeasureResultOne, measureresulttwo = x.MeasureResultTwo })
                    };
                    datas.Add(tempdata);
                }
                return new { code = 0, info = "获取数据成功", count = itemlist.Count(), data = datas };
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }
        #endregion

        #region 获取通用作业列表
        /// <summary>
        /// 5.获取通用作业列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetWorkCommonList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }


                string type = dy.data.type;//0.全部 1. 我的申请 2.待确认 3.已确认 4. 待审核(批) 5.已审核(批)
                string status = dy.data.statevalue;//作业许可状态
                string worktype = dy.data.worktypevalue;//作业类型
                string starttime = dy.data.starttime;//作业开始时间
                string endtime = dy.data.endtime;//作业结束时间
                string deptid = dy.data.deptid;//作业单位id
                string deptcode = dy.data.deptcode;//作业单位code
                string applynumber = dy.data.applynumber;//申请编号

                //获取页数和条数
                int page = Convert.ToInt32(dy.data.pagenum), rows = Convert.ToInt32(dy.data.pagesize);
                Pagination pagination = new Pagination();
                pagination.p_kid = "a.Id as applyid";
                pagination.p_fields = "c.itemname as applystatename,a.workdeptid,a.engineeringid,a.createuserdeptcode,a.nextstepapproveuseraccount,a.applystate,a.workdepttype,case when a.workdepttype=0 then '单位内部'  when  a.workdepttype=1 then '外包单位' end workdepttypename,a.workdeptname,b.itemname as worktype,workplace,workcontent,to_char(workstarttime,'yyyy-mm-dd hh24:mi') as workstarttime,to_char(workendtime,'yyyy-mm-dd hh24:mi') as workendtime,applyusername,'' as approveusername,a.flowdept,a.flowrolename,a.specialtytype,a.flowremark,'' as approveuseraccount,a.flowid,a.approvedeptid,e.outtransferuseraccount,e.intransferuseraccount";
                pagination.p_tablename = @" bis_highriskcommonapply a left join base_dataitemdetail b on a.worktype=b.itemvalue and itemid =(select itemid from base_dataitem where itemcode='CommonType') left join base_dataitemdetail c on a.applystate=c.itemvalue and c.itemid =(select itemid from base_dataitem where itemcode='CommonStatus')
                                             left join (select recid,flowid,outtransferuseraccount,intransferuseraccount,row_number()  over(partition by recid,flowid order by createdate desc) as num from BIS_TRANSFERRECORD where disable=0) e on a.id=e.recid and a.flowid=e.flowid and e.num=1";
                pagination.conditionJson = "1=1";
                if (!curUser.IsSystem)
                {
                    string isJDZ = new DataItemDetailBLL().GetItemValue("景德镇版本");
                    string isHDGZ = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("贵州毕节版本");
                    string isAllDataRange = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetEnableItemValue("HighRiskWorkDataRange");
                    if (curUser.RoleName.Contains("公司") || curUser.RoleName.Contains("厂级") || curUser.RoleName.Contains("值班管理员") || !string.IsNullOrEmpty(isHDGZ) || !string.IsNullOrEmpty(isAllDataRange) || !string.IsNullOrEmpty(isJDZ))
                    {

                        pagination.conditionJson += " and a.createuserorgcode='" + curUser.OrganizeCode + "'";
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(" and ((workdeptcode in(select encode from base_department where encode like '{0}%'))  or (engineeringid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", curUser.DeptCode, curUser.DeptId);
                    }
                }
                if (type == "0")
                {
                    pagination.conditionJson += string.Format("  and a.id  not in(select id from bis_highriskcommonapply where applystate='0' and  applyuserid!='{0}')", curUser.UserId);
                }
                else if (type == "1")//我的许可任务
                {
                    pagination.conditionJson += string.Format(" and ApplyUserId='{0}'", curUser.UserId);
                }
                else if (type == "2" || type == "4" || type == "6")//本人待确认
                {
                    string strCondition = "";
                    if (type == "2")
                    {
                        strCondition = " and  a.applystate ='1'";
                    }
                    else if (type == "4")
                    {
                        strCondition = " and  a.applystate ='3'";
                    }
                    else
                    {
                        strCondition = " and  (a.applystate ='3' or a.applystate='1')";
                    }
                    DataTable data = highriskcommonapplybll.GetTable("select " + pagination.p_kid + "," + pagination.p_fields + " from " + pagination.p_tablename + " where " + pagination.conditionJson + strCondition);
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        string executedept = string.Empty;
                        highriskcommonapplybll.GetExecutedept(data.Rows[i]["workdepttype"].ToString(), data.Rows[i]["workdeptid"].ToString(), data.Rows[i]["engineeringid"].ToString(), out executedept);
                        string createdetpid = departmentBLL.GetEntityByCode(data.Rows[i]["createuserdeptcode"].ToString()).DepartmentId;
                        string outsouringengineerdept = string.Empty;
                        highriskcommonapplybll.GetOutsouringengineerDept(data.Rows[i]["workdeptid"].ToString(), out outsouringengineerdept);
                        string str = manypowercheckbll.GetApproveUserAccount(data.Rows[i]["flowid"].ToString(), data.Rows[i]["applyid"].ToString(), data.Rows[i]["nextstepapproveuseraccount"].ToString(), data.Rows[i]["specialtytype"].ToString(), executedept, outsouringengineerdept, createdetpid, "", data.Rows[i]["approvedeptid"].ToString());
                        data.Rows[i]["approveuseraccount"] = str;
                    }
                    string[] applyids = data.Select("(outtransferuseraccount is null or outtransferuseraccount not like '%" + curUser.Account + ",%') and (approveuseraccount like '%" + curUser.Account + ",%' or intransferuseraccount like '%" + curUser.Account + ",%')").AsEnumerable().Select(d => d.Field<string>("applyid")).ToArray();

                    //strCondition += string.Format(" and a.id in ('{0}')", string.Join("','", applyids));
                    pagination.conditionJson += string.Format(" and a.id in ('{0}') {1}", string.Join("','", applyids), strCondition);
                    //pagination.conditionJson += string.Format(" and a.approveaccount like '%{0}%' {1}", curUser.Account + ",", strCondition);

                }
                else if (type == "3")//全部已确认
                {
                    //不包含申请中,确认中
                    pagination.conditionJson += string.Format(" and ApplyState not in('0','1')", status);
                }
                else if (type == "5")//全部已审核
                {
                    pagination.conditionJson += "and a.id in(select s.scaffoldid from bis_scaffoldauditrecord s where s.audituserid='{0}')";
                    //包含4.审核（批）未通过,5.审核（批）通过）
                    //pagination.conditionJson += string.Format(" and ApplyState  in('4','5')", status);
                }
                else if (type == "7") //全部已确认 全部已审核
                {
                    pagination.conditionJson += " and ( ApplyState not in('0','1') or ApplyState  in('4','5'))";
                }
                //查询条件
                if (!string.IsNullOrEmpty(status))//作业许可状态
                {
                    pagination.conditionJson += string.Format(" and ApplyState='{0}'", status);
                }
                if (!string.IsNullOrEmpty(worktype))//作业类型
                {
                    pagination.conditionJson += string.Format(" and WorkType='{0}'", worktype);
                }
                //时间选择
                if (!string.IsNullOrEmpty(starttime))//作业开始时间
                {
                    pagination.conditionJson += string.Format(" and WorkStartTime>=to_date('{0}','yyyy-mm-dd')", starttime);

                }
                if (!string.IsNullOrEmpty(endtime))//作业结束时间
                {
                    endtime = Convert.ToDateTime(endtime).AddDays(1).ToString("yyyy-MM-dd");
                    pagination.conditionJson += string.Format(" and WorkEndTime<=to_date('{0}','yyyy-mm-dd')", endtime);
                }
                if (!string.IsNullOrEmpty(deptid) && !string.IsNullOrEmpty(deptcode) && deptcode != curUser.OrganizeCode)//作业单位
                {
                    pagination.conditionJson += string.Format(" and ((workdeptcode in(select encode from base_department where encode like '{0}%'))  or (engineeringid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", deptcode, deptid);
                }
                //申请编号
                if (!string.IsNullOrEmpty(applynumber))
                {
                    pagination.conditionJson += string.Format(" and ApplyNumber like '%{0}%'", applynumber);
                }
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "a.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                DataTable dt = highriskcommonapplybll.GetPageDataTable(pagination, null);
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    var checkremark = dt.Rows[i]["flowremark"].ToString();
                //    string checktype = checkremark != "1" ? "0" : "1";
                //    string str = scaffoldbll.GetUserName(dt.Rows[i]["flowdept"].ToString(), dt.Rows[i]["flowrolename"].ToString(), checktype, dt.Rows[i]["specialtytype"].ToString());
                //    dt.Rows[i]["approveusername"] = str.Split('|')[0];
                //}
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string executedept = string.Empty;
                    highriskcommonapplybll.GetExecutedept(dt.Rows[i]["workdepttype"].ToString(), dt.Rows[i]["workdeptid"].ToString(), dt.Rows[i]["engineeringid"].ToString(), out executedept);
                    string createdetpid = departmentBLL.GetEntityByCode(dt.Rows[i]["createuserdeptcode"].ToString()).DepartmentId;
                    string outsouringengineerdept = string.Empty;
                    highriskcommonapplybll.GetOutsouringengineerDept(dt.Rows[i]["workdeptid"].ToString(), out outsouringengineerdept);
                    string str = manypowercheckbll.GetApproveUserAccount(dt.Rows[i]["flowid"].ToString(), dt.Rows[i]["applyid"].ToString(), dt.Rows[i]["nextstepapproveuseraccount"].ToString(), dt.Rows[i]["specialtytype"].ToString(), executedept, outsouringengineerdept, createdetpid, "", dt.Rows[i]["approvedeptid"].ToString());
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
                return new { code = 0, info = "获取数据成功", count = pagination.records, data = dt };
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }
        #endregion

        #region 通用作业申请[保存,提交]
        /// <summary>
        /// 6.通用作业申请[保存,提交]
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object ApplyWorkCommon()
        {
            try
            {
                string res = ctx.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }

                string applyid = dy.data.applyid;//通用申请id
                string type = dy.data.type;//类型 0：保存 1：提交
                string applyentity = JsonConvert.SerializeObject(dy.data.applyentity);
                HighRiskCommonApplyEntity centity = JsonConvert.DeserializeObject<HighRiskCommonApplyEntity>(applyentity);
                string workType = centity.WorkType;
                List<HighRiskApplyMBXXEntity> mbList = new List<HighRiskApplyMBXXEntity>();
                string isJdz = new DataItemDetailBLL().GetItemValue("景德镇版本");
                if (!string.IsNullOrEmpty(isJdz) && workType.Equals("11"))
                {
                    //盲板抽堵作业
                    string mbxx = JsonConvert.SerializeObject(dy.data.mbxx);
                    mbList = JsonConvert.DeserializeObject<List<HighRiskApplyMBXXEntity>>(mbxx);
                }
                if (string.IsNullOrEmpty(applyid))
                {
                    applyid = Guid.NewGuid().ToString();
                }
                centity.ApplyDeptCode = curUser.DeptCode;
                centity.ApplyDeptId = curUser.DeptId;
                centity.ApplyDeptName = curUser.DeptName;
                centity.ApplyUserId = curUser.UserId;
                centity.ApplyUserName = curUser.UserName;
                HighRiskCommonApplyEntity entity = highriskcommonapplybll.GetEntity(applyid);
                if (entity != null)
                {
                    centity.CreateUserDeptCode = entity.CreateUserDeptCode;
                }
                //如果有删除的文件，则进行删除
                if (!string.IsNullOrEmpty(centity.DeleteFileIds))
                {
                    DeleteFile(centity.DeleteFileIds);
                }
                //再重新上传
                HttpFileCollection files = ctx.Request.Files;
                UploadifyFile(applyid, "commonfile", files);
                if (!string.IsNullOrEmpty(isJdz) && workType.Equals("11"))
                {
                    UploadifyFile(applyid + "_01", "mbxxfile", files);
                }
                centity.DeleteFileIds = null;

                string riskrecordlist = JsonConvert.SerializeObject(dy.data.riskrecord);
                var list = JsonConvert.DeserializeObject<List<HighRiskRecordEntity>>(riskrecordlist);
                highriskrecordbll.RemoveFormByWorkId(applyid);
                if (list != null)
                {
                    var num = 0;
                    foreach (var item in list)
                    {
                        item.CreateDate = DateTime.Now.AddSeconds(-num);
                        item.WorkId = applyid;
                        highriskrecordbll.SaveForm("", item);
                        num++;
                    }
                }
                highriskcommonapplybll.SaveForm(applyid, type, centity, list, mbList);
                return new { code = 0, count = 0, info = "保存成功" };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }
        #endregion

        #region 获取高风险通用作业详情
        /// <summary>
        /// 7.获取高风险通用作业详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object WorkCommonDetail([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string webUrl = dataitemdetailbll.GetItemValue("imgUrl");
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }

                string applyid = dy.data.applyid;//通用申请id

                HighRiskCommonApplyEntity commonentity = highriskcommonapplybll.GetEntity(applyid);
                var str = GetShowType(curUser.OrganizeCode);
                List<MeasultData> data = null;
                if (commonentity.ApplyState == "0")//申请中
                {
                    if (str != "1")//默认为手动输入(1:手动输入)
                    {
                        var setlist = (List<HighProjectSetEntity>)highprojectsetbll.GetList(string.Format(" and createuserorgcode='{0}' and typenum='{1}' order by OrderNumber asc", curUser.OrganizeCode, commonentity.WorkType)).ToList();
                        var list = setlist.Select(x => new { measureid = x.Id, measurename = x.MeasureName, measureresultone = x.MeasureResultOne, measureresulttwo = x.MeasureResultTwo, measureresult = "", checkpersons = "", checkpersonsid = "", signpic = "" });

                        data = JsonConvert.DeserializeAnonymousType(JsonConvert.SerializeObject(list), new List<MeasultData>());
                    }
                }
                else
                {
                    var projectlist = scaffoldprojectbll.GetListByCondition(string.Format(" and ScaffoldId='{0}'", applyid)).OrderBy(t => t.CreateDate).ToList();
                    var list1 = projectlist.Select(x => new { measureid = x.Id, measurename = x.ProjectName, measureresultone = x.ResultYes, measureresulttwo = x.ResultNo, measureresult = x.Result, checkpersons = x.CheckPersons, checkpersonsid = x.CheckPersonsId, signpic = string.IsNullOrWhiteSpace(x.SignPic) ? "" : webUrl + x.SignPic.ToString().Replace("../../", "/") });
                    data = JsonConvert.DeserializeAnonymousType(JsonConvert.SerializeObject(list1), new List<MeasultData>());
                }
                var auditlist = scaffoldauditrecordbll.GetList(applyid).Where(t => t.AuditRemark != "确认step");

                DataTable cdt = fileInfoBLL.GetFiles(applyid);
                IList<ERCHTMS.AppSerivce.Model.Photo> cfiles = new List<ERCHTMS.AppSerivce.Model.Photo>();
                foreach (DataRow item in cdt.Rows)
                {
                    ERCHTMS.AppSerivce.Model.Photo p = new ERCHTMS.AppSerivce.Model.Photo();
                    p.id = item[0].ToString();
                    p.filename = item[1].ToString();
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + item[2].ToString().Substring(1);
                    cfiles.Add(p);
                }


                //盲板图位置图片
                cdt = fileInfoBLL.GetFiles(applyid+"_01");
                IList<ERCHTMS.AppSerivce.Model.Photo> mbtfiles = new List<ERCHTMS.AppSerivce.Model.Photo>();
                foreach (DataRow item in cdt.Rows)
                {
                    ERCHTMS.AppSerivce.Model.Photo p = new ERCHTMS.AppSerivce.Model.Photo();
                    p.id = item[0].ToString();
                    p.filename = item[1].ToString();
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + item[2].ToString().Substring(1);
                    mbtfiles.Add(p);
                }

                var riskrecord = highriskrecordbll.GetList(applyid).ToList();

                #region 流程
                //string projectid = "";
                string moduleName = highriskcommonapplybll.GetModuleName(commonentity);
                string isJdz = new DataItemDetailBLL().GetItemValue("景德镇版本");
                //string worktypename = new DataItemDetailBLL().GetItemName("CommonType", commonentity.WorkType);
                List<HighRiskApplyMBXXEntity> mbList = new List<HighRiskApplyMBXXEntity>();
                string riskType = "CommonRiskType";//风险等级
                if (!string.IsNullOrWhiteSpace(isJdz) && commonentity.WorkType == "11")
                {
                    mbList = applymbxxbll.GetList(commonentity.Id);
                }
                else if (!string.IsNullOrWhiteSpace(isJdz) && commonentity.WorkType == "12")
                {
                    riskType = "CommonWorkType";
                }
               
                //bool isEndFlow = false;
                //if (commonentity.InvestigateState == "3")
                //{
                //    isEndFlow = true;
                //}
                var nodelist = highriskcommonapplybll.GetAppFlowList(commonentity.Id, moduleName);
                #endregion

                #region 获取执行情况

                IList<FireWaterCondition> conditionlist = firewaterbll.GetConditionList(applyid).OrderBy(t => t.CreateDate).ToList();
                for (int i = 0; i < conditionlist.Count; i++)
                {
                    var item = conditionlist[i];
                    List<FileInfoEntity> piclist = fileInfoBLL.GetFileList(item.Id);
                    IList<ERCHTMS.Entity.HighRiskWork.ViewModel.Photo> temppiclist = new List<ERCHTMS.Entity.HighRiskWork.ViewModel.Photo>();
                    foreach (var temp in piclist)
                    {
                        ERCHTMS.Entity.HighRiskWork.ViewModel.Photo pic = new ERCHTMS.Entity.HighRiskWork.ViewModel.Photo();
                        pic.filename = temp.FileName;
                        pic.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + temp.FilePath.Substring(1);
                        pic.fileid = temp.FileId;
                        temppiclist.Add(pic);
                    }
                    item.piclist = temppiclist;
                    List<FileInfoEntity> filelist = fileInfoBLL.GetFileList(item.Id + "_02");
                    IList<ERCHTMS.Entity.HighRiskWork.ViewModel.Photo> tempfilelist = new List<ERCHTMS.Entity.HighRiskWork.ViewModel.Photo>();
                    foreach (var temp in filelist)
                    {
                        ERCHTMS.Entity.HighRiskWork.ViewModel.Photo pic = new ERCHTMS.Entity.HighRiskWork.ViewModel.Photo();
                        pic.filename = temp.FileName;
                        pic.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + temp.FilePath.Substring(1);
                        pic.fileid = temp.FileId;
                        tempfilelist.Add(pic);
                    }
                    item.filelist = tempfilelist;
                    item.num = i / 2 + 1;
                }

                #endregion

                
                
                var resdata = new
                {
                    applyid = commonentity.Id,
                    applynumber = commonentity.ApplyNumber,
                    applydeptname = commonentity.ApplyDeptName,
                    applyusername = commonentity.ApplyUserName,
                    applytime = Convert.ToDateTime(commonentity.CreateDate).ToString("yyyy-MM-dd HH:mm"),
                    worktype = commonentity.WorkType,
                    worktypename = scaffoldbll.getName(commonentity.WorkType, "CommonType"),
                    risktype = commonentity.RiskType,
                    risktypename = !string.IsNullOrEmpty(commonentity.RiskType) ? scaffoldbll.getName(commonentity.RiskType, riskType) : "",
                    workdepttype = commonentity.WorkDeptType,
                    workdeptid = commonentity.WorkDeptId,
                    workdepttypename = commonentity.WorkDeptType == "0" ? "单位内部" : "外包单位",
                    workdeptname = commonentity.WorkDeptName,
                    engineeringname = commonentity.EngineeringName,
                    engineeringid = commonentity.EngineeringId,
                    workstarttime = Convert.ToDateTime(commonentity.WorkStartTime).ToString("yyyy-MM-dd HH:mm"),
                    workendtime = Convert.ToDateTime(commonentity.WorkEndTime).ToString("yyyy-MM-dd HH:mm"),
                    workareaname = commonentity.WorkAreaName,
                    workplace = commonentity.WorkPlace,
                    workcontent = commonentity.WorkContent,
                    workdutyuserid = commonentity.WorkDutyUserId,
                    workdutyusername = commonentity.WorkDutyUserName,
                    worktutelageuserid = commonentity.WorkTutelageUserId,
                    worktutelageusername = commonentity.WorkTutelageUserName,
                    workuserids = commonentity.WorkUserIds,
                    workusernames = commonentity.WorkUserNames,
                    riskidentification = commonentity.RiskIdentification,
                    applystate = commonentity.ApplyState,
                    applystatename = scaffoldbll.getName(commonentity.ApplyState, "CommonStatus"),
                    investigatestate = commonentity.InvestigateState,
                    worklicensorname = commonentity.WorkLicensorName,
                    worklicensorid = commonentity.WorkLicensorId,
                    worklicensoraccount = commonentity.WorkLicensorAccount,
                    nextstepapproveuseraccount = commonentity.NextStepApproveUserAccount,
                    workdutyuseraccount = commonentity.WorkDutyUserAccount,
                    approveaccount = commonentity.ApproveAccount,
                    flowapplytype = commonentity.FlowApplyType,
                    poweraccess = commonentity.PowerAccess,
                    voltage = commonentity.Voltage,
                    flowid = commonentity.FlowId,
                    ishandimport = str,
                    specialtytype = !string.IsNullOrEmpty(commonentity.SpecialtyType) ? commonentity.SpecialtyType : "",
                    specialtytypename = !string.IsNullOrEmpty(commonentity.SpecialtyType) ? scaffoldbll.getName(commonentity.SpecialtyType, "SpecialtyType") : "",
                    copyuserids = !string.IsNullOrEmpty(commonentity.CopyUserIds) ? commonentity.CopyUserIds : "",
                    copyusernames = !string.IsNullOrEmpty(commonentity.CopyUserNames) ? commonentity.CopyUserNames : "",
                    ismessage = commonentity.IsMessage,
                    projectlist = data,
                    auditlist = auditlist.Select(y => new
                    {
                        auditdeptname = y.AuditDeptName,
                        auditusername = y.AuditUserName,
                        auditstate = (y.AuditState == 1 ? 0 : 1),
                        auditremark = y.AuditRemark,
                        auditdate = Convert.ToDateTime(y.AuditDate).ToString("yyyy-MM-dd"),
                        auditsignimg = string.IsNullOrWhiteSpace(y.AuditSignImg) ? "" : webUrl + y.AuditSignImg.ToString().Replace("../../", "/")
                    }),
                    pipeline = commonentity.PipeLine,
                    media = commonentity.Media,
                    pressure = commonentity.Pressure,
                    zmbdutyuserid = commonentity.ZMBDutyUserId,
                    zmbdutyusername = commonentity.ZMBDutyUserName,
                    cmbdutyuserid = commonentity.CMBDutyUserId,
                    cmbdutyusername = commonentity.CMBDutyUserName,
                    temperature = commonentity.Temperature,
                    workticketnocontent = commonentity.WorkTicketNoContent,
                    dangeranalyse = commonentity.DangerAnalyse,
                    safetyanalyse = commonentity.SafetyAnalyse,
                    yxpermituserid = commonentity.YXPermitUserId,
                    yxpermitusername = commonentity.YXPermitUserName,
                    watchuserid = commonentity.WatchUserId,
                    watchusername = commonentity.WatchUserName,
                    effectconfimerid = commonentity.EffectConfimerId,
                    effectconfirmername = commonentity.EffectConfirmerName,
                    cfiles = cfiles,
                    mbtfiles= mbtfiles,
                    riskrecord = riskrecord,
                    checkflow = nodelist,
                    nonworkingapprove = commonentity.NonWorkingApprove,
                    approvedept = commonentity.ApproveDept,
                    approvedeptid = commonentity.ApproveDeptId,
                    approvedeptcode = commonentity.ApproveDeptCode,
                    conditionlist = conditionlist,
                    mbxx= mbList
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
                return new
                {
                    //measureresult  0：未 1：已
                    //(web端)AuditState 1:同意 0:不同意 (手机端)AuditState 0:同意 1:不同意
                    code = 0,
                    count = -1,
                    info = "获取数据成功",
                    data = JObject.Parse(JsonConvert.SerializeObject(resdata, Formatting.None, settings))
                };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }
        #endregion

        #region 保存安全措施
        /// <summary>
        /// 8.保存安全措施
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SaveMeasure()
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
                string webUrl = dataitemdetailbll.GetItemValue("imgUrl");
                //安全措施 (审核阶段不用传入)
                List<object> record = res.Contains("record") ? (List<object>)dy.data.record : null;
                foreach (dynamic rdy in record)
                {
                    string measureid = rdy.measureid;  //主键
                    string measureresult = rdy.measureresult; //结果
                    string checkpersons = rdy.checkpersons; //选择的人员
                    string measurename = rdy.measurename; //安全措施项目
                    string checkpersonsid = rdy.checkpersonsid;
                    string signpic = rdy.signpic;

                    if (!string.IsNullOrEmpty(measureid))
                    {
                        var scEntity = scaffoldprojectbll.GetEntity(measureid); //安全措施
                        scEntity.Result = measureresult;
                        scEntity.CheckPersons = checkpersons;
                        if (GetShowType(curUser.OrganizeCode) == "1")
                        {
                            scEntity.ProjectName = measurename;
                        }
                        scEntity.CheckPersonsId = checkpersonsid;
                        scEntity.SignPic = string.IsNullOrWhiteSpace(signpic) ? "" : signpic.Replace(webUrl, "").ToString();
                        HttpFileCollection files = HttpContext.Current.Request.Files;
                        if (files.Count > 0)
                        {
                            for (int i = 0; i < files.AllKeys.Length; i++)
                            {
                                HttpPostedFile file = files[i];
                                string fileOverName = System.IO.Path.GetFileName(file.FileName);
                                string fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                                string FileEextension = Path.GetExtension(file.FileName);
                                if (fileName == scEntity.Id)
                                {
                                    string dir = dataitemdetailbll.GetItemValue("imgPath") + "\\Resource\\sign";
                                    string newFileName = fileName + FileEextension;
                                    string newFilePath = dir + "\\" + newFileName;
                                    file.SaveAs(newFilePath);
                                    scEntity.SignPic = "/Resource/sign/" + fileOverName;
                                    break;
                                }
                            }
                        }
                        scaffoldprojectbll.SaveForm(measureid, scEntity);
                    }
                }
                return new { code = 0, count = 0, info = "保存成功" };
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "保存失败" };
            }

        }
        #endregion

        #region 确认、审核
        /// <summary>
        ///9. 确认、审核
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SubmitCheckWork()
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
                string webUrl = dataitemdetailbll.GetItemValue("imgUrl");
                string applyid = dy.data.applyid;//通用申请id
                List<object> record = res.Contains("record") ? (List<object>)dy.data.record : null;
                string state = res.Contains("state") ? dy.data.state : "";  //判定是否是确认，还是审核阶段
                /*******审核详情*******/

                //给移动端那边是审核结果  0 同意 1 不同意
                //web端是审核结果  1 同意 0不同意
                string id = res.Contains("id") ? dy.data.id : "";  //随机id
                string approveresult = res.Contains("auditstate") ? (dy.data.auditstate == "0" ? "1" : "0") : ""; //审核结果
                string approveopinion = res.Contains("auditremark") ? dy.data.auditremark : "";  //审核意见
                string auditsignimg = res.Contains("auditsignimg") ? dy.data.auditsignimg : "";  //审核签名图片

                HighRiskCommonApplyEntity entity = highriskcommonapplybll.GetEntity(applyid);

                int noDoneCount = 0; //未完成个数

                string newKeyValue = string.Empty;
                ManyPowerCheckEntity mpcEntity = null;
                //string projectid = "";
                string moduleName = highriskcommonapplybll.GetModuleName(entity);
                
                mpcEntity = manypowercheckbll.CheckAuditForNext(curUser, moduleName, entity.FlowId);
                //审核对象
                ScaffoldauditrecordEntity aentity = new ScaffoldauditrecordEntity();
                aentity.FlowId = entity.FlowId;

                //确认状态下更新安全检查项目
                if (state == "1")
                {
                    string str = GetShowType(curUser.OrganizeCode);
                    if (record != null)
                    {
                        foreach (dynamic rdy in record)
                        {
                            string measureid = rdy.measureid;  //主键
                            string measureresult = rdy.measureresult; //结果
                            string checkpersons = rdy.checkpersons; //选择的人员
                            string measurename = rdy.measurename; //安全措施项目
                            string checkpersonsid = rdy.checkpersonsid;
                            string signpic = rdy.signpic;
                            if (!string.IsNullOrEmpty(measureid))
                            {
                                var scEntity = scaffoldprojectbll.GetEntity(measureid); //安全措施
                                scEntity.Result = measureresult;
                                scEntity.CheckPersons = checkpersons;
                                if (measureresult != "1") { noDoneCount += 1; } //存在否定的情况的则累加
                                if (GetShowType(curUser.OrganizeCode) == "1")
                                {
                                    scEntity.ProjectName = measurename;
                                }
                                scEntity.CheckPersonsId = checkpersonsid;
                                scEntity.SignPic = string.IsNullOrWhiteSpace(signpic) ? "" : signpic.Replace(webUrl, "").ToString();
                                HttpFileCollection files = HttpContext.Current.Request.Files;
                                if (files.Count > 0)
                                {
                                    for (int i = 0; i < files.AllKeys.Length; i++)
                                    {
                                        HttpPostedFile file = files[i];
                                        string fileOverName = System.IO.Path.GetFileName(file.FileName);
                                        string fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                                        string FileEextension = Path.GetExtension(file.FileName);
                                        if (fileName == scEntity.Id)
                                        {
                                            string dir = dataitemdetailbll.GetItemValue("imgPath") + "\\Resource\\sign";
                                            string newFileName = fileName + FileEextension;
                                            string newFilePath = dir + "\\" + newFileName;
                                            file.SaveAs(newFilePath);
                                            scEntity.SignPic = "/Resource/sign/" + fileOverName;
                                            break;
                                        }
                                    }
                                }
                                scaffoldprojectbll.SaveForm(measureid, scEntity);
                            }
                        }
                    }
                    //流程结束
                    if (noDoneCount > 0)
                    {
                        approveopinion = "确认step";
                        approveresult = "0";
                        entity.InvestigateState = "3"; //更改状态为流程结束
                        entity.ApplyState = "2";//确认未通过
                        entity.FlowDept = " ";
                        entity.FlowDeptName = " ";
                        entity.FlowRole = " ";
                        entity.FlowRoleName = " ";
                        entity.FlowName = "已完结";
                        entity.FlowRemark = "";
                        entity.FlowApplyType = "";
                        entity.ApproveAccount = "";
                        entity.NextStepApproveUserAccount = "";
                        entity.FlowId = "";

                        UserEntity userEntity = userbll.GetEntity(entity.CreateUserId);
                        if (userEntity != null)
                        {
                            JPushApi.PushMessage(userEntity.Account, userEntity.RealName, "ZY003", "", "您提交的" + scaffoldbll.getName(entity.WorkType, "CommonType") + "申请未通过，请及时处理。", entity.Id);
                        }
                    }
                    else
                    {
                        approveopinion = "确认step";
                        approveresult = "1";
                        if (null != mpcEntity)
                        {
                            entity.FlowDept = mpcEntity.CHECKDEPTID;
                            entity.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                            entity.FlowRole = mpcEntity.CHECKROLEID;
                            entity.FlowRoleName = mpcEntity.CHECKROLENAME;
                            entity.FlowId = mpcEntity.ID;
                            entity.FlowApplyType = mpcEntity.NextStepFlowEntity.IsEmpty() ? "" : mpcEntity.NextStepFlowEntity.ApplyType;
                            entity.FlowName = "审核中";
                            entity.InvestigateState = "2"; //更改状态为审核
                            entity.ApplyState = "3";//审核(批)中
                            entity.FlowRemark = !string.IsNullOrEmpty(mpcEntity.REMARK) ? mpcEntity.REMARK : "";
                            string checktype = entity.FlowRemark != "1" ? "0" : "1";
                            entity.NextStepApproveUserAccount = aentity.NextStepApproveUserAccount;
                            string executedept = string.Empty;
                            highriskcommonapplybll.GetExecutedept(entity.WorkDeptType, entity.WorkDeptId, entity.EngineeringId, out executedept); //获取执行部门
                            string createdetpid = departmentBLL.GetEntityByCode(entity.CreateUserDeptCode).DepartmentId; //获取创建部门ID
                            string outsouringengineerdept = string.Empty;
                            highriskcommonapplybll.GetOutsouringengineerDept(entity.WorkDeptId, out outsouringengineerdept);
                            string accountstr = manypowercheckbll.GetApproveUserAccount(entity.FlowId, entity.Id, entity.NextStepApproveUserAccount, entity.SpecialtyType, executedept, outsouringengineerdept, createdetpid, "", entity.ApproveDeptId); //获取审核人账号
                            entity.ApproveAccount = accountstr;
                            DataTable dtuser = userbll.GetUserTable(entity.ApproveAccount.Split(','));
                            string[] usernames = dtuser.AsEnumerable().Select(d => d.Field<string>("realname")).ToArray();
                            JPushApi.PushMessage(entity.ApproveAccount, string.Join(",", usernames), "ZY003", "", "您有一条" + scaffoldbll.getName(entity.WorkType, "CommonType") + "申请待审批，请及时处理。", entity.Id);
                            //scaffoldbll.SendMessage(entity.FlowDept, entity.FlowRole, "ZY002", entity.Id, "", "您有一条" + scaffoldbll.getName(entity.WorkType, "CommonType") + "申请待审批，请及时处理。", checktype, !string.IsNullOrEmpty(entity.SpecialtyType) ? entity.SpecialtyType : "");

                        }
                        else
                        {
                            entity.FlowRemark = "";
                            entity.ApplyState = "5";
                            entity.FlowDept = "";
                            entity.FlowDeptName = "";
                            entity.FlowRole = "";
                            entity.FlowRoleName = "";
                            entity.FlowName = "已完结";
                            entity.InvestigateState = "3"; //完结状态
                            entity.FlowApplyType = "";
                            entity.ApproveAccount = "";
                            entity.NextStepApproveUserAccount = "";
                            entity.FlowId = "";
                        }

                    }
                }
                else
                {
                    //同意进行下一步
                    if (approveresult == "1")
                    {
                        //下一步流程不为空
                        if (null != mpcEntity)
                        {
                            entity.FlowDept = mpcEntity.CHECKDEPTID;
                            entity.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                            entity.FlowRole = mpcEntity.CHECKROLEID;
                            entity.FlowRoleName = mpcEntity.CHECKROLENAME;
                            entity.FlowId = mpcEntity.ID;
                            entity.FlowApplyType = mpcEntity.NextStepFlowEntity.IsEmpty() ? "" : mpcEntity.NextStepFlowEntity.ApplyType;
                            entity.FlowRemark = !string.IsNullOrEmpty(mpcEntity.REMARK) ? mpcEntity.REMARK : "";
                            string checktype = entity.FlowRemark != "1" ? "0" : "1";
                            entity.NextStepApproveUserAccount = aentity.NextStepApproveUserAccount;
                            string executedept = string.Empty;
                            highriskcommonapplybll.GetExecutedept(entity.WorkDeptType, entity.WorkDeptId, entity.EngineeringId, out executedept); //获取执行部门
                            string createdetpid = departmentBLL.GetEntityByCode(entity.CreateUserDeptCode).DepartmentId; //获取创建部门ID
                            string outsouringengineerdept = string.Empty;
                            highriskcommonapplybll.GetOutsouringengineerDept(entity.WorkDeptId, out outsouringengineerdept);
                            string str = manypowercheckbll.GetApproveUserAccount(entity.FlowId, entity.Id, entity.NextStepApproveUserAccount, entity.SpecialtyType, executedept, outsouringengineerdept, createdetpid, "", entity.ApproveDeptId); //获取审核人账号
                            entity.ApproveAccount = str;

                            DataTable dtuser = userbll.GetUserTable(entity.ApproveAccount.Split(','));
                            string[] usernames = dtuser.AsEnumerable().Select(d => d.Field<string>("realname")).ToArray();
                            JPushApi.PushMessage(entity.ApproveAccount, string.Join(",", usernames), "ZY003", "", "您有一条" + scaffoldbll.getName(entity.WorkType, "CommonType") + "申请待审批，请及时处理。", entity.Id);
                            //scaffoldbll.SendMessage(entity.FlowDept, entity.FlowRole, "ZY002", entity.Id, "", "您有一条" + scaffoldbll.getName(entity.WorkType, "CommonType") + "申请待审批，请及时处理。", checktype, !string.IsNullOrEmpty(entity.SpecialtyType) ? entity.SpecialtyType : "");
                        }
                        else
                        {
                            entity.FlowRemark = "";
                            entity.FlowDept = " ";
                            entity.FlowDeptName = " ";
                            entity.FlowRole = " ";
                            entity.FlowRoleName = " ";
                            entity.FlowName = "已完结";
                            entity.InvestigateState = "3"; //更改状态为完结状态
                            entity.ApplyState = "5";//审核(批)通过
                        }
                    }
                    else
                    {
                        entity.FlowRemark = "";
                        entity.FlowDept = " ";
                        entity.FlowDeptName = " ";
                        entity.FlowRole = " ";
                        entity.FlowRoleName = " ";
                        entity.InvestigateState = "3"; //更改状态为登记状态
                        entity.ApplyState = "4";//审核(批)未通过
                        entity.FlowName = "已完结";

                        UserEntity userEntity = userbll.GetEntity(entity.CreateUserId);
                        if (userEntity != null)
                        {
                            JPushApi.PushMessage(userEntity.Account, userEntity.RealName, "ZY003", "", "您提交的" + scaffoldbll.getName(entity.WorkType, "CommonType") + "申请未通过，请及时处理。", entity.Id);
                        }
                    }
                    aentity.AuditSignImg = string.IsNullOrWhiteSpace(auditsignimg) ? "" : auditsignimg.Replace(webUrl, "").ToString();
                    HttpFileCollection files = HttpContext.Current.Request.Files;
                    if (files.Count > 0)
                    {
                        for (int i = 0; i < files.AllKeys.Length; i++)
                        {
                            HttpPostedFile file = files[i];
                            string fileOverName = System.IO.Path.GetFileName(file.FileName);
                            string fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                            string FileEextension = Path.GetExtension(file.FileName);
                            if (fileName == id)
                            {
                                string dir = dataitemdetailbll.GetItemValue("imgPath") + "\\Resource\\sign";
                                string newFileName = fileName + FileEextension;
                                string newFilePath = dir + "\\" + newFileName;
                                file.SaveAs(newFilePath);
                                aentity.AuditSignImg = "/Resource/sign/" + fileOverName;
                                break;
                            }
                        }
                    }
                }
                //添加审核记录
                aentity.AuditState = Convert.ToInt32(approveresult);
                aentity.AuditRemark = approveopinion;//审核意见
                aentity.AuditUserName = curUser.UserName;
                aentity.AuditUserId = curUser.UserId;
                aentity.AuditDeptName = curUser.DeptName;
                aentity.AuditDeptId = curUser.DeptId;
                aentity.AuditDate = DateTime.Now;
                aentity.ScaffoldId = applyid; //关联id 
                scaffoldauditrecordbll.SaveForm("", aentity);

                highriskcommonapplybll.SaveApplyForm(applyid, entity);

                return new { code = 0, count = 0, info = "保存成功" };
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "保存失败" };
            }

        }
        #endregion

        #region 获取风险等级
        /// <summary>
        /// 获取风险等级
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetRiskTypeList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string isJdz = new DataItemDetailBLL().GetItemValue("景德镇版本");
                string risktype = "";
                if (!string.IsNullOrEmpty(isJdz) && dy.data.worktype.Equals("12"))
                {
                    risktype = "CommonWorkType";
                }
                else
                {
                    risktype = "CommonRiskType";
                }
                var itemlist = dataitemdetailbll.GetDataItemListByItemCode(risktype);
                return new { code = 0, info = "获取数据成功", count = itemlist.Count(), data = itemlist.Select(x => new { risktype = x.ItemValue, risktypename = x.ItemName }) };
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }
        #endregion

        #region 待确认的高风险通用作业撤销
        [HttpPost]
        public object CancelApply()
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
                string keyvalue = dy.data.keyvalue;
                var entity = highriskcommonapplybll.GetEntity(keyvalue);
                if (entity != null)
                {
                    entity.ApplyState = "0";
                    entity.InvestigateState = "0";
                    entity.FlowDept = "";
                    entity.FlowDeptName = "";
                    entity.FlowId = "";
                    entity.FlowName = "";
                    entity.FlowRemark = "";
                    entity.FlowRole = "";
                    entity.FlowRoleName = "";
                    entity.FlowApplyType = "";
                    entity.NextStepApproveUserAccount = "";
                    entity.ApproveAccount = "";
                    highriskcommonapplybll.UpdateForm(entity);
                    scaffoldprojectbll.RemoveForm(t => t.ScaffoldId == keyvalue);
                    var transferlist = transferrecordbll.GetList(t => t.RecId == keyvalue && t.Disable == 0);
                    foreach (var item in transferlist)
                    {
                        item.Disable = 1;
                        transferrecordbll.SaveForm(item.Id, item);
                    }
                    return new { code = 0, count = 0, info = "撤销成功" };
                }
                else
                {
                    return new { code = -1, count = 0, info = "撤销失败" };
                }

            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "撤销失败" };
            }
        }

        #endregion
        #endregion

        #region 高风险作业统计(电厂)
        /// <summary>
        /// 1.按作业类型统计
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetWorkTypeStatistics([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            //获取用户Id
            string userId = dy.userid;
            OperatorProvider.AppUserId = userId;  //设置当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            string starttime = res.Contains("starttime") ? dy.data.starttime : "";//开始时间
            string endtime = res.Contains("endtime") ? dy.data.endtime : "";//结束时间
            string deptid = res.Contains("deptid") ? dy.data.deptid : "";//单位id
            string deptcode = res.Contains("deptcode") ? dy.data.deptcode : "";//单位code
            var resulttable = highriskcommonapplybll.AppGetHighWork(starttime, endtime, deptid, deptcode);
            return new
            {
                code = 0,
                count = resulttable.Rows.Count,
                info = "获取数据成功",
                data = resulttable
            };
        }
        #endregion

        #region 获取安全措施形式
        /// <summary>
        /// 获取安全措施形式
        /// </summary>
        /// <returns></returns>
        public string GetShowType(string userOrganizeCode)
        {
            string str = "1";//默认为手动输入
            var data = highimporttypebll.GetList(string.Format(" and itype='1' and CreateUserOrgCode='{0}'", userOrganizeCode)).FirstOrDefault();
            if (data != null)
                str = data.IsImport;

            return str;
        }
        #endregion

        #region  公用
        #region  根据作业类别获取作业单位
        /// <summary>
        /// 根据作业类别获取作业单位
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetWorkDept([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string userid = dy.userid;

            string type = dy.data.type;//0.单位内部 1.外包单位 
            string showtype = res.Contains("showtype") ? dy.data.showtype.ToString() : "";//搜索

            //获取用户基本信息
            OperatorProvider.AppUserId = userid;  //设置当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            try
            {
                //获取当前部门
                string organizeId = curUser.OrganizeId;
                string parentId = "0";
                IList<DeptData> result = new List<DeptData>();
                IList<DeptData> list = new List<DeptData>();
                //获取当前机构下的所有部门
                if (type == "0")
                {
                    if (!string.IsNullOrEmpty(showtype))
                    {
                        //OrganizeEntity org = orgBLL.GetEntity(organizeId);
                        DeptData dept = new DeptData();
                        dept.deptid = curUser.OrganizeId;
                        dept.code = curUser.OrganizeCode;
                        dept.isorg = 1;
                        dept.oranizeid = curUser.OrganizeId;
                        dept.parentcode = "";
                        dept.parentid = parentId;
                        dept.name = curUser.OrganizeName;
                        dept.isparent = true;
                        list = GetChangeDept(dept);
                        dept.children = list;
                        dept.isoptional = "1";
                        result.Add(dept);
                    }
                    else
                    {
                        if (curUser.RoleName.Contains("厂级") || curUser.RoleName.Contains("公司"))
                        {
                            //OrganizeEntity org = orgBLL.GetEntity(organizeId);
                            DeptData dept = new DeptData();
                            dept.deptid = curUser.OrganizeId;
                            dept.code = curUser.OrganizeCode;
                            dept.isorg = 1;
                            dept.oranizeid = curUser.OrganizeId;
                            dept.parentcode = "";
                            dept.parentid = parentId;
                            dept.name = curUser.OrganizeName;
                            dept.isparent = true;
                            list = GetChangeDept(dept);
                            dept.children = list;
                            dept.isoptional = "1";
                            result.Add(dept);
                        }
                        else
                        {
                            DeptData dept = new DeptData();
                            dept.deptid = curUser.DeptId;
                            dept.code = curUser.DeptCode;
                            dept.isorg = 0;
                            dept.oranizeid = curUser.OrganizeId;
                            dept.parentcode = "";
                            dept.parentid = parentId;
                            dept.name = curUser.DeptName;
                            dept.isparent = true;
                            list = GetChangeDept(dept);
                            dept.children = list;
                            result.Add(dept);
                        }
                    }
                }
                else if (type == "1")
                {
                    if (!string.IsNullOrEmpty(showtype))
                    {
                        DeptData dept = new DeptData();
                        var deptcontract = departmentBLL.GetList().Where(t => t.OrganizeId == organizeId && t.Description == "外包工程承包商").OrderBy(t => t.SortCode).ToList().FirstOrDefault();
                        if (deptcontract != null)
                        {
                            dept.deptid = deptcontract.DepartmentId;
                            dept.code = deptcontract.EnCode;
                            dept.isorg = 0;
                            dept.oranizeid = deptcontract.OrganizeId;
                            dept.parentcode = "";
                            dept.parentid = parentId;
                            dept.name = deptcontract.FullName;
                            dept.isparent = true;
                            dept.isoptional = "1";
                            list = GetChangeDept(dept);
                            dept.children = list;
                        }
                        result.Add(dept);
                    }
                    else
                    {
                        if (curUser.RoleName.Contains("厂级") || curUser.RoleName.Contains("公司"))
                        {
                            DeptData dept = new DeptData();
                            var deptcontract = departmentBLL.GetList().Where(t => t.OrganizeId == organizeId && t.Description == "外包工程承包商").OrderBy(t => t.SortCode).ToList().FirstOrDefault();
                            if (deptcontract != null)
                            {
                                dept.deptid = deptcontract.DepartmentId;
                                dept.code = deptcontract.EnCode;
                                dept.isorg = 0;
                                dept.oranizeid = deptcontract.OrganizeId;
                                dept.parentcode = "";
                                dept.parentid = parentId;
                                dept.name = deptcontract.FullName;
                                dept.isparent = true;
                                dept.isoptional = "1";
                                list = GetChangeDept(dept);
                                dept.children = list;
                            }
                            result.Add(dept);
                        }
                        else if (curUser.RoleName.Contains("承包商") || curUser.RoleName.Contains("分包商"))
                        {
                            DeptData dept = new DeptData();
                            dept.deptid = curUser.DeptId;
                            dept.code = curUser.DeptCode;
                            dept.isorg = 0;
                            dept.oranizeid = curUser.OrganizeId;
                            dept.parentcode = "";
                            dept.parentid = parentId;
                            dept.name = curUser.DeptName;
                            dept.isparent = true;
                            list = GetChangeDept(dept);
                            dept.children = list;
                            result.Add(dept);
                        }
                        else
                        {
                            string departIds = GetDeptIds(curUser.DeptId);
                            var deptcontract = departmentBLL.GetList().Where(t => t.OrganizeId == organizeId && departIds.Contains(t.DepartmentId) && t.DepartmentId != "0").OrderBy(x => x.SortCode).ToList();
                            foreach (var item in deptcontract)
                            {
                                DeptData dept = new DeptData();
                                dept.deptid = item.DepartmentId;
                                dept.code = item.EnCode;
                                dept.isorg = 0;
                                dept.oranizeid = item.OrganizeId;
                                dept.parentcode = "";
                                dept.parentid = parentId;
                                dept.name = item.FullName;
                                dept.isparent = true;
                                dept.isoptional = "";
                                list = GetChangeDept(dept);
                                dept.children = list;
                                result.Add(dept);
                            }
                        }
                    }
                }
                else if (type == "2")
                {
                    DeptData dept = new DeptData();
                    dept.deptid = curUser.OrganizeId;
                    dept.code = curUser.OrganizeCode;
                    dept.isorg = 1;
                    dept.oranizeid = curUser.OrganizeId;
                    dept.parentcode = "";
                    dept.parentid = parentId;
                    dept.name = curUser.OrganizeName;
                    dept.isparent = true;
                    list = GetChangeDept(dept);
                    dept.children = list;
                    dept.isoptional = "1";
                    result.Add(dept);
                    //result.AsEnumerable().Select(t=>t.DeptType!="")
                }
                result = ExchangeData(result.ToList());
                return new { code = 0, info = "获取数据成功", count = result.Count(), data = result };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }
        /// <summary>
        /// 将外包工程承包商节点换成长协、临时两个节点
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<DeptData> ExchangeData(List<DeptData> data)
        {
            List<DeptData> result = new List<DeptData>();
            for (int i = 0; i < data.Count; i++)
            {
                DeptData item = data[i];
                if (item.name == "外包工程承包商")
                {
                    string parentid = item.parentid;
                    DeptData cx = new DeptData()
                    {
                        deptid = "cx001_" + item.deptid,
                        code = "cx001_" + item.code,
                        newcode = "cx001_" + item.newcode,
                        name = "长协外包单位",
                        parentcode = item.parentcode,
                        parentid = item.parentid,
                        oranizeid = item.oranizeid,
                        isorg = item.isorg,
                        isparent = item.isparent,
                        isoptional = item.isoptional,
                        Manager = item.Manager,
                        ManagerId = item.ManagerId,
                        DeptType = item.DeptType,
                        children = item.children.Where(t => t.DeptType == "长协").ToList()
                    };
                    DeptData ls = new DeptData()
                    {
                        deptid = "ls001_" + item.deptid,
                        code = "ls001_" + item.code,
                        newcode = "ls001_" + item.newcode,
                        name = "临时外包单位",
                        parentcode = item.parentcode,
                        parentid = item.parentid,
                        oranizeid = item.oranizeid,
                        isorg = item.isorg,
                        isparent = item.isparent,
                        isoptional = item.isoptional,
                        Manager = item.Manager,
                        ManagerId = item.ManagerId,
                        DeptType = item.DeptType,
                        children = item.children.Where(t => t.DeptType == "临时").ToList()
                    };
                    data.RemoveAt(i);
                    data.Add(cx);
                    data.Add(ls);
                    break;
                }
                else
                {
                    item.children = ExchangeData(item.children.ToList());
                }
            }
            return data;
        }
        #endregion

        #region 获取部门
        /// <summary>
        /// 获取部门
        /// </summary>
        /// <param name="parentDept"></param>
        /// <returns></returns>
        public IList<DeptData> GetChangeDept(DeptData parentDept, string type = "")
        {
            IList<DeptData> list = new List<DeptData>();

            try
            {
                string parentId = parentDept.deptid;
                List<DepartmentEntity> plist = new List<DepartmentEntity>();
                if (!string.IsNullOrEmpty(type))
                {
                    plist = departmentBLL.GetList().Where(t => t.ParentId == parentId).OrderBy(t => t.SortCode).ToList();
                }
                else
                {
                    plist = departmentBLL.GetList().Where(t => t.ParentId == parentId && t.Description != "外包工程承包商").OrderBy(t => t.SortCode).ToList();
                }
                if (plist.Count() > 0)
                {
                    var dlist = departmentBLL.GetList().Where(t => t.OrganizeId == parentDept.oranizeid).OrderBy(t => t.SortCode).ToList();
                    foreach (DepartmentEntity entity in plist)
                    {
                        DeptData depts = new DeptData();
                        depts.deptid = entity.DepartmentId;
                        depts.code = entity.EnCode;
                        depts.oranizeid = entity.OrganizeId;
                        depts.name = entity.FullName;
                        depts.isorg = 0;
                        depts.parentid = entity.ParentId;
                        if (depts.parentid == "0")
                        {
                            depts.parentid = depts.oranizeid;
                        }
                        depts.parentcode = parentDept.parentcode;
                        var pdepts = dlist.Where(p => p.ParentId == depts.deptid).ToList();
                        if (pdepts.Count() > 0)
                        {
                            depts.isparent = true;
                            var glist = GetChangeDept(depts);
                            depts.children = glist;
                        }
                        else
                        {
                            depts.isparent = false;
                            depts.children = new List<DeptData>();
                        }
                        depts.DeptType = entity.DeptType;
                        list.Add(depts);
                    }
                }
                else
                {
                    if (parentDept.parentid != "0")
                    {
                        list.Add(parentDept);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return list;
        }



        /// <summary>
        /// 获取部门
        /// </summary>
        /// <param name="parentDept"></param>
        /// <returns></returns>
        public IList<DeptData> GetContronDept(DeptData parentDept, string oranizeid, string deptid = "")
        {
            IList<DeptData> list = new List<DeptData>();

            try
            {
                string parentId = parentDept.deptid;
                List<DepartmentEntity> plist = new List<DepartmentEntity>();

                string departIds = GetDeptIds(deptid);

                if (!string.IsNullOrEmpty(deptid))
                {
                    plist = departmentBLL.GetList(oranizeid).Where(t => t.ParentId == parentId && departIds.Contains(t.DepartmentId)).OrderBy(x => x.SortCode).ToList();
                }
                else
                {
                    plist = departmentBLL.GetList(oranizeid).Where(t => t.ParentId == parentId).OrderBy(t => t.SortCode).ToList();
                }
                if (plist.Count() > 0)
                {
                    var dlist = departmentBLL.GetList().Where(t => t.OrganizeId == parentDept.oranizeid).OrderBy(t => t.SortCode).ToList();
                    foreach (DepartmentEntity entity in plist)
                    {
                        DeptData depts = new DeptData();
                        depts.deptid = entity.DepartmentId;
                        depts.code = entity.EnCode;
                        depts.oranizeid = entity.OrganizeId;
                        depts.name = entity.FullName;
                        depts.isorg = 0;
                        depts.parentid = entity.ParentId;
                        if (depts.parentid == "0")
                        {
                            depts.parentid = depts.oranizeid;
                        }
                        depts.parentcode = parentDept.parentcode;
                        var pdepts = dlist.Where(p => p.ParentId == depts.deptid).ToList();
                        if (pdepts.Count() > 0)
                        {
                            depts.isparent = true;
                            var glist = GetChangeDept(depts);
                            depts.children = glist;
                        }
                        else
                        {
                            depts.isparent = false;
                            depts.children = new List<DeptData>();
                        }
                        list.Add(depts);
                    }
                }
                else
                {
                    if (parentDept.parentid != "0")
                    {
                        list.Add(parentDept);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return list;
        }

        public string GetDeptIds(string deptId)
        {
            DataTable dt = departmentBLL.GetDataTable(string.Format("select  distinct outprojectid from epg_outsouringengineer where engineerletdeptid in ('{0}')", deptId.Replace(",", "','")));
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in dt.Rows)
            {
                sb.AppendFormat("{0},", dr[0].ToString());
            }
            return sb.ToString().Trim(',');
        }
        #endregion

        #region 根据作业单位获取工程
        /// 根据作业单位获取工程
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetEngineeringInfo([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string showtype = res.Contains("showtype") ? dy.data.showtype : "";//是否是旁站监督页面
                string workdeptid = dy.data.workdeptid;//作业单位id

                //判断是否是承包商底下的部门，如果是则赋值作业单位为承包商
                var dept = departmentBLL.GetEntity(workdeptid);
                if (dept != null)
                {
                    var sjdept = departmentBLL.GetList(curUser.OrganizeId).Where(t => dept.EnCode.Contains(t.EnCode) && t.Nature == "承包商" && t.EnCode != dept.EnCode).FirstOrDefault();
                    if (sjdept != null)
                    {
                        workdeptid = sjdept.DepartmentId;
                    }
                }
                List<OutsouringengineerEntity> engList = new List<OutsouringengineerEntity>();
                if (!string.IsNullOrEmpty(showtype))
                {
                    engList = outsouringengineerbll.GetList().Where(x => x.OUTPROJECTID == workdeptid && x.ENGINEERSTATE == "002").ToList();
                }
                else
                {
                    if (curUser.RoleName.Contains("厂级部门用户") || curUser.RoleName.Contains("公司级用户"))
                    {
                        engList = outsouringengineerbll.GetList().Where(x => x.CREATEUSERORGCODE == curUser.OrganizeCode && x.OUTPROJECTID == workdeptid && x.ENGINEERSTATE == "002").ToList();
                    }
                    else if (curUser.RoleName.Contains("承包商级用户"))
                    {
                        engList = outsouringengineerbll.GetList().Where(x => x.OUTPROJECTID == workdeptid && x.ENGINEERSTATE == "002").ToList();
                    }
                    else if (curUser.RoleName.Contains("专业级用户") || curUser.RoleName.Contains("班组级用户"))
                    {
                        var pDept = new DepartmentBLL().GetParentDeptBySpecialArgs(curUser.ParentId, "部门");
                        engList = outsouringengineerbll.GetList().Where(x => x.ENGINEERLETDEPTID == pDept.DepartmentId && x.OUTPROJECTID == workdeptid && x.ENGINEERSTATE == "002").ToList();
                    }
                    else
                    {
                        engList = outsouringengineerbll.GetList().Where(x => x.ENGINEERLETDEPTID == curUser.DeptId && x.OUTPROJECTID == workdeptid && x.ENGINEERSTATE == "002").ToList();
                    }
                }
                return new
                {
                    code = 0,
                    count = -1,
                    info = "获取数据成功",
                    data = engList.Select(x => new { engineeringname = x.ENGINEERNAME, engineeringid = x.ID, workareaname = (districtbll.GetEntity(string.IsNullOrEmpty(x.ENGINEERAREA) ? "" : x.ENGINEERAREA)) != null ? districtbll.GetEntity(x.ENGINEERAREA).DistrictName : "" })
                };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }
        #endregion

        #region 根据作业类型获取工作任务
        /// <summary>
        /// 根据作业类型获取工作任务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetWorkRiskList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string queryJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    WorkType = dy.data.worktypevalue,//作业类型值
                    keyWord = dy.data.worktask//工作任务
                });
                //获取页数和条数
                int page = Convert.ToInt32(dy.data.pagenum), rows = Convert.ToInt32(dy.data.pagesize);
                Pagination pagination = new Pagination();
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "id";
                pagination.p_fields = @"worktask,worktype,createdate,createuserorgcode,risklevelval";
                pagination.conditionJson += "  createuserorgcode=" + curUser.OrganizeCode;
                pagination.p_tablename = "bis_risktrainlib";
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "createdate";//排序字段
                pagination.sord = "desc";//排序方式
                List<object> datas = new List<object>();
                DataTable dt = risktrainlibbll.GetPageListJson(pagination, queryJson);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var riskdt = risktrainlibdetailbll.GetTrainLibDetail(dt.Rows[i]["id"].ToString());
                    var tempdata = new
                    {
                        id = dt.Rows[i]["id"],
                        worktask = dt.Rows[i]["worktask"],
                        riskdetail = riskdt,
                        risklevel = dt.Rows[i]["risklevelval"]
                    };
                    datas.Add(tempdata);
                }
                Dictionary<string, string> dict_props = new Dictionary<string, string>();
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                    DateFormatString = "yyyy-MM-dd HH:mm", //格式化日期
                };
                return new { code = 0, info = "获取数据成功", count = pagination.records, data = datas };
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }
        #endregion


        /// <summary>
        /// 获取状态值
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetStateList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string showtype = dy.data.showtype;
                var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'" + showtype + "'");
                return new { code = 0, info = "获取数据成功", count = itemlist.Count(), data = itemlist.Select(x => new { itemvalue = x.ItemValue, itemname = x.ItemName }) };
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }
        #endregion

        #region 根据单位获取专业分类
        ///  根据单位获取专业分类
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetSpecialtyType([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string workdeptid = dy.data.workdeptid;//作业单位id
                string workdepttype = dy.data.workdepttype;//作业单位类别
                string workmoduletype = res.Contains("workmoduletype") ? dy.data.workmoduletype : "";
                switch (workmoduletype)
                {
                    case "1": //华润电力起重吊装获取专业获取特定部门
                        workdeptid = string.IsNullOrEmpty(dataitemdetailbll.GetItemValue("LifthoistDept")) ? workdeptid : dataitemdetailbll.GetItemValue("LifthoistDept");
                        break;
                    default:
                        break;
                }
                var list = scaffoldbll.GetSpecialtyToJson(workdeptid, "", workdepttype);
                return new
                {
                    code = 0,
                    count = -1,
                    info = "获取数据成功",
                    data = list
                };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }
        #endregion
        #region 省级
        #region 高风险作业统计(省级)
        /// <summary>
        ///按作业类型统计
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetProvinceHighStatistics([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            //获取用户Id
            string userId = dy.userid;
            OperatorProvider.AppUserId = userId;  //设置当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            string starttime = res.Contains("starttime") ? dy.data.starttime : "";//开始时间
            string endtime = res.Contains("endtime") ? dy.data.endtime : "";//结束时间
            string deptid = res.Contains("deptid") ? dy.data.deptid : "";//单位id
            string deptcode = res.Contains("deptcode") ? dy.data.deptcode : "";//单位code
            var resulttable = provincehighworkbll.AppGetHighWork(starttime, endtime, deptid, deptcode);
            return new
            {
                code = 0,
                count = resulttable.Rows.Count,
                info = "获取数据成功",
                data = resulttable
            };
        }
        #endregion

        #region 获取高风险作业类型
        /// <summary>
        /// 获取高风险作业类型
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetProvinceWorkType()
        {
            try
            {
                var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'StatisticsType'");
                return new { code = 0, info = "获取数据成功", count = itemlist.Count(), data = itemlist.Select(x => new { worktypevalue = x.ItemValue, worktype = x.ItemName }) };
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }
        #endregion

        #region 获取高风险作业清单
        /// <summary>
        /// 获取高风险作业清单
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetProvinceWorkList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

                string queryJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    st = dy.data.starttime,
                    et = dy.data.endtime,
                    worktype = dy.data.worktypevalue,
                    deptid = dy.data.deptid,
                    workdeptcode = dy.data.deptcode
                });

                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                Pagination pagination = new Pagination();
                pagination.p_kid = "a.id";
                pagination.p_fields = "worktype as worktypevalue,b.itemname worktype,applynumber,to_char(WorkStartTime,'yyyy-mm-dd hh24:mi') as workstarttime,to_char(WorkEndTime,'yyyy-mm-dd hh24:mi') as workendtime,workdeptname,c.fullname as createuserorgname";
                pagination.p_tablename = "v_highriskstat a left join base_dataitemdetail b on a.worktype=b.itemvalue and itemid =(select itemid from base_dataitem where itemcode='StatisticsType') left join base_department c on a.createuserorgcode=c.encode";
                pagination.conditionJson = string.Format("WorkDeptCode in (select encode from base_department start with encode='{0}' connect by  prior departmentid = parentid)", curUser.NewDeptCode);
                pagination.page = Convert.ToInt32(dy.data.pagenum);//页数
                pagination.rows = Convert.ToInt32(dy.data.pagesize);//行数
                pagination.sidx = "a.createdate";//排序字段
                pagination.sord = "desc";//排序方式

                DataTable dt = provincehighworkbll.GetPageDataTable(pagination, queryJson);
                return new { code = 0, info = "获取数据成功", count = pagination.records, data = dt };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }
        #endregion
        #endregion

        #region 删除文件
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
        #endregion

        #region 作业台账
        #region 获取作业状态
        /// <summary>
        /// 获取作业状态
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetLedgerType([FromBody]JObject json)
        {
            try
            {
                string str = @"[{ ItemValue: '0', ItemName: '即将作业' },
                                { ItemValue: '1', ItemName: '作业中'},
                                { ItemValue: '2', ItemName: '已结束' },
                                { ItemValue: '3', ItemName: '作业暂停' }
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

        #region   得到高风险通用作业台账列表
        /// <summary>
        /// 得到高风险通用作业台账列表
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
                        worktype = string.Empty,
                        workdeptcode = string.Empty,
                        workdeptid = string.Empty,
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
                pagination.sidx = "a.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                var list = highriskcommonapplybll.GetLedgerList(pagination, JsonConvert.SerializeObject(dy.data));
                int count = pagination.records;
                pagination.rows = 10000;
                var historyCount = highriskcommonapplybll.GetLedgerList(pagination, JsonConvert.SerializeObject(dy.data)).Rows.Count;
                var query = new
                {
                    worktype = dy.data.worktype,
                    workdeptcode = dy.data.workdeptcode,
                    workdeptid = dy.data.workdeptid,
                    st = dy.data.st,
                    et = dy.data.et,
                    ledgertype = "1",
                    applynumber = dy.data.applynumber
                };
                var workingCount = highriskcommonapplybll.GetLedgerList(pagination, JsonConvert.SerializeObject(query)).Rows.Count;
                Dictionary<string, string> dict_props = new Dictionary<string, string>();
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                    DateFormatString = "yyyy-MM-dd HH:mm" //格式化日期
                };
                var data = new
                {
                    list = list,
                    historyCount = historyCount,
                    workingCount = workingCount
                };
                settings.Converters.Add(new DecimalToStringConverter());
                return new { code = 0, info = "获取数据成功", count = count, data = JObject.Parse(JsonConvert.SerializeObject(data, Formatting.None, settings)) };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message };
            }

        }
        #endregion
        #endregion

        #region 今日高风险作业
        /// <summary>
        /// 得到高风险通用作业台账列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetTodayWorkList([FromBody]JObject json)
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
                        dutydeptid = string.Empty
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
                pagination.sidx = "t.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                var list = highriskcommonapplybll.GetTodayWorkList(pagination, JsonConvert.SerializeObject(dy.data));
                Dictionary<string, string> dict_props = new Dictionary<string, string>();
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                    DateFormatString = "yyyy-MM-dd HH:mm" //格式化日期
                };

                settings.Converters.Add(new DecimalToStringConverter());
                return new { code = 0, info = "获取数据成功", count = pagination.records, data = JArray.Parse(JsonConvert.SerializeObject(list, Formatting.None, settings)) };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message };
            }
        }

        #endregion

        #region 西塞山大屏-根据责任部门分组获取今日高风险作业
        [HttpPost]
        public object GetTodayWorkData([FromBody]JObject json)
        {
            try
            {
                //string res = json.Value<string>("json");
                //dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //
                List<DutyDeptWork> tempData = new List<DutyDeptWork>();
                string sql = string.Format(@"select departmentid,departmentname from ( select    to_char(e.engineerletdeptid) as departmentid,to_char(e.engineerletdept) as departmentname,count(*) as worknum
                                      from (select workdeptcode, engineeringid, workdepttype
                                              from v_xssunderwaywork) t
                                      left join epg_outsouringengineer e
                                        on e.id = t.engineeringid
                                      where workdepttype=1
                                      group by e.engineerletdeptid,e.engineerletdept
                                      union all
                                      select to_char(b.departmentid)  as departmentid,to_char(b.fullname) as departmentname,count(*) as worknum
                                        from v_xssunderwaywork a
                                        left join (select encode, fullname, sortcode,departmentid
                                                     from base_department
                                                    where nature = '部门'
                                                   ) b
                                          on substr(a.workdeptcode, 0, length(b.encode)) = b.encode
                                          where a.workdepttype =0 group by  b.departmentid,b.fullname) where departmentid is not null group by departmentid,departmentname");
                var data = highriskcommonapplybll.GetTable(sql);
                var totalProNum = 0;
                var totalPersonNum = 0;
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    DutyDeptWork itemData = new DutyDeptWork();
                    List<TodayWorkEntity> ProList = new List<TodayWorkEntity>();
                    itemData.DutyDeptId = data.Rows[i]["departmentid"].ToString();
                    itemData.DutyDeptName = data.Rows[i]["departmentname"].ToString();
                    string sql1 = string.Format(@"select t.id,b.itemname as risktypename,t.risktype as risktypevalue,t.workdeptname,t.workplace, t.worktypename,t.WorkUserNames,t.WorkDutyUserName,t.WorkTutelageUserName
                                      from v_xssunderwaywork t
                                      left join base_dataitemdetail b
                                        on t.risktype = b.itemvalue
                                       and b.itemid =
                                           (select itemid from base_dataitem where itemcode = 'CommonRiskType')
                                     where ((workdeptcode in
                                           (select encode from base_department where encode like '{0}%')) or
                                           (engineeringid in
                                           (select id
                                                from epg_outsouringengineer a
                                               where a.engineerletdeptid = '{1}')))", departmentBLL.GetEntity(data.Rows[i]["departmentid"].ToString()).EnCode, data.Rows[i]["departmentid"].ToString());
                    DataTable dt = highriskcommonapplybll.GetTable(sql1);
                    itemData.WorkNum = dt.Rows.Count;
                    totalProNum += itemData.WorkNum;
                    foreach (DataRow item in dt.Rows)
                    {
                        TodayWorkEntity pro = new TodayWorkEntity();
                        pro.WorkDept = item["workdeptname"].ToString();
                        pro.WorkType = item["worktypename"].ToString();
                        pro.WorkPlace = item["workplace"].ToString();
                        pro.RiskType = item["risktypename"].ToString();
                        pro.RiskTypeValue = item["risktypevalue"].ToString();
                        pro.WorkTutelagePerson = item["WorkTutelageUserName"].ToString();
                        pro.id = item["id"].ToString();
                        ProList.Add(pro);
                        itemData.WorkPersonNum += string.IsNullOrEmpty(item["WorkUserNames"].ToString()) ? 0 : item["WorkUserNames"].ToString().Split(',').Length;
                        itemData.WorkPersonNum += string.IsNullOrEmpty(item["WorkDutyUserName"].ToString()) ? 0 : 1;
                        itemData.WorkPersonNum += string.IsNullOrEmpty(item["WorkTutelageUserName"].ToString()) ? 0 : 1;
                    }
                    itemData.TodayWorkList = ProList;
                    totalPersonNum += itemData.WorkPersonNum;
                    tempData.Add(itemData);
                }
                var jsonData = new
                {
                    tempData = tempData,
                    totalProNum = totalProNum,
                    totalPersonNum = totalPersonNum
                };
                return new { code = 0, count = tempData.Count, info = "获取数据成功", data = jsonData };
            }
            catch (Exception ex)
            {

                return new { code = -1, count = 0, info = "获取数据失败：" + ex.Message, data = new object() };
            }

        }
        #endregion

        #region 贵州毕节大屏-根据单位内部单位外部分组获取今日高风险作业
        [HttpPost]
        public object GetTodayWorkDataForGZBJ(string orgcode)
        {
            try
            {
                //string res = json.Value<string>("json");
                //dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //string orgcode = dy.orgcode;
                List<DutyDeptWork> tempData = new List<DutyDeptWork>();
                var totalProNum = 0;
                var totalPersonNum = 0;
                string sql = string.Format(@"select t.id,b.itemname as risktypename,t.risktype as risktypevalue,t.workdeptname,t.workplace,t.workcontent,t.worktypename,t.WorkUserNames,t.WorkDutyUserName,t.WorkTutelageUserName,t.auditusername,t.workdepttype
                                      from v_xssunderwaywork t
                                      left join base_dataitemdetail b
                                        on t.risktype = b.itemvalue
                                       and b.itemid =
                                           (select itemid from base_dataitem where itemcode = 'CommonRiskType')
                                     where t.createuserorgcode='{0}' order by risktype asc", orgcode);

                DutyDeptWork itemData = new DutyDeptWork();
                List<TodayWorkEntity> ProList = new List<TodayWorkEntity>();
                DataTable dt = highriskcommonapplybll.GetTable(sql);
                itemData.WorkNum = dt.Rows.Count;
                foreach (DataRow item in dt.Rows)
                {
                    TodayWorkEntity pro = new TodayWorkEntity();
                    pro.WorkDept = item["workdeptname"].ToString();
                    pro.WorkType = item["worktypename"].ToString();
                    pro.RiskType = item["risktypename"].ToString();
                    pro.RiskTypeValue = item["risktypevalue"].ToString();
                    pro.WorkTutelagePerson = item["worktutelageusername"].ToString();
                    pro.AuditUserName = item["auditusername"].ToString();
                    pro.WorkContent = item["workcontent"].ToString();
                    pro.WorkPlace = item["workplace"].ToString();
                    pro.WorkDeptType = item["workdepttype"].ToString();
                    pro.id = item["id"].ToString();
                    ProList.Add(pro);
                    itemData.WorkPersonNum += string.IsNullOrEmpty(item["WorkUserNames"].ToString()) ? 0 : item["WorkUserNames"].ToString().Split(',').Length;
                    itemData.WorkPersonNum += string.IsNullOrEmpty(item["WorkDutyUserName"].ToString()) ? 0 : 1;
                    itemData.WorkPersonNum += string.IsNullOrEmpty(item["WorkTutelageUserName"].ToString()) ? 0 : 1;
                }
                itemData.TodayWorkList = ProList;
                totalProNum += itemData.WorkNum;
                totalPersonNum += itemData.WorkPersonNum;
                tempData.Add(itemData);

                var jsonData = new
                {
                    tempData = tempData,
                    totalProNum = totalProNum,
                    totalPersonNum = totalPersonNum
                };
                return new { code = 0, count = tempData.Count, info = "获取数据成功", data = jsonData };
            }
            catch (Exception ex)
            {

                return new { code = -1, count = 0, info = "获取数据失败：" + ex.Message, data = new object() };
            }

        }
        #endregion

        #region 根据区域获取今日高风险作业列表
        [HttpPost]
        public object GetTodayWorkDataByAreaForGZBJ(string areacode)
        {
            try
            {
                string isJdz = new DataItemDetailBLL().GetItemValue("景德镇版本");
                string str = string.Empty;
                if (!string.IsNullOrEmpty(isJdz))
                {
                    str = "(CASE WHEN WORKTYPE='12' THEN (select itemid from base_dataitem where itemcode='CommonWorkType') ELSE (select itemid from base_dataitem where itemcode='CommonRiskType') END)";
                }
                else
                {
                    str = "(select itemid from base_dataitem where itemcode='CommonRiskType')";
                }
                string sql = string.Format(@"select t.id,b.itemname as risktypename,t.risktype as risktypevalue,t.workdeptname,t.workplace,t.workcontent,t.worktypename,t.WorkUserNames,t.WorkDutyUserName,t.WorkTutelageUserName,t.auditusername,t.workdepttype
                                      from v_xssunderwaywork t
                                      left join base_dataitemdetail b
                                        on t.risktype = b.itemvalue
                                       and b.itemid ={1}
                                     where  workareacode='{0}' order by risktype asc", areacode, str);

                DutyDeptWork itemData = new DutyDeptWork();
                List<TodayWorkEntity> ProList = new List<TodayWorkEntity>();
                DataTable dt = highriskcommonapplybll.GetTable(sql);
                foreach (DataRow item in dt.Rows)
                {
                    TodayWorkEntity pro = new TodayWorkEntity();
                    pro.WorkDept = item["workdeptname"].ToString();
                    pro.WorkType = item["worktypename"].ToString();
                    pro.RiskType = item["risktypename"].ToString();
                    pro.RiskTypeValue = item["risktypevalue"].ToString();
                    pro.WorkTutelagePerson = item["worktutelageusername"].ToString();
                    pro.AuditUserName = item["auditusername"].ToString();
                    pro.WorkContent = item["workcontent"].ToString();
                    pro.WorkPlace = item["workplace"].ToString();
                    pro.WorkDeptType = item["workdepttype"].ToString();
                    pro.id = item["id"].ToString();
                    ProList.Add(pro);
                }

                var jsonData = new
                {
                    tempData = ProList
                };
                return new { code = 0, count = ProList.Count, info = "获取数据成功", data = jsonData };
            }
            catch (Exception ex)
            {

                return new { code = -1, count = 0, info = "获取数据失败：" + ex.Message, data = new object() };
            }

        }
        #endregion

        #region 获取今天还在处理中的高风险作业（包含审核中、审核通过的数据）
        [HttpPost]
        public object GetTodayWorkingData(string orgcode)
        {
            try
            {
                //string res = json.Value<string>("json");
                //dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //string orgcode = dy.orgcode;
                List<DutyDeptWork> tempData = new List<DutyDeptWork>();
                var totalProNum = 0;
                var totalPersonNum = 0;
                string sql = string.Format(@"select t.id,b.itemname as risktypename,t.risktype as risktypevalue,t.workdeptname,t.workplace,t.workcontent,t.worktypename,t.WorkUserNames,t.WorkDutyUserName,t.WorkTutelageUserName,t.auditusername,t.workdepttype,t.statusname
                                      from V_DOINGHIGHWORK t
                                      left join base_dataitemdetail b
                                        on t.risktype = b.itemvalue
                                       and b.itemid =
                                           (select itemid from base_dataitem where itemcode = 'CommonRiskType')
                                     where t.createuserorgcode='{0}' order by risktype asc", orgcode);

                DutyDeptWork itemData = new DutyDeptWork();
                List<TodayWorkEntity> ProList = new List<TodayWorkEntity>();
                DataTable dt = highriskcommonapplybll.GetTable(sql);
                itemData.WorkNum = dt.Rows.Count;
                foreach (DataRow item in dt.Rows)
                {
                    TodayWorkEntity pro = new TodayWorkEntity();
                    pro.WorkDept = item["workdeptname"].ToString();
                    pro.WorkType = item["worktypename"].ToString();
                    pro.RiskType = item["risktypename"].ToString();
                    pro.RiskTypeValue = item["risktypevalue"].ToString();
                    pro.WorkTutelagePerson = item["worktutelageusername"].ToString();
                    pro.AuditUserName = item["auditusername"].ToString();
                    pro.WorkContent = item["workcontent"].ToString();
                    pro.WorkPlace = item["workplace"].ToString();
                    pro.WorkDeptType = item["workdepttype"].ToString();
                    pro.StatusName = item["statusname"].ToString();
                    pro.id = item["id"].ToString();
                    ProList.Add(pro);
                    itemData.WorkPersonNum += string.IsNullOrEmpty(item["WorkUserNames"].ToString()) ? 0 : item["WorkUserNames"].ToString().Split(',').Length;
                    itemData.WorkPersonNum += string.IsNullOrEmpty(item["WorkDutyUserName"].ToString()) ? 0 : 1;
                    itemData.WorkPersonNum += string.IsNullOrEmpty(item["WorkTutelageUserName"].ToString()) ? 0 : 1;
                }
                itemData.TodayWorkList = ProList;
                totalProNum += itemData.WorkNum;
                totalPersonNum += itemData.WorkPersonNum;
                tempData.Add(itemData);

                var jsonData = new
                {
                    tempData = tempData,
                    totalProNum = totalProNum,
                    totalPersonNum = totalPersonNum
                };
                return new { code = 0, count = tempData.Count, info = "获取数据成功", data = jsonData };
            }
            catch (Exception ex)
            {

                return new { code = -1, count = 0, info = "获取数据失败：" + ex.Message, data = new object() };
            }

        }
        #endregion

        #region 审核审批转交
        [HttpPost]
        public object SaveTransferRecord([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, info = "请求失败,请登录!" };
                }
                string transferentity = JsonConvert.SerializeObject(dy.data.transferentity);
                TransferrecordEntity entity = JsonConvert.DeserializeObject<TransferrecordEntity>(transferentity);
                entity.OutTransferUserAccount = curUser.Account;
                entity.OutTransferUserId = curUser.UserId;
                entity.OutTransferUserName = curUser.UserName;
                string modulename = dy.data.modulename;
                string sql = string.Format("select * from BASE_MODULE t where t.fullname='{0}'", modulename);
                var dtModule = highriskcommonapplybll.GetTable(sql);
                if (dtModule.Rows.Count > 0)
                {
                    entity.ModuleId = dtModule.Rows[0]["moduleid"].ToString();
                }
                transferrecordbll.SaveRealForm("", entity);
                return new { code = 0, info = "获取数据成功" };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message };
            }
        }
        #endregion

    }

    #region 状态类
    public class itemClass
    {
        public string ItemValue { get; set; }
        public string ItemName { get; set; }
    }
    #endregion

    #region  措施类
    public class MeasultData
    {
        public string measureid { get; set; }
        public string measurename { get; set; }
        public string measureresultone { get; set; }
        public string measureresulttwo { get; set; }
        public string measureresult { get; set; }
        public string checkpersons { get; set; }
        public string checkpersonsid { get; set; }
        public string signpic { get; set; }
    }
    #endregion

    #region 责任部门作业类
    public class DutyDeptWork
    {
        public string DutyDeptId;
        public string DutyDeptName;
        public int WorkNum;
        public int WorkPersonNum;
        public IList<TodayWorkEntity> TodayWorkList;
        public string WorkDeptType;
    }
    #endregion

    #region 作业列表类
    public class TodayWorkEntity
    {
        public string WorkDept; //作业单位
        public string WorkType; //作业类型
        public string WorkPlace; //作业地点
        public string RiskType;  //风险等级
        public string WorkTutelagePerson;  //监护人
        public string id;
        public string RiskTypeValue; //风险等级value
        public string WorkContent; //作业内容
        public string AuditUserName; //审批人
        public string WorkDeptType; //作业单位类型
        public string StatusName; //作业状态
    }
    #endregion
}