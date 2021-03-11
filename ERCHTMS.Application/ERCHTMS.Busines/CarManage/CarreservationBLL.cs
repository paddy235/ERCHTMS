using ERCHTMS.Entity.CarManage;
using ERCHTMS.IService.CarManage;
using ERCHTMS.Service.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.CarManage
{
    /// <summary>
    /// 描 述：班车预约记录
    /// </summary>
    public class CarreservationBLL
    {
        private CarreservationIService service = new CarreservationService();

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<CarreservationEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public CarreservationEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 获取当前车辆预约记录列表
        /// </summary>
        /// <returns></returns>
        public List<ReserVation> GetCarReser(string userid)
        {
            List<ReserVation> rlist=new List<ReserVation>();
            DataTable dt=service.GetCarReser(userid);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ReserVation rv=new ReserVation();
                    rv.CID = dr["ID"].ToString();
                    rv.CarNo = dr["CarNo"].ToString();
                    rv.Model = dr["Model"].ToString();
                    rv.NumberLimit = Convert.ToInt32(dr["NumberLimit"]);
                    List<ReserList> relist=new List<ReserList>();
                    ReserList rl1=new ReserList();
                    rl1.DateStr = DateTime.Now.ToString("yyyy-MM-dd") + " 12:00";
                    rl1.IsReser = Convert.ToInt32(dr["c3"]);
                    rl1.Num = Convert.ToInt32(dr["c1"]);
                    rl1.Time = 0;

                    ReserList rl2 = new ReserList();
                    rl2.DateStr = DateTime.Now.ToString("yyyy-MM-dd") + " 17:00";
                    rl2.IsReser = Convert.ToInt32(dr["c4"]);
                    rl2.Num = Convert.ToInt32(dr["c2"]);
                    rl2.Time = 1;
                    relist.Add(rl1);
                    relist.Add(rl2);
                    rv.RList = relist;
                    rlist.Add(rv);
                }
            }

            return rlist;

        }


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
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
            service.AddReser(userid, cid, time, CarNo, IsReser, baseid);
        }

        /// <summary>
        /// 司机添加班次
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="entity"></param>
        public void AddDriverCarInfo(string userid, CarreservationEntity entity)
        {
            service.AddDriverCarInfo(userid, entity);
        }


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
        public void SaveForm(string keyValue, CarreservationEntity entity)
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
        #endregion
    }
}
