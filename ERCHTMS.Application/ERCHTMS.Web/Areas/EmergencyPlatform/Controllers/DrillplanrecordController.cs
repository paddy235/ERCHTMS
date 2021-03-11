using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.Busines.EmergencyPlatform;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Code;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System.Data;
using System;
using System.Web;
using System.Linq;
using Aspose.Words;
using BSFramework.Util.Extension;
using System.Net;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Busines.BaseManage;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.QuestionManage;

namespace ERCHTMS.Web.Areas.EmergencyPlatform.Controllers
{
    /// <summary>
    /// 描 述：应急演练记录
    /// </summary>
    public class DrillplanrecordController : MvcControllerBase
    {
        private DrillplanrecordBLL drillplanrecordbll = new DrillplanrecordBLL();
        private DrillassessBLL drillassessbll = new DrillassessBLL();
        private PeopleReviewBLL peoplereviewbll = new PeopleReviewBLL();
        private DrillrecordevaluateBLL drillrecordevaluatebll = new DrillrecordevaluateBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        #region 视图功能
        [HttpGet]
        public ActionResult List()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Files()
        {
            return View();
        }
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }

        /// <summary>
        /// 导入页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }
        /// <summary>
        /// 流程图
        /// </summary>
        /// <returns></returns>
        public ActionResult EvaluateFlow()
        {
            return View();
        }
        /// <summary>
        /// 评估表
        /// </summary>
        /// <returns></returns>
        public ActionResult AssessForm()
        {
            return View();
        }
           /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult HistoryIndex()
        {
            return View();
        }
       
        #endregion

