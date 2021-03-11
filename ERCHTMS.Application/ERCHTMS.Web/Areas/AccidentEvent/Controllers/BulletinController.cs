using ERCHTMS.Entity.AccidentEvent;
using ERCHTMS.Busines.AccidentEvent;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using System.Linq.Expressions;
using System.Linq;
using System.IO;
using Aspose.Words.Saving;
using ERCHTMS.Busines.BaseManage;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Busines.Desktop;

namespace ERCHTMS.Web.Areas.AccidentEvent.Controllers
{
    /// <summary>
    /// 描 述：事故事件快报
    /// </summary>
    public class BulletinController : MvcControllerBase
    {
        private BulletinBLL bulletinbll = new BulletinBLL();

        #region 视图功能
        /// <returns></returns>
        [HttpGet]
        public ActionResult PdBZ()
        {
            return View();
        }


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
        /// 通用版表单页面
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
            pagination.p_fields = "IsSubmit,CREATEUSERID,SGNAME, SGTYPENAME,HAPPENTIME,AREANAME,SGKBUSERNAME,RSSHSGTYPENAME,CREATEUSERDEPTCODE as departmentcode,CREATEUSERORGCODE as  organizecode";
            pagination.p_tablename = "V_AEM_BULLETIN_Order t";
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
            pagination.p_fields = "IsSubmit,CREATEUSERID,SGNAME, SGTYPENAME,HAPPENTIME,AREANAME,SGKBUSERNAME,RSSHSGTYPENAME,CREATEUSERDEPTCODE as departmentcode,CREATEUSERORGCODE as  organizecode";
            pagination.p_tablename = "V_AEM_BULLETIN_Order t";
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
            var data = bulletinbll.GetList(queryJson);
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
            var data = bulletinbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region 提交数据

        [HandlerMonitor(0, "即时单下载")]
        public void Down(string keyValue)
        {
            DesktopBLL dbll=new DesktopBLL();
            bool flag = dbll.IsGeneric();
            //查询数据
            var entity = bulletinbll.GetEntity(keyValue);
            DepartmentBLL orgbll = new DepartmentBLL();
            var org = orgbll.GetEntity(ERCHTMS.Code.OperatorProvider.Provider.Current().OrganizeId);
            //数据合并
            string[] text = new string[] { "SGHSJLX", "ORGNAME", "ORGADDRESS", "ORGTEL", "HAPPENTIME", 
                "AREANAME", 
                "SGTYPENAME",
                "SGLEVELNAME", "JYJG", "SWNUM", "SZNUM", "ZSNUM", "SHQKSHJE", "TDQK", "CBYY", "HFQK", "DEPARTMENTNAME", "SGKBUSERNAME", "MOBILE" };
            string[] value;
            var tempPath = Server.MapPath("~/Resource/Temp/电力事故(事件)即时报告单.doc");
            var outputPath = Server.MapPath("~/Resource/ExcelTemplate/电力事故(事件)即时报告单.doc");
            //是否是通用行业版
            if (flag)
            {
                value = new string[] { 
                    (entity.SGTYPENAME.Contains("事故") ? "事故报告 √　　          事件报告 □" : " 事故报告 □　　          事件报告 √"),
                    org.FullName,
                    "",
                    org.OuterPhone,
                    entity.HAPPENTIME.Value.ToString(),
                    entity.AREANAME,
                    entity.RSSHSGTYPENAME,
                    entity.SGLEVELNAME,
                    entity.JYJG,
                    entity.SWNUM.ToString(),
                    entity.SZNUM.ToString(),
                    entity.ZSNUM.ToString(),
                    entity.SHQKSHJE.ToString(),
                    entity.TDQK,
                    entity.CBYY,
                    entity.HFQK,
                    entity.DEPARTMENTNAME,
                    entity.SGKBUSERNAME,
                    entity.MOBILE
                };
                tempPath = Server.MapPath("~/Resource/Temp/事故(事件)即时报告单.doc");
                outputPath = Server.MapPath("~/Resource/ExcelTemplate/事故(事件)即时报告单.doc");
            }
            else
            {
                value = new string[] { 
                    (entity.SGTYPENAME.Contains("事故") ? "事故报告 √　　          事件报告 □" : " 事故报告 □　　          事件报告 √"),
                    org.FullName,
                    "",
                    org.OuterPhone,
                    entity.HAPPENTIME.Value.ToString(),
                    entity.AREANAME,
                    entity.SGTYPENAME,
                    entity.SGLEVELNAME,
                    entity.JYJG,
                    entity.SWNUM.ToString(),
                    entity.SZNUM.ToString(),
                    entity.ZSNUM.ToString(),
                    entity.SHQKSHJE.ToString(),
                    entity.TDQK,
                    entity.CBYY,
                    entity.HFQK,
                    entity.DEPARTMENTNAME,
                    entity.SGKBUSERNAME,
                    entity.MOBILE
                };
            }

           

           
            Aspose.Words.Document doc = new Aspose.Words.Document(tempPath);
            //数据合并
            doc.MailMerge.Execute(text, value);
            doc.Save(outputPath);
            //调用导出方法
            var docStream = new MemoryStream();
            doc.Save(docStream, SaveOptions.CreateSaveOptions(Aspose.Words.SaveFormat.Doc));
            Response.ContentType = "application/msword";
            if (flag)
            {
                Response.AddHeader("content-disposition", "attachment;filename=事故(事件)即时报告单.doc");
            }
            else
            {
                Response.AddHeader("content-disposition", "attachment;filename=电力事故(事件)即时报告单.doc");
            }

            Response.BinaryWrite(docStream.ToArray());
            Response.End();
        }


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
            Bulletin_dealBLL bulletin_dealbll = new Bulletin_dealBLL();

