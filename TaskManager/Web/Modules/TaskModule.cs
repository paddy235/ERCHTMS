using System;
using Nancy;
using Nancy.ModelBinding;
using Utility.Filter;
using Utility.Quartz;
using System.IO;

namespace Web.Modules
{
    public class TaskModule : BaseModule
    {
        public TaskModule()
            : base("Task")
        {
            //任务列表
            Get["/Grid"] = r =>
            {
                return View["Grid"];
            };
            //任务编辑界面
            Get["/Edit"] = r =>
            {
                return View["Edit"];
            };

            #region "取数接口API"

            //立即运行一次任务
            Get["/Run/{Id}"] = r =>
            {
                //取出单条记录数据
                string TaskId = r.Id;
                ApiResult<string> result = new ApiResult<string>();
                try
                {
                    TaskHelper.RunById(TaskId);
                }
                catch (Exception ex)
                {
                    result.HasError = true;
                    result.Message = ex.Message;
                }
                return Response.AsJson(result);
            };

            Get["/GetById/{Id}"] = r =>
            {
                ApiResult<TaskUtil> result = new ApiResult<TaskUtil>();
                try
                {
                    //取出单条记录数据
                    string TaskId = r.Id;
                    result.Result = TaskHelper.GetById(TaskId);
                }
                catch (Exception ex)
                {
                    result.HasError = true;
                    result.Message = ex.Message;
                }
                return Response.AsJson(result);
            };

            //列表查询接口
            Post["/PostQuery"] = r =>
            {
                QueryCondition condition = this.Bind<QueryCondition>();
                return Response.AsJson(TaskHelper.Query(condition));
            };

            //保存数据
            Post["/"] = r =>
            {
                TaskUtil TaskUtil = this.Bind<TaskUtil>();
                return Response.AsJson(TaskHelper.SaveTask(TaskUtil));
            };
            //更新数据
            Put["/"] = r =>
            {
                TaskUtil TaskUtil = this.Bind<TaskUtil>();
                return Response.AsJson(TaskHelper.SaveTask(TaskUtil));
            };
            //删除任务接口
            Delete["/{Id}"] = r =>
            {
                ApiResult<string> result = new ApiResult<string>();
                try
                {
                    string TaskId = r.Id;
                    TaskHelper.DeleteById(TaskId);
                }
                catch (Exception ex)
                {
                    result.HasError = true;
                    result.Message = ex.Message;
                }
                return Response.AsJson(result);
            };

            //更新任务运行状态
            Put["/{Id}/{Status:int}"] = r =>
            {
                ApiResult<string> result = new ApiResult<string>();
                try
                {
                    TaskStatus Status = Enum.ToObject(typeof(TaskStatus), r.Status);
                    string TaskId = r.Id;
                    TaskHelper.UpdateTaskStatus(TaskId, Status);
                }
                catch (Exception ex)
                {
                    result.HasError = true;
                    result.Message = ex.Message;
                }
                return Response.AsJson(result);
            };
            #endregion
        }
    }
}
