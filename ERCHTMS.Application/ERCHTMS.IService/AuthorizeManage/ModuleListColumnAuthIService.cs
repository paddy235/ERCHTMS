using ERCHTMS.Entity.AuthorizeManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.AuthorizeManage
{
    /// <summary>
    /// 描 述：应用模块列表的列查看权限设置表
    /// </summary>
    public interface ModuleListColumnAuthIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<ModuleListColumnAuthEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        ModuleListColumnAuthEntity GetEntity(string keyValue);

        
        /// <summary>
        /// 获取对应的列表内容(通过模块id)
        /// </summary>
        /// <param name="moduleid"></param>
        /// <param name="userid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        ModuleListColumnAuthEntity GetEntity(string moduleid, string userid, int type);

                /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        DataTable GetListByType(Pagination pagination, string queryJson);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, ModuleListColumnAuthEntity entity);
        #endregion
    }
}