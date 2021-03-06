using ERCHTMS.Entity.CustomerManage;
using ERCHTMS.Entity.CustomerManage.ViewModel;
using ERCHTMS.IService.CustomerManage;
using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace ERCHTMS.Service.CustomerManage
{
    /// <summary>
    /// 描 述：应收账款报表
    /// </summary>
    public class ReceivableReportService : RepositoryFactory, IReceivableReportService
    {
        /// <summary>
        /// 获取收款列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<ReceivableReportModel> GetList(string queryJson)
        {
            var queryParam = queryJson.ToJObject();
            List<DbParameter> parameter = new List<DbParameter>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT  r.ReceivableId ,
                                    o.OrderId ,
                                    o.OrderCode ,
                                    c.CustomerId ,
                                    c.EnCode AS CustomerCode ,
                                    c.FullName AS CustomerName ,
                                    r.PaymentTime ,
                                    r.PaymentPrice ,
                                    r.PaymentMode ,
                                    r.PaymentAccount ,
                                    r.Description ,
                                    r.CreateDate ,
                                    r.CreateUserName
                            FROM    Client_Receivable r
                                    LEFT JOIN Client_Order o ON o.OrderId = r.OrderId
                                    LEFT JOIN Client_Customer c ON c.CustomerId = o.CustomerId
                            WHERE   1 = 1");
            //收款日期
            if (!queryParam["StartTime"].IsEmpty() && !queryParam["EndTime"].IsEmpty())
            {
                strSql.Append(" AND r.PaymentTime Between @StartTime AND @EndTime ");
                parameter.Add(DbParameters.CreateDbParameter("@StartTime", (queryParam["StartTime"].ToString() + " 00:00").ToDate()));
                parameter.Add(DbParameters.CreateDbParameter("@EndTime", (queryParam["EndTime"].ToString() + " 23:59").ToDate()));
            }
            //订单单号
            if (!queryParam["OrderCode"].IsEmpty())
            {
                strSql.Append(" AND o.OrderCode like @OrderCode");
                parameter.Add(DbParameters.CreateDbParameter("@OrderCode", '%' + queryParam["OrderCode"].ToString() + '%'));
            }
            //客户编号
            if (!queryParam["CustomerCode"].IsEmpty())
            {
                strSql.Append(" AND c.CustomerCode like @CustomerCode");
                parameter.Add(DbParameters.CreateDbParameter("@CustomerCode", '%' + queryParam["CustomerCode"].ToString() + '%'));
            }
            //客户名称
            if (!queryParam["CustomerName"].IsEmpty())
            {
                strSql.Append(" AND c.CustomerName like @CustomerName");
                parameter.Add(DbParameters.CreateDbParameter("@CustomerName", '%' + queryParam["CustomerName"].ToString() + '%'));
            }
            strSql.Append(" ORDER BY r.PaymentTime DESC");
            return this.BaseRepository().FindList<ReceivableReportModel>(strSql.ToString(), parameter.ToArray());
        }
    }
}