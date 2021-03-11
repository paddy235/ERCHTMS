using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERCHTMS.AppSerivce.Model
{
    /// <summary>
    /// 整改对象
    /// </summary>
    public class ReformData
    {
        public string reformtimes { get; set; }
        public string dutydept { get; set; }
        public string problemid { get; set; } 
        public string dutyperson { get; set; }
        public string dutytel { get; set; }
        public string deadinetime { get; set; }
        public string reformmeasure { get; set; }
        public string reformdescribe { get; set; }
        public string reformfinishdate { get; set; }
        public string reformresult { get; set; }
        public string realitymanagecapital { get; set; }
        public string reformbackreason { get; set; }
        public string isback { get; set; }
        public IList<Photo> reformpics { get; set; } 
    }
}