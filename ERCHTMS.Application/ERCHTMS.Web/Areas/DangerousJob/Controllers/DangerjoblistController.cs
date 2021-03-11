using ERCHTMS.Entity.DangerousJob;
using ERCHTMS.Busines.DangerousJob;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Aspose.Words;
using BSFramework.Util.Extension;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Entity.SafeReward;

namespace ERCHTMS.Web.Areas.DangerousJob.Controllers
{
    /// <summary>
    /// 描 述：危险作业清单
    /// </summary>
    public class DangerjoblistController : MvcControllerBase
    {
        private DangerjoblistBLL dangerjoblistbll = new DangerjoblistBLL();
        private DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
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
            var data = dangerjoblistbll.GetList(queryJson);
            return ToJsonResult(data);
        }

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

                var data = dangerjoblistbll.GetPageList(pagination, queryJson);
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
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = dangerjoblistbll.GetEntity(keyValue);
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
            dangerjoblistbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, DangerjoblistEntity entity)
        {
            dangerjoblistbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }

        /// <summary>
        /// 导出安全奖励详细
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult ExportDangerJobList(string queryJson)
        {
            try
            {
                HttpResponse resp = System.Web.HttpContext.Current.Response;
                var queryParam = queryJson.ToJObject();
                string fileName = "危险作业清单_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
                string strDocPath = Request.PhysicalApplicationPath + @"Resource\ExcelTemplate\危险作业清单.docx";
                Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);
                DocumentBuilder builder = new DocumentBuilder(doc);
                DataSet dts = new DataSet();
                DataTable dt = new DataTable("A");
                dt.Columns.Add("deptType");
                dt.Columns.Add("deptName");
                dt.Columns.Add("userName");
                dt.Columns.Add("year");
                dt.Columns.Add("month");
                dt.Columns.Add("day");

                DataRow row = dt.NewRow();
                Operator user = OperatorProvider.Provider.Current();


                //树节点code
                var spdepart = dataItemDetailBLL.GetDataItemListByItemCode("spdepart").Select(p => p.ItemValue).FirstOrDefault();
                var deptCode = !queryParam["code"].IsEmpty() ? queryParam["code"].ToString() : user.DeptCode;
                var dept = new DepartmentBLL().GetEntityByCode(deptCode);
                if (dept != null)
                {
                    if (dept.EnCode == user.OrganizeCode || spdepart.Contains(deptCode))
                    {
                        row["deptType"] = "公司级";
                    }
                    else
                    {
                        switch (dept.Nature)
                        {
                            case "部门":
                                row["deptType"] = "部门级";
                                break;
                            case "专业":
                                row["deptType"] = "工序级";
                                break;
                        }
                    }

                }


                row["deptName"] = user.DeptName;
                row["userName"] = user.UserName;
                DateTime dateTime = DateTime.Now;
                row["year"] = dateTime.Year;
                row["month"] = dateTime.Month;
                row["day"] = dateTime.Day;
                dt.Rows.Add(row);
                dts.Tables.Add(dt);


                string pageIndex = queryParam["pageIndex"].ToString();
                string pageSize = queryParam["pageSize"].ToString();
                Pagination pagination = new Pagination();
                pagination.page = int.Parse(pageIndex);
                pagination.rows = int.Parse(pageSize);
                pagination.conditionJson = "1=1";
                DataTable listperson = dangerjoblistbll.GetPageList(pagination, queryJson); ;
                DataTable dtperson = new DataTable("U");
                dtperson.Columns.Add("num");
                dtperson.Columns.Add("dangerjobname");
                dtperson.Columns.Add("numberofpeople");
                dtperson.Columns.Add("deptnames");
                dtperson.Columns.Add("dangerfactors");
                dtperson.Columns.Add("accidentcategories");
                dtperson.Columns.Add("jobfrequency");
                dtperson.Columns.Add("safetymeasures");
                dtperson.Columns.Add("joblevel");
                dtperson.Columns.Add("principalnames");

                int num = 1;
                foreach (DataRow item in listperson.Rows)
                {
                    DataRow dtrow = dtperson.NewRow();
                    dtrow["num"] = num;
                    dtrow["dangerjobname"] = item["dangerjobname"];
                    dtrow["numberofpeople"] = item["numberofpeoplename"].ToString();
                    dtrow["deptnames"] = item["deptnames"];
                    dtrow["dangerfactors"] = item["dangerfactors"];
                    dtrow["accidentcategories"] = item["accidentcategories"];
                    dtrow["jobfrequency"] = item["jobfrequency"];
                    dtrow["safetymeasures"] = item["safetymeasures"];
                    dtrow["joblevel"] = item["joblevelname"];
                    dtrow["principalnames"] = item["principalnames"];
                    dtperson.Rows.Add(dtrow);
                    num++;
                }
                dts.Tables.Add(dtperson);
                doc.MailMerge.ExecuteWithRegions(dts);
                doc.MailMerge.DeleteFields();
                doc.Save(resp, Server.UrlEncode(fileName), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
                return Success("导出成功!");
            }
            catch (Exception e)
            {
                return Success(e.Message);
                throw;
            }
            
        }
        #endregion


    }


}
