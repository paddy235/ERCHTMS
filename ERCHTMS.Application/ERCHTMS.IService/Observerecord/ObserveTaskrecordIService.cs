using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Util.WebControl;
using ERCHTMS.Entity.Observerecord;

namespace ERCHTMS.IService.Observerecord
{
    public interface ObserveTaskrecordIService
    {
        DataTable GetPageList(Pagination pagination, string queryJson);
        DataTable GetTable(string sql);
        IEnumerable<ObserveTaskrecordEntity> GetList();
        DataTable GetObsTypeData(string keyValue);
        ObserveTaskrecordEntity GetEntity(string keyValue);
        string GetSafetyStat(string deptCode, string year, string quarter, string month);
        string GetUntiDbStat(string deptCode, string issafety, string year, string quarter, string month);
        string GetQsStat(string deptCode, string year);
        bool GetObsRecordByPlanIdAndFjId(string planid, string planfjid);
        void RemoveForm(string keyValue);
        void SaveForm(string keyValue, ObserveTaskrecordEntity entity, List<ObserveTaskcategoryEntity> listCategory, List<ObserveTasksafetyEntity> safetyList);
    }
}
