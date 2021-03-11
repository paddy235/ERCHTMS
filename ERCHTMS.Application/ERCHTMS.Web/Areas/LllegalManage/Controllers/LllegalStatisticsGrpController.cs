using BSFramework.Util;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.LllegalManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.LllegalManage.Controllers
{
    /// <summary>
    /// 省公司违章统计
    /// </summary>
    public class LllegalStatisticsGrpController : MvcControllerBase
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

        #region 获取统计信息
        /// <summary>
        /// 获取违章数量统计
        /// </summary>
        /// <returns></returns>
        public ActionResult QueryLllegalNumberPie(string queryJson)
        {           
            var jsonData = new
            {
                legLevalTotal = legbll.GetLllegalLevelTotalGrp(queryJson),
                legLevalList = legbll.GetLllegalLevelListGrp(queryJson),
                legTypeTotal = legbll.GetLllegalTypeTotalGrp(queryJson),
                legTypeList = legbll.GetLllegalTypeListGrp(queryJson)
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
            var dt1 = legbll.GetLllegalLevelListGrp(queryJson);
            dt1.TableName = "Table1";
            var dt2 = legbll.GetLllegalTypeListGrp(queryJson);
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
            var dt = legbll.GetLllegalTrendDataGrp(queryJson);
            var jsonData = new
            {
                lineTotal = GetLllegalTrendTotal(dt,queryJson),
                lineList = dt
            };
            return Content(jsonData.ToJson());
        }
        /// <summary>
        /// 导出违章趋势统计数据
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HandlerMonitor(0, "数据导出")]
        public ActionResult ExportTrendNumber(string queryJson)
        {            
            var dt = legbll.GetLllegalTrendDataGrp(queryJson);
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
            var dt = legbll.GetLllegalCompareDataGrp(queryJson);
            List<string> arr = new List<string>();
            foreach(DataRow dr in dt.Rows)
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
        /// 导出违章对比统计数据
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HandlerMonitor(0, "数据导出")]
        public ActionResult ExportCompareNumber(string queryJson)
        {            
            var dt = legbll.GetLllegalCompareDataGrp(queryJson);
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
        #endregion
    }
}
