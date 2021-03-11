using BSFramework.Util.WebControl;
using ERCHTMS.Entity.TrainPlan;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.IService.TrainPlan
{
   public interface ISafeSummaryService
    {

        /// <summary>
        /// 获取安措计划总结报告
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetList(Pagination pagination, string queryJson);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        SafeSummaryEntity GetEntity(string keyValue);

        /// <summary>
        /// 检查是否提交
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool CheckExists(string keyValue, SafeSummaryEntity entity);

        /// <summary>
        /// 新增/修改
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        void SaveForm(string keyValue, SafeSummaryEntity entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        bool DeleteForm(string keyValue);
    }
}
