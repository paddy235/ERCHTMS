using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERCHTMS.Code;
using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Entity.PublicInfoManage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ERCHTMS.Busines.RiskDatabase;
using ERCHTMS.Cache;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.HighRiskWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;

namespace ERCHTMS.AppSerivce.Controllers
{
    public class FireWaterController : BaseApiController
    {
        FireWaterBLL firewaterbll = new FireWaterBLL();
        ScaffoldauditrecordBLL scaffoldauditrecordbll = new ScaffoldauditrecordBLL();
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private HighRiskRecordBLL highriskrecordbll = new HighRiskRecordBLL();
        private DataItemCache dataItemCache = new DataItemCache();
        private ScaffoldBLL scaffoldbll = new ScaffoldBLL();


        #region 1.获取许可状态
        /// <summary>
        /// 获取许可状态
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetPermitType([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            try
            {
                string type = dy.type;
                string str = string.Empty;
                if (type == "0")
                {
                    str = @"[{ ItemValue: '0', ItemName: '申请中' },
                                { ItemValue: '1', ItemName: '审核(批)中'},
                                { ItemValue: '2', ItemName: '审核(批)未通过'},
                                { ItemValue: '3', ItemName: '审核(批)通过' }
                            ]";
                }
                else
                {
                    str = @"[{ ItemValue: '0', ItemName: '即将作业' },
                                { ItemValue: '1', ItemName: '作业中'},
                                { ItemValue: '2', ItemName: '已结束' },
                                { ItemValue: '3', ItemName: '作业暂停' }
                            ]";
                }
                var data = JsonConvert.DeserializeObject<List<itemClass>>(str);
                return new { code = 0, count = data.Count, info = "获取数据成功", data = data }; ;
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }
        #endregion

        /// <summary>
        /// 得到消防水使用申请列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetList([FromBody]JObject json)
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
                        status = string.Empty,//许可状态
                        st = string.Empty,//开始时间
                        et = string.Empty,//结束时间
                        workdeptcode = string.Empty,//使用消防水单位
                        viewrange = string.Empty,//self:我的申请 selfaudit:我的审核 all:全部
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
                var watch = CommonHelper.TimerStart();
                Pagination pagination = new Pagination();
                pagination.page = dy.data.pagenum;
                pagination.rows = dy.data.pagesize;
                pagination.sidx = "a.createdate";     //排序字段
                pagination.sord = "desc";             //排序方式
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var list = firewaterbll.GetList(pagination, JsonConvert.SerializeObject(dy.data), "app", user);

