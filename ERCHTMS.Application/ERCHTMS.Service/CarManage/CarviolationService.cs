using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Code;
using ERCHTMS.Entity.MatterManage;
using System;

namespace ERCHTMS.Service.CarManage
{
    /// <summary>
    /// 描 述：违章信息类
    /// </summary>
    public class CarviolationService : RepositoryFactory<CarviolationEntity>, CarviolationIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<CarviolationEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            var curuser = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;

            var queryParam = queryJson.ToJObject();

            //车辆表关联ID
            if (!queryParam["cid"].IsEmpty())
            {
                string cid = queryParam["cid"].ToString();

                pagination.conditionJson += string.Format(" and cid = '{0}'", cid);

            }
            //驾驶人
            if (!queryParam["Dirver"].IsEmpty())
            {
                string Dirver = queryParam["Dirver"].ToString();
                pagination.conditionJson += string.Format(" and Dirver like '%{0}%'", Dirver);
            }
            //电话号码
            if (!queryParam["Phone"].IsEmpty())
            {
                string Phone = queryParam["Phone"].ToString();
                pagination.conditionJson += string.Format(" and Phone  like '{0}%'", Phone);
            }

            //报警类型
            if (!queryParam["CarType"].IsEmpty())
            {
                string CarType = queryParam["CarType"].ToString();
                pagination.conditionJson += string.Format(" and violationtype = {0}", CarType);
            }

            if (!queryParam["condition"].IsEmpty())
            {
                //车牌号
                if (queryParam["condition"].ToString() == "CarNo")
                {
                    string CarNo = queryParam["keyword"].ToString();
                    pagination.conditionJson += string.Format(" and cardno like '%{0}%'", CarNo);
                }
            }

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public CarviolationEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 预警中心数据
        /// </summary>
        /// <returns></returns>
        public List<CarviolationEntity> GetIndexWaring()
        {
            var wcl = BaseRepository().IQueryable(x => x.IsProcess == 0).OrderByDescending(x => x.CreateDate).ToList();
            //已处理只取当天的
            var startDate = DateTime.Now.Date;
            var endData = startDate.AddDays(1);
            var ycl = BaseRepository().IQueryable(x => x.IsProcess == 1 && x.CreateDate >= startDate && x.CreateDate < endData).ToList();
            wcl.AddRange(ycl);
            return wcl;
        }

        /// <summary>
        /// 预警中心数据统计
        /// </summary>
        /// <returns></returns>
        public object GetIndexWaringCount()
        {
            var startDate = DateTime.Now.Date;
            var endData = startDate.AddDays(1);
            //已处理取当天
            int count1 = BaseRepository().IQueryable(x => x.CreateDate >= startDate && x.CreateDate < endData && x.IsProcess == 1).Count();
            int count2 = BaseRepository().IQueryable(x => x.IsProcess == 0).Count();
            return new { YCL = count1, WCL = count2 };
        }

        /// <summary>
        /// 获取所有的未处理的订单
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public List<CarviolationEntity> GetUntreatedWaringList(Pagination pagination, string queryJson)
        {
            var query = BaseRepository().IQueryable(x => x.IsProcess == 0);

            var data = query.OrderByDescending(x => x.CreateDate).Skip((pagination.page - 1) * pagination.rows).Take(pagination.rows).ToList();
            return data;
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
        public void SaveForm(string keyValue, CarviolationEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                this.BaseRepository().Update(entity);
            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }

        /// <summary>
        /// 增加一个违章接口
        /// </summary>
        public void AddViolation(string id, int type, int ViolationType,string ViolationMsg)
        {
            CarviolationEntity entity=new CarviolationEntity();

            //0为电厂班车 1为私家车 2为商务公车 3为拜访车辆 4为物料车辆 5为危化品车辆
            switch (type)
            {
                case 0:
                case 1:
                case 2:
                    Repository<CarinfoEntity> inlogdbC = new Repository<CarinfoEntity>(DbFactory.Base());
                    CarinfoEntity car=inlogdbC.FindEntity(id);
                    entity.CardNo = car.CarNo;
                    entity.Dirver = car.Dirver;
                    entity.Phone = car.Phone;
                    break;
                case 3:
                    Repository<VisitcarEntity> inlogdbv = new Repository<VisitcarEntity>(DbFactory.Base());
                    VisitcarEntity oldv = inlogdbv.FindEntity(id);
                    entity.CardNo = oldv.CarNo;
                    entity.Dirver = oldv.Dirver;
                    entity.Phone = oldv.Phone;
                    break;
                case 4:
                    Repository<OperticketmanagerEntity> inlogdbo = new Repository<OperticketmanagerEntity>(DbFactory.Base());
                    OperticketmanagerEntity oldo = inlogdbo.FindEntity(id);
                    entity.CardNo = oldo.Platenumber;
                    entity.Dirver = oldo.DriverName;
                    entity.Phone = oldo.DriverTel;
                    break;
                case 5:
                    Repository<HazardouscarEntity> inlogdb = new Repository<HazardouscarEntity>(DbFactory.Base());
                    HazardouscarEntity old = inlogdb.FindEntity(id);
                   
                    entity.CardNo = old.CarNo;
                    entity.Dirver = old.Dirver;
                    entity.Phone = old.Phone;
                    break;
            }
            entity.CarType = type;
            entity.CID = id;
            entity.ViolationType = ViolationType;
            entity.ViolationMsg = ViolationMsg;
            entity.IsProcess = 0;
            entity.Create();
            this.BaseRepository().Insert(entity);
        }

        /// <summary>
        /// 插入车辆超速信息
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(CarviolationEntity entity)
        {
            BaseRepository().Insert(entity);
        }

        #endregion
    }
}
