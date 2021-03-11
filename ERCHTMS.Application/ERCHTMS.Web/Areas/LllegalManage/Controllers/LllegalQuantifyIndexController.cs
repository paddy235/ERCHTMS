using ERCHTMS.Entity.LllegalManage;
using ERCHTMS.Busines.LllegalManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using BSFramework.Util.Extension;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System;
using Aspose.Cells;
using System.Data;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.HiddenTroubleManage.ViewModel;
using ERCHTMS.Cache;
using System.Drawing;
using BSFramework.Util.Offices;

namespace ERCHTMS.Web.Areas.LllegalManage.Controllers
{
    /// <summary>
    /// 描 述：违章量化指标表
    /// </summary>
    public class LllegalQuantifyIndexController : MvcControllerBase
    {
        private LllegalQuantifyIndexBLL lllegalquantifyindexbll = new LllegalQuantifyIndexBLL();
        private LllegalRegisterBLL lllegalregisterbll = new LllegalRegisterBLL();
        private UserBLL userbll = new UserBLL(); //用户操作对象
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private PostCache postCache = new PostCache();

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
                /// 导入页面
                /// </summary>
                /// <returns></returns>
        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }

        /// <summary>
                /// 统计页面
                /// </summary>
                /// <returns></returns>
        [HttpGet]
        public ActionResult StIndex()
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
            var data = lllegalquantifyindexbll.GetList(queryJson);
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
            var data = lllegalquantifyindexbll.GetEntity(keyValue);

