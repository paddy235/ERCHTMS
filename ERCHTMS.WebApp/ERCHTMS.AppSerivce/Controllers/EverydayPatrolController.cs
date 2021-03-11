using BSFramework.Util.WebControl;
using ERCHTMS.Busines.FireManage;
using ERCHTMS.Busines.HighRiskWork;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.FireManage;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Entity.SystemManage.ViewModel;
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
    public class EverydayPatrolController : BaseApiController
    {
        public HttpContext ctx { get { return HttpContext.Current; } }
        private EverydayPatrolBLL everydaypatrolbll = new EverydayPatrolBLL();
        private EverydayPatrolDetailBLL everydaypatroldetailbll = new EverydayPatrolDetailBLL();
        private EverydayProjectSetBLL EverydayProjectSetbll = new EverydayProjectSetBLL();//获取检查项
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
        private AffirmRecordBLL affirmrecordbll = new AffirmRecordBLL();

        /// <summary>
        /// 获取日常巡查列表（主表）
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetEverydayPatrolList([FromBody]JObject json)
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
                pagination.p_fields = "PatrolType,PatrolTypeCode,AffirmState,AffirmUserId,District,PATROLDEPT,PATROLDATE,PATROLPERSON,PATROLPLACE,PROBLEMNUM,createuserid,createuserdeptcode,createuserorgcode";
                pagination.p_tablename = "HRS_EVERYDAYPATROL";
                pagination.conditionJson = string.Format(" CREATEUSERORGCODE ='{0}'", curUser.OrganizeCode);
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "createdate";//排序字段
                pagination.sord = "desc";//排序方式  

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
                //查询条件 巡查人
                string PatrolPersonId = dy.data.PatrolPersonId;
                if (!string.IsNullOrEmpty(PatrolPersonId))
                {
                    pagination.conditionJson += string.Format(" and PatrolPersonId='{0}'", PatrolPersonId);
                }
                //巡查部门
                string PatrolDeptCode = dy.data.PatrolDeptCode;
                if (!string.IsNullOrEmpty(PatrolDeptCode))
                {
                    pagination.conditionJson += string.Format(" and PatrolDeptCode like '{0}%'", PatrolDeptCode);
                }
                //巡查类型
                string PatrolTypeCode = dy.data.PatrolTypeCode;
                if (!string.IsNullOrEmpty(PatrolTypeCode))
                {
                    pagination.conditionJson += string.Format(" and PatrolTypeCode='{0}'", PatrolTypeCode);
                }
                //流程状态
                string AffirmState = dy.data.AffirmState;
                if (!string.IsNullOrEmpty(AffirmState))
                {
                    pagination.conditionJson += string.Format(" and AffirmState='{0}'", AffirmState);
                }
                
                //全部0 / 我的1
                string Type = dy.data.Type;
                if (!string.IsNullOrEmpty(Type))
                {
                    if (Type == "1")
                    {
                        pagination.conditionJson += string.Format(" and((createuserid='{0}' and AffirmState=0) or (AffirmUserId like '%{1}%' and AffirmState=1))", userId, curUser.Account);
                    }
                }
                DataTable dt = everydaypatrolbll.GetPageList(pagination, null);
                return new { code = 0, info = "获取数据成功", count = pagination.records, data = dt };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }

        /// <summary>
        /// 获取日常巡查检查项（明细表）编辑时获取的数据
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetEverydayPatrolDetailList([FromBody]JObject json)
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
                string PatrolId = dy.data.PatrolId;
                JObject queryJson = new JObject();
                queryJson.Add(new JProperty("PatrolId", PatrolId));
                IEnumerable<EverydayPatrolDetailTjEntity> dt = everydaypatroldetailbll.GetList(queryJson.ToString());
                return new { code = 0, info = "获取数据成功", count = dt.Count(), data = dt };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }

        /// <summary>
        /// 获取巡查项（第一次打开时）新增时获取的数据
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetPatrolProjectList([FromBody]JObject json)
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
                pagination.p_fields = "CreateDate,MeasureName,MeasureResultOne,MeasureResultTwo,OrderNumber";
                pagination.p_tablename = " hrs_EverydayProjectSet";
                pagination.conditionJson = "1=1 ";
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "OrderNumber";//排序字段
                pagination.sord = "asc";//排序方式  
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (!user.IsSystem)
                {
                    pagination.conditionJson += " and createuserorgcode='" + user.OrganizeCode + "'";
                }

                string ehsDepartCode = "";
                //当前用户
                //Operator curUser = OperatorProvider.Provider.Current();
                //获取日常巡查ID
                //巡查类型
                string PatrolType = dy.data.PatrolType;
                if (!string.IsNullOrEmpty(PatrolType))
                {
                    DataItemDetailEntity entity = dataitemdetailbll.GetListByItemCodeEntity(PatrolType);
                    if (entity != null)
                        ehsDepartCode = entity.ItemValue;
                }
                

                //string Code = dy.data.Code;
                JObject queryJson = new JObject();
                queryJson.Add(new JProperty("code", ehsDepartCode));
                DataTable dt = EverydayProjectSetbll.GetPageDataTable(pagination, queryJson.ToString());
                foreach (DataRow item in dt.Rows) {
                    item["id"] = Guid.NewGuid().ToString();
                }
                return new { code = 0, info = "获取数据成功", count = pagination.records, data = dt };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }

        /// <summary>
        /// 新增/编辑日常巡查（主表）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object SaveEverydayPatrol()
        {
            try
            {
                string res = ctx.Request["json"];
                var dy = JsonConvert.DeserializeObject<ExpandoObjectEverydayPatrol>(res);
                string userId = dy.userid; //当前用户ID 
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                
                string keyValue = dy.data.EverydayPatrolEntity.Id;
                //string str = JsonConvert.SerializeObject(dy.data.detailList);
                EverydayPatrolEntity entity = dy.data.EverydayPatrolEntity;
                List<EverydayPatrolDetailEntity> projects = dy.data.EverydayPatrolDetailEntity;

                //everydaypatrolbll.SaveForm(keyValue, entity);
                if (string.IsNullOrEmpty(keyValue))
                {
                    keyValue = Guid.NewGuid().ToString();
                }
                if (projects == null)
                {
                    return new { code = -1, count = 0, info = "保存失败" };
                }
                var num = 0;
                if (projects.Count > 0)
                {
                    foreach (var item in projects)
                    {
                        if (item.Result == 1)
                        {
                            num = num + 1;
                        }
                        item.PatrolId = keyValue;
                        everydaypatroldetailbll.SaveForm(item.Id, item);//保存明细
                    }
                    entity.ProblemNum = num;
                    string majorUserId = everydaypatrolbll.GetMajorUserId(entity.ByDeptId);
                    entity.ByUserId = majorUserId;
                    everydaypatrolbll.SaveForm(keyValue, entity);//保存主表
                }

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
                HttpFileCollection files = ctx.Request.Files;//上传的文件 签名

                UploadifyFile(entity.Id, "AffirmRecord", files);
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "保存失败" };
            }
            return new { code = 0, count = 0, info = "保存成功" };
        }
        /// <summary>
        /// 新增/编辑日常巡查（明细表，日常巡查项目）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object SaveEverydayPatrolDetail()
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
                EverydayPatrolDetailEntity entity = JsonConvert.DeserializeObject<EverydayPatrolDetailEntity>(str);

                //HttpFileCollection files = ctx.Request.Files;//上传的文件 

                everydaypatroldetailbll.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                return new { code = -1, count = 0, info = "保存失败" };
            }
            return new { code = 0, count = 0, info = "保存成功" };
        }

        /// <summary>
        /// 新增/编辑确认记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object SaveAffirmRecord()
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
                AffirmRecordEntity entity = JsonConvert.DeserializeObject<AffirmRecordEntity>(str);

                //HttpFileCollection files = ctx.Request.Files;//上传的文件 
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
                HttpFileCollection files = ctx.Request.Files;//上传的文件 签名
                                                            
                UploadifyFile(entity.Id, "AffirmRecord", files);

                affirmrecordbll.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                return new { code = -1, count = 0, info = "保存失败" };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }

        /// <summary>
        /// 获取消防巡查记录
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetEveryEntity([FromBody]JObject json)
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
                var data = everydaypatrolbll.GetEntity(id);
                string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                if (data.Signature.IndexOf("http") > -1)
                {
                    data.Signature = data.Signature;
                }
                else
                {
                    data.Signature = webUrl + data.Signature;
                }
                EverydayPatrolApp everydayPatrolApp = new EverydayPatrolApp();
                everydayPatrolApp.Jurisdiction = "1,1,1";
                everydayPatrolApp.EverydayPatrolEntity = data;
                everydayPatrolApp.EverydayPatrolDetailEntity = GetEverydayPatrolDetailGrid(id);
                everydayPatrolApp.AffirmRecordEntity = GetAffirmRecordGrid(id);
                return new { Code = 0, Count = 1, Info = "获取数据成功", data = everydayPatrolApp };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        /// <summary>
        /// 关联巡查项目
        /// </summary>
        /// <param name="Id">巡查记录ID</param>
        /// <returns></returns>
        public object GetEverydayPatrolDetailGrid(string PatrolId)
        {
            object data = null;

            if (!string.IsNullOrWhiteSpace(PatrolId))
            {
                JObject queryJson = new JObject();
                queryJson.Add(new JProperty("PatrolId", PatrolId));
                var list = everydaypatroldetailbll.GetList(queryJson.ToString());
                
                data = list;
            }

            return data;
        }
        /// <summary>
        /// 关联确认记录
        /// </summary>
        /// <param name="Id">巡查记录ID</param>
        /// <returns></returns>
        public object GetAffirmRecordGrid(string PatrolId)
        {
            object data = null;

            if (!string.IsNullOrWhiteSpace(PatrolId))
            {
                JObject queryJson = new JObject();
                queryJson.Add(new JProperty("PatrolId", PatrolId));
                var list = affirmrecordbll.GetList(queryJson.ToString());
                string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                foreach (var item in list) {
                    
                    if (item.Signature.IndexOf("http") > -1)
                    {
                        item.Signature = item.Signature;
                    }
                    else
                    {
                        item.Signature = webUrl + item.Signature;
                    }
                }
                data = list;
            }

            return data;
        }
        /// <summary>
        /// 图片上传
        /// </summary>....
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
    }
    public class EverydayPatrolApp
    {
        public string Jurisdiction { get; set; }//0无 1有  “新增，修改，删除”
        public EverydayPatrolEntity EverydayPatrolEntity { get; set; }
        public object EverydayPatrolDetailEntity { get; set; }
        public object AffirmRecordEntity { get; set; }
    }

    public class ExpandoObjectEverydayPatrol
    {

        public string userid { get; set; }
       

        public AddEverydayPatrolApp data { get; set; }
    }
    public class AddEverydayPatrolApp
    {
        public string deleteFileId { get; set; }
        public EverydayPatrolEntity EverydayPatrolEntity { get; set; }
        public List<EverydayPatrolDetailEntity> EverydayPatrolDetailEntity { get; set; }
    }
}