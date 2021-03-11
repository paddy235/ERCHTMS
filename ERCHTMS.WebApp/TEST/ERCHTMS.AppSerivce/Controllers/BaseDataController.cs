using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ERCHTMS.AppSerivce.Controllers
{
    public class BaseDataController : ApiController
    {
        ERCHTMS.Busines.SystemManage.DataItemDetailBLL diBll = new Busines.SystemManage.DataItemDetailBLL();
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public object Get(string name)
        {
            return diBll.GetItemValue(name);
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}