using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Util.WebControl;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Service.OutsourcingProject;

namespace ERCHTMS.Busines.OutsourcingProject
{
    public class HisReturnWorkBLL
    {
        private HisReturnWorkIService service = new HisReturnWorkService();

        public HisReturnWorkEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        public DataTable GetHistoryPageList(Pagination pagination, string queryJson)
        {
            return service.GetHistoryPageList(pagination, queryJson);
        }
    }
}
