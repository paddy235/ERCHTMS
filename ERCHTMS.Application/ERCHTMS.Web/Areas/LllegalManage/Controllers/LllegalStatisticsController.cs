using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BSFramework.Util;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.BaseManage;
using BSFramework.Application.Entity;
using System.Data;
using ERCHTMS.Code;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.LllegalManage;
using BSFramework.Util.Offices;
using Newtonsoft.Json;
using ERCHTMS.Busines.MatterManage;
using ERCHTMS.Web.Areas.HiddenTroubleManage.Controllers;

namespace ERCHTMS.Web.Areas.LllegalManage.Controllers
{
    /// <summary>
    /// 违章统计
    /// </summary>
    public class LllegalStatisticsController : MvcControllerBase
    {

        private LllegalStatisticsBLL legbll = new LllegalStatisticsBLL();
        private UserBLL userbll = new UserBLL(); //用户操作对象
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult TrendIndex()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CompareIndex()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ZgIndex()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ZgTrendIndex()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ZgCompareIndex() 
        {
            return View();
        }

        [HttpGet]
        public ActionResult CjIndex()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CjTrendIndex()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CjCompareIndex() 
        {
            return View();
        }
       
        #region 判断人员是否公司
        /// <summary>
        /// 判断人员是否公司
        /// </summary>
        /// <returns></returns>
        public ActionResult QueryCurUser()
        {
            string result = "";

            var curUser = new OperatorProvider().Current(); //当前用户

            string hidPlantLevel = dataitemdetailbll.GetItemValue("HidPlantLevel");

            string hidOrganize = dataitemdetailbll.GetItemValue("HidOrganize");

            string CompanyRole = hidPlantLevel + "," + hidOrganize;


            var userList = userbll.GetUserListByDeptCode(curUser.DeptCode, CompanyRole, false, curUser.OrganizeId).Where(p => p.UserId == curUser.UserId).ToList();
            //当前用户是厂级
            if (userList.Count() > 0)
            {
                result = "1";
            }
            else
            {
                result = "0";
            }

            return Content(result);
        }
        #endregion

        #region 获取统计信息
        /// <summary>
        /// 获取违章数量统计
        /// </summary>
        /// <returns></returns>
        public ActionResult QueryLllegalNumberPie(string queryJson)
        {           
            var jsonData = new
            {
                legLevalTotal = legbll.GetLllegalLevelTotal(queryJson),
                legLevalList = legbll.GetLllegalLevelList(queryJson),
                legTypeTotal = legbll.GetLllegalTypeTotal(queryJson),
                legTypeList = legbll.GetLllegalTypeList(queryJson)
            };
            return Content(jsonData.ToJson());
        }        
        /// <summary>
        /// 导出隐患级别数据
        /// </summary>
        /// <param name="queryJson"></param>
        [HandlerMonitor(0, "数据导出")]
        public ActionResult ExportNumber(string queryJson)
        {
            var dt1 = legbll.GetLllegalLevelList(queryJson);
            dt1.TableName = "Table1";
            var dt2 = legbll.GetLllegalTypeList(queryJson);
            dt2.TableName = "Table2";
            var ds = new DataSet("ds");
            ds.Tables.Add(dt1);
            ds.Tables.Add(dt2);
            string fileUrl = @"\Resource\ExcelTemplate\违章统计数量_导出模板.xlsx";
            AsposeExcelHelper.ExecuteResultX(ds, fileUrl, new List<string>() { "违章级别数量", "违章类型数量" }, "违章类型、级别统计数量");

            return Success("导出成功。");
        }       

        /// <summary>
        /// 获取违章趋势统计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QueryLllegalNumberLine(string queryJson)
        {
            var dt = legbll.GetLllegalTrendData(queryJson);
            var jsonData = new
            {
                lineTotal = GetLllegalTrendTotal(dt, queryJson),
                lineList = dt
            };

            return Content(jsonData.ToJson());
        }

