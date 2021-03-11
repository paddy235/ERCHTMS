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
    public class HistoryPeopleBLL
    {
        private HistoryPeopleIService service = new HistoryPeopleService();
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HistoryPeople GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
            /// <summary>
        /// 获取人员历史记录分页显示
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetHistoryPageList(Pagination pagination, string queryJson) {
            return service.GetHistoryPageList( pagination,  queryJson);
        }
    }
}
