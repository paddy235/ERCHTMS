using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.AppSerivce.Model;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SafetyWorkSupervise;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Entity.SafetyWorkSupervise;
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
    [HandlerLogin(LoginMode.Enforce)]
    public class SafetyWorkSuperviseController : BaseApiController
    {
        public HttpContext ctx { get { return HttpContext.Current; } }
        private SafetyworksuperviseBLL safetyworksupervisebll = new SafetyworksuperviseBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
        private SafetyworkfeedbackBLL safetyworkfeedbackbll = new SafetyworkfeedbackBLL();
        private SuperviseconfirmationBLL superviseconfirmationbll = new SuperviseconfirmationBLL();
        /// <summary>
        /// 获取安全重点工作督办列表（全部记录）
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetSafetyWorkSuperviseList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid; //用户ID 
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "身份票据已失效,请重新登录!" };
                }
                string supervisedate = dy.data.supervisedate;//督办时间
                string dutydeptcode = dy.data.dutydeptcode;//责任单位code
                string flowstate = dy.data.flowstate;//状态,1办理反馈中,2督办确认中,3已结束
                string worktask = dy.data.worktask;//工作任务
                int pageIndex = int.Parse(dy.pageindex.ToString());
                int pageSize = int.Parse(dy.pagesize.ToString());
                string flag = dy.data.flag;//0查全部,1查待办
                string appflag = "1";
                if (string.IsNullOrEmpty(flowstate)) {
                    flowstate = "-1";
                }

                Pagination pagination = new Pagination();
                pagination.conditionJson = " 1=1 ";
                pagination.page = pageIndex;
                pagination.rows = pageSize;
                string queryJson = new
                {
                    supervisedate = supervisedate,
                    flowstate = flowstate,
                    keyword = worktask,
                    code = dutydeptcode,
                    appflag= appflag
                }.ToJson();
                if (flag == "1")
                {
                    queryJson = new
                    {
                        showrange = "1",
                        flowstate = "-1"
                    }.ToJson();
                }
                else if (flag == "2") {
                    queryJson = new
                    {
                        showrange = "1",
                        flowstate = flowstate
                    }.ToJson();
                }
                var data = safetyworksupervisebll.GetPageList(pagination, queryJson);
                return new { Code = 0, Info = "获取数据成功", Count = pagination.records, data = data };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }

        }

        /// <summary>
        /// 获取安全重点工作督办详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetSafetyWorkSuperviseEntity([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid; //用户ID 
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "身份票据已失效,请重新登录!" };
                }
                string webUrl = dataitemdetailbll.GetItemValue("imgUrl");
                string id = dy.data.safetyworkid;//安全重点工作督办id
                //string fid = dy.data.historyid;//历史记录id
                var data = safetyworksupervisebll.GetEntityByT(id, "");
                List<SuperviseEntity> list = new List<SuperviseEntity>();
                SuperviseEntity entity = new SuperviseEntity();
                dynamic resdata = new ExpandoObject();
                if (data != null && data.Rows.Count > 0)
                {
                    list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SuperviseEntity>>(data.ToJson());
                }
                if (list.Count > 0)
                {
                    entity = list[0];
                    if (!string.IsNullOrEmpty(entity.SignUrl)) {
                        entity.SignUrl = webUrl + entity.SignUrl.ToString().Replace("../../", "/");
                    }
                    if (!string.IsNullOrEmpty(entity.SignUrlT))
                    {
                        entity.SignUrlT = webUrl + entity.SignUrlT.ToString().Replace("../../", "/");
                    }
                    //会议资料附件
                    DataTable file = fileInfoBLL.GetFiles(entity.Fid);
                    IList<Photo> rList = new List<Photo>();
                    foreach (DataRow dr in file.Rows)
                    {
                        Photo p = new Photo();
                        p.id = dr["fileid"].ToString();
                        p.filename = dr["filename"].ToString();
                        p.fileurl = webUrl + dr["filepath"].ToString().Substring(1);
                        rList.Add(p);
                    }
                    resdata = new
                    {
                        id = entity.Id,
                        createdate = !string.IsNullOrEmpty(entity.CreateDate.ToString()) ? entity.CreateDate.Value.ToString("yyyy-MM-dd") : "",
                        createusername = entity.CreateUserName,
                        supervisedate = !string.IsNullOrEmpty(entity.SuperviseDate.ToString()) ? entity.SuperviseDate.Value.ToString("yyyy-MM-dd") : "",
                        worktask = entity.WorkTask,
                        dutydeptname = entity.DutyDeptName,
                        dutydeptid = entity.DutyDeptId,
                        dutydeptcode = entity.DutyDeptCode,
                        dutyperson = entity.DutyPerson,
                        dutypersonid = entity.DutyPersonId,
                        superviseperson = entity.SupervisePerson,
                        supervisepersonid = entity.SupervisePersonId,
                        finishdate = !string.IsNullOrEmpty(entity.FinishDate.ToString()) ? entity.FinishDate.Value.ToString("yyyy-MM-dd") : "",
                        flowstate = entity.FlowState,
                        remark = entity.Remark,
                        fid = entity.Fid,
                        btgnum = entity.btgnum,
                        cid = entity.Cid,
                        finishinfo = entity.FinishInfo,
                        feedbackdate = !string.IsNullOrEmpty(entity.FeedbackDate.ToString()) ? entity.FeedbackDate.Value.ToString("yyyy-MM-dd") : "",
                        superviseresult = entity.SuperviseResult,
                        superviseopinion = entity.SuperviseOpinion,
                        confirmationdate = !string.IsNullOrEmpty(entity.ConfirmationDate.ToString())? entity.ConfirmationDate.Value.ToString("yyyy-MM-dd"): "",
                        signurl = entity.SignUrl,
                        signurlt = entity.SignUrlT,
                        supervisedeptname = entity.SuperviseDeptName,
                        filelist= rList
                    };
                }
                return new { Code = 0, Info = "获取数据成功", data = resdata };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 获取安全重点工作督办历史记录详情列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetSafetyWorkSuperviseHistoryList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid; //用户ID 
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "身份票据已失效,请重新登录!" };
                }
                string safetyworkid = dy.data.safetyworkid;//安全重点工作id
                int pageIndex = int.Parse(dy.pageindex.ToString());
                int pageSize = int.Parse(dy.pagesize.ToString());

                Pagination pagination = new Pagination();
                pagination.page = pageIndex;
                pagination.rows = pageSize;
                List<object> list = new List<object>();
                DataTable data = safetyworkfeedbackbll.GetPageList(pagination, safetyworkid);
                if (data != null && data.Rows.Count > 0) {

                    string webUrl = dataitemdetailbll.GetItemValue("imgUrl");
                    string superviseresult = string.Empty;
                    string title = string.Empty;
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        superviseresult = data.Rows[i]["superviseresult"].ToString();//督办结果1不同意，0同意
                        if (superviseresult == "1")
                        {
                            title = data.Rows[i]["feedbackdate"].ToString() + "办理反馈未通过信息";
                        }
                        else {
                            title = "办理反馈通过信息";
                        }
                        DataTable file = fileInfoBLL.GetFiles(data.Rows[i]["id"].ToString());
                        IList<Photo> rList = new List<Photo>();
                        foreach (DataRow dr in file.Rows)
                        {
                            Photo p = new Photo();
                            p.id = dr["fileid"].ToString();
                            p.filename = dr["filename"].ToString();
                            p.fileurl = webUrl + dr["filepath"].ToString().Substring(1);
                            rList.Add(p);
                        }

                        dynamic dys = new
                        {
                            id = data.Rows[i]["id"].ToString(),
                            title = title,
                            finishinfo = data.Rows[i]["finishinfo"].ToString(),
                            signurl = webUrl + data.Rows[i]["signurl"].ToString().Replace("../../", "/"),
                            feedbackdate = data.Rows[i]["feedbackdate"].ToString(),
                            superviseresult = superviseresult == "1" ? "不通过" : "通过",
                            superviseopinion = data.Rows[i]["superviseopinion"].ToString(),
                            signurlt = webUrl + data.Rows[i]["signurlt"].ToString().Replace("../../", "/"),
                            confirmationdate = data.Rows[i]["confirmationdate"].ToString(),
                            filelist = rList
                        };
                        
                        list.Add(dys);
                    }
                }
                return new { Code = 0, Info = "获取数据成功", Count = pagination.records, data = list };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }

        }

        /// <summary>
        /// 保存反馈信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object SaveFeedback()
        {
            try
            {
                string res = ctx.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid; //当前用户ID 
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string webUrl = dataitemdetailbll.GetItemValue("imgUrl");
                string signurl = dy.data.signurl;
                SafetyworkfeedbackEntity entity = new SafetyworkfeedbackEntity();
                entity.FeedbackDate = Convert.ToDateTime(dy.data.feedbackdate);//反馈时间
                entity.FinishInfo = dy.data.finishinfo;//完成情况
                entity.SignUrl = string.IsNullOrWhiteSpace(signurl) ? "" : signurl.Replace(webUrl, "").ToString();//签名url
                entity.SuperviseId = dy.data.superviseid;//主表id
                entity.Flag = "0";//数据状态,0表示当前数据
                safetyworkfeedbackbll.SaveForm("", entity);
                HttpFileCollection files = ctx.Request.Files;//上传的文件 
                //上传设备图片
                UploadifyFile(entity.Id, "SecurityRedList", files);
            }
            catch (Exception)
            {
                return new { code = -1, count = 0, info = "保存失败" };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }

        /// <summary>
        /// 保存督办确认信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object SaveConfirmation()
        {
            try
            {
                string res = ctx.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid; //当前用户ID 
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string webUrl = dataitemdetailbll.GetItemValue("imgUrl");
                string signurl = dy.data.signurl;
                SuperviseconfirmationEntity entity = new SuperviseconfirmationEntity();
                entity.SuperviseResult = dy.data.superviseresult;//督办结果0同意,1不同意
                entity.SuperviseOpinion = dy.data.superviseopinion;//督办意见
                entity.SignUrl = string.IsNullOrWhiteSpace(signurl) ? "" : signurl.Replace(webUrl, "").ToString();//签名url
                entity.SuperviseId = dy.data.superviseid;//主表id
                entity.FeedbackId = dy.data.feedbackid;//反馈id
                entity.Flag = "0";//数据状态,0表示当前数据
                entity.ConfirmationDate = Convert.ToDateTime(dy.data.confirmationdate);//确认时间
                superviseconfirmationbll.SaveForm("", entity);
            }
            catch (Exception)
            {
                return new { code = -1, count = 0, info = "保存失败" };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }
        [HttpPost]
        public object MutilConfirm()
        {
            try
            {
                string res = ctx.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid; //当前用户ID 
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string webUrl = dataitemdetailbll.GetItemValue("imgUrl");
                string signurl = dy.data.signurl;
                string result = dy.data.result;
                string ids = dy.data.superviseids;
                string remark = res.Contains("remark")?dy.data.remark:"";
                if(!string.IsNullOrWhiteSpace(signurl))
                {
                    signurl = signurl.ToLower();
                    int idx = signurl.IndexOf("/resource");
                    signurl = signurl.Substring(idx);
                }
                bool isOK=safetyworksupervisebll.MutilConfirm(ids, result, signurl,remark);
                if(isOK)
                {
                    return new { code = 0, info = "操作成功" };
                }
                else
                {
                    return new { code = -1,  info = "操作失败" };
                }
            }
            catch (Exception)
            {
                return new { code = -1, count = 0, info = "操作失败" };
            }

           
        }
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
                        //获取文件完整文件名(包含绝对路径)
                        //文件存放路径格式：/Resource/ResourceFile/{userId}{data}/{guid}.{后缀名}
                        string fileGuid = Guid.NewGuid().ToString();
                        long filesize = file.ContentLength;
                        string FileEextension = Path.GetExtension(file.FileName);
                        string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                        string virtualPath = string.Format("~/Resource/DocumentFile/SafetyWorkSupervise/{0}/{1}{2}" , uploadDate, fileGuid, FileEextension);
                        string virtualPath1 = string.Format("/Resource/DocumentFile/SafetyWorkSupervise/{0}/{1}{2}", uploadDate, fileGuid, FileEextension);
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
                            fileInfoEntity.RecId = folderId;
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
            }
        }

    }
}