using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERCHTMS.Busines.HighRiskWork;
using FluentScheduler;

namespace ERCHTMS.Web.TaskManage
{
    //任务对象
    public class ExaminJob : IJob
    {
        void IJob.Execute()
        {
            try
            {
                new StaffInfoBLL().SendTaskInfo();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}