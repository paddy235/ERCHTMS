using BSFramework.Util.WebControl;
using System.Collections.Generic;
using ERCHTMS.Entity.JTSafetyCheck;
using System.Data;

namespace ERCHTMS.IService.JTSafetyCheck
{
    /// <summary>
    /// �� ��������ʲ�Ž�����
    /// </summary>
    public interface JTSafetyCheckIService
    {
        #region ��ȡ����

        /// <summary>
        /// ��ȡȫ���б�����
        /// </summary>
        /// <returns>���ط�ҳ�б�</returns>
        List<SafetyCheckEntity> GetPageList();
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<SafetyCheckEntity> GetList(string queryJson);
        DataTable GetItemsList(string checkId,string status);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        SafetyCheckEntity GetEntity(string keyValue);
        CheckItemsEntity GetItemEntity(string keyValue);
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
        bool SaveForm(string keyValue, SafetyCheckEntity entity);
        void SaveItemForm(string keyValue, CheckItemsEntity entity);
        /// <summary>
        /// �ӿ��޸�״̬�÷���
        /// </summary>
        /// <param name="entity"></param>
        void RemoveItemForm(string keyValue);
        bool Save(string keyValue, SafetyCheckEntity entity, List<CheckItemsEntity> items);
        bool SaveItems(List<CheckItemsEntity> items);

        #endregion
    }
}
