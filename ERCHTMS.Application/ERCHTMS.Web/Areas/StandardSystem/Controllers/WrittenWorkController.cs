using ERCHTMS.Entity.StandardSystem;
using ERCHTMS.Busines.StandardSystem;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Data;
using BSFramework.Data;
using System.Collections.Generic;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Busines.PublicInfoManage;
using System.IO;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Cache;
using System.Linq;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using BSFramework.Util.Offices;
using System;
using System.Web;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.SafetyLawManage;

namespace ERCHTMS.Web.Areas.StandardSystem.Controllers
{
    /// <summary>
    /// 描 述：书面工作程序swp
    /// </summary>
    public class WrittenWorkController : MvcControllerBase
    {
        private WrittenWorkBLL writtenworkbll = new WrittenWorkBLL();
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
        private StoreLawBLL storelawbll = new StoreLawBLL();

        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var strdept = new DataItemDetailBLL().GetItemValue(user.OrganizeId, "DeptSet");
            var entity = new DepartmentBLL().GetEntity(strdept);
            ViewBag.SpecialDept = entity != null ? entity.DeptCode : "";
            ViewBag.SpecialDeptId = entity != null ? entity.DepartmentId : "";
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
        /// 导入页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Import()
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

            var watch = CommonHelper.TimerStart();
            pagination.conditionJson = "1=1";

