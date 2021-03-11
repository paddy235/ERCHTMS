using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.Busines.EmergencyPlatform;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ERCHTMS.Busines.BaseManage;
using System.Linq;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SystemManage;

namespace ERCHTMS.Web.Areas.EmergencyPlatform.Controllers
{
    /// <summary>
    /// 描 述：应急预案
    /// </summary>
    public class ReserverplanController : MvcControllerBase
    {
        private ReserverplanBLL reserverplanbll = new ReserverplanBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();
        #region 视图功能



        /// <summary>
        /// 文件页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Files()
        {
            return View();
        }


        public ActionResult Select()
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
        public ActionResult Import()
        {
            return View();
        }

        #endregion

        #region 获取数据


        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>   
        //[HandlerMonitor(3, "分页查询用户信息!")]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            queryJson = queryJson ?? "";
            pagination.p_kid = "ID";
            pagination.p_fields = "createuserid,createuserdeptcode,createuserorgcode, Files,Name,PLANTYPENAME,ISAUDITNAME,DATATIME_BZ,DEPARTNAME_BZ,CREATEUSERNAME,USERID_BZ,DEPARTID_BZ,USERNAME_BZ,FILEPS,PLANTYPE";
            pagination.p_tablename = "(select * from MAE_RESERVERPLAN order by CREATEDATE,DATATIME_BZ) t";
            pagination.conditionJson = "1=1"; Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "CREATEUSERDEPTCODE", "CREATEUSERORGCODE");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }
            }

            var watch = CommonHelper.TimerStart();
            var data = reserverplanbll.GetPageList(pagination, queryJson);
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
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = reserverplanbll.GetList(queryJson);
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
            var data = reserverplanbll.GetEntity(keyValue);
            return ToJsonResult(data);
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
            foreach (var item in keyValue.Split(','))
            {
                reserverplanbll.RemoveForm(item);

            }
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
        public ActionResult SaveForm(string keyValue, ReserverplanEntity entity)
        {
            reserverplanbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion


        #region 数据导出
        /// <summary>
        /// 导出用户列表
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "应急预案")]
        public ActionResult ExportReserverplanList(string condition, string queryJson)
        {
            Pagination pagination = new Pagination();
            queryJson = queryJson ?? "";
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "ID";
            pagination.p_fields = " name, plantypename,departname_bz,username_bz,datatime_bz,isauditname";
            pagination.p_tablename = "V_MAE_RESERVERPLAN t";
            pagination.conditionJson = "1=1";
            pagination.sidx = "ID";
            #region 权限校验
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "CREATEUSERDEPTCODE", "CREATEUSERORGCODE");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }

            }
            #endregion
            var data = reserverplanbll.GetPageList(pagination, queryJson);

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "应急预案";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "应急预案.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "name", ExcelColumn = "应急预案名称", Width = 50 });
            listColumnEntity.Add(new ColumnEntity() { Column = "plantypename", ExcelColumn = "应急预案类型" });
            listColumnEntity.Add(new ColumnEntity() { Column = "departname_bz", ExcelColumn = "编制部门" });
            listColumnEntity.Add(new ColumnEntity() { Column = "username_bz", ExcelColumn = "编制人" });
            listColumnEntity.Add(new ColumnEntity() { Column = "datatime_bz", ExcelColumn = "编制时间" });
            listColumnEntity.Add(new ColumnEntity() { Column = "isauditname", ExcelColumn = "是否评审" });
            excelconfig.ColumnEntity = listColumnEntity;

            //调用导出方法
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("导出成功。");
        }

         [HandlerLogin(LoginMode.Ignore)]
        [HandlerAuthorize(PermissionMode.Ignore)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(5, "导入应急预案")]
        public string ImportReserverplanData()
        {
            try
            {
                if (OperatorProvider.Provider.Current().IsSystem)
                {
                    return "超级管理员无此操作权限";
                }
                int error = 0;
                string message = "请选择格式正确的文件再导入!";

                string falseMessage = "";
                int count = HttpContext.Request.Files.Count;
                if (count > 0)
                {
                    count = 0;
                    if (HttpContext.Request.Files.Count != 2)
                    {
                        return "请按正确的方式导入两个文件.";
                    }
                    HttpPostedFileBase file = HttpContext.Request.Files[0];
                    HttpPostedFileBase file2 = HttpContext.Request.Files[1];
                    if (string.IsNullOrEmpty(file.FileName) || string.IsNullOrEmpty(file2.FileName))
                    {
                        return message;
                    }
                    Boolean isZip1 = file.FileName.Substring(file.FileName.IndexOf('.')).Contains("zip");//第一个文件是否为Zip格式
                    Boolean isZip2 = file2.FileName.Substring(file2.FileName.IndexOf('.')).Contains("zip");//第二个文件是否为Zip格式
                    if ((isZip1 || isZip2) == false || (isZip1 && isZip2) == true)
                    {
                        return message;
                    }
                    string fileName1 = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName1));
                    string fileName2 = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file2.FileName);
                    file2.SaveAs(Server.MapPath("~/Resource/temp/" + fileName2));
                    string decompressionDirectory = Server.MapPath("~/Resource/decompression/") + DateTime.Now.ToString("yyyyMMddhhmmssfff") + "\\";
                    Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                    if (isZip1)
                    {
                        UnZip(Server.MapPath("~/Resource/temp/" + fileName1), decompressionDirectory, "", true);
                        wb.Open(Server.MapPath("~/Resource/temp/" + fileName2));
                    }
                    else
                    {
                        UnZip(Server.MapPath("~/Resource/temp/" + fileName2), decompressionDirectory, "", true);
                        wb.Open(Server.MapPath("~/Resource/temp/" + fileName1));
                    }
                    Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                    var sheet = wb.Worksheets[0];
                    if (sheet.Cells[1, 0].StringValue != "应急预案名称" || sheet.Cells[1, 1].StringValue != "应急预案类型" 
                         || sheet.Cells[1, 2].StringValue != "编制部门" || sheet.Cells[1, 4].StringValue != "编制时间"
                         || sheet.Cells[1, 5].StringValue != "审核部门" || sheet.Cells[1, 6].StringValue != "审核人"
                         || sheet.Cells[1, 7].StringValue != "审核时间" || sheet.Cells[1, 8].StringValue != "单位性质"
                         || sheet.Cells[1, 9].StringValue != "是否评审" || sheet.Cells[1, 10].StringValue != "附件")
                    {
                        return message;
                    }
                    for (int i = 2; i <= sheet.Cells.MaxDataRow; i++)
                    {
                        count ++;
                        if (string.IsNullOrWhiteSpace(sheet.Cells[i, 0].StringValue) || string.IsNullOrWhiteSpace(sheet.Cells[i, 1].StringValue)
                         || string.IsNullOrWhiteSpace(sheet.Cells[i, 2].StringValue) || string.IsNullOrWhiteSpace(sheet.Cells[i, 4].StringValue)
                         || string.IsNullOrWhiteSpace(sheet.Cells[i, 6].StringValue) || string.IsNullOrWhiteSpace(sheet.Cells[i, 6].StringValue)
                         || string.IsNullOrWhiteSpace(sheet.Cells[i, 7].StringValue) || string.IsNullOrWhiteSpace(sheet.Cells[i, 8].StringValue)
                         || string.IsNullOrWhiteSpace(sheet.Cells[i, 9].StringValue) || string.IsNullOrWhiteSpace(sheet.Cells[i, 10].StringValue))
                        {
                            falseMessage += "</br>" + "第" + (i + 1) + "行未按模板要求填写,未能导入.";
                            error++;
                            continue;
                        }
                        ReserverplanEntity planEntity = new ReserverplanEntity();
                        planEntity.NAME = sheet.Cells[i, 0].StringValue;
                        planEntity.PLANTYPENAME = sheet.Cells[i, 1].StringValue;
                        var dataItem1 = new DataItemDetailBLL().GetDataItemListByItemCode("'MAE_PlanType'").Where(x => x.ItemName == sheet.Cells[i, 1].StringValue).ToList().FirstOrDefault();
                        if (dataItem1 != null) {
                            planEntity.PLANTYPE = dataItem1.ItemValue;
                        }
                        //编制部门
                        var dept = new DepartmentBLL().GetList().Where(x => x.FullName == sheet.Cells[i, 2].StringValue).ToList().FirstOrDefault();
                        if (dept == null) {
                            falseMessage += "</br>" + "第" + (i + 1) + "行编制部门不正确,未能导入.";
                            error++;
                            continue;
                        }
                        planEntity.DEPARTID_BZ = dept.DepartmentId;
                        planEntity.DEPARTNAME_BZ = dept.FullName;
                        //编制人
                        planEntity.USERNAME_BZ = sheet.Cells[i, 3].StringValue;
                        //var userEntity = new UserBLL().GetList().Where(x => x.RealName == sheet.Cells[2, 3].StringValue).ToList().FirstOrDefault();
                        //if (userEntity != null) {
                        //    planEntity.USERID_BZ = userEntity.UserId;
                        //}
                        //审核部门
                        var auditDept = new DepartmentBLL().GetList().Where(x => x.FullName == sheet.Cells[i, 5].StringValue).ToList().FirstOrDefault();
                        if (auditDept == null)
                        {
                            falseMessage += "</br>" + "第" + (i + 1) + "行审核部门不正确,未能导入.";
                            error++;
                            continue;
                        }
                        planEntity.DEPARTID_SH = auditDept.DepartmentId;
                        planEntity.DEPARTNAME_SH = auditDept.FullName;

                        planEntity.USERNAME_SH = sheet.Cells[i, 6].StringValue;
                        var userEntity1 = new UserBLL().GetList().Where(x => x.RealName == sheet.Cells[i, 6].StringValue).ToList().FirstOrDefault();
                        if (userEntity1 != null)
                        {
                            planEntity.USERID_SH = userEntity1.UserId;
                        }
                        DateTime s=new DateTime();
                        try
                        {
                           s= DateTime.Parse(sheet.Cells[i, 4].StringValue);
                            planEntity.DATATIME_BZ = s;
                        }
                        catch (Exception)
                        {

                            falseMessage += "</br>" + "第" + (i + 1) + "行编制时间格式不正确,未能导入.";
                            error++;
                            continue;
                        }

                        try
                        {
                            s=DateTime.Parse(sheet.Cells[i, 7].StringValue);
                            planEntity.DATATIME_SH = s;
                        }
                        catch (Exception)
                        {

                            falseMessage += "</br>" + "第" + (i + 1) + "行审核时间格式不正确,未能导入.";
                            error++;
                            continue;
                        }
                        planEntity.ORGXZTYPE = sheet.Cells[i, 8].StringValue == "单位内部" ? 1 : 2;
                        planEntity.ORGXZNAME=sheet.Cells[i, 8].StringValue;
                       
                        planEntity.ISAUDITNAME = sheet.Cells[i, 9].StringValue;
                        var dataItem = new DataItemDetailBLL().GetDataItemListByItemCode("'MAE_IsAudit'").Where(x => x.ItemName == sheet.Cells[i, 9].StringValue).ToList().FirstOrDefault();
                        if (dataItem != null)
                        {
                            planEntity.ISAUDIT = dataItem.ItemValue;
                        }
                        planEntity.ID = Guid.NewGuid().ToString();
                        //文件路径
                        string filepath = sheet.Cells[i, 10].StringValue;
                        var fileinfo = new FileInfo(decompressionDirectory + filepath);
                        FileInfoEntity fileInfoEntity = new FileInfoEntity();
                        string fileguid = Guid.NewGuid().ToString();
                        fileInfoEntity.Create();
                        fileInfoEntity.RecId = planEntity.ID; //关联ID
                        fileInfoEntity.FileName = filepath;
                        fileInfoEntity.FilePath = "~/Resource/Reserverplan/" + fileguid + fileinfo.Extension;
                        fileInfoEntity.FileSize = (Math.Round(decimal.Parse(fileinfo.Length.ToString()) / decimal.Parse("1024"), 2)).ToString();//文件大小（kb）
                        fileInfoEntity.FileExtensions = fileinfo.Extension;
                        fileInfoEntity.FileType = fileinfo.Extension.Replace(".", "");
                        TransportRemoteToServer(Server.MapPath("~/Resource/Reserverplan/"), decompressionDirectory + filepath, fileguid + fileinfo.Extension);
                        fileinfobll.SaveForm("", fileInfoEntity);
                        planEntity.FILES = fileInfoEntity.RecId;
                        try
                        {
                            reserverplanbll.SaveForm(planEntity.ID, planEntity);
                        }
                        catch
                        {
                            error++;
                        }
                       
                    }

                    message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条。";
                    if (error > 0)
                    {
                        message += "</br>" + falseMessage;
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
         /// 
         /// </summary>  
         /// <param name="src">远程服务器路径（共享文件夹路径）</param>  
         /// <param name="dst">本地文件夹路径</param>  
         /// <param name="filename"></param> 
         public static void TransportRemoteToServer(string src, string dst, string filename)
         {
             if (!Directory.Exists(src))
             {
                 Directory.CreateDirectory(src);
             }
             FileStream inFileStream = new FileStream(src + filename, FileMode.OpenOrCreate);

             FileStream outFileStream = new FileStream(dst, FileMode.Open);

             byte[] buf = new byte[outFileStream.Length];

             int byteCount;

             while ((byteCount = outFileStream.Read(buf, 0, buf.Length)) > 0)
             {
                 inFileStream.Write(buf, 0, byteCount);

             }

             inFileStream.Flush();

             inFileStream.Close();

             outFileStream.Flush();

             outFileStream.Close();

         }

         public void UnZip(string zipedFile, string strDirectory, string password, bool overWrite)
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
        #endregion
    }
}
