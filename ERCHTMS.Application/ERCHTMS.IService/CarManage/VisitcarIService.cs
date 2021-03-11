using ERCHTMS.Entity.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.MatterManage;

namespace ERCHTMS.IService.CarManage
{
    /// <summary>
    /// �� �����ݷó�����
    /// </summary>
    public interface VisitcarIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<VisitcarEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        VisitcarEntity GetEntity(string keyValue);

        /// <summary>
        /// ��ȡ�Ÿڲ�ѯ�����ϼ��ݷ���Ϣ
        /// </summary>
        /// <returns></returns>
        DataTable GetDoorList();

        /// <summary>
        /// ��ҳ��ѯ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);

        /// <summary>
        /// ���ݳ��ƺŻ�ȡ������Ϣ
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        VisitcarEntity GetCar(string CarNo);

        /// <summary>
        /// ���ݳ��ƺŻ�ȡ�˳��ƽ������°ݷ���Ϣ
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        VisitcarEntity NewGetCar(string CarNo);
        /// <summary>
        /// ��õ���������������
        /// </summary>
        /// <returns></returns>
        List<string> GetOutCarNum();

        /// <summary>
        /// ��ѯ�Ƿ����ظ����ƺŰݷó���/Σ��Ʒ����
        /// </summary>
        /// <param name="CarNo">���ƺ�</param>
        /// <param name="type">3λ�ݷ� 5ΪΣ��Ʒ</param>
        /// <returns></returns>
        bool GetVisitCf(string CarNo, int type);

        /// <summary>
        /// ��ʼ���ݷ�\Σ��Ʒ\���ϳ���
        /// </summary>
        /// <returns></returns>
        List<CarAlgorithmEntity> IniVHOCar();
        #endregion

        #region �ύ����

        /// <summary>
        /// ����ID�ı���ѡ·��
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="LineName"></param>
        /// <param name="LineID"></param>
        void ChangeLine(string keyValue, string LineName, string LineID);
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
        void SaveForm(string keyValue, VisitcarEntity entity);
        void SaveFaceUserForm(string keyValue, VisitcarEntity entity, List<CarUserFileImgEntity> userjson);

        /// <summary>
        /// �ı�GPS����Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="pgpslist"></param>
        void ChangeGps(string keyValue, VisitcarEntity entity, List<PersongpsEntity> pgpslist);

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="Note"></param>
        /// <param name="type"></param>
        void CarOut(string keyValue, string Note, int type, List<PersongpsEntity> pergps);

        /// <summary>
        /// �ı�GPS����Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="pgpslist"></param>
        void WlChangeGps(string keyValue, OperticketmanagerEntity entity);

        #endregion
    }
}
