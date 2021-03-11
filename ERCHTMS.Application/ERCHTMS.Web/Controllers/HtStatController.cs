using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Code;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Web.Areas.HiddenTroubleManage.Controllers;
using System.Data;
using ERCHTMS.Busines.SaftyCheck;
namespace ERCHTMS.Web.Controllers
{
    public class HtStatController : Controller
    {
        HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL();
        UserBLL userbll = new UserBLL(); //用户操作对象
        DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        /// <summary>
        /// 隐患等级统计图
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetHtLevelChart(string queryJson)
        {
            var curUser = ERCHTMS.Code.OperatorProvider.Provider.Current(); //当前用户
            var queryParam = queryJson.ToJObject();
            string deptCode = curUser.DeptCode;  //部门
            string year = DateTime.Now.ToString();  //年度
            if (!queryParam["year"].IsEmpty())
            {
                year = queryParam["year"].ToString();
            }
            //判断是否是厂级用户或者是公司用户

            string hidPlantLevel = dataitemdetailbll.GetItemValue("HidPlantLevel");

            string hidOrganize = dataitemdetailbll.GetItemValue("HidOrganize");

            string CompanyRole = hidPlantLevel + "," + hidOrganize;


            StatisticsEntity hentity = new StatisticsEntity();
            hentity.sDeptCode = deptCode;
            hentity.sYear = year;
            hentity.sAction = "2";
            hentity.sOrganize = curUser.OrganizeId;

            var userList = userbll.GetUserListByDeptCode(curUser.DeptCode, CompanyRole, false, curUser.OrganizeId).Where(p => p.UserId == curUser.UserId).ToList();
            //当前用户是厂级
            if (userList.Count() > 0)
            {
                hentity.isCompany = true;
            }
            else
            {
                hentity.isCompany = false;
            }
            //隐患等级统计图
            var hidrank = htbaseinfobll.QueryStatisticsByAction(hentity);
            return hidrank.ToJson();
        }
        /// <summary>
        /// 隐患数量变化趋势图
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetHtNumChart(string queryJson)
        {
            var curUser = ERCHTMS.Code.OperatorProvider.Provider.Current(); //当前用户
            var queryParam = queryJson.ToJObject();

            string deptCode = curUser.DeptCode;  //部门
            string year = DateTime.Now.Year.ToString();
            if (!queryParam["year"].IsEmpty())
            {
                year = queryParam["year"].ToString();
            }
            string hidPoint ="";  //区域 
            string hidRank = string.Empty;
            if (!queryParam["level"].IsEmpty())
            {
                hidRank = queryParam["level"].ToString().Replace("请选择", "");  //隐患级别
            }
            string ischeck ="";
            string checkType = "";
            StatisticsEntity hentity = new StatisticsEntity();
            hentity.sDeptCode = deptCode;
            hentity.sYear = year;
            hentity.sArea = hidPoint;
            hentity.sHidRank = hidRank;
            hentity.sAction = "4";
            hentity.isCheck = ischeck;
            hentity.sCheckType = checkType;

            var dt = htbaseinfobll.QueryStatisticsByAction(hentity);

            IList<series> slist = new List<series>();

            if (!string.IsNullOrEmpty(hidRank))
            {
                string[] arr = hidRank.Split(',');

                //单个或多个隐患级别
                foreach (string aStr in arr)
                {
                    series s = new series();
                    s.name = aStr;
                    List<int> dlist = new List<int>();
                    foreach (DataRow row in dt.Rows)
                    {
                        int tempValue = 0;
                        if (aStr == "一般隐患")
                        {
                            tempValue = Convert.ToInt32(row["OrdinaryHid"].ToString());
                        }
                        else  //重大隐患
                        {
                            tempValue = Convert.ToInt32(row["ImportanHid"].ToString());
                        }
                        dlist.Add(tempValue);
                    }
                    s.data = dlist;
                    slist.Add(s);
                }
            }
            else   //无隐患级别条件
            {
                series s = new series();
                s.name = "所有隐患";
                List<int> dlist = new List<int>();
                foreach (DataRow row in dt.Rows)
                {
                    int tempValue = Convert.ToInt32(row["allhid"].ToString());

                    dlist.Add(tempValue);
                }
                s.data = dlist;
                slist.Add(s);
            }
            return Content(slist.ToJson());
        }
        /// <summary>
        /// 隐患整改情况变化趋势图
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetHtNumChangeChart(string queryJson)
        {
            var curUser = ERCHTMS.Code.OperatorProvider.Provider.Current(); //当前用户
            var queryParam = queryJson.ToJObject();

            string deptCode = curUser.DeptCode;  //部门
            string year = DateTime.Now.Year.ToString();  //年度
            if (!queryParam["year"].IsEmpty())
            {
                year = queryParam["year"].ToString();
            }
            string hidPoint = "";  //区域 
            StatisticsEntity hentity = new StatisticsEntity();
            hentity.sDeptCode = deptCode;
            hentity.sYear = year;
            hentity.sArea = hidPoint;
            hentity.sAction = "7";

            var dt = htbaseinfobll.QueryStatisticsByAction(hentity);

            IList<fseries> slist = new List<fseries>();

            fseries s1 = new fseries();
            s1.name = "所有隐患";
            fseries s2 = new fseries();
            s2.name = "一般隐患";
            fseries s3 = new fseries();
            s3.name = "重大隐患";
            List<decimal> list1 = new List<decimal>();
            List<decimal> list2 = new List<decimal>();
            List<decimal> list3 = new List<decimal>();
            foreach (DataRow row in dt.Rows)
            {
                decimal total = Convert.ToDecimal(row["aChangeVal"].ToString()); //总的整改率
                decimal ordinary = Convert.ToDecimal(row["oChangeVal"].ToString()); //一般隐患整改率
                decimal great = Convert.ToDecimal(row["iChangeVal"].ToString()); //重大隐患整改率

                list1.Add(total);
                list2.Add(ordinary);
                list3.Add(great);
            }
            s1.data = list1;
            s2.data = list2;
            s3.data = list3;
            slist.Add(s1);
            slist.Add(s2);
            slist.Add(s3);

            return Content(slist.ToJson());
        }
        /// <summary>
        /// 隐患整改情况饼图
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetHtNumReadjustChart(string queryJson)
        {
            var curUser = ERCHTMS.Code.OperatorProvider.Provider.Current(); //当前用户
            var queryParam = queryJson.ToJObject();

            string deptCode = curUser.DeptCode;  //部门
            string year = queryParam["year"].ToString();  //年度
            string hidRank = "";  //隐患级别
            if (!queryParam["level"].IsEmpty())
            {
                hidRank = queryParam["level"].ToString().Replace("请选择","");
            }
            string hidPoint= "";  //区域
            StatisticsEntity hentity = new StatisticsEntity();
            hentity.sDeptCode = deptCode; //整改部门
            hentity.sArea = hidPoint; //区域
            hentity.sYear = year; //年度
            hentity.sHidRank = hidRank; //隐患级别

            hentity.sAction = "6";   //对比图分析
            //列表
            var dt = htbaseinfobll.QueryStatisticsByAction(hentity);

            IList<series> slist = new List<series>();
            List<int> oVal = new List<int>();
            List<int> iVal = new List<int>();
            series s1 = new series();
            s1.name = "已整改";
            series s2 = new series();
            s2.name = "未整改";
            //图表分析
            foreach (DataRow row in dt.Rows)
            {
                //y 轴Value 
                oVal.Add(Convert.ToInt32(row["yValue"].ToString()));
                iVal.Add(Convert.ToInt32(row["wValue"].ToString()));
            }
            s1.data = oVal; //已整改
            s2.data = iVal; //未整改
            slist.Add(s1);
            slist.Add(s2);

            var jsonData = new { tdata = dt, sdata = slist };

            return Content(jsonData.ToJson());
        }
        /// <summary>
        ///安全检查统计
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <param name="year">年份</param>
        /// <param name="belongdistrict">区域</param>
        /// <param name="ctype">检查类型</param>
        /// <returns></returns>
        [HttpGet]
        public string GetCheckNumChart(string year = "", string ctype = "")
        {
            var curUser = ERCHTMS.Code.OperatorProvider.Provider.Current(); //当前用户
            return new SaftyCheckDataRecordBLL().GetSaftyList(curUser.DeptCode, year, "", ctype);
        }
        
    }
}