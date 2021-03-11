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
    public class HistoryRecordBLL
    {
        private HistoryRecordIService service = new HistoryRecordService();
        private IHisPeopleReviewService peopleService = new HisPeopleReviewService();
        /// <summary>
        /// 获取历史记录列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public DataTable GetHistoryPageList(Pagination pagination, string queryJson)
        {
            return service.GetHistoryPageList(pagination, queryJson);
        }
        public DataTable GetHistoryPeopleList(Pagination pagination, string queryJson)
        {
            return peopleService.GetHistoryPeopleList(pagination, queryJson);
        }
        public HisPeopleReviewEntity GetPeopleReviewEntity(string keyValue)
        {
            return peopleService.GetEntity(keyValue);
        }
        public HistoryRecord GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
    }
}
