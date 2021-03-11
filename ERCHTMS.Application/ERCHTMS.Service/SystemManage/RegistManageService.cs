using BSFramework.Data.Repository;
using BSFramework.Util.Extension;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.IService.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Service.SystemManage
{
    public class RegistManageService : RepositoryFactory<RegistManageEntity>, IRegistManageService
    {
        public RegistManageEntity GetEntity(string registcode)
        {
           var datas = this.BaseRepository().IQueryable().Where(X => X.RegistCode == registcode).ToList();
            return datas.FirstOrDefault();
        }

        public RegistManageEntity GetForm(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        public List<RegistManageEntity> GetList(string keyword)
        {
            var query= this.BaseRepository().IQueryable();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(p => p.RegistCode.Contains(keyword));
            }
            return query.ToList();
        }

        public void RemoveRegistManage(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }

        public void SaveForm(string keyValue, RegistManageEntity entity)
        {
            if (string.IsNullOrWhiteSpace(keyValue))
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
            else
            {
                entity.Id = keyValue;
                this.BaseRepository().Update(entity);
            }
        }

    }
}
