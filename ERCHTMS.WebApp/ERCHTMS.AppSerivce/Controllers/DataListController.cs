using BSFramework.Util.WebControl;
using ERCHTMS.AppSerivce.Models;
using ERCHTMS.Busines.AccidentEvent;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.AccidentEvent;
using ERCHTMS.Entity.SystemManage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ERCHTMS.AppSerivce.Controllers
{
    public class DataListController : BaseApiController
    {
        [HttpPost]
        public ListResult<DataItemDetailEntity> GetList(ModelParam<string> modelParam)
        {
            DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
            var data = dataItemDetailBLL.ListByCode(modelParam.Data);
            return new ListResult<DataItemDetailEntity> { Success = true, Data = data };
        }

        [HttpPost]
        public List<DataItemDetailEntity> List(ModelParam<string> modelParam)
        {
            DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
            var data = dataItemDetailBLL.GetListItems(modelParam.Data).ToList();
            return data;
        }

        [HttpPost]
        public DataItemDetailEntity GetValue(string name)
        {
            DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
            var data = dataItemDetailBLL.GetEntityByItemName(name);
            return data;
        }
    }
}
