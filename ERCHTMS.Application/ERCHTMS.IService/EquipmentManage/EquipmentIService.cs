using ERCHTMS.Entity.EquipmentManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.EquipmentManage
{
    /// <summary>
    /// �� ������ͨ�豸������Ϣ��
    /// </summary>
    public interface EquipmentIService
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
        IEnumerable<EquipmentEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        EquipmentEntity GetEntity(string keyValue);
        /// <summary>
        /// ��ȡ�豸���
        /// </summary>
        /// <param name="EquipmentNo">�豸���</param>
        /// <returns></returns>
        string GetEquipmentNo(string EquipmentNo, string orgcode);

        /// <summary> 
        /// ͨ���豸id��ȡ�û��б�
        /// </summary>
        /// <returns></returns>
        DataTable GetEquipmentTable(string[] ids);
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
        void SaveForm(string keyValue, EquipmentEntity entity);

        /// <summary>
        /// ��ͨ�豸�볡
        /// </summary>
        /// <param name="equipmentId">�û�Id</param>
        /// <param name="leaveTime">�볡ʱ��</param>
        /// <returns></returns>
        int SetLeave(string equipmentId, string leaveTime, string DepartureReason);
        #endregion
    }
}
