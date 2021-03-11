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
    /// �� �����豸���߼�¼
    /// </summary>
    public class OfflinedeviceService : RepositoryFactory<OfflinedeviceEntity>, OfflinedeviceIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<OfflinedeviceEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public OfflinedeviceEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }

        /// <summary>
        /// ��ȡ��״ͼͳ������
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
        /// ��ѯ�����豸ǰ����
        /// </summary>
        /// <param name="type">�豸���� 0��ǩ 1��վ 2�Ž� 3����ͷ</param>
        /// <param name="Time">1���� 2����</param>
        /// <param name="topNum">ǰ����</param>
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
        /// ��ȡ���ܵ�һ������
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
        /// ��ȡ���ܵ�һ������
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

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
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
