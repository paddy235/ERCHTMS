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
    /// �� �����೵ԤԼ��¼
    /// </summary>
    public class CarreservationBLL
    {
        private CarreservationIService service = new CarreservationService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<CarreservationEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public CarreservationEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// ��ȡ��ǰ����ԤԼ��¼�б�
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
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }


        #endregion

        #region �ύ����

        /// <summary>
        /// ԤԼ/ȡ��ԤԼ
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
        /// ˾����Ӱ��
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="entity"></param>
        public void AddDriverCarInfo(string userid, CarreservationEntity entity)
        {
            service.AddDriverCarInfo(userid, entity);
        }


        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
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
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
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
