using ERCHTMS.Entity.RiskDatabase;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.RiskDatabase
{
    /// <summary>
    /// 描 述：风险预知训练库
    /// </summary>
    public interface RisktrainlibIService
    {
        #region 获取数据
        DataTable GetPageListJson(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<RisktrainlibEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        RisktrainlibEntity GetEntity(string keyValue);

        /// <summary>
        /// 获取作业安全分析库
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        DataTable GetRisktrainlibList(string p);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// 删除来源风险库数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        bool DelRiskData();
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, RisktrainlibEntity entity, List<RisktrainlibdetailEntity> listMesures);
        void InsertRiskTrainLib(List<RisktrainlibEntity> RiskLib);

        void InsertImportData(List<RisktrainlibEntity> RiskLib, List<RisktrainlibdetailEntity> detailLib);
        #endregion


    }
}
