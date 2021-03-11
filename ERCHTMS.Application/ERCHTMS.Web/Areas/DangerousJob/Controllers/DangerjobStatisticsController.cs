using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using System.Web;
using System;
using BSFramework.Util.Offices;
using System.Data;
using System.Collections.Generic;
using ERCHTMS.Busines.DangerousJob;
using ERCHTMS.Busines.HighRiskWork;

namespace ERCHTMS.Web.Areas.DangerousJob.Controllers
{
    /// <summary>
    /// 描 述：危险作业统计分析
    /// </summary>
    public class DangerjobStatisticsController : MvcControllerBase
    {
        private JobSafetyCardApplyBLL jobSafetyCardApplybll = new JobSafetyCardApplyBLL();
        private JobApprovalFormBLL JobApprovalFormbll = new JobApprovalFormBLL();

        /// <summary>
        /// 列表页面
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        ///作业类型统计(统计图)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetDangerousJobCount(string starttime, string endtime, string deptid, string deptcode)
        {
            return jobSafetyCardApplybll.GetDangerousJobCount(starttime, endtime, deptid, deptcode);
        }

        /// <summary>
        ///作业类型统计(统计表格)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetDangerousJobList(string starttime, string endtime, string deptid, string deptcode)
        {
            return jobSafetyCardApplybll.GetDangerousJobList(starttime, endtime, deptid, deptcode);
        }

        /// <summary>
        ///作业级别统计(统计图)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string DangerousJobLevelCount(string starttime, string endtime, string deptid, string deptcode)
        {
            return JobApprovalFormbll.DangerousJobLevelCount(starttime, endtime, deptid, deptcode);
        }

        /// <summary>
        ///作业级别统计(统计表格)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string DangerousJobLevelList(string starttime, string endtime, string deptid, string deptcode)
        {
            return JobApprovalFormbll.DangerousJobLevelList(starttime, endtime, deptid, deptcode);
        }
     


        public string SaveWork(string TableHtml)
        {
            string PID = Guid.NewGuid().ToString();
            try
            {
                if (System.IO.File.Exists(Server.MapPath("~/Resource/Temp/") + PID + ".txt"))
                {
                    System.IO.File.Delete(Server.MapPath("~/Resource/Temp/") + PID + ".txt");
                }
                System.IO.File.AppendAllText(Server.MapPath("~/Resource/Temp/") + PID + ".txt", TableHtml, System.Text.Encoding.UTF8);
            }
            catch (Exception)
            {

                return "0";
            }

            return PID;
        }

        /// <summary>
        /// 导出表格
        /// </summary>
        /// <param name="PID"></param>
        /// <param name="filename"></param>
        public void ExportWork(string PID, string filename)
        {
            System.IO.StreamReader sr = new System.IO.StreamReader(Server.MapPath("~/Resource/Temp/" + PID + ".txt"), System.Text.Encoding.UTF8);
            string res = sr.ReadToEnd();
            sr.Close();
            if (System.IO.File.Exists(Server.MapPath("~/Resource/Temp/") + PID + ".txt"))
            {
                System.IO.File.Delete(Server.MapPath("~/Resource/Temp/") + PID + ".txt");
            }

            System.Web.HttpContext.Current.Response.Charset = "utf-8";
            System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(Server.UrlDecode(filename), System.Text.Encoding.UTF8) + ".xls");
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            System.Web.HttpContext.Current.Response.ContentType = "application/ms-excel";
            System.Web.HttpContext.Current.Response.Write(Server.UrlDecode(res.ToString()));
            System.Web.HttpContext.Current.Response.End();
        }

    }
}
