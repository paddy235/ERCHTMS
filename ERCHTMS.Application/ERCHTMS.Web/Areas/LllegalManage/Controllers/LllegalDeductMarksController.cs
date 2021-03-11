using ERCHTMS.Entity.LllegalManage;
using ERCHTMS.Busines.LllegalManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System.Data;
using System;
using Aspose.Cells;
using System.Drawing;
using System.Web;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
using System.Linq;
using ERCHTMS.Entity.HiddenTroubleManage.ViewModel;
using ERCHTMS.Busines.SystemManage;

namespace ERCHTMS.Web.Areas.LllegalManage.Controllers
{
    /// <summary>
    /// 描 述：违章档案扣分表
    /// </summary>
    public class LllegalDeductMarksController : MvcControllerBase
    {
        private LllegalDeductMarksBLL lllegaldeductmarksbll = new LllegalDeductMarksBLL();
        private UserBLL userbll = new UserBLL(); //用户操作对象
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();

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
        ///导入信息
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
            var data = lllegaldeductmarksbll.GetList(queryJson);
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
            var data = lllegaldeductmarksbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
                /// 获取实体 
                /// </summary>
                /// <param name="keyValue">主键值</param>
                /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetLllegalRecordEntity(string keyValue)
        {
            var data = lllegaldeductmarksbll.GetLllegalRecordEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region 获取违章档案列表数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {

            var watch = CommonHelper.TimerStart();
            var data = lllegaldeductmarksbll.GetLllegalRecordInfo(pagination, queryJson);
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
        #endregion

        #region 获取违章得分列表数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetJfPageListJson(Pagination pagination, string queryJson)
        {

            var watch = CommonHelper.TimerStart();
            var data = lllegaldeductmarksbll.GetLllegalPointInfo(pagination, queryJson);
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
            lllegaldeductmarksbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, LllegalDeductMarksEntity entity)
        {
            lllegaldeductmarksbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion

        #region 数据导出
        /// <summary>
        /// 未遂事件报告与调查处理
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "数据导出")]
        public ActionResult ExportExcel(string queryJson, int mode)
        {
            string fileName = string.Empty;
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.sidx = " nvl(punishdate,sysdate-10000) desc,nvl(createdate,sysdate -10000) desc,deptsort asc,sortcode asc,userid";
            pagination.sord = " desc";
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string userId = curUser.UserId;
            try
            {
                //取出扣分数据源
                DataTable exportTable = lllegaldeductmarksbll.GetLllegalRecordInfo(pagination, queryJson);

                //取出人员积分数据源
                pagination.sidx = " deptsort asc,sortcode asc,userid";
               DataTable userTable = lllegaldeductmarksbll.GetLllegalPointInfo(pagination, queryJson);

                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                //生产部门
                if (mode == 0)
                {
                    wb.Open(Server.MapPath("~/Resource/ExcelTemplate/生产部门违章档案(厂部)模板.xls"));
                    fileName = "生产部门违章档案(厂部)" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                }
                else
                {
                    wb.Open(Server.MapPath("~/Resource/ExcelTemplate/外协单位违章档案(厂部)模板.xls"));
                    fileName = "外协单位违章档案(厂部)" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                }
                string title = string.Empty;
                string title0 = string.Empty;
                //第一张表
                Aspose.Cells.Worksheet sheet = wb.Worksheets[0] as Aspose.Cells.Worksheet;
                Cells cells = sheet.Cells;
                //第二张表
                Aspose.Cells.Worksheet sheet1 = wb.Worksheets[1] as Aspose.Cells.Worksheet;
                Cells cells1 = sheet1.Cells;

                Aspose.Cells.Cell cell0 = sheet.Cells[0, 0];
                Aspose.Cells.Cell cell00 = sheet1.Cells[0, 0];
                if (mode == 0)
                {
                    title = curUser.OrganizeName + "生产部门个人违章扣分表";
                    cell0.PutValue(title);

                    title0 = curUser.OrganizeName + "生产部门个人违章档案(厂级)";
                    cell00.PutValue(title0);
                }
                else
                {
                    title = curUser.OrganizeName + "外协单位个人违章扣分表";
                    cell0.PutValue(title);

                    title0 = curUser.OrganizeName + "外协单位个人违章档案(厂级)";
                    cell00.PutValue(title0);
                }

                int rowIndex = 2;
                //遍历行
                #region 第一张表
                foreach (DataRow row in exportTable.Rows)
                {
                    if (rowIndex - 2 < exportTable.Rows.Count - 1)
                    {
                        cells.CopyRow(cells, rowIndex, rowIndex + 1);
                    }

                    string username = !string.IsNullOrEmpty(row["username"].ToString()) ? row["username"].ToString() : "";
                    Aspose.Cells.Cell rcell1 = sheet.Cells[rowIndex, 1];
                    rcell1.PutValue(username);

                    string deptname = !string.IsNullOrEmpty(row["deptname"].ToString()) ? row["deptname"].ToString() : "";
                    Aspose.Cells.Cell rcell2 = sheet.Cells[rowIndex, 2];
                    rcell2.PutValue(deptname);

                    string teamname = !string.IsNullOrEmpty(row["teamname"].ToString()) ? row["teamname"].ToString() : "";
                    Aspose.Cells.Cell rcell3 = sheet.Cells[rowIndex, 3];
                    rcell3.PutValue(teamname);

                    string dutyname = !string.IsNullOrEmpty(row["dutyname"].ToString()) ? row["dutyname"].ToString() : "";
                    Aspose.Cells.Cell rcell4 = sheet.Cells[rowIndex, 4];
                    rcell4.PutValue(dutyname);

                    string lllegaldescribe = !string.IsNullOrEmpty(row["lllegaldescribe"].ToString()) ? row["lllegaldescribe"].ToString() : "";
                    Aspose.Cells.Cell rcell5 = sheet.Cells[rowIndex, 5];
                    rcell5.PutValue(lllegaldescribe);

                    string lllegaltypename = !string.IsNullOrEmpty(row["lllegaltypename"].ToString()) ? row["lllegaltypename"].ToString() : "";
                    Aspose.Cells.Cell rcell6 = sheet.Cells[rowIndex, 6];
                    rcell6.PutValue(lllegaltypename);

                    string punishdate = !string.IsNullOrEmpty(row["punishdate"].ToString()) ? Convert.ToDateTime(row["punishdate"].ToString()).ToString("yyyy-MM-dd") : "";
                    Aspose.Cells.Cell rcell7 = sheet.Cells[rowIndex, 7];
                    rcell7.PutValue(punishdate);

                    string punishresult = !string.IsNullOrEmpty(row["punishresult"].ToString()) ? row["punishresult"].ToString() : "0";
                    Aspose.Cells.Cell rcell8 = sheet.Cells[rowIndex, 8];
                    rcell8.PutValue(row["punishresult"].ToString());

                    string punishpoint = !string.IsNullOrEmpty(row["punishpoint"].ToString()) ? row["punishpoint"].ToString() : "0";
                    Aspose.Cells.Cell rcell9 = sheet.Cells[rowIndex, 9];
                    rcell9.PutValue(punishpoint);
                    rcell9.R1C1Formula = "=I" + (rowIndex + 1).ToString() + "/100";

                    rowIndex += 1;
                }
                #endregion

                rowIndex = 2;
                #region 第二张表
                foreach (DataRow row in userTable.Rows)
                {
                    if (rowIndex - 2 < userTable.Rows.Count - 1)
                    {
                        cells1.CopyRow(cells1, rowIndex, rowIndex + 1);
                    }
                    string username = !string.IsNullOrEmpty(row["username"].ToString()) ? row["username"].ToString() : "";
                    Aspose.Cells.Cell rcell1 = sheet1.Cells[rowIndex, 1];
                    rcell1.PutValue(username);

                    string deptname = !string.IsNullOrEmpty(row["deptname"].ToString()) ? row["deptname"].ToString() : "";
                    Aspose.Cells.Cell rcell2 = sheet1.Cells[rowIndex, 2];
                    rcell2.PutValue(deptname);

                    string teamname = !string.IsNullOrEmpty(row["teamname"].ToString()) ? row["teamname"].ToString() : "";
                    Aspose.Cells.Cell rcell3 = sheet1.Cells[rowIndex, 3];
                    rcell3.PutValue(teamname);

                    string dutyname = !string.IsNullOrEmpty(row["dutyname"].ToString()) ? row["dutyname"].ToString() : "";
                    Aspose.Cells.Cell rcell4 = sheet1.Cells[rowIndex, 4];
                    rcell4.PutValue(dutyname);

                    string initpoint = !string.IsNullOrEmpty(row["initpoint"].ToString()) ? row["initpoint"].ToString() : "100";
                    Aspose.Cells.Cell rcell5 = sheet1.Cells[rowIndex, 5];
                    rcell5.PutValue(initpoint);

                    Aspose.Cells.Cell rcell6 = sheet1.Cells[rowIndex, 6];
                    if (mode == 0)
                    {
                        rcell6.R1C1Formula = "=SUMIF(生产部门违章扣分表!$B$3:$J$10000,B" + (rowIndex + 1).ToString() + ",生产部门违章扣分表!$J$3:$J$10000)";
                    }
                    else
                    {
                        rcell6.R1C1Formula = "=SUMIF(外协单位违章扣分表!$B$3:$J$10000,B" + (rowIndex + 1).ToString() + ",外协单位违章扣分表!$J$3:$J$10000)";
                    }

                    Aspose.Cells.Cell rcell7 = sheet1.Cells[rowIndex, 7];
                    rcell7.R1C1Formula = "=F" + (rowIndex + 1).ToString() + "-G" + (rowIndex + 1).ToString();

                    rowIndex += 1;
                }
                #endregion

                string tempSavePath = Server.MapPath("~/Resource/Temp/") + fileName;
                wb.Save(tempSavePath);
                string url = "../../Utility/DownloadFile?filePath=~/Resource/Temp/" + fileName + "&speed=10240000&newFileName=" + fileName;
                return Redirect(url);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion

        #region 导入违章扣分信息
        /// <summary>
        /// 导入违章扣分信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportInfo(int mode)
        {
            if (OperatorProvider.Provider.Current().IsSystem)
            {
                return "超级管理员无此操作权限";
            }
            var curUser = OperatorProvider.Provider.Current();
            string orgId = curUser.OrganizeId;//所属公司
            string message = "请选择格式正确的文件(excel数据文件)再导入!";
            string falseMessage = "";
            int count = HttpContext.Request.Files.Count;
            try
            {
                List<string> listIds = new List<string>();
                if (count > 0)
                {
                    HttpPostedFileBase file = HttpContext.Request.Files[0];
                    string hiddenDirectory = string.Empty;
                    Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();

                    #region 过滤文件
                    if (string.IsNullOrEmpty(file.FileName))
                    {
                        return message;
                    }
                    if (!(file.FileName.ToLower().Substring(file.FileName.ToLower().IndexOf('.')).Contains("xls") || file.FileName.ToLower().Substring(file.FileName.ToLower().IndexOf('.')).Contains("xlsx")))
                    {
                        return message;
                    }
                    string fileName1 = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName1));
                    wb.Open(Server.MapPath("~/Resource/temp/" + fileName1));
                    #endregion
                    Worksheet sheets = wb.Worksheets[0];
                    Aspose.Cells.Cells cells = sheets.Cells;
                    DataTable dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn + 1, true);
                    string labledept = mode == 0 ? "部门" : "外协单位";
                    if (dt.Columns.Contains("部门") && mode > 0)
                    {
                        return "当前模板不适用与外协单位违章档案扣分数据导入!";
                    }
                    if (dt.Columns.Contains("外协单位") && mode == 0)
                    {
                        return "当前模板不适用与生产部门违章档案扣分数据导入!";
                    }
                    //记录错误信息
                    List<string> resultlist = new List<string>();
                    List<UserEntity> ulist = userbll.GetList().OrderBy(p => p.SortCode).ToList();
                    List<DepartmentEntity> dlist = departmentBLL.GetList().OrderBy(p => p.SortCode).ToList();
                    var lllegaltypelist = dataitemdetailbll.GetDataItemListByItemCode("'LllegalType'");
                    int total = 0;
                    #region 违章部分
                    if (sheets.Name.Contains("违章"))//违章扣分信息导入
                    {
                        #region 对象装载
                        List<ImportLllegalPunish> list = new List<ImportLllegalPunish>();
                        //先获取到职务列表;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string resultmessage = "第" + (i + 1).ToString() + "行数据"; //显示错误
                            bool isadddobj = true;
                            //姓名
                            string username = dt.Columns.Contains("姓名") ? dt.Rows[i]["姓名"].ToString().Trim() : string.Empty;
                            //部门
                            string deptname = dt.Columns.Contains("部门") ? dt.Rows[i]["部门"].ToString().Trim() : string.Empty;
                            if (mode > 0)
                            {
                                deptname = dt.Columns.Contains("外协单位") ? dt.Rows[i]["外协单位"].ToString().Trim() : string.Empty;  //外协单位
                            }
                            //专业/班组
                            string teamname = dt.Columns.Contains("专业/班组") ? dt.Rows[i]["专业/班组"].ToString().Trim() : string.Empty;
                            //岗位/职务
                            string dutyname = dt.Columns.Contains("岗位/职务") ? dt.Rows[i]["岗位/职务"].ToString().Trim() : string.Empty;
                            //违章过程描述
                            string lllegaldescribe = dt.Columns.Contains("违章过程描述") ? dt.Rows[i]["违章过程描述"].ToString().Trim() : string.Empty;
                            //违章分类
                            string lllegaltypename = dt.Columns.Contains("违章分类") ? dt.Rows[i]["违章分类"].ToString().Trim() : string.Empty;
                            //处罚时间
                            string punishdate = dt.Columns.Contains("处罚时间") ? dt.Rows[i]["处罚时间"].ToString().Trim() : string.Empty;
                            //处罚结果(元)
                            string punishresult = dt.Columns.Contains("处罚结果(元)") ? dt.Rows[i]["处罚结果(元)"].ToString() : string.Empty;
                            //处罚积分(分)
                            string punishpoint = dt.Columns.Contains("处罚积分(分)") ? dt.Rows[i]["处罚积分(分)"].ToString() : string.Empty;

                            string relevanceid = string.Empty;
                            try
                            {
                                #region 对象集合
                                ImportLllegalPunish entity = new ImportLllegalPunish();
                                //序号
                                entity.serialnumber = i + 1; //序号
                                entity.lllegaldescribe = lllegaldescribe;
                                entity.lllegaltypename = lllegaltypename;
                                //部门/承包商层级
                                if (!string.IsNullOrEmpty(deptname))
                                {
                                    var deptlist = dlist.Where(p => p.FullName == deptname && p.Nature != "专业" && p.Nature != "班组");
                                    if (deptlist.Count() > 0)
                                    {
                                        var deptentity = deptlist.FirstOrDefault();
                                        entity.deptid = deptentity.DepartmentId;
                                        entity.deptname = deptentity.FullName;
                                        entity.nature = deptentity.Nature;
                                        var parentdeptEntity = dlist.Where(p => p.DepartmentId == deptentity.ParentId).FirstOrDefault();
                                        if (deptentity.Nature == "承包商" && parentdeptEntity.Nature != "部门")
                                        {
                                            var rpdeptEntity = GetRootContractor(deptentity, dlist);
                                            entity.deptid = rpdeptEntity.DepartmentId;
                                            entity.deptname = rpdeptEntity.FullName;
                                            entity.nature = rpdeptEntity.Nature;
                                            entity.teamid = deptentity.DepartmentId;
                                            entity.teamname = deptentity.FullName;
                                            entity.tnature = deptentity.Nature;
                                        }
                                    }
                                }
                                //专业/班组
                                if (!string.IsNullOrEmpty(teamname))
                                {
                                    var deptlist = dlist.Where(p => p.FullName == teamname && (p.Nature == "专业" || p.Nature == "班组" || p.Nature=="承包商"));
                                    if (deptlist.Count() > 0)
                                    {
                                        string parentcode = string.Empty;
                                        if (!string.IsNullOrEmpty(entity.deptid))
                                        {
                                            parentcode= dlist.Where(p => p.DepartmentId == entity.deptid).FirstOrDefault().EnCode;
                                            var deptentity = deptlist.FirstOrDefault();
                                            //专业和班组、承包商的专业和班组必须来自于对应的部门
                                            if (deptentity.EnCode.StartsWith(parentcode) && deptentity.EnCode != parentcode)
                                            {
                                                entity.teamid = deptentity.DepartmentId;
                                                entity.teamname = deptentity.FullName;
                                                entity.tnature = deptentity.Nature;
                                            }
                                        }
                                    }
                                }

                                #region 人员
                                if (!string.IsNullOrEmpty(username))
                                {
                                    List<UserEntity> userlist = ulist.Where(p => p.RealName == username.Trim() || p.Account == username.Trim() || p.Mobile == username.Trim() || p.Telephone == username.Trim()).ToList();
                                    if (!string.IsNullOrEmpty(entity.deptid) && !string.IsNullOrEmpty(entity.teamid))
                                    {
                                        userlist = userlist.Where(p => p.DepartmentId == entity.teamid).ToList();
                                        if (userlist.Count() > 0)
                                        {
                                            var checkUserEntity = userlist.FirstOrDefault();
                                            entity.userid = checkUserEntity.UserId;
                                            entity.username = checkUserEntity.RealName;
                                            entity.dutyid = checkUserEntity.DutyId;
                                            entity.dutyname = checkUserEntity.DutyName;
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(entity.deptid) && string.IsNullOrEmpty(entity.teamid))
                                    {
                                        userlist = userlist.Where(p => p.DepartmentId == entity.deptid).ToList();
                                        if (userlist.Count() > 0)
                                        {
                                            var checkUserEntity = userlist.FirstOrDefault();
                                            entity.userid = checkUserEntity.UserId;
                                            entity.username = checkUserEntity.RealName;
                                            entity.dutyid = checkUserEntity.DutyId;
                                            entity.dutyname = checkUserEntity.DutyName;
                                        }
                                    }
                                }
                                #endregion

                                //处罚时间
                                if (!string.IsNullOrEmpty(punishdate))
                                {
                                    entity.punishdate = punishdate;
                                }
                                else
                                {
                                    entity.punishdate = DateTime.Now.ToString("yyyy-MM-dd");
                                }

                                //违章类型
                                if (!string.IsNullOrEmpty(lllegaltypename))
                                {
                                    var checktypeEntity = lllegaltypelist.Where(p => p.ItemName == lllegaltypename.ToString()).FirstOrDefault();
                                    if (null != checktypeEntity)
                                    {
                                        entity.lllegaltypename = checktypeEntity.ItemName;
                                        entity.lllegaltype = checktypeEntity.ItemDetailId;
                                    }
                                }
                                #endregion

                                #region 必填验证

                                if (!string.IsNullOrEmpty(username))
                                {
                                    if (!string.IsNullOrEmpty(entity.teamid) && string.IsNullOrEmpty(entity.userid))
                                    {
                                        resultmessage += "人员不存在于专业/班组中、";
                                        isadddobj = false;
                                    }
                                    else if (!string.IsNullOrEmpty(entity.deptid) && string.IsNullOrEmpty(entity.teamid) && string.IsNullOrEmpty(entity.userid))
                                    {
                                        resultmessage += "人员不存在于" + labledept + "中、";
                                        isadddobj = false;
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(entity.userid))
                                        {
                                            resultmessage += "人员填写错误或不存在、";
                                            isadddobj = false;
                                        }
                                    }
                                }

                                if (string.IsNullOrEmpty(deptname))
                                {
                                    resultmessage += labledept + "为空、";
                                    isadddobj = false;
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(entity.deptid))
                                    {
                                        resultmessage += labledept + "填写错误或不存在、";
                                        isadddobj = false;
                                    }
                                }

                                if (!string.IsNullOrEmpty(teamname))
                                {
                                    if (!string.IsNullOrEmpty(entity.deptid) && string.IsNullOrEmpty(entity.teamid))
                                    {
                                        resultmessage += "专业/班组不存在于对应的" + labledept + "、";
                                        isadddobj = false;
                                    }
                                }

                                if (!string.IsNullOrEmpty(entity.nature))
                                {
                                    if ((mode == 0 && entity.nature == "承包商"))
                                    {
                                        resultmessage += "生产部门导入模板中不应存在外协单位数据、";
                                        isadddobj = false;
                                    }
                                    if ((mode == 1 && entity.nature != "承包商"))
                                    {
                                        resultmessage += "外协单位导入模板中不应存在生产部门数据、";
                                        isadddobj = false;
                                    }
                                }

                                if (string.IsNullOrEmpty(lllegaldescribe))
                                {
                                    resultmessage += "违章过程描述为空、";
                                    isadddobj = false;
                                }
                                if (string.IsNullOrEmpty(lllegaltypename))
                                {
                                    resultmessage += "违章分类为空、";
                                    isadddobj = false;
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(entity.lllegaltype))
                                    {
                                        resultmessage += "违章分类不存在、";
                                        isadddobj = false;
                                    }
                                }

                                if (string.IsNullOrEmpty(punishresult))
                                {
                                    resultmessage += "处罚结果(元)为空、";
                                    isadddobj = false;
                                }
                                else
                                {
                                    entity.punishresult = int.Parse(punishresult);
                                }

                                if (string.IsNullOrEmpty(punishpoint))
                                {
                                    resultmessage += "处罚积分(分)为空、";
                                    isadddobj = false;
                                }
                                else
                                {
                                    entity.punishpoint = int.Parse(punishpoint);
                                }

                                if (isadddobj)
                                {
                                    list.Add(entity);
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(resultmessage))
                                    {
                                        resultmessage = resultmessage.Substring(0, resultmessage.Length - 1) + ",无法正常导入";
                                        resultlist.Add(resultmessage);
                                    }
                                }
                                #endregion
                            }
                            catch
                            {
                                resultmessage += "出现数据异常,无法正常导入";
                                resultlist.Add(resultmessage);
                            }
                        }
                        if (resultlist.Count > 0)
                        {
                            foreach (string str in resultlist)
                            {
                                falseMessage += str + "</br>";
                            }
                        }
                        #endregion
                        #region 问题数据集合

