using ERCHTMS.Entity.PowerPlantInside;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.HighRiskWork.ViewModel;

namespace ERCHTMS.IService.PowerPlantInside
{
    /// <summary>
    /// 描 述：事故事件处理
    /// </summary>
    public interface PowerplanthandleIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<PowerplanthandleEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        PowerplanthandleEntity GetEntity(string keyValue);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// 当前登录人是否有权限审核并获取下一次审核权限实体
        /// </summary>
        /// <param name="currUser">当前登录人</param>
        /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
        /// <param name="moduleName">模块名称</param>
        /// <param name="createdeptid">创建人部门ID</param>
        /// <returns>null-当前为最后一次审核,ManyPowerCheckEntity：下一次审核权限实体</returns>
        ManyPowerCheckEntity CheckAuditPower(Operator currUser, out string state, string moduleName, string createdeptid);

        /// <summary>
        /// 事故事件处理待办
        /// </summary>
        /// <returns></returns>
        List<string> ToAuditPowerHandle();

        DataTable GetAuditInfo(string keyValue, string modulename);


        DataTable GetTableBySql(string sql);
        /// <summary>
        /// 获取流程图整改信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="modulename"></param>
        /// <returns></returns>
        DataTable GetReformInfo(string keyValue);

        /// <summary>
        /// 获取流程图验收信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="modulename"></param>
        /// <returns></returns>
        DataTable GetCheckInfo(string keyValue, string modulename);

        string GetUserName(string flowdeptid, string flowrolename, string type = "", string specialtytype = "");

        List<CheckFlowData> GetAppFlowList(string keyValue);

        List<CheckFlowData> GetAppFullFlowList(string keyValue);
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
        void SaveForm(string keyValue, PowerplanthandleEntity entity);

        /// <summary>
        /// 更新事故事件记录状态
        /// </summary>
        /// <param name="keyValue"></param>
        void UpdateApplyStatus(string keyValue);
        #endregion
    }
}
