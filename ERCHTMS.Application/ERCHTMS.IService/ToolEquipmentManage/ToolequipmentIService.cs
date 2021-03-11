using ERCHTMS.Entity.ToolEquipmentManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.ToolEquipmentManage
{
    /// <summary>
    /// �� ���������߻�����Ϣ��
    /// </summary>
    public interface ToolequipmentIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<ToolequipmentEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        ToolequipmentEntity GetEntity(string keyValue);
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
        void SaveForm(string keyValue, ToolequipmentEntity entity);
        #endregion

        DataTable GetPageList(Pagination pagination, string queryJson);
        string GetEquipmentNo(string equipmentNo, string orgcode);
        string GetEquipmentTypeStat(string queryJson);

        DataTable GetToolRecordList(string keyValue);


        void SaveToolrecord(string keyValue, ToolrecordEntity entity);

        DataTable GetToolStatisticsList(string queryJson);

        object GetToolName(string tooltype);
    }
}
