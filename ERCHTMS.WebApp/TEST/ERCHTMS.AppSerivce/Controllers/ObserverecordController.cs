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
using ERCHTMS.Busines.Observerecord;
using ERCHTMS.Entity.Observerecord;
using ERCHTMS.Cache;
using ERCHTMS.AppSerivce.Model;

namespace ERCHTMS.AppSerivce.Controllers
{

    /// <summary>
    /// 观察记录
    /// </summary>
    public class ObserverecordController : BaseApiController
    {
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private ObserverecordBLL observerecordbll = new ObserverecordBLL();
        private ObservecategoryBLL observecategorybll = new ObservecategoryBLL();
        private ObservesafetyBLL observesafetybll = new ObservesafetyBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();
        private ObsplanBLL obsplanbll = new ObsplanBLL();
        private DepartmentCache departmentCache = new DepartmentCache();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        /// <summary>
        /// 获取观察记录列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetObsList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                long pageIndex = dy.pageindex;
                long pageSize = dy.pagesize;
                string action = dy.data.action;//0待提交 1全部
                string starttime = dy.data.starttime;
                string endtime = dy.data.endtime;
                string workname = dy.data.workname;
                string obspeople = dy.data.obspeople;
                string workunitid = dy.data.workunitid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                Pagination pagination = new Pagination();
                pagination.page = int.Parse(pageIndex.ToString());
                pagination.rows = int.Parse(pageSize.ToString());
                pagination.p_kid = "t.id";
                pagination.p_fields = @"t.workname,t.workunitid,t.workunit,
                                       t.workpeople,t.workpeopleid,t.createuserid,
                                       t.createuserorgcode,t.createuserdeptcode,
                                       t.createdate,t.workarea,t.workareaid,
                                       t.workplace,t.obsendtime,t.obsstarttime,
                                       t.obsperson,t.obspersonid,t.obsplanid,t.iscommit";
                pagination.p_tablename = @"bis_observerecord t";
                pagination.sidx = "t.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                pagination.conditionJson += " 1=1 ";

