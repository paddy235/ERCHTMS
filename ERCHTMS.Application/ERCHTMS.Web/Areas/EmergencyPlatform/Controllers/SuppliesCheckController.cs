using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.Busines.EmergencyPlatform;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Data;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Web.Areas.EmergencyPlatform.Controllers
{
    /// <summary>
    /// 描 述：应急物资检查
    /// </summary>
    public class SuppliesCheckController : MvcControllerBase
    {
        private SuppliesCheckBLL suppliescheckbll = new SuppliesCheckBLL();

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
            var data = suppliescheckbll.GetList(queryJson);
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
            var data = suppliescheckbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.conditionJson = " 1=1 ";

                var data = suppliescheckbll.GetPageList(pagination, queryJson);
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
            catch (System.Exception ex)
            {
                return Error(ex.ToString());
            }
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult ExportData(string queryJson)
        {
            try
            {
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 100000;
                pagination.conditionJson = " 1=1 ";
                var data = suppliescheckbll.GetPageList(pagination, queryJson);
                DataTable excelTable = new DataTable();
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "应急物资检查";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "应急物资检查" + DateTime.Now.ToString("yyyyMMdd") + ".xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkdate", ExcelColumn = "检查日期", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkusername", ExcelColumn = "检查人员", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkuserdept", ExcelColumn = "检查单位", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkdetail", ExcelColumn = "应急物资检查情况", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createusername", ExcelColumn = "创建人", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createdate", ExcelColumn = "创建时间", Width = 40 });
                //调用导出方法
                ExcelHelper.ExcelDownload(data, excelconfig);
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
            try
            {
                suppliescheckbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SuppliesCheckEntity entity)
        {
            try
            {
                suppliescheckbll.SaveForm(keyValue, entity);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
            
        }
        #endregion
    }
}
