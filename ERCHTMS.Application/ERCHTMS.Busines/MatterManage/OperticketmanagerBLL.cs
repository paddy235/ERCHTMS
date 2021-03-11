using ERCHTMS.Entity.MatterManage;
using ERCHTMS.IService.MatterManage;
using ERCHTMS.Service.MatterManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.MatterManage
{
    /// <summary>
    /// 描 述：开票管理入厂开票
    /// </summary>
    public class OperticketmanagerBLL
    {
        private OperticketmanagerIService service = new OperticketmanagerService();

        #region 获取数据


        /// <summary>
        /// 生成开票单号
        /// </summary>
        /// <param name="product">副产品名称</param>
        /// <param name="takeGoodsName">提货商</param>
        /// <param name="transportType">运输类型(提货，转运)</param>
        /// <returns></returns>
        public string GetTicketNumber(string product, string takeGoodsName, string transportType)
        {

            return service.GetTicketNumber(product,  takeGoodsName,  transportType);
        }

        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql)
        {
            return service.GetDataTable(sql);
        }
        
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<OperticketmanagerEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }

        /// <summary>
        /// 列表分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }

        /// <summary>
        /// 列表分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable BackGetPageList(Pagination pagination, string queryJson)
        {
            return service.BackGetPageList(pagination, queryJson);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public OperticketmanagerEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 获取查看过程管理实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public OperticketmanagerEntity GetProcessEntity(string keyValue)
        {
            return service.GetProcessEntity(keyValue);
        }

        /// <summary>
        /// 获取最新入场开票记录信息
        /// </summary>
        /// <param name="keyValue">车牌号</param>
        /// <returns></returns>
        public OperticketmanagerEntity GetNewEntity(string keyValue)
        {
            return service.GetNewEntity(keyValue);
        }

        /// <summary>
        /// 根据车牌号获取车辆信息
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public OperticketmanagerEntity GetCar(string CarNo)
        {
            return service.GetCar(CarNo);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, OperticketmanagerEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 添加工作记录日志
        /// </summary>
        /// <param name="entity"></param>
        public void InsetDailyRecord(DailyrRecordEntity entity)
        {
            try
            {
                service.InsetDailyRecord(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }


        
        #endregion
    }
}
