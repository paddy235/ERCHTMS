using ERCHTMS.Entity.SafetyLawManage;
using ERCHTMS.Busines.SafetyLawManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using System.Data;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System;
using System.Web;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Busines.PublicInfoManage;
using System.IO;

namespace ERCHTMS.Web.Areas.SafetyLawManage.Controllers
{
    /// <summary>
    /// 描 述：应急预案范本
    /// </summary>
    public class EmergencyLawController : MvcControllerBase
    {
        private EmergencyLawBLL emergencylawbll = new EmergencyLawBLL();
        private SafeInstitutionBLL safeinstitutionbll = new SafeInstitutionBLL();
        private AccidentCaseLawBLL accidentcaselawbll = new AccidentCaseLawBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private SafeStandardsBLL safestandardsbll = new SafeStandardsBLL();
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();

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
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = emergencylawbll.GetList(queryJson);
            return ToJsonResult(data);
        }

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
            pagination.p_fields = "CreateDate,FileName,EmergencyType,Source,Remark,FilesId,createuserid,createuserdeptcode,createuserorgcode";
            pagination.p_tablename = " bis_emergencylaw";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                if (!string.IsNullOrEmpty(authType))
                {
                    switch (authType)
                    {
                        case "1":
                            pagination.conditionJson += " and (createuserid='" + user.UserId + "' or createuserorgcode='00')";
                            break;
                        case "2":
                            pagination.conditionJson += " and (createuserdeptcode='" + user.DeptCode + "' or createuserorgcode='00')";
                            break;
                        case "3":
                            pagination.conditionJson += " and (createuserdeptcode like'" + user.DeptCode + "%' or createuserorgcode='00')";
                            break;
                        case "4":
                            pagination.conditionJson += " and (createuserorgcode='" + user.OrganizeCode + "' or createuserorgcode='00')";
                            break;
                    }
                }
                else
                {
                    pagination.conditionJson += " and 0=1";
                }
            }
            var data = emergencylawbll.GetPageDataTable(pagination, queryJson);
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
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = emergencylawbll.GetEntity(keyValue);
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
            emergencylawbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, EmergencyLawEntity entity)
        {
            emergencylawbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion


        #region 导入应急预案范本
        /// <summary>
        /// 导入应急预案范本
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportCase()
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
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    order = i;
                    //文件名称
                    string filename = dt.Rows[i][0].ToString();
                    //应急预案类型
                    string ctype = dt.Rows[i][1].ToString();
                    //来源
                    string soure = dt.Rows[i][2].ToString();
                    //备注
                    string remark = dt.Rows[i][3].ToString();
                    //---****值存在空验证*****--
                    if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(ctype) || string.IsNullOrEmpty(soure))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行值存在空,未能导入.";
                        error++;
                        continue;
                    }
                    EmergencyLawEntity sl = new EmergencyLawEntity();
                    sl.FileName = filename;
                    if (ctype == "综合应急预案")
                        ctype = "1";
                    else if (ctype == "专项应急预案")
                        ctype = "2";
                    else if (ctype == "现场处置方案")
                        ctype = "3";
                    sl.EmergencyType = ctype;
                    sl.Source = soure;
                    sl.Remark = remark;
                    try
                    {
                        emergencylawbll.SaveForm("", sl);
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

        #region 导入安全规章制度
        /// <summary>
        /// 导入安全规章制度
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportInstitution()
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
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    order = i;
                    //文件名称（必填）
                    string filename = dt.Rows[i][0].ToString();
                    //发布单位
                    string iuusedept = dt.Rows[i][1].ToString();
                    //文件编号
                    string filecode = dt.Rows[i][2].ToString();
                    //管理制度类别（必填）
                    string lawtype = dt.Rows[i][3].ToString();
                    //施行日期（必填）
                    string carrydate = dt.Rows[i][4].ToString();
                    //有效版本号（必填）
                    string validversions = dt.Rows[i][5].ToString();
                    //---****值存在空验证*****--
                    if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(lawtype) || string.IsNullOrEmpty(carrydate) || string.IsNullOrEmpty(validversions))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行值存在空,未能导入.";
                        error++;
                        continue;
                    }
                    SafeInstitutionEntity sl = new SafeInstitutionEntity();
                    sl.FileName = filename;
                    sl.IssueDept = iuusedept;
                    sl.FileCode = filecode;
                    string itemvalue = dataitemdetailbll.GetItemValue(lawtype, "InstitutionLaw");
                    sl.LawTypeCode = itemvalue;
                    sl.ValidVersions = validversions;
                    try
                    {
                        sl.CarryDate = DateTime.Parse(DateTime.Parse(carrydate).ToString("yyyy-MM-dd"));
                    }
                    catch
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行时间有误,未能导入.";
                        error++;
                        continue;
                    }
                    try
                    {
                        safeinstitutionbll.SaveForm("", sl);
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

        #region 导入事故案例
        /// <summary>
        /// 导入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportAccidentCase()
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

                if (Directory.Exists(Server.MapPath("~/Resource/ht/images/channel")) == false)//如果不存在就创建file文件夹
                {
                    Directory.CreateDirectory(Server.MapPath("~/Resource/ht/images/channel"));
                }
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    order = i;
                    //文件名称
                    string filename = dt.Rows[i][0].ToString();
                    //事故时间
                    string time = dt.Rows[i][1].ToString();
                    //备注
                    string remark = dt.Rows[i][2].ToString();
                    //---****值存在空验证*****--
                    if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(time))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行值存在空,未能导入.";
                        error++;
                        continue;
                    }
                    AccidentCaseLawEntity sl = new AccidentCaseLawEntity();
                    sl.FileName = filename;
                    sl.AccRange = "2";
                    sl.Remark = remark;
                    sl.FilesId = Guid.NewGuid().ToString();
                    FileInfoEntity fileEntity = new FileInfoEntity();
                    fileEntity.RecId = sl.FilesId;
                    fileEntity.EnabledMark = 1;
                    fileEntity.DeleteMark = 0;
                    fileEntity.FilePath = "~/Resource/ht/images/channel/" + filename;
                    fileEntity.FileName = sl.FileName;
                    fileEntity.FolderId = "ht/images";
                    try
                    {
                        sl.AccTime = DateTime.Parse(DateTime.Parse(time).ToString("yyyy-MM-dd HH:mm"));
                    }
                    catch
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行时间有误,未能导入.";
                        error++;
                        continue;
                    }
                    try
                    {
                        accidentcaselawbll.SaveForm("", sl);
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

        #region 导入安全操作规程
        /// <summary>
        /// 导入安全操作规程
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportStandards()
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
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    order = i;
                    //文件名称（必填）
                    string filename = dt.Rows[i][0].ToString();
                    //发布单位
                    string iuusedept = dt.Rows[i][1].ToString();
                    //文件编号
                    string filecode = dt.Rows[i][2].ToString();
                    //岗位类别（必填）
                    string lawtype = dt.Rows[i][3].ToString();
                    //施行日期（必填）
                    string carrydate = dt.Rows[i][4].ToString();
                    //有效版本号（必填）
                    string validversions = dt.Rows[i][5].ToString();
                    //---****值存在空验证*****--
                    if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(lawtype) || string.IsNullOrEmpty(carrydate) || string.IsNullOrEmpty(validversions))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行值存在空,未能导入.";
                        error++;
                        continue;
                    }
                    SafeStandardsEntity sl = new SafeStandardsEntity();
                    sl.FileName = filename;
                    sl.IssueDept = iuusedept;
                    sl.FileCode = filecode;
                    string itemvalue = dataitemdetailbll.GetItemValue(lawtype, "StandardsLaw");
                    sl.LawTypeCode = itemvalue;
                    sl.ValidVersions = validversions;
                    try
                    {
                        sl.CarryDate = DateTime.Parse(DateTime.Parse(carrydate).ToString("yyyy-MM-dd"));
                    }
                    catch
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行时间有误,未能导入.";
                        error++;
                        continue;
                    }
                    try
                    {
                        safestandardsbll.SaveForm("", sl);
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
                pagination.p_fields = "FileName,Source,EmergencyType,Remark";
                pagination.p_tablename = " bis_emergencylaw";
                pagination.conditionJson = "1=1";
                pagination.sidx = "createdate";//排序字段
                pagination.sord = "desc";//排序方式
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (!user.IsSystem)
                {
                    string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                    if (!string.IsNullOrEmpty(authType))
                    {
                        switch (authType)
                        {
                            case "1":
                                pagination.conditionJson += " and (createuserid='" + user.UserId + "' or createuserorgcode='00')";
                                break;
                            case "2":
                                pagination.conditionJson += " and (createuserdeptcode='" + user.DeptCode + "' or createuserorgcode='00')";
                                break;
                            case "3":
                                pagination.conditionJson += " and (createuserdeptcode like'" + user.DeptCode + "%' or createuserorgcode='00')";
                                break;
                            case "4":
                                pagination.conditionJson += " and (createuserorgcode='" + user.OrganizeCode + "' or createuserorgcode='00')";
                                break;
                        }
                    }
                    else
                    {
                        pagination.conditionJson += " and 0=1";
                    }
                }
                DataTable exportTable = emergencylawbll.GetPageDataTable(pagination, queryJson);
                foreach (DataRow item in exportTable.Rows)
                {
                    var ctype = item["EmergencyType"].ToString();
                    if (ctype == "1")
                        ctype = "综合应急预案";
                    else if (ctype == "2")
                        ctype = "专项应急预案";
                    else if (ctype == "3")
                        ctype = "现场处置方案";
                    item["EmergencyType"] = ctype;
                }
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "应急预案范本信息";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "应急预案范本导出.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filename", ExcelColumn = "文件和资料名称", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "source", ExcelColumn = "来源", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "emergencytype", ExcelColumn = "应急预案类型", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "remark", ExcelColumn = "备注", Width = 60 });
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
