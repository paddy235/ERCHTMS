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
    /// �� ��������������¼��
    /// </summary>
    public class CarinlogBLL
    {
        private CarinlogIService service = new CarinlogService();

        #region ��ȡ����
        /// <summary>
        /// ��ҳ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }

        /// <summary>
        /// ���ݳ��ƺŻ�ȡ���½�����Ϣ
        /// </summary>
        /// <param name="CarNo"></param>
        /// <returns></returns>
        public CarinlogEntity GetNewCarinLog(string CarNo)
        {
            return service.GetNewCarinLog(CarNo);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<CarinlogEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public CarinlogEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// ��ȡ��������ͳ��ͼ
        /// </summary>
        /// <param name="deptCode">���ű���</param>
        /// <param name="year">ͳ�����</param>
        /// <returns></returns>
        public string GetLogChart(string year = "")
        {
            return service.GetLogChart(year);
        }

        /// <summary>
        /// ��ȡ�б�����
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public DataTable GetTableChar(string year = "")
        {
            return service.GetTableChar(year);
        }

        /// <summary>
        /// ��ȡ�������볡��Ϣ
        /// </summary>
        /// <param name="year"></param>
        /// <param name="cartype"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public DataTable GetLogDetail(string year, string cartype, string status)
        {
            return service.GetLogDetail(year, cartype, status);
        }

        /// <summary>
        /// ���ص�ǰ���ڳ�������
        /// </summary>
        /// <returns></returns>
        public int GetLogNum()
        {
            return service.GetLogNum();
        }

        #endregion

        #region �ύ����

        /// <summary>
        /// ���ͨ����¼
        /// </summary>
        /// <param name="carlog"></param>
        public void AddPassLog(CarinlogEntity carlog)
        {
            service.AddPassLog(carlog);
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
        public void SaveForm(string keyValue, CarinlogEntity entity)
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
        /// ͨ���ص����ͨ����¼
        /// </summary>
        /// <param name="carlog"></param>
        public void BackAddPassLog(CarinlogEntity carlog, string DeviceName, string imgUrl)
        {
            service.BackAddPassLog(carlog,DeviceName,imgUrl);
        }

        public int[] GetCarData()
        {
            return service.GetCarData();
        }

        public int Insert(CarinlogEntity carlog)
        {
            return service.Insert(carlog);
        }

        #endregion
    }
}
