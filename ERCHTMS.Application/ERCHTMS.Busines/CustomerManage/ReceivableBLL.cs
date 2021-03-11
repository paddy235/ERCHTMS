﻿using ERCHTMS.Entity.CustomerManage;
using ERCHTMS.IService.CustomerManage;
using ERCHTMS.Service.CustomerManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.CustomerManage
{
    /// <summary>
    /// 描 述：应收账款
    /// </summary>
    public class ReceivableBLL
    {
        private IReceivableService service = new ReceivableService();

        #region 获取数据
        /// <summary>
        /// 获取收款单列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表</returns>
        public IEnumerable<OrderEntity> GetPaymentPageList(Pagination pagination, string queryJson)
        {
            return service.GetPaymentPageList(pagination, queryJson);
        }
        /// <summary>
        /// 获取收款记录列表
        /// </summary>
        /// <param name="orderId">订单主键</param>
        /// <returns></returns>
        public IEnumerable<ReceivableEntity> GetPaymentRecord(string orderId)
        {
            return service.GetPaymentRecord(orderId);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 保存表单（新增）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(ReceivableEntity entity)
        {
            try
            {
                service.SaveForm(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}