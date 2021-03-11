using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Busines.HighRiskWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Web;
using System;
using BSFramework.Util.Offices;
using System.Data;
using System.Collections;
using System.Collections.Generic;

namespace ERCHTMS.Web.Areas.HighRiskWork.Controllers
{
    /// <summary>
    /// 描 述：监督任务检查项目
    /// </summary>
    public class SideCheckProjectController : MvcControllerBase
    {
        private SideCheckProjectBLL sidecheckprojectbll = new SideCheckProjectBLL();

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
        /// 模板导入页面
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
        public ActionResult GetListJson(string queryJson)
        {
            var data = sidecheckprojectbll.GetList(queryJson);
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
            var data = sidecheckprojectbll.GetEntity(keyValue);
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
            sidecheckprojectbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SideCheckProjectEntity entity)
        {
            sidecheckprojectbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion

        #region 导入监督任务检查项目
        /// <summary>
        /// 导入监督任务检查项目
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
                List<SideCheckProjectEntity> listEntity = new List<SideCheckProjectEntity>();
                SideCheckProjectEntity chapter = new SideCheckProjectEntity();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var chartnumber = dt.Rows[i][0].ToString();
                    if (!string.IsNullOrEmpty(chartnumber))
                    {
                        if (!chartnumber.Contains("."))
                        {
                            chapter = new SideCheckProjectEntity();
                            chapter.CheckContent = dt.Rows[i][1].ToString();
                            chapter.CheckNumber = chartnumber.ToString();
                            chapter.ParentId = "-1";
                            chapter.Id = Guid.NewGuid().ToString();
                            hast.Add(chapter.CheckNumber, chapter.Id);
                           
                        }
                        else
                        {
                            chapter = new SideCheckProjectEntity();
                            if (hast.Keys.Count > 0)
                            {
                                string m = dt.Rows[i][0].ToString();
                                string[] b = new string[20];
                                string key = "";
                                if (m.Contains("."))
                                {
                                    b = m.Split('.');
                                    if (b.Length > 1)
                                    {
                                        key = b[0];
                                    }
                                }
                                object obj = hast[key];
                                if (obj == null)
                                {
                                    chapter.ParentId = "-1";
                                }
                                else
                                {
                                    chapter.ParentId = obj.ToString();
                                }
                                chapter.CheckContent = dt.Rows[i][1].ToString();
                                chapter.CheckNumber = chartnumber.ToString();
                                chapter.Id = Guid.NewGuid().ToString();
                            }
                        }
                        listEntity.Add(chapter);
                    }
                }
                for (int j = 0; j < listEntity.Count; j++)
                {
                    try
                    {
                        sidecheckprojectbll.SaveForm("", listEntity[j]);
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