            #region 数据权限
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            //查看范围数据权限
            /**
             * */
            string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
            #endregion
            var data = writtenworkbll.GetPageDataTable(pagination, queryJson, authType);
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
            var data = writtenworkbll.GetList(queryJson);
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
            var data = writtenworkbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }


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
                pagination.sidx = "createdate";//排序字段
                pagination.sord = "desc";//排序方式
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                var data = writtenworkbll.GetPageDataTable(pagination, queryJson, authType);
                DataTable excelTable = new DataTable();
                excelTable.Columns.Add(new DataColumn("filename"));
                excelTable.Columns.Add(new DataColumn("issuedept"));
                excelTable.Columns.Add(new DataColumn("filecode"));
                excelTable.Columns.Add(new DataColumn("publishdate"));
                excelTable.Columns.Add(new DataColumn("carrydate"));
                foreach (DataRow item in data.Rows)
                {
                    DataRow newDr = excelTable.NewRow();
                    newDr["filename"] = item["filename"];
                    newDr["issuedept"] = item["issuedept"];
                    newDr["filecode"] = item["filecode"];
                    DateTime publishdate, carrydate;
                    DateTime.TryParse(item["publishdate"].ToString(), out publishdate);
                    DateTime.TryParse(item["carrydate"].ToString(), out carrydate);
                    newDr["publishdate"] = publishdate.ToString("yyyy-MM-dd");
                    newDr["carrydate"] = carrydate.ToString("yyyy-MM-dd");
                    excelTable.Rows.Add(newDr);
                }

                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "书面工程程序SWP信息";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.IsAllSizeColumn = true;
                excelconfig.FileName = "书面工程程序SWP信息导出.xls";
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filename", ExcelColumn = "文件和资料名称", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "issuedept", ExcelColumn = "颁发部门", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filecode", ExcelColumn = "文件编号", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "publishdate", ExcelColumn = "发布日期", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "carrydate", ExcelColumn = "实施日期", Width = 10 });
                //调用导出方法
                ExcelHelper.ExcelDownload(excelTable, excelconfig);
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
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            writtenworkbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, WrittenWorkEntity entity)
        {
            writtenworkbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion

        #region 导入
        /// <summary>
        /// 导入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportSWP(string belongtypecode)
        {
            int error = 0;
            string message = "请选择格式正确的文件再导入!";
            string falseMessage = "";
            int count = HttpContext.Request.Files.Count;
            if (count > 0)
            {
                HttpPostedFileBase file = HttpContext.Request.Files[0];
                if (string.IsNullOrEmpty(file.FileName))
                {
                    return message;
                }
                if (!(file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xls") || file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xlsx")))
                {
                    return message;
                }
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName));
                DataTable dt = ExcelHelper.ExcelImport(Server.MapPath("~/Resource/temp/" + fileName));
                int order = 1;
                if (Directory.Exists(Server.MapPath("~/Resource/ht/images/swp")) == false)//如果不存在就创建file文件夹
                {
                    Directory.CreateDirectory(Server.MapPath("~/Resource/ht/images/swp"));
                }
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    order = i;
                    //文件名称
                    string filename = dt.Rows[i][0].ToString();
                    //文件编号
                    string filecode = dt.Rows[i][1].ToString();
                    //颁发部门
                    string iuusedept = dt.Rows[i][2].ToString();
                    //发布日期
                    string publishdate = dt.Rows[i][3].ToString();
                    //实施日期
                    string carrydate = dt.Rows[i][4].ToString();
                    //---****值存在空验证*****--
                    if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(filecode) || string.IsNullOrEmpty(iuusedept) || string.IsNullOrEmpty(carrydate) || string.IsNullOrEmpty(publishdate))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行值存在空,未能导入.";
                        error++;
                        continue;
                    }
                    WrittenWorkEntity sl = new WrittenWorkEntity();
                    sl.FileName = filename;
                    sl.FileCode = filecode;
                    sl.IssueDept = iuusedept;
                    sl.BelongTypeCode = belongtypecode;//所属单位
                    sl.FilesId = Guid.NewGuid().ToString();

                    FileInfoEntity fileEntity = new FileInfoEntity();
                    fileEntity.RecId = sl.FilesId;
                    fileEntity.EnabledMark = 1;
                    fileEntity.DeleteMark = 0;
                    fileEntity.FilePath = "~/Resource/ht/images/swp/" + filename;
                    fileEntity.FileName = sl.FileName;
                    fileEntity.FolderId = "ht/images";
                    try
                    {
                        sl.CarryDate = DateTime.Parse(DateTime.Parse(carrydate).ToString("yyyy-MM-dd"));
                        sl.PublishDate = DateTime.Parse(DateTime.Parse(publishdate).ToString("yyyy-MM-dd"));
                    }
                    catch
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行时间有误,未能导入.";
                        error++;
                        continue;
                    }
                    try
                    {
                        writtenworkbll.SaveForm("", sl);
                        fileInfoBLL.SaveForm("", fileEntity);
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
        #endregion

        #region 我的收藏
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetStoreListJson(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_kid = "storeid";
            pagination.p_fields = "a.lawid,b.CreateDate,FileName,IssueDept,FileCode,publishdate,CarryDate,FilesId";
            pagination.p_tablename = " bis_storelaw a left join hrs_writtenwork b on a.lawid=b.id";
            pagination.conditionJson = "userid='" + user.UserId + "' and ctype='5'";
            var data = storelawbll.GetPageDataTable(pagination, queryJson);
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
        #endregion

        #region 导出我的收藏
        /// <summary>
        /// 导出
        /// </summary>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult ExportMyStoreData(string queryJson)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.p_kid = "storeid";
                pagination.p_fields = "FileName,IssueDept,FileCode,publishdate,CarryDate";
                pagination.p_tablename = " bis_storelaw a left join hrs_writtenwork b on a.lawid=b.id";
                pagination.conditionJson = "userid='" + user.UserId + "' and ctype='5'";
                pagination.sidx = "a.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                DataTable exportTable = storelawbll.GetPageDataTable(pagination, queryJson);
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "书面工作程序SWP我的收藏";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "书面工作程序SWP我的收藏导出.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filename", ExcelColumn = "文件和资料名称", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "issuedept", ExcelColumn = "颁发部门", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filecode", ExcelColumn = "文件编号", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "publishdate", ExcelColumn = "发布日期", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "carrydate", ExcelColumn = "实施日期", Width = 10 });
                //调用导出方法
                ExcelHelper.ExcelDownload(exportTable, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("导出成功。");
        }
        #endregion
    }
}
