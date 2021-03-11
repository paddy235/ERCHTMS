using BSFramework.Util.WebControl;
using ERCHTMS.AppSerivce.Model;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.DangerousJob;
using ERCHTMS.Busines.DangerousJobConfig;
using ERCHTMS.Busines.HighRiskWork;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.DangerousJob;
using ERCHTMS.Entity.PublicInfoManage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ERCHTMS.AppSerivce.Controllers
{
    /// <summary>
    /// 危险作业审批
    /// </summary>
    public class JobApprovlFormController : BaseApiController
    {
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private JobApprovalFormBLL ApprovalFormBll = new JobApprovalFormBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
        private ClassStandardConfigBLL classstandardconfigbll = new ClassStandardConfigBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();
        private ManyPowerCheckBLL manyPowerCheckbll = new ManyPowerCheckBLL();
        public HttpContext hcontent { get { return HttpContext.Current; } }
        private DangerousJobFlowDetailBLL dangerousJobFlowDetailbll = new DangerousJobFlowDetailBLL();
        public HttpContext ctx { get { return HttpContext.Current; } }
        private JobSafetyCardApplyBLL jobSafetyCardApplybll = new JobSafetyCardApplyBLL();
        #region 危险作业审批

        /// <summary>
        /// 获取作业类型
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getJobTypeList([FromBody]JObject json)
        {
            try
            {
                var itemlist = dataitemdetailbll.GetDataItemListByItemCode("DangerousJobType");
                return new { code = 0, info = "获取数据成功", count = itemlist.Count(), data = itemlist.Select(x => new { value = x.ItemValue, ename = x.ItemName }) };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 危险作业级别
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object getJobLevelList([FromBody]JObject json)
        {
            try
            {
                var itemlist = dataitemdetailbll.GetDataItemListByItemCode("DangerousJobCheck");
                return new { code = 0, info = "获取数据成功", count = itemlist.Count(), data = itemlist.Select(x => new { value = x.ItemValue, ename = x.ItemName }) };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 作业许可状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object getJobStateList([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string type = dy.type;
            string str = string.Empty;
            try
            {
                //危险作业审批台账
                if (type == "1")
                {
                    str = @"[
                                { ItemValue: '2', ItemName: '即将作业'},
                                { ItemValue: '5', ItemName: '作业暂停'},
                                { ItemValue: '6', ItemName: '作业中'},
                                { ItemValue: '7', ItemName: '流程结束'}
                               
                            ]";
                }
                //危险作业审批列表
                else
                {
                    str = @"[
                                { ItemValue: '0', ItemName: '申请中' },
                                { ItemValue: '1', ItemName: '审批中'},
                                { ItemValue: '2', ItemName: '审批通过'},
                                { ItemValue: '4', ItemName: '审批不通过'}
                               
                            ]";
                }
                var data = JsonConvert.DeserializeObject<List<itemClass>>(str);
                return new { code = 0, count = data.Count, info = "获取数据成功", data = data }; ;
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 获取安全措施列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getSafetyMeasuresList([FromBody]JObject json)
        {
            try
            {
                var itemlist = classstandardconfigbll.GetList("").Where(t => t.WorkType == "DangerousJobCheck").FirstOrDefault();

                var data = itemlist.Whbs.Split('$');
                return new { code = 0, info = "获取数据成功", count = data.Count(), data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 获取安全许可证
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getJobSafetyCardList([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string userId = dy.userid;
            OperatorProvider.AppUserId = userId;  //设置当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            long pageIndex = 1;
            long pageSize = 10000;
            Pagination pagination = new Pagination();
            pagination.conditionJson = " 1=1 ";
            pagination.page = int.Parse(pageIndex.ToString());
            pagination.rows = int.Parse(pageSize.ToString());
            pagination.sidx = "t.createdate";     //排序字段
            pagination.sord = "desc";             //排序方式
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var list = ApprovalFormBll.GetJobSafetyCardApplyPageList(pagination, JsonConvert.SerializeObject(dy.data));
            Dictionary<string, string> dict_props = new Dictionary<string, string>();
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                DateFormatString = "yyyy-MM-dd HH:mm", //格式化日期
            };
            return new { code = 0, info = "获取数据成功", count = pagination.records, data = JArray.Parse(JsonConvert.SerializeObject(list, Formatting.None, settings)) };

        }

        /// <summary>
        /// 获取风险审批列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object getJobApprovlFormList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                //if (null == curUser)
                //{
                //    return new { code = -1, info = "请求失败,请登录!" };
                //}
                long pageIndex = dy.pageindex;
                long pageSize = dy.pagesize;
                Pagination pagination = new Pagination();
                pagination.page = int.Parse(pageIndex.ToString());
                pagination.rows = int.Parse(pageSize.ToString());
                pagination.sidx = "t.createdate";     //排序字段
                pagination.sord = "desc";             //排序方式
                pagination.conditionJson = " 1=1 ";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var list = ApprovalFormBll.GetAppPageList(pagination, JsonConvert.SerializeObject(dy.data));
                int count = pagination.records;
                pagination.rows = 1000000;
                var historyCount = ApprovalFormBll.GetAppPageList(pagination, JsonConvert.SerializeObject(dy.data)).Rows.Count;
                var query = new
                {
                    code = dy.data.code,
                    jobstate = "6",
                    //jobtype = dy.data.jobtype,
                    jobstarttime = dy.data.jobstarttime,
                    jobendtime = dy.data.jobendtime,
                    //keyword = dy.data.keyword,
                    joblevel = dy.data.joblevel
                };
                var workingCount = ApprovalFormBll.GetAppPageList(pagination, JsonConvert.SerializeObject(query)).Rows.Count;
                Dictionary<string, string> dict_props = new Dictionary<string, string>();
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                    DateFormatString = "yyyy-MM-dd HH:mm", //格式化日期
                };
                var data = new
                {
                    list = list,
                    count = historyCount,
                    workingCount = workingCount
                };
                //settings.Converters.Add(new DecimalToStringConverter());
                //return new { code = 0, info = "获取数据成功", count = count, data = JArray.Parse(JsonConvert.SerializeObject(list, Formatting.None, settings)) };
                return new { code = 0, info = "获取数据成功", count = count, data = JObject.Parse(JsonConvert.SerializeObject(data, Formatting.None, settings)) };


            }
            catch (Exception ex)
            {

                throw;
            }
        }
        /// <summary>
        /// 获取风险审批详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getJobApprovlFormDetail([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                Operator user = OperatorProvider.Provider.Current();
                JobApprovalFormEntity entity = ApprovalFormBll.GetEntity(dy.data.Id);
                IList<dynamic> cfiles = new List<dynamic>(); //方案文件
                if (entity != null)
                {
                    DataTable cdt = fileInfoBLL.GetFiles(entity.Id);
                    foreach (DataRow item in cdt.Rows)
                    {
                        Photo p = new Photo();
                        p.id = item[0].ToString();
                        p.filename = item[1].ToString();
                        p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + item[2].ToString().Substring(1);
                        cfiles.Add(p);
                    }
                    entity.File = cfiles;
                    //流程记录
                    entity.CheckInfo = new List<CheckInfoEntity>();

                    var dt = ApprovalFormBll.GetCheckInfo(entity.Id);
                    foreach (DataRow item in dt.Rows)
                    {
                        CheckInfoEntity c = new CheckInfoEntity();
                        c.ApproveDeptName = item["approvedeptname"].ToString();
                        c.ApprovePerson = item["ApprovePerson"].ToString();
                        c.Status = item["Status"].ToString();
                        c.ApproveOpinion = item["ApproveOpinion"].ToString();
                        c.ApproveTime = item["ApproveTime"].ToString();
                        c.SignUrl = string.IsNullOrEmpty(item["SignUrl"].ToString()) ? "" : dataitemdetailbll.GetItemValue("imgUrl") + item["SignUrl"].ToString();
                        entity.CheckInfo.Add(c);
                    }
                    if (entity.JobLevel == "0")
                        entity.ModuleName = "YJFXZYSP";
                    if (entity.JobLevel == "1")
                        entity.ModuleName = "EJFXZYSP";
                    if (entity.JobLevel == "2")
                        entity.ModuleName = "SJFXZYSP";
                    var dtt = ApprovalFormBll.ConfigurationByWorkList(entity.Id, entity.ModuleName);
                    ///流程信息
                    entity.Items = new List<checkperson>();
                    foreach (DataRow item in dtt.Rows)
                    {
                        checkperson c = new checkperson();
                        c.id = item["id"].ToString();
                        c.userid = item["userid"].ToString();
                        c.username = item["username"].ToString();
                        c.account = item["account"].ToString();
                        c.flowname = item["flowname"].ToString();
                        c.choosepersontitle = item["ChoosePersonTitle"].ToString();
                        c.choosepersonwarn = item["ChoosePersonWarn"].ToString();
                        c.workid = entity.Id;
                        entity.Items.Add(c);
                    }
                    //执行信息
                    entity.conditionitems = new List<Entity.HighRiskWork.FireWaterCondition>();
                    entity.conditionitems = new FireWaterBLL().GetConditionList(entity.Id).OrderBy(t => t.CreateDate).ToList();
                    //for (int i = 0; i < entity.conditionitems.Count; i++)
                    //{
                    //    List<FileInfoEntity> filelist = fileinfobll.GetFileList(entity.conditionitems[i].Id); //现场图片
                    //    if (filelist.Count > 0)
                    //    {
                    //        entity.conditionitems[i].ScenePicPath = filelist[0].FilePath;
                    //    }
                    //    List<FileInfoEntity> filelist2 = fileinfobll.GetFileList(entity.conditionitems[i].Id + "_02"); //附件
                    //    entity.conditionitems[i].filenum = filelist2.Count().ToString();
                    //    entity.conditionitems[i].num = i / 2 + 1;
                    //}
                    for (int i = 0; i < entity.conditionitems.Count; i++)
                    {
                        var item = entity.conditionitems[i];
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
                    entity.checkflow = new List<Entity.HighRiskWork.ViewModel.CheckFlowData>();
                    entity.checkflow = ApprovalFormBll.GetAppFlowList(entity.Id);  //审核记录
                    entity.SignUrl = string.IsNullOrEmpty(entity.SignUrl) ? "" : dataitemdetailbll.GetItemValue("imgUrl") + entity.SignUrl;
                    entity.JobLevelName = dataitemdetailbll.GetItemName("DangerousJobCheck", entity.JobLevel);
                    if (!string.IsNullOrEmpty(entity.JobSafetyCardId))
                    {
                        var JobSafetyCardId = entity.JobSafetyCardId.Split(',');
                        var d = ApprovalFormBll.GetSafetyCardTable(JobSafetyCardId);
                        entity.JobSafetyCard = "";
                        entity.JobSafetyCardId = "";
                        var status = "";
                        for (int i = 0; i < d.Rows.Count; i++)
                        {
                            switch (d.Rows[i]["jobstate"].ToString())
                            {
                                case "0":
                                    status = "申请中";
                                    break;
                                case "1":
                                    status = "审批中";
                                    break;
                                case "2":
                                    status = "审核不通过";
                                    break;
                                case "3":
                                    status = "措施确认中";
                                    break;
                                case "4":
                                    status = "停电中";
                                    break;
                                case "5":
                                    status = "备案中";
                                    break;
                                case "6":
                                    status = "验收中";
                                    break;
                                case "7":
                                    status = "送电中";
                                    break;
                                case "8":
                                    status = "即将作业";
                                    break;
                                case "9":
                                    status = "作业暂停";
                                    break;
                                case "10":
                                    status = "作业中";
                                    break;
                                case "11":
                                    status = "流程结束";
                                    break;
                                default:
                                    break;
                            }
                            entity.JobSafetyCard += d.Rows[i]["jobtypename"] + "(" + status + "),";
                            entity.JobSafetyCardId += d.Rows[i]["Id"].ToString() + ",";
                        }
                        entity.JobSafetyCardId = entity.JobSafetyCardId.TrimEnd(',');
                        entity.JobSafetyCard = entity.JobSafetyCard.TrimEnd(',');
                    }
                    entity.JobState = entity.JobState;
                    if (entity.JobState == 2 && entity.WorkOperate == "1")
                        entity.JobState = 5;
                    else if (entity.JobState == 2 && entity.RealityJobStartTime != null && entity.RealityJobEndTime == null)
                        entity.JobState = 6;
                    else if (entity.JobState == 2 && entity.RealityJobEndTime != null)
                        entity.JobState = 7;
                    var flow = dangerousJobFlowDetailbll.GetList().Where(x => x.BusinessId == entity.Id && x.Status == 0).ToList().FirstOrDefault();
                    if (flow != null)
                    {
                        if (flow.ProcessorFlag == "3")
                        {
                            entity.OperatorName = flow.UserName;
                            entity.OperatorId = flow.UserId;
                            entity.OperatorAccount = flow.UserAccount;
                        }
                    }

                }
                return new
                {
                    Code = 0,
                    Count = 0,
                    Info = "获取数据成功",
                    data = entity
                };

            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 保存/提交
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SaveJobApprovlForm()
        {
            try
            {
                string webUrl = dataitemdetailbll.GetItemValue("imgUrl");
                string res = ctx.Request["json"];

                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid; //当前用户ID 
                OperatorProvider.AppUserId = userId;  //设置当前用户
                string keyValue = !string.IsNullOrEmpty(dy.data.Id) ? dy.data.Id : Guid.NewGuid().ToString();
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, info = "请求失败,请登录!" };
                }
                if (dy.data == null)
                {
                    throw new ArgumentException("缺少参数：data为空");
                }
                string applyentity = JsonConvert.SerializeObject(dy.data);
                JobApprovalFormEntity model = JsonConvert.DeserializeObject<JobApprovalFormEntity>(applyentity);
                var str = 0;
                var msg = "";
                ///提交判断安全证有没有被其他用户关联。
                //var centity = JsonConvert.SerializeObject(dy.data.Items);
                if (model.IsSubmit == 1)
                {
                    var list = ApprovalFormBll.GetList("").Where(x => x.IsSubmit == 1).Where(x => x.Id != model.Id).ToList();
                    var JobSafetyCardId = string.Join(",", list.Select(x => x.JobSafetyCardId).ToArray()).Replace(",", "','");
                    if (!string.IsNullOrEmpty(JobSafetyCardId))
                    {
                        var cardId = model.JobSafetyCardId.TrimEnd(',').Split(',');
                        foreach (var item in cardId)
                        {
                            if (JobSafetyCardId.Contains(item))
                            {
                                str = 1;
                                var Applyentity = jobSafetyCardApplybll.GetEntity(item);
                                if (Applyentity != null)
                                    msg += Applyentity.JobTypeName + "(" + Applyentity.ApplyNo + "),";
                            }
                        }
                        if (str == 1)
                        {
                            return new { code = -1, info = msg.TrimEnd(',') + "作业安全证已被关联，请重新选择。", count = 0 };
                        }
                        else
                        {
                            model.ApplyTime = DateTime.Now;
                            model.Id = keyValue;
                            var items = JsonConvert.SerializeObject(dy.data.Items);
                            HttpFileCollection files = HttpContext.Current.Request.Files;
                            //如果有删除的文件，则进行删除
                            if (!string.IsNullOrEmpty(model.DeleteFileIds))
                            {
                                DeleteFile(model.DeleteFileIds);
                            }
                            string path = string.Empty;
                            //再重新上传
                            if (files.Count > 0)
                            {
                                UploadifyFile(keyValue, files, ref path);
                            }
                            if (model.JobLevel == "0")
                                model.ModuleName = "YJFXZYSP";
                            if (model.JobLevel == "1")
                                model.ModuleName = "EJFXZYSP";
                            if (model.JobLevel == "2")
                                model.ModuleName = "SJFXZYSP";

                            model.ApplyDeptCode = curUser.DeptCode;
                            model.ApplyDeptId = curUser.DeptId;
                            model.ApplyDeptName = curUser.DeptName;
                            model.ApplyUserId = curUser.UserId;
                            model.ApplyUserName = curUser.UserName;
                            model.JobState = model.IsSubmit;
                            JobApprovalFormEntity entity = ApprovalFormBll.GetEntity(model.Id);
                            if (entity != null)
                            {
                                model.CreateUserDeptCode = entity.CreateUserDeptCode;
                            }
                            var data = manyPowerCheckbll.GetListByModuleNo(curUser.OrganizeCode, model.ModuleName);
                            model.SignUrl = string.IsNullOrWhiteSpace(model.SignUrl) ? "" : model.SignUrl.Replace(webUrl, "").ToString();
                            ApprovalFormBll.SaveForm(keyValue, model, data, items);
                            return new { Code = 0, Count = 0, Info = "保存成功" };
                        }
                    }
                    else
                    {
                        model.ApplyTime = DateTime.Now;
                        model.Id = keyValue;
                        var items = JsonConvert.SerializeObject(dy.data.Items);
                        HttpFileCollection files = HttpContext.Current.Request.Files;
                        //如果有删除的文件，则进行删除
                        if (!string.IsNullOrEmpty(model.DeleteFileIds))
                        {
                            DeleteFile(model.DeleteFileIds);
                        }
                        string path = string.Empty;
                        //再重新上传
                        if (files.Count > 0)
                        {
                            UploadifyFile(keyValue, files, ref path);
                        }
                        if (model.JobLevel == "0")
                            model.ModuleName = "YJFXZYSP";
                        if (model.JobLevel == "1")
                            model.ModuleName = "EJFXZYSP";
                        if (model.JobLevel == "2")
                            model.ModuleName = "SJFXZYSP";

                        model.ApplyDeptCode = curUser.DeptCode;
                        model.ApplyDeptId = curUser.DeptId;
                        model.ApplyDeptName = curUser.DeptName;
                        model.ApplyUserId = curUser.UserId;
                        model.ApplyUserName = curUser.UserName;
                        model.JobState = model.IsSubmit;
                        JobApprovalFormEntity entity = ApprovalFormBll.GetEntity(model.Id);
                        if (entity != null)
                        {
                            model.CreateUserDeptCode = entity.CreateUserDeptCode;
                        }
                        var data = manyPowerCheckbll.GetListByModuleNo(curUser.OrganizeCode, model.ModuleName);
                        model.SignUrl = string.IsNullOrWhiteSpace(model.SignUrl) ? "" : model.SignUrl.Replace(webUrl, "").ToString();
                        ApprovalFormBll.SaveForm(keyValue, model, data, items);
                        return new { Code = 0, Count = 0, Info = "保存成功" };
                    }
                }
                else
                {
                    model.ApplyTime = DateTime.Now;
                    model.Id = keyValue;
                    var items = JsonConvert.SerializeObject(dy.data.Items);
                    HttpFileCollection files = HttpContext.Current.Request.Files;
                    //如果有删除的文件，则进行删除
                    if (!string.IsNullOrEmpty(model.DeleteFileIds))
                    {
                        DeleteFile(model.DeleteFileIds);
                    }
                    string path = string.Empty;
                    //再重新上传
                    if (files.Count > 0)
                    {
                        UploadifyFile(keyValue, files, ref path);
                    }
                    if (model.JobLevel == "0")
                        model.ModuleName = "YJFXZYSP";
                    if (model.JobLevel == "1")
                        model.ModuleName = "EJFXZYSP";
                    if (model.JobLevel == "2")
                        model.ModuleName = "SJFXZYSP";

                    model.ApplyDeptCode = curUser.DeptCode;
                    model.ApplyDeptId = curUser.DeptId;
                    model.ApplyDeptName = curUser.DeptName;
                    model.ApplyUserId = curUser.UserId;
                    model.ApplyUserName = curUser.UserName;
                    model.JobState = model.IsSubmit;
                    JobApprovalFormEntity entity = ApprovalFormBll.GetEntity(model.Id);
                    if (entity != null)
                    {
                        model.CreateUserDeptCode = entity.CreateUserDeptCode;
                    }
                    model.SignUrl = string.IsNullOrWhiteSpace(model.SignUrl) ? "" : model.SignUrl.Replace(webUrl, "").ToString();

                    var data = manyPowerCheckbll.GetListByModuleNo(curUser.OrganizeCode, model.ModuleName);
                    ApprovalFormBll.SaveForm(keyValue, model, data, items);
                    return new { Code = 0, Count = 0, Info = "保存成功" };
                }

            }
            catch (Exception ex)
            {

                return new { code = -1, info = ex.Message, count = 0 };
            }
        }
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object CheckJobApprovlForm()
        {
            string res = ctx.Request["json"];

            //var dy = JsonConvert.DeserializeAnonymousType(res, new
            //{
            //    userid = string.Empty,
            //    data = new DangerousJobFlowDetailEntity()
            //});

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            try
            {

                string applyentity = JsonConvert.SerializeObject(dy.data);

                DangerousJobFlowDetailEntity model = JsonConvert.DeserializeObject<DangerousJobFlowDetailEntity>(applyentity);
                //model.SignUrl = dy.data.c;
                string webUrl = dataitemdetailbll.GetItemValue("imgUrl");
                model.SignUrl = res.Contains("ApproveSignUrl") ? string.IsNullOrWhiteSpace(dy.data.ApproveSignUrl) ? "" : dy.data.ApproveSignUrl.Replace(webUrl, "").ToString() : "";
                dangerousJobFlowDetailbll.ApprovalFormCheckSaveForm(dy.data.Id, model);
                return new { Code = 0, Count = 0, Info = "保存成功" };
            }
            catch (Exception ex)
            {
                return new { Code = 1, Count = 0, Info = "保存失败" + ex.Message };
            }
        }
        /// <summary>
        /// 报废
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SaveCancelReason([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            Operator user = OperatorProvider.Provider.Current();
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            var entity = ApprovalFormBll.GetEntity(dy.data.Id);
            if (entity != null)
            {
                entity.CancelReason = entity.CancelReason;
                entity.CancelTime = DateTime.Now;
                entity.CancelUserId = user != null ? user.UserId : "";
                entity.CancelUserName = user != null ? user.UserName : "";
                entity.JobState = 3;//更改状态(作废)
                ApprovalFormBll.SaveForm(dy.data.Id, entity);
                return new { Code = 0, Count = 0, Info = "保存成功" };
            }
            else
            {
                return new { Code = 1, Count = 0, Info = "保存失败" };
            }
        }


        /// <summary>
        /// 撤销/删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object RemoveForm()
        {
            try
            {
                string res = ctx.Request["json"];

                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                var msg = "";
                if (dy.method == "del")
                {
                    JobApprovalFormEntity entity = ApprovalFormBll.GetEntity(dy.data.Id);
                    if (entity != null)
                    {

                        ApprovalFormBll.RemoveForm(entity.Id);
                        msg = "删除成功";//return new { Code = 0, Count = 0, Info = "删除成功" };
                    }
                    else
                    {
                        msg = "删除失败，数据不存在！";
                    }

                }
                if (dy.method == "remove")
                {
                    JobApprovalFormEntity entity = ApprovalFormBll.GetEntity(dy.data.Id);
                    if (entity != null)
                    {
                        entity.JobState = 0;
                        entity.IsSubmit = 0;

                        ApprovalFormBll.SaveForm(entity.Id, entity);
                        var DetailService = dangerousJobFlowDetailbll.GetList().Where(x => x.BusinessId == entity.Id).ToList();
                        if (DetailService != null && DetailService.Count > 0)
                        {
                            foreach (var item in DetailService)
                            {
                                dangerousJobFlowDetailbll.RemoveForm(item.Id);
                            }
                        }
                        msg = "撤销成功";
                    }
                }
                return new { Code = 0, Count = 0, Info = msg };
            }
            catch (Exception ex)
            {

                return new { Code = 1, Count = 0, Info = "操作失败" + ex.Message };
            }

        }
        /// <summary>
        /// 操作人变更
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object ExchangeForm()
        {
            string res = ctx.Request["json"];
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            try
            {
                var entity = ApprovalFormBll.GetEntity(dy.data.Id);
                if (entity != null)
                {
                    var TransferUserName = dy.data.TransferUserName;
                    var TransferUserAccount = dy.data.TransferUserAccount;
                    var TransferUserId = dy.data.TransferUserId;
                    ApprovalFormBll.ExchangeForm(dy.data.Id, TransferUserName, TransferUserAccount, TransferUserId);
                    return new { Code = 0, Count = 0, Info = "保存成功" };
                }
                else
                {
                    return new { Code = 1, Count = 0, Info = "变更失败,记录不存在!" };
                }
            }
            catch (Exception ex)
            {
                return new { Code = 1, Count = 0, Info = "变更失败" + ex.Message };
            }
        }
        /// <summary>
        /// 开始作业
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object LedgerOp()
        {
            string res = ctx.Request["json"];
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            var keyValue = dy.data.Id;

            string ledgerType = dy.data.ledgerType.ToString();
            var worktime = dy.data.WorkTime.ToString(); //作业时间
            var issendmessage = res.Contains("IssendMessage") ? dy.data.IssendMessage.ToString() : "";//发送告知信息  1 是 0 否
            var conditioncontent = dy.data.ConditionContent;//执行情况说明
            var conditionid = "";//
            var type = res.Contains("JobType") ? dy.data.JobType : ""; //作业安全证作业类型
            if (string.IsNullOrEmpty(type))
            {
                var msg = ApprovalFormBll.IsLedgerSetting(keyValue);
                if (msg == "0")
                {
                    return new { Code = 1, Count = 0, Info = "有未审批通过的安全证,需要审批通过后才可开始作业！" };
                }
                else
                {

                    if (string.IsNullOrEmpty(conditionid))
                        conditionid = Guid.NewGuid().ToString();
                    var iscomplete = dy.data.Iscomplete.ToString();//本项作业已全部完成 1 是  0 否
                    try
                    {
                        HttpFileCollection files = HttpContext.Current.Request.Files;
                        UploadifyFile(conditionid, "", files);
                        ApprovalFormBll.LedgerOp(keyValue, ledgerType, type, worktime, issendmessage, conditioncontent, conditionid, iscomplete);
                        return new { Code = 0, Count = 0, Info = "成功" };
                    }
                    catch (Exception ex)
                    {
                        return new { Code = 1, Count = 0, Info = "作业失败" + ex.Message };
                    }
                }
            }
            else
            {

                if (string.IsNullOrEmpty(conditionid))
                    conditionid = Guid.NewGuid().ToString();
                var iscomplete = dy.data.Iscomplete.ToString();//本项作业已全部完成 1 是  0 否
                try
                {
                    HttpFileCollection files = HttpContext.Current.Request.Files;
                    UploadifyFile(conditionid, "", files);
                    ApprovalFormBll.LedgerOp(keyValue, ledgerType, type, worktime, issendmessage, conditioncontent, conditionid, iscomplete);
                    return new { Code = 0, Count = 0, Info = "成功" };
                }
                catch (Exception ex)
                {
                    return new { Code = 1, Count = 0, Info = "作业失败" + ex.Message };
                }
            }

        }
        /// <summary>
        /// 获取工作流
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetFlow([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");

                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid; //当前用户ID 
                OperatorProvider.AppUserId = userId;  //设置当前用户
                var moduleno = string.Empty;
                string JobType = res.Contains("JobType") ? dy.data.JobType : "";
                if (!string.IsNullOrWhiteSpace(JobType))
                {
                    switch (JobType)
                    {
                        case "HeightWorking":
                            if (dy.data.JobLevel == "0")
                            {
                                moduleno = "YJGCZYSP";
                            }
                            else if (dy.data.JobLevel == "1")
                            {
                                moduleno = "EJGCZYSP";
                            }
                            else if (dy.data.JobLevel == "2")
                            {
                                moduleno = "SJGCZYSP";
                            }
                            else if (dy.data.JobLevel == "3")
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
                            if (dy.data.JobLevel == "0")
                            {
                                moduleno = "TSDHZYSP";
                            }
                            else if (dy.data.JobLevel == "1")
                            {
                                moduleno = "YJDHZYSP";
                            }
                            else if (dy.data.JobLevel == "2")
                            {
                                moduleno = "EJDHZYSP";
                            }
                            else if (dy.data.JobLevel == "3")
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
                }
                else
                {
                    if (dy.data.JobLevel == "0")
                        moduleno = "YJFXZYSP";
                    if (dy.data.JobLevel == "1")
                        moduleno = "EJFXZYSP";
                    if (dy.data.JobLevel == "2")
                        moduleno = "SJFXZYSP";
                }
                var data = ApprovalFormBll.ConfigurationByWorkList(dy.data.Id, moduleno);
                return new { Code = 0, Count = 0, Info = "保存成功", data = data };
                //  return Content(data.ToJson(JsonConvert.SerializeObject(dy.data)));
            }
            catch (Exception ex)
            {
                return new { Code = 1, Count = 0, Info = "保存失败" + ex.Message };
            }
        }
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

        public void UploadifyFile(string folderId, HttpFileCollection fileList, ref string path)
        {
            try
            {
                if (fileList.Count > 0)
                {
                    for (int i = 0; i < fileList.AllKeys.Length; i++)
                    {
                        if (fileList.AllKeys[i] != "sign")
                        {
                            HttpPostedFile file = fileList[i];
                            //获取文件完整文件名(包含绝对路径)
                            //文件存放路径格式：/Resource/ResourceFile/{userId}{data}/{guid}.{后缀名}
                            string userId = OperatorProvider.Provider.Current().UserId;
                            string fileGuid = Guid.NewGuid().ToString();
                            long filesize = file.ContentLength;
                            string FileEextension = Path.GetExtension(file.FileName);
                            string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                            string dir = new DataItemDetailBLL().GetItemValue("imgPath") + "\\Resource\\ht";
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
                                fileInfoEntity.RecId = folderId; //关联ID
                                fileInfoEntity.FileName = file.FileName;
                                fileInfoEntity.FilePath = "~/Resource/ht/" + newFileName;
                                fileInfoEntity.FileSize = (Math.Round(decimal.Parse(filesize.ToString()) / decimal.Parse("1024"), 2)).ToString();//文件大小（kb）
                                fileInfoEntity.FileExtensions = FileEextension;
                                fileInfoEntity.FileType = FileEextension.Replace(".", "");
                                fileinfobll.SaveForm("", fileInfoEntity);
                            }
                        }
                        else
                        {
                            HttpPostedFile file = fileList[i];
                            string fileOverName = System.IO.Path.GetFileName(file.FileName);
                            string fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                            string FileEextension = Path.GetExtension(file.FileName);
                            //if (fileName == scEntity.ID)
                            //{
                            string dir = new DataItemDetailBLL().GetItemValue("imgPath") + "\\Resource\\sign";
                            string newFileName = fileName + FileEextension;
                            string newFilePath = dir + "\\" + newFileName;
                            file.SaveAs(newFilePath);
                            //scEntity.SIGNPIC = "/Resource/sign/" + fileOverName;
                            //    break;
                            //}
                            path = fileOverName;
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 开始作业、结束作业 上传附件
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
                        //获取文件完整文件名(包含绝对路径)
                        //文件存放路径格式：/Resource/ResourceFile/{userId}{data}/{guid}.{后缀名}
                        string userId = OperatorProvider.Provider.Current().UserId;
                        string fileGuid = Guid.NewGuid().ToString();
                        long filesize = file.ContentLength;
                        string FileEextension = Path.GetExtension(file.FileName);
                        string fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                        string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                        string virtualPath = string.Format("~/Resource/ht/images/{0}/{1}{2}", uploadDate, fileGuid, FileEextension);
                        string virtualPath1 = string.Format("/Resource/ht/images/{0}/{1}{2}", uploadDate, fileGuid, FileEextension);
                        string fullFileName = dataitemdetailbll.GetItemValue("imgPath") + virtualPath1;
                        //创建文件夹
                        if (!fileName.Contains("sign"))
                        {
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
                                fileInfoEntity.RecId = fileList.AllKeys[i].Replace("file", "") == "_01" ? folderId : folderId + fileList.AllKeys[i].Replace("file", ""); //关联ID
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

    }
}