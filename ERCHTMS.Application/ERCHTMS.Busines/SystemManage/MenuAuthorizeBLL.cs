using BSFramework.Util.WebControl;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.IService.SystemManage;
using ERCHTMS.Service.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Busines.SystemManage
{
   public  class MenuAuthorizeBLL
    {
        private IMenuAuthorizeService service = new MenuAuthorizeService();
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagination">分页信息</param>
        /// <param name="queryJson">查询参数 {keyword:单位名称 } </param>
        /// <returns></returns>
        public IEnumerable<MenuAuthorizeEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void SaveForm(string keyValue, MenuAuthorizeEntity entity)
        {
             service.SaveForm(keyValue, entity);
        }

        public MenuAuthorizeEntity GetEntity(string keyValue)
        {
          return  service.GetEntity(keyValue);
        }

        public void RemoveForm(string keyValue)
        {
            service.RemoveForm(keyValue);
        }

        public MenuAuthorizeEntity GetEntityByDeptId(string departId)
        {
            return service.GetEntityByDeptId(departId);
        }

        public MenuAuthorizeEntity GetEntityByRegistCode(string registcode)
        {
            return service.GetEntityByRegistCode(registcode);
        }

        public void InsertEntity(params MenuAuthorizeEntity[] authorizeEntities)
        {

            service.InsertEntity(authorizeEntities);
        }

        public List<MenuAuthorizeEntity> GetListByRegistCode(string registcode)
        {
            return service.GetListByRegistCode(registcode);
        }

        /// <summary>
        /// 获取班组文化墙地址
        /// </summary>
        /// <param name="deptId">部门Id</param>
        /// <returns></returns>
        public string GetCultureUrl(string deptId)
        {
            return service.GetCultureUrl(deptId);
        }
    }
}
