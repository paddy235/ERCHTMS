using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.PersonManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.PersonManage;
using ERCHTMS.Entity.PublicInfoManage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    public class CertController : ApiController
    {
        CertificateBLL certificatebll = new CertificateBLL();
        DepartmentBLL deptBll = new DepartmentBLL();
        DataItemDetailBLL itemBll = new DataItemDetailBLL();
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        /// <summary>
        /// 获取证件状态
        /// </summary>
        /// <param name="endDate"></param>
        /// <param name="applyDate"></param>
        /// <returns></returns>
        private int GetStatus(string id,DateTime? endDate, DateTime? applyDate)
        {
            DateTime warnDate = DateTime.Now.AddMonths(3);
            DateTime nowDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            if (endDate != null)
            {
                if (endDate < nowDate)
                {
                    return 2;
                }
                else
                {
                    if (endDate < warnDate && endDate >= nowDate)
                    {
                        if (applyDate != null)
                        {
                            string count = deptBll.GetDataTable(string.Format("select count(1) from BIS_CERTAUDIT where certid='{0}' and audittype='复审' and  auditdate<to_date('{1}','yyyy-mm-dd hh24:mi:ss')", id, applyDate.Value.ToString("yyyy-MM-dd HH:mm:ss"))).Rows[0][0].ToString();
                            if (count=="0")
                            {
                                if (applyDate < nowDate)
                                {
                                    return 4;
                                }
                                else
                                {
                                    if (applyDate < warnDate && applyDate > nowDate)
                                    {
                                        return 3;
                                    }
                                }
                            }
                            else
                            {
                                return 0;
                            }
                            
                        }
                        else
                        {
                            return 1;
                        }
                    }
                }
            }
            if (applyDate != null)
            {
                string count = deptBll.GetDataTable(string.Format("select count(1) from BIS_CERTAUDIT where certid='{0}' and audittype='复审' and  auditdate<to_date('{1}','yyyy-mm-dd hh24:mi:ss')", id, applyDate.Value.ToString("yyyy-MM-dd HH:mm:ss"))).Rows[0][0].ToString();
                if (count == "0")
                {
                    if (applyDate < nowDate)
                    {
                        return 4;
                    }
                    else
                    {
                        if (applyDate < warnDate && applyDate > nowDate)
                        {
                            return 3;
                        }
                    }
                }
                else
                {
                    return 0;
                }
            }
            return 0;
        }
        [HttpGet]
        [HttpPost]
        /// <summary>
        /// 获取人员证件
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public object GetCertList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userId;
                string path = itemBll.GetItemValue("imgUrl");
                FileInfoBLL fileBll = new FileInfoBLL();

                var certs = new ERCHTMS.Busines.PersonManage.CertificateBLL().GetList(userId).Select(t => new { t.Id, t.CertName, t.CertType, t.CertNum, SendDate = t.SendDate.Value.ToString("yyyy-MM-dd"), StartDate = t.StartDate == null ? "" : t.StartDate.Value.ToString("yyyy-MM-dd"), EndDate = t.EndDate == null ? "" : t.EndDate.Value.ToString("yyyy-MM-dd"), ApplyDate = t.ApplyDate == null ? "" : t.ApplyDate.Value.ToString("yyyy-MM-dd"), t.Years, t.SendOrgan, Status = GetStatus(t.Id,t.EndDate, t.ApplyDate), files = fileBll.GetFileList(t.Id).Select(f => new { id = f.FileId, url = path + f.FilePath.Replace("~", "") }) }).ToList();//获取人员证书信息
                return new { code=0,info="获取数据成功",data=certs,count=certs.Count};
            }
            catch(Exception ex)
            {
                return new {code=1,info=ex.Message };
            }
        }
        [HttpPost]
        /// <summary>
        /// 新增或修改人员证书
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public object SaveUserCert()
        {
            try
            {
                string res = HttpContext.Current.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string id = "";
                //var keys = HttpContext.Current.Request.Form.AllKeys;
                //Hashtable ht = new Hashtable();
                //foreach(string key in keys)
                //{
                //    ht.Add(key, HttpContext.Current.Request.Form[key]);
                //}
                var content = Newtonsoft.Json.JsonConvert.SerializeObject(dy.data);
                CertificateEntity cert = Newtonsoft.Json.JsonConvert.DeserializeObject<CertificateEntity>(content);
                if (string.IsNullOrWhiteSpace(cert.Id))
                {
                    id = Guid.NewGuid().ToString();
                }
                else
                {
                    id = cert.Id;
                }
                string path = itemBll.GetItemValue("imgPath");
                string remark = "";
                if (!string.IsNullOrWhiteSpace(cert.Remark))
                {
                    remark = cert.Remark;
                    cert.Remark = "";
                }
                bool result = certificatebll.SaveForm(id, cert);
                FileInfoBLL fileInfoBLL = new FileInfoBLL();
                if (result)
                {
                    
                    HttpFileCollection files = HttpContext.Current.Request.Files;
                    for (int i = 0; i < files.AllKeys.Length; i++)
                    {
                        HttpPostedFile file = files[i];
                        //获取文件完整文件名(包含绝对路径)
                        //文件存放路径格式：/Resource/ResourceFile/{userId}{data}/{guid}.{后缀名}
                        string fileGuid = Guid.NewGuid().ToString();
                        long filesize = file.ContentLength;
                        string FileEextension = Path.GetExtension(file.FileName);
                        string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                        string virtualPath = string.Format("~/Resource/cert/{0}/{1}{2}", uploadDate, fileGuid, FileEextension);
                        string virtualPath1 = string.Format("\\Resource\\cert\\{0}\\{1}{2}", uploadDate, fileGuid, FileEextension);
                        string fullFileName = path + virtualPath1;
                        //创建文件夹
                        string path1 = Path.GetDirectoryName(fullFileName);
                        Directory.CreateDirectory(path1);
                        FileInfoEntity fileInfoEntity = new FileInfoEntity();
                        if (!System.IO.File.Exists(fullFileName))
                        {
                            //保存文件
                            file.SaveAs(fullFileName);
                        }
                        //文件信息写入数据库
                        fileInfoEntity.FileId = fileGuid;
                        fileInfoEntity.RecId = id; //关联ID
                        fileInfoEntity.FolderId = "cert";
                        fileInfoEntity.FileName = file.FileName;
                        fileInfoEntity.FilePath = virtualPath;
                        fileInfoEntity.FileSize = filesize.ToString();
                        fileInfoEntity.FileExtensions = FileEextension;
                        fileInfoEntity.FileType = FileEextension.Replace(".", "");
                        fileInfoBLL.SaveForm("", fileInfoEntity);
                    }
                    if (!string.IsNullOrWhiteSpace(remark))
                    {
                        deptBll.ExecuteSql(string.Format("delete from base_fileinfo where recid='{0}' and fileid in('{1}')", id, remark.Replace(",", "','")));
                    }
                    return new { code = 0, info = "操作成功", data = cert };
                }
                else
                {
                    return new { code = 1, info = "操作失败" };
                }
            }
            catch (Exception ex)
            {
                return new { code = 1, info = ex.Message };
            }
        }
        [HttpPost]
        /// <summary>
        /// 上传证件照片
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public object UploadCertImages()
        {
            try
            {
                string res = HttpContext.Current.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string id = dy.data.id;
                string path = itemBll.GetItemValue("imgPath");
                FileInfoBLL fileInfoBLL = new FileInfoBLL();
                HttpFileCollection files = HttpContext.Current.Request.Files;
                if (res.Contains("remark"))
                {
                    string remark = dy.data.remark;
                    deptBll.ExecuteSql(string.Format("delete from base_fileinfo where recid='{0}' and fileid in('{1}')", id, remark.Replace(",", "','")));
                }
                for (int i = 0; i < files.AllKeys.Length; i++)
                {
                    HttpPostedFile file = files[i];
                    //获取文件完整文件名(包含绝对路径)
                    //文件存放路径格式：/Resource/ResourceFile/{userId}{data}/{guid}.{后缀名}
                    string fileGuid = Guid.NewGuid().ToString();
                    long filesize = file.ContentLength;
                    string FileEextension = Path.GetExtension(file.FileName);
                    string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                    string virtualPath = string.Format("~/Resource/cert/{0}/{1}{2}", uploadDate, fileGuid, FileEextension);
                    string virtualPath1 = string.Format("\\Resource/cert\\{0}\\{1}{2}", uploadDate, fileGuid, FileEextension);
                    string fullFileName = path + virtualPath1;
                    //创建文件夹
                    string path1 = Path.GetDirectoryName(fullFileName);
                    Directory.CreateDirectory(path1);
                    FileInfoEntity fileInfoEntity = new FileInfoEntity();
                    if (!System.IO.File.Exists(fullFileName))
                    {
                        //保存文件
                        file.SaveAs(fullFileName);
                    }
                    //文件信息写入数据库
                    fileInfoEntity.FileId = fileGuid;
                    fileInfoEntity.RecId = id; //关联ID
                    fileInfoEntity.FolderId = "cert";
                    fileInfoEntity.FileName = file.FileName;
                    fileInfoEntity.FilePath = virtualPath;
                    fileInfoEntity.FileSize = filesize.ToString();
                    fileInfoEntity.FileExtensions = FileEextension;
                    fileInfoEntity.FileType = FileEextension.Replace(".", "");
                    fileInfoBLL.SaveForm("", fileInfoEntity);
                }
                return new { code = 0, info = "操作成功", data = id };
            }
            catch (Exception ex)
            {
                return new { code = 1, info = ex.Message };
            }
        }
        [HttpPost]
        [HttpGet]
        /// <summary>
        /// 获取证件照片
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public object GetCertImages([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string id = dy.data.id;
                string path = itemBll.GetItemValue("imgUrl");
                FileInfoBLL fileInfoBLL = new FileInfoBLL();
                var files = fileInfoBLL.GetFileList(id).Select(f => new { id = f.FileId, url = path + f.FilePath.Replace("~", "") });
                return new { code = 0, info = "操作成功", data = files };
            }
            catch (Exception ex)
            {
                return new { code = 1, info = ex.Message };
            }
        }
        [HttpPost]
        /// <summary>
        /// 删除人员证书
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public object DeleteCert([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string id = dy.data.id;
                certificatebll.RemoveForm(id);
                return new { code =0, info ="操作成功"};
            }
            catch (Exception ex)
            {
                return new { code = 1, info = ex.Message };
            }
        }
        [HttpPost]
        /// <summary>
        ///获取单条证书信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public object GetCertInfo([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string id = dy.data.id;
                string path = itemBll.GetItemValue("imgUrl");
                FileInfoBLL fileInfoBLL = new FileInfoBLL();
                CertificateEntity cert=certificatebll.GetEntity(id);
                return new
                {
                    code = 0,
                    info = "获取数据成功",
                    data = new
                    {
                        cert.Id,
                        cert.CertNum,
                        cert.CertName,
                        cert.CertType,
                        cert.WorkType,
                        cert.WorkItem,
                        SendDate = cert.SendDate.Value.ToString("yyyy-MM-dd"),
                        StartDate = cert.StartDate == null ? "" : cert.StartDate.Value.ToString("yyyy-MM-dd"),
                        cert.UserId,
                        cert.Grade,
                        cert.ZGName,
                        cert.Craft,
                        cert.Industry,
                        cert.UserType,
                        cert.Years,
                        cert.SendOrgan,
                        EndDate = cert.EndDate == null ? "" : cert.EndDate.Value.ToString("yyyy-MM-dd"),
                        ApplyDate = cert.ApplyDate == null ? "" : cert.ApplyDate.Value.ToString("yyyy-MM-dd"),
                        files = fileInfoBLL.GetFileList(id).Select(f => new {id=f.FileId, url = path + f.FilePath.Replace("~", "") })
                    }
                };
            }
            catch (Exception ex)
            {
                return new { code = 1, info = ex.Message };
            }
        }
    }
}