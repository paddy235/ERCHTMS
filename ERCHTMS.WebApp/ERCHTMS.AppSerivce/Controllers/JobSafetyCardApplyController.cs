using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ERCHTMS.Busines.DangerousJob;
using ERCHTMS.Busines.DangerousJobConfig;
using ERCHTMS.Entity.DangerousJobConfig;
using ERCHTMS.Entity.DangerousJob;
using BSFramework.Util.WebControl;
using BSFramework.Util;
using ERCHTMS.Code;
using System.Web;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.HighRiskWork;
using System.Data;
using ERCHTMS.Busines.JPush;
using System.IO;
using ERCHTMS.Entity.SystemManage;
using BSFramework.Util.Attributes;
using ERCHTMS.AppSerivce.Model;

namespace ERCHTMS.AppSerivce.Controllers
{
    public class JobSafetyCardApplyController : BaseApiController
    {
        private JobSafetyCardApplyBLL jobSafetyCardApplybll = new JobSafetyCardApplyBLL();
        private ClassStandardConfigBLL classStandardConfigBLL = new ClassStandardConfigBLL();
        private SafetyMeasureConfigBLL safetyMeasureConfigBLL = new SafetyMeasureConfigBLL();
        private SafetyMeasureDetailBLL safetyMeasureDetailBLL = new SafetyMeasureDetailBLL();
        private ManyPowerCheckBLL manyPowerCheckbll = new ManyPowerCheckBLL();
        private DangerousJobFlowDetailBLL dangerousjobflowdetailbll = new DangerousJobFlowDetailBLL();
        private DangerousJobOperateBLL dangerousjoboperatebll = new DangerousJobOperateBLL();
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private FireWaterBLL firewaterbll = new FireWaterBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();
        private WhenHotAnalysisBLL whenhotanalysisbll = new WhenHotAnalysisBLL();
        private BlindPlateWallSpecBLL blindplatewallspecbll = new BlindPlateWallSpecBLL();
        private JobApprovalFormBLL JobApprovalFormbll = new JobApprovalFormBLL();
        public HttpContext ctx { get { return HttpContext.Current; } }
        #region 获取安全作业许可证基础数据

        #region 获取分级标准和危害辨识

