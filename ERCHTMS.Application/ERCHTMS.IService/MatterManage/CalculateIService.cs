using ERCHTMS.Entity.MatterManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.MatterManage
{
    /// <summary>
    /// 描 述：计量管理
    /// </summary>
    public interface CalculateIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<CalculateEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        CalculateEntity GetEntity(string keyValue);

        /// <summary>
        /// 获取用户授权记录
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        UserEmpowerRecordEntity GetUserRecord(string keyValue);

        /// <summary>
        /// 获取最新称重计量信息
        /// </summary>
        /// <param name="keyValue">称重单号</param>
        /// <returns></returns>
        CalculateEntity GetNewEntity(string keyValue);



        /// <summary>
        ///返回未出场订单
        /// </summary>
        /// <returns>返回列表</returns>
        CalculateEntity GetEntranceTicket(string carNo);



        
        /// <summary>
        /// 获取记录管理详情记录实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        CalculateDetailedEntity GetAppDetailedEntity(string keyValue);

        /// <summary>
        /// 列表分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// 列表分页称重
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        DataTable GetNewPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// 获取计量统计列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        DataTable GetCountPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// 获取地磅员列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        DataTable GetPageUserList(Pagination pagination, string queryJson, string res);

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
        void SaveForm(string keyValue, CalculateEntity entity);

        void SaveAppForm(string keyValue, CalculateEntity entity);
        void SaveWeightBridgeDetail(string keyValue, CalculateDetailedEntity entity);

        /// <summary>
        /// 保存用户授权信息
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        void SaveUserForm(string keyValue, UserEmpowerRecordEntity entity);

      

        #endregion
    }
}
