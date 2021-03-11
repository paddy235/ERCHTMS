using BSFramework.Util;
using BSFramework.Util.Offices;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.JPush;
using ERCHTMS.Busines.NosaManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.NosaManage;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ERCHTMS.Web.Areas.NosaManage.Controllers
{
    /// <summary>
    /// 描 述：培训文件
    /// </summary>
    public class NosatrafilesController : MvcControllerBase
    {
        //private NosatrafilesBLL nosatrafilesbll = new NosatrafilesBLL();
        private NosatrafilesBsBLL nosatrafilesbll = new NosatrafilesBsBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private UserBLL userbll = new UserBLL();
        private NosatratypeBLL nosatratypebll = new NosatratypeBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();

        #region 视图功能        
        /// <summary>
        /// 附件页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult InfoFiles()
        {
            return View();
        }
        /// <summary>
        /// 导入页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.ehsDepartCode = "";
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            DataItemModel ehsDepart = dataitemdetailbll.GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            if (ehsDepart != null)//EHS部门Code
                ViewBag.ehsDepartCode = ehsDepart.ItemValue;
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = nosatrafilesbll.GetList(pagination, queryJson);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return Content(JsonData.ToJson());
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = nosatrafilesbll.GetEntity(keyValue);
            if (data != null)
            {//更新查阅次数
                data.ViewTimes = !data.ViewTimes.HasValue ? 1 : data.ViewTimes.Value + 1;
                nosatrafilesbll.SaveForm(data.ID, data);
            }
            //返回值
            var josnData = new
            {
                data               
            };

            return Content(josnData.ToJson());
        }        
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            nosatrafilesbll.RemoveForm(keyValue);
            DeleteFiles(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, NosatrafilesEntity entity)
        {
            var isNew = nosatrafilesbll.GetEntity(keyValue) == null;
            nosatrafilesbll.SaveForm(keyValue, entity);
            if (isNew == true)
                SendMessage(entity);

            return Success("操作成功。");
        }
        private void SendMessage(NosatrafilesEntity entity)
        {
            if (!entity.MsgUserId.IsNullOrWhiteSpace())
            {
                var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var aList = new UserBLL().GetListForCon(x => entity.MsgUserId.Contains(x.UserId)).Select(x => x.Account);
                MessageEntity msg = new MessageEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = string.Join(",",aList),
                    UserName = entity.MsgUserName,
                    SendTime = DateTime.Now,
                    SendUser = user.Account,
                    SendUserName = entity.CREATEUSERNAME,
                    Title = "新的NOSA培训文件提醒",
                    Content = string.Format("您有新的NOSA培训文件“{0}”，请即时查阅。", entity.FileName),
                    Category= "其它"
                };
                if (new MessageBLL().SaveForm("", msg))
                {
                    JPushApi.PublicMessage(msg);
                }
            }
        }
        #endregion

        #region 导入培训文件
        /// <summary>
        /// 导入培训文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportTraFiles()
        {
            int error = 0;
            int sussceed = 0;
            string message = "请选择格式正确的文件再导入!";
            string falseMessage = "";
            int count = HttpContext.Request.Files.Count;
            if (count > 0)
            {
                KeyValuePair<string, string> result = decompress(out message);
                if (!string.IsNullOrWhiteSpace(message))
                    return message;

                DataTable dt = ExcelHelper.ExcelImport(result.Key);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    object[] vals = dt.Rows[i].ItemArray;
                    if (IsEndRow(vals) == true)
                        break;
                    var msg = "";
                    if (Validate(i, vals, userbll, nosatratypebll,result.Value, out msg) == true)
                    {
                        KeyValuePair<NosatrafilesEntity,FileInfoEntity> kvEntity = GenEntity(vals, userbll, nosatratypebll,result.Value);
                        nosatrafilesbll.SaveForm(kvEntity.Key.ID, kvEntity.Key);
                        fileinfobll.SaveForm("", kvEntity.Value);
                        sussceed++;
                    }
                    else
                    {
                        falseMessage += "第" + (i + 1) + "行" + msg + "<br/>";
                        error++;
                    }
                }
                count = dt.Rows.Count;
                message = "共有" + count + "条记录,成功导入" + sussceed + "条，失败" + error + "条";
                message += "<br/>" + falseMessage;

                //删除临时文件
                var tempXlsfl = result.Key;
                if (System.IO.File.Exists(tempXlsfl))
                    System.IO.File.Delete(tempXlsfl);
                var tempZipfl = result.Key.Replace("xlsx", "zip").Replace("xls", "zip");
                if (System.IO.File.Exists(tempZipfl))
                    System.IO.File.Delete(tempZipfl);
            }

            return message;
        }
        private bool IsEndRow(object[] vals)
        {
            bool r = false;

            r = Array.TrueForAll(vals, x => (x == null || x == DBNull.Value || x.ToString() == ""));

            return r;
        }
        private bool IsNull(object obj)
        {
            return obj == null || obj == DBNull.Value || obj.ToString() == "";
        }
        private KeyValuePair<string,string> decompress(out string message)
        {
            string fileName = "";
            string decompressionDirectory = Server.MapPath("~/Resource/NosaTraFiles/") + DateTime.Now.ToString("yyyyMMdd") + "\\";

            message = "";
            if (HttpContext.Request.Files.Count != 2)
            {
                message = "请按正确的方式导入两个文件.";
            }
            else
            {
                HttpPostedFileBase file = HttpContext.Request.Files[0];
                HttpPostedFileBase file2 = HttpContext.Request.Files[1];
                if (string.IsNullOrEmpty(file.FileName) || string.IsNullOrEmpty(file2.FileName))
                {
                    message = "请选择文件格式正确的文件再导入!";
                }
                string sufx1 = System.IO.Path.GetExtension(file.FileName);
                string sufx2 = System.IO.Path.GetExtension(file2.FileName);
                string sufx = sufx1 + sufx2;
                if (!(sufx.Contains(".zip") && (sufx.Contains(".xls") || sufx.Contains(".xlsx"))))
                {
                    message = "请选择文件格式正确的文件再导入!";
                }
                if (string.IsNullOrWhiteSpace(message))
                {
                    string fstr = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string fileName1 = fstr + System.IO.Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName1));
                    string fileName2 = fstr + System.IO.Path.GetExtension(file2.FileName);
                    file2.SaveAs(Server.MapPath("~/Resource/temp/" + fileName2));

                    if (sufx1.Contains("zip"))
                    {
                        UnZip(Server.MapPath("~/Resource/temp/" + fileName1), decompressionDirectory, "", true);
                        fileName = Server.MapPath("~/Resource/temp/" + fileName2);
                    }
                    else
                    {
                        UnZip(Server.MapPath("~/Resource/temp/" + fileName2), decompressionDirectory, "", true);
                        fileName = Server.MapPath("~/Resource/temp/" + fileName1);
                    }
                }
            }

            return new KeyValuePair<string, string>(fileName, decompressionDirectory);
        }
        private void UnZip(string zipedFile, string strDirectory, string password, bool overWrite)
        {
            if (strDirectory == "")
                strDirectory = Directory.GetCurrentDirectory();

            if (!strDirectory.EndsWith("\\"))
                strDirectory = strDirectory + "\\";

            using (ZipInputStream s = new ZipInputStream(System.IO.File.OpenRead(zipedFile)))
            {
                s.Password = password;
                ZipEntry theEntry;

                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = "";
                    string pathToZip = "";
                    pathToZip = theEntry.Name;

                    if (pathToZip != "")
                        directoryName = Path.GetDirectoryName(pathToZip) + "\\";

                    string fileName = Path.GetFileName(pathToZip);

                    Directory.CreateDirectory(strDirectory + directoryName);
                    if (fileName != "")
                    {
                        if ((System.IO.File.Exists(strDirectory + directoryName + fileName) && overWrite) || (!System.IO.File.Exists(strDirectory + directoryName + fileName)))
                        {
                            using (FileStream streamWriter = System.IO.File.Create(strDirectory + directoryName + fileName))
                            {
                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = s.Read(data, 0, data.Length);
                                    if (size > 0)
                                        streamWriter.Write(data, 0, size);
                                    else
                                        break;
                                }
                                streamWriter.Close();
                            }
                        }
                    }
                }
                s.Close();
            }
        }
        private bool Validate(int index, object[] vals, UserBLL userbll, NosatratypeBLL nosatratypebll,string filePath, out string msg)
        {
            var r = true;
            var i = index + 1;
            msg = "";
            if (vals.Length < 7)
            {
                msg += "，格式不正确";
                r = false;
            }
            var obj = vals[1];
            if (IsNull(obj))
            {
                msg += "，文件名不能为空";
                r = false;
            }
            obj = vals[2];
            if (IsNull(obj))
            {
                msg += "，类别不能为空";
                r = false;
            }
            else
            {
                var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var list = nosatratypebll.GetList(String.Format(" and createuserorgcode='{0}' and name='{1}'", user.OrganizeCode, obj.ToString().Trim())).ToList();
                if (list.Count() == 0)
                {
                    msg += "，类别不存在";
                    r = false;
                }
            }
            obj = vals[3];
            if (IsNull(obj))
            {
                msg += "，发布单位（部门）不能为空";
                r = false;
            }

            obj = vals[4];
            if (IsNull(obj))
            {
                msg += "，发布人不能为空";
                r = false;
            }
            else if (!IsNull(vals[3]))
            {
                var entity = userbll.GetUserInfoByName(vals[3].ToString().Trim(), obj.ToString().Trim());
                if (entity == null)
                {
                    msg += "，发布单位（部门）中不存在相应的发布用户";
                    r = false;
                }
            }

            obj = vals[5];
            if (IsNull(obj))
            {
                msg += "，发布日期不能为空";
                r = false;
            }
            else 
            {
                DateTime pubDate = new DateTime();
                if (!DateTime.TryParse(obj.ToString(),out pubDate))
                {
                    msg += "，发布日期格式正确";
                    r = false;
                }
            }

            obj = vals[6];
            if (IsNull(obj))
            {
                msg += "，附件名称不能为空";
                r = false;
            }
            else
            {
                string fn = System.IO.Path.Combine(filePath, obj.ToString().Trim());
                if (!System.IO.File.Exists(fn))
                {
                    msg += "，附件名称不存在";
                    r = false;
                }
            }

            if (!string.IsNullOrWhiteSpace(msg))
            {
                msg += "。";
                r = false;
            }

            return r;
        }
        private KeyValuePair<NosatrafilesEntity, FileInfoEntity> GenEntity(object[] vals, UserBLL userbll, NosatratypeBLL nosatratypebll,string filePath)
        {
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            NosatrafilesEntity entity = new NosatrafilesEntity() { ID = Guid.NewGuid().ToString() };
            entity.FileName = vals[1].ToString().Trim();
            object obj = vals[2].ToString().Trim();
            var list = nosatratypebll.GetList(String.Format(" and createuserorgcode='{0}' and name='{1}'", user.OrganizeCode, obj.ToString())).ToList();
            entity.RefId = list[0].ID;
            entity.RefName = list[0].Name;            
            entity.PubDepartName = vals[3].ToString().Trim();
            entity.PubUserName = vals[4].ToString().Trim();
            var uEntity = userbll.GetUserInfoByName(entity.PubDepartName, entity.PubUserName);
            entity.PubUserId = uEntity.UserId;
            entity.PubDepartId = uEntity.DepartmentId;
            entity.PubDate = DateTime.Parse(vals[5].ToString());
            obj = vals[6];

            string fn = System.IO.Path.Combine(filePath, obj.ToString().Trim());
            var fileinfo = new FileInfo(fn);
            FileInfoEntity fileInfoEntity = new FileInfoEntity();
            fileInfoEntity.RecId = entity.ID; //关联ID
            fileInfoEntity.FolderId = "NosaTraFiles";
            fileInfoEntity.FileName = obj.ToString().Trim();
            fileInfoEntity.FilePath = string.Format("~/Resource/NosaTraFiles/{0}/{1}", DateTime.Now.ToString("yyyyMMdd"), obj.ToString().Trim());
            fileInfoEntity.FileSize = (Math.Round(decimal.Parse(fileinfo.Length.ToString()) / decimal.Parse("1024"), 2)).ToString();//文件大小（kb）
            fileInfoEntity.FileExtensions = fileinfo.Extension;
            fileInfoEntity.FileType = fileinfo.Extension.Replace(".", "");

            return new KeyValuePair<NosatrafilesEntity, FileInfoEntity>(entity, fileInfoEntity);
        }
        #endregion
    }
}
