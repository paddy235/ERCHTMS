using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ERCHTMS.Busines.BaseManage;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Dynamic;
using ERCHTMS.Busines.RiskDatabase;
using ERCHTMS.Entity.RiskDatabase;
using System.Data;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.NosaManage;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Busines.JPush;
using ERCHTMS.Busines.AuthorizeManage;
using System.Web;
using ERCHTMS.Busines.RoutineSafetyWork;
using System.Text;
using BSFramework.Util;
using ERCHTMS.Cache;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.AppSerivce.Model;
using System.Collections;
using ERCHTMS.Busines.HighRiskWork;
using ERCHTMS.Busines.EmergencyPlatform;
using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.Entity.PersonManage;
using ERCHTMS.Busines.PersonManage;
using System.IO;

namespace ERCHTMS.AppSerivce.Controllers
{
    public class BaseDataController : ApiController
    {
        DistrictBLL districtbll = new DistrictBLL();
        RiskAssessBLL riskassessbll = new RiskAssessBLL();
        MeasuresBLL measuresbll = new MeasuresBLL();
        ERCHTMS.Busines.SystemManage.DataItemDetailBLL diBll = new Busines.SystemManage.DataItemDetailBLL();
        WeatherBLL weatherbll = new WeatherBLL();
        UserBLL userbll = new UserBLL();
        private NosaWorkSummaryManagerBLL worksummary = new NosaWorkSummaryManagerBLL();
        private HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL();
        private DrillplanBLL drillplanbll = new DrillplanBLL();
        private DrillplanrecordBLL drillplanrecordbll = new DrillplanrecordBLL();
        private DepartmentBLL departmentbll = new DepartmentBLL();
        public HttpContext ctx { get { return HttpContext.Current; } }

        #region 获取模块流程配置文件

        [HttpPost]
        /// <summary>
        ///获取模块流程配置文件
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public object getFlowFile([FromBody] JObject json)
        {
            try
            {
                DepartmentBLL deptBll = new DepartmentBLL();
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string recId = res.Contains("recId") ? dy.data.recId : "";
                //获取用户基本信息
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                if (user == null)
                {
                    return new { code = -1, info = "用户凭据失效，请重新登陆！" };
                }
                //隐患
                if (recId == "0")
                {
                    recId = "yh" + user.OrganizeId;
                }
                else
                {
                    recId =  user.OrganizeId + recId;
                }
                var strSql = new StringBuilder();
                string path = new DataItemDetailBLL().GetItemValue("imgUrl");
                strSql.AppendFormat(@"SELECT filename,'" + path + "/content/SecurityDynamics/Preview.html~$keyValue={0}!type=0' url FROM  Base_FileInfo where DeleteMark = 0 AND recId='{0}'", recId);
                DataTable data = deptBll.GetDataTable(strSql.ToString());
                foreach (DataRow dr in data.Rows)
                {
                    if (dr["url"] != null)
                    {
                        dr["url"] = dr["url"].ToString().Replace("~$", "?").Replace("!", "&");
                    }
                }
                return new { code = 0, info = "获取数据成功", data = data, count = data.Rows.Count };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message };
            }
        }
        #endregion

        #region 获取未整改的隐患及违章信息
        /// <summary>
        ///获取未整改的隐患及违章信息
        /// </summary>
        /// <param name="code">电厂编码</param>
        /// <returns></returns>
        [HttpPost]
        public object getHtAndLllegalInfoForHome(string code = "001")
        {
            try
            {
                var htdata = htbaseinfobll.GetAppHidStatistics(code, 6);//未整改的隐患信息
                var yqhtdata = htbaseinfobll.GetAppHidStatistics(code, 7);//逾期未整改的隐患
                var lllegaldata = new DataTable();//未整改的违章
                var yqlllegaldata = new DataTable();//逾期未整改的违章
                //西塞山
                if (code == "007")
                {
                    lllegaldata = htbaseinfobll.GetAppHidStatistics(code, 10);//未整改的违章
                    yqlllegaldata = htbaseinfobll.GetAppHidStatistics(code, 11);//逾期未整改的违章
                }
                else
                { //通用版本
                    lllegaldata = htbaseinfobll.GetAppHidStatistics(code, 8);
                    yqlllegaldata = htbaseinfobll.GetAppHidStatistics(code, 9);
                }
                return new
                {
                    Code = 0,
                    Count = 0,
                    Info = "获取数据成功",
                    data = new { wzghtdata = htdata, wzghtcount = htdata.Rows.Count.ToString(), yqwzghtcount = yqhtdata.Rows.Count.ToString(), wzglllegaldata = lllegaldata, wzglllegalcount = lllegaldata.Rows.Count.ToString(), yqwzglllegalcount = yqlllegaldata.Rows.Count.ToString() }
                };

            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
                throw;
            }
        }
        #endregion

        #region 获取用户对模块的数据和操作权限

        /// <summary>
        /// 获取用户对模块的数据和操作权限
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public object getUserPermission(string userId, string moduleId)
        {
            AuthorizeBLL authBll = new AuthorizeBLL();
            UserEntity user = userbll.GetEntity(userId);
            if (user == null)
            {
                return new { code = -1, message = "当前用户信息不存在" };
            }
            else
            {
                Operator curUser = new Operator
                {
                    UserId = userId,
                    IsSystem = user.Account == "System" ? true : false
                };
                string dataScope = authBll.GetDataAuthority(curUser, moduleId, "");
                string operScope = authBll.GetOperAuthority(curUser, moduleId, "");
                return new { code = 0, message = "获取数据成功", data = new { dataScope = dataScope, operScope = operScope } };
            }

        }
        #endregion

