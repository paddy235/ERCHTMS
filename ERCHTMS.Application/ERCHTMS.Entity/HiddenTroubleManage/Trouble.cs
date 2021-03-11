using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.HiddenTroubleManage
{
    public class Trouble
    {
        public string ID { get; set; }
        public string FINDDATE { get; set; }
        public string HIDDESCRIBE { get; set; }

        public string CHANGEMEASURE { get; set; }
    }


    public class TroubleStatists
    {
        public int numbers { get; set; }
        public string encode { get; set; }
        public string fullname { get; set; }

        public  List<TroubleStatistsDetail> detail { get; set; }
    }

    public class TroubleStatistsDetail
    {
        public int numbers { get; set; }
        public string encode { get; set; }
        public string fullname { get; set; }

    }
}
