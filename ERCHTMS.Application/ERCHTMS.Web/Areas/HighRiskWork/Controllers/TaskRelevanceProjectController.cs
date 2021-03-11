using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Busines.HighRiskWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using System.Data;
using ERCHTMS.Busines.HiddenTroubleManage;
using System.Linq;
using ERCHTMS.Busines.LllegalManage;
using System.Web;
using System;
using Aspose.Words;
using Aspose.Words.Saving;
using ERCHTMS.Busines.SystemManage;
using System.IO;

namespace ERCHTMS.Web.Areas.HighRiskWork.Controllers
{
    /// <summary>
    /// 描 述：已检查的检查项目
    /// </summary>
    public class TaskRelevanceProjectController : MvcControllerBase
    {
        private TaskRelevanceProjectBLL taskrelevanceprojectbll = new TaskRelevanceProjectBLL();
        private HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL(); //隐患基本信息
        private LllegalRegisterBLL lllegalregisterbll = new LllegalRegisterBLL(); // 违章基本信息
        private DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
        private TaskSignBLL tasksignbll = new TaskSignBLL();//监督签到信息

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
            var data = taskrelevanceprojectbll.GetList(queryJson);
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
            var data = taskrelevanceprojectbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }


        /// <summary>
        /// 获取检查项目数据
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            var superviseid = queryParam["superviseid"];
            pagination.p_kid = "a.Id as checkprojectid";
            pagination.p_fields = "checkcontent,parentid,remark,IsCorrespond,CFiles,checknumber as hidcount,a.createusername as breakcount";
            pagination.p_tablename = string.Format("bis_SideCheckProject a left join bis_taskrelevanceproject b  on a.id=b.checkprojectid and superviseid='{0}'", superviseid);
            pagination.conditionJson = "1=1";
            var data = taskrelevanceprojectbll.GetPageDataTable(pagination);
            foreach (DataRow item in data.Rows)
            {
                item["hidcount"] = htbaseinfobll.GetList(string.Format(" and RelevanceId='{0}' and  relevanceType ='{1}'", superviseid, item["checkprojectid"].ToString())).ToList().Count.ToString();
                item["breakcount"] = lllegalregisterbll.GetList(string.Format(" and Reseverone='{0}' and  Resevertwo='{1}'", superviseid, item["checkprojectid"].ToString())).ToList().Count.ToString();
            }
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
        /// 根据监督任务获取所有的隐患问题
        /// </summary>
        /// <param name="superviseid">监督任务id</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetTaskHiddenInfo(string superviseid)
        {
            var data = taskrelevanceprojectbll.GetTaskHiddenInfo(superviseid);
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
            taskrelevanceprojectbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, TaskRelevanceProjectEntity entity)
        {
            taskrelevanceprojectbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion


        /// <summary>
        /// 旁站监督详情导出
        /// </summary>
        [HandlerMonitor(0, "旁站监督")]
        public void ExportDetails(string keyValue)
        {
            //SuperviseTaskEntity se = supervisetaskbll.GetEntity(keyValue);

            //string fileName = Server.MapPath("~/Resource/ExcelTemplate/旁站监督记录.doc");

            ////监督签到记录
            //DataTable dt = new DataTable("U", "U");
            //dt.Columns.Add("jx");
            //dt.Columns.Add("jt");
            //dt.Columns.Add("jz");
            //var signlist = tasksignbll.GetTaskSignInfo(keyValue);
            //int count = 1;
            //foreach (TaskSignEntity item in signlist)
            //{
            //    DataRow dr = dt.NewRow();
            //    dr["jx"] = count.ToString();
            //    dr["jt"] = Convert.ToDateTime(item.SuperviseTime).ToString("yyyy-MM-dd HH:ss");
            //    dr["jz"] = item.SuperviseState;
            //    dt.Rows.Add(dr);
            //    count++;
            //}
            //if (signlist.Count() <= 0)
            //{
            //    DataRow dr1 = dt.NewRow();
            //    dr1["jx"] = "";
            //    dr1["jt"] = "";
            //    dr1["jz"] = "";
            //    dt.Rows.Add(dr1);
            //}

            ////检查记录
            //DataTable dtcontent = new DataTable("Record");
            //dtcontent.Columns.Add("AS");
            //dtcontent.Columns.Add("BS");
            //dtcontent.Columns.Add("CS");
            //dtcontent.Columns.Add("DS");
            //dtcontent.Columns.Add("ES");
            //dtcontent.Columns.Add("FS");
            //Pagination pagination = new Pagination();
            //pagination.page = 1;
            //pagination.rows = 1000;
            //pagination.p_kid = "a.Id as checkprojectid";
            //pagination.p_fields = "checkcontent,parentid,remark,IsCorrespond,checknumber as hidcount,a.createusername as breakcount";
            //pagination.p_tablename = string.Format("bis_SideCheckProject a left join bis_taskrelevanceproject b  on a.id=b.checkprojectid and superviseid='{0}'", keyValue);
            //pagination.conditionJson = "1=1";
            //pagination.sidx = "checknumber";
            //pagination.sord = "asc";
            //var data = taskrelevanceprojectbll.GetPageDataTable(pagination);
            //int num = 1;
            //foreach (DataRow item in data.Rows)
            //{
            //    DataRow dr = dtcontent.NewRow();
            //    if (item["parentid"].ToString() != "-1")
            //    {
            //        dr["AS"] = num.ToString();
            //        dr["BS"] = item["checkcontent"].ToString();
            //        if (item["IsCorrespond"].ToString() == "1")
            //            dr["CS"] = "是";
            //        else if (item["IsCorrespond"].ToString() == "2")
            //            dr["CS"] = "否";
            //        else if (item["IsCorrespond"].ToString() == "3")
            //            dr["CS"] = "无此项";
            //        else
            //            dr["CS"] = "";
            //        dr["DS"] = item["remark"].ToString();
            //        dr["ES"] = htbaseinfobll.GetList(string.Format(" and RelevanceId='{0}' and  relevanceType ='{1}'", keyValue, item["checkprojectid"].ToString())).ToList().Count.ToString();//隐患数量
            //        dr["FS"] = lllegalregisterbll.GetList(string.Format(" and Reseverone='{0}' and  Resevertwo='{1}'", keyValue, item["checkprojectid"].ToString())).ToList().Count.ToString();//违章数量
            //        num++;
            //    }
            //    else
            //    {
            //        dr["BS"] = item["checkcontent"].ToString();
            //    }
            //    dtcontent.Rows.Add(dr);
            //}
            //if (data.Rows.Count <= 0)
            //{
            //    DataRow dr1 = dtcontent.NewRow();
            //    dr1["AS"] = "";
            //    dr1["BS"] = "";
            //    dr1["CS"] = "";
            //    dr1["DS"] = "";
            //    dr1["ES"] = "";
            //    dr1["FS"] = "";//违章数量
            //    dtcontent.Rows.Add(dr1);
            //}

            ////“星级”安全旁站监督记录表
            //var datainfo = taskrelevanceprojectbll.GetTaskHiddenInfo(keyValue);
            //string describe = "", measure = "";
            //if (datainfo.Rows.Count > 0)
            //{
            //    //现场主要问题
            //    describe = datainfo.Rows[0]["hiddescribe"].ToString() + datainfo.Rows[0]["lllegaldescribe"].ToString();
            //    //问题修正措施
            //    measure = datainfo.Rows[0]["changemeasure"].ToString() + datainfo.Rows[0]["reformmeasure"].ToString();
            //}
            //string times = "";
            //var dataTimes = tasksignbll.GetTaskSignInfo(keyValue);
            //int amount = 1;
            //foreach (TaskSignEntity signentity in dataTimes)
            //{
            //    if (times != "")
            //    {
            //        if ((amount + 1) % 2 == 0)
            //        {
            //            times = times + "-" + Convert.ToDateTime(signentity.SuperviseTime).ToString("yyyy-MM-dd HH:mm");
            //        }
            //        else
            //        {
            //            times = times + "；" + Convert.ToDateTime(signentity.SuperviseTime).ToString("yyyy-MM-dd HH:mm");
            //        }

            //    }
            //    else
            //    {
            //        times = Convert.ToDateTime(signentity.SuperviseTime).ToString("yyyy-MM-dd HH:mm");
            //    }
            //    amount++;
            //}
            //Document doc = new Document(fileName);
            //doc.MailMerge.ExecuteWithRegions(dt);
            //doc.MailMerge.ExecuteWithRegions(dtcontent);
            //doc.MailMerge.Execute(new string[] { "AN", "BN", "CN", "DN", "EN", "FN", "AR", "BR", "CR", "GN", "HN", "IN", "JN", "KN", "LN", "MN", "NN" }, new object[] { dataItemDetailBLL.GetEntity(se.CheckType).ItemName, se.TaskWorkContent, se.TaskApplyDeptName, Convert.ToDateTime(se.TaskWorkStartTime).ToString("yyyy-MM-dd HH:ss") + " - " + Convert.ToDateTime(se.TaskWorkEndTime).ToString("yyyy-MM-dd HH:ss"), se.TaskWorkPlace, se.TaskBill, se.TaskUserName, se.TaskLevel, se.TaskDeptName, se.TimeLong, se.RiskAnalyse, se.OrganizeManager, se.ConstructLayout, se.SafetyMeasure, describe, measure, times });
            ////string name = DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
            ////string reportName = Server.MapPath(string.Format("~/Resource/ExcelTemplate/SelfReport/{0}", name));
            //// doc.Save(reportName, SaveOptions.CreateSaveOptions(SaveFormat.Doc));
            //var docStream = new MemoryStream();
            //doc.Save(docStream, SaveOptions.CreateSaveOptions(Aspose.Words.SaveFormat.Doc));
            //Response.ContentType = "application/msword";
            //Response.AddHeader("content-disposition", "attachment;filename=旁站监督记录.doc");
            //Response.BinaryWrite(docStream.ToArray());
            //Response.End();
        }
    }
}
