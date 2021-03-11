using BSFramework.Util.WebControl;
using ERCHTMS.AppSerivce.Model;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.FireManage;
using ERCHTMS.Busines.HiddenTroubleManage;
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
    public class KeyPartController : BaseApiController
    {
        public HttpContext ctx { get { return HttpContext.Current; } }
        private MoveFireRecordBLL movefirerecordbll = new MoveFireRecordBLL();
        private PatrolRecordBLL patrolrecordbll = new PatrolRecordBLL();
        private KeyPartBLL keypartbll = new KeyPartBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
        private MoveFireAreaBLL movefireareabll = new MoveFireAreaBLL();

        private HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL(); //隐患基本信息

        /// <summary>
        /// 获取消防重点部位列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetKeyPartList([FromBody]JObject json)
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
                pagination.p_fields = "PartName,District,DutyUser,Dutydept,Rank,NextPatrolDate,EmployState,createuserid";
                pagination.p_tablename = "HRS_KEYPART";
                pagination.conditionJson = string.Format(" CREATEUSERORGCODE ='{0}'", curUser.OrganizeCode);
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "createdate";//排序字段
                pagination.sord = "desc";//排序方式  

                //名称
                string PartNo = dy.data.PartNo;
                if (!string.IsNullOrEmpty(PartNo))
                {
                    pagination.conditionJson += string.Format(" and PartNo='{0}'", PartNo);
                }
                //部门
                string DutydeptCode = dy.data.DutydeptCode;
                if (!string.IsNullOrEmpty(DutydeptCode))
                {
                    pagination.conditionJson += string.Format(" and DutydeptCode like'{0}%'", DutydeptCode);
                }
                string EmployState = dy.data.EmployState;
                //使用状态
                if (!string.IsNullOrEmpty(EmployState))
                {
                    pagination.conditionJson += string.Format(" and EmployState='{0}'", EmployState);
                }
                string Rank = dy.data.Rank;
                //动火级别
                if (!string.IsNullOrEmpty(Rank))
                {
                    pagination.conditionJson += string.Format(" and Rank='{0}'", Rank);
                }
                DataTable dt = keypartbll.GetPageList(pagination, null);
                return new { code = 0, info = "获取数据成功", count = pagination.records, data = dt };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }

        /// <summary>
        /// 获取消防重点部位列表(通用版)
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetGenericKeyPartList([FromBody]JObject json)
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
                pagination.p_fields = "PartName,District,DutyUser,Dutydept,Rank,NextPatrolDate,EmployState,createuserid";
                pagination.p_tablename = "HRS_KEYPART";
                pagination.conditionJson = string.Format(" CREATEUSERORGCODE ='{0}'", curUser.OrganizeCode);
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "createdate";//排序字段
                pagination.sord = "desc";//排序方式  

                //名称
                string PartNo = dy.data.PartNo;
                if (!string.IsNullOrEmpty(PartNo))
                {
                    pagination.conditionJson += string.Format(" and partname like '%{0}%'", PartNo);
                }
                //部门
                string DutydeptCode = dy.data.DutydeptCode;
                if (!string.IsNullOrEmpty(DutydeptCode))
                {
                    pagination.conditionJson += string.Format(" and DutydeptCode like'{0}%'", DutydeptCode);
                }
                string EmployState = dy.data.EmployState;
                //使用状态
                if (!string.IsNullOrEmpty(EmployState))
                {
                    pagination.conditionJson += string.Format(" and EmployState='{0}'", EmployState);
                }
                string Rank = dy.data.Rank;
                //动火级别
                if (!string.IsNullOrEmpty(Rank))
                {
                    pagination.conditionJson += string.Format(" and Rank='{0}'", Rank);
                }
                DataTable dt = keypartbll.GetPageList(pagination, null);
                return new { code = 0, info = "获取数据成功", count = pagination.records, data = dt };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }
        /// <summary>
        /// 获取动火记录
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetMoveFireRecordList([FromBody]JObject json)
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
                pagination.p_fields = "MainId,WorkTicket,WorkUnit,WorkUnitCode,WorkSite,ExecuteStartDate,ExecuteEndDate,ExecuteUser,createuserid,DutyUser,DutyUserId,ExecuteUserId,RegisterUser,RegisterUserId,RegisterDate,WorkEndDate,WorkRegisterUser,WorkRegisterUserId";
                pagination.p_tablename = "HRS_MOVEFIRERECORD";
                pagination.conditionJson = string.Format(" CREATEUSERORGCODE ='{0}'", curUser.OrganizeCode);
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "createdate";//排序字段
                pagination.sord = "desc";//排序方式  
                //查询条件
                string MainId = dy.data.MainId;
                if (!string.IsNullOrEmpty(MainId))
                {
                    pagination.conditionJson += string.Format(" and MainId='{0}'", MainId);
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
                    eTime = (Convert.ToDateTime(eTime).AddDays(1)).ToString("yyyy-MM-dd");
                    pagination.conditionJson += string.Format(" and ExecuteStartDate between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", sTime, eTime);
                }
                DataTable dt = movefirerecordbll.GetPageList(pagination, null);
                return new { code = 0, info = "获取数据成功", count = pagination.records, data = dt };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }
        /// <summary>
        /// 获取巡查记录
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetPatrolRecordList([FromBody]JObject json)
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
                pagination.p_fields = "PatrolDate,NextPatrolDate,PatrolPerson,State,createuserid,Describe,Measure,PatrolPersonId,MainId,PatrolPeriod";
                pagination.p_tablename = "HRS_PATROLRECORD";
                pagination.conditionJson = string.Format(" CREATEUSERORGCODE ='{0}'", curUser.OrganizeCode);
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "createdate";//排序字段
                pagination.sord = "desc";//排序方式 
                //查询条件
                string MainId = dy.data.MainId;
                if (!string.IsNullOrEmpty(MainId))
                {
                    pagination.conditionJson += string.Format(" and MainId='{0}'", MainId);
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
                    eTime = (Convert.ToDateTime(eTime).AddDays(1)).ToString("yyyy-MM-dd");
                    pagination.conditionJson += string.Format(" and PatrolDate between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", sTime, eTime);
                }
                DataTable dt = patrolrecordbll.GetPageList(pagination, null);
                return new { code = 0, info = "获取数据成功", count = pagination.records, data = dt };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }

        /// <summary>
        /// 新增/编辑消防重点部位
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object SaveKeyPart()
        {
            try
            {
                string res = ctx.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid; //当前用户ID 
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                string keyValue = dy.data.Id;
                string str = JsonConvert.SerializeObject(dy.data);
                KeyPartEntity entity = JsonConvert.DeserializeObject<KeyPartEntity>(str);

                //HttpFileCollection files = ctx.Request.Files;//上传的文件 

                keypartbll.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                return new { code = -1, count = 0, info = "保存失败" };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }
        /// <summary>
        /// 新增/编辑动火记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object SaveMoveFireRecord()
        {
            try
            {
                string res = ctx.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid; //当前用户ID 
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                string keyValue = dy.data.Id;
                string str = JsonConvert.SerializeObject(dy.data);
                MoveFireRecordEntity entity = JsonConvert.DeserializeObject<MoveFireRecordEntity>(str);

                //HttpFileCollection files = ctx.Request.Files;//上传的文件 

                movefirerecordbll.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                return new { code = -1, count = 0, info = "保存失败" };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }
        /// <summary>
        /// 新增/编辑巡查记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object SavePatrolRecord()
        {
            try
            {
                string res = ctx.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid; //当前用户ID 
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                string keyValue = dy.data.Id;
                string str = JsonConvert.SerializeObject(dy.data);
                PatrolRecordEntity entity = JsonConvert.DeserializeObject<PatrolRecordEntity>(str);

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
                UploadifyFile(entity.Id, "PatrolRecord", files);

                patrolrecordbll.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                return new { code = -1, count = 0, info = "保存失败" };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }

        /// <summary>
        /// 获取重点部位详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetKeyPartEntity([FromBody]JObject json)
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
                var data = keypartbll.GetEntity(id);
                KeyPartApp keyPartApp = new KeyPartApp();
                if (userId == data.DutyUserId)
                    keyPartApp.Jurisdiction = "1,1,1";
                else
                    keyPartApp.Jurisdiction = "0,0,0";
                keyPartApp.KeyPartEntity = data;
                keyPartApp.MoveFireRecordEntity = GetMoveFireRecordGrid(id);
                keyPartApp.PatrolRecordEntity = GetPatrolRecordGrid(id);
                keyPartApp.HiddenTroubleNum = GetHiddenTroubleNum(id,data.DutyUserId);
                return new { Code = 0, Count = 1, Info = "获取数据成功", data = keyPartApp };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 关联动火记录
        /// </summary>
        /// <param name="MainId">ID</param>
        /// <returns></returns>
        public object GetMoveFireRecordGrid(string MainId)
        {
            object data = null;

            if (!string.IsNullOrWhiteSpace(MainId))
            {
                var list = movefirerecordbll.GetList(string.Format(" and MainId='{0}' and rownum<5", MainId)).OrderByDescending(x => x.CreateDate);
                data = list;
            }

            return data;
        }
        public int GetHiddenTroubleNum(string Id,string DutyUserId)
        {
            try
            {
                int page = 1, rows = 9999999;
                Pagination pagination = new Pagination();

                pagination.page = page;//页数
                pagination.rows = rows;//行数

                JObject queryJson = new JObject();
                Operator opertator = new OperatorProvider().Current();
                string userId = opertator.UserId;
                queryJson.Add(new JProperty("userId", DutyUserId));
                queryJson.Add(new JProperty("isPlanLevel", opertator.isPlanLevel));
                queryJson.Add(new JProperty("RelevanceId", Id));
                queryJson.Add(new JProperty("RelevanceType", "KeyPart"));
                var data = htbaseinfobll.GetHiddenBaseInfoPageList(pagination, queryJson.ToString());
                return data.Rows.Count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        /// <summary>
        /// 关联巡查记录
        /// </summary>
        /// <param name="MainId">ID</param>
        /// <returns></returns>
        public object GetPatrolRecordGrid(string MainId)
        {
            object data = null;

            if (!string.IsNullOrWhiteSpace(MainId))
            {
                var list = patrolrecordbll.GetList(string.Format(" and MainId='{0}' and rownum<5", MainId)).OrderByDescending(x => x.CreateDate);
                data = list;
            }

            return data;
        }
        /// <summary>
        /// 获取动火记录详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetMoveFireEntity([FromBody]JObject json)
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
                var data = movefirerecordbll.GetEntity(id);

                return new { Code = 0, Count = 1, Info = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 获取巡查记录详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetPatrolRecordEntity([FromBody]JObject json)
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
                PatrolRecordEntity entity = patrolrecordbll.GetEntity(id);
                dynamic obj = new ExpandoObject();
                obj.Id = entity.Id;
                obj.NextPatrolDate = entity.NextPatrolDate;
                obj.State = entity.State;
                obj.Describe = entity.Describe;
                obj.Measure = entity.Measure;
                obj.PatrolDate = entity.PatrolDate;
                obj.PatrolPerson = entity.PatrolPerson;
                obj.PatrolPersonId = entity.PatrolPersonId;
                obj.MainId = entity.MainId;
                obj.PatrolPeriod = entity.PatrolPeriod;
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
        /// 获取动火区域划分
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetMoveFireAreaList([FromBody]JObject json)
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

                //查询条件 主表ID
                string Rank = dy.data.Rank;
                JObject queryJson = new JObject();
                queryJson.Add(new JProperty("Rank", Rank));
                IEnumerable<MoveFireAreaEntity> dt = movefireareabll.GetList(queryJson.ToString());
                return new { code = 0, info = "获取数据成功", count = dt.Count(), data = dt };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
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
                var data = keypartbll.GetEntity(id);
                messageEntity.Title = type;
                messageEntity.Content = "请立即巡查消防重点部位（重点防火部位名称：" + data.PartName + "，位置：" + data.District + "）";
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
    public class KeyPartApp
    {
        public string Jurisdiction { get; set; }//0无 1有  “新增，修改，删除”
        public int HiddenTroubleNum { get; set; }
        public KeyPartEntity KeyPartEntity { get; set; }
        public object MoveFireRecordEntity { get; set; }
        public object PatrolRecordEntity { get; set; }
    }

}