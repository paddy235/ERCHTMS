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

namespace ERCHTMS.Web.Areas.FireManage.Controllers
{
    /// <summary>
    /// 描 述：消防队伍
    /// </summary>
    public class FireEstimateController : MvcControllerBase
    {
        private FireEstimateBLL FireEstimatebll = new FireEstimateBLL();
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
            pagination.p_fields = "firetypename,carinvest,equipinvest,practicalinvest,createusername,createdate,createuserorgcode,createuserdeptcode,createuserid";
            pagination.p_tablename = "HRS_FireEstimate";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                pagination.conditionJson += string.Format(" and CREATEUSERORGCODE ='{0}'", user.OrganizeCode);
            }
            var watch = CommonHelper.TimerStart();
            var data = FireEstimatebll.GetPageList(pagination, queryJson);
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
            var data = FireEstimatebll.GetList(queryJson);
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
            var data = FireEstimatebll.GetEntity(keyValue);
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
            FireEstimatebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, FireEstimateEntity entity)
        {
            FireEstimatebll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
        #region 数据导出
        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出各类器材投资估算指标")]
        public ActionResult Export(string queryJson)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "rownum idx";
            pagination.p_fields = @"firetypename,carinvest,
equipinvest,
practicalinvest";
            pagination.p_tablename = "HRS_FireEstimate t";
            pagination.conditionJson = string.Format(" 1=1 ");
            pagination.sidx = "createdate";//排序字段
            pagination.sord = "desc";//排序方式  
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                pagination.conditionJson += string.Format(" and CREATEUSERORGCODE ='{0}'", user.OrganizeCode);
            }

            var watch = CommonHelper.TimerStart();
            var data = FireEstimatebll.GetPageList(pagination, queryJson);

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "各类器材投资估算指标";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 16;
            excelconfig.FileName = "各类器材投资估算指标.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "idx", ExcelColumn = "序号", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "firetypename", ExcelColumn = "名称", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "carinvest", ExcelColumn = "车辆投资（万元）", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "equipinvest", ExcelColumn = "装备和器材投资（万元）", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "practicalinvest", ExcelColumn = "实际投入（万元）", Alignment = "center" });
            //调用导出方法
            ExcelHelper.ExcelDownload(data, excelconfig);

            return Success("导出成功。");
        }
        #endregion
    }
}