                Dictionary<string, string> dict_props = new Dictionary<string, string>();
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                    DateFormatString = "yyyy-MM-dd HH:mm", //格式化日期
                };
                return new { code = 0, info = "获取数据成功", count = pagination.records, data = JArray.Parse(JsonConvert.SerializeObject(list, Formatting.None, settings)) };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message };
            }
        }

        /// <summary>
        /// 得到台账列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetLedgerList([FromBody]JObject json)
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
                        workdeptcode = string.Empty,
                        st = string.Empty,
                        et = string.Empty,
                        ledgertype = string.Empty,      //0-即将作业 1-作业中 2:已结束
                        applynumber = string.Empty
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
                pagination.sidx = "a.createdate desc";
                var list = firewaterbll.GetLedgerList(pagination, JsonConvert.SerializeObject(dy.data), curUser);
                int count = pagination.records;
                pagination.rows = 100000;
                var historyCount = firewaterbll.GetLedgerList(pagination, JsonConvert.SerializeObject(dy.data), curUser).Rows.Count;
                var query = new
                {
                    workdeptcode = dy.data.workdeptcode,
                    st = dy.data.st,
                    et = dy.data.et,
                    ledgertype = "1",      //0-即将作业 1-作业中 2:已结束
                    applynumber = dy.data.applynumber
                };
                var workingCount = firewaterbll.GetLedgerList(pagination, JsonConvert.SerializeObject(query), curUser).Rows.Count;
                Dictionary<string, string> dict_props = new Dictionary<string, string>();
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                    DateFormatString = "yyyy-MM-dd HH:mm" //格式化日期
                };
                var data = new
                {
                    list = list,
                    historyCount = historyCount,
                    workingCount = workingCount
                };
                settings.Converters.Add(new DecimalToStringConverter());
                return new { code = 0, info = "获取数据成功", count = count, data = JObject.Parse(JsonConvert.SerializeObject(data, Formatting.None, settings)) };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message };
            }

        }

        /// <summary>
        /// 消防水使用详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetForm([FromBody]JObject json)
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
                var fireWaterEntity = firewaterbll.GetEntity(dy.data.id);
                if (fireWaterEntity == null)
                {
                    throw new ArgumentException("未找到信息");
                }
                string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                string jsondata = JsonConvert.SerializeObject(fireWaterEntity);
                FireWaterModel model = JsonConvert.DeserializeObject<FireWaterModel>(jsondata);
                if (model.SpecialtyType != null)
                    model.SpecialtyTypeName = scaffoldbll.getName(fireWaterEntity.SpecialtyType, "SpecialtyType");
                model.FireWaterAudits = scaffoldauditrecordbll.GetList(fireWaterEntity.Id);
                foreach (var item in model.FireWaterAudits)
                {
                    item.AuditSignImg = string.IsNullOrWhiteSpace(item.AuditSignImg) ? "" : webUrl + item.AuditSignImg.ToString().Replace("../../", "/");
                }
                DataTable cdt = fileInfoBLL.GetFiles(model.Id);
                IList<Photo> cfiles = new List<Photo>();
                foreach (DataRow item in cdt.Rows)
                {
                    Photo p = new Photo();
                    p.fileid = item[0].ToString();
                    p.filename = item[1].ToString();
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + item[2].ToString().Substring(1);
                    cfiles.Add(p);
                }
                model.cfiles = cfiles;
                DataTable conditionfile = fileInfoBLL.GetFiles(model.Id + "01");
                IList<Photo> conditionFiles = new List<Photo>();
                foreach (DataRow item in conditionfile.Rows)
                {
                    Photo p = new Photo();
                    p.fileid = item[0].ToString();
                    p.filename = item[1].ToString();
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + item[2].ToString().Substring(1);
                    conditionFiles.Add(p);
                }
                model.conditionFiles = conditionFiles;

                model.conditionEntity = firewaterbll.GetConditionEntity(model.Id);

                model.RiskRecord = highriskrecordbll.GetList(model.Id).ToList();
                string moduleName = string.Empty;
                string projectid = "";
                if (fireWaterEntity.WorkDeptType == "0")//单位内部
                {
                    moduleName = "消防水使用-内部审核";
                }
                else
                {
                    moduleName = "消防水使用-外部审核";
                    projectid = fireWaterEntity.EngineeringId;
                }
                var nodelist = firewaterbll.GetAppFlowList(fireWaterEntity.Id, moduleName);
                model.CheckFlow = nodelist;

                #region 获取执行情况
                IList<FireWaterCondition> conditionlist = firewaterbll.GetConditionList(fireWaterEntity.Id).OrderBy(t => t.CreateDate).ToList();
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
                        ERCHTMS.Entity.HighRiskWork.ViewModel.Photo pic = new ERCHTMS.Entity.HighRiskWork.ViewModel.Photo();
                        pic.filename = temp.FileName;
                        pic.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + temp.FilePath.Substring(1);
                        pic.fileid = temp.FileId;
                        tempfilelist.Add(pic);
                    }
                    item.filelist = tempfilelist;
                    item.num = i / 2 + 1;
                }
                model.conditionlist = conditionlist;
                #endregion

                Dictionary<string, string> dict_props = new Dictionary<string, string>();
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                    DateFormatString = "yyyy-MM-dd HH:mm"
                };
                return new { code = 0, info = "获取数据成功", data = JObject.Parse(JsonConvert.SerializeObject(model, Formatting.None, settings)) };
            }
            catch (Exception ex)
            {
                return new { code = -1, data = "", info = ex.Message };
            }
        }

        /// <summary>
        /// 消防水使用申请审核
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object Audit()
        {
            try
            {
                string res = HttpContext.Current.Request["json"];
                var dy = JsonConvert.DeserializeAnonymousType(res, new
                {
                    userid = string.Empty,
                    data = new
                    {
                        id = string.Empty,
                        auditentity = new ScaffoldauditrecordEntity()
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
                    throw new ArgumentException("审核出错，错误信息：data为null");
                }
                if (dy.data.auditentity == null)
                {
                    throw new ArgumentException("审核出错，错误信息：参数为null");
                }
                if (string.IsNullOrEmpty(dy.data.id))
                {
                    throw new ArgumentException("缺少参数：id为空");
                }
                FireWaterEntity firewaterentity = firewaterbll.GetEntity(dy.data.id);
                if (firewaterentity == null)
                {
                    throw new ArgumentException("审核出错，申请信息不存在");
                }
                ////判断当前角色权限 角色一致且部门一样
                //string[] curUserRoles = curUser.RoleName.ToString().Split(',');
                //if (!firewaterentity.FlowDept.Contains(curUser.DeptId))
                //{
                //    throw new ArgumentException("无当前部门权限");
                //}
                //var isApprove = false;
                //foreach (var r in curUserRoles)
                //{
                //    if (firewaterentity.FlowRoleName.Contains(r))
                //    {
                //        isApprove = true;
                //        break;
                //    }
                //}
                //if (!isApprove)
                //{
                //    throw new ArgumentException("无当前角色权限");
                //}
                if (firewaterentity.ApplyState == "2" || firewaterentity.ApplyState == "3")
                {
                    throw new ArgumentException("此申请已处理");
                }
                string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                dy.data.auditentity.AuditSignImg = string.IsNullOrWhiteSpace(dy.data.auditentity.AuditSignImg) ? "" : dy.data.auditentity.AuditSignImg.Replace(webUrl, "").ToString();
                HttpFileCollection files = HttpContext.Current.Request.Files;
                if (files.Count > 0)
                {
                    for (int i = 0; i < files.AllKeys.Length; i++)
                    {
                        HttpPostedFile file = files[i];
                        string fileOverName = System.IO.Path.GetFileName(file.FileName);
                        string fileName = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                        string FileEextension = Path.GetExtension(file.FileName);
                        if (fileName == dy.data.auditentity.Id)
                        {
                            string dir = new DataItemDetailBLL().GetItemValue("imgPath") + "\\Resource\\sign";
                            string newFileName = fileName + FileEextension;
                            string newFilePath = dir + "\\" + newFileName;
                            file.SaveAs(newFilePath);
                            dy.data.auditentity.AuditSignImg = "/Resource/sign/" + fileOverName;
                            break;
                        }
                    }
                }
                dy.data.auditentity.Id = null;
                firewaterbll.ApplyCheck(dy.data.id, dy.data.auditentity);
                return new { code = 0, data = "", info = "操作成功" };

            }
            catch (Exception ex)
            {
                return new { code = -1, data = "", info = ex.Message };
            }
        }
        /// <summary>
        /// 执行情况确认
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object ConditionSubmit()
        {
            try
            {
                string res = HttpContext.Current.Request["json"];
                var dy = JsonConvert.DeserializeAnonymousType(res, new
                {
                    userid = string.Empty,
                    data = new FireWaterCondition()
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
                var conditionModel = dy.data;
                var waterEntity = firewaterbll.GetEntity(conditionModel.FireWaterId);
                waterEntity.ConditionState = "1";
                firewaterbll.SaveForm(waterEntity.Id, waterEntity);
                HttpFileCollection files = HttpContext.Current.Request.Files;
                UploadifyFile(conditionModel.FireWaterId + "01", "scaffoldfile", files);
                firewaterbll.SubmitCondition("", conditionModel);
                return new { code = 0, data = "", info = "操作成功" };
            }
            catch (Exception ex)
            {
                return new { code = -1, data = "", info = ex.Message };
            }
        }
        /// <summary>
        /// 消防水使用申请信息添加或修改
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SaveForm()
        {
            try
            {
                string res = HttpContext.Current.Request["json"];
                var dy = JsonConvert.DeserializeAnonymousType(res, new
                {
                    userid = string.Empty,
                    data = new FireWaterModel()
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
                var model = dy.data;
                if (string.IsNullOrEmpty(model.Id))
                {
                    model.Id = Guid.NewGuid().ToString();
                }
                //如果有删除的文件，则进行删除
                if (!string.IsNullOrEmpty(model.DeleteFileIds))
                {
                    DeleteFile(model.DeleteFileIds);
                }
                //再重新上传
                HttpFileCollection files = HttpContext.Current.Request.Files;
                UploadifyFile(model.Id, "scaffoldfile", files);
                highriskrecordbll.RemoveFormByWorkId(model.Id);
                if (model.RiskRecord != null)
                {
                    var num = 0;
                    foreach (var item in model.RiskRecord)
                    {
                        item.CreateDate = DateTime.Now.AddSeconds(-num);
                        item.WorkId = model.Id;
                        highriskrecordbll.SaveForm("", item);
                        num++;
                    }
                }
                firewaterbll.SaveForm(model.Id, model);

                return new { code = 0, data = "", info = "操作成功" };

            }
            catch (Exception ex)
            {
                return new { code = -1, data = "", info = ex.Message };
            }
        }

        #region 图片上传
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
        #endregion

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}