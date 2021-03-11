using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Services.Description;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SafeReward;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Entity.RoutineSafetyWork;
using ERCHTMS.Entity.SafeReward;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceStack.Messaging.Rcon;

namespace ERCHTMS.AppSerivce.Controllers
{
    public class SafeRewardController : BaseApiController
    {
        SaferewardBLL safereward =  new SaferewardBLL();
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
        private AptitudeinvestigateauditBLL aptitudeinvestigateauditbll = new AptitudeinvestigateauditBLL();
        private SaferewarddetailBLL saferewarddetailbll = new SaferewarddetailBLL();
        public HttpContext ctx { get { return HttpContext.Current; } }
        // GET api/safereward
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/safereward/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/safereward
        public void Post([FromBody]string value)
        {
        }

        // PUT api/safereward/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/safereward/5
        public void Delete(int id)
        {
        }

        /// <summary>
        /// 保存安全奖励
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object SafeRewardSave()
        {
            try
            {
                string res = ctx.Request["json"];
                //dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                var dyObj = JsonConvert.DeserializeAnonymousType(res, new
                {
                    userid = string.Empty,
                    data = new RewardModel()
                });
                string userId = dyObj.userid; //当前用户ID 
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                //SaferewardEntity entity = new SaferewardEntity();
                //CopyProperty(dyObj.data, entity);
                string keyValue = !string.IsNullOrEmpty(dyObj.data.keyvalue) ? dyObj.data.keyvalue : Guid.NewGuid().ToString();               
                var year = DateTime.Now.ToString("yyyy");
                var month = DateTime.Now.ToString("MM");
                var day = DateTime.Now.ToString("dd");
                var rewardCode = "Q/CRPHZHB 2208.06.01-JL01-" + year + month + day + safereward.GetRewardCode();
                dyObj.data.rewentity.SafeRewardCode = !string.IsNullOrEmpty(dyObj.data.rewentity.SafeRewardCode) ? dyObj.data.rewentity.SafeRewardCode : rewardCode;
                

                //删除图片
                string delFileIds = !string.IsNullOrEmpty(dyObj.data.delfileids) ? dyObj.data.delfileids : "";
                if (!string.IsNullOrEmpty(delFileIds))
                {
                    DeleteFile(delFileIds);
                }

                int? rewardmoney = 0;
                HttpFileCollection files = ctx.Request.Files;//上传的文件 
                //上传设备图片
                UploadifyFile(keyValue, files);

                if (saferewarddetailbll.Remove(keyValue) > 0)
                {
                    foreach (SaferewarddetailEntity data in dyObj.data.rewarddetailentity)
                    {
                        rewardmoney += data.RewardNum;
                        data.RewardId = keyValue;
                        saferewarddetailbll.SaveForm("", data);
                    }
                }
                dyObj.data.rewentity.RewardMoney = rewardmoney;
                safereward.SaveForm(keyValue, dyObj.data.rewentity);

            }
            catch (Exception)
            {
                return new { code = -1, count = 0, info = "保存失败" };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }


