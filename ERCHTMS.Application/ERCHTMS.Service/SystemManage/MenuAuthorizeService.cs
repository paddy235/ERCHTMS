using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.IService.SystemManage;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Service.SystemManage
{
    public class MenuAuthorizeService : RepositoryFactory<MenuAuthorizeEntity>, IMenuAuthorizeService
    {
        /// <summary>
        /// 获取班组文化墙地址
        /// </summary>
        /// <param name="deptId">部门Id</param>
        /// <returns></returns>
        public string GetCultureUrl(string deptId)
        {
            var dept = new RepositoryFactory().BaseRepository().IQueryable<DepartmentEntity>(p => p.DepartmentId == deptId).FirstOrDefault();
            if (dept == null) return null;
            var orgId = dept.OrganizeId; 
            if (string.IsNullOrEmpty(orgId)) return null;
            var entity = this.BaseRepository().IQueryable(p => p.DepartId == orgId).FirstOrDefault();
            if (entity == null) return null;
            return entity.CulturalUrl ?? null;
        }

        public MenuAuthorizeEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        public MenuAuthorizeEntity GetEntityByDeptId(string departId)
        {
            var expression = LinqExtensions.True<MenuAuthorizeEntity>();
            if (!string.IsNullOrWhiteSpace(departId))
            {
                expression = expression.And(x => x.DepartId == departId);
            }
            return this.BaseRepository().FindEntity(expression);
        }

        public MenuAuthorizeEntity GetEntityByRegistCode(string registcode)
        {
            var expression = LinqExtensions.True<MenuAuthorizeEntity>();
            expression = expression.And(x => x.RegistCode == registcode);
            return this.BaseRepository().FindEntity(expression);
        }

        public List<MenuAuthorizeEntity> GetListByRegistCode(string registcode)
        {
        var query=    this.BaseRepository().IQueryable().ToList();
            //var query = new RepositoryFactory().BaseRepository().IQueryable<MenuAuthorizeEntity>();
            if (!string.IsNullOrWhiteSpace(registcode))
            {
                query = query.Where(p => p.RegistCode == registcode).ToList();
            }
            return query.ToList();
        }

        /// <summary>
        /// 分页获取授权信息
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public IEnumerable<MenuAuthorizeEntity> GetPageList(Pagination pagination, string queryJson)
        {
            var expression = LinqExtensions.True<MenuAuthorizeEntity>();
            if (!string.IsNullOrWhiteSpace(queryJson))
            {
                JObject jsondata = queryJson.ToJObject();
                if (!jsondata["keyword"].IsEmpty())
                {
                    string keyword = jsondata["keyword"].ToString();
                    expression = expression.And(p => p.DepartName.Contains(keyword));
                }
                if (!jsondata["departId"].IsEmpty())
                {
                    string departId = jsondata["departId"].ToString();
                    expression = expression.And(p => p.DepartId.Contains(departId));
                }
            }
                return this.BaseRepository().FindList(expression, pagination);
        }

        public void InsertEntity(MenuAuthorizeEntity[] authorizeEntities)
        {
            List<MenuAuthorizeEntity> list = authorizeEntities.ToList();
            this.BaseRepository().Insert(list);
        }

        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="entity">实体</param>
        public void SaveForm(string keyValue, MenuAuthorizeEntity entity)
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
