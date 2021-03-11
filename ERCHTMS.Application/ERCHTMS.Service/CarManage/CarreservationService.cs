using System;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BSFramework.Data;
using ERCHTMS.Code;
using BSFramework.Util;
using BSFramework.Util.Extension;


namespace ERCHTMS.Service.CarManage
{
    /// <summary>
    /// 描 述：班车预约记录
    /// </summary>
    public class CarreservationService : RepositoryFactory<CarreservationEntity>, CarreservationIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<CarreservationEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public CarreservationEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 获取当前车辆预约记录列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetCarReser(string userid)
        {
            string sql = string.Format(@"select info.ID,info.CarNo,info.Model,info.numberlimit,NVL(c1,0)as c1,NVL(c2,0)as c2,NVL(c3,0)as c3,NVL(c4,0)as c4 from bis_carinfo info
            left join ( select cid,count(cid) as c1 from BIS_CARRESERVATION where RESDate='{0}' and time=0 group by cid ) v0 on info.id=v0.cid
            left join ( select cid,count(cid) as c2 from BIS_CARRESERVATION where RESDate='{0}' and time=1 group by cid ) v1 on info.id=v1.cid
            left join (select cid,count(cid) as c3 from BIS_CARRESERVATION where RESDate='{0}' and time=0 and createuserid='{1}' group by cid) v2 on info.id=v2.cid
            left join (select cid,count(cid) as c4 from BIS_CARRESERVATION where RESDate='{0}' and time=1 and createuserid='{1}' group by cid) v3 on info.id=v3.cid
             where type=0 and IsEnable=1
            ", DateTime.Now.ToString("yyyy-MM-dd"), userid);
            DataTable dt = BaseRepository().FindTable(sql);
            return dt;
        }

        /// <summary>
        /// 预约列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            var curuser = OperatorProvider.Provider.Current();
            DatabaseType dataType = DbHelper.DbType;
            //var queryParam = queryJson.ToJObject();
            //车辆类型
            //if (!queryParam["Type"].IsEmpty())
            //{
            //    string Type = queryParam["Type"].ToString();

            //    pagination.conditionJson += string.Format(" and info.Type = {0}", Type);

            //}

            return this.BaseRepository().FindTableByProcPager(pagination, dataType);
        }


        #endregion

        #region 提交数据

        /// <summary>
        /// 预约/取消预约
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="cid"></param>
        /// <param name="time"></param>
        /// <param name="CarNo"></param>
        /// <param name="IsReser"></param>
        public void AddReser(string userid, string cid, int time, string CarNo, int IsReser, string baseid)
        {
  
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            if (IsReser == 0)
            {
                //如果是取消预约 则删除相关记录
                this.BaseRepository().Delete(it =>
                    it.CID == cid && it.BaseId == baseid && it.CreateUserId == userid && it.DataType == 1);
            }
            else
            {
                CarreservationEntity car = new CarreservationEntity();
                car.Create();
                car.RESDate = DateTime.Now;
                car.BaseId = baseid;
                car.CarNo = CarNo;
                car.Time = time;
                car.DataType = 1;
                car.CID = cid;
                this.BaseRepository().Insert(car);
            }
        }

        /// <summary>
        /// 司机添加电厂班车班次(添加/修改)
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="entity"></param>
        public void AddDriverCarInfo(string KeyValue, CarreservationEntity entity)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            if (!string.IsNullOrEmpty(KeyValue))
            {
                entity.Modify(KeyValue);
                this.BaseRepository().Update(entity);

            }
            else
            {
                entity.Create();
                this.BaseRepository().Insert(entity);
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            //开始事物
            var res = DbFactory.Base().BeginTrans();
            try
            {
                List<CarreservationEntity> List = this.BaseRepository().IQueryable().Where(a => a.BaseId == keyValue && a.DataType == 1).ToList();
                if (List != null) { this.BaseRepository().Delete(List); }//普通人员预约记录
                this.BaseRepository().Delete(keyValue);
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
        public void SaveForm(string keyValue, CarreservationEntity entity)
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
        #endregion
    }
}
