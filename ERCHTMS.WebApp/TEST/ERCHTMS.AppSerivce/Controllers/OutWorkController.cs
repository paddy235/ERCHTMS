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

namespace ERCHTMS.AppSerivce.Controllers
{
    /// <summary>
    /// 外包工程
    /// </summary>
    public class OutWorkController : BaseApiController
    {

        /// <summary>
        /// 获取单位资质审核详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetListByOutengineerId([FromBody]JObject json)
        {
            try
            {
                string path = new DataItemDetailBLL().GetItemValue("imgUrl");
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string engineerId = dy.data; //外包工程Id
                string userId = dy.userid;
                AptitudeinvestigateinfoBLL AptitudeinvestigateinfoBLL = new AptitudeinvestigateinfoBLL();
                var data = AptitudeinvestigateinfoBLL.GetListByOutengineerId(engineerId);
                var fileList1 = new FileInfoBLL().GetFileList(data.ID + "01");
                var fileList2 = new FileInfoBLL().GetFileList(data.ID + "02");
                var fileList3 = new FileInfoBLL().GetFileList(data.ID + "03");
                var fileList4 = new FileInfoBLL().GetFileList(data.ID + "04");
                var fileList5 = new FileInfoBLL().GetFileList(data.ID + "05");
                var fileList6 = new FileInfoBLL().GetFileList(data.ID + "06");
                var fileList7 = new FileInfoBLL().GetFileList(data.ID + "07");
                if (data.ID == null)
                {
                    return new { Code = -1, Count = 0, Info = "没有数据" };
                }
                else
                {
                    return new { Code = 0, Count = 1, Info = "获取数据成功", data = data, fileList01 = fileList1, fileList02 = fileList2, fileList03 = fileList3, fileList04 = fileList4, fileList05 = fileList5, fileList06 = fileList6, fileList07 = fileList7 };
                }

            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        [HttpPost]
        public object GetSchemeMeasureListByOutengineerId([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string engineerId = dy.data; //外包工程Id
                SchemeMeasureBLL SchemeMeasureBLL = new SchemeMeasureBLL();
                var data = SchemeMeasureBLL.GetSchemeMeasureListByOutengineerId(engineerId);
                var fileList1 = new FileInfoBLL().GetFileList(data.ID + "01");
                if (data.ID == null)
                {
                    return new { Code = -1, Count = 0, Info = "没有数据" };
                }
                else
                {
                    return new { Code = 0, Count = 1, Info = "获取数据成功", data = data, fileList01 = fileList1 };
                }

            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        [HttpPost]
        public object SaveForm([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                var o = JsonConvert.DeserializeObject<AptitudeinvestigateinfoEntity>(dy.data);
                AptitudeinvestigateinfoBLL aptitudeinvestigateinfobll = new AptitudeinvestigateinfoBLL();
                aptitudeinvestigateinfobll.SaveForm(dy.keyValue, o);
                return new { Code = 0, Count = 1, Info = "保存数据成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }

        }

        /// <summary>
        /// 根据RecId来查询附件信息
        /// </summary>
        /// <param name="RecId"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetFilesByRecId(string RecId)
        {
            try
            {
                FileInfoBLL fileInfoBLL = new FileInfoBLL();
                var data = fileInfoBLL.GetFiles(RecId);

                if (data != null)
                {
                    foreach (DataRow item in data.Rows)
                    {
                        var path = item.Field<string>("FilePath");
                        var url = Url.Content(path);
                        item.SetField<string>("FilePath", url);
                    }

                }

                return new { Code = 0, Count = data.Rows.Count, Info = "获取数据成功", data = data };

            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
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


        [HttpPost]
        public object getNewProjects([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator currUser = OperatorProvider.Provider.Current();
                DataTable dt = new OutsouringengineerBLL().GetEngineerByCurrDept();
                return new { Code = 0, Count = 0, Info = "操作成功", data = dt };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        [HttpPost]

        public object GetCompactProtocol([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string keyValue = dy.keyValue;
                var data = new CompactBLL().GetLastCompactProtocol(keyValue);
                //dynamic getdate = JsonConvert.DeserializeObject<ExpandoObject>(Newtonsoft.Json.JsonConvert.SerializeObject(data));


                dynamic dataSource = JsonConvert.DeserializeObject<ExpandoObject>(Newtonsoft.Json.JsonConvert.SerializeObject(data));
                IList<CompactEntity> compact = JsonConvert.DeserializeObject<IList<CompactEntity>>(JsonConvert.SerializeObject(dataSource.Compact));
                IList<ProtocolEntity> protocol = JsonConvert.DeserializeObject<IList<ProtocolEntity>>(JsonConvert.SerializeObject(dataSource.Protocol));
                string CompactID = "";
                if (compact.Count > 0)
                {
                    CompactID = compact[0].ID;
                }
                else
                {
                    CompactID = "";
                }
                var filelist01 = new FileInfoBLL().GetFileList(Newtonsoft.Json.JsonConvert.SerializeObject(CompactID));
                var ProtocolID = "";
                if (protocol.Count > 0)
                {
                    ProtocolID = protocol[0].ID;
                }
                else
                {
                    ProtocolID = "";
                }

                var filelist02 = new FileInfoBLL().GetFileList(Newtonsoft.Json.JsonConvert.SerializeObject(ProtocolID));

                return new { Code = 0, Count = 1, Info = "操作成功", data = data, filelist01 = filelist01, filelist02 = filelist02 };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }


        }


        [HttpPost]
        public object SaveCompact([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

                var o1 = JsonConvert.DeserializeAnonymousType(res, new
                {
                    keyValue = string.Empty,
                    data = string.Empty,
                    file = string.Empty
                });
                var o = JsonConvert.DeserializeObject<CompactEntity>(dy.data);
                o.CREATEDATE = DateTime.Now;
                o.ID = dy.keyValue;
                
                //string str = JsonConvert.SerializeObject(dy.file);
                //IList<FileInfoEntity> filelist = JsonConvert.DeserializeObject<IList<FileInfoEntity>>(o1.file);
                CompactBLL compactbll = new CompactBLL();
                compactbll.SaveForm(dy.keyValue, o);
                FileInfoBLL FileInfoBLL = new FileInfoBLL();
                JArray jlist = JArray.Parse(dy.file);
                foreach (var item in jlist)
                {
                    var file = new FileInfoEntity();
                    file.RecId = dy.keyValue;
                    file.FileName = JObject.Parse(item.ToString())["FileName"].ToString();
                    file.FilePath = JObject.Parse(item.ToString())["FilePath"].ToString();
                    file.FileType= JObject.Parse(item.ToString())["FileType"].ToString();
                    file.FileSize= JObject.Parse(item.ToString())["FileSize"].ToString();
                    file.FileExtensions = JObject.Parse(item.ToString())["FileExtensions"].ToString();
                    FileInfoBLL.SaveForm("", file);
                }
                return new { Code = 0, Count = 1, Info = "保存成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        [HttpPost]
        public object SaveProtocol([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

                var o1 = JsonConvert.DeserializeAnonymousType(res, new
                {
                    keyValue = string.Empty,
                    data = string.Empty,
                    file = string.Empty
                });
                var o = JsonConvert.DeserializeObject<ProtocolEntity>(dy.data);
                o.CREATEDATE = DateTime.Now;
                o.ID = dy.keyValue;

                //string str = JsonConvert.SerializeObject(dy.file);
                //IList<FileInfoEntity> filelist = JsonConvert.DeserializeObject<IList<FileInfoEntity>>(o1.file);
                ProtocolBLL ProtocolBLL = new ProtocolBLL();
                ProtocolBLL.SaveForm(dy.keyValue, o);
                FileInfoBLL FileInfoBLL = new FileInfoBLL();
                JArray jlist = JArray.Parse(dy.file);
                foreach (var item in jlist)
                {
                    var file = new FileInfoEntity();
                    file.RecId = dy.keyValue;
                    file.FileName = JObject.Parse(item.ToString())["FileName"].ToString();
                    file.FilePath = JObject.Parse(item.ToString())["FilePath"].ToString();
                    file.FileType = JObject.Parse(item.ToString())["FileType"].ToString();
                    file.FileSize = JObject.Parse(item.ToString())["FileSize"].ToString();
                    file.FileExtensions = JObject.Parse(item.ToString())["FileExtensions"].ToString();
                    FileInfoBLL.SaveForm("", file);
                }
                return new { Code = 0, Count = 1, Info = "保存成功" };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

    }

}