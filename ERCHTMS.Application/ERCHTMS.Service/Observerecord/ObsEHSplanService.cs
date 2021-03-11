using ERCHTMS.Entity.Observerecord;
using ERCHTMS.IService.Observerecord;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data;
using ERCHTMS.Code;
using System.Text;
using System;

namespace ERCHTMS.Service.Observerecord
{
    /// <summary>
    /// 描 述：观察计划
    /// </summary>
    public class ObsEHSplanService : RepositoryFactory<ObsplanEHSEntity>, ObsplanEHSIService
    {
        #region 获取数据
       
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ObsplanEHSEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        public void SaveForm(string keyValue, ObsplanEHSEntity entity)
        {
            entity.ID = keyValue;
            if (!string.IsNullOrEmpty(keyValue))
            {

                ObsplanEHSEntity old = this.BaseRepository().FindEntity(keyValue);
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
