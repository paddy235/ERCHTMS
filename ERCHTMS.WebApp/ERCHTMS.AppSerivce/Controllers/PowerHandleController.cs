using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ERCHTMS.Entity.PowerPlantInside;
using ERCHTMS.Busines.PowerPlantInside;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Dynamic;
using ERCHTMS.Code;
using BSFramework.Util.WebControl;
using System.Data;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
using System.Web;
using System.IO;
using BSFramework.Util.Extension;
using ERCHTMS.Entity.HighRiskWork.ViewModel;

namespace ERCHTMS.AppSerivce.Controllers
{
    public class PowerHandleController : BaseApiController
    {
        private PowerplanthandleBLL powerplanthandlebll = new PowerplanthandleBLL();
        private PowerplanthandledetailBLL powerplanthandledetailbll = new PowerplanthandledetailBLL();
        private PowerplantreformBLL powerplantreformbll = new PowerplantreformBLL();
        private PowerplantcheckBLL powerplantcheckbll = new PowerplantcheckBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();
        private AptitudeinvestigateauditBLL aptitudeinvestigateauditbll = new AptitudeinvestigateauditBLL();
        private UserBLL userbll = new UserBLL();

        #region 获取事故事件处理列表
        /// <summary>
        /// 获取事故事件处理列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetPowerHandleList([FromBody]JObject json)
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
                
                //获取页数和条数
                int page = Convert.ToInt32(dy.data.pageindex), rows = Convert.ToInt32(dy.data.pagesize);
                Pagination pagination = new Pagination();
                pagination.p_kid = "a.id as powerplanthandleid";
                pagination.p_fields = "a.accidenteventname,a.happentime,a.createusername";
                pagination.p_tablename = " bis_powerplanthandle a";
                pagination.conditionJson = string.Format(" createuserorgcode = '{0}' and ISSAVED=1  and applystate= 1  and flowdept = '{1}' ", curUser.OrganizeCode, curUser.DeptId);

                string[] arrRole = curUser.RoleName.Split(',');
                pagination.conditionJson += " and (";
                foreach (string str in arrRole)
                {
                    pagination.conditionJson += string.Format(" flowrolename like '%{0}%' or", str);
                }
                pagination.conditionJson = pagination.conditionJson.Substring(0, pagination.conditionJson.Length - 2);
                pagination.conditionJson += " )";
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "a.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                DataTable dt = powerplanthandlebll.GetPageList(pagination, "");
                return new { code = 0, info = "获取数据成功", count = pagination.records, data = dt };
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }
        #endregion

        #region 获取事故事件整改/验收列表
        [HttpPost]
        public object GetPowerHandleReformOrCheckList([FromBody]JObject json)
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

                string type = dy.data.type; //0:事故事件整改 1:事故事件验收
                //获取页数和条数
                int page = Convert.ToInt32(dy.data.pageindex), rows = Convert.ToInt32(dy.data.pagesize);
                Pagination pagination = new Pagination();
                pagination.p_kid = "a.id as powerplanthandledetailid";
                pagination.p_fields = "c.accidenteventname,c.happentime,c.createusername,c.id as powerplanthandleid,b.id as powerplantreformid";
                pagination.p_tablename = "bis_powerplanthandledetail a left join bis_powerplantreform b on a.id=b.powerplanthandledetailid and b.disable=0 left join bis_powerplanthandle c on a.powerplanthandleid =c.id ";
                pagination.conditionJson = "1=1";

