using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Entity.Observerecord;

namespace ERCHTMS.IService.Observerecord
{
    public interface IObsTaskService
    {
        DataTable GetPageList(Pagination pagination, string queryJson);
        IEnumerable<ObsTaskEntity> GetList(string queryJson);
        ObsTaskEntity GetEntity(string keyValue);
        DataTable GetPlanById(string planId, string planFjId, string month);
        void RemoveForm(string keyValue);
        void SaveForm(string keyValue, ObsTaskEntity entity);
        bool CommitEhsData(Operator currUser);
        DataTable GetObsRecordIsExist(string planId, string planfjid, string year);
        void InsertImportData(List<ObsTaskEntity> obsplan, List<ObsTaskworkEntity> obsplanwork);
        void InsertImportData(List<ObsTaskEHSEntity> obsplan, List<ObsTaskworkEntity> obsplanwork);
        bool CopyHistoryData(Operator currUser, string oldYear, string newYear);
        void SynchData(string planid, string planfjid);
    }
}
