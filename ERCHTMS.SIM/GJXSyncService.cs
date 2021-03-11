using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.SIM
{
    partial class GJXSyncService : ServiceBase
    {
        System.Timers.Timer timer1 = new System.Timers.Timer();
        string apiUrl = System.Configuration.ConfigurationSettings.AppSettings["ApiUri"];
        public GJXSyncService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (!System.Diagnostics.EventLog.SourceExists("GJXSync"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "GJXSync", "Application");
            }
            eventLog1.Source = "GJXSync";
            eventLog1.Log = "Application";

            string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
            string msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：服务启动\r\n";
            eventLog1.WriteEntry(msg);
            // TODO:  在此处添加代码以启动服务。
            WebClient wc = new WebClient();
            wc.Encoding = System.Text.Encoding.UTF8;
            wc.UseDefaultCredentials = true;
            string val = wc.DownloadString(apiUrl + "basedata/get?name=SyncWay").Replace("\"", "");

            //string way = System.Configuration.ConfigurationSettings.AppSettings["SyncWay"];
            if (val == "0")
            {
                val = wc.DownloadString(apiUrl + "basedata/get?name=IntervalMinutes").Replace("\"", "");
                timer1.Interval = int.Parse(val) * 1000 * 60;
            }
            else
            {
                timer1.Interval =1000*60;
            }
            timer1.Elapsed += timer1_Elapsed;
            timer1.Enabled = true;
            timer1.Start();
        }

        void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            WebClient wc = new WebClient();
            wc.Encoding = System.Text.Encoding.UTF8;
            wc.UseDefaultCredentials = true;
            string val = wc.DownloadString(apiUrl + "basedata/get?name=SyncWay").Replace("\"", "");
            string hour = DateTime.Now.ToString("HH:mm");
            if (val == "0")
            {
                SyncData();
            }
            else
            {
                //string time = System.Configuration.ConfigurationSettings.AppSettings["IntervalMinutes"];
                val = wc.DownloadString(apiUrl + "basedata/get?name=IntervalMinutes").Replace("\"", "");
                string[] arr = val.Split(',');
                if (val.Contains(hour))
                {
                    SyncData();
                }
            }
        }
        private void SyncData()
        {

            WebClient wc = new WebClient();
            wc.Encoding = System.Text.Encoding.UTF8;
            wc.UseDefaultCredentials = true;
            string urls = wc.DownloadString(apiUrl + "basedata/get?name=ApiUrls").Replace("\"", "");

            //string urls = System.Configuration.ConfigurationSettings.AppSettings["ApiUri"];
            string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
            if (!System.IO.Directory.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "logs"))
            {
                System.IO.Directory.CreateDirectory(System.AppDomain.CurrentDomain.BaseDirectory + "logs");
            }
                  foreach (string url in urls.Split(','))
                  {
                      try
                      {
                          wc = new WebClient();
                          wc.Encoding = System.Text.Encoding.UTF8;
                          wc.UseDefaultCredentials = true;
                          wc.DownloadStringCompleted += wc_DownloadStringCompleted;
                          wc.DownloadStringAsync(new Uri(url));
                      }
                      catch(Exception ex)
                      {
                          eventLog1.WriteEntry(string.Format("{0}：地址：{2}，异常信息：{1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message, url));
                      }
                  
                  }
        }

        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                eventLog1.WriteEntry(string.Format("{0}：返回信息>{1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), e.Result));
            }
            catch(Exception ex)
            {
                eventLog1.WriteEntry(string.Format("{0}：异常信息>{1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),Newtonsoft.Json.JsonConvert.SerializeObject(ex)));
            }          
        }
        protected override void OnStop()
        {
            // TODO:  在此处添加代码以执行停止服务所需的关闭操作。
            //string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
            //eventLog1.WriteEntry(string.Format("{0}：{1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "服务停止"));
            timer1.Enabled = false;
        }
        protected override void OnPause()
        {
            // TODO:  在此处添加代码以执行停止服务所需的关闭操作。
            //string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
            //System.IO.File.AppendAllText(System.AppDomain.CurrentDomain.BaseDirectory + "logs\\" + fileName, string.Format("{0}：>{1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "服务暂停"));
            timer1.Enabled = false;
        }
        private void WriteLog(string msg)
        {
            string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
            System.IO.File.AppendAllText(System.AppDomain.CurrentDomain.BaseDirectory + "logs\\" + fileName, string.Format("{0}：{1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), msg));
        }
    }
}
