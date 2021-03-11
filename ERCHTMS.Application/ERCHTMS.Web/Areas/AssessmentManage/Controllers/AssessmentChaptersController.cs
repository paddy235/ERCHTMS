using ERCHTMS.Entity.AssessmentManage;
using ERCHTMS.Busines.AssessmentManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Web;
using System;
using BSFramework.Util.Offices;
using System.Data;
using System.Collections;
using System.Collections.Generic;

namespace ERCHTMS.Web.Areas.AssessmentManage.Controllers
{
    /// <summary>
    /// 描 述：自评标准
    /// </summary>
    public class AssessmentChaptersController : MvcControllerBase
    {
        private AssessmentChaptersBLL assessmentchaptersbll = new AssessmentChaptersBLL();
        private AssessmentStandardBLL assessmentstandardbll = new AssessmentStandardBLL();

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
        /// 详情界面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }

        /// <summary>
        /// 导入页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Import()
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
        public ActionResult GetListDuty(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "Id as sumid";
            pagination.p_fields = "'' as DutyID,concat(MajorNumber,ChaptersName) as SumName,Reserve as DutyName";
            pagination.p_tablename = "bis_assessmentchapters";
            pagination.conditionJson = "ChaptersParentID='-1'";
            pagination.sidx = "cast(replace(majornumber,'.','') as number)";
            pagination.sord = "asc";
            var data = assessmentchaptersbll.GetPageList(pagination, queryJson);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = assessmentchaptersbll.GetList(queryJson);
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
            pagination.p_kid = "Id";
            pagination.p_fields = "CreateDate,MajorNumber,ChaptersName,Content,ReviewWay,Score,createuserid,createuserdeptcode,createuserorgcode";
            pagination.p_tablename = "bis_assessmentchapters";
            pagination.conditionJson = "1=1";
            var data = assessmentchaptersbll.GetPageList(pagination, queryJson);
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
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = assessmentchaptersbll.GetEntity(keyValue);
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
            assessmentchaptersbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, AssessmentChaptersEntity entity)
        {
            assessmentchaptersbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion


        #region 导入自评标准
        /// <summary>
        /// 导入自评标准
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportCase()
        {
            int error = 0;
            string message = "请选择格式正确的文件再导入!";
            string falseMessage = "";
            int count = HttpContext.Request.Files.Count;
            if (count > 0)
            {
                HttpPostedFileBase file = HttpContext.Request.Files[0];
                if (string.IsNullOrEmpty(file.FileName))
                {
                    return message;
                }
                if (!(file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xls") || file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xlsx")))
                {
                    return message;
                }
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName));
                DataTable dt = ExcelHelper.ExcelImport(Server.MapPath("~/Resource/temp/" + fileName));
                Hashtable hast = new Hashtable();
                List<AssessmentChaptersEntity> listEntity = new List<AssessmentChaptersEntity>();
                AssessmentChaptersEntity chapter = new AssessmentChaptersEntity();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i][0].ToString()))
                    {
                        chapter = new AssessmentChaptersEntity();
                        chapter.MajorNumber = dt.Rows[i][0].ToString();
                        chapter.Id = Guid.NewGuid().ToString();
                        chapter.ChaptersName = dt.Rows[i][1].ToString();
                        if (hast.Keys.Count > 0)
                        {
                            string m = dt.Rows[i][0].ToString();
                            string[] b = new string[20];
                            string key = "";
                            if (m.Contains("."))
                            {
                                b = m.Split('.');
                                if (b.Length > 2)
                                {
                                    key = b[0] + "." + b[1];
                                }
                            }
                            object obj = hast[key];
                            if (obj == null)
                            {
                                chapter.ChaptersParentID = "-1";
                            }
                            else
                            {
                                chapter.ChaptersParentID = obj.ToString();
                                if (!string.IsNullOrEmpty(dt.Rows[i][4].ToString()))
                                {
                                    //评分标准
                                    AssessmentStandardEntity aentity = new AssessmentStandardEntity();
                                    aentity.AChapters = chapter.Id;
                                    aentity.Content = dt.Rows[i][4].ToString();
                                    assessmentstandardbll.SaveForm("", aentity);
                                }
                                if (!string.IsNullOrEmpty(dt.Rows[i][5].ToString()))
                                {
                                    chapter.ReviewWay = dt.Rows[i][5].ToString();
                                }
                            }
                        }
                        else
                        {
                            chapter.ChaptersParentID = "-1";
                        }
                        chapter.Content = dt.Rows[i][2].ToString();
                        chapter.Score = Convert.ToInt32(dt.Rows[i][3].ToString());
                        hast.Add(chapter.MajorNumber, chapter.Id);
                        listEntity.Add(chapter);
                    }
                    else
                    {
                        chapter.ChaptersName += dt.Rows[i][1].ToString();
                        chapter.Content += dt.Rows[i][2].ToString();
                        if (!string.IsNullOrEmpty(dt.Rows[i][4].ToString()))
                        {
                            //评分标准
                            AssessmentStandardEntity aentity = new AssessmentStandardEntity();
                            aentity.AChapters = chapter.Id;
                            aentity.Content = dt.Rows[i][4].ToString();
                            assessmentstandardbll.SaveForm("", aentity);
                        }
                    }
                }
                for (int j = 0; j < listEntity.Count; j++)
                {
                    try
                    {
                        assessmentchaptersbll.SaveForm("", listEntity[j]);
                    }
                    catch (Exception ex)
                    {
                        error++;
                    }

                }
                count = listEntity.Count;
                message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条";
                message += "</br>" + falseMessage;
            }
            return message;
        }
        #endregion
    }
}
