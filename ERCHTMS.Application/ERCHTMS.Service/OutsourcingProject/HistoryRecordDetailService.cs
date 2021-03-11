using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Data.Repository;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;

namespace ERCHTMS.Service.OutsourcingProject
{
    public class HistoryRecordDetailService : RepositoryFactory<HistoryRecordDetail>, HistoryRecordDetailIService
    {
        public HistoryRecordDetail GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }

        public void SaveForm(string keyValue, HistoryRecordDetail entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
    }
}
