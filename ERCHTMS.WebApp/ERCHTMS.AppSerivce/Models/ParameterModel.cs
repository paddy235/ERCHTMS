using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERCHTMS.AppSerivce.Models
{
    public class ParameterModel
    {
        public string UserId { get; set; }
    }

    public class ParameterModel<TModel> : ParameterModel
    {
        public TModel Data { get; set; }
    }

    public class PaginationModel<TModel> : ParameterModel<TModel> where TModel : class
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }

    public class ResultModel
    {
        public bool Success { get; set; }
        public string info { get; set; }
        public int code
        {
            get { return Success == false ? -1 : 0; }
            private set { }
        }
        public string message { get; set; }
    }

    public class ResultModel<TModel> : ResultModel where TModel : class, new()
    {
        public TModel data { get; set; }
    }

    public class ListModel<TModel> : ResultModel where TModel : class, new()
    {
        public int Total { get; set; }
        public List<TModel> data { get; set; }
    }
}