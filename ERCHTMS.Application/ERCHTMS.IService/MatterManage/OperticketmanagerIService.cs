using ERCHTMS.Entity.MatterManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.MatterManage
{
    /// <summary>
    /// 描 述：开票管理入厂开票
    /// </summary>
    public interface OperticketmanagerIService
    {
        #region 获取数据


        /// <summary>
        /// 生成开票单号
        /// </summary>
        /// <param name="product">副产品名称</param>
        /// <param name="takeGoodsName">提货商</param>
        /// <param name="transportType">运输类型(提货，转运)</param>
        /// <returns></returns>
        string GetTicketNumber(string product, string takeGoodsName, string transportType);

        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        DataTable GetDataTable(string sql);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<OperticketmanagerEntity> GetList(string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        OperticketmanagerEntity GetEntity(string keyValue);
        /// <summary>
        /// 获取查看过程管理实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        OperticketmanagerEntity GetProcessEntity(string keyValue);

        /// <summary>
        /// 获取最新入场开票记录信息
        /// </summary>
        /// <param name="keyValue">车牌号</param>
        /// <returns></returns>
        OperticketmanagerEntity GetNewEntity(string keyValue);

        /// <summary>
        /// 列表分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// 列表分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        DataTable BackGetPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// 根据车牌号获取车辆信息
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        OperticketmanagerEntity GetCar(string CarNo);

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
        void SaveForm(string keyValue, OperticketmanagerEntity entity);

        /// <summary>
        /// 工作日志
        /// </summary>
        /// <param name="entity"></param>
        void InsetDailyRecord(DailyrRecordEntity entity);

        #endregion
    }
}
