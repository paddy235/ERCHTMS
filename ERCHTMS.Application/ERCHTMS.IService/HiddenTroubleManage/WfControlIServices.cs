using ERCHTMS.Entity.HiddenTroubleManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.IService.HiddenTroubleManage
{
    public  interface WfControlIServices
    {
        WfControlResult GetWfControlObject(WfControlObj obj);
    }
}
