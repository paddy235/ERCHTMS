using ERCHTMS.Entity.LllegalManage;
using ERCHTMS.IService.LllegalManage;
using ERCHTMS.Service.LllegalManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.LllegalManage
{
    /// <summary>
    /// 描 述：违章整改延期信息表
    /// </summary>
    public class LllegalExtensionBLL
    {
        private LllegalExtensionIService service = new LllegalExtensionService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<LllegalExtensionEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public LllegalExtensionEntity GetEntity(string keyValue)
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
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, LllegalExtensionEntity entity)
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


        #region 获取最近的一组申请详情
        /// <summary>
        /// 获取最近的一组申请详情
        /// </summary>
        /// <param name="hidCode"></param>
        /// <returns></returns>
        public IList<LllegalExtensionEntity> GetListByCondition(string lllegalId)
        {
            try
            {
                return service.GetListByCondition(lllegalId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        /// <summary>
        /// 获取最新的一个整改申请对象
        /// </summary>
        /// <param name="hidCode"></param>
        /// <returns></returns>
        public LllegalExtensionEntity GetFirstEntity(string lllegalId)
        {
            try
            {
                return service.GetFirstEntity(lllegalId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}