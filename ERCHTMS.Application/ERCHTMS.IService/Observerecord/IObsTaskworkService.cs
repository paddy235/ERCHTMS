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
    public interface IObsTaskworkService
    {
        DataTable GetPageListJson(Pagination pagination, string queryJson);
        ObsTaskworkEntity GetEntity(string keyValue);
        IEnumerable<ObsTaskworkEntity> GetList();
        void RemoveForm(string keyValue);
        void SaveForm(string keyValue, ObsTaskworkEntity entity);
    }
}