                if (type == "0")
                {
                    pagination.conditionJson += string.Format(" and a.applystate=3 and a.rectificationdutypersonid like '%{0}%'", curUser.UserId);
                }
                else if (type == "1")
                {
                    string[] roles = curUser.RoleName.Split(',');
                    string roleWhere = "";
                    foreach (var r in roles)
                    {
                        roleWhere += string.Format("or a.flowrolename like '%{0}%'", r);
                    }
                    roleWhere = roleWhere.Substring(2);
                    pagination.conditionJson += string.Format(" and a.applystate=4 and a.flowdept like '%{0}%' and ({1})", curUser.DeptId, roleWhere);
                }

                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "a.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                DataTable dt = powerplanthandlebll.GetPageList(pagination, "");
                return new { code = 0, info = "获取数据成功", count = pagination.records, data = dt };
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }
        #endregion

        #region 获取我的数据
        [HttpPost]
        public object GetOperateList([FromBody]JObject json)
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
                string st = res.Contains("sttime") ? dy.data.sttime : "";
                string et = res.Contains("ettime") ? dy.data.ettime : "";
                string deptid = res.Contains("deptid") ? dy.data.deptid : "";
                string state = res.Contains("state") ? dy.data.state : "";
                string accidentname = res.Contains("accidentname") ? dy.data.accidentname : "";

                DataTable dt = new DataTable();
                string sql = string.Empty;
                string where = string.Empty;
                string querywhere = string.Empty;
                if (!string.IsNullOrEmpty(st))
                {
                    querywhere += string.Format(" and a.happentime >=to_date('{0}','yyyy-mm-dd')", st);
                }
                if (!string.IsNullOrEmpty(et))
                {
                    querywhere += string.Format(" and a.happentime <=to_date('{0}','yyyy-mm-dd')", et);
                }
                if (!string.IsNullOrEmpty(deptid))
                {
                    querywhere += string.Format(" and a.belongdeptid like '%{0}%'", deptid);
                }
                if (!string.IsNullOrEmpty(accidentname))
                {
                    querywhere += string.Format(" and a.accidenteventname like '%{0}%'", accidentname);
                }
                #region 获取待审核的数据
                sql = "select a.id as powerplanthandleid,'' as powerplanthandledetailid,a.accidenteventname,a.happentime,a.belongdept,a.applystate,'' as rectificationtime,a.flowdept,a.flowrole,'' as approvenames from bis_powerplanthandle a ";
                where += string.Format(" where createuserorgcode = '{0}' and issaved=1  and applystate= 1  and flowdept = '{1}' ", curUser.OrganizeCode, curUser.DeptId);
                string[] arrRole = curUser.RoleName.Split(',');
                where += " and (";
                foreach (string str in arrRole)
                {
                    where += string.Format(" flowrolename like '%{0}%' or", str);
                }
                where = where.Substring(0, where.Length - 2);
                where += " )";
                DataTable dt1 = powerplanthandlebll.GetTableBySql(sql + where + querywhere);
                #endregion

                #region 获取待签收数据
                sql = string.Empty;
                where = string.Empty;
                sql = @"select d.id as powerplanthandledetailid,a.id as powerplanthandleid,d.applystate,a.accidenteventname,a.happentime,a.belongdept,d.signdeptname,d.rectificationdutydept,d.realreformdept,to_char(d.rectificationtime,'yyyy-mm-dd') as rectificationtime,d.flowdept,d.flowrole,d.signpersonname as approvenames 
                       from bis_powerplanthandledetail d left join bis_powerplanthandle a on d.powerplanthandleid =a.id";
                where += string.Format(" where d.createuserorgcode = '{0}'", curUser.OrganizeCode);
                where += string.Format(" and d.applystate=6 and d.signpersonid like '%{0}%'", curUser.UserId);
                DataTable dt2 = powerplanthandlebll.GetTableBySql(sql + where + querywhere);
                #endregion

                #region 获取待整改数据
                sql = string.Empty;
                where = string.Empty;
                sql = @"select d.id as powerplanthandledetailid,a.id as powerplanthandleid,d.applystate,a.accidenteventname,a.happentime,a.belongdept,d.signdeptname,d.rectificationdutydept,d.realreformdept,to_char(d.rectificationtime,'yyyy-mm-dd') as rectificationtime,d.flowdept,d.flowrole,'' as approvenames,d.rectificationdutyperson,e.outtransferusername,e.intransferusername
                      from bis_powerplanthandledetail d left join bis_powerplanthandle a on d.powerplanthandleid =a.id 
                left join (select recid,flowid,outtransferuseraccount,intransferuseraccount,outtransferusername,intransferusername,row_number()  over(partition by recid,flowid order by createdate desc) as num from BIS_TRANSFERRECORD where disable=0) e on d.id=e.recid and e.num=1";
                where += string.Format(" where d.createuserorgcode = '{0}'", curUser.OrganizeCode);
                where += string.Format(" and d.applystate=3 and (d.rectificationdutypersonid like '%{0}%' or e.intransferuseraccount like '%{1}%') and (e.outtransferuseraccount is null or e.outtransferuseraccount not like '%{1}%')", curUser.UserId, curUser.Account + ",");
                DataTable dt3 = powerplanthandlebll.GetTableBySql(sql + where + querywhere);
                #endregion

                #region 获取待验收数据
                sql = string.Empty;
                where = string.Empty;
                sql = @"select d.id as powerplanthandledetailid,a.id as powerplanthandleid,d.applystate,a.accidenteventname,a.happentime,a.belongdept,d.signdeptname,d.rectificationdutydept,d.realreformdept,to_char(d.rectificationtime,'yyyy-mm-dd') as rectificationtime,d.flowdept,d.flowrole,'' as approvenames
                       from bis_powerplanthandledetail d left join bis_powerplanthandle a on d.powerplanthandleid =a.id ";
                string[] roles = curUser.RoleName.Split(',');
                string roleWhere = "";
                foreach (var r in roles)
                {
                    roleWhere += string.Format("or d.flowrolename like '%{0}%'", r);
                }
                roleWhere = roleWhere.Substring(2);
                where += string.Format(" where d.applystate=4 and d.flowdept like '%{0}%' and ({1})", curUser.DeptId, roleWhere);
                DataTable dt4 = powerplanthandlebll.GetTableBySql(sql + where + querywhere);
                #endregion

                switch (state)
                {
                    case "0": //待审核
                        dt = dt1;
                        break;
                    case "1": //待签收
                        dt = dt2;
                        break;
                    case "2":
                        dt = dt3; //待整改
                        break;
                    case "3":
                        dt = dt4; //待验收
                        break;
                    default:
                        dt.Merge(dt1);
                        dt.Merge(dt2);
                        dt.Merge(dt3);
                        dt.Merge(dt4);
                        break;
                }

                //给审核人赋值
                foreach (DataRow item in dt.Rows)
                {
                    switch (item["applystate"].ToString())
                    {
                        case "1": //审核中
                        case "4": //验收中
                            string[] deptlist = item["flowdept"].ToString().Split(',');
                            string[] rolelist = item["flowrole"].ToString().Split(',');
                            IList<UserEntity> userlist = userbll.GetUserListByDeptId("'" + string.Join("','", deptlist) + "'", "'" + string.Join("','", rolelist) + "'", true, "");
                            string username = "";
                            if (userlist.Count > 0)
                            {
                                foreach (var temp in userlist)
                                {
                                    username += temp.RealName + ",";
                                }
                                username = string.IsNullOrEmpty(username) ? "" : username.Substring(0, username.Length - 1);
                            }
                            item["approvenames"] = username;
                            break;
                        case "3": //整改中
                            string rectificationdutyperson = item["rectificationdutyperson"].ToString(); //整改负责人
                            string outtransferusername = item["outtransferusername"].IsEmpty() ? "" : item["outtransferusername"].ToString();//转交申请人
                            string intransferuseruser = item["intransferusername"].IsEmpty() ? "" : item["intransferusername"].ToString();//转交接收人
                            string[] outtransferusernamelist = outtransferusername.Split(',');
                            string[] intransferuseruserlist = intransferuseruser.Split(',');
                            foreach (var temp in intransferuseruserlist)
                            {
                                if (!temp.IsEmpty() && !rectificationdutyperson.Contains(temp + ","))
                                {
                                    rectificationdutyperson += (temp + ",");//将转交接收人加入整改人中
                                }
                            }
                            foreach (var temp in outtransferusernamelist)
                            {
                                if (!temp.IsEmpty() && rectificationdutyperson.Contains(temp + ","))
                                {
                                    rectificationdutyperson = rectificationdutyperson.Replace(temp + ",", "");//将转交申请人从整改人中移除
                                }
                            }
                            item["approvenames"] = rectificationdutyperson;
                            break;
                        default:
                            break;
                    }

                }
                return new { code = 0, info = "获取数据成功", count = dt.Rows.Count, data = dt };
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }
        #endregion

        #region 获取全部数据
        [HttpPost]
        public object GetAllHandleList([FromBody]JObject json)
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

                //获取页数和条数
                int page = Convert.ToInt32(dy.data.pageindex), rows = Convert.ToInt32(dy.data.pagesize);
                Pagination pagination = new Pagination();
                pagination.p_kid = "a.id as powerplanthandleid";
                pagination.p_fields = "a.accidenteventname,a.happentime,a.belongdept,a.applystate";
                pagination.p_tablename = " bis_powerplanthandle a";
                pagination.conditionJson = string.Format(" createuserorgcode = '{0}'", curUser.OrganizeCode);
                string st = res.Contains("sttime") ? dy.data.sttime : "";
                string et = res.Contains("ettime") ? dy.data.ettime : "";
                string deptid = res.Contains("deptid") ? dy.data.deptid : "";
                string state = res.Contains("state") ? dy.data.state : "";
                string accidentname = res.Contains("accidentname") ? dy.data.accidentname : "";
                
                if (!string.IsNullOrEmpty(st))
                {
                    pagination.conditionJson += string.Format(" and a.happentime >=to_date('{0}','yyyy-mm-dd')", st);
                }
                if (!string.IsNullOrEmpty(et))
                {
                    pagination.conditionJson += string.Format(" and a.happentime <=to_date('{0}','yyyy-mm-dd')", et);
                }
                if (!string.IsNullOrEmpty(deptid))
                {
                    pagination.conditionJson += string.Format(" and a.belongdeptid like '%{0}%'", deptid);
                }
                if (!string.IsNullOrEmpty(accidentname))
                {
                    pagination.conditionJson += string.Format(" and a.accidenteventname like '%{0}%'", accidentname);
                }
                if (state == "0") //进行中
                {
                    pagination.conditionJson += string.Format(" and a.applystate !='5'");
                }
                else if (state == "1") //已经完成
                {
                    pagination.conditionJson += string.Format(" and a.applystate ='5' ");
                }
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "a.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                DataTable dt = powerplanthandlebll.GetPageList(pagination, "");
                return new { code = 0, info = "获取数据成功", count = pagination.records, data = dt };
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }
        #endregion

        #region 获取事故事件处理信息
        /// <summary>
        /// 获取事故事件处理详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetPowerHandleForm([FromBody]JObject json)
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
                PowerPlantHandleModel model = new PowerPlantHandleModel();
                #region 事故事件处理基本信息
                powerplanthandle powerplanthandle = new powerplanthandle();
                var powerplanthandleEntity = powerplanthandlebll.GetEntity(dy.data.id);
                if (powerplanthandleEntity == null)
                {
                    throw new ArgumentException("未找到信息");
                }
                if (powerplanthandleEntity.AccidentEventType != null)
                    powerplanthandleEntity.AccidentEventType = dataitemdetailbll.GetItemName("AccidentEventType", powerplanthandleEntity.AccidentEventType);
                if (powerplanthandleEntity.AccidentEventProperty != null)
                    powerplanthandleEntity.AccidentEventProperty = dataitemdetailbll.GetItemName("AccidentEventProperty", powerplanthandleEntity.AccidentEventProperty);
                string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                List<FileInfoEntity> file = fileinfobll.GetFileList(dy.data.id);
                IList<ERCHTMS.Entity.PowerPlantInside.Photo> fileobjects = new List<ERCHTMS.Entity.PowerPlantInside.Photo>();
                foreach (var item in file)
                {
                    ERCHTMS.Entity.PowerPlantInside.Photo temp = new ERCHTMS.Entity.PowerPlantInside.Photo();
                    temp.fileid = item.FileId;
                    temp.filename = item.FileName;
                    temp.fileurl = webUrl + item.FilePath.Replace("~", "");
                    fileobjects.Add(temp);
                }
                IList<AptitudeinvestigateauditEntity> audits = aptitudeinvestigateauditbll.GetAuditList(dy.data.id).ToList();

                foreach (var item in audits)
                {
                    item.AUDITSIGNIMG = string.IsNullOrWhiteSpace(item.AUDITSIGNIMG) ? "" : webUrl + item.AUDITSIGNIMG;
                }

                powerplanthandle.id = powerplanthandleEntity.Id;
                powerplanthandle.accidenteventname = powerplanthandleEntity.AccidentEventName;
                powerplanthandle.belongdept = powerplanthandleEntity.BelongDept;
                powerplanthandle.accidenteventtype = powerplanthandleEntity.AccidentEventType;
                powerplanthandle.accidenteventproperty = powerplanthandleEntity.AccidentEventProperty;
                powerplanthandle.happentime = powerplanthandleEntity.HappenTime;
                powerplanthandle.situationintroduction = powerplanthandleEntity.SituationIntroduction;
                powerplanthandle.reasonandproblem = powerplanthandleEntity.ReasonAndProblem;
                powerplanthandle.filelist = fileobjects;
                powerplanthandle.powerplanthandleaudits = audits;
                model.powerplanthandle = powerplanthandle;
                #endregion

                #region 事故事件处理信息
                IList<PowerplanthandledetailEntity> detaillist = powerplanthandledetailbll.GetList("").Where(t => t.PowerPlantHandleId == dy.data.id).ToList();
                IList<powerplanthandledetial>  powerplanthandledetial = new List<powerplanthandledetial>();
                foreach (var item in detaillist)
                {
                    powerplanthandledetial temp = new powerplanthandledetial();
                    temp.id = item.Id;
                    temp.rectificationdutydept = item.RectificationDutyDept;
                    temp.rectificationdutyperson = item.RectificationDutyPerson;
                    temp.rectificationmeasures = item.RectificationMeasures;
                    temp.rectificationtime = item.RectificationTime;
                    temp.reasonandproblem = item.ReasonAndProblem;
                    temp.signdeptname = item.SignDeptName;
                    temp.signdeptid = item.SignDeptId;
                    temp.signpersonname = item.SignPersonName;
                    temp.signpersonid = item.SignPersonId;
                    temp.isassignperson = item.IsAssignPerson;
                    temp.applystate = item.ApplyState.ToString();
                    temp.approvenames = powerplanthandledetailbll.GetApproveUserName(item.Id);
                    temp.realreformdept = item.RealReformDept;
                    temp.accidenteventname = powerplanthandleEntity.AccidentEventName;
                    temp.happentime = Convert.ToDateTime(powerplanthandleEntity.HappenTime).ToString("yyyy-MM-dd");
                    temp.belongdept = powerplanthandleEntity.BelongDept;
                    powerplanthandledetial.Add(temp);
                }
                model.powerplanthandledetial = powerplanthandledetial;
                #endregion

                //流程信息
                var nodelist = powerplanthandlebll.GetAppFlowList(dy.data.id);
                model.checkflow = nodelist;
                Dictionary<string, string> dict_props = new Dictionary<string, string>();

                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                    DateFormatString = "yyyy-MM-dd HH:mm", //格式化日期
                    //NullValueHandling = NullValueHandling.Ignore 值为空则在JSON中体现
                };
                return new { code = 0, info = "获取数据成功", data = JObject.Parse(JsonConvert.SerializeObject(model, Formatting.None, settings)) };
            }
            catch (Exception ex)
            {
                return new { code = -1, data = "", info = ex.Message };
            }
        }
        #endregion

        #region 获取事故事件整改/验收信息
        /// <summary>
        /// 获取事故事件处理详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetPowerHandleReformOrCheckForm([FromBody]JObject json)
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
                PowerPlantHandleModel model = new PowerPlantHandleModel();
                
                #region 事故事件处理信息
                PowerplanthandledetailEntity powerplanthandledetialEntity = powerplanthandledetailbll.GetEntity(dy.data.id);
                IList<powerplanthandledetial> powerplanthandledetial = new List<powerplanthandledetial>();
                powerplanthandledetial tempdetail = new powerplanthandledetial();
                tempdetail.id = powerplanthandledetialEntity.Id;
                tempdetail.rectificationdutydept = powerplanthandledetialEntity.RectificationDutyDept;
                tempdetail.rectificationdutyperson = powerplanthandledetialEntity.RectificationDutyPerson;
                tempdetail.rectificationmeasures = powerplanthandledetialEntity.RectificationMeasures;
                tempdetail.rectificationtime = powerplanthandledetialEntity.RectificationTime;
                tempdetail.reasonandproblem = powerplanthandledetialEntity.ReasonAndProblem;
                tempdetail.signdeptname = powerplanthandledetialEntity.SignDeptName;
                tempdetail.signdeptid = powerplanthandledetialEntity.SignDeptId;
                tempdetail.signpersonname = powerplanthandledetialEntity.SignPersonName;
                tempdetail.signpersonid = powerplanthandledetialEntity.SignPersonId;
                tempdetail.isassignperson = powerplanthandledetialEntity.IsAssignPerson;
                tempdetail.applystate = powerplanthandledetialEntity.ApplyState.ToString();
                tempdetail.realsignpersonid = powerplanthandledetialEntity.RealSignPersonId;
                tempdetail.realsignpersonname = powerplanthandledetialEntity.RealSignPersonName;
                powerplanthandledetial.Add(tempdetail);
                model.powerplanthandledetial = powerplanthandledetial;
                #endregion

                #region 事故事件处理基本信息
                powerplanthandle powerplanthandle = new powerplanthandle();
                var powerplanthandleEntity = powerplanthandlebll.GetEntity(powerplanthandledetialEntity.PowerPlantHandleId);
                if (powerplanthandleEntity == null)
                {
                    throw new ArgumentException("未找到信息");
                }
                if (powerplanthandleEntity.AccidentEventType != null)
                    powerplanthandleEntity.AccidentEventType = dataitemdetailbll.GetItemName("AccidentEventType", powerplanthandleEntity.AccidentEventType);
                if (powerplanthandleEntity.AccidentEventProperty != null)
                    powerplanthandleEntity.AccidentEventProperty = dataitemdetailbll.GetItemName("AccidentEventProperty", powerplanthandleEntity.AccidentEventProperty);
                string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                List<FileInfoEntity> file = fileinfobll.GetFileList(powerplanthandledetialEntity.PowerPlantHandleId);
                IList<ERCHTMS.Entity.PowerPlantInside.Photo> fileobjects = new List<ERCHTMS.Entity.PowerPlantInside.Photo>();
                foreach (var item in file)
                {
                    ERCHTMS.Entity.PowerPlantInside.Photo temp = new ERCHTMS.Entity.PowerPlantInside.Photo();
                    temp.fileid = item.FileId;
                    temp.filename = item.FileName;
                    temp.fileurl = webUrl + item.FilePath.Replace("~", "");
                    fileobjects.Add(temp);
                }
                IList<AptitudeinvestigateauditEntity> audits = aptitudeinvestigateauditbll.GetAuditList(powerplanthandledetialEntity.PowerPlantHandleId).ToList();

                foreach (var item in audits)
                {
                    item.AUDITSIGNIMG = string.IsNullOrWhiteSpace(item.AUDITSIGNIMG) ? "" : webUrl + item.AUDITSIGNIMG;
                }

                powerplanthandle.id = powerplanthandleEntity.Id;
                powerplanthandle.accidenteventname = powerplanthandleEntity.AccidentEventName;
                powerplanthandle.belongdept = powerplanthandleEntity.BelongDept;
                powerplanthandle.accidenteventtype = powerplanthandleEntity.AccidentEventType;
                powerplanthandle.accidenteventproperty = powerplanthandleEntity.AccidentEventProperty;
                powerplanthandle.happentime = powerplanthandleEntity.HappenTime;
                powerplanthandle.situationintroduction = powerplanthandleEntity.SituationIntroduction;
                powerplanthandle.reasonandproblem = powerplanthandleEntity.ReasonAndProblem;
                powerplanthandle.filelist = fileobjects;
                powerplanthandle.powerplanthandleaudits = audits;
                model.powerplanthandle = powerplanthandle;
                #endregion

                #region 事故事件整改信息
                IList<PowerplantreformEntity> reformlist = powerplantreformbll.GetList("").Where(t => t.PowerPlantHandleDetailId == dy.data.id && t.Disable == 0).ToList();
                if (reformlist.Count > 0)
                { 
                    reformlist[0].RectificationPersonSignImg = string.IsNullOrWhiteSpace(reformlist[0].RectificationPersonSignImg) ? "" : webUrl + reformlist[0].RectificationPersonSignImg;
                    List<FileInfoEntity> tempfile = fileinfobll.GetFileList(reformlist[0].Id);
                    IList<ERCHTMS.Entity.PowerPlantInside.Photo> tempfileobjects = new List<ERCHTMS.Entity.PowerPlantInside.Photo>();
                    foreach (var k in tempfile)
                    {
                        ERCHTMS.Entity.PowerPlantInside.Photo temp = new ERCHTMS.Entity.PowerPlantInside.Photo();
                        temp.fileid = k.FileId;
                        temp.filename = k.FileName;
                        temp.fileurl = webUrl + k.FilePath.Replace("~", "");
                        tempfileobjects.Add(temp);
                    }
                    reformlist[0].filelist = tempfileobjects;
                    model.powerplanthandlereform = reformlist[0];
                }
                #endregion

                #region 事故事件验收信息
                IList<PowerplantcheckEntity> checklist = powerplantcheckbll.GetList("").Where(t => t.PowerPlantHandleDetailId == dy.data.id && t.Disable == 0).ToList();
                foreach (var item in checklist)
                {
                    item.AuditSignImg = string.IsNullOrWhiteSpace(item.AuditSignImg) ? "" : webUrl + item.AuditSignImg;
                    List<FileInfoEntity> tempfile = fileinfobll.GetFileList(item.Id);
                    IList<ERCHTMS.Entity.PowerPlantInside.Photo> tempfileobjects = new List<ERCHTMS.Entity.PowerPlantInside.Photo>();
                    foreach (var k in tempfile)
                    {
                        ERCHTMS.Entity.PowerPlantInside.Photo temp = new ERCHTMS.Entity.PowerPlantInside.Photo();
                        temp.fileid = k.FileId;
                        temp.filename = k.FileName;
                        temp.fileurl = webUrl + k.FilePath.Replace("~", "");
                        tempfileobjects.Add(temp);
                    }
                    item.filelist = tempfileobjects;

                }
                model.powerplanthandlecheck = checklist;
                #endregion

                //流程信息
                var nodelist = powerplanthandlebll.GetAppFullFlowList(dy.data.id);
                model.checkflow = nodelist;
                Dictionary<string, string> dict_props = new Dictionary<string, string>();

                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                    DateFormatString = "yyyy-MM-dd HH:mm", //格式化日期
                    //NullValueHandling = NullValueHandling.Ignore 值为空则在JSON中体现
                };
                return new { code = 0, info = "获取数据成功", data = JObject.Parse(JsonConvert.SerializeObject(model, Formatting.None, settings)) };
            }
            catch (Exception ex)
            {
                return new { code = -1, data = "", info = ex.Message };
            }
        }
        #endregion

        #region 审核事故事件
        /// <summary>
        /// 审核事故事件
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object ApporveForm([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                OperatorProvider.AppUserId = dy.userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();

                string state = string.Empty;

                string moduleName = "(事故事件处理记录)审核";

                PowerplanthandleEntity entity = powerplanthandlebll.GetEntity(dy.data.keyvalue);
                /// <param name="currUser">当前登录人</param>
                /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
                /// <param name="moduleName">模块名称</param>
                /// <param name="createdeptid">创建人部门ID</param>
                ManyPowerCheckEntity mpcEntity = powerplanthandlebll.CheckAuditPower(curUser, out state, moduleName, new DepartmentBLL().GetEntityByCode(entity.CreateUserDeptCode).DepartmentId);

                string aptitudentity = JsonConvert.SerializeObject(dy.data.aptitudentity);
                AptitudeinvestigateauditEntity aentity = JsonConvert.DeserializeObject<AptitudeinvestigateauditEntity>(aptitudentity);
                #region //审核信息表
                AptitudeinvestigateauditEntity aidEntity = new AptitudeinvestigateauditEntity();
                aidEntity.AUDITRESULT = aentity.AUDITRESULT; //通过
                aidEntity.AUDITTIME = Convert.ToDateTime(aentity.AUDITTIME.Value.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss")); //审核时间
                aidEntity.AUDITPEOPLE = aentity.AUDITPEOPLE;  //审核人员姓名
                aidEntity.AUDITPEOPLEID = aentity.AUDITPEOPLEID;//审核人员id
                aidEntity.APTITUDEID = dy.data.keyvalue;  //关联的业务ID 
                aidEntity.AUDITDEPTID = aentity.AUDITDEPTID;//审核部门id
                aidEntity.AUDITDEPT = aentity.AUDITDEPT; //审核部门
                aidEntity.AUDITOPINION = aentity.AUDITOPINION; //审核意见
                aidEntity.FlowId = entity.FlowId;
                aidEntity.Disable = "0";
                string strurl = new DataItemDetailBLL().GetItemValue("imgUrl");
                aidEntity.AUDITSIGNIMG = string.IsNullOrWhiteSpace(aentity.AUDITSIGNIMG) ? "" : aentity.AUDITSIGNIMG.ToString().Replace(strurl, "");
                if (null != mpcEntity)
                {
                    aidEntity.REMARK = (mpcEntity.AUTOID.Value - 1).ToString(); //备注 存流程的顺序号
                }
                else
                {
                    aidEntity.REMARK = "7";
                }
                aptitudeinvestigateauditbll.SaveForm(aidEntity.ID, aidEntity);
                #endregion

                #region  //保存事故事件处理记录
                //审核通过
                if (aentity.AUDITRESULT == "0")
                {
                    //0表示流程未完成，1表示流程结束
                    if (null != mpcEntity)
                    {
                        entity.FlowDept = mpcEntity.CHECKDEPTID;
                        entity.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                        entity.FlowRole = mpcEntity.CHECKROLEID;
                        entity.FlowRoleName = mpcEntity.CHECKROLENAME;
                        entity.IsSaved = 1;
                        entity.ApplyState = 1;
                        entity.FlowId = mpcEntity.ID;
                        entity.FlowName = mpcEntity.CHECKDEPTNAME + "审核中";
                    }
                    else
                    {
                        entity.FlowDept = "";
                        entity.FlowDeptName = "";
                        entity.FlowRole = "";
                        entity.FlowRoleName = "";
                        entity.IsSaved = 1;
                        entity.ApplyState = 3;
                        entity.FlowName = "";
                        entity.FlowId = "";
                        //更新事故事件处理信息状态
                        IList<PowerplanthandledetailEntity> HandleDetailList = powerplanthandledetailbll.GetHandleDetailList(dy.data.keyvalue);
                        foreach (var item in HandleDetailList)
                        {
                            if (item.IsAssignPerson == "0")
                            {
                                item.ApplyState = 3;
                            }
                            else if (item.IsAssignPerson == "1")
                            {
                                item.ApplyState = 6;
                            }
                            
                            powerplanthandledetailbll.SaveForm(item.Id, item);
                        }
                    }
                }
                else //审核不通过 
                {
                    entity.FlowDept = "";
                    entity.FlowDeptName = "";
                    entity.FlowRole = "";
                    entity.FlowRoleName = "";
                    entity.ApplyState = 0; //处于登记阶段
                    entity.IsSaved = 0; //是否完成状态赋值为未完成
                    entity.FlowName = "";
                    entity.FlowId = "";
                    //更新事故事件处理信息状态
                    IList<PowerplanthandledetailEntity> HandleDetailList = powerplanthandledetailbll.GetHandleDetailList(dy.data.keyvalue);
                    foreach (var item in HandleDetailList)
                    {
                        item.ApplyState = 0;
                        powerplanthandledetailbll.SaveForm(item.Id, item);
                    }

                }
                //更新事故事件基本状态信息
                powerplanthandlebll.SaveForm(dy.data.keyvalue, entity);
                #endregion

                #region    //审核不通过
                if (aentity.AUDITRESULT == "1")
                {

                    //获取当前业务对象的所有审核记录
                    var shlist = aptitudeinvestigateauditbll.GetAuditList(dy.data.keyvalue);
                    //批量更新审核记录关联ID
                    foreach (AptitudeinvestigateauditEntity mode in shlist)
                    {
                        mode.Disable = "1";
                        aptitudeinvestigateauditbll.SaveForm(mode.ID, mode);
                    }
                }
                #endregion
                return new { code = 0, info = "操作成功", data = "" };
            }
            catch (Exception ex)
            {
                return new { code = -1, data = "", info = ex.Message };
            }
        }
        #endregion

        #region 签收事故事件
        /// <summary>
        /// 签收事故事件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object SignForm([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                OperatorProvider.AppUserId = dy.userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                string keyValue = dy.data.keyvalue;
                string handledetailentity = JsonConvert.SerializeObject(dy.data.handledetailentity);
                PowerplanthandledetailEntity entity = JsonConvert.DeserializeObject<PowerplanthandledetailEntity>(handledetailentity);
                entity.RealSignPersonName = curUser.UserName;
                entity.RealSignPersonId = curUser.UserId;
                entity.RealSignDate =Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                entity.RealSignPersonDept = curUser.DeptName;
                entity.RealSignPersonDeptId = curUser.DeptId;
                entity.RealSignPersonDeptCode = curUser.DeptCode;
                entity.ApplyState = 3;
                powerplanthandledetailbll.SaveForm(keyValue, entity);
                powerplanthandlebll.UpdateApplyStatus(entity.PowerPlantHandleId);
                return new { code = 0, info = "操作成功", data = "" };
            }
            catch (Exception ex)
            {
                return new { code = -1, data = "", info = ex.Message };
            }
        }
        #endregion

        #region 整改事故事件
        /// <summary>
        /// 整改事故事件
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object ReformForm()
        {
            try
            {
                string res = HttpContext.Current.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                OperatorProvider.AppUserId = dy.userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();

                string reformentity = JsonConvert.SerializeObject(dy.data.reformentity);
                PowerplantreformEntity entity = JsonConvert.DeserializeObject<PowerplantreformEntity>(reformentity);

                PowerplanthandledetailEntity powerplanthandledetailentity = powerplanthandledetailbll.GetEntity(entity.PowerPlantHandleDetailId);
                if (powerplanthandledetailentity != null)
                {
                    powerplanthandledetailentity.RealReformDept = curUser.DeptName;
                    powerplanthandledetailentity.RealReformDeptId = curUser.DeptId;
                    powerplanthandledetailentity.RealReformDeptCode = curUser.DeptCode;
                    string state = string.Empty;

                    string moduleName = "事故事件处理记录-验收";

                    /// <param name="currUser">当前登录人</param>
                    /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
                    /// <param name="moduleName">模块名称</param>
                    /// <param name="outengineerid">工程Id</param>
                    ManyPowerCheckEntity mpcEntity = powerplanthandlebll.CheckAuditPower(curUser, out state, moduleName, curUser.DeptId);

                    string flowid = string.Empty;
                    List<ManyPowerCheckEntity> powerList = new ManyPowerCheckBLL().GetListBySerialNum(curUser.OrganizeCode, moduleName);
                    List<ManyPowerCheckEntity> checkPower = new List<ManyPowerCheckEntity>();
                    foreach (var item in powerList)
                    {
                        if (item.CHECKDEPTID == "-3" || item.CHECKDEPTID == "-1")
                        {
                            item.CHECKDEPTID = curUser.DeptId;
                            item.CHECKDEPTCODE = curUser.DeptCode;
                            item.CHECKDEPTNAME = curUser.DeptName;
                        }
                    }
                    for (int i = 0; i < powerList.Count; i++)
                    {
                        if (powerList[i].CHECKDEPTID == curUser.DeptId)
                        {
                            var rolelist = curUser.RoleName.Split(',');
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
                    if (checkPower.Count > 0)
                    {
                        ManyPowerCheckEntity check = checkPower.Last();//当前

                        for (int i = 0; i < powerList.Count; i++)
                        {
                            if (check.ID == powerList[i].ID)
                            {
                                flowid = powerList[i].ID;
                            }
                        }
                    }
                    if (null != mpcEntity)
                    {
                        powerplanthandledetailentity.FlowDept = mpcEntity.CHECKDEPTID;
                        powerplanthandledetailentity.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                        powerplanthandledetailentity.FlowRole = mpcEntity.CHECKROLEID;
                        powerplanthandledetailentity.FlowRoleName = mpcEntity.CHECKROLENAME;
                        powerplanthandledetailentity.ApplyState = 4; //流程未完成，1表示完成
                        powerplanthandledetailentity.FlowId = mpcEntity.ID;
                        powerplanthandledetailentity.FlowName = mpcEntity.CHECKDEPTNAME + "验收中";
                    }
                    else  //为空则表示已经完成流程
                    {
                        powerplanthandledetailentity.FlowDept = "";
                        powerplanthandledetailentity.FlowDeptName = "";
                        powerplanthandledetailentity.FlowRole = "";
                        powerplanthandledetailentity.FlowRoleName = "";
                        powerplanthandledetailentity.ApplyState = 5; //流程未完成，1表示完成
                        powerplanthandledetailentity.FlowName = "";
                        powerplanthandledetailentity.FlowId = "";
                    }
                    string keyValue = Guid.NewGuid().ToString();
                    entity.RectificationPerson = curUser.UserName;
                    entity.RectificationPersonId = curUser.UserId;
                    entity.Disable = 0;
                    string strurl = new DataItemDetailBLL().GetItemValue("imgUrl");
                    entity.RectificationPersonSignImg = string.IsNullOrWhiteSpace(entity.RectificationPersonSignImg) ? "" : entity.RectificationPersonSignImg.Replace(strurl, "");
                    HttpFileCollection files = HttpContext.Current.Request.Files;//上传的文件 
                    UploadifyFile(keyValue, files);
                    powerplantreformbll.SaveForm(keyValue, entity);
                    powerplanthandledetailbll.SaveForm(powerplanthandledetailentity.Id, powerplanthandledetailentity);
                    powerplanthandlebll.UpdateApplyStatus(entity.PowerPlantHandleId);

                    //添加验收信息
                    if (state == "1")
                    {
                        //验收信息
                        PowerplantcheckEntity checkEntity = new PowerplantcheckEntity();
                        checkEntity.AuditResult = 0; //通过
                        checkEntity.AuditTime = DateTime.Now;
                        checkEntity.AuditPeople = curUser.UserName;
                        checkEntity.AuditPeopleId = curUser.UserId;
                        checkEntity.PowerPlantHandleId = entity.PowerPlantHandleId;
                        checkEntity.PowerPlantHandleDetailId = entity.PowerPlantHandleDetailId;
                        checkEntity.PowerPlantReformId = keyValue;
                        checkEntity.AuditOpinion = ""; //审核意见
                        checkEntity.AuditSignImg = string.IsNullOrWhiteSpace(entity.RectificationPersonSignImg) ? "" : entity.RectificationPersonSignImg.Replace(strurl, "");
                        checkEntity.FlowId = flowid;
                        if (null != mpcEntity)
                        {
                            checkEntity.Remark = (mpcEntity.AUTOID.Value - 1).ToString(); //备注 存流程的顺序号
                        }
                        else
                        {
                            checkEntity.Remark = "7";
                        }
                        checkEntity.AuditDeptId = curUser.DeptId;
                        checkEntity.AuditDept = curUser.DeptName;
                        checkEntity.Disable = 0;
                        powerplantcheckbll.SaveForm(checkEntity.Id, checkEntity);
                    }
                    powerplantreformbll.SaveForm(keyValue, entity);
                    
                }
                else
                {
                    return new { code = -1, data = "", info = "系统错误,请联系系统管理员" };
                }
                return new { code = 0, info = "操作成功", data = "" };
            }
            catch (Exception ex)
            {
                return new { code = -1, data = "", info = ex.Message };
            }
        }
        #endregion

        #region 验收事故事件
        /// <summary>
        /// 验收事故事件
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object CheckForm()
        {
            try
            {
                string res = HttpContext.Current.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                OperatorProvider.AppUserId = dy.userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();

                string checkentity = JsonConvert.SerializeObject(dy.data.checkentity);
                PowerplantcheckEntity aentity = JsonConvert.DeserializeObject<PowerplantcheckEntity>(checkentity);

                string state = string.Empty;

                string moduleName = "事故事件处理记录-验收";

                PowerplanthandledetailEntity entity = powerplanthandledetailbll.GetEntity(aentity.PowerPlantHandleDetailId);
                /// <param name="currUser">当前登录人</param>
                /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
                /// <param name="moduleName">模块名称</param>
                /// <param name="createdeptid">创建人部门ID</param>
                ManyPowerCheckEntity mpcEntity = powerplanthandlebll.CheckAuditPower(curUser, out state, moduleName, entity.RealReformDeptId);


                #region //审核信息表
                PowerplantcheckEntity aidEntity = new PowerplantcheckEntity();
                aidEntity.AuditResult = aentity.AuditResult; //通过
                aidEntity.AuditTime = Convert.ToDateTime(aentity.AuditTime.Value.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss")); //审核时间
                aidEntity.AuditPeople = aentity.AuditPeople;  //审核人员姓名
                aidEntity.AuditPeopleId = aentity.AuditPeopleId;//审核人员id
                aidEntity.AuditDeptId = aentity.AuditDeptId;//审核部门id
                aidEntity.AuditDept = aentity.AuditDept; //审核部门
                aidEntity.AuditOpinion = aentity.AuditOpinion; //审核意见
                aidEntity.FlowId = entity.FlowId;
                string strurl = new DataItemDetailBLL().GetItemValue("imgUrl");
                aidEntity.AuditSignImg = string.IsNullOrWhiteSpace(aentity.AuditSignImg) ? "" : aentity.AuditSignImg.ToString().Replace(strurl, "");
                aidEntity.PowerPlantHandleDetailId = aentity.PowerPlantHandleDetailId;
                aidEntity.PowerPlantHandleId = aentity.PowerPlantHandleId;
                aidEntity.PowerPlantReformId = aentity.PowerPlantReformId;
                aidEntity.AuditDeptId = curUser.DeptId;
                aidEntity.AuditDept = curUser.DeptName;
                aidEntity.Disable = 0;
                if (null != mpcEntity)
                {
                    aidEntity.Remark = (mpcEntity.AUTOID.Value - 1).ToString(); //备注 存流程的顺序号
                }
                else
                {
                    aidEntity.Remark = "7";
                }
                string keyValue = Guid.NewGuid().ToString();
                HttpFileCollection files = HttpContext.Current.Request.Files;//上传的文件 
                UploadifyFile(keyValue, files);
                powerplantcheckbll.SaveForm(keyValue, aidEntity);
                #endregion

                #region  //保存事故事件处理记录
                //审核通过
                if (aentity.AuditResult == 0)
                {
                    //0表示流程未完成，1表示流程结束
                    if (null != mpcEntity)
                    {
                        entity.FlowDept = mpcEntity.CHECKDEPTID;
                        entity.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                        entity.FlowRole = mpcEntity.CHECKROLEID;
                        entity.FlowRoleName = mpcEntity.CHECKROLENAME;
                        entity.ApplyState = 4;
                        entity.FlowId = mpcEntity.ID;
                        entity.FlowName = mpcEntity.CHECKDEPTNAME + "验收中";
                    }
                    else
                    {
                        entity.FlowDept = "";
                        entity.FlowDeptName = "";
                        entity.FlowRole = "";
                        entity.FlowRoleName = "";
                        entity.ApplyState = 5;
                        entity.FlowName = "";
                        entity.FlowId = "";
                    }
                }
                else //验收不通过 
                {
                    entity.FlowDept = "";
                    entity.FlowDeptName = "";
                    entity.FlowRole = "";
                    entity.FlowRoleName = "";
                    entity.ApplyState = 3; //退回到整改状态
                    entity.FlowName = "";
                    entity.FlowId = "";
                    entity.RealReformDept = "";
                    entity.RealReformDeptCode = "";
                    entity.RealReformDeptId = "";

                }
                //更新事故事件处理信息
                powerplanthandledetailbll.SaveForm(entity.Id, entity);
                powerplanthandlebll.UpdateApplyStatus(entity.PowerPlantHandleId);
                #endregion

                #region    //审核不通过
                if (aentity.AuditResult == 1)
                {
                    var reformlist = powerplantreformbll.GetList("").Where(t => t.PowerPlantHandleDetailId == aentity.PowerPlantHandleDetailId && t.Disable == 0).ToList(); //整改信息
                    var checklist = powerplantcheckbll.GetList("").Where(t => t.PowerPlantHandleDetailId == aentity.PowerPlantHandleDetailId && t.Disable == 0).ToList(); //验收信息
                    foreach (var item in reformlist)
                    {
                        item.Disable = 1; //将整改信息设置失效
                        powerplantreformbll.SaveForm(item.Id, item);
                    }
                    foreach (var item in checklist)
                    {
                        item.Disable = 1; //将验收信息设置失效
                        powerplantcheckbll.SaveForm(item.Id, item);
                    }
                }
                #endregion
                return new { code = 0, info = "操作成功", data = "" };
            }
            catch (Exception ex)
            {
                return new { code = -1, data = "", info = ex.Message };
            }
        }
        #endregion

        #region 获取事故事件审核历史记录
        /// <summary>
        /// 获取事故事件审核历史记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetHistoryPowerHandleList([FromBody]JObject json)
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
                string keyvalue = dy.data.keyvalue;
                var data = aptitudeinvestigateauditbll.GetAuditList(keyvalue).Where(t => t.Disable == "1").OrderByDescending(x => x.AUDITTIME).ToList();
                string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                foreach (var item in data)
                {
                    item.AUDITSIGNIMG = string.IsNullOrWhiteSpace(item.AUDITSIGNIMG) ? "" : webUrl + item.AUDITSIGNIMG;
                }
                Dictionary<string, string> dict_props = new Dictionary<string, string>();

                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                    DateFormatString = "yyyy-MM-dd HH:mm", //格式化日期
                    NullValueHandling = NullValueHandling.Ignore //值为空则在JSON中体现
                };
                return new { code = 0, info = "获取数据成功", data = JArray.Parse(JsonConvert.SerializeObject(data, Formatting.None, settings)) };
                //return new { code = 0, info = "获取数据成功", count = data.Count, data = data };
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }
        #endregion

        #region 获取事故事件整改历史记录
        /// <summary>
        /// 获取事故事件整改历史记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetHistoryPowerHandleReformList([FromBody]JObject json)
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
                string keyvalue = dy.data.keyvalue;
                string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                IList<PowerplantreformEntity> reformlist = powerplantreformbll.GetList("").Where(t => t.PowerPlantHandleDetailId == keyvalue && t.Disable == 1).ToList();
                foreach (var item in reformlist)
                {
                    item.RectificationPersonSignImg = string.IsNullOrWhiteSpace(item.RectificationPersonSignImg) ? "" : webUrl + item.RectificationPersonSignImg;
                    List<FileInfoEntity> tempfile = fileinfobll.GetFileList(item.Id);
                    IList<ERCHTMS.Entity.PowerPlantInside.Photo> tempfileobjects = new List<ERCHTMS.Entity.PowerPlantInside.Photo>();
                    foreach (var k in tempfile)
                    {
                        ERCHTMS.Entity.PowerPlantInside.Photo temp = new ERCHTMS.Entity.PowerPlantInside.Photo();
                        temp.fileid = k.FileId;
                        temp.filename = k.FileName;
                        temp.fileurl = webUrl + k.FilePath.Replace("~", "");
                        tempfileobjects.Add(temp);
                    }
                    item.filelist = tempfileobjects;
                }
                Dictionary<string, string> dict_props = new Dictionary<string, string>();

                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                    DateFormatString = "yyyy-MM-dd HH:mm", //格式化日期
                    NullValueHandling = NullValueHandling.Ignore //值为空则在JSON中体现
                };
                return new { code = 0, info = "获取数据成功", count = reformlist.Count, data = JArray.Parse(JsonConvert.SerializeObject(reformlist, Formatting.None, settings)) };
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }
        #endregion

        #region 获取事故事件验收历史记录
        /// <summary>
        /// 获取事故事件验收历史记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetHistoryPowerHandleCheckList([FromBody]JObject json)
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
                string keyvalue = dy.data.keyvalue;
                string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                IList<PowerplantcheckEntity> checklist = powerplantcheckbll.GetList("").Where(t => t.PowerPlantHandleDetailId == keyvalue && t.Disable == 1).ToList();
                foreach (var item in checklist)
                {
                    List<FileInfoEntity> tempfile = fileinfobll.GetFileList(item.Id);
                    IList<ERCHTMS.Entity.PowerPlantInside.Photo> tempfileobjects = new List<ERCHTMS.Entity.PowerPlantInside.Photo>();
                    foreach (var k in tempfile)
                    {
                        ERCHTMS.Entity.PowerPlantInside.Photo temp = new ERCHTMS.Entity.PowerPlantInside.Photo();
                        temp.fileid = k.FileId;
                        temp.filename = k.FileName;
                        temp.fileurl = webUrl + k.FilePath.Replace("~", "");
                        tempfileobjects.Add(temp);
                    }
                    item.filelist = tempfileobjects;
                    item.AuditSignImg = string.IsNullOrWhiteSpace(item.AuditSignImg) ? "" : webUrl + item.AuditSignImg;
                }
                Dictionary<string, string> dict_props = new Dictionary<string, string>();

                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                    DateFormatString = "yyyy-MM-dd HH:mm", //格式化日期
                    NullValueHandling = NullValueHandling.Ignore //值为空则在JSON中体现
                };
                return new { code = 0, info = "获取数据成功", count = checklist.Count, data = JArray.Parse(JsonConvert.SerializeObject(checklist, Formatting.None, settings)) };
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }
        #endregion

        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="foldername"></param>
        /// <param name="fileList"></param>
        public void UploadifyFile(string folderId, HttpFileCollection fileList)
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
                        string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                        string dir = new DataItemDetailBLL().GetItemValue("imgPath") + "\\Resource\\ht\\images\\" + uploadDate;
                        string newFileName = fileGuid + FileEextension;
                        string newFilePath = dir + "\\" + newFileName;
                        //创建文件夹
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
                            fileInfoEntity.FolderId = "ht/images";
                            fileInfoEntity.RecId = folderId; //关联ID
                            fileInfoEntity.FileName = file.FileName;
                            fileInfoEntity.FilePath = "~/Resource/ht/images/" + uploadDate + '/' + newFileName;
                            fileInfoEntity.FileSize = (Math.Round(decimal.Parse(filesize.ToString()) / decimal.Parse("1024"), 2)).ToString();//文件大小（kb）
                            fileInfoEntity.FileExtensions = FileEextension;
                            fileInfoEntity.FileType = FileEextension.Replace(".", "");
                            fileinfobll.SaveForm("", fileInfoEntity);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }



    public class PowerPlantHandleModel
    {
        public powerplanthandle powerplanthandle { get; set; }
        public IList<powerplanthandledetial> powerplanthandledetial { get; set; }
        public PowerplantreformEntity powerplanthandlereform { get; set; }
        public IList<PowerplantcheckEntity> powerplanthandlecheck { get; set; }

        public List<CheckFlowData> checkflow { get; set; }
    }

    //事故事件处理基本信息
    public class powerplanthandle {
        public string id { get; set; } //主键
        public string accidenteventname { get; set; } //事故事件名称
        public string belongdept { get; set; } //所属部门
        public string accidenteventtype { get; set; } //事故事件类型
        public string accidenteventproperty { get; set; } //事故事件性质
        public DateTime? happentime { get; set; } //发生时间
        public string situationintroduction { get; set; } //情况简介
        public string reasonandproblem { get; set; } //原因及存在问题
        public IList<ERCHTMS.Entity.PowerPlantInside.Photo> filelist { get; set; } //附件
        public IList<AptitudeinvestigateauditEntity> powerplanthandleaudits { get; set; } //审核记录
    }

    //事故事件处理信息
    public class powerplanthandledetial
    {
        public string id { get; set; } //主键
        public string reasonandproblem { get; set; }  //原因及暴露问题
        public string rectificationmeasures { get; set; } //整改(防范措施)措施
        public string rectificationdutyperson { get; set; } //整改责任人
        public string rectificationdutydept { get; set; } //整改责任部门
        public DateTime? rectificationtime { get; set; } //整改期限
        public string signdeptname { get; set; }  //签收部门
        public string signdeptid { get; set; }  //签收部门ID
        public string signpersonname { get; set; }  //签收人
        public string signpersonid { get; set; }  //签收人ID
        public string isassignperson { get; set; }  //是否指定责任人(0：是     1：否)
        public string approvenames { get; set; } //当前步骤操作人
        public string applystate { get; set; } //状态(0.申请中,1.审核中,2.审核不通过,3.整改中,4.验收中,5.已完成,6.签收中)
        public string happentime { get; set; } //发生时间
        public string belongdept { get; set; } //所属部门
        public string accidenteventname { get; set; }  //事故事件名称
        public string realreformdept { get; set; } //实际整改部门(验收中状态使用该字段显示整改部门)
        public string realsignpersonname { get; set; } //实际签收人
        public string realsignpersonid { get; set; } //实际签收人ID
    }

}
