using System;
using Quartz;

namespace TopShelf_WindowsService.QuartzJobs
{
    public sealed class BankCardJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            NLog.LogManager.GetCurrentClassLogger().Debug("代扣成功！");
        }
    }
}
