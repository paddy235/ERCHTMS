using BSFramework.Util.WebControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.IService.NosaManage
{
    /// <summary>
    /// 区域代表工作总结
    /// </summary>
    public interface NosaAreaWorkSummaryIService
    {
        /// <summary>
        /// 执行同步Sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>true 执行成功 false 执行失败</returns>
        bool SyncAreaWorkSummary(string sql);
        /// <summary>
        /// 获取区域代表工作总结分页列表数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetAreaWorkPageJson(Pagination pagination, string queryJson);
        /// <summary>
        /// 工作总结提交
        /// </summary>
        /// <param name="keyValue"></param>
        void CommitAreaSummary(string keyValue);
    }
}
