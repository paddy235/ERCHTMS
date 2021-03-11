using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERCHTMS.AppSerivce.Model
{
    public class SignData
    {
        public string signid { get; set; }
        public string supervisetime { get; set; }
        public string supervisestate { get; set; }
        public string supervisetype { get; set; }
        public IList<Photo> signfile { get; set; }
    }
}