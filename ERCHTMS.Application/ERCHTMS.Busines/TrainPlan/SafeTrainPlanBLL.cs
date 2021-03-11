using BSFramework.Util.WebControl;
using ERCHTMS.Entity.TrainPlan;
using ERCHTMS.IService.TrainPlan;
using ERCHTMS.Service.TrainPlan;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Busines.TrainPlan
{
    /// <summary>
    /// 安全培训计划业务逻辑层
    /// </summary>
    public class SafeTrainPlanBLL
    {
        private ISafeTrainPlanService service = new SafeTrainPlanService();

        #region [获取数据]
        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetList(Pagination pagination, string queryJson)
        {
            return service.GetList(pagination, queryJson);
        }

        /// <summary>
        /// 数据查重 根据培训项目、培训内容、培训对象、培训时间以及责任部门
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool CheckDataExists(SafeTrainPlanEntity entity)
        {
            return service.CheckDataExists(entity);
        }

        /// <summary>
        /// 判断是否有待下发数据
        /// </summary>
        /// <returns></returns>
        public bool CheckUnpublishPlan(string userId)
        {
            return service.CheckUnpublishPlan(userId);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public SafeTrainPlanEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        #endregion

        #region [提交数据]
        /// <summary>
        /// 提交数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="entity"></param>
        public void SaveForm(string keyValue, SafeTrainPlanEntity entity)
        {
            service.SaveForm(keyValue, entity);
        }

        /// <summary>
        /// 批量保存
        /// </summary>
        /// <param name="list"></param>
        public void InsertSafeTrainPlan(List<SafeTrainPlanEntity> list)
        {
            service.InsertSafeTrainPlan(list);
        }

        /// <summary>
        /// 下发安全培训计划
        /// </summary>
        public void IssueData(string userId)
        {
            service.IssueData(userId);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue"></param>
        public void Remove(string keyValue)
        {
            service.Remove(keyValue);
        }
        #endregion
    }
}
