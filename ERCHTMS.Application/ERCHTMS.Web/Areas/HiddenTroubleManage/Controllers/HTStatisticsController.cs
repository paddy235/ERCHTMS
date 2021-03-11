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
using System.IO;
using Svg;
using System.Drawing.Imaging;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Text;
using Svg.Transforms;
using BSFramework.Util.Offices;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.SaftyCheck;
using Newtonsoft.Json;

namespace ERCHTMS.Web.Areas.HiddenTroubleManage.Controllers
{

    /// <summary>
    /// 隐患统计
    /// </summary>
    public class HTStatisticsController : MvcControllerBase
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
        /// <summary>
        /// 省公司安全检查隐患统计
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GrpCheckIndex()
        {
            return View();
        }
        [HttpGet]
        public ActionResult SJIndex()
        {
            return View();
        }
        [HttpGet]
        public ActionResult SJTrendIndex()
        {
            return View();
        }

        #region 获取年份
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

        #region 隐患数量统计(所有)

        #region 获取统计信息(厂级)
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
            string statType = queryJson.Contains("statType") ? queryParam["statType"].ToString() : "";  //统计类型 
            string hidRank = string.Empty;
            if (null != queryParam["hidRank"])
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
            StatisticsEntity hentity = new StatisticsEntity();
            hentity.sDeptCode = deptCode;
            hentity.sArea = hidPoint;
            hentity.sHidRank = hidRank;
            hentity.statType = statType;
            hentity.sYear = year;
            hentity.sAction = "2";
            hentity.isCheck = ischeck;
            hentity.sCheckType = checkType;
            hentity.sOrganize = curUser.OrganizeId;

            //当前用户是厂级
            if (curUser.RoleName.Contains("厂级") || curUser.RoleName.Contains("公司级"))
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

        #region 获取隐患数量趋势图(厂级)
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
            string statType = queryJson.Contains("statType") ? queryParam["statType"].ToString() : "";  //统计类型 
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
            hentity.statType = statType;
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

        #region 隐患部门数量对比图(厂级)
        /// <summary>
        /// 隐患数量对比图
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QueryHidNumberComparison(string queryJson)
        {
            var queryParam = queryJson.ToJObject();

            string deptCode = queryParam["deptCode"].ToString();  //部门
            string year = queryJson.Contains("year") ? queryParam["year"].ToString() : "";  //年度
            string startDate = queryJson.Contains("startDate") ? queryParam["startDate"].ToString() : "";  //起始日期
            string endDate = queryJson.Contains("endDate") ? queryParam["endDate"].ToString() : "";  //截止日期
            string hidPoint = queryJson.Contains("hidPoint") ? queryParam["hidPoint"].ToString() : "";  //区域 
            string statType = queryJson.Contains("statType") ? queryParam["statType"].ToString() : "";  //统计类型 
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

            StatisticsEntity hentity = new StatisticsEntity();
            hentity.sDeptCode = deptCode;
            hentity.sYear = year;
            hentity.startDate = startDate;
            hentity.endDate = endDate;
            hentity.sArea = hidPoint;
            hentity.sHidRank = hidRank;
            hentity.statType = statType;
            hentity.sAction = "5";   //对比图分析
            hentity.isCheck = ischeck;
            hentity.sCheckType = checkType;
            hentity.sMark = 0; //电厂层级或部门
            //当前用户是厂级
            if (curUser.RoleName.Contains("厂级") || curUser.RoleName.Contains("公司级"))
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
            List<dseries> xdata = new List<dseries>();

                //x 轴Title 
            List<dseries> sdata = new List<dseries>(); 
            //一般隐患
            List<dseries_child> yblist = new List<dseries_child>();
            //重大隐患
            List<dseries_child> zdlist = new List<dseries_child>();

            dseries s1 = new dseries();
            s1.name = "一般隐患";
            s1.id = "ybyh";
            dseries s2 = new dseries();
            s2.name = "重大隐患";
            s2.id = "zdyh";
            //图表分析
            foreach (DataRow row in dt.Rows)
            {
                string dname = row["fullname"].ToString();
                string drillId = row["createuserdeptcode"].ToString();
                //一般隐患
                dseries_child ybyh = new dseries_child();
                ybyh.name = dname;
                ybyh.y = Convert.ToInt32(row["OrdinaryHid"].ToString());
                ybyh.drilldown = "yb" + drillId;//部门编码
                yblist.Add(ybyh);

                //重大隐患
                dseries_child zdyh = new dseries_child();
                zdyh.name = row["fullname"].ToString();
                zdyh.y = Convert.ToInt32(row["ImportanHid"].ToString());
                zdyh.drilldown = "zd" + drillId;//部门编码
                zdlist.Add(zdyh);

                //获取一般隐患单位下的各部门机构的数据
                List<dseries_child> cyblist = new List<dseries_child>();
                List<dseries_child> czdlist = new List<dseries_child>();  
                hentity.sDeptCode = row["createuserdeptcode"].ToString();
                hentity.sHidRank = "一般隐患,重大隐患";
                hentity.sMark = 1; //电厂层级或部门
                var yhdt = htbaseinfobll.QueryStatisticsByAction(hentity);
                foreach (DataRow crow in yhdt.Rows) 
                {
                    //一般隐患子项目
                    dseries_child cybmodel = new dseries_child();
                    cybmodel.name = crow["fullname"].ToString();
                    cybmodel.y = Convert.ToInt32(crow["OrdinaryHid"].ToString());
                    cybmodel.drilldown = "next_yb_" + crow["createuserdeptcode"].ToString(); ;//部门编码
                    cyblist.Add(cybmodel);

                    //重大隐患子项目
                    dseries_child czdmodel = new dseries_child();
                    czdmodel.name = crow["fullname"].ToString();
                    czdmodel.y = Convert.ToInt32(crow["ImportanHid"].ToString());
                    czdmodel.drilldown = "next_zd_" + crow["createuserdeptcode"].ToString(); ;//部门编码
                    czdlist.Add(czdmodel);
                }
                //一般隐患子项目
                dseries cybdseries = new dseries();
                cybdseries.name ="一般隐患";
                cybdseries.id = "yb" + drillId;
                cybdseries.data = cyblist;
                sdata.Add(cybdseries);


                //重大隐患子项目
                dseries czddseries = new dseries();
                czddseries.name = "重大隐患";
                czddseries.id = "zd" + drillId;
                czddseries.data = czdlist;
                sdata.Add(czddseries);
            }
            s1.data = yblist; //一般隐患
            xdata.Add(s1);
            s2.data = zdlist; //重大隐患
            xdata.Add(s2);
            //结果集合
            var jsonData = new { tdata = dt, xdata = xdata, sdata = sdata, iscompany = hentity.isCompany ? 1 : 0 };

            return Content(jsonData.ToJson());
        }


