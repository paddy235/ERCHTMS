using ERCHTMS.Entity.ComprehensiveManage;
using ERCHTMS.Busines.ComprehensiveManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Web.Areas.ComprehensiveManage.Controllers
{
    /// <summary>
    /// 描 述：通知公告
    /// </summary>
    public class MeetingRecordController : MvcControllerBase
    {
        private MeetingRecordBLL meetingrecordbll = new MeetingRecordBLL();

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
            var data = meetingrecordbll.GetList(queryJson);
            return ToJsonResult(data);
        }
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
            pagination.p_kid = "Id";
            pagination.p_fields = "Security,Name,SettlePerson,IssueTime,ReadUserIdList,IsSend,CreateUserId";
            pagination.p_tablename = "HRS_MEETINGRECORD";
            pagination.conditionJson = "1=1";


            var watch = CommonHelper.TimerStart();
            var data = meetingrecordbll.GetPageList(pagination, queryJson);
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
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = meetingrecordbll.GetEntity(keyValue);
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
            meetingrecordbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue,string isSend, MeetingRecordEntity entity)
        {
            entity.IsSend = isSend;//是否发送
            entity.IssueTime = DateTime.Now;//发送时间
            meetingrecordbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion

        #region 数据导出
        /// <summary>
        /// 会议纪要导出
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult ExportData(string keyValue)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 1;
            pagination.p_kid = "id";
            pagination.p_fields = "Security,Name,SettlePerson,Content,code,meetingtime,Address,direct,attendperson";//,Content,code,meetingtime,address,direct,attendperson,settleperson
            pagination.p_tablename = "HRS_MEETINGRECORD s";
            pagination.conditionJson = "id='" + keyValue + "'";
            //pagination.sidx = "realname";
            //pagination.sord = "desc";
            DataTable dtUser = meetingrecordbll.GetPageList(pagination, "{}");

            dtUser.TableName = "U";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user != null)
            {
                dtUser.Columns.Add("company");
                dtUser.Rows[0]["company"] = user.OrganizeName;
            }

            Aspose.Words.Document doc = new Aspose.Words.Document(Server.MapPath("~/Resource/ExcelTemplate/会议纪要模板.doc"));
            Aspose.Words.DocumentBuilder db = new Aspose.Words.DocumentBuilder(doc);
            doc.MailMerge.Execute(dtUser);

            doc.MailMerge.DeleteFields();
            string filePath = Server.MapPath("~/Resource/temp/" + dtUser.Rows[0]["name"].ToString() + ".doc");
            doc.Save(filePath);

            string url = "../../Utility/DownloadFile?filePath=" + filePath + "&speed=102400&newFileName=" + dtUser.Rows[0]["name"].ToString() + ".doc";
            return Redirect(url);
        }
        #endregion
    }
}
