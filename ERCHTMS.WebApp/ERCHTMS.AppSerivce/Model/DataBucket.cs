using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERCHTMS.AppSerivce.Models
{
    public class BaseParam
    {
        public string UserId { get; set; }
        public string TokenId { get; set; }
        public bool AllowPaging { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }

    public class ModelParam<T> : BaseParam where T : class
    {
        public T Data { get; set; }
    }

    public class ListParam<T> : BaseParam where T : class
    {
        public List<T> Data { get; set; }
    }

    public class BaseResult
    {
        private bool success;
        private string message;
        public int code { get; set; }
        public string info { get; set; }
        public bool Success { get { return success; } set { success = value; code = value ? 0 : 1; } }
        public string Message { get { return message; } set { message = value; info = value; } }
    }

    public class ListResult<TModel> : BaseResult where TModel : class
    {
        public int Total { get; set; }
        public int count { get { return Total; } private set { } }
        public List<TModel> Data { get; set; }
        public List<TModel> data { get { return this.Data; } private set { this.Data = value; } }
    }

    public class ModelResult<TModel> : BaseResult
    {
        public TModel Data { get; set; }
    }
}