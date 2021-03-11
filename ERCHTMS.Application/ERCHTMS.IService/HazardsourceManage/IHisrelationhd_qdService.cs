using ERCHTMS.Entity.HazardsourceManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.HazardsourceManage
{
    /// <summary>
    /// 描 述：危险源清单
    /// </summary>
    public interface IHisrelationhd_qdService
    {
        #region 获取数据

        IEnumerable<Hisrelationhd_qdEntity> GetListForRecord(string queryJson);

        /// <summary>
        /// 根据区域统计
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetReportForDistrictName(string queryJson);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<Hisrelationhd_qdEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        Hisrelationhd_qdEntity GetEntity(string keyValue);


        string StaQueryList(string queryJson);
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
        void SaveForm(string keyValue, Hisrelationhd_qdEntity entity);
        #endregion
    }
}
