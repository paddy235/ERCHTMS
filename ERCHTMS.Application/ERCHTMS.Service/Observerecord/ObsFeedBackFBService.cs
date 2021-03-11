using BSFramework.Data.Repository;
using ERCHTMS.Entity.Observerecord;
using ERCHTMS.IService.Observerecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Service.Observerecord
{
    public class ObsFeedBackFBService : RepositoryFactory<ObsFeedBackFBEntity>, ObsFeedBackFBIService
    {
        public ObsFeedBackFBEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        public void SaveForm(string keyValue, ObsFeedBackFBEntity entity)
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
