using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Entity.TwoTickets;
using ERCHTMS.IService.TwoTicketsIService;
using ERCHTMS.Service.TwoTickets;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Busines.TwoTickets
{
    /// <summary>
    /// 描 述：两票信息
    /// </summary>
    public class TwoTicketsBLL
    {
        private TwoTicketsIService service = new TwoTicketsService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TwoTicketsEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TwoTicketsEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// 获取票据处理记录
        /// </summary>
        /// <param name="keyValue">票ID</param>
        /// <returns></returns>
        public DataTable GetAuditRecord(string keyValue)
        {
            return service.GetAuditRecord(keyValue);
        }

        /// <summary>
        /// 生成工作票编号
        /// </summary>
        /// <param name="dataType">票分类</param>
        /// <param name="ticketType">票类型</param>
        /// <returns></returns>
        public string CreateTicketCode(string keyValue, string dataType, string ticketType)
        {
            return service.CreateTicketCode(keyValue,dataType, ticketType);
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
        public void SaveForm(string keyValue, TwoTicketsEntity entity)
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
        /// 插入票据处理记录
        /// </summary>
        /// <param name="record">记录对象</param>
        /// <param name="user">当前用户</param>
        /// <returns></returns>
        public int InsertRecord(TwoTicketRecordEntity record)
        {
            return service.InsertRecord(record);
        }
        #endregion
    }
}
