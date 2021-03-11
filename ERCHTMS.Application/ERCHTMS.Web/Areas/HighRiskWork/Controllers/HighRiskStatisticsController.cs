using ERCHTMS.Busines.SaftyCheck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERCHTMS.Busines.EngineeringManage;
using ERCHTMS.Busines.HighRiskWork;

namespace ERCHTMS.Web.Areas.HighRiskWork.Controllers
{
    /// <summary>
    /// 描 述：高风险作业统计
    /// </summary>
    public class HighRiskStatisticsController : Controller
    {
        private HighRiskApplyBLL highriskapplybll = new HighRiskApplyBLL();

        /// <summary>
        /// 列表页面
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 统计跳转页面
        /// </summary>
        public ActionResult Report()
        {
            return View();
        }

        /// <summary>
        ///作业类型统计(统计图)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetHighWorkCount(string starttime, string endtime, string deptid, string deptcode)
        {
            return highriskapplybll.GetHighWorkCount(starttime, endtime, deptid, deptcode);
        }

        /// <summary>
        ///作业类型统计(统计表格)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetHighWorkList(string starttime, string endtime, string deptid, string deptcode)
        {
            return highriskapplybll.GetHighWorkList(starttime, endtime, deptid, deptcode);
        }

        /// <summary>
        ///危险作业数量月度变化(统计图)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetHighWorkYearCount(string year, string deptid, string deptcode)
        {
            return highriskapplybll.GetHighWorkYearCount(year, deptid, deptcode);
        }

        /// <summary>
        ///作业类型统计(统计表格)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetHighWorkYearList(string year, string deptid, string deptcode)
        {
            return highriskapplybll.GetHighWorkYearList(year, deptid, deptcode);
        }

    }
}