        /// <summary>
        /// 获取违章趋势统计(可门电厂专用)
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QueryLllegalNumberLineKm(string queryJson)
        {
            List<object> dic = new List<object>();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            queryJson = "{'deptCode':'" + user.DeptCode + "','year':'" + DateTime.Now.Year + "','levelGroups':'一般违章,较严重违章,严重违章','DepartmentName':''}";

            var dt = legbll.GetLllegalTrendData(queryJson);
            var list = GetLllegalTrendTotal(dt, queryJson);
            List<int> Clist = new List<int>();
            if (list.Count > 3)
            {//发现违章数量集
                LllegalNumberEntity entity = JsonConvert.DeserializeObject<LllegalNumberEntity>(list[3].ToJson());//发现违章数
                dic.Add(new { name = "发现违章数", type = "spline", yAxis = 1, data = entity.data });
                Clist = entity.data;
            }

            var queryParam = queryJson.ToJObject();
            string deptCode = queryParam["deptCode"].ToString();  //部门
            string year = queryParam["year"].ToString();  //年度
            string sql = string.Format(@"select u.month,
           (select count(1)
           from v_lllegalbaseinfo
           where 1 = 1
           and lllegalteamcode like '{0}%'
           and lllegalteamcode in
           (select encode from base_department where encode like '{0}%')
           and to_char(lllegaltime, 'yyyy') = {1}
           and extract(month from lllegaltime) = u.month
           and flowstate ! = '流程结束') as 未闭环违章
           from (select rownum as month from dual connect by rownum <= 12) u", deptCode, year);
            List<decimal> list1 = new List<decimal>();
            List<decimal> list2 = new List<decimal>();

            DataTable opdt = new OperticketmanagerBLL().GetDataTable(sql);
            for (int i = 0; i < opdt.Rows.Count; i++)
            {
                decimal str1 = Convert.ToDecimal(opdt.Rows[i][1].ToString());//未闭环违章数
                decimal str2 = Clist[i];//违章总数
                list1.Add(str1);
                if (str2 == 0) { list2.Add(0); }
                else { list2.Add(Math.Round((str2 - str1) / str2 * 100, 2)); }//违章整改率
            }

            dic.Add(new { name = "未闭环违章数", type = "spline", yAxis = 1, data = list1 });
            dic.Add(new { name = "违章整改率%", type = "spline", yAxis = 0, data = list2 });

            return Content(dic.ToJson());
        }


        /// <summary>
        /// 导出违章趋势统计数据
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HandlerMonitor(0, "数据导出")]
        public ActionResult ExportTrendNumber(string queryJson)
        {            
            var dt = legbll.GetLllegalTrendData(queryJson);
            string fileUrl = @"\Resource\ExcelTemplate\违章趋势数量_导出模板.xlsx";
            AsposeExcelHelper.ExecuteResult(dt, fileUrl,"违章数量", "违章趋势统计数量");

            return Success("导出成功。");
        }
        /// <summary>
        /// 获取违章对比统计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QueryLllegalNumberColumn(string queryJson)
        {
            var dt = legbll.GetLllegalCompareData(queryJson);
            List<string> arr = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                arr.Add(dr["fullname"].ToString());
            }
            var list = GetLllegalCompareTotal(dt, queryJson);
            var jsonData = new
            {
                columns= arr,
                columnTotal = list,
                columnList = dt
            };
            return Content(jsonData.ToJson());
        }


