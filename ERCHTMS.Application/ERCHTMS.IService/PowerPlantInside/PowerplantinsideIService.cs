using ERCHTMS.Entity.PowerPlantInside;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.IService.PowerPlantInside
{
    /// <summary>
    /// 描 述：单位内部快报
    /// </summary>
    public interface PowerplantinsideIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<PowerplantinsideEntity> GetList(string queryJson);

        System.Data.DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        PowerplantinsideEntity GetEntity(string keyValue);
        
        /// <summary>
        /// 统计列表
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        string GetStatisticsList(int year,string mode);
        
        /// <summary>
        /// 月度变化统计图
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        string GetStatisticsHighchart(string year, string mode);

        #region  当前登录人是否有权限审核并获取下一次审核权限实体
        /// <summary>
        /// 当前登录人是否有权限审核并获取下一次审核权限实体
        /// </summary>
        /// <param name="currUser">当前登录人</param>
        /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
        /// <param name="moduleName">模块名称</param>
        /// <param name="createdeptid">创建人部门ID</param>
        /// <returns>null-当前为最后一次审核,ManyPowerCheckEntity：下一次审核权限实体</returns>
        ManyPowerCheckEntity CheckAuditPower(Operator currUser, out string state, string moduleName, string createdeptid);
        #endregion

        /// <summary>
        /// 待审核事故事件数量
        /// </summary>
        /// <returns></returns>
        string GetAccidentEventNum();
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
        void SaveForm(string keyValue, PowerplantinsideEntity entity);
        #endregion
    }
}
