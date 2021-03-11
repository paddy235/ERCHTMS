using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.Offices;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.FireManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Cache;
using ERCHTMS.Code;
using ERCHTMS.Entity.FireManage;
using ERCHTMS.Entity.SystemManage.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ThoughtWorks.QRCode.Codec;

namespace ERCHTMS.Web.Areas.FireManage.Controllers
{
    /// <summary>
    /// 描 述：消防设施
    /// </summary>
    public class FirefightingController : MvcControllerBase
    {
        private FirefightingBLL firefightingbll = new FirefightingBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private readonly DepartmentBLL departBLL = new DepartmentBLL();
        private readonly UserBLL userBLL = new UserBLL();
        private readonly OrganizeBLL orgBLL = new OrganizeBLL();

        #region 视图功能
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
            if (ehsDepart != null)
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
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult BuilderImage()
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

        /// <summary>
        /// 灭火器表单视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MHQForm()
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
            queryJson = queryJson ?? "";
            pagination.p_kid = "Id";
            pagination.p_fields = "EquipmentName,EquipmentNameNo,EquipmentCode,ExtinguisherType,ExtinguisherTypeNo,Specifications,District,DutyUser,dutyuserid,DutyDept,NextExamineDate,NextDetectionDate,NextFillDate,createuserid,createuserdeptcode,createuserorgcode,CreateDate,ExamineUser,ExamineUserId,TerminalDetectionDate,TerminalDetectionUnit,TerminalDetectionVerdict,TerminalDetectionPeriod,NextTerminalDetectionDate";
            pagination.p_tablename = "HRS_FIREFIGHTING";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            System.Diagnostics.Stopwatch watch = CommonHelper.TimerStart();
            DataTable data = firefightingbll.GetPageList(pagination, queryJson);
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
            System.Collections.Generic.IEnumerable<FirefightingEntity> data = firefightingbll.GetList(queryJson);
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
            FirefightingEntity data = firefightingbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取统计数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult StatisticsData(string queryJson)
        {
            DataTable data = firefightingbll.StatisticsData(queryJson);
            var jsonData = new
            {
                rows = data
            };
            return ToJsonResult(jsonData);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult Remove(string keyValue)
        {
            firefightingbll.Remove(keyValue);
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
        public ActionResult RemoveForm(string keyValue)
        {
            firefightingbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, FirefightingEntity entity)
        {
            firefightingbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion

        /// <summary>
        /// 编号不能重复
        /// </summary>
        /// <param name="Type">设施类型</param>
        /// <param name="EquipmentCode">编号</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExistCode(string Type, string EquipmentCode, string keyValue)
        {
            bool IsOk = firefightingbll.ExistCode(Type, EquipmentCode, keyValue);
            return Content(IsOk.ToString());
        }
        /// <summary>
        /// 数据复制
        /// </summary>
        /// <param name="Type">设施类型</param>
        /// <param name="EquipmentCode">编号</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DataCopy(string keyValue)
        {
            FirefightingEntity data = firefightingbll.GetEntity(keyValue);
            data.Id = null;
            data.EquipmentCode = null;
            firefightingbll.SaveForm(data.Id, data);
            return Success("操作成功。");
        }
        /// <summary>
        /// 批量生成二维码并导出到word
        /// </summary>
        /// <param name="equipIds"></param>
        /// <param name="equipNames"></param>
        /// <param name="equipNos"></param>
        /// <param name="equiptype"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        [HandlerAuthorize(PermissionMode.Ignore)]
        public ActionResult BuilderImg(string equipIds, string equipNames, string equipNos, string equiptype)
        {
            if (!System.IO.Directory.Exists(Server.MapPath("~/Resource/qrcode")))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Resource/qrcode"));
            }

            Aspose.Words.Document doc = new Aspose.Words.Document(Server.MapPath("~/Resource/ExcelTemplate/消防设施二维码打印.doc"));
            Aspose.Words.DocumentBuilder db = new Aspose.Words.DocumentBuilder(doc);
            DataTable dt = new DataTable("U");
            dt.Columns.Add("BigEWM2");
            dt.Columns.Add("PersonName");
            dt.Columns.Add("PersonNo");
            int i = 0;
            string fileName = "";
            string[] equipNameArr = equipNames.Split(',');
            string[] equipNoArr = equipNos.Split(',');
            foreach (string code in equipIds.Split(','))
            {
                DataRow dr = dt.NewRow();
                dr[1] = equipNameArr[i];
                dr[2] = equipNoArr[i];

                fileName = code + ".jpg";
                string filePath = Server.MapPath("~/Resource/qrcode/" + fileName);
                if (!System.IO.File.Exists(filePath))
                {
                    BuilderImg10(code, filePath, equiptype);
                }
                dr[0] = Server.MapPath("~/Resource/qrcode/" + fileName);
                dt.Rows.Add(dr);
                i++;
            }

            doc.MailMerge.Execute(dt);
            fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
            doc.Save(Server.MapPath("~/Resource/temp/" + fileName));
            return Success("生成成功", new { fileName = fileName });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="filePath"></param>
        /// <param name="equiptype"></param>
        public void BuilderImg10(string keyValue, string filePath, string equiptype)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeVersion = 10;
            qrCodeEncoder.QRCodeScale = 2;
            qrCodeEncoder.QRCodeForegroundColor = System.Drawing.Color.Black;
            System.Drawing.Image image = qrCodeEncoder.Encode(keyValue + "|" + equiptype, Encoding.UTF8);
            image.Save(filePath, ImageFormat.Jpeg);
            image.Dispose();
        }


        /// <summary>
        /// 消防设施导出
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        //[AjaxOnly]
        //[HandlerAuthorize(PermissionMode.Ignore)]
        //[HandlerMonitor(0, "导出消防设施excel")]
        public ActionResult ExportFirefightingExcel(string condition, string queryJson)
        {
            string colunmStr = @"EquipmentName,EquipmentNameNo,EquipmentCode,ExtinguisherType,ExtinguisherTypeNo,Specifications,District,DutyDept,DutyUser,
to_char(NextExamineDate,'yyyy-MM-dd') as NextExamineDate,
to_char(NextDetectionDate,'yyyy-MM-dd') as NextDetectionDate,
to_char(NextFillDate,'yyyy-MM-dd') as NextFillDate,
to_char(NextTerminalDetectionDate,'yyyy-MM-dd') as NextTerminalDetectionDate";

            DatabaseType dataType = DbHelper.DbType;
            if (queryJson != null && queryJson != "")
            {
                var queryParam = queryJson.ToJObject();

                //查询条件 名称
                if (!queryParam["EquipmentNameNo"].IsEmpty())
                {
                    if (queryParam["EquipmentNameNo"].ToString() == "MHQ")
                    {
                        colunmStr = @"EquipmentName,EquipmentNameNo,EquipmentCode,ExtinguisherType,ExtinguisherTypeNo,Specifications,District,DutyDept,DutyUser,
to_char(NextExamineDate,'yyyy-MM-dd') as NextExamineDate,
to_char(NextFillDate,'yyyy-MM-dd') as NextFillDate,
to_char(NextDetectionDate,'yyyy-MM-dd') as NextDetectionDate";
                    }
                    else {
                        colunmStr = @"EquipmentName,EquipmentNameNo,EquipmentCode,ExtinguisherType,ExtinguisherTypeNo,Specifications,District,DutyDept,DutyUser,
to_char(NextExamineDate,'yyyy-MM-dd') as NextExamineDate,
to_char(NextDetectionDate,'yyyy-MM-dd') as NextDetectionDate,
to_char(NextTerminalDetectionDate,'yyyy-MM-dd') as NextTerminalDetectionDate";
                    }
                }
            }

            Pagination pagination = new Pagination
            {
                page = 1,
                rows = 100000000,
                p_kid = "Id",
                p_fields = colunmStr,
                p_tablename = "HRS_FIREFIGHTING",
                conditionJson = "1=1",
                sidx = "CreateDate",
                sord = "desc"
            };
            DataTable data = firefightingbll.GetPageList(pagination, queryJson);


            //设置导出格式
            //ExcelConfig excelconfig = new ExcelConfig
            //{
            //    Title = "消防设施",
            //    TitleFont = "微软雅黑",
            //    TitlePoint = 25,
            //    FileName = "消防设施" + ".xls",
            //    IsAllSizeColumn = true,
            //    //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            //    ColumnEntity = new List<ColumnEntity>()
            //{
            //    new ColumnEntity() {Column = "equipmentname", ExcelColumn = "设备名称", Alignment = "center"},
            //    new ColumnEntity() {Column = "equipmentcode", ExcelColumn = "设备编号", Alignment = "center"},
            //    new ColumnEntity() {Column = "extinguishertype", ExcelColumn = "类型", Alignment = "center"},
            //    new ColumnEntity() {Column = "specifications", ExcelColumn = "规格型号", Alignment = "center"},
            //    new ColumnEntity() {Column = "district", ExcelColumn = "配置区域", Alignment = "center"},
            //    new ColumnEntity() {Column = "dutydept", ExcelColumn = "责任部门", Alignment = "center"},
            //    new ColumnEntity() {Column = "dutyuser", ExcelColumn = "责任人",  Alignment = "center"},
            //    new ColumnEntity() {Column = "nextexaminedate", ExcelColumn = "下次检查时间", Alignment = "center"},
            //    new ColumnEntity() {Column = "nextfilldate", ExcelColumn = "下次充装/更换时间", Alignment = "center"},
            //    new ColumnEntity() {Column = "nextdetectiondate", ExcelColumn = "下次检测时间", Alignment = "center"}
            //}
            //};

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "消防设施";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "消防设施.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "equipmentname", ExcelColumn = "设备名称", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "equipmentcode", ExcelColumn = "设备编号", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "extinguishertype", ExcelColumn = "类型", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "specifications", ExcelColumn = "规格型号", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "district", ExcelColumn = "配置区域", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "dutydept", ExcelColumn = "责任部门", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "dutyuser", ExcelColumn = "责任人", Alignment = "center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "nextexaminedate", ExcelColumn = "下次检查时间", Alignment = "center" });

            if (queryJson != null && queryJson != "")
            {
                var queryParam = queryJson.ToJObject();

                //查询条件 名称
                if (!queryParam["EquipmentNameNo"].IsEmpty())
                {
                    if (queryParam["EquipmentNameNo"].ToString() == "MHQ")
                    {
                        listColumnEntity.Add(new ColumnEntity() { Column = "nextfilldate", ExcelColumn = "下次充装/更换时间", Alignment = "center" });
                    }
                    else
                    {
                        listColumnEntity.Add(new ColumnEntity() { Column = "nextdetectiondate", ExcelColumn = "下次维保时间", Alignment = "center" });
                        listColumnEntity.Add(new ColumnEntity() { Column = "nextterminaldetectiondate", ExcelColumn = "下次检测时间", Alignment = "center" });
                    }
                }
                else
                {
                    listColumnEntity.Add(new ColumnEntity() { Column = "nextdetectiondate", ExcelColumn = "下次维保时间", Alignment = "center" });
                    listColumnEntity.Add(new ColumnEntity() { Column = "nextfilldate", ExcelColumn = "下次充装/更换时间", Alignment = "center" });
                    listColumnEntity.Add(new ColumnEntity() { Column = "nextterminaldetectiondate", ExcelColumn = "下次检测时间", Alignment = "center" });
                }
            }
            else
            {
                listColumnEntity.Add(new ColumnEntity() { Column = "nextdetectiondate", ExcelColumn = "下次维保时间", Alignment = "center" });
                listColumnEntity.Add(new ColumnEntity() { Column = "nextfilldate", ExcelColumn = "下次充装/更换时间", Alignment = "center" });
                listColumnEntity.Add(new ColumnEntity() { Column = "nextterminaldetectiondate", ExcelColumn = "下次检测时间", Alignment = "center" });
            }
            
            
            

            excelconfig.ColumnEntity = listColumnEntity;
            
            //调用导出方法
            ExcelHelper.ExportByAspose(data, excelconfig.FileName, listColumnEntity);
            return Success("导出成功。");
        }