        #region 分页获取通知公告列表(国电汉川大屏展示)
        /// <summary>
        /// 分页获取通知公告列表(国电汉川大屏展示)
        /// </summary>
        /// <param name="code">机构编码</param>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns></returns>
        [HttpGet]
        public object GetNoticeList(string code = "001", int pageIndex = 1, int pageSize = 20)
        {
            try
            {

                Pagination pagination = new Pagination();
                pagination.p_kid = "id";
                pagination.p_tablename = "BIS_ANNOUNCEMENT";
                pagination.conditionJson = string.Format("createuserorgcode='{0}' and PublisherDept in('生产技术部','安全监察部')", code);
                pagination.page = pageIndex;
                pagination.rows = pageSize;
                pagination.sidx = "ReleaseTime";
                pagination.sord = "desc";
                pagination.p_fields = "title,ReleaseTime,Publisher,Content";
                var list = new ERCHTMS.Busines.RoutineSafetyWork.AnnouncementBLL().GetPageList(pagination, "");
                Dictionary<string, string> dict_props = new Dictionary<string, string>();
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                    DateFormatString = "yyyy-MM-dd HH:mm" //格式化日期
                };
                settings.Converters.Add(new DecimalToStringConverter());
                return new { code = 0, message = "获取数据成功", count = pagination.records, data = JArray.Parse(JsonConvert.SerializeObject(list, Formatting.None, settings)) };
            }
            catch (Exception ex)
            {
                return new { code = -1, message = ex.Message };
            }

        }
        #endregion

        #region 根据编码名称获取编码值
        // GET api/<controller>/5
        public object Get(string name)
        {
            return diBll.GetItemValue(name);
        }
        [HttpGet]
        [HttpPost]
        public object GetCodeList([FromBody]JObject json)
        {
            try
            {
                string code, code1 = "", keyWord = "";
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                code = res.Contains("code") ? dy.data.code : "";
                code1 = res.Contains("code1") ? dy.data.code1 : "";
                keyWord = res.Contains("keyWord") ? dy.data.keyWord : "";
                if (string.IsNullOrWhiteSpace(code))
                {
                    return new { code = 1, info = "参数code不能为空！" };
                }
                string sql = string.Format("select t.ItemName,t.ItemValue from BASE_DATAITEMDETAIL t where t.enabledmark = 1 and t.deletemark = 0 ");
                if (!string.IsNullOrWhiteSpace(keyWord))
                {
                    sql += string.Format(" and ItemName like '%{0}%'", keyWord.Trim());
                }
                if (!string.IsNullOrWhiteSpace(code))
                {
                    sql += string.Format(" and t.itemid=(select itemid from base_dataitem a where a.itemcode='{0}') ", code);
                }
                if (!string.IsNullOrWhiteSpace(code1))
                {
                    sql += string.Format(" and itemcode='{0}'", code1);
                }

                sql += " order by sortcode asc";
                DataTable dt = new DepartmentBLL().GetDataTable(sql);
                return new { code = 0, info = "获取数据成功", data = dt };
            }
            catch (Exception ex)
            {
                return new { code = 1, info = ex.Message };
            }

        }

        #endregion

        #region 获取区域
        /// <summary>
        /// 获取工程类别
        /// </summary>
        /// <param name="encode">华润湖北（hrdl）,国电荥阳（gdxy）</param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public object getDistrictList(string encode)
        {
            try
            {
                encode = string.IsNullOrEmpty(diBll.GetItemValue(encode, "FactoryEncode")) ? "-100" : diBll.GetItemValue(encode, "FactoryEncode");
                var data = districtbll.GetList("").Where(t => t.ChargeDeptCode.StartsWith(encode));
                if (HttpContext.Current.Request.Params["jsoncallback"] != null)
                {
                    string result = Newtonsoft.Json.JsonConvert.SerializeObject(new { code = 0, message = "获取数据成功", data = data.Select(x => new { districtid = x.DistrictID, parentid = x.ParentID, districtname = x.DistrictName, districtcode = x.DistrictCode }) });
                    return HttpContext.Current.Request.Params["jsoncallback"] + "(" + result + ")";
                }
                else
                {
                    return new { code = 0, message = "获取数据成功", data = data.Select(x => new { districtid = x.DistrictID, parentid = x.ParentID, districtname = x.DistrictName, districtcode = x.DistrictCode }) };
                }

            }
            catch (Exception ex)
            {
                return new { code = -1, message = ex.Message };
            }
        }
        #endregion

        #region 获取safety作业票信息并根据数据设置首页动态风险图效果（华润湖北）
        /// <summary>
        /// 工作票查询接口(Safety提供)
        /// </summary>
        /// <param name="districtCode">区域code</param>
        /// <param name="workTime">作业时间</param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public object GetWorkTicketsData(string districtCode, string pageSize = "1000", string workTime = "")
        {

            try
            {
                //List<WorkTicketEntity> list = new List<WorkTicketEntity>() ;
                //list.Add(new WorkTicketEntity { 
                // RA_RA_FX_DJ="一级",
                // EQ_LO_USERDEF3 = "013001004001001003",
                // WO_SD_NO_P="11",
                // WO_SD_TY_Desc = "22",
                // WO_SD_DE = "33",
                // WO_SD_VF_DT = "44",
                // WO_SD_VT_DT = "55",
                // WO_SD_AP_ID_Name="66"
                //});
                //list.Add(new WorkTicketEntity
                //{
                //    RA_RA_FX_DJ = "一级",
                //    EQ_LO_USERDEF3 = "013001004001001001010",
                //    WO_SD_NO_P = "11",
                //    WO_SD_TY_Desc = "22",
                //    WO_SD_DE = "33",
                //    WO_SD_VF_DT = "44",
                //    WO_SD_VT_DT = "55",
                //    WO_SD_AP_ID_Name = "66"
                //});
                //list.Add(new WorkTicketEntity
                //{
                //    RA_RA_FX_DJ = "二级",
                //    EQ_LO_USERDEF3 = "013001004001001001010",
                //    WO_SD_NO_P = "11",
                //    WO_SD_TY_Desc = "22",
                //    WO_SD_DE = "33",
                //    WO_SD_VF_DT = "44",
                //    WO_SD_VT_DT = "55",
                //    WO_SD_AP_ID_Name = "66"
                //});
                //list.Add(new WorkTicketEntity
                //{
                //    RA_RA_FX_DJ = "三级",
                //    EQ_LO_USERDEF3 = "013001004001001001010",
                //    WO_SD_NO_P = "11",
                //    WO_SD_TY_Desc = "22",
                //    WO_SD_DE = "33",
                //    WO_SD_VF_DT = "44",
                //    WO_SD_VT_DT = "55",
                //    WO_SD_AP_ID_Name = "66"
                //});
                //list.Add(new WorkTicketEntity
                //{
                //    RA_RA_FX_DJ = "四级",
                //    EQ_LO_USERDEF3 = "013001004001001001010",
                //    WO_SD_NO_P = "11",
                //    WO_SD_TY_Desc = "22",
                //    WO_SD_DE = "33",
                //    WO_SD_VF_DT = "44",
                //    WO_SD_VT_DT = "55",
                //    WO_SD_AP_ID_Name = "66"
                //});
                //string data = "({\"code\":0,\"message\":\"success\",\"data\":[{\"RA_RA_FX_DJ\":\"三级\",\"EQ_LO_USERDEF3\":\"013001004001001001010\"},{\"RA_RA_FX_DJ\":\"二级\",\"EQ_LO_USERDEF3\":\"013001004001001001010\"}]})".TrimStart('(').TrimEnd(')');
                //dynamic dy = Newtonsoft.Json.JsonConvert.DeserializeObject<ExpandoObject>(data);
                //if(dy.code==0)
                //{
                //    List<object> objs = dy.data;
                //    if (objs.Count > 0)
                //    {
                //        data = Newtonsoft.Json.JsonConvert.SerializeObject(objs);
                //        List<WorkTicketEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WorkTicketEntity>>(data);
                //        string[] arr = districtCode.Split(',');
                //        DepartmentBLL deptBll = new DepartmentBLL();

                //        DataTable dt = new DataTable();
                //        dt.Columns.Add("code"); //区域图片对应编码
                //        dt.Columns.Add("count");//作业票总数量
                //        dt.Columns.Add("areacode");//区域编码
                //        dt.Columns.Add("content");//作业票数据集合
                //        dt.Columns.Add("count1");//一级风险作业票数量
                //        dt.Columns.Add("count2");//二级风险作业票数量
                //        dt.Columns.Add("count3");//三级风险作业票数量
                //        dt.Columns.Add("count4");//四级风险作业票数量
                //        dt.Columns.Add("data1");//一级风险作业票集合
                //        dt.Columns.Add("data2");//一级风险作业票集合
                //        dt.Columns.Add("data3");//一级风险作业票集合
                //        dt.Columns.Add("data4");//一级风险作业票集合
                //        dt.Columns.Add("status");//区域作业票最高风险等级
                //        foreach (string code in arr)
                //        {
                //            DataTable dtDis = deptBll.GetDataTable(string.Format("select d.districtcode from bis_district d where d.description='{0}'", code));
                //            if (dtDis.Rows.Count > 0)
                //            {
                //                int status = 0;
                //                string areaCode = dtDis.Rows[0][0].ToString();
                //                var content = list.Where(t => t.EQ_LO_USERDEF3 == areaCode);
                //                var data1 = content.Where(t => t.RA_RA_FX_DJ == "一级");
                //                var data2 = content.Where(t =>t.RA_RA_FX_DJ == "二级");
                //                var data3 = content.Where(t =>t.RA_RA_FX_DJ == "三级");
                //                var data4 = content.Where(t =>t.RA_RA_FX_DJ == "四级");
                //                var count1 = data1.Count();
                //                var count2 = data2.Count();
                //                var count3 = data3.Count();
                //                var count4 = data4.Count();
                //                if (count1 > 0)
                //                {
                //                    status = 1;
                //                }
                //                else if (count2 > 0)
                //                {
                //                    status = 2;
                //                }
                //                else if (count3 > 0)
                //                {
                //                    status = 3;
                //                }
                //                else if (count4 > 0)
                //                {
                //                    status = 4;
                //                }
                //                else
                //                {
                //                    status = 0;
                //                }
                //                DataRow row = dt.NewRow();
                //                row[0] = code;
                //                row[1] = content.Count();
                //                row[2] = areaCode;
                //                row[3] = content.ToJson();
                //                row["count1"] = count1;
                //                row["count2"] = count2;
                //                row["count3"] = count3;
                //                row["count4"] = count4;
                //                row["data1"] = data1.ToJson();
                //                row["data2"] = data2.ToJson();
                //                row["data3"] = data3.ToJson();
                //                row["data4"] = data4.ToJson();
                //                row["status"] = status;
                //                dt.Rows.Add(row);
                //            }
                //        }
                //        return new { code = 0, message = "获取数据成功", data = dt };
                //    }
                //    else
                //    {
                //        return new { code = 1, message = "获取数据发生错误" };
                //    }
                //}
                //else
                //{
                //    return new { code = 1, message = "无数据" };
                //}



                if (string.IsNullOrEmpty(workTime))
                {
                    workTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
                string token = new TokenCache().GetSafetyToken();
                if (!string.IsNullOrEmpty(token))
                {
                    string environmentalUrl = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("WorkTicketsUrl");
                    if (string.IsNullOrWhiteSpace(environmentalUrl))
                    {
                        return Newtonsoft.Json.JsonConvert.SerializeObject(new { code = -1, message = "地址未配置！" });
                    }
                    string url = environmentalUrl + token;
                    WebClient wc = new WebClient();
                    wc.Credentials = CredentialCache.DefaultCredentials;
                    //wc.Headers.Add("Content-Type", "text/json; charset=utf-8");
                    wc.Encoding = Encoding.GetEncoding("utf-8");
                    System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                    string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string safetytoken = BSFramework.Util.Md5Helper.MD5("GRP" + timeStamp + "Interface", 32);
                    //nc.Add("Token", safetytoken);
                    nc.Add("TimeStamp", timeStamp);
                    nc.Add("ModuleID", "Safety");
                    nc.Add("EntityID", "Permit");
                    nc.Add("CompanyID", "CRPHB");
                    nc.Add("MethodName", "GetPermitListHB");
                    nc.Add("Parameters", JsonConvert.SerializeObject(new { WO_SD_CS_DT_Begin = workTime, PageIndex = 1, PageSize = pageSize }));
                    url += "&TimeStamp=" + timeStamp + "&ModuleID=Safety&EntityID=Permit&CompanyID=CRPHB&MethodName=GetPermitListHB&Parameters=" + JsonConvert.SerializeObject(new { WO_SD_CS_DT_Begin = workTime, PageIndex = 1, PageSize = pageSize });
                    var data = wc.DownloadString(url);
                    if (data.Contains("data"))
                    {
                        if (data.StartsWith("(") && data.EndsWith(")"))
                        {
                            data = data.TrimStart('(').TrimEnd(')');
                        }
                        dynamic dy = Newtonsoft.Json.JsonConvert.DeserializeObject<ExpandoObject>(data);
                        if (dy.code == 0)
                        {
                            List<object> objs = dy.data;
                            if (objs.Count > 0)
                            {
                                data = Newtonsoft.Json.JsonConvert.SerializeObject(objs);
                                List<WorkTicketEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WorkTicketEntity>>(data);
                                string[] arr = districtCode.Split(',');
                                DepartmentBLL deptBll = new DepartmentBLL();

                                DataTable dt = new DataTable();
                                dt.Columns.Add("code"); //区域图片对应编码
                                dt.Columns.Add("count");//作业票总数量
                                dt.Columns.Add("areacode");//区域编码
                                dt.Columns.Add("content");//作业票数据集合
                                dt.Columns.Add("count1");//一级风险作业票数量
                                dt.Columns.Add("count2");//二级风险作业票数量
                                dt.Columns.Add("count3");//三级风险作业票数量
                                dt.Columns.Add("count4");//四级风险作业票数量
                                dt.Columns.Add("data1");//一级风险作业票集合
                                dt.Columns.Add("data2");//一级风险作业票集合
                                dt.Columns.Add("data3");//一级风险作业票集合
                                dt.Columns.Add("data4");//一级风险作业票集合
                                dt.Columns.Add("status");//区域作业票最高风险等级
                                foreach (string code in arr)
                                {
                                    DataTable dtDis = deptBll.GetDataTable(string.Format("select d.districtcode from bis_district d where d.description='{0}'", code));
                                    if (dtDis.Rows.Count > 0)
                                    {
                                        int status = 0;
                                        string areaCode = dtDis.Rows[0][0].ToString();
                                        var content = list.Where(t => t.WO_SD_AR_LO == areaCode);
                                        var data1 = content.Where(t => t.RA_RA_FX_DJ == "一级");
                                        var data2 = content.Where(t => t.RA_RA_FX_DJ == "二级");
                                        var data3 = content.Where(t => t.RA_RA_FX_DJ == "三级");
                                        var data4 = content.Where(t => t.RA_RA_FX_DJ == "四级");
                                        var count1 = data1.Count();
                                        var count2 = data2.Count();
                                        var count3 = data3.Count();
                                        var count4 = data4.Count();
                                        if (count1 > 0)
                                        {
                                            status = 1;
                                        }
                                        else if (count2 > 0)
                                        {
                                            status = 2;
                                        }
                                        else if (count3 > 0)
                                        {
                                            status = 3;
                                        }
                                        else if (count4 > 0)
                                        {
                                            status = 4;
                                        }
                                        else
                                        {
                                            status = 0;
                                        }
                                        DataRow row = dt.NewRow();
                                        row[0] = code;
                                        row[1] = content.Count();
                                        row[2] = areaCode;
                                        row[3] = content.ToJson();
                                        row["count1"] = count1;
                                        row["count2"] = count2;
                                        row["count3"] = count3;
                                        row["count4"] = count4;
                                        row["data1"] = data1.ToJson();
                                        row["data2"] = data2.ToJson();
                                        row["data3"] = data3.ToJson();
                                        row["data4"] = data4.ToJson();
                                        row["status"] = status;
                                        dt.Rows.Add(row);
                                    }
                                }
                                return new { code = 0, message = "获取数据成功", data = dt };
                            }
                            else
                            {
                                return Newtonsoft.Json.JsonConvert.SerializeObject(new { code = -1, message = "没有查询到数据" });
                            }
                        }
                        else
                        {
                            return Newtonsoft.Json.JsonConvert.SerializeObject(new { code = -1, message = "获取数据发生异常" });
                        }
                    }
                    else
                    {
                        return new { code = 0, message = "没有获取到数据", data = new List<object>() };
                    }
                }
                else
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(new { code = -1, message = "token失效", point = "" });
                }

            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(new { code = -1, message = ex.Message });
            }

        }

        /// <summary>
        /// 通过safety系统中的工作内容关联云平台的作业安全分析库
        /// </summary>
        [System.Web.Http.HttpPost]
        public object GetWorkSafeAnalysis([FromBody]JObject json)
        {
            RisktrainlibdetailBLL risktrainlibdetailbll = new RisktrainlibdetailBLL();
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string worktask = dy.worktask;
            DataTable risktrainlib = new RisktrainlibBLL().GetRisktrainlibList(new { worktask = worktask }.ToJson());
            List<object> objects = new List<object>();
            if (risktrainlib.Rows.Count > 0)
            {
                foreach (DataRow row in risktrainlib.Rows)
                {
                    objects.Add(new
                    {
                        Id = row["id"].ToString(),//模板ID
                        WorkTask = row["WorkTask"].ToString(),//工作内容
                        Workdes = row["WorkDes"].ToString(), //作业描述
                        WorkArea = row["WorkArea"].ToString(),//作业区域
                        WorkAreaId = row["WorkAreaId"].ToString(),//作业区域
                        RiskLevel = row["risklevel"].ToString(),//风险等级
                        DetailList = risktrainlibdetailbll.GetList(row["ID"].ToString())
                    });
                }
            }


            return new { Code = 0, Count = -1, Info = "获取数据成功", data = objects };
        }

        #endregion

        #region 风险清单
        /// <summary>
        /// 获取风险清单
        /// </summary>
        /// <param name="encode">华润湖北（hrdl）,国电荥阳（gdxy）</param>
        /// <param name="workcontent"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public object GetRiskassess(string encode, string workcontent)
        {
            try
            {
                if (string.IsNullOrEmpty(workcontent))
                {
                    return new { code = -1, message = "请先选择工作任务." };
                }
                encode = string.IsNullOrEmpty(diBll.GetItemValue(encode, "FactoryEncode")) ? "-100" : diBll.GetItemValue(encode, "FactoryEncode");
                if (encode.ToLower() == "hrdl")
                {
                    var data = new DepartmentBLL().GetDataTable(string.Format("select a.atrisk dangersource,a.controls measures,a.process,t.risklevel grade from bis_risktrainlibdetail a  left join BIS_RISKTRAINLIB t on a.workid=t.id where  deptcode like '{0}%' and t.WorkTask like '%{1}%'", encode, workcontent));
                    return new { code = 0, message = "获取数据成功", data = data };
                }
                else
                {
                    var data = riskassessbll.GetListFor(string.Format(" and deptcode like '{0}%' and worktask like '%{1}%'", encode, workcontent));

                    if (HttpContext.Current.Request.Params["jsoncallback"] != null)
                    {
                        string result = Newtonsoft.Json.JsonConvert.SerializeObject(new { code = 0, message = "获取数据成功", data = data.Select(x => new { process = x.Process, dangersource = x.DangerSource, grade = x.Grade, measures = measuresbll.GetList(string.Format(" and Riskid ='{0}'", x.Id)).Select(y => new { content = y.Content }) }) });
                        return HttpContext.Current.Request.Params["jsoncallback"] + "(" + result + ")";
                    }
                    else
                    {
                        return new { code = 0, message = "获取数据成功", data = data.Select(x => new { process = x.Process, dangersource = x.DangerSource, grade = x.Grade, measures = measuresbll.GetList(string.Format(" and Riskid ='{0}'", x.Id)).Select(y => new { content = y.Content }) }) };
                    }
                }


            }
            catch (Exception ex)
            {
                return new { code = -1, message = ex.Message };
            }
        }
        #endregion

        #region 获取天气预警（华润湖北）
        [System.Web.Http.HttpGet]
        public object GetWeatherWarning(string weather)
        {
            try
            {
                var data = weatherbll.GetRequire(weather);
                return new { code = 0, message = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {
                return new { code = -1, message = ex.Message };
            }
        }

        #endregion

        #region 获取培训平台的安全意识值（华润湖北）
        [System.Web.Http.HttpGet]
        public object GetSafetyPoint(string idcardno)
        {
            try
            {
                string trainserviceurl = diBll.GetItemValue("TrainServiceUrl");
                if (string.IsNullOrEmpty(trainserviceurl))
                {
                    return new { code = -1, message = "服务器连接失败." };
                }
                UserEntity user = userbll.GetUserByIdCard(idcardno);
                if (user == null)
                {
                    return new { code = -1, message = "身份证号码错误." };
                }

                WebClient wc = new WebClient();
                System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                nc.Add("json", JsonConvert.SerializeObject(new { Business = "GetQualityObj", acount = user.Account }));

                var data = System.Text.Encoding.Default.GetString(wc.UploadValues(trainserviceurl, "POST", nc));
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(data);
                string point = "";
                if (dy.Qualitydata.Count > 0)
                {
                    dy = dy.Qualitydata[0];
                    point = dy.point;
                }
                else
                {
                    point = "0";
                }
                return new { code = 0, message = "获取数据成功", data = point };
            }
            catch (Exception ex)
            {
                return new { code = -1, message = ex.Message };
            }

        }
        #endregion

        #region 同步元素负责人工作总结（华润湖北）
        /// <summary>  
        /// 同步元素负责人工作总结
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public void SyncPersonWorkSummary()
        {
            string strSql = string.Format(@"select *  from hrs_nosaworkmanager w where to_char(w.month,'yyyy-MM')='{0}'", DateTime.Now.ToString("yyyy-MM"));
            DataTable dtRow = worksummary.GetTable(strSql);
            if (dtRow.Rows.Count > 0)
            {
                LogEntity logEntity = new LogEntity();
                logEntity.CategoryId = 3;
                logEntity.OperateTypeId = ((int)OperationType.Create).ToString();
                logEntity.OperateType = "自动生成工作总结";
                logEntity.OperateAccount = "System";
                logEntity.OperateUserId = "System";
                logEntity.ExecuteResult = 0;
                logEntity.ExecuteResultJson = "本月已有工作总结,无需生成";
                logEntity.WriteLog();
                //return Success("本月已有工作总结,无需生成");
            }
            else
            {

                string sql = string.Format(@"insert into hrs_nosaworkmanager
                                          (ID,createuserid,createdate,createuserdeptcode,createuserorgcode,
                                           createusername,elementname,elementid,elementsuperid,
                                           elementsuper,dutydepart,dutydepartid,dutydepartcode,
                                           month,iscommit)
                                          select NewGuid(),n.createuserid,n.createdate,n.createuserdeptcode,
                                                 n.createuserorgcode,n.createusername,n.name,n.id,
                                                 n.dutyuserid,n.dutyusername,n.dutydepartname,n.dutydepartid,
                                                 d.encode,sysdate, 0
                                            from hrs_nosaele n
                                            left join base_department d on d.departmentid = n.dutydepartid where n.state!=1");
                if (worksummary.SyncPersonWorkSummary(sql))
                {
                    LogEntity logEntity = new LogEntity();
                    logEntity.CategoryId = 3;
                    logEntity.OperateTypeId = ((int)OperationType.Create).ToString();
                    logEntity.OperateType = "自动生成工作总结";
                    logEntity.OperateAccount = "System";
                    logEntity.OperateUserId = "System";
                    logEntity.ExecuteResult = 0;
                    logEntity.ExecuteResultJson = "工作总结生成成功";
                    logEntity.WriteLog();
                    //                    //获取人员账号
                    //                    string personSql = string.Format(@"select u.account,u.realname,m.id
                    //                                                              from base_user u
                    //                                                             inner join (select distinct m.elementsuperid,m.id from hrs_nosaworkmanager m ) m
                    //                                                                on m.elementsuperid = u.userid ");
                    //                    DataTable dt = worksummary.GetTable(personSql);
                    //                    for (int i = 0; i < dt.Rows.Count; i++)
                    //                    {
                    //                        JPushApi.PushMessage(dt.Rows[i]["account"].ToString(), dt.Rows[i]["realname"].ToString(), "NosaW001", dt.Rows[i]["id"].ToString());
                    //                    }
                    //return Success("同步成功");
                }
                else
                {
                    LogEntity logEntity = new LogEntity();
                    logEntity.CategoryId = 3;
                    logEntity.OperateTypeId = ((int)OperationType.Create).ToString();
                    logEntity.OperateType = "自动生成工作总结";
                    logEntity.OperateAccount = "System";
                    logEntity.OperateUserId = "System";
                    logEntity.ExecuteResult = -1;
                    logEntity.ExecuteResultJson = "执行SQL出错";
                    logEntity.WriteLog();
                }
            }

        }
        #endregion

        #region 同步区域代表工作总结（华润湖北）
        /// <summary>
        /// 同步区域代表工作总结
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public void SyncAreaWorkSummary()
        {
            string strSql = string.Format(@"select *  from hrs_nosaareaworksummary w where to_char(w.month,'yyyy-MM')='{0}'", DateTime.Now.ToString("yyyy-MM"));
            DataTable dtRow = worksummary.GetTable(strSql);
            if (dtRow.Rows.Count > 0)
            {
                LogEntity logEntity = new LogEntity();
                logEntity.CategoryId = 3;
                logEntity.OperateTypeId = ((int)OperationType.Create).ToString();
                logEntity.OperateType = "自动生成工作总结";
                logEntity.OperateAccount = "System";
                logEntity.OperateUserId = "System";
                logEntity.ExecuteResult = 0;
                logEntity.ExecuteResultJson = "本月已有工作总结,无需生成";
                logEntity.WriteLog();
                //return Success("本月已有工作总结,无需生成");
            }
            else
            {
                string sql = string.Format(@"insert into hrs_nosaareaworksummary
                                          (ID,createuserid,createdate,createuserdeptcode,createuserorgcode,
                                           createusername,areaname,areaid,areasuperid,areasuper,
                                           dutydepart,dutydepartid,dutydepartcode,month,iscommit)
                                          select NewGuid(),n.createuserid, n.createdate,
                                                 n.createuserdeptcode,n.createuserorgcode, n.createusername,
                                                 n.name, n.id, n.dutyuserid, n.dutyusername,
                                                 n.dutydepartname,n.dutydepartid, d.encode,sysdate,0 
                                            from HRS_NOSAAREA n
                                            left join base_department d on d.departmentid = n.dutydepartid where n.state!=1");
                if (worksummary.SyncPersonWorkSummary(sql))
                {
                    LogEntity logEntity = new LogEntity();
                    logEntity.CategoryId = 3;
                    logEntity.OperateTypeId = ((int)OperationType.Create).ToString();
                    logEntity.OperateType = "自动生成工作总结";
                    logEntity.OperateAccount = "System";
                    logEntity.OperateUserId = "System";
                    logEntity.ExecuteResult = 0;
                    logEntity.ExecuteResultJson = "工作总结生成成功";
                    logEntity.WriteLog();
                    //                    //获取人员账号
                    //                    string personSql = string.Format(@"select u.account,u.realname,m.id
                    //                                                              from base_user u
                    //                                                             inner join (select distinct m.areasuperid,m.id from hrs_nosaareaworksummary m ) m
                    //                                                                on m.areasuperid = u.userid ");
                    //                    DataTable dt = worksummary.GetTable(personSql);
                    //                    for (int i = 0; i < dt.Rows.Count; i++)
                    //                    {
                    //                        JPushApi.PushMessage(dt.Rows[i]["account"].ToString(), dt.Rows[i]["realname"].ToString(), "NosaW001", dt.Rows[i]["id"].ToString());
                    //                    }
                    //return Success("同步成功");
                }
                else
                {
                    LogEntity logEntity = new LogEntity();
                    logEntity.CategoryId = 3;
                    logEntity.OperateTypeId = ((int)OperationType.Create).ToString();
                    logEntity.OperateType = "自动生成工作总结";
                    logEntity.OperateAccount = "System";
                    logEntity.OperateUserId = "System";
                    logEntity.ExecuteResult = -1;
                    logEntity.ExecuteResultJson = "执行SQL出错";
                    logEntity.WriteLog();
                }
            }

        }
        #endregion

        #region 获取用户信息（支持分页）
        /// <summary>
        /// 获取用户信息（支持分页）
        /// </summary>
        /// <param name="json">分页及查询条件（pageIndex:页索引,pageSize:每页记录数，userName:姓名，deptName:部门名称,jobType:工种,mobile:手机号）</param>
        /// <returns></returns>
        public object GetUserList(string json)
        {
            try
            {
                json = string.IsNullOrEmpty(json) ? "{}" : json;
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(json);
                long pageIndex = json.Contains("pageIndex") ? dy.pageIndex : 1;
                long pageSize = json.Contains("pageSize") ? dy.pageSize : 20;
                Pagination pagination = new Pagination();
                pagination.p_kid = "userid";
                pagination.p_tablename = "v_userinfo u";
                pagination.page = int.Parse(pageIndex.ToString());
                pagination.rows = int.Parse(pageSize.ToString());
                pagination.sidx = "REALNAME";
                pagination.sord = "asc";
                pagination.p_fields = "EnCode,IsPresence,u.REALNAME,u.Gender,u.identifyid,u.Birthday,u.entertime,u.healthstatus,u.Craft,CraftAge,u.POSTNAME,JobTitle,u.DUTYNAME,u.DegreesID Degrees,u.SpecialtyType,Mobile,u.ISSPECIAL,u.ISSPECIALEQU,u.nation,u.NATIVE,u.Political,u.DEPTNAME";
                string sql = " 1=1 ";
                if (json.Contains("userName"))
                {
                    string userName = dy.userName;
                    if (!string.IsNullOrWhiteSpace(userName))
                    {
                        sql += string.Format(" and realname like '%{0}%'", userName);
                    }
                }
                if (json.Contains("deptName"))
                {
                    string deptName = dy.deptName;
                    if (!string.IsNullOrWhiteSpace(deptName))
                    {
                        sql += string.Format(" and deptname like '%{0}%'", deptName);
                    }
                }
                if (json.Contains("jobType"))
                {
                    string jobType = dy.jobType;
                    if (!string.IsNullOrWhiteSpace(jobType))
                    {
                        sql += string.Format(" and Craft like '%{0}%'", jobType);
                    }
                }
                if (json.Contains("mobile"))
                {
                    string mobile = dy.mobile;
                    if (!string.IsNullOrWhiteSpace(mobile))
                    {
                        sql += string.Format(" and mobile like '%{0}%'", mobile);
                    }
                }
                pagination.conditionJson = sql;
                DataTable dtUsers = new UserBLL().GetPageList(pagination, "{}");
                Dictionary<string, string> dict_props = new Dictionary<string, string>();
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                    DateFormatString = "yyyy-MM-dd" //格式化日期
                };
                settings.Converters.Add(new DecimalToStringConverter());
                return new { code = 0, message = "获取数据成功", count = pagination.records, data = JArray.Parse(JsonConvert.SerializeObject(dtUsers, Formatting.None, settings)) };
            }
            catch (Exception ex)
            {
                return new { code = -1, message = ex.Message };
            }
        }
        #endregion

        #region 新增或保存用户(国电荥阳)
        /// <summary>
        /// 新增或保存用户(国电荥阳)
        /// </summary>
        /// <param name="json">用户信息（对象被序列化之后的）</param>
        /// <returns></returns>
        //[HttpPost]
        public object SaveUser(string json)
        {
            string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
            try
            {
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(json);
                string userName = dy.realName;//姓名
                string userAccount = dy.account;//账号
                string workNum = dy.workNum;//工号
                string deptId = dy.deptId;//所属部门ID

                DepartmentBLL deptBll = new DepartmentBLL();
                DataTable dtDept = deptBll.GetDataTable(string.Format("select t.departmentid from BASE_DEPARTMENT t where t.misdeptid='{0}' ", deptId));
                object obj = null;
                if (dtDept.Rows.Count == 0)
                {

                    if (HttpContext.Current.Request.Params["jsoncallback"] != null)
                    {
                        string result = Newtonsoft.Json.JsonConvert.SerializeObject(new { code = 0, message = "当前部门不存在，请先配置部门对应关系" });
                        return HttpContext.Current.Request.Params["jsoncallback"] + "('" + result + "')";
                    }
                    else
                    {
                        return new { code = 0, message = "当前部门不存在，请先配置部门对应关系" };
                    }
                }
                else
                {
                    try
                    {
                        deptId = dtDept.Rows[0][0].ToString();
                        List<RoleEntity> rolist = new List<RoleEntity>();
                        UserEntity user = new UserEntity
                        {
                            Account = userAccount,
                            RealName = userName,
                            EnCode = workNum,
                            DepartmentId = deptId,
                            IsEpiboly = "0",
                            IsPresence = "1"
                        };
                        var dept = deptBll.GetEntity(deptId);
                        string roleId = "2a878044-06e9-4fe4-89f0-ba7bd5a1bde6";
                        string roleName = "普通用户";
                        if (dept != null)
                        {
                            user.DepartmentId = deptId;
                            user.DepartmentCode = dept.EnCode;

                            if (dept.Nature == "班组")
                            {
                                roleId += ",d9432a6e-5659-4f04-9c10-251654199714";
                                roleName += ",班组级用户";

                            }
                            if (dept.Nature == "承包商")
                            {
                                roleId += ",c5530ccf-e84e-4df8-8b27-fd8954a9bbe9";
                                roleName += ",承包商用户";

                            }
                            if (dept.Nature == "部门")
                            {
                                roleId += "6c094cef-cca3-4b41-a71b-6ee5e6b89008";
                                roleName += "部门级用户";
                            }
                            if (dept.Nature == "厂级")
                            {
                                roleId += "aece6d68-ef8a-4eac-a746-e97f0067fab5";
                                roleName += "公司级用户";
                            }
                            dept = deptBll.GetEntity(dept.OrganizeId);
                            if (dept != null)
                            {
                                user.OrganizeCode = dept.EnCode;
                                user.OrganizeId = dept.OrganizeId;
                            }
                            user.RoleId = roleId;
                            user.RoleName = roleName;
                        }
                        else
                        {
                            obj = new { code = 0, message = "当前部门不存在" };
                        }
                        UserBLL userBll = new UserBLL();
                        dtDept = deptBll.GetDataTable(string.Format("select userid from base_user where account='{0}'", userAccount));
                        if (dtDept.Rows.Count == 0)
                        {
                            userBll.SaveForm("", user);
                        }
                        else
                        {
                            userBll.SaveForm(dtDept.Rows[0][0].ToString(), user);
                        }
                        System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ,保存用户成功,同步信息：" + json + "\r\n");
                        obj = new { code = 0, message = "操作成功" };
                        if (HttpContext.Current.Request.Params["jsoncallback"] != null)
                        {
                            string result = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                            return HttpContext.Current.Request.Params["jsoncallback"] + "('" + result + "')";
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (HttpContext.Current.Request.Params["jsoncallback"] != null)
                        {
                            obj = new { code = -1, message = ex.Message };
                            string result = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                            return HttpContext.Current.Request.Params["jsoncallback"] + "('" + result + "')";
                        }
                        else
                        {
                            return new { code = -1, message = ex.Message };
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ,保存用户失败,同步信息：" + json + ",异常信息:" + ex.Message + "\r\n");
                if (HttpContext.Current.Request.Params["jsoncallback"] != null)
                {
                    return HttpContext.Current.Request.Params["jsoncallback"] + "('" + ex.Message + "')";
                }
                else
                {
                    return new { code = -1, message = ex.Message };
                }

            }
        }
        #endregion

        #region 删除用户(国电荥阳)
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userAccount">用户账号</param>
        /// <returns></returns>
        //[HttpPost]
        public object DeleteUser(string userAccount)
        {
            string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
            try
            {

                UserBLL userBll = new UserBLL();
                UserInfoEntity user = userBll.GetUserInfoByAccount(userAccount);
                object obj = null;
                if (user != null)
                {
                    userBll.RemoveForm(user.UserId);
                    System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ,删除用户成功,同步信息：" + Newtonsoft.Json.JsonConvert.SerializeObject(user) + "\r\n");
                    obj = new { code = 0, message = "操作成功" };

                }
                else
                {
                    obj = new { code = -1, message = "用户信息不存在" };
                }
                if (HttpContext.Current.Request.Params["jsoncallback"] != null)
                {
                    string result = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                    return HttpContext.Current.Request.Params["jsoncallback"] + "('" + result + "')";
                }
                else
                {
                    return obj;
                }
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ,删除用户失败,用户账号：" + userAccount + ",异常信息:" + ex.Message + "\r\n");

                if (HttpContext.Current.Request.Params["jsoncallback"] != null)
                {
                    string result = Newtonsoft.Json.JsonConvert.SerializeObject(new { code = -1, message = ex.Message });
                    return HttpContext.Current.Request.Params["jsoncallback"] + "('" + result + "')";
                }
                else
                {
                    return new { code = -1, message = ex.Message };
                }
            }
        }
        #endregion

        #region 新增或保存部门(国电荥阳)
        /// <summary>
        /// 新增或保存部门
        /// </summary>
        /// <param name="keyValue">记录Id</param>
        /// <param name="json">部门信息（对象被序列化之后的）</param>
        /// <returns></returns>
        //[HttpPost]
        public object SaveDept(string json)
        {
            string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
            try
            {
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(json);
                string deptName = dy.deptName;
                string mDeptId = dy.deptId;
                string parentId = dy.parentId;
                DepartmentEntity dept = new DepartmentEntity
                {
                    FullName = deptName,
                    // MisDeptId = mDeptId

                };
                DepartmentBLL deptBll = new DepartmentBLL();
                DataTable dtDept = deptBll.GetDataTable(string.Format("select t.departmentid from BASE_DEPARTMENT t where t.MisDeptId='{0}'", parentId));
                object obj = null;
                if (dtDept.Rows.Count > 0)
                {
                    DepartmentEntity parentDept = deptBll.GetEntity(dtDept.Rows[0][0].ToString());
                    if (parentDept != null)
                    {
                        dept.ParentId = parentDept.DepartmentId;
                        dept.OrganizeId = parentDept.OrganizeId;

                    }
                    else
                    {
                        obj = new { code = -1, message = "上级部门不存在,请确认是否配置对应关系" };
                    }
                }
                else
                {
                    obj = new { code = -1, message = "上级部门不存在,请确认是否配置对应关系" };
                }

                dtDept = deptBll.GetDataTable(string.Format("select t.departmentid from BASE_DEPARTMENT t where t.fullname='{0}'", deptName));
                if (dtDept.Rows.Count > 0)
                {
                    deptBll.SaveForm(dtDept.Rows[0][0].ToString(), dept);
                }
                else
                {
                    deptBll.SaveForm("", dept);
                }
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ",保存部门成功,同步信息：" + json + "\r\n");
                obj = new { code = 0, message = "操作成功" };

                if (HttpContext.Current.Request.Params["jsoncallback"] != null)
                {
                    string result = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                    return HttpContext.Current.Request.Params["jsoncallback"] + "('" + result + "')";
                }
                else
                {
                    return obj;
                }
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ,保存部门失败,同步信息：" + json + ",异常信息:" + ex.Message + "\r\n");

                if (HttpContext.Current.Request.Params["jsoncallback"] != null)
                {
                    string result = Newtonsoft.Json.JsonConvert.SerializeObject(new { code = -1, message = ex.Message });
                    return HttpContext.Current.Request.Params["jsoncallback"] + "('" + result + "')";
                }
                else
                {
                    return new { code = -1, message = ex.Message };
                }
            }
        }
        #endregion

        #region 删除部门(国电荥阳)
        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="deptId">部门Id</param>
        /// <returns></returns>
        //[HttpPost]
        public object DeleteDept(string deptId)
        {
            string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
            try
            {
                object obj = null;
                DepartmentBLL deptBll = new DepartmentBLL();
                DataTable dtDept = deptBll.GetDataTable(string.Format("select t.departmentid from BASE_DEPARTMENT t where t.misdeptid='{0}'", deptId));
                if (dtDept.Rows.Count > 0)
                {
                    DepartmentEntity dept = deptBll.GetEntity(dtDept.Rows[0][0].ToString());
                    dept.EnabledMark = 0;
                    dept.DeleteMark = 0;
                    deptBll.RemoveForm(dtDept.Rows[0][0].ToString(), new List<UserEntity>());
                    System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ",删除部门成功,同步信息：" + Newtonsoft.Json.JsonConvert.SerializeObject(dept) + "\r\n");
                    obj = new { code = 0, message = "操作成功" };
                }
                else
                {
                    obj = new { code = -1, message = "该部门不存在" };
                }
                if (HttpContext.Current.Request.Params["jsoncallback"] != null)
                {
                    string result = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                    return HttpContext.Current.Request.Params["jsoncallback"] + "('" + result + "')";
                }
                else
                {
                    return obj;
                }
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ,删除部门成功,部门MisKey：" + deptId + ",异常信息:" + ex.Message + "\r\n");

                if (HttpContext.Current.Request.Params["jsoncallback"] != null)
                {
                    string result = Newtonsoft.Json.JsonConvert.SerializeObject(new { code = -1, message = ex.Message });
                    return HttpContext.Current.Request.Params["jsoncallback"] + "('" + result + "')";
                }
                else
                {
                    return new { code = -1, message = ex.Message };
                }
            }
        }
        #region 获取风险清单数据（国电荥阳）
        [System.Web.Http.HttpGet]
        public object getRiskList(string json)
        {
            try
            {
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(json);
                string postName = json.Contains("postName") ? dy.postName : "";
                string riskType = json.Contains("riskType") ? dy.riskType : "";
                string areaCode = json.Contains("areaCode") ? dy.areaCode : "";
                string keyWord = json.Contains("keyWord") ? dy.keyWord : "";
                long pageIndex = json.Contains("pageIndex") ? dy.pageIndex : 1;
                long pageSize = json.Contains("pageSize") ? dy.pageSize : int.MaxValue;
                string sql = " status=1 and deletemark=0 ";
                if (!string.IsNullOrEmpty(riskType))
                {
                    sql += " and risktype='" + riskType + "'";
                }
                if (!string.IsNullOrEmpty(postName))
                {
                    sql += " and JobName like '%" + riskType + "%'";
                }
                if (!string.IsNullOrEmpty(areaCode))
                {
                    sql += " and areacode='" + areaCode + "'";
                }
                if (!string.IsNullOrEmpty(keyWord))
                {
                    if (riskType == "工器具及危化品")
                    {
                        sql += " and ToolOrDanger like '%" + keyWord.Trim() + "%'";
                    }
                    if (riskType == "作业")
                    {
                        sql += " and WorkTask like '%" + keyWord.Trim() + "%'";
                    }
                }
                DepartmentBLL deptBll = new DepartmentBLL();
                DataTable dt = deptBll.GetDataTable(string.Format("select count(1)  from BIS_RISKASSESS t where " + sql));
                decimal p_totalRecords = decimal.Parse(dt.Rows[0][0].ToString());
                decimal p_totalPages = 0;

                int v_startRecord = 0, v_endRecord = 0;
                if (p_totalRecords % pageSize == 0)
                {
                    p_totalPages = p_totalRecords / int.Parse(pageSize.ToString());
                    p_totalPages = Math.Round(p_totalPages, 0);
                }
                else
                {
                    p_totalPages = p_totalRecords / pageSize + 1;
                    p_totalPages = Math.Round(p_totalPages, 0);
                }
                if (pageIndex < 1)
                {
                    pageIndex = 1;
                }
                if (pageIndex > p_totalPages)
                {
                    pageIndex = long.Parse(p_totalPages.ToString());
                }
                //--实现分页查询
                v_startRecord = (int.Parse(pageIndex.ToString()) - 1) * int.Parse(pageSize.ToString()) + 1;
                v_endRecord = int.Parse(pageIndex.ToString()) * int.Parse(pageSize.ToString());
                List<object> list = new List<object>();
                dt = deptBll.GetDataTable(string.Format("SELECT * FROM (SELECT A.*, rownum r FROM (SELECT id,WorkTask,Process,JobName,HjEqupment,DangerSource,RiskDesc,DeptName,risktype,grade,areacode,areaname,ToolOrDanger FROM BIS_RISKASSESS WHERE " + sql + " ORDER BY areacode asc) A WHERE rownum <= {1}) B WHERE r >={0}", v_startRecord, v_endRecord));
                foreach (DataRow dr in dt.Rows)
                {
                    DataTable dtMeasure = deptBll.GetDataTable(string.Format("select content from BIS_MEASURES where riskid='{0}'", dr[0].ToString()));
                    list.Add(new
                    {
                        workTask = dr["workTask"].ToString(), //作业名称
                        process = dr["process"].ToString(),//工序
                        jobName = dr["jobName"].ToString(),//岗位名称
                        dangerSource = dr["dangerSource"].ToString(), //危险源
                        equName = dr["HjEqupment"].ToString(), //设备名称
                        factor = dr["RiskDesc"].ToString(),//危险因素
                        grade = dr["grade"].ToString(), //风险级别
                        deptName = dr["deptname"].ToString(), //管控部门
                        riskType = dr["risktype"].ToString(), //风险类别
                        areaCode = dr["areacode"].ToString(),//区域编码
                        areaName = dr["areaname"].ToString(),//区域名称
                        toolsName = dr["ToolOrDanger"].ToString(), //工器具或危化品名称，
                        measures = dtMeasure
                    });
                }

                if (HttpContext.Current.Request.Params["jsoncallback"] != null)
                {
                    string result = Newtonsoft.Json.JsonConvert.SerializeObject(new { code = 0, message = "", data = list });
                    return HttpContext.Current.Request.Params["jsoncallback"] + "('" + result + "')";
                }
                else
                {
                    return new { code = 0, message = "", data = list };
                }
            }
            catch (Exception ex)
            {
                return new { code = -1, message = ex.Message };
            }

        }
        #endregion
        #region 获取风险清单数据
        [System.Web.Http.HttpGet]
        public object getRiskDbList(string queryJson = "{}")
        {
            try
            {
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(queryJson);
                long pageIndex = queryJson.Contains("pageIndex") ? dy.pageIndex : 1;
                long pageSize = queryJson.Contains("pageSize") ? dy.pageSize : 20;
                string sql = " status=1 and deletemark=0 and risktype='作业'";
                int v_startRecord = 0, v_endRecord = 0;
                string isGxhs = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("广西华昇版本");
                if (!string.IsNullOrWhiteSpace(isGxhs))
                {
                    sql = " status=1 and deletemark=0 and risktype='作业活动'";
                    string workName = queryJson.Contains("workName") ? dy.workName : "";
                    if (!string.IsNullOrWhiteSpace(workName))
                    {
                        sql += " and name like '%" + workName.Trim() + "%'";
                    }

                    string stepName = queryJson.Contains("stepName") ? dy.stepName : "";
                    if (!string.IsNullOrWhiteSpace(stepName))
                    {
                        sql += " and WorkContent like '%" + stepName.Trim() + "%'";
                    }

                    string harmName = queryJson.Contains("harmName") ? dy.harmName : "";
                    if (!string.IsNullOrWhiteSpace(harmName))
                    {
                        sql += " and HarmName like '%" + harmName.Trim() + "%'";
                    }
                    string riskDesc = queryJson.Contains("riskDesc") ? dy.riskDesc : "";
                    if (!string.IsNullOrWhiteSpace(harmName))
                    {
                        sql += " and RiskDesc like '%" + riskDesc.Trim() + "%'";
                    }

                    string riskType = queryJson.Contains("riskType") ? dy.riskType : "";
                    if (!string.IsNullOrWhiteSpace(riskType))
                    {
                        sql += " and TypesOfRisk like '%" + riskType.Trim() + "%'";
                    }

                    string riskGrade = queryJson.Contains("riskGrade") ? dy.riskGrade : "";
                    if (!string.IsNullOrWhiteSpace(riskGrade))
                    {
                        sql += " and grade like '%" + riskGrade.Trim() + "%'";
                    }

                }
                else
                {
                    string process = queryJson.Contains("process") ? dy.process : "";
                    string dangerSource = queryJson.Contains("dangerSource") ? dy.dangerSource : "";
                    string riskDesc = queryJson.Contains("riskDesc") ? dy.riskDesc : "";
                    string grade = queryJson.Contains("grade") ? dy.grade : "";
                    string result = queryJson.Contains("result") ? dy.result : "";

                    if (!string.IsNullOrWhiteSpace(process))
                    {
                        sql += " and process like '%" + process.Trim() + "%'";
                    }
                    if (!string.IsNullOrWhiteSpace(dangerSource))
                    {
                        sql += " and dangerSource like '%" + dangerSource.Trim() + "%'";
                    }
                    if (!string.IsNullOrWhiteSpace(riskDesc))
                    {
                        sql += " and riskDesc like '%" + riskDesc.Trim() + "%'";
                    }
                    if (!string.IsNullOrWhiteSpace(grade))
                    {
                        sql += " and grade like '%" + grade.Trim() + "%'";
                    }
                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        sql += " and result like '%" + result.Trim() + "%'";
                    }
                }
                DepartmentBLL deptBll = new DepartmentBLL();
                DataTable dt = deptBll.GetDataTable(string.Format("select count(1)  from BIS_RISKASSESS t where " + sql));
                decimal p_totalRecords = decimal.Parse(dt.Rows[0][0].ToString());
                decimal p_totalPages = 0;
                if (p_totalRecords % pageSize == 0)
                {
                    p_totalPages = p_totalRecords / int.Parse(pageSize.ToString());
                    p_totalPages = Math.Round(p_totalPages, 0);
                }
                else
                {
                    p_totalPages = p_totalRecords / pageSize + 1;
                    p_totalPages = Math.Round(p_totalPages, 0);
                }
                if (pageIndex < 1)
                {
                    pageIndex = 1;
                }
                if (pageIndex > p_totalPages)
                {
                    pageIndex = long.Parse(p_totalPages.ToString());
                }
                //--实现分页查询
                v_startRecord = (int.Parse(pageIndex.ToString()) - 1) * int.Parse(pageSize.ToString()) + 1;
                v_endRecord = int.Parse(pageIndex.ToString()) * int.Parse(pageSize.ToString());
                List<object> list = new List<object>();



                //如果是广西华昇版本
                if (!string.IsNullOrWhiteSpace(isGxhs))
                {
                    dt = deptBll.GetDataTable(string.Format("SELECT * FROM (SELECT A.*, rownum r FROM (SELECT id,Name,WorkContent,HarmName,case when  HazardType is null then '' when HazardType is not null then (select wm_concat(b.itemname) from  (select * from base_dataitemdetail where itemid in (select itemid from base_dataitem where itemcode='HazardType')) b where instr(','||HazardType||',',','||b.itemvalue||',')>0) end  as hazardtype,HarmDescription,RiskDesc,TypesOfRisk,RiskCategory,ExposedRisk,Grade,ExistingMeasures,AdviceMeasures FROM BIS_RISKASSESS WHERE " + sql + " ORDER BY Name,id asc) A WHERE rownum <= {1}) B WHERE r >={0}", v_startRecord, v_endRecord));
                    foreach (DataRow dr in dt.Rows)
                    {
                        list.Add(new
                        {
                            workName = dr["Name"].ToString(),//作业活动
                            stepName = dr["WorkContent"].ToString(),//活动步骤
                            harmName = dr["HarmName"].ToString(),//危害名称
                            harmType = dr["hazardtype"].ToString(),//危害种类
                            harmDesc = dr["HarmDescription"].ToString(), //危害描述
                            riskDesc = dr["RiskDesc"].ToString(),//风险描述
                            riskType = dr["TypesOfRisk"].ToString(),//风险种类
                            riskRange = dr["RiskCategory"].ToString(),//风险范畴
                            exposeRisk = dr["ExposedRisk"].ToString(),//暴露于风险的人员,设备信息
                            existingMeasure = dr["ExistingMeasures"].ToString(),//现有的控制措施
                            adviceMeasure = dr["AdviceMeasures"].ToString(),//建议的控制措施
                            riskGrade = dr["Grade"].ToString() //风险等级
                        });
                    }
                }
                else
                {

                    dt = deptBll.GetDataTable(string.Format("SELECT * FROM (SELECT A.*, rownum r FROM (SELECT id,WorkTask,Process,JobName,HjEqupment,result,DangerSource,RiskDesc,DeptName,risktype,grade,areacode,districtname,ToolOrDanger FROM BIS_RISKASSESS WHERE " + sql + " ORDER BY areacode,id asc) A WHERE rownum <= {1}) B WHERE r >={0}", v_startRecord, v_endRecord));
                    foreach (DataRow dr in dt.Rows)
                    {
                        DataTable dtMeasure = deptBll.GetDataTable(string.Format("select content from BIS_MEASURES where riskid='{0}' and typename in('管理','个体防护','工程技术','应急处置')", dr[0].ToString()));
                        string[] measures = dtMeasure.AsEnumerable().Select(e => e.Field<string>("content")).ToArray();
                        StringBuilder sb = new StringBuilder();
                        int i = 1;
                        foreach (string mes in measures)
                        {
                            sb.AppendFormat("{0}.{1}", i, mes);
                            i++;
                        }
                        list.Add(new
                        {
                            id = dr["id"].ToString(),
                            areaName = dr["districtname"].ToString(),
                            process = dr["process"].ToString(),//工序
                            riskDesc = dr["RiskDesc"].ToString(),//危险因素
                            grade = dr["grade"].ToString(), //风险级别
                            result = dr["result"].ToString(),//危害后果
                            dangerSource = dr["dangerSource"].ToString(),//危险源
                            measures = sb.ToString()
                        });
                    }
                }
                if (HttpContext.Current.Request.Params["jsoncallback"] != null)
                {
                    string data = Newtonsoft.Json.JsonConvert.SerializeObject(new { code = 0, message = "获取数据成功", count = p_totalRecords, data = list });
                    return HttpContext.Current.Request.Params["jsoncallback"] + "('" + data + "')";
                }
                else
                {
                    return new { code = 0, message = "获取数据成功", count = p_totalRecords, data = list };
                }
            }
            catch (Exception ex)
            {
                return new { code = 1, message = ex.Message, count = 0 };
            }

        }
        #endregion
        #endregion

        #region 规章制度学习(终端大屏）
        /// <summary>
        /// 规章制度学习
        /// </summary>
        /// <returns></returns>
        /// 


        [System.Web.Http.HttpPost]
        public object GetNews()
        {
            try
            {
                var list = new SecurityStudyBLL().GetList("").Where(t => t.IsSend == "1").OrderByDescending(t => t.ReleaseTime).OrderByDescending(t => t.CreateDate).Take(5);
                return new { code = 0, message = "获取数据成功", count = list.Count(), data = list.Select(y => new { Id = y.Id, Content = y.Content, Title = y.Title }) };
            }
            catch (Exception ex)
            {
                return new { code = -1, message = ex.Message };
            }
        }
        #endregion

        #region 获取天气预报(终端大屏）
        /// <summary>
        /// 获取天气预报
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public object GetWeather(string url)
        {
            try
            {
                WebClient wc = new WebClient();
                wc.Credentials = CredentialCache.DefaultCredentials;
                wc.Headers.Add("Content-Type", "text/json; charset=utf-8");
                wc.Encoding = Encoding.UTF8;
                //发送请求到web api并获取返回值，默认为post方式
                string content = wc.DownloadString(new Uri(url));
                return new { code = 0, message = "获取数据成功", data = content };
            }
            catch (Exception ex)
            {
                return new { code = -1, message = ex.Message };
            }
        }
        #endregion

        #region 获取安全生产天数（西塞山和毕节大屏）
        /// <summary>
        /// 获取安全生产天数（西塞山和毕节大屏）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetSafeDays()
        {
            string days = "";
            var list = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetDataItemListByItemCode("'XSSIndex'");
            if (list.Count() > 0)
            {
                var entity = list.FirstOrDefault();
                DateTime t1 = DateTime.Parse(entity.ItemCode);
                TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - t1.Ticks);
                days = (ts.Days + int.Parse(entity.ItemValue)).ToString();
            }
            return days;
        }
        #endregion

        #region 通过safety系统中的工作内容关联云平台的作业安全分析库（华润湖北）
        /// <summary>
        /// 通过safety系统中的工作内容关联云平台的作业安全分析库
        /// </summary>
        [System.Web.Http.HttpGet]
        public object GetAreaRiskInfo(string code)
        {
            try
            {
                DepartmentBLL deptBll = new DepartmentBLL();
                DataTable dtArea = deptBll.GetDataTable(string.Format("select districtcode,districtname from BIS_DISTRICT t where t.description='{0}'", code));
                if (dtArea.Rows.Count > 0)
                {
                    string areaCode = dtArea.Rows[0][0].ToString();
                    string areaName = dtArea.Rows[0][1].ToString();
                    Hashtable ht = new Hashtable();
                    string[] arr = { "低风险", "一般风险", "较大风险", "重大风险" };

                    foreach (string key in arr)
                    {
                        DataTable dtCount = deptBll.GetDataTable(string.Format("select count(1) from BIS_RISKASSESS where status=1 and deletemark=0 and enabledmark=0 and areacode='{0}' and grade='{1}'", areaCode, key));
                        ht.Add(key, dtCount.Rows[0][0].ToString());

                    }
                    string count1 = deptBll.GetDataTable(string.Format("select count(1) from v_basehiddeninfo t where t.hidpoint='{0}' and t.rankname='{1}' and t.workstream!='整改结束' ", areaCode, "一般隐患")).Rows[0][0].ToString();
                    string count2 = deptBll.GetDataTable(string.Format("select count(1) from v_basehiddeninfo t where t.hidpoint='{0}' and t.rankname='{1}' and t.workstream!='整改结束' ", areaCode, "重大隐患")).Rows[0][0].ToString();

                    string sql = string.Format(@"select t.id,b.itemname as risktypename,t.risktype as risktypevalue,t.workdeptname,t.workplace,t.workcontent,t.worktypename,t.WorkUserNames,t.WorkDutyUserName,t.WorkTutelageUserName,t.auditusername,t.workdepttype
                                      from v_xssunderwaywork t
                                      left join base_dataitemdetail b
                                        on t.risktype = b.itemvalue
                                       and b.itemid =
                                           (select itemid from base_dataitem where itemcode = 'CommonRiskType')
                                     where  workareaname='{0}' order by risktype asc", areaName);

                    dtArea = deptBll.GetDataTable(sql);
                    return new { code = 0, message = "获取数据成功", data = new { riskInfo = ht, htInfo = new { count1 = count1, count2 = count2 }, workInfo = dtArea } };
                }
                return new { code = -1, message = "区域信息不存在" };

            }
            catch (Exception ex)
            {
                return new { code = -1, message = ex.Message };
            }
        }
        #endregion

        #region 根据区域获取风险，隐患及作业信息（国电荥阳风险四色图使用）
        /// <summary>
        ///根据区域获取风险，隐患及作业信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object GetRiskInfoByCode(string code, string orgCode = "00005001001001001")
        {
            try
            {
                DepartmentBLL deptBll = new DepartmentBLL();
                DataTable dtDis = deptBll.GetDataTable(string.Format("select d.districtcode from bis_district d where d.description='{0}'", code));
                DataTable dtRisk = new DataTable();
                DataTable dtHt = new DataTable();
                List<TodayWorkEntity> ProList = new List<TodayWorkEntity>();
                List<object> listRisk = new List<object>(); List<object> listHt = new List<object>();
                string isGdxy = new DataItemDetailBLL().GetItemValue("IsGdxy");//判断是否国电荥阳版本
                if (dtDis.Rows.Count > 0)
                {
                    string areaCode = dtDis.Rows[0][0].ToString();
                    //获取风险
                    string sql = "";
                    List<string> list = new List<string>();
                    list.Add("低风险"); list.Add("一般风险"); list.Add("较大风险"); list.Add("重大风险");
                    for (int j = 0; j < list.Count; j++)
                    {
                        string key = list[j];
                        sql = string.Format("select count(1) from bis_riskassess t where status=1 and deletemark=0 and enabledmark=0 and areacode like '{0}%' and grade='{2}' and t.deptcode like '{1}%' ", areaCode, orgCode, key);
                        if (!string.IsNullOrWhiteSpace(isGdxy))
                        {
                            sql = string.Format("select count(1) from bis_riskassess t where status=1 and deletemark=0 and enabledmark=0 and areacode like '{0}%' and grade='{2}' and risktype in('管理','设备','区域') and t.deptcode like '{1}%' ", areaCode, orgCode, key);
                        }
                        string count = deptBll.GetDataTable(sql).Rows[0][0].ToString();
                        listRisk.Add(new
                        {
                            grade = key,
                            num = count
                        });
                    }
                    //获取隐患
                    list = new List<string>();
                    list.Add("一般隐患"); list.Add("重大隐患");
                    for (int j = 0; j < list.Count; j++)
                    {
                        string key = list[j];
                        sql = string.Format("select count(1) num from  v_basehiddeninfo t where t.hidpoint like '{0}%' and t.createuserorgcode= '{1}' and rankname='{2}' and t.workstream!='整改结束' ", areaCode, orgCode, key);
                        string count = deptBll.GetDataTable(sql).Rows[0][0].ToString();
                        listHt.Add(new
                        {
                            name = key,
                            num = count
                        });
                    }
                    //获取作业信息
                    sql = string.Format(@"select t.id,b.itemname as risktypename,t.risktype as risktypevalue,t.workdeptname,t.workplace,t.workcontent,t.worktypename,t.WorkUserNames,t.WorkDutyUserName,t.WorkTutelageUserName,t.auditusername,t.workdepttype
                                      from v_xssunderwaywork t
                                      left join base_dataitemdetail b
                                        on t.risktype = b.itemvalue
                                       and b.itemid =
                                           (select itemid from base_dataitem where itemcode = 'CommonRiskType')
                                     where  workareacode='{0}' order by risktype asc", areaCode);

                    DutyDeptWork itemData = new DutyDeptWork();

                    DataTable dt = deptBll.GetDataTable(sql);
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

                }
                else
                {
                    List<string> list = new List<string>();
                    list.Add("低风险"); list.Add("一般风险"); list.Add("较大风险"); list.Add("重大风险");
                    foreach (string str in list)
                    {
                        listRisk.Add(new
                        {
                            grade = str,
                            num = 0
                        });
                    }
                    list = new List<string>();
                    list.Add("一般隐患"); list.Add("重大隐患");
                    foreach (string str in list)
                    {
                        listHt.Add(new
                        {
                            name = str,
                            num = 0
                        });
                    }

                }
                return new { code = 0, message = "获取数据成功", data = new { risk = listRisk, ht = listHt, work = ProList } };
            }
            catch (Exception ex)
            {
                return new { code = -1, message = ex.Message };
            }
        }
        #endregion

        #region 获取常用联系人（供手机app选择人员使用）
        [HttpPost]
        /// <summary>
        /// 获取常用联系人
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public object GetUsers([FromBody]JObject json)
        {
            try
            {

                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                DepartmentBLL deptBll = new DepartmentBLL();
                DataTable dtGroups = deptBll.GetDataTable(string.Format("select id,usergroupname from BIS_USERGROUPMANAGE where createuserid='{0}'", userId));
                DataTable dtUsers = deptBll.GetDataTable(string.Format("select u.userid,u.REALNAME checkperson,u.ACCOUNT,u.dutyname,u.departmentid checkdeptid,u.DEPARTMENTCODE checkdeptcode,u.DEPTNAME checkdept,u.MOBILE telphone,t.moduleid groupid from BIS_USERLISTMANAGE t left join v_userinfo u on t.userid=u.userid where t.createuserid='{0}' and  u.userid is not null and moduleid in(select id from BIS_USERGROUPMANAGE)", userId));
                return new { code = 0, info = "获取数据成功", data = new { groups = dtGroups, users = dtUsers } };
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message, };
            }
        }
        #endregion

        #region 获取培训平台对应部门ID,Code(提供给班组)
        [HttpGet]
        /// <summary>
        /// 获取培训平台对应部门ID,Code
        /// </summary>
        /// <param name="deptId">双控部门Id</param>
        /// <returns>返回培训平台对应部门的ID和Code（格式如Id|Code）</returns>
        public object GetTrainDeptInfo(string deptId)
        {
            try
            {
                DepartmentBLL deptBll = new DepartmentBLL();
                DepartmentEntity dept = deptBll.GetEntity(deptId);
                if (dept == null)
                {
                    return new { code = -1, info = "部门信息不存在!" };
                }
                else
                {
                    DataTable dtDept = deptBll.GetDataTable(string.Format("select PX_DEPTID from XSS_DEPT where DEPTID='{0}'", deptId));
                    if (dtDept.Rows.Count == 0)
                    {
                        return new { code = -1, info = "该部门没有配置部门关系", data = deptId };
                    }
                    else
                    {
                        string id = dtDept.Rows[0][0].ToString();
                        dtDept.Dispose();
                        if (string.IsNullOrEmpty(id))
                        {
                            return new { code = 0, info = "获取数据成功", data = deptId };
                        }
                        else
                        {
                            return new { code = 0, info = "获取数据成功", data = id };
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message };
            }
        }
        #endregion

        #region 获取应急计划,满足条件则自动创建应急演练记录
        [HttpGet]
        /// <summary>
        /// 获取应急计划,满足条件则自动创建应急演练记录
        /// </summary>
        /// <param name="deptId">双控部门Id</param>
        /// <returns>返回培训平台对应部门的ID和Code（格式如Id|Code）</returns>
        public object LoadDrillPlanToCreateRecord()
        {
            try
            {

                string strwhere = string.Format(@"  and  (plantime -7) <= to_date('{0}','yyyy-mm-dd hh24:mi:ss')  and  plantime  >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", DateTime.Now.ToString("yyyy-MM-dd"));

                List<DrillplanEntity> list = drillplanbll.GetList(strwhere).ToList(); //满足条件的所有应急演练计划

                string sql = string.Format(@" select distinct  a.drillplanid  from  mae_drillplanrecord a left join V_mae_Drillplan b  on a.Drillplanid = b.id where 1=1  and a.IsConnectPlan='是'");

                List<string> ids = new List<string>();

                DataTable dt = drillplanrecordbll.GetBZList(sql);

                foreach (DataRow row in dt.Rows)
                {
                    ids.Add(row["drillplanid"].ToString());
                }
                //排除已经存在记录的应急计划
                list = list.Where(p => !ids.Contains(p.ID)).ToList();

                foreach (DrillplanEntity entity in list)
                {
                    string pushcode = "YJYL001"; //待完善应急演练记录
                    string planmonth = entity.PLANTIME.Value.ToString("yyyy年MM月");
                    string message = planmonth + "的演练计划即将开始，请按计划时间进行应急演练并完善演练记录";
                    //执行人
                    if (!string.IsNullOrEmpty(entity.EXECUTEPERSONID))
                    {
                        //获取执行人对象
                        UserEntity userEntity = userbll.GetEntity(entity.EXECUTEPERSONID);
                        //新增应急演练记录信息
                        DrillplanrecordEntity rentity = new DrillplanrecordEntity();
                        rentity.DRILLPLANID = entity.ID;
                        rentity.DRILLPLANNAME = entity.NAME;
                        rentity.IsCommit = 0;
                        rentity.IsConnectPlan = "是";
                        rentity.OrgDept = entity.OrgDept;
                        rentity.OrgDeptCode = entity.OrgDeptCode;
                        rentity.OrgDeptId = entity.OrgDeptId;
                        rentity.DEPARTNAME = entity.DEPARTNAME;
                        rentity.DEPARTID = entity.DEPARTID;
                        rentity.DRILLTYPE = entity.DRILLTYPE;
                        rentity.DRILLTYPENAME = entity.DRILLTYPENAME;
                        rentity.DRILLMODE = entity.DRILLMODE;
                        rentity.DRILLMODENAME = entity.DRILLMODENAME;
                        rentity.CREATEUSERID = userEntity.UserId;
                        rentity.CREATEUSERNAME = userEntity.RealName;
                        rentity.CREATEUSERDEPTCODE = userEntity.DepartmentCode;
                        rentity.CREATEUSERORGCODE = userEntity.OrganizeCode;
                        rentity.DRILLTIME = DateTime.Now;
                        drillplanrecordbll.SaveForm("", rentity);

                        JPushApi.PushMessage(userEntity.Account, userEntity.RealName, pushcode, "应急演练记录完善", message, rentity.ID);

                    }
                }


                return new { code = 0, info = "获取数据成功" };

            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message };
            }
        }
        #endregion

        #region 定时推送隐患排查
        /// <summary>
        /// 定时消息推送
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object WfPushMessageToPerson()
        {
            try
            {
                string queryJson = string.Empty; //参数

                string pushcode = string.Empty; //推送代码

                string pushaccount = string.Empty; //推送账户

                string pushname = string.Empty; //推送人员姓名

                string message = string.Empty; //推送信息

                List<MessagePushRecordEntity> list = new List<MessagePushRecordEntity>();

                //即将到期未评估
                #region 即将到期未评估
                string sql = string.Format(@"select a.* from (
                                                select a.id,a.hiddescribe, c.realname curapproveperson,d.fullname curapprovedeptname, b.createdate curapprovedate,
                                                (b.createdate +  e.beforeapprove / 24) beforeapprovedate,(b.createdate +  e.afterapprove/24) afterapprovedate ,f.participant,f.participantname,a.workstream
                                                from bis_htbaseinfo a
                                                left join v_currenthtapprove b on a.id = b.objectid
                                                left join base_user c on b.createuser = c.account 
                                                left join base_department d on c.departmentid = d.departmentid
                                                left join (select beforeapprove,afterapprove,organizeid from bis_expirationtimesetting where modulename ='Hidden' ) e on a.hiddepart = e.organizeid
                                                left join v_workflow f on a.id = f.id 
                                                where 1=1 and a.workstream ='隐患评估'
                                            ) a where  to_date('{0}','yyyy-mm-dd hh24:mi:ss') >= beforeapprovedate  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') <= afterapprovedate", DateTime.Now);

                var jjdqwpgDt = htbaseinfobll.GetGeneralQueryBySql(sql);

                //即将到期未评估
                foreach (DataRow row in jjdqwpgDt.Rows)
                {

                    queryJson = "{";
                    string tempstr = string.Format("\"workstream\":\"隐患评估\",\"pushcode\":\"YH018\",\"sourcedate\":\"{0}\",\"mark\":\"beforeapprove\",\"relvanceid\":\"{1}\"", Convert.ToDateTime(row["curapprovedate"].ToString()), row["id"].ToString());
                    queryJson += tempstr;
                    queryJson += "}";

                    list = new MessagePushRecordBLL().GetList(queryJson).ToList();
                    //查询无对应的结果后，开始推送，并保存相应的推送记录
                    if (list.Count() == 0)
                    {
                        string keyValue = row["id"].ToString();
                        //推送到评估人
                        pushcode = "YH018";
                        pushaccount = row["participant"].ToString().Replace("$", "");
                        pushname = row["participantname"].ToString();
                        message = string.Format(@"您好，{0} {1}于{2}提交的隐患《{3}》，需要您进行评估，该隐患评估即将于“{4}”逾期，请您及时进行处理。",
                           row["curapprovedeptname"].ToString(), row["curapproveperson"].ToString(),
                           DateTime.Parse(row["curapprovedate"].ToString()).ToString("yyyy-MM-dd HH:mm"), row["hiddescribe"].ToString(), Convert.ToDateTime(row["afterapprovedate"].ToString()).ToString("yyyy-MM-dd HH:mm"));

                        bool isOk = JPushApi.PushMessage(pushaccount, pushname, pushcode, "即将到期未评估消息", message, keyValue);
                        //新增推送记录
                        MessagePushRecordEntity entity = new MessagePushRecordEntity();
                        entity.RELVANCEID = keyValue;
                        entity.SOURCEDATE = Convert.ToDateTime(row["curapprovedate"].ToString());
                        entity.EXECNUM += 1;
                        entity.MARK = "beforeapprove";
                        entity.PUSHACCOUNT = pushaccount;
                        entity.PUSHCODE = pushcode;
                        entity.WORKFLOW = "隐患评估";
                        new MessagePushRecordBLL().SaveForm("", entity);

                    }
                }
                #endregion

                //逾期未评估
                #region 逾期未评估 到负责人
                sql = string.Format(@"select a.* from (
                                                    select a.id,a.hiddescribe, b.createdate curapprovedate,
                                                    (b.createdate +  e.beforeapprove / 24) beforeapprovedate,(b.createdate +  e.afterapprove/24) afterapprovedate ,f.participant,f.participantname,a.workstream
                                                    from bis_htbaseinfo a
                                                    left join v_currenthtapprove b on a.id = b.objectid
                                                    left join (select beforeapprove,afterapprove,organizeid from bis_expirationtimesetting where modulename ='Hidden' ) e on a.hiddepart = e.organizeid
                                                    left join v_workflow f on a.id = f.id 
                                                    where 1=1 and a.workstream ='隐患评估'
                                            ) a where  to_date('{0}','yyyy-mm-dd hh24:mi:ss') > afterapprovedate", DateTime.Now);

                var yqwpgDt = htbaseinfobll.GetGeneralQueryBySql(sql);

                //逾期未评估
                foreach (DataRow row in yqwpgDt.Rows)
                {
                    pushaccount = string.Empty;
                    pushname = string.Empty;
                    queryJson = "{";
                    string tempstr = string.Format("\"workstream\":\"隐患评估\",\"pushcode\":\"YH021\",\"sourcedate\":\"{0}\",\"mark\":\"afterapprovedate\",\"relvanceid\":\"{1}\"", Convert.ToDateTime(row["curapprovedate"].ToString()), row["id"].ToString());
                    queryJson += tempstr;
                    queryJson += "}";
                    list = new MessagePushRecordBLL().GetList(queryJson).ToList();
                    //查询无对应的结果后，开始推送，并保存相应的推送记录
                    if (list.Count() == 0)
                    {
                        string keyValue = row["id"].ToString();
                        //推送到评估的负责人及上一层级负责人
                        pushcode = "YH021";
                        string wfaccount = row["participant"].ToString().Replace("$", "");
                        string[] userAccounts = wfaccount.Split(',');
                        var curUserList = userbll.GetAllUserInfoList().Where(p => userAccounts.Contains(p.Account));
                        string participantdeptname = string.Empty;

                        /* 推送规则
                         	班组级用户：
                            评估人是班组级安全管理员、负责人，推送给班组负责人和上一级负责人（上一级是专业或工序的，推送给专业或工序负责人。上一级是部门的，推送给部门级负责人）
                            	专业级用户（工序级用户）
                            评估人是专业级安全管理员、专工、负责人的，推送给专业（或工序）负责人和上一级负责人（上一级是部门的，就推送给部门负责人）
                            	部门级用户：
                            评估人是部门级安全管理员、专工、负责人，推送给部门级负责人及厂级部门负责人。
                            	厂级部门用户：
                            评估人是厂级部门安全管理员、专工、负责人，推送给厂级部门负责人及分管安全领导
                            	公司领导：
                            评估人是公司领导，推送给评估人本人。
                         */
                        foreach (UserInfoEntity curuserentity in curUserList)
                        {
                            IList<UserInfoEntity> curulist = new List<UserInfoEntity>();
                            if (!participantdeptname.Contains(curuserentity.DeptName))
                            {
                                participantdeptname += curuserentity.DeptName + "、";
                            }

                            //班组、专业推送当前负责人及上级负责人
                            if ((curuserentity.RoleName.Contains("班组级") || curuserentity.RoleName.Contains("专业级"))
                                && (curuserentity.RoleName.Contains("负责人") || curuserentity.RoleName.Contains("安全管理员")
                                || curuserentity.RoleName.Contains("专工")))
                            {
                                curulist = userbll.GetCurLevelAndHigherLevelUserByArgs(wfaccount, "负责人"); //取当前负责人及上级负责人
                            }
                            //非厂级部门用户
                            else if (curuserentity.RoleName.Contains("部门级") && !curuserentity.RoleName.Contains("厂级") &&
                                (curuserentity.RoleName.Contains("负责人") || curuserentity.RoleName.Contains("安全管理员")
                              || curuserentity.RoleName.Contains("专工")))
                            {
                                //取厂级负责人
                                curulist = userbll.GetWFUserListByDeptRoleOrg(curuserentity.OrganizeId, string.Empty, string.Empty, string.Empty, string.Empty, "厂级部门用户&负责人");
                                //取本部门负责人
                                var tempuserlist = userbll.GetWFUserListByDeptRoleOrg(curuserentity.OrganizeId, curuserentity.DepartmentId, string.Empty, string.Empty, string.Empty, "负责人");
                                foreach (UserInfoEntity tempuserinfo in tempuserlist)
                                {
                                    if (curulist.Where(p => p.UserId == tempuserinfo.UserId).Count() == 0)
                                    {
                                        curulist.Add(tempuserinfo);
                                    }
                                }
                            }
                            //厂级部门用户
                            else if (curuserentity.RoleName.Contains("部门级") && curuserentity.RoleName.Contains("厂级") &&
                                 (curuserentity.RoleName.Contains("负责人") || curuserentity.RoleName.Contains("安全管理员")
                               || curuserentity.RoleName.Contains("专工")))
                            {
                                //取公司安全领导
                                curulist = userbll.GetWFUserListByDeptRoleOrg(curuserentity.OrganizeId, string.Empty, string.Empty, string.Empty, string.Empty, "公司领导&安全管理员");
                                //取本部门负责人
                                var tempuserlist = userbll.GetWFUserListByDeptRoleOrg(curuserentity.OrganizeId, curuserentity.DepartmentId, string.Empty, string.Empty, string.Empty, "负责人");
                                foreach (UserInfoEntity tempuserinfo in tempuserlist)
                                {
                                    if (curulist.Where(p => p.UserId == tempuserinfo.UserId).Count() == 0)
                                    {
                                        curulist.Add(tempuserinfo);
                                    }
                                }
                            }
                            //公司级取自己
                            else if (curuserentity.RoleName.Contains("公司级"))
                            {
                                curulist.Add(curuserentity);
                            }

                            foreach (UserInfoEntity lastuser in curulist)
                            {
                                if (!pushaccount.Contains(lastuser.Account + ","))
                                {
                                    pushaccount += lastuser.Account + ",";
                                    pushname += lastuser.RealName + ",";
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(participantdeptname))
                        {
                            participantdeptname = participantdeptname.Substring(0, participantdeptname.Length - 1);
                        }
                        if (!string.IsNullOrEmpty(pushaccount)) { pushaccount = pushaccount.Substring(0, pushaccount.Length - 1); }
                        if (!string.IsNullOrEmpty(pushname)) { pushname = pushname.Substring(0, pushname.Length - 1); }
                        message = string.Format(@"您好，{0} {1}负责评估的隐患《{2}》，未按时进行评估，已于“{3}”逾期，请您知晓并及时督办。",
                          participantdeptname, row["participantname"].ToString(), row["hiddescribe"].ToString(), Convert.ToDateTime(row["afterapprovedate"].ToString()).ToString("yyyy-MM-dd HH:mm"));

                        bool isOk = JPushApi.PushMessage(pushaccount, pushname, pushcode, "逾期未评估消息", message, keyValue);
                        //新增推送记录
                        MessagePushRecordEntity entity = new MessagePushRecordEntity();
                        entity.RELVANCEID = keyValue;
                        entity.SOURCEDATE = Convert.ToDateTime(row["curapprovedate"].ToString());
                        entity.EXECNUM += 1;
                        entity.MARK = "afterapprovedate";
                        entity.PUSHACCOUNT = pushaccount;
                        entity.PUSHCODE = pushcode;
                        entity.WORKFLOW = "隐患评估";
                        new MessagePushRecordBLL().SaveForm("", entity);
                    }
                }
                #endregion

                //即将到期未验收
                #region 即将到期未验收
                sql = string.Format(@"select a.* from (
                                                select a.id,a.hiddescribe, b.createdate curacceptdate,c.realname curacceptperson,d.fullname curacceptdeptname,
                                                (b.createdate + e.beforeaccept/24) beforeacceptdate,(b.createdate + e.afteraccept/24) afteracceptdate ,f.participant,f.participantname,a.workstream
                                                from bis_htbaseinfo a
                                                left join v_currenthtaccept b on a.id = b.objectid
                                                left join base_user c on b.createuser = c.account 
                                                left join base_department d on c.departmentid = d.departmentid
                                                left join (select beforeaccept,afteraccept,organizeid from bis_expirationtimesetting where modulename ='Hidden') e on a.hiddepart = e.organizeid
                                                left join v_workflow f on a.id = f.id 
                                                where 1=1 and a.workstream ='隐患验收'
                                            ) a where  to_date('{0}','yyyy-mm-dd hh24:mi:ss') >= beforeacceptdate  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') <= afteracceptdate", DateTime.Now);

                var jjdqwysDt = htbaseinfobll.GetGeneralQueryBySql(sql);

                //即将到期未验收
                foreach (DataRow row in jjdqwysDt.Rows)
                {
                    pushaccount = string.Empty;
                    pushname = string.Empty;

                    queryJson = "{";
                    string tempstr = string.Format("\"workstream\":\"隐患验收\",\"pushcode\":\"YH020\",\"sourcedate\":\"{0}\",\"mark\":\"beforeaccept\",\"relvanceid\":\"{1}\"", Convert.ToDateTime(row["curacceptdate"].ToString()), row["id"].ToString());
                    queryJson += tempstr;
                    queryJson += "}";

                    list = new MessagePushRecordBLL().GetList(queryJson).ToList();
                    //查询无对应的结果后，开始推送，并保存相应的推送记录
                    if (list.Count() == 0)
                    {
                        string keyValue = row["id"].ToString();
                        //推送到验收人
                        pushcode = "YH020";
                        pushaccount = row["participant"].ToString().Replace("$", "");
                        pushname = row["participantname"].ToString();
                        message = string.Format(@"您好，{0} {1}已于{2}对《{3}》隐患进行了整改，需要您进行验收，该隐患验收即将于“{4}”逾期，请您及时进行处理。",
                           row["curacceptdeptname"].ToString(), row["curacceptperson"].ToString(),
                           Convert.ToDateTime(row["curacceptdate"].ToString()).ToString("yyyy-MM-dd HH:mm"), row["hiddescribe"].ToString(), Convert.ToDateTime(row["afteracceptdate"].ToString()).ToString("yyyy-MM-dd HH:mm"));

                        bool isOk = JPushApi.PushMessage(pushaccount, pushname, pushcode, "即将到期未验收消息", message, keyValue);
                        //新增推送记录
                        MessagePushRecordEntity entity = new MessagePushRecordEntity();
                        entity.RELVANCEID = keyValue;
                        entity.SOURCEDATE = Convert.ToDateTime(row["curacceptdate"].ToString());
                        entity.EXECNUM += 1;
                        entity.MARK = "beforeaccept";
                        entity.PUSHACCOUNT = pushaccount;
                        entity.PUSHCODE = pushcode;
                        entity.WORKFLOW = "隐患验收";
                        new MessagePushRecordBLL().SaveForm("", entity);

                    }
                }
                #endregion

                //逾期未验收
                #region 逾期未验收
                sql = string.Format(@"select a.* from (
                                                select a.id,a.hiddescribe, b.createdate curacceptdate,
                                                (b.createdate + e.beforeaccept/24) beforeacceptdate,(b.createdate + e.afteraccept/24) afteracceptdate ,f.participant,f.participantname,a.workstream
                                                from bis_htbaseinfo a
                                                left join v_currenthtaccept b on a.id = b.objectid
                                                left join (select beforeaccept,afteraccept,organizeid from bis_expirationtimesetting where modulename ='Hidden') e on a.hiddepart = e.organizeid
                                                left join v_workflow f on a.id = f.id 
                                                where 1=1 and a.workstream ='隐患验收'
                                            ) a where  to_date('{0}','yyyy-mm-dd hh24:mi:ss') > afteracceptdate", DateTime.Now);

                var yqwysDt = htbaseinfobll.GetGeneralQueryBySql(sql);

                //逾期未验收
                foreach (DataRow row in yqwysDt.Rows)
                {
                    pushaccount = string.Empty;
                    pushname = string.Empty;

                    queryJson = "{";
                    string tempstr = string.Format("\"workstream\":\"隐患验收\",\"pushcode\":\"YH023\",\"sourcedate\":\"{0}\",\"mark\":\"afteraccept\",\"relvanceid\":\"{1}\"", Convert.ToDateTime(row["curacceptdate"].ToString()), row["id"].ToString());
                    queryJson += tempstr;
                    queryJson += "}";

                    list = new MessagePushRecordBLL().GetList(queryJson).ToList();
                    //查询无对应的结果后，开始推送，并保存相应的推送记录
                    if (list.Count() == 0)
                    {
                        string keyValue = row["id"].ToString();
                        //推送到验收负责人
                        pushcode = "YH023";
                        string wfaccount = row["participant"].ToString().Replace("$", "");
                        string[] userAccounts = wfaccount.Split(',');
                        var curUserList = userbll.GetAllUserInfoList().Where(p => userAccounts.Contains(p.Account));
                        string participantdeptname = string.Empty;
                        /*
                         	班组级用户：
                            验收人是除班组级负责人外的其他角色，推送给班组负责人
                            验收人是班组负责人，推送给上一级负责人（上一级是专业或工序的，推送给专业或工序负责人。上一级是部门的，推送给部门级负责人）。
                            	专业级用户（工序级用户）
                            验收人是除专业（或工序）负责人以外的其他角色，推送给专业（或工序）负责人和上一级负责人（上一级是部门的，就推送给部门负责人）
                            验收人是专业（或工序）负责人的，推送给上一级负责人（上一级是部门的，就推送给部门负责人）
                            	部门级用户：
                            验收人是除部门级负责人外的其他角色，推送给部门级负责人。
                            验收人是部门级负责人，推送给上一级负责人（上一级是厂级部门的，推送给厂级部门负责人）。
                            	厂级部门用户：
                            验收人是除厂级部门负责人外的其他角色，推送给厂级部门负责人
                            验收人是厂级部门负责人，推送给分管安全领导
                            	公司领导：
                            验收人是公司领导，推送给验收人本人。

                         */
                        foreach (UserInfoEntity curuserentity in curUserList)
                        {
                            IList<UserInfoEntity> curulist = new List<UserInfoEntity>();
                            if (!participantdeptname.Contains(curuserentity.DeptName))
                            {
                                participantdeptname += curuserentity.DeptName + "、";
                            }

                            //班组、专业、部门非负责人推送当前负责人
                            if ((curuserentity.RoleName.Contains("班组级") || curuserentity.RoleName.Contains("专业级") || curuserentity.RoleName.Contains("部门级"))
                                && !curuserentity.RoleName.Contains("负责人"))
                            {
                                curulist = userbll.GetWFUserListByDeptRoleOrg(curuserentity.OrganizeId, curuserentity.DepartmentId, string.Empty, string.Empty, string.Empty, "负责人"); //取当前负责人
                            }
                            else if ((curuserentity.RoleName.Contains("班组级") || curuserentity.RoleName.Contains("专业级")) && curuserentity.RoleName.Contains("负责人"))
                            {
                                curulist = userbll.GetWFUserListByDeptRoleOrg(curuserentity.OrganizeId, curuserentity.ParentId, string.Empty, string.Empty, string.Empty, "负责人"); //取上级单位负责人
                            }
                            else if (curuserentity.RoleName.Contains("部门级") && !curuserentity.RoleName.Contains("厂级") && curuserentity.RoleName.Contains("负责人"))
                            {
                                curulist = userbll.GetWFUserListByDeptRoleOrg(curuserentity.OrganizeId, string.Empty, string.Empty, string.Empty, string.Empty, "厂级部门用户&负责人"); //取厂级负责人
                            }
                            else if (curuserentity.RoleName.Contains("部门级") && curuserentity.RoleName.Contains("厂级") && curuserentity.RoleName.Contains("负责人"))
                            {
                                curulist = userbll.GetWFUserListByDeptRoleOrg(curuserentity.OrganizeId, string.Empty, string.Empty, string.Empty, string.Empty, "公司领导&安全管理员"); //取厂级负责人
                            }
                            //公司级取自己
                            else if (curuserentity.RoleName.Contains("公司级"))
                            {
                                curulist.Add(curuserentity);
                            }


                            foreach (UserInfoEntity lastuser in curulist)
                            {
                                if (!pushaccount.Contains(lastuser.Account + ","))
                                {
                                    pushaccount += lastuser.Account + ",";
                                    pushname += lastuser.RealName + ",";
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(participantdeptname))
                        {
                            participantdeptname = participantdeptname.Substring(0, participantdeptname.Length - 1);
                        }
                        if (!string.IsNullOrEmpty(pushaccount)) { pushaccount = pushaccount.Substring(0, pushaccount.Length - 1); }
                        if (!string.IsNullOrEmpty(pushname)) { pushname = pushname.Substring(0, pushname.Length - 1); }
                        message = string.Format(@"您好，{0} {1}负责验收的隐患《{2}》，未按时进行验收，已于“{3}”逾期，请您知晓并及时督办。",
                           participantdeptname, row["participantname"].ToString(), row["hiddescribe"].ToString(), Convert.ToDateTime(row["afteracceptdate"].ToString()).ToString("yyyy-MM-dd HH:mm"));

                        bool isOk = JPushApi.PushMessage(pushaccount, pushname, pushcode, "逾期未验收消息", message, keyValue);
                        //新增推送记录
                        MessagePushRecordEntity entity = new MessagePushRecordEntity();
                        entity.RELVANCEID = keyValue;
                        entity.SOURCEDATE = Convert.ToDateTime(row["curacceptdate"].ToString());
                        entity.EXECNUM += 1;
                        entity.MARK = "afteraccept";
                        entity.PUSHACCOUNT = pushaccount;
                        entity.PUSHCODE = pushcode;
                        entity.WORKFLOW = "隐患验收";
                        new MessagePushRecordBLL().SaveForm("", entity);
                    }
                }
                #endregion

                //即将到期未整改
                #region 即将到期未整改

                sql = string.Format(@"select * from (
                                              select a.id,a.hiddescribe,a.workstream,substr(b.itemname,length(b.itemname)-3) as rankname,c.changedeadine,c.changeperson,c.changepersonname,
                                              c.changedutydepartcode ,c.changedutydepartname ,d.account from bis_htbaseinfo a
                                              left join base_dataitemdetail  b on a.hidrank = b.itemdetailid
                                              left join v_htchangeinfo c on a.hidcode = c.hidcode 
                                              left join base_user d on c.changeperson =d.userid 
                                     ) a where  a.workstream ='隐患整改' and  ((rankname = '一般隐患' and changedeadine - 3 <= to_date('{0}','yyyy-mm-dd hh24:mi:ss')  and to_date('{0}','yyyy-mm-dd hh24:mi:ss') <= changedeadine + 1 )  or 
                                      (rankname = '重大隐患' and changedeadine - 5 <= to_date('{0}','yyyy-mm-dd hh24:mi:ss') and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') <= changedeadine + 1 ) )", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                var jjdqwzgDt = htbaseinfobll.GetGeneralQueryBySql(sql);

                //即将到期未整改
                foreach (DataRow row in jjdqwzgDt.Rows)
                {
                    pushaccount = string.Empty;
                    pushname = string.Empty;

                    queryJson = "{";
                    string tempstr = string.Format("\"workstream\":\"隐患整改\",\"pushcode\":\"YH019\",\"pushaccount\":\"{0}\",\"mark\":\"beforechange\",\"relvanceid\":\"{1}\"", row["account"].ToString(), row["id"].ToString());
                    queryJson += tempstr;
                    queryJson += "}";

                    list = new MessagePushRecordBLL().GetList(queryJson).ToList();
                    //查询无对应的结果后，开始推送，并保存相应的推送记录
                    if (list.Count() == 0)
                    {
                        string keyValue = row["id"].ToString();
                        //推送到整改人
                        pushcode = "YH019";
                        pushaccount = row["account"].ToString().Replace("$", "");
                        pushname = row["changepersonname"].ToString();
                        message = string.Format(@"您好，您负责整改的《{0}》隐患即将于“{1}”逾期，请您及时进行整改；若因某些客观原因，导致不能按时整改的，请您及时进行延期申请。",
                           row["hiddescribe"].ToString(), Convert.ToDateTime(row["changedeadine"].ToString()).ToString("yyyy-MM-dd HH:mm"));

                        bool isOk = JPushApi.PushMessage(pushaccount, pushname, pushcode, "即将到期未整改消息", message, keyValue);
                        //新增推送记录
                        MessagePushRecordEntity entity = new MessagePushRecordEntity();
                        entity.RELVANCEID = keyValue;
                        entity.SOURCEDATE = DateTime.Now;
                        entity.EXECNUM += 1;
                        entity.MARK = "beforechange";
                        entity.PUSHACCOUNT = row["account"].ToString();
                        entity.PUSHCODE = pushcode;
                        entity.WORKFLOW = "隐患整改";
                        new MessagePushRecordBLL().SaveForm("", entity);

                    }
                }
                #endregion

                //逾期未整改
                #region 逾期未整改

                sql = string.Format(@"select * from (
                                              select a.id,a.hiddescribe,a.workstream,substr(b.itemname,length(b.itemname)-3) as rankname,c.changedeadine,c.changeperson,c.changepersonname,
                                              c.changedutydepartcode ,c.changedutydepartname ,d.account from bis_htbaseinfo a
                                              left join base_dataitemdetail  b on a.hidrank = b.itemdetailid
                                              left join v_htchangeinfo c on a.hidcode = c.hidcode
                                              left join base_user d on c.changeperson =d.userid 
                                     ) a  where  a.workstream ='隐患整改' and   to_date('{0}','yyyy-mm-dd hh24:mi:ss') > changedeadine + 1", DateTime.Now);
                var yqwzgDt = htbaseinfobll.GetGeneralQueryBySql(sql);

                //逾期未整改
                foreach (DataRow row in yqwzgDt.Rows)
                {
                    pushaccount = string.Empty;
                    pushname = string.Empty;

                    queryJson = "{";
                    string tempstr = string.Format("\"workstream\":\"隐患整改\",\"pushcode\":\"YH022\",\"pushaccount\":\"{0}\",\"mark\":\"afterchange\",\"relvanceid\":\"{1}\"", row["account"].ToString(), row["id"].ToString());
                    queryJson += tempstr;
                    queryJson += "}";

                    list = new MessagePushRecordBLL().GetList(queryJson).ToList();
                    //查询无对应的结果后，开始推送，并保存相应的推送记录
                    if (list.Count() == 0)
                    {
                        string keyValue = row["id"].ToString();
                        //推送到整改人
                        pushcode = "YH022";
                        UserInfoEntity changUser = userbll.GetUserInfoEntity(row["changeperson"].ToString()); //整改人对象

                        IList<UserInfoEntity> userlist = new List<UserInfoEntity>();

                        //取本部门负责人
                        if ((changUser.RoleName.Contains("班组级") || changUser.RoleName.Contains("专业级") || changUser.RoleName.Contains("部门级")) && !changUser.RoleName.Contains("负责人"))
                        {
                            //取本部门负责人
                            userlist = userbll.GetWFUserListByDeptRoleOrg(changUser.OrganizeId, changUser.DepartmentId, string.Empty, string.Empty, string.Empty, "负责人");
                        }
                        else if ((changUser.RoleName.Contains("班组级") || changUser.RoleName.Contains("专业级")) && changUser.RoleName.Contains("负责人"))
                        {
                            //取上级部门负责人
                            userlist = userbll.GetWFUserListByDeptRoleOrg(changUser.OrganizeId, changUser.ParentId, string.Empty, string.Empty, string.Empty, "负责人");
                        }
                        //非厂级负责人
                        else if (changUser.RoleName.Contains("部门级") && !changUser.RoleName.Contains("厂级") && changUser.RoleName.Contains("负责人"))
                        {
                            //取厂级负责人
                            userlist = userbll.GetWFUserListByDeptRoleOrg(changUser.OrganizeId, string.Empty, string.Empty, string.Empty, string.Empty, "厂级部门用户&负责人");
                        }
                        //厂级负责人
                        else if (changUser.RoleName.Contains("厂级") && changUser.RoleName.Contains("负责人"))
                        {
                            //取分管安全领导
                            userlist = userbll.GetWFUserListByDeptRoleOrg(changUser.OrganizeId, string.Empty, string.Empty, string.Empty, string.Empty, "公司领导&安全管理员");
                        }
                        //公司级取自己
                        else if (changUser.RoleName.Contains("公司级"))
                        {
                            userlist.Add(changUser);
                        }
                        //获取评估人所属的单位负责人及上级负责人
                        foreach (UserInfoEntity userEntity in userlist)
                        {
                            if (!pushaccount.Contains(userEntity.Account + ","))
                            {
                                pushaccount += userEntity.Account + ",";
                                pushname += userEntity.RealName + ",";
                            }
                        }
                        if (!string.IsNullOrEmpty(pushaccount)) { pushaccount = pushaccount.Substring(0, pushaccount.Length - 1); }
                        if (!string.IsNullOrEmpty(pushname)) { pushname = pushname.Substring(0, pushname.Length - 1); }
                        message = string.Format(@"您好，{0} {1}负责整改的《{2}》隐患未按时进行整改，已于“{3}”逾期，请您知晓并及时督办", row["changedutydepartname"].ToString(), row["changepersonname"].ToString(),
                           row["hiddescribe"].ToString(), Convert.ToDateTime(row["changedeadine"].ToString()).ToString("yyyy-MM-dd HH:mm"));

                        bool isOk = JPushApi.PushMessage(pushaccount, pushname, pushcode, "逾期未整改消息", message, keyValue);
                        //新增推送记录
                        MessagePushRecordEntity entity = new MessagePushRecordEntity();
                        entity.RELVANCEID = keyValue;
                        entity.SOURCEDATE = DateTime.Now;
                        entity.EXECNUM += 1;
                        entity.MARK = "afterchange";
                        entity.PUSHACCOUNT = row["account"].ToString();
                        entity.PUSHCODE = pushcode;
                        entity.WORKFLOW = "隐患整改";
                        new MessagePushRecordBLL().SaveForm("", entity);

                    }
                }
                #endregion

                return new { code = 0, info = "获取数据成功" };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message };
            }
        }
        #endregion

        #region 五定安全检查定时检查问题整改超期推送
        [HttpGet]
        public object FiveSafePushMessageToPerson()
        {
            try
            {
                string queryJson = string.Empty; //参数

                string pushcode = string.Empty; //推送代码

                string pushaccount = string.Empty; //推送账户

                string pushname = string.Empty; //推送人员姓名

                string message = string.Empty; //推送信息

                List<MessagePushRecordEntity> list = new List<MessagePushRecordEntity>();


                #region 整改时间到期自动报警提醒并推送到整改责任人
                string sql = string.Format(@"select id, dutyuserid, dutyusername, finishdate,usr.account,findquestion
                                              from bis_fivesafetycheckaudit t inner join  base_user usr on t.dutyuserid = usr.userid
                                             where t.checkpass = '1'
                                               and t.checkid in
                                                   (select id from bis_fivesafetycheck where isover in ('1', '2'))
                                               and (t.actionresult <> '0' or t.actionresult is null)
                                               and finishdate < sysdate");

                var zgdt = htbaseinfobll.GetGeneralQueryBySql(sql);

                foreach (DataRow dr in zgdt.Rows)
                {

                    try
                    {
                        queryJson = "{";
                        string tempstr = string.Format("\"workstream\":\"安全检查问题待整改逾期\",\"pushcode\":\"WDJC002\",\"sourcedate\":\"{0}\",\"mark\":\"beforeapprove\",\"relvanceid\":\"{1}\"", Convert.ToDateTime(dr["finishdate"].ToString()), dr["id"].ToString());
                        queryJson += tempstr;
                        queryJson += "}";

                        list = new MessagePushRecordBLL().GetList(queryJson).ToList();
                        //查询无对应的结果后，开始推送，并保存相应的推送记录
                        if (list.Count() == 0)
                        {
                            string keyValue = dr["id"].ToString();
                            //推送到核准人
                            pushcode = "WDJC002";
                            pushaccount = dr["account"].ToString().Replace("$", "");
                            pushname = dr["dutyusername"].ToString();
                            message = string.Format(@"您好，您负责整改的《{0}》检查问题整改即将于“{1}”逾期，请您及时进行整改。",
                                dr["findquestion"].ToString(), Convert.ToDateTime(dr["finishdate"].ToString()).ToString("yyyy-MM-dd"));
                            bool isOk = JPushApi.PushMessage(pushaccount, pushname, pushcode, "安全检查问题待整改逾期", message, keyValue);
                            //新增推送记录
                            MessagePushRecordEntity entity = new MessagePushRecordEntity();
                            entity.RELVANCEID = keyValue;
                            entity.SOURCEDATE = Convert.ToDateTime(dr["finishdate"].ToString());
                            entity.EXECNUM += 1;
                            entity.MARK = "beforeapprove";
                            entity.PUSHACCOUNT = pushaccount;
                            entity.PUSHCODE = pushcode;
                            entity.WORKFLOW = "安全考核整改";
                            new MessagePushRecordBLL().SaveForm("", entity);

                        }
                    }
                    catch (Exception ex)
                    {


                    }


                }
                #endregion

                return new { code = 0, info = "获取数据成功" };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message };
            }
        }
        #endregion

        #region 违章流程定时消息推送
        /// <summary>
        /// 违章流程定时消息推送
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object LllegalWfPushMessageToPerson()
        {
            try
            {
                string queryJson = string.Empty; //参数

                string pushcode = string.Empty; //推送代码

                string pushaccount = string.Empty; //推送账户

                string pushname = string.Empty; //推送人员姓名

                string message = string.Empty; //推送信息

                List<MessagePushRecordEntity> list = new List<MessagePushRecordEntity>();

                //即将到期未核准
                #region 即将到期未核准
                string sql = string.Format(@"select a.* from (
                                                select a.id,a.lllegaldescribe, c.realname curapproveperson,d.fullname curapprovedeptname, b.createdate curapprovedate,
                                                (b.createdate +  e.beforeapprove / 24) beforeapprovedate,(b.createdate +  e.afterapprove/24) afterapprovedate ,f.participant,f.participantname,a.flowstate
                                                from bis_lllegalregister a
                                                left join v_currentlllegalapprove b on a.id = b.objectid
                                                left join base_user c on b.createuser = c.account 
                                                left join base_department d on c.departmentid = d.departmentid
                                                left join (select beforeapprove,afterapprove,organizeid from bis_expirationtimesetting where modulename ='Lllegal' ) e on a.belongdepartid = e.organizeid
                                                left join v_lllegalworkflow f on a.id = f.id 
                                                where 1=1 and a.flowstate in ('违章核准','违章审核')
                                            ) a where  to_date('{0}','yyyy-mm-dd hh24:mi:ss') >= beforeapprovedate  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') <= afterapprovedate", DateTime.Now);

                var jjdqwpgDt = htbaseinfobll.GetGeneralQueryBySql(sql);

                //即将到期未核准
                foreach (DataRow row in jjdqwpgDt.Rows)
                {

                    queryJson = "{";
                    string tempstr = string.Format("\"workstream\":\"" + row["flowstate"].ToString() + "\",\"pushcode\":\"WZ016\",\"sourcedate\":\"{0}\",\"mark\":\"beforeapprove\",\"relvanceid\":\"{1}\"", Convert.ToDateTime(row["curapprovedate"].ToString()), row["id"].ToString());
                    queryJson += tempstr;
                    queryJson += "}";

                    list = new MessagePushRecordBLL().GetList(queryJson).ToList();
                    //查询无对应的结果后，开始推送，并保存相应的推送记录
                    if (list.Count() == 0)
                    {
                        string keyValue = row["id"].ToString();
                        //推送到核准人
                        pushcode = "WZ016";
                        pushaccount = row["participant"].ToString().Replace("$", "");
                        pushname = row["participantname"].ToString();
                        string hezhunTitle = row["flowstate"].ToString().Contains("核准") ? "核准" : "审核";
                        message = string.Format(@"您好，{0} {1}于{2}提交的违章《{3}》，需要您进行{5}，该违章{5}即将于“{4}”逾期，请您及时进行处理。",
                           row["curapprovedeptname"].ToString(), row["curapproveperson"].ToString(),
                           DateTime.Parse(row["curapprovedate"].ToString()).ToString("yyyy-MM-dd HH:mm"), row["lllegaldescribe"].ToString(), Convert.ToDateTime(row["afterapprovedate"].ToString()).ToString("yyyy-MM-dd HH:mm"), hezhunTitle);

                        bool isOk = JPushApi.PushMessage(pushaccount, pushname, pushcode, "即将到期未" + hezhunTitle + "消息", message, keyValue);
                        //新增推送记录
                        MessagePushRecordEntity entity = new MessagePushRecordEntity();
                        entity.RELVANCEID = keyValue;
                        entity.SOURCEDATE = Convert.ToDateTime(row["curapprovedate"].ToString());
                        entity.EXECNUM += 1;
                        entity.MARK = "beforeapprove";
                        entity.PUSHACCOUNT = pushaccount;
                        entity.PUSHCODE = pushcode;
                        entity.WORKFLOW = row["flowstate"].ToString();
                        new MessagePushRecordBLL().SaveForm("", entity);

                    }
                }
                #endregion

                //逾期未核准
                #region 逾期未核准 到负责人
                sql = string.Format(@"select a.* from (
                                                    select a.id,a.lllegaldescribe, b.createdate curapprovedate,
                                                    (b.createdate +  e.beforeapprove / 24) beforeapprovedate,(b.createdate +  e.afterapprove/24) afterapprovedate ,f.participant,f.participantname,a.flowstate
                                                    from bis_lllegalregister a
                                                    left join v_currentlllegalapprove b on a.id = b.objectid
                                                    left join (select beforeapprove,afterapprove,organizeid from bis_expirationtimesetting where modulename ='Lllegal' ) e on a.belongdepartid = e.organizeid
                                                    left join v_lllegalworkflow f on a.id = f.id 
                                                    where 1=1 and a.flowstate in ('违章核准','违章审核')
                                            ) a where  to_date('{0}','yyyy-mm-dd hh24:mi:ss') > afterapprovedate", DateTime.Now);

                var yqwpgDt = htbaseinfobll.GetGeneralQueryBySql(sql);

                //逾期未核准
                foreach (DataRow row in yqwpgDt.Rows)
                {
                    pushaccount = string.Empty;
                    pushname = string.Empty;
                    queryJson = "{";
                    string tempstr = string.Format("\"workstream\":\"" + row["flowstate"].ToString() + "\",\"pushcode\":\"WZ019\",\"sourcedate\":\"{0}\",\"mark\":\"afterapprovedate\",\"relvanceid\":\"{1}\"", Convert.ToDateTime(row["curapprovedate"].ToString()), row["id"].ToString());
                    queryJson += tempstr;
                    queryJson += "}";
                    list = new MessagePushRecordBLL().GetList(queryJson).ToList();
                    //查询无对应的结果后，开始推送，并保存相应的推送记录
                    if (list.Count() == 0)
                    {
                        string keyValue = row["id"].ToString();
                        //推送到核准的负责人及上一层级负责人
                        pushcode = "WZ019";
                        string wfaccount = row["participant"].ToString().Replace("$", "");
                        string[] userAccounts = wfaccount.Split(',');
                        var curUserList = userbll.GetAllUserInfoList().Where(p => userAccounts.Contains(p.Account));
                        string participantdeptname = string.Empty;

                        /* 推送规则
                         	班组级用户：
                            评估人是班组级安全管理员、负责人，推送给班组负责人和上一级负责人（上一级是专业或工序的，推送给专业或工序负责人。上一级是部门的，推送给部门级负责人）
                            	专业级用户（工序级用户）
                            评估人是专业级安全管理员、专工、负责人的，推送给专业（或工序）负责人和上一级负责人（上一级是部门的，就推送给部门负责人）
                            	部门级用户：
                            评估人是部门级安全管理员、专工、负责人，推送给部门级负责人及厂级部门负责人。
                            	厂级部门用户：
                            评估人是厂级部门安全管理员、专工、负责人，推送给厂级部门负责人及分管安全领导
                            	公司领导：
                            评估人是公司领导，推送给评估人本人。
                         */
                        foreach (UserInfoEntity curuserentity in curUserList)
                        {
                            IList<UserInfoEntity> curulist = new List<UserInfoEntity>();
                            if (!participantdeptname.Contains(curuserentity.DeptName))
                            {
                                participantdeptname += curuserentity.DeptName + "、";
                            }

                            //班组、专业推送当前负责人及上级负责人
                            if ((curuserentity.RoleName.Contains("班组级") || curuserentity.RoleName.Contains("专业级"))
                                && (curuserentity.RoleName.Contains("负责人") || curuserentity.RoleName.Contains("安全管理员")
                                || curuserentity.RoleName.Contains("专工")))
                            {
                                curulist = userbll.GetCurLevelAndHigherLevelUserByArgs(wfaccount, "负责人"); //取当前负责人及上级负责人
                            }
                            //非厂级部门用户
                            else if (curuserentity.RoleName.Contains("部门级") && !curuserentity.RoleName.Contains("厂级") &&
                                (curuserentity.RoleName.Contains("负责人") || curuserentity.RoleName.Contains("安全管理员")
                              || curuserentity.RoleName.Contains("专工")))
                            {
                                //取厂级负责人
                                curulist = userbll.GetWFUserListByDeptRoleOrg(curuserentity.OrganizeId, string.Empty, string.Empty, string.Empty, string.Empty, "厂级部门用户&负责人");
                                //取本部门负责人
                                var tempuserlist = userbll.GetWFUserListByDeptRoleOrg(curuserentity.OrganizeId, curuserentity.DepartmentId, string.Empty, string.Empty, string.Empty, "负责人");
                                foreach (UserInfoEntity tempuserinfo in tempuserlist)
                                {
                                    if (curulist.Where(p => p.UserId == tempuserinfo.UserId).Count() == 0)
                                    {
                                        curulist.Add(tempuserinfo);
                                    }
                                }
                            }
                            //厂级部门用户
                            else if (curuserentity.RoleName.Contains("部门级") && curuserentity.RoleName.Contains("厂级") &&
                                 (curuserentity.RoleName.Contains("负责人") || curuserentity.RoleName.Contains("安全管理员")
                               || curuserentity.RoleName.Contains("专工")))
                            {
                                //取公司安全领导
                                curulist = userbll.GetWFUserListByDeptRoleOrg(curuserentity.OrganizeId, string.Empty, string.Empty, string.Empty, string.Empty, "公司领导&安全管理员");
                                //取本部门负责人
                                var tempuserlist = userbll.GetWFUserListByDeptRoleOrg(curuserentity.OrganizeId, curuserentity.DepartmentId, string.Empty, string.Empty, string.Empty, "负责人");
                                foreach (UserInfoEntity tempuserinfo in tempuserlist)
                                {
                                    if (curulist.Where(p => p.UserId == tempuserinfo.UserId).Count() == 0)
                                    {
                                        curulist.Add(tempuserinfo);
                                    }
                                }
                            }
                            //公司级取自己
                            else if (curuserentity.RoleName.Contains("公司级"))
                            {
                                curulist.Add(curuserentity);
                            }

                            foreach (UserInfoEntity lastuser in curulist)
                            {
                                if (!pushaccount.Contains(lastuser.Account + ","))
                                {
                                    pushaccount += lastuser.Account + ",";
                                    pushname += lastuser.RealName + ",";
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(participantdeptname))
                        {
                            participantdeptname = participantdeptname.Substring(0, participantdeptname.Length - 1);
                        }
                        if (!string.IsNullOrEmpty(pushaccount)) { pushaccount = pushaccount.Substring(0, pushaccount.Length - 1); }
                        if (!string.IsNullOrEmpty(pushname)) { pushname = pushname.Substring(0, pushname.Length - 1); }
                        string hezhunTitle = row["flowstate"].ToString().Contains("核准") ? "核准" : "审核";
                        message = string.Format(@"您好，{0} {1}负责{4}的违章《{2}》，未按时进行{4}，已于“{3}”逾期，请您知晓并及时督办。",
                          participantdeptname, row["participantname"].ToString(), row["lllegaldescribe"].ToString(), Convert.ToDateTime(row["afterapprovedate"].ToString()).ToString("yyyy-MM-dd HH:mm"), hezhunTitle);

                        bool isOk = JPushApi.PushMessage(pushaccount, pushname, pushcode, "逾期未" + hezhunTitle + "消息", message, keyValue);
                        //新增推送记录
                        MessagePushRecordEntity entity = new MessagePushRecordEntity();
                        entity.RELVANCEID = keyValue;
                        entity.SOURCEDATE = Convert.ToDateTime(row["curapprovedate"].ToString());
                        entity.EXECNUM += 1;
                        entity.MARK = "afterapprovedate";
                        entity.PUSHACCOUNT = pushaccount;
                        entity.PUSHCODE = pushcode;
                        entity.WORKFLOW = row["flowstate"].ToString();
                        new MessagePushRecordBLL().SaveForm("", entity);
                    }
                }
                #endregion

                //即将到期未验收
                #region 即将到期未验收
                sql = string.Format(@"select a.* from (
                                                select a.id,a.lllegaldescribe, b.createdate curacceptdate,c.realname curacceptperson,d.fullname curacceptdeptname,
                                                (b.createdate + e.beforeaccept/24) beforeacceptdate,(b.createdate + e.afteraccept/24) afteracceptdate ,f.participant,f.participantname,a.flowstate
                                                from bis_lllegalregister a
                                                left join v_currentlllegalaccept b on a.id = b.objectid
                                                left join base_user c on b.createuser = c.account 
                                                left join base_department d on c.departmentid = d.departmentid
                                                left join (select beforeaccept,afteraccept,organizeid from bis_expirationtimesetting where modulename ='Lllegal') e on a.belongdepartid = e.organizeid
                                                left join v_lllegalworkflow f on a.id = f.id 
                                                where 1=1 and a.flowstate ='违章验收'
                                            ) a where  to_date('{0}','yyyy-mm-dd hh24:mi:ss') >= beforeacceptdate  and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') <= afteracceptdate", DateTime.Now);

                var jjdqwysDt = htbaseinfobll.GetGeneralQueryBySql(sql);

                //即将到期未验收
                foreach (DataRow row in jjdqwysDt.Rows)
                {
                    pushaccount = string.Empty;
                    pushname = string.Empty;

                    queryJson = "{";
                    string tempstr = string.Format("\"workstream\":\"违章验收\",\"pushcode\":\"WZ018\",\"sourcedate\":\"{0}\",\"mark\":\"beforeaccept\",\"relvanceid\":\"{1}\"", Convert.ToDateTime(row["curacceptdate"].ToString()), row["id"].ToString());
                    queryJson += tempstr;
                    queryJson += "}";

                    list = new MessagePushRecordBLL().GetList(queryJson).ToList();
                    //查询无对应的结果后，开始推送，并保存相应的推送记录
                    if (list.Count() == 0)
                    {
                        string keyValue = row["id"].ToString();
                        //推送到验收人
                        pushcode = "WZ018";
                        pushaccount = row["participant"].ToString().Replace("$", "");
                        pushname = row["participantname"].ToString();
                        message = string.Format(@"您好，{0} {1}已于{2}对《{3}》违章进行了整改，需要您进行验收，该违章验收即将于“{4}”逾期，请您及时进行处理。",
                           row["curacceptdeptname"].ToString(), row["curacceptperson"].ToString(),
                           Convert.ToDateTime(row["curacceptdate"].ToString()).ToString("yyyy-MM-dd HH:mm"), row["lllegaldescribe"].ToString(), Convert.ToDateTime(row["afteracceptdate"].ToString()).ToString("yyyy-MM-dd HH:mm"));

                        bool isOk = JPushApi.PushMessage(pushaccount, pushname, pushcode, "即将到期未验收消息", message, keyValue);
                        //新增推送记录
                        MessagePushRecordEntity entity = new MessagePushRecordEntity();
                        entity.RELVANCEID = keyValue;
                        entity.SOURCEDATE = Convert.ToDateTime(row["curacceptdate"].ToString());
                        entity.EXECNUM += 1;
                        entity.MARK = "beforeaccept";
                        entity.PUSHACCOUNT = pushaccount;
                        entity.PUSHCODE = pushcode;
                        entity.WORKFLOW = "违章验收";
                        new MessagePushRecordBLL().SaveForm("", entity);

                    }
                }
                #endregion

                //逾期未验收
                #region 逾期未验收
                sql = string.Format(@"select a.* from (
                                                select a.id,a.lllegaldescribe, b.createdate curacceptdate,
                                                (b.createdate + e.beforeaccept/24) beforeacceptdate,(b.createdate + e.afteraccept/24) afteracceptdate ,f.participant,f.participantname,a.flowstate
                                                from bis_lllegalregister a
                                                left join v_currentlllegalaccept b on a.id = b.objectid
                                                left join (select beforeaccept,afteraccept,organizeid from bis_expirationtimesetting where modulename ='Lllegal') e on a.belongdepartid = e.organizeid
                                                left join v_lllegalworkflow f on a.id = f.id 
                                                where 1=1 and a.flowstate ='违章验收'
                                            ) a where  to_date('{0}','yyyy-mm-dd hh24:mi:ss') > afteracceptdate", DateTime.Now);

                var yqwysDt = htbaseinfobll.GetGeneralQueryBySql(sql);

                //逾期未验收
                foreach (DataRow row in yqwysDt.Rows)
                {
                    pushaccount = string.Empty;
                    pushname = string.Empty;

                    queryJson = "{";
                    string tempstr = string.Format("\"workstream\":\"违章验收\",\"pushcode\":\"WZ021\",\"sourcedate\":\"{0}\",\"mark\":\"afteraccept\",\"relvanceid\":\"{1}\"", Convert.ToDateTime(row["curacceptdate"].ToString()), row["id"].ToString());
                    queryJson += tempstr;
                    queryJson += "}";

                    list = new MessagePushRecordBLL().GetList(queryJson).ToList();
                    //查询无对应的结果后，开始推送，并保存相应的推送记录
                    if (list.Count() == 0)
                    {
                        string keyValue = row["id"].ToString();
                        pushcode = "WZ021";
                        //推送到验收负责人
                        string wfaccount = row["participant"].ToString().Replace("$", "");
                        string[] userAccounts = wfaccount.Split(',');
                        var curUserList = userbll.GetAllUserInfoList().Where(p => userAccounts.Contains(p.Account));
                        string participantdeptname = string.Empty;
                        /*
                         	班组级用户：
                            验收人是除班组级负责人外的其他角色，推送给班组负责人
                            验收人是班组负责人，推送给上一级负责人（上一级是专业或工序的，推送给专业或工序负责人。上一级是部门的，推送给部门级负责人）。
                            	专业级用户（工序级用户）
                            验收人是除专业（或工序）负责人以外的其他角色，推送给专业（或工序）负责人和上一级负责人（上一级是部门的，就推送给部门负责人）
                            验收人是专业（或工序）负责人的，推送给上一级负责人（上一级是部门的，就推送给部门负责人）
                            	部门级用户：
                            验收人是除部门级负责人外的其他角色，推送给部门级负责人。
                            验收人是部门级负责人，推送给上一级负责人（上一级是厂级部门的，推送给厂级部门负责人）。
                            	厂级部门用户：
                            验收人是除厂级部门负责人外的其他角色，推送给厂级部门负责人
                            验收人是厂级部门负责人，推送给分管安全领导
                            	公司领导：
                            验收人是公司领导，推送给验收人本人。

                         */
                        foreach (UserInfoEntity curuserentity in curUserList)
                        {
                            IList<UserInfoEntity> curulist = new List<UserInfoEntity>();
                            if (!participantdeptname.Contains(curuserentity.DeptName))
                            {
                                participantdeptname += curuserentity.DeptName + "、";
                            }

                            //班组、专业、部门非负责人推送当前负责人
                            if ((curuserentity.RoleName.Contains("班组级") || curuserentity.RoleName.Contains("专业级") || curuserentity.RoleName.Contains("部门级"))
                                && !curuserentity.RoleName.Contains("负责人"))
                            {
                                curulist = userbll.GetWFUserListByDeptRoleOrg(curuserentity.OrganizeId, curuserentity.DepartmentId, string.Empty, string.Empty, string.Empty, "负责人"); //取当前负责人
                            }
                            else if ((curuserentity.RoleName.Contains("班组级") || curuserentity.RoleName.Contains("专业级")) && curuserentity.RoleName.Contains("负责人"))
                            {
                                curulist = userbll.GetWFUserListByDeptRoleOrg(curuserentity.OrganizeId, curuserentity.ParentId, string.Empty, string.Empty, string.Empty, "负责人"); //取上级单位负责人
                            }
                            else if (curuserentity.RoleName.Contains("部门级") && !curuserentity.RoleName.Contains("厂级") && curuserentity.RoleName.Contains("负责人"))
                            {
                                curulist = userbll.GetWFUserListByDeptRoleOrg(curuserentity.OrganizeId, string.Empty, string.Empty, string.Empty, string.Empty, "厂级部门用户&负责人"); //取厂级负责人
                            }
                            else if (curuserentity.RoleName.Contains("部门级") && curuserentity.RoleName.Contains("厂级") && curuserentity.RoleName.Contains("负责人"))
                            {
                                curulist = userbll.GetWFUserListByDeptRoleOrg(curuserentity.OrganizeId, string.Empty, string.Empty, string.Empty, string.Empty, "公司领导&安全管理员"); //取厂级负责人
                            }
                            //公司级取自己
                            else if (curuserentity.RoleName.Contains("公司级"))
                            {
                                curulist.Add(curuserentity);
                            }


                            foreach (UserInfoEntity lastuser in curulist)
                            {
                                if (!pushaccount.Contains(lastuser.Account + ","))
                                {
                                    pushaccount += lastuser.Account + ",";
                                    pushname += lastuser.RealName + ",";
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(participantdeptname))
                        {
                            participantdeptname = participantdeptname.Substring(0, participantdeptname.Length - 1);
                        }
                        if (!string.IsNullOrEmpty(pushaccount)) { pushaccount = pushaccount.Substring(0, pushaccount.Length - 1); }
                        if (!string.IsNullOrEmpty(pushname)) { pushname = pushname.Substring(0, pushname.Length - 1); }
                        message = string.Format(@"您好，{0} {1}负责验收的违章《{2}》，未按时进行验收，已于“{3}”逾期，请您知晓并及时督办。",
                           participantdeptname, row["participantname"].ToString(), row["lllegaldescribe"].ToString(), Convert.ToDateTime(row["afteracceptdate"].ToString()).ToString("yyyy-MM-dd HH:mm"));

                        bool isOk = JPushApi.PushMessage(pushaccount, pushname, pushcode, "逾期未验收消息", message, keyValue);
                        //新增推送记录
                        MessagePushRecordEntity entity = new MessagePushRecordEntity();
                        entity.RELVANCEID = keyValue;
                        entity.SOURCEDATE = Convert.ToDateTime(row["curacceptdate"].ToString());
                        entity.EXECNUM += 1;
                        entity.MARK = "afteraccept";
                        entity.PUSHACCOUNT = pushaccount;
                        entity.PUSHCODE = pushcode;
                        entity.WORKFLOW = "违章验收";
                        new MessagePushRecordBLL().SaveForm("", entity);
                    }
                }
                #endregion

                //即将到期未整改
                #region 即将到期未整改

                sql = string.Format(@"select * from (
                                              select a.id,a.lllegaldescribe,a.flowstate,c.reformdeadline,c.reformpeopleid,c.reformpeople,
                                              c.reformdeptcode ,c.reformdeptname ,d.account from bis_lllegalregister a
                                              left join v_lllegalreforminfo c on a.id = c.lllegalid 
                                              left join base_user d on c.reformpeopleid =d.userid 
                                     ) a where  flowstate = '违章整改' and (reformdeadline - 3 <= to_date('{0}','yyyy-mm-dd hh24:mi:ss')  and to_date('{0}','yyyy-mm-dd hh24:mi:ss') <= reformdeadline + 1)", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                var jjdqwzgDt = htbaseinfobll.GetGeneralQueryBySql(sql);

                //即将到期未整改
                foreach (DataRow row in jjdqwzgDt.Rows)
                {
                    pushaccount = string.Empty;
                    pushname = string.Empty;

                    queryJson = "{";
                    string tempstr = string.Format("\"workstream\":\"违章整改\",\"pushcode\":\"WZ017\",\"pushaccount\":\"{0}\",\"mark\":\"beforechange\",\"relvanceid\":\"{1}\"", row["account"].ToString(), row["id"].ToString());
                    queryJson += tempstr;
                    queryJson += "}";

                    list = new MessagePushRecordBLL().GetList(queryJson).ToList();
                    //查询无对应的结果后，开始推送，并保存相应的推送记录
                    if (list.Count() == 0)
                    {
                        string keyValue = row["id"].ToString();
                        //推送到整改人
                        pushcode = "WZ017";
                        pushaccount = row["account"].ToString().Replace("$", "");
                        pushname = row["reformpeople"].ToString();
                        message = string.Format(@"您好，您负责整改的《{0}》违章即将于“{1}”逾期，请您及时进行整改；若因某些客观原因，导致不能按时整改的，请您及时进行延期申请。",
                           row["lllegaldescribe"].ToString(), Convert.ToDateTime(row["reformdeadline"].ToString()).ToString("yyyy-MM-dd HH:mm"));

                        bool isOk = JPushApi.PushMessage(pushaccount, pushname, pushcode, "即将到期未整改消息", message, keyValue);
                        //新增推送记录
                        MessagePushRecordEntity entity = new MessagePushRecordEntity();
                        entity.RELVANCEID = keyValue;
                        entity.SOURCEDATE = DateTime.Now;
                        entity.EXECNUM += 1;
                        entity.MARK = "beforechange";
                        entity.PUSHACCOUNT = row["account"].ToString();
                        entity.PUSHCODE = pushcode;
                        entity.WORKFLOW = "违章整改";
                        new MessagePushRecordBLL().SaveForm("", entity);

                    }
                }
                #endregion

                //逾期未整改
                #region 逾期未整改

                sql = string.Format(@"select * from (
                                             select a.id,a.lllegaldescribe,a.flowstate,c.reformdeadline,c.reformpeopleid,c.reformpeople,
                                              c.reformdeptcode ,c.reformdeptname ,d.account from bis_lllegalregister a
                                              left join v_lllegalreforminfo c on a.id = c.lllegalid 
                                              left join base_user d on c.reformpeopleid =d.userid 
                                     ) a  where  a.flowstate = '违章整改' and   to_date('{0}','yyyy-mm-dd hh24:mi:ss') > reformdeadline + 1", DateTime.Now);
                var yqwzgDt = htbaseinfobll.GetGeneralQueryBySql(sql);

                //逾期未整改
                foreach (DataRow row in yqwzgDt.Rows)
                {
                    pushaccount = string.Empty;
                    pushname = string.Empty;

                    queryJson = "{";
                    string tempstr = string.Format("\"workstream\":\"违章整改\",\"pushcode\":\"WZ020\",\"pushaccount\":\"{0}\",\"mark\":\"afterchange\",\"relvanceid\":\"{1}\"", row["account"].ToString(), row["id"].ToString());
                    queryJson += tempstr;
                    queryJson += "}";

                    list = new MessagePushRecordBLL().GetList(queryJson).ToList();
                    //查询无对应的结果后，开始推送，并保存相应的推送记录
                    if (list.Count() == 0)
                    {
                        string keyValue = row["id"].ToString();
                        //推送到整改人
                        pushcode = "WZ020";
                        UserInfoEntity changUser = userbll.GetUserInfoEntity(row["reformpeopleid"].ToString()); //整改人对象

                        IList<UserInfoEntity> userlist = new List<UserInfoEntity>();

                        //取本部门负责人
                        if ((changUser.RoleName.Contains("班组级") || changUser.RoleName.Contains("专业级") || changUser.RoleName.Contains("部门级")) && !changUser.RoleName.Contains("负责人"))
                        {
                            //取本部门负责人
                            userlist = userbll.GetWFUserListByDeptRoleOrg(changUser.OrganizeId, changUser.DepartmentId, string.Empty, string.Empty, string.Empty, "负责人");
                        }
                        else if ((changUser.RoleName.Contains("班组级") || changUser.RoleName.Contains("专业级")) && changUser.RoleName.Contains("负责人"))
                        {
                            //取上级部门负责人
                            userlist = userbll.GetWFUserListByDeptRoleOrg(changUser.OrganizeId, changUser.ParentId, string.Empty, string.Empty, string.Empty, "负责人");
                        }
                        //非厂级负责人
                        else if (changUser.RoleName.Contains("部门级") && !changUser.RoleName.Contains("厂级") && changUser.RoleName.Contains("负责人"))
                        {
                            //取厂级负责人
                            userlist = userbll.GetWFUserListByDeptRoleOrg(changUser.OrganizeId, string.Empty, string.Empty, string.Empty, string.Empty, "厂级部门用户&负责人");
                        }
                        //厂级负责人
                        else if (changUser.RoleName.Contains("厂级") && changUser.RoleName.Contains("负责人"))
                        {
                            //取分管安全领导
                            userlist = userbll.GetWFUserListByDeptRoleOrg(changUser.OrganizeId, string.Empty, string.Empty, string.Empty, string.Empty, "公司领导&安全管理员");
                        }
                        //公司级取自己
                        else if (changUser.RoleName.Contains("公司级"))
                        {
                            userlist.Add(changUser);
                        }
                        //获取评估人所属的单位负责人及上级负责人
                        foreach (UserInfoEntity userEntity in userlist)
                        {
                            if (!pushaccount.Contains(userEntity.Account + ","))
                            {
                                pushaccount += userEntity.Account + ",";
                                pushname += userEntity.RealName + ",";
                            }
                        }
                        if (!string.IsNullOrEmpty(pushaccount)) { pushaccount = pushaccount.Substring(0, pushaccount.Length - 1); }
                        if (!string.IsNullOrEmpty(pushname)) { pushname = pushname.Substring(0, pushname.Length - 1); }
                        message = string.Format(@"您好，{0} {1}负责整改的《{2}》违章未按时进行整改，已于“{3}”逾期，请您知晓并及时督办", row["reformdeptname"].ToString(), row["reformpeople"].ToString(),
                           row["lllegaldescribe"].ToString(), Convert.ToDateTime(row["reformdeadline"].ToString()).ToString("yyyy-MM-dd HH:mm"));

                        bool isOk = JPushApi.PushMessage(pushaccount, pushname, pushcode, "逾期未整改消息", message, keyValue);
                        //新增推送记录
                        MessagePushRecordEntity entity = new MessagePushRecordEntity();
                        entity.RELVANCEID = keyValue;
                        entity.SOURCEDATE = DateTime.Now;
                        entity.EXECNUM += 1;
                        entity.MARK = "afterchange";
                        entity.PUSHACCOUNT = row["account"].ToString();
                        entity.PUSHCODE = pushcode;
                        entity.WORKFLOW = "违章整改";
                        new MessagePushRecordBLL().SaveForm("", entity);

                    }
                }
                #endregion

                return new { code = 0, info = "获取数据成功" };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message };
            }
        }
        #endregion

        #region 安全重点工作督办定时消息推送
        [HttpGet]
        public object SuperviseMessageToPerson()
        {
            try
            {
                string queryJson = string.Empty; //参数

                string pushcode = string.Empty; //推送代码

                string pushaccount = string.Empty; //推送账户

                string pushname = string.Empty; //推送人员姓名

                string message = string.Empty; //推送信息

                List<MessagePushRecordEntity> list = new List<MessagePushRecordEntity>();
                //任务计划完成时间前2天（完成日期-2）
                string sql = @"select t.dutyperson,t.worktask,c.account from BIS_SafetyWorkSupervise t
left join base_user c on t.dutypersonid = c.userid
 where flowstate='1' and (finishdate - 2 <= sysdate  and sysdate <= finishdate + 1)";
                DataTable dt = htbaseinfobll.GetGeneralQueryBySql(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string worktask = string.Empty;
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    Dictionary<string, string> dic1 = new Dictionary<string, string>();
                    Dictionary<string, int> dic2 = new Dictionary<string, int>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        string account = dr["account"].ToString();
                        string dutyperson = dr["dutyperson"].ToString();
                        if (dic.ContainsKey(account))
                        {
                            dic[account] = dic[account] + "；" + dr["worktask"].ToString();
                            dic2[account] = dic2[account] + 1;//条数
                        }
                        else
                        {
                            worktask = "任务名称：" + dr["worktask"].ToString();
                            dic.Add(account, worktask);//工作内容
                            dic1.Add(account, dutyperson);//姓名
                            dic2.Add(account, 1);//条数
                        }
                    }
                    foreach (string key in dic.Keys)
                    {
                        //推送到核准人
                        pushcode = "WZ016";
                        pushaccount = key;
                        message = string.Format(@"您有{0}条安全重点工作即将到达截止日期，请及时处理。{1}", dic2[key], dic[key]);
                        bool isOk = JPushApi.PushMessage(pushaccount, dic1[key], "GZDB001", "待办理反馈", message, "");
                    }
                }
                return new { code = 0, info = "获取数据成功" };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message };
            }
        }
        #endregion

        #region 检测外包人员超龄并推送消息给相关人员
        /// <summary>
        /// 定时消息推送
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object PushAgeBlackUsers()
        {
            try
            {
                BlacklistBLL blackBll = new BlacklistBLL();
                DepartmentBLL deptBll = new DepartmentBLL();

                DataTable dtItems = deptBll.GetDataTable(string.Format("select itemvalue,deptcode from BIS_BLACKSET t where status=1 and t.itemcode='11'"));
                foreach (DataRow dr in dtItems.Rows)
                {
                    string deptCode = dr[1].ToString();

                    string[] arr = dr[0].ToString().Split('|');
                    string sql = "select '' userid,'' realname,'' deptname,0 age from dual where 0=1";
                    if (arr[0].Length > 0 && arr[1].Length > 0)
                    {
                        sql += string.Format(@" union all select * from (select userid,u.realname,u.DEPTNAME,(cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from v_userinfo u where Gender='男' and u.ispresence='是' and u.departmentcode like '{0}%'
  and u.departmentid in(select d.departmentid from base_department d where (d.nature='承包商' or d.nature='分包商') and d.encode like '{0}%')) t  where t.age< {1} or t.age>{2}", deptCode, arr[0], arr[1]);
                    }

                    if (arr[2].Length > 0 && arr[3].Length > 0)
                    {
                        sql += string.Format(@" union all select * from (select userid,u.realname,u.DEPTNAME,(cast(to_char(sysdate,'yyyy') as number)- cast(substr(identifyid,7,4) as number)) age from v_userinfo u where Gender='女' and u.ispresence='是' and u.departmentcode like '{0}%'
  and u.departmentid in(select d.departmentid from base_department d where (d.nature='承包商' or d.nature='分包商') and d.encode like '{0}%')) t  where t.age< {1} or t.age>{2}", deptCode, arr[2], arr[3]);
                    }
                    //查询外包超龄人员
                    StringBuilder sbUsers = new StringBuilder();
                    DataTable dtUsers1 = deptBll.GetDataTable(sql);
                    foreach (DataRow dr1 in dtUsers1.Rows)
                    {
                        string count = deptBll.GetDataTable(string.Format("select count(1) from bis_blacklist t where t.userid='{0}' and t.Reason='超龄'", dr1[0].ToString())).Rows[0][0].ToString();
                        if (count == "0")
                        {
                            sbUsers.AppendFormat("{0},", dr1[0].ToString());
                        }
                    }
                    if (sbUsers.Length > 0)
                    {
                        string strIds = sbUsers.ToString();
                        BlacklistEntity black = new BlacklistEntity
                        {
                            Id = Guid.NewGuid().ToString(),
                            JoinTime = DateTime.Now,
                            Reason = "超龄",
                            UserId = strIds.Substring(0, strIds.Length - 1)
                        };
                        blackBll.SaveForm("", black);
                    }
                    //推送短消息给相关人员
                    DataTable dtDepts = deptBll.GetDataTable(string.Format("select itemvalue from base_dataitemdetail a where a.itemid=(select itemid from base_dataitem where itemcode='PassAgeSet') and a.itemcode='{0}'", deptCode));
                    if (dtDepts.Rows.Count > 0)
                    {
                        string value = dtDepts.Rows[0][0].ToString();
                        if (value.Trim().Length > 0)
                        {
                            sql = "";
                            arr = value.Split('|');
                            string codes = arr[0];
                            StringBuilder sb = new StringBuilder("(");
                            string[] dCodes = codes.Split(',');
                            foreach (string code in dCodes)
                            {
                                sb.AppendFormat(" departmentcode='{0}' or", code);
                            }
                            sb.Append(")");
                            sql = sb.ToString().Substring(0, sb.ToString().Length - 3) + ")";
                            if (arr.Length == 2)
                            {
                                sb.Clear();
                                sb.Append(" and (");
                                string roles = arr[1];
                                string[] arrRole = roles.Split(',');
                                foreach (string role in arrRole)
                                {
                                    sb.AppendFormat(" rolename like '%{0}%' or", role);
                                }
                                sql += sb.ToString().Substring(0, sb.ToString().Length - 3) + ")";
                            }
                            sql = string.Format("select account,realname from base_user u where organizecode='{1}' and {0}", sql, deptCode);
                            DataTable dtUsers = deptBll.GetDataTable(sql);
                            if (dtUsers.Rows.Count > 0)
                            {
                                foreach (DataRow dr2 in dtUsers.Rows)
                                {
                                    string account = dr2[0].ToString();
                                    string name = dr2[1].ToString();
                                    foreach (DataRow dr1 in dtUsers1.Rows)
                                    {
                                        string count = deptBll.GetDataTable(string.Format("select count(1) from BASE_MESSAGE t where t.recid='{0}' and userid='{1}' and t.remark='超龄'", dr1[0].ToString(), account)).Rows[0][0].ToString();
                                        if (count == "0")
                                        {
                                            string title = string.Format("人员{0}已超龄加入黑名单，请知晓", dr1[1].ToString());
                                            string conetent = string.Format("人员{0}年龄{1}周岁，已达到加入黑名单的年龄标准，现已加入黑名单，请您知晓", dr1[1].ToString(), dr1[3].ToString());
                                            bool isOk = JPushApi.PushMessage(account, name, "WBBlack", title, conetent, dr1[0].ToString());
                                        }

                                    }
                                }
                            }

                        }

                    }

                }
                return new { code = 0, info = "操作成功" };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message };
            }
        }
        #endregion

        #region 京能康巴什隐患提醒
        /// <summary>
        /// 京能康巴什隐患提醒
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object JingNengKangBaShiHiddenWarning()
        {
            try
            {
                string pushaccount = string.Empty;
                string pushname = string.Empty;
                string queryJson = string.Empty;
                string pushcode = string.Empty;
                string message = string.Empty;
                //即将到期未整改
                #region 即将到期未整改

                string sql = string.Format(@"select * from (
                                              select a.id,a.hiddescribe,a.workstream,substr(b.itemname,length(b.itemname)-3) as rankname,c.changedeadine,c.changeperson,c.changepersonname,
                                              c.changedutydepartcode ,c.changedutydepartname ,d.account,d.nature,d.parentid,d.departmentid,e.itemvalue from bis_htbaseinfo a
                                              left join base_dataitemdetail  b on a.hidrank = b.itemdetailid
                                              left join v_htchangeinfo c on a.hidcode = c.hidcode 
                                              left join v_userinfo d on c.changeperson =d.userid 
                                              left join base_dataitemdetail e on a.majorclassify = e.itemdetailid
                                     ) a where  a.workstream ='隐患整改' and  ((rankname = '一般隐患' and changedeadine - 3 <= to_date('{0}','yyyy-mm-dd hh24:mi:ss')  and to_date('{0}','yyyy-mm-dd hh24:mi:ss') <= changedeadine + 1 )  or 
                                      (rankname = '重大隐患' and changedeadine - 5 <= to_date('{0}','yyyy-mm-dd hh24:mi:ss') and  to_date('{0}','yyyy-mm-dd hh24:mi:ss') <= changedeadine + 1 ) )", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                var jjdqwzgDt = htbaseinfobll.GetGeneralQueryBySql(sql);

                //即将到期未整改
                foreach (DataRow row in jjdqwzgDt.Rows)
                {
                    pushaccount = row["account"].ToString().Replace("$", "") + ",";
                    pushname = row["changepersonname"].ToString() + ",";

                    string nature = row["nature"].ToString();
                    string parentid = row["parentid"].ToString();
                    string specialtytype = row["itemvalue"].ToString();
                    string tempWhere = string.Empty;
                    //获取平级、上一级专业及上一级部门专业专工
                    var superior = departmentbll.GetParentDeptBySpecialArgs(parentid, "部门");
                    if (null != superior)
                    {
                        tempWhere = string.Format("  or  (departmentid = '{0}' and nature ='部门')", superior.DepartmentId);
                    }
                    string childsql = string.Format(@"select a.account,a.realname username from v_userinfo  a  where  a.rolename like '%专工%' and a.specialtytype like '%{0}%'  and a.specialtytype is not null and departmentid in 
(select departmentid from base_department  where (parentid = '{1}' and nature ='专业') or  (departmentid = '{1}' and nature ='专业') {2}) ", specialtytype, parentid, tempWhere);
                    var pushObjDt = htbaseinfobll.GetGeneralQueryBySql(childsql);

                    foreach (DataRow crow in pushObjDt.Rows)
                    {
                        string commonaccount = "," + pushaccount;

                        if (!commonaccount.Contains("," + crow["account"].ToString() + ","))
                        {
                            pushaccount += crow["account"].ToString() + ",";
                            pushname += crow["username"].ToString() + ",";
                        }
                    }
                    pushaccount = pushaccount.Substring(0, pushaccount.Length - 1);
                    pushname = pushname.Substring(0, pushname.Length - 1);

                    queryJson = "{";
                    string tempstr = string.Format("\"workstream\":\"隐患整改\",\"pushcode\":\"YH024\",\"pushaccount\":\"{0}\",\"mark\":\"willchange\",\"relvanceid\":\"{1}\"", pushaccount, row["id"].ToString());
                    queryJson += tempstr;
                    queryJson += "}";

                    var list = new MessagePushRecordBLL().GetList(queryJson).ToList();
                    //查询无对应的结果后，开始推送，并保存相应的推送记录
                    if (list.Count() == 0)
                    {
                        string keyValue = row["id"].ToString();
                        //推送到整改人
                        pushcode = "YH024";
                        message = string.Format(@"您好，您负责整改的《{0}》隐患即将于“{1}”逾期，请您及时进行整改；若因某些客观原因，导致不能按时整改的，请您及时进行延期申请。",
                           row["hiddescribe"].ToString(), Convert.ToDateTime(row["changedeadine"].ToString()).ToString("yyyy-MM-dd"));

                        bool isOk = JPushApi.PushMessage(pushaccount, pushname, pushcode, "即将到期未整改消息", message, keyValue);
                        //新增推送记录
                        MessagePushRecordEntity entity = new MessagePushRecordEntity();
                        entity.RELVANCEID = keyValue;
                        entity.SOURCEDATE = DateTime.Now;
                        entity.EXECNUM += 1;
                        entity.MARK = "willchange";
                        entity.PUSHACCOUNT = pushaccount;
                        entity.PUSHCODE = pushcode;
                        entity.WORKFLOW = "隐患整改";
                        new MessagePushRecordBLL().SaveForm("", entity);

                    }
                }
                #endregion

                return new { code = 0, info = "获取数据成功" };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message };
            }
        }
        #endregion

        #region 黑名单推送(违章积分相关)
        /// <summary>
        /// 黑名单推送(违章积分相关)
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public object PushLllegalBlackUsers()
        {
            try
            {
                BlacklistBLL blackBll = new BlacklistBLL();
                DepartmentBLL deptBll = new DepartmentBLL();

                string sql = string.Empty;
                //黑名单标准
                DataTable dtItems = deptBll.GetDataTable(string.Format("select itemvalue,itemcode from BIS_BLACKSET t where status=1 and t.itemcode in ('03','04','12')"));

                foreach (DataRow row in dtItems.Rows)
                {
                    string pushaccount = string.Empty;
                    string pushusername = string.Empty;
                    string pushcode = string.Empty;
                    string blackusername = string.Empty;
                    string title = string.Empty;
                    string content = string.Empty;
                    string tempcontent = string.Empty;
                    string reason = string.Empty;
                    int itemvalue = int.Parse(row["itemvalue"].ToString());
                    string itemcode = row["itemcode"].ToString();
                    string pointStandard = "0";
                    switch (itemcode)
                    {
                        //一般违章次数
                        case "03":
                            reason = "一般违章次数超标";
                            sql = string.Format(@"select * from (
                            select count(a.id) pnum,a.lllegalpersonid userid,a.lllegalperson username ,b.account from v_lllegalbaseinfo a 
                            left join base_user b on a.lllegalpersonid = b.userid  where 1=1 and lllegallevelname like '%一般违章%'  and a.lllegalpersonid is not null  group by a.lllegalpersonid,a.lllegalperson,b.account  
                            ) a where pnum >= {0}", itemvalue);
                            break;
                        //严重违章次数
                        case "04":
                            reason = "严重违章次数超标";
                            sql = string.Format(@"select * from (
                            select count(a.id) pnum,a.lllegalpersonid userid,a.lllegalperson username ,b.account from v_lllegalbaseinfo a 
                            left join base_user b on a.lllegalpersonid = b.userid  where 1=1 and lllegallevelname like '%严重违章%'  and a.lllegalpersonid is not null  group by a.lllegalpersonid,a.lllegalperson,b.account  
                            ) a where pnum >= {0}", itemvalue);
                            break;
                        //外委人员违章积分
                        case "12":
                            pointStandard = new DataItemDetailBLL().GetItemValue("LllegalPointInitValue").Trim(); //违章默认积分
                            reason = "外委人员违章积分低于标准值";
                            sql = string.Format(@"select * from (
                                 select {1} - sum(a.lllegalpoint)  pnum,a.userid,a.realname username ,a.account from v_lllegalassesforperson a where a.isepiboly =1 group by a.userid,a.realname  ,a.account
                            ) a where pnum <= {0}", itemvalue, pointStandard);
                            break;
                    }

                    var lllegalUser = deptBll.GetDataTable(sql);
                    //提交黑名单
                    foreach (DataRow crow in lllegalUser.Rows)
                    {
                        string count = string.Empty;
                        //提交到黑名单
                        count = deptBll.GetDataTable(string.Format("select count(1) from bis_blacklist t where t.userid='{0}' and t.Reason='{1}'", crow["userid"].ToString(), reason)).Rows[0][0].ToString();

                        if (count == "0")
                        {
                            BlacklistEntity black = new BlacklistEntity
                            {
                                Id = Guid.NewGuid().ToString(),
                                JoinTime = DateTime.Now,
                                Reason = reason,
                                UserId = crow["userid"].ToString()
                            };
                            blackBll.SaveForm("", black);
                        }

                        blackusername += crow["username"].ToString() + "、";

                        if (itemcode == "12")
                        {
                            string lastpoint = (Convert.ToDecimal(pointStandard) - Convert.ToDecimal(crow["pnum"].ToString())).ToString();
                            tempcontent += crow["username"].ToString() + "违章积分" + lastpoint + "分、";
                        }
                        pushaccount = crow["account"].ToString();
                        pushusername = crow["username"].ToString();

                        //推送到核准人
                        title = string.Format("您已加入黑名单，请知晓", blackusername);
                        if (itemcode == "03")
                        {
                            pushcode = "WZ022";
                            content = string.Format("您一般违章次数超标，已达到加入黑名单的标准，现已加入黑名单，请您知晓");
                        }
                        else if (itemcode == "04")
                        {
                            pushcode = "WZ023";
                            content = string.Format("您重大违章次数超标，已达到加入黑名单的标准，现已加入黑名单，请您知晓");
                        }
                        else if (itemcode == "12")
                        {
                            pushcode = "WZ024";
                            content = string.Format("您违章积分为{0}分，已达到加入黑名单的标准，现已加入黑名单，请您知晓", crow["pnum"].ToString());
                        }
                        if (!string.IsNullOrEmpty(pushaccount))
                        {
                            count = deptBll.GetDataTable(string.Format("select count(1) from BASE_MESSAGE t where t.title='{0}' and  t.content='{1}' and  userid='{2}'", title.Trim(), content.Trim(), pushaccount)).Rows[0][0].ToString();
                            if (count == "0")
                            {
                                bool isOk = JPushApi.PushMessage(pushaccount, pushusername, pushcode, title.Trim(), content.Trim(), string.Empty);
                            }
                        }

                    }

                    DataTable dtDepts = deptBll.GetDataTable(string.Format("select itemvalue from base_dataitemdetail a where a.itemid= (select itemid from base_dataitem where itemcode='BackUserForLllegal') and a.itemcode='{0}'", itemcode));
                    pushaccount = string.Empty;
                    pushusername = string.Empty;
                    if (dtDepts.Rows.Count == 1)
                    {
                        var otherUser = GetSpecialUserInfoForPush(dtDepts.Rows[0]["itemvalue"].ToString());
                        foreach (DataRow crow in otherUser.Rows)
                        {
                            pushaccount += crow["account"].ToString() + ",";
                            pushusername += crow["username"].ToString() + ",";
                        }
                    }
                    if (!string.IsNullOrEmpty(pushaccount))
                    {
                        pushaccount = pushaccount.Substring(0, pushaccount.Length - 1);
                    }
                    if (!string.IsNullOrEmpty(pushusername))
                    {
                        pushusername = pushusername.Substring(0, pushusername.Length - 1);
                    }
                    if (!string.IsNullOrEmpty(blackusername))
                    {
                        blackusername = blackusername.Substring(0, blackusername.Length - 1);
                    }
                    if (!string.IsNullOrEmpty(tempcontent))
                    {
                        tempcontent = tempcontent.Substring(0, tempcontent.Length - 1);
                    }
                    //推送到核准人
                    title = string.Format("人员{0}已加入黑名单，请知晓", blackusername);
                    if (itemcode == "03")
                    {
                        pushcode = "WZ022";
                        content = string.Format("{0}一般违章次数超标，已达到加入黑名单的标准，现已加入黑名单，请您知晓", blackusername);
                    }
                    else if (itemcode == "04")
                    {
                        pushcode = "WZ023";
                        content = string.Format("{0}重大违章次数超标，已达到加入黑名单的标准，现已加入黑名单，请您知晓", blackusername);
                    }
                    else if (itemcode == "12")
                    {
                        pushcode = "WZ024";
                        content = string.Format("{0}，已达到加入黑名单的标准，现已加入黑名单，请您知晓", tempcontent);
                    }
                    if (!string.IsNullOrEmpty(pushaccount) && !string.IsNullOrEmpty(pushusername) && !string.IsNullOrEmpty(blackusername))
                    {
                        string count = deptBll.GetDataTable(string.Format("select count(1) from BASE_MESSAGE t where t.title='{0}' and  t.content='{1}' and  userid='{2}'", title.Trim(), content.Trim(), pushaccount)).Rows[0][0].ToString();
                        if (count == "0")
                        {
                            bool isOk = JPushApi.PushMessage(pushaccount, pushusername, pushcode, title.Trim(), content.Trim(), string.Empty);
                        }
                    }
                }
                return new { code = 0, info = "操作成功" };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message };
            }
        }
        #endregion

        #region 违章积分提醒(违章积分相关)
        /// <summary>
        /// 违章积分提醒(违章积分相关)
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public object PushLllegalUsersByPoint()
        {
            try
            {
                BlacklistBLL blackBll = new BlacklistBLL();
                DepartmentBLL deptBll = new DepartmentBLL();

                string sql = string.Empty;
                string pushaccount = string.Empty;
                string pushusername = string.Empty;
                string blackusername = string.Empty;
                string title = string.Empty;
                string content = string.Empty;
                string tempcontent = string.Empty;


                decimal wzpoint = 0;

                var wzljzDt = deptBll.GetDataTable(string.Format("select itemvalue from base_dataitemdetail a where a.itemid= (select itemid from base_dataitem where itemcode='LllegalPointRemindSetting') and a.itemcode='WZJFLJZ'"));

                if (wzljzDt.Rows.Count > 0)
                {
                    var pointStandard = new DataItemDetailBLL().GetItemValue("LllegalPointInitValue").Trim(); //违章默认积分

                    wzpoint = Convert.ToDecimal(wzljzDt.Rows[0]["itemvalue"].ToString().Trim());

                    sql = string.Format(@"select * from (
                                 select {1} - sum(a.lllegalpoint)  pnum,a.userid,a.realname username ,a.account from v_lllegalassesforperson a  group by a.userid,a.realname  ,a.account
                            ) a where pnum <= {0}", wzpoint, pointStandard);

                    var lllegalUser = deptBll.GetDataTable(sql);
                    //违章的人员
                    foreach (DataRow crow in lllegalUser.Rows)
                    {
                        pushaccount = crow["account"].ToString();
                        pushusername = crow["username"].ToString();
                        //推送到核准人
                        title = string.Format("您的违章积分已低于或等于{0}分，请知晓。", wzpoint);
                        content = string.Format("您的违章积分为{0}分，已低于或等于{1}分标准，请您知晓并及时处理。", crow["pnum"].ToString(), wzpoint);

                        if (!string.IsNullOrEmpty(pushaccount))
                        {
                            string count = deptBll.GetDataTable(string.Format("select count(1) from BASE_MESSAGE t where t.title='{0}' and  t.content='{1}' and  userid='{2}'", title.Trim(), content.Trim(), pushaccount)).Rows[0][0].ToString();
                            if (count == "0")
                            {
                                bool isOk = JPushApi.PushMessage(pushaccount, pushusername, "WZ025", title.Trim(), content.Trim(), string.Empty);
                            }
                        }

                        //取总   //要推送消息的对象
                        blackusername += crow["username"].ToString() + ",";
                        string lastpoint = (Convert.ToDecimal(pointStandard) - Convert.ToDecimal(crow["pnum"].ToString())).ToString();
                        tempcontent += crow["username"].ToString() + "违章积分" + lastpoint + "分、";
                    }
                }

                //非违章人员接收的信息
                var dtDepts = deptBll.GetDataTable(string.Format("select itemvalue from base_dataitemdetail a where a.itemid= (select itemid from base_dataitem where itemcode='LllegalPointRemindSetting') and a.itemcode='RYWZ'"));

                pushaccount = string.Empty;
                pushusername = string.Empty;

                //要推送消息的对象
                if (dtDepts.Rows.Count > 0)
                {
                    var otherUser = GetSpecialUserInfoForPush(dtDepts.Rows[0]["itemvalue"].ToString());
                    foreach (DataRow crow in otherUser.Rows)
                    {
                        pushaccount += crow["account"].ToString() + ",";
                        pushusername += crow["username"].ToString() + ",";
                    }
                }

                if (!string.IsNullOrEmpty(pushaccount))
                {
                    pushaccount = pushaccount.Substring(0, pushaccount.Length - 1);
                }
                if (!string.IsNullOrEmpty(pushusername))
                {
                    pushusername = pushusername.Substring(0, pushusername.Length - 1);
                }
                if (!string.IsNullOrEmpty(blackusername))
                {
                    blackusername = blackusername.Substring(0, blackusername.Length - 1);
                }
                if (!string.IsNullOrEmpty(tempcontent))
                {
                    tempcontent = tempcontent.Substring(0, tempcontent.Length - 1);
                }

                //推送到接收人
                title = string.Format("{0}的违章积已低于或等于{1}分，请知晓。", blackusername, wzpoint);
                content = string.Format("{0}，已低于或等于{1}分标准，请您知晓并及时处理。", tempcontent, wzpoint);

                if (!string.IsNullOrEmpty(pushaccount) && !string.IsNullOrEmpty(pushusername) && !string.IsNullOrEmpty(blackusername))
                {
                    string count = deptBll.GetDataTable(string.Format("select count(1) from BASE_MESSAGE t where t.title='{0}' and  t.content='{1}' and  userid='{2}'", title.Trim(), content.Trim(), pushaccount)).Rows[0][0].ToString();
                    if (count == "0")
                    {
                        bool isOk = JPushApi.PushMessage(pushaccount, pushusername, "WZ025", title.Trim(), content.Trim(), string.Empty);
                    }
                }

                return new { code = 0, info = "操作成功" };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message };
            }
        }
        #endregion

        #region 违章积分提醒(京泰电厂专用)
        /// <summary>
        /// 违章积分提醒(违章积分相关)
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public object PushLllegalUsersByPointForJNJT()
        {
            try
            {
                BlacklistBLL blackBll = new BlacklistBLL();
                DepartmentBLL deptBll = new DepartmentBLL();

                string sql = string.Empty;
                string pushaccount = string.Empty;
                string pushusername = string.Empty;
                string title = string.Empty;
                string content = string.Empty;

                var pointStandard = new DataItemDetailBLL().GetItemValue("LllegalPointInitValue").Trim(); //违章默认积分


                #region 小于等于4分
                sql = string.Format(@"select * from (
                                  select a.userid,a.realname username,a.account,({0} - sum(a.lllegalpoint))  pnum from v_lllegalassesforperson  a
                                    left join (
                                     select count(1) pnum ,recoveruserid,createdate  from v_lllegalpointrecoverdetail group by recoveruserid,createdate 
                                  ) b on a.userid = b.recoveruserid  
                                  where 1=1  and (nvl(b.pnum,0)=0  or  (nvl(b.pnum,0)>0  and  a.createdate > b.createdate)) group by a.userid,a.realname,a.account
                            ) a where pnum <= 4", pointStandard);

                var lllegalUser = deptBll.GetDataTable(sql);

                #region  违章的人员
                foreach (DataRow crow in lllegalUser.Rows)
                {
                    pushaccount = crow["account"].ToString();
                    pushusername = crow["username"].ToString();
                    //推送到核准人
                    title = string.Format("您的违章积分为{0}分，您需待岗一个月，考试通过后方重新上岗。", crow["pnum"].ToString());
                    content = string.Format("您的违章积分现为{0}分，按照公司《反违章管理办法》的规定，您需待岗一个月，且需要针对自己的违章行为，深入分析，制定书面整改措施，参与公司组织的教育培训与帮扶，认真学习《安规》和相关规章制度，并通过安监部考试合格后方可重新上岗，请您知悉并处理。", crow["pnum"].ToString());
                    if (!string.IsNullOrEmpty(pushaccount))
                    {
                        string count = deptBll.GetDataTable(string.Format("select count(1) from BASE_MESSAGE t where t.title='{0}' and  t.content='{1}' and  userid='{2}'", title.Trim(), content.Trim(), pushaccount)).Rows[0][0].ToString();
                        if (count == "0")
                        {
                            bool isOk = JPushApi.PushMessage(pushaccount, pushusername, "WZ025", title.Trim(), content.Trim(), string.Empty);
                        }
                    }
                }
                #endregion 

                #endregion

                #region 大于4分小于等于6分

                sql = string.Format(@"select * from (
                                    select a.userid,a.realname username,a.account,({0} - sum(a.lllegalpoint))  pnum from v_lllegalassesforperson  a
                                    left join (
                                     select count(1) pnum ,recoveruserid,createdate  from v_lllegalpointrecoverdetail group by recoveruserid,createdate 
                                  ) b on a.userid = b.recoveruserid  
                                  where 1=1  and (nvl(b.pnum,0)=0  or  (nvl(b.pnum,0)>0  and  a.createdate > b.createdate)) group by a.userid,a.realname,a.account
                            ) a where pnum >4  and  pnum <= 6", pointStandard);

                lllegalUser = deptBll.GetDataTable(sql);

                #region  违章的人员
                foreach (DataRow crow in lllegalUser.Rows)
                {
                    pushaccount = crow["account"].ToString();
                    pushusername = crow["username"].ToString();
                    //推送到核准人
                    title = string.Format("您的违章积分为{0}分，您需要做相关培训并通过考试！", crow["pnum"].ToString());
                    content = string.Format("您的违章积分现为{0}分，按照公司《反违章管理办法》的规定，您需要针对自己的违章行为，深入分析，制定书面整改措施，参与公司组织的教育培训与帮扶，认真学习《安规》和相关规章制度，并通过本部门的考试，请您知悉并处理。", crow["pnum"].ToString());
                    if (!string.IsNullOrEmpty(pushaccount))
                    {
                        string count = deptBll.GetDataTable(string.Format("select count(1) from BASE_MESSAGE t where t.title='{0}' and  t.content='{1}' and  userid='{2}'", title.Trim(), content.Trim(), pushaccount)).Rows[0][0].ToString();
                        if (count == "0")
                        {
                            bool isOk = JPushApi.PushMessage(pushaccount, pushusername, "WZ025", title.Trim(), content.Trim(), string.Empty);
                        }
                    }
                }
                #endregion 

                #endregion

                #region 大于6分小于等于8分
                sql = string.Format(@"select * from (
                                   select a.userid,a.realname username,a.account,({0} - sum(a.lllegalpoint))  pnum from v_lllegalassesforperson  a
                                    left join (
                                     select count(1) pnum ,recoveruserid,createdate  from v_lllegalpointrecoverdetail group by recoveruserid,createdate 
                                  ) b on a.userid = b.recoveruserid  
                                  where 1=1  and (nvl(b.pnum,0)=0  or  (nvl(b.pnum,0)>0  and  a.createdate > b.createdate)) group by a.userid,a.realname,a.account
                            ) a where pnum >6  and  pnum <= 8", pointStandard);

                lllegalUser = deptBll.GetDataTable(sql);

                #region  违章的人员
                foreach (DataRow crow in lllegalUser.Rows)
                {
                    pushaccount = crow["account"].ToString();
                    pushusername = crow["username"].ToString();
                    //推送到核准人
                    title = string.Format("您的违章积分为{0}分，您需要做相关培训！", crow["pnum"].ToString());
                    content = string.Format("您的违章积分现为{0}分，按照公司《反违章管理办法》的规定，您需要针对自己的违章行为，深入分析，认真学习《安规》和相关规章制度，并制定书面整改措施报安监部，请您知悉并处理。", crow["pnum"].ToString());
                    if (!string.IsNullOrEmpty(pushaccount))
                    {
                        string count = deptBll.GetDataTable(string.Format("select count(1) from BASE_MESSAGE t where t.title='{0}' and  t.content='{1}' and  userid='{2}'", title.Trim(), content.Trim(), pushaccount)).Rows[0][0].ToString();
                        if (count == "0")
                        {
                            bool isOk = JPushApi.PushMessage(pushaccount, pushusername, "WZ025", title.Trim(), content.Trim(), string.Empty);
                        }
                    }
                }
                #endregion 

                #endregion

                return new { code = 0, info = "操作成功" };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message };
            }
        }
        #endregion


        #region 反违章量化指标提醒(京能康巴什专用)
        /// <summary>
        /// 反违章量化指标提醒(京能康巴什专用)
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public object PushLllegalQuantifyIndexForJNDL()
        {
            try
            {
                DepartmentBLL deptBll = new DepartmentBLL();

                string sql = string.Empty;
                string pushaccount = string.Empty;
                string pushusername = string.Empty;
                string title = string.Empty;
                string content = string.Empty;
                string realnum = string.Empty;
                string plannum = string.Empty;
                #region 获取完成率没到100%的人
                sql = string.Format(@"with indexTb as (
                                        select a.deptid,a.dutyid,a.indexvalue,a.yearvalue,(a.yearvalue ||'-'|| b.month) yearmonth from bis_lllegalquantifyindex a
                                        left join  (select lpad(level,2,0) as month from dual connect by level <13) b on instr(a.monthvalue,b.month)>0  where 
                                        to_char(a.yearvalue ||'-'|| b.month) = '{0}'
                                    ), userTb as (
                                        select  a.userid,a.account,a.realname username,a.deptname,a.dutyid,a.departmentid ,b.yearmonth,b.indexvalue from v_userinfo a
                                        inner join  indexTb b on a.departmentid=b.deptid and a.dutyid=b.dutyid 
                                    ) 
                                    select a.userid,a.account,a.username,a.deptname, nvl(b.pnum,0) realnum,a.indexvalue, round((case when nvl(a.indexvalue,0) = 0 then 0 else  nvl(b.pnum,0) / nvl(a.indexvalue,0) * 100 end ),2) percents from  userTb a
                                    left join (
                                            select count(1) pnum, to_char(createdate,'yyyy-MM') yearmonth , createuserid,dutyid from bis_lllegalregister group by to_char(createdate,'yyyy-MM'),createuserid,dutyid
                                    ) b on a.userid =b.createuserid and a.yearmonth = b.yearmonth and a.dutyid = b.dutyid where  nvl(b.pnum,0) != a.indexvalue ", DateTime.Now.ToString("yyyy-MM"));

                var lllegalUser = deptBll.GetDataTable(sql);

                #region  违章的人员
                foreach (DataRow crow in lllegalUser.Rows)
                {
                    pushaccount = crow["account"].ToString();
                    pushusername = crow["username"].ToString();
                    realnum = crow["realnum"].ToString();
                    plannum = crow["indexvalue"].ToString();
                    //推送到处理人
                    //标题:2021年1月反违章工作指标任务提醒
                    title = string.Format("{0}反违章工作指标任务提醒!", DateTime.Now.ToString("yyyy年MM月"));
                    //内容: 您的违章指标实际已完成1项，目标是2项,2021年1月反违章工作指标暂未完成.
                    content = string.Format("您的反违章指标实际已完成{0}项，目标是{1}项,{2}反违章工作指标暂未完成.", realnum, plannum, DateTime.Now.ToString("yyyy年MM月"));
                    if (!string.IsNullOrEmpty(pushaccount))
                    {
                        string count = deptBll.GetDataTable(string.Format("select count(1) from BASE_MESSAGE t where t.title='{0}' and  t.content='{1}' and  userid='{2}'", title.Trim(), content.Trim(), pushaccount)).Rows[0][0].ToString();
                        if (count == "0")
                        {
                            bool isOk = JPushApi.PushMessage(pushaccount, pushusername, "WZ026", title.Trim(), content.Trim(), string.Empty);
                        }
                    }
                }
                #endregion


                #endregion

                return new { code = 0, info = "操作成功" };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message };
            }
        }
        #endregion

        #region 选择人员
        /// <summary>
        /// 选择人员
        /// </summary>
        /// <param name="itemvalue"></param>
        /// <returns></returns>
        public DataTable GetSpecialUserInfoForPush(string itemvalue)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("userid");
            dt.Columns.Add("account");
            dt.Columns.Add("username");
            string sql = string.Empty;
            string strWhere = " 1=1 ";
            try
            {
                #region MyRegion
                if (!string.IsNullOrEmpty(itemvalue))
                {
                    string[] arr = itemvalue.Split('&');

                    foreach (string str in arr)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            string[] array = str.Split('|');
                            strWhere += string.Format(@" and  (departmentcode = '{0}' or departmentid = '{0}') ", array[0].ToString());
                            string temprole = string.Empty;
                            if (!string.IsNullOrEmpty(array[1]))
                            {
                                string[] arrRole = array[1].Split(',');
                                temprole += " and  (";
                                foreach (string role in arrRole)
                                {
                                    temprole += string.Format(@" rolename like '%{0}%' or", role);
                                }
                                if (!string.IsNullOrEmpty(temprole)) { temprole = temprole.Substring(0, temprole.Length - 2); }
                                temprole += ")";
                            }
                            if (!string.IsNullOrEmpty(temprole))
                            {
                                strWhere += temprole;
                            }
                            sql = string.Format("select userid, account,realname username from v_userinfo u where  {0}", strWhere);

                            DataTable dtUsers = htbaseinfobll.GetGeneralQueryBySql(sql);
                            foreach (DataRow row in dtUsers.Rows)
                            {
                                DataRow nrow = dt.NewRow();
                                nrow["userid"] = row["userid"].ToString();
                                nrow["account"] = row["account"].ToString();
                                nrow["username"] = row["username"].ToString();
                                dt.Rows.Add(nrow);
                            }
                        }

                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        #endregion

        #region 获取各区域隐患数量
        /// <summary>
        /// 获取各区域隐患数量
        /// </summary>
        /// <param name="areaCode">区域编码</param>
        /// <returns></returns>
        public object getAreaHTStat(string areaCode = "")
        {
            try
            {
                string sql = string.Format(@"select a.districtid areaid,t.* from (select HIDPOINT areacode,HIDPOINTNAME areaname,count(1) htcount from V_BASEHIDDENINFO t  where HIDPOINT is not null  group by HIDPOINT,HIDPOINTNAME) t left join bis_district a on t.areacode=a.districtcode");
                if (!string.IsNullOrWhiteSpace(areaCode))
                {
                    sql = string.Format(@"select a.districtid areaid,t.* from (select HIDPOINT areacode,HIDPOINTNAME areaname,count(1) htcount from V_BASEHIDDENINFO t  where HIDPOINT like '{0}%' and HIDPOINT is not null  group by HIDPOINT,HIDPOINTNAME) t left join bis_district a on t.areacode=a.districtcode", areaCode);
                }
                DataTable data = new DepartmentBLL().GetDataTable(sql);
                return new { code = 0, message = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {
                return new { code = 1, message = ex.Message };
            }

        }
        #endregion


        #region 推送并关联用户信息（来自于Java培训平台修改过账号的用户）
        [HttpPost]
        /// <summary>
        /// 获取各区域隐患数量
        /// </summary>
        /// <param name="areaCode">区域编码</param>
        /// <returns></returns>
        public object pushUserInfo([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                List<object> list = dy.data;
                StringBuilder sb = new StringBuilder("begin  \r\n");
                foreach (object obj in list)
                {
                    dy = obj;
                    string newAccount = dy.newUserAccount;
                    string account = dy.userAccount;
                    sb.AppendFormat("update base_user set newaccount='{0}' where account='{1}'; \r\n", newAccount, account);
                }
                sb.Append("commit;\r\n end;");
                int count = new DepartmentBLL().ExecuteSql(sb.ToString());
                return new { code = 0, message = "操作成功", data = res };
            }
            catch (Exception ex)
            {
                return new { code = 1, message = ex.Message };
            }

        }
        #endregion


        #region 获取用户签名照
        [AcceptVerbs("Get", "Post")]
        /// <summary>
        /// 获取用户签名照片
        /// </summary>
        /// <returns></returns>
        public object getUserSign()
        {
            try
            {
                string accounts = HttpContext.Current.Request["accounts"];
                UserInfoEntity user = userbll.GetUserInfoByAccount(accounts);
                if (string.IsNullOrWhiteSpace(accounts))
                {
                    return new { code = -1, message = "参数accounts不能为空!" };
                }
                else
                {
                    DataItemDetailBLL di = new DataItemDetailBLL();
                    string imgurl = di.GetItemValue("imgUrl");
                    string path = di.GetItemValue("imgPath");
                    DataTable dtUsers = departmentbll.GetDataTable(string.Format("select account,SignImg signUrl from base_user u where account in('{0}')", accounts.Replace(",", "','")));
                    foreach (DataRow row in dtUsers.Rows)
                    {
                        string account = row["account"].ToString();
                        string signImg = row["signUrl"].ToString();

                        if (!string.IsNullOrWhiteSpace(signImg))
                        {
                            string filePath = path + signImg.Replace("~/", "/").Replace("/", "\\");
                            if (!File.Exists(filePath))
                            {
                                row["signUrl"] = "";
                            }
                            else
                            {
                                row["signUrl"] = imgurl + signImg.Replace("~/", "/");
                            }
                        }

                    }
                    return new { code = 0, message = "获取数据成功", data = dtUsers };
                }

            }
            catch (Exception ex)
            {
                return new { code = -1, message = ex.Message };
            }
        }
        #endregion
    }
}