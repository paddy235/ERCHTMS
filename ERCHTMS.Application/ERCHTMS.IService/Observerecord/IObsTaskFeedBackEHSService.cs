using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.Observerecord;

namespace ERCHTMS.IService.Observerecord
{
    public interface IObsTaskFeedBackEHSService
    {
        void SaveForm(string keyValue, ObsTaskFeedBackEHSEntity entity);
    }
}
