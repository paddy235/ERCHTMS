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
   public class ObsTaskEHSService : RepositoryFactory<ObsTaskEHSEntity>, IObsTaskEHSService
    {
        #region 获取数据

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ObsTaskEHSEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        public void SaveForm(string keyValue, ObsTaskEHSEntity entity)
        {
            entity.ID = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {

                ObsTaskEHSEntity old = this.BaseRepository().FindEntity(keyValue);
                if (old == null)
                {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                }
                else
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }

            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion

    }
}
