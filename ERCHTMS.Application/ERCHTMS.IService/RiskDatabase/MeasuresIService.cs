using ERCHTMS.Entity.RiskDatabase;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.RiskDatabase
{
    /// <summary>
    /// 描 述：风险管控措施表
    /// </summary>
    public interface MeasuresIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<MeasuresEntity> GetList(string queryJson);
        /// <summary>
        ///获取列表
        /// </summary>
        /// <param name="areaId">区域Id</param>
        /// <param name="riskId">风险库记录ID</param>
        /// <returns></returns>
        IEnumerable<MeasuresEntity> GetList(string areaId,string riskId);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        MeasuresEntity GetEntity(string keyValue);
        /// <summary>
        /// 获取管控措施
        /// </summary>
        /// <param name="riskId">风险记录Id</param>
        /// <param name="typeName">管控措施类别</param>
        /// <returns></returns>
        DataTable GetDTList(string riskId,string typeName);
        string GetMeasures(string riskId, string typeName = "");

        DataTable GetMeasuresDetail(string worktask, string areaid);

        DataTable GetLinkAreaById(string Areaid);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="riskId">风险库记录Id</param>
        int Remove(string riskId);
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveForm(string keyValue, List<MeasuresEntity> entity);
        void Save(string keyValue, MeasuresEntity entity);
        #endregion
    }
}