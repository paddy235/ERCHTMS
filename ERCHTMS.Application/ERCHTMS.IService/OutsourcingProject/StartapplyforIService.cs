using ERCHTMS.Entity.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.OutsourcingProject
{
    /// <summary>
    /// 描 述：开工申请表
    /// </summary>
    public interface StartapplyforIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<StartapplyforEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        StartapplyforEntity GetEntity(string keyValue);
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取开工申请对象
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        StartapplyforEntity GetApplyReturnTime(string outProjectId, string outEngId);

         /// <summary>
        /// 获取开工条件各项目完成情况
        /// </summary>
        /// <param name="projectId">工程Id</param>
        /// <returns></returns>
        DataTable GetStartWorkStatus(string projectId);
                /// <summary>
        /// 获取工程施工现场负责人和安全员信息
        /// </summary>
        /// <param name="projectId">工程Id</param>
        /// <returns></returns>
        List<string> GetSafetyUserInfo(string projectId);
        DataTable GetApplyInfo(string keyValue);
           /// <summary>
        /// 获取工程合同编号
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
       object GetContractSno(string projectId);
        /// <summary>
        /// 判断当前用户是否有审核权限
        /// </summary>
        /// <param name="nodeId">节点Id</param>
        /// <param name="user">当前用户</param>
        /// <param name="projectId">工程Id</param>
        /// <returns></returns>
        bool HasCheckPower(string nodeId, ERCHTMS.Code.Operator user, string projectId);

        List<string> GetSafetyUserInfo(string projectId, string roletype, string deptid);
        DataTable GetStartForItem(string keyValue);
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
        bool SaveForm(string keyValue, StartapplyforEntity entity);
        #endregion
    }
}
