using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.Offices;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.MatterManage;
using ERCHTMS.Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.HJBPerson.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class AttendanceController : MvcControllerBase
    {
        private UserBLL userBLL = new UserBLL();



        #region 视图功能

        /// <summary>
        /// 统计页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.RoleName.Contains("承包商"))
            {
                ViewBag.Mode = "400";
            }
            else
            {
                ViewBag.Mode = "22";
            }
            return View();
        }
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ListInfo()
        {

            return View();
        }

        #endregion



        #region 获取数据


        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>   
        //[HandlerMonitor(3, "分页查询用户信息!")]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            string where = ""; string where1 = "";
            var queryParam = queryJson.ToJObject();
            if (!queryParam["startTime"].IsEmpty())
            {//时间
                string startTime = queryParam["startTime"].ToString();
                startTime = Convert.ToDateTime(startTime).ToString("yyyy-MM-dd") + " 00:00:00";
                if (!string.IsNullOrEmpty(startTime))
                {
                    string endTime = queryParam["startTime"].ToString();
                    where += string.Format(" and TO_CHAR(CREATEDATE,'yyyy-MM-dd HH24:mm:ss') >= '{0}' and  TO_CHAR(CREATEDATE,'yyyy-MM-dd HH24:mm:ss') <= '{1}' ", startTime, endTime);
                }
            }
            if (!queryParam["ComType"].IsEmpty())
            {//单位类型
                string ComType = queryParam["ComType"].ToString() == "长协单位" ? "长协" : "临时";
                where1 = string.Format(" and s.depttype='{0}' ", ComType);
            }
            else
            {
                where1 = string.Format(" and s.depttype in ('长协','临时') ");
            }
            //if (!queryParam["departmentCode"].IsEmpty())
            //{//部门
            //    string departmentCode = queryParam["departmentCode"].ToString();
            //    where1 = string.Format(" and d.departmentcode='{0}' ", departmentCode);
            //}
            //if (!queryParam["departmentCode"].IsEmpty())
            //{//部门
            //    string deptCode = queryParam["departmentCode"].ToString();
            //    where1 += string.Format(" and  instr(departmentcode,'{0}')>0", deptCode);
            //}
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!queryParam["code"].IsEmpty() && !queryParam["isOrg"].IsEmpty())
            {
                string deptCode = queryParam["code"].ToString();
                string deptType = "";
                if (deptCode.StartsWith("cx100_"))
                {
                    deptCode = deptCode.Replace("cx100_", "");
                    deptType = "长协";
                }
                if (deptCode.StartsWith("ls100_"))
                {
                    deptCode = deptCode.Replace("ls100_", "");
                    deptType = "临时";
                }
                if (queryParam["isOrg"].ToString() == "Organize")
                {
                    where1 += string.Format(" and organizecode='{0}'", deptCode);
                }

                else if (queryParam["isOrg"].ToString() == "Department" || user.IsSystem)
                {
                    where1 += string.Format(" and (d.departmentcode like '{0}%' or nickname  in (select departmentid from base_department where encode like '{0}%'))", deptCode);
                }
                else
                {
                    where1 += string.Format(" and (d.departmentcode='{0}' or nickname in (select departmentid from base_department where encode='{0}'))", deptCode);
                }
                if (!string.IsNullOrWhiteSpace(deptType))
                {
                    where1 += string.Format("and d.departmentid in(select departmentid from base_department d where d.depttype='{0}' and d.encode like '{1}%')", deptType, deptCode);
                }
            }

            string where2 = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "departmentcode", "organizecode");
            if (!string.IsNullOrEmpty(where2) && (queryParam["code"].IsEmpty() || !queryParam["code"].IsEmpty()))
            {//默认当前登录用户
                where1 += " and " + where2;
            }

            pagination.p_kid = "tt.departmentid";
            pagination.p_fields = "tt.Answer, fullname as deptname, ss.RealCount,tt.SortCode,tt.deptcode,tt.depttype,'' as NotNum ";
            pagination.p_tablename = @" (select d.departmentid, s.fullname,s.SortCode,s.deptcode, s.depttype,count(1) as Answer
                                    from base_user d
                                    join base_department s on d.departmentid = s.departmentid  " + where1 + "  group by d.departmentid, s.fullname,s.SortCode,s.deptcode,s.depttype) tt left join ( select deptname, deptid, count(1) as RealCount  from (select distinct (userid), aa.deptname, aa.deptid from (select a.deptname, a.deptid, a.userid from bis_hikinoutlog a where a.deptid is not null and a.userid in (select userid from  base_user ) " + where + " ) aa)group by deptname, deptid) ss on tt.departmentid = ss.deptid";

            pagination.conditionJson = "1=1  ";
            pagination.page = 1;
            pagination.rows = 100000000;
            var watch = CommonHelper.TimerStart();
            var data = userBLL.GetPageList(pagination, null);
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
        /// 获取人员信息列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetPageUserList(Pagination pagination, string queryJson)
        {
            string where = ""; string sql = string.Empty;
            var queryParam = queryJson.ToJObject();
            if (!queryParam["Modes"].IsEmpty() && !queryParam["startTime"].IsEmpty())
            {
                string mode = queryParam["Modes"].ToString();
                string startTime = queryParam["startTime"].ToString();
                startTime = Convert.ToDateTime(startTime).ToString("yyyy-MM-dd") + " 00:00:00";
                if (!string.IsNullOrEmpty(startTime) && mode != "0")
                {
                    string endTime = queryParam["startTime"].ToString();
                    sql = string.Format(" and TO_CHAR(CREATEDATE,'yyyy-MM-dd HH24:mm:ss') >= '{0}' and  TO_CHAR(CREATEDATE,'yyyy-MM-dd HH24:mm:ss') <= '{1}' ", startTime, endTime);
                    if (mode == "1")
                    {//实际出勤人数
                        where = string.Format(@" and d.userid in ( select distinct (userid)
                       from (select a.deptname, a.deptid, a.userid
                       from bis_hikinoutlog a
                       where a.deptid is not null  {0} ) aa )  ", sql);
                    }
                    else if (mode == "2")
                    {//未出勤人数
                        where = string.Format(@" and d.userid not in (select userid from bis_hikinoutlog s where 1=1 {0} )  ", sql);
                    }
                }
            }
            pagination.p_kid = "d.userid";
            pagination.p_fields = "d.realname,d.gender,d.departmentid,d.mobile,t.fullname,d.dutyname,d.usertype";
            pagination.p_tablename = @"base_user d join base_department t on d.departmentid = t.departmentid  " + where + "  ";
            pagination.conditionJson = "1=1 ";
            if (!queryParam["departId"].IsEmpty())//departmentcode
            {
                string departmentId = queryParam["departId"].ToString();
                if (departmentId == "0")
                {
                    string datatype = string.Empty;
                    if (!queryParam["code"].IsEmpty())
                    {//部门

                        string deptCode = queryParam["code"].ToString();
                        if (deptCode.StartsWith("cx100_"))
                        {
                            deptCode = deptCode.Replace("cx100_", "");
                            datatype = "长协";
                        }
                        if (deptCode.StartsWith("ls100_"))
                        {
                            deptCode = deptCode.Replace("ls100_", "");
                            datatype = "临时";
                        }

                        pagination.conditionJson += string.Format(" and (d.departmentcode like '{0}%' or nickname  in (select departmentid from base_department where encode like '{0}%'))", deptCode);
                    }
                    if (!queryParam["ComType"].IsEmpty())
                    {//单位类型
                        string ComType = queryParam["ComType"].ToString() == "长协单位" ? "长协" : "临时";
                        pagination.conditionJson += string.Format(" and t.depttype='{0}' ", ComType);
                    }
                    else if (!string.IsNullOrEmpty(datatype))
                    {
                        pagination.conditionJson += string.Format(" and t.depttype='{0}' ", datatype);
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(" and t.depttype in ('长协','临时') ");
                    }
                }
                else
                {
                    pagination.conditionJson += string.Format(" and d.DEPARTMENTID = '{0}'", departmentId);
                }
            }
            var watch = CommonHelper.TimerStart();
            var data = userBLL.GetPageList(pagination, queryJson);
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
        /// 未考勤人员统计
        /// </summary>
        /// <returns></returns>
        public ActionResult GetComNameUserCount(string queryJson)
        {
            string where = ""; string qc = string.Empty;
            var queryParam = queryJson.ToJObject();
            string startTime = queryParam["startTime"].ToString();
            string ComType = queryParam["ComType"].ToString();
            startTime = Convert.ToDateTime(startTime).ToString("yyyy-MM-dd") + " 00:00:00";
            if (!string.IsNullOrEmpty(startTime))
            {
                string endTime = queryParam["startTime"].ToString();
                qc = string.Format(" and TO_CHAR(CREATEDATE,'yyyy-MM-dd HH24:mm:ss') >= '{0}' and  TO_CHAR(CREATEDATE,'yyyy-MM-dd HH24:mm:ss') <= '{1}' ", startTime, endTime);
                where = string.Format(@" and d.userid not in (select userid from bis_hikinoutlog s where 1=1 {0} )  ", qc);
            }
            if (!string.IsNullOrEmpty(ComType))
            {
                ComType = queryParam["ComType"].ToString() == "长协单位" ? "长协" : "临时";
                where += string.Format(" and t.depttype='{0}' ", ComType);
            }
            string sql = string.Format(" select count(1) from  base_user d join base_department t on d.departmentid = t.departmentid and d.IsEpiboly='1' {0} ", where);
            var dt = new OperticketmanagerBLL().GetDataTable(sql);
            return Content(dt.Rows[0][0].ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetTimeNew()
        {
            return Content(DateTime.Now.ToString());
        }

        /// <summary>
        /// 导出用户列表
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出用户数据")]
        public ActionResult ExportUserList(string condition, string queryJson)
        {
            Pagination pagination = new Pagination();
            string where = ""; string where1 = "";
            var queryParam = queryJson.ToJObject();
            if (!queryParam["startTime"].IsEmpty())
            {//时间
                string startTime = queryParam["startTime"].ToString();
                startTime = Convert.ToDateTime(startTime).ToString("yyyy-MM-dd") + " 00:00:00";
                if (!string.IsNullOrEmpty(startTime))
                {
                    string endTime = queryParam["startTime"].ToString();
                    where += string.Format(" and TO_CHAR(CREATEDATE,'yyyy-MM-dd HH24:mm:ss') >= '{0}' and  TO_CHAR(CREATEDATE,'yyyy-MM-dd HH24:mm:ss') <= '{1}' ", startTime, endTime);
                }
            }
            if (!queryParam["ComType"].IsEmpty())
            {//单位类型
                string ComType = queryParam["ComType"].ToString() == "长协单位" ? "长协" : "临时";
                where1 = string.Format(" and s.depttype='{0}' ", ComType);
            }
            pagination.page = 1;
            pagination.sidx = "deptname";
            pagination.sord = "desc";
            pagination.rows = 100000000;
            pagination.p_kid = "tt.departmentid";
            pagination.p_fields = " fullname as deptname,tt.Answer, ss.RealCount,'' as notnum ";
            pagination.p_tablename = @" (select d.departmentid, s.fullname, count(1) as Answer
                                    from base_user d
                                    join base_department s on d.departmentid = s.departmentid and d.IsEpiboly='1' " + where1 + "  group by d.departmentid, s.fullname ) tt left join (select a.deptname, a.deptid, count(1) as RealCount  from bis_hikinoutlog a where a.deptid is not null " + where + "  group by deptname, deptid) ss on tt.departmentid = ss.deptid";

            pagination.conditionJson = "1=1  ";
            var watch = CommonHelper.TimerStart();
            var data = userBLL.GetPageList(pagination, queryJson);
            foreach (DataRow item in data.Rows)
            {
                if (!string.IsNullOrEmpty(item[3].ToString()))
                    item["notnum"] = int.Parse(item[2].ToString()) - int.Parse(item[3].ToString());
                else
                    item["notnum"] = int.Parse(item[2].ToString());
            }

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "考勤统计";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "外包单位考勤统计.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            ColumnEntity columnentity = new ColumnEntity();
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "deptname", ExcelColumn = "单位名称", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "answer", ExcelColumn = "应出勤人数（人）", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "realcount", ExcelColumn = "实际出勤人数（人）", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "notnum", ExcelColumn = "未出勤人数（人）", Alignment = "center" });

            //调用导出方法
            // ExcelHelper.ExcelDownload(data, excelconfig);

            ExcelHelper.ExportByAspose(data, "考勤统计", excelconfig.ColumnEntity);
            return Success("导出成功。");
        }

        #endregion






    }
}