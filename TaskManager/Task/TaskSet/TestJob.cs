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
    public class TestJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
             LogHelper.Debug("这是测试任务");
        }
    }
}
