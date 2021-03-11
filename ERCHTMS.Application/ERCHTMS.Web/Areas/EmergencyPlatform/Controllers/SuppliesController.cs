using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.Busines.EmergencyPlatform;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using System;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Entity.PublicInfoManage;
using System.Web;
using ERCHTMS.Busines.SystemManage;
using System.Linq;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Entity.BaseManage;
using System.IO;

namespace ERCHTMS.Web.Areas.EmergencyPlatform.Controllers
{
    /// <summary>
    /// 描 述：应急物资
    /// </summary>
    public class SuppliesController : MvcControllerBase
    {
        private SuppliesBLL suppliesbll = new SuppliesBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();
        private DistrictBLL districtbll = new DistrictBLL();

        #region 视图功能

        /// <summary>
        /// 列表页面
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
        public ActionResult TestIndex()
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

            //ViewBag.Code = suppliesbll.GetMaxCode();
            UserBLL userbll = new UserBLL();
            ViewBag.User = userbll.GetEntity(OperatorProvider.Provider.Current().UserId);


            return View();
        }

        /// <summary>
        /// 选择应急物资页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Select()
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

            try
            {
                queryJson = queryJson ?? "";
                pagination.p_kid = "ID";
                pagination.p_fields = " SUPPLIESCODE,SUPPLIESTYPENAME,SUPPLIESNAME,NUM,SUPPLIESUNTILNAME,STORAGEPLACE,DEPARTNAME,USERNAME,createuserid,createuserdeptcode,createuserorgcode,WORKAREANAME,models,userid,departid,(select count(1) from mae_suppliescheckdetail where suppliesid=t.id) as checknum";
                pagination.p_tablename = "mae_supplies t";
                pagination.conditionJson = "1=1";
                pagination.sidx = "SUPPLIESCODE";
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

                var data = suppliesbll.GetPageList(pagination, queryJson);
                var watch = CommonHelper.TimerStart();
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
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
            
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = suppliesbll.GetList(queryJson);
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
            var data = suppliesbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 根据ids获取多条数据
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetMutipleDataJson(string Ids)
        {
            try
            {
                var data = suppliesbll.GetMutipleDataJson(Ids);
                return ToJsonResult(data);
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
            
        }

        /// <summary>
        /// 获取负责的物资
        /// </summary>
        /// <param name="DutyPerson"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDutySupplies(string DutyPerson)
        {
            try
            {
                var data = suppliesbll.GetDutySuppliesDataJson(DutyPerson);
                return Success("获取成功", data);
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
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
            try
            {
                foreach (var item in keyValue.Split(','))
                {
                    if (suppliesbll.CheckRemove(item).Rows.Count > 0)
                    {
                        return Error("该物资正在被申请领用，无法删除。");
                    }
                    suppliesbll.RemoveForm(item);
                }
                return Success("删除成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
            
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
        public ActionResult SaveForm(string keyValue, SuppliesEntity entity)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (string.IsNullOrWhiteSpace(entity.SUPPLIESCODE))
                {
                    entity.SUPPLIESCODE = suppliesbll.GetMaxCode();
                }
                suppliesbll.SaveForm(keyValue, entity);
                var entityInorOut = new InoroutrecordEntity
                {
                    USERID = user.UserId,
                    USERNAME = user.UserName,
                    DEPARTID = user.DeptId,
                    DEPARTNAME = user.DeptName,
                    INOROUTTIME = DateTime.Now,
                    SUPPLIESCODE = entity.SUPPLIESCODE,
                    SUPPLIESTYPE = entity.SUPPLIESTYPE,
                    SUPPLIESTYPENAME = entity.SUPPLIESTYPENAME,
                    SUPPLIESNAME = entity.SUPPLIESNAME,
                    SUPPLIESUNTIL = entity.SUPPLIESUNTIL,
                    SUPPLIESUNTILNAME = entity.SUPPLIESUNTILNAME,
                    NUM = entity.NUM,
                    STORAGEPLACE = entity.STORAGEPLACE,
                    MOBILE = entity.MOBILE,
                    SUPPLIESID = entity.ID,
                    STATUS = 0
                };
                var inoroutrecordbll = new InoroutrecordBLL();
                if (keyValue == "")
                    inoroutrecordbll.SaveForm("", entityInorOut);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
            
        }

        /// <summary>
        /// 复制图片
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="recid"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult CopyForm(string keyValue, string recid)
        {
            try
            {
                IList<FileInfoEntity> filelist = fileinfobll.GetFileList(recid);

                IList<FileInfoEntity> filelist1 = fileinfobll.GetFileList(keyValue);
                foreach (var item in filelist1)
                {
                    fileinfobll.RemoveForm(item.FileId);
                }

                string dir = string.Format("~/Resource/{0}/{1}", "ht/images", DateTime.Now.ToString("yyyyMMdd"));
                foreach (var item in filelist)
                {
                    if (!Directory.Exists(Server.MapPath(dir)))
                    {
                        Directory.CreateDirectory(Server.MapPath(dir));
                    }
                    if (System.IO.File.Exists(Server.MapPath(item.FilePath)))
                    {
                        string newFileName = Guid.NewGuid().ToString() + item.FileExtensions;
                        string newFilePath = dir + "/" + newFileName;
                        System.IO.File.Copy(Server.MapPath(item.FilePath), Server.MapPath(newFilePath));
                        item.FilePath = newFilePath;
                    }
                    item.RecId = keyValue;
                    item.FileId = Guid.NewGuid().ToString();
                    fileinfobll.SaveForm("", item);
                }
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
            
        }

        [HandlerLogin(LoginMode.Ignore)]
        [HandlerAuthorize(PermissionMode.Ignore)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(5, "导入应急物资")]
        public string ImportSuppliesData() {
            int error = 0;
            string message = "请选择格式正确的文件再导入!";

            string falseMessage = "";
            int count = HttpContext.Request.Files.Count; 
            if (count > 0)
            {
                count = 0;
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
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                wb.Open(Server.MapPath("~/Resource/temp/" + fileName), file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xlsx") ? Aspose.Cells.FileFormatType.Excel2007Xlsx : Aspose.Cells.FileFormatType.Excel2003);
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

                //判断表头是否正确,以免使用错误模板
                var sheet = wb.Worksheets[0];
                if (sheet.Cells[2, 0].StringValue != "物资类型" || sheet.Cells[2, 1].StringValue != "物资名称" || sheet.Cells[2, 2].StringValue != "规格型号" || sheet.Cells[2, 3].StringValue != "数量"
                   || sheet.Cells[2, 4].StringValue != "单位" || sheet.Cells[2, 5].StringValue != "存放区域" || sheet.Cells[2, 6].StringValue != "存放地点" || sheet.Cells[2, 7].StringValue != "主要功能")
                {
                    return message;
                }
                //List<SuppliesEntity> slist = new List<SuppliesEntity>();
                for (int i = 3; i <= sheet.Cells.MaxDataRow; i++)
                {
                    count++;
                    SuppliesEntity entity = new SuppliesEntity();
                    if (string.IsNullOrWhiteSpace(sheet.Cells[i, 1].StringValue))
                    {
                        falseMessage += "</br>" + "第" + (i + 1) + "行物资名称不能为空,未能导入.";
                        error++;
                        continue;
                    }
                    if (string.IsNullOrWhiteSpace(sheet.Cells[i, 3].StringValue))
                    {
                        falseMessage += "</br>" + "第" + (i + 1) + "行数量不能为空,未能导入.";
                        error++;
                        continue;
                    }
                    entity.SUPPLIESTYPENAME = sheet.Cells[i, 0].StringValue;
                    var item = new DataItemDetailBLL().GetDataItemListByItemCode("'MAE_SUPPLIESTYPE'").Where(x => x.ItemName == entity.SUPPLIESTYPENAME).ToList().FirstOrDefault();
                    if (item != null) {
                        entity.SUPPLIESTYPE = item.ItemValue;
                    }
                    entity.SUPPLIESNAME = sheet.Cells[i, 1].StringValue;
                    entity.Models = sheet.Cells[i, 2].StringValue;
                    entity.NUM = Convert.ToInt32(sheet.Cells[i, 3].StringValue);
                    entity.SUPPLIESUNTILNAME = sheet.Cells[i, 4].StringValue;
                    var itemUnit = new DataItemDetailBLL().GetDataItemListByItemCode("'MAE_SUPPLIESUNTIL'").Where(x => x.ItemName == entity.SUPPLIESUNTILNAME).ToList().FirstOrDefault();
                    if (itemUnit != null)
                    {
                        entity.SUPPLIESUNTIL = itemUnit.ItemValue;
                    }
                    if (!string.IsNullOrWhiteSpace(sheet.Cells[i, 5].StringValue))
                    {
                        DistrictEntity district = districtbll.GetDistrict(user.OrganizeId, sheet.Cells[i, 5].StringValue);
                        if (district == null)
                        {
                            falseMessage += "</br>" + "第" + (i + 1) + "行区域不是系统内区域,未能导入.";
                            error++;
                            continue;
                        }
                        else
                        {
                            entity.WorkAreaCode = district.DistrictCode;
                            entity.WorkAreaName = district.DistrictName;
                        }
                    }
                    entity.STORAGEPLACE = sheet.Cells[i, 6].StringValue;
                    entity.MAINFUN = sheet.Cells[i, 7].StringValue;
                    entity.CREATEDATE = DateTime.Now;
                    entity.DEPARTID = user.DeptId;
                    entity.DEPARTNAME = user.DeptName;
                    entity.USERID = user.UserId;
                    entity.USERNAME = user.UserName;
                    entity.SUPPLIESCODE = suppliesbll.GetMaxCode();
                    //entity.Create();
                    suppliesbll.SaveForm(entity.ID,entity);
                    var entityInorOut = new InoroutrecordEntity
                    {
                        USERID = entity.USERID,
                        USERNAME = entity.USERNAME,
                        DEPARTID = entity.DEPARTID,
                        DEPARTNAME = entity.DEPARTNAME,
                        INOROUTTIME = DateTime.Now,
                        SUPPLIESCODE = entity.SUPPLIESCODE,
                        SUPPLIESTYPE = entity.SUPPLIESTYPE,
                        SUPPLIESTYPENAME = entity.SUPPLIESTYPENAME,
                        SUPPLIESNAME = entity.SUPPLIESNAME,
                        SUPPLIESUNTIL = entity.SUPPLIESUNTIL,
                        SUPPLIESUNTILNAME = entity.SUPPLIESUNTILNAME,
                        NUM = entity.NUM,
                        STORAGEPLACE = entity.STORAGEPLACE,
                        MOBILE = entity.MOBILE,
                        SUPPLIESID = entity.ID,
                        STATUS = 0
                    };
                    var inoroutrecordbll = new InoroutrecordBLL();
                    inoroutrecordbll.SaveForm("", entityInorOut);

                    //slist.Add(entity);
                }
               
                message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条。";
                if (error > 0)
                {
                    message += "</br>" + falseMessage;
                }
            }
            return message;
        }
        #endregion

        #region 数据导出
        /// <summary>
        /// 导出用户列表
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出应急物资")]
        public ActionResult ExportSuppliesList(string condition, string queryJson)
        {
            Pagination pagination = new Pagination();
            queryJson = queryJson ?? "";
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "ID";
            pagination.p_fields = " SUPPLIESCODE,SUPPLIESTYPENAME,SUPPLIESNAME,NUM,SUPPLIESUNTILNAME,WORKAREANAME,STORAGEPLACE,DEPARTNAME,USERNAME";
            pagination.p_tablename = "V_mae_supplies t";
            pagination.conditionJson = "1=1";
            pagination.sidx = "CREATEDATE";
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
            var data = suppliesbll.GetPageList(pagination, queryJson);

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "应急物资";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "应急物资.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "SUPPLIESCODE".ToLower(), ExcelColumn = "物资编号" });
            listColumnEntity.Add(new ColumnEntity() { Column = "SUPPLIESTYPENAME".ToLower(), ExcelColumn = "物资类型" });
            listColumnEntity.Add(new ColumnEntity() { Column = "SUPPLIESNAME".ToLower(), ExcelColumn = "物资名称" });
            listColumnEntity.Add(new ColumnEntity() { Column = "NUM".ToLower(), ExcelColumn = "数量" });
            listColumnEntity.Add(new ColumnEntity() { Column = "SUPPLIESUNTILNAME".ToLower(), ExcelColumn = "单位" });
            listColumnEntity.Add(new ColumnEntity() { Column = "WORKAREANAME".ToLower(), ExcelColumn = "存放区域" });
            listColumnEntity.Add(new ColumnEntity() { Column = "STORAGEPLACE".ToLower(), ExcelColumn = "存放地点" });
            listColumnEntity.Add(new ColumnEntity() { Column = "DEPARTNAME".ToLower(), ExcelColumn = "责任部门" });
            listColumnEntity.Add(new ColumnEntity() { Column = "USERNAME".ToLower(), ExcelColumn = "责任人" });
            excelconfig.ColumnEntity = listColumnEntity;

            //调用导出方法
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("导出成功。");
        }

        /// <summary>
        /// 导出用户列表
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出应急物资")]
        public ActionResult Export(string condition, string queryJson)
        {
            Pagination pagination = new Pagination();
            queryJson = queryJson ?? "";
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "ID";
            pagination.p_fields = " SUPPLIESCODE,SUPPLIESTYPENAME,SUPPLIESNAME,NUM,SUPPLIESUNTILNAME,STORAGEPLACE,DEPARTNAME,USERNAME";
            pagination.p_tablename = "V_mae_supplies t";
            pagination.conditionJson = "1=1";
            pagination.sidx = "CREATEDATE";
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
            var data = suppliesbll.GetPageList(pagination, queryJson);

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "应急物资";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "应急物资.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "SUPPLIESCODE".ToLower(), ExcelColumn = "物资编号" });
            listColumnEntity.Add(new ColumnEntity() { Column = "SUPPLIESTYPENAME".ToLower(), ExcelColumn = "物资类型" });
            listColumnEntity.Add(new ColumnEntity() { Column = "SUPPLIESNAME".ToLower(), ExcelColumn = "物资名称" });
            listColumnEntity.Add(new ColumnEntity() { Column = "NUM".ToLower(), ExcelColumn = "数量" });
            listColumnEntity.Add(new ColumnEntity() { Column = "SUPPLIESUNTILNAME".ToLower(), ExcelColumn = "单位" });
            listColumnEntity.Add(new ColumnEntity() { Column = "STORAGEPLACE".ToLower(), ExcelColumn = "存放地点" });
            listColumnEntity.Add(new ColumnEntity() { Column = "DEPARTNAME".ToLower(), ExcelColumn = "责任部门" });
            listColumnEntity.Add(new ColumnEntity() { Column = "USERNAME".ToLower(), ExcelColumn = "责任人" });
            excelconfig.ColumnEntity = listColumnEntity;

            //调用导出方法
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("导出成功。");
        }
        #endregion
    }
}
