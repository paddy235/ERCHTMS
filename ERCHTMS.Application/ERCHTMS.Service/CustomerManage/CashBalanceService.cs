using ERCHTMS.Entity.CustomerManage;
using ERCHTMS.IService.CustomerManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using BSFramework.Util.Extension;
using BSFramework.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Service.CustomerManage
{
    /// <summary>
    /// 描 述：现金余额
    /// </summary>
    public class CashBalanceService : RepositoryFactory<CashBalanceEntity>, ICashBalanceService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<CashBalanceEntity> GetList(string queryJson)
        {
            var expression = LinqExtensions.True<CashBalanceEntity>();
            var queryParam = queryJson.ToJObject();
            //单据日期
            if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
            {
                DateTime startTime = queryParam["StartTime"].ToDate();
                DateTime endTime = queryParam["EndTime"].ToDate().AddDays(1);
                expression = expression.And(t => t.ExecutionDate >= startTime && t.ExecutionDate <= endTime);
            }
            return this.BaseRepository().IQueryable(expression).OrderBy(t => t.CreateDate).ToList();
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 添加收支余额
        /// </summary>
        /// <param name="db"></param>
        /// <param name="cashBalanceEntity"></param>
        public void AddBalance(IRepository db, CashBalanceEntity cashBalanceEntity)
        {
            decimal balance = 0;
            var data = db.IQueryable<CashBalanceEntity>(t => t.CashAccount == cashBalanceEntity.CashAccount).OrderByDescending(t => t.CreateDate);
            if (data.Count() > 0)
            {
                balance = data.First().Balance.ToDecimal();
            }
            if (cashBalanceEntity.Receivable != null)
            {
                cashBalanceEntity.Balance = cashBalanceEntity.Receivable + balance;
            }
            if (cashBalanceEntity.Expenses != null)
            {
                cashBalanceEntity.Balance = balance - cashBalanceEntity.Expenses;
            }
            cashBalanceEntity.Create();
            db.Insert(cashBalanceEntity);
        }
        #endregion
    }
}
