using ERCHTMS.Entity.EngineeringManage;
using ERCHTMS.Busines.EngineeringManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Data;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Code;
using BSFramework.Data;
using ERCHTMS.Busines.PublicInfoManage;

namespace ERCHTMS.Web.Areas.EngineeringManage.Controllers
{
    /// <summary>
    /// 描 述：危大工程管理
    /// </summary>
    public class PerilEngineeringController : MvcControllerBase
    {
        private PerilEngineeringBLL perilengineeringbll = new PerilEngineeringBLL();
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();

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
        /// 文件页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Files()
        {
            return View();
        }


        /// <summary>
        /// 工程进展情况统计列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CaseList()
        {
            return View();
        }

        /// <summary>
        /// 省级公司表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SIndex()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SJIndexList()
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
            pagination.p_kid = "Id";
            pagination.p_fields = "CreateDate,EvolveCase,TaskFiles,ConstructFiles,EFinishTime,EStartTime,UnitType,BelongDeptName,EngineeringType,EngineeringName,createuserid,createuserdeptcode,createuserorgcode";
            pagination.p_tablename = " bis_perilengineering";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
            if (!string.IsNullOrEmpty(authType))
            {
                switch (authType)
                {
                    case "1":
                        pagination.conditionJson += " createuserid='" + user.UserId + "'";
                        break;
                    case "2":
                        pagination.conditionJson += string.Format(" belongdeptid  in (select departmentid from base_department where encode='{0}' union select ORGANIZEID from BASE_ORGANIZE where encode='{0}')", user.OrganizeCode);
                        break;
                    case "3":
                        pagination.conditionJson += string.Format(" belongdeptid  in (select departmentid from base_department where encode like '{0}%' union select ORGANIZEID from BASE_ORGANIZE where encode like '{0}%')", user.DeptCode);
                        break;
                    case "4":
                        pagination.conditionJson +=string.Format("  belongdeptid  in (select departmentid from base_department where encode like '{0}%' union select ORGANIZEID from BASE_ORGANIZE where encode like '{0}%')",user.OrganizeCode);
                        break;
                }
            }
            else
            {
                pagination.conditionJson += " 0=1";
            }
            var data = perilengineeringbll.GetPageList(pagination, queryJson);
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
        /// 获取省级列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJsonForSJ(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "a.Id";
            pagination.p_fields = "a.CreateDate,a.EvolveCase,a.TaskFiles,a.ConstructFiles,a.EFinishTime,a.EStartTime,a.UnitType,a.BelongDeptName,a.EngineeringType,a.EngineeringName,a.createuserid,a.createuserdeptcode,a.createuserorgcode,b.fullname";
            pagination.p_tablename = " bis_perilengineering a left join base_department b on a.CREATEUSERORGCODE = b.encode";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            //string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
            //if (!string.IsNullOrEmpty(authType))
            //{
            //    switch (authType)
            //    {
            //        case "1":
            //            pagination.conditionJson += " createuserid='" + user.UserId + "'";
            //            break;
            //        case "2":
            //            pagination.conditionJson += string.Format(" belongdeptid  in (select departmentid from base_department where encode='{0}' union select ORGANIZEID from BASE_ORGANIZE where encode='{0}')", user.OrganizeCode);
            //            break;
            //        case "3":
            //            pagination.conditionJson += string.Format(" belongdeptid  in (select departmentid from base_department where encode like '{0}%' union select ORGANIZEID from BASE_ORGANIZE where encode like '{0}%')", user.DeptCode);
            //            break;
            //        case "4":
            //            pagination.conditionJson += string.Format("  belongdeptid  in (select departmentid from base_department where encode like '{0}%' union select ORGANIZEID from BASE_ORGANIZE where encode like '{0}%')", user.OrganizeCode);
            //            break;
            //    }
            //}
            //else
            //{
            //    pagination.conditionJson += " 0=1";
            //}
            pagination.conditionJson += string.Format("  a.belongdeptid  in (select departmentid from base_department where deptcode like '{0}%' union select ORGANIZEID from BASE_ORGANIZE where encode like '{0}%')", user.OrganizeCode);
            var data = perilengineeringbll.GetPageList(pagination, queryJson);
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
        /// 导出
        /// </summary>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult ExportData(string queryJson)
        {
            try
            {
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.p_kid = "a.Id";
                pagination.p_fields = "EngineeringName,ProgrammeCategory as EngineeringType,BelongDeptName,case when UnitType = 1 then '单位内部' else '外包单位' end as UnitType,EStartTime,EFinishTime,ConstructFiles,TaskFiles,EvolveCase";
                pagination.p_tablename = " bis_perilengineering a left join bis_engineeringsetting b on a.EngineeringType = b.id";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                if (!string.IsNullOrEmpty(authType))
                {
                    switch (authType)
                    {
                        case "1":
                            pagination.conditionJson += " a.createuserid='" + user.UserId + "'";
                            break;
                        case "2":
                            pagination.conditionJson += string.Format(" belongdeptid  in (select departmentid from base_department where encode='{0}' union select ORGANIZEID from BASE_ORGANIZE where encode='{0}')", user.OrganizeCode);
                            break;
                        case "3":
                            pagination.conditionJson += string.Format(" belongdeptid  in (select departmentid from base_department where encode like '{0}%' union select ORGANIZEID from BASE_ORGANIZE where encode like '{0}%')", user.DeptCode);
                            break;
                        case "4":
                            pagination.conditionJson += string.Format("  belongdeptid  in (select departmentid from base_department where encode like '{0}%' union select ORGANIZEID from BASE_ORGANIZE where encode like '{0}%')", user.OrganizeCode);
                            break;
                    }
                }
                else
                {
                    pagination.conditionJson += " 0=1";
                }
                DataTable exportTable = perilengineeringbll.GetPageList(pagination, queryJson);
                foreach (DataRow item in exportTable.Rows)
                {
                      var dt= fileInfoBLL.GetFiles(item["ConstructFiles"].ToString());
                      if (dt.Rows.Count > 0)
                          item["ConstructFiles"] = "有";
                      else
                          item["ConstructFiles"] = "无";
                      var dt1 = fileInfoBLL.GetFiles(item["TaskFiles"].ToString());
                      if (dt1.Rows.Count > 0)
                          item["TaskFiles"] = "有";
                      else
                          item["TaskFiles"] = "无";
                }
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "危大工程信息";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "危大工程信息导出.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "engineeringname", ExcelColumn = "工程名称", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "engineeringtype", ExcelColumn = "工程类别", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "belongdeptname", ExcelColumn = "所属单位", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "unittype", ExcelColumn = "单位类别", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "estarttime", ExcelColumn = "开始时间", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "efinishtime", ExcelColumn = "结束时间", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "constructfiles", ExcelColumn = "施工方案", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "taskfiles", ExcelColumn = "安全技术交底", Width = 12 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "evolvecase", ExcelColumn = "进展情况", Width = 15 });
                //调用导出方法
                ExcelHelper.ExcelDownload(exportTable, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("导出成功。");
        }


        /// <summary>
        /// 导出省级数据
        /// </summary>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult ExportDataForSJ(string queryJson)
        {
            try
            {
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.p_kid = "a.Id";
                pagination.p_fields = "EngineeringName,ProgrammeCategory as EngineeringType,BelongDeptName,case when UnitType = 1 then '单位内部' else '外包单位' end as UnitType,EStartTime,EFinishTime,ConstructFiles,TaskFiles,EvolveCase,c.fullname";
                pagination.p_tablename = " bis_perilengineering a left join bis_engineeringsetting b on a.EngineeringType = b.id left join base_department c on a.createuserorgcode = c.encode";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                pagination.conditionJson += string.Format("  a.belongdeptid  in (select departmentid from base_department where deptcode like '{0}%' union select ORGANIZEID from BASE_ORGANIZE where encode like '{0}%')", user.OrganizeCode);
                DataTable exportTable = perilengineeringbll.GetPageList(pagination, queryJson);
                foreach (DataRow item in exportTable.Rows)
                {
                    var dt = fileInfoBLL.GetFiles(item["ConstructFiles"].ToString());
                    if (dt.Rows.Count > 0)
                        item["ConstructFiles"] = "有";
                    else
                        item["ConstructFiles"] = "无";
                    var dt1 = fileInfoBLL.GetFiles(item["TaskFiles"].ToString());
                    if (dt1.Rows.Count > 0)
                        item["TaskFiles"] = "有";
                    else
                        item["TaskFiles"] = "无";
                }
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "危大工程信息";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "危大工程信息导出.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "engineeringname", ExcelColumn = "工程名称", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "engineeringtype", ExcelColumn = "工程类别", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "belongdeptname", ExcelColumn = "所属单位", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "unittype", ExcelColumn = "单位类别", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "estarttime", ExcelColumn = "开始时间", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "efinishtime", ExcelColumn = "结束时间", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "constructfiles", ExcelColumn = "施工方案", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "taskfiles", ExcelColumn = "安全技术交底", Width = 12 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "evolvecase", ExcelColumn = "进展情况", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "fullname", ExcelColumn = "所属电厂", Width = 15 });
                //调用导出方法
                ExcelHelper.ExcelDownload(exportTable, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("导出成功。");
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = perilengineeringbll.GetList(queryJson);
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
            var data = perilengineeringbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }


        /// <summary>
        /// 根据条件获取数据
        /// </summary>
        [HttpGet]
        public ActionResult GetPeril(string code, string st, string et, string keyword)
        {
            var data = perilengineeringbll.GetPeril(code, st, et, keyword);
            return ToJsonResult(data);

        }

        /// <summary>
        /// 省级根据条件获取数据 可与GetPeril 合并
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPerilForSJIndex(string queryJson)
        {
            var data = perilengineeringbll.GetPerilForSJIndex(queryJson);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 工程类别（绑定控件）
        /// </summary>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetEngineeringTypeJson()
        {
            var data = perilengineeringbll.GetEngineeringType();
            return Content(data.ToJson());
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
            perilengineeringbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, PerilEngineeringEntity entity)
        {
            perilengineeringbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
