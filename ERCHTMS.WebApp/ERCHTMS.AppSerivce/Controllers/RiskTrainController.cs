using BSFramework.Util.WebControl;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.RiskDatabase;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Entity.RiskDatabase;
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

namespace ERCHTMS.AppSerivce.Controllers
{
    public class RiskTrainController : BaseApiController
    {
        RisktrainBLL riskBll = new Busines.RiskDatabase.RisktrainBLL();
        private RiskEvaluateBLL riskevaluatebll = new RiskEvaluateBLL();
        /// <summary>
        /// 14.根据状态获取危险预知训练列表
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
                string userId = dy.userid;
                long status = dy.data.status; //查询状态（0:我的，1:全部）
                string taskName = dy.data.taskName;//工作任务名称
                string workstartTime = dy.data.workstartTime;
                string workendTime = dy.data.workendTime;
                long pageIndex = dy.pageindex;
                long pageSize = dy.pagesize;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                if (user == null)
                {
                    return new { Code = -1, Count = 0, Info = "身份票据已失效，请重新登录！" };
                }
                else
                {
                    string where = "1=1";
                    if (!string.IsNullOrWhiteSpace(workstartTime))
                    {
                        where += string.Format(" and to_char(workstarttime,'yyyy-MM-dd')>= '{0}'", Convert.ToDateTime(workstartTime).ToString("yyyy-MM-dd"));
                    }
                    if (!string.IsNullOrWhiteSpace(workendTime))
                    {
                        where += string.Format(" and to_char(workendtime,'yyyy-MM-dd')<='{0}' ", Convert.ToDateTime(workendTime).ToString("yyyy-MM-dd"));
                    }
                    if (!string.IsNullOrWhiteSpace(taskName))
                    {
                        where += string.Format(" and taskname like '%{0}%'", taskName.Trim());
                    }
                    if (status == 0)
                    {
                        where += string.Format(" and (userids like '%,{0},%' or workfzrid='{2}') and status=0 and iscommit=1 and createuserorgcode='{3}'", user.Account, user.UserId, user.Account,user.OrganizeCode);
                    }
                    else
                    {
                        //根据当前用户对模块的权限获取记录
                        where +=" and " +new AuthorizeBLL().GetModuleDataAuthority(user, "04e25e57-7f58-4f51-ab76-91cc291e468c", "createuserdeptcode", "createuserorgcode");
                        if (!string.IsNullOrEmpty(where))
                        {
                            where += string.Format(" and iscommit=1 and (" + where + " or userids like '%,{1},%' or workfzrid='{2}' or createuserid='{0}') ", user.UserId, user.Account, user.Account);
                        }
                        else {
                            where += string.Format(" and (createuserorgcode='{0}' or createuserid='{1}') and iscommit=1", user.OrganizeCode, user.UserId);
                        }
                        //where += string.Format(" and ((createuserorgcode='{0}' and iscommit=1) or createuserid='{1}')", user.OrganizeCode,user.UserId);
                    }
                   
                    Pagination pageObj = new Pagination
                    {
                        p_kid = "id",
                        p_fields = "taskname,to_char(workstarttime,'yyyy-MM-dd hh24:mi') workstarttime,to_char(workendtime,'yyyy-MM-dd hh24:mi') workendtime,taskcontent,status,workusers,workfzr,workfzrid,userids",
                        p_tablename = "bis_risktrain",
                        page = int.Parse(pageIndex.ToString()),
                        rows = int.Parse(pageSize.ToString()),
                        sidx = "createdate",
                        sord = "desc",
                        conditionJson = where
                    };
                    DataTable dt = riskBll.GetPageListJson(pageObj, "");
               
                    return new { Code = 0, Count = dt.Rows.Count, Info = "获取数据成功", data = dt };
                }
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 15. 获取危险预知训练详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetInfo([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string id = dy.data.trainId;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                var rt = riskBll.GetEntity(id);
                if (rt == null)
                {
                    return new { Code = -1, Count = 0, Info = "数据不存在" };
                }
                else
                {
                     var picList = new FileInfoBLL().GetFiles(id+"02");//获取图片

                    string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                    foreach (DataRow dr in picList.Rows)
                    {
                        dr["filepath"] = dr["filepath"].ToString().Replace("~/", webUrl + "/");
                    }
                    var videoList = new FileInfoBLL().GetFiles(id+"01");//获取录音
                    foreach (DataRow dr in videoList.Rows)
                    {
                        dr["filepath"] = dr["filepath"].ToString().Replace("~/", webUrl + "/");
                    }


                    return new
                    {
                        Code = 0,
                        Count = 1,
                        Info = "获取数据成功",
                        data = new
                        {
                            Id = rt.Id,
                            TaskName = rt.TaskName,
                            WorkNum = rt.WorkNum,
                            WorkStartTime = rt.WorkStartTime == null ? "" : rt.WorkStartTime.Value.ToString("yyyy-MM-dd HH:mm"),
                            WorkEndTime = rt.WorkEndTime == null ? "" : rt.WorkEndTime.Value.ToString("yyyy-MM-dd HH:mm"),
                            AreaName = rt.AreaName,
                            AreaId=rt.AreaId,
                            WorkPlace = rt.WorkPlace,
                            WorkUnit = rt.WorkUnit,
                            WorkFzr = rt.WorkFzr,
                            WorkFzrId = rt.WorkFzrId,
                            WorkUsers = rt.WorkUsers,
                            UserIds=rt.UserIds,
                            TaskContent = rt.TaskContent,
                            Status = rt.Status,
                            Measures = new TrainmeasuresBLL().GetListByWorkId(id).Select(t => new { t.RiskContent, t.Measure, t.LsPeople, t.Status, t.Id }).ToList(),//管理措施
                            PicList=picList,//现场图片
                            VideoList=videoList,//现场录音
                            EvaluateList=new RiskEvaluateBLL().GetList().Where(x=>x.WorkId==rt.Id)//效果评价
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
        /// 开展预知训练
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object CarryOutTrain()
        {
            try
            {
                string res = HttpContext.Current.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string deleteids = dy.data.deleteids;//删除附件id集合
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator currUser = OperatorProvider.Provider.Current();
                string measuresJson = JsonConvert.SerializeObject(dy.data.measuresJson);
                List<TrainmeasuresEntity> ListMeasures = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TrainmeasuresEntity>>(measuresJson);//管理措施集合
                string riskJson = JsonConvert.SerializeObject(dy.data.riskEntity);
                RisktrainEntity riskEntity = JsonConvert.DeserializeObject<RisktrainEntity>(riskJson);
                //提交时先查询数据,判断是否还能训练
                RisktrainEntity oldRiskTrain = riskBll.GetEntity(riskEntity.Id);
                if (oldRiskTrain != null) {
                    //训练已经完成此条数据不允许提交
                    if (oldRiskTrain.Status == 1) {
                        return new { code = -1, count = 0, info = "此训练已经结束!无法继续提交!" };
                    }
                }
                if (riskEntity.WorkFzrId == currUser.Account) {
                    riskEntity.Status = 1;
                }
                for (int i = 0; i < ListMeasures.Count; i++)
                {
                    ListMeasures[i].Id = Guid.NewGuid().ToString();
                    ListMeasures[i].WorkId = riskEntity.Id;
                }
                riskBll.SaveForm(riskEntity.Id, riskEntity, ListMeasures);
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
                            if (file.ContentType.Contains("image"))
                            {
                                fileInfoEntity.RecId = riskEntity.Id + "02";
                            }
                            else
                            {
                                fileInfoEntity.RecId = riskEntity.Id + "01";
                            }
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
                return new { code = 0, count = 1, info = "提交成功" };
            }
            catch (Exception ex)
            {

                return new { code = -1, count = 0, info = ex.Message };
            }
           
        }
        /// <summary>
        /// 提交效果评价
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object CommitEvaluate()
        {
            try
            {
                string res = HttpContext.Current.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator currUser = OperatorProvider.Provider.Current();
                string EvaluateJson = JsonConvert.SerializeObject(dy.data.riskEvaluate);
                RiskEvaluate riskEvaluate = JsonConvert.DeserializeObject<RiskEvaluate>(EvaluateJson);
                riskevaluatebll.SaveForm(riskEvaluate.ID, riskEvaluate);
                return new { code = 0, count = 1, info = "提交成功" };
            }
            catch (Exception ex)
            {

                return new { code = -1, count = 0, info = ex.Message };
            }
          
        }
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
}