using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Util.WebControl;
using ERCHTMS.Entity.OutsourcingProject;

namespace ERCHTMS.IService.OutsourcingProject
{
    public interface HistoryMoneyIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表--历史记录
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        DataTable GetHisPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        HistoryMoneyEntity GetEntity(string keyValue);
        #endregion
    }
}
