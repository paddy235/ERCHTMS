using BSFramework.Util.Attributes;
using BSFramework.Util.WebControl;
using ERCHTMS.AppSerivce.Model;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.FireManage;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.FireManage;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Entity.SystemManage;
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
    public class SafetyActivityController : BaseApiController
    {
        public HttpContext ctx { get { return HttpContext.Current; } }
        private SafetyActivityBLL SafetyActivitybll = new SafetyActivityBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
        private DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();

        private OutsouringengineerBLL outsouringengineerbll = new OutsouringengineerBLL();
        private OutsourcingprojectBLL outProjectbll = new OutsourcingprojectBLL();
        private DepartmentBLL departmentbll = new DepartmentBLL();
        /// <summary>
        /// 获取安全活动列表
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
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }

                //获取页数和条数
                int page = Convert.ToInt32(dy.data.pageNum), rows = Convert.ToInt32(dy.data.pageSize);
                Pagination pagination = new Pagination();
                pagination.p_kid = "t.id";
                pagination.p_fields = "t.EngineerId,to_char(t.StartTime,'yyyy-MM-dd HH:mm') as StartTime,to_char(t.EndTime,'yyyy-MM-dd HH:mm') as EndTime,t.PeopleNum,t.PracticalPeopleNum,t.Condition,t.StudyRecord,t.StudyDetails,t.CREATEUSERID,t.CREATEUSERDEPTCODE,t.CREATEUSERORGCODE,t.CREATEDATE,t.CREATEUSERNAME,o.engineerletdept,o.engineername,p.outsourcingname,(select o.filepath from base_fileinfo o where o.recid=t.id and rownum<2) as fileurl";
                pagination.p_tablename = "EPG_SAFETYACTIVITY t left join EPG_OUTSOURINGENGINEER o on t.engineerid=o.id left join EPG_OUTSOURCINGPROJECT p on o.outprojectid=p.outprojectid";
                pagination.conditionJson = string.Format(" t.CREATEUSERORGCODE ='{0}'", curUser.OrganizeCode);
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "t.createdate";//排序字段
                pagination.sord = "desc";//排序方式  

                string OutprojectName = dy.data.OutprojectName;
                //承包单位
                if (!string.IsNullOrEmpty(OutprojectName))
                {
                    pagination.conditionJson += string.Format(" and p.outsourcingname like '%{0}%'", OutprojectName);
                }
                string EngineerName = dy.data.EngineerName;
                //外包工程名称
                if (!string.IsNullOrEmpty(EngineerName))
                {
                    pagination.conditionJson += string.Format(" and o.engineername like '%{0}%'", EngineerName);
                }
                string StartTime = dy.data.StartTime;
                //开始时间
                if (!string.IsNullOrEmpty(StartTime))
                {
                    pagination.conditionJson += string.Format(@" and t.StartTime >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", StartTime);
                }
                string EndTime = dy.data.EndTime;
                //结束时间
                if (!string.IsNullOrEmpty(EndTime))
                {
                    pagination.conditionJson += string.Format(@" and t.EndTime < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(EndTime).AddDays(1).ToString("yyyy-MM-dd"));
                }
                DataTable dt = SafetyActivitybll.GetPageList(pagination, null);
                string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                foreach (DataRow dr in dt.Rows)
                {
                    dr["fileurl"] = dr["fileurl"].ToString().Replace("~/", webUrl + "/");
                }
                return new { code = 0, info = "获取数据成功", count = pagination.records, data = dt };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }
        /// <summary>
        /// 新增/编辑维保记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object Save()
        {
            try
            {
                string res = ctx.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid; //当前用户ID 
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                string keyValue = dy.data.ID;
                string str = JsonConvert.SerializeObject(dy.data);
                SafetyActivityEntity entity = JsonConvert.DeserializeObject<SafetyActivityEntity>(str);

                if (!string.IsNullOrEmpty(keyValue))
                {
                    //获取删除附件ID
                    string deleteFileId = dy.data.deleteFileId;
                    if (!string.IsNullOrEmpty(deleteFileId))
                    {
                        DeleteFile(deleteFileId);
                    }
                }
                else
                {
                    entity.ID = Guid.NewGuid().ToString();
                    keyValue = entity.ID;
                }
                HttpFileCollection files = ctx.Request.Files;
                UploadifyFile(entity.ID + "01", "SafetyActivityFile", files);
                UploadifyFile(entity.ID, "SafetyActivityPic", files);
                SafetyActivitybll.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                return new { code = -1, count = 0, info = "保存失败" };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }
        #region 删除图片
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="recId">各图片Id  xxxxx,xxxxxx,xxxxxxxx</param>
        /// <param name="folderId">关联ID</param>
        /// <returns></returns>
        public bool DeleteFile(string recId)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(recId))
            {
                string ids = "";

                string[] strArray = recId.Split(',');

                foreach (string s in strArray)
                {
                    ids += "'" + s + "',";
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

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetEntity([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string id = dy.data.ID;//ID
                SafetyActivityEntity entity = SafetyActivitybll.GetEntity(id);
                var data = outsouringengineerbll.GetEntity(entity.EngineerId);
                var proData = outProjectbll.GetOutProjectInfo(data.OUTPROJECTID);
                dynamic obj = new ExpandoObject();
                obj.Id = entity.ID;
                obj.CREATEUSERID = entity.CREATEUSERID;
                obj.CREATEUSERDEPTCODE = entity.CREATEUSERDEPTCODE;
                obj.CREATEUSERORGCODE = entity.CREATEUSERORGCODE;
                obj.CREATEDATE = entity.CREATEDATE.Value.ToString("yyyy-MM-dd HH:mm");
                obj.CREATEUSERNAME = entity.CREATEUSERNAME;
                obj.MODIFYDATE = entity.MODIFYDATE;
                obj.MODIFYUSERID = entity.MODIFYUSERID;
                obj.MODIFYUSERNAME = entity.MODIFYUSERNAME;
                obj.EngineerId = entity.EngineerId;
                obj.StartTime = entity.StartTime.Value.ToString("yyyy-MM-dd HH:mm");
                obj.EndTime = entity.EndTime.Value.ToString("yyyy-MM-dd HH:mm");
                obj.PeopleNum = entity.PeopleNum;
                obj.PracticalPeopleNum = entity.PracticalPeopleNum;
                obj.Condition = entity.Condition;
                obj.StudyRecord = entity.StudyRecord;
                obj.StudyDetails = entity.StudyDetails;
                obj.EngineerletDept = data.ENGINEERLETDEPT;
                obj.OutsourcingName = proData.OUTSOURCINGNAME;
                obj.OutprojectId = data.OUTPROJECTID;
                obj.UnitSuper = data.UnitSuper;
                obj.EngineerName = data.ENGINEERNAME;


                IList<Photo> pList = new List<Photo>(); //附件
                DataTable file = fileInfoBLL.GetFiles(entity.ID + "01");
                foreach (DataRow dr in file.Rows)
                {
                    Photo p = new Photo();
                    p.id = dr["fileid"].ToString();
                    p.filename = dr["filename"].ToString();
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + dr["filepath"].ToString().Substring(1);
                    pList.Add(p);
                }
                obj.SafetyActivityFile = pList;

                IList<Photo> pList1 = new List<Photo>(); //附件
                DataTable file1 = fileInfoBLL.GetFiles(entity.ID);
                foreach (DataRow dr in file1.Rows)
                {
                    Photo p = new Photo();
                    p.id = dr["fileid"].ToString();
                    p.filename = dr["filename"].ToString();
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + dr["filepath"].ToString().Substring(1);
                    pList1.Add(p);
                }
                obj.SafetyActivityPic = pList1;

                return new { Code = 0, Count = 1, Info = "获取数据成功", data = obj };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }


        /// <summary>
        /// 获取数据字典
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetDataItemListJson([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string EnCode = dy.data.EnCode;//ID
                var data = dataItemDetailBLL.GetDataItemListByItemCode("'" + EnCode + "'");


                return new { Code = 0, Count = 1, Info = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
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
        /// 获取安全活动列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetEngineerSelect([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }

                //获取页数和条数
                //int page = Convert.ToInt32(dy.data.pageNum), rows = Convert.ToInt32(dy.data.pageSize);
                Pagination pagination = new Pagination();
                pagination.p_kid = "e.id";
                pagination.p_fields = "e.engineerletdept,e.engineername,p.outsourcingname,e.unitsuper,'' as examstatus,'' as pactstatus,'' as compactstatus,'' as technicalstatus,'' as equipmenttoolstatus,'' as peoplestatus,'' as threetwostatus,e.engineerstate";
                pagination.p_tablename = "EPG_OUTSOURINGENGINEER e left join EPG_OUTSOURCINGPROJECT p on e.outprojectid = p.outprojectid";
                pagination.conditionJson = string.Format(" e.CREATEUSERORGCODE ='{0}'", curUser.OrganizeCode);
                pagination.page = 1;//页数
                pagination.rows = 999999999;//行数
                pagination.sidx = "e.createdate";//排序字段
                pagination.sord = "desc";//排序方式  
                Operator currUser = OperatorProvider.Provider.Current();
                if (currUser.IsSystem)
                {
                    pagination.conditionJson = "  1=1 ";
                }
                else if (currUser.RoleName.Contains("省级"))
                {
                    pagination.conditionJson = string.Format(@" e.createuserorgcode  in (select encode
                        from base_department d
                        where d.deptcode like '{0}%' and d.nature = '厂级' and d.description is null)", currUser.NewDeptCode);
                }
                else if (currUser.RoleName.Contains("厂级部门用户") || currUser.RoleName.Contains("公司级用户"))
                {
                    pagination.conditionJson = string.Format(" e.createuserorgcode like'%{0}%' ", currUser.OrganizeCode);
                }
                else if (currUser.RoleName.Contains("承包商级用户"))
                {
                    pagination.conditionJson = string.Format("  (e.outprojectid ='{0}' or e.supervisorid='{0}')", currUser.DeptId);
                }
                else
                {
                    var deptentity = departmentbll.GetEntity(currUser.DeptId);
                    while (deptentity.Nature == "班组" || deptentity.Nature == "专业")
                    {
                        deptentity = departmentbll.GetEntity(deptentity.ParentId);
                    }
                    pagination.conditionJson = string.Format(" e.engineerletdeptid in (select departmentid from base_department where encode like '{0}%') ", deptentity.EnCode, currUser.UserId);

                    //pagination.conditionJson = string.Format("  e.engineerletdeptid ='{0}'", currUser.DeptId);
                }
                pagination.conditionJson += string.Format(" and e.engineerstate in('003','002') ");
                DataTable dt = outsouringengineerbll.GetPageList(pagination, null);

                return new { code = 0, info = "获取数据成功", count = pagination.records, data = dt };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }

    }
}