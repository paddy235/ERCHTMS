using ERCHTMS.Entity.AccidentEvent;
using ERCHTMS.Busines.AccidentEvent;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Code;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Linq;

namespace ERCHTMS.Web.Areas.AccidentEvent.Controllers
{
    /// <summary>
    /// 描 述：WSSJBG
    /// </summary>
    public class WSSJBGController : MvcControllerBase
    {
        private WSSJBGBLL wssjbgbll = new WSSJBGBLL();

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
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Select()
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
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>   
        //[HandlerMonitor(3, "分页查询用户信息!")]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            queryJson = queryJson ?? "";
            pagination.p_kid = "ID";
            pagination.p_fields = "IsSubmit,CREATEUSERID,WSSJNAME, WSSJTYPENAME,HAPPENTIME,AREANAME,WSSJBGUSERNAME,CREATEUSERDEPTCODE as departmentcode,CREATEUSERORGCODE as  organizecode";
            pagination.p_tablename = "V_AEM_WSSJBG_Order t";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "CREATEUSERDEPTCODE", "CREATEUSERORGCODE");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }

            }

            var watch = CommonHelper.TimerStart();
            var data = wssjbgbll.GetPageList(pagination, queryJson);
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




        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = wssjbgbll.GetList(queryJson);
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
            var data = wssjbgbll.GetEntity(keyValue);
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
            Wssjbg_dealBLL wssjbg_dealbll = new Wssjbg_dealBLL();
            foreach (var item in keyValue.Split(','))
            {
                wssjbgbll.RemoveForm(item);
                Expression<Func<Wssjbg_dealEntity, bool>> condition = e => e.WssjbgId == item;
                var list = wssjbg_dealbll.GetListForCon(condition);
                if (list != null && list.Count() > 0)
                {
                    var id = list.FirstOrDefault().Id;
                    wssjbg_dealbll.RemoveForm(id);
                }

            }

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
        public ActionResult SaveForm(string keyValue, WSSJBGEntity entity)
        {
            wssjbgbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion

        #region 数据导出
        /// <summary>
        /// 未遂事件报告
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "未遂事件报告")]
        public ActionResult ExportWssjbgList(string condition, string queryJson)
        {
            Pagination pagination = new Pagination();
            queryJson = queryJson ?? "";
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "ID";
            pagination.p_fields = "WSSJNAME, WSSJTYPENAME,HAPPENTIME,AREANAME,WSSJBGUSERNAME,case WHEN  IsSubmit>0 then '已提交' else '未提交' end  as IsSubmitStr";
            pagination.p_tablename = "V_AEM_WSSJBG_ORDER t";
            pagination.sord = "HAPPENTIME";
            #region 权限校验
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "CREATEUSERDEPTCODE", "CREATEUSERORGCODE");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }

            }
            #endregion
            var data = wssjbgbll.GetPageList(pagination, queryJson);

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "未遂事件报告";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "未遂事件报告.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "WSSJNAME".ToLower(), ExcelColumn = "未遂事件名称" });
            listColumnEntity.Add(new ColumnEntity() { Column = "WSSJTYPENAME".ToLower(), ExcelColumn = "未遂事件类型" });
            listColumnEntity.Add(new ColumnEntity() { Column = "HAPPENTIME".ToLower(), ExcelColumn = "发生时间" });
            listColumnEntity.Add(new ColumnEntity() { Column = "AREANAME".ToLower(), ExcelColumn = "地点（区域）" });
            listColumnEntity.Add(new ColumnEntity() { Column = "WSSJBGUSERNAME".ToLower(), ExcelColumn = "报告人" });
            listColumnEntity.Add(new ColumnEntity() { Column = "IsSubmitStr".ToLower(), ExcelColumn = "是否提交" });
            excelconfig.ColumnEntity = listColumnEntity;

            //调用导出方法
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("导出成功。");
        }
        #endregion
    }
}