        public ActionResult QueryHidNUmberComparisonList(string queryJson) 
        {
            var queryParam = queryJson.ToJObject();

            string deptCode = queryParam["deptCode"].ToString();  //部门
            string year = queryJson.Contains("year") ? queryParam["year"].ToString() : "";  //年度
            string startDate = queryJson.Contains("startDate") ? queryParam["startDate"].ToString() : "";  //起始日期
            string endDate = queryJson.Contains("endDate") ? queryParam["endDate"].ToString() : "";  //截止日期
            string hidPoint = queryJson.Contains("hidPoint") ? queryParam["hidPoint"].ToString() : "";  //区域 
            string statType = queryJson.Contains("statType") ? queryParam["statType"].ToString() : "";  //统计类型 
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

            StatisticsEntity hentity = new StatisticsEntity();
            hentity.sDeptCode = deptCode;
            hentity.sYear = year;
            hentity.startDate = startDate;
            hentity.endDate = endDate;
            hentity.sArea = hidPoint;
            hentity.sHidRank = hidRank;
            hentity.statType = statType;
            hentity.sAction = "5";   //对比图分析
            hentity.isCheck = ischeck;
            hentity.sCheckType = checkType;
            hentity.sMark = 2; //电厂层级或部门
            //当前用户是厂级
            if (curUser.RoleName.Contains("厂级") || curUser.RoleName.Contains("公司级"))
            {
                hentity.isCompany = true;
            }
            else
            {
                hentity.isCompany = false;
            }
            var treeList = new List<TreeGridEntity>();
            //列表
            var dt = htbaseinfobll.QueryStatisticsByAction(hentity);

            foreach (DataRow row in dt.Rows)
            {
                TreeListForHidden tentity = new TreeListForHidden();
                tentity.createuserdeptcode = row["createuserdeptcode"].ToString();
                tentity.fullname = row["fullname"].ToString();
                tentity.sortcode = row["sortcode"].ToString();
                tentity.departmentid = row["departmentid"].ToString();
                if (row["parentid"].ToString() != "0") 
                {
                    tentity.parent = row["parentid"].ToString();
                }
                tentity.importanhid = Convert.ToDecimal(row["importanhid"].ToString());
                tentity.ordinaryhid = Convert.ToDecimal(row["ordinaryhid"].ToString());
                tentity.total = Convert.ToDecimal(row["total"].ToString());
                TreeGridEntity tree = new TreeGridEntity();
                bool hasChildren = dt.Select(string.Format(" parentid ='{0}'", tentity.departmentid)).Count() == 0 ? false : true;
                tentity.haschild = hasChildren;
                tree.id = row["departmentid"].ToString();
                tree.parentId = row["parentid"].ToString();
                string itemJson = tentity.ToJson();
                tree.entityJson = itemJson;
                tree.expanded = false;
                tree.hasChildren = hasChildren;
                treeList.Add(tree);
            }

            //结果集合
            return Content(treeList.TreeJson("0"));
        }


