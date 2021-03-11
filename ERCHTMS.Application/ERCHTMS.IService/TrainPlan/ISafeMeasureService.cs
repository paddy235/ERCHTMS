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
    /// <summary>
    /// 安措计划
    /// </summary>
    public interface ISafeMeasureService
    {
        #region 获取数据
        /// <summary>
        /// 应用列表
        /// </summary>
        /// <param name="deptId">部门id</param>
        /// <returns></returns>
        DataTable GetList(Pagination pagination, string queryJson);

        /// <summary>
        /// 数据查重
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool ExistSafeMeasure(SafeMeasureEntity entity);

        /// <summary>
        /// 应用实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        SafeMeasureEntity GetEntity(string keyValue);

        /// <summary>
        /// 获取流程节点
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="measureId"></param>
        /// <returns></returns>
        DataTable GetCheckInfo(string moduleName, string measureId,string adjustId);

        /// <summary>
        /// 获取执行人所在部门
        /// </summary>
        /// <param name="measureId"></param>
        /// <returns></returns>
        string GetExecuteDept(string measureId);

        /// <summary>
        /// 获取安措计划季度数据
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetSafeMeasureData(string queryJson);
        /// <summary>
        /// 安措计划台账提交后回写数据
        /// </summary>
        /// <param name="postState"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool ChangeFinishData(string postState, SafeMeasureEntity entity);

        /// <summary>
        /// 获取反馈信息
        /// </summary>
        /// <param name="safeMeasureId"></param>
        /// <returns></returns>
        DataTable GetFeedbackInfo(string safeMeasureId);

        /// <summary>
        /// 获取审核人账号
        /// </summary>
        /// <param name="flowRoleName"></param>
        /// <param name="deptId"></param>
        /// <returns></returns>
        string GetNextStepUser(string flowRoleName, string deptId);
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除应用
        /// </summary>
        /// <param name="keyValue">主键</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// 应用（批量新增）
        /// </summary>
        /// <param name="list"></param>
        void SaveForm(List<SafeMeasureEntity> list);

        void UpdateForm(string keyValue, SafeMeasureEntity entity);

        /// <summary>
        /// 修改安措计划
        /// </summary>
        /// <param name="entity"></param>
        void UpdateSafeMeasure(SafeMeasureEntity entity);

        /// <summary>
        /// 判断是否存在未下发数据
        /// </summary>
        /// <returns></returns>
        bool CheckUnPublish(string userId);

        /// <summary>
        /// 下发
        /// </summary>
        /// <param name="userId"></param>
        void IssueData(string userId);
        #endregion
    }
}