        [HttpPost]
        public object GetStandard([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");

                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string jobtype = res.Contains("jobtype") ? dy.data.jobtype : ""; //作业类型
                var data = classStandardConfigBLL.GetList("").Where(t => t.WorkType == jobtype && t.DeptCode == curUser.OrganizeCode).FirstOrDefault();
                if (data == null)
                {
                    return new { code = -1, info = "获取配置失败，请联系系统管理员" };
                }
                Dictionary<string, string> dict_props = new Dictionary<string, string>();
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                    DateFormatString = "yyyy-MM-dd HH:mm" //格式化日期
                };
                return new { code = 0, info = "获取数据成功",  data = JObject.Parse(JsonConvert.SerializeObject(data, Formatting.None, settings)) };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = "操作失败", data = ex.ToString() };
            }
        }
        #endregion

        #region 获取安全措施
        [HttpPost]
        public object GetSafetyMeasure([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");

                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string jobtype = res.Contains("jobtype") ? dy.data.jobtype : ""; //作业类型
                string type = res.Contains("type") ? dy.data.type : "";//获取类型
                var entity = safetyMeasureConfigBLL.GetList("").Where(t => t.WorkType == jobtype && t.ConfigType == type && t.DeptCode == curUser.OrganizeCode).FirstOrDefault();
                if (entity != null)
                {
                    var data = safetyMeasureDetailBLL.GetList("").Where(t => t.RecId == entity.Id).OrderBy(t => t.SortNum).ToList();
                    if (data.Count <= 0)
                    {
                        return new { code = -1, info = "获取配置失败，请联系系统管理员" };
                    }
                    Dictionary<string, string> dict_props = new Dictionary<string, string>();
                    JsonSerializerSettings settings = new JsonSerializerSettings
                    {
                        ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                        DateFormatString = "yyyy-MM-dd HH:mm" //格式化日期
                    };
                    return new { code = 0, info = "获取数据成功", data = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(data, Formatting.None, settings)) };
                }
                else
                {
                    return new { code = -1, info = "获取配置失败，请联系系统管理员" };
                }
                
            }
            catch (Exception ex)
            {
                return new { code = -1, info = "操作失败", data = ex.ToString() };
            }
        }
        #endregion

        #endregion
        
        #region 获取作业安全证列表
        /// <summary>
        /// 获取作业安全证列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");

                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                Pagination pagination = new Pagination();

                pagination.rows = int.Parse(dy.pagesize.ToString()); //每页的记录数

                pagination.page = int.Parse(dy.pageindex.ToString());  //当前页索引

                string keyword = res.Contains("keyword") ? dy.data.keyword : "";  //关键字查询
                string jobstate = res.Contains("jobstate") ? dy.data.jobstate : ""; //状态查询
                string jobtype = res.Contains("jobtype") ? dy.data.jobtype : ""; //作业类型
                string jobstarttime = res.Contains("jobstarttime") ? dy.data.jobstarttime : ""; //作业开始时间
                string jobendtime = res.Contains("jobendtime") ? dy.data.jobendtime : ""; //作业结束时间
                string showrange = res.Contains("showrange") ? dy.data.showrange : ""; //查询范围 0：本人申请 1：本人处理
                string code = res.Contains("deptcode") ? dy.data.deptcode : "";//作业单位
                pagination.conditionJson = "1=1";
                var queryJson = new
                {
                    keyword = keyword,
                    jobstate = jobstate,
                    jobtype = jobtype,
                    jobstarttime = jobstarttime,
                    jobendtime = jobendtime,
                    showrange = showrange,
                    code = code
                };
                var data = jobSafetyCardApplybll.GetPageList(pagination, queryJson.ToJson());
                int count = pagination.records;
                pagination.rows = 100000;
                pagination.page = 1;
                queryJson = new
                {
                    keyword = keyword,
                    jobstate = "10",
                    jobtype = jobtype,
                    jobstarttime = jobstarttime,
                    jobendtime = jobendtime,
                    showrange = showrange,
                    code = code
                };
                var temp = jobSafetyCardApplybll.GetPageList(pagination, queryJson.ToJson());
                int workcount = pagination.records;
                var result = new
                {
                    list = data,
                    workcount = workcount,
                    count = count
                };
                return new { code = 0, info = "获取数据成功", count = count, workcount = workcount, data = result };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = "获取失败", data = ex.ToString() };
            }
            

        }
        #endregion

        #region 保存/提交作业安全证信息
        [HttpPost]
        public object SaveForm()
        {
            string res = ctx.Request["json"];

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            try
            {
                string keyValue = dy.data.id;
                if (string.IsNullOrWhiteSpace(keyValue))
                {
                    keyValue = Guid.NewGuid().ToString();
                }
                JobSafetyCardApplyEntity entity = JsonConvert.DeserializeObject<JobSafetyCardApplyEntity>(JsonConvert.SerializeObject(dy.data));
                string webUrl = dataitemdetailbll.GetItemValue("imgUrl");
                entity.SignUrl = string.IsNullOrWhiteSpace(entity.SignUrl) ? "" : entity.SignUrl.Replace(webUrl, "");
                string arr = JsonConvert.SerializeObject(dy.data.arr);
                string arrData = res.Contains("spec") ? JsonConvert.SerializeObject(dy.data.spec) : "";
                //获取业务数据关联的逐级审核流程步骤信息
                Operator user = OperatorProvider.Provider.Current();
                switch (entity.JobType)
                {
                    case "HeightWorking":
                        entity.JobState = 1;
                        if (entity.JobLevel == "0")
                        {
                          entity.ModuleNo = "YJGCZYSP";
                        }
                        else if (entity.JobLevel == "1")
                        {
                            entity.ModuleNo = "EJGCZYSP";
                        }
                        else if (entity.JobLevel == "2")
                        {
                            entity.ModuleNo = "SJGCZYSP";
                        }
                        else if (entity.JobLevel == "3")
                        {
                            entity.ModuleNo = "TJGCZYSP";
                        }
                        break;
                    case "Lifting":
                        entity.JobState = 1;
                        entity.ModuleNo = "QZDZZYSP";
                        break;
                    case "Digging":
                        entity.JobState = 1;
                        entity.ModuleNo = "DTZYSP";
                        break;
                    case "OpenCircuit":
                        entity.JobState = 1;
                        entity.ModuleNo = "DLZYSP";
                        break;
                    case "WhenHot":
                        entity.JobState = 1;
                        if (entity.JobLevel == "0")
                        {
                            entity.ModuleNo = "TSDHZYSP";
                        }
                        else if (entity.JobLevel == "1")
                        {
                            entity.ModuleNo = "YJDHZYSP";
                        }
                        else if (entity.JobLevel == "2")
                        {
                            entity.ModuleNo = "EJDHZYSP";
                        }
                        else if (entity.JobLevel == "3")
                        {
                            entity.ModuleNo = "SJDHZYSP";
                        }
                        break;
                    case "BlindPlateWall":
                        entity.JobState = 1;
                        entity.ModuleNo = "MBCDZYSP";
                        break;
                    case "LimitedSpace":
                        entity.JobState = 3;
                        entity.ModuleNo = "SXKJZYSP";
                        break;
                    case "EquOverhaulClean":
                        entity.JobState = 3;
                        entity.ModuleNo = "SBJXQLZYSP";
                        break;
                    default:
                        break;
                }
                if (entity.IsSubmit == 0)
                {
                    entity.JobState = 0;
                }
                string deletefileIds = res.Contains("deletefileIds") ? dy.data.deletefileIds : "";
                if (!string.IsNullOrWhiteSpace(deletefileIds))
                {
                    DeleteFile(deletefileIds);
                }
                UploadifyFile(keyValue + "_01", "file", ctx.Request.Files);
                UploadifyFile(keyValue + "_02", "pic", ctx.Request.Files);
                var data = manyPowerCheckbll.GetListByModuleNo(user.OrganizeCode, entity.ModuleNo);
                entity.ApplyNumber = entity.ApplyNumber == 0 ? 1 : entity.ApplyNumber;
                jobSafetyCardApplybll.SaveForm(keyValue, entity, data, arr, arrData);
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "异常信息:" + ex.Message  };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }
        #endregion

        #region 删除作业安全证
        [HttpPost]
        public object RemoveForm()
        {
            string res = ctx.Request["json"];

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            try
            {
                string keyValue = dy.data.id;
                if (JobApprovalFormbll.GetList("").Where(t => t.JobSafetyCardId != null && t.JobSafetyCardId.Contains(keyValue)).Count() > 0)
                {
                    return new { code = -1, count = 0, info = "已经关联危险作业审批单，不可以被删除" };
                }
                jobSafetyCardApplybll.RemoveForm(keyValue);
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "异常信息:" + ex.Message };
            }

            return new { code = 0, count = 0, info = "删除成功" };

        }
        #endregion

        #region 撤销作业安全证
        [HttpPost]
        public object CancelForm()
        {
            string res = ctx.Request["json"];

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            try
            {
                string keyValue = dy.data.id;
                var entity = jobSafetyCardApplybll.GetEntity(keyValue);
                var flowdetail = dangerousjobflowdetailbll.GetList().Where(t => t.BusinessId == keyValue && t.Status == 0).FirstOrDefault();
                if (flowdetail != null)
                {
                    dangerousjobflowdetailbll.RemoveForm(flowdetail.Id);
                }
                entity.JobState = 0;
                entity.IsSubmit = 0;
                entity.ApplyNumber = entity.ApplyNumber + 1;
                jobSafetyCardApplybll.SaveForm(keyValue, entity);
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "异常信息:" + ex.Message };
            }

            return new { code = 0, count = 0, info = "撤销成功" };

        }
        #endregion

        #region 获取作业安全证信息
        [HttpPost]
        public object GetForm([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");

                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string webUrl = dataitemdetailbll.GetItemValue("imgUrl");
                string keyvalue = dy.data.id;
                var entity = jobSafetyCardApplybll.GetEntity(keyvalue);  //业务实体
                entity.SignUrl = string.IsNullOrWhiteSpace(entity.SignUrl) ? "" : webUrl + entity.SignUrl;
                entity.ConfirmSignUrl = string.IsNullOrWhiteSpace(entity.ConfirmSignUrl) ? "" : webUrl + entity.ConfirmSignUrl;
                entity.JobLevelName = string.IsNullOrWhiteSpace(entity.JobLevel) ? "" : jobSafetyCardApplybll.getName(entity.JobType, entity.JobLevel, "001");
                var flowlist = jobSafetyCardApplybll.GetAppFlowList(keyvalue);  //审核记录
                var Empty = dangerousjoboperatebll.GetList("").Where(t => t.RecId == keyvalue && t.OperateType == 2).OrderByDescending(t => t.CreateDate).FirstOrDefault(); //获取停电记录
                var Full = dangerousjoboperatebll.GetList("").Where(t => t.RecId == keyvalue && t.OperateType == 3).OrderByDescending(t => t.CreateDate).FirstOrDefault(); //获取送电记录
                var ElcTemp = new
                {
                    EmptyEqu = Empty == null ? "" : Empty.OperateOpinion,
                    EmptyTime = Empty == null ? "" : Convert.ToDateTime(Empty.OperateTime).ToString("yyyy-MM-dd HH:mm"),
                    EmptyPerson = Empty == null ? "" : Empty.OperatePerson,
                    EmptySignImg = Empty == null ? "" : webUrl + Empty.SignImg,
                    FullTime = Full == null ? "" : Convert.ToDateTime(Full.OperateTime).ToString("yyyy-MM-dd HH:mm"),
                    FullPerson = Full == null ? "" : Full.OperatePerson,
                    FullSignImg = Full == null ? "" : webUrl + Full.SignImg,
                };
                List<object> elc = new List<object>();
                elc.Add(ElcTemp); //停送电信息
                var recheck = dangerousjoboperatebll.GetList("").Where(t => t.RecId == keyvalue && t.OperateType == 0).OrderByDescending(t => t.CreateDate).ToList(); //验收记录
                var copy = dangerousjoboperatebll.GetList("").Where(t => t.RecId == keyvalue && t.OperateType == 1).OrderByDescending(t => t.CreateDate).ToList();  //备案记录
                /**************** 获取执行信息 *************************/
                IList<FireWaterCondition> conditionlist = firewaterbll.GetConditionList(keyvalue).OrderBy(t => t.CreateDate).ToList();
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
                        if (temp.FileExtensions.Contains("mp3"))
                        {
                            ERCHTMS.Entity.HighRiskWork.ViewModel.Photo pic = new ERCHTMS.Entity.HighRiskWork.ViewModel.Photo();
                            pic.filename = temp.FileName;
                            pic.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + temp.FilePath.Substring(1);
                            pic.fileid = temp.FileId;
                            tempfilelist.Add(pic);
                        }
                        
                    }
                    item.filelist = tempfilelist;
                    item.num = i / 2 + 1;
                }
                //作业分析数据
                var AnalysisData = whenhotanalysisbll.GetList("");
                AnalysisData = AnalysisData.Where(t => t.RecId == keyvalue);
                entity.AnalysisData = AnalysisData.ToList();
                //风险辨识、分级说明
                var whbs = classStandardConfigBLL.GetList("").Where(t => t.WorkType == entity.JobType && t.DeptCode == curUser.OrganizeCode).FirstOrDefault();
                //作业单位安全措施
                var workMeasureentity = safetyMeasureConfigBLL.GetList("").Where(t => t.WorkType == entity.JobType && t.ConfigType == "0" && t.DeptCode == curUser.OrganizeCode).FirstOrDefault();
                var workMeasure = safetyMeasureDetailBLL.GetList("").Where(t => t.RecId == entity.Id).OrderBy(t => t.SortNum).ToList();
                //确认安全措施
                var confirmMeasureentity = safetyMeasureConfigBLL.GetList("").Where(t => t.WorkType == entity.JobType && t.ConfigType == "1" && t.DeptCode == curUser.OrganizeCode).FirstOrDefault();
                var confirmMeasure = safetyMeasureDetailBLL.GetList("").Where(t => t.RecId == entity.Id).OrderBy(t => t.SortNum).ToList();
                //盲板抽堵信息
                var spec = blindplatewallspecbll.GetList("");
                if (!string.IsNullOrWhiteSpace(keyvalue))
                {
                    spec = spec.Where(t => t.RecId == keyvalue);
                }
                entity.spec = spec.ToList();
                //操作人
                var flow = dangerousjobflowdetailbll.GetList().Where(x => x.BusinessId == keyvalue && x.Status == 0).ToList().FirstOrDefault();
                if (flow != null)
                {
                    if (flow.ProcessorFlag == "3")
                    {
                        entity.approvename = flow.UserName;
                        entity.approveid = flow.UserId;
                        entity.approveaccount = flow.UserAccount;
                    }
                }
                //对应作业审批单数据
                var ApprovalForm = JobApprovalFormbll.GetList("").Where(t => !string.IsNullOrWhiteSpace(t.JobSafetyCardId) && t.JobSafetyCardId.Contains(keyvalue)).FirstOrDefault();
                if (ApprovalForm != null)
                {
                    entity.approveformid = ApprovalForm.Id;
                }
                else
                {
                    entity.approveformid = "";
                }
                //附件信息
                DataTable cdt = fileInfoBLL.GetFiles(keyvalue + "_01");
                IList<Photo> cfiles = new List<Photo>();
                foreach (DataRow item in cdt.Rows)
                {
                    Photo p = new Photo();
                    p.id = item[0].ToString();
                    p.filename = item[1].ToString();
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + item[2].ToString().Substring(1);
                    cfiles.Add(p);
                }
                cdt = fileInfoBLL.GetFiles(keyvalue + "_02");
                IList<Photo> cfiles2 = new List<Photo>();
                foreach (DataRow item in cdt.Rows)
                {
                    Photo p = new Photo();
                    p.id = item[0].ToString();
                    p.filename = item[1].ToString();
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + item[2].ToString().Substring(1);
                    cfiles2.Add(p);
                }

                //arr 流程信息
                string moduleno = "";
                switch (entity.JobType)
                {
                    case "HeightWorking":
                        if (entity.JobLevel == "0")
                        {
                            moduleno = "YJGCZYSP";
                        }
                        else if (entity.JobLevel == "1")
                        {
                            moduleno = "EJGCZYSP";
                        }
                        else if (entity.JobLevel == "2")
                        {
                            moduleno = "SJGCZYSP";
                        }
                        else if (entity.JobLevel == "3")
                        {
                            moduleno = "TJGCZYSP";
                        }
                        break;
                    case "Lifting":
                        moduleno = "QZDZZYSP";
                        break;
                    case "Digging":
                        moduleno = "DTZYSP";
                        break;
                    case "OpenCircuit":
                        moduleno = "DLZYSP";
                        break;
                    case "WhenHot":
                        if (entity.JobLevel == "0")
                        {
                            moduleno = "TSDHZYSP";
                        }
                        else if (entity.JobLevel == "1")
                        {
                            moduleno = "YJDHZYSP";
                        }
                        else if (entity.JobLevel == "2")
                        {
                            moduleno = "EJDHZYSP";
                        }
                        else if (entity.JobLevel == "3")
                        {
                            moduleno = "SJDHZYSP";
                        }
                        break;
                    case "BlindPlateWall":
                        moduleno = "MBCDZYSP";
                        break;
                    case "LimitedSpace":
                        moduleno = "SXKJZYSP";
                        break;
                    case "EquOverhaulClean":
                        moduleno = "SBJXQLZYSP";
                        break;
                    default:
                        break;
                }
                var flowtemp = jobSafetyCardApplybll.ConfigurationByWorkList(keyvalue, moduleno);
                ///流程信息
                List<checkperson> flowarr = new List<checkperson>();
                foreach (DataRow item in flowtemp.Rows)
                {
                    checkperson c = new checkperson();
                    c.id = item["id"].ToString();
                    c.userid = item["userid"].ToString();
                    c.username = item["username"].ToString();
                    c.account = item["account"].ToString();
                    c.flowname = item["flowname"].ToString();
                    c.workid = entity.Id;
                    c.choosepersontitle = item["ChoosePersonTitle"].ToString();
                    c.choosepersonwarn = item["ChoosePersonWarn"].ToString();
                    flowarr.Add(c);
                }
                entity.arr = flowarr;
                var data = new
                {
                    data = entity,
                    flowlist = flowlist,
                    elc = elc,
                    recheck = recheck,
                    copy = copy,
                    conditionlist = conditionlist,
                    whbs = whbs,
                    workmeasure = workMeasure,
                    confirmmeasure = confirmMeasure,
                    file = cfiles,
                    pic = cfiles2
                };
                
                Dictionary<string, string> dict_props = new Dictionary<string, string>();
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                    DateFormatString = "yyyy-MM-dd HH:mm" //格式化日期
                };
                return new { code = 0, info = "获取数据成功", data = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(data, Formatting.None, settings)) };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = "获取失败", data = ex.ToString() };
            }
        }
        #endregion

        #region 审批作业安全证
        [HttpPost]
        public object CheckSaveForm()
        {
            string res = ctx.Request["json"];

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            try
            {
                string keyValue = dy.data.businessid;
                DangerousJobFlowDetailEntity entity = JsonConvert.DeserializeObject<DangerousJobFlowDetailEntity>(JsonConvert.SerializeObject(dy.data));
                string webUrl = dataitemdetailbll.GetItemValue("imgUrl");
                entity.SignUrl = string.IsNullOrWhiteSpace(entity.SignUrl) ? "" : entity.SignUrl.Replace(webUrl, "");
                dangerousjobflowdetailbll.CheckSaveForm(keyValue, entity);
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "异常信息:" + ex.Message };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }
        #endregion

        #region 措施确认
        [HttpPost]
        public object ConfirmForm()
        {
            string res = ctx.Request["json"];

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            try
            {
                string keyValue = dy.data.id;
                var data = jobSafetyCardApplybll.GetEntity(keyValue);
                JobSafetyCardApplyEntity entity = JsonConvert.DeserializeObject<JobSafetyCardApplyEntity>(JsonConvert.SerializeObject(dy.data));
                string webUrl = dataitemdetailbll.GetItemValue("imgUrl");
                entity.ConfirmSignUrl = string.IsNullOrWhiteSpace(entity.ConfirmSignUrl) ? "" : entity.ConfirmSignUrl.Replace(webUrl, "");
                UserBLL userbll = new UserBLL();
                if (data.JobType == "LimitedSpace")
                {
                    entity.JobState = 1;
                }
                else if (data.JobType == "EquOverhaulClean")
                {
                    entity.JobState = 4;
                }
                entity.ConfirmSignUrl = string.IsNullOrWhiteSpace(entity.ConfirmSignUrl) ? "" : entity.ConfirmSignUrl.Replace("../..", "");
                jobSafetyCardApplybll.SaveForm(keyValue, entity);
                if (data.JobType == "LimitedSpace")
                {
                    DangerousJobFlowDetailEntity flow = dangerousjobflowdetailbll.GetList().Where(t => t.BusinessId == data.Id && t.Status == 0).FirstOrDefault();
                    string userids = dangerousjobflowdetailbll.GetCurrentStepUser(data.Id, flow.Id);
                    DataTable dt = userbll.GetUserTable(userids.Split(','));
                    JPushApi.PushMessage(string.Join(",", dt.AsEnumerable().Select(t => t.Field<string>("account")).ToArray()), string.Join(",", dt.AsEnumerable().Select(t => t.Field<string>("realname")).ToArray()), "ZYAQZ003", data.JobTypeName + "安全证申请待您审批，请您及时处理。", data.CreateUserName + "于" + data.CreateDate.Value.ToString("yyyy年MM月dd日") + "申请的" + data.JobTypeName + "安全证需要您进行审批，请您及时处理。", data.Id);
                }
                else if (data.JobType == "EquOverhaulClean")
                {
                    DataTable dt = userbll.GetUserTable(data.PowerCutPersonId.Split(','));
                    JPushApi.PushMessage(string.Join(",", dt.AsEnumerable().Select(t => t.Field<string>("account")).ToArray()), data.PowerCutPerson, "ZYAQZ008", data.JobTypeName + "安全证待您进行停电，请您及时处理。", data.CreateUserName + "于" + data.CreateDate.Value.ToString("yyyy年MM月dd日") + "申请的" + data.JobTypeName + "安全证需要您进行停电操作，请您及时处理。", data.Id);
                }
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "异常信息:" + ex.Message };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }
        #endregion

        #region 保存作业安全证 备案、验收、停电、送电 操作
        [HttpPost]
        public object SaveOperateForm()
        {
            string res = ctx.Request["json"];

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            try
            {
                string keyValue = dy.data.recid;
                DangerousJobOperateEntity entity = JsonConvert.DeserializeObject<DangerousJobOperateEntity>(JsonConvert.SerializeObject(dy.data));
                entity.RecId = keyValue;
                string webUrl = dataitemdetailbll.GetItemValue("imgUrl");
                entity.SignImg = string.IsNullOrWhiteSpace(entity.SignImg) ? "" : entity.SignImg.Replace(webUrl, "");
                entity.SignImg = string.IsNullOrWhiteSpace(entity.SignImg) ? "" : entity.SignImg.Replace("../..", "");
                dangerousjoboperatebll.SaveForm("", entity);
                var jobSafetyentity = jobSafetyCardApplybll.GetEntity(entity.RecId);
                UserBLL userbll = new UserBLL();
                switch (entity.OperateType)
                {
                    case 0:
                        jobSafetyentity.JobState = 11;
                        break;
                    case 1:
                        jobSafetyentity.JobState = 8;
                        break;
                    case 2:
                        jobSafetyentity.JobState = 1;
                        break;
                    case 3:
                        jobSafetyentity.JobState = 11;
                        break;
                    default:
                        break;
                }
                jobSafetyCardApplybll.SaveForm(jobSafetyentity.Id, jobSafetyentity);
                switch (entity.OperateType)
                {
                    case 1:
                        JPushApi.PushMessage(userbll.GetEntity(jobSafetyentity.CreateUserId).Account, jobSafetyentity.CreateUserName, "ZYAQZ006", jobSafetyentity.JobTypeName + "安全证申请已备案，请您知晓。", "您于" + jobSafetyentity.CreateDate.Value.ToString("yyyy年MM月dd日") + "申请的" + jobSafetyentity.JobTypeName + "安全证已备案，请您知晓。", jobSafetyentity.Id);
                        break;
                    case 2:
                        //给申请人发送已经停电消息
                        JPushApi.PushMessage(userbll.GetEntity(jobSafetyentity.CreateUserId).Account, jobSafetyentity.CreateUserName, "ZYAQZ006", jobSafetyentity.JobTypeName + "安全证申请已停电，请您知晓。", "您于" + jobSafetyentity.CreateDate.Value.ToString("yyyy年MM月dd日") + "申请的" + jobSafetyentity.JobTypeName + "安全证已停电，请您知晓。", jobSafetyentity.Id);

                        //给审核人发送审核消息
                        DangerousJobFlowDetailEntity flow = dangerousjobflowdetailbll.GetList().Where(t => t.BusinessId == jobSafetyentity.Id && t.Status == 0).FirstOrDefault();
                        string userids = dangerousjobflowdetailbll.GetCurrentStepUser(jobSafetyentity.Id, flow.Id);
                        DataTable dt = userbll.GetUserTable(userids.Split(','));
                        JPushApi.PushMessage(string.Join(",", dt.AsEnumerable().Select(t => t.Field<string>("account")).ToArray()), string.Join(",", dt.AsEnumerable().Select(t => t.Field<string>("realname")).ToArray()), "ZYAQZ003", jobSafetyentity.JobTypeName + "安全证申请待您审批，请您及时处理。", jobSafetyentity.CreateUserName + "于" + jobSafetyentity.CreateDate.Value.ToString("yyyy年MM月dd日") + "申请的" + jobSafetyentity.JobTypeName + "安全证需要您进行审批，请您及时处理。", jobSafetyentity.Id);
                        break;
                    case 3:
                        DataTable dt1 = userbll.GetUserTable((jobSafetyentity.CreateUserId + "," + jobSafetyentity.JobPersonId).Split(','));
                        JPushApi.PushMessage(string.Join(",", dt1.AsEnumerable().Select(t => t.Field<string>("account")).ToArray()), string.Join(",", dt1.AsEnumerable().Select(t => t.Field<string>("realname")).ToArray()), "ZYAQZ011", jobSafetyentity.JobTypeName + "已结束，现已送电，请您知晓。", "您于" + jobSafetyentity.CreateDate.Value.ToString("yyyy年MM月dd日") + "申请的" + jobSafetyentity.JobTypeName + "现已送电，请您知晓。", jobSafetyentity.Id);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "异常信息:" + ex.Message};
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }
        #endregion

        #region 保存动火分析、受限空间分析
        [HttpPost]
        public object SaveAnalysisForm()
        {
            string res = ctx.Request["json"];

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string id= Guid.NewGuid().ToString();
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            try
            {
                string keyValue = dy.data.recid;
                WhenHotAnalysisEntity entity = JsonConvert.DeserializeObject<WhenHotAnalysisEntity>(JsonConvert.SerializeObject(dy.data));
                entity.RecId = keyValue;
                entity.Id = string.IsNullOrWhiteSpace(entity.Id) ? id : entity.Id;
                whenhotanalysisbll.SaveForm(entity.Id, entity);
                return new { code = 0, count = 0, info = "保存成功", data = new { id = entity.Id } };
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "异常信息:" + ex.Message };
            }

            
        }
        #endregion

        #region 删除动火分析、受限空间分析
        [HttpPost]
        public object RemoveAnalysisForm()
        {
            string res = ctx.Request["json"];

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            try
            {
                string keyValue = dy.keyvalue;
                whenhotanalysisbll.RemoveForm(keyValue);
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "异常信息:" + ex.Message };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }
        #endregion

        #region 变更处理人
        [HttpPost]
        public object ExchangeForm()
        {
            string res = ctx.Request["json"];

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            try
            {
                string keyValue = dy.data.id;
                string TransferUserName = dy.data.transferusername;
                string TransferUserAccount = dy.data.transferuseraccount;
                string TransferUserId = dy.data.transferuserid;
                jobSafetyCardApplybll.ExchangeForm(keyValue, TransferUserName, TransferUserAccount, TransferUserId, curUser);
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "异常信息:" + ex.Message };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }
        #endregion

        #region 保存受限空间分析标准
        [HttpPost]
        public object SaveStandardForm()
        {
            string res = ctx.Request["json"];

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            try
            {
                string keyValue = dy.data.id;
                string OxygenContentStandard = dy.data.oxygencontentstandard;
                string DangerousStandard = dy.data.dangerousstandard;
                string GasStandard = dy.data.gasstandard;
                var oldentity = jobSafetyCardApplybll.GetEntity(keyValue);
                oldentity.OxygenContentStandard = OxygenContentStandard;
                oldentity.DangerousStandard = DangerousStandard;
                oldentity.GasStandard = GasStandard;
                jobSafetyCardApplybll.SaveForm(keyValue, oldentity);
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "异常信息:" + ex.Message };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }
        #endregion

        #region 上传附件、删除附件
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
                            }
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
            catch (Exception ex)
            {
                LogEntity logEntity = new LogEntity();
                logEntity.CategoryId = 4;
                logEntity.OperateTypeId = ((int)OperationType.Exception).ToString();
                logEntity.OperateType = EnumAttribute.GetDescription(OperationType.Exception);
                logEntity.OperateAccount = curUser.UserName;
                logEntity.OperateUserId = curUser.UserId;
                logEntity.ExecuteResult = -1;
                logEntity.ExecuteResultJson = ex.Message;
                logEntity.Module = SystemInfo.CurrentModuleName;
                logEntity.ModuleId = SystemInfo.CurrentModuleId;
                logEntity.WriteLog();
            }
        }
        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="fileInfoIds"></param>
        public bool DeleteFile(string fileInfoIds)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(fileInfoIds))
            {
                string ids = "";

                string[] strArray = fileInfoIds.Split(',');

                foreach (string s in strArray)
                {
                    ids += "'" + s + "',";
                    var entity = fileinfobll.GetEntity(s);
                    if (entity != null)
                    {
                        var filePath = ctx.Server.MapPath(entity.FilePath);
                        if (File.Exists(filePath))
                            File.Delete(filePath);
                    }
                }

                if (!string.IsNullOrEmpty(ids))
                {
                    ids = ids.Substring(0, ids.Length - 1);
                }
                int count = fileinfobll.DeleteFileForm(ids);

                result = count > 0 ? true : false;
            }

            return result;
        }
        public void DeleteFileByRec(string recId)
        {
            if (!string.IsNullOrWhiteSpace(recId))
            {
                var list = fileinfobll.GetFileList(recId);
                foreach (var file in list)
                {
                    fileinfobll.RemoveForm(file.FileId);
                    var filePath = ctx.Server.MapPath(file.FilePath);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
            }
        }
        #endregion

        #region 安全作业证统计

        /// <summary>
        ///作业类型统计(统计图)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Object GetDangerousJobCount([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string starttime = res.Contains("starttime") ? dy.data.starttime : "";
                string endtime = res.Contains("endtime") ? dy.data.endtime : "";
                string deptid = res.Contains("deptid") ? dy.data.deptid : "";
                string deptcode = res.Contains("deptcode") ? dy.data.deptcode : "";
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string data = jobSafetyCardApplybll.GetDangerousJobCount(starttime, endtime, deptid, deptcode);;
                return new { Code = 0, Count = -1, Info = "获取数据成功", data = JsonConvert.DeserializeObject(data) };

            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        #endregion

        #region 广西华昇竖屏-根据获取今日高风险作业
        [HttpPost]
        public object GetTodayWorkDataForGXHS(string orgcode)
        {
            try
            {
                //string res = json.Value<string>("json");
                //dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //string orgcode = dy.orgcode;
                List<DutyDeptWork> tempData = new List<DutyDeptWork>();
                var totalProNum = 0;
                var totalPersonNum = 0;
                var data = JobApprovalFormbll.GetList("").Where(t => t.RealityJobStartTime != null && t.RealityJobEndTime == null).Select(x => x.JobSafetyCardId).ToList();
                string str = string.Empty;
                List<string> ilist = new List<string>();
                foreach (var item in data)
                {
                    if (item != null)
                    {
                        var lists = item.Split(',');
                        foreach (var item1 in lists)
                        {
                            ilist.Add(item1);
                        }
                    }
                }
                str = string.Join("','", ilist);
                string where = "";
                if (!string.IsNullOrWhiteSpace(str))
                {
                    where += string.Format(" and a.id not in('{0}')", str);
                }
                string sql = string.Format(@"select * from( select to_char(a.id) as id,to_char(a.JobDeptName) as workdeptname, to_char(a.jobtypename) as worktypename,to_char(a.jobplace) as workplace, to_char(a.jobcontent) as workcontent,'4' as RiskTypeValue, '' as risktypename,to_char(a.JobPerson) as WorkUserNames,
                                       to_char(a.CUSTODIAN) as worktutelageusername,
                                       b.APPROVEPERSON as auditusername
                                  from BIS_JOBSAFETYCARDAPPLY a
                                  left join (select id,businessid,APPROVEPERSON,CHECKRESULT,APPROVETIME,row_number() over(partition by businessid order by APPROVETIME desc) as num
                                               from BIS_DangerousJobFlowDetail) b
                                    on a.id = b.businessid
                                   and b.num = 1
                                 where a.jobstate = 10 and a.createuserorgcode='{0}' {1}
                                union all
                                select  to_char(a.id) as id, to_char(a.JobDeptName) as workdeptname,to_char(a.JOBTYPENAME) as worktypename,to_char(a.jobplace) as workplace,to_char(a.jobcontent) as workcontent,to_char(a.joblevel) as RiskTypeValue,
                                d.itemname as risktypename,to_char(a.JobPerson) as WorkUserNames ,to_char(a.CUSTODIAN) as worktutelageusername,b.APPROVEPERSON as auditusername from BIS_JOBAPPROVALFORM a
                                    left join (select id, businessid,APPROVEPERSON,CHECKRESULT,APPROVETIME,row_number() over(partition by businessid order by APPROVETIME desc) as num
                                   from BIS_DangerousJobFlowDetail) b
                            on a.id = b.businessid
                           and b.num = 1
                           left join (select c.itemname,c.itemvalue from base_dataitemdetail c where c.itemid in (select itemid from base_dataitem where itemcode='DangerousJobCheck')) d on a.joblevel=d.itemvalue
                        where a.realityjobstarttime is not null and a.realityjobendtime is null and a.createuserorgcode='{0}') order by RiskTypeValue", orgcode, where);

                DutyDeptWork itemData = new DutyDeptWork();
                List<TodayWorkEntity> ProList = new List<TodayWorkEntity>();
                DataTable dt = jobSafetyCardApplybll.FindTable(sql);
                itemData.WorkNum = dt.Rows.Count;
                foreach (DataRow item in dt.Rows)
                {
                    TodayWorkEntity pro = new TodayWorkEntity();
                    pro.WorkDept = item["workdeptname"].ToString();
                    pro.WorkType = item["worktypename"].ToString();
                    pro.RiskType = item["risktypename"].ToString();
                    pro.RiskTypeValue = item["RiskTypeValue"].ToString();
                    pro.WorkTutelagePerson = item["worktutelageusername"].ToString();
                    pro.AuditUserName = item["auditusername"].ToString();
                    pro.WorkContent = item["workcontent"].ToString();
                    pro.WorkPlace = item["workplace"].ToString();
                    pro.id = item["id"].ToString();
                    ProList.Add(pro);
                    itemData.WorkPersonNum += string.IsNullOrEmpty(item["WorkUserNames"].ToString()) ? 0 : item["WorkUserNames"].ToString().Split(',').Length;
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

        #region 移动端安全指标获取今日危险作业数量
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

                
                var list = jobSafetyCardApplybll.GetTodayWorkList(pagination, JsonConvert.SerializeObject(dy.data));
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
    }
}
