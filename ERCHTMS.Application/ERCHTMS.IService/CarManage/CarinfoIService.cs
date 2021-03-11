using ERCHTMS.Entity.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.CarManage
{
    /// <summary>
    /// �� ��������������Ϣ��
    /// </summary>
    public interface CarinfoIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<CarinfoEntity> GetList(string queryJson);

        /// <summary>
        /// ��ȡ¼�복����
        /// </summary>
        /// <returns></returns>
        List<CarinfoEntity> GetGspCar();
        /// <summary>
        /// ���ƺ��Ƿ����ظ�
        /// </summary>
        /// <param name="CarNo"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        bool GetCarNoIsRepeat(string CarNo, string id);
        /// <summary>
        /// ���ݳ��ƺŻ�ȡ������Ϣ
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        CarinfoEntity GetBusCar(string CarNo);

        /// <summary>
        /// ���ݳ��ƺŻ�ȡ������Ϣ
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        CarinfoEntity GetCar(string CarNo);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        CarinfoEntity GetUserCar(string userid);
        /// <summary>
        /// �û��б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        CarinfoEntity GetEntity(string keyValue);
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        void RemoveForm(string keyValue, string IP, int Port);
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        void SaveForm(string keyValue, CarinfoEntity entity, string pitem, string url, string IP, int Port);
        //void SaveForm(string keyValue, CarinfoEntity entity, string pitem, string url);
        void SaveForm(string keyValue, CarinfoEntity entity);

        void CartoExamine(string keyValue, CarinfoEntity entity);
        /// <summary>
        /// �޸ĺ���������Ϣ
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="NewCar"></param>
        void UpdateHiaKangCar(CarinfoEntity entity, string OldCar);

        #endregion
    }
}
