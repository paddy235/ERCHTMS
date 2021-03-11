using BSFramework.Util;
using BSFramework.Util.Offices;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.CarManage;
using ERCHTMS.Busines.MatterManage;
using ERCHTMS.Code;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
/// <summary>
/// 车辆管理
/// </summary>
namespace ERCHTMS.Web.Areas.HJBPerson.Controllers
{
    public class CarVelocityController : MvcControllerBase
    {
        private OperticketmanagerBLL operticketmanagerbll = new OperticketmanagerBLL();
        private HikinoutlogBLL hikinoutlogbll = new HikinoutlogBLL();
        // GET: HJBPerson/CarVelocity
        #region 视图
        public ActionResult Index()
        {
            return View();
        }
        #endregion



        #region 获取数据
        /// <summary>
        /// 加载车辆超速记录
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetListJson(Pagination pagination,string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "ID";
            pagination.p_fields = "CARDNO,vehicleTypeName,DeptName,DIRVER,PHONE,SPEED,ADDRESS,CREATEDATE,VEHICLEPICURL";
            pagination.p_tablename = "BIS_CARVIOLATION";
            pagination.conditionJson = "1=1";
            var data = hikinoutlogbll.Get_BIS_CARVIOLATION(pagination, queryJson);
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



        #region 导出数据
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        /// 
        [HandlerMonitor(0, "车辆测速数据")]
        public ActionResult ExportData(Pagination pagination, string queryJson)
        {
            try {
                pagination.page = 1;
                pagination.rows = 100000000;
                pagination.p_kid = "ID";
                pagination.p_fields = "CARDNO,vehicleTypeName,DeptName,DIRVER,PHONE,SPEED,ADDRESS,CREATEDATE,VEHICLEPICURL,TO_CHAR(CREATEDATE,'yyyy-mm-dd hh24:mi:ss') as datetime";
                pagination.p_tablename = "BIS_CARVIOLATION";
                pagination.conditionJson = "1=1";
                var data = hikinoutlogbll.Get_BIS_CARVIOLATION(pagination, queryJson);

                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "车辆测速数据信息";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 25;
                excelconfig.FileName = "车辆测速数据导出.xls";
                excelconfig.IsAllSizeColumn = true;
                //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "cardno".ToLower(), ExcelColumn = "车牌号码" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "vehicleTypeName".ToLower(), ExcelColumn = "车辆类型" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "deptname".ToLower(), ExcelColumn = "所属单位（部门）" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "dirver".ToLower(), ExcelColumn = "驾驶员" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "phone".ToLower(), ExcelColumn = "驾驶员电话" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "speed".ToLower(), ExcelColumn = "抓拍车速（km/h）" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "address".ToLower(), ExcelColumn = "抓拍地点" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "datetime".ToLower(), ExcelColumn = "时间" });
                //调用导出方法
                ExcelHelper.ExcelDownload(data, excelconfig);
            }
            catch (Exception ex) {

            }
            return Success("导出成功。");
        }
        #endregion

    }
}