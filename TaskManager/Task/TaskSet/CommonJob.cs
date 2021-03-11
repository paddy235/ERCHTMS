using System;
using Quartz;
using Utility.Log;
using System.Net;

namespace Task.TaskSet
{
    /// <summary>
    /// 测试任务
    /// </summary>
    ///<remarks>DisallowConcurrentExecution：任务不可并行，要是上一任务没运行完即使到了运行时间也不会运行</remarks>
    //[DisallowConcurrentExecution]
    public class CommonJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            object objParam = context.JobDetail.JobDataMap.Get("TaskParam");
            string urls = "";
            if(objParam!=null)
            {
                urls = objParam.ToString();
            }
            if (!string.IsNullOrEmpty(urls))
            {
                WebClient wc = new WebClient();
                wc.Encoding = System.Text.Encoding.UTF8;
                wc.UseDefaultCredentials = true;
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
                    catch (Exception ex)
                    {
                        LogHelper.Debug(string.Format("异常信息>{0}\r\n",  ex));
                    }
                }
            }
        }
        public void ExecuteJob(string taskParam)
        {
            if (!string.IsNullOrEmpty(taskParam))
            {
                    WebClient wc = new WebClient();
                    wc.Encoding = System.Text.Encoding.UTF8;
                    wc.UseDefaultCredentials = true;
                    foreach (string url in taskParam.Split(','))
                    {
                        try
                        {
                            wc = new WebClient();
                            wc.Encoding = System.Text.Encoding.UTF8;
                            wc.UseDefaultCredentials = true;
                            wc.DownloadStringCompleted += wc_DownloadStringCompleted;
                            wc.DownloadStringAsync(new Uri(url));
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Debug(string.Format("异常信息>{0}\r\n", ex));
                        }
                    }
                }
          
        }
        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                LogHelper.Debug(string.Format("返回信息>{0}\r\n",e.Result));
            }
            catch (Exception ex)
            {
                LogHelper.Debug(string.Format("异常信息>{0}\r\n",  ex));
            }
        }
    }
}
