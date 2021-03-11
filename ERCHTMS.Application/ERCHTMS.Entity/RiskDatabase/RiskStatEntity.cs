using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.RiskDatabase
{
    public class RiskStatEntity
    {
        public string name{ set;get;}
        public int lev1 { set; get; }
        public int lev2 { set; get; }
        public int lev3 { set; get; }
        public int lev4 { set; get; }
        public int sum { set; get; }
        public decimal percent { set; get; }
    }
}
