using ERCHTMS.Entity.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.CarManage
{
    /// <summary>
    /// �� ��������������¼��
    /// </summary>
    public interface CarinlogIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<CarinlogEntity> GetList(string Cid);

        /// <summary>
        /// ���ݳ��ƺŻ�ȡ���½�����Ϣ
        /// </summary>
        /// <param name="CarNo"></param>
        /// <returns></returns>
        CarinlogEntity GetNewCarinLog(string CarNo);

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        CarinlogEntity GetEntity(string keyValue);

        /// <summary>
        /// ��ȡ�б�����
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        DataTable GetTableChar(string year = "");

        /// <summary>
        /// ��ȡ�������볡��Ϣ
        /// </summary>
        /// <param name="year"></param>
        /// <param name="cartype"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        DataTable GetLogDetail(string year, string cartype, string status);

        /// <summary>
        /// ���ص�ǰ���ڳ�������
        /// </summary>
        /// <returns></returns>
        int GetLogNum();
        #endregion

        #region �ύ����
        /// <summary>
        /// ��ҳ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// ���ͨ����¼
        /// </summary>
        /// <param name="carlog"></param>
        void AddPassLog(CarinlogEntity carlog);
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        void SaveForm(string keyValue, CarinlogEntity entity);

        /// <summary>
        /// ��ȡ��������ͳ��ͼ
        /// </summary>
        /// <param name="deptCode">���ű���</param>
        /// <param name="year">ͳ�����</param>
        /// <returns></returns>
        string GetLogChart(string year = "");

        /// <summary>
        /// ͨ���ص����ͨ����¼
        /// </summary>
        /// <param name="carlog"></param>
        void BackAddPassLog(CarinlogEntity carlog, string DeviceName, string imgUrl);
        int[] GetCarData();
        int Insert(CarinlogEntity carlog);

        #endregion
    }
}
