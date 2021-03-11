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

namespace ERCHTMS.Web.Areas.HiddenTroubleManageBz.Controllers 
{

    /// <summary>
    /// 隐患统计
    /// </summary>
    public class HTStatisticsController : Controller
    {

        private HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL();
        private UserBLL userbll = new UserBLL(); //用户操作对象
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        //
        // GET: /HiddenTroubleManage/HTStatistics/
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
        public ActionResult CheckIndex()
        {
            return View();
        }

        #region 年份
        /// <summary>
        /// 年份
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult QueryTime()
        {
            var list = new List<TimeEntity>();

            for (int i = DateTime.Now.Year; i >= DateTime.Now.Year - 10; i--)
            {
                TimeEntity entity = new TimeEntity();
                entity.id = i.ToString();
                entity.text = i.ToString();
                list.Add(entity);
            }

            return Content(list.ToJson());
        }
        #endregion

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


        #region 隐患数量统计

        #region 获取统计信息
        /// <summary>
        /// 获取隐患信息
        /// </summary>
        /// <returns></returns>
        public ActionResult QueryHidNumberPie(string queryJson)
        {
            var queryParam = queryJson.ToJObject();

            string deptCode = queryParam["deptCode"].ToString();  //部门
            string year = queryParam["year"].ToString();  //年度
            string ischeck = queryParam["ischeck"].ToString();
            string hidPoint = queryJson.Contains("hidPoint") ? queryParam["hidPoint"].ToString() : "";  //区域 
            string hidRank = string.Empty;
            if (null!=queryParam["hidRank"]) 
            {
                hidRank = queryParam["hidRank"].ToString() == "请选择" ? "" : queryParam["hidRank"].ToString();  //隐患级别
            }
    
            string checkType = "";
            if (queryJson.Contains("checkType"))
            {
                checkType = queryParam["checkType"].ToString();
            }
            var curUser = new OperatorProvider().Current(); //当前用户

            //判断是否是厂级用户或者是公司用户

            string hidPlantLevel = dataitemdetailbll.GetItemValue("HidPlantLevel");

            string hidOrganize = dataitemdetailbll.GetItemValue("HidOrganize");

            string CompanyRole = hidPlantLevel + "," + hidOrganize;


            StatisticsEntity hentity = new StatisticsEntity();
            hentity.sDeptCode = deptCode;
            hentity.sArea = hidPoint;
            hentity.sHidRank = hidRank;
            hentity.sYear = year;
            hentity.sAction = "2";
            hentity.isCheck = ischeck;
            hentity.sCheckType = checkType;
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
            //隐患等级统计图
            hentity.sAction = "3";
            var area = htbaseinfobll.QueryStatisticsByAction(hentity);
            //隐患数量列表图
            hentity.sAction = "1";
            var list = htbaseinfobll.QueryStatisticsByAction(hentity);
            var jsonData = new
            {
                hiddata = hidrank,
                areadata = area,
                tdata = list,
                iscompany = hentity.isCompany ? 1 : 0
            };
            return Content(jsonData.ToJson());
        }
        #endregion

