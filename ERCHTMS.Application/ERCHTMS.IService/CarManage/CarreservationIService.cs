using ERCHTMS.Entity.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.CarManage
{
    /// <summary>
    /// �� �����೵ԤԼ��¼
    /// </summary>
    public interface CarreservationIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<CarreservationEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        CarreservationEntity GetEntity(string keyValue);

        /// <summary>
        /// ��ȡ��ǰ����ԤԼ��¼�б�
        /// </summary>
        /// <returns></returns>
        DataTable GetCarReser(string userid);

        /// <summary>
        /// ԤԼ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);

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
        void AddReser(string userid, string cid, int time, string CarNo, int IsReser,string baseid);
        void AddDriverCarInfo(string userid, CarreservationEntity entity);
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
        void SaveForm(string keyValue, CarreservationEntity entity);
        #endregion
    }
}
