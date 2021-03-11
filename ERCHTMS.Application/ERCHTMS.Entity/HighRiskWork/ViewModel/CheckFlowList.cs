using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.HighRiskWork.ViewModel
{
    /// <summary>
    /// 带签名图片列表
    /// </summary>
    public class CheckFlowList
    {
        public string auditdate { get; set; }
        public string auditdeptname { get; set; }
        public string auditusername { get; set; }
        public string auditstate { get; set; }
        public string auditremark { get; set; }
        //是否当前操作
        public string isoperate { get; set; }//0:否 1:是 是否正在处理
        public string isapprove { get; set; }//0:否 1:是  //是否已经处理过

        public string signpic { get; set; }//签名
    }
}
