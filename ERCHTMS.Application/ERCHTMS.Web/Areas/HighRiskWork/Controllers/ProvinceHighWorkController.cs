using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.AuthorizeManage;
using System.Linq;
using ERCHTMS.Busines.HighRiskWork;
using ERCHTMS.Code;
using System.Data;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Web.Areas.HighRiskWork.Controllers
{
    /// <summary>
    /// 描 述：省级页面
    /// </summary>
    public class ProvinceHighWorkController : MvcControllerBase
    {
        private ProvinceHighWorkBLL provincehighworkbll = new ProvinceHighWorkBLL();

        #region 视图功能
        /// <summary>
        /// 高风险清单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 统计图页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult StatisticsIndex()
        {
            return View();
        }
        #endregion


        /// <summary>
        ///作业类型统计(统计图)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetProvinceHighCount(string starttime, string endtime, string deptid, string deptcode)
        {
            return provincehighworkbll.GetProvinceHighCount(starttime, endtime, deptid, deptcode);
        }

        /// <summary>
        ///作业类型统计(统计表格)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetProvinceHighList(string starttime, string endtime, string deptid, string deptcode)
        {
            return provincehighworkbll.GetProvinceHighList(starttime, endtime, deptid, deptcode);
        }

        /// <summary>
        /// 单位对比(统计图)
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetProvinceHighDepartCount(string starttime, string endtime)
        {
            return provincehighworkbll.GetProvinceHighDepartCount(starttime, endtime);
        }

        /// <summary>
        ///单位对比(统计表格)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetProvinceHighDepartList(string starttime, string endtime)
        {
            return provincehighworkbll.GetProvinceHighDepartList(starttime, endtime);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageTableJson(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_kid = "a.Id";
            pagination.p_fields = "worktype,b.itemname worktypename,applynumber,workdepttypename,engineeringname,workplace,workstarttime,workendtime,workdeptname,a.createusername,a.createdate,c.fullname as createuserorgname";
            pagination.p_tablename = " v_highriskstat a left join base_dataitemdetail b on a.worktype=b.itemvalue and itemid =(select itemid from base_dataitem where itemcode='StatisticsType') left join base_department c on a.createuserorgcode=c.encode";
            pagination.conditionJson = string.Format("WorkDeptCode in (select encode from base_department start with encode='{0}' connect by  prior departmentid = parentid)", user.NewDeptCode);
            var data = provincehighworkbll.GetPageDataTable(pagination, queryJson);
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

        #region 导出
        /// <summary>
        /// 导出
        /// </summary>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult ExportData(string queryJson)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.p_kid = "a.Id";
                pagination.p_fields = "b.itemname worktypename,applynumber,workdepttypename,engineeringname,workplace,to_char(workstarttime,'yyyy-mm-dd hh24:mi') || ' - '||to_char(workendtime,'yyyy-mm-dd hh24:mi') as worktime,workdeptname,a.createusername,a.createdate,c.fullname as createuserorgname";
                pagination.p_tablename = " v_highriskstat a left join base_dataitemdetail b on a.worktype=b.itemvalue and itemid =(select itemid from base_dataitem where itemcode='StatisticsType') left join base_department c on a.createuserorgcode=c.encode";
                pagination.conditionJson = string.Format("WorkDeptCode in (select encode from base_department start with encode='{0}' connect by  prior departmentid = parentid)", user.NewDeptCode);
                pagination.sidx = "a.createdate";//排序字段
                pagination.sord = "desc";//排序方式

                DataTable exportTable = provincehighworkbll.GetPageDataTable(pagination, queryJson);
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "高风险作业信息";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "高风险作业信息.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "worktypename", ExcelColumn = "作业类型", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "applynumber", ExcelColumn = "申请编号", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workdepttypename", ExcelColumn = "作业单位类别", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "engineeringname", ExcelColumn = "工程名称", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workplace", ExcelColumn = "作业地点", Width = 60 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "worktime", ExcelColumn = "作业时间", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "workdeptname", ExcelColumn = "作业单位", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createusername", ExcelColumn = "申请人", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createdate", ExcelColumn = "申请时间", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createuserorgname", ExcelColumn = "所属电厂", Width = 20 });
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
