using ERCHTMS.Service.MatterManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Busines.MatterManage
{
    public class WeightBridgeBLL
    {
        WeightBridgeService service = new WeightBridgeService();
        
        /// <summary>
        /// 获取地磅数据
        /// </summary>
        /// <param name="carNo"></param>
        /// <returns></returns>
        public DataTable GetBridgData(string carNo)
        {
           return service.GetBridgData(carNo);
        }
    }
}
