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
    public interface ISafeTrainPlanService
    {
        #region [获取数据]
        /// <summary>
        /// 安措培训计划列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetList(Pagination pagination, string queryJson);
        /// <summary>
        /// 数据查重 根据培训项目、培训内容、培训对象、培训时间以及责任部门
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool CheckDataExists(SafeTrainPlanEntity entity);

        /// <summary>
        /// 判断是否有待下发数据
        /// </summary>
        /// <returns></returns>
        bool CheckUnpublishPlan(string userId);

        /// <summary>
        /// 下发安全培训计划
        /// </summary>
        void IssueData(string userId);
        #endregion


        #region [提交数据]
        /// <summary>
        /// 批量保存
        /// </summary>
        /// <param name="list"></param>
        void InsertSafeTrainPlan(List<SafeTrainPlanEntity> list);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        SafeTrainPlanEntity GetEntity(string keyValue);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue"></param>
        void Remove(string keyValue);

        /// <summary>
        /// 提交数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="entity"></param>
        void SaveForm(string keyValue, SafeTrainPlanEntity entity);
        #endregion
    }
}
