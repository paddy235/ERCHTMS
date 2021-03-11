using ERCHTMS.Entity.SafetyLawManage;
using ERCHTMS.Busines.SafetyLawManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using System.Web;
using System;
using BSFramework.Util.Offices;
using System.Data;
using System.Collections.Generic;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
using System.Linq;
using System.IO;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Busines.PublicInfoManage;
using ICSharpCode.SharpZipLib.Zip;

namespace ERCHTMS.Web.Areas.SafetyLawManage.Controllers
{
    /// <summary>
    /// 描 述：安全管理制度
    /// </summary>
    public class SafeInstitutionController : MvcControllerBase
    {
        private SafeInstitutionBLL safeinstitutionbll = new SafeInstitutionBLL();
        private SafeInstitutionTreeBLL safeinstitutiontreebll = new SafeInstitutionTreeBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private DepartmentBLL deptBll = new DepartmentBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();

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
        /// 我的收藏页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult myStoreIndex()
        {
            return View();
        }
        /// <summary>
        /// 新列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NewIndex()
        {
            return View();
        }
        /// <summary>
        /// 树表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TreeForm()
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
            pagination.p_kid = "Id";
            pagination.p_fields = " a.CreateDate,FileName,IssueDept,FileCode,CarryDate,FilesId,releasedate,revisedate,lawtypename,a.createuserid,a.createuserdeptcode,a.createuserorgcode ";
            pagination.p_tablename = " bis_safeinstitution a ";
            pagination.conditionJson = "1=1";
           
