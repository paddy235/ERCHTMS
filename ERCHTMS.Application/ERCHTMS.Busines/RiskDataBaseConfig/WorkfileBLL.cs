using ERCHTMS.Entity.RiskDataBaseConfig;
using ERCHTMS.IService.RiskDataBaseConfig;
using ERCHTMS.Service.RiskDataBaseConfig;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.RiskDataBaseConfig
{
    /// <summary>
    /// 描 述：工作任务清单说明表
    /// </summary>
    public class WorkfileBLL
    {
        private WorkfileIService service = new WorkfileService();

        #region 获取数据
        public DataTable GetPageList(Pagination pagination, string queryJson) {
            return service.GetPageList(pagination,queryJson);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<WorkfileEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public WorkfileEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 根据机构Code查询本机构是否已经添加
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public bool GetIsExist(string orgCode)
        {
            return service.GetIsExist(orgCode);
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
        public void SaveForm(string keyValue, WorkfileEntity entity)
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
