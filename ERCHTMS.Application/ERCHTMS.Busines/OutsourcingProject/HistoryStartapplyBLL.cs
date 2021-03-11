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
    public class HistoryStartapplyBLL
    {
        private HistoryStartapplyIService service = new HistoryStartapplyService();

        public DataTable GetHisPageList(Pagination pagination, string queryJson) {
            return service.GetHisPageList(pagination, queryJson);
        }
        public HistoryStartapplyEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        public DataTable GetApplyList(string keyValue)
        {
            return service.GetApplyList(keyValue);
        }
        public DataTable GetApplyInfo(string keyValue)
        {
            return service.GetApplyInfo(keyValue);
        }

        public void SaveForm(string keyValue, HistoryStartapplyEntity entity)
        {
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