                string where = string.Empty;
                //通过菜单名称和菜单Encode查询菜单Id
                string sql = string.Format("select * from BASE_MODULE t where t.fullname='观察记录' and t.encode='ObsSafetyRecord'");
                var dtMenu = observerecordbll.GetMenuTable(sql);
                if (dtMenu.Rows.Count > 0)
                {
                    //根据当前用户对模块的权限获取记录
                    where = new AuthorizeBLL().GetModuleDataAuthority(user, dtMenu.Rows[0]["moduleid"].ToString(), "createuserdeptcode", "createuserorgcode");
                    if (!string.IsNullOrWhiteSpace(where))
                    {
                        pagination.conditionJson += " and " + where;
                    }
                }
                //待提交的观察记录
                if (action == "0")
                {
                    pagination.conditionJson += string.Format(" and t.iscommit=0 and t.createuserid='{0}'", user.UserId);
                }
                else
                {
                    pagination.conditionJson += string.Format(" and (t.iscommit=1 or t.createuserid='{0}')", user.UserId);
                }
                string queryJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    starttime = starttime,
                    endtime = endtime,
                    workname = workname,
                    obspeople = obspeople,
                    workunitid = workunitid
                });
                var data = observerecordbll.GetPageList(pagination, queryJson);
                return new { Code = 0, Count = pagination.records, Info = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }

        }
        /// <summary>
        /// 获取观察记录详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetObsInfo([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string obsid = dy.data.obsid;
                ObserverecordEntity obsRecord = observerecordbll.GetEntity(obsid);
                //观察类别
                var typeData = observerecordbll.GetObsTypeData(obsid);
                //安全行为
                List<object> SafeData = new List<object>();
                //不安全行为
                List<object> NotSafeData = new List<object>();
                var data = dataitemdetailbll.GetDataItemListByItemCode("'ObsType'").ToList();
                for (int i = 0; i < data.Count; i++)
                {
                    var safeData = observesafetybll.GetDataByType("1", data[i].ItemName, obsid);
                    if (safeData.Rows.Count > 0)
                    {
                        var item = new { name = data[i].ItemName, list = safeData };
                        SafeData.Add(item);
                    }
                    var notSafe = observesafetybll.GetDataByType("0", data[i].ItemName, obsid);
                    if (notSafe.Rows.Count > 0)
                    {
                        var notItem = new { name = data[i].ItemName, list = notSafe };
                        NotSafeData.Add(notItem);
                    }
                }
                //获取相关附件
                var files = new FileInfoBLL().GetFiles(obsid);
                string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                foreach (DataRow dr in files.Rows)
                {
                    dr["filepath"] = dr["filepath"].ToString().Replace("~/", webUrl + "/");
                }
                var result = new
                {
                    obsRecordEntity = obsRecord,
                    obsTypeData = typeData,
                    SafeData = SafeData,
                    NotSafeData = NotSafeData,
                    files = files
                };
                return new
                {
                    Code = 0,
                    Count = 1,
                    Info = "获取数据成功",
                    data = result
                };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 是否可新增
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetObsAddByUser([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid;
                string moduleid = "6cd3be4c-0232-46fb-bfc4-d550371beee5";
                string encode = "add";
                OperatorProvider.AppUserId = userid;
                AuthorizeBLL authBLL = new AuthorizeBLL();
                Boolean OperAuthority = string.IsNullOrEmpty(authBLL.GetOperAuthority(OperatorProvider.Provider.Current(), moduleid, encode)) ? false : true;
                var JsonData = new
                {
                    operauthority = OperAuthority
                };
                return new { code = 0, info = "获取数据成功", data = JObject.Parse(JsonConvert.SerializeObject(JsonData)) };
            }
            catch (Exception ex)
            {

                return new { Code = -1, Count = 0, Info = ex.Message };
            }

        }
        /// <summary>
        /// 获取观察类别
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetObsType([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string type = dy.data.type;
                var data = dataitemdetailbll.GetDataItemListByItemCode("'" + type + "'").ToList();
                return new { Code = 0, Count = data.Count, Info = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 保存或者提交数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object SaveOrCommitData()
        {
            try
            {
                string res = HttpContext.Current.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string deleteids = dy.data.deleteids;//删除附件id集合
                OperatorProvider.AppUserId = userId;  //设置当前用户
                var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                //string delsafelist = dy.data.delsafelist;
                //观察记录实体
                string obsRecordEntity = JsonConvert.SerializeObject(dy.data.obsrecordentity);
                var recordEntity = JsonConvert.DeserializeObject<ObserverecordEntity>(obsRecordEntity);
                //观察类别
                string observecategory = JsonConvert.SerializeObject(dy.data.observecategory);
                List<ObservecategoryEntity> listCategory = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ObservecategoryEntity>>(observecategory);
                //安全行为与不安全行为
                string safetyList = JsonConvert.SerializeObject(dy.data.safetylist);
                List<ObservesafetyEntity> listSafety = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ObservesafetyEntity>>(safetyList);

                observerecordbll.SaveForm(recordEntity.ID, recordEntity, listCategory, listSafety);
                string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                //处理附件
                if (!string.IsNullOrEmpty(deleteids))
                {
                    DeleteFile(deleteids);
                }
                ////处理删除的安全行为与不安全行为
                //if (!string.IsNullOrEmpty(delsafelist))
                //{
                //    DeleSafeList(delsafelist);
                //}
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
                            fileInfoEntity.RecId = recordEntity.ID;
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
                return new { Code = 0, Count = 0, Info = "操作成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        ///// <summary>
        ///// 删除安全行为与不安全行为
        ///// </summary>
        ///// <param name="delsafelist"></param>
        ///// <returns></returns>
        //public bool DeleSafeList(string delsafelist)
        //{
        //    bool result = false;
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(delsafelist))
        //        {
        //            string[] strArray = delsafelist.Split(',');
        //            foreach (string s in strArray)
        //            {
        //                if (!string.IsNullOrWhiteSpace(s))
        //                    observesafetybll.RemoveForm(s);
        //                else
        //                    continue;
        //            }
        //            result = true;
        //        }
        //        return result;
        //    }
        //    catch (Exception)
        //    {
        //        return result;
        //    }


        //}
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
        /// <summary>
        /// 保存安全行为或不安全行为
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object CommitSafetyData()
        {
            try
            {
                string res = HttpContext.Current.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                string safetyData = JsonConvert.SerializeObject(dy.data.safetydata);
                var safetyEntity = JsonConvert.DeserializeObject<ObservesafetyEntity>(safetyData);
                observesafetybll.SaveForm(safetyEntity.ID, safetyEntity);
                return new { Code = 0, Count = 0, Info = "操作成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 删除安全行为或不安全行为
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object RemoveSafetyData([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string safeId = dy.data.safeid;
                observesafetybll.RemoveForm(safeId);
                return new { Code = 0, Count = 0, Info = "操作成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 删除观察记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object RemoveObsRecord([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string obsId = dy.data.obsid;
                //附件
                var filelist = fileinfobll.GetFiles(obsId);
                if (filelist.Rows.Count > 0)
                {
                    for (int i = 0; i < filelist.Rows.Count; i++)
                    {
                        fileinfobll.RemoveForm(filelist.Rows[i]["fileid"].ToString());
                    }
                }
                //观察记录
                observerecordbll.RemoveForm(obsId);
                return new { Code = 0, Count = 0, Info = "操作成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }


        }

        /// <summary>
        /// 获取观察计划分解任务
        /// </summary>
        /// <param name="json">status 1:已完成 2:待完成即将逾期 3:待完成 4未完成</param>
        /// <returns></returns>
        [HttpPost]
        public object GetPlanWorkList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                long pageIndex = dy.pageindex;
                long pageSize = dy.pagesize;
                string PlanWorkName = res.Contains("planworkname") ? dy.data.planworkname : "";//作业内容
                string PlanArea = res.Contains("planareaid") ? dy.data.planareaid : "";//区域
                string PlanDeptCode = res.Contains("plandeptcode") ? dy.data.plandeptcode : "";//部门或专业
                string PlanYear = res.Contains("planyear") ? dy.data.planyear : DateTime.Now.Year.ToString();//计划年度
                string PlanRiskLevel = res.Contains("planrisklevel") ? dy.data.planrisklevel : "";//风险等级
                string PlanMonth = res.Contains("planmonth") ? dy.data.planmonth : "";//计划月度
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator currUser = OperatorProvider.Provider.Current();
                if (null == currUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                Pagination pagination = new Pagination();
                var tableClass = "bis_obsplan_tz t";
                pagination.p_kid = "t.id tid";
                pagination.p_fields = @" t.planyear,
                                       t.plandept,
                                       t.planspeciaty,
                                       t.plandeptcode,t.plandeptid,
                                       t.planspeciatycode,t.oldplanid,
                                       t.planarea,t.planareacode,
                                       t.planlevel,p.risklevel,
                                       p.workname fjname,
                                       t.workname,p.id pid,
                                       p.obsperson,p.oldworkid,
                                       p.obspersonid,
                                       p.obsnum,p.obsnumtext,
                                       p.obsmonth,
                                       t.createuserid,
                                       t.createuserdeptcode,
                                       t.createuserorgcode,
                                       t.createdate,t.iscommit,p.remark,null status";


                pagination.conditionJson = "t.iscommit='1' and t.ispublic='1' ";
                pagination.p_tablename = string.Format(@"{0}
                                        left join bis_obsplanwork p
                                            on p.planid = t.id", tableClass);
                pagination.sidx = "t.createdate desc,t.id asc";
                pagination.page = int.Parse(pageIndex.ToString());
                pagination.rows = int.Parse(pageSize.ToString());
                if (!currUser.IsSystem)
                {
                    if (currUser.RoleName.Contains("专业级用户") || currUser.RoleName.Contains("班组级用户"))
                    {
                        var d = new DepartmentBLL().GetParentDeptBySpecialArgs(currUser.ParentId, "部门");
                        if (d != null)
                        {
                            pagination.conditionJson += " and t.plandeptcode like '" + d.EnCode + "%'";
                        }
                        else
                        {
                            pagination.conditionJson += " and 0=1";
                        }
                    }
                    else
                    {
                        //通过菜单名称和菜单Encode查询菜单Id
                        string sql = string.Format("select * from BASE_MODULE t where t.fullname='观察计划台账' and t.encode='ObsPlanStanding'");
                        var dtMenu = observerecordbll.GetMenuTable(sql);
                        string authType = string.Empty;
                        if (dtMenu.Rows.Count > 0) {
                            authType = new AuthorizeBLL().GetOperAuthorzeType(currUser, dtMenu.Rows[0]["moduleid"].ToString(), "search");
                        }
                        if (!string.IsNullOrEmpty(authType))
                        {
                            switch (authType)
                            {
                                case "1":
                                    pagination.conditionJson += " and  t.createuserid='" + currUser.UserId + "'";
                                    break;
                                case "2":
                                    pagination.conditionJson += " and t.plandeptcode='" + currUser.DeptCode + "'";
                                    break;
                                case "3":
                                    pagination.conditionJson += " and t.plandeptcode like '" + currUser.DeptCode + "%'";
                                    break;
                                case "4":
                                    pagination.conditionJson += " and t.plandeptcode like '" + currUser.OrganizeCode + "%'";
                                    break;
                                case "5":
                                    pagination.conditionJson += string.Format(" and t.plandeptcode in(select encode from BASE_DEPARTMENT where deptcode like '{0}%')", currUser.NewDeptCode);
                                    break;
                            }
                        }
                        else
                        {
                            pagination.conditionJson += " and 0=1";
                        }
                    }
                }
                string queryJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    PlanWorkName = PlanWorkName,//作业内容
                    PlanArea = PlanArea,//区域
                    PlanDeptCode = PlanDeptCode,//部门或专业
                    PlanYear = PlanYear,//计划年度
                    PlanRiskLevel = PlanRiskLevel,//风险等级
                    PlanMonth = PlanMonth//计划月度
                });
                var data = obsplanbll.GetPageList(pagination, queryJson);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    var dt = obsplanbll.GetObsRecordIsExist(data.Rows[i]["oldplanid"].ToString(), data.Rows[i]["oldworkid"].ToString(), PlanYear);
                    //var obsmonth = data.Rows[i]["obsmonth"].ToString().Split(',');
                    if (dt.Rows.Count > 0)
                    {
                        //for (int j = 0; j < obsmonth.Length; j++)
                        //{

                        if (DateTime.Now.Year.ToString() == PlanYear && DateTime.Now.Month <= Convert.ToInt32(PlanMonth))
                        {
                            if (Convert.ToInt32(dt.Rows[0]["m" + Convert.ToInt32(PlanMonth)]) > 0)
                            {
                                data.Rows[i]["status"] = "1";
                            }
                            else
                            {
                                if (DateTime.Now.Month == Convert.ToInt32(Convert.ToInt32(PlanMonth)))
                                {
                                    var currTime = DateTime.Now;
                                    var lastTime = Convert.ToDateTime(DateTime.Now.AddMonths(1).ToString("yyyy-MM-01")).AddDays(-1);
                                    if (DateTime.Compare(lastTime, currTime) <= 5)
                                    {
                                        data.Rows[i]["status"] = "2";
                                    }
                                    else
                                    {
                                        data.Rows[i]["status"] = "3";
                                    }
                                }
                                else
                                {
                                    data.Rows[i]["status"] = "3";
                                }
                            }
                        }
                        else
                        {
                            if (Convert.ToInt32(dt.Rows[0]["m" + Convert.ToInt32(PlanMonth)]) > 0)
                            {
                                data.Rows[i]["status"] = "1";
                            }
                            else
                            {
                                data.Rows[i]["status"] = "4";
                            }
                        }

                        //}
                    }
                    else
                    {
                        //for (int j = 0; j < obsmonth.Length; j++)
                        //{

                        if (DateTime.Now.Year.ToString() == PlanYear && DateTime.Now.Month <= Convert.ToInt32(PlanMonth))
                        {
                            if (DateTime.Now.Month == Convert.ToInt32(PlanMonth))
                            {
                                var currTime = DateTime.Now;
                                var lastTime = Convert.ToDateTime(DateTime.Now.AddMonths(1).ToString("yyyy-MM-01")).AddDays(-1);
                                if (DateTime.Compare(lastTime, currTime) <= 5)
                                {
                                    data.Rows[i]["status"] = "2";
                                }
                                else
                                {
                                    data.Rows[i]["status"] = "3";
                                }
                            }
                            else
                            {
                                data.Rows[i]["status"] = "3";
                            }
                        }
                        else
                        {

                            data.Rows[i]["status"] = "4";
                        }
                        //}
                    }
                }
                return new { Code = 0, Count = pagination.records, Info = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 获取观察计划风险等级
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetPlanRiskLevel([FromBody]JObject json)
        {

            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string userId = dy.userid;
            OperatorProvider.AppUserId = userId;  //设置当前用户
            Operator currUser = OperatorProvider.Provider.Current();
            List<object> data = new List<object>() { new { itemname = "I级", itenvaule = "I级" },
            new  {  itemname = "II级", itenvaule = "II级"  },
            new  {  itemname = "III级", itenvaule = "III级"  },
            new  {  itemname = "IV级", itenvaule = "IV级"  },
            new  {  itemname = "V级", itenvaule = "V级"  }};
            return new
            {
                code = 0,
                count = 1,
                info = "成功",
                data = data
            };

        }


        /// <summary>
        /// 根据不同角色获取部门
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetInst([FromBody]JObject json)
        {

            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            //选中的所属单位
            string orgid = string.Empty;

            string organizeId = string.Empty;

            string parentId = string.Empty;
            IList<DeptData> result = new List<DeptData>();
            parentId = departmentBLL.GetEntity(curUser.OrganizeId).ParentId;

            List<DepartmentEntity> plist = new List<DepartmentEntity>();
            IList<DeptData> list = new List<DeptData>();
            try
            {
                DepartmentEntity org = null;
                DeptData dept = new DeptData();
                if (curUser.RoleName.Contains("厂级部门") || curUser.RoleName.Contains("公司级"))
                {


                    organizeId = curUser.OrganizeId;
                    org = departmentBLL.GetEntity(organizeId);
                    parentId = org.DepartmentId;

                    plist = departmentBLL.GetList().Where(t => t.ParentId == parentId && (t.Nature == "厂级" || t.Nature == "部门")).OrderBy(t => t.SortCode).ToList();

                }
                else if (curUser.RoleName.Contains("专业级") || curUser.RoleName.Contains("班组级"))
                {
                    var deptEntity = new DepartmentBLL().GetParentDeptBySpecialArgs(curUser.ParentId, "部门");
                    organizeId = deptEntity.DepartmentId;
                    org = departmentBLL.GetEntity(organizeId);
                    parentId = deptEntity.DepartmentId;
                    plist = departmentBLL.GetList().Where(t => t.ParentId == parentId && t.Nature == "部门").OrderBy(t => t.SortCode).ToList();
                }
                else
                {
                    organizeId = curUser.DeptId;
                    org = departmentBLL.GetEntity(organizeId);
                    parentId = curUser.DeptId;
                    plist = departmentBLL.GetList().Where(t => t.ParentId == parentId && t.Nature == "部门").OrderBy(t => t.SortCode).ToList();
                }
                dept.deptid = org.DepartmentId;
                dept.code = org.EnCode;
                dept.newcode = org.DeptCode;
                dept.isorg = 1;
                dept.oranizeid = org.OrganizeId;
                dept.parentcode = "";
                dept.parentid = parentId;
                dept.name = org.FullName;
                dept.isparent = true;

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
                    depts.parentcode = dept.parentcode;
                    list.Add(depts);
                }
                dept.children = list;
                result.Add(dept);


            }
            catch (Exception ex)
            {

                return new
                {
                    code = -1,
                    info = "获取数据失败",
                    count = 0
                };
            }

            return new
            {
                code = 0,
                info = "获取数据成功",
                count = 0,
                data = result
            };
        }
    }
}