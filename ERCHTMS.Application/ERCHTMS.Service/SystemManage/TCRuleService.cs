using BSFramework.Data.Repository;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.IService.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Service.SystemManage
{
    public class TCRuleService : RepositoryFactory<TCRuleEntity>, ITCRuleService
    {
        public void Delete(string keyValue)
        {
           
            this.BaseRepository().Delete(keyValue);
            var entity = GetEntity(keyValue);
            if (entity !=null)
            {
                //更新版本号
                UpdateVersion(entity.AuthorizCodeId);
            }
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="infotype">
        /// 数据类型 ： 1、主题 2、文化墙地址 3：首页地址
        /// </param>
        /// <param name="authId">授权表（BASE_MENUAUTHORIZE）的主键</param>
        /// <returns></returns>
        public List<TCRuleEntity> GetList(int infotype, string authId)
        {
            List<TCRuleEntity> data = new RepositoryFactory().BaseRepository().IQueryable<TCRuleEntity>(p => p.InfoType == infotype && p.AuthorizCodeId == authId).ToList();
            return data;
        }

        public TCRuleEntity GetEntity(string keyValue)
        {
            var entity = this.BaseRepository().FindEntity(keyValue);
            return entity;
        }

        public void Insert(TCRuleEntity entity)
        {
            entity.Create();
            this.BaseRepository().Insert(entity);
            UpdateVersion(entity.AuthorizCodeId);
        }

        public void Update(string keyValue, TCRuleEntity entity)
        {
            var oldEntity = this.BaseRepository().FindEntity(keyValue);
            if (oldEntity == null) throw new Exception("未找到要修改的数据");
            oldEntity.Modify(keyValue);
            oldEntity.InfoValue = entity.InfoValue;
            oldEntity.RuleIds = entity.RuleIds;
            oldEntity.RuleNames = entity.RuleNames;
            this.BaseRepository().Update(oldEntity);
            UpdateVersion(oldEntity.AuthorizCodeId);
        }




        private void UpdateVersion(string menuauthorizeId)
        {
            MenuAuthorizeEntity entity = new RepositoryFactory().BaseRepository().FindEntity<MenuAuthorizeEntity>(menuauthorizeId);
            entity.Modify(entity.Id);//更行版本号
            new RepositoryFactory().BaseRepository().Update(entity);
        }

        public List<TCRuleEntity> GetList(List<string> authIds)
        {
            List<TCRuleEntity> data = new RepositoryFactory().BaseRepository().IQueryable<TCRuleEntity>(p =>authIds.Contains(p.AuthorizCodeId)).ToList();
            return data;
        }
    }
}
