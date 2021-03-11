using ERCHTMS.Entity.SafetyLawManage;
using ERCHTMS.Busines.SafetyLawManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System;
using ERCHTMS.Code;
using System.Data;
using BSFramework.Util.Offices;
using System.Collections.Generic;

namespace ERCHTMS.Web.Areas.SafetyLawManage.Controllers
{
    /// <summary>
    /// 描 述：收藏法律法规
    /// </summary>
    public class StoreLawController : MvcControllerBase
    {
        private StoreLawBLL storelawbll = new StoreLawBLL();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = storelawbll.GetList(queryJson);
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
            var data = storelawbll.GetEntity(keyValue);
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
            if (!string.IsNullOrEmpty(keyValue))
            {
                if (keyValue.Contains(","))
                {
                    string[] array = keyValue.TrimEnd(',').Split(',');
                    for (int i = 0; i < array.Length; i++)
                    {
                        storelawbll.RemoveForm(array[i]);
                    }
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
        public ActionResult SaveForm(string keyValue, StoreLawEntity entity)
        {
            storelawbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion


        /// <summary>
        /// 根据条件获取数据
        /// </summary>
        [HttpPost]
        [AjaxOnly]
        public ActionResult storeSafetyLaw(string idsData, string ctype)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!string.IsNullOrEmpty(idsData))
            {
                if (idsData.Contains(","))
                {
                    string[] array = idsData.TrimEnd(',').Split(',');
                    for (int i = 0; i < array.Length; i++)
                    {
                        int result = storelawbll.GetStoreBylawId(array[i].ToString());
                        if (result == 0)
                        {
                            StoreLawEntity entity = new StoreLawEntity();
                            entity.UserId = user.UserId;
                            entity.LawId = array[i].ToString();
                            entity.cType = ctype;
                            entity.StoreTime = DateTime.Now;
                            storelawbll.SaveForm("", entity);
                        }
                    }
                }
                return Success("收藏成功。");
            }
            return Error("无数据收藏。");
        }


        #region  我的收藏
        #region 安全生产法律法规
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
            pagination.p_kid = "storeid";
            pagination.p_fields = "a.lawid,b.CreateDate,FileName,LawArea,IssueDept,FileCode,ValidVersions,CarryDate,FilesId,effetstate,updatedate,channeltype";
            pagination.p_tablename = " bis_storelaw a left join bis_safetylaw b on a.lawid=b.id";
            pagination.conditionJson = "userid='" + user.UserId + "' and ctype='1'";
            var data = storelawbll.GetPageDataTable(pagination, queryJson);
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
        public ActionResult ExportDataLaw(string queryJson)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.p_kid = "storeid";
                pagination.p_fields = @"FileName,FileCode,to_char(b.CreateDate,'yyyy-MM-dd') as CreateDate,to_char(CarryDate,'yyyy-MM-dd') as CarryDate,IssueDept,case when Effetstate='1' then '现行有效'
                                            when Effetstate ='2' then '即将实施'
                                            when Effetstate='3'  then '已修订'
                                            when Effetstate='4'  then '废止' end Effetstate";
                pagination.p_tablename = " bis_storelaw a left join bis_safetylaw b on a.lawid=b.id";
                pagination.conditionJson = "userid='" + user.UserId + "' and ctype='1'";
                DataTable exportTable = storelawbll.GetPageDataTable(pagination, queryJson);
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "安全生产法律法规信息";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "安全生产法律法规信息导出.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filename", ExcelColumn = "标题名称", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filecode", ExcelColumn = "文号/标准号", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createdate", ExcelColumn = "发布时间", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "carrydate", ExcelColumn = "实施日期", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "issuedept", ExcelColumn = "发布机关", Width = 30 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "effetstate", ExcelColumn = "时效性", Width = 15 });
                //调用导出方法
                ExcelHelper.ExcelDownload(exportTable, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("导出成功。");
        }
        #endregion


