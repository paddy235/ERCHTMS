using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Service.OutsourcingProject;
using System.Data;

namespace ERCHTMS.Busines.OutsourcingProject
{
    public class HistoryAuditBLL
    {
        private HistoryAuditIService service = new HistoryAuditService();
        public HistoryAudit GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        public DataTable GetAuditRecList(string keyValue)
        {
            return service.GetAuditRecList(keyValue);
        }
        public DataTable GetHisAuditRecList(string keyValue)
        {
            return service.GetHisAuditRecList(keyValue);
        }
    }
}
