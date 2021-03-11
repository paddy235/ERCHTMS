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
    public class HistorySpecificToolsBLL
    {
        private HistorySpecificToolsIService service = new HistorySpecificToolsService();

        public DataTable GetHistoryPageList(Pagination pagination, string queryJson)
        {
            return service.GetHistoryPageList(pagination, queryJson);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HistorySpecificToolsEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        public HistorySpecificToolsEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            service.RemoveForm(keyValue);
        }
    }
}
