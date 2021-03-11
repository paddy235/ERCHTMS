using ERCHTMS.Entity.HazardsourceManage;
using ERCHTMS.Busines.HazardsourceManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using BSFramework.Util.Extension;
using System.Linq;
using System;
using System.Data;
using ERCHTMS.Busines.BaseManage;

namespace ERCHTMS.Web.Areas.HazardsourceManage.Controllers
{
    /// <summary>
    /// 描 述：危险源清单
    /// </summary>
    public class Hisrelationhd_qdController : MvcControllerBase
    {
        private Hisrelationhd_qdBLL hisrelationhd_qdbll = new Hisrelationhd_qdBLL();
        private HazardsourceBLL hazardsourcebll = new HazardsourceBLL();
        private HistoryBLL historybll = new HistoryBLL();
        private Hisrelationhd_qdBLL hisrelationhd_qdbLL = new Hisrelationhd_qdBLL();
        private DistrictBLL bis_districtbll = new DistrictBLL();
        #region 视图功能


        [HttpGet]
        public ActionResult Report()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ShowMeaSure()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DangerList()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Select()
        {
            return View();
        }
        [HttpGet]
        public ActionResult StatisticsDefault()
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
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = hisrelationhd_qdbll.GetList(queryJson);
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
            var data = hisrelationhd_qdbll.GetEntity(keyValue);
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
            hisrelationhd_qdbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, Hisrelationhd_qdEntity entity)
        {
            hisrelationhd_qdbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion


        #region 数据导出
        /// <summary>
        /// 导出用户列表
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult Export(string condition, string queryJson)
        {
            Pagination pagination = new Pagination();
            queryJson = queryJson ?? "";
            pagination.p_kid = "ID";
            pagination.p_fields = "districtname, DANGERSOURCE, ACCIDENTNAME,DEPTNAME,JDGLZRRFULLNAME,case WHEN  ISDANGER>0 then '是' else '否' end as ISDANGERNAME";
            pagination.p_tablename = "HSD_HAZARDSOURCE t";
            pagination.conditionJson = "1=1";
            pagination.page = 1;
            pagination.rows = 100000000;
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                if (user.RoleName.Contains("省级"))
                {
                    pagination.conditionJson += " and (CreateUserId='" + user.UserId + "' or DeptCode in(select  encode from BASE_DEPARTMENT start with encode='" + user.NewDeptCode + "' connect by  prior  departmentid = parentid))";
                }
                else
                {
                    pagination.conditionJson += " and (CreateUserId='" + user.UserId + "' or DeptCode in(select  encode from BASE_DEPARTMENT start with encode='" + user.DeptCode + "' connect by  prior  departmentid = parentid))";
                }
            }
            var title = "危险源清单";
            if (queryJson.Length > 0)
            {
                var queryParam = queryJson.ToJObject();
                if (!queryParam["type"].IsEmpty())
                {
                    pagination.p_fields = "districtname, DANGERSOURCE, ACCIDENTNAME,DEPTNAME,JDGLZRRFULLNAME,   case WHEN  gradeval=1 then '一级' WHEN  gradeval=3 then '三级' WHEN  gradeval=2 then '二级' WHEN  gradeval=4 then '四级' else '未定级' end as gradevalstr";
                    title = "重大危险源清单";
                }

            }

            pagination.p_fields += @",case WHEN way='LEC' then 'LEC法风险辨识' else '危险化学品重大危险源辨识' end as way";
            if (title == "危险源清单")
                pagination.p_fields += @",case WHEN way='LEC' then ITEMA else 0 end as ITEMA
                                     ,case WHEN way='LEC' then ITEMB else 0 end as ITEMB
                                     ,case WHEN way='LEC' then ITEMC else 0 end as ITEMC
                                     ,case WHEN way='LEC' then ITEMR else 0 end as ITEMR";

            pagination.p_fields += @",case WHEN way='DEC' then ITEMDECQ else '' end as ITEMDECQ
                                     ,case WHEN way='DEC' then ITEMDECQ1 else '' end as ITEMDECQ1
                                     ,case WHEN way='DEC' then ITEMDECB1 else '' end as ITEMDECB1
                                     ,case WHEN way='DEC' then ITEMDECB else 0 end as ITEMDECB
                                     ,case WHEN way='DEC' then ITEMDECR else 0 end as ITEMDECR
                                     ,case WHEN way='DEC' then ITEMDECR1 else 0 end as ITEMDECR1";
            var watch = CommonHelper.TimerStart();
            var data = hazardsourcebll.GetPageList(pagination, queryJson);

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = title;
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = title + ".xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "districtname", ExcelColumn = "所属区域" });
            listColumnEntity.Add(new ColumnEntity() { Column = "DANGERSOURCE".ToLower(), ExcelColumn = "危险源名称/场所" });
            listColumnEntity.Add(new ColumnEntity() { Column = "ACCIDENTNAME".ToLower(), ExcelColumn = "可能导致的事故类型" });
            listColumnEntity.Add(new ColumnEntity() { Column = "deptname".ToLower(), ExcelColumn = "责任部门" });
            listColumnEntity.Add(new ColumnEntity() { Column = "jdglzrrfullname".ToLower(), ExcelColumn = "监督管理责任人" });
            if (title == "危险源清单")
            {
                listColumnEntity.Add(new ColumnEntity() { Column = "isdangername".ToLower(), ExcelColumn = "是否为重大危险源" });

            }
            else
                listColumnEntity.Add(new ColumnEntity() { Column = "gradevalstr".ToLower(), ExcelColumn = "重大危险源级别" });
            listColumnEntity.Add(new ColumnEntity() { Column = "way".ToLower(), ExcelColumn = "风险评估方法" });
            if (title == "危险源清单")
            {
                listColumnEntity.Add(new ColumnEntity() { Column = "ITEMA".ToLower(), ExcelColumn = "L" });
                listColumnEntity.Add(new ColumnEntity() { Column = "ITEMB".ToLower(), ExcelColumn = "E" });
                listColumnEntity.Add(new ColumnEntity() { Column = "ITEMC".ToLower(), ExcelColumn = "C" });
                listColumnEntity.Add(new ColumnEntity() { Column = "ITEMR".ToLower(), ExcelColumn = "D" });
            }



            listColumnEntity.Add(new ColumnEntity() { Column = "ITEMDECQ".ToLower(), ExcelColumn = "危险化学品实际存在量q" });
            listColumnEntity.Add(new ColumnEntity() { Column = "ITEMDECQ1".ToLower(), ExcelColumn = "危险化学品临界量Q" });
            listColumnEntity.Add(new ColumnEntity() { Column = "ITEMDECB1".ToLower(), ExcelColumn = "校正系数β" });
            listColumnEntity.Add(new ColumnEntity() { Column = "ITEMDECB".ToLower(), ExcelColumn = "校正系数α" });
            listColumnEntity.Add(new ColumnEntity() { Column = "ITEMDECR".ToLower(), ExcelColumn = "R" });
            listColumnEntity.Add(new ColumnEntity() { Column = "ITEMDECR1".ToLower(), ExcelColumn = "R1" });
            excelconfig.ColumnEntity = listColumnEntity;

            //调用导出方法
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("导出成功。");
        }
        #endregion


