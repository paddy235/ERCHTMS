using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERCHTMS.AppSerivce.Model
{
    public class ApprovalData
    {
        public string approvaltimes { get; set; }
        public string problemid { get; set; }
        public string approvalpersonid { get; set; }
        public string approvalperson { get; set; }
        public string approvaldepartcode { get; set; }
        public string approvaldepartname { get; set; }
        public string approvaldate { get; set; }
        public string approvalresult { get; set; }
        public string approvalreason { get; set; }
        public IList<Photo> approvalpics { get; set; }
    }
}