        /// <summary>
        /// 提交安全奖励
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object SafeRewardApply()
        {
            try
            {
                string res = ctx.Request["json"];
                var dyObj = JsonConvert.DeserializeAnonymousType(res, new
                {
                    userid = string.Empty,
                    data = new RewardModel()
                });
                string userId = dyObj.userid; //当前用户ID 
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string keyValue = !string.IsNullOrEmpty(dyObj.data.keyvalue) ? dyObj.data.keyvalue : Guid.NewGuid().ToString();
                if (dyObj.data.rewentity != null && (string.IsNullOrEmpty(dyObj.data.rewentity.ApplyState) || dyObj.data.rewentity.ApplyState == "0"))
                {
                    var year = DateTime.Now.ToString("yyyy");
                    var month = DateTime.Now.ToString("MM");
                    var day = DateTime.Now.ToString("dd");
                    var rewardCode = "Q/CRPHZHB 2208.06.01-JL01-" + year + month + day + safereward.GetRewardCode();
                    dyObj.data.rewentity.SafeRewardCode = !string.IsNullOrEmpty(dyObj.data.rewentity.SafeRewardCode) ? dyObj.data.rewentity.SafeRewardCode : rewardCode;
                    

                    //删除图片
                    string delFileIds = !string.IsNullOrEmpty(dyObj.data.delfileids) ? dyObj.data.delfileids : "";
                    if (!string.IsNullOrEmpty(delFileIds))
                    {
                        DeleteFile(delFileIds);
                    }
                    HttpFileCollection files = ctx.Request.Files;//上传的文件 
                    //上传设备图片
                    UploadifyFile(keyValue, files);
                    int? rewardmoney = 0;
                    if (saferewarddetailbll.Remove(keyValue) > 0)
                    {
                        foreach (SaferewarddetailEntity data in dyObj.data.rewarddetailentity)
                        {
                            rewardmoney += data.RewardNum;
                            data.RewardId = keyValue;
                            saferewarddetailbll.SaveForm("", data);
                        }
                    }
                    dyObj.data.rewentity.RewardMoney = rewardmoney;
                    safereward.SaveForm(keyValue, dyObj.data.rewentity);
                }

                if (!string.IsNullOrEmpty(keyValue) && dyObj.data.entity != null)
                {
                    if (!string.IsNullOrEmpty(dyObj.data.entity.AUDITSIGNIMG))
                    {
                        string strurl = new DataItemDetailBLL().GetItemValue("imgUrl");
                        dyObj.data.entity.AUDITSIGNIMG = dyObj.data.entity.AUDITSIGNIMG.Replace(strurl, "../../");
                    }
                    safereward.CommitApply(keyValue, dyObj.data.entity, dyObj.data.leadershipid);
                }           

            }
            catch (Exception)
            {
                return new { code = -1, count = 0, info = "保存失败" };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }


        /// <summary>
        /// 获取安全奖励列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public Object GetSafeRewardList([FromBody]JObject json)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;

                //是否只查看我的工作
                string pager = res.Contains("pager") ? dy.data.pager : "";
                //开始日期
                string sTime = res.Contains("stime") ? dy.data.stime : "";
                //结束日期
                string eTime = res.Contains("etime") ? dy.data.etime : "";

                //流程状态
                string flowstate = res.Contains("flowstate") ? dy.data.flowstate : "";

                //模糊查询条件
                string keyword = res.Contains("keyword") ? dy.data.keyword : "";

                int pageSize = res.Contains("pagesize") ? int.Parse(dy.data.pagesize.ToString()) : 10; //每页条数

                int pageIndex = res.Contains("pagenum") ? int.Parse(dy.data.pagenum.ToString()) : 1; //请求页码

                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                Pagination pagination = new Pagination();
                pagination.page = int.Parse(pageIndex.ToString());
                pagination.rows = int.Parse(pageSize.ToString());
                pagination.p_kid = "a.Id";
                pagination.p_fields = @"a.CreateUserId,a.CreateDate,a.CreateUserName,a.ModifyUserId,a.ModifyDate,a.ModifyUserName,a.CreateUserDeptCode,a.CreateUserOrgCode,FlowState,ApplyUserId,ApproverPeopleIds,
                                 ApplyState,SafeRewardCode,ApplyDeptName,ApplyTime,ApplyRewardRmb,RewardRemark,rewardname as RewardUserName";
                pagination.p_tablename = "Bis_SafeReward a left join (select row_number()over(partition by rewardid order by createdate asc) as rn,rewardname,rewardid from  bis_saferewarddetail ) b on a.id=b.rewardid and rn=1";
                pagination.sidx = "a.CreateDate";//排序字段
                pagination.sord = "desc";//排序方式
                pagination.conditionJson = "1=1";
                string queryJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    pager = pager,
                    sTime = sTime,
                    eTime = eTime,
                    flowstate =flowstate ,
                    keyword = keyword
                });
                var rewarddata = safereward.GetPageList(pagination, queryJson);
                var data = new
                {
                    rows = rewarddata,
                    total = pagination.total,
                    page = pagination.page,
                    records = pagination.records,
                    costtime = CommonHelper.TimerEnd(watch)
                };
                return new { Code = 0, Count = pagination.records, Info = "获取数据成功", data = data };

            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }




        /// <summary>
        /// 获取安全奖励详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetSafeRewardDetail([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string keyValue = res.Contains("id") ? dy.data.id : "";
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                object obj = safereward.GetEntity(keyValue);
                List<FileInfoEntity> file = fileInfoBLL.GetFileList(keyValue);
                string strurl = new DataItemDetailBLL().GetItemValue("imgUrl");
                List<object> objects = new List<object>();

                foreach (FileInfoEntity itemEntity in file)
                {
                    objects.Add(new
                    {
                        fileid = itemEntity.FileId,
                        filepath =strurl + itemEntity.FilePath.Replace("~", "")
                    });
                }
                var rewarddetaildata = saferewarddetailbll.GetList("").Where(p => p.RewardId == keyValue);
                object reward = new
                {
                    safereward = obj,
                    rewardfile = objects,
                    rewarddetaildata = rewarddetaildata
                };
                return new { Code = 0, Count = -1, Info = "获取数据成功", data = reward };

            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 获取安全奖励标准
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetStandardJson([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                object obj = safereward.GetStandardJson();
                if (obj == null)
                {
                    return new { Code = -1, Count = 0, Info = "没有查询到数据！" };
                }
                else
                {
                    return new { Code = 0, Count = -1, Info = "获取数据成功", data = obj };
                }

            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }


        /// <summary>
        /// 获取历史审核记录
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetSpecialAuditList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string keyValue = res.Contains("id") ? dy.data.id : "";
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                var obj = aptitudeinvestigateauditbll.GetAuditList(keyValue).Where(p => p.REMARK != "0").ToList();
                if (obj == null)
                {
                    return new { Code = -1, Count = 0, Info = "没有查询到数据！" };
                }
                else
                {
                    string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                    foreach (var item in obj)
                    {
                        item.AUDITSIGNIMG = item.AUDITSIGNIMG.Replace("../../", webUrl + "/");
                    }
                    return new { Code = 0, Count = -1, Info = "获取数据成功", data = obj };
                }

            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }


        /// <summary>
        /// 根据部门id(部门层级)获取审核人信息
        /// </summary>
        /// <returns>返回对象Json</returns>
        [HttpPost]
        public object GetDeptPId([FromBody]JObject json)
        {          
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string applyDeptId = res.Contains("applydeptid") ? dy.data.applydeptid : "";
                if (!string.IsNullOrEmpty(applyDeptId) && applyDeptId != "0")
                {
                    //根据被奖励人部门id获取部门部门级部门id
                    var deptId = safereward.GetDeptPId(applyDeptId);

                    //根据部门id(部门层级)获取审核人信息
                    var obj = safereward.GetSpecialtyPrincipal(deptId);
                    if (obj == null)
                    {
                        return new { Code = -1, Count = 0, Info = "没有查询到数据！" };
                    }
                    else
                    {
                        return new { Code = 0, Count = -1, Info = "获取数据成功", data = obj };
                    }
                }

                return new { Code = -1, Count = 0, Info = "没有查询到数据！" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 获取分管领导
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetLeaderList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                object obj = safereward.GetLeaderList();
                if (obj == null)
                {
                    return new { Code = -1, Count = 0, Info = "没有查询到数据！" };
                }
                else
                {
                    return new { Code = 0, Count = -1, Info = "获取数据成功", data = obj };
                }

            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }


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
                            fileInfoEntity.FilePath = "~/Resource/ht/images/" + uploadDate +'/'+ newFileName;
                            fileInfoEntity.FileSize = (Math.Round(decimal.Parse(filesize.ToString()) / decimal.Parse("1024"), 2)).ToString();//文件大小（kb）
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
                    var entity = fileInfoBLL.GetEntity(s);
                    if (entity != null)
                    {
                       // var filePath = ctx.Server.MapPath(entity.FilePath);
                        var filePath = new DataItemDetailBLL().GetItemValue("imgPath") +
                                       entity.FilePath.Replace("~", "");
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
        /// json字符串反射对象
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>

        public void CopyProperty(object source, object target)
        {
            if (source == null || target == null)
                return;
            Type t = source.GetType();
            PropertyInfo[] ps = t.GetProperties();
            foreach (var item in ps)
            {
                Type t1 = target.GetType();
                PropertyInfo[] ps1 = t1.GetProperties();
                foreach (var item1 in ps1)
                {
                    if (item.Name.ToLower() == item1.Name.ToLower())
                    {
                        object o = item.GetValue(source, null);
                        if (o != null)
                        {
                            item1.SetValue(target, o);
                        }
                    }
                }
            }
        }
    }


    public class RewardModel
    {
        public string keyvalue { get; set; }
        public AptitudeinvestigateauditEntity entity { get; set; }
        public SaferewardEntity rewentity { get; set; }

        public IList<SaferewarddetailEntity> rewarddetailentity { get; set; }

        public string leadershipid { get; set; }

        public string delfileids { get; set; }
    }
}
