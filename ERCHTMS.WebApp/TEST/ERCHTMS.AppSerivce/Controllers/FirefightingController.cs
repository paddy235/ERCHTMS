using BSFramework.Util.WebControl;
using ERCHTMS.AppSerivce.Model;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.FireManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.FireManage;
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
    public class FirefightingController : BaseApiController
    {
        public HttpContext ctx { get { return HttpContext.Current; } }
        private FirefightingBLL firefightingbll = new FirefightingBLL();
        private ExamineRecordBLL examinerecordbll = new ExamineRecordBLL();
        private DetectionRecordBLL detectionrecordbll = new DetectionRecordBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
        private DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
        /// <summary>
        /// 获取消防设施列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetFirefightingList([FromBody]JObject json)
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
                pagination.p_kid = "ID";
                pagination.p_fields = "EquipmentName,EquipmentCode,ExtinguisherType,ExtinguisherTypeNo,Specifications,District,DutyUser,DutyDept,NextExamineDate";
                pagination.p_tablename = "HRS_FIREFIGHTING";
                pagination.conditionJson = string.Format(" CREATEUSERORGCODE ='{0}'", curUser.OrganizeCode);
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "nextexaminedate asc,createdate";//排序字段
                pagination.sord = "desc";//排序方式  
                //查询条件 名称
                string EquipmentNameNo = dy.data.EquipmentNameNo;
                if (!string.IsNullOrEmpty(EquipmentNameNo))
                {
                    pagination.conditionJson += string.Format(" and EquipmentNameNo='{0}'", EquipmentNameNo);
                }
                //查询条件 类型
                string ExtinguisherTypeNo = dy.data.ExtinguisherTypeNo;
                if (!string.IsNullOrEmpty(ExtinguisherTypeNo))
                {
                    pagination.conditionJson += string.Format(" and ExtinguisherTypeNo='{0}'", ExtinguisherTypeNo);
                }
                //部门
                string DutyDeptCode = dy.data.DutyDeptCode;
                if (!string.IsNullOrEmpty(DutyDeptCode))
                {
                    pagination.conditionJson += string.Format(" and DutyDeptCode like '{0}%'", DutyDeptCode);
                }
                DataTable dt = firefightingbll.GetPageList(pagination, null);
                //var JsonData = new
                //{
                //    rows = dt,
                //    total = pagination.total,
                //    page = pagination.page,
                //    records = pagination.records,
                //};
                return new { code = 0, info = "获取数据成功", count = pagination.records, data = dt };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }
        /// <summary>
        /// 获取检查记录（分页）
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetExamineRecordList([FromBody]JObject json)
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
                pagination.p_kid = "ID";
                pagination.p_fields = "EquipmentId,BarrelOrBox,StressOrValve,EffuserOrSpearhead,SealOrWater,Sanitation,Weight,ExaminePerson,ExamineDate,remark,Describe,Verdict";
                pagination.p_tablename = "HRS_EXAMINERECORD";
                pagination.conditionJson = string.Format(" CREATEUSERORGCODE ='{0}'", curUser.OrganizeCode);
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "createdate";//排序字段
                pagination.sord = "desc";//排序方式  
                //查询条件
                string EquipmentId = dy.data.EquipmentId;
                if (!string.IsNullOrEmpty(EquipmentId))
                {
                    pagination.conditionJson += string.Format(" and EquipmentId='{0}'", EquipmentId);
                }
                string sTime = dy.data.sTime;
                string eTime = dy.data.eTime;
                //时间范围
                if (!string.IsNullOrEmpty(sTime) || !string.IsNullOrEmpty(eTime))
                {
                    if (string.IsNullOrEmpty(sTime))
                    {
                        sTime = "1899-01-01";
                    }
                    if (string.IsNullOrEmpty(eTime))
                    {
                        eTime = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    //eTime = (Convert.ToDateTime(eTime).AddDays(1)).ToString("yyyy-MM-dd");
                    pagination.conditionJson += string.Format(" and ExamineDate between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", sTime, eTime);
                }
                DataTable dt = examinerecordbll.GetPageList(pagination, null);

                return new { code = 0, info = "获取数据成功", count = pagination.records, data = dt };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }
        /// <summary>
        /// 获取检测、维保记录（分页）
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetDetectionRecordList([FromBody]JObject json)
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
                pagination.p_kid = "id";
                pagination.p_fields = "EquipmentId,detectiondate,detectionperson,detectionpersonid,project,conclusion,content,describe,remark";
                pagination.p_tablename = "HRS_DETECTIONRECORD";
                pagination.conditionJson = "1=1";
                //pagination.conditionJson = string.Format(" d.CREATEUSERORGCODE ='{0}'", curUser.OrganizeCode);
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "createdate";//排序字段
                pagination.sord = "desc";//排序方式 

                //查询条件
                string EquipmentId = dy.data.EquipmentId;
                if (!string.IsNullOrEmpty(EquipmentId))
                {
                    pagination.conditionJson += string.Format(" and EquipmentId='{0}'", EquipmentId);
                }
                string sTime = dy.data.sTime;
                string eTime = dy.data.eTime;
                //时间范围
                if (!string.IsNullOrEmpty(sTime) || !string.IsNullOrEmpty(eTime))
                {
                    if (string.IsNullOrEmpty(sTime))
                    {
                        sTime = "1899-01-01";
                    }
                    if (string.IsNullOrEmpty(eTime))
                    {
                        eTime = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    //eTime = (Convert.ToDateTime(eTime).AddDays(1)).ToString("yyyy-MM-dd");
                    pagination.conditionJson += string.Format(" and detectiondate between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", sTime, eTime);
                }
                DataTable dt = detectionrecordbll.GetPageList(pagination, null);
                return new { code = 0, info = "获取数据成功", count = pagination.records, data = dt };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }

        /// <summary>
        /// 新增/编辑消防设施
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object SaveFirefighting()
        {
            try
            {
                string res = ctx.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid; //当前用户ID 
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                string EquipmentId = dy.data.EquipmentId;
                string keyValue = dy.data.Id;
                string str = JsonConvert.SerializeObject(dy.data);
                FirefightingEntity entity = JsonConvert.DeserializeObject<FirefightingEntity>(str);

                //HttpFileCollection files = ctx.Request.Files;//上传的文件 

                firefightingbll.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                return new { code = -1, count = 0, info = "保存失败" };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }
        /// <summary>
        /// 新增/编辑检查记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object SaveExamineRecord()
        {
            try
            {
                string res = ctx.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid; //当前用户ID 
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                string EquipmentId = dy.data.EquipmentId;
                string keyValue = dy.data.Id;
                string str = JsonConvert.SerializeObject(dy.data);
                ExamineRecordEntity entity = JsonConvert.DeserializeObject<ExamineRecordEntity>(str);

                //HttpFileCollection files = ctx.Request.Files;//上传的文件 

                examinerecordbll.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                return new { code = -1, count = 0, info = "保存失败" };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }
        /// <summary>
        /// 新增/编辑检测、维保记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object SaveDetectionRecord()
        {
            try
            {
                string res = ctx.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid; //当前用户ID 
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                string EquipmentId = dy.data.EquipmentId;
                string keyValue = dy.data.Id;
                string str = JsonConvert.SerializeObject(dy.data);
                DetectionRecordEntity entity = JsonConvert.DeserializeObject<DetectionRecordEntity>(str);

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
                    entity.Id = Guid.NewGuid().ToString();
                    keyValue = entity.Id;
                }
                 HttpFileCollection files = ctx.Request.Files;//上传的文件 
                                                              //上传设备图片
                 UploadifyFile(entity.Id, "DetectionRecord", files);

                 detectionrecordbll.SaveForm(keyValue, entity);
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
        /// 获取消防设施详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetFireEntity([FromBody]JObject json)
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
                string id = dy.data.Id;//ID
                var data = firefightingbll.GetEntity(id);
                FirefightingEntityApp firefightingEntityApp = new FirefightingEntityApp();
                if (userId == data.DutyUserId)
                {
                    firefightingEntityApp.Jurisdiction = "1,1,1";
                }
                else
                {
                    firefightingEntityApp.Jurisdiction = "0,0,0";
                }
                firefightingEntityApp.FirefightingEntity = data;
                firefightingEntityApp.ExamineRecordList = GetExamineRecordGrid(data.Id);
                firefightingEntityApp.DetectionRecordList = GetDetectionRecordGrid(data.Id);

                return new { Code = 0, Count = 1, Info = "获取数据成功", data = firefightingEntityApp };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 关联检查记录
        /// </summary>
        /// <param name="EquipmentId">消防设施ID</param>
        /// <returns></returns>
        public object GetExamineRecordGrid(string EquipmentId)
        {
            object data = null;

            if (!string.IsNullOrWhiteSpace(EquipmentId))
            {
                var list = examinerecordbll.GetList(string.Format(" and EquipmentId='{0}' and rownum<5", EquipmentId)).OrderByDescending(x => x.CreateDate);
                data = list;
            }

            return data;
        }
        /// <summary>
        /// 关联检测维保记录
        /// </summary>
        /// <param name="EquipmentId">消防设施ID</param>
        /// <returns></returns>
        public object GetDetectionRecordGrid(string EquipmentId)
        {
            object data = null;

            if (!string.IsNullOrWhiteSpace(EquipmentId))
            {
                var list = detectionrecordbll.GetList(string.Format(" and EquipmentId='{0}' and rownum<5", EquipmentId)).OrderByDescending(x => x.CreateDate);
                data = list;
            }

            return data;
        }
        /// <summary>
        /// 获取检查记录
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetExamineEntity([FromBody]JObject json)
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
                string id = dy.data.Id;//ID
                var data = examinerecordbll.GetEntity(id);

                return new { Code = 0, Count = 1, Info = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 获取检测、巡查记录
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetDetectionEntity([FromBody]JObject json)
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
                string id = dy.data.Id;//ID
                DetectionRecordEntity entity = detectionrecordbll.GetEntity(id);
                dynamic obj = new ExpandoObject();
                obj.Id = entity.Id;
                obj.EquipmentId = entity.EquipmentId;
                obj.DetectionPerson = entity.DetectionPerson;
                obj.DetectionDate = entity.DetectionDate; 
                obj.Describe = entity.Describe;
                obj.Project = entity.Project;
                obj.RegisterUser = entity.Conclusion;
                obj.RegisterUserId = entity.DetectionPersonId;
                obj.Remark = entity.Remark;
                IList<Photo> pList = new List<Photo>(); //附件
                DataTable file = fileInfoBLL.GetFiles(entity.Id);
                foreach (DataRow dr in file.Rows)
                {
                    Photo p = new Photo();
                    p.id = dr["fileid"].ToString();
                    p.filename = dr["filename"].ToString();
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + dr["filepath"].ToString().Substring(1);
                    pList.Add(p);
                }
                obj.file = pList;

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
                        //获取文件完整文件名(包含绝对路径)
                        //文件存放路径格式：/Resource/ResourceFile/{userId}{data}/{guid}.{后缀名}
                        string userId = OperatorProvider.Provider.Current().UserId;
                        string fileGuid = Guid.NewGuid().ToString();
                        long filesize = file.ContentLength;
                        string FileEextension = Path.GetExtension(file.FileName);
                        string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                        string virtualPath = string.Format("~/Resource/DocumentFile/{0}/{1}/{2}{3}", userId, uploadDate, fileGuid, FileEextension);
                        string virtualPath1 = string.Format("/Resource/DocumentFile/{0}/{1}/{2}{3}", userId, uploadDate, fileGuid, FileEextension);
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
                            if (!string.IsNullOrEmpty(folderId))
                            {
                                fileInfoEntity.FolderId = folderId;
                            }
                            else
                            {
                                fileInfoEntity.FolderId = "0";
                            }
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

        /// <summary>
        /// 一键提醒
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object Remind([FromBody]JObject json)
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
                string id = dy.data.Id;//ID
                string type = dy.data.Type;//类型
                MessageEntity messageEntity = new MessageEntity();
                var data = firefightingbll.GetEntity(id);
                messageEntity.Title = type;
                if (type == "消防设施检查提醒") {
                    messageEntity.Content = "请按时检查消防器材（器材名称：" + data.EquipmentName + "，编号：" + data.EquipmentCode + "）";
                }
                if (type == "消防设施检测提醒")
                {
                    messageEntity.Content = "请按时检测消防器材（器材名称：" + data.EquipmentName + "，编号：" + data.EquipmentCode + "）";
                }
                messageEntity.SendUser = "System";
                messageEntity.SendUserName = "系统管理员";
                var userinfo = new UserBLL().GetUserInfoByName(data.DutyDept, data.DutyUser);
                messageEntity.UserId = userinfo.Account;
                messageEntity.UserName = data.DutyUser;
                messageEntity.SendTime = DateTime.Now;
                messageEntity.Category = "其它";
                new MessageBLL().SaveForm("", messageEntity);
                return new { Code = 0, Count = 1, Info = "成功", data = "" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
    }

    public class FirefightingEntityApp
    {
        public string Jurisdiction { get; set; }//0无 1有  “新增，修改，删除”
        public FirefightingEntity FirefightingEntity { get; set; }
        public object ExamineRecordList { get; set; }
        public object DetectionRecordList { get; set; }
    }
}