            var data = safeinstitutionbll.GetPageDataTable(pagination, queryJson);
            var watch = CommonHelper.TimerStart();
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
            var data = safeinstitutionbll.GetList(queryJson);
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
            var data = safeinstitutionbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取分类节点
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetTypeTreeJson(string datatype,string orgcode)
        {
            Operator user = OperatorProvider.Provider.Current();

            var treeList = new List<TreeEntity>();
            string code = user.OrganizeCode;
            if (!string.IsNullOrWhiteSpace(orgcode))
            {
                code = orgcode;
            }
            var where = string.Format(" and CreateUserOrgCode='{0}' and datatype='{1}'", code, datatype);
            
            //var where = string.Format(" and 1=1", user.OrganizeCode);
            var data = safeinstitutiontreebll.GetList(where).OrderBy(t => t.CREATEDATE).ToList();
            foreach (var item in data)
            {
                bool hasChild = data.Where(x => x.ParentId == item.ID).Count() > 0 ? true : false;
                TreeEntity tree = new TreeEntity();
                tree.id = item.ID;
                tree.text = item.TreeName;
                tree.value = item.TreeCode;
                tree.parentId = item.ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChild;
                tree.Attribute = "Code";
                tree.AttributeValue = item.TreeCode;

                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson("-1"));
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetTreeFormJson(string keyValue)
        {
            var data = safeinstitutiontreebll.GetEntity(keyValue);
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
            safeinstitutionbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveTreeForm(string keyValue)
        {
            safeinstitutiontreebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SafeInstitutionEntity entity)
        {
            safeinstitutionbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
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
        public ActionResult SaveTreeForm(string keyValue, SafeInstitutionTreeEntity entity)
        {
            entity.ParentId = !string.IsNullOrWhiteSpace(entity.ParentId) ? entity.ParentId : "-1";
            var parent = safeinstitutiontreebll.GetEntity(keyValue);
            if (parent == null) {
                entity.TreeCode = GetDepartmentCode(entity);
            }
            entity.TreeName = entity.TreeName.Replace("\\", "╲");
            safeinstitutiontreebll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion

        /// <summary>
        /// 文件导入
        /// </summary>
        /// <param name="treeId"></param>
        /// <param name="treeName"></param>
        /// <param name="treeCode"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportStandard(string treeId, string treeName, string treeCode)
        {
            try
            {
                if (OperatorProvider.Provider.Current().IsSystem)
                {
                    return "超级管理员无此操作权限";
                }
                string orgId = OperatorProvider.Provider.Current().OrganizeId;//所属公司
                int error = 0;
                int success = 0;
                string message = "请选择文件格式正确的文件再导入!";
                string falseMessage = "";
                int count = HttpContext.Request.Files.Count;
                if (count > 0)
                {
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

                    Aspose.Cells.Cells cells = wb.Worksheets[0].Cells;
                    DataTable dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn, false);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SafeInstitutionEntity entity = new SafeInstitutionEntity();
                        entity.Id = Guid.NewGuid().ToString();
                        entity.FilesId = Guid.NewGuid().ToString();

                        //文件名称
                        string filename = dt.Rows[i][0].ToString();
                        //文件编号
                        string filecode = dt.Rows[i][1].ToString();
                        //发布单位
                        string issuedept = dt.Rows[i][4].ToString();
                        //发布时间
                        string releasedate = dt.Rows[i][5].ToString();
                        //修订时间
                        string revisedate = dt.Rows[i][6].ToString();
                        //实施时间
                        string carrydate = dt.Rows[i][7].ToString();
                        //备注
                        string Remark = dt.Rows[i][8].ToString();



                        //---****值存在空验证*****--
                        if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(filecode))
                        {
                            falseMessage += "</br>" + "第" + (i + 1) + "行值存在空,未能导入.";
                            error++;
                            continue;
                        }

                        bool conbool = false;


                        //正文附件路径
                        string[] filepaths = dt.Rows[i][2].ToString().Split(';');

                        var filepath = "";
                        for (int j = 0; j < filepaths.Length; j++)
                        {
                            filepath = filepaths[j];

                            if (string.IsNullOrEmpty(filepath))
                            {
                                continue;
                            }
                            string strPath = filepath.Substring(filepath.IndexOf('.'));
                            //---****文件格式验证*****--
                            if (!(strPath.Contains("doc") || strPath.Contains("docx") || strPath.Contains("pdf") ))
                            {
                                falseMessage += "</br>" + "第" + (i + 1) + "行指定正文附件格式不正确,未能导入.";
                                error++;
                                conbool = true;
                                continue;
                            }

                            //---****文件是否存在验证*****--
                            if (!System.IO.File.Exists(decompressionDirectory + filepath))
                            {
                                falseMessage += "</br>" + "第" + (i + 1) + "行指定正文附件不存在,未能导入.";
                                error++;
                                conbool = true;
                                continue;
                            }
                            var fileinfo = new FileInfo(decompressionDirectory + filepath);
                            FileInfoEntity fileInfoEntity = new FileInfoEntity();
                            string fileguid = Guid.NewGuid().ToString();
                            fileInfoEntity.Create();
                            fileInfoEntity.RecId = entity.FilesId; //关联ID
                            fileInfoEntity.FileName = filepath;
                            fileInfoEntity.FilePath = "~/Resource/InstitutionSystem/" + fileguid + fileinfo.Extension;
                            fileInfoEntity.FileSize = (Math.Round(decimal.Parse(fileinfo.Length.ToString()) / decimal.Parse("1024"), 2)).ToString();//文件大小（kb）
                            fileInfoEntity.FileExtensions = fileinfo.Extension;
                            fileInfoEntity.FileType = fileinfo.Extension.Replace(".", "");
                            TransportRemoteToServer(Server.MapPath("~/Resource/InstitutionSystem/"), decompressionDirectory + filepath, fileguid + fileinfo.Extension);
                            fileinfobll.SaveForm("", fileInfoEntity);

                        }

                        if (conbool)
                        {
                            continue;
                        }
                        //正文附件路径
                        filepaths = dt.Rows[i][3].ToString().Split(';');

                        filepath = "";
                        for (int j = 0; j < filepaths.Length; j++)
                        {
                            filepath = filepaths[j];

                            if (string.IsNullOrEmpty(filepath))
                            {
                                continue;
                            }
                            string strPath = filepath.Substring(filepath.IndexOf('.'));
                            //---****文件格式验证*****--
                            if (!(strPath.Contains("doc") || strPath.Contains("docx") || strPath.Contains("pdf") || strPath.Contains("ppt") || strPath.Contains("xlsx") || strPath.Contains("xls") || strPath.Contains("png") || strPath.Contains("jpg") || strPath.Contains("jpeg")))
                            {
                                falseMessage += "</br>" + "第" + (i + 1) + "行指定附件格式不正确,未能导入.";
                                error++;
                                conbool = true;
                                continue;
                            }

                            //---****文件是否存在验证*****--
                            if (!System.IO.File.Exists(decompressionDirectory + filepath))
                            {
                                falseMessage += "</br>" + "第" + (i + 1) + "行指定附件不存在,未能导入.";
                                error++;
                                conbool = true;
                                continue;
                            }
                            var fileinfo = new FileInfo(decompressionDirectory + filepath);
                            FileInfoEntity fileInfoEntity = new FileInfoEntity();
                            string fileguid = Guid.NewGuid().ToString();
                            fileInfoEntity.Create();
                            fileInfoEntity.RecId = entity.Id; //关联ID
                            fileInfoEntity.FileName = filepath;
                            fileInfoEntity.FilePath = "~/Resource/InstitutionSystem/" + fileguid + fileinfo.Extension;
                            fileInfoEntity.FileSize = (Math.Round(decimal.Parse(fileinfo.Length.ToString()) / decimal.Parse("1024"), 2)).ToString();//文件大小（kb）
                            fileInfoEntity.FileExtensions = fileinfo.Extension;
                            fileInfoEntity.FileType = fileinfo.Extension.Replace(".", "");
                            TransportRemoteToServer(Server.MapPath("~/Resource/InstitutionSystem/"), decompressionDirectory + filepath, fileguid + fileinfo.Extension);
                            fileinfobll.SaveForm("", fileInfoEntity);

                        }

                        entity.FileName = filename;
                        entity.FileCode = filecode;
                        entity.IssueDept = issuedept;
                        entity.LawTypeId = treeId;
                        entity.LawTypeName = treeName;
                        entity.LawTypeCode = treeCode;
                        if (!string.IsNullOrEmpty(releasedate))
                        {
                            entity.ReleaseDate = Convert.ToDateTime(releasedate);
                        }
                        if (!string.IsNullOrEmpty(revisedate))
                        {
                            entity.ReviseDate = Convert.ToDateTime(revisedate);
                        }
                        if (!string.IsNullOrEmpty(carrydate))
                        {
                            entity.CarryDate = Convert.ToDateTime(carrydate);
                        }

                        entity.Remark = !string.IsNullOrEmpty(Remark) ? Remark : "";

                        try
                        {
                            safeinstitutionbll.SaveForm(entity.Id, entity);
                            success++;
                        }
                        catch
                        {
                            error++;
                        }
                    }
                    message = "共有" + dt.Rows.Count + "条记录,成功导入" + success + "条，失败" + error + "条";
                    message += "</br>" + falseMessage;
                }
                return message;
            }
            catch (Exception e)
            {
                return "导入的Excel数据格式不正确，请下载标准模板重新填写！";
            }

        }

        /// <summary>
        /// 解压zip文件
        /// </summary>
        /// <param name="zipedFile"></param>
        /// <param name="strDirectory"></param>
        /// <param name="password"></param>
        /// <param name="overWrite"></param>
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

        #region 导出
        /// <summary>
        /// 导出
        /// </summary>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult ExportData(string queryJson)
        {
            try
            {
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.p_kid = "Id";
                pagination.p_fields = "FileName,FileCode,IssueDept,to_char(releasedate,'yyyy-MM-dd') as releasedate,to_char(revisedate,'yyyy-MM-dd') as revisedate,to_char(carrydate,'yyyy-MM-dd') as carrydate,lawtypename";
                pagination.p_tablename = " bis_safeinstitution a ";
                pagination.conditionJson = "1=1";
                pagination.sidx = "a.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                
                DataTable exportTable = safeinstitutionbll.GetPageDataTable(pagination, queryJson);
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "安全规章制度信息";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "安全规章制度信息导出.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filename", ExcelColumn = "文件名称", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filecode", ExcelColumn = "文件编号", Width = 25 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "issuedept", ExcelColumn = "发布单位(部门)", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "releasedate", ExcelColumn = "发布时间", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "revisedate", ExcelColumn = "修订时间", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "carrydate", ExcelColumn = "实施时间", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "lawtypename", ExcelColumn = "类型", Width = 20 });

                //调用导出方法
                ExcelHelper.ExcelDownload(exportTable, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("导出成功。");
        }
        #endregion

        /// <summary>
        /// 根据当前机构获取对应的机构代码  机构代码 2-6-8-10  位
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public string GetDepartmentCode(SafeInstitutionTreeEntity Entity)
        {
            string maxCode = string.Empty;
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var maxObj = deptBll.GetDataTable(string.Format("select max(TreeCode) as TreeCode  from bis_safeinstitutiontree t where  parentid='{0}' and CreateUserOrgCode='{1}' and datatype='{2}' ", Entity.ParentId, user.OrganizeCode, Entity.DataType));
            if (maxObj != null && maxObj.Rows.Count > 0 && !string.IsNullOrEmpty(maxObj.Rows[0][0].ToString()))
            {
                string newCode = string.Empty;

                string maxValue = (Convert.ToDecimal(maxObj.Rows[0][0].ToString()) + 1).ToString();

                for (int i = 1; i <= 30; i++)
                {
                    if (maxValue.ToString().Length == i)
                    {
                        newCode = maxObj.Rows[0][0].ToString().Substring(0, maxObj.Rows[0][0].ToString().Length - i) + maxValue;
                        break;
                    }
                }
                maxCode = newCode;
            }
            else
            {
                SafeInstitutionTreeEntity parentEntity = safeinstitutiontreebll.GetEntity(Entity.ParentId);  //获取父对象
                if (parentEntity != null)
                {
                    maxCode = parentEntity.TreeCode + "001";  //固定值,非可变
                }
                else
                {
                    maxCode = "001";
                }

            }

            return maxCode;
        }

        /// <summary>
        /// 获取分类节点
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetParentOrgCode()
        {
            try
            {
                Operator user = OperatorProvider.Provider.Current();
                var provdata = departmentBLL.GetList().Where(t => user.NewDeptCode.StartsWith(t.DeptCode) && t.Nature == "省级" && string.IsNullOrWhiteSpace(t.Description));
                if (provdata.Count() > 0)
                {
                    DepartmentEntity provEntity = provdata.FirstOrDefault();
                    return Content(provEntity.DeptCode);
                }
                else
                {
                    return Content("0");
                }
            }
            catch (Exception)
            {
                return Content("0");
            }
        }
    }
}
