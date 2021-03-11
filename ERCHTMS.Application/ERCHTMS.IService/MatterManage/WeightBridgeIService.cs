using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.IService.MatterManage
{
    interface WeightBridgeIService
    {
        /// <summary>
        /// 读取地磅系统数据
        /// </summary>
        /// <param name="carNo">车牌号</param>
        /// <returns></returns>
        DataTable GetBridgData(string carNo);
    }
}
