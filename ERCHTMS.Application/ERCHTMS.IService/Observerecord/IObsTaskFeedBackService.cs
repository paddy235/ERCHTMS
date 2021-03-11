using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.Observerecord;

namespace ERCHTMS.IService.Observerecord
{
    public interface IObsTaskFeedBackService
    {
        void SaveForm(string keyValue, ObsTaskFeedBackEntity entity);
    }
}
