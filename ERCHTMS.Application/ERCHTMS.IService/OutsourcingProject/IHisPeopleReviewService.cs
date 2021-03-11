using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Util.WebControl;
using ERCHTMS.Entity.OutsourcingProject;

namespace ERCHTMS.IService.OutsourcingProject
{
    public interface IHisPeopleReviewService
    {
        DataTable GetHistoryPeopleList(Pagination pagination, string queryJson);

        HisPeopleReviewEntity GetEntity(string keyValue);
    }
}
