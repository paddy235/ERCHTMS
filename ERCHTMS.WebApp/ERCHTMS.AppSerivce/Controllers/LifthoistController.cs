using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.HighRiskWork;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Entity.PublicInfoManage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ERCHTMS.Busines.RiskDatabase;

namespace ERCHTMS.AppSerivce.Controllers
{
    public class LifthoistController : BaseApiController
    {
        private LifthoistjobBLL lifthoistjobbll = new LifthoistjobBLL();
        private LifthoistcertBLL lifthoistcertbll = new LifthoistcertBLL();
        private LifthoistauditrecordBLL lifthoistauditrecordbll = new LifthoistauditrecordBLL();
        private LifthoistsafetyBLL lifthoistsafetybll = new LifthoistsafetyBLL();
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private HighRiskRecordBLL highriskrecordbll = new HighRiskRecordBLL();
        private LifthoistpersonBLL lifthoistpersonbll = new LifthoistpersonBLL();

        /// <summary>
        /// 列表
        /// </summary>
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
                    pagesize = 8,
                    pageindex = 1,
                    data = new LifthoistSearchModel()
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
                pagination.conditionJson = " 1=1 ";
                pagination.page = dy.pageindex;
                pagination.rows = dy.pagesize;
                pagination.sidx = "a.createdate";     //排序字段
                pagination.sord = "desc";             //排序方式
                #region 数据权限
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (!user.IsSystem)
                {
                    //根据当前用户对模块的权限获取记录
                    string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), "de9dd23d-6951-4f0d-bcd7-563740dfeafb", "a.createuserdeptcode", "a.createuserorgcode");
                    if (!string.IsNullOrEmpty(where))
                    {
                        pagination.conditionJson += " and " + where;
                    }
                }
                #endregion
                //查凭吊证
                DataTable dt = null;
                string[] props = new string[] { "applyuserid", "applyusername", "applycompanyname", "applydate", "applycode", "flowid", "flowname", "flowroleid", "flowrolename", "flowdeptid", "flowdeptname" };
                if (dy.data.pagetype == "1")
                {
                    dt = lifthoistcertbll.GetList(pagination, dy.data);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (string columnname in props)
                        {
                            dt.Columns.Remove(columnname);
                        }
                    }
                }
                else
                {
                    dt = lifthoistjobbll.GetList(pagination, dy.data);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (string columnname in props)
                        {
                            dt.Columns.Remove(columnname);
                        }
                    }
                }
                Dictionary<string, string> dict_props = new Dictionary<string, string>();
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                    DateFormatString = "yyyy-MM-dd HH:mm", //格式化日期
                };
                return new { code = 0, info = string.Empty, count = pagination.records, data = JArray.Parse(JsonConvert.SerializeObject(dt, Formatting.Indented, settings)) };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message };
            }
        }


        /// <summary>
        /// 详情
        /// </summary>
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
                    data = new LifthoistViewModel()
                });
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, info = "请求失败,请登录!" };
                }
                if (dy.data == null)
                {
                    return new { code = -1, info = "请求失败,data参数为空!" };
                }
                LifthoistViewModel retData = new LifthoistViewModel();
                string[] pops = null;
                if (dy.data.pagetype == "1")
                {
                    #region 凭吊证详情处理
                    //需要排的除段
                    pops = new string[] { "deletefileids", "qualitytype", "guardianid", "guardianname", "hoistcontent", "liftschemes" };
                    LifthoistcertEntity certEntity = lifthoistcertbll.GetEntity(dy.data.id);
                    if (certEntity == null)
                    {
                        throw new ArgumentException("无法找到当前业务信息，请检查参数是否有误！");
                    }
                    retData = JsonConvert.DeserializeObject<LifthoistViewModel>(JsonConvert.SerializeObject(certEntity));
                    //取人员资料
                    DataTable persondatas = fileInfoBLL.GetFiles(certEntity.ID + "1");
                    if (persondatas != null && persondatas.Rows.Count > 0)
                    {
                        retData.persondatas = new List<Photo>();
                        foreach (DataRow item in persondatas.Rows)
                        {
                            Photo p = new Photo();
                            p.fileid = item[0].ToString();
                            p.filename = item[1].ToString();
                            p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + item[2].ToString().Substring(1);
                            retData.persondatas.Add(p);
                        }
                    }
                    //取设备资料
                    DataTable driverdatas = fileInfoBLL.GetFiles(certEntity.ID + "2");
                    if (driverdatas != null && driverdatas.Rows.Count > 0)
                    {
                        retData.driverdatas = new List<Photo>();
                        foreach (DataRow item in driverdatas.Rows)
                        {
                            Photo p = new Photo();
                            p.fileid = item[0].ToString();
                            p.filename = item[1].ToString();
                            p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + item[2].ToString().Substring(1);
                            retData.driverdatas.Add(p);
                        }
                    }
                    string[] signs = null;
                    string urls = string.Empty;
                    //处理签字
                    if (!string.IsNullOrEmpty(retData.chargepersonsign))
                    {
                        signs = retData.chargepersonsign.Split(',');
                        urls = string.Empty;
                        foreach (var s in signs)
                        {
                            if (!string.IsNullOrEmpty(s))
                            {
                                if (s.Contains("http://"))
                                {
                                    urls += "," + s;
                                }
                                else
                                {
                                    urls += "," + dataitemdetailbll.GetItemValue("imgUrl") + s.Substring(1);
                                }

                            }
                        }
                        if (!string.IsNullOrEmpty(urls))
                        {
                            retData.chargepersonsign = urls.Substring(1);
                        }
                    }
                    if (!string.IsNullOrEmpty(retData.hoistareapersonsigns))
                    {
                        signs = retData.hoistareapersonsigns.Split(',');
                        urls = string.Empty;
                        foreach (var s in signs)
                        {
                            if (!string.IsNullOrEmpty(s))
                            {
                                if (s.Contains("http://"))
                                {
                                    urls += "," + s;
                                }
                                else
                                {
                                    urls += "," + dataitemdetailbll.GetItemValue("imgUrl") + s.Substring(1);
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(urls))
                        {
                            retData.hoistareapersonsigns = urls.Substring(1);
                        }
                    }
                    //安全措施
                    var safetys = lifthoistsafetybll.GetList(string.Format("LIFTHOISTCERTID = '{0}'", certEntity.ID));
                    if (safetys != null)
                    {
                        retData.safetys = safetys.ToList();
                    }
                    #endregion
                }
                else
                {
                    #region 起重吊装作业详情处理
                    //需要排的除段
                    pops = new string[] { "lifthoistjobid", "deletefileids", "drivername", "drivernumber", "fulltimename", "fulltimenumber", "persondatas", "driverdatas", "safetys", "chargepersonsign", "hoistareapersonnames", "hoistareapersonids", "hoistareapersonsigns" };
                    LifthoistjobEntity jobEntity = lifthoistjobbll.GetEntity(dy.data.id);
                    if (jobEntity == null)
                    {
                        throw new ArgumentException("无法找到当前业务信息，请检查参数是否有误！");
                    }
                    retData = JsonConvert.DeserializeObject<LifthoistViewModel>(JsonConvert.SerializeObject(jobEntity));
                    //取起吊方案
                    DataTable liftschemes = fileInfoBLL.GetFiles(jobEntity.ID);
                    if (liftschemes != null && liftschemes.Rows.Count > 0)
                    {
                        retData.liftschemes = new List<Photo>();
                        foreach (DataRow item in liftschemes.Rows)
                        {
                            Photo p = new Photo();
                            p.fileid = item[0].ToString();
                            p.filename = item[1].ToString();
                            p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + item[2].ToString().Substring(1);
                            retData.liftschemes.Add(p);
                        }
                    }

                    DataTable liftfazls= fileInfoBLL.GetFiles(jobEntity.FAZLFILES);
                    if (liftfazls != null && liftfazls.Rows.Count > 0)
                    {
                        retData.liftfazls = new List<Photo>();
                        foreach (DataRow item in liftfazls.Rows)
                        {
                            Photo p = new Photo();
                            p.fileid = item[0].ToString();
                            p.filename = item[1].ToString();
                            p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + item[2].ToString().Substring(1);
                            retData.liftfazls.Add(p);
                        }
                    }
                    #endregion

                    #region 流程
                    string modulename = string.Empty;
                    if (jobEntity.QUALITYTYPE == "0")
                    {
                        modulename = "(起重吊装作业30T以下)审核";
                    }
                    else
                    {
                        modulename = "(起重吊装作业30T以上)审核";
                    }
                    var nodelist = lifthoistjobbll.GetAppFlowList(jobEntity.ID, modulename);
                    retData.CheckFlow = nodelist;
                    #endregion
                }
                //审核记录
                var records = lifthoistauditrecordbll.GetList(dy.data.id);
                if (records != null)
                {
                    string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                    foreach (var item in records)
                    {
                        item.AUDITSIGNIMG = item.AUDITSIGNIMG.Replace("../../", webUrl + "/");
                    }
                    retData.auditrecords = records.ToList();
                }   
                //作业安全风险分析
                var riskrecord = highriskrecordbll.GetList(dy.data.id).ToList();
                if (riskrecord != null)
                {
                    retData.riskrecord = riskrecord.ToList();
                }
                //起重吊装人员信息
                var lifthoistperson = lifthoistpersonbll.GetRelateList(dy.data.id).ToList();
                if (lifthoistperson != null)
                {
                    foreach (var item in lifthoistperson)
                    {
                        DataTable liftfazls = fileInfoBLL.GetFiles(item.Id);
                        if (liftfazls != null && liftfazls.Rows.Count > 0)
                        {
                            List<Photo> files = new List<Photo>();
                            foreach (DataRow rowitem in liftfazls.Rows)
                            {
                                Photo p = new Photo();
                                p.fileid = rowitem[0].ToString();
                                p.filename = rowitem[1].ToString();
                                p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + rowitem[2].ToString().Substring(1);
                                files.Add(p);
                            }
                            item.lifthoistpersonfile = files;
                        }
                        //var liftfazls = fileInfoBLL.GetFileList(item.Id);
                        //foreach (var file in liftfazls)
                        //{
                        //    file.FilePath = dataitemdetailbll.GetItemValue("imgUrl") + file.FilePath.Substring(1);
                        //}
                        
                    }
                    retData.lifthoistperson = lifthoistperson.ToList();
                }
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver(null, pops, false),
                    DateFormatString = "yyyy-MM-dd HH:mm", //格式化日期
                };
                string retDataJson = JsonConvert.SerializeObject(retData, Formatting.Indented, settings);
                return new { code = 0, info = string.Empty, data = JObject.Parse(retDataJson) };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message };
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object SaveForm()
        {
            try
            {
                string res = HttpContext.Current.Request["json"];
                HttpFileCollection files = HttpContext.Current.Request.Files;
                var dy = JsonConvert.DeserializeAnonymousType(res, new
                {
                    userid = string.Empty,
                    data = new LifthoistViewModel()
                });
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, info = "请求失败,请登录!" };
                }
                if (dy.data == null)
                {
                    return new { code = -1, info = "请求失败,data参数为空!" };
                }
                //如果ID为空，则重新生成一个
                if (string.IsNullOrEmpty(dy.data.id))
                {
                    dy.data.id = Guid.NewGuid().ToString();
                    dy.data.fazlfiles = Guid.NewGuid().ToString();
                }
                else
                {
                    dy.data.fazlfiles = lifthoistjobbll.GetEntity(dy.data.id) == null ? Guid.NewGuid().ToString() : lifthoistjobbll.GetEntity(dy.data.id).FAZLFILES;//方案资料附件关联ID
                }
                //删除文件 
                if (!string.IsNullOrEmpty(dy.data.deletefileids))
                {
                    DeleteFile(dy.data.deletefileids);
                }
                if (dy.data.pagetype == "1")
                {
                    //起吊证
                    LifthoistcertEntity certEntity = JsonConvert.DeserializeObject<LifthoistcertEntity>(JsonConvert.SerializeObject(dy.data));
                    lifthoistcertbll.SaveForm(certEntity.ID, certEntity);
                    //上传人员资料、设备资料
                    UploadifyFile(certEntity.ID + "1", "persondata", files);
                    UploadifyFile(certEntity.ID + "2", "driverdata", files);
                }
                else
                {
                    //起重吊装作业
                    LifthoistjobEntity jobEntity = JsonConvert.DeserializeObject<LifthoistjobEntity>(JsonConvert.SerializeObject(dy.data));
                    lifthoistjobbll.SaveForm(jobEntity.ID, jobEntity);
                    //上传起吊方案
                    UploadifyFile(jobEntity.ID, "liftscheme", files);
                    UploadifyFile(jobEntity.FAZLFILES, "liftfazl", files);
                    highriskrecordbll.RemoveFormByWorkId(dy.data.id);
                    if (dy.data.riskrecord != null)
                    {
                        var num = 0;
                        foreach (var item in dy.data.riskrecord)
                        {
                            item.CreateDate = DateTime.Now.AddSeconds(-num);
                            item.WorkId = dy.data.id;
                            highriskrecordbll.SaveForm("", item);
                            num++;
                        }
                    }
                    lifthoistpersonbll.RemoveFormByWorkId(dy.data.id);
                    if (dy.data.lifthoistperson != null)
                    {
                        var num = 0;
                        for (int i = 0; i < dy.data.lifthoistperson.Count; i++)
                        {
                            var item = dy.data.lifthoistperson[i];
                            item.CreateDate = DateTime.Now.AddSeconds(-num);
                            item.RecId = dy.data.id;
                            item.Id = Guid.NewGuid().ToString();
                            lifthoistpersonbll.SaveForm(item.Id, item);
                            UploadifyFile(item.Id, "pic_" + i, files);//上传人员信息附件
                            if (item.lifthoistpersonfile != null)
                            {
                                foreach (var personfile in item.lifthoistpersonfile)
                                {
                                    var fileinfo = fileInfoBLL.GetEntity(personfile.fileid);
                                    fileinfo.RecId = item.Id;
                                    fileInfoBLL.SaveForm(personfile.fileid, fileinfo);
                                }
                            }
                            num++;
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


        /// <summary>
        /// 审核
        /// </summary>
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
                        pagetype = string.Empty,
                        auditEntity = new LifthoistauditrecordEntity(),
                        entity = new LifthoistcertEntity()
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
                if (dy.data == null)
                {
                    return new { code = -1, info = "请求失败,data参数为空!" };
                }
                LifthoistauditrecordEntity auditEntity = JsonConvert.DeserializeObject<LifthoistauditrecordEntity>(JsonConvert.SerializeObject(dy.data.auditEntity));
                if (dy.data.pagetype == "1")
                {
                    LifthoistcertEntity certEntity = null;
                    if (dy.data.entity != null)
                    {
                        certEntity = JsonConvert.DeserializeObject<LifthoistcertEntity>(JsonConvert.SerializeObject(dy.data.entity));
                        //负责人签字
                        certEntity.CHARGEPERSONSIGN = UploadifyFile(certEntity.ID + "3", "chargerersonsign", HttpContext.Current.Request.Files);
                        //吊装区域内人员签字
                        certEntity.HOISTAREAPERSONSIGNS = UploadifyFile(certEntity.ID + "4", "hoistareapersonsigns", HttpContext.Current.Request.Files);
                    }
                    lifthoistcertbll.ApplyCheck(dy.data.id, certEntity, auditEntity);
                }
                else
                {
                    if (!string.IsNullOrEmpty(auditEntity.AUDITSIGNIMG))
                    {
                        string strurl = new DataItemDetailBLL().GetItemValue("imgUrl");
                        auditEntity.AUDITSIGNIMG = auditEntity.AUDITSIGNIMG.Replace(strurl, "../../");
                    }
                    lifthoistjobbll.ApplyCheck(dy.data.id, auditEntity);
                }

                return new { code = 0, info = "操作成功" };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message };
            }
        }

        #region 图片上传
        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="Filedata"></param>
        public string UploadifyFile(string folderId, string filekey, HttpFileCollection fileList)
        {
            string filePaths = string.Empty;
            try
            {
                if (fileList.Count > 0)
                {
                    for (int i = 0; i < fileList.AllKeys.Length; i++)
                    {
                        if (fileList.AllKeys[i].StartsWith(filekey))
                        {
                            HttpPostedFile file = fileList[i];
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

                                filePaths += "," + virtualPath;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(filePaths))
                        return filePaths.Substring(1);
                }
            }
            catch (Exception ex)
            {
            }
            return filePaths;
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
    }
}
