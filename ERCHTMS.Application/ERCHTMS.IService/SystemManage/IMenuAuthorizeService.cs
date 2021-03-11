using BSFramework.Util.WebControl;
using ERCHTMS.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.IService.SystemManage
{
    public interface IMenuAuthorizeService
    {
        IEnumerable<MenuAuthorizeEntity> GetPageList(Pagination pagination, string queryJson);
        void SaveForm(string keyValue, MenuAuthorizeEntity entity);
        MenuAuthorizeEntity GetEntity(string keyValue);
        void RemoveForm(string keyValue);
        MenuAuthorizeEntity GetEntityByDeptId(string departId);
        MenuAuthorizeEntity GetEntityByRegistCode(string registcode);
        void InsertEntity(MenuAuthorizeEntity[] authorizeEntities);
        List<MenuAuthorizeEntity> GetListByRegistCode(string registcode);
        string GetCultureUrl(string deptId);
    }
}
