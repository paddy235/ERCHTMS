using ERCHTMS.Entity.RoutineSafetyWork;
using ERCHTMS.Busines.RoutineSafetyWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System.Data;
using System;
using Newtonsoft.Json.Linq;
using ERCHTMS.Busines.PublicInfoManage;

namespace ERCHTMS.Web.Areas.RoutineSafetyWork.Controllers
{
    /// <summary>
    /// 描 述：通知公告
    /// </summary>
    public class AnnouncementController : MvcControllerBase
    {
        private AnnouncementBLL announcementbll = new AnnouncementBLL();
        private AnnounDetailBLL announdetailbll = new AnnounDetailBLL();

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
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.conditionJson = " 1=1 ";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");

                var data = announcementbll.GetPageList(pagination, queryJson, authType);
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
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }  
           
        }
        /// <summary>
        /// 获取通知公告详情列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetMessDetailListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "t.id";
                pagination.p_fields = @"t.auuounid,t.username,t.useraccount,t.userid,t.looktime,t.deptid,t.deptname,t.deptcode,t.status";
                pagination.p_tablename = @"bis_announdetail t";
                pagination.sidx = "t.looktime";//排序字段
                pagination.sord = "desc";//排序方式
                Operator currUser = OperatorProvider.Provider.Current();
                pagination.conditionJson = "  1=1 ";

                var data = announdetailbll.GetPageList(pagination, queryJson);
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

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetIndexJson(Pagination pagination, string queryJson)
        {

            var watch = CommonHelper.TimerStart();
            pagination.conditionJson = " 1=1 ";
            pagination.page = 1;
            pagination.rows = 8;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string authType="";
            if (!user.IsSystem)
            {
                if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级"))
                {
                    authType = "4";
                }
                else
                {
                    authType = "3";
                }
            }
            var data = announcementbll.GetPageList(pagination, queryJson, authType);
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
            var data = announcementbll.GetList(queryJson);
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
            var data = announcementbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取实体和附件信息 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormAndFile(string keyValue)
        {
            var data = announcementbll.GetEntity(keyValue);
            var fileList = new FileInfoBLL().GetFileList(keyValue);
            var jsonData = new
            {
                data = data,
                fileList = fileList,
            };
            return ToJsonResult(jsonData);
        }

        /// <summary>
        /// 更新未查看状态
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult UpdateStatus(string keyValue)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var detail = announdetailbll.GetEntity(user.UserId, keyValue);
                if (detail != null)
                {
                    detail.Status = 1;
                    detail.LookTime = DateTime.Now;
                    announdetailbll.SaveForm(detail.Id, detail);
                }
                return Success("操作成功。");
            }
            catch (Exception ex)
            {

                return Error(ex.Message);
            }
          

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
                announcementbll.RemoveForm(keyValue);
                return Success("删除成功。");
            }
            catch (Exception)
            {
                
                throw;
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
        [ValidateInput(false)]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, AnnouncementEntity entity)
        {
            try
            {
                if (entity.IsSend=="0")//发布时间已发送时间为准
                {
                    entity.ReleaseTime = DateTime.Now;
                }
                announcementbll.SaveForm(keyValue, entity);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {

                return Error(ex.Message);
            }
           
        }
        #endregion

        #region 数据导出
        /// <summary>
        /// 导出用户列表
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出通知公告数据")]
        public ActionResult Export(string queryJson)
        {

            var watch = CommonHelper.TimerStart();
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.conditionJson = " 1=1 ";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
            var data = announcementbll.GetPageList(pagination, queryJson, authType);
            DataTable excelTable = new DataTable();
            excelTable.Columns.Add(new DataColumn("isimportant"));
            excelTable.Columns.Add(new DataColumn("notictype"));
            excelTable.Columns.Add(new DataColumn("title"));
            excelTable.Columns.Add(new DataColumn("publisherdept"));
            excelTable.Columns.Add(new DataColumn("publisher"));
            excelTable.Columns.Add(new DataColumn("releasetime"));

            foreach (DataRow item in data.Rows)
            {
                DataRow newDr = excelTable.NewRow();
                newDr["isimportant"] = item["isimportant"];
                newDr["notictype"] = item["notictype"];
                newDr["title"] = item["title"];
                newDr["publisherdept"] = item["publisherdept"];
                newDr["publisher"] = item["publisher"];

                DateTime releasetime;
                DateTime.TryParse(item["releasetime"].ToString(), out releasetime);
                newDr["releasetime"] = releasetime.ToString("yyyy-MM-dd HH:mm") != "0001-01-01 00:00" ? releasetime.ToString("yyyy-MM-dd HH:mm") : "";
                excelTable.Rows.Add(newDr);
            }
            var query = JObject.Parse(queryJson);
            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "公告";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "公告.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            ColumnEntity columnentity = new ColumnEntity();
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "isimportant", ExcelColumn = "重要", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "notictype", ExcelColumn = "分类", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "title", ExcelColumn = "标题", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "publisherdept", ExcelColumn = "发布部门", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "publisher", ExcelColumn = "发布人", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "releasetime", ExcelColumn = "发布时间", Alignment = "center" });
            //调用导出方法
            ExcelHelper.ExcelDownload(excelTable, excelconfig);

            return Success("导出成功。");
        }
        #endregion
    }
}
