using ERCHTMS.Entity.AuthorizeManage;
using ERCHTMS.Entity.ReportManage;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.ReportManage
{
    /// <summary>
    /// 描 述：报表模板管理
    /// </summary>
    public interface IRptTempService
    {
        #region 获取数据
        /// <summary>
        /// 报表模板列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        IEnumerable<RptTempEntity> GetList(string queryJson);
        /// <summary>
        /// 报表模板实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        RptTempEntity GetEntity(string keyValue);
        /// <summary>
        /// 获得报表数据
        /// </summary>
        /// <param name="reportId">主键值</param>
        /// <returns></returns>
        string GetReportData(string reportId);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除报表模板
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// 保存报表模板表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="rptTempEntity">报表实体</param>
        /// <param name="moduleEntity">模块实体</param>
        /// <returns></returns>
        void SaveForm(string keyValue, RptTempEntity rptTempEntity, ModuleEntity moduleEntity);
        #endregion
    }
}
