using ERCHTMS.Entity.LaborProtectionManage;
using ERCHTMS.IService.LaborProtectionManage;
using ERCHTMS.Service.LaborProtectionManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.LaborProtectionManage
{
    /// <summary>
    /// 描 述：劳动防护发放表详情
    /// </summary>
    public class LaborissuedetailBLL
    {
        private LaborissuedetailIService service = new LaborissuedetailService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<LaborissuedetailEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public LaborissuedetailEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 根据物品表id获取最近一次发放记录
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public LaborissuedetailEntity GetOrderLabor(string keyValue)
        {
            return service.GetOrderLabor(keyValue);
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
        public void SaveListForm( string json)
        {
            try
            {
                service.SaveListForm( json);
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
        public void SaveForm(string keyValue, LaborissuedetailEntity entity, string json, string InfoId)
        {
            try
            {
                service.SaveForm(keyValue, entity,json,InfoId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
