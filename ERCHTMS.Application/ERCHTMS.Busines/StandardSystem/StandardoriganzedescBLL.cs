using ERCHTMS.Entity.StandardSystem;
using ERCHTMS.IService.StandardSystem;
using ERCHTMS.Service.StandardSystem;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.StandardSystem
{
    /// <summary>
    /// 描 述：标准化组织机构描述表
    /// </summary>
    public class StandardoriganzedescBLL
    {
        private StandardoriganzedescIService service = new StandardoriganzedescService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<StandardoriganzedescEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public StandardoriganzedescEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 根据标准化组织机构获取实体
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public StandardoriganzedescEntity GetEntityByType(string type)
        {
            return service.GetEntityByType(type);
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
        public void SaveForm(string keyValue, StandardoriganzedescEntity entity)
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
