using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.Desktop;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Busines.PersonManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.RiskDatabase;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Entity.PersonManage;
using ERCHTMS.Entity.PublicInfoManage;
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
using ERCHTMS.AppSerivce.Models;
using ERCHTMS.Entity.HseManage;
using ERCHTMS.Busines.HseManage;
using ERCHTMS.AppSerivce.Model;
using System.Threading.Tasks;

namespace ERCHTMS.AppSerivce.Controllers
{

    /// <summary>
    /// 预警指标卡
    /// </summary>
    public class CheckRedordController : ApiController
    {
        [HttpPost]
        public ListResult<CheckRecordEntity> GetMine(BaseParam model)
        {
            var bll = new CheckRecordBLL();
            var total = 0;
            var data = bll.GetMine(model.UserId, model.PageSize, model.PageIndex, out total);
            return new ListResult<CheckRecordEntity>() { Success = true, Total = total, Data = data };
        }

        [HttpPost]
        public ListResult<CheckRecordEntity> GetList(ModelParam<WarningCardModel> args)
        {
            var bll = new CheckRecordBLL();
            var total = 0;
            if (args.Data.To.HasValue) args.Data.To = args.Data.To.Value.AddDays(1).AddSeconds(-1);
            var depts = new DepartmentBLL().GetSubDepartments(args.Data.DeptId, null);
            var deptis = depts.Select(x => x.DepartmentId).ToArray();

            var data = bll.GetList(deptis, args.Data.Key, args.Data.From, args.Data.To, args.PageSize, args.PageIndex, out total);
            return new ListResult<CheckRecordEntity>() { Success = true, Total = total, Data = data };
        }

