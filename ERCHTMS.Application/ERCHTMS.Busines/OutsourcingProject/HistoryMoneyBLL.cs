using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Util.WebControl;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Service.OutsourcingProject;

namespace ERCHTMS.Busines.OutsourcingProject
{
    public class HistoryMoneyBLL
    {
        private HistoryMoneyIService service = new HistoryMoneyService();
        /// <summary>
        /// 获取历史记录
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetHisPageListJson(Pagination pagination, string queryJson) {
            return service.GetHisPageList(pagination, queryJson);
        }
          /// <summary>
        /// 获取历史记录详情
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        public HistoryMoneyEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
    }
}
