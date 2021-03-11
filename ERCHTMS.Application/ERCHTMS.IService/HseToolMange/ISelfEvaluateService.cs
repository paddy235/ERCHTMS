using ERCHTMS.Entity.HseToolMange;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.IService.HseToolMange
{
    public interface ISelfEvaluateService
    {
        IEnumerable<SelfEvaluateEntity> GetList(string userid, string deptCode, string keyword,string year,string month);
        void SaveForm(SelfEvaluateEntity entity);

        void RemoveForm(string id);

        SelfEvaluateEntity GetEntity(string id);

        EvaluateGroupSummaryEntity GetSummary(string year, string month);
        void SaveSummary(EvaluateGroupSummaryEntity entity);
        DataTable GetChartsData(string year, string month, string organizeCode);
        List<string> GetSubmitByDeptCode(string deptCode, string year, string month);
        EvaluateGroupSummaryEntity GetSummaryById(string id);
        DataTable GetDangerCount(string deptCode, string year, string month);
        DataTable GetPPECount(string deptCode, string year, string month);
        DataTable GetHseCount(string deptCode, string year, string month);
        DataTable GetSafeCount(string deptCode, string year, string month);
        DataTable GetFiveData(string deptCode, string year, string month, int type);
        EvaluateGroupSummaryEntity GetSummary(string year, string month, string deptId);
        decimal GetPeopleCount(string departmentId);
        decimal GetCycle(string category);
        decimal Times(string category);
    }
}
