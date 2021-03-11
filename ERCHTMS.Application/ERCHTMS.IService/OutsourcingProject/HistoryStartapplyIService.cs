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
    public interface HistoryStartapplyIService
    {
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        HistoryStartapplyEntity GetEntity(string keyValue);
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        DataTable GetHisPageList(Pagination pagination, string queryJson);
        DataTable GetApplyList(string keyValue);
        DataTable GetApplyInfo(string keyValue);

        void SaveForm(string keyValue, HistoryStartapplyEntity entity);
    }
}
