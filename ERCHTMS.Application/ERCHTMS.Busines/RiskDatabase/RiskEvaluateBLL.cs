using BSFramework.Util.WebControl;
using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.IService.RiskDatabase;
using ERCHTMS.Service.RiskDatabase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Busines.RiskDatabase
{
    
    public class RiskEvaluateBLL
    {
        IRiskEvaluateService Service = new RiskEvaluateService();
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return Service.GetPageList(pagination,queryJson);
        }

        public IEnumerable<RiskEvaluate> GetList()
        {
            return Service.GetList();
        }
        public void RemoveForm(string keyValue)
        {
            try
            {
                Service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void SaveForm(string keyValue, RiskEvaluate entity)
        {
            try
            {
                Service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