                        foreach (ImportLllegalPunish entity in list)
                        {
                            string keyValue = string.Empty;
                            int excuteVal = 0;
                            //违章档案扣分信息
                            LllegalDeductMarksEntity baseentity = new LllegalDeductMarksEntity();

                            //获取已存在的违章问题
                            if (!string.IsNullOrEmpty(entity.userid))
                            {
                                var llist = lllegaldeductmarksbll.GetLllegalRecorList(entity.punishdate, entity.userid,entity.lllegaldescribe, entity.deptid,entity.teamid);

                                if (llist.Count() > 0)
                                {
                                    var otherwz = llist.Where(p => p.CREATEUSERID != curUser.UserId);
                                    //其他人创建的
                                    if (otherwz.Count() > 0)
                                    {
                                        falseMessage += "人员为'" + entity.username + "'于" + entity.punishdate + "处罚的数据因已被其他人创建而无法覆盖,不予操作</br>";
                                        excuteVal = -1;
                                    }
                                    else //自己创建
                                    {
                                        baseentity = llist.FirstOrDefault();
                                        //先删除，后新增
                                        lllegaldeductmarksbll.RemoveForm(baseentity.ID);
                                        baseentity = new LllegalDeductMarksEntity();
                                        excuteVal = 1;
                                    }
                                }
                                else
                                {
                                    excuteVal = 1;
                                }
                            }
                            else
                            {
                                excuteVal = 1;
                            }

                            if (excuteVal > 0)
                            {
                                baseentity.APPSIGN = "3"; //标识导入的
                                baseentity.USERID = entity.userid;
                                baseentity.USERNAME = entity.username;
                                baseentity.DEPTID = entity.deptid;
                                baseentity.DEPTNAME = entity.deptname;
                                baseentity.TEAMID = entity.teamid;
                                baseentity.TEAMNAME = entity.teamname;
                                baseentity.DUTYNAME = entity.dutyname;
                                baseentity.LLLEGALDESCRIBE = entity.lllegaldescribe;
                                baseentity.LLLEGALTYPE = entity.lllegaltype;
                                baseentity.LLLEGALTYPENAME = entity.lllegaltypename;
                                baseentity.PUNISHDATE = Convert.ToDateTime(entity.punishdate);
                                baseentity.PUNISHRESULT = entity.punishresult;
                                baseentity.PUNISHPOINT = entity.punishpoint;
                                lllegaldeductmarksbll.SaveForm("", baseentity);
                                total += 1;
                            }
                        }
                        #endregion
                    }
                    #endregion
                    count = dt.Rows.Count;
                    message = "共有" + count.ToString() + "条记录,成功导入" + total.ToString() + "条,失败" + (count - total).ToString() + "条";
                    message += "</br>" + falseMessage;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return message;
        }
        #endregion

        public DepartmentEntity GetRootContractor(DepartmentEntity entity, List<DepartmentEntity> dlist)
        {
            var parentEntity = dlist.Where(p => p.DepartmentId == entity.ParentId).FirstOrDefault();
            if (entity.Nature == "承包商" && parentEntity.Nature == "承包商")
            {
                return GetRootContractor(parentEntity, dlist);
            }
            else
            {
                return entity;
            }
        }
    }
}