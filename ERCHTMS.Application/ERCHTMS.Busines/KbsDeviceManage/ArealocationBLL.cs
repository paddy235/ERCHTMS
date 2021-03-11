using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.IService.KbsDeviceManage;
using ERCHTMS.Service.KbsDeviceManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using ERCHTMS.Entity.CarManage;

namespace ERCHTMS.Busines.KbsDeviceManage
{
    /// <summary>
    /// 描 述：区域定位表
    /// </summary>
    public class ArealocationBLL
    {
        private ArealocationIService service = new ArealocationService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<ArealocationEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取所有区域表及关联坐标
        /// </summary>
        /// <returns></returns>
        public List<KbsAreaLocation> GetTable()
        {
            return service.GetTable();
        }

        /// <summary>
        /// 获取所有区域(风险用)
        /// </summary>
        /// <returns></returns>
        public List<KbsAreaColor> GetRiskTable()
        {
            return service.GetRiskTable();
        }

        /// <summary>
        /// 获取隐患数量
        /// </summary>
        /// <returns></returns>
        public List<AreaHiddenCount> GetHiddenCount()
        {
            return service.GetHiddenCount();
        }

        /// <summary>
        /// 获取所有区域表及关联坐标(一级区域)
        /// </summary>
        /// <returns></returns>
        public List<KbsAreaLocation> GetOneLevelTable()
        {
            return service.GetOneLevelTable();
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ArealocationEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, ArealocationEntity entity)
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
