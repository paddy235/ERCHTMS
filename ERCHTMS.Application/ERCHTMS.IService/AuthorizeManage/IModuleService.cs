using ERCHTMS.Entity.AuthorizeManage;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.AuthorizeManage
{
    /// <summary>
    /// 描 述：系统功能
    /// </summary>
    public interface IModuleService
    {
        #region 获取数据
        /// <summary>
        /// 获取最大编号
        /// </summary>
        /// <returns></returns>
        int GetSortCode();
        /// <summary>
        /// 功能列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<ModuleEntity> GetList();
        IEnumerable<ModuleEntity> GetListBySql(string sql);
        DataTable GetModuleIds();
        /// <summary>
        /// 功能实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        ModuleEntity GetEntity(string keyValue);
        /// <summary>
        /// 根据模块地址获取模块ID
        /// </summary>
        /// <param name="enCode">模块编码</param>
        /// <returns></returns>
        string GetModuleIdByCode(string enCode);

          /// <summary>
        /// 根据模块编码获取实体
        /// </summary>
        /// <param name="enCode">功能编号</param>
        /// <returns></returns>
        ModuleEntity GetEntityByCode(string enCode);
        #endregion

        #region 验证数据
        /// <summary>
        /// 功能编号不能重复
        /// </summary>
        /// <param name="enCode">编号</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        bool ExistEnCode(string enCode, string keyValue);
        /// <summary>
        /// 功能名称不能重复
        /// </summary>
        /// <param name="fullName">名称</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        bool ExistFullName(string fullName, string keyValue);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除功能
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="moduleEntity">功能实体</param>
        /// <param name="moduleButtonList">按钮实体列表</param>
        /// <param name="moduleColumnList">视图实体列表</param>
        /// <returns></returns>
        void SaveForm(string keyValue, ModuleEntity moduleEntity, List<ModuleButtonEntity> moduleButtonList, List<ModuleColumnEntity> moduleColumnList);
        #endregion
    }
}
