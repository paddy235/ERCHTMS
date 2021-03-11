using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data;
using System.Net;
using Newtonsoft.Json;
using System.Dynamic;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using System.Text;
using ERCHTMS.Busines.SystemManage;

namespace ERCHTMS.Web.Areas.PersonManage.Controllers
{
    public class UserEntryController : MvcControllerBase
    {
        DepartmentBLL deptBll = new DepartmentBLL();
        private UserBLL userBLL = new UserBLL();
        // GET: PersonManage/UserEntry
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetTrainProjects()
        {
            try
            {
                Operator user = OperatorProvider.Provider.Current();
                if (user == null)
                {
                    return Error("身份票据已过期!");
                }
                DepartmentEntity dept = deptBll.GetEntity(user.OrganizeId);
                WebClient wc = new WebClient();
                wc.Credentials = CredentialCache.DefaultCredentials;
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("companyId", dept.InnerPhone);
                
                string apiUrl = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("TrainApiAddress", "Train");
                wc.Headers.Add("Content-Type", "application/json;charset=UTF-8");
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(dict);
                string result = wc.UploadString(apiUrl + "/api/api/trainRecord/queryProjectList", "POST", data);
                return Content(result);
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
        }
        public ActionResult GetGJXBoxes()
        {
            try
            {
                DataTable dtData = deptBll.GetDataTable(string.Format("select deviceno,deviceno sno from (select distinct deviceno from PX_TRAINRECORD) t"));
                return Success("获取数据成功", dtData);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
                 
            }
        }
        /// <summary>
        /// 获取人员培训记录(来自培训平台)
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetTrainUsersPageList(Pagination pagination, string queryJson)
        {
            Operator user = OperatorProvider.Provider.Current();
            if(user==null)
            {
                return Error("身份票据已过期!");
            }
            DepartmentEntity dept = deptBll.GetEntity(user.OrganizeId);
            if(dept==null)
            {
                return Error("当前用户部门不存在！");
            }
            if(string.IsNullOrWhiteSpace(dept.InnerPhone))
            {
                return Error("当前机构未配置培训平台单位ID！");
            }
            var watch = CommonHelper.TimerStart();
            
            DataTable dtUsers = deptBll.GetDataTable(string.Format("select account from PX_PUSHRECORD where datatype=0 union select account from HIK_USERRELATION t"));
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            Dictionary<string, string> dict = new Dictionary<string, string>() ;
            dict.Add("companyId", dept.InnerPhone);
            dict.Add("pageIndex", pagination.page.ToString());
            dict.Add("pageSize", pagination.rows.ToString());
            if (dtUsers.Rows.Count>0)
            {
                string accounts = string.Join(",",dtUsers.AsEnumerable().Select(t => t.Field<string>("account")).ToArray());
                dict.Add("userAccounts", accounts);
                dict.Add("type", "2");
            }
            if(!string.IsNullOrWhiteSpace(queryJson))
            {
                var queryParam = queryJson.ToJObject();
                if (!queryParam["deptId"].IsEmpty())
                {
                    string deptId = queryParam["deptId"].ToString();
                    dept = deptBll.GetEntity(deptId);
                    if(dept!=null)
                    {
                        if(!string.IsNullOrWhiteSpace(dept.DeptKey))
                        {
                            deptId = dept.DeptKey.Split('|')[0];
                        }
                        dict.Add("deptId", deptId);
                    }
                }
                if (!queryParam["projectId"].IsEmpty())
                {
                    string projectId = queryParam["projectId"].ToString();
                    dict.Add("projectIds", projectId);
                }
                if (!queryParam["keyword"].IsEmpty() && !queryParam["condition"].IsEmpty())
                {
                    string keyword = queryParam["keyword"].ToString().Trim();
                    string condition = queryParam["condition"].ToString();
                    dict.Add(condition, keyword);
                }
            }
            string apiUrl = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("TrainApiAddress", "Train");
            wc.Encoding = Encoding.UTF8;
            wc.Headers.Add("Content-Type", "application/json;charset=UTF-8");
            string data = Newtonsoft.Json.JsonConvert.SerializeObject(dict);
            string result= wc.UploadString(apiUrl+"/api/api/trainRecord/queryTrainList", "POST", data);
            dynamic dyData = JsonConvert.DeserializeObject<ExpandoObject>(result);
            if (dyData.meta.success)
            {
                int page = 0;
                long total = dyData.data.count;
                int totalCount = int.Parse(total.ToString());

                List<string> lstIdCards = deptBll.GetDataTable("select idcard from tmp_idcards").AsEnumerable().Select(t=>t.Field<string>("idcard")).ToList();
                List<object> lstData = dyData.data.trainList;
                for(int j=0;j< lstData.Count;j++)
                {
                    dynamic row = lstData[j];
                    string idCard = row.idNumber;
                    if(!string.IsNullOrWhiteSpace(idCard) && lstIdCards.Contains(idCard))
                    {
                        lstData.RemoveAt(j);
                        totalCount--;
                    }
                }
                if (totalCount % 1000 == 0)
                {  
                    page = totalCount / pagination.rows;
                }
                else
                {
                    page = totalCount / pagination.rows + 1;
                }
                var JsonData = new
                {
                    rows = lstData,
                    total = page,
                    page = pagination.page,
                    records = totalCount,
                    costtime = CommonHelper.TimerEnd(watch)
                };
                return Content(JsonData.ToJson());
            }
            return Content(result);
        }
        /// <summary>
        /// 获取人员培训记录(来自工具箱消息队列的同步)
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetGJXTrainUsersPageList(Pagination pagination, string queryJson)
        {
            try
            {
                pagination.p_kid = "a.id";
                pagination.p_fields = "a.username,a.sex,a.idcard,a.unitname,a.deptname,a.postname,a.worktype,b.projectname,b.starttime,b.endtime,a.score,a.deviceno,a.account";
                pagination.p_tablename = "PX_TRAINRECORD a left join px_trainproject b on a.projectid=b.id";
                pagination.conditionJson = "1=1";

                if (!string.IsNullOrWhiteSpace(queryJson))
                {
                    var queryParam = queryJson.ToJObject();
                    if (!queryParam["sno"].IsEmpty())
                    {
                        string sno = queryParam["sno"].ToString();
                        pagination.conditionJson += $" and deviceno='{sno}'";

                    }
                    if (!queryParam["keyword"].IsEmpty() && !queryParam["condition"].IsEmpty())
                    {
                        string keyword = queryParam["keyword"].ToString().Trim();
                        string condition = queryParam["condition"].ToString();
                         pagination.conditionJson += $" and {condition} like '%{keyword}%'";
                    }
                }
                var apiUrl = new DataItemDetailBLL().GetItemValue("WebApiUrl", "AppSettings");
                WebClient wc = new WebClient();
                wc.UseDefaultCredentials = true;
                string result = wc.DownloadString(apiUrl + "/synctrain/GetAllIdCardsFormHik");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(result);
                if (dy.code == 0)
                {
                    string idCards = dy.data;
                    if (idCards.Length > 0)
                    {
                        string[] arrIdCard = idCards.Split(',');
                        pagination.conditionJson += string.Format(" and idcard not in(select idcard from tmp_idcards)");
                    }
                }
                var watch = CommonHelper.TimerStart();
                var data = userBLL.GetTrainUsersPageList(pagination, queryJson);
                var JsonData = new
                {
                    rows = data,
                    total = pagination.total,
                    page = pagination.page,
                    records = pagination.records,
                    costtime = CommonHelper.TimerEnd(watch)
                };
                return Content(JsonData.ToJson());
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
        }
        /// <summary>
        /// 获取人员培训记录(来自工具箱消息队列的同步)
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetPushRecordPageList(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "a.id";
            pagination.p_fields = "a.username,a.sex,a.idcard,a.unitname,a.deptname,a.postname,a.worktype,b.projectname,b.starttime,b.endtime,a.score,a.deviceno,time";
            pagination.p_tablename = "PX_PUSHRECORD a left join px_trainproject b on a.projectid=b.id";
            pagination.conditionJson = "1=1";
            if (!string.IsNullOrWhiteSpace(queryJson))
            {
                var queryParam = queryJson.ToJObject();
                if (!queryParam["source"].IsEmpty())
                {
                    string source = queryParam["source"].ToString();
                    if(source== "PX001")
                    {
                        pagination.conditionJson += $" and datatype=0";
                    }
                    else
                    {
                        pagination.conditionJson += $" and datatype=1 and deviceno='{source}'";
                    }
                }
                if (!queryParam["keyword"].IsEmpty() && !queryParam["condition"].IsEmpty())
                {
                    string keyword = queryParam["keyword"].ToString().Trim();
                    string condition = queryParam["condition"].ToString();
                    pagination.conditionJson += $" and {condition} like '%{keyword}%'";
                }
            }
            var watch = CommonHelper.TimerStart();
            var data = userBLL.GetTrainUsersPageList(pagination, queryJson);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return Content(JsonData.ToJson());
        }
    }
}