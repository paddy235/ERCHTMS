using ERCHTMS.Entity.AccidentEvent;
using ERCHTMS.Busines.AccidentEvent;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Web.Areas.AccidentEvent.Controllers
{
    /// <summary>
    /// 描 述：事故事件调查处理
    /// </summary>
    public class Bulletin_dealController : MvcControllerBase
    {
        private Bulletin_dealBLL bulletin_dealbll = new Bulletin_dealBLL();
        private BulletinBLL bulletinbll = new BulletinBLL();
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
        public ActionResult GenericIndex()
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
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GenericForm()
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
            pagination.p_fields = "IsSubmit_DEAL,CREATEUSERID ,SGNAME, SGTYPENAME,HAPPENTIME,AREANAME,SGKBUSERNAME,DEALID,RSSHSGTYPENAME,CREATEUSERDEPTCODE as departmentcode,CREATEUSERORGCODE as  organizecode";
            pagination.p_tablename = "V_AEM_BULLETIN_DEAL_ORDER t";
            pagination.conditionJson = "1=1";
            //pagination.sord = "HAPPENTIME";
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


            var pMode = Request["pMode"] ?? "";
            if (pMode.Length > 0)
            {
                pagination.conditionJson += " and IsSubmit_Deal>0 and to_char(createdate,'yyyy')='" + DateTime.Now.Year + "'";
            }

            var watch = CommonHelper.TimerStart();
            var data = bulletinbll.GetPageList(pagination, queryJson);
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
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>   
        //[HandlerMonitor(3, "分页查询用户信息!")]
        public ActionResult GetGenericPageListJson(Pagination pagination, string queryJson)
        {
            queryJson = queryJson ?? "";
            pagination.p_kid = "ID";
            pagination.p_fields = "IsSubmit_DEAL,CREATEUSERID ,SGNAME, SGTYPENAME,HAPPENTIME,AREANAME,SGKBUSERNAME,DEALID,RSSHSGTYPENAME,CREATEUSERDEPTCODE as departmentcode,CREATEUSERORGCODE as  organizecode";
            pagination.p_tablename = "V_AEM_BULLETIN_DEAL_ORDER t";
            pagination.conditionJson = "1=1";
            //pagination.sord = "HAPPENTIME";
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


            var pMode = Request["pMode"] ?? "";
            if (pMode.Length > 0)
            {
                pagination.conditionJson += " and IsSubmit_Deal>0 and to_char(createdate,'yyyy')='" + DateTime.Now.Year + "'";
            }

            var watch = CommonHelper.TimerStart();
            var data = bulletinbll.GetGenericPageList(pagination, queryJson);
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
            var data = bulletin_dealbll.GetList(queryJson);
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
            var data = bulletin_dealbll.GetEntity(keyValue);
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
            bulletin_dealbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, Bulletin_dealEntity entity)
        {
            string HAPPENTIME_DEAL = Request["HAPPENTIME_DEAL"] ?? "";
            if (HAPPENTIME_DEAL.Length > 0)
                entity.HAPPENTIME_DEAL = Convert.ToDateTime(HAPPENTIME_DEAL);
            bulletin_dealbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion

        #region 数据导出
        /// <summary>
        /// 事故事件快报
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "事故事件快报")]
        public ActionResult ExportBulletinDealList(string condition, string queryJson)
        {
            Pagination pagination = new Pagination();
            queryJson = queryJson ?? "";
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "ID";
            pagination.p_fields = "SGNAME, SGTYPENAME,HAPPENTIME,AREANAME,SGKBUSERNAME,case WHEN  IsSubmit_DEAL>0 then '已调查处理' else '未调查处理' end  as DCCLZT";
            pagination.p_tablename = "V_AEM_BULLETIN_DEAL_ORDER t";
            pagination.sord = "DCCLZT";
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
            var data = bulletinbll.GetPageList(pagination, queryJson);

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "事故事件调查处理";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "事故事件调查处理.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "SGNAME".ToLower(), ExcelColumn = "事故/事件名称" });
            listColumnEntity.Add(new ColumnEntity() { Column = "SGTYPENAME".ToLower(), ExcelColumn = "事故或事件类型" });
            listColumnEntity.Add(new ColumnEntity() { Column = "HAPPENTIME".ToLower(), ExcelColumn = "发生时间" });
            listColumnEntity.Add(new ColumnEntity() { Column = "AREANAME".ToLower(), ExcelColumn = "地点（区域）" });
            listColumnEntity.Add(new ColumnEntity() { Column = "SGKBUSERNAME".ToLower(), ExcelColumn = "快报人" });
            listColumnEntity.Add(new ColumnEntity() { Column = "DCCLZT".ToLower(), ExcelColumn = "调查处理状态" });
            excelconfig.ColumnEntity = listColumnEntity;

            //调用导出方法
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("导出成功。");
        }
        #endregion
    }
}
