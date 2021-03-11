using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.SaftyCheck
{
   public class DistinctArray
    {
        public string checkexcelname { get; set; }
        public string checkexcelid { get; set; }
       
        public object checkcontents { get; set; }
        //检查记录字段
        public string checkname { get; set; }
        public string checktime { get; set; }
        public string starttime { get; set; }
        public string endtime { get; set; }
        public string checkid { get; set; }
        public string checkperson { get; set; }
        public string checkpersonid { get; set; }
        public string checktype { get; set; }
        public string checklevel { get; set; }
        public string checklevelid { get; set; }
        public string checkleader { get; set; }

        public string areaname { get; set; }
        public string areanameid { get; set; }
        public string areanamecode { get; set; }
       
        public string disreictchargeperson { get; set; }
        public string disreictchargepersonid { get; set; }
        public string chargedept { get; set; }
        public string chargedeptcode { get; set; }
        public string linktel { get; set; }

        public object riskdescarray { get; set; }

    }
}
