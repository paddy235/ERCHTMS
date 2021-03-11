using ERCHTMS.Entity.AuthorizeManage;
using ERCHTMS.IService.AuthorizeManage;
using ERCHTMS.Service.AuthorizeManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.AuthorizeManage
{
    /// <summary>
    /// 描 述：应用模块列表的列查看权限设置表
    /// </summary>
    public class ModuleListColumnAuthBLL
    {
        private ModuleListColumnAuthIService service = new ModuleListColumnAuthService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<ModuleListColumnAuthEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ModuleListColumnAuthEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataTable GetListByType(Pagination pagination, string queryJson)
        {
            return service.GetListByType(pagination, queryJson);
        }

        
        /// <summary>
        /// 获取对应的列表内容(通过模块id)
        /// </summary>
        /// <param name="moduleid"></param>
        /// <param name="userid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ModuleListColumnAuthEntity GetEntity(string moduleid, string userid, int type)  
        {
            return service.GetEntity(moduleid, userid, type);
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
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, ModuleListColumnAuthEntity entity)
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
        #endregion
    }
}