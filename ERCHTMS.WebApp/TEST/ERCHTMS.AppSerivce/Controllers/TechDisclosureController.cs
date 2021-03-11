using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.OutsourcingProject;
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
    /// 安全技术交底
    /// </summary>
    public class TechDisclosureController : BaseApiController
    {
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private TechDisclosureBLL techdisclosurebll = new TechDisclosureBLL();

        /// <summary>
        /// 获取外包工程列表
        /// </summary>
        /// <param name="json">mode=005</param>
        /// <returns></returns>
        [HttpPost]
        public object GetProjectData([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");

                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string mode = res.Contains("mode") ? dy.data.mode : "";

                DataTable dt = new OutsouringengineerBLL().GetEngineerDataByCurrdeptId(curUser, mode);


                return new { code = 0, count = 0, info = "操作成功", data = dt };
            }
            catch (Exception ex)
            {

                return new { code = -1, count = 0, info = ex.Message };
            }
        }

        /// <summary>
        /// 获取外包工程列表--隐患使用
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetProjectList([FromBody]JObject json) {
            try
            {
                string res = json.Value<string>("json");

                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string orgid = res.Contains("orgid") ? dy.data.orgid : "";

                DataTable dt = new OutsouringengineerBLL().GetEngineerDataByCurrdeptId(curUser, "", orgid);


                return new { code = 0, count = 0, info = "操作成功", data = dt };
            }
            catch (Exception ex)
            {

                return new { code = -1, count = 0, info = ex.Message };
            }
        }
        /// <summary>
        /// 列表
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
                long pageIndex = dy.data.pageindex;
                long pageSize = dy.data.pagesize;
                string starttime = dy.data.starttime;
                string endtime = dy.data.endtime;
                string deptid = dy.data.deptid;
                string unitid = dy.data.unitid;
                string engineerid = dy.data.engineerid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator currUser = OperatorProvider.Provider.Current();
                Pagination pagination = new Pagination();
                pagination.page = int.Parse(pageIndex.ToString());
                pagination.rows = int.Parse(pageSize.ToString());
                pagination.p_kid = "t.ID";
                pagination.p_fields = @" r.id as engineerid,t.disclosurename,t.createusername,to_char(t.createdate, 'yyyy-MM-dd') as createdate,
                                           e.fullname,
                                           r.engineername,
                                           t.disclosuretype,
                                           to_char(t.disclosuredate, 'yyyy-mm-dd') as disclosuredate,
                                           t.disclosurepersonnum,
                                           t.disclosureplace,
                                           t.disclosureperson,
                                           t.receiveperson,
                                           t.createuserid ";
                pagination.p_tablename = @" epg_techdisclosure t
                                          left join epg_outsouringengineer r
                                            on t.projectid = r.id
                                          left join base_department e
                                            on r.outprojectid = e.departmentid ";
                pagination.sidx = "t.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                pagination.conditionJson = "1=1";
                string role = currUser.RoleName;
                if (role.Contains("公司级用户") || role.Contains("厂级部门用户"))
                {
                    pagination.conditionJson = string.Format(" t.CREATEUSERORGCODE  = '{0}'", currUser.OrganizeCode);
                }
                else
                if (role.Contains("承包商级用户"))
                {
                    pagination.conditionJson = string.Format(" e.departmentid = '{0}'", currUser.DeptId);
                }
                else
                {
                    pagination.conditionJson = string.Format(" e.senddeptid = '{0}'", currUser.DeptId);
                }
                string queryJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    sTime = starttime,
                    eTime = endtime,
                    outengineerid = engineerid,
                    unitid = unitid,
                    deptid = deptid
                }); 
                var data = techdisclosurebll.GetList(pagination, queryJson);
                return new { code = 0, count = pagination.records, info = "获取数据成功", data = data };

            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }
       
        /// <summary>
        /// 获取详情
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
                string disid = dy.data.disid;
                var disBll = new DistrictBLL();
                var didBll = new DataItemDetailBLL();
                var deptBll = new DepartmentBLL();
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                var entity = techdisclosurebll.GetEntity(disid);
                var project = new OutsouringengineerBLL().GetEntity(entity.PROJECTID);
                if (!string.IsNullOrWhiteSpace(project.OUTPROJECTID))
                {
                    var dept = new DepartmentBLL().GetEntity(project.OUTPROJECTID);
                    project.OUTPROJECTCODE = dept.EnCode;
                    project.OUTPROJECTNAME = dept.FullName;
                }
                if (!string.IsNullOrWhiteSpace(project.ENGINEERAREA))
                {
                    var area = disBll.GetEntity(project.ENGINEERAREA);
                    if (area != null)
                        project.ENGINEERAREANAME = area.DistrictName;
                }
                if (!string.IsNullOrWhiteSpace(project.ENGINEERTYPE))
                {
                    var listType = didBll.GetDataItem("ProjectType", project.ENGINEERTYPE).ToList();
                    if (listType != null && listType.Count > 0)
                        project.ENGINEERTYPENAME = listType[0].ItemName;
                }
                if (!string.IsNullOrWhiteSpace(project.ENGINEERLEVEL))
                {
                    var listLevel = didBll.GetDataItem("ProjectLevel", project.ENGINEERLEVEL).ToList();
                    if (listLevel != null && listLevel.Count > 0)
                        project.ENGINEERLEVELNAME = listLevel[0].ItemName;
                }
                var files = new FileInfoBLL().GetFiles(disid);//获取相关附件
                var pics = new FileInfoBLL().GetFiles(disid+"01");//获取相关附件
                string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                foreach (DataRow dr in files.Rows)
                {
                    dr["filepath"] = dr["filepath"].ToString().Replace("~/", webUrl + "/");
                }
                foreach (DataRow dr in pics.Rows)
                {
                    dr["filepath"] = dr["filepath"].ToString().Replace("~/", webUrl + "/");
                }
                
               var data = new
                {
                    project= project,
                    entity = entity,
                    piclist=pics,
                    filelist = files
                };
                Dictionary<string, string> dict_props = new Dictionary<string, string>();
                //Id 转换前的列名  keyvalue 转换后的列名
                //dict_props.Add("Id", "keyvalue");

                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ContractResolver = new LowercaseContractResolver(dict_props), //转小写，并对指定的列进行自定义名进行更换
                    DateFormatString = "yyyy-MM-dd HH:mm", //格式化日期
                    //NullValueHandling = NullValueHandling.Ignore 值为空则在JSON中体现
                };
                return new { code = 0, info = "获取数据成功",count = 1, data = JObject.Parse(JsonConvert.SerializeObject(data, Formatting.None, settings)) };
            }
            catch (Exception ex)
            {

                return new { code = -1, count = 0, info = ex.Message };
            }
           

        }
        /// <summary>
        /// 保存/提交
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SaveCommit() {
            try
            {
                string res = HttpContext.Current.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                //string deleteids = dy.deleteids;//删除附件id集合
                string safetyentity = JsonConvert.SerializeObject(dy.data.entity);
                TechDisclosureEntity change = JsonConvert.DeserializeObject<TechDisclosureEntity>(safetyentity);
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator currUser = OperatorProvider.Provider.Current();
                techdisclosurebll.SaveForm(change.ID, change);
                //if (!string.IsNullOrEmpty(deleteids))
                //{
                //    DeleteFile(deleteids);
                //}
                HttpFileCollection files = HttpContext.Current.Request.Files;
                UploadifyFile(change.ID, "fj", files);//上传附件
                UploadifyFile(change.ID + "01", "pic", files);//上传照片
                return new { code = 0, count = 1, info = "提交成功" };
            }
            catch (Exception ex)
            {

                return new { code = -1, count = 0, info = ex.Message };
            }
        }
        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="foldername"></param>
        /// <param name="fileList"></param>
        public void UploadifyFile(string folderId, string foldername, HttpFileCollection fileList)
        {
            try
            {
                if (fileList.Count > 0)
                {
                    foreach (string key in fileList.AllKeys)
                    {
                        if (key.Contains(foldername))
                        {
                            HttpPostedFile file = fileList[key];
                            string userId = OperatorProvider.Provider.Current().UserId;
                            string fileGuid = Guid.NewGuid().ToString();
                            long filesize = file.ContentLength;
                            string FileEextension = Path.GetExtension(file.FileName);
                            string uploadDate = DateTime.Now.ToString("yyyyMMdd");
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
                                fileInfoEntity.RecId = folderId; //关联ID
                                fileInfoEntity.FileName = file.FileName;
                                fileInfoEntity.FilePath = "~/Resource/Upfile/" + newFileName;
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