        /// <summary>
        /// 获取违章对比统计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QueryLllegalNumberDrilldownColumn(string queryJson) 
        {
            var dt = legbll.GetLllegalCompareData(queryJson);
            List<string> arr = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                arr.Add(dr["fullname"].ToString());
            }
            List<object> ybobjlist = new List<object>();
            List<object> jyzobjlist = new List<object>();
            List<object> yzobjlist = new List<object>(); 
            List<object> cobjlist = new List<object>();  
            //图表分析
            foreach (DataRow row in dt.Rows)
            {
                string dname = row["fullname"].ToString();
                string encode = row["encode"].ToString();
                string departmentid = row["departmentid"].ToString();

                ybobjlist.Add(new { name = dname, y = Convert.ToInt32(row["一般违章"].ToString()), drilldown = "ybwz" + encode });
                jyzobjlist.Add(new { name = dname, y = Convert.ToInt32(row["较严重违章"].ToString()), drilldown = "jyzwz" + encode });
                yzobjlist.Add(new { name = dname, y = Convert.ToInt32(row["严重违章"].ToString()), drilldown = "yzwz" + encode });

                List<object> cyblist = new List<object>();
                List<object> cjyzlist = new List<object>();
                List<object> cyzlist = new List<object>();

                var queryParam = queryJson.ToJObject();
                queryParam["deptCode"] = encode;
                queryParam["deptId"] = departmentid;
                var childrenJson = queryParam.ToJson();
                var cddt = legbll.GetLllegalCompareData(childrenJson);
                foreach (DataRow crow in cddt.Rows) 
                {
                    cyblist.Add(new { name = crow["fullname"].ToString(), y = Convert.ToInt32(crow["一般违章"].ToString()), drilldown = "next_ybwz" + crow["encode"].ToString() });
                    cjyzlist.Add(new { name = crow["fullname"].ToString(), y = Convert.ToInt32(crow["较严重违章"].ToString()), drilldown = "next_jyzwz" + crow["encode"].ToString() });
                    cyzlist.Add(new { name = crow["fullname"].ToString(), y = Convert.ToInt32(crow["严重违章"].ToString()), drilldown = "next_yzwz" + crow["encode"].ToString() }); 
                }
                cobjlist.Add(new { name = "一般违章", id = "ybwz" + encode, data = cyblist });
                cobjlist.Add(new { name = "较严重违章", id = "jyzwz" + encode, data = cjyzlist });
                cobjlist.Add(new { name = "严重违章", id = "yzwz" + encode, data = cyzlist });
            }
            List<object> list = new List<object>(); 
            list.Add(new { name = "一般违章", id = "ybwz", data = ybobjlist });
            list.Add(new { name = "较严重违章", id = "jyzwz", data = jyzobjlist });
            list.Add(new { name = "严重违章", id = "yz", data = yzobjlist });
            var jsonData = new
            {
                columns = arr,
                columnTotal = list,
                columnChildren = cobjlist,
                columnList = dt
            };
            return Content(jsonData.ToJson());
        }
        /// <summary>
        /// 导出违章对比统计数据
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HandlerMonitor(0, "数据导出")]
        public ActionResult ExportCompareNumber(string queryJson)
        {            
            var dt = legbll.GetLllegalCompareData(queryJson);
            string fileUrl = @"\Resource\ExcelTemplate\违章比较数量_导出模板.xlsx";
            AsposeExcelHelper.ExecuteResult(dt, fileUrl, "违章数量", "违章对比统计数量");

            return Success("导出成功。");
        }
        #endregion

        #region 协助方法
        private List<object> GetLllegalTrendTotal(DataTable dt,string queryJson)
        {
            var list = new List<dynamic>();

            if (dt != null && dt.Rows.Count > 0)
            {
                var colors = new Dictionary<string, string>()
                {
                    {"一般违章","#558ED5" },
                    {"较严重违章","#FFC000" },
                    {"严重违章","#FF0000" },
                    {"合计","#000000" }
                };
                var select = dt.Select();
                var queryParam = queryJson.ToJObject();
                var groups = queryParam["levelGroups"].ToString();
                var grpList = groups.Split(new char[] { ',' });
                int[] total = new int[dt.Rows.Count];
                for (var i = 0; i < grpList.Length; i++)
                {
                    var grpName = grpList[i];
                    List<int> data = new List<int>();
                    for(int j=0;j< dt.Rows.Count;j++)
                    {
                        DataRow dr = dt.Rows[j];
                        int num = int.Parse(dr[grpName].ToString());
                        total[j] += num;
                        data.Add(num);
                    }
                    list.Add(new { name = grpName,color= GetColor(colors,grpName), data = data });
                }
                var totalName = "合计";
                list.Add(new { name = totalName, color = GetColor(colors,totalName), data = total });
            }

            return list;
        }
        private List<dynamic> GetLllegalCompareTotal(DataTable dt, string queryJson)
        {
            var list = new List<dynamic>();

            if (dt != null && dt.Rows.Count > 0)
            {
                var colors = new Dictionary<string, string>()
                {
                    {"一般违章","#558ED5" },
                    {"较严重违章","#FFC000" },
                    {"严重违章","#FF0000" }
                };
                var select = dt.Select();
                var queryParam = queryJson.ToJObject();
                var groups = queryParam["levelGroups"].ToString();
                var grpList = groups.Split(new char[] { ',' });                
                for (var i = 0; i < grpList.Length; i++)
                {
                    var grpName = grpList[i];
                    List<int> data = new List<int>();
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        DataRow dr = dt.Rows[j];
                        int num = int.Parse(dr[grpName].ToString());                       
                        data.Add(num);
                    }
                    list.Add(new { name = grpName, color = GetColor(colors,grpName), data = data });
                }               
            }

            return list;
        }
        private string GetColor(Dictionary<string, string> dic, string key)
        {
            Random rnd = new Random((int)DateTime.Now.Ticks);
            string r = string.Format("RGB({0}, {1}, {2})", rnd.Next(255), rnd.Next(255), rnd.Next(255));//默认随机颜色

            if (dic.ContainsKey(key))
                r = dic[key];

            return r;
        }

        public class LllegalNumberEntity
        {
            public string name { get; set; }
            public string color { get; set; }
            public List<int> data { get; set; }
        }


        #endregion


        #region 违章曝光治理
        /// <summary>
        /// 违章曝光
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QueryExposureLllegal(string num) 
        {
            Operator curUser = new OperatorProvider().Current();

            var data = legbll.QueryExposureLllegal(num);

            return Content(data.ToJson());
        }
        #endregion
    }
}
