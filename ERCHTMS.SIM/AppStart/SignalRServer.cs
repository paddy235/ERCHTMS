using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;

namespace ERCHTMS.IM
{
    /// <summary>
    /// 描 述：即使通信服务
    /// </summary>
    public class SignalRServer
    {
        /// <summary>
        /// 开启服务
        /// </summary>
        public static void Start()
        {
            try
            {
                DataInit();
            }
            catch (Exception ex)
            {
                WriteLog(string.Format("初始化加载基础信息失败：{0}", ex.ToString()));

                return;
            }
            string SignalRURI = ConfigurationManager.AppSettings["SignalRUrl"].ToString();
            try
            {
                try
                {
                    //WriteLog(string.Format("Step1:打开{0}", SignalRURI));
                    //IDisposable dis=WebApp.Start(SignalRURI);
                    //dis.Dispose();
                    //WriteLog(string.Format("Step2:服务开启成功,运行在{0}", SignalRURI));
                    //WriteLog("==========开始接收消息==============");
                    
                    using (WebApp.Start(SignalRURI))
                    {
                        WriteLog(string.Format("Step2:服务开启成功,运行在{0}", SignalRURI));
                        WriteLog("==========开始接收消息==============");
                        Console.ReadLine();
       
                    }
                }
                catch (TargetInvocationException ex)
                {
                    WriteLog(string.Format("服务开启失败,已经有一个服务运行在{0}", SignalRURI));
                    WriteLog(ex.Message + "_" + ex.Source);

                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message.ToString());

            }
        }
        /// <summary>
        /// 初始化加载联系人列表信息
        /// </summary>
        public static void DataInit()
        {
            //获取联系人列表
          
        }
        private static void WriteLog(string msg)
        {
            string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
            System.IO.File.AppendAllText(System.AppDomain.CurrentDomain.BaseDirectory + "logs\\" + fileName, string.Format("{0}：{1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), msg));
        }
    }
}
