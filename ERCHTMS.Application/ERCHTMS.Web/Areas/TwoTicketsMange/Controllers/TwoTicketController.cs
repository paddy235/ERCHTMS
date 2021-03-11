using ERCHTMS.Entity.PersonManage;
using ERCHTMS.Busines.PersonManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Collections.Generic;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Busines.BaseManage;
using System.Web;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Text;
using BSFramework.Cache.Factory;
using System.Linq;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.SystemManage;
using BSFramework.Util.Offices;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Busines.TwoTickets;
using ERCHTMS.Entity.TwoTickets;
namespace ERCHTMS.Web.Areas.TwoTicketsMange.Controllers
{
    /// <summary>
    /// 描 述：三种人审批业务表
    /// </summary>
    public class TwoTicketController : MvcControllerBase
    {
        private TwoTicketsBLL threepeoplecheckbll = new TwoTicketsBLL();

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
        /// 三种人清单列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult List()
        {
            return View();
        }
        /// <summary>
        /// 三种人导入
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }
        /// <summary>
        /// 新增人员
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form2()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form1()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Form3()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Form4()
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
        public ActionResult GetListJson(Pagination pagination, string queryJson,int mode=1)
        {
            try
            {
                Operator user = OperatorProvider.Provider.Current();
                pagination.p_kid = "t.ID";
                pagination.p_fields = "createtime,createusername,worktime,sno,tickettype,deptname,dutyuser,senduser,audituser,tsdsno,audittime,registeruser,registertime,audituser1,address,tutelageuser,createuserid,t.iscommit,t.status,1 issubmit,0 state,content,workpermittime,monitor";
                pagination.p_tablename = "XSS_TWOTICKETS t";
                pagination.conditionJson = string.Format("datatype={1} and (t.iscommit=1 or (t.iscommit=0 and t.createuserid='{0}')) and CREATEUSERDEPTCODE like '{2}%'", user.UserId, mode, user.OrganizeCode);
                var watch = CommonHelper.TimerStart();
                DataTable data = threepeoplecheckbll.GetPageList(pagination, queryJson);
                DepartmentBLL deptBll=new DepartmentBLL();
                foreach(DataRow dr in data.Rows)
                {
                    DataTable dt= deptBll.GetDataTable(string.Format("select iscommit,status from XLD_TWOTICKETRECORD where ticketid='{0}' order by createdate desc",dr["id"].ToString()));
                    if (dt.Rows.Count>0)
                    {
                        dr["issubmit"] = dt.Rows[0]["iscommit"];
                        dr["state"] = dt.Rows[0]["status"];
                    }
                }
                DateTime nowTime = DateTime.Now;
                int weeknow = Convert.ToInt32(nowTime.DayOfWeek);
                weeknow = (weeknow == 0 ? (7 - 1) : weeknow - 1);
                int daydiff = -1 * weeknow;
                DateTime firstDay = nowTime.AddDays(daydiff);
               
                //本周开票数量
                DataTable dtCount = deptBll.GetDataTable(string.Format("select count(1)from XSS_TWOTICKETS t where iscommit=1 and datatype='{0}' and t.CreateTime between to_date('{1}','yyyy-mm-dd') and to_date('{2}','yyyy-mm-dd')", mode, firstDay.ToString("yyyy-MM-dd"), firstDay.AddDays(6).ToString("yyyy-MM-dd")));
                int count1=int.Parse(dtCount.Rows[0][0].ToString());
                //本月开票数量
                dtCount = deptBll.GetDataTable(string.Format("select count(1) from XSS_TWOTICKETS t where iscommit=1 and datatype='{0}' and t.CreateTime between to_date('{1}','yyyy-mm-dd') and to_date('{2}','yyyy-mm-dd')", mode, DateTime.Now.ToString("yyyy-MM-01"), DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd")));
                int count2 = int.Parse(dtCount.Rows[0][0].ToString());
                var JsonData = new
                {
                    rows = data,
                    total = pagination.total,
                    page = pagination.page,
                    records = pagination.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    userdata = new {weekCount=count1,monthCount=count2 }
                };
                return Content(JsonData.ToJson());
            }
            catch(Exception ex)
            {
                return Error(ex.Message);

            }
            
        }
        [HttpGet]
        public ActionResult GetRecordJson(string keyValue)
        {
            try
            {
                var data = threepeoplecheckbll.GetAuditRecord(keyValue);
                return Success("获取数据成功",data);
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = threepeoplecheckbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 生成工作票编号
        /// </summary>
        /// <param name="dataType">票分类</param>
        /// <param name="ticketType">票类型</param>
        /// <returns></returns>
        [HttpGet]
        public string CreateTicketCode(string keyValue, string dataType, string ticketType)
        {
            try
            {
                return threepeoplecheckbll.CreateTicketCode(keyValue,dataType, ticketType);
            }
            catch (Exception ex)
            {

                return "";
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
            threepeoplecheckbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <param name="ApplyUsers">人员信息</param>
        ///<param name="AuditInfo">审核信息</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, TwoTicketsEntity entity)
        {
            try
            {
                threepeoplecheckbll.SaveForm(keyValue, entity);
                return Success("操作成功。");
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
            
        }

        /// <summary>
        /// 审核信息
        /// </summary>
        /// <param name="keyValue">主键值</param>
        ///<param name="record">审核信息</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveRecord(string keyValue, TwoTicketRecordEntity record)
        {
            try
            {
                threepeoplecheckbll.InsertRecord(record);
                return Success("操作成功");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
         
        }
        #endregion

        /// <summary>
        /// 审核信息
        /// </summary>
        /// <param name="keyValue">主键值</param>
        ///<param name="record">审核信息</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Export(string queryJson,int mode = 1)
        {
            try
            {
                List<ColumnEntity> cols = new List<ColumnEntity>();
                string fileName = "工作票";
                Operator user = OperatorProvider.Provider.Current();
                Pagination pagination = new Pagination();
                string[] columnNames = { "流程状态",  "工作票编号", "工作票类别", "任务名称", "部门/班组", "工作票负责人", "工作票签发人", "工作票许可人", "工作票许可时间", "值长/班长", "批准工作时间" };
                string[] fields = { "status", "sno", "tickettype", "content", "deptname", "dutyuser", "senduser", "audituser", "workpermittime", "monitor", "audittime" };
                if (mode==1)
                {
                    pagination.p_fields = "case when status=1 and iscommit=1 then '已开票' when status=1 and iscommit=0 then '已开票(保存未提交)' when status=2 and iscommit=1 then '已延期' when status=2 and iscommit=0 then '已延期(保存未提交)' when status=3 and iscommit=1 then '已消票' when status=3 and iscommit=0 then '已消票(保存未提交)' when status=4 and iscommit=0 then '已作废(保存未提交)' else '已作废' end status,sno,tickettype,to_char(content) content,deptname,dutyuser,senduser,audituser,to_char(workpermittime,'yyyy-mm-dd hh24:mi') workpermittime,monitor,case when audittime is not null then (to_char(audittime,'yyyy-mm-dd hh24:mi') || '至' || to_char(RegisterTime,'yyyy-mm-dd hh24:mi') ) else '' end audittime";
                }
                if (mode == 2)
                {
                    fileName = "操作票";
                    pagination.p_fields = "case when status=1 and iscommit=1 then '已开票' when status=1 and iscommit=0 then '已开票(保存未提交)' when status=2 and iscommit=1 then '已延期' when status=2 and iscommit=0 then '已延期(保存未提交)' when status=3 and iscommit=1 then '已消票' when status=3 and iscommit=0 then '已消票(保存未提交)' when status=4 and iscommit=0 then '已作废(保存未提交)' else '已作废' end status,sno,tickettype,to_char(content) content,deptname,dutyuser,senduser,audituser,case when audittime is not null then (to_char(audittime,'yyyy-mm-dd hh24:mi') || '至' || to_char(registertime,'yyyy-mm-dd hh24:mi'))  else '' end audittime,createusername,to_char(createtime,'yyyy-mm-dd hh24:mi')createtime";
                    columnNames = new string[] { "流程状态", "操作票编号", "操作票类别", "任务名称", "部门/班组", "发令人", "操作人", "监护人", "批准操作时间", "登记人", "登记时间" };
                    fields = new string[] { "status","sno", "tickettype", "content", "deptname", "dutyuser", "senduser", "audituser", "audittime", "createusername", "createtime" };
                }
                if (mode ==3)
                {
                    fileName = "联系票";
                    pagination.p_fields = "case when status=1 and iscommit=1 then '已开票' when status=1 and iscommit=0 then '已开票(保存未提交)' when status=2 and iscommit=1 then '已延期' when status=2 and iscommit=0 then '已延期(保存未提交)' when status=3 and iscommit=1 then '已消票' when status=3 and iscommit=0 then '已消票(保存未提交)' when status=4 and iscommit=0 then '已作废(保存未提交)' else '已作废' end status,sno,tickettype,to_char(content) content,deptname,dutyuser,audituser,to_char(audittime,'yyyy-mm-dd hh24:mi') audittime,tsdsno,createusername,to_char(createtime,'yyyy-mm-dd hh24:mi') registertime";
                    columnNames = new string[] { "流程状态", "联系票编号", "联系票类别", "任务名称", "部门/班组", "联系人", "许可人", "许可时间", "停送电编号", "登记人", "登记时间" };
                    fields = new string[] { "status", "sno", "tickettype", "content", "deptname", "dutyuser", "audituser", "audittime", "tsdsno", "createusername", "registertime" };
                }
                if (mode ==4)
                {
                    fileName = "动火票";
                    pagination.p_fields = "case when status=1 and iscommit=1 then '已开票' when status=1 and iscommit=0 then '已开票(保存未提交)' when status=2 and iscommit=1 then '已延期' when status=2 and iscommit=0 then '已延期(保存未提交)' when status=3 and iscommit=1 then '已消票' when status=3 and iscommit=0 then '已消票(保存未提交)' when status=4 and iscommit=0 then '已作废(保存未提交)' else '已作废' end status,sno,tickettype,to_char(content) content,deptname,dutyuser,audituser,address,to_char(audittime,'yyyy-mm-dd hh24:mi') audittime";
                    columnNames = new string[] { "流程状态","动火票编号", "作业票类别", "任务名称", "部门/班组", "动火负责人", "许可人/值长", "动火地点", "允许动火时间" };
                    fields = new string[] { "status", "sno", "tickettype", "content", "deptname", "dutyuser", "audituser", "address",  "audittime" };
                }
                int j = 0;
                foreach (string name in columnNames)
                {
                    cols.Add(new ColumnEntity {
                        Column = fields[j],
                        ExcelColumn=name,
                        Width = j == fields.Length-1?300:150
                    });
                    j++;
                }
                pagination.page = 1;
                pagination.sidx = "createtime";
                pagination.sord = "desc";
                pagination.rows = 1000000;
                pagination.p_kid = "t.ID";
                pagination.p_tablename = "XSS_TWOTICKETS t";
                pagination.conditionJson = string.Format("datatype={1} and (t.iscommit=1 or (t.iscommit=0 and t.createuserid='{0}')) and CREATEUSERDEPTCODE like '{2}%'", user.UserId, mode, user.OrganizeCode);
                var watch = CommonHelper.TimerStart();
                DataTable data = threepeoplecheckbll.GetPageList(pagination, queryJson);
                data.Columns.Remove("id"); data.Columns.Remove("r");
                BSFramework.Util.Offices.ExcelHelper.ExportByAspose(data, fileName, cols);
                return Success("操作成功");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }

    }
}