            return ToJsonResult(data);
        }


        #region 列表查询
        /// <summary>
        /// 列表查询
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var queryParam = queryJson.ToJObject();
                pagination.p_kid = "id";
                pagination.p_fields = @"deptid,deptname,dutyid,dutyname,indexvalue,yearvalue,monthvalue,createusername,createdate,createuserid";
                pagination.p_tablename = @" ( select a.* ,b.encode ,b.sortcode,b.deptcode from bis_lllegalquantifyindex a left join base_department b on a.deptid = b.departmentid ) a";
                pagination.conditionJson = string.Format(" 1=1 ");

                if (!queryParam["DutyId"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and dutyid='{0}' ", queryParam["DutyId"].ToString());
                }
                if (!queryParam["DeptId"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and deptid ='{0}' ", queryParam["DeptId"].ToString());
                }
                if (!queryParam["YearValue"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and yearvalue ='{0}' ", queryParam["YearValue"].ToString());
                }
                var watch = CommonHelper.TimerStart();
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
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        } 
        #endregion


        #region 违章指标统计列表查询
        /// <summary>
        /// 违章指标统计列表查询
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetStatPageListJson(Pagination pagination, string queryJson)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var queryParam = queryJson.ToJObject();
                pagination.p_kid = "id";
                pagination.p_fields = @"userid,username,departmentid,deptname,dutyid,dutyname,indexvalue,yearvalue,yearmonth,realnum,percents";
                pagination.p_tablename = string.Format(@" (  
                                           with indexTb  as (
                                                  select a.deptid,a.dutyid,a.indexvalue,a.yearvalue,(a.yearvalue ||'-'|| b.month) yearmonth from bis_lllegalquantifyindex a
                                                  left join  (select lpad(level,2,0) as month from dual connect by level <13) b on instr(a.monthvalue,b.month)>0  where 
                                                  to_date(a.yearvalue ||'-'|| b.month||'-01','yyyy-mm-dd hh24:mi:ss') < to_date('{0}','yyyy-mm-dd hh24:mi:ss') 
                                            ),
                                            olduserTb as (
                                                   select count(1) pnum, b.realname username,b.userid,b.sortcode,b.deptsort,b.departmentid,b.deptname ,a.dutyid,c.fullname dutyname ,to_char(a.createdate,'yyyy-MM') yearmonth from bis_lllegalregister a 
                                                   inner join v_userinfo b on a.createuserid = b.userid inner join base_role c on a.dutyid =c.roleid group by 
                                                   b.realname ,b.userid,b.sortcode,b.deptsort,b.departmentid,b.deptname ,a.dutyid,c.fullname ,to_char(a.createdate,'yyyy-MM')
                                            ),
                                            userTb as (
                                                 select  b.indexvalue,b.yearvalue,a.realname username,a.userid,a.sortcode,a.deptsort,a.departmentid,a.deptname ,a.dutyid,a.dutyname, b.yearmonth from v_userinfo a 
                                                 inner join  indexTb b on a.departmentid=b.deptid and a.dutyid=b.dutyid  
                                                 union
                                                 select  b.indexvalue,b.yearvalue,a.username,a.userid,a.sortcode,a.deptsort,a.departmentid,a.deptname ,a.dutyid,a.dutyname, b.yearmonth from olduserTb a 
                                                 inner join indexTb b on a.departmentid=b.deptid and a.dutyid=b.dutyid and a.yearmonth =b.yearmonth 
                                            ) 
                                            select rownum id ,nvl(b.pnum,0) realnum, round((case when nvl(a.indexvalue,0) = 0 then 0 else  nvl(b.pnum,0) / nvl(a.indexvalue,0) * 100 end ),2) percents,
                                             a.indexvalue,a.yearvalue,a.username,a.userid,a.sortcode,a.deptsort,a.departmentid,a.deptname,a.yearmonth,a.dutyid,a.dutyname from userTb a
                                            left join (
                                              select count(1) pnum, to_char(createdate,'yyyy-MM') yearmonth , createuserid,dutyid from bis_lllegalregister group by to_char(createdate,'yyyy-MM'),createuserid,dutyid
                                            ) b on a.userid =b.createuserid and a.yearmonth = b.yearmonth and a.dutyid = b.dutyid
                                       ) a", DateTime.Now.AddMonths(1).ToString("yyyy-MM-01"));
                pagination.conditionJson = string.Format(" 1=1 ");

                if (!queryParam["DutyId"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and dutyid='{0}' ", queryParam["DutyId"].ToString());
                }
                if (!queryParam["DeptId"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and departmentid ='{0}' ", queryParam["DeptId"].ToString());
                }
                if (!queryParam["YearValue"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and yearvalue ='{0}' ", queryParam["YearValue"].ToString());
                }
                if (!queryParam["MonthValue"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and substr(yearmonth,6,2) = '{0}' ", queryParam["MonthValue"].ToString());
                }
                var watch = CommonHelper.TimerStart();
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
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        } 
        #endregion

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
            lllegalquantifyindexbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, LllegalQuantifyIndexEntity entity)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                int count = lllegalquantifyindexbll.GeScalar(entity);
                if (count > 0)
                {
                    return Error("当前年份、部门及岗位下的反违章指标已存在，请重新创建!");
                }
            }
            lllegalquantifyindexbll.SaveForm(keyValue, entity);

            return Success("操作成功。");
        }
        #endregion

        #region 导入反违章量化指标信息
        /// <summary>
        /// 导入反违章量化指标信息
        /// </summary>
        /// <param name="repeatdata"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportInfo(string repeatdata)
        {
            if (OperatorProvider.Provider.Current().IsSystem)
            {
                return "超级管理员无此操作权限";
            }
            var curUser = OperatorProvider.Provider.Current();
            string message = "请选择格式正确的文件再导入!";
            string falseMessage = "";
            int count = HttpContext.Request.Files.Count;
            try
            {
                List<string> listIds = new List<string>();
                if (count > 0)
                {
                    if (HttpContext.Request.Files.Count > 2)
                    {
                        return "请按正确的方式导入文件,一次上传最多支持两份文件(即一份excel数据文件,一份图片压缩文件).";
                    }
                    HttpPostedFileBase file = HttpContext.Request.Files[0];
                    string hiddenDirectory = string.Empty;
                    Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();

                    if (string.IsNullOrEmpty(file.FileName))
                    {
                        return message;
                    }
                    if (!(file.FileName.ToLower().Substring(file.FileName.ToLower().IndexOf('.')).Contains("xls") || file.FileName.ToLower().Substring(file.FileName.ToLower().IndexOf('.')).Contains("xlsx")))
                    {
                        return message;
                    }
                    string fileName1 = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName1));
                    wb.Open(Server.MapPath("~/Resource/temp/" + fileName1));

                    Worksheet sheets = wb.Worksheets[0];
                    Aspose.Cells.Cells cells = sheets.Cells;
                    DataTable dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn + 1, true);
                    //记录错误信息
                    List<string> resultlist = new List<string>();
                    List<DepartmentEntity> dlist = departmentBLL.GetList().OrderBy(p => p.SortCode).ToList();
                    int total = 0;

                    #region 反违章指标部分
                    #region 对象装载
                    List<ImportQuantifyIndex> list = new List<ImportQuantifyIndex>();
                    //先获取到职务列表;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string resultmessage = "第" + (i + 1).ToString() + "行数据"; //显示错误

                        bool isadddobj = true;
                        //年度
                        string yearvalue = dt.Columns.Contains("年度") ? dt.Rows[i]["年度"].ToString().Trim() : string.Empty;
                        //部门
                        string deptname = dt.Columns.Contains("部门") ? dt.Rows[i]["部门"].ToString().Trim() : string.Empty;
                        //岗位
                        string dutyname = dt.Columns.Contains("岗位名称") ? dt.Rows[i]["岗位名称"].ToString().Trim() : string.Empty;
                        //反违章指标
                        string indexvalue = dt.Columns.Contains("反违章指标") ? dt.Rows[i]["反违章指标"].ToString().Trim() : string.Empty;

                        try
                        {
                            #region 对象集合
                            ImportQuantifyIndex entity = new ImportQuantifyIndex();
                            //年度
                            if (!string.IsNullOrEmpty(yearvalue))
                            {
                                entity.YearValue = yearvalue;
                            }
                            //反违章指标
                            if (!string.IsNullOrEmpty(indexvalue))
                            {
                                entity.IndexValue = indexvalue;
                            }

                            //部门
                            if (!string.IsNullOrEmpty(deptname))
                            {
                                if (deptname.Contains("/"))
                                {
                                    string[] depts = deptname.Split('/');
                                    var deptlist = dlist.Where(p => p.FullName == depts[0].ToString()).ToList();
                                    if (deptlist.Count() > 0)
                                    {
                                        var childdeptlist = dlist.Where(p => p.FullName == depts[1].ToString() && p.ParentId == deptlist.FirstOrDefault().DepartmentId).ToList();
                                        if (childdeptlist.Count() > 0)
                                        {
                                            var dutydeptentity = childdeptlist.FirstOrDefault();
                                            entity.DeptId = dutydeptentity.DepartmentId;
                                            entity.DeptName = dutydeptentity.FullName;
                                        }
                                    }
                                }
                                else
                                {
                                    var deptlist = dlist.Where(e => e.FullName == deptname).ToList();
                                    if (deptlist.Count() > 0)
                                    {
                                        var dutydeptentity = deptlist.FirstOrDefault();
                                        entity.DeptId = dutydeptentity.DepartmentId;
                                        entity.DeptName = dutydeptentity.FullName;
                                    }
                                }
                            }
                            //岗位
                            if (!string.IsNullOrEmpty(dutyname) && !string.IsNullOrEmpty(entity.DeptId))
                            {
                                IList<RoleEntity> rolelist = new List<RoleEntity>();
                                rolelist = postCache.GetRealList(entity.DeptId).Where(p => p.FullName == dutyname).OrderBy(x => x.SortCode).ToList();
                                if (rolelist.Count() > 0)
                                {
                                    var roleDefault = rolelist.FirstOrDefault();
                                    entity.DutyId = roleDefault.RoleId;
                                    entity.DutyName = roleDefault.FullName;
                                }
                            }
                            #endregion

                            #region 必填验证
                            //部门
                            if (string.IsNullOrEmpty(deptname))
                            {
                                resultmessage += "部门为空、";
                                isadddobj = false;
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(entity.DeptId))
                                {
                                    resultmessage += "部门填写错误或不存在、";
                                    isadddobj = false;
                                }
                            }
                            //岗位
                            if (string.IsNullOrEmpty(dutyname))
                            {
                                resultmessage += "岗位为空、";
                                isadddobj = false;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(entity.DeptId))
                                {
                                    if (string.IsNullOrEmpty(entity.DutyId))
                                    {
                                        resultmessage += "部门下的岗位不存在、";
                                        isadddobj = false;
                                    }
                                }
                            }
                            //年度
                            if (string.IsNullOrEmpty(yearvalue))
                            {
                                resultmessage += "年度为空、";
                                isadddobj = false;
                            }
                            //反违章指标
                            if (string.IsNullOrEmpty(indexvalue))
                            {
                                resultmessage += "反违章指标为空、";
                                isadddobj = false;
                            }

                            if (isadddobj)
                            {
                                list.Add(entity);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(resultmessage))
                                {
                                    resultmessage = resultmessage.Substring(0, resultmessage.Length - 1) + ",无法正常导入";
                                    resultlist.Add(resultmessage);
                                }
                            }
                            #endregion
                        }
                        catch
                        {
                            resultmessage += "出现数据异常,无法正常导入";
                            resultlist.Add(resultmessage);
                        }
                    }
                    if (resultlist.Count > 0)
                    {
                        foreach (string str in resultlist)
                        {
                            falseMessage += str + "</br>";
                        }
                    }
                    #endregion
                    #region 反违章指标数据集合

                    foreach (ImportQuantifyIndex entity in list)
                    {
                        string keyValue = string.Empty;
                        int excuteVal = 0;
                        //基本信息
                        LllegalQuantifyIndexEntity baseentity = new LllegalQuantifyIndexEntity();

                        //获取数据
                        var data = lllegalquantifyindexbll.GetList(string.Empty).Where(p => p.DEPTID == entity.DeptId && p.DUTYID == entity.DutyId && p.YEARVALUE == entity.YearValue);

                        if (data.Count() > 0)
                        {
                            //覆盖操作
                            if (repeatdata == "1")
                            {
                                baseentity = data.FirstOrDefault();
                                if (null != baseentity)
                                {
                                    keyValue = baseentity.ID;
                                }
                                excuteVal = 1;
                            }
                            else  //跳过
                            {
                                excuteVal = 0;
                            }
                        }
                        else
                        {
                            excuteVal = 1;
                        }
                        if (excuteVal > 0)
                        {
                            baseentity.DEPTID = entity.DeptId; //部门id
                            baseentity.DEPTNAME = entity.DeptName;//部门名称
                            baseentity.DUTYID = entity.DutyId; //岗位id
                            baseentity.DUTYNAME = entity.DutyName;//岗位名称
                            baseentity.INDEXVALUE = int.Parse(entity.IndexValue); //反违章指标
                            baseentity.YEARVALUE = entity.YearValue; //年度
                            baseentity.MONTHVALUE = "01,02,03,04,05,06,07,08,09,10,11,12";
                            lllegalquantifyindexbll.SaveForm(keyValue, baseentity);
                            total += 1;
                        }
                    }
                    #endregion
                    #endregion
                    count = dt.Rows.Count;
                    message = "共有" + count.ToString() + "条记录,成功导入" + total.ToString() + "条,失败" + (count - total).ToString() + "条";
                    message += "</br>" + falseMessage;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return message;
        }
        #endregion

        #region 导出反违章指标信息
        /// <summary>
        /// 导出反违章指标信息
        /// </summary>
        /// <param name="queryJson"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ActionResult ExportIndexInfo(string queryJson, string fileName)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string p_fields = string.Empty;
            string p_fieldsName = string.Empty;
            try
            {
                var queryParam = queryJson.ToJObject();
                pagination.p_kid = "id";
                pagination.p_fields = @"deptname,dutyname,indexvalue,yearvalue";
                pagination.p_tablename = string.Format(@"( select a.* ,b.encode ,b.sortcode,b.deptcode from bis_lllegalquantifyindex a left join base_department b on a.deptid = b.departmentid ) a");
                pagination.conditionJson = string.Format(" 1=1 ");

                if (!queryParam["DutyId"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and dutyid='{0}' ", queryParam["DutyId"].ToString());
                }
                if (!queryParam["DeptId"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and deptid ='{0}' ", queryParam["DeptId"].ToString());
                }
                if (!queryParam["YearValue"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and yearvalue ='{0}' ", queryParam["YearValue"].ToString());
                }
                pagination.sord = "asc";
                pagination.sidx = "yearvalue desc ,deptcode asc ,sortcode";
                //取出数据源
                var exportTable = lllegalregisterbll.GetGeneralQuery(pagination);
                string fielname = fileName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = fileName;
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 25;
                excelconfig.FileName = fielname;
                excelconfig.IsAllSizeColumn = true;
                //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                ColumnEntity columnentity = new ColumnEntity();
                listColumnEntity.Add(new ColumnEntity() { Column = "deptname".ToLower(), ExcelColumn = "部门" });
                listColumnEntity.Add(new ColumnEntity() { Column = "dutyname".ToLower(), ExcelColumn = "岗位" });
                listColumnEntity.Add(new ColumnEntity() { Column = "indexvalue".ToLower(), ExcelColumn = "反违章指标" });
                listColumnEntity.Add(new ColumnEntity() { Column = "yearvalue".ToLower(), ExcelColumn = "月份" });
                excelconfig.ColumnEntity = listColumnEntity;
                exportTable.Columns.Remove("id");
                //调用导出方法
                ExcelHelper.ExcelDownload(exportTable, excelconfig);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
            return Success("导出成功!");
        }
        #endregion

        #region 导出反违章指标统计信息
        /// <summary>
        /// 导出反违章指标统计信息
        /// </summary>
        /// <param name="queryJson"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ActionResult ExportExcel(string queryJson, string fileName)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string p_fields = string.Empty;
            string p_fieldsName = string.Empty;
            try
            {
                var queryParam = queryJson.ToJObject();
                pagination.p_kid = "id";
                pagination.p_fields = @"username,deptname,dutyname,indexvalue,yearmonth,realnum,percents";
                pagination.p_tablename = string.Format(@" (  
                                           with indexTb  as (
                                                  select a.deptid,a.dutyid,a.indexvalue,a.yearvalue,(a.yearvalue ||'-'|| b.month) yearmonth from bis_lllegalquantifyindex a
                                                  left join  (select lpad(level,2,0) as month from dual connect by level <13) b on instr(a.monthvalue,b.month)>0  where 
                                                  to_date(a.yearvalue ||'-'|| b.month||'-01','yyyy-mm-dd hh24:mi:ss') < to_date('{0}','yyyy-mm-dd hh24:mi:ss') 
                                            ),
                                            olduserTb as (
                                                   select count(1) pnum, b.realname username,b.userid,b.sortcode,b.deptsort,b.departmentid,b.deptname ,a.dutyid,c.fullname dutyname ,to_char(a.createdate,'yyyy-MM') yearmonth from bis_lllegalregister a 
                                                   inner join v_userinfo b on a.createuserid = b.userid inner join base_role c on a.dutyid =c.roleid group by 
                                                   b.realname ,b.userid,b.sortcode,b.deptsort,b.departmentid,b.deptname ,a.dutyid,c.fullname ,to_char(a.createdate,'yyyy-MM')
                                            ),
                                            userTb as (
                                                 select  b.indexvalue,b.yearvalue,a.realname username,a.userid,a.sortcode,a.deptsort,a.departmentid,a.deptname ,a.dutyid,a.dutyname, b.yearmonth from v_userinfo a 
                                                 inner join  indexTb b on a.departmentid=b.deptid and a.dutyid=b.dutyid  
                                                 union
                                                 select  b.indexvalue,b.yearvalue,a.username,a.userid,a.sortcode,a.deptsort,a.departmentid,a.deptname ,a.dutyid,a.dutyname, b.yearmonth from olduserTb a 
                                                 inner join indexTb b on a.departmentid=b.deptid and a.dutyid=b.dutyid and a.yearmonth =b.yearmonth 
                                            ) 
                                            select rownum id ,nvl(b.pnum,0) realnum, (to_char(round((case when nvl(a.indexvalue,0) = 0 then 0 else  nvl(b.pnum,0) / nvl(a.indexvalue,0) * 100 end ),2))||'%') percents,
                                             a.indexvalue,a.yearvalue,a.username,a.userid,a.sortcode,a.deptsort,a.departmentid,a.deptname,a.yearmonth,a.dutyid,a.dutyname from userTb a
                                            left join (
                                              select count(1) pnum, to_char(createdate,'yyyy-MM') yearmonth , createuserid,dutyid from bis_lllegalregister group by to_char(createdate,'yyyy-MM'),createuserid,dutyid
                                            ) b on a.userid =b.createuserid and a.yearmonth = b.yearmonth and a.dutyid = b.dutyid
                                       ) a", DateTime.Now.AddMonths(1).ToString("yyyy-MM-01"));
                pagination.conditionJson = string.Format(" 1=1 ");

                if (!queryParam["DutyId"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and dutyid='{0}' ", queryParam["DutyId"].ToString());
                }
                if (!queryParam["DeptId"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and departmentid ='{0}' ", queryParam["DeptId"].ToString());
                }
                if (!queryParam["YearValue"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and yearvalue ='{0}' ", queryParam["YearValue"].ToString());
                }
                if (!queryParam["MonthValue"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and substr(a.yearmonth,6,2) = '{0}' ", queryParam["MonthValue"].ToString());
                }
                pagination.sord = "desc";
                pagination.sidx = "yearmonth desc,deptsort asc,sortcode asc,userid";
                //取出数据源
                var exportTable = lllegalregisterbll.GetGeneralQuery(pagination);
                string fielname = fileName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = fileName;
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 25;
                excelconfig.FileName = fielname;
                excelconfig.IsAllSizeColumn = true;
                //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                ColumnEntity columnentity = new ColumnEntity();
                listColumnEntity.Add(new ColumnEntity() { Column = "username".ToLower(), ExcelColumn = "姓名" });
                listColumnEntity.Add(new ColumnEntity() { Column = "deptname".ToLower(), ExcelColumn = "部门" });
                listColumnEntity.Add(new ColumnEntity() { Column = "dutyname".ToLower(), ExcelColumn = "岗位" });
                listColumnEntity.Add(new ColumnEntity() { Column = "indexvalue".ToLower(), ExcelColumn = "反违章指标" });
                listColumnEntity.Add(new ColumnEntity() { Column = "yearmonth".ToLower(), ExcelColumn = "月份" });
                listColumnEntity.Add(new ColumnEntity() { Column = "realnum".ToLower(), ExcelColumn = "实际反违章数量" });
                listColumnEntity.Add(new ColumnEntity() { Column = "percents".ToLower(), ExcelColumn = "完成率" });
                excelconfig.ColumnEntity = listColumnEntity;
                exportTable.Columns.Remove("id");
                //调用导出方法
                ExcelHelper.ExcelDownload(exportTable, excelconfig);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
            return Success("导出成功!");
        }
        #endregion
    }
}