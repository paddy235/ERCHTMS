using BSFramework.Data.Repository;
using ERCHTMS.Entity.RiskDatabase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.IService.RiskDatabase
{
    /// <summary>
    /// 描 述：风险预知训练库管控措施
    /// </summary>
    public interface RisktrainlibdetailIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<RisktrainlibdetailEntity> GetList(string workId);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        RisktrainlibdetailEntity GetEntity(string keyValue);
        DataTable GetTrainLibDetail(string workId);
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
        void SaveForm(string keyValue, RisktrainlibdetailEntity entity);
        void InsertRiskTrainDetailLib(List<RisktrainlibdetailEntity> detailLib);
        #endregion
    }
}
