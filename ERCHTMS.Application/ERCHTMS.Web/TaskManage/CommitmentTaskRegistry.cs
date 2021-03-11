using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERCHTMS.Busines.SystemManage;
using FluentScheduler;

namespace ERCHTMS.Web.TaskManage
{
    public class CommitmentTaskRegistry : Registry
    {
        private DataItemDetailBLL datadetailbll = new DataItemDetailBLL();
        public CommitmentTaskRegistry()
        {
            try
            {
                //var result = datadetailbll.GetItemValue("是否启动定时服务");
                //if (result == "1")//启动定时服务
                //{
                //    var timeresult = datadetailbll.GetItemValue("定时服务时间");
                //    int hour = 0, min = 00;
                //    if (!string.IsNullOrEmpty(timeresult))
                //    {
                //        hour = Convert.ToInt32(timeresult.Split(':')[0].ToString());
                //        min = Convert.ToInt32(timeresult.Split(':')[1].ToString());
                //    }
                //    //第一波
                //    Schedule<ExaminJob>().ToRunEvery(1).Days().At(hour, min); //每天6点30分执行 
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}