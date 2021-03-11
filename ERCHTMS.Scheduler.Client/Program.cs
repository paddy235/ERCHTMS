using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Scheduler.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0) return;

            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info(args[0]);
            try
            {
                if (args[0] == "旁站监督计划")
                {
                    var isstart = ConfigurationManager.AppSettings["isstart"].ToString();
                    if (isstart == "1")
                    {
                        var api = ConfigurationManager.AppSettings["erchtmsapi"].ToString();
                        var url = string.Format("{0}/SyncData/BulidTask", api);
                        var webclient = new WebClientPro();
                        var values = new System.Collections.Specialized.NameValueCollection();
                        webclient.UploadValues(url, values);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error("计划执行程序异常，{0}", e.Message);
            }
        }
    }
}
