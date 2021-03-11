using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Busines.HighRiskWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using System.Data;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Web.Areas.HighRiskWork.Controllers
{
    /// <summary>
    /// 描 述：旁站监督人员(高风险作业)
    /// </summary>
    public class SidePersonController : MvcControllerBase
    {
        private SidePersonBLL sidepersonbll = new SidePersonBLL();

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
        /// 选择监督员界面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Select()
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
        public ActionResult GetPageTableJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "a.Id as sideid";
            pagination.p_fields = "SideUserDeptId,SideUserId,a.CreateDate,SideUserName,SideUserSex,SideUserIdCard,SideUserDeptName,itemname as  SideUserLevel,a.createuserid,a.createuserdeptcode,a.createuserorgcode";
            pagination.p_tablename = " bis_sideperson a left join base_dataitemdetail b on a.SideUserLevel=b.itemvalue and itemid =(select itemid from base_dataitem where itemcode='SideLevel')";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                if (!string.IsNullOrEmpty(authType))
                {
                    switch (authType)
                    {
                        case "1":
                            pagination.conditionJson += " and a.createuserid='" + user.UserId + "'";
                            break;
                        case "2":
                            pagination.conditionJson += " and a.createuserdeptcode='" + user.DeptCode + "'";
                            break;
                        case "3":
                            pagination.conditionJson += " and a.createuserdeptcode like'" + user.DeptCode + "%'";
                            break;
                        case "4":
                            pagination.conditionJson += " and a.createuserorgcode='" + user.OrganizeCode + "'";
                            break;
                    }
                }
                else
                {
                    pagination.conditionJson += " and 0=1";
                }
            }
            var data = sidepersonbll.GetPageDataTable(pagination, queryJson);
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
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetSelectPersonJson(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_kid = "a.Id as sideid";
            pagination.p_fields = "SideUserDeptId,SideUserId,a.CreateDate,SideUserName,SideUserSex,SideUserIdCard,SideUserDeptName,itemname as  SideUserLevel,a.createuserid,a.createuserdeptcode,a.createuserorgcode";
            pagination.p_tablename = " bis_sideperson a left join base_dataitemdetail b on a.SideUserLevel=b.itemvalue and itemid =(select itemid from base_dataitem where itemcode='SideLevel')";
            pagination.conditionJson = string.Format("a.createuserorgcode='{0}'", user.OrganizeCode);
            var data = sidepersonbll.GetPageDataTable(pagination, queryJson);
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
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = sidepersonbll.GetList(queryJson);
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
            var data = sidepersonbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 是否存在监督人员
        /// </summary>
        /// <param name="userid"></param>
        [HttpGet]
        public bool ExistSideUser(string userid)
        {
            var data = sidepersonbll.ExistSideUser(userid);
            return data;
        }

        /// <summary>
        ///所有监督人员
        /// </summary>
        /// <param name="userid"></param>
        [HttpGet]
        public string AllSidePerson()
        {
            string siduserids = "";
            var list = sidepersonbll.GetList("");
            foreach (SidePersonEntity item in list)
            {
                siduserids += item.SideUserId + ",";
            }
            if (!string.IsNullOrEmpty(siduserids))
            {
                siduserids = siduserids.TrimEnd(',');
            }
            return siduserids;
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
            sidepersonbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SidePersonEntity entity)
        {
            sidepersonbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion


        #region
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
                pagination.p_kid = "Id";
                pagination.p_fields = "SideUserName,SideUserSex,SideUserIdCard,SideUserDeptName,itemname as SideUserLevel";
                pagination.p_tablename = " bis_sideperson a left join base_dataitemdetail b on a.SideUserLevel=b.itemvalue and itemid =(select itemid from base_dataitem where itemcode='SideLevel')";
                pagination.conditionJson = "1=1";
                pagination.sidx = "a.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (!user.IsSystem)
                {
                    string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                    if (!string.IsNullOrEmpty(authType))
                    {
                        switch (authType)
                        {
                            case "1":
                                pagination.conditionJson += " and a.createuserid='" + user.UserId + "'";
                                break;
                            case "2":
                                pagination.conditionJson += " and a.createuserdeptcode='" + user.DeptCode + "'";
                                break;
                            case "3":
                                pagination.conditionJson += " and a.createuserdeptcode like'" + user.DeptCode + "%'";
                                break;
                            case "4":
                                pagination.conditionJson += " and a.createuserorgcode='" + user.OrganizeCode + "'";
                                break;
                        }
                    }
                    else
                    {
                        pagination.conditionJson += " and 0=1";
                    }
                }
                DataTable exportTable = sidepersonbll.GetPageDataTable(pagination, queryJson);
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "旁站监督人员信息";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "旁站监督人员信息导出.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "sideusername", ExcelColumn = "姓名", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "sideusersex", ExcelColumn = "性别", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "sideuseridcard", ExcelColumn = "身份证号", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "sideuserdeptname", ExcelColumn = "单位/部门", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "sideuserlevel", ExcelColumn = "旁站监督级别", Width = 10 });
                //调用导出方法
                ExcelHelper.ExcelDownload(exportTable, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("导出成功。");
        }
        #endregion
    }
}
