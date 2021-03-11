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

namespace ERCHTMS.Web.Areas.SafetyLawManage.Controllers
{
    /// <summary>
    /// 描 述：事故案例库
    /// </summary>
    public class AccidentCaseLawController : MvcControllerBase
    {
        private AccidentCaseLawBLL accidentcaselawbll = new AccidentCaseLawBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();

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
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CaseForm()
        {
            return View();
        }
        /// <summary>
        /// 我的收藏
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
            pagination.p_kid = "Id";
            pagination.p_fields = "CreateDate,FileName,AccRange,AccTime,Remark,FilesId,AccidentCompany,RelatedCompany,AccidentGrade,intDeaths,AccType,createuserid,createuserdeptcode,createuserorgcode";
            pagination.p_tablename = " bis_accidentCaseLaw";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                IEnumerable<DepartmentEntity> orgcodelist = new List<DepartmentEntity>();
                //电厂获取省公司的机构ID
                if (user.RoleName.Contains("省级用户"))
                {
                    pagination.conditionJson += " and ( createuserorgcode = '" + user.OrganizeCode + "' or createuserorgcode='00')";
                }
                else
                {
                    orgcodelist = departmentBLL.GetList().Where(t => user.NewDeptCode.Contains(t.DeptCode) && t.Nature == "省级");
                    pagination.conditionJson += " and (";
                    foreach (DepartmentEntity item in orgcodelist)
                    {
                        pagination.conditionJson += "createuserorgcode ='" + item.EnCode + "' or ";
                    }
                    pagination.conditionJson += " createuserorgcode = '" + user.OrganizeCode + "' or createuserorgcode='00')";
                }
            }
            var data = accidentcaselawbll.GetPageDataTable(pagination, queryJson);
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
            var data = accidentcaselawbll.GetList(queryJson);
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
            var data = accidentcaselawbll.GetEntity(keyValue);
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
            accidentcaselawbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, AccidentCaseLawEntity entity)
        {
            entity.CaseSource = "0";//内部数据
            accidentcaselawbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
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
                pagination.p_fields = @"FileName,RelatedCompany,AccTime,case when AccidentGrade='1' then '一般事故'
                                            when AccidentGrade ='2' then '较大事故'
                                            when AccidentGrade='3'  then '重大事故'
                                            when AccidentGrade='4'  then '特别重大事故' end AccidentGrade,intDeaths,AccType,
                                        case when AccRange='1' then '本单位事故'
                                            when AccRange ='2' then '本集团事故'
                                            when AccRange='3'  then '电力系统内容事故' end AccRange
                                    ,Remark,FilesId,AccidentCompany";
                pagination.p_tablename = " bis_accidentCaseLaw";
                pagination.conditionJson = "1=1";
                pagination.sidx = "createdate";//排序字段
                pagination.sord = "desc";//排序方式
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (!user.IsSystem)
                {
                    IEnumerable<DepartmentEntity> orgcodelist = new List<DepartmentEntity>();
                    //电厂获取省公司的机构ID
                    if (user.RoleName.Contains("省级用户"))
                    {
                        pagination.conditionJson += " and ( createuserorgcode = '" + user.OrganizeCode + "' or createuserorgcode='00')";
                    }
                    else
                    {
                        orgcodelist = departmentBLL.GetList().Where(t => user.NewDeptCode.Contains(t.DeptCode) && t.Nature == "省级");
                        pagination.conditionJson += " and (";
                        foreach (DepartmentEntity item in orgcodelist)
                        {
                            pagination.conditionJson += "createuserorgcode ='" + item.EnCode + "' or ";
                        }
                        pagination.conditionJson += " createuserorgcode = '" + user.OrganizeCode + "' or createuserorgcode='00')";
                    }
                }
                DataTable exportTable = accidentcaselawbll.GetPageDataTable(pagination, queryJson);
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "事故案例信息";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "事故案例信息导出.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filename", ExcelColumn = "事故名称", Width = 50 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "relatedcompany", ExcelColumn = "涉事单位", Width = 50 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "acctime", ExcelColumn = "事故时间", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "accidentgrade", ExcelColumn = "事故等级", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "intdeaths", ExcelColumn = "死亡人数", Width = 30 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "acctype", ExcelColumn = "事故类别", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "accrange", ExcelColumn = "数据范围", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "remark", ExcelColumn = "备注", Width = 15 });
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