        [HttpPost]
        public ModelResult<CheckRecordEntity> GetDetail(ModelParam<string> args)
        {
            var bll = new CheckRecordBLL();
            var data = bll.GetDetail(args.Data);
            string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
            foreach (var item in data.CheckItems)
            {
                foreach (var item1 in item.Files)
                {
                    item1.FilePath = webUrl + item1.FilePath.Replace("~", string.Empty);
                }
            }
            return new ModelResult<CheckRecordEntity>() { Data = data, Success = true };
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Add()
        {
            if (!this.Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var root = HttpContext.Current.Server.MapPath("~/files");
            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);

            var provider = new MultipartFormDataStreamProvider(root);
            var success = true;
            var message = string.Empty;

            string filepath = new DataItemDetailBLL().GetItemValue("imgPath") + "\\Resource";
            var fold = "hse";

            try
            {
                // Read the form data.
                await this.Request.Content.ReadAsMultipartAsync(provider);
                var json = provider.FormData.Get("json");
                var args = Newtonsoft.Json.JsonConvert.DeserializeObject<ModelParam<CheckRecordEntity>>(json);
                var user = new UserBLL().GetEntity(args.UserId);
                var dept = new DepartmentBLL().GetEntity(user.DepartmentId);
                args.Data.Files = new List<FileInfoEntity>();
                args.Data.CheckRecordId = Guid.NewGuid().ToString();
                args.Data.CreateUserId = args.Data.ModifyUserId = args.UserId;
                args.Data.CreateTime = args.Data.ModifyTime = DateTime.Now;
                args.Data.DeptId = user.DepartmentId;
                args.Data.DeptName = dept.FullName;
                if (args.Data.CheckItems == null) args.Data.CheckItems = new List<CheckItemEntity>();
                foreach (var item in args.Data.CheckItems)
                {
                    item.CheckItemId = Guid.NewGuid().ToString();
                    item.CheckRecordId = args.Data.CheckRecordId;
                    item.CreateUserId = item.ModifyUserId;
                    item.CreateTime = item.ModifyTime;
                }


                // This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData)
                {
                    var filename = file.Headers.ContentDisposition.FileName.Trim('"');
                    var clientid = file.Headers.ContentDisposition.Name.Trim('"');
                    var ss = clientid.Split('_');

                    var fileid = Guid.NewGuid().ToString();
                    var path = Path.Combine(filepath, fold);
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    File.Move(file.LocalFileName, Path.Combine(path, fileid + Path.GetExtension(filename)));

                    var fileentity = new FileInfoEntity() { CreateDate = DateTime.Now, CreateUserId = args.UserId, CreateUserName = user.Account, DeleteMark = 0, FileId = fileid, FileName = filename, ModifyDate = DateTime.Now, ModifyUserId = user.UserId, ModifyUserName = user.Account, FileExtensions = Path.GetExtension(filename) };
                    fileentity.FilePath = "~/Resource/" + fold + "/" + fileid + Path.GetExtension(filename);
                    var ck = args.Data.CheckItems.Find(x => x.clientid == ss[0]);
                    if (ck != null) fileentity.RecId = ck.CheckItemId;
                    fileentity.Description = ss[1];

                    //switch (file.Headers.ContentType.MediaType)
                    //{
                    //    case "image/jpeg":
                    //    //fileentity.Description = "照片";
                    //    //break;
                    //    case "image/jpg":
                    //    //fileentity.Description = "照片";
                    //    //break;
                    //    case "image/png":
                    //        fileentity.Description = "照片";
                    //        break;
                    //    case "audio/mpeg":
                    //    //fileentity.Description = "音频";
                    //    //break;
                    //    case "audio/mp3":
                    //        fileentity.Description = "音频";
                    //        break;
                    //    case "video/mp4":
                    //        fileentity.Description = "视频";
                    //        break;
                    //    default:
                    //        break;
                    //}
                    args.Data.Files.Add(fileentity);

                }
                var bll = new CheckRecordBLL();
                bll.Save(args.Data);

                return Request.CreateResponse<BaseResult>(HttpStatusCode.OK, new BaseResult() { Success = success, Message = message });
            }
            catch (System.Exception e)
            {
                success = false;
                message = e.Message;
                return Request.CreateResponse<BaseResult>(HttpStatusCode.OK, new BaseResult() { Success = success, Message = message });
            }

            //var success = true;
            //var message = string.Empty;
            //var bll = new CheckRecordBLL();
            //try
            //{
            //    bll.Save(args.Data);
            //}
            //catch (Exception e)
            //{
            //    success = false;
            //    message = e.Message;
            //}
            //return new BaseResult() { Success = success, Message = message };
        }

        [HttpPost]
        public BaseResult Update(ModelParam<CheckRecordEntity> args)
        {
            args.Data.CreateUserId = args.Data.ModifyUserId = args.UserId;
            args.Data.CreateTime = args.Data.ModifyTime = DateTime.Now;
            if (args.Data.CheckItems == null) args.Data.CheckItems = new List<CheckItemEntity>();
            foreach (var item in args.Data.CheckItems)
            {
                if (string.IsNullOrEmpty(item.CheckItemId))
                    item.CheckItemId = Guid.NewGuid().ToString();
                item.CheckRecordId = args.Data.CheckRecordId;
                item.CreateUserId = item.ModifyUserId;
                item.CreateTime = item.ModifyTime;
            }

            var success = true;
            var message = string.Empty;
            var bll = new CheckRecordBLL();
            try
            {
                bll.Save(args.Data);
            }
            catch (Exception e)
            {
                success = false;
                message = e.Message;
            }
            return new BaseResult() { Success = success, Message = message };
        }

        [HttpPost]
        public ListResult<WarningCardEntity> Find(ModelParam<string> args)
        {
            var bll = new WarningCardBLL();
            var total = 0;
            var data = bll.GetData(args.Data, args.PageSize, args.PageIndex, out total);
            return new ListResult<WarningCardEntity>() { Success = true, Total = total, Data = data };
        }

        [HttpPost]
        public ModelResult<WarningCardEntity> Detail(ModelParam<string> args)
        {
            var bll = new WarningCardBLL();
            var data = bll.GetDetail(args.Data);

            return new ModelResult<WarningCardEntity>() { Success = true, Data = data };
        }
    }
}