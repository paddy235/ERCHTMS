using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.Busines.EmergencyPlatform;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Data;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Code;
using System.Linq;
using ERCHTMS.Entity.BaseManage;
using Aspose.Words;
using System.Web;
using ERCHTMS.Busines.OutsourcingProject;
using System;

namespace ERCHTMS.Web.Areas.EmergencyPlatform.Controllers
{
    /// <summary>
    /// 描 述：应急物资领用申请
    /// </summary>
    public class SuppliesacceptController : MvcControllerBase
    {
        private SuppliesacceptBLL suppliesacceptbll = new SuppliesacceptBLL();
        

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
        /// 流程图
        /// </summary>
        /// <returns></returns>
        public ActionResult Flow()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPageList(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.conditionJson = " 1=1 ";

                var data = suppliesacceptbll.GetPageList(pagination, queryJson);
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
                return Error(ex.ToString());
            }
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = suppliesacceptbll.GetList(queryJson);
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
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var data = suppliesacceptbll.GetEntity(keyValue);
                if (data.Status == 1)
                {
                    ManyPowerCheckBLL bll = new ManyPowerCheckBLL();
                    ManyPowerCheckEntity power= bll.GetListByModuleNo(user.OrganizeCode, "YJWZLYSP").OrderByDescending(t => t.SERIALNUM).FirstOrDefault();
                    ManyPowerCheckEntity flow = bll.GetEntity(data.FlowId);
                    if (power != null && power.SERIALNUM==flow.SERIALNUM)
                    {
                        data.IsLastAudit = true;
                    }
                    else
                    {
                        data.IsLastAudit = false;
                    }
                }
                else
                {
                    data.IsLastAudit = false;
                }
                return ToJsonResult(data);
            }
            catch (System.Exception ex)
            {
                return Error(ex.ToString());
            }
            
        }

        /// <summary>
        /// 获取流程图
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetFlow(string keyValue)
        {
            try
            {
                var data = suppliesacceptbll.GetFlow(keyValue, "应急物资领用审批");
                return ToJsonResult(data);
            }
            catch (System.Exception ex)
            {
                return Error(ex.ToString());
            }
        }
        
        /// <summary>
        /// 导出审批单
        /// </summary>
        /// <param name="keyValue"></param>
        public void ExportData(string keyValue)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            //报告对象
            string fileName = Server.MapPath("~/Resource/ExcelTemplate/应急物资领用单模板.docx");
            Aspose.Words.Document doc = new Aspose.Words.Document(fileName);
            DocumentBuilder builder = new DocumentBuilder(doc);
            DataTable dt = new DataTable();
            dt.Columns.Add("applydept"); //申请部门
            dt.Columns.Add("applyperson"); //申请人 
            dt.Columns.Add("applytime"); //申请时间 
            dt.Columns.Add("reason"); //申请原因
            dt.Columns.Add("idea1");  //部门负责人意见
            dt.Columns.Add("person1"); //部门负责人
            dt.Columns.Add("time1"); //部门负责人审批时间
            dt.Columns.Add("idea2"); //安环部负责人意见
            dt.Columns.Add("person2"); //安环部负责人
            dt.Columns.Add("time2"); //安环部负责人审批时间
            dt.Columns.Add("idea3");//物料部负责人意见
            dt.Columns.Add("person3");//物料部负责人
            dt.Columns.Add("time3"); //物料部负责人审批时间
            dt.Columns.Add("idea4"); //分管领导意见        
            dt.Columns.Add("person4"); //分管领导签名
            dt.Columns.Add("time4"); //分管领导时间
            dt.Columns.Add("idea5"); //应急物资出库人审批意见
            dt.Columns.Add("person5"); //应急物资出库人
            dt.Columns.Add("time5"); //应急物资出库人审批时间
            DataRow row = dt.NewRow();

            SuppliesacceptEntity entity = suppliesacceptbll.GetEntity(keyValue);
            row["applydept"] = entity.ApplyDept;
            row["applyperson"] = entity.ApplyPerson;
            row["applytime"] = entity.ApplyDate.Value.ToString("yyyy年MM月dd日HH时mm分");
            row["reason"] = entity.AcceptReason;


            //物资信息
            DataTable dt1 = new DataTable("A");
            dt1.Columns.Add("num");
            dt1.Columns.Add("suppliesname");
            dt1.Columns.Add("models");
            dt1.Columns.Add("acceptnum");

            SuppliesAcceptDetailBLL suppliesacceptdetailbll = new SuppliesAcceptDetailBLL();

            var list = suppliesacceptdetailbll.GetList("").Where(t => t.RecId == keyValue).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                DataRow row1 = dt1.NewRow();
                row1["num"] = i + 1;
                row1["suppliesname"] = list[i].SuppliesName;
                row1["models"] = list[i].Models;
                row1["acceptnum"] = list[i].AcceptNum;
                dt1.Rows.Add(row1);
            }
            doc.MailMerge.ExecuteWithRegions(dt1);


            //审核记录
            AptitudeinvestigateauditBLL aptitudeinvestigateauditbll = new AptitudeinvestigateauditBLL();
            ManyPowerCheckBLL manypowercheckbll = new ManyPowerCheckBLL();
            var data = aptitudeinvestigateauditbll.GetAuditRecList(keyValue);
            for (int i = 0; i < data.Rows.Count; i++)
            {
                try
                {
                    var power = manypowercheckbll.GetEntity(data.Rows[i]["flowid"].ToString());
                    row["idea" + power.SERIALNUM] = data.Rows[i]["auditopinion"].ToString();
                    if (string.IsNullOrWhiteSpace(data.Rows[i]["auditsignimg"].ToString()))
                    {
                        row["person" + power.SERIALNUM] = Server.MapPath("~/content/Images/no_1.png");
                    }
                    else
                    {
                        var filepath = Server.MapPath("~/") + data.Rows[i]["auditsignimg"].ToString().Replace("../../", "").ToString();
                        if (System.IO.File.Exists(filepath))
                        {
                            row["person" + power.SERIALNUM] = filepath;
                        }
                        else
                        {
                            row["person" + power.SERIALNUM] = Server.MapPath("~/content/Images/no_1.png");
                        }
                    }
                    row["time" + power.SERIALNUM] = Convert.ToDateTime(data.Rows[i]["audittime"].ToString()).ToString("yyyy年MM月dd日");
                }
                catch (System.Exception ex)
                {
                }
            }
            dt.Rows.Add(row);
            doc.MailMerge.Execute(dt);
            doc.MailMerge.DeleteFields();
            doc.Save(resp, Server.UrlEncode("应急物资领用单" + DateTime.Now.ToString("yyyyMMdd") + ".docx"), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Docx));
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
            suppliesacceptbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SuppliesacceptEntity entity)
        {
            try
            {
                string message = "";
                message= suppliesacceptbll.SaveForm(keyValue, entity);
                if (!string.IsNullOrWhiteSpace(message))
                {
                    return Error(message);
                }
                else
                {
                    return Success("操作成功。");
                }
                
            }
            catch (System.Exception ex)
            {
                return Error(ex.ToString());
            }
        }

        /// <summary>
        /// 审核表单
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="aentity"></param>
        /// <param name="DetailData"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AuditForm(string keyValue, AptitudeinvestigateauditEntity aentity,string DetailData)
        {
            try
            {
                string message = "";
                message = suppliesacceptbll.AuditForm(keyValue, aentity, DetailData);
                if (!string.IsNullOrWhiteSpace(message))
                {
                    return Error(message);
                }
                else
                {
                    return Success("审核成功。");
                }
            }
            catch (System.Exception ex)
            {
                return Error(ex.ToString());
            }
        }
        #endregion
    }
}
