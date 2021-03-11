using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERCHTMS.AppSerivce.Model
{
    public class AcceptData
    {
        public string problemid { get; set; }
        public string checktimes { get; set; }
        public string checkperson { get; set; }
        public string checktime { get; set; }
        public string checkopinion { get; set; }
        public string checkresult { get; set; }
        public IList<Photo> checkpics { get; set; }
    }
}