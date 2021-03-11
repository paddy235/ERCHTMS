using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using BSFramework.Data;
using BSFramework.Data.Repository;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Entity.MatterManage;
using ERCHTMS.IService.MatterManage;

namespace ERCHTMS.Service.MatterManage
{
    /// <summary>
    /// 描 述：开票管理入厂开票
    /// </summary>
    public class OperticketmanagerService : RepositoryFactory<OperticketmanagerEntity>, OperticketmanagerIService
    {
        private object NumberLock = new object();
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
            string sql = string.Format("SELECT COUNT(NUMBERS) FROM WL_OPERTICKETMANAGER WHERE  TO_CHAR(CREATEDATE,'yyyy-MM-dd')='{0}'", DateTime.Now.ToString("yyyy-MM-dd"));
            StringBuilder sbFilter = new StringBuilder();
            string preFix = "";
            string middleFix = DateTime.Now.ToString("yyMMdd");
            switch (product)
            {
                case "粉煤灰":
                    sbFilter.Append(" AND PRODUCTTYPE='粉煤灰'");
                    if (takeGoodsName == "浩翔宇")
                    {
                        preFix = "H";
                        sbFilter.Append(" AND TAKEGOODSNAME='浩翔宇'");
                    }
                    else if (takeGoodsName == "益恒鑫")
                    {
                        preFix = "HE";
                        sbFilter.Append(" AND TAKEGOODSNAME='益恒鑫'");
                    }
                    break;
                case "石膏":
                    sbFilter.Append(" AND PRODUCTTYPE='石膏'");
                    if (transportType == "提货")
                    {
                        preFix = "S";
                        sbFilter.Append(" AND TRANSPORTTYPE='提货'");
                    }
                    break;
                case "石膏转运":
                    preFix = "SG";
                    sbFilter.Append(" AND TRANSPORTTYPE='转运'");
                    break;
                case "炉底渣":
                    preFix = "Z";
                    sbFilter.Append(" AND PRODUCTTYPE='炉底渣'");
                    break;
            }
            Repository<object> repository = new Repository<object>(DbFactory.Base());
           
            string numbers = string.Format("{0}{1}", preFix, middleFix);
            lock (NumberLock)
            {
                sql = sql + sbFilter.ToString();
                DataTable count = repository.FindTable(sql);
                numbers = numbers + (Convert.ToInt32(count.Rows[0][0]) + 1).ToString().PadLeft(3, '0');
            }
            return numbers;
        }

        /// <summary>
        /// 获取DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql)
        {
            return this.BaseRepository().FindTable(sql);
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<OperticketmanagerEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public OperticketmanagerEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 获取查看过程管理实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public OperticketmanagerEntity GetProcessEntity(string keyValue)
        {
            OperticketmanagerEntity entity = this.BaseRepository().FindEntity(keyValue);
            try
            {
                if (entity != null)
                {
                    entity.RCdbTime = this.GetTimeDouble(entity.Getdata, entity.BalanceTime).ToString();
                    entity.DbOutTime = this.GetTimeDouble(entity.BalanceTime, entity.OutDate).ToString();
                    if (entity.StayTime != null && entity.StayTime > 59) { entity.StayTimeStatus = "异常"; }
                    else entity.StayTimeStatus = "正常";
                    entity.NetweightStatus = "正常";
                }
            }
            catch (Exception)
            {
                entity = this.BaseRepository().FindEntity(keyValue);
            }
            return entity;
        }

        /// <summary>
        /// 两个时间之间差（分钟）
        /// </summary>
        /// <param name="stime"></param>
        /// <param name="etime"></param>
        /// <returns></returns>
        public double GetTimeDouble(DateTime? stime, DateTime? etime)
        {
            double tnumber = 0;
            try
            {
                if (stime != null && etime != null)
                {
                    System.TimeSpan t1 = DateTime.Parse(etime.ToString()) - DateTime.Parse(stime.ToString());
                    tnumber = Math.Truncate(t1.TotalMinutes);
                }
            }
            catch (Exception)
            {
                tnumber = 0;
            }
            return tnumber;
        }

        /// <summary>
        /// 列表分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();

            if (!queryParam["keyword"].IsEmpty())
            {//车牌号
                string PlateNumber = queryParam["keyword"].ToString().Trim();
                pagination.conditionJson += string.Format(" and PlateNumber like '%{0}%'", PlateNumber);
            }

            if (!queryParam["Takegoodsname"].IsEmpty() && queryParam["Takegoodsname"].ToString().Trim() != "全部")
            {//提货方
                string Transporttype = queryParam["Takegoodsname"].ToString().Trim();
                pagination.conditionJson += string.Format(" and Takegoodsname='{0}'", Transporttype);
            }
            if (!queryParam["Transporttype"].IsEmpty() && queryParam["Transporttype"].ToString().Trim() != "全部")
            {//运输类型
                string Transporttype = queryParam["Transporttype"].ToString().Trim();
                pagination.conditionJson += string.Format(" and Transporttype like '%{0}%'", Transporttype);
            }
            if (!queryParam["Producttype"].IsEmpty() && queryParam["Producttype"].ToString().Trim() != "全部")
            {//产品类型
                string Producttype = queryParam["Producttype"].ToString().Trim();
                pagination.conditionJson += string.Format(" and Producttype like '%{0}%'", Producttype);
            }
            if (!queryParam["Stime"].IsEmpty())
            {//进厂时间起
                string stime = queryParam["Stime"].ToString().Trim();
                pagination.conditionJson += string.Format(" and GetData >  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') ", Convert.ToDateTime(stime));
            }
            if (!queryParam["Etime"].IsEmpty())
            {//进厂时间至
                string etime = queryParam["Etime"].ToString().Trim();
                DateTime dst = Convert.ToDateTime(etime).AddMinutes(1);
                pagination.conditionJson += string.Format(" and GetData < to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') ", dst);
            }
            //出厂时间起
            if (!queryParam["OutStartTime"].IsEmpty())
            {
                string outStartTime = queryParam["OutStartTime"].ToString().Trim();
                pagination.conditionJson += string.Format(" and OutDate >=  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') ", outStartTime);
            }
            //出厂时间至
            if (!queryParam["OutEndtime"].IsEmpty())
            {
                string outEndTime = queryParam["OutEndtime"].ToString().Trim();
                pagination.conditionJson += string.Format(" and OutDate <= to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') ", outEndTime);
            }

            if (!queryParam["QueryDress"].IsEmpty() && queryParam["QueryDress"].ToString().Trim() != "全部")
            {//装灰点
                string QueryDress = queryParam["QueryDress"].ToString().Trim();
                pagination.conditionJson += string.Format(" and Dress like '%{0}%'", QueryDress);
            }

            if (queryParam["QueryDress"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and Dress  is null");
            }

            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, DbHelper.DbType);



            return dt;
        }


