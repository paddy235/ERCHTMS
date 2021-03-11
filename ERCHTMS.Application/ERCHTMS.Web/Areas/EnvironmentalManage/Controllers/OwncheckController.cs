using ERCHTMS.Entity.EnvironmentalManage;
using ERCHTMS.Busines.EnvironmentalManage;
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

namespace ERCHTMS.Web.Areas.EnvironmentalManage.Controllers
{
    /// <summary>
    /// 描 述：自行检测
    /// </summary>
    public class OwncheckController : MvcControllerBase
    {
        private OwncheckBLL owncheckbll = new OwncheckBLL();
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
            ViewBag.Code= owncheckbll.GetMaxCode();
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
            Operator user = OperatorProvider.Provider.Current();
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "ID";
            pagination.p_fields = "checkcode,dataname,uploadpersonname,uploadpersonid,to_char(uploadtime,'yyyy-MM-dd') as uploadtime,createuserid,createuserdeptcode,createuserorgcode,createdate,createusername";
            pagination.p_tablename = " bis_owncheck ";
            pagination.conditionJson = "1=1";
            pagination.sidx = "createdate";
            pagination.sord = "desc";
            if (!user.IsSystem)
            {
                pagination.conditionJson += " and createuserorgcode='" + user.OrganizeCode + "'";
            }
            var data = owncheckbll.GetPageList(pagination, queryJson);
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
            var data = owncheckbll.GetList(queryJson);
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
            var data = owncheckbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 导出
        /// </summary>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult ExportData(string queryJson)
        {
            try
            {
                var queryParam = queryJson.ToJObject();
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.p_kid = "ID";
                pagination.p_fields = "dataname,checkcode,to_char(uploadtime,'yyyy-MM-dd') as uploadtime,uploadpersonname";
                pagination.p_tablename = " bis_owncheck ";
                pagination.conditionJson = "1=1";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (!user.IsSystem)
                {
                    pagination.conditionJson += " and createuserorgcode='" + user.OrganizeCode + "'";
                }
                var data = owncheckbll.GetPageList(pagination, queryJson);
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();

                excelconfig.Title = "自行检测";
                excelconfig.FileName = "自行检测信息导出.xls";

                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;

                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "dataname", ExcelColumn = "资料名称", Width = 300 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkcode", ExcelColumn = "编号", Width = 300 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "uploadtime", ExcelColumn = "上传时间", Width = 300 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "uploadpersonname", ExcelColumn = "上传人员", Width = 300 });
                

                //调用导出方法
                //ExcelHelper.ExcelDownload(exportTable, excelconfig);
                ExcelHelper.ExportByAspose(data, excelconfig.FileName, excelconfig.ColumnEntity);
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
            owncheckbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, OwncheckEntity entity)
        {
            owncheckbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
