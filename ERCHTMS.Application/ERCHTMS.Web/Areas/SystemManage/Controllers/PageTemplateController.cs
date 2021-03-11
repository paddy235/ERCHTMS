using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Busines.SystemManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.LllegalManage;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Data;

namespace ERCHTMS.Web.Areas.SystemManage.Controllers
{
    /// <summary>
    /// 描 述：模板页面管理表
    /// </summary>
    public class PageTemplateController : MvcControllerBase
    {
        private LllegalRegisterBLL lllegalregisterbll = new LllegalRegisterBLL(); // 违章基本信息
        private PageTemplateBLL pagetemplatebll = new PageTemplateBLL();

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
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ImportForm() 
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
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            Operator opertator = new OperatorProvider().Current();
            pagination.p_tablename = " bis_pagetemplate a ";
            pagination.p_fields = "organizeid,organizename,templatename,filename,templatecode,templatetype,relativepath,modulename,isenable";
            pagination.p_kid = "id";
            pagination.conditionJson = " 1=1";
            var queryParam = queryJson.ToJObject();

            //机构
            if (!string.IsNullOrEmpty(queryParam["organizeid"].ToString()))
            {
                pagination.conditionJson += string.Format(@" and a.organizeid = '{0}'", queryParam["organizeid"].ToString());
            }
            //模块名称
            if (!string.IsNullOrEmpty(queryParam["modulename"].ToString()))
            {
                pagination.conditionJson += string.Format(@" and a.modulename like '%{0}%'", queryParam["modulename"].ToString());
            }
            //模板代码
            if (!string.IsNullOrEmpty(queryParam["templatecode"].ToString()))
            {
                pagination.conditionJson += string.Format(@" and a.templatecode like '%{0}%'", queryParam["templatecode"].ToString());
            }
            //模板名称
            if (!string.IsNullOrEmpty(queryParam["templatename"].ToString()))
            {
                pagination.conditionJson += string.Format(@" and a.templatename like '%{0}%'", queryParam["templatename"].ToString());
            }
            //模板类型
            if (!string.IsNullOrEmpty(queryParam["templatetype"].ToString()))
            {
                pagination.conditionJson += string.Format(@" and a.templatetype like '%{0}%'", queryParam["templatetype"].ToString());
            }
            var data = lllegalregisterbll.GetGeneralQuery(pagination);
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
            var data = pagetemplatebll.GetEntity(keyValue);
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
            pagetemplatebll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, PageTemplateEntity entity)
        {
            if (string.IsNullOrEmpty(entity.TEMPLATETYPE))
            {
                entity.TEMPLATETYPE = "自有模板";
            }
            pagetemplatebll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }


        #region 生成模板文件
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GenerateFile(string keyValue)
        {
            try
            {
                var entity = pagetemplatebll.GetEntity(keyValue);
                if (null != entity)
                {
                    string templateContent = entity.TEMPLATECONTENT;
                    string absolutepath = Server.MapPath(entity.RELATIVEPATH);
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(absolutepath);
                    string fileName = !string.IsNullOrEmpty(entity.FILENAME) ? entity.FILENAME : fileInfo.Name;
                    string filePath = fileInfo.DirectoryName + "\\";

                    if (!string.IsNullOrEmpty(templateContent))
                    {
                        string path = filePath + fileName;
                        if (System.IO.File.Exists(path))
                        {
                            //去掉只读
                            FileAttributes att =  System.IO.File.GetAttributes(path);
                            if ((att & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                            {
                                att = RemoveAttribute(att, FileAttributes.ReadOnly);
                                System.IO.File.SetAttributes(path, att);
                            }
                            System.IO.File.Delete(path); //先删除
                        }
                        // 创建文件
                        FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite); //可以指定盘符，也可以指定任意文件名，还可以为word等文件
                        StreamWriter sw = new StreamWriter(fs,Encoding.UTF8); // 创建写入流
                        sw.WriteLine(templateContent); // 写入内容
                        sw.Close(); //关闭文件
                        fs.Close();
                    }
                }
                return Success("操作成功。");
            }
            catch (System.Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion


        #region 批量生成模板文件
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BatchGenerateFile(string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 100000000;
                Operator opertator = new OperatorProvider().Current();
                pagination.p_tablename = " bis_pagetemplate a ";
                pagination.p_fields = "organizeid,organizename,templatename,filename,templatecode,templatetype,relativepath,modulename,isenable,templatecontent";
                pagination.p_kid = "id";
                pagination.conditionJson = " 1=1";
                var queryParam = queryJson.ToJObject();

                //机构
                if (!string.IsNullOrEmpty(queryParam["organizeid"].ToString()))
                {
                    pagination.conditionJson += string.Format(@" and a.organizeid = '{0}'", queryParam["organizeid"].ToString());
                }
                //模块名称
                if (!string.IsNullOrEmpty(queryParam["modulename"].ToString()))
                {
                    pagination.conditionJson += string.Format(@" and a.modulename like '%{0}%'", queryParam["modulename"].ToString());
                }
                //模板代码
                if (!string.IsNullOrEmpty(queryParam["templatecode"].ToString()))
                {
                    pagination.conditionJson += string.Format(@" and a.templatecode like '%{0}%'", queryParam["templatecode"].ToString());
                }
                //模板名称
                if (!string.IsNullOrEmpty(queryParam["templatename"].ToString()))
                {
                    pagination.conditionJson += string.Format(@" and a.templatename like '%{0}%'", queryParam["templatename"].ToString());
                }
                //模板类型
                if (!string.IsNullOrEmpty(queryParam["templatetype"].ToString()))
                {
                    pagination.conditionJson += string.Format(@" and a.templatetype like '%{0}%'", queryParam["templatetype"].ToString());
                }
                var data = lllegalregisterbll.GetGeneralQuery(pagination);

                foreach (DataRow row in data.Rows) 
                {
                    string templateContent = row["templatecontent"].ToString();
                    string absolutepath = Server.MapPath(row["relativepath"].ToString());
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(absolutepath);
                    string fileName = !string.IsNullOrEmpty(row["filename"].ToString()) ? row["filename"].ToString() : fileInfo.Name;
                    string filePath = fileInfo.DirectoryName + "\\";

                    if (!string.IsNullOrEmpty(templateContent))
                    {
                        string path = filePath + fileName;
                        if (System.IO.File.Exists(path))
                        {
                            //去掉只读
                            FileAttributes att = System.IO.File.GetAttributes(path);
                            if ((att & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                            {
                                att = RemoveAttribute(att, FileAttributes.ReadOnly);
                                System.IO.File.SetAttributes(path, att);
                            }
                            System.IO.File.Delete(path); //先删除
                        }
                        // 创建文件
                        FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite); //可以指定盘符，也可以指定任意文件名，还可以为word等文件
                        StreamWriter sw = new StreamWriter(fs, Encoding.UTF8); // 创建写入流
                        sw.WriteLine(templateContent); // 写入内容
                        sw.Close(); //关闭文件
                        fs.Close();
                    }
                }
                return Success("操作成功。");
            }
            catch (System.Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion

        private static FileAttributes RemoveAttribute(FileAttributes attributes, FileAttributes attributesToRemove) 
        {
            return attributes & ~attributesToRemove;
        }


        #region 复制对象
        /// <summary>
        /// 复制对象
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveCopyForm(string keyValue)
        {
            var entity = pagetemplatebll.GetEntity(keyValue);
            if (null != entity)
            {
                PageTemplateEntity nentity = new PageTemplateEntity();
                nentity = entity;
                nentity.ID = string.Empty;
                nentity.TEMPLATENAME = nentity.TEMPLATENAME + "_复制对象";
                pagetemplatebll.SaveForm("", nentity);
            }
            return Success("操作成功。");
        }
        #endregion

        #region 导出json
        /// <summary>
        /// 导出json
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public ActionResult ExportData(string queryJson) 
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 100000000;
                Operator opertator = new OperatorProvider().Current();
                pagination.p_tablename = " bis_pagetemplate a ";
                pagination.p_fields = "organizeid,organizename,templatename,filename,templatecode,templatetype,relativepath,modulename,isenable,templatecontent";
                pagination.p_kid = "id";
                pagination.conditionJson = " 1=1";
                var queryParam = queryJson.ToJObject();

                //机构
                if (!string.IsNullOrEmpty(queryParam["organizeid"].ToString()))
                {
                    pagination.conditionJson += string.Format(@" and a.organizeid = '{0}'", queryParam["organizeid"].ToString());
                }
                //模块名称
                if (!string.IsNullOrEmpty(queryParam["modulename"].ToString()))
                {
                    pagination.conditionJson += string.Format(@" and a.modulename like '%{0}%'", queryParam["modulename"].ToString());
                }
                //模板代码
                if (!string.IsNullOrEmpty(queryParam["templatecode"].ToString()))
                {
                    pagination.conditionJson += string.Format(@" and a.templatecode like '%{0}%'", queryParam["templatecode"].ToString());
                }
                //模板名称
                if (!string.IsNullOrEmpty(queryParam["templatename"].ToString()))
                {
                    pagination.conditionJson += string.Format(@" and a.templatename like '%{0}%'", queryParam["templatename"].ToString());
                }
                //模板类型
                if (!string.IsNullOrEmpty(queryParam["templatetype"].ToString()))
                {
                    pagination.conditionJson += string.Format(@" and a.templatetype like '%{0}%'", queryParam["templatetype"].ToString());
                }
                var data = lllegalregisterbll.GetGeneralQuery(pagination);
                data.Columns.Remove("r");
                string resultJson = JsonConvert.SerializeObject(data);

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "UTF-8";
                Response.ContentEncoding = Encoding.UTF8;
                string filename = "pagetemplate_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".json";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                Response.Write(resultJson.ToString());
                Response.End();

                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                  return Error(ex.Message);
            }
        }
        #endregion


        #region 导入json
        /// <summary>
        /// 导入json
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public ActionResult ImportData()
        {
            try
            {
                HttpPostedFileBase file = HttpContext.Request.Files[0];
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                string serverpath = Server.MapPath("~/Resource/temp/" + fileName);
                file.SaveAs(serverpath);

                StreamReader sr = new StreamReader(serverpath, Encoding.UTF8);
                string input = sr.ReadToEnd();
                sr.Close();

                JArray jarray = (JArray)JsonConvert.DeserializeObject(input);
                foreach (JObject obj in jarray)
                {
                    string organizeid = obj["organizeid"].ToString();
                    string organizename = obj["organizename"].ToString();
                    string templatename = obj["templatename"].ToString();
                    string filename = obj["filename"].ToString();
                    string templatecode = obj["templatecode"].ToString();
                    string templatetype = obj["templatetype"].ToString();
                    string relativepath = obj["relativepath"].ToString();
                    string modulename = obj["modulename"].ToString();
                    string templatecontent = obj["templatecontent"].ToString();

                    PageTemplateEntity entity = new PageTemplateEntity();
                    entity.ORGANIZEID = organizeid;
                    entity.ORGANIZENAME = organizename;
                    entity.TEMPLATENAME = templatename + "_导入";
                    entity.FILENAME = filename;
                    entity.TEMPLATECODE = templatecode;
                    entity.TEMPLATETYPE = templatetype;
                    entity.RELATIVEPATH = relativepath;
                    entity.MODULENAME = modulename;
                    entity.ISENABLE = "是";
                    entity.TEMPLATECONTENT = templatecontent;
                    pagetemplatebll.SaveForm("", entity);
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
            return Success("操作成功。");
        }
        #endregion


        #endregion
    }
}