using ERCHTMS.Entity.AccidentEvent;
using ERCHTMS.Busines.AccidentEvent;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Linq;
using BSFramework.Util.WebControl;
using BSFramework.Util;
using ERCHTMS.Busines.SystemManage;
using System.Data;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Code;

namespace ERCHTMS.Web.Areas.AccidentEvent.Controllers
{
    public class ReportController : MvcControllerBase
    {
        //
        // GET: /AccidentEvent/Report/
        private BulletinBLL bulletinbll = new BulletinBLL();
        private Bulletin_dealBLL bulletin_dealbll = new Bulletin_dealBLL();
        private WSSJBGBLL wssjbgbll = new WSSJBGBLL();
        private Wssjbg_dealBLL wssjbgbll_deal = new Wssjbg_dealBLL();
        private DataItemBLL dataItemBLL = new DataItemBLL();
        private DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();

        public ActionResult WSSJTypeList()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SGqsList()
        {
            return View();
        }
        public ActionResult SgLvelList()
        {
            return View();
        }
        public ActionResult RsshsgTypeList()
        {
            return View();
        }
        public ActionResult SGYYList()
        {
            return View();
        }


        /// <summary>
        /// 伤亡人数人数统计
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>   
        //[HandlerMonitor(3, "分页查询用户信息!")]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "ID";
            pagination.p_fields = "IsSubmit_deal,SGNAME,SGTYPENAME,HAPPENTIME,AREANAME,CreateDate,dealid";
            pagination.p_tablename = "V_AEM_BULLETIN_deal";
            pagination.conditionJson = "IsSubmit_Deal=1";
            ERCHTMS.Code.Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "CREATEUSERDEPTCODE", "CREATEUSERORGCODE");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }

            }
            pagination.conditionJson += " and IsSubmit_Deal=1";
            var data = bulletinbll.GetPageList(pagination, queryJson);
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
        /// 事故等级统计
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>   
        //[HandlerMonitor(3, "分页查询用户信息!")]
        public ActionResult GetPageListJsonByLevel(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "ID";
            pagination.p_fields = "IsSubmit_deal,CREATEUSERID ,sglevelname,SGNAME,rsshsgtypename, SGTYPENAME,HAPPENTIME,AREANAME,SGKBUSERNAME,DEALID,CREATEUSERDEPTCODE as departmentcode,CREATEUSERORGCODE as  organizecode";
            pagination.p_tablename = "V_AEM_BULLETIN_Deal t";
            pagination.conditionJson = "IsSubmit_Deal=1 and sgtypename like '%事故%' ";
            ERCHTMS.Code.Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "IsSubmit_Deal=1 and sgtypename like '%事故%' ";
            }
            else
            {
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "CREATEUSERDEPTCODE", "CREATEUSERORGCODE");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }

            }
            pagination.conditionJson += " and IsSubmit_Deal=1";

            var data = bulletinbll.GetPageList(pagination, queryJson);
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
        /// 用户列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>   
        //[HandlerMonitor(3, "分页查询用户信息!")]
        public ActionResult GetPageListJsonByRsshsgType(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "ID";
            pagination.p_fields = "IsSubmit_deal,JJYYNAME,BAQZTNAME,BAQXWNAME,RSSHSGTYPENAME,CREATEUSERID ,sglevelname,SGNAME, SGTYPENAME,HAPPENTIME,AREANAME,SGKBUSERNAME,DEALID,CREATEUSERDEPTCODE as departmentcode,CREATEUSERORGCODE as  organizecode";
            pagination.p_tablename = "V_AEM_BULLETIN_deal t";
            pagination.conditionJson = "(sgTypeName='电力生产人身伤亡事故' or sgTypeName='电力建设人身伤亡事故')";
            ERCHTMS.Code.Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "(sgTypeName='电力生产人身伤亡事故' or sgTypeName='电力建设人身伤亡事故')";
            }
            else
            {
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "CREATEUSERDEPTCODE", "CREATEUSERORGCODE");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }

            }
            pagination.conditionJson += " and IsSubmit_Deal=1";
            var data = bulletinbll.GetPageList(pagination, queryJson);
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

        #region 报表统计
        /// <summary>
        /// 应急演练计划完成率
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetReport()
        {
            BulletinBLL bulletinbll = new BulletinBLL();
            Bulletin_dealBLL bulletin_dealbll = new Bulletin_dealBLL();
            WSSJBGBLL wssjbgbll = new WSSJBGBLL();
            //查询条件
            var TimeStart = Request["TimeStart"] ?? "";
            var TimeEnd = Request["TimeEnd"] ?? "";
            var dlrsswtype = Request["dlrsswtype"] ?? "";
            var sgtype = Request["sgtype"] ?? "";
            var wssjtype = Request["wssjtype"] ?? "";
            var bjnf = Request["bjnf"] ?? "";
            var type = int.Parse(Request["type"] ?? "0");
            var returnListObj = new List<Object>();
            var data = new object();
            //统计时间
            var times = DateTime.Now;
            var timee = DateTime.Now;
            if (TimeStart.Length > 0)
                times = DateTime.Parse(TimeStart);
            if (TimeEnd.Length > 0)
                timee = DateTime.Parse(TimeEnd).AddDays(1);
            //事故事件类型统计

            var count = 0;
            var count1 = 0;
            var count2 = 0;
            #region 权限
            string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "CREATEUSERDEPTCODE", "CREATEUSERORGCODE");
            string sqlwhere = " and IsSubmit_Deal=1 ";
            if (!string.IsNullOrEmpty(where))
            {
                sqlwhere += " and " + where;
            }
            else
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                sqlwhere += string.Format(" and CREATEUSERDEPTCODE like '{0}%'", user.DeptCode);
            }
            #endregion
            switch (type)
            {

                //按事故事件类型统计
                case 0:
                    var list = bulletin_dealbll.GetList(sqlwhere).Where(e => e.SGTYPE_DEAL != null && (TimeStart.Length > 0 ? e.HAPPENTIME_DEAL >= times : 1 == 1)
                && (TimeEnd.Length > 0 ? e.HAPPENTIME_DEAL <= timee : 1 == 1));
                    if (list != null)
                    {
                        count = list.Count();
                        var date = list.ToList().GroupBy(e => e.SGTYPENAME_DEAL);
                        foreach (var item in date)
                        {
                            var itemCount = decimal.Parse(item.Count().ToString());
                            returnListObj.Add(new { text = item.Key, value = itemCount, bfb = decimal.Round((itemCount / count) * 100, 2) });
                        }
                    }
                    break;
                //按事故等级统计
                case 1:
                    var list1 = bulletin_dealbll.GetList(sqlwhere).Where(e => e.SGTYPENAME_DEAL.Contains("事故") && e.SGLEVEL_DEAL != null && (TimeStart.Length > 0 ? e.HAPPENTIME_DEAL >= times : 1 == 1)
                && (TimeEnd.Length > 0 ? e.HAPPENTIME_DEAL <= timee : 1 == 1) && (sgtype.Length > 0 ? e.SGTYPE_DEAL == sgtype : 1 == 1));
                    if (list1 != null)
                    {
                        count = list1.Count();
                        var date = list1.ToList().GroupBy(e => e.SGLEVELNAME_DEAL);
                        foreach (var item in date)
                        {
                            var itemCount = decimal.Parse(item.Count().ToString());
                            returnListObj.Add(new { text = item.Key, value = itemCount, bfb = decimal.Round((itemCount / count) * 100, 2) });
                        }
                    }

                    break;
                //按伤亡人数统计
                case 2:
                    var list2 = bulletin_dealbll.GetList(sqlwhere).Where(e => e.SGTYPENAME_DEAL.Contains("人身伤亡事故") && e.SGLEVEL_DEAL != null && (TimeStart.Length > 0 ? e.HAPPENTIME_DEAL >= times : 1 == 1)
                && (TimeEnd.Length > 0 ? e.HAPPENTIME_DEAL <= timee : 1 == 1) && (sgtype.Length > 0 ? e.SGTYPE_DEAL == sgtype : 1 == 1));
                    count = list2.Count();
                    if (list2 != null && count > 0)
                    {
                        var query = from l in list2
                                    group l by new { l.SGLEVELNAME_DEAL } into tab
                                    select new
                                    {
                                        text = tab.Key.SGLEVELNAME_DEAL,
                                        value = tab.Count(),
                                        swnum = list2.Where(e => e.SGLEVELNAME_DEAL == tab.Key.SGLEVELNAME_DEAL).Sum(p => p.SWNUM_DEAL),
                                        zsnum = list2.Where(e => e.SGLEVELNAME_DEAL == tab.Key.SGLEVELNAME_DEAL).Sum(p => p.ZSNUM_DEAL),
                                        sznum = list2.Where(e => e.SGLEVELNAME_DEAL == tab.Key.SGLEVELNAME_DEAL).Sum(p => p.SZNUM_DEAL),
                                        qsnum = list2.Where(e => e.SGLEVELNAME_DEAL == tab.Key.SGLEVELNAME_DEAL).Sum(p => p.QSNUM_DEAL)
                                    };
                        returnListObj.AddRange(query);
                    }
                    break;
                //人身伤害事故类别统计
                case 3:
                    var list3 = bulletin_dealbll.GetList(sqlwhere).Where(e => e.SGTYPENAME_DEAL.Contains("人身伤亡事故") && e.RSSHSGTYPE_DEAL != null && (TimeStart.Length > 0 ? e.HAPPENTIME_DEAL >= times : 1 == 1)
                && (TimeEnd.Length > 0 ? e.HAPPENTIME_DEAL <= timee : 1 == 1) && (sgtype.Length > 0 ? e.SGTYPE_DEAL == sgtype : 1 == 1));
                    count = list3.Count();
                    if (list3 != null && count > 0)
                    {
                        var query = from l in list3
                                    group l by new { l.RSSHSGTYPENAME_DEAL } into tab
                                    select new
                                    {
                                        text = tab.Key.RSSHSGTYPENAME_DEAL,
                                        value = tab.Count(),
                                        swnum = list3.Where(e => e.RSSHSGTYPENAME_DEAL == tab.Key.RSSHSGTYPENAME_DEAL).Sum(p => p.SWNUM_DEAL),
                                        zsnum = list3.Where(e => e.RSSHSGTYPENAME_DEAL == tab.Key.RSSHSGTYPENAME_DEAL).Sum(p => p.ZSNUM_DEAL),
                                        sznum = list3.Where(e => e.RSSHSGTYPENAME_DEAL == tab.Key.RSSHSGTYPENAME_DEAL).Sum(p => p.SZNUM_DEAL),
                                        qsnum = list3.Where(e => e.RSSHSGTYPENAME_DEAL == tab.Key.RSSHSGTYPENAME_DEAL).Sum(p => p.QSNUM_DEAL)
                                    };
                        var listnow = query.ToList();
                        returnListObj.AddRange(query);
                    }
                    break;

                //伤亡事故原因统计
                case 4:
                    var listXXX = bulletin_dealbll.GetList(sqlwhere);
                    var list4 = bulletin_dealbll.GetList(sqlwhere).Where(e => (TimeStart.Length > 0 ? e.HAPPENTIME_DEAL >= times : 1 == 1)
                && (TimeEnd.Length > 0 ? e.HAPPENTIME_DEAL <= timee : 1 == 1) && (e.SGTYPENAME_DEAL != null ? e.SGTYPENAME_DEAL.Contains("人身伤亡事故") : e.SGTYPENAME_DEAL != null)
                && (sgtype.Length > 0 ? e.SGTYPE_DEAL == sgtype : 1 == 1));
                    count = list4.Count();

                    var xwtext = "A不安全行为";
                    int xwswnum = 0;
                    int xwzsnum = 0;
                    int xwsznum = 0;
                    int xwqsnum = 0;
                    int xwvalue = 0;
                    var zttext = "B不安全状态";
                    int ztswnum = 0;
                    int ztzsnum = 0;
                    int ztsznum = 0;
                    int ztqsnum = 0;
                    int ztvalue = 0;
                    //查询包含不安全行为的数据有多少条
                    #region 不安全行为的数据
                    var entityXW = dataItemBLL.GetEntityByCode("AEM_BAQXW");
                    var listXWdetail = dataItemDetailBLL.GetListByCode(entityXW.ItemCode);
                    foreach (DataRow row in listXWdetail.Rows)
                    {
                        count1++;
                        //是否包含
                        var itemname = row["ItemName"].ToString();
                        var listxw = list4.Where(e => (e.BAQXWNAME != null) && e.BAQXWNAME.Contains(row["ItemName"].ToString())).ToList();
                        int swnum1 = 0;
                        int zsnum1 = 0;
                        int sznum1 = 0;
                        int qsnum1 = 0;
                        int value1 = 0;
                        if (listxw != null)
                        {
                            value1 = listxw.Count();
                            foreach (var item in listxw)
                            {
                                xwswnum += item.SWNUM_DEAL.Value;
                                xwzsnum += item.ZSNUM_DEAL.Value;
                                xwsznum += item.SZNUM_DEAL.Value;
                                xwqsnum += item.QSNUM_DEAL.Value;

                                swnum1 += item.SWNUM_DEAL.Value;
                                zsnum1 += item.ZSNUM_DEAL.Value;
                                sznum1 += item.SZNUM_DEAL.Value;
                                qsnum1 += item.QSNUM_DEAL.Value;
                            }
                            xwvalue += value1;
                            returnListObj.Add(new { type = 1, text = row["ItemName"].ToString(), value = value1, swnum = swnum1, zsnum = zsnum1, sznum = sznum1, qsnum = qsnum1 });
                        }
                        else
                        {
                            returnListObj.Add(new { type = 1, text = row["ItemName"].ToString(), value = value1, swnum = swnum1, zsnum = zsnum1, sznum = sznum1, qsnum = qsnum1 });

                        }

                    }
                    returnListObj.Insert(0, new { type = 1, text = xwtext, value = xwvalue, swnum = xwsznum, zsnum = xwzsnum, sznum = xwsznum, qsnum = xwqsnum });

                    #endregion


                    #region 不安全状态的数据
                    var entityZT = dataItemBLL.GetEntityByCode("AEM_BAQZT");
                    var listZTdetail = dataItemDetailBLL.GetListByCode(entityZT.ItemCode);
                    foreach (DataRow row in listZTdetail.Rows)
                    {
                        count2++;
                        count1++;
                        //是否包含
                        var listzt = list4.Where(e => (e.BAQZTNAME != null) && e.BAQZTNAME.Contains(row["ItemName"].ToString()));
                        int swnum2 = 0;
                        int zsnum2 = 0;
                        int sznum2 = 0;
                        int qsnum2 = 0;
                        int value2 = 0;
                        if (listzt != null)
                        {
                            value2 = listzt.Count();
                            foreach (var item in listzt)
                            {
                                ztswnum += item.SWNUM_DEAL.Value;
                                ztzsnum += item.ZSNUM_DEAL.Value;
                                ztsznum += item.SZNUM_DEAL.Value;
                                ztqsnum += item.QSNUM_DEAL.Value;

                                swnum2 += item.SWNUM_DEAL.Value;
                                zsnum2 += item.ZSNUM_DEAL.Value;
                                sznum2 += item.SZNUM_DEAL.Value;
                                qsnum2 += item.QSNUM_DEAL.Value;
                            }
                            ztvalue += value2;

                            returnListObj.Add(new { type = 1, text = row["ItemName"].ToString(), value = value2, swnum = swnum2, zsnum = zsnum2, sznum = sznum2, qsnum = qsnum2 });
                        }
                        else
                        {

                            returnListObj.Add(new { type = 1, text = row["ItemName"].ToString(), value = value2, swnum = swnum2, zsnum = zsnum2, sznum = sznum2, qsnum = qsnum2 });

                        }

                    }
                    returnListObj.Insert(count1 - count2 + 1, new { type = 1, text = zttext, value = ztvalue, swnum = ztsznum, zsnum = ztzsnum, sznum = ztsznum, qsnum = ztqsnum });
                    #endregion


                    #region 间接原因
                    var entityJJYY = dataItemBLL.GetEntityByCode("AEM_JJYY");
                    var listJJYY = dataItemDetailBLL.GetListByCode(entityJJYY.ItemCode);
                    foreach (DataRow row in listJJYY.Rows)
                    {
                        count2++;
                        //是否包含
                        var listjjyy = list4.Where(e => (e.JJYYNAME != null) && e.JJYYNAME.Contains(row["ItemName"].ToString()));
                        int swnum3 = 0;
                        int zsnum3 = 0;
                        int sznum3 = 0;
                        int qsnum3 = 0;
                        int value3 = 0;
                        if (listjjyy != null)
                        {
                            value3 = listjjyy.Count();
                            foreach (var item in listjjyy)
                            {
                                swnum3 += item.SWNUM_DEAL.Value;
                                zsnum3 += item.ZSNUM_DEAL.Value;
                                sznum3 += item.SZNUM_DEAL.Value;
                                qsnum3 += item.QSNUM_DEAL.Value;
                            }
                            //xwvalue += value3;
                            returnListObj.Add(new { type = 2, text = row["ItemName"].ToString(), value = value3, swnum = swnum3, zsnum = zsnum3, sznum = sznum3, qsnum = qsnum3 });
                        }
                        else
                        {
                            returnListObj.Add(new { type = 2, text = row["ItemName"].ToString(), value = value3, swnum = swnum3, zsnum = zsnum3, sznum = sznum3, qsnum = qsnum3 });

                        }
                    }
                    #endregion
                    count1 = count1 + 2;
                    break;
                //按年份统计
                case 5:
                    var listYear = new List<int>();
                    if (bjnf.Length > 0 && bjnf != "0")
                    {
                        //近五年
                        if (bjnf == "1")
                        {
                            timee = DateTime.Now;
                            for (int i = 1; i < 5; i++)
                            {
                                listYear.Add(times.Year - i);
                            }
                            times = DateTime.Now.AddYears(-5);

                        }
                        //近十年
                        if (bjnf == "2")
                        {
                            timee = DateTime.Now;
                            for (int i = 1; i < 10; i++)
                            {
                                listYear.Add(times.Year - i);
                            }
                            times = DateTime.Now.AddYears(-10);

                        }

                    }
                    var list5 = bulletin_dealbll.GetList(sqlwhere).Where(e => e.HAPPENTIME_DEAL != null
                    && (TimeStart.Length > 0 ? e.HAPPENTIME_DEAL.Value.Year >= times.Year : 1 == 1)
                    && (TimeEnd.Length > 0 ? e.HAPPENTIME_DEAL.Value.Year <= timee.Year : 1 == 1)
                    && (sgtype == "-1" ? e.SGTYPENAME_DEAL.Contains("事件") : 1 == 1) //所有的事件
                    && (sgtype == "-2" ? e.SGTYPENAME_DEAL.Contains("事故") : 1 == 1) //所有的事故
                    && ((sgtype.Length > 0 && sgtype != "-1" && sgtype != "-2") ? e.SGTYPE_DEAL == sgtype : 1 == 1) //选择了具体的事故或者事件
                    );
                    count = list5.Count();
                    if (list5 != null && count > 0)
                    {
                        var query = from l in list5
                                    group l by new { l.HAPPENTIME_DEAL.Value.Year } into tab
                                    select new
                                    {
                                        text = tab.Key.Year,
                                        value = list5.Where(e => e.HAPPENTIME_DEAL.Value.Year == tab.Key.Year).Count(),
                                        swnum = list5.Where(e => e.HAPPENTIME_DEAL.Value.Year == tab.Key.Year).Sum(p => p.SWNUM_DEAL),
                                        zsnum = list5.Where(e => e.HAPPENTIME_DEAL.Value.Year == tab.Key.Year).Sum(p => p.ZSNUM_DEAL),
                                        sznum = list5.Where(e => e.HAPPENTIME_DEAL.Value.Year == tab.Key.Year).Sum(p => p.SZNUM_DEAL),
                                        qsnum = list5.Where(e => e.HAPPENTIME_DEAL.Value.Year == tab.Key.Year).Sum(p => p.QSNUM_DEAL)
                                    };
                        var listQuery = query.ToList();
                        foreach (var item in query)
                        {
                            if (listYear.Contains(item.text)) continue;
                            else listYear.Remove(item.text);


                        }
                        foreach (var item in listYear)
                        {
                            returnListObj.Add(new { text = item, value = 0, swnum = 0, zsnum = 0, sznum = 0, qsnum = 0 });
                        }

                        returnListObj.AddRange(query);

                    }
                    break;
                //按未遂事件类型统计
                case 6:
                    var list6 = wssjbgbll_deal.GetList(sqlwhere).Where(e => e.WssjType_Deal != null && (TimeStart.Length > 0 ? e.HappenTime_Deal >= times : 1 == 1)
          && (TimeEnd.Length > 0 ? e.HappenTime_Deal <= timee : 1 == 1));
                    count = list6.Count();
                    if (list6 != null && count > 0)
                    {
                        var date = list6.ToList().GroupBy(e => e.WssjTypeName_Deal);
                        foreach (var item in date)
                        {
                            var itemCount = decimal.Parse(item.Count().ToString());
                            returnListObj.Add(new { text = item.Key, value = itemCount, bfb = decimal.Round((itemCount / count) * 100, 2) });
                        }
                    }
                    break;
                //年度变化统计
                case 7:
                    var listYearWS = new List<int>();
                    if (bjnf.Length > 0 && bjnf != "0")
                    {
                        //近五年
                        if (bjnf == "1")
                        {
                            timee = DateTime.Now;
                            for (int i = 1; i <5; i++)
                            {
                                listYearWS.Add(times.Year - i);
                            }
                            times = DateTime.Now.AddYears(-5);
                        }
                        //近十年
                        if (bjnf == "2")
                        {
                            timee = DateTime.Now;
                            for (int i = 1; i < 10; i++)
                            {
                                listYearWS.Add(times.Year - i);
                            }
                            times = DateTime.Now.AddYears(-10);
                        }

                    }

                    var list7 = wssjbgbll_deal.GetList(sqlwhere).Where(e => e.HappenTime_Deal != null && (bjnf != "0" ? e.HappenTime_Deal.Value.Year >= times.Year : 1 == 1)
          && (bjnf != "0" ? e.HappenTime_Deal.Value.Year <= timee.Year : 1 == 1) && wssjtype.Length > 0 ? e.WssjType_Deal == wssjtype : 1 == 1);
                    count = list7.Count();
                    if (list7 != null && count > 0)
                    {
                        var date = list7.ToList().GroupBy(e => e.HappenTime_Deal.Value.Year);
                        foreach (var item in date)
                        {
                            var itemCount = decimal.Parse(item.Count().ToString());
                            returnListObj.Add(new { text = item.Key, value = itemCount });
                            listYearWS.Remove(item.Key);
                        }
                    }


                    foreach (var item in listYearWS)
                    {
                        returnListObj.Add(new { text = item, value = 0 });
                    }

                    break;
                default:
                    break;
            }

            data = new { list = returnListObj, count = count, count1 = count1, count2 = count2 };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            return json;
        }
        #endregion

    }
}
