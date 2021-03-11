using BSFramework.Data.Repository;
using ERCHTMS.Entity.RiskDatabase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.HighRiskWork;

namespace ERCHTMS.IService.HighRiskWork
{
    /// <summary>
    /// 描 述：高风险作业安全分析
    /// </summary>
    public interface HighRiskRecordIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<HighRiskRecordEntity> GetList(string workId);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        HighRiskRecordEntity GetEntity(string keyValue);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, HighRiskRecordEntity entity);

        /// <summary>
        /// 根据关联id删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveFormByWorkId(string workId);
        #endregion
    }
}