        #region 获取隐患数量趋势图
        /// <summary>
        /// 获取隐患数量趋势图
        /// </summary>
        /// <returns></returns>
        public ActionResult QueryHidNumberTendency(string queryJson)
        {
            var queryParam = queryJson.ToJObject();

            string deptCode = queryParam["deptCode"].ToString();  //部门
            string year = queryParam["year"].ToString();  //年度
            string hidPoint = queryParam["hidPoint"].ToString();  //区域 
            string hidRank = string.Empty;
            if (null != queryParam["hidRank"])
            {
                hidRank = queryParam["hidRank"].ToString() == "请选择" ? "" : queryParam["hidRank"].ToString();  //隐患级别
            }
            string ischeck = queryParam["ischeck"].ToString();
            string checkType = "";
            if (queryJson.Contains("checkType"))
            {
                checkType = queryParam["checkType"].ToString();
            }

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
        #endregion

        #region 隐患部门数量对比图
        /// <summary>
        /// 隐患数量对比图
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QueryHidNumberComparison(string queryJson)
        {
            var queryParam = queryJson.ToJObject();

            string deptCode = queryParam["deptCode"].ToString();  //部门
            string year = queryParam["year"].ToString();  //年度
            string hidPoint = queryJson.Contains("hidPoint") ? queryParam["hidPoint"].ToString() : "";  //区域 
            string hidRank = string.Empty;
            if (null != queryParam["hidRank"])
            {
                hidRank = queryParam["hidRank"].ToString() == "请选择" ? "" : queryParam["hidRank"].ToString();  //隐患级别
            }
            string ischeck = queryParam["ischeck"].ToString();
            string checkType = "";
            if (queryJson.Contains("checkType"))
            {
                checkType = queryParam["checkType"].ToString();
            }
            var curUser = new OperatorProvider().Current(); //当前用户

            string hidPlantLevel = dataitemdetailbll.GetItemValue("HidPlantLevel");

            string hidOrganize = dataitemdetailbll.GetItemValue("HidOrganize");

            string CompanyRole = hidPlantLevel + "," + hidOrganize;

            StatisticsEntity hentity = new StatisticsEntity();
            hentity.sDeptCode = deptCode;
            hentity.sYear = year;
            hentity.sArea = hidPoint;
            hentity.sHidRank = hidRank;
            hentity.sAction = "5";   //对比图分析
            hentity.isCheck = ischeck;
            hentity.sCheckType = checkType;

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

            //列表
            var dt = htbaseinfobll.QueryStatisticsByAction(hentity);

            //x 轴Title 
            List<string> title = new List<string>();

            IList<series> slist = new List<series>();
            List<int> oVal = new List<int>();
            List<int> iVal = new List<int>();
            series s1 = new series();
            s1.name = "一般隐患";
            series s2 = new series();
            s2.name = "重大隐患";
            //图表分析
            foreach (DataRow row in dt.Rows)
            {
                // x 轴Title
                title.Add(row["fullname"].ToString());
                //y 轴Value 
                oVal.Add(Convert.ToInt32(row["OrdinaryHid"].ToString()));
                iVal.Add(Convert.ToInt32(row["ImportanHid"].ToString()));
            }
            s1.data = oVal;
            s2.data = iVal;
            slist.Add(s1);
            slist.Add(s2);

            //结果集合
            var jsonData = new { tdata = dt, xdata = title, sdata = slist, iscompany = hentity.isCompany ? 1 : 0 };

            return Content(jsonData.ToJson());
        }
        #endregion

        #region 隐患区域数量对比图
        /// <summary>
        /// 隐患数量对比图
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QueryComparisonForDistrict(string queryJson)  
        {
            var queryParam = queryJson.ToJObject();

            string deptCode = queryParam["deptCode"].ToString();  //部门
            string year = queryParam["year"].ToString();  //年度
            string hidPoint = queryJson.Contains("hidPoint") ? queryParam["hidPoint"].ToString() : "";  //区域 
            string hidRank = string.Empty;
            if (null != queryParam["hidRank"])
            {
                hidRank = queryParam["hidRank"].ToString() == "请选择" ? "" : queryParam["hidRank"].ToString();  //隐患级别
            }
            string ischeck = queryParam["ischeck"].ToString();
            string checkType = "";
            if (queryJson.Contains("checkType"))
            {
                checkType = queryParam["checkType"].ToString();
            }
            var curUser = new OperatorProvider().Current(); //当前用户
            //判断是否是厂级用户或者是公司用户

            string hidPlantLevel = dataitemdetailbll.GetItemValue("HidPlantLevel");

            string hidOrganize = dataitemdetailbll.GetItemValue("HidOrganize");

            string CompanyRole = hidPlantLevel + "," + hidOrganize;


            StatisticsEntity hentity = new StatisticsEntity();
            hentity.sDeptCode = deptCode;
            hentity.sYear = year;
            hentity.sArea = hidPoint;
            hentity.sHidRank = hidRank;
            //hentity.sAction = "2";
            hentity.isCheck = ischeck;
            hentity.sCheckType = checkType;
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

            //隐患数量列表图
            hentity.sAction = "1";
            var dt = htbaseinfobll.QueryStatisticsByAction(hentity); 

            //x 轴Title 
            List<string> title = new List<string>();

            IList<series> slist = new List<series>();
            List<int> oVal = new List<int>();
            List<int> iVal = new List<int>();
            series s1 = new series();
            s1.name = "一般隐患";
            series s2 = new series();
            s2.name = "重大隐患";
            //图表分析
            foreach (DataRow row in dt.Rows)
            {
                // x 轴Title
                title.Add(row["hidpointname"].ToString());
                //y 轴Value 
                oVal.Add(Convert.ToInt32(row["OrdinaryHid"].ToString()));
                iVal.Add(Convert.ToInt32(row["ImportanHid"].ToString()));
            }
            s1.data = oVal;
            s2.data = iVal;
            slist.Add(s1);
            slist.Add(s2);

            //结果集合
            var jsonData = new { tdata = dt, xdata = title, sdata = slist };

            return Content(jsonData.ToJson());
        }
        #endregion

        #endregion

        #region 隐患整改情况统计

        #region 隐患整改情况统计图(柱图+列表)
        /// <summary>
        /// 隐患整改情况饼图
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QueryChangeSituationForMonth(string queryJson)
        {
            var queryParam = queryJson.ToJObject();

            string deptCode = queryParam["deptCode"].ToString();  //部门
            string year = queryParam["year"].ToString();  //年度
            string hidRank = queryParam["hidRank"].ToString() == "请选择" ? "" : queryParam["hidRank"].ToString();  //隐患级别
            string hidPoint = queryParam["hidPoint"].ToString();  //区域
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
        #endregion

        #region 隐患整改情况趋势图
        /// <summary>
        /// 隐患整改情况趋势图
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QueryChangeSituationTendency(string queryJson)
        {
            var queryParam = queryJson.ToJObject();

            string deptCode = queryParam["deptCode"].ToString();  //部门
            string year = queryParam["year"].ToString();  //年度
            string hidPoint = queryParam["hidPoint"].ToString();  //区域 
            //string hidRank = queryParam["hidRank"].ToString() == "请选择" ? "" : queryParam["hidRank"].ToString();  //隐患级别
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
        #endregion

        #region 隐患整改情况部门对比图(柱图+列表)
        /// <summary>
        /// 隐患整改情况对比图
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QueryChangeSituationComparision(string queryJson)
        {
            var queryParam = queryJson.ToJObject();

            string deptCode = queryParam["deptCode"].ToString();  //部门
            string year = queryParam["year"].ToString();  //年度
            string hidRank = queryParam["hidRank"].ToString() == "请选择" ? "" : queryParam["hidRank"].ToString();  //隐患级别

            var curUser = new OperatorProvider().Current(); //当前用户

            //判断是否是厂级用户或者是公司用户

            string hidPlantLevel = dataitemdetailbll.GetItemValue("HidPlantLevel");

            string hidOrganize = dataitemdetailbll.GetItemValue("HidOrganize");

            string CompanyRole = hidPlantLevel + "," + hidOrganize;


            StatisticsEntity hentity = new StatisticsEntity();
            hentity.sDeptCode = deptCode; //整改部门
            hentity.sYear = year; //年度
            hentity.sHidRank = hidRank; //隐患级别
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
            hentity.sAction = "8";   //对比图分析
            //列表
            var dt = htbaseinfobll.QueryStatisticsByAction(hentity);

            List<string> title = new List<string>();

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
                // x 轴Title
                title.Add(row["fullname"].ToString());
                //y 轴Value 
                oVal.Add(Convert.ToInt32(row["thenChange"].ToString()));
                iVal.Add(Convert.ToInt32(row["nonChange"].ToString()));
            }
            s1.data = oVal; //已整改
            s2.data = iVal; //未整改
            slist.Add(s1);
            slist.Add(s2);

            var jsonData = new
            {
                tdata = dt,
                xdata = title,
                sdata = slist,
                iscompany = hentity.isCompany ? 1 : 0
            };

            return Content(jsonData.ToJson());
        }
        #endregion

        #region 隐患整改情况区域对比图(柱图+列表)
        /// <summary>
        /// 隐患整改情况区域对比图
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QueryChangeComparisionForDistrict(string queryJson) 
        {
            var queryParam = queryJson.ToJObject();

            string deptCode = queryParam["deptCode"].ToString();  //部门
            string year = queryParam["year"].ToString();  //年度
            string hidRank = queryParam["hidRank"].ToString() == "请选择" ? "" : queryParam["hidRank"].ToString();  //隐患级别
            StatisticsEntity hentity = new StatisticsEntity();
            hentity.sDeptCode = deptCode; //整改部门
            hentity.sYear = year; //年度
            hentity.sHidRank = hidRank; //隐患级别
            var curUser = new OperatorProvider().Current(); //当前用户
            hentity.sOrganize = curUser.OrganizeId;
            hentity.sAction = "10";   //对比图分析
            //列表
            var dt = htbaseinfobll.QueryStatisticsByAction(hentity);

            List<string> title = new List<string>();

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
                // x 轴Title
                title.Add(row["hidpointname"].ToString());
                //y 轴Value 
                oVal.Add(Convert.ToInt32(row["thenChange"].ToString()));
                iVal.Add(Convert.ToInt32(row["nonChange"].ToString()));
            }
            s1.data = oVal; //已整改
            s2.data = iVal; //未整改
            slist.Add(s1);
            slist.Add(s2);

            var jsonData = new { tdata = dt, xdata = title, sdata = slist };

            return Content(jsonData.ToJson());
        }
        #endregion

        #endregion

        #region 安全检查下的隐患统计

        #region 获取安全检查下的对比图
        /// <summary>
        /// 获取安全检查下的对比图
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QueryHidNumberColumn(string queryJson)
        {
            var queryParam = queryJson.ToJObject();

            string deptCode = queryParam["deptCode"].ToString();  //部门
            string year = queryParam["year"].ToString();  //年度
            string hidPoint = queryParam["hidPoint"].ToString();  //区域 
            string hidRank = queryParam["hidRank"].ToString() == "请选择" ? "" : queryParam["hidRank"].ToString();  //隐患级别
            string ischeck = queryParam["ischeck"].ToString();
            string checkType = "";
            if (queryJson.Contains("checkType"))
            {
                checkType = queryParam["checkType"].ToString();
            }

            StatisticsEntity hentity = new StatisticsEntity();
            hentity.sDeptCode = deptCode;
            hentity.sYear = year;
            hentity.sArea = hidPoint;
            hentity.sHidRank = hidRank;
            hentity.sAction = "9";
            hentity.isCheck = ischeck;
            hentity.sCheckType = checkType;

            var dt = htbaseinfobll.QueryStatisticsByAction(hentity);

            IList<series> slist = new List<series>();
            List<int> oVal = new List<int>();
            List<int> iVal = new List<int>();
            series s1 = new series();
            s1.name = "一般隐患";
            series s2 = new series();
            s2.name = "重大隐患";
            //图表分析
            foreach (DataRow row in dt.Rows)
            {
                //y 轴Value 
                oVal.Add(Convert.ToInt32(row["OrdinaryHid"].ToString()));
                iVal.Add(Convert.ToInt32(row["ImportanHid"].ToString()));
            }
            s1.data = oVal;
            s2.data = iVal;
            slist.Add(s1);
            slist.Add(s2);

            var jsonData = new
            {
                tdata = dt,
                sdata = slist
            };
            return Content(jsonData.ToJson());
        }
        #endregion

        #region 安全检查下的隐患统计
        /// <summary>
        /// 获取隐患数量趋势图
        /// </summary>
        /// <returns></returns>
        public ActionResult QueryCheckHidTendency(string queryJson)
        {
            var queryParam = queryJson.ToJObject();

            string deptCode = queryParam["deptCode"].ToString();  //部门
            string year = queryParam["year"].ToString();  //年度
            string hidPoint = queryParam["hidPoint"].ToString();  //区域 
            string hidRank = queryParam["hidRank"].ToString() == "请选择" ? "" : queryParam["hidRank"].ToString();  //隐患级别
            string ischeck = queryParam["ischeck"].ToString();
            string checkType = "";
            if (queryJson.Contains("checkType"))
            {
                checkType = queryParam["checkType"].ToString();
            }

            StatisticsEntity hentity = new StatisticsEntity();
            hentity.sDeptCode = deptCode;
            hentity.sYear = year;
            hentity.sArea = hidPoint;
            hentity.sHidRank = hidRank;
            hentity.sAction = "9";
            hentity.isCheck = ischeck;
            hentity.sCheckType = checkType;

            var list = htbaseinfobll.QueryStatisticsByAction(hentity);

            hentity.sAction = "4";

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

            var jsonData = new
            {
                tdata = list,
                sdata = slist
            };
            return Content(jsonData.ToJson());
        }
        #endregion

        #endregion


        #region 首页统计

        #region 工作指标  
        /// <summary>
        /// 工作指标
        /// </summary>
        /// <returns></returns>
        public ActionResult QueryHomeHidWorkIndex(string orgCode = "", string orgId = "")
        {

            Operator curUser = new OperatorProvider().Current();
            if (curUser.RoleName.Contains("集团用户"))
            {
                if (!string.IsNullOrEmpty(orgCode))
                {
                    curUser = new Operator
                    {
                        OrganizeId = orgId,
                        OrganizeCode = orgCode,
                        RoleName = "公司级用户,公司领导"
                    };
                }
                else
                {
                    curUser = new Operator
                    {
                        OrganizeId = orgId,
                        OrganizeCode = "00",
                        DeptCode="00",
                        RoleName = "公司级用户,公司领导"
                    };
                }
            }

            string hidPlantLevel = dataitemdetailbll.GetItemValue("HidPlantLevel");

            string hidOrganize = dataitemdetailbll.GetItemValue("HidOrganize");

            string CompanyRole = hidPlantLevel + "," + hidOrganize;

            var userList = userbll.GetUserListByDeptCode(curUser.DeptCode, CompanyRole, false, curUser.OrganizeId).Where(p => p.UserId == curUser.UserId).ToList();

            string deptcode = "";
            //当前用户是公司级及厂级用户
            if (userList.Count() > 0)
            {
                deptcode = curUser.OrganizeCode;
            }
            else
            {
                deptcode = curUser.DeptCode;
            }

            var data = htbaseinfobll.QueryHidWorkList(deptcode, curUser.OrganizeCode);

            return Content(data.ToJson());
        }

        #endregion

        #region 隐患待办事项
        /// <summary>
        /// 隐患待办事项
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public ActionResult QueryHomeHidBacklog(string mode) 
        {
            Operator curUser = new OperatorProvider().Current();

            var data = htbaseinfobll.QueryHidBacklogRecord(mode, curUser.UserId);

            return Content(data.ToJson());
        }

        #endregion

        #region 隐患曝光治理
        public ActionResult QueryExposureHid(string num)
        {
            Operator curUser = new OperatorProvider().Current();

            var data = htbaseinfobll.QueryExposureHid(curUser.OrganizeCode, num);

            return Content(data.ToJson());
        }
        #endregion
    
        #endregion

    }

    public class TimeEntity
    {
        public string id { get; set; }
        public string text { get; set; }
    }

    public class series
    {
        public string name { get; set; }
        public List<int> data { get; set; }
    }

    public class fseries
    {
        public string name { get; set; }
        public List<decimal> data { get; set; }
    }
}
