using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Service.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.OutsourcingProject
{
    /// <summary>
    /// 描 述：审查记录明细表
    /// </summary>
    public class InvestigateDtRecordBLL
    {
        private InvestigateDtRecordIService service = new InvestigateDtRecordService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="InvestigateRecordId">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<InvestigateDtRecordEntity> GetList(string InvestigateRecordId) 
        {
            return service.GetList(InvestigateRecordId);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public InvestigateDtRecordEntity GetEntity(string keyValue)
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
        public void SaveForm(string keyValue, InvestigateDtRecordEntity entity)
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