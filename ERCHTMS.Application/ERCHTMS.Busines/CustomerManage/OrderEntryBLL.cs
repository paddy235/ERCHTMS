﻿using ERCHTMS.Entity.CustomerManage;
using ERCHTMS.IService.CustomerManage;
using ERCHTMS.Service.CustomerManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;

namespace ERCHTMS.Busines.CustomerManage
{
    /// <summary>
    /// 描 述：订单明细
    /// </summary>
    public class OrderEntryBLL
    {
        private IOrderEntryService service = new OrderEntryService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="orderId">订单主键</param>
        /// <returns>返回列表</returns>
        public IEnumerable<OrderEntryEntity> GetList(string orderId)
        {
            return service.GetList(orderId);
        }
        #endregion
    }
}