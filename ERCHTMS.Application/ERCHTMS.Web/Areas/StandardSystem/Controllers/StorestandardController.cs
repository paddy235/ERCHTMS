using ERCHTMS.Entity.StandardSystem;
using ERCHTMS.Busines.StandardSystem;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System.Data;
using BSFramework.Util.Extension;

namespace ERCHTMS.Web.Areas.StandardSystem.Controllers
{
    /// <summary>
    /// 描 述：收藏标准
    /// </summary>
    public class StorestandardController : MvcControllerBase
    {

        private StorestandardBLL storestandardbll = new StorestandardBLL();
        private StandardsystemBLL standardsystembll = new StandardsystemBLL();

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
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "c.storeid";
            pagination.p_fields = "a.id,filename,b.name as categorycode,relevantelementname,relevantelementid,carrydate,a.createdate,consultnum,d.fullname as createuserdeptname";
            pagination.p_tablename = "hrs_storestandard c left join hrs_standardsystem a on c.standardid = a.id left join hrs_stcategory b on a.categorycode=b.id left join base_department d on a.createuserdeptcode = d.encode";
            pagination.conditionJson = "c.userid='" + user.UserId + "'";
            var data = standardsystembll.GetPageList(pagination, queryJson);
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
            var data = storestandardbll.GetList(queryJson);
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
            var data = storestandardbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 导出
        /// </summary>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult ExportData(string queryJson)
        {
            try
            {
                var queryParam = queryJson.ToJObject();
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.p_kid = "c.storeid";
                if (queryParam["standardtype"].ToString() == "1" || queryParam["standardtype"].ToString() == "2" || queryParam["standardtype"].ToString() == "3" || queryParam["standardtype"].ToString() == "4" || queryParam["standardtype"].ToString() == "5" || queryParam["standardtype"].ToString() == "6")
                {
                    pagination.p_fields = "filename,b.name as categorycode,relevantelementname,to_char(carrydate,'yyyy-MM-dd') as carrydate,to_char(a.createdate,'yyyy-MM-dd') as createdate,consultnum";
                }
                else
                {
                    pagination.p_fields = "filename,to_char(a.createdate,'yyyy-MM-dd') as createdate,d.fullname as createuserdeptname,consultnum";
                }
                pagination.p_tablename = "hrs_storestandard c left join hrs_standardsystem a on c.standardid = a.id left join hrs_stcategory b on a.categorycode=b.id left join base_department d on a.createuserdeptcode = d.encode";
                pagination.conditionJson = "c.userid='" + user.UserId + "'";
                pagination.sidx = "a.createdate";
                pagination.sord = "desc";
                DataTable exportTable = standardsystembll.GetPageList(pagination, queryJson);
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                if (!queryParam["standardtype"].IsEmpty())
                {
                    switch (queryParam["standardtype"].ToString())
                    {
                        case "1":
                            excelconfig.Title = "技术标准体系";
                            excelconfig.FileName = "技术标准体系信息导出.xls";
                            break;
                        case "2":
                            excelconfig.Title = "管理标准体系";
                            excelconfig.FileName = "管理标准体系信息导出.xls";
                            break;
                        case "3":
                            excelconfig.Title = "岗位标准体系";
                            excelconfig.FileName = "岗位标准体系信息导出";
                            break;
                        case "4":
                            excelconfig.Title = "上级标准化文件";
                            excelconfig.FileName = "上级标准化文件信息导出.xls";
                            break;
                        case "5":
                            excelconfig.Title = "指导标准";
                            excelconfig.FileName = "指导标准信息导出.xls";
                            break;
                        case "6":
                            excelconfig.Title = "法律法规";
                            excelconfig.FileName = "法律法规信息导出.xls";
                            break;
                        case "7":
                            excelconfig.Title = "标准体系策划与构建";
                            excelconfig.FileName = "标准体系策划与构建信息导出.xls";
                            break;
                        case "8":
                            excelconfig.Title = "标准体系评价与改进";
                            excelconfig.FileName = "标准体系评价与改进信息导出.xls";
                            break;
                        case "9":
                            excelconfig.Title = "标准化培训";
                            excelconfig.FileName = "标准化培训信息导出.xls";
                            break;
                        default:
                            break;
                    }
                }
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filename", ExcelColumn = "文件名称", Width = 300 });
                if (queryParam["standardtype"].ToString() == "1" || queryParam["standardtype"].ToString() == "2" || queryParam["standardtype"].ToString() == "3" || queryParam["standardtype"].ToString() == "4" || queryParam["standardtype"].ToString() == "5" || queryParam["standardtype"].ToString() == "6")
                {
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "categorycode", ExcelColumn = "类别", Width = 300 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "relevantelementname", ExcelColumn = "对应元素", Width = 300 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "carrydate", ExcelColumn = "施行日期", Width = 300 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createdate", ExcelColumn = "发布日期", Width = 300 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "consultnum", ExcelColumn = "查阅频次", Width = 300 });
                }
                else
                {
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createdate", ExcelColumn = "发布日期", Width = 300 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createuserdeptname", ExcelColumn = "发布单位/部门", Width = 300 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "consultnum", ExcelColumn = "查阅频次", Width = 300 });
                }
                //调用导出方法
                //ExcelHelper.ExcelDownload(exportTable, excelconfig);
                ExcelHelper.ExportByAspose(exportTable, excelconfig.FileName, excelconfig.ColumnEntity);

            }
            catch (Exception ex)
            {

            }
            return Success("导出成功。");
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="idsData">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string idsData)
        {
            if (!string.IsNullOrEmpty(idsData))
            {
                if (idsData.Contains(","))
                {
                    string[] array = idsData.TrimEnd(',').Split(',');
                    for (int i = 0; i < array.Length; i++)
                    {
                        storestandardbll.RemoveForm(array[i]);
                    }
                }
                else
                {
                    storestandardbll.RemoveForm(idsData);
                }
            }
            return Success("取消收藏成功。");
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
        public ActionResult SaveForm(string keyValue, StorestandardEntity entity)
        {
            storestandardbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="idsData">标准主键</param>
        /// <param name="standardType">标准类型</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult StoreStandard(string idsData, string standardType)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!string.IsNullOrEmpty(idsData))
            {
                if (idsData.Contains(","))
                {
                    string[] array = idsData.TrimEnd(',').Split(',');
                    for (int i = 0; i < array.Length; i++)
                    {
                        int result = storestandardbll.GetStoreByStandardID(array[i].ToString());
                        if (result == 0)
                        {
                            StorestandardEntity entity = new StorestandardEntity();
                            entity.USERID = user.UserId;
                            entity.STANDARDID = array[i].ToString();
                            entity.STANDARDTYPE = standardType;
                            entity.STORETIME = DateTime.Now;
                            storestandardbll.SaveForm("", entity);
                        }
                    }
                }
                else
                {
                    int result = storestandardbll.GetStoreByStandardID(idsData);
                    if (result == 0)
                    {
                        StorestandardEntity entity = new StorestandardEntity();
                        entity.USERID = user.UserId;
                        entity.STANDARDID = idsData;
                        entity.STANDARDTYPE = standardType;
                        entity.STORETIME = DateTime.Now;
                        storestandardbll.SaveForm("", entity);
                    }
                }
                return Success("收藏成功。");
            }
            return Error("无数据收藏。");
        }
        #endregion
    }
}
