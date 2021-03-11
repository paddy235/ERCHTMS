using ERCHTMS.Entity.RiskDatabase;
using ERCHTMS.IService.RiskDatabase;
using ERCHTMS.Service.RiskDatabase;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.RiskDatabase
{
    /// <summary>
    /// 描 述：预知训练作业风险措施
    /// </summary>
    public class TrainmeasuresBLL
    {
        private TrainmeasuresIService service = new TrainmeasuresService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TrainmeasuresEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TrainmeasuresEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

         /// <summary>
        /// 根据作业Id获取列表
        /// </summary>
        /// <param name="workId">作业Id</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TrainmeasuresEntity> GetListByWorkId(string workId)
        {
            return service.GetListByWorkId(workId);
        }
        public DataTable GetPageListByWorkId(Pagination pagination, string queryJson)
        {
            return service.GetPageListByWorkId(pagination, queryJson);
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
        public void SaveForm(string keyValue, TrainmeasuresEntity entity)
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