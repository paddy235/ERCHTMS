using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ERCHTMS.Entity.HighRiskWork;

namespace ERCHTMS.AppSerivce.Model
{
    public class WarningCardModel
    {
        public string DeptId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string Key { get; set; }
    }
}