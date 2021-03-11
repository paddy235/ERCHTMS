using ERCHTMS.Entity.EquipmentManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.EquipmentManage
{
    /// <summary>
    /// �� ���������豸������Ϣ��
    /// </summary>
    public interface SpecialEquipmentIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<SpecialEquipmentEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        SpecialEquipmentEntity GetEntity(string keyValue);
        /// <summary>
        /// ��ȡ�豸���
        /// </summary>
        /// <param name="EquipmentNo">�豸���</param>
        /// <returns></returns>
        string GetEquipmentNo(string EquipmentNo, string orgcode);

        /// <summary>
        /// ��ȡ�豸���ͳ��ͼ���б�
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetEquipmentTypeStat(string queryJson, IEnumerable<SpecialEquipmentEntity> se);

        /// <summary>
        /// ��ȡ�豸���й���ͳ��ͼ���б�
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        object GetOperationFailureStat(string queryJson, IEnumerable<SpecialEquipmentEntity> se);
        /// <summary>
        /// ����Id��ȡ�����豸����ͨ�豸��Ϣ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DataTable GetEquimentList(string id);
        #endregion

        #region �ύ����
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
        void SaveForm(string keyValue, SpecialEquipmentEntity entity);

        /// <summary>
        /// �����豸�볡
        /// </summary>
        /// <param name="equipmentId">�û�Id</param>
        /// <param name="leaveTime">�볡ʱ��</param>
        /// <returns></returns>
        int SetLeave(string specialequipmentId, string leaveTime, string DepartureReason);

        /// <summary>
        /// �����豸�����޸ļ�������
        /// </summary>
        /// <param name="specialequipmentId"></param>
        /// <param name="CheckDate"></param>
        /// <returns></returns>
        int SetCheck(string specialequipmentId, string CheckDate);
        #endregion


        #region ��ȡʡ��ͳ������
        /// <summary>
        /// ��ȡʡ���豸���ͳ��
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetEquipmentTypeStatGridForSJ(string queryJson);

        /// <summary>
        /// ��ȡʡ���豸���ͼ��
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        string GetEquipmentTypeStatDataForSJ(string queryJson);

        /// <summary>
        /// ��ȡʡ����������ͼ��
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        string GetEquipmentHidDataForSJ(string queryJson);

        /// <summary>
        /// ��ȡʡ�������������
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetEquipmentHidGridForSJ(string queryJson);

        /// <summary>
        /// ��ȡʡ����ȫ����б�
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetEquipmentCheckGridForSJ(string queryJson);


        /// <summary>
        /// ��ȡʡ��������ͼ��
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        string GetEquipmentCheckDataForSJ(string queryJson);


        /// <summary>
        /// ��ȡʡ�����й����б�
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetEquipmentFailureGridForSJ(string queryJson);

        /// <summary>
        /// ��ȡʡ�����й���ͼ��
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        string GetEquipmentFailureDataForSJ(string queryJson);

        /// <summary>
        /// ��ȡʡ����������¼
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetSafetyCheckRecordForSJ(string queryJson);

        /// <summary> 
        /// ͨ���豸id��ȡ�����豸
        /// </summary>
        /// <returns></returns>
        DataTable GetSpecialEquipmentTable(string[] ids);
        #endregion
        #region app�ӿ�
        DataTable SelectData(string sql);
        int UpdateData(string sql);
        #endregion
    }
}
