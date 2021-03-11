using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Service.OutsourcingProject;

namespace ERCHTMS.Busines.OutsourcingProject
{
    public class HistoryRecordDetailBLL
    {
        private HistoryRecordDetailIService service = new HistoryRecordDetailService();
        public HistoryRecordDetail GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
    }
}
