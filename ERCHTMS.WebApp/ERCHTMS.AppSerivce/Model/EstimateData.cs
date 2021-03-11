using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERCHTMS.AppSerivce.Model
{
    public class EstimateData  
    {
        public string problemid { get; set; }
        public string evaluatetimes { get; set; }
        public string evaluateperson { get; set; }
        public string estimatedate { get; set; } 
        public string evaluateresult { get; set; }
        public IList<Photo> evaluatepics { get; set; }  
    }
}