using ERCHTMS.Entity.StandardSystem;
using ERCHTMS.Busines.StandardSystem;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System;
using ERCHTMS.Code;
using System.Data;
using System.IO;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.PublicInfoManage;
using System.Web;
using BSFramework.Util.Extension;
using ERCHTMS.Entity.BaseManage;
using System.Linq;
using ERCHTMS.Entity.PublicInfoManage;
using ICSharpCode.SharpZipLib.Zip;

namespace ERCHTMS.Web.Areas.StandardSystem.Controllers
{
    /// <summary>
    /// 描 述：标准体系
    /// </summary>
    public class StandardsystemController : MvcControllerBase
    {
        private StandardsystemBLL standardsystembll = new StandardsystemBLL();
        private StcategoryBLL stcategorybll = new StcategoryBLL();
        private DepartmentBLL DepartmentBLL = new DepartmentBLL();
        private PostBLL postBLL = new PostBLL();
        private ElementBLL elementBLL = new ElementBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();
        private StandardreadrecordBLL standardreadrecordbll = new StandardreadrecordBLL();

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
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }

        /// <summary>
        /// 我的收藏
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Mystore()
        {
            return View();
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Import()
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
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            Operator user = OperatorProvider.Provider.Current();
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "a.ID";
            pagination.p_fields = "a.filename,b.name as categorycode,b.id as categorycodeid,relevantelementname,relevantelementid,stationid,stationname,carrydate,a.createdate,consultnum,d.fullname as createuserdeptname,a.standardtype,a.createuserid,a.createuserdeptcode,a.createuserorgcode,(case  when  c.recid is null then '0' else '1' end) as isnew,a.Publishdept,e.name as maincategory,e.id as maincategoryid,e.parentid as mainparentid,filelist.filenum";
            pagination.p_tablename = @" hrs_standardsystem a left join hrs_stcategory b on a.categorycode =b.id left join hrs_standardreadrecord c on a.id =c.recid and c.createuserid ='" + user.UserId + "' left join base_department d on a.createuserdeptcode = d.encode left join hrs_stcategory e on e.id =b.parentid"
                + " left join (select  count(fileid) filenum,recid  from  base_fileinfo f  group by recid) filelist on a.id=filelist.recid ";
            pagination.conditionJson = "1=1";
            if (!user.IsSystem)
            {
                pagination.conditionJson += " and a.createuserorgcode='" + user.OrganizeCode + "'";
            }
            var data = standardsystembll.GetPageList(pagination, queryJson);
            var jsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return ToJsonResult(jsonData);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = standardsystembll.GetList(queryJson);
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
            var data = standardsystembll.GetEntity(keyValue);
            if (!string.IsNullOrEmpty(data.CATEGORYCODE))
            {
                var catory = stcategorybll.GetEntity(data.CATEGORYCODE);
                data.CATEGORYNAME = string.IsNullOrEmpty(catory.NAME) ? "" : catory.NAME;
            }
            data.CREATEUSERDEPTNAME = DepartmentBLL.GetEntityByCode(data.CREATEUSERDEPTCODE).FullName;
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetStandardCount()
        {
            var watch = CommonHelper.TimerStart();
            var data = standardsystembll.GetStandardCount();
            var jsonData = new
            {
                data = data,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return ToJsonResult(jsonData);
        }

        /// <summary>
        /// 导出
        /// </summary>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult ExportData(string queryJson)
        {
            try
            {
                var queryParam = queryJson.ToJObject();
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.p_kid = "a.ID";
                if (queryParam["standardtype"].ToString() == "1" || queryParam["standardtype"].ToString() == "2" || queryParam["standardtype"].ToString() == "3" || queryParam["standardtype"].ToString() == "4" || queryParam["standardtype"].ToString() == "5" || queryParam["standardtype"].ToString() == "6")
                {
                    pagination.p_fields = "filename,b.name as categorycode,relevantelementname,to_char(carrydate,'yyyy-MM-dd') as carrydate,to_char(a.createdate,'yyyy-MM-dd') as createdate,consultnum";
                }
                else
                {
                    pagination.p_fields = "filename,to_char(a.createdate,'yyyy-MM-dd') as createdate,c.fullname as createuserdeptname,consultnum";
                }
                
                pagination.p_tablename = " hrs_standardsystem a left join hrs_stcategory b on a.categorycode=b.id left join base_department c on a.createuserdeptcode = c.encode";
                pagination.conditionJson = "1=1";
                pagination.sidx = "a.createdate";
                pagination.sord = "desc";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                DataTable exportTable = standardsystembll.GetPageList(pagination, queryJson);
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                
                if (!queryParam["standardtype"].IsEmpty())
                {
                    switch (queryParam["standardtype"].ToString())
                    {
                        case "1":
                            excelconfig.Title = "技术标准体系";
                            excelconfig.FileName = "技术标准体系信息导出.xls";
                            break;
                        case "2":
                            excelconfig.Title = "管理标准体系";
                            excelconfig.FileName = "管理标准体系信息导出.xls";
                            break;
                        case "3":
                            excelconfig.Title = "岗位标准体系";
                            excelconfig.FileName = "岗位标准体系信息导出";
                            break;
                        case "4":
                            excelconfig.Title = "上级标准化文件";
                            excelconfig.FileName = "上级标准化文件信息导出.xls";
                            break;
                        case "5":
                            excelconfig.Title = "指导标准";
                            excelconfig.FileName = "指导标准信息导出.xls";
                            break;
                        case "6":
                            excelconfig.Title = "法律法规";
                            excelconfig.FileName = "法律法规信息导出.xls";
                            break;
                        case "7":
                            excelconfig.Title = "标准体系策划与构建";
                            excelconfig.FileName = "标准体系策划与构建信息导出.xls";
                            break;
                        case "8":
                            excelconfig.Title = "标准体系评价与改进";
                            excelconfig.FileName = "标准体系评价与改进信息导出.xls";
                            break;
                        case "9":
                            excelconfig.Title = "标准化培训";
                            excelconfig.FileName = "标准化培训信息导出.xls";
                            break;
                        default:
                            break;
                    }
                }
                
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filename", ExcelColumn = "文件名称", Width = 300 });
                if (queryParam["standardtype"].ToString() == "1" || queryParam["standardtype"].ToString() == "2" || queryParam["standardtype"].ToString() == "3" || queryParam["standardtype"].ToString() == "4" || queryParam["standardtype"].ToString() == "5" || queryParam["standardtype"].ToString() == "6")
                {
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "categorycode", ExcelColumn = "类别", Width = 300 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "relevantelementname", ExcelColumn = "对应元素", Width = 300 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "carrydate", ExcelColumn = "施行日期", Width = 300 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createdate", ExcelColumn = "发布日期", Width = 300 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "consultnum", ExcelColumn = "查阅频次", Width = 300 });
                }
                else
                {
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createdate", ExcelColumn = "发布日期", Width = 300 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createuserdeptname", ExcelColumn = "发布单位/部门", Width = 300 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "consultnum", ExcelColumn = "查阅频次", Width = 300 });
                }
                
                //调用导出方法
                //ExcelHelper.ExcelDownload(exportTable, excelconfig);
                ExcelHelper.ExportByAspose(exportTable, excelconfig.FileName, excelconfig.ColumnEntity);
            }
            catch (Exception ex)
            {

            }
            return Success("导出成功。");
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
        [HandlerMonitor(6, "删除标准体系")]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            standardsystembll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="idsData">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(6, "批量删除标准体系")]
        [AjaxOnly]
        public ActionResult RemoveListForm(string idsData)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!string.IsNullOrEmpty(idsData))
            {
                if (idsData.Contains(","))
                {
                    string[] array = idsData.TrimEnd(',').Split(',');
                    for (int i = 0; i < array.Length; i++)
                    {
                        standardsystembll.RemoveForm(array[i].ToString());
                    }
                }
                else
                {
                    standardsystembll.RemoveForm(idsData);
                }
                return Success("删除成功。");
            }
            return Error("无数据删除。");
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
        public ActionResult SaveForm(string keyValue, StandardsystemEntity entity)
        {
            try
            {
                standardsystembll.SaveForm(keyValue, entity);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        /// <summary>
        /// 标准导入
        /// </summary>
        /// <param name="standardtype"></param>
        /// <param name="categorycode"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportStandard(string standardtype, string categorycode)
        {
            if (OperatorProvider.Provider.Current().IsSystem)
            {
                return "超级管理员无此操作权限";
            }
            string orgId = OperatorProvider.Provider.Current().OrganizeId;//所属公司
            int error = 0;
            string message = "请选择文件格式正确的文件再导入!";
            string falseMessage = "";
            int count = HttpContext.Request.Files.Count;
            if (count > 0)
            {
                if (HttpContext.Request.Files.Count !=2)
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
                
                Aspose.Cells.Cells cells = wb.Worksheets[0].Cells;
                DataTable dt = cells.ExportDataTable(2, 0, cells.MaxDataRow - 1, cells.MaxColumn + 1, false);
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    //文件名称
                    string filename = dt.Rows[i][0].ToString();
                    //文件路径
                    string filepath = dt.Rows[i][1].ToString();
                    //相应元素
                    string relevantelement = "";
                    string relevantelementname = "";
                    string relevantelementid = "";
                    //实施日期
                    string carrydate = "";
                    if (standardtype == "1" || standardtype == "2" || standardtype == "3" || standardtype == "4" || standardtype == "5" || standardtype == "6")
                    {
                        relevantelement = dt.Rows[i][2].ToString();
                        carrydate = dt.Rows[i][3].ToString();
                    }

                    //文学字号
                    string dispatchcode = "";
                    //颁布部门
                    string publishdept = "";
                    if (standardtype == "6")
                    {
                        dispatchcode = dt.Rows[i][4].ToString();
                        publishdept = dt.Rows[i][5].ToString();
                    }


                    string dutyid = "";
                    string dutyName = "";

                    //---****值存在空验证*****--
                    if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(filepath))
                    {
                        falseMessage += "</br>" + "第" + (i + 3) + "行值存在空,未能导入.";
                        error++;
                        continue;
                    }

                    //---****文件格式验证*****--
                    if (!(filepath.Substring(filepath.IndexOf('.')).Contains("doc") || filepath.Substring(filepath.IndexOf('.')).Contains("docx") || filepath.Substring(filepath.IndexOf('.')).Contains("pdf")))
                    {
                        falseMessage += "</br>" + "第" + (i + 3) + "行附件格式不正确,未能导入.";
                        error++;
                        continue;
                    }

                    //---****文件是否存在验证*****--
                    if (!System.IO.File.Exists(decompressionDirectory + filepath))
                    {
                        falseMessage += "</br>" + "第" + (i + 3) + "行附件不存在,未能导入.";
                        error++;
                        continue;
                    }

                    //--**验证岗位是否存在 * *--
                    int startnum = 4;
                    if (standardtype == "1" || standardtype == "2" || standardtype == "3" || standardtype == "4" || standardtype == "5")
                    {
                        startnum = 4;
                    }
                    else if (standardtype == "6")
                    {
                        startnum = 6;
                    }
                    else if (standardtype == "7" || standardtype == "8" || standardtype == "9")
                    {
                        startnum = 2;
                    }
                    for (int j = startnum; j < dt.Columns.Count; j++)
                    {
                        if (!dt.Rows[i][j].IsEmpty())
                        {

                            foreach (var item in dt.Rows[i][j].ToString().Split(','))
                            {
                                DepartmentEntity dept = DepartmentBLL.GetList().Where(t => t.OrganizeId == orgId && t.FullName == dt.Rows[0][j].ToString()).FirstOrDefault();
                                if (dept == null)
                                {
                                    continue;
                                }
                                RoleEntity re = postBLL.GetList().Where(a => a.FullName == item.ToString() && a.OrganizeId == orgId && a.DeleteMark == 0 && a.EnabledMark == 1 && a.DeptId == dept.DepartmentId).FirstOrDefault();
                                if (re == null)
                                {
                                    //falseMessage += "</br>" + "第" + (i + 3) + "行岗位有误,未能导入.";
                                    //error++;
                                    continue;
                                }
                                else
                                {
                                    dutyid += re.RoleId + ",";
                                    dutyName += re.FullName + ",";
                                }
                            }
                        }
                    }

                    dutyid = dutyid.Length > 0 ? dutyid.Substring(0, dutyid.Length - 1) : "";
                    dutyName = dutyName.Length > 0 ? dutyName.Substring(0, dutyName.Length - 1) : "";
                    StandardsystemEntity standard = new StandardsystemEntity();
                    try
                    {
                        if (!string.IsNullOrEmpty(carrydate))
                        {
                            standard.CARRYDATE = DateTime.Parse(DateTime.Parse(carrydate).ToString("yyyy-MM-dd"));
                        }
                    }
                    catch
                    {
                        falseMessage += "</br>" + "第" + (i + 3) + "行时间有误,未能导入.";
                        error++;
                        continue;
                    }
                    if (!string.IsNullOrEmpty(relevantelement))
                    {
                        foreach (var item in relevantelement.Split(','))
                        {

                            ElementEntity re = elementBLL.GetList("").Where(a => a.NAME == item.ToString()).FirstOrDefault();
                            if (re == null)
                            {
                                //falseMessage += "</br>" + "第" + (i + 2) + "行相应元素有误,未能导入.";
                                //error++;
                                continue;
                            }
                            else
                            {
                                relevantelementname += re.NAME + ",";
                                relevantelementid += re.ID + ",";
                            }
                        }
                    }
                    relevantelementname = string.IsNullOrEmpty(relevantelementname) ? "" : relevantelementname.Substring(0, relevantelementname.Length - 1);
                    relevantelementid = string.IsNullOrEmpty(relevantelementid) ? "" : relevantelementid.Substring(0, relevantelementid.Length - 1);
                    standard.FILENAME = filename;
                    standard.STATIONID = dutyid;
                    standard.STATIONNAME = dutyName;
                    standard.RELEVANTELEMENTNAME = relevantelementname;
                    standard.RELEVANTELEMENTID = relevantelementid;
                    standard.DISPATCHCODE = dispatchcode;
                    standard.PUBLISHDEPT = publishdept;
                    standard.STANDARDTYPE = standardtype;
                    standard.CATEGORYCODE = categorycode;
                    standard.CONSULTNUM = 0;
                    standard.ID = Guid.NewGuid().ToString();
                    var fileinfo = new FileInfo(decompressionDirectory + filepath);
                    FileInfoEntity fileInfoEntity = new FileInfoEntity();
                    string fileguid = Guid.NewGuid().ToString();
                    fileInfoEntity.Create();
                    fileInfoEntity.RecId = standard.ID; //关联ID
                    fileInfoEntity.FileName = filepath;
                    fileInfoEntity.FilePath = "~/Resource/StandardSystem/" + fileguid + fileinfo.Extension;
                    fileInfoEntity.FileSize = (Math.Round(decimal.Parse(fileinfo.Length.ToString()) / decimal.Parse("1024"), 2)).ToString();//文件大小（kb）
                    fileInfoEntity.FileExtensions = fileinfo.Extension;
                    fileInfoEntity.FileType = fileinfo.Extension.Replace(".", "");
                    TransportRemoteToServer(Server.MapPath("~/Resource/StandardSystem/"), decompressionDirectory + filepath, fileguid + fileinfo.Extension);
                    fileinfobll.SaveForm("", fileInfoEntity);
                    try
                    {
                        standardsystembll.SaveForm(standard.ID, standard);
                    }
                    catch
                    {
                        error++;
                    }
                }
                count = dt.Rows.Count - 1;
                message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条";
                message += "</br>" + falseMessage;
            }
            return message;
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


        /// <summary>
        /// 添加查阅频次
        /// </summary>
        /// <param name="keyValue"></param>
        [AjaxOnly]
        [HttpPost]
        public ActionResult AddConsultNum(string keyValue)
        {
            Operator user = OperatorProvider.Provider.Current();
            var data = standardsystembll.GetEntity(keyValue);
            data.CONSULTNUM += 1;
            standardsystembll.SaveForm(keyValue, data);
            var standardreadrecordentity = standardreadrecordbll.GetList("").Where(t => t.RecId == keyValue && t.CreateUserId == user.UserId).FirstOrDefault();
            standardreadrecordentity = standardreadrecordentity == null ? new StandardreadrecordEntity() : standardreadrecordentity;
            standardreadrecordentity.RecId = keyValue;
            standardreadrecordbll.SaveForm(standardreadrecordentity.ID, standardreadrecordentity);
            
            return Success("操作成功。");
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
