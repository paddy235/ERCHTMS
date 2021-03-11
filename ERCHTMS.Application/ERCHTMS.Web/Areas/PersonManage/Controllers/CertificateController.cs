using ERCHTMS.Entity.PersonManage;
using ERCHTMS.Busines.PersonManage;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Linq;
using System;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Collections.Generic;
using Aspose.Cells;
using System.Data;
using ERCHTMS.Busines.PublicInfoManage;
using System.Web;
using ERCHTMS.Entity.PublicInfoManage;
namespace ERCHTMS.Web.Areas.PersonManage.Controllers
{
    /// <summary>
    /// 描 述：人员证书
    /// </summary>
    public class CertificateController : MvcControllerBase
    {
        private CertificateBLL certificatebll = new CertificateBLL();
        UserBLL userBll = new UserBLL();
        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AuditForm()
        {
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
        [HttpGet]
        public ActionResult NewForm()
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
        /// 特种作业和特种设备证件导入
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NewImport()
        {
            return View();
        }
        /// <summary>
        /// 查看证书照片
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ViewImage()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Test()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string userId, Pagination pag, string certType = "")
        {
            var data = certificatebll.GetList(userId, pag);
            if (!string.IsNullOrWhiteSpace(certType))
            {
                data = data.Where(t => t.CertType == certType);
            }
            DepartmentBLL deptBll = new DepartmentBLL();
            foreach (CertificateEntity dr in data)
            {
                //判断证件有无照片
                int count = deptBll.GetDataTable(string.Format("select count(1) from base_fileinfo where recid='{0}'", dr.Id)).Rows[0][0].ToInt();
                if (count > 0)
                {
                    dr.FilePath = "1";
                }
            }
            return ToJsonResult(data);
        }
        /// <summary>
        ///获取证书复审记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAuditListJson(string certId)
        {
            var data = certificatebll.GetAuditList(certId);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = certificatebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取复审信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAuditFormJson(string keyValue)
        {
            var data = certificatebll.GetAuditEntity(keyValue);
            return ToJsonResult(data);
        }
        [HttpGet]
        public ActionResult GetItemListJson(string type, string code)
        {
            var rows = new DepartmentBLL().GetDataTable(string.Format("select t.ItemName,t.ItemValue,t.itemcode,Description from BASE_DATAITEMDETAIL t where itemcode='{1}' and t.enabledmark = 1 and t.deletemark = 0 and  t.itemid=(select itemid from base_dataitem a where a.itemcode='{0}') order by sortcode", type, code));
            return ToJsonResult(rows);
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
            certificatebll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 删除复审记录
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveAuditForm(string keyValue)
        {
            certificatebll.RemoveCertAudit(keyValue);
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
        public ActionResult SaveForm(string keyValue, CertificateEntity entity)
        {
            try
            {
                DepartmentBLL deptBll = new DepartmentBLL();
                //string number = deptBll.GetDataTable(string.Format("select count(1) from BIS_CERTIFICATE where certnum='{0}' and id!='{1}'", entity.CertNum, keyValue)).Rows[0][0].ToString();
                //if (number != "0")
                //{
                //    return Error("证书编号已存在！");
                //}
                //else
                //{
                certificatebll.SaveForm(keyValue, entity);
                return Success("操作成功。");
                //}

            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        /// <summary>
        /// 保存复审记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveAuditForm(CertAuditEntity entity)
        {
            try
            {
                DepartmentBLL deptBll = new DepartmentBLL();
                bool result = certificatebll.SaveCertAudit(entity);
                if (result)
                {
                    entity = certificatebll.GetAuditEntity(entity.Id);
                    CertificateEntity cert = certificatebll.GetEntity(entity.CertId);
                    if (cert != null)
                    {
                        cert.SendOrgan = entity.SendOrgan;
                        if (entity.AuditType == "到期换证")
                        {
                            cert.StartDate = entity.AuditDate;
                            cert.ApplyDate = entity.NextDate;
                            cert.EndDate = entity.EndDate;
                        }
                        if (entity.AuditType == "复审")
                        {
                            if (entity.Result == "合格")
                            {
                                cert.Status = 1;
                            }
                            else
                            {
                                cert.Status = 0;
                            }
                        }
                        if (cert.CertType == "特种设备作业人员证")
                        {
                            cert.EndDate = entity.EndDate;
                        }
                        if (certificatebll.SaveForm(entity.CertId, cert))
                        {
                            if (entity.AuditType == "到期换证")
                            {
                                int rad = new Random().Next(0, 1000000);
                                deptBll.ExecuteSql(string.Format("begin \r\n delete from base_fileinfo where recid='{2}';\r\n insert into base_fileinfo(fileid,folderid,filename,filepath,filesize,fileextensions,filetype,deletemark,enabledmark,recid) select fileid || '{0}',folderid,filename,filepath,filesize,fileextensions,filetype,0,1,'{2}' from base_fileinfo where recid='{1}';\r\n end \r\n commit;", rad, entity.Id, cert.Id));
                            }

                        }
                    }
                }
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }

        /// <summary>
        /// 解压zip文件
        /// </summary>
        /// <param name="zipedFile"></param>
        /// <param name="strDirectory"></param>
        /// <param name="password"></param>
        /// <param name="overWrite"></param>
        public bool UnZip(string zipedFile, string strDirectory, string password, bool overWrite)
        {
            if (strDirectory == "")
                strDirectory = Directory.GetCurrentDirectory();

            if (!strDirectory.EndsWith("\\"))
                strDirectory = strDirectory + "\\";
            try
            {
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
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 导入证件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportCert()
        {
            try
            {
                int mode = 1;
                int success = 0;
                string message = "请选择正确的Zip文件！";
                string falseMessage = "";
                int count = HttpContext.Request.Files.Count;
                if (count > 0)
                {
                    HttpPostedFileBase file = HttpContext.Request.Files[0];
                    if (string.IsNullOrEmpty(file.FileName))
                    {
                        return message;
                    }
                    string zipName = Path.GetFileNameWithoutExtension(file.FileName);

                    string dirName = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string path = Server.MapPath("~/Resource/temp/" + dirName + Path.GetExtension(file.FileName));
                    string destPath = Server.MapPath("~/Resource/cert/" + dirName);
                    string dir = "证件照片";
                    file.SaveAs(path);
                    if (System.IO.File.Exists(path))
                    {
                        if (UnZip(path, destPath, "", true))
                        {
                            FileInfo fi = new DirectoryInfo(destPath + "\\" + zipName).GetFiles("*.*").Where(t => t.Name.ToLower().EndsWith(".xls") || t.Name.ToLower().EndsWith(".xlxs")).FirstOrDefault();
                            if (fi == null)
                            {
                                message = "压缩包中没有检测到excel文件！";
                            }
                            else
                            {
                                message = "";
                                DirectoryInfo dirs = new DirectoryInfo(destPath + "\\" + zipName).GetDirectories().FirstOrDefault();
                                if (dirs != null)
                                {
                                    dir = dirs.Name;
                                }
                                List<int> lstErrors = new List<int>();
                                string fileName = fi.Name;
                                Workbook wb = new Aspose.Cells.Workbook();
                                wb.Open(destPath + "\\" + zipName + "\\" + fileName);
                                //导入特种作业人员证件
                                DepartmentBLL deptBll = new DepartmentBLL();
                                FileInfoBLL fileinfobll = new FileInfoBLL();
                                string certType = "特种作业操作证";
                                mode = 1;
                                string kind = "ryzylb";
                                string type = "ryzyxm";
                                Aspose.Cells.Cells cells = wb.Worksheets[0].Cells;
                                DataTable dt = new DataTable();
                                if (cells.MaxDataRow > 1)
                                {
                                    message = "开始导入特种作业人员证件,信息如下：<br />";
                                    dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn + 1, true);
                                  
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        //部门
                                        string deptName = dt.Rows[i]["单位/部门"].ToString().Trim();

                                        //姓名
                                        string userName = dt.Rows[i]["姓名"].ToString().Trim();

                                        //类型
                                        string workType = mode == 1 ? dt.Rows[i]["作业类别"].ToString().Trim() : dt.Rows[i]["种类"].ToString().Trim();
                                        //项目
                                        string workItem = mode == 1 ? dt.Rows[i]["操作项目"].ToString().Trim() : dt.Rows[i]["作业项目"].ToString().Trim();
                                        //发证机关
                                        string sendOrg = dt.Rows[i]["发证机关"].ToString().Trim();
                                        //发证日期
                                        string sendDate = dt.Rows[i]["初领日期"].ToString().Trim();
                                        //开始日期
                                        string startDate = dt.Rows[i]["有效期开始日期"].ToString().Trim();
                                        //结束日期
                                        string endDate = dt.Rows[i]["有效期结束日期"].ToString().Trim();
                                        //复审日期
                                        string applyDate = dt.Rows[i]["应复审日期"].ToString().Trim();
                                        //证书编号
                                        string certNum = dt.Rows[i]["证书编号"].ToString().Trim();
                                        //手机号
                                        string mobile = dt.Rows[i]["手机号"].ToString().Trim();
                                        //证书照片
                                        string photos = dt.Rows[i]["证书照片"].ToString().Trim().Trim(',');
                                        //有效期(年)
                                        string years = dt.Rows[i]["有效期(年)"].ToString().Trim();
                                        //证书名称
                                        string certName = string.Format("{0}-{1}-{2}", certType, workType, workItem);
                                        //项目代号
                                        string code = "";
                                        bool isOk = true;
                                        if (mode == 2)
                                        {
                                            if (dt.Columns.Contains("项目代号"))
                                            {
                                                code = dt.Rows[i]["项目代号"].ToString().Trim();
                                            }
                                            else
                                            {
                                                falseMessage = "模板不正确，缺少”项目代号“列";
                                                isOk = false;
                                                if (!lstErrors.Contains(i + 3))
                                                {
                                                    lstErrors.Add(i + 3);
                                                }
                                                continue;
                                            }
                                        }

                                        //---****值存在空验证*****--
                                        if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate) || string.IsNullOrEmpty(workType) || string.IsNullOrEmpty(workItem) || string.IsNullOrEmpty(certNum) || string.IsNullOrEmpty(sendOrg) || string.IsNullOrEmpty(sendDate) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(deptName) || string.IsNullOrEmpty(applyDate))
                                        {
                                            falseMessage += "</br>" + "第" + (i + 3) + "行值存在空,未能导入.";

                                            if (!lstErrors.Contains(i + 3))
                                            {
                                                lstErrors.Add(i + 3);
                                            }
                                            isOk = false;
                                            //error++;
                                            //continue;
                                        }
                                        int number = deptBll.GetDataTable(string.Format("select count(1) from base_dataitemdetail a where itemvalue='{0}' and a.itemid=(select itemid from base_dataitem  b where b.itemcode='ryzylb')", workType)).Rows[0][0].ToInt();
                                        if (number == 0)
                                        {
                                            falseMessage += "</br>" + "第" + (i + 3) + "行作业类别填写不正确,未能导入.";
                                            if (!lstErrors.Contains(i + 3))
                                            {
                                                lstErrors.Add(i + 3);
                                            }
                                            isOk = false;
                                            //error++;
                                            //continue;
                                        }
                                        number = deptBll.GetDataTable(string.Format("select count(1) from base_dataitemdetail a where itemvalue='{0}' and itemcode='{1}' and a.itemid=(select itemid from base_dataitem  b where b.itemcode='ryzyxm')", workItem, workType)).Rows[0][0].ToInt();
                                        if (number == 0)
                                        {
                                            falseMessage += "</br>" + "第" + (i + 3) + "行操作项目填写不正确,未能导入.";
                                            if (!lstErrors.Contains(i + 3))
                                            {
                                                lstErrors.Add(i + 3);
                                            }
                                            isOk = false;
                                            //error++;
                                            //continue;
                                        }
                                        //--手机号验证
                                        if (!BSFramework.Util.ValidateUtil.IsValidMobile(mobile) && !string.IsNullOrWhiteSpace(mobile))
                                        {
                                            falseMessage += "</br>" + "第" + (i + 3) + "行手机号格式有误,未能导入.";
                                            if (!lstErrors.Contains(i + 3))
                                            {
                                                lstErrors.Add(i + 3);
                                            }
                                            isOk = false;
                                            //error++;
                                            //continue;
                                        }
                                        else
                                        {
                                            //number = deptBll.GetDataTable(string.Format("select count(1) from BIS_CERTIFICATE where certnum='{0}'", certNum)).Rows[0][0].ToInt();
                                            //if (number >0)
                                            //{
                                            //    falseMessage += "</br>" + "第" + (i + 3) + "行证件编号（" + certNum + "）已存在 ,未能导入.";
                                            //    if (!lstErrors.Contains(i + 3))
                                            //    {
                                            //        lstErrors.Add(i + 3);
                                            //    }
                                            //    isOk = false;
                                            //    //error++;
                                            //    //continue;
                                            //}
                                            if (isOk)
                                            {
                                                string[] arr = deptName.Split('/');
                                                deptName = arr[arr.Length - 1];
                                                string sql = string.Format("select userid from v_userinfo where realname='{0}' and DEPTNAME='{1}'", userName, deptName);
                                                if (!string.IsNullOrWhiteSpace(mobile))
                                                {
                                                    sql += string.Format(" and mobile='{0}'", mobile);
                                                }
                                                DataTable dtUser = deptBll.GetDataTable(sql);
                                                if (dtUser.Rows.Count > 0)
                                                {
                                                    CertificateEntity cert = new CertificateEntity();
                                                    string id = Guid.NewGuid().ToString();
                                                    cert.Id = id;
                                                    cert.SendOrgan = sendOrg;
                                                    cert.SendDate = DateTime.Parse(sendDate);
                                                    cert.EndDate = DateTime.Parse(endDate);
                                                    cert.WorkType = workType;
                                                    cert.WorkItem = workItem;
                                                    cert.Years = years.ToInt();
                                                    cert.CertNum = certNum;
                                                    cert.CertType = certType;
                                                    cert.UserId = dtUser.Rows[0][0].ToString();
                                                    cert.ItemNum = code;
                                                    cert.StartDate = DateTime.Parse(startDate);
                                                    cert.ApplyDate = DateTime.Parse(applyDate);
                                                    cert.CertName = certName;
                                                    bool result = certificatebll.SaveForm(id, cert);
                                                    if (result)
                                                    {
                                                        success++;
                                                        if (photos.Length > 0)
                                                        {
                                                            arr = photos.Trim(',').Split(',');
                                                            foreach (string str in arr)
                                                            {
                                                                path = destPath + "\\" + zipName + "\\" + dir + "\\" + str;
                                                                if (System.IO.File.Exists(path))
                                                                {
                                                                    var fileinfo = new FileInfo(path);
                                                                    fileName = Guid.NewGuid().ToString() + Path.GetExtension(str);
                                                                    path = destPath + "\\" + zipName + "\\" + dir + "\\" + fileName;
                                                                    fileinfo.CopyTo(path);
                                                                    FileInfoEntity fileInfoEntity = new FileInfoEntity();
                                                                    fileInfoEntity.Create();
                                                                    fileInfoEntity.RecId = id;
                                                                    fileInfoEntity.FileName = str;
                                                                    fileInfoEntity.FilePath = "~/Resource/cert/" + dirName + "/" + zipName + "/" + dir + "/" + fileName;
                                                                    fileInfoEntity.FileSize = (Math.Round(decimal.Parse(fileinfo.Length.ToString()) / decimal.Parse("1024"), 2)).ToString();//文件大小（kb）
                                                                    fileInfoEntity.FileExtensions = fileinfo.Extension;
                                                                    fileInfoEntity.FileType = fileinfo.Extension.Replace(".", "");
                                                                    fileinfobll.SaveForm("", fileInfoEntity);

                                                                }
                                                                else
                                                                {
                                                                    falseMessage += "</br>" + "第" + (i + 3) + "行证件照片（" + str + "）不存在!";
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    falseMessage += "</br>" + "第" + (i + 3) + "行系统不存在该人员" + userName + "（" + deptName + ")信息！";
                                                    if (!lstErrors.Contains(i + 3))
                                                    {
                                                        lstErrors.Add(i + 3);
                                                    }
                                                    isOk = false;
                                                    //error++;
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                    }
                                    count = dt.Rows.Count;
                                    message += "共有" + count + "条记录,成功导入" + success + "条，失败" + lstErrors.Count + "条.";
                                    if (lstErrors.Count > 0)
                                    {
                                        message += "错误信息如下：" + falseMessage;
                                    }
                                }

                                //导入特种设备人员证件
                                success = 0;
                                falseMessage = "";
                                lstErrors.Clear();
                                cells = wb.Worksheets[1].Cells;
                                if (cells.Rows.Count > 2 && cells.MaxDataRow>1)
                                {
                                    message += "</br></br>开始导入特种设备人员证件,信息如下：</br>";
                                    mode = 2;
                                    certType = "特种设备作业人员证";
                                    kind = "tzzlb";
                                    type = "tzsbxm";
                                    dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn + 1, true);
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        //部门
                                        string deptName = dt.Rows[i]["单位/部门"].ToString().Trim();
                                        //姓名
                                        string userName = dt.Rows[i]["姓名"].ToString().Trim();
                                        //类型
                                        string workType = mode == 1 ? dt.Rows[i]["作业类别"].ToString().Trim() : dt.Rows[i]["种类"].ToString().Trim();
                                        //项目
                                        string workItem = mode == 1 ? dt.Rows[i]["操作项目"].ToString().Trim() : dt.Rows[i]["作业项目"].ToString().Trim();
                                        //发证机关
                                        string sendOrg = dt.Rows[i]["发证机关"].ToString().Trim();
                                        //发证日期
                                        string sendDate = dt.Rows[i]["初领日期"].ToString().Trim();
                                        //有效期
                                        string endDate = dt.Rows[i]["有效期限"].ToString().Trim();
                                        //复审日期
                                        string applyDate = "";
                                        //证书编号
                                        string certNum = dt.Rows[i]["证书编号"].ToString().Trim();
                                        //手机号
                                        string mobile = dt.Rows[i]["手机号"].ToString().Trim();
                                        //证书照片
                                        string photos = dt.Rows[i]["证书照片"].ToString().Trim().Trim(',');
                                        //有效期(年)
                                        string years = mode == 1 ? dt.Rows[i]["有效期(年)"].ToString().Trim() : dt.Rows[i]["复审周期(年)"].ToString().Trim();
                                        //项目代号
                                        string code = "";
                                        //证书名称
                                        string certName = string.Format("{0}-{1}-{2}", certType, workType, workItem);
                                        bool isOk = true;
                                        if (mode == 2)
                                        {
                                            if (dt.Columns.Contains("项目代号"))
                                            {
                                                code = dt.Rows[i]["项目代号"].ToString().Trim();
                                            }
                                            else
                                            {
                                                falseMessage = "模板不正确，缺少”项目代号“列";
                                                isOk = false;
                                                if (!lstErrors.Contains(i + 3))
                                                {
                                                    lstErrors.Add(i + 3);
                                                }
                                                continue;
                                            }
                                        }

                                        //---****值存在空验证*****--
                                        if (string.IsNullOrEmpty(endDate) || string.IsNullOrEmpty(workType) || string.IsNullOrEmpty(workItem) || string.IsNullOrEmpty(certNum) || string.IsNullOrEmpty(sendOrg) || string.IsNullOrEmpty(sendDate) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(deptName))
                                        {
                                            falseMessage += "</br>" + "第" + (i + 3) + "行值存在空,未能导入.";

                                            if (!lstErrors.Contains(i + 3))
                                            {
                                                lstErrors.Add(i + 3);
                                            }
                                            isOk = false;
                                            //error++;
                                            //continue;
                                        }
                                        if (mode == 1)
                                        {
                                            int number = deptBll.GetDataTable(string.Format("select count(1) from base_dataitemdetail a where itemvalue='{0}' and a.itemid=(select itemid from base_dataitem  b where b.itemcode='ryzylb')", workType)).Rows[0][0].ToInt();
                                            if (number == 0)
                                            {
                                                falseMessage += "</br>" + "第" + (i + 3) + "行作业类别填写不正确,未能导入.";
                                                if (!lstErrors.Contains(i + 3))
                                                {
                                                    lstErrors.Add(i + 3);
                                                }
                                                isOk = false;
                                                //error++;
                                                //continue;
                                            }
                                            number = deptBll.GetDataTable(string.Format("select count(1) from base_dataitemdetail a where itemvalue='{0}' and itemcode='{1}' and a.itemid=(select itemid from base_dataitem  b where b.itemcode='ryzyxm')", workItem, workType)).Rows[0][0].ToInt();
                                            if (number == 0)
                                            {
                                                falseMessage += "</br>" + "第" + (i + 3) + "行准操项目填写不正确,未能导入.";
                                                if (!lstErrors.Contains(i + 3))
                                                {
                                                    lstErrors.Add(i + 3);
                                                }
                                                isOk = false;
                                                //error++;
                                                //continue;
                                            }
                                        }
                                        if (mode == 2)
                                        {
                                            if (string.IsNullOrWhiteSpace(code))
                                            {
                                                falseMessage += "</br>" + "第" + (i + 3) + "行项目代号为空,未能导入.";
                                                if (!lstErrors.Contains(i + 3))
                                                {
                                                    lstErrors.Add(i + 3);
                                                }
                                                isOk = false;
                                                //error++;
                                                //continue;
                                            }
                                            int number = deptBll.GetDataTable(string.Format("select count(1) from base_dataitemdetail a where itemvalue='{0}' and a.itemid=(select itemid from base_dataitem  b where b.itemcode='tzzlb')", workType)).Rows[0][0].ToInt();
                                            if (number == 0)
                                            {
                                                falseMessage += "</br>" + "第" + (i + 3) + "行种类填写不正确,未能导入.";
                                                if (!lstErrors.Contains(i + 3))
                                                {
                                                    lstErrors.Add(i + 3);
                                                }
                                                isOk = false;
                                                //error++;
                                                //continue;
                                            }
                                            number = deptBll.GetDataTable(string.Format("select count(1) from base_dataitemdetail a where itemvalue='{0}' and itemcode='{1}' and a.itemid=(select itemid from base_dataitem  b where b.itemcode='tzsbxm')", workItem, workType)).Rows[0][0].ToInt();
                                            if (number == 0)
                                            {
                                                falseMessage += "</br>" + "第" + (i + 3) + "行作业项目填写不正确,未能导入.";
                                                if (!lstErrors.Contains(i + 3))
                                                {
                                                    lstErrors.Add(i + 3);
                                                }
                                                isOk = false;
                                                //error++;
                                                //continue;
                                            }
                                        }

                                        //--手机号验证
                                        if (!BSFramework.Util.ValidateUtil.IsValidMobile(mobile) && !string.IsNullOrWhiteSpace(mobile))
                                        {
                                            falseMessage += "</br>" + "第" + (i + 3) + "行手机号格式有误,未能导入.";
                                            if (!lstErrors.Contains(i + 3))
                                            {
                                                lstErrors.Add(i + 3);
                                            }
                                            isOk = false;
                                            //error++;
                                            //continue;
                                        }
                                        else
                                        {
                                            //string number = deptBll.GetDataTable(string.Format("select count(1) from BIS_CERTIFICATE where certnum='{0}'", certNum)).Rows[0][0].ToString();
                                            //if (number != "0")
                                            //{
                                            //    falseMessage += "</br>" + "第" + (i + 3) + "行证件编号（" + certNum + "）已存在 ,未能导入.";
                                            //    if (!lstErrors.Contains(i + 3))
                                            //    {
                                            //        lstErrors.Add(i + 3);
                                            //    }
                                            //    isOk = false;
                                            //    //error++;
                                            //    //continue;
                                            //}
                                            if (isOk)
                                            {
                                                string[] arr = deptName.Split('/');
                                                deptName = arr[arr.Length - 1];
                                                string sql = string.Format("select userid from v_userinfo where realname='{0}' and DEPTNAME='{1}'", userName, deptName);
                                                if (!string.IsNullOrWhiteSpace(mobile))
                                                {
                                                    sql += string.Format(" and mobile='{0}'", mobile);
                                                }
                                                DataTable dtUser = deptBll.GetDataTable(sql);
                                                if (dtUser.Rows.Count > 0)
                                                {
                                                    CertificateEntity cert = new CertificateEntity();
                                                    string id = Guid.NewGuid().ToString();
                                                    cert.Id = id;
                                                    cert.SendOrgan = sendOrg;
                                                    cert.SendDate = DateTime.Parse(sendDate);
                                                    cert.EndDate = DateTime.Parse(endDate);
                                                    cert.WorkType = workType;
                                                    cert.WorkItem = workItem;
                                                    cert.Years = years.ToInt();
                                                    cert.CertNum = certNum;
                                                    cert.CertType = certType;
                                                    cert.UserId = dtUser.Rows[0][0].ToString();
                                                    cert.ItemNum = code;
                                                    cert.CertName = certName;
                                                    bool result = certificatebll.SaveForm(id, cert);
                                                    if (result)
                                                    {
                                                        success++;
                                                        if (photos.Length > 0)
                                                        {
                                                            arr = photos.Trim(',').Split(',');
                                                            foreach (string str in arr)
                                                            {
                                                                path = destPath + "\\" + zipName + "\\" + dir + "\\" + str;
                                                                if (System.IO.File.Exists(path))
                                                                {
                                                                    var fileinfo = new FileInfo(path);
                                                                    fileName = Guid.NewGuid().ToString() + Path.GetExtension(str);
                                                                    path = destPath + "\\" + zipName + "\\" + dir + "\\" + fileName;
                                                                    fileinfo.CopyTo(path);
                                                                    FileInfoEntity fileInfoEntity = new FileInfoEntity();
                                                                    fileInfoEntity.Create();
                                                                    fileInfoEntity.RecId = id;
                                                                    fileInfoEntity.FileName = str;
                                                                    fileInfoEntity.FilePath = "~/Resource/cert/" + dirName + "/" + zipName + "/" + dir + "/" + fileName;
                                                                    fileInfoEntity.FileSize = (Math.Round(decimal.Parse(fileinfo.Length.ToString()) / decimal.Parse("1024"), 2)).ToString();//文件大小（kb）
                                                                    fileInfoEntity.FileExtensions = fileinfo.Extension;
                                                                    fileInfoEntity.FileType = fileinfo.Extension.Replace(".", "");
                                                                    fileinfobll.SaveForm("", fileInfoEntity);

                                                                }
                                                                else
                                                                {
                                                                    falseMessage += "</br>" + "第" + (i + 3) + "行证件照片（" + str + "）不存在!";
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    falseMessage += "</br>" + "第" + (i + 3) + "行系统不存在该人员" + userName + "（" + deptName + ")信息！";
                                                    if (!lstErrors.Contains(i + 3))
                                                    {
                                                        lstErrors.Add(i + 3);
                                                    }
                                                    isOk = false;
                                                    //error++;
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                    }
                                    count = dt.Rows.Count;
                                    message += "共有" + count + "条记录,成功导入" + success + "条，失败" + lstErrors.Count + "条。";
                                    if (lstErrors.Count > 0)
                                    {
                                        message += "错误信息如下：" + falseMessage;
                                    }
                                }
                                  //导入职业资格证
                                cells = wb.Worksheets[2].Cells;
                                if (cells.Rows.Count > 2 && cells.MaxDataRow > 1)
                                {
                                    certType = "职业资格证";
                                    success = 0;
                                    falseMessage = "";
                                    lstErrors.Clear();
                                    message += "</br></br>开始导入职业资格证,信息如下：</br>";
                                    dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn + 1, true);
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        //部门
                                        string deptName = dt.Rows[i]["单位/部门"].ToString().Trim();
                                        //姓名
                                        string userName = dt.Rows[i]["姓名"].ToString().Trim();

                                        //发证机关
                                        string sendOrg = dt.Rows[i]["发证机关"].ToString().Trim();
                                        //发证日期
                                        string sendDate = dt.Rows[i]["发证日期"].ToString().Trim();
                                        //有效期
                                        string endDate = dt.Rows[i]["有效期限"].ToString().Trim();
                                        //证书编号
                                        string certNum = dt.Rows[i]["证书编号"].ToString().Trim();
                                        //手机号
                                        string mobile = dt.Rows[i]["手机号"].ToString().Trim();
                                        //证书照片
                                        string photos = dt.Rows[i]["证书照片"].ToString().Trim().Trim(',');
                                        //有效期(年)
                                        string years = dt.Rows[i]["有效期(年)"].ToString().Trim();
                                        //等级
                                        string grade = dt.Rows[i]["等级"].ToString().Trim();
                                        //工种
                                        string craft = dt.Rows[i]["工种"].ToString().Trim();

                                        //证书名称
                                        string certName = string.Format("{0}-{1}-{2}", certType, craft, grade);
                                        bool isOk = true;

                                        //---****值存在空验证*****--
                                        if (string.IsNullOrEmpty(endDate) || string.IsNullOrEmpty(certType) || string.IsNullOrEmpty(certName) || string.IsNullOrEmpty(certNum) || string.IsNullOrEmpty(sendOrg) || string.IsNullOrEmpty(sendDate) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(deptName) || string.IsNullOrEmpty(craft))
                                        {
                                            falseMessage += "</br>" + "第" + (i + 3) + "行值存在空,未能导入.";

                                            if (!lstErrors.Contains(i + 3))
                                            {
                                                lstErrors.Add(i + 3);
                                            }
                                            isOk = false;
                                            //error++;
                                            //continue;
                                        }
                                        if (!BSFramework.Util.ValidateUtil.IsValidMobile(mobile) && !string.IsNullOrWhiteSpace(mobile))
                                        {
                                            falseMessage += "</br>" + "第" + (i + 3) + "行手机号格式有误,未能导入.";
                                            if (!lstErrors.Contains(i + 3))
                                            {
                                                lstErrors.Add(i + 3);
                                            }
                                            isOk = false;
                                            //error++;
                                            //continue;
                                        }
                                        else
                                        {
                                            //string number = deptBll.GetDataTable(string.Format("select count(1) from BIS_CERTIFICATE where certnum='{0}'", certNum)).Rows[0][0].ToString();
                                            //if (number != "0")
                                            //{
                                            //    falseMessage += "</br>" + "第" + (i + 3) + "行证件编号（" + certNum + "）已存在 ,未能导入.";
                                            //    if (!lstErrors.Contains(i + 3))
                                            //    {
                                            //        lstErrors.Add(i + 3);
                                            //    }
                                            //    isOk = false;
                                            //    //error++;
                                            //    //continue;
                                            //}
                                            if (isOk)
                                            {
                                                string[] arr = deptName.Split('/');
                                                deptName = arr[arr.Length - 1];
                                                string sql = string.Format("select userid from v_userinfo where realname='{0}' and DEPTNAME='{1}'", userName, deptName);
                                                if (!string.IsNullOrWhiteSpace(mobile))
                                                {
                                                    sql += string.Format(" and mobile='{0}'", mobile);
                                                }
                                                DataTable dtUser = deptBll.GetDataTable(sql);
                                                if (dtUser.Rows.Count > 0)
                                                {
                                                    CertificateEntity cert = new CertificateEntity();
                                                    string id = Guid.NewGuid().ToString();
                                                    cert.Id = id;
                                                    cert.SendOrgan = sendOrg;
                                                    cert.SendDate = DateTime.Parse(sendDate);
                                                    cert.EndDate = DateTime.Parse(endDate);
                                                    cert.Years = years.ToInt();
                                                    cert.CertNum = certNum;
                                                    cert.CertType = certType;
                                                    cert.UserId = dtUser.Rows[0][0].ToString();
                                                    cert.CertName = certName;
                                                    cert.Grade = grade;
                                                    cert.CertName = certName;
                                                    cert.Craft = craft;
                                                    bool result = certificatebll.SaveForm(id, cert);
                                                    if (result)
                                                    {
                                                        success++;
                                                        if (photos.Length > 0)
                                                        {
                                                            arr = photos.Trim(',').Split(',');
                                                            foreach (string str in arr)
                                                            {
                                                                path = destPath + "\\" + zipName + "\\" + dir + "\\" + str;
                                                                if (System.IO.File.Exists(path))
                                                                {
                                                                    var fileinfo = new FileInfo(path);
                                                                    fileName = Guid.NewGuid().ToString() + Path.GetExtension(str);
                                                                    path = destPath + "\\" + zipName + "\\" + dir + "\\" + fileName;
                                                                    fileinfo.CopyTo(path);
                                                                    FileInfoEntity fileInfoEntity = new FileInfoEntity();
                                                                    fileInfoEntity.Create();
                                                                    fileInfoEntity.RecId = id;
                                                                    fileInfoEntity.FileName = str;
                                                                    fileInfoEntity.FilePath = "~/Resource/cert/" + dirName + "/" + zipName + "/" + dir + "/" + fileName;
                                                                    fileInfoEntity.FileSize = (Math.Round(decimal.Parse(fileinfo.Length.ToString()) / decimal.Parse("1024"), 2)).ToString();//文件大小（kb）
                                                                    fileInfoEntity.FileExtensions = fileinfo.Extension;
                                                                    fileInfoEntity.FileType = fileinfo.Extension.Replace(".", "");
                                                                    fileinfobll.SaveForm("", fileInfoEntity);

                                                                }
                                                                else
                                                                {
                                                                    falseMessage += "</br>" + "第" + (i + 3) + "行证件照片（" + str + "）不存在!";
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    falseMessage += "</br>" + "第" + (i + 3) + "行系统不存在该人员" + userName + "（" + deptName + ")信息！";
                                                    if (!lstErrors.Contains(i + 3))
                                                    {
                                                        lstErrors.Add(i + 3);
                                                    }
                                                    isOk = false;
                                                    //error++;
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                    }
                                    count = dt.Rows.Count;
                                    message += "共有" + count + "条记录,成功导入" + success + "条，失败" + lstErrors.Count + "条。";
                                    if (lstErrors.Count > 0)
                                    {
                                        message += "错误信息如下：" + falseMessage;
                                    }
                                }
                               //导入专业技术资格证
                                certType = "专业技术资格证";
                                success = 0;
                                falseMessage = "";
                                lstErrors.Clear();
                                cells = wb.Worksheets[3].Cells;
                                if (cells.Rows.Count > 2 && cells.MaxDataRow>1)
                                {
                                    message += "</br></br>开始导入专业技术资格证,信息如下：</br>";
                                    dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn + 1, true);
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        //部门
                                        string deptName = dt.Rows[i]["单位/部门"].ToString().Trim();
                                        //姓名
                                        string userName = dt.Rows[i]["姓名"].ToString().Trim();

                                        //发证机关
                                        string sendOrg = dt.Rows[i]["发证机关"].ToString().Trim();
                                        //发证日期
                                        string sendDate = dt.Rows[i]["发证日期"].ToString().Trim();
                                        //有效期
                                        string endDate = dt.Rows[i]["有效期限"].ToString().Trim();
                                        //证书编号
                                        string certNum = dt.Rows[i]["证书编号"].ToString().Trim();
                                        //手机号
                                        string mobile = dt.Rows[i]["手机号"].ToString().Trim();
                                        //证书照片
                                        string photos = dt.Rows[i]["证书照片"].ToString().Trim().Trim(',');
                                        //有效期(年)
                                        string years = dt.Rows[i]["有效期(年)"].ToString().Trim();
                                        //资格名称
                                        string zgname = dt.Rows[i]["资格名称"].ToString().Trim();
                                        //证书名称
                                        string certName = string.Format("{0}-{1}", certType, zgname);
                                        bool isOk = true;

                                        //---****值存在空验证*****--
                                        if (string.IsNullOrEmpty(endDate) || string.IsNullOrEmpty(certType) || string.IsNullOrEmpty(certName) || string.IsNullOrEmpty(certNum) || string.IsNullOrEmpty(sendOrg) || string.IsNullOrEmpty(sendDate) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(deptName))
                                        {
                                            falseMessage += "</br>" + "第" + (i + 3) + "行值存在空,未能导入.";

                                            if (!lstErrors.Contains(i + 3))
                                            {
                                                lstErrors.Add(i + 3);
                                            }
                                            isOk = false;
                                            //error++;
                                            //continue;
                                        }
                                        //--手机号验证
                                        if (!BSFramework.Util.ValidateUtil.IsValidMobile(mobile) && !string.IsNullOrWhiteSpace(mobile))
                                        {
                                            falseMessage += "</br>" + "第" + (i + 3) + "行手机号格式有误,未能导入.";
                                            if (!lstErrors.Contains(i + 3))
                                            {
                                                lstErrors.Add(i + 3);
                                            }
                                            isOk = false;
                                            //error++;
                                            //continue;
                                        }
                                        else
                                        {
                                            //string number = deptBll.GetDataTable(string.Format("select count(1) from BIS_CERTIFICATE where certnum='{0}'", certNum)).Rows[0][0].ToString();
                                            //if (number != "0")
                                            //{
                                            //    falseMessage += "</br>" + "第" + (i + 3) + "行证件编号（" + certNum + "）已存在 ,未能导入.";
                                            //    if (!lstErrors.Contains(i + 3))
                                            //    {
                                            //        lstErrors.Add(i + 3);
                                            //    }
                                            //    isOk = false;
                                            //    //error++;
                                            //    //continue;
                                            //}
                                            if (isOk)
                                            {
                                                string[] arr = deptName.Split('/');
                                                deptName = arr[arr.Length - 1];
                                                string sql = string.Format("select userid from v_userinfo where realname='{0}' and DEPTNAME='{1}'", userName, deptName);
                                                if (!string.IsNullOrWhiteSpace(mobile))
                                                {
                                                    sql += string.Format(" and mobile='{0}'", mobile);
                                                }
                                                DataTable dtUser = deptBll.GetDataTable(sql);
                                                if (dtUser.Rows.Count > 0)
                                                {
                                                    CertificateEntity cert = new CertificateEntity();
                                                    string id = Guid.NewGuid().ToString();
                                                    cert.Id = id;
                                                    cert.SendOrgan = sendOrg;
                                                    cert.SendDate = DateTime.Parse(sendDate);
                                                    cert.EndDate = DateTime.Parse(endDate);
                                                    cert.Years = years.ToInt();
                                                    cert.CertNum = certNum;
                                                    cert.CertType = certType;
                                                    cert.UserId = dtUser.Rows[0][0].ToString();
                                                    cert.CertName = certName;
                                                    cert.ZGName = zgname;
                                                    cert.CertName = certName;
                                                    bool result = certificatebll.SaveForm(id, cert);
                                                    if (result)
                                                    {
                                                        success++;
                                                        if (photos.Length > 0)
                                                        {
                                                            arr = photos.Trim(',').Split(',');
                                                            foreach (string str in arr)
                                                            {
                                                                path = destPath + "\\" + zipName + "\\" + dir + "\\" + str;
                                                                if (System.IO.File.Exists(path))
                                                                {
                                                                    var fileinfo = new FileInfo(path);
                                                                    fileName = Guid.NewGuid().ToString() + Path.GetExtension(str);
                                                                    path = destPath + "\\" + zipName + "\\" + dir + "\\" + fileName;
                                                                    fileinfo.CopyTo(path);
                                                                    FileInfoEntity fileInfoEntity = new FileInfoEntity();
                                                                    fileInfoEntity.Create();
                                                                    fileInfoEntity.RecId = id;
                                                                    fileInfoEntity.FileName = str;
                                                                    fileInfoEntity.FilePath = "~/Resource/cert/" + dirName + "/" + zipName + "/" + dir + "/" + fileName;
                                                                    fileInfoEntity.FileSize = (Math.Round(decimal.Parse(fileinfo.Length.ToString()) / decimal.Parse("1024"), 2)).ToString();//文件大小（kb）
                                                                    fileInfoEntity.FileExtensions = fileinfo.Extension;
                                                                    fileInfoEntity.FileType = fileinfo.Extension.Replace(".", "");
                                                                    fileinfobll.SaveForm("", fileInfoEntity);

                                                                }
                                                                else
                                                                {
                                                                    falseMessage += "</br>" + "第" + (i + 3) + "行证件照片（" + str + "）不存在!";
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    falseMessage += "</br>" + "第" + (i + 3) + "行系统不存在该人员" + userName + "（" + deptName + ")信息！";
                                                    if (!lstErrors.Contains(i + 3))
                                                    {
                                                        lstErrors.Add(i + 3);
                                                    }
                                                    isOk = false;
                                                    //error++;
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                    }
                                    count = dt.Rows.Count;

                                    message += "共有" + count + "条记录,成功导入" + success + "条，失败" + lstErrors.Count + "条。";
                                    if (lstErrors.Count > 0)
                                    {
                                        message += "错误信息如下：" + falseMessage;
                                    }
                                }
                              
                                //导入安全生产知识和管理能力考核合格证
                                certType = "安全生产知识和管理能力考核合格证";
                                success = 0;
                                falseMessage = "";
                                lstErrors.Clear();
                                cells = wb.Worksheets[4].Cells;
                                if (cells.Rows.Count > 2 && cells.MaxDataRow > 1)
                                {
                                    message += "</br></br>开始导入安全生产知识和管理能力考核合格证,信息如下：</br>";
                                    dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn + 1, true);
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        //部门
                                        string deptName = dt.Rows[i]["单位/部门"].ToString().Trim();
                                        //姓名
                                        string userName = dt.Rows[i]["姓名"].ToString().Trim();

                                        //发证机关
                                        string sendOrg = dt.Rows[i]["发证机关"].ToString().Trim();
                                        //发证日期
                                        string sendDate = dt.Rows[i]["初领日期"].ToString().Trim();
                                        //开始日期
                                        string startDate = dt.Rows[i]["初领日期"].ToString().Trim();
                                        //有效期
                                        string endDate = dt.Rows[i]["有效期限"].ToString().Trim();
                                        //证书编号
                                        string certNum = dt.Rows[i]["证书编号"].ToString().Trim();
                                        //手机号
                                        string mobile = dt.Rows[i]["手机号"].ToString().Trim();
                                        //证书照片
                                        string photos = dt.Rows[i]["证书照片"].ToString().Trim().Trim(',');
                                        //有效期(年)
                                        string years = dt.Rows[i]["有效期(年)"].ToString().Trim();
                                        //人员类型
                                        string userType = dt.Rows[i]["人员类型"].ToString().Trim();
                                        //行业类别
                                        string industry = dt.Rows[i]["行业类别"].ToString().Trim();
                                        //证书名称
                                        string certName = string.Format("{0}-{1}-{2}", certType, userType, industry);
                                        bool isOk = true;

                                        //---****值存在空验证*****--
                                        if (string.IsNullOrEmpty(endDate) || string.IsNullOrEmpty(certType) || string.IsNullOrEmpty(certName) || string.IsNullOrEmpty(certNum) || string.IsNullOrEmpty(sendOrg) || string.IsNullOrEmpty(sendDate) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(deptName))
                                        {
                                            falseMessage += "</br>" + "第" + (i + 3) + "行值存在空,未能导入.";

                                            if (!lstErrors.Contains(i + 3))
                                            {
                                                lstErrors.Add(i + 3);
                                            }
                                            isOk = false;
                                            //error++;
                                            //continue;
                                        }
                                        //--手机号验证
                                        if (!BSFramework.Util.ValidateUtil.IsValidMobile(mobile) && !string.IsNullOrWhiteSpace(mobile))
                                        {
                                            falseMessage += "</br>" + "第" + (i + 3) + "行手机号格式有误,未能导入.";
                                            if (!lstErrors.Contains(i + 3))
                                            {
                                                lstErrors.Add(i + 3);
                                            }
                                            isOk = false;
                                            //error++;
                                            //continue;
                                        }
                                        else
                                        {
                                            //string number = deptBll.GetDataTable(string.Format("select count(1) from BIS_CERTIFICATE where certnum='{0}'", certNum)).Rows[0][0].ToString();
                                            //if (number != "0")
                                            //{
                                            //    falseMessage += "</br>" + "第" + (i + 3) + "行证件编号（" + certNum + "）已存在 ,未能导入.";
                                            //    if (!lstErrors.Contains(i + 3))
                                            //    {
                                            //        lstErrors.Add(i + 3);
                                            //    }
                                            //    isOk = false;
                                            //    //error++;
                                            //    //continue;
                                            //}
                                            if (isOk)
                                            {
                                                string[] arr = deptName.Split('/');
                                                deptName = arr[arr.Length - 1];
                                                string sql = string.Format("select userid from v_userinfo where realname='{0}' and DEPTNAME='{1}'", userName, deptName);
                                                if (!string.IsNullOrWhiteSpace(mobile))
                                                {
                                                    sql += string.Format(" and mobile='{0}'", mobile);
                                                }
                                                DataTable dtUser = deptBll.GetDataTable(sql);
                                                if (dtUser.Rows.Count > 0)
                                                {
                                                    CertificateEntity cert = new CertificateEntity();
                                                    string id = Guid.NewGuid().ToString();
                                                    cert.Id = id;
                                                    cert.SendOrgan = sendOrg;
                                                    cert.SendDate = cert.StartDate = DateTime.Parse(sendDate);
                                                    cert.EndDate = DateTime.Parse(endDate);
                                                    cert.Years = years.ToInt();
                                                    cert.CertNum = certNum;
                                                    cert.CertType = certType;
                                                    cert.UserId = dtUser.Rows[0][0].ToString();
                                                    cert.CertName = certName;
                                                    cert.UserType = userType;
                                                    cert.Industry = industry;
                                                    cert.CertName = certName;
                                                    bool result = certificatebll.SaveForm(id, cert);
                                                    if (result)
                                                    {
                                                        success++;
                                                        if (photos.Length > 0)
                                                        {
                                                            arr = photos.Trim(',').Split(',');
                                                            foreach (string str in arr)
                                                            {
                                                                path = destPath + "\\" + zipName + "\\" + dir + "\\" + str;
                                                                if (System.IO.File.Exists(path))
                                                                {
                                                                    var fileinfo = new FileInfo(path);
                                                                    fileName = Guid.NewGuid().ToString() + Path.GetExtension(str);
                                                                    path = destPath + "\\" + zipName + "\\" + dir + "\\" + fileName;
                                                                    fileinfo.CopyTo(path);
                                                                    FileInfoEntity fileInfoEntity = new FileInfoEntity();
                                                                    fileInfoEntity.Create();
                                                                    fileInfoEntity.RecId = id;
                                                                    fileInfoEntity.FileName = str;
                                                                    fileInfoEntity.FilePath = "~/Resource/cert/" + dirName + "/" + zipName + "/" + dir + "/" + fileName;
                                                                    fileInfoEntity.FileSize = (Math.Round(decimal.Parse(fileinfo.Length.ToString()) / decimal.Parse("1024"), 2)).ToString();//文件大小（kb）
                                                                    fileInfoEntity.FileExtensions = fileinfo.Extension;
                                                                    fileInfoEntity.FileType = fileinfo.Extension.Replace(".", "");
                                                                    fileinfobll.SaveForm("", fileInfoEntity);

                                                                }
                                                                else
                                                                {
                                                                    falseMessage += "</br>" + "第" + (i + 3) + "行证件照片（" + str + "）不存在!";
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    falseMessage += "</br>" + "第" + (i + 3) + "行系统不存在该人员" + userName + "（" + deptName + ")信息！";
                                                    if (!lstErrors.Contains(i + 3))
                                                    {
                                                        lstErrors.Add(i + 3);
                                                    }
                                                    isOk = false;
                                                    //error++;
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                    }
                                    count = dt.Rows.Count;
                                    message += "共有" + count + "条记录,成功导入" + success + "条，失败" + lstErrors.Count + "条。";
                                    if (lstErrors.Count > 0)
                                    {
                                        message += "错误信息如下：" + falseMessage;
                                    }
                             
                                }
                                //导入其他类人员证件
                                success = 0;
                                falseMessage = "";
                                lstErrors.Clear();

                             
                                cells = wb.Worksheets[5].Cells;
                                if (cells.Rows.Count > 2 && cells.MaxDataRow > 1)
                                {
                                    message += "</br></br>开始导入其他类人员证件,信息如下：</br>";
                                    dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn + 1, true);
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        //部门
                                        string deptName = dt.Rows[i]["单位/部门"].ToString().Trim();
                                        //姓名
                                        string userName = dt.Rows[i]["姓名"].ToString().Trim();
                                        //证书类型
                                        certType = dt.Rows[i]["证件类型"].ToString().Trim();
                                        //证书名称
                                        string certName = dt.Rows[i]["证件名称"].ToString().Trim();
                                        //发证机关
                                        string sendOrg = dt.Rows[i]["发证机关"].ToString().Trim();
                                        //发证日期
                                        string sendDate = dt.Rows[i]["初领日期"].ToString().Trim();
                                        //有效期
                                        string endDate = dt.Rows[i]["有效期限"].ToString().Trim();
                                        //证书编号
                                        string certNum = dt.Rows[i]["证书编号"].ToString().Trim();
                                        //手机号
                                        string mobile = dt.Rows[i]["手机号"].ToString().Trim();
                                        //证书照片
                                        string photos = dt.Rows[i]["证书照片"].ToString().Trim().Trim(',');
                                        //有效期(年)
                                        string years = dt.Rows[i]["有效期(年)"].ToString().Trim();
                                        bool isOk = true;

                                        //---****值存在空验证*****--
                                        if (string.IsNullOrEmpty(endDate) || string.IsNullOrEmpty(certType) || string.IsNullOrEmpty(certName) || string.IsNullOrEmpty(certNum) || string.IsNullOrEmpty(sendOrg) || string.IsNullOrEmpty(sendDate) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(deptName))
                                        {
                                            falseMessage += "</br>" + "第" + (i + 3) + "行值存在空,未能导入.";

                                            if (!lstErrors.Contains(i + 3))
                                            {
                                                lstErrors.Add(i + 3);
                                            }
                                            isOk = false;
                                            //error++;
                                            //continue;
                                        }
                                        //--手机号验证

                                        if (!BSFramework.Util.ValidateUtil.IsValidMobile(mobile) && !string.IsNullOrWhiteSpace(mobile))
                                        {
                                            falseMessage += "</br>" + "第" + (i + 3) + "行手机号格式有误,未能导入.";
                                            if (!lstErrors.Contains(i + 3))
                                            {
                                                lstErrors.Add(i + 3);
                                            }
                                            isOk = false;
                                            //error++;
                                            //continue;
                                        }
                                        else
                                        {
                                            //string number = deptBll.GetDataTable(string.Format("select count(1) from BIS_CERTIFICATE where certnum='{0}'", certNum)).Rows[0][0].ToString();
                                            //if (number != "0")
                                            //{
                                            //    falseMessage += "</br>" + "第" + (i + 3) + "行证件编号（" + certNum + "）已存在 ,未能导入.";
                                            //    if (!lstErrors.Contains(i + 3))
                                            //    {
                                            //        lstErrors.Add(i + 3);
                                            //    }
                                            //    isOk = false;
                                            //    //error++;
                                            //    //continue;
                                            //}
                                            if (isOk)
                                            {
                                                string[] arr = deptName.Split('/');
                                                deptName = arr[arr.Length - 1];
                                                string sql = string.Format("select userid from v_userinfo where realname='{0}' and DEPTNAME='{1}'", userName, deptName);
                                                if (!string.IsNullOrWhiteSpace(mobile))
                                                {
                                                    sql += string.Format(" and mobile='{0}'", mobile);
                                                }
                                                DataTable dtUser = deptBll.GetDataTable(sql);
                                                if (dtUser.Rows.Count > 0)
                                                {
                                                    CertificateEntity cert = new CertificateEntity();
                                                    string id = Guid.NewGuid().ToString();
                                                    cert.Id = id;
                                                    cert.SendOrgan = sendOrg;
                                                    cert.SendDate = DateTime.Parse(sendDate);
                                                    cert.EndDate = DateTime.Parse(endDate);
                                                    cert.Years = years.ToInt();
                                                    cert.CertNum = certNum;
                                                    cert.CertType = certType;
                                                    cert.UserId = dtUser.Rows[0][0].ToString();
                                                    cert.CertName = certName;
                                                    bool result = certificatebll.SaveForm(id, cert);
                                                    if (result)
                                                    {
                                                        success++;
                                                        if (photos.Length > 0)
                                                        {
                                                            arr = photos.Trim(',').Split(',');
                                                            foreach (string str in arr)
                                                            {
                                                                path = destPath + "\\" + zipName + "\\" + dir + "\\" + str;
                                                                if (System.IO.File.Exists(path))
                                                                {
                                                                    var fileinfo = new FileInfo(path);
                                                                    fileName = Guid.NewGuid().ToString() + Path.GetExtension(str);
                                                                    path = destPath + "\\" + zipName + "\\" + dir + "\\" + fileName;
                                                                    fileinfo.CopyTo(path);
                                                                    FileInfoEntity fileInfoEntity = new FileInfoEntity();
                                                                    fileInfoEntity.Create();
                                                                    fileInfoEntity.RecId = id;
                                                                    fileInfoEntity.FileName = str;
                                                                    fileInfoEntity.FilePath = "~/Resource/cert/" + dirName + "/" + zipName + "/" + dir + "/" + fileName;
                                                                    fileInfoEntity.FileSize = (Math.Round(decimal.Parse(fileinfo.Length.ToString()) / decimal.Parse("1024"), 2)).ToString();//文件大小（kb）
                                                                    fileInfoEntity.FileExtensions = fileinfo.Extension;
                                                                    fileInfoEntity.FileType = fileinfo.Extension.Replace(".", "");
                                                                    fileinfobll.SaveForm("", fileInfoEntity);

                                                                }
                                                                else
                                                                {
                                                                    falseMessage += "</br>" + "第" + (i + 3) + "行证件照片（" + str + "）不存在!";
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    falseMessage += "</br>" + "第" + (i + 3) + "行系统不存在该人员" + userName + "（" + deptName + ")信息！";
                                                    if (!lstErrors.Contains(i + 3))
                                                    {
                                                        lstErrors.Add(i + 3);
                                                    }
                                                    isOk = false;
                                                    //error++;
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                    }
                                    count = dt.Rows.Count;
                                    message += "共有" + count + "条记录,成功导入" + success + "条，失败" + lstErrors.Count + "条。";
                                    if (lstErrors.Count > 0)
                                    {
                                        message += "错误信息如下：" + falseMessage;
                                    }
                                }
                            }

                        }
                        else
                        {
                            message = "导入证件失败,请稍后再试";
                        }
                    }
                    else
                    {
                        message = "导入证件失败,请稍后再试";
                    }
                }
                return message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        /// <summary>
        /// 导入特种作业或特种设备证件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportTZCert(int mode = 1)
        {
            try
            {
                int success = 0;
                string message = "请选择正确的Zip文件！";
                string falseMessage = "";
                int count = HttpContext.Request.Files.Count;
                if (count > 0)
                {
                    HttpPostedFileBase file = HttpContext.Request.Files[0];
                    if (string.IsNullOrEmpty(file.FileName))
                    {
                        return message;
                    }
                    string zipName = Path.GetFileNameWithoutExtension(file.FileName);
                    string certType = mode == 1 ? "特种作业操作证" : "特种设备作业人员证";
                    string kind = mode == 1 ? "ryzylb" : "tzzlb";
                    string type = mode == 1 ? "ryzyxm" : "tzsbxm";
                    string dirName = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string path = Server.MapPath("~/Resource/temp/" + dirName + Path.GetExtension(file.FileName));
                    string destPath = Server.MapPath("~/Resource/cert/" + dirName);
                    string dir = "证件照片";
                    file.SaveAs(path);
                    if (System.IO.File.Exists(path))
                    {
                        if (UnZip(path, destPath, "", true))
                        {
                            FileInfo fi = new DirectoryInfo(destPath + "\\" + zipName).GetFiles("*.*").Where(t => t.Name.ToLower().EndsWith(".xls") || t.Name.ToLower().EndsWith(".xlxs")).FirstOrDefault();
                            if (fi == null)
                            {
                                message = "压缩包中没有检测到excel文件！";
                            }
                            else
                            {
                                DirectoryInfo dirs = new DirectoryInfo(destPath + "\\" + zipName).GetDirectories().FirstOrDefault();
                                if (dirs != null)
                                {
                                    dir = dirs.Name;
                                }
                                List<int> lstErrors = new List<int>();
                                string fileName = fi.Name;
                                Workbook wb = new Aspose.Cells.Workbook();
                                wb.Open(destPath + "\\" + zipName + "\\" + fileName);
                                Aspose.Cells.Cells cells = wb.Worksheets[0].Cells;
                                if (cells.Rows.Count > 2 && cells.MaxDataRow>1)
                                {
                                    DataTable dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn + 1, true);
                                    DepartmentBLL deptBll = new DepartmentBLL();
                                    FileInfoBLL fileinfobll = new FileInfoBLL();
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        //姓名
                                        string userName = dt.Rows[i]["姓名"].ToString().Trim();
                                        //部门
                                        string deptName = dt.Rows[i]["单位/部门"].ToString().Trim();

                                        //证书名称
                                        string workType = mode == 1 ? dt.Rows[i]["作业类别"].ToString().Trim() : dt.Rows[i]["种类"].ToString().Trim();
                                        //类型
                                        string workItem = mode == 1 ? dt.Rows[i]["操作项目"].ToString().Trim() : dt.Rows[i]["作业项目"].ToString().Trim();
                                        //发证机关
                                        string sendOrg = dt.Rows[i]["发证机关"].ToString().Trim();
                                        //发证日期
                                        string sendDate = dt.Rows[i]["初领日期"].ToString().Trim();
                                        //开始日期
                                        string startDate = "";
                                        if (mode == 1)
                                        {
                                            startDate = dt.Rows[i]["有效期开始日期"].ToString().Trim();
                                        }
                                        //结束日期
                                        string endDate = mode == 1 ? dt.Rows[i]["有效期结束日期"].ToString().Trim() : dt.Rows[i]["有效期限"].ToString().Trim();
                                        //复审日期
                                        string applyDate = mode == 1 ? dt.Rows[i]["应复审日期"].ToString().Trim() : "";
                                        //证书编号
                                        string certNum = dt.Rows[i]["证书编号"].ToString().Trim();
                                        //手机号
                                        string mobile = dt.Rows[i]["手机号"].ToString().Trim();
                                        //证书照片
                                        string photos = dt.Rows[i]["证书照片"].ToString().Trim().Trim(',');
                                        //有效期(年)
                                        string years = mode == 1 ? dt.Rows[i]["有效期(年)"].ToString().Trim() : dt.Rows[i]["复审周期(年)"].ToString().Trim();
                                        //项目代号
                                        string code = "";
                                        string certName = string.Format("{0}-{1}-{2}", certType, workType, workItem);
                                        bool isOk = true;
                                        if (mode == 2)
                                        {
                                            if (dt.Columns.Contains("项目代号"))
                                            {
                                                code = dt.Rows[i]["项目代号"].ToString().Trim();
                                            }
                                            else
                                            {
                                                falseMessage = "模板不正确，缺少”项目代号“列";
                                                isOk = false;
                                                if (!lstErrors.Contains(i + 3))
                                                {
                                                    lstErrors.Add(i + 3);
                                                }
                                                continue;
                                            }
                                        }
                                        if (mode == 1)
                                        {
                                            if (string.IsNullOrEmpty(workType) || string.IsNullOrEmpty(workItem) || string.IsNullOrEmpty(certNum) || string.IsNullOrEmpty(sendOrg) || string.IsNullOrEmpty(sendDate) || string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(deptName) || string.IsNullOrEmpty(applyDate))
                                            {
                                                falseMessage += "</br>" + "第" + (i + 3) + "行值存在空,未能导入.";

                                                if (!lstErrors.Contains(i + 3))
                                                {
                                                    lstErrors.Add(i + 3);
                                                }
                                                isOk = false;
                                                //error++;
                                                //continue;
                                            }
                                            int number = deptBll.GetDataTable(string.Format("select count(1) from base_dataitemdetail a where itemvalue='{0}' and a.itemid=(select itemid from base_dataitem  b where b.itemcode='ryzylb')", workType)).Rows[0][0].ToInt();
                                            if (number == 0)
                                            {
                                                falseMessage += "</br>" + "第" + (i + 3) + "行作业类别填写不正确,未能导入.";
                                                if (!lstErrors.Contains(i + 3))
                                                {
                                                    lstErrors.Add(i + 3);
                                                }
                                                isOk = false;
                                                //error++;
                                                //continue;
                                            }
                                            number = deptBll.GetDataTable(string.Format("select count(1) from base_dataitemdetail a where itemvalue='{0}' and itemcode='{1}' and a.itemid=(select itemid from base_dataitem  b where b.itemcode='ryzyxm')", workItem, workType)).Rows[0][0].ToInt();
                                            if (number == 0)
                                            {
                                                falseMessage += "</br>" + "第" + (i + 3) + "行操作项目填写不正确,未能导入.";
                                                if (!lstErrors.Contains(i + 3))
                                                {
                                                    lstErrors.Add(i + 3);
                                                }
                                                isOk = false;
                                                //error++;
                                                //continue;
                                            }
                                        }
                                        if (mode == 2)
                                        {
                                            if (string.IsNullOrEmpty(workType) || string.IsNullOrEmpty(workItem) || string.IsNullOrEmpty(certNum) || string.IsNullOrEmpty(sendOrg) || string.IsNullOrEmpty(sendDate) || string.IsNullOrEmpty(endDate) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(deptName))
                                            {
                                                falseMessage += "</br>" + "第" + (i + 3) + "行值存在空,未能导入.";

                                                if (!lstErrors.Contains(i + 3))
                                                {
                                                    lstErrors.Add(i + 3);
                                                }
                                                isOk = false;
                                                //error++;
                                                //continue;
                                            }
                                            //if (string.IsNullOrWhiteSpace(code))
                                            //{
                                            //    falseMessage += "</br>" + "第" + (i + 3) + "行项目代号为空,未能导入.";
                                            //    if (!lstErrors.Contains(i + 3))
                                            //    {
                                            //        lstErrors.Add(i + 3);
                                            //    }
                                            //    isOk = false;
                                            //    //error++;
                                            //    //continue;
                                            //}
                                            int number = deptBll.GetDataTable(string.Format("select count(1) from base_dataitemdetail a where itemvalue='{0}' and a.itemid=(select itemid from base_dataitem  b where b.itemcode='tzzlb')", workType)).Rows[0][0].ToInt();
                                            if (number == 0)
                                            {
                                                falseMessage += "</br>" + "第" + (i + 3) + "行种类填写不正确,未能导入.";
                                                if (!lstErrors.Contains(i + 3))
                                                {
                                                    lstErrors.Add(i + 3);
                                                }
                                                isOk = false;
                                                //error++;
                                                //continue;
                                            }
                                            number = deptBll.GetDataTable(string.Format("select count(1) from base_dataitemdetail a where itemvalue='{0}' and itemcode='{1}' and a.itemid=(select itemid from base_dataitem  b where b.itemcode='tzsbxm')", workItem, workType)).Rows[0][0].ToInt();
                                            if (number == 0)
                                            {
                                                falseMessage += "</br>" + "第" + (i + 3) + "行作业项目填写不正确,未能导入.";
                                                if (!lstErrors.Contains(i + 3))
                                                {
                                                    lstErrors.Add(i + 3);
                                                }
                                                isOk = false;
                                                //error++;
                                                //continue;
                                            }
                                        }

                                        //--手机号验证
                                        if (!BSFramework.Util.ValidateUtil.IsValidMobile(mobile) && !string.IsNullOrWhiteSpace(mobile))
                                        {
                                            falseMessage += "</br>" + "第" + (i + 3) + "行手机号格式有误,未能导入.";
                                            if (!lstErrors.Contains(i + 3))
                                            {
                                                lstErrors.Add(i + 3);
                                            }
                                            isOk = false;
                                            //error++;
                                            //continue;
                                        }
                                        else
                                        {
                                            //string number = deptBll.GetDataTable(string.Format("select count(1) from BIS_CERTIFICATE where certnum='{0}'", certNum)).Rows[0][0].ToString();
                                            //if (number != "0")
                                            //{
                                            //    falseMessage += "</br>" + "第" + (i +3) + "行证件编号（" + certNum + "）已存在 ,未能导入.";
                                            //    if (!lstErrors.Contains(i + 3))
                                            //    {
                                            //        lstErrors.Add(i + 3);
                                            //    }
                                            //    isOk = false;
                                            //    //error++;
                                            //    //continue;
                                            //}
                                            if (isOk)
                                            {
                                                string[] arr = deptName.Split('/');
                                                deptName = arr[arr.Length - 1];
                                                string sql = string.Format("select userid from v_userinfo where realname='{0}' and DEPTNAME='{1}'", userName, deptName);
                                                if (!string.IsNullOrWhiteSpace(mobile))
                                                {
                                                    sql += string.Format(" and mobile='{0}'", mobile);
                                                }
                                                DataTable dtUser = deptBll.GetDataTable(sql);
                                                if (dtUser.Rows.Count > 0)
                                                {
                                                    CertificateEntity cert = new CertificateEntity();
                                                    string id = Guid.NewGuid().ToString();
                                                    cert.Id = id;
                                                    cert.SendOrgan = sendOrg;
                                                    cert.SendDate = DateTime.Parse(sendDate);
                                                    cert.EndDate = DateTime.Parse(endDate);
                                                    cert.WorkType = workType;
                                                    cert.WorkItem = workItem;
                                                    cert.Years = years.ToInt();
                                                    cert.CertNum = certNum;
                                                    cert.CertType = certType;
                                                    cert.UserId = dtUser.Rows[0][0].ToString();
                                                    cert.CertName = certName;
                                                    cert.ItemNum = code;
                                                    if (!string.IsNullOrWhiteSpace(applyDate))
                                                    {
                                                        cert.ApplyDate = DateTime.Parse(applyDate);
                                                    }
                                                    if (!string.IsNullOrWhiteSpace(startDate))
                                                    {
                                                        cert.StartDate = startDate.ToDate();
                                                    }
                                                    bool result = certificatebll.SaveForm(id, cert);
                                                    if (result)
                                                    {
                                                        success++;
                                                        if (photos.Length > 0)
                                                        {
                                                            arr = photos.Trim(',').Split(',');
                                                            foreach (string str in arr)
                                                            {
                                                                path = destPath + "\\" + zipName + "\\" + dir + "\\" + str;
                                                                if (System.IO.File.Exists(path))
                                                                {
                                                                    var fileinfo = new FileInfo(path);
                                                                    fileName = Guid.NewGuid().ToString() + Path.GetExtension(str);
                                                                    path = destPath + "\\" + zipName + "\\" + dir + "\\" + fileName;
                                                                    fileinfo.CopyTo(path);
                                                                    FileInfoEntity fileInfoEntity = new FileInfoEntity();
                                                                    fileInfoEntity.Create();
                                                                    fileInfoEntity.RecId = id;
                                                                    fileInfoEntity.FileName = str;
                                                                    fileInfoEntity.FilePath = "~/Resource/cert/" + dirName + "/" + zipName + "/" + dir + "/" + fileName;
                                                                    fileInfoEntity.FileSize = (Math.Round(decimal.Parse(fileinfo.Length.ToString()) / decimal.Parse("1024"), 2)).ToString();//文件大小（kb）
                                                                    fileInfoEntity.FileExtensions = fileinfo.Extension;
                                                                    fileInfoEntity.FileType = fileinfo.Extension.Replace(".", "");
                                                                    fileinfobll.SaveForm("", fileInfoEntity);

                                                                }
                                                                else
                                                                {
                                                                    falseMessage += "</br>" + "第" + (i + 3) + "行证件照片（" + str + "）不存在!";
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    falseMessage += "</br>" + "第" + (i + 3) + "行不存在该手机号(" + mobile + ")的用户信息！";
                                                    if (!lstErrors.Contains(i + 3))
                                                    {
                                                        lstErrors.Add(i + 3);
                                                    }
                                                    isOk = false;
                                                    //error++;
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                    }

                                    count = dt.Rows.Count;
                                    message = "共有" + count + "条记录,成功导入" + success + "条，失败" + lstErrors.Count + "条";
                                    message += "</br>" + falseMessage;
                                }
                            }

                        }
                        else
                        {
                            message = "导入证件失败,请稍后再试";
                        }
                    }
                    else
                    {
                        message = "导入证件失败,请稍后再试";
                    }
                }
                return message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        #endregion
    }
}
