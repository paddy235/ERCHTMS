using ERCHTMS.Entity.FireManage;
using ERCHTMS.Busines.FireManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Busines.SystemManage;
using System.Linq;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System;
using Aspose.Cells;
using System.Drawing;
using System.Web;
using System.Data;

namespace ERCHTMS.Web.Areas.FireManage.Controllers
{
    /// <summary>
    /// 描 述：消防队伍配备
    /// </summary>
    public class FireEquipController : MvcControllerBase
    {
        private FireEquipBLL FireEquipbll = new FireEquipBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
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
            pagination.p_fields = "EquipmentName,Purpose,EquipOne,EquipUnitOne,EquipRatioOne,PracticalEquipOne,PracticalEquipUnitOne,RemarkOne,EquipTwo,EquipUnitTwo,EquipRatioTwo,PracticalEquipTwo,PracticalEquipUnitTwo,RemarkTwo,createusername,createdate,createuserorgcode,createuserdeptcode,createuserid";
            pagination.p_tablename = "HRS_FireEquip";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                pagination.conditionJson += string.Format(" and CREATEUSERORGCODE ='{0}'", user.OrganizeCode);
            }
            var watch = CommonHelper.TimerStart();
            var data = FireEquipbll.GetPageList(pagination, queryJson);
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
            var data = FireEquipbll.GetList(queryJson);
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
            var data = FireEquipbll.GetEntity(keyValue);
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
            FireEquipbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, FireEquipEntity entity)
        {
            FireEquipbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
        #region 数据导出
        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出基本防护装备配备标准")]
        public ActionResult Export(string queryJson)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "rownum idx";
            pagination.p_fields = @"equipmentname,purpose,
(Concat(equipone,equipunitone)) as equipone,
equipratioone,
 (Concat(practicalequipone,practicalequipunitone)) as practicalequipone,
(Concat(equiptwo,equipunittwo)) as equiptwo,
equipratiotwo,
(Concat(practicalequiptwo,practicalequipunittwo)) as practicalequiptwo";
            pagination.p_tablename = "HRS_FIREEQUIP t";
            pagination.conditionJson = string.Format(" 1=1 ");
            pagination.sidx = "createdate";//排序字段
            pagination.sord = "desc";//排序方式  
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                pagination.conditionJson += string.Format(" and CREATEUSERORGCODE ='{0}'", user.OrganizeCode);
            }

            var watch = CommonHelper.TimerStart();
            var exportTable = FireEquipbll.GetPageList(pagination, queryJson);

            //设置导出格式
            //ExcelConfig excelconfig = new ExcelConfig();
            //excelconfig.Title = "基本防护装备配备标准";
            //excelconfig.TitleFont = "微软雅黑";
            //excelconfig.TitlePoint = 16;
            //excelconfig.FileName = "基本防护装备配备标准.xls";
            //excelconfig.IsAllSizeColumn = true;
            ////每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            //List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            //excelconfig.ColumnEntity = listColumnEntity;
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "idx", ExcelColumn = "序号", Alignment = "center" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipmentname", ExcelColumn = "名称", Alignment = "center" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "purpose", ExcelColumn = "主要用途", Alignment = "center" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipone", ExcelColumn = "一级站配备", Alignment = "center" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipratioone", ExcelColumn = "一级站备份比", Alignment = "center" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "practicalequipone", ExcelColumn = "一级站实际配备数量", Alignment = "center" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equiptwo", ExcelColumn = "二级站配备", Alignment = "center" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipratiotwo", ExcelColumn = "二级站备份比", Alignment = "center" });
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "practicalequiptwo", ExcelColumn = "二级站实际配备数量", Alignment = "center" });

            ////调用导出方法
            //ExcelHelper.ExcelDownload(data, excelconfig);
            Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
            string fName = "基本防护装备配备标准_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            wb.Open(Server.MapPath("~/Resource/ExcelTemplate/tmp.xls"));
            var num = wb.Worksheets[0].Cells.Columns.Count;

            Aspose.Cells.Worksheet sheet = wb.Worksheets[0] as Aspose.Cells.Worksheet;
            Aspose.Cells.Cell cell = sheet.Cells[0, 0];
            cell.PutValue("基本防护装备配备标准"); //标题
            cell.Style.Pattern = BackgroundType.Solid;
            cell.Style.Font.Size = 16;
            cell.Style.Font.Color = Color.Black;
            List<string> colList = new List<string>() {  "名称", "主要用途", "一级站配备", "一级站备份比", "一级站实际配备数量", "二级站配备", "二级站备份比", "二级站实际配备数量" };
            List<string> colList1 = new List<string>() { "equipmentname", "purpose", "equipone", "equipratioone", "practicalequipone", "equiptwo", "equipratiotwo", "practicalequiptwo" };
            for (int i = 0; i < colList.Count; i++)
            {
                //序号列
                Aspose.Cells.Cell serialcell = sheet.Cells[1, 0];
                serialcell.PutValue(" ");

                for (int j = 0; j < colList.Count; j++)
                {
                    Aspose.Cells.Cell curcell = sheet.Cells[1, j];
                    //sheet.Cells.SetColumnWidth(j, 40);
                    curcell.Style.Pattern = BackgroundType.Solid;
                    curcell.Style.Font.Size = 12;
                    curcell.Style.Font.Color = Color.Black;
                    curcell.PutValue(colList[j].ToString()); //列头
                }
                Aspose.Cells.Cells cells = sheet.Cells;
                cells.Merge(0, 0, 1, colList.Count);
            }
            for (int i = 0; i < exportTable.Rows.Count; i++)
            {
                //内容填充
                for (int j = 0; j < colList1.Count; j++)
                {
                    Aspose.Cells.Cell curcell = sheet.Cells[i + 2, j];
                    curcell.PutValue(exportTable.Rows[i][colList1[j]].ToString());
                }

            }
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            //wb.Save(Server.MapPath("~/Resource/Temp/" + fName));
            wb.Save(Server.UrlEncode(fName), Aspose.Cells.FileFormatType.Excel2003, Aspose.Cells.SaveType.OpenInBrowser, resp);

            return Success("导出成功。");
        }

        #endregion
    }
}
