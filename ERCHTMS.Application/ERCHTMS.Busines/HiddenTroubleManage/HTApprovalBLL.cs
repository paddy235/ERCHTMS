using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.IService.HiddenTroubleManage;
using ERCHTMS.Service.HiddenTroubleManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：隐患评估信息表
    /// </summary>
    public class HTApprovalBLL
    {
        private HTApprovalIService service = new HTApprovalService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<HTApprovalEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public HTApprovalEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 通过隐患编码获取
        /// </summary>
        /// <param name="hidCode"></param>
        /// <returns></returns>
        public HTApprovalEntity GetEntityByHidCode(string hidCode)
        {
            return service.GetEntityByHidCode(hidCode);
        }

        /// <summary>
        /// 根据隐患编码获取Table
        /// </summary>
        /// <param name="hidCode"></param>
        /// <returns></returns>
        public DataTable GetDataTableByHidCode(string hidCode)
        {
            return service.GetDataTableByHidCode(hidCode);
        }

        public IEnumerable<HTApprovalEntity> GetHistoryList(string hidCode)
        {
            return service.GetHistoryList(hidCode);
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
        public void SaveForm(string keyValue, HTApprovalEntity entity)
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
