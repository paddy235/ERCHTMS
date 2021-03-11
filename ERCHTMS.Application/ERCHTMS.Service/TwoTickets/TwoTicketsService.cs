using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using ERCHTMS.Entity.TwoTickets;
using ERCHTMS.IService.TwoTicketsIService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Code;
namespace ERCHTMS.Service.TwoTickets
{
    /// <summary>
    /// 描 述：两票信息
    /// </summary>
    public class TwoTicketsService : RepositoryFactory<TwoTicketsEntity>, TwoTicketsIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TwoTicketsEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {

            DatabaseType dataType = DbHelper.DbType;
            var queryParam = queryJson.ToJObject();
            if (!queryParam["status"].IsEmpty() )
            {
                string status = queryParam["status"].ToString().Trim();
                pagination.conditionJson += string.Format(" and status='{0}'", status);
            }
            if (!queryParam["startDate"].IsEmpty())
            {
                string startDate = queryParam["startDate"].ToString().Trim();
                pagination.conditionJson += string.Format(" and WorkTime>=to_date('{0}','yyyy-mm-dd')", startDate);
            }
            if (!queryParam["endDate"].IsEmpty())
            {
                string endDate = queryParam["endDate"].ToString().Trim();
                pagination.conditionJson += string.Format(" and WorkTime<=to_date('{0}','yyyy-mm-dd')", endDate);
            }
            if (!queryParam["type"].IsEmpty())
            {
                string type = queryParam["type"].ToString().Trim();
                pagination.conditionJson += string.Format(" and tickettype='{0}'", type);
            }
            if (!queryParam["deptCode"].IsEmpty())
            {
                string deptCode = queryParam["deptCode"].ToString().Trim();
                pagination.conditionJson += string.Format(" and deptCode like '{0}%'", deptCode);
            }
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyword = queryParam["keyword"].ToString().Trim();
                pagination.conditionJson += string.Format(" and content like '%{0}%'",keyword);
            }
            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, dataType);
            return dt;

        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TwoTicketsEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }
        /// <summary>
        /// 获取票据处理记录
        /// </summary>
        /// <param name="keyValue">票ID</param>
        /// <returns></returns>
        public DataTable GetAuditRecord(string keyValue)
        {
            return BaseRepository().FindTable(string.Format("select id,hearuser,hearuserid,time,remark,status,iscommit,createdate,createuserid,createuser from XLD_TWOTICKETRECORD where ticketid='{0}'", keyValue));
        }

        /// <summary>
        /// 生成工作票编号
        /// </summary>
        /// <param name="dataType">票分类</param>
        /// <param name="ticketType">票类型</param>
        /// <returns></returns>
        public string CreateTicketCode(string keyValue, string dataType,string ticketType)
        {
            //生成工作票编号 字母（J-热机工作票/D-电气工作票）+年份+月份（如01）+流水号（0001，依次叠加，按月计数）+（阿拉伯数字I/II）（统一默认为I））
            string sno = string.Empty, str = string.Empty ;
            if (dataType.Equals("1") || dataType.Equals("2"))
            {
                //工作票、操作票
                if (ticketType.StartsWith("热机"))
                {
                    sno += "J";
                    str = "热机";
                }
                else if (ticketType.StartsWith("电气"))
                {
                    sno += "D";
                    str = "电气";
                }
                else if (ticketType == "低压开关停/送电指令卡")
                {
                    sno += "DZL";
                    str = "低压开关停/送电指令卡";
                }
                else
                {
                    return "";
                }
            }
            sno += DateTime.Now.ToString("yyyyMM");

            //按照年月查询的票数+1，手动修改允许重复
            string sql = string.Empty;
            if (dataType.Equals("1") || dataType.Equals("2"))
            {
                sql = string.Format(@"select count(1) from XSS_TWOTICKETS where instr(sno,'{0}')>0 and DataType='{1}' and TicketType like '{2}%' ", DateTime.Now.ToString("yyyyMM"), dataType, str);
            }
            else
            {
                //联系票、动火票不区分票类别
                sql = string.Format(@"select count(1) from XSS_TWOTICKETS where instr(sno,'{0}')>0  and DataType='{1}'", DateTime.Now.ToString("yyyyMM"), dataType);
            }
            if (!string.IsNullOrEmpty(keyValue))
            {
                sql += string.Format(" and ID<>'{0}'", keyValue);
            }

            int serialCode = Convert.ToInt32(BaseRepository().FindObject(sql));
            //流水号
            sno += (serialCode + 1).ToString().PadLeft(4, '0');
            sno += "(I)";
            return sno;
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            var tran = DbFactory.Base().BeginTrans();
            try
            {
                tran.ExecuteBySql(string.Format("delete from XSS_TWOTICKETS where id='{0}'", keyValue));
                tran.ExecuteBySql(string.Format("delete from XLD_TWOTICKETRECORD where ticketId='{0}'",keyValue));
                tran.Commit();
            }
            catch
            {
                tran.Rollback();

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
            int count = 0;
            var res = DbFactory.Base().BeginTrans();
            try
            {
                res.ExecuteBySql(string.Format("delete from XLD_TWOTICKETRECORD where ticketId='{0}' and status='{1}'", record.TicketId, record.Status));
                count = res.ExecuteBySql(string.Format(@"insert into XLD_TWOTICKETRECORD(id,hearuser,hearuserid,time,remark,createdate,createuser,createuserid,ticketid,status,IsCommit)
            values('{0}','{1}','{2}',to_date('{3}','yyyy-mm-dd hh24:mi:ss'),'{4}',to_date('{5}','yyyy-mm-dd hh24:mi:ss'),'{6}','{7}','{8}','{9}','{10}')", Guid.NewGuid().ToString(), record.HearUser, record.HearUserId, record.Time, record.Remark, record.CreateDate, record.CreateUser, record.CreateUserId, record.TicketId, record.Status, record.IsCommit));
                res.ExecuteBySql(string.Format("update XSS_TWOTICKETS set status='{0}' where id='{1}'", record.Status, record.TicketId));
                res.Commit();
                count = 1;
            }
            catch(Exception ex)
            {
                res.Rollback();
                count = 0;
            }
            return count;
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, TwoTicketsEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Id = keyValue;
                TwoTicketsEntity ticket = GetEntity(keyValue);
                if (ticket==null)
                {
                    entity.Create();
                    this.BaseRepository().Insert(entity);
                }
                else
                {
                    entity.Modify(keyValue);
                    this.BaseRepository().Update(entity);
                }
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
