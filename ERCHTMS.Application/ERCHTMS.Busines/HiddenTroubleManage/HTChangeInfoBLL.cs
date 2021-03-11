using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.IService.HiddenTroubleManage;
using ERCHTMS.Service.HiddenTroubleManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：隐患整改信息表
    /// </summary>
    public class HTChangeInfoBLL
    {
        private HTChangeInfoIService service = new HTChangeInfoService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HTChangeInfoEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HTChangeInfoEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="hidCode"></param>
        /// <returns></returns>
        public HTChangeInfoEntity GetEntityByHidCode(string hidCode)
        {
            try
            {
                return service.GetEntityByHidCode(hidCode);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<HTChangeInfoEntity> GetHistoryList(string hidCode)
        {
            return service.GetHistoryList(hidCode);
        }

        /// <summary>
        /// 根据 Code获取整改实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public HTChangeInfoEntity GetEntityByCode(string keyValue)
        {
            return service.GetEntityByCode(keyValue);
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
        public void SaveForm(string keyValue, HTChangeInfoEntity entity)
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

        public void RemoveFormByCode(string hidcode)
        {
            try
            {
                service.RemoveFormByCode(hidcode);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        #endregion
    }
}