        /// <summary>
        /// 导入消防设施
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //[AjaxOnly]
        //[HandlerAuthorize(PermissionMode.Ignore)]
        // [ValidateAntiForgeryToken]
        public string ImportFirefighting()
        {
            if (OperatorProvider.Provider.Current().IsSystem)
            {
                return "超级管理员无此操作权限";
            }
            string orgId = OperatorProvider.Provider.Current().OrganizeId;//所属公司          
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
                //string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                //file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName));
                //DataTable dt = ExcelHelper.ExcelImport(Server.MapPath("~/Resource/temp/" + fileName));

                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName));
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                wb.Open(Server.MapPath("~/Resource/temp/" + fileName));
                Aspose.Cells.Cells cells = wb.Worksheets[0].Cells;
                DataTable dt = cells.ExportDataTable(0, 0, cells.MaxDataRow+1, cells.MaxColumn+1, true);

                string orgid = OperatorProvider.Provider.Current().OrganizeId;
                int order = 1;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    FirefightingEntity item = new FirefightingEntity();
                    order = i + 1;
                    #region 设备名称
                    string equipmentName = dt.Rows[i][0].ToString();
                    if (!string.IsNullOrEmpty(equipmentName))
                    {
                        item.EquipmentName = equipmentName;
                        var data = new DataItemCache().ToItemValue("EquipmentName", equipmentName);
                        if (data != null && !string.IsNullOrEmpty(data))
                            item.EquipmentNameNo = data;
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,设备名称不存在！</br>", order);
                            error++;
                            continue;
                        }
                    }
                    else
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,设备名称不能为空！</br>", order);
                        error++;
                        continue;
                    }
                    #endregion

                    if (item.EquipmentNameNo == "MHQ")
                    {
                        #region 设备编号
                        string equipmentNo = dt.Rows[i][1].ToString();
                        if (!string.IsNullOrEmpty(equipmentNo))
                            item.EquipmentCode = equipmentNo;
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,设备编号不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 类型
                        string extinguisherType = dt.Rows[i][2].ToString();
                        if (!string.IsNullOrEmpty(extinguisherType))
                        {
                            item.ExtinguisherType = extinguisherType;
                            if (equipmentName == "灭火器")
                            {
                                var data = new DataItemCache().ToItemValue("ExtinguisherType", extinguisherType);
                                if (data != null && !string.IsNullOrEmpty(data))
                                    item.ExtinguisherTypeNo = data;
                                else
                                {
                                    falseMessage += string.Format(@"第{0}行导入失败,灭火器类型不存在！</br>", order);
                                    error++;
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,类型不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 数量
                        string Amount = dt.Rows[i][3].ToString();
                        int tempAmount;
                        if (!string.IsNullOrEmpty(Amount))
                            if (int.TryParse(Amount, out tempAmount))
                                item.Amount = tempAmount;
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,数量必须为数字！</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,数量不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 数量单位
                        string AmountUnit = dt.Rows[i][4].ToString();
                        if (!string.IsNullOrEmpty(AmountUnit))
                        {
                            var data = new DataItemCache().ToItemValue("AmountUnit", AmountUnit);
                            if (data != null && !string.IsNullOrEmpty(data))
                                item.AmountUnit = data;
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,数量单位不存在！</br>", order);
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,数量单位不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 规格型号
                        string specifications = dt.Rows[i][3 + 2].ToString();
                        if (!string.IsNullOrEmpty(specifications))
                            item.Specifications = specifications;
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,规格型号不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 出厂时间
                        string leaveDate = dt.Rows[i][4 + 2].ToString();
                        DateTime tempDate1;
                        if (!string.IsNullOrEmpty(leaveDate))
                            if (DateTime.TryParse(leaveDate, out tempDate1))
                                item.LeaveDate = tempDate1;
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,出厂时间不对！</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,出厂时间不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 检查周期（天）
                        string examinePeriod = dt.Rows[i][5 + 2].ToString();
                        int tempPeriod;
                        if (!string.IsNullOrEmpty(examinePeriod))
                            if (int.TryParse(examinePeriod, out tempPeriod))
                                item.ExaminePeriod = tempPeriod;
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,检查周期必须为数字！</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,检查周期不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 检查时间
                        string examineDate = dt.Rows[i][6 + 2].ToString();
                        DateTime tempDate2;
                        if (!string.IsNullOrEmpty(examineDate))
                            if (DateTime.TryParse(examineDate, out tempDate2))
                                item.ExamineDate = tempDate2;
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,检查时间不对！</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,检查时间不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 下次检查时间（年月日）
                        string nextExamineDate = dt.Rows[i][7 + 2].ToString();
                        DateTime tempDate3;
                        if (!string.IsNullOrEmpty(nextExamineDate))
                            if (DateTime.TryParse(nextExamineDate, out tempDate3))
                                item.NextExamineDate = tempDate3;
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,下次检查时间不对！</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,下次检查时间不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 检查周期（天）
                        string fillPeriod = dt.Rows[i][8 + 2].ToString();
                        int tempFillPeriod;
                        if (!string.IsNullOrEmpty(fillPeriod))
                            if (int.TryParse(fillPeriod, out tempFillPeriod))
                                item.FillPeriod = tempFillPeriod;
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,充装/更换周期必须为数字！</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,充装/更换周期不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 上次充装时间（年月）   new CultureInfo("zh-CN")
                        string lastFillDate = dt.Rows[i][9 + 2].ToString();
                        DateTime tempDate4;
                        if (!string.IsNullOrEmpty(lastFillDate))
                            if (DateTime.TryParse(lastFillDate, out tempDate4))
                                item.LastFillDate = tempDate4;
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,上次充装/更换时间不对！</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,上次充装/更换时间不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 下次充装时间（年月）
                        string nextFillDate = dt.Rows[i][10 + 2].ToString();
                        DateTime tempDate5;
                        if (!string.IsNullOrEmpty(nextFillDate))
                            // if (DateTime.TryParseExact(nextFillDate, "yyyy-MM-dd", null, DateTimeStyles.None, out DateTime tempDate))
                            if (DateTime.TryParse(nextFillDate, out tempDate5))
                                item.NextFillDate = tempDate5;
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,下次充装/更换时间不对！</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,下次充装/更换时间不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 配置区域
                        string district = dt.Rows[i][11 + 2].ToString();
                        if (!string.IsNullOrEmpty(district))
                        {
                            var disItem = new DistrictBLL().GetListForCon(x => x.DistrictName == district && x.OrganizeId == orgid).FirstOrDefault();
                            if (disItem != null)
                            {
                                item.DistrictId = disItem.DistrictID;
                                item.DistrictCode = disItem.DistrictCode;
                                item.District = district;
                            }
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,配置区域不存在！</br>", order);
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,配置区域不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 配置位置
                        string location = dt.Rows[i][12 + 2].ToString();
                        if (!string.IsNullOrEmpty(location))
                            item.Location = location;
                        //else
                        //{
                        //    falseMessage += string.Format(@"第{0}行导入失败,配置位置不能为空！</br>", order);
                        //    error++;
                        //    continue;
                        //}
                        #endregion

                        #region 责任部门
                        string dutydept = dt.Rows[i][13 + 2].ToString();
                        if (!string.IsNullOrEmpty(dutydept))
                        {
                            var data = departBLL.GetList().FirstOrDefault(e => e.FullName == dutydept && e.OrganizeId == orgid);
                            if (data != null)
                            {
                                item.DutyDept = dutydept;
                                item.DutyDeptCode = data.EnCode;
                            }

                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,责任部门不存在！</br>", order);
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,责任部门不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 责任人
                        string dutyUser = dt.Rows[i][14 + 2].ToString();
                        if (!string.IsNullOrEmpty(dutyUser))
                        {
                            var userEntity = userBLL.GetListForCon(x => x.RealName == dutyUser && x.OrganizeId == orgid).FirstOrDefault();
                            if (userEntity != null)
                            {
                                item.DutyUserId = userEntity.UserId;
                                item.DutyUser = dutyUser;
                            }
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,责任人不存在！</br>", order);
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,责任人不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 责任人电话
                        string dutyTel = dt.Rows[i][15 + 2].ToString();
                        if (!string.IsNullOrEmpty(dutyTel))
                        {
                            item.DutyTel = dutyTel;
                        }
                        #endregion

                        #region 检查部门
                        string examineDept = dt.Rows[i][16 + 2].ToString();
                        if (!string.IsNullOrEmpty(examineDept))
                        {
                            var data = departBLL.GetList().FirstOrDefault(e => e.FullName == examineDept && e.OrganizeId == orgid);
                            if (data != null)
                            {
                                item.ExamineDept = examineDept;
                                item.ExamineDeptCode = data.EnCode;
                            }

                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,检查部门不存在！</br>", order);
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,检查部门不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 检查人
                        string examineUser = dt.Rows[i][17 + 2].ToString();
                        if (!string.IsNullOrEmpty(examineUser))
                        {
                            var userEntity = userBLL.GetListForCon(x => x.RealName == examineUser && x.OrganizeId == orgid).FirstOrDefault();
                            if (userEntity != null)
                            {
                                item.ExamineUserId = userEntity.UserId;
                                item.ExamineUser = examineUser;
                            }
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,检查人不存在！</br>", order);
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,检查人不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 备注
                        string remark = dt.Rows[i][18 + 2].ToString();
                        if (!string.IsNullOrEmpty(remark))
                        {
                            item.Remark = remark;
                        }
                        #endregion
                    }
                    else
                    {
                        #region 设备编号
                        string equipmentNo = dt.Rows[i][1].ToString();
                        if (!string.IsNullOrEmpty(equipmentNo))
                            item.EquipmentCode = equipmentNo;
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,设备编号不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 数量
                        string Amount = dt.Rows[i][2].ToString();
                        int tempAmount;
                        if (!string.IsNullOrEmpty(Amount))
                            if (int.TryParse(Amount, out tempAmount))
                                item.Amount = tempAmount;
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,数量必须为数字！</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,数量不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        int index = 0;
                        if (item.EquipmentNameNo == "SNXHS" || item.EquipmentNameNo == "SWXHS") {
                            index = 1;
                        }

                        #region 类型
                        string extinguisherType = dt.Rows[i][2 + index].ToString();
                        if (!string.IsNullOrEmpty(extinguisherType))
                        {
                            item.ExtinguisherType = extinguisherType;
                            var data = new DataItemCache().ToItemValue("ExtinguisherType", extinguisherType);
                            if (data != null && !string.IsNullOrEmpty(data))
                                item.ExtinguisherTypeNo = data;
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,类型不存在！</br>", order);
                                error++;
                                continue;
                            }
                        }
                        #endregion

                        #region 数量单位
                        string AmountUnit = dt.Rows[i][3 + index].ToString();
                        if (!string.IsNullOrEmpty(AmountUnit))
                        {
                            var data = new DataItemCache().ToItemValue("AmountUnit", AmountUnit);
                            if (data != null && !string.IsNullOrEmpty(data))
                                item.AmountUnit = data;
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,数量单位不存在！</br>", order);
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,数量单位不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 规格型号
                        string specifications = dt.Rows[i][2 + 2 + index].ToString();
                        if (!string.IsNullOrEmpty(specifications))
                            item.Specifications = specifications;
                        //else
                        //{
                        //    falseMessage += string.Format(@"第{0}行导入失败,规格型号不能为空！</br>", order);
                        //    error++;
                        //    continue;
                        //}
                        #endregion

                        #region 责任部门
                        string dutydept = dt.Rows[i][3 + 2 + index].ToString();
                        if (!string.IsNullOrEmpty(dutydept))
                        {
                            var data = departBLL.GetList().FirstOrDefault(e => e.FullName == dutydept && e.OrganizeId == orgid);
                            if (data != null)
                            {
                                item.DutyDept = dutydept;
                                item.DutyDeptCode = data.EnCode;
                            }

                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,责任部门不存在！</br>", order);
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,责任部门不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 责任人
                        string dutyUser = dt.Rows[i][4 + 2 + index].ToString();
                        if (!string.IsNullOrEmpty(dutyUser))
                        {
                            var userEntity = userBLL.GetListForCon(x => x.RealName == dutyUser && x.OrganizeId == orgid).FirstOrDefault();
                            if (userEntity != null)
                            {
                                item.DutyUserId = userEntity.UserId;
                                item.DutyUser = dutyUser;
                            }
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,责任人不存在！</br>", order);
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,责任人不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 责任人电话
                        string dutyTel = dt.Rows[i][5 + 2 + index].ToString();
                        if (!string.IsNullOrEmpty(dutyTel))
                        {
                            item.DutyTel = dutyTel;
                        }
                        #endregion

                        #region 检查部门
                        string examineDept = dt.Rows[i][6 + 2 + index].ToString();
                        if (!string.IsNullOrEmpty(examineDept))
                        {
                            var data = departBLL.GetList().FirstOrDefault(e => e.FullName == examineDept && e.OrganizeId == orgid);
                            if (data != null)
                            {
                                item.ExamineDept = examineDept;
                                item.ExamineDeptCode = data.EnCode;
                            }

                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,检查部门不存在！</br>", order);
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,检查部门不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 检查人
                        string examineUser = dt.Rows[i][7 + 2 + index].ToString();
                        if (!string.IsNullOrEmpty(examineUser))
                        {
                            var userEntity = userBLL.GetListForCon(x => x.RealName == examineUser && x.OrganizeId == orgid).FirstOrDefault();
                            if (userEntity != null)
                            {
                                item.ExamineUserId = userEntity.UserId;
                                item.ExamineUser = examineUser;
                            }
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,检查人不存在！</br>", order);
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,检查人不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 配置区域
                        string district = dt.Rows[i][8 + 2 + index].ToString();
                        if (!string.IsNullOrEmpty(district))
                        {
                            var disItem = new DistrictBLL().GetListForCon(x => x.DistrictName == district && x.OrganizeId == orgid).FirstOrDefault();
                            if (disItem != null)
                            {
                                item.DistrictId = disItem.DistrictID;
                                item.DistrictCode = disItem.DistrictCode;
                                item.District = district;
                            }
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,配置区域不存在！</br>", order);
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,配置区域不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 配置位置
                        string location = dt.Rows[i][9 + 2 + index].ToString();
                        if (!string.IsNullOrEmpty(location))
                            item.Location = location;
                        //else
                        //{
                        //    falseMessage += string.Format(@"第{0}行导入失败,配置位置不能为空！</br>", order);
                        //    error++;
                        //    continue;
                        //}
                        #endregion

                        string examineDate = null;
                        string examinePeriod = null;
                        string nextExamineDate = null;
                        string protectobject = null;
                        string mainparameter = null;
                        string designunit = null;
                        string installunit = null;
                        string donedate = null;
                        string safeguardunit = null;
                        string detectionunit = null;
                        string detectionDate = null;
                        string detectionVerdict = null;
                        string detectionPeriod = null;
                        string nextDetectionDate = null;
                        string terminalDetectionunit = null;
                        string terminalDetectionDate = null;
                        string terminalDetectionVerdict = null;
                        string terminalDetectionPeriod = null;
                        string nextTerminalDetectionDate = null;
                        string remark = null;
                        if (item.EquipmentNameNo == "SNXHS" || item.EquipmentNameNo == "SWXHS")
                        {

                            #region 枪头数
                            string spearhead = dt.Rows[i][10 + 2 + index].ToString();
                            int tempSpearhead;
                            if (!string.IsNullOrEmpty(spearhead))
                                if (int.TryParse(spearhead, out tempSpearhead))
                                    item.Spearhead = tempSpearhead;
                                else
                                {
                                    falseMessage += string.Format(@"第{0}行导入失败,枪头数必须为数字！</br>", order);
                                    error++;
                                    continue;
                                }
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,枪头数不能为空！</br>", order);
                                error++;
                                continue;
                            }
                            #endregion

                            #region 水带数
                            string waterbelt = dt.Rows[i][11 + 2 + index].ToString();
                            int tempWaterBelt;
                            if (!string.IsNullOrEmpty(waterbelt))
                                if (int.TryParse(waterbelt, out tempWaterBelt))
                                    item.WaterBelt = tempWaterBelt;
                                else
                                {
                                    falseMessage += string.Format(@"第{0}行导入失败,水带数必须为数字！</br>", order);
                                    error++;
                                    continue;
                                }
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,水带数不能为空！</br>", order);
                                error++;
                                continue;
                            }
                            #endregion

                            examineDate = dt.Rows[i][12 + 2 + index].ToString();
                            examinePeriod = dt.Rows[i][13 + 2 + index].ToString();
                            nextExamineDate = dt.Rows[i][14 + 2 + index].ToString();
                            protectobject = dt.Rows[i][15 + 2 + index].ToString();
                            mainparameter = dt.Rows[i][16 + 2 + index].ToString();
                            designunit = dt.Rows[i][17 + 2 + index].ToString();
                            installunit = dt.Rows[i][18 + 2 + index].ToString();
                            donedate = dt.Rows[i][19 + 2 + index].ToString();
                            safeguardunit = dt.Rows[i][20 + 2 + index].ToString();
                            detectionunit = dt.Rows[i][21 + 2 + index].ToString();
                            detectionDate = dt.Rows[i][22 + 2 + index].ToString();
                            detectionVerdict = dt.Rows[i][23 + 2 + index].ToString();
                            detectionPeriod = dt.Rows[i][24 + 2 + index].ToString();
                            nextDetectionDate = dt.Rows[i][25 + 2 + index].ToString();
                            terminalDetectionunit = dt.Rows[i][26 + 2 + index].ToString();
                            terminalDetectionDate = dt.Rows[i][27 + 2 + index].ToString();
                            terminalDetectionVerdict = dt.Rows[i][28 + 2 + index].ToString();
                            terminalDetectionPeriod = dt.Rows[i][29 + 2 + index].ToString();
                            nextTerminalDetectionDate = dt.Rows[i][30 + 2 + index].ToString();
                            remark = dt.Rows[i][31 + 2 + index].ToString();
                        }
                        else
                        {
                            examineDate = dt.Rows[i][10 + 2 + index].ToString();
                            examinePeriod = dt.Rows[i][11 + 2 + index].ToString();
                            nextExamineDate = dt.Rows[i][12 + 2 + index].ToString();
                            protectobject = dt.Rows[i][13 + 2 + index].ToString();
                            mainparameter = dt.Rows[i][14 + 2 + index].ToString();
                            designunit = dt.Rows[i][15 + 2 + index].ToString();
                            installunit = dt.Rows[i][16 + 2 + index].ToString();
                            donedate = dt.Rows[i][17 + 2 + index].ToString();
                            safeguardunit = dt.Rows[i][18 + 2 + index].ToString();
                            detectionunit = dt.Rows[i][19 + 2 + index].ToString();
                            detectionDate = dt.Rows[i][20 + 2 + index].ToString();
                            detectionVerdict = dt.Rows[i][21 + 2 + index].ToString();
                            detectionPeriod = dt.Rows[i][22 + 2 + index].ToString();
                            nextDetectionDate = dt.Rows[i][23 + 2 + index].ToString();
                            terminalDetectionunit = dt.Rows[i][24 + 2 + index].ToString();
                            terminalDetectionDate = dt.Rows[i][25 + 2 + index].ToString();
                            terminalDetectionVerdict = dt.Rows[i][26 + 2 + index].ToString();
                            terminalDetectionPeriod = dt.Rows[i][27 + 2 + index].ToString();
                            nextTerminalDetectionDate = dt.Rows[i][28 + 2 + index].ToString();
                            remark = dt.Rows[i][29 + 2 + index].ToString();
                        }

                        #region 检查时间
                        //string examineDate = dt.Rows[i][10].ToString();
                        DateTime tempDate2;
                        if (!string.IsNullOrEmpty(examineDate))
                            if (DateTime.TryParse(examineDate, out tempDate2))
                                item.ExamineDate = tempDate2;
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,检查时间不对！</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,检查时间不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 检查周期（天）
                        //string examinePeriod = dt.Rows[i][11].ToString();
                        int tempPeriod;
                        if (!string.IsNullOrEmpty(examinePeriod))
                            if (int.TryParse(examinePeriod, out tempPeriod))
                                item.ExaminePeriod = tempPeriod;
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,检查周期必须为数字！</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,检查周期不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 下次检查时间（年月日）
                        //string nextExamineDate = dt.Rows[i][12].ToString();
                        DateTime tempDate3;
                        if (!string.IsNullOrEmpty(nextExamineDate))
                            if (DateTime.TryParse(nextExamineDate, out tempDate3))
                                item.NextExamineDate = tempDate3;
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,下次检查时间不对！</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,下次检查时间不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 保护对象
                        //string protectobject = dt.Rows[i][13].ToString();
                        if (!string.IsNullOrEmpty(protectobject))
                            item.ProtectObject = protectobject;
                        //else
                        //{
                        //    falseMessage += string.Format(@"第{0}行导入失败,保护对象不能为空！</br>", order);
                        //    error++;
                        //    continue;
                        //}
                        #endregion

                        #region 主要参数
                        //string mainparameter = dt.Rows[i][14].ToString();
                        if (!string.IsNullOrEmpty(mainparameter))
                            item.MainParameter = mainparameter;
                        //else
                        //{
                        //    falseMessage += string.Format(@"第{0}行导入失败,主要参数不能为空！</br>", order);
                        //    error++;
                        //    continue;
                        //}
                        #endregion

                        #region 设计单位
                        //string designunit = dt.Rows[i][15].ToString();
                        if (!string.IsNullOrEmpty(designunit))
                            item.DesignUnit = designunit;
                        //else
                        //{
                        //    falseMessage += string.Format(@"第{0}行导入失败,设计单位不能为空！</br>", order);
                        //    error++;
                        //    continue;
                        //}
                        #endregion

                        #region 安装单位
                        //string installunit = dt.Rows[i][16].ToString();
                        if (!string.IsNullOrEmpty(installunit))
                            item.InstallUnit = installunit;
                        //else
                        //{
                        //    falseMessage += string.Format(@"第{0}行导入失败,安装单位不能为空！</br>", order);
                        //    error++;
                        //    continue;
                        //}
                        #endregion

                        #region 竣工时间
                        //string donedate = dt.Rows[i][17].ToString();
                        DateTime tempDoneDate;
                        if (!string.IsNullOrEmpty(donedate))
                            if (DateTime.TryParse(donedate, out tempDoneDate))
                                item.DoneDate = tempDoneDate;
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,竣工时间不对！</br>", order);
                                error++;
                                continue;
                            }
                        //else
                        //{
                        //    falseMessage += string.Format(@"第{0}行导入失败,竣工时间不能为空！</br>", order);
                        //    error++;
                        //    continue;
                        //}
                        #endregion

                        #region 维护单位
                        //string safeguardunit = dt.Rows[i][18].ToString();
                        if (!string.IsNullOrEmpty(safeguardunit))
                            item.SafeguardUnit = safeguardunit;
                        //else
                        //{
                        //    falseMessage += string.Format(@"第{0}行导入失败,维护单位不能为空！</br>", order);
                        //    error++;
                        //    continue;
                        //}
                        #endregion

                        #region 维保单位
                        //string detectionunit = dt.Rows[i][19].ToString();
                        if (!string.IsNullOrEmpty(detectionunit))
                            item.DetectionUnit = detectionunit;
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,维保单位不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 维保时间
                        //string detectionDate = dt.Rows[i][20].ToString();
                        DateTime tempDetectionDate;
                        if (!string.IsNullOrEmpty(detectionDate))
                            if (DateTime.TryParse(detectionDate, out tempDetectionDate))
                                item.DetectionDate = tempDetectionDate;
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,维保时间不对！</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,维保时间不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 维保结论
                        //string detectionVerdict = dt.Rows[i][21].ToString();
                        if (!string.IsNullOrEmpty(detectionVerdict))
                        {
                            if (detectionVerdict == "合格") item.DetectionVerdict = 1;
                            else if (detectionVerdict == "不合格") item.DetectionVerdict = 0;
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,维保结论不存在！</br>", order);
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,维保结论不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 维保周期（天）
                        //string detectionPeriod = dt.Rows[i][22].ToString();
                        int tempDetectionPeriod;
                        if (!string.IsNullOrEmpty(detectionPeriod))
                            if (int.TryParse(detectionPeriod, out tempDetectionPeriod))
                                item.DetectionPeriod = tempDetectionPeriod;
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,维保周期必须为数字！</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,维保周期不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 下次维保时间（年月日）
                        //string nextDetectionDate = dt.Rows[i][23].ToString();
                        DateTime tempNextDetectionDate;
                        if (!string.IsNullOrEmpty(nextDetectionDate))
                            if (DateTime.TryParse(nextDetectionDate, out tempNextDetectionDate))
                                item.NextDetectionDate = tempNextDetectionDate;
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,下次维保时间不对！</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,下次维保时间不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 检测单位
                        //string detectionunit = dt.Rows[i][19].ToString();
                        if (!string.IsNullOrEmpty(terminalDetectionunit))
                            item.TerminalDetectionUnit = terminalDetectionunit;
                        //else
                        //{
                        //    falseMessage += string.Format(@"第{0}行导入失败,检测单位不能为空！</br>", order);
                        //    error++;
                        //    continue;
                        //}
                        #endregion

                        #region 检测时间
                        //string detectionDate = dt.Rows[i][20].ToString();
                        DateTime tempTerminalDetectionDate;
                        if (!string.IsNullOrEmpty(terminalDetectionDate))
                            if (DateTime.TryParse(terminalDetectionDate, out tempTerminalDetectionDate))
                                item.TerminalDetectionDate = tempTerminalDetectionDate;
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,检测时间不对！</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,检测时间不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 检测结论
                        //string detectionVerdict = dt.Rows[i][21].ToString();
                        if (!string.IsNullOrEmpty(terminalDetectionVerdict))
                        {
                            if (terminalDetectionVerdict == "合格") item.TerminalDetectionVerdict = 0;
                            else if (terminalDetectionVerdict == "不合格") item.TerminalDetectionVerdict = 1;
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,检测结论不存在！</br>", order);
                                error++;
                                continue;
                            }
                        }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,检测结论不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 检测周期（天）
                        //string detectionPeriod = dt.Rows[i][22].ToString();
                        int tempTerminalDetectionPeriod;
                        if (!string.IsNullOrEmpty(terminalDetectionPeriod))
                            if (int.TryParse(terminalDetectionPeriod, out tempTerminalDetectionPeriod))
                                item.TerminalDetectionPeriod = tempTerminalDetectionPeriod;
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,检测周期必须为数字！</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,检测周期不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 下次检测时间（年月日）
                        //string nextDetectionDate = dt.Rows[i][23].ToString();
                        DateTime tempNextTerminalDetectionDate;
                        if (!string.IsNullOrEmpty(nextTerminalDetectionDate))
                            if (DateTime.TryParse(nextTerminalDetectionDate, out tempNextTerminalDetectionDate))
                                item.NextTerminalDetectionDate = tempNextTerminalDetectionDate;
                            else
                            {
                                falseMessage += string.Format(@"第{0}行导入失败,下次检测时间不对！</br>", order);
                                error++;
                                continue;
                            }
                        else
                        {
                            falseMessage += string.Format(@"第{0}行导入失败,下次检测时间不能为空！</br>", order);
                            error++;
                            continue;
                        }
                        #endregion

                        #region 备注
                        //string remark = dt.Rows[i][24].ToString();
                        if (!string.IsNullOrEmpty(remark))
                        {
                            item.Remark = remark;
                        }
                        #endregion
                    }


                    if (!firefightingbll.ExistCode(item.EquipmentNameNo, item.EquipmentCode, ""))
                    {
                        falseMessage += string.Format(@"第{0}行导入失败,设备编号为{1}的{2}已存在！</br>", order, item.EquipmentCode, item.EquipmentName);
                        error++;
                        continue;
                    }

                    try
                    {
                        firefightingbll.SaveForm("", item);
                    }
                    catch
                    {
                        error++;
                    }

                }
                count = dt.Rows.Count;
                message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条";
                message += "</br>" + falseMessage;
            }

            return message;
        }
    }
}
