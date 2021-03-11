using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.IService.RiskDatabase;
using ERCHTMS.Service.RiskDatabase;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.RiskDatabase
{
    /// <summary>
    /// 描 述：辨识评估计划相关联的机构和人员信息
    /// </summary>
    public class RiskPlanDataBLL
    {
        private RiskPlanDataIService service = new RiskPlanDataService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<RiskPlanDataEntity> GetList(int dataType,string planId)
        {
            return service.GetList(dataType, planId);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public RiskPlanDataEntity GetEntity(string keyValue)
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
        /// 根据计划ID删除数据
        /// </summary>
        /// <param name="planId">计划ID</param>
        public int Remove(string planId)
        {
            try
            {
              service.Remove(planId);
              return 1;
            }
            catch (Exception)
            {
                return 0;
                //throw;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, RiskPlanDataEntity entity)
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
        /// 批量保存
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public void Save(List<RiskPlanDataEntity> list)
        {
            try
            {
                service.Save(list);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}