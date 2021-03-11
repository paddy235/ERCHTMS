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
    public class HistoryToolsBLL
    {
        private HistoryToolsIService service = new HistoryToolsService();

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HistoryToolsEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        public DataTable GetHistoryPageList(Pagination pagination, string queryJson) {
            return service.GetHistoryPageList(pagination, queryJson);
        }

        public HistoryToolsEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
