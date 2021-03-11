using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.Observerecord;

namespace ERCHTMS.IService.Observerecord
{
    public interface IObsTaskEHSService
    {
        ObsTaskEHSEntity GetEntity(string keyValue);
        void SaveForm(string keyValue, ObsTaskEHSEntity entity);
    }
}
