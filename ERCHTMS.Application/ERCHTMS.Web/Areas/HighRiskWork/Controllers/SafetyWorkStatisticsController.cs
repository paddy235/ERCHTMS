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
using ERCHTMS.Busines.HighRiskWork;

namespace ERCHTMS.Web.Areas.HighRiskWork.Controllers
{
    /// <summary>
    /// �� �����߷���ͳ�Ʒ���
    /// </summary>
    public class SafetyWorkStatisticsController : MvcControllerBase
    {
        private HighRiskCommonApplyBLL highriskcommonapplybll = new HighRiskCommonApplyBLL();
        /// <summary>
        /// �б�ҳ��
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        ///��ҵ����ͳ��(ͳ��ͼ)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetHighWorkCount(string starttime, string endtime, string deptid, string deptcode)
        {
            return highriskcommonapplybll.GetHighWorkCount(starttime, endtime, deptid, deptcode);
        }

        /// <summary>
        ///��ҵ����ͳ��(ͳ�Ʊ��)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetHighWorkList(string starttime, string endtime, string deptid, string deptcode)
        {
            return highriskcommonapplybll.GetHighWorkList(starttime, endtime, deptid, deptcode);
        }

        /// <summary>
        ///Σ����ҵ�����¶ȱ仯(ͳ��ͼ)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetHighWorkYearCount(string year, string deptid, string deptcode)
        {
            return highriskcommonapplybll.GetHighWorkYearCount(year, deptid, deptcode);
        }

        /// <summary>
        ///��ҵ����ͳ��(ͳ�Ʊ��)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetHighWorkYearList(string year, string deptid, string deptcode)
        {
            return highriskcommonapplybll.GetHighWorkYearList(year, deptid, deptcode);
        }

        /// <summary>
        /// ��λ�Ա�(ͳ��ͼ)
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetHighWorkDepartCount(string starttime, string endtime)
        {
            return highriskcommonapplybll.GetHighWorkDepartCount(starttime, endtime);
        }

        /// <summary>
        ///��λ�Ա�(ͳ�Ʊ��)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetHighWorkDepartList(string starttime, string endtime)
        {
            return highriskcommonapplybll.GetHighWorkDepartList(starttime, endtime);
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
        /// �������
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
