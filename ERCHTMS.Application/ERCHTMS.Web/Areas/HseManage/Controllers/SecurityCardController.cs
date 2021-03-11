using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.HseToolManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.HseManage.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.HseManage.Controllers
{
    public class SecurityCardController : MvcControllerBase
    {
        #region 统计
        public ActionResult Statistics()
        {
            int year = DateTime.Now.Year;
            int minYear = year - 6;
            List<SelectListItem> yearList = new List<SelectListItem>();
            List<SelectListItem> monthList = new List<SelectListItem>();
            do
            {
                yearList.Add(new SelectListItem() { Text = year.ToString(), Value = year.ToString() });
                year--;
            }
            while (year > minYear);
            int month = 1;

            do {
                monthList.Add(new SelectListItem() { Text = month.ToString(), Value = month.ToString(), Selected = month == DateTime.Now.Month ? true : false });
                month++;
            } while (month <= 12);
            ViewBag.MonthList = monthList;
            ViewBag.YearList = yearList;
            return View();
        }
        /// <summary>
        /// 获取参与率
        /// </summary>
        /// <param name="form">Year 年份 DeptId 部门ID</param>
        /// <returns></returns>
        public ActionResult GetCYLCount(FormCollection form)
        {
            var year = form["Year"];
            var deptid = form["DeptId"];
            try
            {
                string deptEncode = string.Empty;
            
                if (!string.IsNullOrWhiteSpace(deptid))
                {
                  var   serchDept = new DepartmentBLL().GetEntity(deptid);
                    if (serchDept != null)
                    {
                        deptEncode = serchDept.EnCode;
                    }
                    else
                    {
                        return Json(new { Code = -1, Message="未找到要查询的部门" });
                    }
                }
                else
                {
                    //为空则查全厂，也就是当前的用户的所在组织的Code
                     deptEncode = OperatorProvider.Provider.Current().OrganizeCode;
                }
                List<HseKeyValue> data = new HseObserveBLL().GetCYLData(year, deptEncode);
                return Json(new { Code = 0, Data= data });
                }
            catch (Exception ex)
            {
                return Json(new { Code = -1, ex.Message });
            }
        }
        /// <summary>
        /// 查询本电厂各部门的参与率
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public ActionResult GetDeptCYL(FormCollection form)
        {
            try
            {
                var user = OperatorProvider.Provider.Current();
                //先找到当前用户所在的电厂
                var searchDept = new DepartmentBLL().GetList().Where(p => p.ParentId == user.OrganizeId || p.DepartmentId == user.OrganizeId).OrderBy(x => x.EnCode).ToList();//当前电厂与电厂底下的部门数据。分组组装数据用
                DataTable dt = new HseObserveBLL().GetDeptCYL(form["Year"], user.OrganizeCode);
                List<DeptMonthData> data = new List<DeptMonthData>();
                var userList = new UserBLL().GetList().Where(p=>p.IsPresence=="1");
                if (searchDept != null && searchDept.Count > 0)
                {
                    //组装各个部门的数据
                    searchDept.ForEach(p =>
                    {
                        DeptMonthData deptMonth = new DeptMonthData(form["Year"]);
                        deptMonth.DeptName = p.FullName;
                        //var drItem = dt.Select(" CREATEUSERDEPTCODE LIKE '" + p.EnCode + "%'");

                        //foreach (DataRow dr in drItem)
                        //{
                        //    decimal userCount = userList.Count(x => x.DepartmentCode.StartsWith(p.EnCode));    //总人数
                        //    var obj = deptMonth.MonthData.FirstOrDefault(x => x.Key == dr["Month"].ToString());
                        //    if (obj != null)
                        //    {
                        //        //参与度=(已提交卡总数/总人数*周数)*(实际提交人数/总人数)*100%

                        //        decimal count = dr["SUBMITCOUNT"] == null ? 0 : Convert.ToDecimal(dr["SUBMITCOUNT"]);    //已提交总数
                        //        decimal submitcount = dr["SUBMITUSER"] == null ? 0 : Convert.ToDecimal(dr["SUBMITUSER"]);    //已提交人数 (重复提交的人只算一次)
                        //        if (userCount > 0)//分母不为0
                        //        {
                        //            obj.Value = Math.Round((count / (userCount * 4)) * (submitcount / userCount) * 100, 2);//百分比
                        //        }
                        //    }
                        //}
                        #region 新
                        decimal userCount = userList.Count(x => x.DepartmentCode.StartsWith(p.EnCode));    //当前部门下的总人数
                        if (userCount > 0)
                        {
                            deptMonth.MonthData.ForEach(x =>//月份循环
                            {
                                //处理当前部门及子部门的数据
                                var drItem = dt.Select(" CREATEUSERDEPTCODE LIKE '" + p.EnCode + "%' AND MONTH='" + x.Key + "'");
                                if (drItem != null && drItem.Length > 0)
                                {
                                    decimal submitTotal = 0;//已提交总数
                                    decimal submitUserCount = 0;//已提交人数 (重复提交的人只算一次)
                                    foreach (DataRow dr in drItem)
                                    {
                                        submitTotal += dr["SUBMITCOUNT"] == null ? 0 : Convert.ToDecimal(dr["SUBMITCOUNT"]);
                                        submitUserCount += dr["SUBMITUSER"] == null ? 0 : Convert.ToDecimal(dr["SUBMITUSER"]);
                                    }
                                    //计算百分比  参与度=(已提交卡总数/总人数*周数)*(实际提交人数/总人数)*100% 
                                    x.Value = Math.Round((submitTotal / (userCount * 4)) * (submitUserCount / userCount) * 100, 2);//百分比
                                }
                            });
                        }
                        #endregion
                        data.Add(deptMonth);
                    });

                }

                return Json(new { Code = 0, Data = data });

            }
            catch (Exception ex)
            {
                return Json(new { Code = -1, ex.Message });
            }
        }
        /// <summary>
        /// 获取危险项统计
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public ActionResult GetWXXCount(FormCollection form)
        {
            try
            {
                var year = form["Year"];
                var month = form["Month"];
                List<HseKeyValue> data = new HseObserveBLL().GetWXXCount(year, month);
                return Json(new { Code = 0, Data = data });
            }
            catch (Exception ex)
            {
                return Json(new { Code = -1, ex.Message });
            }
        }
        /// <summary>
        /// 获取危险项表格每月数据
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public ActionResult GetWXX(FormCollection form)
        {
            try
            {
                var year = form["Year"];
                List<DeptMonthData> data = new List<DeptMonthData>();
                DataTable dt = new HseObserveBLL().GetWXX(year);
                if (dt.Rows != null && dt.Rows.Count>0)
                {
                    List<HseKeyValue> hseKeyValues = new List<HseKeyValue>();
                    var enumtor = dt.Rows.GetEnumerator();
                    while (enumtor.MoveNext())
                    {
                        HseKeyValue keyValue = new HseKeyValue();
                        DataRow dr = enumtor.Current as DataRow;

                        keyValue.Key = dr["CONTENT"] == null ? null : dr["CONTENT"].ToString();
                        keyValue.Str1 = dr["MONTH"] == null ? null : dr["MONTH"].ToString();
                        keyValue.Num1 = dr["COUNT"] == null ? 0 : Convert.ToInt32(dr["COUNT"]);
                        hseKeyValues.Add(keyValue);
                    };
                    List<string> contentList = hseKeyValues.Select(p => p.Key).Distinct().ToList();
                    contentList.ForEach(p => {
                        DeptMonthData deptMonth = new DeptMonthData(year, true);
                        deptMonth.Ttile = p;
                        deptMonth.MonthData.ForEach(x => {
                            x.Value = hseKeyValues.Where(hse => hse.Key == p && hse.Str1 == x.Key).Sum(d => d.Num1);
                        });
                        data.Add(deptMonth);
                    });
                }
                var month = new DeptMonthData(year, true).MonthData.Select(p => p.Key).ToList();
                return Json(new { Code = 0, Data = new { Info = data, MonthInfo = month } });
            }
            catch (Exception ex)
            {
                return Json(new { Code = -1, ex.Message });
            }
        }

        public ActionResult TableHtml(string year)
        {
            List<DeptMonthData> data = new List<DeptMonthData>();
            DataTable dt = new HseObserveBLL().GetWXX(year);
            if (dt.Rows != null && dt.Rows.Count > 0)
            {
                List<HseKeyValue> hseKeyValues = new List<HseKeyValue>();
                var enumtor = dt.Rows.GetEnumerator();
                while (enumtor.MoveNext())
                {
                    HseKeyValue keyValue = new HseKeyValue();
                    DataRow dr = enumtor.Current as DataRow;

                    keyValue.Key = dr["CONTENT"] == null ? null : dr["CONTENT"].ToString();
                    keyValue.Str1 = dr["MONTH"] == null ? null : dr["MONTH"].ToString();
                    keyValue.Num1 = dr["COUNT"] == null ? 0 : Convert.ToInt32(dr["COUNT"]);
                    hseKeyValues.Add(keyValue);
                };
                List<string> contentList = hseKeyValues.Select(p => p.Key).Distinct().ToList();
                contentList.ForEach(p => {
                    DeptMonthData deptMonth = new DeptMonthData(year, true);
                    deptMonth.Ttile = p;
                    deptMonth.MonthData.ForEach(x => {
                        x.Value = hseKeyValues.Where(hse => hse.Key == p && hse.Str1 == x.Key).Sum(d => d.Num1);
                    });
                    data.Add(deptMonth);
                });
            }
            var month = new DeptMonthData(year, true).MonthData.Select(p => p.Key).ToList();
            ViewBag.MonthInfo = month;
            return View(data);
        }

        public ActionResult ShowTip()
        {
            return View();
        }
        #endregion
    }
}