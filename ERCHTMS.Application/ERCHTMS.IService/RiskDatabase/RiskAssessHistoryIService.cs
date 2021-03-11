using BSFramework.Util.WebControl;
using ERCHTMS.Entity.HazardsourceManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.IService.RiskDatabase
{
    public interface RiskAssessHistoryIService
    {
        DataTable GetPageList(Pagination pagination, string queryJson);
        IEnumerable<RiskAssessHistoryEntity> GetList();
        RiskAssessHistoryEntity GetEntity(string keyValue);
        void SaveForm(string keyValue, RiskAssessHistoryEntity entity);
        int ExecuteBySql(string sql);
        int Remove(string historyid);
    }
}