        #region 报表统计
        /// <summary>
        /// 应急演练计划完成率
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetReport()
        {
            string sqlwhere = "";
            var Time = Request["Time"] ?? "";
            var type = int.Parse(Request["type"] ?? "0");
            var returnListObj = new List<Object>();
            var data = new object();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                sqlwhere += " and (CreateUserId='" + user.UserId + "' or DeptCode like '" + user.DeptCode + "%')";
            }
            if (type == 2 && Time.Length > 0)
            {
                sqlwhere += " and (select  to_char(createdate, 'yyyy') from dual)='" + Time + "' ";
            }

            sqlwhere += " and gradeval>0 ";

            switch (type)
            {
                //按级别统计
                case 1:
                    var list = hazardsourcebll.GetList(sqlwhere + " and IsDanger=1 ").Where(e => (Time.Length > 0 ? e.CreateDate.Value.ToString("yyyy") == Time : 1 == 1));

                    var listjb = from l in list
                                 group l by new { l.GradeVal, l.Grade } into t
                                 select new
                                 {
                                     text = t.Key.Grade,
                                     value = t.Count(),
                                 };
                    var count = 0M;
                    foreach (var item in listjb)
                    {
                        count += decimal.Parse(item.value.ToString());
                    }
                    foreach (var item in listjb)
                    {
                        var itemCount = decimal.Parse(item.value.ToString());
                        returnListObj.Add(new { text = item.text, value = itemCount, bfb = decimal.Round((itemCount / count) * 100, 2) });
                    }
                    data = new { list = returnListObj };
                    break;
                //按区域统计
                default:
                    var list2 = hisrelationhd_qdbll.GetReportForDistrictName(sqlwhere);
                    var ListNow = new List<NewReportForDistrictName>();
                    foreach (DataRow datarow in list2.Rows)
                    {
                        ListNow.Add(new NewReportForDistrictName { DistrictCode = datarow["DistrictCode"].ToString(), Grade = datarow["Grade"].ToString() });
                    }
                    //查询出总数
                    var list3 = from l in ListNow
                                group l by new { l.DistrictCode } into t
                                select new
                                {
                                    text = bis_districtbll.GetListForCon(e => e.DistrictCode == t.Key.DistrictCode).ToList().Count>0?bis_districtbll.GetListForCon(e => e.DistrictCode == t.Key.DistrictCode).FirstOrDefault().DistrictName:"",
                                    code = t.Key.DistrictCode,
                                    value = t.Count(),
                                    one = ListNow.Where(e => e.Grade == "一级" && e.DistrictCode == t.Key.DistrictCode).Count(),
                                    two = ListNow.Where(e => e.Grade == "二级" && e.DistrictCode == t.Key.DistrictCode).Count(),
                                    three = ListNow.Where(e => e.Grade == "三级" && e.DistrictCode == t.Key.DistrictCode).Count(),
                                    four = ListNow.Where(e => e.Grade == "四级" && e.DistrictCode == t.Key.DistrictCode).Count(),
                                };
                    //添加到返回数据中
                    data = new { list = list3 };
                    break;
            }

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            return json;

        }


        [HttpGet]
        public string StaQueryList(string queryJson)
        {

            return hisrelationhd_qdbll.StaQueryList(queryJson);
        }
        /// <summary>
        /// 区域获取数据
        /// </summary>
        public class NewReportForDistrictName
        {
            public string DistrictName { get; set; }
            public string DistrictCode { get; set; }
            public string Grade { get; set; }
        }
        #endregion
    }
}
