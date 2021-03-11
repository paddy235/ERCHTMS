﻿using ERCHTMS.Entity.AuthorizeManage;
using ERCHTMS.IService.AuthorizeManage;
using BSFramework.Data.Repository;
using BSFramework.Util.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Service.AuthorizeManage
{
    /// <summary>
    /// 描 述：系统表单实例
    /// </summary>
    public class ModuleFormInstanceService : RepositoryFactory,IModuleFormInstanceService
    {
        #region 获取数据
        /// <summary>
        /// 获取一个实体类
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ModuleFormInstanceEntity GetEntityByObjectId(string objectId)
        {
            try
            {
                var expression = LinqExtensions.True<ModuleFormInstanceEntity>();
                expression = expression.And(t => t.ObjectId.Equals(objectId));
                return this.BaseRepository().FindEntity<ModuleFormInstanceEntity>(expression);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 保存一个实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int SaveEntity(string keyValue, ModuleFormInstanceEntity entity)
        {
            try
            {
                if (string.IsNullOrEmpty(keyValue))
                {
                    entity.Create();
                    return this.BaseRepository().Insert(entity);
                }
                else
                {
                    entity.Modify(keyValue);
                    return this.BaseRepository().Update(entity);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}
