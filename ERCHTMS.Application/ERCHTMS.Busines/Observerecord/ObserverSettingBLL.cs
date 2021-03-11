using BSFramework.Util.WebControl;
using ERCHTMS.Entity.Observerecord;
using ERCHTMS.IService.Observerecord;
using ERCHTMS.Service.Observerecord;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Busines.Observerecord
{
    public class ObserverSettingBLL
    {
        private IObserverSettingService service = new ObserverSettingService();

        public List<ObserverSettingEntity> GetData()
        {
            return service.GetData();
        }

        public void Edit(List<ObserverSettingEntity> models)
        {
            service.Edit(models);
        }
    }
}
