using BSFramework.Util.WebControl;
using ERCHTMS.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.IService.SystemManage
{
  public  interface IMenuConfigService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="deptId">电厂Id</param>
        /// <param name="paltformType">平台类型</param>
        /// <returns>返回列表</returns>
        IEnumerable<MenuConfigEntity> GetList(string deptId, int? paltformType = null, List<string> roleId = null);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        MenuConfigEntity GetEntity(string keyValue);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pagination">分页信息</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        object GetPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, MenuConfigEntity entity);
        DataTable GetPageDataTable(Pagination pagination, string queryJson);
      List<MenuConfigEntity> GetListByModuleIds(List<string> moduleIds, int? platform, string keyword = null);
        List<MenuConfigEntity> GetAllList();

        #endregion
    }
}