            foreach (var item in keyValue.Split(','))
            {
                bulletinbll.RemoveForm(item);
                Expression<Func<Bulletin_dealEntity, bool>> condition = e => e.BULLETINID == item;
                var list = bulletin_dealbll.GetListForCon(condition);
                if (list != null && list.Count() > 0)
                {
                    var keyvaluedeal = list.FirstOrDefault().ID;
                    bulletin_dealbll.RemoveForm(keyvaluedeal);
                }
            }
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
        public ActionResult SaveForm(string keyValue, BulletinEntity entity)
        {
            string HAPPENTIME = Request["HAPPENTIME"] ?? "";
            if (HAPPENTIME.Length > 0)
                entity.HAPPENTIME = DateTime.Parse(HAPPENTIME);
            bulletinbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion

        #region 数据导出
        /// <summary>
        /// 事故事件快报
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "事故事件快报")]
        public ActionResult ExportBulletinList(string condition, string queryJson)
        {
            Pagination pagination = new Pagination();
            queryJson = queryJson ?? "";
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "ID";
            pagination.p_fields = "SGNAME, SGTYPENAME,HAPPENTIME,AREANAME,SGKBUSERNAME,case WHEN  IsSubmit>0 then '已提交' else '未提交' end  as IsSubmitStr";
            pagination.p_tablename = "V_AEM_BULLETIN_Order t";
            pagination.sord = "IsSubmitStr";
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
            excelconfig.Title = "事故事件快报";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "事故事件快报.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "SGNAME".ToLower(), ExcelColumn = "事故/事件名称" });
            listColumnEntity.Add(new ColumnEntity() { Column = "SGTYPENAME".ToLower(), ExcelColumn = "事故或事件类型" });
            listColumnEntity.Add(new ColumnEntity() { Column = "HAPPENTIME".ToLower(), ExcelColumn = "发生时间", Alignment = "Center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "AREANAME".ToLower(), ExcelColumn = "地点（区域）" });
            listColumnEntity.Add(new ColumnEntity() { Column = "SGKBUSERNAME".ToLower(), ExcelColumn = "快报人" });
            listColumnEntity.Add(new ColumnEntity() { Column = "IsSubmitStr".ToLower(), ExcelColumn = "是否提交" });
            excelconfig.ColumnEntity = listColumnEntity;

            //调用导出方法
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("导出成功。");
        }
        #endregion
    }
}