        #region 安全管理制度
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageJsonInstitution(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_kid = "storeid as id";
            pagination.p_fields = "c.lawid,t.CreateDate,t.FileName,t.IssueDept,t.FileCode,t.ValidVersions,t.CarryDate,t.FilesId,t.releasedate,t.revisedate,t.lawtypename";
            pagination.p_tablename = " bis_storelaw c left join (select id as lid,CreateDate,FileName,IssueDept,FileCode,ValidVersions,CarryDate,FilesId,releasedate,revisedate,lawtypename,LawTypeCode,createuserorgcode from  bis_safeinstitution a ) t on c.lawid=t.lid";
            pagination.conditionJson = string.Format("userid='{0}' and ctype='2'", user.UserId);
            var data = storelawbll.GetPageJsonInstitution(pagination, queryJson);
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


        #region 导出
        /// <summary>
        /// 导出
        /// </summary>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult ExportDataInstitution(string queryJson)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.p_kid = "storeid";
                pagination.p_fields = "FileName,FileCode,IssueDept,to_char(releasedate,'yyyy-MM-dd') as releasedate,to_char(revisedate,'yyyy-MM-dd') as revisedate,to_char(carrydate,'yyyy-MM-dd') as carrydate,lawtypename";
                pagination.p_tablename = " bis_storelaw c left join (select id as lid,CreateDate,FileName,IssueDept,FileCode,ValidVersions,CarryDate,FilesId,releasedate,revisedate,lawtypename,LawTypeCode,createuserorgcode from  bis_safeinstitution a ) t on c.lawid=t.lid";
                pagination.conditionJson = string.Format("userid='{0}' and ctype='2'", user.UserId);
                DataTable exportTable = storelawbll.GetPageJsonInstitution(pagination, queryJson);
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "安全规章制度信息";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "安全规章制度信息导出.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filename", ExcelColumn = "文件名称", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filecode", ExcelColumn = "文件编号", Width = 25 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "issuedept", ExcelColumn = "发布单位(部门)", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "releasedate", ExcelColumn = "发布时间", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "revisedate", ExcelColumn = "修订时间", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "carrydate", ExcelColumn = "实施时间", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "lawtypename", ExcelColumn = "类型", Width = 20 });
                //调用导出方法
                ExcelHelper.ExcelDownload(exportTable, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("导出成功。");
        }
        #endregion
        #endregion


        #region 安全操作规程
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageJsonStandards(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_kid = "storeid as id";
            pagination.p_fields = "c.lawid,t.CreateDate,t.FileName,t.IssueDept,t.FileCode,t.ValidVersions,t.CarryDate,t.FilesId,t.releasedate,t.revisedate,t.lawtypename";
            pagination.p_tablename = " bis_storelaw c left join (select id as lid,CreateDate,FileName,IssueDept,FileCode,ValidVersions,CarryDate,FilesId,releasedate,revisedate,lawtypename,LawTypeCode,createuserorgcode from  bis_safestandards a ) t on c.lawid=t.lid";
            pagination.conditionJson = string.Format("userid='{0}' and ctype='3'", user.UserId);
            var data = storelawbll.GetPageJsonStandards(pagination, queryJson);
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
        public ActionResult ExportDataStandards(string queryJson)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.p_kid = "storeid";
                pagination.p_fields = "FileName,FileCode,IssueDept,to_char(releasedate,'yyyy-MM-dd') as releasedate,to_char(revisedate,'yyyy-MM-dd') as revisedate,to_char(carrydate,'yyyy-MM-dd') as carrydate,lawtypename";
                pagination.p_tablename = " bis_storelaw c left join (select id as lid,CreateDate,FileName,IssueDept,FileCode,ValidVersions,CarryDate,FilesId,releasedate,revisedate,lawtypename,LawTypeCode,createuserorgcode from  bis_safestandards a ) t on c.lawid=t.lid";
                pagination.conditionJson = string.Format("userid='{0}' and ctype='3'", user.UserId);
                DataTable exportTable = storelawbll.GetPageJsonStandards(pagination, queryJson);
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "安全操作规程信息";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "安全操作规程导出.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filename", ExcelColumn = "文件名称", Width = 40 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filecode", ExcelColumn = "文件编号", Width = 25 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "issuedept", ExcelColumn = "发布单位(部门)", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "releasedate", ExcelColumn = "发布时间", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "revisedate", ExcelColumn = "修订时间", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "carrydate", ExcelColumn = "实施时间", Width = 10 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "lawtypename", ExcelColumn = "类型", Width = 20 });
                //调用导出方法
                ExcelHelper.ExcelDownload(exportTable, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("导出成功。");
        }
        #endregion

        #region 事故案例
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJsonCase(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_kid = "storeid";
            pagination.p_fields = "a.lawid,b.CreateDate,FileName,AccRange,AccTime,Remark,FilesId,AccidentCompany,RelatedCompany,AccidentGrade,intDeaths,AccType";
            pagination.p_tablename = " bis_storelaw a left join bis_accidentCaseLaw b on a.lawid=b.id";
            pagination.conditionJson = "userid='" + user.UserId + "' and ctype='6'";
            var data = storelawbll.GetPageDataTable(pagination, queryJson);
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
        public ActionResult ExportDataCase(string queryJson)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.p_kid = "storeid";
                pagination.p_fields = @"FileName,RelatedCompany,AccTime,case when AccidentGrade='1' then '一般事故'
                                            when AccidentGrade ='2' then '较大事故'
                                            when AccidentGrade='3'  then '重大事故'
                                            when AccidentGrade='4'  then '特别重大事故' end AccidentGrade,intDeaths,AccType,
                                        case when AccRange='1' then '本单位事故'
                                            when AccRange ='2' then '本集团事故'
                                            when AccRange='3'  then '电力系统内容事故' end AccRange
                                    ,Remark,FilesId,AccidentCompany";
                pagination.p_tablename = " bis_storelaw a left join bis_accidentCaseLaw b on a.lawid=b.id";
                pagination.conditionJson = "userid='" + user.UserId + "' and ctype='6'";
                DataTable exportTable = storelawbll.GetPageDataTable(pagination, queryJson);
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "事故案例信息";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "事故案例信息导出.xls";
                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "filename", ExcelColumn = "事故名称", Width = 50 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "relatedcompany", ExcelColumn = "涉事单位", Width = 50 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "acctime", ExcelColumn = "事故时间", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "accidentgrade", ExcelColumn = "事故等级", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "intdeaths", ExcelColumn = "死亡人数", Width = 30 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "acctype", ExcelColumn = "事故类别", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "accrange", ExcelColumn = "数据范围", Width = 15 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "remark", ExcelColumn = "备注", Width = 15 });
                //调用导出方法
                ExcelHelper.ExcelDownload(exportTable, excelconfig);
            }
            catch (Exception ex)
            {

            }
            return Success("导出成功。");
        }
        #endregion
        #endregion
    }
}
