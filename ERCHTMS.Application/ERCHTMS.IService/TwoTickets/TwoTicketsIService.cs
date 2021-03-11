using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Entity.TwoTickets;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.IService.TwoTicketsIService
{
    /// <summary>
    /// 描 述：两票信息
    /// </summary>
    public interface TwoTicketsIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        IEnumerable<TwoTicketsEntity> GetList(string queryJson);
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        TwoTicketsEntity GetEntity(string keyValue);
         /// <summary>
        /// 获取票据处理记录
        /// </summary>
        /// <param name="keyValue">票ID</param>
        /// <returns></returns>
        DataTable GetAuditRecord(string keyValue);

        /// <summary>
        /// 生成工作票编号
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        string CreateTicketCode(string keyValue, string dataType, string ticketType);
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
        void SaveForm(string keyValue, TwoTicketsEntity entity);
          /// <summary>
        /// 插入票据处理记录
        /// </summary>
        /// <param name="record">记录对象</param>
        /// <returns></returns>
        int InsertRecord(TwoTicketRecordEntity record);
        #endregion
    }
}