        /// <summary>
        /// 列表分页
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable BackGetPageList(Pagination pagination, string queryJson)
        {
            var queryParam = queryJson.ToJObject();


            pagination.conditionJson += " and ExamineStatus > 0";
            if (!queryParam["keyword"].IsEmpty())
            {
                //车牌号
                if (queryParam["condition"].ToString() == "CarNo")
                {
                    string PlateNumber = queryParam["keyword"].ToString().Trim();
                    pagination.conditionJson += string.Format(" and PlateNumber like '%{0}%'", PlateNumber);

                }
                //驾驶人
                if (queryParam["condition"].ToString() == "Dirver")
                {
                    string Dirver = queryParam["keyword"].ToString();
                    pagination.conditionJson += string.Format(" and drivername like '%{0}%'", Dirver);
                }
                //电话号码
                if (queryParam["condition"].ToString() == "Phone")
                {

                    string Phone = queryParam["keyword"].ToString();
                    pagination.conditionJson += string.Format(" and drivertel  like '{0}%'", Phone);
                }
            }
            //运输类型
            if (!queryParam["Transporttype"].IsEmpty() && queryParam["Transporttype"].ToString().Trim() != "全部")
            {
                string Transporttype = queryParam["Transporttype"].ToString().Trim();
                pagination.conditionJson += string.Format(" and Transporttype like '%{0}%'", Transporttype);
            }
            //状态
            if (!queryParam["Status"].IsEmpty())
            {
                string Status = queryParam["Status"].ToString().Trim();
                if (Status == "1")
                {
                    pagination.conditionJson += string.Format(" and (ExamineStatus = 1 or ExamineStatus=2)");
                }
                else
                {

                    pagination.conditionJson += string.Format(" and ExamineStatus = {0}", Status);
                }
            }

            //状态
            if (!queryParam["Vnum"].IsEmpty())
            {
                string Vnum = queryParam["Vnum"].ToString();
                if (Vnum == "0")
                {
                    pagination.conditionJson += string.Format(" and (vnum = 0 or vnum is null)");
                }
                else
                {
                    pagination.conditionJson += string.Format(" and vnum > 0");
                }
            }


            DataTable dt = this.BaseRepository().FindTableByProcPager(pagination, DbHelper.DbType);
            return dt;
        }

        /// <summary>
        /// 获取最新入场开票记录信息
        /// </summary>
        /// <param name="VehicleNumber">车牌号</param>
        /// <returns></returns>
        public OperticketmanagerEntity GetNewEntity(string VehicleNumber)
        {
            OperticketmanagerEntity entity = new OperticketmanagerEntity();
            if (!string.IsNullOrEmpty(VehicleNumber))
            {
                entity = this.BaseRepository().IQueryable().ToList().Where(a => a.Platenumber == VehicleNumber && a.ExamineStatus != 4 && a.Isdelete == 1).OrderByDescending(a => a.CreateDate).FirstOrDefault();
            }
            else
            {
                entity = this.BaseRepository().IQueryable().ToList().Where(a => a.Isdelete == 1).OrderByDescending(a => a.CreateDate).FirstOrDefault();
            }
            return entity;
        }

        /// <summary>
        /// 根据车牌号获取车辆信息
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public OperticketmanagerEntity GetCar(string CarNo)
        {
            return this.BaseRepository().IQueryable(it => it.Platenumber == CarNo).OrderByDescending(it => it.CreateDate).FirstOrDefault();
        }

        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, OperticketmanagerEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                entity.Numbers = GetTicketNumber(entity.Producttype, entity.Takegoodsname, entity.Transporttype);

                if (GetNewEntity(entity.Platenumber) == null)
                    entity.IsFirst = "是";
                else entity.IsFirst = "否";
                var lastTicket = GetCar(entity.Platenumber);
                if (lastTicket != null)
                {
                    entity.DriverName = lastTicket.DriverName;
                    entity.DriverTel = lastTicket.DriverTel;
                }
                this.BaseRepository().Insert(entity);
            }
        }

        /// <summary>
        /// 写日志功能
        /// </summary>
        public void InsetDailyRecord(DailyrRecordEntity entity)
        {
            try
            {
                if (entity != null)
                {
                    Operator user = OperatorProvider.Provider.Current();
                    if (user != null)
                        entity.Create();
                    Repository<DailyrRecordEntity> equ = new Repository<DailyrRecordEntity>(DbFactory.Base());
                    equ.Insert(entity);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