        #region 获取数据

        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string drillplanId = Request["drillplanId"] ?? "";
            string mode = Request["mode"] ?? "";
            string qyearmonth = Request["qyearmonth"] ?? "";
            queryJson = queryJson ?? "";
            pagination.p_kid = "ID";
            pagination.p_fields = @"ylzjfiles, name,drillplanname, departid,nvl(isoverevaluate,0) isoverevaluate,nvl(isoverevaluate,0) isoverevaluatestate,
                                    departname,drilltype,drillmode,drilltime,nvl(isstartconfig,0) isstartconfig,assessperson,assesspersonname,
                                    drilltypename,drillmodename,drillpeoplenumber,drillplace,nodeid,nodename,nvl(iscommit,0) iscommit,
                                    createuserid,createuserdeptcode,createuserorgcode,orgdept,orgdeptid,evaluatedept,isassessrecord,
                                    evaluateroleid,evaluatedeptcode,evaluatedeptid,evaluaterole,orgdeptcode,executepersonid,executepersonname,fxnum,bhnum";
            pagination.p_tablename = @" (select t.*,b.executepersonid,b.executepersonname,c.fxnum,d.bhnum  from mae_drillplanrecord t left join mae_drillplan  b  on t.drillplanid =b.id left join ( select count(1) fxnum,relevanceid  from bis_questioninfo group by relevanceid ) c on t.id = c.relevanceid
left join (select count(1) bhnum, relevanceid  from bis_questioninfo where flowstate = '流程结束' group by relevanceid ) d on t.id = d.relevanceid) a";
            pagination.conditionJson = string.Format("(iscommit=1 or createuserid='{0}' or  executepersonid ='{0}')", user.UserId);
            
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                if (!string.IsNullOrEmpty(mode))
                {
                    string code = string.Empty;
                    if (user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户"))
                    {
                        code = user.OrganizeCode;
                    }
                    else
                    {
                        code = user.DeptCode;
                    }
                    pagination.conditionJson += string.Format(" and createuserdeptcode  like '{0}%' ", code);
                }
                else
                {
                    string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "orgdeptcode", "CREATEUSERORGCODE");
                    if (!string.IsNullOrEmpty(where))
                    {
                        pagination.conditionJson += " and " + where;
                    }
                }
            }
            if (!string.IsNullOrEmpty(drillplanId))
            {
                pagination.conditionJson += string.Format(" and drillplanId='{0}' ", drillplanId);
            }
            if (!string.IsNullOrEmpty(qyearmonth))
            {
                pagination.conditionJson += string.Format(" and to_char(createdate,'yyyy-MM') ='{0}' ", qyearmonth);
            }
          
            var watch = CommonHelper.TimerStart();
            var data = drillplanrecordbll.GetPageList(pagination, queryJson);
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
        /// <summary>
        /// 查询应急预案历史记录
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetHistoryPageListJson(Pagination pagination, string queryJson) {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            //string drillplanId = Request["drillplanId"] ?? "";
            //queryJson = queryJson ?? "";
            pagination.p_kid = "ID";
            pagination.p_fields = @"ylzjfiles, name,drillplanname, departid,nvl(isoverevaluate,0) isoverevaluate,nvl(isoverevaluate,0) isoverevaluatestate,
                                    departname,drilltype,drillmode,drilltime,nvl(isstartconfig,0) isstartconfig,assessperson,assesspersonname,
                                    drilltypename,drillmodename,drillpeoplenumber,drillplace,nodeid,nodename,nvl(iscommit,0) iscommit,
                                    createuserid,createuserdeptcode,createuserorgcode,orgdept,orgdeptid,evaluatedept,isassessrecord,
                                    evaluateroleid,evaluatedeptcode,evaluatedeptid,evaluaterole,orgdeptcode";
            pagination.p_tablename = "mae_drillplanrecordhistory t";
            pagination.conditionJson += "1=1";
            if (queryJson.Length > 0)
            {
                var queryParam = queryJson.ToJObject();
                if (!queryParam["drillplanId"].IsEmpty())
                {
                    string drillplanId = queryParam["drillplanId"].ToString();
                    pagination.conditionJson += string.Format(" and historyid='{0}' ", drillplanId);
                }
            }
            //if (!string.IsNullOrEmpty(drillplanId))
            //{
            //    pagination.conditionJson += string.Format(" and drillplanId='{0}' ", drillplanId);
            //}
            var watch = CommonHelper.TimerStart();
            var data = drillplanrecordbll.GetHistoryPageListJson(pagination, queryJson);
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
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = drillplanrecordbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取评价流程图
        /// </summary>
        [HttpPost]
        [AjaxOnly]
        public ActionResult GetEvaluateFlow(string keyValue)
        {
            var josnData = drillplanrecordbll.GetEvaluateFlow(keyValue);
            return Content(josnData.ToJson());
        }
        [HttpGet]
        public ActionResult GetListCountByDrillplanId(string drillplanId)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 1;
            pagination.p_kid = "ID";
            pagination.p_fields = "DEPARTNAME,NAME,DRILLPLANNAME,DRILLTYPENAME,DRILLMODENAME,DRILLTIME,DRILLPLACE,drillpeoplenumber,drillplanId";
            pagination.p_tablename = "V_MAE_DRILLPLANRECORD t";

            pagination.sidx = "ID";

            pagination.conditionJson += string.Format(" drillplanId='{0}' ", drillplanId);
            var data = drillplanrecordbll.GetPageList(pagination, "");
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取评估记录列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAssessRecordList(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            pagination.p_kid = "ID";
            pagination.p_fields = @"createdate, createuserid,createusername, createuserdeptcode,createuserorgcode,modifydate,
                                    modifyuserid,modifyusername,assessrecordresult,assessrecordopinion,assessrecordperson,
                                    assessrecordpersonid,assessrecordtime,assessrecordsignimg,drillrecordid,assessrecorddept,assessrecorddeptid";
            pagination.p_tablename = "mae_drillrecordassess t";
            pagination.conditionJson = "1=1";
            var watch = CommonHelper.TimerStart();
            var data = drillplanrecordbll.GetAssessRecordList(pagination, queryJson);
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
        /// <summary>
        /// 获取评价记录列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetEvaluateList(Pagination pagination, string queryJson) {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
         
            pagination.p_kid = "ID";
            pagination.p_fields = @"createdate, createuserid,createusername, createuserdeptcode,createuserorgcode,modifydate,
                                    modifyuserid,modifyusername,evaluatescore,evaluateopinion,evaluateperson,
                                    evaluatepersonid,evaluatetime,singimg,drillrecordid,evaluatedept,evaluatedeptid";
            pagination.p_tablename = "mae_drillrecordevaluate t";
            pagination.conditionJson = "1=1";
            var watch = CommonHelper.TimerStart();
            var data = drillplanrecordbll.GetEvaluateList(pagination, queryJson);
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
        /// <summary>
        /// 根据用户Id判断该用户是否进行了评价
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetEvaluateRecordByUserId(string UserId,string DrillPlanRecordId)
        {
           var data= drillrecordevaluatebll.GetList().Where(x => x.DrillRecordId == DrillPlanRecordId && x.EvaluatePersonId == UserId).ToList().Count;
           return Content(data.ToJson());
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            try
            {
                string file = "";
                var data = drillplanrecordbll.GetEntity(keyValue);
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (null != data)
                {
                    WebClient wc = new WebClient();
                    wc.Credentials = CredentialCache.DefaultCredentials;
                    System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                    nc.Add("json", Newtonsoft.Json.JsonConvert.SerializeObject(new { UserId = user.UserId, Data = keyValue }));
                    string appUrl = new DataItemDetailBLL().GetItemValue("yjbzUrl");

                    if (!string.IsNullOrEmpty(appUrl))
                    {
                        Byte[] result = wc.UploadValues(new Uri(appUrl + "/Training/GetFileList"), nc);
                        file = System.Text.Encoding.UTF8.GetString(result);
                    }
                }
                var jsondata = new
                {
                    data = data,
                    file = file
                };
                return ToJsonResult(jsondata);
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
        }
        /// <summary>
        /// 获取历史记录实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
         [HttpGet]
        public ActionResult GetHistoryFormJson(string keyValue)
        {
            try
            {
                var data = drillplanrecordbll.GetHistoryEntity(keyValue);
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                WebClient wc = new WebClient();
                wc.Credentials = CredentialCache.DefaultCredentials;
                System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                nc.Add("json", Newtonsoft.Json.JsonConvert.SerializeObject(new { UserId = user.UserId, Data = keyValue }));
                string appUrl = new DataItemDetailBLL().GetItemValue("yjbzUrl");
                string file = "";
                if (!string.IsNullOrEmpty(appUrl))
                {
                    Byte[] result = wc.UploadValues(new Uri(appUrl + "/Training/GetFileList"), nc);
                    file = System.Text.Encoding.UTF8.GetString(result);
                }
                var jsondata = new
                {
                    data = data,
                    file = file
                };
                return ToJsonResult(jsondata);
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        #region 获取评估表模板
        /// <summary>
        /// 获取评估表模板
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetEvaluationTemplate()
        {
            string result = string.Empty;
            Operator curUser = OperatorProvider.Provider.Current();
            //集合
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'AssessTemplate'"); //评估表模板

            if (itemlist.Count() > 0)
            {
                string itemname = "html_"+curUser.OrganizeId;
                var data = itemlist.Where(p => p.ItemName == itemname).FirstOrDefault();
                if (null != data)
                {
                    string virtualpath = data.ItemValue; //模板文件路径

                    string realpath = Server.MapPath(virtualpath);

                    //存在文件
                    if (System.IO.File.Exists(realpath))
                    {
                        StreamReader reader = new StreamReader(realpath, System.Text.Encoding.Default);
                        result = reader.ReadToEnd();
                        reader.Close();
                    }
                    else
                    {
                        realpath = Server.MapPath("~/Resource/ExcelTemplate/通用电厂版本评估表.txt");
                        if (System.IO.File.Exists(realpath))
                        {
                            StreamReader reader = new StreamReader(realpath, System.Text.Encoding.Default);
                            result = reader.ReadToEnd();
                            reader.Close();
                        }
                        else { return Error("无相应的模板文件!"); }
                    }
                }
                else
                {
                    string realpath = Server.MapPath("~/Resource/ExcelTemplate/通用电厂版本评估表.txt");
                    if (System.IO.File.Exists(realpath))
                    {
                        StreamReader reader = new StreamReader(realpath, System.Text.Encoding.Default);
                        result = reader.ReadToEnd();
                        reader.Close();
                    }
                    else
                    {
                        return Error("无相应的模板文件!");
                    }
                }
            }
            else
            {
                string realpath = Server.MapPath("~/Resource/ExcelTemplate/通用电厂版本评估表.txt");
                if (System.IO.File.Exists(realpath))
                {
                    StreamReader reader = new StreamReader(realpath, System.Text.Encoding.Default);
                    result = reader.ReadToEnd();
                    reader.Close();
                }
                else
                {
                    return Error("无相应的模板文件!");
                }
            }
            var jsondata = new { result = result };
            return Content(jsondata.ToJson());
        }  
        #endregion

        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            foreach (var item in keyValue.Split(','))
            {
                drillplanrecordbll.RemoveForm(item);
            }
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, DrillplanrecordEntity entity)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (entity.IsCommit == 1)
                {
                    Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
                    string moduleName = "厂级演练记录评价";
                    ManyPowerCheckEntity mpcEntity = null;
                    if (!string.IsNullOrWhiteSpace(entity.DrillLevel))
                    {
                        switch (entity.DrillLevel)
                        {
                            case "厂级":
                                moduleName = "厂级演练记录评价";
                                break;
                            case "部门级":
                                moduleName = "部门级演练记录评价";
                                break;
                            case "班组级":
                                moduleName = "班组级演练记录评价";
                                break;
                            default:
                                break;
                        }
                    }
                    if (string.IsNullOrWhiteSpace(entity.CREATEUSERID) || string.IsNullOrWhiteSpace(entity.CREATEUSERDEPTCODE)) {
                        entity.Create();
                    }
                    var IsGdxy = new DataItemDetailBLL().GetDataItemListByItemCode("'VManager'").ToList();
                    if (IsGdxy.Count > 0)
                    {
                       
                    }
                    else
                    {
                        //drillplanrecordbll.SaveForm(keyValue, entity);
                        mpcEntity = peoplereviewbll.CheckEvaluateForNextFlow(curUser, moduleName, entity);
                        if (null != mpcEntity)
                        {
                            entity.EvaluateDept = mpcEntity.CHECKDEPTNAME;
                            entity.EvaluateDeptId = mpcEntity.CHECKDEPTID;
                            entity.EvaluateDeptCode = mpcEntity.CHECKDEPTCODE;
                            entity.EvaluateRoleId = mpcEntity.CHECKROLEID;
                            entity.EvaluateRole = mpcEntity.CHECKROLENAME;
                            entity.NodeId = mpcEntity.ID;
                            entity.NodeName = mpcEntity.FLOWNAME;
                            entity.IsStartConfig = 1;
                            entity.IsOverEvaluate = 0;

                            //DataTable dt = new UserBLL().GetUserAccountByRoleAndDept(curUser.OrganizeId, mpcEntity.CHECKDEPTID, mpcEntity.CHECKROLENAME);
                            //var userAccount = dt.Rows[0]["account"].ToString();
                            //var userName = dt.Rows[0]["realname"].ToString();
                            //JPushApi.PushMessage(userAccount, userName, "WB015", entity.ID);
                        }
                        else
                        {
                            entity.IsStartConfig = 0;
                            entity.IsOverEvaluate = 0;
                            //未配置审核项
                            entity.EvaluateDept = "";
                            entity.EvaluateDeptId = "";
                            entity.EvaluateDeptCode = "";
                            entity.EvaluateRoleId = "";
                            entity.NodeId = "";
                            entity.NodeName = "";
                            //OutsouringengineerEntity engineerEntity = new OutsouringengineerBLL().GetEntity(entity.OUTENGINEERID);
                            //engineerEntity.ENGINEERSTATE = "002";
                            //engineerEntity.PLANENDDATE = entity.APPLYRETURNTIME;
                            //new OutsouringengineerBLL().SaveForm(engineerEntity.ID, engineerEntity);
                        }
                    }
                }
                if (entity.IsCommit == 1)
                {
                    HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL(); //流程业务对象
                    QuestionInfoBLL questioninfobll = new QuestionInfoBLL();
                    var dtHid = questioninfobll.GeQuestiontByRelevanceId(keyValue, "问题登记");

                    string primaryKey = string.Empty;

                    string reformpeopleid = string.Empty; //整改人

                    string createuserid = string.Empty; //创建人id

                    foreach (DataRow row in dtHid.Rows)
                    {
                        primaryKey = row["id"].ToString();

                        reformpeopleid = row["reformpeople"].ToString();

                        createuserid = row["createuserid"].ToString();

                        //此处需要判断当前人是否为安全管理员
                        string wfFlag = string.Empty;
                        //参与人员
                        string participant = string.Empty;

                        wfFlag = "1"; //到整改

                        participant = reformpeopleid;  //整改人

                        if (!string.IsNullOrEmpty(participant))
                        {
                            int count = htworkflowbll.SubmitWorkFlow(primaryKey, participant, wfFlag, createuserid);

                            if (count > 0)
                            {
                                htworkflowbll.UpdateFlowStateByObjectId("bis_questioninfo", "flowstate", primaryKey);  //更新业务流程状态
                            }
                        }
                    }
                }
                drillplanrecordbll.SaveForm(keyValue, entity);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {

                return Success(ex.Message);
            }
           
        }
        /// <summary>
        /// 保存演练记录评价表单
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult EvaluateSaveForm(string keyValue, DrillrecordevaluateEntity entity) 
        {
            try
            {
                string DrillRecordId = Request.Form["DrillRecordId"].ToString();
                string ASSESSDATA = Request.Form["ASSESSDATA"].ToString();
                var user = new UserBLL().GetUserInfoEntity(entity.EvaluatePersonId);
                if (user != null) {
                    entity.SingImg = HttpUtility.UrlDecode(entity.SingImg);
                    entity.EvaluateDept = user.DeptName;
                    entity.EvaluateDeptId = user.DepartmentId;
                }
                drillrecordevaluatebll.SaveForm(keyValue, entity);
                //更新应急演练记录表
                var dentity = drillplanrecordbll.GetEntity(DrillRecordId);
                if (null != dentity) 
                {
                    dentity.ASSESSDATA = ASSESSDATA;
                    drillplanrecordbll.SaveForm(DrillRecordId, dentity);
                }
                return Success("操作成功。");
            }
            catch (Exception ex)
            {

                return Error(ex.Message);
            }
           
        }

          /// <summary>
        /// 保存演练记录评估表单
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult AssessRecordSaveForm(string keyValue, DrillrecordAssessEntity entity)
        {
            try
            {
                string DrillRecordId = Request.Form["DrillRecordId"].ToString();
                string ASSESSDATA = Request.Form["ASSESSDATA"].ToString();
                string IsAssessRecord = Request.Form["IsAssessRecord"].ToString();
                string AssessPerson = Request.Form["AssessPerson"].ToString();
                string AssessPersonName = Request.Form["AssessPersonName"].ToString();
                var user = new UserBLL().GetUserInfoEntity(entity.AssessRecordPersonId);
                if (user != null)
                {
                    entity.AssessRecordDept = user.DeptName;
                    entity.AssessRecordDeptId = user.DepartmentId;
                }
                entity.AssessRecordSignImg = HttpUtility.UrlDecode(entity.AssessRecordSignImg);
                var dentity = drillplanrecordbll.GetEntity(DrillRecordId);
                
                //评估通过
                if (entity.AssessRecordResult == "1")
                {
                    drillrecordevaluatebll.AssessRecordSaveForm(keyValue, entity);
                    //更新应急演练记录表
                    if (null != dentity)
                    {
                        dentity.IsAssessRecord = IsAssessRecord;
                        dentity.AssessPersonName = AssessPersonName;
                        dentity.AssessPerson = AssessPerson;
                        dentity.ASSESSDATA = ASSESSDATA;
                        drillplanrecordbll.SaveForm(DrillRecordId, dentity);
                    }
                }
                //评估不通过保存历史记录
                else {

                    if (null != dentity)
                    {
                        var str = JsonConvert.SerializeObject(dentity);
                        DrillplanrecordHistoryEntity hrd = JsonConvert.DeserializeObject<DrillplanrecordHistoryEntity>(str);
                        hrd.ID = Guid.NewGuid().ToString();
                        hrd.HistoryId = dentity.ID;
                        drillplanrecordbll.SaveHistoryForm(hrd.ID, hrd);
                        entity.DrillRecordId = hrd.ID;
                        drillrecordevaluatebll.AssessRecordSaveForm(keyValue, entity);

                        dentity.IsCommit = 0;
                        dentity.IsAssessRecord = "0";
                        dentity.AssessPersonName = "";
                        dentity.AssessPerson = "";
                        dentity.ASSESSDATA = ASSESSDATA;
                        drillplanrecordbll.SaveForm(DrillRecordId, dentity);
                    }
                }
                return Success("操作成功。");
            }
            catch (Exception ex)
            {

                return Error(ex.Message);
            }
        
        }
        /// <summary>
        /// 导入步骤
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ImportStep()
        {
            try
            {
                string message = "请选择格式正确的文件再导入!";
                int count = HttpContext.Request.Files.Count;
                string purpose = "";
                string purpose2 = "";
                string purpose3 = "";
                List<object> list = new List<object>();
                if (count > 0)
                {
                    HttpPostedFileBase file = HttpContext.Request.Files[0];
                    if (string.IsNullOrEmpty(file.FileName))
                    {
                        return Content(new AjaxResult { type = ResultType.error, message = message }.ToJson());
                    }
                    if (!(file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xls") || file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xlsx")))
                    {
                        return Content(new AjaxResult { type = ResultType.error, message = message }.ToJson());
                    }
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName));
                    Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                    wb.Open(Server.MapPath("~/Resource/temp/" + fileName));
                    Aspose.Cells.Cells cells = wb.Worksheets[0].Cells;
                    DataTable dt = cells.ExportDataTable(0, 0, cells.MaxDataRow + 1, cells.MaxColumn + 1, false);
                    purpose = dt.Rows[0][1].ToString();
                    purpose2 = dt.Rows[1][1].ToString();
                    purpose3 = dt.Rows[2][1].ToString();
                    for (int i = 4; i < dt.Rows.Count; i++)
                    {
                        list.Add(new
                        {
                            sortid = dt.Rows[i][0].ToString(),
                            content = dt.Rows[i][1].ToString()
                        });
                    }
                }
                var resultdata = new
                {
                    purpose = purpose,
                    purpose2 = purpose2,
                    purpose3 = purpose3,
                    steplist = list
                };
                return Content(new AjaxResult { type = ResultType.success, message = message, resultdata = resultdata }.ToJson());
            }
            catch (Exception ex)
            {

                return Error(ex.Message);
            }
           
        }
        #endregion


        #region 数据导出
        /// <summary>
        /// 导出用户列表
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出应急演练计划")]
        public ActionResult ExportDrillplanrecordList(string condition, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            Pagination pagination = new Pagination();
            string drillplanId = Request["drillplanId"] ?? "";
            string mode = Request["mode"] ?? "";
            string qyearmonth = Request["qyearmonth"] ?? "";
            queryJson = queryJson ?? "";
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "ID";
            pagination.p_fields = "ORGDEPT,DEPARTNAME,NAME,DRILLPLANNAME,DRILLTYPENAME,DRILLMODENAME,DRILLTIME,DRILLPLACE,drillpeoplenumber";
            pagination.p_tablename = " (select t.*,b.executepersonid,b.executepersonname  from V_MAE_DRILLPLANRECORD t left join mae_drillplan  b  on t.drillplanid =b.id) a";
            pagination.sidx = "ID";
            pagination.conditionJson = string.Format("(iscommit=1 or createuserid='{0}' or  executepersonid ='{0}')", user.UserId);
            #region 权限校验
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                var queryParam = queryJson.ToJObject();
                if (!queryParam["mode"].IsEmpty())
                {
                    string code = string.Empty;
                    if (user.RoleName.Contains("公司级用户") || user.RoleName.Contains("厂级部门用户"))
                    {
                        code = user.OrganizeCode;
                    }
                    else
                    {
                        code = user.DeptCode;
                    }
                    pagination.conditionJson += string.Format(" createuserdeptcode  like '{0}%' ", code);
                }
                else 
                {
                    string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "CREATEUSERDEPTCODE", "CREATEUSERORGCODE");
                    if (!string.IsNullOrEmpty(where))
                    {
                        pagination.conditionJson += " and " + where;
                    }
                }

                if (!queryParam["qyearmonth"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and to_char(createdate,'yyyy-MM') ='{0}' ", queryParam["qyearmonth"].ToString());
                }
            }
            #endregion
            if (!string.IsNullOrEmpty(drillplanId))
            {
                pagination.conditionJson += string.Format(" and drillplanId='{0}' ", drillplanId);
            }
            if (!string.IsNullOrEmpty(qyearmonth))
            {
                pagination.conditionJson += string.Format(" and to_char(createdate,'yyyy-MM') ='{0}' ", qyearmonth);
            }
            var data = drillplanrecordbll.GetPageList(pagination, queryJson);

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "应急演练记录清单";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "应急演练记录清单.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();
            listColumnEntity.Add(new ColumnEntity() { Column = "ORGDEPT".ToLower(), ExcelColumn = "组织部门" });
            listColumnEntity.Add(new ColumnEntity() { Column = "DEPARTNAME".ToLower(), ExcelColumn = "演练部门" });
            listColumnEntity.Add(new ColumnEntity() { Column = "NAME".ToLower(), ExcelColumn = "演练名称" });
            listColumnEntity.Add(new ColumnEntity() { Column = "DRILLPLANNAME".ToLower(), ExcelColumn = "演练预案名称" });
            listColumnEntity.Add(new ColumnEntity() { Column = "DRILLTYPENAME".ToLower(), ExcelColumn = "演练预案类型" });
            listColumnEntity.Add(new ColumnEntity() { Column = "DRILLMODENAME".ToLower(), ExcelColumn = "演练方式" });
            listColumnEntity.Add(new ColumnEntity() { Column = "DRILLTIME".ToLower(), ExcelColumn = "演练时间" });
            listColumnEntity.Add(new ColumnEntity() { Column = "DRILLPLACE".ToLower(), ExcelColumn = "演练地点" });
            listColumnEntity.Add(new ColumnEntity() { Column = "DRILLPEOPLENUMBER".ToLower(), ExcelColumn = "参与人数" });
            excelconfig.ColumnEntity = listColumnEntity;

            //调用导出方法
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("导出成功。");
        }


        /// <summary>
        /// 导出评估表
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出应急评估表")]
        [HttpPost]
        public ActionResult ExportDrillAssess(string keyValue)
        {
            HttpResponse resp = System.Web.HttpContext.Current.Response;

             string realpath =string.Empty;
            string result = string.Empty;
            Operator curUser = OperatorProvider.Provider.Current();
            //集合
            var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'AssessTemplate'"); //评估表模板

            if (itemlist.Count() > 0)
            {
                string itemname = "word_" + curUser.OrganizeId;
                var data = itemlist.Where(p => p.ItemName == itemname).FirstOrDefault();
                if (null != data)
                {
                    string virtualpath = data.ItemValue; //模板文件路径

                    realpath = Server.MapPath(virtualpath);
                }
                else {
                    realpath = Server.MapPath("~/Resource/ExcelTemplate/通用电厂版本评估表.doc");
                }
            }
            else
            {
                realpath = Server.MapPath("~/Resource/ExcelTemplate/通用电厂版本评估表.doc");
            }


            //存在文件
            if (System.IO.File.Exists(realpath))
            {
                string fileName = "应急演练记录评估表_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
                Aspose.Words.Document doc = new Aspose.Words.Document(realpath);
                DocumentBuilder builder = new DocumentBuilder(doc);
                Aspose.Words.Tables.Table table = doc.GetChildNodes(NodeType.Table, true)[0] as Aspose.Words.Tables.Table;
                //var entity = drillplanrecordbll.GetEntity(keyValue);
                string jsondata = Request.Form["assessdata"];//entity.ASSESSDATA;

             
                string createuserid = string.Empty;
                DataTable dt = new DataTable();
                if (!string.IsNullOrEmpty(jsondata))
                {
                    #region 遍历元素
                    JArray jarray = (JArray)JsonConvert.DeserializeObject(jsondata);

                    foreach (JObject jobj in jarray)
                    {
                        string type = jobj["type"].ToString();
                        string key = jobj["key"].ToString();
                        string value = jobj["value"].ToString();
                        if (type == "radio")
                        {
                            if (realpath.Contains("京泰") || realpath.Contains("康巴什"))
                            {
                                key = key + "_" + value;
                            }
                            else
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    if (value != i.ToString())
                                    {
                                        var newkey = key + i.ToString();
                                        dt.Columns.Add(newkey);
                                    }
                                }
                                key = key + value;
                            }
                            
                        }
                        //评估表创建者
                        if (key == "CreateUserId")
                        {
                            createuserid = value;
                        }
                        dt.Columns.Add(key);
                    }
                    DataRow row = dt.NewRow();
                    foreach (JObject jobj in jarray)
                    {
                        string type = jobj["type"].ToString();
                        string key = jobj["key"].ToString();
                        string value = jobj["value"].ToString();
                        if (type == "radio")
                        {
                            if (realpath.Contains("京泰") || realpath.Contains("康巴什"))
                            {
                                key = key + "_" + value;
                                row[key] = "☑";
                            }
                            else
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    if (value != i.ToString())
                                    {
                                        var newkey = key + i.ToString();
                                        row[newkey] = "□";
                                    }
                                }
                                key = key + value;
                                row[key] = "☑";
                                
                            }
                            
                        }
                        else
                        {
                            row[key] = value;
                        }
                    }
                    dt.Rows.Add(row);
                    #endregion

                    //特殊处理
                    var special = itemlist.Where(p => p.ItemName == "special_export_orgid").FirstOrDefault();
                    #region 特殊处理
                    if (null != special)
                    {
                        if (special.ItemValue == curUser.OrganizeId)
                        {
                            //国电荥阳
                            int rowNum = table.Rows.Count;
                            int lastRowIndex = rowNum - 1;
                            //创建人签名
                            if (!string.IsNullOrEmpty(createuserid))
                            {
                                UserInfoEntity createUser = new UserBLL().GetUserInfoEntity(createuserid);
                                string singImg = "~" + createUser.SignImg;
                                string realPath = Server.MapPath(singImg);
                                if (System.IO.File.Exists(realPath))
                                {
                                    builder.MoveToCell(0, rowNum - 2, 1, 0);
                                    builder.InsertImage(realPath, Aspose.Words.Drawing.RelativeHorizontalPosition.Margin, 1, Aspose.Words.Drawing.RelativeVerticalPosition.Margin, 120, 80, 32, Aspose.Words.Drawing.WrapType.Inline);
                                }
                                else
                                {
                                    EditCell(table, doc, rowNum - 2, 1, !string.IsNullOrEmpty(createUser.RealName) ? createUser.RealName : "");
                                }
                            }

                            var evaluatedata = drillplanrecordbll.GetAssessList(keyValue); //评价内容
                            if (evaluatedata.Count() > 0)
                            {
                                //新增行
                                for (int eindex = 0; eindex < evaluatedata.Count(); eindex++)
                                {
                                    //最后一个评估人，不用再新增行
                                    if (evaluatedata.Count() - 1 > eindex)
                                    {
                                        var row1 = table.Rows[lastRowIndex];
                                        var cloneRow = row1.Clone(true);

                                        table.Rows.Insert(lastRowIndex + eindex, cloneRow); //行数据
                                    }
                                }

                                int i = 0;
                                foreach (DrillrecordAssessEntity evaluateEntity in evaluatedata)
                                {
                                    EditCell(table, doc, lastRowIndex + i, 0, "评估人");
                                    if (evaluatedata.Count() - 1 == i)
                                    {
                                        EditCell(table, doc, lastRowIndex + i, 0, "总指挥");
                                    }
                                    //评估人对象
                                    var userInfo = new UserBLL().GetEntity(evaluateEntity.CreateUserId);
                                    string singImg = "~" + userInfo.SignImg;
                                    string realPath = Server.MapPath(singImg);
                                    if (System.IO.File.Exists(realPath))
                                    {
                                        builder.MoveToCell(0, lastRowIndex + i, 1, 0);
                                        builder.InsertImage(realPath, Aspose.Words.Drawing.RelativeHorizontalPosition.Margin, 1, Aspose.Words.Drawing.RelativeVerticalPosition.Margin, 120, 80, 32, Aspose.Words.Drawing.WrapType.Inline);
                                    }
                                    else
                                    {
                                        EditCell(table, doc, lastRowIndex + i, 1, !string.IsNullOrEmpty(userInfo.RealName) ? userInfo.RealName : "");
                                    }
                                    EditCell(table, doc, lastRowIndex + i, 2, !string.IsNullOrEmpty(userInfo.DutyName) ? userInfo.DutyName : "");
                                    EditCell(table, doc, lastRowIndex + i, 3, "联系方式");
                                    EditCell(table, doc, lastRowIndex + i, 4, !string.IsNullOrEmpty(userInfo.Mobile) ? userInfo.Mobile : "");

                                    i++;
                                }
                            }
                            else
                            {
                                //删除模板最后一行
                                table.Rows[lastRowIndex].Remove(); //删除最后一行
                            }
                        }
                    }
                    #endregion
                    doc.MailMerge.Execute(dt);
                    doc.MailMerge.DeleteFields();
                    doc.Save(resp, Server.UrlEncode(fileName), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));

                    return Success("导出成功!");
                }
                else 
                {
                    return Content("<script>alert('评估数据未保存,无法导出!');</script>");
                }
            }
            else
            {
                return Error("无相应的模板文件!");
            }
        }
        #endregion


        public Aspose.Words.Tables.Table EditCell(Aspose.Words.Tables.Table table, Document doc, int row, int cell, string value) 
        {
            if (null != table.Rows[row]) 
            {
                Aspose.Words.Tables.Cell c = table.Rows[row].Cells[cell];
                Paragraph p = new Paragraph(doc);
                p.AppendChild(new Run(doc, value));
                p.ParagraphFormat.Style.Font.Size = 10;
                p.ParagraphFormat.Style.Font.Name = "宋体";
                p.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                c.FirstParagraph.Remove();
                c.AppendChild(p);
                table.Rows[row].Cells[cell].Remove();
                table.Rows[row].Cells.Insert(cell, c);
            }
            return table;
        }

        public  Aspose.Words.Tables.Row CreatRow(int columnCount, string[] columnValues, Document doc)
         {
             Aspose.Words.Tables.Row r2 = new Aspose.Words.Tables.Row(doc);
             for (int i = 0; i < columnCount; i++) 
             {
                 if (columnValues.Length > i)
                 {
                     var cell = CreateCell(columnValues[i], doc);
                     r2.Cells.Add(cell);
                 }
                 else {
                     var cell = CreateCell("", doc);
                     r2.Cells.Add(cell);
                 }
             }
             return r2;
         }

        public Aspose.Words.Tables.Cell CreateCell(string value, Document doc) 
         {
             Aspose.Words.Tables.Cell c1 = new Aspose.Words.Tables.Cell(doc);
             c1.CellFormat.HorizontalMerge = Aspose.Words.Tables.CellMerge.None;
             c1.CellFormat.VerticalMerge = Aspose.Words.Tables.CellMerge.None;
             Aspose.Words.Paragraph p = new Paragraph(doc);
             p.AppendChild(new Run(doc, value));
             c1.AppendChild(p);
             return c1;
         }
    }
}
