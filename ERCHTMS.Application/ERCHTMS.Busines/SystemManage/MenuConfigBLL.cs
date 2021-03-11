using BSFramework.Util.WebControl;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.IService.SystemManage;
using ERCHTMS.Service.SystemManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Busines.SystemManage
{
   public class MenuConfigBLL
    {
        private IMenuConfigService service = new MenuConfigService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="deptId">电厂Id</param>
        /// <param name="paltformType">平台类型 0-window终端 1-Android终端 2-手机APP </param>
        /// <returns>返回列表</returns>
        public IEnumerable<MenuConfigEntity> GetList(string deptId, int? paltformType = null, List<string> roleId = null)
        {
            return service.GetList(deptId,paltformType, roleId);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagination">分页信息</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public object GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public MenuConfigEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetPageDataTable(Pagination pagination, string queryJson)
        {
                return service.GetPageDataTable(pagination, queryJson);
        }

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, MenuConfigEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// 查询菜单列表
        /// </summary>
        /// <param name="moduleIds">菜单id 的集合</param>
        /// <param name="platform">2 手机APP 1 安卓终端 0 windwos</param>
        /// <param name="keyword">菜单名称  查询关键字</param>
        /// <returns></returns>
        public List<MenuConfigEntity> GetListByModuleIds(List<string> moduleIds, int? platform=null, string keyword=null)
        {
            return service.GetListByModuleIds(moduleIds, platform, keyword);
        }

        public List<MenuConfigEntity> GetAllList()
        {
            return service.GetAllList();
        }
        #endregion
    }
}
