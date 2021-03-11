using System;
using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.IService.KbsDeviceManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ERCHTMS.Service.KbsDeviceManage
{
    /// <summary>
    /// 描 述：设备离线记录
    /// </summary>
    public class OfflinedeviceService : RepositoryFactory<OfflinedeviceEntity>, OfflinedeviceIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<OfflinedeviceEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public OfflinedeviceEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// 获取柱状图统计数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataTable GetTable(int type)
        {
            string sql = string.Format(
                @"select to_char(offlinedevice,'yyyy-mm') as offtime,count(id) as offcount from BIS_OFFLINEDEVICE where devicetype={1} and offlinedevice>=to_date('{0}-01-01 00:00:00','yyyy-mm-dd hh24:mi:ss') and offlinedevice<=to_date('{0}-12-31 23:59:59','yyyy-mm-dd hh24:mi:ss')  group by to_char(offlinedevice,'yyyy-mm') order by to_char(offlinedevice,'yyyy-mm')",
                DateTime.Now.Year, type);
            return BaseRepository().FindTable(sql);
        }

        /// <summary>
        /// 查询离线设备前几条
        /// </summary>
        /// <param name="type">设备类型 0标签 1基站 2门禁 3摄像头</param>
        /// <param name="Time">1本年 2本周</param>
        /// <param name="topNum">前几条</param>
        /// <returns></returns>
        public DataTable GetOffTop(int type, int Time,int topNum)
        {
            string idname = "deviceid";
            string starttime = "";
            string endtime = "";
            if (Time == 1)
            {
                starttime = DateTime.Now.Year + "-01-01 00:00:00";
                endtime = DateTime.Now.Year + "-12-31 23:59:59";
            }
            else
            {
                starttime = GetFirstdayTime();
                endtime = GetLastdayTime();
            }

            string sql = string.Format(@"select * from(select {0},count(od.id) as offcount from BIS_OFFLINEDEVICE  od
            where devicetype={1} and offlinedevice>=to_date('{2}','yyyy-mm-dd hh24:mi:ss') and offlinedevice<=to_date('{3}','yyyy-mm-dd hh24:mi:ss')  
            group by {0}  order by offcount desc) where rownum<={4} order by rownum desc", idname, type, starttime, endtime, topNum);

            return BaseRepository().FindTable(sql);
        }

        /// <summary>
        /// 获取本周第一天日期
        /// </summary>
        /// <returns></returns>
        public string GetFirstdayTime()
        {
            int week = Convert.ToInt32(DateTime.Now.DayOfWeek);
            week = (week == 0 ? (7 - 1) : (week - 1));
            int daydiff = (-1) * week;

            string FirstDay = DateTime.Now.AddDays(daydiff).ToString("yyyy-MM-dd HH:mm:ss");
            return FirstDay;
        }

        /// <summary>
        /// 获取本周第一天日期
        /// </summary>
        /// <returns></returns>
        public string GetLastdayTime()
        {
            int week = Convert.ToInt32(DateTime.Now.DayOfWeek);
            week = (week == 0 ? 7 : week);
            int daydiff = 7 - week;

            string Lastday = DateTime.Now.AddDays(daydiff).ToString("yyyy-MM-dd HH:mm:ss");
            return Lastday;
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
        public void SaveForm(string keyValue, OfflinedeviceEntity entity)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                this.BaseRepository().Update(entity);
            }
            else
            {
                this.BaseRepository().Insert(entity);
            }
        }
        #endregion
    }
}
