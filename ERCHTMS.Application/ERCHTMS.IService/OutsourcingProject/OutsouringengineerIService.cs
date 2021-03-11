using ERCHTMS.Entity.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.HiddenTroubleManage;

namespace ERCHTMS.IService.OutsourcingProject
{
    /// <summary>
    /// 描 述：外包工程信息表
    /// </summary>
    public interface OutsouringengineerIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 首页跳转
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetIndexToList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<OutsouringengineerEntity> GetList();
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<OutsouringengineerEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        OutsouringengineerEntity GetEntity(string keyValue);
        /// <summary>
        /// 根据登录人id 查询已经在建的工程(已经通过开工申请的工程)
        /// </summary>
        /// <returns></returns>
        DataTable GetOnTheStock(Operator currUser);
        /// <summary>
        /// 根据外包单位Id获取外包工程
        /// </summary>
        /// <param name="outProjectId"></param>
        /// <returns></returns>
        DataTable GetEngineerDataByWBId(string deptId, string mode = "");
        /// <summary>
        /// 根据当前登录人Id获取外包工程
        /// </summary>
        /// <param name="outProjectId"></param>
        /// <returns></returns>
        DataTable GetEngineerDataByCurrdeptId(Operator currUser, string mode = "", string orgid = "");

        DataTable GetEngineerDataByCondition(Operator currUser, string mode = "", string orgid = "");

        /// <summary>
        /// 根据当前登录人 获取已经停工的工程信息
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        DataTable GetStopEngineerList();
        /// <summary>
        /// 根据当前登录人 获取工程列表
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        DataTable GetEngineerByCurrDept();
        /// <summary>
        /// 获取工程的流程状态图
        /// </summary>
        /// <param name="keyValue">工程Id</param>
        /// <returns></returns>
        Flow GetProjectFlow(string keyValue);
        string GetTypeCount(string deptid, string year = "", string type = "001,002");
        string GetTypeList(string deptid, string year = "", string type = "001,002");
        string GetStateCount(string deptid, string year = "", string state = "001,002,003");
        string GetStateList(string deptid, string year = "", string state = "001,002,003");
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
        void SaveForm(string keyValue, OutsouringengineerEntity entity);
        bool ProIsOver(string keyValue);
        #endregion
    }
}
