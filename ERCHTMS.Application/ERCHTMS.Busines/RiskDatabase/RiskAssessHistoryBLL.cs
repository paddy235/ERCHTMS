using BSFramework.Util.WebControl;
using ERCHTMS.Entity.HazardsourceManage;
using ERCHTMS.IService.RiskDatabase;
using ERCHTMS.Service.RiskDatabase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Busines.RiskDatabase
{
    public class RiskAssessHistoryBLL
    {
        private RiskAssessHistoryIService service = new RiskAssessHistoryService();
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        public void SaveFrom(string keyValue, RiskAssessHistoryEntity entity) {
             service.SaveForm(keyValue, entity);
        }
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteBySql(string sql)
        {
            return service.ExecuteBySql(sql);

        }
        public int RemoveAssessHistory(string keyValue) {
            return service.Remove(keyValue);
        }
    }
}
