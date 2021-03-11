using BSFramework.Util.WebControl;
using ERCHTMS.Entity.RoutineSafetyWork;
using ERCHTMS.IService.RoutineSafetyWork;
using ERCHTMS.Service.RoutineSafetyWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Busines.RoutineSafetyWork
{
    public class AnnounDetailBLL
    {
        private AnnounDetailIService service = new AnnounDetailService();



        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        public IEnumerable<AnnounDetailEntity> GetList() {
            return service.GetList();
        }

        public AnnounDetailEntity GetEntity(string keyValue) {
            return service.GetEntity(keyValue);
        }
        public AnnounDetailEntity GetEntity(string UserId,string keyValue)
        {
            return service.GetEntity(UserId,keyValue);
        }
        public void RemoveForm(string keyValue) {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                
                throw;
            }
          
        }
        public void SaveForm(string keyValue, AnnounDetailEntity entity) {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }
    }
}
