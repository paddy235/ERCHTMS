using BSFramework.Util.WebControl;
using ERCHTMS.Entity.NosaManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.IService.NosaManage
{
    /// <summary>
    /// 元素负责人工作总结
    /// </summary>
    public interface NosaPersonWorkSummaryIService
    {
        /// <summary>
        /// 执行同步Sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>true 执行成功 false 执行失败</returns>
        bool SyncPersonWorkSummary(string sql);
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        DataTable GetTable(string sql);
        /// <summary>
        /// 获取分页列表数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetElementPageJson(Pagination pagination, string queryJson);
        /// <summary>
        /// 工作总结提交
        /// </summary>
        /// <param name="keyValue"></param>
        void CommitPeopleSummary(string keyValue);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        NosaPersonWorkSummaryEntity GetEntity(string keyValue);

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
        void SaveForm(string keyValue, NosaPersonWorkSummaryEntity entity);
        #endregion
    }
}