        /// <summary>
        /// 省公司隐患数量对比图
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QueryGrpHidNumberComparison(string queryJson)
        {
            var queryParam = queryJson.ToJObject();

            string deptCode = queryParam["deptCode"].ToString();  //部门
            string year = queryParam["year"].ToString();  //年度
            string hidPoint = queryJson.Contains("hidPoint") ? queryParam["hidPoint"].ToString() : "";  //区域 
            string statType = queryJson.Contains("statType") ? queryParam["statType"].ToString() : "";  //统计类型 
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

            StatisticsEntity hentity = new StatisticsEntity();
            hentity.sDeptCode = deptCode;
            hentity.sYear = year;
            hentity.sArea = hidPoint;
            hentity.sHidRank = hidRank;
            hentity.statType = statType;
            hentity.sAction = "12";   //对比图分析
            hentity.isCheck = ischeck;
            hentity.sCheckType = checkType;

            //当前用户是厂级
            if (curUser.RoleName.Contains("厂级") || curUser.RoleName.Contains("公司级"))
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

        #region 隐患区域数量对比图(厂级)
        /// <summary>
        /// 隐患数量对比图
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QueryComparisonForDistrict(string queryJson)
        {
            var queryParam = queryJson.ToJObject();

            string deptCode = queryParam["deptCode"].ToString();  //部门
            string year = queryJson.Contains("year") ? queryParam["year"].ToString() : "";  //年度
            string startDate = queryJson.Contains("startDate") ? queryParam["startDate"].ToString() : "";  //起始日期
            string endDate = queryJson.Contains("endDate") ? queryParam["endDate"].ToString() : "";  //截止日期
            string hidPoint = queryJson.Contains("hidPoint") ? queryParam["hidPoint"].ToString() : "";  //区域 
            string statType = queryJson.Contains("statType") ? queryParam["statType"].ToString() : "";  //统计类型 
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
            StatisticsEntity hentity = new StatisticsEntity();
            hentity.sDeptCode = deptCode;
            hentity.sYear = year;
            hentity.startDate = startDate;
            hentity.endDate = endDate;
            hentity.sArea = hidPoint;
            hentity.sHidRank = hidRank;
            hentity.statType = statType;
            //hentity.sAction = "2";
            hentity.isCheck = ischeck;
            hentity.sCheckType = checkType;
            hentity.sOrganize = curUser.OrganizeId;

            //当前用户是厂级
            if (curUser.RoleName.Contains("厂级") || curUser.RoleName.Contains("公司级"))
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


        #region 获取隐患信息(省级)
        /// <summary>
        /// 获取隐患信息
        /// </summary>
        /// <returns></returns>
        public ActionResult QueryProvHidNumberPie(string queryJson)
        {
            var queryParam = queryJson.ToJObject();

            string deptid = queryParam["deptid"].ToString();  //部门 
            string startdate = queryParam["startdate"].ToString();  //起始日期 
            string enddate = queryParam["enddate"].ToString();//截止日期
            string hidPoint = queryJson.Contains("hidPoint") ? queryParam["hidPoint"].ToString() : "";  //区域 
            string year = queryJson.Contains("year") ? queryParam["year"].ToString() : "";  //年度
            string hidRank = string.Empty;
            if (null != queryParam["hidRank"])
            {
                hidRank = queryParam["hidRank"].ToString() == "请选择" ? "" : queryParam["hidRank"].ToString();  //隐患级别
            }
            var curUser = new OperatorProvider().Current(); //当前用户

            //判断是否是厂级用户或者是公司用户
            ProvStatisticsEntity hentity = new ProvStatisticsEntity();
            hentity.sDepartId = deptid;
            hentity.sArea = hidPoint;
            hentity.sHidRank = hidRank;
            hentity.sStartDate = startdate;
            hentity.sEndDate = enddate;
            hentity.sYear = year;
            hentity.sAction = "2";
            //隐患等级统计图
            var hidrank = htbaseinfobll.QueryProvStatisticsByAction(hentity);
            //隐患等级统计图
            hentity.sAction = "3";
            var area = htbaseinfobll.QueryProvStatisticsByAction(hentity);
            //隐患数量列表图
            hentity.sAction = "1";
            var list = htbaseinfobll.QueryProvStatisticsByAction(hentity);
            var jsonData = new
            {
                hiddata = hidrank,
                areadata = area,
                tdata = list
            };
            return Content(jsonData.ToJson());
        }

        #endregion

        #region 获取隐患数量趋势图(省级)
        /// <summary>
        /// 获取隐患数量趋势图
        /// </summary>
        /// <returns></returns>
        public ActionResult QueryProvHidNumberTendency(string queryJson)
        {
            var queryParam = queryJson.ToJObject();

            string deptid = queryParam["deptid"].ToString();  //部门 
            string startdate = queryParam["startdate"].ToString();  //起始日期 
            string enddate = queryParam["enddate"].ToString();//截止日期
            string year = queryJson.Contains("year") ? queryParam["year"].ToString() : "";  //年度
            string hidPoint = queryJson.Contains("hidPoint") ? queryParam["hidPoint"].ToString() : "";  //区域 
            string hidRank = string.Empty;
            if (null != queryParam["hidRank"])
            {
                hidRank = queryParam["hidRank"].ToString() == "请选择" ? "" : queryParam["hidRank"].ToString();  //隐患级别
            }
            var curUser = new OperatorProvider().Current(); //当前用户

            ProvStatisticsEntity hentity = new ProvStatisticsEntity();
            hentity.sDepartId = deptid;
            hentity.sArea = hidPoint;
            hentity.sHidRank = hidRank;
            hentity.sStartDate = startdate;
            hentity.sEndDate = enddate;
            hentity.sYear = year;
            hentity.sAction = "4";

            var dt = htbaseinfobll.QueryProvStatisticsByAction(hentity);

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

        #region 隐患(部门)数量对比图(省级)
        /// <summary>
        /// 隐患(部门)数量对比图(省级)
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QueryProvHidNumberComparison(string queryJson)
        {
            var queryParam = queryJson.ToJObject();

            string deptid = queryParam["deptid"].ToString();  //部门 
            string startdate = queryParam["startdate"].ToString();  //起始日期 
            string enddate = queryParam["enddate"].ToString();//截止日期
            string year = queryJson.Contains("year") ? queryParam["year"].ToString() : "";  //年度
            string hidPoint = queryJson.Contains("hidPoint") ? queryParam["hidPoint"].ToString() : "";  //区域 
            string hidRank = string.Empty;
            if (null != queryParam["hidRank"])
            {
                hidRank = queryParam["hidRank"].ToString() == "请选择" ? "" : queryParam["hidRank"].ToString();  //隐患级别
            }
            var curUser = new OperatorProvider().Current(); //当前用户

            ProvStatisticsEntity hentity = new ProvStatisticsEntity();
            hentity.sDepartId = deptid;
            hentity.sArea = hidPoint;
            hentity.sHidRank = hidRank;
            hentity.sStartDate = startdate;
            hentity.sEndDate = enddate;
            hentity.sYear = year;
            hentity.sAction = "5";   //对比图分析

            //列表
            var dt = htbaseinfobll.QueryProvStatisticsByAction(hentity);

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
            var jsonData = new { tdata = dt, xdata = title, sdata = slist };

            return Content(jsonData.ToJson());
        }
        #endregion

        #region 隐患区域数量对比图(省级)
        /// <summary>
        /// 隐患数量对比图
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QueryProvComparisonForDistrict(string queryJson)
        {
            var queryParam = queryJson.ToJObject();

            string deptid = queryParam["deptid"].ToString();  //部门 
            string startdate = queryParam["startdate"].ToString();  //起始日期 
            string enddate = queryParam["enddate"].ToString();//截止日期
            string year = queryJson.Contains("year") ? queryParam["year"].ToString() : "";  //年度
            string hidPoint = queryJson.Contains("hidPoint") ? queryParam["hidPoint"].ToString() : "";  //区域 
            string hidRank = string.Empty;
            if (null != queryParam["hidRank"])
            {
                hidRank = queryParam["hidRank"].ToString() == "请选择" ? "" : queryParam["hidRank"].ToString();  //隐患级别
            }
            var curUser = new OperatorProvider().Current(); //当前用户

            ProvStatisticsEntity hentity = new ProvStatisticsEntity();
            hentity.sDepartId = deptid;
            hentity.sArea = hidPoint;
            hentity.sHidRank = hidRank;
            hentity.sStartDate = startdate;
            hentity.sEndDate = enddate;
            hentity.sYear = year;

            //隐患数量列表图
            hentity.sAction = "1";
            var dt = htbaseinfobll.QueryProvStatisticsByAction(hentity);

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

        #region 隐患整改情况统计图(柱图+列表)(厂级)
        /// <summary>
        /// 隐患整改情况饼图
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QueryChangeSituationForMonth(string queryJson)
        {
            var queryParam = queryJson.ToJObject();

            string deptid = queryJson.Contains("deptid") ? queryParam["deptid"].ToString():"";  //部门 
            string deptCode = queryParam["deptCode"].ToString();  //部门
            //string startdate = queryParam["startdate"].ToString();  //起始日期 
            //string enddate = queryParam["enddate"].ToString();//截止日期
            string year = queryJson.Contains("year") ? queryParam["year"].ToString() : "";  //年度
            string hidPoint = queryJson.Contains("hidPoint") ? queryParam["hidPoint"].ToString() : "";  //区域 
            string hidRank = string.Empty;
            if (null != queryParam["hidRank"])
            {
                hidRank = queryParam["hidRank"].ToString() == "请选择" ? "" : queryParam["hidRank"].ToString();  //隐患级别
            }
            var curUser = new OperatorProvider().Current(); //当前用户

            ProvStatisticsEntity hentity = new ProvStatisticsEntity();
            hentity.sDepartId = deptid;
            hentity.sDepartCode = deptCode;
            hentity.sArea = hidPoint;
            hentity.sHidRank = hidRank;
            //hentity.sStartDate = startdate;
            //hentity.sEndDate = enddate;
            hentity.sYear = year;

            hentity.sAction = "6";   //对比图分析
            //列表
            var dt = htbaseinfobll.QueryProvStatisticsByAction(hentity);

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

        #region 隐患整改情况趋势图(厂级)
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

        /// <summary>
        /// 隐患整改情况趋势图(可门电厂专用)
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QueryChangeSituationTendencyKm(string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string deptCode = user.DeptCode;// queryParam["deptCode"].ToString();  //部门
            string year = DateTime.Now.Year.ToString();// queryParam["year"].ToString();  //年度
            string hidPoint = queryParam["hidPoint"].ToString();  //区域 
            //string hidRank = queryParam["hidRank"].ToString() == "请选择" ? "" : queryParam["hidRank"].ToString();  //隐患级别
            StatisticsEntity hentity = new StatisticsEntity();
            hentity.sDeptCode = deptCode;
            hentity.sYear = year;
            hentity.sArea = hidPoint;
            hentity.sAction = "7";

            var dt = htbaseinfobll.QueryStatisticsByAction(hentity);
            //IList<fseries> slist = new List<fseries>();
            List<object> dic = new List<object>();
            //fseries s1 = new fseries();
            //s1.name = "所有隐患";
            //fseries s2 = new fseries();
            //s2.name = "一般隐患";
            //fseries s3 = new fseries();
            //s3.name = "重大隐患";
            List<decimal> list1 = new List<decimal>();
            List<decimal> list2 = new List<decimal>();
            List<decimal> list3 = new List<decimal>();
            foreach (DataRow row in dt.Rows)
            {
                decimal total = Convert.ToDecimal(row["aChangeVal"].ToString()); //总的整改率
                decimal ordinary = Convert.ToDecimal(row["aValue"].ToString()); //发现隐患数
                decimal great = Convert.ToDecimal(row["yValue"].ToString()); //已整改隐患数量
                decimal wbhnum = ordinary - great;//未闭合隐患数

                list1.Add(total);
                list2.Add(ordinary);
                list3.Add(wbhnum);
            }
            //s1.data = list1;
            //s2.data = list2;
            //s3.data = list3;
            //slist.Add(s1);
            //slist.Add(s2);
            //slist.Add(s3);
            string CheckNum = new SaftyCheckDataRecordBLL().getMonthCheckCount("", DateTime.Now.Year.ToString(), "", "");
            checkentity entity = JsonConvert.DeserializeObject<checkentity>(CheckNum);//安全检查次数

            dic.Add(new { name = "隐患整改率%", type = "spline", yAxis = 0, data = list1 });
            dic.Add(new { name = "安全检查次数", type = "spline", yAxis = 1, data = entity.x});
            dic.Add(new { name = "发现隐患数", type = "spline", yAxis = 1, data = list2 });
            dic.Add(new { name = "未闭合隐患数", type = "spline", yAxis = 1, data = list3 });

            return Content(dic.ToJson());
        }

        #endregion

        #region 隐患整改情况部门对比图(柱图+列表)(厂级)
        /// <summary>
        /// 隐患整改情况对比图
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QueryChangeSituationComparision(string queryJson)
        {
            var queryParam = queryJson.ToJObject();

            string deptCode = queryParam["deptCode"].ToString();  //部门
            string year = queryJson.Contains("year") ? queryParam["year"].ToString() : "";  //年度
            string hidRank = queryParam["hidRank"].ToString() == "请选择" ? "" : queryParam["hidRank"].ToString();  //隐患级别
            string startDate = queryJson.Contains("startDate") ? queryParam["startDate"].ToString() : "";  //起始日期
            string endDate = queryJson.Contains("endDate") ? queryParam["endDate"].ToString() : "";  //截止日期
            var curUser = new OperatorProvider().Current(); //当前用户

            //判断是否是厂级用户或者是公司用户
            StatisticsEntity hentity = new StatisticsEntity();
            hentity.sDeptCode = deptCode; //整改部门
            hentity.sYear = year; //年度
            hentity.startDate = startDate;
            hentity.endDate = endDate;
            hentity.sHidRank = hidRank; //隐患级别

            //当前用户是厂级
            if (curUser.RoleName.Contains("厂级") || curUser.RoleName.Contains("公司级"))
            {
                hentity.isCompany = true;
            }
            else
            {
                hentity.isCompany = false;
            }
            hentity.sAction = "8";   //对比图分析
            hentity.sMark = 0; //电厂层级或部门
            //列表
            var dt = htbaseinfobll.QueryStatisticsByAction(hentity);

            //x 轴Title 
            List<dseries> xdata = new List<dseries>();

            //x 轴Title 
            List<dseries> sdata = new List<dseries>();
            //已整改
            List<dseries_child> yzglist = new List<dseries_child>();
            //未整改
            List<dseries_child> wzglist = new List<dseries_child>(); 

            dseries s1 = new dseries();
            s1.name = "已整改";
            s1.id = "yzg";
            dseries s2 = new dseries();
            s2.name = "未整改";
            s2.id = "wzg";
            //图表分析
            foreach (DataRow row in dt.Rows)
            {
                string dname = row["fullname"].ToString();
                string drillId = row["changedutydepartcode"].ToString();
                //已整改
                dseries_child yzg = new dseries_child();
                yzg.name = dname;
                yzg.y = Convert.ToInt32(row["thenChange"].ToString());
                yzg.drilldown = "yzg" + drillId;//部门编码
                yzglist.Add(yzg);

                //未整改
                dseries_child wzg = new dseries_child();
                wzg.name = row["fullname"].ToString();
                wzg.y = Convert.ToInt32(row["nonChange"].ToString());
                wzg.drilldown = "wzg" + drillId;//部门编码
                wzglist.Add(wzg);

                //获取一般隐患单位下的各部门机构的数据
                List<dseries_child> cyblist = new List<dseries_child>();
                List<dseries_child> czdlist = new List<dseries_child>();
                hentity.sDeptCode = row["changedutydepartcode"].ToString();
                hentity.sMark = 1; //电厂层级或部门
                var yhdt = htbaseinfobll.QueryStatisticsByAction(hentity);
                foreach (DataRow crow in yhdt.Rows)
                {
                    //已整改
                    dseries_child cybmodel = new dseries_child();
                    cybmodel.name = crow["fullname"].ToString();
                    cybmodel.y = Convert.ToInt32(crow["thenChange"].ToString());
                    cybmodel.drilldown = "next_yzg_" + crow["changedutydepartcode"].ToString(); ;//部门编码
                    cyblist.Add(cybmodel);

                    //未整改
                    dseries_child czdmodel = new dseries_child();
                    czdmodel.name = crow["fullname"].ToString();
                    czdmodel.y = Convert.ToInt32(crow["nonChange"].ToString());
                    czdmodel.drilldown = "next_wzg_" + crow["changedutydepartcode"].ToString(); ;//部门编码
                    czdlist.Add(czdmodel);
                }
                //已整改隐患项目
                dseries cybdseries = new dseries();
                cybdseries.name = "已整改";
                cybdseries.id = "yzg" + drillId;
                cybdseries.data = cyblist;
                sdata.Add(cybdseries);

                //未整改隐患项目
                dseries czddseries = new dseries();
                czddseries.name = "未整改";
                czddseries.id = "wzg" + drillId;
                czddseries.data = czdlist;
                sdata.Add(czddseries);
            }
            s1.data = yzglist; //已整改
            xdata.Add(s1);
            s2.data = wzglist; //未整改
            xdata.Add(s2);
            //结果集合
            var jsonData = new { tdata = dt, xdata = xdata, sdata = sdata, iscompany = hentity.isCompany ? 1 : 0 };

            return Content(jsonData.ToJson());
        }
        #endregion


        #region 隐患整改情况部门对比图(柱图+列表)(厂级)
        /// <summary>
        /// 隐患整改情况对比图
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QueryChangeSituationComparisionList(string queryJson) 
        {
            var queryParam = queryJson.ToJObject();

            string deptCode = queryParam["deptCode"].ToString();  //部门
            string year = queryJson.Contains("year") ? queryParam["year"].ToString() : "";  //年度
            string hidRank = queryParam["hidRank"].ToString() == "请选择" ? "" : queryParam["hidRank"].ToString();  //隐患级别
            string startDate = queryJson.Contains("startDate") ? queryParam["startDate"].ToString() : "";  //起始日期
            string endDate = queryJson.Contains("endDate") ? queryParam["endDate"].ToString() : "";  //截止日期
            var curUser = new OperatorProvider().Current(); //当前用户

            //判断是否是厂级用户或者是公司用户
            StatisticsEntity hentity = new StatisticsEntity();
            hentity.sDeptCode = deptCode; //整改部门
            hentity.sYear = year; //年度
            hentity.startDate = startDate;
            hentity.endDate = endDate;
            hentity.sHidRank = hidRank; //隐患级别

            //当前用户是厂级
            if (curUser.RoleName.Contains("厂级") || curUser.RoleName.Contains("公司级"))
            {
                hentity.isCompany = true;
            }
            else
            {
                hentity.isCompany = false;
            }
            hentity.sAction = "8";   //对比图分析
            hentity.sMark = 2; //电厂层级或部门
            var treeList = new List<TreeGridEntity>();
            //列表
            var dt = htbaseinfobll.QueryStatisticsByAction(hentity);

            foreach (DataRow row in dt.Rows)
            {
                TreeListForHiddenSituation tentity = new TreeListForHiddenSituation();
                tentity.changedutydepartcode = row["changedutydepartcode"].ToString();
                tentity.fullname = row["fullname"].ToString();
                tentity.sortcode = row["sortcode"].ToString();
                tentity.departmentid = row["departmentid"].ToString();
                if (row["parentid"].ToString() != "0")
                {
                    tentity.parent = row["parentid"].ToString();
                }
                tentity.thenchange = Convert.ToDecimal(row["thenchange"].ToString());
                tentity.nonchange = Convert.ToDecimal(row["nonchange"].ToString());
                tentity.total = Convert.ToDecimal(row["total"].ToString());
                TreeGridEntity tree = new TreeGridEntity();
                bool hasChildren = dt.Select(string.Format(" parentid ='{0}'", tentity.departmentid)).Count() == 0 ? false : true;
                tentity.haschild = hasChildren;
                tree.id = row["departmentid"].ToString();
                tree.parentId = row["parentid"].ToString();
                string itemJson = tentity.ToJson();
                tree.entityJson = itemJson;
                tree.expanded = false;
                tree.hasChildren = hasChildren;
                treeList.Add(tree);
            }

            //结果集合
            return Content(treeList.TreeJson("0"));
        }
        #endregion

        #region 隐患整改情况区域对比图(柱图+列表)(厂级)
        /// <summary>
        /// 隐患整改情况区域对比图
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QueryChangeComparisionForDistrict(string queryJson)
        {
            var queryParam = queryJson.ToJObject();

            string deptCode = queryParam["deptCode"].ToString();  //部门
            string year = queryJson.Contains("year") ? queryParam["year"].ToString() : "";  //年度
            string startDate = queryJson.Contains("startDate") ? queryParam["startDate"].ToString() : "";  //起始日期
            string endDate = queryJson.Contains("endDate") ? queryParam["endDate"].ToString() : "";  //截止日期
            string hidRank = queryParam["hidRank"].ToString() == "请选择" ? "" : queryParam["hidRank"].ToString();  //隐患级别
            StatisticsEntity hentity = new StatisticsEntity();
            hentity.sDeptCode = deptCode; //整改部门
            hentity.sYear = year; //年度
            hentity.startDate = startDate;
            hentity.endDate = endDate; 
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


        #region 隐患整改情况统计图(柱图+列表)(省级)
        /// <summary>
        /// 隐患整改情况饼图
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QueryProvChangeSituationForMonth(string queryJson)
        {
            var queryParam = queryJson.ToJObject();

            string deptid = queryParam["deptid"].ToString();  //部门 
            string startdate = queryParam["startdate"].ToString();  //起始日期 
            string enddate = queryParam["enddate"].ToString();//截止日期
            string year = queryJson.Contains("year") ? queryParam["year"].ToString() : "";  //年度
            string hidPoint = queryJson.Contains("hidPoint") ? queryParam["hidPoint"].ToString() : "";  //区域 
            string hidRank = string.Empty;
            if (null != queryParam["hidRank"])
            {
                hidRank = queryParam["hidRank"].ToString() == "请选择" ? "" : queryParam["hidRank"].ToString();  //隐患级别
            }
            var curUser = new OperatorProvider().Current(); //当前用户

            ProvStatisticsEntity hentity = new ProvStatisticsEntity();
            hentity.sDepartId = deptid;
            hentity.sArea = hidPoint;
            hentity.sHidRank = hidRank;
            hentity.sStartDate = startdate;
            hentity.sEndDate = enddate;
            hentity.sYear = year;

            hentity.sAction = "6";   //对比图分析
            //列表
            var dt = htbaseinfobll.QueryProvStatisticsByAction(hentity);

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

        #region 隐患整改情况趋势图(省级)
        /// <summary>
        /// 隐患整改情况趋势图
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QueryProvChangeSituationTendency(string queryJson)
        {
            var queryParam = queryJson.ToJObject();

            string deptid = queryParam["deptid"].ToString();  //部门 
            string startdate = queryParam["startdate"].ToString();  //起始日期 
            string enddate = queryParam["enddate"].ToString();//截止日期
            string year = queryJson.Contains("year") ? queryParam["year"].ToString() : "";  //年度
            string hidPoint = queryJson.Contains("hidPoint") ? queryParam["hidPoint"].ToString() : "";  //区域 
            string hidRank = string.Empty;
            if (null != queryParam["hidRank"])
            {
                hidRank = queryParam["hidRank"].ToString() == "请选择" ? "" : queryParam["hidRank"].ToString();  //隐患级别
            }
            var curUser = new OperatorProvider().Current(); //当前用户

            ProvStatisticsEntity hentity = new ProvStatisticsEntity();
            hentity.sDepartId = deptid;
            hentity.sArea = hidPoint;
            hentity.sHidRank = hidRank;
            hentity.sStartDate = startdate;
            hentity.sEndDate = enddate;
            hentity.sYear = year;
            hentity.sAction = "7";

            var dt = htbaseinfobll.QueryProvStatisticsByAction(hentity);

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

        #region 隐患整改情况部门对比图(柱图+列表)(省级)
        /// <summary>
        /// 隐患整改情况对比图
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QueryProvChangeSituationComparision(string queryJson)
        {
            var queryParam = queryJson.ToJObject();

            string deptid = queryParam["deptid"].ToString();  //部门 
            string startdate = queryParam["startdate"].ToString();  //起始日期 
            string enddate = queryParam["enddate"].ToString();//截止日期
            string year = queryJson.Contains("year") ? queryParam["year"].ToString() : "";  //年度
            string hidPoint = queryJson.Contains("hidPoint") ? queryParam["hidPoint"].ToString() : "";  //区域 
            string hidRank = string.Empty;
            if (null != queryParam["hidRank"])
            {
                hidRank = queryParam["hidRank"].ToString() == "请选择" ? "" : queryParam["hidRank"].ToString();  //隐患级别
            }
            var curUser = new OperatorProvider().Current(); //当前用户

            ProvStatisticsEntity hentity = new ProvStatisticsEntity();
            hentity.sDepartId = deptid;
            hentity.sArea = hidPoint;
            hentity.sHidRank = hidRank;
            hentity.sStartDate = startdate;
            hentity.sEndDate = enddate;
            hentity.sYear = year;
            hentity.sAction = "8";   //对比图分析
            //列表
            var dt = htbaseinfobll.QueryProvStatisticsByAction(hentity);

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
                sdata = slist
            };

            return Content(jsonData.ToJson());
        }
        #endregion

        #region 隐患整改情况区域对比图(柱图+列表)(省级)
        /// <summary>
        /// 隐患整改情况区域对比图
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QueryProvChangeComparisionForDistrict(string queryJson)
        {
            var queryParam = queryJson.ToJObject();

            string deptid = queryParam["deptid"].ToString();  //部门 
            string startdate = queryParam["startdate"].ToString();  //起始日期 
            string enddate = queryParam["enddate"].ToString();//截止日期
            string year = queryJson.Contains("year") ? queryParam["year"].ToString() : "";  //年度
            string hidPoint = queryJson.Contains("hidPoint") ? queryParam["hidPoint"].ToString() : "";  //区域 
            string hidRank = string.Empty;
            if (null != queryParam["hidRank"])
            {
                hidRank = queryParam["hidRank"].ToString() == "请选择" ? "" : queryParam["hidRank"].ToString();  //隐患级别
            }
            var curUser = new OperatorProvider().Current(); //当前用户

            ProvStatisticsEntity hentity = new ProvStatisticsEntity();
            hentity.sDepartId = deptid;
            hentity.sArea = hidPoint;
            hentity.sHidRank = hidRank;
            hentity.sStartDate = startdate;
            hentity.sEndDate = enddate;
            hentity.sYear = year;
            hentity.sAction = "10";   //对比图分析
            //列表
            var dt = htbaseinfobll.QueryProvStatisticsByAction(hentity);

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
        /// <summary>
        /// 获取安全检查下的对比图
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult QueryGrpHidNumberColumn(string queryJson)
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
            hentity.sAction = "11";
            hentity.isCheck = ischeck;
            hentity.sCheckType = checkType;
            hentity.sCType = queryParam["ctype"].ToString();

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
        /// <summary>
        /// 省公司获取隐患数量趋势图
        /// </summary>
        /// <returns></returns>
        public ActionResult QueryGrpCheckHidTendency(string queryJson)
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
            hentity.isCheck = ischeck;
            hentity.sCheckType = checkType;
            hentity.sCType = queryParam["ctype"].ToString();
            hentity.sAction = "11";
            var list = htbaseinfobll.QueryStatisticsByAction(hentity);
            hentity.sAction = "13";
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
                        DeptCode = orgCode,
                        RoleName = "公司级用户,公司领导"
                    };
                }
                else
                {
                    curUser = new Operator
                    {
                        OrganizeId = orgId,
                        OrganizeCode = "00",
                        DeptCode = "00",
                        RoleName = "公司级用户,公司领导"
                    };
                }
            }

            string hidPlantLevel = dataitemdetailbll.GetItemValue("HidPlantLevel");

            string hidOrganize = dataitemdetailbll.GetItemValue("HidOrganize");

            string CompanyRole = hidPlantLevel + "," + hidOrganize;

            var userList = userbll.GetUserListByDeptCode(curUser.DeptCode, CompanyRole, false, curUser.OrganizeId).Where(p => p.UserId == curUser.UserId).ToList();

            var data = htbaseinfobll.QueryHidWorkList(curUser);

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
        [HttpPost]
        public ActionResult QueryExposureHid(string num)
        {
            Operator curUser = new OperatorProvider().Current();

            var data = htbaseinfobll.QueryExposureHid(num);

            return Content(data.ToJson());
        }
        #endregion

        #endregion

        #region 导出图片
        //HighCharts 导出图片 svg
        //filename type width scale svg
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Export(FormCollection fc)
        {
            string tType = fc["type"];
            string tSvg = fc["svg"];
            string tFileName = fc["filename"];
            string tWidth = fc["width"];
            if (string.IsNullOrEmpty(tFileName))
            {
                tFileName = "DefaultChart";
            }
            MemoryStream tData = new MemoryStream(Encoding.UTF8.GetBytes(tSvg));
            Svg.SvgDocument tSvgObj = SvgDocument.Open<SvgDocument>(tData);
            tSvgObj.Transforms = new SvgTransformCollection();
            float scalar = (float)int.Parse(tWidth) / (float)tSvgObj.Width;
            tSvgObj.Transforms.Add(new SvgScale(scalar, scalar));
            tSvgObj.Width = new SvgUnit(tSvgObj.Width.Type, tSvgObj.Width * scalar);
            tSvgObj.Height = new SvgUnit(tSvgObj.Height.Type, tSvgObj.Height * scalar);
            MemoryStream tStream = new MemoryStream();
            string tTmp = new Random().Next().ToString();
            string tExt = "";

            switch (tType)
            {
                case "image/png":
                    tExt = "png";
                    break;
                case "image/jpeg":
                    tExt = "jpg";
                    break;
                case "application/pdf":
                    tExt = "pdf";
                    break;
                case "image/svg+xml":
                    tExt = "svg";
                    break;
            }

            // Svg.SvgDocument tSvgObj = SvgDocument.Open<SvgDocument>(tData);
            switch (tExt)
            {
                case "jpg":
                    tSvgObj.Draw().Save(tStream, ImageFormat.Jpeg);
                    break;
                case "png":
                    tSvgObj.Draw().Save(tStream, ImageFormat.Png);
                    break;
                case "pdf":
                    PdfWriter tWriter = null;
                    Document tDocumentPdf = null;
                    try
                    {
                        tSvgObj.Draw().Save(tStream, ImageFormat.Png);
                        tDocumentPdf = new Document(new iTextSharp.text.Rectangle((float)tSvgObj.Width, (float)tSvgObj.Height));
                        tDocumentPdf.SetMargins(0.0f, 0.0f, 0.0f, 0.0f);
                        iTextSharp.text.Image tGraph = iTextSharp.text.Image.GetInstance(tStream.ToArray());
                        tGraph.ScaleToFit((float)tSvgObj.Width, (float)tSvgObj.Height);

                        tStream = new MemoryStream();
                        tWriter = PdfWriter.GetInstance(tDocumentPdf, tStream);
                        tDocumentPdf.Open();
                        tDocumentPdf.NewPage();
                        tDocumentPdf.Add(tGraph);
                        tDocumentPdf.Close();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        tDocumentPdf.Close();
                        tWriter.Close();
                        tData.Dispose();
                        tData.Close();

                    }
                    break;
                case "svg":
                    tStream = tData;
                    break;
            }
            tFileName = tFileName + "." + tExt;
            return File(tStream.ToArray(), tType, tFileName);
        }

        #endregion

        #region 导出excel
        /// <summary>
        /// 导出excel
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public void ExportExcel(string queryJson)
        {
            DataTable dt = new DataTable();
            var queryParam = queryJson.ToJObject();
            int model = int.Parse(queryParam["model"].ToString()); //导出的类型
            string deptid = queryJson.Contains("deptid")? queryParam["deptid"].ToString():"";  //部门 
            string deptCode = queryJson.Contains("deptCode") ? queryParam["deptCode"].ToString() : "";  //部门编码
            string startdate = queryJson.Contains("startdate") ? queryParam["startdate"].ToString() : "";  //起始日期 
            string enddate = queryJson.Contains("enddate") ? queryParam["enddate"].ToString() : "";//截止日期
            string hidPoint = queryJson.Contains("hidPoint") ? queryParam["hidPoint"].ToString() : "";  //区域 
            string year = queryJson.Contains("year") ? queryParam["year"].ToString() : "";  //年度
            string statType = queryJson.Contains("statType") ? queryParam["statType"].ToString() : "";  //统计类型
            string hidRank = string.Empty;
            if (null != queryParam["hidRank"])
            {
                hidRank = queryParam["hidRank"].ToString() == "请选择" ? "" : queryParam["hidRank"].ToString();  //隐患级别
            }
            string mark = queryJson.Contains("mark") ? queryParam["mark"].ToString() : ""; //判定是省级还是厂级
            string ischeck = queryJson.Contains("ischeck") ? queryParam["ischeck"].ToString():""; //是否检查
            string checkType = queryJson.Contains("checkType") ?queryParam["checkType"].ToString():""; //检查类型
            var curUser = new OperatorProvider().Current(); //当前用户

            ////设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            ////每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            ColumnEntity columnentity = new ColumnEntity();
            //省级
            if (mark == "province")
            {
                #region 省级导出统计内容
                //判断是否是厂级用户或者是公司用户
                ProvStatisticsEntity hentity = new ProvStatisticsEntity();
                hentity.sDepartId = deptid;
                hentity.sArea = hidPoint;
                hentity.sHidRank = hidRank;
                hentity.statType = statType;
                hentity.sStartDate = startdate;
                hentity.sEndDate = enddate;
                hentity.sYear = year;
              

                //省级隐患数量统计图
                if (model == 0)
                {
                    //隐患区域统计数量列表
                    hentity.sAction = "1";
                    excelconfig.Title = "区域隐患统计基本信息";
                    excelconfig.FileName = "区域隐患统计基本信息.xls";
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "hidpointname", ExcelColumn = "区域名称", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "ordinaryhid", ExcelColumn = "一般隐患", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "importanhid", ExcelColumn = "重大隐患", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "total", ExcelColumn = "合计", Width = 20 });
                }
                else if (model == 1)//省级隐患数量统计图之对比图-按单位对比
                {
                    hentity.sAction = "5";   //对比图分析
                    excelconfig.Title = "单位隐患统计基本信息";
                    excelconfig.FileName = "单位隐患统计基本信息.xls";
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "fullname", ExcelColumn = "部门名称", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "ordinaryhid", ExcelColumn = "一般隐患", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "importanhid", ExcelColumn = "重大隐患", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "total", ExcelColumn = "合计", Width = 20 });
                }
                else if (model == 2) // //省级整改情况统计图
                {
                    hentity.sAction = "6";   //对比图分析
                    excelconfig.Title = "年度隐患整改情况统计基本信息";
                    excelconfig.FileName = "年度隐患整改情况统计基本信息.xls";
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "month", ExcelColumn = "月份", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "yvalue", ExcelColumn = "已整改隐患数量", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "wvalue", ExcelColumn = "未整改隐患数量", Width = 20 });
                }
                else if (model == 3) //省级对比图--按单位对比 
                {
                    hentity.sAction = "8";   //对比图分析
                    excelconfig.Title = "隐患整改状态单位对比统计基本信息";
                    excelconfig.FileName = "隐患整改状态单位对比统计基本信息.xls";
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "fullname", ExcelColumn = "月份", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "nonchange", ExcelColumn = "未整改隐患数量", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "thenchange", ExcelColumn = "已整改隐患数量", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "total", ExcelColumn = "合计", Width = 20 });
                }
                else if (model == 4) //省级对比图--按区域对比 
                {
                    hentity.sAction = "10";   //对比图分析
                    excelconfig.Title = "隐患整改状态区域对比统计基本信息";
                    excelconfig.FileName = "隐患整改状态区域对比统计基本信息.xls";
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "hidpointname", ExcelColumn = "区域名称", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "nonchange", ExcelColumn = "未整改隐患数量", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "thenchange", ExcelColumn = "已整改隐患数量", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "total", ExcelColumn = "合计", Width = 20 });
                }
                dt = htbaseinfobll.QueryProvStatisticsByAction(hentity); 
                #endregion
            }
            else 
            {
                //判断是否是厂级用户或者是公司用户
                StatisticsEntity hentity = new StatisticsEntity();
                hentity.sDeptCode = deptCode;
                hentity.sArea = hidPoint;
                hentity.sHidRank = hidRank;
                hentity.startDate = startdate;
                hentity.endDate = enddate;
                hentity.sYear = year;
                hentity.isCheck = ischeck;
                hentity.sCheckType = checkType;
                hentity.sOrganize = curUser.OrganizeId;
                hentity.statType = statType;
                //当前用户是厂级
                if (curUser.RoleName.Contains("厂级") || curUser.RoleName.Contains("公司级"))
                {
                    hentity.isCompany = true;
                }
                else
                {
                    hentity.isCompany = false;
                }
                  //省级隐患数量统计图
                if (model == 0)
                {
                    hentity.sAction = "1";
                    excelconfig.Title = "区域隐患统计基本信息";
                    excelconfig.FileName = "区域隐患统计基本信息.xls";
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "hidpointname", ExcelColumn = "区域名称", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "ordinaryhid", ExcelColumn = "一般隐患", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "importanhid", ExcelColumn = "重大隐患", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "total", ExcelColumn = "合计", Width = 20 });
                }
                else if (model == 1) 
                {
                    hentity.sAction = "5";   //对比图分析
                    excelconfig.Title = "单位隐患统计基本信息";
                    excelconfig.FileName = "单位隐患统计基本信息.xls";
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "fullname", ExcelColumn = "部门名称", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "ordinaryhid", ExcelColumn = "一般隐患", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "importanhid", ExcelColumn = "重大隐患", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "total", ExcelColumn = "合计", Width = 20 });
                }
                else if (model == 2) // //省级整改情况统计图
                {
                    hentity.sAction = "6";   //对比图分析
                    excelconfig.Title = "年度隐患整改情况统计基本信息";
                    excelconfig.FileName = "年度隐患整改情况统计基本信息.xls";
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "month", ExcelColumn = "月份", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "yvalue", ExcelColumn = "已整改隐患数量", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "wvalue", ExcelColumn = "未整改隐患数量", Width = 20 });
                }
                else if (model == 3) //省级对比图--按单位对比 
                {
                    hentity.sAction = "8";   //对比图分析
                    excelconfig.Title = "隐患整改状态单位对比统计基本信息";
                    excelconfig.FileName = "隐患整改状态单位对比统计基本信息.xls";
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "fullname", ExcelColumn = "月份", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "nonchange", ExcelColumn = "未整改隐患数量", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "thenchange", ExcelColumn = "已整改隐患数量", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "total", ExcelColumn = "合计", Width = 20 });
                }
                else if (model == 4) //省级对比图--按区域对比 
                {
                    hentity.sAction = "10";   //对比图分析
                    excelconfig.Title = "隐患整改状态区域对比统计基本信息";
                    excelconfig.FileName = "隐患整改状态区域对比统计基本信息.xls";
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "hidpointname", ExcelColumn = "区域名称", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "nonchange", ExcelColumn = "未整改隐患数量", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "thenchange", ExcelColumn = "已整改隐患数量", Width = 20 });
                    excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "total", ExcelColumn = "合计", Width = 20 });
                }
                dt = htbaseinfobll.QueryStatisticsByAction(hentity);
            }

            ////调用导出方法
            ExcelHelper.ExcelDownload(dt, excelconfig);
        }
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

    public class dseries 
    {
        public string name { get; set; }
        public string id { get; set; }
        public List<dseries_child> data { get; set; }
    }

    public class dseries_child 
    {
        public string name { get; set; }
        public int y { get; set; }
        public string drilldown { get; set; }
    }
    public class fseries
    {
        public string name { get; set; }
        public List<decimal> data { get; set; }
    }

    public class checkentity
    {
        public List<int> x { get; set; }
        public List<string> y { get; set; }
    }


}
